<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" CodeFile="DrawingList.aspx.vb"
    Inherits="DrawingList" AutoEventWireup="true" Title="PE Drawings Management System"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="maincontent" runat="Server" ContentPlaceHolderID="maincontent">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <table style="width: 344px">
            <tr>
                <td class="p_smalltextbold" style="color: #990000" colspan="3" align="left">
                    Review existing data or press
                    <asp:Button ID="btnAdd" runat="server" Text="Add" />
                    to enter new data.
                </td>
            </tr>
        </table>
        <asp:Label ID="lblMessage" runat="server" ForeColor="#ff0000" SkinID="MessageLabelSkin"></asp:Label><br />
        <asp:Label ID="lblSearchTip" runat="server" ><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label><br />
        <asp:ValidationSummary ID="vsDrawing" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgDrawing" />
        <hr />
        <table width="98%">
            <tr>
                <td class="p_text">
                    Drawing No:
                </td>
                <td>
                    <asp:TextBox ID="txtDrawingNo" runat="server" Width="200" MaxLength="18"></asp:TextBox>
                </td>
                <td class="p_text" align="right">
                    Customer Part No:
                </td>
                <td>
                    <asp:TextBox ID="txtCustomerPartNo" runat="server" Width="200" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Internal Part No:
                </td>
                <td>
                    <asp:TextBox ID="txtPartNo" runat="server" Width="200" MaxLength="40"></asp:TextBox>
                </td>
                <td class="p_text">
                    Part Name:
                </td>
                <td>
                    <asp:TextBox ID="txtPartName" runat="server" Width="200" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="p_text" valign="top">
                    Commodity:
                </td>
                <td valign="top">
                    <asp:DropDownList ID="ddCommodity" runat="server">
                    </asp:DropDownList>
                    <br />
                    {Commodity / Classification}
                </td>
                <td class="p_text">
                    Designation Type:
                </td>
                <td>
                    <asp:DropDownList ID="ddDesignationType" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Make:
                </td>
                <td>
                    <asp:DropDownList ID="ddMake" runat="server" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
                <td class="p_text">
                    Various Text Fields:
                </td>
                <td>
                    <asp:TextBox ID="txtNotes" runat="server" Width="200" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            
            <tr>
                <td class="p_text">
                    Drawing Release Type:
                </td>
                <td>
                    <asp:DropDownList ID="ddReleaseType" runat="server">
                    </asp:DropDownList>
                </td>
                <!--  asp:ListItem Value="A" Text="Approved" / -->
                <!--  asp:ListItem Value="P" Text="Pending" / -->
                <!--  asp:ListItem Value="R" Text="Rejected" / -->
                <!-- asp:ListItem Value="W" Text="Waived" / -->
                <!-- asp:ListItem Value="M" Text="Waiting for My Approval" / -->
                <td class="p_text">
                    Drawing Status:
                </td>
                <td>
                    <asp:DropDownList ID="ddStatus" runat="server">
                        <asp:ListItem Selected="True" />
                        <asp:ListItem Value="I" Text="Issued" />
                        <asp:ListItem Value="N" Text="New" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Customer:
                </td>
                <td>
                    <asp:DropDownList ID="ddCustomer" runat="server">
                    </asp:DropDownList>
                </td>
                 <td class="p_text">
                    Year:
                </td>
                <td>
                    <asp:DropDownList ID="ddYear" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Program:
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddProgram" runat="server">
                    </asp:DropDownList>
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
                        <table width="90%">
                            <tr>
                                <td class="p_text">
                                    Construction:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtConstruction" runat="server" Width="150" MaxLength="50"></asp:TextBox>
                                </td>
                                <td class="p_text">
                                    Drawing By Engineer:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddDrawingByEngineer" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    Family-SubFamily:
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddSubFamily" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    Purchased Good:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddPurchasedGood" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td class="p_text">
                                    Density Value:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddDensityValue" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    Product Technology:
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddProductTechnology" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    Last Updated On<br />
                                    (Begin Range):
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLastUpdatedOnStart" runat="server" Width="85px" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton runat="server" ID="imgLastUpdatedOnStart" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                                    <ajax:CalendarExtender ID="cbeLastUpdatedOnStart" runat="server" TargetControlID="txtLastUpdatedOnStart"
                                        PopupButtonID="imgLastUpdatedOnStart" />
                                    <asp:RegularExpressionValidator ID="revLastUpdatedOnStart" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                        ControlToValidate="txtLastUpdatedOnStart" Font-Bold="True" ToolTip="MM/DD/YYYY"
                                        ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                        Width="8px" ValidationGroup="vgDrawing"><</asp:RegularExpressionValidator>
                                </td>
                                <td class="p_text">
                                    Last Updated On
                                    <br />
                                    (End Range):
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLastUpdatedOnEnd" runat="server" Width="85px" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton runat="server" ID="imgLastUpdatedOnEnd" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                                    <ajax:CalendarExtender ID="cbeLastUpdatedOnEnd" runat="server" TargetControlID="txtLastUpdatedOnEnd"
                                        PopupButtonID="imgLastUpdatedOnEnd" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                        ControlToValidate="txtLastUpdatedOnEnd" Font-Bold="True" ToolTip="MM/DD/YYYY"
                                        ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                        Width="8px" ValidationGroup="vgDrawing"><</asp:RegularExpressionValidator>
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
                    <asp:Button ID="btnSearch" runat="server" Text="Search" ValidationGroup="vgDrawing" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" />
                </td>
            </tr>
        </table>
        <hr />
        <em class="p_smalltextbold">Use the parameters above to filter the list below</em>
        <table width="98%">
            <tbody>
                <tr>
                    <td colspan="7" align="right">
                        <asp:RegularExpressionValidator ID="revGoToPage" runat="server" ControlToValidate="txtGoToPage"
                            ValidationGroup="vgDrawing" ErrorMessage="Only numbers can be used for the pages."
                            SetFocusOnError="True" ValidationExpression="\b\d+\b" Height="20px"></asp:RegularExpressionValidator>
                        <asp:Label ID="lblCurrentPage" runat="server"></asp:Label>
                        <asp:Button ID="cmdFirst" runat="server" Text="|<" CssClass="button-search" />
                        <asp:Button ID="cmdPrev" runat="server" Text="<<" CssClass="button-search" />
                        <asp:TextBox ID="txtGoToPage" runat="server" MaxLength="4" Width="25" Height="15px"
                            Font-Size="Small" />
                        <asp:Button ID="cmdGo" runat="server" Text="Go" CssClass="button-search" ValidationGroup="vgDrawing" />
                        <asp:Button ID="cmdNext" runat="server" Text=">>" CssClass="button-search" />
                        <asp:Button ID="cmdLast" runat="server" Text=">|" CssClass="button-search" />
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <table width="100%">
                            <asp:Repeater ID="rpDrawingInfo" runat="server">
                                <HeaderTemplate>
                                    <tr>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkDrawingNo" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="DrawingNo">Drawing No.</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkPartNo" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="PartNo">Internal Part No.</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkOldPartName" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="OldPartName">Drawing Name</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkDensityValue" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="DensityValue">Density</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkAMDValue" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="AMDvalue">AMD</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkWMDValue" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="WMDvalue">WMD</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkReleaseType" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="ReleaseType">Release Type</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkStatus" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="ApprovalStatus">Status</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkPreviewHeader" ForeColor="white" runat="server">Preview</asp:LinkButton>
                                        </td>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td align="center" style="white-space: nowrap;">
                                            <asp:HyperLink ID="selectDrawing" runat="server" NavigateUrl='<%# "DrawingDetail.aspx?DrawingNo=" & DataBinder.Eval (Container.DataItem,"DrawingNo").tostring %>'><%#DataBinder.Eval(Container, "DataItem.ddDrawingNo")%></asp:HyperLink>
                                        </td>
                                        <td align="left" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.PartNo")%>
                                        </td>
                                        <td align="left">
                                            <%#DataBinder.Eval(Container, "DataItem.OldPartName")%>
                                        </td>
                                        <td align="center" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.DensityValue")%>
                                        </td>
                                        <td align="center" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.AMDValue")%>
                                        </td>
                                        <td align="center" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.WMDValue")%>
                                        </td>
                                        <td align="left" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.ddReleaseTypeName")%>
                                        </td>
                                        <td align="left" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.approvalStatusDecoded")%>
                                        </td>
                                        <td align="center" style="white-space: nowrap;">
                                            <asp:HyperLink ID="lnkPreview" runat="server" Target="_blank" NavigateUrl='<%# "DMSDrawingPreview.aspx?DrawingNo=" & DataBinder.Eval (Container.DataItem,"DrawingNo").tostring %>'
                                                ImageUrl="~/images/PreviewUp.jpg"><%#DataBinder.Eval(Container, "DataItem.ddDrawingNo")%></asp:HyperLink>
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
