using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransactionModel;
using TransactionModel.Utils;
using ReconciliationFileProcessor.SCB;
using System.IO;

namespace ReconciliationFileProcessor
{
    public class SCBProcessor : IBankProcessor
    {
        public void Reader(ReconciliationFile rec)
        {
            if (rec.FileType == FileType.C.ToString())
            {
                CreditCard(rec);
            }
            else if (rec.FileType == FileType.P.ToString())
            {
                PayInSlip(rec);
            }
        }

        private void CreditCard(ReconciliationFile rec)
        {
            ReadStream(rec);
        }

        private void PayInSlip(ReconciliationFile rec)
        {
            ReadStream(rec);
        }

        private void ReadStream(ReconciliationFile rec)
        {
            var reconciliations = new List<Reconciliation>();

            var header = new HeaderDetail();
            var debitList = new List<DebitDetail>();
            var creditList = new List<CreditDetail>();
            var payeeList = new List<PayeeDetail>();
            var data = new ScbDto();

            using (var stream = new StreamReader(new MemoryStream(rec.Contents), Encoding.Default))
            {
                string str = stream.ReadLine();
                while (str != null)
                {
                    if (str.AsRecordType() == SCBFlag.Header)
                    {
                        header.RecordType = str.AsRecordType();
                        header.CompanyId = str.AsCompanyId();
                        header.CustomerReference = str.AsCustomerReference();
                        header.MessageFileDate = str.AsMessageFileDate();
                        header.MessageFileTime = str.AsMessageFileTime();
                        header.ChannelId = str.AsChannelId();
                        header.BatchReference = str.AsBatchReference();
                    }
                    else if (str.AsRecordType() == SCBFlag.DebitDetail)
                    {
                        debitList.Add(new DebitDetail()
                        {
                            RecordType = str.AsRecordType(),
                            ProductCode = str.AsProductCode(),
                            ValueDate = str.AsValueDate(),
                            DebitAccountNo = str.AsDebitAccountNo(),
                            AccountTypeOfDebitAccount = str.AsAccountTypeOfDebitAccount(),
                            DebitBranchCode = str.AsDebitBranchCode(),
                            DebitCurrency = str.AsDebitCurrency(),
                            DebitAmount = str.AsDebitAmount(),
                            InternalReference = str.AsDebitInternalReference(),
                            NoOfCredits = str.AsNoOfCredits(),
                            FeeDebitAccount = str.AsFeeDebitAccount(),
                            Filler = str.AsFiller(),
                            MediaClearingCycle = str.AsMediaClearingCycle(),
                            AccountTypeFee = str.AsAccountTypeFee(),
                            DebitBranchCodeFee = str.AsDebitBranchCode()
                        });
                    }
                    else if (str.AsRecordType() == SCBFlag.CreditDetail)
                    {
                        creditList.Add(new CreditDetail()
                        {
                            RecordType = str.AsRecordType(),
                            CreditSequenceNumber = str.AsCreditSequenceNumber(),
                            CreditAccount = str.AsCreditAccount(),
                            CreditAmount = str.AsCreditAmount(),
                            CreditCurrency = str.AsCreditCurrency(),
                            InternalReference = str.AsCreditInternalReference(),
                            WHTPresent = str.AsWHTPresent(),
                            InvoiceDetailsPresent = str.AsInvoiceDetailsPresent(),
                            CreditAdviceRequired = str.AsCreditAdviceRequired(),
                            DeliveryMode = str.AsDeliveryMode(),
                            PickupLocation = str.AsPickupLocation(),
                            WHTFormType = str.AsWHTFormType(),
                            WHTTaxRuningNo = str.AsWHTTaxRuningNo(),
                            WHTAttachNo = str.AsWHTAttachNo(),
                            NoOfWHTDetails = str.AsNoOfWHTDetails(),
                            TotalWHTAmount = str.AsTotalWHTAmount(),
                            NoOfInvoiceDetails = str.AsNoOfInvoiceDetails(),
                            TotalInvoiceAmount = str.AsTotalInvoiceAmount(),
                            WHTPayType = str.AsWHTPayType(),
                            WHTRemark = str.AsWHTRemark(),
                            WHTDeductDate = str.AsWHTDeductDate(),
                            ReceivingBankCode = str.AsReceivingBankCode(),
                            ReceivingBankName = str.AsReceivingBankName(),
                            ReceivingBranchCode = str.AsReceivingBranchCode(),
                            ReceivingBranchName = str.AsReceivingBranchName(),
                            WHTSignatory = str.AsWHTSignatory(),
                            BeneficiaryNotification = str.AsBeneficiaryNotification(),
                            CustomerReferenceNumber = str.AsCustomerReferenceNumber(),
                            ChequeReferenceDocumentType = str.AsChequeReferenceDocumentType(),
                            PaymentTypeCode = str.AsPaymentTypeCode(),
                            ServicesType = str.AsServicesType(),
                            Remark = str.AsRemark(),
                            SCBRemark = str.AsSCBRemark(),
                            BeneficiaryCharge = str.AsBeneficiaryCharge(),
                        });
                    }
                    else if (str.AsRecordType() == SCBFlag.PayeeDetail)
                    {
                        payeeList.Add(new PayeeDetail()
                        {
                            RecordType = str.AsRecordType(),
                            PayeeInternalReference = str.AsPayeeInternalReference(),
                            PayeeCreditSequenceNumber = str.AsPayeeCreditSequenceNumber(),
                            Payee1_IDCard = str.AsPayee1_IDCard(),
                            Payee1_NameThai = str.AsPayee1_NameThai(),
                            Payee1_Address1 = str.AsPayee1_Address1(),
                            Payee1_Address2 = str.AsPayee1_Address2(),
                            Payee1_Address3 = str.AsPayee1_Address3(),
                            Payee1_TaxID = str.AsPayee1_TaxID(),
                            Payee1_NameEnglish = str.AsPayee1_NameEnglish(),
                            Payee1_FaxNumber = str.AsPayee1_FaxNumber(),
                            Payee1_MobilePhoneNumber = str.AsPayee1_MobilePhoneNumber(),
                            Payee1_EmailAddress = str.AsPayee1_EmailAddress(),
                            Payee2_NameThai = str.AsPayee2_NameThai(),
                            Payee2_Address1 = str.AsPayee2_Address1(),
                            Payee2_Address2 = str.AsPayee2_Address2(),
                            Payee2_Address3 = str.AsPayee2_Address3(),
                        });
                    }

                    str = stream.ReadLine();
                }
            }


            data.SetHeaderDetail(header);

            foreach (var payee in payeeList)
            {
                var payeeDetail = new PayeeDetail();

                payeeDetail.SetPayeeDetail(payee);

                var debit = debitList.Where(s => s.InternalReference == payee.PayeeInternalReference).ToList();
                if (debit.Count > 0)
                {



                }


            }

        }

        public void SaveReconcilation(Reconciliation[] recons)
        {
            foreach (Reconciliation recon in recons)
                Reconciliation.SaveReconciliation(recon);
        }

    }
}

namespace ReconciliationFileProcessor.SCB
{
    internal class SCBFlag
    {
        public static readonly string Header = "001";
        public static readonly string DebitDetail = "002";
        public static readonly string CreditDetail = "003";
        public static readonly string PayeeDetail = "004";
        public static readonly string WHTDetail = "005";
        public static readonly string InvoidDetail = "006";
    }

    internal class ScbDto
    {
        public ScbDto()
        {
            Payee = new List<PayeeDetail>();
        }

        public HeaderDetail Header { get; set; }

        public List<PayeeDetail> Payee { get; set; }

        public void SetHeaderDetail(HeaderDetail header)
        {
            Header.RecordType = header.RecordType;
            Header.CompanyId = header.CompanyId;
            Header.CustomerReference = header.CustomerReference;
            Header.MessageFileDate = header.MessageFileDate;
            Header.MessageFileTime = header.MessageFileTime;
            Header.ChannelId = header.ChannelId;
            Header.BatchReference = header.BatchReference;
        }
    }

    internal class HeaderDetail
    {
        public string RecordType { get; set; }
        public string CompanyId { get; set; }
        public string CustomerReference { get; set; }
        public string MessageFileDate { get; set; }
        public string MessageFileTime { get; set; }
        public string ChannelId { get; set; }
        public string BatchReference { get; set; }
    }

    internal class DebitDetail
    {
        public string RecordType { get; set; }
        public string ProductCode { get; set; }
        public string ValueDate { get; set; }
        public string DebitAccountNo { get; set; }
        public string AccountTypeOfDebitAccount { get; set; }
        public string DebitBranchCode { get; set; }
        public string DebitCurrency { get; set; }
        public string DebitAmount { get; set; }
        public string InternalReference { get; set; }
        public string NoOfCredits { get; set; }
        public string FeeDebitAccount { get; set; }
        public string Filler { get; set; }
        public string MediaClearingCycle { get; set; }
        public string AccountTypeFee { get; set; }
        public string DebitBranchCodeFee { get; set; }
    }

    internal class CreditDetail
    {
        public string RecordType { get; set; }
        public string CreditSequenceNumber { get; set; }
        public string CreditAccount { get; set; }
        public string CreditAmount { get; set; }
        public string CreditCurrency { get; set; }
        public string InternalReference { get; set; }
        public string WHTPresent { get; set; }
        public string InvoiceDetailsPresent { get; set; }
        public string CreditAdviceRequired { get; set; }
        public string DeliveryMode { get; set; }
        public string PickupLocation { get; set; }
        public string WHTFormType { get; set; }
        public string WHTTaxRuningNo { get; set; }
        public string WHTAttachNo { get; set; }
        public string NoOfWHTDetails { get; set; }
        public string TotalWHTAmount { get; set; }
        public string NoOfInvoiceDetails { get; set; }
        public string TotalInvoiceAmount { get; set; }
        public string WHTPayType { get; set; }
        public string WHTRemark { get; set; }
        public string WHTDeductDate { get; set; }
        public string ReceivingBankCode { get; set; }
        public string ReceivingBankName { get; set; }
        public string ReceivingBranchCode { get; set; }
        public string ReceivingBranchName { get; set; }
        public string WHTSignatory { get; set; }
        public string BeneficiaryNotification { get; set; }
        public string CustomerReferenceNumber { get; set; }
        public string ChequeReferenceDocumentType { get; set; }
        public string PaymentTypeCode { get; set; }
        public string ServicesType { get; set; }
        public string Remark { get; set; }
        public string SCBRemark { get; set; }
        public string BeneficiaryCharge { get; set; }

    }

    internal class PayeeDetail
    {
        public PayeeDetail()
        {
            Debit = new DebitDetail();
            Credit = new CreditDetail();
        }

        public string RecordType { get; set; }
        public string PayeeInternalReference { get; set; }
        public string PayeeCreditSequenceNumber { get; set; }
        public string Payee1_IDCard { get; set; }
        public string Payee1_NameThai { get; set; }
        public string Payee1_Address1 { get; set; }
        public string Payee1_Address2 { get; set; }
        public string Payee1_Address3 { get; set; }
        public string Payee1_TaxID { get; set; }
        public string Payee1_NameEnglish { get; set; }
        public string Payee1_FaxNumber { get; set; }
        public string Payee1_MobilePhoneNumber { get; set; }
        public string Payee1_EmailAddress { get; set; }
        public string Payee2_NameThai { get; set; }
        public string Payee2_Address1 { get; set; }
        public string Payee2_Address2 { get; set; }
        public string Payee2_Address3 { get; set; }

        public DebitDetail Debit { get; set; }
        public CreditDetail Credit { get; set; }

        public void SetPayeeDetail(PayeeDetail payee)
        {
            RecordType = payee.RecordType;
            PayeeInternalReference = payee.PayeeInternalReference;
            PayeeCreditSequenceNumber = payee.PayeeCreditSequenceNumber;
            Payee1_IDCard = payee.Payee1_IDCard;
            Payee1_NameThai = payee.Payee1_NameThai;
            Payee1_Address1 = payee.Payee1_Address1;
            Payee1_Address2 = payee.Payee1_Address2;
            Payee1_Address3 = payee.Payee1_Address3;
            Payee1_TaxID = payee.Payee1_TaxID;
            Payee1_NameEnglish = payee.Payee1_NameEnglish;
            Payee1_FaxNumber = payee.Payee1_FaxNumber;
            Payee1_MobilePhoneNumber = payee.Payee1_MobilePhoneNumber;
            Payee1_EmailAddress = payee.Payee1_EmailAddress;
            Payee2_NameThai = payee.Payee2_NameThai;
            Payee2_Address1 = payee.Payee2_Address1;
            Payee2_Address2 = payee.Payee2_Address2;
            Payee2_Address3 = payee.Payee2_Address3;
        }

        public void SetDebitDetail(DebitDetail debit)
        {
            Debit.RecordType = debit.RecordType;
            Debit.ProductCode = debit.ProductCode;
            Debit.ValueDate = debit.ValueDate;
            Debit.DebitAccountNo = debit.DebitAccountNo;
            Debit.AccountTypeOfDebitAccount = debit.AccountTypeOfDebitAccount;
            Debit.DebitBranchCode = debit.DebitBranchCode;
            Debit.DebitCurrency = debit.DebitCurrency;
            Debit.DebitAmount = debit.DebitAmount;
            Debit.InternalReference = debit.InternalReference;
            Debit.NoOfCredits = debit.NoOfCredits;
            Debit.FeeDebitAccount = debit.FeeDebitAccount;
            Debit.Filler = debit.Filler;
            Debit.MediaClearingCycle = debit.MediaClearingCycle;
            Debit.AccountTypeFee = debit.AccountTypeFee;
            Debit.DebitBranchCodeFee = debit.DebitBranchCodeFee;
        }
    }

    internal static class SCBExtension
    {
        #region Header
        public static string AsRecordType(this string value)
        {
            return value.Substring(0, 3).Trim();
        }

        public static string AsCompanyId(this string value)
        {
            return value.Substring(3, 12).Trim();
        }

        public static string AsCustomerReference(this string value)
        {
            return value.Substring(15, 32).Trim();
        }

        public static string AsMessageFileDate(this string value)
        {
            return value.Substring(47, 8).Trim();
        }

        public static string AsMessageFileTime(this string value)
        {
            return value.Substring(55, 6).Trim();
        }

        public static string AsChannelId(this string value)
        {
            return value.Substring(61, 3).Trim();
        }

        public static string AsBatchReference(this string value)
        {
            return value.Substring(64, 32).Trim();
        }
        #endregion

        #region DebitDetail
        //public static string AsDetailRecordType(this string value)
        //{
        //    return value.Substring(0, 3).Trim();
        //}

        public static string AsProductCode(this string value)
        {
            return value.Substring(3, 3).Trim();
        }

        public static string AsValueDate(this string value)
        {
            return value.Substring(6, 8).Trim();
        }

        public static string AsDebitAccountNo(this string value)
        {
            return value.Substring(14, 25).Trim();
        }

        public static string AsAccountTypeOfDebitAccount(this string value)
        {
            return value.Substring(39, 2).Trim();
        }

        public static string AsDebitBranchCode(this string value)
        {
            return value.Substring(41, 4).Trim();
        }

        public static string AsDebitCurrency(this string value)
        {
            return value.Substring(45, 3).Trim();
        }

        public static string AsDebitAmount(this string value)
        {
            return value.Substring(48, 16).Trim();
        }

        public static string AsDebitInternalReference(this string value)
        {
            return value.Substring(64, 8).Trim();
        }

        public static string AsNoOfCredits(this string value)
        {
            return value.Substring(72, 6).Trim();
        }

        public static string AsFeeDebitAccount(this string value)
        {
            return value.Substring(78, 15).Trim();
        }

        public static string AsFiller(this string value)
        {
            return value.Substring(93, 9).Trim();
        }

        public static string AsMediaClearingCycle(this string value)
        {
            return value.Substring(102, 1).Trim();
        }

        public static string AsAccountTypeFee(this string value)
        {
            return value.Substring(103, 2).Trim();
        }

        public static string AsDebitBranchCodeFee(this string value)
        {
            return value.Substring(105, 4).Trim();
        }
        #endregion

        #region CreditDetail
        //public static string AsCreditRecordType(this string value)
        //{
        //    return value.Substring(0, 3).Trim();
        //}
        public static string AsCreditSequenceNumber(this string value)
        {
            return value.Substring(3, 6).Trim();
        }
        public static string AsCreditAccount(this string value)
        {
            return value.Substring(9, 25).Trim();
        }
        public static string AsCreditAmount(this string value)
        {
            return value.Substring(34, 16).Trim();
        }
        public static string AsCreditCurrency(this string value)
        {
            return value.Substring(50, 3).Trim();
        }
        public static string AsCreditInternalReference(this string value)
        {
            return value.Substring(53, 8).Trim();
        }
        public static string AsWHTPresent(this string value)
        {
            return value.Substring(61, 1).Trim();
        }
        public static string AsInvoiceDetailsPresent(this string value)
        {
            return value.Substring(62, 1).Trim();
        }
        public static string AsCreditAdviceRequired(this string value)
        {
            return value.Substring(63, 1).Trim();
        }
        public static string AsDeliveryMode(this string value)
        {
            return value.Substring(64, 1).Trim();
        }
        public static string AsPickupLocation(this string value)
        {
            return value.Substring(65, 4).Trim();
        }
        public static string AsWHTFormType(this string value)
        {
            return value.Substring(69, 2).Trim();
        }
        public static string AsWHTTaxRuningNo(this string value)
        {
            return value.Substring(71, 14).Trim();
        }
        public static string AsWHTAttachNo(this string value)
        {
            return value.Substring(85, 6).Trim();
        }
        public static string AsNoOfWHTDetails(this string value)
        {
            return value.Substring(91, 2).Trim();
        }
        public static string AsTotalWHTAmount(this string value)
        {
            return value.Substring(93, 16).Trim();
        }
        public static string AsNoOfInvoiceDetails(this string value)
        {
            return value.Substring(109, 6).Trim();
        }
        public static string AsTotalInvoiceAmount(this string value)
        {
            return value.Substring(115, 16).Trim();
        }
        public static string AsWHTPayType(this string value)
        {
            return value.Substring(131, 1).Trim();
        }
        public static string AsWHTRemark(this string value)
        {
            return value.Substring(132, 40).Trim();
        }
        public static string AsWHTDeductDate(this string value)
        {
            return value.Substring(172, 8).Trim();
        }
        public static string AsReceivingBankCode(this string value)
        {
            return value.Substring(180, 3).Trim();
        }
        public static string AsReceivingBankName(this string value)
        {
            return value.Substring(183, 35).Trim();
        }
        public static string AsReceivingBranchCode(this string value)
        {
            return value.Substring(218, 4).Trim();
        }
        public static string AsReceivingBranchName(this string value)
        {
            return value.Substring(222, 35).Trim();
        }
        public static string AsWHTSignatory(this string value)
        {
            return value.Substring(257, 1).Trim();
        }
        public static string AsBeneficiaryNotification(this string value)
        {
            return value.Substring(258, 1).Trim();
        }
        public static string AsCustomerReferenceNumber(this string value)
        {
            return value.Substring(259, 20).Trim();
        }
        public static string AsChequeReferenceDocumentType(this string value)
        {
            return value.Substring(279, 1).Trim();
        }
        public static string AsPaymentTypeCode(this string value)
        {
            return value.Substring(280, 3).Trim();
        }
        public static string AsServicesType(this string value)
        {
            return value.Substring(283, 2).Trim();
        }
        public static string AsRemark(this string value)
        {
            return value.Substring(285, 50).Trim();
        }
        public static string AsSCBRemark(this string value)
        {
            return value.Substring(335, 18).Trim();
        }
        public static string AsBeneficiaryCharge(this string value)
        {
            return value.Substring(353, 2).Trim();
        }
        #endregion

        #region PayeeDetail
        //public static string AsPayeeRecordType(this string value)
        //{
        //    return value.Substring(0, 3).Trim();
        //}
        public static string AsPayeeInternalReference(this string value)
        {
            return value.Substring(3, 8).Trim();
        }
        public static string AsPayeeCreditSequenceNumber(this string value)
        {
            return value.Substring(11, 6).Trim();
        }
        public static string AsPayee1_IDCard(this string value)
        {
            return value.Substring(17, 15).Trim();
        }
        public static string AsPayee1_NameThai(this string value)
        {
            return value.Substring(32, 100).Trim();
        }
        public static string AsPayee1_Address1(this string value)
        {
            return value.Substring(132, 70).Trim();
        }
        public static string AsPayee1_Address2(this string value)
        {
            return value.Substring(202, 70).Trim();
        }
        public static string AsPayee1_Address3(this string value)
        {
            return value.Substring(272, 70).Trim();
        }
        public static string AsPayee1_TaxID(this string value)
        {
            return value.Substring(342, 10).Trim();
        }
        public static string AsPayee1_NameEnglish(this string value)
        {
            return value.Substring(352, 70).Trim();
        }
        public static string AsPayee1_FaxNumber(this string value)
        {
            return value.Substring(422, 10).Trim();
        }
        public static string AsPayee1_MobilePhoneNumber(this string value)
        {
            return value.Substring(432, 10).Trim();
        }
        public static string AsPayee1_EmailAddress(this string value)
        {
            return value.Substring(442, 64).Trim();
        }
        public static string AsPayee2_NameThai(this string value)
        {
            return value.Substring(506, 100).Trim();
        }
        public static string AsPayee2_Address1(this string value)
        {
            return value.Substring(606, 70).Trim();
        }
        public static string AsPayee2_Address2(this string value)
        {
            return value.Substring(676, 70).Trim();
        }
        public static string AsPayee2_Address3(this string value)
        {
            return value.Substring(746, 70).Trim();
        }
        #endregion
    }
}
