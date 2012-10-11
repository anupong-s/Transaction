using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransactionFrontEnd.Common;
using TransactionModel;
using TransactionModel.Reports;
using Telerik.Reporting.Processing;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TransactionModel.Utils;
using TransactionServiceAdaptor.IDMService;
using TransactionCommon;

namespace TransactionFrontEnd.ViewPayment
{
    [Serializable]
    public class ViewPaymentPaymentItems
    {
        public virtual string No { get; set; }
        public virtual string Group { get; set; }
        public virtual string Name { get; set; }
        public virtual string Quantity { get; set; }
        public virtual string UnitPrice { get; set; }
        public virtual string SubTotal { get; set; }
        public virtual string WithholdingTaxPercent { get; set; }
        public virtual string WithholdingTaxAmount { get; set; }
        public virtual string VatPercent { get; set; }
        public virtual string VatAmount { get; set; }
        public virtual string NetAmount { get; set; }
    }

    [Serializable]
    public class ViewPaymentInstallmentItems
    {
        public virtual string No { get; set; }
        public virtual string InstallmentId { get; set; }
        public virtual string InstalledDate { get; set; }
        public virtual string InstalledBy { get; set; }
        public virtual string Method { get; set; }
        public virtual string Amount { get; set; }
    }

    [Serializable]
    public class ViewPayment : AbstractViewModel
    {
        public virtual string No { get; set; }
        public virtual string date { get; set; }
        public virtual string ReferenceId { get; set; }
        public virtual string CustomerIdmId { get; set; }
        public virtual string CustomerCode { get; set; }
        public virtual string CustomerName { get; set; }
        public virtual string CustomerAddress { get; set; }
        public virtual string CustomerMobilePhoneNo { get; set; }
        public virtual string CustomerAccountNo { get; set; }
        public virtual string CustomerAccountName { get; set; }
        public virtual string Remark { get; set; }
        public virtual string PaymentId { get; set; }
        public virtual string PaymentCode { get; set; }
        public virtual string Ref1 { get; set; }
        public virtual string Ref2 { get; set; }
        public virtual string Ref3 { get; set; }
        public virtual string PaymentStatus { get; set; }
        public virtual string RemainingAmount { get; set; }
        public virtual ViewPaymentPaymentItems[] Items { get; set; }
        public virtual ViewPaymentInstallmentItems[] InstallmentItems { get; set; }

        public virtual string GrandTotal_SubTotal { get; set; }
        public virtual string GrandTotal_WH_Amount { get; set; }
        public virtual string GrandTotal_VAT_Amount { get; set; }
        public virtual string GrandTotal_Net_Total { get; set; }

        public virtual string Summary_Total { get; set; }
        public virtual string Summary_Adjustment { get; set; }

        public virtual string Installment_Total { get; set; }

        public ViewPayment()
        {
            Items = new ViewPaymentPaymentItems[0];
            InstallmentItems = new ViewPaymentInstallmentItems[0];
        }

        public void RefreshWhenDomainModelChanged(Payment payment)
        {
            string NumberFormat = ConfigurationManager.Format.Decimal_Format;

            PaymentId = payment.Id.ToString("0000000000");
            PaymentCode = payment.PaymentCode;
            PaymentStatus = payment.Status;

            CustomerIdmId = payment.CustomerIdmPartyId.ToString("0000000000");
            CustomerName = payment.CustomerName;
            CustomerAddress = payment.CustomerAddress;
            CustomerMobilePhoneNo = payment.CustomerMobilePhoneNo;
            CustomerAccountNo = payment.CustomerRefundAccountNo;
            CustomerAccountName = payment.CustomerRefundAccountName;

            Ref1 = payment.Ref1;
            Ref2 = payment.Ref2;
            Ref3 = payment.Ref3;
            Remark = payment.Remark;

            GrandTotal_SubTotal = payment.TotalUnitAmount().ToString(NumberFormat);
            GrandTotal_WH_Amount = payment.TotalWithholdingTax().ToString(NumberFormat);
            GrandTotal_VAT_Amount = payment.TotalVat().ToString(NumberFormat);
            GrandTotal_Net_Total = payment.GrandTotal().ToString(NumberFormat);

            Summary_Total = payment.TotalNoDiscount().ToString(NumberFormat);
            Summary_Adjustment = payment.TotalDiscount().ToString(NumberFormat);

            Installment_Total = payment.InstallmentAmount().ToString(NumberFormat);
            RemainingAmount = payment.RemainingAmount().ToString(NumberFormat);

            // PaymentItems
            Items = payment.PaymentItems.CreateViewPaymentItems();
            // Installments
            InstallmentItems = payment.Installments.CreateInstallment();
        }

        public void Initialize(Main main, long paymentId, long ticket)
        {
            TheMain = main;
            TheMain.SetUpUser(ticket);

            using (var container = new TransactionModelContainer())
            {
                var payment = container.Payments
                    .Include("PaymentItems")
                    .Include("Installments")
                    .FirstOrDefault(x => x.Id == paymentId && 
                                    x.CustomerIdmPartyId == TheMain.PartyId);

                if (payment == null)
                {
                    throw new ArgumentException("PAYMENT_NOT_NULL");
                }
                else
                {
                    RefreshWhenDomainModelChanged(payment);
                }
            }
        }

        public virtual byte[] CreatePayInSlip()
        {
            byte[] pdf = new byte[0];
            using (var container = new TransactionModelContainer())
            {
                long id = long.Parse(PaymentId);
                Payment payment = container.Payments.Where(x => x.Id == id).FirstOrDefault();

                if (payment != null)
                {
                    var config = container.Configurations.Where(x => x.Group == "Pay_In").ToList();

                    PayInSlipReport report = new PayInSlipReport();
                    report.DataSource = PayInSlipDetail.CreatePayInSlipSource(
                        config.Where(x => x.Name.ToLower() == "companyname").FirstOrDefault().Value1,
                        config.Where(x => x.Name.ToLower() == "companyname").FirstOrDefault().Value2,
                        config.Where(x => x.Name.ToLower() == "companyname").FirstOrDefault().Value3,
                        config.Where(x => x.Name.ToLower() == "taxno").FirstOrDefault().Value1,
                        config.Where(x => x.Name.ToLower() == "accno").FirstOrDefault().Value1,
                        config.Where(x => x.Name.ToLower() == "accno").FirstOrDefault().Value2,
                        config.Where(x => x.Name.ToLower() == "accno").FirstOrDefault().Value3,
                        DateTime.Now.ToString("dd/MM/yyyy"),
                        config.Where(x => x.Name.ToLower() == "servicecode").FirstOrDefault().Value1,
                        payment.CustomerName,
                        payment.PaymentCode,
                        payment.CustomerIdmPartyId.ToString(),
                        payment.RemainingAmount().ToString(ConfigurationManager.Format.Decimal_Format)
                    );

                    var reportprocess = new ReportProcessor();
                    var result = reportprocess.RenderReport("PDF", report, null);
                    pdf = result.DocumentBytes;

                    payment.AddPayInSlip(DateTime.Now, payment.CustomerName, pdf,
                        Convert.ToInt32(payment.Id), payment.PaymentCode);

                    container.SaveChanges();
                    RefreshWhenDomainModelChanged(payment);
                }
                else
                {
                    throw new ArgumentException("PAYMENT_NOT_FOUND");
                }
            }
            return pdf;
        }

        public virtual byte[] CreateTaxInvoiceReceipt()
        {
            byte[] pdf = new byte[0];
            using (var idmContainer = new IDMServiceClient())
            using (var container = new TransactionModelContainer())
            {
                long id = long.Parse(PaymentId);
                var payment = container.Payments.Where(x => x.Id == id).FirstOrDefault();
                if (payment != null)
                {
                    var response = idmContainer.GetPersonByPartyId(new
                            GetPersonByPartyIdRequest { PartyId = payment.CustomerIdmPartyId });

                    if (!response.IsSuccessful && response.Result == null)
                        throw new Exception("Customer party not found.");
                    if (string.IsNullOrEmpty(response.Result.CustomerCode))
                        throw new Exception("Not Customer");

                    var header = payment.CreateViewPayment();
                    header.CustomerCode = response.Result.CustomerCode;

                    TaxInvoice report = new TaxInvoice();
                    report.DataSource = header;
                    report.PaymentDataSource.DataSource = payment.PaymentItems.CreateViewPaymentItems();
                    var reportprocess = new ReportProcessor();
                    var result = reportprocess.RenderReport("PDF", report, null);
                    pdf = result.DocumentBytes;

                    payment.AddTaxInvoiceReceipt(DateTime.Now, payment.CustomerName, pdf,
                        Convert.ToInt32(payment.Id), payment.PaymentCode);

                    container.SaveChanges();

                    RefreshWhenDomainModelChanged(payment);
                }
                else
                {
                    throw new ArgumentException("PAYMENT_NOT_FOUND");
                }
            }
            return pdf;
        }

        public virtual void CreateCreditCardSuccess()
        {
            using (var container = new TransactionModelContainer())
            {
                long id = long.Parse(PaymentId);
                var payment = container.Payments.Where(x => x.Id == id).FirstOrDefault();
                if (payment != null)
                {
                    string fullname = TheMain.FirstName + " " + TheMain.LastName;
                    payment.AddInstallments(payment.CustomerName,
                        new Installment(fullname, payment.GrandTotal(), PaymentMethodEnum.CREDIT_CARD, null));

                    container.SaveChanges();

                    RefreshWhenDomainModelChanged(payment);
                }
            }
        }

        public static void LogsError(Exception ex, string issuedBy)
        {
            string msg = ex.Message;
            if (ex.InnerException != null)
                msg = ex.InnerException.Message;

            ErrorLog.CreateErrorLog(issuedBy, msg, SeverityEnum.MEDIUM, SystemError.Web);
        }
    }

    public static class PaymentExtension
    {
        public static ViewPayment CreateViewPayment(this Payment input)
        {
            return new ViewPayment
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

                Installment_Total = input.InstallmentAmount().ToString(ConfigurationManager.Format.Decimal_Format),
                RemainingAmount = input.RemainingAmount().ToString(ConfigurationManager.Format.Decimal_Format),
            };
        }

        public static ViewPaymentPaymentItems[] CreateViewPaymentItems(this IEnumerable<PaymentItem> input)
        {
            string numberFormat = ConfigurationManager.Format.NUMBER_FORMAT;

            var Items = new ViewPaymentPaymentItems[0];
            if (input.Count() > 0)
            {
                Items = new ViewPaymentPaymentItems[input.Count()];
                var item = input.ToArray();
                for (int i = 0; i < item.Count(); i++)
                {
                    Items[i] = new ViewPaymentPaymentItems
                    {
                        No = (i + 1).ToString(numberFormat),
                        Name = item[i].ItemDescription,
                        Quantity = item[i].Qty.ToString(numberFormat),
                        UnitPrice = item[i].UnitAmount.ToString(ConfigurationManager.Format.Decimal_Format),
                        SubTotal = item[i].SubTotal().ToString(ConfigurationManager.Format.Decimal_Format),
                        WithholdingTaxPercent = item[i].WithholdingTexPercent.ToString("0"),
                        WithholdingTaxAmount = item[i].WithHoldingTaxAmount().ToString(ConfigurationManager.Format.Decimal_Format),
                        VatPercent = item[i].VatPercent.ToString("0"),
                        VatAmount = item[i].VATAmount().ToString(ConfigurationManager.Format.Decimal_Format),
                        NetAmount = item[i].NetTotal().ToString(ConfigurationManager.Format.Decimal_Format),
                    };
                }
            }
            return Items;
        }

        public static ViewPaymentInstallmentItems[] CreateInstallment(this IEnumerable<Installment> input)
        {
            string numberFormat = ConfigurationManager.Format.NUMBER_FORMAT;
            var InstallmentItems = new ViewPaymentInstallmentItems[0];

            if (input.Count() > 0)
            {
                InstallmentItems = new ViewPaymentInstallmentItems[input.Count()];
                var installment = input.ToArray();
                for (int i = 0; i < installment.Count(); i++)
                {
                    InstallmentItems[i] = new ViewPaymentInstallmentItems
                    {
                        No = (i + 1).ToString(numberFormat),
                        InstallmentId = installment[i].Id.ToString("0000000000"),
                        InstalledDate = installment[i].InstallmentDate.ToString(ConfigurationManager.Format.Date_Format),
                        InstalledBy = installment[i].InstallmentBy,
                        Method = installment[i].Method,
                        Amount = installment[i].Amount.ToString(ConfigurationManager.Format.Decimal_Format),
                    };
                }
            }

            return InstallmentItems;
        }
    }
}
