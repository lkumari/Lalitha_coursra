<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="PackagingExpProj.aspx.vb" Inherits="EXP_PackagingExpProj" Title="Untitled Page"
    MaintainScrollPositionOnPostback="True" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server">
        <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
            Visible="False" CssClass="c_textbold" />
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
                <td class="p_text" style="width: 100px">
                    Project Number:
                </td>
                <td style="color: #990000; width: 100px;" class="c_text">
                    <asp:Label ID="lblProjectID" runat="server" Text="P0000" CssClass="c_text" Font-Bold="True"
                        Font-Overline="False" Font-Underline="False" />
                </td>
                <td class="p_text">
                    <asp:Label ID="Label3" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                    Project Title:
                </td>
                <td>
                    <asp:TextBox ID="txtProjectTitle" runat="server" MaxLength="50" Width="400px" />
                    <asp:RequiredFieldValidator ID="rfvProjTitle" runat="server" ControlToValidate="txtProjectTitle"
                        ErrorMessage="Project Title is a required field." Font-Bold="False" ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                    <asp:Label ID="lblProjectTitle" runat="server" Font-Bold="True" ForeColor="Red" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Project Status:
                </td>
                <td class="c_textbold" style="color: red;" colspan="3">
                    <asp:DropDownList ID="ddProjectStatus" runat="server" AutoPostBack="True">
                        <asp:ListItem Value="Open">New Project</asp:ListItem>
                        <asp:ListItem>Approved</asp:ListItem>
                        <asp:ListItem>Capitalized</asp:ListItem>
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
            <%--Display the following rows after 'P' is voided.--%>
            <tr>
                <td class="p_text" valign="top" style="height: 71px">
                    <asp:Label ID="lblReqVoidRsn" runat="server" Font-Bold="True" ForeColor="Red" Text="* "
                        Visible="false" />
                    <asp:Label ID="lblVoidRsn" runat="server" Text="Void Reason:" />
                </td>
                <td class="c_text" colspan="3" style="height: 71px">
                    <asp:TextBox ID="txtVoidReason" runat="server" MaxLength="300" Rows="3" TextMode="MultiLine"
                        Width="550px" /><br />
                    <asp:Label ID="lblVoidReason" runat="server" Font-Bold="True" ForeColor="Red" />
                </td>
            </tr>
        </table>
        <table width="100%" border="0">
            <tr>
                <td style="width: 30px">
                    <asp:Menu ID="mnuTabs" Height="30px" runat="server" Orientation="Horizontal" StaticEnableDefaultPopOutImage="false"
                        StaticMenuItemStyle-CssClass="tab" StaticSelectedStyle-CssClass="selectedTab"
                        CssClass="tabs">
                        <Items>
                            <asp:MenuItem Text="Project Detail" Value="0" ImageUrl="" Selected="true" />
                            <asp:MenuItem Text="Customer Info" Value="1" ImageUrl="" Selected="true" />
                            <asp:MenuItem Text="Packaging Expense" Value="2" ImageUrl="" />
                            <asp:MenuItem Text="Approval Status" Value="3" ImageUrl="" />
                            <asp:MenuItem Text="Communication Board" Value="4" ImageUrl="" />
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
                        <td class="c_text" style="color: #990000; width: 176px;">
                            <asp:Label ID="lblPrntProjNo" runat="server" Text="" />
                        </td>
                        <td class="p_text">
                            <asp:Label ID="Label26" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Originating Project Approved Date:
                        </td>
                        <td class="c_text" style="color: #990000; width: 209px;">
                            <asp:Label ID="lblPrntAppDate" runat="server" Text="" />
                        </td>
                    </tr>
                    <%End If%>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label4" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Project Leader:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddProjectLeader" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvProjectLeader" runat="server" ControlToValidate="ddProjectLeader"
                                ErrorMessage="Project Leader is a required field." Font-Bold="False" ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
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
                            <asp:Label ID="Label17" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Account Manager:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddAccountManager" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvAccountManager" runat="server" ControlToValidate="ddAccountManager"
                                ErrorMessage="Account Manager is a required field." Font-Bold="False" ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                        </td>
                        <td class="p_text" colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblUGNLocation" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            UGN Location(s):
                        </td>
                        <td colspan="3" class="c_text">
                            <asp:CheckBox ID="cbUT" runat="server" Text="Tinley Park, IL" />
                            &nbsp;
                            <asp:CheckBox ID="cbUN" runat="server" Text="Chicago Heights, IL" />
                            &nbsp;
                            <asp:CheckBox ID="cbUP" runat="server" Text="Jackson, TN" />
                            &nbsp;
                            <asp:CheckBox ID="cbUR" runat="server" Text="Somerset, KY" />
                            &nbsp;
                            <asp:CheckBox ID="cbUS" runat="server" Text="Valparaiso, IN" />
                            &nbsp;
                            <asp:CheckBox ID="cbOH" runat="server" Text="Monroe, OH" />
                            &nbsp;
                            <asp:CheckBox ID="cbUW" runat="server" Text="Silao, MX" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" valign="top">
                            <asp:Label ID="Label8" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Projected Date Notes:
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtProjDateNotes" runat="server" MaxLength="2000" Rows="12" TextMode="MultiLine"
                                Width="600px" />
                            <asp:RequiredFieldValidator ID="rfvProjDateNotes" runat="server" ControlToValidate="txtProjDateNotes"
                                ErrorMessage="Description is a required field." Font-Bold="False" ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator><br />
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
                            <asp:RangeValidator ID="rvAmtToBeRecovered" runat="server" ControlToValidate="txtAmtToBeRecovered"
                                Display="Dynamic" ErrorMessage="Amount to be Recovered requires a numeric value -999999.99 to 999999.99"
                                MaximumValue="999999.99" MinimumValue="-999999.99" Type="Double"><</asp:RangeValidator>
                            <asp:RequiredFieldValidator ID="rfvAmtToBeRecovered" runat="server" ControlToValidate="txtAmtToBeRecovered"
                                ErrorMessage="Amount to be Recovered is a required field." ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                            <ajax:FilteredTextBoxExtender ID="ftAmtToBeRec" runat="server" TargetControlID="txtAmtToBeRecovered"
                                FilterType="Custom, Numbers" ValidChars="-,." />
                        </td>
                        <%Else%>
                        <td style="width: 243px">
                            <asp:TextBox ID="txtNextAmtToBeRecovered" runat="server" Width="100px" MaxLength="16"
                                Text="0.00" AutoPostBack="true" />
                            <asp:RangeValidator ID="rvNextAmtToBeRecovered" runat="server" ControlToValidate="txtNextAmtToBeRecovered"
                                Display="Dynamic" ErrorMessage="Amount to be Recovered requires a numeric value -999999.99 to 999999.99"
                                MaximumValue="999999.99" MinimumValue="-999999.99" Type="Double"><</asp:RangeValidator>
                            <asp:RequiredFieldValidator ID="rfvNextAmtToBeRecovered" runat="server" ControlToValidate="txtNextAmtToBeRecovered"
                                ErrorMessage="Amount to be Recovered is a required field." ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                            <ajax:FilteredTextBoxExtender ID="ftbNextAmtToBeRecovered" runat="server" TargetControlID="txtNextAmtToBeRecovered"
                                FilterType="Custom, Numbers" ValidChars="-,." />
                        </td>
                        <% End If%>
                        <td class="p_text">
                            <asp:Label ID="Label27" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Memo at Program Awarded - Amount to be Recovered ($):
                        </td>
                        <td style="width: 243px">
                            <asp:TextBox ID="txtMPAAmtToBeRecovered" runat="server" Width="100px" MaxLength="16"
                                Text="0.00" />
                            <asp:RangeValidator ID="rvMPAAmtToBeRecovered" runat="server" ControlToValidate="txtMPAAmtToBeRecovered"
                                Display="Dynamic" ErrorMessage="Memo at Program Awarded - Amount to be Recovered requires a numeric value -999999.99 to 999999.99"
                                MaximumValue="999999.99" MinimumValue="-999999.99" Type="Double"><</asp:RangeValidator>
                            <asp:RequiredFieldValidator ID="rfvMPAAmtToBeRecovered" runat="server" ControlToValidate="txtMPAAmtToBeRecovered"
                                ErrorMessage="Memo at Program Awarded - Amount to be Recovered is a required field."
                                ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                            <ajax:FilteredTextBoxExtender ID="ftbeMPAAmtToBeRecovered" runat="server" TargetControlID="txtMPAAmtToBeRecovered"
                                FilterType="Custom, Numbers" ValidChars="-,." />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            UGN Total Cost ($):
                        </td>
                        <td class="c_textbold" style="color: #990000;">
                            <asp:Label ID="lblUGNTotalCost" runat="server" Text="0.00" />
                            <asp:TextBox ID="txtHDOrigUGNTotalCost" runat="server" Visible="False" 
                                Width="20px" />
                        </td>
                        <td class="p_text">
                            Memo at Program Awarded - Total Cost ($):
                        </td>
                        <td class="c_textbold" style="color: #990000;">
                            <asp:Label ID="lblMPATotalCost" runat="server" Text="0.00" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Customer Total Cost ($):
                        </td>
                        <td class="c_textbold" style="color: #990000;">
                            <asp:Label ID="lblCustTotalCost" runat="server" Text="0.00" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Variance ($):
                        </td>
                        <td class="c_textbold" style="color: #990000;">
                            <asp:Label ID="lblVarTotalCost" runat="server" Text="0.00" />
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
                            <asp:Label ID="Label9" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
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
                        <% Else%>
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
                            <asp:Label ID="Label12" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Estimated Start Spend Date:
                        </td>
                        <td>
                            <asp:TextBox ID="txtEstSpendDt" runat="server" Width="80px" MaxLength="10" />
                            <ajax:FilteredTextBoxExtender ID="ftbeEstSpendDt" runat="server" TargetControlID="txtEstSpendDt"
                                FilterType="Custom, Numbers" ValidChars="/" />
                            <asp:ImageButton runat="server" ID="imgEstSpendDt" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                                CausesValidation="False" />
                            <asp:RequiredFieldValidator ID="rfvEstSpendDt" runat="server" ControlToValidate="txtEstSpendDt"
                                ErrorMessage="Estimated Start Spend Date is a required field." ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revEstSpendDt" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                ControlToValidate="txtEstSpendDt" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                Width="8px"><</asp:RegularExpressionValidator>
                            <ajax:CalendarExtender ID="ceEstSpendDt" runat="server" TargetControlID="txtEstSpendDt"
                                Format="MM/dd/yyyy" PopupButtonID="imgEstSpendDt" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label7" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Estimated End Spend Date:
                        </td>
                        <td style="width: 209px">
                            <asp:TextBox ID="txtEstEndSpendDt" runat="server" Width="80px" MaxLength="10" />
                            <ajax:FilteredTextBoxExtender ID="ftbeEstEndSpendDt" runat="server" TargetControlID="txtEstEndSpendDt"
                                FilterType="Custom, Numbers" ValidChars="/" />
                            <asp:ImageButton runat="server" ID="imgEstEndSpendDt" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                                CausesValidation="False" />
                            <asp:RequiredFieldValidator ID="rfvEstEndSpendDt" runat="server" ControlToValidate="txtEstEndSpendDt"
                                ErrorMessage="Estimated End Spend Date is a required field." ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revEstEndSpendDt" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                ControlToValidate="txtEstEndSpendDt" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                Width="8px"><</asp:RegularExpressionValidator>
                            <ajax:CalendarExtender ID="ceEstEndSpendDt" runat="server" TargetControlID="txtEstEndSpendDt"
                                Format="MM/dd/yyyy" PopupButtonID="imgEstEndSpendDt" />
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
                    <%--Display the following rows after 'P' is closed.--%>
                    <% If (ViewState("ProjectStatus") = "Closed" Or ViewState("ProjectStatus") = "Capitalized") Then%>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblReqActualCost" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="* " Visible="false" />
                            <asp:Label ID="lblActualCost" runat="server" Text="Actual Cost ($):" />
                        </td>
                        <td class="c_text" style="width: 198px">
                            <asp:TextBox ID="txtActualCost" runat="server" Width="100px" />
                            <asp:RequiredFieldValidator ID="rfvActualCost" runat="server" ControlToValidate="txtActualCost"
                                ErrorMessage="Actual Cost is a required field." ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
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
                            <asp:RequiredFieldValidator ID="rfvCustomerCost" runat="server" ControlToValidate="txtCustomerCost"
                                ErrorMessage="Customer Cost is a required field." ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                            <ajax:FilteredTextBoxExtender ID="ftCCost" runat="server" TargetControlID="txtCustomerCost"
                                FilterType="Custom, Numbers" ValidChars="-." />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" valign="top" style="height: 57px">
                            <asp:Label ID="lblReqClosingNts" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="* " Visible="false" />
                            <asp:Label ID="lblClosingNts" runat="server" Text="Capitalized Notes:" />
                        </td>
                        <td class="c_text" colspan="3" style="height: 57px">
                            <asp:TextBox ID="txtClosingNotes" runat="server" MaxLength="300" Rows="3" TextMode="MultiLine"
                                Width="550px" />
                            <asp:RequiredFieldValidator ID="rfvClostingNotes" runat="server" ControlToValidate="txtClosingNotes"
                                ErrorMessage="Capitalized Notes is a required field." ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator><br />
                            <asp:Label ID="lblClosingNotes" runat="server" Font-Bold="True" ForeColor="Red" />&nbsp;
                        </td>
                    </tr>
                    <%End If%>
                    <% If (ViewState("ProjectStatus") <> "Open" Or ViewState("ProjectStatus") <> "Void" Or ViewState("ProjectStatus") <> Nothing) Then%>
                    <% If (txtAmtToBeRecovered.Text <> txtNextAmtToBeRecovered.Text) Then%>
                    <tr>
                        <td style="height: 53px" class="p_text">
                            <asp:Label ID="lblReqAmtToBeRecoveredChange" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="* " Visible="false" />
                            <asp:Label ID="lblAmtToBeRecoveredChange" runat="server" Text="Amt to be Rcvrd, Change Reason:"
                                Visible="false" ForeColor="Red" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtAmtToBeRecoveredChngRsn" Width="400px" runat="server" Visible="false" />
                            <asp:RequiredFieldValidator ID="rfvAmtToBeRecoveredChngRsn" runat="server" ErrorMessage="Amount to Be Recovered Change Reason is a required field."
                                ControlToValidate="txtAmtToBeRecoveredChngRsn" ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator><br />
                            <asp:Label ID="lblAmtToBeRecovered" runat="server" Font-Bold="True" ForeColor="Red" />
                        </td>
                    </tr>
                    <% End If%>
                    <% If (txtHDEstCmpltDt.Text <> txtNextEstCmpltDt.Text) Then%>
                    <tr>
                        <td style="height: 53px" class="p_text">
                            <asp:Label ID="lblReqEstCmpltDtChange" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="* " Visible="false" />
                            <asp:Label ID="lblEstCmpltDtChange" runat="server" Text=" Est Cmplt Date, Change Reason:"
                                Visible="false" ForeColor="Red" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtEstCmpltDtChngRsn" Width="400px" runat="server" Visible="false" />
                            <asp:RequiredFieldValidator ID="rfvEstCmpltDtChngRsn" runat="server" ErrorMessage="Estimated Completion Date Change Reason is a required field."
                                ControlToValidate="txtEstCmpltDtChngRsn" ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator><br />
                            <asp:Label ID="lblEstCmpltDt" runat="server" Font-Bold="True" ForeColor="Red" />
                        </td>
                        <% End If%>
                    </tr>
                    <% End If%>
                    <tr>
                        <td class="p_text" style="height: 17px">
                        </td>
                        <td colspan="3" style="height: 17px">
                            <asp:Button ID="btnSave1" runat="server" Text="Save" CausesValidation="True" ValidationGroup="vsProjectDetail" />
                            <asp:Button ID="btnReset1" runat="server" Text="Reset" CausesValidation="False" />
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CausesValidation="False" />
                            <asp:Button ID="btnPreview" runat="server" Text="Preview" CausesValidation="False" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="height: 26px">
                            <asp:ValidationSummary ID="sProjectDetail" ValidationGroup="vsProjectDetail" runat="server"
                                ShowMessageBox="True" />
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vwCustomerPart" runat="server">
                <asp:Panel ID="TCPanel" runat="server" CssClass="collapsePanelHeader" Width="600px">
                    <asp:Image ID="imgTC" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                        Height="12px" />&nbsp;
                    <asp:Label ID="lblTC" runat="server" CssClass="c_textbold" Text="CUSTOMER:" />
                </asp:Panel>
                <asp:Panel ID="TCContentPanel" runat="server" CssClass="collapsePanel" Width="100%">
                    <table width="80%">
                        <tr>
                            <td class="p_text" style="width: 130px">
                                Make:
                            </td>
                            <td style="font-size: smaller">
                                <asp:DropDownList ID="ddMake" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text" style="width: 130px">
                                Model:
                            </td>
                            <td style="font-size: smaller">
                                <asp:DropDownList ID="ddModel" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text" valign="top" style="width: 130px">
                                <asp:Label ID="Label5" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />Program:
                            </td>
                            <td style="font-size: smaller">
                                <asp:DropDownList ID="ddProgram" runat="server" AutoPostBack="True" />
                                <asp:RequiredFieldValidator ID="rfvProgram" runat="server" ControlToValidate="ddProgram"
                                    ErrorMessage="Program is a required field." Font-Bold="False" ValidationGroup="vsCustomer"><</asp:RequiredFieldValidator>
                                <asp:ImageButton ID="iBtnPreviewDetail" runat="server" ImageUrl="~/images/PreviewUp.jpg"
                                    ToolTip="Review Program Detail" Visible="false" />
                                <br />
                                {Program / Platform / Customer / Assembly Plant}
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
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
                        <tr>
                            <td class="p_text" style="height: 15px; width: 130px;">
                                Revision Level:
                            </td>
                            <td style="height: 15px">
                                <asp:TextBox ID="txtRevisionLvl" runat="server" MaxLength="25" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text" style="width: 130px">
                                Lead Time:
                            </td>
                            <td>
                                <asp:TextBox ID="txtLeadTime" runat="server" MaxLength="25" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text" style="width: 130px">
                                <asp:Label ID="lblSOP" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                Program SOP Date:
                            </td>
                            <td style="font-size: smaller">
                                <asp:TextBox ID="txtSOP" runat="server" MaxLength="12" Width="80px" />
                                <ajax:FilteredTextBoxExtender ID="ftbeSOP" runat="server" TargetControlID="txtSOP"
                                    FilterType="Custom, Numbers" ValidChars="/" />
                                <asp:ImageButton runat="server" ID="imgSOP" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                    AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                                    CausesValidation="False" />
                                <ajax:CalendarExtender ID="cbeSOP" runat="server" TargetControlID="txtSOP" PopupButtonID="imgSOP"
                                    Format="MM/dd/yyyy" />
                                <asp:RequiredFieldValidator ID="rfvSOP" runat="server" ControlToValidate="txtSOP"
                                    ErrorMessage="Program SOP Date is a required field." ValidationGroup="vsCustomer"><</asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revSOP" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                    ControlToValidate="txtSOP" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                    Width="8px" ValidationGroup="vsCustomer"><</asp:RegularExpressionValidator>
                                <asp:CompareValidator ID="cvSOP" runat="server" ErrorMessage="Program SOP Date must be less than Program EOP Date."
                                    ControlToCompare="txtEOP" ControlToValidate="txtSOP" Operator="LessThan" Type="Date"
                                    ValidationGroup="vsCustomer"><</asp:CompareValidator>&nbsp;{Defaulted by Program
                                Selection above}
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text" style="width: 130px">
                                <asp:Label ID="Label18" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                Program EOP Date:
                            </td>
                            <td style="font-size: smaller">
                                <asp:TextBox ID="txtEOP" runat="server" MaxLength="12" Width="80px" />
                                <ajax:FilteredTextBoxExtender ID="ftbeEOP" runat="server" TargetControlID="txtEOP"
                                    FilterType="Custom, Numbers" ValidChars="/" />
                                <asp:ImageButton runat="server" ID="imgEOP" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                    AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                                    CausesValidation="False" />
                                <ajax:CalendarExtender ID="cbeEOP" runat="server" TargetControlID="txtEOP" PopupButtonID="imgEOP"
                                    Format="MM/dd/yyyy" />
                                <asp:RequiredFieldValidator ID="rfvEOP" runat="server" ControlToValidate="txtEOP"
                                    ErrorMessage="Program EOP Date is a required field." ValidationGroup="vsCustomer"><</asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revEOP" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                    ControlToValidate="txtEOP" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                    Width="8px" ValidationGroup="vsCustomer"><</asp:RegularExpressionValidator>
                                <asp:CompareValidator ID="cvEOP" runat="server" ControlToCompare="txtSOP" ControlToValidate="txtEOP"
                                    ErrorMessage="Program EOP Date must be greater than Program SOP Date." Operator="GreaterThan"
                                    Type="Date" ValidationGroup="vsCustomer"><</asp:CompareValidator>&nbsp;{Defaulted
                                by Program Selection above}
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text" style="width: 130px">
                                <asp:Label ID="Label19" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                Packaging SOP Date:
                            </td>
                            <td>
                                <asp:TextBox ID="txtPPAPDt" runat="server" MaxLength="10" Width="80px" />
                                <ajax:FilteredTextBoxExtender ID="ftbePPAPDt" runat="server" TargetControlID="txtPPAPDt"
                                    FilterType="Custom, Numbers" ValidChars="/" />
                                <asp:ImageButton ID="imgPPAPDt" runat="server" AlternateText="Click to show calendar"
                                    CausesValidation="False" Height="19px" ImageAlign="Middle" ImageUrl="~/images/AJAX/Calendar_scheduleHS.png"
                                    Width="19px" />
                                <ajax:CalendarExtender ID="cbePPAP" runat="server" PopupButtonID="imgPPAPDt" TargetControlID="txtPPAPDt"
                                    Format="MM/dd/yyyy" />
                                <asp:RequiredFieldValidator ID="rfvPPAPDt" runat="server" ControlToValidate="txtPPAPDt"
                                    ErrorMessage="Estimated PPAP Date is a required field." ValidationGroup="vsCustomer"><</asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revPPAPDt" runat="server" ControlToValidate="txtPPAPDt"
                                    ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format ' Font-Bold="True"
                                    ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                    Width="8px" ValidationGroup="vsCustomer"><</asp:RegularExpressionValidator>
                                <asp:CompareValidator ID="cvPPAPDt" runat="server" ErrorMessage="Packaging SOP Date must be less than Program EOP Date."
                                    ControlToCompare="txtEOP" ControlToValidate="txtPPAPDt" Operator="LessThanEqual"
                                    Type="Date" ValidationGroup="vsCustomer"><</asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 26px; width: 130px;">
                            </td>
                            <td style="height: 26px">
                                <asp:Button ID="btnAddtoGrid1" runat="server" Text="Save" ToolTip="Add to grid."
                                    ValidationGroup="vsCustomer" />
                                <asp:Button ID="btnReset2" runat="server" Text="Reset" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Label ID="lblMessageView2" runat="server" Text="" Visible="false" Font-Size="Medium"
                        ForeColor="Red" />
                    <asp:ValidationSummary ID="vsCustomer" runat="server" Font-Size="X-Small" ShowMessageBox="True"
                        ShowSummary="true" ValidationGroup="vsCustomer" />
                    <ajax:CascadingDropDown ID="cddMake" runat="server" TargetControlID="ddMake" Category="Make"
                        PromptText="Select a Make..." LoadingText="[Loading Make(s)...]" ServicePath="~/WS/VehicleCDDService.asmx"
                        ServiceMethod="GetMakesSearch" />
                    <ajax:CascadingDropDown ID="cddModel" runat="server" TargetControlID="ddModel" Category="Model"
                        ParentControlID="ddMake" PromptText="Select a Model..." LoadingText="[Loading Model(s)...]"
                        ServicePath="~/WS/VehicleCDDService.asmx" ServiceMethod="GetModelSearch" />
                    <ajax:CascadingDropDown ID="cddProgram" runat="server" TargetControlID="ddProgram"
                        ParentControlID="ddModel" Category="Program" PromptText="Select a Program..."
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
                <asp:GridView ID="gvCustomer" runat="server" AutoGenerateColumns="False" DataKeyNames="PCID,ProjectNo,ProgramID,PartNo"
                    OnRowDataBound="gvCustomer_RowDataBound" DataSourceID="odsCustomer" CellPadding="4"
                    EmptyDataText="No data available for grid view. Use fields above to add new entry."
                    GridLines="Horizontal" Width="1015px" PageSize="100" SkinID="StandardGridWOFooter">
                    <Columns>
                        <asp:TemplateField HeaderText="Customer" SortExpression="ddCustomerDesc">
                            <ItemTemplate>
                                <% If ViewState("Admin") = "true" Then%>
                                <asp:HyperLink ID="lblCustDesc" runat="server" Font-Underline="true" Text='<%# Bind("ddCustomerDesc") %>'
                                    NavigateUrl='<%# "PackagingExpProj.aspx?pPCID=" & DataBinder.Eval (Container.DataItem,"PCID").tostring & "&pProjNo=" & ViewState("pProjNo")%>' />
                                <% Else%>
                                <asp:Label ID="HyperLink3" runat="server" Text='<%# Bind("ddCustomerDesc") %>' />
                                <% End If%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Program / Platform / Assembly" SortExpression="ProgramName">
                            <ItemTemplate>
                                <asp:HyperLink ID="lblPgmName" runat="server" ToolTip="Review Program Detail" Font-Underline="true"
                                    Text='<%# Bind("ProgramName") %>' Target="_blank" NavigateUrl='<%# GoToDetailPP(DataBinder.Eval (Container.DataItem,"ProgramID").tostring ) %>' />
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
                        <asp:BoundField DataField="PartDesc" HeaderText="Part Name" SortExpression="PartDesc"
                            ItemStyle-Wrap="false">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="RevisionLevel" HeaderText="Revision Level" SortExpression="RevisionLevel">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="LeadTime" HeaderText="Lead Time" SortExpression="LeadTime">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SOP" HeaderText="Program SOP" SortExpression="SOP">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="EOP" HeaderText="Program EOP" SortExpression="EOP">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PPAP" HeaderText="Packaging SOP" SortExpression="PPAP">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                    ToolTip="Delete" ImageUrl="~/images/delete.jpg" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsCustomer" runat="server" SelectMethod="GetExpProjPackagingCustomer"
                    OldValuesParameterFormatString="original_{0}" TypeName="ExpProjPackagingBLL"
                    DeleteMethod="DeleteExpProjPackagingCustomer" UpdateMethod="UpdateExpProjPackagingApproval">
                    <UpdateParameters>
                        <asp:Parameter Name="Status" Type="String" />
                        <asp:Parameter Name="Comments" Type="String" />
                        <asp:Parameter Name="original_ProjectNo" Type="String" />
                        <asp:Parameter Name="original_SeqNo" Type="Int32" />
                        <asp:Parameter Name="original_TeamMemberID" Type="Int32" />
                        <asp:Parameter Name="original_OrigTeamMemberID" Type="Int32" />
                        <asp:Parameter Name="DateNotified" Type="String" />
                    </UpdateParameters>
                    <SelectParameters>
                        <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="String" />
                        <asp:Parameter Name="PCID" Type="Int32" DefaultValue="0" />
                    </SelectParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="PCID" Type="Int32" />
                        <asp:Parameter Name="ProjectNo" Type="String" />
                        <asp:Parameter Name="ProgramID" Type="Int32" />
                        <asp:Parameter Name="PartNo" Type="String" />
                        <asp:Parameter Name="original_PCID" Type="Int32" />
                        <asp:Parameter Name="original_ProjectNo" Type="String" />
                        <asp:Parameter Name="original_ProgramID" Type="Int32" />
                        <asp:Parameter Name="original_PartNo" Type="String" />
                    </DeleteParameters>
                </asp:ObjectDataSource>
            </asp:View>
            <asp:View ID="vwExpense" runat="server">
                <asp:Panel ID="ExPanel" runat="server" CssClass="collapsePanelHeader" Width="496px">
                    <asp:Image ID="imgEX" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                        Height="12px" />&nbsp;
                    <asp:Label ID="lblEX" runat="server" Text="Label" CssClass="c_textbold">EXPENDITURE:</asp:Label>
                </asp:Panel>
                <asp:Panel ID="EXContentPanel" runat="server" CssClass="collapsePanel" Width="600px">
                    <table>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="Label22" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />Select:
                            </td>
                            <td class="c_text">
                                <asp:RadioButtonList ID="rblVendorStatus" runat="server" RepeatDirection="Horizontal"
                                    AutoPostBack="true">
                                    <asp:ListItem Value="1">New Supplier</asp:ListItem>
                                    <asp:ListItem Value="2">Existing Supplier</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="rfvVendorStatus" runat="server" ControlToValidate="rblVendorStatus"
                                    ErrorMessage="Select one: New Vendor or Existing Vendor." ValidationGroup="vsExpense"><</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="Label10" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                Vendor Type:
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddVendorType" runat="server" AppendDataBoundItems="TRUE" AutoPostBack="true"
                                    Enabled="false" />
                                &nbsp;<asp:RequiredFieldValidator ID="rfvVendorType" runat="server" ControlToValidate="ddVendorType"
                                    ErrorMessage="Vendor Type is a required field." Font-Bold="False" ValidationGroup="vsExpense"><</asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="Label6" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                Supplier:
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddVendor" runat="server" AutoPostBack="True" Enabled="false" />
                                &nbsp;<asp:RequiredFieldValidator ID="rfvVendor" runat="server" ControlToValidate="ddVendor"
                                    ErrorMessage="Supplier is a required field." Font-Bold="False" ValidationGroup="vsExpense"><</asp:RequiredFieldValidator><asp:TextBox
                                        ID="txtVendorName" runat="server" Visible="False" Width="40px" Wrap="False" />
                                <asp:TextBox ID="txtVTYPE" runat="server" Visible="False" Width="40px" Wrap="False" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text" valign="top">
                                <asp:Label ID="Label13" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                Description:
                            </td>
                            <td class="c_text" colspan="2">
                                <asp:TextBox ID="txtDescription" runat="server" MaxLength="50" Width="400px" />
                                <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription"
                                    ErrorMessage="Description is a required field." ValidationGroup="vsExpense"><</asp:RequiredFieldValidator><br />
                                <asp:Label ID="lblDescription" runat="server" Font-Bold="True" ForeColor="Red" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="Label23" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                UGN Location:
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddUGNLocation" runat="server" />
                                <asp:RequiredFieldValidator ID="rfvUGNLocation" runat="server" ControlToValidate="ddUGNLocation"
                                    ErrorMessage="UGN Location is a required field." Font-Bold="False" ValidationGroup="vsExpense"><</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="Label14" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                Quantity:
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtQuantity" runat="server" MaxLength="10" Width="80px" />
                                <ajax:FilteredTextBoxExtender ID="ftbeQuantity" runat="server" TargetControlID="txtQuantity"
                                    FilterType="Numbers" />
                                <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ControlToValidate="txtQuantity"
                                    ErrorMessage="Quantity is a required field." ValidationGroup="vsExpense"><</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="Label15" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                UGN Unit Cost ($):
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtUGNUnitCost" runat="server" MaxLength="20" Width="100px" />
                                <ajax:FilteredTextBoxExtender ID="ftbeUGNUnitCost" runat="server" TargetControlID="txtUGNUnitCost"
                                    FilterType="Custom, Numbers" ValidChars="-.," />
                                <asp:RequiredFieldValidator ID="rfvUGNUnitCost" runat="server" ControlToValidate="txtUGNUnitCost"
                                    ErrorMessage="UGN Unit Cost ($) is a required field." ValidationGroup="vsExpense"><</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="Label16" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                Customer Unit Cost ($):
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtCustUnitCost" runat="server" MaxLength="20" Width="100px" />
                                <ajax:FilteredTextBoxExtender ID="ftbCustUnitCost" runat="server" TargetControlID="txtCustUnitCost"
                                    FilterType="Custom, Numbers" ValidChars="-.," />
                                <asp:RequiredFieldValidator ID="rfvCustUnitCost" runat="server" ControlToValidate="txtCustUnitCost"
                                    ErrorMessage="Customer Unit Cost ($) is a required field." ValidationGroup="vsExpense"><</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="Label11" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                Memo at Program Awarded - Total Cost ($):
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtMPATotalCost" runat="server" MaxLength="20" Width="100px" />
                                <ajax:FilteredTextBoxExtender ID="ftbeMPATotalCost" runat="server" TargetControlID="txtMPATotalCost"
                                    FilterType="Custom, Numbers" ValidChars="-." />
                                <asp:RequiredFieldValidator ID="rfvMPATotalCost" runat="server" ControlToValidate="txtMPATotalCost"
                                    ErrorMessage="Memo at Program Awarded - Total Cost ($) is a required field."
                                    ValidationGroup="vsToolingExpense"><</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text" valign="top">
                                Notes:
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtNotes" runat="server" MaxLength="300" Rows="3" Width="400px"
                                    TextMode="MultiLine" /><br />
                                <asp:Label ID="lblNotes" runat="server" Font-Bold="True" ForeColor="Red" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td colspan="2">
                                <asp:Button ID="btnAddtoGrid2" runat="server" Text="Save" CommandName="AddtoGrid2"
                                    ToolTip="Add to grid." ValidationGroup="vsExpense" />
                                <asp:Button ID="btnReset3" runat="server" Text="Reset" CausesValidation="False" />
                            </td>
                        </tr>
                    </table>
                    <asp:ValidationSummary ID="vsExpense" runat="server" Font-Size="X-Small" ShowMessageBox="True"
                        ShowSummary="true" ValidationGroup="vsExpense" />
                </asp:Panel>
                <ajax:CollapsiblePanelExtender ID="EXExtender" runat="server" TargetControlID="EXContentPanel"
                    ExpandControlID="EXPanel" CollapseControlID="EXPanel" Collapsed="FALSE" TextLabelID="lblEX"
                    ExpandedText="EXPENDITURE:" CollapsedText="EXPENDITURE:" ImageControlID="imgEX"
                    CollapsedImage="~/images/expand_blue.jpg" ExpandedImage="~/images/collapse_blue.jpg"
                    SuppressPostBack="true">
                </ajax:CollapsiblePanelExtender>
                <br />
                <asp:GridView ID="gvExpense" runat="server" AutoGenerateColumns="False" DataKeyNames="EID,ProjectNo"
                    OnRowDataBound="gvExpense_RowDataBound" DataSourceID="odsExpense" CellPadding="4"
                    EmptyDataText="No data available for grid view. Use fields above to add new entry."
                    GridLines="Horizontal" Width="1100px" PageSize="100" SkinID="StandardGridWOFooter"
                    CssClass="c_smalltext">
                    <Columns>
                        <asp:BoundField DataField="EID" HeaderText="Line #" SortExpression="EID">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Description" SortExpression="Description">
                            <ItemTemplate>
                                <% If ViewState("Admin") = "true" Then%>
                                <asp:HyperLink ID="lblCustDesc" runat="server" Font-Underline="true" Text='<%# Bind("Description") %>'
                                    NavigateUrl='<%# "PackagingExpProj.aspx?pEID=" & DataBinder.Eval (Container.DataItem,"EID").tostring & "&pProjNo=" & ViewState("pProjNo")%>' />
                                <% Else%>
                                <asp:Label ID="lblDesc" runat="server" Text='<%# Bind("Description") %>' />
                                <% End If%>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" Width="200px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Vendor Type" SortExpression="VendorType">
                            <ItemTemplate>
                                <asp:Label ID="lblVType" runat="server" Text='<%# Bind("VendorType") %>' /></ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                            <ItemStyle Wrap="False" Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Vendor" SortExpression="VendorNameCombo">
                            <ItemTemplate>
                                <asp:Label ID="lblVend" runat="server" Text='<%# Bind("VendorNameCombo") %>' /></ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="UGN Location" SortExpression="UGNFacilityName" DataField="UGNFacilityName" />
                        <asp:BoundField DataField="Quantity" HeaderText="Quantity" SortExpression="Quantity">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UGNUnitCost" HeaderText="UGN Unit Cost" SortExpression="UGNUnitCost"
                            DataFormatString="{0:c}">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UGNTotalCost" HeaderText="UGN Total Cost" SortExpression="UGNTotalCost"
                            DataFormatString="{0:c}">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CustUnitCost" HeaderText="Customer Unit Cost" SortExpression="CustUnitCost"
                            DataFormatString="{0:c}">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CustTotalCost" HeaderText="Customer Total Cost" SortExpression="CustTotalCost"
                            DataFormatString="{0:c}">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Variance" HeaderText="Variance" SortExpression="Variance"
                            DataFormatString="{0:c}">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MPATotalCost" HeaderText="Memo at Program Awarded Total Cost"
                            SortExpression="MPATotalCost" DataFormatString="{0:c}">
                            <HeaderStyle HorizontalAlign="Right" Wrap="true" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="250px" Wrap="True" />
                        </asp:BoundField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                    ToolTip="Delete" ImageUrl="~/images/delete.jpg" /></ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsExpense" runat="server" DeleteMethod="DeleteExpProjPackagingExpenditure"
                    OldValuesParameterFormatString="original_{0}" SelectMethod="GetExpProjPackagingExpenditure"
                    TypeName="ExpProjPackagingBLL" UpdateMethod="UpdateExpProjPackagingApproval">
                    <DeleteParameters>
                        <asp:Parameter Name="ProjectNo" Type="String" />
                        <asp:Parameter Name="Original_EID" Type="Int32" />
                        <asp:Parameter Name="Original_ProjectNo" Type="String" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="Status" Type="String" />
                        <asp:Parameter Name="Comments" Type="String" />
                        <asp:Parameter Name="original_ProjectNo" Type="String" />
                        <asp:Parameter Name="original_SeqNo" Type="Int32" />
                        <asp:Parameter Name="original_TeamMemberID" Type="Int32" />
                        <asp:Parameter Name="original_OrigTeamMemberID" Type="Int32" />
                        <asp:Parameter Name="DateNotified" Type="String" />
                    </UpdateParameters>
                    <SelectParameters>
                        <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="String" />
                        <asp:Parameter Name="EID" Type="Int32" DefaultValue="0" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <hr />
                <br />
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
                                Line #:
                            </td>
                            <td class="c_text">
                                <asp:DropDownList ID="ddLineNo" runat="server" />
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
                                <asp:Button ID="btnReset4" runat="server" CausesValidation="False" Text="Reset" />
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
                </asp:Panel>
                <ajax:CollapsiblePanelExtender ID="SDExtender" runat="server" TargetControlID="SDContentPanel"
                    ExpandControlID="SDPanel" CollapseControlID="SDPanel" Collapsed="FALSE" TextLabelID="lblSD"
                    ExpandedText="SUPPORTING DOCUMENT(S):" CollapsedText="SUPPORTING DOCUMENT(S):"
                    ImageControlID="imgSD" CollapsedImage="~/images/expand_blue.jpg" ExpandedImage="~/images/collapse_blue.jpg"
                    SuppressPostBack="true">
                </ajax:CollapsiblePanelExtender>
                <br />
                <asp:GridView ID="gvSupportingDocument" runat="server" AutoGenerateColumns="False"
                    DataKeyNames="ProjectNo,DocID" DataSourceID="odsSupportingDocument" Width="900px"
                    SkinID="StandardGridWOFooter">
                    <Columns>
                        <asp:BoundField DataField="Description" HeaderText="File Description" SortExpression="Description">
                            <HeaderStyle HorizontalAlign="Left" Width="400px" />
                            <ItemStyle Width="400px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="EID" HeaderText="Line #" SortExpression="EID">
                            <HeaderStyle HorizontalAlign="Center" Width="40px" />
                            <ItemStyle Width="40px" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="comboUploadBy" HeaderText="Uploaded By" SortExpression="comboUploadBy">
                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                            <ItemStyle Width="150px" />
                        </asp:BoundField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="hlnkPreview" ImageUrl='<%# DisplayImage(DataBinder.Eval(Container, "DataItem.EncodeType").tostring)  %>'
                                    NavigateUrl='<%# "PackagingExpProjDocument.aspx?pProjNo=" & DataBinder.Eval (Container.DataItem,"ProjectNo").tostring & "&pDocID=" & DataBinder.Eval (Container.DataItem,"DocID").tostring %>'
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
                <asp:ObjectDataSource ID="odsSupportingDocument" runat="server" DeleteMethod="DeleteExpProjPackagingDocuments"
                    OldValuesParameterFormatString="original_{0}" SelectMethod="GetExpProjPackagingDocuments"
                    TypeName="ExpProjPackagingBLL">
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
                <br />
                <hr />
                <br />
                <table>
                    <tr>
                        <td colspan="2" class="c_textbold">
                            JUSTIFICATION:
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Not Required:
                        </td>
                        <td>
                            <asp:CheckBox ID="cbNotRequired" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Return to Average Assets (%):
                        </td>
                        <td>
                            <asp:TextBox ID="txtRtnAvgAssets" runat="server" Width="60px" MaxLength="3" Text="0" />
                            <ajax:FilteredTextBoxExtender ID="ftbRtnAvgAssets" runat="server" TargetControlID="txtRtnAvgAssets"
                                FilterType="Numbers" />
                            <asp:RangeValidator ID="rvRtnAvgAssets" runat="server" ControlToValidate="txtRtnAvgAssets"
                                Display="Dynamic" ErrorMessage="Return to Average Assets requires a numeric value 0 to 100"
                                Height="16px" MaximumValue="100" MinimumValue="0" Type="Integer" ValidationGroup="vsOtherExp"><</asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Discounted Return (%):
                        </td>
                        <td>
                            <asp:TextBox ID="txtDiscountReturn" runat="server" Width="60px" MaxLength="3" Text="0" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Payback in Years:
                        </td>
                        <td>
                            <asp:TextBox ID="txtPayback" runat="server" Width="60px" MaxLength="3" Text="0" />
                            <ajax:FilteredTextBoxExtender ID="ftbPayback" runat="server" TargetControlID="txtPayback"
                                FilterType="Custom, Numbers" ValidChars="-." />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnSave3" runat="server" Text="Save" CausesValidation="true" ValidationGroup="vsOtherExp" />
                            <asp:Button ID="btnReset5" runat="server" Text="Reset" />
                        </td>
                    </tr>
                </table>
                <asp:ValidationSummary ID="vsOtherExp" runat="server" Font-Size="X-Small" ShowMessageBox="True"
                    ShowSummary="true" ValidationGroup="vsOtherExp" />
            </asp:View>
            <asp:View ID="vwApprovalStatus" runat="server">
                <br />
                <asp:Label ID="lblReqAppComments" runat="server" Visible="false" CssClass="c_text"
                    Font-Bold="true" />
                <asp:GridView ID="gvApprovers" runat="server" AutoGenerateColumns="False" DataKeyNames="ProjectNo,SeqNo,OrigTeamMemberID,TeamMemberID"
                    OnRowUpdating="gvApprovers_RowUpdating" OnRowDataBound="gvApprovers_RowDataBound"
                    DataSourceID="odsApprovers" Width="1000px" RowStyle-Height="20px" RowStyle-CssClass="c_text"
                    HeaderStyle-CssClass="c_text" SkinID="StandardGridWOFooter">
                    <RowStyle CssClass="c_text" Height="20px" />
                    <Columns>
                        <asp:BoundField DataField="SeqNo" HeaderText="Approval Level" ReadOnly="True" SortExpression="SeqNo">
                            <HeaderStyle HorizontalAlign="Center" Width="50px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
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
                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>' /></ItemTemplate>
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
                                    Text='<%# Bind("Comments") %>' Width="300px" /><asp:RequiredFieldValidator ID="rfvComments"
                                        runat="server" ControlToValidate="txtAppComments" ErrorMessage="Comments is a required field when approving for another team member."
                                        Font-Bold="True" ValidationGroup="EditApprovalInfo"><</asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtTeamMemberID" runat="server" Text='<%# Eval("TeamMemberID") %>'
                                    ReadOnly="true" Width="0px" Visible="false" />
                                <asp:TextBox ID="txtOrigTeamMemberID" runat="server" Text='<%# Eval("OrigTeamMemberID") %>'
                                    ReadOnly="true" Width="0px" Visible="false" />
                                <asp:TextBox ID="hfSeqNo" runat="server" Text='<%# Eval("SeqNo") %>' ReadOnly="true"
                                    Width="0px" Visible="false" /></EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Comments") %>' /></ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="400px" />
                            <ItemStyle Wrap="True" />
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <EditItemTemplate>
                                <asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                    ImageUrl="~/images/save.jpg" ToolTip="Save" ValidationGroup="EditPriceInfo" />&nbsp;&nbsp;&nbsp;
                                <asp:ImageButton ID="ibtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                    ImageUrl="~/images/cancel.jpg" ToolTip="Cancel" ValidationGroup="EditPriceInfo" /></EditItemTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                    ToolTip="Edit" ImageUrl="~/images/edit.jpg" ValidationGroup="EditPriceInfo" />&nbsp;&nbsp;&nbsp;</ItemTemplate>
                            <ItemStyle Width="60px" />
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                    ToolTip="Delete" ImageUrl="~/images/delete.jpg" /></ItemTemplate>
                            <ItemStyle Width="30px" />
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="c_text" />
                </asp:GridView>
                <asp:ValidationSummary ID="vsEditApprovalInfo" runat="server" ShowMessageBox="True"
                    ValidationGroup="EditApprovalInfo" />
                <asp:ObjectDataSource ID="odsApprovers" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetExpProjPackagingApproval" TypeName="ExpProjPackagingBLL" UpdateMethod="UpdateExpProjPackagingApproval"
                    DeleteMethod="DeleteExpProjPackagingApproval">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="String" />
                        <asp:Parameter DefaultValue="0" Name="Sequence" Type="Int32" />
                    </SelectParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="Status" Type="String" />
                        <asp:Parameter Name="Comments" Type="String" />
                        <asp:Parameter Name="original_ProjectNo" Type="String" />
                        <asp:Parameter Name="original_SeqNo" Type="Int32" />
                        <asp:Parameter Name="original_TeamMemberID" Type="Int32" />
                        <asp:Parameter Name="original_OrigTeamMemberID" Type="Int32" />
                        <asp:Parameter Name="DateNotified" Type="String" />
                    </UpdateParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="ProjectNo" Type="String" />
                        <asp:Parameter Name="SeqNo" Type="Int32" />
                        <asp:Parameter Name="ResponsibleTMID" Type="Int32" />
                        <asp:Parameter Name="original_ProjectNo" Type="String" />
                        <asp:Parameter Name="original_SeqNo" Type="Int32" />
                        <asp:Parameter Name="original_TeamMemberID" Type="Int32" />
                        <asp:Parameter Name="original_OrigTeamMemberID" Type="Int32" />
                    </DeleteParameters>
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
                            <asp:Button ID="btnBuildApproval" runat="server" CausesValidation="False" Text="Build Approval List" />
                            <asp:Button ID="btnFwdApproval" runat="server" Text="Submit for Approval" Width="130px"
                                CausesValidation="true" ValidationGroup="ReSubmitApproval" />
                        </td>
                    </tr>
                </table>
                <asp:ValidationSummary ID="vsReSubmit" runat="server" ValidationGroup="ReSubmitApproval"
                    ShowMessageBox="true" ShowSummary="true" />
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
                            <asp:Button ID="btnReset6" runat="server" Text="Reset" CausesValidation="False" />
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
                                <%-- <% If ViewState("Admin") = "true" Then%>--%>
                                <asp:HyperLink ID="lnkAppend" runat="server" ImageUrl="~/images/messanger30.jpg"
                                    ToolTip="Reply" NavigateUrl='<%# GoToCommunicationBoard(DataBinder.Eval(Container, "DataItem.ProjectNo"),DataBinder.Eval(Container, "DataItem.RSSID"),DataBinder.Eval(Container, "DataItem.ApprovalLevel"),DataBinder.Eval(Container, "DataItem.TeamMemberID")) %>' />
                                <%-- <%Else%>
                                <asp:HyperLink ID="HyperLink1" runat="server" ImageUrl="~/images/messanger30.jpg" />
                                <% End If%>--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="RSSID" HeaderText="RSSID" SortExpression="RSSID" Visible="False" />
                        <asp:BoundField DataField="Comments" HeaderText="Question / Comment" SortExpression="Comments">
                            <HeaderStyle Width="500px" />
                            <ItemStyle CssClass="c_text" Font-Bold="True" Width="500px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TeamMemberName" HeaderText="Submitted By" SortExpression="TeamMemberName">
                            <HeaderStyle Width="100px" />
                            <ItemStyle Font-Bold="True" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PostDate" HeaderText="Post Date" SortExpression="PostDate">
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
                                                    HeaderStyle-Width="500px" ItemStyle-Width="500px" ItemStyle-Font-Bold="true">
                                                    <HeaderStyle Width="500px" />
                                                    <ItemStyle Font-Bold="True" Width="500px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TeamMemberName" HeaderText="" SortExpression="TeamMemberName"
                                                    HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                                    <HeaderStyle Width="100px" />
                                                    <ItemStyle Width="100px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PostDate" HeaderText="" SortExpression="PostDate" />
                                            </Columns>
                                        </asp:GridView>
                                        <asp:ObjectDataSource ID="odsReply" runat="server" OldValuesParameterFormatString="original_{0}"
                                            SelectMethod="GetExpProjPackagingRSSReply" TypeName="ExpProjPackagingBLL">
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
                    SelectMethod="GetExpProjPackagingRSS" TypeName="ExpProjPackagingBLL">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="String" />
                        <asp:Parameter Name="RSSID" Type="Int32" DefaultValue="0" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </asp:View>
        </asp:MultiView>
    </asp:Panel>
</asp:Content>
