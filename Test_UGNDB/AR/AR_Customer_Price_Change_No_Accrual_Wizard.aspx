<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    MaintainScrollPositionOnPostback="true" CodeFile="AR_Customer_Price_Change_No_Accrual_Wizard.aspx.vb"
    Inherits="AR_Customer_Price_Change_No_Accrual_Wizard" Title="Untitled Page" %>

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
            </tr>
        </table>
        <hr />
        <table width="58%">            
            <tr>
                <td class="p_textbold">
                    UGN Facility:&nbsp;<span style="color: red">*</span>
                </td>
                <td>
                    <asp:DropDownList ID="ddUGNFacility" runat="server" AutoPostBack="true" CausesValidation="true"
                        ValidationGroup="vgSave">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvFacility" runat="server" ControlToValidate="ddUGNFacility"
                        Text="<" ErrorMessage="UGN Facility is Required." SetFocusOnError="True" ValidationGroup="vgSave"></asp:RequiredFieldValidator>
                </td>
                <td align="left">
                    <span style="color: red">(UGN Facility is required)</span>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Sold To:
                </td>
                <td>
                    <asp:DropDownList ID="ddSoldTo" runat="server">
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="3" align="center">
                    <asp:Button runat="server" ID="btnFilterSoldTo" Text="Filter SoldTo List" CausesValidation="true"
                        ValidationGroup="vgSave" />
                    <asp:Button runat="server" ID="btnClearSoldTo" Text="Show All Possible SOLDTOs" />
                </td>
            </tr>
        </table>
        <hr />
        <h2>
            SOLDTO selection is required.</h2>
        <asp:GridView runat="server" ID="gvSOLDTO" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" PageSize="50" DataSourceID="odsSOLDTO" DataKeyNames="SOLDTO"
            CellPadding="3" Width="98%">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
            <Columns>
                <asp:TemplateField HeaderText="Select">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbSelect" runat="server" Enabled="true" AutoPostBack="true" ToolTip='<%# Bind("SOLDTO") %>'
                            OnCheckedChanged="cbSelectSOLDTO_OnCheckedChanged" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:BoundField DataField="SOLDTO" ReadOnly="True" HeaderText="SOLDTO">
                    <ItemStyle HorizontalAlign="Center" />
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
                <asp:SessionParameter Name="SQLWhereClause" SessionField="SOLDTOWhereClause" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <br />
        <asp:Button runat="server" ID="btnFilterPriceCode" Text="Filter Price Code List" />
        <asp:Button runat="server" ID="btnClearPriceCode" Text="Clear Price Code Filters" />
        <h2>
            Price Code is optional.</h2>
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
                <asp:BoundField DataField="PRCCDE" ReadOnly="True">
                    <HeaderStyle CssClass="none" />
                    <ItemStyle HorizontalAlign="center" CssClass="none" />
                </asp:BoundField>
                <asp:BoundField DataField="ddPriceCodeName" ReadOnly="True" HeaderText="Price Code">
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
        <asp:Button runat="server" ID="btnFilterCABBV" Text="Filter CABBV List" />
        <asp:Button runat="server" ID="btnClearCABBV" Text="Clear CABBV Selections" />
        <br />
        <h2>
            CABBV is optional.</h2>
        <asp:GridView runat="server" ID="gvCABBV" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" PageSize="50" DataSourceID="odsCABBV" DataKeyNames="CABBV"
            EmptyDataText="No CABBVs Found" Width="98%">
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
                <asp:BoundField DataField="CABBV" ReadOnly="True" HeaderText="CABBV">
                    <ItemStyle HorizontalAlign="center" />
                </asp:BoundField>
                <asp:BoundField DataField="SOLDTO" ReadOnly="True" HeaderText="SOLDTO">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="PRCCDE" ReadOnly="True">
                    <HeaderStyle CssClass="none" />
                    <ItemStyle HorizontalAlign="center" CssClass="none" />
                </asp:BoundField>
                <asp:BoundField DataField="ddPriceCodeName" ReadOnly="True" HeaderText="Price Code">
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
                <asp:Parameter DefaultValue="CABBV,SOLDTO,PRCCDE" Name="Columns" Type="String" />
                <asp:SessionParameter DefaultValue="" Name="SQLWhereClause" SessionField="CABBVWhereClause"
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <br />
        <asp:Button runat="server" ID="btnFilterPartNo" Text="Search Part List" ValidationGroup="vgSave" />
        <asp:Button runat="server" ID="btnClearFilterPartNo" Text="Clear Part Search" />
        <h2>
            Part No. selection is optional.</h2>
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
                 <asp:BoundField DataField="CABBV" ReadOnly="True" HeaderText="CABBV">
                    <ItemStyle HorizontalAlign="center" />
                </asp:BoundField>
                <asp:BoundField DataField="SOLDTO" ReadOnly="True" HeaderText="SOLDTO">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="PRCCDE" ReadOnly="True">
                    <HeaderStyle CssClass="none" />
                    <ItemStyle HorizontalAlign="center" CssClass="none" />
                </asp:BoundField>
                <asp:BoundField DataField="ddPriceCodeName" ReadOnly="True" HeaderText="Price Code">
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
                <asp:Parameter DefaultValue="PARTNO,IDESC,CABBV,SOLDTO,PRCCDE" Name="Columns" Type="String" />
                <asp:SessionParameter DefaultValue="" Name="SQLWhereClause" SessionField="PARTNOWhereClause"
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <br />
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
