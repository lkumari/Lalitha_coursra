<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="TestIssuanceDetail.aspx.vb" Inherits="RnD_TID" Title="Untitled Page"
    MaintainScrollPositionOnPostback="True" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="1100px">
        <asp:Label ID="lblErrors" runat="server" Font-Bold="True" Font-Size="Large" ForeColor="Red"
            Text="Label" Visible="False" />
        <%If HttpContext.Current.Request.QueryString("pReqID") <> Nothing And HttpContext.Current.Request.QueryString("pReqID") <> "" Then%>
        <table>
            <tr>
                <td class="p_smalltextbold" style="width: 300px; color: #990000; height: 15px;">
                    Edit data below or press
                    <asp:Button ID="btnAdd" runat="server" CausesValidation="False" Text="Add" />
                    to enter new data.&nbsp;
                </td>
            </tr>
        </table>
        <%End If%>
        <hr />
        <br />
        <table width="80%">
            <tr>
                <td class="p_text">
                    Test Issuance Request #:
                </td>
                <td class="c_textbold" style="color: #990000;" colspan="3">
                    <%If HttpContext.Current.Request.QueryString("pReqID") = Nothing Or HttpContext.Current.Request.QueryString("pReqID") = "" Then%>
                    <i>Automated</i>
                    <%Else%>
                    <asp:Label ID="lblTestIssuanceReq" runat="server" Width="200px" />
                    <% End If%>
                </td>
            </tr>
            <tr>
                <td class="p_text" style="height: 26px">
                    <asp:Label ID="lblSampleProdDesc" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="* " />
                    Sample Product Description:
                </td>
                <td style="height: 26px" colspan="3">
                    <asp:TextBox ID="txtSampleProdDesc" runat="server" MaxLength="100" Width="400px" />
                    <asp:RequiredFieldValidator ID="rfvSampleProdDesc" runat="server" ControlToValidate="txtSampleProdDesc"
                        ErrorMessage="Sample Product Description is a required field." Font-Bold="False"><</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="p_text" style="height: 26px">
                    Request Status:
                </td>
                <td style="color: #990000; height: 26px" class="c_text" colspan="3">
                    <asp:Label ID="lblRequestStatus" runat="server" Width="200px" />
                    <asp:DropDownList ID="ddRequestStatus" runat="server" AutoPostBack="True">
                        <asp:ListItem Selected="True">Unassigned</asp:ListItem>
                        <asp:ListItem>Abandoned</asp:ListItem>
                        <asp:ListItem>Completed</asp:ListItem>
                        <asp:ListItem>Nearly Complete</asp:ListItem>
                        <asp:ListItem>On Hold</asp:ListItem>
                        <asp:ListItem>Outstanding</asp:ListItem>
                        <asp:ListItem>Overdue</asp:ListItem>
                        <asp:ListItem>Testing In Progress</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text" style="height: 26px">
                    <asp:CheckBox ID="cbACReq" runat="server" TextAlign="Left" />
                </td>
                <td class="c_text" style="color: #990000; height: 26px">
                    Check this box if Acoustic Testing is Required.
                </td>
                <td class="p_text">
                    <asp:Label ID="lvlACProjNo" runat="server" Text="Acoustic Project No.:" />
                </td>
                <td class="c_text">
                    <asp:HyperLink ID="hlnkACProjNo" runat="server" Text="" Target="_blank" />
                </td>
            </tr>
        </table>
        <ajax:CascadingDropDown ID="cddOEMMfg" runat="server" TargetControlID="ddCustomer"
            Category="OEMMfg" PromptText="Please select a Customer." LoadingText="[Loading Customers...]"
            ServicePath="~/WS/VehicleCDDService.asmx" ServiceMethod="GetOEMMfg" />
        <ajax:CascadingDropDown ID="cddProgram" runat="server" TargetControlID="ddProgram"
            ParentControlID="ddCustomer" Category="Program" PromptText="Please select a Program."
            LoadingText="[Loading Programs...]" ServicePath="~/WS/VehicleCDDService.asmx"
            ServiceMethod="GetProgramsModelPlatformAssembly" />
        <br />
        <table width="100%" border="0">
            <tr>
                <td style="width: 30px">
                    <asp:Menu ID="mnuTabs" Height="30px" runat="server" Orientation="Horizontal" StaticEnableDefaultPopOutImage="false"
                        StaticMenuItemStyle-CssClass="tab" StaticSelectedStyle-CssClass="selectedTab"
                        CssClass="tabs">
                        <Items>
                            <asp:MenuItem Text="Request Info" Value="0" ImageUrl="" Selected="true" />
                            <asp:MenuItem Text="Customer / Part" Value="1" ImageUrl="" />
                            <asp:MenuItem Text="Administrative" Value="2" ImageUrl="" />
                            <asp:MenuItem Text="Assignments" Value="3" ImageUrl="" />
                            <asp:MenuItem Text="Test Reports" Value="4" ImageUrl="" />
                        </Items>
                    </asp:Menu>
                </td>
            </tr>
        </table>
        <asp:MultiView ID="mvTabs" runat="server" Visible="true" ActiveViewIndex="0" EnableViewState="true">
            <asp:View ID="vwRequestInfoTab" runat="server">
                <table>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblSampleIssuer" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Sample Issuer:
                        </td>
                        <td style="height: 26px; width: 362px;">
                            <asp:DropDownList ID="ddSampleIssuer" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvSampleIssuer" runat="server" ControlToValidate="ddSampleIssuer"
                                ErrorMessage="Sample Issuer is a required field." Font-Bold="False"><</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Sample Issuer Department:
                        </td>
                        <td style="width: 362px">
                            <asp:TextBox ID="txtDepartment" runat="server" MaxLength="50" Width="250px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblPlantMfg" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            UGN Location:
                        </td>
                        <td style="width: 362px">
                            <asp:DropDownList ID="ddUGNFacility" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvUGNFacility" runat="server" ControlToValidate="ddUGNFacility"
                                ErrorMessage="UGN Location is a required field." Font-Bold="False"><</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label7" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Commodity:
                        </td>
                        <td style="width: 362px">
                            <asp:DropDownList ID="ddCommodity" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvCommodity" runat="server" ControlToValidate="ddCommodity"
                                ErrorMessage="Commodity is a required field." Font-Bold="False"><</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Purchased Good:
                        </td>
                        <td style="width: 362px">
                            <asp:DropDownList ID="ddPurchasedGood" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <%If HttpContext.Current.Request.QueryString("pReqCategory") = 2 Then%>
                        <td class="p_text">
                            Formula:
                        </td>
                        <td style="width: 362px">
                            <asp:DropDownList ID="ddFormula" runat="server" />
                        </td>
                        <% End If%>
                    </tr>
                    <tr>
                        <td class="p_text">
                            General Material Thickness:
                        </td>
                        <td style="width: 362px">
                            <asp:TextBox ID="txtGeneralThickness" runat="server" MaxLength="50" Width="400px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" style="white-space: nowrap; height: 26px">
                            Quantity of Test Samples:
                        </td>
                        <td style="width: 362px; height: 26px;">
                            <asp:TextBox ID="txtSampleQty" runat="server" MaxLength="100" Width="400px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Request Date:
                        </td>
                        <td style="width: 362px">
                            <asp:TextBox ID="txtTodaysDate" runat="server" ReadOnly="True" Width="80px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" style="white-space: nowrap">
                            <asp:Label ID="lblTestCmpltDt" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Date Requested for Test Completion:
                        </td>
                        <td style="width: 362px">
                            <asp:TextBox ID="txtTestCmpltDt" runat="server" MaxLength="10" Width="80px" />
                            <ajax:FilteredTextBoxExtender ID="ftbeTestCmpltDt" runat="server" TargetControlID="txtTestCmpltDt"
                                FilterType="Custom, Numbers" ValidChars="/" />
                            &nbsp;<asp:ImageButton ID="imgTestCmpltDt" runat="server" AlternateText="Click to show calendar"
                                CausesValidation="False" Height="19px" ImageAlign="Middle" ImageUrl="~/images/AJAX/Calendar_scheduleHS.png"
                                Width="19px" />
                            <ajax:CalendarExtender ID="ceTestCmpltDt" runat="server" PopupButtonID="imgTestCmpltDt"
                                TargetControlID="txtTestCmpltDt" />
                            <asp:RequiredFieldValidator ID="rfvTestCmpltDt" runat="server" ControlToValidate="txtTestCmpltDt"
                                ErrorMessage="Date Requested for Test Completion is a required field." Font-Bold="False"><</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revTestCmpltDt" runat="server" ControlToValidate="txtTestCmpltDt"
                                ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format ' Font-Bold="True"
                                ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                Width="8px"><</asp:RegularExpressionValidator>
                            <asp:CompareValidator ID="cvTestCmpltDt" runat="server" ControlToCompare="txtTodaysDate"
                                ControlToValidate="txtTestCmpltDt" ErrorMessage="Date Requested for Test Completion must be greater than or equal to Current Date."
                                Operator="GreaterThanEqual" Type="Date"><</asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" style="white-space: nowrap">
                            Estimated Annual Cost Savings:
                        </td>
                        <td style="width: 362px">
                            <asp:TextBox ID="txtEstAnnCostSav" runat="server" MaxLength="15" Width="80px" Text="0.00" />
                            <asp:RangeValidator ID="rvEstAnnCostSav" runat="server" ControlToValidate="txtEstAnnCostSav"
                                Display="Dynamic" ErrorMessage="Estimated Annual Cost Savings requires a numeric value -999999.99 to 999999.99"
                                Height="16px" MaximumValue="999999.99" MinimumValue="-999999.99" Type="Double"><</asp:RangeValidator>
                            <ajax:FilteredTextBoxExtender ID="ftbeEstAnnCostSav" runat="server" TargetControlID="txtEstAnnCostSav"
                                FilterType="Custom, Numbers" ValidChars="-." />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblAppropriation" runat="server" Text="Appropriation (A, D, P, T, R):" />
                        </td>
                        <td class="c_text">
                            <asp:TextBox ID="txtAppropriation" runat="server" MaxLength="15" Width="100px" AutoPostBack="True" />
                            <asp:HyperLink ID="hplkAppropriation" runat="server" Font-Underline="true" ForeColor="Blue"
                                Target="_blank" Visible="false" />
                            <asp:TextBox ID="txtProjectTitle" runat="server" Visible="false" Width="16px" />
                            <asp:TextBox ID="txtDefinedCapex" runat="server" Visible="false" Width="16px" />
                            <asp:TextBox ID="txtProjectStatus" runat="server" Visible="false" Width="16px" />
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td class="p_text" valign="top" style="width: 213px">
                            <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Description of Required Testing:
                        </td>
                        <td>
                            <asp:TextBox ID="txtDescReqTesting" runat="server" Rows="5" TextMode="MultiLine"
                                Width="500px" />
                            <asp:RequiredFieldValidator ID="rfvDescReqTesting" runat="server" ControlToValidate="txtDescReqTesting"
                                ErrorMessage="Description of Required Testing is a required field." Font-Bold="False"><</asp:RequiredFieldValidator><br />
                            <asp:Label ID="lblDescReqTesting" runat="server" Font-Bold="True" ForeColor="Red" />
                        </td>
                    </tr>
                    <%If HttpContext.Current.Request.QueryString("pReqCategory") <> 2 Then%>
                    <tr>
                        <td class="p_text" valign="top" style="width: 213px">
                            Part Application/Market:
                        </td>
                        <td>
                            <asp:TextBox ID="txtPartAppMkt" runat="server" Rows="5" TextMode="MultiLine" Width="500px" /><br />
                            <asp:Label ID="lblPartAppMkt" runat="server" Font-Bold="True" ForeColor="Red" /><br />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" valign="top" style="width: 213px">
                            Objectives and Performance Targets:
                        </td>
                        <td>
                            <asp:TextBox ID="txtObjPerfTargets" runat="server" Rows="5" TextMode="MultiLine"
                                Width="500px" /><br />
                            <asp:Label ID="lblObjPerfTargets" runat="server" Font-Bold="True" ForeColor="Red" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" valign="top" style="width: 213px">
                            Misc Agenda or Remaining Outstanding Items:
                        </td>
                        <td>
                            <asp:TextBox ID="txtMiscAgenda" runat="server" Rows="5" TextMode="MultiLine" Width="500px" /><br />
                            <asp:Label ID="lblMiscAgenda" runat="server" Font-Bold="True" ForeColor="Red" />
                        </td>
                    </tr>
                    <%End If%>
                    <tr>
                        <td style="width: 213px; height: 21px;">
                        </td>
                        <td style="height: 21px">
                            <br />
                            <asp:Button ID="btnSave1" runat="server" Text="Save" CausesValidation="true" />&nbsp;
                            <asp:Button ID="btnReset1" runat="server" Text="Reset" CausesValidation="False" />&nbsp;<asp:Button
                                ID="btnDelete" runat="server" Text="Delete" CausesValidation="False" />
                            <ajax:ConfirmButtonExtender ID="ceDelete" runat="server" TargetControlID="btnDelete"
                                ConfirmText="Are you sure you want to delete this record?" />
                            &nbsp;
                            <%If HttpContext.Current.Request.QueryString("pReqID") <> "" Then%>
                            <asp:Button ID="btnSubmit1" runat="server" Text="Submit Request &gt;&gt;" CausesValidation="true"
                                Width="120px" />
                            <ajax:ConfirmButtonExtender ID="ceSubmit1" runat="server" TargetControlID="btnSubmit1"
                                ConfirmText="Are you sure you want to submit Test Request?" />
                            <%End If%>
                        </td>
                    </tr>
                </table>
                <asp:ValidationSummary ID="vsRnD" runat="server" Font-Size="X-Small" ShowMessageBox="True"
                    ShowSummary="true" />
            </asp:View>
            <asp:View ID="vwCustomerPart" runat="server">
                <asp:Label CssClass="c_text" Font-Size="Small" Font-Bold="true" ForeColor="Red" ID="lblMsgCategory4"
                    runat="server" Text="Note: ECI No and/or VALID DMS Drawing is required for New Program Launch."
                    Visible="false" />
                <table>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Customer:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddCustomer" runat="server" AppendDataBoundItems="true">
                                <asp:ListItem Text="N/A" Value="N/A" Selected="True">N/A</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvCustomer" runat="server" ControlToValidate="ddCustomer"
                                ErrorMessage="Customer is a required field." Font-Bold="False" ValidationGroup="vsCustomerPart"><</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" valign="top">
                            <asp:Label ID="Label5" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Program:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddProgram" runat="server" AppendDataBoundItems="true">
                                <asp:ListItem Text="N/A" Value="0" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvProgram" runat="server" ControlToValidate="ddProgram"
                                ErrorMessage="Program is a required field." Font-Bold="False" ValidationGroup="vsCustomerPart"><</asp:RequiredFieldValidator><br />
                            {Program / Platform / Model / Assembly Plant}
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label6" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Internal Part No:
                        </td>
                        <td>
                            <asp:TextBox ID="txtPartNo" runat="server" Width="200px" MaxLength="40" />
                            <asp:RequiredFieldValidator ID="rfvPartNo" runat="server" ControlToValidate="txtPartNo"
                                ErrorMessage="Part No is a required field." Font-Bold="False" ValidationGroup="vsCustomerPart"><</asp:RequiredFieldValidator>
                            <ajax:FilteredTextBoxExtender ID="ftbPartNo" runat="server" TargetControlID="txtPartNo"
                                FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,- " />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Design Level:
                        </td>
                        <td>
                            <asp:TextBox ID="txtDesignLvl" runat="server" MaxLength="30" Width="200px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Cost Sheet ID:
                        </td>
                        <td>
                            <asp:TextBox ID="txtCostSheetID" runat="server" Width="100px" MaxLength="10" />
                            <ajax:FilteredTextBoxExtender ID="ftbeCostSheetID" runat="server" FilterType="Numbers"
                                TargetControlID="txtCostSheetID" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            ECI No:
                        </td>
                        <td>
                            <asp:TextBox ID="txtECINo" runat="server" Width="100px" MaxLength="10" />
                            <asp:Label ID="lblReqECI" runat="server" Font-Bold="True" ForeColor="Red" Text="<"
                                Visible="false" />
                            <ajax:FilteredTextBoxExtender ID="ftbECINo" runat="server" FilterType="Numbers" TargetControlID="txtECINo" />
                            <asp:ImageButton ID="iBtnECISearch" runat="server" ImageUrl="~/images/Search.gif"
                                ToolTip="Click here to search for an ECI." />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            DMS Drawing No:
                        </td>
                        <td>
                            <asp:TextBox ID="txtDrawingNo" runat="server" Width="100px" MaxLength="10" />
                            <asp:Label ID="lblReqDMS" runat="server" Font-Bold="True" ForeColor="Red" Text="<"
                                Visible="false" />
                            <asp:ImageButton ID="iBtnDrawingSearch" runat="server" ImageUrl="~/images/Search.gif"
                                ToolTip="Click here to search for a DMS Drawing." />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Customer Spec No:
                        </td>
                        <td>
                            <asp:TextBox ID="txtCustSpecNo" runat="server" MaxLength="50" Width="200px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Lot No:
                        </td>
                        <td>
                            <asp:TextBox ID="txtLotNo" runat="server" MaxLength="50" Width="200px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Manufacture Date:
                        </td>
                        <td>
                            <asp:TextBox ID="txtMfgDt" runat="server" MaxLength="10" Width="80px" />
                            <ajax:FilteredTextBoxExtender ID="ftbMfgDt" runat="server" TargetControlID="txtMfgDt"
                                FilterType="Custom, Numbers" ValidChars="/" />
                            &nbsp;<asp:ImageButton ID="imgMfgDt" runat="server" AlternateText="Click to show calendar"
                                CausesValidation="False" Height="19px" ImageAlign="Middle" ImageUrl="~/images/AJAX/Calendar_scheduleHS.png"
                                Width="19px" />
                            <ajax:CalendarExtender ID="ceMfgDt" runat="server" PopupButtonID="imgMfgDt" TargetControlID="txtMfgDt" />
                            <asp:RegularExpressionValidator ID="revMfgDt" runat="server" ControlToValidate="txtMfgDt"
                                ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format ' Font-Bold="True"
                                ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                Width="8px" ValidationGroup="vsCustomerPart"><</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnAddtoGrid" runat="server" Text="Add" ToolTip="Add to grid." ValidationGroup="vsCustomerPart"
                                CausesValidation="true" />
                        </td>
                    </tr>
                </table>
                <asp:ValidationSummary ID="vsCustomerPart" runat="server" Font-Size="X-Small" ShowMessageBox="True"
                    ShowSummary="true" ValidationGroup="vsCustomerPart" />
                <br />
                <br />
                <asp:GridView ID="gvCustomerPart" runat="server" AutoGenerateColumns="False" DataKeyNames="RequestID,RowID"
                    OnRowDataBound="gvCustomerPart_RowDataBound" DataSourceID="odsCustomerPart" CellPadding="4"
                    EmptyDataText="No data available for grid view. Use fields above to add new entry."
                    GridLines="Horizontal" Width="1100px" PageSize="100" SkinID="StandardGridWOFooter">
                    <Columns>
                        <asp:BoundField DataField="ddCustomerDesc" HeaderText="Customer" ReadOnly="True"
                            SortExpression="ddCustomerDesc">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ProgramName" HeaderText="Program / Platform / Model / Assembly Plant"
                            SortExpression="ProgramName">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Internal Part No" SortExpression="PartNo">
                            <EditItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Eval("PartNo") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("PartNo") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="PartName" HeaderText="Part Name" SortExpression="PartName">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DesignLevel" HeaderText="Design Level" SortExpression="DesignLevel">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Cost Sheet ID" SortExpression="CostSheetID">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlnkCostSheet" runat="server" NavigateUrl='<%# GoToCostSheetDetail(Replace(DataBinder.Eval(Container.DataItem, "CostSheetID" ) & "", ",", environment.newline)) %>'
                                    Target="_blank" Font-Underline="true" Text='<%# Bind("CostSheetID") %>' />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ECI No." SortExpression="ECINo">
                            <ItemTemplate>
                                <asp:HyperLink ID="hlnkECI" runat="server" NavigateUrl='<%# GoToECIDetail(Replace(DataBinder.Eval(Container.DataItem, "ECINo" ) & "", ",", environment.newline)) %>'
                                    Target="_blank" Font-Underline="true" Text='<%# Bind("ECINo") %>' />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DMS Drawing No." SortExpression="DrawingNo">
                            <ItemTemplate>
                                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# GoToDMSDetail(Replace(DataBinder.Eval(Container.DataItem, "DrawingNo" ) & "", ",", environment.newline)) %>'
                                    Target="_blank" Font-Underline="true" Text='<%# Bind("DrawingNo") %>' />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Customer Spec No." SortExpression="CustomerSpecNo">
                            <ItemTemplate>
                                <asp:Label ID="lblCustSpecNo" runat="server" Text='<%# Bind("CustomerSpecNo") %>' />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Lot No." SortExpression="LotNo">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("LotNo") %>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("LotNo") %>' />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Manufacture Date" SortExpression="ManufactureDate">
                            <EditItemTemplate>
                                <asp:Label ID="Label4" runat="server" Text='<%# Eval("ManufactureDate") %>' />
                            </EditItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("ManufactureDate") %>' />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                    ImageUrl="~/images/delete.jpg" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsCustomerPart" runat="server" SelectMethod="GetTestIssuanceCustomerPart"
                    OldValuesParameterFormatString="original_{0}" TypeName="TestIssuanceCustomerPartBLL"
                    DeleteMethod="DeleteTestIssuanceCustomerPart">
                    <DeleteParameters>
                        <asp:Parameter Name="RequestID" Type="Int32" />
                        <asp:Parameter Name="RowID" Type="Int32" />
                        <asp:Parameter Name="original_RequestID" Type="Int32" />
                        <asp:Parameter Name="original_RowID" Type="Int32" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:QueryStringParameter Name="RequestID" QueryStringField="pReqID" Type="Int32"
                            DefaultValue="0" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </asp:View>
            <asp:View ID="vwAdmin" runat="server" EnableTheming="False">
                <table>
                    <tr>
                        <td class="p_text" valign="top">
                            Objective:
                        </td>
                        <td>
                            <asp:TextBox ID="txtObjective" runat="server" MaxLength="250" Rows="3" TextMode="MultiLine"
                                Width="400px"></asp:TextBox><br />
                            <asp:Label ID="lblObjective" runat="server" Font-Bold="True" ForeColor="Red" /><br />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" valign="top">
                            Status Notes:
                        </td>
                        <td>
                            <asp:TextBox ID="txtStatusNotes" runat="server" MaxLength="400" Rows="3" TextMode="MultiLine"
                                Width="500px" /><br />
                            <asp:Label ID="lblStatusNotes" runat="server" Font-Bold="True" ForeColor="Red" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Priority:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddPriority" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Estimated Man Hours Remaining:
                        </td>
                        <td>
                            <asp:TextBox ID="txtEstManHrs" runat="server" MaxLength="8" Width="60px" Text="0.00" />
                            <asp:RangeValidator ID="rvEstManHrs" runat="server" ControlToValidate="txtEstManHrs"
                                ErrorMessage="Estimated Man Hours Remaining requires a numeric value -999.99 to 999.99"
                                MaximumValue="999.99" MinimumValue="-999.99" Height="16px" Display="Dynamic"
                                Type="Double" ValidationGroup="vsAdmin"><</asp:RangeValidator>
                            <ajax:FilteredTextBoxExtender ID="ftbeEstManHrs" runat="server" TargetControlID="txtEstManHrs"
                                FilterType="Custom, Numbers" ValidChars="-." />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Did Drawing Review Occur?
                        </td>
                        <td>
                            <asp:DropDownList ID="ddDrawReview" runat="server">
                                <asp:ListItem Value="N/A" Selected="True">N/A</asp:ListItem>
                                <asp:ListItem Value="No">No</asp:ListItem>
                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Testing Classification:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddTestClass" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Longest Aging Cycle in hours:
                        </td>
                        <td>
                            <asp:TextBox ID="txtLongestAgingCycle" runat="server" MaxLength="10" Width="80px"
                                AutoPostBack="True" Text="0" />
                            <ajax:FilteredTextBoxExtender ID="ftbeLongestAgingCycle" runat="server" TargetControlID="txtLongestAgingCycle"
                                FilterType="Numbers" />
                            <asp:Label ID="lblLACdays" runat="server" Text="Label" Visible="False" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            TAG:
                        </td>
                        <td>
                            <asp:TextBox ID="txtTAG" runat="server" MaxLength="100" Width="300px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Start Date:
                        </td>
                        <td>
                            <asp:TextBox ID="txtStartDt" runat="server" MaxLength="10" Width="80px" />
                            <ajax:FilteredTextBoxExtender ID="rtbStartDt" runat="server" TargetControlID="txtStartDt"
                                FilterType="Custom, Numbers" ValidChars="/" />
                            &nbsp;<asp:ImageButton ID="imgStartDt" runat="server" AlternateText="Click to show calendar"
                                CausesValidation="False" Height="19px" ImageAlign="Middle" ImageUrl="~/images/AJAX/Calendar_scheduleHS.png"
                                Width="19px" />
                            <ajax:CalendarExtender ID="ceStartDt" runat="server" PopupButtonID="imgStartDt" TargetControlID="txtStartDt" />
                            <asp:RegularExpressionValidator ID="revStartDt" runat="server" ControlToValidate="txtStartDt"
                                ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format ' Font-Bold="True"
                                ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                Width="8px" ValidationGroup="vsAdmin"><</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Projected Completion Date:
                        </td>
                        <td>
                            <asp:TextBox ID="txtProjCmplDt" runat="server" MaxLength="10" Width="80px" />
                            <ajax:FilteredTextBoxExtender ID="ftbProjCmplDt" runat="server" TargetControlID="txtProjCmplDt"
                                FilterType="Custom, Numbers" ValidChars="/" />
                            <asp:ImageButton ID="imgProjCmplDt" runat="server" AlternateText="Click to show calendar"
                                CausesValidation="False" Height="19px" ImageAlign="Middle" ImageUrl="~/images/AJAX/Calendar_scheduleHS.png"
                                Width="19px" />
                            <ajax:CalendarExtender ID="ceProjCmplDt" runat="server" PopupButtonID="imgProjCmplDt"
                                TargetControlID="txtProjCmplDt" />
                            <asp:RegularExpressionValidator ID="revProjCmplDt" runat="server" ControlToValidate="txtProjCmplDt"
                                ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format ' Font-Bold="True"
                                ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                ValidationGroup="vsAdmin" Width="8px"><</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Actual Completion Date:
                        </td>
                        <td>
                            <asp:TextBox ID="txtActCmplDt" runat="server" MaxLength="10" Width="80px" />
                            <ajax:FilteredTextBoxExtender ID="ftbActCmplDt" runat="server" TargetControlID="txtActCmplDt"
                                FilterType="Custom, Numbers" ValidChars="/" />
                            <asp:ImageButton ID="imgActCmplDt" runat="server" AlternateText="Click to show calendar"
                                CausesValidation="False" Height="19px" ImageAlign="Middle" ImageUrl="~/images/AJAX/Calendar_scheduleHS.png"
                                Width="19px" />
                            <ajax:CalendarExtender ID="ceActCmplDt" runat="server" PopupButtonID="imgActCmplDt"
                                TargetControlID="txtActCmplDt" />
                            <asp:RegularExpressionValidator ID="revActCmplDt" runat="server" ControlToValidate="txtActCmplDt"
                                ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format ' Font-Bold="True"
                                ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                ValidationGroup="vsAdmin" Width="8px"><</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                        </td>
                        <td>
                            <br />
                            <asp:Button ID="btnSave2" runat="server" Text="Save" CausesValidation="true" ValidationGroup="vsAdmin" />
                            <asp:Button ID="btnReset2" runat="server" Text="Reset" CausesValidation="False" />
                            <asp:Button ID="btnResponse" runat="server" Text="Notify Issuer" />
                        </td>
                    </tr>
                </table>
                <asp:ValidationSummary ID="vsAdmin" runat="server" Font-Size="X-Small" ShowMessageBox="True"
                    ShowSummary="true" ValidationGroup="vsAdmin" />
                <br />
            </asp:View>
            <asp:View ID="vwAssignments" runat="server">
                <table width="80%">
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="gvTMAssignments" runat="server" AutoGenerateColumns="False" DataKeyNames="RequestID,TeamMemberID"
                                DataSourceID="odsTMAssignments" ShowFooter="True" OnRowDataBound="gvTMAssignments_RowDataBound"
                                OnRowCommand="gvTMAssignments_RowCommand" Width="50%" SkinID="StandardGrid">
                                <Columns>
                                    <asp:TemplateField HeaderText="Team Member" SortExpression="TeamMemberID">
                                        <EditItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("TMName") %>' />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("TMName") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddTeamMember" runat="server" DataSourceID="sdsGetTeamMember"
                                                DataTextField="TeamMemberName" DataValueField="TeamMemberID" AppendDataBoundItems="True">
                                                <asp:ListItem Value=" " Text=" " />
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="sdsGetTeamMember" runat="server" ConnectionString="<%$ ConnectionStrings:SQLConnection %>"
                                                SelectCommand="sp_Get_Team_Member" SelectCommandType="StoredProcedure" />
                                            <asp:ObjectDataSource ID="odsGetTeamMember" runat="server" />
                                            <asp:RequiredFieldValidator ID="rfvTeamMember" runat="server" ControlToValidate="ddTeamMember"
                                                ErrorMessage="Team Member is a required field." Font-Bold="False" ValidationGroup="InsertTMInfo"><</asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="EmailNotificationDate" HeaderText="Notification Date"
                                        SortExpression="EmailNotificationDate" />
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                ImageUrl="~/images/delete.jpg" AlternateText="Delete" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="ibtnInsert" runat="server" CausesValidation="True" CommandName="Insert"
                                                ImageUrl="~/images/save.jpg" AlternateText="Insert" ValidationGroup="InsertTMInfo" />&nbsp;&nbsp;&nbsp;
                                            <asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo" ImageUrl="~/images/undo-gray.jpg"
                                                AlternateText="Undo" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:ValidationSummary ID="vsTMAssignments" runat="server" ValidationGroup="InsertTMInfo"
                                ShowMessageBox="True" />
                            <asp:ObjectDataSource ID="odsTMAssignments" runat="server" DeleteMethod="DeleteTestIssuanceAssignments"
                                InsertMethod="InsertTestIssuanceAssignments" OldValuesParameterFormatString="original_{0}"
                                SelectMethod="GetTestIssuanceAssignments" TypeName="TestIssuanceAssignmentsBLL">
                                <DeleteParameters>
                                    <asp:QueryStringParameter Name="RequestID" QueryStringField="pReqID" Type="Int32" />
                                    <asp:Parameter Name="TeamMemberID" Type="Int32" />
                                    <asp:QueryStringParameter Name="original_RequestID" QueryStringField="pReqID" Type="Int32" />
                                    <asp:Parameter Name="original_TeamMemberID" Type="Int32" />
                                </DeleteParameters>
                                <SelectParameters>
                                    <asp:QueryStringParameter Name="RequestID" QueryStringField="pReqID" Type="Int32"
                                        DefaultValue="0" />
                                </SelectParameters>
                                <InsertParameters>
                                    <asp:QueryStringParameter Name="RequestID" QueryStringField="pReqID" Type="Int32" />
                                    <asp:Parameter Name="TeamMemberID" Type="Int32" />
                                </InsertParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15px">
                        </td>
                        <td>
                            <br />
                            <asp:Button ID="btnNotify" runat="server" Text="Send Notification" CausesValidation="False" />
                            <ajax:ConfirmButtonExtender ID="ceNotify" runat="server" TargetControlID="btnNotify"
                                ConfirmText="Are you sure you want to submit?" />
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vwTestReports" runat="server">
                <table>
                    <tr>
                        <td class="p_text" style="width: 161px">
                            Test Report #:
                        </td>
                        <td class="c_textbold" style="color: #990000">
                            <%If (ViewState("pRptID") = 0) Then%>
                            <i>Automated</i>
                            <%Else%>
                            <asp:Label ID="lblTestReportNo" runat="server" Width="200px"></asp:Label>
                            <% End If%>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" style="width: 161px">
                            <asp:Label ID="Label8" runat="server" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label>
                            Report Issuer:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddReportIssuer" runat="server">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvReportIssuer" runat="server" ControlToValidate="ddReportIssuer"
                                ErrorMessage="Report Issuer is a required field." Font-Bold="False" ValidationGroup="vsTestReport"><</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" style="width: 161px" valign="top">
                            <asp:Label ID="Label9" runat="server" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label>
                            Test Description:
                        </td>
                        <td>
                            <asp:TextBox ID="txtTestDesc" runat="server" Rows="5" TextMode="MultiLine" Width="500px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvTestDesc" runat="server" ControlToValidate="txtTestDesc"
                                ErrorMessage="Test Description is a required field." Font-Bold="False" ValidationGroup="vsTestReport"><</asp:RequiredFieldValidator><br />
                            <asp:Label ID="lblTestDesc" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" style="width: 161px" valign="top">
                            <asp:Label ID="Label10" runat="server" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label>
                            Assessment:
                        </td>
                        <td>
                            <asp:TextBox ID="txtAssessment" runat="server" Rows="5" TextMode="MultiLine" Width="500px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvAssessment" runat="server" ControlToValidate="txtAssessment"
                                ErrorMessage="Assessment is a required field." Font-Bold="False" ValidationGroup="vsTestReport"><</asp:RequiredFieldValidator><br />
                            <asp:Label ID="lblAssessment" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <%If (ViewState("pRptID") > 0) Then%>
                    <tr>
                        <td class="p_text" style="width: 161px; height: 26px" valign="top">
                            <asp:Label ID="Label11" runat="server" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label>
                            Test Report:
                        </td>
                        <td style="height: 26px">
                            <asp:FileUpload ID="uploadFile" runat="server" Height="22px" Width="600px" />
                            <asp:RequiredFieldValidator ID="rfvUpload" runat="server" ControlToValidate="uploadFile"
                                ErrorMessage="Test Report is required." Font-Bold="False" ValidationGroup="vsTestReport"><</asp:RequiredFieldValidator><br />
                            <asp:RegularExpressionValidator ID="revUploadFile" runat="server" ErrorMessage="Only *.PDF files are allowed!"
                                ValidationExpression="(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.pdf)$+(.PDF)$"
                                ControlToValidate="uploadFile" ValidationGroup="vsTestReport" Font-Bold="True"
                                Font-Size="Small" /><br />
                            <asp:Label ID="lblMessageView4" runat="server" Font-Bold="True" ForeColor="Red" Height="16px"
                                Text="Label" Visible="False" Width="368px" Font-Size="Small"></asp:Label>
                        </td>
                    </tr>
                    <%End If%>
                    <tr>
                        <td class="p_text" style="width: 161px; height: 27px;">
                        </td>
                        <td style="height: 27px">
                            <asp:Button ID="btnSaveTestRpt" runat="server" Text="Save" ValidationGroup="vsTestReport" />
                            <%If (HttpContext.Current.Request.QueryString("pRptID") <> Nothing) Or (HttpContext.Current.Request.QueryString("pRptID") <> "") Then%>
                            <asp:Button ID="btnUpload" runat="server" Text="Upload" CausesValidation="False" /><asp:Button
                                ID="btnReset3" runat="server" CausesValidation="False" Text="Reset" />
                            <asp:TextBox ID="txtFileName" runat="server" Visible="False" Width="32px"></asp:TextBox><asp:TextBox
                                ID="txtFilePathName" runat="server" Visible="False" Width="32px"></asp:TextBox><%End If%>
                        </td>
                    </tr>
                </table>
                <asp:ValidationSummary ID="vsTestReport" runat="server" ShowMessageBox="True" ValidationGroup="vsTestReport" />
                <br />
                <asp:GridView ID="gvTestReport" runat="server" AutoGenerateColumns="False" DataSourceID="odsTestReport"
                    DataKeyNames="TestReportID,RequestID" EmptyDataText="No data available for grid view. Use fields above to add new entry."
                    OnRowDataBound="gvTestReport_RowDataBound" Width="1000px" PageSize="100" SkinID="StandardGridWOFooter">
                    <Columns>
                        <asp:TemplateField HeaderText="Test Report #" InsertVisible="False" SortExpression="TestReportID">
                            <ItemTemplate>
                                <asp:HyperLink ID="lblTestReport" runat="server" Font-Underline="true" Text='<%# Bind("TestReportID") %>'
                                    NavigateUrl='<%# "TestIssuanceDetail.aspx?pReqID=" & DataBinder.Eval (Container.DataItem,"RequestID").tostring & "&pReqCategory=" & ViewState("pReqCategory") & "&pRptID=" & DataBinder.Eval (Container.DataItem,"TestReportID").tostring %>'></asp:HyperLink>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="TeamMemberName" HeaderText="Report Issuer" SortExpression="TeamMemberName">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="IssueDate" HeaderText="Issue Date" SortExpression="IssueDate">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TestDescription" HeaderText="Test Description" SortExpression="TestDescription">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Assessment" HeaderText="Assessment" SortExpression="Assessment">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="hlnkRptCoverSheet" ImageUrl="~/images/PreviewUp.jpg"
                                    NavigateUrl='<%# "crViewTestReportCoverSheet.aspx?pRptID=" & DataBinder.Eval (Container.DataItem,"TestReportID").tostring & "&pReqID=" & DataBinder.Eval (Container.DataItem,"RequestID").tostring & "&pReqCategory=" & Request.QueryString("pReqCategory") %>'
                                    Visible='<%# Bind("BinaryFound") %>' ToolTip="Preview Cover Sheet" />
                            </ItemTemplate>
                            <HeaderStyle Width="30px" />
                            <ItemStyle HorizontalAlign="center" Width="30px" />
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="hlnkPreview" ImageUrl='<%# DisplayImage(DataBinder.Eval(Container, "DataItem.EncodeType").tostring)  %>'
                                    NavigateUrl='<%# "TestReport.aspx?pRptID=" & DataBinder.Eval (Container.DataItem,"TestReportID").tostring & "&pReqID=" & DataBinder.Eval (Container.DataItem,"RequestID").tostring %>'
                                    Target="_blank" Visible='<%# Bind("BinaryFound") %>' ToolTip="Preview Test Report" />
                            </ItemTemplate>
                            <HeaderStyle Width="30px" />
                            <ItemStyle HorizontalAlign="center" Width="30px" />
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
                <asp:ObjectDataSource ID="odsTestReport" runat="server" DeleteMethod="DeleteTestIssuanceTestReport"
                    OldValuesParameterFormatString="original_{0}" SelectMethod="GetTestIssuanceTestReport"
                    TypeName="RDTestReportBLL">
                    <DeleteParameters>
                        <asp:ControlParameter ControlID="gvTestReport" Name="TestReportID" PropertyName="SelectedValue"
                            Type="Int32" />
                        <asp:QueryStringParameter Name="RequestID" QueryStringField="pReqID" Type="Int32" />
                        <asp:Parameter Name="original_TestReportID" Type="Int32" />
                        <asp:Parameter Name="original_RequestID" Type="Int32" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:QueryStringParameter Name="RequestID" QueryStringField="pReqID" Type="Int32" />
                        <asp:Parameter Name="TestReportID" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </asp:View>
        </asp:MultiView>
    </asp:Panel>
</asp:Content>
