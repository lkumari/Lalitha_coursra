<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="RFD_Detail.aspx.vb" Inherits="RFD_Detail" Title="RFD Detail" MaintainScrollPositionOnPostback="true"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">

    <script language="javascript" type="text/javascript">

        function doBrowseClick() {

            document.all.ctl00$maincontent$fileInputNetworkFileReference.click();
            //alert('click');
        }

        function saveNetworkFileName() {
            if (document.all.ctl00$maincontent$fileInputNetworkFileReference.value != null) {
                document.all.ctl00$maincontent$fileTextNetworkFileReference.value = document.all.ctl00$maincontent$fileInputNetworkFileReference.value;
                clearFileInputField('uploadFile_div');
            }
        }

        function clearFileInputField(tagId) {
            document.getElementById(tagId).innerHTML =
                    document.getElementById(tagId).innerHTML;
        }

        function doCopyReason(RFDNo) {
            var copyReason = prompt('Please enter a reason for the copy such as the difference between this RFD and the next', '', 'Copy Reason');

            if ((copyReason == '') || (copyReason == ' ') || (copyReason == null)) {
                return false;
            } else {
                if (document.all.ctl00$maincontent$txtCopyReason != null) {
                    document.all.ctl00$maincontent$txtCopyReason.value = 'Copied from RFDNo: ' + RFDNo + '\r\n' + copyReason
                }
                return true;
            }

        }
    </script>

    <asp:Panel ID="localPanel" runat="server">
        <asp:Label runat="server" ID="lblMessage" SkinID="MessageLabelSkin" />
        <h1>
            RFD No: &nbsp;<asp:Label runat="server" ID="lblRFDNo" ForeColor="Red" /></h1>
        <table>
            <tr>
                <td class="p_text">
                    <asp:Label runat="server" ID="lblPreviousRFDNo" Text="Previous RFD No:" Visible="false" />
                </td>
                <td class="c_textbold">
                    <asp:HyperLink runat="server" ID="hlnkPreviousRFDNo" Font-Underline="true" Font-Bold="true"
                        ToolTip="Click here to see previous RFD." Target="_blank" Visible="false" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Overall Status:
                </td>
                <td>
                    <asp:DropDownList ID="ddStatus" runat="server" Enabled="false" />
                </td>
                <td class="p_text">
                    <asp:Label runat="server" ID="lblCompletionDateLabel" Text="Completion Date:" Visible="false" />
                </td>
                <td class="c_textbold">
                    <asp:Label runat="server" ID="lblCompletionDateValue" />
                </td>
                <td class="p_text">
                    <asp:Label ID="Label23" runat="server" Font-Bold="True" ForeColor="Red" Text="* " /><asp:Label
                        ID="lblisCostReduction" runat="server" Text="Is this a Cost Reduction?" />
                </td>
                <td>
                    <asp:DropDownList ID="ddisCostReduction" runat="server">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Value="False">No</asp:ListItem>
                        <asp:ListItem Value="True">Yes</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;<asp:RequiredFieldValidator ID="rfvisCostReduction" runat="server" ControlToValidate="ddisCostReduction"
                        ErrorMessage="Is this a Cost Reduction? question requires an answer." Font-Bold="False"
                        ValidationGroup="vgSaveDescription"><</asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label runat="server" ID="lblCreatedOnDateLabel" Text="Created:" />
                </td>
                <td class="c_textbold" colspan="3">
                    <asp:Label runat="server" ID="lblCreatedOnDateValue" />
                </td>
                <td class="p_text">
                    <asp:Label runat="server" ID="lblBusinessAwaredDateLabel" Text="Business Awarded Date:"
                        Visible="false" />
                </td>
                <td class="c_textbold" colspan="3">
                    <asp:Label runat="server" ID="lblBusinessAwardedDateValue" Visible="false" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label runat="server" ID="lblNewCustomerPartNoTopLabel" Text="New Customer Part No:"
                        Visible="false" />
                </td>
                <td class="c_textbold">
                    <asp:Label runat="server" ID="lblNewCustomerPartNoTopValue" Visible="false" />
                </td>
                <td class="p_text">
                    <asp:Label runat="server" ID="lblNewDesignLevelTopLabel" Text="New Design Level:"
                        Visible="false" />
                </td>
                <td class="c_textbold">
                    <asp:Label runat="server" ID="lblNewDesignLevelTopValue" Visible="false" />
                </td>
            </tr>
        </table>
        <br />
        <table width="80%">
            <tr>
                <td>
                    <asp:Button ID="btnPreview" runat="server" Text="Preview" Visible="false" />
                    &nbsp;
                    <asp:Button ID="btnBusinessAwarded" runat="server" Text="Business Awarded" Visible="false"
                        ValidationGroup="vgSaveDescription" />
                    &nbsp;
                    <asp:Button ID="btnClose" runat="server" Text="Close" Visible="false" />
                    &nbsp;
                    <asp:Button ID="btnCloseCancel" runat="server" Text="Cancel Close" Visible="false" />
                    &nbsp;
                    <asp:Button ID="btnCopy" runat="server" Text="Copy" Visible="false" CausesValidation="false" />
                    &nbsp;
                    <asp:Button ID="btnVoid" runat="server" Text="Void" Visible="false" />
                    &nbsp;
                    <asp:Button ID="btnVoidCancel" runat="server" Text="Cancel Void" Visible="false" />
                </td>
            </tr>
            <tr align="center">
                <td>
                    <asp:RadioButtonList runat="server" ID="rbCopyType" RepeatDirection="Horizontal">
                        <asp:ListItem Text="New Part/Rev Copy (New info here becomes current info in the copy)"
                            Value="N"></asp:ListItem>
                        <asp:ListItem Text="Duplicate Copy (Keep info the same)" Value="D" Selected="True"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblVoidCommentMarker" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="*" Visible="false" />
                    <asp:Label runat="server" ID="lblVoidComment" Text="Void Comment:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtVoidComment" MaxLength="100" Height="80px" TextMode="MultiLine"
                        Visible="false"></asp:TextBox>
                    <br />
                    <asp:Label ID="lblVoidCommentCharCount" SkinID="MessageLabelSkin" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <table>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblCloseCommentMarker" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="*" Visible="false" />
                    <asp:Label runat="server" ID="lblCloseComment" Text="Close Comment:" Visible="false" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtCloseComment" MaxLength="100" Height="80px" TextMode="MultiLine"
                        Visible="false" />
                    <br />
                    <asp:Label ID="lblCloseCommentCharCount" SkinID="MessageLabelSkin" runat="server" />
                </td>
            </tr>
        </table>
        <br />
        <table width="98%">
            <tr>
                <td>
                    <a href="RFD_Initiator_Help.aspx" style="text-decoration: 'underline'; color: Blue"
                        target="_blank">
                        <img src="../images/help.jpg" alt="" style="border: 0" />Initiator Help</a>
                </td>
                <td>
                    <a href="RFD_Packaging_Help.aspx" style="text-decoration: 'underline'; color: Blue"
                        target="_blank">
                        <img src="../images/help.jpg" alt="" style="border: 0" />Packaging Help</a>
                </td>
                <td>
                    <a href="RFD_Plant_Controller_Help.aspx" style="text-decoration: 'underline'; color: Blue"
                        target="_blank">
                        <img src="../images/help.jpg" alt="" style="border: 0" />Finance/Plant Controller
                        Help</a>
                </td>
                <td>
                    <a href="RFD_Process_Help.aspx" style="text-decoration: 'underline'; color: Blue"
                        target="_blank">
                        <img src="../images/help.jpg" alt="" style="border: 0" />Process Help</a>
                </td>
                <td>
                    <a href="RFD_Product_Development_Help.aspx" style="text-decoration: 'underline';
                        color: Blue" target="_blank">
                        <img src="../images/help.jpg" alt="" style="border: 0" />Product Engineering Help</a>
                </td>
            </tr>
            <tr>
                <td>
                    <a href="RFD_Tooling_Help.aspx" style="text-decoration: 'underline'; color: Blue"
                        target="_blank">
                        <img src="../images/help.jpg" alt="" style="border: 0" />Capital and Tooling Help</a>
                </td>
                <td>
                    <a href="RFD_Costing_Help.aspx" style="text-decoration: 'underline'; color: Blue"
                        target="_blank">
                        <img src="../images/help.jpg" alt="" style="border: 0" />Costing Help</a>
                </td>
                <td>
                    <a href="RFD_Quality_Engineer_Help.aspx" style="text-decoration: 'underline'; color: Blue"
                        target="_blank">
                        <img src="../images/help.jpg" alt="" style="border: 0" />Quality Engineer Help</a>
                </td>
                <td>
                    <a href="RFD_Purchasing_Help.aspx" style="text-decoration: 'underline'; color: Blue"
                        target="_blank">
                        <img src="../images/help.jpg" alt="" style="border: 0" />Purchasing Help</a>
                </td>
            </tr>
        </table>
        <br />
        <asp:Menu ID="menuTopTabs" Height="30px" runat="server" Orientation="Horizontal"
            StaticEnableDefaultPopOutImage="False" StaticMenuItemStyle-CssClass="tab" StaticSelectedStyle-CssClass="selectedTab"
            CssClass="tabs">
            <Items>
                <asp:MenuItem Text="Desc." Value="0" ImageUrl=""></asp:MenuItem>
                <asp:MenuItem Text="KIT" Value="1" ImageUrl=""></asp:MenuItem>
                <asp:MenuItem Text="Customer PartNo and F.G. PartNo" Value="2" ImageUrl=""></asp:MenuItem>
                <asp:MenuItem Text="Child PartNo" Value="3" ImageUrl=""></asp:MenuItem>
                <asp:MenuItem Text="Customer Program" Value="4" ImageUrl=""></asp:MenuItem>
                <asp:MenuItem Text="UGN Facility" Value="5" ImageUrl=""></asp:MenuItem>
                <asp:MenuItem Text="Vendor" Value="6" ImageUrl=""></asp:MenuItem>
            </Items>
        </asp:Menu>
        <asp:Menu ID="menuBottomTabs" Height="30px" runat="server" Orientation="Horizontal"
            StaticEnableDefaultPopOutImage="False" StaticMenuItemStyle-CssClass="tab" StaticSelectedStyle-CssClass="selectedTab"
            CssClass="tabs">
            <Items>
                <asp:MenuItem Text="Packaging" Value="7" ImageUrl=""></asp:MenuItem>
                <asp:MenuItem Text="Labor and Overhead" Value="8" ImageUrl=""></asp:MenuItem>
                <asp:MenuItem Text="Process" Value="9" ImageUrl=""></asp:MenuItem>
                <asp:MenuItem Text="Capital and Tooling" Value="10" ImageUrl=""></asp:MenuItem>
                <asp:MenuItem Text="Supporting Docs" Value="11" ImageUrl=""></asp:MenuItem>
                <asp:MenuItem Text="Approval Status" Value="12" ImageUrl=""></asp:MenuItem>
                <asp:MenuItem Text="Communication Board" Value="13" ImageUrl=""></asp:MenuItem>
            </Items>
        </asp:Menu>
        <asp:MultiView ID="mvTabs" runat="server" Visible="true" ActiveViewIndex="0" EnableViewState="true">
            <asp:View ID="vDesc" runat="server">
                <asp:ValidationSummary runat="server" ID="vsSaveDescription" ValidationGroup="vgSaveDescription"
                    ShowMessageBox="true" ShowSummary="true" />
                <table width="98%">
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblInitiatorMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="*" />
                            Initiator:
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddInitiator" runat="server">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="rfvInitiator" ControlToValidate="ddInitiator"
                                SetFocusOnError="true" ErrorMessage="Initiator is required" Text="<" ValidationGroup="vgSaveDescription" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblBusinessProcessTypeMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="*" />
                            Business Process Type:
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddBusinessProcessType" runat="server" AutoPostBack="true" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvBusinessProcesssType" ControlToValidate="ddBusinessProcessType"
                                SetFocusOnError="true" ErrorMessage="Business process type is required" Text="<"
                                ValidationGroup="vgSaveDescription" />
                            <asp:CheckBox runat="server" ID="cbAffectsCostSheetOnly" Text="Affects Cost Sheet Only"
                                AutoPostBack="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblBusinessProcessActionMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="*" Visible="false" />
                            <asp:Label ID="lblBusinessProcessAction" runat="server" Text="Type of action to the part:"
                                Visible="false" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddBusinessProcessAction" runat="server" Visible="false" AutoPostBack="true" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvBusinessProcessAction" ControlToValidate="ddBusinessProcessAction"
                                SetFocusOnError="true" ErrorMessage="Type of action to the part is required"
                                Text="<" ValidationGroup="vgSaveDescription" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblDesignationTypeMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="*" />
                            Designation Type:
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddDesignationType" AutoPostBack="true" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvDesignationType" ControlToValidate="ddDesignationType"
                                SetFocusOnError="true" ErrorMessage="Designation type is required" Text="<" ValidationGroup="vgSaveDescription" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblAccountManagerMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="*" />
                            <asp:Label ID="lblAccountManager" runat="server" Text="Account Manager:" />
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddAccountManager" runat="server" AutoPostBack="true" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvAccountManager" ControlToValidate="ddAccountManager"
                                SetFocusOnError="true" ErrorMessage="Account Manager is required" Text="<" ValidationGroup="vgSaveDescription" />
                        </td>
                    </tr>
                    <tr runat="server" id="tblProgramManager" visible="false">
                        <td class="p_text">
                            <asp:Label ID="lblProgramManagerMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="*" />
                            <asp:Label ID="lblProgramManager" runat="server" Text="Program Manager:" />
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddProgramManager" runat="server" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvProgramManager" ControlToValidate="ddProgramManager"
                                SetFocusOnError="true" ErrorMessage="Program Manager is required" Text="<" ValidationGroup="vgSaveDescription" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblWorkFlowMakeMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="*" Visible="false" />
                            <asp:Label runat="server" ID="lblWorkFlowMake" Text="Make:" Visible="false" />
                        </td>
                        <td class="c_textbold">
                            <asp:DropDownList ID="ddWorkFlowMake" runat="server" Visible="false" AutoPostBack="true" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvWorkFlowMake" ControlToValidate="ddWorkFlowMake"
                                SetFocusOnError="true" ErrorMessage="Make is required" Text="<" ValidationGroup="vgSaveDescription" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label runat="server" ID="lblPurchasingTeamMemberByMake" Text="Purchasing Team Member assigned to Make:"
                                Visible="false" />
                        </td>
                        <td class="c_textbold">
                            <asp:DropDownList ID="ddPurchasingTeamMemberByMake" runat="server" Visible="false" />
                            <asp:Label runat="server" ID="lblPurchasingTeamMemberByMakeTip" Text="Changing this team member will NOT override a previously selected team member on the approval tab."
                                Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" valign="top">
                            <asp:Label ID="lblWorkFlowCommodityMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="*" Visible="false" />
                            <asp:Label runat="server" ID="lblWorkFlowCommodity" Text="Commodity:" Visible="false" />
                        </td>
                        <td class="c_textbold" visible="true" valign="top">
                            <asp:DropDownList ID="ddWorkFlowCommodity" runat="server" Visible="false" AutoPostBack="true" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvWorkFlowCommodity" ControlToValidate="ddWorkFlowCommodity"
                                SetFocusOnError="true" ErrorMessage="Commodity is required" Text="<" ValidationGroup="vgSaveDescription" />
                            <br />
                            <asp:Label runat="server" ID="lblWorkFlowCommodityNote" Text="{Commodity / Classification}"
                                Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" valign="top">
                            <asp:Label runat="server" ID="lblProductDevelopmentTeamMemberByCommodity" Text="Product Development Team Member assigned to Commodity:"
                                Visible="false" />
                        </td>
                        <td class="c_textbold" valign="top">
                            <asp:DropDownList ID="ddProductDevelopmentTeamMemberByCommodity" runat="server" Visible="false" />
                            <asp:Label runat="server" ID="lblProductDevelopmentTeamMemberByCommodityTip" Text="Changing this team member will NOT override a previously selected team member on the approval tab."
                                Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblWorkflowFamilyMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="*" Visible="false" />
                            <asp:Label runat="server" ID="lblWorkflowFamily" Text="Family:" Visible="false" />
                        </td>
                        <td class="c_textbold">
                            <asp:DropDownList ID="ddWorkflowFamily" runat="server" Visible="false" AutoPostBack="true" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvWorkflowFamily" ControlToValidate="ddWorkflowFamily"
                                SetFocusOnError="true" ErrorMessage="Family is required" Text="<" ValidationGroup="vgSaveDescription" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label runat="server" ID="lblPurchasingTeamMemberByFamily" Text="Purchasing (Raw Material) Family:"
                                Visible="false" />
                        </td>
                        <td class="c_textbold">
                            <asp:DropDownList ID="ddPurchasingTeamMemberByFamily" runat="server" Visible="false" />
                            <asp:Label runat="server" ID="lblPurchasingTeamMemberByFamilyTip" Text="Changing this team member will NOT override a previously selected team member on the approval tab."
                                Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblPriceCodeMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="*" />
                            Price Code
                            <br />
                            (Production Status):
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddPriceCode" runat="server">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="rfvPriceCode" ControlToValidate="ddPriceCode"
                                SetFocusOnError="true" ErrorMessage="Price Code is required" Text="<" ValidationGroup="vgSaveDescription" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Priority:
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddPriority" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblDueDateMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="*" />
                            Due Date:
                        </td>
                        <td colspan="3">
                            <asp:TextBox runat="server" ID="txtDueDate" MaxLength="10" Width="100px" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvDueDate" ControlToValidate="txtDueDate"
                                SetFocusOnError="true" ErrorMessage="Due date is required" Text="<" ValidationGroup="vgSaveDescription" />
                            <asp:ImageButton runat="server" ID="imgDueDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                            <ajax:CalendarExtender ID="ceDueDate" runat="server" TargetControlID="txtDueDate"
                                PopupButtonID="imgDueDate" />
                            <asp:RegularExpressionValidator ID="revDueDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                ControlToValidate="txtDueDate" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                Width="8px" ValidationGroup="vgSaveDescription" Text="<"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Copy Reason:
                        </td>
                        <td colspan="3">
                            <asp:TextBox runat="server" ID="txtCopyReason" TextMode="MultiLine" Width="600px"
                                Height="80px" Font-Bold="true" />
                            <br />
                            <asp:Label ID="lblCopyReasonCharCount" SkinID="MessageLabelSkin" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Description:
                        </td>
                        <td colspan="3">
                            <asp:TextBox runat="server" ID="txtRFDDesc" TextMode="MultiLine" Width="600px" Height="80px" />
                            <br />
                            <asp:Label ID="lblRFDDescCharCount" SkinID="MessageLabelSkin" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            RFD Meeting Notes:<br />
                            (Impact On UGN):
                        </td>
                        <td colspan="3">
                            <asp:TextBox runat="server" ID="txtImpactOnUGN" TextMode="MultiLine" Width="600px"
                                Height="80px" />
                            <br />
                            <asp:Label ID="lblImpactOnUGNCharCount" SkinID="MessageLabelSkin" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Target Price:
                        </td>
                        <td colspan="3">
                            <asp:TextBox runat="server" ID="txtTargetPrice" MaxLength="10" />
                            <asp:RangeValidator ID="rvTargetPrice" runat="server" ControlToValidate="txtTargetPrice"
                                SetFocusOnError="true" Text="<" Display="Dynamic" ErrorMessage="Price requires a numeric value -99,999,999.99 to 99,999,999.99"
                                Height="16px" MaximumValue="99999999.99" MinimumValue="-99999999.99" Type="Currency"
                                ValidationGroup="vgSaveDescription"><</asp:RangeValidator>
                            <ajax:FilteredTextBoxExtender ID="ftbeTargetPrice" runat="server" TargetControlID="txtTargetPrice"
                                FilterType="Custom, Numbers" ValidChars="-.," />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Target Annual Volume:
                        </td>
                        <td colspan="3">
                            <asp:TextBox runat="server" ID="txtTargetAnnualVolume" MaxLength="10" />
                            <asp:CompareValidator runat="server" ID="cvTargetAnnualVolume" Operator="DataTypeCheck"
                                ValidationGroup="vgSaveDescription" Type="integer" Text="<" ControlToValidate="txtTargetAnnualVolume"
                                ErrorMessage="Target annual volume must be an integer." SetFocusOnError="True" />
                            &nbsp;
                            <asp:Button runat="server" ID="btnCalculateTargetAnnualSales" Text="Calculate Sales"
                                CausesValidation="true" ValidationGroup="vgSaveDescription" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Target Annual Sales:
                        </td>
                        <td colspan="3">
                            <asp:TextBox runat="server" ID="txtTargetAnnualSales" MaxLength="15" />
                            <asp:RangeValidator ID="rvTargetAnnualSales" runat="server" ControlToValidate="txtTargetAnnualSales"
                                SetFocusOnError="true" Text="<" Display="Dynamic" ErrorMessage="Target Annual Sales requires a numeric value -99,999,999.99 to 99,999,999.99"
                                Height="16px" MaximumValue="99999999.99" MinimumValue="-99999999.99" Type="Currency"
                                ValidationGroup="vgSaveDescription"><</asp:RangeValidator>
                            <ajax:FilteredTextBoxExtender ID="ftbeTargetAnnualSales" runat="server" TargetControlID="txtTargetAnnualSales"
                                FilterType="Custom, Numbers" ValidChars="-.," />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" style="white-space: nowrap;">
                            Packaging required:
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="cbPackagingRequired" />
                        </td>
                        <td class="p_text" style="white-space: nowrap;">
                            Finance (Corporate/Plant Controller) required:
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="cbPlantControllerRequired" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" style="white-space: nowrap;">
                            Process required:
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="cbProcessRequired" />
                        </td>
                        <td class="p_text">
                            Tooling required:
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="cbToolingRequired" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Costing required:
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="cbCostingRequired" />
                        </td>
                        <td class="p_text">
                            Capital required:
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="cbCapitalRequired" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" style="white-space: nowrap;">
                            Product Engineering required:
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="cbProductDevelopmentRequired" />
                        </td>
                        <td class="p_text" style="white-space: nowrap;">
                            Purchasing for External RFQ:
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="cbPurchasingExternalRFQRequired" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Quality Engineering required:
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="cbQualityEngineeringRequired" />
                        </td>
                        <td class="p_text">
                            Purchasing for Contract / P.O. required:
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="cbPurchasingRequired" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Research and Development required:
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="cbRDrequired" />
                            <i>(CC Notification Only - No approval needed)</i>
                        </td>
                        <td class="p_text" style="white-space: nowrap;">
                            DVPR Document required:
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="cbDVPRrequired" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Label runat="server" ID="lblMessageDescription" SkinID="MessageLabelSkin"></asp:Label>
                <br />
                <table width="100%">
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Button ID="btnSaveDescription" runat="server" Text="Save" ValidationGroup="vgSaveDescription"
                                CausesValidation="true" />
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vKit" runat="server">
                <asp:Label runat="server" ID="lblMessageKIT" SkinID="MessageLabelSkin"></asp:Label>
                <asp:ValidationSummary runat="server" ID="vsEditKit" ValidationGroup="vgEditKit"
                    ShowMessageBox="true" ShowSummary="true" />
                <asp:ValidationSummary runat="server" ID="vsInsertKit" ValidationGroup="vgInsertKit"
                    ShowMessageBox="true" ShowSummary="true" />
                <br />
                <asp:GridView ID="gvKit" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                    AllowPaging="True" PageSize="15" DataKeyNames="RowID" DataSourceID="odsKit" EmptyDataText="No records found"
                    Width="98%" ShowFooter="true">
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
                        <asp:BoundField DataField="RFDNo">
                            <ItemStyle CssClass="none" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Kit Part No" SortExpression="ddKitPartNo">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditKitPartNo" runat="server" Text='<%# Bind("KitPartNo") %>'
                                    MaxLength="15" Width="140px" />
                                <asp:ImageButton ID="iBtnEditSearchKitPartNo" runat="server" CommandName="Insert"
                                    CausesValidation="False" ImageUrl="~/images/Search.gif" ToolTip="Fill in Part No if known"
                                    AlternateText="Search Kit PartNo" ValidationGroup="vgEditKit" />
                                <asp:RequiredFieldValidator ID="rfvEditKitPartNo" runat="server" ControlToValidate="txtEditKitPartNo"
                                    ErrorMessage="Kit PartNo is required." Font-Bold="True" ValidationGroup="vgEditKit"
                                    Text="<" SetFocusOnError="true">				                                                            
                                </asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewKitPartNo" runat="server" Text='<%# Bind("ddKitPartNo") %>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtInsertKitPartNo" runat="server" MaxLength="15" Width="140px" />
                                <asp:ImageButton ID="iBtnInsertSearchKitPartNo" runat="server" CausesValidation="False"
                                    ImageUrl="~/images/Search.gif" ToolTip="Fill in Part No if known" AlternateText="Search Kit PartNo" />
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
                                <asp:TextBox ID="txtEditKitPartRevision" runat="server" Text='<%# Bind("KitPartRevision") %>'
                                    MaxLength="2" Width="20px" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewKitPartRevision" runat="server" Text='<%# Bind("KitPartRevision") %>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtInsertKitPartRevision" runat="server" MaxLength="2" Width="20px" />
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                            <FooterStyle HorizontalAlign="Left" Wrap="False" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="KitPartName" ReadOnly="true"></asp:BoundField>
                        <asp:TemplateField HeaderText="Finished Good&lt;br/&gt; Part No" SortExpression="ddFinishedGoodPartNo">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditFinishedGoodPartNo" runat="server" Text='<%# Bind("FinishedGoodPartNo") %>'
                                    MaxLength="15" Width="140px" />
                                <asp:ImageButton ID="iBtnEditSearchFinishedGoodPartNo" runat="server" CausesValidation="False"
                                    ImageUrl="~/images/Search.gif" ToolTip="Fill in Part No if known" AlternateText="Search FinishedGood Part No" />
                                <asp:RequiredFieldValidator ID="rfvEditFinishedGoodPartNo" runat="server" ControlToValidate="txtEditFinishedGoodPartNo"
                                    ErrorMessage="FinishedGood Part No is required." Font-Bold="True" ValidationGroup="vgEditKit"
                                    Text="<" SetFocusOnError="true">				                                                            
                                </asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewFinishedGoodPartNo" runat="server" Text='<%# Bind("ddFinishedGoodPartNo") %>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtInsertFinishedGoodPartNo" runat="server" MaxLength="15" Width="140px" />
                                <asp:ImageButton ID="iBtnInsertSearchFinishedGoodPartNo" runat="server" CausesValidation="False"
                                    ImageUrl="~/images/Search.gif" ToolTip="Fill in Part No if known" AlternateText="Search FinishedGood Part No" />
                                <asp:RequiredFieldValidator ID="rfvInsertFinishedGoodPartNo" runat="server" ControlToValidate="txtInsertFinishedGoodPartNo"
                                    ErrorMessage="FinishedGood Part No is required." Font-Bold="True" ValidationGroup="vgInsertKit"
                                    Text="<" SetFocusOnError="true">				                                                            
                                </asp:RequiredFieldValidator>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                            <FooterStyle HorizontalAlign="Left" Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rev" SortExpression="FinishedGoodPartRevision">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditFinishedGoodPartRevision" runat="server" Text='<%# Bind("FinishedGoodPartRevision") %>'
                                    MaxLength="2" Width="20px" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewFinishedGoodPartRevision" runat="server" Text='<%# Bind("FinishedGoodPartRevision") %>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtInsertFinishedGoodPartRevision" runat="server" MaxLength="2"
                                    Width="20px" />
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="FinishedGoodPartName" ReadOnly="true"></asp:BoundField>
                        <asp:TemplateField>
                            <EditItemTemplate>
                                <asp:ImageButton ID="iBtnUpdateKit" runat="server" CommandName="Update" ImageUrl="~/images/save.jpg"
                                    AlternateText="Update" CausesValidation="true" ValidationGroup="vgEditKit" />
                                <asp:ImageButton ID="iBtnCancelEdit" runat="server" CausesValidation="False" CommandName="Cancel"
                                    ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="iBtnKitEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                    ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
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
                    SelectMethod="GetRFDKit" TypeName="RFDKitBLL" DeleteMethod="DeleteRFDKit" InsertMethod="InsertRFDKit"
                    UpdateMethod="UpdateRFDKit">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="RFDNo" QueryStringField="RFDNo" Type="Int32" />
                    </SelectParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="RowID" Type="Int32" />
                        <asp:Parameter Name="original_RowID" Type="Int32" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="original_RowID" Type="Int32" />
                        <asp:Parameter Name="RFDNo" Type="Int32" />
                        <asp:Parameter Name="KitPartNo" Type="String" />
                        <asp:Parameter Name="KitPartRevision" Type="String" />
                        <asp:Parameter Name="FinishedGoodPartNo" Type="String" />
                        <asp:Parameter Name="FinishedGoodPartRevision" Type="String" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:Parameter Name="RFDNo" Type="Int32" />
                        <asp:Parameter Name="KitPartNo" Type="String" />
                        <asp:Parameter Name="KitPartRevision" Type="String" />
                        <asp:Parameter Name="FinishedGoodPartNo" Type="String" />
                        <asp:Parameter Name="FinishedGoodPartRevision" Type="String" />
                    </InsertParameters>
                </asp:ObjectDataSource>
            </asp:View>
            <asp:View ID="vCustomerPartNo" runat="server">
                <asp:ValidationSummary runat="server" ID="vsSaveCustomerPartNo" ValidationGroup="vgSaveCustomerPartNo"
                    ShowMessageBox="true" ShowSummary="true" />
                <br />
                <asp:Label runat="server" ID="lblMessageCustomerPartNo" SkinID="MessageLabelSkin" />
                <br />
                <table width="80%" border="1" cellpadding="1" cellspacing="1" style="border-color: Navy"
                    runat="server" id="tblCustomerPart" visible="false">
                    <tr>
                        <td class="p_bigtextbold" align="center" style="background-color: #DEDEDE; width: 583px;">
                            <asp:Label runat="server" ID="lblCurrentCustomerPartTitle" Text="CURRENT Customer Part"
                                Visible="false" />
                        </td>
                        <td class="p_bigtextbold" align="center" style="background-color: Aqua">
                            <asp:Label runat="server" ID="lblNewCustomerPartTitle" Text="NEW Customer Part" />
                        </td>
                    </tr>
                    <tr valign="top">
                        <td style="background-color: #DEDEDE; width: 583px;" valign="top">
                            <table runat="server" id="tblCurrentCustomerPart" width="100%">
                                <tr style="height: 25px">
                                    <td class="p_text" style="white-space: nowrap;">
                                        <asp:Label runat="server" ID="lblCurrentCustomerPartNo" Text="Customer Part No:"
                                            Visible="false" />
                                    </td>
                                    <td class="c_textbold">
                                        <asp:TextBox runat="server" ID="txtCurrentCustomerPartNo" MaxLength="40" Visible="false"
                                            Width="200px" />
                                        <asp:ImageButton ID="iBtnCurrentCustomerPartNoSearch" runat="server" ImageUrl="~/images/Search.gif"
                                            Visible="false" ToolTip="Click here to search for the current customer part number." />
                                        <asp:ImageButton ID="iBtnCurrentShipHistoryCopy" runat="server" ImageUrl="~/images/SelectUser.gif"
                                            ToolTip="Click here to copy details based on shipping history and the Customer PartNo-F.G. PartNo cross reference"
                                            Visible="false" />
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td class="p_text">
                                        <asp:Label runat="server" ID="lblCurrentDesignLevel" Text="Design Level:" Visible="false" />
                                    </td>
                                    <td class="c_textbold">
                                        <asp:TextBox runat="server" ID="txtCurrentDesignLevel" MaxLength="30" Visible="false"
                                            Width="200px" />
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td class="p_text">
                                        <asp:Label runat="server" ID="lblCurrentCustomerDrawingNo" Text="Customer Drawing No:"
                                            Visible="false" />
                                    </td>
                                    <td class="c_textbold" style="white-space: nowrap;">
                                        <asp:TextBox runat="server" ID="txtCurrentCustomerDrawingNo" MaxLength="30" Visible="false"
                                            Width="200px" />
                                        <asp:HyperLink runat="server" ID="hlnkCurrentCustomerDrawingNo" Font-Underline="true"
                                            ToolTip="Click here to view the current Customer Drawing." Text="View CAD Image"
                                            Target="_blank" />
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td class="p_text">
                                        <asp:Label runat="server" ID="lblCurrentCustomerPartName" Text="Customer Part Name:"
                                            Visible="false" />
                                    </td>
                                    <td class="c_textbold" style="white-space: nowrap;">
                                        <asp:TextBox runat="server" ID="txtCurrentCustomerPartName" MaxLength="30" Visible="false"
                                            Width="300px" />
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td class="p_text">
                                        <asp:Label runat="server" ID="lblCurrentDrawingNo" Text="DMS Drawing No:" Visible="false" />
                                    </td>
                                    <td class="c_textbold" style="white-space: nowrap;">
                                        <asp:TextBox runat="server" ID="txtCurrentDrawingNo" MaxLength="18" Visible="false"
                                            Width="200px" />
                                        <asp:ImageButton ID="iBtnCurrentDrawingSearch" runat="server" ImageUrl="~/images/Search.gif"
                                            ToolTip="Click here to search for a DMS Drawing." Visible="false" />
                                        <asp:HyperLink runat="server" ID="hlnkCurrentDrawingNo" Font-Underline="true" ToolTip="Click here to view the current DMS Drawing."
                                            Text="View" Target="_blank" />
                                        <asp:ImageButton ID="iBtnCurrentDrawingCopy" runat="server" ImageUrl="~/images/SelectUser.gif"
                                            ToolTip="Click here to copy details based on the current DMS Drawing." Visible="false" />
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td class="p_text" valign="top">
                                        <asp:Label runat="server" ID="lblCurrentCommodity" Text="Commodity:" Visible="false" />
                                    </td>
                                    <td class="c_textbold" valign="top">
                                        <asp:DropDownList ID="ddCurrentCommodity" runat="server" Visible="false" />
                                        <br />
                                        <asp:Label runat="server" ID="lblCurrentCommodityNote" Text="{Commodity / Classification}"
                                            Visible="false" Font-Size="Smaller" />
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td class="p_text">
                                        <asp:Label runat="server" ID="lblCurrentProductTechnology" Text="Product Technology:"
                                            Visible="false" />
                                    </td>
                                    <td class="c_textbold">
                                        <asp:DropDownList ID="ddCurrentProductTechnology" runat="server" Visible="false" />
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table width="100%">
                                <tr style="height: 25px">
                                    <td class="p_text" style="white-space: nowrap;">
                                        <asp:Label runat="server" Text="* " ForeColor="Red" />
                                        <asp:Label runat="server" ID="lblNewCustomerPartNo" Text="New Customer Part No:"
                                            Visible="true" />
                                    </td>
                                    <td class="c_textbold">
                                        <asp:TextBox runat="server" ID="txtNewCustomerPartNo" MaxLength="20" Visible="true"
                                            Height="22px" Width="200px" />
                                        <br />
                                        &nbsp;<asp:Label runat="server" Text="(often received from the customer)" Font-Size="Smaller" />
                                        <ajax:FilteredTextBoxExtender ID="ftbNewCustomerPartNo" runat="server" TargetControlID="txtNewCustomerPartNo"
                                            FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,- " />
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td class="p_text">
                                        <asp:Label runat="server" ID="lblNewDesignLevel" Text="New Design Level:" />
                                    </td>
                                    <td class="c_textbold">
                                        <asp:TextBox runat="server" ID="txtNewDesignLevel" MaxLength="30" Width="200px" />
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td class="p_text">
                                        <asp:Label runat="server" ID="lblNewCustomerDrawingNo" Text="Customer Drawing No:" />
                                    </td>
                                    <td class="c_textbold" style="white-space: nowrap;">
                                        <asp:TextBox runat="server" ID="txtNewCustomerDrawingNo" MaxLength="18" Width="200px" />
                                        <asp:HyperLink runat="server" ID="hlnkNewCustomerDrawingNo" Font-Underline="true"
                                            ToolTip="Click here to view the New Customer Drawing." Text="View CAD Image"
                                            Target="_blank" Visible="false" />
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td class="p_text">
                                        <asp:Label runat="server" ID="lblNewCustomerPartName" Text="Customer Part Name:" />
                                    </td>
                                    <td class="c_textbold" style="white-space: nowrap;">
                                        <asp:TextBox runat="server" ID="txtNewCustomerPartName" MaxLength="30" Width="300px" />
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td class="p_text">
                                        DMS Drawing No:
                                    </td>
                                    <td class="c_textbold" style="white-space: nowrap;">
                                        <asp:TextBox runat="server" ID="txtNewDrawingNo" MaxLength="18" Width="200px"  AutoPostBack="true"/><asp:TextBox
                                            runat="server" ID="txtHDNewDrawingNo"  Width="20px" Visible="false" />
                                        <asp:ImageButton ID="iBtnNewDrawingSearch" runat="server" ImageUrl="~/images/Search.gif"
                                            ToolTip="Click here to search for a DMS Drawing." />
                                        <asp:HyperLink runat="server" ID="hlnkNewDrawingNo" Visible="false" Font-Underline="true"
                                            ToolTip="Click here to view the new DMS Drawing." Text="View Drawing" Target="_blank" />
                                        <asp:ImageButton ID="iBtnNewDrawingCopy" runat="server" ImageUrl="~/images/SelectUser.gif"
                                            ToolTip="Click here to copy details based on the new DMS Drawing if it exists."
                                            Visible="false" />
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td class="p_text">
                                        Cost Sheet ID:
                                    </td>
                                    <td class="c_textbold">
                                        <asp:TextBox runat="server" ID="txtNewCostSheetID" MaxLength="10" Width="200px" />
                                        <asp:CompareValidator runat="server" ID="cvNewCostSheetID" Operator="DataTypeCheck"
                                            ValidationGroup="vgSaveCustomerPartNo" Type="integer" Text="<" ControlToValidate="txtNewCostSheetID"
                                            ErrorMessage="Cost Sheet ID must be an integer." SetFocusOnError="True" />
                                        <asp:HyperLink runat="server" ID="hlnkNewCostSheetID" Visible="false" Font-Underline="true"
                                            ToolTip="Click here to view the new Cost Sheet." Text="View Cost Sheet" Target="_blank" />
                                        <asp:HyperLink runat="server" ID="hlnkNewDieLayout" Visible="false" Font-Underline="true"
                                            ToolTip="Click here to view the new Die Layout." Text="View Die Layout" Target="_blank" />
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td class="p_text">
                                        ECI No.:
                                    </td>
                                    <td class="c_textbold">
                                        <asp:TextBox runat="server" ID="txtNewECINo" MaxLength="10" Width="200px" />
                                        <asp:ImageButton ID="iBtnNewECINoSearch" runat="server" ImageUrl="~/images/Search.gif"
                                            ToolTip="Click here to add or search for an ECI." />
                                        <asp:CompareValidator runat="server" ID="cvNewECINo" Operator="DataTypeCheck" ValidationGroup="vgSaveCustomerPartNo"
                                            Type="integer" Text="<" ControlToValidate="txtNewECINo" ErrorMessage="ECI number must be an integer."
                                            SetFocusOnError="True" />
                                        <asp:HyperLink runat="server" ID="hlnkNewECINo" Visible="false" Font-Underline="true"
                                            ToolTip="Click here to view the new ECI." Text="View ECI" Target="_blank" />
                                        <asp:CheckBox runat="server" ID="cbNewECIOverrideNA" Text="N/A" />
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td class="p_text">
                                        CapEx Project No.:
                                    </td>
                                    <td class="c_textbold">
                                        <asp:TextBox runat="server" ID="txtNewCapExProjectNo" MaxLength="15" />
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td class="p_text">
                                        PO. No.:
                                    </td>
                                    <td class="c_textbold">
                                        <asp:TextBox runat="server" ID="txtNewPONo" MaxLength="15" />
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td class="p_text" valign="top">
                                        <asp:Label runat="server" ID="lblNewCommodity" Text="Commodity:" />
                                    </td>
                                    <td class="c_textbold" visible="true" valign="top">
                                        <asp:DropDownList ID="ddNewCommodity" runat="server" AutoPostBack="true" />
                                        <br />
                                        <asp:Label runat="server" ID="lblNewCommodityNote" Text="{Commodity / Classification}"
                                            Font-Size="Smaller" />
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td class="p_text">
                                        <asp:Label runat="server" ID="lblNewProductTechnology" Text="Product Technology:" />
                                    </td>
                                    <td class="c_textbold">
                                        <asp:DropDownList ID="ddNewProductTechnology" runat="server" />
                                    </td>
                                </tr>
                                <tr style="height: 25px">
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Label runat="server" ID="lblMessageCustomerPartNoMiddle" SkinID="MessageLabelSkin" /><br />
                            <asp:Button ID="btnSaveCustomerPartNo" runat="server" Text="Save Customer Part Info"
                                CausesValidation="true" ValidationGroup="vgSaveCustomerPartNo" />
                        </td>
                    </tr>
                </table>
                <ajax:Accordion ID="acFinishedGoodMeasurements" runat="server" SelectedIndex="0"
                    HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                    ContentCssClass="accordionContent" FadeTransitions="false" FramesPerSecond="20"
                    TransitionDuration="250" AutoSize="None" RequireOpenedPane="false" SuppressHeaderPostbacks="true"
                    Visible="false">
                    <Panes>
                        <ajax:AccordionPane ID="aFinishedGoodMeasurements" runat="server">
                            <Header>
                                <a href="">Edit Finished Good Details / Measurements</a></Header>
                            <Content>
                                <asp:ValidationSummary runat="server" ID="vsGenerateFGDrawing" ValidationGroup="vgGenerateFGDrawing"
                                    ShowMessageBox="true" ShowSummary="true" />
                                <br />
                                <asp:Label runat="server" ID="lblMessageFG" SkinID="MessageLabelSkin" />
                                <br />
                                <table width="80%" border="1" cellpadding="1" cellspacing="1" style="border-color: Navy">
                                    <tr>
                                        <td class="p_bigtextbold" align="center" style="background-color: #DEDEDE">
                                            <asp:Label runat="server" ID="lblCurrentCustomerPartMeasurementsTitle" Text="CURRENT Customer Part"
                                                Visible="false" />
                                        </td>
                                        <td class="p_bigtextbold" align="center" style="background-color: Aqua">
                                            NEW Customer Part
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td style="background-color: #DEDEDE" valign="top">
                                            <table runat="server" id="tblCurrentFGMeasurements" visible="false">
                                                <tr>
                                                    <td align="right" colspan="2">
                                                        <asp:Button runat="server" ID="btnCurrentFGCopyAll" Text="Copy all fields below to the new part >>"
                                                            Visible="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Current Initial Dimension and Density:
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtCurrentFGInitialDimensionAndDensity" runat="server" MaxLength="2"
                                                            Width="25px" Enabled="false">00</asp:TextBox>
                                                        <asp:Button runat="server" ID="btnCurrentFGCopyInitialDimensionAndDensity" Text=">>" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Current Process Step No.:
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtCurrentFGInStepTracking" runat="server" MaxLength="1" Width="25px"
                                                            Enabled="false" />
                                                        <asp:Button runat="server" ID="btnCurrentFGCopyInStepTracking" Text=">>" Visible="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Current AMD Value:
                                                    </td>
                                                    <td style="white-space: nowrap;">
                                                        <asp:TextBox runat="server" ID="txtCurrentFGAMDValue" MaxLength="7" Width="50px"
                                                            Enabled="false" />
                                                        &nbsp;
                                                        <asp:TextBox runat="server" ID="txtCurrentFGAMDTolerance" MaxLength="7" Enabled="false"
                                                            Width="50px" />
                                                        &nbsp;
                                                        <asp:DropDownList runat="server" ID="ddCurrentFGAMDUnits" Width="50px" Enabled="false">
                                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                                            <asp:ListItem Value="m" Text="m"></asp:ListItem>
                                                            <asp:ListItem Value="mm" Text="mm"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:Button runat="server" ID="btnCurrentFGCopyAMD" Text=">>" Visible="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Current WMD Value:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtCurrentFGWMDValue" MaxLength="7" Width="50px"
                                                            Enabled="false" />
                                                        &nbsp;
                                                        <asp:TextBox runat="server" ID="txtCurrentFGWMDTolerance" MaxLength="7" Enabled="false"
                                                            Width="50px" />
                                                        &nbsp;
                                                        <asp:DropDownList runat="server" ID="ddCurrentFGWMDUnits" Width="50px" Enabled="false">
                                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                                            <asp:ListItem Value="m" Text="m"></asp:ListItem>
                                                            <asp:ListItem Value="mm" Text="mm"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:Button runat="server" ID="btnCurrentFGCopyWMD" Text=">>" Visible="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Current Density Value:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtCurrentFGDensityValue" MaxLength="7" Enabled="false"
                                                            Width="50px" />
                                                        &nbsp;
                                                        <asp:TextBox runat="server" ID="txtCurrentFGDensityTolerance" MaxLength="7" Enabled="false"
                                                            Width="50px" />
                                                        &nbsp;
                                                        <asp:TextBox runat="server" ID="txtCurrentFGDensityUnits" MaxLength="7" Enabled="false"
                                                            Width="50px" />
                                                        <asp:Button runat="server" ID="btnCurrentFGCopyDensity" Text=">>" Visible="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Current Construction:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtCurrentFGConstruction" MaxLength="400" Enabled="false"
                                                            TextMode="MultiLine" Height="100px" />
                                                        <asp:Button runat="server" ID="btnCurrentFGCopyConstruction" Text=">>" Visible="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Current Drawing Notes:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtCurrentFGDrawingNotes" MaxLength="400" Enabled="false"
                                                            TextMode="MultiLine" Height="100px" />
                                                        <asp:Button runat="server" ID="btnCurrentFGCopyNotes" Text=">>" Visible="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Current Family:
                                                    </td>
                                                    <td class="c_textbold">
                                                        <asp:DropDownList ID="ddCurrentFGFamily" runat="server" Enabled="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="c_text" colspan="2">
                                                        Current SubFamily:
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="c_textbold" colspan="2" style="white-space: nowrap">
                                                        <asp:DropDownList ID="ddCurrentFGSubFamily" runat="server" Enabled="false" />
                                                        <asp:Button runat="server" ID="btnCurrentFGCopySubFamily" Text=">>" Visible="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td valign="top">
                                            <table>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        <asp:Label ID="lblNewFGInitialDimensionAndDensityMarker" runat="server" Font-Bold="True"
                                                            ForeColor="Red" Text="*" />
                                                        New Initial Dimension and Density:
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtNewFGInitialDimensionAndDensity" runat="server" MaxLength="2"
                                                            Width="25px" Enabled="false">00</asp:TextBox>
                                                        &nbsp;<i>**Inserted into the Drawing No.</i>
                                                        <asp:RequiredFieldValidator ID="rfvNewFGInitialDimensionAndDensity" runat="server"
                                                            ControlToValidate="txtNewFGInitialDimensionAndDensity" Text="<" ErrorMessage="Initial Dimension And Density Digits are required."
                                                            SetFocusOnError="true" ValidationGroup="vgGenerateFGDrawing">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:CompareValidator runat="server" ID="cvtNewFGInitialDimensionAndDensity" Operator="DataTypeCheck"
                                                            ValidationGroup="vgGenerateFGDrawing" Type="integer" Text="<" ControlToValidate="txtNewFGInitialDimensionAndDensity"
                                                            ErrorMessage="Initial Dimension And Density Digits must be integers." SetFocusOnError="True" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        <asp:Label ID="lblNewFGInStepTrackingNoMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                                            Text="*" />
                                                        New Process Step No.:
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtNewFGInStepTracking" runat="server" MaxLength="1" Width="25px"
                                                            Enabled="false" />
                                                        &nbsp;<i>**Inserted into the Drawing No.</i>
                                                        <asp:RequiredFieldValidator ID="rfvNewFGInStepTracking" CssClass="p_text" runat="server"
                                                            Display="Dynamic" ControlToValidate="txtNewFGInStepTracking" SetFocusOnError="True"
                                                            ErrorMessage="Process Number is required." Text="<" ValidationGroup="vgGenerateFGDrawing">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:CompareValidator runat="server" ID="cvNewFGInstepTracking" Operator="DataTypeCheck"
                                                            Type="Integer" Text="<" ErrorMessage="Process Step must be an integer." ControlToValidate="txtNewFGInStepTracking"
                                                            SetFocusOnError="True" ValidationGroup="vgGenerateFGDrawing" />
                                                        <asp:RegularExpressionValidator ID="revNewFGProcessStepNumber" runat="server" ControlToValidate="txtNewFGInStepTracking"
                                                            Text="<" ErrorMessage="Value Must be 1 through 9" SetFocusOnError="True" ValidationExpression="[1-9]"
                                                            ValidationGroup="vgGenerateFGDrawing">
                                                        </asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        New AMD Value:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtNewFGAMDValue" MaxLength="10" Width="50px" />
                                                        <asp:CompareValidator runat="server" ID="cvNewFGAMDValue" Operator="DataTypeCheck"
                                                            ValidationGroup="vgGenerateFGDrawing" Type="double" Text="<" ControlToValidate="txtNewFGAMDValue"
                                                            ErrorMessage="AMD must be a number" SetFocusOnError="True" />
                                                        &nbsp;
                                                        <asp:TextBox runat="server" ID="txtNewFGAMDTolerance" MaxLength="10" Width="50px" />
                                                        &nbsp;
                                                        <asp:DropDownList runat="server" ID="ddNewFGAMDUnits" Width="50px">
                                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                                            <asp:ListItem Value="m" Text="m"></asp:ListItem>
                                                            <asp:ListItem Value="mm" Text="mm"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        New WMD Value:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtNewFGWMDValue" MaxLength="10" Width="50px" />
                                                        <asp:CompareValidator runat="server" ID="cvNewFGWMDValue" Operator="DataTypeCheck"
                                                            ValidationGroup="vgGenerateFGDrawing" Type="double" Text="<" ControlToValidate="txtNewFGWMDValue"
                                                            ErrorMessage="WMD must be a number" SetFocusOnError="True" />
                                                        &nbsp;
                                                        <asp:TextBox runat="server" ID="txtNewFGWMDTolerance" MaxLength="10" Width="50px" />
                                                        &nbsp;
                                                        <asp:DropDownList runat="server" ID="ddNewFGWMDUnits" Width="50px">
                                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                                            <asp:ListItem Value="m" Text="m"></asp:ListItem>
                                                            <asp:ListItem Value="mm" Text="mm"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        New Density Value:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtNewFGDensityValue" MaxLength="10" Width="50px" />
                                                        <asp:CompareValidator runat="server" ID="cvNewFGDensityValue" Operator="DataTypeCheck"
                                                            ValidationGroup="vgGenerateFGDrawing" Type="double" Text="<" ControlToValidate="txtNewFGDensityValue"
                                                            ErrorMessage="Density must be a number" SetFocusOnError="True" />
                                                        &nbsp;
                                                        <asp:TextBox runat="server" ID="txtNewFGDensityTolerance" MaxLength="10" Width="50px" />
                                                        &nbsp;
                                                        <asp:TextBox runat="server" ID="txtNewFGDensityUnits" MaxLength="10" Width="50px" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <tr>
                                                        <td class="p_text">
                                                            New Construction:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtNewFGConstruction" MaxLength="100" TextMode="MultiLine"
                                                                Height="100px" />
                                                            <br />
                                                            <asp:Label ID="lblNewFGConstructionCharCount" SkinID="MessageLabelSkin" runat="server" />
                                                        </td>
                                                    </tr>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        New Drawing Notes:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtNewFGDrawingNotes" MaxLength="100" TextMode="MultiLine"
                                                            Height="100px" />
                                                        <br />
                                                        <asp:Label ID="lblNewFGDrawingNotesCharCount" SkinID="MessageLabelSkin" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        New Family:
                                                    </td>
                                                    <td class="c_textbold">
                                                        <asp:DropDownList ID="ddNewFGFamily" runat="server" Enabled="false" AutoPostBack="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="c_text" colspan="2">
                                                        New SubFamily:
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="c_textbold" colspan="2">
                                                        <asp:DropDownList ID="ddNewFGSubFamily" runat="server" ValidationGroup="vgGenerateFGDrawing" />
                                                        <asp:RequiredFieldValidator ID="rfvNewFGSubFamily" CssClass="p_text" runat="server"
                                                            Display="Dynamic" ControlToValidate="ddNewFGSubFamily" SetFocusOnError="True"
                                                            ErrorMessage="SubFamily is required." Text="<" ValidationGroup="vgGenerateFGDrawing">
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2">
                                                        <asp:Button runat="server" ID="btnSaveFGMeasurements" Text="Save New Measurements"
                                                            Width="250px" CausesValidation="true" ValidationGroup="vgSaveDescription" Visible="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2">
                                                        <asp:Button runat="server" ID="btnGenerateNewFGDrawing" Text="Generate New DMS Drawing"
                                                            Width="250px" CausesValidation="true" ValidationGroup="vgGenerateFGDrawing" Visible="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2">
                                                        <asp:RadioButtonList runat="server" ID="rbGenerateNewFGDrawing" RepeatDirection="Horizontal">
                                                            <asp:ListItem Text="New Part" Value="N"></asp:ListItem>
                                                            <asp:ListItem Text="New Revision" Value="R" Selected="True"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2">
                                                        <asp:Label runat="server" ID="lblMessageFGBottom" SkinID="MessageLabelSkin" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </Content>
                        </ajax:AccordionPane>
                    </Panes>
                </ajax:Accordion>
                <br />
                <asp:Label runat="server" ID="lblMessageCustomerPartNoBottom" SkinID="MessageLabelSkin"></asp:Label><br />
                <%-- (LREY 01/22/2014) 
               <br /> <asp:Label runat="Server" ID="lblTitleCurrentFinisedGood" Text="Below is the list of Current Finished Good Part No(s) based on CURRENT Customer Part No above."
                    Visible="false"></asp:Label>
                <br />
                <asp:GridView ID="gvCurrentFinishedGood" runat="server" AutoGenerateColumns="False"
                    DataKeyNames="CustomerPartNo" DataSourceID="odsCurrentFinishedGood" EmptyDataText="No Finished Goods exist for the current Customer Part No above."
                    AllowSorting="True" AllowPaging="True" PageSize="15" Width="98%">
                    <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#CCCCCC" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="PartNo" HeaderText="Current Internal Part No (FG)" SortExpression="PartNo" />
                        <asp:BoundField DataField="CustomerPartName" HeaderText="Name" SortExpression="PartName" />
                        <asp:TemplateField HeaderText="Copy Customer Information if exists" ShowHeader="true">
                            <ItemTemplate>
                                <asp:ImageButton ID="ibtnCopy" runat="server" CommandName="Select" ImageUrl="~/images/SelectUser.gif"
                                    AlternateText="Select" />&nbsp;&nbsp;&nbsp;
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsCurrentFinishedGood" runat="server" SelectMethod="GetCustomerPartPartRelate"
                    TypeName="commonFunctions">
                    <SelectParameters>
                        <asp:Parameter Name="PartNo" Type="String" />
                        <asp:ControlParameter ControlID="txtCurrentCustomerPartNo" Name="customerPartNo"
                            PropertyName="Text" Type="String" DefaultValue="99999999999999999999" />
                        <asp:Parameter Name="customerPartName" Type="String" />
                        <asp:Parameter Name="cabbv" Type="String" />
                        <asp:Parameter Name="barCodePartNo" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <br />
                <asp:Label runat="server" ID="lblFinishedGoodTip" Text="AT LEASET ONE Finished Good Part No(s) MUST BE ASSIGNED in order for the RFD to be closed. However, team members still may submit this for approval without prior knowledge of what the new part numbers should be."
                    Font-Italic="true" Font-Size="XX-Small" Visible="false"></asp:Label>
                <br />
                <table width="98%">
                    <tr>
                        <td>
                            <asp:Button ID="btnGenerateFGPartNo" runat="server" Text="Part No Wizard" ToolTip="Assign one or many Finished Good Part No(s) to Customer Part No" />
                        </td>
                    </tr>
                </table>
                <br />
                <ajax:Accordion ID="acFinishedGood" runat="server" SelectedIndex="-1" HeaderCssClass="accordionHeader"
                    HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
                    FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
                    RequireOpenedPane="false" SuppressHeaderPostbacks="true" Visible="false">
                    <Panes>
                        <ajax:AccordionPane ID="apFinishedGood" runat="server">
                            <Header>
                                <a href="">Add / Edit Finished Good Part No(s) (usually by Quality Engineers) </a>
                            </Header>
                            <Content>
                                <asp:ValidationSummary runat="server" ID="vsFinishedGood" ValidationGroup="vgFinishedGood"
                                    ShowMessageBox="true" ShowSummary="true" />
                                <table>
                                    <tr>
                                        <td class="p_text">
                                            New Finished Good Part No.:
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtFGPartNo" MaxLength="40" Width="200"/>
                                            <asp:ImageButton ID="iBtnFGPartNoSearch" runat="server" ImageUrl="~/images/Search.gif"
                                                ToolTip="Click here to search for a PartNo. (if exists)" Visible="true" />
                                            <asp:RequiredFieldValidator runat="server" ID="rfvFGPartNo" ControlToValidate="txtFGPartNo"
                                                SetFocusOnError="true" ErrorMessage="New Part number is required" Text="<" ValidationGroup="vgFinishedGood" />
                                            <asp:ImageButton ID="iBtnPFCopy" runat="server" ImageUrl="~/images/SelectUser.gif"
                                                ToolTip="Click here to copy Customer/Program Info from Planning and Forecasting Sales Projection per Internal PartNo (FG)."
                                                Visible="false" ValidationGroup="vgFinishedGood" />
                                        </td>
                                    </tr>
                                     <tr>
                                        <td class="p_text">
                                            New Finished Good Part Revision:
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtFGPartRevision" MaxLength="2"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="p_text">
                                            New Finished Good Part Name:
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtFGPartName" MaxLength="30"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="p_text">
                                            DMS Drawing No.:
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtFGDrawingNo" MaxLength="18"></asp:TextBox>
                                            <i>(same as above if blank)</i>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="p_text">
                                            Cost Sheet:
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtFGCostSheetID" MaxLength="10"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvFGCostSheetID" Operator="DataTypeCheck"
                                                ValidationGroup="vgFinishedGood" Type="integer" Text="<" ControlToValidate="txtFGCostSheetID"
                                                ErrorMessage="Cost Sheet ID must be an integer." SetFocusOnError="True" />
                                            <i>(same as above if blank)</i>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="p_text">
                                            ECI No:
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtFGECINo" MaxLength="10"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvFGECINo" Operator="DataTypeCheck" ValidationGroup="vgFinishedGood"
                                                Type="integer" Text="<" ControlToValidate="txtFGECINo" ErrorMessage="ECI number must be an integer."
                                                SetFocusOnError="True" />
                                            <i>(same as above if blank)</i>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="p_text">
                                            CapEx Project No.:
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtFGCapExProjectNo" MaxLength="15"></asp:TextBox>
                                            <i>(same as above if blank)</i>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="p_text">
                                            P.O. No.:
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtFGPONo" MaxLength="15"></asp:TextBox>
                                            <i>(same as above if blank)</i>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:Button runat="server" ID="btnSaveFinishedGood" Text="Add/Update FG Part No"
                                                CausesValidation="true" ValidationGroup="vgFinishedGood" />
                                            <asp:Button runat="server" ID="btnCancelFinishedGood" Text="Cancel/Edit FG Part No" />
                                        </td>
                                    </tr>
                                </table>
                            </Content>
                        </ajax:AccordionPane>
                    </Panes>
                </ajax:Accordion>
               <br />
                <asp:Label runat="Server" ID="lblTitleNewFinishedGood" Text="Below is the list of New Finished Good Part No(s) based on NEW Customer Part No above (See also Docushare SOP Document: QA 166)"
                    Visible="false"></asp:Label>
                <br />
                <br />
                <asp:GridView ID="gvNewFinishedGood" runat="server" AutoGenerateColumns="False" DataKeyNames="RowID"
                    DataSourceID="odsNewFinishedGood" EmptyDataText="No New Finished Goods found"
                    AllowSorting="True" AllowPaging="True" PageSize="15" Width="98%">
                    <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#CCCCCC" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="RowID" HeaderText="RowID" InsertVisible="False" ReadOnly="True"
                            SortExpression="RowID" />
                        <asp:BoundField DataField="RFDNo" HeaderText="RFDNo" SortExpression="RFDNo" ReadOnly="True" />
                        <asp:BoundField DataField="PartNo" HeaderText="New F.G. Part No" SortExpression="PartNo" />
                        <asp:BoundField DataField="PartRevision" HeaderText="New Rev." SortExpression="PartRevision" />
                        <asp:BoundField DataField="PartName" HeaderText="Name" SortExpression="PartName" />
                        <asp:BoundField DataField="DrawingNo" />
                        <asp:TemplateField HeaderText="Drawing No." SortExpression="DrawingNo">
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkViewDrawingNo" runat="server" NavigateUrl='<%# Eval("DrawingNo", "~/PE/DrawingDetail.aspx?DrawingNo={0}") %>'
                                    Font-Underline="true" Target="_blank" Text='<%# Eval("DrawingNo") %>'>
                                </asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="CostSheetID" />
                        <asp:TemplateField HeaderText="Cost Sheet" SortExpression="CostSheetID">
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkViewCostSheetID" runat="server" NavigateUrl='<%# Eval("CostSheetID", "~/Costing/Cost_Sheet_Preview.aspx?CostSheetID={0}") %>'
                                    Font-Underline="true" Target="_blank" Text='<%# Eval("CostSheetID") %>'>
                                </asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="ECINo" />
                        <asp:TemplateField HeaderText="ECI No." SortExpression="ECINo">
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkViewECINo" runat="server" NavigateUrl='<%# Eval("ECINo", "~/ECI/ECI_Detail.aspx?ECINo={0}") %>'
                                    Font-Underline="true" Target="_blank" Text='<%# Eval("ECINo") %>'>
                                </asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="CapExProjectNo" HeaderText="CapEx Project No." SortExpression="CapExProjectNo" />
                        <asp:BoundField DataField="PurchasingPONo" HeaderText="P.O. No." SortExpression="PurchasingPONo" />
                        <asp:TemplateField HeaderText="Edit/Delete <br />FG Part Info" ShowHeader="true">
                            <ItemTemplate>
                                <asp:ImageButton ID="ibtnFinishedGoodEdit" runat="server" CommandName="Select" ImageUrl="~/images/edit.jpg"
                                    AlternateText="Edit" />&nbsp;&nbsp;&nbsp;
                                <asp:ImageButton ID="ibtnFinishedGoodDelete" runat="server" CommandName="Delete"
                                    ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsNewFinishedGood" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetRFDFinishedGood" TypeName="RFDFinishedGoodBLL" DeleteMethod="DeleteRFDFinishedGood">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="RFDNo" QueryStringField="RFDNo" Type="Int32" />
                    </SelectParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="RowID" Type="Int32" />
                        <asp:QueryStringParameter Name="RFDNo" QueryStringField="RFDNo" Type="Int32" />
                        <asp:Parameter Name="original_RowID" Type="Int32" />
                    </DeleteParameters>
                </asp:ObjectDataSource>
                --%>
            </asp:View>
            <asp:View ID="vChildPart" runat="server">
                <asp:Label runat="server" ID="lblChildTip" Text="Please only list child parts that are new or are being changed. The complete bill of materials for any part will be handled in the Costing and the DMS modules."
                    SkinID="MessageLabelSkin" Font-Size="X-Small"></asp:Label>
                <asp:Label runat="server" ID="lblMessageChildPart" SkinID="MessageLabelSkin"></asp:Label>
                <br />
                <asp:ValidationSummary runat="server" ID="vsChildPart" ValidationGroup="vgChildPart"
                    ShowMessageBox="true" ShowSummary="true" />
                <br />
                <asp:Button runat="server" ID="btnGetFGDMSBOM" Text="Get Finished Good DMS BOM" Visible="false" />
                <br />
                <table runat="server" id="tblChildPart" visible="false">
                    <tr>
                        <td class="p_text">
                            <asp:Label runat="server" ID="lblCurrentChildPartNoLabel" Text="Current Child Part No (if exists):" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtCurrentChildPartNo" runat="server" MaxLength="40" Width="200px" />
                            <asp:ImageButton ID="iBtnCurrentChildPartNoSearch" runat="server" ImageUrl="~/images/Search.gif"
                                ToolTip="Click here to search for a Child Part No." />
                            <%-- &nbsp;Rev.&nbsp;
                            <asp:TextBox runat="server" ID="txtCurrentChildPartRevision" MaxLength="2" Width="20px"/>--%>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblNewChildPartNameMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="* " />
                            <asp:Label runat="server" ID="lblNewChildPartNameLabel" Text="New Child Part Name:" />
                        </td>
                        <td class="c_textbold">
                            <asp:TextBox runat="server" ID="txtNewChildPartNameValue" MaxLength="240" Width="300px" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvNewChildPartName" ControlToValidate="txtNewChildPartNameValue"
                                SetFocusOnError="true" ErrorMessage="Child Part Name is required" Text="<" ValidationGroup="vgChildPart" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label runat="server" ID="lblNewChildPartNoLabel" Text="New Child Part No.:" />
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtNewChildPartNoValue" MaxLength="40" />
                            <asp:ImageButton ID="iBtnNewChildPartNoSearch" runat="server" ImageUrl="~/images/Search.gif"
                                ToolTip="Click here to search for a Part No. (if exists)" Visible="true" />
                            <%--&nbsp;Rev.&nbsp;
                            <asp:TextBox runat="server" ID="txtNewChildPartRevisionValue" MaxLength="2" Width="20px"/>--%>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            New DMS Drawing No.:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtNewChildDrawingNo" MaxLength="18" />
                            <asp:ImageButton ID="iBtnNewChildDrawingSearch" runat="server" ImageUrl="~/images/Search.gif"
                                ToolTip="Click here to search for a DMS Drawing." />
                            &nbsp;
                            <asp:HyperLink runat="server" ID="hlnkNewChildDrawingNo" Font-Underline="true" ToolTip="Click here to view the new DMS Drawing."
                                Text="View" Target="_blank" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            New Cost Sheet ID:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtNewChildCostSheetID" MaxLength="10" />
                            <asp:CompareValidator runat="server" ID="cvNewChildCostSheetID" Operator="DataTypeCheck"
                                ValidationGroup="vgChildPart" Type="integer" Text="<" ControlToValidate="txtNewChildCostSheetID"
                                ErrorMessage="Cost Sheet ID must be an integer." SetFocusOnError="True" />
                            <asp:HyperLink runat="server" ID="hlnkNewChildCostSheetID" Visible="false" Font-Underline="true"
                                ToolTip="Click here to view the new Cost Sheet." Text="View Cost Sheet" Target="_blank" />
                            <asp:HyperLink runat="server" ID="hlnkNewChildDieLayout" Visible="false" Font-Underline="true"
                                ToolTip="Click here to view the new Die Layout." Text="View Die Layout" Target="_blank" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            New External RFQ No:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtNewChildExternalRFQNo" MaxLength="15" />
                            &nbsp;
                            <asp:CheckBox runat="server" ID="cbNewChildExternalRFQNoNA" Text="N/A" />
                            &nbsp; <a runat="server" id="aDocushareExternalRFQTemplate" style="text-decoration: underline;"
                                href="http://tapsd.ugnnet.com:8080/docushare/dsweb/Get/Document-1284/CST103a%20Request%20for%20Quotation%20(RFQ)%20External.xls"
                                target="_blank">Click Here to Create New External RFQ Document</a>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Material Lead time:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtNewChildLeadTime" MaxLength="2" Width="50px" />
                            <asp:CompareValidator runat="server" ID="cvNewChildLeadTime" Operator="DataTypeCheck"
                                ValidationGroup="vgChildPart" Type="integer" Text="<" ControlToValidate="txtNewChildLeadTime"
                                ErrorMessage="New child lead time must be an integer" SetFocusOnError="True" />
                            &nbsp;
                            <asp:DropDownList runat="server" ID="ddNewChildLeadUnits">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem Value="days">days</asp:ListItem>
                                <asp:ListItem Value="weeks">weeks</asp:ListItem>
                                <asp:ListItem Value="months">months</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            New ECI No:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtNewChildECINo" MaxLength="10" />
                            <asp:ImageButton ID="iBtnNewChildECINoSearch" runat="server" ImageUrl="~/images/Search.gif"
                                ToolTip="Click here to add or search for an ECI." />
                            <asp:CompareValidator runat="server" ID="cvNewChildECINo" Operator="DataTypeCheck"
                                ValidationGroup="vgChildPart" Type="integer" Text="<" ControlToValidate="txtNewChildECINo"
                                ErrorMessage="ECI number must be an integer." SetFocusOnError="True" />
                            <asp:HyperLink runat="server" ID="hlnkNewChildECINo" Visible="false" Font-Underline="true"
                                ToolTip="Click here to view the new ECI." Text="View ECI" Target="_blank" />
                            <asp:CheckBox runat="server" ID="cbNewChildECIOverrideNA" Text="N/A" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            New P.O. No.:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtNewChildPONo" MaxLength="15" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Button runat="server" ID="btnSaveChild" Text="Add Child" CausesValidation="true"
                                ValidationGroup="vgChildPart" />
                            <asp:Button runat="server" ID="btnCancelChild" CausesValidation="false" Text="Cancel" />
                        </td>
                    </tr>
                </table>
                <ajax:Accordion ID="acChildPart" runat="server" SelectedIndex="-1" HeaderCssClass="accordionHeader"
                    HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
                    FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
                    RequireOpenedPane="false" SuppressHeaderPostbacks="true" Visible="false">
                    <Panes>
                        <ajax:AccordionPane ID="apChildPart" runat="server">
                            <Header>
                                <a href="">
                                    <asp:Label runat="server" ID="lblChildPartLink" Text="Edit Child Part Details / Measurements" /></a></Header>
                            <Content>
                                <asp:ValidationSummary runat="server" ID="vsChildPartGenerateDrawing" ValidationGroup="vgChildPartGenerateDrawing"
                                    ShowMessageBox="true" ShowSummary="true" />
                                <table width="98%" border="1" cellpadding="1" cellspacing="1" style="border-color: Navy">
                                    <tr>
                                        <td class="p_bigtextbold" align="center" style="background-color: #DEDEDE">
                                            <asp:Label runat="server" ID="lblCurrentChildPartTitle" Text="CURRENT" Visible="false" />
                                        </td>
                                        <td class="p_bigtextbold" align="center" style="background-color: Aqua">
                                            <asp:Label runat="server" ID="lblNewChildPartTitle" Text="NEW" />
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td style="background-color: #DEDEDE" valign="top">
                                            <table runat="server" id="tblCurrentChildPart" visible="False">
                                                <tr>
                                                    <td class="p_text">
                                                        Internal Part No:
                                                    </td>
                                                    <td class="c_textbold" style="white-space: nowrap;">
                                                        <asp:Label ID="lblCurrentChildPartNo" runat="server" />
                                                        <asp:HyperLink runat="server" ID="hlnkCurrentChildPartBOM" Visible="true" Font-Underline="true"
                                                            ToolTip="Click here to view the Bill of Materials." Text="BOM" Target="_blank" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Revision:
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblCurrentChildPartRevision" />
                                                        <asp:HyperLink runat="server" ID="hlnkCurrentChildBPCSParentParts" Visible="true"
                                                            Font-Underline="true" ToolTip="Click here to view parent parts affected (or where used) and then choose to create new RFDs for them"
                                                            Text="Parent Parts Affected By Change" Target="_blank" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Name:
                                                    </td>
                                                    <td class="c_textbold">
                                                        <asp:Label ID="txtCurrentChildPartName" runat="server" BorderStyle="None" BackColor="#DEDEDE" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        DMS Drawing No.:
                                                    </td>
                                                    <td class="c_textbold" style="white-space: nowrap;">
                                                        <asp:TextBox ID="txtCurrentChildDrawingNo" runat="server" MaxLength="18" />
                                                        <asp:ImageButton ID="iBtnCurrentChildDrawingNoSearch" runat="server" ImageUrl="~/images/Search.gif"
                                                            ToolTip="Click here to search for a Drawing No." />
                                                        &nbsp;
                                                        <asp:HyperLink runat="server" ID="hlnkCurrentChildDrawingNo" Visible="false" Font-Underline="true"
                                                            ToolTip="Click here to view the DMS Drawing." Text="View" Target="_blank" />
                                                        &nbsp;
                                                        <asp:ImageButton ID="iBtnCurrentChildDrawingCopy" runat="server" ImageUrl="~/images/SelectUser.gif"
                                                            ToolTip="Click here to copy details based on the current DMS Drawing." Visible="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2">
                                                        <asp:Button runat="server" ID="btnCurrentChildCopyAll" Text="Copy all fields below to the new part >>"
                                                            Width="300px" Visible="false" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td class="p_text">
                                                        Part No:
                                                    </td>
                                                    <td class="c_textbold" style="white-space: nowrap; height: 25px">
                                                        <asp:Label ID="lblNewChildPartNo" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Revision:
                                                    </td>
                                                    <td class="c_textbold" style="white-space: nowrap; height: 25px">
                                                        <asp:Label runat="server" ID="lblNewChildPartRevision" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Name:
                                                    </td>
                                                    <td class="c_textbold" style="white-space: nowrap; height: 25px">
                                                        <asp:Label ID="lblNewChildPartName" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        DMS Drawing No.:
                                                    </td>
                                                    <td class="c_textbold" style="white-space: nowrap; height: 25px">
                                                        <asp:Label ID="lblNewChildDrawingNo" runat="server" />
                                                        &nbsp;
                                                        <asp:HyperLink runat="server" ID="hlnkNewChildDrawingNo2" Visible="false" Font-Underline="true"
                                                            ToolTip="Click here to view the DMS Drawing." Text="View" Target="_blank" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" colspan="2">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" colspan="2">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td style="background-color: #DEDEDE" valign="top">
                                            <table runat="server" id="tblCurrentChildPartMeasurements" visible="False">
                                                <tr>
                                                    <td class="p_text">
                                                        Current Initial Dimension and Density:
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtCurrentChildInitialDimensionAndDensity" runat="server" MaxLength="2"
                                                            Width="25px" Enabled="false">00</asp:TextBox>
                                                        <asp:Button runat="server" ID="btnCurrentChildCopyInitialDimensionAndDensity" Text=">>"
                                                            Visible="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Current Process Step No.:
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtCurrentChildInStepTracking" runat="server" MaxLength="1" Width="25px"
                                                            Enabled="false" />
                                                        <asp:Button runat="server" ID="btnCurrentChildCopyInStepTracking" Text=">>" Visible="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        AMD:
                                                    </td>
                                                    <td style="white-space: nowrap;">
                                                        <asp:TextBox runat="server" ID="txtCurrentChildAMDValue" MaxLength="7" Width="75px"
                                                            Enabled="false" />
                                                        &nbsp; Tol:
                                                        <asp:TextBox runat="server" ID="txtCurrentChildAMDTolerance" MaxLength="7" Width="50px"
                                                            Enabled="false" />
                                                        &nbsp;
                                                        <asp:DropDownList runat="server" ID="ddCurrentChildAMDUnits" Width="50px" Enabled="false">
                                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                                            <asp:ListItem Value="m" Text="m"></asp:ListItem>
                                                            <asp:ListItem Value="mm" Text="mm"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:Button runat="server" ID="btnCurrentChildCopyAMD" Text=">>" Visible="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        WMD:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtCurrentChildWMDValue" MaxLength="7" Width="75px"
                                                            Enabled="false" />
                                                        &nbsp; Tol:
                                                        <asp:TextBox runat="server" ID="txtCurrentChildWMDTolerance" MaxLength="7" Width="50px"
                                                            Enabled="false" />
                                                        &nbsp;
                                                        <asp:DropDownList runat="server" ID="ddCurrentChildWMDUnits" Width="50px" Enabled="false">
                                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                                            <asp:ListItem Value="m" Text="m"></asp:ListItem>
                                                            <asp:ListItem Value="mm" Text="mm"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:Button runat="server" ID="btnCurrentChildCopyWMD" Text=">>" Visible="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Density:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtCurrentChildDensityValue" MaxLength="7" Width="75px"
                                                            Enabled="false" />
                                                        &nbsp; Tol:
                                                        <asp:TextBox runat="server" ID="txtCurrentChildDensityTolerance" MaxLength="7" Width="50px"
                                                            Enabled="false" />
                                                        &nbsp;
                                                        <asp:TextBox runat="server" ID="txtCurrentChildDensityUnits" MaxLength="7" Width="50px"
                                                            Enabled="false" />
                                                        <asp:Button runat="server" ID="btnCurrentChildCopyDensity" Text=">>" Visible="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Construction:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtCurrentChildConstruction" MaxLength="400" TextMode="MultiLine"
                                                            Enabled="false" Height="100px" />
                                                        <asp:Button runat="server" ID="btnCurrentChildCopyConstruction" Text=">>" Visible="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Drawing Notes:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtCurrentChildDrawingNotes" MaxLength="400" TextMode="MultiLine"
                                                            Enabled="false" Height="100px" />
                                                        <asp:Button runat="server" ID="btnCurrentChildCopyNotes" Text=">>" Visible="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Designation Type:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddCurrentChildDesignationType" Enabled="false" />
                                                        <asp:Button runat="server" ID="btnCurrentChildCopyDesignationType" Text=">>" Visible="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Family:
                                                    </td>
                                                    <td class="c_textbold">
                                                        <asp:DropDownList ID="ddCurrentChildFamily" runat="server" Enabled="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="c_text" colspan="2">
                                                        Sub-Family:
                                                        <asp:DropDownList ID="ddCurrentChildSubFamily" runat="server" Enabled="false" />
                                                        <asp:Button runat="server" ID="btnCurrentChildCopySubfamily" Text=">>" Visible="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Purchased Good:
                                                    </td>
                                                    <td class="c_textbold" style="white-space: nowrap;">
                                                        <asp:DropDownList ID="ddCurrentChildPurchasedGood" runat="server" Enabled="false" />
                                                        <asp:Button runat="server" ID="btnCurrentChildCopyPurchasedGood" Text=">>" Visible="false" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td valign="top">
                                            <table>
                                                <tr>
                                                    <td class="p_text">
                                                        <asp:Label ID="lblNewChildInitialDimensionAndDensityMarker" runat="server" Font-Bold="True"
                                                            ForeColor="Red" Text="*" />
                                                        New Initial Dimension and Density:
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtNewChildInitialDimensionAndDensity" runat="server" MaxLength="2"
                                                            Width="25px" Enabled="false">00</asp:TextBox>
                                                        &nbsp;<i>**Inserted into the Drawing No.</i>
                                                        <asp:RequiredFieldValidator ID="rfvNewChildInitialDimensionAndDensity" runat="server"
                                                            ControlToValidate="txtNewChildInitialDimensionAndDensity" Text="<" ErrorMessage="Initial Dimension And Density Digits are required."
                                                            SetFocusOnError="true" ValidationGroup="vgChildPartGenerateDrawing">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:CompareValidator runat="server" ID="cvtNewChildInitialDimensionAndDensity" Operator="DataTypeCheck"
                                                            ValidationGroup="vgChildPartGenerateDrawing" Type="integer" Text="<" ControlToValidate="txtNewChildInitialDimensionAndDensity"
                                                            ErrorMessage="Initial Dimension And Density Digits must be integers." SetFocusOnError="True" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        <asp:Label ID="lblNewChildInStepTrackingNoMarker" runat="server" Font-Bold="True"
                                                            ForeColor="Red" Text="*" />
                                                        New Process Step No.:
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:TextBox ID="txtNewChildInStepTracking" runat="server" MaxLength="1" Width="25px"
                                                            Enabled="false" />
                                                        &nbsp;<i>**Inserted into the Drawing No.</i>
                                                        <asp:RequiredFieldValidator ID="rfvNewChildInStepTracking" CssClass="p_text" runat="server"
                                                            Display="Dynamic" ControlToValidate="txtNewChildInStepTracking" SetFocusOnError="True"
                                                            ErrorMessage="Process Number is required." Text="<" ValidationGroup="vgChildPartGenerateDrawing">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:CompareValidator runat="server" ID="cvNewChildInstepTracking" Operator="DataTypeCheck"
                                                            Type="Integer" Text="<" ErrorMessage="Process Step must be an integer." ControlToValidate="txtNewChildInStepTracking"
                                                            SetFocusOnError="True" ValidationGroup="vgChildPart" />
                                                        <asp:RegularExpressionValidator ID="revNewChildProcessStepNumber" runat="server"
                                                            ControlToValidate="txtNewChildInStepTracking" Text="<" ErrorMessage="Value Must be 1 through 9"
                                                            SetFocusOnError="True" ValidationExpression="[1-9]" ValidationGroup="vgChildPartGenerateDrawing">
                                                        </asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        AMD:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtNewChildAMDValue" MaxLength="10" Width="75px" />
                                                        <asp:CompareValidator runat="server" ID="cvNewChildAMDValue" Operator="DataTypeCheck"
                                                            ValidationGroup="vgChildPartMeasurement" Type="double" Text="<" ControlToValidate="txtNewChildAMDValue"
                                                            ErrorMessage="AMD must be a number." SetFocusOnError="True" />
                                                        &nbsp; Tol:
                                                        <asp:TextBox runat="server" ID="txtNewChildAMDTolerance" MaxLength="10" Width="50px" />
                                                        &nbsp;
                                                        <asp:DropDownList runat="server" ID="ddNewChildAMDUnits" Width="50px">
                                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                                            <asp:ListItem Value="m" Text="m"></asp:ListItem>
                                                            <asp:ListItem Value="mm" Text="mm"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        WMD:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtNewChildWMDValue" MaxLength="10" Width="75px" />
                                                        <asp:CompareValidator runat="server" ID="cvNewChildWMDValue" Operator="DataTypeCheck"
                                                            ValidationGroup="vgChildPartMeasurement" Type="double" Text="<" ControlToValidate="txtNewChildWMDValue"
                                                            ErrorMessage="WMD must be a number." SetFocusOnError="True" />
                                                        &nbsp; Tol:
                                                        <asp:TextBox runat="server" ID="txtNewChildWMDTolerance" MaxLength="10" Width="50px" />
                                                        &nbsp;
                                                        <asp:DropDownList runat="server" ID="ddNewChildWMDUnits" Width="50px">
                                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                                            <asp:ListItem Value="m" Text="m"></asp:ListItem>
                                                            <asp:ListItem Value="mm" Text="mm"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Density:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtNewChildDensityValue" MaxLength="10" Width="75px" />
                                                        <asp:CompareValidator runat="server" ID="cvNewChildDensityValue" Operator="DataTypeCheck"
                                                            ValidationGroup="vgChildPartMeasurement" Type="double" Text="<" ControlToValidate="txtNewChildDensityValue"
                                                            ErrorMessage="Density must be a number." SetFocusOnError="True" />
                                                        &nbsp; Tol:
                                                        <asp:TextBox runat="server" ID="txtNewChildDensityTolerance" MaxLength="7" Width="50px" />
                                                        &nbsp;
                                                        <asp:TextBox runat="server" ID="txtNewChildDensityUnits" MaxLength="7" Width="50px" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Construction:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtNewChildConstruction" MaxLength="100" TextMode="MultiLine"
                                                            Height="100px" />
                                                        <br />
                                                        <asp:Label ID="lblNewChildConstructionCharCount" SkinID="MessageLabelSkin" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Drawing Notes:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtNewChildDrawingNotes" MaxLength="100" TextMode="MultiLine"
                                                            Height="100px" />
                                                        <br />
                                                        <asp:Label ID="lblNewChildDrawingNotesCharCount" SkinID="MessageLabelSkin" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Designation Type:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList runat="server" ID="ddNewChildDesignationType" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Family:
                                                    </td>
                                                    <td class="c_textbold">
                                                        <asp:DropDownList ID="ddNewChildFamily" runat="server" AutoPostBack="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="c_text" colspan="2">
                                                        Sub-Family:
                                                        <asp:DropDownList ID="ddNewChildSubFamily" runat="server" />
                                                        <asp:RequiredFieldValidator ID="rfvNewChildSubFamily" runat="server" ControlToValidate="ddNewChildSubFamily"
                                                            Text="<" ErrorMessage="Sub-Family is required." SetFocusOnError="true" ValidationGroup="vgChildPartGenerateDrawing" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="p_text">
                                                        Purchased Good:
                                                    </td>
                                                    <td class="c_textbold">
                                                        <asp:DropDownList ID="ddNewChildPurchasedGood" runat="server" />
                                                        <asp:RequiredFieldValidator ID="rfvNewChildPurchasedGood" runat="server" ControlToValidate="ddNewChildPurchasedGood"
                                                            Text="<" ErrorMessage="Purchased Good is required." SetFocusOnError="true" ValidationGroup="vgChildPartGenerateDrawing" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="center">
                                                        <asp:Button runat="server" ID="btnGenerateNewChildDrawing" Text="Generate New DMS Drawing"
                                                            ValidationGroup="vgChildPartGenerateDrawing" Width="300px" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2">
                                                        <asp:RadioButtonList runat="server" ID="rbGenerateNewChildDrawing" RepeatDirection="Horizontal"
                                                            Visible="false">
                                                            <asp:ListItem Text="New Part" Value="N"></asp:ListItem>
                                                            <asp:ListItem Text="New Revision" Value="R" Selected="True"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label runat="server" ID="lblMessageChildPartDetails" SkinID="MessageLabelSkin" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:Button runat="server" ID="btnSaveChildDetails" Text="Update Child" CausesValidation="true"
                                                ValidationGroup="vgChildPartMeasurement" />
                                            <asp:Button runat="server" ID="btnCancelChildDetails" Text="Close" />
                                            <br />
                                            <asp:ValidationSummary runat="server" ID="vsChildPartMeasurement" ValidationGroup="vgChildPartMeasurement"
                                                ShowMessageBox="true" ShowSummary="true" />
                                        </td>
                                    </tr>
                                </table>
                            </Content>
                        </ajax:AccordionPane>
                    </Panes>
                </ajax:Accordion>
                <asp:Label runat="server" ID="lblMessageChildPartBottom" SkinID="MessageLabelSkin"></asp:Label>
                <hr />
                <asp:Label runat="server" ID="lblChildTip2" Text="For the RFC Business process type, AT LEASET ONE Child Part No(s) MUST BE ASSIGNED in order for the RFD to be closed after all approvals have been completed. However, team members still may submit this for approval without prior knowledge of what the new part numbers should be. The approvers can determine what the new numbers should be."
                    Font-Italic="true" Font-Size="XX-Small"></asp:Label>
                <br />
                <asp:Label runat="server" ID="lblTitleBPCSChildPart" Text="Below is the list of Child Part No(s) based on Docushare SOP Document: QA 167"
                    Visible="false" />
                <br />
                <asp:GridView ID="gvChildPart" runat="server" AutoGenerateColumns="False" DataKeyNames="RowID"
                    DataSourceID="odsChildPart" EmptyDataText="No Child Parts have been defined yet."
                    AllowSorting="True" AllowPaging="True" PageSize="15" Width="98%">
                    <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#CCCCCC" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="RowID" HeaderText="RowID" />
                        <asp:BoundField DataField="RFDNo" HeaderText="RFDNo" />
                        <asp:BoundField DataField="CurrentPartNo" HeaderText="Current Internal Part No."
                            SortExpression="CurrentPartNo" />
                        <asp:TemplateField HeaderText="Current Drawing No." SortExpression="CurrentDrawingNo">
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkViewCurrentDrawingNo" runat="server" NavigateUrl='<%# Eval("CurrentDrawingNo", "~/PE/DrawingDetail.aspx?DrawingNo={0}") %>'
                                    Font-Underline="true" Target="_blank" Text='<%# Eval("CurrentDrawingNo") %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="NewPartNo" HeaderText="New Internal Part No." SortExpression="NewPartNo" />
                        <asp:BoundField DataField="NewPartName" HeaderText="New Internal Part Name" SortExpression="NewPartName" />
                        <asp:TemplateField HeaderText="New Drawing No." SortExpression="NewDrawingNo">
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkViewNewDrawingNo" runat="server" NavigateUrl='<%# Eval("NewDrawingNo", "~/PE/DrawingDetail.aspx?DrawingNo={0}") %>'
                                    Font-Underline="true" Target="_blank" Text='<%# Eval("NewDrawingNo") %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cost Sheet" SortExpression="CostSheetID">
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkViewNewCostSheetID" runat="server" NavigateUrl='<%# Eval("CostSheetID", "~/Costing/Cost_Sheet_Preview.aspx?CostSheetID={0}") %>'
                                    Font-Underline="true" Target="_blank" Text='<%# Eval("CostSheetID") %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="ExternalRFQNo" HeaderText="External RFQ No" SortExpression="ExternalRFQNo" />
                        <asp:CheckBoxField ItemStyle-HorizontalAlign="Center" DataField="isExternalRFQrequired"
                            HeaderText="External RFQ Required" SortExpression="isExternalRFQrequired">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:CheckBoxField>
                        <asp:TemplateField HeaderText="ECI No." SortExpression="ECINo">
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkViewNewECINo" runat="server" NavigateUrl='<%# Eval("ECINo", "~/ECI/ECI_Detail.aspx?ECINo={0}") %>'
                                    Font-Underline="true" Target="_blank" Text='<%# Eval("ECINo") %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="PurchasingPONo" HeaderText="P.O. No." SortExpression="PurchasingPONo" />
                        <asp:TemplateField HeaderText="Edit / Delete <br />(Identifiers Shown Above)<br />(Details Shown Below)">
                            <ItemTemplate>
                                <asp:ImageButton ID="ibtnChildPartEdit" runat="server" CausesValidation="False" CommandName="Select"
                                    ImageUrl="~/images/edit.jpg" AlternateText="Edit" />&nbsp;&nbsp;&nbsp;
                                <asp:ImageButton ID="ibtnChildPartDelete" runat="server" CausesValidation="False"
                                    CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsChildPart" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetRFDChildPart" TypeName="RFDChildPartBLL" DeleteMethod="DeleteRFDChildPart">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="0" Name="RowID" Type="Int32" />
                        <asp:QueryStringParameter Name="RFDNo" QueryStringField="RFDNo" Type="Int32" />
                    </SelectParameters>
                    <DeleteParameters>
                        <asp:QueryStringParameter Name="RFDNo" QueryStringField="RFDNo" Type="Int32" />
                        <asp:Parameter Name="original_RowID" Type="Int32" />
                    </DeleteParameters>
                </asp:ObjectDataSource>
            </asp:View>
            <asp:View ID="vCustomerProgram" runat="server">
                <asp:ValidationSummary runat="server" ID="vsCustomerProgram" ValidationGroup="vgCustomerProgram"
                    ShowMessageBox="true" ShowSummary="true" />
                <asp:Label ID="lblMessageCustomerProgram" runat="server" SkinID="MessageLabelSkin"></asp:Label>
                <table runat="server" id="tblCustomerProgram" visible="false">
                    <tr>
                        <td valign="top">
                            <table runat="server" id="tblMakes" visible="false">
                                <tr>
                                    <td class="p_text">
                                        <asp:Label runat="server" ID="lblMake" Text="Make:" Visible="true" />
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
                                        <asp:Label runat="server" ID="lblProgram" Text="Program:" Visible="true" />
                                    </td>
                                    <td colspan="3" style="white-space: nowrap">
                                        <asp:DropDownList ID="ddProgram" runat="server" />
                                        <asp:RequiredFieldValidator ID="rfvProgram" runat="server" ControlToValidate="ddProgram"
                                            ErrorMessage="Program is required." Font-Bold="True" ValidationGroup="vgCustomerProgram"
                                            Text="<" SetFocusOnError="true" />
                                        <asp:ImageButton ID="iBtnPreviewDetail" runat="server" ImageUrl="~/images/PreviewUp.jpg"
                                            ToolTip="Review Program Detail" Visible="false" />
                                        <br />
                                        {Program / Platform / Assembly Plant}
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table>
                                <%-- <tr>
                                   <td class="p_text">
                                        <asp:Label runat="server" ID="lblCustomerEdit" Text="Customer:" Visible="false"/>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddCustomerEdit" runat="server" Visible="false"/>
                                    </td>
                                   
                                </tr>--%>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label runat="server" ID="lblSOPDate" Text="Program SOP Date:" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSOPDate" runat="server" MaxLength="10" Width="75px" />
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
                                        <asp:Label ID="lblCustomerApprovalRequired" runat="server" Text="Customer Approval Required" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cbCustomerApprovalRequired" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label runat="server" ID="lblEOPDate" Text="Program EOP Date:" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEOPDate" runat="server" MaxLength="10" Width="75px" />
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
                                        <asp:Label ID="lblCustomerApprovalDate" runat="server" Text="Customer Approval Date" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCustomerApprovalDate" runat="server" MaxLength="10" Width="70px" />
                                        <asp:ImageButton runat="server" ID="imgCustomerApprovalDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                            AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                                        <ajax:CalendarExtender ID="ceCustomerApprovalDate" runat="server" TargetControlID="txtCustomerApprovalDate"
                                            PopupButtonID="imgCustomerApprovalDate" />
                                        <asp:RegularExpressionValidator ID="revCustomerApprovalDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                            ControlToValidate="txtCustomerApprovalDate" Font-Bold="True" ToolTip="MM/DD/YYYY"
                                            ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                            Width="8px" ValidationGroup="vgCustomerProgram"><</asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label ID="lblYearMarker" runat="server" Font-Bold="True" ForeColor="Red" Text="*"
                                            Visible="false" />
                                        <asp:Label runat="server" ID="lblYear" Text="Year:" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddYear" runat="server" />
                                        <asp:RequiredFieldValidator ID="rfvYear" runat="server" ControlToValidate="ddYear"
                                            ErrorMessage="Year is required." Font-Bold="True" ValidationGroup="vgCustomerProgram"
                                            Text="<" SetFocusOnError="true" />
                                    </td>
                                    <td class="p_text">
                                        <asp:Label ID="lblCustomerApprovalNo" runat="server" Text="Customer Approval No." />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCustomerApprovalNo" runat="server" MaxLength="20" Width="150px" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Button runat="server" ID="btnSaveCustomerProgram" Text="Add/Update Customer/Program"
                                CausesValidation="true" ValidationGroup="vgCustomerProgram" Visible="false" />
                            <asp:Button runat="server" ID="btnCancelCustomerProgram" Text="Cancel" Visible="false" />
                            <asp:Button runat="server" ID="btnGetPlanningForecastingVehicle" Text="Get Planning and Forecasting Vehicle Info"
                                Visible="false" ToolTip="Click here to copy details based on Planning and Forecasting Vehicle Info based on program with an option of year and or Customer." />
                        </td>
                    </tr>
                </table>
                <asp:Label ID="lblMessageCustomerProgramBottom" runat="server" SkinID="MessageLabelSkin" />
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
                    EmptyDataText="No Programs or Customers found" Width="900px">
                    <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#CCCCCC" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="RowID" HeaderText="RowID" SortExpression="RowID" />
                        <asp:BoundField DataField="RFDNo" HeaderText="RFDNo" SortExpression="RFDNo" />
                        <asp:BoundField DataField="ddCustomerDesc" HeaderText="Customer" SortExpression="ddCustomerDesc"
                            ReadOnly="True" />
                        <asp:BoundField DataField="ProgramID" SortExpression="ProgramID" ReadOnly="True" />
                        <asp:BoundField DataField="ddProgramName" HeaderText="Program / Make / Model / Platform / Assembly"
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
                    SelectMethod="GetRFDCustomerProgram" TypeName="RFDCustomerProgramBLL" DeleteMethod="DeleteRFDCustomerProgram">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="RFDNo" QueryStringField="RFDNo" Type="Int32" />
                    </SelectParameters>
                    <DeleteParameters>
                        <asp:QueryStringParameter Name="RFDNo" QueryStringField="RFDNo" Type="Int32" />
                        <asp:Parameter Name="original_RowID" Type="Int32" />
                    </DeleteParameters>
                </asp:ObjectDataSource>
            </asp:View>
            <asp:View ID="vFacilityDept" runat="server">
                <asp:ValidationSummary runat="server" ID="vsFacilityDepartment" ValidationGroup="vgFacilityDept"
                    ShowMessageBox="true" ShowSummary="true" />
                <asp:Label ID="lblMessageFacilityDepartment" runat="server" SkinID="MessageLabelSkin"></asp:Label>
                <asp:ValidationSummary runat="server" ID="vsEditFacilityDept" ValidationGroup="vgEditFacilityDept"
                    ShowMessageBox="true" ShowSummary="true" />
                <asp:ValidationSummary runat="server" ID="vsInsertFacilityDept" ValidationGroup="vgInsertFacilityDept"
                    ShowMessageBox="true" ShowSummary="true" />
                <br />
                <asp:GridView ID="gvFacilityDept" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                    AllowPaging="True" PageSize="15" DataKeyNames="RowID" DataSourceID="odsFacilityDept"
                    EmptyDataText="No records found" Width="98%" ShowFooter="true">
                    <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#CCCCCC" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="RowID" HeaderText="RowID" SortExpression="RowID" />
                        <asp:BoundField DataField="RFDNo" HeaderText="RFDNo" SortExpression="RFDNo" />
                        <asp:TemplateField HeaderText="UGN Facility" SortExpression="UGNFacilityName">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddEditFacility" runat="server" DataSource='<%# commonFunctions.GetUGNFacility("") %>'
                                    Enabled="false" DataValueField="UGNFacility" DataTextField="ddUGNFacilityName"
                                    AppendDataBoundItems="True" SelectedValue='<%# Bind("UGNFacility") %>'>
                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvEditFacility" runat="server" ControlToValidate="ddEditFacility"
                                    ErrorMessage="UGN Facility is required." Font-Bold="True" ValidationGroup="vgEditFacilityDept"
                                    Text="<" SetFocusOnError="true" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewUGNFacilityName" runat="server" Text='<%# Bind("ddUGNFacilityName") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddInsertFacility" runat="server" DataSource='<%# commonFunctions.GetUGNFacility("") %>'
                                    AutoPostBack="true" OnSelectedIndexChanged="ddInsertUGNFacility_SelectedIndexChanged"
                                    AppendDataBoundItems="True" DataValueField="UGNFacility" DataTextField="ddUGNFacilityName">
                                    <asp:ListItem Text="" Value="" />
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvInsertFacility" runat="server" ControlToValidate="ddInsertFacility"
                                    ErrorMessage="UGN Facility is required." Font-Bold="True" ValidationGroup="vgInsertFacilityDept"
                                    Text="<" SetFocusOnError="true" />
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Department" SortExpression="DepartmentName">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddEditDepartment" runat="server" DataSource='<%# commonfunctions.GetDepartmentGLNo("") %>'
                                    DataValueField="DepartmentID" DataTextField="ddDepartmentName" SelectedValue='<%# Bind("DepartmentID") %>'
                                    AppendDataBoundItems="true">
                                    <asp:ListItem Text="" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewDepartmentName" runat="server" Text='<%# Bind("ddDepartmentName") %>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddInsertDepartment" runat="server" DataValueField="DepartmentID"
                                    DataTextField="ddDepartmentName" />
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <EditItemTemplate>
                                <asp:ImageButton ID="iBtnFacilityDeptUpdate" runat="server" CausesValidation="True"
                                    CommandName="Update" ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditFacilityDept" />
                                <asp:ImageButton ID="iBtnFacilityDeptCancel" runat="server" CausesValidation="False"
                                    CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="iBtnFacilityDeptEdit" runat="server" CausesValidation="False"
                                    CommandName="Edit" ImageUrl="~/images/edit.jpg" AlternateText="Edit" ValidationGroup="vgEditFacilityDept" />
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
                    SelectMethod="GetRFDFacilityDept" TypeName="RFDFacilityDeptBLL" DeleteMethod="DeleteRFDFacilityDept"
                    InsertMethod="InsertRFDFacilityDept" UpdateMethod="UpdateRFDFacilityDept">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="RFDNo" QueryStringField="RFDNo" Type="Int32" />
                    </SelectParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="RowID" Type="Int32" />
                        <asp:Parameter Name="original_RowID" Type="Int32" />
                        <asp:QueryStringParameter Name="RFDNo" QueryStringField="RFDNo" Type="Int32" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="RowID" Type="Int32" />
                        <asp:QueryStringParameter Name="RFDNo" QueryStringField="RFDNo" Type="Int32" />
                        <asp:Parameter Name="original_RowID" Type="Int32" />
                        <asp:Parameter Name="UGNFacility" Type="String" />
                        <asp:Parameter Name="DepartmentID" Type="Int32" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:QueryStringParameter Name="RFDNo" QueryStringField="RFDNo" Type="Int32" />
                        <asp:Parameter Name="UGNFacility" Type="String" />
                        <asp:Parameter Name="DepartmentID" Type="Int32" />
                    </InsertParameters>
                </asp:ObjectDataSource>
            </asp:View>
            <asp:View ID="vVendor" runat="server">
                <asp:Label ID="lblMessageVendor" runat="server" SkinID="MessageLabelSkin" />
                <asp:ValidationSummary runat="server" ID="vsVendor" ValidationGroup="vgVendor" ShowMessageBox="true"
                    ShowSummary="true" />
                <table width="98%">
                    <tr>
                        <td class="p_text">
                            PPAP Required:
                        </td>
                        <td class="c_textbold">
                            <asp:CheckBox ID="cbPPAP" runat="server" />
                            &nbsp; (See CARS Application)
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Vendor Requirement:
                        </td>
                        <td colspan="3" class="c_textbold">
                            <asp:TextBox ID="txtVendorRequirement" runat="server" TextMode="MultiLine" Width="500px" />
                            <br />
                            <asp:Label ID="lblVendorRequirementCharCount" SkinID="MessageLabelSkin" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:Button ID="btnSaveVendor" runat="server" Text="Save" ValidationGroup="vgVendor" />
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
                    EmptyDataText="No records found" Width="98%" ShowFooter="True">
                    <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#CCCCCC" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="RowID" HeaderText="RowID" ReadOnly="True" />
                        <asp:BoundField DataField="RFDNo" HeaderText="RFDNo" ReadOnly="True" />
                        <asp:TemplateField HeaderText="Vendor" SortExpression="ddUGNDBVendorName">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddEditVendor" runat="server" DataSource='<%# CommonFunctions.GetUGNDBVendor(0,"","",0) %>'
                                    DataValueField="UGNDBVendorID" DataTextField="ddVendorName" AppendDataBoundItems="True"
                                    SelectedValue='<%# Bind("UGNDBVendorID") %>' />
                                <asp:RequiredFieldValidator ID="rfvEditVendor" runat="server" ControlToValidate="ddEditVendor"
                                    ErrorMessage="Vendor is required." Font-Bold="True" ValidationGroup="vgEditVendor"
                                    Text="<" SetFocusOnError="true" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewVendor" runat="server" Text='<%# Bind("ddVendorName") %>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddInsertVendor" runat="server" DataSource='<%# CommonFunctions.GetUGNDBVendor(0,"","",0) %>'
                                    DataValueField="UGNDBVendorID" DataTextField="ddVendorName" AppendDataBoundItems="True">
                                    <asp:ListItem Text="" Value="0" Selected="False"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvInsertVendor" runat="server" ControlToValidate="ddInsertVendor"
                                    ErrorMessage="Vendor is required." Font-Bold="True" ValidationGroup="vgInsertVendor"
                                    Text="<" SetFocusOnError="true" />
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
                    SelectMethod="GetRFDVendor" TypeName="RFDVendorBLL" DeleteMethod="DeleteRFDVendor"
                    InsertMethod="InsertRFDVendor" UpdateMethod="UpdateRFDVendor">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="RFDNo" QueryStringField="RFDNo" Type="Int32" />
                    </SelectParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="RowID" Type="Int32" />
                        <asp:Parameter Name="original_RowID" Type="Int32" />
                        <asp:QueryStringParameter Name="RFDNo" QueryStringField="RFDNo" Type="Int32" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="original_RowID" Type="Int32" />
                        <asp:QueryStringParameter Name="RFDNo" QueryStringField="RFDNo" Type="Int32" />
                        <asp:Parameter Name="UGNDBVendorID" Type="Int32" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:QueryStringParameter Name="RFDNo" QueryStringField="RFDNo" Type="Int32" />
                        <asp:Parameter Name="UGNDBVendorID" Type="Int32" />
                    </InsertParameters>
                </asp:ObjectDataSource>
            </asp:View>
            <asp:View ID="vPackaging" runat="server">
                <asp:ValidationSummary runat="server" ID="vsPackaging" ValidationGroup="vgPackaging"
                    ShowMessageBox="true" ShowSummary="true" />
                <asp:Label ID="lblMessagePackaging" runat="server" SkinID="MessageLabelSkin" />
                <asp:ValidationSummary runat="server" ID="vsFinishedGoodPackaging" ValidationGroup="vgFinishedGoodPackaging"
                    ShowMessageBox="true" ShowSummary="true" />
                <asp:ValidationSummary runat="server" ID="vsChildPartPackaging" ValidationGroup="vgChildPartPackaging"
                    ShowMessageBox="true" ShowSummary="true" />
                <br />
                <asp:GridView ID="gvFinishedGoodPackaging" runat="server" AutoGenerateColumns="False"
                    AllowSorting="True" AllowPaging="True" PageSize="15" DataKeyNames="RFDNo" DataSourceID="odsFinishedGoodPackaging"
                    EmptyDataText="No customer parts found found" Width="98%">
                    <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#CCCCCC" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="NewCustomerPartNo" HeaderText="Cust PartNo" SortExpression="NewCustomerPartNo"
                            ReadOnly="True" ItemStyle-Font-Size="XX-Small" />
                        <asp:BoundField DataField="NewCustomerPartName" HeaderText="Name" SortExpression="NewCustomerPartName"
                            ReadOnly="True" ItemStyle-Font-Size="XX-Small" />
                        <asp:TemplateField HeaderText="Nbr<br />Containers" SortExpression="ContainerCount"
                            HeaderStyle-Font-Size="XX-Small">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditFinishedGoodPackagingContainerCount" runat="server" Text='<%# Bind("ContainerCount") %>'
                                    MaxLength="5" Width="30px" />
                                <asp:CompareValidator runat="server" ID="cvEditFinishedGoodPackagingContainerCount"
                                    Operator="DataTypeCheck" ValidationGroup="vgFinishedGoodPackaging" Type="double"
                                    Text="<" ControlToValidate="txtEditFinishedGoodPackagingContainerCount" ErrorMessage="Number of containers must be numeric"
                                    SetFocusOnError="True" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewFinishedGoodPackagingContainerCount" runat="server" Text='<%# Bind("ContainerCount") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Height" SortExpression="ContainerHeight">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditFinishedGoodPackagingContainerHeight" runat="server" Text='<%# Bind("ContainerHeight") %>'
                                    MaxLength="5" Width="30px" />
                                <asp:CompareValidator runat="server" ID="cvEditFinishedGoodPackagingContainerHeight"
                                    Operator="DataTypeCheck" ValidationGroup="vgFinishedGoodPackaging" Type="double"
                                    Text="<" ControlToValidate="txtEditFinishedGoodPackagingContainerHeight" ErrorMessage="Container Height must be numeric"
                                    SetFocusOnError="True" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewFinishedGoodPackagingContainerHeight" runat="server" Text='<%# Bind("ContainerHeight") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Unit" SortExpression="ContainerHeightUnitID">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddEditFinishedGoodPackagingContainerHeightUnit" runat="server"
                                    Font-Size="X-Small" DataSource='<%# Commonfunctions.GetUnit(0,"","") %>' DataValueField="UnitID"
                                    DataTextField="ddUnitAbbr" AppendDataBoundItems="True" Width="50px" SelectedValue='<%# Bind("ContainerHeightUnitID") %>'>
                                    <asp:ListItem Text="" Value="0" Selected="False"></asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewFinishedGoodPackagingContainerHeightUnit" runat="server" Text='<%# Bind("ContainerHeightUnitAbbr") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Width" SortExpression="ContainerWidth">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditFinishedGoodPackagingContainerWidth" runat="server" Text='<%# Bind("ContainerWidth") %>'
                                    MaxLength="5" Width="30px" />
                                <asp:CompareValidator runat="server" ID="cvEditFinishedGoodPackagingContainerWidth"
                                    Operator="DataTypeCheck" ValidationGroup="vgFinishedGoodPackaging" Type="double"
                                    Text="<" ControlToValidate="txtEditFinishedGoodPackagingContainerWidth" ErrorMessage="Container Width must be numeric"
                                    SetFocusOnError="True" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewFinishedGoodPackagingContainerWidth" runat="server" Text='<%# Bind("ContainerWidth") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Unit" SortExpression="ContainerWidthUnitID">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddEditFinishedGoodPackagingContainerWidthUnit" runat="server"
                                    Font-Size="X-Small" DataSource='<%# Commonfunctions.GetUnit(0,"","") %>' DataValueField="UnitID"
                                    DataTextField="ddUnitAbbr" AppendDataBoundItems="True" Width="50px" SelectedValue='<%# Bind("ContainerWidthUnitID") %>'>
                                    <asp:ListItem Text="" Value="0" Selected="False"></asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewFinishedGoodPackagingContainerWidthUnit" runat="server" Text='<%# Bind("ContainerWidthUnitAbbr") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Depth" SortExpression="ContainerDepth">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditFinishedGoodPackagingContainerDepth" runat="server" Text='<%# Bind("ContainerDepth") %>'
                                    MaxLength="5" Width="30px" />
                                <asp:CompareValidator runat="server" ID="cvEditFinishedGoodPackagingContainerDepth"
                                    Operator="DataTypeCheck" ValidationGroup="vgFinishedGoodPackaging" Type="double"
                                    Text="<" ControlToValidate="txtEditFinishedGoodPackagingContainerDepth" ErrorMessage="Container Depth must be numeric"
                                    SetFocusOnError="True" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewFinishedGoodPackagingContainerDepth" runat="server" Text='<%# Bind("ContainerDepth") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Unit" SortExpression="ContainerDepthUnitID">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddEditFinishedGoodPackagingContainerDepthUnit" runat="server"
                                    Font-Size="X-Small" DataSource='<%# Commonfunctions.GetUnit(0,"","") %>' DataValueField="UnitID"
                                    DataTextField="ddUnitAbbr" AppendDataBoundItems="True" Width="50px" SelectedValue='<%# Bind("ContainerDepthUnitID") %>'>
                                    <asp:ListItem Text="" Value="0" Selected="False" />
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewFinishedGoodPackagingContainerDepthUnit" runat="server" Text='<%# Bind("ContainerDepthUnitAbbr") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Annual<br />Vol." SortExpression="PackagingAnnualVolume">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditFinishedGoodPackagingContainerAnnualVolume" runat="server"
                                    Text='<%# Bind("PackagingAnnualVolume") %>' MaxLength="12" Width="80px" />
                                <asp:CompareValidator runat="server" ID="cvEditFinishedGoodPackagingContainerAnnualVolume"
                                    Operator="DataTypeCheck" ValidationGroup="vgFinishedGoodPackaging" Type="double"
                                    Text="<" ControlToValidate="txtEditFinishedGoodPackagingContainerAnnualVolume"
                                    ErrorMessage="Container Annual Volume must be numeric" SetFocusOnError="True" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewFinishedGoodPackagingContainerAnnualVolume" runat="server"
                                    Text='<%# Bind("PackagingAnnualVolume") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="System<br />Day<br />Count" SortExpression="SystemDayCount">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditFinishedGoodPackagingContainerSystemDayCount" runat="server"
                                    Text='<%# Bind("SystemDayCount") %>' MaxLength="5" Width="30px" />
                                <asp:CompareValidator runat="server" ID="cvEditFinishedGoodPackagingContainerSystemDayCount"
                                    Operator="DataTypeCheck" ValidationGroup="vgFinishedGoodPackaging" Type="double"
                                    Text="<" ControlToValidate="txtEditFinishedGoodPackagingContainerSystemDayCount"
                                    ErrorMessage="Container System Day Count must be numeric" SetFocusOnError="True" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewFinishedGoodPackagingContainerSystemDayCount" runat="server"
                                    Text='<%# Bind("SystemDayCount") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Comments" SortExpression="PackagingComments">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditFinishedGoodPackagingContainerComments" runat="server" Text='<%# Bind("PackagingComments") %>'
                                    MaxLength="400" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblFinishedGoodPackagingContainerComments" runat="server" Text='<%# Bind("PackagingComments") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <EditItemTemplate>
                                <asp:ImageButton ID="iBtnFinishedGoodPackagingUpdate" runat="server" CausesValidation="True"
                                    CommandName="Update" ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgFinishedGoodPackaging" />
                                <asp:ImageButton ID="iBtnPackagingCancel" runat="server" CausesValidation="False"
                                    CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="iBtnFinishedGoodPackagingEdit" runat="server" CausesValidation="False"
                                    CommandName="Edit" ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsFinishedGoodPackaging" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetRFDFinishedGoodPackaging" TypeName="RFDFinishedGoodPackagingBLL"
                    UpdateMethod="UpdateRFDFinishedGoodPackaging">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="RFDNo" QueryStringField="RFDNo" Type="Int32" />
                    </SelectParameters>
                    <UpdateParameters>
                        <asp:QueryStringParameter Name="RFDNo" QueryStringField="RFDNo" Type="Int32" />
                        <asp:Parameter Name="ContainerCount" Type="Double" />
                        <asp:Parameter Name="ContainerHeight" Type="Double" />
                        <asp:Parameter Name="ContainerHeightUnitID" Type="Int32" />
                        <asp:Parameter Name="ContainerWidth" Type="Double" />
                        <asp:Parameter Name="ContainerWidthUnitID" Type="Int32" />
                        <asp:Parameter Name="ContainerDepth" Type="Double" />
                        <asp:Parameter Name="ContainerDepthUnitID" Type="Int32" />
                        <asp:Parameter Name="PackagingAnnualVolume" Type="Double" />
                        <asp:Parameter Name="SystemDayCount" Type="Double" />
                        <asp:Parameter Name="PackagingComments" Type="String" />
                    </UpdateParameters>
                </asp:ObjectDataSource>
                <br />
                <asp:GridView ID="gvChildPartPackaging" runat="server" AutoGenerateColumns="False"
                    AllowSorting="True" AllowPaging="True" PageSize="15" DataKeyNames="RowID" DataSourceID="odsChildPartPackaging"
                    EmptyDataText="No child parts found" Width="98%">
                    <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#CCCCCC" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="NewPartNo" HeaderText="Part No" SortExpression="NewPartNo"
                            ReadOnly="True" ItemStyle-Font-Size="XX-Small">
                            <ItemStyle Font-Size="XX-Small" />
                        </asp:BoundField>
                        <asp:BoundField DataField="NewPartName" HeaderText="Name" SortExpression="NewPartName"
                            ReadOnly="True" ItemStyle-Font-Size="XX-Small">
                            <ItemStyle Font-Size="XX-Small" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Nbr<br />Containers" SortExpression="ContainerCount"
                            HeaderStyle-Font-Size="XX-Small">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditChildPartPackagingContainerCount" runat="server" Text='<%# Bind("ContainerCount") %>'
                                    MaxLength="5" Width="30px" />
                                <asp:CompareValidator runat="server" ID="cvEditChildPartPackagingContainerCount"
                                    Operator="DataTypeCheck" ValidationGroup="vgChildPartPackaging" Type="double"
                                    Text="<" ControlToValidate="txtEditChildPartPackagingContainerCount" ErrorMessage="Number of containers must be numeric"
                                    SetFocusOnError="True" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewChildPartPackagingContainerCount" runat="server" Text='<%# Bind("ContainerCount") %>' />
                            </ItemTemplate>
                            <HeaderStyle Font-Size="XX-Small" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Height" SortExpression="ContainerHeight">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditChildPartPackagingContainerHeight" runat="server" Text='<%# Bind("ContainerHeight") %>'
                                    MaxLength="5" Width="30px" />
                                <asp:CompareValidator runat="server" ID="cvEditChildPartPackagingContainerHeight"
                                    Operator="DataTypeCheck" ValidationGroup="vgChildPartPackaging" Type="double"
                                    Text="<" ControlToValidate="txtEditChildPartPackagingContainerHeight" ErrorMessage="Container Height must be numeric"
                                    SetFocusOnError="True" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewChildPartPackagingContainerHeight" runat="server" Text='<%# Bind("ContainerHeight") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Unit" SortExpression="ContainerHeightUnitID">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddEditChildPartPackagingContainerHeightUnit" runat="server"
                                    Font-Size="X-Small" DataSource='<%# Commonfunctions.GetUnit(0,"","") %>' DataValueField="UnitID"
                                    DataTextField="ddUnitAbbr" AppendDataBoundItems="True" Width="50px" SelectedValue='<%# Bind("ContainerHeightUnitID") %>'>
                                    <asp:ListItem Text="" Value="0" Selected="False"></asp:ListItem>
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewChildPartPackagingContainerHeightUnit" runat="server" Text='<%# Bind("ContainerHeightUnitAbbr") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Width" SortExpression="ContainerWidth">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditChildPartPackagingContainerWidth" runat="server" Text='<%# Bind("ContainerWidth") %>'
                                    MaxLength="5" Width="30px" />
                                <asp:CompareValidator runat="server" ID="cvEditChildPartPackagingContainerWidth"
                                    Operator="DataTypeCheck" ValidationGroup="vgChildPartPackaging" Type="double"
                                    Text="<" ControlToValidate="txtEditChildPartPackagingContainerWidth" ErrorMessage="Container Width must be numeric"
                                    SetFocusOnError="True" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewChildPartPackagingContainerWidth" runat="server" Text='<%# Bind("ContainerWidth") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Unit" SortExpression="ContainerWidthUnitID">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddEditChildPartPackagingContainerWidthUnit" runat="server"
                                    Font-Size="X-Small" DataSource='<%# Commonfunctions.GetUnit(0,"","") %>' DataValueField="UnitID"
                                    DataTextField="ddUnitAbbr" AppendDataBoundItems="True" Width="50px" SelectedValue='<%# Bind("ContainerWidthUnitID") %>'>
                                    <asp:ListItem Text="" Value="0" Selected="False" />
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewChildPartPackagingContainerWidthUnit" runat="server" Text='<%# Bind("ContainerWidthUnitAbbr") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Depth" SortExpression="ContainerDepth">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditChildPartPackagingContainerDepth" runat="server" Text='<%# Bind("ContainerDepth") %>'
                                    MaxLength="5" Width="30px" />
                                <asp:CompareValidator runat="server" ID="cvEditChildPartPackagingContainerDepth"
                                    Operator="DataTypeCheck" ValidationGroup="vgChildPartPackaging" Type="double"
                                    Text="<" ControlToValidate="txtEditChildPartPackagingContainerDepth" ErrorMessage="Container Depth must be numeric"
                                    SetFocusOnError="True" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewChildPartPackagingContainerDepth" runat="server" Text='<%# Bind("ContainerDepth") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Unit" SortExpression="ContainerDepthUnitID">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddEditChildPartPackagingContainerDepthUnit" runat="server"
                                    Font-Size="X-Small" DataSource='<%# Commonfunctions.GetUnit(0,"","") %>' DataValueField="UnitID"
                                    DataTextField="ddUnitAbbr" AppendDataBoundItems="True" Width="50px" SelectedValue='<%# Bind("ContainerDepthUnitID") %>'>
                                    <asp:ListItem Text="" Value="0" Selected="False" />
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewChildPartPackagingContainerDepthUnit" runat="server" Text='<%# Bind("ContainerDepthUnitAbbr") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Annual<br />Vol." SortExpression="PackagingAnnualVolume">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditChildPartPackagingContainerAnnualVolume" runat="server" Text='<%# Bind("PackagingAnnualVolume") %>'
                                    MaxLength="12" Width="80px" />
                                <asp:CompareValidator runat="server" ID="cvEditChildPartPackagingContainerAnnualVolume"
                                    Operator="DataTypeCheck" ValidationGroup="vgChildPartPackaging" Type="double"
                                    Text="<" ControlToValidate="txtEditChildPartPackagingContainerAnnualVolume" ErrorMessage="Container Annual Volume must be numeric"
                                    SetFocusOnError="True" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewChildPartPackagingContainerAnnualVolume" runat="server" Text='<%# Bind("PackagingAnnualVolume") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="System<br />Day<br />Count" SortExpression="SystemDayCount">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditChildPartPackagingContainerSystemDayCount" runat="server"
                                    Text='<%# Bind("SystemDayCount") %>' MaxLength="5" Width="30px" />
                                <asp:CompareValidator runat="server" ID="cvEditChildPartPackagingContainerSystemDayCount"
                                    Operator="DataTypeCheck" ValidationGroup="vgChildPartPackaging" Type="double"
                                    Text="<" ControlToValidate="txtEditChildPartPackagingContainerSystemDayCount"
                                    ErrorMessage="Container System Day Count must be numeric" SetFocusOnError="True" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewChildPartPackagingContainerSystemDayCount" runat="server" Text='<%# Bind("SystemDayCount") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Comments" SortExpression="PackagingComments">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditChildPartPackagingContainerComments" runat="server" Text='<%# Bind("PackagingComments") %>'
                                    MaxLength="400" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblChildPartPackagingContainerComments" runat="server" Text='<%# Bind("PackagingComments") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <EditItemTemplate>
                                <asp:ImageButton ID="iBtnChildPartPackagingUpdate" runat="server" CausesValidation="True"
                                    CommandName="Update" ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgChildPartPackaging" />
                                <asp:ImageButton ID="iBtnChildPartPackagingCancel" runat="server" CausesValidation="False"
                                    CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="iBtnChildPartPackagingEdit" runat="server" CausesValidation="False"
                                    CommandName="Edit" ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsChildPartPackaging" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetRFDChildPartPackaging" TypeName="RFDChildPartPackagingBLL" UpdateMethod="UpdateRFDChildPartPackaging">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="RFDNo" QueryStringField="RFDNo" Type="Int32" />
                    </SelectParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="original_RowID" Type="Int32" />
                        <asp:QueryStringParameter Name="RFDNo" QueryStringField="RFDNo" Type="Int32" />
                        <asp:Parameter Name="ContainerCount" Type="Double" />
                        <asp:Parameter Name="ContainerHeight" Type="Double" />
                        <asp:Parameter Name="ContainerHeightUnitID" Type="Int32" />
                        <asp:Parameter Name="ContainerWidth" Type="Double" />
                        <asp:Parameter Name="ContainerWidthUnitID" Type="Int32" />
                        <asp:Parameter Name="ContainerDepth" Type="Double" />
                        <asp:Parameter Name="ContainerDepthUnitID" Type="Int32" />
                        <asp:Parameter Name="PackagingAnnualVolume" Type="Double" />
                        <asp:Parameter Name="SystemDayCount" Type="Double" />
                        <asp:Parameter Name="PackagingComments" Type="String" />
                    </UpdateParameters>
                </asp:ObjectDataSource>
            </asp:View>
            <asp:View ID="vLaborOverHead" runat="server">
                <asp:ValidationSummary runat="server" ID="vsInsertLabor" ValidationGroup="vgInsertLabor"
                    ShowMessageBox="true" ShowSummary="true" />
                <asp:ValidationSummary runat="server" ID="vsEditLabor" ValidationGroup="vgEditLabor"
                    ShowMessageBox="true" ShowSummary="true" />
                <asp:Label ID="lblMessageLaborOverhead" runat="server" SkinID="MessageLabelSkin"></asp:Label>
                <asp:GridView ID="gvLabor" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                    AllowPaging="True" PageSize="15" DataKeyNames="RowID" DataSourceID="odsLabor"
                    EmptyDataText="No labor found" Width="98%" ShowFooter="True">
                    <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#CCCCCC" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField HeaderText="Labor" SortExpression="ddLaborDesc">
                            <EditItemTemplate>
                                <asp:Label ID="lblEditLaborMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                    Text="*" />
                                <asp:DropDownList ID="ddEditLabor" runat="server" DataSource='<%# CostingModule.GetLabor(0,"",0,0) %>'
                                    DataValueField="LaborID" DataTextField="ddLaborDesc" SelectedValue='<%# Bind("LaborID") %>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewLabor" runat="server" Text='<%# Bind("ddLaborDesc") %>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblInsertLaborMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                    Text="*" />
                                <asp:DropDownList ID="ddInsertLabor" runat="server" DataSource='<%# CostingModule.GetLabor(0,"",0,0) %>'
                                    OnSelectedIndexChanged="ddFooterLabor_SelectedIndexChanged" AutoPostBack="true"
                                    DataValueField="LaborID" DataTextField="ddLaborDesc" AppendDataBoundItems="true">
                                    <asp:ListItem Text="" Value="0" Selected="False" />
                                </asp:DropDownList>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rate" SortExpression="Rate">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditLaborRate" runat="server" Text='<%# Bind("Rate") %>' MaxLength="10"
                                    Width="50px" />
                                <asp:CompareValidator runat="server" ID="cvEditLaborRate" Operator="DataTypeCheck"
                                    ValidationGroup="vgEditLabor" Type="double" Text="<" ControlToValidate="txtEditLaborRate"
                                    ErrorMessage="Labor Rate must be numeric" SetFocusOnError="True" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewLaborRate" runat="server" Text='<%# Bind("Rate") %>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtInsertLaborRate" runat="server" Text='<%# Bind("Rate") %>' MaxLength="10"
                                    Width="50px" />
                                <asp:CompareValidator runat="server" ID="cvInsertLaborRate" Operator="DataTypeCheck"
                                    ValidationGroup="vgInsertLabor" Type="double" Text="<" ControlToValidate="txtInsertLaborRate"
                                    ErrorMessage="Labor Rate must be numeric" SetFocusOnError="True" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Crew Size" SortExpression="CrewSize">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditLaborCrewSize" runat="server" Text='<%# Bind("CrewSize") %>'
                                    MaxLength="10" Width="40px" />
                                <asp:CompareValidator runat="server" ID="cvEditLaborCrewSize" Operator="DataTypeCheck"
                                    ValidationGroup="vgEditLabor" Type="double" Text="<" ControlToValidate="txtEditLaborCrewSize"
                                    ErrorMessage="Labor Crew Size must be numeric" SetFocusOnError="True" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewLaborCrewSize" runat="server" Text='<%# Bind("CrewSize") %>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtInsertLaborCrewSize" runat="server" Text='<%# Bind("CrewSize") %>'
                                    MaxLength="10" Width="50px" />
                                <asp:CompareValidator runat="server" ID="cvInsertLaborCrewSize" Operator="DataTypeCheck"
                                    ValidationGroup="vgInsertLabor" Type="double" Text="<" ControlToValidate="txtInsertLaborCrewSize"
                                    ErrorMessage="Labor Crew Size must be numeric" SetFocusOnError="True" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Offline" SortExpression="isOffline">
                            <EditItemTemplate>
                                <asp:CheckBox ID="cbEditIsOffline" runat="server" Checked='<%# Bind("isOffline") %>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbViewIsOffline" runat="server" Checked='<%# Bind("isOffline") %>'
                                    Enabled="false" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:CheckBox ID="cbInsertIsOffline" runat="server" Checked='<%# Bind("isOffline") %>' />
                            </FooterTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <EditItemTemplate>
                                <asp:ImageButton ID="iBtnLaborUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                    ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditLabor" />
                                <asp:ImageButton ID="iBtnLaborCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                    ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="iBtnLaborEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                    ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                                <asp:ImageButton ID="iBtnLaborDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                    ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgInsertLabor"
                                    runat="server" ID="iBtnLaborSave" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                                <asp:ImageButton ID="iBtnLaborUndo" runat="server" CommandName="Undo" CausesValidation="false"
                                    ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsLabor" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetRFDLabor" TypeName="RFDLaborBLL" UpdateMethod="UpdateRFDLabor"
                    DeleteMethod="DeleteRFDLabor" InsertMethod="InsertRFDLabor">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="RFDNo" QueryStringField="RFDNo" Type="Int32" />
                    </SelectParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="RowID" Type="Int32" />
                        <asp:QueryStringParameter Name="RFDNo" QueryStringField="RFDNo" Type="Int32" />
                        <asp:Parameter Name="original_RowID" Type="Int32" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="original_RowID" Type="Int32" />
                        <asp:QueryStringParameter Name="RFDNo" QueryStringField="RFDNo" Type="Int32" />
                        <asp:Parameter Name="LaborID" Type="Int32" />
                        <asp:Parameter Name="Rate" Type="Double" />
                        <asp:Parameter Name="CrewSize" Type="Double" />
                        <asp:Parameter Name="isOffline" Type="Boolean" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:QueryStringParameter Name="RFDNo" QueryStringField="RFDNo" Type="Int32" />
                        <asp:Parameter Name="LaborID" Type="Int32" />
                        <asp:Parameter Name="Rate" Type="Double" />
                        <asp:Parameter Name="CrewSize" Type="Double" />
                        <asp:Parameter Name="isOffline" Type="Boolean" />
                    </InsertParameters>
                </asp:ObjectDataSource>
                <br />
                <asp:ValidationSummary runat="server" ID="vsInsertOverhead" ValidationGroup="vgInsertOverhead"
                    ShowMessageBox="true" ShowSummary="true" />
                <asp:ValidationSummary runat="server" ID="vsEditOverhead" ValidationGroup="vgEditOverhead"
                    ShowMessageBox="true" ShowSummary="true" />
                <asp:GridView ID="gvOverhead" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                    AllowPaging="True" PageSize="15" DataKeyNames="RowID" DataSourceID="odsOverhead"
                    EmptyDataText="No Overhead found" Width="98%" ShowFooter="True">
                    <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#CCCCCC" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField HeaderText="Overhead" SortExpression="ddLaborDesc">
                            <EditItemTemplate>
                                <asp:Label ID="lblEditOverheadMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                    Text="*" />
                                <asp:DropDownList ID="ddEditOverhead" runat="server" DataSource='<%# CostingModule.GetOverhead(0,"") %>'
                                    DataValueField="LaborID" DataTextField="ddLaborDesc" SelectedValue='<%# Bind("LaborID") %>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewOverhead" runat="server" Text='<%# Bind("ddLaborDesc") %>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblInsertOverheadMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                    Text="*" />
                                <asp:DropDownList ID="ddInsertOverhead" runat="server" DataSource='<%# CostingModule.GetOverhead(0,"") %>'
                                    OnSelectedIndexChanged="ddFooterOverhead_SelectedIndexChanged" AutoPostBack="true"
                                    DataValueField="LaborID" DataTextField="ddLaborDesc" AppendDataBoundItems="true">
                                    <asp:ListItem Text="" Value="0" Selected="False" />
                                </asp:DropDownList>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="FixedRate" SortExpression="FixedRate">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditOverheadFixedRate" runat="server" Text='<%# Bind("FixedRate") %>'
                                    MaxLength="10" Width="50px" />
                                <asp:CompareValidator runat="server" ID="cvEditOverheadFixedRate" Operator="DataTypeCheck"
                                    ValidationGroup="vgEditOverhead" Type="double" Text="<" ControlToValidate="txtEditOverheadFixedRate"
                                    ErrorMessage="Overhead Fixed Rate must be numeric" SetFocusOnError="True" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewOverheadFixedRate" runat="server" Text='<%# Bind("FixedRate") %>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtInsertOverheadFixedRate" runat="server" Text='<%# Bind("FixedRate") %>'
                                    MaxLength="10" Width="50px" />
                                <asp:CompareValidator runat="server" ID="cvInsertOverheadFixedRate" Operator="DataTypeCheck"
                                    ValidationGroup="vgInsertOverhead" Type="double" Text="<" ControlToValidate="txtInsertOverheadFixedRate"
                                    ErrorMessage="Overhead Fixed Rate must be numeric" SetFocusOnError="True" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="VariableRate" SortExpression="VariableRate">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditOverheadVariableRate" runat="server" Text='<%# Bind("VariableRate") %>'
                                    MaxLength="10" Width="50px" />
                                <asp:CompareValidator runat="server" ID="cvEditOverheadVariableRate" Operator="DataTypeCheck"
                                    ValidationGroup="vgEditOverhead" Type="double" Text="<" ControlToValidate="txtEditOverheadVariableRate"
                                    ErrorMessage="Overhead Variable Rate must be numeric" SetFocusOnError="True" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewOverheadVariableRate" runat="server" Text='<%# Bind("VariableRate") %>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtInsertOverheadVariableRate" runat="server" Text='<%# Bind("VariableRate") %>'
                                    MaxLength="5" Width="50px" />
                                <asp:CompareValidator runat="server" ID="cvInsertOverheadVariableRate" Operator="DataTypeCheck"
                                    ValidationGroup="vgInsertOverhead" Type="double" Text="<" ControlToValidate="txtInsertOverheadVariableRate"
                                    ErrorMessage="Overhead Variable Rate must be numeric" SetFocusOnError="True" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Crew Size" SortExpression="CrewSize">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditOverheadCrewSize" runat="server" Text='<%# Bind("CrewSize") %>'
                                    MaxLength="10" Width="50px" />
                                <asp:CompareValidator runat="server" ID="cvEditOverheadCrewSize" Operator="DataTypeCheck"
                                    ValidationGroup="vgEditOverhead" Type="double" Text="<" ControlToValidate="txtEditOverheadCrewSize"
                                    ErrorMessage="Overhead Crew Size must be numeric" SetFocusOnError="True" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewOverheadCrewSize" runat="server" Text='<%# Bind("CrewSize") %>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtInsertOverheadCrewSize" runat="server" Text='<%# Bind("CrewSize") %>'
                                    MaxLength="10" Width="50px" />
                                <asp:CompareValidator runat="server" ID="cvInsertOverheadCrewSize" Operator="DataTypeCheck"
                                    ValidationGroup="vgInsertOverhead" Type="double" Text="<" ControlToValidate="txtInsertOverheadCrewSize"
                                    ErrorMessage="Overhead Crew Size must be numeric" SetFocusOnError="True" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="NumberOfCarriers" SortExpression="NumberOfCarriers">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditOverheadNumberOfCarriers" runat="server" Text='<%# Bind("NumberOfCarriers") %>'
                                    MaxLength="5" Width="50px" />
                                <asp:CompareValidator runat="server" ID="cvEditOverheadNumberOfCarriers" Operator="DataTypeCheck"
                                    ValidationGroup="vgEditOverhead" Type="double" Text="<" ControlToValidate="txtEditOverheadNumberOfCarriers"
                                    ErrorMessage="Overhead Variable Rate must be numeric" SetFocusOnError="True" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewOverheadRate" runat="server" Text='<%# Bind("NumberOfCarriers") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtInsertOverheadNumberOfCarriers" runat="server" Text='<%# Bind("NumberOfCarriers") %>'
                                    MaxLength="10" Width="50px" />
                                <asp:CompareValidator runat="server" ID="cvInsertOverheadNumberOfCarriers" Operator="DataTypeCheck"
                                    ValidationGroup="vgInsertOverhead" Type="double" Text="<" ControlToValidate="txtInsertOverheadNumberOfCarriers"
                                    ErrorMessage="Overhead Variable Rate must be numeric" SetFocusOnError="True" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Offline" SortExpression="isOffline">
                            <EditItemTemplate>
                                <asp:CheckBox ID="cbEditIsOffline" runat="server" Checked='<%# Bind("isOffline") %>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbViewIsOffline" runat="server" Checked='<%# Bind("isOffline") %>'
                                    Enabled="false" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:CheckBox ID="cbInsertIsOffline" runat="server" Checked='<%# Bind("isOffline") %>' />
                            </FooterTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <EditItemTemplate>
                                <asp:ImageButton ID="iBtnOverheadUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                    ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditOverhead" />
                                <asp:ImageButton ID="iBtnOverheadCancel" runat="server" CausesValidation="False"
                                    CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="iBtnOverheadEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                    ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                                <asp:ImageButton ID="iBtnOverheadDelete" runat="server" CausesValidation="False"
                                    CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgInsertOverhead"
                                    runat="server" ID="iBtnOverheadSave" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                                <asp:ImageButton ID="iBtnOverheadUndo" runat="server" CommandName="Undo" CausesValidation="false"
                                    ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsOverhead" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetRFDOverhead" TypeName="RFDOverheadBLL" UpdateMethod="UpdateRFDOverhead"
                    DeleteMethod="DeleteRFDOverhead" InsertMethod="InsertRFDOverhead">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="RFDNo" QueryStringField="RFDNo" Type="Int32" />
                    </SelectParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="RowID" Type="Int32" />
                        <asp:QueryStringParameter Name="RFDNo" QueryStringField="RFDNo" Type="Int32" />
                        <asp:Parameter Name="original_RowID" Type="Int32" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="original_RowID" Type="Int32" />
                        <asp:QueryStringParameter Name="RFDNo" QueryStringField="RFDNo" Type="Int32" />
                        <asp:Parameter Name="LaborID" Type="Int32" />
                        <asp:Parameter Name="FixedRate" Type="Double" />
                        <asp:Parameter Name="VariableRate" Type="Double" />
                        <asp:Parameter Name="CrewSize" Type="Double" />
                        <asp:Parameter Name="NumberOfCarriers" Type="Double" />
                        <asp:Parameter Name="isOffline" Type="Boolean" />
                        <asp:Parameter Name="RowID" Type="Int32" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:QueryStringParameter Name="RFDNo" QueryStringField="RFDNo" Type="Int32" />
                        <asp:Parameter Name="LaborID" Type="Int32" />
                        <asp:Parameter Name="FixedRate" Type="Double" />
                        <asp:Parameter Name="VariableRate" Type="Double" />
                        <asp:Parameter Name="CrewSize" Type="Double" />
                        <asp:Parameter Name="NumberOfCarriers" Type="Double" />
                        <asp:Parameter Name="isOffline" Type="Boolean" />
                    </InsertParameters>
                </asp:ObjectDataSource>
            </asp:View>
            <asp:View ID="vProcess" runat="server">
                <table>
                    <tr>
                        <td class="p_text">
                            <asp:Label runat="server" ID="lblProcessNotes" Text="Process Notes:" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox runat="server" ID="txtProcessNotes" TextMode="MultiLine" Width="600px"
                                Height="80px" />
                            <br />
                            <asp:Label ID="lblProcessNotesCharCount" SkinID="MessageLabelSkin" runat="server" />
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Button ID="btnSaveProcess" runat="server" Text="Save" />
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vTooling" runat="server">
                <asp:ValidationSummary runat="server" ID="vsTooling" ValidationGroup="vgTooling"
                    ShowMessageBox="true" ShowSummary="true" />
                <br />
                <asp:Label runat="server" ID="lblMessageTooling" SkinID="MessageLabelSkin" />
                <br />
                <table>
                    <tr>
                        <td class="p_text">
                            <asp:Label runat="server" ID="lblCapitalNotes" Text="Capital Notes:" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox runat="server" ID="txtCapitalNotes" TextMode="MultiLine" Width="600px"
                                Height="80px" />
                            <br />
                            <asp:Label ID="lblCapitalNotesCharCount" SkinID="MessageLabelSkin" runat="server" />
                        </td>
                    </tr>
                    <tr class="p_text">
                        <td>
                            Capital Lead Time:
                        </td>
                        <td colspan="3" class="c_text">
                            <asp:TextBox runat="server" ID="txtCapitalLeadTime" MaxLength="2" Width="50px" />
                            <asp:CompareValidator runat="server" ID="cvCapitalLeadTime" Operator="DataTypeCheck"
                                ValidationGroup="vgTooling" Type="integer" Text="<" ControlToValidate="txtCapitalLeadTime"
                                ErrorMessage="Capital lead time must be an integer" SetFocusOnError="True" />
                            &nbsp;
                            <asp:DropDownList runat="server" ID="ddCapitalLeadUnits">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem Value="days">days</asp:ListItem>
                                <asp:ListItem Value="weeks">weeks</asp:ListItem>
                                <asp:ListItem Value="months">months</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label runat="server" ID="lblToolingNotes" Text="Tooling Notes:" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox runat="server" ID="txtToolingNotes" TextMode="MultiLine" Width="600px"
                                Height="80px" />
                            <br />
                            <asp:Label ID="lblToolingNotesCharCount" SkinID="MessageLabelSkin" runat="server" />
                        </td>
                    </tr>
                    <tr class="p_text">
                        <td>
                            Tooling Lead Time:
                        </td>
                        <td colspan="3" class="c_text">
                            <asp:TextBox runat="server" ID="txtToolingLeadTime" MaxLength="2" Width="50px" />
                            <asp:CompareValidator runat="server" ID="cvToolingLeadTime" Operator="DataTypeCheck"
                                ValidationGroup="vgTooling" Type="integer" Text="<" ControlToValidate="txtToolingLeadTime"
                                ErrorMessage="Tooling lead time must be an integer" SetFocusOnError="True" />
                            &nbsp;
                            <asp:DropDownList runat="server" ID="ddToolingLeadUnits">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem Value="days">days</asp:ListItem>
                                <asp:ListItem Value="weeks">weeks</asp:ListItem>
                                <asp:ListItem Value="months">months</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Button ID="btnSaveTooling" runat="server" Text="Save" ValidationGroup="vgTooling"
                                CausesValidation="true" />
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vSupportingDocs" runat="server">
                <asp:Label runat="server" ID="lblMessageSupportingDocs" SkinID="MessageLabelSkin"></asp:Label>
                <asp:Label runat="server" ID="lblSupportingFiles" Text="Supporting files" Font-Italic="true"
                    SkinID="StandardLabelSkin" Font-Bold="true"></asp:Label><br />
                <br />
                <asp:ValidationSummary ID="vsSupportingDocs" runat="server" DisplayMode="List" ShowMessageBox="true"
                    ShowSummary="true" ValidationGroup="vgSupportingDocs" />
                <br />
                <table width="98%">
                    <tr>
                        <td class="c_text" valign="top" colspan="2">
                            File Description: &nbsp;
                            <asp:TextBox ID="txtSupportingDocDesc" runat="server" MaxLength="200" Rows="3" TextMode="MultiLine"
                                Width="600px" />
                            <asp:RequiredFieldValidator ID="rfvSupportingDocDesc" runat="server" ControlToValidate="txtSupportingDocDesc"
                                ErrorMessage="Supporting Document File Description is a required field." Font-Bold="False"
                                ValidationGroup="vgSupportingDocs" SetFocusOnError="true" Text="<" /><br />
                            <br />
                            <asp:Label ID="lblSupportingDocDescCharCount" SkinID="MessageLabelSkin" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" align="right">
                            <asp:Label runat="server" ID="lblFileUploadLabel" Text="Upload a supporting (PDF,DOC,DOCX,XLS,XLSX,JPEG,TIF,PPT,PPTX) file under 3 MB:"
                                Visible="false" />
                        </td>
                        <td>
                            <asp:FileUpload runat="server" ID="fileUploadSupportingDoc" Width="334px" Visible="False" />
                            <asp:Button ID="btnSaveUploadSupportingDocument" runat="server" Text="Upload" Visible="False"
                                Width="67px" CausesValidation="true" ValidationGroup="vgSupportingDocs" />
                            <br />
                            <asp:RegularExpressionValidator ID="revUploadFile" runat="server" ErrorMessage="Please upload only *.PDF, *.DOC, *.XLS, *.JPEG, *.JPG, *.TIF, *. PPT files are allowed."
                                ValidationExpression="(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.pdf|.xls|.xlsx|.doc|.docx|.jpeg|.jpg|.tif|.ppt|.pptx|.msg|.PDF|.XLS|.XLSX|.DOC|.DOCX|.JPEG|.JPG|.TIF|.PPT|.PPTX|.MSG)$"
                                ControlToValidate="fileUploadSupportingDoc" ValidationGroup="vgSupportingDocs"
                                Font-Bold="True" Font-Size="Small" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Label runat="server" ID="lblMaxNote" Text="(A maximum of seven supporting documents are allowed.)"
                    Visible="false" />
                <br />
                <asp:GridView ID="gvSupportingDoc" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                    AllowPaging="True" PageSize="15" DataKeyNames="RowID" DataSourceID="odsSupportingDoc"
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
                        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="SubscriptionID" SortExpression="SubscriptionID">
                            <ItemTemplate>
                                <asp:Label ID="lblViewSubscriptionID" runat="server" Text='<%# Bind("SubscriptionID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Subscription" HeaderText="Role" SortExpression="Subscription"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="SupportingDocDesc" HeaderText="Desc" SortExpression="SupportingDocDesc"
                            ItemStyle-HorizontalAlign="Left" />
                        <asp:TemplateField HeaderText="Doc Name" SortExpression="SupportingDocName">
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkViewSupportingDoc" runat="server" NavigateUrl='<%# Eval("RowID", "~/RFD/RFD_Supporting_Doc_View.aspx?RowID={0}") %>'
                                    Target="_blank" Text='<%# Eval("SupportingDocName") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="iBtnSupportingDocDelete" runat="server" CausesValidation="False"
                                    CssClass="none" CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete"
                                    OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsSupportingDoc" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetRFDSupportingDoc" TypeName="RFDSupportingDocBLL" DeleteMethod="DeleteRFDSupportingDoc">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="RFDNo" QueryStringField="RFDNo" Type="Int32" />
                    </SelectParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="RowID" Type="Int32" />
                        <asp:Parameter Name="original_RowID" Type="Int32" />
                    </DeleteParameters>
                </asp:ObjectDataSource>
                <br />
                <asp:Label runat="server" ID="lblMessageSupportingDocsBottom" SkinID="MessageLabelSkin" />
                <br />
                <asp:Label runat="server" ID="lblNetworkFileTitle" Text="Network File References (for example customer drawings)"
                    Font-Italic="true" SkinID="StandardLabelSkin" Font-Bold="true" /><br />
                <asp:Label runat="server" ID="lblNetworkFileNote" Text="Note: This will only save the location of the file and NOT the file itself."
                    Font-Italic="true" Font-Size="XX-Small" /><br />
                <asp:Label runat="server" ID="lblNetworkFileWarning" Text="Warning: If the file is moved or deleted, the reference will be broken."
                    Font-Italic="true" Font-Size="XX-Small" /><br />
                <table width="98%">
                    <tr>
                        <td class="p_text" style="white-space: nowrap;" align="right">
                            <asp:Label runat="server" ID="lblNetworkFileLabel" Text="Network File Name:" Visible="false" />
                        </td>
                        <td>
                            <div id="uploadFile_div">
                                <input id="fileInputNetworkFileReference" name="fileInputNetworkFileReference" type="file"
                                    size="70" runat="server" onchange="saveNetworkFileName();" style="display: none" />
                            </div>
                            <input id="fileTextNetworkFileReference" name="fileTextNetworkFileReference" type="text"
                                runat="server" maxlength="200" />
                            <input type="button" id="btnBrowserNetworkFileReference" name="btnBrowserNetworkFileReference"
                                runat="server" value="Browse" onclick="doBrowseClick();" />
                            <asp:Button ID="btnSaveNetworkFileReference" runat="server" Text="Save File Reference"
                                ValidationGroup="vgSupportingDocs" Visible="False"></asp:Button>
                            <asp:RegularExpressionValidator ID="revTextNetworkFileReference" runat="server" ErrorMessage="Please use a valid windows file name."
                                ControlToValidate="fileTextNetworkFileReference" ValidationGroup="vgSupportingDocs"
                                Font-Bold="True" Font-Size="Small" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:GridView ID="gvNetworkFiles" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                    AllowPaging="True" PageSize="15" DataKeyNames="RowID" DataSourceID="odsNetworkFiles"
                    EmptyDataText="No file references exist yet." Width="98%">
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
                        <asp:TemplateField HeaderText="File Name" SortExpression="NetworkFilesName">
                            <ItemTemplate>
                                <a href='<%# Eval("FilePath") %>' target="_blank">
                                    <%# Eval("FilePath") %>
                                </a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="iBtnNetworkFilesDelete" runat="server" CausesValidation="False"
                                    CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsNetworkFiles" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetRFDNetworkFiles" TypeName="RFDNetworkFilesBLL" DeleteMethod="DeleteRFDNetworkFile">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="RFDNo" QueryStringField="RFDNo" Type="Int32" />
                    </SelectParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="RowID" Type="Int32" />
                        <asp:QueryStringParameter Name="RFDNo" QueryStringField="RFDNo" Type="Int32" />
                        <asp:Parameter Name="original_RowID" Type="Int32" />
                    </DeleteParameters>
                </asp:ObjectDataSource>
            </asp:View>
            <asp:View ID="vApprovalStatus" runat="server">
                <asp:Label runat="server" ID="lblMessageApproval" SkinID="MessageLabelSkin"></asp:Label>
                <br />
                <asp:ValidationSummary ID="vsApproval" runat="server" DisplayMode="List" ShowMessageBox="true"
                    ShowSummary="true" EnableClientScript="true" ValidationGroup="vgApproval" />
                <table width="60%" runat="server" id="tblCurrentApprover" visible="false">
                    <tr>
                        <td class="p_text" style="white-space: nowrap">
                            Team Member:
                        </td>
                        <td style="white-space: nowrap">
                            <asp:Label ID="lblApprovalTeamMember" runat="server" CssClass="c_text" Style="color: #990000;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Role:
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddApprovalSubscription" runat="server" AutoPostBack="true">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" style="vertical-align: top; white-space: nowrap">
                            <asp:Label ID="lblApprovalNumberOfCavitiesMarker" runat="server" Font-Bold="True"
                                ForeColor="Red" Text="*" Visible="false" />
                            <asp:Label ID="lblApprovalNumberOfCavitiesLabel" runat="server" Text="Number of Cavities:"
                                Visible="false" />
                        </td>
                        <td style="white-space: nowrap">
                            <asp:TextBox ID="txtApprovalNumberOfCavities" runat="server" MaxLength="3" Visible="false"></asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="ftApprovalNumberOfCavities" runat="server" TargetControlID="txtApprovalNumberOfCavities"
                                FilterType="Custom, Numbers" ValidChars="-." />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" style="vertical-align: top; white-space: nowrap">
                            <asp:Label ID="lblApprovalCommentsMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="*" Visible="false" />
                            Comment:
                        </td>
                        <td style="white-space: nowrap">
                            <asp:TextBox ID="txtApprovalComments" runat="server" MaxLength="200" Rows="3" TextMode="MultiLine"
                                Width="350px" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvApprovalComments" EnableClientScript="true"
                                Enabled="false" ControlToValidate="txtApprovalComments" ErrorMessage="Comments are needed for rejection"
                                ValidationGroup="vgApproval" SetFocusOnError="True" Text="<" /><br />
                            <asp:Label ID="lblApprovalCommentsCharCount" runat="server" Font-Bold="True" ForeColor="Red" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Approval Status:
                        </td>
                        <td style="vertical-align: top" colspan="3">
                            <asp:DropDownList ID="ddApprovalStatus" runat="server" AutoPostBack="true" />
                            <asp:RangeValidator ID="rvApprovalStatus" runat="server" ValidationGroup="vgApproval"
                                ErrorMessage="Approval Status must be set to approve or reject only" ControlToValidate="ddApprovalStatus"
                                MaximumValue="9" MinimumValue="3" SetFocusOnError="True" Text="<" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="3">
                            <asp:Button ID="btnApprovalStatusSubmit" runat="server" Text="Submit" ValidationGroup="vgApproval"
                                CausesValidation="true" />
                            <asp:Button ID="btnApprovalStatusReset" runat="server" Text="Reset" />
                        </td>
                    </tr>
                </table>
                <hr />
                <asp:Label runat="server" ID="lblNoteApprovalStatus" Text="Below is the status of approval information.<br /><br />Costing does not approve until all of the routing level 1 team members have approved.<br />The Quality Engineer does not approve until Costing has approved.<br />Purchasing does not approve until all other team members have approved.<br /><br />IN ADDITION, for the Business Process Type of RFQ, the Quality Engineer and Purchasing team member would not approve until business was awarded."
                    Font-Italic="true" Font-Size="XX-Small" />
                <br />
                <br />
                <asp:ValidationSummary ID="vsEditApproval" runat="server" DisplayMode="List" EnableClientScript="true"
                    ShowMessageBox="True" ValidationGroup="vgEditApproval" />
                <asp:GridView ID="gvApproval" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                    AllowPaging="True" PageSize="15" DataKeyNames="RowID" DataSourceID="odsApproval"
                    EmptyDataText="No approvers assigned yet" Width="98%" ShowFooter="false">
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#CCCCCC" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField HeaderText="SubscriptionID" SortExpression="SubscriptionID">
                            <EditItemTemplate>
                                <asp:Label ID="lblEditSubscriptionID" runat="server" Text='<%# Bind("SubscriptionID") %>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewSubscriptionID" runat="server" Text='<%# Bind("SubscriptionID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="TeamMemberID" SortExpression="TeamMemberID">
                            <EditItemTemplate>
                                <asp:Label ID="lblEditTeamMemberID" runat="server" Text='<%# Bind("TeamMemberID") %>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewTeamMemberID" runat="server" Text='<%# Bind("TeamMemberID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="RoutingLevel" HeaderText="Approval Level" ReadOnly="True"
                            HeaderStyle-Wrap="true" ItemStyle-HorizontalAlign="center" ItemStyle-Width="10%"
                            ItemStyle-Wrap="true" />
                        <asp:BoundField DataField="Subscription" HeaderText="Role" ReadOnly="True" />
                        <asp:TemplateField HeaderText="Team Member">
                            <EditItemTemplate>
                                <asp:Label ID="lblEditApproverTeamMemberMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                    Text="*" />
                                <asp:DropDownList ID="ddEditApproverTeamMember" runat="server" DataSource='<%# GetTeamMembersBySelectedSubscription() %>'
                                    DataValueField="TMID" DataTextField="TMName" SelectedValue='<%# Bind("TeamMemberID") %>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewApproverTeamMember" runat="server" Text='<%# Bind("FullTeamMemberName") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Comments" HeaderText="Comments" ReadOnly="True" />
                        <asp:BoundField DataField="CavityCount" HeaderText="Cavity Count Check" ReadOnly="True"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="ddStatusName" HeaderText="Status" ReadOnly="True" />
                        <asp:BoundField DataField="StatusDate" HeaderText="Status Date" ReadOnly="True" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="NotificationDate" HeaderText="Notification Date" ItemStyle-HorizontalAlign="Center"
                            ReadOnly="True" />
                        <asp:TemplateField>
                            <EditItemTemplate>
                                <asp:ImageButton ID="iBtnUpdateApprover" runat="server" CommandName="Update" ImageUrl="~/images/save.jpg"
                                    AlternateText="Update" />
                                <asp:ImageButton ID="iBtnCancelApprover" runat="server" CausesValidation="False"
                                    CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="iBtnEditApprover" runat="server" CausesValidation="False" CommandName="Edit"
                                    ImageUrl="~/images/edit.jpg" AlternateText="Edit" CssClass="none" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsApproval" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetRFDApproval" TypeName="RFDApprovalBLL" UpdateMethod="UpdateRFDApproval">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="RFDNo" QueryStringField="RFDNo" Type="Int32" />
                        <asp:Parameter Name="SubscriptionID" Type="Int32" />
                        <asp:Parameter Name="TeamMemberID" Type="Int32" />
                        <asp:Parameter Name="filterNotified" Type="Boolean" />
                        <asp:Parameter Name="isNotified" Type="Boolean" />
                        <asp:Parameter DefaultValue="False" Name="isHistorical" Type="Boolean" />
                        <asp:Parameter DefaultValue="False" Name="filterWorking" Type="Boolean" />
                        <asp:Parameter DefaultValue="False" Name="isWorking" Type="Boolean" />
                    </SelectParameters>
                    <UpdateParameters>
                        <asp:QueryStringParameter Name="RFDNo" QueryStringField="RFDNo" Type="Int32" />
                        <asp:Parameter Name="SubscriptionID" Type="Int32" />
                        <asp:Parameter Name="TeamMemberID" Type="Int32" />
                    </UpdateParameters>
                </asp:ObjectDataSource>
                <br />
                <table>
                    <tr align="center">
                        <td>
                            <asp:Button ID="btnSubmitApproval" runat="server" Text="Submit for Approval" />
                            &nbsp;<asp:CheckBox runat="server" ID="cbMeetingRequired" Checked="true" Text="An RFD Meeting is required." />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Label runat="server" ID="lblMessageApprovalBottom" SkinID="MessageLabelSkin" />
            </asp:View>
            <asp:View ID="vCommunicationBoard" runat="server">
                <asp:Label runat="server" ID="lblMessageCommunicationBoard" SkinID="MessageLabelSkin" />
                <asp:ValidationSummary ID="vsCommunicationBoard" runat="server" ValidationGroup="vgCommunicationBoard"
                    ShowMessageBox="true" ShowSummary="true" />
                <table runat="server" id="tblCommunicationBoardNewQuestion" visible="false">
                    <tr>
                        <td class="p_text" valign="top">
                            <asp:Label ID="lblRSSComment" runat="server" Font-Bold="True" ForeColor="Red" Text="*" />
                            New Question:
                        </td>
                        <td>
                            <asp:TextBox ID="txtRSSComment" runat="server" Width="550px" TextMode="MultiLine"
                                Rows="3" />
                            <asp:RequiredFieldValidator ID="rfvRSSComment" runat="server" ControlToValidate="txtRSSComment"
                                ErrorMessage="Question / Comment is a required field." ValidationGroup="vgCommunicationBoard"><</asp:RequiredFieldValidator>
                            <br />
                            <asp:Label ID="lblRSSCommentCharCount" runat="server" SkinID="MessageLabelSkin" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnRSSSubmit" runat="server" Text="Submit" CausesValidation="true"
                                ValidationGroup="vgCommunicationBoard" />
                            <asp:Button ID="btnRSSReset" runat="server" Text="Reset" CausesValidation="False" />
                        </td>
                    </tr>
                </table>
                <asp:Label ID="lblSQC" runat="server" CssClass="p_smalltextbold" Style="width: 532px;
                    color: #990000" Text="Select a 'Question / Comment' from discussion thread below to respond." />
                <table runat="server" id="tblCommunicationBoardExistingQuestion" visible="false">
                    <tr>
                        <td class="p_text" valign="top">
                            Question to Answer:
                        </td>
                        <td class="c_text">
                            <asp:TextBox ID="txtQuestionComment" runat="server" Font-Bold="True" Rows="3" TextMode="MultiLine"
                                Width="550px" Enabled="False" />
                            <asp:RequiredFieldValidator ID="rfvQuestionComment" runat="server" ErrorMessage="Select a Question / Comment from table below for response."
                                ValidationGroup="vgReplyComment" ControlToValidate="txtQuestionComment"><</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" valign="top">
                            <asp:Label ID="lblReqReply" runat="server" Text="*" ForeColor="Red" />
                            Reply / Comment:
                        </td>
                        <td>
                            <asp:TextBox ID="txtReply" runat="server" Rows="3" TextMode="MultiLine" Width="550px" />
                            <asp:RequiredFieldValidator ID="rfvReply" runat="server" ErrorMessage="Reply / Comment is a required field."
                                SetFocusOnError="true" ValidationGroup="vgReplyComment" ControlToValidate="txtReply"><</asp:RequiredFieldValidator><br />
                            <br />
                            <asp:Label runat="server" ID="lblReplyCharCount" SkinID="MessageLabelSkin" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Button ID="btnSaveReplyComment" runat="server" Text="Submit" CausesValidation="true"
                                ValidationGroup="vgReplyComment" Visible="false" />
                            <asp:Button ID="btnResetReplyComment" runat="server" Text="Reset" CausesValidation="False"
                                Visible="false" />
                        </td>
                    </tr>
                </table>
                <asp:ValidationSummary ID="vsReplyComment" runat="server" ValidationGroup="vgReplyComment"
                    ShowMessageBox="true" ShowSummary="true" />
                <br />
                <asp:GridView ID="gvQuestion" runat="server" AutoGenerateColumns="False" DataSourceID="odsQuestion"
                    Width="900px" RowStyle-BorderStyle="None" EmptyDataText="No Questions have been submitted.">
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" Wrap="False" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Height="20px" />
                    <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <Columns>
                        <asp:TemplateField HeaderText="Reply">
                            <ItemTemplate>
                                <asp:ImageButton ID="ibtnAppendReply" runat="server" CausesValidation="False" OnClick="gvQuestionAppendReply_Click"
                                    ToolTip="Reply" ImageUrl="~/images/messanger30.jpg" AlternateText='<%# Bind("Comment") %>'
                                    CommandName='<%# Bind("RSSID") %>' />&nbsp;&nbsp;&nbsp;
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="RSSID" HeaderText="RSSID" SortExpression="RSSID" Visible="false" />
                        <asp:BoundField DataField="Comment" HeaderText="Question / Comment" SortExpression="Comment"
                            HeaderStyle-Width="500px" ItemStyle-Width="500px" ItemStyle-Font-Bold="true"
                            ItemStyle-CssClass="c_text" />
                        <asp:BoundField DataField="ddTeamMemberName" HeaderText="Submitted By" SortExpression="ddTeamMemberName"
                            HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-Font-Bold="true" />
                        <asp:BoundField DataField="PostDate" HeaderText="Post Date" SortExpression="PostDate"
                            ItemStyle-Font-Bold="true" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                    </td>
                                    <td colspan="3">
                                        <asp:GridView ID="gvReply" runat="server" AutoGenerateColumns="False" DataSourceID="odsReply"
                                            DataKeyNames="RFDNo,RSSID" Width="100%">
                                            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" Wrap="False" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Height="20px" />
                                            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            <HeaderStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="red" HorizontalAlign="Left" />
                                            <EditRowStyle BackColor="#E2DED6" />
                                            <EmptyDataRowStyle Wrap="False" />
                                            <Columns>
                                                <asp:BoundField DataField="Comment" HeaderText="Response" SortExpression="Comment"
                                                    HeaderStyle-Width="500px" ItemStyle-Width="500px" ItemStyle-Font-Bold="true" />
                                                <asp:BoundField DataField="ddTeamMemberName" HeaderText="" SortExpression="ddTeamMemberName"
                                                    HeaderStyle-Width="100px" ItemStyle-Width="100px" />
                                                <asp:BoundField DataField="PostDate" HeaderText="" SortExpression="PostDate" />
                                            </Columns>
                                        </asp:GridView>
                                        <asp:ObjectDataSource ID="odsReply" runat="server" OldValuesParameterFormatString="original_{0}"
                                            SelectMethod="GetRFDRSSReply" TypeName="RFDRSSReplyBLL">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="lblRFDNo" Name="RFDNo" PropertyName="Text" Type="Int32" />
                                                <asp:Parameter Name="RSSID" Type="Int32" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsQuestion" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetRFDRSS" TypeName="RFDRSSBLL">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="lblRFDNo" Name="RFDNo" PropertyName="Text" Type="Int32" />
                        <asp:Parameter Name="RSSID" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </asp:View>
        </asp:MultiView>
        <table width="100%">
            <tr>
                <td align="center">
                    &nbsp;
                    <asp:Button ID="btnPreviewBottom" runat="server" Text="Preview" Visible="false" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
