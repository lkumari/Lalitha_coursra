<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="DrawingDetail.aspx.vb" Inherits="DrawingDetail" Title="PE Drawing Management: Drawing Detail"
    EnableEventValidation="false" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="maincontent" runat="Server" ContentPlaceHolderID="maincontent">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSave">
        <asp:Label ID="lblMessage" SkinID="MessageLabelSkin" runat="server"></asp:Label>
        <table width="98%">
            <tr>
                <td class="p_textbold">
                    Drawing No:
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblDrawingNo" runat="server" />
                </td>
                <td class="p_textbold">
                    <asp:Label ID="lblLastUpdatedByLabel" Text="Last Updated By:" runat="server" Visible="False" />
                </td>
                <td class="c_text">
                    <asp:Label ID="lblLastUpdatedByValue" runat="server" Visible="False" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label runat="server" ID="lblPartName" Visible="false" /><asp:Label runat="server"
                        ID="lblOldDrawingPartNameValue" Visible="false" />
                </td>
                <td class="p_textbold">
                    <asp:Label ID="lblLastUpdatedOnLabel" Text="Last Updated On:" runat="server" Visible="False"></asp:Label>
                </td>
                <td class="c_text">
                    <asp:Label ID="lblLastUpdatedOnValue" runat="server" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="p_textbold">
                    Internal Part No:
                </td>
                <td class="c_text">
                    <asp:Label runat="server" ID="lblPartNo" Text="Not Assigned Yet" />
                    &nbsp; Revision: &nbsp;
                    <asp:Label runat="server" ID="lblPartRevision" />
                </td>
                <td class="p_textbold">
                    Current Status:
                </td>
                <td align="left">
                    <asp:Label ID="lblApprovalStatus" runat="server" />
                    <asp:Label ID="lblApprovalStatusID" runat="server" Visible="false" />
                </td>
            </tr>
            <tr>
                <td class="p_textbold">
                    ECI No:
                </td>
                <td class="c_text">
                    <asp:HyperLink runat="server" ID="hlnkECINo" Visible="false" Font-Underline="true"
                        ToolTip="Click here to preview the ECI" Target="_blank"></asp:HyperLink>
                </td>
                <td class="p_textbold">
                    Date Issued/Released:
                </td>
                <td style="white-space: nowrap;">
                    <asp:Label ID="lblSubmitApproval" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <table width="98%">
            <tr>
                <td>
                    <asp:Button ID="btnVoid" runat="server" Text="Void" Visible="false" />
                    <asp:Button ID="btnCopy" runat="server" Text="Copy for New Part" Visible="false" />
                    <asp:Button ID="btnRevision" runat="server" Text="Create Revision" Visible="false" />
                    <asp:Button ID="btnStep" runat="server" Text="Create new Step" Visible="false" />
                    <asp:Button ID="btnSendNotification" runat="server" Text="Release (Send Notification)"
                        Visible="false" />
                    <asp:Button ID="btnEdit" runat="server" Text="Edit" Visible="false" CausesValidation="false" />
                    <br />
                    <asp:CheckBox runat="server" ID="cbCopyMaterialSpecList" Checked="true" Text="Copy Material Specifications" />
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:Label runat="server" ID="lblAppendRevisionNotes" Text="Edit Notes (to be appended to Revision Notes):"
                        Visible="false"></asp:Label>
                    &nbsp;
                    <asp:TextBox runat="server" ID="txtAppendRevisionNotes" MaxLength="100" Visible="false"
                        Width="300px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvAppendRevisionNotes" runat="server" ControlToValidate="txtAppendRevisionNotes"
                        Text="<" ErrorMessage="You are editing an issued drawing. Please enter some notes for editing. They will be appended to the revision notes."
                        SetFocusOnError="true" ValidationGroup="vgDrawing" Enabled="false" />
                </td>
            </tr>
        </table>
        <br />
        <asp:Menu ID="menuDMSTabs" Height="30px" runat="server" Orientation="Horizontal"
            StaticEnableDefaultPopOutImage="False" StaticMenuItemStyle-CssClass="tab" StaticSelectedStyle-CssClass="selectedTab"
            CssClass="tabs">
            <Items>
                <asp:MenuItem Text="Identification" Value="0" ImageUrl=""></asp:MenuItem>
                <asp:MenuItem Text="Specifications" Value="1" ImageUrl=""></asp:MenuItem>
                <asp:MenuItem Text="Bill Of Materials" Value="2" ImageUrl=""></asp:MenuItem>
                <asp:MenuItem Text="Principals" Value="3" ImageUrl=""></asp:MenuItem>
                <asp:MenuItem Text="Assignments" Value="4" ImageUrl=""></asp:MenuItem>
                <asp:MenuItem Text="Packaging and Inspection" Value="5" ImageUrl=""></asp:MenuItem>
            </Items>
        </asp:Menu>
        <br />
        <asp:ValidationSummary ID="vsDrawing" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgDrawing" />
        <asp:MultiView ID="mvDMSTabs" runat="server" Visible="true" ActiveViewIndex="0" EnableViewState="true">
            <asp:View ID="vIdentification" runat="server">
                <table>
                    <tr>
                        <td colspan="4">
                            <asp:Label ID="lblMessageIdentification" runat="server" SkinID="MessageLabelSkin">
                            </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Drawing Name:
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtOldPartName" runat="server" Width="395px" MaxLength="100">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Drawing Release Type:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddReleaseType" runat="server">
                            </asp:DropDownList>
                            <asp:LinkButton runat="server" ID="lnkChangeSubDrawingReleaseTypes" Text="Change Sub-Drawing Release Types"
                                Visible="false" CausesValidation="false"></asp:LinkButton>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:LinkButton runat="server" ID="lnkOpenPreviousRevision" Text="Open Previous Drawing Revision"
                                Visible="false" CausesValidation="false"></asp:LinkButton>
                            &nbsp;
                            <asp:LinkButton runat="server" ID="lnkOpenNextRevision" Text="Open Next Drawing Revision"
                                Visible="false" CausesValidation="false"></asp:LinkButton>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblInitialDimensionAndDensityMarker" runat="server" Font-Bold="True"
                                ForeColor="Red" Text="*" />
                            <asp:Label ID="lblInitialDimensionAndDensity" runat="server" Text="Initial Dimension and Density:"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtInitialDimensionAndDensity" runat="server" MaxLength="2" Width="25px">00</asp:TextBox>
                            &nbsp;<i>**Inserted into the Drawing No.</i>
                            <asp:RequiredFieldValidator ID="reqInitialDimensionAndDensity" runat="server" ControlToValidate="txtInitialDimensionAndDensity"
                                Text="<" ErrorMessage="Initial Dimension And Density Digits are required." SetFocusOnError="true"
                                ValidationGroup="vgDrawing">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblInStepNoMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="*" />
                            Process Step No.:
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtInStep" runat="server" MaxLength="1" Width="25px"></asp:TextBox>
                            &nbsp;<i>**Inserted into the Drawing No.</i>
                            <asp:RequiredFieldValidator ID="reqtxtInStep" CssClass="p_text" runat="server" Display="Dynamic"
                                ControlToValidate="txtInStep" SetFocusOnError="True" ErrorMessage="Process Number is required."
                                Text="<" ValidationGroup="vgDrawing">
                            </asp:RequiredFieldValidator>
                            <asp:CompareValidator runat="server" ID="cvInstep" Operator="DataTypeCheck" Type="Integer"
                                Text="<" ErrorMessage="Process Step must be a valid integer value." ControlToValidate="txtInStep"
                                SetFocusOnError="True" ValidationGroup="vgDrawing" />
                            <asp:RegularExpressionValidator ID="revProcessStepNumber" runat="server" ControlToValidate="txtInStep"
                                Text="<" ErrorMessage="Value Must be 1 through 9" SetFocusOnError="True" ValidationExpression="[1-9]"
                                ValidationGroup="vgDrawing">
                            </asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" valign="top">
                            Comments:
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtComments" runat="server" Width="500px" TextMode="Multiline" Height="100px"></asp:TextBox>
                            <br />
                            <asp:Label ID="lblCommentsCharCount" SkinID="MessageLabelSkin" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Designation Type:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddDesignationType" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Family:
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddFamily" runat="server" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblSubFamilyMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="*" />
                            Sub-Family:
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddSubFamily" runat="server">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="reqSubFamily" CssClass="p_text" runat="server" Display="Dynamic"
                                ControlToValidate="ddSubFamily" ValidationGroup="vgDrawing" ErrorMessage="SubFamily is required."
                                Text="<" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label runat="server" ID="lblOldCategoryTypeLabel" Text="Old Category/Type:"
                                Visible="false"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:Label runat="server" ID="lblOldCategoryTypeValue"></asp:Label>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" valign="top">
                            Commodity:
                        </td>
                        <td colspan="3" valign="top">
                            <asp:DropDownList ID="ddCommodity" runat="server">
                            </asp:DropDownList>
                            <br />
                            {Commodity / Classification}
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Product Technology:
                        </td>
                        <td class="c_textbold">
                            <asp:DropDownList ID="ddProductTechnology" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Purchased Good:
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddPurchasedGood" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label runat="server" ID="lblCustomerLabel" Text="Customer:" Visible="false"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:Label runat="server" ID="lblCustomerValue" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label runat="server" ID="lblOldCustomerPartNameLabel" Text="Customer Part Name:"
                                Visible="false"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:Label ID="lblOldCustomerPartNameValue" runat="server" Visible="false">
                            </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label runat="server" ID="lblCustomerPartNoLabel" Text="Customer Part No:">
                            </asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox runat="server" ID="txtCustomerPartNoValue" MaxLength="30"></asp:TextBox>
                            <asp:ImageButton ID="iBtnCustomerPartNoSearch" runat="server" ImageUrl="~/images/Search.gif"
                                Visible="false" ToolTip="Click here to search for the customer part number." />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:Button ID="btnSaveIdentification" runat="server" Text="Save" Visible="false"
                                ValidationGroup="vgDrawing" />
                        </td>
                    </tr>
                </table>
                <hr />
                <asp:ValidationSummary ID="vsCustomerProgram" runat="server" DisplayMode="List" ShowMessageBox="true"
                    ShowSummary="true" EnableClientScript="true" ValidationGroup="vgCustomerProgram" />
                <asp:Label runat="server" ID="lblMessageCustomerProgram" SkinID="MessageLabelSkin"></asp:Label>
                <table width="600px">
                    <tr>
                        <td valign="top">
                            <table runat="server" id="tblMakes" visible="false">
                                <%-- <tr>
                                    <td class="p_text">
                                        <asp:Label runat="server" ID="lblCustomer" Text="Customer:"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="ddCustomer" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label runat="server" ID="lblMake" Text="Make:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddMakes" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label runat="server" ID="lblModel" Text="Model:"></asp:Label>
                                    </td>
                                    <td style="font-size: smaller">
                                        <asp:DropDownList ID="ddModel" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label ID="lblProgramMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                            Text="*" />
                                        <asp:Label runat="server" ID="lblProgram" Text="Program:"></asp:Label>
                                    </td>
                                    <td colspan="3" style="white-space: nowrap">
                                        <asp:DropDownList ID="ddProgram" runat="server" AutoPostBack="true" />
                                        <asp:RequiredFieldValidator ID="rfvProgram" runat="server" ControlToValidate="ddProgram"
                                            ErrorMessage="Program is required." Font-Bold="True" ValidationGroup="vgCustomerProgram"
                                            Text="<" SetFocusOnError="true" />
                                        <br />
                                        {Program / Platform / Assembly Plant}
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label runat="server" ID="lblSOPDate" Text="Program SOP Date:" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="txtSOPDate" runat="server" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label runat="server" ID="lblEOPDate" Text="Program EOP Date:" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="txtEOPDate" runat="server" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label ID="lblYearMarker" runat="server" Font-Bold="True" ForeColor="Red" Text="*"
                                            Visible="false" />
                                        <asp:Label runat="server" ID="lblYear" Text="Year:" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddYear" runat="server" Visible="false">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvYear" runat="server" ControlToValidate="ddYear"
                                            ErrorMessage="Year is required." Font-Bold="True" ValidationGroup="vgCustomerProgram"
                                            Text="<" SetFocusOnError="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Button runat="Server" ID="btnAddToCustomerProgram" Text="Add Program / Customer"
                                ValidationGroup="vgCustomerProgram" Visible="false" />
                            <asp:Button runat="Server" ID="btnCancelEditCustomerProgram" Text="Cancel Edit Program / Customer"
                                CausesValidation="false" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
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
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="gvCustomerProgram" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                                AllowPaging="True" PageSize="15" DataKeyNames="RowID" DataSourceID="odsCustomerProgram"
                                EmptyDataText="No Programs or Customers found" Width="500px">
                                <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#CCCCCC" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField DataField="RowID" HeaderText="RowID" SortExpression="RowID" Visible="false" />
                                    <asp:BoundField DataField="ProgramYear" HeaderText="Year" SortExpression="ProgramYear"
                                        ReadOnly="True" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="ddProgramName" HeaderText="Program / Make / Model / Platform / Customer"
                                        SortExpression="ddProgramName" ReadOnly="True" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:TemplateField ShowHeader="true">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="iBtnCustomerProgramDelete" runat="server" CausesValidation="False"
                                                CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:ObjectDataSource ID="odsCustomerProgram" runat="server" OldValuesParameterFormatString="original_{0}"
                                SelectMethod="GetDrawingCustomerProgram" TypeName="DrawingCustomerProgramBLL"
                                DeleteMethod="DeleteDrawingCustomerProgram">
                                <SelectParameters>
                                    <asp:QueryStringParameter Name="DrawingNo" QueryStringField="DrawingNo" Type="String" />
                                </SelectParameters>
                                <DeleteParameters>
                                    <asp:Parameter Name="RowID" Type="Int32" />
                                    <asp:Parameter Name="original_RowID" Type="Int32" />
                                </DeleteParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:LinkButton runat="server" ID="lnkPushCustomerProgramToSubDrawing" Text="Push Program  / Customer Info to SubDrawings"
                                Visible="false" CausesValidation="false" />
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vSpecification" runat="server">
                <asp:Label runat="server" ID="lblVendorTip" />
                <table width="98%">
                    <tr>
                        <td valign="top">
                            <table width="100%">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblMessageSpecifications" runat="server" SkinID="MessageLabelSkin" /><br />
                                        <asp:Label ID="lblMessageDrawingImage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text" valign="top">
                                        Construction:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConstruction" TextMode="MultiLine" runat="Server" Width="400px">
                                        </asp:TextBox>
                                        <br />
                                        <asp:Label ID="lblConstructionCharCount" SkinID="MessageLabelSkin" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        Tolerance Type:
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ddTolerance" runat="server" AutoPostBack="True" CausesValidation="true"
                                            ValidationGroup="vgDrawing">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        Density Value:
                                    </td>
                                    <td class="c_text" style="white-space: nowrap;" valign="top">
                                        <asp:TextBox ID="txtDensityVal" runat="server" Width="105px" MaxLength="25" CausesValidation="true"
                                            ValidationGroup="vgDrawing"></asp:TextBox>
                                        <asp:CompareValidator runat="server" ID="cvValueDouble" Operator="DataTypeCheck"
                                            Type="Double" Text="<" ControlToValidate="txtDensityVal" ValidationGroup="vgDrawing"
                                            ErrorMessage="Density must be numeric." />
                                        &nbsp;&nbsp; Tolerance: &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:TextBox ID="txtDensityTol" runat="server" Width="65" MaxLength="10"></asp:TextBox>
                                        &nbsp;&nbsp; Units: &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:TextBox ID="txtDensityUnits" runat="server" Width="45" MaxLength="10"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        Thickness Value:
                                    </td>
                                    <td class="c_text" style="white-space: nowrap;" valign="top">
                                        <asp:TextBox ID="txtThickVal" runat="server" Width="105px" MaxLength="25" CausesValidation="true"
                                            ValidationGroup="vgDrawing"></asp:TextBox>
                                        <asp:CompareValidator runat="server" ID="cvThicknessValue" Operator="DataTypeCheck"
                                            ValidationGroup="vgDrawing" Type="Double" Text="<" ControlToValidate="txtThickVal"
                                            ErrorMessage="Thickness must be numeric." />
                                        &nbsp;&nbsp; Tolerance: &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:TextBox ID="txtThickTol" runat="server" MaxLength="10" Width="65"></asp:TextBox>
                                        &nbsp;&nbsp; Units: &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:TextBox ID="txtThicknessUnits" runat="server" Width="45" MaxLength="10"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text" style="white-space: nowrap;">
                                        Drawing Layout Type:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddDrawingLayoutType" runat="server" AutoPostBack="True" CausesValidation="true"
                                            ValidationGroup="vgDrawing">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem Value="Non-Rectangular">Non-Rectangular Shape</asp:ListItem>
                                            <asp:ListItem Value="No-Shape">No Shape</asp:ListItem>
                                            <asp:ListItem Value="Blank-Standard">Blank - Standard</asp:ListItem>
                                            <asp:ListItem Value="Blank-MD-Critical">Blank - MD - Critical</asp:ListItem>
                                            <asp:ListItem Value="Other-MD-Critical">Other - MD - Critical (Manually Uploaded)</asp:ListItem>
                                            <asp:ListItem Value="Rolled-Goods">Rolled - Goods</asp:ListItem>
                                            <asp:ListItem Value="Other">Other (Manually Uploaded)</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label runat="server" ID="lblUploadDMSImage" Text="Upload DMS Drawing Image:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:FileUpload runat="server" ID="uploadImage" Width="334px" Visible="false" />
                                        <asp:Button ID="btnSaveUploadImage" runat="server" Text="Upload DMS Image" CausesValidation="false"
                                            Visible="false"></asp:Button>
                                        <br />
                                        <asp:Label ID="lblMessageDMSImageUpload" runat="server" SkinID="MessageLabelSkin" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label ID="lblWMDVal" Text="WMD Value (direction of the arrow):" runat="server" />
                                    </td>
                                    <td class="c_text">
                                        <asp:TextBox ID="txtWMDVal" runat="server" Width="105px" AutoPostBack="True" MaxLength="15"
                                            CausesValidation="true" ValidationGroup="vgDrawing"></asp:TextBox>
                                        <asp:CompareValidator runat="server" ID="cvWMDValue" Operator="DataTypeCheck" ValidationGroup="vgDrawing"
                                            Type="Double" Text="<" ControlToValidate="txtWMDVal" ErrorMessage="WMD Value must be numeric." />
                                        <asp:Label runat="server" ID="lblWMDToleranceLabel" Text="Tolerance:"></asp:Label>&nbsp;<asp:TextBox
                                            ID="txtWMDTol" runat="server" Width="45" MaxLength="10"></asp:TextBox><asp:DropDownList
                                                ID="ddWMDUnits" runat="server" Style="width: 60px" AutoPostBack="True" CausesValidation="true"
                                                ValidationGroup="vgDrawing">
                                                <asp:ListItem>
                                                </asp:ListItem>
                                                <asp:ListItem Value="m">m</asp:ListItem>
                                                <asp:ListItem Value="mm">mm</asp:ListItem>
                                            </asp:DropDownList>
                                        <asp:TextBox ID="txtWMDRef" runat="server" Enabled="False" Width="85px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label ID="lblAMDVal" Text="AMD Value:" runat="server" CssClass="p_text" />
                                    </td>
                                    <td class="c_text">
                                        <asp:TextBox ID="txtAMDVal" runat="server" Width="105px" AutoPostBack="True" MaxLength="15"
                                            CausesValidation="true" ValidationGroup="vgDrawing"></asp:TextBox>
                                        <asp:CompareValidator runat="server" ID="cvAMDValue" Operator="DataTypeCheck" ValidationGroup="vgDrawing"
                                            Type="Double" Text="<" ControlToValidate="txtAMDVal" ErrorMessage="AMD Value must be numeric." />
                                        Tolerance:&nbsp;<asp:TextBox ID="txtAMDTol" runat="server" Width="45" MaxLength="10"></asp:TextBox><asp:DropDownList
                                            ID="ddAMDUnits" runat="server" Style="width: 60px" AutoPostBack="True" CausesValidation="true"
                                            ValidationGroup="vgDrawing">
                                            <asp:ListItem>
                                            </asp:ListItem>
                                            <asp:ListItem Value="m">m</asp:ListItem>
                                            <asp:ListItem Value="mm">mm</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtAMDRef" runat="server" Enabled="False" Width="85px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text" valign="top">
                                        Notes:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtNotes" runat="server" Width="400px" TextMode="Multiline">
                                        </asp:TextBox>
                                        <br />
                                        <asp:Label ID="lblNotesCharCount" SkinID="MessageLabelSkin" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text" valign="top">
                                        <asp:Label ID="lblInStepNoMarker0" runat="server" Font-Bold="True" 
                                            ForeColor="Red" Text="*" />
                                        Revision Notes:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtRevisionNotes" runat="server" Width="400px" TextMode="Multiline"/>
                      <asp:RequiredFieldValidator ID="rfvRevisionNotes" runat="server" ControlToValidate="txtRevisionNotes"
                                            ErrorMessage="Revision Notes is required." Font-Bold="True" ValidationGroup="vgDrawingCustomerImage"
                                            Text="<" SetFocusOnError="true" />
                                        <br />
                                        <asp:Label ID="lblRevisionNotesCharCount" SkinID="MessageLabelSkin" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text" style="white-space: nowrap;">
                                        UGN CAD Drawing:
                                    </td>
                                    <td class="c_text" style="white-space: nowrap;" align="left">
                                        <asp:CheckBox runat="server" ID="cbCADavailable" AutoPostBack="true" CausesValidation="true"
                                            ValidationGroup="vgDrawing" />
                                        <asp:HyperLink runat="server" ID="hlnkCustomerImage" Visible="false" Target="_blank">View CAD Drawing Image</asp:HyperLink>
                                        <asp:Button ID="btnDeleteDrawingCustomerImage" OnClick="deleteCustomerImage" Text="Delete"
                                            runat="Server" Font-Size="8pt" CausesValidation="false" Visible="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text" style="white-space: nowrap;">
                                        <asp:Label runat="server" ID="lblCustomerDrawingNo" Text="Customer Drawing No.:"
                                            Visible="false"></asp:Label>
                                    </td>
                                    <td class="c_textbold">
                                        <asp:TextBox runat="server" ID="txtCustomerDrawingNo" MaxLength="30" Visible="false"
                                            ValidationGroup="vgDrawingCustomerImage" CausesValidation="true"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvCustomerDrawingNo" CssClass="p_text" runat="server"
                                            Display="Dynamic" ControlToValidate="txtCustomerDrawingNo" SetFocusOnError="True"
                                            ErrorMessage="Customer Drawing Number is required." Text="<" ValidationGroup="vgDrawingCustomerImage">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label runat="server" ID="lblUploadCustomerDrawingImage" Text="Upload CAD Drawing Image:"
                                            Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:FileUpload runat="server" ID="uploadCustomerImage" Width="334px" Visible="false" />
                                        <asp:RequiredFieldValidator ID="rfvUploadCustomerImage" CssClass="p_text" runat="server"
                                            Display="Dynamic" ControlToValidate="uploadCustomerImage" SetFocusOnError="True"
                                            ErrorMessage="Customer Drawing Image is required." Text="<" ValidationGroup="vgDrawingCustomerImage">
                                        </asp:RequiredFieldValidator>
                                        <asp:Button ID="btnSaveUploadCustomerImage" runat="server" Text="Upload CAD Image"
                                            CausesValidation="true" Visible="false" ValidationGroup="vgDrawingCustomerImage">
                                        </asp:Button>
                                        <br />
                                        <asp:Label ID="lblMessageCustomerImageUpload" runat="server" SkinID="MessageLabelSkin" />
                                        <asp:ValidationSummary ID="vsDrawingCustomerImage" runat="server" DisplayMode="List"
                                            ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgDrawingCustomerImage" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table>
                                <tr>
                                    <td class="p_text">
                                        <asp:Label ID="lblDrawingGraphicImageLabel" runat="server" Text="Drawing Graphic/Image:"
                                            Visible="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Button ID="btnDeleteDrawingImage" OnClick="deleteImage" Text="Delete" runat="Server"
                                            Font-Size="8pt" CausesValidation="false" Visible="false" />
                                        <br />
                                        <img id="imgDrawing" runat="server" alt="Drawing" src="" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="98%">
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblDrawingMaterialRelateTitle" Text="Material specifications associated to this Drawings"
                                CssClass="p_bigtextbold" Visible="false"></asp:Label>
                            <asp:Label ID="lblMessageDrawingMaterialRelate" SkinID="MessageLabelSkin" runat="server"></asp:Label>
                            <asp:ValidationSummary ID="vsEditDrawingMaterialSpecRelate" runat="server" ShowMessageBox="True"
                                ShowSummary="true" ValidationGroup="vgEditDrawingMaterialSpecRelate" />
                            <asp:ValidationSummary ID="vsInsertDrawingMaterialSpecRelate" runat="server" ShowMessageBox="True"
                                ShowSummary="true" ValidationGroup="vgInsertDrawingMaterialSpecRelate" />
                            <asp:GridView ID="gvDrawingMaterialSpecRelate" runat="server" AutoGenerateColumns="False"
                                AllowSorting="True" AllowPaging="True" PageSize="15" DataKeyNames="RowID" DataSourceID="odsDrawingMaterialSpecRelate"
                                EmptyDataText="No DMS Drawings relate to this Material Specification yet." ShowFooter="True"
                                Width="98%" Visible="false">
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
                                    <asp:TemplateField ShowHeader="False">
                                        <FooterTemplate>
                                            <asp:ImageButton ID="iBtnSearchMaterialSpecNo" runat="server" CausesValidation="False"
                                                ImageUrl="~/images/Search.gif" ToolTip="Search for material specification number"
                                                AlternateText="Search MaterialSpecNo" />
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Center" Wrap="False" />
                                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="MaterialSpecNo" SortExpression="MaterialSpecNo">
                                        <EditItemTemplate>
                                            <asp:Label runat="server" ID="lblEditMaterialSpecNo" Text='<%# Bind("MaterialSpecNo") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lnkViewMaterialSpecNo" runat="server" NavigateUrl='<%# Eval("MaterialSpecNo", "~/PE/MaterialSpecDetail.aspx?MaterialSpecNo={0}") %>'
                                                Target="_blank" Text='<%# Eval("MaterialSpecNo") %>'>
                                            </asp:HyperLink>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtInsertMaterialSpecNo" runat="server" MaxLength="18"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvInsertMaterialSpecNo" runat="server" ControlToValidate="txtInsertMaterialSpecNo"
                                                ErrorMessage="MaterialSpecNo is required." Font-Bold="True" ValidationGroup="vgInsertDrawingMaterialSpecRelate"
                                                Text="<" SetFocusOnError="true">				                                                            
                                            </asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Notes" SortExpression="DrawingMaterialSpecNotes">
                                        <EditItemTemplate>
                                            <asp:TextBox runat="server" ID="txtEditDrawingMaterialSpecNotes" Text='<%# Bind("DrawingMaterialSpecNotes") %>'
                                                MaxLength="100" Width="300px"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblViewDrawingMaterialSpecNotes" Text='<%# Bind("DrawingMaterialSpecNotes") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtInsertDrawingMaterialSpecNotes" runat="server" MaxLength="100"
                                                Width="300px"></asp:TextBox>
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="iBtnDrawingMaterialSpecRelateUpdate" runat="server" CausesValidation="False"
                                                CommandName="Update" ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditDrawingMaterialSpecRelate" />&nbsp;&nbsp;&nbsp;
                                            <asp:ImageButton ID="iBtnDrawingMaterialSpecRelateCancel" runat="server" CausesValidation="False"
                                                CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="iBtnDrawingMaterialSpecRelateEdit" runat="server" CausesValidation="False"
                                                CommandName="Edit" ImageUrl="~/images/edit.jpg" AlternateText="Edit" />&nbsp;&nbsp;&nbsp;
                                            <asp:ImageButton ID="iBtnDrawingMaterialSpecRelateDelete" runat="server" CausesValidation="False"
                                                CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="iBtnDrawingMaterialSpecRelateInsert" runat="server" CausesValidation="True"
                                                CommandName="Insert" ImageUrl="~/images/save.jpg" AlternateText="Insert" ValidationGroup="vgInsertDrawingMaterialSpecRelate" />&nbsp;&nbsp;&nbsp;
                                            <asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo" ImageUrl="~/images/undo-gray.jpg"
                                                AlternateText="Undo" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:ObjectDataSource ID="odsDrawingMaterialSpecRelate" runat="server" DeleteMethod="DeleteDrawingMaterialSpecRelate"
                                InsertMethod="InsertDrawingMaterialSpecRelate" SelectMethod="GetDrawingMaterialSpecRelateByDrawingNo"
                                TypeName="DrawingMaterialSpecRelateByDrawingNoBLL" UpdateMethod="UpdateDrawingMaterialSpecRelate"
                                OldValuesParameterFormatString="original_{0}">
                                <DeleteParameters>
                                    <asp:Parameter Name="original_RowID" Type="Int32" />
                                </DeleteParameters>
                                <UpdateParameters>
                                    <asp:Parameter Name="original_RowID" Type="Int32" />
                                    <asp:Parameter Name="MaterialSpecNo" Type="String" />
                                    <asp:ControlParameter ControlID="lblDrawingNo" Name="DrawingNo" PropertyName="Text"
                                        Type="String" />
                                    <asp:Parameter Name="DrawingMaterialSpecNotes" Type="String" />
                                    <asp:Parameter Name="RowID" Type="Int32" />
                                </UpdateParameters>
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="lblDrawingNo" Name="DrawingNo" PropertyName="Text"
                                        Type="String" />
                                </SelectParameters>
                                <InsertParameters>
                                    <asp:ControlParameter ControlID="lblDrawingNo" Name="DrawingNo" PropertyName="Text"
                                        Type="String" />
                                    <asp:Parameter Name="MaterialSpecNo" Type="String" />
                                    <asp:Parameter Name="DrawingMaterialSpecNotes" Type="String" />
                                </InsertParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vBillOfMaterials" runat="server">
                <asp:Label ID="lblMessageBillOfMaterials" runat="server" SkinID="MessageLabelSkin"></asp:Label>
                <br />
                <asp:ValidationSummary ID="vsSubDrawing" runat="server" ShowMessageBox="True" ShowSummary="true"
                    ValidationGroup="vgSubDrawing" />
                <br />
                <% If ViewState("isEnabled") = True Then%>
                <table>
                    <tr>
                        <td class="p_text">
                            Sub-DrawingNo:
                        </td>
                        <td colspan="3">
                            <asp:TextBox runat="server" ID="txtSubDrawingNo" MaxLength="18"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvSubDrawingNo" runat="server" ControlToValidate="txtSubDrawingNo"
                                ErrorMessage="Sub Drawing is Required for B.O.M." Font-Bold="True" ValidationGroup="vgSubDrawing"
                                Text="<" SetFocusOnError="true">				                                                            
                            </asp:RequiredFieldValidator>
                            <asp:ImageButton ID="ibtnSearchSubDrawing" runat="server" CausesValidation="False"
                                ImageUrl="~/images/Search.gif" ToolTip="Search Sub-Drawing No." AlternateText="Search Sub-Drawing No." />
                            <asp:HyperLink ID="lnkViewSubDrawing" runat="server" Target="_blank" Text="View"
                                Visible="false"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            CAD Available:
                        </td>
                        <td colspan="3">
                            <asp:CheckBox runat="server" ID="cbSubDrawingCADAvailable" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Quantity:
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtSubDrawingQuantity" runat="server" MaxLength="10"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvSubDrawingQuantity" runat="server" ControlToValidate="txtSubDrawingQuantity"
                                ErrorMessage="Quantity is Required for Sub-Drawing." Font-Bold="True" ValidationGroup="vgSubDrawing"
                                Text="<" SetFocusOnError="true"> </asp:RequiredFieldValidator>
                            <asp:CompareValidator runat="server" ID="cvSubDrawingQuantity" Operator="DataTypeCheck"
                                ValidationGroup="vgSubDrawing" Type="Double" Text="<" ControlToValidate="txtSubDrawingQuantity"
                                ErrorMessage="Sub-Drawing Quantity must be numeric." SetFocusOnError="True" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" valign="top">
                            Notes:
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtSubDrawingNotes" runat="server" TextMode="MultiLine" Rows="2"
                                Width="400px">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvSubDrawingNotes" runat="server" ControlToValidate="txtSubDrawingNotes"
                                ErrorMessage="Notes are required for Sub-Drawing" ValidationGroup="SubDrawing"
                                Text="<" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            <br />
                            <asp:Label ID="lblSubDrawingNotesCharCount" SkinID="MessageLabelSkin" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" valign="top">
                            Process:
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtSubDrawingProcess" runat="server" TextMode="MultiLine" Rows="2"
                                Width="400px">
                            </asp:TextBox>
                            <br />
                            <asp:Label ID="lblSubDrawingProcessCharCount" SkinID="MessageLabelSkin" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" valign="top">
                            <!-- Equipment: -->
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtSubDrawingEquipment" runat="server" TextMode="MultiLine" Rows="2"
                                Width="400px" Visible="false">
                            </asp:TextBox>
                            <br />
                            <asp:Label ID="lblSubDrawingEquipmentCharCount" SkinID="MessageLabelSkin" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" valign="top">
                            Process Parameters:
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtSubDrawingProcessParameters" runat="server" TextMode="MultiLine"
                                Rows="2" Width="400px">
                            </asp:TextBox>
                            <br />
                            <asp:Label ID="lblSubDrawingProcessParametersCharCount" SkinID="MessageLabelSkin"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:Button runat="server" ID="btnSaveSubDrawing" Text="Add / Update SubDrawing"
                                CausesValidation="true" ValidationGroup="vgSubDrawing" />
                            <asp:Button runat="server" ID="btnCancelEditSubDrawing" Text="Cancel SubDrawing"
                                Visible="false" CausesValidation="false" />
                        </td>
                    </tr>
                </table>
                <% End If%>
                <asp:ValidationSummary ID="vsSubDrawingOverride" runat="server" ShowMessageBox="True"
                    ShowSummary="true" ValidationGroup="vgSubDrawingOverride" />
                <br />
                <asp:Label ID="lblMessageBillOfMaterialsBottom" runat="server" SkinID="MessageLabelSkin"></asp:Label>
                <table>
                    <tr>
                        <td align="left" colspan="3">
                            <asp:LinkButton runat="server" ID="lnkViewBOMTree" Text="View BOM with this Drawing as the top"></asp:LinkButton>
                            &nbsp;
                            <asp:LinkButton runat="Server" ID="btnPrinterFriendlyBOMView" Text="(Printer Friendly View)" />
                        </td>
                        <td align="right">
                            <asp:Button ID="btnDeleteAllCheckedBOM" runat="server" Text="Delete All Checked"
                                Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="4" style="white-space: nowrap">
                            <asp:TreeView runat="server" ID="tvCurrentDrawingAsTop" Font-Underline="false" Style="white-space: nowrap">
                            </asp:TreeView>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="4">
                            <asp:Button runat="server" ID="btnManageParentDrawings" Text="Manage Parent Drawing" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="4">
                            <asp:LinkButton runat="server" ID="lnkWhereUsed" Text="Find Where Used (WARNING: THIS COULD TAKE A FEW MINUTES!!)"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="4">
                            <asp:Label runat="server" ID="lblDrawingWhereUsedMessage" SkinID="MessageLabelSkin"></asp:Label>
                            <asp:TreeView runat="server" ID="tvDrawingWhereUsed">
                            </asp:TreeView>
                        </td>
                    </tr>
                </table>
                <asp:TextBox ID="txtSaveCheckBoxBOMDrawingNo" runat="server" TextMode="MultiLine"
                    Rows="20" CssClass="none"></asp:TextBox>
            </asp:View>
            <asp:View ID="vPrincipals" runat="server">
                <asp:Label runat="server" ID="lblNote" SkinID="MessageLabelSkin" Text="** NOTE: At a minimum, at least a Process Engineer and a Quality Engineer need to  be defined."></asp:Label>
                <table>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblMessagePrincipals" runat="server" SkinID="MessageLabelSkin">
                            </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Date Notification Sent:
                        </td>
                        <td align="left">
                            <asp:Label ID="lblNotification" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Engineer:
                        </td>
                        <td align="left" style="width: 296px">
                            <asp:DropDownList ID="ddEngineer" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Drawing By:
                        </td>
                        <td align="left" style="width: 296px">
                            <asp:DropDownList ID="ddDrawingByEngineer" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Checked By:
                        </td>
                        <td align="left" style="width: 296px">
                            <asp:DropDownList ID="ddCheckedByEngineer" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" style="height: 24px">
                            Process Engineer:
                        </td>
                        <td align="left" style="width: 296px; height: 24px">
                            <asp:DropDownList ID="ddProcessEngineer" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Quality Engineer:
                        </td>
                        <td align="left" style="width: 296px">
                            <asp:DropDownList ID="ddQualityEngineer" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <table width="60%">
                    <tr>
                        <td class="c_textbold" colspan="2">
                            Notification List
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblRpNotificationList" CssClass="p_textbold" Visible="False" runat="server" />
                            <br />
                            <asp:GridView ID="gvDrawingNotifications" runat="server" AllowPaging="True" AllowSorting="True"
                                DataSourceID="odsDrawingNotifications" AutoGenerateColumns="False" PageSize="25"
                                ShowFooter="True" EmptyDataText="There are no team members assigned to be notification yet."
                                DataKeyNames="DrawingNo,TeamMemberID" Width="400px">
                                <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" Wrap="False" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Wrap="False" />
                                <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" Wrap="False" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" Wrap="False" />
                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#E2DED6" Wrap="False" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" Wrap="False" />
                                <EmptyDataRowStyle BackColor="White" Wrap="False" />
                                <EmptyDataTemplate>
                                    No Records Found
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField HeaderText="Name" HeaderStyle-HorizontalAlign="Left">
                                        <EditItemTemplate>
                                            <asp:Label ID="lblEditNotificationTeamMemberName" runat="server" Text='<%# Bind("TeamMemberName") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewNotificationTeamMemberName" runat="server" Text='<%# Bind("TeamMemberName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddInsertNotificationTeamMember" runat="server" DataSource='<%# commonFunctions.GetTeamMemberBySubscription("27") %>'
                                                DataValueField="TMID" DataTextField="TMName" AppendDataBoundItems="True" SelectedValue='<%# Bind("TMID") %>'>
                                                <asp:ListItem Value="" Text="" Selected="true"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvInsertNotifcation" runat="server" ControlToValidate="ddInsertNotificationTeamMember"
                                                ErrorMessage="Notification Team Member is Required." Font-Bold="True" ValidationGroup="InsertNotification"
                                                Text="<" SetFocusOnError="true">				                                            
                                            </asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibtnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="ibtnInsert" runat="server" CausesValidation="True" CommandName="Insert"
                                                ImageUrl="~/images/save.jpg" AlternateText="Insert" ValidationGroup="InsertNotifications" />&nbsp;&nbsp;&nbsp;
                                            <asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo" ImageUrl="~/images/undo-gray.jpg"
                                                AlternateText="Undo" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:ValidationSummary ID="vsEmptyNotifications" runat="server" ShowMessageBox="True"
                                ShowSummary="true" ValidationGroup="EmptyNotifications" />
                            <asp:ValidationSummary ID="vsInsertNotification" runat="server" ShowMessageBox="True"
                                ShowSummary="true" ValidationGroup="InsertNotification" />
                            <asp:ObjectDataSource ID="odsDrawingNotifications" runat="server" SelectMethod="GetDrawingNotifications"
                                TypeName="DrawingNotificationsBLL" OldValuesParameterFormatString="original_{0}"
                                DeleteMethod="DeleteDrawingNotification" InsertMethod="InsertDrawingNotification">
                                <SelectParameters>
                                    <asp:QueryStringParameter Name="DrawingNo" QueryStringField="DrawingNo" Type="String" />
                                </SelectParameters>
                                <DeleteParameters>
                                    <asp:QueryStringParameter Name="DrawingNo" QueryStringField="DrawingNo" Type="String" />
                                    <asp:Parameter Name="TeamMemberID" Type="Int32" />
                                </DeleteParameters>
                                <InsertParameters>
                                    <asp:QueryStringParameter Name="DrawingNo" QueryStringField="DrawingNo" Type="String" />
                                    <asp:Parameter Name="TeamMemberID" Type="Int32" />
                                </InsertParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vBPCSassignments" runat="server">
                <asp:Label ID="lblMessageBPCSassignments" runat="server" SkinID="MessageLabelSkin"></asp:Label>
                <table width="98%" border="0">
                    <tr>
                        <td>
                            <asp:ValidationSummary ID="vsInsertBPCSInfo" runat="server" ShowMessageBox="True"
                                ShowSummary="true" ValidationGroup="vgInsertBPCSInfo" />
                            <asp:ValidationSummary ID="vsEditBPCSInfo" runat="server" ShowMessageBox="True" ShowSummary="true"
                                ValidationGroup="vgEditBPCSInfo" />
                            <asp:GridView ID="gvBPCSInfo" runat="server" AutoGenerateColumns="False" DataKeyNames="RowID"
                                AllowSorting="True" AllowPaging="True" PageSize="5" ShowFooter="True" DataSourceID="odsBPCSInfo"
                                EmptyDataText="There are no parts currently defined for this drawing." Width="500px">
                                <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#CCCCCC" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Internal Part No" SortExpression="PartNo">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditPartNo" runat="server" MaxLength="40" Text='<%# Bind("PartNo") %>'
                                                Width="175px" />
                                            <asp:ImageButton ID="ibtnEditSearchInfo" runat="server" CausesValidation="False"
                                                ImageUrl="~/images/Search.gif" ToolTip="Search PartNo" AlternateText="Search Internal Part No"
                                                ValidationGroup="vgEditBPCSInfo" />
                                            <asp:RequiredFieldValidator ID="rfvFooterTopLevelBPCSInfo" runat="server" ControlToValidate="txtEditPartNo"
                                                ErrorMessage="Internal Part No is Required." Font-Bold="True" ValidationGroup="vgEditBPCSInfo"
                                                Text="<" SetFocusOnError="true">				                                                            
                                            </asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lnkViewPartNo" runat="server" NavigateUrl='<%# Bind("PartNo", "~/DataMaintenance/PartMaintenance.aspx?PartNo={0}") %>'
                                                Target="_blank" Text='<%# Bind("PartNo") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtInsertPartNo" runat="server" MaxLength="40" Width="175px" />
                                            <asp:ImageButton ID="ibtnInsertSearchInfo" runat="server" CausesValidation="False"
                                                ImageUrl="~/images/Search.gif" ToolTip="Search PartNo" AlternateText="Search Internal Part No"
                                                ValidationGroup="vgInsertBPCSInfo" />
                                            <asp:RequiredFieldValidator ID="rfvInsertBPCSInfo" runat="server" ControlToValidate="txtInsertPartNo"
                                                ErrorMessage="Internal Part No is required." Font-Bold="True" ValidationGroup="vgInsertBPCSInfo"
                                                Text="<" SetFocusOnError="true">				                                                            
                                            </asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                                        <FooterStyle HorizontalAlign="Left" Wrap="False" />
                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Rev." SortExpression="PartRevision">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditPartRevision" runat="server" Text='<%# Bind("PartRevision") %>'
                                                MaxLength="2" Width="25px"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewPartRevision" runat="server" Text='<%# Bind("PartRevision") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtInsertPartRevision" runat="server" MaxLength="2" Width="25px"></asp:TextBox>
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                                        <FooterStyle HorizontalAlign="Left" Wrap="False" />
                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Part Name" SortExpression="PartName">
                                        <EditItemTemplate>
                                            <asp:Label ID="lblEditPartName" runat="server" Text='<%# Bind("PartName") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewPartName" runat="server" Text='<%# Bind("PartName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                                        <FooterStyle HorizontalAlign="Left" Wrap="False" />
                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="ibtnInfoUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                                ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditBPCSInfo" />
                                            &nbsp;&nbsp;&nbsp;
                                            <asp:ImageButton ID="ibtnInfoCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                                ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibtnInfoEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                                ImageUrl="~/images/edit.jpg" AlternateText="Edit" />&nbsp;&nbsp;&nbsp;
                                            <asp:ImageButton ID="ibtnInfoDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                                ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="ibtnInfoInsert" runat="server" CausesValidation="True" CommandName="Insert"
                                                ImageUrl="~/images/save.jpg" AlternateText="Insert" ValidationGroup="vgInsertBPCSInfo" />&nbsp;&nbsp;&nbsp;
                                            <asp:ImageButton ID="ibtnInfoUndo" runat="server" CommandName="Undo" ImageUrl="~/images/undo-gray.jpg"
                                                AlternateText="Undo" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:ObjectDataSource ID="odsBPCSInfo" runat="server" OldValuesParameterFormatString="original_{0}"
                                SelectMethod="GetDrawingBPCS" TypeName="DrawingBPCSBLL" DeleteMethod="DeleteDrawingBPCS"
                                InsertMethod="InsertDrawingBPCS" UpdateMethod="UpdateDrawingBPCS">
                                <SelectParameters>
                                    <asp:QueryStringParameter Name="DrawingNo" QueryStringField="DrawingNo" Type="String" />
                                </SelectParameters>
                                <DeleteParameters>
                                    <asp:Parameter Name="RowID" Type="Int32" />
                                    <asp:Parameter Name="original_RowID" Type="Int32" />
                                </DeleteParameters>
                                <UpdateParameters>
                                    <asp:Parameter Name="PartNo" Type="String" />
                                    <asp:Parameter Name="PartRevision" Type="String" />
                                    <asp:Parameter Name="original_RowID" Type="Int32" />
                                    <asp:Parameter Name="PartName" Type="String" />
                                </UpdateParameters>
                                <InsertParameters>
                                    <asp:Parameter Name="DrawingNo" Type="String" />
                                    <asp:Parameter Name="PartNo" Type="String" />
                                    <asp:Parameter Name="PartRevision" Type="String" />
                                </InsertParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vPackaging" runat="server">
                <asp:Label ID="lblMessagePackaging" runat="server" SkinID="MessageLabelSkin" />
                <br />
                <table width="98%" border="0">
                    <tr>
                        <td colspan="2">
                            <asp:ValidationSummary runat="server" ID="vsEditDrawingApprovedVendor" ValidationGroup="vgEditDrawingApprovedVendor"
                                ShowMessageBox="true" ShowSummary="true" />
                            <asp:ValidationSummary runat="server" ID="vsInsertDrawingApprovedVendor" ValidationGroup="vgInsertDrawingApprovedVendor"
                                ShowMessageBox="true" ShowSummary="true" />
                            <asp:GridView ID="gvDrawingApprovedVendor" runat="server" AutoGenerateColumns="False"
                                AllowSorting="True" AllowPaging="True" PageSize="15" DataKeyNames="RowID" DataSourceID="odsDrawingApprovedVendor"
                                ShowFooter="True" EmptyDataText="No approved vendors are defined for this drawing">
                                <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#CCCCCC" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField DataField="RowID" HeaderText="RowID" ReadOnly="True" />
                                    <asp:TemplateField HeaderText="Approved Vendor" SortExpression="ddUGNDBVendorName">
                                        <EditItemTemplate>
                                            <asp:Label ID="lblEditDrawingApprovedVendorMarker" runat="server" Font-Bold="True"
                                                ForeColor="Red" Text="*" />
                                            <asp:DropDownList ID="ddEditDrawingApprovedVendor" runat="server" DataSource='<%# CommonFunctions.GetUGNDBVendor(0,"","",0) %>'
                                                DataValueField="UGNDBVendorID" DataTextField="ddVendorName" AppendDataBoundItems="True"
                                                SelectedValue='<%# Bind("UGNDBVendorID") %>'>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvEditDrawingApprovedVendor" runat="server" ControlToValidate="ddEditDrawingApprovedVendor"
                                                ErrorMessage="Approved Vendor is required." Font-Bold="True" ValidationGroup="vgEditDrawingApprovedVendor"
                                                Text="<" SetFocusOnError="true">				                                                            
                                            </asp:RequiredFieldValidator>
                                            <br />
                                            <br />
                                            &nbsp;<asp:Label runat="server" ID="lblEditSubVendor" ForeColor="black" Font-Underline="true">Sub Vendor Name:</asp:Label>&nbsp;
                                            <asp:TextBox ID="txtEditSubVendorName" runat="server" Text='<%# Bind("SubVendorName") %>'
                                                MaxLength="25"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewDrawingApprovedVendor" runat="server" Text='<%# Bind("ddVendorName") %>'></asp:Label><br />
                                            <br />
                                            &nbsp;<asp:Label runat="server" ID="lblViewSubVendor" ForeColor="black" Font-Underline="true">Sub Vendor Name:</asp:Label>&nbsp;
                                            <asp:Label ID="lblViewSubVendorName" runat="server" Text='<%# Bind("SubVendorName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblInsertDrawingApprovedVendorMarker" runat="server" Font-Bold="True"
                                                ForeColor="Red" Text="*" />
                                            <asp:DropDownList ID="ddInsertDrawingApprovedVendor" runat="server" DataSource='<%# CommonFunctions.GetUGNDBVendor(0,"","",1) %>'
                                                DataValueField="UGNDBVendorID" DataTextField="ddVendorName" AppendDataBoundItems="True">
                                                <asp:ListItem></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvInsertDrawingApprovedVendor" runat="server" ControlToValidate="ddInsertDrawingApprovedVendor"
                                                ErrorMessage="Approved Vendor is required." Font-Bold="True" ValidationGroup="vgInsertDrawingApprovedVendor"
                                                Text="<" SetFocusOnError="true">				                                                            
                                            </asp:RequiredFieldValidator>
                                            <br />
                                            <br />
                                            &nbsp;<asp:Label runat="server" ID="lblInsertSubVendor" ForeColor="black" Font-Underline="true">Sub Vendor Name:</asp:Label>&nbsp;
                                            <asp:TextBox ID="txtInsertSubVendorName" runat="server" MaxLength="25"></asp:TextBox>
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Brand / Internal Part No / Approval Date">
                                        <EditItemTemplate>
                                            &nbsp;<asp:Label runat="server" ID="lblEditVendorBrand" ForeColor="black" Font-Underline="true">Brand:</asp:Label>&nbsp;
                                            <asp:TextBox ID="txtEditVendorBrand" runat="server" Text='<%# Bind("VendorBrand") %>'
                                                MaxLength="25" Width="100px"></asp:TextBox><br />
                                            <br />
                                            &nbsp;<asp:Label runat="server" ID="lblEditVendorPartNo" ForeColor="black" Font-Underline="true">Internal Part No:</asp:Label>&nbsp;
                                            <asp:TextBox ID="txtEditVendorPartNo" runat="server" Text='<%# Bind("VendorPartNo") %>'
                                                MaxLength="25" Width="100px"></asp:TextBox>
                                            <br />
                                            <br />
                                            &nbsp;<asp:Label runat="server" ID="lblEditVendorApprovalDate" ForeColor="black"
                                                Font-Underline="true">Approval Date:</asp:Label>&nbsp;
                                            <asp:TextBox ID="txtEditVendorApprovalDate" runat="server" Text='<%# Bind("VendorApprovalDate") %>'
                                                MaxLength="10" Width="75px"></asp:TextBox>
                                            <asp:ImageButton runat="server" ID="imgEditVendorApprovalDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                                AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                                            <ajax:CalendarExtender ID="cbeEditVendorApprovalDate" runat="server" TargetControlID="txtEditVendorApprovalDate"
                                                PopupButtonID="imgEditVendorApprovalDate" />
                                            <asp:RegularExpressionValidator ID="revEditVendorApprovalDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                                ControlToValidate="txtEditVendorApprovalDate" Font-Bold="True" ToolTip="MM/DD/YYYY"
                                                ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                                Width="8px" ValidationGroup="vgEditDrawingApprovedVendor"><</asp:RegularExpressionValidator>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            &nbsp;<asp:Label runat="server" ID="lblViewVendorBrand1" ForeColor="black" Font-Underline="true">Brand:</asp:Label>&nbsp;
                                            <asp:Label ID="lblViewVendorBrand2" runat="server" Text='<%# Bind("VendorBrand") %>'></asp:Label>
                                            <br />
                                            <br />
                                            &nbsp;<asp:Label runat="server" ID="lblViewVendorPartNo1" ForeColor="black" Font-Underline="true">Internal Part No:</asp:Label>&nbsp;
                                            <asp:Label ID="lblViewVendorPartNo2" runat="server" Text='<%# Bind("VendorPartNo") %>'></asp:Label>
                                            <br />
                                            <br />
                                            &nbsp;<asp:Label runat="server" ID="lblViewVendorApprovalDate1" ForeColor="black"
                                                Font-Underline="true">Approval Date:</asp:Label>&nbsp;
                                            <asp:Label ID="lblViewVendorApprovalDate2" runat="server" Text='<%# Bind("VendorApprovalDate") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            &nbsp;<asp:Label runat="server" ID="lblInsertVendorBrand" ForeColor="black" Font-Underline="true">Brand:</asp:Label>&nbsp;
                                            <asp:TextBox ID="txtInsertVendorBrand" runat="server" MaxLength="25"></asp:TextBox><br />
                                            <br />
                                            &nbsp;<asp:Label runat="server" ID="lblInsertVendorPartNo" ForeColor="black" Font-Underline="true">Internal Part No:</asp:Label>&nbsp;
                                            <asp:TextBox ID="txtInsertVendorPartNo" runat="server" MaxLength="25"></asp:TextBox>
                                            <br />
                                            <br />
                                            &nbsp;<asp:Label runat="server" ID="lblInsertVendorApprovalDate1" ForeColor="black"
                                                Font-Underline="true">Approval Date:</asp:Label>&nbsp;
                                            <asp:TextBox ID="txtInsertVendorApprovalDate" runat="server" MaxLength="10" Width="75px"></asp:TextBox>
                                            <asp:ImageButton runat="server" ID="imgInsertVendorApprovalDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                                AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                                            <ajax:CalendarExtender ID="cbeInsertVendorApprovalDate" runat="server" TargetControlID="txtInsertVendorApprovalDate"
                                                PopupButtonID="imgInsertVendorApprovalDate" />
                                            <asp:RegularExpressionValidator ID="revInsertVendorApprovalDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                                ControlToValidate="txtInsertVendorApprovalDate" Font-Bold="True" ToolTip="MM/DD/YYYY"
                                                ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                                Width="8px" ValidationGroup="vgInsertDrawingApprovedVendor"><</asp:RegularExpressionValidator>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Notes" SortExpression="VendorNotes">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditVendorNotes" runat="server" Text='<%# Bind("VendorNotes") %>'
                                                MaxLength="100" TextMode="MultiLine" Rows="5" Width="250px"></asp:TextBox><br />
                                            <asp:Label runat="server" ID="lblEditVendorNotesCharCount" SkinID="MessageLabelSkin"></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox ID="lblViewVendorNotes" runat="server" Text='<%# Bind("VendorNotes") %>'
                                                TextMode="MultiLine" Rows="5" Width="250px" Enabled="false"></asp:TextBox>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtInsertVendorNotes" runat="server" MaxLength="100" TextMode="MultiLine"
                                                Rows="5" Width="250px"></asp:TextBox><br />
                                            <asp:Label runat="server" ID="lblInsertVendorNotesCharCount" SkinID="MessageLabelSkin"></asp:Label>
                                        </FooterTemplate>
                                        <ItemStyle Wrap="True" />
                                        <ControlStyle Width="200px" />
                                        <FooterStyle Wrap="True" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="iBtnDrawingApprovedVendorUpdate" runat="server" CausesValidation="True"
                                                CommandName="Update" ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditDrawingApprovedVendor" />
                                            <asp:ImageButton ID="iBtnVendorDrawingApprovedCancel" runat="server" CausesValidation="False"
                                                CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="iBtnDrawingApprovedVendorEdit" runat="server" CausesValidation="False"
                                                CommandName="Edit" ImageUrl="~/images/edit.jpg" AlternateText="Edit" ValidationGroup="vgEditDrawingApprovedVendor" />
                                            <asp:ImageButton ID="iBtnDrawingApprovedVendorDelete" runat="server" CausesValidation="False"
                                                CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgInsertDrawingApprovedVendor"
                                                runat="server" ID="iBtnDrawingApprovedVendorSave" ImageUrl="~/images/save.jpg"
                                                AlternateText="Insert" />
                                            <asp:ImageButton ID="iBtnDrawingApprovedVendorUndo" runat="server" CommandName="Undo"
                                                CausesValidation="false" ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:ObjectDataSource ID="odsDrawingApprovedVendor" runat="server" OldValuesParameterFormatString="original_{0}"
                                SelectMethod="GetDrawingApprovedVendor" TypeName="DrawingApprovedVendorBLL" DeleteMethod="DeleteDrawingApprovedVendor"
                                InsertMethod="InsertDrawingApprovedVendor" UpdateMethod="UpdateDrawingApprovedVendor">
                                <DeleteParameters>
                                    <asp:Parameter Name="original_RowID" Type="Int32" />
                                </DeleteParameters>
                                <SelectParameters>
                                    <asp:QueryStringParameter Name="DrawingNo" QueryStringField="DrawingNo" Type="String" />
                                </SelectParameters>
                                <InsertParameters>
                                    <asp:Parameter Name="DrawingNo" Type="String" />
                                    <asp:Parameter Name="UGNDBVendorID" Type="Int32" />
                                    <asp:Parameter Name="SubVendorName" Type="String" />
                                    <asp:Parameter Name="VendorBrand" Type="String" />
                                    <asp:Parameter Name="VendorPartNo" Type="String" />
                                    <asp:Parameter Name="VendorNotes" Type="String" />
                                    <asp:Parameter Name="VendorApprovalDate" Type="String" />
                                </InsertParameters>
                                <UpdateParameters>
                                    <asp:Parameter Name="UGNDBVendorID" Type="Int32" />
                                    <asp:Parameter Name="SubVendorName" Type="String" />
                                    <asp:Parameter Name="VendorBrand" Type="String" />
                                    <asp:Parameter Name="VendorPartNo" Type="String" />
                                    <asp:Parameter Name="VendorNotes" Type="String" />
                                    <asp:Parameter Name="VendorApprovalDate" Type="String" />
                                    <asp:Parameter Name="original_RowID" Type="Int32" />
                                </UpdateParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblMessageVendor" runat="server" SkinID="MessageLabelSkin" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:ValidationSummary runat="server" ID="vsEditDrawingUnapprovedVendor" ValidationGroup="vgEditDrawingUnapprovedVendor"
                                ShowMessageBox="true" ShowSummary="true" />
                            <asp:ValidationSummary runat="server" ID="vsInsertDrawingUnapprovedVendor" ValidationGroup="vgInsertDrawingUnapprovedVendor"
                                ShowMessageBox="true" ShowSummary="true" />
                            <br />
                            <asp:GridView ID="gvDrawingUnapprovedVendor" runat="server" AutoGenerateColumns="False"
                                AllowSorting="True" AllowPaging="True" PageSize="15" DataKeyNames="RowID" DataSourceID="odsDrawingUnapprovedVendor"
                                ShowFooter="True" EmptyDataText="No unapproved vendors are defined for this drawing">
                                <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#CCCCCC" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField DataField="RowID" HeaderText="RowID" ReadOnly="True" />
                                    <asp:TemplateField HeaderText="Unapproved Vendor" SortExpression="VendorName">
                                        <EditItemTemplate>
                                            <asp:Label ID="lblEditDrawingUnapprovedVendorMarker" runat="server" Font-Bold="True"
                                                ForeColor="Red" Text="*" />
                                            <asp:TextBox ID="txtEditUnapprovedVendorName" runat="server" Text='<%# Bind("VendorName") %>'
                                                MaxLength="25"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="tfvEditUnapprovedVendorName" runat="server" ControlToValidate="txtEditUnapprovedVendorName"
                                                ErrorMessage="Unapproved Vendor is required." Font-Bold="True" ValidationGroup="vgEditDrawingUnapprovedVendor"
                                                Text="<" SetFocusOnError="true">				                                                            
                                            </asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewUnapprovedVendorName" runat="server" Text='<%# Bind("VendorName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblInsertDrawingApprovedVendorMarker" runat="server" Font-Bold="True"
                                                ForeColor="Red" Text="*" />
                                            <asp:TextBox ID="txtInsertUnapprovedVendorName" runat="server" MaxLength="25"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="tfvInsertUnapprovedVendorName" runat="server" ControlToValidate="txtInsertUnapprovedVendorName"
                                                ErrorMessage="Unapproved Vendor is required." Font-Bold="True" ValidationGroup="vgInsertDrawingUnapprovedVendor"
                                                Text="<" SetFocusOnError="true">				                                                            
                                            </asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                        <ItemStyle Wrap="true" />
                                        <FooterStyle Wrap="true" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Notes" SortExpression="VendorNotes">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditUnapprovedVendorNotes" runat="server" Text='<%# Bind("VendorNotes") %>'
                                                TextMode="MultiLine" Rows="5" Width="250px" MaxLength="100"></asp:TextBox><br />
                                            <asp:Label runat="server" ID="lblEditUnapprovedVendorNotesCharCount" SkinID="MessageLabelSkin"></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox ID="lblViewUnapprovedVendorNotes" runat="server" TextMode="MultiLine"
                                                Rows="5" Width="250px" Text='<%# Bind("VendorNotes") %>' Enabled="false"></asp:TextBox>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtInsertUnapprovedVendorNotes" runat="server" MaxLength="100" TextMode="MultiLine"
                                                Rows="5" Width="250px"></asp:TextBox><br />
                                            <asp:Label runat="server" ID="lblInsertUnapprovedVendorNotesCharCount" SkinID="MessageLabelSkin"></asp:Label>
                                        </FooterTemplate>
                                        <ItemStyle Wrap="true" />
                                        <FooterStyle Wrap="true" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="iBtnDrawingUnapprovedVendorUpdate" runat="server" CausesValidation="True"
                                                CommandName="Update" ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditDrawingUnapprovedVendor" />
                                            <asp:ImageButton ID="iBtnVendorDrawingUnapprovedCancel" runat="server" CausesValidation="False"
                                                CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="iBtnDrawingUnapprovedVendorEdit" runat="server" CausesValidation="False"
                                                CommandName="Edit" ImageUrl="~/images/edit.jpg" AlternateText="Edit" ValidationGroup="vgEditDrawingUnapprovedVendor" />
                                            <asp:ImageButton ID="iBtnDrawingUnapprovedVendorDelete" runat="server" CausesValidation="False"
                                                CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgInsertDrawingUnapprovedVendor"
                                                runat="server" ID="iBtnDrawingUnapprovedVendorSave" ImageUrl="~/images/save.jpg"
                                                AlternateText="Insert" />
                                            <asp:ImageButton ID="iBtnDrawingUnapprovedVendorUndo" runat="server" CommandName="Undo"
                                                CausesValidation="false" ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:ObjectDataSource ID="odsDrawingUnapprovedVendor" runat="server" OldValuesParameterFormatString="original_{0}"
                                SelectMethod="GetDrawingUnapprovedVendor" TypeName="DrawingUnapprovedVendorBLL"
                                DeleteMethod="DeleteDrawingUnapprovedVendor" InsertMethod="InsertDrawingUnapprovedVendor"
                                UpdateMethod="UpdateDrawingUnapprovedVendor">
                                <DeleteParameters>
                                    <asp:Parameter Name="original_RowID" Type="Int32" />
                                </DeleteParameters>
                                <UpdateParameters>
                                    <asp:Parameter Name="VendorName" Type="String" />
                                    <asp:Parameter Name="VendorNotes" Type="String" />
                                    <asp:Parameter Name="original_RowID" Type="Int32" />
                                </UpdateParameters>
                                <SelectParameters>
                                    <asp:QueryStringParameter Name="DrawingNo" QueryStringField="DrawingNo" Type="String" />
                                </SelectParameters>
                                <InsertParameters>
                                    <asp:Parameter Name="DrawingNo" Type="String" />
                                    <asp:Parameter Name="VendorName" Type="String" />
                                    <asp:Parameter Name="VendorNotes" Type="String" />
                                </InsertParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblMessageVendorBottom" runat="server" SkinID="MessageLabelSkin" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" align="right">
                            <asp:Label runat="server" ID="lblVendorLabel" Text="Vendor:"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddVendor" runat="server" AppendDataBoundItems="True" Enabled="false">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" align="right" valign="top">
                            Packaging Instructions:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtPackagingInstructions" runat="server" Text="Vendor Will Supply"
                                Width="395px" TextMode="MultiLine" Height="100px"></asp:TextBox><br />
                            <asp:Label ID="lblPackagingInstructionsCharCount" SkinID="MessageLabelSkin" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" align="right">
                            <asp:Label runat="server" ID="lblPackagingRollLengthLabel" Text="Roll Length:" Visible="false" />
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtPackagingRollLength" runat="server" Width="95px" MaxLength="25"></asp:TextBox>
                            <asp:CompareValidator runat="server" ID="cvPackagingRollLength" Operator="DataTypeCheck"
                                ValidationGroup="vgDrawing" Type="Double" Text="<" ControlToValidate="txtPackagingRollLength"
                                ErrorMessage="Packaging Roll Length value must be numeric." />
                            <asp:Label ID="lblPackagingRollLengthTolerance" runat="server" Text="Tolerance:"></asp:Label><asp:TextBox
                                ID="txtPackagingRollLengthTolerance" runat="server" MaxLength="10" Width="45"></asp:TextBox>
                            <asp:DropDownList ID="ddPackagingRollLengthUnits" runat="server" Style="width: 60px"
                                AutoPostBack="True">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem Value="m">m</asp:ListItem>
                                <asp:ListItem Value="mm">mm</asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="txtPackagingRollLengthRef" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" align="right" valign="top">
                            Incoming Inspection Comments:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtPackagingIncomingInspectionComments" runat="server" Width="395px"
                                TextMode="MultiLine" Height="100px"></asp:TextBox>
                            <asp:HyperLink runat="server" ID="lnkPackagingPreview" Target="_blank" Text="(Printer Friendly View of Packaging Info ONLY)" />
                            <asp:LinkButton runat="server" ID="lnkPackagingPreviewOLD" Text="(Printer Friendly View of Packaging Info ONLY)"
                                Visible="false" /><br />
                            <asp:Label ID="lblPackagingIncomingInspectionCommentsCharCount" SkinID="MessageLabelSkin"
                                runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:Label ID="lblMessagePackagingBottom" runat="server" SkinID="MessageLabelSkin" />
            </asp:View>
        </asp:MultiView><br />
        <table>
            <tr>
                <td align="center" colspan="2" style="white-space: nowrap;">
                    <asp:Button ID="btnFindSimilar" runat="server" Width="100" Text="Find Similar" CausesValidation="False"
                        ToolTip="Find similar drawings based on the same density and commidity/purchased good."
                        Visible="false"></asp:Button>
                    <asp:Button ID="btnSave" runat="server" Text="Save" Visible="false" ValidationGroup="vgDrawing" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="False" Visible="false">
                    </asp:Button>
                    <asp:Button ID="btnCompareRevisions" runat="server" Text="Compare Revision" CausesValidation="False"
                        ToolTip="Compare this drawing revision to the previous revision." Visible="false">
                    </asp:Button>
                    <asp:Button ID="btnPreview" runat="server" Text="Preview Drawing" CausesValidation="false"
                        Visible="false" />
                    <asp:CheckBox runat="server" ID="cbPreviewBOM" Text="Include BOM on drawing preview"
                        Visible="false" AutoPostBack="true" />&nbsp;
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
