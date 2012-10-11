using System;
using System.Collections.Generic;
using System.Linq;
using TransactionServiceInterface;
using TransactionModel;
using System.ServiceModel;
using TransactionServiceAdaptor.IDMService;
using TransactionFrontEnd.ViewPayment;
using CarPass.Transaction.Common;
using System.Transactions;

namespace TransactionServiceImpl
{
    [ServiceBehavior(TransactionIsolationLevel = IsolationLevel.Serializable,
        TransactionTimeout = "00:00:45")]
    public partial class TransactionService : ITransactionService
    {
        #region CreatePayment

        [OperationBehavior(TransactionScopeRequired = true)]
        public CreatePaymentResponse CreatePayment(CreatePaymentRequest req)
        {
            var res = new CreatePaymentResponse();
            try
            {
                using (var idmClient = new IDMServiceClient())
                using (var container = new TransactionModelContainer())
                {
                    ValidateCreatePaymentRequest(idmClient, req);
                    var payment = new Payment(
                        req.CreateBy,
                        req.CustomerIDMPartyID,
                        req.CustomerName,
                        req.CustomerAddress,
                        req.PaymentItems.CreatePaymentItems()
                        );

                    var grandTotal = payment.GrandTotal();
                    var credits = container.CustomerCredits.Where(x =>
                            x.CustomerIdmPartyId == req.CustomerIDMPartyID &&
                            x.IsUsedOrRefund == false).ToList();

                    var credit = credits.GetCustomerCredit(grandTotal);

                    foreach (var c in credit)
                        payment.AddPaymentItem(PaymentItem.CreateCustomerCredit(c.Id, c.Amount));

                    CustomerCredit.UpdateCustomerCredit(credit);

                    container.Payments.AddObject(payment);
                    container.SaveChanges();

                    res.SetPaymentResponse(payment);
                    res.Succeed();
                }
            }
            catch (Exception x)
            {
                res.Fail(x);
                CreateLog(req, x);
            }

            return res;
        }

        #endregion

        #region GetPaymentStatus

        public GetPaymentStatusResponse GetPaymentStatusByPaymentId(GetPaymentStatusRequest req)
        {
            var res = new GetPaymentStatusResponse();
            try
            {
                long id = long.Parse(req.PaymentId);
                using (var container = new TransactionModelContainer())
                {
                    var payment = container.Payments.FirstOrDefault(x => x.Id == id);
                    res.PaymentStatus = (PaymentStatusEnum)Enum.Parse(typeof(PaymentStatusEnum), payment.Status);
                    res.Succeed();
                }
            }
            catch (Exception ex)
            {
                res.Fail(ex);
                CreateLog(ex);
            }

            return res;
        }

        #endregion

        #region GetPaymentByPaymentId

        public GetPaymentResponse GetPaymentByPaymentId(GetPaymentRequest req)
        {
            var res = new GetPaymentResponse();
            try
            {
                long id = long.Parse(req.PaymentId);
                using (var container = new TransactionModelContainer())
                {
                    var payment = container.Payments
                                    .Include("PaymentItems")
                                    .FirstOrDefault(x => x.Id == id);

                    if (payment == null) throw new Exception("Payment is not found");

                    res.SetPaymentResponse(payment);
                    res.Succeed();
                }
            }
            catch (Exception ex)
            {
                res.Fail(ex);
                CreateLog(ex);
            }
            return res;
        }

        #endregion

        #region GetPaymentByPaymentCode

        public GetPaymentCodeResponse GetPaymentByPaymentCode(GetPaymentCodeRequest req)
        {
            var res = new GetPaymentCodeResponse();
            try
            {
                using (var container = new TransactionModelContainer())
                {
                    var payment = container.Payments
                                    .Include("PaymentItems")
                                    .FirstOrDefault(x => x.PaymentCode == req.PaymentCode);

                    res.SetPaymentResponse(payment);

                    res.Succeed();
                }
            }
            catch (Exception ex)
            {
                res.Fail(ex);
                CreateLog(ex);
            }

            return res;
        }

        #endregion

        #region CalculatePayment

        public CalculatePaymentResponse CalculatePayment(CalculatePaymentRequest req)
        {
            var res = new CalculatePaymentResponse();
            try
            {
                using (var container = new TransactionModelContainer())
                {
                    var payment = new Payment("SystemService", -1,
                        req.CustomerName, "CARPASS", req.PaymentItems.CreatePaymentItems());

                    res.CustomerName = payment.CustomerName;
                    res.GrandTotal = payment.GrandTotal();
                    res.RemainingAmount = payment.RemainingAmount();
                    res.TotalVatAmount = payment.TotalVat();
                    res.TotalWithholdingTaxAmount = payment.TotalWithholdingTax();
                    res.TotalBeforeAdjustment = payment.TotalNoDiscount();
                    res.TotalAdjustment = payment.TotalDiscount();
                    res.PaymentItems = payment.PaymentItems.CreateCalculatePaymentItems();

                    res.Succeed();
                }
            }
            catch (Exception ex)
            {
                res.Fail(ex);
                CreateLog(ex);
            }
            return res;
        }

        #endregion

        #region GetTaxInvoiceReceipt

        public GetTaxInvoiceReceiptResponse GetTaxInvoiceReceiptByPaymentId(GetTaxInvoiceReceiptRequest req)
        {
            var res = new GetTaxInvoiceReceiptResponse();
            try
            {
                long paymentId = long.Parse(req.PaymentId);
                var pdf = TaxInvoiceReceiptReport.CreateTaxInvoiceReceipt(paymentId);
                res.TaxInvoicePDF = pdf;

                res.Succeed();
            }
            catch (Exception ex)
            {
                res.Fail(ex);
                CreateLog(ex);
            }

            return res;
        }

        #endregion

        #region GetPaymentSummary

        public GetPaymentSummaryResponse GetPaymentSummary(GetPaymentSummaryRequest req)
        {
            var res = new GetPaymentSummaryResponse();
            try
            {
                using (var container = new TransactionModelContainer())
                {
                    var payment = GetPaymentByPaymentId(new GetPaymentRequest { PaymentId = req.PaymentId.ToString() });
                    if (payment.IsSuccessful)
                    {
                        var installment = container.Installments.Where(i => i.PaymentId == req.PaymentId).ToList();
                        bool Ispaid = installment.Count() > 0;

                        var paidAmount = (Ispaid) ? (double)installment.Sum(i => i.Amount) : 0.00;

                        var remainingAmount = payment.RemainingAmount;
                        var amount = payment.GrandTotal;

                        res.PaymentId = req.PaymentId;
                        res.IssuedDate = payment.CreateDate;
                        res.PaymentStatus = payment.PaymentStatus;
                        res.Amount = (double)amount;
                        res.PaidAmount = (double)paidAmount;
                        res.RemainingAmount = (double)remainingAmount;
                    }
                    else
                    {
                        throw new Exception(payment.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                res.Fail(ex);
                CreateLog(ex);
            }

            return res;
        }

        #endregion

        #region GetReconcilliationByPaymentCode

        [OperationBehavior(TransactionScopeRequired = true)]
        public GetReconciliationByPaymentCodeResponse[] GetReconciliationByPaymentCode(GetReconciliationByPaymentCodeRequest[] req)
        {
            var result = new List<GetReconciliationByPaymentCodeResponse>();
            using (var ctx = new TransactionModelContainer())
            {
                //var pCode = req.Select(s => s.PaymentCode).ToArray();
                //var recons = ctx.Reconciliations.Where(s => pCode.Contains(s.PaymentCode) && s.IsRead == false).ToList();
                var recons = ctx.Reconciliations.Where(s => s.IsRead == false).ToList();
                if (recons.Count > 0)
                {
                    foreach (var r in recons)
                    {
                        r.IsRead = true;
                        var paymentMethod = r.PaymentMethod.ToUpper() == "C" ? PaymentMethodEnum.CREDIT_CARD : PaymentMethodEnum.PAY_IN_SLIP;

                        //var payment = ctx.Payments.Select(x => new { PaymentId = x.Id, PaymentCode = x.PaymentCode })
                        //                    .FirstOrDefault(s => s.PaymentCode == r.PaymentCode);

                        result.Add(new GetReconciliationByPaymentCodeResponse
                        {
                            ReconciliationId = r.Id,
                            //PaymentId = payment.PaymentId,
                            PaymentCode = r.PaymentCode,
                            PaymentDate = r.PaymentDate,
                            PaymentBy = r.PaymentBy,
                            PaymentMethod = paymentMethod,
                            Amount = r.Amount,
                        });
                    }
                    ctx.SaveChanges();
                }
            }

            return result.ToArray();
        }

        #endregion

        #region RetrievePaymentCode

        public RetrievePaymentCodeResponse RetrievePaymentCode(RetrievePaymentCodeRequest req)
        {
            var res = new RetrievePaymentCodeResponse();

            using(var ctx = new TransactionModelContainer())
            {
                res.PaymentCode = PaymentCode.NextPaymentCode(ctx);
                ctx.SaveChanges();
            }

            return res;
        }

        #endregion

        #region Private Method

        private void CreateLog(CreatePaymentRequest req, Exception x)
        {
            string msg = x.GetBaseException().Message;

            ErrorLog.CreateErrorLog(
                req.CustomerName, msg,
                SeverityEnum.HIGH,
                SystemError.TransactionService);
        }

        private void CreateLog(Exception x)
        {
            string msg = x.GetBaseException().Message;

            ErrorLog.CreateErrorLog("System", msg,
                SeverityEnum.HIGH,
                SystemError.TransactionService);
        }

        private void ValidateCreatePaymentRequest(IDMServiceClient client, CreatePaymentRequest req)
        {

            if (req == null) throw new ApplicationException("MISSING_REQUIRED_PARAMETER(S)");

            if (string.IsNullOrEmpty(req.CreateBy)) throw new ApplicationException("EMPTY_OR_NULL_CREATED_BY");

            if (string.IsNullOrEmpty(req.CustomerName)) throw new ApplicationException("EMPTY_OR_NULL_CUSTOMERNAME");

            if (string.IsNullOrEmpty(req.CustomerAddress)) throw new ApplicationException("EMPTY_OR_NULL_CUSTOMERADDRESS");

            if (req.PaymentItems == null || req.PaymentItems.Length < 1)
                throw new ApplicationException("AT_LEAST_ONE_PAYMENT_ITEM_IS_REQUIRED");
        }

        #endregion

    }
}
