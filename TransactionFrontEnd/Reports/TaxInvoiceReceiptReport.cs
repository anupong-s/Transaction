using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransactionServiceAdaptor.IDMService;
using TransactionModel;
using TransactionModel.Reports;
using Telerik.Reporting.Processing;

namespace TransactionFrontEnd.ViewPayment
{
    public static class TaxInvoiceReceiptReport
    {
        public static byte[] CreateTaxInvoiceReceipt(long paymentId)
        {
            byte[] pdf = new byte[0];

            using (var idmContainer = new IDMServiceClient())
            using (var container = new TransactionModelContainer())
            {
                try
                {
                    var payment = container.Payments.Where(x => x.Id == paymentId).FirstOrDefault();
                    if (payment != null)
                    {
                        var response = idmContainer.GetPersonByPartyId(new
                                GetPersonByPartyIdRequest { PartyId = payment.CustomerIdmPartyId });

                        if (!response.IsSuccessful && response.Result == null)
                            throw new Exception("Customer party not found.");
                        if (string.IsNullOrEmpty(response.Result.CustomerCode))
                            throw new Exception("Not Customer");

                        var header = CreateViewPayment(payment, container);
                        header.CustomerCode = response.Result.CustomerCode;

                        TaxInvoice report = new TaxInvoice();
                        report.DataSource = header;
                        report.PaymentDataSource.DataSource = CreateViewPaymentItems(payment.PaymentItems, container);
                        var reportprocess = new ReportProcessor();
                        var result = reportprocess.RenderReport("PDF", report, null);
                        pdf = result.DocumentBytes;

                        payment.AddTaxInvoiceReceipt(DateTime.Now, payment.CustomerName, pdf,
                            Convert.ToInt32(payment.Id), payment.PaymentCode);

                        container.SaveChanges();
                    }
                    else
                    {
                        throw new ArgumentException("PAYMENT_NOT_FOUND");
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.Log("System", ex, SystemError.TransactionService);
                }
            }

            return pdf;
        }

        public static ReportHeader CreateViewPayment(Payment input, TransactionModelContainer container)
        {
            return new ReportHeader
            {
                No = string.Format("GAL-{0}-{1}", DateTime.Now.ToString("yyyy-MM"), input.Id),
                date = DateTime.Now.ToString(ConfigurationManager.Format.Date_Format),
                PaymentId = input.Id.ToString("0000000000"),
                PaymentCode = input.PaymentCode,
                PaymentStatus = input.Status,

                CustomerIdmId = input.CustomerIdmPartyId.ToString("0000000000"),
                CustomerName = input.CustomerName,
                CustomerAddress = input.CustomerAddress,
                CustomerMobilePhoneNo = input.CustomerMobilePhoneNo,
                CustomerAccountNo = input.CustomerRefundAccountNo,
                CustomerAccountName = input.CustomerRefundAccountName,

                Ref1 = input.Ref1,
                Ref2 = input.Ref2,
                Ref3 = input.Ref3,
                Remark = input.Remark,

                GrandTotal_SubTotal = input.TotalUnitAmount().ToString(ConfigurationManager.Format.Decimal_Format),
                GrandTotal_WH_Amount = input.TotalWithholdingTax().ToString(ConfigurationManager.Format.Decimal_Format),
                GrandTotal_VAT_Amount = input.TotalVat().ToString(ConfigurationManager.Format.Decimal_Format),
                GrandTotal_Net_Total = input.GrandTotal().ToString(ConfigurationManager.Format.Decimal_Format),

                Summary_Total = input.TotalNoDiscount().ToString(ConfigurationManager.Format.Decimal_Format),
                Summary_Adjustment = input.TotalDiscount().ToString(ConfigurationManager.Format.Decimal_Format),

                RemainingAmount = input.RemainingAmount().ToString(ConfigurationManager.Format.Decimal_Format),
            };
        }

        public static ReportItems[] CreateViewPaymentItems(IEnumerable<PaymentItem> input, TransactionModelContainer container)
        {
            string NumberFormat = ConfigurationManager.Format.Decimal_Format;

            var Items = new ReportItems[0];
            if (input.Count() > 0)
            {
                Items = new ReportItems[input.Count()];
                var item = input.ToArray();
                for (int i = 0; i < item.Count(); i++)
                {
                    Items[i] = new ReportItems
                    {
                        No = (i + 1).ToString("#,##0"),
                        //Name = item[i].ServiceName + item[i].ItemDescription,
                        Quantity = item[i].Qty.ToString("#,##0"),
                        UnitPrice = item[i].UnitAmount.ToString(NumberFormat),
                        SubTotal = item[i].SubTotal().ToString(NumberFormat),
                        WithholdingTaxPercent = item[i].WithholdingTexPercent.ToString("0"),
                        WithholdingTaxAmount = item[i].WithHoldingTaxAmount().ToString(NumberFormat),
                        VatPercent = item[i].VatPercent.ToString("0"),
                        VatAmount = item[i].VATAmount().ToString(NumberFormat),
                        NetAmount = item[i].NetTotal().ToString(NumberFormat),
                    };
                }
            }
            return Items;
        }

    }

}
