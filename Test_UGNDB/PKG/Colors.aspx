<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Colors.aspx.vb" Inherits="Packaging_Colors" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Height="398px">
        <asp:Label runat="server" ID="lblMessage"></asp:Label><asp:Label ID="lblSearchTip"
            runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <br />
        <table >
            <tr>
                <td>
                    <asp:Label ID="lblColor" runat="server" Text="Color:" CssClass="p_text"></asp:Label>
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
        <asp:GridView ID="gvColor" runat="server" SkinID="StandardGrid" DataSourceID="odsColorMaint"
            DataKeyNames="CCode" OnRowCommand="gvColor_RowCommand" AllowPaging="True" AllowSorting="True"
            PageSize="30" Width="450px">
            <Columns>
                <asp:TemplateField HeaderText="Code" SortExpression="CCode">
                    <EditItemTemplate>
                        <asp:Label ID="lblCCode" runat="server" Text='<%# Bind("CCode") %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewCode" runat="server" Text='<%# Bind("ddCCode") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="marker2" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                        <asp:TextBox ID="txtInsertCCode" runat="server" MaxLength="2" Width="50px"></asp:TextBox>
                        <ajax:FilteredTextBoxExtender ID="ftbeInsertCCode" runat="server" TargetControlID="txtInsertCCode"
                            FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ" />
                        <asp:RequiredFieldValidator ID="rfvInsertCCode" runat="server" ControlToValidate="txtInsertCCode"
                            ErrorMessage="A Code is required." Text="<" Font-Bold="True" ValidationGroup="vgInsert"> 
                        </asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Color" SortExpression="Color">
                    <EditItemTemplate>
                        <asp:Label ID="marker1" runat="server" Text="*" Font-Bold="True" ForeColor="Red"></asp:Label>
                        <asp:TextBox ID="txtEditColor" runat="server" Text='<%# Bind("Color") %>' MaxLength="30"
                            Width="250px">
                        </asp:TextBox>
                        <ajax:FilteredTextBoxExtender ID="ftbeEditColor" runat="server" TargetControlID="txtEditColor"
                            FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,&/ " />
                        <asp:RequiredFieldValidator ID="rfvEditColor" runat="server" ControlToValidate="txtEditColor"
                            ErrorMessage="A Color is required." Text="<" Font-Bold="True" ValidationGroup="vgEdit"> 
                        </asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewColor" runat="server" Text='<%# Bind("Color") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="marker3" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                        <asp:TextBox ID="txtInsertColor" runat="server" MaxLength="30" Width="250px"></asp:TextBox>
                        <ajax:FilteredTextBoxExtender ID="ftbeInsertColor" runat="server" TargetControlID="txtInsertColor"
                            FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,&/ " />
                        <asp:RequiredFieldValidator ID="rfvInsertColor" runat="server" ControlToValidate="txtInsertColor"
                            ErrorMessage="A Color is required." Text="<" Font-Bold="True" ValidationGroup="vgInsert"> 
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
        <asp:ObjectDataSource ID="odsColorMaint" runat="server" InsertMethod="InsertColorMaint"
            OldValuesParameterFormatString="original_{0}" SelectMethod="GetColorMaint" TypeName="PKGBLL"
            UpdateMethod="UpdateColorMaint">
            <UpdateParameters>
                <asp:Parameter Name="Color" Type="String" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
                <asp:Parameter Name="original_CCode" Type="String" />
                <asp:Parameter Name="CCode" Type="String" />
            </UpdateParameters>
            <SelectParameters>
                <asp:Parameter Name="CCode" Type="String" />
                <asp:ControlParameter ControlID="txtSearch" Name="Color" PropertyName="Text" Type="String" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="CCode" Type="String" />
                <asp:Parameter Name="Color" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
