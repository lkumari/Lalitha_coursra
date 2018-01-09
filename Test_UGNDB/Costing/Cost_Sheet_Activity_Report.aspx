<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Cost_Sheet_Activity_Report.aspx.vb" MaintainScrollPositionOnPostback="true"
    Inherits="Costing_Cost_Sheet_Activity_Report" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server">
        <asp:Label ID="lblMessage" SkinID="MessageLabelSkin" runat="server"></asp:Label>
        <asp:ValidationSummary ID="vsCosting" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgCosting" />
        <table style="width: 98%">
            <tr>
                <td class="p_text" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblQuoteDateFromLabel" Text="Quote Date FROM:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtQuoteDateFromValue" Width="150px" Visible="false"></asp:TextBox>
                    <asp:ImageButton runat="server" ID="imgQuoteDateFromValue" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                    <ajax:CalendarExtender ID="cbeQuoteDateFrom" runat="server" TargetControlID="txtQuoteDateFromValue"
                        PopupButtonID="imgQuoteDateFromValue" />
                    <asp:RegularExpressionValidator ID="revQuoteDateFromDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtQuoteDateFromValue" Font-Bold="True" ToolTip="MM/DD/YYYY"
                        ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px" ValidationGroup="vgCosting"><</asp:RegularExpressionValidator>
                </td>
                <td class="p_text" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblQuoteDateToLabel" Text="Quote Date TO:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtQuoteDateToValue" Width="150px" Visible="false"></asp:TextBox>
                    <asp:ImageButton runat="server" ID="imgQuoteDateToValue" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                    <ajax:CalendarExtender ID="cbeQuoteDateTo" runat="server" TargetControlID="txtQuoteDateToValue"
                        PopupButtonID="imgQuoteDateToValue" />
                    <asp:RegularExpressionValidator ID="revQuoteDateToDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtQuoteDateToValue" Font-Bold="True" ToolTip="MM/DD/YYYY"
                        ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px" ValidationGroup="vgCosting"><</asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label runat="server" ID="lblUGNFacilityLabel" Text="UGN Facility:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddUGNFacilityValue" runat="server" Width="156px" Visible="false">
                    </asp:DropDownList>
                </td>
                <td class="p_text">
                    <asp:Label runat="server" ID="lblTeamMemberLabel" Text="Team Member:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddTeamMemberValue" runat="server" Width="156px" Visible="false">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center">
                    <asp:Button ID="btnSummary" runat="server" Text="Summary Report" ValidationGroup="vgCosting"
                        Visible="false" />
                    <asp:Button ID="btnDetail" runat="server" Text="Detail Report" ValidationGroup="vgCosting"
                        Visible="false" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" ValidationGroup="vgCosting"
                        Visible="false" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
