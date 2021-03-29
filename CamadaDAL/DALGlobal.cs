using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilidades;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics.SymbolStore;

namespace CamadaDAL
{
    public class DALGlobal
    {

        /// <summary>
        /// Connection String do Banco.
        /// </summary>
        public static string SQLConnectionString => Settings.ObterStringConexao<string>("SQLConnectionString");

    }
}

