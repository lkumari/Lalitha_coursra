<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Cost_Sheet_Pre_Approval_PopUp.aspx.vb"
    Inherits="Costing_Cost_Sheet_Pre_Approval_PopUp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cost Sheet Pre Approval List</title>
</head>
<body>
    <br /><br />
    <form id="form1" runat="server">
        <div>
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
                    </tr>
                </table>
                <asp:GridView runat="server" ID="gvPreApprovalList" AllowPaging="True" AllowSorting="True"
                    AutoGenerateColumns="False" PageSize="15" ShowFooter="True" DataSourceID="odsPreApprovalList"
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
                        <asp:BoundField DataField="RoutingLevel" HeaderText="RoutingLevel" SortExpression="RoutingLevel"
                            ReadOnly="True">
                            <HeaderStyle CssClass="none" />
                            <ItemStyle CssClass="none" />
                            <FooterStyle CssClass="none" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Team Member" SortExpression="TeamMemberName">
                            <ItemTemplate>
                                <asp:Label ID="lblViewPreApprovalListFirstRoutingLevelTeamMember" runat="server"
                                    Text='<%# Bind("ddTeamMemberName") %>'></asp:Label>
                            </ItemTemplate>
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
                            <ItemTemplate>
                                <asp:Label ID="lblViewPreApprovalListFirstRoutingLevelSubscription" runat="server"
                                    Text='<%# Bind("Subscription") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsPreApprovalList" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetCostSheetPreApprovalList" TypeName="CostSheetPreApprovalBLL">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="CostSheetID" QueryStringField="CostSheetID" Type="Int32" />
                        <asp:Parameter DefaultValue="0" Name="TeamMemberID" Type="Int32" />
                        <asp:Parameter DefaultValue="0" Name="RoutingLevel" Type="Int32" />
                        <asp:Parameter DefaultValue="" Name="SignedStatus" Type="String" />
                        <asp:Parameter DefaultValue="0" Name="SubscriptionID" Type="Int32" />
                        <asp:Parameter DefaultValue="False" Name="FilterNotified" Type="Boolean" />
                        <asp:Parameter DefaultValue="False" Name="isNotified" Type="Boolean" />
                        <asp:Parameter DefaultValue="False" Name="isHistorical" Type="Boolean" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <br />
            </asp:Panel>
        </div>
    </form>
</body>
</html>
