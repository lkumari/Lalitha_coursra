<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Manufacturing_Metric_Detail.aspx.vb" MaintainScrollPositionOnPostback="true"
    Inherits="PlantSpecificReports_Manufacturing_Metric_Detail" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSave">
        <asp:Label ID="lblMessage" SkinID="MessageLabelSkin" runat="server"></asp:Label>
        <asp:ValidationSummary ID="vsSave" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgSave" />
        <table>
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap">
                    Status:
                </td>
                <td>
                    <asp:DropDownList ID="ddStatus" runat="server" Enabled="false">
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="p_text" align="right" style="white-space: nowrap">
                    <asp:Label ID="lblUGNFacilityMarker" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="*" />
                    <asp:Label ID="lblUGNFacilityLabel" runat="server" Text="UGN Facility:" ForeColor="Blue" />
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
                    <asp:Label ID="lblMonthMarker" runat="server" Font-Bold="True" ForeColor="Red" Text="*" />
                    <asp:Label ID="lblMonthLabel" runat="server" Text="Month:" ForeColor="Blue" />
                </td>
                <td>
                    <asp:DropDownList ID="ddMonth" runat="server" AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvMonth" runat="server" ControlToValidate="ddMonth"
                        ErrorMessage="Month is required." Font-Bold="True" ValidationGroup="vgSave" Text="<"
                        SetFocusOnError="true" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblYearMarker" runat="server" Font-Bold="True" ForeColor="Red" Text="*" />
                    <asp:Label ID="lblYearLabel" runat="server" Text="Year:" ForeColor="Blue" />
                </td>
                <td>
                    <asp:DropDownList ID="ddYear" runat="server" AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvYear" runat="server" ControlToValidate="ddYear"
                        ErrorMessage="Year is required." Font-Bold="True" ValidationGroup="vgSave" Text="<"
                        SetFocusOnError="true" />
                </td>
            </tr>           
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Department:
                </td>
                <td colspan="3">
                    <asp:DropDownList runat="server" ID="ddDepartment" AutoPostBack="true">
                    </asp:DropDownList>                  
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblIncludeDepartmentLabel" runat="server" Text=" Include Selected Department:"
                        ForeColor="Blue" />
                </td>
                <td colspan="3">
                    <asp:CheckBox runat="server" ID="cbIncludeDepartment" />
                    <i>(Only checked Department information will be saved.)</i>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Last Updated By:
                </td>
                <td>
                    <asp:DropDownList ID="ddCreatedByTMID" runat="server" Enabled="false">
                    </asp:DropDownList>
                </td>
                <td class="p_text">
                    Last Updated:
                </td>
                <td>
                    <asp:Label runat="server" ID="lblUpdatedOn" CssClass="p_textbold"></asp:Label>
                </td>
            </tr>            
        </table>
        <asp:ValidationSummary ID="vsVoid" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgVoid" />
        <table width="80%">
            <tr>
                <td align="center">
                    <asp:Button runat="server" ID="btnCalculate" Text="Calculate" CausesValidation="true"
                        ValidationGroup="vgSave" Visible="false" />
                    <asp:Button runat="server" ID="btnSave" Text="Save" CausesValidation="true" ValidationGroup="vgSave" />
                    <asp:Button runat="server" ID="btnPreview" Text="Preview" CausesValidation="true"
                        ValidationGroup="vgSave" />                   
                    <br />
                    <asp:Button runat="server" ID="btnNofityInternal" Text="Notify Internal Reviewers"
                        CausesValidation="true" ValidationGroup="vgSave" />
                    <asp:Button runat="server" ID="btnNotifyFinal" Text="Notify Final Reviewers" CausesValidation="true"
                        ValidationGroup="vgSave" />
                </td>
            </tr>
        </table>
        <h2>
            Production Performance
            <asp:Label runat="server" ID="lblProductionPerformance" ForeColor="Blue"></asp:Label></h2>
        <table>
            <tr valign="top">
                <td colspan="2">
                    <table>
                        <tr valign="top">
                            <td>
                                <asp:Label runat="server" ID="lblNotes" Text="Department Notes:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNotes" runat="server" Height="60px" TextMode="MultiLine" Width="700px"></asp:TextBox>
                                <br />
                                <asp:Label ID="lblNotesCharCount" SkinID="MessageLabelSkin" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <table>
                        <tr valign="top">
                            <td class="p_textbold">
                                Metric&nbsp&nbsp&nbsp&nbsp
                            </td>
                            <td class="c_textbold">
                                <asp:Label ID="lblBudgetMetric" runat="server" Text="Budget" ForeColor="Blue" />
                            </td>
                            <td class="c_textbold">
                                Actual
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="lblOEELabel" runat="server" Text="OEE (Based on Available Hours):" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtBudgetOEE" Width="75px" MaxLength="10"></asp:TextBox>%
                                <asp:CompareValidator runat="server" ID="cvBudgetOEE" Operator="DataTypeCheck" ValidationGroup="vgSave"
                                    Type="double" Text="<" ControlToValidate="txtBudgetOEE" ErrorMessage="Budget OEE must be numeric"
                                    SetFocusOnError="True" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtActualOEE" Width="75px" MaxLength="10"></asp:TextBox>%
                                <asp:CompareValidator runat="server" ID="cvActualOEE" Operator="DataTypeCheck" ValidationGroup="vgSave"
                                    Type="double" Text="<" ControlToValidate="txtActualOEE" ErrorMessage="Actual OEE must be numeric"
                                    SetFocusOnError="True" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="lblEarnedDLHoursLabel" runat="server" Text="Earned Direct Labor Hours:" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtBudgetEarnedDLHours" Width="75px" MaxLength="10"></asp:TextBox>
                                <asp:CompareValidator runat="server" ID="cvBudgetEarnedDLHours" Operator="DataTypeCheck"
                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtBudgetEarnedDLHours"
                                    ErrorMessage="Budget Earned DL Hours must be numeric" SetFocusOnError="True" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtActualEarnedDLHours" Width="75px" MaxLength="10"></asp:TextBox>
                                <asp:CompareValidator runat="server" ID="cvActualEarnedDLHours" Operator="DataTypeCheck"
                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtActualEarnedDLHours"
                                    ErrorMessage="Actual Earned DL Hours must be numeric" SetFocusOnError="True" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="lblActualDLHoursLabel" runat="server" Text="Actual Direct Labor Hours:" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtBudgetDLHours" Width="75px" MaxLength="10"></asp:TextBox>
                                <asp:CompareValidator runat="server" ID="cvBudgetDLHours" Operator="DataTypeCheck"
                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtBudgetDLHours"
                                    ErrorMessage="Budget DL Hours must be numeric" SetFocusOnError="True" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtActualDLHours" Width="75px" MaxLength="10"></asp:TextBox>
                                <asp:CompareValidator runat="server" ID="cvActualDLHours" Operator="DataTypeCheck"
                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtActualDLHours"
                                    ErrorMessage="Actual DL Hours must be numeric" SetFocusOnError="True" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                Net Variance:
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblBudgetDLHoursNetVariance"></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblActualDLHoursNetVariance"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                Labor Productivty:
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblBudgetLaborProductivity"></asp:Label>%
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblActualLaborProductivity"></asp:Label>%
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                Machine Utilization:
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblBudgetMachineUtilization"></asp:Label>%
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblActualMachineUtilization"></asp:Label>%
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="lblDirectOTHoursLabel" runat="server" Text="Overtime Hours - Direct:"
                                    ForeColor="Blue" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtBudgetDirectOTHours" Width="75px" MaxLength="10"></asp:TextBox>
                                <asp:CompareValidator runat="server" ID="cvBudgetDirectOTHours" Operator="DataTypeCheck"
                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtBudgetDirectOTHours"
                                    ErrorMessage="Budget Direct OT Hours must be numeric" SetFocusOnError="True" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtActualDirectOTHours" Width="75px" MaxLength="10"></asp:TextBox>
                                <asp:CompareValidator runat="server" ID="cvActualDirectOTHours" Operator="DataTypeCheck"
                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtActualDirectOTHours"
                                    ErrorMessage="Actual Direct OT Hours must be numeric" SetFocusOnError="True" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="lblIndirectOTHoursLabel" runat="server" Text="Overtime Hours - Indirect:"
                                    ForeColor="Blue" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtBudgetIndirectOTHours" Width="75px" MaxLength="10"></asp:TextBox>
                                <asp:CompareValidator runat="server" ID="cvBudgetIndirectOTHours" Operator="DataTypeCheck"
                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtBudgetIndirectOTHours"
                                    ErrorMessage="Budget Indirect OT Hours must be numeric" SetFocusOnError="True" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtActualIndirectOTHours" Width="75px" MaxLength="10"></asp:TextBox>
                                <asp:CompareValidator runat="server" ID="cvActualIndirectOTHours" Operator="DataTypeCheck"
                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtActualIndirectOTHours"
                                    ErrorMessage="Actual Indirect OT Hours must be numeric" SetFocusOnError="True" />
                            </td>
                        </tr>
                        <tr runat="server" id="trAllocatedSupportOTHours">
                            <td class="p_text">
                                <asp:Label ID="lblAllocatedSupportOTHoursLabel" runat="server" Text="Overtime Hours - Allocated Support:"
                                    ForeColor="Blue" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtBudgetAllocatedSupportOTHours" Width="75px" MaxLength="10"></asp:TextBox>
                                <asp:CompareValidator runat="server" ID="cvBudgetAllocatedSupportOTHours" Operator="DataTypeCheck"
                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtBudgetAllocatedSupportOTHours"
                                    ErrorMessage="Budget Allocated Support OT Hours must be numeric" SetFocusOnError="True" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtActualAllocatedSupportOTHours" Width="75px" MaxLength="10"></asp:TextBox>
                                <asp:CompareValidator runat="server" ID="cvActualAllocatedSupportOTHours" Operator="DataTypeCheck"
                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtActualAllocatedSupportOTHours"
                                    ErrorMessage="Actual Allocated Support OT Hours must be numeric" SetFocusOnError="True" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="lblScrapLabel" runat="server" Text="F.G. Scrap as a percentage of Cost of Production:" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtBudgetScrapPercent" Width="75px" MaxLength="10"></asp:TextBox>%
                                <asp:CompareValidator runat="server" ID="cvBudgetScrapPercent" Operator="DataTypeCheck"
                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtBudgetScrapPercent"
                                    ErrorMessage="Budget Scrap must be numeric" SetFocusOnError="True" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtActualScrapPercent" Width="75px" MaxLength="10"></asp:TextBox>%
                                <asp:CompareValidator runat="server" ID="cvActualScrapPercent" Operator="DataTypeCheck"
                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtActualScrapPercent"
                                    ErrorMessage="Actual Scrap must be numeric" SetFocusOnError="True" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="lblInProcessScrapLabel" runat="server" Text="In-Process Scrap as a percentage of Cost of Production:" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtBudgetRawWipScrapPercent" Width="75px" MaxLength="10" Enabled="false"></asp:TextBox>%                              
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtActualRawWipScrapPercent" Width="75px" MaxLength="10" Enabled="false"></asp:TextBox>%                               
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="lblTeamMemberContainmentCountLabel" runat="server" Text="Team Members Used for Containment:"
                                    ForeColor="Blue" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtBudgetTeamMemberContainmentCount" Width="75px"
                                    MaxLength="10"></asp:TextBox>
                                <asp:CompareValidator runat="server" ID="cvBudgetTeamMemberContainmentCount" Operator="DataTypeCheck"
                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtBudgetTeamMemberContainmentCount"
                                    ErrorMessage="Budget Team Member containment must be numeric" SetFocusOnError="True" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtActualTeamMemberContainmentCount" Width="75px"
                                    MaxLength="10"></asp:TextBox>
                                <asp:CompareValidator runat="server" ID="cvActualTeamMemberContainmentCount" Operator="DataTypeCheck"
                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtActualTeamMemberContainmentCount"
                                    ErrorMessage="Actual Team Member containment must be numeric" SetFocusOnError="True" />
                            </td>
                        </tr>
                        <tr runat="server" id="trAllocatedSupportTeamMemberContainmentCount">
                            <td class="p_text">
                                <asp:Label ID="lblAllocatedSupportTeamMemberContainmentCountLabel" runat="server"
                                    Text="Team Members Used for Allocated Support Containment:" ForeColor="Blue" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtBudgetAllocatedSupportTeamMemberContainmentCount"
                                    Width="75px" MaxLength="10"></asp:TextBox>
                                <asp:CompareValidator runat="server" ID="cvBudgetAllocatedSupportTeamMemberContainmentCount"
                                    Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtBudgetAllocatedSupportTeamMemberContainmentCount"
                                    ErrorMessage="Budget Allocated Support Team Member containment must be numeric"
                                    SetFocusOnError="True" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtActualAllocatedSupportTeamMemberContainmentCount"
                                    Width="75px" MaxLength="10"></asp:TextBox>
                                <asp:CompareValidator runat="server" ID="cvActualAllocatedSupportTeamMemberContainmentCount"
                                    Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtActualAllocatedSupportTeamMemberContainmentCount"
                                    ErrorMessage="Actual Allocated Support Team Member containment must be numeric"
                                    SetFocusOnError="True" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="lblPartContainmentCountLabel" runat="server" Text="Number of Parts in Containment:"
                                    ForeColor="Blue" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtBudgetPartContainmentCount" Width="75px" MaxLength="10"></asp:TextBox>
                                <asp:CompareValidator runat="server" ID="cvBudgetPartContainmentCount" Operator="DataTypeCheck"
                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtBudgetPartContainmentCount"
                                    ErrorMessage="Budget Number of parts in containment must be numeric" SetFocusOnError="True" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtActualPartContainmentCount" Width="75px" MaxLength="10"></asp:TextBox>
                                <asp:CompareValidator runat="server" ID="cvActualPartContainmentCount" Operator="DataTypeCheck"
                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtActualPartContainmentCount"
                                    ErrorMessage="Actual Number of parts in containment must be numeric" SetFocusOnError="True" />
                            </td>
                        </tr>
                        <tr runat="server" id="trAllocatedSupportPartContainmentCount">
                            <td class="p_text">
                                <asp:Label ID="lblAllocatedSupportPartContainmentCountLabel" runat="server" Text="Number of Parts in Allocated Support Containment:"
                                    ForeColor="Blue" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtBudgetAllocatedSupportPartContainmentCount" Width="75px"
                                    MaxLength="10"></asp:TextBox>
                                <asp:CompareValidator runat="server" ID="cvBudgetAllocatedSupportPartContainmentCount"
                                    Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtBudgetAllocatedSupportPartContainmentCount"
                                    ErrorMessage="Budget Allocated Support Number of parts in  containment must be numeric"
                                    SetFocusOnError="True" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtActualAllocatedSupportPartContainmentCount" Width="75px"
                                    MaxLength="10"></asp:TextBox>
                                <asp:CompareValidator runat="server" ID="cvActualAllocatedSupportPartContainmentCount"
                                    Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtActualAllocatedSupportPartContainmentCount"
                                    ErrorMessage="Actual Allocated Support Number of parts in containment must be numeric"
                                    SetFocusOnError="True" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="lblOffStandardDirectCountLabel" runat="server" Text="Number of Off-Standard Team Members - Direct:"
                                    ForeColor="Blue" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtBudgetOffStandardDirectCount" Width="75px" MaxLength="10"></asp:TextBox>
                                <asp:CompareValidator runat="server" ID="cvBudgetOffStandardDirectCount" Operator="DataTypeCheck"
                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtBudgetOffStandardDirectCount"
                                    ErrorMessage="Budget Number of direct off-standard team member must be numeric"
                                    SetFocusOnError="True" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtActualOffStandardDirectCount" Width="75px" MaxLength="10"></asp:TextBox>
                                <asp:CompareValidator runat="server" ID="cvActualOffStandardDirectCount" Operator="DataTypeCheck"
                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtActualOffStandardDirectCount"
                                    ErrorMessage="Actual Number of direct off-standard team member must be numeric"
                                    SetFocusOnError="True" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="lblOffStandardIndirectCountLabel" runat="server" Text="Number of Off-Standard Team Members - Indirect:"
                                    ForeColor="Blue" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtBudgetOffStandardIndirectCount" Width="75px" MaxLength="10"></asp:TextBox>
                                <asp:CompareValidator runat="server" ID="cvBudgetOffStandardIndirectCount" Operator="DataTypeCheck"
                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtBudgetOffStandardIndirectCount"
                                    ErrorMessage="Budget Number of indirect off-standard team member must be numeric"
                                    SetFocusOnError="True" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtActualOffStandardIndirectCount" Width="75px" MaxLength="10"></asp:TextBox>
                                <asp:CompareValidator runat="server" ID="cvActualOffStandardIndirectCount" Operator="DataTypeCheck"
                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtActualOffStandardIndirectCount"
                                    ErrorMessage="Actual Number of indirect off-standard team member must be numeric"
                                    SetFocusOnError="True" />
                            </td>
                        </tr>
                        <tr runat="server" id="trAllocatedSupportOffStandardIndirectCount">
                            <td class="p_text">
                                <asp:Label ID="lblAllocatedSupportOffStandardIndirectCountLabel" runat="server" Text="Number of Off-Standard Team Members - Allocated Support:"
                                    ForeColor="Blue" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtBudgetAllocatedSupportOffStandardIndirectCount"
                                    Width="75px" MaxLength="10"></asp:TextBox>
                                <asp:CompareValidator runat="server" ID="cvBudgetAllocatedSupportOffStandardIndirectCount"
                                    Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtBudgetAllocatedSupportOffStandardIndirectCount"
                                    ErrorMessage="Budget Allocated Support Number of off-standard indirect team member must be numeric"
                                    SetFocusOnError="True" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtActualAllocatedSupportOffStandardIndirectCount"
                                    Width="75px" MaxLength="10"></asp:TextBox>
                                <asp:CompareValidator runat="server" ID="cvActualAllocatedSupportOffStandardIndirectCount"
                                    Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtActualAllocatedSupportOffStandardIndirectCount"
                                    ErrorMessage="Actual Allocated Support Number of off-standard indirect team member must be numeric"
                                    SetFocusOnError="True" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="lblStandardizedCellWorkLabel" runat="server" Text="Check if there is Standardized Work in All Cells:"
                                    ForeColor="Blue" />
                            </td>
                            <td>
                                <asp:CheckBox runat="server" ID="cbBudgetStandardizedCellWork" />
                            </td>
                            <td>
                                <asp:CheckBox runat="server" ID="cbActualStandardizedCellWork" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="lblTeamMemberLeaderRatioLabel" runat="server" Text="Team Member to Team Leader:"
                                    ForeColor="Blue" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtBudgetTeamMemberFactorCount" Width="20px" MaxLength="10"></asp:TextBox>
                                <asp:CompareValidator runat="server" ID="cvBudgetTeamMemberFactorCount" Operator="DataTypeCheck"
                                    ValidationGroup="vgSave" Type="integer" Text="<" ControlToValidate="txtBudgetTeamMemberFactorCount"
                                    ErrorMessage="Budget team member factor must be an integer" SetFocusOnError="True" />
                                &nbsp;
                                <asp:TextBox runat="server" ID="txtBudgetTeamLeaderFactorCount" Width="20px" MaxLength="10"></asp:TextBox>
                                <asp:CompareValidator runat="server" ID="cvBudgetTeamLeaderFactorCount" Operator="DataTypeCheck"
                                    ValidationGroup="vgSave" Type="integer" Text="<" ControlToValidate="txtBudgetTeamLeaderFactorCount"
                                    ErrorMessage="Budget team leader factor must be an integer" SetFocusOnError="True" />
                                <br />
                                <asp:Label runat="server" ID="lblBudgetTeamMemberLeaderRatio"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtActualTeamMemberFactorCount" Width="20px" MaxLength="10"></asp:TextBox>
                                <asp:CompareValidator runat="server" ID="cvActualTeamMemberFactorCount" Operator="DataTypeCheck"
                                    ValidationGroup="vgSave" Type="integer" Text="<" ControlToValidate="txtActualTeamMemberFactorCount"
                                    ErrorMessage="Actual team member factor must be an integer" SetFocusOnError="True" />
                                &nbsp;
                                <asp:TextBox runat="server" ID="txtActualTeamLeaderFactorCount" Width="20px" MaxLength="5"></asp:TextBox>
                                <asp:CompareValidator runat="server" ID="cvActualTeamLeaderFactorCount" Operator="DataTypeCheck"
                                    ValidationGroup="vgSave" Type="integer" Text="<" ControlToValidate="txtActualTeamLeaderFactorCount"
                                    ErrorMessage="Actual team leader factor must be an integer" SetFocusOnError="True" />
                                <br />
                                <asp:Label runat="server" ID="lblActualTeamMemberLeaderRatio"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="lblCapacityUtilizationLabel" runat="server" Text="Capacity Utilization (Based on 24/7/365):"
                                    ForeColor="Blue" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtBudgetCapacityUtilization" Width="75px" MaxLength="10"></asp:TextBox>%
                                <asp:CompareValidator runat="server" ID="cvBudgetCapacityUtilization" Operator="DataTypeCheck"
                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtBudgetCapacityUtilization"
                                    ErrorMessage="Budget Capacity Utilization must be numeric" SetFocusOnError="True" />
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtActualCapacityUtilization" Width="75px" MaxLength="10"></asp:TextBox>%
                                <asp:CompareValidator runat="server" ID="cvActualCapacityUtilization" Operator="DataTypeCheck"
                                    ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtActualCapacityUtilization"
                                    ErrorMessage="Actual Capacity Utilization must be numeric" SetFocusOnError="True" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top" align="center">
                    <asp:Button runat="server" ID="btnViewCalculationSources" Text="View Calculation Sources"
                        Visible="false" />
                    <table runat="server" id="tblSupportingCalcs" border="1">
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td  align="center">
                                            <asp:Label ID="lblOEEActualGoodPartCountLabel" runat="server" Text="Good Part Count"  CssClass="c_textbold"/>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblOEEActualScrapPartCountLabel" runat="server" Text="Scrap Part Count" CssClass="c_textbold"/>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblOEEActualTotalPartCountLabel" runat="server" Text="Total Part Count" CssClass="c_textbold"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="c_text">
                                            <asp:Label ID="lblOEEBudgetPartCount" runat="server" Text="Budget" />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtOEEBudgetGoodPartCount" Width="75px" MaxLength="10"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvOEEBudgetGoodPartCount" Operator="DataTypeCheck"
                                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtOEEBudgetGoodPartCount"
                                                ErrorMessage="OEE Budget Good Part count must be numeric" SetFocusOnError="True" />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtOEEBudgetScrapPartCount" Width="75px" MaxLength="10"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvOEEBudgetScrapPartCount" Operator="DataTypeCheck"
                                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtOEEBudgetScrapPartCount"
                                                ErrorMessage="OEE Budget Scrap Part count must be numeric" SetFocusOnError="True" />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtOEEBudgetTotalPartCount" Width="75px" MaxLength="10"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvOEEBudgetTotalPartCount" Operator="DataTypeCheck"
                                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtOEEBudgetTotalPartCount"
                                                ErrorMessage="OEE Budget Total Part count must be numeric" SetFocusOnError="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="c_text">
                                            Actual
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtOEEActualGoodPartCount" Width="75px" MaxLength="10"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvOEEActualGoodPartCount" Operator="DataTypeCheck"
                                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtOEEActualGoodPartCount"
                                                ErrorMessage="OEE Actual Good Part count must be numeric" SetFocusOnError="True" />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtOEEActualScrapPartCount" Width="75px" MaxLength="10"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvOEEActualScrapPartCount" Operator="DataTypeCheck"
                                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtOEEActualScrapPartCount"
                                                ErrorMessage="OEE Actual Scrap Part count must be numeric" SetFocusOnError="True" />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtOEEActualTotalPartCount" Width="75px" MaxLength="10"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvOEEActualTotalPartCount" Operator="DataTypeCheck"
                                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtOEEActualTotalPartCount"
                                                ErrorMessage="OEE Actual Total Part count must be numeric" SetFocusOnError="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblOEEUtilizationLabel" runat="server" Text="Utilization" CssClass="c_textbold" />
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblOEEAvailableHoursLabel" runat="server" Text="Available Time" CssClass="c_textbold"/>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblOEEDowntimeLabel" runat="server" Text="Down Time" CssClass="c_textbold"/>
                                            <i>(Unscheduled)</i>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="c_text">
                                            <asp:Label ID="lblOEEBudgetUsage" runat="server" Text="Budget" />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtOEEBudgetUtilization" Width="75px" MaxLength="10"></asp:TextBox>%
                                            <asp:CompareValidator runat="server" ID="cvOEEBudgetUtilization" Operator="DataTypeCheck"
                                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtOEEBudgetUtilization"
                                                ErrorMessage="OEE Budget Utilization must be numeric" SetFocusOnError="True" />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtOEEBudgetAvailableHours" Width="75px" MaxLength="10"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvOEEBudgetAvailableHours" Operator="DataTypeCheck"
                                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtOEEBudgetAvailableHours"
                                                ErrorMessage="OEE Budget Available Hours must be numeric" SetFocusOnError="True" />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtOEEBudgetDownHours" Width="75px" MaxLength="10"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvOEEBudgetDownHours" Operator="DataTypeCheck"
                                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtOEEBudgetDownHours"
                                                ErrorMessage="OEE Budget Down Hours must be numeric" SetFocusOnError="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="c_text">
                                            Actual
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtOEEActualUtilization" Width="75px" MaxLength="10"></asp:TextBox>%
                                            <asp:CompareValidator runat="server" ID="cvOEEActualUtilization" Operator="DataTypeCheck"
                                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtOEEActualUtilization"
                                                ErrorMessage="OEE Actual Utilization must be numeric" SetFocusOnError="True" />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtOEEActualAvailableHours" Width="75px" MaxLength="10"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvOEEActualAvailableHours" Operator="DataTypeCheck"
                                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtOEEActualAvailableHours"
                                                ErrorMessage="OEE Actual Available Hours must be numeric" SetFocusOnError="True" />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtOEEActualDownHours" Width="75px" MaxLength="10"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvOEEActualDownHours" Operator="DataTypeCheck"
                                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtOEEActualDownHours"
                                                ErrorMessage="OEE Actual Down Hours must be numeric" SetFocusOnError="True" />
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trShift1Labels" visible="false">
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblMonthlyShippingDaysLabel" runat="server" Text="Monthly Shipping Days" CssClass="c_textbold"/>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblHoursPerShiftLabel" runat="server" Text="Hours Per Shift" CssClass="c_textbold"/>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblAvailablePerShiftFactorColTitle" runat="server" Text="Available Per Shift Factor" CssClass="c_textbold"/>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trShift1Values" visible="false">
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtMonthlyShippingDays" Width="75px" MaxLength="10"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvMonthlyShippingDays" Operator="DataTypeCheck"
                                                ValidationGroup="vgSave" Type="integer" Text="<" ControlToValidate="txtMonthlyShippingDays"
                                                ErrorMessage="Monthly Shipping Days must be an integer" SetFocusOnError="True" />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtHoursPerShift" Width="75px" MaxLength="10"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvHoursPerShift" Operator="DataTypeCheck"
                                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtHoursPerShift"
                                                ErrorMessage="Hours per shift must be numeric" SetFocusOnError="True" />
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblAvailablePerShiftFactor"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trShift2Labels" visible="false">
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td align="center">
                                             <asp:Label ID="lblBudgetShiftCountColTitle" runat="server" Text="Budget Shift Count" CssClass="c_textbold"/>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblActualShiftCountColTitle" runat="server" Text="Actual Shift Count" CssClass="c_textbold"/>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trShift2Values" visible="false">
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblBudgetShiftCount"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblActualShiftCount"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblMachineWorkedHoursLabel" runat="server" Text="Machine Hours Worked" CssClass="c_textbold"/>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblDowntimeHoursLabel" runat="server" Text="Machine Hours Downtime" CssClass="c_textbold"/>
                                            <br /><i>(Scheduled and Unscheduled)</i>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblMachineStandardHoursLabel" runat="server" Text="Machine Hours Earned" CssClass="c_textbold"/>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblMachineAvailableHoursLabel" runat="server" Text="Machine Hours Available" CssClass="c_textbold"
                                                Visible="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="c_text">
                                            <asp:Label ID="lblBudgetMachineHours" runat="server" Text="Budget" />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtBudgetMachineWorkedHours" Width="75px" MaxLength="10"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvBudgetMachineWorkedHours" Operator="DataTypeCheck"
                                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtBudgetMachineWorkedHours"
                                                ErrorMessage="Budget Machine Hours Worked must be numeric" SetFocusOnError="True" />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtBudgetDowntimeHours" Width="75px" MaxLength="10"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvBudgetDowntimeHours" Operator="DataTypeCheck"
                                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtBudgetDowntimeHours"
                                                ErrorMessage="Budget Downtime Hours must be numeric" SetFocusOnError="True" />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtBudgetMachineStandardHours" Width="75px" MaxLength="10"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvBudgetMachineStandardHours" Operator="DataTypeCheck"
                                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtBudgetMachineStandardHours"
                                                ErrorMessage="Budget Earned Machine Hours must be numeric" SetFocusOnError="True" />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtBudgetMachineAvailableHours" Width="75px" MaxLength="10"
                                                Visible="false"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvBudgetMachineAvailableHours" Operator="DataTypeCheck"
                                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtBudgetMachineAvailableHours"
                                                ErrorMessage="Budget Machine Hours Available must be numeric" SetFocusOnError="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="c_text">
                                            Actual
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtActualMachineWorkedHours" Width="75px" MaxLength="10"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvActualMachineWorkedHours" Operator="DataTypeCheck"
                                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtActualMachineWorkedHours"
                                                ErrorMessage="Actual Machine Hours Worked must be numeric" SetFocusOnError="True" />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtActualDowntimeHours" Width="75px" MaxLength="10"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvActualDowntimeHours" Operator="DataTypeCheck"
                                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtActualDowntimeHours"
                                                ErrorMessage="Actual Downtime Hours must be numeric" SetFocusOnError="True" />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtActualMachineStandardHours" Width="75px" MaxLength="10"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvActualMachineStandardHours" Operator="DataTypeCheck"
                                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtActualMachineStandardHours"
                                                ErrorMessage="Actual Earned Machine Hours must be numeric" SetFocusOnError="True" />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtActualMachineAvailableHours" Width="75px" MaxLength="10"
                                                Visible="false"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvActualMachineAvailableHours" Operator="DataTypeCheck"
                                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtActualMachineAvailableHours"
                                                ErrorMessage="Actual Machine Hours Available must be numeric" SetFocusOnError="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trManHourLabels" visible="false">
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblManWorkedHoursLabel" runat="server" Text="Man Hours Worked" CssClass="c_textbold" />
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblDowntimeManHoursLabel" runat="server" Text="Man Hours Downtime" CssClass="c_textbold" /><br />
                                            <i>(Scheduled and Unscheduled)</i>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trManHourBudgetValues" visible="false">
                                        <td class="c_text">
                                            <asp:Label ID="lblBudgetManHours" runat="server" Text="Budget" />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtBudgetManWorkedHours" Width="75px" MaxLength="10"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvBudgetManWorkedHours" Operator="DataTypeCheck"
                                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtBudgetManWorkedHours"
                                                ErrorMessage="Budget Man Hours Worked must be numeric" SetFocusOnError="True" />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtBudgetDowntimeManHours" Width="75px" MaxLength="10"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvBudgetDowntimeManHours" Operator="DataTypeCheck"
                                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtBudgetDowntimeManHours"
                                                ErrorMessage="Budget Downtime Man Worked must be numeric" SetFocusOnError="True" />
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trManHourActualValues" visible="false">
                                        <td class="c_text">
                                            Actual
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtActualManWorkedHours" Width="75px" MaxLength="10"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvActualManWorkedHours" Operator="DataTypeCheck"
                                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtActualManWorkedHours"
                                                ErrorMessage="Actual Man Hours Worked must be numeric" SetFocusOnError="True" />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtActualDowntimeManHours" Width="75px" MaxLength="10"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvActualDowntimeManHours" Operator="DataTypeCheck"
                                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtActualDowntimeManHours"
                                                ErrorMessage="Actual Downtime Man Worked must be numeric" SetFocusOnError="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblTotalScrapDollarLabel" runat="server" Text="(S) Finished Scrap Dollars" CssClass="c_textbold" />
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblMiscScrapDollarLabel" runat="server" Text="(SM) Misc Scrap Dollars" CssClass="c_textbold" />
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblTotalProductionDollarLabel" runat="server" Text="Production Dollars" CssClass="c_textbold" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="c_text">
                                            <asp:Label ID="lblBudgetScrapDollars" runat="server" Text="Budget" />
                                        </td>
                                        <td>
                                            $<asp:TextBox runat="server" ID="txtTotalBudgetSpecificScrapDollar" Width="100px"
                                                MaxLength="20"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvTotalBudgetSpecificScrapDollar" Operator="DataTypeCheck"
                                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtTotalBudgetSpecificScrapDollar"
                                                ErrorMessage="Total Budget Specific Scrap Dollar must be numeric" SetFocusOnError="True" />
                                        </td>
                                        <td>
                                            $<asp:TextBox runat="server" ID="txtTotalBudgetMiscScrapDollar" Width="100px" MaxLength="20"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvTotalBudgetMiscScrapDollar" Operator="DataTypeCheck"
                                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtTotalBudgetMiscScrapDollar"
                                                ErrorMessage="Budget Misc Scrap Dollar must be numeric" SetFocusOnError="True" />
                                        </td>
                                        <td>
                                            $<asp:TextBox runat="server" ID="txtTotalBudgetProductionDollar" Width="100px" MaxLength="20"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvTotalBudgetProductionDollar" Operator="DataTypeCheck"
                                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtTotalBudgetProductionDollar"
                                                ErrorMessage="Budget Production Dollar must be numeric" SetFocusOnError="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="c_text">
                                            Actual
                                        </td>
                                        <td>
                                            $<asp:TextBox runat="server" ID="txtTotalActualSpecificScrapDollar" Width="100px"
                                                MaxLength="20"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvTotalActualSpecificScrapDollar" Operator="DataTypeCheck"
                                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtTotalActualSpecificScrapDollar"
                                                ErrorMessage="Actual Specific Scrap Dollar must be numeric" SetFocusOnError="True" />
                                        </td>
                                        <td>
                                            $<asp:TextBox runat="server" ID="txtTotalActualMiscScrapDollar" Width="100px" MaxLength="20"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvTotalActualMiscScrapDollar" Operator="DataTypeCheck"
                                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtTotalActualMiscScrapDollar"
                                                ErrorMessage="Actual Misc Scrap Dollar must be numeric" SetFocusOnError="True" />
                                        </td>
                                        <td>
                                            $<asp:TextBox runat="server" ID="txtTotalActualProductionDollar" Width="100px" MaxLength="20"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvTotalActualProductionDollar" Operator="DataTypeCheck"
                                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtTotalActualProductionDollar"
                                                ErrorMessage="Actual Production Dollar must be numeric" SetFocusOnError="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblRawWIPDollarLabel" runat="server" Text="(I) In-Process Scrap Dollars" CssClass="c_textbold" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblBudgetRawWIPScrapDollars" runat="server" Text="Budget" />
                                        </td>
                                        <td>
                                            $<asp:TextBox runat="server" ID="txtTotalBudgetRawWipScrapDollar" Width="100px" MaxLength="20"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvTotalBudgetRawWipScrapDollar" Operator="DataTypeCheck"
                                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtTotalBudgetRawWipScrapDollar"
                                                ErrorMessage="Budget Other Scrap Dollar must be numeric" SetFocusOnError="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Actual
                                        </td>
                                        <td>
                                            $<asp:TextBox runat="server" ID="txtTotalActualRawWipScrapDollar" Width="100px" MaxLength="20"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvTotalActualRawWipScrapDollar" Operator="DataTypeCheck"
                                                ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtTotalActualRawWipScrapDollar"
                                                ErrorMessage="Actual Other Scrap Dollar must be numeric" SetFocusOnError="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            <hr />
                                            <asp:Label runat="server" ID="lblSMTransactions" SkinID="MessageLabelSkin" Text="SM Transactions that do not relate to departments directly."></asp:Label>
                                            <asp:Label runat="Server" ID="lblStartDate" Visible="false"></asp:Label>
                                            <asp:Label runat="Server" ID="lblEndDate" Visible="false"></asp:Label>
                                            <asp:GridView runat="server" ID="gvMiscScrapDollarNoDepartment" DataSourceID="odsMiscScrapDollarNoDepartment"
                                                AutoGenerateColumns="False" PageSize="10000" AllowPaging="true" Width="98%" ShowFooter="false"
                                                EmptyDataText="No Misc Scrap Dollar Without Departments Found. All SM Transactions relate to departments">
                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                                <EditRowStyle BackColor="#CCCCCC" />
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                <Columns>
                                                    <asp:BoundField DataField="PartNo" ReadOnly="True" HeaderText="PartNo" SortExpression="PartNo">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TotalQuantity" ReadOnly="True" HeaderText="Total Quantity"
                                                        SortExpression="TotalQuantity">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TotalDollar" ReadOnly="True" HeaderText="Total Dollar"
                                                        SortExpression="TotalDollar">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                </Columns>
                                            </asp:GridView>
                                            <asp:ObjectDataSource ID="odsMiscScrapDollarNoDepartment" runat="server" SelectMethod="GetManufacturingMetricMiscScrapDollarNoDept"
                                                TypeName="PSRModule">
                                                <SelectParameters>
                                                    <asp:ControlParameter Name="UGNFacility" ControlID="ddUGNFacility" PropertyName="SelectedValue"
                                                        Type="String" />
                                                    <asp:ControlParameter Name="StartDate" ControlID="lblStartDate" PropertyName="Text"
                                                        Type="String" />
                                                    <asp:ControlParameter Name="EndDate" ControlID="lblEndDate" PropertyName="Text" Type="String" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                            <br />
                                            <asp:Label ID="lblTotalActualIndirectScrapDollar" runat="server" Text="Additional Indirect Misc Scrap Dollars to use:"
                                                ForeColor="Blue" />
                                            &nbsp; $<asp:TextBox runat="server" ID="txtTotalActualIndirectScrapDollar" MaxLength="10"
                                                Width="100px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table align="center">
            <tr>
                <td>
                    <asp:Button runat="server" ID="btnSaveMiddle" Text="Save" CausesValidation="true"
                        ValidationGroup="vgSave" />
                </td>
            </tr>
        </table>
        <br />
        <h2>
            Team Members
        </h2>
        <table>
            <tr>
                <td class="p_textbold">
                    Metric (Monthly)
                </td>
                <td class="c_textbold">
                    Budget
                </td>
                <td class="c_textbold">
                    Flex Budget
                </td>
                <td class="c_textbold">
                    Actual
                </td>
                <td class="c_textbold">
                    B / (W)<br />
                    Flex Budget
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblDirectPermLabel" runat="server" Text="Direct - Perm:" ForeColor="Blue" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtBudgetDirectPerm" Width="75px" MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvBudgetDirectPerm" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtBudgetDirectPerm"
                        ErrorMessage="Budget Direct Perm must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtFlexDirectPerm" Width="75px" MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvFlexDirectPerm" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtFlexDirectPerm"
                        ErrorMessage="Flex Direct Perm must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtActualDirectPerm" Width="75px" MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvActualDirectPerm" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtActualDirectPerm"
                        ErrorMessage="Actual Direct Perm must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:Label runat="server" ID="lblBWDirectPerm"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblDirectTempLabel" runat="server" Text="Direct - Temp:" ForeColor="Blue" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtBudgetDirectTemp" Width="75px" MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvBudgetDirectTemp" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtBudgetDirectTemp"
                        ErrorMessage="Budget Direct Temp must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtFlexDirectTemp" Width="75px" MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvFlexDirectTemp" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtFlexDirectTemp"
                        ErrorMessage="Flex Direct Temp must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtActualDirectTemp" Width="75px" MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvActualDirectTemp" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtActualDirectTemp"
                        ErrorMessage="Actual Direct Temp must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:Label runat="server" ID="lblBWDirectTemp"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="p_textbold">
                    Total Direct Labor:
                </td>
                <td>
                    <asp:Label runat="server" ID="lblBudgetDirectLaborTotal"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblFlexDirectLaborTotal"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblActualDirectLaborTotal"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblBWDirectLaborTotal"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblIndirectPermLabel" runat="server" Text="Indirect Hourly Production- Perm:"
                        ForeColor="Blue" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtBudgetIndirectPerm" Width="75px" MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvBudgetIndirectPerm" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtBudgetIndirectPerm"
                        ErrorMessage="Budget Indirect Perm must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtFlexIndirectPerm" Width="75px" MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvFlexIndirectPerm" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtFlexIndirectPerm"
                        ErrorMessage="Flex Indirect Perm must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtActualIndirectPerm" Width="75px" MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvActualIndirectPerm" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtActualIndirectPerm"
                        ErrorMessage="Actual Indirect Perm must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:Label runat="server" ID="lblBWIndirectPerm"></asp:Label>
                </td>
            </tr>
            <tr runat="server" id="trAllocatedSupportIndirectPerm">
                <td class="p_text">
                    <asp:Label ID="lblAllocatedSupportIndirectPermLabel" runat="server" Text="Allocated Support Indirect Production- Perm:"
                        ForeColor="Blue" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtBudgetAllocatedSupportIndirectPerm" MaxLength="10"
                        Width="75px"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvBudgetAllocatedSupportIndirectPerm" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtBudgetAllocatedSupportIndirectPerm"
                        ErrorMessage="Budget Allocated Support Indirect Production Perm must be numeric"
                        SetFocusOnError="True" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtFlexAllocatedSupportIndirectPerm" Width="75px"
                        MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvFlexAllocatedSupportIndirectPerm" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtFlexAllocatedSupportIndirectPerm"
                        ErrorMessage="Flex Allocated Support Indirect Production Perm must be numeric"
                        SetFocusOnError="True" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtActualAllocatedSupportIndirectPerm" MaxLength="10"
                        Width="75px"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvActualAllocatedSupportIndirectPerm" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtActualAllocatedSupportIndirectPerm"
                        ErrorMessage="Actual Allocated Support Indirect Production Perm must be numeric"
                        SetFocusOnError="True" />
                </td>
                <td>
                    <asp:Label runat="server" ID="lblBWAllocatedSupportIndirectPerm"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblIndirectTempLabel" runat="server" Text="Indirect Hourly Production - Temp:"
                        ForeColor="Blue" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtBudgetIndirectTemp" Width="75px" MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvBudgetIndirectTemp" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtBudgetIndirectTemp"
                        ErrorMessage="Budget Indirect Temp must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtFlexIndirectTemp" Width="75px" MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvFlexIndirectTemp" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtFlexIndirectTemp"
                        ErrorMessage="Flex Indirect Temp must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtActualIndirectTemp" Width="75px" MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvActualIndirectTemp" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtActualIndirectTemp"
                        ErrorMessage="Actual Indirect Temp must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:Label runat="server" ID="lblBWIndirectTemp"></asp:Label>
                </td>
            </tr>
            <tr runat="server" id="trAllocatedSupportIndirectTemp">
                <td class="p_text">
                    <asp:Label ID="lblAllocatedSupportIndirectTempLabel" runat="server" Text="Allocated Support Indirect Production - Temp:"
                        ForeColor="Blue" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtBudgetAllocatedSupportIndirectTemp" MaxLength="10"
                        Width="75px"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvBudgetAllocatedSupportIndirectTemp" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtBudgetAllocatedSupportIndirectTemp"
                        ErrorMessage="Budget Allocated Support Indirect Productiont Temp must be numeric"
                        SetFocusOnError="True" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtFlexAllocatedSupportIndirectTemp" Width="75px"
                        MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvFlexAllocatedSupportIndirectTemp" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtFlexAllocatedSupportIndirectTemp"
                        ErrorMessage="Flex Allocated Support Indirect Production Temp must be numeric"
                        SetFocusOnError="True" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtActualAllocatedSupportIndirectTemp" MaxLength="10"
                        Width="75px"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvActualAllocatedSupportIndirectTemp" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtActualAllocatedSupportIndirectTemp"
                        ErrorMessage="Actual Allocated Support Indirect Production Temp must be numeric"
                        SetFocusOnError="True" />
                </td>
                <td>
                    <asp:Label runat="server" ID="lblBWAllocatedSupportIndirectTemp"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="p_textbold">
                    Total Indirect Labor:
                </td>
                <td>
                    <asp:Label runat="server" ID="lblBudgetIndirectLaborTotal"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblFlexIndirectLaborTotal"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblActualIndirectLaborTotal"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblBWIndirectLaborTotal"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblOfficeHourlyPermLabel" runat="server" Text="Office Hourly - Perm:"
                        ForeColor="Blue" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtBudgetOfficeHourlyPerm" Width="75px" MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvBudgetOfficeHourlyPerm" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtBudgetOfficeHourlyPerm"
                        ErrorMessage="Budget Office Hourly Perm must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtFlexOfficeHourlyPerm" Width="75px" MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvFlexOfficeHourlyPerm" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtFlexOfficeHourlyPerm"
                        ErrorMessage="Flex Office Hourly Perm must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtActualOfficeHourlyPerm" Width="75px" MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvActualOfficeHourlyPerm" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtActualOfficeHourlyPerm"
                        ErrorMessage="Actual Office Hourly Perm must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:Label runat="server" ID="lblBWOfficeHourlyPerm"></asp:Label>
                </td>
            </tr>
            <tr runat="server" id="trAllocatedSupportOfficeHourlyPerm">
                <td class="p_text">
                    <asp:Label ID="lblAllocatedSupportOfficeHourlyPermLabel" runat="server" Text="Allocated Support Office - Perm:"
                        ForeColor="Blue" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtBudgetAllocatedSupportOfficeHourlyPerm" Width="75px"
                        MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvBudgetAllocatedSupportOfficeHourlyPerm"
                        Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtBudgetAllocatedSupportOfficeHourlyPerm"
                        ErrorMessage="Budget Allocated Support Office Hourly Perm must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtFlexAllocatedSupportOfficeHourlyPerm" Width="75px"
                        MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvFlexAllocatedSupportOfficeHourlyPerm"
                        Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtFlexAllocatedSupportOfficeHourlyPerm"
                        ErrorMessage="Flex Allocated Support Office Hourly Perm must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtActualAllocatedSupportOfficeHourlyPerm" Width="75px"
                        MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvActualAllocatedSupportOfficeHourlyPerm"
                        Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtActualAllocatedSupportOfficeHourlyPerm"
                        ErrorMessage="Actual Allocated Support Office Hourly Perm must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:Label runat="server" ID="lblBWAllocatedSupportOfficeHourlyPerm"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblOfficeHourlyTempLabel" runat="server" Text="Office Hourly - Temp:"
                        ForeColor="Blue" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtBudgetOfficeHourlyTemp" Width="75px" MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvBudgetOfficeHourlyTemp" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtBudgetOfficeHourlyTemp"
                        ErrorMessage="Budget Office Hourly Temp must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtFlexOfficeHourlyTemp" Width="75px" MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvFlexOfficeHourlyTemp" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtFlexOfficeHourlyTemp"
                        ErrorMessage="Flex Office Hourly Temp must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtActualOfficeHourlyTemp" Width="75px" MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvActualOfficeHourlyTemp" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtActualOfficeHourlyTemp"
                        ErrorMessage="Actual Office Hourly Temp must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:Label runat="server" ID="lblBWOfficeHourlyTemp"></asp:Label>
                </td>
            </tr>
            <tr runat="server" id="trAllocatedSupportOfficeHourlyTemp">
                <td class="p_text">
                    <asp:Label ID="lblOfficeAllocatedSupportOfficeHourlyTempLabel" runat="server" Text="Allocated Support Office- Temp:"
                        ForeColor="Blue" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtBudgetAllocatedSupportOfficeHourlyTemp" Width="75px"
                        MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvBudgetAllocatedSupportOfficeHourlyTemp"
                        Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtBudgetAllocatedSupportOfficeHourlyTemp"
                        ErrorMessage="Budget Allocated Support Office Hourly Temp must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtFlexAllocatedSupportOfficeHourlyTemp" Width="75px"
                        MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvFlexAllocatedSupportOfficeHourlyTemp"
                        Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtFlexAllocatedSupportOfficeHourlyTemp"
                        ErrorMessage="Flex Allocated Support Office Hourly Temp must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtActualAllocatedSupportOfficeHourlyTemp" Width="75px"
                        MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvActualAllocatedSupportOfficeHorlyTemp"
                        Operator="DataTypeCheck" ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtActualAllocatedSupportOfficeHourlyTemp"
                        ErrorMessage="Actual Allocated Support Office Hourly Temp must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:Label runat="server" ID="lblBWAllocatedSupportOfficeHourlyTemp"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="p_textbold">
                    Total Office Hourly:
                </td>
                <td>
                    <asp:Label runat="server" ID="lblBudgetOfficeHourlyTotal"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblFlexOfficeHourlyTotal"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblActualOfficeHourlyTotal"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblBWOfficeHourlyTotal"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblSalaryPermLabel" runat="server" Text="Salary - Perm:" ForeColor="Blue" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtBudgetSalaryPerm" Width="75px" MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvBudgetSalaryPerm" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtBudgetSalaryPerm"
                        ErrorMessage="Budget Salary Perm must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtFlexSalaryPerm" Width="75px" MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvFlexSalaryPerm" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtFlexSalaryPerm"
                        ErrorMessage="Flex Salary Perm must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtActualSalaryPerm" Width="75px" MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvActualSalaryPerm" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtActualSalaryPerm"
                        ErrorMessage="Actual Salary Perm must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:Label runat="server" ID="lblBWSalaryPerm"></asp:Label>
                </td>
            </tr>
            <tr runat="server" id="trAllocatedSupportSalaryPerm">
                <td class="p_text">
                    <asp:Label ID="lblAllocatedSupportSalaryPermLabel" runat="server" Text="Allocated Support Salary - Perm:"
                        ForeColor="Blue" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtBudgetAllocatedSupportSalaryPerm" Width="75px"
                        MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvBudgetAllocatedSupportSalaryPerm" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtBudgetAllocatedSupportSalaryPerm"
                        ErrorMessage="Budget Allocated Support Salary Perm must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtFlexAllocatedSupportSalaryPerm" Width="75px" MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="CompareValidator3" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtFlexAllocatedSupportSalaryPerm"
                        ErrorMessage="Flex Allocated Support Salary Perm must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtActualAllocatedSupportSalaryPerm" Width="75px"
                        MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvActualAllocatedSupportSalaryPerm" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtActualAllocatedSupportSalaryPerm"
                        ErrorMessage="Actual Allocated Support Salary Perm must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:Label runat="server" ID="lblBWAllocatedSupportSalaryPerm"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblSalaryTempLabel" runat="server" Text="Salary - Temp:" ForeColor="Blue" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtBudgetSalaryTemp" Width="75px" MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvBudgetSalaryTemp" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtBudgetSalaryTemp"
                        ErrorMessage="Budget Salary Temp must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtFlexSalaryTemp" Width="75px" MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvFlexSalaryTemp" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtFlexSalaryTemp"
                        ErrorMessage="Flex Salary Temp must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtActualSalaryTemp" Width="75px" MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvActualSalaryTemp" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtActualSalaryTemp"
                        ErrorMessage="Actual Salary Temp must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:Label runat="server" ID="lblBWSalaryTemp"></asp:Label>
                </td>
            </tr>
            <tr runat="server" id="trAllocatedSupportSalaryTemp">
                <td class="p_text">
                    <asp:Label ID="lblAllocatedSupportSalaryTempLabel" runat="server" Text="Allocated Support Salary - Temp:"
                        ForeColor="Blue" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtBudgetAllocatedSupportSalaryTemp" Width="75px"
                        MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvBudgetAllocatedSupportSalaryTemp" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtBudgetAllocatedSupportSalaryTemp"
                        ErrorMessage="Budget Allocated Support Salary Temp must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtFlexAllocatedSupportSalaryTemp" Width="75px" MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvFlexAllocatedSupportSalaryTemp" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtFlexAllocatedSupportSalaryTemp"
                        ErrorMessage="Flex Allocated Support Salary Temp must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtActualAllocatedSupportSalaryTemp" Width="75px"
                        MaxLength="10"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="CompareValidator4" Operator="DataTypeCheck"
                        ValidationGroup="vgSave" Type="double" Text="<" ControlToValidate="txtActualAllocatedSupportSalaryTemp"
                        ErrorMessage="Actual Allocated Support Salary Temp must be numeric" SetFocusOnError="True" />
                </td>
                <td>
                    <asp:Label runat="server" ID="lblBWAllocatedSupportSalaryTemp"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="p_textbold">
                    Total Salary:
                </td>
                <td>
                    <asp:Label runat="server" ID="lblBudgetSalaryTotal"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblFlexSalaryTotal"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblActualSalaryTotal"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblBWSalaryTotal"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="p_textbold">
                    Total Team Members:
                </td>
                <td>
                    <asp:Label runat="server" ID="lblBudgetTotalTeamMembers"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblFlexTotalTeamMembers"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblActualTotalTeamMembers"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblBWTotalTeamMembers"></asp:Label>
                </td>
            </tr>
        </table>
        <table width="80%">
            <tr>
                <td align="center">
                    <asp:Button runat="server" ID="btnSaveBottom" Text="Save" CausesValidation="true"
                        ValidationGroup="vgSave" />
                    <asp:Button runat="server" ID="btnPreviewBottom" Text="Preview" CausesValidation="true"
                        ValidationGroup="vgSave" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
