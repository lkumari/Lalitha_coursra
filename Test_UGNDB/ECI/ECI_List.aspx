<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    MaintainScrollPositionOnPostback="true" CodeFile="ECI_List.aspx.vb" Inherits="ECI_List"
    Title="Engineering Change List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <table width="98%">
            <tr>
                <td class="p_smalltextbold" style="color: #990000" align="left">
                    <asp:Label runat="server" ID="lblReview1" Text="Review existing data or press" />
                    <asp:Button ID="btnAdd" runat="server" Text="Add" Enabled="false" />
                    <asp:Label runat="server" ID="lblReview2" Text="to enter new data." />
                </td>
            </tr>
        </table>
        <asp:Label ID="lblMessage" runat="server"></asp:Label>
        <asp:ValidationSummary runat="server" ID="vsSearch" ValidationGroup="vgSearch" ShowMessageBox="true"
            DisplayMode="BulletList" ShowSummary="true" EnableClientScript="true" />
        <table width="90%">
            <tr>
                <td style="white-space: nowrap;" class="p_text">
                    ECI No:
                </td>
                <td>
                    <asp:TextBox ID="txtSearchECINo" runat="server" MaxLength="9" Width="100px" />
                </td>
                <td style="white-space: nowrap;" class="p_text">
                    ECI Type:
                </td>
                <td>
                    <asp:DropDownList ID="ddSearchECIType" runat="server">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Text="External"></asp:ListItem>
                        <asp:ListItem Text="Internal"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    ECI Status:
                </td>
                <td>
                    <asp:DropDownList ID="ddSearchStatus" runat="server" />
                </td>
                <td style="white-space: nowrap;" class="p_text">
                    Description:
                </td>
                <td>
                    <asp:TextBox ID="txtSearchECIDesc" runat="server" MaxLength="100" Width="200px" />
                </td>
            </tr>
            <tr>
                <td style="white-space: nowrap;" class="p_text">
                    RFD No:
                </td>
                <td>
                    <asp:TextBox ID="txtSearchRFDNo" runat="server" MaxLength="16" Width="100px" />
                </td>
                <td style="white-space: nowrap;" class="p_text">
                    Cost Sheet ID:
                </td>
                <td>
                    <asp:TextBox ID="txtSearchCostSheetID" runat="server" MaxLength="15" Width="100px" />
                </td>
            </tr>
            <tr>
                <td style="white-space: nowrap;" class="p_text">
                    Drawing No.:
                </td>
                <td>
                    <asp:TextBox ID="txtSearchDrawingNo" runat="server" MaxLength="17" Width="194px" />
                </td>
                <td style="white-space: nowrap;" class="p_text">
                    Customer Part No.:
                </td>
                <td>
                    <asp:TextBox ID="txtSearchCustomerPartNo" runat="server" MaxLength="40" Width="200px" />
                </td>
            </tr>
            <tr>
                <td style="white-space: nowrap;" class="p_text">
                    Internal Part No.:
                </td>
                <td>
                    <asp:TextBox ID="txtSearchPartNo" runat="server" MaxLength="40" Width="200px" />
                </td>
                <td class="p_text">
                    Part Name:
                </td>
                <td>
                    <asp:TextBox ID="txtSearchPartName" runat="server" MaxLength="240" Width="200px" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Design Level:
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtSearchDesignLevel" runat="server" MaxLength="30" Width="200px" />
                </td>
            </tr>
            <tr>
                <td style="white-space: nowrap;" class="p_text">
                    Initiator:
                </td>
                <td>
                    <asp:DropDownList ID="ddSearchInitiatorTeamMember" runat="server" />
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
                        <a href="" class="accordionLink">Advanced Search</a></Header>
                    <Content>
                        <asp:CheckBox runat="server" ID="cbShowAdvancedSearch" Text="Keep advanced search open"
                            AutoPostBack="true" /><br />
                        <asp:CheckBox runat="server" ID="cbIncludeArchive" Text="Include Archive Data (WARNING: CHECKING THIS WILL ADD CONSIDERABLE TIME TO WAIT FOR RESULTS.)"
                            AutoPostBack="true" /><br />
                        <table width="90%">
                            <tr>
                                <td style="white-space: nowrap;" class="p_text">
                                    Business Process Type:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddSearchBusinessProcessType" runat="server" />
                                </td>
                                <td style="white-space: nowrap;" class="p_text">
                                    Designation Type:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddSearchDesignationType" runat="server">
                                        <asp:ListItem></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    Issue Date:
                                </td>
                                <td style="white-space: nowrap;">
                                    <asp:TextBox ID="txtSearchIssueDate" runat="server" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton runat="server" ID="imgSearchIssueDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                                    <ajax:CalendarExtender ID="ceSearchIssueDate" runat="server" TargetControlID="txtSearchIssueDate"
                                        PopupButtonID="imgSearchIssueDate" />
                                    <asp:RegularExpressionValidator ID="revSearchIssueDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                        ControlToValidate="txtSearchIssueDate" Font-Bold="True" ToolTip="MM/DD/YYYY"
                                        ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                        Width="8px" ValidationGroup="vgSearch"><</asp:RegularExpressionValidator>
                                </td>
                                <td class="p_text">
                                    Implementation Date:
                                </td>
                                <td style="white-space: nowrap;">
                                    <asp:TextBox ID="txtSearchImplementationDate" runat="server" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton runat="server" ID="imgSearchImplementationDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                                    <ajax:CalendarExtender ID="ceSearchImplementationDate" runat="server" TargetControlID="txtSearchImplementationDate"
                                        PopupButtonID="imgSearchImplementationDate" />
                                    <asp:RegularExpressionValidator ID="revSearchImplementationDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                        ControlToValidate="txtSearchImplementationDate" Font-Bold="True" ToolTip="MM/DD/YYYY"
                                        ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                        Width="8px" ValidationGroup="vgSearch"><</asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    UGN Facility:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddSearchUGNFacility" runat="server" />
                                </td>
                                <td class="p_text">
                                    Customer IPP:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddSearchCustomerIPP" runat="server">
                                        <asp:ListItem Selected="True" Text="All" Value="All"></asp:ListItem>
                                        <asp:ListItem Text="NO Customer IPP" Value="None"></asp:ListItem>
                                        <asp:ListItem Text="Only Customer IPP" Value="Only"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    Customer:
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddSearchCustomer" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    Program:
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddSearchProgram" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    Account Manager:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddSearchAccountManager" runat="server" />
                                </td>
                                <td style="white-space: nowrap;" class="p_text">
                                    Product Technology:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddSearchProductTechnology" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    Vendor:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddSearchVendor" runat="server" />
                                </td>
                                <td class="p_text">
                                    PPAP:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddSearchPPAP" runat="server">
                                        <asp:ListItem Selected="True" Text="All" Value="All"></asp:ListItem>
                                        <asp:ListItem Text="NO PPAP" Value="None"></asp:ListItem>
                                        <asp:ListItem Text="Only PPAP" Value="Only"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    SubFamily:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddSearchSubFamily" runat="server" />
                                </td>
                                <td class="p_text">
                                    UGN IPP:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddSearchUgnIPP" runat="server">
                                        <asp:ListItem Selected="True" Text="All" Value="All"></asp:ListItem>
                                        <asp:ListItem Text="NO UGN IPP" Value="None"></asp:ListItem>
                                        <asp:ListItem Text="Only UGN IPP" Value="Only"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text" valign="top">
                                    Commodity:
                                </td>
                                <td valign="top">
                                    <asp:DropDownList ID="ddSearchCommodity" runat="server" />
                                    <br />
                                    {Commodity / Classification}
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    Purchased Good:
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddSearchPurchasedGood" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    Last Updated On
                                    <br />
                                    (Begin Range):
                                </td>
                                <td style="white-space: nowrap;">
                                    <asp:TextBox ID="txtSearchLastUpdatedOnStartDate" runat="server" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton runat="server" ID="imgSearchLastUpdatedOnStartDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                                    <ajax:CalendarExtender ID="ceSearchLastUpdatedOnStartDate" runat="server" TargetControlID="txtSearchLastUpdatedOnStartDate"
                                        PopupButtonID="imgSearchLastUpdatedOnStartDate" />
                                    <asp:RegularExpressionValidator ID="revSearchLastUpdatedOnStartDate" runat="server"
                                        ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format ' ControlToValidate="txtSearchLastUpdatedOnStartDate"
                                        Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                        Width="8px" ValidationGroup="vgSearch"><</asp:RegularExpressionValidator>
                                </td>
                                <td class="p_text">
                                    Last Updated On
                                    <br />
                                    (End Range):
                                </td>
                                <td style="white-space: nowrap;">
                                    <asp:TextBox ID="txtSearchLastUpdatedOnEndDate" runat="server" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton runat="server" ID="imgSearchLastUpdatedOnEndDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                                    <ajax:CalendarExtender ID="ceSearchLastUpdatedOnEndDate" runat="server" TargetControlID="txtSearchLastUpdatedOnEndDate"
                                        PopupButtonID="imgSearchLastUpdatedOnEndDate" />
                                    <asp:RegularExpressionValidator ID="revSearchLastUpdatedOnEndDate" runat="server"
                                        ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format ' ControlToValidate="txtSearchLastUpdatedOnEndDate"
                                        Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                        Width="8px" ValidationGroup="vgSearch"><</asp:RegularExpressionValidator>
                                </td>
                            </tr>
                        </table>
                    </Content>
                </ajax:AccordionPane>
            </Panes>
        </ajax:Accordion>
        <table width="90%">
            <tr>
                <td align="center">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" ValidationGroup="vgSearch" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" ValidationGroup="vgSearch" />
                </td>
            </tr>
        </table>
        <br />
        <hr />
        <em class="p_smalltextbold" runat="server" id="emTip">Use the parameters above to filter
            the list below. <u>A row with yellow background indicates the ECI has not been released.
                A gray background indicates the ECI has been voided.</u></em>
        <table width="98%">
            <tbody>
                <tr>
                    <td colspan="7" align="right">
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
                    <td colspan="7">
                        <table width="98%">
                            <asp:Repeater ID="rpECIInfo" runat="server">
                                <HeaderTemplate>
                                    <tr>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkECINo" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="ECINo">ECI No</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkECIType" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="ECIType">Type</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkECIStatus" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="StatusName">Status</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkNewDrawingNo" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="NewDrawingNo">New Drawing No</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkNewPartNo" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="NewPartNo">New Internal Part No</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkNewCustomerPartNo" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="NewCustomerPartNo">New Customer Part No</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkNewDesignLevel" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="NewDesignLevel">New Design Level</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkNewPartName" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="NewPartName">New Part Name</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkIssueDate" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="IssueDate">Issue Date</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkPreviewECI" ForeColor="white" runat="server">Preview<br />ECI</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkPreviewUgnIPP" ForeColor="white" runat="server">Preview<br />UGN IPP</asp:LinkButton>
                                        </td>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr style="background-color: <%# SetBackGroundColor(Container.DataItem("StatusID")).ToString %>">
                                        <td align="center" style="white-space: nowrap;">
                                            <asp:HyperLink ID="selectECINo" runat="server" Font-Underline="true" NavigateUrl='<%# SetECIHyperlink(Container.DataItem("ECINo"),Container.DataItem("ECIType"),Container.DataItem("ArchiveData")).ToString %>'><%#DataBinder.Eval(Container, "DataItem.ddECINo")%></asp:HyperLink>
                                        </td>
                                        <td align="left">
                                            <%#DataBinder.Eval(Container, "DataItem.ECIType")%>
                                        </td>
                                        <td align="left" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.StatusName")%>
                                        </td>
                                        <td align="left" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.NewDrawingNo")%>
                                        </td>
                                        <td align="left" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.NewPartNo")%>
                                        </td>
                                        <td align="left" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.NewCustomerPartNo")%>
                                        </td>
                                        <td align="left" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.NewDesignLevel")%>
                                        </td>
                                        <td align="left" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.NewPartName")%>
                                        </td>
                                        <td align="left" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.IssueDate")%>
                                        </td>
                                        <td align="center" style="white-space: nowrap;">
                                            <a runat="server" id="aPreviewECI" href="#" onclick='<%# SetPreviewECIHyperLink(Container.DataItem("ECINo"),Container.DataItem("ECIType"),Container.DataItem("StatusID")).ToString %>'>
                                                <asp:Image runat="server" ID="imgPreviewECI" ImageUrl="~/images/PreviewUp.jpg" Visible='<%# SetVisibleECIHyperLink(Container.DataItem("ECINo"),Container.DataItem("StatusID")).ToString %>' />
                                            </a>
                                        </td>
                                        <td align="center" style="white-space: nowrap;">
                                            <a runat="server" id="aPreviewIPP" href="#" onclick='<%# SetPreviewUgnIPPHyperLink(Container.DataItem("ECINo"),Container.DataItem("StatusID"),Container.DataItem("ArchiveData")).ToString %>'
                                                visible='<%# Eval("isUgnIPP") %>'>
                                                <asp:Image runat="server" ID="imgPreviewIPP" ImageUrl="~/images/PreviewUp.jpg" />
                                            </a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="11" style="font-size: xx-small">
                                            DESCRIPTION:
                                            <%#DataBinder.Eval(Container, "DataItem.DesignDesc")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="11">
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
            </tbody>
        </table>
    </asp:Panel>
</asp:Content>
