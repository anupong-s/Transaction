using System.Linq;
using TransactionModel;
using TransactionServiceAdaptor.GalileoService;
using System;

namespace InstallmentUpdatingScheduler
{
    public class InstallmentUpdator
    {
        public void UpdateInstallments()
        {
            try
            {
                using (var container = new TransactionModelContainer())
                {
                    var unreadReconciliations = container.Reconciliations.Where(r => !r.IsRead).ToList();

                    if (unreadReconciliations.Count <= 0) return;

                    for (var i = 0; i < unreadReconciliations.Count(); i++)
                    {
                        var seq = (i + 1);
                        UpdateInstallmentGalileo(unreadReconciliations[i], seq);
                    }

                    #region Old Code

                    /*
                foreach (var r in unreadReconciliations)
                {
                    
                    switch (r.PaymentMethod)
                    {
                        case "C":
                            {
                                // An installment expected
                                var installment = payment.Installments.Where(i => !i.ReconciliationId.HasValue
                                                                                          && i.Method == PaymentMethodEnum.CREDIT_CARD.ToString()
                                                                                          && i.Amount >= r.Amount
                                                                                          && (r.PaymentDate - i.InstallmentDate).Days <= 2).FirstOrDefault();
                                if (installment != null)
                                {
                                    installment.ReconciliationId = r.Id;
                                    r.IsRead = true;
                                }
                            }
                            break;
                        case "P":
                            payment.AddInstallments(r.PaymentBy,
                                                    new Installment(r.PaymentBy, r.Amount,
                                                                    r.PaymentMethod == "C" ? PaymentMethodEnum.CREDIT_CARD : PaymentMethodEnum.PAY_IN_SLIP,
                                                                    r.Id));
                            r.IsRead = true;
                            break;
                    }
                }
                     */

                    #endregion
                }
            }
            catch(Exception exception)
            {
                ErrorLog.CreateErrorLog("System", exception.GetBaseException().Message, 
                    SeverityEnum.HIGH, SystemError.ServiceReader);
            }
        }

        private static void UpdateInstallmentGalileo(Reconciliation reconciliation, int seq)
        {
            using (var client = new GalileoInternalServiceWcfClient())
            {
                client.UpdateInstallment(new UpdateInstallmentRequest()
                                        {
                                            PaymentCode = reconciliation.PaymentCode,
                                            Amount = reconciliation.Amount,
                                            InstallmentSeq = (byte)seq,
                                            Method = reconciliation.PaymentMethod,
                                            ReconciliationId = reconciliation.Id,
                                            InstallmentBy = reconciliation.PaymentBy,
                                            InstallmentDate = reconciliation.PaymentDate
                                        });
            }
        }
    }
}
