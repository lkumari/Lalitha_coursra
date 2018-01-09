<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="SpendingRequestReport.aspx.vb" Inherits="EXP_SpendingRequestReport"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="1000px" DefaultButton="btnSubmit">
        <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
            Visible="False"></asp:Label>
        <hr />
        <br />
        <table width="60%" border="0">
            <tr>
                <td colspan="2" class="c_text">
                    <b><i>Filter Options:</i></b>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblSRType" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                    <asp:Label ID="lblSpendingRequest" runat="server" Text="Spending Request:" />
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddSRType" runat="server">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Value="Assets">A - Property Plant Equipment</asp:ListItem>
                        <asp:ListItem Value="Development">D - Development Projects</asp:ListItem>
                        <asp:ListItem Value="Packaging">P - Packaging Expenditure</asp:ListItem>
                        <asp:ListItem Value="Repair">R - Repair Projects</asp:ListItem>
                        <asp:ListItem Value="Tooling">T - Customer Owned Tooling</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvSRType" runat="server" ControlToValidate="ddSRType"
                        ErrorMessage="Spending Request is a required field." Font-Bold="False" ValidationGroup="vsReport"><</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblUGNLocation" runat="server" Text="UGN Location:" />
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddUGNFacility" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblProjectStatus" runat="server" Text="Project Status:" />
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddProjectStatus" runat="server" AutoPostBack="true">
                        <asp:ListItem Text="ALL" Value="" />
                        <asp:ListItem Text="Approved" Value="Approved" />
                        <asp:ListItem Text="Capitalized" Value="Capitalized" />
                        <asp:ListItem Text="Closed" Value="Closed" />
                        <asp:ListItem Text="Hold" Value="Hold" />
                        <asp:ListItem Text="In Process" Value="In Process" />
                        <asp:ListItem Text="Rejected" Value="Rejected" />
                        <asp:ListItem Text="Pending Submission" Value="Pending Submission" />
                        <asp:ListItem Text="Tooling Completed" Value="Tooling Completed" />
                        <asp:ListItem Text="Void" Value="Void" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblFromDate" runat="server" Text="From Date:" Visible="false" />
                </td>
                <td>
                    <asp:TextBox ID="txtFromDate" runat="server" MaxLength="12" Width="80px" Visible="false" />
                    <ajax:FilteredTextBoxExtender ID="ftbeFromDate" runat="server" TargetControlID="txtFromDate"
                        FilterType="Custom, Numbers" ValidChars="/" />
                    <asp:ImageButton runat="server" ID="imgFromDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                        CausesValidation="False" Visible="false" />
                    <ajax:CalendarExtender ID="cbeFromDate" runat="server" TargetControlID="txtFromDate"
                        PopupButtonID="imgFromDate" Format="MM/dd/yyyy" />
                    <asp:RegularExpressionValidator ID="revFromDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtFromDate" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px" ValidationGroup="vsReport"><</asp:RegularExpressionValidator>
                    <asp:CompareValidator ID="cvFromDate" runat="server" ErrorMessage="From Date must be less than To Date."
                        ControlToCompare="txtToDate" ControlToValidate="txtFromDate" Operator="LessThanEqual"
                        Type="Date" ValidationGroup="vsReport"><</asp:CompareValidator>
                </td>
                <td class="p_text">
                    <asp:Label ID="lblToDate" runat="server" Text="To Date:" Visible="false" />
                </td>
                <td>
                    <asp:TextBox ID="txtToDate" runat="server" MaxLength="12" Width="80px" Visible="false" />
                    <ajax:FilteredTextBoxExtender ID="ftbeToDate" runat="server" TargetControlID="txtToDate"
                        FilterType="Custom, Numbers" ValidChars="/" />
                    <asp:ImageButton runat="server" ID="imgToDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                        CausesValidation="False" Visible="false" />
                    <ajax:CalendarExtender ID="cbeToDate" runat="server" TargetControlID="txtToDate"
                        PopupButtonID="imgToDate" Format="MM/dd/yyyy" />
                    <asp:RegularExpressionValidator ID="revToDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtToDate" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px" ValidationGroup="vsReport"><</asp:RegularExpressionValidator>
                    <asp:CompareValidator ID="cvToDate" runat="server" ControlToCompare="txtFromDate"
                        ControlToValidate="txtToDate" ErrorMessage="To Date must be greater than From Date."
                        Operator="GreaterThanEqual" Type="Date" ValidationGroup="vsReport"><</asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="3">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CommandName="submit" CausesValidation="true" ValidationGroup="vsReport"/>
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CommandName="reset" CausesValidation="False" />
                </td>
            </tr>
        </table>
        <asp:ValidationSummary ID="vsReport" runat="server" ValidationGroup="vsReport" />
    </asp:Panel>
</asp:Content>
