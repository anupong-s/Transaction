using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TransactionFrontEnd.Common;
using TransactionFrontEnd.ViewPayment;

namespace WebTransactionFrontEnd
{
    /// <summary>
    /// Summary description for PageBase
    /// </summary>
    public abstract class PageBase : System.Web.UI.Page
    {
        public Main Main
        {
            get
            {
                Main main = new Main();
                if (Session["Main"] != null)
                {
                    main = (Main)Session["Main"];
                }
                else
                {
                    Session.Add("Main", main);
                }
                return main;
            }
            private set
            {
                if (value != null && value is Main)
                {
                    Session["Main"] = value;
                }
            }
        }

        public void Alert(string msg)
        {
            var script = string.Format("alert('{0}')", msg);
            if (ScriptManager.GetCurrent(this) == null)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertMessage", script, true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertMessage", script, true);
            }
        }

        public ViewPayment Model
        {
            get { return Session["ViewPayment"] as ViewPayment; }
            set { Session["ViewPayment"] = value; }
        }

        public string PaymentId
        {
            get { return ViewState["PaymentId"] as string; }
            set { ViewState["PaymentId"] = value; }
        }

        public string Ticket
        {
            get { return ViewState["Ticket"] as string; }
            set { ViewState["Ticket"] = value; }
        }
    }
}