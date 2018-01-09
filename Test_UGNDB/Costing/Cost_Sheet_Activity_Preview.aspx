<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Cost_Sheet_Activity_Preview.aspx.vb"
    Inherits="Cost_Sheet_Activity_Preview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Team Member Turn Around Time Activity Report</title>
</head>
<body>
    <asp:Panel ID="localPanel" runat="server">
        <form id="form1" runat="server">
            <asp:Label ID="lblMessage" SkinID="MessageLabelSkin" runat="server"></asp:Label>
            <CrystalRpt:CrystalReportViewer ID="crActityReportPreview" runat="server" Height="1000" ReportSourceID="crsActivityReport"
                AutoDataBind="True" EnableDatabaseLogonPrompt="False" ReuseParameterValuesOnRefresh="True"
                Width="1200" HasCrystalLogo="False" HasToggleGroupTreeButton="False" HasViewList="False"
                Visible="false" BestFitPage="False" />
            <CrystalRpt:CrystalReportSource ID="crsActivityReport" runat="server">
                <Report FileName="Costing\Forms\TeamMemberTurnAroundTimeSummary.rpt">
                </Report>
            </CrystalRpt:CrystalReportSource>
        </form>
    </asp:Panel>
</body>
</html>
