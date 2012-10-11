using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransactionModel
{
    public partial class PaymentCode
    {
        /// <summary>
        /// Generate a payment code by CarPass rules
        /// [1-9][00-99][0000-9999][0-9]
        /// Era  Gen    Random     Mod11 
        /// where the random part is garanteed not consecutive for any couple of calls
        /// </summary>
        /// <returns></returns>
        public static string NextPaymentCode(TransactionModelContainer container)
        {
            PaymentCode code = container.PaymentCodes.FirstOrDefault(pc => !pc.IsUsed);

            if (code == null)
            {
                throw new ApplicationException("NO_MORE_PAYMENT_CODE_AVAILABLE");
            }

            code.IsUsed = true;
            return code.Code;
        }
    }
}
