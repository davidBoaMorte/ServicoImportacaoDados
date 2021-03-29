using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.IO;

namespace Utilidades
{
    public class Log
    {
        private string _pasta;
        private string _arquivo;
        private string _nomeArquivo;
        private CultureInfo _provider = new CultureInfo("pt-BR");

        public enum TIPO
        {
            SUCESSO = 1,
            ERRO = 2,
            AVISO = 3
        }

        public Log(string nomeArquivo, string pasta)
        {
            if (! Directory.Exists (pasta)) {
                CriarPastas(pasta);
            }

            _pasta = pasta;
            _nomeArquivo = UltimoArquivo(_pasta, nomeArquivo);
        }

        private bool CriarPastas(string pCaminho) {
            string[] pastas;
            string caminho = string.Empty;

            try
            {
                pastas = pCaminho.Split('\\');
                caminho = pastas[0];

                if (!VerificarExistenciaRaiz(pastas)) {
                    throw new Exception("A raiz do diretorio informado não existe!");
                }

                for (int i = 1; i < pastas.Length; i++)
                {               
                    caminho += string.Concat ('\\',pastas[i]);
                    if (!Directory.Exists(caminho)){
                        Directory.CreateDirectory(caminho);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception (String.Format("Ocorreu um erro ao salvar o caminho {0}, {1}", pCaminho, ex.Message));
            }
        }

        private bool VerificarExistenciaRaiz(string[] pDiretorio) {
            string raiz = pDiretorio[0];

            return Directory.Exists(raiz);
        }

        private string UltimoArquivo(string pasta, string nomeArquivo)
        {
            /*Essa rotina retorna o último arquivo do dia, pois ao atingir 50MB deve ser criado outro na sequência*/
            int UltimoIndice = 1;
            string ArquivoLog = nomeArquivo;
            DirectoryInfo diretorio = new DirectoryInfo(pasta); 
            foreach (FileInfo f in diretorio.GetFiles())
            {
                if (f.Name.Substring(0,38) == nomeArquivo.Substring(0, 38))
                {
                    string indice = nomeArquivo.Remove(0,39);
                    indice = indice.Replace(".log", "");
                    ArquivoLog = f.ToString();
                    if (UltimoIndice < Convert.ToInt16(indice))
                    {
                        UltimoIndice = Convert.ToInt16(indice);
                        ArquivoLog = nomeArquivo.Substring(0, 33) + "_parte" + indice;
                    }                        
                }
            }
            return ArquivoLog;
        }

        public void Gravar(DateTime data, string mensagem, TIPO tipoLog)
        {
            StreamWriter swrite;
            string linha = "";

            try
            {
                // Verifico se a pasta existe, se não existir crio
                if (!Directory.Exists(_pasta))
                {
                    Directory.CreateDirectory(_pasta);
                }

                //Nome do arquivo
                if (string.IsNullOrEmpty(_nomeArquivo)) _nomeArquivo = DateTime.Now.ToString("dd-MM-yyyy") + "_ServicoImportacaoDados_parte1.log";
                
                //Achar o ultimo arquivo desse dia
                _nomeArquivo = UltimoArquivo(_pasta, _nomeArquivo);

                //Existe arquivo?
                if (!System.IO.File.Exists(Path.Combine(_pasta, _nomeArquivo)))
                    System.IO.File.Create(Path.Combine(_pasta, _nomeArquivo)).Close();

                //Informações do arquivo
                var infoFile = new System.IO.FileInfo(Path.Combine(_pasta, _nomeArquivo));

                //Validando tamanho do Arquivo de Log - 50MB
                if (infoFile.Length >= 50000000)
                {
                    string numero = _nomeArquivo.Remove(0, 39);
                    numero = numero.Replace(".log","");
                    int indice = Convert.ToInt16(numero);
                    indice = indice + 1;
                    numero = Convert.ToString(indice).PadLeft(4,'0');
                    _nomeArquivo = DateTime.Now.ToString("dd-MM-yyyy") + "_ServicoImportacaoDados_parte" + numero + ".log";
                }


                // Monto o nome do arquivo
                _arquivo = _pasta + "\\" + _nomeArquivo;

                swrite = new StreamWriter(_arquivo, true, Encoding.GetEncoding(1252));

                if (tipoLog == TIPO.SUCESSO)
                {
                    linha = "[SUCESSO] ";
                }
                else if (tipoLog == TIPO.ERRO)
                {
                    linha = "[ERRO]    ";
                }
                else if (tipoLog == TIPO.AVISO)
                {
                    linha = "[AVISO]   ";
                }

                //linha += data.ToString("dd/MM/yyyy HH@mm@ss", _provider) + " --> " + mensagem;

                //ex@	17/01/2019 10@20@22 [SUCESSO] -> Arquivo de carga [nome do arquivo] foi carregado com sucesso
                swrite.WriteLine(string.Format("{0} {1} {2}", data.ToString("dd/MM/yyyy HH:mm:ss", _provider), linha, " --> " + mensagem));

                swrite.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

