<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="RolesMaintenance.aspx.vb" Inherits="Security_RolesMaintenance"
    title="Roles Maintenance" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" Runat="Server">
    <hr />
    <asp:Label ID="lblErrors" runat="server" SkinID="MessageLabelSkin"></asp:Label>  
    <!-- Display Search fields -->
    <table width="100%">
        <tr>
            <td>
                Role Name:
            </td>
            <td>
                <asp:TextBox ID="txtRoleName" runat="server" 
                    ToolTip="Search for Role Name (May use % wildcard characters)" 
                    Width="216px">
                </asp:TextBox>
                &nbsp;
                <asp:RegularExpressionValidator ID="revRoleName" runat="server" 
                    ControlToValidate="txtRoleName"
                    ErrorMessage="Role Name  must  contain letters or numbers. Wildcard characters may only be used at the beginning or end."
                    ValidationExpression="^[%]?[\d\w-'\s]{1,30}[%]?$" 
                    ValidationGroup="vgSearch">
                    *
                </asp:RegularExpressionValidator>
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                Description:
            </td>
            <td>
                <asp:TextBox ID="txtDescription" runat="server" 
                    ToolTip="Search for Description (May use % wildcard characters)" 
                    Width="216px">
                </asp:TextBox>
                &nbsp;
                <asp:RegularExpressionValidator ID="revDescription" runat="server" 
                    ErrorMessage="Description is limited to letters, numbers, and the .',() special characters. Wildcard characters may only be used at the beginning or end."
                    ValidationExpression="^[%]?[\d\w-'\s\.\,\(\)]{1,50}[%]?$" 
                    ValidationGroup="vgSearch" 
                    ControlToValidate="txtDescription">
                    *
                </asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td colspan="4">
                <%-- Validation Summary for search fields --%>
                <asp:ValidationSummary ID="vsSearch" runat="server" ValidationGroup="vgSearch" Width="400px" ShowMessageBox="True" />
            </td>
        </tr>
        <tr>
            <td colspan="5" align="center">
                <asp:Button ID="btnSearch" runat="server" Text="Search" ValidationGroup="vgSearch" />
                <asp:Button ID="btnResetSearch" runat="server" Text="Reset" CausesValidation="False" />
            </td>
        </tr>
    </table>
    <hr />
    <asp:GridView ID="gvRoles" runat="server" 
                  Width="815px"
                  DataKeyNames="RoleID"
                  AllowPaging="True" 
                  AllowSorting="True" 
                  ShowFooter="True"
                  AutoGenerateColumns="False" 
                  DataSourceID="odsRoles">
        <FooterStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="White"/>
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
            <asp:TemplateField HeaderText="Role Id" SortExpression="RoleID" >
                <FooterStyle HorizontalAlign="Right" Wrap="False" />
                <FooterTemplate>
                    <asp:Image ID="imgAsterisk" runat="server" ImageUrl="~/images/asterick_blue.gif" 
                        AlternateText="New row"/>
                    &nbsp;
                </FooterTemplate>
                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                <ItemTemplate>
                    <asp:Label ID="lblRoleIdPreEdit" runat="server" Text='<%# Eval("RoleID") %>' /> 
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Role Name" SortExpression="RoleName" >
                <EditItemTemplate>
                    <asp:TextBox ID="txtRoleNameEdit" runat="server" Text='<%# Bind("RoleName") %>' />
                    <asp:RequiredFieldValidator ID="rfvRoleNameEdit" runat="server" 
                        ControlToValidate="txtRoleNameEdit"
                        ErrorMessage="Role Name is required" 
                        ValidationGroup="vgEditRoleInfo">
                        &lt;
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revRoleNameEdit" runat="server" 
                        ControlToValidate="txtRoleNameEdit"
                        ErrorMessage="Role Name must contain only letters or numbers" 
                        ValidationGroup="vgEditRoleInfo" 
                        ValidationExpression="^[\d\w-'\s]{1,30}$">
                        &lt;
                    </asp:RegularExpressionValidator>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="txtRoleNameInsert" runat="server" Text='<%# Bind("RoleName") %>' />
                    <asp:RequiredFieldValidator ID="rfvRoleNameInsert" runat="server" 
                        ControlToValidate="txtRoleNameInsert"
                        ErrorMessage="Role Name is required" 
                        ValidationGroup="vgInsertRoleInfo">
                        &lt;
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revRoleNameInsert" runat="server" 
                        ControlToValidate="txtRoleNameInsert"
                        ErrorMessage="Role Name must contain only letters or numbers" 
                        ValidationGroup="vgINsertRoleInfo" 
                        ValidationExpression="^[\d\w-'\s]{1,30}$">
                        &lt;
                    </asp:RegularExpressionValidator>
                </FooterTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblRoleNamePreEdit" runat="server" Text='<%# Eval("RoleName") %>' /> 
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Description" SortExpression="Description" >
                <EditItemTemplate>
                    <asp:TextBox ID="txtDescriptionEdit" runat="server" Text='<%# Bind("Description") %>' />
                    <asp:RegularExpressionValidator ID="revDescriptionEdit" runat="server" 
                        ControlToValidate="txtDescriptionEdit"
                        ErrorMessage="Description is limited to letters, numbers, and .,'() special characters" 
                        ValidationExpression="^[\d\w-'\s\.\,\(\)]{1,50}$" 
                        ValidationGroup="vgEditRoleInfo">
                        &lt;
                    </asp:RegularExpressionValidator>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="txtDescriptionInsert" runat="server" Text='<%# Bind("Description") %>' />
                    <asp:RegularExpressionValidator ID="revDescriptionInsert" runat="server" 
                        ControlToValidate="txtDescriptionInsert"
                        ErrorMessage="Description is limited to letters, numbers, and .,'() special characters"  
                        ValidationExpression="^[\d\w-'\s\.\,\(\)]{1,50}$" 
                        ValidationGroup="vgInsertRoleInfo">
                        &lt;
                    </asp:RegularExpressionValidator>
                </FooterTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblDescriptionPreEdit" runat="server" Text='<%# Eval("Description") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Obsolete" SortExpression="Obsolete" >
                <EditItemTemplate>
                    <asp:CheckBox ID="chkObsoleteEdit" runat="server" Checked='<%# Bind("Obsolete") %>' />
                </EditItemTemplate>
                <FooterStyle HorizontalAlign="Center" Wrap="False" />
                <FooterTemplate>
                    <asp:CheckBox ID="chkObsoleteInsert" runat="server" Checked='<%# Bind("Obsolete") %>' />
                </FooterTemplate>
                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                <ItemTemplate>
                    <asp:CheckBox ID="chkObsoletePreEdit" runat="server" Checked='<%# Eval("Obsolete") %>' Enabled="false" /> 
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False" AccessibleHeaderText="Buttons column">
                <EditItemTemplate>
                    <asp:ImageButton ID="ibtnUpdate" runat="server" 
                        CommandName="UpdateCustom" 
                        CausesValidation="True" ValidationGroup="vgEditRoleInfo"
                        ImageUrl="~/images/save.jpg" 
                        Tooltip="Save changes" AlternateText="Save changes" 
                        OnClick="ibtnUpdate_Click" />
                    <asp:ImageButton ID="ibtnCancelEdit" runat="server"
                        CommandName="Cancel" 
                        CausesValidation="False" 
                        ImageUrl="~/images/undo-transparent.gif" 
                        ToolTip="Undo changes" AlternateText="Undo changes" />
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:ImageButton ID="ibtnInsert" runat="server" 
                        CommandName="InsertCustom" 
                        CausesValidation="true" ValidationGroup="vgInsertRoleInfo"
                        ImageUrl="~/images/save.jpg"
                        ToolTip="Save new row" AlternateText="Save new row" />
                    <asp:ImageButton ID="ibtnCancelInsert" runat="server"
                        CommandName="Cancel" 
                        CausesValidation="False" 
                        ImageUrl="~/images/undo-transparent.gif" 
                        ToolTip="Undo changes" AlternateText="Undo changes" />
                </FooterTemplate>
                <ItemTemplate>
                    <asp:ImageButton ID="ibtnEdit" runat="server"
                        CommandName="Edit" 
                        CausesValidation="False" 
                        ImageUrl="~/images/edit.jpg" 
                        ToolTip="Edit row" AlternateText="Edit row" />
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
     <asp:ValidationSummary ID="vsEditRoleInfo" runat="server" ShowMessageBox="True"
        Width="599px" ValidationGroup="vgEditRoleInfo" />
     <asp:ValidationSummary ID="vsInsertRoleInfo" runat="server" ShowMessageBox="True"
        Width="599px" ValidationGroup="vgInsertRoleInfo" />
     <asp:Label ID="lblStatus" runat="server" />
    <br />
    
    <%-- Data Source for GridView --%>    
    <asp:ObjectDataSource ID="odsRoles" runat="server" 
        SelectMethod="GetRole" TypeName="SecurityModule" >
        <SelectParameters>
            <asp:Parameter Name="RoleID" Type="Int32" />
            <asp:ControlParameter ControlID="txtRoleName"  Name="RoleName"
                PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="txtDescription" Name="Description" PropertyName="Text"
                Type="String" />
            <asp:Parameter Name="Obsolete" Type="Boolean" />
            <asp:Parameter Name="SortBy" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>

