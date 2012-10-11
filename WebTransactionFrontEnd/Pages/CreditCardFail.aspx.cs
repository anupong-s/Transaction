using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebTransactionFrontEnd;
using TransactionFrontEnd.ViewPayment;

public partial class Pages_CreditCardFail : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitParameters();
            MessageLabel.Text = "Credit card pay fail";
        }
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