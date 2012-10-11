using System.Runtime.Serialization;
using System.ServiceModel;
using CarPass.Transaction.Common;
using System;

namespace TransactionServiceInterface
{
    [DataContract]
    public class GetReconciliationByPaymentCodeResponse : AbstractResponse
    {
        [DataMember]
        public long ReconciliationId { get; set; }

        //[DataMember]
        //public long PaymentId { get; set; }

        [DataMember]
        public string PaymentCode { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public DateTime PaymentDate { get; set; }

        [DataMember]
        public string PaymentBy { get; set; }

        [DataMember]
        public PaymentMethodEnum PaymentMethod { get; set; }
    }

    [DataContract]
    public class GetReconciliationByPaymentCodeRequest : AbstractRequest
    {
        [DataMember]
        public string PaymentCode { get; set; }
    }

    public partial interface ITransactionService
    {
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        GetReconciliationByPaymentCodeResponse[] GetReconciliationByPaymentCode(GetReconciliationByPaymentCodeRequest[] req);
    }
}
