<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Material_LookUp.aspx.vb"
    Inherits="Costing_Material_LookUp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>UGN, Inc. Costing Material Look Up</title>

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
    <p>
        A</p>
    <form id="form1" runat="server" defaultbutton="btnSearch">
        <br />
        <h1 style="text-align: center; background-color: White;">
            Lookup Costing Materials&nbsp;</h1>
        <hr />
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
        <br />
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <asp:ValidationSummary ID="vsMaterial" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgSearchMaterial" />
        <table width="98%">            
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblSearchMaterialIDLabel" Text="Material ID:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchMaterialIDValue" Width="200px" MaxLength="10"></asp:TextBox>
                </td>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblSearchPartNameLabel" Text="Material/Internal Part Desc:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchPartNameValue" Width="200px" MaxLength="30"></asp:TextBox>
                </td>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblSearchDrawingNoLabel" Text="DMS Drawing No:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchDrawingNoValue" Width="200px" MaxLength="17"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblSearchPartNoLabel" Text="Internal Part No (RM):"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchPartNoValue" Width="200px" MaxLength="15"></asp:TextBox>
                </td>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblSearchVendorLabel" Text="Vendor:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddSearchVendorValue" runat="server" Width="206px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblSearchPurchasedGoodLabel" Text="Purchased Good:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddSearchPurchasedGoodValue" runat="server" Width="206px">
                    </asp:DropDownList>
                </td>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblSearchOldMaterialGroupLabel" Text="Old Material Group:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchOldMaterialGroupValue" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblSearchCoatingLabel" Text="Coating:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddSearchCoating" Width="206px">
                        <asp:ListItem Selected="True" Text="All" Value="All"></asp:ListItem>
                        <asp:ListItem Text="NO Coating" Value="None"></asp:ListItem>
                        <asp:ListItem Text="Only Coating" Value="Only"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblSearchObsoleteLabel" Text="Obsolete:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddSearchObsolete" Width="206px">
                        <asp:ListItem Text="All" Value="All"></asp:ListItem>
                        <asp:ListItem Text="NO Obsolete" Value="None"></asp:ListItem>
                        <asp:ListItem Text="Only Obsolete" Value="Only"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblSearchPackagingLabel" Text="Packaging:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddSearchPackaging" Width="206px">
                        <asp:ListItem Selected="True" Text="All" Value="All"></asp:ListItem>
                        <asp:ListItem Text="NO Packaging" Value="None"></asp:ListItem>
                        <asp:ListItem Text="Only Packaging" Value="Only"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblSearchUGNFacilityCodeLabel" Text="UGN Facility:" ></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddSearchUGNFacilityCode" runat="server" Width="206px" >
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center">
                    <asp:Button ID="btnSearch" runat="server" Width="100" Text="Search" CausesValidation="false">
                    </asp:Button>
                    &nbsp;
                    <asp:Button ID="btnReset" runat="server" Width="100" Text="Reset" CausesValidation="false">
                    </asp:Button>
                </td>
            </tr>
        </table>
        <hr />
        <table width="100%" style="background-color: White;">
            <tr>
                <td style="white-space: nowrap;" align="left" colspan="5">
                    <asp:GridView ID="gvMaterial" runat="server" AutoGenerateColumns="False" DataKeyNames="MaterialID"
                        DataSourceID="odsMaterial" AllowPaging="True" Width="98%" PageSize="15">
                        <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#CCCCCC" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <EmptyDataTemplate>
                            No Records Found.
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField ShowHeader="False">
                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibtnSelectMaterial" runat="server" CommandName="Select" ImageUrl="~/images/SelectUser.gif"
                                        AlternateText="Send material and most back to previous page" ToolTip="Send material and most back to previous page" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="MaterialID" HeaderText="Material ID" SortExpression="MaterialID" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ddMaterialName" HeaderText="Material Name" SortExpression="ddMaterialName" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MaterialDesc" HeaderText="Material Desc" SortExpression="MaterialDesc" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PartNo" HeaderText="Internal Part No (RM)" SortExpression="PartNo" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PartDescription" HeaderText="Part Description" SortExpression="PartName" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DrawingNo" HeaderText="Drawing No" SortExpression="DrawingNo" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ddVendorName" HeaderText="Vendor"
                                SortExpression="ddVendorName" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ddPurchasedGoodName" HeaderText="Purchased Good"
                                SortExpression="ddPurchasedGoodName" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                             <asp:BoundField DataField="UGNFacilityCode" HeaderText="Facility"
                                SortExpression="UGNFacilityCode" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="QuoteCost" HeaderText="Cost" SortExpression="QuoteCost">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FreightCost" HeaderText="Freight" SortExpression="FreightCost">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="isCoating" HeaderText="Coating" SortExpression="isCoating" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="isPackaging" HeaderText="Packaging" SortExpression="isPackaging" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Obsolete" HeaderText="Obsolete" SortExpression="Obsolete" >
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="odsMaterial" runat="server" OldValuesParameterFormatString="original_{0}"
                        SelectMethod="GetMaterial" TypeName="MaterialBLL">
                        <SelectParameters>
                            <asp:QueryStringParameter Name="MaterialID" QueryStringField="MaterialID" Type="String" />
                            <asp:QueryStringParameter Name="PartName" QueryStringField="PartName" Type="String" />
                            <asp:QueryStringParameter Name="PartNo" QueryStringField="PartNo" Type="String" />
                            <asp:QueryStringParameter Name="DrawingNo" QueryStringField="DrawingNo" Type="String" />
                            <asp:QueryStringParameter Name="UGNDBVendorID" QueryStringField="UGNDBVendorID" Type="Int32" />
                            <asp:QueryStringParameter Name="PurchasedGoodID" QueryStringField="PurchasedGoodID"
                                Type="Int32" />
                            <asp:QueryStringParameter Name="UGNFacilityCode" QueryStringField="UGNFacilityCode"
                                Type="String" />                             
                            <asp:QueryStringParameter Name="OldMaterialGroup" QueryStringField="OldMaterialGroup"
                                Type="String" />
                            <asp:QueryStringParameter Name="isPackaging" QueryStringField="isPackaging" Type="Int32" />
                            <asp:QueryStringParameter Name="filterPackaging" QueryStringField="filterPackaging"
                                Type="Int32" />
                            <asp:QueryStringParameter Name="isCoating" QueryStringField="isCoating" Type="Int32" />
                            <asp:QueryStringParameter Name="filterCoating" QueryStringField="filterCoating" Type="Int32" />
                            <asp:QueryStringParameter Name="Obsolete" QueryStringField="Obsolete" DefaultValue="0" Type="Int32" />
                            <asp:QueryStringParameter Name="filterObsolete" QueryStringField="filterObsolete" DefaultValue="1" Type="Int32" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
