<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="ToolingExpProj.aspx.vb" Inherits="EXP_ToolingExpProj" Title="Untitled Page"
    MaintainScrollPositionOnPostback="True" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="1000">
        <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
            Visible="False" CssClass="c_textbold" Font-Size="Medium" />
        <% If ViewState("pProjNo") <> Nothing Then%>
        <table>
            <tr>
                <td class="p_smalltextbold" style="width: 532px; color: #990000">
                    Edit data below or press
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CausesValidation="False" />
                    to enter new data.
                    <% If ViewState("pPrntProjNo") = Nothing And (txtRoutingStatus.Text <> "N" And txtRoutingStatus.Text <> "S" And txtRoutingStatus.Text <> "T") Then%>
                    &nbsp; Press
                    <asp:Button ID="btnAppend" runat="server" Text="Append" CausesValidation="False" />
                    to add a Supplement.
                    <%End If%>
                </td>
            </tr>
        </table>
        <%  End If%>
        <hr />
        <br />
        <table>
            <tr>
                <td class="p_text">
                    Project Number:
                </td>
                <td style="color: #990000; width: 136px;">
                    <asp:Label ID="lblProjectID" runat="server" Text="T0000" CssClass="c_text" Font-Bold="True"
                        Font-Overline="False" Font-Size="Larger" Font-Underline="False" />
                </td>
                <td class="p_text">
                    <asp:Label ID="Label3" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                    Project Title:
                </td>
                <td>
                    <asp:TextBox ID="txtProjectTitle" runat="server" MaxLength="50" Width="400px" />&nbsp;<asp:RequiredFieldValidator
                        ID="rfvProjTitle" runat="server" ControlToValidate="txtProjectTitle" ErrorMessage="Project Title is a required field."
                        Font-Bold="False" ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator><br />
                    <asp:Label ID="lblProjectTitle" runat="server" Font-Bold="True" ForeColor="Red" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="Label32" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                    Project Type:
                </td>
                <td class="c_textbold" style="color: red">
                    <asp:DropDownList ID="ddProjectType" runat="server">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>Internal</asp:ListItem>
                        <asp:ListItem>External</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvProjType" runat="server" ControlToValidate="ddProjectType"
                        ErrorMessage="Project Type is a required field." Font-Bold="False" ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                </td>
                <td class="p_text" colspan="1">
                    Project Status:
                </td>
                <td class="c_textbold" style="color: red;">
                    <asp:DropDownList ID="ddProjectStatus" runat="server" AutoPostBack="True">
                        <asp:ListItem Value="Open">New Tooling Project</asp:ListItem>
                        <asp:ListItem>Approved</asp:ListItem>
                        <asp:ListItem>Tooling Completed</asp:ListItem>
                        <asp:ListItem>Closed</asp:ListItem>
                        <asp:ListItem>Void</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddProjectStatus2" runat="server" AutoPostBack="True">
                        <asp:ListItem>In Process</asp:ListItem>
                        <asp:ListItem>Void</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp; &nbsp;
                    <asp:Label ID="lblRoutingStatusDesc" runat="server" Visible="False" />
                    <asp:TextBox ID="txtRoutingStatus" Visible="false" runat="server" Width="1px" />
                </td>
            </tr>
            <%--Display the following rows after 'T' is voided.--%>
            <tr>
                <td class="p_text" valign="top">
                    <asp:Label ID="lblReqVoidRsn" runat="server" Font-Bold="True" ForeColor="Red" Text="* "
                        Visible="false" />
                    <asp:Label ID="lblVoidRsn" runat="server" Text="Void Reason:" />
                </td>
                <td class="c_text" colspan="3">
                    <asp:TextBox ID="txtVoidReason" runat="server" MaxLength="300" Rows="3" TextMode="MultiLine"
                        Width="550px" /><br />
                    <asp:Label ID="lblVoidReason" runat="server" Font-Bold="True" ForeColor="Red" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="3" class="c_textbold" style="color: red;">
                </td>
            </tr>
        </table>
        <br />
        <table width="100%" border="0">
            <tr>
                <td style="width: 30px">
                    <asp:Menu ID="mnuTabs" Height="30px" runat="server" Orientation="Horizontal" StaticEnableDefaultPopOutImage="false"
                        StaticMenuItemStyle-CssClass="tab" StaticSelectedStyle-CssClass="selectedTab"
                        CssClass="tabs">
                        <Items>
                            <asp:MenuItem Text="Project Detail" Value="0" ImageUrl="" Selected="true" />
                            <asp:MenuItem Text="Customer Info" Value="1" ImageUrl="" />
                            <asp:MenuItem Text="Recovery" Value="2" ImageUrl="" />
                            <asp:MenuItem Text="Tooling Expense" Value="3" ImageUrl="" />
                            <asp:MenuItem Text="Approval Status" Value="4" ImageUrl="" />
                            <asp:MenuItem Text="Supporting Documents" Value="5" ImageUrl="" />
                            <asp:MenuItem Text="Communication Board" Value="6" ImageUrl="" />
                        </Items>
                    </asp:Menu>
                </td>
            </tr>
        </table>
        <asp:MultiView ID="mvTabs" runat="server" Visible="true" ActiveViewIndex="0" EnableViewState="true">
            <asp:View ID="vwRequestInfoTab" runat="server">
                <table>
                    <%If ViewState("pPrntProjNo") <> Nothing Then%>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label24" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Originating Project Number:
                        </td>
                        <td class="c_text" style="color: #990000;">
                            <asp:Label ID="lblPrntProjNo" runat="server" Text="" />
                        </td>
                        <td class="p_text">
                            <asp:Label ID="Label26" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Originating Project Approved Date:
                        </td>
                        <td class="c_text" style="color: #990000;">
                            <asp:Label ID="lblPrntAppDate" runat="server" Text="" />
                        </td>
                    </tr>
                    <%End If%>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblSampleProdDesc" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="* " />
                            UGN Location:
                        </td>
                        <td style="width: 198px">
                            <asp:DropDownList ID="ddUGNFacility" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvUGNFacility" runat="server" ControlToValidate="ddUGNFacility"
                                ErrorMessage="UGN Facility is a required field." Font-Bold="False" ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                        </td>
                        <td class="p_text">
                            <asp:Label ID="Label20" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Date Submitted:
                        </td>
                        <td>
                            <asp:TextBox ID="txtDateSubmitted" runat="server" Width="80px" MaxLength="10" />
                            <ajax:FilteredTextBoxExtender ID="ftbeDateSubmitted" runat="server" TargetControlID="txtDateSubmitted"
                                FilterType="Custom, Numbers" ValidChars="/" />
                            <asp:ImageButton runat="server" ID="imgDateSubmitted" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                                CausesValidation="False" />
                            <asp:RequiredFieldValidator ID="rfvDateSubmitted" runat="server" ControlToValidate="txtDateSubmitted"
                                ErrorMessage="Date Submitted is a required field." ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revDateSubmitted" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                ControlToValidate="txtDateSubmitted" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                Width="8px"><</asp:RegularExpressionValidator>
                            <ajax:CalendarExtender ID="ceDateSub" runat="server" TargetControlID="txtDateSubmitted"
                                Format="MM/dd/yyyy" PopupButtonID="imgDateSubmitted" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label4" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Account Manager:
                        </td>
                        <td style="width: 198px">
                            <asp:DropDownList ID="ddAccountManager" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvAccountManager" runat="server" ControlToValidate="ddAccountManager"
                                ErrorMessage="Account Manager is a required field." Font-Bold="False" ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                        </td>
                        <td class="p_text">
                            <asp:Label ID="Label22" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Program Manager:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddProgramManager" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvProgramManager" runat="server" ControlToValidate="ddProgramManager"
                                ErrorMessage="Program Manager is a required field." Font-Bold="False" ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label7" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Tooling Lead:
                        </td>
                        <td style="width: 198px">
                            <asp:DropDownList ID="ddToolingLead" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvToolingLead" runat="server" ControlToValidate="ddToolingLead"
                                ErrorMessage="Tooling Lead is a required field." Font-Bold="False" ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                        </td>
                        <td class="p_text">
                            <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Purchasing Lead:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddPurchasingLead" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvPurchasingLead" runat="server" ControlToValidate="ddPurchasingLead"
                                ErrorMessage="Purchasing Lead is a required field." Font-Bold="False" ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Tooling Engineering Manager:
                        </td>
                        <td>
                        <asp:DropDownList ID="ddToolEngrMgr" runat="server" Enabled="false" /><asp:textbox runat="server" ID="txtHDToolEngrMgrNotified" Visible="false"/>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" valign="top">
                            <asp:Label ID="Label8" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Projected Date Notes:
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtProjDateNotes" runat="server" MaxLength="2000" Rows="8" TextMode="MultiLine"
                                Width="600px" />
                            <asp:RequiredFieldValidator ID="rfvProjDateNotes" runat="server" ControlToValidate="txtProjDateNotes"
                                ErrorMessage="Projected Date Notes is a required field." Font-Bold="False" ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator><br />
                            <asp:Label ID="lblProjDateNotes" runat="server" Font-Bold="True" ForeColor="Red" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label21" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Amount to be Recovered ($):
                            <asp:TextBox ID="txtHDAmtToBeRecovered" runat="server" Visible="False" Width="20px" />
                        </td>
                        <% If ViewState("pProjNo") = Nothing Then%>
                        <td style="width: 243px">
                            <asp:TextBox ID="txtAmtToBeRecovered" runat="server" Width="100px" MaxLength="16"
                                Text="0.00" />
                            <asp:RequiredFieldValidator ID="rfvAmtToBeRecovered" runat="server" ControlToValidate="txtAmtToBeRecovered"
                                ErrorMessage="Amount to be Recovered is a required field." ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                            <ajax:FilteredTextBoxExtender ID="ftAmtToBeRec" runat="server" TargetControlID="txtAmtToBeRecovered"
                                FilterType="Custom, Numbers" ValidChars="-,." />
                        </td>
                        <% Else%>
                        <td style="width: 243px">
                            <asp:TextBox ID="txtNextAmtToBeRecovered" runat="server" Width="100px" MaxLength="16"
                                Text="0.00" AutoPostBack="true" />
                            <asp:RequiredFieldValidator ID="rfvNextAmtToBeRecovered" runat="server" ControlToValidate="txtNextAmtToBeRecovered"
                                ErrorMessage="Amount to be Recovered is a required field." ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                            <ajax:FilteredTextBoxExtender ID="ftbNextAmtToBeRecovered" runat="server" TargetControlID="txtNextAmtToBeRecovered"
                                FilterType="Custom, Numbers" ValidChars="-,." />
                        </td>
                        <% End If%>
                        <td class="p_text">
                            <asp:Label ID="Label28" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Memo at Program Awarded - Amount to be Recovered ($):
                        </td>
                        <td style="width: 243px">
                            <asp:TextBox ID="txtMPAAmtToBeRecovered" runat="server" Width="100px" MaxLength="16"
                                Text="0.00" />
                            <asp:RequiredFieldValidator ID="rfvMPAAmtToBeRecovered" runat="server" ControlToValidate="txtMPAAmtToBeRecovered"
                                ErrorMessage="Memo at Program Awarded - Amount to be Recovered is a required field."
                                ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                            <ajax:FilteredTextBoxExtender ID="ftbeMPAAmtToBeRecovered" runat="server" TargetControlID="txtMPAAmtToBeRecovered"
                                FilterType="Custom, Numbers" ValidChars="-,." />
                        </td>
                    </tr>
                    <%--Display the following rows after save button has been clicked.--%>
                    <tr>
                        <td class="p_text">
                            Total Investment ($):
                        </td>
                        <td class="c_textbold" style="width: 243px; color: #990000;">
                            <asp:Label ID="lblTotalInvestment" runat="server" Text="0.00" />
                            <asp:TextBox ID="txtHDTotalInvestment" runat="server" Visible="False" Width="20px" />
                        </td>
                        <td class="p_text">
                            Memo at Program Awarded - Total Investment ($):
                        </td>
                        <td class="c_textbold" style="width: 243px; color: #990000;">
                            <asp:Label ID="lblMPATotalInvestment" runat="server" Text="0.00" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Profit / Loss ($):
                        </td>
                        <td class="c_textbold" style="width: 243px; color: #990000;">
                            <asp:Label ID="lblProfitLoss" runat="server" Text="0.00" />
                        </td>
                        <td class="p_text">
                            Memo at Program Awarded - Profit / Loss ($):
                        </td>
                        <td class="c_textbold" style="width: 243px; color: #990000;">
                            <asp:Label ID="lblMPAProfitLoss" runat="server" Text="0.00" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" valign="top">
                            Return on Investment (%):
                        </td>
                        <td class="c_textbold" style="width: 243px; color: #990000;">
                            <asp:Label ID="lblROI" runat="server" Text="0.00" />
                        </td>
                        <td class="p_text" valign="top">
                            Memo at Program Awarded - Return on Investment (%):
                        </td>
                        <td class="c_textbold" style="width: 243px; color: #990000;">
                            <asp:Label ID="lblMPAROI" runat="server" Text="0.00" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="height: 21px">
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblEstCmptDt" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Estimated Completion Date:
                            <asp:TextBox ID="txtHDEstCmpltDt" runat="server" Visible="False" Width="20px" />
                        </td>
                        <% If ViewState("pProjNo") = Nothing Then%>
                        <td class="c_text">
                            <asp:TextBox ID="txtEstCmpltDt" runat="server" Width="80px" MaxLength="10" />
                            <ajax:FilteredTextBoxExtender ID="ftbeEstCmpltDt" runat="server" TargetControlID="txtEstCmpltDt"
                                FilterType="Custom, Numbers" ValidChars="/" />
                            <asp:ImageButton runat="server" ID="imgEstCmpltDt" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                                CausesValidation="False" />
                            <asp:RequiredFieldValidator ID="rfvEstCmpltDt" runat="server" ControlToValidate="txtEstCmpltDt"
                                ErrorMessage="Estimated Completion Date is a required field." ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revEstCmpltDt" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                ControlToValidate="txtEstCmpltDt" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                Width="8px" ValidationGroup="vsProjectDetail"><</asp:RegularExpressionValidator>
                            <ajax:CalendarExtender ID="ceEstCmpltDt" runat="server" TargetControlID="txtEstCmpltDt"
                                Format="MM/dd/yyyy" PopupButtonID="imgEstCmpltDt" />
                        </td>
                        <%  Else%>
                        <td class="c_text">
                            <asp:TextBox ID="txtNextEstCmpltDt" runat="server" Width="80px" MaxLength="10" AutoPostBack="true" />
                            <ajax:FilteredTextBoxExtender ID="ftbeNextEstCmpltDt" runat="server" TargetControlID="txtNextEstCmpltDt"
                                FilterType="Custom, Numbers" ValidChars="/" />
                            <asp:ImageButton runat="server" ID="imgNextEstCmpltDt" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                                CausesValidation="False" />
                            <asp:RequiredFieldValidator ID="rfvNextEstCmpltDt" runat="server" ControlToValidate="txtNextEstCmpltDt"
                                ErrorMessage="Estimated Completion Date is a required field." ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revNextEstCmpltDt" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                ControlToValidate="txtNextEstCmpltDt" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                Width="8px" ValidationGroup="vsProjectDetail"><</asp:RegularExpressionValidator>
                            <ajax:CalendarExtender ID="ceNextEstCmpltDt" runat="server" TargetControlID="txtNextEstCmpltDt"
                                Animated="true" Format="MM/dd/yyyy" PopupButtonID="imgNextEstCmpltDt" />
                            <asp:CompareValidator ID="cvNextEstCmpltDt" runat="server" ControlToValidate="txtNextEstCmpltDt"
                                ControlToCompare="txtEstCmpltDt" ErrorMessage="New Estimated Completion Date must be greater than the Original Estimated Completion Date."
                                Operator="GreaterThan" Type="Date" ValidationGroup="vsProjectDetail"><</asp:CompareValidator>
                        </td>
                        <% End If%>
                        <td class="p_text">
                            <asp:Label ID="Label10" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Expected Tool Return Date:
                        </td>
                        <td>
                            <asp:TextBox ID="txtExpToolRtnDt" runat="server" Width="80px" MaxLength="10" />
                            <ajax:FilteredTextBoxExtender ID="ftbeExpToolRtnDt" runat="server" TargetControlID="txtExpToolRtnDt"
                                FilterType="Custom, Numbers" ValidChars="/" />
                            <asp:ImageButton runat="server" ID="imgExpToolRtnDt" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                                CausesValidation="False" /><asp:RequiredFieldValidator ID="rfvExpToolRtnDt" runat="server"
                                    ControlToValidate="txtExpToolRtnDt" ErrorMessage="Expected Tool Return Date is a required field."
                                    ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revExpToolRtnDt" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                ControlToValidate="txtExpToolRtnDt" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                Width="8px"><</asp:RegularExpressionValidator>
                            <ajax:CalendarExtender ID="ceExpTRtnDt" runat="server" TargetControlID="txtExpToolRtnDt"
                                Format="MM/dd/yyyy" PopupButtonID="imgExpToolRtnDt" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label12" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Estimated Spend Date:
                        </td>
                        <td>
                            <asp:TextBox ID="txtEstSpendDt" runat="server" Width="80px" MaxLength="10" />
                            <ajax:FilteredTextBoxExtender ID="ftbeEstSpendDt" runat="server" TargetControlID="txtEstSpendDt"
                                FilterType="Custom, Numbers" ValidChars="/" />
                            <asp:ImageButton runat="server" ID="imgEstSpendDt" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                                CausesValidation="False" />
                            <asp:RequiredFieldValidator ID="rfvEstSpendDt" runat="server" ControlToValidate="txtEstSpendDt"
                                ErrorMessage="Estimated Spend Date is a required field." ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revEstSpendDt" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                ControlToValidate="txtEstSpendDt" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                Width="8px"><</asp:RegularExpressionValidator>
                            <ajax:CalendarExtender ID="ceEstSpendDt" runat="server" TargetControlID="txtEstSpendDt"
                                Format="MM/dd/yyyy" PopupButtonID="imgEstSpendDt" />
                        </td>
                        <td class="p_text">
                            <asp:Label ID="Label25" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Estimated Customer Recovery Date:
                        </td>
                        <td>
                            <asp:TextBox ID="txtEstRecoveryDt" runat="server" Width="80px" MaxLength="10" />
                            <ajax:FilteredTextBoxExtender ID="ftbeEstRecoveryDt" runat="server" TargetControlID="txtEstRecoveryDt"
                                FilterType="Custom, Numbers" ValidChars="/" />
                            <asp:ImageButton runat="server" ID="imgEstRecoveryDt" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                                CausesValidation="False" />
                            <asp:RequiredFieldValidator ID="rfvEstRecoveryDt" runat="server" ControlToValidate="txtEstRecoveryDt"
                                ErrorMessage="Estimated Customer Recovery Date is a required field." ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revEstRecoveryDt" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                ControlToValidate="txtEstRecoveryDt" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                Width="8px"><</asp:RegularExpressionValidator>
                            <ajax:CalendarExtender ID="ceEstRecoveryDt" runat="server" TargetControlID="txtEstRecoveryDt"
                                Format="MM/dd/yyyy" PopupButtonID="imgEstRecoveryDt" />
                        </td>
                    </tr>
                    <%--Display the following rows after 'T' is closed.--%>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblReqActualCost" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="* " Visible="false" />
                            <asp:Label ID="lblActualCost" runat="server" Text="Actual Cost ($):" />
                        </td>
                        <td class="c_text" style="width: 198px">
                            <asp:TextBox ID="txtActualCost" runat="server" Width="100px" />
                            <ajax:FilteredTextBoxExtender ID="ftActualCost" runat="server" TargetControlID="txtActualCost"
                                FilterType="Custom, Numbers" ValidChars="-." />
                        </td>
                        <td class="p_text">
                            <asp:Label ID="lblReqCustomerCost" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="* " Visible="false" />
                            <asp:Label ID="lblCustomerCost" runat="server" Text="Customer Cost ($):" />
                        </td>
                        <td class="c_text">
                            <asp:TextBox ID="txtCustomerCost" runat="server" Width="100px" />
                            <ajax:FilteredTextBoxExtender ID="ftCCost" runat="server" TargetControlID="txtCustomerCost"
                                FilterType="Custom, Numbers" ValidChars="-." />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" valign="top" style="height: 57px">
                            <asp:Label ID="lblReqClosingNts" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="* " Visible="false" />
                            <asp:Label ID="lblClosingNts" runat="server" Text="Closing Notes:" />
                        </td>
                        <td class="c_text" colspan="3" style="height: 57px">
                            <asp:TextBox ID="txtClosingNotes" runat="server" MaxLength="300" Rows="6" TextMode="MultiLine"
                                Width="550px" /><br />
                            <asp:RequiredFieldValidator ID="rfvClosingNotes" runat="server" ControlToValidate="txtClosingNotes"
                                ErrorMessage="Capitalized Notes is a required field." ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator><br />
                            <asp:Label ID="lblClosingNotes" runat="server" Font-Bold="True" ForeColor="Red" />&nbsp;
                        </td>
                    </tr>
                    <% If (ViewState("ProjectStatus") <> "Open" Or ViewState("ProjectStatus") <> "Void" Or ViewState("ProjectStatus") <> Nothing) Then%>
                    <tr>
                        <% If (txtAmtToBeRecovered.Text <> txtNextAmtToBeRecovered.Text) Then%>
                        <td colspan="2" style="height: 53px">
                            <asp:Label ID="lblReqAmtToBeRecoveredChange" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="* " Visible="false" />
                            <asp:Label ID="lblAmtToBeRecoveredChange" runat="server" Text="Amt to be Rcvrd, Change Reason:"
                                Visible="false" ForeColor="Red" />
                            <asp:TextBox ID="txtAmtToBeRecoveredChngRsn" Width="400px" runat="server" Visible="false" />
                            <asp:RequiredFieldValidator ID="rfvAmtToBeRecoveredChngRsn" runat="server" ErrorMessage="Amount to Be Recovered Change Reason is a required field."
                                ControlToValidate="txtAmtToBeRecoveredChngRsn" ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator><br />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label
                                ID="lblAmtToBeRecovered" runat="server" Font-Bold="True" ForeColor="Red" />
                        </td>
                        <% End If%>
                        <% If (txtHDEstCmpltDt.Text <> txtNextEstCmpltDt.Text) Then%>
                        <td colspan="2" style="height: 53px">
                            <asp:Label ID="lblReqEstCmpltDtChange" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="* " Visible="false" />
                            <asp:Label ID="lblEstCmpltDtChange" runat="server" Text=" Est Cmplt Date, Change Reason:"
                                Visible="false" ForeColor="Red" />
                            <asp:TextBox ID="txtEstCmpltDtChngRsn" Width="400px" runat="server" Visible="false" />
                            <asp:RequiredFieldValidator ID="rfvEstCmpltDtChngRsn" runat="server" ErrorMessage="Estimated Completion Date Change Reason is a required field."
                                ControlToValidate="txtEstCmpltDtChngRsn" ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator><br />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label
                                ID="lblEstCmpltDt" runat="server" Font-Bold="True" ForeColor="Red" />
                        </td>
                        <% End If%>
                    </tr>
                    <% End If%>
                    <tr>
                        <td class="p_text" style="height: 50px">
                        </td>
                        <td colspan="3" style="height: 50px">
                            <asp:Button ID="btnSave1" runat="server" Text="Save" CausesValidation="True" ValidationGroup="vsProjectDetail" />
                            <asp:Button ID="btnReset1" runat="server" Text="Reset" CausesValidation="False" />
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CausesValidation="False" />
                            <asp:Button ID="btnPreview" runat="server" Text="Preview" CausesValidation="False" />
                            <asp:Button ID="btnFwdToProjLead" runat="server" Text="Forward to Project Lead" Width="180px"
                                CausesValidation="False" /><asp:Button ID="btnFwdToolEngrMgr" runat="server" Text="Forward to Tooling Engineering Manager"
                                    CausesValidation="False" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="height: 26px">
                            <asp:ValidationSummary ID="sProjectDetail" ValidationGroup="vsProjectDetail" runat="server"
                                ShowMessageBox="True" />
                        </td>
                    </tr>
                </table>
                <asp:Label ID="lblErrors2" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
                    Visible="False" CssClass="c_textbold" />
            </asp:View>
            <asp:View ID="vwCustomerPart" runat="server">
                <asp:Panel ID="TCPanel" runat="server" CssClass="collapsePanelHeader" Width="496px">
                    <asp:Image ID="imgTC" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                        Height="12px" />&nbsp;
                    <asp:Label ID="lblTC" runat="server" Text="Label" CssClass="c_textbold">CUSTOMER:</asp:Label>
                </asp:Panel>
                <asp:Panel ID="TCContentPanel" runat="server" CssClass="collapsePanel">
                    <table>
                        <tr>
                            <td class="p_text" style="width: 130px">
                                Make:
                            </td>
                            <td style="font-size: smaller" colspan="3">
                                <asp:DropDownList ID="ddMakes" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text" style="width: 130px">
                                Model:
                            </td>
                            <td style="font-size: smaller" colspan="3">
                                <asp:DropDownList ID="ddModel" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text" valign="top" style="width: 130px">
                                <asp:Label ID="Label5" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />&nbsp;Program:
                            </td>
                            <td style="font-size: smaller" colspan="3">
                                <asp:DropDownList ID="ddProgram" runat="server" />
                                &nbsp;<asp:RequiredFieldValidator ID="rfvProgram" runat="server" ControlToValidate="ddProgram"
                                    ErrorMessage="Program is a required field." Font-Bold="False" ValidationGroup="vsCustomer">&lt;</asp:RequiredFieldValidator><asp:ImageButton
                                        ID="iBtnPreviewDetail" runat="server" ImageUrl="~/images/PreviewUp.jpg" ToolTip="Review Program Detail"
                                        Visible="false" />
                                <br />
                                {Program / Platform / Customer / Assembly Plant}
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="Label6" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                Internal Part No:
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtPartNo" runat="server" MaxLength="40" Width="200px" />
                                <asp:RequiredFieldValidator ID="rfvPartNo" runat="server" ControlToValidate="txtPartNo"
                                    ErrorMessage="Part No is a required field." Font-Bold="False" ValidationGroup="vsCustomer"><</asp:RequiredFieldValidator>
                                <ajax:FilteredTextBoxExtender ID="ftbPartNo" runat="server" TargetControlID="txtPartNo"
                                    FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,- " />
                            </td>
                        </tr>
                        <%-- <% If ViewState("pFPNo") = True Then%>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="Label30" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                Future Part No:
                            </td>
                            <td>
                                <asp:TextBox ID="txtFuturePartNo" runat="server" MaxLength="15" Width="180px" />
                                <asp:RequiredFieldValidator ID="rfvFuturePartNo" runat="server" ControlToValidate="txtFuturePartNo"
                                    ErrorMessage="Future Part No is a required field." Font-Bold="False" ValidationGroup="vsCustomer"><</asp:RequiredFieldValidator>
                                <ajax:FilteredTextBoxExtender ID="ftbFuturePartNo" runat="server" TargetControlID="txtFuturePartNo"
                                    FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,- " />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="Label31" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                Future Part Description:
                            </td>
                            <td>
                                <asp:TextBox ID="txtFuturePartDesc" runat="server" MaxLength="50" Width="300px" />
                                <asp:RequiredFieldValidator ID="rfvFuturePartDesc" runat="server" ControlToValidate="txtFuturePartDesc"
                                    ErrorMessage="Future Part Description is a required field." Font-Bold="False"
                                    ValidationGroup="vsCustomer"><</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <% End If%>--%>
                        <tr>
                            <td class="p_text" style="height: 15px">
                                Revision Level:
                            </td>
                            <td style="height: 15px" colspan="3">
                                <asp:TextBox ID="txtRevisionLvl" runat="server" MaxLength="25" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="Label33" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                Lead Time:
                            </td>
                            <td>
                                <asp:TextBox ID="txtLeadTime" runat="server" MaxLength="25" Width="50px" />
                                <ajax:FilteredTextBoxExtender ID="ftbeLeadTime" runat="server" TargetControlID="txtLeadTime"
                                    FilterType="Custom, Numbers" ValidChars="." />
                                <asp:DropDownList ID="ddLeadTime" runat="server">
                                    <asp:ListItem Value="D">Day(s)</asp:ListItem>
                                    <asp:ListItem Value="W" Selected="True">Week(s)</asp:ListItem>
                                    <asp:ListItem Value="M">Month(s)</asp:ListItem>
                                </asp:DropDownList>
                                &nbsp;<asp:RequiredFieldValidator ID="rfvLeadTime" runat="server" ControlToValidate="txtLeadTime"
                                    ErrorMessage="Lead Time is a required field." ValidationGroup="vsCustomer"><</asp:RequiredFieldValidator></td>
                            <td class="p_text">
                                Add'l Lead Comments:
                            </td>
                            <td>
                                <asp:TextBox ID="txtLeadTimeComments" runat="server" MaxLength="30" Width="200px"
                                    Wrap="False" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="Label17" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                SOP Date:
                            </td>
                            <td style="height: 11px" colspan="3">
                                <asp:TextBox ID="txtSOP" runat="server" MaxLength="12" Width="80px" />
                                <ajax:FilteredTextBoxExtender ID="ftbeSOP" runat="server" TargetControlID="txtSOP"
                                    FilterType="Custom, Numbers" ValidChars="/" />
                                <asp:ImageButton runat="server" ID="imgSOP" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                    AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                                    CausesValidation="False" />
                                <ajax:CalendarExtender ID="cbeSOP" runat="server" TargetControlID="txtSOP" PopupButtonID="imgSOP"
                                    Format="MM/dd/yyyy" />
                                &nbsp;<asp:RequiredFieldValidator ID="rfvSOP" runat="server" ControlToValidate="txtSOP"
                                    ErrorMessage="Start of Production is a required field." ValidationGroup="vsCustomer"><</asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                        ID="revSOP" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                        ControlToValidate="txtSOP" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                        Width="8px" ValidationGroup="vsCustomer"><</asp:RegularExpressionValidator><asp:CompareValidator
                                            ID="cvSOP" runat="server" ErrorMessage="Start of Production must be less than End of Production."
                                            ControlToCompare="txtEOP" ControlToValidate="txtSOP" Operator="LessThan" Type="Date"
                                            ValidationGroup="vsCustomer"><</asp:CompareValidator></td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="Label18" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                EOP Date:
                            </td>
                            <td style="height: 11px" colspan="3">
                                <asp:TextBox ID="txtEOP" runat="server" MaxLength="12" Width="80px" />
                                <ajax:FilteredTextBoxExtender ID="ftbeEOP" runat="server" TargetControlID="txtEOP"
                                    FilterType="Custom, Numbers" ValidChars="/" />
                                <asp:ImageButton runat="server" ID="imgEOP" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                    AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                                    CausesValidation="False" />
                                <ajax:CalendarExtender ID="cbeEOP" runat="server" TargetControlID="txtEOP" PopupButtonID="imgEOP"
                                    Format="MM/dd/yyyy" />
                                &nbsp;<asp:RequiredFieldValidator ID="rfvEOP" runat="server" ControlToValidate="txtEOP"
                                    ErrorMessage="End of Production is a required field." ValidationGroup="vsCustomer"><</asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                        ID="revEOP" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                        ControlToValidate="txtEOP" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                        Width="8px" ValidationGroup="vsCustomer"><</asp:RegularExpressionValidator><asp:CompareValidator
                                            ID="cvEOP" runat="server" ControlToCompare="txtSOP" ControlToValidate="txtEOP"
                                            ErrorMessage="End of Production must be greater than Start of Production." Operator="GreaterThan"
                                            Type="Date" ValidationGroup="vsCustomer"><</asp:CompareValidator></td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="Label19" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                Estimated PPAP Date:
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtPPAPDt" runat="server" MaxLength="10" Width="80px" />
                                <ajax:FilteredTextBoxExtender ID="ftbePPAPDt" runat="server" TargetControlID="txtPPAPDt"
                                    FilterType="Custom, Numbers" ValidChars="/" />
                                <asp:ImageButton ID="imgPPAPDt" runat="server" AlternateText="Click to show calendar"
                                    CausesValidation="False" Height="19px" ImageAlign="Middle" ImageUrl="~/images/AJAX/Calendar_scheduleHS.png"
                                    Width="19px" />
                                <ajax:CalendarExtender ID="cbePPAP" runat="server" PopupButtonID="imgPPAPDt" TargetControlID="txtPPAPDt"
                                    Format="MM/dd/yyyy" />
                                &nbsp;<asp:RequiredFieldValidator ID="rfvPPAPDt" runat="server" ControlToValidate="txtPPAPDt"
                                    ErrorMessage="Estimated PPAP Date is a required field." ValidationGroup="vsCustomer"><</asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                        ID="revPPAPDt" runat="server" ControlToValidate="txtPPAPDt" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                        Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                        Width="8px" ValidationGroup="vsCustomer"><</asp:RegularExpressionValidator></td>
                        </tr>
                        <tr>
                            <td style="height: 26px">
                            </td>
                            <td style="height: 26px" colspan="3">
                                <asp:Button ID="btnAddtoGrid1" runat="server" Text="Save" ToolTip="Add to grid."
                                    ValidationGroup="vsCustomer" />
                                <asp:Button ID="btnReset2" runat="server" Text="Reset" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Label ID="Label27" runat="server" Text="** Please allow the screen to refresh after each save, otherwise the next part number entry you select will not be captured."
                        Style="color: #990000" Font-Size="Small" Font-Bold="true" />
                    <asp:ValidationSummary ID="vsCustomer" runat="server" Font-Size="X-Small" ShowMessageBox="True"
                        ShowSummary="true" ValidationGroup="vsCustomer" />
                    <ajax:CascadingDropDown ID="cddMakes" runat="server" TargetControlID="ddMakes" Category="Make"
                        PromptText="Select a Make to filter Model for Program selection below." LoadingText="[Loading Makes...]"
                        ServicePath="~/WS/VehicleCDDService.asmx" ServiceMethod="GetMakes" />
                    <ajax:CascadingDropDown ID="cddModel" runat="server" TargetControlID="ddModel" ParentControlID="ddMakes"
                        Category="Model" PromptText="Select a Model to filter Program below." LoadingText="[Loading Models...]"
                        ServicePath="~/WS/VehicleCDDService.asmx" ServiceMethod="GetModelMaint" />
                    <ajax:CascadingDropDown ID="cddProgram" runat="server" TargetControlID="ddProgram"
                        ParentControlID="ddModel" Category="Program" PromptText="Select a Program for this entry."
                        LoadingText="[Loading Programs...]" ServicePath="~/WS/VehicleCDDService.asmx"
                        ServiceMethod="GetProgramsPlatformAssembly" />
                </asp:Panel>
                <ajax:CollapsiblePanelExtender ID="TCExtender" runat="server" TargetControlID="TCContentPanel"
                    ExpandControlID="TCPanel" CollapseControlID="TCPanel" Collapsed="FALSE" TextLabelID="lblTC"
                    ExpandedText="CUSTOMER:" CollapsedText="CUSTOMER:" ImageControlID="imgTC" CollapsedImage="~/images/expand_blue.jpg"
                    ExpandedImage="~/images/collapse_blue.jpg" SuppressPostBack="true">
                </ajax:CollapsiblePanelExtender>
                <br />
                <br />
                <asp:GridView ID="gvCustomer" runat="server" AutoGenerateColumns="False" DataKeyNames="TCID,ProjectNo,ProgramID,PartNo"
                    OnRowDataBound="gvCustomer_RowDataBound" DataSourceID="odsCustomer" CellPadding="4"
                    EmptyDataText="No data available for grid view. Use fields above to add new entry."
                    GridLines="Horizontal" Width="1015px" PageSize="100" SkinID="StandardGridWOFooter">
                    <Columns>
                        <asp:BoundField DataField="ProgramID" HeaderText="ProgramID" 
                            SortExpression="ProgramID" Visible="False" />
                        <asp:TemplateField HeaderText="Customer" SortExpression="ddCustomerDesc">
                            <ItemTemplate>
                                <% If ViewState("Admin") = "true" Then%>
                                <asp:HyperLink ID="lblCustDesc" runat="server" Font-Underline="true" 
                                    NavigateUrl='<%# "ToolingExpProj.aspx?pTCID=" & DataBinder.Eval (Container.DataItem,"TCID").tostring & "&pProjNo=" & ViewState("pProjNo")%>' 
                                    Text='<%# Bind("ddCustomerDesc") %>' />
                                <% Else%>
                                <asp:Label ID="HyperLink3" runat="server" 
                                    Text='<%# Bind("ddCustomerDesc") %>' />
                                <% End If%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Part No." SortExpression="PartNo">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("PartNo") %>' />
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="ProgramName" HeaderText="ProgramName" 
                            SortExpression="ProgramName">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="RevisionLevel" HeaderText="RevisionLevel" 
                            SortExpression="RevisionLevel">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="LeadTime" HeaderText="LeadTime" 
                            SortExpression="LeadTime">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SOP" HeaderText="SOP" SortExpression="SOP">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="EOP" HeaderText="EOP" SortExpression="EOP">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PPAP" HeaderText="PPAP" SortExpression="PPAP">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" 
                                    CommandName="Delete" ImageUrl="~/images/delete.jpg" ToolTip="Delete" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsCustomer" runat="server" SelectMethod="GetExpProjToolingCustomer"
                    OldValuesParameterFormatString="original_{0}" TypeName="ExpProjToolingBLL" DeleteMethod="DeleteExpProjToolingCustomer">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="String" />
                        <asp:Parameter Name="TCID" Type="Int32" DefaultValue="0" />
                    </SelectParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="TCID" Type="Int32" />
                        <asp:Parameter Name="ProjectNo" Type="String" />
                        <asp:Parameter Name="ProgramID" Type="Int32" />
                        <asp:Parameter Name="PartNo" Type="String" />
                        <asp:Parameter Name="original_TCID" Type="Int32" />
                        <asp:Parameter Name="original_ProjectNo" Type="String" />
                        <asp:Parameter Name="original_ProgramID" Type="Int32" />
                        <asp:Parameter Name="original_PartNo" Type="String" />
                    </DeleteParameters>
                </asp:ObjectDataSource>
            </asp:View>
            <asp:View ID="vwRecovery" runat="server">
                <table>
                    <tr>
                        <td class="p_text" style="height: 22px">
                            Amount to be Recovered ($):
                        </td>
                        <td style="height: 22px; color: #990000; text-align: right">
                            <asp:Label ID="lblAmtRecvrd" runat="server" CssClass="c_textbold" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" style="height: 22px">
                            Total Investment ($):
                        </td>
                        <td style="height: 22px; color: #990000; text-align: right">
                            <asp:Label ID="lblTotalInvestment2" runat="server" CssClass="c_textbold" Text="0.00" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" style="height: 22px">
                            Profit/Loss ($):
                        </td>
                        <td style="height: 22px; color: #990000; text-align: right">
                            <hr />
                            <asp:Label ID="lblProfitLoss2" runat="server" CssClass="c_textbold" Text="0.00" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" valign="top">
                            Return on Investment (%):
                        </td>
                        <td style="width: 243px; color: #990000;">
                            <asp:Label ID="lblROI2" runat="server" Text="0.0" CssClass="c_textbold" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td class="p_text" style="height: 22px">
                            <asp:Label ID="Label11" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Recovery Type:
                        </td>
                        <td class="c_text" style="height: 22px">
                            <asp:CheckBox ID="ckRecoveryType1" runat="server" AutoPostBack="True" />&nbsp;Lump
                            Sum&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:CheckBox ID="ckRecoveryType2" runat="server" AutoPostBack="True" />&nbsp;Piece
                            Price&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lblReqRecType" runat="server" Font-Bold="True" ForeColor="Red" Text=" < choose one "
                                Visible="false" />
                        </td>
                    </tr>
                    <%  If ViewState("toolLumpSum") = True Then%>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <table width="100%">
                                <tr>
                                    <td class="p_text" style="width: 159px">
                                        <asp:Label ID="Label16" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                        1st Recovery Amount ($):
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt1stRecoveryAmt" runat="server" MaxLength="16" Width="100px" />
                                        <ajax:FilteredTextBoxExtender ID="ft1stRecAmt" runat="server" TargetControlID="txt1stRecoveryAmt"
                                            FilterType="Custom, Numbers" ValidChars="-,." />
                                        <asp:RequiredFieldValidator ID="rfv1stRecAmt" runat="server" ControlToValidate="txt1stRecoveryAmt"
                                            ErrorMessage="1st Recovery Amount is a required field." ValidationGroup="vsRecoveryType"><</asp:RequiredFieldValidator>
                                    </td>
                                    <td class="p_text">
                                        <asp:Label ID="Label23" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                        1st Recovery Date:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt1stRecoveryDate" runat="server" MaxLength="10" Width="80px" />
                                        <ajax:FilteredTextBoxExtender ID="ftbe1stRecoveryDate" runat="server" TargetControlID="txt1stRecoveryDate"
                                            FilterType="Custom, Numbers" ValidChars="/" />
                                        <asp:ImageButton runat="server" ID="img1stRecDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                            AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                                            CausesValidation="False" />
                                        <asp:RequiredFieldValidator ID="rfv1stRecDate" runat="server" ControlToValidate="txt1stRecoveryDate"
                                            ErrorMessage="1st Recovery Date is a required field." ValidationGroup="vsRecoveryType"><</asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="rev1stRecDate" runat="server" ControlToValidate="txt1stRecoveryDate"
                                            ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format ' Font-Bold="True"
                                            ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                            Width="8px" ValidationGroup="vsRecoveryType"><</asp:RegularExpressionValidator>
                                        <asp:CompareValidator ID="cv1stRecDate" runat="server" ErrorMessage="1st Recovery date must be less than 2nd Recovery Date."
                                            ControlToCompare="txt2ndRecoveryDate" ControlToValidate="txt1stRecoveryDate"
                                            Operator="LessThan" Type="Date" ValidationGroup="vsRecoveryType"><</asp:CompareValidator>
                                        <ajax:CalendarExtender ID="ce1stRecDate" runat="server" TargetControlID="txt1stRecoveryDate"
                                            Format="MM/dd/yyyy" PopupButtonID="img1stRecDate" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text" style="width: 159px">
                                        2nd Recovery Amount ($):
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt2ndRecoveryAmt" runat="server" MaxLength="16" Width="100px" />
                                        <ajax:FilteredTextBoxExtender ID="ft2ndRecAmt" runat="server" TargetControlID="txt2ndRecoveryAmt"
                                            FilterType="Custom, Numbers" ValidChars="-,." />
                                    </td>
                                    <td class="p_text">
                                        2nd Recovery Date:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt2ndRecoveryDate" runat="server" MaxLength="10" Width="80px" />
                                        <ajax:FilteredTextBoxExtender ID="ftbe2ndRecoveryDate" runat="server" TargetControlID="txt2ndRecoveryDate"
                                            FilterType="Custom, Numbers" ValidChars="/" />
                                        <asp:ImageButton runat="server" ID="img2ndRecDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                            AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                                            CausesValidation="False" />
                                        <asp:RegularExpressionValidator ID="rev2ndRecDate" runat="server" ControlToValidate="txt2ndRecoveryDate"
                                            ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format ' Font-Bold="True"
                                            ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                            Width="8px" ValidationGroup="vsRecoveryType"><</asp:RegularExpressionValidator>
                                        <asp:CompareValidator ID="cv2ndRecDate" runat="server" ErrorMessage="2nd Recovery Date must be greater than 1st Recovery Date."
                                            ControlToCompare="txt1stRecoveryDate" ControlToValidate="txt2ndRecoveryDate"
                                            Operator="GreaterThan" Type="Date" ValidationGroup="vsRecoveryType"><</asp:CompareValidator>
                                        <ajax:CalendarExtender ID="ce2ndRecDate" runat="server" TargetControlID="txt2ndRecoveryDate"
                                            Format="MM/dd/yyyy" PopupButtonID="img2ndRecDate" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="height: 9px">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Button ID="btnSave3" runat="server" Text="Save" CausesValidation="true" ValidationGroup="vsRecoveryType" />
                                        <asp:Button ID="btnReset3" runat="server" Text="Reset" CausesValidation="False" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <% End If%>
                    <%If ViewState("toolPiecePrice") = True Then%>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:GridView ID="gvYearlyVolume" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                DataKeyNames="YVID,ProjectNo" EmptyDataText="No data available for grid view. Use fields above to add new entry."
                                GridLines="Horizontal" Width="700px" PageSize="100" DataSourceID="odsYearlyVolume"
                                OnRowDataBound="gvYearlyVolume_RowDataBound" OnRowCommand="gvYearlyVolume_RowCommand"
                                SkinID="StandardGrid">
                                <Columns>
                                    <asp:TemplateField HeaderText="Estimated Volume (All Parts)" SortExpression="Volume">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtVolume" runat="server" Text='<%# Bind("Volume") %>' MaxLength="10"
                                                Width="100px" />
                                            <ajax:FilteredTextBoxExtender ID="ftbeVolume" runat="server" TargetControlID="txtVolume"
                                                FilterType="Numbers" />
                                            <asp:RequiredFieldValidator ID="rfvVolume" runat="server" ControlToValidate="txtVolume"
                                                ErrorMessage="Volume is a required field." ValidationGroup="EditYearlyVolume"><</asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblVolume" runat="server" Text='<%# Bind("Volume","{0:#,###}") %>' />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <% If ViewState("Admin") = "true" Then%>
                                            <asp:TextBox ID="txtVolume" runat="server" Text='<%# Bind("Year") %>' MaxLength="10"
                                                Width="100px" />
                                            <ajax:FilteredTextBoxExtender ID="ftbeVolume" runat="server" TargetControlID="txtVolume"
                                                FilterType="Numbers" />
                                            <asp:RequiredFieldValidator ID="rfvVolume" runat="server" ControlToValidate="txtVolume"
                                                ErrorMessage="Volume is a required field." ValidationGroup="InsertYearlyVolume"><</asp:RequiredFieldValidator>
                                            <% End If%>
                                        </FooterTemplate>
                                        <HeaderStyle Width="120px" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Year" SortExpression="Year">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtYear" runat="server" Text='<%# Bind("Year") %>' MaxLength="4"
                                                Width="80px" />
                                            <ajax:FilteredTextBoxExtender ID="ftbeYear" runat="server" TargetControlID="txtYear"
                                                FilterType="Numbers" />
                                            <asp:RequiredFieldValidator ID="rfvYear" runat="server" ControlToValidate="txtYear"
                                                ErrorMessage="Year is a required field." ValidationGroup="EditYearlyVolume"><</asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblYear" runat="server"
                                                Text='<%# Bind("Year") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <% If ViewState("Admin") = "true" Then%>
                                            <asp:TextBox ID="txtYear" runat="server" Text='<%# Bind("Year") %>' MaxLength="4"
                                                Width="80px" />
                                            <ajax:FilteredTextBoxExtender ID="ftbeYear" runat="server" TargetControlID="txtYear"
                                                FilterType="Numbers" />
                                            <asp:RequiredFieldValidator ID="rfvYear" runat="server" ControlToValidate="txtYear"
                                                ErrorMessage="Year is a required field." ValidationGroup="InsertYearlyVolume"><</asp:RequiredFieldValidator>
                                            <%End If%>
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                                ImageUrl="~/images/save.jpg" ToolTip="Save" ValidationGroup="EditYearlyVolume" />&nbsp;&nbsp;&nbsp;
                                            <asp:ImageButton ID="ibtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                                ImageUrl="~/images/cancel.jpg" ToolTip="Cancel" ValidationGroup="EditYearlyVolume" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                                ImageUrl="~/images/edit.jpg" ToolTip="Edit" ValidationGroup="EditYearlyVolume" />&nbsp;&nbsp;&nbsp;
                                            <asp:ImageButton ID="ibtnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                Visible='<%# ViewState("ObjectRole")%>' ImageUrl="~/images/delete.jpg" ToolTip="Delete"
                                                OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="ibtnInsert" runat="server" CausesValidation="True" CommandName="Insert"
                                                ImageUrl="~/images/save.jpg" ToolTip="Insert" ValidationGroup="InsertYearlyVolume" />&nbsp;&nbsp;&nbsp;
                                            <asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo" ImageUrl="~/images/undo-gray.jpg"
                                                ToolTip="Undo" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <FooterTemplate>
                                        </FooterTemplate>
                                        <HeaderStyle Width="40px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <FooterTemplate>
                                        </FooterTemplate>
                                        <HeaderStyle Width="40px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <FooterTemplate>
                                        </FooterTemplate>
                                        <HeaderStyle Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <FooterTemplate>
                                        </FooterTemplate>
                                        <HeaderStyle Width="40px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <FooterTemplate>
                                        </FooterTemplate>
                                        <HeaderStyle Width="40px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <FooterTemplate>
                                        </FooterTemplate>
                                        <HeaderStyle Width="40px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:ObjectDataSource ID="odsYearlyVolume" runat="server" DeleteMethod="DeleteExpProjToolingYearlyVolume"
                                InsertMethod="InsertExpProjToolingYearlyVolume" OldValuesParameterFormatString="original_{0}"
                                SelectMethod="GetExpProjToolingYearlyVolume" TypeName="ExpProjToolingBLL" UpdateMethod="UpdateExpProjToolingYearlyVolume">
                                <DeleteParameters>
                                    <asp:Parameter Name="YVID" Type="Int32" />
                                    <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="String" />
                                    <asp:Parameter Name="original_Year" Type="Int32" />
                                    <asp:Parameter Name="original_Volume" Type="Int32" />
                                    <asp:Parameter Name="original_YVID" Type="Int32" />
                                    <asp:Parameter Name="original_ProjectNo" Type="String" />
                                </DeleteParameters>
                                <UpdateParameters>
                                    <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="String" />
                                    <asp:Parameter Name="Year" Type="Int32" />
                                    <asp:Parameter Name="Volume" Type="Int32" />
                                    <asp:Parameter Name="Original_Year" Type="Int32" />
                                    <asp:Parameter Name="Original_Volume" Type="Int32" />
                                    <asp:Parameter Name="original_YVID" Type="Int32" />
                                </UpdateParameters>
                                <SelectParameters>
                                    <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="String" />
                                </SelectParameters>
                                <InsertParameters>
                                    <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="String" />
                                    <asp:Parameter Name="Year" Type="Int32" />
                                    <asp:Parameter Name="Volume" Type="Int32" />
                                </InsertParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                    <%End If%>
                </table>
                <asp:ValidationSummary ID="vsRecovery" runat="server" ValidationGroup="vsRecoveryType" />
                <asp:ValidationSummary ID="vsInsertYearlyVolume" runat="server" ShowMessageBox="True"
                    ValidationGroup="InsertYearlyVolume" />
                <asp:ValidationSummary ID="vsEditYearlyVolume" runat="server" ShowMessageBox="True"
                    ValidationGroup="EditYearlyVolume" />
                &nbsp;
                <asp:ValidationSummary ID="vsEmptyYearlyVolume" runat="server" ShowMessageBox="True"
                    ValidationGroup="EmptyYearlyVolume" />
            </asp:View>
            <asp:View ID="vwToolingExpense" runat="server">
                <asp:Panel ID="TEPanel" runat="server" CssClass="collapsePanelHeader" Width="496px">
                    <asp:Image ID="imgTE" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                        Height="12px" />&nbsp;
                    <asp:Label ID="lblTE" runat="server" Text="Label" CssClass="c_textbold">EXPENDITURE:</asp:Label>
                </asp:Panel>
                <asp:Panel ID="TEContentPanel" runat="server" CssClass="collapsePanel" Width="600px">
                    <table>
                        <tr>
                            <td class="p_text" valign="baseline">
                                <asp:Label ID="Label13" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                Description:
                            </td>
                            <td>
                                <asp:TextBox ID="txtDescription" runat="server" MaxLength="50" Width="400px" />
                                <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription"
                                    ErrorMessage="Description is a required field." ValidationGroup="vsToolingExpense"><</asp:RequiredFieldValidator><br />
                                <asp:Label ID="lblDescription" runat="server" Font-Bold="True" ForeColor="Red" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="Label14" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                Quantity:
                            </td>
                            <td>
                                <asp:TextBox ID="txtQuantity" runat="server" MaxLength="10" Width="80px" />
                                <ajax:FilteredTextBoxExtender ID="ftbeQuantity" runat="server" TargetControlID="txtQuantity"
                                    FilterType="Numbers" />
                                <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ControlToValidate="txtQuantity"
                                    ErrorMessage="Quantity is a required field." ValidationGroup="vsToolingExpense"><</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="Label15" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                Amount Per ($):
                            </td>
                            <td>
                                <asp:TextBox ID="txtAmountPer" runat="server" MaxLength="20" Width="100px" />
                                <ajax:FilteredTextBoxExtender ID="ftbeAmountPer" runat="server" TargetControlID="txtAmountPer"
                                    FilterType="Custom, Numbers" ValidChars="-,." />
                                <asp:RequiredFieldValidator ID="rfvAmountPer" runat="server" ControlToValidate="txtAmountPer"
                                    ErrorMessage="Amount Per ($) is a required field." ValidationGroup="vsToolingExpense"><</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="Label9" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                Memo at Program Awarded - Total Cost ($):
                            </td>
                            <td>
                                <asp:TextBox ID="txtMPATotalCost" runat="server" MaxLength="20" Width="100px" />
                                <ajax:FilteredTextBoxExtender ID="ftbeMPATotalCost" runat="server" TargetControlID="txtMPATotalCost"
                                    FilterType="Custom, Numbers" ValidChars="-,." />
                                <asp:RequiredFieldValidator ID="rfvMPATotalCost" runat="server" ControlToValidate="txtMPATotalCost"
                                    ErrorMessage="Memo at Program Awarded - Total Cost ($) is a required field."
                                    ValidationGroup="vsToolingExpense"><</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text" valign="top">
                                Notes:
                            </td>
                            <td>
                                <asp:TextBox ID="txtNotes" runat="server" MaxLength="300" Rows="3" Width="400px"
                                    TextMode="MultiLine" /><br />
                                <asp:Label ID="lblNotes" runat="server" Font-Bold="True" ForeColor="Red" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Button ID="btnAddtoGrid2" runat="server" Text="Save" CommandName="AddtoGrid2"
                                    ToolTip="Add to grid." ValidationGroup="vsToolingExpense" />
                                <asp:Button ID="btnReset4" runat="server" Text="Reset" CausesValidation="False" />
                            </td>
                        </tr>
                    </table>
                    <asp:ValidationSummary ID="vsToolingExpense" runat="server" Font-Size="X-Small" ShowMessageBox="True"
                        ShowSummary="true" ValidationGroup="vsToolingExpense" />
                </asp:Panel>
                <ajax:CollapsiblePanelExtender ID="TEExtender" runat="server" TargetControlID="TEContentPanel"
                    ExpandControlID="TEPanel" CollapseControlID="TEPanel" Collapsed="FALSE" TextLabelID="lblTE"
                    ExpandedText="EXPENDITURE:" CollapsedText="EXPENDITURE:" ImageControlID="imgTE"
                    CollapsedImage="~/images/expand_blue.jpg" ExpandedImage="~/images/collapse_blue.jpg"
                    SuppressPostBack="true">
                </ajax:CollapsiblePanelExtender>
                <br />
                <br />
                <asp:GridView ID="gvExpense" runat="server" AutoGenerateColumns="False" DataKeyNames="EID,ProjectNo"
                    OnRowDataBound="gvExpense_RowDataBound" DataSourceID="odsExpense" CellPadding="4"
                    EmptyDataText="No data available for grid view. Use fields above to add new entry."
                    GridLines="Horizontal" Width="1000px" PageSize="100" SkinID="StandardGridWOFooter">
                    <Columns>
                        <asp:TemplateField HeaderText="Description" SortExpression="Description">
                            <ItemTemplate>
                                <% If ViewState("Admin") = "true" Then%>
                                <asp:HyperLink ID="lblCustDesc" runat="server" Font-Underline="true" Text='<%# Bind("Description") %>'
                                    NavigateUrl='<%# "ToolingExpProj.aspx?pEID=" & DataBinder.Eval (Container.DataItem,"EID").tostring & "&pProjNo=" & ViewState("pProjNo")%>' />
                                <% Else%>
                                <asp:Label ID="lblDesc" runat="server" Text='<%# Bind("Description") %>' />
                                <% End If%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="Quantity" HeaderText="Quantity" SortExpression="Quantity">
                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" DataFormatString="{0:c}">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TotalCost" HeaderText="Total Cost" SortExpression="TotalCost"
                            DataFormatString="{0:c}">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MPATotalCost" HeaderText="Memo at Program Awarded Total Cost"
                            SortExpression="MPATotalCost" DataFormatString="{0:c}">
                            <HeaderStyle HorizontalAlign="Right" Wrap="true" />
                            <ItemStyle HorizontalAlign="Right" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="400px" Wrap="True" />
                        </asp:BoundField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                    ToolTip="Delete" ImageUrl="~/images/delete.jpg" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsExpense" runat="server" DeleteMethod="DeleteExpProjToolingExpenditure"
                    OldValuesParameterFormatString="original_{0}" SelectMethod="GetExpProjToolingExpenditure"
                    TypeName="ExpProjToolingBLL">
                    <DeleteParameters>
                        <asp:Parameter Name="ProjectNo" Type="String" />
                        <asp:Parameter Name="Original_EID" Type="Int32" />
                        <asp:Parameter Name="Original_ProjectNo" Type="String" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="String" />
                        <asp:Parameter Name="EID" Type="Int32" DefaultValue="0" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </asp:View>
            <asp:View ID="vwApprovalStatus" runat="server">
                <br />
                <asp:Label ID="lblReqAppComments" runat="server" Visible="false" CssClass="c_text"
                    Font-Bold="true" />
                <asp:GridView ID="gvApprovers" runat="server" AutoGenerateColumns="False" DataKeyNames="ProjectNo,SeqNo,OrigTeamMemberID,TeamMemberID"
                    OnRowUpdating="gvApprovers_RowUpdating" OnRowDataBound="gvApprovers_RowDataBound"
                    DataSourceID="odsApprovers" Width="1015px" RowStyle-Height="20px" RowStyle-CssClass="c_text"
                    HeaderStyle-CssClass="c_text" SkinID="StandardGridWOFooter">
                    <RowStyle CssClass="c_text" Height="20px" />
                    <Columns>
                        <asp:TemplateField HeaderText="Approval Level" SortExpression="SeqNo">
                            <EditItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("SeqNo") %>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("SeqNo") %>' />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="50px" />
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:Label ID="lblMsg1" runat="server" Text="1" Font-Italic="true" ForeColor="Black" />
                            </FooterTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="OrigTeamMemberName" HeaderText="Original Team Member"
                            SortExpression="OrigTeamMemberName" Visible="False">
                            <HeaderStyle HorizontalAlign="Left" Width="140px" Wrap="True" />
                        </asp:BoundField>
                        <asp:BoundField DataField="OrigTeamMemberName" HeaderText="Assigned Team Member"
                            SortExpression="TeamMemberName" ReadOnly="True">
                            <HeaderStyle HorizontalAlign="Left" Width="150px" Wrap="True" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Date Notified" SortExpression="DateNotified">
                            <EditItemTemplate>
                                <asp:Label ID="txtDateNotified" runat="server" Text='<%# Bind("DateNotified") %>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDateNotified" runat="server" Text='<%# Bind("DateNotified") %>' />
                            </ItemTemplate>
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
                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>' />
                            </ItemTemplate>
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
                                    Text='<%# Bind("Comments") %>' Width="300px" />
                                <asp:RequiredFieldValidator ID="rfvComments" runat="server" ControlToValidate="txtAppComments"
                                    ErrorMessage="Comments is a required field when approving for another team member."
                                    Font-Bold="True" ValidationGroup="EditApprovalInfo"><</asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtTeamMemberID" runat="server" Text='<%# Eval("TeamMemberID") %>'
                                    ReadOnly="true" Width="0px" Visible="false" />
                                <asp:TextBox ID="txtOrigTeamMemberID" runat="server" Text='<%# Eval("OrigTeamMemberID") %>'
                                    ReadOnly="true" Width="0px" Visible="false" />
                                <asp:TextBox ID="hfSeqNo" runat="server" Text='<%# Eval("SeqNo") %>' ReadOnly="true"
                                    Width="0px" Visible="false" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Comments") %>' />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="400px" />
                            <ItemStyle Wrap="True" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="comboUpdateInfo" HeaderText="Last Update" ReadOnly="True"
                            SortExpression="comboUpdateInfo" />
                        <asp:TemplateField ShowHeader="False">
                            <EditItemTemplate>
                                <asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                    ImageUrl="~/images/save.jpg" ToolTip="Save" ValidationGroup="EditApprovalInfo" />&nbsp;&nbsp;&nbsp;
                                <asp:ImageButton ID="ibtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                    ImageUrl="~/images/cancel.jpg" ToolTip="Cancel" ValidationGroup="EditApprovalInfo" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                    ToolTip="Edit" ImageUrl="~/images/edit.jpg" ValidationGroup="EditApprovalInfo" />&nbsp;&nbsp;&nbsp;
                            </ItemTemplate>
                            <ItemStyle Width="60px" />
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="c_text" />
                </asp:GridView>
                <asp:ValidationSummary ID="vsEditApprovalInfo" runat="server" ShowMessageBox="True"
                    ValidationGroup="EditApprovalInfo" />
                <asp:ObjectDataSource ID="odsApprovers" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetExpProjToolingApproval" TypeName="ExpProjToolingBLL" UpdateMethod="UpdateExpProjToolingApproval"
                    DeleteMethod="DeleteExpProjToolingYearlyVolume" InsertMethod="InsertExpProjToolingYearlyVolume">
                    <DeleteParameters>
                        <asp:Parameter Name="YVID" Type="Int32" />
                        <asp:Parameter Name="ProjectNo" Type="String" />
                        <asp:Parameter Name="original_Year" Type="Int32" />
                        <asp:Parameter Name="original_Volume" Type="Int32" />
                        <asp:Parameter Name="original_YVID" Type="Int32" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="Status" Type="String" />
                        <asp:Parameter Name="Comments" Type="String" />
                        <asp:Parameter Name="original_ProjectNo" Type="String" />
                        <asp:Parameter Name="original_SeqNo" Type="Int32" />
                        <asp:Parameter Name="original_TeamMemberID" Type="Int32" />
                        <asp:Parameter Name="original_OrigTeamMemberID" Type="Int32" />
                    </UpdateParameters>
                    <SelectParameters>
                        <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="String" />
                        <asp:Parameter DefaultValue="0" Name="Sequence" Type="Int32" />
                    </SelectParameters>
                    <InsertParameters>
                        <asp:Parameter Name="ProjectNo" Type="String" />
                        <asp:Parameter Name="Year" Type="Int32" />
                        <asp:Parameter Name="Volume" Type="Int32" />
                    </InsertParameters>
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
                            &nbsp;<asp:Button ID="btnBuildApproval" runat="server" CausesValidation="False" Text="Build Approval List" />
                            <asp:Button ID="btnFwdApproval" runat="server" Text="Submit for Approval" Width="130px"
                                CausesValidation="true" ValidationGroup="ReSubmitApproval" />
                        </td>
                    </tr>
                </table>
                 <asp:ValidationSummary ID="vsReSubmit" runat="server" ValidationGroup="ReSubmitApproval"
                    ShowMessageBox="true" ShowSummary="true" />
            </asp:View>
            <asp:View ID="vsSupportingDocuments" runat="server">
                <asp:Label ID="lblSupDoc" runat="server" CssClass="p_smalltextbold" Style="width: 800px;
                    color: #990000" Text="This section is available as an option to include additional information. *.PDF, *.DOC, *.DOCX, *.XLS and *.XLSX files are allowed for upload up to 4MB each." /><br />
                <asp:Label ID="lblSupDoc2" runat="server" CssClass="p_smalltextbold" Style="width: 800px;
                    color: #990000" Text="NOTE: Please be sure to upload the latest copy of any document. Any changes you make will not be saved to the upload files. Please be sure to make a copy<br/> of the file locally and upload a new version. You have the option to delete or keep previous version of the file for reference. Please use the 'File Description'<br/> area to comment on the changes you make." />
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
                            File Description:
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
                            <asp:Button ID="btnReset6" runat="server" CausesValidation="False" Text="Reset" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="lblMessageView4" runat="server" Font-Bold="True" ForeColor="Red" Height="16px"
                                Text="Label" Visible="False" Width="368px" Font-Size="Small" />
                        </td>
                    </tr>
                </table>
                <asp:ValidationSummary ID="vsSupDoc" runat="server" ValidationGroup="vsSupportingDocuments"
                    ShowMessageBox="true" ShowSummary="true" />
                <br />
                <asp:GridView ID="gvSupportingDocument" runat="server" AutoGenerateColumns="False"
                    DataKeyNames="ProjectNo,DocID" DataSourceID="odsSupportingDocument" Width="900px"
                    SkinID="StandardGridWOFooter">
                    <Columns>
                        <asp:BoundField DataField="Description" HeaderText="File Description" SortExpression="Description"
                            ItemStyle-Width="500px" HeaderStyle-Width="500px" HeaderStyle-HorizontalAlign="Left">
                            <HeaderStyle HorizontalAlign="Left" Width="500px" />
                            <ItemStyle Width="500px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TeamMemberName" HeaderText="Uploaded By" SortExpression="TeamMemberName"
                            HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="150px" HeaderStyle-Width="150px">
                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                            <ItemStyle Width="150px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DateOfUpload" HeaderText="Upload Date" SortExpression="DateOfUpload"
                            HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100px" HeaderStyle-Width="100px">
                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="hlnkPreview" ImageUrl='<%# DisplayImage(DataBinder.Eval(Container, "DataItem.EncodeType").tostring)  %>'
                                    NavigateUrl='<%# "ToolingExpProjDocument.aspx?pProjNo=" & DataBinder.Eval (Container.DataItem,"ProjectNo").tostring & "&pDocID=" & DataBinder.Eval (Container.DataItem,"DocID").tostring %>'
                                    Target="_blank" Visible='<%# Bind("BinaryFound") %>' ToolTip="Preview Test Report" />
                            </ItemTemplate>
                            <HeaderStyle Width="30px" />
                            <ItemStyle HorizontalAlign="center" Width="30px" />
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                    ImageUrl="~/images/delete.jpg" AlternateText="Delete" />
                            </ItemTemplate>
                            <HeaderStyle Width="30px" />
                            <ItemStyle HorizontalAlign="Right" Width="30px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsSupportingDocument" runat="server" DeleteMethod="DeleteExpProjToolingDocuments"
                    OldValuesParameterFormatString="original_{0}" SelectMethod="GetExpProjToolingDocuments"
                    TypeName="ExpProjToolingBLL">
                    <DeleteParameters>
                        <asp:Parameter Name="ProjectNo" Type="String" />
                        <asp:Parameter Name="Original_DocID" Type="Int32" />
                        <asp:Parameter Name="Original_ProjectNo" Type="String" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="String" />
                        <asp:Parameter Name="DocID" Type="Int32" DefaultValue="0" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </asp:View>
            <asp:View ID="vwCommunicationBoard" runat="server">
                <asp:Label ID="lblSQC" runat="server" CssClass="p_smalltextbold" Style="width: 532px;
                    color: #990000" Text="Select a 'Question / Comment' from discussion thread below to respond." />
                <table>
                    <tr>
                        <td class="p_text" valign="top">
                            Question / Comment:
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
                            Reply / Comments:
                        </td>
                        <td>
                            <asp:TextBox ID="txtReply" runat="server" Rows="3" TextMode="MultiLine" Width="550px" />
                            <asp:RequiredFieldValidator ID="rfvReply" runat="server" ErrorMessage="Reply / Comments is a required field."
                                ValidationGroup="ReplyComments" ControlToValidate="txtReply"><</asp:RequiredFieldValidator><br />
                            <asp:Label ID="lblReply" runat="server" Font-Bold="True" ForeColor="Red" />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 26px">
                        </td>
                        <td style="height: 26px">
                            <asp:Button ID="btnSave2" runat="server" Text="Submit" CausesValidation="true" ValidationGroup="ReplyComments" />
                            <asp:Button ID="btnReset5" runat="server" Text="Reset" CausesValidation="False" />
                        </td>
                    </tr>
                </table>
                <asp:ValidationSummary ID="vsReplyComments" runat="server" ValidationGroup="ReplyComments"
                    ShowMessageBox="true" ShowSummary="true" />
                <br />
                <asp:GridView ID="gvQuestion" runat="server" AutoGenerateColumns="False" DataKeyNames="ProjectNo,RSSID"
                    DataSourceID="odsQuestion" OnRowDataBound="gvQuestion_RowDataBound" Width="900px"
                    RowStyle-BorderStyle="None" SkinID="CommBoardRSS">
                    <RowStyle BorderStyle="None" />
                    <Columns>
                        <asp:TemplateField HeaderText="Reply">
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkAppend" runat="server" ImageUrl="~/images/messanger30.jpg"
                                    NavigateUrl='<%# GoToCommunicationBoard(DataBinder.Eval(Container, "DataItem.ProjectNo"),DataBinder.Eval(Container, "DataItem.RSSID"),DataBinder.Eval(Container, "DataItem.ApprovalLevel"),DataBinder.Eval(Container, "DataItem.TeamMemberID")) %>'
                                    ToolTip="Reply" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="RSSID" HeaderText="RSSID" SortExpression="RSSID" Visible="false" />
                        <asp:BoundField DataField="Comments" HeaderText="Question / Comment" SortExpression="Comments"
                            HeaderStyle-Width="500px" ItemStyle-Width="500px" ItemStyle-Font-Bold="true"
                            ItemStyle-CssClass="c_text">
                            <HeaderStyle Width="500px" />
                            <ItemStyle CssClass="c_text" Font-Bold="True" Width="500px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TeamMemberName" HeaderText="Submitted By" SortExpression="TeamMemberName"
                            HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-Font-Bold="true">
                            <HeaderStyle Width="100px" />
                            <ItemStyle Font-Bold="True" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PostDate" HeaderText="Post Date" SortExpression="PostDate"
                            ItemStyle-Font-Bold="true">
                            <ItemStyle Font-Bold="True" />
                        </asp:BoundField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                    </td>
                                    <td colspan="3">
                                        <asp:GridView ID="gvReply" runat="server" AutoGenerateColumns="False" DataSourceID="odsReply"
                                            DataKeyNames="ProjectNo,RSSID" Width="100%" SkinID="CommBoardResponse">
                                            <Columns>
                                                <asp:BoundField DataField="Comments" HeaderText="Response" SortExpression="Comments"
                                                    HeaderStyle-Width="500px" ItemStyle-Width="500px" ItemStyle-Font-Bold="true" />
                                                <asp:BoundField DataField="TeamMemberName" HeaderText="" SortExpression="TeamMemberName"
                                                    HeaderStyle-Width="100px" ItemStyle-Width="100px" />
                                                <asp:BoundField DataField="PostDate" HeaderText="" SortExpression="PostDate" />
                                            </Columns>
                                        </asp:GridView>
                                        <asp:ObjectDataSource ID="odsReply" runat="server" OldValuesParameterFormatString="original_{0}"
                                            SelectMethod="GetExpProjToolingRSSReply" TypeName="ExpProjToolingBLL">
                                            <SelectParameters>
                                                <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="String" />
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
                    SelectMethod="GetExpProjToolingRSS" TypeName="ExpProjToolingBLL">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="String" />
                        <asp:Parameter Name="RSSID" Type="Int32" DefaultValue="0" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </asp:View>
        </asp:MultiView>
    </asp:Panel>
</asp:Content>
