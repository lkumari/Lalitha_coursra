<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    EnableEventValidation="false" MaintainScrollPositionOnPostback="true" CodeFile="AR_Event_Accrual.aspx.vb"
    Inherits="AR_Event_Accrual" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server">
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin" />
        <asp:ValidationSummary ID="vsSave" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgSave" />
        <table width="98%">
            <tr>
                <td class="p_textbold" style="white-space: nowrap;">
                    AR Event ID:
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblAREID" runat="server" SkinID="MessageLabelSkin"></asp:Label>
                </td>
                <td class="p_textbold" style="white-space: nowrap;">
                    Event Type:
                </td>
                <td class="c_textbold">
                    <asp:DropDownList ID="ddEventType" runat="server" Enabled="false">
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
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
               
            </tr>
            <tr>
                <td class="p_textbold" style="white-space: nowrap;">
                    Total Quantity Shipped:
                </td>
                <td class="c_text">
                    <asp:Label runat="server" ID="lblTotalQuantityShipped"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="p_textbold" style="white-space: nowrap;">
                    Total Current Price By Quantity Shipped:
                </td>
                <td class="c_text">
                    <asp:Label runat="server" ID="lblTotalShippedPriceByQuantityShipped"></asp:Label>
                </td>
                <td class="p_textbold" style="white-space: nowrap;">
                    Calculated Deduction/Recovert Amount
                    <br />
                    (Event Price Minus Current Price) By Quantity Shipped:
                </td>
                <td class="c_text">
                    <asp:Label runat="server" ID="lblCalculatedDeductionAmount"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="p_textbold" style="white-space: nowrap;">
                    Total Override Current Price By Quantity Shipped:
                </td>
                <td class="c_text">
                    <asp:Label runat="server" ID="lblTotalOverrideShippedPriceByQuantityShipped"></asp:Label>
                </td>
                <td class="p_textbold" style="white-space: nowrap;">
                    Override Calculated Deduction/Recovery Amount
                    <br />
                    (Event Price Minus Current (or Override) Price) By Quantity Shipped:
                </td>
                <td class="c_text">
                    <asp:Label runat="server" ID="lblOverrideCalculatedDeductionAmount"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="c_textbold">
                    Override Current Price Comment:
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtOverrideCurrentPriceComment" runat="server" Height="60px" TextMode="MultiLine"
                        Width="650px" ValidationGroup="vgSave"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="revOverrideCurrentPriceComment" runat="server" ControlToValidate="txtOverrideCurrentPriceComment"
                        Text="<" ErrorMessage="Override comment is required." SetFocusOnError="True"
                        ValidationGroup="vgSave"></asp:RequiredFieldValidator>
                    <br />
                    <asp:Label ID="lblOverrideCurrentPriceCommentCharCount" SkinID="MessageLabelSkin"
                        runat="server"></asp:Label>
                </td>
            </tr>
            <tr align="center">
                <td colspan="4">
                    <asp:Button ID="btnSave" runat="server"  Text="Save" ValidationGroup="vgSave" visible="false" />
                    <asp:Button ID="btnUpdateAccrual" runat="server" Text="Refresh Accrual Calculations" />
                </td>
            </tr>
        </table>
        <br />
        <asp:Label runat="server" SkinID="MessageLabelSkin" id="lblNote1" Text="Accounting team members can use this list to override groups of accruing data." Visible="false"></asp:Label>
        <asp:ValidationSummary ID="vsEditAccrualOverride" runat="server" DisplayMode="List"
            ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgEditAccrualOverride" />
        <asp:ValidationSummary ID="vsInsertAccrualOverride" runat="server" DisplayMode="List"
            ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgInsertAccrualOverride" />
        <asp:GridView runat="server" ID="gvAccrualOverride" AllowPaging="True" AllowSorting="True"
            DataKeyNames="RowID" AutoGenerateColumns="False" PageSize="15" DataSourceID="odsAccrualOverride"
            Width="98%" ShowFooter="true" EmptyDataText="No parts and/or ship dates have been selected by the accounting team members to override the current prices.">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
            <Columns>
                <asp:TemplateField HeaderText="Part No.">
                    <EditItemTemplate>
                        <asp:Label ID="lblEditAccrualOverridePartNo" runat="server" Text='<%# Bind("PARTNO") %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewAccrualOverridePartNo" runat="server" Text='<%# Bind("PARTNO") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddInsertPartNo" runat="server">
                        </asp:DropDownList>
                    </FooterTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Price Code">
                    <EditItemTemplate>
                        <asp:Label ID="lblEditAccrualOverridePriceCode" runat="server" Text='<%# Bind("ddPriceCodeName") %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewAccrualOverridePriceCode" runat="server" Text='<%# Bind("ddPriceCodeName") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddInsertPriceCode" runat="server">
                        </asp:DropDownList>
                    </FooterTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Start Ship Date">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditStartShipDate" runat="server" MaxLength="10" Width="75px" Text='<%# Bind("StartShipDate") %>'></asp:TextBox>
                        <asp:ImageButton runat="server" ID="imgEditStartShipDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                            AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                             />
                        <ajax:CalendarExtender ID="ceInsertEditShipDate" runat="server" TargetControlID="txtEditStartShipDate"
                            PopupButtonID="imgEditStartShipDate" />
                        <asp:RegularExpressionValidator ID="revEditStartShipDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                            ControlToValidate="txtEditStartShipDate" Font-Bold="True" ToolTip="MM/DD/YYYY"
                            ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                            Width="8px" ValidationGroup="vgEditAccrualOverride"><</asp:RegularExpressionValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewAccrualOverrideStartShipDate" runat="server" Text='<%# Bind("StartShipDate") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtInsertStartShipDate" runat="server" MaxLength="10" Width="75px"></asp:TextBox>
                        <asp:ImageButton runat="server" ID="imgInsertStartShipDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                            AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                        <ajax:CalendarExtender ID="ceInsertStartShipDate" runat="server" TargetControlID="txtInsertStartShipDate"
                            PopupButtonID="imgInsertStartShipDate" />
                        <asp:RegularExpressionValidator ID="revInsertStartShipDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                            ControlToValidate="txtInsertStartShipDate" Font-Bold="True" ToolTip="MM/DD/YYYY"
                            ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                            Width="8px" ValidationGroup="vgInsertAccrualOverride"><</asp:RegularExpressionValidator>
                    </FooterTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="End Ship Date">
                    <EditItemTemplate>
                          <asp:TextBox ID="txtEditEndShipDate" runat="server" MaxLength="10" Width="75px" Text='<%# Bind("EndShipDate") %>'></asp:TextBox>
                        <asp:ImageButton runat="server" ID="imgEditEndShipDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                            AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                        <ajax:CalendarExtender ID="ceEditEndShipDate" runat="server" TargetControlID="txtEditEndShipDate"
                            PopupButtonID="imgEditEndShipDate" />
                        <asp:RegularExpressionValidator ID="revEditEndShipDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                            ControlToValidate="txtEditEndShipDate" Font-Bold="True" ToolTip="MM/DD/YYYY"
                            ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                            Width="8px" ValidationGroup="vgEditAccrualOverride"><</asp:RegularExpressionValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewAccrualOverrideEndShipDate" runat="server" Text='<%# Bind("EndShipDate") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtInsertEndShipDate" runat="server" MaxLength="10" Width="75px"></asp:TextBox>
                        <asp:ImageButton runat="server" ID="imgInsertEndShipDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                            AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                        <ajax:CalendarExtender ID="ceInsertEndShipDate" runat="server" TargetControlID="txtInsertEndShipDate"
                            PopupButtonID="imgInsertEndShipDate" />
                        <asp:RegularExpressionValidator ID="revInsertEndShipDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                            ControlToValidate="txtInsertEndShipDate" Font-Bold="True" ToolTip="MM/DD/YYYY"
                            ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                            Width="8px" ValidationGroup="vgInsertAccrualOverride"><</asp:RegularExpressionValidator>
                    </FooterTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Override Current Price">
                    <EditItemTemplate>
                        <asp:TextBox runat="server" ID="txtEditAccrualOverrideCurrentPrice" Text='<%# Bind("Override_RELPRC") %>'
                            MaxLength="10"></asp:TextBox>
                        <asp:CompareValidator runat="server" ID="cvEditOverrideCurrentPrice" Operator="DataTypeCheck"
                            ValidationGroup="vgEditAccrualOverride" Type="double" Text="<" ControlToValidate="txtEditAccrualOverrideCurrentPrice"
                            ErrorMessage="Override current price must be a number." SetFocusOnError="True" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblViewAccrualOverrideCurrentPrice" Text='<%# Bind("Override_RELPRC") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox runat="server" ID="txtInsertAccrualOverrideCurrentPrice" MaxLength="10"></asp:TextBox>
                        <asp:CompareValidator runat="server" ID="cvInsertOverrideCurrentPrice" Operator="DataTypeCheck"
                            ValidationGroup="vgInsertAccrualOverride" Type="double" Text="<" ControlToValidate="txtInsertAccrualOverrideCurrentPrice"
                            ErrorMessage="Override current price must be a number." SetFocusOnError="True" />
                    </FooterTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <EditItemTemplate>
                        <asp:ImageButton ID="iBtnUpdate" runat="server" CommandName="Update" ImageUrl="~/images/save.jpg"
                            AlternateText="Update" ValidationGroup="vgEditAccrualOverride" />
                        <asp:ImageButton ID="iBtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                            ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="iBtnEdit" runat="server" CausesValidation="False" CommandName="Edit"
                            ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                        <asp:ImageButton ID="ibtnDetailDelete" runat="server" CausesValidation="False" CommandName="Delete"
                            ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:ImageButton ID="ibtnInsert" runat="server" CausesValidation="True" CommandName="Insert"
                            ImageUrl="~/images/save.jpg" AlternateText="Insert" ValidationGroup="vgInsertAccrualOverride" />&nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo" ImageUrl="~/images/undo-gray.jpg"
                            AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsAccrualOverride" runat="server" DeleteMethod="DeleteAREventAccrualOverrideCriteria"
            InsertMethod="InsertAREventAccrualOverrideCriteria" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetAREventAccrualOverrideCriteria" TypeName="AREventAccrualOverrideCriteriaBLL"
            UpdateMethod="UpdateAREventAccrualOverrideCriteria">
            <SelectParameters>
                <asp:ControlParameter ControlID="lblAREID" Name="AREID" PropertyName="Text" Type="Int32" />
            </SelectParameters>
            <DeleteParameters>
                <asp:ControlParameter ControlID="lblAREID" Name="AREID" PropertyName="Text" Type="Int32" />
                <asp:Parameter Name="original_RowID" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:ControlParameter ControlID="lblAREID" Name="AREID" PropertyName="Text" Type="Int32" />               
                <asp:Parameter Name="Override_RELPRC" Type="Double" />
                <asp:Parameter Name="StartShipDate" Type="String" />
                <asp:Parameter Name="EndShipDate" Type="String" />
                <asp:Parameter Name="original_RowID" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="AREID" Type="Int32" />
                <asp:Parameter Name="PartNo" Type="String" />
                <asp:Parameter Name="PRCCDE" Type="String" />
                <asp:Parameter Name="Override_RELPRC" Type="Double" />
                <asp:Parameter Name="StartShipDate" Type="String" />
                <asp:Parameter Name="EndShipDate" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
        <br />
        <table align="right">
            <tr>
                <td class="c_textbold">
                    <asp:Label runat="server" ID="lblOverrideCurrentPrice" Text="Push Override Current Price to all checked rows:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtOverrideCurrentPrice" MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvOverrideCurrentPrice" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtOverrideCurrentPrice"
                        ErrorMessage="Override current price must be numeric" SetFocusOnError="True" />
                </td>
            </tr>
            <tr align="right">
                <td colspan="2">                    
                    <asp:Button ID="btnSelectAllRows" runat="server" Text="Select All Rows" />
                    <asp:Button ID="btnUnselectAllRows" runat="server" Text="Deselect All Rows" />
                    <asp:Button ID="btnUpdateCurrentPrice" runat="server" Text="Override Current Price"
                        CausesValidation="true" ValidationGroup="vgSave" />
                </td>
            </tr>
        </table>
        <br />
        <br />
        <br />
        <br />
        <br />
        <h1>Accrual Details</h1>
        <table width="98%">
            <tr>
                <td align="center">
                    <asp:Button ID="btnExportToExcel" runat="server" Text="Export to Excel" CausesValidation="true" visible="false" />
                </td>
            </tr>
        </table>
        <asp:ValidationSummary ID="vsEditAccrual" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgEditAccrual" />
        <asp:GridView runat="server" ID="gvAccrual" AllowPaging="True" AllowSorting="True"
            DataKeyNames="RowID" AutoGenerateColumns="False" PageSize="150" DataSourceID="odsAccrual"
             EmptyDataText="No Accrual Details exist."
            Width="98%">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
            <Columns>
                <asp:BoundField DataField="FormattedSHPDTE" ReadOnly="True" HeaderText="Ship Date" SortExpression="SHPDTE">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="SOLDTO" ReadOnly="True" HeaderText="SOLDTO" SortExpression="SOLDTO">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="CABBV" ReadOnly="True" HeaderText="CABBV" SortExpression="CABBV">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="PARTNO" ReadOnly="True" HeaderText="Finished Good Part No."
                    SortExpression="PARTNO">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="PriceCodeName" ReadOnly="True" HeaderText="Price Code"
                    SortExpression="PriceCodeName">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="UGNFacilityName" ReadOnly="True" HeaderText="UGN Facility"
                    SortExpression="UGNFacilityName">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="INVNO" ReadOnly="True" HeaderText="Invoice No." SortExpression="INVNO">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="QTYSHP" ReadOnly="True" HeaderText="Qty Shp" SortExpression="QTYSHP">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="PRCDOLR" ReadOnly="True" HeaderText="Event Price" SortExpression="PRCDOLR">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="USE_RELPRC" ReadOnly="True" HeaderText="Current Price"
                    SortExpression="USE_RELPRC">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Override Current Price">
                    <EditItemTemplate>
                        <asp:TextBox runat="server" ID="txtEditOverrideCurrentPrice" Text='<%# Bind("Override_RELPRC") %>'
                            MaxLength="10"></asp:TextBox>
                        <asp:CompareValidator runat="server" ID="cvEditOverrideCurrentPrice" Operator="DataTypeCheck"
                            ValidationGroup="vgEditAccrual" Type="double" Text="<" ControlToValidate="txtEditOverrideCurrentPrice"
                            ErrorMessage="Override current price must be a number." SetFocusOnError="True" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblViewOverrideCurrentPrice" Text='<%# Bind("Override_RELPRC") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Select">
                    <ItemTemplate>
                        <asp:CheckBox ID="cbSelect" runat="server" Enabled="true" AutoPostBack="true" ToolTip='<%# Bind("RowID") %>'
                            OnCheckedChanged="cbSelectAccrualRow_OnCheckedChanged" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <EditItemTemplate>
                        <asp:ImageButton ID="iBtnUpdate" runat="server" CommandName="Update" ImageUrl="~/images/save.jpg"
                            AlternateText="Update" ValidationGroup="vgEditAccrual" />
                        <asp:ImageButton ID="iBtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                            ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="iBtnEdit" runat="server" CausesValidation="False" CommandName="Edit"
                            ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Accrual" ReadOnly="True" HeaderText="Accrual">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="OverrideAccrual" ReadOnly="True" HeaderText="Override Accrual">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsAccrual" runat="server" SelectMethod="GetAREventAccrual"
            UpdateMethod="UpdateAREventAccrualCurrentPrice" TypeName="ARGroupModule" OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:ControlParameter ControlID="lblAREID" Name="AREID" PropertyName="Text" Type="Int32" />
            </SelectParameters>
            <UpdateParameters>
                <asp:ControlParameter ControlID="lblAREID" Name="AREID" PropertyName="Text" Type="Int32" />
                <asp:Parameter Name="original_RowID" Type="Int32" />
                <asp:Parameter Name="Override_RELPRC" Type="Double" />
            </UpdateParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
