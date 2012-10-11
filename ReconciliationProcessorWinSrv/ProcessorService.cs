using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using ReconciliationFileProcessor;
using TransactionModel;


namespace ReconciliationProcessorWinSrv
{
    public partial class ProcessorService : ServiceBase
    {
        class Runner
        {
            private string basePath = null;
            private Thread t = null;
            private bool running = false;

            public void Run()
            {
                Processor updator = new Processor();

                while (running)
                {
                    updator.ReadReconciliation();
                    Thread.Sleep(5000);
                }
            }

            public void Start()
            {
                running = true;
                t = new Thread(new ThreadStart(this.Run));
                t.Start();
            }

            public void Stop()
            {
                running = false;
                while (t.IsAlive);
            }
        }

        public ProcessorService()
        {
            InitializeComponent();
        }

        Runner runner = new Runner();
        protected override void OnStart(string[] args)
        {
            try
            {
                runner.Start();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (ex.InnerException != null)
                    msg = ex.InnerException.Message;

                ErrorLog.CreateErrorLog("ReconciliationProcessor",
                            msg,
                            SeverityEnum.HIGH,
                            SystemError.ServiceReader);
            }
        }

        protected override void OnStop()
        {
            try
            {
                runner.Stop();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (ex.InnerException != null)
                    msg = ex.InnerException.Message;

                ErrorLog.CreateErrorLog("ReconciliationProcessor",
                            msg,
                            SeverityEnum.HIGH,
                            SystemError.ServiceReader);
            }
        }
    }
}
