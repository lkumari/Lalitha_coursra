<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Formula_Maint.aspx.vb" Inherits="Formula_Maint" MaintainScrollPositionOnPostback="true"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">

    <script language="javascript" type="text/javascript">

        function doCreateRevisionReason() {
            var reason = prompt('Please enter a reason for the new revision.', '', 'Reason');

            if ((reason == '') || (reason == ' ') || (reason == null)) {
                return false;
            } else {
                if (document.all.ctl00$maincontent$txtCreateRevisionReason != null) {
                    document.all.ctl00$maincontent$txtCreateRevisionReason.value = reason
                }
                return true;
            }

        }
    </script>

    <asp:Panel ID="localPanel" runat="server">
        <asp:Label ID="lblMessage" SkinID="MessageLabelSkin" runat="server"></asp:Label>
        <asp:ValidationSummary ID="vsFormulaMaint" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgFormulaMaint" />
        <table border="0" width="98%">
            <tr>
                <td align="right">
                    <asp:Label runat="server" ID="lblFormulaIDLabel" Text="Formula ID:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblFormulaIDValue" Visible="false"></asp:Label>
                </td>
                <td align="right">
                    <asp:Label runat="server" ID="lblObsoleteLabel" Text="Obsolete:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:CheckBox runat="server" ID="cbObsoleteValue" Visible="false" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblFormulaNameMarker" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="*" Visible="false" />
                    <asp:Label runat="server" ID="lblFormulaNameLabel" Text="Formula Name:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtFormulaNameValue" MaxLength="50" Visible="false"
                        Width="200px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvFormulaNameValue" runat="server" ControlToValidate="txtFormulaNameValue"
                        ErrorMessage="The name is required." Font-Bold="True" ValidationGroup="vgFormulaMaint"
                        Text="<" SetFocusOnError="true" />
                    &nbsp;<asp:Label runat="server" ID="lblFormulaRevisionLabel" Text="Rev.:" Visible="false"></asp:Label>&nbsp;
                    <asp:Label runat="server" ID="lblFormulaRevisionValue" Visible="false"></asp:Label>
                </td>
                <td align="right">
                    <asp:Label ID="lblFormulaDrawingNoLabelMarker" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="*" Visible="false" />
                    <asp:Label runat="server" ID="lblFormulaDrawingNoLabel" Text="Drawing No:" Visible="false"></asp:Label>
                </td>
                <td style="white-space: nowrap">                
                    <asp:TextBox runat="server" ID="txtFormulaDrawingNoValue" MaxLength="17" Visible="false"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvFormulaDrawingNoValue" runat="server" ControlToValidate="txtFormulaDrawingNoValue"
                        ErrorMessage="The DMS DrawingNo is required." Font-Bold="True" ValidationGroup="vgFormulaMaint"
                        Text="<" SetFocusOnError="true" />
                    &nbsp;
                    <asp:ImageButton runat="server" ID="iBtnFormulaDrawingNo" ImageUrl="~/images/Search.gif"
                        Visible="false" />
                    &nbsp;
                    <asp:HyperLink runat="server" ID="hlnkNewDrawingNo" Visible="false" Font-Underline="true"
                        Target="_blank" ToolTip="Click here to view the DMS Drawing." Text="View DMS Drawing"></asp:HyperLink>
                    <br />
                    <asp:Label runat="server" ID="lblFormulaDrawingNameLabel" Text="Name:" Visible="false"></asp:Label>&nbsp;
                    <asp:Label runat="server" ID="lblFormulaDrawingNameValue" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label runat="server" ID="lblFormulaNameRevisionsLabel" Text="View Other Formula Revisions:"
                        Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddFormulaRevisions" Visible="false" AutoPostBack="true"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblFormulaStartDateMarker" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="*" Visible="false" />
                    <asp:Label runat="server" ID="lblFormulaStartDateLabel" Text="Formula Start Date:"
                        Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtFormulaStartDateValue" MaxLength="10" Visible="false"
                        Width="85px"></asp:TextBox>
                    <asp:ImageButton runat="server" ID="imgFormulaStartDateValue" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                    <ajax:CalendarExtender ID="cbeFormulaStartDateValue" runat="server" TargetControlID="txtFormulaStartDateValue"
                        PopupButtonID="imgFormulaStartDateValue" />
                    <asp:RegularExpressionValidator ID="revFormulaStartDateValue" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtFormulaStartDateValue" Font-Bold="True" ToolTip="MM/DD/YYYY"
                        ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px" ValidationGroup="vgFormulaMaint"><</asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="rfvFormulaStartDate" runat="server" ControlToValidate="txtFormulaStartDateValue"
                        ErrorMessage="Formula Start Date is required." Font-Bold="True" ValidationGroup="vgFormulaMaint"
                        Text="<" SetFocusOnError="true" />
                </td>
                <td align="right">
                    <asp:Label runat="server" ID="lblFormulaPartNoLabel" Text="BPCS Part No:" Visible="false"></asp:Label>
                </td>
                <td colspan="3">
                    <!-- must be checked for valid BPCS PartNo before saving -->
                    <asp:TextBox runat="server" ID="txtFormulaPartNoValue" MaxLength="15" Visible="false"></asp:TextBox>
                    &nbsp;
                    <asp:ImageButton runat="server" ID="iBtnFormulaPartNo" ImageUrl="~/images/Search.gif"
                        Visible="false" />
                    &nbsp;<asp:Label runat="server" ID="lblFormulaPartRevisionLabel" Text="Rev.:"
                        Visible="false"></asp:Label>&nbsp;
                    <asp:TextBox runat="server" ID="txtFormulaPartRevisionValue" MaxLength="2" Width="40px"
                        Visible="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label runat="server" ID="lblFormulaEndDateLabel" Text="Formula End Date:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtFormulaEndDateValue" MaxLength="10" Visible="false"
                        Width="85px"></asp:TextBox>
                    <asp:ImageButton runat="server" ID="imgFormulaEndDateValue" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                        AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                    <ajax:CalendarExtender ID="cbeFormulaEndDateValue" runat="server" TargetControlID="txtFormulaEndDateValue"
                        PopupButtonID="imgFormulaEndDateValue" />
                    <asp:RegularExpressionValidator ID="revFormulaEndDateValue" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                        ControlToValidate="txtFormulaEndDateValue" Font-Bold="True" ToolTip="MM/DD/YYYY"
                        ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                        Width="8px" ValidationGroup="vgFormulaMaint"><</asp:RegularExpressionValidator>
                </td>
                <td align="right">
                    <asp:Label runat="server" ID="lblFormulaPartNameLabel" Text="BPCS Part Name:"
                        Visible="false"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox runat="server" ID="txtFormulaPartNameValue" Visible="false" Enabled="false"
                        Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label runat="server" ID="lblSpecificGravityLabel" Text="Specific Gravity:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSpecificGravityValue" MaxLength="10" Width="100px"
                        Visible="false"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvFooterMinimumPartLength" Operator="DataTypeCheck"
                        ValidationGroup="vgFormulaMaint" Type="double" Text="<" ControlToValidate="txtSpecificGravityValue"
                        ErrorMessage="Specific gravity must be a number." SetFocusOnError="True" />
                    <asp:DropDownList runat="server" ID="ddSpecificGravityUnits" Visible="false" Enabled="false">
                    </asp:DropDownList>
                </td>
                <td align="right">
                    <asp:Label runat="server" ID="lblMaximumMixCapacityLabel" Text="Maximum Mix Capacity:"
                        Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtMaximumMixCapacityValue" MaxLength="10" Width="100px"
                        Visible="false"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvMaximumMixCapacity" Operator="DataTypeCheck"
                        ValidationGroup="vgFormulaMaint" Type="integer" Text="<" ControlToValidate="txtMaximumMixCapacityValue"
                        ErrorMessage="Maximum Mix Capacity must be an integer." SetFocusOnError="True" />
                    <asp:DropDownList runat="server" ID="ddMaximumMixCapacityUnits" Visible="false" Enabled="false">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label runat="server" ID="lblMaximumLineSpeedLabel" Text="Maximum Line Speed:"
                        Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtMaximumLineSpeedValue" MaxLength="10" Width="100px"
                        Visible="false"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvMaximumLineSpeed" Operator="DataTypeCheck"
                        ValidationGroup="vgFormulaMaint" Type="integer" Text="<" ControlToValidate="txtMaximumLineSpeedValue"
                        ErrorMessage="Maximum line speed must be an integer." SetFocusOnError="True" />
                    <asp:DropDownList runat="server" ID="ddMaximumLineSpeedUnits" Visible="false" Enabled="false">
                    </asp:DropDownList>
                </td>
                <td align="right">
                    <asp:Label runat="server" ID="lblMaximumPressCyclesLabel" Text="Maximum Press Cycles:"
                        Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtMaximumPressCyclesValue" MaxLength="10" Width="100px"
                        Visible="false"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvMaximumPressCycles" Operator="DataTypeCheck"
                        ValidationGroup="vgFormulaMaint" Type="integer" Text="<" ControlToValidate="txtMaximumPressCyclesValue"
                        ErrorMessage="Maximum press cycles must be an integer." SetFocusOnError="True" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label runat="server" ID="lblWeightPerAreaLabel" Text="Weight Per Area:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtWeightPerAreaValue" MaxLength="10" Width="100px"
                        Visible="false"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvWeightPerArea" Operator="DataTypeCheck"
                        ValidationGroup="vgFormulaMaint" Type="double" Text="<" ControlToValidate="txtWeightPerAreaValue"
                        ErrorMessage="Weight per area must be a number." SetFocusOnError="True" />
                    <asp:DropDownList runat="server" ID="ddWeightPerAreaUnits" Visible="false" Enabled="false">
                        <asp:ListItem Text="in/ft2" Value="in/ft2"></asp:ListItem>
                        <asp:ListItem Text="g/m2" Value="g/m2"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td align="right">
                    <asp:Label runat="server" ID="lblCoatingSidesLabel" Text="Coating Sides:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtCoatingSidesValue" MaxLength="10" Width="100px"
                        Visible="false"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvCoatingSides" Operator="DataTypeCheck"
                        ValidationGroup="vgFormulaMaint" Type="integer" Text="<" ControlToValidate="txtCoatingSidesValue"
                        ErrorMessage="Coating sides must be an integer." SetFocusOnError="True" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label runat="server" ID="lblMaximumFormingRateLabel" Text="Maximum Forming Rate:"
                        Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtMaximumFormingRateValue" MaxLength="10" Width="100px"
                        Visible="false"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvMaximumFormingRate" Operator="DataTypeCheck"
                        ValidationGroup="vgFormulaMaint" Type="double" Text="<" ControlToValidate="txtMaximumFormingRateValue"
                        ErrorMessage="Maximum Forming Rate must be a number." SetFocusOnError="True" />
                    <asp:DropDownList runat="server" ID="ddMaximumFormingRateUnits" Visible="false" Enabled="false">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblDepartmentMarker" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="*" Visible="false" />
                    <asp:Label runat="server" ID="lblDepartmentLabel" Text="Department:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddDepartmentValue" runat="server" Visible="false">
                    </asp:DropDownList>
                </td>
                <td align="right">
                    <asp:Label runat="server" ID="lblDieCutLabel" Text="Diecut:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:CheckBox runat="server" ID="cbDiecutValue" Visible="false" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblProcessMarker" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="*" Visible="false" />
                    <asp:Label runat="server" ID="lblProcessLabel" Text="Process:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddProcessValue" runat="server" Visible="false" Width="306px">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvProcess" runat="server" ControlToValidate="ddProcessValue"
                        ErrorMessage="The process is required." Font-Bold="True" ValidationGroup="vgFormulaMaint"
                        Text="<" SetFocusOnError="true" />
                </td>
                <td align="right">
                    <asp:Label runat="server" ID="lblRecycleReturnLabel" Text="Recycle Return:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:CheckBox runat="server" ID="cbRecycleReturnValue" Visible="false" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblTemplateMarker" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="*" Visible="false" />
                    <asp:Label runat="server" ID="lblTemplateLabel" Text="Template:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddTemplateValue" runat="server" Visible="false">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvTemplate" runat="server" ControlToValidate="ddTemplateValue"
                        ErrorMessage="The template is required." Font-Bold="True" ValidationGroup="vgFormulaMaint"
                        Text="<" SetFocusOnError="true" />
                </td>
                <td align="right">
                    <asp:Label runat="server" ID="lblFleeceTypeLabel" Text="FleeceType:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:CheckBox runat="server" ID="cbFleeceTypeValue" Visible="false" AutoPostBack="true" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td valign="top" align="right">
                    <asp:Label runat="server" ID="lblCreateRevisionReason" Text="Revision Reason:" Visible="false"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox runat="server" ID="txtCreateRevisionReason" TextMode="MultiLine" Visible="false"
                        Width="650px" Height="100px"></asp:TextBox>
                    <br />
                    <asp:Label ID="lblCreateRevisionReasonCharCount" SkinID="MessageLabelSkin" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button runat="server" ID="btnSave" CausesValidation="true" Text="Save" Visible="false"
                        ValidationGroup="vgFormulaMaint" />
                    &nbsp;
                    <asp:Button runat="server" ID="btnCopy" CausesValidation="false" Text="Copy to be a New Formula"
                        Visible="false" />
                    <asp:Button runat="server" ID="btnCreateRevision" CausesValidation="true" Text="Create Revision"
                        Visible="false" ValidationGroup="vgFormulaMaint" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:ValidationSummary ID="vsFooterDepartment" runat="server" ShowMessageBox="True"
                        ShowSummary="true" ValidationGroup="vgFooterDepartment" />
                    <asp:ValidationSummary ID="vsEditDepartment" runat="server" ShowMessageBox="True"
                        ShowSummary="true" ValidationGroup="vgEditDepartment" />
                    <br />
                    <asp:GridView runat="server" ID="gvDepartment" Width="100%" DataSourceID="odsFormulaDepartment"
                        AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" PageSize="2"
                        ShowFooter="True" DataKeyNames="RowID">
                        <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#CCCCCC" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="RowID" SortExpression="RowID" />
                            <asp:BoundField DataField="FormulaID" SortExpression="FormulaID" />
                            <asp:TemplateField HeaderText="Department(s)" SortExpression="ddDepartmentName">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddEditDepartment" runat="server" DataSource='<%# CostingModule.GetCostingDepartmentList("","",False) %>'
                                        DataValueField="DepartmentID" DataTextField="ddDepartmentName" AppendDataBoundItems="True"
                                        SelectedValue='<%# Bind("DepartmentID") %>'>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvEditDepartment" runat="server" ControlToValidate="ddEditDepartment"
                                        ErrorMessage="The department is required." Font-Bold="True" ValidationGroup="vgEditDepartment"
                                        Text="<" SetFocusOnError="true" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblViewDepartmentName" runat="server" Text='<%# Bind("ddDepartmentName") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddFooterDepartment" runat="server" DataSource='<%# CostingModule.GetCostingDepartmentList("","",False) %>'
                                        DataValueField="DepartmentID" DataTextField="ddDepartmentName" AppendDataBoundItems="True">
                                        <asp:ListItem Text="" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvFooterDepartment" runat="server" ControlToValidate="ddFooterDepartment"
                                        ErrorMessage="The department is required." Font-Bold="True" ValidationGroup="vgFooterDepartment"
                                        Text="<" SetFocusOnError="true" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <EditItemTemplate>
                                    <asp:ImageButton ID="iBtnDepartmentUpdate" runat="server" CausesValidation="True"
                                        CommandName="Update" ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditDepartment" />
                                    <asp:ImageButton ID="iBtnDepartmentCancel" runat="server" CausesValidation="False"
                                        CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="iBtnDepartmentEdit" runat="server" CausesValidation="False"
                                        CommandName="Edit" ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                                    <asp:ImageButton ID="ibtnDepartmentDelete" runat="server" CausesValidation="False"
                                        CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgFooterDepartment"
                                        runat="server" ID="iBtnFooterDepartment" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                                    <asp:ImageButton ID="iBtnDepartmentUndo" runat="server" CommandName="Undo" CausesValidation="false"
                                        ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="odsFormulaDepartment" runat="server" OldValuesParameterFormatString="original_{0}"
                        SelectMethod="GetFormulaDepartment" TypeName="FormulaDepartmentBLL" DeleteMethod="DeleteFormulaDepartment"
                        UpdateMethod="UpdateFormulaDepartment" InsertMethod="InsertFormulaDepartment">
                        <SelectParameters>
                            <asp:QueryStringParameter Name="FormulaID" QueryStringField="FormulaID" Type="Int32" />
                        </SelectParameters>
                        <DeleteParameters>
                            <asp:Parameter Name="RowID" Type="Int32" />
                            <asp:Parameter Name="original_RowID" Type="Int32" />
                        </DeleteParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="DepartmentID" Type="Int32" />
                            <asp:Parameter Name="original_RowID" Type="Int32" />
                            <asp:Parameter Name="FormulaID" Type="Int32" />
                        </UpdateParameters>
                        <InsertParameters>
                            <asp:Parameter Name="FormulaID" Type="Int32" />
                            <asp:Parameter Name="DepartmentID" Type="Int32" />
                        </InsertParameters>
                    </asp:ObjectDataSource>
                </td>
            </tr>
        </table>
        <br />
        <asp:Menu ID="menuFormulaTabs" Height="30px" runat="server" Orientation="Horizontal"
            StaticEnableDefaultPopOutImage="False" StaticMenuItemStyle-CssClass="tab" StaticSelectedStyle-CssClass="selectedTab"
            CssClass="tabs" StaticDisplayLevels="2" Visible="false">
            <Items>
                <asp:MenuItem Text="Coating Factors" Value="0"></asp:MenuItem>
                <asp:MenuItem Text="Hole Deplug Factors" Value="1"></asp:MenuItem>
                <asp:MenuItem Text="Material" Value="2"></asp:MenuItem>
                <asp:MenuItem Text="Packaging" Value="3"></asp:MenuItem>
                <asp:MenuItem Text="Labor" Value="4"></asp:MenuItem>
                <asp:MenuItem Text="Overhead" Value="5"></asp:MenuItem>
                <asp:MenuItem Text="Misc. Cost" Value="6"></asp:MenuItem>
            </Items>
        </asp:Menu>
        <asp:Label ID="lblMessageBottom" SkinID="MessageLabelSkin" runat="server"></asp:Label>
        <br />
        <table width="98%" border="0">
            <tr>
                <td>
                    <asp:MultiView ID="mvFormulas" runat="server" Visible="true" ActiveViewIndex="0"
                        EnableViewState="true">
                        <asp:View ID="vCoatingFactor" runat="server">
                            <asp:Label runat="server" ID="lblMessageCoatingFactor"></asp:Label>
                            <asp:ValidationSummary ID="vsEditFormulaCoatingFactor" runat="server" DisplayMode="List"
                                ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgEditFormulaCoatingFactor" />
                            <asp:ValidationSummary ID="vsFooterFormulaCoatingFactor" runat="server" DisplayMode="List"
                                ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgFooterFormulaCoatingFactor" />
                            <asp:GridView runat="server" ID="gvFormulaCoatingFactor" AllowPaging="True" AllowSorting="True"
                                AutoGenerateColumns="False" PageSize="15" ShowFooter="True" DataSourceID="odsFormulaCoatingFactor"
                                DataKeyNames="FactorID" Width="100%" Visible="False">
                                <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#CCCCCC" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
                                <Columns>
                                    <asp:BoundField DataField="FactorID" SortExpression="FactorID" ReadOnly="True" />
                                    <asp:BoundField DataField="FormulaID" SortExpression="FormulaID" ReadOnly="True" />
                                    <asp:TemplateField HeaderText="Minimum" SortExpression="MinimumFactor">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditCoatingFactorMinimum" runat="server" Text='<%# Bind("MinimumFactor") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvEditCoatingFactorMinimum" runat="server" ControlToValidate="txtEditCoatingFactorMinimum"
                                                ErrorMessage="Minimum is required." Font-Bold="True" ValidationGroup="vgEditFormulaCoatingFactor"
                                                Text="<" SetFocusOnError="true" />
                                            <asp:CompareValidator runat="server" ID="cvEditCoatingFactorMinimum" Operator="DataTypeCheck"
                                                ValidationGroup="vgEditFormulaCoatingFactor" Type="double" Text="<" ControlToValidate="txtEditCoatingFactorMinimum"
                                                ErrorMessage="Minimum must be a number." SetFocusOnError="True" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewCoatingFactorMinimum" runat="server" Text='<%# Bind("MinimumFactor") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFooterCoatingFactorMinimum" runat="server" Text='<%# Bind("MinimumFactor") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvFooterCoatingFactorMinimum" runat="server" ControlToValidate="txtFooterCoatingFactorMinimum"
                                                ErrorMessage="Minimum is required." Font-Bold="True" ValidationGroup="vgFooterFormulaCoatingFactor"
                                                Text="<" SetFocusOnError="true" />
                                            <asp:CompareValidator runat="server" ID="cvFooterCoatingFactorMinimum" Operator="DataTypeCheck"
                                                ValidationGroup="vgFooterFormulaCoatingFactor" Type="double" Text="<" ControlToValidate="txtFooterCoatingFactorMinimum"
                                                ErrorMessage="Minimum must be a number." SetFocusOnError="True" />
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Maximum" SortExpression="MaximumFactor">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditCoatingFactorMaximum" runat="server" Text='<%# Bind("MaximumFactor") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvEditCoatingFactorMaximum" runat="server" ControlToValidate="txtEditCoatingFactorMaximum"
                                                ErrorMessage="Maximum is required." Font-Bold="True" ValidationGroup="vgEditFormulaCoatingFactor"
                                                Text="<" SetFocusOnError="true" />
                                            <asp:CompareValidator runat="server" ID="cvEditCoatingFactorMaximum" Operator="DataTypeCheck"
                                                ValidationGroup="vgEditFormulaCoatingFactor" Type="double" Text="<" ControlToValidate="txtEditCoatingFactorMaximum"
                                                ErrorMessage="Maximum must be a number." SetFocusOnError="True" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewCoatingFactorMaximum" runat="server" Text='<%# Bind("MaximumFactor") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFooterCoatingFactorMaximum" runat="server" Text='<%# Bind("MaximumFactor") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvFooterCoatingFactorMaximum" runat="server" ControlToValidate="txtFooterCoatingFactorMaximum"
                                                ErrorMessage="Maximum is required." Font-Bold="True" ValidationGroup="vgFooterFormulaCoatingFactor"
                                                Text="<" SetFocusOnError="true" />
                                            <asp:CompareValidator runat="server" ID="cvFooterCoatingFactorMaximum" Operator="DataTypeCheck"
                                                ValidationGroup="vgFooterFormulaCoatingFactor" Type="double" Text="<" ControlToValidate="txtFooterCoatingFactorMaximum"
                                                ErrorMessage="Maximum must be a number." SetFocusOnError="True" />
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Coating Factor" SortExpression="CoatingFactor">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditCoatingFactor" runat="server" Text='<%# Bind("CoatingFactor") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvEditCoatingFactor" runat="server" ControlToValidate="txtEditCoatingFactor"
                                                ErrorMessage="Coating factor is required." Font-Bold="True" ValidationGroup="vgEditFormulaCoatingFactor"
                                                Text="<" SetFocusOnError="true" />
                                            <asp:CompareValidator runat="server" ID="cvEditCoatingFactor" Operator="DataTypeCheck"
                                                ValidationGroup="vgEditFormulaCoatingFactor" Type="double" Text="<" ControlToValidate="txtEditCoatingFactor"
                                                ErrorMessage="Coating factor  must be a number." SetFocusOnError="True" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewCoatingFactor" runat="server" Text='<%# Bind("CoatingFactor") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFooterCoatingFactor" runat="server" Text='<%# Bind("CoatingFactor") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvFooterCoatingFactor" runat="server" ControlToValidate="txtFooterCoatingFactor"
                                                ErrorMessage="Coating factor is required." Font-Bold="True" ValidationGroup="vgFooterFormulaCoatingFactor"
                                                Text="<" SetFocusOnError="true" />
                                            <asp:CompareValidator runat="server" ID="cvFooterCoatingFactor" Operator="DataTypeCheck"
                                                ValidationGroup="vgFooterFormulaCoatingFactor" Type="double" Text="<" ControlToValidate="txtFooterCoatingFactor"
                                                ErrorMessage="Coating factor  must be a number." SetFocusOnError="True" />
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Obsolete" SortExpression="Obsolete">
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="cbEditFormulaCoatingFactorObsolete" runat="server" Checked='<%# Bind("Obsolete") %>' />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbViewFormulaCoatingFactorObsolete" runat="server" Checked='<%# Bind("Obsolete") %>'
                                                Enabled="false" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:CheckBox ID="cbFooterFormulaCoatingFactorObsolete" runat="server" Checked='<%# Bind("Obsolete") %>' />
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="iBtnFormulaCoatingFactorUpdate" runat="server" CausesValidation="True"
                                                CommandName="Update" ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditFormulaCoatingFactor" />
                                            <asp:ImageButton ID="iBtnFormulaCoatingFactorCancel" runat="server" CausesValidation="False"
                                                CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="iBtnFormulaCoatingFactorEdit" runat="server" CausesValidation="False"
                                                CommandName="Edit" ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgFooterFormulaCoatingFactor"
                                                runat="server" ID="iBtnFooterFormulaCoatingFactor" ImageUrl="~/images/save.jpg"
                                                AlternateText="Insert" />
                                            <asp:ImageButton ID="iBtnFormulaCoatingFactorUndo" runat="server" CommandName="Undo"
                                                CausesValidation="false" ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:ObjectDataSource ID="odsFormulaCoatingFactor" runat="server" OldValuesParameterFormatString="original_{0}"
                                SelectMethod="GetFormulaCoatingFactor" TypeName="FormulaCoatingFactorBLL" UpdateMethod="UpdateFormulaCoatingFactor"
                                InsertMethod="InsertFormulaCoatingFactor">
                                <SelectParameters>
                                    <asp:Parameter Name="FactorID" Type="Int32" />
                                    <asp:ControlParameter ControlID="lblFormulaIDValue" DefaultValue="0" Name="FormulaID"
                                        PropertyName="Text" Type="Int32" />
                                </SelectParameters>
                                <UpdateParameters>
                                    <asp:Parameter Name="original_FactorID" Type="Int32" />
                                    <asp:ControlParameter ControlID="lblFormulaIDValue" DefaultValue="0" Name="FormulaID"
                                        PropertyName="Text" Type="Int32" />
                                    <asp:Parameter Name="MinimumFactor" Type="Double" />
                                    <asp:Parameter Name="MaximumFactor" Type="Double" />
                                    <asp:Parameter Name="CoatingFactor" Type="Double" />
                                    <asp:Parameter Name="Obsolete" Type="Boolean" />
                                </UpdateParameters>
                                <InsertParameters>
                                    <asp:ControlParameter ControlID="lblFormulaIDValue" DefaultValue="0" Name="FormulaID"
                                        PropertyName="Text" Type="Int32" />
                                    <asp:Parameter Name="MinimumFactor" Type="Double" />
                                    <asp:Parameter Name="MaximumFactor" Type="Double" />
                                    <asp:Parameter Name="CoatingFactor" Type="Double" />
                                    <asp:Parameter Name="Obsolete" Type="Boolean" />
                                </InsertParameters>
                            </asp:ObjectDataSource>
                        </asp:View>
                        <asp:View ID="vHoleDeplugFactor" runat="server">
                            <asp:Label runat="server" ID="lblMessageHoleDeplugFactor"></asp:Label>
                            <asp:ValidationSummary ID="vsEditFormulaDeplugFactor" runat="server" DisplayMode="List"
                                ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgEditFormulaDeplugFactor" />
                            <asp:ValidationSummary ID="vsFooterFormulaDeplugFactor" runat="server" DisplayMode="List"
                                ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgFooterFormulaDeplugFactor" />
                            <asp:GridView runat="server" ID="gvFormulaDeplugFactor" AllowPaging="True" AllowSorting="True"
                                AutoGenerateColumns="False" PageSize="15" ShowFooter="True" DataSourceID="odsFormulaDeplugFactor"
                                DataKeyNames="FactorID" Width="100%" Visible="False">
                                <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#CCCCCC" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
                                <Columns>
                                    <asp:BoundField DataField="FactorID" SortExpression="FactorID" ReadOnly="True" />
                                    <asp:BoundField DataField="FormulaID" SortExpression="FormulaID" ReadOnly="True" />
                                    <asp:TemplateField HeaderText="Minimum" SortExpression="MinimumFactor">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditDeplugFactorMinimum" runat="server" Text='<%# Bind("MinimumFactor") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvEditDeplugFactorMinimum" runat="server" ControlToValidate="txtEditDeplugFactorMinimum"
                                                ErrorMessage="Minimum is required." Font-Bold="True" ValidationGroup="vgEditFormulaDeplugFactor"
                                                Text="<" SetFocusOnError="true" />
                                            <asp:CompareValidator runat="server" ID="cvEditDeplugFactorMinimum" Operator="DataTypeCheck"
                                                ValidationGroup="vgEditFormulaDeplugFactor" Type="double" Text="<" ControlToValidate="txtEditDeplugFactorMinimum"
                                                ErrorMessage="Minimum must be a number." SetFocusOnError="True" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewDeplugFactorMinimum" runat="server" Text='<%# Bind("MinimumFactor") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFooterDeplugFactorMinimum" runat="server" Text='<%# Bind("MinimumFactor") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvFooterDeplugFactorMinimum" runat="server" ControlToValidate="txtFooterDeplugFactorMinimum"
                                                ErrorMessage="Minimum is required." Font-Bold="True" ValidationGroup="vgFooterFormulaDeplugFactor"
                                                Text="<" SetFocusOnError="true" />
                                            <asp:CompareValidator runat="server" ID="cvFooterDeplugFactorMinimum" Operator="DataTypeCheck"
                                                ValidationGroup="vgFooterFormulaDeplugFactor" Type="double" Text="<" ControlToValidate="txtFooterDeplugFactorMinimum"
                                                ErrorMessage="Minimum must be a number." SetFocusOnError="True" />
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Maximum" SortExpression="MaximumFactor">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditDeplugFactorMaximum" runat="server" Text='<%# Bind("MaximumFactor") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvEditDeplugFactorMaximum" runat="server" ControlToValidate="txtEditDeplugFactorMaximum"
                                                ErrorMessage="Maximum is required." Font-Bold="True" ValidationGroup="vgEditFormulaDeplugFactor"
                                                Text="<" SetFocusOnError="true" />
                                            <asp:CompareValidator runat="server" ID="cvEditDeplugFactorMaximum" Operator="DataTypeCheck"
                                                ValidationGroup="vgEditFormulaDeplugFactor" Type="double" Text="<" ControlToValidate="txtEditDeplugFactorMaximum"
                                                ErrorMessage="Maximum must be a number." SetFocusOnError="True" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewDeplugFactorMaximum" runat="server" Text='<%# Bind("MaximumFactor") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFooterDeplugFactorMaximum" runat="server" Text='<%# Bind("MaximumFactor") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvFooterDeplugFactorMaximum" runat="server" ControlToValidate="txtFooterDeplugFactorMaximum"
                                                ErrorMessage="Maximum is required." Font-Bold="True" ValidationGroup="vgFooterFormulaDeplugFactor"
                                                Text="<" SetFocusOnError="true" />
                                            <asp:CompareValidator runat="server" ID="cvFooterDeplugFactorMaximum" Operator="DataTypeCheck"
                                                ValidationGroup="vgFooterFormulaDeplugFactor" Type="double" Text="<" ControlToValidate="txtFooterDeplugFactorMaximum"
                                                ErrorMessage="Maximum must be a number." SetFocusOnError="True" />
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Deplug Factor" SortExpression="DeplugFactor">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditDeplugFactor" runat="server" Text='<%# Bind("DeplugFactor") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvEditDeplugFactor" runat="server" ControlToValidate="txtEditDeplugFactor"
                                                ErrorMessage="Deplug factor is required." Font-Bold="True" ValidationGroup="vgEditFormulaDeplugFactor"
                                                Text="<" SetFocusOnError="true" />
                                            <asp:CompareValidator runat="server" ID="cvEditDeplugFactor" Operator="DataTypeCheck"
                                                ValidationGroup="vgEditFormulaDeplugFactor" Type="double" Text="<" ControlToValidate="txtEditDeplugFactor"
                                                ErrorMessage="Deplug factor  must be a number." SetFocusOnError="True" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewDeplugFactor" runat="server" Text='<%# Bind("DeplugFactor") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFooterDeplugFactor" runat="server" Text='<%# Bind("DeplugFactor") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvFooterDeplugFactor" runat="server" ControlToValidate="txtFooterDeplugFactor"
                                                ErrorMessage="Deplug factor is required." Font-Bold="True" ValidationGroup="vgFooterFormulaDeplugFactor"
                                                Text="<" SetFocusOnError="true" />
                                            <asp:CompareValidator runat="server" ID="cvFooterDeplugFactor" Operator="DataTypeCheck"
                                                ValidationGroup="vgFooterFormulaDeplugFactor" Type="double" Text="<" ControlToValidate="txtFooterDeplugFactor"
                                                ErrorMessage="Deplug factor  must be a number." SetFocusOnError="True" />
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Obsolete" SortExpression="Obsolete">
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="cbEditFormulaDeplugFactorObsolete" runat="server" Checked='<%# Bind("Obsolete") %>' />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbViewFormulaDeplugFactorObsolete" runat="server" Checked='<%# Bind("Obsolete") %>'
                                                Enabled="false" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:CheckBox ID="cbFooterFormulaDeplugFactorObsolete" runat="server" Checked='<%# Bind("Obsolete") %>' />
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="iBtnFormulaDeplugFactorUpdate" runat="server" CausesValidation="True"
                                                CommandName="Update" ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditFormulaDeplugFactor" />
                                            <asp:ImageButton ID="iBtnFormulaDeplugFactorCancel" runat="server" CausesValidation="False"
                                                CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="iBtnFormulaDeplugFactorEdit" runat="server" CausesValidation="False"
                                                CommandName="Edit" ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgFooterFormulaDeplugFactor"
                                                runat="server" ID="iBtnFooterFormulaDeplugFactor" ImageUrl="~/images/save.jpg"
                                                AlternateText="Insert" />
                                            <asp:ImageButton ID="iBtnFormulaDeplugFactorUndo" runat="server" CommandName="Undo"
                                                CausesValidation="false" ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:ObjectDataSource ID="odsFormulaDeplugFactor" runat="server" OldValuesParameterFormatString="original_{0}"
                                SelectMethod="GetFormulaDeplugFactor" TypeName="FormulaDeplugFactorBLL" InsertMethod="InsertFormulaDeplugFactor"
                                UpdateMethod="UpdateFormulaDeplugFactor">
                                <SelectParameters>
                                    <asp:Parameter Name="FactorID" Type="Int32" />
                                    <asp:ControlParameter ControlID="lblFormulaIDValue" DefaultValue="0" Name="FormulaID"
                                        PropertyName="Text" Type="Int32" />
                                </SelectParameters>
                                <UpdateParameters>
                                    <asp:Parameter Name="original_FactorID" Type="Int32" />
                                    <asp:QueryStringParameter Name="FormulaID" QueryStringField="FormulaID" Type="Int32" />
                                    <asp:Parameter Name="MinimumFactor" Type="Double" />
                                    <asp:Parameter Name="MaximumFactor" Type="Double" />
                                    <asp:Parameter Name="DeplugFactor" Type="Double" />
                                    <asp:Parameter Name="Obsolete" Type="Boolean" />
                                </UpdateParameters>
                                <InsertParameters>
                                    <asp:Parameter Name="FormulaID" Type="Int32" />
                                    <asp:Parameter Name="MinimumFactor" Type="Double" />
                                    <asp:Parameter Name="MaximumFactor" Type="Double" />
                                    <asp:Parameter Name="DeplugFactor" Type="Double" />
                                    <asp:Parameter Name="Obsolete" Type="Boolean" />
                                </InsertParameters>
                            </asp:ObjectDataSource>
                        </asp:View>
                        <asp:View ID="vMaterial" runat="server">
                            <asp:Label runat="server" ID="lblMessageMaterial"></asp:Label>
                            <asp:ValidationSummary ID="vsEditFormulaMaterial" runat="server" DisplayMode="List"
                                ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgEditFormulaMaterial" />
                            <asp:ValidationSummary ID="vsFooterFormulaMaterial" runat="server" DisplayMode="List"
                                ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgFooterFormulaMaterial" />
                            <asp:GridView runat="server" ID="gvFormulaMaterial" AllowPaging="True" AllowSorting="True"
                                AutoGenerateColumns="False" PageSize="15" ShowFooter="True" DataSourceID="odsFormulaMaterial"
                                DataKeyNames="RowID" Width="100%" Visible="False">
                                <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#CCCCCC" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
                                <Columns>
                                    <asp:BoundField DataField="RowID" HeaderText="Row ID" SortExpression="RowID" ReadOnly="True" />
                                    <asp:BoundField DataField="FormulaID" SortExpression="FormulaID" ReadOnly="True" />
                                    <asp:BoundField DataField="MaterialID" HeaderText="Material ID" SortExpression="MaterialID"
                                        ReadOnly="true">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Material" SortExpression="MaterialName">
                                        <EditItemTemplate>
                                            <asp:Label ID="lblEditFormulaMaterialName" runat="server" Text='<%# Bind("ddMaterialNameCombo") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewFormulaMaterialName" runat="server" Text='<%# Bind("ddMaterialNameCombo") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddFooterMaterialName" runat="server" DataSource='<%# CostingModule.GetMaterial(0, "", "",  "", 0, 0, "", "", False, False, False, False, False, False) %>'
                                                DataValueField="MaterialID" DataTextField="ddMaterialNameCombo" AppendDataBoundItems="True">
                                                <asp:ListItem Text="" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:ImageButton ID="ibtnGetMaterial" runat="server" CausesValidation="False" ImageUrl="~/images/Search.gif"
                                                ToolTip="Get Material" AlternateText="Get Material" />
                                            <asp:RequiredFieldValidator ID="rfvFooterMaterial" runat="server" ControlToValidate="ddFooterMaterialName"
                                                ErrorMessage="The name is required." Font-Bold="True" ValidationGroup="vgFooterFormulaMaterial"
                                                Text="<" SetFocusOnError="true" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DrawingNo" HeaderText="Drawing No" SortExpression="DrawingNo"
                                        ReadOnly="True" />
                                    <asp:TemplateField HeaderText="Usage Factor" SortExpression="UsageFactor">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditFormulaMaterialUsageFactor" runat="server" MaxLength="10"
                                                Width="75px" Text='<%# Bind("UsageFactor") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvEditFormulaMaterialUsageFactor" runat="server"
                                                ControlToValidate="txtEditFormulaMaterialUsageFactor" ErrorMessage="The usage factor is required."
                                                Font-Bold="True" ValidationGroup="vgEditFormulaMaterial" Text="<" SetFocusOnError="true" />
                                            <asp:CompareValidator runat="server" ID="cvEditFormulaMaterialUsageFactor" Operator="DataTypeCheck"
                                                ValidationGroup="vgEditFormulaMaterial" Type="double" Text="<" ControlToValidate="txtEditFormulaMaterialUsageFactor"
                                                ErrorMessage="Usage factor must be a number." SetFocusOnError="True" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewFormulaMaterialUsageFactor" runat="server" Text='<%# Bind("UsageFactor") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFooterFormulaMaterialUsageFactor" runat="server" MaxLength="10"
                                                Width="75px" Text='<%# Bind("UsageFactor") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvFooterFormulaMaterialUsageFactor" runat="server"
                                                ControlToValidate="txtFooterFormulaMaterialUsageFactor" ErrorMessage="The usage factor is required."
                                                Font-Bold="True" ValidationGroup="vgFooterFormulaMaterial" Text="<" SetFocusOnError="true" />
                                            <asp:CompareValidator runat="server" ID="cvEditFormulaMaterialUsageFactor" Operator="DataTypeCheck"
                                                ValidationGroup="vgFooterFormulaMaterial" Type="double" Text="<" ControlToValidate="txtFooterFormulaMaterialUsageFactor"
                                                ErrorMessage="Usage factor must be a number." SetFocusOnError="True" />
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ordinal" SortExpression="Ordinal">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditFormulaMaterialOrdinal" runat="server" MaxLength="10" Width="50px"
                                                Text='<%# Bind("Ordinal") %>'></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvEditFormulaMaterialOrdinal" Operator="DataTypeCheck"
                                                ValidationGroup="vgEditFormulaMaterial" Type="integer" Text="<" ControlToValidate="txtEditFormulaMaterialOrdinal"
                                                ErrorMessage="Ordinal must be an integer." SetFocusOnError="True" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewFormulaMaterialOrdinal" runat="server" Text='<%# Bind("Ordinal") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFooterFormulaMaterialOrdinal" runat="server" MaxLength="10" Width="50px"
                                                Text="99"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvFooterFormulaMaterialOrdinal" Operator="DataTypeCheck"
                                                ValidationGroup="vgFooterFormulaMaterial" Type="integer" Text="<" ControlToValidate="txtFooterFormulaMaterialOrdinal"
                                                ErrorMessage="Ordinal must be an integer." SetFocusOnError="True" />
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Obsolete" SortExpression="Obsolete">
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="cbEditFormulaMaterialObsolete" runat="server" Checked='<%# Bind("Obsolete") %>' />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbViewFormulaMaterialObsolete" runat="server" Checked='<%# Bind("Obsolete") %>'
                                                Enabled="false" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:CheckBox ID="cbFooterFormulaMaterialObsolete" runat="server" Checked='<%# Bind("Obsolete") %>' />
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="iBtnFormulaMaterialUpdate" runat="server" CausesValidation="True"
                                                CommandName="Update" ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditFormulaMaterial" />
                                            <asp:ImageButton ID="iBtnFormulaMaterialCancel" runat="server" CausesValidation="False"
                                                CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="iBtnFormulaMaterialEdit" runat="server" CausesValidation="False"
                                                CommandName="Edit" ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgFooterFormulaMaterial"
                                                runat="server" ID="iBtnFooterFormulaMaterial" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                                            <asp:ImageButton ID="iBtnFormulaMaterialUndo" runat="server" CommandName="Undo" CausesValidation="false"
                                                ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:ObjectDataSource ID="odsFormulaMaterial" runat="server" OldValuesParameterFormatString="original_{0}"
                                SelectMethod="GetFormulaMaterial" TypeName="FormulaMaterialBLL" UpdateMethod="UpdateFormulaMaterial"
                                InsertMethod="InsertFormulaMaterial">
                                <SelectParameters>
                                    <asp:Parameter Name="RowID" Type="Int32" />
                                    <asp:ControlParameter ControlID="lblFormulaIDValue" Name="FormulaID" PropertyName="Text"
                                        Type="Int32" />
                                    <asp:Parameter Name="MaterialID" Type="Int32" />
                                </SelectParameters>
                                <UpdateParameters>
                                    <asp:Parameter Name="original_RowID" Type="Int32" />
                                    <asp:Parameter Name="UsageFactor" Type="Double" />
                                    <asp:Parameter Name="Ordinal" Type="Int32" />
                                    <asp:Parameter Name="Obsolete" Type="Boolean" />
                                    <asp:Parameter Name="ddMaterialName" Type="String" />
                                </UpdateParameters>
                                <InsertParameters>
                                    <asp:Parameter Name="FormulaID" Type="Int32" />
                                    <asp:Parameter Name="MaterialID" Type="Int32" />
                                    <asp:Parameter Name="UsageFactor" Type="Double" />
                                    <asp:Parameter Name="Ordinal" Type="Int32" />
                                    <asp:Parameter Name="Obsolete" Type="Boolean" />
                                </InsertParameters>
                            </asp:ObjectDataSource>
                        </asp:View>
                        <asp:View ID="vPackaging" runat="server">
                            <asp:Label runat="server" ID="lblMessagePackaging"></asp:Label>
                            <asp:ValidationSummary ID="vsEditFormulaPackaging" runat="server" DisplayMode="List"
                                ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgEditFormulaPackaging" />
                            <asp:ValidationSummary ID="vsFooterFormulaPackaging" runat="server" DisplayMode="List"
                                ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgFooterFormulaPackaging" />
                            <asp:GridView runat="server" ID="gvFormulaPackaging" AllowPaging="True" AllowSorting="True"
                                AutoGenerateColumns="False" PageSize="15" ShowFooter="True" DataSourceID="odsFormulaPackaging"
                                DataKeyNames="RowID" Width="100%" Visible="False">
                                <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#CCCCCC" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
                                <Columns>
                                    <asp:BoundField DataField="RowID" HeaderText="Row ID" SortExpression="RowID" ReadOnly="True" />
                                    <asp:BoundField DataField="FormulaID" SortExpression="FormulaID" ReadOnly="True" />
                                    <asp:BoundField DataField="MaterialID" HeaderText="Material ID" SortExpression="MaterialID"
                                        ReadOnly="true">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Material" SortExpression="MaterialName">
                                        <EditItemTemplate>
                                            <asp:Label ID="lblEditFormulaPackagingName" runat="server" Text='<%# Bind("ddMaterialNameCombo") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewFormulaPackagingName" runat="server" Text='<%# Bind("ddMaterialNameCombo") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddFooterPackagingName" runat="server" DataSource='<%# CostingModule.GetMaterial(0, "", "",  "", 0, 0, "", "", False, False, False, False, False, False) %>'
                                                DataValueField="MaterialID" DataTextField="ddMaterialNameCombo" AppendDataBoundItems="True"
                                                SelectedValue='<%# Bind("MaterialID") %>'>
                                                <asp:ListItem Text="" Value="0" Selected="False"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:ImageButton ID="ibtnGetPackaging" runat="server" CausesValidation="False" ImageUrl="~/images/Search.gif"
                                                ToolTip="Get Packaging" AlternateText="Get Packaging" />
                                            <asp:RequiredFieldValidator ID="rfvFooterMaterial" runat="server" ControlToValidate="ddFooterPackagingName"
                                                ErrorMessage="The name is required." Font-Bold="True" ValidationGroup="vgFooterFormulaPackaging"
                                                Text="<" SetFocusOnError="true" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DrawingNo" HeaderText="Drawing No" SortExpression="DrawingNo"
                                        ReadOnly="True" />
                                    <asp:TemplateField HeaderText="Ordinal" SortExpression="Ordinal">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditFormulaPackagingOrdinal" runat="server" MaxLength="10" Width="50px"
                                                Text='<%# Bind("Ordinal") %>'></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvEditFormulaPackagingOrdinal" Operator="DataTypeCheck"
                                                ValidationGroup="vgEditFormulaPackaging" Type="integer" Text="<" ControlToValidate="txtEditFormulaPackagingOrdinal"
                                                ErrorMessage="Ordinal must be an integer." SetFocusOnError="True" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewFormulaPackagingOrdinal" runat="server" Text='<%# Bind("Ordinal") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFooterFormulaPackagingOrdinal" runat="server" MaxLength="10"
                                                Width="50px" Text="99"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvFooterFormulaPackagingOrdinal" Operator="DataTypeCheck"
                                                ValidationGroup="vgFooterFormulaPackaging" Type="integer" Text="<" ControlToValidate="txtFooterFormulaPackagingOrdinal"
                                                ErrorMessage="Ordinal must be an integer." SetFocusOnError="True" />
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Obsolete" SortExpression="Obsolete">
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="cbEditFormulaPackagingObsolete" runat="server" Checked='<%# Bind("Obsolete") %>' />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbViewFormulaPackagingObsolete" runat="server" Checked='<%# Bind("Obsolete") %>'
                                                Enabled="false" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:CheckBox ID="cbFooterFormulaPackagingObsolete" runat="server" Checked='<%# Bind("Obsolete") %>' />
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="iBtnFormulaPackagingUpdate" runat="server" CausesValidation="True"
                                                CommandName="Update" ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditFormulaPackaging" />
                                            <asp:ImageButton ID="iBtnFormulaPackagingCancel" runat="server" CausesValidation="False"
                                                CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="iBtnFormulaPackagingEdit" runat="server" CausesValidation="False"
                                                CommandName="Edit" ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgFooterFormulaPackaging"
                                                runat="server" ID="iBtnFooterFormulaPackaging" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                                            <asp:ImageButton ID="iBtnFormulaPackagingUndo" runat="server" CommandName="Undo"
                                                CausesValidation="false" ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:ObjectDataSource ID="odsFormulaPackaging" runat="server" OldValuesParameterFormatString="original_{0}"
                                InsertMethod="InsertFormulaPackaging" SelectMethod="GetFormulaPackaging" TypeName="FormulaPackagingBLL"
                                UpdateMethod="UpdateFormulaPackaging">
                                <UpdateParameters>
                                    <asp:Parameter Name="original_RowID" Type="Int32" />
                                    <asp:Parameter Name="Ordinal" Type="Int32" />
                                    <asp:Parameter Name="Obsolete" Type="Boolean" />
                                    <asp:Parameter Name="ddMaterialName" Type="String" />
                                </UpdateParameters>
                                <SelectParameters>
                                    <asp:Parameter Name="RowID" Type="Int32" />
                                    <asp:ControlParameter ControlID="lblFormulaIDValue" DefaultValue="0" Name="FormulaID"
                                        PropertyName="Text" Type="Int32" />
                                    <asp:Parameter Name="MaterialID" Type="Int32" />
                                </SelectParameters>
                                <InsertParameters>
                                    <asp:Parameter Name="FormulaID" Type="Int32" />
                                    <asp:Parameter Name="MaterialID" Type="Int32" />
                                    <asp:Parameter Name="Ordinal" Type="Int32" />
                                    <asp:Parameter Name="Obsolete" Type="Boolean" />
                                </InsertParameters>
                            </asp:ObjectDataSource>
                        </asp:View>
                        <asp:View ID="vLabor" runat="server">
                            <asp:Label runat="server" ID="lblMessageLabor"></asp:Label>
                            <asp:ValidationSummary ID="vsEditFormulaLabor" runat="server" DisplayMode="List"
                                ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgEditFormulaLabor" />
                            <asp:ValidationSummary ID="vsFooterFormulaLabor" runat="server" DisplayMode="List"
                                ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgFooterFormulaLabor" />
                            <asp:GridView runat="server" ID="gvFormulaLabor" AllowPaging="True" AllowSorting="True"
                                AutoGenerateColumns="False" PageSize="15" ShowFooter="True" DataSourceID="odsFormulaLabor"
                                DataKeyNames="RowID" Width="100%" Visible="False">
                                <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#CCCCCC" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
                                <Columns>
                                    <asp:BoundField DataField="RowID" HeaderText="Row ID" SortExpression="RowID" ReadOnly="True" />
                                    <asp:BoundField DataField="FormulaID" SortExpression="FormulaID" ReadOnly="True" />
                                    <asp:BoundField DataField="LaborID" HeaderText="Labor ID" SortExpression="LaborID"
                                        ReadOnly="True" />
                                    <asp:TemplateField HeaderText="Description" SortExpression="LaborDesc">
                                        <EditItemTemplate>
                                            <asp:Label ID="lblEditFormulaLaborDesc" runat="server" Text='<%# Bind("ddLaborDesc") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewFormulaLaborDesc" runat="server" Text='<%# Bind("ddLaborDesc") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddFooterLaborDesc" runat="server" DataSource='<%# CostingModule.GetLabor(0,"",False,False) %>'
                                                DataValueField="LaborID" DataTextField="ddLaborDesc" AppendDataBoundItems="True"
                                                SelectedValue='<%# Bind("LaborID") %>'>
                                                <asp:ListItem Text="" Value="0" Selected="False"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvFooterLabor" runat="server" ControlToValidate="ddFooterLaborDesc"
                                                ErrorMessage="The description is required." Font-Bold="True" ValidationGroup="vgFooterFormulaLabor"
                                                Text="<" SetFocusOnError="true" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ordinal" SortExpression="Ordinal">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditFormulaLaborOrdinal" runat="server" MaxLength="10" Width="50px"
                                                Text='<%# Bind("Ordinal") %>'></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvEditFormulaLaborOrdinal" Operator="DataTypeCheck"
                                                ValidationGroup="vgEditFormulaLabor" Type="integer" Text="<" ControlToValidate="txtEditFormulaLaborOrdinal"
                                                ErrorMessage="Ordinal must be an integer." SetFocusOnError="True" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewFormulaLaborOrdinal" runat="server" Text='<%# Bind("Ordinal") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFooterFormulaLaborOrdinal" runat="server" MaxLength="10" Width="50px"
                                                Text="99"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvFooterFormulaLaborOrdinal" Operator="DataTypeCheck"
                                                ValidationGroup="vgFooterFormulaLabor" Type="integer" Text="<" ControlToValidate="txtFooterFormulaLaborOrdinal"
                                                ErrorMessage="Ordinal must be an integer." SetFocusOnError="True" />
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Obsolete" SortExpression="Obsolete">
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="cbEditFormulaLaborObsolete" runat="server" Checked='<%# Bind("Obsolete") %>' />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbViewFormulaLaborObsolete" runat="server" Checked='<%# Bind("Obsolete") %>'
                                                Enabled="false" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:CheckBox ID="cbFooterFormulaLaborObsolete" runat="server" Checked='<%# Bind("Obsolete") %>' />
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="iBtnFormulaLaborUpdate" runat="server" CausesValidation="True"
                                                CommandName="Update" ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditFormulaLabor" />
                                            <asp:ImageButton ID="iBtnFormulaLaborCancel" runat="server" CausesValidation="False"
                                                CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="iBtnFormulaLaborEdit" runat="server" CausesValidation="False"
                                                CommandName="Edit" ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgFooterFormulaLabor"
                                                runat="server" ID="iBtnFooterFormulaLabor" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                                            <asp:ImageButton ID="iBtnFormulaLaborUndo" runat="server" CommandName="Undo" CausesValidation="false"
                                                ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:ObjectDataSource ID="odsFormulaLabor" runat="server" OldValuesParameterFormatString="original_{0}"
                                InsertMethod="InsertFormulaLabor" SelectMethod="GetFormulaLabor" TypeName="FormulaLaborBLL"
                                UpdateMethod="UpdateFormulaLabor">
                                <UpdateParameters>
                                    <asp:Parameter Name="original_RowID" Type="Int32" />
                                    <asp:Parameter Name="Ordinal" Type="Int32" />
                                    <asp:Parameter Name="Obsolete" Type="Boolean" />
                                    <asp:Parameter Name="ddLaborDesc" Type="String" />
                                </UpdateParameters>
                                <SelectParameters>
                                    <asp:Parameter Name="RowID" Type="Int32" />
                                    <asp:ControlParameter ControlID="lblFormulaIDValue" DefaultValue="0" Name="FormulaID"
                                        PropertyName="Text" Type="Int32" />
                                    <asp:Parameter Name="LaborID" Type="Int32" />
                                </SelectParameters>
                                <InsertParameters>
                                    <asp:Parameter Name="FormulaID" Type="Int32" />
                                    <asp:Parameter Name="LaborID" Type="Int32" />
                                    <asp:Parameter Name="Ordinal" Type="Int32" />
                                    <asp:Parameter Name="Obsolete" Type="Boolean" />
                                </InsertParameters>
                            </asp:ObjectDataSource>
                        </asp:View>
                        <asp:View ID="vOverhead" runat="server">
                            <asp:Label runat="server" ID="lblMessageOverhead"></asp:Label>
                            <asp:ValidationSummary ID="vsEditFormulaOverhead" runat="server" DisplayMode="List"
                                ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgEditFormulaOverhead" />
                            <asp:ValidationSummary ID="vsFooterFormulaOverhead" runat="server" DisplayMode="List"
                                ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgFooterFormulaOverhead" />
                            <asp:GridView runat="server" ID="gvFormulaOverhead" AllowPaging="True" AllowSorting="True"
                                AutoGenerateColumns="False" PageSize="15" ShowFooter="True" DataSourceID="odsFormulaOverhead"
                                DataKeyNames="RowID" Width="100%" Visible="False">
                                <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#CCCCCC" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
                                <Columns>
                                    <asp:BoundField DataField="RowID" HeaderText="Row ID" SortExpression="RowID" ReadOnly="True" />
                                    <asp:BoundField DataField="FormulaID" SortExpression="FormulaID" ReadOnly="True" />
                                    <asp:BoundField DataField="LaborID" HeaderText="Labor ID" SortExpression="LaborID"
                                        ReadOnly="True" />
                                    <asp:TemplateField HeaderText="Description" SortExpression="LaborDesc">
                                        <EditItemTemplate>
                                            <asp:Label ID="lblEditFormulaOverheadDesc" runat="server" Text='<%# Bind("ddLaborDesc") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewFormulaOverheadDesc" runat="server" Text='<%# Bind("ddLaborDesc") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddFooterOverheadDesc" runat="server" DataSource='<%# CostingModule.GetLabor(0,"",False,False) %>'
                                                DataValueField="LaborID" DataTextField="ddLaborDesc" AppendDataBoundItems="True"
                                                SelectedValue='<%# Bind("LaborID") %>'>
                                                <asp:ListItem Text="" Value="0" Selected="False"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvFooterOverhead" runat="server" ControlToValidate="ddFooterOverheadDesc"
                                                ErrorMessage="The description is required." Font-Bold="True" ValidationGroup="vgFooterFormulaOverhead"
                                                Text="<" SetFocusOnError="true" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ordinal" SortExpression="Ordinal">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditFormulaOverheadOrdinal" runat="server" MaxLength="10" Width="50px"
                                                Text='<%# Bind("Ordinal") %>'></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvEditFormulaOverheadOrdinal" Operator="DataTypeCheck"
                                                ValidationGroup="vgEditFormulaOverhead" Type="integer" Text="<" ControlToValidate="txtEditFormulaOverheadOrdinal"
                                                ErrorMessage="Ordinal must be an integer." SetFocusOnError="True" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewFormulaOverheadOrdinal" runat="server" Text='<%# Bind("Ordinal") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFooterFormulaOverheadOrdinal" runat="server" MaxLength="10" Width="50px"
                                                Text="99"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvFooterFormulaOverheadOrdinal" Operator="DataTypeCheck"
                                                ValidationGroup="vgFooterFormulaOverhead" Type="integer" Text="<" ControlToValidate="txtFooterFormulaOverheadOrdinal"
                                                ErrorMessage="Ordinal must be an integer." SetFocusOnError="True" />
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Obsolete" SortExpression="Obsolete">
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="cbEditFormulaOverheadObsolete" runat="server" Checked='<%# Bind("Obsolete") %>' />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbViewFormulaOverheadObsolete" runat="server" Checked='<%# Bind("Obsolete") %>'
                                                Enabled="false" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:CheckBox ID="cbFooterFormulaOverheadObsolete" runat="server" Checked='<%# Bind("Obsolete") %>' />
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="iBtnFormulaOverheadUpdate" runat="server" CausesValidation="True"
                                                CommandName="Update" ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditFormulaOverhead" />
                                            <asp:ImageButton ID="iBtnFormulaOverheadCancel" runat="server" CausesValidation="False"
                                                CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="iBtnFormulaOverheadEdit" runat="server" CausesValidation="False"
                                                CommandName="Edit" ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgFooterFormulaOverhead"
                                                runat="server" ID="iBtnFooterFormulaOverhead" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                                            <asp:ImageButton ID="iBtnFormulaOverheadUndo" runat="server" CommandName="Undo" CausesValidation="false"
                                                ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:ObjectDataSource ID="odsFormulaOverhead" runat="server" OldValuesParameterFormatString="original_{0}"
                                InsertMethod="InsertFormulaOverhead" SelectMethod="GetFormulaOverhead" TypeName="FormulaOverheadBLL"
                                UpdateMethod="UpdateFormulaOverhead">
                                <UpdateParameters>
                                    <asp:Parameter Name="original_RowID" Type="Int32" />
                                    <asp:Parameter Name="Ordinal" Type="Int32" />
                                    <asp:Parameter Name="Obsolete" Type="Boolean" />
                                    <asp:Parameter Name="ddOverheadDesc" Type="String" />
                                    <asp:Parameter Name="ddLaborDesc" Type="String" />
                                </UpdateParameters>
                                <SelectParameters>
                                    <asp:Parameter Name="RowID" Type="Int32" />
                                    <asp:ControlParameter ControlID="lblFormulaIDValue" DefaultValue="0" Name="FormulaID"
                                        PropertyName="Text" Type="Int32" />
                                    <asp:Parameter Name="LaborID" Type="Int32" />
                                </SelectParameters>
                                <InsertParameters>
                                    <asp:ControlParameter ControlID="lblFormulaIDValue" DefaultValue="0" Name="FormulaID"
                                        PropertyName="Text" Type="Int32" />
                                    <asp:Parameter Name="LaborID" Type="Int32" />
                                    <asp:Parameter Name="Ordinal" Type="Int32" />
                                    <asp:Parameter Name="Obsolete" Type="Boolean" />
                                </InsertParameters>
                            </asp:ObjectDataSource>
                        </asp:View>
                        <asp:View ID="vMiscCosts" runat="server">
                            <asp:Label runat="server" ID="lblMessageMiscCost"></asp:Label>
                            <asp:ValidationSummary ID="vsEditFormulaMiscCost" runat="server" DisplayMode="List"
                                ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgEditFormulaMiscCost" />
                            <asp:ValidationSummary ID="vsFooterFormulaMiscCost" runat="server" DisplayMode="List"
                                ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgFooterFormulaMiscCost" />
                            <asp:GridView runat="server" ID="gvFormulaMiscCost" AllowPaging="True" AllowSorting="True"
                                AutoGenerateColumns="False" PageSize="15" ShowFooter="True" DataSourceID="odsFormulaMiscCost"
                                DataKeyNames="RowID" Width="100%" Visible="False">
                                <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#CCCCCC" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
                                <Columns>
                                    <asp:BoundField DataField="RowID" HeaderText="Row ID" SortExpression="RowID" ReadOnly="True" />
                                    <asp:BoundField DataField="FormulaID" SortExpression="FormulaID" ReadOnly="True" />
                                    <asp:BoundField DataField="MiscCostID" HeaderText="Misc Cost ID" SortExpression="MiscCostID"
                                        ReadOnly="True" />
                                    <asp:TemplateField HeaderText="Description" SortExpression="MiscCostDesc">
                                        <EditItemTemplate>
                                            <asp:Label ID="lblEditFormulaMiscCostDesc" runat="server" Text='<%# Bind("ddMiscCostDesc") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewFormulaMiscCostDesc" runat="server" Text='<%# Bind("ddMiscCostDesc") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddFooterMiscCostDesc" runat="server" DataSource='<%# CostingModule.GetMiscCost(0,"") %>'
                                                DataValueField="MiscCostID" DataTextField="ddMiscCostDesc" AppendDataBoundItems="True"
                                                SelectedValue='<%# Bind("MiscCostID") %>'>
                                                <asp:ListItem Text="" Value="0" Selected="False"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvFooterMiscCost" runat="server" ControlToValidate="ddFooterMiscCostDesc"
                                                ErrorMessage="The description is required." Font-Bold="True" ValidationGroup="vgFooterFormulaMiscCost"
                                                Text="<" SetFocusOnError="true" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ordinal" SortExpression="Ordinal">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditFormulaMiscCostOrdinal" runat="server" MaxLength="10" Width="50px"
                                                Text='<%# Bind("Ordinal") %>'></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvEditFormulaMiscCostOrdinal" Operator="DataTypeCheck"
                                                ValidationGroup="vgEditFormulaMiscCost" Type="integer" Text="<" ControlToValidate="txtEditFormulaMiscCostOrdinal"
                                                ErrorMessage="Ordinal must be an integer." SetFocusOnError="True" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewFormulaMiscCostOrdinal" runat="server" Text='<%# Bind("Ordinal") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFooterFormulaMiscCostOrdinal" runat="server" MaxLength="10" Width="50px"
                                                Text="99"></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cvFooterFormulaMiscCostOrdinal" Operator="DataTypeCheck"
                                                ValidationGroup="vgFooterFormulaMiscCost" Type="integer" Text="<" ControlToValidate="txtFooterFormulaMiscCostOrdinal"
                                                ErrorMessage="Ordinal must be an integer." SetFocusOnError="True" />
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Obsolete" SortExpression="Obsolete">
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="cbEditFormulaMiscCostObsolete" runat="server" Checked='<%# Bind("Obsolete") %>' />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbViewFormulaMiscCostObsolete" runat="server" Checked='<%# Bind("Obsolete") %>'
                                                Enabled="false" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:CheckBox ID="cbFooterFormulaMiscCostObsolete" runat="server" Checked='<%# Bind("Obsolete") %>' />
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="iBtnFormulaMiscCostUpdate" runat="server" CausesValidation="True"
                                                CommandName="Update" ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditFormulaMiscCost" />
                                            <asp:ImageButton ID="iBtnFormulaMiscCostCancel" runat="server" CausesValidation="False"
                                                CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="iBtnFormulaMiscCostEdit" runat="server" CausesValidation="False"
                                                CommandName="Edit" ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgFooterFormulaMiscCost"
                                                runat="server" ID="iBtnFooterFormulaMiscCost" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                                            <asp:ImageButton ID="iBtnFormulaMiscCostUndo" runat="server" CommandName="Undo" CausesValidation="false"
                                                ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:ObjectDataSource ID="odsFormulaMiscCost" runat="server" OldValuesParameterFormatString="original_{0}"
                                InsertMethod="InsertFormulaMiscCost" SelectMethod="GetFormulaMiscCost" TypeName="FormulaMiscCostBLL"
                                UpdateMethod="UpdateFormulaMiscCost">
                                <UpdateParameters>
                                    <asp:Parameter Name="original_RowID" Type="Int32" />
                                    <asp:Parameter Name="Ordinal" Type="Int32" />
                                    <asp:Parameter Name="Obsolete" Type="Boolean" />
                                    <asp:Parameter Name="ddMiscCostDesc" Type="String" />
                                </UpdateParameters>
                                <SelectParameters>
                                    <asp:Parameter Name="RowID" Type="Int32" />
                                    <asp:ControlParameter ControlID="lblFormulaIDValue" DefaultValue="0" Name="FormulaID"
                                        PropertyName="Text" Type="Int32" />
                                    <asp:Parameter Name="MiscCostID" Type="Int32" />
                                </SelectParameters>
                                <InsertParameters>
                                    <asp:Parameter Name="FormulaID" Type="Int32" />
                                    <asp:Parameter Name="MiscCostID" Type="Int32" />
                                    <asp:Parameter Name="Ordinal" Type="Int32" />
                                    <asp:Parameter Name="Obsolete" Type="Boolean" />
                                </InsertParameters>
                            </asp:ObjectDataSource>
                        </asp:View>
                    </asp:MultiView>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
