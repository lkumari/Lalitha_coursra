<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    MaintainScrollPositionOnPostback="true" CodeFile="Team_Member_Make_Accounts.aspx.vb"
    Inherits="Workflow_Team_Member_Make_Accounts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin" /><br />
        <hr />
        <table>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblTeamMember" runat="server" Text="Team Member:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddTeamMember" runat="server" />
                </td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" />
                </td>
            </tr>
        </table>
        <hr />
        <em class="p_smalltextbold">Use the parameters above to filter the list below.</em>
        <br />
        <br />
        <asp:ValidationSummary ID="vsEditWorkFlow" runat="server" ShowMessageBox="True" ValidationGroup="EditWorkFlow" />
        <asp:ValidationSummary ID="vsInsertWorkFlow" runat="server" ShowMessageBox="True"
            ValidationGroup="InsertWorkFlow" />
        <asp:GridView ID="gvWorkFlow" runat="server" AutoGenerateColumns="False" DataSourceID="odsWorkFlow"
            DataKeyNames="RowID" SkinID="StandardGrid" AllowSorting="True" AllowPaging="True"
            PageSize="30" Width="600px">
            <Columns>
                <asp:TemplateField HeaderText="Team Member" SortExpression="TeamMemberName">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddEditTeamMember" runat="server" DataSource='<%# commonFunctions.GetTeamMember("") %>'
                            DataValueField="TeamMemberID" DataTextField="TeamMemberName" SelectedValue='<%# Bind("TeamMemberID") %>'>
                            <asp:ListItem Selected="True">
                            </asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvTeamMember" runat="server" ControlToValidate="ddEditTeamMember"
                            ErrorMessage="Team Member is a required field." ValidationGroup="EditWorkFlow"><</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewTeamMember" runat="server" Text='<%# Bind("TeamMemberName") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddInsertTeamMember" runat="server" DataSource='<%# commonFunctions.GetTeamMember("") %>'
                            DataValueField="TeamMemberID" DataTextField="TeamMemberName" SelectedValue='<%# Bind("TeamMemberID") %>'
                            AppendDataBoundItems="true">
                            <asp:ListItem></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvInsertTeamMember" runat="server" ControlToValidate="ddInsertTeamMember"
                            ErrorMessage="Team Member is a required field." ValidationGroup="InsertWorkFlow"><</asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Make" SortExpression="Make">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddEditMake" runat="server" DataSource='<%# commonFunctions.GetProgramMake() %>'
                            DataValueField="MAKE" DataTextField="MAKE" SelectedValue='<%# Bind("MAKE") %>'>
                            <asp:ListItem Selected="True">
                            </asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvEditMake" runat="server" ControlToValidate="ddEditMake"
                            ErrorMessage="Make is a required field." ValidationGroup="EditWorkFlow"><</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewMake" runat="server" Text='<%# Bind("Make") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddInsertMake" runat="server" DataSource='<%# commonFunctions.GetProgramMake() %>'
                            DataValueField="MAKE" DataTextField="MAKE" SelectedValue='<%# Bind("MAKE") %>'
                            AppendDataBoundItems="true">
                            <asp:ListItem></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvInsertMake" runat="server" ControlToValidate="ddInsertMake"
                            ErrorMessage="Make is a required field." ValidationGroup="InsertWorkFlow"><</asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:BoundField DataField="UpdateInfo" HeaderText="Last Update" SortExpression="UpdatedOn"
                    ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:TemplateField ShowHeader="False">
                    <EditItemTemplate>
                        <asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True" CommandName="Update"
                            ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="EditWorkFlow" />&nbsp;&nbsp;&nbsp;<asp:ImageButton
                                ID="ibtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False" CommandName="Edit"
                            ImageUrl="~/images/edit.jpg" AlternateText="Edit" />&nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ibtnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                            ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');"
                            Visible='<%# ViewState("ObjectRole")%>' />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:ImageButton ID="ibtnInsert" runat="server" CausesValidation="True" CommandName="Insert"
                            ImageUrl="~/images/save.jpg" AlternateText="Insert" ValidationGroup="InsertWorkFlow" />&nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo" ImageUrl="~/images/undo-gray.jpg"
                            AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsWorkFlow" runat="server" DeleteMethod="DeleteWorkFlowMakeAssignments"
            InsertMethod="InsertWorkFlowMakeAssignments" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetWorkFlowMakeAssignments" TypeName="WorkFlowMakeAssignmentsBLL"
            UpdateMethod="UpdateWorkFlowMakeAssignments">
            <DeleteParameters>
                <asp:Parameter Name="RowID" Type="Int32" />
                <asp:Parameter Name="Original_RowID" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="TeamMemberID" Type="Int32" />
                <asp:Parameter Name="Make" Type="String" />
                <asp:Parameter Name="Original_RowID" Type="Int32" />
            </UpdateParameters>
            <SelectParameters>
                <asp:Parameter Name="Make" Type="String" />
                <asp:QueryStringParameter Name="TeamMemberID" QueryStringField="TeamMemberID" 
                    Type="Int32" DefaultValue="0" />
                <asp:Parameter Name="SubscriptionID" Type="Int32" DefaultValue="0" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="TeamMemberID" Type="Int32" />
                <asp:Parameter Name="Make" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
