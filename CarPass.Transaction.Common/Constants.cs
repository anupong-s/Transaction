using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace CarPass.Transaction.Common
{
    [DataContract]
    public enum PaymentStatusEnum
    {
        [EnumMember]
        UNPAID,
        [EnumMember]
        UNDERPAID,
        [EnumMember]
        PAID,
        [EnumMember]
        OVERPAID
    }

    [DataContract]
    public enum PaymentMethodEnum
    {
        [EnumMember]
        PAY_IN_SLIP,
        [EnumMember]
        CREDIT_CARD
    }
}
