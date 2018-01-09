<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="CommodityMaintenance.aspx.vb" Inherits="DataMaintenance_CommodityMaintenance"
    Title="" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <table style="width: 70%; border-bottom-style: groove;" class="sampleStyleC">
            <tr>
                <td class="p_textbold" style="color: red">
                    <asp:Label ID="lblCommodityClass" runat="server" Text="Commodity Classification:" />
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblCommodityClassVal" runat="server" Text="" ForeColor="Black" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblStatus" runat="server" Text="Status:" />&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblStatusVal" runat="server" Text="" />
                </td>
                <td style="width: 400px">
                    &nbsp;
                </td>
            </tr>
        </table>
        <hr />
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.></i></asp:Label>
        <asp:Label ID="lblMessage" runat="server" Width="818px" SkinID="MessageLabelSkin" />
        <br />
        <table width="100%">
            <tr>
                <td style="height: 15px; width: 106px;" class="p_text">
                    <asp:Label ID="lblCommodity" runat="server" Text="Commodity Name:" />
                </td>
                <td>
                    <asp:TextBox ID="txtCommodityNameSearch" runat="server" Width="250px" MaxLength="30" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td style="height: 15px;">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" ValidationGroup="CommoditySearch" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="False" />
                </td>
            </tr>
            <tr>
                <td style="height: 15px;" colspan="2" align="center">
                    &nbsp;
                </td>
            </tr>
        </table>
        <hr />
        <asp:GridView ID="gvCommodityList" runat="server" AutoGenerateColumns="False" DataKeyNames="CommodityID"
            ShowFooter="True" DataSourceID="odsCommodityList" AllowPaging="True" OnRowCommand="gvCommodityList_RowCommand"
            OnRowUpdating="gvCommodityList_RowUpdating" AllowSorting="True" PageSize="30"
            SkinID="StandardGrid">
            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <EditItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True"
                            CommandName="Update" ImageUrl="~/images/save.jpg" Text="Update" AlternateText="Update"
                            ValidationGroup="EditInfo" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ibtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                            ImageUrl="~/images/cancel.jpg" Text="Cancel" AlternateText="Cancel" ValidationGroup="EditInfo" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False"
                            CommandName="Edit" ImageUrl="~/images/edit.jpg" Text="Edit" AlternateText="Edit" />&nbsp;&nbsp;&nbsp;
                    </ItemTemplate>
                    <FooterTemplate>
                        &nbsp;&nbsp;<asp:ImageButton Text="Insert" CommandName="Insert" CausesValidation="true"
                            ValidationGroup="InsertInfo" runat="server" ID="ibtnInsert" ImageUrl="~/images/save.jpg"
                            AlternateText="Insert" />
                        &nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo"
                            ImageUrl="~/images/undo-gray.jpg" Text="Undo" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CommodityID" HeaderText="Commodity ID" ReadOnly="True"
                    SortExpression="CommodityID" Visible="False" />
                <asp:TemplateField HeaderText="Commodity Classification" SortExpression="ddCommodityClassification"
                    HeaderStyle-HorizontalAlign="Left">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddCommodityClass1" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvCommodityClass1" runat="server" ControlToValidate="ddCommodityClass1"
                            Display="Dynamic" ErrorMessage="Commodity Classification is a required field."
                            ValidationGroup="EditInfo"><</asp:RequiredFieldValidator>
                        <ajax:CascadingDropDown ID="cddCommodityClass1" runat="server" TargetControlID="ddCommodityClass1"
                            Category="CommodityClassID" SelectedValue='<%# Bind("CCID") %>' PromptText="Please select a Commodity Classification."
                            LoadingText="[Loading Commodity Classification...]" ServicePath="~/WS/GeneralCDDService.asmx"
                            ServiceMethod="GetCommodityClass" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("ddCommodityClassification") %>' />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddCommodityClass" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvCommodityClass" runat="server" ControlToValidate="ddCommodityClass"
                            Display="Dynamic" ErrorMessage="Commodity Classification is a required field."
                            ValidationGroup="InsertInfo"><</asp:RequiredFieldValidator>
                        <ajax:CascadingDropDown ID="cddCommodityClass" runat="server" TargetControlID="ddCommodityClass"
                            Category="CommodityClassID" PromptText="Please select a Commodity Classification."
                            LoadingText="[Loading Commodity Classification...]" ServicePath="~/WS/GeneralCDDService.asmx"
                            ServiceMethod="GetCommodityClass" />
                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Commodity Name" SortExpression="CommodityName">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtCommodityNameEdit" runat="server" Text='<%# Bind("CommodityName") %>'
                            MaxLength="30" Width="250px" />
                        <asp:RequiredFieldValidator ID="rfvCommodityNameEdit" runat="server" ControlToValidate="txtCommodityNameEdit"
                            Display="Dynamic" ErrorMessage="Sub-Commodity Name is a required field." Font-Bold="True"
                            Font-Size="Medium" ValidationGroup="EditInfo"><</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblCommodityName" runat="server" Text='<%# Bind("ddCommodityName") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <FooterTemplate>
                        <asp:TextBox ID="txtCommodityName" runat="server" Text="" ValidationGroup="InsertInfo"
                            MaxLength="30" Width="250px" />&nbsp;
                        <asp:RequiredFieldValidator ID="rfvCommodityName" runat="server" ControlToValidate="txtCommodityName"
                            ErrorMessage="Sub-Commodity Name is a required field." ValidationGroup="InsertInfo"><</asp:RequiredFieldValidator>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="BPCS Ref" SortExpression="BPCSCommodityRef">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtBPCSRef1" runat="server" Text='<%# Bind("BPCSCommodityRef") %>'
                            MaxLength="2" Width="30px" />
                        <ajax:FilteredTextBoxExtender ID="ftbBPCSRef1" runat="server" TargetControlID="txtBPCSRef1"
                            FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblBPCSRef" runat="server" Text='<%# Bind("BPCSCommodityRef") %>' />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtBPCSRef" runat="server" MaxLength="2" Width="30px" />
                        <ajax:FilteredTextBoxExtender ID="ftbBPCSRef" runat="server" TargetControlID="txtBPCSRef"
                            FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Project Code" SortExpression="ProjectCode">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtProjectCode1" runat="server" Text='<%# Bind("ProjectCode") %>'
                            MaxLength="2" Width="30px" />
                        <asp:RequiredFieldValidator ID="rfvFunction1" runat="server" ControlToValidate="txtProjectCode1"
                            Display="Dynamic" ErrorMessage="Project Code is a required field." Font-Bold="True"
                            Font-Size="Medium" ValidationGroup="EditInfo"><</asp:RequiredFieldValidator>
                        <ajax:FilteredTextBoxExtender ID="ftbProjectCode1" runat="server" TargetControlID="txtProjectCode1"
                            FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblFunction" runat="server" Text='<%# Bind("ProjectCode") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <FooterTemplate>
                        <asp:TextBox ID="txtProjectCode" runat="server" Text="" ValidationGroup="InsertInfo"
                            MaxLength="2" Width="30px" />&nbsp;
                        <asp:RequiredFieldValidator ID="rfvFunction" runat="server" ControlToValidate="txtProjectCode"
                            ErrorMessage="Project Code is a required field." ValidationGroup="InsertInfo"><</asp:RequiredFieldValidator>
                        <ajax:FilteredTextBoxExtender ID="ftbProjectCode" runat="server" TargetControlID="txtProjectCode"
                            FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Pre-Dvp Code" SortExpression="PreDevCode">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtPreDev1" runat="server" Text='<%# Bind("PreDevCode") %>' MaxLength="2"
                            Width="30px" />
                        <ajax:FilteredTextBoxExtender ID="ftbPreDev1" runat="server" TargetControlID="txtPreDev1"
                            FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblPreDev" runat="server" Text='<%# Bind("PreDevCode") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <FooterTemplate>
                        <asp:TextBox ID="txtPreDev" runat="server" Text="" ValidationGroup="InsertInfo" MaxLength="2"
                            Width="30px" />&nbsp;
                        <ajax:FilteredTextBoxExtender ID="ftbPreDev" runat="server" TargetControlID="txtPreDev"
                            FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890" />
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
                    SortExpression="comboUpdateInfo" HeaderStyle-HorizontalAlign="left">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
        &nbsp;&nbsp;
        <asp:ValidationSummary ID="vsEditInfo" runat="server" ShowMessageBox="True" Width="599px"
            ValidationGroup="EditInfo" Height="35px" />
        <asp:ValidationSummary ID="vsInsertInfo" runat="server" ShowMessageBox="True" Width="597px"
            ValidationGroup="InsertInfo" />
        <asp:ObjectDataSource ID="odsCommodityList" runat="server" SelectMethod="GetCommodities"
            TypeName="CommoditiesBLL" UpdateMethod="UpdateCommodity" InsertMethod="AddCommodity"
            OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:Parameter Name="CommodityID" Type="Int32" DefaultValue="0" />
                <asp:QueryStringParameter Name="CommodityName" QueryStringField="sCName" Type="String" />
                <asp:Parameter Name="CommodityClass" Type="String" DefaultValue="" />
                <asp:QueryStringParameter Name="CCID" QueryStringField="sCCID" Type="Int32" DefaultValue="0" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="commodityID" Type="Int32" />
                <asp:Parameter Name="commodityName" Type="String" />
                <asp:Parameter Name="BPCSCommodityRef" Type="String" />
                <asp:Parameter Name="CCID" Type="Int32" />
                <asp:Parameter Name="PreDevCode" Type="String" />
                <asp:Parameter Name="obsolete" Type="Boolean" />
                <asp:Parameter Name="UpdatedBy" Type="String" />
                <asp:Parameter Name="original_CommodityID" Type="Int32" />
                <asp:Parameter Name="ProjectCode" Type="String" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="commodityName" Type="String" />
                <asp:Parameter Name="BPCSCommodityRef" Type="String" />
                <asp:Parameter Name="CCID" Type="Int32" />
                <asp:Parameter Name="ProjectCode" Type="String" />
                <asp:Parameter Name="PreDevCode" Type="String" />
                <asp:Parameter Name="createdBy" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
