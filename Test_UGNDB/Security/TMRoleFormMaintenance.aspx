<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="TMRoleFormMaintenance.aspx.vb" Inherits="Security_TMRoleFormMaintenance"
    Title="Team Member Roles" Debug="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Label ID="lblErrors" runat="server" SkinID="MessageLabelSkin"></asp:Label>
    <br />
    <!-- Menu Tab Links -->
    &nbsp;&nbsp;&nbsp;
    <asp:LinkButton ID="lbTMGeneralTab" runat="server" Font-Overline="False" Font-Underline="False"
        CssClass="tab">
    General
    </asp:LinkButton>
    &nbsp;
    <asp:LinkButton ID="lbTMWorkHistoryTab" runat="server" CssClass="tab">
    Work History
    </asp:LinkButton>
    &nbsp;
    <asp:Label ID="lblTMRolesTab" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;Roles and Forms&nbsp;&nbsp;&nbsp;&nbsp;"
        Font-Bold="True" CssClass="selectedTab" Height="15px"></asp:Label>
    <br />
    <br />
    <!-- Collapsible Edit User Pane -->
    <ajax:Accordion ID="accEditUser" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
        HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
        FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="none"
        RequireOpenedPane="false" SuppressHeaderPostbacks="true" ToolTip="View and edit a Team Member's Role/Form profile.">
        <Panes>
            <ajax:AccordionPane ID="accEditUserPane" runat="server">
                <Header>
                    <a href="" onclick="return false;">1. View/Edit a Team Member</a>
                </Header>
                <Content>
                    <div style="padding-left: 15px; padding-top: 10px;">
                        <asp:DropDownList ID="ddlLookupUser" runat="server" AutoPostBack="True" Width="300px"
                            ToolTip="Select a Team Member">
                        </asp:DropDownList>
                        &nbsp;&nbsp;
                        <asp:Label ID="lblTeamMemberId" runat="server" ForeColor="#2F4F4F" Font-Names="Courier New">
                        </asp:Label>
                        <asp:HiddenField ID="hfTeamMemberId" runat="server" />
                        <br />
                        <br />
                        <asp:GridView ID="gvTMRoleForm" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            DataSourceID="odsTMRoleForm" ShowFooter="True" AllowSorting="True">
                            <FooterStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#CCCCCC" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <EmptyDataTemplate>
                                No Data to display.
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="Form Id" SortExpression="FormID">
                                    <FooterStyle HorizontalAlign="Right" Wrap="False" />
                                    <FooterTemplate>
                                        <asp:Image ID="imgAsterisk" runat="server" ImageUrl="~/images/asterick_blue.gif"
                                            AlternateText="New row" />
                                        &nbsp;
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblFormID" runat="server" Text='<%# Bind("FormID") %>'>
                                        </asp:Label>
                                        &nbsp;
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Form Name" SortExpression="FormName">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFormName" runat="server" Text='<%# Bind("comboFormNameObsolete") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlNewForm" runat="server" DataSourceID="odsForms" DataTextField="FormName"
                                            DataValueField="FormID" AppendDataBoundItems="True" ToolTip="Select a new form">
                                            <asp:ListItem Text="" Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="vrfNewForm" runat="server" ControlToValidate="ddlNewForm"
                                            Display="Dynamic" ErrorMessage="Please select a new Form" ValidationGroup="vgInsert">
                                     &lt; 
                                        </asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Role Id" SortExpression="RoleID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRoleID" runat="server" Text='<%# Bind("RoleID") %>'></asp:Label>
                                        &nbsp;
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Role Name" SortExpression="RoleName">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlEditRole" runat="server" DataSourceID="odsRoles" DataTextField="RoleName"
                                            DataValueField="RoleID" SelectedValue='<%# Bind("RoleID") %>' AppendDataBoundItems="True"
                                            ToolTip="Select a new role">
                                            <asp:ListItem Text="" Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="vrfEditRole" runat="server" ControlToValidate="ddlEditRole"
                                            Display="Dynamic" ErrorMessage="Please select a Role" ValidationGroup="vgEdit">
                                   &lt;
                                        </asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRoleName" runat="server" Text='<%# Bind("comboRoleNameObsolete") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlNewRole" runat="server" DataSourceID="odsRoles" DataTextField="RoleName"
                                            DataValueField="RoleID" AppendDataBoundItems="True" ToolTip="Select a new role">
                                            <asp:ListItem Text="" Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="vrfNewRole" runat="server" ControlToValidate="ddlNewRole"
                                            Display="Dynamic" ErrorMessage="Please select a new Role" ValidationGroup="vgInsert">
                                   &lt;
                                        </asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False" AccessibleHeaderText="Button Column">
                                    <EditItemTemplate>
                                        <asp:ImageButton ID="ibtnUpdate" runat="server" CommandName="UpdateCustom" CausesValidation="True"
                                            ValidationGroup="vgEdit" ImageUrl="~/images/save.jpg" ToolTip="Save changes"
                                            AlternateText="Save changes" OnClick="ibtnUpdate_Click" />
                                        <asp:ImageButton ID="ibtnCancelEdit" runat="server" CommandName="Cancel" CausesValidation="False"
                                            ImageUrl="~/images/undo-transparent.gif" ToolTip="Undo changes" AlternateText="Undo changes" />
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibtnEdit" runat="server" CommandName="Edit" CausesValidation="False"
                                            ImageUrl="~/images/edit.jpg" ToolTip="Edit row" AlternateText="Edit row" />
                                        <asp:ImageButton CommandName="DeleteCustom" CausesValidation="true" runat="server"
                                            ID="ibtnDelete" ImageUrl="~/images/delete.jpg" AlternateText="Delete row" ToolTip="Delete row"
                                            OnClick="ibtnDelete_Click" />
                                        <!-- OnClientClick="return confirm('Are you sure you want to delete this record?');" -->
                                    </ItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" Wrap="False" />
                                    <FooterTemplate>
                                        <asp:ImageButton CommandName="InsertCustom" CausesValidation="true" runat="server"
                                            ID="ibtnInsert" ImageUrl="~/images/save.jpg" AlternateText="Save new row" ToolTip="Save new row"
                                            ValidationGroup="vgInsert" />
                                        <asp:ImageButton CommandName="Cancel" CausesValidation="true" runat="server" ID="ibtnCancel"
                                            ImageUrl="~/images/undo-transparent.gif" AlternateText="Undo changes" ToolTip="Undo changes" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Last Updated" SortExpression="comboUpdateInfo">
                                    <ItemTemplate>
                                        <asp:Label ID="lblComboUpdateInfo" runat="server" Text='<%# Eval("comboUpdateInfo") %>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:Label ID="lblStatus" runat="server"></asp:Label><br />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="vgInsert"
                            ShowMessageBox="True" />
                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="vgEdit"
                            ShowMessageBox="True" />
                        <asp:ObjectDataSource ID="odsTMRoleForm" runat="server" SelectMethod="GetTMRoleForm"
                            TypeName="SecurityModule" OldValuesParameterFormatString="original_{0}">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="ddlLookupUser" Name="TeamMemberID" PropertyName="SelectedValue"
                                    Type="Int32" />
                                <asp:Parameter Name="RoleID" Type="Int32" />
                                <asp:Parameter Name="FormID" Type="Int32" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </div>
                </Content>
            </ajax:AccordionPane>
        </Panes>
    </ajax:Accordion>
    <!-- Collapsible Copy Profiles Pane -->
    <ajax:Accordion ID="accCopyRolesForms" runat="server" SelectedIndex="-1" HeaderCssClass="accordionHeader"
        HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
        FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="none"
        RequireOpenedPane="false" SuppressHeaderPostbacks="true" ToolTip="View and copy another Team Member's Role/Form profile.">
        <Panes>
            <ajax:AccordionPane ID="accCopyRolesFormsPane" runat="server">
                <Header>
                    <a href="" onclick="return false;">2. View/Copy another Team Member</a>
                </Header>
                <Content>
                    <div style="padding-left: 15px; padding-top: 10px; padding-bottom: 10px;">
                        <asp:DropDownList ID="ddlCopyFrom" runat="server" Width="300px" ToolTip="Select a Team Member for Foles and Forms copy."
                            AutoPostBack="True" Font-Bold="False">
                        </asp:DropDownList>
                        &nbsp;
                        <asp:Button ID="btnCopyFrom" runat="server" Text="Copy" CausesValidation="False"
                            Enabled="False" />
                        <asp:Label ID="lblCopyMessage" runat="server" Font-Names="Courier New" />
                        <br />
                        <br />
                        <asp:GridView ID="gvCopyFrom" runat="server" AllowPaging="True" AllowSorting="True"
                            AutoGenerateColumns="False" DataSourceID="odsTMRoleFormCopy" ForeColor="Black"
                            EmptyDataText="No Data to Display">
                            <FooterStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" Wrap="true" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <EmptyDataRowStyle BackColor="#F7F6F3" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" />
                            <Columns>
                                <asp:TemplateField HeaderText="Form Id" SortExpression="FormID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFormID_Copy" runat="server" Text='<%# Bind("FormID") %>'></asp:Label>
                                        &nbsp;
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Form Name" SortExpression="FormName">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFormName_Copy" runat="server" Text='<%# Bind("comboFormNameObsolete") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Role Id" SortExpression="RoleID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRoleID_Copy" runat="server" Text='<%# Bind("RoleID") %>'></asp:Label>
                                        &nbsp;
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Role Name" SortExpression="RoleName">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRoleName_Copy" runat="server" Text='<%# Bind("comboRoleNameObsolete") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Last Updated" SortExpression="comboUpdateInfo">
                                    <ItemTemplate>
                                        <asp:Label ID="lblComboUpdateInfo_Copy" runat="server" Text='<%# Eval("comboUpdateInfo") %>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsTMRoleFormCopy" runat="server" SelectMethod="GetTMRoleForm"
                            TypeName="SecurityModule" OldValuesParameterFormatString="original_{0}">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="ddlCopyFrom" Name="TeamMemberID" PropertyName="SelectedValue"
                                    Type="Int32" />
                                <asp:Parameter Name="RoleID" Type="Int32" />
                                <asp:Parameter Name="FormID" Type="Int32" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </div>
                </Content>
            </ajax:AccordionPane>
        </Panes>
    </ajax:Accordion>
    <asp:ObjectDataSource ID="odsForms" runat="server" OldValuesParameterFormatString="original_{0}"
        SelectMethod="GetForm" TypeName="SecurityModule">
        <SelectParameters>
            <asp:Parameter Name="FormID" Type="Int32" />
            <asp:Parameter Name="FormName" Type="String" />
            <asp:Parameter Name="HyperlinkID" Type="String" />
            <asp:Parameter DefaultValue="False" Name="Obsolete" Type="Boolean" />
            <asp:Parameter Name="SortBy" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsRoles" runat="server" OldValuesParameterFormatString="original_{0}"
        SelectMethod="GetRole" TypeName="SecurityModule">
        <SelectParameters>
            <asp:Parameter Name="RoleID" Type="Int32" />
            <asp:Parameter Name="RoleName" Type="String" />
            <asp:Parameter Name="Description" Type="String" />
            <asp:Parameter DefaultValue="False" Name="Obsolete" Type="Boolean" />
            <asp:Parameter Name="SortBy" Type="Object" DefaultValue="" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
