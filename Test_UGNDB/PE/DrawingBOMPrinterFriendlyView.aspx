<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DrawingBOMPrinterFriendlyView.aspx.vb"
    Inherits="PE_PE_Drawings_DrawingBOMPrinterFriendlyView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>UGN, Inc. Printer Friendly Drawing BOM View</title>

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
    <form id="form1" runat="server">
        <div>
            <h1 style="text-align: center; background-color: White;">
                Preview Bill of Materials</h1>
            <hr />
            <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
            <br />
            <asp:Label ID="lblDrawingNoLabel" runat="server" CssClass="p_bigtextbold" Text="Drawing No:" />
            <asp:Label ID="lblDrawingNoValue" runat="server" CssClass="p_bigtextbold" />
            <br />
            <table  style="background-color: White;">
                <tr align="center">
                    <td>
                        <asp:GridView ID="gvSubDrawings" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                            AllowPaging="True" PageSize="500" DataKeyNames="DrawingNo,SubDrawingNo"
                            DataSourceID="odsSubDrawings" EmptyDataText="There are no components currently defined for this drawing."
                            >
                            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#CCCCCC" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />                            
                            <Columns>
                                <asp:TemplateField HeaderText="Sub-Drawing No Link" SortExpression="SubDrawingNo">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lnkViewSubDrawing" runat="server" NavigateUrl='<%# Eval("SubDrawingNo", "DrawingDetail.aspx?DrawingNo={0}") %>'
                                            Target="_blank" Text='<%# Eval("SubDrawingNo") %>'></asp:HyperLink>
                                    </ItemTemplate>
                                    <ItemStyle Wrap="false" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="OldPartName" HeaderText="Drawing Name" 
                                    SortExpression="OldPartName" ItemStyle-HorizontalAlign="left" >
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="SubPartNo" HeaderText="Sub-Internal Part No" 
                                    SortExpression="SubPartNo" ItemStyle-HorizontalAlign="center" >
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="DrawingQuantity" HeaderText="Quantity" 
                                    SortExpression="DrawingQuantity" ItemStyle-HorizontalAlign="center" >
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:BoundField>
                                    <asp:BoundField DataField="Notes" HeaderText="Notes" 
                                    SortExpression="Notes" ItemStyle-HorizontalAlign="left" >                              
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Process" HeaderText="Process"  SortExpression="Process"
                                    ItemStyle-HorizontalAlign="left" >
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Equipment" HeaderText="Equipment"  SortExpression="Equipment"
                                    ItemStyle-HorizontalAlign="left" >
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="ProcessParameters" HeaderText="ProcessParameters" 
                                    SortExpression="ProcessParameters" ItemStyle-HorizontalAlign="left" >
<ItemStyle HorizontalAlign="Left"></ItemStyle>
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsSubDrawings" runat="server" OldValuesParameterFormatString="original_{0}"
                            SelectMethod="GetSubDrawings" TypeName="SubDrawingsBLL" 
                            DeleteMethod="DeleteSubDrawings" InsertMethod="InsertSubDrawing" 
                            UpdateMethod="UpdateSubDrawings">
                            <DeleteParameters>
                                <asp:Parameter Name="original_RowID" Type="Int32" />
                                <asp:Parameter Name="DrawingStatusID" Type="String" />
                                <asp:Parameter Name="AppendRevisionNotes" Type="String" />
                            </DeleteParameters>
                            <UpdateParameters>
                                <asp:Parameter Name="DrawingQuantity" Type="Double" />
                                <asp:Parameter Name="Notes" Type="String" />
                                <asp:Parameter Name="Process" Type="String" />
                                <asp:Parameter Name="Equipment" Type="String" />
                                <asp:Parameter Name="ProcessParameters" Type="String" />
                                <asp:Parameter Name="original_RowID" Type="Int32" />
                                <asp:Parameter Name="SubDrawingNo" Type="String" />
                                <asp:Parameter Name="RowID" Type="Int32" />
                            </UpdateParameters>
                            <SelectParameters>
                                <asp:QueryStringParameter Name="DrawingNo" QueryStringField="DrawingNo" Type="String" />
                                <asp:Parameter Name="SubDrawingNo" Type="String" />
                                <asp:Parameter Name="PartNo" Type="String" />
                                <asp:Parameter Name="PartRevision" Type="String" />
                                <asp:Parameter Name="SubPartNo" Type="String" />
                                <asp:Parameter Name="SubPartRevision" Type="String" />
                                <asp:Parameter Name="DrawingQuantity" Type="Double" />
                                <asp:Parameter Name="Notes" Type="String" />
                            </SelectParameters>
                            <InsertParameters>
                                <asp:Parameter Name="DrawingNo" Type="String" />
                                <asp:Parameter Name="SubDrawingNo" Type="String" />
                                <asp:Parameter Name="DrawingQuantity" Type="Double" />
                                <asp:Parameter Name="Notes" Type="String" />
                                <asp:Parameter Name="Process" Type="String" />
                                <asp:Parameter Name="Equipment" Type="String" />
                                <asp:Parameter Name="ProcessParameters" Type="String" />
                            </InsertParameters>
                        </asp:ObjectDataSource>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox runat="server" ID="cbShowTree" Text="Show / Hide Tree View (all sub-parts of sub-parts)"
                            Checked="true" AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TreeView runat="server" ID="tvCurrentDrawingAsTop" Font-Size="9pt">
                        </asp:TreeView>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
