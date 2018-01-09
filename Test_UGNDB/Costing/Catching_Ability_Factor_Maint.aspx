<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    MaintainScrollPositionOnPostback="true" CodeFile="Catching_Ability_Factor_Maint.aspx.vb"
    Inherits="Catching_Ability_Factor_Maint" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server">
        <asp:Label runat="server" ID="lblMessage" SkinID="MessageLabelSkin"></asp:Label>
        <br />
        <asp:ValidationSummary ID="vsEditCatchingAbilityFactor" runat="server" DisplayMode="List"
            ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgEditCatchingAbilityFactor" />
        <asp:ValidationSummary ID="vsFooterCatchingAbilityFactor" runat="server" DisplayMode="List"
            ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgFooterCatchingAbilityFactor" />
        <asp:GridView runat="server" ID="gvCatchingAbilityFactor" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" PageSize="100" ShowFooter="True" DataSourceID="odsCatchingAbilityFactor"
            DataKeyNames="FactorID" Width="100%">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
            <Columns>
                <asp:BoundField DataField="FactorID" HeaderText="FactorID" InsertVisible="False"
                    ReadOnly="True" SortExpression="FactorID" />
                <asp:TemplateField HeaderText="Min Part Length" SortExpression="MinimumPartLength">
                    <EditItemTemplate>
                        <asp:Label ID="lblEditMinimumPartLengthMarker" runat="server" Font-Bold="True" ForeColor="Red"
                            Text="*" />
                        <asp:TextBox ID="txtEditMinimumPartLength" runat="server" MaxLength="8" Width="75px"
                            Text='<%# Bind("MinimumPartLength") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvEditMinimumPartLength" runat="server" ControlToValidate="txtEditMinimumPartLength"
                            ErrorMessage="Minimum part length is required." Font-Bold="True" ValidationGroup="vgEditCatchingAbilityFactor"
                            Text="<" SetFocusOnError="true" />
                        <asp:CompareValidator runat="server" ID="cvEditMinimumPartLength" Operator="DataTypeCheck"
                            ValidationGroup="vgEditCatchingAbilityFactor" Type="double" Text="<" ControlToValidate="txtEditMinimumPartLength"
                            ErrorMessage="Minimum part length must be a number." SetFocusOnError="True" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewMinimumPartLength" runat="server" Text='<%# Bind("MinimumPartLength") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblFooterMinimumPartLengthMarker" runat="server" Font-Bold="True"
                            ForeColor="Red" Text="*" />
                        <asp:TextBox ID="txtFooterMinimumPartLength" runat="server" MaxLength="8" Width="75px"
                            Text='<%# Bind("MinimumPartLength") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvFoterMinimumPartLength" runat="server" ControlToValidate="txtFooterMinimumPartLength"
                            ErrorMessage="Minimum part length is required." Font-Bold="True" ValidationGroup="vgFooterCatchingAbilityFactor"
                            Text="<" SetFocusOnError="true" />
                        <asp:CompareValidator runat="server" ID="cvFooterMinimumPartLength" Operator="DataTypeCheck"
                            ValidationGroup="vgFooterCatchingAbilityFactor" Type="double" Text="<" ControlToValidate="txtFooterMinimumPartLength"
                            ErrorMessage="Minimum part length must be a number." SetFocusOnError="True" />
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="&lt;=">
                    <EditItemTemplate>
                        <asp:Label ID="txtLesThanEqual" runat="server" Text="<="></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblLesThanEqual" runat="server" Text="<="></asp:Label>
                    </ItemTemplate>
                    <FooterStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Max Part Length" SortExpression="MaximumPartLength">
                    <EditItemTemplate>
                        <asp:Label ID="lblEditMaximumPartLengthMarker" runat="server" Font-Bold="True" ForeColor="Red"
                            Text="*" />
                        <asp:TextBox ID="txtEditMaximumPartLength" runat="server" MaxLength="8" Width="75px"
                            Text='<%# Bind("MaximumPartLength") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvEditMaximumPartLength" runat="server" ControlToValidate="txtEditMaximumPartLength"
                            ErrorMessage="Maximum part length is required." Font-Bold="True" ValidationGroup="vgEditCatchingAbilityFactor"
                            Text="<" SetFocusOnError="true" />
                        <asp:CompareValidator runat="server" ID="cvEditMaximumPartLength" Operator="DataTypeCheck"
                            ValidationGroup="vgEditCatchingAbilityFactor" Type="double" Text="<" ControlToValidate="txtEditMaximumPartLength"
                            ErrorMessage="Maximum part length must be a number." SetFocusOnError="True" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblMaximumPartLength" runat="server" Text='<%# Bind("MaximumPartLength") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblFooterMaximumPartLengthMarker" runat="server" Font-Bold="True"
                            ForeColor="Red" Text="*" />
                        <asp:TextBox ID="txtFooterMaximumPartLength" runat="server" MaxLength="8" Width="75px"
                            Text='<%# Bind("MaximumPartLength") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvFooterMaximumPartLength" runat="server" ControlToValidate="txtFooterMaximumPartLength"
                            ErrorMessage="Maximum part length is required." Font-Bold="True" ValidationGroup="vgFooterCatchingAbilityFactor"
                            Text="<" SetFocusOnError="true" />
                        <asp:CompareValidator runat="server" ID="cvFooterMaximumPartLength" Operator="DataTypeCheck"
                            ValidationGroup="vgFooterCatchingAbilityFactor" Type="double" Text="<" ControlToValidate="txtFooterMaximumPartLength"
                            ErrorMessage="Maximum part length must be a number." SetFocusOnError="True" />
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Side By Side" SortExpression="isSideBySide">
                    <EditItemTemplate>
                        <asp:CheckBox ID="cbEditIsSideBySide" runat="server" Checked='<%# Bind("isSideBySide") %>' />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="cbViewIsSideBySide" runat="server" Checked='<%# Bind("isSideBySide") %>'
                            Enabled="false" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:CheckBox ID="cbFooterIsSideBySide" runat="server" Checked='<%# Bind("isSideBySide") %>' />
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Factor" SortExpression="CatchingAbilityFactor">
                    <EditItemTemplate>
                        <asp:Label ID="lblEditCatchingAbilityFactorMarker" runat="server" Font-Bold="True"
                            ForeColor="Red" Text="*" />
                        <asp:TextBox ID="txtEditCatchingAbilityFactor" runat="server" MaxLength="8" Width="75px"
                            Text='<%# Bind("CatchingAbilityFactor") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvEditCatchingAbilityFactor" runat="server" ControlToValidate="txtEditCatchingAbilityFactor"
                            ErrorMessage="Catching Ability Factor is required." Font-Bold="True" ValidationGroup="vgEditCatchingAbilityFactor"
                            Text="<" SetFocusOnError="true" />
                        <asp:CompareValidator runat="server" ID="cvEditCatchingAbilityFactor" Operator="DataTypeCheck"
                            ValidationGroup="vgEditCatchingAbilityFactor" Type="double" Text="<" ControlToValidate="txtEditCatchingAbilityFactor"
                            ErrorMessage="Catching Ability Factor must be a number." SetFocusOnError="True" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblCatchingAbilityFactor" runat="server" Text='<%# Bind("CatchingAbilityFactor") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblFooterCatchingAbilityFactorMarker" runat="server" Font-Bold="True"
                            ForeColor="Red" Text="*" />
                        <asp:TextBox ID="txtFooterCatchingAbilityFactor" runat="server" MaxLength="8" Width="75px"
                            Text='<%# Bind("CatchingAbilityFactor") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvFooterCatchingAbilityFactor" runat="server" ControlToValidate="txtFooterCatchingAbilityFactor"
                            ErrorMessage="Catching Ability Factor is required." Font-Bold="True" ValidationGroup="vgFooterCatchingAbilityFactor"
                            Text="<" SetFocusOnError="true" />
                        <asp:CompareValidator runat="server" ID="cvFooterCatchingAbilityFactor" Operator="DataTypeCheck"
                            ValidationGroup="vgFooterCatchingAbilityFactor" Type="double" Text="<" ControlToValidate="txtFooterCatchingAbilityFactor"
                            ErrorMessage="Catching Ability Factor must be a number." SetFocusOnError="True" />
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Obsolete" SortExpression="Obsolete">
                    <EditItemTemplate>
                        <asp:CheckBox ID="cbEditObsolete" runat="server" Checked='<%# Bind("Obsolete") %>' />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="cbViewObsolete" runat="server" Checked='<%# Bind("Obsolete") %>'
                            Enabled="false" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:CheckBox ID="cbFooterObsolete" runat="server" Checked='<%# Bind("Obsolete") %>' />
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <EditItemTemplate>
                        <asp:ImageButton ID="iBtnCatchingAbilityFactorUpdate" runat="server" CausesValidation="True"
                            CommandName="Update" ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditCatchingAbilityFactor" />
                        <asp:ImageButton ID="iBtnCatchingAbilityFactorCancel" runat="server" CausesValidation="False"
                            CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="iBtnCatchingAbilityFactorEdit" runat="server" CausesValidation="False"
                            CommandName="Edit" ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgFooterCatchingAbilityFactor"
                            runat="server" ID="iBtnFooterCatchingAbilityFactor" ImageUrl="~/images/save.jpg"
                            AlternateText="Insert" />
                        <asp:ImageButton ID="iBtnCatchingAbilityFactorUndo" runat="server" CommandName="Undo"
                            CausesValidation="false" ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsCatchingAbilityFactor" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetCatchingAbilityFactor" TypeName="CatchingAbilityFactorBLL" UpdateMethod="UpdateCatchingAbilityFactor"
            InsertMethod="InsertCatchingAbilityFactor">
            <SelectParameters>
                <asp:QueryStringParameter Name="FactorID" QueryStringField="FactorID" Type="Int32"
                    DefaultValue="0" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="original_FactorID" Type="Int32" />
                <asp:Parameter Name="MinimumPartLength" Type="Double" />
                <asp:Parameter Name="MaximumPartLength" Type="Double" />
                <asp:Parameter Name="isSideBySide" Type="Boolean" />
                <asp:Parameter Name="CatchingAbilityFactor" Type="Double" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="MinimumPartLength" Type="Double" />
                <asp:Parameter Name="MaximumPartLength" Type="Double" />
                <asp:Parameter Name="isSideBySide" Type="Boolean" />
                <asp:Parameter Name="CatchingAbilityFactor" Type="Double" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
