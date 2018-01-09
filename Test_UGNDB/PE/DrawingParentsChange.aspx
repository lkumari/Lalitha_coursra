<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="DrawingParentsChange.aspx.vb" Inherits="DrawingParentsChange" Title="Drawing Parents Change" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server">
        <h1 style="color:Red">Please note: If you make a change to the drawing number dropdown boxes at the top of this screen, the bottom of the screen will refresh and re-check all.</h1>
        <table cellpadding="3" cellspacing="3" style="background-color: White;" width="98%">
            <tr>
                <td colspan="4">
                    <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Current Child Revision
                </td>
                <td colspan="3">
                    <asp:DropDownList runat="server" ID="ddCurrentChildDrawingNo" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    New Child Revision:
                </td>
                <td >
                    <asp:DropDownList runat="server" ID="ddNewChildDrawingNo" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
                <td>
                    Alternative Drawing:
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtAltDrawingNo" MaxLength="20"></asp:TextBox>
                    <asp:ImageButton ID="iBtnAltDrawingNoSearch" runat="server" ImageUrl="~/images/Search.gif"
                        ToolTip="Click here to search for a DMS Drawing." />
                    <asp:HyperLink runat="server" ID="hlnkAltDrawingNo" Visible="false" Font-Underline="true"
                        ToolTip="Click here to view the alternative DMS Drawing." Text="View Drawing"
                        Target="_blank"></asp:HyperLink>                   
                </td>
            </tr>
            <tr>
                <td>
                    Release Type for the New Parent Drawings that will be generated:
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddReleaseType" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvReleaseType" runat="server" ControlToValidate="ddReleaseType"
                        ErrorMessage="Release Type is required." Font-Bold="True" SetFocusOnError="True"
                        ValidationGroup="vgSave"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblParentNewRevisionNotes" runat="server" Text="Add Revision Notes that will be added to each selected parent:"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox runat="server" ID="txtParentNewRevisionNotes" MaxLength="100" Width="400px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvParentNewRevisionNotes" runat="server" ControlToValidate="txtParentNewRevisionNotes"
                        ErrorMessage="New revision notes are required." Font-Bold="True" SetFocusOnError="True"
                        ValidationGroup="vgSave"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblGrandParentNewRevisionNotes" runat="server" Text="Add Revision Notes that will be added to each selected grandparent:"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox runat="server" ID="txtGrandParentNewRevisionNotes" MaxLength="100" Width="400px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td  colspan="2">
                    <asp:Label ID="lblCheckInstructions" runat="server" Text="Check each box to select:"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="4">
                    <br />
                    <asp:Label runat="server" ID="lblNote1" Text="Below is a list of parent drawings that use the CURRENT Child Revision."
                        SkinID="MessageLabelSkin"></asp:Label>
                    <br />
                    <br />
                    <asp:TreeView runat="server" ID="tvDrawingWhereUsed" ImageSet="Arrows" PathSeparator="|"
                        ShowCheckBoxes="All">
                        <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                        <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px"
                            VerticalPadding="0px" />
                        <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px"
                            NodeSpacing="0px" VerticalPadding="0px" />
                    </asp:TreeView>
                </td>
            </tr>
        </table>
        <br />
        <asp:Label runat="server" ID="lblNote2" Text="Please Note: Each checked box will receive a new revision with the selected release type. The New Child Revision (or alternative drawing) will be entered into the BOM of the new parent."
            SkinID="MessageLabelSkin"></asp:Label>
        <br />
        <br />
        <asp:Button ID="cmdCheckSelectNode" runat="server" Text="Nodes Selected" Visible="False"
            CausesValidation="False" />
        <asp:Button ID="btnSelectAll" runat="server" Text="Select All" CausesValidation="False" />
        <asp:Button ID="btnUnselectAll" runat="server" Text="Unselect All" />
        <asp:Button ID="btnUpdateParent" runat="server" Text="Create Revisions and update BOMs"
            ValidationGroup="vgSave" />
            <br />
            <asp:Label ID="lblMessageButtons" runat="server" SkinID="MessageLabelSkin"></asp:Label>
        <br />
        <asp:ValidationSummary ID="vsSave" runat="server" ShowMessageBox="True" ShowSummary="true"
            ValidationGroup="vgSave" />
        <br />
        <asp:Label ID="lblShowMessage" runat="server" SkinID="MessageLabelSkin" />
        <br />
        <asp:Label ID="lblWarning" runat="server" SkinID="MessageLabelSkin"></asp:Label>
        <table>
            <tr>
                <td align="left" colspan="2">
                    <br />
                    <asp:Label runat="server" ID="lblNote3" Text="Below is a list of Parent Drawings (some parents may have existed already) that use the NEW Child Revision (or alternative drawing)."
                        SkinID="MessageLabelSkin" Visible="false"></asp:Label>
                    <br />
                    <br />
                    <asp:TreeView runat="server" ID="tvNewParentDrawings" ImageSet="Arrows" PathSeparator="|"
                        ShowCheckBoxes="None">
                        <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                        <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px"
                            VerticalPadding="0px" />
                        <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px"
                            NodeSpacing="0px" VerticalPadding="0px" />
                    </asp:TreeView>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
