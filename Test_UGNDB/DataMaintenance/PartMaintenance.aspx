<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="PartMaintenance.aspx.vb" Inherits="DataMaintenance_PartMaintenance"
    Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <hr />
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <br />
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
        <br />
        <table style="width: 812px">
            <tr>
                <td style="height: 15px" class="p_text">
                    Internal Part Number:
                </td>
                <td>
                    <asp:TextBox ID="txtPartNoSearch" runat="server" Width="200px" MaxLength="40"></asp:TextBox>
                </td>
                <td>
                    &nbsp;
                </td>
                <td style="height: 15px" class="p_text">
                    Internal Part Name:
                </td>
                <td style="width: 270px">
                    <asp:TextBox ID="txtPartNameSearch" runat="server" Width="250px" MaxLength="240"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="height: 15px" class="p_text">
                    Drawing Number:
                </td>
                <td>
                    <asp:TextBox ID="txtDrawingNoSearch" runat="server" Width="200px" MaxLength="25"></asp:TextBox>
                </td>
                <td>
                    &nbsp;
                </td>
                <td style="height: 15px" class="p_text">
                    Active Type:
                </td>
                <td style="width: 270px">
                    <asp:DropDownList ID="ddActiveTypeSearch" runat="server">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Text="IM-Active" Value="IM"></asp:ListItem>
                        <asp:ListItem Text="IZ-Inactive" Value="IZ"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="height: 15px" class="p_text">
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <%-- <td style="height: 15px" class="p_text">
                    Designation Type:
                </td>
                <td style="width: 270px">
                    <asp:DropDownList ID="ddDesignationTypeSearch" runat="server">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Text="A-Consumables" Value="A"></asp:ListItem>
                        <asp:ListItem Text="B-Semi-Finished Goods" Value="B"></asp:ListItem>
                        <asp:ListItem Text="C-Finished Goods" Value="C"></asp:ListItem>
                        <asp:ListItem Text="F-Trade Items" Value="F"></asp:ListItem>
                        <asp:ListItem Text="G-Error Filed" Value="G"></asp:ListItem>
                        <asp:ListItem Text="H-Service" Value="H"></asp:ListItem>
                        <asp:ListItem Text="I-Prototype" Value="I"></asp:ListItem>
                        <asp:ListItem Text="R-Raw" Value="R"></asp:ListItem>
                        <asp:ListItem Text="0-Phantom" Value="0"></asp:ListItem>
                        <asp:ListItem Text="6-Non-Inv" Value="6"></asp:ListItem>
                    </asp:DropDownList>
                </td>--%>
            </tr>
            <tr>
                <td align="center">
                </td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" />
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
        </table>
        <hr />
        <asp:GridView ID="gvBPCSPartList" runat="server" AutoGenerateColumns="False" DataKeyNames="PartNo"
            DataSourceID="odsBPCSPartList" AllowPaging="True" Width="800px" AllowSorting="True"
            PageSize="20">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <EmptyDataTemplate>
                No records found.
            </EmptyDataTemplate>
            <Columns>
                <asp:BoundField DataField="ddPartNo" HeaderText="Internal Part Number" ReadOnly="True"
                    SortExpression="PartNo">
                    <ItemStyle Wrap="False" />
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
              <%--  <asp:BoundField DataField="PartRevision" HeaderText="Revision" ReadOnly="True" SortExpression="PartRevision">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>--%>
                <asp:BoundField DataField="DrawingNo" HeaderText="DrawingNo" ReadOnly="True" SortExpression="DrawingNo">
                    <ItemStyle Wrap="False" />
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="PartName" HeaderText="Internal Part Name" ReadOnly="True"
                    SortExpression="PartName">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
              <%--  <asp:BoundField DataField="DesignationTypeText" HeaderText="Designation Type" ReadOnly="True"
                    SortExpression="DesignationType">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>--%>
                <asp:BoundField DataField="ActiveTypeText" HeaderText="Active Type" ReadOnly="True"
                    SortExpression="ActiveTypeText">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
        &nbsp;&nbsp; &nbsp;&nbsp;
        <asp:ObjectDataSource ID="odsBPCSPartList" runat="server" SelectMethod="GetBPCSParts"
            TypeName="BPCSParts" OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:QueryStringParameter Name="PartNo" QueryStringField="PartNo" Type="String" />
                <asp:QueryStringParameter Name="PartName" QueryStringField="PartName" Type="String" />
                <asp:QueryStringParameter Name="DrawingNo" QueryStringField="DrawingNo" Type="String" />
                <asp:Parameter DefaultValue="" Name="DesignationType" Type="String" />
                <asp:QueryStringParameter Name="ActiveType" QueryStringField="ActiveType" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
