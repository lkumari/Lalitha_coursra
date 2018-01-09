<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Team_Member_Commodity_Accounts.aspx.vb" Inherits="Workflow_Team_Member_Commodity_Accounts"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
        Visible="False" /><br />
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <hr />
        <table>
            <tr>
                <td class="p_text">
                    <asp:Label ID="txtTeamMember" runat="server" Text="Team Member:" />
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
        <asp:GridView ID="gvWorkFlow" runat="server" AutoGenerateColumns="False" DataSourceID="odsWorkFlow"
            DataKeyNames="TeamMemberID,CommodityID" SkinID="StandardGrid" AllowSorting="True"
            AllowPaging="True" OnRowCommand="gvWorkFlow_RowCommand" OnRowDataBound="gvWorkFlow_RowDataBound"
            PageSize="30" Width="800px">
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
                <asp:TemplateField HeaderText="Commodity / Classification" SortExpression="CommodityID">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddCommodity" runat="server" DataSource='<%# commonFunctions.GetCommodity(0,"","",0) %>'
                            DataValueField="CommodityID" DataTextField="ddCommodityByClassification" SelectedValue='<%# Bind("CommodityID") %>'>
                            <asp:ListItem Selected="True">
                            </asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvCommodityID" runat="server" ControlToValidate="ddCommodity"
                            ErrorMessage="Commodity is a required field." ValidationGroup="EditWorkFlow"><</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("CommodityName") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddCommodity" runat="server" DataSource='<%# commonFunctions.GetCommodity(0,"","",0) %>'
                            DataValueField="CommodityID" DataTextField="ddCommodityByClassification" SelectedValue='<%# Bind("CommodityID") %>'
                            AppendDataBoundItems="true">
                            <asp:ListItem Selected="True">
                            </asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvCommodityID" runat="server" ControlToValidate="ddCommodity"
                            ErrorMessage="Commodity is a required field." ValidationGroup="InsertWorkFlow"><</asp:RequiredFieldValidator>
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
                            ImageUrl="~/images/save.jpg"  AlternateText="Insert" ValidationGroup="InsertWorkFlow" />&nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo" ImageUrl="~/images/undo-gray.jpg"
                            AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ValidationSummary ID="vsEditWorkFlow" runat="server" ShowMessageBox="True" ValidationGroup="EditWorkFlow" />
        <asp:ValidationSummary ID="vsInsertWorkFlow" runat="server" ShowMessageBox="True"
            ValidationGroup="InsertWorkFlow" />
        <asp:ValidationSummary ID="vsEmptyWorkFlow" runat="server" ShowMessageBox="True"
            ValidationGroup="EmptyWorkFlow" />
        <asp:ObjectDataSource ID="odsWorkFlow" runat="server" DeleteMethod="DeleteWorkFlowCommodityAssignments"
            InsertMethod="AddWorkFlowCommodityAssignments" SelectMethod="GetWorkFlowCommodityAssignments"
            TypeName="WorkFlow_Commodity_AssignmentsBLL" UpdateMethod="UpdateWorkFlowCommodityAssignments"
            OldValuesParameterFormatString="original_{0}">
            <DeleteParameters>
                <asp:Parameter Name="TeamMemberID" Type="Int32" />
                <asp:Parameter Name="CommodityID" Type="String" />
                <asp:Parameter Name="SoldTo" Type="Int32" />
                <asp:Parameter Name="original_TeamMemberID" Type="Int32" />
                <asp:Parameter Name="original_CommodityID" Type="String" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="TeamMemberID" Type="Int32" />
                <asp:Parameter Name="CommodityID" Type="String" />
                <asp:Parameter Name="original_TeamMemberID" Type="Int32" />
                <asp:Parameter Name="original_CommodityID" Type="String" />
            </UpdateParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="TeamMemberID" QueryStringField="sTeamMember" Type="Int32"
                    DefaultValue="0" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="TeamMemberID" Type="Int32" />
                <asp:Parameter Name="CommodityID" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
