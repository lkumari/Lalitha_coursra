<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    MaintainScrollPositionOnPostback="true" CodeFile="ECI_Detail.aspx.vb" Inherits="ECI_Detail"
    Title="Engineering Change Instruction" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSave" Width="1000px">
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
        <asp:ValidationSummary runat="server" ID="vsSave" ValidationGroup="vgSave" ShowMessageBox="true"
            ShowSummary="true" />
        <table width="98%">
            <tr>
                <td align="center" colspan="2">
                    <asp:Button ID="btnSave" runat="server" Text="Save" Visible="false" ValidationGroup="vgSave" />
                    <asp:Button ID="btnCopy" runat="server" Text="Copy" Visible="false" CausesValidation="false" />
                    <asp:Button ID="btnVoid" runat="server" Text="Void" Visible="false" CausesValidation="false" />
                    <asp:Button ID="btnPreviewECI" runat="server" Text="Preview ECI" Visible="false"
                        CausesValidation="false" />
                    <asp:Button ID="btnPreviewUgnIPP" runat="server" Text="Preview UGN IPP" Visible="false"
                        CausesValidation="false" />
                    <asp:Button ID="btnPreviewCustomerIPP" runat="server" Text="Preview Customer IPP"
                        Visible="false" CausesValidation="false" />
                    <asp:Button ID="btnEdit" runat="server" Text="Edit" Visible="false" CausesValidation="false" />
                    <asp:Button ID="btnUpdate" runat="server" Text="Update And Notify" Visible="false"
                        ValidationGroup="vgSave" />
                    <asp:Button ID="btnCancelEdit" runat="server" Text="Cancel Edit" Visible="false"
                        CausesValidation="false" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblVoidCommentMarker" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="* " Visible="false" />
                    <asp:Label runat="server" ID="lblVoidComment" Text="Void Comments:" Visible="false"
                        SkinID="MessageLabelSkin"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtVoidComment" runat="server" Height="60px" TextMode="MultiLine"
                        Width="757px" MaxLength="150" Visible="false"></asp:TextBox>
                    <br />
                    <asp:Label ID="lblVoidCommentCharCount" SkinID="MessageLabelSkin" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <table width="500px">
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblECIStatusLabel" runat="Server" Text="ECI Status:" />
                </td>
                <td>
                    <asp:Label ID="lblECIStatusValue" runat="Server" SkinID="MessageLabelSkin" />
                </td>
                <td class="p_text" style="white-space: nowrap;">
                    Issue Date:
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblIssueDate" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="p_text" style="white-space: nowrap;">
                    ECI No:
                </td>
                <td>
                    <asp:Label runat="server" ID="lblECINo" CssClass="c_textbold" />
                </td>
                <td class="p_text" style="white-space: nowrap;">
                    Previous ECI No:
                </td>
                <td>
                    <asp:HyperLink runat="server" ID="hlnkPreviousECINo" Font-Underline="true" Font-Bold="true"
                        ToolTip="Click here to see previous ECI." Target="_blank" />
                </td>
            </tr>
        </table>
        <ajax:Accordion ID="accECIHeader" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
            FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
            RequireOpenedPane="false" SuppressHeaderPostbacks="true">
            <Panes>
                <ajax:AccordionPane ID="apECIHeader" runat="server">
                    <Header>
                        <a href="">Description</a></Header>
                    <Content>
                        <table width="900px">
                            <tr>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    <asp:Label ID="lblECITypeMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                        Text="* " />
                                    ECI Type:
                                </td>
                                <td class="c_textbold" style="white-space: nowrap;">
                                    <asp:DropDownList ID="ddECIType" runat="server" AutoPostBack="true">
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem Text="External"></asp:ListItem>
                                        <asp:ListItem Text="Internal"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvECIType" ControlToValidate="ddECIType"
                                        SetFocusOnError="true" ErrorMessage="ECI type is required" Text="<" ValidationGroup="vgSave" />
                                </td>
                                <td class="p_text">
                                    <asp:Label ID="lblBusinessProcessTypeMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                        Text="* " />
                                    Business Process Type:
                                </td>
                                <td class="c_textbold">
                                    <asp:DropDownList ID="ddBusinessProcessType" runat="server">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvBusinessProcessType" ControlToValidate="ddBusinessProcessType"
                                        SetFocusOnError="true" ErrorMessage="Business process type is required" Text="<"
                                        ValidationGroup="vgSave" />
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    <asp:Label ID="lblPriceCodeMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                        Text="* " />
                                    Price Code
                                    <br />
                                    (Production Status):
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddPriceCode" runat="server">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvPriceCode" ControlToValidate="ddPriceCode"
                                        SetFocusOnError="true" ErrorMessage="Price Code is required" Text="<" ValidationGroup="vgSave" />
                                </td>
                                <td class="p_text">
                                    <asp:Label ID="lblDesignationTypeMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                        Text="* " />
                                    Designation Type:
                                </td>
                                <td class="c_textbold">
                                    <asp:DropDownList ID="ddDesignationType" runat="server">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvDesignationType" ControlToValidate="ddDesignationType"
                                        SetFocusOnError="true" ErrorMessage="Designation type is required" Text="<" ValidationGroup="vgSave" />
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    <asp:Label ID="lblCostSheetIDMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                        Text="* " />
                                    Cost Sheet ID:
                                </td>
                                <td class="c_textbold" style="white-space: nowrap;" colspan="3">
                                    <asp:TextBox runat="server" ID="txtCostSheetID" MaxLength="10"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvCostSheetID" ControlToValidate="txtCostSheetID"
                                        SetFocusOnError="true" ErrorMessage="Cost Sheet is required" Text="<" ValidationGroup="vgSave" />
                                    <asp:RegularExpressionValidator ID="revCostSheetID" runat="server" ControlToValidate="txtCostSheetID"
                                        ValidationGroup="vgSave" Text="<" ErrorMessage="Only numbers can be used for the Cost Sheet."
                                        SetFocusOnError="True" ValidationExpression="\b\d+\b"></asp:RegularExpressionValidator>
                                    &nbsp;
                                    <asp:HyperLink runat="server" ID="hlnkCostSheet" Font-Underline="true" ToolTip="Click here to see Cost Sheet."
                                        Target="_blank" Text="View Cost Sheet" Visible="false"></asp:HyperLink>&nbsp;
                                    &nbsp;
                                    <asp:HyperLink runat="server" ID="hlnkDieLayout" Font-Underline="true" ToolTip="Click here to see Die Layout."
                                        Target="_blank" Text="View Die Layout" Visible="false"></asp:HyperLink>&nbsp;
                                    <asp:ImageButton ID="iBtnCostSheetCopy" runat="server" ImageUrl="~/images/SelectUser.gif"
                                        ToolTip="Click here to copy details based on Cost Sheet." Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    RFD No:
                                </td>
                                <td class="c_textbold" colspan="3">
                                    <asp:TextBox runat="server" ID="txtRFDNo" MaxLength="10"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="revRFDNo" runat="server" ControlToValidate="txtRFDNo"
                                        ValidationGroup="vgSave" Text="<" ErrorMessage="Only numbers can be used for the RFD."
                                        SetFocusOnError="True" ValidationExpression="\b\d+\b"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    <asp:Label ID="lblInitiatorTeamMemberMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                        Text="* " />
                                    Initiator:
                                </td>
                                <td class="c_textbold">
                                    <asp:DropDownList ID="ddInitiatorTeamMember" runat="server">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="rfInitiatorTeamMember" ControlToValidate="ddInitiatorTeamMember"
                                        SetFocusOnError="true" ErrorMessage="Initiator is required" Text="<" ValidationGroup="vgSave" />
                                </td>
                                <td class="p_text" style="white-space: nowrap;">
                                    <asp:Label ID="lblImplementationDateMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                        Text="* " />
                                    Implementation Date:
                                </td>
                                <td class="c_textbold" style="white-space: nowrap;">
                                    <asp:TextBox ID="txtImplementationDate" runat="server" MaxLength="10"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvImplementationDate" ControlToValidate="txtImplementationDate"
                                        SetFocusOnError="true" ErrorMessage="Implementation date is required" Text="<"
                                        ValidationGroup="vgSave" />
                                    <asp:ImageButton runat="server" ID="imgImplementationDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                                    <ajax:CalendarExtender ID="ceImplementationDate" runat="server" TargetControlID="txtImplementationDate"
                                        PopupButtonID="imgImplementationDate" />
                                    <asp:RegularExpressionValidator ID="revImplementationDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                        ControlToValidate="txtImplementationDate" Font-Bold="True" ToolTip="MM/DD/YYYY"
                                        ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                        Width="8px" ValidationGroup="SearchInfo" Text="<"></asp:RegularExpressionValidator>
                                    <asp:RangeValidator ID="rvImplementationDate" runat="server" Font-Bold="True" Type="Date"
                                        ToolTip="The date must be between 1950 and 2100" ErrorMessage="Invalid Date Entry: The date must be between 1950 and 2100"
                                        Text="<" ValidationGroup="vgSave" MaximumValue="01/01/2100" MinimumValue="01/01/1950"
                                        ControlToValidate="txtImplementationDate"></asp:RangeValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    Account Manager:
                                </td>
                                <td class="c_textbold">
                                    <asp:DropDownList ID="ddAccountManager" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td class="p_text">
                                    Quality Engineer:
                                </td>
                                <td class="c_textbold">
                                    <asp:DropDownList ID="ddQualityEngineer" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    <asp:Label ID="lblDesignDescMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                        Text="* " />
                                    Design Desc.:
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtDesignDesc" runat="server" TextMode="MultiLine" Height="90px"
                                        Width="500px" />
                                    <asp:RequiredFieldValidator runat="server" ID="rfvDesignDesc" ControlToValidate="txtDesignDesc"
                                        SetFocusOnError="true" ErrorMessage="Design decription is required" Text="<"
                                        ValidationGroup="vgSave" />
                                    <br />
                                    <asp:Label ID="lblDesignDescCharCount" SkinID="MessageLabelSkin" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    Initial Part Production (IPP) Desc:
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtIPPDesc" runat="server" TextMode="MultiLine" Height="90px" Width="500px" />
                                    <br />
                                    <asp:Label ID="lblIPPDescCharCount" SkinID="MessageLabelSkin" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text" style="white-space: nowrap;">
                                    Customer IPP:
                                </td>
                                <td>
                                    <asp:CheckBox ID="cbCustomerIPP" runat="server" />
                                    &nbsp;(Supporting Document Needed)
                                </td>
                                <td class="p_text" style="white-space: nowrap;">
                                    UGN IPP:
                                </td>
                                <td>
                                    <asp:CheckBox ID="cbUGNIPP" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text" valign="top">
                                    IPP Date:
                                </td>
                                <td valign="top">
                                    <asp:TextBox ID="txtIPPDate" runat="server" MaxLength="10"></asp:TextBox>
                                    <asp:ImageButton runat="server" ID="imgIPPDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                                    <ajax:CalendarExtender ID="ceIPPDate" runat="server" TargetControlID="txtIPPDate"
                                        PopupButtonID="imgIPPDate" />
                                    <asp:RegularExpressionValidator ID="revIPPDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                        ControlToValidate="txtIPPDate" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                        Width="8px" ValidationGroup="SearchInfo"><</asp:RegularExpressionValidator>
                                    <asp:RangeValidator ID="rvIPPDate" runat="server" Font-Bold="True" Type="Date" ToolTip="The date must be between 1950 and 2100"
                                        ErrorMessage="Invalid Date Entry: The date must be between 1950 and 2100" Text="<"
                                        ValidationGroup="vgSave" MaximumValue="01/01/2100" MinimumValue="01/01/1950"
                                        ControlToValidate="txtIPPDate"></asp:RangeValidator>
                                </td>
                                <td class="p_text" valign="top">
                                    <asp:Label ID="lblExistingMaterialAction" runat="server" Font-Bold="True" ForeColor="Red"
                                        Text="* " />
                                    Existing Material Action:
                                </td>
                                <td class="c_textbold" valign="top">
                                    <asp:DropDownList ID="ddExistingMaterialAction" runat="server">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvExistingMaterialAction" ControlToValidate="ddExistingMaterialAction"
                                        SetFocusOnError="true" ErrorMessage="Existing material action is required" Text="<"
                                        ValidationGroup="vgSave" />
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    <asp:Label runat="server" ID="lblInternalRequirement" Text="Internal Requirement:"
                                        Visible="false"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtInternalRequirement" runat="server" TextMode="MultiLine" Width="250px"
                                        Visible="false"></asp:TextBox>
                                    <br />
                                    <asp:Label ID="lblInternalRequirementCharCount" SkinID="MessageLabelSkin" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    <asp:Label ID="lblPurchasingComment" runat="server" Text="Purchasing Comment:" Visible="false"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtPurchasingComment" runat="server" TextMode="MultiLine" Width="250px"
                                        Visible="false"></asp:TextBox>
                                    <br />
                                    <asp:Label ID="lblPurchasingCharCount" SkinID="MessageLabelSkin" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <table width="98%" border="1" cellpadding="1" cellspacing="1" style="border-color: Navy">
                            <tr style="background-color: Aqua">
                                <td class="p_bigtextbold" align="center">
                                    CURRENT
                                </td>
                                <td class="p_bigtextbold" align="center">
                                    NEW
                                </td>
                            </tr>
                            <tr>
                                <td style="background-color: #DEDEDE" valign="top">
                                    <table>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblCurrentCustomerPartNo" Text="Customer Part No:" />
                                            </td>
                                            <td class="c_textbold">
                                                <asp:TextBox runat="server" ID="txtCurrentCustomerPartNo" MaxLength="40" Width="200px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblCurrentDesignLevel" Text="Design Level:" />
                                            </td>
                                            <td class="c_textbold">
                                                <asp:TextBox runat="server" ID="txtCurrentDesignLevel" MaxLength="30" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblCurrentCustomerDrawingNo" Text="Customer Drawing No.:" />
                                            </td>
                                            <td class="c_textbold">
                                                <asp:TextBox runat="server" ID="txtCurrentCustomerDrawingNo" MaxLength="30" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                &nbsp;
                                                <asp:HyperLink runat="server" ID="hlnkCurrentCustomerImage" Visible="false" Font-Underline="true"
                                                    Target="_blank">View CAD Drawing Image</asp:HyperLink>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                DMS Drawing No.:
                                            </td>
                                            <td class="c_textbold">
                                                <asp:TextBox runat="server" ID="txtCurrentDrawingNo" MaxLength="18" />
                                                &nbsp;
                                                <asp:ImageButton ID="iBtnCurrentDrawingSearch" runat="server" ImageUrl="~/images/Search.gif"
                                                    ToolTip="Click here to search for a DMS Drawing." />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                &nbsp;
                                                <asp:HyperLink runat="server" ID="hlnkCurrentDrawingNo" Visible="false" Font-Underline="true"
                                                    ToolTip="Click here to view the current DMS Drawing." Text="View DMS Drawing Image"
                                                    Target="_blank" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                Interal Part No:
                                            </td>
                                            <td class="c_textbold" style="white-space: nowrap;">
                                                <asp:TextBox ID="txtCurrentPartNo" runat="server" MaxLength="40" Width="200px" />
                                                &nbsp;
                                                <asp:ImageButton ID="iBtnCurrentPartNoSearch" runat="server" ImageUrl="~/images/Search.gif"
                                                    ToolTip="Click here to search for an Internal Part No." />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                Part Name:
                                            </td>
                                            <td class="c_textbold">
                                                <asp:Label ID="lblCurrentPartName" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblNewCustomerPartNo" Text="Customer Part No:" />
                                            </td>
                                            <td class="c_textbold">
                                                <asp:TextBox runat="server" ID="txtNewCustomerPartNo" MaxLength="40" Width="200px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblNewDesignLevel" Text="Design Level:" />
                                            </td>
                                            <td class="c_textbold">
                                                <asp:TextBox runat="server" ID="txtNewDesignLevel" MaxLength="30" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblNewCustomerDrawingNo" Text="Customer Drawing No:" />
                                            </td>
                                            <td class="c_textbold">
                                                <asp:TextBox runat="server" ID="txtNewCustomerDrawingNo" MaxLength="30" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                &nbsp;
                                                <asp:HyperLink runat="server" ID="hlnkNewCustomerImage" Visible="false" Font-Underline="true"
                                                    Target="_blank" Text="View CAD Drawing Image" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                DMS Drawing No:
                                            </td>
                                            <td class="c_textbold">
                                                <asp:TextBox runat="server" ID="txtNewDrawingNo" MaxLength="18" />
                                                &nbsp;
                                                <asp:ImageButton ID="iBtnNewDrawingSearch" runat="server" ImageUrl="~/images/Search.gif"
                                                    ToolTip="Click here to search for a DMS Drawing." />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                &nbsp;
                                                <asp:HyperLink runat="server" ID="hlnkNewDrawingNo" Visible="false" Font-Underline="true"
                                                    ToolTip="Click here to view the new DMS Drawing." Text="View DMS Drawing Image"
                                                    Target="_blank" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label ID="lblNewPartNoMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                                    Text="* " />
                                                Internal Part No:
                                            </td>
                                            <td class="c_textbold" style="white-space: nowrap;">
                                                <asp:TextBox runat="server" ID="txtNewPartNo" MaxLength="40" Width="200px" />
                                                &nbsp;
                                                <asp:RequiredFieldValidator runat="server" ID="rfNewPartNo" ControlToValidate="txtNewPartNo"
                                                    SetFocusOnError="true" ErrorMessage="New Part number is required" Text="<" ValidationGroup="vgSave" />
                                                &nbsp;
                                                <asp:ImageButton ID="iBtnNewPartNoSearch" runat="server" ImageUrl="~/images/Search.gif"
                                                    ToolTip="Click here to search for a Internal Part No. (if exists)" Visible="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label ID="lblNewPartNameMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                                    Text="* " />
                                                Part Name:
                                            </td>
                                            <td class="c_textbold">
                                                <asp:TextBox runat="server" ID="txtNewPartName" MaxLength="240" Width="250px" />
                                                <asp:RequiredFieldValidator runat="server" ID="rfvNewPartName" ControlToValidate="txtNewPartName"
                                                    SetFocusOnError="true" ErrorMessage="New Part name is required" Text="<" ValidationGroup="vgSave" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label runat="server" ID="lblMessageBPCS" SkinID="MessageLabelSkin" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:Button ID="btnSaveHeader" runat="server" Text="Save" Visible="false" ValidationGroup="vgSave" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <a id="alnkCustomerBreakdown" runat="server" visible="false" href="http://www.ugnnet.com/Purchasing%20%20Packaging%20Document%20Library/Customer%20-%20Part%20Breakdown%20Reference.xls"
                                                    target="_blank">View Rules to convert Customer PartNo to Finished Good PartNo</a>
                                                <br />
                                                <a id="alnkQa166" runat="server" visible="false" href="http://taps.ugnnet.com/docushare/dsweb/Get/Document-2252/Qa166%20-%20Part%20Numbering%20for%20Finished%20Goods%20.doc"
                                                    target="_blank">View Docushare QA166 SOP</a>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="c_text" valign="top" colspan="2">
                                    Part Bill of Material of Current Part:
                                    <asp:ImageButton ID="iBtnPartBOMView" runat="server" ImageUrl="~/images/SelectUser.gif"
                                        ToolTip="Click here to pull Bill Of Materials." />
                                    <br />
                                    <asp:Label runat="server" ID="lblMessagePartBOM" Text="The bill of materials always reflects the current data and, therefore, is subject to change over time."></asp:Label>
                                    <br />
                                    <asp:TreeView ID="tvCurrentPartBOM" runat="server">
                                    </asp:TreeView>
                                </td>
                            </tr>
                            <tr>
                                <td class="c_text" valign="top" colspan="2">
                                    Parent Parts Affected if Current Part is changed:
                                    <asp:ImageButton ID="iBtnParentPartCopy" runat="server" ImageUrl="~/images/SelectUser.gif"
                                        ToolTip="Click here to find any parts that directly use this existing part as a child part." />
                                    <br />
                                    <asp:GridView ID="gvParentPart" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                                        DataKeyNames="RowID" AllowPaging="True" PageSize="15" ShowFooter="True" DataSourceID="odsParentPart"
                                        EmptyDataText="No parent parts found." Width="800px">
                                        <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                        <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                        <EditRowStyle BackColor="#CCCCCC" />
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="ParentPartNo" HeaderText="Parent Part Numbers Affected"
                                                SortExpression="PartNo" />
                                            <asp:BoundField DataField="ChildPartNo" HeaderText="Child Part Being Changed" SortExpression="PartNo" />
                                        </Columns>
                                    </asp:GridView>
                                    <asp:ObjectDataSource ID="odsParentPart" runat="server" SelectMethod="GetECIBPCSParentPartsAffected"
                                        TypeName="ECIBPCSParentPartsAffectedBLL">
                                        <SelectParameters>
                                            <asp:ControlParameter Name="ECINo" Type="Int32" ControlID="lblECINo" DefaultValue="0" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                </td>
                            </tr>
                        </table>
                    </Content>
                </ajax:AccordionPane>
            </Panes>
        </ajax:Accordion>
        <ajax:Accordion ID="accFacilityDept" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
            FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
            RequireOpenedPane="false" SuppressHeaderPostbacks="true" Visible="false">
            <Panes>
                <ajax:AccordionPane ID="apFacilityDept" runat="server">
                    <Header>
                        <a href="">UGN Facility and Department</a></Header>
                    <Content>
                        <asp:ValidationSummary runat="server" ID="vsInsertFacilityDepartment" ValidationGroup="vgInsertFacilityDept"
                            ShowMessageBox="true" ShowSummary="true" />
                        <asp:Label ID="lblMessageFacilityDepartment" runat="server" SkinID="MessageLabelSkin"></asp:Label>
                        <asp:GridView ID="gvFacilityDept" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                            AllowPaging="True" PageSize="15" DataKeyNames="RowID" ShowFooter="False" DataSourceID="odsFacilityDept"
                            EmptyDataText="No records found" Width="98%">
                            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#CCCCCC" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="RowID" HeaderText="RowID" SortExpression="RowID" />
                                <asp:BoundField DataField="ECINo" HeaderText="ECINo" SortExpression="ECINo" />
                                <asp:TemplateField HeaderText="UGN Facility" SortExpression="UGNFacilityName">
                                    <EditItemTemplate>
                                        <asp:Label ID="lblEditUGNFacilityName" runat="server" Text='<%# Bind("ddUGNFacilityName") %>'></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblViewUGNFacilityName" runat="server" Text='<%# Bind("ddUGNFacilityName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblInsertFacilityMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                            Text="* " />
                                        <asp:DropDownList ID="ddInsertFacility" runat="server" DataSource='<%# commonFunctions.GetUGNFacility("") %>'
                                            DataValueField="UGNFacility" DataTextField="UGNFacilityName" AppendDataBoundItems="True">
                                            <asp:ListItem Text="" Value="0" Selected="False"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvInsertFacility" runat="server" ControlToValidate="ddInsertFacility"
                                            ErrorMessage="UGN Facility is required." Font-Bold="True" ValidationGroup="vgInsertFacility"
                                            Text="<" SetFocusOnError="true">				                                                            
                                        </asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Department" SortExpression="DepartmentName">
                                    <EditItemTemplate>
                                        <asp:Label ID="lblEditDepartmentName" runat="server" Text='<%# Bind("ddDepartmentName") %>'></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblViewDepartmentName" runat="server" Text='<%# Bind("ddDepartmentName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddInsertDepartment" runat="server" DataSource='<%# commonFunctions.GetDepartment("","",False) %>'
                                            DataValueField="DepartmentID" DataTextField="ddDepartmentName" AppendDataBoundItems="True">
                                            <asp:ListItem Text="" Value="0" Selected="False"></asp:ListItem>
                                        </asp:DropDownList>
                                    </FooterTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="iBtnFacilityDeptDelete" runat="server" CausesValidation="False"
                                            CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgInsertFacilityDept"
                                            runat="server" ID="iBtnFacilityDeptSave" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                                        <asp:ImageButton ID="iBtnFacilityDeptUndo" runat="server" CommandName="Undo" CausesValidation="false"
                                            ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsFacilityDept" runat="server" OldValuesParameterFormatString="original_{0}"
                            SelectMethod="GetECIFacilityDept" TypeName="ECIFacilityDeptBLL" DeleteMethod="DeleteECIFacilityDept"
                            InsertMethod="InsertECIFacilityDept">
                            <SelectParameters>
                                <asp:ControlParameter Name="ECINo" Type="Int32" ControlID="lblECINo" DefaultValue="0" />
                            </SelectParameters>
                            <DeleteParameters>
                                <asp:Parameter Name="RowID" Type="Int32" />
                                <asp:Parameter Name="original_RowID" Type="Int32" />
                            </DeleteParameters>
                            <InsertParameters>
                                <asp:Parameter Name="ECINo" Type="Int32" />
                                <asp:Parameter Name="UGNFacility" Type="String" />
                                <asp:Parameter Name="DepartmentID" Type="Int32" />
                            </InsertParameters>
                        </asp:ObjectDataSource>
                    </Content>
                </ajax:AccordionPane>
            </Panes>
        </ajax:Accordion>
        <ajax:Accordion ID="accCustomerProgram" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
            FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
            RequireOpenedPane="false" SuppressHeaderPostbacks="true" Visible="false">
            <Panes>
                <ajax:AccordionPane ID="apCustomerProgram" runat="server">
                    <Header>
                        <a href="">Program / Customer </a>
                    </Header>
                    <Content>
                        <asp:ValidationSummary runat="server" ID="vsSaveInternal" ValidationGroup="vgSaveInternal"
                            ShowMessageBox="true" ShowSummary="true" />
                        <asp:Label ID="lblMessageSupplementalPartInformation" runat="server" SkinID="MessageLabelSkin"></asp:Label>
                        <br />
                        <table width="98%">
                            <tr>
                                <td class="p_text" valign="top">
                                    <asp:Label ID="lblCommodityMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                        Text="* " Visible="false" />
                                    <asp:Label runat="server" ID="lblCommodity" Text="Commodity:" Visible="false"></asp:Label>
                                </td>
                                <td class="c_textbold" visible="false" valign="top">
                                    <asp:DropDownList ID="ddCommodity" runat="server" Visible="false">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvCommodity" ControlToValidate="ddCommodity"
                                        SetFocusOnError="true" ErrorMessage="Commodity is required" Text="<" ValidationGroup="vgSaveInternal" />
                                    <br />
                                    <asp:Label runat="server" ID="lblCommodityNote" Text="{Commodity / Classification}"
                                        Visible="false"></asp:Label>
                                </td>
                                <td class="p_text">
                                    <asp:Label ID="lblProductTechnologyMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                        Text="* " Visible="false" />
                                    <asp:Label runat="server" ID="lblProductTechnology" Text="Product Technology:" Visible="false"></asp:Label>
                                </td>
                                <td class="c_textbold">
                                    <asp:DropDownList ID="ddProductTechnology" runat="server" Visible="false">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvProductTechnology" ControlToValidate="ddProductTechnology"
                                        SetFocusOnError="true" ErrorMessage="Product technology is required" Text="<"
                                        ValidationGroup="vgSaveInternal" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4">
                                    <asp:Button ID="btnSaveSupplementalPartInformation" runat="server" Text="Save" ValidationGroup="vgSaveInternal"
                                        Visible="false" />
                                </td>
                            </tr>
                        </table>
                        <hr />
                        <table>
                            <tr>
                                <td valign="top">
                                    <table runat="server" id="tblMakes" visible="false">
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblMake" Text="Make:" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddMakes" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblModel" Text="Model:" />
                                            </td>
                                            <td style="font-size: smaller">
                                                <asp:DropDownList ID="ddModel" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label ID="lblProgramMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                                    Text="* " />
                                                <asp:Label runat="server" ID="lblProgram" Text="Program:" />
                                            </td>
                                            <td colspan="3" style="white-space: nowrap">
                                                <asp:DropDownList ID="ddProgram" runat="server" />
                                                <asp:RequiredFieldValidator ID="rfvProgram" runat="server" ControlToValidate="ddProgram"
                                                    ErrorMessage="Program is required." Font-Bold="True" ValidationGroup="vgCustomerProgram"
                                                    Text="<" SetFocusOnError="true" />
                                                <asp:ImageButton ID="iBtnPreviewDetail" runat="server" ImageUrl="~/images/PreviewUp.jpg"
                                                    ToolTip="Review Program Detail" Visible="false" />
                                                <br />
                                                <asp:Label runat="server" Font-Size="Smaller" Text="{Program / Platform / Assembly Plant}" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top">
                                    <table>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblSOPDate" Text="Program SOP Date:" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSOPDate" runat="server" MaxLength="10" Width="75px" Visible="false"></asp:TextBox>
                                                <asp:ImageButton runat="server" ID="imgSOPDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                                    AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                                                    Visible="false" />
                                                <ajax:CalendarExtender ID="ceSOPDate" runat="server" TargetControlID="txtSOPDate"
                                                    PopupButtonID="imgSOPDate" Format="MM/dd/yyyy" />
                                                <asp:RegularExpressionValidator ID="revSOPDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                                    ControlToValidate="txtSOPDate" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                                    Width="8px" ValidationGroup="vgCustomerProgram"><</asp:RegularExpressionValidator>
                                                <asp:CompareValidator ID="cvSOP" runat="server" ErrorMessage="Program SOP Date must be less than Program EOP Date."
                                                    ControlToCompare="txtEOPDate" ControlToValidate="txtSOPDate" Operator="LessThan"
                                                    Type="Date" ValidationGroup="vgCustomerProgram"><</asp:CompareValidator>
                                            </td>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblCustomerApprovalRequired" Text="Customer Approval Required"
                                                    Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="cbCustomerApprovalRequired" runat="server" Visible="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblEOPDate" Text="Program EOP Date:" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEOPDate" runat="server" MaxLength="10" Width="75px" Visible="false"></asp:TextBox>
                                                <asp:ImageButton runat="server" ID="imgEOPDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                                    AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                                                    Visible="false" />
                                                <ajax:CalendarExtender ID="ceEOPDate" runat="server" TargetControlID="txtEOPDate"
                                                    PopupButtonID="imgEOPDate" Format="MM/dd/yyyy" />
                                                <asp:RegularExpressionValidator ID="revEOPDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                                    ControlToValidate="txtEOPDate" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                                    Width="8px" ValidationGroup="vgCustomerProgram"><</asp:RegularExpressionValidator>
                                                <asp:CompareValidator ID="cvEOP" runat="server" ControlToCompare="txtSOPDate" ControlToValidate="txtEOPDate"
                                                    ErrorMessage="Program EOP Date must be greater than Program SOP Date." Operator="GreaterThan"
                                                    Type="Date" ValidationGroup="vgCustomerProgram"><</asp:CompareValidator>
                                            </td>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblCustomerApprovalDate" Text="Customer Approval Date"
                                                    Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCustomerApprovalDate" runat="server" MaxLength="10" Width="70px"
                                                    Visible="false"></asp:TextBox>
                                                <asp:ImageButton runat="server" ID="imgCustomerApprovalDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                                    AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                                                    Visible="false" />
                                                <ajax:CalendarExtender ID="ceCustomerApprovalDate" runat="server" TargetControlID="txtCustomerApprovalDate"
                                                    PopupButtonID="imgCustomerApprovalDate" Format="MM/dd/yyyy" />
                                                <asp:RegularExpressionValidator ID="revCustomerApprovalDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                                    ControlToValidate="txtCustomerApprovalDate" Font-Bold="True" ToolTip="MM/DD/YYYY"
                                                    ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                                    Width="8px" ValidationGroup="vgCustomerProgram"><</asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label ID="lblYearMarker" runat="server" Font-Bold="True" ForeColor="Red" Text="* "
                                                    Visible="false" />
                                                <asp:Label runat="server" ID="lblYear" Text="Year:" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddYear" runat="server" Visible="false">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvYear" runat="server" ControlToValidate="ddYear"
                                                    ErrorMessage="Year is required." Font-Bold="True" ValidationGroup="vgCustomerProgram"
                                                    Text="<" SetFocusOnError="true" />
                                            </td>
                                            <td class="p_text">
                                                <asp:Label runat="server" ID="lblCustomerApprovalNo" Text="Customer Approval No."
                                                    Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCustomerApprovalNo" runat="server" MaxLength="20" Width="100px"
                                                    Visible="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Button runat="Server" ID="btnAddToCustomerProgram" Text="Add/Update Program / Customer"
                                        ValidationGroup="vgCustomerProgram" Visible="false" />
                                    <asp:Button runat="Server" ID="btnCancelEditCustomerProgram" Text="Cancel Edit Program / Customer"
                                        CausesValidation="false" Visible="false" />
                                </td>
                            </tr>
                        </table>
                        <asp:ValidationSummary ID="vsCustomerProgram" runat="server" DisplayMode="List" ShowMessageBox="true"
                            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgCustomerProgram" />
                        <asp:Label runat="server" ID="lblMessageCustomerProgram" SkinID="MessageLabelSkin"></asp:Label>
                        <ajax:CascadingDropDown ID="cddMakes" runat="server" TargetControlID="ddMakes" Category="Make"
                            PromptText="Please select a Make." LoadingText="[Loading Makes...]" ServicePath="~/WS/VehicleCDDService.asmx"
                            ServiceMethod="GetMakes" />
                        <ajax:CascadingDropDown ID="cddModel" runat="server" TargetControlID="ddModel" ParentControlID="ddMakes"
                            Category="Model" PromptText="Please select a Model." LoadingText="[Loading Models...]"
                            ServicePath="~/WS/VehicleCDDService.asmx" ServiceMethod="GetModelMaint" />
                        <ajax:CascadingDropDown ID="cddProgram" runat="server" TargetControlID="ddProgram"
                            ParentControlID="ddModel" Category="Program" PromptText="Please select a Program."
                            LoadingText="[Loading Programs...]" ServicePath="~/WS/VehicleCDDService.asmx"
                            ServiceMethod="GetProgramsPlatformAssembly" />
                        <br />
                        <asp:GridView ID="gvCustomerProgram" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                            AllowPaging="True" PageSize="15" DataKeyNames="RowID" DataSourceID="odsCustomerProgram"
                            EmptyDataText="No Programs or Customers found" width="98%" ShowFooter="false">
                            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#CCCCCC" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="RowID" HeaderText="RowID" SortExpression="RowID" />
                                <asp:BoundField DataField="ECINo" HeaderText="ECINo" SortExpression="ECINo" />
                                <asp:BoundField DataField="ddCustomerDesc" HeaderText="Customer" SortExpression="ddCustomerDesc"
                                    ReadOnly="True" />
                                <asp:BoundField DataField="ProgramID" SortExpression="ProgramID" ReadOnly="True" />
                                <asp:BoundField DataField="ddProgramName" HeaderText="Program / Make / Model / Platform / Assembly Plant"
                                    SortExpression="ddProgramName" ReadOnly="True" />
                                <asp:BoundField DataField="ProgramYear" HeaderText="Year" SortExpression="ProgramYear"
                                    ReadOnly="True" />
                                <asp:BoundField DataField="SOPDate" HeaderText="SOPDate" SortExpression="SOPDate"
                                    ReadOnly="True" />
                                <asp:BoundField DataField="EOPDate" HeaderText="EOPDate" SortExpression="EOPDate"
                                    ReadOnly="True" />
                                <asp:CheckBoxField DataField="isCustomerApprovalRequired" HeaderText="Cust. Appr. Req."
                                    ReadOnly="True" SortExpression="isCustomerApprovalRequired" />
                                <asp:BoundField DataField="CustomerApprovalDate" HeaderText="Appr. Date" SortExpression="CustomerApprovalDate"
                                    ReadOnly="True" />
                                <asp:BoundField DataField="CustomerApprovalNo" HeaderText="Appr. No." SortExpression="CustomerApprovalNo"
                                    ReadOnly="True" />
                                <asp:TemplateField ShowHeader="False">
                                    <EditItemTemplate>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="iBtnCustomerProgramEdit" runat="server" CausesValidation="False"
                                            CommandName="Select" ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                                        <asp:ImageButton ID="iBtnCustomerProgramDelete" runat="server" CausesValidation="False"
                                            CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsCustomerProgram" runat="server" OldValuesParameterFormatString="original_{0}"
                            SelectMethod="GetECICustomerProgram" TypeName="ECICustomerProgramBLL" DeleteMethod="DeleteECICustomerProgram">
                            <SelectParameters>
                                <asp:ControlParameter Name="ECINo" Type="Int32" ControlID="lblECINo" DefaultValue="0" />
                            </SelectParameters>
                            <DeleteParameters>
                                <asp:Parameter Name="RowID" Type="Int32" />
                                <asp:Parameter Name="original_RowID" Type="Int32" />
                            </DeleteParameters>
                        </asp:ObjectDataSource>
                    </Content>
                </ajax:AccordionPane>
            </Panes>
        </ajax:Accordion>
        <ajax:Accordion ID="accVendor" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
            FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
            RequireOpenedPane="false" SuppressHeaderPostbacks="true" Visible="false">
            <Panes>
                <ajax:AccordionPane ID="apVendor" runat="server">
                    <Header>
                        <a href="">Material and Vendor</a></Header>
                    <Content>
                        <asp:Label ID="lblMessageMaterialVendor" runat="server" SkinID="MessageLabelSkin"></asp:Label>
                        <asp:ValidationSummary runat="server" ID="vsSaveExternal" ValidationGroup="vgSaveExternal"
                            ShowMessageBox="true" ShowSummary="true" />
                        <table >
                            <tr>
                                <td class="p_text">
                                    Family:
                                </td>
                                <td class="c_textbold">
                                    <asp:DropDownList ID="ddFamily" runat="server" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td class="p_text">
                                    <asp:Label ID="lblSubFamilyMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                        Text="* " />
                                    Sub-Family:
                                </td>
                                <td class="c_textbold">
                                    <asp:DropDownList ID="ddSubFamily" runat="server">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvSubFamily" ControlToValidate="ddSubFamily"
                                        SetFocusOnError="true" ErrorMessage="Sub-Family is required" Text="<" ValidationGroup="vgSaveExternal" />
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    &nbsp;
                                    <!-- PPAP Required: -->
                                </td>
                                <td class="c_textbold">
                                    <asp:RadioButtonList ID="rbPPAP" runat="server" AutoPostBack="true">
                                        <asp:ListItem Text="PPAP Required (See CARS Application)" Value="1" />
                                        <asp:ListItem Text="N/A" Value="0" />
                                    </asp:RadioButtonList>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvPPAP" ControlToValidate="rbPPAP"
                                        SetFocusOnError="true" ErrorMessage="Select whether or not PPAP is required"
                                        Text="<" ValidationGroup="vgSaveExternal" />
                                </td>
                                <td class="p_text">
                                    Purchased Good:
                                </td>
                                <td class="c_textbold">
                                    <asp:DropDownList ID="ddPurchasedGood" runat="server" />
                                </td>
                                <td class="p_text">
                                    <asp:Label ID="lblPPAPLevelMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                        Text="* " />
                                    <asp:Label runat="server" ID="lblPPAPLevel" Text="PPAP Level:"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddPPAPLevel" runat="server">
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvPPAPLevel" ControlToValidate="ddPPAPLevel"
                                        Enabled="false" SetFocusOnError="true" ErrorMessage="PPAP Level is required"
                                        Text="<" ValidationGroup="vgSaveExternal" />
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td class="p_text">
                                    Vendor Requirement:
                                </td>
                                <td colspan="3" class="c_textbold">
                                    <asp:TextBox ID="txtVendorRequirement" runat="server" TextMode="MultiLine" Height="90px"
                                        Width="500px"></asp:TextBox>
                                    <br />
                                    <asp:Label ID="lblVendorRequirementCharCount" SkinID="MessageLabelSkin" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center">
                                    <asp:Button ID="btnSaveMaterialVendor" runat="server" Text="Save" ValidationGroup="vgSaveExternal" />
                                </td>
                            </tr>
                        </table>
                        <asp:ValidationSummary runat="server" ID="vsEditVendor" ValidationGroup="vgEditVendor"
                            ShowMessageBox="true" ShowSummary="true" />
                        <asp:ValidationSummary runat="server" ID="vsInsertVendor" ValidationGroup="vgInsertVendor"
                            ShowMessageBox="true" ShowSummary="true" />
                        <br />
                        <asp:GridView ID="gvVendor" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                            AllowPaging="True" PageSize="15" DataKeyNames="RowID" DataSourceID="odsVendor"
                            EmptyDataText="No records found" Width="98%">
                            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#CCCCCC" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="RowID" HeaderText="RowID" ReadOnly="True" />
                                <asp:TemplateField HeaderText="Vendor" SortExpression="ddUGNDBVendorName">
                                    <EditItemTemplate>
                                        <asp:Label ID="lblEditVendorMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                            Text="* " />
                                        <asp:DropDownList ID="ddEditVendor" runat="server" DataSource='<%# CommonFunctions.GetUGNDBVendor(0,"","",0) %>'
                                            DataValueField="UGNDBVendorID" DataTextField="ddVendorName" AppendDataBoundItems="True"
                                            SelectedValue='<%# Bind("UGNDBVendorID") %>' Enabled="false">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblViewVendor" runat="server" Text='<%# Bind("ddVendorName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblInsertVendorMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                            Text="* " />
                                        <asp:DropDownList ID="ddInsertVendor" runat="server" DataSource='<%# CommonFunctions.GetUGNDBVendor(0,"","",1) %>'
                                            DataValueField="UGNDBVendorID" DataTextField="ddVendorName" AppendDataBoundItems="True">
                                            <asp:ListItem Text="" Value="0" Selected="False"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvInsertVendor" runat="server" ControlToValidate="ddInsertVendor"
                                            ErrorMessage="Vendor is required." Font-Bold="True" ValidationGroup="vgInsertVendor"
                                            Text="<" SetFocusOnError="true">				                                                            
                                        </asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PPAP Due Date" SortExpression="PPAPDueDate">
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" ID="txtEditPPAPDueDate" Text='<%# Bind("PPAPDueDate") %>'
                                            MaxLength="10"></asp:TextBox>
                                        <asp:ImageButton runat="server" ID="imgEditPPAPDueDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                            AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                                        <ajax:CalendarExtender ID="ceEditPPAPDueDate" runat="server" TargetControlID="txtEditPPAPDueDate"
                                            PopupButtonID="imgEditPPAPDueDate" />
                                        <asp:RegularExpressionValidator ID="revEditPPAPDueDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                            ControlToValidate="txtEditPPAPDueDate" Font-Bold="True" ToolTip="MM/DD/YYYY"
                                            ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                            Width="8px" ValidationGroup="vgEditVendor"><</asp:RegularExpressionValidator>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblViewPPAPDueDate" runat="server" Text='<%# Bind("PPAPDueDate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox runat="server" ID="txtInsertPPAPDueDate" Text='<%# Bind("PPAPDueDate") %>'
                                            MaxLength="10"></asp:TextBox>
                                        <asp:ImageButton runat="server" ID="imgInsertPPAPDueDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                            AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                                        <ajax:CalendarExtender ID="ceInsertPPAPDueDate" runat="server" TargetControlID="txtInsertPPAPDueDate"
                                            PopupButtonID="imgInsertPPAPDueDate" />
                                        <asp:RegularExpressionValidator ID="revInsertPPAPDueDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                            ControlToValidate="txtInsertPPAPDueDate" Font-Bold="True" ToolTip="MM/DD/YYYY"
                                            ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                            Width="8px" ValidationGroup="vgEditVendor"><</asp:RegularExpressionValidator>
                                    </FooterTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PPAP Completion Date" SortExpression="PPAPCompletionDate">
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" ID="txtEditPPAPCompletionDate" Text='<%# Bind("PPAPCompletionDate") %>'
                                            MaxLength="10"></asp:TextBox>
                                        <asp:ImageButton runat="server" ID="imgEditPPAPCompletionDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                            AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                                        <ajax:CalendarExtender ID="ceEditPPAPCompletionDate" runat="server" TargetControlID="txtEditPPAPCompletionDate"
                                            PopupButtonID="imgEditPPAPCompletionDate" />
                                        <asp:RegularExpressionValidator ID="revEditPPAPCompletionDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                            ControlToValidate="txtEditPPAPCompletionDate" Font-Bold="True" ToolTip="MM/DD/YYYY"
                                            ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                            Width="8px" ValidationGroup="vgEditVendor"><</asp:RegularExpressionValidator>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblViewPPAPCompletionDate" runat="server" Text='<%# Bind("PPAPCompletionDate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox runat="server" ID="txtInsertPPAPCompletionDate" Text='<%# Bind("PPAPCompletionDate") %>'
                                            MaxLength="10"></asp:TextBox>
                                        <asp:ImageButton runat="server" ID="imgInsertPPAPCompletionDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                            AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                                        <ajax:CalendarExtender ID="ceInsertPPAPCompletionDate" runat="server" TargetControlID="txtInsertPPAPCompletionDate"
                                            PopupButtonID="imgInsertPPAPCompletionDate" />
                                        <asp:RegularExpressionValidator ID="revInsertPPAPCompletionDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                            ControlToValidate="txtInsertPPAPCompletionDate" Font-Bold="True" ToolTip="MM/DD/YYYY"
                                            ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                            Width="8px" ValidationGroup="vgEditVendor"><</asp:RegularExpressionValidator>
                                    </FooterTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <EditItemTemplate>
                                        <asp:ImageButton ID="iBtnVendorUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                            ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditVendor" />
                                        <asp:ImageButton ID="iBtnVendorCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                            ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="iBtnVendorEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                            ImageUrl="~/images/edit.jpg" AlternateText="Edit" ValidationGroup="vgEditVendor" />
                                        <asp:ImageButton ID="iBtnVendorDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                            ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgInsertVendor"
                                            runat="server" ID="iBtnVendorSave" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                                        <asp:ImageButton ID="iBtnVendorUndo" runat="server" CommandName="Undo" CausesValidation="false"
                                            ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsVendor" runat="server" OldValuesParameterFormatString="original_{0}"
                            SelectMethod="GetECIVendor" TypeName="ECIVendorBLL" DeleteMethod="DeleteECIVendor"
                            InsertMethod="InsertECIVendor" UpdateMethod="UpdateECIVendor">
                            <SelectParameters>
                                <asp:ControlParameter Name="ECINo" Type="Int32" ControlID="lblECINo" DefaultValue="0" />
                            </SelectParameters>
                            <DeleteParameters>
                                <asp:Parameter Name="RowID" Type="Int32" />
                                <asp:Parameter Name="original_RowID" Type="Int32" />
                            </DeleteParameters>
                            <InsertParameters>
                                <asp:Parameter Name="ECINo" Type="Int32" />
                                <asp:Parameter Name="UGNDBVendorID" Type="Int32" />
                                <asp:Parameter Name="PPAPDueDate" Type="String" />
                                <asp:Parameter Name="PPAPCompletionDate" Type="String" />
                                <asp:Parameter Name="VendorSignedDate" Type="String" />
                            </InsertParameters>
                            <UpdateParameters>
                                <asp:Parameter Name="original_RowID" Type="Int32" />
                                <asp:Parameter Name="UGNDBVendorID" Type="Int32" />
                                <asp:Parameter Name="PPAPDueDate" Type="String" />
                                <asp:Parameter Name="PPAPCompletionDate" Type="String" />
                                <asp:Parameter Name="VendorSignedDate" Type="String" />
                            </UpdateParameters>
                        </asp:ObjectDataSource>
                    </Content>
                </ajax:AccordionPane>
            </Panes>
        </ajax:Accordion>
        <ajax:Accordion ID="accKit" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
            FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
            RequireOpenedPane="false" SuppressHeaderPostbacks="true" Visible="false">
            <Panes>
                <ajax:AccordionPane ID="apKit" runat="server">
                    <Header>
                        <a href="">Kit (if exists)</a></Header>
                    <Content>
                        <asp:ValidationSummary runat="server" ID="vsEditKit" ValidationGroup="vgEditKit"
                            ShowMessageBox="true" ShowSummary="true" />
                        <asp:ValidationSummary runat="server" ID="vsInsertKit" ValidationGroup="vgInsertKit"
                            ShowMessageBox="true" ShowSummary="true" />
                        <asp:Label runat="server" ID="lblMessageKIT"></asp:Label>
                        <br />
                        <asp:GridView ID="gvKit" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                            AllowPaging="True" PageSize="15" DataKeyNames="RowID" ShowFooter="False" DataSourceID="odsKit"
                            EmptyDataText="No records found" Width="98%">
                            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#CCCCCC" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="RowID">
                                    <ItemStyle CssClass="none" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ECINo">
                                    <ItemStyle CssClass="none" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Kit Part No" SortExpression="ddKitPartNo">
                                    <EditItemTemplate>
                                        <asp:Label ID="lblEditKitPartNo" runat="server" Text='<%# Bind("ddKitPartNo") %>'></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblViewKitPartNo" runat="server" Text='<%# Bind("ddKitPartNo") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtInsertKitPartNo" runat="server" MaxLength="15" Width="140px"></asp:TextBox>
                                        <asp:ImageButton ID="iBtnInsertSearchKitPartNo" runat="server" CommandName="Insert"
                                            CausesValidation="False" ImageUrl="~/images/Search.gif" ToolTip="Fill in Part No if known"
                                            AlternateText="Search Kit PartNo" ValidationGroup="vgInsertKit" />
                                        <asp:RequiredFieldValidator ID="rfvInsertKitPartNo" runat="server" ControlToValidate="txtInsertKitPartNo"
                                            ErrorMessage="Kit PartNo is required." Font-Bold="True" ValidationGroup="vgInsertKit"
                                            Text="<" SetFocusOnError="true">				                                                            
                                        </asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rev" SortExpression="KitPartRevision">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="lblEditKitPartRevision" runat="server" Text='<%# Bind("KitPartRevision") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblViewKitPartRevision" runat="server" Text='<%# Bind("KitPartRevision") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtInsertKitPartRevision" runat="server" MaxLength="2" Width="20px"></asp:TextBox>
                                    </FooterTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                    <FooterStyle HorizontalAlign="Left" Wrap="false" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="KitPartName"></asp:BoundField>
                                <asp:TemplateField HeaderText="Finished Good<br/> Part No" SortExpression="ddFinishedGoodPartNo">
                                    <EditItemTemplate>
                                        <asp:Label ID="lblEditFinishedGoodPartNo" runat="server" Text='<%# Bind("ddFinishedGoodPartNo") %>'></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblViewFinishedGoodPartNo" runat="server" Text='<%# Bind("ddFinishedGoodPartNo") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtInsertFinishedGoodPartNo" runat="server" MaxLength="15" Width="140px"></asp:TextBox>
                                        <asp:ImageButton ID="iBtnInsertSearchFinishedGoodPartNo" runat="server" CommandName="Insert"
                                            CausesValidation="False" ImageUrl="~/images/Search.gif" ToolTip="Fill in Part No if known"
                                            AlternateText="Search FinishedGood PartNo" ValidationGroup="vgInsertFinishedGood" />
                                        <asp:RequiredFieldValidator ID="rfvInsertFinishedGoodPartNo" runat="server" ControlToValidate="txtInsertFinishedGoodPartNo"
                                            ErrorMessage="FinishedGood PartNo is required." Font-Bold="True" ValidationGroup="vgInsertFinishedGood"
                                            Text="<" SetFocusOnError="true">				                                                            
                                        </asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                    <FooterStyle HorizontalAlign="Left" Wrap="false" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rev" SortExpression="FinishedGoodPartRevision">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="lblEditFinishedGoodPartRevision" runat="server" Text='<%# Bind("FinishedGoodPartRevision") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblViewFinishedGoodPartRevision" runat="server" Text='<%# Bind("FinishedGoodPartRevision") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtInsertFinishedGoodPartRevision" runat="server" MaxLength="2"
                                            Width="20px"></asp:TextBox>
                                    </FooterTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="FinishedGoodPartName"></asp:BoundField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="iBtnKitDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                            ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgInsertKit"
                                            runat="server" ID="iBtnKitSave" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                                        <asp:ImageButton ID="iBtnKitUndo" runat="server" CommandName="Undo" CausesValidation="false"
                                            ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsKit" runat="server" OldValuesParameterFormatString="original_{0}"
                            SelectMethod="GetECIKit" TypeName="ECIKitBLL" DeleteMethod="DeleteECIKit" InsertMethod="InsertECIKit">
                            <SelectParameters>
                                <asp:ControlParameter Name="ECINo" Type="Int32" ControlID="lblECINo" DefaultValue="0" />
                            </SelectParameters>
                            <DeleteParameters>
                                <asp:Parameter Name="RowID" Type="Int32" />
                                <asp:Parameter Name="original_RowID" Type="Int32" />
                            </DeleteParameters>
                            <InsertParameters>
                                <asp:Parameter Name="ECINo" Type="Int32" />
                                <asp:Parameter Name="KitPartNo" Type="String" />
                                <asp:Parameter Name="KitPartRevision" Type="String" />
                                <asp:Parameter Name="FinishedGoodPartNo" Type="String" />
                                <asp:Parameter Name="FinishedGoodPartRevision" Type="String" />
                            </InsertParameters>
                        </asp:ObjectDataSource>
                    </Content>
                </ajax:AccordionPane>
            </Panes>
        </ajax:Accordion>
        <ajax:Accordion ID="accSupportingDocs" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
            FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
            RequireOpenedPane="false" SuppressHeaderPostbacks="true" Visible="false">
            <Panes>
                <ajax:AccordionPane ID="apSupportingDocs" runat="server">
                    <Header>
                        <a href="">Supporting Documents</a></Header>
                    <Content>
                        <asp:Label runat="server" ID="lblMessageSupportingDocs"></asp:Label>
                        <asp:ValidationSummary ID="vsSupportingDocs" runat="server" DisplayMode="List" ShowMessageBox="true"
                            ShowSummary="true" ValidationGroup="vgSupportingDocs" />
                        <br />
                        <table width="98%" runat="server" id="tblUpload" visible="false">
                            <tr>
                                <td class="p_textbold" valign="top">
                                    File Description:
                                </td>
                                <td class="c_text">
                                    <asp:TextBox ID="txtSupportingDocDesc" runat="server" MaxLength="200" Rows="3" TextMode="MultiLine"
                                        Width="600px" />
                                    <asp:RequiredFieldValidator ID="rfvSupportingDocDesc" runat="server" ControlToValidate="txtSupportingDocDesc"
                                        ErrorMessage="Supporting Document File Description is a required field." Font-Bold="False"
                                        ValidationGroup="vgSupportingDocs"><</asp:RequiredFieldValidator><br />
                                    <br />
                                    <asp:Label ID="lblSupportingDocDescCharCount" SkinID="MessageLabelSkin" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="white-space: nowrap;">
                                    <asp:Label runat="server" ID="lblFileUploadLabel" Text="Upload a supporting file under 3 MB:<br />(PDF,DOC,DOCX,XLS,JPEG,TIF,MSG,PPT)"
                                        CssClass="p_textbold"></asp:Label>
                                </td>
                                <td style="white-space: nowrap;">
                                    <asp:FileUpload ID="fileUploadSupportingDoc" runat="server" Width="600px" />
                                    <asp:Button ID="btnSaveUploadSupportingDocument" runat="server" Text="Upload" CausesValidation="true"
                                        ValidationGroup="vgSupportingDocs" />
                                    <asp:RequiredFieldValidator ID="rfvFileUploadSupportingDoc" runat="server" ControlToValidate="fileUploadSupportingDoc"
                                        ErrorMessage="PDF File is required." Font-Bold="False" ValidationGroup="vgUpload"
                                        SetFocusOnError="true" Text="<"></asp:RequiredFieldValidator><br />
                                    <asp:RegularExpressionValidator ID="revUploadFile" runat="server" ErrorMessage="Please upload only *.PDF, *.DOC,*.DOCX, *.XLS, *.JPEG, *.JPG, *.TIF files are allowed."
                                        ValidationExpression="(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.pdf|.xls|.xlsx|.doc|.docx|.jpeg|.jpg|.tif|.msg|.ppt|.PDF|.XLS|.XLSX|.DOC|.DOCX|.JPEG|.JPG|.TIF|.MSG|.PPT)$"
                                        ControlToValidate="fileUploadSupportingDoc" ValidationGroup="vgSupportingDocs"
                                        SetFocusOnError="true" Font-Bold="True" Font-Size="Small" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Label runat="server" ID="lblMaxNote" Text="(A maximum of three supporting documents are allowed.)"
                            Visible="false"></asp:Label>
                        <br />
                        <asp:GridView ID="gvSupportingDoc" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                            AllowPaging="True" PageSize="15" DataKeyNames="RowID" ShowFooter="False" DataSourceID="odsSupportingDoc"
                            EmptyDataText="No supporting documents exist yet." Width="98%">
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#CCCCCC" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="RowID">
                                    <ItemStyle CssClass="none" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Supporting Document Name" SortExpression="SupportingDocName">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lnkViewSupportingDoc" runat="server" NavigateUrl='<%# Eval("RowID", "~/ECI/ECI_Supporting_Doc_View.aspx?RowID={0}") %>'
                                            Target="_blank" Text='<%# Eval("SupportingDocName") %>'>
                                        </asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Description" DataField="SupportingDocDesc">
                                    <ControlStyle Font-Size="X-Small" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Preview Document">
                                    <ItemTemplate>
                                        <asp:HyperLink runat="server" ID="hlnkPreview" ImageUrl='<%# DisplayImage(DataBinder.Eval(Container, "DataItem.SupportingDocEncodeType").tostring)  %>'
                                            NavigateUrl='<%# "ECI_Supporting_Doc_View.aspx?RowID=" & DataBinder.Eval (Container.DataItem,"RowID").tostring %>'
                                            Target="_blank" ToolTip="Preview Document" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="iBtnSupportingDocDelete" runat="server" CausesValidation="False"
                                            CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsSupportingDoc" runat="server" OldValuesParameterFormatString="original_{0}"
                            SelectMethod="GetECISupportingDoc" TypeName="ECISupportingDocBLL" DeleteMethod="DeleteECISupportingDoc">
                            <SelectParameters>
                                <asp:ControlParameter Name="ECINo" Type="Int32" ControlID="lblECINo" DefaultValue="0" />
                            </SelectParameters>
                            <DeleteParameters>
                                <asp:Parameter Name="RowID" Type="Int32" />
                                <asp:Parameter Name="original_RowID" Type="Int32" />
                            </DeleteParameters>
                        </asp:ObjectDataSource>
                    </Content>
                </ajax:AccordionPane>
            </Panes>
        </ajax:Accordion>
        <ajax:Accordion ID="accAssignedTask" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
            FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
            RequireOpenedPane="false" SuppressHeaderPostbacks="true" Visible="false">
            <Panes>
                <ajax:AccordionPane ID="apAssignedTask" runat="server">
                    <Header>
                        <a href="">Assigned Tasks to Team Members</a></Header>
                    <Content>
                        <asp:ValidationSummary runat="server" ID="vsEditAssignedTask" ValidationGroup="vgEditAssignedTask"
                            ShowMessageBox="true" ShowSummary="true" />
                        <asp:ValidationSummary runat="server" ID="vsInserAssignedTask" ValidationGroup="vgInsertAssignedTask"
                            ShowMessageBox="true" ShowSummary="true" />
                        <br />
                        <asp:Label runat="server" ID="lblMessageAssignedTask" SkinID="MessageLabelSkin"></asp:Label>
                        <br />
                        <asp:GridView ID="gvAssignedTask" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                            AllowPaging="True" PageSize="15" DataKeyNames="RowID" ShowFooter="False" DataSourceID="odsAssignedTask"
                            EmptyDataText="No records found" Width="98%">
                            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#CCCCCC" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="RowID">
                                    <ItemStyle CssClass="none" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Task" SortExpression="ddTaskName">
                                    <EditItemTemplate>
                                        <asp:Label ID="lblEditAssignedTaskNameMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                            Text="* " />
                                        <asp:DropDownList ID="ddEditAssignedTaskName" runat="server" DataSource='<%# ECIModule.GetECITaskDesc(0,"") %>'
                                            DataValueField="TaskID" DataTextField="ddTaskName" AppendDataBoundItems="True"
                                            SelectedValue='<%# Bind("TaskID") %>'>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvEditAssignedTaskName" runat="server" ControlToValidate="ddEditAssignedTaskName"
                                            ErrorMessage="Task Name is required." Font-Bold="True" ValidationGroup="vgEditAssignedTask"
                                            Text="<">				     
                                        </asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblViewAssignedTaskName" runat="server" Text='<%# Bind("ddTaskName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblInsertAssignedTaskNameMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                            Text="* " />
                                        <asp:DropDownList ID="ddInsertAssignedTaskName" runat="server" DataSource='<%# ECIModule.GetECITaskDesc(0,"") %>'
                                            DataValueField="TaskID" DataTextField="ddTaskName" AppendDataBoundItems="True">
                                            <asp:ListItem Text="" Value="0" Selected="False"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvInsertAssignedTaskName" runat="server" ControlToValidate="ddInsertAssignedTaskName"
                                            ErrorMessage="Task Name is required." Font-Bold="True" ValidationGroup="vgInsertAssignedTask"
                                            Text="<">				     
                                        </asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Team Member" SortExpression="ddTeamMemberName">
                                    <EditItemTemplate>
                                        <asp:Label ID="lblEditAssignedTaskTeamMemberMarker" runat="server" Font-Bold="True"
                                            ForeColor="Red" Text="* " />
                                        <asp:DropDownList ID="ddEditAssignedTaskTeamMember" runat="server" DataSource='<%# commonFunctions.GetTeamMemberBySubscription(64) %>'
                                            DataValueField="TMID" DataTextField="TMName" AppendDataBoundItems="True" SelectedValue='<%# Bind("TaskTeamMemberID") %>'>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvEditAssignedTaskTeamMember" runat="server" ControlToValidate="ddEditAssignedTaskTeamMember"
                                            ErrorMessage="Team Member is required." Font-Bold="True" ValidationGroup="vgEditAssignedTask"
                                            Text="<">				     
                                        </asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblViewAssignedTaskTeamMember" runat="server" Text='<%# Bind("ddTeamMemberName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblInsertAssignedTaskTeamMemberMarker" runat="server" Font-Bold="True"
                                            ForeColor="Red" Text="* " />
                                        <asp:DropDownList ID="ddInsertAssignedTaskTeamMember" runat="server" DataSource='<%# commonFunctions.GetTeamMemberBySubscription(64) %>'
                                            DataValueField="TMID" DataTextField="TMName" AppendDataBoundItems="True" SelectedValue='<%# Bind("TaskTeamMemberID") %>'>
                                            <asp:ListItem Text="" Value="0" Selected="False"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvInsertAssignedTaskTeamMember" runat="server" ControlToValidate="ddInsertAssignedTaskTeamMember"
                                            ErrorMessage="Team Member is required." Font-Bold="True" ValidationGroup="vgInsertAssignedTask"
                                            Text="<">				     
                                        </asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Target Date" SortExpression="TargetDate">
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" ID="txtEditAssignedTaskTargetDate" Text='<%# Bind("TargetDate") %>'
                                            MaxLength="10"></asp:TextBox>
                                        <asp:ImageButton runat="server" ID="imgEditAssignedTaskTargetDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                            AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                                        <ajax:CalendarExtender ID="ceEditAssignedTaskTargetDate" runat="server" TargetControlID="txtEditAssignedTaskTargetDate"
                                            PopupButtonID="imgEditAssignedTaskTargetDate" />
                                        <asp:RegularExpressionValidator ID="revEditAssignedTaskTargetDate" runat="server"
                                            ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format ' ControlToValidate="txtEditAssignedTaskTargetDate"
                                            Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                            Width="8px" ValidationGroup="vgEditAssignedTask"><</asp:RegularExpressionValidator>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblViewAssignedTaskTargetDate" runat="server" Text='<%# Bind("TargetDate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox runat="server" ID="txtInsertAssignedTaskTargetDate" Text='<%# Bind("TargetDate") %>'
                                            MaxLength="10"></asp:TextBox>
                                        <asp:ImageButton runat="server" ID="imgInsertAssignedTaskTargetDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                            AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                                        <ajax:CalendarExtender ID="ceInsertAssignedTaskTargetDate" runat="server" TargetControlID="txtInsertAssignedTaskTargetDate"
                                            PopupButtonID="imgInsertAssignedTaskTargetDate" />
                                        <asp:RegularExpressionValidator ID="revInsertAssignedTaskTargetDate" runat="server"
                                            ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format ' ControlToValidate="txtInsertAssignedTaskTargetDate"
                                            Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                            Width="8px" ValidationGroup="vgInsertAssignedTask"><</asp:RegularExpressionValidator>
                                    </FooterTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <EditItemTemplate>
                                        <asp:ImageButton ID="iBtnAssignedTaskUpdate" runat="server" CausesValidation="True"
                                            CommandName="Update" ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditAssignedTask" />
                                        <asp:ImageButton ID="iBtnAssignedTaskCancel" runat="server" CausesValidation="False"
                                            CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="iBtnAssignedTaskEdit" runat="server" CausesValidation="False"
                                            CommandName="Edit" ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                                        <asp:ImageButton ID="iBtnAssignedTaskDelete" runat="server" CausesValidation="False"
                                            CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgInsertAssignedTask"
                                            runat="server" ID="iBtnAssignedTaskSave" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                                        <asp:ImageButton ID="iBtnAssignedTaskUndo" runat="server" CommandName="Undo" CausesValidation="false"
                                            ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsAssignedTask" runat="server" OldValuesParameterFormatString="original_{0}"
                            SelectMethod="GetECITask" TypeName="ECITaskBLL" DeleteMethod="DeleteECITask"
                            InsertMethod="InsertECITask" UpdateMethod="UpdateECITask">
                            <SelectParameters>
                                <asp:ControlParameter Name="ECINo" Type="Int32" ControlID="lblECINo" DefaultValue="0" />
                            </SelectParameters>
                            <DeleteParameters>
                                <asp:Parameter Name="RowID" Type="Int32" />
                                <asp:Parameter Name="original_RowID" Type="Int32" />
                            </DeleteParameters>
                            <UpdateParameters>
                                <asp:Parameter Name="RowID" Type="Int32" />
                                <asp:Parameter Name="original_RowID" Type="Int32" />
                                <asp:Parameter Name="TaskID" Type="Int32" />
                                <asp:Parameter Name="TaskTeamMemberID" Type="Int32" />
                                <asp:Parameter Name="TargetDate" Type="String" />
                            </UpdateParameters>
                            <InsertParameters>
                                <asp:Parameter Name="ECINo" Type="Int32" />
                                <asp:Parameter Name="TaskID" Type="Int32" />
                                <asp:Parameter Name="TaskTeamMemberID" Type="Int32" />
                                <asp:Parameter Name="TargetDate" Type="String" />
                            </InsertParameters>
                        </asp:ObjectDataSource>
                    </Content>
                </ajax:AccordionPane>
            </Panes>
        </ajax:Accordion>
        <ajax:Accordion ID="accNotification" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
            FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
            RequireOpenedPane="false" SuppressHeaderPostbacks="true" Visible="false">
            <Panes>
                <ajax:AccordionPane ID="apNotification" runat="server">
                    <Header>
                        <a href="">ECI Notification</a></Header>
                    <Content>
                        <asp:Label runat="server" ID="lblMessageECINotification" SkinID="MessageLabelSkin"></asp:Label>
                        <asp:ValidationSummary runat="server" ID="vsNotification" ValidationGroup="vgNotification"
                            ShowMessageBox="true" ShowSummary="true" />
                        <asp:ValidationSummary runat="server" ID="vsInsertNotificationGroup" ValidationGroup="vgInsertNotificationGroup"
                            ShowMessageBox="true" ShowSummary="true" />
                        <asp:GridView ID="gvNotificationGroup" runat="server" AutoGenerateColumns="False"
                            AllowSorting="True" AllowPaging="True" PageSize="15" DataKeyNames="RowID" DataSourceID="odsNotificationGroup"
                            EmptyDataText="Notification Groups have NOT been assigned yet." Width="98%">
                            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#CCCCCC" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="RowID">
                                    <ItemStyle CssClass="none" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Group" SortExpression="ddGroupName">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lnkViewNotificationGroup" runat="server" NavigateUrl='<%# Eval("GroupID", "ECI_Notification_Group_Maint.aspx?GroupID={0}") %>'
                                            Target="_blank" Text='<%# Eval("ddGroupName") %>'></asp:HyperLink>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblInsertNotificationGroupMarker" runat="server" Font-Bold="True"
                                            ForeColor="Red" Text="* " />
                                        <asp:DropDownList ID="ddInsertNotificationGroup" runat="server" DataSource='<%# ECIModule.GetECIGroup(0,"") %>'
                                            DataValueField="GroupID" DataTextField="ddGroupName" AppendDataBoundItems="True">
                                            <asp:ListItem Text="" Value="0" Selected="False"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvInsertNotificationGroup" runat="server" ControlToValidate="ddInsertNotificationGroup"
                                            ErrorMessage="Notification Group is required." Font-Bold="True" ValidationGroup="vgInsertNotificationGroup"
                                            Text="<">				     
                                        </asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="iBtnNotificationGroupDelete" runat="server" CausesValidation="False"
                                            CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgInsertNotificationGroup"
                                            runat="server" ID="iBtnNotificationGroupSave" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                                        <asp:ImageButton ID="iBtnNotificationGroupUndo" runat="server" CommandName="Undo"
                                            CausesValidation="false" ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsNotificationGroup" runat="server" OldValuesParameterFormatString="original_{0}"
                            TypeName="ECINotificationGroupBLL" DeleteMethod="DeleteECINotificationGroup"
                            InsertMethod="InsertECINotificationGroup" SelectMethod="GetECINotificationGroup">
                            <DeleteParameters>
                                <asp:Parameter Name="RowID" Type="Int32" />
                            </DeleteParameters>
                            <SelectParameters>
                                <asp:ControlParameter Name="ECINo" Type="Int32" ControlID="lblECINo" DefaultValue="0" />
                            </SelectParameters>
                            <InsertParameters>
                                <asp:Parameter Name="ECINo" Type="Int32" />
                                <asp:Parameter Name="GroupID" Type="Int32" />
                            </InsertParameters>
                        </asp:ObjectDataSource>
                        <br />
                        <table width="98%">
                            <tr>
                                <td class="p_text" style="white-space: nowrap;">
                                    Email Comments:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmailComments" runat="server" TextMode="MultiLine" Width="500px"
                                        Text="This notification is being issued to inform you of a new ECI release. Upon receipt of this notification, please take the necessary steps as indicated in the ECI fields."
                                        Height="80"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center">
                                    <asp:Button ID="btnRelease" runat="server" Text="Release and Notify (ECI Complete)"
                                        Visible="false" ValidationGroup="vgNotification" />
                                    <asp:Button ID="btnUpdateFooter" runat="server" Text="Update And Notify" Visible="false"
                                        ValidationGroup="vgSave" />
                                    <asp:Button ID="btnPreviewECIBottom" runat="server" Text="Preview ECI" Visible="false"
                                        CausesValidation="false" />
                                    <asp:Button ID="btnPreviewUgnIPPBottom" runat="server" Text="Preview UGN IPP" Visible="false"
                                        CausesValidation="false" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:GridView ID="gvNotificationSent" runat="server" AutoGenerateColumns="False"
                            AllowSorting="True" AllowPaging="True" PageSize="15" DataKeyNames="RowID" ShowFooter="False"
                            DataSourceID="odsNotificationSent" EmptyDataText="Notifications NOT sent yet."
                            Width="98%">
                            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#CCCCCC" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="ddTeamMemberFullName" HeaderText="Team Member">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="NotificationSent" HeaderText="Notification Sent">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsNotificationSent" runat="server" OldValuesParameterFormatString="original_{0}"
                            SelectMethod="GetECINotification" TypeName="ECINotificationBLL">
                            <SelectParameters>
                                <asp:ControlParameter Name="ECINo" Type="Int32" ControlID="lblECINo" DefaultValue="0" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <br />
                        <asp:GridView ID="gvHistory" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                            AllowPaging="True" PageSize="15" DataSourceID="odsHistory" EmptyDataText="No history recorded yet."
                            Width="98%">
                            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#CCCCCC" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="HistoryDesc" HeaderText="Action" SortExpression="HistoryDesc">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CreatedBy" HeaderText="Team Member" SortExpression="CreatedBy">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="FormattedCreatedOn" HeaderText="Date" ReadOnly="True"
                                    SortExpression="FormattedCreatedOn">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsHistory" runat="server" OldValuesParameterFormatString="original_{0}"
                            SelectMethod="GetECIHistory" TypeName="ECIHistoryBLL">
                            <SelectParameters>
                                <asp:ControlParameter Name="ECINo" Type="Int32" ControlID="lblECINo" DefaultValue="0" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </Content>
                </ajax:AccordionPane>
            </Panes>
        </ajax:Accordion>
    </asp:Panel>
</asp:Content>
