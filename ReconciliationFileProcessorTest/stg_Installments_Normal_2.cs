using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReconciliationFileProcessorTest
{
    public class stg_Installments_Normal
    {
        public DateTime PaymentDate { get; set; }
        public string CustomerName { get; set; }
        public string PaymentCode { get; set; }
        public string BranchNo { get; set; }
        public string TransactionType { get; set; }
        public string TransactionCode { get; set; }
        public decimal Amount { get; set; }
        public string FileName { get; set; }
        public string RecordStatus { get; set; }
        public string RecordRemark { get; set; }
        public string BankCode { get; set; }
        public string ProcessStatus { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime PostingDate { get; set; }
        public string Ref2 { get; set; }
        public string TellerNo { get; set; }
        public string ChequeNo { get; set; }
        public string ChequeBankCode { get; set; }
        public string CompanyAccount { get; set; }
    }
}
