<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RFD_To_Cost_Sheet.aspx.vb"
    Inherits="RFD_To_Cost_Sheet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>UGN, Inc. RFD to Cost Sheet</title>

    <script language="JavaScript" type="text/javascript" src="../javascripts/calendar.js"></script>

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
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <form runat="server" id="frmSearchRFD">
            <br />
            <h1 style="text-align: center; background-color: White;">
                Search for an RFD in which Costing is involved&nbsp;</h1>
            <hr />
            <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
            <br />
            <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
            <br />
            <table width="68%" style="background-color: White;">
                <tr>
                    <td class="p_text" style="white-space: nowrap;" align="right">
                        RFD No:
                    </td>
                    <td>
                        <asp:TextBox ID="txtRFDNo" runat="server" MaxLength="15"></asp:TextBox>
                    </td>
                    <td class="p_text" style="white-space: nowrap;" align="right">
                        Description:
                    </td>
                    <td>
                        <asp:TextBox ID="txtRFDDesc" runat="server" MaxLength="100"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="p_text" style="white-space: nowrap;" align="right">
                        Overall Status:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddOverallStatus" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td class="p_text" style="white-space: nowrap;" align="right">
                        Approval Status:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddApproverStatus" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="p_text" style="white-space: nowrap;" align="right">
                        Drawing No:
                    </td>
                    <td>
                        <asp:TextBox ID="txtDrawingNo" runat="server" MaxLength="18"></asp:TextBox>
                    </td>
                    <td class="p_text" style="white-space: nowrap;" align="right">
                        Part Name:
                    </td>
                    <td>
                        <asp:TextBox ID="txtPartName" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="p_text" style="white-space: nowrap;" align="right">
                        Customer Part No:
                    </td>
                    <td>
                        <asp:TextBox ID="txtCustomerPartNo" runat="server" MaxLength="30"></asp:TextBox>
                    </td>
                    <td class="p_text" style="white-space: nowrap;" align="right">
                        BPCS Part No:
                    </td>
                    <td>
                        <asp:TextBox ID="txtPartNo" runat="server" MaxLength="15"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="4">
                        <asp:Button ID="btnSearch" runat="server" Text="Search"></asp:Button>
                        &nbsp;
                        <asp:Button ID="btnReset" runat="server" Text="Reset"></asp:Button>
                    </td>
                </tr>
            </table>
            <asp:GridView ID="gvRFDList" runat="server" DataSourceID="odsRFDList" AllowPaging="True"
                Width="98%" PageSize="15" AllowSorting="True" AutoGenerateColumns="False">
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
                            <asp:ImageButton ID="ibtnSelectRFD" runat="server" CommandName="Select" ImageUrl="~/images/SelectUser.gif"
                                AlternateText="Send RFD to previous page" ToolTip="Send RFD back to parent page" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Selection Type">                       
                        <ItemTemplate>
                            <asp:RadioButtonList runat="server" ID="rbSelectionType" width="100px">
                                <asp:ListItem Text="Top Level" Value="TL" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Child Part" Value="CP"></asp:ListItem>
                            </asp:RadioButtonList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RFDNo" HeaderText="RFD No." ReadOnly="True" SortExpression="RFDNo">
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ChildRowID" ReadOnly="True" >
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="StatusName" HeaderText="Overall Status" ReadOnly="True" SortExpression="StatusName">
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ApproverStatusName" HeaderText="Costing Status" ReadOnly="True" SortExpression="ApproverStatusName">
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="RFDDesc" HeaderText="Description" ReadOnly="True" SortExpression="RFDDesc">
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NewCustomerPartNo" HeaderText="New Customer PartNo" ReadOnly="True"
                        SortExpression="NewCustomerPartNo">
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NewTopLevelDrawingNo" HeaderText="New Top Level DrawingNo" ReadOnly="True"
                        SortExpression="NewTopLevelDrawingNo">
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NewCustomerPartName" HeaderText="New Customer Part Name" ReadOnly="True"
                        SortExpression="NewCustomerPartName">
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NewChildDrawingNo" HeaderText="New Child DrawingNo" ReadOnly="True"
                        SortExpression="NewChildDrawingNo">
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                     <asp:BoundField DataField="NewPartName" HeaderText="New Child Part Name" ReadOnly="True"
                        SortExpression="NewPartName">
                        <ItemStyle HorizontalAlign="Left" />
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsRFDList" runat="server" SelectMethod="GetRFDCostingSearch"
                TypeName="RFDModule">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtRFDNo" Name="RFDNo" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="txtRFDDesc" Name="RFDDesc" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="ddOverallStatus" Name="StatusID" PropertyName="SelectedValue"
                        Type="Int32" DefaultValue="2" />
                    <asp:ControlParameter ControlID="ddApproverStatus" Name="ApproverStatusID" PropertyName="SelectedValue"
                        Type="Int32" DefaultValue="2" />
                    <asp:ControlParameter ControlID="txtDrawingNo" DefaultValue="" Name="DrawingNo" PropertyName="Text"
                        Type="String" />
                    <asp:ControlParameter ControlID="txtCustomerPartNo" Name="CustomerPartNo" PropertyName="Text"
                        Type="String" />
                    <asp:ControlParameter ControlID="txtPartNo" Name="PartNo" PropertyName="Text"
                        Type="String" />
                    <asp:ControlParameter ControlID="txtPartName" Name="PartName" PropertyName="Text"
                        Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </form>
    </asp:Panel>
</body>
</html>
