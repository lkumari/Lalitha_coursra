<%@ Page Language="VB" MasterPageFile="~/crViewMasterPage.master" AutoEventWireup="false"
    CodeFile="crViewTestReportCoverSheet.aspx.vb" Inherits="RnD_crViewTestReportCoverSheet"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Label ID="lblErrors" runat="server" Text="Label" Visible="False" Width="696px" />
    <br />
    <CrystalRpt:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
        ReportSourceID="CrystalReportSource1" ReuseParameterValuesOnRefresh="True" BestFitPage="False"
        BackColor="White" Width="1000px" Height="1300px" DisplayGroupTree="False" EnableDatabaseLogonPrompt="False"
        HasGotoPageButton="False" HasSearchButton="False" PageZoomFactor="120" HasCrystalLogo="False"
        HasDrillUpButton="False" HasPageNavigationButtons="True" HasToggleGroupTreeButton="true"
        HasViewList="False" />
    <CrystalRpt:CrystalReportSource ID="CrystalReportSource1" runat="server">
        <Report FileName="RnD\Forms\crTestReportCoverSheet.rpt">
        </Report>
    </CrystalRpt:CrystalReportSource>
</asp:Content>
