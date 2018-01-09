<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    MaintainScrollPositionOnPostback="true" CodeFile="Cost_Sheet_Post_Approval_List.aspx.vb"
    Inherits="Cost_Sheet_Post_Approval_List" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server">
        <asp:Label ID="lblMessage" SkinID="MessageLabelSkin" runat="server"></asp:Label>
        <br />
        <table width="28%">
            <tr>
                <td class="c_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblCostSheetLabel" Width="200px" Text="Cost Sheet ID:"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblCostSheetValue" Width="200px"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <table width="98%" border="0">
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblCommentsLabel" Text="Notification Comments (to include in email):"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtCommentsValue" runat="server" Text="" Width="98%" Height="100px"
                        TextMode="MultiLine"></asp:TextBox>
                    <asp:Label ID="lblCommentsCharCount" SkinID="MessageLabelSkin" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button runat="server" ID="btnSave" Text="Save" Width="200px" />
                </td>
            </tr>
        </table>
        <br />
        <table width="98%" border="0">
            <tr>
                <td style="white-space: nowrap;">
                    Choose another cost sheet to copy the list(s) from:
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtChooseAnotherCostSheetID"></asp:TextBox>
                    <asp:ImageButton ID="iBtnGetAnotherCostSheetPostApprovalList" runat="server" ImageUrl="~/images/SelectUser.gif"
                        ToolTip="Click here to pull the notification lists from another cost sheet." />
                </td>
            </tr>
        </table>
        <asp:ValidationSummary ID="vsEditPostApproval" runat="server" DisplayMode="List"
            ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgEditPostApproval" />
        <asp:ValidationSummary ID="vsFooterPostApproval" runat="server" DisplayMode="List"
            ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgFooterPostApproval" />
        <asp:GridView runat="server" ID="gvPostApproval" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" PageSize="15" ShowFooter="True" DataSourceID="odsPostApproval"
            DataKeyNames="RowID" Width="100%" EmptyDataText="No records found." Visible="False">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
            <Columns>
                <asp:BoundField DataField="RowID" HeaderText="RowID" ReadOnly="True">
                    <HeaderStyle CssClass="none" />
                    <ItemStyle CssClass="none" />
                    <FooterStyle CssClass="none" />
                </asp:BoundField>
                <asp:BoundField DataField="CostSheetID" HeaderText="CostSheetID" SortExpression="CostSheetID"
                    ReadOnly="True">
                    <HeaderStyle CssClass="none" />
                    <ItemStyle CssClass="none" />
                    <FooterStyle CssClass="none" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Team Member" SortExpression="TeamMemberName">
                    <EditItemTemplate>
                        <asp:Label ID="lblEditPostApprovalTeamMember" runat="server" Text='<%# Bind("TeamMemberName") %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewPostApprovalTeamMember" runat="server" Text='<%# Bind("TeamMemberName") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblFooterPostApprovalTeamMemberMarker" runat="server" Font-Bold="True"
                            ForeColor="Red" Text="*" />
                        <asp:DropDownList ID="ddFooterPostApprovalTeamMember" runat="server" DataSource='<%# CostingModule.GetCostSheetPostApprovalTeamMembers() %>'
                            DataValueField="TeamMemberID" DataTextField="ddTeamMemberName" AppendDataBoundItems="True"
                            SelectedValue='<%# Bind("TeamMemberID") %>'>
                            <asp:ListItem Text="" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="NotificationDate" HeaderText="Notification Date" SortExpression="NotificationDate"
                    ReadOnly="True" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="iBtnPostApprovalDelete" runat="server" CausesValidation="False"
                            CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgFooterFormula"
                            runat="server" ID="iBtnFooterPostApproval" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                        <asp:ImageButton ID="iBtnPostApprovalUndo" runat="server" CommandName="Undo" CausesValidation="false"
                            ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsPostApproval" runat="server" OldValuesParameterFormatString="original_{0}"
            DeleteMethod="DeleteCostSheetPostApprovalItem" InsertMethod="InsertCostSheetPostApprovalItem"
            SelectMethod="GetCostSheetPostApprovalList" TypeName="CostSheetPostApprovalBLL">
            <DeleteParameters>
                <asp:Parameter Name="RowID" Type="Int32" />
                <asp:Parameter Name="original_RowID" Type="Int32" />
            </DeleteParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="CostSheetID" QueryStringField="CostSheetID" Type="Int32" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="CostSheetID" Type="Int32" />
                <asp:Parameter Name="TeamMemberID" Type="Int32" />
            </InsertParameters>
        </asp:ObjectDataSource>
        <br />
        <br />
        <asp:Button runat="server" ID="btnNotify" Text="Notify" Width="200px" />
        <asp:CheckBox runat="server" ID="cbNotifyOnlyNew" Text="Check to only notify new people added to the list"
            Checked="true" />
        <br />
        <asp:HyperLink runat="server" ID="hlnkRFD" Visible="false" Font-Underline="true"
            ToolTip="Click here to view the RFD" Text="View RFD" Target="_blank"></asp:HyperLink>
    </asp:Panel>
</asp:Content>
