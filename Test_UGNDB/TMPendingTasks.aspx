<%@ Page Language="VB" MasterPageFile="~/crViewTMMasterPage.master" AutoEventWireup="false"
    CodeFile="TMPendingTasks.aspx.vb" Inherits="TMPendingTasks" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="1000px">
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Height="8px" Width="600px" /><br />
        <table width="900">
            <tbody>
                <tr>
                    <td class="p_smalltextbold">
                        <asp:Label ID="lblGreen" BackColor="Lime" runat="server" Text="GREEN (@23hrs or less)" />
                        &nbsp;&nbsp;<asp:Label ID="lblYellow" BackColor="Yellow" runat="server" Text="YELLOW (@24hrs - 47hrs)" />
                        &nbsp;&nbsp;<asp:Label ID="lblRed" BackColor="Red" ForeColor="White" runat="server"
                            Text="RED (@48hrs or greater)" />
                    </td>
                    <td colspan="4" align="right">
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
                    <td colspan="5">
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
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td style="background-color: <%# SetBackGroundColor(Container.DataItem("NoOfHoursOverdue")).ToString%>;">
                                            &nbsp;&nbsp;<asp:Label ID="lblNoOfHrsOverDue" Font-Bold="true" runat="server" ForeColor='<%# SetTextColor(DataBinder.Eval(Container, "DataItem.NoOfHoursOverdue")) %>'
                                                Text='<%# SetTextLabel(DataBinder.Eval(Container, "DataItem.NoOfHoursOverdue")) %>'></asp:Label>
                                        </td>
                                        <td align="center" style="white-space: nowrap;">
                                            <asp:HyperLink ID="selectRecIDPending" runat="server" Font-Underline="true" Target="_blank"
                                                NavigateUrl='<%# GoToApproval(DataBinder.Eval(Container, "DataItem.RecType"),DataBinder.Eval(Container, "DataItem.SecondaryPreview"), DataBinder.Eval(Container, "DataItem.RecID"),DataBinder.Eval(Container, "DataItem.Status"))  %>'><%#DataBinder.Eval(Container, "DataItem.RecID")%></asp:HyperLink>
                                        </td>
                                        <td align="left">
                                            <%#DataBinder.Eval(Container, "DataItem.RecType")%>
                                        </td>
                                        <td align="left">
                                            <%#DataBinder.Eval(Container, "DataItem.Description")%>
                                        </td>
                                        <td align="center" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.DateNotified")%>
                                        </td>
                                        <td align="left">
                                            <%#DataBinder.Eval(Container, "DataItem.Status")%>
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
