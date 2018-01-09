<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CustomerPartNoLookUp.aspx.vb"
    Inherits="CustomerPartNoLookUp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>UGN, Inc. Customer Part No Look Up</title>

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
    <form id="form1" runat="server" defaultbutton="btnSearch">
        <br />
        <h1 style="text-align: center; background-color: White;">
            Lookup the Customer Part No&nbsp;</h1>
        <hr />
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
        <br />
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <br />
        <table width="700px" style="background-color: White;">
            <tr>
                <td class="p_text" style="white-space: nowrap;" align="right">
                    Customer Part No.:
                </td>
                <td>
                    <asp:TextBox ID="txtCustomerPartNo" runat="server" MaxLength="30"></asp:TextBox>
                </td>
                <td class="p_text" style="white-space: nowrap;" align="right">
                    Internal Part No.:
                </td>
                <td>
                    <asp:TextBox ID="txtBPCSPartNo" runat="server" MaxLength="15"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="p_text" style="white-space: nowrap;" align="right">
                    Customer:
                </td>
                <td>
                    <asp:DropDownList ID="ddCABBV" runat="server">
                    </asp:DropDownList>
                </td>
                <td class="p_text">
                    Customer Part Name:
                </td>
                <td>
                    <asp:TextBox ID="txtCustomerPartName" runat="server" MaxLength="50"></asp:TextBox></td>
            </tr>
           <%-- <tr>
                <td class="p_text">
                    Bar Code Part No:</td>
                <td colspan="3">
                    <asp:TextBox ID="txtBarCodePartNo" runat="server" MaxLength="30"></asp:TextBox></td>
            </tr>--%>
            <tr>
                <td align="center" colspan="4">
                    <asp:Button ID="btnSearch" runat="server" Text="Search"></asp:Button>
                    &nbsp;
                    <asp:Button ID="btnReset" runat="server" Text="Reset"></asp:Button>
                </td>
            </tr>
        </table>
        <asp:GridView ID="gvCustomerPartNoList" runat="server" DataSourceID="odsCustomerPartNoList"
            AllowPaging="True" Width="98%" PageSize="15" AllowSorting="True" AutoGenerateColumns="False">
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
                        <asp:ImageButton ID="ibtnSelectOldPrice" runat="server" CommandName="Select" ImageUrl="~/images/SelectUser.gif"
                            AlternateText="Send price back to previous page" ToolTip="Send price data back to parent page" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CustomerPartNo" HeaderText="Customer Part No." ReadOnly="True"
                    SortExpression="CustomerPartNo">
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <%--<asp:BoundField DataField="BarCodePartNo" HeaderText="Bar Code Part No." SortExpression="BarCodePartNo" />
                <asp:BoundField DataField="BPCSPartNo" HeaderText="F.G. BPCS Part No." ReadOnly="True"
                    SortExpression="BPCSPartNo">
                    <ItemStyle Wrap="False" HorizontalAlign="Left" />
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" />
                </asp:BoundField>--%>
                <asp:BoundField DataField="CustomerPartName" HeaderText="Customer Part Name" ReadOnly="True"
                    SortExpression="CustomerPartName">
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="CABBV" HeaderText="CABBV" ReadOnly="True" SortExpression="CABBV">
                    <ItemStyle Wrap="False" HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsCustomerPartNoList" runat="server" SelectMethod="GetCustomerParts"
            TypeName="CustomerPartsBLL" OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtBPCSPartNo" Name="BPCSPartNo" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="txtCustomerPartNo" Name="CustomerPartNo" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="txtCustomerPartName" Name="CustomerPartName" Type="String" />
                <asp:ControlParameter ControlID="ddCABBV" Name="CABBV" PropertyName="SelectedValue"
                    Type="String" />
                <asp:Parameter Name="BarCodePartNo" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </form>
</body>
</html>
