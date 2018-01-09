<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="ExpProj_Category_Maint.aspx.vb" Inherits="ExpProj_Category_Maint" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
        Width="568px" Visible="False"></asp:Label><br />
    <asp:Panel ID="localPanel" runat="server">
        <hr />
        <table>
            <tr>
                <td class="p_text">
                    Category Name:
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
            AllowSorting="True" AllowPaging="True" Width="850px" OnRowCommand="gvCategory_RowCommand"
            DataKeyNames="CategoryID" OnDataBound="gvCategory_DataBound" PageSize="20" ShowFooter="True">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" Wrap="False" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Wrap="False" BorderColor="White" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" Wrap="False" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" Wrap="False" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#E2DED6" Wrap="False" BorderStyle="None" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" Wrap="False" />
            <EmptyDataRowStyle BackColor="White" Wrap="False" />
            <Columns>
                <asp:BoundField DataField="CategoryID" HeaderText="CategoryID" SortExpression="CategoryID"
                    ReadOnly="True" Visible="False" />
                <asp:TemplateField HeaderText="Category Name" SortExpression="CategoryName">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtCategory" runat="server" Text='<%# Bind("CategoryName") %>' MaxLength="50"
                            Width="300px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvCategory" runat="server" ControlToValidate="txtCategory"
                            Display="Dynamic" ErrorMessage="Project Category is a required field." ValidationGroup="EditCategoryInfo"><</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblCategory" runat="server" Text='<%# Bind("CategoryName") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtCategory" runat="server" MaxLength="50" Width="300px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvCategory" runat="server" ControlToValidate="txtCategory"
                            Display="Dynamic" ErrorMessage="Project Category is a required field." ValidationGroup="InsertCategoryInfo"><</asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="General Ledger #" SortExpression="GLNo">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtGLNo" runat="server" MaxLength="10" Text='<%# Bind("GLNo") %>'
                            Width="100px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvGLNo" runat="server" ControlToValidate="txtGLNo"
                            Display="Dynamic" ErrorMessage="General Ledger # is a required field." ValidationGroup="EditCategoryInfo"><</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtGLNo" runat="server" MaxLength="10" Text='<%# Bind("GLNo") %>'
                            Width="100px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvGLNo" runat="server" ControlToValidate="txtGLNo"
                            Display="Dynamic" ErrorMessage="General Ledger # is a required field." ValidationGroup="InsertCategoryInfo"><</asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("GLNo") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Useful Life in #Years" SortExpression="UsefulLife">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtUsefulLife" runat="server" MaxLength="3" Text='<%# Bind("UsefulLife") %>'
                            Width="40px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvUsefulLife" runat="server" ControlToValidate="txtUsefulLife"
                            Display="Dynamic" ErrorMessage="Useful Life is a required field." ValidationGroup="EditCategoryInfo"><</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtUsefulLife" runat="server" MaxLength="3" Text='<%# Bind("UsefulLife") %>'
                            Width="40px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvUsefulLife" runat="server" ControlToValidate="txtUsefulLife"
                            Display="Dynamic" ErrorMessage="Useful Life is a required field." ValidationGroup="InsertCategoryInfo"><</asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("UsefulLife") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:CheckBoxField DataField="Obsolete" HeaderText="Obsolete" SortExpression="Obsolete">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:CheckBoxField>
                
                  <asp:BoundField DataField="comboUpdateInfo" HeaderText="Last Update" ReadOnly="True"
                    SortExpression="comboUpdateInfo" />
                    
                <asp:TemplateField ShowHeader="False">
                    <ItemStyle HorizontalAlign="Left" />
                    <EditItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True"
                            CommandName="Update" ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="EditCategoryInfo" />&nbsp;&nbsp;&nbsp;<asp:ImageButton
                                ID="ibtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False"
                            CommandName="Edit" ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                    </ItemTemplate>
                    <FooterTemplate>
                        &nbsp;&nbsp;<asp:ImageButton CommandName="Insert" CausesValidation="true" runat="server"
                            ID="ibtnInsert" ImageUrl="~/images/save.jpg" AlternateText="Insert" ValidationGroup="InsertCategoryInfo" />
                        &nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo"
                            ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsCategory" runat="server" InsertMethod="InsertExpProjCategory"
            OldValuesParameterFormatString="original_{0}" SelectMethod="GetExpProjCategory"
            TypeName="ExpProjCategoryBLL" UpdateMethod="UpdateExpProjCategory">
            <UpdateParameters>
                <asp:Parameter Name="CategoryName" Type="String" />
                <asp:Parameter Name="GLNo" Type="Int32" />
                <asp:Parameter Name="UsefulLife" Type="Int32" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
                <asp:Parameter Name="Original_CategoryID" Type="Int32" />
            </UpdateParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="CategoryName" QueryStringField="CategoryName" Type="String" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="CategoryName" Type="String" />
                <asp:Parameter Name="GLNo" Type="Int32" />
                <asp:Parameter Name="UsefulLife" Type="Int32" />
            </InsertParameters>
        </asp:ObjectDataSource>
        <asp:ValidationSummary ID="EditCategoryInfo" runat="server" ShowMessageBox="True"
            ValidationGroup="EditCategoryInfo" />
        &nbsp;<asp:ValidationSummary ID="InsertCategoryInfo" runat="server" ShowMessageBox="True"
            ValidationGroup="InsertCategoryInfo" />
    </asp:Panel>
</asp:Content>
