using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Utilidades
{
    public static class Settings
    {
        public static T Convert<T>(this string input)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter != null)
                {
                    // Cast ConvertFromString(string text) @ object to (T)
                    return (T)converter.ConvertFromString(input);
                }
                return default(T);
            }
            catch (NotSupportedException)
            {
                return default(T);
            }
        }

        public static T RecuperarConfiguracao<T>(string configKey)
        {
            var valorConfigurado = ConfigurationManager.AppSettings[configKey].ToString();
            if (string.IsNullOrEmpty(valorConfigurado))
                throw new ArgumentException("A chave {0} do arquivo de configuração não existe ou não foi preenchida.");

            var valorConvertido = Convert<T>(valorConfigurado);

            if (EqualityComparer<T>.Default.Equals(valorConvertido, default(T)))
                throw new ArgumentException("A chave {0} do arquivo de configuração não possui o tipo de dado correto.");

            return valorConvertido;
        }


        public static T ObterStringConexao<T>(string configKey)
        {
            string stringConexaoCriptografada = RecuperarConfiguracao<string>(configKey);

            T stringConexaoDescriptografada = Convert < T > (Cryptografia.DecryptString(stringConexaoCriptografada));

            return stringConexaoDescriptografada;
        }
    }
}
