namespace ReconciliationFileProcessor
{
    using System;

    using TransactionModel.Utils;

    /// <summary>
    /// This extension use for KBank and SCB
    /// </summary>
    internal static class FileProcessorExtension
    {
        private static string AddDecimalPoint(string value)
        {
            if (value.Length < 4)
            {
                return value;
            }

            var length = value.Length - 2;
            return value.Insert(length, ".");
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

        public static DateTime AsPaymentDateTime(this string value)
        {
            var dt = value.Substring(20, 14).Trim();

            var date = dt.Substring(0, 8).Trim();
            var time = dt.Substring(8, 6).Trim();

            return ConvertToDateTime(date, time);
        }

        private static DateTime ConvertToDateTime(string date, string time)
        {
            var day = date.Substring(0, 2);
            var month = date.Substring(2, 2);
            var year = date.Substring(4, 4);

            var hour = time.Substring(0, 2);
            var minute = time.Substring(2, 2);
            var second = time.Substring(4, 2);

            var dateTime = new DateTime(year.ToInt(), month.ToInt(), day.ToInt(),
                                        hour.ToInt(), minute.ToInt(), second.ToInt());

            return dateTime;
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

        public static string AsCustomerNoRef1(this string value)
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
            var flag = value.AsTransactionCode();
            return flag.ToUpper().Equals("CD");
        }

        public static decimal AsAmount(this string value)
        {
            var amount = AddDecimalPoint(value.Substring(163, 13).Trim());

            decimal result;
            return decimal.TryParse(amount, out result) ? result : 0.00M;
        }

        #endregion

        #region Total

        public static decimal AsTotalDebitAmount(this string value)
        {
            var amount = value.Substring(20, 13).Trim();

            decimal result;
            return decimal.TryParse(amount, out result) ? result : 0.00M;
        }

        public static decimal AsTotalCreditAmount(this string value)
        {
            var amount = value.Substring(39, 13).Trim();

            decimal result;
            return decimal.TryParse(amount, out result) ? result : 0.00M;
        }

        #endregion
    }
}
