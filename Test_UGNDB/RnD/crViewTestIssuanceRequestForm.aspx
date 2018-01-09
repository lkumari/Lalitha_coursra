<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="crViewTestIssuanceRequestForm.aspx.vb" Inherits="RnD_crViewTestIssuanceRequestForm"
    Title="Untitled Page" EnableSessionState="ReadOnly" %>
   
<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Label ID="lblErrors" runat="server" Text="Label" Visible="False" Width="696px"></asp:Label>
    <br />
    <CrystalRpt:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
        ReportSourceID="CrystalReportSource1" ReuseParameterValuesOnRefresh="True" BestFitPage="False"
        BackColor="White" Width="1000px" Height="1300px" DisplayGroupTree="False" EnableDatabaseLogonPrompt="False"
        HasGotoPageButton="False" HasSearchButton="False" PageZoomFactor="120" HasCrystalLogo="False"
        HasDrillUpButton="False" HasPageNavigationButtons="True" HasToggleGroupTreeButton="False"
        HasViewList="False" />
    <CrystalRpt:CrystalReportSource ID="CrystalReportSource1" runat="server">
        <Report FileName="RnD\Forms\crTestIssuanceRequestForm.rpt">
        </Report>
    </CrystalRpt:CrystalReportSource>
</asp:Content>
