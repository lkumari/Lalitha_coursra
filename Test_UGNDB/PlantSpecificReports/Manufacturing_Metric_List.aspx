<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Manufacturing_Metric_List.aspx.vb" Inherits="PlantSpecificReports_Manufacturing_Metric_List" title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin" /><br />
        <asp:ValidationSummary ID="vsSearch" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgSearch" />
        <br />
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
       
        <table width="98%">
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap">
                    Status:
                </td>
                <td>
                    <asp:DropDownList ID="ddStatus" runat="server">
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                </td>
                
            </tr>
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap">
                    Month:
                </td>
                <td>
                    <asp:DropDownList ID="ddMonth" runat="server">
                    </asp:DropDownList>
                </td>
                <td class="p_text" align="right" style="white-space: nowrap">
                    Year:
                </td>
                <td>
                     <asp:DropDownList ID="ddYear" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>                    
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap">
                    UGN Facility:
                </td>
                <td>
                    <asp:DropDownList ID="ddUGNFacility" runat="server">
                    </asp:DropDownList>
                </td>   
                 <td class="p_text">
                    Created by:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddCreatedByTMID" >                       
                    </asp:DropDownList>
                </td>            
            </tr>                      
        </table>
        <table width="98%">
            <tr>
                <td align="center">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CausesValidation="true" ValidationGroup="vgSearch" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="false" />
                </td>
            </tr>
        </table>
        <hr />
        <table width="75%" border="1">
            <tr>
                <td align="center" style="white-space: nowrap;">
                    Completed
                </td>
                <td align="center" style="background-color: yellow; white-space: nowrap;">
                    In-Process
                </td>
                <td align="center" style="background-color: Fuchsia; white-space: nowrap;">
                    Open
                </td>               
            </tr>
        </table>
        <em class="p_smalltextbold">Use the parameters above to filter the list below</em>
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
                        <table width="100%">
                            <asp:Repeater ID="rpSearchResult" runat="server">
                                <SeparatorTemplate>
                                    <tr>
                                        <td colspan="9">
                                            <hr style="height: 0.01em" />
                                        </td>
                                    </tr>
                                </SeparatorTemplate>
                                <HeaderTemplate>
                                    <tr>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkStatus" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="ddStatusName">Status</asp:LinkButton></td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkMonth" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="MonthName">Month</asp:LinkButton></td>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkYear" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="YearID">Year</asp:LinkButton></td>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkUGNFacility" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="UGNFacility">UGN Facility</asp:LinkButton></td>                                       
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:Label ID="lnkPreview" runat="server">Preview</asp:Label>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:Label ID="lnkHistory" runat="server">History</asp:Label>
                                        </td>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td align="left">
                                            <asp:HyperLink ID="selectReportIDfromStatus" runat="server" NavigateUrl='<%# "Manufacturing_Metric_Detail.aspx?ReportID=" & DataBinder.Eval (Container.DataItem,"ReportID").tostring %>'><span style=" text-decoration:underline; color:<%# SetForeGroundColor(Container.DataItem("StatusID")).ToString %>;background-color: <%# SetBackGroundColor(Container.DataItem("StatusID")).ToString %>"> <%#DataBinder.Eval(Container, "DataItem.ddStatusName")%></span> </asp:HyperLink>
                                        </td>
                                        <td align="center">
                                            <asp:HyperLink ID="selectReportIDfromMonth" runat="server" Font-Underline="true" NavigateUrl='<%# "Manufacturing_Metric_Detail.aspx?ReportID=" & DataBinder.Eval (Container.DataItem,"ReportID").tostring %>'><%#DataBinder.Eval(Container, "DataItem.ddMonthName")%></asp:HyperLink>
                                        </td>
                                        <td align="left">
                                            <%#DataBinder.Eval(Container, "DataItem.YearID")%>
                                        </td>
                                        <td align="left">
                                            <%#DataBinder.Eval(Container, "DataItem.ddUGNFacilityName")%>
                                        </td>                                       
                                        <td align="center">
                                            <a runat="server" id="aPreview" href="#" visible='<%# SetPreviewVisible(Container.DataItem("StatusID")).ToString %>'
                                                onclick='<%# SetPreviewHyperLink(Container.DataItem("ReportID")).ToString %>'>
                                                <asp:Image runat="server" ID="imgPreview" ImageUrl="~/images/PreviewUp.jpg" />
                                            </a>
                                        </td>
                                        <td align="center">
                                            <asp:HyperLink ID="lnkHistory" runat="server" NavigateUrl='<%# "Manufacturing_Metric_History.aspx?ReportID=" & DataBinder.Eval (Container.DataItem,"ReportID").tostring %>'
                                                ImageUrl="~/images/PreviewUp.jpg" />
                                        </td>
                                    </tr>                                   
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </asp:Panel>
</asp:Content>

