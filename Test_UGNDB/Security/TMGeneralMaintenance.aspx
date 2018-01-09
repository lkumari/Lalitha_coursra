<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"  
         CodeFile="TMGeneralMaintenance.aspx.vb" Inherits="Security_TMGeneralMaintenance" 
         title="Team Member Maintenance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" Runat="Server">
    <asp:Label ID="lblErrors" runat="server" SkinID="MessageLabelSkin"></asp:Label>
    <br />
    <!-- Menu Tab Links -->
    &nbsp;&nbsp;&nbsp;
    <asp:Label ID="lblTMGeneralTab" runat="server" 
               Text="&nbsp;&nbsp;&nbsp;General&nbsp;&nbsp;&nbsp;" 
               Font-Bold="True" Font-Underline="False" ForeColor="Black" 
               CssClass="selectedTab" Height="15px" Font-Overline="False"></asp:Label>
    &nbsp;
    <asp:LinkButton ID="lbTMWorkHistoryTab" runat="server" 
                    CssClass="tab">Work History</asp:LinkButton>
    &nbsp;
    <asp:LinkButton ID="lbTMRolesTab" runat="server" 
                    CssClass="tab">Roles and Forms</asp:LinkButton>&nbsp;&nbsp;&nbsp;
    &nbsp;
 
    <!-- Search Fields -->
    <hr />
    <table width="100%">
        <tr>
            <td>User Name:</td>
            <td>
                <asp:TextBox ID="txtUserName" runat="server" 
                    ToolTip="Search for User Name (May use % wildcard characters)" 
                    Width="216px">
                </asp:TextBox>
                &nbsp;
                <asp:RegularExpressionValidator ID="revUserName" runat="server" 
                    ControlToValidate="txtUserName"
                    ErrorMessage="User Name contains invalid characters. Wildcard characters may only be used at the beginning or end."
                    ValidationExpression="^[%]?[\w-\.]{0,50}[%]?$" 
                    ValidationGroup="vgSearch">
                    *
                </asp:RegularExpressionValidator>
            </td>
            <td>&nbsp;</td>
            <td>Email:</td>
            <td>
                <asp:TextBox ID="txtEmail" runat="server" 
                    Width="216px" 
                    ToolTip="Search for Email Address (May use % wildcard characters)">
                </asp:TextBox>
                &nbsp;
                <asp:RegularExpressionValidator ID="revEmail" runat="server" 
                    ControlToValidate="txtEmail"
                    ErrorMessage="Email address contains invalid characters. Wildcard characters may only be used at the beginning or end." 
                    ValidationExpression="^[%]?[\w-\.@]{0,50}[%]?$" 
                    ValidationGroup="vgSearch">
                    *
                </asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td>First Name:</td>
            <td>
                <asp:TextBox ID="txtFName" runat="server" 
                    ToolTip="Search for First Name (May use % wildcard characters)" 
                    Width="216px">
                </asp:TextBox>
                &nbsp;
                <asp:RegularExpressionValidator ID="vreFirstName" runat="server" 
                    ControlToValidate="txtFName"
                    ErrorMessage="Description  must  contain letters or numbers. Wildcard characters may only be used at the beginning or end."
                    ValidationExpression="^[%]?[\d\w-'\s\.\,]{1,50}[%]?$" 
                    ValidationGroup="vgSearch"> 
                    *
                </asp:RegularExpressionValidator>
            </td>
            <td>&nbsp;</td>
            <td>Last Name:</td>
            <td>
                <asp:TextBox ID="txtLName" runat="server" 
                    Width="216px" 
                    ToolTip="Search for Last Name (May use % wildcard characters)">
                </asp:TextBox>
                &nbsp;
                <asp:RegularExpressionValidator ID="revLName" runat="server" 
                    ControlToValidate="txtLName" 
                    ErrorMessage="Last name contains invalid characters. Wildcard characters may only be used at the beginning or end." 
                    ValidationExpression="^[%]?[\d\w-'\s\.\,]{1,50}[%]?$" 
                    ValidationGroup="vgSearch">
                    *
                </asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td>Status:</td>
            <td>
                <asp:RadioButtonList ID="optWorkStatusList" runat="server" 
                    RepeatDirection="Horizontal" RepeatLayout="Flow" 
                    Width="224px">
                    <asp:ListItem>Working</asp:ListItem>
                    <asp:ListItem Value="NotWorking">Not Working</asp:ListItem>
                    <asp:ListItem Selected="True">Both</asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td colspan="4">
                <asp:ValidationSummary ID="vsSearch" runat="server" 
                    ValidationGroup="vgSearch" Width="500px" ShowMessageBox="True" />
            </td>
        </tr>
        <tr>
            <td colspan="5" align="center">
                <asp:Button ID="btnSearch" runat="server" Text="Search" ValidationGroup="vgSearch" CausesValidation="True" />&nbsp;
                <asp:Button ID="btnResetSearch" runat="server" Text="Reset" CausesValidation="False" />
            </td>
        </tr>
    </table>
    <hr />
    <asp:Label ID="lblCurrentPage" runat="server" />
    <asp:GridView ID="gvTeamMembers" runat="server"
        Width="1000px" 
        DataKeyNames="TeamMemberID"
        AllowPaging="True" 
        AllowSorting="True" 
        ShowFooter="True"
        AutoGenerateColumns="False"
        PagerSettings-Mode="NumericFirstLast"
        DataSourceID="odsTeamMembers">
        
        <FooterStyle BackColor="#CCCCCC"  Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#CCCCCC" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <EmptyDataTemplate>No records found from the database.</EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText = "ID" SortExpression="TeamMemberID">
                <FooterStyle HorizontalAlign="Right" Wrap="False" />
                <FooterTemplate>
                    <asp:Image ID="imgAsterisk" runat="server" ImageUrl="~/images/asterick_blue.gif" 
                        AlternateText="New row"/>
                    &nbsp;
                </FooterTemplate>
                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                <ItemTemplate>
                    <asp:LinkButton ID="lnkTeamMemberId" runat="server" 
                        Text='<%# Eval("TeamMemberID") %>' OnClick="lnkTeamMemberId_Click" 
                        ToolTip="Show Work History" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <EditItemTemplate>
                    <asp:ImageButton ID="ibtnEditGetAD" runat="server" 
                        CommandName="EditGetAD"
                        CausesValidation="False" 
                        ImageUrl="~/images/Search.gif" 
                        Tooltip="Fill UserName, FirstName, LastName, Email from Active Directory" 
                        AlternateText="Fill UserName, FirstName, LastName, Email from Active Directory"  />
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:ImageButton ID="ibtnInsertGetAD" runat="server" 
                        CommandName="InsertGetAD"
                        CausesValidation="False" 
                        ImageUrl="~/images/Search.gif" 
                        Tooltip="Fill UserName, FirstName, LastName, Email from Active Directory" 
                        AlternateText="Fill UserName, FirstName, LastName, Email from Active Directory"  />
                </FooterTemplate>
                <FooterStyle HorizontalAlign="Center" Wrap="False" />
                <ItemStyle HorizontalAlign="Center" Wrap="False" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText = "User Name" SortExpression="UserName">
                <EditItemTemplate>
                    <asp:TextBox ID="txtUserNameEdit" runat="server" Text='<%# Bind("UserName") %>' />
                    <asp:RequiredFieldValidator ID="rfvUserNameEdit" runat="server"
                        ControlToValidate="txtUserNameEdit"
                        ErrorMessage="User Name is required"
                        ValidationGroup="vgEditTMInfo">
                        &lt;
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revUserNameEdit" runat="server"
                        ControlToValidate="txtUserNameEdit"
                        ValidationGroup="vgEditTMInfo"
                        ErrorMessage="User Name contains invalid data"
                        ValidationExpression="^[a-zA-Z-]{1,20}\.[a-zA-Z-]{1,20}$">
                        &lt;
                    </asp:RegularExpressionValidator>  
                </EditItemTemplate>
                <FooterStyle HorizontalAlign="Left" Wrap="False" />
                <FooterTemplate>
                    <asp:TextBox ID="txtUserNameInsert" runat="server" Text='<%# Bind("UserName") %>' />
                    <asp:RequiredFieldValidator ID="rfvUserNameInsert" runat="server"
                        ControlToValidate="txtUserNameInsert"
                        ErrorMessage="User Name is required"
                        ValidationGroup="vgInsertTMInfo">
                        &lt;
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revUserNameInsert" runat="server"
                        ControlToValidate="txtUserNameInsert"
                        ValidationGroup="vgInsertTMInfo"
                        ErrorMessage="User Name contains invalid data"
                        ValidationExpression="^[a-zA-Z-]{1,20}\.[a-zA-Z-]{1,20}$">
                        &lt;
                    </asp:RegularExpressionValidator>  
                </FooterTemplate>
                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                <ItemTemplate>
                    <asp:LinkButton ID="lnkUserName" runat="server" 
                        Text='<%# Eval("UserName") %>' OnClick="lnkUserName_Click" 
                        ToolTip="Show Roles and Forms" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText = "First Name" SortExpression="FirstName">
                <EditItemTemplate>
                    <asp:TextBox ID="txtFirstNameEdit" runat="server" Text='<%# Bind("FirstName") %>' />
                    <asp:RequiredFieldValidator ID="rfvFirstNameEdit" runat="server"
                        ControlToValidate="txtFirstNameEdit"
                        ErrorMessage="First Name is required"
                        ValidationGroup="vgEditTMInfo">
                        &lt;
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revFirstNameEdit" runat="server"
                        ControlToValidate="txtFirstNameEdit"
                        ValidationGroup="vgEditTMInfo"
                        ErrorMessage="First Name contains invalid data"
                        ValidationExpression="^[a-zA-Z-'\s]{1,30}$">
                        &lt;
                    </asp:RegularExpressionValidator>  
                </EditItemTemplate>
                <FooterStyle HorizontalAlign="Left" Wrap="False" />
                <FooterTemplate>
                    <asp:TextBox ID="txtFirstNameInsert" runat="server" Text='<%# Bind("FirstName") %>' />
                    <asp:RequiredFieldValidator ID="rfvFirstNameInsert" runat="server"
                        ControlToValidate="txtFirstNameInsert"
                        ErrorMessage="First Name is required"
                        ValidationGroup="vgInsertTMInfo">
                        &lt;
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revFirstNameInsert" runat="server"
                        ControlToValidate="txtFirstNameInsert"
                        ValidationGroup="vgInsertTMInfo"
                        ErrorMessage="First Name contains invalid data"
                        ValidationExpression="^[a-zA-Z-'\s]{1,30}$">
                        &lt;
                    </asp:RegularExpressionValidator>  
                </FooterTemplate>
                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                <ItemTemplate>
                    <asp:Label ID="lblFirstNamePreEdit" runat="server" Text='<%# Eval("FirstName") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText = "Last Name" SortExpression="LastName">
                <EditItemTemplate>
                    <asp:TextBox ID="txtLastNameEdit" runat="server" Text='<%# Bind("LastName") %>' />
                    <asp:RequiredFieldValidator ID="rfvLastNameEdit" runat="server"
                        ControlToValidate="txtLastNameEdit"
                        ErrorMessage="Last Name is required"
                        ValidationGroup="vgEditTMInfo">
                        &lt;
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revLastNameEdit" runat="server"
                        ControlToValidate="txtLastNameEdit"
                        ValidationGroup="vgEditTMInfo"
                        ErrorMessage="Last Name contains invalid data"
                        ValidationExpression="^[a-zA-Z-'\s]{1,30}$">
                        &lt;
                    </asp:RegularExpressionValidator>  
                </EditItemTemplate>
                <FooterStyle HorizontalAlign="Left" Wrap="False" />
                <FooterTemplate>
                    <asp:TextBox ID="txtLastNameInsert" runat="server" Text='<%# Bind("LastName") %>' />
                    <asp:RequiredFieldValidator ID="rfvLastNameInsert" runat="server"
                        ControlToValidate="txtLastNameInsert"
                        ErrorMessage="Last Name is required"
                        ValidationGroup="vgInsertTMInfo">
                        &lt;
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revLastNameInsert" runat="server"
                        ControlToValidate="txtLastNameInsert"
                        ValidationGroup="vgInsertTMInfo"
                        ErrorMessage="Last Name contains invalid data"
                        ValidationExpression="^[a-zA-Z-'\s]{1,30}$">
                        &lt;
                    </asp:RegularExpressionValidator>  
                </FooterTemplate>
                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                <ItemTemplate>
                    <asp:Label ID="lblLastNamePreEdit" runat="server" Text='<%# Eval("LastName") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText = "Email" SortExpression="Email">
                <EditItemTemplate>
                    <asp:TextBox ID="txtEmailEdit" runat="server" Text='<%# Bind("Email") %>' />
                    <asp:RequiredFieldValidator ID="rfvEmailEdit" runat="server"
                        ControlToValidate="txtEmailEdit"
                        ErrorMessage="Email address is required"
                        ValidationGroup="vgEditTMInfo">
                        &lt;
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revEmailEdit" runat="server"
                        ControlToValidate="txtEmailEdit"
                        ValidationGroup="vgEditTMInfo"
                        ErrorMessage="Invalid Email address"
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                        &lt;
                    </asp:RegularExpressionValidator>  
                </EditItemTemplate>
                <FooterStyle HorizontalAlign="Left" Wrap="False" />
                <FooterTemplate>
                    <asp:TextBox ID="txtEmailInsert" runat="server" Text='<%# Bind("Email") %>' />
                    <asp:RequiredFieldValidator ID="rfvEmailInsert" runat="server"
                        ControlToValidate="txtEmailInsert"
                        ErrorMessage="Email address is required"
                        ValidationGroup="vgInsertTMInfo">
                        &lt;
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revEmailInsert" runat="server"
                        ControlToValidate="txtEmailInsert"
                        ValidationGroup="vgInsertTMInfo"
                        ErrorMessage="Invalid Email address"
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                        &lt;
                    </asp:RegularExpressionValidator>  
                </FooterTemplate>
                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                <ItemTemplate>
                    <asp:Label ID="lblEmailPreEdit" runat="server" Text='<%# Eval("Email") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText = "Working" SortExpression="Working">
                <EditItemTemplate>
                    <asp:CheckBox ID="chkWorkingEdit" runat="server" Checked='<%# Bind("Working") %>' />
                </EditItemTemplate>
                <FooterStyle HorizontalAlign="Center" Wrap="False" />
                <FooterTemplate>
                    <asp:CheckBox ID="chkWorkingInsert" runat="server" Checked='<%# Bind("Working") %>' />
                </FooterTemplate>
                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                <ItemTemplate>
                    <asp:CheckBox ID="chkWorkingPreEdit" runat="server" Checked='<%# Eval("Working") %>' Enabled="False" />
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField ShowHeader="False" AccessibleHeaderText="Button Column">
                <EditItemTemplate>
                    <asp:ImageButton ID="ibtnUpdate" runat="server" 
                        CommandName="UpdateCustom" 
                        CausesValidation="True" ValidationGroup="vgEditTMInfo"
                        ImageUrl="~/images/save.jpg" 
                        Tooltip="Save changes" AlternateText="Save changes" OnClick="ibtnUpdate_Click"  />
                    <asp:ImageButton ID="ibtnCancelEdit" runat="server"
                        CommandName="Cancel" 
                        CausesValidation="False" 
                        ImageUrl="~/images/undo-transparent.gif" 
                        ToolTip="Undo changes" AlternateText="Undo changes" />
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:ImageButton ID="ibtnInsert" runat="server" 
                        CommandName="InsertCustom" 
                        CausesValidation="true" ValidationGroup="vgInsertTMInfo"
                        ImageUrl="~/images/save.jpg"
                        ToolTip="Save new row" AlternateText="Save new row" />
                    <asp:ImageButton ID="ibtnCancelInsert" runat="server"
                        CommandName="Cancel" 
                        CausesValidation="False" 
                        ImageUrl="~/images/undo-transparent.gif" 
                        ToolTip="Undo changes" AlternateText="Undo changes" />
                </FooterTemplate>
                <FooterStyle HorizontalAlign="Center" Wrap="False" />
                <ItemTemplate>
                    <asp:ImageButton ID="ibtnEdit" runat="server"
                        CommandName="Edit" 
                        CausesValidation="False" 
                        ImageUrl="~/images/edit.jpg" 
                        ToolTip="Edit row" AlternateText="Edit row" />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" Wrap="False" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Last Update" SortExpression="comboUpdateInfo">
                <ItemTemplate>
                    <asp:Label ID="lblComboUpdateInfo" runat="server" Text='<%# Eval("comboUpdateInfo") %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerSettings Mode="NumericFirstLast" />
    </asp:GridView>

     &nbsp;&nbsp;
     
     <%-- Validation Summary for GridView controls --%>
     <asp:ValidationSummary ID="vsEditTMInfo" runat="server" ShowMessageBox="True"
        Width="599px" ValidationGroup="vgEditTMInfo" />
     <asp:ValidationSummary ID="vsInsertTMInfo" runat="server" ShowMessageBox="True"
        Width="599px" ValidationGroup="vgInsertTMInfo" />
     <asp:Label ID="lblStatus" runat="server" />
    <br />

    <br />
    <asp:ObjectDataSource ID="odsTeamMembers" runat="server" 
                          SelectMethod="GetTeamMember"
                          TypeName="SecurityModule"
                          OldValuesParameterFormatString="{0}" >
        <SelectParameters>
            <asp:Parameter Name="TeamMemberID" Type="Int32" />
            <asp:ControlParameter ControlID="txtUserName" 
                ConvertEmptyStringToNull="False" Name="UserName"
                PropertyName="Text" Type="String" />
            <asp:Parameter Name="ShortName" Type="String" />
            <asp:ControlParameter ControlID="txtLName" Name="LastName" PropertyName="Text"
                Type="String" />
            <asp:ControlParameter ControlID="txtFName" Name="FirstName" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="txtEmail" Name="Email" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="optWorkStatusList" Name="Working" PropertyName="SelectedValue"
                Type="Object" />
            <asp:Parameter Name="SortBy" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>

</asp:Content>

