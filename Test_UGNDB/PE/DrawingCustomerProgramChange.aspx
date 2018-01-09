<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="DrawingCustomerProgramChange.aspx.vb" Inherits="DrawingCustomerProgramChange"
    Title="Drawing Customer Program Change" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server">
        <table cellpadding="3" cellspacing="3" width="700px;" style="background-color: White;">
            <tr>
                <td colspan="2" align="left" style="font-weight: bold; font-size: large;">
                    <asp:Label runat="server" ID="lblDrawingNo"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left">
                    <table width="100%">
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="p_textbold" colspan="2">
                                <asp:GridView ID="gvCustomerProgram" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                                    AllowPaging="True" PageSize="15" DataKeyNames="RowID" DataSourceID="odsCustomerProgram"
                                    EmptyDataText="No Programs or Customers found" Width="98%">
                                    <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                    <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                    <EditRowStyle BackColor="#CCCCCC" />
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:BoundField DataField="ProgramYear" HeaderText="Year" SortExpression="ProgramYear"
                                            ReadOnly="True" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="ddProgramName" HeaderText="Program / Make / Model / Platform / Customer"
                                            SortExpression="ddProgramName" ReadOnly="True" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left" />
                                    </Columns>
                                </asp:GridView>
                                <asp:ObjectDataSource ID="odsCustomerProgram" runat="server" OldValuesParameterFormatString="original_{0}"
                                    SelectMethod="GetDrawingCustomerProgram" TypeName="DrawingCustomerProgramBLL">
                                    <SelectParameters>
                                        <asp:QueryStringParameter Name="DrawingNo" QueryStringField="DrawingNo" Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblCheckInstructions" runat="server" Text="Check each box to change Program / Customer  Info:"
                                    Font-Bold="true" Font-Size="Small" />
                                <asp:TreeView ID="tvBOM" runat="server" ImageSet="Arrows" PathSeparator="|" ShowCheckBoxes="All">
                                    <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                                    <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px"
                                        VerticalPadding="0px" />
                                    <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px"
                                        NodeSpacing="0px" VerticalPadding="0px" />
                                </asp:TreeView>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="cmdCheckSelectNode" runat="server" Text="Nodes Selected" Visible="False"
                                    CausesValidation="False" />
                                <asp:Button ID="btnSelectAll" runat="server" Text="Select All" CausesValidation="False" />
                                <asp:Button ID="btnUnselectAll" runat="server" Text="Unselect All" />
                                <asp:Button ID="btnChange" runat="server" Text="Push Program / Customer To SubDrawings" />
                                <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CausesValidation="False" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Label ID="lblShowMessage" runat="server" SkinID="MessageLabelSkin" />
                    <br />
                    <asp:Label ID="lblWarning" runat="server" SkinID="MessageLabelSkin"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
