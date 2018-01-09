<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="CRProjectCategory.aspx.vb" Inherits="CRProjectCategory" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
        Width="568px" Visible="False"></asp:Label><br />
    <asp:Panel ID="localPanel" runat="server">
        <hr />
        <table>
            <tr>
                <td class="p_text">
                    Project Category Name:
                </td>
                <td>
                    <asp:TextBox ID="txtCategoryName" runat="server" MaxLength="50" Width="300px"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" /></td>
            </tr>
        </table>
        <hr />
        <br />
        <asp:GridView ID="gvCategory" runat="server" AutoGenerateColumns="False" DataSourceID="odsCategory"
            AllowSorting="True" AllowPaging="True" Width="550px" OnRowCommand="gvCategory_RowCommand"
            DataKeyNames="PCID" OnDataBound="gvCategory_DataBound" PageSize="20">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" Wrap="False" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Wrap="False" BorderColor="White" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" Wrap="False" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" Wrap="False" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#E2DED6" Wrap="False" BorderStyle="None" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" Wrap="False" />
            <EmptyDataRowStyle BackColor="White" Wrap="False" />
            <Columns>
                <asp:BoundField DataField="PCID" HeaderText="PCID" SortExpression="CategoryID"
                    ReadOnly="True" Visible="False" />
                <asp:TemplateField HeaderText="Project Category Name" SortExpression="ProjectCategoryName">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtCategory" runat="server" Text='<%# Bind("ProjectCategoryName") %>' MaxLength="50"
                            Width="300px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvCategory" runat="server" ControlToValidate="txtCategory"
                            Display="Dynamic" ErrorMessage="Project Category is a required field." ValidationGroup="EditCategoryInfo"><</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblCategory" runat="server" Text='<%# Bind("ProjectCategoryName") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtCategory" runat="server" MaxLength="50" Width="300px"></asp:TextBox>
                      <asp:RequiredFieldValidator ID="rfvCategory" runat="server" ControlToValidate="txtCategory"
                            Display="Dynamic" ErrorMessage="Project Category is a required field." ValidationGroup="InsertCategoryInfo"><</asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:CheckBoxField DataField="Obsolete" HeaderText="Obsolete" SortExpression="Obsolete">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:CheckBoxField>
                <asp:TemplateField HeaderText="Last Update" SortExpression="comboUpdateInfo">
                    <EditItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("comboUpdateInfo") %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("comboUpdateInfo") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemStyle HorizontalAlign="Left" />
                    <EditItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True"
                            CommandName="Update" ImageUrl="~/images/save.jpg" Text="Update" AlternateText="Update"
                            ValidationGroup="EditCategoryInfo" />&nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ibtnCancel"
                                runat="server" CausesValidation="False" CommandName="Cancel" ImageUrl="~/images/cancel.jpg"
                                Text="Cancel" AlternateText="Cancel" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False"
                            CommandName="Edit" ImageUrl="~/images/edit.jpg" Text="Edit" AlternateText="Edit" />
                    </ItemTemplate>
                    <FooterTemplate>
                        &nbsp;&nbsp;<asp:ImageButton Text="Insert" CommandName="Insert" CausesValidation="true"
                            runat="server" ID="ibtnInsert" ImageUrl="~/images/save.jpg" AlternateText="Insert"
                            ValidationGroup="InsertCategoryInfo" />
                        &nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo"
                            ImageUrl="~/images/undo-gray.jpg" Text="Undo" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
            
        </asp:GridView>
        <asp:ObjectDataSource ID="odsCategory" runat="server" InsertMethod="InsertCRProjCategory"
            OldValuesParameterFormatString="original_{0}" SelectMethod="GetCRProjCategory"
            TypeName="CRProjCategoryBLL" UpdateMethod="UpdateCRProjCategory">
            <UpdateParameters>
                <asp:Parameter Name="ProjectCategoryName" Type="String" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
                <asp:Parameter Name="original_PCID" Type="Int32" />
            </UpdateParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="ProjectCategoryName" QueryStringField="pProjCat"
                    Type="String" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="ProjectCategoryName" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
        <asp:ValidationSummary ID="EditCategoryInfo" runat="server" ShowMessageBox="True"
            ValidationGroup="EditCategoryInfo" />
        &nbsp;<asp:ValidationSummary ID="InsertCategoryInfo" runat="server" ShowMessageBox="True"
            ValidationGroup="InsertCategoryInfo" />
    </asp:Panel>
</asp:Content>
