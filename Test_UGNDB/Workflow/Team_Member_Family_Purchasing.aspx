<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Team_Member_Family_Purchasing.aspx.vb" Inherits="Workflow_Team_Member_Family_Purchasing"
    Title="Team Member Family Purchasing Assignments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
            Visible="False" />
        <br />
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
                    <asp:Label ID="lblFamily" runat="server" Text="Family:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddFamily" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    &nbsp;
                </td>
                <td colspan="3">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" />
                </td>
            </tr>
        </table>
        <hr />
        <em class="p_smalltextbold">Use the parameters above to filter the list below.</em>&nbsp;<br />
        <asp:GridView ID="gvWorkFlow" runat="server" AutoGenerateColumns="False" DataSourceID="odsWorkFlow"
            DataKeyNames="TeamMemberID,FamilyID" SkinID="StandardGrid" AllowSorting="True"
            AllowPaging="True" OnRowCommand="gvWorkFlow_RowCommand" OnRowDataBound="gvWorkFlow_RowDataBound"
            PageSize="30" Width="600px">
            <Columns>
                <asp:BoundField DataField="RowID" HeaderText="RowID" SortExpression="RowID" Visible="False" />
                <asp:TemplateField HeaderText="Team Member" SortExpression="TeamMemberName" HeaderStyle-HorizontalAlign="left">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddTeamMember" runat="server" DataSource='<%# commonFunctions.GetTeamMember("") %>'
                            DataTextField="TeamMemberName" DataValueField="TeamMemberID" SelectedValue='<%# Bind("TeamMemberID") %>'>
                            <asp:ListItem Selected="True">
                            </asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvTeamMember" runat="server" ControlToValidate="ddTeamMember"
                            ErrorMessage="Team Member is a required field." ValidationGroup="EditWorkFlow"><</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddTeamMember" runat="server" AppendDataBoundItems="true" DataSource='<%# commonFunctions.GetTeamMember("") %>'
                            DataTextField="TeamMemberName" DataValueField="TeamMemberID" SelectedValue='<%# Bind("TeamMemberID") %>'>
                            <asp:ListItem Selected="True">
                            </asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvTeamMember" runat="server" ControlToValidate="ddTeamMember"
                            ErrorMessage="Team Member is a required field." ValidationGroup="InsertWorkFlow"><</asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("TeamMemberName") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Family Name" SortExpression="ddFamilyName" HeaderStyle-HorizontalAlign="left">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddFamily" runat="server" DataSource='<%# commonFunctions.GetFamily() %>'
                            DataTextField="ddFamilyName" DataValueField="FamilyID" SelectedValue='<%# Bind("FamilyID") %>'>
                            <asp:ListItem Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvFamilyID" runat="server" ControlToValidate="ddFamily"
                            ErrorMessage="Family is a required field." ValidationGroup="EditWorkFlow"><</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddFamily" runat="server" AppendDataBoundItems="true" DataSource='<%# commonFunctions.GetFamily() %>'
                            DataTextField="ddFamilyName" DataValueField="FamilyID" SelectedValue='<%# Bind("FamilyID") %>'>
                            <asp:ListItem Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvFamilyID" runat="server" ControlToValidate="ddFamily"
                            ErrorMessage="Family is a required field." ValidationGroup="InsertWorkFlow"><</asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("ddFamilyName") %>'></asp:Label>
                    </ItemTemplate>
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
        <br />
        <asp:ValidationSummary ID="vsEditWorkFlow" runat="server" ShowMessageBox="True" ValidationGroup="EditWorkFlow" />
        <asp:ValidationSummary ID="vsInsertWorkFlow" runat="server" ShowMessageBox="True"
            ValidationGroup="InsertWorkFlow" />
        <asp:ObjectDataSource ID="odsWorkFlow" runat="server" SelectMethod="GetWorkFlowFamily_PurchasingAssignments"
            TypeName="WorkFlow_Family_Purchasing_AssignmentsBLL" OldValuesParameterFormatString="original_{0}"
            DeleteMethod="DeleteWorkFlowFamilyPurchasingAssignments" InsertMethod="InsertWorkFlowFamilyPurchasingAssignments"
            UpdateMethod="UpdateWorkFlowFamilyPurchasingAssignments">
            <SelectParameters>
                <asp:QueryStringParameter Name="FamilyID" QueryStringField="sFam" Type="Int32" 
                    DefaultValue="0" />
                <asp:QueryStringParameter Name="TeamMemberID" QueryStringField="sTM" 
                    Type="Int32" DefaultValue="0" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="TeamMemberID" Type="Int32" />
                <asp:Parameter Name="FamilyID" Type="String" />
                <asp:Parameter Name="SoldTo" Type="Int32" />
                <asp:Parameter Name="Original_TeamMemberID" Type="Int32" />
                <asp:Parameter Name="Original_FamilyID" Type="String" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="TeamMemberID" Type="Int32" />
                <asp:Parameter Name="FamilyID" Type="String" />
                <asp:Parameter Name="Original_TeamMemberID" Type="Int32" />
                <asp:Parameter Name="Original_FamilyID" Type="String" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="TeamMemberID" Type="Int32" />
                <asp:Parameter Name="FamilyID" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
