<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="SupplierRequest.aspx.vb" Inherits="SUP_SupplierRequest" Title="Untitled Page"
    MaintainScrollPositionOnPostback="True" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="1000px">
        <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
            Visible="False" Font-Size="Large"></asp:Label>
        <% If ViewState("pSUPNo") <> Nothing Then%>
        <table>
            <tr>
                <td class="p_smalltextbold" style="color: #990000">
                    Review existing data or press
                    <asp:Button ID="btnAdd" runat="server" Text="Add" />
                    to enter new data.
                </td>
            </tr>
        </table>
        <% End If%>
        <hr />
        <table>
            <tr>
                <td class="p_text">
                    Reference #:
                </td>
                <td style="color: #990000;">
                    <asp:Label ID="lblSUPNO" runat="server" Text="0" CssClass="c_text" Font-Bold="True"
                        Font-Overline="False" Font-Size="Larger" Font-Underline="False" />
                </td>
                <td class="p_text" >
                    Date Submitted:
                </td>
                <td style="color: #990000;">
                    <asp:Label ID="lblDateSubmitted" runat="server" Text="" CssClass="c_text" Font-Bold="True" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="Label3" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                    &nbsp;Supplier Name:
                </td>
                <td>
                    <asp:TextBox ID="txtVendorName" runat="server" MaxLength="240" Width="300px" AutoPostBack="true" />&nbsp;<asp:RequiredFieldValidator
                        ID="rfvVendorName" runat="server" ControlToValidate="txtVendorName" ErrorMessage="Supplier Name is a required field."
                        Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                </td>
                <td class="p_text">
                    <asp:Label ID="lblInBPCS" runat="server" Font-Bold="True" ForeColor="Red" Text="*"
                        Visible="false" />
                    &nbsp;Supplier Created in Oracle:
                </td>
                <td>
                    <asp:DropDownList ID="ddInBPCS" runat="server">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Value="False">No</asp:ListItem>
                        <asp:ListItem Value="True">Yes</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;<asp:RequiredFieldValidator ID="rfvInBPCS" runat="server" ControlToValidate="ddInBPCS"
                        ErrorMessage="Supplier Created in Oracle is a required field." Font-Bold="False"
                        ValidationGroup="vsDetail" Enabled="false"><</asp:RequiredFieldValidator><asp:CheckBox
                            ID="cbTen99" runat="server" Text="1099?" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                    &nbsp;Product Description:
                </td>
                <td>
                    <asp:TextBox ID="txtProdDesc" runat="server" MaxLength="50" Width="300px" />&nbsp;<asp:RequiredFieldValidator
                        ID="rfvProdDesc" runat="server" ControlToValidate="txtProdDesc" ErrorMessage="Product Description is a required field."
                        Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                </td>
                <td class="p_text">
                    <asp:Label ID="lblVendorNo" runat="server" Font-Bold="True" ForeColor="Red" Text="*"
                        Visible="false" />
                    &nbsp;Supplier No. Assigned:
                </td>
                <td>
                    <asp:TextBox ID="txtVendorNo" runat="server" MaxLength="10" Width="100px" /><ajax:FilteredTextBoxExtender
                        ID="ftbeVendorNo" runat="server" TargetControlID="txtVendorNo" FilterType="Numbers" />
                    &nbsp;<asp:RequiredFieldValidator ID="rfvVendorNo" runat="server" ControlToValidate="txtVendorNo"
                        ErrorMessage="Supplier No Assigned is a required field." Font-Bold="False" ValidationGroup="vsDetail"
                        Enabled="false"><</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:CheckBox ID="cbNew" runat="server" Text="New Vendor" />
                </td>
                <td class="c_text">
                    <asp:CheckBox ID="cbChange" runat="server" Text="Change to current vendor" />&nbsp;
                    <asp:Label ID="lblReqNewOrChange" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="Label" Visible="False"></asp:Label>
                </td>
                <td class="p_text">
                    <asp:Label ID="Label23" runat="server" Font-Bold="True" ForeColor="Red" Text="* " /><asp:Label
                        ID="lblFutureVendor" runat="server" Text="Is this a future vendor for Quoting a Cost Sheet?" />
                </td>
                <td>
                    <asp:DropDownList ID="ddFutureVendor" runat="server">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Value="False">No</asp:ListItem>
                        <asp:ListItem Value="True">Yes</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;<asp:RequiredFieldValidator ID="rfvFutureVendor" runat="server" ControlToValidate="ddFutureVendor"
                        ErrorMessage="Is this a future vendor for Quoting a Cost Sheet? question requires an answer."
                        Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Status:
                </td>
                <td style="color: #990000;">
                    <asp:DropDownList ID="ddStatus" runat="server" AutoPostBack="True">
                        <asp:ListItem>New Entry</asp:ListItem>
                        <asp:ListItem>In Process</asp:ListItem>
                        <asp:ListItem>Void</asp:ListItem>
                        <asp:ListItem>Approved</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;<asp:Label ID="lblRoutingStatusDesc" runat="server" Text="Pending Submission"
                        CssClass="c_text" Font-Bold="True" Font-Overline="False" Font-Size="Larger" Font-Underline="False" />
                    <asp:TextBox ID="txtRoutingStatus" Visible="false" runat="server" Width="1px" />
                </td>
                <td colspan="2">
                </td>
            </tr>
            <tr>
                <td class="p_text" style="vertical-align: top">
                    <asp:Label ID="lblReqVoidReason" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="* " Visible="false" />
                    &nbsp;<asp:Label ID="lblVoidReason" runat="server" Text="Void Reason:" Visible="false"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtVoidReason" runat="server" MaxLength="400" Rows="2" TextMode="MultiLine"
                        Visible="False" Width="600px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvVoidReason" runat="server" ControlToValidate="txtVoidReason"
                        Enabled="false" ErrorMessage="Void Reason is a required field." Font-Bold="False"
                        ValidationGroup="vsDetail"><</asp:RequiredFieldValidator><br />
                    <asp:Label ID="lblVoidRsn" runat="server" Font-Bold="True" ForeColor="Red" />
                </td>
            </tr>
        </table>
        <br />
        <br />
        <table width="100%" border="0">
            <tr>
                <td colspan="3" style="width: 30px">
                    <asp:Menu ID="mnuTabs" Height="30px" runat="server" Orientation="Horizontal" StaticEnableDefaultPopOutImage="false"
                        StaticMenuItemStyle-CssClass="tab" StaticSelectedStyle-CssClass="selectedTab"
                        CssClass="tabs">
                        <Items>
                            <asp:MenuItem Text="Supplier Detail" Value="0" ImageUrl="" Selected="true" />
                            <asp:MenuItem Text="Supplier Contact Info" Value="1" ImageUrl="" Selected="true" />
                            <asp:MenuItem Text="Required Forms / Documents" Value="2" ImageUrl="" />
                            <asp:MenuItem Text="Approval Status" Value="3" ImageUrl="" />
                        </Items>
                    </asp:Menu>
                </td>
            </tr>
        </table>
        <asp:MultiView ID="mvTabs" runat="server" Visible="true" ActiveViewIndex="0" EnableViewState="true">
            <asp:View ID="vwDetail" runat="server">
                <table width="1000px">
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label9" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Requestor:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddRequestedBy" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvRequestedBy" runat="server" ControlToValidate="ddRequestedBy"
                                ErrorMessage="Requested By is a required field." ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" valign="top">
                            <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            UGN Location:
                        </td>
                        <td class="c_text">
                            <asp:CheckBox ID="cbUT" runat="server" Text="Tinley Park, IL" />&nbsp;
                            <asp:CheckBox ID="cbUN" runat="server" Text="Chicago Heights, IL" />&nbsp;
                            <asp:CheckBox ID="cbUP" runat="server" Text="Jackson, TN" />&nbsp;
                            <asp:CheckBox ID="cbUR" runat="server" Text="Somerset, KY" />&nbsp;
                            <asp:CheckBox ID="cbUS" runat="server" Text="Valparaiso, IN" />&nbsp;
                            <asp:CheckBox ID="cbOH" runat="server" Text="Monroe, OH" />&nbsp;
                            <asp:CheckBox ID="cbUW" runat="server" Text="Silao, MX" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label10" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Type of Vendor:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddVendorType" runat="server" AutoPostBack="True" />
                            &nbsp;<asp:RequiredFieldValidator ID="rfvVendorType" runat="server" ControlToValidate="ddVendorType"
                                ErrorMessage="Vendor Type is a required field." Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator><asp:DropDownList
                                    ID="ddINVU" runat="server" Visible="False" AppendDataBoundItems="True">
                                    <asp:ListItem Value="" Text="" />
                                    <asp:ListItem>Packaging</asp:ListItem>
                                    <asp:ListItem>Production Material</asp:ListItem>
                                </asp:DropDownList>
                            <asp:DropDownList ID="ddMROU" runat="server" Visible="False" AppendDataBoundItems="True">
                                <asp:ListItem Value="" Text="" />
                                <asp:ListItem>Capital and Tooling</asp:ListItem>
                                <asp:ListItem>General Expenses</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblComPri" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Family (Item Class):
                        </td>
                        <td>
                            <asp:DropDownList ID="ddFamily" runat="server" AutoPostBack="True">
                                <asp:ListItem Text="" Value="0" />
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvFamily" runat="server" ControlToValidate="ddFamily"
                                ErrorMessage="Commodity Primary is a required field." Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator><asp:Label
                                    ID="lblFamily" runat="server" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Sub-Family (SUBFM):
                        </td>
                        <td>
                            <asp:DropDownList ID="ddSubFamily" runat="server">
                                <asp:ListItem Text="" Value="0" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblReqContractorOnSite" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="* " />
                            Contractor On Site:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddContractorOnSite" runat="server" AutoPostBack="True">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem Value="False">No</asp:ListItem>
                                <asp:ListItem Value="True">Yes</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvContractorOnSite" runat="server" ControlToValidate="ddContractorOnSite"
                                ErrorMessage="Contractor on Site is a required field." Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label20" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Terms:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddTerms" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvTerms" runat="server" ControlToValidate="ddTerms"
                                ErrorMessage="Terms is a required field." Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label21" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Payment Type:
                        </td>
                        <td style="font-size: x-small">
                            <asp:DropDownList ID="ddPayType" runat="server">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem Value="Check">Check</asp:ListItem>
                                <asp:ListItem Value="Wire Transfer">Wire Transfer</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvPayType" runat="server" ControlToValidate="ddPayType"
                                ErrorMessage="Payment Type is a required field." Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                            Wire Transfer - Only for Suppliers outside US and/or payments are in Non-US funds.
                            Otherwise Acctg Mgr authorization required.
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Amount of Initial Purchase ($):
                        </td>
                        <td>
                            <asp:TextBox ID="txtInitialPurchaseAmt" runat="server" MaxLength="20" Width="100px" /><ajax:FilteredTextBoxExtender
                                ID="ftInitialPurchaseAmt" runat="server" TargetControlID="txtInitialPurchaseAmt"
                                FilterType="Custom, Numbers" ValidChars="-,." />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Est. Amount of Annual Purchase ($):
                        </td>
                        <td>
                            <asp:TextBox ID="txtEstAmtAnnualPurchase" runat="server" MaxLength="20" Width="100px" /><ajax:FilteredTextBoxExtender
                                ID="ftEstAmtAnnualPurchase" runat="server" TargetControlID="txtEstAmtAnnualPurchase"
                                FilterType="Custom, Numbers" ValidChars="-,." />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" nowrap="nowrap">
                            Does This Supplier replaces a current supplier?
                        </td>
                        <td>
                            <asp:DropDownList ID="ddReplacesCurrentVendor" runat="server" AutoPostBack="True">
                                <asp:ListItem Value="False">No</asp:ListItem>
                                <asp:ListItem Value="True">Yes</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblReqVendor" runat="server" Font-Bold="True" ForeColor="Red" Text="*"
                                Visible="false" />
                            &nbsp;If yes, Supplier Name:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddVendor" runat="server">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvReqVendor" ControlToValidate="ddVendor" runat="server"
                                ErrorMessage="Supplier Name is a required field." ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" valign="top">
                            <asp:Label ID="lblReqRsnSupAdd" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Reason for New Supplier Addition:
                        </td>
                        <td>
                            <asp:TextBox ID="txtReasonForAddition" runat="server" MaxLength="300" Rows="3" TextMode="MultiLine"
                                Width="600px" />
                            <asp:RequiredFieldValidator ID="rfvReqRsnSupAdd" ControlToValidate="txtReasonForAddition"
                                runat="server" ErrorMessage="Reason for New Supplier Addition is a required field."
                                ValidationGroup="vsDetail"><</asp:RequiredFieldValidator><br />
                            <asp:Label ID="lblReasonForAddition" runat="server" Font-Bold="True" ForeColor="Red" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            &nbsp;
                        </td>
                        <td class="c_text">
                            <asp:Button ID="btnSave1" runat="server" CausesValidation="True" Text="Save" ValidationGroup="vsDetail" />
                            <asp:Button ID="btnReset1" runat="server" CausesValidation="False" Text="Reset" />
                            <asp:Button ID="btnDelete" runat="server" CausesValidation="False" Text="Delete" />
                            <asp:Button ID="btnPreview" runat="server" Text="Preview" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:ValidationSummary ID="sDetail" ValidationGroup="vsDetail" runat="server" ShowMessageBox="True" />
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vsContacts" runat="server">
                <table width="1000px">
                    <tr>
                        <td>
                            <table width="700px">
                                <tr>
                                    <td class="p_textbold">
                                        <asp:Label ID="Label4" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                        &nbsp;Sales Contact:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSalesContactName" runat="server" Width="250px" />
                                        <asp:RequiredFieldValidator ID="rfvSalesContact" runat="server" ControlToValidate="txtSalesContactName"
                                            ErrorMessage="Sales Contact is a required field." ValidationGroup="vsContact"><</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label ID="Label19" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                        Phone:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPhone" runat="server" MaxLength="25" Width="250px" />
                                        <asp:RequiredFieldValidator ID="rfvSalesContactPhone" runat="server" ControlToValidate="txtPhone"
                                            ErrorMessage="Sales Contact Phone is a required field." ValidationGroup="vsContact"><</asp:RequiredFieldValidator>
                                        <ajax:FilteredTextBoxExtender ID="ftPhone" runat="server" TargetControlID="txtPhone"
                                            FilterType="Custom, Numbers" ValidChars="()- ext." />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label ID="Label6" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                        Fax:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSalesFax" runat="server" MaxLength="25" Width="250px" />
                                        <asp:RequiredFieldValidator ID="rfvSalesFax" runat="server" ControlToValidate="txtSalesFax"
                                            ErrorMessage="Sales Contact Fax is a required field." ValidationGroup="vsContact"><</asp:RequiredFieldValidator>
                                        <ajax:FilteredTextBoxExtender ID="ftSalesFax" runat="server" TargetControlID="txtSalesFax"
                                            FilterType="Custom, Numbers" ValidChars="()- " />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_textbold">
                                        <asp:Label ID="Label5" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                        Accounting Contact:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAcctContact" runat="server" MaxLength="30" Width="250px" />
                                        <asp:RequiredFieldValidator ID="rfvAccountingContact" runat="server" ControlToValidate="txtAcctContact"
                                            ErrorMessage="Accounting Contact is a required field." ValidationGroup="vsContact"><</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label ID="Label18" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                        Phone:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAcctPhone" runat="server" MaxLength="25" Width="250px" />
                                        <asp:RequiredFieldValidator ID="rfvAccountingContactPhone" runat="server" ControlToValidate="txtAcctContact"
                                            ErrorMessage="Accounting Contact Phone is a required field." ValidationGroup="vsContact"><</asp:RequiredFieldValidator>
                                        <ajax:FilteredTextBoxExtender ID="ftAcctPhone" runat="server" TargetControlID="txtAcctPhone"
                                            FilterType="Custom, Numbers" ValidChars="()- ext." />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        Fax:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAcctFax" runat="server" MaxLength="25" Width="250px" />
                                        <ajax:FilteredTextBoxExtender ID="ftAcctFax" runat="server" TargetControlID="txtAcctFax"
                                            FilterType="Custom, Numbers" ValidChars="()- " />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_textbold">
                                        Customer Service Contact:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCustServContact" runat="server" MaxLength="30" Width="250px" />&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        Phone:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCustServPhone" runat="server" MaxLength="25" Width="250px" />&nbsp;
                                        <ajax:FilteredTextBoxExtender ID="ftCustServPhone" runat="server" TargetControlID="txtCustServPhone"
                                            FilterType="Custom, Numbers" ValidChars="()- ext." />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        Fax:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCustServFax" runat="server" MaxLength="25" Width="250px" />
                                        <ajax:FilteredTextBoxExtender ID="ftCustServFax" runat="server" TargetControlID="txtCustServFax"
                                            FilterType="Custom, Numbers" ValidChars="()- " />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        Email:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCustServEmail" runat="server" MaxLength="50" Width="300px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text" colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_textbold">
                                        <asp:Label ID="Label7" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                        Remit to Address:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRemitToAddr1" runat="server" Width="300px" MaxLength="50" />
                                        <asp:RequiredFieldValidator ID="rfvRemitAddr" runat="server" ControlToValidate="txtRemitToAddr1"
                                            ErrorMessage="Remit to Address is a required field." ValidationGroup="vsContact"><</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRemitToAddr2" runat="server" Width="300px" MaxLength="50" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRemitToAddr3" runat="server" Width="300px" MaxLength="50" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRemitToAddr4" runat="server" Width="300px" MaxLength="50" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label ID="Label16" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                        City:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRemitCity" runat="server" MaxLength="30" Width="250px" />
                                        <asp:RequiredFieldValidator ID="rfvRemitCity" runat="server" ControlToValidate="txtRemitCity"
                                            ErrorMessage="Remit City is a required field." ValidationGroup="vsContact"><</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label ID="Label15" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                        State:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRemitState" runat="server" MaxLength="30" Width="150px" />
                                        <asp:RequiredFieldValidator ID="rfvRemitState" runat="server" ControlToValidate="txtRemitState"
                                            ErrorMessage="Remit State is a required field." ValidationGroup="vsContact"><</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label ID="Label14" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                        Zip:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRemitZip" runat="server" MaxLength="10" />
                                        <asp:RequiredFieldValidator ID="rfvRemitZip" runat="server" ControlToValidate="txtRemitZip"
                                            ErrorMessage="Remit Zip is a required field." ValidationGroup="vsContact"><</asp:RequiredFieldValidator>
                                        &nbsp;<ajax:FilteredTextBoxExtender ID="ftRemitZip" runat="server" TargetControlID="txtRemitZip"
                                            FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,- " />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label ID="Label22" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                        Country:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddRemitToCountry" runat="server" />
                                        <asp:RequiredFieldValidator ID="rfvRemitToCountry" runat="server" ControlToValidate="ddRemitToCountry"
                                            ErrorMessage="Remit Country is a required field." ValidationGroup="vsContact"><</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text" colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_textbold">
                                        <asp:Label ID="Label8" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                        &nbsp;PO Address/ Ship from:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtShipFromAddr1" runat="server" Width="300px" MaxLength="50" />
                                        <asp:RequiredFieldValidator ID="rfvShipAddr" runat="server" ControlToValidate="txtShipFromAddr1"
                                            ErrorMessage="Ship from Address is a required field." ValidationGroup="vsContact"><</asp:RequiredFieldValidator>&nbsp;
                                        <asp:CheckBox ID="cbSameAsRemitToAddr" runat="server" AutoPostBack="True" Font-Italic="True"
                                            Text="Same as Remit To Address" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtShipFromAddr2" runat="server" Width="300px" MaxLength="50" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtShipFromAddr3" runat="server" Width="300px" MaxLength="50" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtShipFromAddr4" runat="server" Width="300px" MaxLength="50" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label ID="Label11" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                        City:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtShipFromCity" runat="server" MaxLength="30" Width="250px" />
                                        <asp:RequiredFieldValidator ID="rfvShipCity" runat="server" ControlToValidate="txtShipFromCity"
                                            ErrorMessage="Ship from City is a required field." ValidationGroup="vsContact"><</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label ID="Label12" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                        State:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtShipFromState" runat="server" MaxLength="30" Width="150px" />
                                        <asp:RequiredFieldValidator ID="rfvShipState" runat="server" ControlToValidate="txtShipFromState"
                                            ErrorMessage="Ship from State is a required field." ValidationGroup="vsContact"><</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label ID="Label13" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                        Zip:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtShipFromZip" runat="server" MaxLength="10" />
                                        <asp:RequiredFieldValidator ID="rfvShipZip" runat="server" ControlToValidate="txtShipFromZip"
                                            ErrorMessage="Ship from Zip is a required field." ValidationGroup="vsContact"><</asp:RequiredFieldValidator>
                                        &nbsp;<ajax:FilteredTextBoxExtender ID="ftShipFromZip" runat="server" TargetControlID="txtShipFromZip"
                                            FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,%. " />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label ID="Label17" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                        Country:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddShipFromCountry" runat="server" />
                                        <asp:RequiredFieldValidator ID="rfvShipFromCountry" runat="server" ControlToValidate="ddShipFromCountry"
                                            ErrorMessage="Ship From Country is a required field." ValidationGroup="vsContact"><</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td class="c_text">
                                        <asp:Button ID="btnSave2" runat="server" CausesValidation="True" ValidationGroup="vsContact"
                                            Text="Save" />
                                        <asp:Button ID="btnReset2" runat="server" CausesValidation="False" Text="Reset" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <asp:ValidationSummary ID="vsSupplierContactInfo" ValidationGroup="vsContact" runat="server"
                                ShowMessageBox="True" />
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vsSupportingDocuments" runat="server">
                <asp:Panel ID="SDPanel" runat="server" CssClass="collapsePanelHeader" Width="680px">
                    <asp:Image ID="imgSD" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                        Height="12px" />&nbsp;
                    <asp:Label ID="lblSD" runat="server" Text="Label" CssClass="c_textbold">UPLOAD DOCUMENT(S):</asp:Label>
                </asp:Panel>
                <asp:Panel ID="SDContentPanel" runat="server" CssClass="collapsePanel" Width="800px">
                    <br />
                    <asp:Label ID="lblSupDoc" runat="server" CssClass="p_smalltextbold" Style="width: 800px;
                        color: #990000" Text="This section is available as an option to include additional information. *.PDF, *.DOC, *.DOCX, *.XLS and *.XLSX files are allowed for upload up to 4MB each." />
                    <br />
                    <asp:Label ID="lblSupDoc2" runat="server" CssClass="p_smalltextbold" Style="width: 800px;
                        color: #990000" Text="NOTE: Please be sure to upload the latest copy of any document. Any changes you make will not be saved to the upload files. Please be sure to make a copy of the file locally and upload a new version. You have the option to delete or keep previous version of the file for reference." /><br />
                    <br />
                    <table>
                        <tr>
                            <td class="p_text">
                                Upload By:
                            </td>
                            <td class="c_text">
                                <asp:DropDownList ID="ddTeamMember" runat="server" />
                                <asp:RequiredFieldValidator ID="rfvTeamMember" runat="server" ControlToValidate="ddTeamMember"
                                    ErrorMessage="Team Member is a required field." Font-Bold="False" ValidationGroup="vsSupportingDocuments"><</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text" valign="top">
                                Form Name:
                            </td>
                            <td class="c_text">
                                <asp:TextBox ID="txtFormName" runat="server" MaxLength="200" Width="300px" />
                                <asp:TextBox ID="txtSRFID" runat="server" Visible="false" Width="48px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text" valign="top">
                                Supporting Document:
                            </td>
                            <td class="c_text">
                                <asp:FileUpload ID="uploadFile" runat="server" Height="22px" Width="600px" />
                                <asp:RequiredFieldValidator ID="rfvUpload" runat="server" ControlToValidate="uploadFile"
                                    ErrorMessage="Supporting Document is required." Font-Bold="False" ValidationGroup="vsSupportingDocuments"><</asp:RequiredFieldValidator><br />
                                <asp:RegularExpressionValidator ID="revUploadFile" runat="server" ErrorMessage="Only *.PDF, *.DOC, *.DOCX, *.XLS, *.XLSX files are allowed!"
                                    ValidationExpression="(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.pdf|.xls|.doc|.xlsx|.docx|.PDF|.XLS|.DOC|.XLSX|.DOCX)$"
                                    ControlToValidate="uploadFile" ValidationGroup="vsSupportingDocuments" Font-Bold="True"
                                    Font-Size="Small" />
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 27px">
                            </td>
                            <td style="height: 27px">
                                <asp:Button ID="btnUpload" runat="server" Text="Upload" CausesValidation="true" ValidationGroup="vsSupportingDocuments" />
                                <asp:Button ID="btnReset3" runat="server" CausesValidation="False" Text="Reset" /><br />
                                <br />
                                <asp:Label ID="lblMessageView4" runat="server" Font-Bold="True" ForeColor="Red" Height="16px"
                                    Text="Label" Visible="False" Width="368px" Font-Size="Small"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <asp:ValidationSummary ID="vsSupDoc" runat="server" ValidationGroup="vsSupportingDocuments"
                        ShowMessageBox="true" ShowSummary="true" />
                </asp:Panel>
                <ajax:CollapsiblePanelExtender ID="SDExtender" runat="server" TargetControlID="SDContentPanel"
                    ExpandControlID="SDPanel" CollapseControlID="SDPanel" Collapsed="FALSE" TextLabelID="lblSD"
                    ExpandedText="UPLOAD DOCUMENT(S):" CollapsedText="UPLOAD DOCUMENT(S):" ImageControlID="imgSD"
                    CollapsedImage="~/images/expand_blue.jpg" ExpandedImage="~/images/collapse_blue.jpg"
                    SuppressPostBack="true">
                </ajax:CollapsiblePanelExtender>
                <br />
                <asp:GridView ID="gvSupportingDocument" runat="server" AutoGenerateColumns="False"
                    DataKeyNames="SUPNo,DocID" DataSourceID="odsSupportingDocument" Width="800px"
                    AllowSorting="True" SkinID="StandardGridWOFooter">
                    <Columns>
                        <asp:TemplateField HeaderText="Preview">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="hlnkPreview" ImageUrl='<%# DisplayImage(DataBinder.Eval(Container, "DataItem.EncodeType").tostring)  %>'
                                    NavigateUrl='<%# "SupplierRequestDocument.aspx?pSUPNo=" & DataBinder.Eval (Container.DataItem,"SUPNo").tostring & "&pDocID=" & DataBinder.Eval (Container.DataItem,"DocID").tostring %>'
                                    Target="_blank" Visible='<%# Bind("BinaryFound") %>' ToolTip="Preview" />
                            </ItemTemplate>
                            <HeaderStyle Width="30px" />
                            <ItemStyle HorizontalAlign="Center" Width="30px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Form Name" SortExpression="FormName">
                            <ItemTemplate>
                                <% If ViewState("pForm") <> Nothing Then%>
                                <asp:HyperLink ID="lblFormName" runat="server" Font-Underline="true" Text='<%# Bind("FormName") %>'
                                    NavigateUrl='<%# GoBackToForm(DataBinder.Eval (Container.DataItem,"DocID").tostring)  %>' />
                                <% Else%>
                                <asp:HyperLink ID="lblFormName2" runat="server" Font-Underline="true" Text='<%# Bind("FormName") %>'
                                    NavigateUrl='<%# "SupplierRequest.aspx?pDocID=" & DataBinder.Eval (Container.DataItem,"DocID").tostring & "&pSUPNo=" & ViewState("pSUPNo")  %>' />
                                <% End If%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Required" SortExpression="RequiredForm" HeaderStyle-Width="30px"
                            ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbReqForm" runat="server" Checked='<%# Bind("RequiredForm") %>'
                                    Enabled="false" />
                            </ItemTemplate>
                            <HeaderStyle Width="30px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="TeamMemberName" HeaderText="Uploaded By" SortExpression="TeamMemberName">
                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                            <ItemStyle Width="150px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DateOfUpload" HeaderText="Upload Date" SortExpression="DateOfUpload">
                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                    ImageUrl="~/images/delete.jpg" Visible='<%# DisplayDeleteBtn(DataBinder.Eval(Container, "DataItem.SRFID")).ToString %>'
                                    AlternateText="Delete" />
                            </ItemTemplate>
                            <HeaderStyle Width="30px" />
                            <ItemStyle HorizontalAlign="Right" Width="30px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsSupportingDocument" runat="server" DeleteMethod="DeleteSupplierRequestDocuments"
                    OldValuesParameterFormatString="original_{0}" SelectMethod="GetSupplierRequestDocuments"
                    TypeName="SupplierRequestDocumentsBLL">
                    <DeleteParameters>
                        <asp:Parameter Name="SUPNo" Type="Int32" />
                        <asp:Parameter Name="Original_DocID" Type="Int32" />
                        <asp:Parameter Name="Original_SUPNo" Type="Int32" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:QueryStringParameter Name="SUPNo" QueryStringField="pSUPNo" Type="Int32" DefaultValue="" />
                        <asp:Parameter Name="DocID" Type="Int32" DefaultValue="0" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </asp:View>
            <asp:View ID="vwApprovalStatus" runat="server">
                <br />
                <table>
                    <tr>
                        <td class="p_text">
                            Vendor Type:
                        </td>
                        <td style="color: #990000;">
                            <asp:Label ID="lblVendorType" runat="server" Text="0" CssClass="c_text" Font-Bold="True"
                                Font-Overline="False" Font-Size="Larger" Font-Underline="False" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Label ID="lblReqAppComments" runat="server" Visible="false" CssClass="c_text"
                    Font-Bold="true" />
                <asp:GridView ID="gvApprovers" runat="server" AutoGenerateColumns="False" DataKeyNames="SUPNo,SeqNo,OrigTeamMemberID,TeamMemberID"
                    OnRowUpdating="gvApprovers_RowUpdating" OnRowDataBound="gvApprovers_RowDataBound"
                    DataSourceID="odsApprovers" Width="1000px" RowStyle-Height="20px" RowStyle-CssClass="c_text"
                    HeaderStyle-CssClass="c_text">
                    <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" Wrap="False" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="c_text" Height="20px" />
                    <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" CssClass="c_text" />
                    <EditRowStyle BackColor="#E2DED6" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <EmptyDataRowStyle Wrap="False" />
                    <Columns>
                        <asp:TemplateField HeaderText="Approval Level" SortExpression="SeqNo">
                            <EditItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("SeqNo") %>'></asp:Label></EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("SeqNo") %>'></asp:Label></ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="50px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="OrigTeamMemberName" HeaderText="Original Team Member"
                            SortExpression="OrigTeamMemberName" Visible="False">
                            <HeaderStyle HorizontalAlign="Left" Width="140px" Wrap="True" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Assigned Team Member" SortExpression="TeamMemberName">
                            <EditItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("OrigTeamMemberName") %>'></asp:Label></EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("OrigTeamMemberName") %>'></asp:Label></ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="150px" Wrap="True" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date Notified" SortExpression="DateNotified">
                            <EditItemTemplate>
                                <asp:Label ID="txtDateNotified" runat="server" Text='<%# Bind("DateNotified") %>'></asp:Label></EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDateNotified" runat="server" Text='<%# Bind("DateNotified") %>'></asp:Label></ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="80px" />
                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status" SortExpression="Status">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddStatus" runat="server" SelectedValue='<%# Bind("Status") %>'>
                                    <asp:ListItem>Pending</asp:ListItem>
                                    <asp:ListItem>Approved</asp:ListItem>
                                    <asp:ListItem>Rejected</asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>'></asp:Label></ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="70px" />
                            <ItemStyle HorizontalAlign="Center" Width="70px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="DateSigned" HeaderText="Date Signed" SortExpression="DateSigned"
                            ReadOnly="True">
                            <HeaderStyle HorizontalAlign="Center" Width="80px" />
                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Comments" SortExpression="Comments">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtAppComments" runat="server" MaxLength="200" Rows="2" TextMode="MultiLine"
                                    Text='<%# Bind("Comments") %>' Width="300px"></asp:TextBox><asp:RequiredFieldValidator
                                        ID="rfvComments" runat="server" ControlToValidate="txtAppComments" ErrorMessage="Comments is a required field when approving for another team member."
                                        Font-Bold="True" ValidationGroup="EditApprovalInfo"><</asp:RequiredFieldValidator><asp:TextBox
                                            ID="txtTeamMemberID" runat="server" Text='<%# Eval("TeamMemberID") %>' ReadOnly="true"
                                            Width="0px" Visible="false" /><asp:TextBox ID="hfSeqNo" runat="server" Text='<%# Eval("SeqNo") %>'
                                                ReadOnly="true" Width="0px" Visible="false" /></EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Comments") %>'></asp:Label></ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="400px" />
                            <ItemStyle Wrap="True" />
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <EditItemTemplate>
                                <asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                    ImageUrl="~/images/save.jpg" ToolTip="Save" ValidationGroup="EditApprovalInfo" />&nbsp;&nbsp;&nbsp;
                                <asp:ImageButton ID="ibtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                    ImageUrl="~/images/cancel.jpg" ToolTip="Cancel" ValidationGroup="EditApprovalInfo" /></EditItemTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                    ToolTip="Edit" ImageUrl="~/images/edit.jpg" ValidationGroup="EditApprovalInfo" />&nbsp;&nbsp;&nbsp;</ItemTemplate>
                            <ItemStyle Width="60px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ValidationSummary ID="vsEditApprovalInfo" runat="server" ShowMessageBox="True"
                    ValidationGroup="EditApprovalInfo" />
                <asp:ValidationSummary ID="vsInsertApprovalInfo" runat="server" ShowMessageBox="True"
                    ValidationGroup="InsertApprovalInfo" />
                <asp:ObjectDataSource ID="odsApprovers" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetSupplierRequestApproval" UpdateMethod="UpdateSupplierRequestApproval"
                    TypeName="SupplierRequestApprovalBLL">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="SUPNo" QueryStringField="pSUPNo" Type="Int32" />
                        <asp:Parameter DefaultValue="0" Name="Sequence" Type="Int32" />
                    </SelectParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="Status" Type="String" />
                        <asp:Parameter Name="Comments" Type="String" />
                        <asp:Parameter Name="SameTMID" Type="Boolean" />
                        <asp:Parameter Name="original_SUPNo" Type="Int32" />
                        <asp:Parameter Name="original_SeqNo" Type="Int32" />
                        <asp:Parameter Name="original_TeamMemberID" Type="Int32" />
                        <asp:Parameter Name="original_OrigTeamMemberID" Type="Int32" />
                        <asp:Parameter Name="TeamMemberName" Type="String" />
                        <asp:Parameter Name="DateNotified" Type="String" />
                        <asp:Parameter Name="SeqNo" Type="Int32" />
                        <asp:Parameter Name="OrigTeamMemberName" Type="String" />
                    </UpdateParameters>
                </asp:ObjectDataSource>
                <br />
                <table>
                    <tr>
                        <td>
                        </td>
                        <td>
                            &nbsp;<asp:Button ID="btnBuildApproval" runat="server" CausesValidation="False" Text="Build Approval List" />
                            <asp:Button ID="btnFwdApproval" runat="server" Text="Submit for Approval" Width="130px"
                                CausesValidation="False" />
                        </td>
                    </tr>
                </table>
            </asp:View>
        </asp:MultiView>
    </asp:Panel>
</asp:Content>
