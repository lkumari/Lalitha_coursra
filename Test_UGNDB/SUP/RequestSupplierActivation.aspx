<%@ Page Language="VB" MasterPageFile="~/LookUpMasterPage.master" AutoEventWireup="false"
    CodeFile="RequestSupplierActivation.aspx.vb" Inherits="DataMaintenance_RequestSupplierActivation"
    Title="UGN, Inc.: Assembly Plant Display" MaintainScrollPositionOnPostback="True" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
        Visible="False"></asp:Label>
    <asp:Panel ID="localPanel" runat="server" Width="700px">
        <hr />
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
        <table style="width: 100%; border-bottom-style: groove;" class="sampleStyleC">
            <tr>
                <td class="p_textbold" style="width:150px"">
                    From:&nbsp;
                </td>
                <td class="c_text">
                    <asp:Label ID="lblFrom" runat="server" Text="" />
                </td>
            </tr>
            <tr>
                <td class="p_textbold">
                    To:&nbsp;
                </td>
                <td class="c_text">
                    <asp:Label ID="lblTo" runat="server" Text="" />
                </td>
            </tr>
            <tr>
                <td class="p_textbold">
                    Subject:&nbsp;
                </td>
                <td class="c_text">
                    <asp:Label ID="lblSubject" runat="server" Text="" />
                </td>
            </tr>
        </table>
        <table style="border-style: dotted; border-color: Red; width: 100%;">
            <tr>
                <td class="p_textbold" style="vertical-align: top; width:150px">
                    Message:&nbsp;
                </td>
                <td class="c_text" colspan="3">
                    <asp:Label ID="lblBody" runat="server" Text="" />
                </td>
            </tr>
        </table><br /><br />
        <table>
            <tr>
                <td>
                    <asp:Button ID="btnClose" runat="server" Text="Close Window" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
