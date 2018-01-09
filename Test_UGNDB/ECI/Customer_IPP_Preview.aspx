<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Customer_IPP_Preview.aspx.vb"
    Inherits="Customer_IPP_Preview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Customer IPP Preview</title>
</head>
<body>
    <asp:Panel ID="localPanel" runat="server" Width="100%">
        <asp:Label runat="server" ID="lblMessage" SkinID="MessageLabelSkin"></asp:Label>
        <form id="form1" runat="server">
        <CrystalRpt:CrystalReportViewer ID="crCustomerIppPreview" runat="server" ReportSourceID="crsCustomerIPP"
            AutoDataBind="True" ReuseParameterValuesOnRefresh="True" EnableDatabaseLogonPrompt="False"
            Width="1250px" Height="1100px" BestFitPage="False" PageZoomFactor="100" HasCrystalLogo="False"
            HasToggleGroupTreeButton="False" HasViewList="False" HasZoomFactorList="True" />
        <CrystalRpt:CrystalReportSource ID="crsCustomerIPP" runat="server">
            <Report FileName="ECI\Forms\CustomerIPP.rpt">
            </Report>
        </CrystalRpt:CrystalReportSource>
        </form>
    </asp:Panel>
</body>
</html>
