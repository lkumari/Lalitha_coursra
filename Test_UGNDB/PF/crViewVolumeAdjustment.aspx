<%@ Page Language="VB" MasterPageFile="~/crViewMasterPage.master" AutoEventWireup="false"
    CodeFile="crViewVolumeAdjustment.aspx.vb" Inherits="PF_crViewVolumeAdjustment"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Label ID="lblErrors" runat="server" Text="Label" Visible="False" Width="696px"></asp:Label>
    <br />
    <CrystalRpt:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
        ReportSourceID="CrystalReportSource1" ReuseParameterValuesOnRefresh="True" BestFitPage="False"
        BackColor="White" Width="1400px" Height="1000px" EnableDatabaseLogonPrompt="False"
        HasCrystalLogo="False" HasPageNavigationButtons="True" PrintMode="Pdf" DisplayGroupTree="False"
        PageZoomFactor="200" />
    <CrystalRpt:CrystalReportSource ID="CrystalReportSource1" runat="server">
        <Report FileName="PF\Forms\crVolumeAdjustment.rpt">
        </Report>
    </CrystalRpt:CrystalReportSource>
</asp:Content>
