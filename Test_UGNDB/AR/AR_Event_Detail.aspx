<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" MaintainScrollPositionOnPostback="true"
    EnableEventValidation="false" AutoEventWireup="false" CodeFile="AR_Event_Detail.aspx.vb"
    Inherits="AR_Event_Detail" Title="AR Event Detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server">
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin" />
        <asp:ValidationSummary ID="vsSave" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgSave" />
        <asp:ValidationSummary ID="vsVoid" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgVoid" />
        <asp:Label ID="lblTeamMemberID" runat="server" Visible="false" Text="0"></asp:Label>
        <table width="98%" border="0">
            <tr>
                <td class="p_textbold" style="white-space: nowrap;">
                    <asp:Label ID="lblEventStatus" runat="server" Text="Event Status:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddEventStatus" runat="server" Enabled="false">
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="p_textbold" style="white-space: nowrap;">
                    <asp:Label ID="lblCloseDateLabel" runat="server" Text="Date Closed:" Visible="false"></asp:Label>
                </td>
                <td style="white-space: nowrap;">
                    <asp:Label ID="lblCloseDateValue" runat="server" Visible="false" SkinID="MessageLabelSkin"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="p_textbold" style="white-space: nowrap;">
                    AR Event ID:
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblAREID" runat="server" SkinID="MessageLabelSkin"></asp:Label>
                    <asp:Label ID="lblMessageAREIDNew" runat="server" Text="New ID has not been assigned yet."
                        Visible="false" SkinID="MessageLabelSkin"></asp:Label>
                </td>
                <td class="p_textbold" style="white-space: nowrap;">
                    Account Manager:
                </td>
                <td style="white-space: nowrap;">
                    <asp:DropDownList ID="ddAccountManager" runat="server" AutoPostBack="true">
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="reqAccountManager" runat="server" ControlToValidate="ddAccountManager"
                        Text="<" ErrorMessage="Account Manager is Required." SetFocusOnError="True" ValidationGroup="vgSave"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="p_textbold" style="white-space: nowrap;">
                    Event Type:
                </td>
                <td class="c_textbold">
                    <asp:DropDownList ID="ddEventType" runat="server">
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvEventType" runat="server" ControlToValidate="ddEventType"
                        Text="<" ErrorMessage="Event Type is Required." SetFocusOnError="True" ValidationGroup="vgSave"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label runat="server" ID="lblCustApprvEffDateNote" Visible="false" SkinID="MessageLabelSkin"
                        Text="(For accruing events, please use a past effective date.)"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="p_textbold" style="white-space: nowrap;">
                    Customer Approved Effective Date:
                </td>
                <td style="white-space: nowrap;">
                    <asp:TextBox ID="txtCustApprvEffDate" runat="server" Width="85px" MaxLength="10"></asp:TextBox>
                    <asp:ImageButton runat="server" ID="imgCustApprvEffDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                    <ajax:CalendarExtender ID="cbeCustApprvEffDate" runat="server" TargetControlID="txtCustApprvEffDate"
                        PopupButtonID="imgCustApprvEffDate" />
                    <asp:RegularExpressionValidator ID="revCustApprvEffDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtCustApprvEffDate" Font-Bold="True" ToolTip="MM/DD/YYYY"
                        ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px" ValidationGroup="vgSave" Text="<"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="rfvCustApprvEffDate" runat="server" ControlToValidate="txtCustApprvEffDate"
                        Text="<" ErrorMessage="Customer Approved Effective Date is Required." SetFocusOnError="True"
                        ValidationGroup="vgSave"></asp:RequiredFieldValidator>
                    &nbsp;
                </td>
                <td class="p_textbold" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblCustApprvEndDate" Text="Customer Approved End Date:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCustApprvEndDate" runat="server" Width="85px" MaxLength="10"></asp:TextBox>
                    <asp:ImageButton runat="server" ID="imgCustApprvEndDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                    <ajax:CalendarExtender ID="cbeCustApprvEndDate" runat="server" TargetControlID="txtCustApprvEndDate"
                        PopupButtonID="imgCustApprvEndDate" />
                    <asp:RegularExpressionValidator ID="revCustApprvEndDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtCustApprvEndDate" Font-Bold="True" ToolTip="MM/DD/YYYY"
                        ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px" ValidationGroup="vgSave" Text="<"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td class="p_textbold" style="white-space: nowrap;">
                    Event Desc:
                </td>
                <td colspan="3" style="white-space: nowrap;">
                    <asp:TextBox ID="txtEventDesc" runat="server" Height="100px" TextMode="MultiLine"
                        Width="650px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvEventDesc" runat="server" ControlToValidate="txtEventDesc"
                        Text="<" ErrorMessage="Event Description is Required." SetFocusOnError="True"
                        ValidationGroup="vgSave"></asp:RequiredFieldValidator>
                    <br />
                    <asp:Label ID="lblEventDescCharCount" SkinID="MessageLabelSkin" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="p_textbold" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblDeductionReason" Text="Deduction Reason from Accounting:"
                        Visible="false"></asp:Label>
                </td>
                <td colspan="3" style="white-space: nowrap;">
                    <asp:TextBox ID="txtDeductionReason" runat="server" Height="100px" TextMode="MultiLine"
                        Width="650px"></asp:TextBox>
                    <br />
                    <asp:Label ID="lblDeductionReasonCharCount" SkinID="MessageLabelSkin" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="p_textbold" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblVoidReason" Text="Void Reason:" Visible="false"
                        SkinID="MessageLabelSkin"></asp:Label>
                </td>
                <td colspan="3" style="white-space: nowrap;">
                    <asp:TextBox ID="txtVoidReason" runat="server" Height="60px" TextMode="MultiLine"
                        MaxLength="150" Visible="false" Width="600px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvVoidReason" runat="server" ControlToValidate="txtVoidReason"
                        Enabled="false" Text="<" ErrorMessage="Void Reason is Required." SetFocusOnError="True"
                        ValidationGroup="vgVoid"></asp:RequiredFieldValidator>
                    <br />
                    <asp:Label ID="lblVoidReasonCharCount" SkinID="MessageLabelSkin" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td colspan="1">
                    <asp:CheckBox runat="server" ID="cbCustomerApproved" Text="Approved By Customer"
                        Enabled="false" />
                </td>
                <td class="p_textbold" style="white-space: nowrap;">
                    <asp:Label ID="lblCreditDebitDate" runat="server" Text="Credit/Debit Date:"></asp:Label>
                </td>
                <td style="white-space: nowrap;">
                    <asp:TextBox ID="txtCreditDebitDate" runat="server" Width="85px" MaxLength="10" Enabled="false"></asp:TextBox>
                    <asp:ImageButton runat="server" ID="imgCreditDebitDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                    <ajax:CalendarExtender ID="ceCreditDebitDate" runat="server" TargetControlID="txtCreditDebitDate"
                        PopupButtonID="imgCreditDebitDate" />
                    <asp:RegularExpressionValidator ID="revCreditDebitDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtCreditDebitDate" Font-Bold="True" ToolTip="MM/DD/YYYY"
                        ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px" ValidationGroup="vgSave" Text="<"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:CheckBox runat="server" ID="cbPriceUpdatedByAccounting" Text="Price Updated by Accounting"
                        Enabled="false" Visible="false" />
                    <asp:Label runat="server" ID="lblPriceChangeDate" Visible="false" SkinID="MessageLabelSkin"></asp:Label>
                </td>
                <td class="p_textbold">
                    <asp:Label ID="lblCreditDebitMemo" runat="server" Text="Credit/Debit Memo:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCreditDebitMemo" runat="server" Width="85px" MaxLength="10" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="p_textbold">
                    <asp:Label ID="lblBPCSInvoiceNo" runat="server" Text="Invoice No:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtBPCSInvoiceNo" runat="server" Width="85px" MaxLength="25" Enabled="false"></asp:TextBox>
                </td>
                <td class="p_textbold" style="white-space: nowrap;">
                    <asp:Label ID="lblQuantityShippedLabel" runat="server" Text="Quantity Shipped:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblQuantityShippedValue" runat="server" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="p_textbold" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblCalculatedDeductionAmountLabel" Text="Calculated Deduction/Recovery Amount:"
                        Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblCalculatedDeductionAmountValue" Visible="false"></asp:Label>
                </td>
                <td class="p_textbold" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblFinalDeductionAmount" Text="Final Deduction/Recovery Amount:"
                        Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtFinalDeductionAmount" Visible="false"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvFinalDeductionAmount" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtFinalDeductionAmount"
                        ErrorMessage="Final Deduction must be numeric" SetFocusOnError="True" />
                </td>
            </tr>
        </table>
        <asp:Label ID="lblMessageButtons" runat="server" SkinID="MessageLabelSkin" />
        <table width="98%">
            <tr>
                <td align="center" style="white-space: nowrap; width: 90%">
                    <asp:Button ID="btnAccrual" runat="server" Text="View Accrual Details" ValidationGroup="vgSave"
                        Visible="false" />
                    <asp:Button ID="btnCustomerApproved" runat="server" Text="Customer Approved" ToolTip="Customer Approved - Notify Accounting to Update Price in Future 3"
                        ValidationGroup="vgSave" Visible="false" />
                    <asp:Button ID="btnClose" runat="server" Text="Close" ValidationGroup="vgSave" Visible="false" />
                </td>
            </tr>
            <tr>
                <td align="center" style="white-space: nowrap; width: 90%">
                    <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="vgSave" Visible="false" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="False" Visible="false" />
                    <asp:Button ID="btnPreview" runat="server" Text="Preview" Visible="false" />
                    <asp:Button ID="btnNotifyPriceUpdatedByAccounting" runat="server" Text="Notify Accounting Manager of update"
                        Visible="false" />
                    <asp:Button ID="btnVoid" runat="server" Text="Void" Visible="false" CausesValidation="False"
                        ValidationGroup="vgVoid" />
                    <br />
                    <asp:Button ID="btnCopy" runat="server" Text="Copy" CausesValidation="False" Visible="false" />
                    <asp:Button ID="btnCreateAccountingAccrual" runat="server" Text="Create Accounting Accrual Event"
                        CausesValidation="False" Visible="false" />
                    <asp:Button ID="btnCreatePriceChangeNoAccrual" runat="server" Text="Create Price Change NO ACCRUAL Event"
                        CausesValidation="False" Visible="false" />
                    <asp:Button ID="btnNotifyAccounting" runat="server" Text="Notify of Update" Visible="false"
                        ToolTip="THIS IS NOT THE SUBMISSION FOR APPROVAL. It is intended to allow the manual notificatin to the accounting group that an update was made to the AR Event, outside of submission or the communication board." />
                </td>
            </tr>
        </table>
        <br />
        <hr />
        <br />
        <asp:Menu ID="menuTabs" Height="30px" runat="server" Orientation="Horizontal" StaticEnableDefaultPopOutImage="False"
            StaticMenuItemStyle-CssClass="tab" StaticSelectedStyle-CssClass="selectedTab"
            CssClass="tabs">
            <Items>
                <asp:MenuItem Text="Selected Details" Value="0" ImageUrl=""></asp:MenuItem>
                <asp:MenuItem Text="Supporting Documents" Value="1" ImageUrl=""></asp:MenuItem>
                <asp:MenuItem Text="Approval Status" Value="2" ImageUrl=""></asp:MenuItem>
                <asp:MenuItem Text="Communication Board" Value="3" ImageUrl=""></asp:MenuItem>
            </Items>
        </asp:Menu>
        <asp:MultiView ID="mvTabs" runat="server" Visible="true" ActiveViewIndex="0" EnableViewState="true">
            <asp:View ID="vDetail" runat="server">
                <table width="98%">
                    <tr align="center">
                        <td>
                            <asp:RadioButtonList runat="server" ID="rbSelectionWizard" RepeatDirection="Horizontal"
                                Visible="false" AutoPostBack="true">
                                <asp:ListItem Text="Current Parts" Value="C" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Future Parts" Value="F"></asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:Button ID="btnSelectionWizard" runat="server" Text="Selection Wizard" ValidationGroup="vgSave"
                                Visible="false" />
                        </td>
                    </tr>
                </table>
                <table width="98%" runat="server" id="tblPriceAdjustment">
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:RadioButtonList runat="server" ID="rbPriceAdjustment" RepeatDirection="Horizontal"
                                AutoPostBack="true">
                                <asp:ListItem Text="Adjust by Percentage" Value="P" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Adjust by Dollar Amount" Value="D"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" style="white-space: nowrap;">
                            <asp:Label runat="server" ID="lblPricePercent" Text="Percent adjustment from Current Price to New Price:"></asp:Label>
                        </td>
                        <td style="white-space: nowrap;">
                            <asp:TextBox ID="txtPricePercent" runat="server" MaxLength="10"></asp:TextBox>
                            <asp:Label runat="server" ID="lblPricePercentSign" Text="%"></asp:Label>
                            &nbsp;&nbsp;
                            <asp:Label ID="lblPricePercentDecimal" runat="server" Text=" = (0.00)"></asp:Label>
                            <asp:CompareValidator runat="server" ID="cvPricePercent" Operator="DataTypeCheck"
                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtPricePercent"
                                ErrorMessage="Price Percent must be numeric" SetFocusOnError="True" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" style="white-space: nowrap;">
                            <asp:Label ID="lblPriceDollar" runat="server" Text="Add/Subtract from Current Price to get New Price:"
                                Visible="false"></asp:Label>
                        </td>
                        <td style="white-space: nowrap;">
                            <asp:TextBox ID="txtPriceDollar" runat="server" MaxLength="10" Visible="false"></asp:TextBox>
                            <asp:CompareValidator runat="server" ID="cvPriceDollar" Operator="DataTypeCheck"
                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtPriceDollar"
                                ErrorMessage="Price must be numeric" SetFocusOnError="True" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Button runat="server" ID="btnPushAdjustments" Text="Update the New Price in all rows below"
                                ValidationGroup="vgSave" Visible="false" />
                        </td>
                    </tr>
                </table>
                <table width="98%">
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnExportToExcel" runat="server" Text="Export to Excel" CausesValidation="true"
                                Visible="false" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:ValidationSummary ID="vsDetailEdit" runat="server" DisplayMode="List" ShowMessageBox="true"
                    ShowSummary="true" EnableClientScript="true" ValidationGroup="vgDetailEdit" />
                <asp:ValidationSummary ID="vsDetailInsert" runat="server" DisplayMode="List" ShowMessageBox="true"
                    ShowSummary="true" EnableClientScript="true" ValidationGroup="vgDetailInsert" />
                <asp:GridView runat="server" ID="gvDetail" AllowPaging="True" AllowSorting="True"
                    AutoGenerateColumns="False" PageSize="15" DataSourceID="odsDetail" DataKeyNames="RowID"
                    Width="98%" EmptyDataText="No details have been selected yet." OnRowCommand="gvDetail_RowCommand"
                    ShowFooter="True">
                    <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Size="Medium" />
                    <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#CCCCCC" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
                    <Columns>
                        <asp:BoundField DataField="AREID" ReadOnly="True" HeaderText="AREID" HeaderStyle-CssClass="none"
                            ItemStyle-CssClass="none">
                            <HeaderStyle CssClass="none" />
                            <ItemStyle CssClass="none" />
                        </asp:BoundField>
                        <asp:BoundField DataField="RowID" ReadOnly="True" HeaderText="RowID" HeaderStyle-CssClass="none"
                            ItemStyle-CssClass="none">
                            <HeaderStyle CssClass="none" />
                            <ItemStyle CssClass="none" />
                        </asp:BoundField>
                        <asp:BoundField DataField="COMPNY" ReadOnly="True" HeaderText="COMPNY" HeaderStyle-CssClass="none"
                            ItemStyle-CssClass="none">
                            <HeaderStyle CssClass="none" />
                            <ItemStyle CssClass="none" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="UGN Facility" SortExpression="UGNFacilityName">
                            <EditItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("UGNFacilityName") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("UGNFacilityName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:DropDownList ID="ddUGNFacilityInsert" runat="server" DataSource='<%# commonFunctions.GetUGNFacility("")%>'
                                    DataValueField="UGNFacility" DataTextField="ddUGNFacilityName" SelectedValue='<%# Bind("UGNFacility") %>'
                                    AppendDataBoundItems="true" Width="156px" ValidationGroup="vgDetailInsert">
                                    <asp:ListItem Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvUGNFacilityInsert" runat="server" ControlToValidate="ddUGNFacilityInsert"
                                    ErrorMessage="UGN Facility is a required field." ValidationGroup="vgDetailInsert">                    
                    <
                                </asp:RequiredFieldValidator>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Internal Part No." SortExpression="PARTNO">
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkViewPFSalesProjection" runat="server" NavigateUrl='<%# Eval("PARTNO", "~/PF/Sales_Projection.aspx?sPartNo={0}") %>'
                                    Font-Underline="true" Target="_blank" Text='<%# Eval("PARTNO") %>'>
                                </asp:HyperLink>
                                <asp:Label ID="lblViewPartNo" runat="server" Text='<%# Bind("PARTNO") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" />
                            <FooterTemplate>
                                <asp:TextBox ID="txtPartNoInsert" runat="server" MaxLength="40" />
                                <asp:RequiredFieldValidator ID="rfvPartNoInsert" runat="server" ControlToValidate="txtPartNoInsert"
                                    ErrorMessage="Internal Part No is a required field." ValidationGroup="vgDetailInsert">                    
                    <
                                </asp:RequiredFieldValidator>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="PRCCDE" ReadOnly="True" HeaderText="PRCCDE" HeaderStyle-CssClass="none"
                            ItemStyle-CssClass="none">
                            <HeaderStyle CssClass="none" />
                            <ItemStyle CssClass="none" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Price Code" SortExpression="PriceCodeName">
                            <EditItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("PriceCodeName") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("PriceCodeName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:DropDownList ID="ddPriceCodeInsert" runat="server" DataSource='<%# commonFunctions.GetPriceCode("")%>'
                                    DataValueField="PriceCode" DataTextField="ddPriceCodeName" SelectedValue='<%# Bind("PriceCode") %>'>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvPriceCodeInsert" runat="server" ControlToValidate="ddPriceCodeInsert"
                                    ErrorMessage="Price Code is a required field." ValidationGroup="vgDetailInsert">                    
                    <
                                </asp:RequiredFieldValidator>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Customer" SortExpression="Customer">
                            <EditItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Eval("Customer") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("Customer") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:DropDownList ID="ddCustomerInsert" runat="server" DataSource='<%# commonFunctions.GetOEMManufacturer("")%>'
                                    DataValueField="OEMManufacturer" DataTextField="ddOEMManufacturer" SelectedValue='<%# Bind("OEMManufacturer") %>'
                                    AppendDataBoundItems="true" Width="156px" ValidationGroup="vgDetailInsert">
                                    <asp:ListItem Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Current Price" SortExpression="USE_RELPRC">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditCurrentPrice" runat="server" Text='<%# Bind("USE_RELPRC") %>'
                                    MaxLength="10" />
                                &nbsp;
                                <asp:Label ID="lblEditUSE_RELPRC" runat="server" Text='<%# Bind("USE_RELPRC") %>'
                                    CssClass="none" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewUSE_RELPRC" runat="server" Text='<%# Bind("USE_RELPRC") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:TextBox ID="txtCurrentPriceInsert" runat="server" MaxLength="10" />
                                <ajax:FilteredTextBoxExtender ID="ftbeCurrentPrice" runat="server" TargetControlID="txtCurrentPriceInsert"
                                    FilterType="Custom, Numbers" ValidChars="-." />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Price Change Percent" SortExpression="PRCPRNT">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditPricePercent" runat="server" Text='<%# Bind("PRCPRNT") %>'
                                    MaxLength="10"></asp:TextBox>
                                <asp:CompareValidator runat="server" ID="cvEditPricePercent" Operator="DataTypeCheck"
                                    ValidationGroup="vgDetailEdit" Type="double" Text="<" ControlToValidate="txtEditPricePercent"
                                    ErrorMessage="Percent adjustment must be numeric" SetFocusOnError="True" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewPRCPRNT" runat="server" Text='<%# Bind("PRCPRNT") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:TextBox ID="txtPricePercentInsert" runat="server" MaxLength="10" />
                                <ajax:FilteredTextBoxExtender ID="ftbePricePercent" runat="server" TargetControlID="txtPricePercentInsert"
                                    FilterType="Custom, Numbers" ValidChars="-." />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="New Price" SortExpression="PRCDOLR">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditPriceDollar" runat="server" Text='<%# Bind("PRCDOLR") %>'
                                    MaxLength="10"></asp:TextBox>
                                <asp:CompareValidator runat="server" ID="cvEditPriceDollar" Operator="DataTypeCheck"
                                    ValidationGroup="vgDetailEdit" Type="double" Text="<" ControlToValidate="txtEditPriceDollar"
                                    ErrorMessage="Price adjustment must be numeric" SetFocusOnError="True" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewPRCDOLR" runat="server" Text='<%# Bind("PRCDOLR") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:TextBox ID="txtPriceDollarInsert" runat="server" MaxLength="10" />
                                <ajax:FilteredTextBoxExtender ID="ftbePriceDollar" runat="server" TargetControlID="txtPriceDollarInsert"
                                    FilterType="Custom, Numbers" ValidChars="-." />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Estimated Price" SortExpression="ESTPRC">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditEstimatedPrice" runat="server" Text='<%# Bind("ESTPRC") %>'
                                    MaxLength="10"></asp:TextBox>
                                <asp:CompareValidator runat="server" ID="cvEditEstimatedPrice" Operator="DataTypeCheck"
                                    ValidationGroup="vgDetailEdit" Type="double" Text="<" ControlToValidate="txtEditEstimatedPrice"
                                    ErrorMessage="Estimated Price must be numeric" SetFocusOnError="True" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewESTPRC" runat="server" Text='<%# Bind("ESTPRC") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:TextBox ID="txtEstimatedPriceInsert" runat="server" MaxLength="10" />
                                <ajax:FilteredTextBoxExtender ID="ftbeEstimatedPrice" runat="server" TargetControlID="txtEstimatedPriceInsert"
                                    FilterType="Custom, Numbers" ValidChars="-." />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <EditItemTemplate>
                                <asp:ImageButton ID="iBtnDetailUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                    ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgDetailEdit" />&nbsp;
                                <asp:ImageButton ID="iBtnDetailCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                    ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="iBtnDetailEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                    ImageUrl="~/images/edit.jpg" AlternateText="Edit" />&nbsp;
                                <asp:ImageButton ID="ibtnDetailDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                    ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:ImageButton ID="ibtnInsert" runat="server" CausesValidation="True" CommandName="Insert"
                                    ImageUrl="~/images/save.jpg" AlternateText="Insert" ValidationGroup="vgDetailInsert" />&nbsp;
                                <asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo" ImageUrl="~/images/undo-gray.jpg"
                                    AlternateText="Undo" />
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsDetail" runat="server" SelectMethod="GetAREventDetail"
                    TypeName="AREventDetailBLL" OldValuesParameterFormatString="original_{0}" UpdateMethod="UpdateAREventDetail"
                    DeleteMethod="DeleteAREventDetail" InsertMethod="InsertAREventDetail">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="lblAREID" Name="AREID" PropertyName="Text" Type="Int32" />
                    </SelectParameters>
                    <UpdateParameters>
                        <asp:ControlParameter ControlID="lblAREID" Name="AREID" PropertyName="Text" Type="Int32" />
                        <asp:ControlParameter ControlID="ddEventType" Name="EventTypeID" PropertyName="SelectedValue"
                            Type="Int32" />
                        <asp:Parameter Name="USE_RELPRC" Type="Double" />
                        <asp:Parameter Name="PRCPRNT" Type="Double" />
                        <asp:Parameter Name="PRCDOLR" Type="Double" />
                        <asp:Parameter Name="ESTPRC" Type="Double" />
                        <asp:Parameter Name="original_RowID" Type="Int32" />
                        <asp:Parameter Name="PartNo" Type="String" />
                    </UpdateParameters>
                    <DeleteParameters>
                        <asp:ControlParameter ControlID="lblAREID" Name="AREID" PropertyName="Text" Type="Int32" />
                        <asp:Parameter Name="original_RowID" Type="Int32" />
                    </DeleteParameters>
                    <InsertParameters>
                        <asp:ControlParameter ControlID="lblAREID" Name="AREID" PropertyName="Text" Type="Int32" />
                        <asp:ControlParameter ControlID="ddEventType" Name="EventTypeID" PropertyName="SelectedValue"
                            Type="Int32" />
                        <asp:Parameter Name="COMPNY" Type="String" />
                        <asp:Parameter Name="Customer" Type="String" />
                        <asp:Parameter Name="PARTNO" Type="String" />
                        <asp:Parameter Name="PRCCDE" Type="String" />
                        <asp:Parameter Name="PRCPRNT" Type="Double" />
                        <asp:Parameter Name="PRCDOLR" Type="Double" />
                        <asp:Parameter Name="USE_RELPRC" Type="Double" />
                        <asp:Parameter Name="ESTPRC" Type="Double" />
                    </InsertParameters>
                </asp:ObjectDataSource>
                <br />
                <asp:Label runat="server" ID="lblAffectedInvoicesOnHoldLabel" Text="Affected Invoices On Hold While the Event was open"
                    Visible="false" Font-Bold="true"></asp:Label>
                <br />
                <asp:GridView runat="server" ID="gvAffectedInvoicesOnHold" AllowPaging="True" AllowSorting="True"
                    AutoGenerateColumns="False" PageSize="15" DataSourceID="odsAffectedInvoicesOnHold"
                    Width="98%" EmptyDataText="No Invoices have been assigned to this event yet."
                    Visible="false">
                    <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Size="Medium" />
                    <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#CCCCCC" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
                    <Columns>
                        <asp:BoundField DataField="INVNO" ReadOnly="True" HeaderText="Invoice No." SortExpression="INVNO">
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Customer" ReadOnly="True" HeaderText="Customer" SortExpression="Customer">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <%--<asp:BoundField DataField="SOLDTO" ReadOnly="True" HeaderText="SOLDTO" SortExpression="SOLDTO">
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
                        </asp:BoundField>--%>
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
                <asp:ObjectDataSource ID="odsAffectedInvoicesOnHold" runat="server" SelectMethod="GetAREventInvoicesOnHold"
                    TypeName="ARGroupModule">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="lblAREID" Name="AREID" PropertyName="Text" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:TextBox runat="server" ID="txtInvoicePartNo" MaxLength="15" Visible="false"></asp:TextBox>
                <asp:TextBox runat="server" ID="txtInvoicePriceCode" MaxLength="1" Visible="false"></asp:TextBox>
                <br />
                <asp:Label runat="server" ID="lblCurrentInvoicesOnHoldLabel" Text="Active Invoices On Hold"
                    Visible="false" Font-Bold="true"></asp:Label>
                <br />
                <asp:GridView runat="server" ID="gvInvoicesOnHold" AllowPaging="True" AllowSorting="True"
                    AutoGenerateColumns="False" PageSize="15" DataSourceID="odsInvoicesOnHold" Width="98%"
                    EmptyDataText="No invoices are currently on hold. Please close this event.">
                    <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Size="Medium" />
                    <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#CCCCCC" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
                    <Columns>
                        <asp:BoundField DataField="INVNO" ReadOnly="True" HeaderText="Invoice No." SortExpression="INVNO">
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Customer" ReadOnly="True" HeaderText="Customer" SortExpression="Customer">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <%--  <asp:BoundField DataField="SOLDTO" ReadOnly="True" HeaderText="SOLDTO" SortExpression="SOLDTO">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CABBV" ReadOnly="True" HeaderText="CABBV" SortExpression="CABBV">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DABBV" ReadOnly="True" HeaderText="DABBV" SortExpression="DABBV">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>--%>
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
            </asp:View>
            <asp:View ID="vSupportingDocuments" runat="server">
                <asp:ValidationSummary ID="vsSupportingDocs" runat="server" DisplayMode="List" ShowMessageBox="true"
                    ShowSummary="true" EnableClientScript="true" ValidationGroup="vgSupportingDocs" />
                <br />
                <table runat="server" id="tblUpload" visible="false">
                    <tr>
                        <td colspan="2">
                            <asp:Label runat="server" ID="lblUploadTitle" SkinID="StandardLabelSkin" Font-Bold="true">Upload a Supporting Document</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="p_textbold" valign="top">
                            File Description:
                        </td>
                        <td class="c_text">
                            <asp:TextBox ID="txtSupportingDocDesc" runat="server" MaxLength="200" Rows="3" TextMode="MultiLine"
                                Width="600px" />
                            <asp:RequiredFieldValidator ID="rfvSupportingDocDesc" runat="server" ControlToValidate="txtSupportingDocDesc"
                                ErrorMessage="Supporting Document File Description is a required field." Font-Bold="False"
                                ValidationGroup="vgSupportingDocs" SetFocusOnError="true" Text="<"></asp:RequiredFieldValidator><br />
                            <br />
                            <asp:Label ID="lblSupportingDocDescCharCount" SkinID="MessageLabelSkin" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="white-space: nowrap;">
                            <asp:Label runat="server" ID="lblFileUploadLabel" Text="Upload a supporting file under 3 MB:<br>(PDF,DOC,DOCX,XLS,XLSX,JPEG,TIF,MSG,PPT,PPTX)"
                                CssClass="p_textbold"></asp:Label>
                        </td>
                        <td style="white-space: nowrap;">
                            <asp:FileUpload ID="fileUploadSupportingDoc" runat="server" Width="600px" />
                            <asp:Button ID="btnUpload" runat="server" Text="Upload" CausesValidation="true" ValidationGroup="vgSupportingDocs" />
                            <asp:RequiredFieldValidator ID="rfvFileUploadSupportingDoc" runat="server" ControlToValidate="fileUploadSupportingDoc"
                                ErrorMessage="PDF File is required." Font-Bold="False" ValidationGroup="vgUpload"><</asp:RequiredFieldValidator><br />
                            <asp:RegularExpressionValidator ID="revUploadFile" runat="server" ErrorMessage="Please upload only *.PDF, *.DOC,*.DOCX, *.XLS, *.XLSX, *.JPEG, *.JPG, *.TIF, *.PPT, *.PPTX files are allowed."
                                ValidationExpression="(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.pdf|.xls|.xlsx|.doc|.docx|.jpeg|.jpg|.tif|.msg|.ppt|.pptx|.PDF|.XLS|.XLSX|.DOC|.DOCX|.JPEG|.JPG|.TIF|.MSG|.PPT|.PPTX)$"
                                ControlToValidate="fileUploadSupportingDoc" ValidationGroup="vgSupportingDocs"
                                Font-Bold="True" Font-Size="Small" />
                        </td>
                    </tr>
                </table>
                <asp:Label runat="server" ID="lblMaxNote" Text="(A maximum of three supporting documents are allowed.)"
                    Visible="false"></asp:Label>
                <asp:GridView runat="server" ID="gvSupportingDoc" AllowPaging="True" AllowSorting="True"
                    AutoGenerateColumns="False" PageSize="10" DataSourceID="odsSupportingDoc" DataKeyNames="RowID"
                    EmptyDataText="No documents have been uploaded." Width="98%">
                    <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Size="Medium" />
                    <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#CCCCCC" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
                    <Columns>
                        <asp:BoundField DataField="RowID">
                            <ItemStyle CssClass="none" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Supporting Document Name" SortExpression="SupportingDocName">
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkViewFormula" runat="server" NavigateUrl='<%# Eval("RowID", "AR_Supporting_Doc_Viewer.aspx?AREID=" & ViewState("AREID") & "&RowID={0}") %>'
                                    Target="_blank" Text='<%# Eval("SupportingDocName") %>'></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Description" DataField="SupportingDocDesc">
                            <ControlStyle Font-Size="X-Small" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Preview Document">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="hlnkPreview" ImageUrl='<%# DisplayImage(DataBinder.Eval(Container, "DataItem.SupportingDocEncodeType").tostring)  %>'
                                    NavigateUrl='<%# "AR_Supporting_Doc_Viewer.aspx?AREID=" & DataBinder.Eval (Container.DataItem,"AREID").tostring & "&RowID=" & DataBinder.Eval (Container.DataItem,"RowID").tostring %>'
                                    Target="_blank" ToolTip="Preview Document" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="ibtnSupportingDocDelete" runat="server" CausesValidation="False"
                                    CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsSupportingDoc" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetAREventSupportingDoc" TypeName="AREventSupportingDocBLL" DeleteMethod="DeleteAREventSupportingDoc">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="lblAREID" DefaultValue="0" Name="AREID" PropertyName="Text"
                            Type="Int32" />
                    </SelectParameters>
                    <DeleteParameters>
                        <asp:ControlParameter ControlID="lblAREID" DefaultValue="0" Name="AREID" PropertyName="Text"
                            Type="Int32" />
                        <asp:Parameter Name="original_RowID" Type="Int32" />
                    </DeleteParameters>
                </asp:ObjectDataSource>
            </asp:View>
            <asp:View ID="vApprovalStatus" runat="server">
                <asp:Label runat="server" ID="lblNoteApprovalStatus" Text="Below is the status of approval information."
                    SkinID="MessageLabelSkin"></asp:Label>
                <br />
                <br />
                <asp:HyperLink runat="server" ID="hlnkApprovalPage" Text="Click here to go to the actual approval page"
                    Visible="false" Font-Bold="true" Font-Underline="true" ForeColor="Blue"></asp:HyperLink>
                <br />
                <br />
                <asp:ValidationSummary ID="vsEditApproval" runat="server" DisplayMode="List" EnableClientScript="true"
                    ShowMessageBox="True" ValidationGroup="vgEditApproval" />
                <asp:GridView ID="gvApproval" runat="server" AutoGenerateColumns="False" DataKeyNames="RowID"
                    DataSourceID="odsApproval" HeaderStyle-CssClass="c_text" Width="98%">
                    <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" Wrap="False" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Size="Medium" />
                    <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#E2DED6" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <EmptyDataRowStyle Wrap="False" />
                    <Columns>
                        <asp:BoundField DataField="RowID">
                            <ItemStyle CssClass="none" />
                        </asp:BoundField>
                        <asp:TemplateField>
                            <EditItemTemplate>
                                <asp:Label ID="lblEditRoutingLevel" runat="server" Text='<%# Bind("RoutingLevel") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewRoutingLevel" runat="server" Text='<%# Bind("RoutingLevel") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="none" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <EditItemTemplate>
                                <asp:Label ID="lblEditSubscriptionID" runat="server" Text='<%# Bind("SubscriptionID") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewSubscriptionID" runat="server" Text='<%# Bind("SubscriptionID") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="none" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="ddTeamMemberName" HeaderText="Team Member" SortExpression="ddTeamMemberName"
                            ReadOnly="True">
                            <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Subscription" HeaderText="Role" SortExpression="Subscription"
                            ReadOnly="True">
                            <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                        </asp:BoundField>
                        <asp:BoundField DataField="NotificationDate" HeaderText="Notification Date" SortExpression="NotificationDate"
                            ReadOnly="True">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Status" SortExpression="StatusName">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddEditStatus" runat="server" DataSource='<%#ARGroupModule.GetARApprovalStatusList() %>'
                                    DataValueField="StatusID" DataTextField="ddStatusName" AppendDataBoundItems="True"
                                    SelectedValue='<%# Bind("StatusID") %>'>
                                    <asp:ListItem Text="" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewStatus" runat="server" Text='<%# Bind("StatusName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="SignedDate" HeaderText="Signed Date" SortExpression="SignedDate"
                            ReadOnly="True">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Comment" SortExpression="Comment">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditComment" runat="server" MaxLength="200" Rows="2" TextMode="MultiLine"
                                    Text='<%# Bind("Comment") %>' Width="300px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvComment" runat="server" ControlToValidate="txtEditComment"
                                    ErrorMessage="Comments is a required field when approving for another team member."
                                    Font-Bold="True" ValidationGroup="vgEditApproval"><</asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewComment" runat="server" Text='<%# Bind("Comment") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="400px" />
                            <ItemStyle Wrap="True" />
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <EditItemTemplate>
                                <asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                    ImageUrl="~/images/save.jpg" ToolTip="Save" ValidationGroup="EditPriceInfo" />&nbsp;&nbsp;&nbsp;
                                <asp:ImageButton ID="ibtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                    ImageUrl="~/images/cancel.jpg" ToolTip="Cancel" ValidationGroup="EditPriceInfo" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                    ToolTip="Edit" ImageUrl="~/images/edit.jpg" ValidationGroup="EditPriceInfo" />&nbsp;&nbsp;&nbsp;
                            </ItemTemplate>
                            <ItemStyle Width="60px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsApproval" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetAREventApprovalStatus" TypeName="ARApprovalBLL" UpdateMethod="UpdateAREventApprovalStatus">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="lblAREID" Name="AREID" PropertyName="Text" Type="Int32" />
                        <asp:Parameter DefaultValue="0" Name="SubscriptionID" Type="Int32" />
                    </SelectParameters>
                    <UpdateParameters>
                        <asp:ControlParameter ControlID="lblAREID" Name="AREID" PropertyName="Text" Type="Int32" />
                        <asp:Parameter Name="RoutingLevel" Type="Int32" />
                        <asp:ControlParameter ControlID="lblTeamMemberID" Name="TeamMemberID" PropertyName="Text"
                            Type="Int32" />
                        <asp:Parameter Name="SubscriptionID" Type="Int32" />
                        <asp:Parameter Name="Comment" Type="String" />
                        <asp:Parameter Name="StatusID" Type="Int32" />
                        <asp:Parameter Name="original_RowID" Type="Int32" />
                        <asp:Parameter Name="RowID" Type="Int32" />
                    </UpdateParameters>
                </asp:ObjectDataSource>
                <br />
                <table>
                    <tr align="center">
                        <td>
                            <asp:Button ID="btnSubmitApproval" runat="server" Text="Submit for Approval" />
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vCommunicationBoard" runat="server">
                <asp:Label runat="server" ID="lblMessageCommunicationBoard" SkinID="MessageLabelSkin"></asp:Label>
                <asp:ValidationSummary ID="vsCommunicationBoard" runat="server" ValidationGroup="vgCommunicationBoard"
                    ShowMessageBox="true" ShowSummary="true" />
                <table runat="server" id="tblCommunicationBoardNewQuestion" visible="false">
                    <tr>
                        <td class="p_text" valign="top">
                            <asp:Label ID="lblRSSComment" runat="server" Font-Bold="True" ForeColor="Red" Text="*" />
                            New Question:
                        </td>
                        <td>
                            <asp:TextBox ID="txtRSSComment" runat="server" Width="550px" TextMode="MultiLine"
                                Rows="3" />
                            <asp:RequiredFieldValidator ID="rfvRSSComment" runat="server" ControlToValidate="txtRSSComment"
                                ErrorMessage="Question / Comment is a required field." ValidationGroup="vgCommunicationBoard"><</asp:RequiredFieldValidator>
                            <br />
                            <asp:Label ID="lblRSSCommentCharCount" runat="server" SkinID="MessageLabelSkin"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnRSSSubmit" runat="server" Text="Submit" CausesValidation="true"
                                ValidationGroup="vgCommunicationBoard" />
                            <asp:Button ID="btnRSSReset" runat="server" Text="Reset" CausesValidation="False" />
                        </td>
                    </tr>
                </table>
                <asp:Label ID="lblSQC" runat="server" CssClass="p_smalltextbold" Style="width: 532px;
                    color: #990000" Text="Select a Question / Comment from discussion thread below to respond." />
                <table runat="server" id="tblCommunicationBoardExistingQuestion" visible="false">
                    <tr>
                        <td class="p_text" valign="top">
                            Question to Answer:
                        </td>
                        <td class="c_text">
                            <asp:TextBox ID="txtQuestionComment" runat="server" Font-Bold="True" Rows="3" TextMode="MultiLine"
                                Width="550px" Enabled="False" />
                            <asp:RequiredFieldValidator ID="rfvQuestionComment" runat="server" ErrorMessage="Select a Question / Comment from table below for response."
                                ValidationGroup="vgReplyComment" ControlToValidate="txtQuestionComment"><</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" valign="top">
                            <asp:Label ID="lblReqReply" runat="server" Text="*" ForeColor="Red" />
                            Reply / Comment:
                        </td>
                        <td>
                            <asp:TextBox ID="txtReply" runat="server" Rows="3" TextMode="MultiLine" Width="550px" />
                            <asp:RequiredFieldValidator ID="rfvReply" runat="server" ErrorMessage="Reply / Comment is a required field."
                                SetFocusOnError="true" ValidationGroup="vgReplyComment" ControlToValidate="txtReply"><</asp:RequiredFieldValidator><br />
                            <br />
                            <asp:Label runat="server" ID="lblReplyCharCount" SkinID="MessageLabelSkin"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Button ID="btnSaveReplyComment" runat="server" Text="Submit" CausesValidation="true"
                                ValidationGroup="vgReplyComment" Visible="false" />
                            <asp:Button ID="btnResetReplyComment" runat="server" Text="Reset" CausesValidation="False"
                                Visible="false" />
                        </td>
                    </tr>
                </table>
                <asp:ValidationSummary ID="vsReplyComment" runat="server" ValidationGroup="vgReplyComment"
                    ShowMessageBox="true" ShowSummary="true" />
                <br />
                <asp:GridView ID="gvQuestion" runat="server" AutoGenerateColumns="False" DataSourceID="odsQuestion"
                    Width="900px" RowStyle-BorderStyle="None" EmptyDataText="No Questions have been submitted.">
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" Wrap="False" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Height="20px" />
                    <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <Columns>
                        <asp:TemplateField HeaderText="Reply">
                            <ItemTemplate>
                                <asp:ImageButton ID="ibtnAppendReply" runat="server" CausesValidation="False" OnClick="gvQuestionAppendReply_Click"
                                    ToolTip="Reply" ImageUrl="~/images/messanger30.jpg" AlternateText='<%# Bind("Comment") %>'
                                    CommandName='<%# Bind("RSSID") %>' />&nbsp;&nbsp;&nbsp;
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="RSSID" HeaderText="RSSID" SortExpression="RSSID" Visible="false" />
                        <asp:BoundField DataField="Comment" HeaderText="Question / Comment" SortExpression="Comment"
                            HeaderStyle-Width="500px" ItemStyle-Width="500px" ItemStyle-Font-Bold="true"
                            ItemStyle-CssClass="c_text" />
                        <asp:BoundField DataField="ddTeamMemberName" HeaderText="Submitted By" SortExpression="ddTeamMemberName"
                            HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-Font-Bold="true" />
                        <asp:BoundField DataField="PostDate" HeaderText="Post Date" SortExpression="PostDate"
                            ItemStyle-Font-Bold="true" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                    </td>
                                    <td colspan="3">
                                        <asp:GridView ID="gvReply" runat="server" AutoGenerateColumns="False" DataSourceID="odsReply"
                                            DataKeyNames="AREID,RSSID" Width="100%">
                                            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" Wrap="False" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Height="20px" />
                                            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            <HeaderStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="red" HorizontalAlign="Left" />
                                            <EditRowStyle BackColor="#E2DED6" />
                                            <EmptyDataRowStyle Wrap="False" />
                                            <Columns>
                                                <asp:BoundField DataField="Comment" HeaderText="Response" SortExpression="Comment"
                                                    HeaderStyle-Width="500px" ItemStyle-Width="500px" ItemStyle-Font-Bold="true" />
                                                <asp:BoundField DataField="ddTeamMemberName" HeaderText="" SortExpression="ddTeamMemberName"
                                                    HeaderStyle-Width="100px" ItemStyle-Width="100px" />
                                                <asp:BoundField DataField="PostDate" HeaderText="" SortExpression="PostDate" />
                                            </Columns>
                                        </asp:GridView>
                                        <asp:ObjectDataSource ID="odsReply" runat="server" OldValuesParameterFormatString="original_{0}"
                                            SelectMethod="GetARRSSReply" TypeName="ARRSSReplyBLL">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="lblAREID" Name="AREID" PropertyName="Text" Type="Int32" />
                                                <asp:Parameter Name="RSSID" Type="Int32" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsQuestion" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetARRSS" TypeName="ARRSSBLL">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="lblAREID" Name="AREID" PropertyName="Text" Type="Int32" />
                        <asp:Parameter Name="RSSID" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </asp:View>
        </asp:MultiView>
        <br />
        <asp:Label ID="lblMessageBottom" runat="server" SkinID="MessageLabelSkin" />
    </asp:Panel>
</asp:Content>
