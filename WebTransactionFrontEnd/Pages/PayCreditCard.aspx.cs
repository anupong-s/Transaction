using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebTransactionFrontEnd;
using TransactionFrontEnd.ViewPayment;
using TransactionModel;

public partial class Pages_PayCreditCard : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                InitParameters();
                CallBank();
            }
            catch(Exception ex)
            {
                Model.TheMain.LogsError(ex);
            }
        }
    }

    private void CallBank()
    {
        string prefixedUrl = string.Format("http://{0}:{1}{2}/Pages/",
                Request.Url.Host, Request.Url.Port, Request.ApplicationPath);

        successUrl.Value = prefixedUrl + "CreditCardSuccess.aspx?PaymentId=" + Model.PaymentId + "&Ticket=" + Ticket;
        failUrl.Value = prefixedUrl + "ViewPayment.aspx?PaymentId=" + Model.PaymentId + "&Ticket=" + Ticket;
        cancelUrl.Value = prefixedUrl + "ViewPayment.aspx?PaymentId=" + Model.PaymentId + "&Ticket=" + Ticket;

        merchantId.Value = ConfigurationManager.PaymentGateways.MerchantID;
        currCode.Value = ConfigurationManager.PaymentGateways.CurrencyCode;
        payType.Value = ConfigurationManager.PaymentGateways.PaymentType;
        lang.Value = ConfigurationManager.PaymentGateways.Language;

        orderRef.Value = Model.PaymentCode;
        remark.Value = Model.Remark;
        amount.Value = Model.RemainingAmount.Replace(",","");
    }

    private void InitParameters()
    {
        PaymentId = Request.QueryString["PaymentId"];
        Ticket = Request.QueryString["Ticket"];

        if (string.IsNullOrEmpty(PaymentId) || string.IsNullOrEmpty(Ticket))
            throw new ArgumentException("MISSING_REQUIRED_PARAMETERS");

        if (Model == null)
        {
            Model = new ViewPayment();
            Model.Initialize(Main, long.Parse(PaymentId), long.Parse(Ticket));
        }
    }
}