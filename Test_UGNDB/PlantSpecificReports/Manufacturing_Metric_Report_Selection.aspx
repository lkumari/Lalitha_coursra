<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Manufacturing_Metric_Report_Selection.aspx.vb" Inherits="Manufacturing_Metric_Report_Selection"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server">
        <asp:Label ID="lblMessage" SkinID="MessageLabelSkin" runat="server"></asp:Label>
        <asp:ValidationSummary ID="vsSave" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgSave" />
        <table width="80%">
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap">
                    <asp:Label ID="lblReportTypeMarker" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="*" />
                    Report Type:
                </td>
                <td>
                    <asp:DropDownList ID="ddReportType" runat="server" AutoPostBack="true">
                        <asp:ListItem Value="D" Text="Daily Actuals" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="W" Text="Weekly Actuals"></asp:ListItem>
                        <asp:ListItem Value="MTD" Text="Month To Date Actuals"></asp:ListItem>
                        <asp:ListItem Value="M" Text="Monthly (Built By Plant Controllers)"></asp:ListItem>
                        <asp:ListItem Value="MAC" Text="Monthly Actuals Comparison (UGN Quick Summary)"></asp:ListItem>
                        <asp:ListItem Value="Y" Text="Year To Month (Monthly Built By Plant Controllers)"></asp:ListItem>
                        <asp:ListItem Value="YTD" Text="Year To Date Actuals"></asp:ListItem>
                        <asp:ListItem Value="DR" Text="Other Date Range Actuals"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvReportType" runat="server" ControlToValidate="ddReportType"
                        ErrorMessage="Report Type is required." Font-Bold="True" ValidationGroup="vgSave"
                        Text="<" SetFocusOnError="true" />
                </td>
                <td class="p_text" align="right" style="white-space: nowrap">
                    <asp:Label ID="lblUGNFacilityMarker" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="*" />
                    <asp:Label ID="lblUGNFacilityLabel" runat="server" Text="UGN Facility:" ForeColor="Blue" />
                </td>
                <td>
                    <asp:DropDownList ID="ddUGNFacility" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="ALL UGN FACILITIES" Value=""></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblDayMarker" runat="server" Font-Bold="True" ForeColor="Red" Text="*" />
                    <asp:Label ID="lblDayLabel" runat="server" Text="Day:" ForeColor="Blue" />
                </td>
                <td>
                    <asp:DropDownList ID="ddDay" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvDay" runat="server" ControlToValidate="ddDay"
                        ErrorMessage="Day is required." Font-Bold="True" ValidationGroup="vgSave" Text="<"
                        SetFocusOnError="true" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblWeekMarker" runat="server" Font-Bold="True" ForeColor="Red" Text="*" />
                    <asp:Label ID="lblWeekLabel" runat="server" Text="Week:" ForeColor="Blue" />
                </td>
                <td>
                    <asp:DropDownList ID="ddWeek" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvWeek" runat="server" ControlToValidate="ddWeek"
                        ErrorMessage="Week is required." Font-Bold="True" ValidationGroup="vgSave" Text="<"
                        SetFocusOnError="true" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblMonthMarker" runat="server" Font-Bold="True" ForeColor="Red" Text="*" />
                    <asp:Label ID="lblMonthLabel" runat="server" Text="Month:" ForeColor="Blue" />
                </td>
                <td>
                    <asp:DropDownList ID="ddMonth" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvMonth" runat="server" ControlToValidate="ddMonth"
                        ErrorMessage="Month is required." Font-Bold="True" ValidationGroup="vgSave" Text="<"
                        SetFocusOnError="true" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblYearMarker" runat="server" Font-Bold="True" ForeColor="Red" Text="*" />
                    <asp:Label ID="lblYearLabel" runat="server" Text="Year:" ForeColor="Blue" />
                </td>
                <td>
                    <asp:DropDownList ID="ddYear" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvYear" runat="server" ControlToValidate="ddYear"
                        ErrorMessage="Year is required." Font-Bold="True" ValidationGroup="vgSave" Text="<"
                        SetFocusOnError="true" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblStartDateMarker" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="*" />
                    <asp:Label ID="lblStartDateLabel" runat="server" Text="Start Date (mm/dd/yyyy):"
                        ForeColor="Blue" />
                </td>
                <td style="white-space: nowrap">
                    <asp:TextBox ID="txtStartDate" runat="server" Width="85px" MaxLength="10"></asp:TextBox>
                    <asp:ImageButton runat="server" ID="imgStartDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                    <ajax:CalendarExtender ID="cbeStartDate" runat="server" TargetControlID="txtStartDate"
                        PopupButtonID="imgStartDate" />
                    <asp:RegularExpressionValidator ID="revStartDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtStartDate" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px" ValidationGroup="vgSave"><</asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ControlToValidate="txtStartDate"
                        ErrorMessage="Start Date is required." Font-Bold="True" ValidationGroup="vgSave"
                        Text="<" SetFocusOnError="true" />
                </td>
                <td class="p_text">
                     <asp:Label ID="lblEndDateMarker" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="*" />
                    <asp:Label ID="lblEndDateLabel" runat="server" Text="End Date (mm/dd/yyyy):"
                        ForeColor="Blue" />
                </td>
                <td style="white-space: nowrap">
                    <asp:TextBox ID="txtEndDate" runat="server" Width="85px" MaxLength="10"></asp:TextBox>
                    <asp:ImageButton runat="server" ID="imgEndDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                    <ajax:CalendarExtender ID="cbeEndDate" runat="server" TargetControlID="txtEndDate"
                        PopupButtonID="imgEndDate" />
                    <asp:RegularExpressionValidator ID="revEndDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtEndDate" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px" ValidationGroup="vgSave"><</asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ControlToValidate="txtEndDate"
                        ErrorMessage="End Date is required." Font-Bold="True" ValidationGroup="vgSave"
                        Text="<" SetFocusOnError="true" />
                </td>
            </tr>
        </table>
        <table width="80%">
            <tr>
                <td align="center">
                    <asp:Button runat="server" ID="btnSubmit" Text="Submit" CausesValidation="true" ValidationGroup="vgSave" />
                    <asp:Button runat="server" ID="btnReset" Text="Reset" CausesValidation="false" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
