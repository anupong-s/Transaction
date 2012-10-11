using System.Runtime.Serialization;
using System.ServiceModel;

namespace TransactionServiceInterface
{
    [DataContract]
    public class RetrievePaymentCodeResponse : AbstractResponse
    {
        [DataMember]
        public string PaymentCode { get; set; }
    }

    [DataContract]
    public class RetrievePaymentCodeRequest
    {
    }

    public partial interface ITransactionService
    {
        [OperationContract]
        RetrievePaymentCodeResponse RetrievePaymentCode(RetrievePaymentCodeRequest req);
    }
}
