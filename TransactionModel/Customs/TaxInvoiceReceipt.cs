using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransactionModel.Utils;
using CarPass.Transaction.Common;

namespace TransactionModel
{
    public partial class TaxInvoiceReceipt
    {
        #region Partial Method

        partial void OnIssuedDateChanging(DateTime value)
        {
            if (!value.IsPersistable())
                throw new ArgumentException("ISSUED_DATE_NOT_PERSISTABLE");
        }

        partial void OnIssuedByChanging(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("NULL_OR_EMPTY_ISSUED_BY");

            if (value.Length > 50)
                throw new ArgumentException("ISSUED_BY_TOO_LONG");
        }

        partial void OnIssuedByChanged()
        {
            if (!string.IsNullOrEmpty(_IssuedBy))
            {
                _IssuedBy = _IssuedBy.TextEncode();
            }
        }

        partial void OnTaxInvoiceReceiptPdfChanging(byte[] value)
        {
            if (value.Count() <= 0 || value == null)
                throw new ArgumentException("NULL_OR_EMPTY_TAXINVOICE_RECEIPT_PDF");
        }

        partial void OnDocumentNumberChanging(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("NULL_OR_EMPTY_DOCUMENT_NUMBER");
        }

        partial void OnDocumentNumberChanged()
        {
            if (!string.IsNullOrEmpty(_DocumentNumber))
            {
                _DocumentNumber = DocumentNumber.TextEncode();
            }
        }

        #endregion

        #region Constractor

        internal TaxInvoiceReceipt()
        {

        }

        public TaxInvoiceReceipt(DateTime issuedDate, string issuedBy, byte[] taxInvoiceReceiptPdf, int paymentId, string documentNumber)
        {
            IssuedDate = issuedDate;
            IssuedBy = issuedBy;
            TaxInvoiceReceiptPdf = taxInvoiceReceiptPdf;
            PaymentId = paymentId;
            DocumentNumber = documentNumber;
        }

        #endregion
    }
}