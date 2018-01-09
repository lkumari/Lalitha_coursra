<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="CycleCounterMatrix.aspx.vb" Inherits="CCM_CycleCounterMatrix" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="1200px">
        <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
            Visible="False"></asp:Label>
        <hr />
        <table width="60%">
            <tr>
                <td colspan="4" style="font-size: small; color: #990000">
                    <b>Note:</b> This page has an auto reload when selecting report options, please
                    allow the page to load before submitting.
                </td>
            </tr>
        </table>
        <br />
        <table width="30%">
            <tr>
                <td class="p_text">
                    <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="* " />
                    Select a Report Type:
                </td>
                <td>
                    <asp:DropDownList ID="ddReportType" runat="server" AutoPostBack="true">
                        <asp:ListItem>Matrix</asp:ListItem>
                        <asp:ListItem>Classification</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <br />
        <table width="60%">
            <tr>
                <td colspan="4" class="c_textbold">
                    <i>Filter Selections:</i>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblFacility" runat="server" ForeColor="Red" Text="* " />
                    Facility:
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddUGNFacility" runat="server" />
                    &nbsp;<asp:RequiredFieldValidator ID="rfvUGNFacility" runat="server" ControlToValidate="ddUGNFacility"
                        ErrorMessage="Facility is a required field." ValidationGroup="vgCCM"><</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="* " />
                    Run Report From Date:
                </td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server" MaxLength="12" Width="80px"></asp:TextBox>
                    <asp:ImageButton ID="imgFromDate" runat="server" AlternateText="Click to show calendar"
                        CausesValidation="False" Height="19px" ImageAlign="Middle" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        Width="19px" />
                    <ajax:CalendarExtender ID="ceFromDate" runat="server" PopupButtonID="imgFromDate"
                        TargetControlID="txtFromDate">
                    </ajax:CalendarExtender>
                    &nbsp;<asp:RequiredFieldValidator ID="rfvFromDate" runat="server" ControlToValidate="txtFromDate"
                        ErrorMessage="From Date is a required field." ValidationGroup="vgCCM"><</asp:RequiredFieldValidator><asp:RegularExpressionValidator
                            ID="revFromDate" runat="server" ControlToValidate="txtFromDate" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                            Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                            Width="8px" ValidationGroup="vgCCM"><</asp:RegularExpressionValidator><asp:CompareValidator
                                ID="cvFromDate" runat="server" ControlToCompare="txtToDate" ControlToValidate="txtFromDate"
                                ErrorMessage="'From Date' must be less than 'To Date'." Operator="LessThanEqual"
                                Type="Date" ValidationGroup="vgCCM"><</asp:CompareValidator>
                </td>
                <td class="p_text">
                    <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="* " />
                    To Date:
                </td>
                <td>
                    <asp:TextBox ID="txtToDate" runat="server" MaxLength="12" Width="80px"></asp:TextBox>
                    <asp:ImageButton ID="imgToDate" runat="server" AlternateText="Click to show calendar"
                        CausesValidation="False" Height="19px" ImageAlign="Middle" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        Width="19px" />
                    <ajax:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgToDate"
                        TargetControlID="txtToDate">
                    </ajax:CalendarExtender>
                    &nbsp;<asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtToDate"
                        ErrorMessage="To Date is a required field." ValidationGroup="vgCCM"><</asp:RequiredFieldValidator><asp:RegularExpressionValidator
                            ID="revToDate" runat="server" ControlToValidate="txtToDate" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                            Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                            Width="8px" ValidationGroup="vgCCM"><</asp:RegularExpressionValidator><asp:CompareValidator
                                ID="cvToDate" runat="server" ControlToCompare="txtToDate" ControlToValidate="txtFromDate"
                                ErrorMessage="'To Date' must be greater than 'From Date'." Operator="LessThanEqual"
                                Type="Date" ValidationGroup="vgCCM"><</asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td colspan="4" style="height: 25px">
                </td>
            </tr>
            <tr>
                <td colspan="4" class="c_textbold">
                    <i>Report Options:</i>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="Label4" runat="server" ForeColor="Red" Text="* " />
                    &nbsp;Select a Report Format:
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddReportFormat" runat="server" AutoPostBack="true">
                        <asp:ListItem Selected="True"></asp:ListItem>
                        <asp:ListItem>Detail</asp:ListItem>
                        <asp:ListItem>Grid View Summary</asp:ListItem>
                        <asp:ListItem>Chart View Summary</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;<asp:RequiredFieldValidator ID="rfvReportFormat" runat="server" ControlToValidate="ddReportFormat"
                        ErrorMessage="Report Format is a required field." ValidationGroup="vgCCM"><</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblSortBy" runat="server" Text=" Sort By:" Visible="false" />
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddSortBy" runat="server">
                        <asp:ListItem Value="I">Item Number</asp:ListItem>
                        <asp:ListItem Value="E">Extended Amount</asp:ListItem>
                        <asp:ListItem Value="R">Reason Code</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <% If ViewState("ObjectRole") = True Then%>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblStoreMonthEndValues" runat="server" Text="Store Month End Values?"
                        Visible="false" />
                </td>
                <td colspan="3">
                    <asp:CheckBox ID="cbStoreMEV" runat="server" Visible="false" AutoPostBack="true" />
                    <asp:Label ID="lblCheckBox" runat="server" Text=" check the box if you wish to save"
                        Visible="false" />
                </td>
            </tr>
            <%End If%>
            <tr>
                <td colspan="4" style="height: 23px">
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="3">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CausesValidation="true" ValidationGroup="vgCCM" />&nbsp;
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="False" Style="height: 26px" />
                </td>
            </tr>
        </table>
        <asp:ValidationSummary ID="vsCCM" runat="server" ShowMessageBox="True" ValidationGroup="vgCCM"
            Width="316px" />
    </asp:Panel>
</asp:Content>
