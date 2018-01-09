<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    MaintainScrollPositionOnPostback="true" CodeFile="Cost_Sheet_Pre_Approval_List.aspx.vb"
    Inherits="Cost_Sheet_Pre_Approval_List" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server">
        <asp:Label ID="lblMessage" SkinID="MessageLabelSkin" runat="server"></asp:Label>
        <asp:ValidationSummary ID="vsCostSheet" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgCosting" />
        <br />
        <table width="28%">
            <tr>
                <td class="p_textbold" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblCostSheetLabel" Text="Cost Sheet ID:" Visible="false"></asp:Label>
                </td>
                <td class="c_text">
                    <asp:Label runat="server" ID="lblCostSheetValue" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:Button runat="server" ID="btnEdit" Text="Edit" Visible="false" />
                </td>
            </tr>
        </table>
        <br />
        <table width="98%" border="0">
            <tr>
                <td class="p_textbold" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblChooseNotificationGroupLabel" Text="Choose a Notification Group:"
                        Visible="false"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:DropDownList ID="ddChooseNotificationGroupValue" runat="server" Visible="false"
                        Width="206px">
                    </asp:DropDownList>
                    <asp:ImageButton ID="iBtnGetPreApprovalListFromGroup" runat="server" ImageUrl="~/images/SelectUser.gif"
                        ToolTip="Click here to pull the notification lists from a group." Visible="false" />
                </td>
            </tr>
            <tr>
                <td class="p_textbold" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblChooseCostSheetLabel" Text="Choose another cost sheet to copy the list(s) from:"
                        Visible="false"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox runat="server" ID="txtChooseCostSheetValue" Visible="false" ValidationGroup="vgCosting"
                        Width="200px"></asp:TextBox>
                    <asp:ImageButton ID="iBtnGetAnotherCostSheetPreApprovalList" runat="server" ImageUrl="~/images/SelectUser.gif"
                        ToolTip="Click here to pull the notification lists from another cost sheet."
                        Visible="false" ValidationGroup="vgCosting" />
                    <asp:CompareValidator runat="server" ID="cvChooseCostSheetValue" Operator="DataTypeCheck"
                        ValidationGroup="vgCosting" Type="integer" Text="<" ControlToValidate="txtChooseCostSheetValue"
                        ErrorMessage="Cost Sheet ID must be an integer." SetFocusOnError="True" />
                </td>
            </tr>
        </table>
        <br />
        <h1>
            First Level Routing</h1>
        <asp:ValidationSummary ID="vsEditPreApprovalListFirstRoutingLevel" runat="server"
            DisplayMode="List" ShowMessageBox="true" ShowSummary="true" EnableClientScript="true"
            ValidationGroup="vgEditPreApprovalListFirstRoutingLevel" />
        <asp:ValidationSummary ID="vsFooterPreApprovalListFirstRoutingLevel" runat="server"
            DisplayMode="List" ShowMessageBox="true" ShowSummary="true" EnableClientScript="true"
            ValidationGroup="vgFooterPreApprovalListFirstRoutingLevel" />
        <asp:GridView runat="server" ID="gvPreApprovalListFirstRoutingLevel" AllowPaging="True"
            AllowSorting="True" AutoGenerateColumns="False" PageSize="15" ShowFooter="True"
            DataSourceID="odsPreApprovalListFirstRoutingLevel" DataKeyNames="RowID" Width="100%"
            EmptyDataText="No records found." Visible="False">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
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
                <asp:BoundField DataField="RoutingLevel" HeaderText="RoutingLevel" SortExpression="RoutingLevel"
                    ReadOnly="True">
                    <HeaderStyle CssClass="none" />
                    <ItemStyle CssClass="none" />
                    <FooterStyle CssClass="none" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Team Member" SortExpression="TeamMemberName">
                    <EditItemTemplate>
                        <asp:Label ID="lblEditPreApprovalListFirstRoutingLevelTeamMember" runat="server"
                            Text='<%# Bind("ddTeamMemberName") %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewPreApprovalListFirstRoutingLevelTeamMember" runat="server"
                            Text='<%# Bind("ddTeamMemberName") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblFooterPreApprovalListFirstRoutingLevelTeamMemberMarker" runat="server"
                            Font-Bold="True" ForeColor="Red" Text="*" />
                        <asp:DropDownList ID="ddFooterPreApprovalListFirstRoutingLevelTeamMember" runat="server"
                            AutoPostBack="true" DataSource='<%# CostingModule.GetCostSheetApproverBySubscription(0,1) %>'
                            DataValueField="TeamMemberID" DataTextField="ddTeamMemberName" AppendDataBoundItems="True"
                            OnSelectedIndexChanged="ddFooterPreApprovalListFirstRoutingLevelTeamMember_SelectedIndexChanged">
                            <asp:ListItem Text="" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Comments" HeaderText="Comments" SortExpression="Comments"
                    ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" Wrap="True" />
                </asp:BoundField>
                <asp:BoundField DataField="SignedStatusDesc" HeaderText="Signed Status" SortExpression="SignedStatusDesc"
                    ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="SignedDate" HeaderText="Signed Date" ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="NotificationDate" HeaderText="Notification Date" ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Subscription" SortExpression="Subscription">
                    <EditItemTemplate>
                        <asp:Label ID="lblEditPreApprovalListFirstRoutingLevelSubscription" runat="server"
                            Text='<%# Bind("Subscription") %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewPreApprovalListFirstRoutingLevelSubscription" runat="server"
                            Text='<%# Bind("Subscription") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblFooterPreApprovalListFirstRoutingLevelSubscriptionMarker" runat="server"
                            Font-Bold="True" ForeColor="Red" Text="*" />
                        <asp:DropDownList ID="ddFooterPreApprovalListFirstRoutingLevelSubscription" runat="server"
                            AppendDataBoundItems="true" DataSource='<%# CostingModule.GetCostSheetSubscriptionByApprover(0,1) %>'
                            DataValueField="SubscriptionID" DataTextField="Subscription">
                            <asp:ListItem Text="" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="iBtnPreApprovalListFirstRoutingLevelDelete" runat="server" CausesValidation="False"
                            CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgFooterPreApprovalListFirstRoutingLevel"
                            runat="server" ID="iBtnFooterPreApprovalListFirstRoutingLevel" ImageUrl="~/images/save.jpg"
                            AlternateText="Insert" />
                        <asp:ImageButton ID="iBtnPreApprovalListFirstRoutingLevelUndo" runat="server" CommandName="Undo"
                            CausesValidation="false" ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsPreApprovalListFirstRoutingLevel" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetCostSheetPreApprovalList" TypeName="CostSheetPreApprovalBLL"
            DeleteMethod="DeleteCostSheetPreApprovalItem" InsertMethod="InsertCostSheetPreApprovalItem">
            <SelectParameters>
                <asp:QueryStringParameter Name="CostSheetID" QueryStringField="CostSheetID" Type="Int32" />
                <asp:Parameter DefaultValue="0" Name="TeamMemberID" Type="Int32" />
                <asp:Parameter DefaultValue="1" Name="RoutingLevel" Type="Int32" />
                <asp:Parameter DefaultValue="" Name="SignedStatus" Type="String" />
                <asp:Parameter DefaultValue="0" Name="SubscriptionID" Type="Int32" />
                <asp:Parameter DefaultValue="False" Name="FilterNotified" Type="Boolean" />
                <asp:Parameter DefaultValue="False" Name="isNotified" Type="Boolean" />
                <asp:Parameter DefaultValue="False" Name="isHistorical" Type="Boolean" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="RowID" Type="Int32" />
                <asp:Parameter Name="original_RowID" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="CostSheetID" Type="Int32" />
                <asp:Parameter Name="TeamMemberID" Type="Int32" />
                <asp:Parameter DefaultValue="1" Name="RoutingLevel" Type="Int32" />
                <asp:Parameter Name="SignedStatus" Type="String" />
                <asp:Parameter Name="SubscriptionID" Type="Int32" />
            </InsertParameters>
        </asp:ObjectDataSource>
        <br />
        <asp:Label ID="lblFirstLevelRoutingMessage" SkinID="MessageLabelSkin" runat="server"></asp:Label>
        <br />
        <asp:Button runat="server" ID="btnFirstLevelRoutingNotify" Text="Notify First Level Routing"
            Width="200px" Visible="false" />
        <asp:CheckBox runat="server" ID="cbFirstLevelRoutingNotifyOnlyNew" Text="Check to only notify new people added to the list"
            Checked="true" Visible="false" />
        <br />
        <br />
        <hr />
        <h1>
            Second Level Routing</h1>
        <asp:ValidationSummary ID="vsEditPreApprovalListSecondRoutingLevel" runat="server"
            DisplayMode="List" ShowMessageBox="true" ShowSummary="true" EnableClientScript="true"
            ValidationGroup="vgEditPreApprovalListSecondRoutingLevel" />
        <asp:ValidationSummary ID="vsFooterPreApprovalListSecondRoutingLevel" runat="server"
            DisplayMode="List" ShowMessageBox="true" ShowSummary="true" EnableClientScript="true"
            ValidationGroup="vgFooterPreApprovalListSecondRoutingLevel" />
        <asp:GridView runat="server" ID="gvPreApprovalListSecondRoutingLevel" AllowPaging="True"
            AllowSorting="True" AutoGenerateColumns="False" PageSize="15" ShowFooter="True"
            DataSourceID="odsPreApprovalListSecondRoutingLevel" DataKeyNames="RowID" Width="100%"
            EmptyDataText="No records found." Visible="False">
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
                <asp:BoundField DataField="RoutingLevel" HeaderText="RoutingLevel" SortExpression="RoutingLevel"
                    ReadOnly="True">
                    <HeaderStyle CssClass="none" />
                    <ItemStyle CssClass="none" />
                    <FooterStyle CssClass="none" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Team Member" SortExpression="TeamMemberName">
                    <EditItemTemplate>
                        <asp:Label ID="lblEditPreApprovalListSecondRoutingLevelTeamMember" runat="server"
                            Text='<%# Bind("ddTeamMemberName") %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewPreApprovalListSecondRoutingLevelTeamMember" runat="server"
                            Text='<%# Bind("ddTeamMemberName") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblFooterPreApprovalListSecondRoutingLevelTeamMemberMarker" runat="server"
                            Font-Bold="True" ForeColor="Red" Text="*" />
                        <asp:DropDownList ID="ddFooterPreApprovalListSecondRoutingLevelTeamMember" runat="server"
                            AutoPostBack="true" OnSelectedIndexChanged="ddFooterPreApprovalListSecondRoutingLevelTeamMember_SelectedIndexChanged"
                            DataSource='<%# CostingModule.GetCostSheetApproverBySubscription(0,2) %>' DataValueField="TeamMemberID"
                            DataTextField="ddTeamMemberName" AppendDataBoundItems="True">
                            <asp:ListItem Text="" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Comments" HeaderText="Comments" SortExpression="Comments"
                    ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" Wrap="True" />
                </asp:BoundField>
                <asp:BoundField DataField="SignedStatusDesc" HeaderText="Signed Status" SortExpression="SignedStatusDesc"
                    ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="SignedDate" HeaderText="Signed Date" SortExpression="SignedDate"
                    ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="NotificationDate" HeaderText="Notification Date" SortExpression="NotificationDate"
                    ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Subscription" SortExpression="Subscription">
                    <EditItemTemplate>
                        <asp:Label ID="lblEditPreApprovalListSecondRoutingLevelSubscription" runat="server"
                            Text='<%# Bind("Subscription") %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewPreApprovalListSecondRoutingLevelSubscription" runat="server"
                            Text='<%# Bind("Subscription") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblFooterPreApprovalListSecondRoutingLevelSubscriptionMarker" runat="server"
                            Font-Bold="True" ForeColor="Red" Text="*" />
                        <asp:DropDownList ID="ddFooterPreApprovalListSecondRoutingLevelSubscription" AppendDataBoundItems="true"
                            runat="server" DataSource='<%# CostingModule.GetCostSheetSubscriptionByApprover(0,2) %>'
                            DataValueField="Subscription">
                            <asp:ListItem Text="" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="iBtnPreApprovalListSecondRoutingLevelDelete" runat="server"
                            CausesValidation="False" CommandName="Delete" ImageUrl="~/images/delete.jpg"
                            AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgFooterPreApprovalListSecondRoutingLevel"
                            runat="server" ID="iBtnFooterPreApprovalListSecondRoutingLevel" ImageUrl="~/images/save.jpg"
                            AlternateText="Insert" />
                        <asp:ImageButton ID="iBtnPreApprovalListSecondRoutingLevelUndo" runat="server" CommandName="Undo"
                            CausesValidation="false" ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsPreApprovalListSecondRoutingLevel" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetCostSheetPreApprovalList" TypeName="CostSheetPreApprovalBLL"
            DeleteMethod="DeleteCostSheetPreApprovalItem" InsertMethod="InsertCostSheetPreApprovalItem">
            <SelectParameters>
                <asp:QueryStringParameter Name="CostSheetID" QueryStringField="CostSheetID" Type="Int32" />
                <asp:Parameter DefaultValue="0" Name="TeamMemberID" Type="Int32" />
                <asp:Parameter DefaultValue="2" Name="RoutingLevel" Type="Int32" />
                <asp:Parameter DefaultValue="" Name="SignedStatus" Type="String" />
                <asp:Parameter DefaultValue="0" Name="SubscriptionID" Type="Int32" />
                <asp:Parameter DefaultValue="False" Name="FilterNotified" Type="Boolean" />
                <asp:Parameter DefaultValue="False" Name="isNotified" Type="Boolean" />
                <asp:Parameter DefaultValue="False" Name="isHistorical" Type="Boolean" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="RowID" Type="Int32" />
                <asp:Parameter Name="original_RowID" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="CostSheetID" Type="Int32" />
                <asp:Parameter Name="TeamMemberID" Type="Int32" />
                <asp:Parameter DefaultValue="2" Name="RoutingLevel" Type="Int32" />
                <asp:Parameter Name="SignedStatus" Type="String" />
                <asp:Parameter Name="SubscriptionID" Type="Int32" />
            </InsertParameters>
        </asp:ObjectDataSource>
        <br />
        <asp:Label ID="lblSecondLevelRoutingMessage" SkinID="MessageLabelSkin" runat="server"></asp:Label>
        <br />
        <asp:Button runat="server" ID="btnSecondLevelRoutingNotify" Text="Notify Second Level Routing"
            Width="200px" Visible="false" />
        <asp:CheckBox runat="server" ID="cbSecondLevelRoutingNotifyOnlyNew" Text="Check to only notify new people added to the list"
            Checked="true" Visible="false" />
        <br />
        <br />
        <br />
        <br />
        <hr />
        <asp:Button runat="server" ID="btnNotifyAll" Text="Notify All Levels" Width="200px"
            Visible="false" />
        <asp:CheckBox runat="server" ID="cbAllLevelRoutingNotifyOnlyNew" Text="Check to only notify new people added to the list"
            Checked="true" Visible="false" />
        <hr />
        <h1>
            Complete Approval Routing History</h1>
        <asp:GridView runat="server" ID="gvPreApprovalHistory" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" PageSize="50" ShowFooter="True" DataSourceID="odsPreApprovaHistory"
            DataKeyNames="RowID" Width="90%" EmptyDataText="No records found." Visible="False">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
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
                <asp:BoundField DataField="RoutingLevel" HeaderText="RoutingLevel" SortExpression="RoutingLevel"
                    ReadOnly="True">
                    <HeaderStyle CssClass="none" />
                    <ItemStyle CssClass="none" />
                    <FooterStyle CssClass="none" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Team Member" SortExpression="TeamMemberName">
                    <ItemTemplate>
                        <asp:Label ID="lblViewPreApprovalListSecondRoutingLevelTeamMember" runat="server"
                            Text='<%# Bind("ddTeamMemberName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Comments" SortExpression="Comments" ItemStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblComments" runat="server" Text='<%# Bind("Comments") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="SignedStatusDesc" HeaderText="Signed Status" SortExpression="SignedStatusDesc"
                    ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="SignedDate" HeaderText="Signed Date" SortExpression="SignedDate"
                    ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="NotificationDate" HeaderText="Notification Date" SortExpression="NotificationDate"
                    ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Subscription" SortExpression="Subscription">
                    <ItemTemplate>
                        <asp:Label ID="lblViewPreApprovalListSecondRoutingLevelSubscription" runat="server"
                            Text='<%# Bind("Subscription") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsPreApprovaHistory" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetCostSheetPreApprovalList" TypeName="CostSheetPreApprovalBLL">
            <SelectParameters>
                <asp:QueryStringParameter Name="CostSheetID" QueryStringField="CostSheetID" Type="Int32" />
                <asp:Parameter DefaultValue="0" Name="TeamMemberID" Type="Int32" />
                <asp:Parameter DefaultValue="0" Name="RoutingLevel" Type="Int32" />
                <asp:Parameter DefaultValue="" Name="SignedStatus" Type="String" />
                <asp:Parameter DefaultValue="0" Name="SubscriptionID" Type="Int32" />
                <asp:Parameter DefaultValue="False" Name="FilterNotified" Type="Boolean" />
                <asp:Parameter DefaultValue="False" Name="isNotified" Type="Boolean" />
                <asp:Parameter DefaultValue="True" Name="isHistorical" Type="Boolean" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
