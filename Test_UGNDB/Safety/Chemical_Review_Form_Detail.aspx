<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" MaintainScrollPositionOnPostback="true"
    AutoEventWireup="false" CodeFile="Chemical_Review_Form_Detail.aspx.vb" Inherits="Safety_Chemical_Review_Form_Detail"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSave">
        <asp:Label ID="lblMessage" SkinID="MessageLabelSkin" runat="server"></asp:Label>
        <asp:ValidationSummary ID="vsSave" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgSave" />
        <asp:Label runat="server" ID="lblTimingTip" style="font-weight:bold" Text="If the form is not approved after 7 calendar days from the Request Date, it will be automatically voided."></asp:Label>
        <table width="68%">
            <tr>
                <td class="p_text">
                    <asp:Label runat="server" ID="lblChemicalReviewFormIDLabel" Text="Chemical Review Formt ID:"
                        Visible="false"></asp:Label>
                </td>
                <td class="c_textbold">
                    <asp:Label runat="server" ID="lblChemicalReviewFormIDValue" Visible="false"></asp:Label>
                </td>
                <td class="p_text">
                    <asp:Label runat="server" ID="lblOverallStatus" Text="Overall Status:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddStatus" runat="server" Enabled="false" Visible="false">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:CheckBox runat="server" ID="cbActive" text="Active" Visible="false" Enabled="false" />
                </td>
                <td>
                    <asp:Button runat="server" ID="btnUpdateActive" Text="Update" Visible="false" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblLastUpdatedByLabel" Text="Last Updated By:" runat="server" Visible="false" />
                </td>
                <td class="c_text">
                    <asp:Label ID="lblLastUpdatedByValue" runat="server" Visible="false" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblLastUpdatedOnLabel" Text="Last Updated On:" runat="server" Visible="false"></asp:Label>
                </td>
                <td class="c_text" colspan="2">
                    <asp:Label ID="lblLastUpdatedOnValue" runat="server" Visible="false"></asp:Label>
                </td>
            </tr>
        </table>
        <table width="68%">
            <tr>
                <td align="left" colspan="2">
                    <asp:Label runat="server" ID="lblVoidComment" Text="Void Comment:" Visible="false"></asp:Label>
                    <asp:Label ID="lblVoidCommentMarker" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="*" Visible="false" />
                    <asp:TextBox runat="server" ID="txtVoidComment" MaxLength="100" TextMode="MultiLine"
                        Visible="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Button ID="btnPreview" runat="server" Text="Preview" Visible="false" />
                    &nbsp;
                    <asp:Button ID="btnCopy" runat="server" Text="Copy" Visible="false" />
                </td>
                <td align="left">
                    &nbsp;
                    <asp:Button ID="btnVoid" runat="server" Text="Void" Visible="false" />
                    &nbsp;
                    <asp:Button ID="btnVoidCancel" runat="server" Text="Cancel Void" Visible="false" />
                    &nbsp;
                </td>
            </tr>
        </table>
        <asp:Menu ID="menuTabs" Height="30px" runat="server" Orientation="Horizontal" StaticEnableDefaultPopOutImage="False"
            StaticMenuItemStyle-CssClass="tab" StaticSelectedStyle-CssClass="selectedTab"
            CssClass="tabs">
            <Items>
                <asp:MenuItem Text="Description" Value="0" ImageUrl=""></asp:MenuItem>
                <asp:MenuItem Text="Approval" Value="1" ImageUrl=""></asp:MenuItem>
            </Items>
        </asp:Menu>
        <asp:MultiView ID="mvTabs" runat="server" Visible="true" ActiveViewIndex="0" EnableViewState="true">
            <asp:View ID="vDescription" runat="server">
                <table width="98%">
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblUGNFacilityMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="*" />
                            UGN Facility:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddUGNFacility" runat="server" AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvUGNFacility" runat="server" ControlToValidate="ddUGNFacility"
                                ErrorMessage="UGN Facility is required." Font-Bold="True" ValidationGroup="vgSave"
                                Text="<" SetFocusOnError="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblRequestedByMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="*" />
                            Requested By:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddRequestedByTeamMember" runat="server">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvRequestedByTeamMember" runat="server" ControlToValidate="ddRequestedByTeamMember"
                                ErrorMessage="Requested by team member is required." Font-Bold="True" ValidationGroup="vgSave"
                                Text="<" SetFocusOnError="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblRequestedDateMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="*" />
                            Request Date:
                        </td>
                        <td>
                            <asp:TextBox ID="txtRequestDate" runat="server" MaxLength="10"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="rfvRequestDate" ControlToValidate="txtRequestDate"
                                SetFocusOnError="true" ErrorMessage="Request date is required" Text="<" ValidationGroup="vgSave" />
                            <asp:ImageButton runat="server" ID="imgRequestDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                            <ajax:CalendarExtender ID="ceRequestDate" runat="server" TargetControlID="txtRequestDate"
                                PopupButtonID="imgRequestDate" />
                            <asp:RegularExpressionValidator ID="revRequestDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                ControlToValidate="txtRequestDate" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                Width="8px" ValidationGroup="vgSave" Text="<"></asp:RegularExpressionValidator>
                            <asp:RangeValidator ID="rvRequestDate" runat="server" Font-Bold="True" Type="Date"
                                ToolTip="The date must be between 1950 and 2100" ErrorMessage="Invalid Date Entry: The date must be between 1950 and 2100"
                                Text="<" ValidationGroup="vgSave" MaximumValue="01/01/2100" MinimumValue="01/01/1950"
                                ControlToValidate="txtRequestDate"></asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblProductNameMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="*" />
                            Product Name:
                        </td>
                        <td>
                            <asp:TextBox ID="txtProductName" runat="Server" MaxLength="50">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvProductName" runat="server" ControlToValidate="txtProductName"
                                ErrorMessage="Product name is required." Font-Bold="True" ValidationGroup="vgSave"
                                Text="<" SetFocusOnError="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblProductManufacturerMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="*" />
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
                    </tr>
                    <tr>
                        <td class="p_text">
                            Department / Area:
                        </td>
                        <td>
                            <asp:TextBox ID="txtDeptArea" runat="server" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Description:
                        </td>
                        <td>
                            <asp:TextBox ID="txtChemicalDesc" TextMode="MultiLine" runat="Server" Rows="2" Height="100px"
                                Width="400px" />
                            <br />
                            <asp:Label ID="lblChemicalDescCharCount" SkinID="MessageLabelSkin" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Use of Material:
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="cbProductionUsage" Text="Production" />
                            <asp:CheckBox runat="server" ID="cbLabUsage" Text="Lab" />
                            <asp:CheckBox runat="server" ID="cbMaintenanceUsage" Text="Maintenance" />
                            <asp:CheckBox runat="server" ID="cbOtherUsage" Text="Other" />
                            <asp:TextBox runat="server" ID="txtOtherUsageDesc" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" valign="top">
                            Labeling:
                        </td>
                        <td valign="top">
                            <table>
                                <tr>
                                    <td valign="top">
                                        Health:
                                    </td>
                                    <td valign="top">
                                        <asp:DropDownList runat="server" ID="ddHealthLevel">
                                            <asp:ListItem Value="0" Text="0"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Flammability:
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddFlammabilityLevel">
                                            <asp:ListItem Value="0" Text="0"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Reactivity:
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddReactivityLevel">
                                            <asp:ListItem Value="0" Text="0"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Protective Equipment:
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddProtectiveEquipmentLevel">
                                            <asp:ListItem Value="0" Text="0"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Hazards:
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="cbPhysicalHazard" Text="Physical" />
                            <asp:CheckBox runat="server" ID="cbHealthHazard" Text="Health" />
                            <asp:CheckBox runat="server" ID="cbEnvironmentalHazard" Text="Environmental" />
                            <asp:CheckBox runat="server" ID="cbOtherHazard" Text="Other" />
                            <asp:TextBox runat="server" ID="txtOtherHazardDesc" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Personal Protective Equipment:
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="cbGlovesEquip" Text="Gloves" />
                            <asp:CheckBox runat="server" ID="cbGogglesEquip" Text="Goggles" />
                            <asp:CheckBox runat="server" ID="cbRespiratoryEquip" Text="Respiratory" />
                            <asp:CheckBox runat="server" ID="cbProtectiveClothingEquip" Text="Protective Clothing" />
                            <asp:CheckBox runat="server" ID="cbOtherEquip" Text="Other" />
                            <asp:TextBox runat="server" ID="txtOtherEquipDesc" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Engineering Controls:
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="cbVentilationEng" Text="Ventilation" />
                            <asp:CheckBox runat="server" ID="cbContainmentEng" Text="Containment" />
                            <asp:CheckBox runat="server" ID="cbOtherEng" Text="Other" />
                            <asp:TextBox runat="server" ID="txtOtherEngDesc" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Incompatible With:
                        </td>
                        <td>
                            <asp:TextBox ID="txtIncompatibleWith" runat="server" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Storage / Handling Reqs:
                        </td>
                        <td>
                            <asp:TextBox ID="txtStorageDesc" TextMode="MultiLine" runat="Server" Rows="2" Height="100px"
                                Width="400px">
                            </asp:TextBox>
                            <br />
                            <asp:Label ID="lblStorageDescCharCount" SkinID="MessageLabelSkin" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblDisposalDescMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="*" />
                            Disposal:
                        </td>
                        <td>
                            <asp:TextBox ID="txtDisposalDesc" TextMode="MultiLine" runat="Server" Rows="2" Height="100px"
                                Width="400px">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvDisposal" runat="server" ControlToValidate="txtDisposalDesc"
                                ErrorMessage="Disposal method is required." Font-Bold="True" ValidationGroup="vgSave"
                                Text="<" SetFocusOnError="true" />
                            <br />
                            <asp:Label ID="lblDisposalDescCharCount" SkinID="MessageLabelSkin" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Environmental:
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="cbMSDSEnv" Text="MSDS" />
                            <asp:CheckBox runat="server" ID="cbAspectListEnv" Text="Aspect List" />
                            <asp:CheckBox runat="server" ID="cbEMPEnv" Text="EMP" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Aspect Type:
                        </td>
                        <td>
                            <asp:RadioButtonList runat="server" ID="rbAspectType" RepeatDirection="Horizontal">
                                <asp:ListItem Value="S" Text="Significant"></asp:ListItem>
                                <asp:ListItem Value="N" Text="Normal"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
                <table width="68%">
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnPreviewBottom" runat="server" Text="Preview" Visible="false" />
                            &nbsp;
                            <asp:Button runat="server" ID="btnSave" Text="Save" ValidationGroup="vgSave" />
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vApprovals" runat="server">
                <asp:ValidationSummary ID="vsSaveApprovers" runat="server" DisplayMode="List" ShowMessageBox="true"
                    ShowSummary="true" EnableClientScript="true" ValidationGroup="vgSaveApprovers" />
                <br />
                <asp:Label runat="server" ID="lblApprovalTip" style="font-weight:bold" Text="For approval, a minumum of at least the HR Safety Manager and one Enrironmental Engineer (Plant or Corpotate) are required, but all roles are required for notification. If all required team members approve the form, other will have the opportunity to review and update the form until up to 7 calendar days from the Request Date."></asp:Label>
                <br />
                <br />
                <asp:Label runat="server" ID="lblMessageApprovals" SkinID="MessageLabelSkin"></asp:Label>
                <table runat="server" id="tblApprovals">
                    <tr>
                        <td class="p_textbold">
                            Role
                        </td>
                        <td class="c_textbold">
                            Team Member
                        </td>
                        <td class="c_textbold">
                            Status
                        </td>
                        <td class="c_textbold">
                            Comments
                        </td>
                        <td class="c_textbold">
                            Actions
                        </td>
                        <td class="c_textbold">
                            Last Notified
                        </td>
                        <td class="c_textbold">
                            Last Updated
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <hr />
                        </td>
                    </tr>
                    <tr valign="top">
                        <td class="p_text" valign="top">
                            <asp:Label ID="lblRnDTeamMemberMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="*" />
                            <asp:Label runat="server" ID="lblRnDRole" Text="R & D Lab:"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddRnDTeamMember" runat="server" Enabled="false" AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvRnDTeamMember" runat="server" ControlToValidate="ddRnDTeamMember"
                                ErrorMessage="R & D Lab team member is required." Font-Bold="True" ValidationGroup="vgSaveApprovers"
                                Text="<" SetFocusOnError="true" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddRnDStatus" runat="server" Enabled="false">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtRndComments" MaxLength="100" Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btnRnDSave" runat="server" Text="Save" Width="70px" Visible="false" />
                            <asp:Button ID="btnRnDNotify" runat="server" Text="Notify" Width="70px" Visible="false" />
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblRnDLastNotified"></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblRnDLastUpdated"></asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td class="p_text">
                            <asp:Label ID="lblHRSafetyTeamMemberMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="*" />
                            <asp:Label runat="server" ID="lblHRSafetyRole" Text="HR Safety:"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddHRSafetyTeamMember" runat="server" Enabled="false" AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvHRSafetyTeamMember" runat="server" ControlToValidate="ddHRSafetyTeamMember"
                                ErrorMessage="HR Safety team member is required." Font-Bold="True" ValidationGroup="vgSaveApprovers"
                                Text="<" SetFocusOnError="true" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddHRSafetyStatus" runat="server" Enabled="false">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtHRSafetyComments" MaxLength="100" Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btnHRSafetySave" runat="server" Text="Save" Width="70px" Visible="false" />
                            <asp:Button ID="btnHRSafetyNotify" runat="server" Text="Notify" Width="70px" Visible="false" />
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblHRSafetyLastNotified"></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblHRSafetyLastUpdated"></asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td class="p_text">
                            <asp:Label ID="lblCorpEnvTeamMemberMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="*" />
                            <asp:Label runat="server" ID="lblCorpEnvRole" Text="Corporate Environmental:"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddCorpEnvTeamMember" runat="server" Enabled="false" AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvCorpEnvTeamMember" runat="server" ControlToValidate="ddCorpEnvTeamMember"
                                ErrorMessage="Corporate Environment manager team member is required." Font-Bold="True"
                                ValidationGroup="vgSaveApprovers" Text="<" SetFocusOnError="true" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddCorpEnvStatus" runat="server" Enabled="false">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCorpEnvComments" MaxLength="100" Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btnCorpEnvSave" runat="server" Text="Save" Width="70px" Visible="false" />
                            <asp:Button ID="btnCorpEnvNotify" runat="server" Text="Notify" Width="70px" Visible="false" />
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblCorpEnvLastNotified"></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblCorpEnvLastUpdated"></asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td class="p_text">
                            <asp:Label ID="lblPlantEnvMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="*" />
                            <asp:Label runat="server" ID="lblPlantEnvRole" Text="Plant Environmental:"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddPlantEnvTeamMember" runat="server" Enabled="false" AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvPlantEnvTeamMember" runat="server" ControlToValidate="ddPlantEnvTeamMember"
                                ErrorMessage="Plant Environment manager team member is required." Font-Bold="True"
                                ValidationGroup="vgSaveApprovers" Text="<" SetFocusOnError="true" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddPlantEnvStatus" runat="server" Enabled="false">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtPlantEnvComments" MaxLength="100" Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btnPlantEnvSave" runat="server" Text="Save" Width="70px" Visible="false" />
                            <asp:Button ID="btnPlantEnvNotify" runat="server" Text="Notify" Width="70px" Visible="false" />
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblPlantEnvLastNotified"></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblPlantEnvLastUpdated"></asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td class="p_text">
                            <asp:Label ID="lblPurchasingTeamMemberMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="*" />
                            <asp:Label runat="server" ID="lblPurchasingRole" Text="Purchasing:"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddPurchasingTeamMember" runat="server" Enabled="false" AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvPurchasingTeamMember" runat="server" ControlToValidate="ddPurchasingTeamMember"
                                ErrorMessage="Purchasing team member is required." Font-Bold="True" ValidationGroup="vgSaveApprovers"
                                Text="<" SetFocusOnError="true" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddPurchasingStatus" runat="server" Enabled="false">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtPurchasingComments" MaxLength="100" Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btnPurchasingSave" runat="server" Text="Save" Width="70px" Visible="false" />
                            <asp:Button ID="btnPurchasingNotify" runat="server" Text="Notify" Width="70px" Visible="false" />
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblPurchasingLastNotified"></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblPurchasingLastUpdated"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table width="68%">
                    <tr>
                        <td align="center">
                            <asp:Button runat="server" ID="btnSaveApprovers" Text="Update Approver List" CausesValidation="true"
                                ValidationGroup="vgSaveApprovers" />
                            &nbsp;
                            <asp:Button ID="btnNotify" runat="server" Text="Notify" Visible="false" CausesValidation="true"
                                ValidationGroup="vgSaveApprovers" />
                        </td>
                    </tr>
                </table>
            </asp:View>
        </asp:MultiView>
        <br />
        <asp:Label ID="lblMessageBottom" SkinID="MessageLabelSkin" runat="server"></asp:Label>
        <br />
        <asp:Label runat="server" ID="lblMessageSupportingDocs" SkinID="MessageLabelSkin"></asp:Label>
        <table width="98%">
            <tr>
                <td class="p_text" style="white-space: nowrap;" align="right">
                    <asp:Label runat="server" ID="lblFileUploadLabel" Text="Upload a supporting PDF File:"
                        Visible="false"></asp:Label></td>
                <td>
                    <asp:FileUpload runat="server" ID="fileUploadSupportingDoc" Width="334px" Visible="False" />
                    <asp:Button ID="btnSaveUploadSupportingDocument" runat="server" Text="Upload" Visible="False"
                        Width="67px" CausesValidation="false"></asp:Button>
                </td>
            </tr>
        </table>
        <br />
        <asp:Label runat="server" ID="lblMaxNote" Text="(A maximum of three supporting documents are allowed.)"
            Visible="false"></asp:Label>
        <br />
        <asp:GridView ID="gvSupportingDoc" runat="server" AutoGenerateColumns="False" AllowSorting="True"
            AllowPaging="True" PageSize="15" DataKeyNames="RowID" DataSourceID="odsSupportingDoc"
            EmptyDataText="No supporting documents exist yet." Width="68%">
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
                        <asp:HyperLink ID="lnkViewSupportingDoc" runat="server" NavigateUrl='<%# Eval("RowID", "~/Safety/Chemical_Review_Form_Supporting_Doc_View.aspx?RowID={0}") %>'
                            Target="_blank" Text='<%# Eval("SupportingDocName") %>'>
                        </asp:HyperLink>
                    </ItemTemplate>
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
            DeleteMethod="DeleteChemicalReviewFormSupportingDoc" SelectMethod="GetChemicalReviewFormSupportingDoc"
            TypeName="ChemicalReviewFormSupportingDocBLL">
            <DeleteParameters>
                <asp:Parameter Name="RowID" Type="Int32" />
                <asp:Parameter Name="original_RowID" Type="Int32" />
            </DeleteParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="lblChemicalReviewFormIDValue" DefaultValue="0" Name="ChemRevFormID"
                    PropertyName="Text" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
