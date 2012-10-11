using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransactionServiceInterface;
using System.ServiceModel;
using System.Transactions;
using TransactionModel;

namespace TransactionServiceImpl
{
    public partial class TransactionService : ITransactionService
    {
        [OperationBehavior(TransactionScopeRequired = true)]
        public RefundResponse CreateRefund(RefundRequest req)
        {
            var response = new RefundResponse();
            try
            {
                using (var container = new TransactionModelContainer())
                {
                    var refund = new Refund()
                    {
                        CreatedBy = req.CreateBy,
                        CreatedDate = DateTime.Now,
                        CustomerAddress = req.CustomerAddress,
                        CustomerIdmPartyId = req.CustomerIdmPartyId,
                        CustomerMobilePhoneNo = req.CustomerMobilePhoneNo,
                        CustomerName = req.CustomerName,
                        CustomerRefundAccountName = req.CustomerRefundAccountName,
                        CustomerRefundAccountNo = req.CustomerRefundAccountNo,
                        CustomerRefundBankId = req.CustomerRefundBankId,
                        IsVoid = req.IsVoid,
                        PaymentCode = req.PaymentCode,
                        Ref1 = req.Ref1,
                        Ref2 = req.Ref2,
                        Ref3 = req.Ref3,
                        Remark = req.Remark,
                        Status = req.Status,
                        UpdatedBy = req.UpdateBy,
                        UpdatedDate = DateTime.Now,
                    };

                    container.Refunds.AddObject(refund);

                    foreach (var r in req.Items)
                    {
                        var refundItem = new RefundItem()
                        {
                            GroupRef1 = r.GroupRef1,
                            GroupRef2 = r.GroupRef2,
                            ItemDescription = r.ItemDescription,
                            Qty = r.Qty,
                            Remark = r.Remark,
                            UnitAmount = r.UnitAmount,
                        };

                        container.RefundItems.AddObject(refundItem);
                    }

                    container.SaveChanges();

                    //response.SetPaymentResponse(payment);
                    response.Succeed();
                }
            }
            catch (Exception ex)
            {
                response.Fail(ex);
                CreateLog(ex);
            }            

            return response;
        }

        
    }
}
