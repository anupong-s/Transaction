using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TransactionFrontEnd.ViewPayment;
using Telerik.Web.UI;
using TransactionModel.Utils;
using WebTransactionFrontEnd;
using CarPass.Transaction.Common;
using System.Configuration;

public partial class Pages_ViewPayment : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ajaxRequest();
        if (!IsPostBack)
        {
            InitParameters();
            InitModel();
            InitButton();
        }
    }

    private void InitButton()
    {
        string theme = Page.Theme;
        string path = ResolveUrl("~/App_Themes/" + theme + "/Images/Button/");

        btnPayInSlip.Image.ImageUrl = path + "PayInSlip.png";
        btnPayInSlip.Image.HoveredImageUrl = path + "PayInSlipHover.png";
        btnPayInSlip.CssClass = "pointer";

        btnCreditCard.Image.ImageUrl = path + "CreditCard.png";
        btnCreditCard.Image.HoveredImageUrl = path + "CreditCardHover.png";
        btnCreditCard.CssClass = "pointer";

        btnTaxInvReceipt.Image.ImageUrl = path + "TaxInvoice.png";
        btnTaxInvReceipt.Image.HoveredImageUrl = path + "TaxInvoiceHover.png";
        btnTaxInvReceipt.CssClass = "pointer";
    }

    // Bind controls when load or postback
    protected void Page_Prerender(object sender, EventArgs e)
    {
        BindModel();
    }

    private void ajaxRequest()
    {
        RadAjaxManager manager = RadAjaxManager.GetCurrent(this.Page);
        manager.AjaxRequest += new RadAjaxControl.AjaxRequestDelegate(TheRadAjaxManager_AjaxRequest);
    }

    protected void TheRadAjaxManager_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        BindModel();
        //udp.Update();
    }

    private void InitParameters()
    {
        var paymentId = Request.QueryString["PaymentId"];
        var ticket = Request.QueryString["Ticket"];

        if (string.IsNullOrEmpty(paymentId) || string.IsNullOrEmpty(ticket))
            throw new ArgumentException("MISSING_REQUIRED_PARAMETERS");

        //for test
        var isEncryption = ConfigurationManager.AppSettings["Encryption"];
        if (!isEncryption.ToBoolDefaultBool(false))
        {
            paymentId = paymentId.ToEncryption();
        }
        
        PaymentId = paymentId.ToDecryption();
        Ticket = ticket;
    }

    private void InitModel()
    {
        bool init = false;

        if (Model == null)
        {
            init = true;
        }
        else if (Model.PaymentId != PaymentId)
        {
            init = true;
        }

        if (init)
        {
            Model = new ViewPayment();
            Model.Initialize(Main, long.Parse(PaymentId), long.Parse(Ticket));
        }
    }

    private void BindModel()
    {
        CustomerIdmIdLabel.Text = Model.CustomerIdmId;
        CustomerCodeLabel.Text = Model.CustomerCode;
        CustomerAccountNoLabel.Text = Model.CustomerAccountNo;
        CustomerNameLabel.Text = Model.CustomerName;
        CustomerAccountNameLabel.Text = Model.CustomerAccountName;
        PaymentCodeLabel.Text = Model.PaymentCode;
        CustomerAddressLabel.Text = Model.CustomerAddress;
        PaymentIDLabel.Text = Model.PaymentId;
        Ref1Label.Text = Model.Ref1;
        Ref2Label.Text = Model.Ref2;
        Ref3Label.Text = Model.Ref3;
        RemarkLabel.Text = Model.Remark;
        RemainingAmountLabel.Text = Model.RemainingAmount;

        if (Model.PaymentStatus == PaymentStatusEnum.OVERPAID.ToString() ||
            Model.PaymentStatus == PaymentStatusEnum.PAID.ToString())
        {
            btnPayInSlip.Visible = false;
            btnCreditCard.Visible = false;
            btnTaxInvReceipt.Visible = true;
        }

        BindPaymentItemsGrid();
        BindInstallmentsGrid();
    }

    private void BindInstallmentsGrid()
    {
        InstallmentTotalLabel.Text = Model.RemainingAmount;
        RemainingLabel.Text = Model.RemainingAmount;

        InstallmentGrid.DataSource = Model.InstallmentItems;
        InstallmentGrid.DataBind();
    }

    private void BindPaymentItemsGrid()
    {
        //PaymentItemsGrid.VirtualItemCount = Model.NumberOfRecords;
        //PaymentItemsGrid.CurrentPageIndex = Model.CurrentPageIndex;
        //PaymentItemsGrid.PageSize = Model.PageSize;
        //GridSortExpression sortExpr = new GridSortExpression();
        //sortExpr.FieldName = Model.SortExpression;
        //sortExpr.SortOrder = Model.IsSortDescending ? GridSortOrder.Descending : GridSortOrder.Ascending;
        //PaymentItemsGrid.MasterTableView.SortExpressions.AddSortExpression(sortExpr);

        TotalLabel.Text = Model.Summary_Total;
        AdjustmenLabel.Text = Model.Summary_Adjustment;
        WitholdingTaxLabel.Text = Model.GrandTotal_WH_Amount;
        VATLabel.Text = Model.GrandTotal_VAT_Amount;
        GrandTotalLabel.Text = Model.GrandTotal_Net_Total;

        PaymentItemsGrid.DataSource = Model.Items;
        PaymentItemsGrid.DataBind();
    }

    private void DownloadPayInSlip()
    {
        byte[] pdf = Model.CreatePayInSlip();
        Response.CreatePDF(pdf, "payinslip");
    }

    private void DownloadTaxInvoice()
    {
        byte[] pdf = Model.CreateTaxInvoiceReceipt();
        Response.CreatePDF(pdf, "taxinvoice");
    }

    protected void btnPayInSlip_Click(object sender, EventArgs e)
    {
        try
        {
            DownloadPayInSlip();
        }
        catch (Exception ex)
        {
            Model.TheMain.LogsError(ex);
        }
    }
    protected void btnCreditCard_Click(object sender, EventArgs e)
    {
        string url = string.Format("PayCreditCard.aspx?PaymentId={0}&Ticket={1}",
                        Model.PaymentId, Ticket);

        Response.Redirect(url);
    }
    protected void btnTaxInvReceipt_Click(object sender, EventArgs e)
    {
        try
        {
            DownloadTaxInvoice();
        }
        catch (Exception ex)
        {
            Model.TheMain.LogsError(ex);
        }
    }
}

public static class WebExtension
{
    public static void CreatePDF(this HttpResponse response, byte[] pdf, string filename)
    {
        response.ClearContent();
        response.ClearHeaders();
        response.ContentType = "application/pdf";
        //Response.ContentType = @"application/octet-stream";
        response.AppendHeader("Content-Disposition", "attachment;filename=" + filename + ".pdf");
        response.OutputStream.Write(pdf, 0, pdf.Length);
        response.BufferOutput = true;
        response.BinaryWrite(pdf);
        response.Flush();
        response.End();
    }

}