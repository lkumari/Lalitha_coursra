<%@ Page Language="VB" MasterPageFile="~/crViewMasterPage.master" AutoEventWireup="false"
    CodeFile="crViewSpendingRequestReport.aspx.vb" Inherits="EXP_crViewSpendingRequestReport"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Label ID="lblErrors" runat="server" Text="Label" Visible="False" Width="696px"></asp:Label>
    <br />
    <CrystalRpt:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
        DisplayGroupTree="False" ReportSourceID="CrystalReportSource1" ReuseParameterValuesOnRefresh="True"
        BackColor="White" Width="350px" Height="50px" EnableDatabaseLogonPrompt="False"
        HasCrystalLogo="False" HasPageNavigationButtons="True" />
    <CrystalRpt:CrystalReportSource ID="CrystalReportSource1" runat="server">
        <Report FileName="EXP\Forms\crSpendingRequestReport.rpt">
        </Report>
    </CrystalRpt:CrystalReportSource>
</asp:Content>
