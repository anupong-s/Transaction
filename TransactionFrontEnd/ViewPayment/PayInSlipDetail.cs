using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Telerik.Reporting;

namespace TransactionFrontEnd.ViewPayment
{
    public class PayInSlipDetail
    {
        public string CarpassName { set; get; }
        public string CarpassAddr { set; get; }
        public string CarpassAddr2 { set; get; }
        public string NoTax { set; get; }
        public string ScbCompCode { set; get; }
        public string KBankAccountNo { set; get; }
        public string BKKBank { set; get; }
        public string Date { set; get; }
        //public string branch { set; get; }
        public string ServiceCode { set; get; }
        public string CustName { set; get; }
        public string CustNo { set; get; }
        public string RefNo { set; get; }
        public string AmountInCash { set; get; }
        public string AmountInWord { set; get; }
        
        public PayInSlipDetail (
            string carpassNameSelector, string carpassAddrSelector,
            string carpassAddr2Selector, string noTaxSelector,
            string scbCompcodeSelector, string ktcAccountNoSelector,
            string bkkBankselector, string dateSelector,
            string serviceCodeSelector, string custNameSelector,
            string custNoSelector, string refNoSelector,
            string amountInCashSelector
            )
        {
            CarpassName = carpassNameSelector;
            CarpassAddr = carpassAddrSelector;
            CarpassAddr2 = carpassAddr2Selector;
            NoTax = noTaxSelector;
            ScbCompCode = scbCompcodeSelector;
            KBankAccountNo = ktcAccountNoSelector;
            BKKBank = bkkBankselector;
            Date = dateSelector;
            ServiceCode = serviceCodeSelector;
            CustName = custNameSelector;
            CustNo = custNoSelector;
            RefNo = refNoSelector;
            AmountInCash = amountInCashSelector;
            AmountInWord = ConvertToWord(decimal.Parse(amountInCashSelector));
            
        }

        #region Private Method

        private string ConvertToWord(decimal amount)
        {
            string word = string.Empty;

            string[] num = new string[2];

            string strNum = string.Format("{0:0.00}", amount);
            strNum = strNum.Replace(",", "");
            strNum = strNum.Replace(" ", "");

            num = strNum.Split('.');

            word += ConvertToText(num[0]) + "บาท";

            if (int.Parse(num[1]) == 0)//*
            {
                word += "ถ้วน";
            }
            else
            {
                word += ConvertToText(num[1]) + "สตางค์";
            }

            return word;
        }

        private string ConvertToText(string strNum)
        {
            string word = string.Empty;
            string[] txtNum = { "", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า" };
            string[] txtUnit = { "", "สิบ", "ร้อย", "พัน", "หมื่น", "แสน", "ล้าน", "" };

            int unitIndex = strNum.Length - 1;
            for (int i = 0; i < strNum.Length; i++)
            {
                if (strNum.Length == 1 && strNum[i] == '0')
                {
                    word += "ศูนย์";
                }
                else if (strNum[i] == '0')
                {
                    word += "";
                }
                else if ((strNum.Length == 1 && strNum[i] == '1') || (i == unitIndex && strNum[i] == '1' && strNum[i - 1] == '0'))
                {
                    word += "หนึ่ง";
                }
                else if (i == unitIndex && strNum[i] == '1')
                {
                    word += "เอ็ด";
                }
                else if (unitIndex == 7 && i == 1 && strNum[i] == '1')
                {
                    word += "เอ็ดล้าน";
                }
                else if ((i == unitIndex - 1 && strNum[i] == '2') || (unitIndex == 7 && i == 0 && strNum[i] == '2'))
                {
                    word += "ยี่สิบ";
                }
                else if ((i == unitIndex - 1 && strNum[i] == '1'))
                {
                    word += "สิบ";
                }
                else
                {
                    switch (strNum[i])
                    {
                        //case '0': word += txtNum[0]; break;
                        case '1': word += txtNum[1]; break;
                        case '2': word += txtNum[2]; break;
                        case '3': word += txtNum[3]; break;
                        case '4': word += txtNum[4]; break;
                        case '5': word += txtNum[5]; break;
                        case '6': word += txtNum[6]; break;
                        case '7': word += txtNum[7]; break;
                        case '8': word += txtNum[8]; break;
                        case '9': word += txtNum[9]; break;
                    }

                    word += txtUnit[unitIndex - i];
                }
            }
            return word;
        }

        #endregion

        public static PayInSlipDetail CreatePayInSlipSource(string carpassName, string carpassAddr,
            string carpassAddr2, string noTax,
            string scbCompcode, string ktcAccountNo,
            string bkkBank, string date,
            string serviceCode, string custName,
            string custNo, string refNo,
            string amountInCash)
        {
            return new PayInSlipDetail(carpassName, carpassAddr,
            carpassAddr2, noTax, scbCompcode, ktcAccountNo,bkkBank, date,
            serviceCode, custName, custNo, refNo, amountInCash);
        }
        
    }
}
