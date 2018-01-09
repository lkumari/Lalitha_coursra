<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DMSDrawingDeleteBOM.aspx.vb"
    Inherits="DMSDrawingDeleteBOM" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Delete Sub-Drawing</title>
</head>
<body onbeforeunload="javascript:window.opener.__doPostBack('ctl00$maincontent$lnkViewBOMTree','');">
    <form id="form1" runat="server">
    <br />
    <br />
    <br />
    <h1>
        Are you sure you would like to delete Sub-Drawing from the BOM of the parent drawing?
    </h1>
    <br />
    <asp:Label ID="lblMessage" SkinID="MessageLabelSkin" runat="server"></asp:Label>
    <br />
    <asp:ValidationSummary ID="vsSubDrawing" runat="server" ShowMessageBox="True" ShowSummary="true"
        ValidationGroup="vgSubDrawing" />
    <br />
    <table>
        <tr>
            <td class="p_text">
                Parent Drawing:
            </td>
            <td class="p_textbold">
                <asp:Label ID="lblParentDrawingNo" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="p_text">
                Remove Child Drawing:
            </td>
            <td class="p_textbold">
                <asp:Label ID="lblChildDrawingNo" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="c_textbold" colspan="2">
                <asp:Label ID="lblChildDrawingName" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <table>
        <tr>
            <td valign="top">
                <asp:Label runat="server" ID="lblAppendRevisionNotes" Text="Edit Notes (to be appended to Revision Notes):"
                    Visible="false"></asp:Label>
                &nbsp;
                <asp:TextBox runat="server" ID="txtAppendRevisionNotes" MaxLength="100" Visible="false"
                    Width="300px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvAppendRevisionNotes" runat="server" ControlToValidate="txtAppendRevisionNotes"
                    Text="<" ErrorMessage="You are editing an issued drawing. Please enter some notes for editing. They will be appended to the revision notes."
                    SetFocusOnError="true" ValidationGroup="vgSubDrawing" Enabled="false" />
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td colspan="4" align="center">
                <asp:Button runat="server" ID="btnRemoveSubDrawing" Text="Yes Remove Sub-Drawing"
                    CausesValidation="true" ValidationGroup="vgSubDrawing" />
                <button onclick="javascript:window.open('','_self','');window.close();">
                    Close</button>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
