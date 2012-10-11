using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace TransactionServiceInterface
{
    [DataContract]
    public class GetTaxInvoiceReceiptRequest
    {
        [DataMember]
        public string PaymentId { get; set; }

    }

    [DataContract]
    public class GetTaxInvoiceReceiptResponse : AbstractResponse
    {
        [DataMember]
        public byte[] TaxInvoicePDF { get; set; }
    }

    public partial interface ITransactionService
    {
        [OperationContract]
        GetTaxInvoiceReceiptResponse GetTaxInvoiceReceiptByPaymentId(GetTaxInvoiceReceiptRequest req);
    }
}
