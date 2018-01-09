<%@ Page Language="VB" AutoEventWireup="false" CodeFile="UGN_IPP_Preview.aspx.vb"
    Inherits="UGN_IPP_Preview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>UGN IPP Preview</title>
</head>
<body>
    <asp:Panel ID="localPanel" runat="server" Width="100%">
        <asp:Label runat="server" ID="lblMessage" SkinID="MessageLabelSkin"></asp:Label>
        <form id="form1" runat="server">
        <CrystalRpt:CrystalReportViewer ID="crUgnIppPreview" runat="server" ReportSourceID="crsUgnIpp"
            AutoDataBind="True" ReuseParameterValuesOnRefresh="True" EnableDatabaseLogonPrompt="False"
            Width="1250px" Height="1100px" BestFitPage="False" PageZoomFactor="100" HasCrystalLogo="False"
            HasToggleGroupTreeButton="False" HasViewList="False" HasZoomFactorList="True" />
        <CrystalRpt:CrystalReportSource ID="crsUgnIpp" runat="server">
            <Report FileName="ECI\Forms\UgnIPP.rpt">
            </Report>
        </CrystalRpt:CrystalReportSource>
        </form>
    </asp:Panel>
</body>
</html>
