<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RFD_Child_Part_Parents.aspx.vb"
    Inherits="RFD_Child_Part_Parents" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>RFD Parent Parts Affected By Child Change</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Panel ID="localPanel" runat="server">
            <br />
            <br />
            <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
            <br />
            <asp:Label ID="lblPartNo" runat="server" CssClass="p_bigtextbold"></asp:Label>
            <hr />
            <table>
                <tr>
                    <td valign="top">
                        <asp:GridView ID="gvParentParts" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                            AllowPaging="True" PageSize="15" DataSourceID="odsParentParts" EmptyDataText="No parent parts found">
                            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#CCCCCC" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="PartNo" HeaderText="Parent BPCS PartNo" SortExpression="PartNo" />
                                <asp:TemplateField HeaderText="View BOM">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="iBtnViewSingleBOM" runat="server" ImageUrl="~/images/Search.gif"
                                            ToolTip='<%# Bind("PartNo") %>' OnClick="iBtnViewSingleBOM_Click" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsParentParts" runat="server" SelectMethod="GetBillOfMaterials"
                            TypeName="commonFunctions">
                            <SelectParameters>
                                <asp:Parameter Name="PartNo" Type="String" />
                                <asp:QueryStringParameter Name="SubPartNo" QueryStringField="PartNo" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </td>
                    <td valign="top">
                        <asp:Button runat="server" ID="btnViewBOM" Text="View All BOMs" />
                        <i>(WARNING: This could take more than a few minutes.)</i>
                        <br />
                        <asp:TreeView ID="tvBOM" runat="server">
                        </asp:TreeView>
                    </td>
                </tr>
            </table>
           
        </asp:Panel>
    </form>
</body>
</html>
