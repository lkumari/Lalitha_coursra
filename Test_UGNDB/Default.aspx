<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Default.aspx.vb" Inherits="_Default" Title="UGNDB Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <table>
        <tr style="background-color: white;">
            <td>
                <asp:Menu ID="Menu1" runat="server" Orientation="Horizontal" StaticEnableDefaultPopOutImage="False"
                    StaticMenuItemStyle-CssClass="tab" StaticSelectedStyle-CssClass="selectedTab"
                    CssClass="tabs">
                    <Items>
                        <asp:MenuItem Text="General" Value="0"></asp:MenuItem>
                        <asp:MenuItem Text="Description of Need" Value="1"></asp:MenuItem>
                        <asp:MenuItem Text="Reason for Need" Value="2"></asp:MenuItem>
                        <asp:MenuItem Text="Approval" Value="3"></asp:MenuItem>
                    </Items>
                    <StaticMenuItemStyle CssClass="tab" />
                    <StaticSelectedStyle CssClass="selectedTab" />
                </asp:Menu>
            </td>
        </tr>
        <tr>
            <td align="left" style="background-color: white">
                <asp:Label ID="lblErrors" runat="server" ForeColor="#ff0000" Visible="False" CssClass="p_textbold"
                    Height="24px">
                </asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                Most of the text on this page "Greeked". Its fake text used to approximate how your content will look. This page has many sample elements (a form, a table, lists, etc..). Use these elements to build out your site. Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Donec molestie. Sed aliquam sem ut arcu. Del sam familie. Lor separat existentie es un myth. Por scientie, musica, sport etc., li tot Europa usa li sam vocabularium.Praesent aliquet pretium erat. Praesent non odio. Pellentesque a magna a mauris vulputate lacinia. Aenean viverra per conubia nostra, per&nbsp;
            </td>
        </tr>
                <tr>
            <td>
                Most of the text on this page "Greeked". Its fake text used to approximate how your content will look. This page has many sample elements (a form, a table, lists, etc..). Use these elements to build out your site. Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Donec molestie. Sed aliquam sem ut arcu. Del sam familie. Lor separat existentie es un myth. Por scientie, musica, sport etc., li tot Europa usa li sam vocabularium.Praesent aliquet pretium erat. Praesent non odio. Pellentesque a magna a mauris vulputate lacinia. Aenean viverra per conubia nostra, per
            </td>
        </tr>

    </table>
</asp:Content>
