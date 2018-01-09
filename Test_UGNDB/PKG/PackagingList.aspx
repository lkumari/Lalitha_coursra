<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="PackagingList.aspx.vb" Inherits="PKG_PackagingList" ValidateRequest="false"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="1000px" DefaultButton="btnSearch">
        <table style="width: 344px">
            <tr>
                <td align="left" class="p_smalltextbold" style="color: #990000">
                    Review existing data or press
                    <asp:Button ID="btnAdd" runat="server" Text="Add" />
                    to enter new data.
                </td>
            </tr>
        </table>
        <asp:Label ID="lblMessage" runat="server" ForeColor="#ff0000" SkinID="MessageLabelSkin"></asp:Label>
        <br />
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" />
        <br />
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <br />
        <hr />
        <table>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblDesc" runat="server" Text="Layout Description:" />
                </td>
                <td>
                    <asp:TextBox ID="txtDescription" runat="server" MaxLength="50" Width="250px" />
                    <ajax:FilteredTextBoxExtender ID="ftbDescription" runat="server" TargetControlID="txtDescription"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,-/% " />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblContainerNo" runat="server" Text="Container No:" />
                </td>
                <td>
                    <asp:TextBox ID="txtContainerNo" runat="server" MaxLength="11" Width="150px" />
                    <ajax:FilteredTextBoxExtender ID="ftbContainerNo" runat="server" TargetControlID="txtContainerNo"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,-% " />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblOEMMfg" runat="server" Text="OEM Manufacturer:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddOEMMfg" runat="server" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblUGNLocation" runat="server" Text="UGN Location:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddUGNLocation" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblMake" runat="server" Text="Make:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddMake" runat="server" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblDepartment" runat="server" Text="Department:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddDepartment" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblModel" runat="server" Text="Model:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddModel" runat="server" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblWorkCenter" runat="server" Text="WorkCenter:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddWorkCenter" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblCustomer" runat="server" Text="Customer:" />
                </td>
                <td>
                    <%-- <asp:DropDownList ID="ddCustomer" runat="server" />--%>
                    <asp:TextBox ID="txtCustomer" runat="server" MaxLength="240" Width="250px" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblPartNo" runat="server" Text="Part No:" />
                </td>
                <td>
                    <asp:TextBox ID="txtPartNo" runat="server" MaxLength="25" Width="150px" />
                    <ajax:FilteredTextBoxExtender ID="ftbPartNo" runat="server" TargetControlID="txtPartNo"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,-% " />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="3">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" />
                </td>
            </tr>
        </table>
        <ajax:CascadingDropDown ID="cddUGNLocation" runat="server" TargetControlID="ddUGNLocation"
            Category="UGNLocation" PromptText=" " LoadingText="[Loading UGN Location(s)...]"
            ServicePath="~/WS/GeneralCDDService.asmx" ServiceMethod="GetUGNLocationByTMFac" />
        <ajax:CascadingDropDown ID="cddDepartment" runat="server" TargetControlID="ddDepartment"
            ParentControlID="ddUGNLocation" Category="Department" PromptText="..." LoadingText="[Loading Department(s)...]"
            ServicePath="~/WS/GeneralCDDService.asmx" ServiceMethod="GetDepartment" />
        <ajax:CascadingDropDown ID="cddWorkCenter" runat="server" TargetControlID="ddWorkCenter"
            ParentControlID="ddDepartment" Category="WorkCenter" PromptText="..." LoadingText="[Loading Work Center(s)...]"
            ServicePath="~/WS/GeneralCDDService.asmx" ServiceMethod="GetWorkCenter" />
        <ajax:CascadingDropDown ID="cddOEMMfg" runat="server" TargetControlID="ddOEMMfg"
            Category="OEMMfg" PromptText=" " LoadingText="[Loading OEM Manufacturer(s)...]"
            ServicePath="~/WS/VehicleCDDService.asmx" ServiceMethod="GetOEMMfg" />
        <ajax:CascadingDropDown ID="cddMake" runat="server" ParentControlID="ddOEMMfg" TargetControlID="ddMake"
            Category="Make" PromptText="..." LoadingText="[Loading Make(s)...]" ServicePath="~/WS/VehicleCDDService.asmx"
            ServiceMethod="GetMakesSearch" />
        <ajax:CascadingDropDown ID="cddModel" runat="server" TargetControlID="ddModel" Category="Model"
            ParentControlID="ddMake" PromptText="..." LoadingText="[Loading Model(s)...]"
            ServicePath="~/WS/VehicleCDDService.asmx" ServiceMethod="GetModelSearch" />
        <asp:ValidationSummary ID="summaryList" runat="server" ValidationGroup="vsList" ShowMessageBox="true" />
        <hr />
        <i>Use the parameters above to filter the list below.</i>
        <br />
        <table>
            <tr>
                <td class="c_smalltext" style="font-style: italic; width: 700px">
                    <asp:Label ID="lblRecListed" runat="server" Text="Records Listed: " />
                    <asp:Label ID="lblFromRec" runat="server" ForeColor="Red" />
                    <asp:Label ID="lblTo" runat="server" Text=" to " />
                    <asp:Label ID="lblToRec" runat="server" ForeColor="Red" />
                    <asp:Label ID="lblOf" runat="server" Text=" of " />
                    <asp:Label ID="lblTotalRecords" runat="server" ForeColor="Red" />
                </td>
                <td style="width: 300px">
                    <asp:Label ID="PagingInformation" runat="server" Text="" /><asp:DropDownList ID="PageList"
                        runat="server" CssClass="c_smalltext" AutoPostBack="true" OnSelectedIndexChanged="PageList_SelectedIndexChanged" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:LinkButton ID="btnPrint" runat="server" Font-Underline="true">Print Selection</asp:LinkButton><asp:Label
                        ID="PrintResults" runat="server" EnableViewState="False" Visible="False" CssClass="c_textbold" />
                </td>
            </tr>
        </table>
        <%--OnPageIndexChanged="gvLayout_PageIndexChanged" OnPageIndexChanging="gvLayout_PageIndexChanging">--%>
        <asp:GridView ID="gvLayout" runat="server" AutoGenerateColumns="False" DataSourceID="odsLayout"
            SkinID="StandardGridWOFooter" DataKeyNames="PKGID" Width="850px" PageSize="3"
            OnRowDataBound="gvLayout_RowDataBound" OnSorting="gvLayout_Sorting">
            <Columns>
                <asp:TemplateField HeaderText="Print" ItemStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelectAll_CheckedChanged"
                            ToolTip="Select All" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="PrintSelector" runat="server" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:BoundField DataField="PKGID" HeaderText="PKGID" SortExpression="PKGID" Visible="false" />
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" ID="hlknkEdit" ImageUrl="~/images/edit.jpg" ToolTip="Edit Record"
                            NavigateUrl='<%# "Packaging.aspx?pPKGID=" & DataBinder.Eval (Container.DataItem,"PKGID").tostring%>' /></ItemTemplate>
                    <HeaderStyle Width="30px" />
                    <ItemStyle HorizontalAlign="Center" Width="30px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Layout Description" SortExpression="LayoutDesc">
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("LayoutDesc") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:BoundField DataField="ContainerNo" HeaderText="Container No" SortExpression="ContainerNo"
                    HeaderStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="OEMManufacturer" HeaderText="OEM Mfg" SortExpression="OEMManufacturer"
                    HeaderStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="ModelYr" HeaderText="Model Yr" SortExpression="ModelYr"
                    HeaderStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Make" HeaderText="Make" SortExpression="Make" HeaderStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Model" HeaderText="Model" SortExpression="Model" HeaderStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="RevisionDate" HeaderText="Revision Date" SortExpression="RevisionDate"
                    HeaderStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsLayout" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetPKGLayoutSearch" TypeName="PKGBLL">
            <SelectParameters>
                <asp:Parameter Name="PKGID" Type="String" />
                <asp:ControlParameter ControlID="txtDescription" Name="LayoutDesc" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="txtContainerNo" Name="ContainerNo" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="ddOEMMfg" Name="OEMManufacturer" PropertyName="SelectedValue"
                    Type="String" />
                <asp:ControlParameter ControlID="ddMake" Name="Make" PropertyName="SelectedValue"
                    Type="String" />
                <asp:ControlParameter ControlID="ddModel" Name="Model" PropertyName="SelectedValue"
                    Type="String" />
                <asp:ControlParameter ControlID="ddUGNLocation" Name="UGNFacility" PropertyName="SelectedValue"
                    Type="String" />
                <asp:ControlParameter ControlID="ddDepartment" DefaultValue="0" Name="DepartmentID"
                    PropertyName="SelectedValue" Type="Int32" />
                <asp:ControlParameter ControlID="ddWorkCenter" DefaultValue="0" Name="WorkCenter"
                    PropertyName="SelectedValue" Type="Int32" />
                <asp:ControlParameter ControlID="txtCustomer" DefaultValue="" Name="Customer" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="txtPartNo" DefaultValue="" Name="PartNo" PropertyName="Text"
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:Label CssClass="c_smalltext" ID="SortInformationLabel" runat="server" />
        <asp:HiddenField ID="hiddenCatIDs" runat="server" />
    </asp:Panel>
</asp:Content>
