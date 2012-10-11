using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransactionServiceInterface;
using TransactionServiceImpl;

namespace TransactionServiceAdaptorTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
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
        public void TestMethod1()
        {
            //
            // TODO: Add test logic here
            //
        }

        [TestMethod]
        public void TestGetReconciliationByPaymentCode()
        {
            var service = new TransactionService();

            var result = service.GetReconciliationByPaymentCode(new[] { 
                    new GetReconciliationByPaymentCodeRequest(){ PaymentCode = "40700003"},
                    new GetReconciliationByPaymentCodeRequest(){ PaymentCode = "40700097"},
                    new GetReconciliationByPaymentCodeRequest(){ PaymentCode = "40700046"},
                    new GetReconciliationByPaymentCodeRequest(){ PaymentCode = "40700070"},
                    new GetReconciliationByPaymentCodeRequest(){ PaymentCode = "4070002X"},
                    new GetReconciliationByPaymentCodeRequest(){ PaymentCode = "40700062"},
                    new GetReconciliationByPaymentCodeRequest(){ PaymentCode = "40700089"},
                    new GetReconciliationByPaymentCodeRequest(){ PaymentCode = "40700038"},
                    new GetReconciliationByPaymentCodeRequest(){ PaymentCode = "40700011"},
                    new GetReconciliationByPaymentCodeRequest(){ PaymentCode = "40800059"},
            });




        }
    }
}
