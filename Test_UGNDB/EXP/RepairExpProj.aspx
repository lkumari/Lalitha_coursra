<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="RepairExpProj.aspx.vb" Inherits="EXP_RepairExpProj" Title="Untitled Page"
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
                <td style="color: #990000; width: 100px;">
                    <asp:Label ID="lblProjectID" runat="server" Text="R0000" CssClass="c_text" Font-Bold="True"
                        Font-Overline="False" Font-Underline="False" />
                </td>
                <td class="p_text">
                    <asp:Label ID="Label3" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                    Project Title:
                </td>
                <td>
                    <asp:TextBox ID="txtProjectTitle" runat="server" MaxLength="50" Width="400px" />&nbsp;
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
                        <asp:ListItem>Completed</asp:ListItem>
                        <asp:ListItem>Void</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddProjectStatus2" runat="server" AutoPostBack="True">
                        <asp:ListItem>Hold</asp:ListItem>
                        <asp:ListItem>In Process</asp:ListItem>
                        <asp:ListItem>Void</asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="txtRoutingStatus" Visible="false" runat="server" Width="1px" />
                    <asp:Label ID="lblRoutingStatusDesc" runat="server" Visible="False" Width="312px" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                </td>
                <td colspan="3" class="c_text">
                    <asp:CheckBox ID="cbProjectInLatestForecast" runat="server" />Project is in Latest
                    Weekly Financial Forecast.
                </td>
            </tr>
            <%--Display the following rows after 'R' is voided.--%>
            <tr>
                <td class="p_text" valign="top" style="height: 71px">
                    <asp:Label ID="lblReqVoidRsn" runat="server" Font-Bold="True" ForeColor="Red" Text="* "
                        Visible="false" />
                    <asp:Label ID="lblVoidRsn" runat="server" Text="Void Reason:" />
                </td>
                <td class="c_text" colspan="3" style="height: 71px">
                    <asp:TextBox ID="txtVoidReason" runat="server" MaxLength="300" Rows="3" TextMode="MultiLine"
                        Width="550px" />
                    <asp:RequiredFieldValidator ID="rfvVoidReason" runat="server" ErrorMessage="Void Reason is a required field."
                        ControlToValidate="txtVoidReason" ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator><br />
                    <asp:Label ID="lblVoidReason" runat="server" Font-Bold="True" ForeColor="Red" />
                </td>
            </tr>
        </table>
        <ajax:CascadingDropDown ID="cddUGNLocation" runat="server" TargetControlID="ddUGNLocation"
            Category="UGNLocation" PromptText=" " LoadingText="[Loading UGN Location(s)...]"
            ServicePath="~/WS/GeneralCDDService.asmx" ServiceMethod="GetUGNLocationByTMFac" />
        <ajax:CascadingDropDown ID="cddDepartment" runat="server" TargetControlID="ddDepartment"
            ParentControlID="ddUGNLocation" Category="DeptGLNo" PromptText=" " LoadingText="[Loading Department or Cost Centers...]"
            ServicePath="~/WS/GeneralCDDService.asmx" ServiceMethod="GetDepartmentGLNO" />
        <table border="0">
            <tr>
                <td>
                    <asp:Menu ID="mnuTabs" Height="30px" runat="server" Orientation="Horizontal" StaticEnableDefaultPopOutImage="false"
                        StaticMenuItemStyle-CssClass="tab" StaticSelectedStyle-CssClass="selectedTab"
                        CssClass="tabs">
                        <Items>
                            <asp:MenuItem Text="Project Detail" Value="0" ImageUrl="" Selected="true" />
                            <asp:MenuItem Text="Repair Expense" Value="1" ImageUrl="" />
                            <asp:MenuItem Text="Approval Status" Value="2" ImageUrl="" />
                            <asp:MenuItem Text="Communication Board" Value="3" ImageUrl="" />
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
                                CausesValidation="False" /><asp:RequiredFieldValidator ID="rfvDateSubmitted" runat="server"
                                    ControlToValidate="txtDateSubmitted" ErrorMessage="Date Submitted is a required field."
                                    ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revDateSubmitted" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                ControlToValidate="txtDateSubmitted" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                Width="8px"><</asp:RegularExpressionValidator>
                            <ajax:CalendarExtender ID="ceDateSub" runat="server" TargetControlID="txtDateSubmitted"
                                Format="MM/dd/yyyy" PopupButtonID="imgDateSubmitted" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblSampleProdDesc" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="* " />
                            UGN Location:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddUGNLocation" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvUGNFacility" runat="server" ControlToValidate="ddUGNLocation"
                                ErrorMessage="UGN Location is a required field." Font-Bold="False" ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                        </td>
                        <td class="p_text">
                            <asp:Label ID="Label6" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Department or Cost Center:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddDepartment" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvDCC" runat="server" ControlToValidate="ddDepartment"
                                ErrorMessage="Department or Cost Center is a required field." Font-Bold="False"
                                ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" valign="top">
                            <asp:Label ID="Label8" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Description:
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtProjDateNotes" runat="server" MaxLength="2000" Rows="8" TextMode="MultiLine"
                                Width="600px" />
                            <asp:RequiredFieldValidator ID="rfvProjDateNotes" runat="server" ControlToValidate="txtProjDateNotes"
                                ErrorMessage="Description is a required field." Font-Bold="False" ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator><br />
                            <asp:Label ID="lblProjDateNotes" runat="server" Font-Bold="True" ForeColor="Red" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" valign="top">
                            <asp:Label ID="Label10" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Justification/Analysis:
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtJustification" runat="server" MaxLength="2000" Rows="8" TextMode="MultiLine"
                                Width="600px" />
                            <asp:RequiredFieldValidator ID="rfvJustification" runat="server" ControlToValidate="txtJustification"
                                ErrorMessage="Justification/Analysis is a required field." Font-Bold="False"
                                ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator><br />
                            <asp:Label ID="lblJustification" runat="server" Font-Bold="True" ForeColor="Red" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Total Investment ($):
                        </td>
                        <td class="c_textbold" style="color: #990000;">
                            <asp:Label ID="lblTotalInvestment" runat="server" Text="0.00" />
                        </td>
                        <% If ViewState("pProjNo") = Nothing Then%>
                        <td class="p_text">
                            <asp:TextBox ID="txtHDEstCmpltDt" runat="server" Visible="False" Width="2px" />
                            <asp:Label ID="Label9" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Estimated Completion Date:
                        </td>
                        <td class="c_text" colspan="2">
                            <asp:TextBox ID="txtEstCmpltDt" runat="server" Width="80px" MaxLength="10" />
                            <ajax:FilteredTextBoxExtender ID="ftbeEstCmpltDt" runat="server" TargetControlID="txtEstCmpltDt"
                                FilterType="Custom, Numbers" ValidChars="/" />
                            <asp:ImageButton runat="server" ID="imgEstCmpltDt" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                                CausesValidation="False" />
                            <asp:RequiredFieldValidator ID="rfvEstCmpltDt" runat="server" ControlToValidate="txtEstCmpltDt"
                                ErrorMessage="Estimated Completion Date 1 is a required field." ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revEstCmpltDt" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                ControlToValidate="txtEstCmpltDt" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                Width="8px" ValidationGroup="vsProjectDetail"><</asp:RegularExpressionValidator>
                            <ajax:CalendarExtender ID="ceEstCmpltDt" runat="server" TargetControlID="txtEstCmpltDt"
                                Format="MM/dd/yyyy" PopupButtonID="imgEstCmpltDt" />
                        </td>
                        <% Else%>
                        <td class="p_text">
                            <asp:Label ID="Label11" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Estimated Completion Date:
                        </td>
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
                                ControlToCompare="txtEstCmpltDt" ErrorMessage="Next Estimated Completion Date must be greater than the Original Estimated Completion Date."
                                Operator="GreaterThan" Type="Date" ValidationGroup="vsProjectDetail"><</asp:CompareValidator>
                        </td>
                    </tr>
                    <% End If%>
                    <% If (ViewState("pProjNo") <> Nothing) And (txtHDEstCmpltDt.Text <> txtNextEstCmpltDt.Text) And (ViewState("ProjectStatus") <> "Open") Then%>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lblReqEstCmpltDtChange" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="* " Visible="false" />
                            <asp:Label ID="lblEstCmpltDtChange" runat="server" Text="Change Reason:" Visible="false"
                                ForeColor="Red" />
                            <asp:TextBox ID="txtEstCmpltDtChngRsn" Width="400px" runat="server" Visible="false" />
                            <asp:RequiredFieldValidator ID="rfvEstCmpltDtChngRsn" runat="server" ErrorMessage="Estimated Completion Date Change Reason is a required field."
                                ControlToValidate="txtEstCmpltDtChngRsn" ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator><br />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label
                                ID="lblEstCmpltDt" runat="server" Font-Bold="True" ForeColor="Red" />
                        </td>
                    </tr>
                    <% End If%>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblReqActualCost" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="* " Visible="false" />
                            <asp:Label ID="lblActualCost" runat="server" Text="Actual Cost ($):" />
                        </td>
                        <td class="c_text">
                            <asp:TextBox ID="txtActualCost" runat="server" Width="100px" />
                            <ajax:FilteredTextBoxExtender ID="ftActualCost" runat="server" TargetControlID="txtActualCost"
                                FilterType="Custom, Numbers" ValidChars="-." />
                            <asp:RequiredFieldValidator ID="rfvActualCost" runat="server" ControlToValidate="txtActualCost"
                                ErrorMessage="Actual Cost is a required field." ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator><br />
                        </td>
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
                            <asp:Label ID="lblReqCustomerCost" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="* " Visible="false" />
                            <asp:Label ID="lblCustomerCost" runat="server" Text="Customer Cost ($):" />
                        </td>
                        <td class="c_text">
                            <asp:TextBox ID="txtCustomerCost" runat="server" Width="100px" />
                            <ajax:FilteredTextBoxExtender ID="ftCCost" runat="server" TargetControlID="txtCustomerCost"
                                FilterType="Custom, Numbers" ValidChars="-." />
                            <asp:RequiredFieldValidator ID="rfvCustomerCost" runat="server" ControlToValidate="txtCustomerCost"
                                ErrorMessage="Customer Cost is a required field." ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                        </td>
                        <td class="p_text">
                            <asp:Label ID="Label7" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Estimated End Spend Date:
                        </td>
                        <td>
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
                    </tr>
                    <tr>
                        <td class="p_text" valign="top">
                            <asp:Label ID="lblReqClosingNts" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="* " Visible="false" />
                            <asp:Label ID="lblClosingNts" runat="server" Text="Capitalized Notes:" />
                        </td>
                        <td class="c_text" colspan="3">
                            <asp:TextBox ID="txtClosingNotes" runat="server" MaxLength="300" Rows="3" TextMode="MultiLine"
                                Width="550px" />
                            <asp:RequiredFieldValidator ID="rfvClostingNotes" runat="server" ControlToValidate="txtClosingNotes"
                                ErrorMessage="Capitalized Notes is a required field." ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator><br />
                            <asp:Label ID="lblClosingNotes" runat="server" Font-Bold="True" ForeColor="Red" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                        </td>
                        <td colspan="3">
                            <asp:Button ID="btnSave1" runat="server" Text="Save" CausesValidation="True" ValidationGroup="vsProjectDetail" />
                            <asp:Button ID="btnReset1" runat="server" Text="Reset" CausesValidation="False" />
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CausesValidation="False" />
                            <asp:Button ID="btnPreview" runat="server" Text="Preview" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:ValidationSummary ID="sProjectDetail" ValidationGroup="vsProjectDetail" runat="server"
                                ShowMessageBox="True" />
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vwAssetExpense" runat="server">
                <asp:Panel ID="AEPanel" runat="server" CssClass="collapsePanelHeader" Width="496px">
                    <asp:Image ID="imgAE" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                        Height="12px" />&nbsp;
                    <asp:Label ID="lblAE" runat="server" Text="Label" CssClass="c_textbold">EXPENDITURE:</asp:Label>
                </asp:Panel>
                <asp:Panel ID="AEContentPanel" runat="server" CssClass="collapsePanel" Width="600px">
                    <table>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="Label13" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                Description:
                            </td>
                            <td>
                                <asp:TextBox ID="txtDescription" runat="server" MaxLength="50" Width="400px" />
                                <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription"
                                    ErrorMessage="Description is a required field." ValidationGroup="vsRepairExpense"><</asp:RequiredFieldValidator><br />
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
                                    ErrorMessage="Quantity is a required field." ValidationGroup="vsRepairExpense"><</asp:RequiredFieldValidator>
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
                                    FilterType="Custom, Numbers" ValidChars="-." />
                                <asp:RequiredFieldValidator ID="rfvAmountPer" runat="server" ControlToValidate="txtAmountPer"
                                    ErrorMessage="Amount Per ($) is a required field." ValidationGroup="vsRepairExpense"><</asp:RequiredFieldValidator>
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
                                    ToolTip="Add to grid." ValidationGroup="vsRepairExpense" />
                                <asp:Button ID="btnReset4" runat="server" Text="Reset" CausesValidation="False" />
                            </td>
                        </tr>
                    </table>
                    <asp:ValidationSummary ID="vsRepairExpense" runat="server" Font-Size="X-Small" ShowMessageBox="True"
                        ShowSummary="true" ValidationGroup="vsRepairExpense" />
                </asp:Panel>
                <ajax:CollapsiblePanelExtender ID="AEExtender" runat="server" TargetControlID="AEContentPanel"
                    ExpandControlID="AEPanel" CollapseControlID="AEPanel" Collapsed="FALSE" TextLabelID="lblAE"
                    ExpandedText="EXPENDITURE:" CollapsedText="EXPENDITURE:" ImageControlID="imgAE"
                    CollapsedImage="~/images/expand_blue.jpg" ExpandedImage="~/images/collapse_blue.jpg"
                    SuppressPostBack="true">
                </ajax:CollapsiblePanelExtender>
                <br />
                <asp:GridView ID="gvExpense" runat="server" AutoGenerateColumns="False" DataKeyNames="EID,ProjectNo"
                    OnRowDataBound="gvExpense_RowDataBound" DataSourceID="odsExpense"
                    CellPadding="4" EmptyDataText="No data available for grid view. Use fields above to add new entry."
                    GridLines="Horizontal" Width="850px" PageSize="100" SkinID="StandardGridWOFooter">
                    <Columns>
                        <asp:BoundField DataField="EID" HeaderText="Line #" SortExpression="EID" ItemStyle-HorizontalAlign="center"
                            ItemStyle-Width="30px">
                            <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Description" SortExpression="Description">
                            <ItemTemplate>
                                <% If ViewState("Admin") = "true" Then%>
                                <asp:HyperLink ID="lblCategory" runat="server" Font-Underline="true" Text='<%# Bind("Description") %>'
                                    NavigateUrl='<%# "RepairExpProj.aspx?pEID=" & DataBinder.Eval (Container.DataItem,"EID").tostring & "&pProjNo=" & ViewState("pProjNo")%>' />
                                <% Else%>
                                <asp:Label ID="lblCategory1" runat="server" Text='<%# Bind("Description") %>' />
                                <% End If%>
                            </ItemTemplate>
                            <ItemStyle Width="200px" Wrap="True" />
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
                        <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="250px" Wrap="True" />
                        </asp:BoundField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                    ToolTip="Delete" ImageUrl="~/images/delete.jpg" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsExpense" runat="server" DeleteMethod="DeleteExpProjRepairExpenditure"
                    OldValuesParameterFormatString="original_{0}" SelectMethod="GetExpProjRepairExpenditure"
                    TypeName="ExpProjRepairBLL">
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
                                    Font-Size="Small" /><br />
                                <asp:Label ID="lblMessageView4" runat="server" Font-Bold="True" ForeColor="Red" Height="16px"
                                    Text="Label" Visible="False" Width="368px" Font-Size="Small" />
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
                                    NavigateUrl='<%# "RepairExpProjDocument.aspx?pProjNo=" & DataBinder.Eval (Container.DataItem,"ProjectNo").tostring & "&pDocID=" & DataBinder.Eval (Container.DataItem,"DocID").tostring %>'
                                    Target="_blank" Visible='<%# Bind("BinaryFound") %>' ToolTip="Preview Document" />
                            </ItemTemplate>
                            <HeaderStyle Width="30px" />
                            <ItemStyle HorizontalAlign="Center" Width="30px" />
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
                <asp:ObjectDataSource ID="odsSupportingDocument" runat="server" DeleteMethod="DeleteExpProjRepairDocuments"
                    OldValuesParameterFormatString="original_{0}" SelectMethod="GetExpProjRepairDocuments"
                    TypeName="ExpProjRepairBLL">
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
                        <td class="p_textbold">
                            Sub Total Expenditures ($):
                        </td>
                        <td class="c_text" style="width: 243px; color: #990000;">
                            <b>
                                <asp:Label ID="lblSubtotalRepair" runat="server" Text="0.00" /></b>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            LESS - Retired Equipment Value ($):
                        </td>
                        <td>
                            <asp:TextBox ID="txtLessRtrdEqVal" runat="server" Width="100px" MaxLength="12">0.00</asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="ftbLessRtrdEqVal" runat="server" TargetControlID="txtLessRtrdEqVal"
                                FilterType="Custom, Numbers" ValidChars="-." />
                            <asp:RangeValidator ID="rvLessRtrdEqVal" runat="server" ControlToValidate="txtLessRtrdEqVal"
                                Display="Dynamic" ErrorMessage="Less - Retired Equipment Value requires a numeric value -999999999.99 to 999999999.99"
                                Height="16px" MaximumValue="999999999.99" MinimumValue="-999999999.99" Type="Double"
                                ValidationGroup="vsOtherExp"><</asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Working Capital ($):
                        </td>
                        <td>
                            <asp:TextBox ID="txtWorkingCapital" runat="server" Width="100px" MaxLength="12" ToolTip="Standard working capital is calculated on and linked from Sch2 (10% of sales). Use this cell only for extraodinary working capital. Enter a negative number if working capital is increasing. Enter a postitive number if working capital is decreasing.">0.00</asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="ftbWorkingCapital" runat="server" TargetControlID="txtWorkingCapital"
                                FilterType="Custom, Numbers" ValidChars="-." />
                            <asp:RangeValidator ID="rvWorkingCapital" runat="server" ControlToValidate="txtWorkingCapital"
                                Display="Dynamic" ErrorMessage="Working Capital requires a numeric value -999999999.99 to 999999999.99"
                                Height="16px" MaximumValue="999999999.99" MinimumValue="-999999999.99" Type="Double"
                                ValidationGroup="vsOtherExp"><</asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_textbold">
                            Total Investment ($):
                        </td>
                        <td class="c_textbold" style="width: 243px; color: #990000;">
                            <asp:Label ID="lblTotalInvestment1" runat="server" Text="0.00" />
                            <asp:TextBox ID="txtHDTotalInvestment" runat="server" Visible="False" Width="20px" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="c_textbold">
                            RELATED EXPENSES:
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Start-up Expense ($):
                        </td>
                        <td>
                            <asp:TextBox ID="txtStartupExpense" runat="server" Width="100px" MaxLength="12" Text="0.00" />
                            <ajax:FilteredTextBoxExtender ID="ftbExpense" runat="server" TargetControlID="txtStartupExpense"
                                FilterType="Custom, Numbers" ValidChars="-." />
                            <asp:RangeValidator ID="rvStartupExpense" runat="server" ControlToValidate="txtStartupExpense"
                                Display="Dynamic" ErrorMessage="Start-Up Expense requires a numeric value -999999999.99 to 999999999.99"
                                Height="16px" MaximumValue="999999999.99" MinimumValue="-999999999.99" Type="Double"
                                ValidationGroup="vsOtherExp"><</asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Customer Reimbursement ($):
                        </td>
                        <td>
                            <asp:TextBox ID="txtCustReimb" runat="server" Width="100px" MaxLength="12" Text="0.00" />
                            <ajax:FilteredTextBoxExtender ID="ftbCustReimb" runat="server" TargetControlID="txtCustReimb"
                                FilterType="Custom, Numbers" ValidChars="-." />
                            <asp:RangeValidator ID="rvCustReimb" runat="server" ControlToValidate="txtCustReimb"
                                Display="Dynamic" ErrorMessage="Customer Reimbursement requires a numeric value -999999999.99 to 999999999.99"
                                Height="16px" MaximumValue="999999999.99" MinimumValue="-999999999.99" Type="Double"
                                ValidationGroup="vsOtherExp"><</asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
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
                            <asp:TextBox ID="txtDiscountReturn" runat="server" Width="5px" MaxLength="3" Visible="False"
                                Text="0" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblReqCRProjNo" runat="server" Font-Bold="True" ForeColor="Red" Text="* "
                                Visible="False" />
                            Cost Reduction Reference:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddCRProjNo" runat="server" AutoPostBack="True" />
                            <asp:RequiredFieldValidator ID="rfvCRProjNo" runat="server" ControlToValidate="ddCRProjNo"
                                ErrorMessage="Cost Reduction Reference is a required field." ValidationGroup="vsOtherExp"
                                Visible="false"><</asp:RequiredFieldValidator>
                            <asp:Button ID="btnCRProjNoReq" runat="server" Text="Request" />
                            <asp:CheckBox ID="cbCRProjNoReq" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Repair Savings ($):
                        </td>
                        <td>
                            <asp:TextBox ID="txtRepairSavings" runat="server" Width="100px" MaxLength="12" Text="0.00" />
                            <ajax:FilteredTextBoxExtender ID="ftbRepairSavings" runat="server" TargetControlID="txtRepairSavings"
                                FilterType="Custom, Numbers" ValidChars="-." />
                            <asp:RangeValidator ID="rvRepairSavings" runat="server" ControlToValidate="txtRepairSavings"
                                Display="Dynamic" ErrorMessage="Repair Savings requires a numeric value -999999999.99 to 999999999.99"
                                Height="16px" MaximumValue="999999999.99" MinimumValue="-999999999.99" Type="Double"
                                ValidationGroup="vsOtherExp"><</asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Scrap Savings ($):
                        </td>
                        <td>
                            <asp:TextBox ID="txtScrapSavings" runat="server" Width="100px" MaxLength="12" Text="0.00" />
                            <ajax:FilteredTextBoxExtender ID="ftbScrapSavings" runat="server" TargetControlID="txtScrapSavings"
                                FilterType="Custom, Numbers" ValidChars="-." />
                            <asp:RangeValidator ID="rvScrapSavings" runat="server" ControlToValidate="txtScrapSavings"
                                Display="Dynamic" ErrorMessage="Scrap Savings requires a numeric value -999999999.99 to 999999999.99"
                                Height="16px" MaximumValue="999999999.99" MinimumValue="-999999999.99" Type="Double"
                                ValidationGroup="vsOtherExp"><</asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Consumable Savings ($):
                        </td>
                        <td>
                            <asp:TextBox ID="txtConsumableSavings" runat="server" Width="100px" MaxLength="12"
                                Text="0.00" />
                            <ajax:FilteredTextBoxExtender ID="ftbConsumableSavings" runat="server" TargetControlID="txtConsumableSavings"
                                FilterType="Custom, Numbers" ValidChars="-." />
                            <asp:RangeValidator ID="rvConsumableSavings" runat="server" ControlToValidate="txtConsumableSavings"
                                Display="Dynamic" ErrorMessage="Consumable Savings requires a numeric value -999999999.99 to 999999999.99"
                                Height="16px" MaximumValue="999999999.99" MinimumValue="-999999999.99" Type="Double"
                                ValidationGroup="vsOtherExp"><</asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Labor Savings ($):
                        </td>
                        <td>
                            <asp:TextBox ID="txtLaborSavings" runat="server" Width="100px" MaxLength="12" Text="0.00" />
                            <ajax:FilteredTextBoxExtender ID="ftbLaborSavings" runat="server" TargetControlID="txtLaborSavings"
                                FilterType="Custom, Numbers" ValidChars="-." />
                            <asp:RangeValidator ID="rvLaborSavings" runat="server" ControlToValidate="txtLaborSavings"
                                Display="Dynamic" ErrorMessage="Labor Savings requires a numeric value -999999999.99 to 999999999.99"
                                Height="16px" MaximumValue="999999999.99" MinimumValue="-999999999.99" Type="Double"
                                ValidationGroup="vsOtherExp"><</asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Other Savings ($):
                        </td>
                        <td>
                            <asp:TextBox ID="txtOtherSavings" runat="server" Width="100px" MaxLength="12" Text="0.00" />
                            <ajax:FilteredTextBoxExtender ID="ftbOtherSavings" runat="server" TargetControlID="txtOtherSavings"
                                FilterType="Custom, Numbers" ValidChars="-." />
                            <asp:RangeValidator ID="rvOtherSavings" runat="server" ControlToValidate="txtOtherSavings"
                                Display="Dynamic" ErrorMessage="Other Savings requires a numeric value -999999999.99 to 999999999.99"
                                Height="16px" MaximumValue="999999999.99" MinimumValue="-999999999.99" Type="Double"
                                ValidationGroup="vsOtherExp"><</asp:RangeValidator>
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
                            <asp:Button ID="btnReset3" runat="server" Text="Reset" />
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
                    OnRowCommand="gvApprovers_RowCommand" DataSourceID="odsApprovers" Width="1000px"
                    RowStyle-Height="20px" RowStyle-CssClass="c_text" HeaderStyle-CssClass="c_text"
                    SkinID="StandardGrid">
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
                        <asp:TemplateField HeaderText="Assigned Team Member" SortExpression="TeamMemberName">
                            <EditItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("TeamMemberName") %>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("TeamMemberName") %>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddResponsibleTM" runat="server" DataSource='<%# commonFunctions.GetTeamMemberbySubscription(92) %>'
                                    DataValueField="TMID" DataTextField="TMName" SelectedValue='<%# Bind("TMID") %>'
                                    AppendDataBoundItems="true">
                                    <asp:ListItem Selected="True">
                                    </asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvResposibleTM" runat="server" ControlToValidate="ddResponsibleTM"
                                    ErrorMessage="Assigned Team Member is a required field." Font-Bold="True" ValidationGroup="InsertApprovalInfo"><</asp:RequiredFieldValidator>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="150px" Wrap="True" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date Notified" SortExpression="DateNotified">
                            <EditItemTemplate>
                                <asp:Label ID="txtDateNotified" runat="server" Text='<%# Eval("DateNotified") %>'></asp:Label>
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
                                    Font-Bold="True" ValidationGroup="EditApprovalInfo"><</asp:RequiredFieldValidator><asp:TextBox
                                        ID="txtTeamMemberID" runat="server" Text='<%# Eval("TeamMemberID") %>' ReadOnly="true"
                                        Width="0px" Visible="false" />
                                <asp:TextBox ID="txtOrigTeamMemberID" runat="server" Text='<%# Eval("OrigTeamMemberID") %>'
                                    ReadOnly="true" Width="0px" Visible="false" /><asp:TextBox ID="hfSeqNo" runat="server"
                                        Text='<%# Eval("SeqNo") %>' ReadOnly="true" Width="0px" Visible="false" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Comments") %>' />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="400px" />
                            <ItemStyle Wrap="True" />
                            <FooterTemplate>
                                <asp:Label ID="lblMsg2" runat="server" Text="<< Use this row to add another TM for approval, when required. >>"
                                    Font-Italic="true" ForeColor="Black" />
                            </FooterTemplate>
                        </asp:TemplateField>
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
                        <asp:TemplateField ShowHeader="False">
                            <FooterTemplate>
                                <asp:ImageButton ID="btnInsert" runat="server" CausesValidation="true" ValidationGroup="InsertApprovalInfo"
                                    CommandName="Insert" ToolTip="Insert" ImageUrl="~/images/save.jpg" />
                                <asp:ImageButton ID="ibtnUndo" runat="server" CausesValidation="False" CommandName="Undo"
                                    ImageUrl="~/images/undo-gray.jpg" ToolTip="Cancel" ValidationGroup="InsertApprovalInfo" />
                            </FooterTemplate>
                            <ItemStyle Width="60px" HorizontalAlign="Center" />
                            <FooterStyle Width="60px" />
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                    ToolTip="Delete" ImageUrl="~/images/delete.jpg" />
                            </ItemTemplate>
                            <ItemStyle Width="30px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="c_text" />
                </asp:GridView>
                <asp:ValidationSummary ID="vsEditApprovalInfo" runat="server" ShowMessageBox="True"
                    ValidationGroup="EditApprovalInfo" />
                <asp:ValidationSummary ID="vsInsertApprovalInfo" runat="server" ShowMessageBox="True"
                    ValidationGroup="InsertApprovalInfo" />
                <asp:ObjectDataSource ID="odsApprovers" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetExpProjRepairApproval" TypeName="ExpProjRepairBLL" UpdateMethod="UpdateExpProjRepairApproval"
                    DeleteMethod="DeleteExpProjRepairApproval" InsertMethod="InsertExpProjRepairAddLvl1Aprvl">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="String" />
                        <asp:Parameter DefaultValue="0" Name="Sequence" Type="Int32" />
                    </SelectParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="Status" Type="String" />
                        <asp:Parameter Name="Comments" Type="String" />
                        <asp:Parameter Name="SameTMID" Type="Boolean" />
                        <asp:Parameter Name="original_ProjectNo" Type="String" />
                        <asp:Parameter Name="original_SeqNo" Type="Int32" />
                        <asp:Parameter Name="original_TeamMemberID" Type="Int32" />
                        <asp:Parameter Name="original_OrigTeamMemberID" Type="Int32" />
                        <asp:Parameter Name="SeqNo" Type="Int32" />
                        <asp:Parameter Name="TeamMemberName" Type="String" />
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
                    <InsertParameters>
                        <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="String" />
                        <asp:Parameter Name="SeqNo" Type="Int32" />
                        <asp:Parameter Name="ResponsibleTMID" Type="Int32" />
                        <asp:Parameter Name="OriginalTMID" Type="Int32" />
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
                            <asp:Button ID="btnBuildApproval" runat="server" CausesValidation="False" Text="Build Approval List" />&nbsp;
                            <asp:Button ID="btnFwdApproval" runat="server" CausesValidation="true" Text="Submit for Approval"
                                Width="130px"  ValidationGroup="ReSubmitApproval"/>
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
                            <asp:Label ID="lblReply" runat="server" Font-Bold="True" ForeColor="Red" />&nbsp;
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
                <asp:GridView ID="gvQuestion" runat="server" AutoGenerateColumns="False" DataSourceID="odsQuestion"
                    OnRowDataBound="gvQuestion_RowDataBound" Width="900px" RowStyle-BorderStyle="None"
                    SkinID="CommBoardRSS">
                    <RowStyle BorderStyle="None" />
                    <Columns>
                        <asp:TemplateField HeaderText="Reply">
                            <ItemTemplate>
                                <%-- <% If ViewState("Admin") = "true" Then%>--%>
                                <asp:HyperLink ID="lnkAppend" runat="server" ImageUrl="~/images/messanger30.jpg"
                                    ToolTip="Reply" NavigateUrl='<%# GoToCommunicationBoard(DataBinder.Eval(Container, "DataItem.ProjectNo"),DataBinder.Eval(Container, "DataItem.RSSID"),DataBinder.Eval(Container, "DataItem.ApprovalLevel"),DataBinder.Eval(Container, "DataItem.TeamMemberID")) %>' />
                                <%--  <%Else%>
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
                                                    HeaderStyle-Width="500px" ItemStyle-Width="500px" ItemStyle-Font-Bold="true" />
                                                <asp:BoundField DataField="TeamMemberName" HeaderText="" SortExpression="TeamMemberName"
                                                    HeaderStyle-Width="100px" ItemStyle-Width="100px" />
                                                <asp:BoundField DataField="PostDate" HeaderText="" SortExpression="PostDate" />
                                            </Columns>
                                        </asp:GridView>
                                        <asp:ObjectDataSource ID="odsReply" runat="server" OldValuesParameterFormatString="original_{0}"
                                            SelectMethod="GetExpProjRepairRSSReply" TypeName="ExpProjRepairBLL">
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
                    SelectMethod="GetExpProjRepairRSS" TypeName="ExpProjRepairBLL">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="String" />
                        <asp:Parameter Name="RSSID" Type="Int32" DefaultValue="0" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </asp:View>
        </asp:MultiView>
    </asp:Panel>
</asp:Content>
