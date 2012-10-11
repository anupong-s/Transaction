<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayCreditCard.aspx.cs" Inherits="Pages_PayCreditCard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="payFormCcard" name="payFormCcard" runat="server" method="post" action="https://ipay.bangkokbank.com/b2c/eng/payment/payForm.jsp">
    <input type="hidden" id="successUrl" name="successUrl" runat="server" />
    <input type="hidden" id="failUrl" name="failUrl" runat="server" />
    <input type="hidden" id="cancelUrl" name="cancelUrl" runat="server" />
    <input type="hidden" id="merchantId" name="merchantId" runat="server" />
    <input type="hidden" id="amount" name="amount" runat="server" />
    <input type="hidden" id="orderRef" name="orderRef" runat="server" />
    <input type="hidden" id="currCode" name="currCode" runat="server" />
    <input type="hidden" id="payType" name="payType" runat="server" />
    <input type="hidden" id="lang" name="lang" runat="server" />
    <input type="hidden" id="remark" name="remark" runat="server" />
    </form>
    <script type="text/javascript">
        document.getElementById("payFormCcard").submit();
    </script>
</body>
</html>
