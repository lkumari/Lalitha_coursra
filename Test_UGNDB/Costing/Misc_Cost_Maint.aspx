<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    MaintainScrollPositionOnPostback="true" CodeFile="Misc_Cost_Maint.aspx.vb" Inherits="Misc_Cost_Maint"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="98%" DefaultButton="btnSearch">
        <asp:Label runat="server" ID="lblMessage" SkinID="MessageLabelSkin"></asp:Label>
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <table width="100%">
            <tr>
                <td align="right">
                    <asp:Label runat="server" ID="lblMiscCostDesc" Text="Misc Cost Type Description:"
                        CssClass="p_text"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchMiscCostDesc" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" />
                </td>
            </tr>
        </table>
        <br />
        <hr />
        <asp:ValidationSummary ID="vsEditMiscCost" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgEditMiscCost" />
        <asp:ValidationSummary ID="vsFooterMiscCost" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgFooterMiscCost" />
        <asp:GridView runat="server" ID="gvMiscCost" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" PageSize="15" ShowFooter="True" DataSourceID="odsMiscCost"
            DataKeyNames="MiscCostID" Width="100%">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
            <Columns>
                <asp:BoundField DataField="MiscCostID" HeaderText="MiscCostID" SortExpression="MiscCostID" />
                <asp:TemplateField HeaderText="Description" SortExpression="MiscCostDesc">
                    <EditItemTemplate>
                        <asp:Label ID="lblEditMiscCostDescMarker" runat="server" Font-Bold="True" ForeColor="Red"
                            Text="*" />
                        <asp:TextBox ID="txtEditMiscCostDesc" runat="server" MaxLength="50" Width="300px"
                            Text='<%# Bind("MiscCostDesc") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvEditMiscCostDesc" runat="server" ControlToValidate="txtEditMiscCostDesc"
                            ErrorMessage="The description is required." Font-Bold="True" ValidationGroup="vgEditMiscCost"
                            Text="<" SetFocusOnError="true" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewMiscCost" runat="server" Text='<%# Bind("ddMiscCostDesc") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblFooterMiscCostDescMarker" runat="server" Font-Bold="True" ForeColor="Red"
                            Text="*" />
                        <asp:TextBox ID="txtFooterMiscCostDesc" runat="server" MaxLength="50" Width="300px"
                            Text='<%# Bind("MiscCostDesc") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvFooterMiscCostDesc" runat="server" ControlToValidate="txtFooterMiscCostDesc"
                            ErrorMessage="The description is required." Font-Bold="True" ValidationGroup="vgFooterMiscCost"
                            Text="<" SetFocusOnError="true" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rate" SortExpression="Rate">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditRate" runat="server" MaxLength="8" Width="75px" Text='<%# Bind("Rate") %>'></asp:TextBox>
                        <asp:CompareValidator runat="server" ID="cvEditRate" Operator="DataTypeCheck" ValidationGroup="vgEditMiscCost"
                            Type="double" Text="<" ControlToValidate="txtEditRate" ErrorMessage="Rate must be a number."
                            SetFocusOnError="True" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewRate" runat="server" Text='<%# Bind("Rate") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtFooterRate" runat="server" MaxLength="8" Width="75px" Text='<%# Bind("Rate") %>'></asp:TextBox>
                        <asp:CompareValidator runat="server" ID="cvFooterRate" Operator="DataTypeCheck" ValidationGroup="vgFooterMiscCost"
                            Type="double" Text="<" ControlToValidate="txtFooterRate" ErrorMessage="Rate must be a number."
                            SetFocusOnError="True" />
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Show as Rate Percent On Cost Form" SortExpression="isRatePercentage">
                    <EditItemTemplate>
                        <asp:CheckBox ID="cbEditisRatePercentage" runat="server" Checked='<%# Bind("isRatePercentage") %>' />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="cbViewisRatePercentage" runat="server" Checked='<%# Bind("isRatePercentage") %>'
                            Enabled="false" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:CheckBox ID="cbFooterisRatePercentage" runat="server" Checked='<%# Bind("isRatePercentage") %>' />
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
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
                        <asp:ImageButton ID="iBtnMiscCostUpdate" runat="server" CausesValidation="True" CommandName="Update"
                            ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditMiscCost" />
                        <asp:ImageButton ID="iBtnMiscCostCancel" runat="server" CausesValidation="False"
                            CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="iBtnMiscCostEdit" runat="server" CausesValidation="False" CommandName="Edit"
                            ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgFooterMiscCost"
                            runat="server" ID="iBtnFooterMiscCost" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                        <asp:ImageButton ID="iBtnMiscCostUndo" runat="server" CommandName="Undo" CausesValidation="false"
                            ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsMiscCost" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetMiscCost" TypeName="MiscCostBLL" UpdateMethod="UpdateMiscCost"
            InsertMethod="InsertMiscCost">
            <SelectParameters>
                <asp:QueryStringParameter Name="MiscCostID" QueryStringField="MiscCostID" Type="Int32" />
                <asp:QueryStringParameter Name="MiscCostDesc" QueryStringField="MiscCostDesc" Type="String" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="MiscCostDesc" Type="String" />
                <asp:Parameter Name="original_MiscCostID" Type="Int32" />
                <asp:Parameter Name="MiscCostID" Type="Int32" />
                <asp:Parameter Name="Rate" Type="Double" />
                <asp:Parameter Name="QuoteRate" Type="Double" />
                <asp:Parameter Name="isRatePercentage" Type="Boolean" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="MiscCostDesc" Type="String" />
                <asp:Parameter Name="Rate" Type="Double" />
                <asp:Parameter Name="QuoteRate" Type="Double" />
                <asp:Parameter Name="isRatePercentage" Type="Boolean" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
