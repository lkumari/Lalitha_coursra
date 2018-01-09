<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Subscriptions OLD .aspx.vb" Inherits="Workflow_Subscriptions" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:GridView ID="gvSubscriptions" runat="server" AutoGenerateColumns="False" CellPadding="4"
        DataKeyNames="SubscriptionID" DataSourceID="dsSubscriptions" ForeColor="#333333"
        ShowFooter="True" GridLines="None" AllowPaging="True" AllowSorting="True" Width="400px"
        OnRowCommand="gvSubscriptions_RowCommand">
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
    <asp:SqlDataSource ID="dsSubscriptions" runat="server" ConnectionString="<%$ ConnectionStrings:SQLConnection %>"
        InsertCommand="INSERT INTO Subscriptions_Maint(Subscription,CreatedOn) VALUES(@Subscription,GetDate())"
        SelectCommand="SELECT SubscriptionID, Subscription, Obsolete FROM Subscriptions_Maint"
        UpdateCommand="UPDATE Subscriptions_Maint SET Subscription = @Subscription, Obsolete = @Obsolete, UpdatedBy = @UpdatedBy, UpdatedOn = GetDate() WHERE SubscriptionID = @SubscriptionID"
        >
        <UpdateParameters>
            <asp:Parameter Name="Subscription" />
            <asp:Parameter Name="Obsolete" />
            <asp:Parameter Name="UpdatedBy" />
            <asp:Parameter Name="SubscriptionID" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Type="String" Name="Subscription" />
        </InsertParameters>
    </asp:SqlDataSource>
</asp:Content>
