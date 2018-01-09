<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Chemical_Review_Form_List.aspx.vb" Inherits="Safety_Chemical_Review_Form_List"
    MaintainScrollPositionOnPostback="true" Title="Chemical Review Form Search" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <asp:Label ID="lblMessage" runat="server" ForeColor="#ff0000" SkinID="MessageLabelSkin"></asp:Label><br />
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label><br />
        <asp:ValidationSummary ID="vsSearch" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgSearch" />
        <hr />
        <table width="75%">
            <tr>
                <td class="p_smalltextbold" style="color: #990000" colspan="3" align="left">
                    Review existing data or press
                    <asp:Button ID="btnAdd" runat="server" Text="Add" />
                    to enter a new chemical review form.
                    <hr />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Chemical Review Formt ID:
                </td>
                <td class="c_textbold">
                    <asp:TextBox runat="server" ID="txtChemRevFormID" MaxLength="10" />
                </td>
                <td class="p_text">
                    Description:
                </td>
                <td>
                    <asp:TextBox ID="txtChemicalDesc" runat="Server" MaxLength="100" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Overall Status:
                </td>
                <td>
                    <asp:DropDownList ID="ddStatus" runat="server">
                    </asp:DropDownList>
                </td>
                <td class="p_text">
                    UGN Facility:
                </td>
                <td>
                    <asp:DropDownList ID="ddUGNFacility" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Requested By:
                </td>
                <td>
                    <asp:DropDownList ID="ddRequestedByTeamMember" runat="server">
                    </asp:DropDownList>
                </td>
                <td class="p_text">
                    Approver:
                </td>
                <td>
                    <asp:DropDownList ID="ddApprover" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Request Date
                    <br />
                    (Begin Range):
                </td>
                <td>
                    <asp:TextBox ID="txtRequestDateStart" runat="server" MaxLength="10"></asp:TextBox>
                    <asp:ImageButton runat="server" ID="imgRequestDateStart" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                    <ajax:CalendarExtender ID="ceRequestDateStart" runat="server" TargetControlID="txtRequestDateStart"
                        PopupButtonID="imgRequestDateStart" />
                    <asp:RegularExpressionValidator ID="revRequestDateStart" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtRequestDateStart" Font-Bold="True" ToolTip="MM/DD/YYYY"
                        ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px" ValidationGroup="vgSearch" Text="<"></asp:RegularExpressionValidator>
                    <asp:RangeValidator ID="rvRequestDateStart" runat="server" Font-Bold="True" Type="Date"
                        ToolTip="The date must be between 1950 and 2100" ErrorMessage="Invalid Date Entry: The date must be between 1950 and 2100"
                        Text="<" ValidationGroup="vgSearch" MaximumValue="01/01/2100" MinimumValue="01/01/1950"
                        ControlToValidate="txtRequestDateStart"></asp:RangeValidator>
                </td>
                <td class="p_text">
                    Request Date
                    <br />
                    (End Range):
                </td>
                <td>
                    <asp:TextBox ID="txtRequestDateEnd" runat="server" Width="85px" MaxLength="10"></asp:TextBox>
                    <asp:ImageButton runat="server" ID="imgRequestDateEnd" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                    <ajax:CalendarExtender ID="cbeRequestDateEnd" runat="server" TargetControlID="txtRequestDateEnd"
                        PopupButtonID="imgRequestDateEnd" />
                    <asp:RegularExpressionValidator ID="rfvRequestDateEnd" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtRequestDateEnd" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px" ValidationGroup="vgSearch"><</asp:RegularExpressionValidator>
                    <asp:RangeValidator ID="rvRequestDateEnd" runat="server" Font-Bold="True" Type="Date"
                        ToolTip="The date must be between 1950 and 2100" ErrorMessage="Invalid Date Entry: The date must be between 1950 and 2100"
                        Text="<" ValidationGroup="vgSearch" MaximumValue="01/01/2100" MinimumValue="01/01/1950"
                        ControlToValidate="txtRequestDateEnd"></asp:RangeValidator>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Product Name:
                </td>
                <td>
                    <asp:TextBox ID="txtProductName" runat="Server" MaxLength="50">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvProductName" runat="server" ControlToValidate="txtProductName"
                        ErrorMessage="Product name is required." Font-Bold="True" ValidationGroup="vgSave"
                        Text="<" SetFocusOnError="true" />
                </td>
                <td class="p_text">
                    Product Manufacturer:
                </td>
                <td>
                    <asp:TextBox ID="txtProductManufacturer" runat="Server" MaxLength="50">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvProductManufacturer" runat="server" ControlToValidate="txtProductManufacturer"
                        ErrorMessage="Product manufacturer is required." Font-Bold="True" ValidationGroup="vgSave"
                        Text="<" SetFocusOnError="true" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Purchase From:
                </td>
                <td>
                    <asp:TextBox ID="txtPurchaseFrom" runat="Server" MaxLength="50">
                    </asp:TextBox>
                </td>
                <td class="p_text">
                    Department / Area:
                </td>
                <td>
                    <asp:TextBox ID="txtDeptArea" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Active:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddActive">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                        <asp:ListItem Text="NOT Active" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table width="75%">
            <tr>
                <td colspan="4" align="center">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" ValidationGroup="vgSearch" />
                    &nbsp;
                    <asp:Button ID="btnReset" runat="server" Text="Reset" ValidationGroup="vgSearch" />
                </td>
            </tr>
        </table>
        <table width="75%" border="1">
            <tr>
                <td align="center" style="white-space: nowrap;">
                    Approved
                </td>
                <td align="center" style="background-color: yellow; white-space: nowrap;">
                    In-Process
                </td>
                <td align="center" style="background-color: Fuchsia; white-space: nowrap;">
                    Open
                </td>
                <td align="center" style="background-color: red; color: white; white-space: nowrap;">
                    Rejected
                </td>
                <td align="center" style="background-color: gray; color: white; white-space: nowrap;">
                    Void
                </td>
            </tr>
        </table>
        <table width="75%">
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
                        <table>
                            <asp:Repeater ID="rpChemicalReviewFormInfo" runat="server">
                                <HeaderTemplate>
                                    <tr>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkStatusName" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="ddStatusName">Overall <br />Status</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkChemicalReviewFormID" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="ChemRevFormID">Chemical Review Form ID</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkRequestDate" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="RequestDate">Request Date</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkProductName" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="ProductName">Product Name</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            Preview
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkRnDStatusName" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="ddRnDStatusName">RnD Status</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkHRSafetyStatusName" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="ddHRSafetyStatusName">HR Safety Status</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkCorpEnvStatusName" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="ddCorpEnvStatusName">Corp Env Status</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkPlantEnvStatusName" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="ddPlantEnvStatusName">Plant Env Status</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkPurchasingStatusName" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="ddPurchasingStatusName">Purchasing Status</asp:LinkButton>
                                        </td>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td align="center" style="white-space: nowrap; background-color: <%# SetBackGroundColor(Container.DataItem("StatusID")).ToString %>;
                                            color: <%# SetForeGroundColor(Container.DataItem("StatusID")).ToString %>">
                                            <%#DataBinder.Eval(Container, "DataItem.ddStatusName")%>
                                        </td>
                                        <td align="center" style="white-space: nowrap;">
                                            <asp:HyperLink ID="selectChemRevFormID" runat="server" Font-Underline="true" NavigateUrl='<%# SetFormHyperlink(Container.DataItem("ChemRevFormID")).ToString %>'><%#DataBinder.Eval(Container, "DataItem.ChemRevFormID")%></asp:HyperLink>
                                        </td>
                                        <td align="left">
                                            <%#DataBinder.Eval(Container, "DataItem.RequestDate")%>
                                        </td>
                                        <td align="left" style="white-space: nowrap;">
                                            <asp:HyperLink ID="selectChemRevFormName" runat="server" Font-Underline="true" NavigateUrl='<%# SetFormHyperlink(Container.DataItem("ChemRevFormID")).ToString %>'><%#DataBinder.Eval(Container, "DataItem.ProductName")%></asp:HyperLink>
                                        </td>
                                        <td align="center">
                                            <a runat="server" id="aPreviewRFD" href="#" visible='<%# SetPreviewVisible(Container.DataItem("StatusID")).ToString %>'
                                                onclick='<%# SetPreviewFormHyperLink(Container.DataItem("ChemRevFormID"),Container.DataItem("StatusID")).ToString %>'>
                                                <asp:Image runat="server" ID="imgPreviewForm" ImageUrl="~/images/PreviewUp.jpg" />
                                            </a>
                                        </td>
                                        <td align="left" style="white-space: nowrap; background-color: <%# SetBackGroundColor(Container.DataItem("RnDStatusID")).ToString %>;
                                            color: <%# SetForeGroundColor(Container.DataItem("RnDStatusID")).ToString %>">
                                            <%#DataBinder.Eval(Container, "DataItem.ddRndStatusName")%>
                                        </td>
                                        <td align="left" style="white-space: nowrap; background-color: <%# SetBackGroundColor(Container.DataItem("HRSafetyStatusID")).ToString %>;
                                            color: <%# SetForeGroundColor(Container.DataItem("HRSafetyStatusID")).ToString %>">
                                            <%#DataBinder.Eval(Container, "DataItem.ddHRSafetyStatusName")%>
                                        </td>
                                        <td align="left" style="white-space: nowrap; background-color: <%# SetBackGroundColor(Container.DataItem("CorpEnvStatusID")).ToString %>;
                                            color: <%# SetForeGroundColor(Container.DataItem("CorpEnvStatusID")).ToString %>">
                                            <%#DataBinder.Eval(Container, "DataItem.ddCorpEnvStatusName")%>
                                        </td>
                                        <td align="left" style="white-space: nowrap; background-color: <%# SetBackGroundColor(Container.DataItem("PlantEnvStatusID")).ToString %>;
                                            color: <%# SetForeGroundColor(Container.DataItem("PlantEnvStatusID")).ToString %>">
                                            <%#DataBinder.Eval(Container, "DataItem.ddPlantEnvStatusName")%>
                                        </td>
                                        <td align="left" style="white-space: nowrap; background-color: <%# SetBackGroundColor(Container.DataItem("PurchasingStatusID")).ToString %>;
                                            color: <%# SetForeGroundColor(Container.DataItem("PurchasingStatusID")).ToString %>">
                                            <%#DataBinder.Eval(Container, "DataItem.ddPurchasingStatusName")%>
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
