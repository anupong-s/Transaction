using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransactionModel;
using TransactionModel.Utils;
using System.Data;
using System.Globalization;

namespace ReconciliationFileProcessor
{
    public static class PayInSlipExtension
    {
        public static int ToPaySlipDay(this string input)
        {
            return input.Substring(20, 2).ToInt();
        }

        public static int ToPaySlipMonth(this string input)
        {
            return input.Substring(22, 2).ToInt();
        }

        public static int ToPaySlipYear(this string input)
        {
            return input.Substring(24, 4).ToInt();
        }

        public static int ToPaySlipHour(this string input)
        {
            return input.Substring(28, 2).ToInt();
        }

        public static int ToPaySlipMinute(this string input)
        {
            return input.Substring(30, 2).ToInt();
        }

        public static int ToPaySlipSecond(this string input)
        {
            return input.Substring(32, 2).ToInt();
        }

        public static string AsPayment(this string input,string value)
        {
            string val = value.ToLower();
            if (val == "paymentby")
                return input.Substring(34, 50).Trim();
            else if (val == "paymentcode")
                return input.Substring(84, 20).Trim();
            else
                return string.Empty;
        }

        public static decimal ToAmount(this string input)
        {
            return Convert.ToDecimal(input.Substring(163, 13).Trim());
        }
    }

    public class MapperReconciliation
    {
        public MapperReconciliation()
        {

        }

        public static Reconciliation[] CreateCreditCardToBBL(DataTable dt, string paymentMethod, long reconciliationFileId)
        {
            List<Reconciliation> result = new List<Reconciliation>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Reconciliation recon = new Reconciliation
                {
                    PaymentCode = dt.Rows[i]["Merchant Ref."].ToString(),
                    Amount = dt.Rows[i]["Amount"].ToString().ToDecimal(),
                    PaymentDate = Convert.ToDateTime(dt.Rows[i]["Transaction Date"].ToString(), new CultureInfo("en-US")),
                    PaymentBy = dt.Rows[i]["Name"].ToString(),
                    PaymentMethod = paymentMethod,
                    ReconciliationFileId = reconciliationFileId
                };

                result.Add(recon);
            }
            return result.ToArray();
        }

        public static Reconciliation CreatePayInSlip(string strLine, long reconciliationFileId)
        {
            try
            {
                var PaymentDate = new DateTime(strLine.ToPaySlipYear(), 
                                                strLine.ToPaySlipMonth(), 
                                                strLine.ToPaySlipDay(),
                                                strLine.ToPaySlipHour(), 
                                                strLine.ToPaySlipMinute(), 
                                                strLine.ToPaySlipSecond());

                return new Reconciliation
                { 
                    PaymentDate = PaymentDate,
                    PaymentBy = strLine.AsPayment("PaymentBy"),
                    PaymentCode = strLine.AsPayment("PaymentCode"),
                    Amount = strLine.ToAmount(),
                    PaymentMethod = FileType.P.ToString(),
                    ReconciliationFileId = reconciliationFileId,
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
