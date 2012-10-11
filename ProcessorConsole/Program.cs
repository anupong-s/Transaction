using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ReconciliationFileProcessor;

namespace ProcessorConsole
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
            while (t.IsAlive) ;
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            Runner runner = new Runner();

            string cmd = null;

            do
            {
                Console.Write("Enter a command: ");
                cmd = Console.ReadLine();

                switch (cmd)
                {
                    case "Start":
                        runner.Start();
                        break;
                    case "Stop":
                        runner.Stop();
                        break;
                    default: Console.WriteLine("Wrong command!!!"); break;
                }
            }
            while (cmd != "Q");
            Console.WriteLine("Thanks");
        }
    }
}
