<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    EnableEventValidation="false" 
    CodeFile="Support_List.aspx.vb" Inherits="Support_List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <asp:Label runat="server" ID="lblMessage" SkinID="MessageLabelSkin"></asp:Label>
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label><br />
        <asp:ValidationSummary ID="vsSearch" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgSearch" />
        <hr />
        <table width="98%">
            <tr>
                <td class="p_smalltextbold" style="color: #990000" colspan="3" align="left">
                    Review existing data or press
                    <asp:Button ID="btnAdd" runat="server" Text="Add" />
                    to enter a new support requestor.
                </td>
            </tr>
        </table>
        <hr />
        <table width="98%">
            <tr>
                <td style="white-space: nowrap;" class="p_text">
                    Requestor ID:
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchJobNumber" MaxLength="10"></asp:TextBox>
                </td>
                <td style="white-space: nowrap;" class="p_text">
                    Description:
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchJobDescription" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="white-space: nowrap;" class="p_text">
                    Status:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddSearchStatus">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="Open" Value="Open"></asp:ListItem>
                        <asp:ListItem Text="Closed" Value="Closed"></asp:ListItem>
                        <asp:ListItem Text="Completed" Value="Completed"></asp:ListItem>
                        <asp:ListItem Text="In Process" Value="In Process"></asp:ListItem>
                        <asp:ListItem Text="Hold" Value="Hold"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="white-space: nowrap;" class="p_text">
                    Category:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddSearchCategory">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="white-space: nowrap;" class="p_text">
                    Related To:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddSearchRelatedTo">
                        <asp:ListItem Text="" Value=""></asp:ListItem>
                        <asp:ListItem Text="Helpdesk" Value="H"></asp:ListItem>
                        <asp:ListItem Text="Programming" Value="P"></asp:ListItem>
                        <asp:ListItem Text="Other" Value="O"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="white-space: nowrap;" class="p_text">
                    Requested By:
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchRequestBy" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Module:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddSearchModule">
                    </asp:DropDownList>
                </td>
                <td class="p_text">
                    Assigned To:
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchAssignedTo" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
        </table>
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
                <td style="background-color: Fuchsia; color: white; white-space: nowrap;">
                    Open
                </td>
                <td style="background-color: yellow; white-space: nowrap;">
                    In-Process
                </td>
                <td style="background-color: blue; color: white; white-space: nowrap;">
                    On-Hold
                </td>
                <td style="white-space: nowrap;">
                    Completed/Closed
                </td>
            </tr>
        </table>
        <br />
        <br />
        <table width="98%" runat="server" id="tblPageNavigation">
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
                                            <asp:LinkButton ID="lnkJobNumber" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="JobNumber">Requestor ID</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkStatus" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="Status">Status</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkRequestBy" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="RequestBy">Requested By</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkRequestDate" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="RequestDate">Request Date</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkModule" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="Module">Module</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkCategory" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="Category">Category</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkJobDescription" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="JobDescription">Desc</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkDateCompleted" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="DateCompleted">Date Completed</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkAssignedTo" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="AssignedTo">Assigned To</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            Preview
                                        </td>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td align="center" style="white-space: nowrap;">
                                            <asp:HyperLink ID="selectJobNumber" runat="server" NavigateUrl='<%# "Support_Detail.aspx?JobNumber=" & DataBinder.Eval (Container.DataItem,"JobNumber").tostring %>'><span style=" text-decoration:underline;"> <%#DataBinder.Eval(Container, "DataItem.JobNumber")%></span> </asp:HyperLink>
                                        </td>
                                        <td align="center" style="white-space: nowrap; background-color: <%# SetBackGroundColor(Container.DataItem("Status")).ToString %>;">
                                            <asp:HyperLink ID="selectStatus" runat="server" NavigateUrl='<%# "Support_Detail.aspx?JobNumber=" & DataBinder.Eval (Container.DataItem,"JobNumber").tostring %>'><span style=" text-decoration:underline; color:<%# SetForeGroundColor(Container.DataItem("Status")).ToString %>;"> <%#DataBinder.Eval(Container, "DataItem.Status")%></span> </asp:HyperLink>
                                        </td>
                                        <td align="left">
                                            <%#DataBinder.Eval(Container, "DataItem.RequestBy")%>
                                        </td>
                                        <td align="center" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.RequestDate")%>
                                        </td>
                                        <td align="left" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.Module")%>
                                        </td>
                                        <td align="left" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.Category")%>
                                        </td>
                                        <td align="left">
                                            <%#DataBinder.Eval(Container, "DataItem.JobDescription")%>
                                        </td>
                                        <td align="center">
                                            <%#DataBinder.Eval(Container, "DataItem.DateCompleted")%>
                                        </td>
                                        <td align="left">
                                            <%#DataBinder.Eval(Container, "DataItem.AssignedTo")%>
                                        </td>
                                        <td align="center">
                                            <a runat="server" id="aPreviewJob" href="#" onclick='<%# SetPreviewHyperLink(Container.DataItem("JobNumber")).ToString %>'>
                                                <asp:Image runat="server" ID="imgPreviewJob" ImageUrl="~/images/PreviewUp.jpg" />
                                            </a>
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
