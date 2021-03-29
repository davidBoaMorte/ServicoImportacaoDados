using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace CamadaDAL
{

    public class INFOExmeplo
    {
        public string Nome { get; set; }
        public int IDUsuario { get; set; }

        public string Senha { get; set; }

    }

    public class DALExemplo : DALBase
    {
        //public INFOExmeplo DbObterFoto(long pIDprocesso)
        //{

        //    StringBuilder sql = new StringBuilder();
        //    sql.AppendLine(" SELECT [FOT_BLO_IMG_FOTO] as Imagem_Foto");
        //    sql.AppendLine("       ,[FOT_BOL_FL_RECENTE] as Foto_Recente");
        //    sql.AppendLine(" FROM [ID_FOTO] ");
        //    sql.AppendLine(" WHERE [PRO_INT_ID_PROCESSO] = @pIDprocesso");

        //    /// Devido ao framework não funciona
        //   // return cn.ExecQueryFirstOrDefault<INFOExmeplo>(sql.ToString(), new { pIDprocesso });


        //}


        public void DbInserirFoto(INFOExmeplo pExemplo)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(" INSERT INTO [ID_FOTO] ");
            sql.AppendLine(" ([PRO_INT_ID_PROCESSO] ");
            sql.AppendLine(" ,[FOT_BLO_IMG_FOTO] ");
            sql.AppendLine(" ,[FOT_BOL_FL_RECENTE]) ");
            sql.AppendLine(" VALUES ");
            sql.AppendLine(" (@Nome");
            sql.AppendLine(" ,@Senha");
            sql.AppendLine(" ,@IDUsuario) ");

            cn.ExecNonQuery(sql.ToString(), new
            {
                Nome = pExemplo.Nome,
                IDUsuario = pExemplo.IDUsuario,
                Senha = pExemplo.Senha
            });
        }


        public void DbAtualizarDigital(int pIdCidadao, INFOExmeplo pExemplo)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine(" UPDATE [ID_DIGITAIS] ");
            sql.AppendLine(" SET [DIG_DAT_DT_ATUALIZACAO] = GETDATE(),   ");
            sql.AppendLine("     [DIA_INT_ID_ANOMALIA] = @IDANOMALIA,            ");
            sql.AppendLine("     [DIG_STR_DS_OBSERVACAO] =  @Observacao,         ");
            sql.AppendLine("     [DIG_BLO_IMG_DIGITAL] = @Imagem_Digital          ");
            sql.AppendLine(" WHERE [CID_INT_ID_CIDADAO] = @pIdCidadao             ");
            sql.AppendLine(" AND [DIG_INT_ID_DIGITAL] = @ID_Dedo                  ");

            cn.ExecNonQuery(sql.ToString(), new
            {
                //  parametros
                //IDANOMALIA = pDigital.Anomalia.ID_Anomalia,
                //Observacao = pDigital.Observacao,
                //Imagem_Digital = pDigital.Imagem_Digital,
                //pIdCidadao,
                //ID_Dedo = pDigital.ID_Dedo
            });

        }
    }
}
