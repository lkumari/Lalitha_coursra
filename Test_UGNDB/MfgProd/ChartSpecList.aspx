<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="ChartSpecList.aspx.vb" Inherits="MfgProd_ChartSpecList" Title="UGNDB - Chart Spec"
    MaintainScrollPositionOnPostback="True" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
        <br />
        <table>
            <tr>
                <td class="p_smalltextbold" style="color: #990000">
                    Review existing data or press
                    <asp:Button ID="btnAdd" runat="server" Text="Add" />
                    to enter new data.
                </td>
            </tr>
        </table>
        <hr />
        <br />
        <asp:Panel ID="FEPanel" runat="server" CssClass="collapsePanelHeader" Width="680px">
            <asp:Image ID="imgFE" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                Height="12px" />&nbsp;
            <asp:Label ID="lblFE" runat="server" Text="Label" CssClass="c_textbold">Filter Part Specification table below:</asp:Label>
        </asp:Panel>
        <asp:Panel ID="FEContentPanel" runat="server" CssClass="collapsePanel">
            <asp:Label ID="lblRowID" runat="server" Text="" CssClass="c_text" ForeColor="Red"
                Font-Bold="True" Font-Overline="False" Font-Size="Larger" Font-Underline="False"></asp:Label>
            <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
            <br />
            <table>
                <tr>
                    <td class="p_text">
                        UGN Location:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddUGNLocation" runat="server" />
                    </td>
                    <td class="p_text">
                        Department:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddDepartment" runat="server" />
                    </td>
                    <td class="p_text">
                        Work Center:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddWorkCenter" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="p_text">
                        OEM Manufacturer:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddOEMMfg" runat="server" />
                    </td>
                    <td class="p_text">
                        Customer:
                    </td>
                    <td>
                        <asp:TextBox ID="txtCustLoc" runat="server" MaxLength="30" Width="200px" />
                        <ajax:FilteredTextBoxExtender ID="ftbCustLoc" runat="server" TargetControlID="txtCustLoc"
                            FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,%/- " />
                    </td>
                    <td class="p_text">
                        Part Designation Type:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddDesignationType" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="p_text">
                        Part Number:
                    </td>
                    <td>
                        <asp:TextBox ID="txtPartNo" runat="server" MaxLength="30" Width="200px" />
                        <ajax:FilteredTextBoxExtender ID="ftbPartNo" runat="server" TargetControlID="txtPartNo"
                            FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,%/- " />
                    </td>
                    <td class="p_text">
                        Formula:
                    </td>
                    <td>
                        <asp:TextBox ID="txtFormula" runat="server" MaxLength="30" Width="200px" />
                        <ajax:FilteredTextBoxExtender ID="ftbFormula" runat="server" TargetControlID="txtFormula"
                            FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,%/- " />
                    </td>
                    <td class="p_text">
                        Record Status:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddRecStatus" runat="server">
                            <asp:ListItem Value="0">Active</asp:ListItem>
                            <asp:ListItem Value="1">Inactive</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td colspan="5">
                        <asp:Button ID="btnSearch" runat="server" Text="Submit" CausesValidation="False" />
                        <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="False" />
                        <asp:Button ID="btnExport" runat="server" Text="Export to Excel" CausesValidation="False" />
                    </td>
                </tr>
            </table>
            <ajax:CascadingDropDown ID="cddUGNLocation" runat="server" TargetControlID="ddUGNLocation"
                Category="UGNLocation" PromptText=" " LoadingText="[Loading UGN Location...]"
                ServicePath="~/WS/GeneralCDDService.asmx" ServiceMethod="GetUGNLocationByTMFac" />
            <ajax:CascadingDropDown ID="cddDepartment" runat="server" TargetControlID="ddDepartment"
                ParentControlID="ddUGNLocation" Category="Department" PromptText=" " LoadingText="[Loading Department...]"
                ServicePath="~/WS/GeneralCDDService.asmx" ServiceMethod="GetDepartment" />
            <ajax:CascadingDropDown ID="cddWorkCenter" runat="server" TargetControlID="ddWorkCenter"
                ParentControlID="ddDepartment" Category="WorkCenter" PromptText=" " LoadingText="[Loading Work Centers...]"
                ServicePath="~/WS/GeneralCDDService.asmx" ServiceMethod="GetWorkCenter" />
            <ajax:CascadingDropDown ID="CascadingDropDown1" runat="server" TargetControlID="ddOEMMfg"
                Category="OEMMfg" PromptText="Please select an OEM Manufacturer." LoadingText="[Loading OEM Manufacturer...]"
                ServicePath="~/WS/VehicleCDDService.asmx" ServiceMethod="GetOEMMfg" />
        </asp:Panel>
        <ajax:CollapsiblePanelExtender ID="FEExtender" runat="server" TargetControlID="FEContentPanel"
            ExpandControlID="FEPanel" CollapseControlID="FEPanel" Collapsed="FALSE" TextLabelID="lblFE"
            ExpandedText="Filter Part Specification table below:" CollapsedText="Filter Part Specification table below:"
            ImageControlID="imgFE" CollapsedImage="~/images/expand_blue.jpg" ExpandedImage="~/images/collapse_blue.jpg"
            SuppressPostBack="true">
        </ajax:CollapsiblePanelExtender>
        <hr />
        <asp:Label ID="lblRaiseError" runat="server" Text="" Visible="false" SkinID="MessageLabelSkin" /><br />
        <br />
        <asp:Panel ID="PSPanel" runat="server" CssClass="collapsePanelHeader" Width="100%">
            <asp:Image ID="imgPS" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                Height="12px" />&nbsp;
            <asp:Label ID="lblPS" runat="server" Text="Label" CssClass="c_textbold">Part Specification data below:</asp:Label>
        </asp:Panel>
        <asp:Panel ID="PSContentPanel" runat="server" CssClass="collapsePanel" Height="700px">
            <asp:Label ID="PagingInformation" runat="server" Text="" /><asp:DropDownList ID="PageList"
                runat="server" CssClass="c_textxsmall" AutoPostBack="true" OnSelectedIndexChanged="PageList_SelectedIndexChanged" />
            <asp:GridView ID="gvChartSpec" runat="server" AutoGenerateColumns="False" DataSourceID="odsChartSpec"
                AllowPaging="True" AllowSorting="True" CssClass="c_smalltext" PageSize="30" EmptyDataRowStyle-Font-Size="Medium"
                EmptyDataRowStyle-Font-Bold="true" EmptyDataRowStyle-ForeColor="Red" OnRowDataBound="ChartSpec_DataBound">
                <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#CCCCCC" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <EmptyDataTemplate>
                    No records found for the combination above.
                </EmptyDataTemplate>
                <EmptyDataRowStyle Font-Bold="True" Font-Size="Medium" ForeColor="Red" />
                <Columns>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" ID="hlknkEdit" ImageUrl="~/images/edit.jpg" ToolTip="Edit Record"
                                NavigateUrl='<%# "ChartSpec.aspx?pCSID=" & DataBinder.Eval (Container.DataItem,"CSID").tostring & "&pFormula=" &  DataBinder.Eval (Container.DataItem,"FormulaName").tostring%>'
                                Visible='<%# ShowEdit(DataBinder.Eval(Container, "DataItem.CSID"))%>' /></ItemTemplate>
                        <HeaderStyle Width="30px" />
                        <ItemStyle HorizontalAlign="Center" Width="30px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="FormulaName" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true"
                        HeaderText="Formula" ItemStyle-HorizontalAlign="left" SortExpression="FormulaName">
                        <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                        <ItemStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PartNoDisplay" HeaderText="Part No" SortExpression="PartNoDisplay">
                        <ItemStyle Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ddDesignationTypeName" HeaderText="Part Designation Type"
                        SortExpression="ddDesignationTypeName">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="UGNFacilityName" HeaderText="UGN Facility" SortExpression="UGNFacilityName"
                        ItemStyle-Wrap="false">
                        <ItemStyle Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ddDepartmentName" HeaderText="Department" ItemStyle-Wrap="false"
                        SortExpression="ddDepartmentName">
                        <ItemStyle Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ddWorkCenterName" HeaderText="Work Center" ItemStyle-Wrap="false"
                        SortExpression="ddWorkCenterName">
                        <ItemStyle Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CABBV" HeaderText="Customer" SortExpression="CABBV" />
                    <asp:BoundField DataField="MY" HeaderText="MY" SortExpression="MY" />
                    <asp:BoundField DataField="ddMake" HeaderText="Make" SortExpression="ddMake" />
                    <asp:BoundField DataField="ddModel" HeaderText="Model" SortExpression="ddModel" />
                    <asp:BoundField DataField="ddProgram" HeaderText="Program" SortExpression="ddProgram" />
                    <asp:BoundField DataField="PlatformName" HeaderText="Platform Name" ItemStyle-HorizontalAlign="left"
                        SortExpression="PlatformName">
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" />
                    <asp:BoundField DataField="comboUpdateInfo" HeaderText="Last Updated By" ReadOnly="True"
                        SortExpression="comboUpdateInfo" HeaderStyle-HorizontalAlign="left" HeaderStyle-Wrap="true">
                        <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsChartSpec" runat="server" OldValuesParameterFormatString="original_{0}"
                SelectMethod="GetChartSpecListing" TypeName="MfgProdBLL">
                <SelectParameters>
                    <asp:Parameter Name="CSID" Type="Int32" />
                    <asp:QueryStringParameter Name="UGNFacility" QueryStringField="sFac" Type="String" />
                    <asp:QueryStringParameter Name="OEMManufacturer" QueryStringField="sOMfg" Type="String" />
                    <asp:QueryStringParameter Name="CustLoc" QueryStringField="sCust" Type="String" />
                    <asp:QueryStringParameter Name="DesignationType" QueryStringField="sDType" Type="String" />
                    <asp:QueryStringParameter Name="PartNo" QueryStringField="sPNo" Type="String" />
                    <asp:QueryStringParameter Name="Department" QueryStringField="sDept" Type="Int32" />
                    <asp:QueryStringParameter Name="WorkCenter" QueryStringField="sWrkCntr" Type="Int32" />
                    <asp:QueryStringParameter Name="Formula" QueryStringField="sFormula" Type="String" />
                    <asp:QueryStringParameter Name="Obsolete" QueryStringField="sRecStatus" Type="Boolean" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <br />
            <br />
            <br />
        </asp:Panel>
        <ajax:CollapsiblePanelExtender ID="PSExtender" runat="server" TargetControlID="PSContentPanel"
            ExpandControlID="PSPanel" CollapseControlID="PSPanel" Collapsed="FALSE" TextLabelID="lblPS"
            ExpandedText="Part Specification data below:" CollapsedText="Part Specification data below:"
            ImageControlID="imgPS" CollapsedImage="~/images/expand_blue.jpg" ExpandedImage="~/images/collapse_blue.jpg"
            SuppressPostBack="true" ScrollContents="false">
        </ajax:CollapsiblePanelExtender>
        <br />
        <asp:ValidationSummary ID="vsEditInfo" runat="server" ShowMessageBox="True" Width="599px"
            ValidationGroup="EditInfo" Height="35px" />
        <asp:ValidationSummary ID="vsInsertInfo" runat="server" ShowMessageBox="True" Width="599px"
            ValidationGroup="InsertInfo" />
        <asp:ValidationSummary ID="vsEmptyInfo" runat="server" ShowMessageBox="True" Width="599px"
            ValidationGroup="EmptyInfo" />
    </asp:Panel>
</asp:Content>
