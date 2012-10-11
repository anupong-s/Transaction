using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace TransactionServiceInterface
{
    [DataContract]
    public class CalculatePaymentItemRequest
    {
        public CalculatePaymentItemRequest() { }

        public CalculatePaymentItemRequest(string itemdescription, int qty, decimal unitAmount, decimal withholdingTaxPercent
            ,decimal vatPercent,bool isRevenue, bool isLegalPerson)
        {
            //ServiceCode = serviceCode;
            ItemDescription = itemdescription;
            Quantity = qty;
            UnitAmount = unitAmount;
            WithholdingTaxPercent = withholdingTaxPercent;
            VatPercent = vatPercent;
            ServiceIsRevenue = isRevenue;
            IsLegalPerson = isLegalPerson;
        }

        /// <summary>
        /// The Service Code. Check the Transaction system's service code master table. 
        /// This field is mandatory.
        /// </summary>
        //[DataMember]
        //public string ServiceCode { get; set; }
        
        /// <summary>
        /// Additional details about the item
        /// This field is optional.
        /// </summary>
        [DataMember]
        public string ItemDescription { get; set; }

        /// <summary>
        /// Number of this item (mandatory)
        /// </summary>
        [DataMember]
        public int Quantity { get; set; }

        /// <summary>
        /// Unit amount (mandatory)
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
        /// IsRevenue
        /// </summary>
        [DataMember]
        public bool ServiceIsRevenue { get; set; }
    }

    [DataContract]
    public class CalculatePaymentRequest : AbstractRequest
    {
        /// <summary>
        /// The first name and last name (or company name) of the customer (mandatory)
        /// </summary>
        [DataMember]
        public string CustomerName { get; set; }

        /// <summary>
        /// PaymentItems (mandatory-at least one required)
        /// </summary>
        [DataMember]
        public CalculatePaymentItemRequest[] PaymentItems { get; set; }
    }

    [DataContract]
    public class CalculatePaymentItemResponse
    {
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
    public class CalculatePaymentResponse : AbstractResponse
    {
        /// <summary>
        /// The first name and last name (or company name) of the customer 
        /// </summary>
        [DataMember]
        public string CustomerName { get; set; }

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
        public CalculatePaymentItemResponse[] PaymentItems { get; set; }
    }

    public partial interface ITransactionService
    {
        [OperationContract]
        CalculatePaymentResponse CalculatePayment(CalculatePaymentRequest req);
    }
}
