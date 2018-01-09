<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    MaintainScrollPositionOnPostback="true" CodeFile="AR_Invoices_On_Hold_Wizard.aspx.vb"
    Inherits="AR_Invoices_On_Hold_Wizard" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server">
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin" />
        <table>
            <tr>
                <td class="p_text" style="white-space: nowrap;">
                    AR Event ID:
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblAREID" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblFGPartNo" Text="Finished Good Part No.:" CssClass="p_text"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtFGPartNo" MaxLength="15"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Button runat="server" ID="btnFilterPartNo" Text="Filter Part List" />
                    <asp:Button runat="server" ID="btnClearFilterPartNo" Text="Clear Part Filters" />
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td align="center" style="p_text">
                    Please enter an estimated price if known:
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtEstimatedPrice" MaxLength="10"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <asp:Label runat="server" ID="lblNote1" Text="Below is a list of parts with 0 price holding up an invoice."></asp:Label>
        <br />
        <asp:GridView runat="server" ID="gvPartNo" AllowPaging="True" AllowSorting="True"
            DataKeyNames="PARTNO" AutoGenerateColumns="False" PageSize="15" DataSourceID="odsInvoicesOnHoldPartList"
            Width="98%">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
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
                <asp:TemplateField HeaderText="Matches AR Module">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbMatch" runat="server" Checked='<%# Bind("isMatch") %>' Enabled="false" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsInvoicesOnHoldPartList" runat="server" SelectMethod="GetInvoicesOnHoldPartList"
            TypeName="ARGroupModule">
            <SelectParameters>
                <asp:ControlParameter ControlID="lblAREID" Name="AREID" PropertyName="Text" Type="Int32" />
                <asp:ControlParameter ControlID="txtFGPartNo" Name="PartNo" PropertyName="Text" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:TextBox runat="server" ID="txtInvoicePartNo" MaxLength="15" Visible="false"></asp:TextBox>
        <br />
        <h2>
            Price Code is required</h2>
        <asp:GridView runat="server" ID="gvPriceCode" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" PageSize="15" DataSourceID="odsPriceCode" DataKeyNames="PRCCDE"
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
        <asp:ObjectDataSource ID="odsPriceCode" runat="server" SelectMethod="GetInvoicesOnHoldPriceCodeByPartList"
            TypeName="ARGroupModule">
            <SelectParameters>
                <asp:ControlParameter ControlID="lblAREID" Name="AREID" PropertyName="Text" Type="Int32" />
                <asp:ControlParameter ControlID="txtInvoicePartNo" Name="PartNo" PropertyName="Text"
                    Type="String" />
                <asp:Parameter Name="PriceCode" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:TextBox runat="server" ID="txtInvoicePriceCode" MaxLength="1" Visible="false"></asp:TextBox>
        <br />
        <asp:Button runat="server" ID="btnUpdate" Text="Update AR Event" />
        <asp:Button runat="server" ID="btnBackToAREvent" Text="Cancel" />
        <asp:Label ID="lblMessageBottom" runat="server" SkinID="MessageLabelSkin" />
        <br />
        <br />
        <br />
        <br />
        <asp:Label runat="server" ID="lblNote2" Text="Below is a list of invoices on hold."></asp:Label>
        <br />
        <asp:GridView runat="server" ID="gvInvoicesOnHold" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" PageSize="15" DataSourceID="odsInvoicesOnHold" Width="98%">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
            <Columns>
                <asp:BoundField DataField="INVNO" ReadOnly="True" HeaderText="Invoice No." SortExpression="INVNO">
                    <ItemStyle HorizontalAlign="Center" Font-Bold="true" />
                </asp:BoundField>
                <asp:BoundField DataField="SOLDTO" ReadOnly="True" HeaderText="SOLDTO" SortExpression="SOLDTO">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="CABBV" ReadOnly="True" HeaderText="CABBV" SortExpression="CABBV">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="DABBV" ReadOnly="True" HeaderText="DABBV" SortExpression="DABBV">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="PARTNO" ReadOnly="True" HeaderText="Finished Good Part No."
                    SortExpression="PARTNO">
                    <ItemStyle HorizontalAlign="center" Font-Bold="true" />
                </asp:BoundField>
                <asp:BoundField DataField="PRCCDE" ReadOnly="True" HeaderText="Price Code" SortExpression="PRCCDE">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="COMPNY" ReadOnly="True" HeaderText="UGN Facility" SortExpression="COMPNY">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="QTYSHP" ReadOnly="True" HeaderText="Qty Shp" SortExpression="QTYSHP">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="RELPRC" ReadOnly="True" HeaderText="Invoice On Hold Price"
                    SortExpression="RELPRC">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsInvoicesOnHold" runat="server" SelectMethod="GetInvoicesOnHold"
            TypeName="ARGroupModule">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtInvoicePartNo" Name="PartNo" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="txtInvoicePriceCode" Name="PriceCode" PropertyName="Text"
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
