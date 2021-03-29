using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using System.Data;
using CamadaINFO;

namespace CamadaDAL
{
    public class ValidConnection : IDisposable
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["IDMA_ConnectionString"].ConnectionString;
        private CultureInfo _provider = new CultureInfo("pt-BR");
        public static byte[] Key = { 124, 222, 121, 82, 172, 21, 185, 111, 228, 182, 72, 132, 233, 123, 80, 12 };
        public static byte[] IV = { 172, 111, 13, 42, 244, 102, 81, 211 };
        private static ValidConnection _validConnection = null;

        public static ValidConnection Cn
        {
            get
            {
                if (_validConnection == null)
                {
                    _validConnection = new ValidConnection();

                }

                return _validConnection;
            }
        }

        public string StringConexao
        {
            get
            {
                UTILITY.Cryptografia.IV = IV;
                UTILITY.Cryptografia.Key = Key;
                return UTILITY.Cryptografia.DecryptString(connectionString);
            }
            set { connectionString = value; }
        }


        public CultureInfo Provider
        {
            get { return _provider; }
        }

        private SqlConnection cn = null;



        public SqlTransaction Trans { get; set; } = null;

        public SqlDataReader ExecReader(string sql, SqlParameter[] parameters = null, CommandType commandType = CommandType.Text)
        {
            SqlDataReader reader = null;

            try
            {
                OpenConnection();

                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandText = sql;
                    command.CommandType = commandType;
                    command.Connection = cn;
                    command.Transaction = Trans;

                    if (parameters != null && parameters.Length > 0)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    reader = command.ExecuteReader();
                }

                return reader;
            }
            catch
            {
                CloseReader(reader);
                throw;
            }
        }
        public int ExecNonQuery(string sql, IDbDataParameter[] parameters = null, CommandType commandType = CommandType.Text)
        {
            try
            {
                OpenConnection();

                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandText = sql;
                    command.CommandType = commandType;
                    command.Connection = cn;
                    command.Transaction = Trans;

                    if (parameters != null && parameters.Length > 0)
                    {
                        foreach (IDbDataParameter param in parameters)
                        {
                            command.Parameters.Add(param);
                        }
                    }
                    return command.ExecuteNonQuery();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        public int ExecNonQuery<T>(string sql, T entity, CommandType commandType = CommandType.Text) where T : class
        {
            try
            {
                OpenConnection();
                return cn.Execute(sql, entity, Trans, null, commandType);
            }
            catch
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }
        public IEnumerable<T> ExecQuery<T>(string sql, object parameters = null, CommandType commandType = CommandType.Text) where T : class
        {
            try
            {
                OpenConnection();
                return cn.Query<T>(sql, parameters, Trans, true, null, commandType);
            }
            catch
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        public object ExecuteScalar(string sql, object parameters = null, CommandType commandType = CommandType.Text)
        {
            try
            {
                OpenConnection();
                return cn.ExecuteScalar(sql, parameters, Trans, null, commandType);
            }
            catch
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// Realizar o Insert do objeto no banco de dados
        /// </summary>
        /// <typeparam name="T">INFO</typeparam>
        /// <param name="entity">INFO que será inserida no banco de dados</param>
        public void ExecuteInsert<T>(T entity) where T : IINFO
        {
            try
            {
                string sql = CreateCommandInsert(entity);
                OpenConnection();
                entity.Id = (int)cn.ExecuteScalar(sql, entity, Trans, null, CommandType.Text);
            }
            catch
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// Realizar o Update do objeto no banco de dados
        /// </summary>
        /// <typeparam name="T">INFO</typeparam>
        /// <param name="entity">INFO que será inserida no banco de dados</param>
        public void ExecuteUpdate<T>(T entity) where T : IINFO
        {
            try
            {
                string sql = CreateCommandUpdate(entity);
                ExecNonQuery<IINFO>(sql, entity);
            }
            catch
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }
        private string[] GetProperties(Type type)
        {
            string[] listColumns = type.GetProperties().ToList()
                                       .Where(x => x.Name != "Id")
                                       .Select(x => x.Name)
                                       .ToArray();
            return listColumns;
        }

        private string CreateCommandInsert<T>(T entity) where T : IINFO
        {
            var entityType = entity.GetType();
            StringBuilder sql = new StringBuilder();
            StringBuilder columns = new StringBuilder();
            StringBuilder parameters = new StringBuilder();

            string[] listColumns = GetProperties(entityType);

            for (int i = 0; i < listColumns.Length; i++)
            {
                if (i > 0)
                {
                    columns.AppendLine($"    ,{listColumns[i]}");
                    parameters.AppendLine($"    ,@{listColumns[i]}");
                }
                else
                {
                    columns.AppendLine($"    {listColumns[i]}");
                    parameters.AppendLine($"    @{listColumns[i]}");
                }

            }

            sql.AppendLine($"INSERT INTO {entityType.Name}");
            sql.AppendLine("(");
            sql.AppendLine(columns.ToString());
            sql.AppendLine(") VALUES");
            sql.AppendLine("(");
            sql.AppendLine(parameters.ToString());
            sql.AppendLine(")");
            sql.AppendLine(";SELECT ISNULL(CAST(SCOPE_IDENTITY() AS INT), 0) AS Id");

            return sql.ToString();
        }

        private string CreateCommandUpdate<T>(T entity) where T : IINFO
        {
            var entityType = entity.GetType();
            StringBuilder sql = new StringBuilder();
            StringBuilder columns = new StringBuilder();

            string[] listColumns = GetProperties(entityType);

            for (int i = 0; i < listColumns.Length; i++)
            {
                if (i > 0)
                {
                    columns.AppendLine($"    ,{listColumns[i]} = @{listColumns[i]}");
                }
                else
                {
                    columns.AppendLine($"    {listColumns[i]} = @{listColumns[i]}");
                }
            }

            sql.AppendLine($"UPDATE {entityType.Name} SET");
            sql.AppendLine(columns.ToString());
            sql.AppendLine($"WHERE  Id = @Id");

            return sql.ToString();
        }
        /// <summary>
        /// Método não é compatível como framework da class DAL
        /// </summary>
        //public T ExecQueryFirstOrDefault<T>(string sql, object parameters = null, CommandType commandType = CommandType.Text) where T : class
        //{
        //    try
        //    {
        //        OpenConnection();
        //        return cn.QueryFirstOrDefault<T>(sql, parameters, Trans, null, commandType);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        CloseConnection();
        //    }
        //}

        protected void OpenConnection()
        {
            try
            {
                if (cn == null)
                {
                    cn = new SqlConnection(StringConexao);
                    cn.Open();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Não foi possível se conectar ao banco de dados: {ex.Message}");
            }
        }

        public void BeginTransaction()
        {
            if (Trans != null)
                return;

            OpenConnection();
            Trans = cn.BeginTransaction();
        }

        public void CommitTransaction()
        {
            Trans.Commit();
            Trans.Dispose();
            Trans = null;
            CloseConnection();
        }

        public void RollbackTransaction()
        {
            try
            {
                if (Trans == null)
                    return;

                Trans.Rollback();
                Trans.Dispose();
                Trans = null;
            }
            catch
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        protected void CloseConnection()
        {
            try
            {
                if (Trans == null)
                {
                    if (cn != null)
                    {
                        cn.Close();
                        cn.Dispose();
                        cn = null;
                    }
                }
            }
            finally
            {
            }
        }

        public void CloseReader(IDataReader reader)
        {
            try
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            finally
            {
                CloseConnection();
            }
        }

        public void Clear()
        {
            if (_validConnection != null)
            {
                _validConnection.Dispose();
                _validConnection = null;
            }
        }

        public void Dispose()
        {
            RollbackTransaction();
        }
    }
}
