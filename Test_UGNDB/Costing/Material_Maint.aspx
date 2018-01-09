<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Material_Maint.aspx.vb" Inherits="Material_Maint" MaintainScrollPositionOnPostback="true"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server">
        <asp:Label runat="server" ID="lblMessage" SkinID="MessageLabelSkin"></asp:Label>
        <asp:ValidationSummary ID="vsMaterial" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgMaterial" />
        <table>
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblMaterialIDLabel" Text="Material ID:" Visible="false"></asp:Label>
                </td>
                <td class="c_textbold">
                    <asp:Label runat="server" ID="lblMaterialIDValue" Visible="false"></asp:Label>
                </td>
                <td class="p_text" align="left" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblObsoleteLabel" Text="Obsolete:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:CheckBox runat="server" ID="cbObsoleteValue" Visible="false" />
                </td>
            </tr>
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label ID="lblMaterialNameMarker" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="*" Visible="false" />
                    <asp:Label runat="server" ID="lblMaterialNameLabel" Text="Material Name:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtMaterialNameValue" runat="server" MaxLength="50" Width="200px"
                        Visible="false"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvMaterialName" runat="server" ControlToValidate="txtMaterialNameValue"
                        ErrorMessage="The name is required." Font-Bold="True" ValidationGroup="vgMaterial"
                        Text="<" SetFocusOnError="true" />
                </td>
            </tr>
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblMaterialDescLabel" Text="Material Desc:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtMaterialDescValue" runat="server" MaxLength="50" Width="200px"
                        Visible="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblPartNoLabel" Text="Internal Part No (RM):" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPartNoValue" runat="server" MaxLength="40" Width="200px"
                        Visible="false"></asp:TextBox>
                    <asp:ImageButton ID="iBtnPartNo" runat="server" ImageUrl="~/images/Search.gif"
                        Visible="false" ToolTip="Click here to search for a Part No." />
                </td>
               <%-- <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblPartRevisionLabel" Text="Part Revision:"
                        Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPartRevisionValue" runat="server" MaxLength="2" Width="40px"
                        Visible="false"></asp:TextBox>
                </td>--%>
            </tr>
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblPartDescLabel" Text="Internal Part Description:"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:Label runat="server" ID="lblPartDescValue" CssClass="c_textbold"></asp:Label>
                </td>
            </tr>
           <%-- <tr> 
              <td class="p_text">
                    <asp:Label runat="server" ID="Label1" Text="UGN Facility Code:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddUGNFacilityCodeValue" runat="server" Visible="false">
                    </asp:DropDownList>
                </td>
              
            </tr>--%>
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblDrawingNoLabel" Text="Drawing No.:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDrawingNoValue" runat="server" MaxLength="17" Width="200px" Visible="false"></asp:TextBox>
                    <asp:ImageButton ID="iBtnGetDrawingInfo" runat="server" ImageUrl="~/images/Search.gif"
                        Visible="false" ToolTip="Click here to search for a DMS Drawing." />
                    &nbsp;
                    <asp:HyperLink runat="server" ID="hlnkDrawingNo" Visible="false" Font-Underline="true"
                        ToolTip="Click here to view the DMS Drawing." Text="View DMS Drawing" Target="_blank"></asp:HyperLink>
                </td>
            </tr>
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblIsCoatingLabel" Text="Coating:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:CheckBox runat="server" ID="cbIsCoatingValue" />
                </td>
            </tr>
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblIsPackagingLabel" Text="Packaging:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:CheckBox runat="server" ID="cbIsPackagingValue" Visible="false" />
                </td>
            </tr>
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblUGNDBVendorLabel" Text="Vendor:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddUGNDBVendorValue" runat="server" Visible="false">
                    </asp:DropDownList>
                    &nbsp; <a href="../DataMaintenance/UGNDBVendorMaintenance.aspx" target="_blank">View
                        Vendor List</a>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblPurchasedGoodLabel" Text="Purchased Good:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddPurchasedGoodValue" runat="server" Visible="false">
                    </asp:DropDownList>
                </td>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblOldMaterialGroupLabel" Text="Old Material Group:"
                        Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblOldMaterialGroupValue" Visible="false" CssClass="c_textbold"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblUnitofMeasureLabel" Text="Unit of Measure:" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddUnitofMeasureValue" runat="server" Visible="false">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label runat="server" ID="lblUGNFacilityCodeLabel" Text="UGN Facility:" ></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddUGNFacilityCodeValue" runat="server" >
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblQuoteCostLabel" Text="Quote Cost:" Visible="false"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtQuoteCostValue" runat="server" MaxLength="10" Width="100px" Visible="false"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvtxtQuoteCost" Operator="DataTypeCheck"
                        ValidationGroup="vgMaterial" Type="double" Text="<" ControlToValidate="txtQuoteCostValue"
                        ErrorMessage="Quote Cost must be a number." SetFocusOnError="True" />
                    <asp:Label runat="server" ID="lblQuoteCostDateLabel" Text="Last Changed:" Visible="false"></asp:Label>
                    &nbsp;
                    <asp:Label runat="server" ID="lblQuoteCostDateValue" Visible="false" CssClass="c_textbold"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblFreightCostLabel" Text="Freight Cost:" Visible="false"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtFreightCostValue" runat="server" MaxLength="10" Width="100px"
                        Visible="false"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cvFreightCost" Operator="DataTypeCheck"
                        ValidationGroup="vgMaterial" Type="double" Text="<" ControlToValidate="txtFreightCostValue"
                        ErrorMessage="Freight Cost must be a number." SetFocusOnError="True" />
                    <asp:Label runat="server" ID="lblFreightCostDateLabel" Text="Last Changed:" Visible="false"></asp:Label>
                    &nbsp;
                    <asp:Label runat="server" ID="lblFreightCostDateValue" Visible="false" CssClass="c_textbold"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblStandardCostLabel" Text="Standard Cost:"
                        Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblStandardCostValue" runat="server" Visible="false" CssClass="c_textbold"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:Label runat="server" ID="lblStandardCostDateLabel" Text="Last Changed:"
                        Visible="false"></asp:Label>
                    &nbsp;
                    <asp:Label runat="server" ID="lblStandardCostDateValue" Visible="false" CssClass="c_textbold"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap; height: 27px;">
                    <asp:Label runat="server" ID="lblPurchasedCostLabel" Text="Purchased Cost:"
                        Visible="false"></asp:Label>
                </td>
                <td style="height: 27px">
                    <asp:Label ID="lblPurchasedCostValue" runat="server" Visible="false" CssClass="c_textbold"></asp:Label>
                </td>
                <td colspan="2" style="height: 27px">
                    <asp:Label runat="server" ID="lblPurchasedCostDateLabel" Text="Last Changed:"
                        Visible="false"></asp:Label>
                    &nbsp;
                    <asp:Label runat="server" ID="lblPurchasedCostDateValue" Visible="false" CssClass="c_textbold"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center">
                    <asp:Button runat="server" ID="btnSave" Text="Save" CausesValidation="true" ValidationGroup="vgMaterial"
                        Visible="false" />
                    <asp:Button runat="server" ID="btnCopy" Text="Copy" CausesValidation="true" ValidationGroup="vgMaterial"
                        Visible="false" />
                    <asp:Button runat="server" ID="btnUpdateDrawing" Text="Update DMS Drawing" CausesValidation="true"
                        ValidationGroup="vgMaterial" Visible="false" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
