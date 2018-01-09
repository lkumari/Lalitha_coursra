<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RFD_Status_Report.aspx.vb" Inherits="RFD_Status_Report" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
   
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <title>Request For Development Status Report</title>
       <script language="javascript" type="text/javascript">
       // Keep the popup in focus until it gets closed.
       // This method works when the document loses focus.
       // It does not work if a form field loses focus.
       function restoreFocus()
       {
          if (!document.hasFocus())
          {
             window.focus();
          }
       }
       onblur=restoreFocus;
    </script>
</head>
<body>

    <asp:Panel ID="localPanel" runat="server" Width="100%">
        <asp:Label runat="server" ID="lblMessage" SkinID="MessageLabelSkin"></asp:Label>
        <form id="form1" runat="server">            
       
        </form>
    </asp:Panel>
</body>
</html>
