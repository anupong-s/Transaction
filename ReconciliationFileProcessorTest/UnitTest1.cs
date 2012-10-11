using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReconciliationFileProcessor;
using System.IO;
using System.Threading;

namespace ReconciliationFileProcessorTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestReconciliation()
        {
            Processor process = new Processor();
            process.ReadReconciliation();
        }

        [TestMethod]
        public void TestReadPayInSlip()
        {
            ReadFileFromIn(@"C:\Reconciliation\PayInSlip\In\RCC.txt");
        }

        int detailCount;
        bool flag = true;
        string fileName;
        string bankCode;
        string effectiveDate;
        string srcInFile;
        decimal totalCreditPaymentAmt = 0;

        private void ReadFileFromIn(string sourceFileName)
        {
            var info = new FileInfo(sourceFileName);
            try
            {
                if (info.Exists) //ตรวจสอบไฟล์ว่ามีไฟล์หรือไม่
                {
                    StreamReader SR;
                    string str;
                    string temp;

                    Thread.Sleep(5000);
                    SR = new StreamReader(sourceFileName, Encoding.Default);
                    str = SR.ReadLine();

                    //Check filename that is RCC or KBANK.
                    fileName = sourceFileName.Split('\\').Last().Substring(0, sourceFileName.Split('\\').Last().Length - 4);

                    //bankCode = sourceFileName.Substring(28, 3);
                    //Find bankCode from file.
                    if (fileName.Equals("RCC"))
                    {
                        bankCode = str.Split(' ').ElementAt(1).Substring(7, 3);
                    }
                    else
                    {
                        bankCode = str.Substring(7, 3);
                    }

                    srcInFile = sourceFileName;
                    fileName = "PMT" + bankCode + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".txt";

                    Thread.Sleep(5000);
                    while (str != null)
                    {
                        if (bankCode == "002")
                        {
                            str = str.Substring(19);
                        }

                        temp = str.Substring(0, 1);

                        if (temp == "D")
                        {
                            detailCount++;
                            LoadFieldToStg_Installments_Normal(str);
                        }
                        else if (temp == "T")
                        {
                            totalCreditPaymentAmt = Convert.ToDecimal(str.Substring(40, 12).Trim());
                        }
                        else if (temp == "H")
                        {
                            while (flag)
                            {
                                string[] values = str.Split(' ').Where(i => !string.IsNullOrEmpty(i)).ToArray();

                                if (values.Count() > 0)
                                {
                                    string dd = values[values.Count() - 1].Substring(0, 2);//str.Substring(60, 2);
                                    string mm = values[values.Count() - 1].Substring(2, 2);//str.Substring(62, 2);
                                    string yyyy = values[values.Count() - 1].Substring(4, 4);//str.Substring(64, 4);
                                    effectiveDate = dd + "/" + mm + "/" + yyyy;
                                    flag = false;
                                }
                            }
                        }
                        Console.WriteLine(str);
                        str = SR.ReadLine();
                    }
                    Thread.Sleep(5000);
                    SR.Close();
                    SR.Dispose();

                    //GetStg_Installments_NormalByFileName();
                    //GenerateTaxInvoice();
                    //DeleteStg_Installments_Normal();
                }
                else
                {
                    Console.WriteLine("Invalid file name");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadFieldToStg_Installments_Normal(string str)
        {
            try
            {
                //using (var transacContainer = new TransactionModelContainer())
                //{
                var stgInstallmentNormal = new stg_Installments_Normal();

                stgInstallmentNormal.BankCode = str.Substring(7, 3).Trim();
                stgInstallmentNormal.CompanyAccount = str.Substring(10, 10).Trim();

                string dd = str.Substring(20, 2);
                string mm = str.Substring(22, 2);
                string yyyy = str.Substring(24, 4);
                string hh = str.Substring(28, 2);
                string m = str.Substring(30, 2);
                string ss = str.Substring(32, 2);

                var dt = new DateTime(Convert.ToInt32(yyyy), Convert.ToInt32(mm), Convert.ToInt32(dd), Convert.ToInt32(hh), Convert.ToInt32(m), Convert.ToInt32(ss));
                stgInstallmentNormal.PaymentDate = dt;
                stgInstallmentNormal.CustomerName = str.Substring(34, 50).Trim();
                stgInstallmentNormal.PaymentCode = str.Substring(84, 20).Trim();
                stgInstallmentNormal.BranchNo = str.Substring(144, 4).Trim();
                stgInstallmentNormal.TransactionType = str.Substring(152, 1).Trim();
                stgInstallmentNormal.TransactionCode = str.Substring(153, 3).Trim();
                stgInstallmentNormal.Amount = Convert.ToDecimal(str.Substring(163, 13).Trim());
                stgInstallmentNormal.FileName = fileName;
                stgInstallmentNormal.RecordStatus = "Success";
                stgInstallmentNormal.RecordRemark = "";

                if (stgInstallmentNormal.BankCode == "014")
                {
                    stgInstallmentNormal.ProcessStatus = str.Substring(179, 1).Trim();
                    stgInstallmentNormal.TransactionDate = Convert.ToDateTime(str.Substring(180, 8).Trim());
                    stgInstallmentNormal.PostingDate = Convert.ToDateTime(str.Substring(188, 60).Trim());
                }
                else
                {
                    stgInstallmentNormal.ProcessStatus = " ";
                    stgInstallmentNormal.TransactionDate = Convert.ToDateTime("1753/1/1");
                    stgInstallmentNormal.PostingDate = Convert.ToDateTime("1753/1/1"); ;
                }

                // check null 
                if (string.IsNullOrEmpty(str.Substring(104, 20).Trim()))
                {
                    stgInstallmentNormal.Ref2 = "";
                }
                else
                {
                    stgInstallmentNormal.Ref2 = str.Substring(104, 20).Trim();
                }

                // check null 
                if (string.IsNullOrEmpty(str.Substring(148, 4).Trim()))
                {
                    stgInstallmentNormal.TellerNo = "";
                }
                else
                {
                    stgInstallmentNormal.TellerNo = str.Substring(148, 4).Trim();
                }

                // check null 
                if (string.IsNullOrEmpty(str.Substring(156, 7).Trim()))
                {
                    stgInstallmentNormal.ChequeNo = "";
                }
                else
                {
                    stgInstallmentNormal.ChequeNo = str.Substring(156, 7).Trim();
                }

                // check null 
                if (string.IsNullOrEmpty(str.Substring(176, 3).Trim()))
                {
                    stgInstallmentNormal.ChequeBankCode = "";
                }
                else
                {
                    stgInstallmentNormal.ChequeBankCode = str.Substring(176, 3).Trim();
                }

                //transacContainer.stg_Installments_Normal.AddObject(stgInstallmentNormal);
                //transacContainer.SaveChanges();
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
