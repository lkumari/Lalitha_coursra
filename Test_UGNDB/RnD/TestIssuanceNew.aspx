<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="TestIssuanceNew.aspx.vb" Inherits="RnD_TestIssuanceNew" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" Runat="Server">
    <asp:Label ID="lblMessage" SkinID="MessageLabelSkin" runat="server" />
    <hr />
    <br />
    <asp:Panel ID="pnlCopyFromMessage" runat="server" >
        <span class="p_textbold" style="text-align:left;" >
            Copy from Request Id:&nbsp; 
        </span>
        <span class="c_text">
            <asp:Label ID="lblCopyFromNo" runat="server"  />
            <br /><br />
        </span>
    </asp:Panel>


    <table cellpadding="0px" width="0px">
        <tr>
            <td colspan="2">
                <span class="p_textbold" style="text-align:left;">Request Category:<br /></span>
            </td>
        </tr>
        <tr style="padding-top: 5px;">
            <td style="text-align:center; vertical-align:middle; height: 47px;" >
                <asp:RadioButton ID="opt1" runat="server" 
                    GroupName="gnNew" Text="" />
            </td>
            <td style="font-size: 13pt; font-family: Verdana; height: 47px">
                Product Innovation<br />
                <span style="font-size:12px; font-variant: small-caps; color:#6A5ACD">Testing involves a new product launch.<br />(Product Development & Sales)<br /></span>
            </td>
        </tr>
        <tr style="padding-top: 5px;">
            <td style="text-align:center; vertical-align:middle; height: 47px;" >
                <asp:RadioButton ID="opt2" runat="server" 
                    GroupName="gnNew" Text="" />
            </td >
            <td style="font-size: 13pt; font-family: Verdana; height: 47px">
                Current Mass Production Part<br />
                <span style="font-size:12px; font-variant: small-caps; color:#6A5ACD;">Testing involves an active current production part.<br /></span>
            </td>
        </tr>
        <tr style="padding-top: 5px; padding-bottom: 10px;">
            <td style="text-align:center; vertical-align:middle; height: 47px;" >
                <asp:RadioButton ID="opt3" runat="server" 
                    GroupName="gnNew" Text="" />
            </td>
            <td class="c_text" style="font-size: 13pt; font-family: Verdana">
                Consultation<br />
                <span style="font-size:12px; font-variant: small-caps; color:#6A5ACD;">Any other test category.<br /></span>
            </td>
        </tr>
                <tr style="padding-top: 5px; padding-bottom: 10px;">
            <td style="text-align:center; vertical-align:middle; height: 47px;" >
                <asp:RadioButton ID="opt4" runat="server" 
                    GroupName="gnNew" Text="" />
            </td>
            <td class="c_text" style="font-size: 13pt; font-family: Verdana">
                New Program Launch<br />
                <span style="font-size:12px; font-variant: small-caps; color:#6A5ACD;">Testing involves launch of a new vehicle program.<br />(QE & Program Management)<br /></span>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblValidation" SkinID="MessageLabelSkin" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="c_text" colspan="2" align="center">
                <br />
                <asp:Button ID="btnAdd" runat="server" 
                    Text="Continue..." 
                    tooltip="Begin Entry of a new Test Issuance Request" />
                &nbsp;
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" 
                    ToolTip="Cancel this request"/>    
            </td>
        </tr>
    </table> 
</asp:Content>

