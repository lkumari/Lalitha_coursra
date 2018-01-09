<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Cost_Sheet_Preview.aspx.vb"
    Inherits="Costing_Cost_Sheet_Preview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Cost Form Preview</title>
</head>
<body>
    <asp:Panel ID="localPanel" runat="server" Width="100%">
        <form id="form1" runat="server">
        <asp:Label runat="server" ID="lblFileName" Text="File Name:" Visible="false"></asp:Label>
        <asp:TextBox runat="server" ID="txtFileName" MaxLength="50" Width="300px" Visible="false"></asp:TextBox>
        <asp:Button runat="server" ID="btnCreate" Text="Create File" Visible="false" />
        <br />
        <asp:Label ID="lblMessage" SkinID="MessageLabelSkin" runat="server"></asp:Label>
        <CrystalRpt:CrystalReportViewer runat="server" ID="crCostFormPreview" />
        </form>
    </asp:Panel>
</body>
</html>
