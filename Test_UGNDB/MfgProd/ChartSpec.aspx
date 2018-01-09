<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="ChartSpec.aspx.vb" Inherits="MfgProd_ChartSpec" Title="UGNDB - Chart Spec"
    MaintainScrollPositionOnPostback="True" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="1000px">
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
        <br />
        <asp:Label ID="Label2" runat="server"><i>An asterick (<asp:Label ID="Label11" runat="server"
            Font-Bold="True" ForeColor="Red" Text="* " />) denotes a required field.</i></asp:Label><br />
        <%--<table>
            <tr>
                <td class="p_smalltextbold" style="color: #990000">
                    You are viewing Specs for Part Number:
                </td>
                <td class="p_smalltextbold" style="color: #990000">
                    <asp:Label ID="lblPno" runat="server" Text="" />
                </td>
                <td class="p_smalltextbold" style="color: #990000">
                    Go to:
                </td>
                <td>
                    <asp:DropDownList ID="ddGoToPNo" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
        </table>--%>
        <hr />
        <table>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblFormulaRqrd" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                    &nbsp;<asp:Label ID="lblFormula" runat="server" Text="Formula Name:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddFormula" runat="server" CssClass="c_textxsmall" AutoPostBack="true" />
                    <asp:RequiredFieldValidator ID="rfvFormula" runat="server" ControlToValidate="ddFormula"
                        ErrorMessage="Formula is a required field." Font-Bold="False" ValidationGroup="vsAddEditChartSpec"><</asp:RequiredFieldValidator>
                    <asp:Label ID="lblFormulaID" runat="server" Text="ID:" class="p_textxsmall" />
                    <asp:TextBox ID="txtFormulaID" runat="server" MaxLength="6" Width="30px" ReadOnly="true"
                        Enabled="false" CssClass="c_textxsmall" Text="0" />
                </td>
                <td valign="top" rowspan="11">
                    <asp:ValidationSummary ID="vsAddEditChartSpec" runat="server" ShowMessageBox="True"
                        ValidationGroup="vsAddEditChartSpec" Width="316px" />
                </td>
            </tr>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblUGNFacilityRqrd" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="* " />
                    &nbsp;<asp:Label ID="lblUGNFacility" runat="server" Text="UGN Facility:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddUGNLocation" runat="server" CssClass="c_textxsmall" />
                    <asp:RequiredFieldValidator ID="rfvUGNLocation" runat="server" ControlToValidate="ddUGNLocation"
                        ErrorMessage="UGN Location is a required field." Font-Bold="False" ValidationGroup="vsAddEditChartSpec"><</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblWorkCenterRqrd" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="* " />
                    &nbsp;<asp:Label ID="lblWorkCenter" runat="server" Text="Work Center:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddWorkCenter" runat="server" CssClass="c_textxsmall" />
                    <asp:RequiredFieldValidator ID="rfvWorkCenter" runat="server" ControlToValidate="ddWorkCenter"
                        ErrorMessage="Work Center is a required field." Font-Bold="False" ValidationGroup="vsAddEditChartSpec"><</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblOEMMfgRqrd" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                    &nbsp;<asp:Label ID="lblOEMMfg" runat="server" Text="OEM Manufacturer:" />
                </td>
                <td class="c_textxsmall">
                    <asp:DropDownList ID="ddOEMMfg" runat="server" CssClass="c_textxsmall" />
                    <asp:RequiredFieldValidator ID="rfvOEMMfg" runat="server" ControlToValidate="ddOEMMfg"
                        ErrorMessage="OEM Manufacturer is a required field." Font-Bold="False" ValidationGroup="vsAddEditChartSpec"><</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="p_textxsmall" valign="top">
                    <asp:Label ID="lblCABBVRqrd" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                    &nbsp;<asp:Label ID="lblCABBV" runat="server" Text="Customer:" />
                </td>
                <td class="c_textxsmall">
                    <asp:DropDownList ID="ddCustomer" runat="server" CssClass="c_textxsmall" />
                    <asp:RequiredFieldValidator ID="rfvCustomer" runat="server" ControlToValidate="ddCustomer"
                        ErrorMessage="Customer is a required field." Font-Bold="False" ValidationGroup="vsAddEditChartSpec"><</asp:RequiredFieldValidator>&nbsp;&nbsp;{Sold
                    To / CABBV / Customer Name}
                </td>
            </tr>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblCommodityRqrd" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="* " />
                    &nbsp;<asp:Label ID="lblCommodity" runat="server" Text="Commodity:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddCommodity" runat="server" class="c_textxsmall" />
                    &nbsp;&nbsp;{Commodity / Classification}
                    <asp:RequiredFieldValidator ID="rfvCommodity" runat="server" ControlToValidate="ddCommodity"
                        ErrorMessage="Commodity is a required field." Font-Bold="False" ValidationGroup="vsAddEditChartSpec"><</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblPartNoRqrd" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                    &nbsp;<asp:Label ID="lblPartNo" runat="server" Text="Internal Part Number:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddPartNo" runat="server" class="c_textxsmall" />
                    <asp:RequiredFieldValidator ID="rfvPartNo" runat="server" ControlToValidate="ddPartNo"
                        ErrorMessage="Internal Part Number is a required field." Font-Bold="False" ValidationGroup="vsAddEditChartSpec"><</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblDesignLvlRqrd" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="* " />
                    &nbsp;<asp:Label ID="lblDesignLvl" runat="server" Text="Design Level:" />
                </td>
                <td>
                    <asp:TextBox ID="txtDesignLvl" runat="server" MaxLength="30" Width="200px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbDesignLvl" runat="server" TargetControlID="txtDesignLvl"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,- " />
                    <asp:RequiredFieldValidator ID="rfvDesignLvl" runat="server" ControlToValidate="txtDesignLvl"
                        ErrorMessage="Design Level is a required field." Font-Bold="False" ValidationGroup="vsAddEditChartSpec"><</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblMakeRqrd" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />&nbsp;<asp:Label
                        ID="lblMake" runat="server" Text="Make:" />
                </td>
                <td style="font-size: smaller">
                    <asp:DropDownList ID="ddMakes" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblModelRqrd" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />&nbsp;<asp:Label
                        ID="lblModel" runat="server" Text="Model:" />
                </td>
                <td style="font-size: smaller">
                    <asp:DropDownList ID="ddModel" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblProgramRqrd" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />&nbsp;<asp:Label
                        ID="lblProgram" runat="server" Text="Program:" />
                </td>
                <td class="c_textxsmall">
                    <asp:DropDownList ID="ddProgram" runat="server" CssClass="c_textxsmall" />
                    <asp:RequiredFieldValidator ID="rfvProgram" runat="server" ControlToValidate="ddProgram"
                        ErrorMessage="Program is a required field." Font-Bold="False" ValidationGroup="vsAddEditChartSpec"><</asp:RequiredFieldValidator>
                    <asp:ImageButton ID="iBtnPreviewDetail" runat="server" ImageUrl="~/images/PreviewUp.jpg"
                        ToolTip="Review Program Detail" Visible="false" />&nbsp;&nbsp;{Program / Platform
                    / Assembly Plant}
                </td>
            </tr>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblMYRqrd" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                    &nbsp;<asp:Label ID="lblMY" runat="server" Text="Model Year:" />
                </td>
                <td>
                    <asp:TextBox ID="txtModelYear" runat="server" MaxLength="6" Width="60px" CssClass="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbYear" runat="server" TargetControlID="txtModelYear"
                        FilterType="Custom,Numbers" ValidChars="." />
                    <asp:RequiredFieldValidator ID="rfvModelYear" runat="server" ControlToValidate="txtModelYear"
                        ErrorMessage="Model Year is a required field." Font-Bold="False" ValidationGroup="vsAddEditChartSpec"><</asp:RequiredFieldValidator>
                    <asp:RangeValidator ID="rvModelYear" runat="server" ErrorMessage="Model Year values must between Current Year to 2030.5"
                        ControlToValidate="txtModelYear" MinimumValue="2008" MaximumValue="2030.5" ValidationGroup="vsAddEditChartSpec"><</asp:RangeValidator>
                    <asp:TextBox ID="txtModelYearCompare" runat="server" MaxLength="6" Width="60px" Visible="false" />
                    <asp:CompareValidator ID="cvYear" runat="server" ControlToCompare="txtModelYear"
                        ControlToValidate="txtModelYearCompare" ErrorMessage="Model Year must be greater than Current Year."
                        Operator="LessThan" Type="Date" ValidationGroup="vsAddEditChartSpec"><</asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblObsolete" runat="server" Text="Record Status:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddObsolete" runat="server" CssClass="c_textxsmall">
                        <asp:ListItem Value="0">Active</asp:ListItem>
                        <asp:ListItem Value="1">Inactive</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_textxsmall" valign="top">
                    <asp:Label ID="lblComments" runat="server" Text="Comments:" />
                </td>
                <td>
                    <asp:TextBox ID="txtNotes" runat="server" MaxLength="200" Rows="8" TextMode="MultiLine"
                        Width="391px" class="c_textxsmall" /><br />
                    <asp:Label ID="lblNotes" runat="server" Font-Bold="True" ForeColor="Red" />
                </td>
            </tr>
        </table>
        <asp:Panel ID="TCPanel" runat="server" CssClass="collapsePanelHeader" BackColor="#ddffdd"
            Width="650PX">
            <%-- <asp:Image ID="imgTC" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                Height="12px" />&nbsp;--%>
            <asp:Label ID="lblTC" runat="server" CssClass="c_textbold" Text="SPECIFY REQUIRED TESTING BELOW:" />
        </asp:Panel>
        <%--<asp:Panel ID="TCContentPanel" runat="server" CssClass="collapsePanel" Width="650PX">--%>
        <table>
            <% If ViewState("KitPartNo") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblKitPartNo" runat="server" Text="Kit Part Number:" />
                </td>
                <td>
                    <asp:TextBox ID="txtKitPartNo" runat="server" MaxLength="25" Width="200px" />
                    <ajax:FilteredTextBoxExtender ID="ftbKitPartNo" runat="server" TargetControlID="txtKitPartNo"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,- " />
                </td>
            </tr>
            <% End If%>
            <% If ViewState("FamilyPartNo") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblFamilyPartNo" runat="server" Text="Family Part Number:" />
                </td>
                <td>
                    <asp:TextBox ID="txtFamilyPartNo" runat="server" MaxLength="50" Width="200px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbFamilyPartNo" runat="server" TargetControlID="txtFamilyPartNo"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,- " />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("BlankPartNo") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblBlankPartNo" runat="server" Text="Blank Part Number:" />
                </td>
                <td>
                    <asp:TextBox ID="txtBlankPartNo" runat="server" MaxLength="25" Width="200px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbBlankPartNo" runat="server" TargetControlID="txtBlankPartNo"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,- " />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("BlankSize") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblBlankSize" runat="server" Text="Blank Size:" />
                </td>
                <td>
                    <asp:TextBox ID="txtBlankSize" runat="server" MaxLength="100" Width="400px" class="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("SpGravFrom") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblSpGravFrom" runat="server" Text="Specific Gravity From:" />
                </td>
                <td class="c_textxsmall">
                    <asp:TextBox ID="txtSpGravFrom" runat="server" MaxLength="20" Width="60px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbSpGravFrom" runat="server" TargetControlID="txtSpGravFrom"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddSpGravFromUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("SpGravTo") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblSpGravTo" runat="server" Text="Specific Gravity To:" />
                </td>
                <td>
                    <asp:TextBox ID="txtSpGravTo" runat="server" MaxLength="20" Width="60px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbSpGravTo" runat="server" TargetControlID="txtSpGravTo"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddSpGravToUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("SpecFrequency") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblSpecFrequency" runat="server" Text="Spec Frequency:" />
                </td>
                <td>
                    <asp:TextBox ID="txtSpecFrequency" runat="server" MaxLength="50" Width="300px" class="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("ThicknessFrom") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblThicknessFrom" runat="server" Text="Thickness From:" />
                </td>
                <td>
                    <asp:TextBox ID="txtThicknessFrom" runat="server" MaxLength="20" Width="60px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbTRF" runat="server" TargetControlID="txtThicknessFrom"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddThicknessFromUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("ThicknessTo") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblThicknessTo" runat="server" Text="Thickness To:" />
                </td>
                <td>
                    <asp:TextBox ID="txtThicknessTo" runat="server" MaxLength="20" Width="60px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbTRT" runat="server" TargetControlID="txtThicknessTo"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddThicknessToUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("ThicknessFrequency") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblThicknessFrequency" runat="server" Text="Thickness Frequency:" />
                </td>
                <td>
                    <asp:TextBox ID="txtThicknessFrequency" runat="server" MaxLength="50" Width="300px"
                        class="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("TargetThickness") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblTargetThickness" runat="server" Text="Target Thickness:" />
                </td>
                <td>
                    <asp:TextBox ID="txtTargetThickness" runat="server" MaxLength="20" Width="60px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbTT" runat="server" TargetControlID="txtTargetThickness"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddTargetThicknessUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("Width") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblWidth" runat="server" Text="Width:" />
                </td>
                <td>
                    <asp:TextBox ID="txtWidth" runat="server" MaxLength="20" Width="60px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbWidth" runat="server" TargetControlID="txtWidth"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddWidthUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("ContainerDescription") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblContainerDescription" runat="server" Text="Container Description:" />
                </td>
                <td>
                    <asp:TextBox ID="txtContainerDescription" runat="server" MaxLength="50" Width="300px"
                        class="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("ContainerDimensions") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblContainerDimensions" runat="server" Text="Container Dimensions:" />
                </td>
                <td>
                    <asp:TextBox ID="txtContainerDimensions" runat="server" MaxLength="50" Width="300px"
                        class="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("PTAreaFrom") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblPTAreaFrom" runat="server" Text="PT Area From:" />
                </td>
                <td>
                    <asp:TextBox ID="txtPTAreaFrom" runat="server" MaxLength="20" Width="60px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbPTAreaFrom" runat="server" TargetControlID="txtPTAreaFrom"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddPTAreaFromUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("PTAreaTo") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblPTAreaTo" runat="server" Text="PT Area To:" />
                </td>
                <td>
                    <asp:TextBox ID="txtPTAreaTo" runat="server" MaxLength="20" Width="60px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbPTAreaTo" runat="server" TargetControlID="txtPTAreaTo"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddPTAreaToUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("SPQ") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblSPQRqrd" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                    &nbsp;<asp:Label ID="lblSPQ" runat="server" Text="Standard Pack Quantity:" />
                </td>
                <td>
                    <asp:TextBox ID="txtSPQ" runat="server" MaxLength="20" Width="100px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbSPQ" runat="server" TargetControlID="txtSPQ"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:RequiredFieldValidator ID="rfvSPQ" runat="server" ControlToValidate="txtSPQ"
                        ErrorMessage="SPQ is a required field." Font-Bold="False" ValidationGroup="vsAddEditChartSpec"><</asp:RequiredFieldValidator>
                </td>
            </tr>
            <%End If%>
            <% If ViewState("PcsPerHour") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblPcsPerHourRqrd" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="* " />
                    &nbsp;<asp:Label ID="lblPcsPerHour" runat="server" Text="Pcs/Hour:" />
                </td>
                <td>
                    <asp:TextBox ID="txtPcsPerHour" runat="server" MaxLength="20" Width="100px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbPPH" runat="server" TargetControlID="txtPcsPerHour"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:RequiredFieldValidator ID="rfvPcsPerHour" runat="server" ControlToValidate="txtPcsPerHour"
                        ErrorMessage="Pcs/Hour is a required field." Font-Bold="False" ValidationGroup="vsAddEditChartSpec"><</asp:RequiredFieldValidator>
                </td>
            </tr>
            <%End If%>
            <% If ViewState("PcsPerCycle") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblPcsPerCycleRqrd" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="* " />
                    &nbsp;<asp:Label ID="lblPcsPerCycle" runat="server" Text="Pcs/Cycle:" />
                </td>
                <td>
                    <asp:TextBox ID="txtPcsPerCycle" runat="server" MaxLength="20" Width="100px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbPPC" runat="server" TargetControlID="txtPcsPerCycle"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:RequiredFieldValidator ID="rfvPcsPerCycle" runat="server" ControlToValidate="txtPcsPerCycle"
                        ErrorMessage="Pcs/Cycle is a required field." Font-Bold="False" ValidationGroup="vsAddEditChartSpec"><</asp:RequiredFieldValidator>
                </td>
            </tr>
            <%End If%>
            <% If ViewState("SagSpecFrom") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblSagSpecFrom" runat="server" Text="Sag Spec From:" />
                </td>
                <td>
                    <asp:TextBox ID="txtSagSpecFrom" runat="server" MaxLength="20" Width="60px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbSagSpecFrom" runat="server" TargetControlID="txtSagSpecFrom"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddSagSpecFromUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("SagSpecTo") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblSagSpecTo" runat="server" Text="Sag Spec To:" />
                </td>
                <td>
                    <asp:TextBox ID="txtSagSpecTo" runat="server" MaxLength="20" Width="60px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbSagSpecTo" runat="server" TargetControlID="txtSagSpecTo"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddSagSpecToUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("SagPanelSize") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblSagPanelSize" runat="server" Text="Sag Panel Size:" />
                </td>
                <td>
                    <asp:TextBox ID="txtSagPanelSize" runat="server" MaxLength="20" Width="100px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbSPS" runat="server" TargetControlID="txtSagPanelSize"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddSagPanelUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("Travel") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblTravel" runat="server" Text="Travel:" />
                </td>
                <td>
                    <asp:TextBox ID="txtTravel" runat="server" MaxLength="20" Width="100px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbTravel" runat="server" TargetControlID="txtTravel"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("CallUpNo") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblCallUpNo" runat="server" Text="Call Up No:" />
                </td>
                <td>
                    <asp:TextBox ID="txtCallUpNo" runat="server" MaxLength="20" Width="100px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbCallUpno" runat="server" TargetControlID="txtCallUpno"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("LineSpeed") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblLineSpeed" runat="server" Text="Line Speed:" />
                </td>
                <td>
                    <asp:TextBox ID="txtLineSpeed" runat="server" MaxLength="20" Width="60px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbLineSpeed" runat="server" TargetControlID="txtLineSpeed"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddLineSpeed" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("PressCycles") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblPressCycles" runat="server" Text="Press Cycles:" />
                </td>
                <td>
                    <asp:TextBox ID="txtPressCycles" runat="server" MaxLength="20" Width="100px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbPressCucles" runat="server" TargetControlID="txtPressCycles"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("StandardTime") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblStandardTime" runat="server" Text="Standard Time:" />
                </td>
                <td>
                    <asp:TextBox ID="txtStandardTime" runat="server" MaxLength="20" Width="100px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbStandardTime" runat="server" TargetControlID="txtStandardTime"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("Quantity") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblQuantity" runat="server" Text="Total Shift Quantity:" />
                </td>
                <td>
                    <asp:TextBox ID="txtQuantity" runat="server" MaxLength="20" Width="100px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbQuantity" runat="server" TargetControlID="txtQuantity"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("OvenCondTemp") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblOvenCondTemp" runat="server" Text="Oven Condition Temp:" />
                </td>
                <td>
                    <asp:TextBox ID="txtOvenCondTemp" runat="server" MaxLength="20" Width="60px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbOvenCondTemp" runat="server" TargetControlID="txtOvenCondTemp"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddOvenCondTempUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("OvenCondTime") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblOvenCondTime" runat="server" Text="Oven Condition Time:" />
                </td>
                <td>
                    <asp:TextBox ID="txtOvenCondTime" runat="server" MaxLength="20" Width="60px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbOvenCondTime" runat="server" TargetControlID="txtOvenCondTime"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddOvenCondTimeUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("BondTemp") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblBondTemp" runat="server" Text="Bond Temp:" />
                </td>
                <td>
                    <asp:TextBox ID="txtBondTemp" runat="server" MaxLength="20" Width="60px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ttbBondTemp" runat="server" TargetControlID="txtBondTemp"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddBondTempUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("BondTime") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblBondTime" runat="server" Text="Bond Time:" />
                </td>
                <td>
                    <asp:TextBox ID="txtBondTime" runat="server" MaxLength="20" Width="60px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbBondTime" runat="server" TargetControlID="txtBondTime"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddBondTimeUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("BondPLFrom") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblBondPLFrom" runat="server" Text="Bond PL From:" />
                </td>
                <td>
                    <asp:TextBox ID="txtBondPLFrom" runat="server" MaxLength="20" Width="60px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbBondPLFrom" runat="server" TargetControlID="txtBondPLFrom"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddBondPLFromUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("BondPLTo") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblBondPLTo" runat="server" Text="Bond PL To:" />
                </td>
                <td>
                    <asp:TextBox ID="txtBondPLTo" runat="server" MaxLength="20" Width="60px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbBondPLTo" runat="server" TargetControlID="txtBondPLTo"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddBondPLToUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("ExpTemp") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblExpTemp" runat="server" Text="Expansion Temp:" />
                </td>
                <td>
                    <asp:TextBox ID="txtExpTemp" runat="server" MaxLength="20" Width="60px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbExpTemp" runat="server" TargetControlID="txtExpTemp"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddExpTempUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("ExpTime") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblExpTime" runat="server" Text="Expansion Time:" />
                </td>
                <td>
                    <asp:TextBox ID="txtExpTime" runat="server" MaxLength="20" Width="60px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbExpTime" runat="server" TargetControlID="txtExpTime"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddExpTimeUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("ExpSpecFrom") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblExpSpecFrom" runat="server" Text="Exp Spec From:" />
                </td>
                <td>
                    <asp:TextBox ID="txtExpSpecFrom" runat="server" MaxLength="20" Width="60px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbExpSpecFrom" runat="server" TargetControlID="txtExpSpecFrom"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddExpSpecFromUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("ExpSpecTo") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblExpSpecTo" runat="server" Text="Exp Spec To:" />
                </td>
                <td>
                    <asp:TextBox ID="txtExpSpecTo" runat="server" MaxLength="20" Width="60px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbExpSpecTo" runat="server" TargetControlID="txtExpSpecTo"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddExpSpecToUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("Configuration") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblConfiguration" runat="server" Text="Configuration" />
                </td>
                <td>
                    <asp:TextBox ID="txtConfiguration" runat="server" MaxLength="50" Width="300px" class="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("WeightFrom") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblWeightFrom" runat="server" Text="Weight From:" />
                </td>
                <td>
                    <asp:TextBox ID="txtWeightFrom" runat="server" MaxLength="20" Width="60px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbWeightFrom" runat="server" TargetControlID="txtWeightFrom"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddWeightFromUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("WeightTo") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblWeightTo" runat="server" Text="Weight To:" />
                </td>
                <td>
                    <asp:TextBox ID="txtWeightTo" runat="server" MaxLength="20" Width="60px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbWeightTo" runat="server" TargetControlID="txtWeightTo"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddWeightToUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("WeightFrequency") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblWeightFrequency" runat="server" Text="Weight Frequency:" />
                </td>
                <td>
                    <asp:TextBox ID="txtWeightFrequency" runat="server" MaxLength="50" Width="300px"
                        class="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("Moldability") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblMoldability" runat="server" Text="Moldability:" />
                </td>
                <td>
                    <asp:TextBox ID="txtMoldability" runat="server" MaxLength="50" Width="300px" class="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("MoldOvenCondTemp") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblMoldOvenCondTemp" runat="server" Text="Mold Oven Cond Temp:" />
                </td>
                <td>
                    <asp:TextBox ID="txtMoldOvenCondTemp" runat="server" MaxLength="20" Width="60px"
                        class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbMoldOvenCondTemp" runat="server" TargetControlID="txtMoldOvenCondTemp"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddMoldOvenCondTempUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("MoldOvenCondTime") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblMoldOvenCondTime" runat="server" Text="Mold Oven Cond Time:" />
                </td>
                <td>
                    <asp:TextBox ID="txtMoldOvenCondTime" runat="server" MaxLength="20" Width="60px"
                        class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbMoldOvenCondTime" runat="server" TargetControlID="txtMoldOvenCondTime"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddMoldOvenCondTimeUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("MoldOvenFrequency") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblMoldOvenFrequency" runat="server" Text="Mold Oven Frequency:" />
                </td>
                <td>
                    <asp:TextBox ID="txtMoldOvenFrequency" runat="server" MaxLength="50" Width="300px"
                        class="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("Coating") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblCoating" runat="server" Text="Coating:" />
                </td>
                <td>
                    <asp:TextBox ID="txtCoating" runat="server" MaxLength="50" Width="300px" class="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("Shrinkage") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblShrinkage" runat="server" Text="Shrinkage:" />
                </td>
                <td>
                    <asp:TextBox ID="txtShrinkage" runat="server" MaxLength="20" Width="60px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbShrinkage" runat="server" TargetControlID="txtShrinkage"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddShrinkageUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("ShrinkOvenCondTemp") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblShrinkOvenCondTemp" runat="server" Text="Shrink Oven Cond Temp:" />
                </td>
                <td>
                    <asp:TextBox ID="txtShrinkOvenCondTemp" runat="server" MaxLength="20" Width="60px"
                        class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbShrinkOvenCondTemp" runat="server" TargetControlID="txtShrinkOvenCondTemp"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddShrinkOvenCondTempUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("ShrinkOvenCondTime") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblShrinkOvenCondTime" runat="server" Text="Shrink Oven Cond Time:" />
                </td>
                <td>
                    <asp:TextBox ID="txtShrinkOvenCondTime" runat="server" MaxLength="20" Width="60px"
                        class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbShrinkOvenCondTime" runat="server" TargetControlID="txtShrinkOvenCondTime"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddShrinkOvenCondTimeUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("ShrinkOvenFrequency") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblShrinkOvenFrequency" runat="server" Text="Shrink Oven Frequency:" />
                </td>
                <td>
                    <asp:TextBox ID="txtShrinkOvenFrequency" runat="server" MaxLength="50" Width="300px"
                        class="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("BallTestFrom") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblBallTestFrom" runat="server" Text="Ball Test From:" />
                </td>
                <td>
                    <asp:TextBox ID="txtBallTestFrom" runat="server" MaxLength="20" Width="60px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbBallTestFrom" runat="server" TargetControlID="txtBallTestFrom"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddBallTestFromUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("BallTestTo") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblBallTestTo" runat="server" Text="Ball Test To:" />
                </td>
                <td>
                    <asp:TextBox ID="txtBallTestTo" runat="server" MaxLength="20" Width="60px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbBallTestTo" runat="server" TargetControlID="txtBallTestTo"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddBallTestToUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("BallFrequency") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblBallFrequency" runat="server" Text="Ball Frequency:" />
                </td>
                <td>
                    <asp:TextBox ID="txtBallFrequency" runat="server" MaxLength="50" Width="300px" class="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("ReleasePoly") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblReleasePoly" runat="server" Text="Release Poly:" />
                </td>
                <td>
                    <asp:TextBox ID="txtReleasePoly" runat="server" MaxLength="25" Width="300px" class="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("GluePumpCapacity") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblGluePumpCapacity" runat="server" Text="Glue Pump Capacity:" />
                </td>
                <td>
                    <asp:TextBox ID="txtGluePumpCapacity" runat="server" MaxLength="20" Width="60px"
                        class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbGluePumpCapacity" runat="server" TargetControlID="txtGluePumpCapacity"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddGluePumpCapacityUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("NominalWeight") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblNominalWeight" runat="server" Text="Nominal Weight:" />
                </td>
                <td>
                    <asp:TextBox ID="txtNominalWeight" runat="server" MaxLength="20" Width="60px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbNominalWeight" runat="server" TargetControlID="txtNominalWeight"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddNominalWeightUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("HangTest") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblHangTest" runat="server" Text="Hang Test:" />
                </td>
                <td>
                    <asp:TextBox ID="txtHangTest" runat="server" MaxLength="20" Width="60px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbHangTest" runat="server" TargetControlID="txtHangTest"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddHangTestUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("HardnessFrom") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblHardnessFrom" runat="server" Text="Hardness From:" />
                </td>
                <td>
                    <asp:TextBox ID="txtHardnessFrom" runat="server" MaxLength="20" Width="60px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbHardnessFrom" runat="server" TargetControlID="txtHardnessFrom"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddHardnessFromUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("HardnessTo") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblHardnessTo" runat="server" Text="Hardness To:" />
                </td>
                <td>
                    <asp:TextBox ID="txtHardnessTo" runat="server" MaxLength="20" Width="60px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbHardnessTo" runat="server" TargetControlID="txtHardnessTo"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                    <asp:DropDownList ID="ddHardnessToUOM" runat="server" CssClass="c_textxsmall" />
                </td>
            </tr>
            <%End If%>
            <% If ViewState("Elongation") = True Then%>
            <tr>
                <td class="p_textxsmall">
                    <asp:Label ID="lblElongation" runat="server" Text="Elongation:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddElongationUOM" runat="server" CssClass="c_textxsmall" />
                    <asp:TextBox ID="txtElongation" runat="server" MaxLength="20" Width="60px" class="c_textxsmall" />
                    <ajax:FilteredTextBoxExtender ID="ftbElongation" runat="server" TargetControlID="txtElongation"
                        FilterType="Custom, Numbers" ValidChars="-.," />
                </td>
            </tr>
            <%End If%>
        </table>
        <ajax:CascadingDropDown ID="cddUGNLocation" runat="server" TargetControlID="ddUGNLocation"
            SelectedValue='<%#   HttpContext.Current.Request.Cookies("UGNDB_TMLoc").Value %>'
            Category="UGNLocation" PromptText="Select a UGN Location" LoadingText="[Loading UGN Location...]"
            ServicePath="~/WS/GeneralCDDService.asmx" ServiceMethod="GetUGNLocationByTMFac" />
        <ajax:CascadingDropDown ID="cddWorkCenter" runat="server" TargetControlID="ddWorkCenter"
            ParentControlID="ddUGNLocation" Category="WorkCenter" PromptText="Select a Work Center"
            LoadingText="[Loading Work Centers...]" ServicePath="~/WS/GeneralCDDService.asmx"
            ServiceMethod="GetWorkCenter" />
        <ajax:CascadingDropDown ID="cddOEMMfg" runat="server" TargetControlID="ddOEMMfg"
            ParentControlID="ddWorkCenter" Category="OEMMfg" PromptText="Please select an OEM Manufacturer."
            LoadingText="[Loading OEM Manufacturer...]" ServicePath="~/WS/VehicleCDDService.asmx"
            ServiceMethod="GetOEMMfg" />
        <ajax:CascadingDropDown ID="cddCustomer" runat="server" TargetControlID="ddCustomer"
            ParentControlID="ddOEMMfg" Category="CABBVSOLDTO" PromptText="Select a Customer Abbreviation"
            LoadingText="[Loading Customer Abbreviations...]" ServicePath="~/WS/GeneralCDDService.asmx"
            ServiceMethod="GetCABBVbyOEMMfg" />
        <ajax:CascadingDropDown ID="cddPartNo" runat="server" TargetControlID="ddPartNo"
            Category="PartNo" ParentControlID="ddOEMMfg" PromptText="Please select a Part Number."
            LoadingText="[Loading Part Numbers...]" ServicePath="~/WS/VehicleCDDService.asmx"
            ServiceMethod="GetPartNos" />
        <ajax:CascadingDropDown ID="cddMakes" runat="server" TargetControlID="ddMakes" Category="Make"
            ParentControlID="ddOEMMfg" PromptText="Please select a Make." LoadingText="[Loading Makes...]"
            ServicePath="~/WS/VehicleCDDService.asmx" ServiceMethod="GetMakesSearch" />
        <ajax:CascadingDropDown ID="cddModel" runat="server" TargetControlID="ddModel" ParentControlID="ddMakes"
            Category="Model" PromptText="Please select a Model." LoadingText="[Loading Models...]"
            ServicePath="~/WS/VehicleCDDService.asmx" ServiceMethod="GetModelMaint" />
        <ajax:CascadingDropDown ID="cddProgram" runat="server" TargetControlID="ddProgram"
            ParentControlID="ddModel" Category="Program" PromptText="Please select a Program."
            LoadingText="[Loading Programs...]" ServicePath="~/WS/VehicleCDDService.asmx"
            ServiceMethod="GetProgramsPlatformAssembly" />
        <br />
        <br />
        <%-- </asp:Panel>--%>
        <%--        <ajax:CollapsiblePanelExtender ID="TCExtender" runat="server" TargetControlID="TCContentPanel"
            ExpandControlID="TCPanel" CollapseControlID="TCPanel" Collapsed="FALSE" TextLabelID="lblTC"
            ExpandedText="SPECIFY REQUIRED TESTING BELOW:" CollapsedText="SPECIFY REQUIRED TESTING:"
            ImageControlID="imgTC" CollapsedImage="~/images/expand_blue.jpg" ExpandedImage="~/images/collapse_blue.jpg"
            SuppressPostBack="true" ScrollContents="true" ExpandedSize="300">
        </ajax:CollapsiblePanelExtender>
--%>
        <table>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Save" CausesValidation="true" ValidationGroup="vsAddEditChartSpec" />
                    <asp:Button ID="btnReset2" runat="server" Text="Reset" CausesValidation="False" />
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
