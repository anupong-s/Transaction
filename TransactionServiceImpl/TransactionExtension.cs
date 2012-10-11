using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransactionModel.Utils;
using TransactionServiceAdaptor.IDMService;
using TransactionFrontEnd.ViewPayment;
using TransactionCommon;
using TransactionServiceInterface;
using TransactionModel;
using System.ServiceModel;

namespace TransactionServiceImpl
{
    public static class TransactionExtension
    {
        public static int ToIntDefault(this string input)
        {
            int result = 0;
            if (int.TryParse(input, out result))
                return result;
            return result;
        }

        public static PaymentItem[] CreatePaymentItems(this CreatePaymentItemRequest[] input)
        {
            var result = new PaymentItem[input.Count()];
            using (var container = new TransactionModelContainer())
            {
                for (int i = 0; i < result.Length; i++)
                {
                    //int serviceCode = input[i].ServiceCode.ToInt();
                    result[i] = new PaymentItem(
                        //input[i].ServiceCode.ToInt(),
                        //container.ServiceCodes.FirstOrDefault(t => t.Code == serviceCode).Name,
                        input[i].Quantity,
                        input[i].UnitAmount,
                        input[i].VatPercent,
                        input[i].WithholdingTaxPercent,
                        input[i].ServiceIsRevenue,
                        input[i].IsLegalPerson,
                        input[i].ItemDescription);
                }

            }

            return result;
        }

        public static CreatePaymentItemResponse[] CreatePaymentResponseItem(this IEnumerable<PaymentItem> items)
        {
            var input = items.ToArray();
            var result = new CreatePaymentItemResponse[input.Count()];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new CreatePaymentItemResponse
                {
                    PaymentItemId = input[i].PaymentId,
                    //ServiceCode = input[i].ServiceCode.ToString(),
                    //ServiceName = input[i].ServiceName,
                    IsRevenue = input[i].ServiceIsRevenue,
                    ItemDescription = input[i].ItemDescription,
                    Quantity = input[i].Qty,
                    UnitAmount = input[i].UnitAmount,
                    WithholdingTaxPercent = input[i].WithholdingTexPercent,
                    VatPercent = input[i].VatPercent,
                    IsLegalPerson = input[i].IsLegalPerson,
                    Remark = input[i].Remark,
                    GroupRef1 = input[i].GroupRef1,
                    GroupRef2 = input[i].GroupRef2,
                    GroupRef3 = input[i].GroupRef3,
                    SubTotal = input[i].SubTotal(),
                    WithholdingTaxAmount = input[i].WithHoldingTaxAmount(),
                    VatAmount = input[i].VATAmount(),
                    NetTotal = input[i].NetTotal()
                };
            }

            return result;
        }

        public static PaymentItem[] CreatePaymentItems(this CalculatePaymentItemRequest[] items)
        {
            var input = items.ToArray();
            var result = new PaymentItem[input.Count()];
            using (var container = new TransactionModelContainer())
            {
                for (int i = 0; i < result.Length; i++)
                {
                    //int serviceCode = input[i].ServiceCode.ToInt();
                    result[i] = new PaymentItem(
                        //input[i].ServiceCode.ToInt(),
                        //container.ServiceCodes.FirstOrDefault(t => t.Code == serviceCode).Name,
                        input[i].Quantity,
                        input[i].UnitAmount,
                        input[i].VatPercent,
                        input[i].WithholdingTaxPercent,
                        input[i].ServiceIsRevenue,
                        input[i].IsLegalPerson,
                        input[i].ItemDescription);
                }
            }
            return result;
        }

        public static CalculatePaymentItemResponse[] CreateCalculatePaymentItems(this IEnumerable<PaymentItem> items)
        {
            var input = items.ToArray();
            var result = new CalculatePaymentItemResponse[input.Count()];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new CalculatePaymentItemResponse
                {
                    IsRevenue = input[i].ServiceIsRevenue,
                    IsLegalPerson = input[i].IsLegalPerson,
                    ItemDescription = input[i].ItemDescription,
                    NetTotal = input[i].NetTotal(),
                    Quantity = input[i].Qty,
                    //ServiceCode = input[i].ServiceCode.ToString(),
                    //ServiceName = input[i].ServiceName,
                    SubTotal = input[i].SubTotal(),
                    UnitAmount = input[i].UnitAmount,
                    VatAmount = input[i].VATAmount(),
                    VatPercent = input[i].VatPercent,
                    WithholdingTaxAmount = input[i].WithHoldingTaxAmount(),
                    WithholdingTaxPercent = input[i].WithholdingTexPercent,
                };
            }
            return result;
        }

        public static CustomerCredit[] GetCustomerCredit(this List<CustomerCredit> input, decimal grandTotal)
        {
            var custCredits = new List<CustomerCredit>();

            decimal saving = 0;
            foreach (var credit in input.OrderBy(x => x.CreatedDate))
            {
                saving += credit.Amount;
                if (saving <= grandTotal)
                    custCredits.Add(credit);
                else
                    break;
            }

            return custCredits.ToArray();
        }

        public static void SetPaymentResponse(this CreatePaymentResponse res, Payment payment)
        {
            res.PaymentId = payment.Id;
            res.PaymentCode = payment.PaymentCode;
            res.CreateBy = payment.CreatedBy;
            res.CreateDate = payment.CreatedDate;
            res.UpdateBy = payment.UpdatedBy;
            res.UpdateDate = payment.UpdatedDate;
            res.CustomerIDMPartyID = payment.CustomerIdmPartyId;
            res.CustomerCode = null;
            res.CustomerName = payment.CustomerName;
            res.CustomerAddress = payment.CustomerAddress;
            res.CustomerAccountNumber = payment.CustomerRefundAccountNo;
            res.CustomerAccountName = payment.CustomerRefundAccountName;
            res.CustomerAccountBankId = payment.CustomerRefundBankId;
            res.CustomerMobilePhoneNumber = payment.CustomerMobilePhoneNo;
            res.Remark = payment.Remark;
            res.Ref1 = payment.Ref1;
            res.Ref2 = payment.Ref2;
            res.Ref3 = payment.Ref3;
            res.PaymentStatus = payment.Status;
            res.RemainingAmount = payment.RemainingAmount();
            res.GrandTotal = payment.GrandTotal();
            res.TotalVatAmount = payment.TotalVat();
            res.TotalWithholdingTaxAmount = payment.TotalWithholdingTax();
            res.TotalBeforeAdjustment = payment.TotalNoDiscount();
            res.TotalAdjustment = payment.TotalDiscount();
            res.PaymentItems = payment.PaymentItems.CreatePaymentResponseItem();
        }

        public static void SetPaymentResponse(this GetPaymentResponse res, Payment payment)
        {
            res.PaymentId = payment.Id;
            res.PaymentCode = payment.PaymentCode;
            res.CreateBy = payment.CreatedBy;
            res.CreateDate = payment.CreatedDate;
            res.UpdateBy = payment.UpdatedBy;
            res.UpdateDate = payment.UpdatedDate;
            res.CustomerIDMPartyID = payment.CustomerIdmPartyId;
            res.CustomerCode = null;
            res.CustomerName = payment.CustomerName;
            res.CustomerAddress = payment.CustomerAddress;
            res.CustomerAccountNumber = payment.CustomerRefundAccountNo;
            res.CustomerAccountName = payment.CustomerRefundAccountName;
            res.CustomerAccountBankId = payment.CustomerRefundBankId;
            res.CustomerMobilePhoneNumber = payment.CustomerMobilePhoneNo;
            res.Remark = payment.Remark;
            res.Ref1 = payment.Ref1;
            res.Ref2 = payment.Ref2;
            res.Ref3 = payment.Ref3;
            res.PaymentStatus = payment.Status;
            res.RemainingAmount = payment.RemainingAmount();
            res.GrandTotal = payment.GrandTotal();
            res.TotalVatAmount = payment.TotalVat();
            res.TotalWithholdingTaxAmount = payment.TotalWithholdingTax();
            res.TotalBeforeAdjustment = payment.TotalNoDiscount();
            res.TotalAdjustment = payment.TotalDiscount();

            if (payment.PaymentItems.Count > 0)
            {
                var items = payment.PaymentItems.ToArray();
                res.PaymentItems = new GetPaymentItemResponse[items.Count()];
                for (int i = 0; i < items.Length; i++)
                {
                    res.PaymentItems[i] = new GetPaymentItemResponse
                    {
                        //ServiceCode = items[i].ServiceCode.ToString(),
                        //ServiceName = items[i].ServiceName,
                        ItemDescription = items[i].ItemDescription,
                        Quantity = items[i].Qty,
                        UnitAmount = items[i].UnitAmount,
                        VatPercent = items[i].VatPercent,
                        WithholdingTaxPercent = items[i].WithholdingTexPercent,
                        WithholdingTaxAmount = items[i].WithHoldingTaxAmount(),
                        IsRevenue = items[i].ServiceIsRevenue,
                        IsLegalPerson = items[i].IsLegalPerson,
                        VatAmount = items[i].VATAmount(),
                        Remark = items[i].Remark,
                        SubTotal = items[i].SubTotal(),
                        PaymentItemId = items[i].Id,
                        GroupRef1 = items[i].GroupRef1,
                        GroupRef2 = items[i].GroupRef2,
                        GroupRef3 = items[i].GroupRef3,
                        NetTotal = items[i].NetTotal()
                    };
                }
            }
        }

        public static void SetPaymentResponse(this GetPaymentCodeResponse res, Payment payment)
        {
            res.PaymentId = payment.Id;
            res.PaymentCode = payment.PaymentCode;
            res.CreateBy = payment.CreatedBy;
            res.CreateDate = payment.CreatedDate;
            res.UpdateBy = payment.UpdatedBy;
            res.UpdateDate = payment.UpdatedDate;
            res.CustomerIDMPartyID = payment.CustomerIdmPartyId;
            res.CustomerCode = null;
            res.CustomerName = payment.CustomerName;
            res.CustomerAddress = payment.CustomerAddress;
            res.CustomerAccountNumber = payment.CustomerRefundAccountNo;
            res.CustomerAccountName = payment.CustomerRefundAccountName;
            res.CustomerAccountBankId = payment.CustomerRefundBankId;
            res.CustomerMobilePhoneNumber = payment.CustomerMobilePhoneNo;
            res.Remark = payment.Remark;
            res.Ref1 = payment.Ref1;
            res.Ref2 = payment.Ref2;
            res.Ref3 = payment.Ref3;
            res.PaymentStatus = payment.Status;
            res.RemainingAmount = payment.RemainingAmount();
            res.GrandTotal = payment.GrandTotal();
            res.TotalVatAmount = payment.TotalVat();
            res.TotalWithholdingTaxAmount = payment.TotalWithholdingTax();
            res.TotalBeforeAdjustment = payment.TotalNoDiscount();
            res.TotalAdjustment = payment.TotalDiscount();

            var input = payment.PaymentItems.ToArray();
            var result = new PaymentItemResponse[input.Count()];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new PaymentItemResponse
                {
                    //ServiceCode = input[i].ServiceCode.ToString(),
                    //ServiceName = input[i].ServiceName,
                    ItemDescription = input[i].ItemDescription,
                    Quantity = input[i].Qty,
                    UnitAmount = input[i].UnitAmount,
                    VatPercent = input[i].VatPercent,
                    WithholdingTaxPercent = input[i].WithholdingTexPercent,
                    WithholdingTaxAmount = input[i].WithHoldingTaxAmount(),
                    IsRevenue = input[i].ServiceIsRevenue,
                    IsLegalPerson = input[i].IsLegalPerson,
                    VatAmount = input[i].VATAmount(),
                    Remark = input[i].Remark,
                    SubTotal = input[i].SubTotal(),
                    PaymentItemId = input[i].Id,
                    GroupRef1 = input[i].GroupRef1,
                    GroupRef2 = input[i].GroupRef2,
                    GroupRef3 = input[i].GroupRef3,
                    NetTotal = input[i].NetTotal()
                };
            }
        }
    }
}
