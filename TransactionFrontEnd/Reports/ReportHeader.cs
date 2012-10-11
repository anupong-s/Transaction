using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransactionFrontEnd.ViewPayment
{
    public class ReportHeader
    {
        public virtual string No { get; set; }
        public virtual string date { get; set; }
        public virtual string ReferenceId { get; set; }
        public virtual string CustomerIdmId { get; set; }
        public virtual string CustomerCode { get; set; }
        public virtual string CustomerName { get; set; }
        public virtual string CustomerAddress { get; set; }
        public virtual string CustomerMobilePhoneNo { get; set; }
        public virtual string CustomerAccountNo { get; set; }
        public virtual string CustomerAccountName { get; set; }
        public virtual string Remark { get; set; }
        public virtual string PaymentId { get; set; }
        public virtual string PaymentCode { get; set; }
        public virtual string Ref1 { get; set; }
        public virtual string Ref2 { get; set; }
        public virtual string Ref3 { get; set; }
        public virtual string PaymentStatus { get; set; }
        public virtual string RemainingAmount { get; set; }
        public virtual ReportItems[] Items { get; set; }

        public virtual string GrandTotal_SubTotal { get; set; }
        public virtual string GrandTotal_WH_Amount { get; set; }
        public virtual string GrandTotal_VAT_Amount { get; set; }
        public virtual string GrandTotal_Net_Total { get; set; }

        public virtual string Summary_Total { get; set; }
        public virtual string Summary_Adjustment { get; set; }
    }

    public class ReportItems
    {
        public virtual string No { get; set; }
        public virtual string Group { get; set; }
        public virtual string Name { get; set; }
        public virtual string Quantity { get; set; }
        public virtual string UnitPrice { get; set; }
        public virtual string SubTotal { get; set; }
        public virtual string WithholdingTaxPercent { get; set; }
        public virtual string WithholdingTaxAmount { get; set; }
        public virtual string VatPercent { get; set; }
        public virtual string VatAmount { get; set; }
        public virtual string NetAmount { get; set; }
    }
}
