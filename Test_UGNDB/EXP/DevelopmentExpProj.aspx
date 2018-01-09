<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="DevelopmentExpProj.aspx.vb" Inherits="EXP_DevelopmentExpProj" Title="Untitled Page"
    MaintainScrollPositionOnPostback="True" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="1200px">
        <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
            Visible="False" CssClass="c_textbold" />
        <% If ViewState("pProjNo") <> Nothing Then%>
        <table>
            <tr>
                <td class="p_smalltextbold" style="color: #990000">
                    <% If ViewState("pPrntProjNo") = Nothing And (txtRoutingStatus.Text <> "N" And txtRoutingStatus.Text <> "S" And txtRoutingStatus.Text <> "T") Then%>
                    <asp:Button ID="btnAppend" runat="server" Text="Append" CausesValidation="False" />
                    &nbsp;To add a supplement that capture cost changes to the existing D#.
                    <%End If%><br />
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CausesValidation="False" />
                    &nbsp;To create a D# for new commodity on the same program.
                    <br />
                </td>
            </tr>
        </table>
        <%  End If%>
        <hr />
        <br />
        <table>
            <tr>
                <td class="p_text">
                    Project Number:
                </td>
                <td style="color: #990000;">
                    <asp:Label ID="lblProjectID" runat="server" Text="D0000??" CssClass="c_text" Font-Bold="True"
                        Font-Overline="False" Font-Underline="False" />
                    <asp:TextBox ID="txtProjectID" runat="server" MaxLength="15" Width="80px" />
                    <asp:TextBox ID="txtOrigProjectID" runat="server" MaxLength="15" Width="80px" Visible="false" />
                </td>
                <td class="p_text">
                    Project Title:
                </td>
                <td style="color: #990000;" colspan="3">
                    <asp:Label ID="lblProjectTitle" runat="server" Text="" CssClass="c_text" Font-Bold="True"
                        Font-Overline="False" Font-Underline="False" />
                    <asp:HiddenField ID="hfYear" runat="server" />
                    <asp:HiddenField ID="hfMake" runat="server" />
                    <asp:HiddenField ID="hfModel" runat="server" />
                    <asp:HiddenField ID="hfProgram" runat="server" />
                    <asp:HiddenField ID="hfPreDev" runat="server" />
                    <asp:HiddenField ID="hfCommodityID" runat="server" />
                </td>
            </tr>
            <%  If ViewState("Admin") = False And ViewState("pPrntProjNo") = Nothing Then%>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblReqYear" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                    &nbsp;<asp:Label ID="lblYear" runat="server" Text="Year:" Visible="false" />
                </td>
                <td>
                    <asp:TextBox ID="txtYear" runat="server" MaxLength="6" Width="60px" />
                    <ajax:FilteredTextBoxExtender ID="ftbYear" runat="server" TargetControlID="txtYear"
                        FilterType="Custom,Numbers" ValidChars="." />
                    <asp:RangeValidator ID="rvYear" runat="server" ErrorMessage="Year values must between Current Year to 2030.5"
                        ControlToValidate="txtYear" MinimumValue="2011" MaximumValue="2030.5" ValidationGroup="vsProjectDetail"><</asp:RangeValidator>
                </td>
                <td class="p_text">
                    <asp:Label ID="lblMake" runat="server" Text="Make:" Visible="false" />
                </td>
                <td style="font-size: smaller">
                    <asp:DropDownList ID="ddMakes" runat="server" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblModel" runat="server" Text="Model:" Visible="false" />
                </td>
                <td style="font-size: smaller;">
                    <asp:DropDownList ID="ddModel" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblReqProgram" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                    &nbsp;<asp:Label ID="lblProgram" runat="server" Text="Program:" Visible="false" />
                </td>
                <td style="font-size: smaller" colspan="5">
                    <asp:DropDownList ID="ddProgram" runat="server" />
                    <asp:RequiredFieldValidator ID="rfvProgram" runat="server" ControlToValidate="ddProgram"
                        ErrorMessage="Program is a required field." Font-Bold="False" ValidationGroup="vsProjectDetail">&lt;</asp:RequiredFieldValidator>
                </td>
            </tr>
            <%Else%>
            <tr>
                <td class="p_text">
                    &nbsp;Customer:
                </td>
                <td style="font-size: smaller" colspan="5">
                    <asp:Label ID="txtCustomer" runat="server" Text="" CssClass="c_text" Font-Bold="True"
                        Font-Overline="False" Font-Underline="False" />
                </td>
            </tr>
            <%End If%>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblReqPreDev" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                    &nbsp;Pre-Development:
                </td>
                <td>
                    <asp:Label ID="lblPreDev" runat="server" Text="" CssClass="c_text" Font-Bold="True"
                        Font-Overline="False" Font-Underline="False" />
                    <asp:DropDownList ID="ddPreDvp" runat="server">
                        <asp:ListItem Selected="True" Value="False">No</asp:ListItem>
                        <asp:ListItem Value="True">Yes</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvPreDvp" runat="server" ControlToValidate="ddPreDvp"
                        ErrorMessage="Pre-Development is a required field." Font-Bold="False" ValidationGroup="vsProjectDetail">&lt;</asp:RequiredFieldValidator>
                </td>
                <td class="p_text">
                    Commodity Classification:
                </td>
                <td>
                    <asp:Label ID="lblCClass" runat="server" Text="" CssClass="c_text" Font-Bold="True"
                        Font-Overline="False" Font-Underline="False" />
                    <asp:DropDownList ID="ddCClass" runat="server" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblReqCommodity" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                    &nbsp;Commodity:
                </td>
                <td>
                    <asp:Label ID="lblCommodity" runat="server" Text="" CssClass="c_text" Font-Bold="True"
                        Font-Overline="False" Font-Underline="False" />
                    <asp:DropDownList ID="ddCommodity" runat="server" />
                    <asp:RequiredFieldValidator ID="rfvCommodity" runat="server" ControlToValidate="ddCommodity"
                        ErrorMessage="Commodity is a required field." Font-Bold="False" ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="Label21" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                    &nbsp;Budgeted:
                </td>
                <td class="c_text">
                    <asp:DropDownList ID="ddBudgeted" runat="server">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Value="False">No</asp:ListItem>
                        <asp:ListItem Value="True">Yes</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvBudgeted" runat="server" ControlToValidate="ddBudgeted"
                        ErrorMessage="Budgeted is a required field." Font-Bold="False" ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                </td>
                <td class="p_text">
                    <asp:Label ID="lblSOP" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                    &nbsp;Vehicle SOP Date:
                </td>
                <td style="font-size: smaller">
                    <asp:TextBox ID="txtSOP" runat="server" MaxLength="12" Width="80px" />
                    <asp:ImageButton runat="server" ID="imgSOP" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                        CausesValidation="False" />
                    <ajax:CalendarExtender ID="cbeSOP" runat="server" TargetControlID="txtSOP" PopupButtonID="imgSOP"
                        Format="MM/dd/yyyy" />
                    <asp:RequiredFieldValidator ID="rfvSOP" runat="server" ControlToValidate="txtSOP"
                        ErrorMessage="Vehicle SOP Date is a required field." ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revSOP" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtSOP" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px" ValidationGroup="vsProjectDetail"><</asp:RegularExpressionValidator>
                    <asp:CompareValidator ID="cvSOP" runat="server" ControlToCompare="txtSOP" ControlToValidate="txtDateSubmitted"
                        ErrorMessage="Vehicle SOP Date must be greater than Current Date." Operator="LessThan"
                        Type="Date" ValidationGroup="vsProjectDetail"><</asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Project Status:
                </td>
                <td class="c_textbold" style="color: red;" colspan="5">
                    <asp:DropDownList ID="ddProjectStatus" runat="server" AutoPostBack="True">
                        <asp:ListItem Value="Open">New Project</asp:ListItem>
                        <asp:ListItem>Approved</asp:ListItem>
                        <asp:ListItem>Completed</asp:ListItem>
                        <asp:ListItem>Void</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddProjectStatus2" runat="server" AutoPostBack="True">
                        <asp:ListItem>In Process</asp:ListItem>
                        <asp:ListItem>Void</asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="txtRoutingStatus" runat="server" Visible="false" Width="1px" />
                    <asp:Label ID="lblRoutingStatusDesc" runat="server" Visible="False" Width="312px"></asp:Label>
                </td>
            </tr>
            <%--Display the following rows after 'D' is voided.--%>
            <tr>
                <td class="p_text" valign="top">
                    <asp:Label ID="lblReqVoidRsn" runat="server" Font-Bold="True" ForeColor="Red" Text="*"
                        Visible="false" />
                    <asp:Label ID="lblVoidRsn" runat="server" Text="Void Reason:" />
                </td>
                <td class="c_text" colspan="3">
                    <asp:TextBox ID="txtVoidReason" runat="server" MaxLength="300" Rows="3" TextMode="MultiLine"
                        Width="550px"></asp:TextBox><asp:RequiredFieldValidator ID="rfvVoidReason" runat="server"
                            ErrorMessage="Void Reason is a required field." ControlToValidate="txtVoidReason"
                            ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator><br />
                    <asp:Label ID="lblVoidReason" runat="server" Font-Bold="True" ForeColor="Red" />&nbsp;
                </td>
            </tr>
        </table>
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
        <ajax:CascadingDropDown ID="cddCClass" runat="server" TargetControlID="ddCClass"
            ParentControlID="ddProgram" Category="CommodityClassID" PromptText="Please select a Commodity Classification."
            LoadingText="[Loading Commodity Classification...]" ServicePath="~/WS/GeneralCDDService.asmx"
            ServiceMethod="GetCommodityClass" />
        <ajax:CascadingDropDown ID="cddCommodity" runat="server" TargetControlID="ddCommodity"
            ParentControlID="ddCClass" Category="CommodityID" PromptText="Please select a Commodity."
            LoadingText="[Loading Commodity...]" ServicePath="~/WS/GeneralCDDService.asmx"
            ServiceMethod="GetCommodityByClass" />
        <table border="0">
            <tr>
                <td>
                    <asp:Menu ID="mnuTabs" Height="30px" runat="server" Orientation="Horizontal" StaticEnableDefaultPopOutImage="false"
                        StaticMenuItemStyle-CssClass="tab" StaticSelectedStyle-CssClass="selectedTab"
                        CssClass="tabs">
                        <Items>
                            <asp:MenuItem Text="Project Detail" Value="0" ImageUrl="" Selected="true" />
                            <asp:MenuItem Text="Supporting Documents" Value="1" ImageUrl="" />
                            <asp:MenuItem Text="Expected Costs/Savings" Value="2" ImageUrl="" />
                            <asp:MenuItem Text="Approval Status" Value="3" ImageUrl="" />
                            <asp:MenuItem Text="Communication Board" Value="4" ImageUrl="" />
                        </Items>
                    </asp:Menu>
                </td>
            </tr>
        </table>
        <asp:MultiView ID="mvTabs" runat="server" Visible="true" ActiveViewIndex="0" EnableViewState="true">
            <asp:View ID="vwRequestInfoTab" runat="server">
                <table>
                    <%If ViewState("pPrntProjNo") <> Nothing Then%>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label24" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Originating Project Number:
                        </td>
                        <td class="c_text" style="color: #990000;">
                            <asp:Label ID="lblPrntProjNo" runat="server" Text="" />
                        </td>
                        <td class="p_text">
                            <asp:Label ID="Label26" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Originating Project Approved Date:
                        </td>
                        <td class="c_text" style="color: #990000;">
                            <asp:Label ID="lblPrntAppDate" runat="server" Text="" />
                        </td>
                    </tr>
                    <%End If%>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label19" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            &nbsp;Requested By:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddRequestedBy" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvRequestedBy" runat="server" ControlToValidate="ddRequestedBy"
                                ErrorMessage="Requested By is a required field." Font-Bold="False" ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                        </td>
                        <td class="p_text">
                            <asp:Label ID="Label20" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            &nbsp;Date Submitted:
                        </td>
                        <td>
                            <asp:TextBox ID="txtDateSubmitted" runat="server" Width="80px" MaxLength="10" />
                            <asp:ImageButton runat="server" ID="imgDateSubmitted" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                                CausesValidation="False" /><asp:RequiredFieldValidator ID="rfvDateSubmitted" runat="server"
                                    ControlToValidate="txtDateSubmitted" ErrorMessage="Date Submitted is a required field."
                                    ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revDateSubmitted" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                ControlToValidate="txtDateSubmitted" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                Width="8px"><</asp:RegularExpressionValidator>
                            <ajax:CalendarExtender ID="ceDateSub" runat="server" TargetControlID="txtDateSubmitted"
                                Format="MM/dd/yyyy" PopupButtonID="imgDateSubmitted" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label4" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            &nbsp;Project Leader:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddProjectLeader" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvProjectLeader" runat="server" ControlToValidate="ddProjectLeader"
                                ErrorMessage="Project Leader is a required field." Font-Bold="False" ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                        </td>
                        <td class="p_text" valign="top">
                            <asp:Label ID="Label18" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            &nbsp;Account Manager:
                        </td>
                        <td valign="top">
                            <asp:DropDownList ID="ddAccountManager" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvAcctMgr" runat="server" ControlToValidate="ddAccountManager"
                                ErrorMessage="Account Manager is a required field." Font-Bold="False" ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator><br />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblSampleProdDesc" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="* " />
                            &nbsp;UGN Location:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddUGNFacility" runat="server" AutoPostBack="true" />
                            <asp:RequiredFieldValidator ID="rfvUGNFacility" runat="server" ControlToValidate="ddUGNFacility"
                                ErrorMessage="UGN Location is a required field." Font-Bold="False" ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                        </td>
                        <td class="p_text">
                            <asp:Label ID="Label6" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            &nbsp;Department or Cost Center:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddDepartment" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvDCC" runat="server" ControlToValidate="ddDepartment"
                                ErrorMessage="Department or Cost Center is a required field." Font-Bold="False"
                                ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" valign="top">
                            <asp:Label ID="Label8" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            &nbsp;Description:
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtProjDateNotes" runat="server" MaxLength="2000" Rows="8" TextMode="MultiLine"
                                Width="600px" />
                            <asp:RequiredFieldValidator ID="rfvProjDateNotes" runat="server" ControlToValidate="txtProjDateNotes"
                                ErrorMessage="Description is a required field." Font-Bold="False" ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator><br />
                            <asp:Label ID="lblProjDateNotes" runat="server" Font-Bold="True" ForeColor="Red" />
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" valign="top">
                            <asp:Label ID="Label10" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Justification:
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtJustification" runat="server" MaxLength="2000" Rows="8" TextMode="MultiLine"
                                Width="600px" />
                            <asp:RequiredFieldValidator ID="rfvJustification" runat="server" ControlToValidate="txtJustification"
                                ErrorMessage="Justification/Analysis is a required field." Font-Bold="False"
                                ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator><br />
                            <asp:Label ID="lblJustification" runat="server" Font-Bold="True" ForeColor="Red" />&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:TextBox ID="txtHDEstCmpltDt" runat="server" Visible="False" Width="2px" />
                            <asp:Label ID="Label9" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Estimated Completion Date:
                        </td>
                        <td class="c_text" colspan="2">
                            <asp:TextBox ID="txtEstCmpltDt" runat="server" MaxLength="10" Width="80px" />
                            <asp:ImageButton ID="imgEstCmpltDt" runat="server" AlternateText="Click to show calendar"
                                CausesValidation="False" Height="19px" ImageAlign="Middle" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                Width="19px" />
                            <asp:RequiredFieldValidator ID="rfvEstCmpltDt" runat="server" ControlToValidate="txtEstCmpltDt"
                                ErrorMessage="Estimated Completion Date 1 is a required field." ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revEstCmpltDt" runat="server" ControlToValidate="txtEstCmpltDt"
                                ErrorMessage="Invalid Date Entry:  use &quot;mm/dd/yyyy&quot; or &quot;m/d/yyyy&quot; format "
                                Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                ValidationGroup="vsProjectDetail" Width="8px">&lt;</asp:RegularExpressionValidator>
                            <ajax:CalendarExtender ID="ceEstCmpltDt" runat="server" Format="MM/dd/yyyy" PopupButtonID="imgEstCmpltDt"
                                TargetControlID="txtEstCmpltDt" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label12" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Estimated Start Spend Date:
                        </td>
                        <td>
                            <asp:TextBox ID="txtEstSpendDt" runat="server" Width="80px" MaxLength="10" />
                            <asp:ImageButton runat="server" ID="imgEstSpendDt" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                                CausesValidation="False" />
                            <asp:RequiredFieldValidator ID="rfvEstSpendDt" runat="server" ControlToValidate="txtEstSpendDt"
                                ErrorMessage="Estimated Start Spend Date is a required field." ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revEstSpendDt" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                ControlToValidate="txtEstSpendDt" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                Width="8px"><</asp:RegularExpressionValidator>
                            <ajax:CalendarExtender ID="ceEstSpendDt" runat="server" TargetControlID="txtEstSpendDt"
                                Format="MM/dd/yyyy" PopupButtonID="imgEstSpendDt" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label7" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Estimated End Spend Date:
                        </td>
                        <td>
                            <asp:TextBox ID="txtEstEndSpendDt" runat="server" Width="80px" MaxLength="10"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="imgEstEndSpendDt" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                                CausesValidation="False" />
                            <asp:RequiredFieldValidator ID="rfvEstEndSpendDt" runat="server" ControlToValidate="txtEstEndSpendDt"
                                ErrorMessage="Estimated End Spend Date is a required field." ValidationGroup="vsProjectDetail"><</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revEstEndSpendDt" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                ControlToValidate="txtEstEndSpendDt" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                Width="8px"><</asp:RegularExpressionValidator>
                            <ajax:CalendarExtender ID="ceEstEndSpendDt" runat="server" TargetControlID="txtEstEndSpendDt"
                                Format="MM/dd/yyyy" PopupButtonID="imgEstEndSpendDt" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                        </td>
                        <td colspan="3">
                            <asp:Button ID="btnSave1" runat="server" Text="Save" CausesValidation="True" ValidationGroup="vsProjectDetail" />
                            <asp:Button ID="btnReset1" runat="server" Text="Reset" CausesValidation="False" />
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CausesValidation="False" />
                            <asp:Button ID="btnPreview" runat="server" Text="Preview" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:ValidationSummary ID="sProjectDetail" ValidationGroup="vsProjectDetail" runat="server"
                                ShowMessageBox="True" />
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vwSupportingDocument" runat="server">
                <asp:Panel ID="SDPanel" runat="server" CssClass="collapsePanelHeader" Width="800px">
                    <asp:Image ID="imgSD" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                        Height="12px" />&nbsp;
                    <asp:Label ID="lblSD" runat="server" Text="Label" CssClass="c_textbold">SUPPORTING DOCUMENT(S):</asp:Label>
                </asp:Panel>
                <asp:Panel ID="SDContentPanel" runat="server" CssClass="collapsePanel" Width="800px">
                    <asp:Label ID="lblSupDoc" runat="server" CssClass="p_smalltextbold" Style="width: 800px;
                        color: #990000" Text="This section is used to include quotes, project activity log and/or other pertinent documentation. *.PDF, *.DOC and *.XLS files are allowed for upload up to 4MB each." /><br />
                    <asp:Label ID="lblSupDoc2" runat="server" CssClass="p_smalltextbold" Style="width: 800px;
                        color: #990000" Text="NOTE: Please be sure to upload the latest copy of any document. Any changes you make will not be saved to the upload files. Please be sure to make a copy of the file locally and upload a new version. You have the option to delete or keep previous version of the file for reference. Please use the 'File Description' area to comment on the changes you make." />
                    <table>
                        <%-- <tr>
                            <td class="p_text">
                                Upload By:
                            </td>
                            <td class="c_text">
                                <asp:DropDownList ID="ddTeamMember" runat="server" />
                                <asp:RequiredFieldValidator ID="rfvTeamMember" runat="server" ControlToValidate="ddTeamMember"
                                    ErrorMessage="Team Member is a required field." Font-Bold="False" ValidationGroup="vsSupportingDocuments"><</asp:RequiredFieldValidator>
                            </td>
                        </tr>--%>
                        <tr>
                            <td class="p_text" valign="top">
                                File Description:
                            </td>
                            <td class="c_text">
                                <asp:TextBox ID="txtFileDesc" runat="server" MaxLength="200" Rows="3" TextMode="MultiLine"
                                    Width="600px" />
                                <asp:RequiredFieldValidator ID="rfvFileDesc" runat="server" ControlToValidate="txtFileDesc"
                                    ErrorMessage="File Description is a required field." Font-Bold="False" ValidationGroup="vsSupportingDocuments"><</asp:RequiredFieldValidator><br />
                                <asp:Label ID="lblFileDesc" runat="server" Font-Bold="True" ForeColor="Red" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text" valign="top">
                                Supporting Document:
                            </td>
                            <td class="c_text">
                                <asp:FileUpload ID="uploadFile" runat="server" Height="22px" Width="600px" />
                                <asp:RequiredFieldValidator ID="rfvUpload" runat="server" ControlToValidate="uploadFile"
                                    ErrorMessage="Supporting Document is required." Font-Bold="False" ValidationGroup="vsSupportingDocuments"><</asp:RequiredFieldValidator><br />
                                <asp:RegularExpressionValidator ID="revUploadFile" runat="server" ErrorMessage="Only *.PDF, *.DOC, *.DOCX, *.XLS, *.XLSX files are allowed!"
                                    ValidationExpression="(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.pdf|.xls|.doc|.xlsx|.docx|.PDF|.XLS|.DOC|.XLSX|.DOCX)$"
                                    ControlToValidate="uploadFile" ValidationGroup="vsSupportingDocuments" Font-Bold="True"
                                    Font-Size="Small" /><br />
                                <asp:Label ID="lblMessageView4" runat="server" Font-Bold="True" ForeColor="Red" Height="16px"
                                    Text="Label" Visible="False" Width="368px" Font-Size="Small"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 27px">
                            </td>
                            <td style="height: 27px">
                                <asp:Button ID="btnUpload" runat="server" Text="Upload" CausesValidation="true" ValidationGroup="vsSupportingDocuments" />
                                <asp:Button ID="btnReset3" runat="server" CausesValidation="False" Text="Reset" />
                            </td>
                        </tr>
                    </table>
                    <asp:ValidationSummary ID="vsSupDoc" runat="server" ValidationGroup="vsSupportingDocuments"
                        ShowMessageBox="true" ShowSummary="true" />
                </asp:Panel>
                <ajax:CollapsiblePanelExtender ID="SDExtender" runat="server" TargetControlID="SDContentPanel"
                    ExpandControlID="SDPanel" CollapseControlID="SDPanel" Collapsed="FALSE" TextLabelID="lblSD"
                    ExpandedText="SUPPORTING DOCUMENT(S):" CollapsedText="SUPPORTING DOCUMENT(S):"
                    ImageControlID="imgSD" CollapsedImage="~/images/expand_blue.jpg" ExpandedImage="~/images/collapse_blue.jpg"
                    SuppressPostBack="true">
                </ajax:CollapsiblePanelExtender>
                <br />
                <asp:GridView ID="gvSupportingDocument" runat="server" AutoGenerateColumns="False"
                    DataKeyNames="ProjectNo,DocID" DataSourceID="odsSupportingDocument" Width="900px"
                    AllowSorting="True" SkinID="StandardGridWOFooter">
                    <Columns>
                        <asp:BoundField DataField="Description" HeaderText="File Description" SortExpression="Description">
                            <HeaderStyle HorizontalAlign="Left" Width="400px" />
                            <ItemStyle Width="400px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="comboUploadBy" HeaderText="Uploaded By" SortExpression="comboUploadBy">
                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                            <ItemStyle Width="150px" />
                        </asp:BoundField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="hlnkPreview" ImageUrl='<%# DisplayImage(DataBinder.Eval(Container, "DataItem.EncodeType").tostring)  %>'
                                    NavigateUrl='<%# "DevelopmentExpProjDocument.aspx?pProjNo=" & DataBinder.Eval (Container.DataItem,"ProjectNo").tostring & "&pDocID=" & DataBinder.Eval (Container.DataItem,"DocID").tostring %>'
                                    Target="_blank" Visible='<%# Bind("BinaryFound") %>' ToolTip="Preview Document" />
                            </ItemTemplate>
                            <HeaderStyle Width="30px" />
                            <ItemStyle HorizontalAlign="Center" Width="30px" />
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
                <asp:ObjectDataSource ID="odsSupportingDocument" runat="server" DeleteMethod="DeleteExpProjDevelopmentDocuments"
                    OldValuesParameterFormatString="original_{0}" SelectMethod="GetExpProjDevelopmentDocuments"
                    TypeName="ExpProjDevelopmentBLL">
                    <DeleteParameters>
                        <asp:Parameter Name="ProjectNo" Type="String" />
                        <asp:Parameter Name="Original_DocID" Type="Int32" />
                        <asp:Parameter Name="Original_ProjectNo" Type="String" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="String" />
                        <asp:Parameter Name="DocID" Type="Int32" DefaultValue="0" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </asp:View>
            <asp:View ID="vwExpense" runat="server">
                <table>
                    <tr>
                        <td colspan="2" class="c_textbold">
                            COSTS:
                        </td>
                    </tr>
                    <tr>
                        <td class="c_text">
                            Materials
                            <asp:Label ID="lblMaterials" runat="server" Font-Size="Smaller" Font-Italic="true"
                                Text="(Material to be used in manufacturing process)" />
                        </td>
                        <td>
                            ($):
                            <asp:TextBox ID="txtMaterials" runat="server" MaxLength="20" Width="100px" Text="0.00" />
                            <ajax:FilteredTextBoxExtender ID="ftbeMaterials" runat="server" TargetControlID="txtMaterials"
                                FilterType="Custom, Numbers" ValidChars="-." />
                        </td>
                    </tr>
                    <tr>
                        <td class="c_text">
                            Labor and OH
                            <asp:Label ID="lblLaborOH" runat="server" Font-Size="Smaller" Font-Italic="true"
                                Text="(Direct and Indirect Labor with associated OH rate)" />
                        </td>
                        <td>
                            ($):
                            <asp:TextBox ID="txtLaborOH" runat="server" MaxLength="20" Width="100px" Text="0.00" />
                            <ajax:FilteredTextBoxExtender ID="ftbeLaborOH" runat="server" TargetControlID="txtLaborOH"
                                FilterType="Custom, Numbers" ValidChars="-." />
                        </td>
                    </tr>
                    <tr>
                        <td class="c_text">
                            Packaging
                        </td>
                        <td>
                            ($):
                            <asp:TextBox ID="txtPackaging" runat="server" MaxLength="20" Width="100px" Text="0.00" />
                            <ajax:FilteredTextBoxExtender ID="ftbePackaging" runat="server" TargetControlID="txtPackaging"
                                FilterType="Custom, Numbers" ValidChars="-." />
                        </td>
                    </tr>
                    <tr>
                        <td class="c_text">
                            Freight
                            <asp:Label ID="lblFreight" runat="server" Font-Size="Smaller" Font-Italic="true"
                                Text="(Costs to ship sample or test material, not inbound freight)" />
                        </td>
                        <td>
                            ($):
                            <asp:TextBox ID="txtFreight" runat="server" MaxLength="20" Width="100px" Text="0.00" />
                            <ajax:FilteredTextBoxExtender ID="ftbeFreight" runat="server" TargetControlID="txtFreight"
                                FilterType="Custom, Numbers" ValidChars="-." />
                        </td>
                    </tr>
                    <tr>
                        <td class="c_text">
                            Travel Expenditures
                            <asp:Label ID="lblTravelExpenditures" runat="server" Font-Size="Smaller" Font-Italic="true"
                                Text="(Costs of related travel expense)" />
                        </td>
                        <td>
                            ($):
                            <asp:TextBox ID="txtTravelExpenditures" runat="server" MaxLength="20" Width="100px"
                                Text="0.00" />
                            <ajax:FilteredTextBoxExtender ID="ftbeTravelExpenditures" runat="server" TargetControlID="txtTravelExpenditures"
                                FilterType="Custom, Numbers" ValidChars="-." />
                        </td>
                    </tr>
                    <tr>
                        <td class="c_text">
                            Nittoku or UGN Design Charges
                        </td>
                        <td>
                            ($):
                            <asp:TextBox ID="txtNITUGN" runat="server" MaxLength="20" Width="100px" Text="0.00" />
                            <ajax:FilteredTextBoxExtender ID="ftbeNITUGN" runat="server" TargetControlID="txtNITUGN"
                                FilterType="Custom, Numbers" ValidChars="-." />
                        </td>
                    </tr>
                    <tr>
                        <td class="c_text">
                            Farmington Acoustic Testing Charges
                            <asp:Label ID="lblReiter" runat="server" Font-Size="Smaller" Font-Italic="true" Text="(Reiter Lab)" />
                        </td>
                        <td>
                            ($):
                            <asp:TextBox ID="txtReiter" runat="server" MaxLength="20" Width="100px" Text="0.00" />
                            <ajax:FilteredTextBoxExtender ID="ftbeReiter" runat="server" TargetControlID="txtReiter"
                                FilterType="Custom, Numbers" ValidChars="-." />
                        </td>
                    </tr>
                    <tr>
                        <td class="c_text">
                            Other Testing charges/related expenses<br />
                            <asp:Label ID="lblOtherTesting" runat="server" Font-Size="Smaller" Font-Italic="true"
                                Text="(Perishable supplies, Tooling modifications, Plant rearrangement, etc...)" />
                        </td>
                        <td>
                            ($):
                            <asp:TextBox ID="txtOtherTesting" runat="server" MaxLength="20" Width="100px" Text="0.00" />
                            <ajax:FilteredTextBoxExtender ID="ftbeOtherTesting" runat="server" TargetControlID="txtOtherTesting"
                                FilterType="Custom, Numbers" ValidChars="-." />
                        </td>
                    </tr>
                    <tr>
                        <td class="c_textbold">
                            Total Request
                        </td>
                        <td class="c_text" style="width: 243px; color: #990000;">
                            <b>($):
                                <asp:Label ID="lblTotalRequest" runat="server" Text="0.00" /></b>
                        </td>
                    </tr>
                    <tr>
                        <td class="c_text">
                            Expected Customer Reimbursement
                        </td>
                        <td>
                            ($):
                            <asp:TextBox ID="txtCustReimb" runat="server" MaxLength="20" Width="100px"></asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="ftbeCustReimb" runat="server" TargetControlID="txtCustReimb"
                                FilterType="Custom, Numbers" ValidChars="-." />
                        </td>
                    </tr>
                    <tr>
                        <td class="c_textbold">
                            Total Investment
                        </td>
                        <td class="c_text" style="width: 243px; color: #990000;">
                            <b>($):
                                <asp:Label ID="lblTotalInvestment1" runat="server" Text="0.00"></asp:Label></b>
                            <asp:TextBox ID="txtHDTotalInvestment" runat="server" Visible="False" Width="20px" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="c_textbold" colspan="2">
                            General Notes: <i>Please provide additional pertinent information.</i>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:TextBox ID="txtGeneralNotes" runat="server" Rows="4" TextMode="MultiLine" Width="600px"
                                MaxLength="2000" /><br />
                            <asp:Label ID="lblGeneralNotes" runat="server" Font-Bold="True" ForeColor="Red" />
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <br />
                <table>
                    <tr>
                        <td colspan="2" class="c_textbold">
                            SAVINGS:
                        </td>
                    </tr>
                    <tr>
                        <td class="c_text">
                            Cost Reduction Reference:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddCRProjNo" runat="server" AutoPostBack="True" />
                            <asp:Button ID="btnCRProjNoReq" runat="server" Text="Request" />
                            <asp:CheckBox ID="cbCRProjNoReq" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="c_text">
                            Development Savings
                        </td>
                        <td>
                            ($):
                            <asp:TextBox ID="txtDevSavings" runat="server" Width="100px" MaxLength="12">0.00</asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="ftbDevSavings" runat="server" TargetControlID="txtDevSavings"
                                FilterType="Custom, Numbers" ValidChars="-." />
                        </td>
                    </tr>
                    <tr>
                        <td class="c_text">
                            Scrap Savings
                        </td>
                        <td>
                            ($):
                            <asp:TextBox ID="txtScrapSavings" runat="server" Width="100px" MaxLength="12">0.00</asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="ftbScrapSavings" runat="server" TargetControlID="txtScrapSavings"
                                FilterType="Custom, Numbers" ValidChars="-." />
                        </td>
                    </tr>
                    <tr>
                        <td class="c_text">
                            Consumable Savings
                        </td>
                        <td>
                            ($):
                            <asp:TextBox ID="txtConsumableSavings" runat="server" Width="100px" MaxLength="12">0.00</asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="ftbConsumableSavings" runat="server" TargetControlID="txtConsumableSavings"
                                FilterType="Custom, Numbers" ValidChars="-." />
                        </td>
                    </tr>
                    <tr>
                        <td class="c_text">
                            Labor Savings
                        </td>
                        <td>
                            ($):
                            <asp:TextBox ID="txtLaborSavings" runat="server" Width="100px" MaxLength="12">0.00</asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="ftbLaborSavings" runat="server" TargetControlID="txtLaborSavings"
                                FilterType="Custom, Numbers" ValidChars="-." />
                        </td>
                    </tr>
                    <tr>
                        <td class="c_text">
                            Other Savings
                        </td>
                        <td>
                            ($):
                            <asp:TextBox ID="txtOtherSavings" runat="server" Width="100px" MaxLength="12">0.00</asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="ftbOtherSavings" runat="server" TargetControlID="txtOtherSavings"
                                FilterType="Custom, Numbers" ValidChars="-." />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnExpenditure" runat="server" Text="Save" CommandName="Expenditure"
                                ValidationGroup="vsDevelopmentExpense" />
                            <asp:Button ID="btnReset2" runat="server" Text="Reset" CausesValidation="False" />
                        </td>
                    </tr>
                </table>
                <asp:ValidationSummary ID="vsDevelopmentExpense" runat="server" Font-Size="X-Small"
                    ShowMessageBox="True" ShowSummary="true" ValidationGroup="vsDevelopmentExpense" />
            </asp:View>
            <asp:View ID="vwApprovalStatus" runat="server">
                <br />
                <asp:Label ID="lblReqAppComments" runat="server" Visible="false" CssClass="c_text"
                    Font-Bold="true" />
                <asp:GridView ID="gvApprovers" runat="server" AutoGenerateColumns="False" DataKeyNames="ProjectNo,SeqNo,OrigTeamMemberID,TeamMemberID"
                    OnRowUpdating="gvApprovers_RowUpdating" OnRowDataBound="gvApprovers_RowDataBound"
                    OnRowCommand="gvApprovers_RowCommand" DataSourceID="odsApprovers" Width="1000px"
                    RowStyle-Height="20px" RowStyle-CssClass="c_text" HeaderStyle-CssClass="c_text"
                    SkinID="StandardGrid">
                    <RowStyle CssClass="c_text" Height="20px" />
                    <Columns>
                        <asp:TemplateField HeaderText="Approval Level" SortExpression="SeqNo">
                            <EditItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("SeqNo") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("SeqNo") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="50px" />
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:Label ID="lblMsg1" runat="server" Text="1" Font-Italic="true" ForeColor="Black" />
                            </FooterTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="OrigTeamMemberName" HeaderText="Original Team Member"
                            SortExpression="OrigTeamMemberName" Visible="False">
                            <HeaderStyle HorizontalAlign="Left" Width="140px" Wrap="True" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Assigned Team Member" SortExpression="TeamMemberName">
                            <EditItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("TeamMemberName") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("TeamMemberName") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddResponsibleTM" runat="server" DataSource='<%# commonFunctions.GetTeamMemberbySubscription(92) %>'
                                    DataValueField="TMID" DataTextField="TMName" SelectedValue='<%# Bind("TMID") %>'
                                    AppendDataBoundItems="true">
                                    <asp:ListItem Selected="True">
                                    </asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvResposibleTM" runat="server" ControlToValidate="ddResponsibleTM"
                                    ErrorMessage="Assigned Team Member is a required field." Font-Bold="True" ValidationGroup="InsertApprovalInfo"><</asp:RequiredFieldValidator>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="150px" Wrap="True" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date Notified" SortExpression="DateNotified">
                            <EditItemTemplate>
                                <asp:Label ID="txtDateNotified" runat="server" Text='<%# Bind("DateNotified") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDateNotified" runat="server" Text='<%# Bind("DateNotified") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="80px" />
                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status" SortExpression="Status">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddStatus" runat="server" SelectedValue='<%# Bind("Status") %>'>
                                    <asp:ListItem>Pending</asp:ListItem>
                                    <asp:ListItem>Approved</asp:ListItem>
                                    <asp:ListItem>Rejected</asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="70px" />
                            <ItemStyle HorizontalAlign="Center" Width="70px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="DateSigned" HeaderText="Date Signed" SortExpression="DateSigned"
                            ReadOnly="True">
                            <HeaderStyle HorizontalAlign="Center" Width="80px" />
                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Comments" SortExpression="Comments">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtAppComments" runat="server" MaxLength="200" Rows="2" TextMode="MultiLine"
                                    Text='<%# Bind("Comments") %>' Width="300px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvComments" runat="server" ControlToValidate="txtAppComments"
                                    ErrorMessage="Comments is a required field when approving for another team member."
                                    Font-Bold="True" ValidationGroup="EditApprovalInfo"><</asp:RequiredFieldValidator><asp:TextBox
                                        ID="txtTeamMemberID" runat="server" Text='<%# Eval("TeamMemberID") %>' ReadOnly="true"
                                        Width="0px" Visible="false" />
                                <asp:TextBox ID="txtOrigTeamMemberID" runat="server" Text='<%# Eval("OrigTeamMemberID") %>'
                                    ReadOnly="true" Width="0px" Visible="false" /><asp:TextBox ID="hfSeqNo" runat="server"
                                        Text='<%# Eval("SeqNo") %>' ReadOnly="true" Width="0px" Visible="false" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Comments") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="400px" />
                            <ItemStyle Wrap="True" />
                            <FooterTemplate>
                                <asp:Label ID="lblMsg2" runat="server" Text="<< Use this row to add another TM for approval, when required. >>"
                                    Font-Italic="true" ForeColor="Black" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <EditItemTemplate>
                                <asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                    ImageUrl="~/images/save.jpg" ToolTip="Save" ValidationGroup="EditApprovalInfo" />&nbsp;&nbsp;&nbsp;
                                <asp:ImageButton ID="ibtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                    ImageUrl="~/images/cancel.jpg" ToolTip="Cancel" ValidationGroup="EditApprovalInfo" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                    ToolTip="Edit" ImageUrl="~/images/edit.jpg" ValidationGroup="EditApprovalInfo" />&nbsp;&nbsp;&nbsp;
                            </ItemTemplate>
                            <ItemStyle Width="60px" />
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <FooterTemplate>
                                <asp:ImageButton ID="btnInsert" runat="server" CausesValidation="true" ValidationGroup="InsertApprovalInfo"
                                    CommandName="Insert" ToolTip="Insert" ImageUrl="~/images/save.jpg" />
                                <asp:ImageButton ID="ibtnUndo" runat="server" CausesValidation="False" CommandName="Undo"
                                    ImageUrl="~/images/undo-gray.jpg" ToolTip="Cancel" ValidationGroup="InsertApprovalInfo" />
                            </FooterTemplate>
                            <ItemStyle Width="60px" HorizontalAlign="Center" />
                            <FooterStyle Width="60px" />
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                    ToolTip="Delete" ImageUrl="~/images/delete.jpg" />
                            </ItemTemplate>
                            <ItemStyle Width="30px" HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="c_text" />
                </asp:GridView>
                <asp:ValidationSummary ID="vsEditApprovalInfo" runat="server" ShowMessageBox="True"
                    ValidationGroup="EditApprovalInfo" />
                <asp:ValidationSummary ID="vsInsertApprovalInfo" runat="server" ShowMessageBox="True"
                    ValidationGroup="InsertApprovalInfo" />
                <asp:ObjectDataSource ID="odsApprovers" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetExpProjDevelopmentApproval" TypeName="ExpProjDevelopmentBLL"
                    UpdateMethod="UpdateExpProjDevelopmentApproval" DeleteMethod="DeleteExpProjDevelopmentApproval"
                    InsertMethod="InsertExpProjDevelopmentAddLvl1Aprvl">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="String" />
                        <asp:Parameter DefaultValue="0" Name="Sequence" Type="Int32" />
                    </SelectParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="Status" Type="String" />
                        <asp:Parameter Name="Comments" Type="String" />
                        <asp:Parameter Name="SameTMID" Type="Boolean" />
                        <asp:Parameter Name="original_ProjectNo" Type="String" />
                        <asp:Parameter Name="original_SeqNo" Type="Int32" />
                        <asp:Parameter Name="original_TeamMemberID" Type="Int32" />
                        <asp:Parameter Name="original_OrigTeamMemberID" Type="Int32" />
                        <asp:Parameter Name="TeamMemberName" Type="String" />
                        <asp:Parameter Name="SeqNo" Type="Int32" />
                        <asp:Parameter Name="DateNotified" Type="String" />
                    </UpdateParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="ProjectNo" Type="String" />
                        <asp:Parameter Name="SeqNo" Type="Int32" />
                        <asp:Parameter Name="ResponsibleTMID" Type="Int32" />
                        <asp:Parameter Name="original_ProjectNo" Type="String" />
                        <asp:Parameter Name="original_SeqNo" Type="Int32" />
                        <asp:Parameter Name="original_TeamMemberID" Type="Int32" />
                        <asp:Parameter Name="original_OrigTeamMemberID" Type="Int32" />
                    </DeleteParameters>
                    <InsertParameters>
                        <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="String" />
                        <asp:Parameter Name="SeqNo" Type="Int32" />
                        <asp:Parameter Name="ResponsibleTMID" Type="Int32" />
                        <asp:Parameter Name="OriginalTMID" Type="Int32" />
                    </InsertParameters>
                </asp:ObjectDataSource>
                <br />
                <table>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnBuildApproval" runat="server" CausesValidation="False" Text="Build Approval List" />&nbsp;<asp:Button
                                ID="btnFwdApproval" runat="server" CausesValidation="False" Text="Submit for Approval"
                                Width="130px" />
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vwCommunicationBoard" runat="server">
                <asp:Label ID="lblSQC" runat="server" CssClass="p_smalltextbold" Style="width: 532px;
                    color: #990000" Text="Select a 'Question / Comment' from discussion thread below to respond." />
                <table>
                    <tr>
                        <td class="p_text" valign="top">
                            Question / Comment:
                        </td>
                        <td class="c_text">
                            <asp:TextBox ID="txtQC" runat="server" Font-Bold="True" Rows="3" TextMode="MultiLine"
                                Width="550px" ReadOnly="true" />
                            <asp:RequiredFieldValidator ID="rfvQC" runat="server" ErrorMessage="Select a Question / Comment from table below for response."
                                ValidationGroup="ReplyComments" ControlToValidate="txtQC"><</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" valign="top">
                            <asp:Label ID="lblReqReply" runat="server" Text="*" ForeColor="Red" />
                            Reply / Comments:
                        </td>
                        <td>
                            <asp:TextBox ID="txtReply" runat="server" Rows="3" TextMode="MultiLine" Width="550px" />
                            <asp:RequiredFieldValidator ID="rfvReply" runat="server" ErrorMessage="Reply / Comments is a required field."
                                ValidationGroup="ReplyComments" ControlToValidate="txtReply"><</asp:RequiredFieldValidator><br />
                            <asp:Label ID="lblReply" runat="server" Font-Bold="True" ForeColor="Red" />&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 26px">
                        </td>
                        <td style="height: 26px">
                            <asp:Button ID="btnSave2" runat="server" Text="Submit" CausesValidation="true" ValidationGroup="ReplyComments" />
                            <asp:Button ID="btnReset4" runat="server" Text="Reset" CausesValidation="False" />
                        </td>
                    </tr>
                </table>
                <asp:ValidationSummary ID="vsReplyComments" runat="server" ValidationGroup="ReplyComments"
                    ShowMessageBox="true" ShowSummary="true" />
                <br />
                <asp:GridView ID="gvQuestion" runat="server" AutoGenerateColumns="False" DataSourceID="odsQuestion"
                    OnRowDataBound="gvQuestion_RowDataBound" Width="900px" RowStyle-BorderStyle="None"
                    SkinID="CommBoardRSS">
                    <Columns>
                        <asp:TemplateField HeaderText="Reply">
                            <ItemTemplate>
                                <%--  <% If ViewState("Admin") = "true" Then%>--%>
                                <asp:HyperLink ID="lnkAppend" runat="server" ImageUrl="~/images/messanger30.jpg"
                                    ToolTip="Reply" NavigateUrl='<%# GoToCommunicationBoard(DataBinder.Eval(Container, "DataItem.ProjectNo"),DataBinder.Eval(Container, "DataItem.RSSID"),DataBinder.Eval(Container, "DataItem.ApprovalLevel"),DataBinder.Eval(Container, "DataItem.TeamMemberID")) %>' />
                                <%-- <%Else%>
                                <asp:HyperLink ID="HyperLink1" runat="server" ImageUrl="~/images/messanger30.jpg" />
                                <% End If%>--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="RSSID" HeaderText="RSSID" SortExpression="RSSID" Visible="False" />
                        <asp:BoundField DataField="Comments" HeaderText="Question / Comment" SortExpression="Comments">
                            <HeaderStyle Width="500px" />
                            <ItemStyle CssClass="c_text" Font-Bold="True" Width="500px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TeamMemberName" HeaderText="Submitted By" SortExpression="TeamMemberName">
                            <HeaderStyle Width="100px" />
                            <ItemStyle Font-Bold="True" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PostDate" HeaderText="Post Date" SortExpression="PostDate">
                            <ItemStyle Font-Bold="True" />
                        </asp:BoundField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                    </td>
                                    <td colspan="3">
                                        <asp:GridView ID="gvReply" runat="server" AutoGenerateColumns="False" DataSourceID="odsReply"
                                            DataKeyNames="ProjectNo,RSSID" Width="100%" SkinID="CommBoardResponse">
                                            <Columns>
                                                <asp:BoundField DataField="Comments" HeaderText="Response" SortExpression="Comments"
                                                    HeaderStyle-Width="500px" ItemStyle-Width="500px" ItemStyle-Font-Bold="true" />
                                                <asp:BoundField DataField="TeamMemberName" HeaderText="" SortExpression="TeamMemberName"
                                                    HeaderStyle-Width="100px" ItemStyle-Width="100px" />
                                                <asp:BoundField DataField="PostDate" HeaderText="" SortExpression="PostDate" />
                                            </Columns>
                                        </asp:GridView>
                                        <asp:ObjectDataSource ID="odsReply" runat="server" OldValuesParameterFormatString="original_{0}"
                                            SelectMethod="GetExpProjDevelopmentRSSReply" TypeName="ExpProjDevelopmentBLL">
                                            <SelectParameters>
                                                <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="String" />
                                                <asp:Parameter Name="RSSID" Type="Int32" DefaultValue="0" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsQuestion" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetExpProjDevelopmentRSS" TypeName="ExpProjDevelopmentBLL">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="String" />
                        <asp:Parameter Name="RSSID" Type="Int32" DefaultValue="0" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </asp:View>
        </asp:MultiView>
    </asp:Panel>
</asp:Content>
