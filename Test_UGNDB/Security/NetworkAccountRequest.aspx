<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="NetworkAccountRequest.aspx.vb" MaintainScrollPositionOnPostback="True"
    Inherits="Security_NetworkAccountRequest" Title="Untitled Page" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="100%">
    <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
            Visible="False" CssClass="c_textbold" />
        <%--form contents go here--%>
    </asp:Panel>
</asp:Content>
