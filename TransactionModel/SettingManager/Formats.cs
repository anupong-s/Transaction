using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransactionModel
{
    // Housing all configurations, should be load once a session

    public class Formats
    {
        static Formats _instance;

        static internal Formats Instance
        {
            get
            {
                return _instance ?? (_instance = new Formats());
            }
        }

        private Formats()
        {

        }

        public string Date_Format
        {
            get
            {
                return ConfigurationManager.Value ?? (ConfigurationManager.Value = "yyyy-MM-dd");
            }
        }
        
        public string Time_Format
        {
            get
            {
                return ConfigurationManager.Value ?? (ConfigurationManager.Value = "HH:mm:ss");
            }
        }

        public string DateTime_Format
        {
            get
            {
                return ConfigurationManager.Value ?? (ConfigurationManager.Value = "yyyy-MM-dd HH:mm:ss");
            }
        }

        public string Decimal_Format
        {
            get
            {
                return ConfigurationManager.Value ?? (ConfigurationManager.Value = "#,##0.00");
            }
        }

        public string NUMBER_FORMAT
        {
            get
            {
                return ConfigurationManager.Value ?? (ConfigurationManager.Value = "#,##0");
            }
        }
    }
}
