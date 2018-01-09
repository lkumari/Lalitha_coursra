<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    MaintainScrollPositionOnPostback="true" CodeFile="Cost_Sheet_Notification_Maint.aspx.vb"
    Inherits="Cost_Sheet_Notification_Maint" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server">
        <asp:Label ID="lblMessage" SkinID="MessageLabelSkin" runat="server"></asp:Label>
        <table width="98%">
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblGroupName" Text="Group Name:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddSearchGroupName" runat="server" Width="200px" Visible="false">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblTeamMember" Text=" Team Member:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddSearchTeamMember" runat="server" Width="200px" Visible="false">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button runat="server" ID="btnSearch" Text="Search" Visible="false" />
                    &nbsp;
                    <asp:Button runat="server" ID="btnReset" Text="Reset" Visible="false" />
                </td>
            </tr>
        </table>
        <br />
        <hr />
        <h1>
            List of Groups</h1>
        <asp:ValidationSummary ID="vsFooterGroup" runat="server" ShowMessageBox="True" ShowSummary="true"
            ValidationGroup="vgFooterGroup" />
        <asp:ValidationSummary ID="vsEditGroup" runat="server" ShowMessageBox="True" ShowSummary="true"
            ValidationGroup="vgEditGroup" />
        <asp:GridView ID="gvGroup" runat="server" AutoGenerateColumns="False" DataKeyNames="GroupID"
            AllowSorting="True" AllowPaging="True" PageSize="15" ShowFooter="True" DataSourceID="odsCostSheetGroup"
            EmptyDataText="No records found." Width="98%" Visible="false">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:BoundField DataField="GroupID" HeaderText="GroupID" ReadOnly="True" />
                <asp:TemplateField HeaderText="Group" SortExpression="ddGroupName">
                    <EditItemTemplate>
                        <asp:Label ID="lblEditGroupNameMarker" runat="server" Font-Bold="True" ForeColor="Red"
                            Text="*" />
                        <asp:TextBox ID="txtEditGroupName" runat="server" MaxLength="50" Width="400px" Text='<%# Bind("GroupName") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvEditGroupName" runat="server" ControlToValidate="txtEditGroupName"
                            ErrorMessage="The name is required." Font-Bold="True" ValidationGroup="vgEditGroup"
                            Text="<" SetFocusOnError="true" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewGroupName" runat="server" Text='<%# Bind("ddGroupName") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblFooterGroupNameMarker" runat="server" Font-Bold="True" ForeColor="Red"
                            Text="*" />
                        <asp:TextBox ID="txtFooterGroupName" runat="server" MaxLength="50" Width="400px"
                            Text='<%# Bind("GroupName") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvFooterGroupName" runat="server" ControlToValidate="txtFooterGroupName"
                            ErrorMessage="The name is required." Font-Bold="True" ValidationGroup="vgFooterGroup"
                            Text="<" SetFocusOnError="true" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Obsolete" SortExpression="Obsolete">
                    <EditItemTemplate>
                        <asp:CheckBox ID="cbEditGroupObsolete" runat="server" Checked='<%# Bind("Obsolete") %>' />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="cbViewGroupObsolete" runat="server" Checked='<%# Bind("Obsolete") %>'
                            Enabled="false" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:CheckBox ID="cbFooterGroupObsolete" runat="server" Checked='<%# Bind("Obsolete") %>' />
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <EditItemTemplate>
                        <asp:ImageButton ID="ibtnGroupUpdate" runat="server" CausesValidation="True" CommandName="Update"
                            ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditGroup" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ibtnGroupCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                            ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="ibtnGroupEdit" runat="server" CausesValidation="False" CommandName="Edit"
                            ImageUrl="~/images/edit.jpg" AlternateText="Edit" />&nbsp;&nbsp;&nbsp;
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:ImageButton ID="ibtnGroupInsert" runat="server" CausesValidation="True" CommandName="Insert"
                            ImageUrl="~/images/save.jpg" AlternateText="Insert" ValidationGroup="vgFooterGroup" />&nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ibtnGroupUndo" runat="server" CommandName="Undo" ImageUrl="~/images/undo-gray.jpg"
                            AlternateText="Undo" />
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
        <asp:ObjectDataSource ID="odsCostSheetGroup" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetCostSheetGroup" TypeName="CostSheetGroupBLL" InsertMethod="InsertCostSheetGroup"
            UpdateMethod="UpdateCostSheetGroup">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddSearchGroupName" Name="GroupID" PropertyName="SelectedValue"
                    Type="Int32" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="GroupName" Type="String" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
                <asp:Parameter Name="original_GroupID" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="GroupName" Type="String" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
            </InsertParameters>
        </asp:ObjectDataSource>
        <br />
        <h1>
            Team Members for selected Group</h1>
        <asp:ValidationSummary ID="vsFooterGroupTeamMember" runat="server" ShowMessageBox="True"
            ShowSummary="true" ValidationGroup="vgFooterGroupTeamMember" />
        <asp:ValidationSummary ID="vsEditGroupTeamMember" runat="server" ShowMessageBox="True"
            ShowSummary="true" ValidationGroup="vgEditGroupTeamMember" />
        <asp:GridView ID="gvGroupTeamMember" runat="server" AutoGenerateColumns="False" DataKeyNames="RowID"
            AllowSorting="True" AllowPaging="True" PageSize="15" ShowFooter="True" DataSourceID="odsCostSheetGroupTeamMember"
            EmptyDataText="No records found." Width="98%" Visible="false">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:BoundField DataField="RowID" HeaderText="RowID" ReadOnly="True" SortExpression="RowID" />
                <asp:TemplateField HeaderText="Group" SortExpression="ddGroupName">
                    <EditItemTemplate>
                        <asp:Label ID="lblEditGroupTeamMemberNameMarker" runat="server" Font-Bold="True"
                            ForeColor="Red" Text="*" />
                        <asp:DropDownList ID="ddEditGroupTeamMemberName" runat="server" DataSource='<%# CostingModule.GetCostSheetGroup(0) %>'
                            DataValueField="GroupID" DataTextField="ddGroupName" AppendDataBoundItems="True"
                            SelectedValue='<%# Bind("GroupID") %>'>
                            <asp:ListItem Text="" Value="0" Selected="False"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvEditGroupTeamMemberName" runat="server" ControlToValidate="ddEditGroupTeamMemberName"
                            ErrorMessage="The group is required." Font-Bold="True" ValidationGroup="vgEditGroupTeamMember"
                            Text="<" SetFocusOnError="true" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewGroupTeamMemberName" runat="server" Text='<%# Bind("ddGroupName") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblFooterGroupTeamMemberNameMarker" runat="server" Font-Bold="True"
                            ForeColor="Red" Text="*" />
                        <asp:DropDownList ID="ddFooterGroupTeamMemberName" runat="server" DataSource='<%# CostingModule.GetCostSheetGroup(0) %>'
                            DataValueField="GroupID" DataTextField="ddGroupName" AppendDataBoundItems="True"
                            SelectedValue='<%# Bind("GroupID") %>'>
                            <asp:ListItem Text="" Value="0" Selected="False"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvFooterGroupTeamMemberName" runat="server" ControlToValidate="ddFooterGroupTeamMemberName"
                            ErrorMessage="The group is required." Font-Bold="True" ValidationGroup="vgFooterGroupTeamMember"
                            Text="<" SetFocusOnError="true" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Team Member" SortExpression="TeamMemberName">
                    <EditItemTemplate>
                        <asp:Label ID="lblEditTeamMemberNameMarker" runat="server" Font-Bold="True" ForeColor="Red"
                            Text="*" />
                        <asp:DropDownList ID="ddEditTeamMemberName" runat="server" DataSource='<%# CostingModule.GetCostSheetApproverBySubscription(0,0) %>'
                            DataValueField="TeamMemberID" DataTextField="ddTeamMemberName" AppendDataBoundItems="True"
                            SelectedValue='<%# Bind("TeamMemberID") %>'>
                            <asp:ListItem Text="" Value="0" Selected="False"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvEditTeamMemberName" runat="server" ControlToValidate="ddEditTeamMemberName"
                            ErrorMessage="The team member is required." Font-Bold="True" ValidationGroup="vgEditGroupTeamMember"
                            Text="<" SetFocusOnError="true" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewTeamMemberName" runat="server" Text='<%# Bind("TeamMemberName") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblFooterTeamMemberNameMarker" runat="server" Font-Bold="True" ForeColor="Red"
                            Text="*" />
                        <asp:DropDownList ID="ddFooterTeamMemberName" runat="server" DataSource='<%# CostingModule.GetCostSheetApproverBySubscription(0,0) %>'
                            DataValueField="TeamMemberID" DataTextField="ddTeamMemberName" AppendDataBoundItems="True"
                            AutoPostBack="true" OnSelectedIndexChanged="ddFooterTeamMemberName_SelectedIndexChanged"
                            SelectedValue='<%# Bind("TeamMemberID") %>'>
                            <asp:ListItem Text="" Value="0" Selected="False"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvFooterTeamMemberName" runat="server" ControlToValidate="ddFooterTeamMemberName"
                            ErrorMessage="The team member is required." Font-Bold="True" ValidationGroup="vgFooterGroupTeamMember"
                            Text="<" SetFocusOnError="true" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Subscription" SortExpression="Subscription">
                    <EditItemTemplate>
                        <asp:Label ID="lblEditSubscription" runat="server" Text='<%# Bind("Subscription") %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewSubscription" runat="server" Text='<%# Bind("Subscription") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddFooterSubscription" runat="server" AppendDataBoundItems="true"
                            DataSource='<%# CostingModule.GetCostSheetSubscriptionByApprover(0,0) %>' DataValueField="SubscriptionID"
                            DataTextField="Subscription">
                            <asp:ListItem Text="" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:ImageButton ID="ibtnGroupTeamMemberDelete" runat="server" CausesValidation="False"
                            CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:ImageButton ID="ibtnGroupTeamMemberInsert" runat="server" CausesValidation="True"
                            CommandName="Insert" ImageUrl="~/images/save.jpg" AlternateText="Insert" ValidationGroup="vgFooterGroupTeamMember" />&nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ibtnGroupTeamMemberUndo" runat="server" CommandName="Undo" ImageUrl="~/images/undo-gray.jpg"
                            AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsCostSheetGroupTeamMember" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetCostSheetGroupTeamMember" TypeName="CostSheetGroupTeamMemberBLL"
            DeleteMethod="DeleteCostSheetGroupTeamMember" InsertMethod="InsertCostSheetGroupTeamMember"
            UpdateMethod="UpdateCostSheetGroupTeamMember">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddSearchGroupName" Name="GroupID" PropertyName="SelectedValue"
                    Type="Int32" />
                <asp:ControlParameter ControlID="ddSearchTeamMember" Name="TeamMemberID" PropertyName="SelectedValue"
                    Type="Int32" />
                <asp:Parameter DefaultValue="0" Name="SubscriptionID" Type="Int32" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="RowID" Type="Int32" />
                <asp:Parameter Name="original_RowID" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="GroupID" Type="Int32" />
                <asp:Parameter Name="TeamMemberID" Type="Int32" />
                <asp:Parameter Name="SubscriptionID" Type="Int32" />
                <asp:Parameter Name="original_RowID" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="GroupID" Type="Int32" />
                <asp:Parameter Name="TeamMemberID" Type="Int32" />
                <asp:Parameter Name="SubscriptionID" Type="Int32" />
            </InsertParameters>
        </asp:ObjectDataSource>
        <asp:Label ID="lblMessageBottom" SkinID="MessageLabelSkin" runat="server"></asp:Label>
    </asp:Panel>
</asp:Content>
