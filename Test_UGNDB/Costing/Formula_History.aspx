<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" 
MaintainScrollPositionOnPostback="true"
AutoEventWireup="false" CodeFile="Formula_History.aspx.vb" Inherits="Costing_Formula_History" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" Runat="Server">
<asp:Panel ID="localPanel" runat="server" Width="1000px">
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
        <asp:ValidationSummary ID="vsHistory" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgHistory" />
        <table>
            <tr>
                <td class="p_text">
                    Formuala ID:
                </td>
                <td class="c_textbold" style="color: #990000">
                    <asp:Label ID="lblFormulaID" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Formuala Name:
                </td>
                <td class="p_textbold" style="color: #990000">
                    <asp:Label ID="lblFormulaName" runat="server"></asp:Label>
                    &nbsp;
                    Rev:
                    &nbsp;
                    <asp:Label ID="lblFormulaRevision" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <hr />
        <table>
            <tbody>
                <tr>
                    <td align="right" colspan="8">
                        <asp:RegularExpressionValidator ID="revGoToPage" runat="server" ControlToValidate="txtGoToPage"
                            ValidationGroup="vgHistory" ErrorMessage="Numeric Value Required." SetFocusOnError="True"
                            ValidationExpression="\b\d+\b" Height="20px"></asp:RegularExpressionValidator>
                        <asp:Label ID="lblCurrentPage" runat="server"></asp:Label>
                        <asp:Button ID="cmdFirst" runat="server" Text="|<" CssClass="button-search" />
                        <asp:Button ID="cmdPrev" runat="server" Text="<<" CssClass="button-search" />
                        <asp:TextBox ID="txtGoToPage" runat="server" MaxLength="4" Width="25" Height="15px"
                            Font-Size="Small" />
                        <asp:Button ID="cmdGo" runat="server" Text="Go" CssClass="button-search" ValidationGroup="vgHistory" />
                        <asp:Button ID="cmdNext" runat="server" Text=">>" CssClass="button-search" />
                        <asp:Button ID="cmdLast" runat="server" Text=">|" CssClass="button-search" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        &nbsp;<asp:Repeater ID="rpHistory" runat="server">
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
                                        <asp:Label ID="lblActionDate" runat="server">Action Date</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:Label ID="lblActionTakenBy" runat="server">Action Taken By</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:Label ID="lblDescription" runat="server">Description</asp:Label>
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
                                     <td>
                                        <%#DataBinder.Eval(Container, "DataItem.ActionDesc")%>
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

