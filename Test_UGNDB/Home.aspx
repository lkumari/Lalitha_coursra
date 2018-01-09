<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    MaintainScrollPositionOnPostback="true" EnableEventValidation="false" CodeFile="Home.aspx.vb"
    Inherits="Home" Title="UGN Database Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Label ID="lblWelcomeText" runat="server" ForeColor="Black" Height="30px"></asp:Label>
    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Height="30px"></asp:Label>
    <br />
    <br />
    <asp:LinkButton ID="lnkOldUGNDBSignOn" runat="server" Font-Bold="True" Font-Size="Large"
        Font-Underline="True" ForeColor="Blue">Click here to Sign On to the Classic UGN Database Applications.</asp:LinkButton>
    <hr />
    <ajax:Accordion ID="accPending" runat="server" SelectedIndex="-1" HeaderCssClass="accordionHeader"
        HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
        FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
        RequireOpenedPane="false" SuppressHeaderPostbacks="true" Width="1100PX" Height="50%">
        <Panes>
            <ajax:AccordionPane ID="apPendingHeader" runat="server">
                <Header>
                    <asp:Panel ID="PHPanel" runat="server" CssClass="collapsePanelHeader" Width="680px">
                        <asp:Image ID="imgPH" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                            Height="12px" />&nbsp;<asp:Label ID="lblPH" runat="server" Text="Label" CssClass="c_textbold">PENDING ACTIVITIES:</asp:Label>
                    </asp:Panel>
                    <asp:Panel ID="PHContentPanel" runat="server" CssClass="collapsePanel">
                    </asp:Panel>
                    <ajax:CollapsiblePanelExtender ID="PHExtender" runat="server" TargetControlID="PHContentPanel"
                        ExpandControlID="PHPanel" CollapseControlID="PHPanel" Collapsed="FALSE" TextLabelID="lblPH"
                        ExpandedText="PENDING ACTIVITIES:" CollapsedText="PENDING ACTIVITIES:" ImageControlID="imgPH"
                        CollapsedImage="~/images/expand_blue.jpg" ExpandedImage="~/images/collapse_blue.jpg"
                        SuppressPostBack="true">
                    </ajax:CollapsiblePanelExtender>
                </Header>
                <Content>
                    <%--<ajax:Accordion ID="accPending" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
        HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
        FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
        RequireOpenedPane="false" SuppressHeaderPostbacks="true"  Visible="false" Width="1000PX" Height="30%">
        <Panes>
            <ajax:AccordionPane ID="apPendingHeader" runat="server">
                <Header>
                    <a href="" class="accordionLink">Click here to view your pending activities</a>
                </Header>
                <Content>--%><asp:CheckBox runat="server" ID="cbShowPendingTasks" Text="Keep open"
                    AutoPostBack="true" /><br />
                    <br />
                    <em class="p_textbold" runat="server" id="emTipPending" visible="false">Below is a List
                        Your Pending Activities.</em>
                    <table width="100%">
                        <tbody>
                            <tr>
                                <td class="p_smalltextbold">
                                    <asp:Label ID="lblGreen" BackColor="Lime" runat="server" Text="GREEN (@23hrs or less)" />
                                    &nbsp;&nbsp;<asp:Label ID="lblYellow" BackColor="Yellow" runat="server" Text="YELLOW (@24hrs - 47hrs)" />
                                    &nbsp;&nbsp;<asp:Label ID="lblRed" BackColor="Red" ForeColor="White" runat="server"
                                        Text="RED (@48hrs or greater)" />
                                </td>
                                <td colspan="8" align="right">
                                    <asp:RegularExpressionValidator ID="revGoToPagePending" runat="server" ControlToValidate="txtGoToPagePending"
                                        ValidationGroup="vgPending" ErrorMessage="Only numbers can be used for the pages."
                                        SetFocusOnError="True" ValidationExpression="\b\d+\b" Height="20px"></asp:RegularExpressionValidator>
                                    <asp:Label ID="lblCurrentPagePending" runat="server" Visible="false"></asp:Label>
                                    <asp:Button ID="cmdFirstPending" runat="server" Text="|<" CssClass="button-search"
                                        Visible="false" />
                                    <asp:Button ID="cmdPrevPending" runat="server" Text="<<" CssClass="button-search"
                                        Visible="false" />
                                    <asp:TextBox ID="txtGoToPagePending" runat="server" MaxLength="4" Width="25" Height="15px"
                                        Font-Size="Small" Visible="false" />
                                    <asp:Button ID="cmdGoPending" runat="server" Text="Go" CssClass="button-search" Visible="false"
                                        ValidationGroup="vgPending" />
                                    <asp:Button ID="cmdNextPending" runat="server" Text=">>" CssClass="button-search"
                                        Visible="false" />
                                    <asp:Button ID="cmdLastPending" runat="server" Text=">|" CssClass="button-search"
                                        Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="9">
                                    <table width="100%">
                                        <asp:Repeater ID="rpTasksPending" runat="server" Visible="false">
                                            <HeaderTemplate>
                                                <tr>
                                                    <td class="p_smalltextbold">
                                                        &nbsp;
                                                    </td>
                                                    <td class="p_tablebackcolor" align="center">
                                                        Rec ID #
                                                    </td>
                                                    <td class="p_tablebackcolor" align="left">
                                                        Module
                                                    </td>
                                                    <td class="p_tablebackcolor" align="left">
                                                        Description
                                                    </td>
                                                    <td class="p_tablebackcolor" align="center">
                                                        Date Notified
                                                    </td>
                                                    <td class="p_tablebackcolor" align="left">
                                                        Status
                                                    </td>
                                                    <td class="p_tablebackcolor" align="center">
                                                        Preview<br />
                                                        Primary
                                                    </td>
                                                    <td class="p_tablebackcolor" align="center">
                                                        Preview<br />
                                                        Secondary
                                                    </td>
                                                    <td class="p_tablebackcolor" align="center">
                                                        History
                                                    </td>
                                                </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td style="background-color: <%# SetBackGroundColor(Container.DataItem("NoOfHoursOverdue")).ToString%>;">
                                                        &nbsp;&nbsp;<asp:Label ID="lblNoOfHrsOverDue" Font-Bold="true" runat="server" ForeColor='<%# SetTextColor(DataBinder.Eval(Container, "DataItem.NoOfHoursOverdue")) %>'
                                                            Text='<%# SetTextLabel(DataBinder.Eval(Container, "DataItem.NoOfHoursOverdue")) %>'></asp:Label>
                                                    </td>
                                                    <td align="center" style="white-space: nowrap;">
                                                        <asp:HyperLink ID="selectRecIDPending" runat="server" Font-Underline="true" NavigateUrl='<%# GoToApproval(DataBinder.Eval(Container, "DataItem.RecType"),DataBinder.Eval(Container, "DataItem.SecondaryPreview"),DataBinder.Eval(Container, "DataItem.RecID"),DataBinder.Eval(Container, "DataItem.Status"))  %>'><%#DataBinder.Eval(Container, "DataItem.RecID")%></asp:HyperLink>
                                                    </td>
                                                    <td align="left" style="white-space: nowrap;">
                                                        <%#DataBinder.Eval(Container, "DataItem.RecType")%>
                                                    </td>
                                                    <td align="left" style="white-space: nowrap;">
                                                        <%#DataBinder.Eval(Container, "DataItem.Description")%>
                                                    </td>
                                                    <td align="center" style="white-space: nowrap;">
                                                        <%#DataBinder.Eval(Container, "DataItem.DateNotified")%>
                                                    </td>
                                                    <td align="left">
                                                        <%#DataBinder.Eval(Container, "DataItem.Status")%>
                                                    </td>
                                                    <td align="center" style="white-space: nowrap;">
                                                        <a runat="server" id="aPreviewPrimaryPending" href='<%# SetPrimaryPreviewHyperLinkHREF(DataBinder.Eval(Container, "DataItem.PrimaryPreview"),DataBinder.Eval(Container, "DataItem.RecID")).ToString %>'
                                                            onclick='<%# SetPrimaryPreviewHyperLinkOnClick(DataBinder.Eval(Container, "DataItem.PrimaryPreview"),DataBinder.Eval(Container, "DataItem.SecondaryPreview"),DataBinder.Eval(Container, "DataItem.RecID")).ToString %>'>
                                                            <asp:Image runat="server" ID="imgPreviewPrimaryPending" ImageUrl="~/images/PreviewUp.jpg"
                                                                ToolTip='<%# GetToolTip(DataBinder.Eval(Container, "DataItem.PrimaryPreview")) %>' />
                                                        </a>
                                                    </td>
                                                    <td align="center" style="white-space: nowrap;">
                                                        <a runat="server" id="aPreviewSecondaryPending" href="#" onclick='<%# SetSecondaryPreviewHyperLink(DataBinder.Eval(Container, "DataItem.SecondaryPreview"),DataBinder.Eval(Container, "DataItem.RecID")).ToString %>'>
                                                            <asp:Image runat="server" ID="imgPreviewSecondaryPending" ImageUrl="~/images/PreviewUp.jpg"
                                                                Visible='<%# SetPreviewVisible(DataBinder.Eval(Container, "DataItem.SecondaryPreview"))  %>'
                                                                ToolTip='<%# GetToolTip(DataBinder.Eval(Container, "DataItem.SecondaryPreview")) %>' />
                                                        </a>
                                                    </td>
                                                    <td align="center" style="white-space: nowrap;">
                                                        <%-- If ViewState("vHplnk") = False Then --%><a runat="server" id="aHistoryPending"
                                                            href='<%# SetHistoryHyperLinkHREF(DataBinder.Eval(Container, "DataItem.RecType"),DataBinder.Eval(Container, "DataItem.RecID")).ToString %>'
                                                            onclick='<%# SetHistoryHyperLinkOnclick(DataBinder.Eval(Container, "DataItem.RecType"),DataBinder.Eval(Container, "DataItem.RecID")).ToString %>'><asp:Image
                                                                runat="server" ID="imgHistoryPending" ImageUrl="~/images/History.jpg" Visible='<%# SetPreviewVisible(DataBinder.Eval(Container, "DataItem.History"))  %>' /></a><%-- Else 
                                                        <asp:HyperLink ID="hlHistory" runat="server" 
                                                            NavigateUrl='<%# SetHistoryHyperLink(DataBinder.Eval(Container, "DataItem.RecType"),DataBinder.Eval(Container, "DataItem.RecID")).ToString %>'
                                                            ImageUrl="~/images/History.jpg" ></asp:HyperLink>
                                                        <%End If --%>
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
                </Content>
            </ajax:AccordionPane>
        </Panes>
    </ajax:Accordion>
    <br />
    <ajax:Accordion ID="accRecent" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
        HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
        FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
        RequireOpenedPane="false" SuppressHeaderPostbacks="true" Visible="false">
        <Panes>
            <ajax:AccordionPane ID="apRecentHeader" runat="server">
                <Header>
                    <a href="" class="accordionLink">+ Click here to view your most recent updates</a>
                </Header>
                <Content>
                    <asp:CheckBox runat="server" ID="cbShowRecentTasks" Text="Keep open" AutoPostBack="true" /><br />
                    <br />
                    <em class="p_textbold" runat="server" id="emTipRecent" visible="false">Below is the
                        list of your most recent updates to the AR, Costing, ECI, and Safety Modules the
                        past two days.</em>
                    <table width="98%">
                        <tbody>
                            <tr>
                                <td colspan="7" align="right">
                                    <asp:RegularExpressionValidator ID="revGoToPageRecent" runat="server" ControlToValidate="txtGoToPageRecent"
                                        ValidationGroup="vgRecent" ErrorMessage="Only numbers can be used for the pages."
                                        SetFocusOnError="True" ValidationExpression="\b\d+\b" Height="20px"></asp:RegularExpressionValidator>
                                    <asp:Label ID="lblCurrentPageRecent" runat="server" Visible="false"></asp:Label>
                                    <asp:Button ID="cmdFirstRecent" runat="server" Text="|<" CssClass="button-search"
                                        Visible="false" />
                                    <asp:Button ID="cmdPrevRecent" runat="server" Text="<<" CssClass="button-search"
                                        Visible="false" />
                                    <asp:TextBox ID="txtGoToPageRecent" runat="server" MaxLength="4" Width="25" Height="15px"
                                        Font-Size="Small" Visible="false" />
                                    <asp:Button ID="cmdGoRecent" runat="server" Text="Go" CssClass="button-search" Visible="false"
                                        ValidationGroup="vgRecent" />
                                    <asp:Button ID="cmdNextRecent" runat="server" Text=">>" CssClass="button-search"
                                        Visible="false" />
                                    <asp:Button ID="cmdLastRecent" runat="server" Text=">|" CssClass="button-search"
                                        Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7">
                                    <table width="100%">
                                        <asp:Repeater ID="rpTasksRecent" runat="server" Visible="false">
                                            <HeaderTemplate>
                                                <tr>
                                                    <td class="p_tablebackcolor" align="center">
                                                        Rec ID #
                                                    </td>
                                                    <td class="p_tablebackcolor" align="left">
                                                        Module
                                                    </td>
                                                    <td class="p_tablebackcolor" align="left">
                                                        Description
                                                    </td>
                                                    <td class="p_tablebackcolor" align="center">
                                                        Date Updated
                                                    </td>
                                                    <td class="p_tablebackcolor" align="left">
                                                        Status
                                                    </td>
                                                    <td class="p_tablebackcolor" align="center">
                                                        Preview<br />
                                                        Primary
                                                    </td>
                                                    <td class="p_tablebackcolor" align="center">
                                                        Preview<br />
                                                        Secondary
                                                    </td>
                                                    <td class="p_tablebackcolor" align="center">
                                                        History
                                                    </td>
                                                </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td align="center" style="white-space: nowrap;">
                                                        <asp:HyperLink ID="selectRecIDRecent" runat="server" Font-Underline="true" NavigateUrl='<%# GoToView(DataBinder.Eval(Container, "DataItem.RecType"),DataBinder.Eval(Container, "DataItem.SecondaryPreview"),DataBinder.Eval(Container, "DataItem.RecID"))  %>'><%#DataBinder.Eval(Container, "DataItem.RecID")%></asp:HyperLink>
                                                    </td>
                                                    <td align="left">
                                                        <%#DataBinder.Eval(Container, "DataItem.RecType")%>
                                                    </td>
                                                    <td align="left">
                                                        <%#DataBinder.Eval(Container, "DataItem.Description")%>
                                                    </td>
                                                    <td align="center" style="white-space: nowrap;">
                                                        <%#DataBinder.Eval(Container, "DataItem.DateUpdated")%>
                                                    </td>
                                                    <td align="left">
                                                        <%#DataBinder.Eval(Container, "DataItem.Status")%>
                                                    </td>
                                                    <td align="center" style="white-space: nowrap;">
                                                        <a runat="server" id="aPreviewPrimaryRecent" href='<%# SetPrimaryPreviewHyperLinkHREF(DataBinder.Eval(Container, "DataItem.PrimaryPreview"),DataBinder.Eval(Container, "DataItem.RecID")).ToString %>'
                                                            onclick='<%# SetPrimaryPreviewHyperLinkOnClick(DataBinder.Eval(Container, "DataItem.PrimaryPreview"),DataBinder.Eval(Container, "DataItem.SecondaryPreview"),DataBinder.Eval(Container, "DataItem.RecID")).ToString %>'>
                                                            <asp:Image runat="server" ID="imgPreviewPrimaryRecent" ImageUrl="~/images/PreviewUp.jpg"
                                                                ToolTip='<%# GetToolTip(DataBinder.Eval(Container, "DataItem.PrimaryPreview")) %>' />
                                                        </a>
                                                    </td>
                                                    <td align="center" style="white-space: nowrap;">
                                                        <a runat="server" id="aPreviewSecondaryRecent" href="#" onclick='<%# SetSecondaryPreviewHyperLink(DataBinder.Eval(Container, "DataItem.SecondaryPreview"),DataBinder.Eval(Container, "DataItem.RecID")).ToString %>'>
                                                            <asp:Image runat="server" ID="imgPreviewSecondaryRecent" ImageUrl="~/images/PreviewUp.jpg"
                                                                Visible='<%# SetPreviewVisible(DataBinder.Eval(Container, "DataItem.SecondaryPreview"))  %>'
                                                                ToolTip='<%# GetToolTip(DataBinder.Eval(Container, "DataItem.SecondaryPreview")) %>' />
                                                        </a>
                                                    </td>
                                                    <td align="center" style="white-space: nowrap;">
                                                        <a runat="server" id="aHistoryRecent" href='<%# SetHistoryHyperLinkHREF(DataBinder.Eval(Container, "DataItem.RecType"),DataBinder.Eval(Container, "DataItem.RecID")).ToString %>'
                                                            onclick='<%# SetHistoryHyperLinkOnclick(DataBinder.Eval(Container, "DataItem.RecType"),DataBinder.Eval(Container, "DataItem.RecID")).ToString %>'>
                                                            <asp:Image runat="server" ID="imgHistoryRecent" ImageUrl="~/images/History.jpg" Visible='<%# SetPreviewVisible(DataBinder.Eval(Container, "DataItem.History"))  %>' />
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
                </Content>
            </ajax:AccordionPane>
        </Panes>
    </ajax:Accordion>
    <br />
    <asp:Label SkinID="MessageLabelSkin" ID="lblClassicASPInstructions" runat="server"
        Visible="false">The following applications are scheduled to be integrated.</asp:Label>
    <br />
    <asp:LinkButton ID="lnkOldUGNDBMainMenu" runat="server" Visible="false">Classic UGN DB Main Menu</asp:LinkButton>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
</asp:Content>
