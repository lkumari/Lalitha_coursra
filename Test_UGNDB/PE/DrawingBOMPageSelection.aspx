<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DrawingBOMPageSelection.aspx.vb" 
 MaintainScrollPositionOnPostback="true" Inherits="PE_DrawingBOMPageSelection" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <asp:Panel ID="localPanel" runat="server">
        <form id="form1" runat="server">
            <br />
            <asp:Label ID="lblMessage" SkinID="MessageLabelSkin" runat="server"></asp:Label>
            <asp:Label ID="lblShowMessage" runat="server" />
            <br />
            <asp:Label ID="lblWarning" runat="server"></asp:Label>
            <br />
            <asp:Button ID="btnPrintPreview" runat="server" Text="Preview All Checked Items" />
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
            <asp:Button ID="btnPrintPreviewBottom" runat="server" Text="Preview All Checked Items" />
            <asp:Button ID="btnSelectAllBottom" runat="server" Text="Select All" />
            <asp:Button ID="btnUnselectAllBottom" runat="server" Text="Unselect All" />
        </form>
    </asp:Panel>
</body>
</html>
