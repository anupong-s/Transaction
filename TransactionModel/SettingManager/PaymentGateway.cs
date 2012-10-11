using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransactionModel
{
    public class PaymentGateway
    {
        static PaymentGateway _instance;
        static internal PaymentGateway Instance
        {
            get { return _instance ?? (_instance = new PaymentGateway()); }
        }

        private PaymentGateway() { }

        public string CurrencyCode
        {
            get { return ConfigurationManager.Value; }
        }

        public string Language
        {
            get { return ConfigurationManager.Value; }
        }

        public string MerchantID
        {
            get { return ConfigurationManager.Value; }
        }

        public string PaymentType
        {
            get { return ConfigurationManager.Value; }
        }
    }
}
