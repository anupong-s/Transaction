using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using InstallmentUpdatingScheduler;
using System.Threading;

namespace InstallmentServiceWinSrv
{
    public partial class InstallmentService : ServiceBase
    {
        class Runner
        {
            private string basePath = null;
            private Thread t = null;
            private bool running = false;

            public void Run()
            {
                InstallmentUpdator updator = new InstallmentUpdator();

                while (running)
                {
                    updator.UpdateInstallments();
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
                while (t.IsAlive) ;
            }
        }

        public InstallmentService()
        {
            InitializeComponent();
        }

        Runner runner = new Runner();
        protected override void OnStart(string[] args)
        {
            runner.Start();
        }

        protected override void OnStop()
        {
            runner.Stop();
        }
    }
}
