<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    MaintainScrollPositionOnPostback="true" CodeFile="AR_Price_Change_No_Accrual_Wizard_Current.aspx.vb"
    Inherits="AR_Price_Change_No_Accrual_Wizard_Current" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server">
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin" />
        <asp:ValidationSummary ID="vsSave" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgSave" />
        <table>
            <tr>
                <td class="p_text" style="white-space: nowrap;">
                    AR Event ID:
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblAREID" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Button runat="server" ID="btnCustomerWizard" Text="Select By Customer" />
                </td>
            </tr>
        </table>
        <hr />
        <table>
            <tr>
                <td class="p_text">
                </td>
                <td class="c_text">
                    <asp:Label runat="server" ID="lblCustApprvEffdate" Visible="false"></asp:Label>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="p_textbold">
                    UGN Facility:&nbsp;<span style="color: red">*</span>
                </td>
                <td>
                    <asp:DropDownList ID="ddUGNFacility" runat="server" AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvFacility" runat="server" ControlToValidate="ddUGNFacility"
                        Text="<" ErrorMessage="UGN Facility is Required." SetFocusOnError="True" ValidationGroup="vgSave"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <span style="color: red">(UGN Facility is required)</span>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="p_textbold">
                    <asp:Label runat="server" ID="lblCustomerPartNo" Text="Customer Part No.:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtCustomerPartNo" MaxLength="30"></asp:TextBox>
                    <asp:ImageButton ID="iBtnCustomerPartNo" runat="server" ImageUrl="~/images/Search.gif"
                        ToolTip="Click here to search for the finished good part number." />
                </td>
                <td>
                    <span style="color: red">(You can search by customer part number to get ALL finished
                        goods associated to it. If so, then leave the Finished Good field blank.)</span>
                </td>
            </tr>
            <tr>
                <td class="p_textbold">
                    <asp:Label runat="server" ID="lblFGPartNo" Text="Finished Good Part No.:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtFGPartNo" MaxLength="15"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="3">
                    <asp:Button runat="server" ID="btnFilterPartNo" Text="Filter Part List" ValidationGroup="vgSave"
                        CausesValidation="true" />
                    <asp:Button runat="server" ID="btnClearFilterPartNo" Text="Clear Part Filters" />
                    <asp:Button runat="server" ID="btnFuturePriceChangeNoAccrualWizard" Text="Switch to Wizard for Future or Pending Parts."
                        CausesValidation="false" />
                </td>
            </tr>
        </table>
        <asp:TextBox runat="server" ID="txtTemp" CssClass="none"></asp:TextBox>
        <br />
        <h2>
            Part No. selection is required.</h2>
        <asp:GridView runat="server" ID="gvPartNo" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" PageSize="50" DataSourceID="odsPARTNO" DataKeyNames="PARTNO"
            EmptyDataText="No Parts Found" CellPadding="3" Width="98%">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:TemplateField HeaderText="Select PARTNO">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbSelect" runat="server" Enabled="true" AutoPostBack="true" ToolTip='<%# Bind("PARTNO") %>'
                            OnCheckedChanged="cbSelectPartNo_OnCheckedChanged" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:BoundField DataField="PARTNO" ReadOnly="True" HeaderText="Finished Good BPCS Part No.">
                    <ItemStyle HorizontalAlign="center" Font-Bold="true" />
                </asp:BoundField>
                <asp:BoundField DataField="IDESC" ReadOnly="True" HeaderText="Part Name">
                    <ItemStyle HorizontalAlign="center" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Matches AR Module">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbMatch" runat="server" Checked='<%# Bind("isMatch") %>' Enabled="false" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsPARTNO" runat="server" SelectMethod="GetARShippingHistoryDynamically"
            TypeName="ARGroupModule">
            <SelectParameters>
                <asp:ControlParameter ControlID="lblAREID" Name="AREID" PropertyName="Text" Type="Int32" />
                <asp:Parameter DefaultValue="PARTNO" Name="KeyColumn" Type="String" />
                <asp:Parameter DefaultValue="PARTNO,IDESC" Name="Columns" Type="String" />
                <asp:SessionParameter DefaultValue="" Name="SQLWhereClause" SessionField="PARTNOWhereClause"
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:Button runat="server" ID="btnFilterPriceCode" Text="Filter Price Code List" />
        <asp:Button runat="server" ID="btnClearFilterPriceCode" Text="Clear Selected Part No(s)." />
        <br />
        <h2>
            Price Code is required</h2>
        <asp:GridView runat="server" ID="gvPriceCode" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" PageSize="50" DataSourceID="odsPriceCode" DataKeyNames="PRCCDE"
            EmptyDataText="No Price Codes Found" CellPadding="3" Width="98%">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:TemplateField HeaderText="Select Price Code">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbSelect" runat="server" Enabled="true" AutoPostBack="true" ToolTip='<%# Bind("PRCCDE") %>'
                            OnCheckedChanged="cbSelectPriceCode_OnCheckedChanged" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:BoundField DataField="PRCCDE" ReadOnly="True" HeaderText="Price Code">
                    <ItemStyle HorizontalAlign="center" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Matches AR Module">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbMatch" runat="server" Checked='<%# Bind("isMatch") %>' Enabled="false" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsPriceCode" runat="server" SelectMethod="GetARShippingHistoryDynamically"
            TypeName="ARGroupModule">
            <SelectParameters>
                <asp:ControlParameter ControlID="lblAREID" Name="AREID" PropertyName="Text" Type="Int32" />
                <asp:Parameter DefaultValue="PRCCDE" Name="KeyColumn" Type="String" />
                <asp:Parameter DefaultValue="PRCCDE" Name="Columns" Type="String" />
                <asp:SessionParameter DefaultValue="" Name="SQLWhereClause" SessionField="PRCCDEWhereClause"
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <br />
        <asp:RadioButtonList runat="server" ID="rbUpdateType">
            <asp:ListItem Text="Append to selected items in the AR Event" Value="A" Selected="True"></asp:ListItem>
            <asp:ListItem Text="Replace all selected items in the AR Event" Value="R"></asp:ListItem>
        </asp:RadioButtonList>
        <asp:Button runat="server" ID="btnUpdate" Text="Update AR Event" />
        <asp:Button runat="server" ID="btnBackToAREvent" Text="Cancel" />
        <asp:Label ID="lblMessageBottom" runat="server" SkinID="MessageLabelSkin" />
    </asp:Panel>
</asp:Content>
