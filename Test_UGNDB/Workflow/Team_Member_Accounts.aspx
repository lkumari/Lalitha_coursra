<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Team_Member_Accounts.aspx.vb" Inherits="Workflow_Team_Member_Accounts"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
        Width="568px" Visible="False"></asp:Label><br />
    <asp:Panel ID="localPanel" runat="server">
        <hr />
        <table>
            <tr>
                <td class="p_text">
                   <asp:Label ID="lblTeamMember" runat="server" Text=" Team Member:"/>
                </td>
                <td>
                    <asp:DropDownList ID="ddTeamMember" runat="server"/>
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
        <asp:GridView ID="gvWorkFlow" runat="server" AutoGenerateColumns="False" DataSourceID="odsWorkFlow"
            DataKeyNames="TeamMemberID,CABBV,SoldTo" SkinID="StandardGrid" AllowSorting="True"
            AllowPaging="True" OnRowCommand="gvWorkFlow_RowCommand" OnRowDataBound="gvWorkFlow_RowDataBound"
            PageSize="30" Width="60%">
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
                <asp:TemplateField HeaderText="Customer" SortExpression="CABBV">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddCABBV" runat="server" DataSource='<%# commonFunctions.GetCustomer("true") %>'
                            DataValueField="ddCustomerValue" DataTextField="ddCustomerDesc" SelectedValue='<%# Bind("ddCustomerValue") %>'
                            Width="270px">
                            <asp:ListItem Selected="True">
                            </asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvCABBV" runat="server" ControlToValidate="ddCABBV"
                            ErrorMessage="Customer is a required field." ValidationGroup="EditWorkFlow"><</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("ddCustomerDesc") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddCABBV" runat="server" DataSource='<%# commonFunctions.GetCustomer("true") %>'
                            DataValueField="ddCustomerValue" DataTextField="ddCustomerDesc" SelectedValue='<%# Bind("ddCustomerValue") %>'
                            Width="270px" AppendDataBoundItems="true">
                            <asp:ListItem Selected="True">
                            </asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvCABBV" runat="server" ControlToValidate="ddCABBV"
                            ErrorMessage="Customer is a required field." ValidationGroup="InsertWorkFlow"><</asp:RequiredFieldValidator>
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
                            ImageUrl="~/images/save.jpg" Text="Update" AlternateText="Update" ValidationGroup="EditWorkFlow" />&nbsp;&nbsp;&nbsp;<asp:ImageButton
                                ID="ibtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                ImageUrl="~/images/cancel.jpg" Text="Cancel" AlternateText="Cancel" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False" CommandName="Edit"
                            ImageUrl="~/images/edit.jpg" Text="Edit" AlternateText="Edit" />&nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ibtnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                            ImageUrl="~/images/delete.jpg" Text="Delete" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');"
                            Visible='<%# ViewState("ObjectRole")%>' />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:ImageButton ID="ibtnInsert" runat="server" CausesValidation="True" CommandName="Insert"
                            ImageUrl="~/images/save.jpg" Text="Update" AlternateText="Insert" ValidationGroup="InsertWorkFlow" />&nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo" ImageUrl="~/images/undo-gray.jpg"
                            Text="Undo" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ValidationSummary ID="vsEditWorkFlow" runat="server" ShowMessageBox="True" ValidationGroup="EditWorkFlow" />
        <asp:ValidationSummary ID="vsInsertWorkFlow" runat="server" ShowMessageBox="True"
            ValidationGroup="InsertWorkFlow" />
        <asp:ValidationSummary ID="vsEmptyWorkFlow" runat="server" ShowMessageBox="True"
            ValidationGroup="EmptyWorkFlow" />
        <asp:ObjectDataSource ID="odsWorkFlow" runat="server" DeleteMethod="DeleteWorkFlowAssignments"
            InsertMethod="AddWorkFlowAssignments" SelectMethod="GetWorkFlowAssignments" TypeName="WorkFlow_AssignmentsBLL"
            UpdateMethod="UpdateWorkFlowAssignments" OldValuesParameterFormatString="original_{0}">
            <DeleteParameters>
                <asp:Parameter Name="TeamMemberID" Type="Int32" />
                <asp:Parameter Name="CABBV" Type="String" />
                <asp:Parameter Name="SoldTo" Type="Int32" />
                <asp:Parameter Name="original_TeamMemberID" Type="Int32" />
                <asp:Parameter Name="original_CABBV" Type="String" />
                <asp:Parameter Name="Original_SoldTo" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="original_TeamMemberID" Type="Int32" />
                <asp:Parameter Name="original_CABBV" Type="String" />
                <asp:Parameter Name="Original_SoldTo" Type="Int32" />
                <asp:Parameter Name="ddCustomerValue" Type="String" />
                <asp:Parameter Name="TeamMemberID" Type="Int32" />
            </UpdateParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="TeamMemberID" QueryStringField="sTeamMember" 
                    Type="Int32" DefaultValue="0" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="TeamMemberID" Type="Int32" />
                <asp:Parameter Name="CABBV" Type="String" />
                <asp:Parameter Name="SoldTo" Type="Int32" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
