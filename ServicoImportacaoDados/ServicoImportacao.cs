using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using CamadaBLL;
using ServicoImportacaoDados.Singleton;
using System.Threading;
using Utilidades;

namespace ServicoImportacaoDados
{
    public partial class ServicoImportacao : ServiceBase
    {
        #region Variáveis
        BLLImportarDados _bll = null;
        Log _log = null;
        private System.Timers.Timer timer;
        private bool FatalError = false;
        #endregion
        public ServicoImportacao()
        {
            ServiceName = nameof(ServicoImportacao);

            EventLog.BeginInit();

            if (!EventLog.SourceExists(ServiceName))
                EventLog.CreateEventSource(ServiceName, "Application");

            EventLog.EndInit();

            EventLog.Source = ServiceName;
            EventLog.Log = "Application";

            ApplicationEventLogInstance.Instance.EventLog = EventLog;

            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                //Diretório arquivos RID
                if (!System.IO.Directory.Exists(BLLGlobal.DiretorioArquivosRID))
                    throw new Exception("Caminho para buscar os arquivos não foi encontrado.");

                //LOG Windows
                ApplicationEventLogInstance.Instance.EventLog.WriteEntry("Serviço de Importação de Dados iniciado com Sucesso.", EventLogEntryType.Information);

                //LOG Arquivo
                _log = new Utilidades.Log(DateTime.Now.ToString("dd-MM-yyyy") + "_ServicoImportacaoDados_parte1.log", BLLGlobal.PathLog);
                _log.Gravar(DateTime.Now, "Serviço de Importação de Dados iniciado com Sucesso.", Utilidades.Log.TIPO.SUCESSO);

                _bll = new BLLImportarDados();

                ExecutarService();
            }
            catch (Exception ex)
            {
                //Log do Windows
                ApplicationEventLogInstance.Instance.EventLog.WriteEntry(string.Concat("Serviço de Importação de Dados será parado.\n",
                                                                                                       ex.Message,
                                                                                                       "\n\n\n",
                                                                                                       ex.StackTrace), EventLogEntryType.Error);
                //Arquivo de Log da Aplicação
                _log = new Utilidades.Log(DateTime.Now.ToString("dd-MM-yyyy") + "_ServicoImportacaoDados_parte1.log", BLLGlobal.PathLog);
                _log.Gravar(DateTime.Now, ex.Message, Utilidades.Log.TIPO.ERRO);
                _log.Gravar(DateTime.Now, "Serviço de Importação de Dados será parado.", Utilidades.Log.TIPO.ERRO);
                FatalError = true;
                this.OnStop();
            }
        }

        private void ExecutarService()
        {
            timer = new System.Timers.Timer(BLLGlobal.ConverterTempo(BLLGlobal.AguardarProcessamento.ToString()));
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();
            if (OnExecutar())
            {
                timer.Start();
            }
            else
            {
                this.OnStop();
            }
        }

        private bool OnExecutar()
        {
            try
            {
                //Horário de execução do serviço
                var dtinicio = string.Concat(DateTime.Now.Day, "/", DateTime.Now.Month, "/", DateTime.Now.Year, " ", BLLGlobal.HorarioInicioProcessamento);
                var dtfim = string.Concat(DateTime.Now.Day, "/", DateTime.Now.Month, "/", DateTime.Now.Year, " ", BLLGlobal.HorarioTerminoProcessamento);

                if (DateTime.Now >= Convert.ToDateTime(dtinicio) && DateTime.Now <= Convert.ToDateTime(dtfim))
                    //Processar Remessa
                    _bll.Processar();


                return true;
            }
            catch (Exception ex)
            {

                ApplicationEventLogInstance.Instance.EventLog.WriteEntry(string.Concat("Houve um erro durante o processamento. Iniciando parada.\n",
                                                                                                         ex.Message,
                                                                                                         "\n\n\n",
                                                                                                         ex.StackTrace), EventLogEntryType.Error);

                _log = new Utilidades.Log(DateTime.Now.ToString("dd-MM-yyyy") + "_ServicoImportacaoDados_parte1.log", BLLGlobal.PathLog);
                _log.Gravar(DateTime.Now, ex.Message, Utilidades.Log.TIPO.ERRO);
                FatalError = true;
                return false;
            }
        }

        protected override void OnStop()
        {
            try
            {
                //System.Threading.Thread.Sleep(1000);
                if (FatalError)
                {
                    ApplicationEventLogInstance.Instance.EventLog.WriteEntry("Serviço Parado.", EventLogEntryType.Information);
                    _log = new Utilidades.Log(DateTime.Now.ToString("dd-MM-yyyy") + "_ServicoImportacaoDados_parte1.log", BLLGlobal.PathLog);
                    _log.Gravar(DateTime.Now, "Serviço de Importação parado.", Utilidades.Log.TIPO.SUCESSO);

                    var Processo = System.Diagnostics.Process.GetProcessesByName("ServicoImportacaoDados");
                    if (Processo.Length > 0)
                    {
                        Processo[0].Kill();
                    }
                }
            }
            catch (Exception ex)
            {
                _log = new Utilidades.Log(DateTime.Now.ToString("dd-MM-yyyy") + "_ServicoImportacaoDados_parte1.log", BLLGlobal.PathLog);
                _log.Gravar(DateTime.Now, ex.Message, Utilidades.Log.TIPO.ERRO);
                _log.Gravar(DateTime.Now, "Serviço de Importação parado.", Utilidades.Log.TIPO.SUCESSO);
                var Processo = System.Diagnostics.Process.GetProcessesByName("ServicoImportacaoDados");
                if (Processo.Length > 0)
                {
                    Processo[0].Kill();
                }
            }
        }
    }
}
