<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    MaintainScrollPositionOnPostback="true" CodeFile="UGNDBVendorMaintenance.aspx.vb"
    Inherits="UGNDBVendorMaintenance" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <table>
            <tr>
                <td class="p_smalltextbold" style="color: #990000" colspan="3" align="left">
                    <asp:Label runat="server" ID="lblReview1" Text="Review existing data or press" />
                    <asp:Button ID="btnAdd" runat="server" Text="Add" Enabled="false" />
                    <asp:Label runat="server" ID="lblReview2" Text="to enter a new vendor." />
                </td>
            </tr>
        </table>
        <br />
        <asp:Label runat="server" ID="lblMessage" Text="" />
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <br />
        <br />
        <asp:ValidationSummary ID="vsVendor" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgSearchVendor" />
        <table>
            <tr>
                <td class="p_text">
                    <asp:Label runat="server" ID="lblSearchVendorLabel" Text="Supplier:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddSearchVendorValue" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label runat="server" ID="lblSearchSupplierNoLabel" Text="Supplier Number:" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchSupplierNoValue" Width="400px" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label runat="server" ID="lblSearchSupplierNameLabel" Text="Supplier Name:" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchSupplierNameValue"  Width="400px" />
                    <ajax:AutoCompleteExtender runat="server" ID="acSupplierName" TargetControlID="txtSearchSupplierNameValue"
                        ServicePath="~/AutoComplete.asmx" ServiceMethod="GetVendorList" MinimumPrefixLength="2"
                        CompletionInterval="1000" EnableCaching="true" CompletionSetCount="12" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button runat="server" ID="btnSearch" CausesValidation="true" Text="Search" ValidationGroup="vgSearchVendor" />
                    &nbsp;
                    <asp:Button runat="server" ID="btnReset" CausesValidation="false" Text="Reset" ValidationGroup="vgSearchVendor" />
                </td>
            </tr>
        </table>
        <hr />
        <asp:ValidationSummary ID="vsEditUGNDBVendor" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgEditUGNDBVendor" />
        <asp:ValidationSummary ID="vsFooterUGNDBVendor" runat="server" DisplayMode="List"
            ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgFooterUGNDBVendor" />
        <asp:GridView runat="server" ID="gvUGNDBVendor" AllowPaging="True" AllowSorting="True"
            DataKeyNames="UGNDBVendorID" DataSourceID="odsUGNDBVendor" PageSize="30" SkinID="StandardGridWOFooter"
            Width="1000px">
            <Columns>
                <asp:BoundField DataField="UGNDBVendorID" HeaderText="UGN Supplier No" ReadOnly="true" />
                <asp:BoundField DataField="SupplierNo" HeaderText="Supplier No" ReadOnly="true" />
                <asp:TemplateField HeaderText="Supplier Name">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddEditBPCSVendor" runat="server" AppendDataBoundItems="True"
                            DataSource='<%# commonFunctions.GetVendor(0, "", "", "", "", "", "", "","") %>'
                            DataTextField="ddVNDNAMcombo" DataValueField="Vendor" SelectedValue='<%# Bind("SupplierNo") %>'>
                            <asp:ListItem Selected="False" Text="" Value="0" />
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewBPCSVendor" runat="server" Text='<%# Bind("SupplierName") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="UGNDB Supplier Name" SortExpression="SupplierName">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditUGNDBSupplierName" runat="server" MaxLength="240" Text='<%# Bind("UGNDBVendorName") %>' Width="300px" />
                        <asp:RequiredFieldValidator ID="rfvEditVendor" runat="server" ControlToValidate="txtEditUGNDBSupplierName"
                            ErrorMessage="The name is required." Font-Bold="True" SetFocusOnError="true"
                            Text="&lt;" ValidationGroup="vgEditUGNDBVendor" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewUGNDBSupplierName" runat="server" Text='<%# Bind("ddSupplierName") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Obsolete" SortExpression="Obsolete">
                    <EditItemTemplate>
                        <asp:CheckBox ID="cbEditObsolete" runat="server" Checked='<%# Bind("Obsolete") %>' />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="cbViewObsolete" runat="server" Checked='<%# Bind("Obsolete") %>'
                            Enabled="false" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <EditItemTemplate>
                        <asp:ImageButton ID="iBtnUGNDBVendorUpdate" runat="server" AlternateText="Update"
                            CausesValidation="True" CommandName="Update" ImageUrl="~/images/save.jpg" ValidationGroup="vgEditUGNDBVendor" />
                        <asp:ImageButton ID="iBtnUGNDBVendorCancel" runat="server" AlternateText="Cancel"
                            CausesValidation="False" CommandName="Cancel" ImageUrl="~/images/cancel.jpg" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="iBtnUGNDBVendorEdit" runat="server" AlternateText="Edit" CausesValidation="False"
                            CommandName="Edit" ImageUrl="~/images/edit.jpg" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsUGNDBVendor" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetUGNDBVendor" TypeName="UGNDBVendorBLL" UpdateMethod="UpdateUGNDBVendor">
            <UpdateParameters>
                <asp:Parameter Name="original_UGNDBVendorID" Type="Int32" />
                <asp:Parameter Name="UGNDBVendorName" Type="String" />
                <asp:Parameter Name="SupplierNo" Type="Int32" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
                <asp:Parameter Name="SupplierName" Type="String" />
            </UpdateParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="UGNDBVendorID" QueryStringField="UGNDBVendorID" Type="Int32" />
                <asp:QueryStringParameter Name="SupplierNo" QueryStringField="SupplierNo" 
                    Type="Int32" />
                <asp:QueryStringParameter Name="SupplierName" QueryStringField="SupplierName" Type="String" />
                <asp:Parameter DefaultValue="False" Name="isActiveBPCS" Type="Boolean" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
