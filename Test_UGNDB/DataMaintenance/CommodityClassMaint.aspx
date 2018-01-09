<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="CommodityClassMaint.aspx.vb" Inherits="DataMaintenance_CommodityClassMaint"
    Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <hr />
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <br />
        <asp:Label ID="lblMessage" runat="server" Width="818px" SkinID="MessageLabelSkin"></asp:Label>
        <table>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblCommodityClass" runat="server" Text="Commodity Classification:" />
                </td>
                <td>
                    <asp:TextBox ID="txtCommodityClassification" runat="server" Width="250px" MaxLength="30" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
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
        <asp:GridView ID="gvCommodityClass" runat="server" AutoGenerateColumns="False" DataKeyNames="CCID"
            ShowFooter="True" DataSourceID="odsCommodityClass" AllowPaging="True" Width="600px"
            OnRowCommand="gvCommodityClass_RowCommand" AllowSorting="True" PageSize="30"
            SkinID="StandardGrid">
            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <EditItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True"
                            CommandName="Update" ImageUrl="~/images/save.jpg" Text="Update" AlternateText="Update"
                            ValidationGroup="EditCommodityInfo" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ibtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                            ImageUrl="~/images/cancel.jpg" Text="Cancel" AlternateText="Cancel" ValidationGroup="EditCommodityInfo" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False"
                            CommandName="Edit" ImageUrl="~/images/edit.jpg" Text="Edit" AlternateText="Edit" />&nbsp;&nbsp;&nbsp;
                    </ItemTemplate>
                    <FooterTemplate>
                        &nbsp;&nbsp;<asp:ImageButton Text="Insert" CommandName="Insert" CausesValidation="true"
                            ValidationGroup="InsertCommodityInfo" runat="server" ID="ibtnInsert" ImageUrl="~/images/save.jpg"
                            AlternateText="Insert" />
                        &nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo"
                            ImageUrl="~/images/undo-gray.jpg" Text="Undo" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Commodity(ies)">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" ID="hlnkVolume" ImageUrl="~/images/commodity.jpg" ToolTip="Preview/Insert/Edit Commodity(ies)"
                            NavigateUrl='<%# "CommodityMaintenance.aspx?sCCID=" & DataBinder.Eval (Container.DataItem,"CCID").tostring & "&sClass=" & ViewState("sClass")%>' />
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="center" />
                </asp:TemplateField>
                <asp:BoundField DataField="CCID" HeaderText="Commodity ID" ReadOnly="True" SortExpression="CCID"
                    Visible="False" />
                <asp:TemplateField HeaderText="Commodity Classification" SortExpression="Commodity_Classification">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtCommodityClassEdit" runat="server" Text='<%# Bind("Commodity_Classification") %>'
                            MaxLength="30" Width="250px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvCommodityClassEdit" runat="server" ControlToValidate="txtCommodityClassEdit"
                            Display="Dynamic" ErrorMessage="Commodity Classification is a required field."
                            Font-Bold="True" Font-Size="Medium" ValidationGroup="EditCommodityInfo"><</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblCommodityClass" runat="server" Text='<%# Bind("ddCommodityClassification") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <FooterTemplate>
                        <asp:TextBox ID="txtCommodityClass" runat="server" Text="" ValidationGroup="InsertCommodityInfo"
                            MaxLength="30" Width="250px"></asp:TextBox>&nbsp;
                        <asp:RequiredFieldValidator ID="rfvCommodityClass" runat="server" ControlToValidate="txtCommodityClass"
                            ErrorMessage="Commodity Classification is a required field." ValidationGroup="InsertCommodityInfo"><</asp:RequiredFieldValidator>
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
        <asp:ObjectDataSource ID="odsCommodityClass" runat="server" InsertMethod="InsertCommodityClass"
            OldValuesParameterFormatString="original_{0}" SelectMethod="GetCommodityClass"
            TypeName="CommoditiesBLL" UpdateMethod="UpdateCommodityClass">
            <UpdateParameters>
                <asp:Parameter Name="CommodityClass" Type="String" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
                <asp:Parameter Name="UpdatedBy" Type="String" />
                <asp:Parameter Name="original_CCID" Type="Int32" />
                <asp:Parameter Name="Commodity_Classification" Type="String" />
            </UpdateParameters>
            <SelectParameters>
                <asp:Parameter Name="CCID" Type="Int32" DefaultValue="0" />
                <asp:QueryStringParameter Name="CommodityClass" QueryStringField="sClass" Type="String" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="CommodityClass" Type="String" />
                <asp:Parameter Name="createdBy" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
        &nbsp;&nbsp;
        <asp:ValidationSummary ID="vsEditCommodityInfo" runat="server" ShowMessageBox="True"
            Width="599px" ValidationGroup="EditCommodityInfo" Height="35px" />
        <asp:ValidationSummary ID="vsInsertCommodityInfo" runat="server" ShowMessageBox="True"
            Width="597px" ValidationGroup="InsertCommodityInfo" />
    </asp:Panel>
</asp:Content>
