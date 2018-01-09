<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="SubFamilyMaintenance.aspx.vb" Inherits="DataMaintenance_SubFamilyMaintenance"
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
                    <asp:Label ID="lblSubFamily" runat="server" Text="Sub Family Name:" />
                </td>
                <td>
                    <asp:TextBox ID="txtSubFamilyNameSearch" runat="server" Width="174px" />
                </td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CausesValidation="False" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="False" />
                </td>
            </tr>
        </table>
        <hr />
        <asp:GridView ID="gvSubFamilyList" runat="server" AutoGenerateColumns="False" DataKeyNames="SubFamilyID"
            SkinID="StandardGridWOFooter" DataSourceID="odsSubFamilyList" AllowPaging="True"
            PageSize="30" AllowSorting="True">
            <Columns>
                <asp:BoundField DataField="SubFamilyID" HeaderText="Sub Family ID" ReadOnly="True"
                    SortExpression="SubFamilyID" HeaderStyle-HorizontalAlign="Left" >
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="SubFamilyName" HeaderText="Sub Family Name" ReadOnly="True"
                    SortExpression="SubFamilyName" HeaderStyle-HorizontalAlign="Left" >
                    <HeaderStyle HorizontalAlign="Left" />
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
        <asp:ValidationSummary ID="vsEditSubFamilyInfo" runat="server" ShowMessageBox="True"
            Width="599px" ValidationGroup="EditSubFamilyInfo" Height="35px" />
        <asp:ValidationSummary ID="vsEmptySubFamilyInfo" runat="server" ShowMessageBox="True"
            Width="599px" ValidationGroup="EmptySubFamilyInfo" />
        <asp:ObjectDataSource ID="odsSubFamilyList" runat="server" SelectMethod="GetSubFamilies"
            TypeName="SubFamiliesBLL" UpdateMethod="UpdateSubFamily" OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:Parameter DefaultValue="0" Name="FamilyID" Type="Int32" />
                <asp:Parameter DefaultValue="0" Name="SubFamilyID" Type="Int32" />
                <asp:QueryStringParameter Name="SubFamilyName" QueryStringField="SubFamilyName" Type="String"
                    DefaultValue="" />
                <asp:Parameter DefaultValue="False" Name="getOldSubFamilyName" Type="Boolean" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="SubFamilyName" Type="String" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
                <asp:Parameter Name="UpdatedBy" Type="String" />
                <asp:Parameter Name="original_SubFamilyID" Type="Int32" />
            </UpdateParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
