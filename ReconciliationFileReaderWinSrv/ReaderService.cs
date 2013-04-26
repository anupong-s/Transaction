using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using ReconciliationFileReader;
using TransactionModel;

namespace ReconciliationFileReaderWinSrv
{
    partial class ReaderService : ServiceBase
    {
        public ReaderService()
        {
            InitializeComponent();
        }

        List<Runner> runners = new List<Runner>();

        protected override void OnStart(string[] args)
        {
            try
            {
                var runners = new List<Runner>();
                using (var container = new TransactionModelContainer())
                {
                    var basePaths = container.Configurations.Where(x => x.Group.ToLower() == "basepath").ToArray();
                    foreach (var path in basePaths)
                    {
                        if (!string.IsNullOrEmpty(path.Value1))
                        {
                            var slip = new Runner(path.Value1);
                            runners.Add(slip);
                        }
                        if (!string.IsNullOrEmpty(path.Value2))
                        {
                            var credit = new Runner(path.Value2);
                            runners.Add(credit);
                        }
                    }
                }

                if (runners.Count() > 0)
                {
                    runners.ForEach(x => x.Start());
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Log("ReconciliationFileReader", ex, SystemError.ServiceReader);
            }
        }

        protected override void OnStop()
        {
            try
            {
                if (runners.Count > 0)
                {
                    runners.ForEach(x => x.Stop());
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Log("System", ex, SystemError.ServiceReader);
            }
        }
    }

    class Runner
    {
        private string basePath = null;
        private Thread t = null;
        private bool running = false;

        public Runner(string basePath)
        {
            this.basePath = basePath;
        }

        public void Run()
        {
            var watcher = new System.IO.FileSystemWatcher();
            var reader = new Reader(watcher, basePath);
            reader.StartReading();

            while (running)
            {
                Thread.Sleep(5000);
            }

            reader.StopReading();
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
}
