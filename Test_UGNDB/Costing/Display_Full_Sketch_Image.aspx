<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Display_Full_Sketch_Image.aspx.vb" MaintainScrollPositionOnPostback="true"
    Inherits="Costing_Display_Full_Sketch_Image" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Enlarged Cost Sheet Sketch</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <A HREF="javascript:window.print()">Click to Print This Page</A><br />

            <img id="imgDrawingPartSketch" runat="server" style="border:0" alt="DrawingPartSketch"  src=""  width="1100" height="900"/>
        </div>
    </form>
</body>
</html>
