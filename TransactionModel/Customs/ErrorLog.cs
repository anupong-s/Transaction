using System;
using System.Transactions;
using TransactionModel.Utils;

namespace TransactionModel
{
    /// <summary>
    /// How severe is this error
    /// </summary>
    public enum SeverityEnum : byte
    {
        LOW,
        MEDIUM,
        HIGH
    }

    public enum SystemError : byte
    {
        ServiceReader,
        ServiceProcessor,
        Web,
        TransactionService
    }

    public partial class ErrorLog
    {
        #region Static Methods

        /// <summary>
        /// Logs an occurring error to the system. The log will actually create after a SaveChanges call.
        /// </summary>
        /// <param name="issuedBy">Issuer</param>
        /// <param name="issuedMsg">Message to log</param>
        /// <param name="severity">Severity (default = LOW)</param>
        /// <remarks>container.SaveChanges is required</remarks>
        public static ErrorLog LogsError(TransactionModelContainer container, string issuedBy, string issuedMsg,
            SeverityEnum severity = SeverityEnum.LOW)
        {
            ErrorLog log = new ErrorLog(issuedBy, issuedMsg, severity);
            container.ErrorLogs.AddObject(log);

            return log;
        }

        /// <summary>
        /// Returns erorr logs by criteria
        /// </summary>
        /// <param name="criteria">The criteria</param>
        /// <param name="skip">Number of records to skip</param>
        /// <param name="take">Number of records to take</param>
        /// <remarks>Give a negative number to either skip or take, retrieves all records found</remarks>
        public static ErrorLog[] GetErrorLogs(TransactionModelContainer container, Predicate<ErrorLog>[] criteria,
            int skip = 0, int take = 1)
        {
            ErrorLog[] errorLogs = new ErrorLog[0];

            int i = 1;
            if (i == 1)
                throw new NotImplementedException("THIS METHOD IS NOT IMPLEMENTED YET");

            return errorLogs;
        }

        public static void CreateErrorLog(string issuedBy, string issuedMessage, SeverityEnum severity, SystemError sys)
        {
            var options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            using (var exceptionLogScope = new TransactionScope(TransactionScopeOption.RequiresNew, options))
            {
                using (var container = new TransactionModelContainer())
                {
                    container.ErrorLogs.AddObject(new ErrorLog()
                    {
                        IssuedBy = issuedBy,
                        IssuedDate = DateTime.Now,
                        IssuedMessage = issuedMessage,
                        Severity = (byte)severity,
                        SystemErrorId = (byte)sys,
                        SystemErrorName = sys.ToString(),
                    });
                    container.SaveChanges();

                    exceptionLogScope.Complete();
                }
            }
        }

        #endregion

        public SeverityEnum SeverityEnum
        {
            get
            {
                return (SeverityEnum)Severity;
            }
            internal set
            {
                Severity = (byte)value;
            }
        }

        internal ErrorLog(string issuedBy, string issuedMsg, SeverityEnum severity)
        {
            IssuedBy = issuedBy;
            IssuedDate = DateTime.Now;
            IssuedMessage = issuedMsg;
            SeverityEnum = severity;
        }

        // Use by EF only!!!
        internal ErrorLog() { }

        #region Partial methods

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

        partial void OnIssuedMessageChanging(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("NULL_OR_EMPTY_ISSUED_MESSAGE");
            }
        }

        partial void OnSeverityChanging(byte value)
        {
            if (!Enum.IsDefined(typeof(SeverityEnum), value))
            {
                throw new ArgumentException("INVALID_SEVERITY_VALUE");
            }
        }

        #endregion


    }
}
