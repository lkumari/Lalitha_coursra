<%@ Page Language="VB" AutoEventWireup="false" CodeFile="UGNError.aspx.vb" Inherits="UGNError" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>UGN Error Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div id="mainnav-container">
            <div id="mainnav-side">
                <div id="welcome">
                    <asp:Label ID="lblWElcome" runat="server" Text="Your are Logged in as:"></asp:Label>
                    <asp:Label ID="lblUserName" runat="server" Text="Error Finding User Name" Font-Italic="False"
                        ForeColor="#C00000" Font-Bold="False"></asp:Label>&nbsp;&nbsp;&nbsp;
                </div>
            </div>
            <div class="clear">
            </div>
        </div>
        <!--  Header & Slogan  -->
        <div id="header">
            <asp:Label runat="server" ID="lblHeaderTitle" />
            <div id="slogan">
                Copyright &copy; 2008 UGN, Inc.<br />
                All rights reserved.</div>
        </div>
        <div>
            <center>
                <asp:Label runat="server" ID="lblMessage" SkinID="MessageLabelSkin" Font-Size="Large" />
                <br />
                <asp:LinkButton ID="lnkGoBack" runat="server" PostBackUrl="~/Home.aspx" SkinID="LinkButtonSkin">Go Back</asp:LinkButton>
            </center>
        </div>
    </form>
</body>
</html>
