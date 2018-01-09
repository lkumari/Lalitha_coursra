<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Support_Detail_Popup.aspx.vb"
    Inherits="Support_Detail_Popup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="UserControl" TagName="SupportDetailControl" Src="Support_Detail_Control.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Support Detail</title>

    <script language="javascript" type="text/javascript"> 

    function tbLimit() {
        var tbObj = event.srcElement;
        if (tbObj.value.length == tbObj.maxLength * 1) return false;
    }
    function tbCount(visCnt) {
        var tbObj = event.srcElement;
        if (tbObj.value.length > tbObj.maxLength * 1) tbObj.value = tbObj.value.substring(0, tbObj.maxLength * 1);
        if (visCnt) visCnt.innerText = tbObj.maxLength - tbObj.value.length + " char(s) remaining.";
    }

    </script>

</head>
<body>
    <br />
    <br />
    <br />
    <form runat="server" id="frmSupportDetail">
    <ajax:ToolkitScriptManager runat="Server" ID="ScriptManager1" />
    <asp:Label runat="server" ID="lblPageMessage" SkinID="MessageLabelSkin"></asp:Label>
    <UserControl:SupportDetailControl ID="SupportDetailControl1" runat="server" />
    </form>
</body>
</html>
