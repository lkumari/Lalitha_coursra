<%@ Page Language="VB" MasterPageFile="~/crViewMasterPage.master" AutoEventWireup="false"
    CodeFile="crViewCostSavingsReport.aspx.vb" Inherits="RnD_crViewCostSavingsReport"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Label ID="lblErrors" runat="server" Text="Label" Visible="False" Width="696px"></asp:Label>
    <br />
    <CrystalRpt:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"
        ReportSourceID="CrystalReportSource1" ReuseParameterValuesOnRefresh="true" BestFitPage="false"
        BackColor="white" Width="1200px" Height="1000px" EnableDatabaseLogonPrompt="false"
        HasCrystalLogo="False" HasPageNavigationButtons="true" PrintMode="pdf" />
    <CrystalRpt:CrystalReportSource ID="CrystalReportSource1" runat="server">
        <Report FileName="RnD\Forms\crCostSavingsReport.rpt">
        </Report>
    </CrystalRpt:CrystalReportSource>
</asp:Content>
