<%@ Page Title="" Language="VB" MasterPageFile="~/LookUpMasterPage.master" AutoEventWireup="false"
    CodeFile="InvalidUser.aspx.vb" Inherits="InvalidUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <br />
    <h1>Error while attempting to connect to the UGN Database.</h1>
    <br />    
    <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
    <br />
    <br />
    <asp:Label ID="lblUserName" runat="server" Text="Error Finding User Name" Font-Italic="False"
        ForeColor="#C00000" Font-Bold="False" Font-Size="Large"></asp:Label>
    <br />
    <br />
    <span style="font-size:large">
    The following reasons might cause this error:
     </span>
     <br />
    <br />
    <li style="font-size:medium">Your UGN Email address or name has changed</li>    
    <li style="font-size:medium">You do not have an account in the UGN Database.</li>   
    <br />
    <br />
    <span style="font-weight:bold; font-size:large">
    Please contact the Applications Group at the corporate office: <a href="mailto:TNPISAppGrp@ugnauto.com"><u>Click Here to send an email for support.</u></a>
    <br />
    Please have the HR department and/or your supervisor complete the following Docushare SOP Documents:<br />
    </span>
    <br />
    <a font-size="larger" href="http://tapsd.ugnnet.com:8080/docushare/dsweb/Get/Document-1576/IS107_-_Network_Account_Request_Form.doc" target="_blank">
        <u>IS-107</u></a>
   
    <br />
    <a href="http://tapsd.ugnnet.com:8080/docushare/dsweb/Get/Document-7802/IS110%20-%20UGN%20Database%20Access%20Sign-in%20Sheet.doc" target="_blank">
        <u>IS-110</u></a>
   <br />
   <br />If you had trouble with the direct links to the forms mentioned above, please open Docushare and follow the &quotDocument Control&quot folder to the &quot(IS) Information Systems Documents&quot folder, and you will see the forms. Docushare can be opened by <a href="http://tapsd.ugnnet.com:8080/docushare" target="_blank"><u>clicking here</u></a>
</asp:Content>
