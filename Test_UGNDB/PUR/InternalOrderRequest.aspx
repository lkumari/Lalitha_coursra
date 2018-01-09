<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="InternalOrderRequest.aspx.vb" Inherits="IOR_InternalOrderRequest" Title="Untitled Page"
    MaintainScrollPositionOnPostback="True" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="1000px">
        <asp:Label ID="lblErrors" runat="server" Font-Bold="True" Font-Size="Large" ForeColor="Red"
            Text="Label" Visible="False" CssClass="c_textbold" SkinID="MessageLabelSkin" />
     <%--   <% If ViewState("pIORNo") <> Nothing Then%>
        <table>
            <tr>
                <td class="p_smalltextbold" style="width: 532px; color: #990000">
                    Edit data below or press
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CausesValidation="False" />
                    to enter new data.
                </td>
            </tr>
        </table>
        <%  End If%>--%>
        <hr />
        <br />
        <table width="930px">
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblRefNo" runat="server" Text="Reference #:" />
                </td>
                <td>
                    <asp:Label ID="lblIORNO" runat="server" Text="0" CssClass="c_text" Style="color: #990000;"
                        Font-Bold="True" Font-Overline="False" Font-Size="Small" Font-Underline="False" />
                </td>
                <td class="p_text" style="font-style: italic;">
                    <asp:Label ID="lblReqApprovedSpending" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="* " Visible="false" />
                    <asp:Label ID="lblTotalCapEx" runat="server" Text="Approved Appropriation Amount ($):" />
                </td>
                <td class="p_textbold" style="color: red; text-align: right">
                    <asp:Label ID="txtTotalCapEx" runat="server" Text="0.00" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="Label3" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                    <asp:Label ID="lblDescription" runat="server" Text="Description:" />
                </td>
                <td>
                    <asp:TextBox ID="txtIORDescription" runat="server" MaxLength="50" Width="300px" />
                    <asp:RequiredFieldValidator ID="rfvIORDescription" runat="server" ControlToValidate="txtIORDescription"
                        ErrorMessage="Description is a required field." Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator><br />
                    <asp:Label ID="lblIORDescription" runat="server" Font-Bold="True" ForeColor="Red" />
                </td>
                <td class="p_text" style="font-style: italic;">
                    <asp:Label ID="lblTotalExtension" runat="server" Text="Current IOR Request ($):" />
                </td>
                <td class="p_textbold" style="color: red; text-align: right">
                    <asp:Label ID="txtTotalExtension" runat="server" Text="0.00" />
                    <asp:Label ID="hdTotalExtension" runat="server" Text="0.00" Visible="false" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblAppropriation" runat="server" Text="Appropriation (A, D, P, T, R):" />
                </td>
                <td class="c_text">
                    <asp:TextBox ID="txtAppropriation" runat="server" MaxLength="15" Width="100px" AutoPostBack="True" />
                    <asp:HyperLink ID="hplkAppropriation" runat="server" Font-Underline="true" ForeColor="Blue"
                        Target="_blank" Visible="false" />
                    <asp:TextBox ID="txtProjectTitle" runat="server" Visible="false" Width="16px" />
                    <asp:TextBox ID="txtDefinedCapex" runat="server" Visible="false" Width="16px" />
                    <asp:TextBox ID="txtProjectStatus" runat="server" Visible="false" Width="16px" />
                </td>
                <td class="p_text" style="font-style: italic;">
                    <% If ViewState("pProjNo") <> Nothing Then%>
                    <asp:ImageButton ID="iBtnPreview" runat="server" ImageUrl="~/images/Search.gif" ToolTip="Preview list of IOR's by assigned Appropriation." />
                    <% End If%>
                    <asp:Label ID="lblTotalSpent" runat="server" Text="Other IOR Requests ($):" Height="20px" />
                </td>
                <td class="p_textbold" style="color: red; text-align: right">
                    <asp:Label ID="txtTotalSpent" runat="server" Text="0.00" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblStatus" runat="server" Text="Status:" />
                </td>
                <td style="color: #990000;">
                    <asp:DropDownList ID="ddIORStatus" runat="server" AutoPostBack="True">
                        <asp:ListItem Value="Open">New Entry</asp:ListItem>
                        <asp:ListItem>Void</asp:ListItem>
                        <asp:ListItem>Approved</asp:ListItem>
                        <asp:ListItem>Completed</asp:ListItem>
                        <asp:ListItem>Closed</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddIORStatus2" runat="server" AutoPostBack="True">
                        <asp:ListItem>In Process</asp:ListItem>
                        <asp:ListItem>Void</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;<asp:Label ID="lblRoutingStatusDesc" runat="server" Text="Pending Submission"
                        CssClass="c_text" Font-Bold="True" Font-Overline="False" Font-Size="Small" Font-Underline="False" />
                    <asp:TextBox ID="txtRoutingStatus" Visible="false" runat="server" Width="1px" />
                </td>
                <td class="p_text" style="font-style: italic;">
                    <asp:Label ID="lblRemainingCapEx" runat="server" Text="Remaining Appropriation Balance ($):" />
                </td>
                <td class="p_textbold" style="color: red; text-align: right">
                    <asp:Label ID="txtRemainingCapEx" runat="server" Text="0.00" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblDateSubmitted" runat="server" Text="Date Submitted:" />
                </td>
                <td style="color: #990000;">
                    <asp:Label ID="txtSubmittedOn" runat="server" Text="" CssClass="c_text" Font-Bold="True" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblReqPONo" runat="server" Font-Bold="True" ForeColor="Red" Text="* "
                        Visible="false" />
                    <asp:Label ID="lblPONo" runat="server" Text="Purchase Order #:" />
                </td>
                <td class="c_text" style="color: #990000; text-align: right">
                    <asp:TextBox ID="txtPONo" runat="server" MaxLength="6" Width="100px" />
                    <ajax:FilteredTextBoxExtender ID="ftPONo" runat="server" TargetControlID="txtPONo"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,%" />
                    <asp:RequiredFieldValidator ID="rfvPONo" runat="server" ControlToValidate="txtPONo"
                        Enabled="false" ErrorMessage="Purchase Order # is a required field." Font-Bold="False"
                        ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="p_text" style="vertical-align: top">
                    <asp:Label ID="lblReqVoidReason" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="* " Visible="false" />
                    <asp:Label ID="lblVoidReason" runat="server" Text="Void Reason:" Visible="false" />
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtVoidReason" runat="server" MaxLength="600" Rows="2" TextMode="MultiLine"
                        Visible="False" Width="300px" />
                    <asp:RequiredFieldValidator ID="rfvVoidReason" runat="server" ControlToValidate="txtVoidReason"
                        Enabled="false" ErrorMessage="Void Reason is a required field." Font-Bold="False"
                        ValidationGroup="vsDetail"><</asp:RequiredFieldValidator><br />
                    <asp:Label ID="lblVoidRsn" runat="server" Font-Bold="True" ForeColor="Red" />
                </td>
            </tr>
        </table>
        <br />
        <table width="100%" border="0">
            <tr>
                <td colspan="3" style="width: 30px">
                    <asp:Menu ID="mnuTabs" Height="30px" runat="server" Orientation="Horizontal" StaticEnableDefaultPopOutImage="false"
                        StaticMenuItemStyle-CssClass="tab" StaticSelectedStyle-CssClass="selectedTab"
                        CssClass="tabs">
                        <Items>
                            <asp:MenuItem Text="Detail" Value="0" ImageUrl="" Selected="true" />
                            <asp:MenuItem Text="Extension" Value="1" ImageUrl="" />
                            <asp:MenuItem Text="Supporting Documents" Value="2" ImageUrl="" />
                            <asp:MenuItem Text="Approval Status" Value="3" ImageUrl="" />
                            <asp:MenuItem Text="Communication Board" Value="4" ImageUrl="" />
                        </Items>
                    </asp:Menu>
                </td>
            </tr>
        </table>
        <asp:MultiView ID="mvTabs" runat="server" Visible="true" ActiveViewIndex="0" EnableViewState="true">
            <asp:View ID="vwDetail" runat="server">
                <table>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label9" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            <asp:Label ID="lblRequestedBy" runat="server" Text="Requested By:" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddRequestedBy" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvRequestedBy" runat="server" ControlToValidate="ddRequestedBy"
                                ErrorMessage="Requested By is a required field." ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtSubmittedByTMID" runat="server" Visible="false" Width="16px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label10" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            <asp:Label ID="lblBuyer" runat="server" Text="Buyer:" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddBuyer" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvBuyer" runat="server" ControlToValidate="ddBuyer"
                                ErrorMessage="Buyer is a required field." Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            <asp:Label ID="lblShipToAttention" runat="server" Text="Ship To Attention:" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddShipToAttention" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvShipToAttn" runat="server" ControlToValidate="ddShipToAttention"
                                ErrorMessage="Ship To Attention is a required field." Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label11" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            <asp:Label ID="lblShipTo" runat="server" Text="Ship To:" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddShipTo" runat="server">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem>UGN Location</asp:ListItem>
                                <asp:ListItem>Vendor</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvShipTo" runat="server" ControlToValidate="ddShipTo"
                                ErrorMessage="Ship To is a required field." Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            <asp:Label ID="lblUGNLocation" runat="server" Text="UGN Location:" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddShipToLocation" runat="server" AutoPostBack="true" />
                            <asp:RequiredFieldValidator ID="rfvShipToLocation" runat="server" ControlToValidate="ddShipToLocation"
                                ErrorMessage="Ship To is a required field." ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtUGNLocation" runat="server" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblReqPOinPesos" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            <asp:Label ID="lblPOinPesos" runat="server" Text="Create PO in MXN Pesos?" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddPOinPesos" runat="server">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem Value="False">No</asp:ListItem>
                                <asp:ListItem Value="True">Yes</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvPOinPesos" runat="server" ControlToValidate="ddPOinPesos"
                                ErrorMessage="Create PO in Pesos? is required. " Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label4" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            <asp:Label ID="lblDeptCostCenter" runat="server" Text="Department / Cost
                            Center:" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddDepartment" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvDepartment" runat="server" ControlToValidate="ddDepartment"
                                ErrorMessage="Department / Cost Center is a required field." Font-Bold="False"
                                ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label5" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            <asp:Label ID="lblGLAccount" runat="server" Text="G/L Account #:" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddGLAccount" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvGLAccount" runat="server" ControlToValidate="ddGLAccount"
                                ErrorMessage="GL Account # is a required field." Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label8" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            <asp:Label ID="lblExpDeliveryDt" runat="server" Text="Expected Delivery Date:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtExptdDeliveryDate" runat="server" MaxLength="10" Width="80px" />
                            <asp:ImageButton runat="server" ID="imgExptdDeliveryDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                                CausesValidation="False" />
                            &nbsp;<asp:RequiredFieldValidator ID="rfvExptdDeliveryDate" runat="server" ControlToValidate="txtExptdDeliveryDate"
                                ErrorMessage="Expected Delivery Date is a required field." ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>&nbsp;<asp:RegularExpressionValidator
                                    ID="revExptdDeliveryDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                    ControlToValidate="txtExptdDeliveryDate" Font-Bold="True" ToolTip="MM/DD/YYYY"
                                    ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                    ValidationGroup="vsDetail" Width="8px"><</asp:RegularExpressionValidator><ajax:CalendarExtender
                                        ID="ceExptdDeliveryDate" runat="server" TargetControlID="txtExptdDeliveryDate"
                                        Format="MM/dd/yyyy" PopupButtonID="imgExptdDeliveryDate" />
                        </td>
                    </tr>
                    <%If ViewState("Admin") = True Then%>
                    <tr>
                        <td class="p_text">
                            <asp:ImageButton ID="ibtnSupplierLookUp" runat="server" ImageUrl="~/images/Search.gif" />
                            <asp:Label ID="Label6" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            <asp:Label ID="lblVendor" runat="server" Text="Vendor:" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddVendor" runat="server" AutoPostBack="True" />
                            <asp:RequiredFieldValidator ID="rfvVendor" runat="server" ControlToValidate="ddVendor"
                                ErrorMessage="Vendor is a required field." Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>&nbsp;&nbsp;
                            <asp:TextBox ID="txtVendorName" runat="server" Visible="False" Width="40px" Wrap="False" />
                            <asp:TextBox ID="txtVTYPE" runat="server" Visible="false" Width="40px" Wrap="False" />
                            <asp:TextBox ID="txtFutureVendor" runat="server" Visible="false" Width="40px" Wrap="False" />
                        </td>
                    </tr>
                    <tr>
                        <td class="c_textbold" colspan="2" style="color: #990000">
                            <asp:Label ID="lblVendorInfo" runat="server" Text="VENDOR INFORMATION: Please make sure that the Vendor Information is correct before submitting." />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblVendorAddress" runat="server" Text="Vendor Address:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtVendorAddr1" runat="server" MaxLength="30" Width="300px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                        </td>
                        <td>
                            <asp:TextBox ID="txtVendorAddr2" runat="server" Width="300px" MaxLength="30" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblVendorCountry" runat="server" Text="Vendor Country:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtVendorCountry" runat="server" MaxLength="30" Width="150px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblVendorCity" runat="server" Text="Vendor City:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtVendorCity" runat="server" MaxLength="30" Width="250px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblVendorState" runat="server" Text="Vendor State:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtVendorState" runat="server" MaxLength="30" Width="150px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblVendorZip" runat="server" Text="Vendor Zip:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtVendorZip" runat="server" MaxLength="10" /><ajax:FilteredTextBoxExtender
                                ID="ftVendorZip" runat="server" TargetControlID="txtVendorZip" FilterType="Custom"
                                ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,- " />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblVendorContact" runat="server" Text="Vendor Contact:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtVendorContact" runat="server" MaxLength="50" Width="250px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblContactEmail" runat="server" Text="Contact Email:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtVendorEmail" runat="server" MaxLength="100" Width="370px" />
                            <br />
                            <asp:Label ID="lblVendorEmail" runat="server" Font-Bold="True" ForeColor="Red" /><ajax:FilteredTextBoxExtender
                                ID="ftbeVendorEmail" runat="server" TargetControlID="txtVendorEmail" FilterType="Custom"
                                ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,@-_." />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblVendorWeb" runat="server" Text="Vendor Website:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtVendorWebsite" runat="server" MaxLength="60" Width="370px" /><br />
                            <asp:Label ID="lblVendorWebSite" runat="server" Font-Bold="True" ForeColor="Red" /><ajax:FilteredTextBoxExtender
                                ID="ftVendorWebSite" runat="server" TargetControlID="txtVendorWebSite" FilterType="Custom"
                                ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,@-_." />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblVendorPhone" runat="server" Text="Vendor Phone:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtVendorPhone" runat="server" MaxLength="25" Width="200px" /><ajax:FilteredTextBoxExtender
                                ID="ftVendorPhone" runat="server" TargetControlID="txtVendorPhone" FilterType="Custom, Numbers"
                                ValidChars="-./\() " />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblVendorFax" runat="server" Text="Vendor Fax:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtVendorFax" runat="server" MaxLength="25" Width="200px" /><ajax:FilteredTextBoxExtender
                                ID="ftVendorFax" runat="server" TargetControlID="txtVendorFax" FilterType="Custom, Numbers"
                                ValidChars="-./\() " />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblTerms" runat="server" Text="Terms:" />
                        </td>
                        <td class="c_text">
                            <asp:Label ID="lblNet" runat="server" Text="Net " />&nbsp;<asp:Label ID="txtTerms"
                                runat="server" /><asp:Label ID="lblDays" runat="server" Text=" Days" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblFreight" runat="server" Text="Freight:" />
                        </td>
                        <td class="c_text">
                            <asp:CheckBox ID="cbShippingPoint" runat="server" Text="Shipping Point" />&nbsp;&nbsp;
                            <asp:CheckBox ID="cbDestination" runat="server" Text="Destination" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Tax:
                        </td>
                        <td class="c_text">
                            <asp:CheckBox ID="cbTaxExempt" runat="server" Text="Exempt" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox
                                ID="cbTaxable" runat="server" Text="Taxable" />
                        </td>
                    </tr>
                    <%End If%>
                    <tr>
                        <td class="p_text" valign="top">
                            <asp:Label ID="lblNotes" runat="server" Text="Notes:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtNotes" runat="server" Rows="4" TextMode="MultiLine" Width="600px"
                                MaxLength="300" /><br />
                            <asp:Label ID="lblNotesChar" runat="server" Font-Bold="True" ForeColor="Red" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
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
                            <asp:ValidationSummary ID="sDetail" runat="server" ShowMessageBox="True" ValidationGroup="vsDetail" />
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vwExtension" runat="server">
                <asp:Panel ID="ExtPanel" runat="server" CssClass="collapsePanelHeader" Width="496px">
                    <asp:Image ID="imgExt" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                        Height="12px" />&nbsp;
                    <asp:Label ID="lblExt" runat="server" CssClass="c_textbold">EXTENSION:</asp:Label>
                </asp:Panel>
                <asp:Panel ID="ExtContentPanel" runat="server" CssClass="collapsePanel" Width="600px">
                    <table>
                        <tr>
                            <td class="p_text" style="width: 341px">
                                <asp:Label ID="lblSizePN" runat="server" Text="Size / PN:" />
                            </td>
                            <td style="width: 485px">
                                <asp:TextBox ID="txtSizePN" runat="server" MaxLength="50" Width="400px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text" style="width: 341px">
                                <asp:Label ID="Label7" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                <asp:Label ID="lblDescription2" runat="server" Text="Description:" />
                            </td>
                            <td style="width: 485px">
                                <asp:TextBox ID="txtDescription" runat="server" MaxLength="50" Width="400px" />
                                &nbsp;<asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription"
                                    ErrorMessage="Description is a required field." ValidationGroup="vsExtExpense"><</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text" style="width: 341px">
                                <asp:Label ID="Label14" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                <asp:Label ID="lblQuantity" runat="server" Text="Quantity:" />
                            </td>
                            <td style="width: 485px">
                                <asp:TextBox ID="txtQuantity" runat="server" MaxLength="10" Width="80px" /><ajax:FilteredTextBoxExtender
                                    ID="ftbeQuantity" runat="server" TargetControlID="txtQuantity" FilterType="Numbers" />
                                &nbsp;<asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ControlToValidate="txtQuantity"
                                    ErrorMessage="Quantity is a required field." ValidationGroup="vsExtExpense"><</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text" style="width: 341px">
                                <asp:Label ID="Label15" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                <asp:Label ID="lblPricePerUnit" runat="server" Text="Price Per Unit ($):" />
                            </td>
                            <td style="width: 485px">
                                <asp:TextBox ID="txtAmountPer" runat="server" MaxLength="20" Width="100px" />
                                <ajax:FilteredTextBoxExtender ID="ftbeAmountPer" runat="server" TargetControlID="txtAmountPer"
                                    FilterType="Custom, Numbers" ValidChars="-." />
                                &nbsp;<asp:RequiredFieldValidator ID="rfvAmountPer" runat="server" ControlToValidate="txtAmountPer"
                                    ErrorMessage="Price Per Unit ($) is a required field." ValidationGroup="vsExtExpense"><</asp:RequiredFieldValidator><asp:DropDownList
                                        ID="ddCurrency" runat="server">
                                        <asp:ListItem Selected="True">USD</asp:ListItem>
                                        <asp:ListItem>MXN</asp:ListItem>
                                    </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text" style="width: 341px">
                                <asp:Label ID="Label12" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                <asp:Label ID="lblUOM" runat="server" Text="Unit of Measure:" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddUOM" runat="server" />
                                &nbsp;<asp:RequiredFieldValidator ID="rfvUOM" runat="server" ControlToValidate="ddUOM"
                                    ErrorMessage="Unit of Measure is a required field." ValidationGroup="vsExtExpense"><</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <%-- <tr>
                            <td class="p_text">
                                Department / Cost Center:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddDepartment2" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                G/L Account #:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddGLAccount2" runat="server" />
                            </td>
                        </tr>--%>
                        <tr>
                            <td class="p_text" valign="top" style="width: 341px">
                                <asp:Label ID="lblComments" runat="server" Text="Comments:" />
                            </td>
                            <td style="width: 485px">
                                <asp:TextBox ID="txtComments" runat="server" MaxLength="300" Rows="3" Width="400px"
                                    TextMode="MultiLine" /><br />
                                <asp:Label ID="lblCommentsChar" runat="server" Font-Bold="True" ForeColor="Red" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 341px">
                            </td>
                            <td style="width: 485px">
                                <asp:Button ID="btnExtension" runat="server" Text="Save" CommandName="AddtoGrid2"
                                    ToolTip="Add to grid." ValidationGroup="vsExtExpense" />
                                <asp:Button ID="btnReset2" runat="server" Text="Reset" CausesValidation="False" />
                                <asp:TextBox ID="txtHDExpenseAmount" runat="server" Visible="False" Width="26px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <asp:ValidationSummary ID="vsExtExpense" runat="server" Font-Size="X-Small" ShowMessageBox="True"
                        ShowSummary="true" ValidationGroup="vsExtExpense" />
                </asp:Panel>
                <ajax:CollapsiblePanelExtender ID="ExtExtender" runat="server" TargetControlID="ExtContentPanel"
                    ExpandControlID="ExtPanel" CollapseControlID="ExtPanel" Collapsed="FALSE" TextLabelID="lblExt"
                    ExpandedText="EXTENSION:" CollapsedText="EXTENSION:" ImageControlID="imgExt"
                    CollapsedImage="~/images/expand_blue.jpg" ExpandedImage="~/images/collapse_blue.jpg"
                    SuppressPostBack="true">
                </ajax:CollapsiblePanelExtender>
                <br />
                <asp:GridView ID="gvExpense" runat="server" AutoGenerateColumns="False" DataKeyNames="EID,IORNO"
                    OnRowDataBound="gvExpense_RowDataBound" OnRowCommand="gvExpense_RowCommand" DataSourceID="odsExpense"
                    CellPadding="4" EmptyDataText="No data available for grid view. Use fields above to add new entry."
                    GridLines="Horizontal" Width="1000px" PageSize="100" SkinID="StandardGridWOFooter">
                    <Columns>
                        <asp:TemplateField HeaderText="Line #" SortExpression="EID">
                            <ItemTemplate>
                                <asp:HyperLink ID="lblSizePN1" runat="server" Font-Underline="true" Text='<%# Bind("EID") %>'
                                    NavigateUrl='<%# "InternalOrderRequest.aspx?pEID=" & DataBinder.Eval (Container.DataItem,"EID").tostring & "&pIORNo=" & ViewState("pIORNo")%>' /></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Size / PN" SortExpression="SizePN">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("SizePN") %>'></asp:Label></ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="200px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description" SortExpression="Description">
                            <ItemTemplate>
                                <asp:Label ID="lblDesc" runat="server" Text='<%# Bind("Description") %>' /></ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="Quantity" HeaderText="Quantity" SortExpression="Quantity"
                            DataFormatString="{0:c}">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Amount" HeaderText="Price Per Unit" SortExpression="Amount"
                            DataFormatString="{0:c}" ItemStyle-HorizontalAlign="right">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Currency" HeaderText="Currency" SortExpression="Currency"
                            ItemStyle-HorizontalAlign="center" Visible="false">
                            <HeaderStyle HorizontalAlign="center" />
                            <ItemStyle HorizontalAlign="center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UnitAbbr" HeaderText="UOM" SortExpression="UnitAbbr">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TotalCost" HeaderText="Extension" SortExpression="TotalCost"
                            DataFormatString="{0:c}">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DepartmentName" HeaderText="Department/Cost Center" SortExpression="DepartmentName"
                            Visible="false" />
                        <asp:BoundField DataField="GLAccountName" HeaderText="G/L Account" SortExpression="GLAccountName"
                            Visible="false" />
                        <asp:BoundField DataField="Notes" HeaderText="Comments" SortExpression="Notes">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                    ToolTip="Delete" ImageUrl="~/images/delete.jpg" /></ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsExpense" runat="server" DeleteMethod="DeleteInternalOrderRequestExpenditure"
                    OldValuesParameterFormatString="original_{0}" SelectMethod="GetInternalOrderRequestExpenditure"
                    TypeName="InternalOrderRequestBLL">
                    <DeleteParameters>
                        <asp:Parameter Name="IORNO" Type="Int32" />
                        <asp:Parameter Name="Original_EID" Type="Int32" />
                        <asp:Parameter Name="Original_IORNO" Type="Int32" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:QueryStringParameter Name="IORNO" QueryStringField="pIORNo" Type="Int32" />
                        <asp:Parameter Name="EID" Type="Int32" DefaultValue="0" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </asp:View>
            <asp:View ID="vsSupportingDocuments" runat="server">
                <asp:Panel ID="SDPanel" runat="server" CssClass="collapsePanelHeader" Width="680px">
                    <asp:Image ID="imgSD" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                        Height="12px" />&nbsp;
                    <asp:Label ID="lblSD" runat="server" Text="Label" CssClass="c_textbold">SUPPORTING DOCUMENT(S):</asp:Label>
                </asp:Panel>
                <asp:Panel ID="SDContentPanel" runat="server" CssClass="collapsePanel" Width="800px">
                    <asp:Label ID="lblSupDoc" runat="server" CssClass="p_smalltextbold" Style="width: 800px;
                        color: #990000" Text="This section is available as an option to include additional information. *.PDF, *.DOC, *.DOCX, *.XLS and *.XLSX files are allowed for upload up to 4MB each." /><br />
                    <asp:Label ID="lblSupDoc2" runat="server" CssClass="p_smalltextbold" Style="width: 800px;
                        color: #990000" Text="NOTE: Please be sure to upload the latest copy of any document. Any changes you make will not be saved to the upload files. Please be sure to make a copy of the file locally and upload a new version. You have the option to delete or keep previous version of the file for reference. Please use the 'File Description' area to comment on the changes you make." />
                    <table>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="Label17" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                <asp:Label ID="lblUploadBy" runat="server" Text="Upload By:" />
                            </td>
                            <td class="c_text">
                                <asp:DropDownList ID="ddTeamMember" runat="server" />
                                <asp:RequiredFieldValidator ID="rfvTeamMember" runat="server" ControlToValidate="ddTeamMember"
                                    ErrorMessage="Team Member is a required field." Font-Bold="False" ValidationGroup="vsSupportingDocuments"><</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text" valign="top">
                                <asp:Label ID="Label16" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                <asp:Label ID="lblFileDescription" runat="server" Text="File Description:" />
                            </td>
                            <td class="c_text">
                                <asp:TextBox ID="txtFileDesc" runat="server" MaxLength="200" Rows="3" TextMode="MultiLine"
                                    Width="600px" />
                                <asp:RequiredFieldValidator ID="rfvFileDesc" runat="server" ControlToValidate="txtFileDesc"
                                    ErrorMessage="File Description is a required field." Font-Bold="False" ValidationGroup="vsSupportingDocuments"><</asp:RequiredFieldValidator><br />
                                <asp:Label ID="lblFileDesc" runat="server" Font-Bold="True" ForeColor="Red" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text" valign="top">
                                <asp:Label ID="Label18" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                <asp:Label ID="lblSupportingDocument" runat="server" Text="Supporting Document:" />
                            </td>
                            <td class="c_text">
                                <asp:FileUpload ID="uploadFile" runat="server" Height="22px" Width="600px" />
                                <asp:RequiredFieldValidator ID="rfvUpload" runat="server" ControlToValidate="uploadFile"
                                    ErrorMessage="Supporting Document is required." Font-Bold="False" ValidationGroup="vsSupportingDocuments"><</asp:RequiredFieldValidator><br />
                                <asp:RegularExpressionValidator ID="revUploadFile" runat="server" ErrorMessage="Only *.PDF, *.DOC, *.DOCX, *.XLS, *.XLSX files are allowed!"
                                    ValidationExpression="(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.pdf|.xls|.doc|.xlsx|.docx|.PDF|.XLS|.DOC|.XLSX|.DOCX)$"
                                    ControlToValidate="uploadFile" ValidationGroup="vsSupportingDocuments" Font-Bold="True"
                                    Font-Size="Small" /><br />
                                <asp:Label ID="lblMessageView4" runat="server" Font-Bold="True" ForeColor="Red" Height="16px"
                                    Text="Label" Visible="False" Width="368px" Font-Size="Small"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 27px">
                            </td>
                            <td style="height: 27px">
                                <asp:Button ID="btnUpload" runat="server" Text="Upload" CausesValidation="true" ValidationGroup="vsSupportingDocuments" />
                                <asp:Button ID="btnReset3" runat="server" CausesValidation="False" Text="Reset" />
                            </td>
                        </tr>
                    </table>
                    <asp:ValidationSummary ID="vsSupDoc" runat="server" ValidationGroup="vsSupportingDocuments"
                        ShowMessageBox="true" ShowSummary="true" />
                </asp:Panel>
                <ajax:CollapsiblePanelExtender ID="SDExtender" runat="server" TargetControlID="SDContentPanel"
                    ExpandControlID="SDPanel" CollapseControlID="SDPanel" Collapsed="FALSE" TextLabelID="lblSD"
                    ExpandedText="SUPPORTING DOCUMENT(S):" CollapsedText="SUPPORTING DOCUMENT(S):"
                    ImageControlID="imgSD" CollapsedImage="~/images/expand_blue.jpg" ExpandedImage="~/images/collapse_blue.jpg"
                    SuppressPostBack="true">
                </ajax:CollapsiblePanelExtender>
                <br />
                <asp:GridView ID="gvSupportingDocument" runat="server" AutoGenerateColumns="False"
                    DataKeyNames="IORNo,DocID" DataSourceID="odsSupportingDocument" Width="900px"
                    SkinID="StandardGridWOFooter">
                    <Columns>
                        <asp:BoundField DataField="Description" HeaderText="File Description for IOR" SortExpression="Description">
                            <HeaderStyle HorizontalAlign="Left" Width="400px" />
                            <ItemStyle Width="400px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="comboUploadBy" HeaderText="Uploaded By" SortExpression="comboUploadBy">
                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                            <ItemStyle Width="150px" />
                        </asp:BoundField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="hlnkPreview" ImageUrl='<%# DisplayImage(DataBinder.Eval(Container, "DataItem.EncodeType").tostring)  %>'
                                    NavigateUrl='<%# "InternalOrderRequestDocument.aspx?pIORNo=" & DataBinder.Eval (Container.DataItem,"IORNo").tostring & "&pDocID=" & DataBinder.Eval (Container.DataItem,"DocID").tostring %>'
                                    Target="_blank" Visible='<%# Bind("BinaryFound") %>' ToolTip="Preview Test Report" /></ItemTemplate>
                            <HeaderStyle Width="30px" />
                            <ItemStyle HorizontalAlign="Center" Width="30px" />
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                    ImageUrl="~/images/delete.jpg" AlternateText="Delete" /></ItemTemplate>
                            <HeaderStyle Width="30px" />
                            <ItemStyle HorizontalAlign="Right" Width="30px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsSupportingDocument" runat="server" DeleteMethod="DeleteInternalOrderRequestDocuments"
                    OldValuesParameterFormatString="original_{0}" SelectMethod="GetInternalOrderRequestDocuments"
                    TypeName="InternalOrderRequestBLL">
                    <DeleteParameters>
                        <asp:Parameter Name="IORNO" Type="Int32" />
                        <asp:Parameter Name="Original_DocID" Type="Int32" />
                        <asp:Parameter Name="Original_IORNO" Type="Int32" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:QueryStringParameter Name="IORNO" QueryStringField="pIORNo" Type="Int32" />
                        <asp:Parameter Name="DocID" Type="Int32" DefaultValue="0" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <br />
                <asp:GridView ID="gvExpProjDocuments" runat="server" AutoGenerateColumns="False"
                    DataKeyNames="ProjectNo" DataSourceID="odsExpProjDocuments" Width="800px" SkinID="StandardGridWOFooter">
                    <Columns>
                        <asp:BoundField DataField="Description" HeaderText="File Description for Appropriation"
                            SortExpression="Description">
                            <HeaderStyle HorizontalAlign="Left" Width="700px" />
                            <ItemStyle Width="700px" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Preview" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="hlnkPreview" ImageUrl='<%# DisplayImage(DataBinder.Eval(Container, "DataItem.EncodeType").tostring)  %>'
                                    NavigateUrl='<%# GoToCapEx(DataBinder.Eval(Container.DataItem,"ProjectNo"),DataBinder.Eval (Container.DataItem,"DocID"))%>'
                                    Target="_blank" Visible='<%# Bind("BinaryFound") %>' ToolTip="Preview Document" /></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsExpProjDocuments" runat="server" SelectMethod="GetExpProjDocuments"
                    TypeName="ExpProjDocumentsBLL" OldValuesParameterFormatString="original_{0}">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <br />
                <br />
                <asp:Panel ID="COTPanel" runat="server" CssClass="collapsePanelHeader" Width="680px">
                    <asp:Image ID="imgCOT" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                        Height="12px" />&nbsp;
                    <asp:Label ID="lblCOT" runat="server" Text="REVISION LEVEL(S) FROM CUSTOMER OWNED TOOLING:"
                        CssClass="c_textbold" />
                </asp:Panel>
                <asp:Panel ID="COTContentPanel" runat="server" CssClass="collapsePanel" Width="800px">
                    <asp:GridView ID="gvExpProjToolingCustomer" runat="server" SkinID="StandardGridWOFooter"
                        AutoGenerateColumns="False" DataKeyNames="ProjectNo,PartNo,RevisionLevel" DataSourceID="odsExpProjToolingCustomer"
                        Width="330px">
                        <Columns>
                            <asp:BoundField DataField="ProjectNo" HeaderText="ProjectNo" ReadOnly="True" SortExpression="ProjectNo"
                                Visible="False" />
                            <asp:BoundField DataField="PartNo" HeaderText="Part Number" SortExpression="PartNo"
                                ReadOnly="true" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="RevisionLevel" HeaderText="Revision Level" SortExpression="RevisionLevel"
                                HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField ShowHeader="False">
                                <EditItemTemplate>
                                    &nbsp;&nbsp;<asp:ImageButton ID="ibtnUpdate" runat="server" AlternateText="Update"
                                        CausesValidation="True" CommandName="Update" ImageUrl="~/images/save.jpg" ValidationGroup="EditSampleTrialEventInfo" />
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:ImageButton ID="ibtnCancel" runat="server" AlternateText="Cancel" CausesValidation="False"
                                        CommandName="Cancel" ImageUrl="~/images/cancel.jpg" ValidationGroup="EditInfo" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    &nbsp;&nbsp;<asp:ImageButton ID="ibtnEdit" runat="server" AlternateText="Edit" CausesValidation="False"
                                        CommandName="Edit" ImageUrl="~/images/edit.jpg" />
                                    &nbsp;&nbsp;&nbsp;
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="odsExpProjToolingCustomer" runat="server" OldValuesParameterFormatString="original_{0}"
                        SelectMethod="GetExpProjToolingCustomerEIOR" TypeName="ExpProjToolingBLL" UpdateMethod="UpdateExpProjToolingCustomerEIOR">
                        <UpdateParameters>
                            <asp:Parameter Name="RevisionLevel" Type="String" />
                            <asp:Parameter Name="original_ProjectNo" Type="String" />
                            <asp:Parameter Name="original_PartNo" Type="String" />
                            <asp:Parameter Name="original_RevisionLevel" Type="String" />
                        </UpdateParameters>
                        <SelectParameters>
                            <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </asp:Panel>
                <ajax:CollapsiblePanelExtender ID="COTExtender" runat="server" TargetControlID="COTContentPanel"
                    ExpandControlID="COTPanel" CollapseControlID="COTPanel" Collapsed="FALSE" TextLabelID="lblCOT"
                    ExpandedText="REVISION LEVEL(S) FROM CUSTOMER OWNED TOOLING:" CollapsedText="REVISION LEVEL(S) FROM CUSTOMER OWNED TOOLING:"
                    ImageControlID="imgCOT" CollapsedImage="~/images/expand_blue.jpg" ExpandedImage="~/images/collapse_blue.jpg"
                    SuppressPostBack="true">
                </ajax:CollapsiblePanelExtender>
            </asp:View>
            <asp:View ID="vwApprovalStatus" runat="server">
                <br />
                <asp:Label ID="lblReqAppComments" runat="server" Visible="false" CssClass="c_text"
                    Font-Bold="true" />
                <asp:GridView ID="gvApprovers" runat="server" AutoGenerateColumns="False" DataKeyNames="IORNO,SeqNo,OrigTeamMemberID,TeamMemberID"
                    OnRowUpdating="gvApprovers_RowUpdating" OnRowDataBound="gvApprovers_RowDataBound"
                    DataSourceID="odsApprovers" Width="1000px" RowStyle-Height="20px" RowStyle-CssClass="c_text"
                    HeaderStyle-CssClass="c_text" SkinID="StandardGridWOFooter">
                    <RowStyle CssClass="c_text" Height="20px" />
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
                    <HeaderStyle CssClass="c_text" />
                </asp:GridView>
                <asp:ValidationSummary ID="vsEditApprovalInfo" runat="server" ShowMessageBox="True"
                    ValidationGroup="EditApprovalInfo" />
                <asp:ValidationSummary ID="vsInsertApprovalInfo" runat="server" ShowMessageBox="True"
                    ValidationGroup="InsertApprovalInfo" />
                <asp:ObjectDataSource ID="odsApprovers" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetInternalOrderRequestApproval" UpdateMethod="UpdateInternalOrderRequestApproval"
                    TypeName="InternalOrderRequestBLL">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="IORNO" QueryStringField="pIORNo" Type="String" />
                        <asp:Parameter DefaultValue="0" Name="Sequence" Type="Int32" />
                    </SelectParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="Status" Type="String" />
                        <asp:Parameter Name="Comments" Type="String" />
                        <asp:Parameter Name="SameTMID" Type="Boolean" />
                        <asp:Parameter Name="original_IORNO" Type="String" />
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
                        <td class="p_text" valign="top">
                            <asp:Label ID="lblReqReSubmit" runat="server" Text="* " ForeColor="Red" /><asp:Label
                                ID="lblReSubmit" runat="server" Text="Reason for Resubmission:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtReSubmit" runat="server" MaxLength="300" Rows="3" TextMode="MultiLine"
                                Width="400px" /><asp:RequiredFieldValidator ID="rfvReSubmit" runat="server" ErrorMessage="Reason for Resubmission is a required field."
                                    ValidationGroup="ReSubmitApproval" ControlToValidate="txtReSubmit"><</asp:RequiredFieldValidator><br />
                            <asp:Label ID="lblReSubmitCnt" runat="server" Font-Bold="True" ForeColor="Red" />&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btnBuildApprovalList" runat="server" Text="Build Approval List" />
                            <asp:Button ID="btnFwdApproval" runat="server" Text="Submit for Approval" CausesValidation="true"
                                ValidationGroup="ReSubmitApproval" />
                        </td>
                    </tr>
                </table>
                <asp:ValidationSummary ID="vsReSubmit" runat="server" ValidationGroup="ReSubmitApproval"
                    ShowMessageBox="true" ShowSummary="true" />
                <br />
            </asp:View>
            <asp:View ID="vwCommunicationBoard" runat="server">
                <asp:Label ID="lblSQC" runat="server" CssClass="p_smalltextbold" Style="width: 532px;
                    color: #990000" Text="Select a 'Question / Comment' from discussion thread below to respond." />
                <table>
                    <tr>
                        <td class="p_text" valign="top">
                            <asp:Label ID="lblQuestComment" runat="server" Text="Question / Comment:" />
                        </td>
                        <td class="c_text">
                            <asp:TextBox ID="txtQC" runat="server" Font-Bold="True" Rows="3" TextMode="MultiLine"
                                Width="550px" ReadOnly="true" />
                            <asp:RequiredFieldValidator ID="rfvQC" runat="server" ErrorMessage="Select a Question / Comment from table below for response."
                                ValidationGroup="ReplyComments" ControlToValidate="txtQC"><</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" valign="top">
                            <asp:Label ID="lblReqReply" runat="server" Text="* " ForeColor="Red" />
                            <asp:Label ID="lblReplyComments" runat="server" Text="Reply / Comments:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtReply" runat="server" Rows="3" TextMode="MultiLine" Width="550px" />
                            <asp:RequiredFieldValidator ID="rfvReply" runat="server" ErrorMessage="Reply / Comments is a required field."
                                ValidationGroup="ReplyComments" ControlToValidate="txtReply"><</asp:RequiredFieldValidator><br />
                            <asp:Label ID="lblReply" runat="server" Font-Bold="True" ForeColor="Red" />&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 26px">
                        </td>
                        <td style="height: 26px">
                            <asp:Button ID="btnRSS" runat="server" Text="Submit" CausesValidation="true" ValidationGroup="ReplyComments" />
                            <asp:Button ID="btnReset4" runat="server" Text="Reset" CausesValidation="False" />
                        </td>
                    </tr>
                </table>
                <asp:ValidationSummary ID="vsReplyComments" runat="server" ValidationGroup="ReplyComments"
                    ShowMessageBox="true" ShowSummary="true" />
                <br />
                <asp:GridView ID="gvQuestion" runat="server" AutoGenerateColumns="False" DataSourceID="odsQuestion"
                    OnRowDataBound="gvQuestion_RowDataBound" Width="900px" RowStyle-BorderStyle="None"
                    SkinID="CommBoardRSS">
                    <RowStyle BorderStyle="None" />
                    <Columns>
                        <asp:TemplateField HeaderText="Reply">
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkAppend" runat="server" ImageUrl="~/images/messanger30.jpg"
                                    ToolTip="Reply" NavigateUrl='<%# GoToCommunicationBoard(DataBinder.Eval(Container, "DataItem.IORNO"),DataBinder.Eval(Container, "DataItem.RSSID"),DataBinder.Eval(Container, "DataItem.ApprovalLevel"),DataBinder.Eval(Container, "DataItem.TeamMemberID")) %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="RSSID" HeaderText="RSSID" SortExpression="RSSID" Visible="false" />
                        <asp:BoundField DataField="Comments" HeaderText="Question / Comment" SortExpression="Comments"
                            HeaderStyle-Width="500px" ItemStyle-Width="500px" ItemStyle-Font-Bold="true"
                            ItemStyle-CssClass="c_text" />
                        <asp:BoundField DataField="TeamMemberName" HeaderText="Submitted By" SortExpression="TeamMemberName"
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
                                            DataKeyNames="IORNO,RSSID" Width="100%" SkinID="CommBoardResponse">
                                            <Columns>
                                                <asp:BoundField DataField="Comments" HeaderText="Response" SortExpression="Comments"
                                                    HeaderStyle-Width="500px" ItemStyle-Width="500px" ItemStyle-Font-Bold="true" />
                                                <asp:BoundField DataField="TeamMemberName" HeaderText="" SortExpression="TeamMemberName"
                                                    HeaderStyle-Width="100px" ItemStyle-Width="100px" />
                                                <asp:BoundField DataField="PostDate" HeaderText="" SortExpression="PostDate" />
                                            </Columns>
                                        </asp:GridView>
                                        <asp:ObjectDataSource ID="odsReply" runat="server" OldValuesParameterFormatString="original_{0}"
                                            SelectMethod="GetInternalOrderRequestRSSReply" TypeName="InternalOrderRequestBLL">
                                            <SelectParameters>
                                                <asp:QueryStringParameter Name="IORNO" QueryStringField="pIORNo" Type="String" />
                                                <asp:Parameter Name="RSSID" Type="Int32" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsQuestion" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetInternalOrderRequestRss" TypeName="InternalOrderRequestBLL">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="IORNO" QueryStringField="pIORNo" Type="Int32" />
                        <asp:Parameter Name="RSSID" Type="Int32" DefaultValue="0" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </asp:View>
        </asp:MultiView>
    </asp:Panel>
</asp:Content>
