<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DMSDrawingPreview.aspx.vb" Inherits="PE_DMSDrawingPreview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

    
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Preview DMS Drawing</title>
</head>

<script language="javascript" type="text/javascript">
    function HandleOnClose(evt) { 
                
        if (window.event.clientY < 0 && (window.event.clientX > (document.documentElement.clientWidth - 5) || window.event.clientX < 15)) {
                        
            var btn = eval(document.getElementById("<%=btnClosingWork.ClientID %>"));                
     
                if (btn != null) {
                    btn.click();
                }                                
        } 
    }  
     function HandleOnCloseTimed() { 
                
        if (window.event.clientY < 0 && (window.event.clientX > (document.documentElement.clientWidth - 5) || window.event.clientX < 15)) {
                        
            var btn = eval(document.getElementById("<%=btnClosingWork.ClientID %>"));                
     
                if (btn != null) {
                    btn.click();
                }                                
        } 
    }  
</script>

<body>
    <form id="form1" runat="server">
    <asp:Panel ID="localPanel" runat="server">
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
        <asp:Button Visible="true" runat="server" ID="btnClosingWork" Text="clear" Height="1px"
                Width="1px" CausesValidation="false" />
       
    </asp:Panel>
    </form>
</body>
</html>
