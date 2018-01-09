<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DrawingBPCSPartNoLookUp.aspx.vb"
    Inherits="PE_PE_Drawings_DrawingBPCSPartNoLookUp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>UGN, Inc. Internal Part No Look Up</title>

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
            Lookup Internal Parts&nbsp;</h1>
        <hr />
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
        <br />
        <table width="100%" style="background-color: White;">
            <tr>
                <td class="p_text" style="white-space: nowrap;" align="right">
                    Search Internal Part No.:
                </td>
                <td>
                    <asp:TextBox ID="txtSearchPartNo" runat="server" Width="200px" MaxLength="40">
                    </asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
                <td class="p_text" style="white-space: nowrap;" align="right">
                    Search Internal Part Name:
                </td>
                <td style="width: 270px">
                    <asp:TextBox ID="txtSearchPartName" runat="server" Width="250px" MaxLength="240">
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="5">
                    <asp:Button ID="btnSearch" runat="server" Width="100" Text="Search" CausesValidation="false">
                    </asp:Button>
                    &nbsp;
                    <asp:Button ID="btnReset" runat="server" Width="100" Text="Reset" CausesValidation="false">
                    </asp:Button>
                </td>
            </tr>
            <tr>
                <td style="white-space: nowrap;" align="left" colspan="5">
                    <asp:GridView ID="gvBPCSPartList" runat="server" AutoGenerateColumns="False" DataKeyNames="PartNo"
                        DataSourceID="odsBPCSPartList" AllowPaging="True" Width="98%" PageSize="15">
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
                                    <asp:ImageButton ID="ibtnSelectPartNo" runat="server" CommandName="Select" ImageUrl="~/images/SelectUser.gif"
                                        AlternateText="Send Internal Part Number back to previous page" ToolTip="Send Internal Part Number data back to parent page" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ddPartNo" HeaderText="Internal Part No" ReadOnly="True"
                                SortExpression="PartNo">
                                <ItemStyle Wrap="False" />
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PartRevision" HeaderText="Revision" ReadOnly="True"
                                SortExpression="PartRevision">
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PartName" HeaderText="Part Name" ReadOnly="True"
                                SortExpression="PartName">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DesignationTypeText" HeaderText="Designation Type" ReadOnly="True"
                                SortExpression="DesignationType">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ActiveTypeText" HeaderText="Active Type" ReadOnly="True"
                                SortExpression="ActiveTypeText">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="BPCSObsoleteText" HeaderText="BPCS Obsolete" ReadOnly="True"
                                SortExpression="BPCSObsolete">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="odsBPCSPartList" runat="server" SelectMethod="GetBPCSParts"
                        TypeName="BPCSParts" OldValuesParameterFormatString="original_{0}">
                        <SelectParameters>
                            <asp:FormParameter Name="PartNo" Type="String" />
                            <asp:Parameter Name="PreviousPartNo" Type="String" />
                            <asp:FormParameter Name="PartName" Type="String" />
                            <asp:Parameter Name="DrawingNo" Type="String" />
                            <asp:Parameter Name="DesignationType" Type="String" />
                            <asp:Parameter Name="ActiveType" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
