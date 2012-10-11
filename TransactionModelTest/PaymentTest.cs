using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransactionModel;

namespace TransactionModelTest
{
    /// <summary>
    /// Summary description for PaymentTest
    /// </summary>
    [TestClass]
    public class PaymentTest
    {
        public PaymentTest()
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

        [TestMethod]
        public void CreatePayment_Test()
        {
            using (TransactionModelContainer container = new TransactionModelContainer())
            {
                Payment payment = new Payment("TESTER", 912, "LMG Insurance", "สุขุมวิท 5",
                    new PaymentItem(1, 400, 7, 3, true, true, ""),
                    new PaymentItem(1, 5000, 0, 0, false, true, ""));

                container.Payments.AddObject(payment);

                container.SaveChanges();
            }
        }

        [TestMethod]
        public void GetPayment_Test()
        {
            using (TransactionModelContainer container = new TransactionModelContainer())
            {
                Payment payment = container.Payments.Where(p => p.Id == 2).FirstOrDefault();

                if (payment != null)
                {
                    decimal grandTotal = payment.GrandTotal();
                    decimal totalUnitAmount = payment.TotalUnitAmount();
                    decimal totalVat = payment.TotalVat();
                    decimal totalWithholdingTax = payment.TotalWithholdingTax();
                    decimal remainingAmount = payment.RemainingAmount();
                }
            }
        }
    }
}
