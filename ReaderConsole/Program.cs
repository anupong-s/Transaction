using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReconciliationFileReader;
using System.IO;
using System.Threading;
using TransactionModel;

namespace ReaderConsole
{
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
            FileSystemWatcher watcher = new System.IO.FileSystemWatcher();
            Reader reader = new Reader(watcher, basePath);
            //Reader reader = new Reader(watcher, @"C:\Reconciliation\BBL\CreditCard\");
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

    class Program
    {
        static void Main(string[] args)
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

                //Runner bblRunner = new Runner(@"C:\Reconciliation\BBL\CreditCard\");
                //Runner scbRunner = new Runner(@"C:\Reconciliation\SCB\CreditCard\");
                //Runner bblRunner = new Runner(@"C:\Reconciliation\BBL\PayInSlip\");
                //Runner scbRunner = new Runner(@"C:\Reconciliation\SCB\PayInSlip\");

            }

            string cmd = null;

            do
            {
                Console.Write("Enter a command: ");
                cmd = Console.ReadLine();

                switch (cmd)
                {
                    case "Start":
                        runners.ForEach(x => x.Start());
                        //bblRunner.Start();
                        //scbRunner.Start();
                        break;
                    case "Stop":
                        runners.ForEach(x => x.Stop());
                        //bblRunner.Stop();
                        //scbRunner.Stop();
                        break;
                    default: Console.WriteLine("Wrong command!!!"); break;
                }
            }
            while (cmd != "Q");
            Console.WriteLine("Thanks");
        }

        //private static void BBL()
        //{
        //    FileSystemWatcher watcher = new System.IO.FileSystemWatcher();
        //    Reader reader = new Reader(watcher, @"C:\Reconciliation\BBL\PayInSlip\");
        //    //Reader reader = new Reader(watcher, @"C:\Reconciliation\BBL\CreditCard\");
        //    reader.StartReading();
        //    while (true)
        //    {
        //        Thread.Sleep(5000);
        //    }
        //    reader.StopReading();
        //}

        //private static void SCB()
        //{
        //    FileSystemWatcher watcher = new System.IO.FileSystemWatcher();
        //    Reader reader = new Reader(watcher, @"C:\Reconciliation\SCB\PayInSlip\");
        //    //Reader reader = new Reader(watcher, @"C:\Reconciliation\SCB\CreditCard\");
        //    reader.StartReading();
            
        //    //reader.StopReading();
        //}
    }
}
