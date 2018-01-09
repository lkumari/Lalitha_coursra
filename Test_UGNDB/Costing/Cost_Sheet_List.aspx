<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    MaintainScrollPositionOnPostback="true" CodeFile="Cost_Sheet_List.aspx.vb" Inherits="Cost_Sheet_List"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <table style="width: 344px">
            <tr>
                <td class="p_smalltextbold" style="color: #990000" colspan="3" align="left">
                    <asp:Label runat="server" ID="lblReview1" Text="Review existing data or press"></asp:Label>
                    <asp:Button ID="btnAdd" runat="server" Text="Add" Enabled="false" />
                    <asp:Label runat="server" ID="lblReview2" Text="to enter new data."></asp:Label>
                </td>
            </tr>
        </table>
        <asp:Label ID="lblMessage" SkinID="MessageLabelSkin" runat="server"></asp:Label>
        <asp:ValidationSummary ID="vsCostSheetList" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgCosting" />
        <br />
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <table style="width: 98%">
            <tr>
                <td class="p_text">
                    <asp:Label runat="server" ID="lblSearchCostSheetIDLabel" Text="Cost Sheet ID:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchCostSheetIDValue" MaxLength="10" Visible="false"></asp:TextBox>
                    &nbsp;<asp:CheckBox ID="cbQuickQuote" runat="server" Font-Italic="True" 
                        Text="Quick Quote Only" />
                </td>
                <td class="p_text">
                    <asp:Label runat="server" ID="lblSearchCostSheetStatusLabel" Text="Cost Sheet Status:"
                        Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddSearchCostSheetStatusValue" runat="server" Visible="false">
                        <asp:ListItem Text="All" Value="All"></asp:ListItem>
                        <asp:ListItem Text="Current" Value="Current"></asp:ListItem>
                        <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                        <asp:ListItem Text="Previous" Value="Previous"></asp:ListItem>
                        <asp:ListItem Text="Proposal" Value="Proposal"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label runat="server" ID="lblSearchPartNoLabel" Text="Internal Part No:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchPartNoValue" MaxLength="40" Visible="false"></asp:TextBox>
                    &nbsp;<asp:CheckBox runat="server" ID="cbBOM" Visible="false" Text="Check BOM" 
                        Font-Italic="True" />
                </td>
                <td class="p_text">
                    <asp:Label runat="server" ID="lblSearchPartNameLabel" Text="Part Name:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchPartNameValue" MaxLength="240" Visible="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label runat="server" ID="lblSearchCustomerPartNoLabel" Text="Customer Part No:"
                        Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchCustomerPartNoValue" MaxLength="40" Visible="false"></asp:TextBox>
                </td>
                <td class="p_text">
                    <asp:Label runat="server" ID="lblSearchDesignLevelLabel" Text="Design Level:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchDesignLevelValue" MaxLength="50" Visible="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td class="p_text">
                    <asp:Label runat="server" ID="lblSearchMaterialIDLabel" Text="Material ID:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchMaterialIDValue" MaxLength="10" Visible="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label runat="server" ID="lblSearchDrawingNoLabel" Text="Drawing No:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchDrawingNoValue" MaxLength="17" Visible="false"></asp:TextBox>
                </td>
                <td class="p_text">
                    <asp:Label runat="server" ID="lblSearchRFDNoLabel" Text="RFD No:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchRFDNoValue" MaxLength="10" Visible="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label runat="server" ID="lblSearchApprovedLabel" Text="Approval Status:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddSearchApprovedValue" Visible="false">
                        <asp:ListItem Selected="True" Text="" Value="All"></asp:ListItem>
                        <asp:ListItem Text="Approved" Value="Approved"></asp:ListItem>
                        <asp:ListItem Text="NOT Approved" Value="Pending"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="p_text">
                    <asp:Label runat="server" ID="lblSearchWaitingForTeamMemberApprovalLabel" Text="Waiting For Which<br> Team Member To Approve:"
                        Visible="false"> </asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddSearchWaitingForTeamMemberApprovalValue" runat="server" Visible="false">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label runat="server" ID="lblSearchApprovedByTeamMemberLabel" Text="Approved By<br> Team Member:"
                        Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddSearchApprovedByTeamMemberValue" runat="server" Visible="false">
                    </asp:DropDownList>
                </td>
                <td class="p_text">
                    <asp:Label runat="server" ID="lblSearchAccountManagerLabel" Text="Account Manager:"
                        Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddSearchAccountManagerValue" runat="server" Visible="false">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <ajax:Accordion ID="accAdvancedSearch" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
            FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
            RequireOpenedPane="false" SuppressHeaderPostbacks="true" Visible="false">
            <Panes>
                <ajax:AccordionPane ID="apAdvancedSearch" runat="server">
                    <Header>
                        <a href="" class="accordionLink">Advanced Search</a></Header>
                    <Content>
                        <asp:CheckBox runat="server" ID="cbShowAdvancedSearch" Text="Keep advanced search open"
                            AutoPostBack="true" /><br />
                        <table style="width: 98%">
                            <tr>
                                <td class="p_text">
                                    <asp:Label runat="server" ID="lblSearchCustomerLabel" Text="Customer:" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddSearchCustomerValue" runat="server" Visible="false">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    <asp:Label runat="server" ID="lblSearchProgramLabel" Text="Program:" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddSearchProgramValue" runat="server" Visible="false">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text" valign="top">
                                    <asp:Label runat="server" ID="lblSearchCommodityLabel" Text="Commodity:" Visible="false"></asp:Label>
                                </td>
                                <td valign="top">
                                    <asp:DropDownList ID="ddSearchCommodityValue" runat="server" Visible="false">
                                    </asp:DropDownList>
                                    <br />
                                    {Commodity / Classification}
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    <asp:Label runat="server" ID="lblSearchYearLabel" Text="Year:" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddSearchYearValue" runat="server" Visible="false">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    <asp:Label runat="server" ID="lblSearchFormulaLabel" Text="Formula:" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddSearchFormulaValue" runat="server" Visible="false">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    <asp:Label runat="server" ID="lblSearchUGNFacilityLabel" Text="UGN Facility:" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddSearchUGNFacilityValue" runat="server" Visible="false">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    <asp:Label runat="server" ID="lblSearchDepartmentLabel" Text="Department:" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddSearchDepartmentValue" runat="server" Visible="false">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </Content>
                </ajax:AccordionPane>
            </Panes>
        </ajax:Accordion>
        <table width="98%">
            <tr>
                <td colspan="4" align="center">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" ValidationGroup="vgCosting"
                        Visible="false" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" ValidationGroup="vgCosting"
                        Visible="false" />
                </td>
            </tr>
        </table>
        <br />
        <asp:Label ID="lblMessageBottom" SkinID="MessageLabelSkin" runat="server"></asp:Label>
        <hr />
        <em class="p_smalltextbold" runat="server" id="emTip">Use the parameters above to filter
            the list below. <u>A row with yellow background indicates approvals are pending by one
                or more team members. A row with red background indicates at leat one team member
                has rejected the cost form.</u></em>
        <table width="98%">
            <tbody>
                <tr>
                    <td colspan="7" align="right">
                        <asp:RegularExpressionValidator ID="revGoToPage" runat="server" ControlToValidate="txtGoToPage"
                            ValidationGroup="vgCosting" ErrorMessage="Only numbers can be used for the pages."
                            SetFocusOnError="True" ValidationExpression="\b\d+\b" Height="20px"></asp:RegularExpressionValidator>
                        <asp:Label ID="lblCurrentPage" runat="server" Visible="false"></asp:Label>
                        <asp:Button ID="cmdFirst" runat="server" Text="|<" CssClass="button-search" Visible="false" />
                        <asp:Button ID="cmdPrev" runat="server" Text="<<" CssClass="button-search" Visible="false" />
                        <asp:TextBox ID="txtGoToPage" runat="server" MaxLength="4" Width="25" Height="15px"
                            Font-Size="Small" Visible="false" />
                        <asp:Button ID="cmdGo" runat="server" Text="Go" CssClass="button-search" Visible="false"
                            ValidationGroup="vgCosting" />
                        <asp:Button ID="cmdNext" runat="server" Text=">>" CssClass="button-search" Visible="false" />
                        <asp:Button ID="cmdLast" runat="server" Text=">|" CssClass="button-search" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <table width="100%">
                            <asp:Repeater ID="rpCostSheetInfo" runat="server" Visible="false">
                                <HeaderTemplate>
                                    <tr>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkCostSheetID" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="CostSheetID">Cost Sheet</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkCostSheetStatus" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="CostSheetStatus">Status</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnlPreNotify" ForeColor="white" runat="server">Notifications</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkNewCustomerPartName" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="NewPartName">Part Name</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkPartNo" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="ShowPartNo">Part No</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkNewDesignLevel" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="NewDesignLevel">New Design Level</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkRFDNo" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="RFDNo">RFD No</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkPreviewCostForm" ForeColor="white" runat="server">Preview<br />Cost Form</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkPreviewDieLayout" ForeColor="white" runat="server">Preview<br />Die Layout</asp:LinkButton>
                                        </td>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr style="background-color: <%# SetBackGroundColor(Container.DataItem("ApprovedDate"),Container.DataItem("RejectedCount")).ToString  %>">
                                        <td align="center" style="white-space: nowrap;">
                                            <asp:HyperLink ID="selectCostSheetID" runat="server" Font-Underline="true" Visible='<%# ViewState("isAdmin") %>'
                                                NavigateUrl='<%# "Cost_Sheet_Detail.aspx?CostSheetID=" & DataBinder.Eval (Container.DataItem,"CostSheetID").tostring %>'><%#DataBinder.Eval(Container, "DataItem.CostSheetID")%></asp:HyperLink>
                                            <asp:Label ID="showCostSheetID" runat="server" Visible='<%# NOT ViewState("isAdmin") %>'
                                                Text='<%# DataBinder.Eval (Container.DataItem,"CostSheetID").tostring %>'></asp:Label>
                                        </td>
                                        <td align="left">
                                            <%#DataBinder.Eval(Container, "DataItem.CostSheetStatus")%>
                                        </td>
                                        <td align="center" style="white-space: nowrap;">
                                            <asp:HyperLink ID="selectPreApprovalList" runat="server" Font-Underline="true" NavigateUrl='<%# "Cost_Sheet_Pre_Approval_List.aspx?CostSheetID=" & DataBinder.Eval (Container.DataItem,"CostSheetID").tostring %>'>Approvers</asp:HyperLink>
                                        </td>
                                        <td align="left">
                                            <%#DataBinder.Eval(Container, "DataItem.NewPartName")%>
                                        </td>
                                        <td align="left" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.ShowPartNo")%>
                                        </td>
                                        <td align="left" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.NewDesignLevel")%>
                                        </td>
                                        <td align="center" style="white-space: nowrap;">
                                            <asp:HyperLink ID="selectRFDNo" runat="server" Font-Underline="true" Target="_blank"
                                                NavigateUrl='<%# "../RFD/RFD_Detail.aspx?RFDNo=" & DataBinder.Eval (Container.DataItem,"RFDNo").tostring %>'><%#DataBinder.Eval(Container, "DataItem.RFDNo")%></asp:HyperLink>
                                        </td>
                                        <td align="center" style="white-space: nowrap;">
                                            <a runat="server" id="aPreviewCostForm" href="#" visible='<%# NOT ViewState("isDieLayoutOnly") %>'
                                                onclick='<%# SetCostFormHyperLink(Container.DataItem("CostSheetID")).ToString %>'>
                                                <asp:Image runat="server" ID="imgPreviewCostForm" ImageUrl="~/images/PreviewUp.jpg" />
                                            </a>
                                        </td>
                                        <td align="center" style="white-space: nowrap;">
                                            <a runat="server" id="aPreviewDieLayout" href="#" onclick='<%# SetDieLayoutHyperLink(Container.DataItem("CostSheetID")).ToString %>'
                                                visible='<%# Eval("isDiecut") %>'>
                                                <asp:Image runat="server" ID="imgPreviewDieLayout" ImageUrl="~/images/PreviewUp.jpg" />
                                            </a>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                </FooterTemplate>
                            </asp:Repeater>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </asp:Panel>
</asp:Content>
