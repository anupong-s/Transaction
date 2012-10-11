<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestPageX.aspx.cs" Inherits="WebControl_TestPageX" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="TextBox1" 
                        UpdatePanelRenderMode="Inline" />
                    <telerik:AjaxUpdatedControl ControlID="TextBox2" 
                        UpdatePanelRenderMode="Inline" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div>
        <asp:TextBox ID="TextBox1" runat="server" ReadOnly="true"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="..." OnClientClick="openWin('PopupPage.aspx?sk=x'); return false" />
    </div>
    <div>
        <asp:TextBox ID="TextBox2" runat="server" ReadOnly="true"></asp:TextBox>
        <asp:Button ID="Button2" runat="server" Text="..." OnClientClick="openWin('PopupPage.aspx?sk=x'); return false" />
    </div>
    <br />
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
    </telerik:RadWindowManager>
    <telerik:RadWindow ID="RadWindow1" runat="server">
    </telerik:RadWindow>
    </form>
    <script type="text/javascript">
        function openWin(url) {
            var manager = $find("RadWindowManager1");
            manager.open(url);
        }

        function refreshPage(arg) {
            var ajax = $find("RadAjaxManager1");
            ajax.ajaxRequest(arg);
        }
    </script>
</body>
</html>
