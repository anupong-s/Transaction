using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using CarPass.Transaction.Common;

namespace TransactionServiceInterface
{
    public partial interface ITransactionService
    {
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        RefundResponse CreateRefund(RefundRequest req);
    }

    [DataContract]
    public class RefundResponse : AbstractResponse
    {

    }

    [DataContract]
    public class RefundRequest : AbstractRequest
    {
        [DataMember]
        public string CreateBy { get; set; }

        [DataMember]
        public string CustomerAddress { get; set; }

        [DataMember]
        public long CustomerIdmPartyId { get; set; }

        [DataMember]
        public string CustomerMobilePhoneNo { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string CustomerRefundAccountName { get; set; }

        [DataMember]
        public string CustomerRefundAccountNo { get; set; }

        [DataMember]
        public int? CustomerRefundBankId { get; set; }

        [DataMember]
        public bool IsVoid { get; set; }

        [DataMember]
        public string PaymentCode { get; set; }

        [DataMember]
        public string Ref1 { get; set; }

        [DataMember]
        public string Ref2 { get; set; }

        [DataMember]
        public string Ref3 { get; set; }

        [DataMember]
        public string Remark { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string UpdateBy { get; set; }

        [DataMember]
        public RefundItemRequest[] Items { get; set; }
    }

    [DataContract]
    public class RefundItemRequest
    {
        [DataMember]
        public int Qty { get; set; }
        [DataMember]
        public int UnitAmount { get; set; }
        [DataMember]
        public string ItemDescription { get; set; }
        [DataMember]
        public string Remark { get; set; }
        [DataMember]
        public string GroupRef1 { get; set; }
        [DataMember]
        public string GroupRef2 { get; set; }
    }
}
