using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dapper;
using CamadaINFO;
using System.Data.Common;

namespace CamadaDAL
{
    public class DALTipo2_Carga 
    {
        public void dbAtualizarTipo2_Carga(InfoArquivoRID registro)
        {
            try
            {
                using (var conn = new SqlConnection(DALGlobal.SQLConnectionString))
                {
                    conn.Open();

                    var query = "";

                              
                    var result = conn.Execute(query, new
                    {
                       
                     
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void dbInserirTipo2_Carga(InfoArquivoRID registro)
        {
            try
            {
                using (var conn = new SqlConnection(DALGlobal.SQLConnectionString))
                {
                    conn.Open();



                    var query = "";

                    var result = conn.Execute(query, new
                    {
                      
                      

                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
