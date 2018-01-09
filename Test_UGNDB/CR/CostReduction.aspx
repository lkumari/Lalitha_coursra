<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="CostReduction.aspx.vb" Inherits="CR_CostReduction" Title="Untitled Page"
    MaintainScrollPositionOnPostback="True" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="100%">
        <asp:Label ID="lblErrors" runat="server" SkinID="MessageLabelSkin" />
        <% If ViewState("pProjNo") > 0 Then%>
        <table>
            <tr>
                <td class="p_smalltextbold" style="color: #990000;">
                    Edit data below or press
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CausesValidation="False" />
                    to enter new data.
                </td>
            </tr>
        </table>
        <% End If%>
        <hr />
        <br />
        <%-- Project Detail--%>
        <ajax:Accordion ID="accProjectDetail" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
            FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
            RequireOpenedPane="false" SuppressHeaderPostbacks="true">
            <Panes>
                <ajax:AccordionPane ID="apProjectDetail" runat="server">
                    <Header>
                        1. <a href="" class="accordionLink">Project Detail</a></Header>
                    <Content>
                        <table width="100%">
                            <tr>
                                <td class="p_text" style="width: 200px;">
                                    Project No:
                                </td>
                                <td style="color: #990000;" colspan="2">
                                    <asp:Label ID="lblProjectNo" runat="server" Text="0" CssClass="c_text" Font-Bold="True"
                                        Font-Overline="False" Font-Size="Larger" Font-Underline="False" Visible="true" />
                                    <asp:TextBox ID="txtHDEstImpDate" runat="server" Visible="False" Width="20px" />
                                    <asp:TextBox ID="txtHDEstAnnCostSave" runat="server" Visible="False" Width="20px" />
                                    <asp:TextBox ID="txtHDCapEx" runat="server" Visible="False" Width="20px" />
                                    <asp:TextBox ID="txtHDSuccessRate" runat="server" Visible="False" Width="20px" />
                                    <asp:TextBox ID="txtTodaysDate" runat="server" ReadOnly="True" Width="80px" Visible="False" />
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text" valign="top" style="width: 200px;">
                                    <asp:Label ID="lblDescriptionMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                        Text="*" />
                                    Description:
                                </td>
                                <td class="c_textbold" style="color: red" colspan="2">
                                    <asp:TextBox ID="txtDescription" runat="server" MaxLength="200" Width="600px" TextMode="MultiLine"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription"
                                        ErrorMessage="Description is a required field." Font-Bold="False" ValidationGroup="ProjectInfo"><</asp:RequiredFieldValidator><br />
                                    <asp:Label ID="lblDescription" runat="server" Font-Bold="True" ForeColor="Red" />
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text" style="width: 200px;">
                                    <asp:Label ID="lblProjectCategoryMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                        Text="*" />
                                    Project Category:
                                </td>
                                <td class="c_text" colspan="2">
                                    <asp:DropDownList ID="ddProjectCategory" runat="server" AutoPostBack="true" />
                                    <asp:RequiredFieldValidator ID="rfvProjectCategory" runat="server" ControlToValidate="ddProjectCategory"
                                        ErrorMessage="Project Category is a required field." Font-Bold="False" ValidationGroup="ProjectInfo"><</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text" style="width: 200px;">
                                    <asp:Label ID="lblProjectLeaderMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                        Text="*" />
                                    Project Leader:
                                </td>
                                <td class="c_text" colspan="2">
                                    <asp:DropDownList ID="ddLeader" runat="server" />
                                    <asp:RequiredFieldValidator ID="rfvLeader" runat="server" ControlToValidate="ddLeader"
                                        ErrorMessage="Leader is a required field." Font-Bold="False" ValidationGroup="ProjectInfo"><</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    <asp:Label ID="lblUGNFacilityMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                        Text="*" />
                                    UGN Facility:
                                </td>
                                <td class="c_text" colspan="2">
                                    <asp:DropDownList ID="ddUGNFacility" runat="server" />
                                    <asp:RequiredFieldValidator ID="rfvUGNFacility" runat="server" ControlToValidate="ddUGNFacility"
                                        ErrorMessage="UGN Facility is a required field." Font-Bold="False" ValidationGroup="ProjectInfo"><</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text" valign="top">
                                    <asp:Label ID="lblCommodityMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                        Text="*" />
                                    Commodity:
                                </td>
                                <td class="c_text" colspan="2" valign="top">
                                    <asp:DropDownList ID="ddCommodity" runat="server" />
                                    <asp:RequiredFieldValidator ID="rfvCommodity" runat="server" ControlToValidate="ddCommodity"
                                        ErrorMessage="Commodity is a required field." Font-Bold="False" ValidationGroup="ProjectInfo"><</asp:RequiredFieldValidator>
                                    <br />
                                    {Commodity / Classification}
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    RFD No:
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtRFDNo" runat="server" Width="100px">0</asp:TextBox>
                                    <asp:RangeValidator ID="rvRFDNo" runat="server" ControlToValidate="txtRFDNo" Display="Dynamic"
                                        ErrorMessage="RFD No requires a numeric value 0 to 999999" Height="16px" MaximumValue="999999"
                                        MinimumValue="0" Type="Integer" ValidationGroup="ProjectInfo"><</asp:RangeValidator>
                                    <ajax:FilteredTextBoxExtender ID="ftbRFDNo" runat="server" TargetControlID="txtRFDNo"
                                        FilterType="Custom, Numbers" />
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    Capital Project No:
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtCapExProjNo" runat="server" Width="100px"></asp:TextBox>
                                    <ajax:FilteredTextBoxExtender ID="ftbCapExProjNo" runat="server" TargetControlID="txtCapExProjNo"
                                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890" />
                                </td>
                            </tr>
                            <% If ViewState("pProjNo") = 0 Then%>
                            <tr>
                                <td class="p_text">
                                    <asp:Label ID="lblEstAnnCostSaveMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                        Text="*" />
                                    Gross Annual Cost Save ($):
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtEstAnnCostSave" runat="server" Width="120px" MaxLength="16" Enabled="false"
                                        Text="0" />
                                    <asp:RangeValidator ID="rvEstAnnCostSave" runat="server" ControlToValidate="txtEstAnnCostSave"
                                        Display="Dynamic" ErrorMessage="Annual Cost Savings requires a numeric value -999999999.99 to 999999999.99"
                                        Height="16px" MaximumValue="999999999.99" MinimumValue="-999999999.99" Type="Double"
                                        SetFocusOnError="true" Text="<" ValidationGroup="ProjectInfo"></asp:RangeValidator>
                                    <ajax:FilteredTextBoxExtender ID="ftbEstAnnCostSave" runat="server" TargetControlID="txtEstAnnCostSave"
                                        FilterType="Custom, Numbers" ValidChars="-." />
                                    <asp:RequiredFieldValidator ID="rfvEstAnnCostSave" runat="server" ControlToValidate="txtEstAnnCostSave"
                                        ErrorMessage="Annual Cost Save is a required field." ValidationGroup="ProjectInfo"><</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <% Else%>
                            <tr>
                                <td class="p_text">
                                    <asp:Label ID="lblNextAnnCostSaveMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                        Text="*" />
                                    Actual Gross Annual Cost Save ($):
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNextAnnCostSave" runat="server" Width="120px" MaxLength="16"
                                        Text="0" Enabled="false" />
                                    <asp:RangeValidator ID="rvNextAnnCostSave" runat="server" ControlToValidate="txtNextAnnCostSave"
                                        Display="Dynamic" ErrorMessage="Annual Cost Savings requires a numeric value -999999999.99 to 999999999.99"
                                        Height="16px" MaximumValue="999999999.99" MinimumValue="-999999999.99" Type="Double"
                                        ValidationGroup="ProjectInfo"><</asp:RangeValidator>
                                    <asp:RequiredFieldValidator ID="rfvNextAnnCostSave" runat="server" ControlToValidate="txtNextAnnCostSave"
                                        ErrorMessage="Annual Cost Save is a required field." ValidationGroup="ProjectInfo"><</asp:RequiredFieldValidator><ajax:FilteredTextBoxExtender
                                            ID="ftbNextAnnCostSave" runat="server" TargetControlID="txtNextAnnCostSave" FilterType="Custom, Numbers"
                                            ValidChars="-." />
                                </td>
                                <td>
                                    <asp:Label ID="lblReqAnnCostChngRsn" runat="server" Font-Bold="True" ForeColor="Red"
                                        Text="*" Visible="false" />
                                    <asp:Label ID="lblAnnCostChngRsn" runat="server" Text="Change Reason:" ForeColor="Red"
                                        Visible="false" /><asp:TextBox ID="txtAnnCostChngRsn" runat="server" Width="400px"
                                            Visible="false" />
                                    <asp:RequiredFieldValidator ID="rfvAnnCostChngRsn" runat="server" ErrorMessage="Annual Cost Change Reason is a required field."
                                        ControlToValidate="txtAnnCostChngRsn" ValidationGroup="ProjectInfo"><</asp:RequiredFieldValidator><br />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label
                                        ID="lblAnnCostSave" runat="server" Font-Bold="True" ForeColor="Red" />
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    <asp:Label runat="server" ID="lblCustomerGiveBackDollar">Customer Give Back ($):</asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtCustomerGiveBackDollar" runat="server" Width="120px" MaxLength="16"
                                        Text="0" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    <asp:Label runat="server" ID="lblActualNetAnnualCostSavings">Actual Net Annual Cost Save ($):</asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtActualNetAnnualCostSavings" runat="server" Width="120px" MaxLength="16"
                                        Text="0" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text">
                                    <asp:Label runat="server" ID="lblBudgetNetAnnualCostSavings">Budget Net Annual Cost Save ($):</asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtBudgetNetAnnualCostSavings" runat="server" Width="120px" MaxLength="16"
                                        Text="0" Enabled="false" />
                                </td>
                            </tr>
                            <% End If%>
                            <% If ViewState("pProjNo") = 0 Then%>
                            <tr>
                                <td class="p_text">
                                    <asp:Label ID="lblCapExMarker" runat="server" Font-Bold="True" ForeColor="Red" Text="*" />
                                    CAPEX ($):
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtCapEx" runat="server" Width="120px" MaxLength="16" Enabled="false"
                                        Text="0" />
                                    <asp:RangeValidator ID="rvCapEx" runat="server" ControlToValidate="txtCapEx" Display="Dynamic"
                                        ErrorMessage="CAPEX requires a numeric value -999999999.99 to 999999999.99" Height="16px"
                                        MaximumValue="999999999.99" MinimumValue="-999999999.99" Type="Double" ValidationGroup="ProjectInfo"><</asp:RangeValidator>
                                    <ajax:FilteredTextBoxExtender ID="ftbCapEx" runat="server" TargetControlID="txtCapEx"
                                        FilterType="Custom, Numbers" ValidChars="-." />
                                    <asp:RequiredFieldValidator ID="rfvCapEx" runat="server" ControlToValidate="txtCapEx"
                                        ErrorMessage="CAPEX is a required field." ValidationGroup="ProjectInfo"><</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <% Else%>
                            <tr>
                                <td class="p_text">
                                    <asp:Label ID="lblNextCapExMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                        Text="*" />
                                    CAPEX ($):
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNextCapEx" runat="server" Width="120px" MaxLength="16" Enabled="false" />
                                    <asp:RangeValidator ID="rvNextCapEx" runat="server" ControlToValidate="txtNextCapEx"
                                        Display="Dynamic" ErrorMessage="CAPEX requires a numeric value -999999999.99 to 999999999.99"
                                        Height="16px" MaximumValue="999999999.99" MinimumValue="-999999999.99" Type="Double"
                                        ValidationGroup="ProjectInfo"><</asp:RangeValidator>
                                    <asp:RequiredFieldValidator ID="rfvNextCapEx" runat="server" ControlToValidate="txtNextCapEx"
                                        ErrorMessage="CAPEX is a required field." ValidationGroup="ProjectInfo"><</asp:RequiredFieldValidator><ajax:FilteredTextBoxExtender
                                            ID="ftbNextCapEx" runat="server" TargetControlID="txtNextCapEx" FilterType="Custom, Numbers"
                                            ValidChars="-." />
                                </td>
                                <td>
                                    <asp:Label ID="lblReqCapExChngRsn" runat="server" Font-Bold="True" ForeColor="Red"
                                        Text="*" Visible="false" />
                                    <asp:Label ID="lblCapExChngRsn" runat="server" Text="Change Reason:" ForeColor="Red"
                                        Visible="false" /><asp:TextBox ID="txtCapExChngRsn" runat="server" Width="400px"
                                            Visible="false" />
                                    <asp:RequiredFieldValidator ID="rfvCapExChngRsn" runat="server" ErrorMessage="CAPEX Change Reason is a required field."
                                        ControlToValidate="txtCapExChngRsn" ValidationGroup="ProjectInfo"><</asp:RequiredFieldValidator><br />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label
                                        ID="lblCapEx" runat="server" Font-Bold="True" ForeColor="Red" />
                                </td>
                            </tr>
                            <% End If%>
                            <% If ViewState("pProjNo") = 0 Then%>
                            <tr>
                                <td class="p_text">
                                    <asp:Label ID="Label5" runat="server" Font-Bold="True" ForeColor="Red" Text="*" />
                                    Success Rate (%):
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtSuccessRate" runat="server" Width="30px" MaxLength="3" />
                                    <asp:RangeValidator ID="rvSuccessRate" runat="server" ControlToValidate="txtSuccessRate"
                                        Display="Dynamic" ErrorMessage="Success Rate requires a numeric value 0 to 100"
                                        Height="16px" MaximumValue="100" MinimumValue="0" Type="Integer" ValidationGroup="ProjectInfo"><</asp:RangeValidator>
                                    <ajax:FilteredTextBoxExtender ID="ftbSuccessRate" runat="server" TargetControlID="txtSuccessRate"
                                        FilterType="Numbers" />
                                    <asp:RequiredFieldValidator ID="rfvSuccessRate" runat="server" ControlToValidate="txtSuccessRate"
                                        ErrorMessage="Success Rate is a required field." ValidationGroup="ProjectInfo"><</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <% Else%>
                            <tr>
                                <td class="p_text">
                                    <asp:Label ID="lblNextSuccessRateMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                        Text="*" />
                                    Success Rate (%):
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNextSuccessRate" runat="server" Width="30px" MaxLength="16" AutoPostBack="true" />
                                    <asp:RangeValidator ID="rvNextSuccessRate" runat="server" ControlToValidate="txtNextSuccessRate"
                                        Display="Dynamic" ErrorMessage="Success Rate requires a numeric value 0 to 100"
                                        Height="16px" MaximumValue="100" MinimumValue="0" Type="Double" ValidationGroup="ProjectInfo"><</asp:RangeValidator>
                                    <asp:RequiredFieldValidator ID="rfvNextSuccessRate" runat="server" ControlToValidate="txtNextSuccessRate"
                                        ErrorMessage="Success Rate is a required field." ValidationGroup="ProjectInfo"><</asp:RequiredFieldValidator>
                                    <ajax:FilteredTextBoxExtender ID="ftbeNextSuccessRate" runat="server" TargetControlID="txtNextSuccessRate"
                                        FilterType="Numbers" />
                                </td>
                                <td>
                                    <asp:Label ID="lblReqSuccessRateChngRsn" runat="server" Font-Bold="True" ForeColor="Red"
                                        Text="*" Visible="false" />
                                    <asp:Label ID="lblSuccessRateChngRsn" runat="server" Text="Change Reason:" ForeColor="Red"
                                        Visible="false" /><asp:TextBox ID="txtSuccessRateChngRsn" runat="server" Width="400px"
                                            Visible="false" />
                                    <asp:RequiredFieldValidator ID="rfvSuccessRateChngRsn" runat="server" ErrorMessage="Success Rate Change Reason is a required field."
                                        ControlToValidate="txtSuccessRateChngRsn" ValidationGroup="ProjectInfo"><</asp:RequiredFieldValidator><br />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label
                                        ID="lblSuccessRate" runat="server" Font-Bold="True" ForeColor="Red" />
                                </td>
                            </tr>
                            <% End If%>
                            <tr>
                                <td class="p_text">
                                    Rank:
                                </td>
                                <td colspan="2" class="c_text">
                                    <asp:Label ID="lblRank" runat="server" CssClass="c_text" Text='0' />
                                </td>
                            </tr>
                            <% If ViewState("pProjNo") > 0 And txtDateSubmitted.Text <> "" Then%>
                            <tr>
                                <td class="p_text">
                                    Date Submitted:
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtDateSubmitted" Width="80px" runat="server" Enabled="true" />
                                </td>
                            </tr>
                            <% End If%>
                            <% If ViewState("pProjNo") = 0 Then%>
                            <tr>
                                <td class="p_text">
                                    <asp:Label ID="Label7" runat="server" Font-Bold="True" ForeColor="Red" Text="*" />
                                    Implementation Date:
                                </td>
                                <td class="c_text" colspan="2">
                                    <asp:TextBox ID="txtEstImpDate" runat="server" Width="80px" MaxLength="10" />
                                    <asp:ImageButton runat="server" ID="imgEstImpDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                                        CausesValidation="False" />
                                    <asp:RequiredFieldValidator ID="rfvEstImpDate" runat="server" ControlToValidate="txtEstImpDate"
                                        ErrorMessage="Implementation Date is a required field." ValidationGroup="ProjectInfo"><</asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revEstImpDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                        ControlToValidate="txtEstImpDate" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                        Width="8px" ValidationGroup="ProjectInfo"><</asp:RegularExpressionValidator>
                                    <ajax:CalendarExtender ID="ceEstImpDate" runat="server" TargetControlID="txtEstImpDate"
                                        Format="MM/dd/yyyy" PopupButtonID="imgEstImpDate" />
                                    <asp:CompareValidator ID="cvImpDt1" runat="server" ControlToCompare="txtTodaysDate"
                                        ControlToValidate="txtEstImpDate" ErrorMessage="Implementation Date must be greater than or equal to Current Date."
                                        ValidationGroup="ProjectInfo" Operator="GreaterThanEqual" Type="Date"><</asp:CompareValidator>
                                </td>
                            </tr>
                            <% Else%>
                            <tr>
                                <td class="p_text">
                                    <asp:Label ID="lblImlementationDateMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                        Text="*" />
                                    Implementation Date:
                                </td>
                                <td class="c_text">
                                    <asp:TextBox ID="txtNextImpDate" runat="server" Width="80px" MaxLength="10" AutoPostBack="true"
                                        CausesValidation="true" ValidationGroup="ProjectInfo" />
                                    <asp:ImageButton runat="server" ID="imgNextImpDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                                        CausesValidation="False" />
                                    <asp:RequiredFieldValidator ID="rfvNextImpDate" runat="server" ControlToValidate="txtNextImpDate"
                                        ErrorMessage="Implementation Date is a required field." ValidationGroup="ProjectInfo"><</asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revNextImpDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                        ControlToValidate="txtNextImpDate" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                        Width="8px" ValidationGroup="ProjectInfo"><</asp:RegularExpressionValidator>
                                    <ajax:CalendarExtender ID="ceNextImpDate" runat="server" TargetControlID="txtNextImpDate"
                                        Animated="true" Format="MM/dd/yyyy" PopupButtonID="imgNextImpDate" />
                                    <%--<asp:CompareValidator ID="cvTNextImpDate" runat="server" ControlToValidate="txtNextImpDate"
                                        ControlToCompare="txtTodaysDate" ErrorMessage="Next Implementation Date must be greater than or equal to Implementation Date."
                                        Operator="GreaterThanEqual" Type="Date" ValidationGroup="ProjectInfo"><</asp:CompareValidator>--%>
                                </td>
                                <td>
                                    <asp:Label ID="lblReqImpDateChange" runat="server" Font-Bold="True" ForeColor="Red"
                                        Text="*" Visible="false" />
                                    <asp:Label ID="lblImpDateChange" runat="server" Text="Change Reason:" Visible="false"
                                        ForeColor="Red" />
                                    <asp:TextBox ID="txtImpDateChngRsn" Width="400px" runat="server" Visible="false" />
                                    <asp:RequiredFieldValidator ID="rfvImpDateChngRsn" runat="server" ErrorMessage="Implementation Date Change Reason is a required field."
                                        ControlToValidate="txtImpDateChngRsn" ValidationGroup="ProjectInfo"><</asp:RequiredFieldValidator><br />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label
                                        ID="lblImpDate" runat="server" Font-Bold="True" ForeColor="Red" /><asp:CompareValidator
                                            ID="cvImpDt2" runat="server" ControlToCompare="txtTodaysDate" ControlToValidate="txtNextImpDate"
                                            ErrorMessage="Implementation Date must be greater than or equal to Current Date."
                                            ValidationGroup="ProjectInfo" Operator="GreaterThanEqual" Type="Date"><</asp:CompareValidator>
                                </td>
                            </tr>
                            <% End If%>
                            <% If ViewState("pProjNo") > 0 And txtDateSubmitted.Text <> "" Then%>
                            <tr>
                                <td class="p_text" valign="top">
                                    Project Timeline:
                                </td>
                                <td>
                                    <table frame="vsides" style="border-left-color: Black; border-right-color: Black;
                                        border-top-color: White; border-bottom-color: White;">
                                        <tr style="height: 10px">
                                            <td style="width: 100px; height: 5px">
                                                <asp:TextBox ID="txtProjectTimeline" runat="server" BorderStyle="Ridge" ReadOnly="true" />
                                            </td>
                                        </tr>
                                    </table>
                                    &nbsp;<asp:Label ID="Label19" runat="server" Text="0" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label
                                        ID="Label23" runat="server" Text="100" />
                                </td>
                                <td>
                                </td>
                            </tr>
                            <% End If%>
                            <tr>
                                <td class="p_text" valign="top">
                                    Completion (%):
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCompletion" runat="server" Width="30" /><ajax:FilteredTextBoxExtender
                                        ID="fteSlider1" runat="server" TargetControlID="txtCompletion" FilterType="Numbers" />
                                    <asp:RangeValidator ID="rvSlider1" runat="server" ControlToValidate="txtCompletion"
                                        Display="Dynamic" ErrorMessage="Completion requires a numeric value 0 to 100"
                                        Height="16px" MaximumValue="100" MinimumValue="0" Type="Double" ValidationGroup="ProjectInfo"><</asp:RangeValidator>
                                    <table frame="vsides" style="border-left-color: Black; border-right-color: Black;
                                        border-top-color: White; border-bottom-color: White;">
                                        <tr style="height: 10px">
                                            <td style="width: 100px; height: 5px">
                                                <asp:TextBox ID="txtCompletionPercent" runat="server" BorderStyle="Ridge" ReadOnly="true" />
                                            </td>
                                        </tr>
                                    </table>
                                    &nbsp;<asp:Label ID="Label14" runat="server" Text="0" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label
                                        ID="Label15" runat="server" Text="100" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text" valign="top">
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:CheckBox runat="server" ID="cbOffsetsCostDowns" Text="Offsets Cost Downs" />
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text" valign="top">
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:CheckBox runat="server" ID="cbPlantControllerReviewed" Text="Reviewed By Plant Controller" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td colspan="2">
                                    <asp:Button ID="btnSave1" runat="server" Text="Save" CausesValidation="True" ValidationGroup="ProjectInfo" />
                                    <asp:Button ID="btnReset1" runat="server" Text="Reset" />
                                    <asp:Button ID="btnDelete" runat="server" Text="Delete" />
                                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CausesValidation="True" ValidationGroup="ProjectInfo" />
                                    <asp:Button ID="btnProposedDetails" runat="server" Text="Proposed Details" CausesValidation="True"
                                        ValidationGroup="ProjectInfo" />
                                    <asp:Button runat="server" ID="btnPreview" Text="Preview" />
                                    <asp:Button runat="server" ID="btnCopy" Text="Copy" />
                                    <br />
                                    <asp:Label ID="lblErrorsButtons" runat="server" SkinID="MessageLabelSkin" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td colspan="2">
                                    <asp:ValidationSummary ID="vsProjectDetail" runat="server" ShowMessageBox="True"
                                        ValidationGroup="ProjectInfo" />
                                </td>
                            </tr>
                        </table>
                    </Content>
                </ajax:AccordionPane>
            </Panes>
        </ajax:Accordion>
        <%-- Team Leader Status/Updates--%>
        <ajax:Accordion ID="accStatus" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
            FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
            RequireOpenedPane="false" SuppressHeaderPostbacks="true">
            <Panes>
                <ajax:AccordionPane ID="apStatus" runat="server">
                    <Header>
                        2. <a href="" class="accordionLink">Status/Updates</a></Header>
                    <Content>
                        <table>
                            <tr>
                                <td class="p_text" valign="top" style="width: 200px;">
                                    Status/Updates:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtStatus" runat="server" MaxLength="2000" Width="600px" Rows="8"
                                        TextMode="MultiLine"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvStatus" runat="server" ControlToValidate="txtStatus"
                                        ErrorMessage="Status/Updates is a required field." Font-Bold="False" ValidationGroup="StatusUpdates"><</asp:RequiredFieldValidator><br />
                                    <asp:Label ID="lblStatus" runat="server" Font-Bold="True" ForeColor="Red" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnSaveToGrid1" runat="server" Text="Save" CausesValidation="true"
                                        ValidationGroup="StatusUpdates" ToolTip="Please remember to update project's 'Completion (%)'."
                                        UseSubmitBehavior="true" OnClientClick="return confirm('Please remember to update projects Completion (%)');" />
                                    <asp:Button ID="btnReset2" runat="server" Text="Reset" />
                                </td>
                            </tr>
                        </table>
                        <asp:ValidationSummary ID="vsStatus" runat="server" ShowMessageBox="True" ValidationGroup="StatusUpdates" />
                        <br />
                        <asp:GridView ID="gvStatus" runat="server" DataSourceID="odsStatus" DataKeyNames="StatusID,ProjectNo"
                            OnRowDataBound="gvStatus_RowDataBound" OnRowCommand="gvStatus_RowCommand" AutoGenerateColumns="False"
                            Width="900px" PageSize="50" HorizontalAlign="Center">
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" Wrap="False" />
                            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#E2DED6" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="StatusID" HeaderText="StatusID" SortExpression="StatusID"
                                    Visible="False" />
                                <asp:TemplateField HeaderText="Date Entered" SortExpression="DateEntered">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lblStatus" runat="server" Font-Underline="true" Text='<%# Bind("DateEntered") %>'
                                            NavigateUrl='<%# "CostReduction.aspx?pStatusID=" & DataBinder.Eval (Container.DataItem,"StatusID").tostring & "&pProjNo=" & ViewState("pProjNo")%>'></asp:HyperLink>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Status" HeaderText="Status/Updates" SortExpression="Status"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                            ImageUrl="~/images/delete.jpg" AlternateText="Delete" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="30px" />
                                    <ItemStyle HorizontalAlign="Right" Width="30px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsStatus" runat="server" DeleteMethod="DeleteCostReductionStatus"
                            OldValuesParameterFormatString="original_{0}" SelectMethod="GetCostReductionStatus"
                            TypeName="CRStatusBLL">
                            <DeleteParameters>
                                <asp:Parameter Name="StatusID" Type="Int32" />
                                <asp:Parameter Name="ProjectNo" Type="Int32" />
                                <asp:Parameter Name="original_StatusID" Type="Int32" />
                                <asp:Parameter Name="original_ProjectNo" Type="Int32" />
                            </DeleteParameters>
                            <SelectParameters>
                                <asp:Parameter Name="StatusID" Type="Int32" />
                                <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="Int32" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </Content>
                </ajax:AccordionPane>
            </Panes>
        </ajax:Accordion>
        <%-- Team Member Steps/Comments --%>
        <ajax:Accordion ID="accSteps" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
            FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
            RequireOpenedPane="false" SuppressHeaderPostbacks="true">
            <Panes>
                <ajax:AccordionPane ID="apSteps" runat="server">
                    <Header>
                        3. <a href="" class="accordionLink">Steps/Comments</a></Header>
                    <Content>
                        <%-- Team Member Steps/Comments--%>
                        <table>
                            <tr>
                                <td class="p_text" style="width: 200px;">
                                    Team Member:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddTeamMember" runat="server" />
                                    <asp:RequiredFieldValidator ID="rfvTeamMember" runat="server" ControlToValidate="ddTeamMember"
                                        ErrorMessage="Team Member is a required field." Font-Bold="False" ValidationGroup="StepsComments"><</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="p_text" valign="top">
                                    Steps/Comments:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSteps" runat="server" MaxLength="2000" Width="600px" Rows="8"
                                        TextMode="MultiLine" />
                                    <asp:RequiredFieldValidator ID="rfvSteps" runat="server" ControlToValidate="txtSteps"
                                        ErrorMessage="Steps/Comments is a required field." Font-Bold="False" ValidationGroup="StepsComments"><</asp:RequiredFieldValidator><br />
                                    <asp:Label ID="lblSteps" runat="server" Font-Bold="True" ForeColor="Red" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnSaveToGrid2" runat="server" Text="Save" CausesValidation="true"
                                        ValidationGroup="StepsComments" />
                                    <asp:Button ID="btnReset3" runat="server" Text="Reset" />
                                </td>
                            </tr>
                        </table>
                        <asp:ValidationSummary ID="vsSteps" runat="server" ShowMessageBox="True" ValidationGroup="StepsComments" />
                        <br />
                        <asp:GridView ID="gvSteps" runat="server" DataSourceID="odsSteps" DataKeyNames="StepID,ProjectNo"
                            OnRowDataBound="gvSteps_RowDataBound" OnRowCommand="gvSteps_RowCommand" AutoGenerateColumns="False"
                            Width="900px" PageSize="50" HorizontalAlign="Center">
                            <EmptyDataRowStyle Wrap="False" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <EmptyDataRowStyle Wrap="False" />
                            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" Wrap="False" />
                            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#E2DED6" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="StepID" HeaderText="StepID" SortExpression="StepID" Visible="False" />
                                <asp:BoundField DataField="TeamMemberID" HeaderText="TeamMemberID" SortExpression="TeamMemberID"
                                    Visible="False" />
                                <asp:TemplateField HeaderText="Date Entered" SortExpression="DateEntered">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lblSteps" runat="server" Font-Underline="true" Text='<%# Bind("DateEntered") %>'
                                            NavigateUrl='<%# "CostReduction.aspx?pStepID=" & DataBinder.Eval (Container.DataItem,"StepID").tostring & "&pProjNo=" & ViewState("pProjNo")%>'></asp:HyperLink>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Team Member" SortExpression="TeamMemberID">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddTeamMember" runat="server" DataSource='<%# commonFunctions.GetTeamMember("") %>'
                                            DataValueField="TeamMemberID" DataTextField="TeamMemberName" SelectedValue='<% # Bind("TeamMemberID")%>' />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("TMName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="150px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Steps/Comments" SortExpression="StepsComments">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtSteps" runat="server" MaxLength="2000" Width="600px" Rows="8"
                                            TextMode="MultiLine" Text='<%# Bind("StepsComments") %>' />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("StepsComments") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                            ImageUrl="~/images/delete.jpg" AlternateText="Delete" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="30px" />
                                    <ItemStyle HorizontalAlign="Right" Width="30px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsSteps" runat="server" DeleteMethod="DeleteCostReductionSteps"
                            OldValuesParameterFormatString="original_{0}" SelectMethod="GetCostReductionSteps"
                            TypeName="CRStepsBLL">
                            <DeleteParameters>
                                <asp:Parameter Name="StepID" Type="Int32" />
                                <asp:Parameter Name="ProjectNo" Type="Int32" />
                                <asp:Parameter Name="original_StepID" Type="Int32" />
                                <asp:Parameter Name="original_ProjectNo" Type="Int32" />
                            </DeleteParameters>
                            <SelectParameters>
                                <asp:Parameter Name="StepID" Type="Int32" />
                                <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="Int32" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </Content>
                </ajax:AccordionPane>
            </Panes>
        </ajax:Accordion>
        <%-- Change Value History --%>
        <ajax:Accordion ID="accHistory" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
            FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
            RequireOpenedPane="false" SuppressHeaderPostbacks="true">
            <Panes>
                <ajax:AccordionPane ID="AccordionPane1" runat="server">
                    <Header>
                        4. <a href="" class="accordionLink">Change Value History</a></Header>
                    <Content>
                        <%-- Change Value History --%>
                        <asp:GridView ID="gvCRHistory" runat="server" AutoGenerateColumns="False" DataSourceID="odsCRHistory"
                            HorizontalAlign="Center" Width="1000px" AllowPaging="true" AllowSorting="true">
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" Wrap="False" />
                            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#E2DED6" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="ActionDate" HeaderText="Action Date" SortExpression="ActionDate"
                                    ItemStyle-Width="120px" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="TMName" HeaderText="Team Member" SortExpression="TMName"
                                    ItemStyle-Width="130px" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="FieldChange" HeaderText="Field Change" SortExpression="FieldChange"
                                    ItemStyle-Width="130px" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="PreviousValue" HeaderText="Previous Value" SortExpression="PreviousValue"
                                    ItemStyle-Width="100px" ItemStyle-HorizontalAlign="right" />
                                <asp:BoundField DataField="NewValue" HeaderText="New Value" SortExpression="NewValue"
                                    ItemStyle-Width="100px" ItemStyle-HorizontalAlign="right" />
                                <asp:BoundField DataField="ActionDesc" HeaderText="Change Reason" SortExpression="ActionDesc"
                                    HeaderStyle-HorizontalAlign="Left" />
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsCRHistory" runat="server" OldValuesParameterFormatString="original_{0}"
                            SelectMethod="GetCostReductionHistory" TypeName="CRHistoryBLL">
                            <SelectParameters>
                                <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="Int32" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </Content>
                </ajax:AccordionPane>
            </Panes>
        </ajax:Accordion>
    </asp:Panel>
</asp:Content>
