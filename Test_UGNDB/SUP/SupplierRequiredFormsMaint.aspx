<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="SupplierRequiredFormsMaint.aspx.vb" Inherits="Supplier_Required_Forms_Maint"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
        Width="568px" Visible="False"></asp:Label><br />
    <asp:Panel ID="localPanel" runat="server" Width="1000px">
        <hr />
        <table>
            <tr>
                <td class="p_text">
                    Form Name:
                </td>
                <td>
                    <asp:TextBox ID="txtFormName" runat="server" MaxLength="200" Width="300px"></asp:TextBox>
                </td>
                <td class="p_text">
                    Vendor Type:</td>
                <td>
                    <asp:DropDownList ID="ddVendorType" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                </td>
                <td colspan="3">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" /></td>
            </tr>
        </table>
        <hr />
        <br />
        <asp:GridView ID="gvSupplierRequiredForms" runat="server" AutoGenerateColumns="False"
            DataSourceID="odsSupplierRequiredForms" AllowSorting="True" AllowPaging="True"
            Width="1000px" OnRowCommand="gvSupplierRequiredForms_RowCommand" DataKeyNames="SRFID"
            OnDataBound="gvSupplierRequiredForms_DataBound" PageSize="20" ShowFooter="True">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" Wrap="False" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Wrap="False" BorderColor="White" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" Wrap="False" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" Wrap="False" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#E2DED6" Wrap="False" BorderStyle="None" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" Wrap="False" />
            <EmptyDataRowStyle BackColor="White" Wrap="False" />
            <Columns>
                <asp:BoundField DataField="SRFID" HeaderText="SRFID" SortExpression="SRFID" ReadOnly="True"
                    Visible="False" />
                <asp:TemplateField HeaderText="Form Name" SortExpression="FormName" HeaderStyle-HorizontalAlign="left"
                    ItemStyle-Width="300px">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtForm" runat="server" MaxLength="200" ValidationGroup="InsertForm"
                            Text='<%# Bind("FormName") %>' Width="300px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvForm" runat="server" ControlToValidate="txtForm"
                            Display="Dynamic" ErrorMessage="Form Name is a required field." ValidationGroup="EditFormInfo"><</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblForm" runat="server" Text='<%# Bind("FormName") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtForm" runat="server" MaxLength="200" Width="300px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvForm" runat="server" ControlToValidate="txtForm"
                            Display="Dynamic" ErrorMessage="Form Name is a required field." ValidationGroup="InsertFormInfo"><</asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <HeaderStyle Width="300px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Vendor Type" SortExpression="VendorType" HeaderStyle-HorizontalAlign="left"
                    ItemStyle-Width="300px">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddVType" runat="server" AppendDataBoundItems="true" DataSource='<%# commonFunctions.GetVendorType(0)%>'
                            DataTextField="ddVTYPE" DataValueField="VTYPE" SelectedValue='<%# Bind("VendorType") %>'
                            ValidationGroup="EditFormInfo">
                            <asp:ListItem Selected="True" />
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvVType" runat="server" ControlToValidate="ddVType"
                            ErrorMessage="Vendor Type is Required for Insert" ValidationGroup="InsertFormInfo"><</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("ddVTYPE") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddVTypeInsert" runat="server" AppendDataBoundItems="true" DataSource='<%# commonFunctions.GetVendorType(0)%>'
                            DataTextField="ddVTYPE" DataValueField="VTYPE" SelectedValue='<%# Bind("VTYPE") %>'
                            ValidationGroup="InsertFormInfo">
                            <asp:ListItem Selected="True" />
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvVTypeInsert" runat="server" ControlToValidate="ddVTypeInsert"
                            ErrorMessage="Vendor Type is Required for Insert" ValidationGroup="InsertFormInfo"><</asp:RequiredFieldValidator>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Required Form" SortExpression="RequiredForm" FooterStyle-HorizontalAlign="center">
                    <EditItemTemplate>
                        <asp:CheckBox ID="chkReqFormEdit" runat="server" Checked='<%# Bind("RequiredForm") %>' />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkReqFormPreEdit" runat="server" Checked='<%# Bind("RequiredForm") %>'
                            Enabled="false" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                    <FooterTemplate>
                        <asp:CheckBox ID="chkReqFormInsert" runat="server" Checked='<%# Bind("RequiredForm") %>' />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Obsolete" SortExpression="Obsolete">
                    <EditItemTemplate>
                        <asp:CheckBox ID="chkObsoleteEdit" runat="server" Checked='<%# Bind("Obsolete") %>' />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkObsoletePreEdit" runat="server" Checked='<%# Bind("Obsolete") %>'
                            Enabled="false" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:BoundField DataField="comboUpdateInfo" HeaderText="Last Update" ReadOnly="True"
                    SortExpression="comboUpdateInfo" />
                <asp:TemplateField ShowHeader="False">
                    <ItemStyle HorizontalAlign="Left" />
                    <EditItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True"
                            CommandName="Update" ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="EditFormInfo" />&nbsp;&nbsp;&nbsp;<asp:ImageButton
                                ID="ibtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False"
                            CommandName="Edit" ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                    </ItemTemplate>
                    <FooterTemplate>
                        &nbsp;&nbsp;<asp:ImageButton CommandName="Insert" CausesValidation="true" runat="server"
                            ID="ibtnInsert" ImageUrl="~/images/save.jpg" AlternateText="Insert" ValidationGroup="InsertFormInfo" />
                        &nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo"
                            ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsSupplierRequiredForms" runat="server" InsertMethod="InsertSupplierRequiredForms"
            OldValuesParameterFormatString="original_{0}" SelectMethod="GetSupplierRequiredForms"
            TypeName="SupplierRequiredFormsBLL" UpdateMethod="UpdateSupplierRequiredForms">
            <UpdateParameters>
                <asp:Parameter Name="SRFID" Type="Int32" />
                <asp:Parameter Name="FormName" Type="String" />
                <asp:Parameter Name="VendorType" Type="String" />
                <asp:Parameter Name="RequiredForm" Type="Boolean" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
                <asp:Parameter Name="original_FormName" Type="String" />
                <asp:Parameter Name="original_VendorType" Type="String" />
                <asp:Parameter Name="original_SRFID" Type="Int32" />
            </UpdateParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="FormName" QueryStringField="pFN" Type="String" />
                <asp:QueryStringParameter Name="VendorType" QueryStringField="pVT" Type="String" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="FormName" Type="String" />
                <asp:Parameter Name="VendorType" Type="String" />
                <asp:Parameter Name="RequiredForm" Type="Boolean" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
            </InsertParameters>
        </asp:ObjectDataSource>
        <asp:ValidationSummary ID="EditFormInfo" runat="server" ShowMessageBox="True" ValidationGroup="EditFormInfo" />
        &nbsp;<asp:ValidationSummary ID="InsertFormInfo" runat="server" ShowMessageBox="True"
            ValidationGroup="InsertFormInfo" />
    </asp:Panel>
</asp:Content>
