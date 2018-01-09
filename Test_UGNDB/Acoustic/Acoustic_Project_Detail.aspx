<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Acoustic_Project_Detail.aspx.vb" Inherits="Acoustic_Project_Detail"
    MaintainScrollPositionOnPostback="True" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server">
        <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
            Visible="False"></asp:Label>
        <%If HttpContext.Current.Request.QueryString("pProjID") <> Nothing And HttpContext.Current.Request.QueryString("pProjID") <> "" Then%>
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
        <table>
            <tr>
                <td class="p_text">
                    Project No.:
                </td>
                <td class="c_textbold" style="color: #990000;">
                    <%If HttpContext.Current.Request.QueryString("pProjID") = Nothing Or HttpContext.Current.Request.QueryString("pProjID") = "" Then%>
                    <i>Automated</i>
                    <%Else%>
                    <asp:Label ID="lblProjectNo" runat="server" MaxLength="10" Width="200px" />
                    <% End If%>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="Label1" runat="server" Font-Size="Larger" ForeColor="red">*</asp:Label>
                    Test Description:
                </td>
                <td>
                    <asp:TextBox ID="txtTestDescription" runat="server" Width="300px" MaxLength="100" />
                    <asp:RequiredFieldValidator ID="rfvTestDesc" runat="server" ControlToValidate="txtTestDescription"
                        ErrorMessage="Test Description is a required field." Font-Bold="False"><</asp:RequiredFieldValidator>
                    <ajax:FilteredTextBoxExtender ID="ftbTestDescription" runat="server" TargetControlID="txtTestDescription"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,/-()., " />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Project Status:
                </td>
                <td>
                    <asp:DropDownList ID="ddProjectStatus" runat="server" AutoPostBack="True" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblRnDTestReq" runat="server" Text="R&D Test Request No:" />
                </td>
                <td class="c_text">
                    <asp:HyperLink ID="hlnkRDReqNo" runat="server" Text="" Target="_blank" />
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
            <tr style="background-color: white;">
                <td>
                    <asp:Menu ID="mnuTabs" runat="server" Orientation="Horizontal" StaticEnableDefaultPopOutImage="False"
                        StaticMenuItemStyle-CssClass="tab" StaticSelectedStyle-CssClass="selectedTab"
                        CssClass="tabs">
                        <Items>
                            <asp:MenuItem Text="Project Details" Value="0" />
                            <asp:MenuItem Text="Administrative" Value="1" />
                            <asp:MenuItem Text="Project Reports" Value="2" />
                        </Items>
                        <StaticMenuItemStyle CssClass="tab" />
                        <StaticSelectedStyle CssClass="selectedTab" />
                    </asp:Menu>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblMessage" runat="server" ForeColor="#ff0000" Visible="False" CssClass="p_text"
                        Height="24px" />
                    <asp:MultiView ID="mvTabs" runat="server" Visible="true" ActiveViewIndex="0" EnableViewState="true">
                        <asp:View ID="vProjectDetail" runat="server">
                            <table width="100%">
                                <tr>
                                    <td style="width: 750px">
                                        <table width="750px">
                                            <tr>
                                                <td class="p_text">
                                                    <asp:Label ID="lblRequester" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                                    Project Requester:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddRequester" runat="server" />
                                                    <asp:RequiredFieldValidator ID="rfvRequester" runat="server" ControlToValidate="ddRequester"
                                                        ErrorMessage="Project Requester is a required field." Font-Bold="False"><</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="p_text">
                                                    Date Requested:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDateRequested" runat="server" MaxLength="10" ReadOnly="True"
                                                        Width="80px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="p_text">
                                                    <asp:Label ID="Label5" runat="server" Font-Size="Larger" ForeColor="red" Text="* " />
                                                    Customer:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddCustomer" runat="server" />
                                                    <asp:RequiredFieldValidator ID="rfvCustomer" runat="server" ControlToValidate="ddCustomer"
                                                        ErrorMessage="Customer is a required field." Font-Bold="False"><</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="p_text" valign="top">
                                                    <asp:Label ID="Label4" runat="server" Font-Size="Larger" ForeColor="red" Text="* " />
                                                    Program:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddProgram" runat="server" />
                                                    <asp:RequiredFieldValidator ID="rfvProgram" runat="server" ControlToValidate="ddProgram"
                                                        ErrorMessage="Program is a required field." Font-Bold="False"><</asp:RequiredFieldValidator><br/>
                                                    {Program / Platform / Model / Assembly Plant}
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="p_text">
                                                    <asp:Label ID="Label12" runat="server" Font-Size="Larger" ForeColor="red" Text="* " />
                                                    Development Expense No.:
                                                </td>
                                                <td colspan="4">
                                                    <asp:TextBox ID="txtDevExp" runat="server" MaxLength="30" Width="200px" />
                                                    <asp:RequiredFieldValidator ID="rfvDevExpNo" runat="server" ControlToValidate="txtDevExp"
                                                        ErrorMessage="Development Expense No. is a required field." Font-Bold="False"><</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="p_text">
                                                    # of Test Samples:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNoOfTestSamples" runat="server" MaxLength="10" Width="80px" Text="0" />
                                                    <ajax:FilteredTextBoxExtender ID="ftbeNoOfTestSamples" runat="server" FilterType="Numbers"
                                                        TargetControlID="txtNoOfTestSamples" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="p_text">
                                                    <asp:Label ID="lblTestCmpltDt" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                                    Requested Completion Date:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTestCmpltDt" runat="server" MaxLength="10" Width="80px" />
                                                    &nbsp;<asp:ImageButton ID="imgTestCmpltDt" runat="server" AlternateText="Click to show calendar"
                                                        CausesValidation="False" Height="19px" ImageAlign="Middle" ImageUrl="~/images/AJAX/Calendar_scheduleHS.png"
                                                        Width="19px" />
                                                    <ajax:CalendarExtender ID="ceTestCmpltDt" runat="server" PopupButtonID="imgTestCmpltDt"
                                                        TargetControlID="txtTestCmpltDt" />
                                                    <asp:RequiredFieldValidator ID="rfvTestCmpltDt" runat="server" ControlToValidate="txtTestCmpltDt"
                                                        ErrorMessage="Requested Completion Date is a required field." Font-Bold="False"><</asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="revTestCmpltDt" runat="server" ControlToValidate="txtTestCmpltDt"
                                                        ErrorMessage="Invalid Date Entry:  use &quot;mm/dd/yyyy&quot; or &quot;m/d/yyyy&quot; format "
                                                        Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                                        Width="8px"><</asp:RegularExpressionValidator>
                                                    <asp:CompareValidator ID="cvTestCmpltDt" runat="server" ControlToCompare="txtDateRequested"
                                                        ControlToValidate="txtTestCmpltDt" ErrorMessage="Requested Completion Date must be greater than or equal to Current Date."
                                                        Operator="GreaterThanEqual" Type="Date"><</asp:CompareValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="p_text" valign="top">
                                                    <asp:Label ID="Label2" runat="server" Font-Size="Larger" ForeColor="red" Text="* " />
                                                    Sample Description:
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtSampleDesc" runat="server" Rows="5" TextMode="Multiline" Width="400px" />
                                                    <asp:RequiredFieldValidator ID="rfvSampleDesc" runat="server" ControlToValidate="txtSampleDesc"
                                                        ErrorMessage="Sample Description is a required field." Font-Bold="False"><</asp:RequiredFieldValidator>
                                                    <br />
                                                    <asp:Label ID="lblSampleDesc" runat="server" Font-Bold="True" ForeColor="Red" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="p_text" valign="top">
                                                    <asp:Label ID="Label7" runat="server" Font-Size="Larger" ForeColor="red" Text="* " />
                                                    Project Goals/Description:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtProjectGoals" runat="server" Rows="5" TextMode="Multiline" Width="400px" />
                                                    <asp:RequiredFieldValidator ID="rfvProjectGoals" runat="server" ControlToValidate="txtProjectGoals"
                                                        ErrorMessage="Project Goals/Description is a required field." Font-Bold="False"><</asp:RequiredFieldValidator>
                                                    <br />
                                                    <asp:Label ID="lblProjectGoals" runat="server" Font-Bold="True" ForeColor="Red" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="p_text" valign="top">
                                                    Background:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtBackground" runat="server" Rows="5" TextMode="Multiline" Width="400px" />
                                                    <br />
                                                    <asp:Label ID="lblBackground" runat="server" Font-Bold="True" ForeColor="Red" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="p_text">
                                                    Reporting Requirements:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddRptReq" runat="server">
                                                        <asp:ListItem Text=" " Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="Data Only" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Lab Test Report" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="Formal Report with Conclusions" Value="3"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="p_text" valign="top">
                                                    Special Instructions:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSpecialInst" runat="server" Rows="5" TextMode="Multiline" Width="400px" />
                                                    <br />
                                                    <asp:Label ID="lblSpecial" runat="server" Font-Bold="True" ForeColor="Red" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="p_text">
                                                    Submitted to Lab:
                                                </td>
                                                <td>
                                                    <asp:Label ID="txtSubmittedToLab" runat="server" Text="" CssClass="c_textbold" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td class="c_text" style="font-size: 14px; color: red;">
                                                    <br />
                                                    <asp:Label ID="lblComReq" runat="server" Text="A Commodity is required before submitting this lab request."
                                                        Font-Bold="true" Visible="false" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:GridView ID="gvCommodity" runat="server" AutoGenerateColumns="False" DataSourceID="odsCommodity"
                                                        DataKeyNames="ProjectID,CommodityID" OnRowDataBound="gvCommodity_RowDataBound"
                                                        OnRowCommand="gvCommodity_RowCommand" CellPadding="2" EmptyDataText="No data available for grid view. Use fields above to add new entry."
                                                        GridLines="Horizontal" Width="450px" PageSize="100" ShowFooter="True">
                                                        <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" Wrap="False" />
                                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                        <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                                        <EditRowStyle BackColor="#E2DED6" />
                                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                        <EmptyDataRowStyle Wrap="False" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Commodity" HeaderStyle-CssClass="c_text" SortExpression="CommodityName">
                                                                <ItemTemplate>
                                                                    <asp:Label CssClass="c_text" ID="lblCommodity" runat="server" Text='<%# Bind("CommodityName") %>' />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:DropDownList ID="ddCommodity" runat="server" DataSource='<%# commonFunctions.GetCommodity(0,"","",0) %>'
                                                                        DataValueField="CommodityID" DataTextField="ddCommodityByClassification" SelectedValue='<%# Bind("CommodityID") %>'
                                                                        AppendDataBoundItems="True">
                                                                        <asp:ListItem Selected="True" Value="" Text="">
                                                                        </asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="rfvCommodity" runat="server" ControlToValidate="ddCommodity"
                                                                        ErrorMessage="Commodity is a required field." Font-Bold="False" ValidationGroup="vInsertCommodity"><</asp:RequiredFieldValidator>
                                                                    <br>
                                                                    <asp:Label ID="Label6" runat="server" ForeColor="Black" Font-Size="Smaller" Text="{Commodity / Classification}" />
                                                                </FooterTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ShowHeader="False">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                                        ImageUrl="~/images/delete.jpg" Text="Delete" />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:ImageButton ID="ibtnInsert" runat="server" CausesValidation="True" CommandName="Insert"
                                                                        ImageUrl="~/images/save.jpg" Text="Insert" AlternateText="Insert" ValidationGroup="vInsertCommodity" />&nbsp;&nbsp;&nbsp;
                                                                    <asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo" ImageUrl="~/images/undo-gray.jpg"
                                                                        Text="Undo" AlternateText="Undo" />
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <asp:ValidationSummary ID="vsCommodity" runat="server" ShowMessageBox="true" ValidationGroup="vInsertCommodity" />
                                                    <asp:SqlDataSource ID="dsCommodityMaint" runat="server" ConnectionString="<%$ ConnectionStrings:SQLConnection %>"
                                                        SelectCommand="sp_Get_Commodity" SelectCommandType="StoredProcedure">
                                                        <SelectParameters>
                                                            <asp:Parameter Name="commodityName" Type="String" />
                                                        </SelectParameters>
                                                    </asp:SqlDataSource>
                                                    <asp:ObjectDataSource ID="odsCommodity" runat="server" DeleteMethod="DeleteAcousticProjectCommodities"
                                                        InsertMethod="InsertAcousticProjectCommodities" OldValuesParameterFormatString="original_{0}"
                                                        SelectMethod="GetAcousticProjectCommodities" TypeName="AcousticBLL">
                                                        <DeleteParameters>
                                                            <asp:QueryStringParameter Name="ProjectID" QueryStringField="pProjID" Type="Int32" />
                                                            <asp:Parameter Name="CommodityID" Type="Int32" />
                                                            <asp:QueryStringParameter Name="original_ProjectID" Type="Int32" QueryStringField="pProjID" />
                                                            <asp:Parameter Name="original_CommodityID" Type="Int32" />
                                                        </DeleteParameters>
                                                        <SelectParameters>
                                                            <asp:QueryStringParameter Name="ProjectID" QueryStringField="pProjID" Type="Int32" />
                                                        </SelectParameters>
                                                        <InsertParameters>
                                                            <asp:QueryStringParameter Name="ProjectID" QueryStringField="pProjID" Type="Int32" />
                                                            <asp:Parameter Name="CommodityID" Type="Int32" />
                                                        </InsertParameters>
                                                    </asp:ObjectDataSource>
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 193px">
                                                </td>
                                                <td align="left" colspan="2" style="height: 26px">
                                                    &nbsp;<asp:Button ID="btnSave1" runat="server" CommandName="save" Text="Save" Width="100" />
                                                    &nbsp;
                                                    <asp:Button ID="btnReset1" runat="server" CommandName="resetview1" Text="Reset" Width="100"
                                                        CausesValidation="False" />
                                                    &nbsp;
                                                    <asp:Button ID="btnDelete" runat="server" CommandName="delete" Text="Delete" Width="100"
                                                        CausesValidation="False" />
                                                    <ajax:ConfirmButtonExtender ID="ceDelete" runat="server" ConfirmText="Are you sure you want to delete this record?"
                                                        TargetControlID="btnDelete">
                                                    </ajax:ConfirmButtonExtender>
                                                    &nbsp;
                                                    <%If HttpContext.Current.Request.QueryString("pProjID") <> "" Then%>
                                                    <asp:Button ID="btnSubmit1" runat="server" CausesValidation="true" Text="Submit Request >>"
                                                        Width="125px" />
                                                    <ajax:ConfirmButtonExtender ID="ceSubmit1" runat="server" ConfirmText="Are you sure you want to submit this Acoustic Lab Request?"
                                                        TargetControlID="btnSubmit1">
                                                    </ajax:ConfirmButtonExtender>
                                                    &nbsp;
                                                    <%End If%>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top">
                                        <table width="60%">
                                            <tr>
                                                <td class="c_text" style="border-right: gray thin double; border-top: gray thin double;
                                                    border-left: gray thin double; border-bottom: gray thin double">
                                                    <br />
                                                    &nbsp;&nbsp;&nbsp;<b><u>Standard Test Sample Sizes</u></b>
                                                    <ul>
                                                        Cabin -- 1.2 x 1.0 meters</ul>
                                                    <ul>
                                                        APAMAT -- 33 x 33 Inches</ul>
                                                    <ul>
                                                        Sound Transmission Loss -- 33 x 33 Inches</ul>
                                                    <ul>
                                                        Impedance Tube -- 12 x 12 Inches</ul>
                                                    <ul>
                                                        NorsonicAFR -- 12 x 12 Inches</ul>
                                                    &nbsp;&nbsp;&nbsp;* Test samples should be larger than what is listed above
                                                    <br />
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;so they can be cut to appropriate size.
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:ValidationSummary ID="vsAcoustic" runat="server" Font-Size="Small" ShowMessageBox="True"
                                            ShowSummary="true" />
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="vAdmin" runat="server">
                            <table width="900">
                                <tr>
                                    <td colspan="6" style="font-weight: bold">
                                        <asp:Label ID="Label3" runat="server" ForeColor="#ff0000" Visible="False" CssClass="p_text"
                                            Height="24px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        Responsible Engineer:
                                    </td>
                                    <td colspan="4">
                                        <asp:DropDownList ID="ddEngineer" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        Responsible Technician:
                                    </td>
                                    <td colspan="4">
                                        <asp:DropDownList ID="ddTechnician" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        Autoneum Reference No:
                                    </td>
                                    <td colspan="4">
                                        <asp:TextBox ID="txtReiterRefNo" runat="server" Width="100" MaxLength="10" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        Estimated Cost:
                                    </td>
                                    <td colspan="4">
                                        <asp:TextBox ID="txtEstCost" runat="server" Width="100" Text="0.00" />
                                        <asp:RangeValidator ID="rvEstCost" runat="server" ControlToValidate="txtEstCost"
                                            ErrorMessage="Estimated Cost requires a numeric value -999999.99 to 999999.99"
                                            MaximumValue="999999.99" MinimumValue="-999999.99" Height="16px" Display="Dynamic"
                                            Type="Currency"><</asp:RangeValidator>
                                        <ajax:FilteredTextBoxExtender ID="fEstCost" runat="server" TargetControlID="txtEstCost"
                                            FilterType="Custom, Numbers" ValidChars="-." />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        Actual Cost:
                                    </td>
                                    <td colspan="4">
                                        <asp:TextBox ID="txtActualCost" runat="server" Width="100" Text="0.00" />
                                        <asp:RangeValidator ID="rvActualCost" runat="server" ControlToValidate="txtActualCost"
                                            ErrorMessage="Actual Cost requires a numeric value -999999.99 to 999999.99" MaximumValue="999999.99"
                                            MinimumValue="-999999.99" Height="16px" Display="Dynamic" Type="Currency"><</asp:RangeValidator>
                                        <ajax:FilteredTextBoxExtender ID="fActualCost" runat="server" TargetControlID="txtActualCost"
                                            FilterType="Custom, Numbers" ValidChars="-." />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        Project Initiation Date:
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtProjIntDt" runat="server" MaxLength="12" Width="80px" />
                                        <asp:ImageButton runat="server" ID="imgInitDt" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                            AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                                            CausesValidation="False" />
                                        <ajax:CalendarExtender ID="ceInitDt" runat="server" TargetControlID="txtProjIntDt"
                                            PopupButtonID="imgInitDt" />
                                        <asp:RegularExpressionValidator ID="revInitDt" runat="server" ControlToValidate="txtProjIntDt"
                                            ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format ' Font-Bold="True"
                                            ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                            Width="8px"><</asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        Estimated Completion Date:
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtEstCmpltDt" runat="server" MaxLength="12" Width="80px" />
                                        <asp:ImageButton runat="server" ID="imgExpComplDt" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                            AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                                            CausesValidation="False" />
                                        <ajax:CalendarExtender ID="ceExpComplDt" runat="server" TargetControlID="txtEstCmpltDt"
                                            PopupButtonID="imgExpComplDt" />
                                        <asp:RegularExpressionValidator ID="revExpComplDt" runat="server" ControlToValidate="txtEstCmpltDt"
                                            ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format ' Font-Bold="True"
                                            ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                            Width="8px"><</asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        Actual Completion Date:
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtActualCmplDt" runat="server" MaxLength="12" Width="80px" />
                                        <asp:ImageButton runat="server" ID="imgActualCmplDt" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                            AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                                            CausesValidation="False" />
                                        <ajax:CalendarExtender ID="ceActualCmplDt" runat="server" TargetControlID="txtActualCmplDt"
                                            PopupButtonID="imgActualCmplDt" />
                                        <asp:RegularExpressionValidator ID="revActualCmplDt" runat="server" ControlToValidate="txtActualCmplDt"
                                            ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format ' Font-Bold="True"
                                            ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                            Width="8px"><</asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        Status Notes:
                                    </td>
                                    <td style="width: 193px">
                                        <asp:TextBox ID="txtStatusNotes" runat="server" Width="300px" MaxLength="100" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text" valign="top">
                                        Additional Instructions:
                                    </td>
                                    <td colspan="4">
                                        <asp:TextBox ID="txtAddInstructions" TextMode="MultiLine" Width="400px" runat="Server"
                                            Rows="5" /><br />
                                        <asp:Label ID="lblInstructions" runat="server" Font-Bold="True" ForeColor="Red" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 158px">
                                    </td>
                                    <td colspan="4">
                                        <asp:Button ID="btnSave2" runat="server" Width="100" Text="Save" CommandName="save" />
                                        <asp:Button ID="btnReset2" runat="server" Width="100" Text="Reset" CommandName="resetview3" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table width="900px">
                                <tr>
                                    <td colspan="2" class="c_text" style="color: #990000; font-weight: bold">
                                        Use this section to add additional comments to submit back to the Requester.<br />
                                        <font style="font-size: smaller">(This will be used only in the body of the email.)</font>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text" style="vertical-align: top;">
                                        Comments:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtComments" runat="server" MaxLength="500" Rows="4" TextMode="MultiLine"
                                            Width="500px" /><br />
                                        <asp:Label ID="lblComments" runat="server" Font-Bold="True" ForeColor="Red" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnNotify" runat="server" Text="Notify Requester &gt;&gt;" Width="145px" />
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="vProjectReports" runat="server">
                            <table>
                                <tr>
                                    <td class="p_text" style="width: 161px">
                                        <asp:Label ID="Label8" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                        Report Issuer:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddReportIssuer" runat="server" />
                                        <asp:RequiredFieldValidator ID="rfvReportIssuer" runat="server" ControlToValidate="ddReportIssuer"
                                            ErrorMessage="Report Issuer is a required field." Font-Bold="False" ValidationGroup="vsProjectReport"><</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text" style="width: 161px" valign="top">
                                        <asp:Label ID="Label9" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                        Report Description:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRptDesc" runat="server" Rows="5" TextMode="MultiLine" Width="500px" />
                                        <asp:RequiredFieldValidator ID="rfvRptDesc" runat="server" ControlToValidate="txtRptDesc"
                                            ErrorMessage="Report Description is a required field." Font-Bold="False" ValidationGroup="vsProjectReport"><</asp:RequiredFieldValidator><br />
                                        <asp:Label ID="lblRptDesc" runat="server" Font-Bold="True" ForeColor="Red" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text" style="width: 161px; height: 26px" valign="top">
                                        <asp:Label ID="Label11" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                        Upload PDF Report:
                                    </td>
                                    <td style="height: 26px">
                                        <asp:FileUpload ID="uploadFile" runat="server" Height="22px" Width="600px" />
                                        <asp:RequiredFieldValidator ID="rfvUpload" runat="server" ControlToValidate="uploadFile"
                                            ErrorMessage="Report is required." Font-Bold="False" ValidationGroup="vsProjectReport"><</asp:RequiredFieldValidator><br />
                                        <asp:RegularExpressionValidator ID="revUploadFile" runat="server" ErrorMessage="Only *.PDF files are allowed!"
                                            ValidationExpression="(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.pdf)$" ControlToValidate="uploadFile"
                                            ValidationGroup="vsProjectReport" Font-Bold="True" Font-Size="Small" /><br />
                                        <asp:Label ID="lblMessageView4" runat="server" Font-Bold="True" ForeColor="Red" Height="16px"
                                            Text="Label" Visible="False" Width="368px" Font-Size="Small" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text" style="width: 161px; height: 27px;">
                                    </td>
                                    <td style="height: 27px">
                                        <asp:Button ID="btnUpload" runat="server" Text="Upload" CausesValidation="False" />
                                        <asp:Button ID="btnReset3" runat="server" CausesValidation="False" Text="Reset" />
                                    </td>
                                </tr>
                            </table>
                            <asp:ValidationSummary ID="vsProjectReport" runat="server" ShowMessageBox="True"
                                ValidationGroup="vsProjectReport" />
                            <br />
                            <asp:GridView ID="gvProjectReport" runat="server" AutoGenerateColumns="False" DataSourceID="odsReport"
                                DataKeyNames="ReportID,ProjectID" EmptyDataText="No data available for grid view. Use fields above to add new entry."
                                OnRowDataBound="gvProjectReport_RowDataBound" OnRowCommand="gvProjectReport_RowCommand"
                                Width="900px" PageSize="100" SkinID="StandardGridWOFooter">
                                <Columns>
                                    <asp:BoundField DataField="ReportID" HeaderText="Report No." SortExpression="ReportID"
                                        HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="left">
                                        <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TeamMemberName" HeaderText="Report Issuer" SortExpression="TeamMemberName"
                                        HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="left">
                                        <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="IssueDate" HeaderText="Issue Date" SortExpression="IssueDate"
                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                                        <HeaderStyle Width="80px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ReportDescription" HeaderText="Report Description" SortExpression="ReportDescription"
                                        HeaderStyle-HorizontalAlign="left">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:HyperLink runat="server" ID="hlnkPreview" ImageUrl='<%# DisplayImage(DataBinder.Eval(Container, "DataItem.EncodeType").tostring)  %>'
                                                NavigateUrl='<%# "ProjectReport.aspx?pRptID=" & DataBinder.Eval (Container.DataItem,"ReportID").tostring & "&pProjID=" & DataBinder.Eval (Container.DataItem,"ProjectID").tostring %>'
                                                Target="_blank" Visible='<%# Bind("BinaryFound") %>' Text="Preview" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="30px" />
                                        <ItemStyle HorizontalAlign="center" Width="30px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                ImageUrl="~/images/delete.jpg" Text="Delete" AlternateText="Delete" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="30px" />
                                        <ItemStyle HorizontalAlign="center" Width="30px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:ObjectDataSource ID="odsReport" runat="server" DeleteMethod="DeleteAcousticProjectReport"
                                OldValuesParameterFormatString="original_{0}" SelectMethod="GetAcousticProjectReport"
                                TypeName="AcousticReportBLL">
                                <DeleteParameters>
                                    <asp:Parameter Name="ReportID" Type="Int32" />
                                    <asp:Parameter Name="ProjectID" Type="Int32" />
                                    <asp:Parameter Name="original_ReportID" Type="Int32" />
                                    <asp:Parameter Name="original_ProjectID" Type="Int32" />
                                </DeleteParameters>
                                <SelectParameters>
                                    <asp:QueryStringParameter DefaultValue="" Name="ProjectID" QueryStringField="pProjID"
                                        Type="Int32" />
                                    <asp:Parameter Name="ReportID" Type="Int32" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </asp:View>
                    </asp:MultiView>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
