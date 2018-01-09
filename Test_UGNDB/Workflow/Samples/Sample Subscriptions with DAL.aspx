<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Sample Subscriptions with DAL.aspx.vb" Inherits="Workflow_Subscriptions" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    &nbsp;
    <asp:GridView ID="GridView1" runat="server" CssClass="DataWebControlStyle">
        <HeaderStyle CssClass="HeaderStyle" />
        <AlternatingRowStyle CssClass="AlternatingRowStyle" />
    </asp:GridView>
    <asp:GridView ID="GridView2" runat="server" CssClass="DataWebControlStyle">
        <HeaderStyle CssClass="HeaderStyle" />
        <AlternatingRowStyle CssClass="AlternatingRowStyle" />
    </asp:GridView>
    <asp:GridView ID="GridView3" runat="server" CssClass="DataWebControlStyle">
        <HeaderStyle CssClass="HeaderStyle" />
        <AlternatingRowStyle CssClass="AlternatingRowStyle" />
    </asp:GridView>
</asp:Content>
