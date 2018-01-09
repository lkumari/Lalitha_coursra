<%@ Page Language="VB" AutoEventWireup="false" CodeFile="crViewARDeduction.aspx.vb"
    Inherits="AR_crViewARDeduction" Title="Untitled Page" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <asp:Panel ID="localPanel" runat="server" Width="100%">
        <asp:Label ID="lblErrors" runat="server" Text="Label" Visible="False" Width="800px"></asp:Label>
        <br />
        <CrystalRpt:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
            ReportSourceID="CrystalReportSource1" ReuseParameterValuesOnRefresh="True" BestFitPage="false"
            BackColor="White" Width="1000px" Height="1350px" EnableDatabaseLogonPrompt="False"
            HasCrystalLogo="False" HasPageNavigationButtons="True" DisplayGroupTree="False"
            HasSearchButton="False" HasToggleGroupTreeButton="False" PageZoomFactor="125"
            HyperlinkTarget="_blank" HasDrillUpButton="False" />
        <CrystalRpt:CrystalReportSource ID="CrystalReportSource1" runat="server">
            <Report FileName="AR\Forms\crARDeduction.rpt">
            </Report>
        </CrystalRpt:CrystalReportSource>
    </asp:Panel>
</body>
</html>
