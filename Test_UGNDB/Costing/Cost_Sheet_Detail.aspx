<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Cost_Sheet_Detail.aspx.vb" MaintainScrollPositionOnPostback="true"
    EnableEventValidation="false" Inherits="Cost_Sheet_Detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server">
        <asp:Label ID="lblMessage" SkinID="MessageLabelSkin" runat="server"></asp:Label>
        <asp:ValidationSummary ID="vsSave" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgSave" />
        <table width="98%">
            <tr>
                <td class="p_text" style="white-space: nowrap; width: 20%">
                    <asp:Label runat="server" ID="lblCostSheetIDLabel" Text="Cost Sheet ID:" Visible="false"></asp:Label>
                </td>
                <td class="c_textbold" style="white-space: nowrap; width: 20%">
                    <asp:Label runat="server" ID="lblCostSheetIDValue" Visible="false"></asp:Label>
                </td>
                <td class="p_text" style="white-space: nowrap; width: 15%">
                    <asp:Label runat="server" ID="lblPreviousCostSheetIDLabel" Text="Previous Cost Sheet ID:"
                        Visible="false"></asp:Label>
                </td>
                <td class="c_textbold" style="white-space: nowrap; width: 15%">
                    <asp:HyperLink runat="server" ID="hlnkPreviousCostSheetIDValue" Visible="false" Font-Underline="true"
                        ToolTip="Click here to see previous Cost Sheet." Target="_blank"></asp:HyperLink>
                </td>
                <td class="p_text" style="white-space: nowrap; width: 15%">
                    <asp:Label runat="server" ID="lblApprovedDateLabel" Text="Approved Date:" Visible="false"></asp:Label>
                </td>
                <td class="c_textbold" style="white-space: nowrap; width: 15%">
                    <asp:Label runat="server" ID="lblApprovedDateValue" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="p_text" style="white-space: nowrap;">
                    <asp:Label ID="lblCostSheetStatusMarker" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="*" Visible="false" />
                    <asp:Label runat="server" ID="lblCostSheetStatusLabel" Text="Cost Sheet Status:"
                        Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddCostSheetStatusValue" runat="server" Enabled="false" AutoPostBack="true">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Text="Current" Value="Current"></asp:ListItem>
                        <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                        <asp:ListItem Text="Previous" Value="Previous"></asp:ListItem>
                        <asp:ListItem Text="Proposal" Value="Proposal"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvCostSheetStatus" runat="server" ControlToValidate="ddCostSheetStatusValue"
                        ErrorMessage="Cost sheet status is required." Font-Bold="True" ValidationGroup="vgSave"
                        Text="<" SetFocusOnError="true" />
                </td>
                <td class="p_text" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblQuoteDateLabel" Text="Last Revision Date:" Visible="false"></asp:Label>
                </td>
                <td style="white-space: nowrap;">
                    <asp:TextBox runat="server" ID="txtQuoteDateValue" Enabled="false" Visible="false"></asp:TextBox>
                    <asp:ImageButton runat="server" ID="imgQuoteDateValue" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                    <ajax:CalendarExtender ID="calendarButtonExtender" runat="server" TargetControlID="txtQuoteDateValue"
                        PopupButtonID="imgQuoteDateValue" />
                    <asp:RegularExpressionValidator ID="revQuoteDateDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtQuoteDateValue" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px" ValidationGroup="vgSave"><</asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td class="p_text" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblRFDNoLabel" Text="RFD No:"></asp:Label>
                </td>
                <td style="white-space: nowrap;">
                    <asp:TextBox runat="server" ID="txtRFDNoValue" MaxLength="10"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="revRFDNo" runat="server" ControlToValidate="txtRFDNoValue"
                        ValidationGroup="vgSave" Text="<" ErrorMessage="Only numbers can be used for the RFD."
                        SetFocusOnError="True" ValidationExpression="\b\d+\b"></asp:RegularExpressionValidator>
                    <asp:ImageButton ID="iBtnSearchRFD" runat="server" ImageUrl="~/images/Search.gif"
                        ToolTip="Click here to search for an RFD." />
                    <asp:ImageButton ID="iBtnGetRFDinfo" runat="server" ImageUrl="~/images/SelectUser.gif"
                        ToolTip="Click here to pull part information from an RFD." />
                    <asp:HyperLink runat="server" ID="hlnkRFD" Visible="false" Font-Underline="true"
                        ToolTip="Click here to view the RFD" Text="View RFD" Target="_blank"></asp:HyperLink>
                    <br />
                    <asp:TextBox runat="server" ID="txtRFDSelectionType" CssClass="none"></asp:TextBox>
                    <asp:TextBox runat="server" ID="txtRFDChildRow" CssClass="none"></asp:TextBox>
                </td>
                <td class="p_text" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblECINoLabel" Text="ECI No:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtECINoValue" MaxLength="10" Enabled="false" Visible="false"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="revECINoValue" runat="server" ControlToValidate="txtECINoValue"
                        ValidationGroup="vgSave" Text="<" ErrorMessage="Only numbers can be used for the ECI."
                        SetFocusOnError="True" ValidationExpression="\b\d+\b"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td class="p_text" style="white-space: nowrap;">
                    <asp:CheckBox ID="cbQuickQuote" runat="server" Text="Quick Quote" TextAlign="Left" />
                </td>
                <td style="white-space: nowrap;">
                </td>
                <td class="p_text" style="white-space: nowrap;">
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
        <ajax:Accordion ID="accReplicationActivity" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
            FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
            RequireOpenedPane="false" SuppressHeaderPostbacks="true">
            <Panes>
                <ajax:AccordionPane ID="apReplicationActivity" runat="server">
                    <Header>
                        <a href="" class="accordionLink">Replication Activity</a></Header>
                    <Content>
                        <table width="98%">
                            <tr>
                                <td>
                                    <asp:GridView runat="server" ID="gvReplicatedFrom" Width="100%" DataSourceID="odsReplicatedFrom"
                                        AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" PageSize="1"
                                        Visible="False" EmptyDataText="This Cost Sheet was NOT replicated FROM another.">
                                        <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                        <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                        <EditRowStyle BackColor="#CCCCCC" />
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Replicated FROM<br>Cost Sheet ID" SortExpression="CostSheetID"
                                                ItemStyle-Width="25%">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="lnkViewReplicatedFromCostSheet" runat="server" NavigateUrl='<%# Eval("CostSheetID", "Cost_Sheet_Detail.aspx?CostSheetID={0}") %>'
                                                        Target="_blank" Text='<%# Eval("CostSheetID") %>'></asp:HyperLink>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Status" DataField="CostSheetStatus" SortExpression="CostSheetStatus"
                                                ItemStyle-Width="25%">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Quote Date" DataField="QuoteDate" SortExpression="QuoteDate"
                                                ItemStyle-Width="25%">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Approved Date" DataField="ApprovedDate" SortExpression="ApprovedDate"
                                                ItemStyle-Width="25%">
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:ObjectDataSource ID="odsReplicatedFrom" runat="server" SelectMethod="GetCostSheetReplicatedFrom"
                                        TypeName="CostingModule" OldValuesParameterFormatString="original_{0}">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="hlnkPreviousCostSheetIDValue" DefaultValue="0" Name="PreviousCostSheetID"
                                                PropertyName="Text" Type="Int32" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView runat="server" ID="gvReplicatedTo" Width="100%" DataSourceID="odsReplicatedTo"
                                        AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" PageSize="4"
                                        ShowFooter="False" Visible="false" EmptyDataText="This Cost Sheet was NOT replicated TO any others.">
                                        <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                        <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                        <EditRowStyle BackColor="#CCCCCC" />
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Replicated TO<br>Cost Sheet ID" SortExpression="CostSheetID"
                                                ItemStyle-HorizontalAlign="Center" ItemStyle-Width="25%">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="lnkViewReplicatedToCostSheet" runat="server" NavigateUrl='<%# Eval("CostSheetID", "Cost_Sheet_Detail.aspx?CostSheetID={0}") %>'
                                                        Target="_blank" Text='<%# Eval("CostSheetID") %>'></asp:HyperLink>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Status" DataField="CostSheetStatus" SortExpression="CostSheetStatus"
                                                ItemStyle-HorizontalAlign="Center" ItemStyle-Width="25%" />
                                            <asp:BoundField HeaderText="Quote Date" DataField="QuoteDate" SortExpression="QuoteDate"
                                                ItemStyle-HorizontalAlign="Center" ItemStyle-Width="25%" />
                                            <asp:BoundField HeaderText="Approved Date" DataField="ApprovedDate" SortExpression="ApprovedDate"
                                                ItemStyle-HorizontalAlign="Center" ItemStyle-Width="25%" />
                                        </Columns>
                                    </asp:GridView>
                                    <asp:ObjectDataSource ID="odsReplicatedTo" runat="server" SelectMethod="GetCostSheetReplicatedTo"
                                        TypeName="CostingModule">
                                        <SelectParameters>
                                            <asp:QueryStringParameter Name="CostSheetID" QueryStringField="CostSheetID" Type="Int32" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                </td>
                            </tr>
                        </table>
                    </Content>
                </ajax:AccordionPane>
            </Panes>
        </ajax:Accordion>
        <ajax:Accordion ID="accCostHeader" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
            FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
            RequireOpenedPane="false" SuppressHeaderPostbacks="true">
            <Panes>
                <ajax:AccordionPane ID="apCostHeader" runat="server">
                    <Header>
                        <a href="" class="accordionLink">Cost Sheet Internal Part No(s), Customer Part No, and
                            Drawing No</a></Header>
                    <Content>
                        <table width="98%">
                            <tr>
                                <td class="p_text">
                                    <asp:Label runat="server" ID="lblDesignmationTypeLabel" Text="Designation Type:"
                                        Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddDesignationTypeValue" runat="server" Visible="false" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td class="p_text">
                                    <asp:Label runat="server" ID="lblNewDrawingNoLabel" Text="New DrawingNo:" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtNewDrawingNoValue" MaxLength="17" Enabled="false"
                                        Visible="false"></asp:TextBox>
                                    <asp:ImageButton ID="iBtnGetDrawingInfo" runat="server" ImageUrl="~/images/Search.gif"
                                        ToolTip="Click here to search for a DMS Drawing." Visible="false" />
                                    <asp:ImageButton ID="iBtnCopyDrawingInfo" runat="server" ImageUrl="~/images/SelectUser.gif"
                                        ToolTip="Click here to copy details based on the DMS Drawing." Visible="false" />
                                    &nbsp;
                                    <asp:HyperLink runat="server" ID="hlnkNewDrawingNo" Visible="false" Font-Underline="true"
                                        ToolTip="Click here to view the DMS Drawing." Text="View DMS Drawing" Target="_self"></asp:HyperLink>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    <asp:Label ID="lblUGNFacilityMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                        Text="*" Visible="false" />
                                    <asp:Label runat="server" ID="lblUGNFacilityLabel" Text="UGN Facility:" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddUGNFacilityValue" runat="server" Enabled="false" Visible="false">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvUGNFacility" runat="server" ControlToValidate="ddUGNFacilityValue"
                                        ErrorMessage="UGN Facility is required." Font-Bold="True" ValidationGroup="vgSave"
                                        Text="<" SetFocusOnError="true" />
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    <asp:Label runat="server" ID="lblOldPartNoLabel" Text="Part #:" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblOldPartNoValue" Visible="false"></asp:Label>
                                </td>
                                <td class="p_text">
                                    <asp:Label runat="server" ID="lblOldFinishedGoodPartNoLabel" Text="Finished Good Part #:"
                                        Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblOldFinishedGoodPartNoValue" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    <asp:Label runat="server" ID="lblOldOriginalPartNoLabel" Text="Original Part #:"
                                        Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblOldOriginalPartNoValue" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    <asp:Label runat="server" ID="lblNewPartNameLabel" Text="New Customer Part Name:"
                                        Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtNewPartNameValue" MaxLength="240" Width="200px"
                                        Enabled="false" Visible="false"></asp:TextBox>
                                </td>
                                <td colspan="2">
                                    &nbsp
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    <asp:Label runat="server" ID="lblPurchasedGoodLabel" Text="Purchased Good:" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddPurchasedGoodValue" runat="server" Enabled="false" Visible="false">
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2">
                                    &nbsp
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    <asp:Label runat="server" ID="lblNewPartNoLabel" Text="New Internal Part No:" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtNewPartNoValue" MaxLength="40" Width="200px" Enabled="false"
                                        Visible="false" />
                                    <asp:ImageButton ID="iBtnGetNewPartNo" runat="server" ImageUrl="~/images/Search.gif"
                                        ToolTip="Click here to search for a Internal Part No." />
                                </td>
                                <td class="p_text">
                                    <asp:Label runat="server" ID="lblNewPartRevisionLabel" Text="New Revision:" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtNewPartRevisionValue" MaxLength="2" Enabled="false"
                                        Visible="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    <asp:Label runat="server" ID="lblOriginalPartNoLabel" Text="Original Internal Part No:"
                                        Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtOriginalPartNoValue" MaxLength="40" Width="200px"
                                        Enabled="false" Visible="false"></asp:TextBox>
                                    <asp:ImageButton ID="iBtnOriginalPartNo" runat="server" ImageUrl="~/images/Search.gif"
                                        ToolTip="Click here to search for a Internal Part No." />
                                </td>
                                <td class="p_text">
                                    <asp:Label runat="server" ID="lblOriginalPartRevisionLabel" Text="Revision:" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtOriginalPartRevisionValue" MaxLength="2" Enabled="false"
                                        Visible="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text" valign="top">
                                    <asp:Label runat="server" ID="lblCommodityLabel" Text="Commodity:" Visible="false"></asp:Label>
                                </td>
                                <td valign="top">
                                    <asp:DropDownList ID="ddCommodityValue" runat="server" Enabled="false">
                                    </asp:DropDownList>
                                    <br />
                                    {Commodity / Classification}
                                </td>
                                <td colspan="2">
                                    &nbsp
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    <asp:Label runat="server" ID="lblNewCustomerPartNoLabel" Text="New Customer Part No:"
                                        Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtNewCustomerPartNoValue" MaxLength="40" Width="200px"
                                        Enabled="false" Visible="false"></asp:TextBox>
                                    <%--<asp:ImageButton ID="ibtnGetNewCustomerPartNo" runat="server" ImageUrl="~/images/Search.gif"
                                        Visible="false" ToolTip="Click here to search for a customer part number." />--%>
                                </td>
                                <td class="p_text">
                                    <asp:Label runat="server" ID="lblNewDesignLevelLabel" Text="New Design Level:" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtNewDesignLevelValue" MaxLength="25" Enabled="false"
                                        Visible="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    <asp:Label runat="server" ID="lblOriginalCustomerPartNoLabel" Text="Original Customer Part No:"
                                        Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtOriginalCustomerPartNoValue" MaxLength="40" Width="200px"
                                        Enabled="false" Visible="false"></asp:TextBox>
                                    <%-- <asp:ImageButton ID="ibtnGetOriginalCustomerPartNo" runat="server" ImageUrl="~/images/Search.gif"
                                        Visible="false" ToolTip="Click here to search for a customer part number." />--%>
                                </td>
                                <td class="p_text">
                                    <asp:Label runat="server" ID="lblOriginalDesignLevelLabel" Text="Original Design Level:"
                                        Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtOriginalDesignLevelValue" MaxLength="25" Enabled="false"
                                        Visible="false"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <table width="98%">
                            <tr>
                                <td colspan="4">
                                    <table width="98%" border="0">
                                        <asp:ValidationSummary ID="vsFooterTopLevelInfo" runat="server" ShowMessageBox="True"
                                            ShowSummary="true" ValidationGroup="vgFooterTopLevelInfo" />
                                        <asp:ValidationSummary ID="vsEditTopLevelInfo" runat="server" ShowMessageBox="True"
                                            ShowSummary="true" ValidationGroup="vgEditTopLevelInfo" />
                                        <asp:GridView ID="gvTopLevelInfo" runat="server" AutoGenerateColumns="False" DataKeyNames="CostSheetID,PartNo"
                                            AllowSorting="True" AllowPaging="True" PageSize="5" ShowFooter="True" DataSourceID="odsCostSheetTopLevelInfo"
                                            EmptyDataText="There are no top level parts currently defined for this cost sheet."
                                            Width="100%">
                                            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                            <EditRowStyle BackColor="#CCCCCC" />
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Finished Good Part No" SortExpression="PartNo">
                                                    <EditItemTemplate>
                                                        <asp:Label ID="lblEditPartNo" runat="server" Text='<%# Bind("PartNo") %>'></asp:Label>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="lnkViewPartNo" runat="server" NavigateUrl='<%# Bind("PartNo", "~/DataMaintenance/PartMaintenance.aspx?PartNo={0}") %>'
                                                            Target="_blank" Text='<%# Bind("PartNo") %>'></asp:HyperLink>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtFooterTopLevelPartNo" runat="server" MaxLength="17" Text='<%# Bind("PartNo") %>'
                                                            Width="175px"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvFooterTopLevelInfo" runat="server" ControlToValidate="txtFooterTopLevelPartNo"
                                                            ErrorMessage="Part No is Required for New Insert." Font-Bold="True" ValidationGroup="vgFooterTopLevelInfo"
                                                            Text="<" SetFocusOnError="true">				                                                            
                                                        </asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                    <HeaderStyle HorizontalAlign="left" Wrap="true" />
                                                    <FooterStyle HorizontalAlign="left" Wrap="False" />
                                                    <ItemStyle HorizontalAlign="left" Wrap="False" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Part Name" SortExpression="PartName">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtEditPartName" runat="server" Text='<%# Bind("PartName") %>' Width="350px"
                                                            MaxLength="30"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblViewPartName" runat="server" Text='<%# Bind("PartName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtFooterPartName" runat="server" MaxLength="30" Text='<%# Bind("PartName") %>'
                                                            Width="350px"></asp:TextBox>
                                                    </FooterTemplate>
                                                    <HeaderStyle HorizontalAlign="left" Wrap="true" />
                                                    <FooterStyle HorizontalAlign="left" Wrap="False" />
                                                    <ItemStyle HorizontalAlign="left" Wrap="False" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ShowHeader="False">
                                                    <EditItemTemplate>
                                                        <asp:ImageButton ID="ibtnTopLevelInfoUpdate" runat="server" CausesValidation="True"
                                                            CommandName="Update" ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditTopLevelInfo" />
                                                        &nbsp;&nbsp;&nbsp;
                                                        <asp:ImageButton ID="ibtnTopLevelInfoCancel" runat="server" CausesValidation="False"
                                                            CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ibtnTopLevelInfoEdit" runat="server" CausesValidation="False"
                                                            CommandName="Edit" ImageUrl="~/images/edit.jpg" AlternateText="Edit" />&nbsp;&nbsp;&nbsp;
                                                        <asp:ImageButton ID="ibtnTopLevelInfoDelete" runat="server" CausesValidation="False"
                                                            CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:ImageButton ID="ibtnTopLevelInfoInsert" runat="server" CausesValidation="True"
                                                            CommandName="Insert" ImageUrl="~/images/save.jpg" AlternateText="Insert" ValidationGroup="vgFooterTopLevelInfo" />&nbsp;&nbsp;&nbsp;
                                                        <asp:ImageButton ID="ibtnTopLevelInfoUndo" runat="server" CommandName="Undo" ImageUrl="~/images/undo-gray.jpg"
                                                            AlternateText="Undo" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:ObjectDataSource ID="odsCostSheetTopLevelInfo" runat="server" OldValuesParameterFormatString="original_{0}"
                                            SelectMethod="GetCostSheetTopLevelBPCSPartInfo" TypeName="CostSheetTopLevelBPCSPartInfoBLL"
                                            DeleteMethod="DeleteCostSheetTopLevelBPCSPartInfo" InsertMethod="InsertCostSheetTopLevelBPCSPartInfo"
                                            UpdateMethod="UpdateCostSheetTopLevelBPCSPartInfo">
                                            <SelectParameters>
                                                <asp:QueryStringParameter Name="CostSheetID" QueryStringField="CostSheetID" Type="Int32" />
                                            </SelectParameters>
                                            <DeleteParameters>
                                                <asp:QueryStringParameter Name="CostSheetID" QueryStringField="CostSheetID" Type="Int32" />
                                                <asp:Parameter Name="PartNo" Type="String" />
                                                <asp:Parameter Name="original_CostSheetID" Type="Int32" />
                                                <asp:Parameter Name="original_PartNo" Type="String" />
                                            </DeleteParameters>
                                            <UpdateParameters>
                                                <asp:QueryStringParameter Name="CostSheetID" QueryStringField="CostSheetID" Type="Int32" />
                                                <asp:Parameter Name="PartNo" Type="String" />
                                                <asp:Parameter Name="PartRevision" Type="String" />
                                                <asp:Parameter Name="PartName" Type="String" />
                                                <asp:Parameter Name="original_CostSheetID" Type="Int32" />
                                                <asp:Parameter Name="original_PartNo" Type="String" />
                                            </UpdateParameters>
                                            <InsertParameters>
                                                <asp:QueryStringParameter Name="CostSheetID" QueryStringField="CostSheetID" Type="Int32" />
                                                <asp:Parameter Name="PartNo" Type="String" />
                                                <asp:Parameter Name="PartRevision" Type="String" />
                                                <asp:Parameter Name="PartName" Type="String" />
                                            </InsertParameters>
                                        </asp:ObjectDataSource>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table width="98%" border="0">
                            <tr>
                                <td class="c_text" align="left">
                                    <asp:Label ID="lblNotesValue" runat="server" Text="Notes:" Visible="false"></asp:Label>
                                    <br />
                                    <asp:TextBox runat="server" ID="txtNotesValue" TextMode="MultiLine" Visible="false"
                                        Enabled="false" Height="60px" Width="90%"></asp:TextBox>
                                    <br />
                                    <asp:Label runat="server" ID="lblNotesValueCharCount" SkinID="MessageLabelSkin"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Label runat="server" ID="lblMessageHeader" SkinID="MessageLabelSkin"></asp:Label>
                        <table width="98%" border="0">
                            <tr>
                                <td valign="top">
                                    <asp:RadioButtonList runat="server" ID="rbCopyInformationType" RepeatDirection="horizontal"
                                        Visible="false">
                                        <asp:ListItem Text="Replicate using Formula Information Exactly <br>(Each list of items will reflect the formula as defined on the formula maintenance page.)"
                                            Value="Formula" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Replicate using Formula Information but with some Cost Sheet Data as well. <br>(The newest rates, crew sizes, etc. will be refreshed from the maintenance pages. If the value on the maintenance page is 0, then it will be copied from this cost sheet.)"
                                            Value="CostSheet"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:RadioButtonList runat="server" ID="rbCostStatusType" RepeatDirection="horizontal"
                                        Visible="false">
                                        <asp:ListItem Text="Replicate as Pending Quote" Value="Pending" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Replicate as Proposal" Value="Proposal"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="white-space: nowrap" colspan="2">
                                    <asp:Button ID="btnSave" runat="server" Width="90" Text="Save" Visible="false" CausesValidation="true"
                                        ValidationGroup="vgSave"></asp:Button>
                                    <asp:Button ID="btnDelete" runat="server" Width="90" Text="Delete" Visible="false">
                                    </asp:Button>
                                    <asp:Button ID="btnCopy" runat="server" Width="90" Text="Replicate" Visible="false"
                                        ValidationGroup="vgSave"></asp:Button>
                                    <asp:Button ID="btnPreApprovalNotification" runat="server" Width="150" Text="PRE-Approval Notify"
                                        Visible="false" />
                                    <asp:Button ID="btnPostApprovalNotification" runat="server" Width="150" Text="POST-Approval Notify"
                                        Visible="false" />
                                    <asp:Button ID="btnEdit" runat="server" Width="150" Text="Edit" Visible="false" />
                                </td>
                            </tr>
                        </table>
                    </Content>
                </ajax:AccordionPane>
            </Panes>
        </ajax:Accordion>
        <ajax:Accordion ID="accCostCustomerProgram" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
            FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
            RequireOpenedPane="false" SuppressHeaderPostbacks="true">
            <Panes>
                <ajax:AccordionPane ID="apCostCustomerProgram" runat="server">
                    <Header>
                        <a href="" class="accordionLink">Program / Customer </a>
                    </Header>
                    <Content>
                        <table>
                            <tr>
                                <td class="p_text">
                                    <asp:Label runat="server" ID="lblOldModelLabel" Text="Model:" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblOldModelValue" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    <asp:Label runat="server" ID="lblOldMakeLabel" Text="Make:" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblOldMakeValue" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    <asp:Label runat="server" ID="lblOldYearLabel" Text="Year:" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblOldYearValue" Visible="false"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <asp:ValidationSummary ID="vsCustomerProgram" runat="server" DisplayMode="List" ShowMessageBox="true"
                            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgCustomerProgram" />
                        <table width="98%">
                            <tr>
                                <td>
                                    <table runat="server" id="tblMakes" visible="false">
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblMake" Text="Make:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddMakes" runat="server" />
                                            </td>
                                            <td class="p_text">
                                                <asp:Label ID="lblProgramMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                                    Text="*" />
                                                <asp:Label runat="server" ID="lblProgram" Text="Program:"></asp:Label>
                                            </td>
                                            <td colspan="3" style="white-space: nowrap">
                                                <asp:DropDownList ID="ddProgram" runat="server">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvProgram" runat="server" ControlToValidate="ddProgram"
                                                    ErrorMessage="Program is required." Font-Bold="True" ValidationGroup="vgCustomerProgram"
                                                    Text="<" SetFocusOnError="true" />
                                                <%-- <asp:ImageButton ID="iBtnPreviewDetail" runat="server" ImageUrl="~/images/PreviewUp.jpg"
                                                    ToolTip="Review Program Detail" Visible="false" />--%>
                                                <br />
                                                {Program / Platform / Assembly Plant}
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblModel" Text="Model:"></asp:Label>
                                            </td>
                                            <td style="font-size: smaller">
                                                <asp:DropDownList ID="ddModel" runat="server" />
                                            </td>
                                            <td class="p_text">
                                                <asp:Label ID="lblYearMarker" runat="server" Font-Bold="True" ForeColor="Red" Text="*" />
                                                <asp:Label runat="server" ID="lblYear" Text="Year:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddYear" runat="server">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvYear" runat="server" ControlToValidate="ddYear"
                                                    ErrorMessage="Year is required." Font-Bold="True" ValidationGroup="vgCustomerProgram"
                                                    Text="<" SetFocusOnError="true" />
                                            </td>
                                        </tr>
                                        <%-- <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblCustomer" Text="Customer:"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="ddCustomer" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblSOPDate" Text="Program SOP Date:" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="txtSOPDate" runat="server" Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblEOPDate" Text="Program EOP Date:" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="txtEOPDate" runat="server" Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button runat="Server" ID="btnAddToCustomerProgram" Text="Add Program / Customer"
                                        ValidationGroup="vgCustomerProgram" Visible="false" />
                                    <asp:Button runat="Server" ID="btnCancelEditCustomerProgram" Text="Cancel Edit Program / Customer"
                                        CausesValidation="false" Visible="false" />
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label runat="server" ID="lblCustomerProgram" SkinID="MessageLabelSkin"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <ajax:CascadingDropDown ID="cddMakes" runat="server" TargetControlID="ddMakes" Category="Make"
                                        PromptText="Please select a Make." LoadingText="[Loading Makes...]" ServicePath="~/WS/VehicleCDDService.asmx"
                                        ServiceMethod="GetMakes" />
                                    <ajax:CascadingDropDown ID="cddModel" runat="server" TargetControlID="ddModel" ParentControlID="ddMakes"
                                        Category="Model" PromptText="Please select a Model." LoadingText="[Loading Models...]"
                                        ServicePath="~/WS/VehicleCDDService.asmx" ServiceMethod="GetModelMaint" />
                                    <ajax:CascadingDropDown ID="cddProgram" runat="server" TargetControlID="ddProgram"
                                        ParentControlID="ddModel" Category="Program" PromptText="Please select a Program."
                                        LoadingText="[Loading Programs...]" ServicePath="~/WS/VehicleCDDService.asmx"
                                        ServiceMethod="GetProgramsPlatformAssembly" />
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="gvCustomerProgram" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                            AllowPaging="True" PageSize="15" DataKeyNames="RowID" DataSourceID="odsCustomerProgram"
                            EmptyDataText="No customers or programs found" Width="600px" ShowFooter="False">
                            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#CCCCCC" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="RowID" HeaderText="RowID" SortExpression="RowID" />
                                <asp:BoundField DataField="CostSheetID" HeaderText="CostSheetID" SortExpression="CostSheetID" />
                                <%-- <asp:BoundField DataField="Make" HeaderText="Make" SortExpression="Make" />
                                <asp:BoundField DataField="ddModel" HeaderText="Model" SortExpression="ddModel" />--%>
                                <asp:BoundField DataField="ddCustomerDesc" HeaderText="Customer" SortExpression="ddCustomerDesc"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ddProgramName" HeaderText="Program / Make / Model / Platform"
                                    SortExpression="ProgramName" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ProgramYear" HeaderText="Year" SortExpression="ProgramYear"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="iBtnCustomerProgramDelete" runat="server" CausesValidation="False"
                                            CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsCustomerProgram" runat="server" OldValuesParameterFormatString="original_{0}"
                            DeleteMethod="DeleteCostSheetCustomerProgram" SelectMethod="GetCostSheetCustomerProgram"
                            TypeName="CostSheetCustomerProgramBLL">
                            <DeleteParameters>
                                <asp:Parameter Name="RowID" Type="Int32" />
                                <asp:Parameter Name="original_RowID" Type="Int32" />
                            </DeleteParameters>
                            <SelectParameters>
                                <asp:QueryStringParameter Name="CostSheetID" QueryStringField="CostSheetID" Type="Int32" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </Content>
                </ajax:AccordionPane>
            </Panes>
        </ajax:Accordion>
        <ajax:Accordion ID="accCostCalculations" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
            FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
            RequireOpenedPane="false" SuppressHeaderPostbacks="true" Visible="false">
            <Panes>
                <ajax:AccordionPane ID="apCostCalculations" runat="server">
                    <Header>
                        <a href="" class="accordionLink">Cost Sheet Calculation Factors</a></Header>
                    <Content>
                        <asp:Menu ID="menuCostSheetTopTabs" Height="30px" runat="server" Orientation="Horizontal"
                            StaticEnableDefaultPopOutImage="False" StaticMenuItemStyle-CssClass="tab" StaticSelectedStyle-CssClass="selectedTab"
                            CssClass="tabs" StaticDisplayLevels="2">
                            <Items>
                                <asp:MenuItem Text="Part Specification" Value="0"></asp:MenuItem>
                                <asp:MenuItem Text="Additional Offline Rates" Value="1"></asp:MenuItem>
                                <asp:MenuItem Text="Production Rates" Value="2"></asp:MenuItem>
                                <asp:MenuItem Text="Quote Info" Value="3"></asp:MenuItem>
                                <asp:MenuItem Text="Materials" Value="4"></asp:MenuItem>
                                <asp:MenuItem Text="Packaging" Value="5"></asp:MenuItem>
                                <asp:MenuItem Text="Labor" Value="6"></asp:MenuItem>
                            </Items>
                            <StaticSelectedStyle CssClass="selectedTab" />
                            <StaticMenuItemStyle CssClass="tab" />
                        </asp:Menu>
                        <asp:Menu ID="menuCostSheetBottomTabs" Height="30px" runat="server" Orientation="Horizontal"
                            StaticEnableDefaultPopOutImage="False" StaticMenuItemStyle-CssClass="tab" StaticSelectedStyle-CssClass="selectedTab"
                            CssClass="tabs" StaticDisplayLevels="2">
                            <Items>
                                <asp:MenuItem Text="Overhead" Value="7"></asp:MenuItem>
                                <asp:MenuItem Text="Misc Costs" Value="8"></asp:MenuItem>
                                <asp:MenuItem Text="Drawings" Value="9"></asp:MenuItem>
                                <asp:MenuItem Text="Composite Part Spec." Value="10"></asp:MenuItem>
                                <asp:MenuItem Text="Molded Barrier" Value="11"></asp:MenuItem>
                                <asp:MenuItem Text="Capital" Value="12"></asp:MenuItem>
                                <asp:MenuItem Text="Assumptions" Value="13"></asp:MenuItem>
                            </Items>
                        </asp:Menu>
                        <table width="98%" border="0">
                            <tr>
                                <td>
                                    <asp:MultiView ID="mvBuildCostSheet" runat="server" Visible="true" ActiveViewIndex="0"
                                        EnableViewState="true">
                                        <asp:View ID="vPartSpecifications" runat="server">
                                            <asp:Label runat="server" ID="lblMessagePartSpecifications" SkinID="MessageLabelSkin"></asp:Label><br />
                                            <asp:Label runat="server" ID="lblPartSpecificationsTip" SkinID="MessageLabelSkin"
                                                Text="A yellow background indicates a difference from the value in the formula."></asp:Label><br />
                                            <table>
                                                <tr>
                                                    <td class="p_text">
                                                        <asp:Label runat="server" ID="lblPartSpecificationsFormulaLabel" Text="Formula:"
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddPartSpecificationsFormulaValue" runat="server" Enabled="false"
                                                            AutoPostBack="true" Visible="false" ValidationGroup="vgSave">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        &nbsp
                                                    </td>
                                                    <td class="p_text">
                                                        <asp:Label runat="server" ID="lblPartSpecificationsPiecesPerCycleLabel" Text="Pieces Per Cycle:"
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtPartSpecificationsPiecesPerCycleValue" MaxLength="10"
                                                            Visible="false" Enabled="false" Width="75px"></asp:TextBox>
                                                        <asp:CompareValidator runat="server" ID="cvPartSpecificationsPiecesPerCycle" Operator="DataTypeCheck"
                                                            ValidationGroup="vgSave" Type="integer" Text="<" ControlToValidate="txtPartSpecificationsPiecesPerCycleValue"
                                                            ErrorMessage="Pieces per cycle must be an integer." SetFocusOnError="True" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        <asp:Label runat="server" ID="lblPartSpecificationsIsDiecutLabel" Text="Diecut" Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" ID="cbPartSpecificationsIsDiecutValue" Visible="false"
                                                            AutoPostBack="true" Enabled="false" />
                                                    </td>
                                                    <td>
                                                        &nbsp
                                                    </td>
                                                    <td class="p_text">
                                                        <asp:Label runat="server" ID="lblPartSpecificationsPiecesCaughtTogetherLabel" Text="Pieces Caught Together:"
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtPartSpecificationsPiecesCaughtTogetherValue" MaxLength="10"
                                                            Width="75px" Visible="false" Enabled="false"></asp:TextBox>
                                                        <asp:CompareValidator runat="server" ID="cvPartSpecificationsPiecesCaughtTogetherValue"
                                                            Operator="DataTypeCheck" ValidationGroup="vgSave" Type="integer" Text="<" ControlToValidate="txtPartSpecificationsPiecesCaughtTogetherValue"
                                                            ErrorMessage="Pieces caught together must be an integer." SetFocusOnError="True" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        <asp:Label runat="server" ID="lblPartSpecificationsThicknessLabel" Text="Thickness:"
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox runat="server" ID="txtPartSpecificationsThicknessValue" MaxLength="10"
                                                            Width="75px" Visible="false" Enabled="false"></asp:TextBox>
                                                        <asp:CompareValidator runat="server" ID="cvPartSpecificationsThicknessValue" Operator="DataTypeCheck"
                                                            ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtPartSpecificationsThicknessValue"
                                                            ErrorMessage="Thickness must be a number." SetFocusOnError="True" />
                                                        &nbsp;
                                                        <asp:DropDownList runat="server" ID="ddPartSpecificationsThicknessUnits" Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="p_text">
                                                        <asp:Label runat="server" ID="lblPartSpecificationsIsSideBySideLabel" Text="Side By Side"
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" ID="cbPartSpecificationsIsSideBySideValue" Visible="false"
                                                            Enabled="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        <asp:Label runat="server" ID="lblPartSpecificationsIsCompletedOfflineLabel" Text="Completed Offline"
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" ID="cbPartSpecificationsIsCompletedOfflineValue" Visible="false"
                                                            Enabled="false" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td class="p_text">
                                                        <asp:Label runat="server" ID="lblPartSpecificationsCalculatedAreaLabel" Text="Calculated Area:"
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox runat="server" ID="txtPartSpecificationsCalculatedAreaValue" MaxLength="10"
                                                            Width="75px" Visible="false" Enabled="false"></asp:TextBox>
                                                        <asp:CompareValidator runat="server" ID="cvPartSpecificationsCalculatedAreaValue"
                                                            Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtPartSpecificationsCalculatedAreaValue"
                                                            ErrorMessage="Calculated area value must be a number." SetFocusOnError="True" />
                                                        &nbsp;
                                                        <asp:DropDownList runat="server" ID="ddPartSpecificationsCalculatedAreaUnits" Enabled="false"
                                                            Visible="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        <asp:Label runat="server" ID="lblPartSpecificationsOffLineRateLabel" Text="Off Line Rate:"
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtPartSpecificationsOffLineRateValue" MaxLength="10"
                                                            Width="75px" Visible="false" Enabled="false"></asp:TextBox>
                                                        <asp:CompareValidator runat="server" ID="cvPartSpecificationsOffLineRateValue" Operator="DataTypeCheck"
                                                            ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtPartSpecificationsOffLineRateValue"
                                                            ErrorMessage="Offline rate must be a number." SetFocusOnError="True" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td class="p_text">
                                                        <asp:Label runat="server" ID="lblPartSpecificationsChangedAreaLabel" Text="Changed Area:"
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox runat="server" ID="txtPartSpecificationsChangedAreaValue" MaxLength="10"
                                                            Width="75px" Visible="false" Enabled="false"></asp:TextBox>
                                                        <asp:CompareValidator runat="server" ID="cvPartSpecificationsChangedArea" Operator="DataTypeCheck"
                                                            ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtPartSpecificationsChangedAreaValue"
                                                            ErrorMessage="Changed area value must be a number." SetFocusOnError="True" />
                                                        &nbsp;
                                                        <asp:DropDownList runat="server" ID="ddPartSpecificationsChangedAreaUnits" Enabled="false"
                                                            Visible="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        <asp:Label runat="server" ID="lblPartSpecificationsNumberOfHolesLabel" Text="Number of Holes:"
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtPartSpecificationsNumberOfHolesValue" MaxLength="10"
                                                            Width="75px" Visible="false" Enabled="false"></asp:TextBox>
                                                        <asp:CompareValidator runat="server" ID="cvPartSpecificationsNumberOfHolesValue"
                                                            Operator="DataTypeCheck" ValidationGroup="vgSave" Type="integer" Text="<" ControlToValidate="txtPartSpecificationsNumberOfHolesValue"
                                                            ErrorMessage="Number of holes value must be an integer." SetFocusOnError="True" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td class="p_text">
                                                        <asp:Label runat="server" ID="lblPartSpecificationsDieLayoutWidthLabel" Text="Die Layout Width:"
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox runat="server" ID="txtPartSpecificationsDieLayoutWidthValue" MaxLength="10"
                                                            Width="75px" Visible="false" Enabled="false"></asp:TextBox>
                                                        <asp:CompareValidator runat="server" ID="cvPartSpecificationsDieLayoutWidthValue"
                                                            Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtPartSpecificationsDieLayoutWidthValue"
                                                            ErrorMessage="Die layout width must be a number." SetFocusOnError="True" />
                                                        &nbsp;
                                                        <asp:DropDownList runat="server" ID="ddPartSpecificationsDieLayoutWidthUnits" Enabled="false"
                                                            Visible="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        <asp:Label runat="server" ID="lblPartSpecificationsPartWidthLabel" Text="Part Width:"
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox runat="server" ID="txtPartSpecificationsPartWidthValue" MaxLength="10"
                                                            Width="75px" Visible="false" Enabled="false"></asp:TextBox>
                                                        <asp:CompareValidator runat="server" ID="cvPartSpecificationsPartWidth" Operator="DataTypeCheck"
                                                            ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtPartSpecificationsPartWidthValue"
                                                            ErrorMessage="Part width must be a number." SetFocusOnError="True" />
                                                        &nbsp;
                                                        <asp:DropDownList runat="server" ID="ddPartSpecificationsPartWidthUnits" Visible="false"
                                                            Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="p_text">
                                                        <asp:Label runat="server" ID="lblPartSpecificationsDieLayoutTravelLabel" Text="Die Layout Travel:"
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox runat="server" ID="txtPartSpecificationsDieLayoutTravelValue" MaxLength="10"
                                                            Width="75px" Visible="false" Enabled="false"></asp:TextBox>
                                                        <asp:CompareValidator runat="server" ID="cvPartSpecificationsDieLayoutTravel" Operator="DataTypeCheck"
                                                            ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtPartSpecificationsDieLayoutTravelValue"
                                                            ErrorMessage="Die layout travel must be a number." SetFocusOnError="True" />
                                                        &nbsp;
                                                        <asp:DropDownList runat="server" ID="ddPartSpecificationsDieLayoutTravelUnits" Enabled="false"
                                                            Visible="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        <asp:Label runat="server" ID="lblPartSpecificationsPartLengthLabel" Text="Part Length:"
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox runat="server" ID="txtPartSpecificationsPartLengthValue" MaxLength="10"
                                                            Width="75px" Visible="false" Enabled="false"></asp:TextBox>
                                                        <asp:CompareValidator runat="server" ID="cvPartSpecificationsPartLength" Operator="DataTypeCheck"
                                                            ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtPartSpecificationsPartLengthValue"
                                                            ErrorMessage="Part length must be a number." SetFocusOnError="True" />
                                                        &nbsp;
                                                        <asp:DropDownList runat="server" ID="ddPartSpecificationsPartLengthUnits" Visible="false"
                                                            Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="p_text">
                                                        <asp:Label runat="server" ID="lblPartSpecificationsWeightPerAreaLabel" Text="Weight Per Area:"
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox runat="server" ID="txtPartSpecificationsWeightPerAreaValue" MaxLength="10"
                                                            Width="75px" Visible="false" Enabled="false"></asp:TextBox>
                                                        <asp:CompareValidator runat="server" ID="cvPartSpecificationsWeightPerAreaValue"
                                                            Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtPartSpecificationsWeightPerAreaValue"
                                                            ErrorMessage="Weight per area must be a number." SetFocusOnError="True" />
                                                        &nbsp;
                                                        <asp:DropDownList runat="server" ID="ddPartSpecificationsWeightPerAreaUnits" Visible="false"
                                                            Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        <asp:Label runat="server" ID="lblPartSpecificationsConfigurationFactorLabel" Text="Configuration Factor:"
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtPartSpecificationsConfigurationFactorValue" MaxLength="10"
                                                            Width="75px" Visible="false" Enabled="false"></asp:TextBox>
                                                        <asp:CompareValidator runat="server" ID="cvPartSpecificationsConfigurationFactorValue"
                                                            Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtPartSpecificationsConfigurationFactorValue"
                                                            ErrorMessage="Configuration factor must be a number." SetFocusOnError="True" />
                                                        <asp:Label runat="server" ID="lblPartSpecificationsConfigurationFactorPercentageValue"
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td class="p_text">
                                                        <asp:Label runat="server" ID="lblPartSpecificationsSpecificGravityLabel" Text="Specific Gravity:"
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox runat="server" ID="txtPartSpecificationsSpecificGravityValue" MaxLength="10"
                                                            Width="75px" Visible="false" Enabled="false"></asp:TextBox>
                                                        <asp:CompareValidator runat="server" ID="cvPartSpecificationsSpecificGravity" Operator="DataTypeCheck"
                                                            ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtPartSpecificationsSpecificGravityValue"
                                                            ErrorMessage="Specific gravity must be a number." SetFocusOnError="True" />
                                                        &nbsp;
                                                        <asp:DropDownList runat="server" ID="ddPartSpecificationsSpecificGravityUnits" Enabled="false"
                                                            Visible="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        <asp:Label runat="server" ID="lblPartSpecificationsProcessLabel" Text="Process:"
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddPartSpecificationsProcessValue" runat="server" Visible="false"
                                                            Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td class="p_text">
                                                        <asp:Label runat="server" ID="lblPartSpecificationsDepartmentLabel" Text="Department:"
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddPartSpecificationsDepartmentValue" runat="server" Visible="false"
                                                            Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        <asp:Label ID="lblPartSpecificationsRepackMaterialLabel" runat="server" Text="Repack Material:"
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                    <td colspan="4">
                                                        <asp:TextBox runat="server" ID="txtPartSpecificationsRepackMaterialValue" TextMode="MultiLine"
                                                            Visible="false" Enabled="false" Height="60px" Width="78%"></asp:TextBox>
                                                        <br />
                                                        <asp:Label runat="server" ID="lblPartSpecificationsRepackMaterialValueCharCount"
                                                            SkinID="MessageLabelSkin"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        <asp:Label ID="lblPartSpecificationsApproxWeightLabel" runat="server" Text="Approx Weight:"
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                    <td colspan="4">
                                                        <asp:TextBox runat="server" ID="txtPartSpecificationsApproxWeightValue" MaxLength="10"
                                                            Width="75px" Visible="false" Enabled="false"></asp:TextBox>
                                                        <asp:CompareValidator runat="server" ID="cvPartSpecificationsApproxWeight" Operator="DataTypeCheck"
                                                            ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtPartSpecificationsApproxWeightValue"
                                                            ErrorMessage="Approx weight must be a number." SetFocusOnError="True" />
                                                        &nbsp;
                                                        <asp:DropDownList runat="server" ID="ddPartSpecificationsApproxWeightUnits" Visible="false"
                                                            Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        <asp:Label ID="lblPartSpecificationsProductionRateLabel" runat="server" Text="Production Rate:"
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtPartSpecificationsProductionRateValue" MaxLength="10"
                                                            Width="75px" Visible="false" Enabled="false"></asp:TextBox>
                                                        <asp:CompareValidator runat="server" ID="cvPartSpecificationsProductionRate" Operator="DataTypeCheck"
                                                            ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtPartSpecificationsProductionRateValue"
                                                            ErrorMessage="Production rate must be a number." SetFocusOnError="True" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td class="p_text">
                                                        <asp:Label ID="txtPartSpecificationsNumberofCarriersLabel" runat="server" Text="Number of Carriers:"
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtPartSpecificationsNumberOfCarriersValue" MaxLength="10"
                                                            Width="75px" Visible="false" Enabled="false"></asp:TextBox>
                                                        <asp:CompareValidator runat="server" ID="cvPartSpecificationsNumberOfCarriers" Operator="DataTypeCheck"
                                                            ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtPartSpecificationsNumberOfCarriersValue"
                                                            ErrorMessage="Number of carriers must be a number." SetFocusOnError="True" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        <asp:Label ID="lblPartSpecificationsFoamLabel" runat="server" Text="Foam:" Visible="false"></asp:Label>
                                                    </td>
                                                    <td colspan="4">
                                                        <asp:TextBox runat="server" ID="txtPartSpecificationsFoamValue" MaxLength="10" Visible="false"
                                                            Width="75px" Enabled="false"></asp:TextBox>
                                                        <asp:CompareValidator runat="server" ID="cvPartSpecificationsFoamValue" Operator="DataTypeCheck"
                                                            ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtPartSpecificationsFoamValue"
                                                            ErrorMessage="Foam value must be a number." SetFocusOnError="True" />
                                                        &nbsp;
                                                        <asp:DropDownList runat="server" ID="ddPartSpecificationsFoamUnits" Visible="false"
                                                            Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5">
                                                        <asp:ValidationSummary ID="vsFooterDepartment" runat="server" ShowMessageBox="True"
                                                            ShowSummary="true" ValidationGroup="vgFooterDepartment" />
                                                        <asp:ValidationSummary ID="vsEditDepartment" runat="server" ShowMessageBox="True"
                                                            ShowSummary="true" ValidationGroup="vgEditDepartment" />
                                                        <br />
                                                        <asp:GridView runat="server" ID="gvDepartment" Width="100%" DataSourceID="odsCostSheetDepartment"
                                                            AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" PageSize="5"
                                                            ShowFooter="True" DataKeyNames="RowID">
                                                            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                                            <EditRowStyle BackColor="#CCCCCC" />
                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                            <Columns>
                                                                <asp:BoundField DataField="RowID" SortExpression="RowID" />
                                                                <asp:BoundField DataField="CostSheetID" SortExpression="CostSheetID" />
                                                                <asp:TemplateField HeaderText="Department(s)" SortExpression="ddDepartmentName">
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddEditDepartment" runat="server" DataSource='<%# CostingModule.GetCostingDepartmentList("","",False) %>'
                                                                            DataValueField="DepartmentID" DataTextField="ddDepartmentName" AppendDataBoundItems="True"
                                                                            SelectedValue='<%# Bind("DepartmentID") %>'>
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvEditDepartment" runat="server" ControlToValidate="ddEditDepartment"
                                                                            ErrorMessage="The department is required." Font-Bold="True" ValidationGroup="vgEditDepartment"
                                                                            Text="<" SetFocusOnError="true" />
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblViewDepartmentName" runat="server" Text='<%# Bind("ddDepartmentName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddFooterDepartment" runat="server" DataSource='<%# CostingModule.GetCostingDepartmentList("","",False) %>'
                                                                            DataValueField="DepartmentID" DataTextField="ddDepartmentName" AppendDataBoundItems="True">
                                                                            <asp:ListItem Text="" Value="0"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvFooterDepartment" runat="server" ControlToValidate="ddFooterDepartment"
                                                                            ErrorMessage="The department is required." Font-Bold="True" ValidationGroup="vgFooterDepartment"
                                                                            Text="<" SetFocusOnError="true" />
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <EditItemTemplate>
                                                                        <asp:ImageButton ID="iBtnDepartmentUpdate" runat="server" CausesValidation="True"
                                                                            CommandName="Update" ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditDepartment" />
                                                                        <asp:ImageButton ID="iBtnDepartmentCancel" runat="server" CausesValidation="False"
                                                                            CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="iBtnDepartmentEdit" runat="server" CausesValidation="False"
                                                                            CommandName="Edit" ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                                                                        <asp:ImageButton ID="ibtnDepartmentDelete" runat="server" CausesValidation="False"
                                                                            CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgFooterDepartment"
                                                                            runat="server" ID="iBtnFooterDepartment" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                                                                        <asp:ImageButton ID="iBtnDepartmentUndo" runat="server" CommandName="Undo" CausesValidation="false"
                                                                            ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                        <asp:ObjectDataSource ID="odsCostSheetDepartment" runat="server" OldValuesParameterFormatString="original_{0}"
                                                            SelectMethod="GetCostSheetDepartment" TypeName="CostSheetDepartmentBLL" DeleteMethod="DeleteCostSheetDepartment"
                                                            UpdateMethod="UpdateCostSheetDepartment" InsertMethod="InsertCostSheetDepartment">
                                                            <SelectParameters>
                                                                <asp:QueryStringParameter Name="CostSheetID" QueryStringField="CostSheetID" Type="Int32" />
                                                            </SelectParameters>
                                                            <DeleteParameters>
                                                                <asp:Parameter Name="RowID" Type="Int32" />
                                                                <asp:Parameter Name="original_RowID" Type="Int32" />
                                                            </DeleteParameters>
                                                            <UpdateParameters>
                                                                <asp:Parameter Name="DepartmentID" Type="Int32" />
                                                                <asp:Parameter Name="original_RowID" Type="Int32" />
                                                                <asp:Parameter Name="CostSheetID" Type="Int32" />
                                                            </UpdateParameters>
                                                            <InsertParameters>
                                                                <asp:Parameter Name="CostSheetID" Type="Int32" />
                                                                <asp:Parameter Name="DepartmentID" Type="Int32" />
                                                            </InsertParameters>
                                                        </asp:ObjectDataSource>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:View>
                                        <asp:View ID="vAdditionalOfflineRates" runat="server">
                                            <asp:Label runat="server" ID="lblMessageAdditionalOfflineRates"></asp:Label>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:ValidationSummary ID="vsFooterAdditionalOfflineRate" runat="server" ShowMessageBox="True"
                                                            ShowSummary="true" ValidationGroup="vgFooterAdditionalOfflineRateInfo" />
                                                        <asp:ValidationSummary ID="vsEditAdditionalOfflineRate" runat="server" ShowMessageBox="True"
                                                            ShowSummary="true" ValidationGroup="vgEditAdditionalOfflineRateInfo" />
                                                        <br />
                                                        <asp:Button runat="server" ID="btnRemoveAdditionalOfflineRate" Text="Remove All Addition Offline Rates"
                                                            Visible="false" />
                                                        <asp:GridView ID="gvAdditionalOfflineRate" runat="server" AllowPaging="True" AllowSorting="True"
                                                            AutoGenerateColumns="False" DataSourceID="odsCostSheetAdditionalOfflineRate"
                                                            DataKeyNames="RowID" Width="100%" PageSize="15" ShowFooter="True">
                                                            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                                            <EditRowStyle BackColor="#CCCCCC" />
                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                            <Columns>
                                                                <asp:BoundField DataField="CostSheetID" SortExpression="CostSheetID" ReadOnly="True" />
                                                                <asp:BoundField DataField="LaborID" SortExpression="LaborID" ReadOnly="True" />
                                                                <asp:TemplateField HeaderText="Description" SortExpression="LaborID">
                                                                    <EditItemTemplate>
                                                                        <asp:Label ID="lblEditAdditionalOfflineRateLaborItemID" runat="server" CssClass="none"
                                                                            Text='<%# Bind("LaborID") %>'></asp:Label>
                                                                        <asp:Label ID="lblEditAdditionalOfflineRateLaborItemLongDesc" runat="server" Text='<%# Bind("ddLongLaborDesc") %>'></asp:Label>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblViewAdditionalOfflineRateLaborItemID" runat="server" CssClass="none"
                                                                            Text='<%# Bind("LaborID") %>'></asp:Label>
                                                                        <asp:Label ID="lblViewAdditionalOfflineRateLaborItemLongDesc" runat="server" Text='<%# Bind("ddLongLaborDesc") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddFooterAdditionalOfflineRateLaborItem" runat="server" DataSource='<%# CostingModule.GetLabor(0,"",True,True) %>'
                                                                            DataValueField="LaborID" DataTextField="ddLongLaborDesc" AppendDataBoundItems="True"
                                                                            SelectedValue='<%# Bind("LaborID") %>'>
                                                                            <asp:ListItem Text="" Value="0"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvFooterAdditionalOfflineRateLaborItem" runat="server"
                                                                            ControlToValidate="ddFooterAdditionalOfflineRateLaborItem" ErrorMessage="The description is required."
                                                                            Font-Bold="True" ValidationGroup="vgFooterAdditionalOfflineRateInfo" Text="<"
                                                                            SetFocusOnError="true" />
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Pieces / Hr" SortExpression="PiecesPerHour">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtEditAdditionalOfflineRatePiecesPerHour" runat="server" MaxLength="10"
                                                                            Text='<%# Bind("PiecesPerHour") %>'></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="rfvAdditionalOfflineRatePiecesPerHourEdit" runat="server"
                                                                            ControlToValidate="txtEditAdditionalOfflineRatePiecesPerHour" ErrorMessage="Pieces per hour is required."
                                                                            Font-Bold="True" ValidationGroup="vgEditAdditionalOfflineRateInfo" Text="<" SetFocusOnError="true"> </asp:RequiredFieldValidator>
                                                                        <asp:CompareValidator runat="server" ID="cvPiecesPerHourEdit" Operator="DataTypeCheck"
                                                                            ValidationGroup="vgEditAdditionalOfflineRateInfo" Type="Double" Text="<" ControlToValidate="txtEditAdditionalOfflineRatePiecesPerHour"
                                                                            ErrorMessage="Pieces per hour must be a number." SetFocusOnError="True" />
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblViewAdditionalOfflineRatePiecesPerHour" runat="server" Text='<%# Bind("PiecesPerHour") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtFooterAdditionalOfflineRatePiecesPerHour" runat="server" MaxLength="10"
                                                                            Text='<%# Bind("PiecesPerHour") %>'></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="rfvFooterAdditionalOfflineRatePiecesPerHourFooter"
                                                                            runat="server" ControlToValidate="txtFooterAdditionalOfflineRatePiecesPerHour"
                                                                            ErrorMessage="Pieces per hour is required." Font-Bold="True" ValidationGroup="vgFooterAdditionalOfflineRateInfo"
                                                                            Text="<" SetFocusOnError="true"> </asp:RequiredFieldValidator>
                                                                        <asp:CompareValidator runat="server" ID="cvtxtFooterAdditionalOfflineRatePiecesPerHourPiecesPerHourFooter"
                                                                            Operator="DataTypeCheck" ValidationGroup="vgFooterAdditionalOfflineRateInfo"
                                                                            Type="Double" Text="<" ControlToValidate="txtFooterAdditionalOfflineRatePiecesPerHour"
                                                                            ErrorMessage="Pieces per hour must be a number." SetFocusOnError="True" />
                                                                    </FooterTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Ordinal" SortExpression="Ordinal">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtEditAdditionalOfflineRateOrdinal" runat="server" MaxLength="10"
                                                                            Text='<%# Bind("Ordinal") %>'></asp:TextBox>
                                                                        <asp:CompareValidator runat="server" ID="cvEditAdditionalOfflineRateOrdinal" Operator="DataTypeCheck"
                                                                            ValidationGroup="vgEditAdditionalOfflineRateInfo" Type="Integer" Text="<" ControlToValidate="txtEditAdditionalOfflineRateOrdinal"
                                                                            ErrorMessage="Ordinal must be an integer." SetFocusOnError="True" />
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblViewAdditionalOfflineRateOrdinal" runat="server" Text='<%# Bind("Ordinal") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtFooterAdditionalOfflineRateOrdinal" runat="server" MaxLength="10"
                                                                            Text="99"></asp:TextBox>
                                                                        <asp:CompareValidator runat="server" ID="cvFooterAdditionalOfflineRateOrdinal" Operator="DataTypeCheck"
                                                                            ValidationGroup="vgFooterAdditionalOfflineRateInfo" Type="Integer" Text="<" ControlToValidate="txtFooterAdditionalOfflineRateOrdinal"
                                                                            ErrorMessage="Ordinal must be an integer." SetFocusOnError="True" />
                                                                    </FooterTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ShowHeader="False">
                                                                    <EditItemTemplate>
                                                                        <asp:ImageButton ID="iBtnAdditionalOfflineRateUpdate" runat="server" CausesValidation="True"
                                                                            CommandName="Update" ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditAdditionalOfflineRateInfo" />
                                                                        <asp:ImageButton ID="iBtnAdditionalOfflineRateCancel" runat="server" CausesValidation="False"
                                                                            CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                                                                    </EditItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="iBtnAdditionalOfflineRateEdit" runat="server" CausesValidation="False"
                                                                            CommandName="Edit" ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                                                                        <asp:ImageButton ID="ibtnAdditionalOfflineRateDelete" runat="server" CausesValidation="False"
                                                                            CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgFooterAdditionalOfflineRateInfo"
                                                                            runat="server" ID="iBtnFooterAdditionalOfflineRate" ImageUrl="~/images/save.jpg"
                                                                            AlternateText="Insert" />
                                                                        <asp:ImageButton ID="iBtnAdditionalOfflineRateUndo" runat="server" CommandName="Undo"
                                                                            CausesValidation="false" ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                        <asp:ObjectDataSource ID="odsCostSheetAdditionalOfflineRate" runat="server" DeleteMethod="DeleteCostSheetAdditionalOfflineRates"
                                                            OldValuesParameterFormatString="original_{0}" SelectMethod="GetCostSheetAdditionalOfflineRate"
                                                            TypeName="CostSheetAdditionalOfflineRateBLL" UpdateMethod="UpdateCostSheetAdditionalOfflineRate"
                                                            InsertMethod="InsertCostSheetAdditionalOfflineRate">
                                                            <DeleteParameters>
                                                                <asp:Parameter Name="RowID" Type="Int32" />
                                                                <asp:Parameter Name="original_RowID" Type="Int32" />
                                                            </DeleteParameters>
                                                            <SelectParameters>
                                                                <asp:QueryStringParameter Name="CostSheetID" QueryStringField="CostSheetID" Type="Int32" />
                                                                <asp:Parameter DefaultValue="0" Name="LaborID" Type="Int32" />
                                                            </SelectParameters>
                                                            <UpdateParameters>
                                                                <asp:Parameter Name="PiecesPerHour" Type="Double" />
                                                                <asp:Parameter Name="Ordinal" Type="Int32" />
                                                                <asp:Parameter Name="original_RowID" Type="Int32" />
                                                                <asp:Parameter Name="original_LaborID" Type="Int32" />
                                                                <asp:Parameter Name="ddLongLaborDesc" Type="String" />
                                                                <asp:Parameter Name="LaborID" Type="Int32" />
                                                            </UpdateParameters>
                                                            <InsertParameters>
                                                                <asp:Parameter Name="CostSheetID" Type="Int32" />
                                                                <asp:Parameter Name="LaborID" Type="Int32" />
                                                                <asp:Parameter Name="PiecesPerHour" Type="Double" />
                                                                <asp:Parameter Name="Ordinal" Type="Int32" />
                                                            </InsertParameters>
                                                        </asp:ObjectDataSource>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:View>
                                        <asp:View ID="vProductionRates" runat="server">
                                            <table>
                                                <tr>
                                                    <td style="width: 50%; white-space: nowrap;">
                                                        <table border="1">
                                                            <tr>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td colspan="2" align="center">
                                                                                <b><u>Formula Values</u></b>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="p_text">
                                                                                <asp:Label runat="server" ID="lblProductionRatesMaxMixCapacityLabel" Text="Max Mix Capacity:"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" ID="txtProductionRatesMaxMixCapacityValue" MaxLength="10"
                                                                                    Enabled="false" Width="75px"></asp:TextBox>
                                                                                <asp:CompareValidator runat="server" ID="cvProductionRatesMaxMixCapacityValue" Operator="DataTypeCheck"
                                                                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtProductionRatesMaxMixCapacityValue"
                                                                                    ErrorMessage="Max Mix Capacity must be a number." SetFocusOnError="True" />
                                                                                &nbsp;
                                                                                <asp:DropDownList runat="server" ID="ddProductionRatesMaxMixCapacityUnits" Enabled="false">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="p_text">
                                                                                <asp:Label runat="server" ID="lblProductionRatesMaxFormingRateLabel" Text="Max Forming Rate:"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" ID="txtProductionRatesMaxFormingRateValue" MaxLength="10"
                                                                                    Enabled="false" Width="75px"></asp:TextBox>
                                                                                <asp:CompareValidator runat="server" ID="cvProductionRatesMaxFormingRate" Operator="DataTypeCheck"
                                                                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtProductionRatesMaxFormingRateValue"
                                                                                    ErrorMessage="Max Forming Rate must be a number." SetFocusOnError="True" />
                                                                                &nbsp;
                                                                                <asp:DropDownList runat="server" ID="ddProductionRatesMaxFormingRateUnits" Enabled="false">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="p_text">
                                                                                <asp:Label runat="server" ID="lblProductionRatesCatchingAbilityLabel" Text="Catching Ability:"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" ID="txtProductionRatesCatchingAbilityValue" MaxLength="10"
                                                                                    Enabled="false" Width="75px"></asp:TextBox>
                                                                                <asp:CompareValidator runat="server" ID="cvProductionRatesCatchingAbilityValue" Operator="DataTypeCheck"
                                                                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtProductionRatesCatchingAbilityValue"
                                                                                    ErrorMessage="Catching ability must be a number." SetFocusOnError="True" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="p_text">
                                                                                <asp:Label runat="server" ID="lblProductionRatesLineSpeedLimitationLabel" Text="Line Speed Limitation:"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" ID="txtProductionRatesLineSpeedLimitationValue" MaxLength="10"
                                                                                    Enabled="false" Width="75px"></asp:TextBox>
                                                                                <asp:CompareValidator runat="server" ID="cvProductionRatesLineSpeedLimitationValue"
                                                                                    Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtProductionRatesLineSpeedLimitationValue"
                                                                                    ErrorMessage="Line speed limitation must be a number." SetFocusOnError="True" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="p_text">
                                                                                <asp:Label runat="server" ID="lblProductionRatesCatchPercentLabel" Text="Catch Percent:"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" ID="txtProductionRatesCatchPercentValue" MaxLength="10"
                                                                                    Enabled="false" Width="75px"></asp:TextBox>
                                                                                <asp:CompareValidator runat="server" ID="cvProductionRatesCatchPercentValue" Operator="DataTypeCheck"
                                                                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtProductionRatesCatchPercentValue"
                                                                                    ErrorMessage="catch percent must be a number." SetFocusOnError="True" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="p_text">
                                                                                <asp:Label runat="server" ID="lblProductionRatesCoatingFactorLabel" Text="Coating Factor:"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox runat="server" ID="txtProductionRatesCoatingFactorValue" MaxLength="10"
                                                                                    Enabled="false" Width="75px"></asp:TextBox>
                                                                                <asp:CompareValidator runat="server" ID="cvProductionRatesCoatingFactorValue" Operator="DataTypeCheck"
                                                                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtProductionRatesCoatingFactorValue"
                                                                                    ErrorMessage="coating factor must be a number." SetFocusOnError="True" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="p_text">
                                                                                <asp:Label runat="server" ID="lblProductionRatesWeightPerAreaLabel" Text="Weight Per Area:"></asp:Label>
                                                                            </td>
                                                                            <td style="white-space: nowrap;">
                                                                                <asp:TextBox runat="server" ID="txtProductionRatesWeightPerAreaValue" MaxLength="10"
                                                                                    Enabled="false" Width="75px"></asp:TextBox>
                                                                                <asp:CompareValidator runat="server" ID="cvProductionRatesWeightPerAreaValue" Operator="DataTypeCheck"
                                                                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtProductionRatesWeightPerAreaValue"
                                                                                    ErrorMessage="Weight per area must be a number." SetFocusOnError="True" />
                                                                                &nbsp;
                                                                                <asp:DropDownList runat="server" ID="ddProductionRatesWeightPerAreaUnits" Enabled="false">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 50%" valign="top">
                                                        <asp:Label runat="server" ID="lblProductionLimitsMessage" Text="Production Limits Based on 100%"></asp:Label>
                                                        <asp:GridView ID="gvProductionLimit" runat="server" AutoGenerateColumns="False" DataSourceID="odsCostSheetProductionLimit"
                                                            AllowPaging="True" AllowSorting="True" PageSize="10" Width="80%">
                                                            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                                            <EditRowStyle BackColor="#CCCCCC" />
                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
                                                            <EmptyDataTemplate>
                                                                No Production Limits currently exist.
                                                            </EmptyDataTemplate>
                                                            <Columns>
                                                                <asp:BoundField DataField="CostSheetID" HeaderText="CostSheetID" SortExpression="CostSheetID"
                                                                    Visible="False" />
                                                                <asp:BoundField DataField="ProductionLimitID" HeaderText="ProductionLimitID" SortExpression="ProductionLimitID"
                                                                    Visible="False" />
                                                                <asp:BoundField DataField="ProductionLimitName" HeaderText="Description" SortExpression="ProductionLimitName" />
                                                                <asp:BoundField DataField="ProductionLimit" HeaderText="Production Limit" SortExpression="ProductionLimit">
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="UnitAbbr" HeaderText="Units" SortExpression="UnitAbbr">
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                            </Columns>
                                                        </asp:GridView>
                                                        <asp:ObjectDataSource ID="odsCostSheetProductionLimit" runat="server" SelectMethod="GetCostSheetProductionLimit"
                                                            TypeName="CostingModule" OldValuesParameterFormatString="original_{0}">
                                                            <SelectParameters>
                                                                <asp:QueryStringParameter Name="CostSheetID" QueryStringField="CostSheetID" Type="Int32" />
                                                            </SelectParameters>
                                                        </asp:ObjectDataSource>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td class="p_text">
                                                                    <asp:Label runat="server" ID="lblProductionRatesOfflineSpecificSheetsUpLabel" Text="Sheets Up:"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" ID="txtProductionRatesOfflineSpecificSheetsUpValue" Enabled="false"
                                                                        Width="75px"></asp:TextBox>
                                                                    <asp:CompareValidator runat="server" ID="cvProductionRatesOfflineSpecificSheetsUpValue"
                                                                        Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtProductionRatesOfflineSpecificSheetsUpValue"
                                                                        ErrorMessage="Offline specific sheets up must be a number." SetFocusOnError="True" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="p_text">
                                                                    <asp:Label runat="server" ID="lblProductionRatesOfflineSpecificBlankCodeLabel" Text="Blank Code:"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" ID="txtProductionRatesOfflineSpecificBlankCodeValue"
                                                                        Width="75px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="p_text">
                                                                    <asp:Label runat="server" ID="lblProductionRatesOfflineSpecificQuotedPressCyclesLabel"
                                                                        Text="Quoted Press Cycles:"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" ID="txtProductionRatesOfflineSpecificQuotedPressCyclesValue"
                                                                        Width="75px" Enabled="false"></asp:TextBox>
                                                                    <asp:CompareValidator runat="server" ID="cvProductionRatesOfflineSpecificQuotedPressCyclesValue"
                                                                        Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtProductionRatesOfflineSpecificQuotedPressCyclesValue"
                                                                        ErrorMessage="Offline specific quoted press cycles must be a number." SetFocusOnError="True" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="p_text">
                                                                    <asp:Label runat="server" ID="lblProductionRatesOfflineSpecificQuotedOffLineRateLabel"
                                                                        Text="Quoted Off Line Rate:"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" ID="txtProductionRatesOfflineSpecificQuotedOfflineRatesValue"
                                                                        Width="75px" Enabled="false"></asp:TextBox>
                                                                    <asp:CompareValidator runat="server" ID="cvProductionRatesOfflineSpecificQuotedOfflineRatesValue"
                                                                        Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtProductionRatesOfflineSpecificQuotedOfflineRatesValue"
                                                                        ErrorMessage="Offline specific quoted offline rate must be a number." SetFocusOnError="True" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="p_text">
                                                                    <asp:Label runat="server" ID="lblProductionRatesOfflineSpecificCrewSizeLabel" Text="Crew Size:"
                                                                        Visible="false"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" ID="txtProductionRatesOfflineSpecificCrewSizeValue" Enabled="false"
                                                                        Visible="false" Width="75px"></asp:TextBox>
                                                                    <asp:CompareValidator runat="server" ID="cvProductionRatesOfflineSpecificCrewSizeValue"
                                                                        Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtProductionRatesOfflineSpecificCrewSizeValue"
                                                                        ErrorMessage="Offline specific crew size must be a number." SetFocusOnError="True" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="p_text">
                                                                    <asp:Label runat="server" ID="lblProductionRatesOfflineSpecificPiecesManHourLabel"
                                                                        Text="Pieces/Man Hour:"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" ID="txtProductionRatesOfflineSpecificPiecesManHourValue"
                                                                        Width="75px" Enabled="false"></asp:TextBox>
                                                                    <asp:CompareValidator runat="server" ID="cvProductionRatesOfflineSpecificPiecesManHourValue"
                                                                        Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtProductionRatesOfflineSpecificPiecesManHourValue"
                                                                        ErrorMessage="Offline specific pieces per man hour must be a number." SetFocusOnError="True" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="p_text">
                                                                    <asp:Label runat="server" ID="lblProductionRatesOfflineSpecificPercentRecycleLabel"
                                                                        Text="Percent Recycle:"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" ID="txtProductionRatesOfflineSpecificPercentRecycleValue"
                                                                        Width="75px" Enabled="false"></asp:TextBox>
                                                                    <asp:Label runat="server" ID="lblProductionRatesOfflineSpecificPercentRecycleValuePercent"></asp:Label>
                                                                    <asp:CompareValidator runat="server" ID="cvProductionRatesOfflineSpecificPercentRecycleValue"
                                                                        Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtProductionRatesOfflineSpecificPercentRecycleValue"
                                                                        ErrorMessage="Offline specific pieces per man hour must be a number." SetFocusOnError="True" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td colspan="2">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td style="white-space: nowrap;">
                                                                    <asp:Label runat="server" ID="lblProductionRatesFinalFiguresQuotedMessage" Text="Quoted"></asp:Label>
                                                                </td>
                                                                <td style="white-space: nowrap;">
                                                                    <asp:Label runat="server" ID="lblProductionRatesFinalFiguresMaximumMessage" Text="Maximum"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="p_text" style="white-space: nowrap;">
                                                                    <asp:Label runat="server" ID="lblProductionRatesFinalFiguresMaxPiecesLabel" Text="Max Pieces:"></asp:Label>
                                                                </td>
                                                                <td style="white-space: nowrap;">
                                                                    <asp:TextBox runat="server" ID="txtProductionRatesFinalFiguresMaxPiecesQuotedValue"
                                                                        Width="75px" Enabled="false"></asp:TextBox>
                                                                    <asp:CompareValidator runat="server" ID="cvProductionRatesFinalFiguresMaxPiecesQuotedValue"
                                                                        Operator="DataTypeCheck" ValidationGroup="vgSave" Type="integer" Text="<" ControlToValidate="txtProductionRatesFinalFiguresMaxPiecesQuotedValue"
                                                                        ErrorMessage="Max Pieces Quoted must be an integer." SetFocusOnError="True" />
                                                                    <asp:DropDownList runat="server" ID="ddProductionRatesFinalFiguresMaxPiecesQuotedUnits"
                                                                        Enabled="false">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td style="white-space: nowrap;">
                                                                    <asp:TextBox runat="server" ID="txtProductionRatesFinalFiguresMaxPiecesMaximumValue"
                                                                        Width="75px" Enabled="false"></asp:TextBox>
                                                                    <asp:CompareValidator runat="server" ID="cvProductionRatesFinalFiguresMaxPiecesMaximumValue"
                                                                        Operator="DataTypeCheck" ValidationGroup="vgSave" Type="integer" Text="<" ControlToValidate="txtProductionRatesFinalFiguresMaxPiecesMaximumValue"
                                                                        ErrorMessage="Max Pieces Maximum must be an integer." SetFocusOnError="True" />
                                                                    <asp:DropDownList runat="server" ID="ddProductionRatesFinalFiguresMaxPiecesMaximumUnits"
                                                                        Enabled="false">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="p_text" style="white-space: nowrap;">
                                                                    <asp:Label runat="server" ID="lblProductionRatesFinalFiguresPressCyclesQuotedLabel"
                                                                        Text="Press Cycles:"></asp:Label>
                                                                </td>
                                                                <td style="white-space: nowrap;">
                                                                    <asp:TextBox runat="server" ID="txtProductionRatesFinalFiguresPressCyclesQuotedValue"
                                                                        Width="75px" Enabled="false"></asp:TextBox>
                                                                    <asp:CompareValidator runat="server" ID="cvProductionRatesFinalFiguresPressCyclesQuotedValue"
                                                                        Operator="DataTypeCheck" ValidationGroup="vgSave" Type="integer" Text="<" ControlToValidate="txtProductionRatesFinalFiguresPressCyclesQuotedValue"
                                                                        ErrorMessage="Press cycles quoted must be an integer." SetFocusOnError="True" />
                                                                    <asp:DropDownList runat="server" ID="ddProductionRatesFinalFiguresPressCyclesQuotedUnits"
                                                                        Enabled="false">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td class="p_text" style="white-space: nowrap;">
                                                                    <asp:TextBox runat="server" ID="txtProductionRatesFinalFiguresPressCyclesMaximumValue"
                                                                        Width="75px" Enabled="false"></asp:TextBox>
                                                                    <asp:CompareValidator runat="server" ID="cvProductionRatesFinalFiguresPressCyclesMaximumValue"
                                                                        Operator="DataTypeCheck" ValidationGroup="vgSave" Type="integer" Text="<" ControlToValidate="txtProductionRatesFinalFiguresPressCyclesMaximumValue"
                                                                        ErrorMessage="Press cycles maximum must be an integer." SetFocusOnError="True" />
                                                                    <asp:DropDownList runat="server" ID="ddProductionRatesFinalFiguresPressCyclesMaximumUnits"
                                                                        Enabled="false">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="p_text" style="white-space: nowrap;">
                                                                    <asp:Label runat="server" ID="lblProductionRatesFinalFiguresLineSpeedQuotedLabel"
                                                                        Text="Line Speed:"></asp:Label>
                                                                </td>
                                                                <td style="white-space: nowrap;">
                                                                    <asp:TextBox runat="server" ID="txtProductionRatesFinalFiguresLineSpeedQuotedValue"
                                                                        Width="75px" Enabled="false"></asp:TextBox>
                                                                    <asp:CompareValidator runat="server" ID="cvProductionRatesFinalFiguresLineSpeedQuotedValue"
                                                                        Operator="DataTypeCheck" ValidationGroup="vgSave" Type="integer" Text="<" ControlToValidate="txtProductionRatesFinalFiguresLineSpeedQuotedValue"
                                                                        ErrorMessage="Line speed quoted must be an integer." SetFocusOnError="True" />
                                                                    <asp:DropDownList runat="server" ID="ddProductionRatesFinalFiguresLineSpeedQuotedUnits"
                                                                        Enabled="false">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td class="p_text" style="white-space: nowrap;">
                                                                    <asp:TextBox runat="server" ID="txtProductionRatesFinalFiguresLineSpeedMaximumValue"
                                                                        Width="75px" Enabled="false"></asp:TextBox>
                                                                    <asp:CompareValidator runat="server" ID="cvProductionRatesFinalFiguresLineSpeedMaximumValue"
                                                                        Operator="DataTypeCheck" ValidationGroup="vgSave" Type="integer" Text="<" ControlToValidate="txtProductionRatesFinalFiguresLineSpeedMaximumValue"
                                                                        ErrorMessage="Line speed maximum must be an integer." SetFocusOnError="True" />
                                                                    <asp:DropDownList runat="server" ID="ddProductionRatesFinalFiguresLineSpeedMaximumUnits"
                                                                        Enabled="false">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="p_text" style="white-space: nowrap;">
                                                                    <asp:Label runat="server" ID="lblProductionRatesFinalFiguresMixCapacityQuotedLabel"
                                                                        Text="Mix Capacity:"></asp:Label>
                                                                </td>
                                                                <td style="white-space: nowrap;">
                                                                    <asp:TextBox runat="server" ID="txtProductionRatesFinalFiguresMixCapacityQuotedValue"
                                                                        Width="75px" Enabled="false"></asp:TextBox>
                                                                    <asp:CompareValidator runat="server" ID="cvProductionRatesFinalFiguresMixCapacityQuotedValue"
                                                                        Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtProductionRatesFinalFiguresMixCapacityQuotedValue"
                                                                        ErrorMessage="Mix Capacity quoted must be a number." SetFocusOnError="True" />
                                                                    <asp:DropDownList runat="server" ID="ddProductionRatesFinalFiguresMixCapacityQuotedUnits"
                                                                        Enabled="false">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td style="white-space: nowrap;">
                                                                    <asp:TextBox runat="server" ID="txtProductionRatesFinalFiguresMixCapacityMaximumValue"
                                                                        Width="75px" Enabled="false"></asp:TextBox>
                                                                    <asp:CompareValidator runat="server" ID="cvProductionRatesFinalFiguresMixCapacityMaximumValue"
                                                                        Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtProductionRatesFinalFiguresMixCapacityMaximumValue"
                                                                        ErrorMessage="Mix Capacity maximum must be a number." SetFocusOnError="True" />
                                                                    <asp:DropDownList runat="server" ID="ddProductionRatesFinalFiguresMixCapacityMaximumUnits"
                                                                        Enabled="false">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="p_text" style="white-space: nowrap;">
                                                                    <asp:Label runat="server" ID="lblProductionRatesFinalFiguresNetFormingRateQuotedLabel"
                                                                        Text="Net Forming Rate:"></asp:Label>
                                                                </td>
                                                                <td style="white-space: nowrap;">
                                                                    <asp:TextBox runat="server" ID="txtProductionRatesFinalFiguresNetFormingRateQuotedValue"
                                                                        Width="75px" Enabled="false"></asp:TextBox>
                                                                    <asp:CompareValidator runat="server" ID="cvProductionRatesFinalFiguresNetFormingRateQuotedValue"
                                                                        Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtProductionRatesFinalFiguresNetFormingRateQuotedValue"
                                                                        ErrorMessage="Net Forming rate quoted must be a number." SetFocusOnError="True" />
                                                                    <asp:DropDownList runat="server" ID="ddProductionRatesFinalFiguresNetFormingRateQuotedUnits"
                                                                        Enabled="false">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td style="white-space: nowrap;">
                                                                    <asp:TextBox runat="server" ID="txtProductionRatesFinalFiguresNetFormingRateMaximumValue"
                                                                        Width="75px" Enabled="false"></asp:TextBox>
                                                                    <asp:CompareValidator runat="server" ID="cvProductionRatesFinalFiguresNetFormingRateMaximumValue"
                                                                        Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtProductionRatesFinalFiguresNetFormingRateMaximumValue"
                                                                        ErrorMessage="Net Forming rate maximum must be a number." SetFocusOnError="True" />
                                                                    <asp:DropDownList runat="server" ID="ddProductionRatesFinalFiguresNetFormingRateMaximumUnits"
                                                                        Enabled="False">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="p_text" style="white-space: nowrap;">
                                                                    <asp:Label runat="server" ID="lblProductionRatesFinalFiguresRecycleRateQuotedLabel"
                                                                        Text="Recycle Rate:"></asp:Label>
                                                                </td>
                                                                <td style="white-space: nowrap;">
                                                                    <asp:TextBox runat="server" ID="txtProductionRatesFinalFiguresRecycleRateQuotedValue"
                                                                        Width="75px" Enabled="false"></asp:TextBox>
                                                                    <asp:CompareValidator runat="server" ID="cvProductionRatesFinalFiguresRecycleRateQuotedValue"
                                                                        Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtProductionRatesFinalFiguresRecycleRateQuotedValue"
                                                                        ErrorMessage="Recycle rate quoted must be a number." SetFocusOnError="True" />
                                                                    <asp:DropDownList runat="server" ID="ddProductionRatesFinalFiguresRecycleRateQuotedUnits"
                                                                        Enabled="false">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td style="white-space: nowrap;">
                                                                    <asp:TextBox runat="server" ID="txtProductionRatesFinalFiguresRecycleRateMaximumValue"
                                                                        Width="75px" Enabled="false"></asp:TextBox>
                                                                    <asp:CompareValidator runat="server" ID="cvProductionRatesFinalFiguresRecycleRateMaximumValue"
                                                                        Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtProductionRatesFinalFiguresRecycleRateMaximumValue"
                                                                        ErrorMessage="Recycle rate maximum must be a number." SetFocusOnError="True" />
                                                                    <asp:DropDownList runat="server" ID="ddProductionRatesFinalFiguresRecycleRateMaximumUnits"
                                                                        Enabled="false">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="p_text" style="white-space: nowrap;">
                                                                    <asp:Label runat="server" ID="lblProductionRatesFinalFiguresPartWeightQuotedLabel"
                                                                        Text="Part Weight"></asp:Label>
                                                                </td>
                                                                <td style="white-space: nowrap;">
                                                                    <asp:TextBox runat="server" ID="txtProductionRatesFinalFiguresPartWeightQuotedValue"
                                                                        Width="75px" Enabled="false"></asp:TextBox>
                                                                    <asp:CompareValidator runat="server" ID="cvProductionRatesFinalFiguresPartWeightQuotedValue"
                                                                        Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtProductionRatesFinalFiguresPartWeightQuotedValue"
                                                                        ErrorMessage="Part weight quoted must be a number." SetFocusOnError="True" />
                                                                    <asp:DropDownList runat="server" ID="ddProductionRatesFinalFiguresPartWeightQuotedUnits"
                                                                        Enabled="false">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td style="white-space: nowrap;">
                                                                    <asp:TextBox runat="server" ID="txtProductionRatesFinalFiguresPartWeightMaximumValue"
                                                                        Width="75px" Enabled="false"></asp:TextBox>
                                                                    <asp:CompareValidator runat="server" ID="cvProductionRatesFinalFiguresPartWeightMaximumValue"
                                                                        Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtProductionRatesFinalFiguresPartWeightMaximumValue"
                                                                        ErrorMessage="Part weight maximum must be a number." SetFocusOnError="True" />
                                                                    <asp:DropDownList runat="server" ID="ddProductionRatesFinalFiguresPartWeightMaximumUnits"
                                                                        Enabled="false">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="p_text" style="white-space: nowrap;">
                                                                    <asp:Label runat="server" ID="lblProductionRatesFinalFiguresCoatingWeightQuotedLabel"
                                                                        Text="Coating Weight"></asp:Label>
                                                                </td>
                                                                <td style="white-space: nowrap;">
                                                                    <asp:TextBox runat="server" ID="txtProductionRatesFinalFiguresCoatingWeightQuotedValue"
                                                                        Width="75px" Enabled="false"></asp:TextBox>
                                                                    <asp:CompareValidator runat="server" ID="cvProductionRatesFinalFiguresCoatingWeightQuotedValue"
                                                                        Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtProductionRatesFinalFiguresCoatingWeightQuotedValue"
                                                                        ErrorMessage="Coating weight quoted must be a number." SetFocusOnError="True" />
                                                                    <asp:DropDownList runat="server" ID="ddProductionRatesFinalFiguresCoatingWeightQuotedUnits"
                                                                        Enabled="false">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td class="p_text" style="white-space: nowrap;">
                                                                    <asp:TextBox runat="server" ID="txtProductionRatesFinalFiguresCoatingWeightMaximumValue"
                                                                        Width="75px" Enabled="false" Visible="false"></asp:TextBox>
                                                                    <asp:CompareValidator runat="server" ID="cvProductionRatesFinalFiguresCoatingWeightMaximumValue"
                                                                        Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtProductionRatesFinalFiguresCoatingWeightMaximumValue"
                                                                        ErrorMessage="Coating weight maximum  must be a number." SetFocusOnError="True" />
                                                                    <asp:DropDownList runat="server" ID="ddProductionRatesFinalFiguresCoatingWeightMaximumUnits"
                                                                        Enabled="false" Visible="false">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="p_text" style="white-space: nowrap;">
                                                                    <asp:Label runat="server" ID="lblProductionRatesFinalFiguresTotalWeightQuotedLabel"
                                                                        Text="Total Weight"></asp:Label>
                                                                </td>
                                                                <td style="white-space: nowrap;">
                                                                    <asp:TextBox runat="server" ID="txtProductionRatesFinalFiguresTotalWeightQuotedValue"
                                                                        Width="75px" Enabled="false"></asp:TextBox>
                                                                    <asp:CompareValidator runat="server" ID="cvProductionRatesFinalFiguresTotalWeightQuotedValue"
                                                                        Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtProductionRatesFinalFiguresTotalWeightQuotedValue"
                                                                        ErrorMessage="Total weight quoted must be a number." SetFocusOnError="True" />
                                                                    <asp:DropDownList runat="server" ID="ddProductionRatesFinalFiguresTotalWeightQuotedUnits"
                                                                        Enabled="false">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td style="white-space: nowrap;">
                                                                    <asp:TextBox runat="server" ID="txtProductionRatesFinalFiguresTotalWeightMaximumValue"
                                                                        Width="75px" Enabled="false" Visible="false"></asp:TextBox>
                                                                    <asp:CompareValidator runat="server" ID="cvProductionRatesFinalFiguresTotalWeightMaximumValue"
                                                                        Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtProductionRatesFinalFiguresTotalWeightMaximumValue"
                                                                        ErrorMessage="Total weight maximum must be a number." SetFocusOnError="True" />
                                                                    <asp:DropDownList runat="server" ID="ddProductionRatesFinalFiguresTotalWeightMaximumUnits"
                                                                        Visible="false" Enabled="false">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:View>
                                        <asp:View ID="vQuotedInfo" runat="server">
                                            <table>
                                                <tr>
                                                    <td class="p_text" style="white-space: nowrap">
                                                        Account Manager:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddQuotedInfoAccountManager" runat="server" Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="p_text" style="white-space: nowrap">
                                                        Standard Cost Factor:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtQuotedInfoStandardCostFactor" Enabled="false"
                                                            Text="1.02"></asp:TextBox>
                                                        &nbsp; <i>(for example: 1.02 = 2%)</i>
                                                        <asp:CompareValidator runat="server" ID="cvQuotedInfoStandardCostFactor" Operator="DataTypeCheck"
                                                            ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtQuotedInfoStandardCostFactor"
                                                            ErrorMessage="Standard cost factor must be a number." SetFocusOnError="True" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text" style="white-space: nowrap">
                                                        Pieces/Year:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtQuotedInfoPiecesPerYear" Enabled="false"></asp:TextBox>
                                                        <asp:CompareValidator runat="server" ID="cvQuotedInfoPiecesPerYear" Operator="DataTypeCheck"
                                                            ValidationGroup="vgSave" Type="integer" Text="<" ControlToValidate="txtQuotedInfoPiecesPerYear"
                                                            ErrorMessage="Pieces per year must be an integer." SetFocusOnError="True" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        &nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text" style="vertical-align: top">
                                                        Comments:
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox runat="server" ID="txtQuotedInfoComments" Width="90%" Height="400px"
                                                            Enabled="false" TextMode="MultiLine"></asp:TextBox>
                                                        <br />
                                                        <asp:Label ID="lblQuotedInfoCommentsCharCount" SkinID="MessageLabelSkin" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:View>
                                        <asp:View ID="vMaterials" runat="server">
                                            <asp:Label runat="server" ID="lblMessageMaterial" SkinID="MessageLabelSkin"></asp:Label>
                                            <asp:Label runat="server" ID="lblTipMaterial" SkinID="MessageLabelSkin" Text="When viewing the Cost per Unit below:<br>If the border is aqua, then in the Material Maintenance page, the Freight Cost Plus the Standard Cost does NOT equal the Purchased Cost.<br>If the foreground color is red, then on the Material Maintenance page, the Purchased Cost is not matched to the Quote Cost.<br>If the background color is yellow, then on the Material Maintenance page, the Quote Cost has been updated and does not match the cost listed below."></asp:Label>
                                            <br />
                                            <asp:ValidationSummary ID="vsFooterMaterial" runat="server" ShowMessageBox="True"
                                                ShowSummary="true" ValidationGroup="vgFooterMaterial" />
                                            <asp:ValidationSummary ID="vsEditMaterial" runat="server" ShowMessageBox="True" ShowSummary="true"
                                                ValidationGroup="vgEditMaterial" />
                                            <br />
                                            <asp:Button runat="server" ID="btnRemoveMaterials" Text="Remove All Materials" Visible="false" />
                                            <asp:GridView runat="server" ID="gvMaterial" Width="100%" AllowPaging="True" AllowSorting="True"
                                                AutoGenerateColumns="False" DataSourceID="odsCostSheetMaterial" PageSize="15"
                                                ShowFooter="True" DataKeyNames="RowID">
                                                <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                                <EditRowStyle BackColor="#CCCCCC" />
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
                                                <Columns>
                                                    <asp:BoundField DataField="CostSheetID" SortExpression="CostSheetID" ShowHeader="False">
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="MaterialID" HeaderText="Material ID" SortExpression="MaterialID"
                                                        ReadOnly="true">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Part No (if exists) and Material Name">
                                                        <HeaderStyle Wrap="true" />
                                                        <FooterStyle Wrap="false" />
                                                        <ItemStyle Wrap="true" />
                                                        <EditItemTemplate>
                                                            <asp:Label ID="lblEditMaterialID" runat="server" Text='<%# Bind("MaterialID") %>'
                                                                CssClass="none"></asp:Label>
                                                            <asp:Label ID="lblEditMaterialName" runat="server" Text='<%# Bind("ddMaterialNameCombo") %>'></asp:Label>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewMaterialID" runat="server" Text='<%# Bind("MaterialID") %>'
                                                                CssClass="none"></asp:Label>
                                                            <asp:Label ID="lblViewMaterialName" runat="server" Text='<%# Bind("ddMaterialNameCombo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddFooterMaterial" runat="server" DataSource='<%# CostingModule.GetMaterial("","","","",0,0,"","",False, False, False, False, False, False) %>'
                                                                DataValueField="MaterialID" DataTextField="ddMaterialNameCombo" AppendDataBoundItems="True"
                                                                OnSelectedIndexChanged="ddFooterMaterial_SelectedIndexChanged" AutoPostBack="true"
                                                                Width="225px">
                                                                <asp:ListItem Text="" Value="0" Selected="False"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:ImageButton ID="ibtnGetMaterial" runat="server" CausesValidation="False" ImageUrl="~/images/Search.gif"
                                                                ToolTip="Get Material" AlternateText="Get Material" />
                                                            <asp:RequiredFieldValidator ID="rfvFooterMaterial" runat="server" ControlToValidate="ddFooterMaterial"
                                                                ErrorMessage="The name is required." Font-Bold="True" ValidationGroup="vgFooterMaterial"
                                                                Text="<" SetFocusOnError="true" />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Quantity" SortExpression="Quantity">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditMaterialQuantity" runat="server" Text='<%# Bind("Quantity") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditMaterialQuantity" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditMaterial" Type="Double" Text="<" ControlToValidate="txtEditMaterialQuantity"
                                                                ErrorMessage="Quantity must be a number." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewMaterialQuantity" runat="server" Text='<%# Bind("Quantity") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtFooterMaterialQuantity" runat="server" Text='<%# Bind("Quantity") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvFooterMaterialQuantity" Operator="DataTypeCheck"
                                                                ValidationGroup="vgFooterMaterial" Type="Double" Text="<" ControlToValidate="txtFooterMaterialQuantity"
                                                                ErrorMessage="Quantity must be an integer." SetFocusOnError="True" />
                                                        </FooterTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Usage Factor" SortExpression="UsageFactor">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditMaterialUsageFactor" runat="server" Text='<%# Bind("UsageFactor") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditMaterialUsageFactor" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditMaterial" Type="double" Text="<" ControlToValidate="txtEditMaterialUsageFactor"
                                                                ErrorMessage="Usage factor must be a number." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewMaterialUsageFactor" runat="server" Text='<%# Bind("UsageFactor") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtFooterMaterialUsageFactor" runat="server" Text='<%# Bind("UsageFactor") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvFooterMaterialUsageFactor" Operator="DataTypeCheck"
                                                                ValidationGroup="vgFooterMaterial" Type="double" Text="<" ControlToValidate="txtFooterMaterialUsageFactor"
                                                                ErrorMessage="Usage factor must be a number." SetFocusOnError="True" />
                                                        </FooterTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cost / Unit" SortExpression="CostPerUnit">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditMaterialCostPerUnit" runat="server" Text='<%# Bind("CostPerUnit") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditMaterialCostPerUnit" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditMaterial" Type="double" Text="<" ControlToValidate="txtEditMaterialCostPerUnit"
                                                                ErrorMessage="Cost per unit must be a number." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewMaterialCostPerUnit" runat="server" Text='<%# Bind("CostPerUnit") %>'></asp:Label>
                                                            <asp:Label ID="lblViewMaterialNewQuoteCost" runat="server" Text='<%# Bind("isNewQuoteCost") %>'
                                                                Visible="false"></asp:Label>
                                                            <asp:Label ID="lblViewMaterialMismatchedQuoteAndPurchasedCost" runat="server" Text='<%# Bind("isMismatchedQuoteAndPurchasedCost") %>'
                                                                Visible="false"></asp:Label>
                                                            <asp:Label ID="lblViewMaterialMismatchedFreightPlusStandardCost" runat="server" Text='<%# Bind("isMismatchedFreightPlusStandardCost") %>'
                                                                Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtFooterMaterialCostPerUnit" runat="server" Text='<%# Bind("CostPerUnit") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvFooterMaterialCostPerUnit" Operator="DataTypeCheck"
                                                                ValidationGroup="vgFooterMaterial" Type="double" Text="<" ControlToValidate="txtFooterMaterialCostPerUnit"
                                                                ErrorMessage="Cost per unit must be a number." SetFocusOnError="True" />
                                                        </FooterTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Freight" SortExpression="FreightCost">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditMaterialFreightCost" runat="server" Text='<%# Bind("FreightCost") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditMaterialFreightCost" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditMaterial" Type="double" Text="<" ControlToValidate="txtEditMaterialFreightCost"
                                                                ErrorMessage="Freight Cost must be a number." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewMaterialFreightCost" runat="server" Text='<%# Bind("FreightCost") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtFooterMaterialFreightCost" runat="server" Text='<%# Bind("FreightCost") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvFooterMaterialFreightCost" Operator="DataTypeCheck"
                                                                ValidationGroup="vgFooterMaterial" Type="double" Text="<" ControlToValidate="txtFooterMaterialFreightCost"
                                                                ErrorMessage="Freight Cost must be a number." SetFocusOnError="True" />
                                                        </FooterTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Standard Cost<br> Per Unit<br> W/O Scrap" SortExpression="StandardCostPerUnitWOScrap">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditMaterialStandardCostPerUnitWOScrap" runat="server" Text='<%# Bind("StandardCostPerUnitWOScrap") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditMaterialStandardCostPerUnitWOScrap"
                                                                Operator="DataTypeCheck" ValidationGroup="vgEditMaterial" Type="double" Text="<"
                                                                ControlToValidate="txtEditMaterialStandardCostPerUnitWOScrap" ErrorMessage="Standard cost per unit must be a number."
                                                                SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewMaterialStandardCostPerUnitWOScrap" runat="server" Text='<%# Bind("StandardCostPerUnitWOScrap") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Standard Cost<br> Factor (Scrap)" SortExpression="StandardCostFactor">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditMaterialStandardCostFactor" runat="server" Text='<%# Bind("StandardCostFactor") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditMaterialStandardCostFactor" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditMaterial" Type="double" Text="<" ControlToValidate="txtEditMaterialStandardCostFactor"
                                                                ErrorMessage="Standard cost factor must be a number." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewMaterialStandardCostFactor" runat="server" Text='<%# Bind("StandardCostFactor") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtFooterMaterialStandardCostFactor" runat="server" Text='<%# Bind("StandardCostFactor") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvFooterMaterialStandardCostFactor" Operator="DataTypeCheck"
                                                                ValidationGroup="vgFooterMaterial" Type="double" Text="<" ControlToValidate="txtFooterMaterialStandardCostFactor"
                                                                ErrorMessage="Standard cost factor must be a number." SetFocusOnError="True" />
                                                        </FooterTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Standard Cost<br> Per Unit<br> WITH Scrap" SortExpression="StandardCostPerUnit">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditMaterialStandardCostPerUnit" runat="server" Text='<%# Bind("StandardCostPerUnit") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditMaterialStandardCostPerUnit" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditMaterial" Type="double" Text="<" ControlToValidate="txtEditMaterialStandardCostPerUnit"
                                                                ErrorMessage="Standard cost per unit must be a number." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewMaterialStandardCostPerUnit" runat="server" Text='<%# Bind("StandardCostPerUnit") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Ordinal" SortExpression="Ordinal">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditMaterialOrdinal" runat="server" Text='<%# Bind("Ordinal") %>'
                                                                MaxLength="2" Width="25px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditMaterialOrdinal" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditMaterial" Type="integer" Text="<" ControlToValidate="txtEditMaterialOrdinal"
                                                                ErrorMessage="Ordinal must be an integer." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewMaterialOrdinal" runat="server" Text='<%# Bind("Ordinal") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtFooterMaterialOrdinal" runat="server" Text="99" MaxLength="2"
                                                                Width="25px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvFooterMaterialOrdinal" Operator="DataTypeCheck"
                                                                ValidationGroup="vgFooterMaterial" Type="integer" Text="<" ControlToValidate="txtFooterMaterialOrdinal"
                                                                ErrorMessage="Ordinal must be an integer." SetFocusOnError="True" />
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ShowHeader="False">
                                                        <EditItemTemplate>
                                                            <asp:ImageButton ID="ibtnUpdateMaterial" runat="server" CausesValidation="True" CommandName="Update"
                                                                ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditMaterial" />
                                                            &nbsp;&nbsp;&nbsp;
                                                            <asp:ImageButton ID="ibtnCancelMaterial" runat="server" CausesValidation="False"
                                                                CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtnEditMaterial" runat="server" CausesValidation="False" CommandName="Edit"
                                                                ImageUrl="~/images/edit.jpg" AlternateText="Edit" />&nbsp;&nbsp;&nbsp;
                                                            <asp:ImageButton ID="ibtnDeleteMaterial" runat="server" CausesValidation="False"
                                                                CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:ImageButton ID="ibtnInsertMaterial" runat="server" CausesValidation="True" CommandName="Insert"
                                                                ImageUrl="~/images/save.jpg" AlternateText="Insert" ValidationGroup="vgFooterMaterial" />&nbsp;&nbsp;&nbsp;
                                                            <asp:ImageButton ID="ibtnUndoMaterial" runat="server" CommandName="Undo" ImageUrl="~/images/undo-gray.jpg"
                                                                AlternateText="Undo" />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <asp:ObjectDataSource ID="odsCostSheetMaterial" runat="server" OldValuesParameterFormatString="original_{0}"
                                                SelectMethod="GetCostSheetMaterial" TypeName="CostSheetMaterialBLL" DeleteMethod="DeleteCostSheetMaterial"
                                                UpdateMethod="UpdateCostSheetMaterial" InsertMethod="InsertCostSheetMaterial">
                                                <SelectParameters>
                                                    <asp:QueryStringParameter Name="CostSheetID" QueryStringField="CostSheetID" Type="Int32" />
                                                    <asp:Parameter DefaultValue="0" Name="MaterialID" Type="Int32" />
                                                </SelectParameters>
                                                <DeleteParameters>
                                                    <asp:Parameter Name="RowID" Type="Int32" />
                                                    <asp:Parameter Name="original_RowID" Type="Int32" />
                                                </DeleteParameters>
                                                <UpdateParameters>
                                                    <asp:Parameter Name="MaterialID" Type="Int32" />
                                                    <asp:Parameter Name="Quantity" Type="Double" />
                                                    <asp:Parameter Name="UsageFactor" Type="Double" />
                                                    <asp:Parameter Name="CostPerUnit" Type="Double" />
                                                    <asp:Parameter Name="FreightCost" Type="Double" />
                                                    <asp:Parameter Name="StandardCostFactor" Type="Double" />
                                                    <asp:Parameter Name="QuoteCostFactor" Type="Double" />
                                                    <asp:Parameter Name="StandardCostPerUnitWOScrap" Type="Double" />
                                                    <asp:Parameter Name="StandardCostPerUnit" Type="Double" />
                                                    <asp:Parameter Name="Ordinal" Type="Int32" />
                                                    <asp:Parameter Name="original_RowID" Type="Int32" />
                                                    <asp:Parameter Name="ddMaterialDesc" Type="String" />
                                                    <asp:Parameter Name="CostSheetID" Type="Int32" />
                                                    <asp:Parameter Name="ddMaterialName" Type="String" />
                                                </UpdateParameters>
                                                <InsertParameters>
                                                    <asp:Parameter Name="CostSheetID" Type="Int32" />
                                                    <asp:Parameter Name="MaterialID" Type="Int32" />
                                                    <asp:Parameter Name="Quantity" Type="Double" />
                                                    <asp:Parameter Name="UsageFactor" Type="Double" />
                                                    <asp:Parameter Name="CostPerUnit" Type="Double" />
                                                    <asp:Parameter Name="FreightCost" Type="Double" />
                                                    <asp:Parameter Name="StandardCostFactor" Type="Double" />
                                                    <asp:Parameter Name="Ordinal" Type="Int32" />
                                                </InsertParameters>
                                            </asp:ObjectDataSource>
                                        </asp:View>
                                        <asp:View ID="vPackaging" runat="server">
                                            <asp:Label runat="server" ID="lblMessagePackaging" Text=""></asp:Label><br />
                                            <asp:Label runat="server" ID="lblTipPackaging" SkinID="MessageLabelSkin" Text="When viewing the Cost per Unit below:<br>If the foreground color is red, then on the Material Maintenance page, the Purchased Cost is not matched to the Quote Cost.<br>If the background color is yellow, then on the Material Maintenance page, the Quote Cost has been updated and does not match the cost listed below."></asp:Label><br />
                                            <asp:ValidationSummary ID="vsFooterPackaging" runat="server" ShowMessageBox="True"
                                                ShowSummary="true" ValidationGroup="vgFooterPackaging" />
                                            <asp:ValidationSummary ID="vsEditPackaging" runat="server" ShowMessageBox="True"
                                                ShowSummary="true" ValidationGroup="vgEditPackaging" />
                                            <br />
                                            <asp:Button runat="server" ID="btnRemovePackaging" Text="Remove All Materials" Visible="false" />
                                            <asp:GridView runat="server" ID="gvPackaging" Width="100%" AllowPaging="True" AllowSorting="True"
                                                AutoGenerateColumns="False" DataSourceID="odsCostSheetPackaging" PageSize="15"
                                                ShowFooter="True" DataKeyNames="RowID">
                                                <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                                <EditRowStyle BackColor="#CCCCCC" />
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
                                                <Columns>
                                                    <asp:BoundField DataField="CostSheetID" SortExpression="CostSheetID" ShowHeader="False" />
                                                    <asp:BoundField DataField="MaterialID" HeaderText="Material ID" SortExpression="MaterialID"
                                                        ReadOnly="true">
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Part No (if exists) and Material Name">
                                                        <HeaderStyle Wrap="true" />
                                                        <FooterStyle Wrap="false" />
                                                        <ItemStyle Wrap="true" />
                                                        <EditItemTemplate>
                                                            <asp:Label ID="lblEditPackagingID" runat="server" CssClass="none" Text='<%# Bind("MaterialID") %>'></asp:Label>
                                                            <asp:Label ID="lblEditPackagingDesc" runat="server" Text='<%# Bind("ddMaterialName") %>'></asp:Label>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewPackagingID" runat="server" CssClass="none" Text='<%# Bind("MaterialID") %>'></asp:Label>
                                                            <asp:Label ID="lblViewPackagingDesc" runat="server" Text='<%# Bind("ddMaterialName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddFooterPackaging" runat="server" DataSource='<%# CostingModule.GetMaterial("","","","",0,0,"","",True,True,False,False,False,False) %>'
                                                                DataValueField="MaterialID" DataTextField="ddMaterialNameCombo" AppendDataBoundItems="True"
                                                                OnSelectedIndexChanged="ddFooterPackaging_SelectedIndexChanged" AutoPostBack="true"
                                                                Width="200px" SelectedValue='<%# Bind("MaterialID") %>'>
                                                                <asp:ListItem Text="" Value="0" Selected="False"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:ImageButton ID="ibtnGetPackaging" runat="server" CausesValidation="False" ImageUrl="~/images/Search.gif"
                                                                ToolTip="Get Packaging" AlternateText="Get Packaging" />
                                                            <asp:RequiredFieldValidator ID="rfvFooterPackaging" runat="server" ControlToValidate="ddFooterPackaging"
                                                                ErrorMessage="The name is required." Font-Bold="True" ValidationGroup="vgFooterPackaging"
                                                                Text="<" SetFocusOnError="true" />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cost / Unit" SortExpression="CostPerUnit">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditPackagingCostPerUnit" runat="server" MaxLength="10" Width="50px"
                                                                Text='<%# Bind("CostPerUnit") %>'></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditPackagingCostPerUnit" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditPackaging" Type="Double" Text="<" ControlToValidate="txtEditPackagingCostPerUnit"
                                                                ErrorMessage="Cost per unit must be a number." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewPackagingCostPerUnit" runat="server" Text='<%# Bind("CostPerUnit") %>'></asp:Label>
                                                            <asp:Label ID="lblViewPackagingNewQuoteCost" runat="server" Text='<%# Bind("isNewQuoteCost") %>'
                                                                Visible="false"></asp:Label>
                                                            <asp:Label ID="lblViewPackagingMismatchedQuoteAndPurchasedCost" runat="server" Text='<%# Bind("isMismatchedQuoteAndPurchasedCost") %>'
                                                                Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtFooterPackagingCostPerUnit" runat="server" MaxLength="10" Width="50px"
                                                                Text='<%# Bind("CostPerUnit") %>'></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvFooterPackagingCostPerUnit" Operator="DataTypeCheck"
                                                                ValidationGroup="vgFooterPackaging" Type="Double" Text="<" ControlToValidate="txtFooterPackagingCostPerUnit"
                                                                ErrorMessage="Cost per unit must be a number." SetFocusOnError="True" />
                                                        </FooterTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Units Needed" SortExpression="UnitsNeeded">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditPackagingUnitsNeeded" runat="server" MaxLength="10" Width="50px"
                                                                Text='<%# Bind("UnitsNeeded") %>'></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditPackagingUnitsNeeded" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditPackaging" Type="double" Text="<" ControlToValidate="txtEditPackagingUnitsNeeded"
                                                                ErrorMessage="Units needed must be a number." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewPackagingUnitsNeeded" runat="server" Text='<%# Bind("UnitsNeeded") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtFooterPackagingUnitsNeeded" runat="server" MaxLength="10" Width="50px"
                                                                Text='<%# Bind("UnitsNeeded") %>'></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvFooterPackagingUnitsNeeded" Operator="DataTypeCheck"
                                                                ValidationGroup="vgFooterPackaging" Type="double" Text="<" ControlToValidate="txtFooterPackagingUnitsNeeded"
                                                                ErrorMessage="Units needed must be a number." SetFocusOnError="True" />
                                                        </FooterTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Parts / Container" SortExpression="PartsPerContainer">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditPackagingPartsPerContainer" runat="server" MaxLength="10"
                                                                Width="50px" Text='<%# Bind("PartsPerContainer") %>'></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditPackagingPartsPerContainer" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditPackaging" Type="integer" Text="<" ControlToValidate="txtEditPackagingPartsPerContainer"
                                                                ErrorMessage="Parts per container must be an integer." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewPackagingPartsPerContainer" runat="server" Text='<%# Bind("PartsPerContainer") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtFooterPackagingPartsPerContainer" runat="server" MaxLength="10"
                                                                Width="50px" Text='<%# Bind("PartsPerContainer") %>'></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvFooterPackagingPartsPerContainer" Operator="DataTypeCheck"
                                                                ValidationGroup="vgFooterPackaging" Type="integer" Text="<" ControlToValidate="txtFooterPackagingPartsPerContainer"
                                                                ErrorMessage="Parts per container must be an integer." SetFocusOnError="True" />
                                                        </FooterTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="UnitsNeededDIVPartsPerContainer" HeaderText="Units Needed / Parts Per Container"
                                                        ReadOnly="True" SortExpression="UnitsNeededDIVPartsPerContainer">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Standard Cost<br> Per Unit<br> W/O Scrap" SortExpression="StandardCostPerUnitWOScrap">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditPackagingStandardCostPerUnitWOScrap" runat="server" Text='<%# Bind("StandardCostPerUnitWOScrap") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditPackagingStandardCostPerUnitWOScrap"
                                                                Operator="DataTypeCheck" ValidationGroup="vgEditPackaging" Type="double" Text="<"
                                                                ControlToValidate="txtEditPackagingStandardCostPerUnitWOScrap" ErrorMessage="Standard cost per unit must be a number."
                                                                SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewPackagingStandardCostPerUnitWOScrap" runat="server" Text='<%# Bind("StandardCostPerUnitWOScrap") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Standard Cost<br> Factor (Scrap)" SortExpression="StandardCostFactor">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditPackagingStandardCostFactor" runat="server" Text='<%# Bind("StandardCostFactor") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditPackagingStandardCostFactor" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditPackaging" Type="double" Text="<" ControlToValidate="txtEditPackagingStandardCostFactor"
                                                                ErrorMessage="Standard cost factor must be a number." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewPackagingStandardCostFactor" runat="server" Text='<%# Bind("StandardCostFactor") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtFooterPackagingStandardCostFactor" runat="server" Text='<%# Bind("StandardCostFactor") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvFooterPackagingStandardCostFactor" Operator="DataTypeCheck"
                                                                ValidationGroup="vgFooterPackaging" Type="double" Text="<" ControlToValidate="txtFooterPackagingStandardCostFactor"
                                                                ErrorMessage="Standard cost factor must be a number." SetFocusOnError="True" />
                                                        </FooterTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Standard Cost Per Unit<br> WITH Scrap" SortExpression="StandardCostPerUnit">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditPackagingStandardCostPerUnit" runat="server" MaxLength="10"
                                                                Width="50px" Text='<%# Bind("StandardCostPerUnit") %>'></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditPackagingStandardCostPerUnit" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditPackaging" Type="double" Text="<" ControlToValidate="txtEditPackagingStandardCostPerUnit"
                                                                ErrorMessage="Standard cost per unit must be a number." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewPackagingStandardCostPerUnit" runat="server" Text='<%# Bind("StandardCostPerUnit") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Used" SortExpression="isUsed">
                                                        <EditItemTemplate>
                                                            <asp:CheckBox ID="chkEditPackagingIsUsed" runat="server" Checked='<%# Bind("isUsed") %>' />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkViewPackagingIsUsed" runat="server" Checked='<%# Bind("isUsed") %>'
                                                                Enabled="false" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:CheckBox ID="chkFooterPackagingIsUsed" runat="server" />
                                                        </FooterTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Ordinal" SortExpression="Ordinal">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditPackagingOrdinal" runat="server" MaxLength="2" Width="25px"
                                                                Text='<%# Bind("Ordinal") %>'></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditPackagingOrdinal" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditPackaging" Type="integer" Text="<" ControlToValidate="txtEditPackagingOrdinal"
                                                                ErrorMessage="Ordinal must be an integer." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewPackagingOrdinal" runat="server" Text='<%# Bind("Ordinal") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtFooterPackagingOrdinal" runat="server" MaxLength="2" Width="25px"
                                                                Text="99"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvFooterPackagingOrdinal" Operator="DataTypeCheck"
                                                                ValidationGroup="vgFooterPackaging" Type="integer" Text="<" ControlToValidate="txtFooterPackagingOrdinal"
                                                                ErrorMessage="Ordinal must be an integer." SetFocusOnError="True" />
                                                        </FooterTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <EditItemTemplate>
                                                            <asp:ImageButton ID="iBtnPackagingUpdate" runat="server" CausesValidation="True"
                                                                CommandName="Update" ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditPackaging" />
                                                            <asp:ImageButton ID="iBtnPackagingCancel" runat="server" CausesValidation="False"
                                                                CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="iBtnPackagingEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                                                ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                                                            <asp:ImageButton ID="ibtnDeleteCostingPackaging" runat="server" CausesValidation="False"
                                                                CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgFooterPackaging"
                                                                runat="server" ID="iBtnFooterPackagingRates" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                                                            <asp:ImageButton ID="iBtnPackagingUndo" runat="server" CommandName="Undo" CausesValidation="false"
                                                                ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <asp:ObjectDataSource ID="odsCostSheetPackaging" runat="server" OldValuesParameterFormatString="original_{0}"
                                                SelectMethod="GetCostSheetPackaging" TypeName="CostSheetPackagingBLL" DeleteMethod="DeleteCostSheetPackaging"
                                                UpdateMethod="UpdateCostSheetPackaging" InsertMethod="InsertCostSheetPackaging">
                                                <SelectParameters>
                                                    <asp:QueryStringParameter Name="CostSheetID" QueryStringField="CostSheetID" Type="Int32" />
                                                    <asp:Parameter DefaultValue="0" Name="MaterialID" Type="Int32" />
                                                </SelectParameters>
                                                <DeleteParameters>
                                                    <asp:Parameter Name="RowID" Type="Int32" />
                                                    <asp:Parameter Name="original_RowID" Type="Int32" />
                                                </DeleteParameters>
                                                <UpdateParameters>
                                                    <asp:Parameter Name="MaterialID" Type="Int32" />
                                                    <asp:Parameter Name="CostPerUnit" Type="Double" />
                                                    <asp:Parameter Name="UnitsNeeded" Type="Double" />
                                                    <asp:Parameter Name="PartsPerContainer" Type="Int32" />
                                                    <asp:Parameter Name="StandardCostFactor" Type="Double" />
                                                    <asp:Parameter Name="StandardCostPerUnitWOScrap" Type="Double" />
                                                    <asp:Parameter Name="StandardCostPerUnit" Type="Double" />
                                                    <asp:Parameter Name="isUsed" Type="Boolean" />
                                                    <asp:Parameter Name="Ordinal" Type="Int32" />
                                                    <asp:Parameter Name="original_RowID" Type="Int32" />
                                                    <asp:Parameter Name="ddMaterialDesc" Type="String" />
                                                    <asp:Parameter Name="CostSheetID" Type="Int32" />
                                                    <asp:Parameter Name="ddMaterialName" Type="String" />
                                                </UpdateParameters>
                                                <InsertParameters>
                                                    <asp:Parameter Name="CostSheetID" Type="Int32" />
                                                    <asp:Parameter Name="MaterialID" Type="Int32" />
                                                    <asp:Parameter Name="CostPerUnit" Type="Double" />
                                                    <asp:Parameter Name="UnitsNeeded" Type="Double" />
                                                    <asp:Parameter Name="PartsPerContainer" Type="Int32" />
                                                    <asp:Parameter Name="StandardCostFactor" Type="Double" />
                                                    <asp:Parameter Name="isUsed" Type="Boolean" />
                                                    <asp:Parameter Name="Ordinal" Type="Int32" />
                                                </InsertParameters>
                                            </asp:ObjectDataSource>
                                        </asp:View>
                                        <asp:View ID="vLabor" runat="server">
                                            <asp:Label runat="server" ID="lblMessageLabor" Text=""></asp:Label><br />
                                            <asp:Label runat="server" ID="lblLaborTip" SkinID="MessageLabelSkin" Text="When viewing the Rate, Crew Size, and Offline below:<br>If the background color is yellow, then on the Labor Maintenance page, the value has been updated and does not match the value listed below."></asp:Label><br />
                                            <asp:ValidationSummary ID="vsFooterLabor" runat="server" ShowMessageBox="True" ShowSummary="true"
                                                ValidationGroup="vgFooterLabor" />
                                            <asp:ValidationSummary ID="vsEditLabor" runat="server" ShowMessageBox="True" ShowSummary="true"
                                                ValidationGroup="vgEditLabor" />
                                            <br />
                                            <asp:Button runat="server" ID="btnRemoveLabor" Text="Remove All Labor" Visible="false" />
                                            <asp:GridView runat="server" ID="gvLabor" Width="100%" AllowPaging="True" AllowSorting="True"
                                                AutoGenerateColumns="False" DataSourceID="odsCostSheetLabor" PageSize="15" ShowFooter="True"
                                                DataKeyNames="RowID">
                                                <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                                <EditRowStyle BackColor="#CCCCCC" />
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
                                                <Columns>
                                                    <asp:BoundField DataField="CostSheetID" SortExpression="CostSheetID" />
                                                    <asp:TemplateField HeaderText="Description" SortExpression="ddLaborDesc">
                                                        <EditItemTemplate>
                                                            <asp:Label ID="lblEditLaborID" runat="server" CssClass="none" Text='<%# Bind("LaborID") %>'></asp:Label>
                                                            <asp:Label ID="lblEditLaborDesc" runat="server" Text='<%# Bind("ddLaborDesc") %>'></asp:Label>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewLaborID" runat="server" CssClass="none" Text='<%# Bind("LaborID") %>'></asp:Label>
                                                            <asp:Label ID="lblViewLaborDesc" runat="server" Text='<%# Bind("ddLaborDesc") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddFooterLabor" runat="server" DataSource='<%# CostingModule.GetLabor(0,"",False,False) %>'
                                                                DataValueField="LaborID" DataTextField="ddLaborDesc" AppendDataBoundItems="True"
                                                                OnSelectedIndexChanged="ddFooterLabor_SelectedIndexChanged" AutoPostBack="true"
                                                                SelectedValue='<%# Bind("LaborID") %>'>
                                                                <asp:ListItem Text="" Value="0" Selected="False"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvFooterLabor" runat="server" ControlToValidate="ddFooterLabor"
                                                                ErrorMessage="The description is required." Font-Bold="True" ValidationGroup="vgFooterLabor"
                                                                Text="<" SetFocusOnError="true" />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Rate" SortExpression="Rate">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditLaborRate" runat="server" Text='<%# Bind("Rate") %>' MaxLength="10"
                                                                Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditLaborRate" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditLabor" Type="double" Text="<" ControlToValidate="txtEditLaborRate"
                                                                ErrorMessage="Rate must be a number." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewLaborRate" runat="server" Text='<%# Bind("Rate") %>'></asp:Label>
                                                            <asp:Label ID="lblViewLaborNewRate" runat="server" Text='<%# Bind("isNewRate") %>'
                                                                Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtFooterLaborRate" runat="server" Text='<%# Bind("Rate") %>' MaxLength="10"
                                                                Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvFooterLaborRate" Operator="DataTypeCheck"
                                                                ValidationGroup="vgFooterLabor" Type="double" Text="<" ControlToValidate="txtFooterLaborRate"
                                                                ErrorMessage="Rate must be a number." SetFocusOnError="True" />
                                                        </FooterTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Crew Size" SortExpression="CrewSize">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditLaborCrewSize" runat="server" Text='<%# Bind("CrewSize") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditCrewSize" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditLabor" Type="double" Text="<" ControlToValidate="txtEditLaborCrewSize"
                                                                ErrorMessage="Crew size must be a number." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewLaborCrewSize" runat="server" Text='<%# Bind("CrewSize") %>'></asp:Label>
                                                            <asp:Label ID="lblViewLaborNewCrewSize" runat="server" Text='<%# Bind("isNewCrewSize") %>'
                                                                Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtFooterLaborCrewSize" runat="server" Text='<%# Bind("CrewSize") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvFooterCrewSize" Operator="DataTypeCheck"
                                                                ValidationGroup="vgFooterLabor" Type="double" Text="<" ControlToValidate="txtFooterLaborCrewSize"
                                                                ErrorMessage="Crew size must be a number." SetFocusOnError="True" />
                                                        </FooterTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Offline" SortExpression="isOffline">
                                                        <EditItemTemplate>
                                                            <asp:CheckBox ID="cbEditLaborIsOffline" runat="server" Checked='<%# Bind("isOffline") %>' />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cbViewLaborIsOffline" runat="server" Checked='<%# Bind("isOffline") %>'
                                                                Enabled="false" />
                                                            <asp:Label ID="lblViewLaborNewOffline" runat="server" Text='<%# Bind("isNewOffline") %>'
                                                                Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:CheckBox ID="cbFooterLaborIsOffline" runat="server" Checked='<%# Bind("isOffline") %>' />
                                                        </FooterTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Standard Cost<br>Per Unit<br> W/O Scrap" SortExpression="StandardCostPerUnit">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditLaborStandardCostPerUnitWOScrap" runat="server" Text='<%# Bind("StandardCostPerUnitWOScrap") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditLaborStandardCostPerUnitWOScrap" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditLabor" Type="double" Text="<" ControlToValidate="txtEditLaborStandardCostPerUnitWOScrap"
                                                                ErrorMessage="Standard Cost must be a number." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewLaborStandardCostPerUnitWOScrap" runat="server" Text='<%# Bind("StandardCostPerUnitWOScrap") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Standard Cost<br> Factor (Scrap)" SortExpression="StandardCostFactor">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditLaborStandardCostFactor" runat="server" Text='<%# Bind("StandardCostFactor") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditLaborStandardCostFactor" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditLabor" Type="double" Text="<" ControlToValidate="txtEditLaborStandardCostFactor"
                                                                ErrorMessage="Standard cost factor must be a number." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewLaborStandardCostFactor" runat="server" Text='<%# Bind("StandardCostFactor") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtFooterLaborStandardCostFactor" runat="server" Text='<%# Bind("StandardCostFactor") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvFooterLaborStandardCostFactor" Operator="DataTypeCheck"
                                                                ValidationGroup="vgFooterLabor" Type="double" Text="<" ControlToValidate="txtFooterLaborStandardCostFactor"
                                                                ErrorMessage="Standard cost factor must be a number." SetFocusOnError="True" />
                                                        </FooterTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Standard Cost<br> Per Unit<br> WITH Scrap" SortExpression="StandardCostPerUnit">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditLaborStandardCostPerUnit" runat="server" Text='<%# Bind("StandardCostPerUnit") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditLaborStandardCostPerUnit" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditLabor" Type="double" Text="<" ControlToValidate="txtEditLaborStandardCostPerUnit"
                                                                ErrorMessage="Standard Cost must be a number." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewLaborStandardCostPerUnit" runat="server" Text='<%# Bind("StandardCostPerUnit") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Ordinal" SortExpression="Ordinal">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditLaborOrdinal" runat="server" Text='<%# Bind("Ordinal") %>'
                                                                MaxLength="2" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditLaborOrdinal" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditLabor" Type="integer" Text="<" ControlToValidate="txtEditLaborOrdinal"
                                                                ErrorMessage="Ordinal must be an integer." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewLaborOrdinal" runat="server" Text='<%# Bind("Ordinal") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtFooterLaborOrdinal" runat="server" Text="99" MaxLength="2" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvFooterLaborOrdinal" Operator="DataTypeCheck"
                                                                ValidationGroup="vgFooterLabor" Type="integer" Text="<" ControlToValidate="txtFooterLaborOrdinal"
                                                                ErrorMessage="Ordinal must be an integer." SetFocusOnError="True" />
                                                        </FooterTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <EditItemTemplate>
                                                            <asp:ImageButton ID="iBtnLaborUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                                                ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditLabor" />
                                                            <asp:ImageButton ID="iBtnLaborCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                                                ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="iBtnLaborEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                                                ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                                                            <asp:ImageButton ID="ibtnLaborDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                                ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgFooterLabor"
                                                                runat="server" ID="iBtnLaborInsert" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                                                            <asp:ImageButton ID="iBtnLaborUndo" runat="server" CommandName="Undo" CausesValidation="false"
                                                                ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <asp:ObjectDataSource ID="odsCostSheetLabor" runat="server" OldValuesParameterFormatString="original_{0}"
                                                SelectMethod="GetCostSheetLabor" TypeName="CostSheetLaborBLL" DeleteMethod="DeleteCostSheetLabor"
                                                UpdateMethod="UpdateCostSheetLabor" InsertMethod="InsertCostSheetLabor">
                                                <SelectParameters>
                                                    <asp:QueryStringParameter Name="CostSheetID" QueryStringField="CostSheetID" Type="Int32" />
                                                    <asp:Parameter DefaultValue="0" Name="LaborID" Type="Int32" />
                                                    <asp:Parameter DefaultValue="False" Name="filterOffline" Type="Boolean" />
                                                    <asp:Parameter Name="isOffline" Type="Boolean" DefaultValue="False" />
                                                </SelectParameters>
                                                <DeleteParameters>
                                                    <asp:Parameter Name="RowID" Type="Int32" />
                                                    <asp:Parameter Name="original_RowID" Type="Int32" />
                                                </DeleteParameters>
                                                <UpdateParameters>
                                                    <asp:Parameter Name="Rate" Type="Double" />
                                                    <asp:Parameter Name="CrewSize" Type="Double" />
                                                    <asp:Parameter Name="Ordinal" Type="Int32" />
                                                    <asp:Parameter Name="isOffline" Type="Boolean" />
                                                    <asp:Parameter Name="StandardCostFactor" Type="Double" />
                                                    <asp:Parameter Name="StandardCostPerUnitWOScrap" Type="Double" />
                                                    <asp:Parameter Name="StandardCostPerUnit" Type="Double" />
                                                    <asp:Parameter Name="original_RowID" Type="Int32" />
                                                    <asp:Parameter Name="LaborID" Type="Int32" />
                                                    <asp:Parameter Name="ddLaborDesc" Type="String" />
                                                    <asp:Parameter Name="CostSheetID" Type="Int32" />
                                                </UpdateParameters>
                                                <InsertParameters>
                                                    <asp:Parameter Name="CostSheetID" Type="Int32" />
                                                    <asp:Parameter Name="LaborID" Type="Int32" />
                                                    <asp:Parameter Name="Rate" Type="Double" />
                                                    <asp:Parameter Name="CrewSize" Type="Double" />
                                                    <asp:Parameter Name="StandardCostFactor" Type="Double" />
                                                    <asp:Parameter Name="Ordinal" Type="Int32" />
                                                    <asp:Parameter Name="isOffline" Type="Boolean" />
                                                </InsertParameters>
                                            </asp:ObjectDataSource>
                                        </asp:View>
                                        <asp:View ID="vOverhead" runat="server">
                                            <asp:Label runat="server" ID="lblMessageOverhead"></asp:Label><br />
                                            <asp:Label runat="server" ID="lblOverheadTip" SkinID="MessageLabelSkin" Text="When viewing the Rate, Crew Size, and Offline below:<br>If the background color is yellow, then on the Overhead Maintenance page, the value has been updated and does not match the value listed below."></asp:Label><br />
                                            <asp:ValidationSummary ID="vsFooterOverhead" runat="server" ShowMessageBox="True"
                                                ShowSummary="true" ValidationGroup="vgFooterOverhead" />
                                            <asp:ValidationSummary ID="vsEditOverhead" runat="server" ShowMessageBox="True" ShowSummary="true"
                                                ValidationGroup="vgEditOverhead" />
                                            <br />
                                            <asp:Button runat="server" ID="btnRemoveOverhead" Text="Remove All Overhead" Visible="false" />
                                            <asp:GridView runat="server" ID="gvOverhead" Width="100%" DataSourceID="odsCostSheetOverhead"
                                                AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" PageSize="15"
                                                ShowFooter="True" DataKeyNames="RowID">
                                                <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                                <EditRowStyle BackColor="#CCCCCC" />
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
                                                <Columns>
                                                    <asp:BoundField DataField="CostSheetID" HeaderText="CostSheetID" SortExpression="CostSheetID"
                                                        ShowHeader="False" />
                                                    <asp:TemplateField HeaderText="Description" SortExpression="ddLaborDesc">
                                                        <EditItemTemplate>
                                                            <asp:Label ID="lblEditOverheadID" runat="server" CssClass="none" Text='<%# Bind("LaborID") %>'></asp:Label>
                                                            <asp:Label ID="lblEditOverheadDesc" runat="server" Text='<%# Bind("ddLaborDesc") %>'></asp:Label>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewOverheadID" runat="server" CssClass="none" Text='<%# Bind("LaborID") %>'></asp:Label>
                                                            <asp:Label ID="lblViewOverheadDesc" runat="server" Text='<%# Bind("ddLaborDesc") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddFooterOverhead" runat="server" DataSource='<%# CostingModule.GetOverhead(0,"") %>'
                                                                DataValueField="LaborID" DataTextField="ddLaborDesc" AppendDataBoundItems="True"
                                                                OnSelectedIndexChanged="ddFooterOverhead_SelectedIndexChanged" AutoPostBack="true"
                                                                SelectedValue='<%# Bind("LaborID") %>'>
                                                                <asp:ListItem Text="" Value="0" Selected="False"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvFooterOverhead" runat="server" ControlToValidate="ddFooterOverhead"
                                                                ErrorMessage="The description is required." Font-Bold="True" ValidationGroup="vgFooterOverhead"
                                                                Text="<" SetFocusOnError="true" />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Fixed Rate" SortExpression="Rate">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditOverheadRate" runat="server" Text='<%# Bind("Rate") %>' MaxLength="10"
                                                                Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditOverheadRate" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditOverhead" Type="double" Text="<" ControlToValidate="txtEditOverheadRate"
                                                                ErrorMessage="Rate must be a number." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewOverheadRate" runat="server" Text='<%# Bind("Rate") %>'></asp:Label>
                                                            <asp:Label ID="lblViewOverheadNewRate" runat="server" Text='<%# Bind("isNewRate") %>'
                                                                Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtFooterOverheadRate" runat="server" Text='<%# Bind("Rate") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvFooterOverheadRate" Operator="DataTypeCheck"
                                                                ValidationGroup="vgFooterOverhead" Type="double" Text="<" ControlToValidate="txtFooterOverheadRate"
                                                                ErrorMessage="Rate must be a number." SetFocusOnError="True" />
                                                        </FooterTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Variable Rate" SortExpression="VariableRate">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditOverheadVariableRate" runat="server" Text='<%# Bind("VariableRate") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditOverheadVariableRate" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditOverhead" Type="double" Text="<" ControlToValidate="txtEditOverheadVariableRate"
                                                                ErrorMessage="Variable Rate must be a number." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewOverheadVariableRate" runat="server" Text='<%# Bind("VariableRate") %>'></asp:Label>
                                                            <asp:Label ID="lblViewOverheadNewVariableRate" runat="server" Text='<%# Bind("isNewVariableRate") %>'
                                                                Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtFooterOverheadVariableRate" runat="server" Text='<%# Bind("VariableRate") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvFooterOverheadVariableRate" Operator="DataTypeCheck"
                                                                ValidationGroup="vgFooterOverhead" Type="double" Text="<" ControlToValidate="txtFooterOverheadVariableRate"
                                                                ErrorMessage="VariableRate must be a number." SetFocusOnError="True" />
                                                        </FooterTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Crew Size" SortExpression="CrewSize">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditOverheadCrewSize" runat="server" Text='<%# Bind("CrewSize") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditOverheadCrewSize" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditOverhead" Type="double" Text="<" ControlToValidate="txtEditOverheadCrewSize"
                                                                ErrorMessage="Crew size must be a number." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewOverheadCrewSize" runat="server" Text='<%# Bind("CrewSize") %>'></asp:Label>
                                                            <asp:Label ID="lblViewOverheadNewCrewSize" runat="server" Text='<%# Bind("isNewCrewSize") %>'
                                                                Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtFooterOverheadCrewSize" runat="server" Text='<%# Bind("CrewSize") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvFooterOverheadCrewSize" Operator="DataTypeCheck"
                                                                ValidationGroup="vgFooterOverhead" Type="double" Text="<" ControlToValidate="txtFooterOverheadCrewSize"
                                                                ErrorMessage="Crew size must be a number." SetFocusOnError="True" />
                                                        </FooterTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Nbr Carriers" SortExpression="NumberOfCarriers">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditOverheadNumberOfCarriers" runat="server" Text='<%# Bind("NumberOfCarriers") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditOverheadNumberOfCarriers" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditOverhead" Type="double" Text="<" ControlToValidate="txtEditOverheadNumberOfCarriers"
                                                                ErrorMessage="Number Of Carriers must be a number." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewOverheadNumberOfCarriers" runat="server" Text='<%# Bind("NumberOfCarriers") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtFooterOverheadNumberOfCarriers" runat="server" Text='<%# Bind("NumberOfCarriers") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvFooterOverheadNumberOfCarriers" Operator="DataTypeCheck"
                                                                ValidationGroup="vgFooterOverhead" Type="double" Text="<" ControlToValidate="txtFooterOverheadNumberOfCarriers"
                                                                ErrorMessage="Number Of carriers must be a number." SetFocusOnError="True" />
                                                        </FooterTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Offline" SortExpression="isOffline">
                                                        <EditItemTemplate>
                                                            <asp:CheckBox ID="cbEditOverheadIsOffline" runat="server" Checked='<%# Bind("isOffline") %>' />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cbViewOverheadIsOffline" runat="server" Checked='<%# Bind("isOffline") %>'
                                                                Enabled="false" />
                                                            <asp:Label ID="lblViewOverheadNewOffline" runat="server" Text='<%# Bind("isNewOffline") %>'
                                                                Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:CheckBox ID="cbFooterOverheadIsOffline" runat="server" Checked='<%# Bind("isOffline") %>' />
                                                        </FooterTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Proportion" SortExpression="isProportion">
                                                        <EditItemTemplate>
                                                            <asp:CheckBox ID="cbEditOverheadIsProportion" runat="server" Checked='<%# Bind("isProportion") %>' />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cbEditOverheadIsProportion" runat="server" Checked='<%# Bind("isProportion") %>'
                                                                Enabled="false" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:CheckBox ID="cbFooterOverheadIsProportion" runat="server" Checked='<%# Bind("isProportion") %>' />
                                                        </FooterTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="StandardCostPerUnitWOScrapFixedRate" ShowHeader="False" />
                                                    <asp:BoundField DataField="StandardCostPerUnitWOScrapVariableRate" ShowHeader="False" />
                                                    <asp:TemplateField HeaderText="Standard Cost<br>Per Unit<br> W/O Scrap" SortExpression="StandardCostPerUnitWOScrap">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditOverheadStandardCostPerUnitWOScrap" runat="server" Text='<%# Bind("StandardCostPerUnitWOScrap") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditOverheadStandardCostPerUnitWOScrap"
                                                                Operator="DataTypeCheck" ValidationGroup="vgEditOverhead" Type="double" Text="<"
                                                                ControlToValidate="txtEditOverheadStandardCostPerUnitWOScrap" ErrorMessage="Standard Cost must be a number."
                                                                SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewOverheadStandardCostPerUnitWOScrap" runat="server" Text='<%# Bind("StandardCostPerUnitWOScrap") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Standard Cost<br> Factor (Scrap)" SortExpression="StandardCostFactor">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditOverheadStandardCostFactor" runat="server" Text='<%# Bind("StandardCostFactor") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditOverheadStandardCostFactor" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditOverhead" Type="double" Text="<" ControlToValidate="txtEditOverheadStandardCostFactor"
                                                                ErrorMessage="Standard cost factor must be a number." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewOverheadStandardCostFactor" runat="server" Text='<%# Bind("StandardCostFactor") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtFooterOverheadStandardCostFactor" runat="server" Text='<%# Bind("StandardCostFactor") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvFooterOverheadStandardCostFactor" Operator="DataTypeCheck"
                                                                ValidationGroup="vgFooterOverhead" Type="double" Text="<" ControlToValidate="txtFooterOverheadStandardCostFactor"
                                                                ErrorMessage="Standard cost factor must be a number." SetFocusOnError="True" />
                                                        </FooterTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="StandardCostPerUnitFixedRate" ShowHeader="False" />
                                                    <asp:BoundField DataField="StandardCostPerUnitVariableRate" ShowHeader="False" />
                                                    <asp:TemplateField HeaderText="Standard Cost<br>Per Unit<br> WITH Scrap" SortExpression="StandardCostPerUnit">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditOverheadStandardCostPerUnit" runat="server" Text='<%# Bind("StandardCostPerUnit") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditOverheadStandardCostPerUnit" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditOverhead" Type="double" Text="<" ControlToValidate="txtEditOverheadStandardCostPerUnit"
                                                                ErrorMessage="Standard Cost must be a number." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewOverheadStandardCostPerUnit" runat="server" Text='<%# Bind("StandardCostPerUnit") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Ordinal" SortExpression="Ordinal">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditOverheadOrdinal" runat="server" Text='<%# Bind("Ordinal") %>'
                                                                MaxLength="2" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditOverheadOrdinal" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditOverhead" Type="integer" Text="<" ControlToValidate="txtEditOverheadOrdinal"
                                                                ErrorMessage="Ordinal must be an integer." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewOverheadOrdinal" runat="server" Text='<%# Bind("Ordinal") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtFooterOverheadOrdinal" runat="server" Text="99" MaxLength="2"
                                                                Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvFooterOverheadOrdinal" Operator="DataTypeCheck"
                                                                ValidationGroup="vgFooterOverhead" Type="integer" Text="<" ControlToValidate="txtFooterOverheadOrdinal"
                                                                ErrorMessage="Ordinal must be an integer." SetFocusOnError="True" />
                                                        </FooterTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <EditItemTemplate>
                                                            <asp:ImageButton ID="iBtnOverheadUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                                                ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditOverhead" />
                                                            <asp:ImageButton ID="iBtnOverheadCancel" runat="server" CausesValidation="False"
                                                                CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="iBtnOverheadEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                                                ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                                                            <asp:ImageButton ID="ibtnOverheadDelete" runat="server" CausesValidation="False"
                                                                CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgFooterOverhead"
                                                                runat="server" ID="iBtnOverheadInsert" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                                                            <asp:ImageButton ID="iBtnOverheadUndo" runat="server" CommandName="Undo" CausesValidation="false"
                                                                ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <asp:ObjectDataSource ID="odsCostSheetOverhead" runat="server" OldValuesParameterFormatString="original_{0}"
                                                SelectMethod="GetCostSheetOverhead" TypeName="CostSheetOverheadBLL" DeleteMethod="DeleteCostSheetOverhead"
                                                UpdateMethod="UpdateCostSheetOverhead" InsertMethod="InsertCostSheetOverhead">
                                                <SelectParameters>
                                                    <asp:QueryStringParameter Name="CostSheetID" QueryStringField="CostSheetID" Type="Int32" />
                                                    <asp:Parameter Name="LaborID" Type="Int32" />
                                                </SelectParameters>
                                                <DeleteParameters>
                                                    <asp:Parameter Name="RowID" Type="Int32" />
                                                    <asp:Parameter Name="original_RowID" Type="Int32" />
                                                </DeleteParameters>
                                                <UpdateParameters>
                                                    <asp:Parameter Name="LaborID" Type="Int32" />
                                                    <asp:Parameter Name="Rate" Type="Double" />
                                                    <asp:Parameter Name="VariableRate" Type="Double" />
                                                    <asp:Parameter Name="CrewSize" Type="Double" />
                                                    <asp:Parameter Name="NumberofCarriers" Type="Double" />
                                                    <asp:Parameter Name="Ordinal" Type="Int32" />
                                                    <asp:Parameter Name="isOffline" Type="Boolean" />
                                                    <asp:Parameter Name="isProportion" Type="Boolean" />
                                                    <asp:Parameter Name="StandardCostFactor" Type="Double" />
                                                    <asp:Parameter Name="StandardCostPerUnitWOScrapFixedRate" Type="Double" />
                                                    <asp:Parameter Name="StandardCostPerUnitWOScrapVariableRate" Type="Double" />
                                                    <asp:Parameter Name="StandardCostPerUnitWOScrap" Type="Double" />
                                                    <asp:Parameter Name="StandardCostPerUnitFixedRate" Type="Double" />
                                                    <asp:Parameter Name="StandardCostPerUnitVariableRate" Type="Double" />
                                                    <asp:Parameter Name="StandardCostPerUnit" Type="Double" />
                                                    <asp:Parameter Name="original_RowID" Type="Int32" />
                                                    <asp:Parameter Name="ddLaborDesc" Type="String" />
                                                    <asp:Parameter Name="CostSheetID" Type="Int32" />
                                                </UpdateParameters>
                                                <InsertParameters>
                                                    <asp:Parameter Name="CostSheetID" Type="Int32" />
                                                    <asp:Parameter Name="LaborID" Type="Int32" />
                                                    <asp:Parameter Name="Rate" Type="Double" />
                                                    <asp:Parameter Name="VariableRate" Type="Double" />
                                                    <asp:Parameter Name="CrewSize" Type="Double" />
                                                    <asp:Parameter Name="StandardCostFactor" Type="Double" />
                                                    <asp:Parameter Name="Ordinal" Type="Int32" />
                                                    <asp:Parameter Name="isOffline" Type="Boolean" />
                                                    <asp:Parameter Name="isProportion" Type="Boolean" />
                                                </InsertParameters>
                                            </asp:ObjectDataSource>
                                        </asp:View>
                                        <asp:View ID="vMiscCosts" runat="server">
                                            <asp:Label runat="server" ID="lblMessageMiscCost"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="lblMiscCostTip" SkinID="MessageLabelSkin" Text="When viewing the Rate below:<br>If the background color is yellow, then on the Misc Cost Maintenance page, the value has been updated and does not match the value listed below."></asp:Label><br />
                                            <asp:ValidationSummary ID="vsFooterMiscCost" runat="server" ShowMessageBox="True"
                                                ShowSummary="true" ValidationGroup="vgFooterMiscCost" />
                                            <asp:ValidationSummary ID="vsEditMiscCost" runat="server" ShowMessageBox="True" ShowSummary="true"
                                                ValidationGroup="vgEditMiscCost" />
                                            <br />
                                            <asp:Button runat="server" ID="btnRemoveMiscCost" Text="Remove All Misc Costs" Visible="false" />
                                            <asp:GridView runat="server" ID="gvMiscCost" Width="100%" DataSourceID="odsCostSheetMiscCost"
                                                AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" PageSize="15"
                                                ShowFooter="True" DataKeyNames="RowID">
                                                <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                                <EditRowStyle BackColor="#CCCCCC" />
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
                                                <Columns>
                                                    <asp:BoundField DataField="CostSheetID" ShowHeader="False" SortExpression="CostSheetID" />
                                                    <asp:TemplateField HeaderText="Description" SortExpression="ddMiscCostDesc">
                                                        <EditItemTemplate>
                                                            <asp:Label ID="lblEditMiscCostID" runat="server" CssClass="none" Text='<%# Bind("MiscCostID") %>'></asp:Label>
                                                            <asp:Label ID="lblEditMiscCostDesc" runat="server" Text='<%# Bind("ddMiscCostDesc") %>'></asp:Label>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewMiscCostID" runat="server" CssClass="none" Text='<%# Bind("MiscCostID") %>'></asp:Label>
                                                            <asp:Label ID="lblViewMiscCostDesc" runat="server" Text='<%# Bind("ddMiscCostDesc") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddFooterMiscCostID" runat="server" DataSource='<%# CostingModule.GetMiscCost(0,"") %>'
                                                                DataValueField="MiscCostID" DataTextField="ddMiscCostDesc" AppendDataBoundItems="True"
                                                                OnSelectedIndexChanged="ddFooterMiscCost_SelectedIndexChanged" AutoPostBack="true"
                                                                SelectedValue='<%# Bind("MiscCostID") %>'>
                                                                <asp:ListItem Text="" Value="0" Selected="False"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvFooterMiscCostID" runat="server" ControlToValidate="ddFooterMiscCostID"
                                                                ErrorMessage="The description is required." Font-Bold="True" ValidationGroup="vgFooterMiscCost"
                                                                Text="<" SetFocusOnError="true" />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Rate" SortExpression="Rate">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditMiscCostRate" runat="server" Text='<%# Bind("Rate") %>' MaxLength="10"
                                                                Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditMiscCostRate" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditMiscCost" Type="double" Text="<" ControlToValidate="txtEditMiscCostRate"
                                                                ErrorMessage="Rate be a number." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewMiscCostRate" runat="server" Text='<%# Bind("Rate") %>'></asp:Label>
                                                            <asp:Label ID="lblViewMiscCostNewRate" runat="server" Text='<%# Bind("isNewRate") %>'
                                                                Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtFooterMiscCostRate" runat="server" Text='<%# Bind("Rate") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvFooterMiscCostRate" Operator="DataTypeCheck"
                                                                ValidationGroup="vgFooterMiscCost" Type="double" Text="<" ControlToValidate="txtFooterMiscCostRate"
                                                                ErrorMessage="Rate be a number." SetFocusOnError="True" />
                                                        </FooterTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cost" SortExpression="Cost">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditMiscCost" runat="server" Text='<%# Bind("Cost") %>' MaxLength="10"
                                                                Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditMiscCost" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditMiscCost" Type="double" Text="<" ControlToValidate="txtEditMiscCost"
                                                                ErrorMessage="Cost be a number." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEditMiscCost" runat="server" Text='<%# Bind("Cost") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtFooterMiscCost" runat="server" Text='<%# Bind("Cost") %>' MaxLength="10"
                                                                Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvFooterMiscCost" Operator="DataTypeCheck"
                                                                ValidationGroup="vgFooterMiscCost" Type="double" Text="<" ControlToValidate="txtFooterMiscCost"
                                                                ErrorMessage="Cost be a number." SetFocusOnError="True" />
                                                        </FooterTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amort Volume" SortExpression="AmortVolume">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditMiscCostAmortVolume" runat="server" Text='<%# Bind("AmortVolume") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditMiscCostAmortVolume" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditMiscCost" Type="integer" Text="<" ControlToValidate="txtEditMiscCostAmortVolume"
                                                                ErrorMessage="Amort Volume be an integer." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewMiscCostAmortVolume" runat="server" Text='<%# Bind("AmortVolume") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtFooterMiscCostAmortVolume" runat="server" Text='<%# Bind("AmortVolume") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvFooterMiscCostAmortVolume" Operator="DataTypeCheck"
                                                                ValidationGroup="vgFooterMiscCost" Type="integer" Text="<" ControlToValidate="txtFooterMiscCostAmortVolume"
                                                                ErrorMessage="Amort Volume be an integer." SetFocusOnError="True" />
                                                        </FooterTemplate>
                                                        <HeaderStyle HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Standard Cost" SortExpression="StandardCostPerUnit">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditMiscCostStandardCostPerUnit" runat="server" Text='<%# Bind("StandardCostPerUnit") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditMiscCostStandardCostPerUnit" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditMiscCost" Type="double" Text="<" ControlToValidate="txtEditMiscCostStandardCostPerUnit"
                                                                ErrorMessage="Standard Cost must be a number." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewMiscCostStandardCostPerUnit" runat="server" Text='<%# Bind("StandardCostPerUnit") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Ordinal" SortExpression="Ordinal">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditMiscCostOrdinal" runat="server" Text='<%# Bind("Ordinal") %>'
                                                                MaxLength="2" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditMiscCostOrdinal" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditMiscCost" Type="integer" Text="<" ControlToValidate="txtEditMiscCostOrdinal"
                                                                ErrorMessage="Ordinal be an integer." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewMiscCostOrdinal" runat="server" Text='<%# Bind("Ordinal") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtFooterMiscCostOrdinal" runat="server" Text="99" MaxLength="2"
                                                                Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvFooterMiscCostOrdinal" Operator="DataTypeCheck"
                                                                ValidationGroup="vgFooterMiscCost" Type="integer" Text="<" ControlToValidate="txtFooterMiscCostOrdinal"
                                                                ErrorMessage="Ordinal be an integer." SetFocusOnError="True" />
                                                        </FooterTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <EditItemTemplate>
                                                            <asp:ImageButton ID="iBtnMiscCostUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                                                ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditMiscCost" />
                                                            <asp:ImageButton ID="iBtnMiscCostCancel" runat="server" CausesValidation="False"
                                                                CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="iBtnMiscCostEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                                                ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                                                            <asp:ImageButton ID="ibtnMiscCostDelete" runat="server" CausesValidation="False"
                                                                CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgFooterMiscCost"
                                                                runat="server" ID="iBtnFooterMiscCost" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                                                            <asp:ImageButton ID="iBtnMiscCostUndo" runat="server" CommandName="Undo" CausesValidation="false"
                                                                ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <asp:ObjectDataSource ID="odsCostSheetMiscCost" runat="server" OldValuesParameterFormatString="original_{0}"
                                                SelectMethod="GetCostSheetMiscCost" TypeName="CostSheetMiscCostBLL" DeleteMethod="DeleteCostSheetMiscCost"
                                                UpdateMethod="UpdateCostSheetMiscCost" InsertMethod="InsertCostSheetMiscCost">
                                                <SelectParameters>
                                                    <asp:QueryStringParameter Name="costSheetID" QueryStringField="CostSheetID" Type="Int32" />
                                                    <asp:Parameter Name="MiscCostID" Type="Int32" />
                                                </SelectParameters>
                                                <DeleteParameters>
                                                    <asp:Parameter Name="RowID" Type="Int32" />
                                                    <asp:Parameter Name="original_RowID" Type="Int32" />
                                                </DeleteParameters>
                                                <UpdateParameters>
                                                    <asp:Parameter Name="MiscCostID" Type="Int32" />
                                                    <asp:Parameter Name="Rate" Type="Double" />
                                                    <asp:Parameter Name="QuoteRate" Type="Double" />
                                                    <asp:Parameter Name="Cost" Type="Double" />
                                                    <asp:Parameter Name="AmortVolume" Type="Int32" />
                                                    <asp:Parameter Name="StandardCostPerUnit" Type="Double" />
                                                    <asp:Parameter Name="isPiecesPerHour" Type="Boolean" />
                                                    <asp:Parameter Name="isPiecesPerYear" Type="Boolean" />
                                                    <asp:Parameter Name="isPiecesPerContainer" Type="Boolean" />
                                                    <asp:Parameter Name="Ordinal" Type="Int32" />
                                                    <asp:Parameter Name="original_RowID" Type="Int32" />
                                                    <asp:Parameter Name="ddMiscCostDesc" Type="String" />
                                                    <asp:Parameter Name="CostSheetID" Type="Int32" />
                                                </UpdateParameters>
                                                <InsertParameters>
                                                    <asp:Parameter Name="CostSheetID" Type="Int32" />
                                                    <asp:Parameter Name="MiscCostID" Type="Int32" />
                                                    <asp:Parameter Name="Rate" Type="Double" />
                                                    <asp:Parameter Name="QuoteRate" Type="Double" />
                                                    <asp:Parameter Name="Cost" Type="Double" />
                                                    <asp:Parameter Name="AmortVolume" Type="Int32" />
                                                    <asp:Parameter Name="isPiecesPerHour" Type="Boolean" />
                                                    <asp:Parameter Name="isPiecesPerYear" Type="Boolean" />
                                                    <asp:Parameter Name="isPiecesPerContainer" Type="Boolean" />
                                                    <asp:Parameter Name="Ordinal" Type="Int32" />
                                                </InsertParameters>
                                            </asp:ObjectDataSource>
                                        </asp:View>
                                        <asp:View ID="vDrawings" runat="server">
                                            <asp:Label runat="server" ID="lblMessageDrawings" Text=""></asp:Label>
                                            <table width="100%">
                                                <tr>
                                                    <td class="c_text">
                                                        Part Sketch:
                                                    </td>
                                                    <td class="c_text">
                                                        Sketch Memo:
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top">
                                                        <asp:FileUpload runat="server" ID="uploadImage" Enabled="false" />
                                                        <asp:Button ID="btnSaveUploadDrawingPartSketchImage" runat="server" Text="Upload"
                                                            Enabled="false"></asp:Button>
                                                        <br />
                                                        <asp:Button ID="btnDeleteDrawingPartSketchImage" CssClass="stdbutton" Text="Delete"
                                                            Enabled="false" runat="Server" Font-Size="8pt" />
                                                        &nbsp;
                                                        <img id="imgDrawingPartSketch" runat="server" alt="DrawingPartSketch" style="border: 0"
                                                            width="300" height="300" src="" />
                                                        <asp:LinkButton runat="server" ID="lnkShowLargerSketchImage" Text="Click here to view larger image"
                                                            Enabled="false"></asp:LinkButton>
                                                    </td>
                                                    <td valign="top">
                                                        <asp:TextBox runat="server" ID="txtDrawingPartSketchMemo" Width="300px" Height="200px"
                                                            Enabled="false" TextMode="MultiLine"></asp:TextBox>
                                                        <br />
                                                        <asp:Label ID="lblDrawingPartSketchMemoCharCount" SkinID="MessageLabelSkin" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:View>
                                        <asp:View ID="vCompositePartSpec" runat="server">
                                            <asp:Label runat="server" ID="lblMessageCompositePartSpec" Text=""></asp:Label>
                                            <table>
                                                <tr>
                                                    <td class="p_text">
                                                        Formula:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddCompositePartSpecFormula" runat="server" Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Part Thickness:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtCompositePartSpecPartThicknessValue" MaxLength="10"
                                                            Enabled="false"></asp:TextBox>
                                                        <asp:CompareValidator runat="server" ID="cvCompositePartSpecPartThicknessValue" Operator="DataTypeCheck"
                                                            ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtCompositePartSpecPartThicknessValue"
                                                            ErrorMessage="Composite part thickness must be a number." SetFocusOnError="True" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddCompositePartSpecPartThicknessUnits" Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Part Specific Gravity:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtCompositePartSpecPartSpecificGravityValue" MaxLength="10"
                                                            Enabled="false"></asp:TextBox>
                                                        <asp:CompareValidator runat="server" ID="cvCompositePartSpecPartSpecificGravityValue"
                                                            Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtCompositePartSpecPartSpecificGravityValue"
                                                            ErrorMessage="Composite part specific gravity must be a number." SetFocusOnError="True" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddCompositePartSpecPartSpecificGravityUnits"
                                                            Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Part Area:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtCompositePartSpecPartAreaValue" MaxLength="10"
                                                            Enabled="false"></asp:TextBox>
                                                        <asp:CompareValidator runat="server" ID="cvCompositePartSpecPartAreaValue" Operator="DataTypeCheck"
                                                            ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtCompositePartSpecPartAreaValue"
                                                            ErrorMessage="Composite part area must be a number." SetFocusOnError="True" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddCompositePartSpecPartAreaUnits" Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        RSS Weight:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtCompositePartSpecRSSWeightValue" MaxLength="10"
                                                            Enabled="false"></asp:TextBox>
                                                        <asp:CompareValidator runat="server" ID="cvCompositePartSpecRSSWeightValue" Operator="DataTypeCheck"
                                                            ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtCompositePartSpecRSSWeightValue"
                                                            ErrorMessage="Composite part RSS weight must be a number." SetFocusOnError="True" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddCompositePartSpecRSSWeightUnits" Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Anti-Block Coating:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtCompositePartSpecAntiBlockCoatingValue" MaxLength="10"
                                                            Enabled="false"></asp:TextBox>
                                                        <asp:CompareValidator runat="server" ID="cvCompositePartSpecAntiBlockCoatingValue"
                                                            Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtCompositePartSpecAntiBlockCoatingValue"
                                                            ErrorMessage="Composite part anti-block coating must be a number." SetFocusOnError="True" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddCompositePartSpecAntiBlockCoatingUnits" Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Hot Meld Adhesive:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtCompositePartSpecHotMeldAdhesiveValue" MaxLength="10"
                                                            Enabled="false"></asp:TextBox>
                                                        <asp:CompareValidator runat="server" ID="cvCompositePartSpecHotMeldAdhesiveValue"
                                                            Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtCompositePartSpecHotMeldAdhesiveValue"
                                                            ErrorMessage="Composite part hot meld adhesive must be a number." SetFocusOnError="True" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddCompositePartSpecHotMeldAdhesiveUnits" Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:View>
                                        <asp:View ID="vMoldedBarrier" runat="server">
                                            <asp:Label runat="server" ID="lblMessageMoldedBarrier" Text=""></asp:Label>
                                            <table>
                                                <tr>
                                                    <td class="p_text">
                                                        Formula:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddMoldedBarrierFormula" runat="server" Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Approximate Length:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtMoldedBarrierApproximateLengthValue" MaxLength="10"
                                                            Enabled="false"></asp:TextBox>
                                                        <asp:CompareValidator runat="server" ID="cvMoldedBarrierApproximateLengthValue" Operator="DataTypeCheck"
                                                            ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtMoldedBarrierApproximateLengthValue"
                                                            ErrorMessage="Barrier length must be a number." SetFocusOnError="True" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddMoldedBarrierApproximateLengthUnits" Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Approximate Width:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtMoldedBarrierApproximateWidthValue" MaxLength="10"
                                                            Enabled="false"></asp:TextBox>
                                                        <asp:CompareValidator runat="server" ID="cvMoldedBarrierApproximateWidthValue" Operator="DataTypeCheck"
                                                            ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtMoldedBarrierApproximateWidthValue"
                                                            ErrorMessage="Barrier width must be a number." SetFocusOnError="True" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddMoldedBarrierApproximateWidthUnits" Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Approximate Thickness:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtMoldedBarrierApproximateThicknessValue" MaxLength="10"
                                                            Enabled="false"></asp:TextBox>
                                                        <asp:CompareValidator runat="server" ID="cvMoldedBarrierApproximateThicknessValue"
                                                            Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtMoldedBarrierApproximateThicknessValue"
                                                            ErrorMessage="Barrier thickness must be a number." SetFocusOnError="True" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddMoldedBarrierApproximateThicknessUnits" Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Barrier Blank Area:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtMoldedBarrierBlankAreaValue" MaxLength="10" Enabled="false"></asp:TextBox>
                                                        <asp:CompareValidator runat="server" ID="cvMoldedBarrierBlankAreaValue" Operator="DataTypeCheck"
                                                            ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtMoldedBarrierBlankAreaValue"
                                                            ErrorMessage="Barrier blank area must be a number." SetFocusOnError="True" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddMoldedBarrierBlankAreaUnits" Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Specific Gravity:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtMoldedBarrierSpecificGravityValue" MaxLength="10"
                                                            Enabled="false"></asp:TextBox>
                                                        <asp:CompareValidator runat="server" ID="cvMoldedBarrierSpecificGravityValue" Operator="DataTypeCheck"
                                                            ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtMoldedBarrierSpecificGravityValue"
                                                            ErrorMessage="Barroer specific gravity must be a number." SetFocusOnError="True" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddMoldedBarrierSpecificGravityUnits" Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Weight Per Area:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtMoldedBarrierWeightPerAreaValue" MaxLength="10"
                                                            Enabled="false"></asp:TextBox>
                                                        <asp:CompareValidator runat="server" ID="cvMoldedBarrierWeightPerAreaValue" Operator="DataTypeCheck"
                                                            ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtMoldedBarrierWeightPerAreaValue"
                                                            ErrorMessage="Barrier weight per area must be a number." SetFocusOnError="True" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddMoldedBarrierWeightPerAreaUnits" Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Blank Weight:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtMoldedBarrierBlankWeightValue" MaxLength="10"
                                                            Enabled="false"></asp:TextBox>
                                                        <asp:CompareValidator runat="server" ID="cvMoldedBarrierBlankWeightValue" Operator="DataTypeCheck"
                                                            ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtMoldedBarrierBlankWeightValue"
                                                            ErrorMessage="Barrier blank weight must be a number." SetFocusOnError="True" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddMoldedBarrierBlankWeightUnits" Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Anti-Block Coating:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtMoldedBarrierAntiBlockCoatingValue" MaxLength="10"
                                                            Enabled="false"></asp:TextBox>
                                                        <asp:CompareValidator runat="server" ID="cvMoldedBarrierAntiBlockCoatingValue" Operator="DataTypeCheck"
                                                            ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtMoldedBarrierAntiBlockCoatingValue"
                                                            ErrorMessage="Barrier anti-block coating must be a number." SetFocusOnError="True" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddMoldedBarrierAntiBlockCoatingUnits" Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        *** TOTAL BARRIER WEIGHT:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtMoldedBarrierTotalWeightValue" MaxLength="10"
                                                            Enabled="false"></asp:TextBox>
                                                        <asp:CompareValidator runat="server" ID="cvMoldedBarrierTotalWeightValue" Operator="DataTypeCheck"
                                                            ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtMoldedBarrierTotalWeightValue"
                                                            ErrorMessage="Total barrier weight must be a number." SetFocusOnError="True" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddMoldedBarrierTotalWeightUnits" Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:View>
                                        <asp:View ID="vCapital" runat="server">
                                            <asp:Label runat="server" ID="lblMessageCapital"></asp:Label>
                                            <asp:ValidationSummary ID="vsFooterCapital" runat="server" ShowMessageBox="True"
                                                ShowSummary="true" ValidationGroup="vgFooterCapital" />
                                            <asp:ValidationSummary ID="vsEditCapital" runat="server" ShowMessageBox="True" ShowSummary="true"
                                                ValidationGroup="vgEditCapital" />
                                            <br />
                                            <asp:Button runat="server" ID="btnRemoveCapital" Text="Remove All Capitals" Visible="false" />
                                            <asp:GridView runat="server" ID="gvCapital" Width="100%" DataSourceID="odsCostSheetCapital"
                                                AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" PageSize="15"
                                                ShowFooter="True" DataKeyNames="RowID">
                                                <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                                <EditRowStyle BackColor="#CCCCCC" />
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
                                                <Columns>
                                                    <asp:BoundField DataField="CostSheetID" SortExpression="CostSheetID" />
                                                    <asp:TemplateField HeaderText="Description" SortExpression="CapitalID">
                                                        <EditItemTemplate>
                                                            <asp:DropDownList ID="ddEditCapital" runat="server" DataSource='<%# CostingModule.GetCapital(0,"") %>'
                                                                DataValueField="CapitalID" DataTextField="ddCapitalDesc" AppendDataBoundItems="True"
                                                                SelectedValue='<%# Bind("CapitalID") %>'>
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvEditCapital" runat="server" ControlToValidate="ddEditCapital"
                                                                ErrorMessage="The description is required." Font-Bold="True" ValidationGroup="vgEditCapital"
                                                                Text="<" SetFocusOnError="true" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewCapitalID" runat="server" CssClass="none" Text='<%# Bind("CapitalID") %>'></asp:Label>
                                                            <asp:Label ID="lblViewCapitalDesc" runat="server" Text='<%# Bind("ddCapitalDesc") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddFooterCapital" runat="server" DataSource='<%# CostingModule.GetCapital(0,"") %>'
                                                                DataValueField="CapitalID" DataTextField="ddCapitalDesc" AppendDataBoundItems="True"
                                                                SelectedValue='<%# Bind("CapitalID") %>'>
                                                                <asp:ListItem Text="" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvFooterCapital" runat="server" ControlToValidate="ddFooterCapital"
                                                                ErrorMessage="The description is required." Font-Bold="True" ValidationGroup="vgFooterCapital"
                                                                Text="<" SetFocusOnError="true" />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total $ Years Of Depr" SortExpression="TotalDollarAmount">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditCapitalTotalDollarAmount" runat="server" MaxLength="10" Width="75px"
                                                                Text='<%# Bind("TotalDollarAmount") %>'></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditCapitalTotalDollarAmount" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditCapital" Type="double" Text="<" ControlToValidate="txtEditCapitalTotalDollarAmount"
                                                                ErrorMessage="Total dollar amount must be a number." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewTotalDollarAmount" runat="server" Text='<%# Bind("TotalDollarAmount") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtFooterCapitalTotalDollarAmount" runat="server" MaxLength="10"
                                                                Width="75px" Text='<%# Bind("TotalDollarAmount") %>'></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvFooterCapitalTotalDollarAmount" Operator="DataTypeCheck"
                                                                ValidationGroup="vgFooterCapital" Type="double" Text="<" ControlToValidate="txtFooterCapitalTotalDollarAmount"
                                                                ErrorMessage="Total dollar amount must be a number." SetFocusOnError="True" />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Years Of Depr" SortExpression="YearsOfDepreciation">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditCapitalYearsOfDepreciation" runat="server" MaxLength="10"
                                                                Width="50px" Text='<%# Bind("YearsOfDepreciation") %>'></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditCapitalYearsOfDepreciation" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditCapital" Type="integer" Text="<" ControlToValidate="txtEditCapitalYearsOfDepreciation"
                                                                ErrorMessage="Years of depreciation must be an integer." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewYearsOfDepreciation" runat="server" Text='<%# Bind("YearsOfDepreciation") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtFooterCapitalYearsOfDepreciation" runat="server" MaxLength="10"
                                                                Width="50px" Text='<%# Bind("YearsOfDepreciation") %>'></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvFooterCapitalYearsOfDepreciation" Operator="DataTypeCheck"
                                                                ValidationGroup="vgFooterCapital" Type="integer" Text="<" ControlToValidate="txtFooterCapitalYearsOfDepreciation"
                                                                ErrorMessage="Years of depreciation must be an integer." SetFocusOnError="True" />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Annual Amort. Vol" SortExpression="CapitalAnnualVolume">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditCapitalAnnualVolume" runat="server" MaxLength="10" Width="50px"
                                                                Text='<%# Bind("CapitalAnnualVolume") %>'></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditCapitalAnnualVolume" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditCapital" Type="integer" Text="<" ControlToValidate="txtEditCapitalAnnualVolume"
                                                                ErrorMessage="Capital annual volume must be an integer." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewCapitalAnnualVolume" runat="server" Text='<%# Bind("CapitalAnnualVolume") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtFooterCapitalAnnualVolume" runat="server" MaxLength="10" Width="50px"
                                                                Text='<%# Bind("CapitalAnnualVolume") %>'></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvFooterCapitalAnnualVolume" Operator="DataTypeCheck"
                                                                ValidationGroup="vgFooterCapital" Type="integer" Text="<" ControlToValidate="txtFooterCapitalAnnualVolume"
                                                                ErrorMessage="Capital annual volume must be an integer." SetFocusOnError="True" />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Rate" SortExpression="OverheadAmount">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditCapitalOverheadAmount" runat="server" MaxLength="10" Width="50px"
                                                                Text='<%# Bind("OverheadAmount") %>'></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditCapitalOverheadAmount" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditCapital" Type="double" Text="<" ControlToValidate="txtEditCapitalOverheadAmount"
                                                                ErrorMessage="Overhead amount must be a number." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewCapitalOverheadAmount" runat="server" Text='<%# Bind("OverheadAmount") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtFooterCapitalOverheadAmount" runat="server" MaxLength="10" Width="50px"
                                                                Text='<%# Bind("OverheadAmount") %>'></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvFooterCapitalOverheadAmount" Operator="DataTypeCheck"
                                                                ValidationGroup="vgFooterCapital" Type="double" Text="<" ControlToValidate="txtFooterCapitalOverheadAmount"
                                                                ErrorMessage="Overhead amount must be a number." SetFocusOnError="True" />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Standard Cost Per Unit" SortExpression="StandardCostPerUnit">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditCapitalStandardCostPerUnit" runat="server" Text='<%# Bind("StandardCostPerUnit") %>'
                                                                MaxLength="10" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditCapitalStandardCostPerUnit" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditCapital" Type="double" Text="<" ControlToValidate="txtEditCapitalStandardCostPerUnit"
                                                                ErrorMessage="Standard cost per unit must be a number." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewCapitalStandardCostPerUnit" runat="server" Text='<%# Bind("StandardCostPerUnit") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Offline" SortExpression="isOffline">
                                                        <EditItemTemplate>
                                                            <asp:CheckBox ID="cbEditCapitalIsOffline" runat="server" Checked='<%# Bind("isOffline") %>' />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cbViewCapitalIsOffline" runat="server" Checked='<%# Bind("isOffline") %>'
                                                                Enabled="false" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:CheckBox ID="cbFooterCapitalIsOffline" runat="server" Checked='<%# Bind("isOffline") %>' />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Inline" SortExpression="isInline">
                                                        <EditItemTemplate>
                                                            <asp:CheckBox ID="cbEditCapitalisInline" runat="server" Checked='<%# Bind("isInline") %>' />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cbViewCapitalisInline" runat="server" Checked='<%# Bind("isInline") %>'
                                                                Enabled="false" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:CheckBox ID="cbFooterCapitalisInline" runat="server" Checked='<%# Bind("isInline") %>' />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Ordinal" SortExpression="Ordinal">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEditCapitalOrdinal" runat="server" Text='<%# Bind("Ordinal") %>'
                                                                MaxLength="2" Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvEditCapitalOrdinal" Operator="DataTypeCheck"
                                                                ValidationGroup="vgEditCapital" Type="integer" Text="<" ControlToValidate="txtEditCapitalOrdinal"
                                                                ErrorMessage="Ordinal be an integer." SetFocusOnError="True" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblViewCapitalOrdinal" runat="server" Text='<%# Bind("Ordinal") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtFooterCapitalOrdinal" runat="server" Text="99" MaxLength="2"
                                                                Width="50px"></asp:TextBox>
                                                            <asp:CompareValidator runat="server" ID="cvFooterCapitalOrdinal" Operator="DataTypeCheck"
                                                                ValidationGroup="vgFooterCapital" Type="integer" Text="<" ControlToValidate="txtFooterCapitalOrdinal"
                                                                ErrorMessage="Ordinal be an integer." SetFocusOnError="True" />
                                                        </FooterTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <EditItemTemplate>
                                                            <asp:ImageButton ID="iBtnCapitalUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                                                ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditCapital" />
                                                            <asp:ImageButton ID="iBtnCapitalCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                                                ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="iBtnCapitalEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                                                ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                                                            <asp:ImageButton ID="ibtnCapitalDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                                ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgFooterCapital"
                                                                runat="server" ID="iBtnFooterCapital" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                                                            <asp:ImageButton ID="iBtnCapitalUndo" runat="server" CommandName="Undo" CausesValidation="false"
                                                                ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <asp:ObjectDataSource ID="odsCostSheetCapital" runat="server" OldValuesParameterFormatString="original_{0}"
                                                SelectMethod="GetCostSheetCapital" TypeName="CostSheetCapitalBLL" DeleteMethod="DeleteCostSheetCapital"
                                                UpdateMethod="UpdateCostSheetCapital" InsertMethod="InsertCostSheetCapital">
                                                <SelectParameters>
                                                    <asp:QueryStringParameter Name="CostSheetID" QueryStringField="CostSheetID" Type="Int32" />
                                                    <asp:Parameter DefaultValue="0" Name="CapitalID" Type="Int32" />
                                                </SelectParameters>
                                                <DeleteParameters>
                                                    <asp:Parameter Name="RowID" Type="Int32" />
                                                    <asp:Parameter Name="original_RowID" Type="Int32" />
                                                </DeleteParameters>
                                                <UpdateParameters>
                                                    <asp:Parameter Name="CapitalID" Type="Int32" />
                                                    <asp:Parameter Name="TotalDollarAmount" Type="Double" />
                                                    <asp:Parameter Name="YearsOfDepreciation" Type="Int32" />
                                                    <asp:Parameter Name="CapitalAnnualVolume" Type="Int32" />
                                                    <asp:Parameter Name="OverheadAmount" Type="Double" />
                                                    <asp:Parameter Name="StandardCostPerUnit" Type="Double" />
                                                    <asp:Parameter Name="isOffline" Type="Boolean" />
                                                    <asp:Parameter Name="isInline" Type="Boolean" />
                                                    <asp:Parameter Name="Ordinal" Type="Int32" />
                                                    <asp:Parameter Name="original_RowID" Type="Int32" />
                                                    <asp:Parameter Name="CostSheetID" Type="Int32" />
                                                </UpdateParameters>
                                                <InsertParameters>
                                                    <asp:Parameter Name="CostSheetID" Type="Int32" />
                                                    <asp:Parameter Name="CapitalID" Type="Int32" />
                                                    <asp:Parameter Name="TotalDollarAmount" Type="Double" />
                                                    <asp:Parameter Name="YearsOfDepreciation" Type="Int32" />
                                                    <asp:Parameter Name="CapitalAnnualVolume" Type="Int32" />
                                                    <asp:Parameter Name="OverheadAmount" Type="Double" />
                                                    <asp:Parameter Name="isOffline" Type="Boolean" />
                                                    <asp:Parameter Name="isInline" Type="Boolean" />
                                                    <asp:Parameter Name="Ordinal" Type="Int32" />
                                                </InsertParameters>
                                            </asp:ObjectDataSource>
                                        </asp:View>
                                        <asp:View ID="vAssumptions" runat="server">
                                            <asp:GridView ID="gvAssumptions" runat="server" AutoGenerateColumns="False" DataKeyNames="CostSheetID,AID"
                                                DataSourceID="odsAssumptions" AllowPaging="True" AllowSorting="True" PageSize="10"
                                                Width="850px" SkinID="StandardGrid" ShowFooter="true" OnRowCommand="gvAssumptions_RowCommand"
                                                OnRowDataBound="gvAssumptions_RowDataBound">
                                                <Columns>
                                                    <asp:BoundField DataField="CostSheetID" HeaderText="CostSheetID" ReadOnly="True"
                                                        SortExpression="CostSheetID" Visible="false" />
                                                    <asp:BoundField DataField="AID" HeaderText="AID" ReadOnly="True" SortExpression="AID"
                                                        Visible="false" />
                                                    <asp:TemplateField HeaderText="Category" SortExpression="Category" HeaderStyle-Width="100px"
                                                        HeaderStyle-HorizontalAlign="Left">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtCategory1" runat="server" MaxLength="50" Text='<%# Bind("Category") %>'
                                                                Width="150px" />
                                                            <asp:RequiredFieldValidator ID="rfvCategory1" runat="server" ControlToValidate="txtCategory1"
                                                                ErrorMessage="Category is a required field." Font-Bold="True" ValidationGroup="EditAssumptionsInfo"><</asp:RequiredFieldValidator>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCategory" runat="server" Text='<%# Bind("Category") %>' />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtCategory" runat="server" MaxLength="50" Width="150px" />
                                                            <asp:RequiredFieldValidator ID="rfvCategory" runat="server" ControlToValidate="txtCategory"
                                                                ErrorMessage="Category is a required field." Font-Bold="True" ValidationGroup="InsertAssumptionsInfo"><</asp:RequiredFieldValidator>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Notes" SortExpression="Notes">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtNotes1" runat="server" MaxLength="2000" Rows="3" TextMode="MultiLine"
                                                                Text='<%# Bind("Notes") %>' Width="600px" />
                                                            <asp:RequiredFieldValidator ID="rfvNotes1" runat="server" ControlToValidate="txtNotes1"
                                                                ErrorMessage="Notes is a required field." Font-Bold="True" ValidationGroup="EditAssumptionsInfo"><</asp:RequiredFieldValidator>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblitNotes" runat="server" Text='<%# Bind("Notes") %>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="True" />
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtNotes" runat="server" MaxLength="2000" Rows="3" TextMode="MultiLine"
                                                                Width="600px" />
                                                            <asp:RequiredFieldValidator ID="rfvNotes" runat="server" ControlToValidate="txtNotes"
                                                                ErrorMessage="Notes is a required field." Font-Bold="True" ValidationGroup="InsertAssumptionsInfo"><</asp:RequiredFieldValidator>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ShowHeader="False" HeaderStyle-Width="50px">
                                                        <EditItemTemplate>
                                                            <asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                                                ImageUrl="~/images/save.jpg" Text="Update" AlternateText="Update" ValidationGroup="EditAssumptionsInfo" />
                                                            &nbsp;
                                                            <asp:ImageButton ID="ibtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                                                ImageUrl="~/images/cancel.jpg" Text="Cancel" AlternateText="Cancel" ValidationGroup="EditAssumptionsInfo" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                                                ImageUrl="~/images/edit.jpg" Text="Edit" AlternateText="Edit" />&nbsp;
                                                            <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                                ToolTip="Delete" ImageUrl="~/images/delete.jpg" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:ImageButton Text="Insert" CommandName="Insert" CausesValidation="true" ValidationGroup="InsertAssumptionsInfo"
                                                                runat="server" ID="ibtnInsert" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                                                            &nbsp;<asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo" ImageUrl="~/images/undo-gray.jpg"
                                                                Text="Undo" AlternateText="Undo" />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <asp:ObjectDataSource ID="odsAssumptions" runat="server" SelectMethod="GetCostSheetAssumptions"
                                                TypeName="CostSheetAssumptionsBLL" UpdateMethod="UpdateCostSheetAssumptions"
                                                InsertMethod="InsertCostSheetAssumptions" DeleteMethod="DeleteCostSheetAssumptions"
                                                OldValuesParameterFormatString="original_{0}">
                                                <SelectParameters>
                                                    <asp:QueryStringParameter Name="CostSheetID" QueryStringField="CostSheetID" Type="String" />
                                                </SelectParameters>
                                                <UpdateParameters>
                                                    <asp:QueryStringParameter Name="CostSheetID" QueryStringField="CostSheetID" Type="String" />
                                                    <asp:Parameter Name="AID" Type="Int32" />
                                                    <asp:Parameter Name="Category" Type="String" />
                                                    <asp:Parameter Name="Notes" Type="String" />
                                                    <asp:Parameter Name="original_CostSheetID" Type="Int32" />
                                                    <asp:Parameter Name="original_AID" Type="Int32" />
                                                </UpdateParameters>
                                                <InsertParameters>
                                                    <asp:QueryStringParameter Name="CostSheetID" QueryStringField="CostSheetID" Type="String" />
                                                    <asp:Parameter Name="Category" Type="String" />
                                                    <asp:Parameter Name="Notes" Type="String" />
                                                </InsertParameters>
                                                <DeleteParameters>
                                                    <asp:QueryStringParameter Name="CostSheetID" QueryStringField="CostSheetID" Type="String" />
                                                    <asp:Parameter Name="AID" Type="Int32" />
                                                </DeleteParameters>
                                            </asp:ObjectDataSource>
                                            &nbsp;&nbsp;
                                            <asp:ValidationSummary ID="vsEditAssumptionsInfo" runat="server" ShowMessageBox="True"
                                                Width="599px" ValidationGroup="EditAssumptionsInfo" Height="35px" />
                                            <asp:ValidationSummary ID="vsInsertAssumptionsInfo" runat="server" ShowMessageBox="True"
                                                Width="599px" ValidationGroup="InsertAssumptionsInfo" />
                                            <br />
                                            <asp:ValidationSummary ID="vsEmptyAssumptionsInfo" runat="server" ShowMessageBox="True"
                                                Width="599px" ValidationGroup="EmptyAssumptionsInfo" />
                                            <br />
                                            <br />
                                            <br />
                                            <asp:GridView ID="gvAssumptionsApproval" runat="server" AutoGenerateColumns="False"
                                                DataKeyNames="CostSheetID,Department" DataSourceID="odsAssumptionsApproval" AllowPaging="True"
                                                AllowSorting="True" PageSize="10" Width="600px" SkinID="StandardGrid" ShowFooter="false"
                                                OnRowDataBound="gvAssumptionsApproval_RowDataBound">
                                                <Columns>
                                                    <asp:BoundField DataField="CostSheetID" HeaderText="CostSheetID" ReadOnly="True"
                                                        SortExpression="CostSheetID" Visible="false" />
                                                    <asp:BoundField DataField="Department" HeaderText="Department" ReadOnly="True" SortExpression=" Department "
                                                        Visible="true" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                                                    <asp:TemplateField HeaderText="Team Member" SortExpression="TeamMemberName">
                                                        <EditItemTemplate>
                                                            <asp:DropDownList ID="ddTeamMember1" runat="server" DataSource='<%# commonFunctions.GetTeamMember("") %>'
                                                                DataValueField="TeamMemberID" DataTextField="TeamMemberName" AppendDataBoundItems="True"
                                                                SelectedValue='<%# Bind("TeamMemberID") %>'>
                                                                <asp:ListItem Selected="True" Value="0" Text="Select a Team Member">
                                                                </asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvTeamMember1" runat="server" ControlToValidate="ddTeamMember1"
                                                                Display="Dynamic" ErrorMessage="Team Member is a required field." ValidationGroup="EditAAInfo"><</asp:RequiredFieldValidator>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTeamMember" runat="server" Text='<%# Bind("TeamMemberName") %>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="True" />
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddTeamMember" runat="server" DataSource='<%# commonFunctions.GetTeamMember("") %>'
                                                                DataValueField="TeamMemberID" DataTextField="TeamMemberName" AppendDataBoundItems="True">
                                                                <asp:ListItem Selected="True" Value="null" Text="Select a Team Member">
                                                                </asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvTeamMember" runat="server" ControlToValidate="ddTeamMember"
                                                                Display="Dynamic" ErrorMessage="Team Member is a required field." ValidationGroup="InsertAAInfo"><</asp:RequiredFieldValidator>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Approval Date" SortExpression="ApprovalDate">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <EditItemTemplate>
                                                            <asp:TextBox runat="server" ID="txtApprovalDate1" Text='<%# Bind("ApprovalDate") %>'
                                                                Width="85px" />&nbsp;
                                                            <asp:ImageButton runat="server" ID="imgApprovalDate1" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                                                AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                                                            <ajax:CalendarExtender ID="calendarButtonExtender" runat="server" TargetControlID="txtApprovalDate1"
                                                                PopupButtonID="imgApprovalDate1" Format="MM/dd/yyyy" />
                                                            <asp:RegularExpressionValidator ID="revApprovalDate1" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                                                ControlToValidate="txtApprovalDate1" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                                                Width="8px" ValidationGroup="EditAAInfo"><</asp:RegularExpressionValidator>
                                                            <asp:RequiredFieldValidator ID="rfvApprovalDate1" runat="server" ErrorMessage="Approval Date is a required field."
                                                                ControlToValidate="txtApprovalDate1" Font-Bold="True" ValidationGroup="EditAAInfo"><</asp:RequiredFieldValidator>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            &nbsp<asp:Label ID="lblApprovalDate" runat="server" Text='<%# Bind("ApprovalDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox runat="server" ID="txtApprovalDate" Width="85px" />&nbsp;
                                                            <asp:ImageButton runat="server" ID="imgApprovalDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                                                AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                                                            <ajax:CalendarExtender ID="calendarButtonExtender" runat="server" TargetControlID="txtApprovalDate"
                                                                PopupButtonID="imgApprovalDate" Format="MM/dd/yyyy" />
                                                            <asp:RegularExpressionValidator ID="revApprovalDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                                                ControlToValidate="txtApprovalDate" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                                                Width="8px" ValidationGroup="InsertAAInfo"><</asp:RegularExpressionValidator>
                                                            <asp:RequiredFieldValidator ID="rfvApprovalDate" runat="server" ControlToValidate="txtApprovalDate"
                                                                ErrorMessage="Approval Date is a required field." Font-Bold="True" ValidationGroup="InsertAAInfo"><</asp:RequiredFieldValidator>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ShowHeader="False" HeaderStyle-Width="50px">
                                                        <EditItemTemplate>
                                                            <asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                                                ImageUrl="~/images/save.jpg" Text="Update" AlternateText="Update" ValidationGroup="EditAAInfo" />
                                                            &nbsp;
                                                            <asp:ImageButton ID="ibtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                                                ImageUrl="~/images/cancel.jpg" Text="Cancel" AlternateText="Cancel" ValidationGroup="EditAAInfo" />
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                                                ImageUrl="~/images/edit.jpg" Text="Edit" AlternateText="Edit" />&nbsp;
                                                            <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                                ToolTip="Delete" ImageUrl="~/images/delete.jpg" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:ImageButton Text="Insert" CommandName="Insert" CausesValidation="true" ValidationGroup="InsertAAInfo"
                                                                runat="server" ID="ibtnInsert" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                                                            &nbsp;<asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo" ImageUrl="~/images/undo-gray.jpg"
                                                                Text="Undo" AlternateText="Undo" />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <asp:ObjectDataSource ID="odsAssumptionsApproval" runat="server" SelectMethod="GetCostSheetAssumptionsApproval"
                                                TypeName="CostSheetAssumptionsBLL" UpdateMethod="UpdateCostSheetAssumptionsApproval"
                                                DeleteMethod="DeleteCostSheetAssumptionsApproval" OldValuesParameterFormatString="original_{0}">
                                                <SelectParameters>
                                                    <asp:QueryStringParameter Name="CostSheetID" QueryStringField="CostSheetID" Type="String" />
                                                </SelectParameters>
                                                <UpdateParameters>
                                                    <asp:QueryStringParameter Name="CostSheetID" QueryStringField="CostSheetID" Type="String" />
                                                    <asp:Parameter Name="Department" Type="String" />
                                                    <asp:Parameter Name="TeamMemberID" Type="Int32" />
                                                    <asp:Parameter Name="ApprovalDate" Type="String" />
                                                    <asp:Parameter Name="original_CostSheetID" Type="Int32" />
                                                    <asp:Parameter Name="original_Department" Type="String" />
                                                </UpdateParameters>
                                                <DeleteParameters>
                                                    <asp:QueryStringParameter Name="CostSheetID" QueryStringField="CostSheetID" Type="String" />
                                                    <asp:Parameter Name="Department" Type="String" />
                                                </DeleteParameters>
                                            </asp:ObjectDataSource>
                                            &nbsp;&nbsp;
                                            <asp:ValidationSummary ID="vsEditAAInfo" runat="server" ShowMessageBox="True" Width="599px"
                                                ValidationGroup="EditAAInfo" Height="35px" />
                                            <asp:ValidationSummary ID="vsInsertAAInfo" runat="server" ShowMessageBox="True" Width="599px"
                                                ValidationGroup="InsertAAInfo" />
                                            <br />
                                            <asp:ValidationSummary ID="vsEmptyAAInfo" runat="server" ShowMessageBox="True" Width="599px"
                                                ValidationGroup="EmptyAAInfo" />
                                        </asp:View>
                                    </asp:MultiView>
                                </td>
                            </tr>
                        </table>
                        <asp:Label ID="lblMessageLowerPage" SkinID="MessageLabelSkin" runat="server"></asp:Label>
                        <table width="98%">
                            <tr>
                                <td>
                                    <asp:Button ID="btnSaveLowerPage" runat="server" Width="90" Text="Save" Visible="false"
                                        CausesValidation="true" ValidationGroup="vgSave"></asp:Button>
                                    <asp:Button ID="btnCalculate" runat="server" Width="90" Text="Calculate" Visible="false"
                                        ValidationGroup="vgSave"></asp:Button>
                                    <asp:Button ID="btnUpdateTotals" runat="server" Width="90" Text="Update Totals" Visible="false"
                                        ValidationGroup="vgSave"></asp:Button>
                                    <asp:Button ID="btnPreviewCostSheet" runat="server" Width="150" Text="Preview Cost Sheet"
                                        Visible="false" />
                                    <asp:Button ID="btnPreviewDieLayout" runat="server" Width="150" Text="Preview Die Layout"
                                        Visible="false" />
                                </td>
                            </tr>
                        </table>
                    </Content>
                </ajax:AccordionPane>
            </Panes>
        </ajax:Accordion>
        <ajax:Accordion ID="accCostTotals" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
            FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
            RequireOpenedPane="false" SuppressHeaderPostbacks="true" Visible="false">
            <Panes>
                <ajax:AccordionPane ID="apCostTotals" runat="server">
                    <Header>
                        <a href="" class="accordionLink">Cost Sheet Totals</a></Header>
                    <Content>
                        <table border="1" runat="server" id="tblCostSheetTotals" visible="false">
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td class="c_text">
                                                Without Scrap<br />
                                                (for Material, Packaging, Labor, and Overhead)
                                            </td>
                                            <td class="c_text">
                                                With Scrap
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblMaterialCostTotalLabel" Text="Total Material:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtMaterialCostTotalWOScrapValue" Enabled="false"
                                                    MaxLength="10"></asp:TextBox>
                                                <asp:CompareValidator runat="server" ID="cvMaterialCostTotalWOScrapValue" Operator="DataTypeCheck"
                                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtMaterialCostTotalWOScrapValue"
                                                    ErrorMessage="Material Cost Total W/O Scrap  must be a number." SetFocusOnError="True" />
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtMaterialCostTotalValue" Enabled="false" MaxLength="10"></asp:TextBox>
                                                <asp:CompareValidator runat="server" ID="cvMaterialCostTotalValue" Operator="DataTypeCheck"
                                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtMaterialCostTotalValue"
                                                    ErrorMessage="Material cost total must be a number." SetFocusOnError="True" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblPackagingCostTotalLabel" Text="Total Packaging:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtPackagingCostTotalWOScrapValue" Enabled="false"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtPackagingCostTotalValue" Enabled="false" MaxLength="10"></asp:TextBox>
                                                <asp:CompareValidator runat="server" ID="cvPackagingCostTotalValue" Operator="DataTypeCheck"
                                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtPackagingCostTotalValue"
                                                    ErrorMessage="Packaging cost total must be a number." SetFocusOnError="True" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblMaterialAndPackagingCostTotalLabel" Text="Total Material + Packaging:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblMaterialAndPackagingCostTotalWOScrapValue"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblMaterialAndPackagingCostTotalValue"></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblLaborCostTotalLabel" Text="Total Labor:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtLaborCostTotalWOScrapValue" Enabled="false" MaxLength="10"></asp:TextBox>
                                                <asp:CompareValidator runat="server" ID="cvLaborCostTotalWOScrapValue" Operator="DataTypeCheck"
                                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtLaborCostTotalWOScrapValue"
                                                    ErrorMessage="Labor Cost Total W/O Scrap  must be a number." SetFocusOnError="True" />
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtLaborCostTotalValue" Enabled="false" MaxLength="10"></asp:TextBox>
                                                <asp:CompareValidator runat="server" ID="cvLaborCostTotalValue" Operator="DataTypeCheck"
                                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtLaborCostTotalValue"
                                                    ErrorMessage="Labor cost total must be a number." SetFocusOnError="True" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblOverheadCostTotalLabel" Text="Total Overhead:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtOverheadCostTotalWOScrapValue" Enabled="false"
                                                    MaxLength="10"></asp:TextBox>
                                                <asp:CompareValidator runat="server" ID="cvOverheadCostTotalWOScrapValue" Operator="DataTypeCheck"
                                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtOverheadCostTotalWOScrapValue"
                                                    ErrorMessage="Overhead Cost Total W/O Scrap  must be a number." SetFocusOnError="True" />
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtOverheadCostTotalValue" Enabled="false" MaxLength="10"></asp:TextBox>
                                                <asp:CompareValidator runat="server" ID="cvOverheadCostTotalValue" Operator="DataTypeCheck"
                                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtOverheadCostTotalValue"
                                                    ErrorMessage="Overhead cost total must be a number." SetFocusOnError="True" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblScrapCostTotalLabel" Text="Total Scrap:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblScrapCostTotalValue"></asp:Label>
                                            </td>
                                            <td>
                                                N/A
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblCapitalCostTotalLabel" Text="Total Capital:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblCapitalCostTotalWOScrapValue"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtCapitalCostTotalValue" Enabled="false" MaxLength="10"></asp:TextBox>
                                                <asp:CompareValidator runat="server" ID="cvCapitalCostTotalValue" Operator="DataTypeCheck"
                                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtCapitalCostTotalValue"
                                                    ErrorMessage="Capital cost total must be a number." SetFocusOnError="True" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblManufacturingCostTotalLabel" Text="Total Manufacturing:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblManufacturingCostTotalWOScrapValue"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblManufacturingCostTotalValue"></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <hr />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblVariableCostTotalLabel" Text="Total Variable Cost:<br>(Material + Packaging + Labor + Variable Overhead + Scrap)"></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblVariableCostTotalValue"></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblFixedCostTotalLabel" Text="Total Fixed Cost:<br>(Fixed Overhead)"></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblFixedCostTotalValue"></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblSGACostTotalLabel" Text="SGA:"></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblSGACostTotalValue"></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblMiscCostTotalLabel" Text="Total Misc:"></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtMiscCostTotalValue" Enabled="false" MaxLength="10"></asp:TextBox>
                                                <asp:CompareValidator runat="server" ID="cvMiscCostTotalValue" Operator="DataTypeCheck"
                                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtMiscCostTotalValue"
                                                    ErrorMessage="Miscellaneous cost total must be a number." SetFocusOnError="True" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblCapitalCostTotalLabel2" Text="Total Capital:"></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblCapitalCostTotalValue2"></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblOverallCostTotalLabel" BackColor="Yellow" Text="Total Standard Cost:"></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtOverallCostTotalValue" BackColor="Yellow" Enabled="false"
                                                    MaxLength="10"></asp:TextBox>
                                                <asp:CompareValidator runat="server" ID="cvOverallCostTotalValue" Operator="DataTypeCheck"
                                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtOverallCostTotalValue"
                                                    ErrorMessage="Overall total cost must be a number." SetFocusOnError="True" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <hr />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_textbold">
                                                <asp:Label runat="server" ID="lblMinimumSellingPriceLabel" Text="Minimum Selling Price:<br>(Variable Cost + Misc Cost (W/O SGA) + Capital) / (1 - Facility Target Margin)"></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td class="c_textbold">
                                                <asp:Label runat="server" ID="lblMinimumSellingPriceValue"></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblPriceVariableMarginDollarLabel" Text="Variable Margin Dollar:<br>(Min Selling Price - Total Variable Cost) "></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                $<asp:Label runat="server" ID="lblPriceVariableMarginDollarValue"></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblPriceVariableMarginPercentLabel" Text="Variable Margin Percent:<br>(Variable Margin Dollar / Min Selling Price)"></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblPriceVariableMarginPercentValue"></asp:Label>%
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblPriceGrossMarginDollarLabel" Text="C2 Dollar:<br>(Min Selling Price - Total Standard Cost)"></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                $<asp:Label runat="server" ID="lblPriceGrossMarginDollarValue"></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblPriceGrossMarginPercentLabel" Text="C2 Percent:<br>(C2 / Min Selling Price)"></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblPriceGrossMarginPercentValue"></asp:Label>%
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblPriceVariableMarginInclDeprDollarLabel" Text="Variable Margin Dollar (Incl. Depr.):<br>(Min Selling Price - Total Variable Cost) - (Capital + Misc Cost)"></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                $<asp:Label runat="server" ID="lblPriceVariableMarginInclDeprDollarValue"></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblPriceVariableMarginInclDeprPercentLabel" Text="Variable Margin Percent (Incl. Depr.):<br>(Variable Margin Incl Depr. Dollar / Min Selling Price )"></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblPriceVariableMarginInclDeprPercentValue"></asp:Label>%
                                            </td>
                                            <td>
                                                (Variable Margin Target:&nbsp;<asp:Label runat="server" ID="lblPriceVariableMarginPercentTargetValue"></asp:Label>%
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </Content>
                </ajax:AccordionPane>
            </Panes>
        </ajax:Accordion>
    </asp:Panel>
</asp:Content>
