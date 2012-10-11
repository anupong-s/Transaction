using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransactionModel.Utils
{
    public enum FileType { E = 1, C = 2, P = 3 };

    public enum TransactionBanks { BBL, KBANK, SCB };

    //public enum PaymentStatusEnum
    //{
    //    UNPAID,
    //    UNDERPAID,
    //    PAID,
    //    OVERPAID
    //}

    public class Constants
    {
        public static string SystemName = "TRANSACTION";
        public static string UnknownStr = "Unknown";
        public static string NotSpecifiedStr = "NotSpecified";

        public static DateTime MinPersistableDate = new DateTime(1753, 1, 1);
        public static DateTime MaxPersistableDate = new DateTime(9999, 12, 31);
    }
}
