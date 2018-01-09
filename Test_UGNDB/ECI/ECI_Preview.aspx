<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ECI_Preview.aspx.vb" Inherits="ECI_ECI_Preview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ECI Preview</title>
</head>

<body>
    <asp:Panel ID="localPanel" runat="server" Width="100%">
        <asp:Label runat="server" ID="lblMessage" SkinID="MessageLabelSkin"></asp:Label>
        <form id="form1" runat="server">            
            
        </form>
    </asp:Panel>
</body>
</html>
