<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="AR_Deduction_Reports.aspx.vb" Inherits="AR_AR_Deduction_Reports" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="1200px">
        <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
            Visible="False" />
        <br />
        <table width="45%">
            <tr>
                <td class="p_text">
                    <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="* " />
                    <asp:Label ID="lblReprtType" runat="server" Text="Select a Report Type:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddReportType" runat="server" AutoPostBack="true">
                        <asp:ListItem Value="0">Customer Deductions by Customer</asp:ListItem>
                        <asp:ListItem Value="1">Customer Deductions by Reason</asp:ListItem>
                        <asp:ListItem Value="2">Counter Measures</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table width="100%" border="0">
            <tr>
                <td colspan="4" class="c_textbold">
                    <i>
                        <asp:Label ID="lblFilterSelections" runat="server" Text="Filter Selections:" /></i>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblRecNo" runat="server" Text="Rec No:" />
                </td>
                <td>
                    <asp:TextBox ID="txtARDID" runat="server" MaxLength="15" Width="100px"></asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftbARDID" runat="server" TargetControlID="txtARDID"
                        FilterType="Custom" ValidChars="1234567890%" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblReferenceNo" runat="server" Text="Reference No:" />
                </td>
                <td>
                    <asp:TextBox ID="txtReferenceNo" runat="server" MaxLength="15" Width="100px"></asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftbReferenceNo" runat="server" TargetControlID="txtReferenceNo"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890%" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblDateSentFrom" runat="server" Text="Date Sent From:" />
                </td>
                <td>
                    <asp:TextBox ID="txtDateSubFrom" runat="server" MaxLength="12" Width="80px" />
                    <ajax:FilteredTextBoxExtender ID="ftbeDateSubFrom" runat="server" TargetControlID="txtDateSubFrom"
                        FilterType="Custom, Numbers" ValidChars="/" />
                    <asp:ImageButton runat="server" ID="imgDateSubFrom" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                        CausesValidation="False" />
                    <ajax:CalendarExtender ID="cbeDateSubFrom" runat="server" TargetControlID="txtDateSubFrom"
                        PopupButtonID="imgDateSubFrom" Format="MM/dd/yyyy" />
                    <asp:RegularExpressionValidator ID="revDateSubFrom" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtDateSubFrom" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px" ValidationGroup="vsList"><</asp:RegularExpressionValidator>
                    <asp:CompareValidator ID="cvDateSubFrom" runat="server" ErrorMessage="Date Sent From must be less than Date Sent To."
                        ControlToCompare="txtDateSubTo" ControlToValidate="txtDateSubFrom" Operator="LessThanEqual"
                        Type="Date" ValidationGroup="vsList"><</asp:CompareValidator>
                </td>
                <td class="p_text">
                    <asp:Label ID="lblDateSentTo" runat="server" Text="Date Sent To:" />
                </td>
                <td>
                    <asp:TextBox ID="txtDateSubTo" runat="server" MaxLength="12" Width="80px" />
                    <ajax:FilteredTextBoxExtender ID="ftbeDateSubTo" runat="server" TargetControlID="txtDateSubTo"
                        FilterType="Custom, Numbers" ValidChars="/" />
                    <asp:ImageButton runat="server" ID="imgDateSubTo" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                        CausesValidation="False" />
                    <ajax:CalendarExtender ID="cbeDateSubTo" runat="server" TargetControlID="txtDateSubTo"
                        PopupButtonID="imgDateSubTo" Format="MM/dd/yyyy" />
                    <asp:RegularExpressionValidator ID="revDateSubTo" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtDateSubTo" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px" ValidationGroup="vsList"><</asp:RegularExpressionValidator>
                    <asp:CompareValidator ID="cvDateSubTo" runat="server" ControlToCompare="txtDateSubFrom"
                        ControlToValidate="txtDateSubTo" ErrorMessage="Date Sent To must be greater than Date Sent From."
                        Operator="GreaterThanEqual" Type="Date" ValidationGroup="vsList"><</asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="Label1" runat="server" Text="Date Sent To:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddSubmittedBy" runat="server" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblUGNLocation" runat="server" Text="UGN Location:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddUGNFacility" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="p_text" style="width: 160px">
                    <asp:Label ID="lblCustomer" runat="server" Text="Customer:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddCustomer" runat="server" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblRecordStatus" runat="server" Text="Record Status:" />
                </td>
                <td class="c_textbold" style="color: red;" colspan="3">
                    <asp:DropDownList ID="ddRecStatus" runat="server">
                        <asp:ListItem Value="" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="NOpen">New Record</asp:ListItem>
                        <asp:ListItem Value="CClosed">Closed</asp:ListItem>
                        <asp:ListItem Value="6Closed @60 days">Closed @60 days</asp:ListItem>
                        <asp:ListItem Value="TIn Process">In Process</asp:ListItem>
                        <asp:ListItem Value="I6C">Issued</asp:ListItem>
                        <asp:ListItem Value="OIRA">Outstanding</asp:ListItem>
                        <%-- <asp:ListItem Value="RIn Process">Rejected</asp:ListItem>--%>
                        <asp:ListItem Value="AApproved">Review Completed</asp:ListItem>
                        <asp:ListItem Value="VVoid">Void</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblReasonDeduction" runat="server" Text="Reason for Deduction:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddReason" runat="server" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblComments" runat="server" Text="Comments:" />
                </td>
                <td>
                    <asp:TextBox ID="txtComments" runat="server" MaxLength="50" Width="250px"></asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftbComments" runat="server" TargetControlID="txtComments"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890, %. " />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblClosedDateFrom" runat="server" Text="Closed Date From:" />
                </td>
                <td>
                    <asp:TextBox ID="txtClosedDateFrom" runat="server" MaxLength="12" Width="80px" />
                    <ajax:FilteredTextBoxExtender ID="ftbeClosedDateFrom" runat="server" TargetControlID="txtClosedDateFrom"
                        FilterType="Custom, Numbers" ValidChars="/" />
                    <asp:ImageButton runat="server" ID="imgClosedDateFrom" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                        CausesValidation="False" />
                    <ajax:CalendarExtender ID="cbeClosedDateFrom" runat="server" TargetControlID="txtClosedDateFrom"
                        PopupButtonID="imgClosedDateFrom" Format="MM/dd/yyyy" />
                    <asp:RegularExpressionValidator ID="revClosedDateFrom" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtClosedDateFrom" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px" ValidationGroup="vsList"><</asp:RegularExpressionValidator>
                    <asp:CompareValidator ID="cvClosedDateFrom" runat="server" ErrorMessage="Closed Date From must be less than Closed Date To."
                        ControlToCompare="txtClosedDateTo" ControlToValidate="txtClosedDateFrom" Operator="LessThanEqual"
                        Type="Date" ValidationGroup="vsList"><</asp:CompareValidator>
                </td>
                <td class="p_text">
                    <asp:Label ID="lblClosedDateTo" runat="server" Text="Closed Date To:" />
                </td>
                <td>
                    <asp:TextBox ID="txtClosedDateTo" runat="server" MaxLength="12" Width="80px" />
                    <ajax:FilteredTextBoxExtender ID="ftbeClosedDateTo" runat="server" TargetControlID="txtClosedDateTo"
                        FilterType="Custom, Numbers" ValidChars="/" />
                    <asp:ImageButton runat="server" ID="imgClosedDateTo" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                        CausesValidation="False" />
                    <ajax:CalendarExtender ID="cbeClosedDateTo" runat="server" TargetControlID="txtClosedDateTo"
                        PopupButtonID="imgClosedDateTo" Format="MM/dd/yyyy" />
                    <asp:RegularExpressionValidator ID="revClosedDateTo" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtClosedDateTo" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px" ValidationGroup="vsList"><</asp:RegularExpressionValidator>
                    <asp:CompareValidator ID="cvClosedDateTo" runat="server" ControlToCompare="txtClosedDateFrom"
                        ControlToValidate="txtClosedDateTo" ErrorMessage="Closed Date To must be greater than Closed Date From."
                        Operator="GreaterThanEqual" Type="Date" ValidationGroup="vsList"><</asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblPartNo" runat="server" Text="Part Number:" />
                </td>
                <td>
                    <asp:TextBox ID="txtPartNo" runat="server" MaxLength="40" Width="150px" />
                    <ajax:FilteredTextBoxExtender ID="ftbPartNo" runat="server" TargetControlID="txtPartNo"
                        ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,-% "
                        Enabled="True" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblSortBy" runat="server" Text="Sort By:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddSortBy" runat="server">
                        <asp:ListItem Value=""></asp:ListItem>
                        <asp:ListItem Value="RecStatus">Record Status</asp:ListItem>
                        <asp:ListItem Value="ARDID">RecNo</asp:ListItem>
                        <asp:ListItem>Reason</asp:ListItem>
                        <asp:ListItem Value="UGNFacility">UGN Location</asp:ListItem>
                        <asp:ListItem Value="ReferenceNo">Reference No</asp:ListItem>
                        <asp:ListItem Value="DeductionAmount">Deduction Amount</asp:ListItem>
                        <asp:ListItem Value="DateSubmitted">Date Sent</asp:ListItem>
                        <asp:ListItem Value="UpdatedOn">Closed Date</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="3">
                    <asp:Button ID="btnReport" runat="server" Text="Create Report" CommandName="reset"
                        CausesValidation="true" ValidationGroup="vsList" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CommandName="reset" CausesValidation="False" />
                </td>
            </tr>
        </table>
        <asp:ValidationSummary ID="summaryList" runat="server" ValidationGroup="vsList" ShowMessageBox="true" />
    </asp:Panel>
</asp:Content>
