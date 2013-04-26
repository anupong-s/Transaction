using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TransactionModel;
using TransactionModel.Utils;

namespace ReconciliationFileProcessor
{
    public class SCBProcessor : IBankProcessor
    {
        public void Reader(ReconciliationFile rec)
        {
            if (rec.FileType == FileType.C.ToString())
            {
                CreditCard(rec);
            }
            else if (rec.FileType == FileType.P.ToString())
            {
                PayInSlip(rec);
            }
        }

        private void CreditCard(ReconciliationFile rec)
        {
            throw new NotImplementedException("SCB_CAN_NOT_PAY_CREDIT_CARD");
        }

        private void PayInSlip(ReconciliationFile rec)
        {
            ReadStream(rec);
        }

        private void ReadStream(ReconciliationFile rec)
        {
            var reconciliations = ReadContents(rec);

            if (reconciliations.Count > 0)
            {
                SaveReconcilation(reconciliations.ToArray());
                ReconciliationFile.UpdateSuccessReconciliationFile(rec.Id);
            }
        }

        private static List<Reconciliation> ReadContents(ReconciliationFile rec)
        {
            var reconciliations = new List<Reconciliation>();
            using (var stream = new StreamReader(new MemoryStream(rec.Contents), Encoding.Default))
            {
                var str = stream.ReadLine();
                while (str != null)
                {
                    var flag = str.AsRecordType();
                    if (flag.ToUpper().Equals("D")) //Detail
                    {
                        var recDto = new Reconciliation
                            {
                                ReconciliationFileId = rec.Id,
                                PaymentMethod = rec.FileType,
                                PaymentCode = str.AsCustomerNoRef1(),
                                PaymentDate = str.AsPaymentDateTime(),
                                PaymentBy = str.AsCustomerName(),
                                Amount = str.AsAmount()
                            };

                        reconciliations.Add(recDto);
                    }

                    str = stream.ReadLine();
                }
            }
            return reconciliations;
        }

        public void SaveReconcilation(Reconciliation[] recons)
        {
            foreach (var recon in recons)
            {
                Reconciliation.SaveReconciliation(recon);
            }
        }
    }
}
