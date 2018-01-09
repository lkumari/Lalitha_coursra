<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="BillOfMaterialsTree.aspx.vb" Inherits="DataMaintenance_BillOfMaterialsTree"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <hr />
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
        <br />
        <table>
            <tr>
                <td class="p_text">
                    Finished Goods Internal Part Number:
                </td>
                <td style="white-space:nowrap">
                    <asp:DropDownList ID="ddFGPartNo" runat="server" AutoPostBack="true" 
                        Width="250px">
                    </asp:DropDownList>
                </td>
            </tr>
          
            <tr>
                <td colspan="4" align="center">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CausesValidation="False" />
                    &nbsp;
                    <asp:Button ID="btnGoBack" runat="server" Text="Back to BOM Search" CausesValidation="False" />
                </td>
            </tr>
        </table>
        <asp:TreeView ID="tvBOM" runat="server" Width="630px">
        </asp:TreeView>
        <hr />
    </asp:Panel>
</asp:Content>
