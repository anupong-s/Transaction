using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransactionModel.Utils;
using TransactionCommon;

namespace TransactionModel
{
    public partial class PayInSlip
    {
        partial void OnIssuedDateChanging(DateTime value)
        {
            if (!value.IsPersistable())
            {
                throw new ArgumentException("ISSUED_DATE_NOT_PERSISTABLE");
            }
        }

        partial void OnIssuedByChanging(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("NULL_OR_EMPTY_ISSUED_BY");
            }

            if (value.Length > 50)
            {
                throw new ArgumentException("ISSUED_BY_TOO_LONG");
            }
        }

        partial void OnIssuedByChanged()
        {
            if (!string.IsNullOrEmpty(_IssuedBy))
            {
                _IssuedBy = _IssuedBy.TextEncode();
            }
        }

        partial void OnSlipPdfChanging(byte[] value)
        {
            if (value.Count() == 0 || value == null)
            {
                throw new ArgumentException("NULL_OR_EMPTY_SLIP_PDF");
            }
        }


        #region Constractor

        internal PayInSlip()
        {

        }

        public PayInSlip(DateTime issuedDate, string issuedBy, byte[] slipPDF, int paymentId, string documentNumber)
        {
            IssuedDate = issuedDate;
            IssuedBy = issuedBy;
            SlipPdf = slipPDF;
            PaymentId = paymentId;
            DocumentNumber = documentNumber;
        }

        #endregion
    }
}
