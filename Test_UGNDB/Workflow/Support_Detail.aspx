<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Support_Detail.aspx.vb" Inherits="Support_Detail" MaintainScrollPositionOnPostback="true" %>

<%@ Register TagPrefix="UserControl"  
             TagName="SupportDetailControl"  
             Src="Support_Detail_Control.ascx" %> 


<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server">
        <asp:Label runat="server" ID="lblPageMessage" SkinID="MessageLabelSkin"></asp:Label>
        
        <UserControl:SupportDetailControl runat="server" />
        
    </asp:Panel>
</asp:Content>
