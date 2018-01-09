<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    MaintainScrollPositionOnPostback="true" CodeFile="RFD_List.aspx.vb" Inherits="RFD_List"
    EnableEventValidation="false" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <asp:Label ID="lblMessage" runat="server" ForeColor="#ff0000" SkinID="MessageLabelSkin"></asp:Label><br />
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label><br />
        <asp:ValidationSummary ID="vsSearch" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgSearch" />
        <hr />
        <table width="98%">
            <tr>
                <td class="p_smalltextbold" style="color: #990000" colspan="3" align="left">
                    Review existing data or press
                    <asp:Button ID="btnAdd" runat="server" Text="Add" />
                    to enter a new RFD.
                </td>
            </tr>
        </table>
        <hr />
        <table>
            <tr>
                <td>
                    <table width="65%">
                        <tr>
                            <td style="white-space: nowrap;" class="p_text">
                                RFD No:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtSearchRFDNo" MaxLength="10" />
                            </td>
                            <td class="p_text" style="white-space: nowrap;">
                                Drawing No:
                            </td>
                            <td style="white-space: nowrap;">
                                <asp:TextBox runat="server" ID="txtSearchDrawingNo" MaxLength="18" />
                            </td>
                        </tr>
                        <tr>
                            <td style="white-space: nowrap;" class="p_text">
                                Customer Part No:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtSearchCustomerPartNo" MaxLength="40" />
                            </td>
                            <td style="white-space: nowrap;" class="p_text">
                                Design Level:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtSearchDesignLevel" MaxLength="40" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text" style="white-space: nowrap;">
                                Internal Part No:
                            </td>
                            <td style="white-space: nowrap;">
                                <asp:TextBox runat="server" ID="txtSearchPartNo" MaxLength="15" />
                            </td>
                            <td style="white-space: nowrap;" class="p_text">
                                Part Name:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtSearchPartName" MaxLength="30" />
                            </td>
                        </tr>
                        <tr>
                            <td style="white-space: nowrap;" class="p_text">
                                Overall Status:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddSearchStatus" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td style="white-space: nowrap;" class="p_text">
                                Initiator:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddSearchInitiator" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="white-space: nowrap;" class="p_text">
                                Approver Status:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddSearchApproverStatus" runat="server" />
                            </td>
                            <td style="white-space: nowrap;" class="p_text">
                                Approver:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddSearchApprover" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                Role:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddSearchSubscription" runat="server" AppendDataBoundItems="true">
                                    <asp:ListItem Value="0" Text="" />
                                    <asp:ListItem Value="4" Text="Initiator" />
                                </asp:DropDownList>
                            </td>
                            <td class="p_text">
                                Program Manager:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddSearchProgramManager" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="white-space: nowrap;" class="p_text">
                                Business Process Type:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddSearchBusinessProcessType" runat="server" AutoPostBack="true" />
                            </td>
                            <td style="white-space: nowrap;" class="p_text">
                                RFD Desc:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtSearchRFDDesc" MaxLength="100" />
                            </td>
                        </tr>
                        <tr>
                            <td style="white-space: nowrap;" class="p_text">
                                <asp:Label runat="server" ID="lblSearchBusinessProcessAction" Text="Business Process Action:" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddSearchBusinessProcessAction" runat="server" AutoPostBack="true" />
                            </td>
                            <td style="white-space: nowrap;" class="p_text">
                                <asp:Label runat="server" ID="lblSearchBusinessAwarded" Text="Business Awarded:" />
                            </td>
                            <td style="white-space: nowrap;">
                                <asp:DropDownList runat="server" ID="ddSearchBusinessAwarded">
                                    <asp:ListItem></asp:ListItem>
                                    <asp:ListItem Text="Awarded" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="NOT Awarded" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table width="35%" style="border-color: Blue" border="1">
                        <tr align="center">
                            <td>
                                <table width="100%">
                                    <tr align="center">
                                        <td colspan="2">
                                            <h4>
                                                Approval Status Report Selection</h4>
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td class="p_text">
                                            Department/Role:
                                        </td>
                                        <td class="c_text">
                                            <asp:DropDownList ID="ddStatusSubscription" runat="server">
                                                <asp:ListItem Value="0" Text=""></asp:ListItem>
                                                <asp:ListItem Value="4" Text="Plant Champion"></asp:ListItem>
                                                <asp:ListItem Value="9" Text="Sales"></asp:ListItem>
                                                <asp:ListItem Value="5" Text="Product Development"></asp:ListItem>
                                                <asp:ListItem Value="119" Text="Capital"></asp:ListItem>
                                                <asp:ListItem Value="108" Text="Packaging"></asp:ListItem>
                                                <asp:ListItem Value="66" Text="Process"></asp:ListItem>
                                                <asp:ListItem Value="20" Text="Plant Controller"></asp:ListItem>
                                                <asp:ListItem Value="65" Text="Tooling"></asp:ListItem>
                                                <asp:ListItem Value="42" Text="Corporate Engineering (Cap, Proc, Tool)"></asp:ListItem>
                                                <asp:ListItem Value="6" Text="Costing"></asp:ListItem>
                                                <asp:ListItem Value="22" Text="Quality Engineering"></asp:ListItem>
                                                <asp:ListItem Value="7" Text="Purchasing for Contract P.O."></asp:ListItem>
                                                <asp:ListItem Value="139" Text="Purchasing for External RFQ"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td class="p_text">
                                            UGN Facility:
                                        </td>
                                        <td class="c_text">
                                            <asp:DropDownList ID="ddStatusFacility" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td class="p_text">
                                            File Type:
                                        </td>
                                        <td class="c_text">
                                            <asp:DropDownList ID="ddFileType" runat="server">
                                                <asp:ListItem Text="Adobe PDF" Value="PDF"></asp:ListItem>
                                                <asp:ListItem Text="MS Excel With Formatting" Value="XLSX1"></asp:ListItem>
                                                <asp:ListItem Text="MS Excel Without Formatting" Value="XLSX2"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr align="center">
                                        <td colspan="2">
                                            <asp:Button ID="btnStatusReport" runat="server" Text="Status Report" CausesValidation="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <ajax:Accordion ID="accAdvancedSearch" runat="server" SelectedIndex="-1" HeaderCssClass="accordionHeader"
            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
            FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
            RequireOpenedPane="false" SuppressHeaderPostbacks="true">
            <Panes>
                <ajax:AccordionPane ID="apAdvancedSearch" runat="server">
                    <Header>
                        <a href="" class="accordionLink" style="text-decoration: underline">Advanced Search</a></Header>
                    <Content>
                        <asp:CheckBox runat="server" ID="cbShowAdvancedSearch" Text="Keep advanced search open"
                            AutoPostBack="true" Visible="false" /><br />
                        <asp:CheckBox runat="server" ID="cbIncludeArchive" Text="Include Archive Data (WARNING: CHECKING THIS WILL ADD CONSIDERABLE TIME TO WAIT FOR RESULTS.)"
                            AutoPostBack="true" /><br />
                        <table width="90%">
                            <tr>
                                <td style="white-space: nowrap;" class="p_text">
                                    Designation Type:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddSearchDesignationType" runat="server">
                                        <asp:ListItem></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="white-space: nowrap;" class="p_text">
                                    Priority:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddSearchPriority" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text" style="white-space: nowrap;">
                                    Customer:
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddSearchCustomer" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    Account Manager:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddSearchAccountManager" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    UGN Facility:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddSearchUGNFacility" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    Program:
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddSearchProgram" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text" valign="top">
                                    Commodity:
                                </td>
                                <td valign="top" colspan="3">
                                    <asp:DropDownList ID="ddSearchCommodity" runat="server">
                                    </asp:DropDownList>
                                    <br />
                                    {Commodity / Classification}
                                </td>
                            </tr>
                            <tr>
                                <td style="white-space: nowrap;" class="p_text">
                                    Product Technology:
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddSearchProductTechnology" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    SubFamily:
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddSearchSubFamily" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    Vendor:
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddSearchVendor" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    Purchased Good:
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddSearchPurchasedGood" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="white-space: nowrap;" class="p_text">
                                    Cost Sheet ID:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtSearchCostSheetID"></asp:TextBox>
                                </td>
                                <td style="white-space: nowrap;" class="p_text">
                                    ECI No:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtSearchECINo"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="white-space: nowrap;" class="p_text">
                                    CapEx Project No.:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtSearchCapExProjectNo"></asp:TextBox>
                                </td>
                                <td style="white-space: nowrap;" class="p_text">
                                    PO No.:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtSearchPONo"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="white-space: nowrap;" class="p_text">
                                    Due Date
                                    <br />
                                    (Begin Range):
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSearchDueDateStart" runat="server" Width="85px" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton runat="server" ID="imgSearchDueDateStart" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                                    <ajax:CalendarExtender ID="cbeSearchDueDateStart" runat="server" TargetControlID="txtSearchDueDateStart"
                                        PopupButtonID="imgSearchDueDateStart" />
                                    <asp:RegularExpressionValidator ID="revSearchDueDateStart" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                        ControlToValidate="txtSearchDueDateStart" Font-Bold="True" ToolTip="MM/DD/YYYY"
                                        ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                        Width="8px" ValidationGroup="vgSearch"><</asp:RegularExpressionValidator>
                                </td>
                                <td style="white-space: nowrap;" class="p_text">
                                    Last Updated On
                                    <br />
                                    (End Range):
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSearchDueDateEnd" runat="server" Width="85px" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton runat="server" ID="imgSearchDueDateEnd" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                                    <ajax:CalendarExtender ID="cbeSearchDueDateEnd" runat="server" TargetControlID="txtSearchDueDateEnd"
                                        PopupButtonID="imgSearchDueDateEnd" />
                                    <asp:RegularExpressionValidator ID="rfvDueDateEnd" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                        ControlToValidate="txtSearchDueDateEnd" Font-Bold="True" ToolTip="MM/DD/YYYY"
                                        ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                        Width="8px" ValidationGroup="vgSearch"><</asp:RegularExpressionValidator>
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
                    <asp:Button ID="btnSearch" runat="server" Text="Search" ValidationGroup="vgSearch" />
                    &nbsp;
                    <asp:Button ID="btnReset" runat="server" Text="Reset" ValidationGroup="vgSearch" />
                    &nbsp;
                    <asp:Button ID="btnExportToExcel" runat="server" Text="Export to Excel" CausesValidation="true" />
                </td>
            </tr>
        </table>
        <table width="98%" border="1">
            <tr align="center">
                <td style="font-weight: bold">
                    Status Colors
                </td>
                <td style="background-color: Fuchsia; white-space: nowrap;">
                    Open
                </td>
                <td style="background-color: yellow; white-space: nowrap;">
                    In-Process
                </td>
                <td style="background-color: blue; color: white; white-space: nowrap;">
                    On-Hold/Waiting on Cost Sheet Approval
                </td>
                <td style="background-color: red; color: white; white-space: nowrap;">
                    Rejected
                </td>
                <td style="background-color: aqua; white-space: nowrap;">
                    Tasked
                </td>
                <td style="white-space: nowrap;">
                    Approved or N/A
                </td>
                <td style="background-color: gray; color: white; white-space: nowrap;">
                    Void
                </td>
            </tr>
        </table>
        <br />
        <table width="98%" border="1">
            <tr align="center">
                <td style="font-weight: bold">
                    Approval Roles
                </td>
                <td>
                    PD = Product Engineering
                </td>
                <td>
                    Pack = Packaging
                </td>
                <td>
                    PC = Plant Controller/Finance
                </td>
                <td>
                    Proc = Process
                </td>
                <td>
                    Pur Ext = Purchasing For External RFQ
                </td>
                <td>
                    Tlg = Tooling
                </td>
                <td>
                    CAP = Capital
                </td>
                <td>
                    CO = Costing
                </td>
                <td>
                    QE = Quality Engineering
                </td>
                <td>
                    Pur P.O. = Purchasing For Contract P.O.
                </td>
            </tr>
        </table>
        <br />
        <table width="98%">
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
        </table>
        <table width="98%">
            <tbody>
                <tr>
                    <td>
                        <table width="98%">
                            <asp:Repeater ID="rpRFDInfo" runat="server">
                                <HeaderTemplate>
                                    <tr>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkStatusName" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="ddStatusName">Overall <br />Status</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkRFDNo" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="RFDNo">RFD No</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkPreviousRFDNo" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="PreviousRFDNo">Prev. RFD</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkNewDrawingNo" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="NewDrawingNo">Drawing No</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkNewCustomerPartNo" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="NewCustomerPartNo">Cust. Part No</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkNewPartNo" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="NewPartNo">Internal Part No</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkNewPartName" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="NewPartName">Internal Part Name</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkNewDesignLevel" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="NewDesignLevel">Design Level</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            Preview
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            History
                                        </td>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td align="left" style="white-space: nowrap; background-color: <%# SetBackGroundColor(Container.DataItem("StatusID")).ToString %>;
                                            color: <%# SetForeGroundColor(Container.DataItem("StatusID")).ToString %>">
                                            <asp:HyperLink ID="selectRFDStatus" runat="server" Font-Underline="true" NavigateUrl='<%# SetRFDHyperlink(Container.DataItem("RFDNo"),Container.DataItem("BusinessProcessTypeID"),Container.DataItem("ArchiveData")).ToString %>'><%#DataBinder.Eval(Container, "DataItem.ddStatusName")%></asp:HyperLink>
                                        </td>
                                        <td align="center" style="white-space: nowrap;">
                                            <asp:HyperLink ID="selectRFDNo" runat="server" Font-Underline="true" NavigateUrl='<%# SetRFDHyperlink(Container.DataItem("RFDNo"),Container.DataItem("BusinessProcessTypeID"),Container.DataItem("ArchiveData")).ToString %>'><%#DataBinder.Eval(Container, "DataItem.RFDNo")%></asp:HyperLink>
                                        </td>
                                        <td align="left">
                                            <%#DataBinder.Eval(Container, "DataItem.PreviousRFDNo")%>
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
                                        <td align="left">
                                            <%#DataBinder.Eval(Container, "DataItem.NewPartName")%>
                                        </td>
                                        <td align="left">
                                            <%#DataBinder.Eval(Container, "DataItem.NewDesignLevel")%>
                                        </td>
                                        <td align="center">
                                            <a runat="server" id="aPreviewRFD" href="#" onclick='<%# SetPreviewRFDHyperLink(Container.DataItem("RFDNo"),Container.DataItem("StatusID")).ToString %>'>
                                                <asp:Image runat="server" ID="imgPreviewRFD" ImageUrl="~/images/PreviewUp.jpg" Visible='<%# SetPreviewVisible(Container.DataItem("StatusID")).ToString %>' />
                                            </a>
                                        </td>
                                        <td align="center">
                                            <asp:HyperLink ID="lnkHistory" runat="server" NavigateUrl='<%# "RFD_History.aspx?RFDNo=" & DataBinder.Eval (Container.DataItem,"RFDNo").tostring  %>'
                                                ImageUrl="~/images/history.jpg" Visible='<%# SetHistoryVisible(Container.DataItem("ArchiveData")).ToString %>' />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <table width="15%" border="1">
                                                <tr style="font-size: xx-small">
                                                    <td align="left" style="white-space: nowrap; background-color: <%# SetBackGroundColor(Container.DataItem("ProductDevelopmentStatusID")).ToString %>;
                                                        color: <%# SetForeGroundColor(Container.DataItem("ProductDevelopmentStatusID")).ToString %>">
                                                        PD
                                                    </td>
                                                    <td align="left" style="white-space: nowrap; background-color: <%# SetBackGroundColor(Container.DataItem("PackagingStatusID")).ToString %>;
                                                        color: <%# SetForeGroundColor(Container.DataItem("PackagingStatusID")).ToString %>">
                                                        Pack
                                                    </td>
                                                    <td align="left" style="white-space: nowrap; background-color: <%# SetBackGroundColor(Container.DataItem("PlantControllerStatusID")).ToString %>;
                                                        color: <%# SetForeGroundColor(Container.DataItem("PlantControllerStatusID")).ToString %>">
                                                        PC
                                                    </td>
                                                    <td align="left" style="white-space: nowrap; background-color: <%# SetBackGroundColor(Container.DataItem("ProcessStatusID")).ToString %>;
                                                        color: <%# SetForeGroundColor(Container.DataItem("ProcessStatusID")).ToString %>">
                                                        Proc
                                                    </td>
                                                    <td align="left" style="white-space: nowrap; background-color: <%# SetBackGroundColor(Container.DataItem("PurchasingExternalRFQStatusID")).ToString %>;
                                                        color: <%# SetForeGroundColor(Container.DataItem("PurchasingExternalRFQStatusID")).ToString %>">
                                                        Pur Ext. RFQ
                                                    </td>
                                                    <td align="left" style="white-space: nowrap; background-color: <%# SetBackGroundColor(Container.DataItem("ToolingStatusID")).ToString %>;
                                                        color: <%# SetForeGroundColor(Container.DataItem("ToolingStatusID")).ToString %>">
                                                        Tlg
                                                    </td>
                                                    <td align="left" style="white-space: nowrap; background-color: <%# SetBackGroundColor(Container.DataItem("CapitalStatusID")).ToString %>;
                                                        color: <%# SetForeGroundColor(Container.DataItem("CapitalStatusID")).ToString %>">
                                                        CAP
                                                    </td>
                                                    <td align="left" style="white-space: nowrap; background-color: <%# SetBackGroundColor(Container.DataItem("CostingStatusID")).ToString %>;
                                                        color: <%# SetForeGroundColor(Container.DataItem("CostingStatusID")).ToString %>">
                                                        CO
                                                    </td>
                                                    <td align="left" style="white-space: nowrap; background-color: <%# SetBackGroundColor(Container.DataItem("QualityEngineeringStatusID")).ToString %>;
                                                        color: <%# SetForeGroundColor(Container.DataItem("QualityEngineeringStatusID")).ToString %>">
                                                        QE
                                                    </td>
                                                    <td align="left" style="white-space: nowrap; background-color: <%# SetBackGroundColor(Container.DataItem("PurchasingStatusID")).ToString %>;
                                                        color: <%# SetForeGroundColor(Container.DataItem("PurchasingStatusID")).ToString %>">
                                                        Pur P.O.
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td colspan="6" style="font-size: xx-small">
                                            DESCRIPTION:
                                            <%#DataBinder.Eval(Container, "DataItem.RFDDesc")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="10">
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
