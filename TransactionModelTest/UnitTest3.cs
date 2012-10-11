using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransactionModel;
using System.IO;
using TransactionModel.Utils;

namespace TransactionModelTest
{
    /// <summary>
    /// Summary description for UnitTest3
    /// </summary>
    [TestClass]
    public class UnitTest3
    {
        public UnitTest3()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        //[TestMethod]
        //public void TestMethod1()
        //{
        //    using (TransactionModelContainer context = new TransactionModelContainer())
        //    {
        //        var targets = from p in context.Transactions.OfType<Payment>()
        //                      //where p.PaymentCode == "10000089"
        //                      select p;
        //        int c = targets.Count();
        //    }
        //}


        [TestMethod]
        public void TestCreateReconciliation()
        {
            using (var context = new TransactionModelContainer())
            {
                ReconciliationFile reconcil = new ReconciliationFile("test", new byte[] { 1, 2 }, DateTime.Now, "system",null,true,null,false);
                context.ReconciliationFiles.AddObject(reconcil);

                context.SaveChanges();
            }
        }

        [TestMethod]
        public void TestCreateErrorLog()
        {
            ErrorLog.CreateErrorLog("System", "error message", SeverityEnum.HIGH, SystemError.TransactionService);
        }

        [TestMethod]
        public void TestGetRootDirectory()
        {
            string directory = @"C:\Reconciliation\CreditCard\In";

            DirectoryInfo dirInfo = new DirectoryInfo(directory);
            DirectoryInfo[] dirs = dirInfo.GetDirectories();
            string name = dirInfo.Name;
            string parentName = dirInfo.Parent.Name;
            
        }

        [TestMethod]
        public void TestCreatePayment()
        {
            //Payment payment = new Payment();

            //payment.Installments.Add(new Installment { Amount = 2000 });
            //payment.Installments.Add(new Installment { Amount = 2798.80M });

            //payment.PaymentItems.Add(PaymentItem.CreateSubscriptionFee(1, 400, 7, 3, false));
            //payment.PaymentItems.Add(PaymentItem.CreateDeviceDeposit(1, 5000));
            //payment.PaymentItems.Add(PaymentItem.CreateFullYearUpgrade(1, 800, 7, 3, false));

            //payment.PaymentItems.Add(PaymentItem.CreateSubscriptionFee(1, 400, 7, 3, false));
            //payment.PaymentItems.Add(PaymentItem.CreateDeviceDeposit(1, 5000));
            //payment.PaymentItems.Add(PaymentItem.CreateFullYearUpgrade(1, 800, 7, 3, false));

            //payment.PaymentItems.Add(PaymentItem.CreateDiscount(2, -40, 7, 3));
            //payment.PaymentItems.Add(PaymentItem.CreateDiscount(2, -800, 7, 3));

            //payment.PaymentItems.Add(PaymentItem.CreateDiscount(1, -200, 0, 0));
            //payment.PaymentItems.Add(PaymentItem.CreateDiscount(1, -750, 0, 0));


            ////var subtotal = payment.TotalUnitAmount();

            //var grandTotal = payment.GrandTotal();
            
            //var remain = payment.RemainingAmount();

        }

        [TestMethod]
        public void TestConfigurationManager()
        {
            string dateFormat = ConfigurationManager.Format.Date_Format;
            string dateFormat1 = ConfigurationManager.Format.Date_Format;
            string AccNo = ConfigurationManager.PayIn.AccNO;
            

        }
    }
}
