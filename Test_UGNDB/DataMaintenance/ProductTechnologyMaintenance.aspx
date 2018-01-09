<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="ProductTechnologyMaintenance.aspx.vb" Inherits="DataMaintenance_ProductTechnologyMaintenance"
    Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <hr />
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <br />
        <asp:Label ID="lblMessage" runat="server" Width="818px" SkinID="MessageLabelSkin" />
        <table>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblProductTechnology" runat="server" Text=" Product Technology Name:" />
                </td>
                <td>
                    <asp:TextBox ID="txtProductTechnologyNameSearch" runat="server" MaxLength="25" Width="200px" />
                </td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" />
                </td>
            </tr>
        </table>
        <hr />
        <asp:GridView ID="gvProductTechnologyList" runat="server" AutoGenerateColumns="False"
            DataKeyNames="ProductTechnologyID" DataSourceID="odsProductTechnologyList" AllowPaging="True"
            Width="500px" OnRowCommand="gvProductTechnologyList_RowCommand" AllowSorting="True"
            SkinID="StandardGrid" PageSize="30">
            <Columns>
                <asp:BoundField DataField="ProductTechnologyID" HeaderText="ProductTechnology ID"
                    ReadOnly="True" SortExpression="ProductTechnologyID" Visible="False" />
                <asp:TemplateField HeaderText="Product Technology Name" SortExpression="ProductTechnologyName">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtProductTechnologyNameEdit" runat="server" Text='<%# Bind("ProductTechnologyName") %>'
                            ValidationGroup="EditProductTechnologyInfo" MaxLength="25" Width="200px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvProductTechnologyName" runat="server" ControlToValidate="txtProductTechnologyNameEdit"
                            Display="Dynamic" ErrorMessage="You must enter a value for ProductTechnology Name."
                            ValidationGroup="EditProductTechnologyInfo">
                        <
                        </asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblProductTechnologyNamePreEdit" runat="server" Text='<%# Bind("ddProductTechnologyName") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <FooterTemplate>
                        <asp:TextBox ID="txtProductTechnologyNameInsert" runat="server" ValidationGroup="InsertProductTechnologyInfo"
                            MaxLength="25" Width="200px"></asp:TextBox>&nbsp;
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtProductTechnologyNameInsert"
                            ErrorMessage="ProductTechnology Name is required to Insert" ValidationGroup="InsertProductTechnologyInfo">
                    <
                        </asp:RequiredFieldValidator>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Obsolete" SortExpression="Obsolete">
                    <EditItemTemplate>
                        <asp:CheckBox ID="chkObsoleteEdit" runat="server" Checked='<%# Bind("Obsolete") %>' />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkObsoletePreEdit" runat="server" Checked='<%# Bind("Obsolete") %>'
                            Enabled="false" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:BoundField DataField="comboUpdateInfo" HeaderText="Last Update" ReadOnly="True"
                    SortExpression="comboUpdateInfo" HeaderStyle-HorizontalAlign="left" >
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:TemplateField ShowHeader="False">
                    <EditItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True"
                            CommandName="Update" ValidationGroup="EditProductTechnologyInfo" ImageUrl="~/images/save.jpg"
                            Text="Update" AlternateText="Update" />&nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ibtnCancel"
                                runat="server" CausesValidation="False" CommandName="Cancel" ImageUrl="~/images/cancel.jpg"
                                Text="Cancel" AlternateText="Cancel" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False"
                            CommandName="Edit" ImageUrl="~/images/edit.jpg" Text="Edit" AlternateText="Edit" />&nbsp;&nbsp;&nbsp;
                    </ItemTemplate>
                    <FooterTemplate>
                        &nbsp;&nbsp;<asp:ImageButton Text="Insert" CommandName="Insert" CausesValidation="true"
                            runat="server" ID="ibtnInsert" ImageUrl="~/images/save.jpg" AlternateText="Insert"
                            ValidationGroup="InsertProductTechnologyInfo" />
                        &nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo"
                            ImageUrl="~/images/undo-gray.jpg" Text="Undo" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        &nbsp;&nbsp;
        <asp:ValidationSummary ID="vsEditProductTechnologyInfo" runat="server" ShowMessageBox="True"
            ShowSummary="True" Width="498px" ValidationGroup="EditProductTechnologyInfo" />
        <asp:ValidationSummary ID="vsInsertProductTechnologyInfo" runat="server" ShowMessageBox="True"
            ShowSummary="True" Width="498px" ValidationGroup="InsertProductTechnologyInfo" />
        <asp:ObjectDataSource ID="odsProductTechnologyList" runat="server" SelectMethod="GetProductTechnologies"
            TypeName="ProductTechnologiesBLL" UpdateMethod="UpdateProductTechnologies" InsertMethod="InsertProductTechnologies"
            OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:QueryStringParameter Name="ProductTechnologyName" QueryStringField="ProductTechnologyName"
                    Type="String" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="ProductTechnologyName" Type="String" />
                <asp:Parameter Name="obsolete" Type="Boolean" />
                <asp:Parameter Name="original_ProductTechnologyID" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="ProductTechnologyName" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
