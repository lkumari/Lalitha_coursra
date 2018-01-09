<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RFD_Drawing_BOM_Selection.aspx.vb"
    MaintainScrollPositionOnPostback="true" Inherits="RFD_Drawing_BOM_Selection" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>RFD Finished Good DMS BOM Selection</title>
</head>
<body onbeforeunload="window.opener.document.location.href=window.opener.document.location.href;">
    <asp:Panel ID="localPanel" runat="server">
        <form id="form1" runat="server">
        <br />
        <br />
        <br />
        <br />
        <br />
        <asp:Label ID="lblMessage" SkinID="MessageLabelSkin" runat="server"></asp:Label>
        <asp:Label ID="lblShowMessage" runat="server" />
        <br />
        <asp:Label ID="lblWarning" runat="server"></asp:Label>
        <br />
        <asp:Button ID="btnAddToRFDChildParts" runat="server" Text="Add All Checked Items to RFD Child Part tab" />
        <asp:Button ID="btnSelectAll" runat="server" Text="Select All" />
        <asp:Button ID="btnUnselectAll" runat="server" Text="Unselect All" /><br />
        <br />
        <asp:TreeView ID="tvBOM" runat="server" ImageSet="Arrows" PathSeparator="|" ShowCheckBoxes="All">
            <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
            <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px"
                VerticalPadding="0px" />
            <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px"
                NodeSpacing="0px" VerticalPadding="0px" />
        </asp:TreeView>
        <br />
        <asp:Button ID="btnAddToRFDChildPartsBottom" runat="server" Text="Add All Checked Items to RFD Child Part tab" />
        <asp:Button ID="btnSelectAllBottom" runat="server" Text="Select All" />
        <asp:Button ID="btnUnselectAllBottom" runat="server" Text="Unselect All" />
        </form>
    </asp:Panel>
</body>
</html>
