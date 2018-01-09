<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Team_Member_WorkFlow.aspx.vb" Inherits="Workflow_Team_Member_WorkFlow"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
        Width="1000px" Visible="False" />
    <br />
    <asp:Panel ID="localPanel" runat="server">
        <hr />
        <table>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblTeamMember" runat="server" Text="Team Member:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddTeamMember" runat="server" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblSubscription" runat="server" Text="Subscription:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddSubscription" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="3">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" />
                </td>
            </tr>
        </table>
        <hr />
        <em class="p_smalltextbold">Use the parameters above to filter the list below.</em>
        <br />
        <br />
        <asp:GridView ID="gvWorkFlow" runat="server" AutoGenerateColumns="False" DataSourceID="odsWorkFlow"
            DataKeyNames="TeamMemberID,SubscriptionID" SkinID="StandardGrid" AllowSorting="True"
            AllowPaging="True" OnRowCommand="gvWorkFlow_RowCommand" OnRowDataBound="gvWorkFlow_RowDataBound"
            PageSize="30" Width="1000px">
            <Columns>
                <asp:TemplateField HeaderText="Team Member" SortExpression="TeamMemberName">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddTeamMember" runat="server" DataSource='<%# commonFunctions.GetTeamMember("") %>'
                            DataValueField="TeamMemberID" DataTextField="TeamMemberName" SelectedValue='<%# Bind("TeamMemberID") %>'>
                            <asp:ListItem Selected="True">
                            </asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvTeamMember" runat="server" ControlToValidate="ddTeamMember"
                            ErrorMessage="Team Member is a required field." ValidationGroup="EditWorkFlow"><</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("TeamMemberName") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddTeamMember" runat="server" DataSource='<%# commonFunctions.GetTeamMember("") %>'
                            DataValueField="TeamMemberID" DataTextField="TeamMemberName" SelectedValue='<%# Bind("TeamMemberID") %>'
                            AppendDataBoundItems="true">
                            <asp:ListItem Selected="True">
                            </asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvTeamMember" runat="server" ControlToValidate="ddTeamMember"
                            ErrorMessage="Team Member is a required field." ValidationGroup="InsertWorkFlow"><</asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Subscription" SortExpression="Subscription">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddSubscription" runat="server" DataSource='<%# commonFunctions.GetSubscriptions("") %>'
                            DataValueField="SubscriptionID" DataTextField="Subscription" SelectedValue='<%# Bind("SubscriptionID") %>'>
                            <asp:ListItem Selected="True">
                            </asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvSubscription" runat="server" ControlToValidate="ddSubscription"
                            ErrorMessage="Subscription is a required field." ValidationGroup="EditWorkFlow"><</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("Subscription") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddSubscription" runat="server" DataSource='<%# commonFunctions.GetSubscriptions("") %>'
                            DataValueField="SubscriptionID" DataTextField="Subscription" SelectedValue='<%# Bind("SubscriptionID") %>'
                            AppendDataBoundItems="true">
                            <asp:ListItem Selected="True">
                            </asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvSubscription" runat="server" ControlToValidate="ddSubscription"
                            ErrorMessage="Subscription is a required field." ValidationGroup="InsertWorkFlow"><</asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Backup Team Member" SortExpression="BackupTMName">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddBackupTeamMember" runat="server" DataSource='<%# commonFunctions.GetTeamMember("") %>'
                            DataValueField="TeamMemberID" DataTextField="TeamMemberName" SelectedValue='<%# Bind("BackupTeamMemberID") %>'>
                            <asp:ListItem Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvBackupTeamMember" runat="server" ControlToValidate="ddBackupTeamMember"
                            ErrorMessage="Backup Team Member is a required field." ValidationGroup="EditWorkFlow"><</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("BackupTMName") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddBackupTeamMember" runat="server" DataSource='<%# commonFunctions.GetTeamMember("") %>'
                            DataValueField="TeamMemberID" DataTextField="TeamMemberName" SelectedValue='<%# Bind("BackupTeamMemberID") %>'
                            AppendDataBoundItems="true">
                            <asp:ListItem Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvBackupTeamMember" runat="server" ControlToValidate="ddBackupTeamMember"
                            ErrorMessage="Backup Team Member is a required field." ValidationGroup="InsertWorkFlow"><</asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Department In Charge" SortExpression="DeptInChargeName">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddDeptInCharge" runat="server" DataSource='<%# commonFunctions.GetTeamMember("") %>'
                            DataValueField="TeamMemberID" DataTextField="TeamMemberName" SelectedValue='<%# Bind("DeptInChargeTMID") %>'>
                            <asp:ListItem Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvDeptInCharge" runat="server" ControlToValidate="ddDeptInCharge"
                            ErrorMessage="Department In Charge is a required field." ValidationGroup="EditWorkFlow"><</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("DeptInChargeName") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddDeptInCharge" runat="server" DataSource='<%# commonFunctions.GetTeamMember("") %>'
                            DataValueField="TeamMemberID" DataTextField="TeamMemberName" SelectedValue='<%# Bind("DeptInChargeTMID") %>'
                            AppendDataBoundItems="true">
                            <asp:ListItem Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvDeptInCharge" runat="server" ControlToValidate="ddDeptInCharge"
                            ErrorMessage="Department In Charge is a required field." ValidationGroup="InsertWorkFlow"><</asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:BoundField DataField="UpdateInfo" HeaderText="Last Update" ReadOnly="True" SortExpression="UpdateInfo">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:TemplateField ShowHeader="False">
                    <EditItemTemplate>
                        <asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True" CommandName="Update"
                            ImageUrl="~/images/save.jpg" Text="Update" AlternateText="Update" ValidationGroup="EditWorkFlow" />&nbsp;&nbsp;&nbsp;<asp:ImageButton
                                ID="ibtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                ImageUrl="~/images/cancel.jpg" Text="Cancel" AlternateText="Cancel" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False" CommandName="Edit"
                            ImageUrl="~/images/edit.jpg" Text="Edit" AlternateText="Edit" />&nbsp;&nbsp;&nbsp;
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:ImageButton ID="ibtnInsert" runat="server" CausesValidation="True" CommandName="Insert"
                            ImageUrl="~/images/save.jpg" Text="Update" AlternateText="Insert" ValidationGroup="InsertWorkFlow" />&nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo" ImageUrl="~/images/undo-gray.jpg"
                            Text="Undo" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <EditItemTemplate>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="ibtnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                            ImageUrl="~/images/delete.jpg" Text="Delete" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');"
                            Visible='<%# ViewState("ObjectRole")%>' />
                    </ItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ValidationSummary ID="vsEditWorkFlow" runat="server" ShowMessageBox="True" ValidationGroup="EditWorkFlow" />
        <asp:ValidationSummary ID="vsInsertWorkFlow" runat="server" ShowMessageBox="True"
            ValidationGroup="InsertWorkFlow" />
        <asp:ValidationSummary ID="vsEmptyWorkFlow" runat="server" ShowMessageBox="True"
            ValidationGroup="EmptyWorkFlow" />
        <asp:ObjectDataSource ID="odsWorkFlow" runat="server" DeleteMethod="DeleteWorkFlow"
            InsertMethod="AddWorkFlow" OldValuesParameterFormatString="original_{0}" SelectMethod="GetWorkFlow"
            TypeName="WorkFlowBLL" UpdateMethod="UpdateWorkFlow">
            <DeleteParameters>
                <asp:Parameter Name="TeamMemberID" Type="Int32" />
                <asp:Parameter Name="SubscriptionID" Type="Int32" />
                <asp:Parameter Name="original_TeamMemberID" Type="Int32" />
                <asp:Parameter Name="original_SubscriptionID" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="TeamMemberID" Type="Int32" />
                <asp:Parameter Name="SubscriptionID" Type="Int32" />
                <asp:Parameter Name="BackupTeamMemberID" Type="Int32" />
                <asp:Parameter Name="DeptInChargeTMID" Type="Int32" />
                <asp:Parameter Name="original_TeamMemberID" Type="Int32" />
                <asp:Parameter Name="original_SubscriptionID" Type="Int32" />
            </UpdateParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="TeamMemberID" QueryStringField="sTeamMember" Type="Int32"
                    DefaultValue="0" />
                <asp:QueryStringParameter Name="SubscriptionID" QueryStringField="sSubscription"
                    Type="Int32" DefaultValue="0" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="TeamMemberID" Type="Int32" />
                <asp:Parameter Name="SubscriptionID" Type="Int32" />
                <asp:Parameter Name="BackupTeamMemberID" Type="Int32" />
                <asp:Parameter Name="DeptInChargeTMID" Type="Int32" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
