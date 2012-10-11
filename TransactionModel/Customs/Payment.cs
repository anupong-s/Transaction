using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransactionModel.Utils;
using CarPass.Transaction.Common;

namespace TransactionModel
{
    public partial class Payment
    {
        #region Partial Method

        partial void OnCreatedDateChanging(DateTime value)
        {
            if (!value.IsPersistable())
            {
                throw new ArgumentException("CREATE_DATE_NOT_PERSISTABLE");
            }
        }

        partial void OnCreatedByChanging(String value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("NULL_OR_EMPTY_CREATE_BY");
            }

            if (value.Length > 50)
            {
                throw new ArgumentException("CREATE_BY_TOO_LONG");
            }
        }

        partial void OnCreatedByChanged()
        {
            if (!string.IsNullOrEmpty(_CreatedBy))
            {
                _CreatedBy = CreatedBy.TextEncode();
            }
        }

        partial void OnUpdatedDateChanging(DateTime value)
        {
            if (!value.IsPersistable())
            {
                throw new ArgumentException("UPDATE_DATE_NOT_PERSISTABLE");
            }
        }

        partial void OnUpdatedByChanging(String value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("NULL_OR_EMPTY_UPDATE_BY");
            }

            if (value.Length > 50)
            {
                throw new ArgumentException("UPDATE_BY_TOO_LONG");
            }
        }

        partial void OnUpdatedByChanged()
        {
            if (!string.IsNullOrEmpty(_UpdatedBy))
            {
                _UpdatedBy = UpdatedBy.TextEncode();
            }
        }

        partial void OnPaymentCodeChanging(String value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("NULL_OR_EMPTY_PAYMENT_CODE");
            }

            if (value.Length > 8)
            {
                throw new ArgumentException("PAYMENT_CODE_TOO_LONG");
            }
        }

        partial void OnPaymentCodeChanged()
        {
            if (!string.IsNullOrEmpty(_PaymentCode))
            {
                _PaymentCode = PaymentCode.TextEncode();
            }
        }

        partial void OnStatusChanging(String value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("NULL_OR_EMPTY_STATUS");
            }

            if (value.Length > 20)
            {
                throw new ArgumentException("STATUS_TOO_LONG");
            }
        }

        partial void OnStatusChanged()
        {
            if (!string.IsNullOrEmpty(_Status))
            {
                _Status = Status.TextEncode();
            }
        }

        partial void OnCustomerNameChanging(String value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("NULL_OR_EMPTY_CUSTOMER_NAME");
            }

            if (value.Length > 100)
            {
                throw new ArgumentException("CUSTOMER_NAME_TOO_LONG");
            }
        }

        partial void OnCustomerNameChanged()
        {
            if (!string.IsNullOrEmpty(_CustomerName))
            {
                _CustomerName = CustomerName.TextEncode();
            }
        }

        partial void OnCustomerAddressChanging(String value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("NULL_OR_EMPTY_CUSTOMER_ADDRESS");
            }

            if (value.Length > 255)
            {
                throw new ArgumentException("CUSTOMER_ADDRESS_TOO_LONG");
            }
        }

        partial void OnCustomerAddressChanged()
        {
            if (!string.IsNullOrEmpty(_CustomerAddress))
            {
                _CustomerAddress = CustomerAddress.TextEncode();
            }
        }

        partial void OnCustomerRefundAccountNoChanging(String value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length > 50)
                {
                    throw new ArgumentException("CUSTOMER_REFUND_ACCOUNT_NO_TOO_LONG");
                }
            }
        }

        partial void OnCustomerRefundAccountNoChanged()
        {
            if (!string.IsNullOrEmpty(_CustomerRefundAccountNo))
            {
                _CustomerRefundAccountNo = CustomerRefundAccountNo.TextEncode();
            }
        }

        partial void OnCustomerRefundAccountNameChanging(String value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length > 50)
                {
                    throw new ArgumentException("CUSTOMER_REFUND_ACCOUNT_NAME_TOO_LONG");
                }
            }
        }

        partial void OnCustomerRefundAccountNameChanged()
        {
            if (!string.IsNullOrEmpty(_CustomerRefundAccountName))
            {
                _CustomerRefundAccountName = CustomerRefundAccountName.TextEncode();
            }
        }

        partial void OnCustomerMobilePhoneNoChanging(String value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length > 20)
                {
                    throw new ArgumentException("CUSTOMER_MOBILE_PHONE_NO_TOO_LONG");
                }
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

        partial void OnRef1Changing(String value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length > 50)
                {
                    throw new ArgumentException("REF1_TOO_LONG");
                }
            }
        }

        partial void OnRef1Changed()
        {
            if (!string.IsNullOrEmpty(_Ref1))
            {
                _Ref1 = Ref1.TextEncode();
            }
        }

        partial void OnRef2Changing(String value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length > 50)
                {
                    throw new ArgumentException("REF2_TOO_LONG");
                }
            }
        }

        partial void OnRef2Changed()
        {
            if (!string.IsNullOrEmpty(_Ref2))
            {
                _Ref2 = Ref2.TextEncode();
            }
        }

        partial void OnRef3Changing(String value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length > 50)
                {
                    throw new ArgumentException("REF3_TOO_LONG");
                }
            }
        }

        partial void OnRef3Changed()
        {
            if (!string.IsNullOrEmpty(_Ref3))
            {
                _Ref3 = Ref3.TextEncode();
            }
        }

        #endregion

        #region Constructors

        internal Payment() { }

        public Payment(string createBy, long customerIdmPartyId, 
            string customerName, string customerAddress,
            params PaymentItem[] paymentItems)
        {
            CreatedBy = createBy;
            CreatedDate = DateTime.Now;
            UpdatedBy = createBy;
            UpdatedDate = DateTime.Now;

            CustomerIdmPartyId = customerIdmPartyId;
            CustomerName = customerName;
            CustomerAddress = customerAddress;
            Status = PaymentStatusEnum.UNPAID.ToString();

            foreach (var paymentItem in paymentItems)
            {
                PaymentItems.Add(paymentItem);
            }
        }

        #endregion

        #region Public Method

        public void AddInstallments(string installedBy, params Installment[] installments)
        {
            if (string.IsNullOrEmpty(installedBy))
                throw new ApplicationException("MISSING_UPDATE_BY");
            if (installments == null || installments.Length < 1)
                throw new ApplicationException("AT_LEAST_ONE_ITEM_REQUIRED");

            UpdatedBy = installedBy;
            UpdatedDate = DateTime.Now;

            foreach (var installment in installments)
            {
                Installments.Add(installment);
                var remaining = RemainingAmount();

                if (remaining == 0)
                    Status = PaymentStatusEnum.PAID.ToString();
                else if (remaining <= 0)
                {
                    Status = PaymentStatusEnum.OVERPAID.ToString();
                    installment.CustomerCredits.Add(new CustomerCredit("System", Math.Abs(remaining), installment.Id,
                                CustomerIdmPartyId, CustomerName, CustomerAddress));
                }
            }
        }

        public void AddPayInSlip(DateTime issuedDate, string issuedBy, byte[] slipPDF, int paymentId, string documentNumber)
        {
            if (Status.ToUpper() == PaymentStatusEnum.PAID.ToString())
            {
                PayInSlips.Add(new PayInSlip
                        (issuedDate, issuedBy, slipPDF, paymentId, documentNumber));
            }
        }

        public void AddTaxInvoiceReceipt(DateTime issuedDate, string issuedBy, byte[] taxInvoiceReceipt, int paymentId, string documentNumber)
        {
            if (Status.ToUpper() == PaymentStatusEnum.PAID.ToString() || 
                Status.ToUpper() == PaymentStatusEnum.OVERPAID.ToString())
            {
                TaxInvoiceReceipts.Add(new TaxInvoiceReceipt
                        (issuedDate, issuedBy, taxInvoiceReceipt, paymentId, documentNumber));
            }
        }

        public void AddPaymentItem(int serviceCode, string serviceName, int quantity, decimal unitAmount, decimal taxOrVat,
                    decimal withHoldingTax, bool isRevenue, bool isLegalPerson, string itemDescription)
        {
            if (Installments.Count() <= 0 && PayInSlips.Count() <= 0)
            {
                PaymentItems.Add(new PaymentItem(
                    //serviceCode,
                    //serviceName,
                    quantity, 
                    unitAmount, 
                    taxOrVat, 
                    withHoldingTax, 
                    isRevenue, 
                    isLegalPerson,
                    itemDescription));
            }
        }

        public void AddPaymentItem(PaymentItem item)
        {
            if (Installments.Count() <= 0 && PayInSlips.Count() <= 0)
            {
                PaymentItems.Add(item);
            }
        }


        public decimal TotalUnitAmount()
        {
            return PaymentItems.Sum(x => x.SubTotal());
        }

        public decimal TotalVat()
        {
            return PaymentItems.Sum(x => x.VATAmount());
        }

        public decimal TotalWithholdingTax()
        {
            return PaymentItems.Sum(x => x.WithHoldingTaxAmount());
        }

        public decimal GrandTotal()
        {
            return TotalUnitAmount() + TotalVat() - TotalWithholdingTax();
        }

        public decimal InstallmentAmount()
        {
            return Installments.Sum(x => x.Amount);
        }

        public decimal RemainingAmount()
        {
            return GrandTotal() - InstallmentAmount();
        }

        public decimal TotalNoDiscount()
        {
            return PaymentItems.Where(x => x.NetTotal() >= 0).Sum(x => x.NetTotal());
        }

        public decimal TotalDiscount()
        {
            return PaymentItems.Where(x => x.NetTotal() < 0).Sum(x => x.NetTotal());
        }

        #endregion
    }
}
