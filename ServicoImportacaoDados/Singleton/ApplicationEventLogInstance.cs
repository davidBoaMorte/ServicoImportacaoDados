using System;
using System.Diagnostics;

namespace ServicoImportacaoDados.Singleton
{
    public class ApplicationEventLogInstance
    {
        private static readonly Lazy<ApplicationEventLogInstance> lazy = new Lazy<ApplicationEventLogInstance>(() => new ApplicationEventLogInstance());

        public static ApplicationEventLogInstance Instance { get { return lazy.Value; } }

        public EventLog EventLog { get; set; }

        private ApplicationEventLogInstance() { }
    }
}
