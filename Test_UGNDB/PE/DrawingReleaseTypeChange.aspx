<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="DrawingReleaseTypeChange.aspx.vb" Inherits="PE_PE_Drawings_DrawingReleaseTypeChange"
    Title="Drawing Release Type Change" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnChangeReleaseType">
        <table cellpadding="3" cellspacing="3" width="700px;" style="background-color: White;">
            <tr>
                <td colspan="2" align="left" style="font-weight: bold; font-size: large;">
                    <asp:Label runat="server" ID="lblDrawingNo"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left">
                    <table width="100%">
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="p_textbold" colspan="2">
                                <asp:Label ID="lblDropdownInstructions" runat="server" Text="Select a Release Type:"></asp:Label>
                                &nbsp;
                                <asp:DropDownList ID="ddReleaseType" runat="server" Style="width: 300px">
                                                                                              
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvReleaseType" runat="server" ControlToValidate="ddReleaseType"
                                    ErrorMessage="Release Type is required." Font-Bold="True" SetFocusOnError="True"
                                    ValidationGroup="vgReleaseType"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="p_textbold" colspan="2">
                                <asp:Label ID="lblCheckInstructions" runat="server" Text="Check each box to change Release Type:"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TreeView ID="tvBOM" runat="server" ImageSet="Arrows" PathSeparator="|" ShowCheckBoxes="All">
                                    <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                                    <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px"
                                        VerticalPadding="0px" />
                                    <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px"
                                        NodeSpacing="0px" VerticalPadding="0px" />
                                </asp:TreeView>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="cmdCheckSelectNode" runat="server" Text="Nodes Selected" Visible="False"
                                    CausesValidation="False" />
                                <asp:Button ID="btnSelectAll" runat="server" Text="Select All" CausesValidation="False" />
                                <asp:Button ID="btnUnselectAll" runat="server" Text="Unselect All" />
                                <asp:Button ID="btnChangeReleaseType" runat="server" Text="Update Release Type" ValidationGroup="vgReleaseType" />
                                <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CausesValidation="False" />
                            </td>
                        </tr>
                    </table>
                    <asp:ValidationSummary ID="vsReleaseType" runat="server" ShowMessageBox="True" ShowSummary="true"
                        ValidationGroup="vgReleaseType" />
                    <br />
                    <asp:Label ID="lblShowMessage" runat="server" SkinID="MessageLabelSkin" />
                    <br />
                    <asp:Label ID="lblWarning" runat="server" SkinID="MessageLabelSkin"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
