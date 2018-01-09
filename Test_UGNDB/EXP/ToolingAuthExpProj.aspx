<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="ToolingAuthExpProj.aspx.vb" Inherits="ToolingAuthExpProj" MaintainScrollPositionOnPostback="true"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSave" Width="1150px">
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin" />
        <asp:ValidationSummary runat="server" ID="vsSave" ValidationGroup="vgSave" ShowMessageBox="true"
            ShowSummary="true" />
        <table width="98%">
            <tr>
                <td align="center" colspan="2">
                    <asp:Button ID="btnSave" runat="server" Text="Save" Visible="false" ValidationGroup="vgSave" />
                    <asp:Button ID="btnPreviewTA" runat="server" Text="Preview TA" Visible="false" CausesValidation="false" />
                    <asp:Button ID="btnPreviewDieshop" runat="server" Text="Preview Dieshop Cost Form"
                        Visible="false" CausesValidation="false" />
                </td>
            </tr>
        </table>
        <table width="48%">
            <tr>
                <td class="p_text" style="white-space: nowrap;" align="left">
                    TA Project No:
                </td>
                <td style="color: #990000;">
                    <asp:Label runat="server" ID="lblTAProjectNo" CssClass="c_text" Font-Bold="True"
                        Font-Overline="False" Font-Size="Larger"></asp:Label>
                    <asp:Label runat="server" ID="lblTANo" CssClass="none" Text="0"></asp:Label>
                </td>
                <td class="p_text">
                    Status:
                </td>
                <td class="c_text">
                    <asp:DropDownList runat="server" ID="ddStatus" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text" style="white-space: nowrap;" align="right">
                    Issue Date:
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblIssueDate" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="p_text" style="white-space: nowrap">
                    <asp:Label ID="lblVoidCommentMarker" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="* " Visible="false" />
                    <asp:Label runat="server" ID="lblVoidComment" Text="Void Comments:" Visible="false"
                        SkinID="MessageLabelSkin"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtVoidComment" runat="server" Height="60px" TextMode="MultiLine"
                        Width="300px" MaxLength="150" Visible="false"></asp:TextBox>
                    <br />
                    <asp:Label ID="lblVoidCommentCharCount" SkinID="MessageLabelSkin" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <table width="100%" border="0">
            <tr>
                <td style="width: 30px">
                    <asp:Menu ID="menuTabs" Height="30px" runat="server" Orientation="Horizontal" StaticEnableDefaultPopOutImage="false"
                        StaticMenuItemStyle-CssClass="tab" StaticSelectedStyle-CssClass="selectedTab"
                        CssClass="tabs">
                        <Items>
                            <asp:MenuItem Text="Project Detail" Value="0" ImageUrl="" Selected="true" />
                            <asp:MenuItem Text="Customer / Part Info" Value="1" ImageUrl="" />
                            <asp:MenuItem Text="Team Member Tasks" Value="2" ImageUrl="" />
                            <asp:MenuItem Text="Dieshop" Value="3" ImageUrl="" />
                            <asp:MenuItem Text="Supporting Documents" Value="4" ImageUrl="" />
                            <asp:MenuItem Text="Communication Board" Value="5" ImageUrl="" />
                        </Items>
                    </asp:Menu>
                </td>
            </tr>
        </table>
        <asp:MultiView ID="mvTabs" runat="server" Visible="true" ActiveViewIndex="0" EnableViewState="true">
            <asp:View ID="vwProjectDetail" runat="server">
                <table width="98%">
                    <tr>
                        <td class="p_text">
                            RFD No:
                        </td>
                        <td class="c_textbold">
                            <asp:TextBox runat="server" ID="txtRFDNo" MaxLength="6"></asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="ftRFDNo" runat="server" TargetControlID="txtRFDNo"
                                FilterType="Numbers" />
                            <asp:ImageButton ID="iBtnGetRFDinfo" runat="server" ImageUrl="~/images/SelectUser.gif"
                                ToolTip="Click here to pull part information from an RFD." />
                            <asp:HyperLink runat="server" ID="hlnkRFD" Visible="false" Font-Underline="true"
                                ToolTip="Click here to view the RFD" Text="View RFD" Target="_blank"></asp:HyperLink>
                        </td>
                        <td class="p_text" style="white-space: nowrap;">
                            Cost Sheet:
                        </td>
                        <td class="c_textbold" style="white-space: nowrap;">
                            <asp:TextBox ID="txtCostSheetID" runat="server" MaxLength="6"></asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="ftCostSheetID" runat="server" TargetControlID="txtCostSheetID"
                                FilterType="Numbers" />
                            <asp:HyperLink runat="server" ID="hlnkCostSheet" Font-Underline="true" ToolTip="Click here to see Cost Sheet."
                                Target="_blank" Text="View Cost Sheet" Visible="false"></asp:HyperLink>&nbsp;
                            &nbsp;
                            <asp:HyperLink runat="server" ID="hlnkDieLayout" Font-Underline="true" ToolTip="Click here to see Die Layout."
                                Target="_blank" Text="View Die Layout" Visible="false"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            Type of Change:
                        </td>
                        <td class="c_textbold" style="white-space: nowrap;">
                            <asp:DropDownList ID="ddChangeType" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td class="p_text">
                            <asp:Label ID="lblUGNFacilityMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="* " />
                            UGN Facility:<br />
                            <i>(Final Destination)</i>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddUGNFacility" runat="server">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvUGNFacility" runat="server" ControlToValidate="ddUGNFacility"
                                ErrorMessage="UGN Facility is required." Font-Bold="True" ValidationGroup="vgSave"
                                Text="<" SetFocusOnError="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" style="white-space: nowrap;">
                            <asp:Label ID="lblDueDateMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="* " />
                            Due Date:
                        </td>
                        <td class="c_textbold" style="white-space: nowrap;">
                            <asp:TextBox ID="txtDueDate" runat="server" MaxLength="10"></asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="ftbeDueDate" runat="server" TargetControlID="txtDueDate"
                                FilterType="Custom, Numbers" ValidChars="/" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvDueDate" ControlToValidate="txtDueDate"
                                SetFocusOnError="true" ErrorMessage="Due date is required" Text="<" ValidationGroup="vgSave" />
                            <asp:ImageButton runat="server" ID="imgDueDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                            <ajax:CalendarExtender ID="ceDueDate" runat="server" TargetControlID="txtDueDate"
                                PopupButtonID="imgDueDate" />
                            <asp:RegularExpressionValidator ID="revDueDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                ControlToValidate="txtDueDate" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                Width="8px" ValidationGroup="vgSave" Text="<"></asp:RegularExpressionValidator>
                            <asp:RangeValidator ID="rvDueDate" runat="server" Font-Bold="True" Type="Date" ToolTip="The date must be between 1950 and 2100"
                                ErrorMessage="Invalid Date Entry: The date must be between 1950 and 2100" Text="<"
                                ValidationGroup="vgSave" MaximumValue="01/01/2100" MinimumValue="01/01/1950"
                                ControlToValidate="txtDueDate"></asp:RangeValidator>
                        </td>
                        <td class="p_text" style="white-space: nowrap;">
                            <asp:Label ID="lblImplementationDateMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="* " />
                            Implementation Date:
                        </td>
                        <td class="c_textbold" style="white-space: nowrap;">
                            <asp:TextBox ID="txtImplementationDate" runat="server" MaxLength="10"></asp:TextBox>
                            <ajax:FilteredTextBoxExtender ID="ftbeImplementationDate" runat="server" TargetControlID="txtImplementationDate"
                                FilterType="Custom, Numbers" ValidChars="/" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvImplementationDate" ControlToValidate="txtImplementationDate"
                                SetFocusOnError="true" ErrorMessage="Implementation date is required" Text="<"
                                ValidationGroup="vgSave" />
                            <asp:ImageButton runat="server" ID="imgImplementationDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                            <ajax:CalendarExtender ID="ceImplementationDate" runat="server" TargetControlID="txtImplementationDate"
                                PopupButtonID="imgImplementationDate" />
                            <asp:RegularExpressionValidator ID="revImplementationDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                ControlToValidate="txtImplementationDate" Font-Bold="True" ToolTip="MM/DD/YYYY"
                                ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                Width="8px" ValidationGroup="vgSave" Text="<"></asp:RegularExpressionValidator>
                            <asp:RangeValidator ID="rvImplementationDate" runat="server" Font-Bold="True" Type="Date"
                                ToolTip="The date must be between 1950 and 2100" ErrorMessage="Invalid Date Entry: The date must be between 1950 and 2100"
                                Text="<" ValidationGroup="vgSave" MaximumValue="01/01/2100" MinimumValue="01/01/1950"
                                ControlToValidate="txtImplementationDate"></asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblInitiatorTeamMemberMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="* " />
                            Initiator:
                        </td>
                        <td class="c_textbold">
                            <asp:DropDownList ID="ddInitiatorTeamMember" runat="server">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="rfInitiatorTeamMember" ControlToValidate="ddInitiatorTeamMember"
                                SetFocusOnError="true" ErrorMessage="Initiator is required" Text="<" ValidationGroup="vgSave" />
                        </td>
                        <td class="p_text">
                            <asp:Label ID="lblQualityEngineerMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="* " />
                            Quality Engineer:
                        </td>
                        <td class="c_textbold">
                            <asp:DropDownList ID="ddQualityEngineer" runat="server">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="rfvQualityEngineer" ControlToValidate="ddQualityEngineer"
                                SetFocusOnError="true" ErrorMessage="Quality Engineer is required" Text="<" ValidationGroup="vgSave" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblProgramManagerMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="* " />
                            Program Manager:
                        </td>
                        <td class="c_textbold">
                            <asp:DropDownList ID="ddProgramManager" runat="server">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="rfvProgramManager" ControlToValidate="ddProgramManager"
                                SetFocusOnError="true" ErrorMessage="Program Manager is required" Text="<" ValidationGroup="vgSave" />
                        </td>
                        <td class="p_text">
                            Account Manager:
                        </td>
                        <td class="c_textbold">
                            <asp:DropDownList ID="ddAccountManager" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblChangeDescriptionMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="* " />
                            Desc:
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtTADesc" runat="server" TextMode="MultiLine" Height="90px" Width="600px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvTADesc" runat="server" ControlToValidate="txtTADesc"
                                ErrorMessage="Description is required." Font-Bold="True" ValidationGroup="vgSave"
                                Text="<" SetFocusOnError="true" />
                            <br />
                            <asp:Label ID="lblTADescCharCount" SkinID="MessageLabelSkin" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:CheckBox runat="server" ID="cbCharge" Text="Charge To Customer" Checked="true"
                                AutoPostBack="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Charge To <b>(IF NOT CUSTOMER)</b>:
                        </td>
                        <td>
                            <asp:TextBox ID="txtChargeOther" runat="server" MaxLength="50" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                            <asp:Button ID="btnSaveBottom" runat="server" Text="Save" Visible="false" ValidationGroup="vgSave" />
                            <asp:Button ID="btnPreviewTABottom" runat="server" Text="Preview TA" Visible="false"
                                CausesValidation="false" />
                            <asp:Button ID="btnPreviewDieshopBottom" runat="server" Text="Preview Dieshop Cost Form"
                                Visible="false" CausesValidation="false" />
                            <asp:Button ID="btnCopyBottom" runat="server" Text="Copy" Visible="false" CausesValidation="false" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Label ID="lblMessageSaveBottom" runat="server" SkinID="MessageLabelSkin"></asp:Label>
            </asp:View>
            <asp:View ID="vwCustomerPart" runat="server">
                <asp:ValidationSummary ID="vsCustomer" runat="server" Font-Size="X-Small" ShowMessageBox="True"
                    ShowSummary="true" ValidationGroup="vgCustomer" />
                <table runat="server" id="tblCustomerProgramEdit" visible="false">
                    <tr>
                        <td class="p_text" style="width: 130px">
                            Make:
                        </td>
                        <td style="font-size: smaller">
                            <asp:DropDownList ID="ddMakes" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" style="width: 130px">
                            Model:
                        </td>
                        <td style="font-size: smaller">
                            <asp:DropDownList ID="ddModel" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" valign="top" style="width: 130px">
                            <asp:Label ID="lblProgramMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="* " />&nbsp;Program:
                        </td>
                        <td style="font-size: smaller">
                            <asp:DropDownList ID="ddProgram" runat="server" />
                            &nbsp;<asp:RequiredFieldValidator ID="rfvProgram" runat="server" ControlToValidate="ddProgram"
                                ErrorMessage="Program is a required field." Font-Bold="False" ValidationGroup="vgCustomer">&lt;</asp:RequiredFieldValidator><asp:ImageButton
                                    ID="iBtnPreviewDetail" runat="server" ImageUrl="~/images/PreviewUp.jpg" ToolTip="Review Program Detail"
                                    Visible="false" />
                            <br />
                            {Program / Platform / Assembly Plant}
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblYearMarker" runat="server" Font-Bold="True" ForeColor="Red" Text="* "
                                Visible="false" />
                            <asp:Label runat="server" ID="lblYear" Text="Year:"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddYear" runat="server">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvYear" runat="server" ControlToValidate="ddYear"
                                ErrorMessage="Year is required." Font-Bold="True" ValidationGroup="vgCustomer"
                                Text="<" SetFocusOnError="true" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Button runat="Server" ID="btnAddToCustomerProgram" Text="Add Program / Customer"
                                ValidationGroup="vgCustomer" />
                        </td>
                    </tr>
                </table>
                <asp:ValidationSummary ID="vsCustomerProgram" runat="server" DisplayMode="List" ShowMessageBox="true"
                    ShowSummary="true" EnableClientScript="true" ValidationGroup="vgCustomerProgram" />
                <asp:Label runat="server" ID="lblMessageCustomerProgram" SkinID="MessageLabelSkin"></asp:Label>
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
                    EmptyDataText="No Programs or Customers found" Width="800px">
                    <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#CCCCCC" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="iBtnCustomerProgramDelete" runat="server" CausesValidation="False"
                                    CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ddCustomerDesc" HeaderText="Customer" SortExpression="ddCustomerDesc"
                            ReadOnly="True" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="ProgramYear" HeaderText="Year" SortExpression="ProgramYear"
                            ReadOnly="True" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="ddProgramName" HeaderText="Program / Make / Model / Platform / Assembly Plant"
                            SortExpression="ddProgramName" ReadOnly="True" HeaderStyle-HorizontalAlign="Left" />
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsCustomerProgram" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetTACustomerProgram" TypeName="TAModule" DeleteMethod="DeleteTACustomerProgram">
                    <DeleteParameters>
                        <asp:Parameter Name="RowID" Type="Int32" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:ControlParameter ControlID="lblTANo" DefaultValue="0" Name="TANo" PropertyName="Text"
                            Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <br />
                <table runat="server" id="tblFinishedPart" visible="false" width="98%">
                    <tr>
                        <td class="p_text">
                            Internal Finished Good Part No:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtNewInternalPartNo" MaxLength="18" />
                            <asp:RequiredFieldValidator ID="rfvNewInternalPartNo" runat="server" ControlToValidate="txtNewInternalPartNo"
                                ErrorMessage="Internal Finished PartNo is a required field." Font-Bold="False"
                                ValidationGroup="vgFinishedPart">&lt;</asp:RequiredFieldValidator>
                            <asp:ImageButton ID="iBtnNewInternalPartNo" runat="server" ImageUrl="~/images/Search.gif"
                                ToolTip="Click here to search for a part number." />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Design Level:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtNewDesignLevel" MaxLength="25" />
                            <asp:RequiredFieldValidator ID="rfvNewDesignLevel" runat="server" ControlToValidate="txtNewDesignLevel"
                                ErrorMessage="Design Level is a required field." Font-Bold="False" ValidationGroup="vgFinishedPart"><</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            DMS Drawing No:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtNewInternalDrawingNo" MaxLength="18"></asp:TextBox>
                            <asp:ImageButton ID="iBtnNewInternalDrawingNoSearch" runat="server" ImageUrl="~/images/Search.gif"
                                ToolTip="Click here to search for a DMS Drawing." />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Button runat="Server" ID="btnAddFinishedPart" Text="Add Finished Part" ValidationGroup="vgFinishedPart" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:ValidationSummary ID="vsFinishedPart" runat="server" DisplayMode="List" ShowMessageBox="true"
                    ShowSummary="true" EnableClientScript="true" ValidationGroup="vgFinishedPart" />
                <asp:Label runat="server" ID="lblMessageFinishedPart" SkinID="MessageLabelSkin"></asp:Label>
                <br />
                <asp:GridView ID="gvFinishedPart" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                    AllowPaging="True" PageSize="15" DataKeyNames="RowID" DataSourceID="odsFinishedPart"
                    ShowFooter="false" Width="800px">
                    <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#CCCCCC" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField ShowHeader="False">
                            <EditItemTemplate>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="iBtnFinishedPartDelete" runat="server" CausesValidation="False"
                                    CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CurrentCustomerPartNo" SortExpression="CurrentCustomerPartNo"
                            HeaderText="Current Customer PartNo" ReadOnly="True" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="NewCustomerPartNo" SortExpression="NewCustomerPartNo"
                            HeaderText="New Customer Part No" ReadOnly="True" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="NewInternalPartNo" SortExpression="NewInternalPartNo"
                            HeaderText="Internal Part No" ReadOnly="True" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="NewDesignLevel" SortExpression="NewDesignLevel" HeaderText="Design Level"
                            ReadOnly="True" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="NewDrawingNo" SortExpression="NewDrawingNo" HeaderText="Drawing No"
                            ReadOnly="True" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="NewCustomerPartName" SortExpression="NewCustomerPartName"
                            HeaderText="Name" ReadOnly="True" ItemStyle-HorizontalAlign="Center" />
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsFinishedPart" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetTAFinishedPart" TypeName="TAModule" DeleteMethod="DeleteTAFinishedPart">
                    <DeleteParameters>
                        <asp:Parameter Name="RowID" Type="Int32" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:ControlParameter ControlID="lblTANo" DefaultValue="0" Name="TANo" PropertyName="Text"
                            Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <br />
                <table runat="server" id="tblChildPart" visible="false" width="98%">
                    <tr>
                        <td class="p_text">
                            Component Part No:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtNewPartNo" MaxLength="40" />
                            <asp:RequiredFieldValidator ID="rfvNewPartNo" runat="server" ControlToValidate="txtNewPartNo"
                                ErrorMessage="Child BPCS PartNo is a required field." Font-Bold="False" ValidationGroup="vgChildPart"><</asp:RequiredFieldValidator>
                            <asp:ImageButton ID="iBtnNewPartNo" runat="server" ImageUrl="~/images/Search.gif"
                                ToolTip="Click here to search for a part number." />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            DMS Drawing No:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtNewChildDrawingNo" MaxLength="18" />
                            <asp:ImageButton ID="iBtnNewChildDrawingNoSearch" runat="server" ImageUrl="~/images/Search.gif"
                                ToolTip="Click here to search for a DMS Drawing." />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Button runat="Server" ID="btnAddChildPart" Text="Add Child BPCS Part" ValidationGroup="vgChildPart" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:ValidationSummary ID="vsChildPart" runat="server" DisplayMode="List" ShowMessageBox="true"
                    ShowSummary="true" EnableClientScript="true" ValidationGroup="vgChildPart" />
                <asp:Label runat="server" ID="lblMessageChildPart" SkinID="MessageLabelSkin"></asp:Label>
                <asp:GridView ID="gvChildPart" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                    AllowPaging="True" PageSize="15" DataKeyNames="RowID" DataSourceID="odsChildPart"
                    ShowFooter="false" Width="98%">
                    <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#CCCCCC" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField ShowHeader="False">
                            <EditItemTemplate>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="iBtnChildPartDelete" runat="server" CausesValidation="False"
                                    CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CurrentPartNo" SortExpression="NewPartNo" HeaderText="Current Component Part No"
                            ReadOnly="True" />
                        <asp:BoundField DataField="CurrentDrawingNo" SortExpression="CurrentDrawingNo" HeaderText="DrawingNo"
                            ReadOnly="True" />
                        <asp:BoundField DataField="CurrentPartName" SortExpression="CurrentPartName" HeaderText="Name"
                            ReadOnly="True" />
                        <asp:BoundField DataField="NewPartNo" SortExpression="NewPartNo" HeaderText="New Component Part No"
                            ReadOnly="True" />
                        <asp:BoundField DataField="NewDrawingNo" SortExpression="NewDrawingNo" HeaderText="DrawingNo"
                            ReadOnly="True" />
                        <asp:BoundField DataField="NewPartName" SortExpression="NewPartName" HeaderText="Name"
                            ReadOnly="True" />
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsChildPart" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetTAChildPart" TypeName="TAModule" DeleteMethod="DeleteTAChildPart">
                    <DeleteParameters>
                        <asp:Parameter Name="RowID" Type="Int32" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:ControlParameter ControlID="lblTANo" DefaultValue="0" Name="TANo" PropertyName="Text"
                            Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </asp:View>
            <asp:View ID="vwTasks" runat="server">
                <br />
                <asp:ValidationSummary ID="vsEditTaskTeamMember" runat="server" DisplayMode="List"
                    ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgEditTaskTeamMember" />
                <asp:ValidationSummary ID="vsInsertTaskTeamMember" runat="server" DisplayMode="List"
                    ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgInsertTaskTeamMember" />
                <asp:Label ID="lblMessageTasks" runat="server" SkinID="MessageLabelSkin" />
                <asp:GridView ID="gvToolingAuthTask" runat="server" AutoGenerateColumns="False" DataKeyNames="RowID"
                    DataSourceID="odsToolingAuthTask" RowStyle-Height="20px" RowStyle-CssClass="c_text"
                    HeaderStyle-CssClass="c_text" EmptyDataText="No team members have been assigned tasks yet."
                    Width="98%">
                    <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" Wrap="False" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="c_text" Height="20px" />
                    <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" CssClass="c_text" />
                    <EditRowStyle BackColor="#E2DED6" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <EmptyDataRowStyle Wrap="False" />
                    <Columns>
                        <asp:TemplateField HeaderText="Edit">
                            <EditItemTemplate>
                                <asp:ImageButton ID="ibtnUpdateTaskTeamMember" runat="server" CausesValidation="True"
                                    CommandName="Update" ImageUrl="~/images/save.jpg" ToolTip="Save" ValidationGroup="vgEditTaskTeamMember" />&nbsp;&nbsp;&nbsp;
                                <asp:ImageButton ID="ibtnCancelTaskTeamMember" runat="server" CausesValidation="False"
                                    CommandName="Cancel" ImageUrl="~/images/cancel.jpg" ToolTip="Cancel" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="ibtnEditTaskTeamMember" runat="server" CausesValidation="False"
                                    CommandName="Edit" ToolTip="Edit" ImageUrl="~/images/edit.jpg" />&nbsp;&nbsp;&nbsp;
                            </ItemTemplate>
                            <ItemStyle Width="60px" />
                            <FooterTemplate>
                                <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgInsertTaskTeamMember"
                                    runat="server" ID="iBtnInserTaskTeamMember" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                                <asp:ImageButton ID="iBtnUndoInserTaskTeamMember" runat="server" CommandName="Undo"
                                    CausesValidation="false" ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delete">
                            <ItemTemplate>
                                <asp:ImageButton ID="ibtnDeleteTaskTeamMember" runat="server" CausesValidation="False"
                                    CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Task" SortExpression="ddTaskName">
                            <HeaderStyle Wrap="true" />
                            <FooterStyle Wrap="false" />
                            <ItemStyle Wrap="true" />
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddEditTask" runat="server" DataSource='<%# TAModule.GetTATaskMaint(0,"") %>'
                                    DataValueField="TaskID" DataTextField="ddTaskName" SelectedValue='<%# Bind("TaskID") %>'>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvEditTask" runat="server" ControlToValidate="ddEditTask"
                                    ErrorMessage="The task is required." Font-Bold="True" ValidationGroup="vgEditTaskTeamMember"
                                    Text="<" SetFocusOnError="true" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewTaskID" runat="server" Text='<%# Bind("TaskID") %>' CssClass="none"></asp:Label>
                                <asp:Label ID="lblViewTaskName" runat="server" Text='<%# Bind("ddTaskName") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddInsertTask" runat="server" DataSource='<%# TAModule.GetTATaskMaint(0,"") %>'
                                    DataValueField="TaskID" DataTextField="ddTaskName" AppendDataBoundItems="True">
                                    <asp:ListItem Text="" Value="0" Selected="False"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvInsertTask" runat="server" ControlToValidate="ddInsertTask"
                                    ErrorMessage="The task is required." Font-Bold="True" ValidationGroup="vgInsertTaskTeamMember"
                                    Text="<" SetFocusOnError="true" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Team Member" SortExpression="ddTeamMemberName">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddEditTaskTeamMember" runat="server" DataSource='<%# TAModule.GetTATaskTeamMember() %>'
                                    DataValueField="TeamMemberID" DataTextField="ddFullTeamMemberName" SelectedValue='<%# Bind("TeamMemberID") %>'>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvEditTaskTeamMember" runat="server" ControlToValidate="ddEditTaskTeamMember"
                                    ErrorMessage="The task team member is required." Font-Bold="True" ValidationGroup="vgEditTaskTeamMember"
                                    Text="<" SetFocusOnError="true" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewTaskTeamMember" runat="server" Text='<%# Bind("ddTeamMemberName") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblTaskTeamMemberMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                    Text="* " />
                                <asp:DropDownList ID="ddInsertTaskTeamMember" runat="server" DataSource='<%# TAModule.GetTATaskTeamMember() %>'
                                    DataValueField="TeamMemberID" DataTextField="ddFullTeamMemberName" AppendDataBoundItems="True">
                                    <asp:ListItem Text="" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvInsertTaskTeamMember" runat="server" ControlToValidate="ddInsertTaskTeamMember"
                                    ErrorMessage="The task team member is required." Font-Bold="True" ValidationGroup="vgInsertTaskTeamMember"
                                    Text="<" SetFocusOnError="true" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Due" SortExpression="TargetDate" ItemStyle-HorizontalAlign="Center"
                            HeaderStyle-HorizontalAlign="Center">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditTaskTargetDate" runat="server" Text='<%# Bind("TargetDate") %>'
                                    MaxLength="10"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender ID="ftbeEditTaskTargetDate" runat="server" TargetControlID="txtEditTaskTargetDate"
                                    FilterType="Custom, Numbers" ValidChars="/" />
                                <asp:ImageButton runat="server" ID="imgEditTaskTargetDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                    AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                                <ajax:CalendarExtender ID="cbeEditTaskTargetDate" runat="server" TargetControlID="txtEditTaskTargetDate"
                                    PopupButtonID="imgEditTaskTargetDate" />
                                <asp:RegularExpressionValidator ID="revEditTaskTargetDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                    ControlToValidate="txtEditTaskTargetDate" Font-Bold="True" ToolTip="MM/DD/YYYY"
                                    ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                    Width="8px" ValidationGroup="vgEditTaskTeamMember" Text="<"></asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="rfvEditTaskTargetDate" runat="server" ControlToValidate="txtEditTaskTargetDate"
                                    Text="<" ErrorMessage="Due Date is Required." SetFocusOnError="True" ValidationGroup="vgEditTaskTeamMember"></asp:RequiredFieldValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewTaskTargetDate" runat="server" Text='<%# Bind("TargetDate") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblTaskTargetDateMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                    Text="* " />
                                <asp:TextBox ID="txtInsertTaskTargetDate" runat="server" Text='<%# Bind("TargetDate") %>'
                                    MaxLength="10"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender ID="ftbeInsertTaskTargetDate" runat="server" TargetControlID="txtInsertTaskTargetDate"
                                    FilterType="Custom, Numbers" ValidChars="/" />
                                <asp:ImageButton runat="server" ID="imgInsertTaskTargetDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                    AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                                <ajax:CalendarExtender ID="cbeInsertTaskTargetDate" runat="server" TargetControlID="txtInsertTaskTargetDate"
                                    PopupButtonID="imgInsertTaskTargetDate" />
                                <asp:RegularExpressionValidator ID="revInsertTaskTargetDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                    ControlToValidate="txtInsertTaskTargetDate" Font-Bold="True" ToolTip="MM/DD/YYYY"
                                    ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                    Width="8px" ValidationGroup="vgInsertTaskTeamMember" Text="<"></asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="rfvInsertTaskTargetDate" runat="server" ControlToValidate="txtInsertTaskTargetDate"
                                    Text="<" ErrorMessage="Due Date is Required." SetFocusOnError="True" ValidationGroup="vgInsertTaskTeamMember"></asp:RequiredFieldValidator>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Completion" SortExpression="CompletionDate" ItemStyle-HorizontalAlign="Center"
                            HeaderStyle-HorizontalAlign="Center">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditTaskCompletionDate" runat="server" Text='<%# Bind("CompletionDate") %>'
                                    MaxLength="10"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender ID="ftbeEditTaskCompletionDate" runat="server" TargetControlID="txtEditTaskCompletionDate"
                                    FilterType="Custom, Numbers" ValidChars="/" />
                                <asp:ImageButton runat="server" ID="imgEditTaskCompletionDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                    AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                                <ajax:CalendarExtender ID="cbeEditTaskCompletionDate" runat="server" TargetControlID="txtEditTaskCompletionDate"
                                    PopupButtonID="imgEditTaskCompletionDate" />
                                <asp:RegularExpressionValidator ID="revEditTaskCompletionDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                    ControlToValidate="txtEditTaskCompletionDate" Font-Bold="True" ToolTip="MM/DD/YYYY"
                                    ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                    Width="8px" ValidationGroup="vgEditTaskTeamMember" Text="<"></asp:RegularExpressionValidator>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewTaskCompletionDate" runat="server" Text='<%# Bind("CompletionDate") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Notification" SortExpression="NotificationDate" ItemStyle-HorizontalAlign="Center"
                            HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblViewNotificationDate" runat="server" Text='<%# Bind("NotificationDate") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ValidationSummary ID="vsToolingAuthorizationTeamMemberTask" runat="server" ShowMessageBox="True"
                    ValidationGroup="EditTeamMemberTaskInfo" />
                <asp:ObjectDataSource ID="odsToolingAuthTask" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetTATask" TypeName="ExpProjToolingAuthBLL" InsertMethod="InsertTATask"
                    UpdateMethod="UpdateTATask" DeleteMethod="DeleteTATask">
                    <DeleteParameters>
                        <asp:Parameter Name="RowID" Type="Int32" />
                        <asp:Parameter Name="original_RowID" Type="Int32" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="original_RowID" Type="Int32" />
                        <asp:ControlParameter ControlID="lblTANo" Name="TANo" PropertyName="Text" Type="Int32" />
                        <asp:Parameter Name="TaskID" Type="Int32" />
                        <asp:Parameter Name="TeamMemberID" Type="Int32" />
                        <asp:Parameter Name="NotificationDate" Type="String" />
                        <asp:Parameter Name="TargetDate" Type="String" />
                        <asp:Parameter Name="CompletionDate" Type="String" />
                        <asp:Parameter Name="RowID" Type="Int32" />
                    </UpdateParameters>
                    <SelectParameters>
                        <asp:ControlParameter ControlID="lblTANo" Name="TANo" PropertyName="Text" Type="Int32" />
                    </SelectParameters>
                    <InsertParameters>
                        <asp:ControlParameter ControlID="lblTANo" Name="TANo" PropertyName="Text" Type="Int32" />
                        <asp:Parameter Name="TaskID" Type="Int32" />
                        <asp:Parameter Name="TeamMemberID" Type="Int32" />
                        <asp:Parameter Name="TargetDate" Type="String" />
                    </InsertParameters>
                </asp:ObjectDataSource>
                <br />
                <table>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnNotify" runat="server" Text="Notify" Width="130px" />
                            <asp:CheckBox runat="server" ID="cbNotifyAll" Text="Notify All Team Members listed"
                                Checked="false" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Label ID="lblMessageTasksBottom" runat="server" SkinID="MessageLabelSkin" />
            </asp:View>
            <asp:View ID="vwDieshop" runat="server">
                <asp:Label ID="lblMessageDieshop" runat="server" SkinID="MessageLabelSkin"></asp:Label>
                <asp:ValidationSummary runat="server" ID="vsDieshop" ValidationGroup="vgDieshop"
                    ShowMessageBox="true" ShowSummary="true" />
                <table>
                    <tr>
                        <td class="p_text" style="white-space: nowrap;" align="left">
                            Serial No:
                        </td>
                        <td>
                            <asp:TextBox ID="txtSerialNo" runat="server" MaxLength="50" CssClass="c_text"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblInstructionsMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="* " />
                            Special Instructions:
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtInstructions" runat="server" TextMode="MultiLine" Height="90px"
                                Width="500px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvInstructions" runat="server" ControlToValidate="txtInstructions"
                                ErrorMessage="Instructions are required." Font-Bold="True" ValidationGroup="vgDieshop"
                                Text="<" SetFocusOnError="true" />
                            <br />
                            <asp:Label ID="lblInstructionsCharCount" SkinID="MessageLabelSkin" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Rules:
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtRules" runat="server" TextMode="MultiLine" Height="90px" Width="500px"></asp:TextBox>
                            <br />
                            <asp:Label ID="lblRulesCharCount" SkinID="MessageLabelSkin" runat="server"></asp:Label>
                            <br />
                            <span style="font-size: xx-small; font-style: italic">(Examples:
                                <br />
                                4 Pt 6 53/1 6 lb. CF-1.125
                                <br />
                                3 Pt 6 53/1 6 lb. CF-1.125
                                <br />
                                4 Pt 1 1/2 X 055 CB-3/16 DB)</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Button ID="btnSaveDieshop" runat="server" Text="Save" Visible="false" ValidationGroup="vgDieshop" />
                        </td>
                    </tr>
                </table>
                <br />
                <table width="98%">
                    <tr>
                        <td>
                            <asp:Label ID="lblMessageMaterial" runat="server" SkinID="MessageLabelSkin"></asp:Label>
                            <asp:ValidationSummary runat="server" ID="vsInsertMat" ValidationGroup="vgInsertMat"
                                ShowMessageBox="true" ShowSummary="true" />
                            <asp:ValidationSummary runat="server" ID="vsEditMat" ValidationGroup="vgEditMat"
                                ShowMessageBox="true" ShowSummary="true" />
                            <asp:GridView ID="gvMaterial" runat="server" AutoGenerateColumns="False" DataSourceID="odsTADSMaterial"
                                DataKeyNames="RowID">
                                <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="Black" Wrap="False" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#E2DED6" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <EmptyDataRowStyle Wrap="False" />
                                <Columns>
                                    <asp:TemplateField ShowHeader="False">
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="iBtnUpdateMat" runat="server" CausesValidation="True" CommandName="Update"
                                                ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditMat" />
                                            <asp:ImageButton ID="iBtnCancelMat" runat="server" CausesValidation="False" CommandName="Cancel"
                                                ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibtnEditMat" runat="server" CausesValidation="False" CommandName="Edit"
                                                ToolTip="Edit" ImageUrl="~/images/edit.jpg" />
                                            <asp:ImageButton ID="ibtnDeleteMat" runat="server" CausesValidation="False" CommandName="Delete"
                                                ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" /></ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgInsertMat"
                                                runat="server" ID="iBtnInsertMat" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                                            <asp:ImageButton ID="iBtnUndoMat" runat="server" CommandName="Undo" CausesValidation="false"
                                                ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                                        </FooterTemplate>
                                      
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Material" SortExpression="ddMaterialName">
                                        <HeaderStyle Wrap="false" />
                                        <FooterStyle Wrap="false" />
                                        <ItemStyle Wrap="false" />
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddEditMat" Width="200px" runat="server" DataSource='<%# TAModule.GetTADSMaterialMaint(0,"") %>'
                                                DataValueField="DSMaterialID" DataTextField="ddMaterialName" SelectedValue='<%# Bind("DSMaterialID") %>'>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvEditMat" runat="server" ControlToValidate="ddEditMat"
                                                ErrorMessage="Material  is required." Text="<" Font-Bold="True" ValidationGroup="vgEditMat"> 
                                            </asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblMaterialName" runat="server" Text='<%# Bind("ddMaterialName") %>'></asp:Label></ItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddInsertMat" Width="200px" runat="server" DataSource='<%# TAModule.GetTADSMaterialMaint(0,"") %>'
                                                DataValueField="DSMaterialID" DataTextField="ddMaterialName" AppendDataBoundItems="True">
                                                <asp:ListItem Text="" Value="0" Selected="False"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvInsertMat" runat="server" ControlToValidate="ddInsertMat"
                                                ErrorMessage="The Matertial is required." Font-Bold="True" ValidationGroup="vgInsertMat"
                                                Text="<" SetFocusOnError="true" /></FooterTemplate>
                                        <FooterStyle Width="225px" />
                                        <ItemStyle Width="225px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Desc / Size">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditMatNotes" runat="server" Text='<%# Bind("Notes") %>' MaxLength="50"
                                                Width="150px"></asp:TextBox></EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewMatNotes" runat="server" Text='<%# Bind("Notes") %>'> </asp:Label></ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtInsertMatNotes" runat="server" Text='<%# Bind("Notes") %>' MaxLength="50"
                                                Width="150px"> 
                                            </asp:TextBox>
                                        </FooterTemplate>
                                        <FooterStyle Width="200px" />
                                        <ItemStyle Width="200px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Qty Used">
                                        <EditItemTemplate>
                                            <asp:Label ID="lblEditMatQtyMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                                Text="* " />
                                            <asp:TextBox ID="txtEditMatQty" runat="server" Text='<%# Bind("Quantity") %>' MaxLength="10"
                                                Width="50px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvEditMatQty" runat="server" ControlToValidate="txtEditMatQty"
                                                ErrorMessage="Quantity is required" Font-Bold="True" ValidationGroup="vgEditMat"
                                                Text="<" SetFocusOnError="true" />
                                            <ajax:FilteredTextBoxExtender ID="ftEditMatQty" runat="server" TargetControlID="txtEditMatQty"
                                                FilterType="Custom, Numbers" ValidChars="-.," />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewMatQty" runat="server" Text='<%# Bind("Quantity") %>'></asp:Label></ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblInsertMatQtyMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                                Text="* " /><asp:TextBox ID="txtInsertMatQty" runat="server" Text='<%# Bind("Quantity") %>'
                                                    MaxLength="10" Width="50px"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvInsertMatQty" runat="server" ControlToValidate="txtInsertMatQty"
                                                ErrorMessage="Quantity is required" Font-Bold="True" ValidationGroup="vgInsertMat"
                                                Text="<" SetFocusOnError="true" />
                                            <ajax:FilteredTextBoxExtender ID="ftInsertInsertMatQty" runat="server" TargetControlID="txtInsertMatQty"
                                                FilterType="Custom, Numbers" ValidChars="-.," />
                                        </FooterTemplate>
                                        <FooterStyle Width="75px" />
                                        <ItemStyle Width="75px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Units" SortExpression="ddUnitName">
                                        <HeaderStyle Wrap="false" />
                                        <FooterStyle Wrap="false" />
                                        <ItemStyle Wrap="false" />
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddEditMatUnit" Width="75px" runat="server" DataSource='<%# commonfunctions.GetUnit(0,"","") %>'
                                                DataValueField="UnitID" DataTextField="ddUnitName" SelectedValue='<%# Bind("UnitID") %>'
                                                AppendDataBoundItems="true">
                                                <asp:ListItem Text="" Value="0" Selected="False"></asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblUnitName" runat="server" Text='<%# Bind("ddUnitName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddInsertMatUnit" Width="75px" runat="server" DataSource='<%# commonfunctions.GetUnit(0,"","")  %>'
                                                DataValueField="UnitID" DataTextField="ddUnitName" AppendDataBoundItems="True">
                                                <asp:ListItem Text="" Value="0" Selected="False"></asp:ListItem>
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                        <FooterStyle Width="100px" />
                                        <ItemStyle Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cost">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditMatCost" runat="server" Text='<%# Bind("Cost") %>' MaxLength="10"
                                                Width="50px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvEditMatCost" runat="server" ControlToValidate="txtEditMatCost"
                                                ErrorMessage="Cost is required" Font-Bold="True" ValidationGroup="vgEditMat"
                                                Text="<" SetFocusOnError="true" />
                                            <ajax:FilteredTextBoxExtender ID="ftEditLaborNumHrs" runat="server" TargetControlID="txtEditMatCost"
                                                FilterType="Custom, Numbers" ValidChars="-.," />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewMatCost" runat="server" Text='<%# Bind("Cost") %>'></asp:Label></ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lbltxtInsertMatCostMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                                Text="* " /><asp:TextBox ID="txtInsertMatCost" runat="server" Text='<%# Bind("Cost") %>'
                                                    MaxLength="10" Width="50px"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvInsertMatCost" runat="server" ControlToValidate="txtInsertMatCost"
                                                ErrorMessage="Cost is required" Font-Bold="True" ValidationGroup="vgInsertMat"
                                                Text="<" SetFocusOnError="true" />
                                            <ajax:FilteredTextBoxExtender ID="ftInsertLaborNumHrs" runat="server" TargetControlID="txtInsertMatCost"
                                                FilterType="Custom, Numbers" ValidChars="-.," />
                                        </FooterTemplate>
                                        <FooterStyle Width="75px" />
                                        <ItemStyle Width="75px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-ForeColor="Red" ItemStyle-HorizontalAlign="Right">
                                        <FooterTemplate>
                                        </FooterTemplate>
                                        <FooterStyle Width="120px" />
                                        <ItemStyle Width="120px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-ForeColor="Black" ItemStyle-HorizontalAlign="Right">
                                        <FooterTemplate>
                                        </FooterTemplate>
                                        <FooterStyle Width="60px" />
                                        <ItemStyle Width="60px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-ForeColor="Red" ItemStyle-HorizontalAlign="Right">
                                        <FooterTemplate>
                                        </FooterTemplate>
                                        <FooterStyle Width="100px" />
                                        <ItemStyle Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-ForeColor="Black" ItemStyle-HorizontalAlign="Right"
                                        HeaderText="Total Material" ItemStyle-Wrap="false">
                                        <FooterTemplate>
                                        </FooterTemplate>
                                        <FooterStyle Width="60px" />
                                        <ItemStyle Width="60px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:ObjectDataSource ID="odsTADSMaterial" runat="server" DeleteMethod="DeleteTADSMaterial"
                                InsertMethod="InsertTADSMaterial" OldValuesParameterFormatString="original_{0}"
                                SelectMethod="GetTADSMaterial" TypeName="ExpProjToolingAuthBLL" UpdateMethod="UpdateTADSMaterial">
                                <DeleteParameters>
                                    <asp:Parameter Name="RowID" Type="Int32" />
                                    <asp:Parameter Name="original_RowID" Type="Int32" />
                                </DeleteParameters>
                                <UpdateParameters>
                                    <asp:Parameter Name="original_RowID" Type="Int32" />
                                    <asp:ControlParameter ControlID="lblTANo" Name="TANo" PropertyName="Text" Type="Int32" />
                                    <asp:Parameter Name="DSMaterialID" Type="Int32" />
                                    <asp:Parameter Name="Notes" Type="String" />
                                    <asp:Parameter Name="Quantity" Type="Decimal" />
                                    <asp:Parameter Name="Cost" Type="Decimal" />
                                    <asp:Parameter Name="UnitID" Type="int32" />
                                    <asp:Parameter Name="RowID" Type="Int32" />
                                </UpdateParameters>
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="lblTANo" Name="TANo" PropertyName="Text" Type="Int32" />
                                </SelectParameters>
                                <InsertParameters>
                                    <asp:ControlParameter ControlID="lblTANo" Name="TANo" PropertyName="Text" Type="Int32" />
                                    <asp:Parameter Name="DSMaterialID" Type="Int32" />
                                    <asp:Parameter Name="Notes" Type="String" />
                                    <asp:Parameter Name="Quantity" Type="Decimal" />
                                    <asp:Parameter Name="Cost" Type="Decimal" />
                                    <asp:Parameter Name="UnitID" Type="int32" />
                                </InsertParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblMessageLabor" runat="server" SkinID="MessageLabelSkin"></asp:Label>
                            <asp:ValidationSummary runat="server" ID="vsInsertLabor" ValidationGroup="vgInsertLabor"
                                ShowMessageBox="true" ShowSummary="true" />
                            <asp:ValidationSummary runat="server" ID="vsEditLabor" ValidationGroup="vgEditLabor"
                                ShowMessageBox="true" ShowSummary="true" />
                            <asp:GridView ID="gvLabor" runat="server" AutoGenerateColumns="False" DataSourceID="odsTADSLabor"
                                DataKeyNames="RowID">
                                <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="Black" Wrap="False" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#E2DED6" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <EmptyDataRowStyle Wrap="False" />
                                <Columns>
                                    <asp:TemplateField ShowHeader="False">
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="iBtnUpdateLabor" runat="server" CausesValidation="True" CommandName="Update"
                                                ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditLabor" />
                                            <asp:ImageButton ID="iBtnCancelLabor" runat="server" CausesValidation="False" CommandName="Cancel"
                                                ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" /></EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibtnEditLabor" runat="server" CausesValidation="False" CommandName="Edit"
                                                ToolTip="Edit" ImageUrl="~/images/edit.jpg" />
                                            <asp:ImageButton ID="ibtnDeleteLabor" runat="server" CausesValidation="False" CommandName="Delete"
                                                ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" /></ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgInsertLabor"
                                                runat="server" ID="iBtnInsertLabor" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                                            <asp:ImageButton ID="iBtnUndoLabor" runat="server" CommandName="Undo" CausesValidation="false"
                                                ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" /></FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Labor" SortExpression="ddLaborName">
                                        <HeaderStyle Wrap="false" />
                                        <FooterStyle Wrap="false" />
                                        <ItemStyle Wrap="false" />
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddEditLabor" Width="200px" runat="server" DataSource='<%# TAModule.GetTADSLaborMaint(0,"") %>'
                                                DataValueField="DSLaborID" DataTextField="ddLaborName" SelectedValue='<%# Bind("DSLaborID") %>'>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvEditLabor" runat="server" ControlToValidate="ddEditLabor"
                                                ErrorMessage="Labor  is required." Text="<" Font-Bold="True" ValidationGroup="vgEditLabor"> 
                                            </asp:RequiredFieldValidator></EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewLaborName" runat="server" Text='<%# Bind("ddLaborName") %>'></asp:Label></ItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddInsertLabor" Width="200px" runat="server" DataSource='<%# TAModule.GetTADSLaborMaint(0,"") %>'
                                                DataValueField="DSLaborID" DataTextField="ddLaborName" AppendDataBoundItems="True">
                                                <asp:ListItem Text="" Value="0" Selected="False"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvInsertLabor" runat="server" ControlToValidate="ddInsertLabor"
                                                ErrorMessage="The Labor is required." Font-Bold="True" ValidationGroup="vgInsertLabor"
                                                Text="<" SetFocusOnError="true" /></FooterTemplate>
                                        <FooterStyle Width="225px" />
                                        <ItemStyle Width="225px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Notes">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditLaborNotes" runat="server" Text='<%# Bind("Notes") %>' MaxLength="50"
                                                Width="150px"></asp:TextBox></EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewLaborNotes" runat="server" Text='<%# Bind("Notes") %>'>"></asp:Label></ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtInsertLaborNotes" runat="server" Text='<%# Bind("Notes") %>'
                                                MaxLength="50" Width="150px"> </asp:TextBox>
                                        </FooterTemplate>
                                        <FooterStyle Width="200px" />
                                        <ItemStyle Width="200px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Number of Hours">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditLaborNumHrs" runat="server" Text='<%# Bind("NumberHours") %>'
                                                MaxLength="10" Width="50px"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftEditLaborNumHrs" runat="server" TargetControlID="txtEditLaborNumHrs"
                                                FilterType="Custom, Numbers" ValidChars="-.," />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewLaborNumHrs" runat="server" Text='<%# Bind("NumberHours") %>'></asp:Label></ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblInsertLaborNumHrsMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                                Text="* " />
                                            <asp:TextBox ID="txtInsertLaborNumHrs" runat="server" Text='<%# Bind("NumberHours") %>'
                                                MaxLength="10" Width="50px"> </asp:TextBox><asp:RequiredFieldValidator ID="rfvInsertLaborNumHrs"
                                                    runat="server" ControlToValidate="txtInsertLaborNumHrs" ErrorMessage="Numbe of hours are required"
                                                    Font-Bold="True" ValidationGroup="vgInsertLabor" Text="<" SetFocusOnError="true" />
                                            <ajax:FilteredTextBoxExtender ID="ftInsertLaborNumHrs" runat="server" TargetControlID="txtInsertLaborNumHrs"
                                                FilterType="Custom, Numbers" ValidChars="-.," />
                                        </FooterTemplate>
                                        <FooterStyle Width="75px" />
                                        <ItemStyle Width="75px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cost">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditLaborCost" runat="server" Text='<%# Bind("Cost") %>' MaxLength="10"
                                                Width="50px"></asp:TextBox>
                                            <ajax:FilteredTextBoxExtender ID="ftEditLaborCost" runat="server" TargetControlID="txtEditLaborCost"
                                                FilterType="Custom, Numbers" ValidChars="-.," />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewLaborCost" runat="server" Text='<%# Bind("Cost") %>'></asp:Label></ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblInsertLaborCostMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                                Text="* " />
                                            <asp:TextBox ID="txtInsertLaborCost" runat="server" Text='<%# Bind("Cost") %>' MaxLength="10"
                                                Width="50px"> </asp:TextBox><asp:RequiredFieldValidator ID="rfvInsertLaborCost" runat="server"
                                                    ControlToValidate="txtInsertLaborCost" ErrorMessage="Cost is required" Font-Bold="True"
                                                    ValidationGroup="vgInsertLabor" Text="<" SetFocusOnError="true" />
                                            <ajax:FilteredTextBoxExtender ID="ftInsertLaborCost" runat="server" TargetControlID="txtInsertLaborCost"
                                                FilterType="Custom, Numbers" ValidChars="-.," />
                                        </FooterTemplate>
                                        <FooterStyle Width="75px" />
                                        <ItemStyle Width="75px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-ForeColor="Red" ItemStyle-HorizontalAlign="Right">
                                        <FooterTemplate>
                                        </FooterTemplate>
                                        <FooterStyle Width="120px" />
                                        <ItemStyle Width="120px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-ForeColor="Black" ItemStyle-HorizontalAlign="Right">
                                        <FooterTemplate>
                                        </FooterTemplate>
                                        <FooterStyle Width="60px" />
                                        <ItemStyle Width="60px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-ForeColor="Red" ItemStyle-HorizontalAlign="Right">
                                        <FooterTemplate>
                                        </FooterTemplate>
                                        <FooterStyle Width="100px" />
                                        <ItemStyle Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-ForeColor="Black" ItemStyle-HorizontalAlign="Right"
                                        HeaderText="Total Labor" ItemStyle-Wrap="false">
                                        <FooterTemplate>
                                        </FooterTemplate>
                                        <FooterStyle Width="60px" />
                                        <ItemStyle Width="60px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:ObjectDataSource ID="odsTADSLabor" runat="server" DeleteMethod="DeleteTADSLabor"
                                InsertMethod="InsertTADSLabor" OldValuesParameterFormatString="original_{0}"
                                SelectMethod="GetTADSLabor" TypeName="ExpProjToolingAuthBLL" UpdateMethod="UpdateTADSLabor">
                                <DeleteParameters>
                                    <asp:Parameter Name="RowID" Type="Int32" />
                                    <asp:Parameter Name="original_RowID" Type="Int32" />
                                </DeleteParameters>
                                <UpdateParameters>
                                    <asp:Parameter Name="original_RowID" Type="Int32" />
                                    <asp:ControlParameter ControlID="lblTANo" Name="TANo" PropertyName="Text" Type="Int32" />
                                    <asp:Parameter Name="DSLaborID" Type="Int32" />
                                    <asp:Parameter Name="NumberHours" Type="Decimal" />
                                    <asp:Parameter Name="Notes" Type="String" />
                                    <asp:Parameter Name="Cost" Type="Decimal" />
                                    <asp:Parameter Name="RowID" Type="Int32" />
                                </UpdateParameters>
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="lblTANo" Name="TANo" PropertyName="Text" Type="Int32" />
                                </SelectParameters>
                                <InsertParameters>
                                    <asp:ControlParameter ControlID="lblTANo" Name="TANo" PropertyName="Text" Type="Int32" />
                                    <asp:Parameter Name="DSLaborID" Type="Int32" />
                                    <asp:Parameter Name="NumberHours" Type="Decimal" />
                                    <asp:Parameter Name="Notes" Type="String" />
                                    <asp:Parameter Name="Cost" Type="Decimal" />
                                </InsertParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                </table>
                <br />
                <table align="right" width="50%">
                    <tr>
                        <td class="p_text" style="white-space: nowrap;">
                            Total Tooling Authorization Dieshop Cost:
                        </td>
                        <td style="color: #990000;">
                            <asp:Label runat="server" ID="lblTotalDieShop" CssClass="c_textbold"></asp:Label>
                        </td>
                        <td width="20px">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <br />
                <table align="center" width="50%">
                    <tr>
                        <td align="right">
                            <asp:CheckBox runat="server" ID="cbDieshopComplete" Text="Dieshop Cost Complete"
                                Enabled="false" />
                        </td>
                        <td align="left">
                            <asp:Button ID="btnDieshopComplete" runat="server" Text="Dieshop Complete" Visible="false"
                                ValidationGroup="vgDieshop" />
                        </td>
                    </tr>
                </table>
                <asp:Label ID="lblMessageDieshopBottom" runat="server" SkinID="MessageLabelSkin"></asp:Label>
            </asp:View>
            <asp:View ID="vsSupportingDocuments" runat="server">
                <asp:Label runat="server" ID="lblMessageSupportingDocs"></asp:Label>
                <asp:ValidationSummary ID="vsSupportingDocs" runat="server" DisplayMode="List" ShowMessageBox="true"
                    ShowSummary="true" ValidationGroup="vgSupportingDocs" />
                <br />
                <table width="98%" runat="server" id="tblUpload" visible="false">
                    <tr>
                        <td class="p_textbold" valign="top">
                            File Description:
                        </td>
                        <td class="c_text">
                            <asp:TextBox ID="txtSupportingDocDesc" runat="server" MaxLength="200" Rows="3" TextMode="MultiLine"
                                Width="600px" />
                            <asp:RequiredFieldValidator ID="rfvSupportingDocDesc" runat="server" ControlToValidate="txtSupportingDocDesc"
                                ErrorMessage="Supporting Document File Description is a required field." Font-Bold="False"
                                ValidationGroup="vgSupportingDocs"><</asp:RequiredFieldValidator><br />
                            <br />
                            <asp:Label ID="lblSupportingDocDescCharCount" SkinID="MessageLabelSkin" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="white-space: nowrap;">
                            <asp:Label runat="server" ID="lblFileUploadLabel" Text="Upload a supporting file under 3 MB:<br />(PDF,DOC,DOCX,XLS,XLSX,JPEG,TIF,MSG,PPT,PPTX)"
                                CssClass="p_textbold"></asp:Label>
                        </td>
                        <td style="white-space: nowrap;">
                            <asp:FileUpload ID="fileUploadSupportingDoc" runat="server" Width="600px" />
                            <asp:Button ID="btnSaveUploadSupportingDocument" runat="server" Text="Upload" CausesValidation="true"
                                ValidationGroup="vgSupportingDocs" />
                            <asp:RequiredFieldValidator ID="rfvFileUploadSupportingDoc" runat="server" ControlToValidate="fileUploadSupportingDoc"
                                ErrorMessage="PDF File is required." Font-Bold="False" ValidationGroup="vgUpload"
                                SetFocusOnError="true" Text="<"></asp:RequiredFieldValidator><br />
                            <asp:RegularExpressionValidator ID="revUploadFile" runat="server" ErrorMessage="Please upload only *.PDF, *.DOC,*.DOCX, *.XLS, *.XLSX, *.JPEG, *.JPG, *.TIF, *.PPT, *.PPTX files are allowed."
                                ValidationExpression="(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.pdf|.xls|.xlsx|.doc|.docx|.jpeg|.jpg|.tif|.msg|.ppt|.pptx|.PDF|.XLS|.XLSX|.DOC|.DOCX|.JPEG|.JPG|.TIF|.MSG|.PPT|.PPTX)$"
                                ControlToValidate="fileUploadSupportingDoc" ValidationGroup="vgSupportingDocs"
                                SetFocusOnError="true" Font-Bold="True" Font-Size="Small" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:GridView ID="gvSupportingDoc" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                    AllowPaging="True" PageSize="15" DataKeyNames="RowID" ShowFooter="False" DataSourceID="odsSupportingDoc"
                    EmptyDataText="No supporting documents exist yet." Width="98%">
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#CCCCCC" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="iBtnSupportingDocDelete" runat="server" CausesValidation="False"
                                    CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Supporting Document Name" SortExpression="SupportingDocName">
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkViewSupportingDoc" runat="server" NavigateUrl='<%# Eval("RowID", "~/EXP/ToolingAuthExpProjDocument.aspx?RowID={0}") %>'
                                    Target="_blank" Text='<%# Eval("SupportingDocName") %>'>
                                </asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Description" DataField="SupportingDocDesc">
                            <ControlStyle Font-Size="X-Small" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Preview Document">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="hlnkPreview" ImageUrl='<%# DisplayImage(DataBinder.Eval(Container, "DataItem.EncodeType").tostring)  %>'
                                    NavigateUrl='<%# "ToolingAuthExpProjDocument.aspx?RowID=" & DataBinder.Eval (Container.DataItem,"RowID").tostring %>'
                                    Target="_blank" ToolTip="Preview Document" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsSupportingDoc" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetTASupportingDocList" TypeName="TAModule" DeleteMethod="DeleteTASupportingDoc">
                    <SelectParameters>
                        <asp:ControlParameter Name="TANo" Type="Int32" ControlID="lblTANo" DefaultValue="0" />
                    </SelectParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="RowID" Type="Int32" />
                        <asp:Parameter Name="original_RowID" Type="Int32" />
                    </DeleteParameters>
                </asp:ObjectDataSource>
            </asp:View>
            <asp:View ID="vwCommunicationBoard" runat="server">
                <asp:Label runat="server" ID="lblMessageCommunicationBoard" SkinID="MessageLabelSkin"></asp:Label>
                <asp:ValidationSummary ID="vsCommunicationBoard" runat="server" ValidationGroup="vgCommunicationBoard"
                    ShowMessageBox="true" ShowSummary="true" />
                <table runat="server" id="tblCommunicationBoardNewQuestion" visible="false">
                    <tr>
                        <td class="p_text" valign="top">
                            <asp:Label ID="lblRSSComment" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            New Question:
                        </td>
                        <td>
                            <asp:TextBox ID="txtRSSComment" runat="server" Width="550px" TextMode="MultiLine"
                                Rows="3" />
                            <asp:RequiredFieldValidator ID="rfvRSSComment" runat="server" ControlToValidate="txtRSSComment"
                                ErrorMessage="Question / Comment is a required field." ValidationGroup="vgCommunicationBoard"><</asp:RequiredFieldValidator>
                            <br />
                            <asp:Label ID="lblRSSCommentCharCount" runat="server" SkinID="MessageLabelSkin"></asp:Label>
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
                            <asp:Label ID="lblReqReply" runat="server" Text="* " ForeColor="Red" />
                            Reply / Comment:
                        </td>
                        <td>
                            <asp:TextBox ID="txtReply" runat="server" Rows="3" TextMode="MultiLine" Width="550px" />
                            <asp:RequiredFieldValidator ID="rfvReply" runat="server" ErrorMessage="Reply / Comment is a required field."
                                SetFocusOnError="true" ValidationGroup="vgReplyComment" ControlToValidate="txtReply"><</asp:RequiredFieldValidator><br />
                            <br />
                            <asp:Label runat="server" ID="lblReplyCharCount" SkinID="MessageLabelSkin"></asp:Label>
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
                                            DataKeyNames="TANo,RSSID" Width="100%">
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
                                            SelectMethod="GetTARSSReply" TypeName="ExpProjToolingAuthBLL">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="lblTANo" Name="TANo" PropertyName="Text" Type="Int32" />
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
                    SelectMethod="GetTARSS" TypeName="ExpProjToolingAuthBLL">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="lblTANo" Name="TANo" PropertyName="Text" Type="Int32" />
                        <asp:Parameter Name="RSSID" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </asp:View>
        </asp:MultiView>
    </asp:Panel>
</asp:Content>
