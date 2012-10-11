<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"
    Theme="Default" AutoEventWireup="true" CodeFile="ViewPayment.aspx.cs" Inherits="Pages_ViewPayment" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
    <telerik:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
    </telerik:RadAjaxManagerProxy>
    <%--</fieldset>--%>


    <div class="tableHead clear">
    </div>
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <%-----%>
            <td width="50%" valign="top">
                <table cellpadding="0" cellspacing="0" class="tableContent">
                    <tr>
                        <th colspan="2">
                        </th>
                    </tr>
                    <tr class="roll1">
                        <td class="tableContentTextFields PaddingLeft5">
                            <asp:Label ID="lblCustomerIdmId" runat="server" Text="Customer IDM ID"></asp:Label>
                        </td>
                        <td class="tableContentTextBox">
                            :
                            <asp:Label ID="CustomerIdmIdLabel" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr class="roll2">
                        <td class="tableContentTextFields PaddingLeft5">
                            <asp:Label ID="lblPaymentCode" runat="server" Text="Payment Code"></asp:Label>
                        </td>
                        <td class="tableContentTextBox">
                            :
                            <asp:Label ID="PaymentCodeLabel" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr class="roll1">
                        <td class="tableContentTextFields PaddingLeft5">
                            <asp:Label ID="lblCustomerCode" runat="server" Text="Customer Code"></asp:Label>
                        </td>
                        <td class="tableContentTextBox">
                            :
                            <asp:Label ID="CustomerCodeLabel" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr class="roll2">
                        <td class="tableContentTextFields PaddingLeft5">
                            <asp:Label ID="lblCustomerName" runat="server" Text="CustomerName"></asp:Label>
                        </td>
                        <td class="tableContentTextBox">
                            :
                            <asp:Label ID="CustomerNameLabel" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr class="roll1">
                        <td class="tableContentTextFields PaddingLeft5">
                            <asp:Label ID="lblCustomerAddress" runat="server" Text="Customer Address"></asp:Label>
                        </td>
                        <td class="tableContentTextBox">
                            :
                            <asp:Label ID="CustomerAddressLabel" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr class="roll2">
                        <td class="tableContentTextFields PaddingLeft5">
                            <asp:Label ID="lblRef1" runat="server" Text="Ref1"></asp:Label>
                        </td>
                        <td class="tableContentTextBox">
                            :
                            <asp:Label ID="Ref1Label" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr class="roll1">
                        <td class="tableContentTextFields PaddingLeft5">
                            <asp:Label ID="lblRef3" runat="server" Text="Ref3"></asp:Label>
                        </td>
                        <td class="tableContentTextBox">
                            :
                            <asp:Label ID="Ref3Label" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr class="roll2">
                        <td class="tableContentTextFields PaddingLeft5">
                            <asp:Label ID="lblRemainingAmount" runat="server" Text="Remaining Amount"></asp:Label>
                        </td>
                        <td class="tableContentTextBox">
                            :
                            <asp:Label ID="RemainingAmountLabel" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
            <td class="Barspace">
                <asp:Image ID="Image2" runat="server" SkinID="barBGspace" />
            </td>
            <td width="50%" valign="top">
                <table cellpadding="0" cellspacing="0" class="tableContent">
                    <tr>
                        <th colspan="2">
                        </th>
                    </tr>
                    <tr class="roll1">
                        <td class="tableContentTextFields PaddingLeft5">
                   
                        </td>
                        <td class="tableContentTextBox">
                        </td>
                    </tr>
                    <tr class="roll2">
                        <td class="tableContentTextFields PaddingLeft5">
                            <asp:Label ID="lblPaymentID" runat="server" Text="Payment ID"></asp:Label>
                        </td>
                        <td class="tableContentTextBox">
                            :
                            <asp:Label ID="PaymentIDLabel" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr class="roll1">
                        <td class="tableContentTextFields PaddingLeft5">
                            <asp:Label ID="lblCustomerAccountNo" runat="server" Text="Customer Account No."></asp:Label>
                        </td>
                        <td class="tableContentTextBox">
                            :
                            <asp:Label ID="CustomerAccountNoLabel" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr class="roll2">
                        <td class="tableContentTextFields PaddingLeft5">
                            <asp:Label ID="lblCustomerAccountName" runat="server" Text="Customer Account Name."></asp:Label>
                        </td>
                        <td class="tableContentTextBox">
                            :
                            <asp:Label ID="CustomerAccountNameLabel" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr class="roll1">
                        <td class="tableContentTextFields PaddingLeft5">
                        </td>
                        <td class="tableContentTextBox">
                        </td>
                    </tr>
                    <tr class="roll2">
                        <td class="tableContentTextFields PaddingLeft5">
                            <asp:Label ID="lblRef2" runat="server" Text="Ref2"></asp:Label>
                        </td>
                        <td class="tableContentTextBox">
                            :
                            <asp:Label ID="Ref2Label" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr class="roll1">
                        <td class="tableContentTextFields PaddingLeft5">
                            <asp:Label ID="lblRemark" runat="server" Text="Remark"></asp:Label>
                        </td>
                        <td class="tableContentTextBox">
                            :
                            <asp:Label ID="RemarkLabel" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr class="roll2">
                        <td class="tableContentTextFields PaddingLeft5">
                        </td>
                        <td class="tableContentTextBox">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div style="text-align: right; margin-top: 10px;" class="clear">
        <telerik:RadButton ID="btnPayInSlip" runat="server" ButtonType="ToggleButton" Width="41px"
            Height="47px" OnClick="btnPayInSlip_Click">
        </telerik:RadButton>
        <telerik:RadButton ID="btnCreditCard" runat="server" ButtonType="ToggleButton" Width="41px"
            Height="47px" OnClick="btnCreditCard_Click">
        </telerik:RadButton>
        <telerik:RadButton ID="btnTaxInvReceipt" runat="server" ButtonType="ToggleButton"
            Width="41px" Height="47px" Visible="False" OnClick="btnTaxInvReceipt_Click">
        </telerik:RadButton>
    </div>
    <div>
        <telerik:RadGrid ID="PaymentItemsGrid" runat="server" AutoGenerateColumns="False"
            GridLines="None" Skin="Grid" EnableEmbeddedSkins="False" Font-Bold="False" Width="100%"
            EnableViewState="False">
            <MasterTableView Font-Size="0.88em">
                <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                <RowIndicatorColumn>
                    <HeaderStyle Width="20px"></HeaderStyle>
                </RowIndicatorColumn>
                <ExpandCollapseColumn>
                    <HeaderStyle Width="20px"></HeaderStyle>
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridBoundColumn DataField="No" HeaderText="No." UniqueName="No">
                        <HeaderStyle HorizontalAlign="Center" Width="35px" CssClass="RadGridTHMiddle" />
                        <ItemStyle HorizontalAlign="Center" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Name" HeaderText="Name" UniqueName="Name">
                        <HeaderStyle HorizontalAlign="Center" Width="150px" 
                            CssClass="RadGridTHMiddle" />
                        <ItemStyle Width="80px" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="Quantity" HeaderText="Qty" UniqueName="Quantity">
                        <HeaderStyle HorizontalAlign="Center" Width="35px" CssClass="RadGridTHMiddle" />
                        <ItemStyle HorizontalAlign="Center" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="UnitPrice" HeaderText="Unit Price" UniqueName="UnitPrice">
                        <HeaderStyle HorizontalAlign="Center" Width="70px" CssClass="RadGridTHMiddle" />
                        <ItemStyle HorizontalAlign="Right" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="SubTotal" HeaderText="Sub-Total" UniqueName="SubTotal">
                        <HeaderStyle HorizontalAlign="Center" Width="70px" CssClass="RadGridTHMiddle" />
                        <ItemStyle HorizontalAlign="Right" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="WithholdingTaxPercent" HeaderText="W/H %" UniqueName="WithholdingTaxPercent">
                        <HeaderStyle HorizontalAlign="Center" Width="35px" CssClass="RadGridTHMiddle" />
                        <ItemStyle HorizontalAlign="Center" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="WithholdingTaxAmount" HeaderText="W/H Amount"
                        UniqueName="WithholdingTaxAmount">
                        <HeaderStyle HorizontalAlign="Center" Width="35px" CssClass="RadGridTHMiddle" />
                        <ItemStyle HorizontalAlign="Right" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="VatPercent" HeaderText="VAT %" UniqueName="VatPercent">
                        <HeaderStyle HorizontalAlign="Center" Width="35px" CssClass="RadGridTHMiddle" />
                        <ItemStyle HorizontalAlign="Center" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="VatAmount" HeaderText="VAT Amount" UniqueName="VatAmount">
                        <HeaderStyle HorizontalAlign="Center" Width="35px" CssClass="RadGridTHMiddle" />
                        <ItemStyle HorizontalAlign="Right" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="NetAmount" HeaderText="Net Total" UniqueName="NetAmount">
                        <HeaderStyle HorizontalAlign="Center" Width="70px" CssClass="RadGridTHMiddle" />
                        <ItemStyle HorizontalAlign="Right" />
                    </telerik:GridBoundColumn>
                </Columns>

<EditFormSettings>
<EditColumn InsertImageUrl="Update.gif" UpdateImageUrl="Update.gif" EditImageUrl="Edit.gif" CancelImageUrl="Cancel.gif"></EditColumn>
</EditFormSettings>
            </MasterTableView>

<FilterMenu EnableEmbeddedSkins="False"></FilterMenu>

            <HeaderContextMenu EnableImageSprites="True" CssClass="GridContextMenu GridContextMenu_Default">
            </HeaderContextMenu>
        </telerik:RadGrid>
    </div>
    <%-----------------------------------------------------------------------------------------------------%>
    <div class="barNomal" style="margin-top: 10px;">
        <asp:Label ID="lblInstallment" runat="server" Text="Installment"></asp:Label>
    </div>
    <div>
        <table border="0" cellpadding="0" cellspacing="0" width="700px">
            <tr>
                <td width="80%" valign="top">
                    <div>
                        <telerik:RadGrid ID="InstallmentGrid" runat="server" AutoGenerateColumns="False"
                            GridLines="None" Skin="Grid" EnableEmbeddedSkins="false" AllowPaging="True" PageSize="5">
                            <MasterTableView Font-Size="0.88em">
                                <CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="No" HeaderText="No" UniqueName="column">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="InstallmentId" HeaderText="Installment ID" UniqueName="column1">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="InstalledDate" HeaderText="Installed Date" UniqueName="column2">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="InstalledBy" HeaderText="Installed By" UniqueName="column3">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Method" HeaderText="Method" UniqueName="column4">
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Amount" HeaderText="Amount" UniqueName="column5">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>
                            <PagerStyle Mode="NumericPages" />
                            <HeaderContextMenu EnableImageSprites="True" CssClass="GridContextMenu GridContextMenu_Black">
                            </HeaderContextMenu>
                        </telerik:RadGrid>
                    </div>
                    <div align="right">
                        <table border="0" cellpadding="0" cellspacing="0" width="150px">
                            <tr align="center">
                                <td class="label">
                                    <asp:Label ID="lblInstallmentTotal" runat="server" Text="Total:"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="InstallmentTotalLabel" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr align="center">
                                <td class="label">
                                    <asp:Label ID="lblRemaining" runat="server" Text="Remaining:"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="RemainingLabel" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td valign="top" align="right">
                    <div align="right">
                        <table border="0" cellpadding="0" cellspacing="5" width="250px">
                            <tr align="right">
                                <td class="label">
                                    <asp:Label ID="lblTotal" runat="server" Text="Total:"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="TotalLabel" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr align="right">
                                <td class="label">
                                    <asp:Label ID="lblAdjustment" runat="server" Text="Adjustment:"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="AdjustmenLabel" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr align="right">
                                <td class="label">
                                    <asp:Label ID="lblWitholdingTax" runat="server" Text="Witholding Tax:"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="WitholdingTaxLabel" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr align="right">
                                <td class="label">
                                    <asp:Label ID="lblVAT" runat="server" Text="VAT:"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="VATLabel" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr align="right">
                                <td class="label">
                                    <asp:Label ID="lblGrandTotal" runat="server" Text="Grand Total:"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="GrandTotalLabel" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <telerik:RadWindowManager ID="TheRadWindowManager" runat="server">
    </telerik:RadWindowManager>
    <script type="text/javascript">
        function OpenPayInSlip(url) {
            var width = screen.width;
            var height = screen.height;
            var param = "toolbar=no,menubar=no,location=no,directories=no,scrollbars=no,status=no,resizable=no,width=" + width + ",height=" + height + "";
            window.open(url, null, param); return false;
        }
    </script>
</asp:Content>
