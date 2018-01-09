<%@ Page Language="VB" MasterPageFile="~/crViewMasterPage.master" AutoEventWireup="false"
    CodeFile="crViewDatabaseGrowthTracking.aspx.vb" Inherits="RnD_crViewDatabaseGrowthTracking"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Label ID="lblErrors" runat="server" Text="Label" Visible="False" Width="696px"></asp:Label>
    <br />
    <CrystalRpt:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
        ReportSourceID="CrystalReportSource1" ReuseParameterValuesOnRefresh="True" BestFitPage="False"
        BackColor="White" Width="1200px" Height="1250px" EnableDatabaseLogonPrompt="False"
        HasCrystalLogo="False" HasPageNavigationButtons="True" PrintMode="Pdf" PageZoomFactor="120" />
    <CrystalRpt:CrystalReportSource ID="CrystalReportSource1" runat="server">
        <Report FileName="DBA_Workspace\Reports\crDatabaseGrowthTracking.rpt">
        </Report>
    </CrystalRpt:CrystalReportSource>
</asp:Content>
