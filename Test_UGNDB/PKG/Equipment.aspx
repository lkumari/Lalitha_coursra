<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Equipment.aspx.vb" Inherits="Packaging_Equipment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Height="398px">
        <asp:Label runat="server" ID="lblMessage"></asp:Label><asp:Label ID="lblSearchTip"
            runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <br />
        <table>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblEquip" runat="server" Text="Equipment"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" />
                </td>
            </tr>
        </table>
        <hr />
        <asp:ValidationSummary runat="server" ID="vsEdit" ValidationGroup="vgEdit" ShowMessageBox="true"
            ShowSummary="true" DisplayMode="List" />
        <asp:ValidationSummary runat="server" ID="vsInsert" ValidationGroup="vgInsert" ShowMessageBox="true"
            ShowSummary="true" DisplayMode="List" />
        <asp:GridView ID="gvEquip" runat="server" SkinID="StandardGrid" Width="500px" DataSourceID="odsEquipMaint"
            DataKeyNames="EQPTID" OnRowCommand="gvEquip_RowCommand" AllowPaging="True" AllowSorting="True"
            PageSize="30" >
            <Columns>
                <asp:TemplateField HeaderText="Equipment" SortExpression="EquipmentDesc">
                    <EditItemTemplate>
                        <asp:Label ID="maker1" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label>
                        <asp:TextBox ID="txtEdit" runat="server" Text='<%# Bind("EquipmentDesc") %>' MaxLength="25"
                            Width="150px">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvEdit" runat="server" ControlToValidate="txtEdit"
                            ErrorMessage="A Equipment is required." Text="<" Font-Bold="True" ValidationGroup="vgEdit"> 
                        </asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblView" runat="server" Text='<%# Bind("ddEquipmentDesc") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="marker2" runat="server" Font-Bold="True" ForeColor="Red" Text="*" />
                        <asp:TextBox ID="txtInsert" runat="server" MaxLength="25" Width="150px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvInsert" runat="server" ControlToValidate="txtInsert"
                            ErrorMessage="A Equipment is required." Text="<" Font-Bold="True" ValidationGroup="vgInsert"> 
                        </asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Obsolete" SortExpression="Obsolete">
                    <EditItemTemplate>
                        <asp:CheckBox ID="cbEditObsolete" runat="Server" Checked='<%# Bind("Obsolete") %>' />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="cbViewObsolete" runat="Server" Checked='<%# Bind("Obsolete") %>'
                            Enabled="false" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <EditItemTemplate>
                        <asp:ImageButton ID="iBtnUpdate" runat="server" CausesValidation="True" CommandName="Update"
                            ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEdit" />
                        <asp:ImageButton ID="iBtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                            ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="iBtnEdit" runat="server" CausesValidation="False" CommandName="Edit"
                            ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgInsert"
                            runat="server" ID="iBtnSave" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                        <asp:ImageButton ID="iBtnUndo" runat="server" CommandName="Undo" CausesValidation="false"
                            ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsEquipMaint" runat="server" InsertMethod="InsertEquipMaint"
            OldValuesParameterFormatString="original_{0}" SelectMethod="GetEquipMaint" TypeName="PKGBLL"
            UpdateMethod="UpdateEquipMaint">
            <UpdateParameters>
                <asp:Parameter Name="EquipmentDesc" Type="String" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
                <asp:Parameter Name="original_EQPTID" Type="Int32" />
            </UpdateParameters>
            <SelectParameters>
                <asp:Parameter Name="EQPTID" Type="Int32" />
                <asp:ControlParameter ControlID="txtSearch" Name="EquipmentDesc" PropertyName="Text"
                    Type="String" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="EquipmentDesc" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
