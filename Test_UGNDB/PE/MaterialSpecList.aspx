<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="MaterialSpecList.aspx.vb" Inherits="MaterialSpecList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <asp:Label ID="lblMessage" runat="server" ForeColor="#ff0000" SkinID="MessageLabelSkin"></asp:Label><br />
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label><br />
        <asp:ValidationSummary ID="vsSearch" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgSearch" />
        <hr />
        <table width="98%">
            <tr>
                <td class="p_smalltextbold" style="color: #990000"  align="left">
                    Review existing data or press
                    <asp:Button ID="btnAdd" runat="server" Text="Add" />
                    to enter a new material specification.
                    
                </td>
            </tr>
        </table>
        <hr />
        <table width="98%">
            <tr>
                <td style="white-space: nowrap;" class="p_text">
                    Material Specification No:
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchMaterialSpecNo" MaxLength="18"></asp:TextBox>
                </td>
                <td style="white-space: nowrap;" class="p_text">
                    Desc:
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchMaterialSpecDesc" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Family-SubFamily:
                </td>
                <td>
                    <asp:DropDownList ID="ddSubFamily" runat="server">
                    </asp:DropDownList>
                </td>
                  <td style="white-space: nowrap;" class="p_text">
                    Area Weight:
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchMaterialAreaWeight" MaxLength="6"></asp:TextBox>
                </td>
            </tr>
            <tr>
             <td style="white-space: nowrap;" class="p_text">
                    Drawing No:
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchDrawingNo" MaxLength="25"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table width="98%">
            <tr>
                <td colspan="4" align="center">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" ValidationGroup="vgSearch" />
                    &nbsp;
                    <asp:Button ID="btnReset" runat="server" Text="Reset" />
                </td>
            </tr>
        </table>
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
                            <asp:Repeater ID="rpInfo" runat="server">
                                <HeaderTemplate>
                                    <tr>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkMaterialSpecNo" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="MaterialSpecNo">Material Specification No.</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkMaterialSpecDesc" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="MaterialSpecDesc">Description</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkRevisionDate" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="RevisionDate">Revision Date</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkAreaWeight" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="AreaWeight">Area Weight</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkSubfamily" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="SubfamilyID">Sub-Family</asp:LinkButton>
                                        </td>
                                         <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkDrawingNo" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="DrawingNo">Drawing No</asp:LinkButton>
                                        </td>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td align="center" style="white-space: nowrap;">
                                            <asp:HyperLink ID="selectMaterialSpecNo" runat="server" Font-Underline="true" NavigateUrl='<%# SetSelectMaterialSpecNoHyperlink(Container.DataItem("MaterialSpecNo")).ToString %>'><%#DataBinder.Eval(Container, "DataItem.MaterialSpecNo")%></asp:HyperLink>
                                        </td>
                                        <td align="left" style="width: 50%">
                                            <%#DataBinder.Eval(Container, "DataItem.MaterialSpecDesc")%>
                                        </td>
                                        <td align="center" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.ddRevisionDate")%>
                                        </td>
                                        <td align="center" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.AreaWeight")%>
                                        </td>
                                        <td align="center" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.SubfamilyID")%>
                                        </td>
                                        <td align="center" style="white-space: nowrap;">
                                            <asp:HyperLink  Target="_blank" ID="selectDrawingNo" runat="server" Font-Underline="true" NavigateUrl='<%# SetSelectDrawingNoHyperlink(Container.DataItem("DrawingNo")).ToString %>'><%#DataBinder.Eval(Container, "DataItem.DrawingNo")%></asp:HyperLink>
                                        </td>                                       
                                    </tr>
                                    <tr>
                                        <td colspan="6">
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
        <table width="98%">
            <tr>
                <td colspan="7" align="center">
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
        </table>
    </asp:Panel>
</asp:Content>
