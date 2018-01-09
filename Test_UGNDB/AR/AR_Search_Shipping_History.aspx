<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    EnableEventValidation="false" CodeFile="AR_Search_Shipping_History.aspx.vb" Inherits="AR_Search_Shipping_History"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
        <br />
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <asp:ValidationSummary ID="vsSearch" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgSearch" />
        <table style="width: 98%" runat="server" id="tblSearch" visible="false">
            <tr>
                <td class="p_text">
                    Start Ship Date:
                </td>
                <td>
                    <asp:TextBox ID="txtStartShipDate" runat="server" Width="85px" MaxLength="10"></asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftbeStartShipDate" runat="server" TargetControlID="txtStartShipDate"
                        FilterType="Custom, Numbers" ValidChars="/" />
                    <asp:ImageButton runat="server" ID="imgStartShipDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                    <ajax:CalendarExtender ID="ceStartShipDate" runat="server" TargetControlID="txtStartShipDate"
                        PopupButtonID="imgStartShipDate" />
                    <asp:RegularExpressionValidator ID="revStartShipDate" runat="server" ErrorMessage='Invalid Start Ship Date:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtStartShipDate" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px" ValidationGroup="vgSearch"><</asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator runat="server" ID="rfvStartShipDate" ControlToValidate="txtStartShipDate"
                        SetFocusOnError="true" ErrorMessage="Start ship date is required" ValidationGroup="vgSearch"></asp:RequiredFieldValidator>
                </td>
                <td class="p_text">
                    End Ship Date:
                </td>
                <td>
                    <asp:TextBox ID="txtEndShipDate" runat="server" Width="85px" MaxLength="10"></asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftbeEndShipDate" runat="server" TargetControlID="txtEndShipDate"
                        FilterType="Custom, Numbers" ValidChars="/" />
                    <asp:ImageButton runat="server" ID="imgEndShipDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                    <ajax:CalendarExtender ID="ceEndShipDate" runat="server" TargetControlID="txtEndShipDate"
                        PopupButtonID="imgEndShipDate" />
                    <asp:RegularExpressionValidator ID="revEndShipDate" runat="server" ErrorMessage='Invalid End Ship Date:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtEndShipDate" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px" ValidationGroup="vgSearch"><</asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    UGN Facility:
                </td>
                <td>
                    <asp:DropDownList ID="ddUGNFacility" runat="server">
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
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
                <td class="p_text">
                    CABBV:
                </td>
                <td>
                    <asp:DropDownList ID="ddCABBV" runat="server">
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    BPCS Part No:
                </td>
                <td>
                    <asp:TextBox ID="txtBPCSPartNo" runat="server" MaxLength="15"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Invoice No.:
                </td>
                <td>
                    <asp:TextBox ID="txtINVNo" runat="server" Width="100px"></asp:TextBox>
                </td>
                <td class="p_text">
                    Price Code:
                </td>
                <td>
                    <asp:DropDownList ID="ddPriceCode" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    RANNo:
                </td>
                <td>
                    <asp:TextBox ID="txtRANNo" runat="server" MaxLength="15"></asp:TextBox>
                </td>
                <td class="p_text">
                    P.O. No.:
                </td>
                <td>
                    <asp:TextBox ID="txtPONO" runat="server" MaxLength="15"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Start REQ Date:
                </td>
                <td>
                    <asp:TextBox ID="txtStartREQDate" runat="server" Width="85px" MaxLength="10"></asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftbeStartREQDate" runat="server" TargetControlID="txtStartREQDate"
                        FilterType="Custom, Numbers" ValidChars="/" />
                    <asp:ImageButton runat="server" ID="imgStartREQDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                    <ajax:CalendarExtender ID="ceStartREQDate" runat="server" TargetControlID="txtStartREQDate"
                        PopupButtonID="imgStartREQDate" />
                    <asp:RegularExpressionValidator ID="revStartREQDate" runat="server" ErrorMessage='Invalid Start REQ Date:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtStartREQDate" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px" ValidationGroup="vgSearch"><</asp:RegularExpressionValidator>
                </td>
                <td class="p_text">
                    End REQ Date:
                </td>
                <td>
                    <asp:TextBox ID="txtEndREQDate" runat="server" Width="85px" MaxLength="10"></asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftbeEndREQDate" runat="server" TargetControlID="txtEndREQDate"
                        FilterType="Custom, Numbers" ValidChars="/" />
                    <asp:ImageButton runat="server" ID="imgEndREQDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                    <ajax:CalendarExtender ID="ceEndREQDate" runat="server" TargetControlID="txtEndREQDate"
                        PopupButtonID="imgEndREQDate" />
                    <asp:RegularExpressionValidator ID="revEndREQDate" runat="server" ErrorMessage='Invalid End REQ Date:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtEndREQDate" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px" ValidationGroup="vgSearch"><</asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="4">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CausesValidation="true" ValidationGroup="vgSearch" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="true" ValidationGroup="vgSearch" />
                    <asp:Button ID="btnExportToExcel" runat="server" Text="Export to Excel" CausesValidation="true"
                        ValidationGroup="vgSearch" />
                </td>
            </tr>
            <tr>
                <td class="p_textbold">
                    Quantity Shipped Total:
                </td>
                <td class="c_textbold">
                    <asp:Label runat="server" ID="lblShippingQuantityTotal"></asp:Label>
                </td>
                <td class="p_textbold">
                    Sales Total:
                </td>
                <td class="c_textbold">
                    <asp:Label runat="server" ID="lblShippingSalesTotal"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <asp:GridView ID="gvShippingInfo" runat="server" DataSourceID="odsShippingInfo" PageSize="100"
            AllowPaging="True" AllowSorting="True" Width="98%" AutoGenerateColumns="False"
            Visible="false" CellPadding="2">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <EmptyDataTemplate>
                No records found.
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField HeaderText="Ship Date" SortExpression="SortTrueShipDate">
                    <ItemTemplate>
                        <asp:Label ID="lblSHPDTE" runat="server" Text='<%# Bind("ShowShipDate") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Req Date" SortExpression="SortTrueReqDate">
                    <ItemTemplate>
                        <asp:Label ID="lblREQDAT" runat="server" Text='<%# Bind("ShowReqDate") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="UGN Facility" SortExpression="UGNFacilityName">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:Label ID="lblUGNFacilityName" runat="server" Text='<%# Bind("UGNFacilityName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="SOLDTO" SortExpression="SoldTo">
                    <ItemTemplate>
                        <asp:Label ID="lblSoldTo" runat="server" Text='<%# Bind("SoldTo") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="CABBV" SortExpression="CABBV">
                    <ItemTemplate>
                        <asp:Label ID="lblCABBV" runat="server" Text='<%# Bind("CABBV") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Part No" SortExpression="PARTNO">
                    <ItemTemplate>
                        <asp:Label ID="lblPARTNO" runat="server" Text='<%# Bind("PARTNO") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Price Code" SortExpression="PRCCDE">
                    <ItemTemplate>
                        <asp:Label ID="lblPriceCodeName" runat="server" Text='<%# Bind("PriceCodeName") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Invoice No" SortExpression="INVNO">
                    <ItemTemplate>
                        <asp:Label ID="lblINVNO" runat="server" Text='<%# Bind("INVNO") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="RANNo" SortExpression="RANNO">
                    <ItemTemplate>
                        <asp:Label ID="lblRANNo" runat="server" Text='<%# Bind("RANNO") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="P.O. No." SortExpression="INVNO">
                    <ItemTemplate>
                        <asp:Label ID="lblPONO" runat="server" Text='<%# Bind("PONO") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Quantity Shipped" SortExpression="QTYSHP">
                    <ItemTemplate>
                        <asp:Label ID="lblQTYSHP" runat="server" Text='<%# Bind("QTYSHP") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Current Price" SortExpression="RELPRC">
                    <ItemTemplate>
                        <asp:Label ID="lblRELPRC" runat="server" Text='<%# Bind("RELPRC") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Revenue" SortExpression="Revenue">
                    <ItemTemplate>
                        <asp:Label ID="lblRevenue" runat="server" Text='<%# Bind("Revenue") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsShippingInfo" runat="server" SelectMethod="GetARShippingHistory"
            TypeName="ARGroupModule" OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddUGNFacility" Name="COMPNY" PropertyName="SelectedValue"
                    Type="String" />
                <asp:ControlParameter ControlID="ddCABBV" Name="CABBV" PropertyName="SelectedValue"
                    Type="String" />
                <asp:ControlParameter ControlID="ddSoldTo" Name="SOLDTO" PropertyName="SelectedValue"
                    Type="String" />
                <asp:ControlParameter ControlID="txtBPCSPartNo" Name="PARTNO" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="ddPriceCode" Name="PRCCDE" PropertyName="SelectedValue"
                    Type="String" />
                <asp:ControlParameter ControlID="txtStartShipDate" Name="StartShipDate" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="txtEndShipDate" Name="EndShipDate" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="txtINVNo" Name="INVNO" PropertyName="Text" Type="String" />
                <asp:ControlParameter ControlID="txtStartReqDate" Name="StartReqDate" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="txtEndReqDate" Name="EndReqDate" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="txtRANNo" Name="RANNO" PropertyName="Text" Type="String" />
                <asp:ControlParameter ControlID="txtPONo" Name="PONO" PropertyName="Text" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
