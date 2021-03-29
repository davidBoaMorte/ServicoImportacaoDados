using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilidades;

namespace CamadaBLL
{
    public static class BLLGlobal
    {

        /// <summary>
        /// Diretório de gravação do Log.
        /// </summary>
        public static string PathLog => Settings.RecuperarConfiguracao<string>("PATH_LOG");
        public static string DiretorioArquivosRID => Settings.RecuperarConfiguracao<string>("PATH_ARQUIVOS");
        public static string DiretorioArquivosProcessados => Settings.RecuperarConfiguracao<string>("PATH_PROCESSADOS");
        public static string DiretorioArquivosProcessadosErro => Settings.RecuperarConfiguracao<string>("PATH_PROCESSADOS_ERRO");
        public static string AguardarProcessamento => Settings.RecuperarConfiguracao<string>("Intervalo");
        public static string HorarioInicioProcessamento => ValidarHorario<string>(Settings.RecuperarConfiguracao<string>("HorarioInicio"));
        public static string HorarioTerminoProcessamento => ValidarHorario<string>(Settings.RecuperarConfiguracao<string>("HorarioTermino"));
        public static string TipoArquivo => Settings.RecuperarConfiguracao<string>("TipoArquivo");



        #region Métodos públicos

        public static int ConverterTempo(string AguardarProcessamento)
        {
            try
            {
                var tempo = TimeSpan.Parse(AguardarProcessamento);
                return int.Parse(tempo.TotalMilliseconds.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Métodos privados

        private static T ValidarHorario<T>(string configKey)
        {
            var valorConfigurado = "";

            try
            {
                if (ConverterTempo(configKey) <= 0)
                    valorConfigurado = "00:00:00";
                else
                    valorConfigurado = configKey;

            }
            catch (Exception)
            {
                valorConfigurado = "00:00:00";
            }

            var valorConvertido = Settings.Convert<T>(valorConfigurado);
            return valorConvertido;
        }
        #endregion
    }
}
