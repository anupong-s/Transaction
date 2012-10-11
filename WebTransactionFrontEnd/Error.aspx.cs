using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TransactionFrontEnd.ViewPayment;
using WebTransactionFrontEnd;

public partial class Error : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Exception ex = Server.GetLastError();
            if (ex != null)
            {
                string message = ex.GetBaseException().Message;
                MessageLabel.Text = message;

                ViewPayment.LogsError(ex, "System");
            }
        }
    }
}