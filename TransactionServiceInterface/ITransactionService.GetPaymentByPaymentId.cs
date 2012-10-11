using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace TransactionServiceInterface
{
    [DataContract]
    public class GetPaymentResponse : AbstractResponse
    {
        /// <summary>
        /// The one who create this payment
        /// </summary>
        [DataMember]
        public string CreateBy { get; set; }

        /// <summary>
        /// The create date
        /// </summary>
        [DataMember]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// The one who create this payment
        /// </summary>
        [DataMember]
        public string UpdateBy { get; set; }

        /// <summary>
        /// The create date
        /// </summary>
        [DataMember]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// Customer IDM party ID referred to the IDM system 
        /// </summary>
        [DataMember]
        public long CustomerIDMPartyID { get; set; }

        /// <summary>
        /// Customer Code
        /// </summary>
        [DataMember]
        public string CustomerCode { get; set; }

        /// <summary>
        /// The first name and last name (or company name) of the customer 
        /// </summary>
        [DataMember]
        public string CustomerName { get; set; }

        /// <summary>
        /// Customer address 
        /// </summary>
        [DataMember]
        public string CustomerAddress { get; set; }

        /// <summary>
        /// Customer refund account number (use for refund) 
        /// </summary>
        [DataMember]
        public string CustomerAccountNumber { get; set; }

        /// <summary>
        /// Customer refund account name (use for refund) 
        /// </summary>
        [DataMember]
        public string CustomerAccountName { get; set; }

        /// <summary>
        /// Customer refund bank ID (as in Master system) 
        /// </summary>
        [DataMember]
        public int? CustomerAccountBankId { get; set; }

        /// <summary>
        /// Customer mobile phone number (use for refund) 
        /// </summary>
        [DataMember]
        public string CustomerMobilePhoneNumber { get; set; }

        /// <summary>
        /// The stamped payment ID
        /// </summary>
        [DataMember]
        public long PaymentId { get; set; }

        /// <summary>
        /// The stamped payment code
        /// </summary>
        [DataMember]
        public string PaymentCode { get; set; }

        /// <summary>
        /// Remark 
        /// </summary>
        [DataMember]
        public string Remark { get; set; }

        /// <summary>
        /// Ref1 
        /// </summary>
        [DataMember]
        public string Ref1 { get; set; }

        /// <summary>
        /// Ref2 
        /// </summary>
        [DataMember]
        public string Ref2 { get; set; }

        /// <summary>
        /// Ref3 
        /// </summary>
        [DataMember]
        public string Ref3 { get; set; }

        /// <summary>
        /// PaymentStatus 
        /// </summary>
        [DataMember]
        public string PaymentStatus { get; set; }

        /// <summary>
        /// RemainingAmount 
        /// </summary>
        [DataMember]
        public decimal RemainingAmount { get; set; }

        /// <summary>
        /// Grand Total 
        /// </summary>
        [DataMember]
        public decimal GrandTotal { get; set; }

        /// <summary>
        /// TotalVatAmount
        /// </summary>
        [DataMember]
        public decimal TotalVatAmount { get; set; }

        /// <summary>
        /// TotalWithholdingTaxAmount
        /// </summary>
        [DataMember]
        public decimal TotalWithholdingTaxAmount { get; set; }

        /// <summary>
        /// TotalBeforeAdjusted
        /// </summary>
        [DataMember]
        public decimal TotalBeforeAdjustment { get; set; }

        /// <summary>
        /// TotalBeforeAdjusted
        /// </summary>
        [DataMember]
        public decimal TotalAdjustment { get; set; }

        /// <summary>
        /// PaymentItems
        /// </summary>
        [DataMember]
        public GetPaymentItemResponse[] PaymentItems { get; set; }
    }

    [DataContract]
    public class GetPaymentItemResponse
    {
        /// <summary>
        /// The payment item Id.        
        /// </summary>
        [DataMember]
        public long PaymentItemId { get; set; }

        /// <summary>
        /// The Service Code. Check the Transaction system's service code master table. 
        /// </summary>
        //[DataMember]
        //public string ServiceCode { get; set; }

        /// <summary>
        /// The Service Name. Check the Transaction system's service code master table. 
        /// </summary>
        [DataMember]
        public string ServiceName { get; set; }

        /// <summary>
        /// Whether the item is revenue
        /// </summary>
        [DataMember]
        public bool IsRevenue { get; set; }

        /// <summary>
        /// Additional details about the item
        /// </summary>
        [DataMember]
        public string ItemDescription { get; set; }

        /// <summary>
        /// Number of this item
        /// </summary>
        [DataMember]
        public int Quantity { get; set; }

        /// <summary>
        /// Unit amount 
        /// </summary>
        [DataMember]
        public decimal UnitAmount { get; set; }

        /// <summary>
        /// Withholding tax percent
        /// </summary>
        [DataMember]
        public decimal WithholdingTaxPercent { get; set; }

        /// <summary>
        /// Vat percent
        /// </summary>
        [DataMember]
        public decimal VatPercent { get; set; }

        /// <summary>
        /// Whether to charge withholding tax
        /// </summary>
        [DataMember]
        public bool IsLegalPerson { get; set; }

        /// <summary>
        /// Additional remark (optional)
        /// </summary>
        [DataMember]
        public string Remark { get; set; }

        /// <summary>
        /// Group reference 1 
        /// </summary>
        [DataMember]
        public string GroupRef1 { get; set; }

        /// <summary>
        /// Group reference 2
        /// </summary>
        [DataMember]
        public string GroupRef2 { get; set; }

        /// <summary>
        /// Group reference 3
        /// </summary>
        [DataMember]
        public string GroupRef3 { get; set; }

        /// <summary>
        /// The sub-total from qty * unitAmount
        /// </summary>
        [DataMember]
        public decimal SubTotal { get; set; }

        /// <summary>
        /// The withholding tax amount from withholdingTaxPercent * SubTotal
        /// </summary>
        [DataMember]
        public decimal WithholdingTaxAmount { get; set; }

        /// <summary>
        /// The VAT amount from VatPercent * SubTotal
        /// </summary>
        [DataMember]
        public decimal VatAmount { get; set; }

        /// <summary>
        /// The net total from SubTotal + VatAmount - WithholdingTaxAmount
        /// </summary>
        [DataMember]
        public decimal NetTotal { get; set; }
    }

    [DataContract]
    public class GetPaymentRequest
    {
        [DataMember]
        public string PaymentId { get; set; }
    }

    public partial interface ITransactionService
    {
        [OperationContract]
        GetPaymentResponse GetPaymentByPaymentId(GetPaymentRequest req);
    }
}
