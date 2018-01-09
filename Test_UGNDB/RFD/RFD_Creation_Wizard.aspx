<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    MaintainScrollPositionOnPostback="true" CodeFile="RFD_Creation_Wizard.aspx.vb"
    Inherits="RFD_Creation_Wizard" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server">
        <asp:Label runat="server" ID="lblMessage" SkinID="MessageLabelSkin"></asp:Label>
        <table width="60%" runat="server" id="tblRFDCreationWizard">
            <tr runat="server" id="trInitiator" style="background-color: Gray">
                <td>
                    <asp:Button runat="server" ID="btnInitiatorEdit" Text="Edit" Visible="false" />
                </td>
                <td class="p_text" style="white-space: nowrap">
                    Who is the Initiator:
                </td>
                <td>
                    <asp:DropDownList ID="ddInitiator" runat="server" Enabled="false" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
                <td>
                    <asp:Button runat="server" ID="btnInitiatorNext" Text="Next" />
                </td>
            </tr>
            <tr runat="server" id="trBusinessProcessType" visible="false" style="background-color: Gray">
                <td>
                    <asp:Button runat="server" ID="btnBusinessProcessTypeEdit" Text="Edit" Visible="false" />
                </td>
                <td class="p_text" style="white-space: nowrap">
                    What is the Business Process Type?
                </td>
                <td>
                    <asp:DropDownList ID="ddBusinessProcessType" runat="server" Enabled="false" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
                <td>
                    <asp:Button runat="server" ID="btnBusinessProcessTypePrevious" Text="Previous" Visible="false" />
                    <asp:Button runat="server" ID="btnBusinessProcessTypeNext" Text="Next" Visible="false" />
                </td>
            </tr>
            <tr runat="server" id="trBusinessProcessAction" visible="false">
                <td>
                    <asp:Button runat="server" ID="btnBusinessProcessActionEdit" Text="Edit" Visible="false" />
                </td>
                <td class="p_text" style="white-space: nowrap">
                    Type of action to the part:
                </td>
                <td>
                    <asp:DropDownList ID="ddBusinessProcessAction" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Button runat="server" ID="btnBusinessProcessActionPrevious" Text="Previous"
                        Visible="false" />
                    <asp:Button runat="server" ID="btnBusinessProcessActionNext" Text="Next" Visible="false" />
                </td>
            </tr>
            <tr runat="server" id="trIsCostReduction" visible="false" style="background-color: Gray">
                <td>
                    <asp:Button runat="server" ID="btnIsCostReductionEdit" Text="Edit" Visible="false" />
                </td>
                <td class="p_text" style="white-space: nowrap">
                    Is this a Cost Reduction?
                </td>
                <td>
                    <asp:DropDownList ID="ddIsCostReduction" runat="server">
                        <asp:ListItem Value="False" Selected="True">No</asp:ListItem>
                        <asp:ListItem Value="True">Yes</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Button runat="server" ID="btnIsCostReductionPrevious" Text="Previous"
                        Visible="false" />
                    <asp:Button runat="server" ID="btnIsCostReductionNext" Text="Next" Visible="false" />
                </td>
            </tr>
            <tr runat="server" id="trUGNFacility" visible="false" style="background-color: Gray">
                <td>
                    <asp:Button runat="server" ID="btnUGNFacilityEdit" Text="Edit" Visible="false" />
                </td>
                <td class="p_text" style="white-space: nowrap">
                    UGN Facility:
                </td>
                <td>
                    <asp:DropDownList ID="ddUGNFacility" runat="server" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvUGNFacility" ControlToValidate="ddUGNFacility"
                        SetFocusOnError="true" ErrorMessage="UGN Facility is required" Text="<" ValidationGroup="vgSaveUGNFacility" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Button runat="server" ID="btnUGNFacilityPrevious" Text="Previous" Visible="false" />
                    <asp:Button runat="server" ID="btnUGNFacilityNext" Text="Next" Visible="false" ValidationGroup="vgSaveUGNFacility" />
                </td>
            </tr>
            <tr runat="server" id="trDesignationType" visible="false">
                <td>
                    <asp:Button runat="server" ID="btnDesignationTypeEdit" Text="Edit" Visible="false" />
                </td>
                <td class="p_text" style="white-space: nowrap">
                    Designation Type:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddDesignationType" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
                <td>
                    <asp:Button runat="server" ID="btnDesignationTypePrevious" Text="Previous" Visible="false" />
                    <asp:Button runat="server" ID="btnDesignationTypeNext" Text="Next" Visible="false" />
                </td>
            </tr>
            <tr runat="server" id="trMessageAccountManager" visible="false">
                <td colspan="3">
                    <asp:Label ID="lblMessageAccountManager" runat="server" Text="Account Manager is optional"
                        SkinID="MessageLabelSkin" />
                    <asp:ValidationSummary runat="server" ID="vsAccountManager" ValidationGroup="vgAccountManager"
                        ShowMessageBox="true" ShowSummary="true" />
                </td>
            </tr>
            <tr runat="server" id="trCustomer" visible="false">
                <td>
                    &nbsp;
                </td>
                <td class="p_text" style="white-space: nowrap">
                    Customer:
                </td>
                <td>
                    <asp:DropDownList ID="ddCustomer" runat="server" Visible="true" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr runat="server" id="trAccountManager" visible="false">
                <td>
                    <asp:Button runat="server" ID="btnAccountManagerEdit" Text="Edit" Visible="false" />
                </td>
                <td class="p_text" style="white-space: nowrap">
                    Account Manager:
                </td>
                <td>
                    <asp:DropDownList ID="ddAccountManager" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ID="rfvAccountManager" ControlToValidate="ddAccountManager"
                        SetFocusOnError="true" ErrorMessage="Account Manager is required" Text="<" ValidationGroup="vgAccountManager" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
                <td>
                    <asp:Button runat="server" ID="btnAccountManagerPrevious" Text="Previous" Visible="false" />
                    <asp:Button runat="server" ID="btnAccountManagerNext" Text="Next" Visible="false"
                        CausesValidation="true" ValidationGroup="vgAccountManager" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:ValidationSummary runat="server" ID="vsProgramManager" ValidationGroup="vgProgramManager"
                        ShowMessageBox="true" ShowSummary="true" />
                </td>
            </tr>
            <tr runat="server" id="trProgramManager" visible="false">
                <td>
                    <asp:Button runat="server" ID="btnProgramManagerEdit" Text="Edit" Visible="false" />
                </td>
                <td class="p_text" style="white-space: nowrap">
                    Program Manager:
                </td>
                <td>
                    <asp:DropDownList ID="ddProgramManager" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ID="rfvProgramManager" ControlToValidate="ddProgramManager"
                        SetFocusOnError="true" ErrorMessage="Program Manager is required" Text="<" ValidationGroup="vgProgramManager" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
                <td>
                    <asp:Button runat="server" ID="btnProgramManagerPrevious" Text="Previous" Visible="false" />
                    <asp:Button runat="server" ID="btnProgramManagerNext" Text="Next" Visible="false"
                        CausesValidation="true" ValidationGroup="vgProgramManager" />
                </td>
            </tr>
            <tr runat="server" id="trMake" visible="false">
                <td>
                    &nbsp;
                </td>
                <td class="p_text" style="white-space: nowrap">
                    Make:
                    <asp:ValidationSummary runat="server" ID="vsSaveWorkFlowMake" ValidationGroup="vgSaveWorkFlowMake"
                        ShowMessageBox="true" ShowSummary="true" />
                </td>
                <td class="c_textbold">
                    <asp:DropDownList ID="ddWorkFlowMake" runat="server" AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ID="rfvWorkFlowMake" ControlToValidate="ddWorkFlowMake"
                        SetFocusOnError="true" ErrorMessage="Make is required" Text="<" ValidationGroup="vgSaveWorkFlowMake" />
                </td>
            </tr>
            <tr runat="server" id="trPurchasingTeamMemberByMake" visible="false">
                <td>
                    <asp:Button runat="server" ID="btnWorkflowMakeEdit" Text="Edit" Visible="false" />
                </td>
                <td class="p_text" style="white-space: nowrap">
                    Purchasing Team Member<br />
                    Assigned to Make:
                </td>
                <td class="c_textbold">
                    <asp:DropDownList ID="ddPurchasingTeamMemberByMake" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Button runat="server" ID="btnWorkflowMakePrevious" Text="Previous" Visible="false" />
                    <asp:Button runat="server" ID="btnWorkflowMakeNext" Text="Next" Visible="false" />
                </td>
            </tr>
            <tr runat="server" id="trFamily" visible="false">
                <td>
                    &nbsp;
                </td>
                <td class="p_text" style="white-space: nowrap">
                    (Raw Material) Family:
                    <asp:ValidationSummary runat="server" ID="vsSaveWorkflowFamily" ValidationGroup="vgSaveWorkflowFamily"
                        ShowMessageBox="true" ShowSummary="true" />
                </td>
                <td class="c_textbold">
                    <asp:DropDownList ID="ddWorkflowFamily" runat="server" AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ID="rfvWorkflowFamily" ControlToValidate="ddWorkflowFamily"
                        SetFocusOnError="true" ErrorMessage="Family is required" Text="<" ValidationGroup="vgSaveWorkflowFamily" />
                </td>
            </tr>
            <tr runat="server" id="trPurchasingTeamMemberByFamily" visible="false">
                <td>
                    <asp:Button runat="server" ID="btnWorkflowFamilyEdit" Text="Edit" Visible="false" />
                </td>
                <td class="p_text" style="white-space: nowrap">
                    Purchasing Team Member<br />
                    Assigned to Family:
                </td>
                <td class="c_textbold">
                    <asp:DropDownList ID="ddPurchasingTeamMemberByFamily" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
                <td>
                    <asp:Button runat="server" ID="btnWorkflowFamilyPrevious" Text="Previous" Visible="false" />
                    <asp:Button runat="server" ID="btnWorkflowFamilyNext" Text="Next" Visible="false"
                        ValidationGroup="vgSaveWorkflowFamily" />
                </td>
            </tr>
            <tr runat="server" id="trCommodity" visible="false">
                <td>
                    &nbsp;
                </td>
                <td class="p_text" style="white-space: nowrap" valign="top">
                    Commodity:
                    <asp:ValidationSummary runat="server" ID="vsSaveWorkflowCommodity" ValidationGroup="vgSaveWorkflowCommodity"
                        ShowMessageBox="true" ShowSummary="true" />
                </td>
                <td class="c_textbold" visible="true" valign="top">
                    <asp:DropDownList ID="ddWorkFlowCommodity" runat="server" AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ID="rfvWorkflowCommodity" ControlToValidate="ddWorkFlowCommodity"
                        SetFocusOnError="true" ErrorMessage="Commodity is required" Text="<" ValidationGroup="vgSaveWorkflowCommodity" />
                    <br />
                    {Commodity / Classification}
                </td>
            </tr>
            <tr runat="server" id="trProductDevelopmentTeamMemberByCommodity" visible="false">
                <td>
                    <asp:Button runat="server" ID="btnWorkflowCommodityEdit" Text="Edit" Visible="false" />
                </td>
                <td class="p_text" style="white-space: nowrap">
                    Product Development Team Member<br />
                    Assigned to Commodity:
                </td>
                <td class="c_textbold">
                    <asp:DropDownList ID="ddProductDevelopmentTeamMemberByCommodity" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
                <td>
                    <asp:Button runat="server" ID="btnWorkFlowCommodityPrevious" Text="Previous" Visible="false" />
                    <asp:Button runat="server" ID="btnWorkFlowCommodityNext" Text="Next" Visible="false"
                        ValidationGroup="vgSaveWorkflowCommodity" />
                </td>
            </tr>
            <tr runat="server" id="trPriceCode" visible="false">
                <td>
                    <asp:Button runat="server" ID="btnPriceCodeEdit" Text="Edit" Visible="false" />
                </td>
                <td class="p_text" style="white-space: nowrap">
                    Price Code
                    <br />
                    (Production Status):
                </td>
                <td>
                    <asp:DropDownList ID="ddPriceCode" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ID="rfvPriceCode" ControlToValidate="ddPriceCode"
                        SetFocusOnError="true" ErrorMessage="Family is required" Text="<" ValidationGroup="vgSavePriceCode" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
                <td>
                    <asp:Button runat="server" ID="btnPriceCodePrevious" Text="Previous" Visible="false" />
                    <asp:Button runat="server" ID="btnPriceCodeNext" Text="Next" Visible="false" ValidationGroup="vgSavePriceCode" />
                </td>
            </tr>
            <tr runat="server" id="trPriority" visible="false">
                <td>
                    <asp:Button runat="server" ID="btnPriorityEdit" Text="Edit" Visible="false" />
                </td>
                <td class="p_text" style="white-space: nowrap">
                    Priority:
                </td>
                <td>
                    <asp:DropDownList ID="ddPriority" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
                <td>
                    <asp:Button runat="server" ID="btnPriorityPrevious" Text="Previous" Visible="false" />
                    <asp:Button runat="server" ID="btnPriorityNext" Text="Next" Visible="false" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label ID="lblMessageDueDate" runat="server" Text="Due Date is required for an RFC."
                        SkinID="MessageLabelSkin" Visible="false" />
                    <asp:ValidationSummary runat="server" ID="vsSaveDueDate" ValidationGroup="vgSaveDueDate"
                        ShowMessageBox="true" ShowSummary="true" />
                </td>
            </tr>
            <tr runat="server" id="trDueDate" visible="false">
                <td>
                    <asp:Button runat="server" ID="btnDueDateEdit" Text="Edit" Visible="false" />
                </td>
                <td class="p_text" style="white-space: nowrap">
                    Due Date:
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtDueDate" MaxLength="10" Width="100px"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ID="rfvDueDate" ControlToValidate="txtDueDate"
                        SetFocusOnError="true" ErrorMessage="Due date is required" Text="<" ValidationGroup="vgSaveDueDate" />
                    <asp:ImageButton runat="server" ID="imgDueDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                    <ajax:CalendarExtender ID="ceDueDate" runat="server" TargetControlID="txtDueDate"
                        PopupButtonID="imgDueDate" />
                    <asp:RegularExpressionValidator ID="revDueDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtDueDate" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px" ValidationGroup="vgSaveDueDate" Text="<"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
                <td>
                    <asp:Button runat="server" ID="btnDueDatePrevious" Text="Previous" Visible="false" />
                    <asp:Button runat="server" ID="btnDueDateNext" Text="Next" Visible="false" CausesValidation="true"
                        ValidationGroup="vgSaveDueDate" />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:ValidationSummary runat="server" ID="vsSaveRFDDesc" ValidationGroup="vgSaveRFDDesc"
                        ShowMessageBox="true" ShowSummary="true" />
                </td>
            </tr>
            <tr runat="server" id="trRFDDesc" visible="false">
                <td>
                    <asp:Button runat="server" ID="btnRFDDescEdit" Text="Edit" Visible="false" />
                </td>
                <td class="p_text" style="white-space: nowrap">
                    Description:
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtRFDDesc" TextMode="MultiLine" Width="400px" Height="80px"></asp:TextBox>
                    <br />
                    <asp:Label ID="lblRFDDescCharCount" SkinID="MessageLabelSkin" runat="server"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="rfvRFDDesc" ControlToValidate="txtRFDDesc"
                        SetFocusOnError="true" ErrorMessage="Description is required" Text="<" ValidationGroup="vgSaveRFDDesc" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
                <td>
                    <asp:Button runat="server" ID="btnRFDDescPrevious" Text="Previous" Visible="false" />
                    <asp:Button runat="server" ID="btnRFDDescNext" Text="Next" Visible="false" CausesValidation="true"
                        ValidationGroup="vgSaveRFDDesc" />
                </td>
            </tr>
            <tr runat="server" id="trImpactOnUGN" visible="false">
                <td>
                    <asp:Button runat="server" ID="btnImpactOnUGNEdit" Text="Edit" Visible="false" />
                </td>
                <td class="p_text" style="white-space: nowrap">
                    RFD Meeting Notes:
                    <br />
                    (Impact On UGN):
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtImpactOnUGN" TextMode="MultiLine" Width="400px"
                        Height="80px"></asp:TextBox>
                    <br />
                    <asp:Label ID="lblImpactOnUGNCharCount" SkinID="MessageLabelSkin" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
                <td>
                    <asp:Button runat="server" ID="btnImpactOnUGNPrevious" Text="Previous" Visible="false" />
                    <asp:Button runat="server" ID="btnImpactOnUGNNext" Text="Next" Visible="false" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:ValidationSummary runat="server" ID="vsSaveTarget" ValidationGroup="vgSaveTarget"
                        ShowMessageBox="true" ShowSummary="true" />
                </td>
            </tr>
            <tr runat="server" id="trTargetPrice" visible="false">
                <td>
                    <asp:Button runat="server" ID="btnTargetEdit" Text="Edit" Visible="false" />
                </td>
                <td class="p_text" style="white-space: nowrap">
                    Target Price: ($)
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtTargetPrice" MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvTargetPrice" Operator="DataTypeCheck"
                        ValidationGroup="vgSaveTarget" Type="double" Text="<" ControlToValidate="txtTargetPrice"
                        ErrorMessage="Target price must be a number." SetFocusOnError="True" />
                    <asp:RangeValidator ID="rvTargetPrice" runat="server" ControlToValidate="txtTargetPrice"
                        Display="Dynamic" ErrorMessage="Target price requires a numeric value -999999999.99 to 999999999.99"
                        Height="16px" MaximumValue="999999999.99" MinimumValue="-999999999.99" Type="Double"
                        ValidationGroup="vgSaveTarget"><</asp:RangeValidator>
                </td>
            </tr>
            <tr runat="server" id="trTargetAnnualVolume" visible="false">
                <td>
                    &nbsp;
                </td>
                <td class="p_text" style="white-space: nowrap">
                    Target Annual Volume:
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtTargetAnnualVolume" MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvTargetAnnualVolume" Operator="DataTypeCheck"
                        ValidationGroup="vgSaveTarget" Type="integer" Text="<" ControlToValidate="txtTargetAnnualVolume"
                        ErrorMessage="Target annual volume must be an integer." SetFocusOnError="True" />
                    &nbsp;
                    <asp:Button runat="server" ID="btnCalculateTargetAnnualSales" Text="Calculate Sales"
                        CausesValidation="true" ValidationGroup="vgSaveTarget" />
                </td>
            </tr>
            <tr runat="server" id="trTargetAnnualSales" visible="false">
                <td>
                    &nbsp;
                </td>
                <td class="p_text" style="white-space: nowrap">
                    Target Annual Sales: ($)
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtTargetAnnualSales" MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvTargetAnnualSales" Operator="DataTypeCheck"
                        ValidationGroup="vgSaveTarget" Type="double" Text="<" ControlToValidate="txtTargetAnnualSales"
                        ErrorMessage="Target annual sales must be a number." SetFocusOnError="True" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
                <td>
                    <asp:Button runat="server" ID="btnTargetPrevious" Text="Previous" Visible="false" />
                    <asp:Button runat="server" ID="btnTargetNext" Text="Next" Visible="false" CausesValidation="true"
                        ValidationGroup="vgSaveTarget" />
                </td>
            </tr>
        </table>
        <table runat="server" id="tblRequiredTeamMembers" visible="false">
            <tr>
                <td>
                    <asp:CheckBox ID="cbCustomerApprovalRequired" runat="server" Text="Is Customer Approval Required?" />
                </td>
                <td>
                    <asp:CheckBox runat="server" ID="cbAffectsCostSheetOnly" Text="Does this affect Cost Sheets only?"
                        AutoPostBack="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox runat="server" ID="cbPackagingRequired" Text="Is Packaging required?" />
                </td>
                <td>
                    <asp:CheckBox runat="server" ID="cbPlantControllerRequired" Text="Is Finance required?" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox runat="server" ID="cbProcessRequired" Text="Is Process required?" />
                </td>
                <td>
                    <asp:CheckBox runat="server" ID="cbToolingRequired" Text=" Is Tooling required?" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox runat="server" ID="cbCostingRequired" Text="Is Costing required?" />
                </td>
                <td>
                    <asp:CheckBox runat="server" ID="cbCapitalRequired" Text="Capital required?" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox runat="server" ID="cbProductDevelopmentRequired" Text="Is Product Development required?" />
                </td>
                <td>
                    <asp:CheckBox runat="server" ID="cbPurchasingExternalRFQRequired" Text="Is Purchasing for External RFQ required?" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox runat="server" ID="cbQualityEngineeringRequired" Text="Is Quality Engineering required?" />
                </td>
                <td>
                    <asp:CheckBox runat="server" ID="cbPurchasingRequired" Text="Is Purchasing for Contract / P.O. required?" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox runat="server" ID="cbRDrequired" Text="Is Research and Development required? <i>(CC Notification Only - No approval needed)</i>" />
                </td>
                <td>
                    <asp:CheckBox runat="server" ID="cbDVPRrequired" Text="Is DVPR Document required?" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox runat="server" ID="cbMeetingRequired" Checked="true" Text="An RFD Meeting is required." />
                </td>
                <td>
                    <asp:Button runat="server" ID="btnRequiredTeamMembersPrevious" Text="Previous" Visible="false" />
                    <asp:Button runat="server" ID="btnCreateRFD" Text="Create RFD" Visible="false" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
