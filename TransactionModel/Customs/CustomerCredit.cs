using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransactionModel.Utils;
using CarPass.Transaction.Common;

namespace TransactionModel
{
    public partial class CustomerCredit
    {
        #region Partial Methods
         partial void OnCreatedDateChanging(DateTime value)
        {
            if (!value.IsPersistable())
            {
                throw new ArgumentException("CREATE_DATE_NOT_PERSISTABLE");
            }
        }

        partial void OnCreatedByChanging(String value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("NULL_OR_EMPTY_CREATE_BY");
            }

            if (value.Length > 50)
            {
                throw new ArgumentException("CREATE_BY_TOO_LONG");
            }
        }

        partial void OnCreatedByChanged()
        {
            if (!string.IsNullOrEmpty(_CreatedBy))
            {
                _CreatedBy = _CreatedBy.TextEncode();
            }
        }

        partial void OnUpdatedDateChanging(DateTime value)
        {
            if (!value.IsPersistable())
            {
                throw new ArgumentException("UPDATE_DATE_NOT_PERSISTABLE");
            }
        }

        partial void OnUpdatedByChanging(String value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("NULL_OR_EMPTY_UPDATE_BY");
            }

            if (value.Length > 50)
            {
                throw new ArgumentException("UPDATE_BY_TOO_LONG");
            }
        }

        partial void OnUpdatedByChanged()
        {
            if (!string.IsNullOrEmpty(_UpdatedBy))
            {
                _UpdatedBy = UpdatedBy.TextEncode();
            }
        }

        partial void OnCustomerNameChanging(String value)
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

        partial void OnCustomerNameChanged()
        {
            if (!string.IsNullOrEmpty(_CustomerName))
            {
                _CustomerName = CustomerName.TextEncode();
            }
        }

        partial void OnCustomerAddressChanging(String value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("NULL_OR_EMPTY_CUSTOMER_ADDRESS");
            }

            if (value.Length > 255)
            {
                throw new ArgumentException("CUSTOMER_ADDRESS_TOO_LONG");
            }
        }

        partial void OnCustomerAddressChanged()
        {
            if (!string.IsNullOrEmpty(_CustomerAddress))
            {
                _CustomerAddress = CustomerAddress.TextEncode();
            }
        }

        partial void OnCustomerRefundAccountNoChanging(String value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length > 255)
                {
                    throw new ArgumentException("CUSTOMER_REFUND_ACCOUNT_NO_TOO_LONG");
                }
            }
        }

        partial void OnCustomerRefundAccountNoChanged()
        {
            if (!string.IsNullOrEmpty(_CustomerRefundAccountNo))
            {
                _CustomerRefundAccountNo = CustomerRefundAccountNo.TextEncode();
            }
        }

        partial void OnCustomerRefundAccountNameChanging(String value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length > 50)
                {
                    throw new ArgumentException("CUSTOMER_REFUND_ACCOUNT_NAME_TOO_LONG");
                }
            }
        }

        partial void OnCustomerRefundAccountNameChanged()
        {
            if (!string.IsNullOrEmpty(_CustomerRefundAccountName))
            {
                _CustomerRefundAccountName = CustomerRefundAccountName.TextEncode();
            }
        }

        partial void OnCustomerMobilePhoneNoChanging(String value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length > 20)
                {
                    throw new ArgumentException("CUSTOMER_REFUND_ACCOUNT_NO_TOO_LONG");
                }
            }
        }

        partial void OnCustomerMobilePhoneNoChanged()
        {
            if (!string.IsNullOrEmpty(_CustomerMobilePhoneNo))
            {
                _CustomerMobilePhoneNo = CustomerMobilePhoneNo.TextEncode();
            }
        }

        partial void OnBeneficiaryChargeChanging(String value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length > 5)
                {
                    throw new ArgumentException("BENEFICIARY_CHARGE_TOO_LONG");
                }
            }
        }

        partial void OnBeneficiaryChargeChanged()
        {
            if (!string.IsNullOrEmpty(_BeneficiaryCharge))
            {
                _BeneficiaryCharge = BeneficiaryCharge.TextEncode();
            }
        }
        
        partial void OnHotKeyChanging(String value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length > 50)
                {
                    throw new ArgumentException("HOT_KEY_TOO_LONG");
                }
            }
        }

        partial void OnHotKeyChanged()
        {
            if (!string.IsNullOrEmpty(_HotKey))
            {
                _HotKey = HotKey.TextEncode();
            }
        }

        #endregion

        #region Constructor

        internal CustomerCredit() { }

        public CustomerCredit(string createBy, decimal amount, int installmentId,
            long customerIdmPartyId, string customerName, string customerAddress)
        {
            CreatedBy = createBy;
            CreatedDate = DateTime.Now;
            UpdatedBy = createBy;
            UpdatedDate = DateTime.Now;

            Amount = amount;
            InstallmentId = installmentId;
            CustomerIdmPartyId = customerIdmPartyId;
            CustomerName = customerName;
            CustomerAddress = customerAddress;
        }

        #endregion


        #region Static Method

        public static void UpdateCustomerCredit(params CustomerCredit[] credits)
        {
            foreach (var credit in credits)
            {
                credit.IsUsedOrRefund = true;
            }
        }

        #endregion
    }
}
