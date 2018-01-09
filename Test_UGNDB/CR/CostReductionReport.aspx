<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="CostReductionReport.aspx.vb" Inherits="CR_CostReductionReport" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="1000px" DefaultButton="btnSubmit">
        <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
            Visible="False"></asp:Label>
        <table>
            <tr>
                <td class="p_smalltextbold" style="color: #990000">
                    Use the parameters below to filter the report.
                </td>
            </tr>
        </table>
        <hr />
        <br />
        <table width="80%" border="0">
        <tr>
        <td class="p_text">Select a report format:</td>
        <td> 
            <asp:DropDownList ID="ddReportFormat" runat="server">
                <asp:ListItem Value="Detail">Detail (includes status and step updates)
                </asp:ListItem>
                <asp:ListItem Value="Summary">Summary (without the status and step updates)</asp:ListItem>
                <asp:ListItem Value="Daily">Daily Summary (same format received in email)</asp:ListItem>
            </asp:DropDownList>
        </td>
        </tr>
            <tr>
                <td class="p_text">
                    Implementation Date From:
                </td>
                <td>
                    <asp:TextBox ID="txtImpDtFrom" runat="server" MaxLength="10" Width="80px" />&nbsp;<asp:ImageButton
                        ID="imgImpDtFrom" runat="server" AlternateText="Click to show calendar" CausesValidation="False"
                        Height="19px" ImageAlign="Middle" ImageUrl="~/images/AJAX/Calendar_scheduleHS.png"
                        Width="19px" />
                    <ajax:CalendarExtender ID="ceImpDtFrom" runat="server" PopupButtonID="imgImpDtFrom"
                        Format="MM/dd/yyyy" TargetControlID="txtImpDtFrom">
                    </ajax:CalendarExtender>
                    <asp:RegularExpressionValidator ID="revImpDtFrom" runat="server" ControlToValidate="txtImpDtFrom"
                        ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format ' Font-Bold="True"
                        ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px"><</asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Implementation Date To:
                </td>
                <td>
                    <asp:TextBox ID="txtImpDtTo" runat="server" MaxLength="10" Width="80px" />&nbsp;<asp:ImageButton
                        ID="imgImpDtTo" runat="server" AlternateText="Click to show calendar" CausesValidation="False"
                        Height="19px" ImageAlign="Middle" ImageUrl="~/images/AJAX/Calendar_scheduleHS.png"
                        Width="19px" />
                    <ajax:CalendarExtender ID="ceImpDtTo" runat="server" PopupButtonID="imgImpDtTo"
                        Format="MM/dd/yyyy" TargetControlID="txtImpDtTo">
                    </ajax:CalendarExtender>
                    <asp:RegularExpressionValidator ID="revImpDtTo" runat="server" ControlToValidate="txtImpDtTo"
                        ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format ' Font-Bold="True"
                        ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px"><</asp:RegularExpressionValidator>
                    <asp:CompareValidator ID="cvImpDt1" runat="server" ControlToCompare="txtImpDtFrom"
                        ControlToValidate="txtImpDtTo" ErrorMessage="Implementation Date From must be greater than or equal to Implementation Date To."
                        Operator="GreaterThanEqual" Type="Date"><</asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Project Leader:
                </td>
                <td>
                    <asp:DropDownList ID="ddLeader" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    UGN Facility:
                </td>
                <td>
                    <asp:DropDownList ID="ddUGNFacility" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="p_text" valign="top">
                    Commodity:
                </td>
                <td valign="top">
                    <asp:DropDownList ID="ddCommodity" runat="server" />
                    <br />
                    {Commodity / Classification}
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Customer:
                </td>
                <td>
                    <asp:DropDownList ID="ddCustomer" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Program:
                </td>
                <td>
                    <asp:DropDownList ID="ddProgram" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Project Category:
                </td>
                <td>
                    <asp:DropDownList ID="ddProjectCategory" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Reviewed By Plant Controller:
                </td>
                <td>
                    <asp:DropDownList ID="ddPCR" runat="server">
                        <asp:ListItem Value="0">No</asp:ListItem>
                        <asp:ListItem Value="1">Yes</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Sort Report By:
                </td>
                <td>
                    <asp:DropDownList ID="ddSortBy" runat="server">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Value="EstImpDate Desc">Implementation Date high to low</asp:ListItem>
                        <asp:ListItem Value="EstImpDate Asc">Implementation Date low to high</asp:ListItem>
                        <asp:ListItem Value="PrcntNearImpDate Desc">Project Timeline high to low</asp:ListItem>
                        <asp:ListItem Value="PrcntNearImpDate Asc">Project Timeline low to high</asp:ListItem>
                        <asp:ListItem Value="EstAnnualCostSave Desc">Annual Cost Save high to low</asp:ListItem>
                        <asp:ListItem Value="EstAnnualCostSave Asc">Annual Cost Save low to high</asp:ListItem>
                        <asp:ListItem Value="CapEx Desc">CAPEX high to low</asp:ListItem>
                        <asp:ListItem Value="CapEx Asc">CAPEX low to high</asp:ListItem>
                        <asp:ListItem Value="SuccessRate Desc">Success Rate high to low</asp:ListItem>
                        <asp:ListItem Value="SuccessRate Asc">Success Rate low to high</asp:ListItem>
                        <asp:ListItem Value="Rank Desc">Rank high to low</asp:ListItem>
                        <asp:ListItem Value="Rank Asc">Rank low to high</asp:ListItem>
                        <asp:ListItem Value="Completion Desc">Percent Completed high to low</asp:ListItem>
                        <asp:ListItem Value="Completion Asc">Percent Completed low to high</asp:ListItem>
                        <asp:ListItem Value="TotalPayback Desc">Payback high to low</asp:ListItem>
                        <asp:ListItem Value="TotalPayback Asc">Payback low to high</asp:ListItem>
                    </asp:DropDownList>
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
