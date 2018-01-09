<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Sample Subscriptions using ObjectDataSource.aspx.vb" Inherits="Workflow_Sample_Subscriptions_with_ObjectDataSource"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:GridView ID="gvSubscriptions" runat="server" AutoGenerateColumns="False" DataKeyNames="SubscriptionID"
        ShowFooter="True" DataSourceID="odsSubscriptions" AllowPaging="True" Width="336px"
        OnRowCommand="gvSubscriptions_RowCommand" OnRowDataBound="gvSubscriptions_RowDataBound">
        <FooterStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <EmptyDataTemplate>
            No records found from the database.
        </EmptyDataTemplate>
        <Columns>
            <asp:BoundField DataField="SubscriptionID" HeaderText="SubscriptionID" InsertVisible="False"
                ReadOnly="True" SortExpression="SubscriptionID" Visible="False" />
            <asp:TemplateField HeaderText="Subscription" SortExpression="Subscription">
                <EditItemTemplate>
                    <asp:TextBox ID="Subscription" runat="server" Text='<%# Bind("Subscription") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Subscription") %>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" />
                <FooterTemplate>
                    <asp:TextBox ID="newSubscription" runat="server" Text=""></asp:TextBox>&nbsp;
                        </FooterTemplate>
            </asp:TemplateField>
            <asp:CheckBoxField DataField="Obsolete" HeaderText="Obsolete" SortExpression="Obsolete">
                <ItemStyle HorizontalAlign="Center" />
            </asp:CheckBoxField>
            <asp:TemplateField ShowHeader="False">
                <EditItemTemplate>
                    <asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True" CommandName="Update"
                        ImageUrl="~/images/save.jpg" Text="Update" AlternateText="Update" />&nbsp;&nbsp;&nbsp;<asp:ImageButton
                            ID="ibtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                            ImageUrl="~/images/cancel.jpg" Text="Cancel" AlternateText="Cancel" />
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False" CommandName="Edit"
                        ImageUrl="~/images/edit.jpg" Text="Edit" AlternateText="Edit" />&nbsp;&nbsp;&nbsp;
                    <asp:ImageButton ID="ibtnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                        ImageUrl="~/images/delete.jpg" Text="Delete" AlternateText="Delete" OnClientClick="return confirm('Are you certain you want to delete this 
Subscription?');" />
                </ItemTemplate>
                <FooterTemplate>
                    <asp:ImageButton Text="Insert" CommandName="Insert" CausesValidation="true" runat="server"
                        ID="ibtnInsert" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                </FooterTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#CCCCCC" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    </asp:GridView>
    &nbsp;
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
        ShowSummary="False" />
    &nbsp;
    <asp:ObjectDataSource ID="odsSubscriptions" runat="server" DeleteMethod="DeleteSubscription"
        InsertMethod="AddSubscription" SelectMethod="GetSubscriptions" TypeName="SubscriptionsBLL"
        UpdateMethod="UpdateSubscription" OldValuesParameterFormatString="original_{0}">
        <DeleteParameters>
            <asp:Parameter Name="SubscriptionID" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="SubscriptionID" Type="Int32" />
            <asp:Parameter Name="subscription" Type="String" />
            <asp:Parameter Name="Obsolete" Type="Boolean" />
            <asp:Parameter Name="UpdatedBy" Type="String" DefaultValue="lrey" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="subscription" Type="String" />
            <asp:Parameter Name="createdBy" Type="String" DefaultValue="lrey" />
        </InsertParameters>
        <SelectParameters>
            <asp:Parameter Name="subscription" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
