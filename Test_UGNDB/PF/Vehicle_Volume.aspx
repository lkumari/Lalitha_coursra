<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Vehicle_Volume.aspx.vb" Inherits="PMT_Vehicle" Title="Vehicle Volume"
    MaintainScrollPositionOnPostback="True" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server">
        <asp:Label ID="lblErrors" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="Red"
            Text="" Visible="False" />
        <%  If HttpContext.Current.Request.QueryString("sPGMID") <> "" And HttpContext.Current.Request.QueryString("sPGMID") <> Nothing Then%>
        <table>
            <tr>
                <td class="p_smalltextbold" style="width: 532px; color: #990000">
                    Edit data below or press
                    <asp:Button ID="btnAdd" runat="server" Text="Add" />
                    to enter new data.&nbsp;
                    <asp:Button ID="btnCopy" runat="server" Text="Copy" Visible="false" />
                </td>
            </tr>
        </table>
        <% End If%>
        <hr />
        <br />
        <% If ViewState("Admin") = True Then%>
        <asp:Panel ID="AEPanel" runat="server" CssClass="collapsePanelHeader" Width="100%">
            <asp:Image ID="imgAE" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                Height="12px" />&nbsp;
            <asp:Label ID="lblAE" runat="server" Text="Label" CssClass="c_textbold">IHS PLATFORM, PROGRAM, MODEL, ASSEMBLY AND MONTHLY VOLUME INFORMATION... </asp:Label>
        </asp:Panel>
        <asp:Panel ID="AEContentPanel" runat="server" CssClass="collapsePanel" Width="100%">
            <table style="width: 100%; border-bottom-style: groove;" class="sampleStyleC">
                <tr>
                    <td class="p_textbold" style="color: red">
                        Platform:&nbsp;
                    </td>
                    <td class="c_textbold">
                        <asp:Label ID="lblPlatformName" runat="server" Text="" />
                    </td>
                    <td class="p_text">
                        OEM Manufacturer:&nbsp;
                    </td>
                    <td class="c_textbold">
                        <asp:Label ID="lblOEM" runat="server" Text="" />
                    </td>
                    <td class="p_text">
                        UGN Business:&nbsp;
                    </td>
                    <td class="c_textbold">
                        <asp:Label ID="lblUGNBusiness" runat="server" Text="" />
                    </td>
                    <td class="p_text">
                        Current Platform:&nbsp;
                    </td>
                    <td class="c_textbold">
                        <asp:Label ID="lblCurrentPlatform" runat="server" Text="" />
                    </td>
                    <td class="p_textbold" style="color: red">
                        Beginning Year:&nbsp;
                    </td>
                    <td class="c_textbold">
                        <asp:Label ID="lblBegYear" runat="server" Text="" />
                    </td>
                    <td class="p_textbold" style="color: red">
                        End Year:&nbsp;
                    </td>
                    <td class="c_textbold">
                        <asp:Label ID="lblEndYear" runat="server" Text="" />
                    </td>
                </tr>
                <tr>
                    <td colspan="12">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="p_textbold" style="color: red">
                        Program Code:&nbsp;
                    </td>
                    <td class="c_textbold">
                        <asp:Label ID="lblPgmCode" runat="server" Text="" />
                    </td>
                    <td class="p_text">
                        Program Generation:&nbsp;
                    </td>
                    <td class="c_textbold">
                        <asp:Label ID="lblPgmGen" runat="server" Text="" />
                    </td>
                    <td class="p_text">
                        Make:&nbsp;
                    </td>
                    <td class="c_textbold">
                        <asp:Label ID="lblMake" runat="server" Text="" />
                    </td>
                    <td class="p_text">
                        Model Name:&nbsp;
                    </td>
                    <td class="c_textbold">
                        <asp:Label ID="lblModelName" runat="server" Text="" />
                    </td>
                    <td class="p_text">
                        Vehicle Type:&nbsp;
                    </td>
                    <td class="c_textbold">
                        <asp:Label ID="lblVehicleType" runat="server" Text="" />
                    </td>
                    <td class="p_text">
                        &nbsp;</td>
                    <td class="c_textbold">
                        <asp:Label ID="lblBodyStyle" runat="server" Text="" />
                    </td>
                </tr>
                <tr>
                    <td class="p_text">
                        Assembly Plant Location:&nbsp;
                    </td>
                    <td class="c_textbold">
                        <asp:Label ID="lblAPL" runat="server" Text="" />
                    </td>
                    <td class="p_text">
                        State:&nbsp;
                    </td>
                    <td class="c_textbold">
                        <asp:Label ID="lblState" runat="server" Text="" />
                    </td>
                    <td class="p_text">
                        Country:&nbsp;
                    </td>
                    <td class="c_textbold">
                        <asp:Label ID="lblCountry" runat="server" Text="" />
                    </td>
                    <td class="p_textbold" style="color: red">
                        SOP:&nbsp;
                    </td>
                    <td class="c_textbold">
                        <asp:Label ID="lblSOP" runat="server" Text="" />
                        <asp:Label ID="lblSOPMM" runat="server" Text="" Visible="false" />
                        <asp:Label ID="lblSOPYY" runat="server" Text="" Visible="false" />
                    </td>
                    <td class="p_textbold" style="color: red">
                        EOP:&nbsp;
                    </td>
                    <td class="c_textbold">
                        <asp:Label ID="lblEOP" runat="server" Text="" />
                        <asp:Label ID="lblEOPMM" runat="server" Text="" Visible="false" />
                        <asp:Label ID="lblEOPYY" runat="server" Text="" Visible="false" />
                    </td>
                    <td class="p_text">
                        UGN Business:&nbsp;
                    </td>
                    <td class="c_textbold">
                        <asp:Label ID="lblUGNBiz2" runat="server" Text="" />
                    </td>
                </tr>
                <tr>
                    <td class="p_text">
                        Status:&nbsp;
                    </td>
                    <td class="c_textbold">
                        <asp:Label ID="lblRecStatus" runat="server" Text="" />
                    </td>
                    <td colspan="10">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="12">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="p_textbold" style="color: red">
                        Year:
                    </td>
                    <td class="c_text">
                        <asp:Label ID="lblYearID" runat="server" Text="" />
                    </td>
                    <td class="p_textbold" style="color: red">
                        Annual Volume:
                    </td>
                    <td class="c_text">
                        <asp:Label ID="lblAnnualVolume" runat="server" Text="" />
                    </td>
                    <td class="p_textbold" style="color: red">
                        Quarter 1 Volume:
                    </td>
                    <td class="c_text">
                        <asp:Label ID="lblQtr1" runat="server" Text="" />
                    </td>
                    <td class="p_textbold" style="color: red">
                        Quarter 2 Volume:
                    </td>
                    <td class="c_text">
                        <asp:Label ID="lblQtr2" runat="server" Text="" />
                    </td>
                    <td class="p_textbold" style="color: red">
                        Quarter 3 Volume:
                    </td>
                    <td class="c_text">
                        <asp:Label ID="lblQtr3" runat="server" Text="" />
                    </td>
                    <td class="p_textbold" style="color: red">
                        Quarter 4 Volume:
                    </td>
                    <td class="c_text">
                        <asp:Label ID="lblQtr4" runat="server" Text="" />
                    </td>
                </tr>
                <tr>
                    <td class="p_textbold" style="color: red">
                        JAN:
                    </td>
                    <td class="c_text">
                        <asp:Label ID="lblJan" runat="server" Text="" />
                    </td>
                    <td class="p_textbold" style="color: red">
                        FEB:
                    </td>
                    <td class="c_text">
                        <asp:Label ID="lblFeb" runat="server" Text="" />
                    </td>
                    <td class="p_textbold" style="color: red">
                        MAR:
                    </td>
                    <td class="c_text">
                        <asp:Label ID="lblMar" runat="server" Text="" />
                    </td>
                    <td class="p_textbold" style="color: red">
                        APR:
                    </td>
                    <td class="c_text">
                        <asp:Label ID="lblApr" runat="server" Text="" />
                    </td>
                    <td class="p_textbold" style="color: red">
                        MAY:
                    </td>
                    <td class="c_text">
                        <asp:Label ID="lblMay" runat="server" Text="" />
                    </td>
                    <td class="p_textbold" style="color: red">
                        JUN:
                    </td>
                    <td class="c_text">
                        <asp:Label ID="lblJun" runat="server" Text="" />
                    </td>
                </tr>
                <tr>
                    <td class="p_textbold" style="color: red">
                        JUL:
                    </td>
                    <td class="c_text">
                        <asp:Label ID="lblJul" runat="server" Text="" />
                    </td>
                    <td class="p_textbold" style="color: red">
                        AUG:
                    </td>
                    <td class="c_text">
                        <asp:Label ID="lblAug" runat="server" Text="" />
                    </td>
                    <td class="p_textbold" style="color: red">
                        SEP:
                    </td>
                    <td class="c_text">
                        <asp:Label ID="lblSep" runat="server" Text="" />
                    </td>
                    <td class="p_textbold" style="color: red">
                        OCT:
                    </td>
                    <td class="c_text">
                        <asp:Label ID="lblOct" runat="server" Text="" />
                    </td>
                    <td class="p_textbold" style="color: red">
                        NOV:
                    </td>
                    <td class="c_text">
                        <asp:Label ID="lblNov" runat="server" Text="" />
                    </td>
                    <td class="p_textbold" style="color: red">
                        DEC:
                    </td>
                    <td class="c_text">
                        <asp:Label ID="lblDec" runat="server" Text="" />
                    </td>
                </tr>
                <tr>
                    <td colspan="12">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="12">
                        <asp:Button ID="btnInsert" runat="server" Text="Click Here to Use IHS Data for this Vehicle Entry"
                            CausesValidation="False" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <ajax:CollapsiblePanelExtender ID="AEExtender" runat="server" TargetControlID="AEContentPanel"
            ExpandControlID="AEPanel" CollapseControlID="AEPanel" Collapsed="FALSE" TextLabelID="lblAE"
            ExpandedText="IHS PLATFORM, PROGRAM, MODEL, ASSEMBLY AND MONTHLY VOLUME INFORMATION... "
            CollapsedText="IHS PLATFORM, PROGRAM, MODEL, ASSEMBLY AND MONTHLY VOLUME INFORMATION... "
            ImageControlID="imgAE" CollapsedImage="~/images/expand_blue.jpg" ExpandedImage="~/images/collapse_blue.jpg"
            SuppressPostBack="true">
        </ajax:CollapsiblePanelExtender>
        <br />
        <% End If%>
        <table>
            <tr>
                <td class="p_text" style="height: 9px" valign="top">
                    <asp:Label ID="lblReq1" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />&nbsp;Program:
                </td>
                <td style="height: 9px">
                    <asp:DropDownList ID="ddProgram" runat="server" AutoPostBack="true" />
                    <asp:RequiredFieldValidator ID="rfvProgram" runat="server" ControlToValidate="ddProgram"
                        ErrorMessage="Program is a required field." Font-Bold="False"><</asp:RequiredFieldValidator><br />
                    {Program / Model / Platform / Assembly Plant}
                </td>
                <td class="p_text" style="height: 9px">
                    <asp:Label ID="Label7" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />&nbsp;Planning
                    Year:
                </td>
                <td style="height: 9px">
                    <asp:DropDownList ID="ddYear" runat="server" AutoPostBack="True">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvYear" runat="server" ControlToValidate="ddYear"
                        ErrorMessage="Planning Year is a required field."><</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="p_text" style="height: 10px" valign="top">
                    <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />&nbsp;Customer:
                </td>
                <td style="height: 10px">
                    <asp:DropDownList ID="ddCustomer" runat="server" AutoPostBack="True" />
                    <asp:RequiredFieldValidator ID="rfvCustomer" runat="server" ControlToValidate="ddCustomer"
                        ErrorMessage="Customer is a required field."><</asp:RequiredFieldValidator><br />
                    {Sold To / CABBV / Customer Name}
                </td>
                <td class="p_text" style="height: 10px">
                    <asp:Label ID="Label5" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />&nbsp;Account
                    Manager:
                </td>
                <td style="height: 10px">
                    <asp:DropDownList ID="ddAccountManager" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvAccountManager" runat="server" ControlToValidate="ddAccountManager"
                        ErrorMessage="Account Manager is a required field."><</asp:RequiredFieldValidator>
                    <asp:Label ID="lblPrevAcctMgr" runat="server" Visible="false" />
                </td>
            </tr>
            <tr>
                <td class="p_text" style="height: 11px">
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />&nbsp;Start
                    of Production:
                </td>
                <td style="height: 11px">
                    <asp:TextBox ID="txtSOP" runat="server" MaxLength="12" Width="80px" AutoPostBack="true" />
                    <asp:ImageButton runat="server" ID="imgSOP" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                        CausesValidation="False" />
                    <ajax:CalendarExtender ID="cbeSOP" runat="server" TargetControlID="txtSOP" Format="MM/dd/yyyy"
                        PopupButtonID="imgSOP" />
                    <asp:RequiredFieldValidator ID="rfvSOP" runat="server" ControlToValidate="txtSOP"
                        ErrorMessage="Start of Production is a required field."><</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revSOP" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtSOP" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px"><</asp:RegularExpressionValidator>
                    <asp:CompareValidator ID="cvSOP" runat="server" ErrorMessage="Start of Production must be less than End of Production."
                        ControlToCompare="txtEOP" ControlToValidate="txtSOP" Operator="LessThan" Type="Date"><</asp:CompareValidator>
                    <asp:Label ID="lblPrevSOP" runat="server" Visible="false" />
                </td>
                <td class="p_text" style="height: 11px">
                    <asp:Label ID="Label4" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />&nbsp;End
                    of Production:
                </td>
                <td style="height: 11px">
                    <asp:TextBox ID="txtEOP" runat="server" MaxLength="12" Width="80px" AutoPostBack="true" />
                    <asp:ImageButton runat="server" ID="imgEOP" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                        CausesValidation="False" />
                    <ajax:CalendarExtender ID="cbeEOP" runat="server" TargetControlID="txtEOP" Format="MM/dd/yyyy"
                        PopupButtonID="imgEOP" />
                    <asp:RequiredFieldValidator ID="rfvEOP" runat="server" ControlToValidate="txtEOP"
                        ErrorMessage="End of Production is a required field."><</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revEOP" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtEOP" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px"><</asp:RegularExpressionValidator>
                    <asp:CompareValidator ID="cvEOP" runat="server" ControlToCompare="txtSOP" ControlToValidate="txtEOP"
                        ErrorMessage="End of Production must be greater than Start of Production." Operator="GreaterThan"
                        Type="Date"><</asp:CompareValidator>
                    <asp:Label ID="lblPrevEop" runat="server" Visible="false" />
                </td>
            </tr>
            <tr>
                <td class="p_text" style="height: 13px">
                    &nbsp;<asp:Label ID="Label3" runat="server" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label>Annual Volume:
                </td>
                <td style="height: 13px">
                    <asp:TextBox ID="txtVolume" runat="server" MaxLength="16" Width="85px"></asp:TextBox>
                    <asp:RangeValidator ID="rvVolume" runat="server" ControlToValidate="txtVolume" ErrorMessage="numeric value required for Annual Volume"
                        MaximumValue="999999" MinimumValue="0"><</asp:RangeValidator>
                    <asp:RequiredFieldValidator ID="rfvVolume" runat="server" ControlToValidate="txtVolume"
                        ErrorMessage="Annual Volume is a required field."><</asp:RequiredFieldValidator>
                    <asp:Label ID="lblPrevAnnualVolume" runat="server" Visible="false" />
                </td>
                <td class="p_text" style="height: 13px">
                </td>
                <td style="height: 13px">
                </td>
            </tr>
        </table>
        <br />
        <asp:Label ID="lblMessage0" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="Red"
            Text="" Visible="False" /><br />
        <asp:Label ID="lblMessage1" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="Red"
            Text="" Visible="False" />
        <table style="width: 95%">
            <tr>
                <td colspan="8" style="font-weight: bold; color: white; background-color: #990000">
                    Monthly Volume
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    JAN
                </td>
                <td>
                    <asp:TextBox ID="txtJan" runat="server" MaxLength="16" Width="85px" CausesValidation="True">0</asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftbJanVolume" runat="server" TargetControlID="txtJan"
                        FilterType="Numbers" />
                    <asp:Label ID="lblPrevJan" runat="server" Visible="false" />
                </td>
                <td class="p_text">
                    FEB
                </td>
                <td>
                    <asp:TextBox ID="txtFeb" runat="server" MaxLength="16" Width="85px">0</asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftbFebVolume" runat="server" TargetControlID="txtFeb"
                        FilterType="Numbers" />
                    <asp:Label ID="lblPrevFeb" runat="server" Visible="false" />
                </td>
                <td class="p_text">
                    MAR
                </td>
                <td>
                    <asp:TextBox ID="txtMar" runat="server" MaxLength="16" Width="85px">0</asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftbMarVolume" runat="server" TargetControlID="txtMar"
                        FilterType="Numbers" />
                    <asp:Label ID="lblPrevMar" runat="server" Visible="false" />
                </td>
                <td class="p_text">
                    APR
                </td>
                <td>
                    <asp:TextBox ID="txtApr" runat="server" MaxLength="16" Width="85px">0</asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftbAprVolume" runat="server" TargetControlID="txtApr"
                        FilterType="Numbers" />
                    <asp:Label ID="lblPrevApr" runat="server" Visible="false" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    MAY
                </td>
                <td>
                    <asp:TextBox ID="txtMay" runat="server" MaxLength="16" Width="85px">0</asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftbMayVolume" runat="server" TargetControlID="txtMay"
                        FilterType="Numbers" />
                    <asp:Label ID="lblPrevMay" runat="server" Visible="false" />
                </td>
                <td class="p_text">
                    JUN
                </td>
                <td>
                    <asp:TextBox ID="txtJun" runat="server" MaxLength="16" Width="85px">0</asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftbJunVolume" runat="server" TargetControlID="txtJun"
                        FilterType="Numbers" />
                    <asp:Label ID="lblPrevJun" runat="server" Visible="false" />
                </td>
                <td class="p_text">
                    JUL
                </td>
                <td>
                    <asp:TextBox ID="txtJul" runat="server" MaxLength="16" Width="85px">0</asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftbJulVolume" runat="server" TargetControlID="txtJul"
                        FilterType="Numbers" />
                    <asp:Label ID="lblPrevJul" runat="server" Visible="false" />
                </td>
                <td class="p_text">
                    AUG
                </td>
                <td>
                    <asp:TextBox ID="txtAug" runat="server" MaxLength="16" Width="85px">0</asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftbAugVolume" runat="server" TargetControlID="txtAug"
                        FilterType="Numbers" />
                    <asp:Label ID="lblPrevAug" runat="server" Visible="false" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    SEP
                </td>
                <td>
                    <asp:TextBox ID="txtSep" runat="server" MaxLength="16" Width="85px">0</asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftbSepVolume" runat="server" TargetControlID="txtSep"
                        FilterType="Numbers" />
                    <asp:Label ID="lblPrevSep" runat="server" Visible="false" />
                </td>
                <td class="p_text">
                    OCT
                </td>
                <td>
                    <asp:TextBox ID="txtOct" runat="server" MaxLength="16" Width="85px">0</asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftbOctVolume" runat="server" TargetControlID="txtOct"
                        FilterType="Numbers" />
                    <asp:Label ID="lblPrevOct" runat="server" Visible="false" />
                </td>
                <td class="p_text">
                    NOV
                </td>
                <td>
                    <asp:TextBox ID="txtNov" runat="server" MaxLength="16" Width="85px">0</asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftbNovVolume" runat="server" TargetControlID="txtNov"
                        FilterType="Numbers" />
                    <asp:Label ID="lblPrevNov" runat="server" Visible="false" />
                </td>
                <td class="p_text">
                    DEC
                </td>
                <td>
                    <asp:TextBox ID="txtDec" runat="server" MaxLength="16" Width="85px">0</asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftbDecVolume" runat="server" TargetControlID="txtDec"
                        FilterType="Numbers" />
                    <asp:Label ID="lblPrevDec" runat="server" Visible="false" />
                </td>
            </tr>
        </table>
        <br />
        <br />
        <asp:Label ID="lblMessage" runat="server" Font-Bold="True" ForeColor="Red" Text=""
            Visible="False" Font-Size="Medium" />
        <br />
        <table>
            <% If ViewState("Admin") = True Then%>
            <tr>
                <td class="p_text" style="vertical-align: top;">
                    <asp:Label ID="lblReqNotes" runat="server" Font-Bold="True" ForeColor="Red" Text="*"
                        Visible="false" />&nbsp;Notes:
                </td>
                <td>
                    <asp:TextBox ID="txtNotes" runat="server" MaxLength="400" Rows="3" TextMode="MultiLine"
                        Width="500px" />&nbsp;<asp:Label ID="lblReqNotesText" runat="server" Font-Bold="True"
                            ForeColor="Red" Text="< Required" Visible="false" /><br />
                    <asp:Label ID="lblNotes" runat="server" Font-Bold="True" ForeColor="Red" />
                    <ajax:FilteredTextBoxExtender ID="ftbNotes" runat="server" TargetControlID="txtNotes"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,.()\/_+*!-=%$#' " />
                    <asp:Label ID="lblNotes1" runat="server" Text="Notes will not display after save. Information will be stored in history."></asp:Label>
                </td>
            </tr>
            <% End If%>
            <tr>
                <td>
                    &nbsp;<asp:Label ID="lblIHSDataUsed" runat="server" Visible="false" />
                </td>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Save" CausesValidation="true" />&nbsp;
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="False" />&nbsp;
                    <asp:Button ID="btnDelete" runat="server" Text="Delete" CausesValidation="False"
                        OnClientClick="return confirm('Are you sure you want to delete this record?');" />
                </td>
            </tr>
        </table>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
            ShowSummary="true" Font-Size="X-Small" />
        <asp:Label ID="lblMessage2" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="Red"
            Text="" Visible="False" /><br />
        <asp:Label ID="lblMessage3" runat="server" Font-Bold="True" Font-Size="small" ForeColor="Red"
            Text="" Visible="False" />
    </asp:Panel>
</asp:Content>
