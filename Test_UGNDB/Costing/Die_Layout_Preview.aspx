<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Die_Layout_Preview.aspx.vb"
    Inherits="Costing_Die_Layout_Preview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Die Layout Preview</title>
</head>

<body>
    <asp:Panel ID="localPanel" runat="server" Width="100%">
        <form id="form1" runat="server">
            <asp:Label ID="lblMessage" SkinID="MessageLabelSkin" runat="server"></asp:Label>           
            <CrystalRpt:CrystalReportViewer ID="crDieLayoutPreview" runat="server" ReportSourceID="crsDieLayout"
                HasZoomFactorList="True" AutoDataBind="True" EnableDatabaseLogonPrompt="False"
                ReuseParameterValuesOnRefresh="True" BestFitPage="False" Width="1200px" Height="1000px"
                PageZoomFactor="100" HasCrystalLogo="False" HasToggleGroupTreeButton="False"
                HasViewList="False" />
            <CrystalRpt:CrystalReportSource ID="crsDieLayout" runat="server">
                <Report FileName="Costing\Forms\DieLayout.rpt">
                </Report>
            </CrystalRpt:CrystalReportSource>
        </form>
    </asp:Panel>
</body>
</html>
