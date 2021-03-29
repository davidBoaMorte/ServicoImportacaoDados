using System;
using System.Collections.Generic;
using System.Linq;
using CamadaINFO;
using CamadaDAL;
using System.Transactions;
using System.IO;

namespace CamadaBLL {
    public class BLLImportarDados {
        Utilidades.Log _log = null;
        string nomeArquivo = string.Empty;

        public void Processar()
        {
            //System.Threading.Thread.Sleep(BLLGlobal.ConverterTempo("00:00:10"));
            int qtdRegistro = 0;

            //Criando arquivo de Log correto
            _log = new Utilidades.Log(DateTime.Now.ToString("dd-MM-yyyy") + "_ServicoImportacaoDados_parte1.log", BLLGlobal.PathLog);

            try
            {
                var qtdArquivos = ArquivosRIDLocalizados();

                //Processando cada arquivo encontrado
                if (qtdArquivos != null && qtdArquivos.Length > 0)
                    foreach (var arquivoRID in qtdArquivos)
                    {
                        //Nome do arquivo
                        nomeArquivo = new System.IO.FileInfo(arquivoRID)?.Name;

                        //Validando registros do arquivo encontrado
                        var listaRegistros = LerConteudoArquivoRID(arquivoRID);

                        //Quantidade de registros retornados do arquivo
                        if (listaRegistros != null && listaRegistros.Count > 0)
                        {
                            //Iniciando processamento

                            //Inserindo Logs de processamento
                            _log.Gravar(DateTime.Now,
                                string.Format("Arquivo {0} foi carregado com sucesso.",
                                nomeArquivo), Utilidades.Log.TIPO.SUCESSO);

                            _log.Gravar(DateTime.Now,
                                string.Format("Arquivo {0} possuem {1} registros para serem inseridos.",
                                nomeArquivo, listaRegistros.Count().ToString()), Utilidades.Log.TIPO.SUCESSO);

                            _log.Gravar(DateTime.Now,
                                string.Format("Iniciando a inserção/alteração dos registros do arquivo {0}.",
                                nomeArquivo), Utilidades.Log.TIPO.SUCESSO);

                            try
                            {
                                //Iniciando transacao
                                using (var trans = new TransactionScope())
                                {
                                    //Inserindo registro no banco, caso já exista o mesmo tem seu conteúdo atualizado
                                    foreach (var registro in listaRegistros)
                                    {
                                        if (registro.ErroProcessamento == 1)
                                        {
                                            _log.Gravar(DateTime.Now, string.Format(
                                               " CARTA{0} - Erro no processamento do registro.",
                                               registro.NomeCarta),
                                               Utilidades.Log.TIPO.ERRO);
                                        }
                                        else
                                        {
                                           

                                           
                                           
                                        }
                                    }

                                    //Commitando dados
                                    trans.Complete();
                                }
                            }
                            catch (Exception ex)
                            {
                                //Erro no processamento, desfez a transação e inseriu o log do erro no arquivo txt
                                _log.Gravar(DateTime.Now, ex.Message, Utilidades.Log.TIPO.ERRO);
                                qtdRegistro = 0;

                                _log.Gravar(DateTime.Now,
                                    string.Format("Arquivo {0} removido para a pasta de erro.",
                                    nomeArquivo), Utilidades.Log.TIPO.ERRO);

                                MoverFolderErro(nomeArquivo);
                            }

                            _log.Gravar(DateTime.Now,
                                string.Format("Total de {0} Registros inseridos do arquivo de carga {1}.",
                                qtdRegistro, arquivoRID), Utilidades.Log.TIPO.SUCESSO);

                            MoverFolderSucesso(nomeArquivo);
                        }
                        else
                        {
                            //Erro no processamento, desfez a transação e inseriu o log do erro no arquivo txt
                            _log.Gravar(DateTime.Now,
                                string.Format("Erro no processamento do arquivo {0}.",
                                nomeArquivo), Utilidades.Log.TIPO.ERRO);

                            _log.Gravar(DateTime.Now,
                                string.Format("Arquivo {0} removido para a pasta de erro.",
                                nomeArquivo), Utilidades.Log.TIPO.ERRO);

                            MoverFolderErro(nomeArquivo);
                        }
                    }

            }
            catch (Exception ex)
            {
                _log.Gravar(DateTime.Now, ex.Message, Utilidades.Log.TIPO.ERRO);
            }
        }      

        #region "Receber Arquivo"

     

    
        #endregion

        #region "Operações Carga"
        private void busInserirTipo2_Carga(InfoArquivoRID registro)
        {
            try
            {
                DALTipo2_Carga dal = new DALTipo2_Carga();
                dal.dbInserirTipo2_Carga(registro);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void busAtualizarTipo2_Carga(InfoArquivoRID registro)
        {
            try
            {
                DALTipo2_Carga dal = new DALTipo2_Carga();
                dal.dbAtualizarTipo2_Carga(registro);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region "Operações 
        private void busInserirProcessamento(InfoArquivoRID registro)
        {
            try
            {
               

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void busAtualizarProcessamento(InfoArquivoRID registro)
        {
            try
            {
              

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region "Metodos Pastas"
        private void MoverFolderErro(string nomeArquivo)
        {
            try
            {
                // Caminho do diretorio a ser criado
                string folderError = BLLGlobal.DiretorioArquivosProcessadosErro;

                // Verificar existencia do diretorio
                if (!Directory.Exists(folderError))
                {
                    // Criando o diretorio caso não exista
                    Directory.CreateDirectory(folderError);
                }

                // Movendo o arquivo entre os diretorios
                string sourceFileError = BLLGlobal.DiretorioArquivosRID + "\\" + nomeArquivo;
                string destinationFileError = BLLGlobal.DiretorioArquivosProcessadosErro + "\\" + nomeArquivo;
                System.IO.File.Move(sourceFileError, destinationFileError);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MoverFolderSucesso(string nomeArquivo)
        {
            try
            {
                // Caminho do diretorio a ser criado
                string folder = BLLGlobal.DiretorioArquivosProcessados;

                // Verificar existencia do diretorio
                if (!Directory.Exists(folder))
                {
                    // Criando o diretorio caso não exista
                    Directory.CreateDirectory(folder);
                }

                // Movendo o arquivo entre os diretorios
                string sourceFile = BLLGlobal.DiretorioArquivosRID + "\\" + nomeArquivo;
                string destinationFile = BLLGlobal.DiretorioArquivosProcessados + "\\" + nomeArquivo;
                System.IO.File.Move(sourceFile, destinationFile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region "Metodos Arquivos"
        private string[] ArquivosRIDLocalizados()
        {
            
            try
            {
                //Extensão do arquivo
                string TipoArquivo = BLLGlobal.TipoArquivo;
                return System.IO.Directory.GetFiles(BLLGlobal.DiretorioArquivosRID, TipoArquivo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<InfoArquivoRID> LerConteudoArquivoRID(string arquivoRID)
        {
            List<InfoArquivoRID> lstRegistros = null;
            InfoArquivoRID objRegistro = null;
            string[] ConteudoArquivo = null;

            try
            {
                //Lista com os dados a serem processados
                lstRegistros = new List<InfoArquivoRID>();

                //Leio o conteúdo do arquivo
                ConteudoArquivo = System.IO.File.ReadAllLines(arquivoRID, System.Text.Encoding.GetEncoding("utf-8"));

                //Verifico se existe erro de encoding na acentuação do arquivo que não resolveu no UTF-8 e aplicando ISO-8859-1
                foreach (var item in ConteudoArquivo)
                    if (item.Contains("?") || item.Contains("�"))
                    {
                        ConteudoArquivo = System.IO.File.ReadAllLines(arquivoRID, System.Text.Encoding.GetEncoding("iso-8859-1"));
                        break;
                    }

                for (int i = 0; i < ConteudoArquivo.Length - 1; i++)
                    if (!string.IsNullOrEmpty(ConteudoArquivo[i]))
                    {
                        objRegistro = new InfoArquivoRID();
                        //Validando os dados do arquivo
                        objRegistro = RetornarDadosLinhaArquivo(ConteudoArquivo);
                        lstRegistros.Add(objRegistro);

                    }

            }
            catch (Exception ex)
            {
                _log.Gravar(DateTime.Now, ex.Message, Utilidades.Log.TIPO.ERRO);
                lstRegistros = null;
            }
            finally
            {
                objRegistro = null;
                ConteudoArquivo = null;
            }
            return lstRegistros;
        }

        private InfoArquivoRID RetornarDadosLinhaArquivo(string[] linha)
        {
            InfoArquivoRID registro = null;

            try
            {

                registro = new InfoArquivoRID();
                registro.ErroProcessamento = 0;
                registro.DiretorioCartaGerada = linha[0];
                registro.NomeCarta = linha[1];

                //Verificando se existe o item TEXTO 0011
                //foreach (var item in linha)
                //{
                //    if (item.Contains("DVG_TEXTO_0011"))
                //    {
                //        registro = new InfoArquivoRID();
                //        registro.DVG_TEXTO_0011 = item.Remove(0, 15);
                //    }
                //}


                //CORPO DA CARTA
                registro.DVG_TEXTO_0011 = linha[8].Remove(0,15); // nome do cidadão
                registro.DVG_TEXTO_0012 = linha[9].Remove(0, 15); // endereço
                registro.DVG_TEXTO_0013 = linha[10].Remove(0, 15); // endereço
                registro.DVG_TEXTO_0015 = linha[11].Remove(0, 15);
                registro.DVG_TEXTO_0017 = linha[12].Remove(0, 15);
                registro.DVG_TEXTO_0019 = linha[13].Remove(0, 15);
                registro.DVG_TEXTO_0022 = linha[14].Remove(0, 15);
                registro.DVG_TEXTO_0024 = linha[15].Remove(0, 15) + "  " + linha[16].Remove(0, 15);
                registro.DVG_TEXTO_0025 = linha[17].Remove(0, 15) + " "  + linha[18].Remove(0, 15);
                registro.DVG_TEXTO_0026 = linha[19].Remove(0, 15);
                registro.DVG_TEXTO_0028 = linha[20].Remove(0, 15);
                registro.DVG_TEXTO_0029 = linha[21].Remove(0, 15);
                registro.DVG_TEXTO_0031 = linha[22].Remove(0, 15) + " " + linha[23].Remove(0, 15);
                registro.DVG_TEXTO_0032 = linha[24].Remove(0, 15) + " " + linha[25].Remove(0, 15);
                registro.DVG_TEXTO_0033 = linha[26].Remove(0, 15);
                registro.DVG_TEXTO_0035 = linha[27].Remove(0, 15) + " " + linha[28].Remove(0, 15);
                registro.DVG_TEXTO_0036 = linha[29].Remove(0, 15) + " " + linha[30].Remove(0, 15);
                registro.DVG_TEXTO_0037 = linha[31].Remove(0, 15);
                registro.DVG_TEXTO_0039 = linha[32].Remove(0, 15) + " " + linha[33].Remove(0, 15);
                registro.DVG_TEXTO_0040 = linha[34].Remove(0, 15);
                registro.DVG_TEXTO_0043 = linha[35].Remove(0, 15);
                registro.DVG_TEXTO_0044 = linha[36].Remove(0, 15);
                registro.DVG_TEXTO_0047 = linha[37].Remove(0, 15) + " " + linha[38].Remove(0, 15);

                // TOPO DA CARTA
                registro.DGV_NAME_TOPO = linha[41].Remove(0,8);
                registro.DY_ADDR_LINES_LINE1 = linha[42].Remove(0,19);
                registro.DY_ADDR_LINES_LINE2 = linha[43].Remove(0,19);
                registro.DY_ADDR_LINES_LINE3 = linha[44].Remove(0,19);

                //VERSO DA CARTA
                registro.DGV_NAME_VERSO = linha[51].Remove(0,8);
                registro.DGV_END_ENT1 = linha[52].Remove(0,12);
                registro.DGV_END_ENT2 = linha[53].Remove(0,12);
                registro.DGV_END_ENT3 = linha[54].Remove(0,12);
                registro.DGV_INSTAL = linha[55].Remove(0,11);
                registro.DGV_ROT_ENTREGA = linha[56].Remove(0,16);
                
                return registro;
            }
            catch (Exception ex)
            {
                //throw ex;
                registro.ErroProcessamento = 1;
                registro.NomeCarta = linha[1];
                return registro;
            }
            finally
            {
                registro = null;
            }
        }
        #endregion
    }
}
