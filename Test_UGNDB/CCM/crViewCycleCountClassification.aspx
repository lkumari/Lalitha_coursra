<%@ Page Language="VB" MasterPageFile="~/crViewMasterPage.master" AutoEventWireup="false"
    CodeFile="crViewCycleCountClassification.aspx.vb" Inherits="CCM_crCycleCounterMatrixDetail"
    Title="Untitled Page" %>


<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="1200px">
        <asp:Label ID="lblErrors" runat="server" Text="Label" Visible="False" Width="800px"></asp:Label>
        <br />
        <CrystalRpt:CrystalReportViewer ID="CrystalReportViewer1"  runat="server" AutoDataBind="true"
            ReportSourceID="CrystalReportSource1" ReuseParameterValuesOnRefresh="true" BestFitPage="false"
            BackColor="white" Width="1300px" Height="1000px" EnableDatabaseLogonPrompt="false" HasCrystalLogo="False"
            HasPageNavigationButtons="true" PrintMode="pdf" 
            HasToggleGroupTreeButton="False" PageZoomFactor="80" />
        <CrystalRpt:CrystalReportSource ID="CrystalReportSource1" runat="server">
            <Report FileName="CCM\Forms\crCCMClassification.rpt">
            </Report>
        </CrystalRpt:CrystalReportSource>
    </asp:Panel>
</asp:Content>
