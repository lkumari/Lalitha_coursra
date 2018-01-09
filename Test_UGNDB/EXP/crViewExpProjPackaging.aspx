<%@ Page Language="VB" MasterPageFile="~/crViewMasterPage.master" AutoEventWireup="false"
    CodeFile="crViewExpProjPackaging.aspx.vb" Inherits="EXP_crViewExpProjPackaging"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Label ID="lblErrors" runat="server" Text="Label" Visible="False" Width="800px"></asp:Label>
    <br />
    <CrystalRpt:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
        ReportSourceID="CrystalReportSource1" ReuseParameterValuesOnRefresh="True" BestFitPage="false"
        BackColor="White" Width="1000px" Height="1350px" EnableDatabaseLogonPrompt="False"
        HasCrystalLogo="False" HasPageNavigationButtons="True" DisplayGroupTree="False"
        HasSearchButton="False" HasToggleGroupTreeButton="False" PageZoomFactor="125"
        HyperlinkTarget="_blank" HasDrillUpButton="False" />
    <CrystalRpt:CrystalReportSource ID="CrystalReportSource1" runat="server">
        <Report FileName="EXP\Forms\crExpProjPackaging.rpt">
        </Report>
    </CrystalRpt:CrystalReportSource>
</asp:Content>
