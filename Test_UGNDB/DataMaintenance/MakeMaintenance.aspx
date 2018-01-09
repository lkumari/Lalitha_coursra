<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="MakeMaintenance.aspx.vb" Inherits="DataMaintenance_MakeMaintenance"
    MaintainScrollPositionOnPostback="True" Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <hr />
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <br />
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
        <br />
        <table width="70%">
            <tr>
                <td class="p_text" style="width: 75px">
                    <asp:Label ID="lblMake" runat="server" Text="Make:" />
                </td>
                <td style="width: 206px">
                    <asp:TextBox ID="txtMakeNameSearch" runat="server" Width="200px" MaxLength="25" />
                </td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CausesValidation="False" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="False" />
                </td>
            </tr>
        </table>
        <hr />
        <asp:GridView ID="gvMakeList" runat="server" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" DataKeyNames="MakeID" DataSourceID="odsMakeList"
            EmptyDataText="No records found." OnRowCommand="gvMakeList_RowCommand" PageSize="50"
            SkinID="StandardGrid" Width="500px">
            <Columns>
                <asp:BoundField DataField="MakeID" HeaderText="Make ID" ReadOnly="True" SortExpression="MakeID" />
                <asp:TemplateField HeaderText="Make Name" SortExpression="MakeName">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtMakeNameEdit" runat="server" MaxLength="25" Text='<%# Bind("MakeName") %>'
                            Width="200px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvMakeName" runat="server" ControlToValidate="txtMakeNameEdit"
                            Display="Dynamic" ErrorMessage="Make Name is Required for Update." Font-Bold="True"
                            Font-Size="Medium" ValidationGroup="EditMakeInfo"> &lt;
                        </asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblMakeNamePreEdit" runat="server" Text='<%# Bind("ddMakeName") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <FooterTemplate>
                        <asp:TextBox ID="txtMakeNameInsert" runat="server" MaxLength="25" Text="" ValidationGroup="InsertMakeInfo"
                            Width="200px"></asp:TextBox>
                        &nbsp;
                        <asp:RequiredFieldValidator ID="rfvMakeNameInsert" runat="server" ControlToValidate="txtMakeNameInsert"
                            ErrorMessage="Make Name is Required for Insert" ValidationGroup="InsertMakeInfo"> &lt;
                        </asp:RequiredFieldValidator>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="UGN Business **" SortExpression="UGNBusiness">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddUGNBusiness" runat="server" SelectedValue='<%# Bind("UGNBusiness") %>'>
                            <asp:ListItem Selected="True" Value="False">No</asp:ListItem>
                            <asp:ListItem Value="True">Yes</asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblUGNBiz" runat="server" Text='<%# Bind("UGNBusinessDisplay") %>' />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddUGNBusinessGV" runat="server" SelectedValue='<%# Bind("UGNBusiness") %>'>
                            <asp:ListItem Selected="True" Value="False">No</asp:ListItem>
                            <asp:ListItem Value="True">Yes</asp:ListItem>
                        </asp:DropDownList>
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
                            ValidationGroup="EditMakeInfo" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ibtnCancel" runat="server" AlternateText="Cancel" CausesValidation="False"
                            CommandName="Cancel" ImageUrl="~/images/cancel.jpg" Text="Cancel" ValidationGroup="EditMakeInfo" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnEdit" runat="server" AlternateText="Edit" CausesValidation="False"
                            CommandName="Edit" ImageUrl="~/images/edit.jpg" Text="Edit" />
                        &nbsp;&nbsp;&nbsp;
                    </ItemTemplate>
                    <FooterTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnInsert" runat="server" AlternateText="Insert"
                            CausesValidation="true" CommandName="Insert" ImageUrl="~/images/save.jpg" Text="Insert"
                            ValidationGroup="InsertMakeInfo" />
                        &nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ibtnUndo" runat="server" AlternateText="Undo"
                            CommandName="Undo" ImageUrl="~/images/undo-gray.jpg" Text="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        &nbsp;&nbsp;
        <asp:ValidationSummary ID="vsEditMakeInfo" runat="server" ShowMessageBox="True" Width="599px"
            ValidationGroup="EditMakeInfo" Height="35px" />
        <asp:ValidationSummary ID="vsInsertMakeInfo" runat="server" ShowMessageBox="True"
            Width="599px" ValidationGroup="InsertMakeInfo" />
        <br />
        <asp:ValidationSummary ID="vsEmptyMakeInfo" runat="server" ShowMessageBox="True"
            Width="599px" ValidationGroup="EmptyMakeInfo" />
        <asp:ObjectDataSource ID="odsMakeList" runat="server" SelectMethod="GetMakes" TypeName="MakesBLL"
            UpdateMethod="UpdateMake" InsertMethod="InsertMake" OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:QueryStringParameter Name="MakeName" QueryStringField="MakeName" Type="String" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="MakeName" Type="String" />
                <asp:Parameter Name="UGNBusiness" Type="Boolean" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
                <asp:Parameter Name="UpdatedBy" Type="String" />
                <asp:Parameter Name="original_MakeID" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="MakeName" Type="String" />
                <asp:Parameter Name="UGNBusiness" Type="Boolean" />
                <asp:Parameter Name="createdBy" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
