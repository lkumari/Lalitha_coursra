<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="ModelMaintenance.aspx.vb" Inherits="DataMaintenance_ModelMaintenance"
    MaintainScrollPositionOnPostback="True" Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server">
        <hr />
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <br />
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
        <br />
        <table>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblModel" runat="server" Text="Model:" />
                </td>
                <td>
                    <asp:TextBox ID="txtModelNameSearch" runat="server" Width="200px" MaxLength="25" />
                    <ajax:FilteredTextBoxExtender ID="ftbModelSearch" runat="server" TargetControlID="txtModelNameSearch"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,%/- " />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblMake" runat="server" Text="Make:" />
                </td>
                <td>
                    <asp:TextBox ID="txtMakeSearch" runat="server" Width="200px" MaxLength="25" />
                    <ajax:FilteredTextBoxExtender ID="ftbMakeSearch" runat="server" TargetControlID="txtMakeSearch"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,%/- " />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="3">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CausesValidation="False" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="False" />
                </td>
            </tr>
        </table>
        <hr />
        <asp:GridView ID="gvModelList" runat="server" AutoGenerateColumns="False" DataKeyNames="ModelID"
            DataSourceID="odsModelList" AllowPaging="True" Width="600px" OnRowCommand="gvModelList_RowCommand"
            AllowSorting="True" EmptyDataText="No records found." PageSize="50" SkinID="StandardGrid">
            <Columns>
                <asp:BoundField DataField="ModelID" HeaderText="Model ID" ReadOnly="True" SortExpression="ModelID" />
                <asp:TemplateField HeaderText="Make **" SortExpression="Make">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddMake1" runat="server" DataSource='<%# commonFunctions.GetMake("") %>'
                            DataValueField="MakeName" DataTextField="ddMakeName" SelectedValue='<%# Bind("Make") %>'
                            AppendDataBoundItems="True">
                            <asp:ListItem Selected="True" Value="" Text="N/A">
                            </asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvMake11" runat="server" ControlToValidate="ddMake1"
                            Display="Dynamic" ErrorMessage="Make is a required field." ValidationGroup="EditInfo"><</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblMake" runat="server" Text='<%# Bind("ddMakeName") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <FooterTemplate>
                        <asp:DropDownList ID="ddMake" runat="server" DataSource='<%# commonFunctions.GetMake("") %>'
                            DataValueField="MakeName" DataTextField="ddMakeName" SelectedValue='<%# Bind("Make") %>'
                            AppendDataBoundItems="True">
                            <asp:ListItem Selected="True" Value="" Text="N/A">
                            </asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvMake11" runat="server" ControlToValidate="ddMake"
                            Display="Dynamic" ErrorMessage="Make is a required field." ValidationGroup="InsertInfo"><</asp:RequiredFieldValidator>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Model Name" SortExpression="ModelName">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtModelNameEdit" runat="server" MaxLength="60" Text='<%# Bind("ModelName") %>'
                            Width="250px" />
                        <ajax:FilteredTextBoxExtender ID="ftbModelEdit" runat="server" TargetControlID="txtModelNameEdit"
                            FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,&/- " />
                        <asp:RequiredFieldValidator ID="rfvModelName" runat="server" ControlToValidate="txtModelNameEdit"
                            Display="Dynamic" ErrorMessage="Model Name is Required for Update." Font-Bold="True"
                            Font-Size="Medium" ValidationGroup="EditInfo">&lt;</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblModelNamePreEdit" runat="server" Text='<%# Bind("ModelName") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <FooterTemplate>
                        <asp:TextBox ID="txtModelNameInsert" runat="server" MaxLength="60" Text="" ValidationGroup="InsertInfo"
                            Width="250px" />
                        <ajax:FilteredTextBoxExtender ID="ftbModelInsert" runat="server" TargetControlID="txtModelNameInsert"
                            FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,&/- " />
                        &nbsp;
                        <asp:RequiredFieldValidator ID="rfvModelNameInsert" runat="server" ControlToValidate="txtModelNameInsert"
                            ErrorMessage="Model Name is Required for Insert" ValidationGroup="InsertInfo"> &lt;
                        </asp:RequiredFieldValidator>
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
                <asp:BoundField DataField="comboUpdateInfo" HeaderStyle-HorizontalAlign="left" HeaderText="Last Update"
                    ReadOnly="True" SortExpression="comboUpdateInfo">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:TemplateField ShowHeader="False">
                    <EditItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnUpdate" runat="server" AlternateText="Update"
                            CausesValidation="True" CommandName="Update" ImageUrl="~/images/save.jpg" Text="Update"
                            ValidationGroup="EditInfo" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ibtnCancel" runat="server" AlternateText="Cancel" CausesValidation="False"
                            CommandName="Cancel" ImageUrl="~/images/cancel.jpg" Text="Cancel" ValidationGroup="EditInfo" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnEdit" runat="server" AlternateText="Edit" CausesValidation="False"
                            CommandName="Edit" ImageUrl="~/images/edit.jpg" Text="Edit" />
                        &nbsp;&nbsp;&nbsp;
                    </ItemTemplate>
                    <FooterTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnInsert" runat="server" AlternateText="Insert"
                            CausesValidation="true" CommandName="Insert" ImageUrl="~/images/save.jpg" Text="Insert"
                            ValidationGroup="InsertInfo" />
                        &nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ibtnUndo" runat="server" AlternateText="Undo"
                            CommandName="Undo" ImageUrl="~/images/undo-gray.jpg" Text="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        &nbsp;&nbsp;
        <asp:ValidationSummary ID="vsEditInfo" runat="server" ShowMessageBox="True" Width="599px"
            ValidationGroup="EditInfo" Height="35px" />
        <asp:ValidationSummary ID="vsInsertInfo" runat="server" ShowMessageBox="True" Width="599px"
            ValidationGroup="InsertInfo" />
        <br />
        <asp:ValidationSummary ID="vsEmptyModelInfo" runat="server" ShowMessageBox="True"
            Width="599px" ValidationGroup="EmptyModelInfo" />
        <asp:ObjectDataSource ID="odsModelList" runat="server" SelectMethod="GetModels" TypeName="ModelsBLL"
            UpdateMethod="UpdateModel" InsertMethod="InsertModel" OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:QueryStringParameter Name="ModelName" QueryStringField="sMName" Type="String" />
                <asp:QueryStringParameter Name="Make" QueryStringField="sMake" Type="String" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="ModelName" Type="String" />
                <asp:Parameter Name="Make" Type="String" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
                <asp:Parameter Name="UpdatedBy" Type="String" />
                <asp:Parameter Name="original_ModelID" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="ModelName" Type="String" />
                <asp:Parameter Name="Make" Type="String" />
                <asp:Parameter Name="createdBy" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
