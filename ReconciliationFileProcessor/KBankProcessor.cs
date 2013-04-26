using System;
using System.Collections.Generic;
using System.Text;
using TransactionModel;
using System.IO;
using TransactionModel.Utils;

namespace ReconciliationFileProcessor
{
    public class KBankProcessor : IBankProcessor
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
            throw new NotImplementedException("KBANK_CAN_NOT_PAY_CREDIT_CARD");
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
                        var recDto = new Reconciliation { ReconciliationFileId = rec.Id, PaymentMethod = rec.FileType };

                        //var paymentDate = str.AsPaymentDate();
                        //var paymentTime = str.AsPaymentTime();

                        recDto.PaymentCode = str.AsCustomerNoRef1();
                        recDto.PaymentDate = str.AsPaymentDateTime(); //ConvertToDateTime(paymentDate, paymentTime);
                        recDto.PaymentBy = str.AsCustomerName();
                        recDto.Amount = str.AsAmount();

                        reconciliations.Add(recDto);
                    }

                    str = stream.ReadLine();
                }
            }
            return reconciliations;
        }

        //private static DateTime ConvertToDateTime(string date, string time)
        //{
        //    var day = date.Substring(0, 2);
        //    var month = date.Substring(2, 2);
        //    var year = date.Substring(4, 4);

        //    var hour = time.Substring(0, 2);
        //    var minute = time.Substring(2, 2);
        //    var second = time.Substring(4, 2);

        //    var dateTime = new DateTime(year.ToInt(), month.ToInt(), day.ToInt(),
        //                                hour.ToInt(), minute.ToInt(), second.ToInt());

        //    return dateTime;
        //}

        public void SaveReconcilation(Reconciliation[] recons)
        {
            foreach (var recon in recons)
            {
                Reconciliation.SaveReconciliation(recon);
            }
        }
    }
}
