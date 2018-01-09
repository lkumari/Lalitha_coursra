<%@ Page Language="VB" MasterPageFile="~/crViewMasterPage.master" AutoEventWireup="false"
    CodeFile="crViewCostReductionDetail.aspx.vb" Inherits="CR_crViewCostReductionDetail"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
    <br />
    <CrystalRpt:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"
        ReportSourceID="CrystalReportSource1" ReuseParameterValuesOnRefresh="true" BestFitPage="false"
        BackColor="white" Width="1200px" Height="1000px" EnableDatabaseLogonPrompt="false"
        HasCrystalLogo="False" HasPageNavigationButtons="true" PrintMode="pdf"  HasToggleGroupTreeButton="False" />
    <CrystalRpt:CrystalReportSource ID="CrystalReportSource1" runat="server">
        <Report FileName="CR\Forms\crCostReductionDetail.rpt">
        </Report>
    </CrystalRpt:CrystalReportSource>
</asp:Content>
