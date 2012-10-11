using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransactionModel.Utils;
using CarPass.Transaction.Common;

namespace TransactionModel
{
    //public enum PaymentMethodEnum
    //{
    //    PAY_IN_SLIP,
    //    CREDIT_CARD
    //}

    public partial class Installment
    {
        #region Partial Methods

        partial void OnInstallmentDateChanging(DateTime value)
        {
            if (!value.IsPersistable())
            {
                throw new ArgumentException("ISSUED_DATE_NOT_PERSISTABLE");
            }
        }

        partial void OnInstallmentByChanging(String value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("NULL_OR_EMPTY_INSTALLMENT_BY");
            }

            if (value.Length > 50)
            {
                throw new ArgumentException("INSTALLMENT_BY_TOO_LONG");
            }
        }

        partial void OnInstallmentByChanged()
        {
            if (!string.IsNullOrEmpty(_InstallmentBy))
            {
                _InstallmentBy = InstallmentBy.TextEncode();
            }
        }

        partial void OnMethodChanging(String value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("NULL_OR_EMPTY_METHOD");
            }

            if (value.Length > 20)
            {
                throw new ArgumentException("METHOD_TOO_LONG");
            }
        }

        partial void OnMethodChanged()
        {
            if (!string.IsNullOrEmpty(_Method))
            {
                _Method = Method.TextEncode();
            }
        }

        #endregion

        #region Constructors

        internal Installment() { }

        public Installment(string installedBy, decimal amount,
            PaymentMethodEnum method, long? reconciliationId)
        {
            InstallmentDate = DateTime.Now;
            InstallmentBy = installedBy;
            Amount = amount;
            Method = method.ToString();
            ReconciliationId = reconciliationId;
        }

        #endregion

        #region Public Methods

        #endregion
    }
}
