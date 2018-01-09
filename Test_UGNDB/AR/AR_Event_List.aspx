<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" MaintainScrollPositionOnPostback="true"
    AutoEventWireup="false" CodeFile="AR_Event_List.aspx.vb" Inherits="AR_Event_List"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin" /><br />
        <asp:ValidationSummary ID="vsSearch" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgSearch" />
        <br />
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <table width="98%">
            <tr>
                <td class="p_smalltextbold" style="color: #990000" colspan="4" align="left">
                    Review existing data or press
                    <asp:Button ID="btnAdd" runat="server" Text="Add" />
                    to enter a new AR Event.
                    <hr />
                </td>
            </tr>
        </table>
        <table width="1000px">
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap">
                    Event Status:
                </td>
                <td>
                    <asp:DropDownList ID="ddEventStatus" runat="server">
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                    <br />
                    <asp:CheckBox runat="server" ID="cbShowVoid" Text="Show Voided Events" />
                </td>
                <td class="p_text" align="right" style="white-space: nowrap">
                    Event Type:
                </td>
                <td>
                    <asp:DropDownList ID="ddEventType" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap">
                    AR Event ID:
                </td>
                <td>
                    <asp:TextBox ID="txtAREID" runat="server" MaxLength="10"></asp:TextBox>
                </td>
                <td class="p_text" align="right" style="white-space: nowrap">
                    Desc:
                </td>
                <td>
                    <asp:TextBox ID="txtEventDesc" runat="server" MaxLength="200"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="p_text" style="white-space: nowrap;">
                    Start Date Range:
                </td>
                <td style="white-space: nowrap;">
                    <asp:TextBox ID="txtCustApprvEffDate" runat="server" Width="85px" MaxLength="10"></asp:TextBox>
                    <asp:ImageButton runat="server" ID="imgCustApprvEffDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                    <ajax:CalendarExtender ID="cbeCustApprvEffDate" runat="server" TargetControlID="txtCustApprvEffDate"
                        PopupButtonID="imgCustApprvEffDate" />
                    <asp:RegularExpressionValidator ID="revCustApprvEffDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtCustApprvEffDate" Font-Bold="True" ToolTip="MM/DD/YYYY"
                        ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px" ValidationGroup="vgSearch" Text="<"></asp:RegularExpressionValidator>
                </td>
                <td class="p_text" style="white-space: nowrap;">
                    End Date Range:
                </td>
                <td>
                    <asp:TextBox ID="txtCustApprvEndDate" runat="server" Width="85px" MaxLength="10"></asp:TextBox>
                    <asp:ImageButton runat="server" ID="imgCustApprvEndDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                    <ajax:CalendarExtender ID="cbeCustApprvEndDate" runat="server" TargetControlID="txtCustApprvEndDate"
                        PopupButtonID="imgCustApprvEndDate" />
                    <asp:RegularExpressionValidator ID="revCustApprvEndDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtCustApprvEndDate" Font-Bold="True" ToolTip="MM/DD/YYYY"
                        ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px" ValidationGroup="vgSearch" Text="<"></asp:RegularExpressionValidator>
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
                    Approved By Customer:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddCustomerApproved">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Text="Approved" Value="1"></asp:ListItem>
                        <asp:ListItem Text="NOT Approved" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text" align="right">
                    Customer:
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddCustomer" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap">
                    Internal Part No:
                </td>
                <td>
                    <asp:TextBox ID="txtPartNo" runat="server" MaxLength="17"></asp:TextBox>
                </td>
                <td class="p_text" align="right" style="white-space: nowrap">
                    Part Name:
                </td>
                <td>
                    <asp:TextBox ID="txtPartName" runat="server" MaxLength="32"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap">
                    Price Code:
                </td>
                <td>
                    <asp:DropDownList ID="ddPriceCode" runat="server">
                    </asp:DropDownList>
                </td>
                <td class="p_text" align="right" style="white-space: nowrap">
                    Account Manager:
                </td>
                <td>
                    <asp:DropDownList ID="ddAccountManager" runat="server">
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="CENTER" colspan="4" style="white-space: nowrap" >
                    <asp:UpdatePanel runat="server" ID="upSearch">
                        <ContentTemplate>
                            <asp:Button ID="btnSearch" runat="server" Text="Search" CausesValidation="true" ValidationGroup="vgSearch" />
                            <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="false" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="CENTER" colspan="4" style="white-space: nowrap">
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="CENTER" colspan="4" style="white-space: nowrap">
                     <asp:UpdateProgress runat="server" ID="pupSearch">
                        <ProgressTemplate>
                            <font size="18pt">
                                <img alt="" src="../images/AJAX/loading.gif" />
                                Please Wait...<img alt="" src="../images/AJAX/loading.gif" /></font>
                        </ProgressTemplate>
                    </asp:UpdateProgress></td>
            </tr>
        </table>
       
        <hr />
        <i>Use the parameters above to filter the list below.</i>
        <table width="480px" border="0">
            <tr>
                <td align="center" width="80px" style="white-space: nowrap;">
                    Completed
                </td>
                <td align="center" width="80px" style="background-color: Fuchsia; white-space: nowrap;">
                    Open
                </td>
                <td align="center" width="80px" style="background-color: yellow; white-space: nowrap;">
                    In-Process
                </td>
                <td align="center" width="80px" style="background-color: red; color: white; white-space: nowrap;">
                    Rejected
                </td>
                <td align="center" width="80px" style="background-color: gray; color: white; white-space: nowrap;">
                    Void
                </td>
            </tr>
        </table>
        <table width="98%">
            <tbody>
                <tr>
                    <td class="c_text" style="font-style: italic" colspan="2">
                        <asp:Label ID="lblRecListed" runat="server" Text="Records Listed: " />
                        <asp:Label ID="lblFromRec" runat="server" ForeColor="Red" />
                        <asp:Label ID="lblTo" runat="server" Text=" to " />
                        <asp:Label ID="lblToRec" runat="server" ForeColor="Red" />
                        <asp:Label ID="lblOf" runat="server" Text=" of " />
                        <asp:Label ID="lblTotalRecords" runat="server" ForeColor="Red" />
                    </td>
                    <td colspan="5" align="right">
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
                                            <asp:LinkButton ID="lnkEventStatus" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="ddEventStatusName">Status</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkAREID" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="AREID">Event ID</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkEventType" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="ddEventTypeName">Type</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkCustApprvEffDate" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="CustApprvEffDate">Cust. Appr. <br />Eff. Date</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkKeyField" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="KeyField">Key Field</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkKeyNewPrice" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="KeyNewPrice">New Price / Percent</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkKeyUGNFacility" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="KeyUGNFacility">UGN Facility</asp:LinkButton>
                                        </td>
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
                                            <asp:HyperLink ID="selectAREIDStatus" runat="server" NavigateUrl='<%# "AR_Event_Detail.aspx?AREID=" & DataBinder.Eval (Container.DataItem,"AREID").tostring %>'><span style=" text-decoration:underline; color:<%# SetEventForeGroundColor(Container.DataItem("EventStatusID")).ToString %>;background-color: <%# SetEventBackGroundColor(Container.DataItem("EventStatusID")).ToString %>"> <%#DataBinder.Eval(Container, "DataItem.ddEventStatusName")%></span> </asp:HyperLink>
                                        </td>
                                        <td align="center">
                                            <asp:HyperLink ID="selectAREID" runat="server" Font-Underline="true" NavigateUrl='<%# "AR_Event_Detail.aspx?AREID=" & DataBinder.Eval (Container.DataItem,"AREID").tostring %>'><%#DataBinder.Eval(Container, "DataItem.ddAREID")%></asp:HyperLink>
                                        </td>
                                        <td align="left">
                                            <%#DataBinder.Eval(Container, "DataItem.ddEventTypeName")%>
                                        </td>
                                        <td align="left">
                                            <%#DataBinder.Eval(Container, "DataItem.CustApprvEffDate")%>
                                        </td>
                                        <td align="left" style="font-weight: bold; white-space: nowrap">
                                            <%#DataBinder.Eval(Container, "DataItem.KeyField")%>
                                        </td>
                                        <td align="center" style="white-space: nowrap">
                                            <%#DataBinder.Eval(Container, "DataItem.KeyNewPrice")%>
                                        </td>
                                        <td align="center" style="white-space: nowrap">
                                            <%#DataBinder.Eval(Container, "DataItem.KeyUGNFacility")%>
                                        </td>
                                        <td align="center">
                                            <a runat="server" id="aPreview" href="#" visible='<%# SetPreviewVisible(Container.DataItem("EventStatusID")).ToString %>'
                                                onclick='<%# SetPreviewHyperLink(Container.DataItem("AREID")).ToString %>'>
                                                <asp:Image runat="server" ID="imgPreview" ImageUrl="~/images/PreviewUp.jpg" />
                                            </a>
                                        </td>
                                        <td align="center">
                                            <asp:HyperLink ID="lnkHistory" runat="server" NavigateUrl='<%# "AR_Event_History.aspx?AREID=" & DataBinder.Eval (Container.DataItem,"AREID").tostring  %>'
                                                ImageUrl="~/images/PreviewUp.jpg" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <table>
                                                <tr style="display: <%# SetApprovalRowDisplay(Container.DataItem("EventStatusID")).ToString %>">
                                                    <td align="left" style="white-space: nowrap;">
                                                        <span style="white-space: nowrap; display: <%# SetApprovalItemDisplay(Container.DataItem("BillingStatusID")).ToString %>;
                                                            background-color: <%# SetApprovalBackGroundColor(Container.DataItem("BillingStatusID")).ToString %>;
                                                            color: <%# SetApprovalForeGroundColor(Container.DataItem("BillingStatusID")).ToString %>">
                                                            Accounting Manager
                                                            <%#DataBinder.Eval(Container, "DataItem.BillingStatusName")%>
                                                        </span>
                                                    </td>
                                                    <td align="left" style="white-space: nowrap; display: <%# SetApprovalItemDisplay(Container.DataItem("SalesStatusID")).ToString %>">
                                                        <span style="white-space: nowrap; background-color: <%# SetApprovalBackGroundColor(Container.DataItem("SalesStatusID")).ToString %>;
                                                            color: <%# SetApprovalForeGroundColor(Container.DataItem("SalesStatusID")).ToString %>">
                                                            Sales
                                                            <%#DataBinder.Eval(Container, "DataItem.SalesStatusName")%></span>
                                                    </td>
                                                    <td align="left" style="white-space: nowrap; display: <%# SetApprovalItemDisplay(Container.DataItem("VPSalesStatusID")).ToString %>">
                                                        <span style="white-space: nowrap; background-color: <%# SetApprovalBackGroundColor(Container.DataItem("VPSalesStatusID")).ToString %>;
                                                            color: <%# SetApprovalForeGroundColor(Container.DataItem("VPSalesStatusID")).ToString %>">
                                                            VP Sales
                                                            <%#DataBinder.Eval(Container, "DataItem.VPSalesStatusName")%></span>
                                                    </td>
                                                    <td align="left" style="white-space: nowrap; display: <%# SetApprovalItemDisplay(Container.DataItem("VPFinanceStatusID")).ToString %>">
                                                        <span style="white-space: nowrap; background-color: <%# SetApprovalBackGroundColor(Container.DataItem("VPFinanceStatusID")).ToString %>;
                                                            color: <%# SetApprovalForeGroundColor(Container.DataItem("VPFinanceStatusID")).ToString %>">
                                                            VP Finance
                                                            <%#DataBinder.Eval(Container, "DataItem.VPFinanceStatusName")%></span>
                                                    </td>
                                                    <td align="left" style="white-space: nowrap; display: <%# SetApprovalItemDisplay(Container.DataItem("CeoStatusID")).ToString %>">
                                                        <span style="white-space: nowrap; background-color: <%# SetApprovalBackGroundColor(Container.DataItem("CeoStatusID")).ToString %>;
                                                            color: <%# SetApprovalForeGroundColor(Container.DataItem("CeoStatusID")).ToString %>">
                                                            CEO
                                                            <%#DataBinder.Eval(Container, "DataItem.CeoStatusName")%></span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td colspan="5">
                                            <%#DataBinder.Eval(Container, "DataItem.EventDesc")%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </td>
                </tr>
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
            </tbody>
        </table>
    </asp:Panel>
</asp:Content>
