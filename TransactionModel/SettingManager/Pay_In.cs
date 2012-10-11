using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransactionModel
{
    public class Pay_In
    {
        static Pay_In _instance;

        static internal Pay_In Instance 
        {
            get
            {
                return _instance ?? (_instance = new Pay_In());
            }
        }

        private Pay_In()
        {

        }

        public string AccNO
        {
            get { return ConfigurationManager.Value; }
        }

        public string CompanyName
        {
            get { return ConfigurationManager.Value; }
        }

        public string CompanyAddress
        {
            get { return ConfigurationManager.Value; }
        }

        public string CompanyAddress2
        {
            get { return ConfigurationManager.Value; }
        }

        public string TaxNO
        {
            get { return ConfigurationManager.Value; }
        }
    }
}
