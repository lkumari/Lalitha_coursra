<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="ToolingAuthExpProjList.aspx.vb" Inherits="ToolingAuthExpProjList" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <table width="98%">
            <tr>
                <td class="p_smalltextbold" style="color: #990000" align="left">
                    <asp:Label runat="server" ID="lblReview1" Text="Review existing data or press"></asp:Label>
                    <asp:Button ID="btnAdd" runat="server" Text="Add" Enabled="false" />
                    <asp:Label runat="server" ID="lblReview2" Text="to enter new data."></asp:Label>
                </td>
            </tr>
        </table>
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
        <asp:ValidationSummary runat="server" ID="vsSearch" ValidationGroup="vgSearch" ShowMessageBox="true"
            DisplayMode="BulletList" ShowSummary="true" EnableClientScript="true" />
        <table width="90%">
            <tr>
                <td style="white-space: nowrap;" class="p_text">
                    Project No:
                </td>
                <td>
                    <asp:TextBox ID="txtSearchTAProjectNo" runat="server" MaxLength="15" />
                </td>
                <td style="white-space: nowrap;" class="p_text">
                    TA Status:
                </td>
                <td>
                    <asp:DropDownList ID="ddSearchStatus" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="white-space: nowrap;" class="p_text">
                    Description:
                </td>
                <td>
                    <asp:TextBox ID="txtSearchTADesc" runat="server" MaxLength="240" />
                </td>
                <td style="white-space: nowrap;" class="p_text">
                    Initiator:
                </td>
                <td>
                    <asp:DropDownList ID="ddSearchInitiatorTeamMember" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="white-space: nowrap;" class="p_text">
                    RFD No.:
                </td>
                <td>
                    <asp:TextBox ID="txtSearchRFDNo" runat="server" MaxLength="6"></asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftSearchRFDNo" runat="server" TargetControlID="txtSearchRFDNo"
                        FilterType="Custom, Numbers" />
                </td>
                <td style="white-space: nowrap;" class="p_text">
                    Cost Sheet:</td>
                <td>
                    <asp:TextBox ID="txtSearchCostSheetID" runat="server" MaxLength="6"></asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftSearchCostSheetID" runat="server" TargetControlID="txtSearchCostSheetID"
                        FilterType="Custom, Numbers" />
                </td>
            </tr>
            <tr>
                <td style="white-space: nowrap;" class="p_text">
                    Part No.:
                </td>
                <td>
                    <asp:TextBox ID="txtSearchPartNo" runat="server" MaxLength="40"/>
                </td>
                <td class="p_text">
                    Part Name:
                </td>
                <td>
                    <asp:TextBox ID="txtSearchPartName" runat="server" MaxLength="240" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Design Level:
                </td>
                <td>
                    <asp:TextBox ID="txtSearchDesignLevel" runat="server" MaxLength="30" />
                </td>
                <td style="white-space: nowrap;" class="p_text">
                    Quality Engineer:
                </td>
                <td>
                    <asp:DropDownList ID="ddSearchQualityEngineer" runat="server" />
                </td>
            </tr>
        </table>
        <ajax:Accordion ID="accAdvancedSearch" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
            FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
            RequireOpenedPane="false" SuppressHeaderPostbacks="true">
            <Panes>
                <ajax:AccordionPane ID="apAdvancedSearch" runat="server">
                    <Header>
                        <a href="" >Advanced Search</a></Header>
                    <Content>
                        <asp:CheckBox runat="server" ID="cbShowAdvancedSearch" Text="Keep advanced search open"
                            AutoPostBack="true" /><br />
                        <asp:CheckBox runat="server" ID="cbIncludeArchive" Text="Include Archive Data (WARNING: CHECKING THIS WILL ADD CONSIDERABLE TIME TO WAIT FOR RESULTS.)"
                            AutoPostBack="true" /><br />
                        <table width="90%">
                            <tr>
                                <td class="p_text">
                                    UGN Facility:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddSearchUGNFacility" runat="server"/>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    Customer:
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddSearchCustomer" runat="server"/>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    Program:
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddSearchProgram" runat="server"/>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    Account Manager:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddSearchAccountManager" runat="server"/>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    Program Manager:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddSearchProgramManager" runat="server"/>
                                </td>
                            </tr>
                        </table>
                    </Content>
                </ajax:AccordionPane>
            </Panes>
        </ajax:Accordion>
        <table width="98%">
            <tr>
                <td align="center">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CausesValidation="true" ValidationGroup="vgSearch" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="false" />
                </td>
            </tr>
        </table>
        <hr />
        <i>Use the parameters above to filter the list below.</i>
        <table width="480px" border="0">
            <tr>
                <td align="center" width="80px" style="white-space: nowrap;">
                    Completed
                </td>
                <td align="center" width="80px" style="background-color: Fuchsia; white-space: nowrap;">
                    Open
                </td>
                <td align="center" width="80px" style="background-color: yellow; white-space: nowrap;">
                    In-Process
                </td>
                <td align="center" width="80px" style="background-color: gray; color: white; white-space: nowrap;">
                    Void
                </td>
            </tr>
        </table>
        <table width="98%" runat="server" id="tblResult">
            <tbody>
                <tr>
                    <td class="c_text" style="font-style: italic">
                        <asp:Label ID="lblRecListed" runat="server" Text="Records Listed: " />
                        <asp:Label ID="lblFromRec" runat="server" ForeColor="Red" />
                        <asp:Label ID="lblTo" runat="server" Text=" to " />
                        <asp:Label ID="lblToRec" runat="server" ForeColor="Red" />
                        <asp:Label ID="lblOf" runat="server" Text=" of " />
                        <asp:Label ID="lblTotalRecords" runat="server" ForeColor="Red" />
                    </td>
                    <td align="right">
                        <asp:RegularExpressionValidator ID="revGoToPage" runat="server" ControlToValidate="txtGoToPage"
                            ValidationGroup="vgSearch" ErrorMessage="Only numbers can be used for the pages."
                            SetFocusOnError="True" ValidationExpression="\b\d+\b" Height="20px"></asp:RegularExpressionValidator>
                        <asp:Label ID="lblCurrentPage" runat="server"></asp:Label>
                        <asp:Button ID="cmdFirst" runat="server" Text="|<" CssClass="button-search" />
                        <asp:Button ID="cmdPrev" runat="server" Text="<<" CssClass="button-search" />
                        <asp:TextBox ID="txtGoToPage" runat="server" MaxLength="4" Width="25" Height="15px"
                            Font-Size="Small" />
                        <asp:Button ID="cmdGo" runat="server" Text="Go" CssClass="button-search" ValidationGroup="vgSearch" />
                        <asp:Button ID="cmdNext" runat="server" Text=">>" CssClass="button-search" />
                        <asp:Button ID="cmdLast" runat="server" Text=">|" CssClass="button-search" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table width="98%">
                            <asp:Repeater ID="rpInfo" runat="server">
                                <HeaderTemplate>
                                    <tr>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkStatus" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="ddStatusName">Status</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkTAProjectNo" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="TAProjectNo">Project No.</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkPartNo" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="PartNo">Part No</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkNewDesignLevel" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="NewDesignLevel">Design Level</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkPartName" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="NewPartName">Part Name</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkIssueDate" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="IssueDate">Issue Date</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkPreviewTA" ForeColor="white" runat="server">Preview<br />TA</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkPreviewDieshop" ForeColor="white" runat="server">Preview<br />Dieshop</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            History
                                        </td>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td align="left" style="background-color: <%# SetBackGroundColor(Container.DataItem("StatusID")).ToString %>;
                                            color: <%# SetForeGroundColor(Container.DataItem("StatusID")).ToString %>">
                                            <%#DataBinder.Eval(Container, "DataItem.ddStatusName")%>
                                        </td>
                                        <td align="center" style="white-space: nowrap;">
                                            <asp:HyperLink ID="selectTANo" runat="server" Font-Underline="true" NavigateUrl='<%# SetToolingAuthHyperlink(Container.DataItem("TANo"),Container.DataItem("TAProjectNo"),Container.DataItem("ArchiveData")).ToString %>'><%#DataBinder.Eval(Container, "DataItem.ddTAProjectNo")%></asp:HyperLink>
                                        </td>
                                        <td align="left" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.PartNo")%>
                                        </td>
                                        <td align="left" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.NewDesignLevel")%>
                                        </td>
                                        <td align="left" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.PartName")%>
                                        </td>
                                        <td align="center" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.IssueDate")%>
                                        </td>
                                        <td align="center" style="white-space: nowrap;">
                                            <a runat="server" id="aPreviewTA" href="#" onclick='<%# SetPreviewToolingAuthHyperLink(Container.DataItem("TAProjectNo"),Container.DataItem("StatusID"),Container.DataItem("ArchiveData")).ToString %>'>
                                                <asp:Image runat="server" ID="imgPreviewToolingAuth" ImageUrl="~/images/PreviewUp.jpg"
                                                    Visible='<%# SetVisibleToolingAuthHyperLink(Container.DataItem("TAProjectNo"),Container.DataItem("StatusID")).ToString %>' />
                                            </a>
                                        </td>
                                        <td align="center" style="white-space: nowrap;">
                                            <a runat="server" id="aPreviewDieshop" href="#" onclick='<%# SetPreviewDieshopHyperLink(Container.DataItem("TAProjectNo"),Container.DataItem("StatusID"),Container.DataItem("ArchiveData")).ToString %>'>
                                                <asp:Image runat="server" ID="imgPreviewDieshop" ImageUrl="~/images/PreviewUp.jpg"
                                                    Visible='<%# SetVisibleToolingAuthHyperLink(Container.DataItem("TAProjectNo"),Container.DataItem("StatusID")).ToString %>' />
                                            </a>
                                        </td>
                                        <td align="center">
                                            <asp:HyperLink ID="lnkHistory" runat="server" NavigateUrl='<%# "ToolingAuthExpProjHistory.aspx?TANo=" & DataBinder.Eval (Container.DataItem,"TANo").tostring  %>'
                                                ImageUrl="~/images/PreviewUp.jpg" Visible='<%# SetHistoryVisible(Container.DataItem("ArchiveData")).ToString %>' />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="9" style="font-size: xx-small">
                                            Desc:
                                            <%#DataBinder.Eval(Container, "DataItem.TADesc")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="9">
                                            <hr />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                </FooterTemplate>
                            </asp:Repeater>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:RegularExpressionValidator ID="revGoToPageBottom" runat="server" ControlToValidate="txtGoToPageBottom"
                            ValidationGroup="vgSearch" ErrorMessage="Only numbers can be used for the pages."
                            SetFocusOnError="True" ValidationExpression="\b\d+\b" Height="20px"></asp:RegularExpressionValidator>
                        <asp:Label ID="lblCurrentPageBottom" runat="server"></asp:Label>
                        <asp:Button ID="cmdFirstBottom" runat="server" Text="|<" CssClass="button-search" />
                        <asp:Button ID="cmdPrevBottom" runat="server" Text="<<" CssClass="button-search" />
                        <asp:TextBox ID="txtGoToPageBottom" runat="server" MaxLength="4" Width="25" Height="15px"
                            Font-Size="Small" />
                        <asp:Button ID="cmdGoBottom" runat="server" Text="Go" CssClass="button-search" ValidationGroup="vgSearch" />
                        <asp:Button ID="cmdNextBottom" runat="server" Text=">>" CssClass="button-search" />
                        <asp:Button ID="cmdLastBottom" runat="server" Text=">|" CssClass="button-search" />
                    </td>
                </tr>
            </tbody>
        </table>
    </asp:Panel>
</asp:Content>
