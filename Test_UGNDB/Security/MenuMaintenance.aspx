<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="MenuMaintenance.aspx.vb" Inherits="Security_MenuMaintenance" Title="Menu Maintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <hr />
    <asp:Label ID="lblErrors" runat="server" SkinID="MessageLabelSkin"></asp:Label>
    <!-- Display Search fields -->
    <table width="60%">
        <tr>
            <td>
                Menu Name:
            </td>
            <td>
                <asp:TextBox ID="txtMenuName" runat="server" ToolTip="Search for Menu Name (May use % wildcard characters)"
                    Width="216px">
                </asp:TextBox>
                &nbsp;
                <asp:RegularExpressionValidator ID="revMenuName" runat="server" ControlToValidate="txtMenuName"
                    ErrorMessage="Menu Name  must  contain letters or numbers. Wildcard characters may only be used at the beginning or end."
                    ValidationExpression="^[%]?[\d\w-'\s]{1,30}[%]?$" ValidationGroup="vgSearch">
                    *
                </asp:RegularExpressionValidator>
            </td>
            <td>
                <asp:Button ID="btnSearch" runat="server" Text="Search" ValidationGroup="vgSearch" />
                <asp:Button ID="btnResetSearch" runat="server" Text="Reset" CausesValidation="False" />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td colspan="3">
                <%-- Validation Summary for search fields --%>
                <asp:ValidationSummary ID="vsSearch" runat="server" ValidationGroup="vgSearch" Width="400px"
                    ShowMessageBox="True" />
            </td>
        </tr>
        <tr>
            <td colspan="3" align="center">
            </td>
        </tr>
    </table>
    <hr />
    <asp:GridView ID="gvMenu" runat="server" Width="600px" DataKeyNames="MenuID" AllowPaging="True"
        AllowSorting="True" ShowFooter="True" AutoGenerateColumns="False" DataSourceID="odsMenu">
        <FooterStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#CCCCCC" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <EmptyDataTemplate>
            No data to display
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="Menu Id" SortExpression="MenuID">
                <FooterStyle HorizontalAlign="Right" Wrap="False" />
                <FooterTemplate>
                    <asp:Image ID="imgAsterisk" runat="server" ImageUrl="~/images/asterick_blue.gif"
                        AlternateText="New row" />
                    &nbsp;
                </FooterTemplate>
                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                <ItemTemplate>
                    <asp:Label ID="lblMenuIDPreEdit" runat="server" Text='<%# Eval("MenuID") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Menu Name" SortExpression="MenuName">
                <EditItemTemplate>
                    <asp:TextBox ID="txtMenuNameEdit" runat="server" Text='<%# Bind("MenuName") %>' MaxLength="50"
                        Width="200px" />
                    <asp:RequiredFieldValidator ID="rfvMenuNameEdit" runat="server" ControlToValidate="txtMenuNameEdit"
                        ErrorMessage="Menu Name is required" ValidationGroup="vgEditMenuInfo">
                        &lt;
                    </asp:RequiredFieldValidator>
                   <%-- <asp:RegularExpressionValidator ID="revMenuNameEdit" runat="server" ControlToValidate="txtMenuNameEdit"
                        ErrorMessage="Menu Name must contain only letters or numbers" ValidationGroup="vgEditMenuInfo"
                        ValidationExpression="^[\d\w-'\s]{1,30}$">
                        &lt;
                    </asp:RegularExpressionValidator>--%>
                      <ajax:FilteredTextBoxExtender ID="ftbMenuNameEdit" runat="server" TargetControlID="txtMenuNameEdit"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,/-&()[] " />
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="txtMenuNameInsert" runat="server" Text='<%# Bind("MenuName") %>'
                        MaxLength="50" Width="200px" />
                    <asp:RequiredFieldValidator ID="rfvMenuNameInsert" runat="server" ControlToValidate="txtMenuNameInsert"
                        ErrorMessage="Menu Name is required" ValidationGroup="vgInsertMenuInfo">
                        &lt;
                    </asp:RequiredFieldValidator>
                    <%-- <asp:RegularExpressionValidator ID="revMenuNameInsert" runat="server" 
                        ControlToValidate="txtMenuNameInsert"
                        ErrorMessage="Menu Name must contain only letters or numbers" 
                        ValidationGroup="vgInsertMenuInfo" 
                        ValidationExpression="^[\d\w-'&\s]{1,30}$">
                        &lt;
                    </asp:RegularExpressionValidator>--%>
                    <ajax:FilteredTextBoxExtender ID="ftbMenuNameInsert" runat="server" TargetControlID="txtMenuNameInsert"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,/-&()[] " />
                </FooterTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblMenuNamePreEdit" runat="server" Text='<%# Eval("MenuName") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Obsolete" SortExpression="Obsolete">
                <EditItemTemplate>
                    <asp:CheckBox ID="chkObsoleteEdit" runat="server" Checked='<%# Bind("Obsolete") %>' />
                </EditItemTemplate>
                <FooterStyle HorizontalAlign="Center" Wrap="False" />
                <FooterTemplate>
                    <asp:CheckBox ID="chkObsoleteInsert" runat="server" Checked='<%# Bind("Obsolete") %>' />
                </FooterTemplate>
                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                <ItemTemplate>
                    <asp:CheckBox ID="chkObsoletePreEdit" runat="server" Checked='<%# Eval("Obsolete") %>'
                        Enabled="false" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False" AccessibleHeaderText="Buttons column">
                <EditItemTemplate>
                    <asp:ImageButton ID="ibtnUpdate" runat="server" CommandName="UpdateCustom" CausesValidation="True"
                        ValidationGroup="vgEditMenuInfo" ImageUrl="~/images/save.jpg" ToolTip="Save changes"
                        AlternateText="Save changes" OnClick="ibtnUpdate_Click" />
                    <asp:ImageButton ID="ibtnCancelEdit" runat="server" CommandName="Cancel" CausesValidation="False"
                        ImageUrl="~/images/undo-transparent.gif" ToolTip="Undo changes" AlternateText="Undo changes" />
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:ImageButton ID="ibtnInsert" runat="server" CommandName="InsertCustom" CausesValidation="true"
                        ValidationGroup="vgInsertMenuInfo" ImageUrl="~/images/save.jpg" ToolTip="Save new row"
                        AlternateText="Save new row" />
                    <asp:ImageButton ID="ibtnCancelInsert" runat="server" CommandName="Cancel" CausesValidation="False"
                        ImageUrl="~/images/undo-transparent.gif" ToolTip="Undo changes" AlternateText="Undo changes" />
                </FooterTemplate>
                <ItemTemplate>
                    <asp:ImageButton ID="ibtnEdit" runat="server" CommandName="Edit" CausesValidation="False"
                        ImageUrl="~/images/edit.jpg" ToolTip="Edit row" AlternateText="Edit row" />
                </ItemTemplate>
                <FooterStyle HorizontalAlign="Center" Wrap="False" />
                <ItemStyle HorizontalAlign="Center" Wrap="False" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Last Update" SortExpression="comboUpdateInfo">
                <ItemTemplate>
                    <asp:Label ID="lblComboUpdateInfo" runat="server" Text='<%# Eval("comboUpdateInfo") %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    &nbsp;&nbsp;
    <%-- Validation Summary for GridView controls --%>
    <asp:ValidationSummary ID="vsEditMenuInfo" runat="server" ShowMessageBox="True" Width="599px"
        ValidationGroup="vgEditMenuInfo" />
    <asp:ValidationSummary ID="vsInsertMenuInfo" runat="server" ShowMessageBox="True"
        Width="599px" ValidationGroup="vgInsertMenuInfo" />
    <asp:Label ID="lblStatus" runat="server" />
    <br />
    <%-- Data Source for GridView --%>
    <asp:ObjectDataSource ID="odsMenu" runat="server" SelectMethod="GetMenu" TypeName="SecurityModule">
        <SelectParameters>
            <asp:Parameter Name="MenuID" Type="Int32" />
            <asp:ControlParameter ControlID="txtMenuName" Name="MenuName" PropertyName="Text"
                Type="String" />
            <asp:Parameter Name="Obsolete" Type="Boolean" />
            <asp:Parameter Name="SortBy" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
