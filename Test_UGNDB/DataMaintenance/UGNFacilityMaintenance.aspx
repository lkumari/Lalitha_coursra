<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="UGNFacilityMaintenance.aspx.vb" Inherits="DataMaintenance_UGNFacilityMaintenance"
    Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server">
        <hr />
        <asp:Label ID="lblMessage" runat="server" Width="818px" SkinID="MessageLabelSkin"/>
        <br />
        <asp:GridView ID="gvUGNFacilityList" runat="server" AutoGenerateColumns="False" DataKeyNames="UGNFacility"
            DataSourceID="odsUGNFacilityList" AllowPaging="True" Width="550px" PageSize="30"
            SkinID="StandardGrid">
            <Columns>
                <asp:TemplateField HeaderText="UGN Facility ID" SortExpression="UGNFacility">
                    <ItemStyle Wrap="False" />
                    <EditItemTemplate>
                        <asp:Label ID="lblUGNFacilityEdit" runat="server" Text='<%# Bind("UGNFacility") %>' />
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtUGNFacilityInsert" runat="server" MaxLength="2" Width="30px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvUGNFacilityInsert" runat="server" ControlToValidate="txtUGNFacilityInsert"
                            ErrorMessage="UGN Facility ID is Required for Insert" ValidationGroup="InsertUGNFacilityInfo">
                    <
                        </asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblUGNFacilityPreEdit" runat="server" Text='<%# Bind("UGNFacility") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterStyle Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="UGN Facility Name" SortExpression="UGNFacilityName">
                    <ItemStyle Wrap="False" />
                    <EditItemTemplate>
                        <asp:TextBox ID="txtUGNFacilityNameEdit" runat="server" Text='<%# Bind("UGNFacilityName") %>'
                            MaxLength="25" Width="200px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvUGNFacilityNameEdit" runat="server" ControlToValidate="txtUGNFacilityNameEdit"
                            Display="Dynamic" ErrorMessage="UGNFacility Name is Required for Update." Font-Bold="True"
                            ValidationGroup="EditUGNFacilityInfo"><</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblUGNFacilityNamePreEdit" runat="server" Text='<%# Bind("ddUGNFacilityName") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <FooterTemplate>
                        <asp:TextBox ID="txtUGNFacilityNameInsert" runat="server" Text="" ValidationGroup="InsertUGNFacilityInfo"
                            MaxLength="25" Width="200px"></asp:TextBox>&nbsp;
                        <asp:RequiredFieldValidator ID="rfvUGNFacilityNameInsert" runat="server" ControlToValidate="txtUGNFacilityNameInsert"
                            ErrorMessage="UGNFacility Name is Required for Insert" ValidationGroup="InsertUGNFacilityInfo"><                </asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <FooterStyle Wrap="False" />
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
                    SortExpression="comboUpdateInfo" HeaderStyle-HorizontalAlign="left" />
                <asp:TemplateField ShowHeader="False">
                    <EditItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True"
                            CommandName="Update" ImageUrl="~/images/save.jpg" Text="Update" AlternateText="Update"
                            ValidationGroup="EditUGNFacilityInfo" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ibtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                            ImageUrl="~/images/cancel.jpg" Text="Cancel" AlternateText="Cancel" ValidationGroup="EditUGNFacilityInfo" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False"
                            CommandName="Edit" ImageUrl="~/images/edit.jpg" Text="Edit" AlternateText="Edit" />&nbsp;&nbsp;&nbsp;
                    </ItemTemplate>
                    <FooterTemplate>
                        &nbsp;&nbsp;<asp:ImageButton Text="Insert" CommandName="Insert" CausesValidation="true"
                            ValidationGroup="InsertUGNFacilityInfo" runat="server" ID="ibtnInsert" ImageUrl="~/images/save.jpg"
                            AlternateText="Insert" />
                        &nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo"
                            ImageUrl="~/images/undo-gray.jpg" Text="Undo" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        &nbsp;&nbsp;
        <asp:ValidationSummary ID="vsEditUGNFacilityInfo" runat="server" ShowMessageBox="True"
            Width="599px" ValidationGroup="EditUGNFacilityInfo" Height="35px" />
        <asp:ValidationSummary ID="vsInsertUGNFacilityInfo" runat="server" ShowMessageBox="True"
            Width="597px" ValidationGroup="InsertUGNFacilityInfo" />
        <asp:ObjectDataSource ID="odsUGNFacilityList" runat="server" InsertMethod="InsertUGNFacilty"
            OldValuesParameterFormatString="original_{0}" SelectMethod="GetUGNFacilties"
            TypeName="UGNFaciltiesBLL" UpdateMethod="UpdateUGNFacilty">
            <UpdateParameters>
                <asp:Parameter Name="UGNFacility" Type="String" />
                <asp:Parameter Name="UGNFacilityName" Type="String" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
                <asp:Parameter Name="UpdatedBy" Type="String" />
                <asp:Parameter Name="original_UGNFacility" Type="String" />
            </UpdateParameters>
            <SelectParameters>
                <asp:Parameter Name="UGNFacilityName" Type="String" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="UGNFacility" Type="String" />
                <asp:Parameter Name="UGNFacilityName" Type="String" />
                <asp:Parameter Name="createdBy" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
