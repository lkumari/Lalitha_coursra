<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MaterialSpecLookUp.aspx.vb"
    Inherits="MaterialSpecLookUp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>UGN, Inc. Material Specification Look Up</title>

    <script language="javascript" type="text/javascript">
        // Keep the popup in focus until it gets closed.
        // This method works when the document loses focus.
        // It does not work if a form field loses focus.
        function restoreFocus() {
            if (!document.hasFocus()) {
                window.focus();
            }
        }
        onblur = restoreFocus;
    </script>

</head>
<body>
    <form id="form1" runat="server" defaultbutton="btnSearch">
    <ajax:ToolkitScriptManager runat="Server" ID="ScriptManager1" />
    <br />
    <h1 style="text-align: center; background-color: White;">
        Lookup Material Specification Numbers
    </h1>
    <hr />
    <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
    <br />
    <table>
        <tr>
                <td style="white-space: nowrap;" class="p_text">
                    Material Specification No:
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchMaterialSpecNo" MaxLength="18"></asp:TextBox>
                </td>
                <td style="white-space: nowrap;" class="p_text">
                    Desc:
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchMaterialSpecDesc" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Family-SubFamily:
                </td>
                <td>
                    <asp:DropDownList ID="ddSubFamily" runat="server">
                    </asp:DropDownList>
                </td>
                  <td style="white-space: nowrap;" class="p_text">
                    Area Weight:
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchMaterialAreaWeight" MaxLength="6"></asp:TextBox>
                </td>
            </tr>
            <tr>
             <td style="white-space: nowrap;" class="p_text">
                    Drawing No:
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchDrawingNo" MaxLength="25"></asp:TextBox>
                </td>
            </tr>
    </table>
    <table width="98%">
        <tr>
            <td align="center">
                <asp:Button ID="btnSearch" runat="server" Text="Search" ValidationGroup="vgDrawing" />
                <asp:Button ID="btnReset" runat="server" Text="Reset" />
            </td>
        </tr>
    </table>
    <hr />
    <br />
    <asp:GridView ID="gvDrawingMaterialSpec" runat="server" AutoGenerateColumns="False"
        AllowSorting="True" AllowPaging="True" PageSize="15" DataKeyNames="RowID" DataSourceID="odsDrawingMaterialSpec"
        EmptyDataText="No Material Specifications found." ShowFooter="True" Width="98%">
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#CCCCCC" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <Columns>
            <asp:TemplateField ShowHeader="False">
                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                <ItemTemplate>
                    <asp:ImageButton ID="ibtnSelectUser" runat="server" CommandName="Select" ImageUrl="~/images/SelectUser.gif"
                        AlternateText="Send MaterialSpecNo data back to previous page" ToolTip="Send MaterialSpecNo data back to parent page" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="MaterialSpecNo" HeaderStyle-CssClass="none" ItemStyle-CssClass="none">
                <HeaderStyle CssClass="none"></HeaderStyle>
                <ItemStyle CssClass="none"></ItemStyle>
            </asp:BoundField>
            <asp:TemplateField HeaderText="Material Spec No." SortExpression="MaterialSpecNo">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkViewMaterialSpecNo" runat="server" NavigateUrl='<%# Eval("MaterialSpecNo", "~/PE/MaterialSpecDetail.aspx?MaterialSpecNo={0}") %>'
                        Font-Underline="true" Target="_blank" Text='<%# Eval("MaterialSpecNo") %>'>
                    </asp:HyperLink>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Drawing No." SortExpression="DrawingNo">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkViewDrawingNo" runat="server" NavigateUrl='<%# Eval("DrawingNo", "~/PE/DMSDrawingPreview.aspx?DrawingNo={0}") %>'
                        Font-Underline="true" Target="_blank" Text='<%# Eval("DrawingNo") %>'>
                    </asp:HyperLink>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="center" />
            </asp:TemplateField>
            <asp:BoundField DataField="MaterialSpecDesc" HeaderStyle-CssClass="none"
                ItemStyle-CssClass="none">
                <HeaderStyle CssClass="none"></HeaderStyle>
                <ItemStyle CssClass="none"></ItemStyle>
            </asp:BoundField>
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="odsDrawingMaterialSpec" runat="server" SelectMethod="GetDrawingMaterialSpecSearch"
        TypeName="PEModule" OldValuesParameterFormatString="original_{0}">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtSearchMaterialSpecNo" Name="MaterialSpecNo" PropertyName="Text"
                Type="String" />
            <asp:ControlParameter ControlID="txtSearchMaterialSpecDesc" 
                Name="MaterialSpecDesc" PropertyName="Text" Type="String" />
            <asp:Parameter Name="StartRevisionDate" Type="String" />
            <asp:Parameter Name="EndRevisionDate" Type="String" />
            <asp:ControlParameter ControlID="ddSubFamily" Name="SubfamilyID" 
                PropertyName="SelectedValue" Type="String" />
            <asp:ControlParameter ControlID="txtSearchMaterialAreaWeight" Name="AreaWeight" 
                PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="txtSearchDrawingNo" Name="DrawingNo" PropertyName="Text"
                Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    </form>
</body>
</html>
