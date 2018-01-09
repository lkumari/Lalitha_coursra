<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    MaintainScrollPositionOnPostback="true" CodeFile="AR_Part_Accrual_Wizard.aspx.vb"
    Inherits="AR_Part_Accrual_Wizard" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server">
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin" />
        <asp:ValidationSummary ID="vsSave" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgSave" />
        <table>
            <tr>
                <td class="p_textbold" style="white-space: nowrap;">
                    AR Event ID:
                </td>
                <td>
                    <asp:Label ID="lblAREID" runat="server" SkinID="MessageLabelSkin"></asp:Label>
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
                    Ship Date From:
                </td>
                <td class="c_text">
                    <asp:Label runat="server" ID="lblShipDateFrom"></asp:Label>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Ship Date To:
                </td>
                <td class="c_text">
                    <asp:Label runat="server" ID="lblShipDateTo"></asp:Label>
                </td>
                <td>
                    &nbsp;
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
                <td colspan="2">
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
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Button runat="server" ID="btnFilterPartNo" Text="Search Part List" ValidationGroup="vgSave" />
                    <asp:Button runat="server" ID="btnClearFilterPartNo" Text="Clear Part Search" />
                </td>
            </tr>
        </table>
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
                    <ItemStyle HorizontalAlign="left" />
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
        <br />
        <asp:Button runat="server" ID="btnFilterPriceCode" Text="Filter Price Code List" />
        <asp:Button runat="server" ID="btnClearFilterPriceCode" Text="Clear Price Code Filters" />
        <br />
        <h2>
            Price Code is required.</h2>
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
        <asp:Button runat="server" ID="btnFilterSOLDTO" Text="Filter SOLDTO List" />
        <asp:Button runat="server" ID="btnClearFilterSOLDTO" Text="Clear SOLDTO Filters" />
        <br />
        <h2>
            SOLDTO and CABBV are optioanl.</h2>
        <br />
        <asp:GridView runat="server" ID="gvSOLDTO" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" PageSize="50" DataSourceID="odsSOLDTO" DataKeyNames="SOLDTO,CUSNM"
            EmptyDataText="No SOLDTOs Found" CellPadding="3" Width="98%">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:TemplateField HeaderText="Select SOLDTO">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbSelect" runat="server" Enabled="true" AutoPostBack="true" ToolTip='<%# Bind("SOLDTO") %>'
                            OnCheckedChanged="cbSelectSOLDTO_OnCheckedChanged" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:BoundField DataField="SOLDTO" ReadOnly="True" HeaderText="SOLDTO">
                    <ItemStyle HorizontalAlign="center" />
                </asp:BoundField>
                <asp:BoundField DataField="CUSNM" ReadOnly="True" HeaderText="Customer Name">
                    <ItemStyle HorizontalAlign="left" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Matches AR Module">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbMatch" runat="server" Checked='<%# Bind("isMatch") %>' Enabled="false" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsSOLDTO" runat="server" SelectMethod="GetARShippingHistoryDynamically"
            TypeName="ARGroupModule">
            <SelectParameters>
                <asp:ControlParameter ControlID="lblAREID" Name="AREID" PropertyName="Text" Type="Int32" />
                <asp:Parameter DefaultValue="SOLDTO" Name="KeyColumn" Type="String" />
                <asp:Parameter DefaultValue="SOLDTO,CUSNM" Name="Columns" Type="String" />
                <asp:SessionParameter DefaultValue="" Name="SQLWhereClause" SessionField="SOLDTOWhereClause"
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <br />
        <asp:Button runat="server" ID="btnFilterCABBV" Text="Filter CABBV List" />
        <asp:Button runat="server" ID="btnClearFilterCABBV" Text="Clear CABBV Filters" />
        <br />
        <asp:GridView runat="server" ID="gvCABBV" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" PageSize="50" DataSourceID="odsCABBV" DataKeyNames="CABBV"
            EmptyDataText="No CABBVs Found" CellPadding="3" Width="98%">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:TemplateField HeaderText="Select CABBV">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbSelect" runat="server" Enabled="true" AutoPostBack="true" ToolTip='<%# Bind("CABBV") %>'
                            OnCheckedChanged="cbSelectCABBV_OnCheckedChanged" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:BoundField DataField="SOLDTO" ReadOnly="True" HeaderText="SOLDTO">
                    <ItemStyle HorizontalAlign="center" />
                </asp:BoundField>
                <asp:BoundField DataField="CABBV" ReadOnly="True" HeaderText="CABBV">
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
        <asp:ObjectDataSource ID="odsCABBV" runat="server" SelectMethod="GetARShippingHistoryDynamically"
            TypeName="ARGroupModule">
            <SelectParameters>
                <asp:ControlParameter ControlID="lblAREID" Name="AREID" PropertyName="Text" Type="Int32" />
                <asp:Parameter DefaultValue="CABBV" Name="KeyColumn" Type="String" />
                <asp:Parameter DefaultValue="SOLDTO,CABBV" Name="Columns" Type="String" />
                <asp:SessionParameter DefaultValue="" Name="SQLWhereClause" SessionField="CABBVWhereClause"
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
