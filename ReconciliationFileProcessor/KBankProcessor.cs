using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransactionModel;
using System.IO;
using TransactionModel.Utils;
using ReconciliationFileProcessor.KBankExtension;

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
            ReadStream(rec);
        }

        private void PayInSlip(ReconciliationFile rec)
        {
            ReadStream(rec);
        }

        private void ReadStream(ReconciliationFile rec)
        {
            var reconciliations = new List<Reconciliation>();
            var kbanks = new List<KBankDto>();

            using (var stream = new StreamReader(new MemoryStream(rec.Contents), Encoding.Default))
            {
                string str = stream.ReadLine();
                while (str != null)
                {
                    var flag = str.AsRecordType();
                    if (flag.ToUpper().Equals("D")) //Detail
                    {
                        var recDto = new Reconciliation();
                        recDto.ReconciliationFileId = rec.Id;
                        recDto.PaymentMethod = rec.FileType;

                        var paymentDate = str.AsPaymentDate();
                        var paymentTime = str.AsPaymentTime();

                        recDto.PaymentCode = str.AsCustomerNo_Ref1();
                        recDto.PaymentDate = ConvertToDateTime(paymentDate, paymentTime);
                        recDto.PaymentBy = str.AsCustomerName();
                        recDto.Amount = str.AsAmount();

                        reconciliations.Add(recDto);

                        #region Detail
                        //kbanks.Add(new KBankDto()
                        //{
                        //    RecordType = str.AsRecordType(),
                        //    SequenceNo = str.AsSequenceNo(),
                        //    BankCode = str.AsBankCode(),
                        //    CompanyAccount = str.AsCompanyAccount(),
                        //    PaymentDate = str.AsPaymentDate(),
                        //    PaymentTime = str.AsPaymentTime(),
                        //    CustomerName = str.AsCustomerName(),
                        //    CustomerNo_Ref1 = str.AsCustomerNo_Ref1(),
                        //    Ref2 = str.AsRef2(),
                        //    Ref3 = str.AsRef3(),
                        //    BranchNo = str.AsBranchNo(),
                        //    TellerNo = str.AsTellerNo(),
                        //    KindOfTransaction = str.AsKindOfTransaction(),
                        //    TransactionCode = str.AsTransactionCode(),
                        //    ChequeNo = str.AsChequeNo(),
                        //    Amount = str.AsAmount().ToString(),
                        //    ChequeBankCode = str.AsChequeBankCode(),
                        //});
                        #endregion

                    }

                    str = stream.ReadLine();
                }
            }

            if (reconciliations.Count > 0)
            {
                SaveReconcilation(reconciliations.ToArray());
                ReconciliationFile.UpdateSuccessReconciliationFile(rec.Id);
            }
        }

        private DateTime ConvertToDateTime(string date, string time)
        {
            string day = date.Substring(0, 2);
            string month = date.Substring(2, 2);
            string year = date.Substring(4, 4);

            string hour = time.Substring(0, 2);
            string minute = time.Substring(2, 2);
            string second = time.Substring(4, 2);

            var dateTime = new DateTime(year.ToInt(), month.ToInt(), day.ToInt(), hour.ToInt(), minute.ToInt(), second.ToInt());

            return dateTime;
        }

        public void SaveReconcilation(Reconciliation[] recons)
        {
            foreach (Reconciliation recon in recons)
                Reconciliation.SaveReconciliation(recon);
        }
    }

    public class KBankDto
    {
        public string RecordType { get; set; }
        public string SequenceNo { get; set; }
        public string BankCode { get; set; }
        public string CompanyAccount { get; set; }
        public string PaymentDate { get; set; }
        public string PaymentTime { get; set; }
        public string CustomerName { get; set; }
        public string CustomerNo_Ref1 { get; set; }
        public string Ref2 { get; set; }
        public string Ref3 { get; set; }
        public string BranchNo { get; set; }
        public string TellerNo { get; set; }
        public string KindOfTransaction { get; set; }
        public string TransactionCode { get; set; }
        public string ChequeNo { get; set; }
        public string Amount { get; set; }
        public string ChequeBankCode { get; set; }
        public string Spare { get; set; }
    }
}

namespace ReconciliationFileProcessor.KBankExtension
{
    internal static class KBankExtension
    {
        private static string AddDecimalPoint(string value)
        {
            if (value.Length >= 4)
            {
                var length = value.Length - 2;
                return value.Insert(length, ".");
            }
            else
            {
                return value;
            }
        }

        public static string AsCompanyName(this string value)
        {
            return value.Substring(20, 40).Trim();
        }

        #region Detail

        public static string AsRecordType(this string value)
        {
            return value.Substring(0, 1).Trim();
        }

        public static string AsSequenceNo(this string value)
        {
            return value.Substring(1, 6).Trim();
        }

        public static string AsBankCode(this string value)
        {
            return value.Substring(7, 3).Trim();
        }

        public static string AsCompanyAccount(this string value)
        {
            return value.Substring(10, 10).Trim();
        }

        public static string AsPaymentDate(this string value)
        {
            return value.Substring(20, 8).Trim();
        }

        public static string AsPaymentTime(this string value)
        {
            return value.Substring(28, 6).Trim();
        }

        public static string AsCustomerName(this string value)
        {
            return value.Substring(34, 50).Trim();
        }

        public static string AsCustomerNo_Ref1(this string value)
        {
            return value.Substring(84, 20).Trim();
        }

        public static string AsRef2(this string value)
        {
            return value.Substring(104, 20).Trim();
        }

        public static string AsRef3(this string value)
        {
            return value.Substring(124, 20).Trim();
        }

        public static string AsBranchNo(this string value)
        {
            return value.Substring(144, 4).Trim();
        }

        public static string AsTellerNo(this string value)
        {
            return value.Substring(148, 4).Trim();
        }

        public static string AsKindOfTransaction(this string value)
        {
            return value.Substring(152, 1).Trim();
        }

        public static string AsTransactionCode(this string value)
        {
            return value.Substring(153, 3).Trim();
        }

        public static string AsChequeNo(this string value)
        {
            return value.Substring(156, 7).Trim();
        }

        public static string AsChequeBankCode(this string value)
        {
            return value.Substring(176, 3).Trim();
        }

        public static string AsSpare(this string value)
        {
            return value.Substring(179, 10).Trim();
        }

        public static bool IsCreditCard(this string value)
        {
            string flag = value.AsTransactionCode();
            return flag.ToUpper().Equals("CD");
        }

        public static decimal AsAmount(this string value)
        {
            var amount = AddDecimalPoint(value.Substring(163, 13).Trim());

            decimal result;
            if (decimal.TryParse(amount, out result))
            {
                return result;
            }
            else
            {
                return 0.00M;
            }
        }
        
        #endregion

        #region Total
        public static decimal AsTotalDebitAmount(this string value)
        {
            var amount = value.Substring(20, 13).Trim();

            decimal result;
            if (decimal.TryParse(amount, out result))
            {
                return result;
            }
            else
            {
                return 0.00M;
            }
        }

        public static decimal AsTotalCreditAmount(this string value)
        {
            var amount = value.Substring(39, 13).Trim();

            decimal result;
            if (decimal.TryParse(amount, out result))
            {
                return result;
            }
            else
            {
                return 0.00M;
            }
        }
        #endregion
    }
}