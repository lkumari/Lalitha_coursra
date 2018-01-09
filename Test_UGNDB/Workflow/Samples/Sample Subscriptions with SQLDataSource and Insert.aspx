<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Sample Subscriptions with SQLDataSource and Insert.aspx.vb" Inherits="Workflow_Subscriptions"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:GridView ID="gvSubscriptions" runat="server" AutoGenerateColumns="False" CellPadding="4"
        DataSourceID="dsSubscription" ForeColor="#333333" ShowFooter="True" GridLines="None"
        AllowPaging="True" AllowSorting="True" Width="400px" DataKeyNames="SubscriptionID"
        OnRowCommand="gvSubscriptions_RowCommand">
        <FooterStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <EmptyDataTemplate>
            No records found in the database.
        </EmptyDataTemplate>
        <Columns>
            <asp:BoundField DataField="SubscriptionID" HeaderText="SubscriptionID" InsertVisible="False"
                ReadOnly="True" SortExpression="SubscriptionID" Visible="False" />
            <asp:TemplateField HeaderText="Subscription" SortExpression="Subscription">
                <EditItemTemplate>
                    <asp:TextBox ID="txtSubscription" runat="server" Text='<%# Bind("Subscription") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblSubscription" runat="server" Text='<%# Bind("Subscription") %>'></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" />
                <FooterTemplate>
                    <asp:TextBox ID="Subscription" runat="server" Text=""></asp:TextBox>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Obsolete" SortExpression="Obsolete">
                <EditItemTemplate>
                    <asp:CheckBox ID="cbObsolete" runat="server" Checked='<%# Bind("Obsolete") %>' />
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="cbObsolete" runat="server" Checked='<%# Bind("Obsolete") %>' Enabled="false" />
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <EditItemTemplate>
                    <asp:ImageButton ID="imgUpdate" runat="server" CausesValidation="True" CommandName="Update"
                        ImageUrl="~/images/save.jpg" Text="Update" />&nbsp;&nbsp;&nbsp;<asp:ImageButton ID="imgCancel"
                            runat="server" CausesValidation="False" CommandName="Cancel" ImageUrl="~/images/cancel.jpg"
                            Text="Cancel" />
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:ImageButton ID="imgEdit" runat="server" CausesValidation="False" CommandName="Edit"
                        ImageUrl="~/images/edit.jpg" Text="Edit" />
                </ItemTemplate>
                <FooterTemplate>
                    <asp:ImageButton Text="Insert" CommandName="Insert" CausesValidation="False" runat="server"
                        ID="imgInsert" ImageUrl="~/images/save.jpg" />
                </FooterTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#CCCCCC" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    </asp:GridView>
    <asp:SqlDataSource ID="dsSubscription" runat="server" ConnectionString="<%$ ConnectionStrings:SQLConnection %>"
        InsertCommand="Exec sp_Insert_Subscriptions_Maint @Subscription, @CreatedBy"
        SelectCommand="Exec sp_Get_Subscriptions_Maint" UpdateCommand="Exec sp_Update_Subscriptions_Maint @Subscription, @Obsolete, @UpdatedBy, @SubscriptionID ">
        <UpdateParameters>
            <asp:Parameter Name="SubscriptionID" Type="Int32" />
            <asp:Parameter Name="Subscription" Type="String" />
            <asp:Parameter Name="Obsolete" Type="Boolean" />
            <asp:Parameter Name="UpdatedBy" Type="String" DefaultValue="lrey" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="Subscription" Type="String" />
            <asp:Parameter Name="CreatedBy" Type="String" DefaultValue="lrey" />
        </InsertParameters>
    </asp:SqlDataSource>
</asp:Content>
