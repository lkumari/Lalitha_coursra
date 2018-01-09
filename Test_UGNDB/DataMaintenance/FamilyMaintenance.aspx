<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="FamilyMaintenance.aspx.vb" Inherits="DataMaintenance_FamilyMaintenance"
    Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <hr />
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <br />
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"/>
        <br />
        <table>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblFamilyName" runat="server" Text="Family Name:" />
                </td>
                <td style="width: 217px">
                    <asp:TextBox ID="txtFamilyNameSearch" runat="server" Width="200px" />
                </td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CausesValidation="False" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="False" />
                </td>
            </tr>
        </table>
        <hr />
        <asp:GridView ID="gvFamilyList" runat="server" AutoGenerateColumns="False" DataKeyNames="FamilyID"
            DataSourceID="odsFamilyList" AllowPaging="True" AllowSorting="True" PageSize="30"
            SkinID="StandardGridWOFooter" Width="600px">
            <Columns>
                <asp:BoundField DataField="FamilyID" HeaderText="Family ID" ReadOnly="True" SortExpression="FamilyID">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="FamilyName" HeaderText="Family Name" ReadOnly="True" SortExpression="FamilyName">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="FormulaCode" HeaderText="Formula Code" ReadOnly="True"
                    SortExpression="FormulaCode">
                    <ItemStyle HorizontalAlign="center" />
                </asp:BoundField>
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
                    SortExpression="comboUpdateInfo" HeaderStyle-HorizontalAlign="left" >
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:TemplateField ShowHeader="False">
                    <EditItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True"
                            CommandName="Update" ImageUrl="~/images/save.jpg" Text="Update" AlternateText="Update"
                            ValidationGroup="EditFamilyInfo" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ibtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                            ImageUrl="~/images/cancel.jpg" Text="Cancel" AlternateText="Cancel" ValidationGroup="EditFamilyInfo" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False"
                            CommandName="Edit" ImageUrl="~/images/edit.jpg" Text="Edit" AlternateText="Edit" />&nbsp;&nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        &nbsp;&nbsp;
        <asp:ValidationSummary ID="vsEditFamilyInfo" runat="server" ShowMessageBox="True"
            Width="599px" ValidationGroup="EditFamilyInfo" Height="35px" />
        <asp:ValidationSummary ID="vsEmptyFamilyInfo" runat="server" ShowMessageBox="True"
            Width="599px" ValidationGroup="EmptyFamilyInfo" />
        <asp:ObjectDataSource ID="odsFamilyList" runat="server" SelectMethod="GetFamilies"
            TypeName="FamiliesBLL" UpdateMethod="UpdateFamily" OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:Parameter DefaultValue="0" Name="FamilyID" Type="Int32" />
                <asp:Parameter DefaultValue="0" Name="OldFamilyID" Type="Int32" />
                <asp:QueryStringParameter Name="FamilyName" QueryStringField="FamilyName" Type="String"
                    DefaultValue="" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="FamilyName" Type="String" />
                <asp:Parameter Name="FormulaCode" Type="String" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
                <asp:Parameter Name="UpdatedBy" Type="String" />
                <asp:Parameter Name="original_FamilyID" Type="Int32" />
            </UpdateParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
