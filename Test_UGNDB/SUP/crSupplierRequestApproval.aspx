<%@ Page Language="VB" MasterPageFile="~/crViewMasterPage.master" AutoEventWireup="false"
    CodeFile="crSupplierRequestApproval.aspx.vb" Inherits="SUP_crViewSupplierRequest"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="1000px">
        <table width="90%">
            <tr>
                <td class="c_text" colspan="4">
                    <b>Review the information and submit your response in the section provided.</b>
                </td>
            </tr>
            <tr>
                <td class="p_text" style="height: 17px">
                    Team Member:
                </td>
                <td style="height: 17px">
                    <asp:Label ID="lblTeamMbr" runat="server" Text="" CssClass="c_text" Style="width: 243px;
                        color: #990000;"></asp:Label>
                    <asp:HiddenField ID="hfSeqNo" runat="server" />
                </td>
                <td class="p_text" rowspan="3" style="vertical-align: top">
                    <asp:Label ID="ReqComments" runat="server" Font-Bold="True" ForeColor="Red" Text="*"
                        Visible="false" />
                    Comments:
                </td>
                <td rowspan="3">
                    &nbsp;<asp:TextBox ID="txtComments" runat="server" MaxLength="200" Rows="3" TextMode="MultiLine"
                        Width="350px"></asp:TextBox><br />
                    <asp:Label ID="lblComments" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>&nbsp;
                </td>
            </tr>
            <tr>
                <td class="p_text" style="vertical-align: top">
                    Date Notified:
                </td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblDateNotified" runat="server" Text="" CssClass="c_text" Style="width: 243px;
                        color: #990000;"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Status:
                </td>
                <td style="vertical-align: top">
                    <asp:DropDownList ID="ddStatus" runat="server">
                        <asp:ListItem>Pending</asp:ListItem>
                        <asp:ListItem>Approved</asp:ListItem>
                        <asp:ListItem>Rejected</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <% If ViewState("Admin") = True Then%>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblInBPCS" runat="server" Font-Bold="True" ForeColor="Red" Text="*" />
                    Supplier Created in Oracle:
                </td>
                <td>
                    <asp:DropDownList ID="ddInBPCS" runat="server">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Value="False">No</asp:ListItem>
                        <asp:ListItem Value="True">Yes</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;<asp:RequiredFieldValidator ID="rfvInBPCS" runat="server" ControlToValidate="ddInBPCS"
                        ErrorMessage="Supplier Created on BPCS is a required field." Font-Bold="False"
                        ValidationGroup="vsDetail" Enabled="false"><</asp:RequiredFieldValidator><asp:CheckBox ID="cbTen99" runat="server" Text="1099?" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblVendorNo" runat="server" Font-Bold="True" ForeColor="Red" Text="*" />
                    Supplier No. Assigned:
                </td>
                <td>
                    <asp:TextBox ID="txtVendorNo" runat="server" MaxLength="10" Width="100px" />&nbsp;<asp:RequiredFieldValidator
                        ID="rfvVendorNo" runat="server" ControlToValidate="txtVendorNo" ErrorMessage="Supplier No Assigned is a required field."
                        Font-Bold="False" ValidationGroup="vsDetail" Enabled="false"><</asp:RequiredFieldValidator></td>
            </tr>
            <% End IF %>
            <tr>
                <td style="height: 28px">
                </td>
                <td colspan="3" style="height: 28px">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClientClick="return confirm('Are you sure you want to submit your response?');"
                        CausesValidation="true" ValidationGroup="vsDetail" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" />&nbsp;
                </td>
            </tr>
        </table>
        <asp:ValidationSummary ID="sDetail" runat="server" ValidationGroup="vsDetail" ShowMessageBox="True">
        </asp:ValidationSummary>
        <br />
        <asp:Label ID="lblErrors" runat="server" Text="Label" Visible="False" Width="800px"
            CssClass="c_text" Font-Bold="True" ForeColor="Red"></asp:Label>
        <br />
    </asp:Panel>
    <br />
    <CrystalRpt:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
        ReportSourceID="CrystalReportSource1" ReuseParameterValuesOnRefresh="True" BestFitPage="false"
        BackColor="White" Width="980px" Height="1350px" EnableDatabaseLogonPrompt="False"
        HasCrystalLogo="False" HasPageNavigationButtons="True" DisplayGroupTree="False"
        HasSearchButton="False" HasToggleGroupTreeButton="False" PageZoomFactor="125"
        HyperlinkTarget="_blank" HasDrillUpButton="False" PrintMode="ActiveX" />
    <CrystalRpt:CrystalReportSource ID="CrystalReportSource1" runat="server">
        <Report FileName="SUP\Forms\crSupplierRequest.rpt">
        </Report>
    </CrystalRpt:CrystalReportSource>
</asp:Content>
