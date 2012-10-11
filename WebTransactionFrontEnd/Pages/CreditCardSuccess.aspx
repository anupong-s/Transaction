<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreditCardSuccess.aspx.cs"
    Inherits="Pages_CreditCardSuccess" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Credit Card Success</title>
    <link href="../Styles/LayOut.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="wrapper">
        <div id="container">
            <div id="content">
                <asp:Label ID="MessageLabel" runat="server" Text=""></asp:Label><br />
                <asp:Button ID="btnOK" runat="server" Text="OK" /><br /><br />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
