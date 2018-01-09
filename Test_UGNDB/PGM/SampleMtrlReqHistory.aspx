<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="SampleMtrlReqHistory.aspx.vb" Inherits="PGM_SampleMtrlReqHistory" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="1000px">
        <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
            Visible="False"></asp:Label>
        <table>
            <tr>
                <td class="p_text">
                    Request #:
                </td>
                <td class="p_textbold" style="color: #990000">
                    <asp:Label ID="lblSMRNo" runat="server" Text="" />
                </td>
                <td>
                    &nbsp;
                </td>
                <td class="p_text">
                    Sample Description:
                </td>
                <td class="p_textbold" style="color: #990000">
                    <asp:Label ID="lblSampleDesc" runat="server" Text="" />
                </td>
            </tr>
        </table>
        <hr />
        <table id="TABLE1" width="70%">
            <tbody>
                <tr>
                    <td align="right" colspan="8">
                        <asp:RegularExpressionValidator ID="revGoToPage" runat="server" ControlToValidate="txtGoToPage"
                            ValidationGroup="vgTEP" ErrorMessage="Numeric Value Required." SetFocusOnError="True"
                            ValidationExpression="\b\d+\b" Height="20px"></asp:RegularExpressionValidator>
                        <asp:Label ID="lblCurrentPage" runat="server"></asp:Label>
                        <asp:Button ID="cmdFirst" runat="server" Text="|<" CssClass="button-search" />
                        <asp:Button ID="cmdPrev" runat="server" Text="<<" CssClass="button-search" />
                        <asp:TextBox ID="txtGoToPage" runat="server" MaxLength="4" Width="25" Height="15px"
                            Font-Size="Small" />
                        <asp:Button ID="cmdGo" runat="server" Text="Go" CssClass="button-search" ValidationGroup="vgTEP" />
                        <asp:Button ID="cmdNext" runat="server" Text=">>" CssClass="button-search" />
                        <asp:Button ID="cmdLast" runat="server" Text=">|" CssClass="button-search" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        &nbsp;<asp:Repeater ID="rpSampleMtrlReq" runat="server">
                            <SeparatorTemplate>
                                <tr>
                                    <td colspan="4">
                                        <hr style="height: 0.01em" />
                                    </td>
                                </tr>
                            </SeparatorTemplate>
                            <HeaderTemplate>
                                <tr>
                                    <td class="p_tablebackcolor">
                                        <asp:Label ID="Label6" runat="server">Action Date</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:Label ID="Label1" runat="server">Action Taken By</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:Label ID="lnkProgram" runat="server">Description</asp:Label>
                                    </td>
                                </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.ActionDate")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.ActionTakenBy")%>
                                    </td>
                                    <td valign="middle">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:HyperLink ID="lnkCommBoad" runat="server" NavigateUrl='<%# GoToCommBoard(DataBinder.Eval(Container, "DataItem.SMRNo"), DataBinder.Eval(Container, "DataItem.ActionDesc")) %>'
                                                        ImageUrl="~/images/messanger30.jpg" Visible='<%#  ShowHideLink(DataBinder.Eval(Container, "DataItem.ActionDesc")) %>' />
                                                </td>
                                                <td>
                                                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# GoToCommBoard(DataBinder.Eval(Container, "DataItem.SMRNo"), DataBinder.Eval(Container, "DataItem.ActionDesc")) %>'
                                                        Font-Bold='<%#  ShowHideLink(DataBinder.Eval(Container, "DataItem.ActionDesc")) %>'
                                                        Font-Underline='<%#  ShowHideLink(DataBinder.Eval(Container, "DataItem.ActionDesc")) %>'>
                                                    <%#DataBinder.Eval(Container, "DataItem.ActionDesc")%> </asp:HyperLink>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                <tr>
                                    <td colspan="4">
                                        <hr style="height: 0.01em" />
                                    </td>
                                </tr>
                            </FooterTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
            </tbody>
        </table>
    </asp:Panel>
</asp:Content>
<%----%>
