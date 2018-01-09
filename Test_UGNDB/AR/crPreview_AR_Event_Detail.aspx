<%@ Page Language="VB" AutoEventWireup="false" CodeFile="crPreview_AR_Event_Detail.aspx.vb"
    Inherits="crPreview_AR_Event_Detail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AR Event Preview</title>
</head>
<body>
    <asp:Panel ID="localPanel" runat="server" Width="100%">
        <form id="form1" runat="server">
            <asp:Label runat="server" ID="lblMessage" SkinID="MessageLabelSkin"></asp:Label>
            <CrystalRpt:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
                ReportSourceID="CrystalReportSource1" ReuseParameterValuesOnRefresh="True" BestFitPage="false"
                BackColor="White" Width="1000px" Height="1350px" EnableDatabaseLogonPrompt="False"
                HasCrystalLogo="False" HasPageNavigationButtons="True" DisplayGroupTree="False"
                HasSearchButton="False" HasToggleGroupTreeButton="False" PageZoomFactor="125"
                HyperlinkTarget="_blank" HasDrillUpButton="False" />
            <CrystalRpt:CrystalReportSource ID="CrystalReportSource1" runat="server">
                <Report FileName="AR\Forms\crAREvent.rpt">
                </Report>
            </CrystalRpt:CrystalReportSource>
        </form>
    </asp:Panel>
</body>
</html>
