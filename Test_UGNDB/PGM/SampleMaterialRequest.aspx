<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="SampleMaterialRequest.aspx.vb" Inherits="PGM_SampleMaterialRequest"
    MaintainScrollPositionOnPostback="True" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="1150px">
        <asp:Label ID="lblErrors" runat="server" Font-Bold="True" Font-Size="Large" ForeColor="Red"
            Text="Label" Visible="False" CssClass="c_textbold" SkinID="MessageLabelSkin" />
        <% If ViewState("pSMRNo") <> Nothing Then%>
        <table>
            <tr>
                <td class="p_smalltextbold" style="color: #990000">
                    Edit data below, press
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CausesValidation="False" />
                    to enter new data or
                    <asp:Button ID="btnCopy" runat="server" Text="Copy" CausesValidation="False" />
                    to start new entry with a selection of data from record below.
                </td>
            </tr>
        </table>
        <%  End If%>
        <hr />
        <br />
        <table width="1000px">
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblReqNo" runat="server" Text="Request #:" />
                </td>
                <td>
                    <asp:Label ID="lblSMRNo" runat="server" Text="0" CssClass="c_textbold" Style="color: #990000;" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblTodaysDt" runat="server" Text="Todays Date:" />
                </td>
                <td>
                    <asp:TextBox ID="txtToday" runat="server" Enabled="false" Width="80px" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="Label8" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                    <asp:Label ID="lblSampleDesc" runat="server" Text="Sample Description:" />
                </td>
                <td>
                    <asp:TextBox ID="txtSampleDesc" runat="server" Width="300px" MaxLength="50" />
                    &nbsp;<asp:RequiredFieldValidator ID="rfvSampleDesc" runat="server" ControlToValidate="txtSampleDesc"
                        ErrorMessage="Sample Description is a required field." Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator><br />
                    <asp:Label ID="lblSampleDescrChar" runat="server" Font-Bold="True" ForeColor="Red" />
                </td>
                <td class="p_text">
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                    <asp:Label ID="lblDueDt" runat="server" Text="Due Date:" />
                </td>
                <td>
                    <asp:TextBox ID="txtDueDate" runat="server" MaxLength="10" Width="80px" />
                    <ajax:CalendarExtender ID="ceDueDate" runat="server" Format="MM/dd/yyyy" PopupButtonID="imgDueDate"
                        TargetControlID="txtDueDate" />
                    <asp:ImageButton runat="server" ID="imgDueDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px"
                        CausesValidation="False" />
                    &nbsp;<asp:RequiredFieldValidator ID="rfvDueDate" runat="server" ControlToValidate="txtDueDate"
                        ErrorMessage="Due Date is a required field." ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>&nbsp;
                    <asp:RegularExpressionValidator ID="revDueDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtDueDate" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        ValidationGroup="vsDetail" Width="8px"><</asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblRecStatus" runat="server" Text="Status:" />
                </td>
                <td class="c_textbold" style="color: red;">
                    <asp:DropDownList ID="ddRecStatus" runat="server" AutoPostBack="True">
                        <asp:ListItem Value="Open">New Request</asp:ListItem>
                        <asp:ListItem>Completed</asp:ListItem>
                        <asp:ListItem>Void</asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddRecStatus2" runat="server" AutoPostBack="True">
                        <asp:ListItem>In Process</asp:ListItem>
                        <asp:ListItem>Void</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="txtRoutingStatus" Visible="false" runat="server" Width="1px" Text="N" />
                    <asp:Label ID="lblRoutingStatusDesc" runat="server" Text="Pending Submission" CssClass="c_text"
                        Font-Bold="True" Font-Overline="False" Font-Size="Small" Font-Underline="False"
                        Width="312px" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblIssueDt" runat="server" Text="Issue Date:" />
                </td>
                <td>
                    <asp:TextBox ID="txtIssueDate" runat="server" ReadOnly="true" Width="80px" />
                </td>
            </tr>
            <%--Display the following rows after SMRNo is voided.--%>
            <tr>
                <td class="p_text" valign="top">
                    <asp:Label ID="lblReqVoidRsn" runat="server" Font-Bold="True" ForeColor="Red" Text="* "
                        Visible="false" />
                    <asp:Label ID="lblVoidReason" runat="server" Text="Void Reason:" />
                </td>
                <td class="c_text" colspan="3">
                    <asp:TextBox ID="txtVoidReason" runat="server" MaxLength="300" Rows="3" TextMode="MultiLine"
                        Width="550px" />
                    <asp:RequiredFieldValidator ID="rfvVoidReason" runat="server" ErrorMessage="Void Reason is a required field."
                        ControlToValidate="txtVoidReason" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator><br />
                    <asp:Label ID="lblVoidReasonChar" runat="server" Font-Bold="True" ForeColor="Red" />
                </td>
            </tr>
        </table>
        <br />
        <br />
        <ajax:CascadingDropDown ID="cddCustomer" runat="server" TargetControlID="ddCustomer"
            Category="Customer" PromptText=" " LoadingText="[Loading Customers...]" ServicePath="~/WS/GeneralCDDService.asmx"
            ServiceMethod="GetOEMMfgCABBV" />
        <ajax:CascadingDropDown ID="cddTrialEvent" runat="server" TargetControlID="ddTrialEvent"
            ParentControlID="ddCustomer" Category="TEvent" PromptText=" " LoadingText="[Loading Trial Events...]"
            ServicePath="~/WS/GeneralCDDService.asmx" ServiceMethod="GetSampleTrialEvent" />
        <table width="100%" border="0">
            <tr>
                <td style="width: 30px">
                    <asp:Menu ID="mnuTabs" Height="30px" runat="server" Orientation="Horizontal" StaticEnableDefaultPopOutImage="false"
                        StaticMenuItemStyle-CssClass="tab" StaticSelectedStyle-CssClass="selectedTab"
                        CssClass="tabs">
                        <Items>
                            <asp:MenuItem Text="Sample Information" Value="0" ImageUrl="" Selected="true" />
                            <asp:MenuItem Text="Requirements / Addt'l Info" Value="1" ImageUrl="" />
                            <asp:MenuItem Text="Notifications" Value="2" ImageUrl="" />
                            <asp:MenuItem Text="Shipping Information" Value="3" ImageUrl="" />
                            <asp:MenuItem Text="Communication Board" Value="4" ImageUrl="" />
                        </Items>
                    </asp:Menu>
                </td>
            </tr>
        </table>
        <asp:MultiView ID="mvTabs" runat="server" Visible="true" ActiveViewIndex="0" EnableViewState="true">
            <asp:View ID="vwDetail" runat="server">
                <table width="1000px">
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            <asp:Label ID="lblRequestor" runat="server" Text="Requestor:" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddRequestor" runat="server" />
                            &nbsp;<asp:RequiredFieldValidator ID="rfbRequestor" runat="server" ControlToValidate="ddRequestor"
                                ErrorMessage="Requestor is a required field." Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator></td>
                        <td class="p_text">
                            <asp:Label ID="Label5" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            <asp:Label ID="lblUGNLoc" runat="server" Text="UGN Location:" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddUGNLocation" runat="server" />
                            &nbsp;<asp:RequiredFieldValidator ID="rfvUGNLocation" runat="server" ControlToValidate="ddUGNLocation"
                                ErrorMessage="UGN Location is a required field." Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator></td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label3" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            <asp:Label ID="lblAccountManager" runat="server" Text="Account Manager:" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddAccountManager" runat="server" />
                            &nbsp;<asp:RequiredFieldValidator ID="rfvAccountManager" runat="server" ControlToValidate="ddAccountManager"
                                ErrorMessage="Account Manager is a required field." Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator><asp:CheckBox
                                    ID="cbNotifyActMgr" runat="server" Font-Italic="true" Font-Size="Smaller" Text="Check to notify" />
                        </td>
                        <td class="p_text">
                            <asp:Label ID="Label7" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            <asp:Label ID="lblIntExt" runat="server" Text="Internal/External:" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddIntExt" runat="server">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem>Internal</asp:ListItem>
                                <asp:ListItem>External</asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;<asp:RequiredFieldValidator ID="rfvIntExt" runat="server" ControlToValidate="ddIntExt"
                                ErrorMessage="Select either Internal or External." Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator></td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label11" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            <asp:Label ID="lblQualityEngr" runat="server" Text="Quality Engineer:" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddQualityEngr" runat="server" />
                            &nbsp;<asp:RequiredFieldValidator ID="rfvQualityEngr" runat="server" ControlToValidate="ddQualityEngr"
                                ErrorMessage="Quality Engineer is a required field." Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator></td>
                        <td class="p_text">
                            <asp:Label ID="Label4" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            <asp:Label ID="lblCustomer" runat="server" Text="Customer:" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddCustomer" runat="server" />
                            &nbsp;<asp:RequiredFieldValidator ID="rfvCustomer" runat="server" ControlToValidate="ddCustomer"
                                ErrorMessage="Customer is a required field." Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator></td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label15" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            <asp:Label ID="lblPackaging" runat="server" Text="Packaging Coordinator:" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddPackaging" runat="server" />
                            &nbsp;<asp:RequiredFieldValidator ID="rfvPackaging" runat="server" ControlToValidate="ddPackaging"
                                ErrorMessage="Packaging Coordinator is a required field." Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator><asp:CheckBox
                                    ID="cbNotifyPkgCoord" runat="server" Font-Italic="true" Font-Size="Smaller" Text="Check to notify" />
                        </td>
                        <td class="p_text">
                            <asp:Label ID="Label9" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            <asp:Label ID="lblTrialEvent" runat="server" Text="Trial Event:" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddTrialEvent" runat="server" />
                            &nbsp;<asp:RequiredFieldValidator ID="rfvTrialEvent" runat="server" ControlToValidate="ddTrialEvent"
                                ErrorMessage="Trial Event is a required field." Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator></td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label6" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            <asp:Label ID="lblFormula" runat="server" Text="Formula, Product or Process Description:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtFormula" runat="server" MaxLength="30" Width="250px" />
                            &nbsp;<asp:RequiredFieldValidator ID="rfvFormula" runat="server" ControlToValidate="txtFormula"
                                ErrorMessage="Formula, Product or Process Description is a required field." Font-Bold="False"
                                ValidationGroup="vsDetail"><</asp:RequiredFieldValidator></td>
                        <td class="p_text">
                            <asp:Label ID="lblProjNo" runat="server" Text="D Project No:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtProjNo" runat="server" MaxLength="15" Width="100px" />
                            <ajax:FilteredTextBoxExtender ID="ftbProjNo" runat="server" TargetControlID="txtProjNo"
                                FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,- " />
                            <asp:HyperLink ID="hplkAppropriation" runat="server" Font-Underline="true" ForeColor="Blue"
                                Target="_blank" Visible="false" />
                            &nbsp;
                            <asp:TextBox ID="txtProjectTitle" runat="server" Visible="false" Width="16px" />
                            <asp:TextBox ID="txtDefinedCapex" runat="server" Visible="false" Width="16px" />
                            <asp:TextBox ID="txtProjectStatus" runat="server" Visible="false" Width="16px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="Label12" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            <asp:Label ID="lblRecoveryType" runat="server" Text="Recovery Type:" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddRecoveryType" runat="server" SelectedValue='<%# Bind("RecoveryType") %>'>
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem>Development</asp:ListItem>
                                <asp:ListItem>Plant</asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;<asp:RequiredFieldValidator ID="rfveRecoveryType" runat="server" ControlToValidate="ddRecoveryType"
                                Display="Dynamic" ErrorMessage="Type of Recovery is a required field." ValidationGroup="vsDetail"> < </asp:RequiredFieldValidator></td>
                        <td class="p_text">
                            <asp:Label ID="Label13" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                            <asp:Label ID="lblProductionLevel" runat="server" Text="Production Level:" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddProdLevel" runat="server" SelectedValue='<%# Bind("ProdLevel") %>'>
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem>Pre-Production</asp:ListItem>
                                <asp:ListItem>Mass Production</asp:ListItem>
                                <asp:ListItem>Prototype</asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;<asp:RequiredFieldValidator ID="rfveProdLevel" runat="server" ControlToValidate="ddProdLevel"
                                Display="Dynamic" ErrorMessage="Production Level is a required field." ValidationGroup="vsDetail"> < </asp:RequiredFieldValidator></td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:TextBox ID="hfAcctMgrEmail" runat="server" Visible="false" Width="1px" />
                            <asp:TextBox ID="hfPkgEmail" runat="server" Visible="false" Width="1px" />
                            <asp:TextBox ID="hfQEngrEmail" runat="server" Visible="false" Width="1px" />
                            <asp:TextBox ID="hfRequestorEmail" runat="server" Visible="false" Width="1px" />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan="3">
                            <asp:Button ID="btnSave1" runat="server" Text="Save" CausesValidation="true" ValidationGroup="vsDetail" />
                            <asp:Button ID="btnReset1" runat="server" Text="Reset" />
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" />
                            <asp:Button ID="btnPreview" runat="server" Text="Preview" />
                        </td>
                    </tr>
                </table>
                <asp:ValidationSummary ID="vDetail" runat="server" ValidationGroup="vsDetail" ShowMessageBox="true" />
                <br />
                <asp:Panel ID="Panel1" runat="server" CssClass="collapsePanelHeader" BackColor="DarkSeaGreen">
                    <asp:Image ID="imgPanel1" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                        Height="12px" />&nbsp;
                    <asp:Label ID="lblPanel1" runat="server" CssClass="c_textbold" Text="Sample Information:" />
                </asp:Panel>
                <asp:Panel ID="ContentPanel1" runat="server" CssClass="collapsePanel">
                    <asp:GridView ID="gvPartNo" runat="server" SkinID="StandardGrid" AllowPaging="True"
                        AllowSorting="True" AutoGenerateColumns="False" DataSourceID="odsPartNo" PageSize="30"
                        DataKeyNames="SMRNo,RowID" ShowFooter="True" Width="900px">
                        <Columns>
                            <asp:BoundField DataField="RowID" HeaderText="RowID" ReadOnly="True" SortExpression="RowID"
                                Visible="False" />
                            <asp:TemplateField HeaderText="* Customer Part Number" SortExpression="PartNo" HeaderStyle-HorizontalAlign="Left">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtPartNoEdit" runat="server" MaxLength="40" Width="150px" Text='<%# Bind("PartNo") %>' />
                                    <asp:RequiredFieldValidator ID="rfvePartNo" runat="server" ControlToValidate="txtPartNoEdit"
                                        Display="Dynamic" ErrorMessage="Part Number is a required field." Font-Bold="True"
                                        Font-Size="Small" ValidationGroup="vgEdit"> <</asp:RequiredFieldValidator>
                                    <ajax:FilteredTextBoxExtender ID="ftbPartno" runat="server" TargetControlID="txtPartNoEdit"
                                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-  " />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPartNo" runat="server" Text='<%# Bind("ddPartNo") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtPartNo" runat="server" MaxLength="40" Width="150px" />
                                    <asp:RequiredFieldValidator ID="rfvPartNo" runat="server" ControlToValidate="txtPartNo"
                                        Display="Dynamic" ErrorMessage="Part Number is a required field." Font-Bold="True"
                                        Font-Size="Small" ValidationGroup="vgInsert"> <</asp:RequiredFieldValidator>
                                    <ajax:FilteredTextBoxExtender ID="ftbPartno" runat="server" TargetControlID="txtPartNo"
                                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-  " />
                                </FooterTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="* Design Level" SortExpression="DesignLevel" HeaderStyle-HorizontalAlign="Left">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDesignLevelEdit" runat="server" MaxLength="20" Width="150px"
                                        Text='<%# Bind("DesignLevel") %>' />
                                    <asp:RequiredFieldValidator ID="rfvDesignLevel" runat="server" ControlToValidate="txtDesignLevelEdit"
                                        Display="Dynamic" ErrorMessage="Design Level is a required field." Font-Bold="True"
                                        Font-Size="Small" ValidationGroup="vgEdit"> <</asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDesignLevel" runat="server" Text='<%# Bind("DesignLevel") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtDesignLevel" runat="server" MaxLength="20" Width="150px" />
                                    <asp:RequiredFieldValidator ID="rfvDesignLevel" runat="server" ControlToValidate="txtDesignLevel"
                                        Display="Dynamic" ErrorMessage="Design Level is a required field." Font-Bold="True"
                                        Font-Size="Small" ValidationGroup="vgInsert"> <</asp:RequiredFieldValidator>
                                </FooterTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="* Size / Thickness" SortExpression="SizeThickness"
                                HeaderStyle-HorizontalAlign="Left">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtSizeThicknessEdit" runat="server" MaxLength="50" Width="200px"
                                        Text='<%# Bind("SizeThickness") %>' />
                                    <asp:RequiredFieldValidator ID="rfvSize" runat="server" ControlToValidate="txtSizeThicknessEdit"
                                        Display="Dynamic" ErrorMessage="Size / Thickness is a required field." Font-Bold="True"
                                        Font-Size="Small" ValidationGroup="vgEdit"> <</asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSizeThickness" runat="server" Text='<%# Bind("SizeThickness") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtSizeThickness" runat="server" MaxLength="50" Width="200px" />
                                    <asp:RequiredFieldValidator ID="rfvSize" runat="server" ControlToValidate="txtSizeThickness"
                                        Display="Dynamic" ErrorMessage="Size Thickness is a required field." Font-Bold="True"
                                        Font-Size="Small" ValidationGroup="vgInsert"> <</asp:RequiredFieldValidator>
                                </FooterTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="* Quantity" SortExpression="Qty" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-HorizontalAlign="Center">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtQtyEdit" runat="server" MaxLength="6" Width="60px" Text='<%# Bind("Qty") %>' />
                                    <ajax:FilteredTextBoxExtender ID="ftQty" runat="server" TargetControlID="txtQtyEdit"
                                        FilterType="Custom, Numbers" ValidChars="-," />
                                    <asp:RequiredFieldValidator ID="rfvQty" runat="server" ControlToValidate="txtQtyEdit"
                                        Display="Dynamic" ErrorMessage="Quantity is a required field." Font-Bold="True"
                                        Font-Size="Small" ValidationGroup="vgEdit"> <</asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblQty" runat="server" Text='<%# Bind("Qty") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtQty" runat="server" MaxLength="6" Width="60px" />
                                    <ajax:FilteredTextBoxExtender ID="ftQty" runat="server" TargetControlID="txtQty"
                                        FilterType="Custom, Numbers" ValidChars="-," />
                                    <asp:RequiredFieldValidator ID="rfvQty" runat="server" ControlToValidate="txtQty"
                                        Display="Dynamic" ErrorMessage="Quantity is a required field." Font-Bold="True"
                                        Font-Size="Small" ValidationGroup="vgInsert"> <</asp:RequiredFieldValidator>
                                </FooterTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="* Price (USD)" SortExpression="Price" ItemStyle-HorizontalAlign="Right"
                                HeaderStyle-HorizontalAlign="right">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtPriceEdit" runat="server" Width="80px" MaxLength="16" Text='<%# Bind("Price") %>' />
                                    <ajax:FilteredTextBoxExtender ID="ftPriceEdit" runat="server" TargetControlID="txtPriceEdit"
                                        FilterType="Custom, Numbers" ValidChars="-,." />
                                    <asp:RequiredFieldValidator ID="rfvePrice" runat="server" ControlToValidate="txtPriceEdit"
                                        Display="Dynamic" ErrorMessage="Price is a required field." Font-Bold="True"
                                        Font-Size="Small" ValidationGroup="vgEdit"> <</asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPrice" runat="server" Text='<%# Bind("Price") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtPrice" runat="server" Width="80px" MaxLength="16" />
                                    <ajax:FilteredTextBoxExtender ID="ftPrice" runat="server" TargetControlID="txtPrice"
                                        FilterType="Custom, Numbers" ValidChars="-,." />
                                    <asp:RequiredFieldValidator ID="rfvPriceEdit" runat="server" ControlToValidate="txtPrice"
                                        Display="Dynamic" ErrorMessage="Price is a required field." Font-Bold="True"
                                        Font-Size="Small" ValidationGroup="vgInsert"> <</asp:RequiredFieldValidator>
                                </FooterTemplate>
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="* Selling Recovery<br/>Amount (USD)" SortExpression="RecoveryAmt"
                                ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtRecoveryAmtEdit" runat="server" Width="100px" MaxLength="16"
                                        Text='<%# Bind("RecoveryAmt") %>' />
                                    <ajax:FilteredTextBoxExtender ID="ftRecoveryAmtEdit" runat="server" TargetControlID="txtRecoveryAmtEdit"
                                        FilterType="Custom, Numbers" ValidChars="-,." />
                                    <asp:RequiredFieldValidator ID="rfveRecoveryAmt" runat="server" ControlToValidate="txtRecoveryAmtEdit"
                                        Display="Dynamic" ErrorMessage="Selling Recovery Amount is a required field."
                                        Font-Bold="True" Font-Size="Small" ValidationGroup="vgEdit"> <</asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRecoveryAmt" runat="server" Text='<%# Bind("RecoveryAmt")%>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtRecoveryAmt" runat="server" Width="100px" MaxLength="16" />
                                    <ajax:FilteredTextBoxExtender ID="ftRecoveryAmt" runat="server" TargetControlID="txtRecoveryAmt"
                                        FilterType="Custom, Numbers" ValidChars="-,." />
                                    <asp:RequiredFieldValidator ID="rfvRecoveryAmt" runat="server" ControlToValidate="txtRecoveryAmt"
                                        Display="Dynamic" ErrorMessage="Selling Recovery Amount is a required field."
                                        Font-Bold="True" Font-Size="Small" ValidationGroup="vgInsert"> <</asp:RequiredFieldValidator>
                                </FooterTemplate>
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Purchase Order #" SortExpression="PONo" HeaderStyle-HorizontalAlign="Left">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtPONoEdit" runat="server" MaxLength="15" Width="110px" Text='<%# Bind("PONo") %>' />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPONo" runat="server" Text='<%# Bind("PONo") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtPONo" runat="server" MaxLength="15" Width="110px" />
                                </FooterTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <EditItemTemplate>
                                    <asp:ImageButton ID="ibtnUpdate" runat="server" AlternateText="Save" CausesValidation="True"
                                        CommandName="Update" ImageUrl="~/images/save.jpg" ValidationGroup="vgEdit" />&nbsp;<asp:ImageButton
                                            ID="ibtnCancel" runat="server" AlternateText="Cancel" CausesValidation="False"
                                            CommandName="Cancel" ImageUrl="~/images/cancel.jpg" ValidationGroup="vgEdit" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibtnEdit" runat="server" AlternateText="Edit" CausesValidation="False"
                                        CommandName="Edit" ImageUrl="~/images/edit.jpg" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgInsert"
                                        runat="server" ID="iBtnSaveCust" ImageUrl="~/images/save.jpg" AlternateText="Save" />&nbsp;<asp:ImageButton
                                            ID="iBtnUndoCust" runat="server" CommandName="Undo" CausesValidation="false"
                                            ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                        ImageUrl="~/images/delete.jpg" AlternateText="Delete" />
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:ValidationSummary runat="server" ID="vsEdit" ValidationGroup="vgEdit" ShowMessageBox="true"
                        ShowSummary="true" />
                    <asp:ValidationSummary runat="server" ID="vsInsert" ValidationGroup="vgInsert" ShowMessageBox="true"
                        ShowSummary="true" />
                    <asp:ObjectDataSource ID="odsPartNo" runat="server" DeleteMethod="DeleteSampleMtrlReqPartNo"
                        OldValuesParameterFormatString="original_{0}" SelectMethod="GetSampleMtrlReqPartNo"
                        TypeName="PGMBLL" InsertMethod="InsertSampleMtrlReqPartNo" UpdateMethod="UpdateSampleMtrlReqPartNo">
                        <DeleteParameters>
                            <asp:Parameter Name="SMRNo" Type="Int32" />
                            <asp:Parameter Name="RowID" Type="Int32" />
                            <asp:Parameter Name="original_SMRNo" Type="Int32" />
                            <asp:Parameter Name="original_RowID" Type="Int32" />
                            <asp:Parameter Name="original_PartNo" Type="String" />
                        </DeleteParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="DesignLevel" Type="String" />
                            <asp:Parameter Name="SizeThickness" Type="String" />
                            <asp:Parameter Name="Qty" Type="Decimal" />
                            <asp:Parameter Name="Price" Type="Decimal" />
                            <asp:Parameter Name="RecoveryAmt" Type="Decimal" />
                            <asp:Parameter Name="PONo" Type="String" />
                            <asp:Parameter Name="original_SMRNo" Type="Int32" />
                            <asp:Parameter Name="original_RowID" Type="Int32" />
                            <asp:Parameter Name="original_PartNo" Type="String" />
                            <asp:Parameter Name="PartNo" Type="String" />
                        </UpdateParameters>
                        <SelectParameters>
                            <asp:QueryStringParameter DefaultValue="0" Name="SMRNo" QueryStringField="pSMRNo"
                                Type="Int32" />
                            <asp:Parameter Name="RowID" Type="Int32" DefaultValue="0" />
                        </SelectParameters>
                        <InsertParameters>
                            <asp:Parameter Name="SMRNo" Type="Int32" />
                            <asp:Parameter Name="PartNo" Type="String" />
                            <asp:Parameter Name="DesignLevel" Type="String" />
                            <asp:Parameter Name="SizeThickness" Type="String" />
                            <asp:Parameter Name="Qty" Type="Decimal" />
                            <asp:Parameter Name="Price" Type="Decimal" />
                            <asp:Parameter Name="RecoveryAmt" Type="Decimal" />
                            <asp:Parameter Name="PONo" Type="String" />
                        </InsertParameters>
                    </asp:ObjectDataSource>
                </asp:Panel>
                <ajax:CollapsiblePanelExtender ID="Extender1" runat="server" TargetControlID="ContentPanel1"
                    ExpandControlID="Panel1" CollapseControlID="Panel1" Collapsed="FALSE" TextLabelID="lblPanel1"
                    ExpandedText="Sample Information:" CollapsedText="Sample Information:" ImageControlID="imgPanel1"
                    CollapsedImage="~/images/expand_blue.jpg" ExpandedImage="~/images/collapse_blue.jpg"
                    SuppressPostBack="true">
                </ajax:CollapsiblePanelExtender>
            </asp:View>
            <asp:View ID="vwRequirements" runat="server">
                <asp:Panel ID="Panel2" runat="server" CssClass="collapsePanelHeader" BackColor="DarkSeaGreen"
                    Width="800px">
                    <asp:Image ID="imgPanel2" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                        Height="12px" />&nbsp;
                    <asp:Label ID="lblPanel2" runat="server" CssClass="c_textbold" Text="Packaging Requirements:" />
                </asp:Panel>
                <asp:Panel ID="ContentPanel2" runat="server" CssClass="collapsePanel" Width="800px">
                    <table>
                        <tr>
                            <td class="p_text" valign="top">
                                <asp:Label ID="Label16" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                <asp:Label ID="lblPkgReq" runat="server" Text="Requirements:" />
                            </td>
                            <td class="c_text">
                                <asp:TextBox ID="txtPkgReq" runat="server" MaxLength="300" Rows="3" TextMode="MultiLine"
                                    Width="500px" />
                                <asp:RequiredFieldValidator ID="rfvPkgReq" runat="server" ControlToValidate="txtPkgReq"
                                    ErrorMessage="Requirements is a required field." Font-Bold="False" ValidationGroup="vsPkgReq"><</asp:RequiredFieldValidator><br />
                                <asp:Label ID="lblPkgReqChar" runat="server" Font-Bold="True" ForeColor="Red" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="lblFileDesc1" runat="server" Text="Packaging Layout File Description:" />
                            </td>
                            <td class="c_text">
                                <asp:TextBox ID="txtFileDesc1" runat="server" MaxLength="200" Width="400px" />
                                <asp:RequiredFieldValidator ID="rfvFileDesc1" runat="server" ControlToValidate="txtFileDesc1"
                                    ErrorMessage="Packaging Layout File Description is a required field." Font-Bold="False"
                                    ValidationGroup="vsPkgReq"><</asp:RequiredFieldValidator><br />
                                <asp:Label ID="lblFileDescChar1" runat="server" Font-Bold="True" ForeColor="Red" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text" valign="top">
                                <asp:Label ID="Label21" runat="server" Text="Attach Packaging Layout:" />
                            </td>
                            <td class="c_text">
                                <asp:FileUpload ID="uploadFilePkgReq" runat="server" Height="22px" Width="400px" />
                                <asp:RequiredFieldValidator ID="rfvUploadPkgReq" runat="server" ControlToValidate="uploadFilePKGReq"
                                    ErrorMessage="Packaging Layout is required." Font-Bold="False" ValidationGroup="vsPkgReq"><</asp:RequiredFieldValidator><br />
                                <asp:RegularExpressionValidator ID="revUploadFilePkgReq" runat="server" ErrorMessage="Only *.PDF, *.DOC, *.DOCX, *.XLS, *.XLSX, *.SNP, *.TIF, *.JPG files are allowed!"
                                    ValidationExpression="(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.pdf|.xls|.doc|.xlsx|.docx|.snp|.tif|.jpg|.PDF|.XLS|.DOC|.XLSX|.DOCX|.SNP|.TIF|.JPG)$"
                                    ControlToValidate="uploadFilePkgReq" ValidationGroup="vsPkgReq" Font-Bold="True"
                                    Font-Size="Small" />
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 27px">
                            </td>
                            <td style="height: 27px">
                                <asp:Button ID="btnUploadPkgReq" runat="server" Text="Upload" CausesValidation="true"
                                    ValidationGroup="vsPkgReq" />
                                <asp:Button ID="btnResetPkgReq" runat="server" CausesValidation="False" Text="Reset" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="lblMessageView1" runat="server" Font-Bold="True" ForeColor="Red" Height="16px"
                                    Visible="False" Width="368px" Font-Size="Small" />
                            </td>
                        </tr>
                    </table>
                    <asp:ValidationSummary ID="vsPkgReq" runat="server" ValidationGroup="vsPkgReq" ShowMessageBox="true"
                        ShowSummary="true" />
                    <br />
                    <asp:GridView ID="gvPkgReq" runat="server" AutoGenerateColumns="False" DataKeyNames="SMRNo,DocID"
                        DataSourceID="odsPkgReq" Width="600px" SkinID="StandardGridWOFooter">
                        <Columns>
                            <asp:BoundField DataField="Description" HeaderText="File Description" SortExpression="Description"
                                ItemStyle-Width="500px" HeaderStyle-Width="500px" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" Width="500px" />
                                <ItemStyle Width="500px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TeamMemberName" HeaderText="Uploaded By" SortExpression="TeamMemberName"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="150px" HeaderStyle-Width="150px">
                                <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                <ItemStyle Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DateOfUpload" HeaderText="Upload Date" SortExpression="DateOfUpload"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100px" HeaderStyle-Width="100px">
                                <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                <ItemStyle Width="100px" />
                            </asp:BoundField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:HyperLink runat="server" ID="hlnkPreview" ImageUrl='<%# DisplayImage(DataBinder.Eval(Container, "DataItem.EncodeType").tostring)  %>'
                                        NavigateUrl='<%# "SampleMtrlReqDocument.aspx?pSMRNo=" & DataBinder.Eval (Container.DataItem,"SMRNo").tostring & "&pDocID=" & DataBinder.Eval (Container.DataItem,"DocID").tostring %>'
                                        Target="_blank" Visible='<%# Bind("BinaryFound") %>' ToolTip="Preview" />
                                </ItemTemplate>
                                <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="center" Width="30px" />
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                        ImageUrl="~/images/delete.jpg" AlternateText="Delete" /></ItemTemplate>
                                <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Right" Width="30px" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="odsPkgReq" runat="server" DeleteMethod="DeleteSampleMtrlReqDocuments"
                        OldValuesParameterFormatString="original_{0}" SelectMethod="GetSampleMtrlReqDocuments"
                        TypeName="PGMBLL">
                        <DeleteParameters>
                            <asp:Parameter Name="SMRNo" Type="Int32" />
                            <asp:Parameter Name="DocID" Type="Int32" />
                            <asp:Parameter Name="Section" Type="String" />
                            <asp:Parameter Name="original_SMRNo" Type="Int32" />
                            <asp:Parameter Name="original_DocID" Type="Int32" />
                        </DeleteParameters>
                        <SelectParameters>
                            <asp:QueryStringParameter DefaultValue="0" Name="SMRNo" QueryStringField="pSMRNo"
                                Type="Int32" />
                            <asp:Parameter DefaultValue="0" Name="DocID" Type="Int32" />
                            <asp:Parameter DefaultValue="P" Name="Section" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </asp:Panel>
                <ajax:CollapsiblePanelExtender ID="Extender2" runat="server" TargetControlID="ContentPanel2"
                    ExpandControlID="Panel2" CollapseControlID="Panel2" Collapsed="FALSE" TextLabelID="lblPanel2"
                    ExpandedText="Packaging Requirements:" CollapsedText="Packaging Requirements:"
                    ImageControlID="imgPanel2" CollapsedImage="~/images/expand_blue.jpg" ExpandedImage="~/images/collapse_blue.jpg"
                    SuppressPostBack="true" />
                <br />
                <asp:Panel ID="Panel3" runat="server" CssClass="collapsePanelHeader" BackColor="DarkSeaGreen"
                    Width="800px">
                    <asp:Image ID="imgPanel3" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                        Height="13px" />&nbsp;
                    <asp:Label ID="lblPanel3" runat="server" CssClass="c_textbold" Text="Delivery Instructions:" />
                </asp:Panel>
                <asp:Panel ID="ContentPanel3" runat="server" CssClass="collapsePanel" Width="800px">
                    <table>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="Label17" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                <asp:Label ID="lblShipMethod" runat="server" Text="Shipping Method:" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddShipMethod" runat="server">
                                    <asp:ListItem></asp:ListItem>
                                    <asp:ListItem>Collect</asp:ListItem>
                                    <asp:ListItem>Milk Run</asp:ListItem>
                                    <asp:ListItem>Prepaid</asp:ListItem>
                                    <asp:ListItem>Third Party Billing</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvShipMethod" runat="server" ControlToValidate="ddShipMethod"
                                    ErrorMessage="Shipping Method is a required field." Font-Bold="False" ValidationGroup="vsDeliveryInst"><</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text" valign="top">
                                <asp:Label ID="Label26" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                <asp:Label ID="Label14" runat="server" Text="Special Instructions:" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtSpecInst" runat="server" MaxLength="300" Rows="3" TextMode="MultiLine"
                                    Width="500px" /><br />
                                <asp:Label ID="lblSpecInstChar" runat="server" Font-Bold="True" ForeColor="Red" />
                                <asp:RequiredFieldValidator ID="rfvSpecInst" runat="server" ControlToValidate="txtSpecInst"
                                    ErrorMessage="Special Instructions is a required field." Font-Bold="False" ValidationGroup="vsDeliveryInst"><</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="lblFileDesc2" runat="server" Text="Kick Off Packet File Description:" />
                            </td>
                            <td class="c_text">
                                <asp:TextBox ID="txtFileDesc2" runat="server" MaxLength="200" Width="400px" />
                                <asp:RequiredFieldValidator ID="rfvFileDesc2" runat="server" ControlToValidate="txtFileDesc2"
                                    ErrorMessage="Kick Off Packet File Description is a required field." Font-Bold="False"
                                    ValidationGroup="vsDeliveryInst"><</asp:RequiredFieldValidator><br />
                                <asp:Label ID="lblFileDescChar2" runat="server" Font-Bold="True" ForeColor="Red" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text" valign="top">
                                <asp:Label ID="Label22" runat="server" Text="Attach Kick Off Packet:" />
                            </td>
                            <td class="c_text">
                                <asp:FileUpload ID="uploadFileDeliveryInst" runat="server" Height="22px" Width="400px" />
                                <asp:RequiredFieldValidator ID="rfvUploadDeliveryInst" runat="server" ControlToValidate="uploadFileDeliveryInst"
                                    ErrorMessage="Kick Off Packet is required." Font-Bold="False" ValidationGroup="vsDeliveryInst"><</asp:RequiredFieldValidator><br />
                                <asp:RegularExpressionValidator ID="revUploadFileDeliveryInst" runat="server" ErrorMessage="Only *.PDF, *.DOC, *.DOCX, *.XLS, *.XLSX, *.SNP, *.TIF, *.JPG files are allowed!"
                                    ValidationExpression="(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.pdf|.xls|.doc|.xlsx|.docx|.snp|.tif|.jpg|.PDF|.XLS|.DOC|.XLSX|.DOCX|.SNP|.TIF|.JPG)$"
                                    ControlToValidate="uploadFileDeliveryInst" ValidationGroup="vsDeliveryInst" Font-Bold="True"
                                    Font-Size="Small" />
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 27px">
                            </td>
                            <td style="height: 27px">
                                <asp:Button ID="btnUploadDeliveryInst" runat="server" Text="Upload" CausesValidation="true"
                                    ValidationGroup="vsDeliveryInst" />
                                <asp:Button ID="btnResetDeliveryInst" runat="server" CausesValidation="False" Text="Reset" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="lblMessageView2" runat="server" Font-Bold="True" ForeColor="Red" Height="16px"
                                    Visible="False" Width="368px" Font-Size="Small" />
                            </td>
                        </tr>
                    </table>
                    <asp:ValidationSummary ID="vsDeliveryInst" runat="server" ValidationGroup="vsDeliveryInst"
                        ShowMessageBox="true" ShowSummary="true" />
                    <br />
                    <asp:GridView ID="gvDeliveryInst" runat="server" AutoGenerateColumns="False" DataKeyNames="SMRNo,DocID"
                        DataSourceID="odsDeliveryInst" Width="600px" SkinID="StandardGridWOFooter">
                        <Columns>
                            <asp:BoundField DataField="Description" HeaderText="File Description" SortExpression="Description"
                                ItemStyle-Width="500px" HeaderStyle-Width="500px" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" Width="500px" />
                                <ItemStyle Width="500px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TeamMemberName" HeaderText="Uploaded By" SortExpression="TeamMemberName"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="150px" HeaderStyle-Width="150px">
                                <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                <ItemStyle Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DateOfUpload" HeaderText="Upload Date" SortExpression="DateOfUpload"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100px" HeaderStyle-Width="100px">
                                <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                <ItemStyle Width="100px" />
                            </asp:BoundField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:HyperLink runat="server" ID="hlnkPreview" ImageUrl='<%# DisplayImage(DataBinder.Eval(Container, "DataItem.EncodeType").tostring)  %>'
                                        NavigateUrl='<%# "SampleMtrlReqDocument.aspx?pSMRNo=" & DataBinder.Eval (Container.DataItem,"SMRNo").tostring & "&pDocID=" & DataBinder.Eval (Container.DataItem,"DocID").tostring %>'
                                        Target="_blank" Visible='<%# Bind("BinaryFound") %>' ToolTip="Preview" /></ItemTemplate>
                                <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="center" Width="30px" />
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                        ImageUrl="~/images/delete.jpg" AlternateText="Delete" /></ItemTemplate>
                                <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Right" Width="30px" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="odsDeliveryInst" runat="server" DeleteMethod="DeleteSampleMtrlReqDocuments"
                        OldValuesParameterFormatString="original_{0}" SelectMethod="GetSampleMtrlReqDocuments"
                        TypeName="PGMBLL">
                        <DeleteParameters>
                            <asp:Parameter Name="SMRNo" Type="Int32" />
                            <asp:Parameter Name="DocID" Type="Int32" />
                            <asp:Parameter Name="Section" Type="String" />
                            <asp:Parameter Name="original_SMRNo" Type="Int32" />
                            <asp:Parameter Name="original_DocID" Type="Int32" />
                        </DeleteParameters>
                        <SelectParameters>
                            <asp:QueryStringParameter DefaultValue="0" Name="SMRNo" QueryStringField="pSMRNo"
                                Type="Int32" />
                            <asp:Parameter DefaultValue="0" Name="DocID" Type="Int32" />
                            <asp:Parameter DefaultValue="D" Name="Section" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </asp:Panel>
                <ajax:CollapsiblePanelExtender ID="Extender3" runat="server" TargetControlID="ContentPanel3"
                    ExpandControlID="Panel3" CollapseControlID="Panel3" Collapsed="FALSE" TextLabelID="lblPanel3"
                    ExpandedText="Delivery Instructions:" CollapsedText="Delivery Instructions:"
                    ImageControlID="imgPanel3" CollapsedImage="~/images/expand_blue.jpg" ExpandedImage="~/images/collapse_blue.jpg"
                    SuppressPostBack="true" />
                <br />
                <asp:Panel ID="Panel4" runat="server" CssClass="collapsePanelHeader" BackColor="DarkSeaGreen"
                    Width="800px">
                    <asp:Image ID="imgPanel4" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                        Height="14px" />&nbsp;
                    <asp:Label ID="lblPanel4" runat="server" CssClass="c_textbold" Text="Label Requirements:" />
                </asp:Panel>
                <asp:Panel ID="ContentPanel4" runat="server" CssClass="collapsePanel" Width="800px">
                    <table>
                        <tr>
                            <td class="p_text" valign="top">
                                <asp:Label ID="Label27" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                <asp:Label ID="lblLblReqComments" runat="server" Text="Comments:" />
                            </td>
                            <td class="c_text">
                                <asp:TextBox ID="txtLblReqComments" runat="server" MaxLength="300" Rows="3" TextMode="MultiLine"
                                    Width="500px" />
                                <asp:RequiredFieldValidator ID="rfvLblReqComments" runat="server" ControlToValidate="txtLblReqComments"
                                    ErrorMessage="Label Requirement - Comments is a required field." Font-Bold="False"
                                    ValidationGroup="vsLblReq"><</asp:RequiredFieldValidator><br />
                                <asp:Label ID="lblLblReqCommentsChar" runat="server" Font-Bold="True" ForeColor="Red" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="lblLblReqFileDesc" runat="server" Text="Packaging Label File Description:" />
                            </td>
                            <td class="c_text">
                                <asp:TextBox ID="txtFileDesc3" runat="server" MaxLength="200" Width="400px" />
                                <asp:RequiredFieldValidator ID="rfvLblReqFileDesc" runat="server" ControlToValidate="txtFileDesc3"
                                    ErrorMessage="Packaging Label File Description is a required field." Font-Bold="False"
                                    ValidationGroup="vsLblReq"><</asp:RequiredFieldValidator><br />
                                <asp:Label ID="lblFileDescChar3" runat="server" Font-Bold="True" ForeColor="Red" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text" valign="top">
                                <asp:Label ID="Label23" runat="server" Text="Attach Packaging Label:" />
                            </td>
                            <td class="c_text">
                                <asp:FileUpload ID="uploadFileLblReq" runat="server" Height="22px" Width="400px" />
                                <asp:RequiredFieldValidator ID="rfvUploadLblReq" runat="server" ControlToValidate="uploadFileLblReq"
                                    ErrorMessage="Packaging Layout is required." Font-Bold="False" ValidationGroup="vsLblReq"><</asp:RequiredFieldValidator><br />
                                <asp:RegularExpressionValidator ID="revUploadFileLblReq" runat="server" ErrorMessage="Only *.PDF, *.DOC, *.DOCX, *.XLS, *.XLSX, *.SNP, *.TIF, *.JPG files are allowed!"
                                    ValidationExpression="(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.pdf|.xls|.doc|.xlsx|.docx|.snp|.tif|.jpg|.PDF|.XLS|.DOC|.XLSX|.DOCX|.SNP|.TIF|.JPG)$"
                                    ControlToValidate="uploadFileLblReq" ValidationGroup="vsLblReq" Font-Bold="True"
                                    Font-Size="Small" />
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 27px">
                            </td>
                            <td style="height: 27px">
                                <asp:Button ID="btnUploadLblReq" runat="server" Text="Upload" CausesValidation="true"
                                    ValidationGroup="vsLblReq" />
                                <asp:Button ID="btnResetLblReq" runat="server" CausesValidation="False" Text="Reset" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="lblMessageView3" runat="server" Font-Bold="True" ForeColor="Red" Height="16px"
                                    Visible="False" Width="368px" Font-Size="Small" />
                            </td>
                        </tr>
                    </table>
                    <asp:ValidationSummary ID="vsLblReq" runat="server" ValidationGroup="vsLblReq" ShowMessageBox="true"
                        ShowSummary="true" />
                    <br />
                    <asp:GridView ID="gvLblReq" runat="server" AutoGenerateColumns="False" DataKeyNames="SMRNo,DocID"
                        DataSourceID="odsLblReq" Width="600px" SkinID="StandardGridWOFooter">
                        <Columns>
                            <asp:BoundField DataField="Description" HeaderText="File Description" SortExpression="Description"
                                ItemStyle-Width="500px" HeaderStyle-Width="500px" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" Width="500px" />
                                <ItemStyle Width="500px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TeamMemberName" HeaderText="Uploaded By" SortExpression="TeamMemberName"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="150px" HeaderStyle-Width="150px">
                                <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                <ItemStyle Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DateOfUpload" HeaderText="Upload Date" SortExpression="DateOfUpload"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100px" HeaderStyle-Width="100px">
                                <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                <ItemStyle Width="100px" />
                            </asp:BoundField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:HyperLink runat="server" ID="hlnkPreview" ImageUrl='<%# DisplayImage(DataBinder.Eval(Container, "DataItem.EncodeType").tostring)  %>'
                                        NavigateUrl='<%# "SampleMtrlReqDocument.aspx?pSMRNo=" & DataBinder.Eval (Container.DataItem,"SMRNo").tostring & "&pDocID=" & DataBinder.Eval (Container.DataItem,"DocID").tostring %>'
                                        Target="_blank" Visible='<%# Bind("BinaryFound") %>' ToolTip="Preview" /></ItemTemplate>
                                <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="center" Width="30px" />
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                        ImageUrl="~/images/delete.jpg" AlternateText="Delete" /></ItemTemplate>
                                <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Right" Width="30px" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="odsLblReq" runat="server" DeleteMethod="DeleteSampleMtrlReqDocuments"
                        OldValuesParameterFormatString="original_{0}" SelectMethod="GetSampleMtrlReqDocuments"
                        TypeName="PGMBLL">
                        <DeleteParameters>
                            <asp:Parameter Name="SMRNo" Type="Int32" />
                            <asp:Parameter Name="DocID" Type="Int32" />
                            <asp:Parameter Name="Section" Type="String" />
                            <asp:Parameter Name="original_SMRNo" Type="Int32" />
                            <asp:Parameter Name="original_DocID" Type="Int32" />
                        </DeleteParameters>
                        <SelectParameters>
                            <asp:QueryStringParameter DefaultValue="0" Name="SMRNo" QueryStringField="pSMRNo"
                                Type="Int32" />
                            <asp:Parameter DefaultValue="0" Name="DocID" Type="Int32" />
                            <asp:Parameter DefaultValue="L" Name="Section" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </asp:Panel>
                <ajax:CollapsiblePanelExtender ID="Extender4" runat="server" TargetControlID="ContentPanel4"
                    ExpandControlID="Panel4" CollapseControlID="Panel4" Collapsed="FALSE" TextLabelID="lblPanel4"
                    ExpandedText="Label Requirements:" CollapsedText="Label Requirements:" ImageControlID="imgPanel4"
                    CollapsedImage="~/images/expand_blue.jpg" ExpandedImage="~/images/collapse_blue.jpg"
                    SuppressPostBack="true" />
                <br />
                <asp:Panel ID="Panel5" runat="server" CssClass="collapsePanelHeader" BackColor="DarkSeaGreen"
                    Width="800px">
                    <asp:Image ID="imgPanel5" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                        Height="15px" />&nbsp;
                    <asp:Label ID="lblPanel5" runat="server" CssClass="c_textbold" Text="Invoice Information:" />
                </asp:Panel>
                <asp:Panel ID="ContentPanel5" runat="server" CssClass="collapsePanel" Width="800px">
                    <table>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="Label10" runat="server" Font-Bold="True" ForeColor="Red" Text="* " /><asp:Label
                                    ID="lblInvPONO" runat="server" Text="Purchase Order #:" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtInvPONO" runat="server" Width="150px" MaxLength="20" />
                                <asp:RequiredFieldValidator ID="rfvInvPONO" runat="server" ControlToValidate="txtInvPONO"
                                    ErrorMessage="Purchase Order # is a required field." Font-Bold="False" ValidationGroup="vsInvInfo"><</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="lblInvInfoFileDesc" runat="server" Text="P.O. File Description:" />
                            </td>
                            <td class="c_text">
                                <asp:TextBox ID="txtFileDesc4" runat="server" MaxLength="200" Width="400px" />
                                <asp:RequiredFieldValidator ID="rfvInvInfoFileDesc" runat="server" ControlToValidate="txtFileDesc4"
                                    ErrorMessage="P.O. File Description is a required field." Font-Bold="False" ValidationGroup="vsInvInfo"><</asp:RequiredFieldValidator><br />
                                <asp:Label ID="lblFileDescChar4" runat="server" Font-Bold="True" ForeColor="Red" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text" valign="top">
                                <asp:Label ID="Label24" runat="server" Text="Attach P.O.:" />
                            </td>
                            <td class="c_text">
                                <asp:FileUpload ID="uploadFileInvInfo" runat="server" Height="22px" Width="400px" />
                                <asp:RequiredFieldValidator ID="rfvUploadInvInfo" runat="server" ControlToValidate="uploadFileInvInfo"
                                    ErrorMessage="P.O. document is required." Font-Bold="False" ValidationGroup="vsInvInfo"><</asp:RequiredFieldValidator><br />
                                <asp:RegularExpressionValidator ID="revUploadFileInvInfo" runat="server" ErrorMessage="Only *.PDF, *.DOC, *.DOCX, *.XLS, *.XLSX, *.SNP, *.TIF, *.JPG files are allowed!"
                                    ValidationExpression="(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.pdf|.xls|.doc|.xlsx|.docx|.snp|.tif|.jpg|.PDF|.XLS|.DOC|.XLSX|.DOCX|.SNP|.TIF|.JPG)$"
                                    ControlToValidate="uploadFileInvInfo" ValidationGroup="vsInvInfo" Font-Bold="True"
                                    Font-Size="Small" />
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 27px">
                            </td>
                            <td style="height: 27px">
                                <asp:Button ID="btnUploadInvInfo" runat="server" Text="Upload" CausesValidation="true"
                                    ValidationGroup="vsInvInfo" />
                                <asp:Button ID="btnResetInvInfo" runat="server" CausesValidation="False" Text="Reset" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="lblMessageView4" runat="server" Font-Bold="True" ForeColor="Red" Height="16px"
                                    Visible="False" Width="368px" Font-Size="Small" />
                            </td>
                        </tr>
                    </table>
                    <asp:ValidationSummary ID="vsInvInfo" runat="server" ValidationGroup="vsInvInfo"
                        ShowMessageBox="true" ShowSummary="true" />
                    <br />
                    <asp:GridView ID="gvInvInfo" runat="server" AutoGenerateColumns="False" DataKeyNames="SMRNo,DocID"
                        DataSourceID="odsInvInfo" Width="600px" SkinID="StandardGridWOFooter">
                        <Columns>
                            <asp:BoundField DataField="Description" HeaderText="File Description" SortExpression="Description"
                                ItemStyle-Width="500px" HeaderStyle-Width="500px" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" Width="500px" />
                                <ItemStyle Width="500px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TeamMemberName" HeaderText="Uploaded By" SortExpression="TeamMemberName"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="150px" HeaderStyle-Width="150px">
                                <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                <ItemStyle Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DateOfUpload" HeaderText="Upload Date" SortExpression="DateOfUpload"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100px" HeaderStyle-Width="100px">
                                <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                <ItemStyle Width="100px" />
                            </asp:BoundField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:HyperLink runat="server" ID="hlnkPreview" ImageUrl='<%# DisplayImage(DataBinder.Eval(Container, "DataItem.EncodeType").tostring)  %>'
                                        NavigateUrl='<%# "SampleMtrlReqDocument.aspx?pSMRNo=" & DataBinder.Eval (Container.DataItem,"SMRNo").tostring & "&pDocID=" & DataBinder.Eval (Container.DataItem,"DocID").tostring %>'
                                        Target="_blank" Visible='<%# Bind("BinaryFound") %>' ToolTip="Preview" /></ItemTemplate>
                                <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="center" Width="30px" />
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                        ImageUrl="~/images/delete.jpg" AlternateText="Delete" /></ItemTemplate>
                                <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Right" Width="30px" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="odsInvInfo" runat="server" DeleteMethod="DeleteSampleMtrlReqDocuments"
                        OldValuesParameterFormatString="original_{0}" SelectMethod="GetSampleMtrlReqDocuments"
                        TypeName="PGMBLL">
                        <DeleteParameters>
                            <asp:Parameter Name="SMRNo" Type="Int32" />
                            <asp:Parameter Name="DocID" Type="Int32" />
                            <asp:Parameter Name="Section" Type="String" />
                            <asp:Parameter Name="original_SMRNo" Type="Int32" />
                            <asp:Parameter Name="original_DocID" Type="Int32" />
                        </DeleteParameters>
                        <SelectParameters>
                            <asp:QueryStringParameter DefaultValue="0" Name="SMRNo" QueryStringField="pSMRNo"
                                Type="Int32" />
                            <asp:Parameter DefaultValue="0" Name="DocID" Type="Int32" />
                            <asp:Parameter DefaultValue="I" Name="Section" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </asp:Panel>
                <ajax:CollapsiblePanelExtender ID="Extender5" runat="server" TargetControlID="ContentPanel5"
                    ExpandControlID="Panel5" CollapseControlID="Panel5" Collapsed="FALSE" TextLabelID="lblPanel5"
                    ExpandedText="Invoice Information:" CollapsedText="Invoice Information:" ImageControlID="imgPanel5"
                    CollapsedImage="~/images/expand_blue.jpg" ExpandedImage="~/images/collapse_blue.jpg"
                    SuppressPostBack="true" />
                <br />
                <asp:Panel ID="Panel6" runat="server" CssClass="collapsePanelHeader" BackColor="DarkSeaGreen"
                    Width="800px">
                    <asp:Image ID="imgPanel6" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                        Height="15px" />&nbsp;
                    <asp:Label ID="lblPanel6" runat="server" CssClass="c_textbold" Text="Additional Supporting Documents:" />
                </asp:Panel>
                <asp:Panel ID="ContentPanel6" runat="server" CssClass="collapsePanel" Width="800px">
                    <br />
                    <table>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="lblAddtlDocsFileDesc" runat="server" Text="Addt’l Supporting Document File Description:" />
                            </td>
                            <td class="c_text">
                                <asp:TextBox ID="txtFileDesc5" runat="server" MaxLength="200" Width="400px" />
                                <asp:RequiredFieldValidator ID="rfvAddtlDocsFileDesc" runat="server" ControlToValidate="txtFileDesc5"
                                    ErrorMessage="Addt’l Supporting Document File Description is a required field"
                                    Font-Bold="False" ValidationGroup="vsAddtlDocs"><</asp:RequiredFieldValidator><br />
                                <asp:Label ID="lblFileDescChar5" runat="server" Font-Bold="True" ForeColor="Red" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text" valign="top">
                                <asp:Label ID="Label20" runat="server" Text="Attach Addt’l Supporting Documents:" />
                            </td>
                            <td class="c_text">
                                <asp:FileUpload ID="uploadFileAddtlDocs" runat="server" Height="22px" Width="400px" />
                                <asp:RequiredFieldValidator ID="rfvUploadAddtlDocs" runat="server" ControlToValidate="uploadFileAddtlDocs"
                                    ErrorMessage="Addt’l Supporting Document is required." Font-Bold="False" ValidationGroup="vsAddtlDocs"><</asp:RequiredFieldValidator><br />
                                <asp:RegularExpressionValidator ID="revUploadFileAddtlDocs" runat="server" ErrorMessage="Only *.PDF, *.DOC, *.DOCX, *.XLS, *.XLSX, *.SNP, *.TIF, *.JPG files are allowed!"
                                    ValidationExpression="(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.pdf|.xls|.doc|.xlsx|.docx|.snp|.tif|.jpg|.PDF|.XLS|.DOC|.XLSX|.DOCX|.SNP|.TIF|.JPG)$"
                                    ControlToValidate="uploadFileAddtlDocs" ValidationGroup="vsAddtlDocs" Font-Bold="True"
                                    Font-Size="Small" />
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 27px">
                            </td>
                            <td style="height: 27px">
                                <asp:Button ID="btnUploadAddtlDocs" runat="server" Text="Upload" CausesValidation="true"
                                    ValidationGroup="vsAddtlDocs" />
                                <asp:Button ID="btnResetAddtlDocs" runat="server" CausesValidation="False" Text="Reset" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="lblMessageView5" runat="server" Font-Bold="True" ForeColor="Red" Height="16px"
                                    Visible="False" Width="368px" Font-Size="Small" />
                            </td>
                        </tr>
                    </table>
                    <asp:ValidationSummary ID="vsAddtlDocs" runat="server" ValidationGroup="vsAddtlDocs"
                        ShowMessageBox="true" ShowSummary="true" />
                    <br />
                    <asp:GridView ID="gvAddtlDocs" runat="server" AutoGenerateColumns="False" DataKeyNames="SMRNo,DocID"
                        DataSourceID="odsAddtlDocs" Width="600px" SkinID="StandardGridWOFooter">
                        <Columns>
                            <asp:BoundField DataField="Description" HeaderText="File Description" SortExpression="Description"
                                ItemStyle-Width="500px" HeaderStyle-Width="500px" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" Width="500px" />
                                <ItemStyle Width="500px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TeamMemberName" HeaderText="Uploaded By" SortExpression="TeamMemberName"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="150px" HeaderStyle-Width="150px">
                                <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                <ItemStyle Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DateOfUpload" HeaderText="Upload Date" SortExpression="DateOfUpload"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100px" HeaderStyle-Width="100px">
                                <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                <ItemStyle Width="100px" />
                            </asp:BoundField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:HyperLink runat="server" ID="hlnkPreview" ImageUrl='<%# DisplayImage(DataBinder.Eval(Container, "DataItem.EncodeType").tostring)  %>'
                                        NavigateUrl='<%# "SampleMtrlReqDocument.aspx?pSMRNo=" & DataBinder.Eval (Container.DataItem,"SMRNo").tostring & "&pDocID=" & DataBinder.Eval (Container.DataItem,"DocID").tostring %>'
                                        Target="_blank" Visible='<%# Bind("BinaryFound") %>' ToolTip="Preview" />
                                </ItemTemplate>
                                <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="center" Width="30px" />
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                        ImageUrl="~/images/delete.jpg" AlternateText="Delete" /></ItemTemplate>
                                <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Right" Width="30px" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="odsAddtlDocs" runat="server" DeleteMethod="DeleteSampleMtrlReqDocuments"
                        OldValuesParameterFormatString="original_{0}" SelectMethod="GetSampleMtrlReqDocuments"
                        TypeName="PGMBLL">
                        <DeleteParameters>
                            <asp:Parameter Name="SMRNo" Type="Int32" />
                            <asp:Parameter Name="DocID" Type="Int32" />
                            <asp:Parameter Name="Section" Type="String" />
                            <asp:Parameter Name="original_SMRNo" Type="Int32" />
                            <asp:Parameter Name="original_DocID" Type="Int32" />
                        </DeleteParameters>
                        <SelectParameters>
                            <asp:QueryStringParameter DefaultValue="0" Name="SMRNo" QueryStringField="pSMRNo"
                                Type="Int32" />
                            <asp:Parameter DefaultValue="0" Name="DocID" Type="Int32" />
                            <asp:Parameter DefaultValue="A" Name="Section" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </asp:Panel>
                <ajax:CollapsiblePanelExtender ID="Extender6" runat="server" TargetControlID="ContentPanel6"
                    ExpandControlID="Panel6" CollapseControlID="Panel6" Collapsed="FALSE" TextLabelID="lblPanel6"
                    ExpandedText="Additional Supporting Documents:" CollapsedText="Additional Supporting Documents:"
                    ImageControlID="imgPanel6" CollapsedImage="~/images/expand_blue.jpg" ExpandedImage="~/images/collapse_blue.jpg"
                    SuppressPostBack="true" />
            </asp:View>
            <asp:View ID="vwApprovalStatus" runat="server">
                <br />
                <asp:Label ID="lblReqAppComments" runat="server" Visible="false" CssClass="c_text"
                    Font-Bold="true" />
                <asp:GridView ID="gvApprovers" runat="server" AutoGenerateColumns="False" DataKeyNames="SMRNo,SeqNo,OrigTeamMemberID,TeamMemberID"
                    OnRowUpdating="gvApprovers_RowUpdating" OnRowDataBound="gvApprovers_RowDataBound"
                    DataSourceID="odsApprovers" Width="1015px" RowStyle-Height="20px" RowStyle-CssClass="c_text"
                    HeaderStyle-CssClass="c_text" SkinID="StandardGrid">
                    <RowStyle CssClass="c_text" Height="20px" />
                    <Columns>
                        <asp:TemplateField HeaderText="Review Level" SortExpression="SeqNo" Visible="false">
                            <EditItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("SeqNo") %>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("SeqNo") %>' />
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
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("TeamMemberName") %>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("TeamMemberName") %>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddResponsibleTM" runat="server" DataSource='<%# commonFunctions.GetTeamMemberBySubscriptionByUGNFacility(147,ddUGNLocation.SelectedValue) %>'
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
                                <asp:Label ID="txtDateNotified" runat="server" Text='<%# Bind("DateNotified") %>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblDateNotified" runat="server" Text='<%# Bind("DateNotified") %>' />
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
                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>' />
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
                                    Text='<%# Bind("Comments") %>' Width="300px" />
                                <asp:RequiredFieldValidator ID="rfvComments" runat="server" ControlToValidate="txtAppComments"
                                    ErrorMessage="Comments is a required field when approving for another team member."
                                    Font-Bold="True" ValidationGroup="EditApprovalInfo"><</asp:RequiredFieldValidator><asp:TextBox
                                        ID="txtTeamMemberID" runat="server" Text='<%# Eval("TeamMemberID") %>' ReadOnly="true"
                                        Width="0px" Visible="false" /><asp:TextBox ID="hfSeqNo" runat="server" Text='<%# Eval("SeqNo") %>'
                                            ReadOnly="true" Width="0px" Visible="false" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Comments") %>' />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="400px" />
                            <ItemStyle Wrap="True" />
                            <FooterTemplate>
                                <asp:Label ID="lblMsg2" runat="server" Text="<< Use this row to add a Shipping/EDI Coordinator. >>"
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
                <asp:ObjectDataSource ID="odsApprovers" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetSampleMtrlReqApproval" TypeName="PGMBLL" UpdateMethod="UpdateSampleMtrlReqApproval"
                    DeleteMethod="DeleteSampleMtrlReqApproval" InsertMethod="InsertSampleMtrlReqAddLvl1Aprvl">
                    <InsertParameters>
                        <asp:QueryStringParameter Name="SMRNo" QueryStringField="pSMRNo" Type="Int32" DefaultValue="0" />
                        <asp:Parameter Name="SeqNo" Type="Int32" DefaultValue="0" />
                        <asp:Parameter Name="ResponsibleTMID" Type="Int32" DefaultValue="0" />
                        <asp:Parameter Name="OriginalTMID" Type="Int32" DefaultValue="0" />
                    </InsertParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="Status" Type="String" />
                        <asp:Parameter Name="Comments" Type="String" />
                        <asp:Parameter Name="original_SMRNo" Type="Int32" />
                        <asp:Parameter Name="original_SeqNo" Type="Int32" />
                        <asp:Parameter Name="original_TeamMemberID" Type="Int32" />
                        <asp:Parameter Name="original_OrigTeamMemberID" Type="Int32" />
                        <asp:Parameter Name="DateNotified" Type="String" />
                        <asp:Parameter Name="SeqNo" Type="Int32" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="Status" Type="String" />
                        <asp:Parameter Name="Comments" Type="String" />
                        <asp:Parameter Name="original_SMRNo" Type="Int32" />
                        <asp:Parameter Name="original_SeqNo" Type="Int32" />
                        <asp:Parameter Name="original_TeamMemberID" Type="Int32" />
                        <asp:Parameter Name="original_OrigTeamMemberID" Type="Int32" />
                        <asp:Parameter Name="DateNotified" Type="String" />
                        <asp:Parameter Name="TeamMemberName" Type="String" />
                    </UpdateParameters>
                    <SelectParameters>
                        <asp:QueryStringParameter Name="SMRNo" QueryStringField="pSMRNo" Type="Int32" DefaultValue="0" />
                        <asp:Parameter DefaultValue="0" Name="Sequence" Type="Int32" />
                        <asp:Parameter DefaultValue="0" Name="ResponsibleTMID" Type="Int32" />
                        <asp:Parameter DefaultValue="" Name="PendingApprovals" Type="Boolean" />
                        <asp:Parameter Name="RejectedTM" Type="Boolean" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <br />
                <table>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Button ID="btnBuildApproval" runat="server" Text="Build Routing" />&nbsp;<asp:Button
                                ID="btnFwdApproval" runat="server" Text="Submit for Review" Width="130px" />
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vwShipInfo" runat="server">
                <br />
                <table>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblReqShipEDICoord" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="* " />
                            <asp:Label ID="lblShipEDICoord" runat="server" Text="Shipping/EDI Coordinator:" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddShipEDICoord" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvShipEDICoord" runat="server" ControlToValidate="ddShipEDICoord"
                                Display="Dynamic" ErrorMessage="Shipping/EDI Coordinator is a required field."
                                Font-Bold="True" Font-Size="Small" ValidationGroup="vsShipEdi"> <</asp:RequiredFieldValidator>
                            <asp:TextBox ID="hfShipEDICoordEmail" runat="server" Visible="false" />
                            <asp:TextBox ID="hfShipEdiCoordName" runat="server" Visible="false" />
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="Panel7" runat="server" CssClass="collapsePanelHeader" BackColor="DarkSeaGreen"
                    Width="800px">
                    <asp:Image ID="imgPanel7" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                        Height="15px" />&nbsp;
                    <asp:Label ID="lblPanel7" runat="server" CssClass="c_textbold" Text="Shipping Documents:" />
                </asp:Panel>
                <asp:Panel ID="ContentPanel7" runat="server" CssClass="collapsePanel" Width="800px">
                    <br />
                    <table>
                        <tr>
                            <td class="p_text">
                                <asp:Label ID="lblShipDocFileDesc" runat="server" Text="Shipping Document File Description:" />
                            </td>
                            <td class="c_text">
                                <asp:TextBox ID="txtFileDesc6" runat="server" MaxLength="200" Width="400px" />
                                <asp:RequiredFieldValidator ID="rfvShipDocFileDesc" runat="server" ControlToValidate="txtFileDesc6"
                                    ErrorMessage="Shipping Document File Description is a required field" Font-Bold="False"
                                    ValidationGroup="vsShipDocs"><</asp:RequiredFieldValidator><br />
                                <asp:Label ID="lblFileDescChar6" runat="server" Font-Bold="True" ForeColor="Red" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text" valign="top">
                                <asp:Label ID="Label25" runat="server" Text="Attach Shipping Document:" />
                            </td>
                            <td class="c_text">
                                <asp:FileUpload ID="uploadFileShipDoc" runat="server" Height="22px" Width="400px" />
                                <asp:RequiredFieldValidator ID="rfvUploadShipDoc" runat="server" ControlToValidate="uploadFileShipDoc"
                                    ErrorMessage="Shipping Document is required." Font-Bold="False" ValidationGroup="vsShipDocs"><</asp:RequiredFieldValidator><br />
                                <asp:RegularExpressionValidator ID="revUploadFileShipDoc" runat="server" ErrorMessage="Only *.PDF, *.DOC, *.DOCX, *.XLS, *.XLSX, *.SNP, *.TIF, *.JPG files are allowed!"
                                    ValidationExpression="(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.pdf|.xls|.doc|.xlsx|.docx|.snp|.tif|.jpg|.PDF|.XLS|.DOC|.XLSX|.DOCX|.SNP|.TIF|.JPG)$"
                                    ControlToValidate="uploadFileShipDoc" ValidationGroup="vsShipDocs" Font-Bold="True"
                                    Font-Size="Small" />
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 27px">
                            </td>
                            <td style="height: 27px">
                                <asp:Button ID="btnUploadShipDoc" runat="server" Text="Upload" CausesValidation="true"
                                    ValidationGroup="vsShipDocs" />
                                <asp:Button ID="btnResetShipDoc" runat="server" CausesValidation="False" Text="Reset" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="lblMessageView6" runat="server" Font-Bold="True" ForeColor="Red" Height="16px"
                                    Visible="False" Width="368px" Font-Size="Small" />
                            </td>
                        </tr>
                    </table>
                    <asp:ValidationSummary ID="vsShipDocs" runat="server" ValidationGroup="vsShipDocs"
                        ShowMessageBox="true" ShowSummary="true" />
                    <br />
                    <asp:GridView ID="gvShipDocs" runat="server" AutoGenerateColumns="False" DataKeyNames="SMRNo,DocID"
                        DataSourceID="odsShipDocs" Width="600px" SkinID="StandardGridWOFooter">
                        <Columns>
                            <asp:BoundField DataField="Description" HeaderText="File Description" SortExpression="Description"
                                ItemStyle-Width="500px" HeaderStyle-Width="500px" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left" Width="500px" />
                                <ItemStyle Width="500px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TeamMemberName" HeaderText="Uploaded By" SortExpression="TeamMemberName"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="150px" HeaderStyle-Width="150px">
                                <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                <ItemStyle Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DateOfUpload" HeaderText="Upload Date" SortExpression="DateOfUpload"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100px" HeaderStyle-Width="100px">
                                <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                <ItemStyle Width="100px" />
                            </asp:BoundField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:HyperLink runat="server" ID="hlnkPreview" ImageUrl='<%# DisplayImage(DataBinder.Eval(Container, "DataItem.EncodeType").tostring)  %>'
                                        NavigateUrl='<%# "SampleMtrlReqDocument.aspx?pSMRNo=" & DataBinder.Eval (Container.DataItem,"SMRNo").tostring & "&pDocID=" & DataBinder.Eval (Container.DataItem,"DocID").tostring %>'
                                        Target="_blank" Visible='<%# Bind("BinaryFound") %>' ToolTip="Preview" />
                                </ItemTemplate>
                                <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="center" Width="30px" />
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                        ImageUrl="~/images/delete.jpg" AlternateText="Delete" /></ItemTemplate>
                                <HeaderStyle Width="30px" />
                                <ItemStyle HorizontalAlign="Right" Width="30px" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="odsShipDocs" runat="server" DeleteMethod="DeleteSampleMtrlReqDocuments"
                        OldValuesParameterFormatString="original_{0}" SelectMethod="GetSampleMtrlReqDocuments"
                        TypeName="PGMBLL">
                        <DeleteParameters>
                            <asp:Parameter Name="SMRNo" Type="Int32" />
                            <asp:Parameter Name="DocID" Type="Int32" />
                            <asp:Parameter Name="Section" Type="String" />
                            <asp:Parameter Name="original_SMRNo" Type="Int32" />
                            <asp:Parameter Name="original_DocID" Type="Int32" />
                        </DeleteParameters>
                        <SelectParameters>
                            <asp:QueryStringParameter DefaultValue="0" Name="SMRNo" QueryStringField="pSMRNo"
                                Type="Int32" />
                            <asp:Parameter DefaultValue="0" Name="DocID" Type="Int32" />
                            <asp:Parameter DefaultValue="S" Name="Section" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </asp:Panel>
                <ajax:CollapsiblePanelExtender ID="Extender7" runat="server" TargetControlID="ContentPanel7"
                    ExpandControlID="Panel7" CollapseControlID="Panel7" Collapsed="FALSE" TextLabelID="lblPanel7"
                    ExpandedText="Shipping Documents:" CollapsedText="Shipping Documents:" ImageControlID="imgPanel7"
                    CollapsedImage="~/images/expand_blue.jpg" ExpandedImage="~/images/collapse_blue.jpg"
                    SuppressPostBack="true" />
                <br />
                <asp:Panel ID="Panel8" runat="server" CssClass="collapsePanelHeader" BackColor="DarkSeaGreen"
                    Width="800px">
                    <asp:Image ID="imgPanel8" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                        Height="15px" />&nbsp;
                    <asp:Label ID="lblPanel8" runat="server" CssClass="c_textbold" Text="Shipping Info:" />
                </asp:Panel>
                <asp:Panel ID="ContentPanel8" runat="server" CssClass="collapsePanel" Width="800px">
                    <br />
                    <asp:GridView ID="gvShipping" runat="server" SkinID="StandardGrid" AllowPaging="True"
                        AllowSorting="True" AutoGenerateColumns="False" DataSourceID="odsShipping" PageSize="30"
                        DataKeyNames="SMRNo,RowID" ShowFooter="True" Width="550px">
                        <Columns>
                            <asp:BoundField DataField="RowID" HeaderText="RowID" ReadOnly="True" SortExpression="RowID"
                                Visible="False" />
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="* Shipper Number"
                                SortExpression="ShipperNo">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtShipperNoEdit" runat="server" MaxLength="15" Text='<%# Bind("ShipperNo") %>'
                                        Width="200px" />
                                    <asp:RequiredFieldValidator ID="rfveShipping" runat="server" ControlToValidate="txtShipperNoEdit"
                                        Display="Dynamic" ErrorMessage="Shipper Number is a required field." Font-Bold="True"
                                        Font-Size="Small" ValidationGroup="vgEdit2"> <</asp:RequiredFieldValidator>
                                    <ajax:FilteredTextBoxExtender ID="ftbShipperNoEdit" runat="server" TargetControlID="txtShipperNoEdit"
                                        FilterType="Numbers" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblShipping" runat="server" Text='<%# Bind("ShipperNo") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtShipperNo" runat="server" MaxLength="15" Width="200px" />
                                    <asp:RequiredFieldValidator ID="rfvShipperNo" runat="server" ControlToValidate="txtShipperNo"
                                        Display="Dynamic" ErrorMessage="Shipper Number is a required field." Font-Bold="True"
                                        Font-Size="Small" ValidationGroup="vgInsert2"> <</asp:RequiredFieldValidator>
                                    <ajax:FilteredTextBoxExtender ID="ftbShipperNo" runat="server" TargetControlID="txtShipperNo"
                                        FilterType="Numbers" />
                                </FooterTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="* Total Shipping Cost (USD)" SortExpression="TotalShippingCost">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtTotalShippingCostEdit" runat="server" MaxLength="16" Text='<%# Bind("TotalShippingCost") %>'
                                        Width="100px" />
                                    <ajax:FilteredTextBoxExtender ID="ftTotalShippingCostEdit" runat="server" FilterType="Custom, Numbers"
                                        TargetControlID="txtTotalShippingCostEdit" ValidChars="-,." />
                                    <asp:RequiredFieldValidator ID="rfveTotalShippingCost" runat="server" ControlToValidate="txtTotalShippingCostEdit"
                                        Display="Dynamic" ErrorMessage="Total Shipping Cost is a required field." Font-Bold="True"
                                        Font-Size="Small" ValidationGroup="vgEdit2"> < </asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalShippingCost" runat="server" Text='<%# Bind("TotalShippingCost") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtTotalShippingCost" runat="server" MaxLength="16" Width="100px" />
                                    <ajax:FilteredTextBoxExtender ID="ftTotalShippingCost" runat="server" FilterType="Custom, Numbers"
                                        TargetControlID="txtTotalShippingCost" ValidChars="-,." />
                                    <asp:RequiredFieldValidator ID="rfvTotalShippingCostEdit" runat="server" ControlToValidate="txtTotalShippingCost"
                                        Display="Dynamic" ErrorMessage="Total Shipping Cost is a required field." Font-Bold="True"
                                        Font-Size="Small" ValidationGroup="vgInsert2"> &lt; </asp:RequiredFieldValidator>
                                </FooterTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="* Freight Bill ProNo"
                                SortExpression="FreightBillProNo">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtFreightBillProNoEdit" runat="server" MaxLength="25" Text='<%# Bind("FreightBillProNo") %>'
                                        Width="150px" />
                                    <asp:RequiredFieldValidator ID="rfveFreightBillProNo" runat="server" ControlToValidate="txtFreightBillProNoEdit"
                                        Display="Dynamic" ErrorMessage="Freight Bill ProNo is a required field." Font-Bold="True"
                                        Font-Size="Small" ValidationGroup="vgEdit2"> &lt; </asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblFreightBillProNo" runat="server" Text='<%# Bind("FreightBillProNo") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtFreightBillProNo" runat="server" MaxLength="25" Width="150px" />
                                    <asp:RequiredFieldValidator ID="rfvFreightBillProNo" runat="server" ControlToValidate="txtFreightBillProNo"
                                        Display="Dynamic" ErrorMessage="Freight Bill ProNo is a required field." Font-Bold="True"
                                        Font-Size="Small" ValidationGroup="vgInsert2"> &lt; </asp:RequiredFieldValidator>
                                </FooterTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <EditItemTemplate>
                                    <asp:ImageButton ID="ibtnUpdate" runat="server" AlternateText="Save" CausesValidation="True"
                                        CommandName="Update" ImageUrl="~/images/save.jpg" ValidationGroup="vgEdit2" />&nbsp;<asp:ImageButton
                                            ID="ibtnCancel" runat="server" AlternateText="Cancel" CausesValidation="False"
                                            CommandName="Cancel" ImageUrl="~/images/cancel.jpg" ValidationGroup="vgEdit2" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibtnEdit" runat="server" AlternateText="Edit" CausesValidation="False"
                                        CommandName="Edit" ImageUrl="~/images/edit.jpg" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgInsert2"
                                        runat="server" ID="iBtnSaveCust" ImageUrl="~/images/save.jpg" AlternateText="Save" />&nbsp;<asp:ImageButton
                                            ID="iBtnUndoCust" runat="server" CommandName="Undo" CausesValidation="false"
                                            ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                        ImageUrl="~/images/delete.jpg" AlternateText="Delete" />
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:ValidationSummary runat="server" ID="vsEdit2" ValidationGroup="vgEdit2" ShowMessageBox="true"
                        ShowSummary="true" />
                    <asp:ValidationSummary runat="server" ID="vsInsert2" ValidationGroup="vgInsert2"
                        ShowMessageBox="true" ShowSummary="true" />
                    <asp:ValidationSummary runat="server" ID="vsShipEdi" ValidationGroup="vsShipEdi"
                        ShowMessageBox="true" ShowSummary="true" />
                    <asp:ObjectDataSource ID="odsShipping" runat="server" DeleteMethod="DeleteSampleMtrlReqShipping"
                        OldValuesParameterFormatString="original_{0}" SelectMethod="GetSampleMtrlReqShipping"
                        TypeName="PGMBLL" InsertMethod="InsertSampleMtrlReqShipping" UpdateMethod="UpdateSampleMtrlReqShipping">
                        <DeleteParameters>
                            <asp:Parameter Name="SMRNo" Type="Int32" />
                            <asp:Parameter Name="RowID" Type="Int32" />
                            <asp:Parameter Name="original_SMRNo" Type="Int32" />
                            <asp:Parameter Name="original_RowID" Type="Int32" />
                        </DeleteParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="ShipperNo" Type="Int32" />
                            <asp:Parameter Name="TotalShippingCost" Type="Decimal" />
                            <asp:Parameter Name="FreightBillProNo" Type="String" />
                            <asp:Parameter Name="original_SMRNo" Type="Int32" />
                            <asp:Parameter Name="original_RowID" Type="Int32" />
                        </UpdateParameters>
                        <SelectParameters>
                            <asp:QueryStringParameter DefaultValue="0" Name="SMRNo" QueryStringField="pSMRNo"
                                Type="Int32" />
                            <asp:Parameter Name="RowID" Type="Int32" DefaultValue="0" />
                        </SelectParameters>
                        <InsertParameters>
                            <asp:Parameter Name="SMRNo" Type="Int32" />
                            <asp:Parameter Name="ShipperNo" Type="Int32" />
                            <asp:Parameter Name="TotalShippingCost" Type="Decimal" />
                            <asp:Parameter Name="FreightBillProNo" Type="String" />
                        </InsertParameters>
                    </asp:ObjectDataSource>
                </asp:Panel>
                <ajax:CollapsiblePanelExtender ID="Extender8" runat="server" TargetControlID="ContentPanel8"
                    ExpandControlID="Panel8" CollapseControlID="Panel8" Collapsed="FALSE" TextLabelID="lblPanel8"
                    ExpandedText="Shipping Info:" CollapsedText="Shipping Info:" ImageControlID="imgPanel8"
                    CollapsedImage="~/images/expand_blue.jpg" ExpandedImage="~/images/collapse_blue.jpg"
                    SuppressPostBack="true" />
                <br />
                <table>
                    <tr>
                        <td class="p_text" valign="top">
                            <asp:Label ID="lblShippingComments" runat="server" Text="Comments:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtShippingComments" runat="server" MaxLength="300" Rows="3" TextMode="MultiLine"
                                Width="500px" /><br />
                            <asp:Label ID="lblShippingCommentsChar" runat="server" Font-Bold="True" ForeColor="Red" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnSubmitCmplt" runat="server" Text="Submit Completion" CausesValidation="true" />
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
                            <asp:Label ID="Label18" runat="server" Text="Question / Comment:" />
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
                            <asp:Label ID="lblReqReply" runat="server" Text="* " ForeColor="Red" />
                            <asp:Label ID="Label19" runat="server" Text="Reply / Comments:" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtReply" runat="server" Rows="3" TextMode="MultiLine" Width="550px" />
                            <asp:RequiredFieldValidator ID="rfvReply" runat="server" ErrorMessage="Reply / Comments is a required field."
                                ValidationGroup="ReplyComments" ControlToValidate="txtReply"><</asp:RequiredFieldValidator><br />
                            <asp:Label ID="lblReply" runat="server" Font-Bold="True" ForeColor="Red" />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 26px">
                        </td>
                        <td style="height: 26px">
                            <asp:Button ID="btnSaveCB" runat="server" Text="Submit" CausesValidation="true" ValidationGroup="ReplyComments" />
                            <asp:Button ID="btnResetCB" runat="server" Text="Reset" CausesValidation="False" />
                        </td>
                    </tr>
                </table>
                <asp:ValidationSummary ID="vsReplyComments" runat="server" ValidationGroup="ReplyComments"
                    ShowMessageBox="true" ShowSummary="true" />
                <br />
                <asp:GridView ID="gvQuestion" runat="server" AutoGenerateColumns="False" DataKeyNames="SMRNo,RSSID"
                    DataSourceID="odsQuestion" OnRowDataBound="gvQuestion_RowDataBound" Width="900px"
                    RowStyle-BorderStyle="None" SkinID="CommBoardRSS">
                    <RowStyle BorderStyle="None" />
                    <Columns>
                        <asp:TemplateField HeaderText="Reply">
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkAppend" runat="server" ImageUrl="~/images/messanger30.jpg"
                                    NavigateUrl='<%# GoToCommunicationBoard(DataBinder.Eval(Container, "DataItem.SMRNo"),DataBinder.Eval(Container, "DataItem.RSSID"),DataBinder.Eval(Container, "DataItem.ApprovalLevel"),DataBinder.Eval(Container, "DataItem.TeamMemberID")) %>'
                                    ToolTip="Reply" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="RSSID" HeaderText="RSSID" SortExpression="RSSID" Visible="false" />
                        <asp:BoundField DataField="Comments" HeaderText="Question / Comment" SortExpression="Comments"
                            HeaderStyle-Width="500px" ItemStyle-Width="500px" ItemStyle-Font-Bold="true"
                            ItemStyle-CssClass="c_text">
                            <HeaderStyle Width="500px" />
                            <ItemStyle CssClass="c_text" Font-Bold="True" Width="500px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TeamMemberName" HeaderText="Submitted By" SortExpression="TeamMemberName"
                            HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-Font-Bold="true">
                            <HeaderStyle Width="100px" />
                            <ItemStyle Font-Bold="True" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PostDate" HeaderText="Post Date" SortExpression="PostDate"
                            ItemStyle-Font-Bold="true">
                            <ItemStyle Font-Bold="True" />
                        </asp:BoundField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                    </td>
                                    <td colspan="3">
                                        <asp:GridView ID="gvReply" runat="server" AutoGenerateColumns="False" DataSourceID="odsReply"
                                            DataKeyNames="SMRNo,RSSID" Width="100%" SkinID="CommBoardResponse">
                                            <Columns>
                                                <asp:BoundField DataField="Comments" HeaderText="Response" SortExpression="Comments"
                                                    HeaderStyle-Width="500px" ItemStyle-Width="500px" ItemStyle-Font-Bold="true" />
                                                <asp:BoundField DataField="TeamMemberName" HeaderText="" SortExpression="TeamMemberName"
                                                    HeaderStyle-Width="100px" ItemStyle-Width="100px" />
                                                <asp:BoundField DataField="PostDate" HeaderText="" SortExpression="PostDate" />
                                            </Columns>
                                        </asp:GridView>
                                        <asp:ObjectDataSource ID="odsReply" runat="server" OldValuesParameterFormatString="original_{0}"
                                            SelectMethod="GetSampleMtrlReqRSSReply" TypeName="PGMBLL">
                                            <SelectParameters>
                                                <asp:QueryStringParameter Name="SMRNo" QueryStringField="pSMRNo" Type="String" />
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
                    SelectMethod="GetSampleMtrlReqRSS" TypeName="PGMBLL">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="SMRNo" QueryStringField="pSMRNo" Type="String" />
                        <asp:Parameter Name="RSSID" Type="Int32" DefaultValue="0" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </asp:View>
        </asp:MultiView>
    </asp:Panel>
</asp:Content>
