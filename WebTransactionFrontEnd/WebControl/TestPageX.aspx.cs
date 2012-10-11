using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TEST;

public partial class WebControl_TestPageX : System.Web.UI.Page
{
    private string _sessionPrefix;
    public string SessionPrefix
    {
        get { return _sessionPrefix; }
        set { _sessionPrefix = value; }
    }

    public XViewModel Model
    {
        get { return Session["x"] as XViewModel; }
        set { Session["x"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionPrefix = Guid.NewGuid().ToString();
            Model = new XViewModel();
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //BindModel();
    }

    private void BindModel()
    {
        TextBox1.Text = Model.LicensePlate;
    }

    protected void RadAjaxManager1_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
    {
        if (e.Argument.ToLower() == "bind")
        {
            BindModel();
        }
    }
}