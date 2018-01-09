<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Subscriptions.aspx.vb" Inherits="Workflow_Sample_Subscriptions_with_ObjectDataSource"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
        Width="568px" Visible="False" />
    <br />
    <asp:Panel ID="localPanel" runat="server" Width="900px">
        <hr />
        <table>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblSubscription" runat="server" Text="Subscription:" />
                </td>
                <td>
                    <asp:TextBox ID="txtSuscriptionDescr" runat="server" />
                </td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" />
                </td>
            </tr>
        </table>
        <hr />
        <asp:GridView ID="gvSubscriptions" runat="server" AutoGenerateColumns="False" DataKeyNames="SubscriptionID"
            SkinID="StandardGrid" DataSourceID="odsSubscriptions" AllowSorting="True" AllowPaging="True"
            Width="500px" OnRowCommand="gvSubscriptions_RowCommand" OnDataBound="gvSubscriptions_DataBound"
            PageSize="30">
            <Columns>
                <asp:BoundField DataField="SubscriptionID" HeaderText="Sub ID" InsertVisible="False"
                    ReadOnly="True" SortExpression="SubscriptionID">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Subscription" SortExpression="Subscription">
                    <EditItemTemplate>
                        <asp:TextBox ID="Subscription" runat="server" Text='<%# Bind("Subscription") %>'
                            MaxLength="25" Width="200px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvSubscription" runat="server" ControlToValidate="Subscription"
                            Display="Dynamic" ErrorMessage="Subscription is a required field." Font-Bold="True"
                            Font-Size="Small" ValidationGroup="EditSubscriptionInfo"><</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("Subscription") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="newSubscription" runat="server" Text="" MaxLength="25" Width="200px"></asp:TextBox>&nbsp;
                        <asp:RequiredFieldValidator ID="rfvSubscription" runat="server" ControlToValidate="newSubscription"
                            Display="Dynamic" ErrorMessage="Subscription is a required field." Font-Bold="True"
                            Font-Size="Small" ValidationGroup="InsertSubscriptionInfo"><</asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:CheckBoxField DataField="Obsolete" HeaderText="Obsolete" SortExpression="Obsolete">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:CheckBoxField>
                <asp:BoundField DataField="UpdateInfo" HeaderText="Updated By" ReadOnly="True" SortExpression="UpdateInfo">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:TemplateField ShowHeader="False">
                    <ItemStyle HorizontalAlign="Left" />
                    <EditItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True"
                            CommandName="Update" ImageUrl="~/images/save.jpg" Text="Update" AlternateText="Update"
                            ValidationGroup="EditSubscriptionInfo" />&nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ibtnCancel"
                                runat="server" CausesValidation="False" CommandName="Cancel" ImageUrl="~/images/cancel.jpg"
                                Text="Cancel" AlternateText="Cancel" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False"
                            CommandName="Edit" ImageUrl="~/images/edit.jpg" Text="Edit" AlternateText="Edit" />
                    </ItemTemplate>
                    <FooterTemplate>
                        &nbsp;&nbsp;<asp:ImageButton Text="Insert" CommandName="Insert" CausesValidation="true"
                            runat="server" ID="ibtnInsert" ImageUrl="~/images/save.jpg" AlternateText="Insert"
                            ValidationGroup="InsertSubscriptionInfo" />
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo" ImageUrl="~/images/undo-gray.jpg"
                            Text="Undo" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ValidationSummary ID="EditSubscriptionInfo" runat="server" ShowMessageBox="True"
            ValidationGroup="EditSubscriptionInfo" />
        &nbsp;<asp:ValidationSummary ID="InsertSubscriptionInfo" runat="server" ShowMessageBox="True"
            ValidationGroup="InsertSubscriptionInfo" />
        <asp:ObjectDataSource ID="odsSubscriptions" runat="server" DeleteMethod="DeleteSubscription"
            InsertMethod="AddSubscription" SelectMethod="GetSubscriptions" TypeName="SubscriptionsBLL"
            UpdateMethod="UpdateSubscription" OldValuesParameterFormatString="original_{0}">
            <DeleteParameters>
                <asp:Parameter Name="SubscriptionID" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="subscription" Type="String" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
                <asp:Parameter Name="UpdatedBy" Type="String" DefaultValue="lrey" />
                <asp:Parameter Name="original_SubscriptionID" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="subscription" Type="String" />
                <asp:Parameter Name="createdBy" Type="String" DefaultValue="lrey" />
            </InsertParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="subscription" QueryStringField="Subscription" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
