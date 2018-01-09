<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DrawingRevisionCompare.aspx.vb"
    EnableTheming="true" Inherits="DrawingRevisionCompare" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>DMS Drawing Revision Comparison</title>
    <!--#include file="~/javascripts/clientFunctions.js"-->
</head>
<body style="background-color: white; vertical-align: top; text-align: left; margin-left: 0px;
    padding: 0px;">
    <asp:Panel ID="localPanel" runat="server">
        <form id="form1" runat="server" style="background-color: White;">
            <div>
                <asp:Label ID="lblMessage" runat="server" CssClass="p_smalltext" Font-Italic="True"
                    Text="Changes since the last revision have a background color of yellow and are in italics."></asp:Label>
                <table width="100%">
                    <tr>
                        <td colspan="2" align="left">
                            <img src="../../images/UGN logo.jpg" height="60px" alt="UGN INC." />&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="2" align="left" class="p_bigtextbold">
                            Compare Drawing&nbsp;
                            <asp:Label ID="lblDrawingNo" runat="server" />
                            &nbsp;to&nbsp;
                            <asp:Label ID="lblPreviousDrawingNo" runat="server" Text="none.             This is the first revision." />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="left" class="p_bigtextbold">
                            <asp:Label ID="lblOldPartName" runat="server" />
                            <br />
                            <asp:Label ID="lblPartName" runat="server" />
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="DrawingDetails" runat="server">
                    <table width="100%" style="background-color: White;">
                        <tr>
                            <td align="right" class="p_text">
                                Current Status:</td>
                            <td align="left" class="p_textbold">
                                <asp:Label ID="lblStatus" runat="server" /></td>
                        </tr>
                        <tr>
                            <td align="right" class="p_text">
                                BPCS Part No.:</td>
                            <td colspan="3" class="c_textbold">
                                <asp:Label ID="lblPartNo" runat="server" /></td>
                        </tr>
                        <tr>
                            <td align="right" class="p_text">
                                Construction:</td>
                            <td colspan="3" class="c_textbold">
                                <asp:Label ID="lblConstruction" runat="server" /></td>
                        </tr>
                        <tr>
                            <td align="right" class="p_text">
                                Control Plan Ref:</td>
                            <td colspan="3" class="c_textbold">
                                <asp:Label ID="lblControl" runat="server" /></td>
                        </tr>
                        <tr>
                            <td align="right" class="p_text">
                                Density Value/Tolerance/Units:</td>
                            <td class="p_textbold">
                                <asp:Label ID="lblDValue" runat="server" />&nbsp;&nbsp;
                                <asp:Label ID="lblDTolerance" runat="server" />&nbsp;&nbsp;
                                <asp:Label ID="lblDUnits" runat="server" />
                            </td>
                            <td align="right" class="p_text">
                                Thickness Value/Tolerance/Units:</td>
                            <td class="p_textbold">
                                <asp:Label ID="lblTValue" runat="server" />&nbsp;&nbsp;
                                <asp:Label ID="lblTTolerance" runat="server" />&nbsp;&nbsp;
                                <asp:Label ID="lblTUnits" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="p_text">
                                <asp:Label ID="lblWMDVal" Text="Width:" runat="server"></asp:Label>
                            </td>
                            <td class="p_textbold">
                                <asp:Label ID="lblWidth" runat="server" /></td>
                            <td align="right" class="p_text">
                                <asp:Label ID="lblCommidityLabel" runat="server" CssClass="p_text" Text="Commodity"
                                    Visible="false"></asp:Label></td>
                            <td class="p_textbold">
                                <asp:Label ID="lblCommodityValue" runat="server" Visible="false" /></td>
                        </tr>
                        <tr>
                            <td align="right" class="p_text">
                                <asp:Label ID="lblAMDVal" Text="Length:" runat="server"></asp:Label>
                            </td>
                            <td class="p_textbold">
                                <asp:Label ID="lblLength" runat="server" />
                            </td>
                            <td align="right" class="p_text">
                                <asp:Label ID="lblPurchasedGoodLabel" runat="server" CssClass="p_text" Text="Purchased Good"
                                    Visible="false"></asp:Label></td>
                            <td class="p_textbold">
                                <asp:Label ID="lblPurchasedGoodValue" runat="server" Visible="false" /></td>
                        </tr>
                        <tr>
                            <td align="right" class="p_text" valign="top">
                                Notes:</td>
                            <td colspan="3" class="p_textbold">
                                <asp:Label ID="lblNotes" runat="server" Style="overflow: auto" /></td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4">
                                <table border="2" runat="server" id="tbDrawingImage">
                                    <tr>
                                        <td>
                                            <img id="imgDrawing" runat="server" alt="Drawing" src="" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="BillOfMaterials" runat="server">
                    <table width="100%">
                        <asp:Repeater ID="rpBillOfMaterials" runat="server">
                            <SeparatorTemplate>
                                <tr>
                                    <td colspan="8">
                                        <hr style="height: 0.01em" />
                                    </td>
                                </tr>
                            </SeparatorTemplate>
                            <HeaderTemplate>
                                <tr>
                                    <td class="p_tablebackcolor" align="center">
                                        Name</td>
                                    <td class="p_tablebackcolor" align="center">
                                        Sub-Drawing No.</td>
                                    <td class="p_tablebackcolor" align="center">
                                        Quantity</td>
                                    <td class="p_tablebackcolor" align="center">
                                        Notes</td>
                                </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.OldPartName")%>
                                        &nbsp;&nbsp;
                                        <%#DataBinder.Eval(Container, "DataItem.PartName")%>
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="selectDrawing" runat="server" Target="_blank" NavigateUrl='<%# "DrawingDetail.aspx?DrawingNo=" & DataBinder.Eval (Container.DataItem,"SubDrawingNo").tostring %>'><%#DataBinder.Eval(Container, "DataItem.SubDrawingNo")%></asp:HyperLink>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.DrawingQuantity")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.notes")%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                            </FooterTemplate>
                        </asp:Repeater>
                        <tr>
                            <td colspan="8">
                                <hr style="height: 0.01em" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table width="100%" style="background-color: white;">
                    <tr>
                        <td align="right" class="p_text" style="white-space: nowrap;">
                            Date Issued:
                        </td>
                        <td align="left">
                            <asp:Label ID="lblDate" runat="server" /></td>
                        <td rowspan="4" class="c_textbold">
                            Revision Notes:&nbsp;&nbsp;<br />
                            <asp:Label ID="lblRevisionNotes" runat="server" cssclass="c_textbold"/></td>
                    </tr>                                                     
                </table>
            </div>
        </form>
    </asp:Panel>
</body>
</html>
