<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DMSDrawingEditBOM.aspx.vb"
    Inherits="DMSDrawingEditBOM" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit BOM of Drawing</title>

    <script type="text/javascript" language="javascript">
        //counter below used for limit users to enter a set number of characters in textboxes.
        function tbLimit() {
            var tbObj = event.srcElement;
            if (tbObj.value.length == tbObj.maxLength * 1) return false;
        }
        function tbCount(visCnt) {
            var tbObj = event.srcElement;
            if (tbObj.value.length > tbObj.maxLength * 1) tbObj.value = tbObj.value.substring(0, tbObj.maxLength * 1);
            if (visCnt) visCnt.innerText = tbObj.maxLength - tbObj.value.length + " char(s) remaining.";
        }   
    </script>

</head>
<body onbeforeunload="javascript:window.opener.__doPostBack('ctl00$maincontent$lnkViewBOMTree','');">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSaveSubDrawing">
        <form id="form1" runat="server">
        <br />
        <br />
        <br />
        <h1>
            Edit DMS Bill of Material Details</h1>
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
                    Child Drawing:
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
                <td class="p_text">
                    Sub-DrawingNo:
                    <asp:ImageButton ID="iBtnIncSubDrawing" runat="server" CausesValidation="true" ImageUrl="~/images/up.jpg"
                        ToolTip="Increment Sub-Drawing Revision" AlternateText="Inc Sub-Drawing Rev."
                        ValidationGroup="vgSubDrawing" Visible="false" />
                    <asp:ImageButton ID="iBtnDecSubDrawing" runat="server" CausesValidation="true" ImageUrl="~/images/down.jpg"
                        ToolTip="Decrement Sub-Drawing Revision" AlternateText="Dec Sub-Drawing Rev."
                        ValidationGroup="vgSubDrawing" Visible="false" />
                </td>
                <td colspan="3">
                    <asp:TextBox runat="server" ID="txtSubDrawingNo" MaxLength="18"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvSubDrawingNo" runat="server" ControlToValidate="txtSubDrawingNo"
                        ErrorMessage="Sub Drawing is Required for B.O.M." Font-Bold="True" ValidationGroup="vgSubDrawing"
                        Text="<" SetFocusOnError="true">				                                                            
                    </asp:RequiredFieldValidator>
                    <asp:ImageButton ID="ibtnSearchSubDrawing" runat="server" CausesValidation="False"
                        ImageUrl="~/images/Search.gif" ToolTip="Search Sub-Drawing No." AlternateText="Search Sub-Drawing No." />
                    <asp:HyperLink ID="lnkViewSubDrawing" runat="server" Target="_blank" Text="View"
                        Visible="false"></asp:HyperLink>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    CAD Available:
                </td>
                <td colspan="3">
                    <asp:CheckBox runat="server" ID="cbSubDrawingCADAvailable" Enabled="false" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Quantity:
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtSubDrawingQuantity" runat="server" MaxLength="10"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvSubDrawingQuantity" runat="server" ControlToValidate="txtSubDrawingQuantity"
                        ErrorMessage="Quantity is Required for Sub-Drawing." Font-Bold="True" ValidationGroup="vgSubDrawing"
                        Text="<" SetFocusOnError="true"> </asp:RequiredFieldValidator>
                    <asp:CompareValidator runat="server" ID="cvSubDrawingQuantity" Operator="DataTypeCheck"
                        ValidationGroup="vgSubDrawing" Type="Double" Text="<" ControlToValidate="txtSubDrawingQuantity"
                        ErrorMessage="Sub-Drawing Quantity must be numeric." SetFocusOnError="True" />
                </td>
            </tr>
            <tr>
                <td class="p_text" valign="top">
                    Notes:
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtSubDrawingNotes" runat="server" TextMode="MultiLine" Rows="2"
                        Width="400px">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvSubDrawingNotes" runat="server" ControlToValidate="txtSubDrawingNotes"
                        ErrorMessage="Notes are required for Sub-Drawing" ValidationGroup="SubDrawing"
                        Text="<" SetFocusOnError="true"></asp:RequiredFieldValidator>
                    <br />
                    <asp:Label ID="lblSubDrawingNotesCharCount" SkinID="MessageLabelSkin" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="p_text" valign="top">
                    Process:
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtSubDrawingProcess" runat="server" TextMode="MultiLine" Rows="2"
                        Width="400px">
                    </asp:TextBox>
                    <br />
                    <asp:Label ID="lblSubDrawingProcessCharCount" SkinID="MessageLabelSkin" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="p_text" valign="top">
                    Process Parameters:
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtSubDrawingProcessParameters" runat="server" TextMode="MultiLine"
                        Rows="2" Width="400px">
                    </asp:TextBox>
                    <br />
                    <asp:Label ID="lblSubDrawingProcessParametersCharCount" SkinID="MessageLabelSkin"
                        runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center">
                    <asp:Button runat="server" ID="btnSaveSubDrawing" Text="Update SubDrawing" CausesValidation="true"
                        ValidationGroup="vgSubDrawing" />
                    <button onclick="javascript:window.open('','_self','');window.close();">
                        Close</button>
                </td>
            </tr>
        </table>
        </form>
    </asp:Panel>
</body>
</html>
