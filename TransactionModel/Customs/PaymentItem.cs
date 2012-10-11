using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransactionModel.Utils;
using TransactionCommon;

namespace TransactionModel
{
    public partial class PaymentItem
    {
        #region Constructors

         internal PaymentItem() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceCode"></param>
        /// <param name="quantity"></param>
        /// <param name="unitAmount"></param>
        /// <param name="taxOrVat">Example: 3, 5, 7</param>
        /// <param name="withHoldingTax">Example: 3, 5, 7</param>
        /// <param name="isRevenue"></param>
        /// <param name="isLegalPerson"></param>
        public PaymentItem(int quantity, decimal unitAmount, decimal taxOrVat,
                    decimal withHoldingTax, bool isRevenue, bool isLegalPerson, string itemDescription)
        {
            //this.ServiceCode = serviceCode;
            //this.ServiceName = serviceName;
            this.Qty = quantity;
            this.UnitAmount = unitAmount;
            this.VatPercent = taxOrVat;
            this.WithholdingTexPercent = withHoldingTax;
            this.ServiceIsRevenue = isRevenue;
            this.IsLegalPerson = isLegalPerson;
            this.ItemDescription = itemDescription;
        }   
    
        #endregion

        #region Partial Method

        partial void OnGroupRef1Changing(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length > 50)
                {
                    throw new ArgumentException("GROUP_REF1_TOO_LONG");
                }
            }
        }

        partial void OnGroupRef1Changed()
        {
            if (!string.IsNullOrEmpty(_GroupRef1))
            {
                _GroupRef1 = _GroupRef1.TextEncode();
            }
        }

        partial void OnGroupRef2Changing(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length > 50)
                {
                    throw new ArgumentException("GROUP_REF2_TOO_LONG");
                }
            }
        }

        partial void OnGroupRef2Changed()
        {
            if (!string.IsNullOrEmpty(_GroupRef2))
            {
                _GroupRef2 = GroupRef2.TextEncode();
            }
        }

        partial void OnGroupRef3Changing(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length > 50)
                {
                    throw new ArgumentException("GROUP_REF3_TOO_LONG");
                }
            }
        }

        partial void OnGroupRef3Changed()
        {
            if (!string.IsNullOrEmpty(_GroupRef3))
            {
                _GroupRef3 = GroupRef3.TextEncode();
            }
        }

        partial void OnRemarkChanging(String value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length > 255)
                {
                    throw new ArgumentException("REMARK_TOO_LONG");
                }
            }
        }

        partial void OnRemarkChanged()
        {
            if (!string.IsNullOrEmpty(_Remark))
            {
                _Remark = Remark.TextEncode();
            }
        }

        #endregion

        #region Public Method

        public decimal SubTotal()
        {
            return Qty * UnitAmount;
        }
        
        public decimal VATAmount()
        {
            return SubTotal() * (VatPercent / 100);
        }

        public decimal WithHoldingTaxAmount()
        {
            return SubTotal() * (WithholdingTexPercent / 100);
        }

        public decimal NetTotal()
        {
            return (SubTotal() + VATAmount()) - WithHoldingTaxAmount();
        }

        #endregion

        #region Static Method

        public static PaymentItem CreateSubscriptionFee(int quantity, decimal unitAmount, 
                                        decimal taxOrVat,decimal withHoldingTax, bool isLegalPerson)
        {
            return new PaymentItem
            {
                Qty = quantity,
                UnitAmount = unitAmount,
                VatPercent = taxOrVat,
                WithholdingTexPercent = withHoldingTax,
                ServiceIsRevenue = true,
                IsLegalPerson = isLegalPerson,
            };
        }

        public static PaymentItem CreateFullYearUpgrade(int quantity, decimal unitAmount,
                                        decimal taxOrVat, decimal withHoldingTax, bool isLegalPerson)
        {
            return new PaymentItem
            {
                Qty = quantity,
                UnitAmount = unitAmount,
                VatPercent = taxOrVat,
                WithholdingTexPercent = withHoldingTax,
                ServiceIsRevenue = true,
                IsLegalPerson = isLegalPerson,
            };
        }

        public static PaymentItem CreateDiscount(int quantity, decimal discount, decimal taxOrVat, decimal withHoldingTax)
        {
            return new PaymentItem
            {
                
                Qty = quantity,
                UnitAmount = Math.Abs(discount) * -1,
                VatPercent = taxOrVat,
                WithholdingTexPercent = withHoldingTax,
                ServiceIsRevenue = true,
                IsLegalPerson = false,
            };
        }

        public static PaymentItem CreateDeviceDeposit(int quantity, decimal unitAmount)
        {
            return new PaymentItem
            {
                
                Qty = quantity,
                UnitAmount = unitAmount,
                VatPercent = 0,
                WithholdingTexPercent = 0,
                ServiceIsRevenue = false,
                IsLegalPerson = false,
            };
        }

        public static PaymentItem CreateCustomerCredit(long Id, decimal amount)
        {
            string serviceName = string.Format("Customer Credit (Customer Credit ID={0})", 
                                        Id.ToString().PadLeft(10,'0'));
            return new PaymentItem
            {
                
                ItemDescription = string.Format("(Customer Credit ID={0})" , Id.ToString().PadLeft(10,'0')),
                Qty = 1,
                UnitAmount = Math.Abs(amount) * -1,
                VatPercent = 0,
                WithholdingTexPercent = 0,
                ServiceIsRevenue = false,
                IsLegalPerson = false,
            };
        }


        #endregion

    }
}
