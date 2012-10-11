using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TEST;

public partial class WebControl_PopupPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string key = Request["SK"];
            Model = Session[key] as ILicensePlatePopup;
        }
    }

    public ILicensePlatePopup Model
    {
        get { return Session["LicensePopup"] as ILicensePlatePopup; }
        set { Session["LicensePopup"] = value; }
    }
       
    protected void Button1_Click(object sender, EventArgs e)
    {
        Model.SelectLicensePlate(TextBox1.Text, TextBox2.Text, DropDownList1.SelectedItem.Text);
        ScriptManager.RegisterStartupScript(Button1, Button1.GetType(), "close", "Close();", true);
    }
}