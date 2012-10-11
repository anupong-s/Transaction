using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransactionModel.Utils;
using System.Data.Objects;
using System.Data;

namespace TransactionModel
{
    public partial class TransactionModelContainer
    {
        #region Domain Integrity

        public override int SaveChanges(SaveOptions options)
        {
            foreach (ObjectStateEntry entry in
                ObjectStateManager.GetObjectStateEntries(
                EntityState.Added | EntityState.Modified))
            {
                if (entry.Entity is Configuration)
                {
                    ValidateConfigurationAggregate((Configuration)entry.Entity);
                }
                else if (entry.Entity is Payment)
                {

                    ValidatePaymentAggregate((Payment)entry.Entity);                    

                    if (entry.State == EntityState.Added)
                    {
                        Payment payment = (Payment)entry.Entity;
                        payment.PaymentCode = PaymentCode.NextPaymentCode(this);
                    }
                }
            }

            return base.SaveChanges(options);
        }

        private void ValidatePaymentAggregate(Payment payment)
        {
            if (payment.PaymentItems == null || payment.PaymentItems.Count < 1)
                throw new ApplicationException("AT_LEAST_ONE_ITEM_REQUIRED");
        }

        private void ValidateConfigurationAggregate(Configuration cfg)
        {
            if (Configurations.Any(c => c.Group == cfg.Group && c.Name == cfg.Name))
            {
                throw new ApplicationException("DUP_CONFIGURATION");
            }

            // More validations...
        }

        #endregion

        #region Utils

        /// <summary>
        /// A Get configuration short cut
        /// </summary>
        public Configuration this[string grp, string name]
        {
            get
            {
                return Configuration.GetConfiguration(this, grp, name);
            }
        }

        /// <summary>
        /// Logs an occurring error to the system. The log will actually create after a SaveChanges call.
        /// </summary>
        /// <param name="issuedBy">Issuer</param>
        /// <param name="issuedMsg">Message to log</param>
        /// <param name="severity">Severity (default = LOW)</param>
        /// <remarks>container.SaveChanges is required</remarks>
        public ErrorLog LogsError(string issuedBy, string issuedMsg,
            SeverityEnum severity = SeverityEnum.LOW)
        {
            return ErrorLog.LogsError(this, issuedBy, issuedMsg, severity);
        }

        #endregion
    }
}
