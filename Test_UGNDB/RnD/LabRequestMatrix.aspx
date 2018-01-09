<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="LabRequestMatrix.aspx.vb" Inherits="RnD_LabRequestMatrix" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="1000px" DefaultButton="btnSubmit">
        <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
            Visible="False"></asp:Label>
        <hr />
        <br />
        <table width="40%" border="0">
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblSampleProdDesc" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="* " />
                    Request Date From:
                </td>
                <td>
                    <asp:TextBox ID="txtReqDtFrom" runat="server" MaxLength="10" Width="80px" />
                    <ajax:FilteredTextBoxExtender ID="rtbReqDtFrom" runat="server" TargetControlID="txtReqDtFrom"
                        FilterType="Custom, Numbers" ValidChars="/" />
                    &nbsp;<asp:ImageButton ID="imgReqDtFrom" runat="server" AlternateText="Click to show calendar"
                        CausesValidation="False" Height="19px" ImageAlign="Middle" ImageUrl="~/images/AJAX/Calendar_scheduleHS.png"
                        Width="19px" />
                    <ajax:CalendarExtender ID="ceReqDtFrom" runat="server" PopupButtonID="imgReqDtFrom"
                        TargetControlID="txtReqDtFrom" />
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
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                    Request Date To:
                </td>
                <td>
                    <asp:TextBox ID="txtReqDtTo" runat="server" MaxLength="10" Width="80px" />
                    <ajax:FilteredTextBoxExtender ID="ftbReqDtTo" runat="server" TargetControlID="txtReqDtTo"
                        FilterType="Custom, Numbers" ValidChars="/" />
                    &nbsp;<asp:ImageButton ID="imgReqDtTo" runat="server" AlternateText="Click to show calendar"
                        CausesValidation="False" Height="19px" ImageAlign="Middle" ImageUrl="~/images/AJAX/Calendar_scheduleHS.png"
                        Width="19px" />
                    <ajax:CalendarExtender ID="ceReqDtTo" runat="server" PopupButtonID="imgReqDtTo" TargetControlID="txtReqDtTo" />
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
                    UGN Location:
                </td>
                <td>
                    <asp:DropDownList ID="ddUGNFacility" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="p_text" style="height: 26px">
                    Request Status:
                </td>
                <td style="color: #990000; height: 26px" class="c_text">
                    <asp:DropDownList ID="ddRequestStatus" runat="server">
                        <asp:ListItem Selected="True"></asp:ListItem>
                        <asp:ListItem>Unassigned</asp:ListItem>
                        <asp:ListItem>Abandoned</asp:ListItem>
                        <asp:ListItem>Completed</asp:ListItem>
                        <asp:ListItem>Nearly Complete</asp:ListItem>
                        <asp:ListItem>On Hold</asp:ListItem>
                        <asp:ListItem>Outstanding</asp:ListItem>
                        <asp:ListItem>Overdue</asp:ListItem>
                        <asp:ListItem>Testing In Progress</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Testing Classification:
                </td>
                <td>
                    <asp:DropDownList ID="ddTestClass" runat="server" />
                </td>
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
