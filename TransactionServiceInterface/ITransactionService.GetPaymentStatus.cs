using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using CarPass.Transaction.Common;

namespace TransactionServiceInterface
{
    [DataContract]
    public class GetPaymentStatusResponse : AbstractResponse
    {
        [DataMember]
        public PaymentStatusEnum PaymentStatus { get; set; }
    }

    [DataContract]
    public class GetPaymentStatusRequest
    {
        [DataMember]
        public string PaymentId { get; set; }
    }

    public partial interface ITransactionService
    {
        [OperationContract]
        GetPaymentStatusResponse GetPaymentStatusByPaymentId(GetPaymentStatusRequest req);
    }
}
