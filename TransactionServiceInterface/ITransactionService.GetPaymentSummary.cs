using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace TransactionServiceInterface
{
    [DataContract]
    public class GetPaymentSummaryResponse : AbstractResponse
    {
        [DataMember]
        public long PaymentId { get; set; }

        [DataMember]
        public DateTime IssuedDate { get; set; }

        [DataMember]
        public string PaymentStatus { get; set; }

        [DataMember]
        public double Amount { get; set; }

        [DataMember]
        public double PaidAmount { get; set; }

        [DataMember]
        public double RemainingAmount { get; set; }
    }

    [DataContract]
    public class GetPaymentSummaryRequest : AbstractRequest
    {
        [DataMember]
        public long PaymentId { get; set; }
    }

    public partial interface ITransactionService
    {
        [OperationContract]
        GetPaymentSummaryResponse GetPaymentSummary(GetPaymentSummaryRequest req);
    }
}
