<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Cost_Sheet_Approve.aspx.vb" Inherits="Costing_Cost_Sheet_Approve" Title="Untitled Page"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <asp:Label ID="lblMessage" SkinID="MessageLabelSkin" runat="server"></asp:Label>
        <br />
        <asp:ValidationSummary ID="vsSave" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgSave" />
        <br />
        <asp:Label ID="lblTip" SkinID="MessageLabelSkin" runat="server" Text="If you need to approve as a backup to another team member, select the name in the dropdown and click search. <br>Once you actually approve or reject the appropriate Cost Sheet in the lower section, your name will replace the original person."></asp:Label>
        <br />
        <table style="width: 98%">
            <tr>
                <td class="p_textbold" style="width: 40%">
                    <asp:Label runat="server" ID="lblSearchCostSheetLabel" Text="Cost Sheet ID:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchCostSheetID" MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvSearchCostSheetID" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="integer" Text="<" ControlToValidate="txtSearchCostSheetID"
                        ErrorMessage="CostsheetID must be an exact number when used." SetFocusOnError="True" />
                </td>
            </tr>
            <tr>
                <td class="p_textbold">
                    <asp:Label runat="server" ID="lblSearchSignedStatusLabel" Text="Signed Status:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddSearchSignedStatus" runat="server">
                        <asp:ListItem Text="Pending" Value="P" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Approved" Value="A"></asp:ListItem>
                        <asp:ListItem Text="Rejected" Value="R"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_textbold">
                    <asp:Label runat="server" ID="lblRole" Text="Role:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddSubscription" runat="server" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_textbold">
                    <asp:Label runat="server" ID="lblSearchTeamMember" Text="Team Member:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddSearchTeamMember" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <br />
        <table style="width: 98%">
            <tr>
                <td align="center">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" Visible="false" ValidationGroup="vgSave"
                        CausesValidation="true" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" Visible="false" CausesValidation="false" />
                    <br />
                    <br />
                    <asp:LinkButton runat="server" ID="lnkGoToCostSheetSearch" PostBackUrl="~/Costing/Cost_Sheet_List.aspx"
                        Text="Click here to go to the full search page." Font-Bold="true"></asp:LinkButton>
                </td>
            </tr>
        </table>
        <table style="width: 98%">
            <tr>
                <td colspan="2">
                    <asp:TextBox runat="server" ID="txtCurrentTeamMemberID" Visible="false"></asp:TextBox>
                    <asp:Label ID="lblInstructions" SkinID="MessageLabelSkin" runat="server" Text="Click the edit button under the ACTION column. Enter the comment and the approval status. THEN SAVE.<br>"></asp:Label>
                    <br />
                    <br />
                    <asp:Label ID="lblMessageBottom" SkinID="MessageLabelSkin" runat="server"></asp:Label>
                    <br />
                    <asp:ValidationSummary ID="vsEditApprovalList" runat="server" DisplayMode="List"
                        ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgEditApprovalList" />
                    <asp:GridView runat="server" ID="gvApprovalList" AllowPaging="True" AllowSorting="True"
                        AutoGenerateColumns="False" PageSize="15" DataSourceID="odsApprovalList" DataKeyNames="RowID"
                        Width="100%" EmptyDataText="No records.">
                        <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" Font-Size="Small" ForeColor="#333333" />
                        <HeaderStyle BackColor="#990000" Font-Bold="True" Font-Size="Small" ForeColor="White" />
                        <EditRowStyle BackColor="#CCCCCC" Font-Size="Small" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" Font-Size="Small" Width="100%" />
                        <Columns>
                            <asp:BoundField DataField="RowID" HeaderText="RowID" ReadOnly="True">
                                <HeaderStyle CssClass="none" />
                                <ItemStyle CssClass="none" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Action">
                                <EditItemTemplate>
                                    <asp:ImageButton ID="iBtnApprovalListUpdate" runat="server" CausesValidation="True"
                                        CommandName="Update" ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditApprovalList" />
                                    <asp:ImageButton ID="iBtnApprovalListCancel" runat="server" CausesValidation="False"
                                        CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="iBtnApprovalListEdit" runat="server" CausesValidation="False"
                                        CommandName="Edit" ImageUrl="~/images/edit.jpg" AlternateText="Edit" Visible='<%# Bind("isAllowEdit") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" Font-Size="Small" />
                                <ItemStyle HorizontalAlign="Center" Wrap="False" Font-Size="Small" BackColor="Yellow" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cost Sheet ID" SortExpression="CostSheetID">
                                <EditItemTemplate>
                                    <asp:Label ID="lblEditCostSheetID" runat="server" Text='<%# Bind("CostSheetID") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblViewCostSheetID" runat="server" Text='<%# Bind("CostSheetID") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Font-Size="Small" HorizontalAlign="Center" />
                                <ItemStyle Font-Size="Small" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="ddTeamMemberName" HeaderText="Team Member Name" SortExpression="ddTeamMemberName"
                                ReadOnly="True">
                                <HeaderStyle HorizontalAlign="Center" Font-Size="Small" />
                                <ItemStyle HorizontalAlign="Left" Font-Size="Small" />
                            </asp:BoundField>
                             <asp:BoundField DataField="ShowPartNo" HeaderText="Part No" SortExpression="ShowPartNo"
                                ReadOnly="True">
                                <HeaderStyle HorizontalAlign="Center" Font-Size="Small"  Wrap="false" />
                                <ItemStyle HorizontalAlign="Left" Font-Size="Small"   Wrap="false" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Comments" SortExpression="Comments">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditApprovalListComments" runat="server" MaxLength="100" Width="200px"
                                        Text='<%# Bind("Comments") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblViewApprovalListComments" runat="server" Text='<%# Bind("Comments") %>'
                                        Width="200px"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" Font-Size="Small" />
                                <ItemStyle HorizontalAlign="Left" Font-Size="Small" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SignedStatus" SortExpression="SignedStatus">
                                <EditItemTemplate>
                                    <asp:Label ID="lblEditApprovalListSignedStatusDescMarker" runat="server" Font-Bold="True"
                                        ForeColor="Red" Text="*" />
                                    <asp:DropDownList ID="ddEditApprovalListSignedStatusDesc" runat="server" DataValueField="SubscriptionID"
                                        SelectedValue='<%# Bind("SignedStatus") %>'>
                                        <asp:ListItem Text="" Value="P"></asp:ListItem>
                                        <asp:ListItem Text="Approved" Value="A"></asp:ListItem>
                                        <asp:ListItem Text="Rejected" Value="R"></asp:ListItem>
                                        <asp:ListItem Text="" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblViewSignStatusDesc" runat="server" Text='<%# Bind("SignedStatusDesc") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" Font-Size="Small" />
                                <ItemStyle HorizontalAlign="Left" Font-Size="Small" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="SignedDate" HeaderText="Signed Date" SortExpression="SignedDate"
                                ReadOnly="True">
                                <HeaderStyle HorizontalAlign="Center" Font-Size="Small" />
                                <ItemStyle HorizontalAlign="Center" Font-Size="Small" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="RoutingLevel" SortExpression="RoutingLevel">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditApprovalListRoutingLevel" runat="server" Text='<%# Bind("RoutingLevel") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblViewApprovalListRoutingLevel" runat="server" Text='<%# Bind("RoutingLevel") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="none" />
                                <ItemStyle CssClass="none" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SubscriptionID" SortExpression="SubscriptionID">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditApprovalListSubscriptionID" runat="server" Text='<%# Bind("SubscriptionID") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblViewApprovalListSubscriptionID" runat="server" Text='<%# Bind("SubscriptionID") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="none" />
                                <ItemStyle CssClass="none" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="isCostReduction" HeaderText="Cost Reduction?" 
                                SortExpression="isCostReduction" ItemStyle-HorizontalAlign="Center" 
                                ItemStyle-Font-Size="Small" ReadOnly="true" >
                                <ItemStyle Font-Size="Small" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="See All Approvers">
                                <EditItemTemplate>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <a runat="server" id="aCostSheetPreApprovalInfo" href="#">
                                        <asp:Image runat="server" ID="imgPreApprovalInfo" ImageUrl="~/images/History.jpg" /></a>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" Font-Size="Small" />
                                <ItemStyle HorizontalAlign="Center" Font-Size="Small" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Preview Cost Form">
                                <EditItemTemplate>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <a runat="server" id="aCostForm" href="#">
                                        <asp:Image runat="server" ID="imgCostForm" ImageUrl="~/images/PreviewUp.jpg" /></a>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" Font-Size="Small" />
                                <ItemStyle HorizontalAlign="Center" Font-Size="Small" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Preview Die Layout">
                                <EditItemTemplate>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <a runat="server" id="aCostSheetDieLayout" href="#" visible='<%# Eval("isDieCut") %>'>
                                        <asp:Image runat="server" ID="imgCostSheetDieLayout" ImageUrl="~/images/PreviewUp.jpg" /></a>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" Font-Size="Small" />
                                <ItemStyle HorizontalAlign="Center" Font-Size="Small" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="odsApprovalList" runat="server" OldValuesParameterFormatString="original_{0}"
                        SelectMethod="GetCostSheetPreApprovalList" TypeName="CostSheetPreApprovalBLL"
                        UpdateMethod="UpdateCostSheetPreApprovalStatus" 
                        DeleteMethod="DeleteCostSheetPreApprovalItem" 
                        InsertMethod="InsertCostSheetPreApprovalItem">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="txtSearchCostSheetID" Name="CostSheetID" PropertyName="Text"
                                Type="Int32" />
                            <asp:ControlParameter ControlID="ddSearchTeamMember" DefaultValue="0" Name="TeamMemberID"
                                PropertyName="SelectedValue" Type="Int32" />
                            <asp:Parameter DefaultValue="0" Name="RoutingLevel" Type="Int32" />
                            <asp:ControlParameter ControlID="ddSearchSignedStatus" DefaultValue="P" Name="SignedStatus"
                                PropertyName="SelectedValue" Type="String" />
                            <asp:Parameter DefaultValue="0" Name="SubscriptionID" Type="Int32" />
                            <asp:Parameter DefaultValue="True" Name="FilterNotified" Type="Boolean" />
                            <asp:Parameter DefaultValue="True" Name="isNotified" Type="Boolean" />
                            <asp:Parameter DefaultValue="False" Name="isHistorical" Type="Boolean" />
                        </SelectParameters>
                        <DeleteParameters>
                            <asp:Parameter Name="RowID" Type="Int32" />
                            <asp:Parameter Name="original_RowID" Type="Int32" />
                        </DeleteParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="CostSheetID" Type="Int32" />
                            <asp:ControlParameter ControlID="txtCurrentTeamMemberID" Name="TeamMemberID" PropertyName="Text"
                                Type="Int32" />
                            <asp:Parameter Name="Comments" Type="String" />
                            <asp:Parameter Name="SignedStatus" Type="String" />
                            <asp:Parameter Name="original_RowID" Type="Int32" />
                            <asp:Parameter Name="RoutingLevel" Type="Int32" />
                            <asp:Parameter Name="SubscriptionID" Type="Int32" />
                            <asp:Parameter Name="isCostReduction" Type="Boolean" />
                        </UpdateParameters>
                        <InsertParameters>
                            <asp:Parameter Name="CostSheetID" Type="Int32" />
                            <asp:Parameter Name="TeamMemberID" Type="Int32" />
                            <asp:Parameter Name="RoutingLevel" Type="Int32" />
                            <asp:Parameter Name="SignedStatus" Type="String" />
                            <asp:Parameter Name="SubscriptionID" Type="Int32" />
                        </InsertParameters>
                    </asp:ObjectDataSource>
                    <br />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
