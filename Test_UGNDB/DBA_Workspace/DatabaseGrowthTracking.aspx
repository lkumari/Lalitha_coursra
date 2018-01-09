<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="DatabaseGrowthTracking.aspx.vb" Inherits="DBA_Workspace_DatabaseGrowthTracking"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="1000px" DefaultButton="btnSubmit">
        <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
            Visible="False"></asp:Label>
        <hr />
        <br />
        <table width="40%" border="0">
        <tr>
        <td colspan="2" class="c_text"><b><i>Filter Options:</i></b></td>
        </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblReqDtFrom" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                    Date Recorded From:</td>
                <td>
                    <asp:TextBox ID="txtReqDtFrom" runat="server" MaxLength="10" Width="80px" />&nbsp;<asp:ImageButton
                        ID="imgReqDtFrom" runat="server" AlternateText="Click to show calendar" CausesValidation="False"
                        Height="19px" ImageAlign="Middle" ImageUrl="~/images/AJAX/Calendar_scheduleHS.png"
                        Width="19px" />
                    <ajax:CalendarExtender ID="ceReqDtFrom" runat="server" PopupButtonID="imgReqDtFrom"
                        TargetControlID="txtReqDtFrom">
                    </ajax:CalendarExtender>
                    <asp:RegularExpressionValidator ID="revReqDtFrom" runat="server" ControlToValidate="txtReqDtFrom"
                        ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format ' Font-Bold="True"
                        ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px"><</asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="rfvReqDtFrom" runat="server" ControlToValidate="txtReqDtFrom"
                        ErrorMessage="Request Date From is a required field." Font-Bold="False"><</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblReqDtTo" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                     Date Recorded To:</td>
                <td>
                    <asp:TextBox ID="txtReqDtTo" runat="server" MaxLength="10" Width="80px" />&nbsp;<asp:ImageButton
                        ID="imgReqDtTo" runat="server" AlternateText="Click to show calendar" CausesValidation="False"
                        Height="19px" ImageAlign="Middle" ImageUrl="~/images/AJAX/Calendar_scheduleHS.png"
                        Width="19px" />
                    <ajax:CalendarExtender ID="ceReqDtTo" runat="server" PopupButtonID="imgReqDtTo"
                        TargetControlID="txtReqDtTo">
                    </ajax:CalendarExtender>
                    <asp:RegularExpressionValidator ID="revReqDtTo" runat="server" ControlToValidate="txtReqDtTo"
                        ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format ' Font-Bold="True"
                        ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px"><</asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="rfvReqDtTo" runat="server" ControlToValidate="txtReqDtTo"
                        ErrorMessage="Request Date From is a required field." Font-Bold="False"><</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Server Name:
                </td>
                <td>
                    <asp:DropDownList ID="ddServerName" runat="server">
                        <asp:ListItem Text="ALL" Value="" />
                        <asp:ListItem Text="SQLCLUSTERVS" Value="SQLCLUSTERVS" />
                        <asp:ListItem Text="INVNOW05" Value="INVNOW05" />
                        <asp:ListItem Text="INVNOW06" Value="INVNOW06" />
                    </asp:DropDownList></td>
            </tr>
            
            <tr>
                <td>
                </td>
                <td colspan="3">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CommandName="submit" CausesValidation="true" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CommandName="reset" CausesValidation="False" />
                </td>
            </tr>
        </table>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
    </asp:Panel>
</asp:Content>
