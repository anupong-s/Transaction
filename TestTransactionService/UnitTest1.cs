using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransactionServiceImpl;
using TransactionServiceInterface;
using System.Transactions;

namespace TestTransactionService
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestCreatePayment()
        {
            
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var service = new TransactionServiceImpl.TransactionService();

                    CreatePaymentRequest req = new CreatePaymentRequest();
                    req.CreateBy = "TEST 2";
                    req.CustomerIDMPartyID = 912;
                    req.CustomerName = "TEST 2";
                    req.CustomerAddress = "44/55";

                    CreatePaymentItemRequest item = new CreatePaymentItemRequest();
                    //item.ServiceCode = "1";
                    item.ItemDescription = "Subscription Fee";
                    item.Quantity = 1;
                    item.UnitAmount = 3000;
                    item.VatPercent = 7;
                    item.WithholdingTaxPercent = 3;
                    item.ServiceIsRevenue = true;
                    item.IsLegalPerson = false;

                    req.PaymentItems = new CreatePaymentItemRequest[1];
                    req.PaymentItems[0] = item;

                    service.CreatePayment(req);

                    bool err = true;
                    if (err)
                        throw new Exception("error");
                    
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                
            }


        }


        [TestMethod]
        public void TestGetPaymentStatus()
        {
            var service = new TransactionServiceImpl.TransactionService();
            var result = service.GetPaymentStatusByPaymentId(
                            new GetPaymentStatusRequest { PaymentId = "8" });

            //var x = result.PaymentStatus;
        }

        [TestMethod]
        public void TestCalculatePayment()
        {
            var service = new TransactionServiceImpl.TransactionService();

            var items = new CalculatePaymentItemRequest[2];

            var item = new CalculatePaymentItemRequest();
            //item.ServiceCode = "1";
            item.ItemDescription = "Subscription Fee";
            item.Quantity = 1;
            item.UnitAmount = 3000;
            item.VatPercent = 7;
            item.WithholdingTaxPercent = 3;
            item.ServiceIsRevenue = true;
            item.IsLegalPerson = false;
            items[0] = item;
            item = new CalculatePaymentItemRequest();
            //item.ServiceCode = "7";
            item.ItemDescription = "Discount";
            item.Quantity = 1;
            item.UnitAmount = -1000;
            item.VatPercent = 0;
            item.WithholdingTaxPercent = 0;
            item.ServiceIsRevenue = false;
            item.IsLegalPerson = false;
            items[1] = item;

            var result = service.CalculatePayment(new CalculatePaymentRequest { 
                CustomerName = "TEST",
                PaymentItems = items           
            });
        }
        
        [TestMethod]
        public void TestGetTaxInvoiceReceipt()
        {
            var service = new TransactionServiceImpl.TransactionService();
            GetTaxInvoiceReceiptResponse res = service.GetTaxInvoiceReceiptByPaymentId(
                new GetTaxInvoiceReceiptRequest { PaymentId = "8" });

            byte[] report = res.TaxInvoicePDF;
        }
    }
}
