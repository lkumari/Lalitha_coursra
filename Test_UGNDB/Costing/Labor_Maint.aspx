<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Labor_Maint.aspx.vb" Inherits="Labor_Maint" MaintainScrollPositionOnPostback="true"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="98%" DefaultButton="btnSearch">
        <asp:Label runat="server" ID="lblMessage" SkinID="MessageLabelSkin"></asp:Label>
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <table width="100%">
            <tr>
                <td align="right">
                    <asp:Label runat="server" ID="lblLaborDesc" Text="Labor Description:" CssClass="p_text"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchLaborDesc" MaxLength="50"></asp:TextBox>
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
        <asp:ValidationSummary ID="vsEditLabor" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgEditLabor" />
        <asp:ValidationSummary ID="vsFooterLabor" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgFooterLabor" />
        <asp:GridView runat="server" ID="gvLabor" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" PageSize="15" ShowFooter="True" DataSourceID="odsLabor"
            DataKeyNames="LaborID" Width="100%">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
            <Columns>
                <asp:BoundField DataField="LaborID" HeaderText="LaborID" SortExpression="LaborID" />
                <asp:TemplateField HeaderText="Description" SortExpression="LaborDesc">
                    <EditItemTemplate>
                        <asp:Label ID="lblEditLaborDescMarker" runat="server" Font-Bold="True" ForeColor="Red"
                            Text="*" />
                        <asp:TextBox ID="txtEditLaborDesc" runat="server" MaxLength="50" Width="300px" Text='<%# Bind("LaborDesc") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvEditLabor" runat="server" ControlToValidate="txtEditLaborDesc"
                            ErrorMessage="The description is required." Font-Bold="True" ValidationGroup="vgEditLabor"
                            Text="<" SetFocusOnError="true" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewLaborDesc" runat="server" Text='<%# Bind("ddLaborDesc") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblFooterLaborDescMarker" runat="server" Font-Bold="True" ForeColor="Red"
                            Text="*" />
                        <asp:TextBox ID="txtFooterLaborDesc" runat="server" MaxLength="50" Width="300px"
                            Text='<%# Bind("LaborDesc") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvFooterLabor" runat="server" ControlToValidate="txtFooterLaborDesc"
                            ErrorMessage="The description is required." Font-Bold="True" ValidationGroup="vgFooterLabor"
                            Text="<" SetFocusOnError="true" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rate" SortExpression="Rate">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditRate" runat="server" MaxLength="8" Width="75px" Text='<%# Bind("Rate") %>'></asp:TextBox>
                        <asp:CompareValidator runat="server" ID="cvEditRate" Operator="DataTypeCheck" ValidationGroup="vgEditLabor"
                            Type="double" Text="<" ControlToValidate="txtEditRate" ErrorMessage="Rate must be a number."
                            SetFocusOnError="True" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewRate" runat="server" Text='<%# Bind("Rate") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtFooterRate" runat="server" MaxLength="8" Width="75px" Text='<%# Bind("Rate") %>'></asp:TextBox>
                        <asp:CompareValidator runat="server" ID="cvFooterRate" Operator="DataTypeCheck" ValidationGroup="vgFooterLabor"
                            Type="double" Text="<" ControlToValidate="txtFooterRate" ErrorMessage="Rate must be a number."
                            SetFocusOnError="True" />
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Crew Size" SortExpression="CrewSize">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditCrewSize" runat="server" MaxLength="8" Width="75px" Text='<%# Bind("CrewSize") %>'></asp:TextBox>
                        <asp:CompareValidator runat="server" ID="cvEditCrewSize" Operator="DataTypeCheck"
                            ValidationGroup="vgEditLabor" Type="double" Text="<" ControlToValidate="txtEditCrewSize"
                            ErrorMessage="Crew Size must be a number." SetFocusOnError="True" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewCrewSize" runat="server" Text='<%# Bind("CrewSize") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtFooterCrewSize" runat="server" MaxLength="8" Width="75px" Text='<%# Bind("CrewSize") %>'></asp:TextBox>
                        <asp:CompareValidator runat="server" ID="cvFooterCrewSize" Operator="DataTypeCheck"
                            ValidationGroup="vgFooterLabor" Type="double" Text="<" ControlToValidate="txtFooterCrewSize"
                            ErrorMessage="Crew Size must be a number." SetFocusOnError="True" />
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Offline" SortExpression="isOffline">
                    <EditItemTemplate>
                        <asp:CheckBox ID="cbEditIsOffline" runat="server" Checked='<%# Bind("isOffline") %>' />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="cbViewIsOffline" runat="server" Checked='<%# Bind("isOffline") %>'
                            Enabled="false" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:CheckBox ID="cbFooterIsOffline" runat="server" Checked='<%# Bind("isOffline") %>' />
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
                        <asp:ImageButton ID="iBtnLaborUpdate" runat="server" CausesValidation="True" CommandName="Update"
                            ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditLabor" />
                        <asp:ImageButton ID="iBtnLaborCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                            ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="iBtnLaborEdit" runat="server" CausesValidation="False" CommandName="Edit"
                            ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgFooterLabor"
                            runat="server" ID="iBtnFooterLabor" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                        <asp:ImageButton ID="iBtnLaborUndo" runat="server" CommandName="Undo" CausesValidation="false"
                            ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsLabor" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetLabor" TypeName="LaborBLL" UpdateMethod="UpdateLabor" InsertMethod="InsertLabor">
            <SelectParameters>
                <asp:QueryStringParameter Name="LaborID" QueryStringField="LaborID" Type="Int32"
                    DefaultValue="" />
                <asp:QueryStringParameter Name="LaborDesc" QueryStringField="LaborDesc" Type="String" />
                <asp:Parameter DefaultValue="" Name="filterOffline" Type="Boolean" />
                <asp:Parameter DefaultValue="" Name="isOffline" Type="Boolean" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="LaborID" Type="Int32" />
                <asp:Parameter Name="original_LaborID" Type="Int32" />
                <asp:Parameter Name="LaborDesc" Type="String" />
                <asp:Parameter Name="Rate" Type="Double" />
                <asp:Parameter Name="CrewSize" Type="Double" />
                <asp:Parameter Name="isOffline" Type="Boolean" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="LaborDesc" Type="String" />
                <asp:Parameter Name="Rate" Type="Double" />
                <asp:Parameter Name="CrewSize" Type="Double" />
                <asp:Parameter Name="isOffline" Type="Boolean" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
