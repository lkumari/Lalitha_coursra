<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="ECI_Notification_Group_Maint.aspx.vb" Inherits="ECI_Notification_Group_Maint"
    Title="ECI Notification Group Maintenance" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="98%" DefaultButton="btnSearch">
        <asp:Label runat="server" ID="lblMessage"></asp:Label>
        <br />
        <table width="100%">
            <tr>
                <td align="right">
                    <asp:Label runat="server" ID="lblSearchGroupName" Text="Group Name:" CssClass="p_text"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddSearchGroupName" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblTeamMember" Text=" Team Member:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddSearchTeamMember" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" />
                </td>
            </tr>
        </table>
        <br />
        <hr />
        <asp:ValidationSummary runat="server" ID="vsEditGroup" ValidationGroup="vgEditGroup"
            ShowMessageBox="true" ShowSummary="true" />
        <asp:ValidationSummary runat="server" ID="vsInsertGroup" ValidationGroup="vgInsertGroup"
            ShowMessageBox="true" ShowSummary="true" />
        <br />
        <asp:GridView ID="gvGroup" runat="server" AutoGenerateColumns="False" AllowSorting="True"
            AllowPaging="True" PageSize="25" DataKeyNames="GroupID" ShowFooter="True" DataSourceID="odsGroup"
            EmptyDataText="No Records Found." Width="100%">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:BoundField DataField="GroupID" HeaderText="GroupID" ReadOnly="True" />
                <asp:TemplateField HeaderText="Group Name" SortExpression="GroupName">
                    <EditItemTemplate>
                        <asp:Label ID="lblEditGroupNameMarker" runat="server" Font-Bold="True" ForeColor="Red"
                            Text="*" />
                        <asp:TextBox ID="txtEditGroupName" runat="server" Text='<%# Bind("GroupName") %>'
                            MaxLength="50" Width="300px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvECIGroupNameEdit" runat="server" ControlToValidate="txtEditGroupName"
                            ErrorMessage="Group Name is required." Font-Bold="True" ValidationGroup="vgEditGroup"
                            Text="<">				     
                        </asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewGroupName" runat="server" Text='<%# Bind("ddGroupName") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblInsertGroupNameMarker" runat="server" Font-Bold="True" ForeColor="Red"
                            Text="*" />
                        <asp:TextBox ID="txtInsertGroupName" runat="server" Text='<%# Bind("GroupName") %>'
                            MaxLength="50" Width="300px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvECIGroupNameInsert" runat="server" ControlToValidate="txtInsertGroupName"
                            ErrorMessage="Group Name is required." Font-Bold="True" ValidationGroup="vgInsertGroup"
                            Text="<">				    
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
                <asp:TemplateField ShowHeader="False">
                    <EditItemTemplate>
                        <asp:ImageButton ID="iBtnGroupUpdate" runat="server" CausesValidation="True" CommandName="Update"
                            ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditGroup" />
                        <asp:ImageButton ID="iBtnGroupCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                            ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="iBtnGroupEdit" runat="server" CausesValidation="False" CommandName="Edit"
                            ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgInsertGroup"
                            runat="server" ID="iBtnGroupSave" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                        <asp:ImageButton ID="iBtnGroupUndo" runat="server" CommandName="Undo" CausesValidation="false"
                            ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Copy Group">
                    <ItemTemplate>
                        <asp:ImageButton ID="iBtnGroupCopy" runat="server" CausesValidation="False" CommandName="Copy"
                            ImageUrl="~/images/copy.jpg" AlternateText="Copy" OnClick="imageBtnCopyGroup_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsGroup" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetECIGroup" TypeName="ECIGroupBLL" InsertMethod="InsertECIGroup"
            UpdateMethod="UpdateECIGroup">
            <SelectParameters>
                <asp:QueryStringParameter DefaultValue="0" Name="GroupID" QueryStringField="GroupID"
                    Type="Int32" />
                <asp:Parameter Name="GroupName" Type="String" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="GroupName" Type="String" />
                <asp:Parameter Name="original_GroupID" Type="Int32" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="GroupName" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
        <br />
        <hr />
        <asp:ValidationSummary runat="server" ID="vsEditGroupTeamMember" ValidationGroup="vgEditGroupTeamMember"
            ShowMessageBox="true" ShowSummary="true" />
        <asp:ValidationSummary runat="server" ID="vsInsertGroupTeamMember" ValidationGroup="vgInsertGroupTeamMember"
            ShowMessageBox="true" ShowSummary="true" />
        <br />
        <asp:GridView ID="gvGroupTeamMember" runat="server" AutoGenerateColumns="False" AllowSorting="True"
            AllowPaging="True" PageSize="15" DataKeyNames="RowID" ShowFooter="True" DataSourceID="odsGroupTeamMember"
            EmptyDataText="No Records Found." Width="100%">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:TemplateField HeaderText="Group Name" SortExpression="ddGroupName">
                    <EditItemTemplate>
                        <asp:Label ID="lblEditGroupName" runat="server" Text='<%# Bind("ddGroupName") %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewGroupName" runat="server" Text='<%# Bind("ddGroupName") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblInsertGroupMarker" runat="server" Font-Bold="True" ForeColor="Red"
                            Text="*" />
                        <asp:DropDownList ID="ddInsertGroup" runat="server" DataSource='<%# ECIModule.GetECIGroup(0,"") %>'
                            DataValueField="GroupID" DataTextField="ddGroupName" AppendDataBoundItems="True"
                            SelectedValue='<%# Bind("GroupID") %>'>
                            <asp:ListItem Text="" Value="0" Selected="False"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvGroupInsert" runat="server" ControlToValidate="ddInsertGroup"
                            ErrorMessage="Group is required" Font-Bold="True" ValidationGroup="vgInsertGroupTeamMember"
                            Text="<">				    
                        </asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Team Member" SortExpression="ECIGroupName">
                    <EditItemTemplate>
                        <asp:Label ID="lblEditGroupTeamMember" runat="server" Text='<%# Bind("ddTeamMemberName") %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewGroupNameTeamMember" runat="server" Text='<%# Bind("ddTeamMemberName") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblInsertGroupTeamMemberMarker" runat="server" Font-Bold="True" ForeColor="Red"
                            Text="*" />
                        <asp:DropDownList ID="ddInsertGroupTeamMember" runat="server" DataSource='<%# commonFunctions.GetTeamMemberBySubscription(64) %>'
                            DataValueField="TMID" DataTextField="TMName" AppendDataBoundItems="True" SelectedValue='<%# Bind("TeamMemberID") %>'>
                            <asp:ListItem Text="" Value="0" Selected="False"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvInsertGroupTeamMemberInsert" runat="server" ControlToValidate="ddInsertGroupTeamMember"
                            ErrorMessage="Team Member is required." Font-Bold="True" ValidationGroup="vgInsertGroupTeamMember"
                            Text="<">				    
                        </asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:ImageButton ID="ibtnGroupTeamMemberDelete" runat="server" CausesValidation="False"
                            CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgInsertGroupTeamMember"
                            runat="server" ID="iBtnGroupTeamMemberSave" ImageUrl="~/images/save.jpg" AlternateText="Save" />
                        <asp:ImageButton ID="iBtnGroupTeamMemberUndo" runat="server" CommandName="Undo" CausesValidation="false"
                            ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsGroupTeamMember" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetECIGroupTeamMember" TypeName="ECIGroupTeamMemberBLL" InsertMethod="InsertECIGroupTeamMember"
            DeleteMethod="DeleteECIGroupTeamMember">
            <SelectParameters>
                <asp:QueryStringParameter DefaultValue="0" Name="GroupID" QueryStringField="GroupID"
                    Type="Int32" />
                <asp:QueryStringParameter DefaultValue="0" Name="TeamMemberID" QueryStringField="TeamMemberID"
                    Type="Int32" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="GroupID" Type="Int32" />
                <asp:Parameter Name="TeamMemberID" Type="Int32" />
            </InsertParameters>
            <DeleteParameters>
                <asp:Parameter Name="RowID" Type="Int32" />
                <asp:Parameter Name="original_RowID" Type="Int32" />
            </DeleteParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
