using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransactionModel;
using TransactionModel.Utils;
using CarPass.Transaction.Common;

namespace TransactionModel
{
    public partial class Reconciliation
    {
        public Reconciliation()
        {
        }

        public Reconciliation(string paymentCode, decimal amount, DateTime paymentDate, string paymentBy, string paymentMethod, long reconciliationFileId)
        {
            PaymentCode = paymentCode;
            Amount = amount;
            PaymentDate = paymentDate;
            PaymentBy = paymentBy;
            PaymentMethod = paymentMethod;
            ReconciliationFileId = reconciliationFileId;
        }

        #region Static Method

        public static Reconciliation GetReconciliation(long Id)
        {
            using (var container = new TransactionModelContainer())
            {
                return container.Reconciliations.FirstOrDefault(x => x.Id == Id);
            }
        }

        public static Reconciliation GetReconciliation(long Id, bool isRead)
        {
            using (var container = new TransactionModelContainer())
            {
                return container.Reconciliations.FirstOrDefault(x => x.Id == Id && x.IsRead == isRead);
            }
        }

        public static bool SaveReconciliation(Reconciliation reconciliation)
        {
            Exception exception = null;
            try
            {
                using (var container = new TransactionModelContainer())
                {
                    container.Reconciliations.AddObject(reconciliation);
                    container.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                CreateLogs(ex);
                exception = ex;
            }

            if (exception != null)
                return false;
            else
                return true;
        }

        
        #endregion

        #region Partial Method

        partial void OnPaymentCodeChanging(String value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("NULL_OR_EMPTY_PAYMENT_CODE");
            }

            if (value.Length > 20)
            {
                throw new ArgumentException("PAYMENT_CODE_TOO_LONG");
            }
        }

        partial void OnPaymentCodeChanged()
        {
            if (!string.IsNullOrEmpty(_PaymentCode))
            {
                _PaymentCode = PaymentCode.TextEncode();
            }
        }

        partial void OnPaymentDateChanging(DateTime value)
        {
            if (!value.IsPersistable())
            {
                throw new ArgumentException("PAYMENT_DATE_NOT_PERSISTABLE");
            }
        }

        partial void OnPaymentByChanging(String value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("NULL_OR_EMPTY_PAYMENT_BY");
            }

            if (value.Length > 50)
            {
                throw new ArgumentException("PAYMENT_BY_TOO_LONG");
            }
        }

        partial void OnPaymentByChanged()
        {
            if (!string.IsNullOrEmpty(_PaymentBy))
            {
                _PaymentBy = PaymentBy.TextEncode();
            }
        }

        partial void OnPaymentMethodChanging(String value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("NULL_OR_EMPTY_PAYMENT_METHOD");
            }

            if (value.Length > 1)
            {
                throw new ArgumentException("PAYMENT_METHOD_TOO_LONG");
            }
        }

        partial void OnPaymentMethodChanged()
        {
            if (!string.IsNullOrEmpty(_PaymentMethod))
            {
                _PaymentMethod = PaymentMethod.TextEncode();
            }
        }

        #endregion

        #region Private Method

        private static void CreateLogs(Exception ex)
        {
            string message = ex.Message;
            if (ex.InnerException != null)
                message = ex.InnerException.Message;

            ErrorLog.CreateErrorLog("System", ex.Message, SeverityEnum.HIGH, SystemError.ServiceProcessor);
        }

        #endregion
    }
}
