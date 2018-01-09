<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Overhead_Maint.aspx.vb" Inherits="Overhead_Maint" MaintainScrollPositionOnPostback="true"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="98%" DefaultButton="btnSearch">
        <asp:Label runat="server" ID="lblMessage" SkinID="MessageLabelSkin"></asp:Label>
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <table width="100%">
            <tr>
                <td align="right">
                    <asp:Label runat="server" ID="lblOverheadDesc" Text="Description:" CssClass="p_text"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchOverheadDesc" MaxLength="50"></asp:TextBox>
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
        <asp:ValidationSummary ID="vsEditOverhead" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgEditOverhead" />
        <asp:ValidationSummary ID="vsFooterOverhead" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgFooterOverhead" />
        <asp:GridView runat="server" ID="gvOverhead" AllowPaging="True" AllowSorting="True"
            AutoGenerateColumns="False" PageSize="15" ShowFooter="True" DataSourceID="odsOverhead"
            DataKeyNames="RowID" Width="100%">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
            <Columns>
                <asp:BoundField DataField="RowID" HeaderText="RowID" SortExpression="RowID" />
                <asp:BoundField DataField="LaborID" HeaderText="LaborID" SortExpression="LaborID" />
                <asp:TemplateField HeaderText="Description" SortExpression="LaborDesc">
                    <EditItemTemplate>
                        <asp:Label ID="lblEditLaborDesc" runat="server" Text='<%# Bind("ddLaborDesc") %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewLaborDesc" runat="server" Text='<%# Bind("ddLaborDesc") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblFooterOverheadMarker" runat="server" Font-Bold="True" ForeColor="Red"
                            Text="*" />
                        <asp:DropDownList ID="ddFooterOverhead" runat="server" DataSource='<%# CostingModule.GetLabor(0,"",False,False) %>'
                            DataValueField="LaborID" DataTextField="ddLaborDesc" AppendDataBoundItems="True"
                            SelectedValue='<%# Bind("LaborID") %>'>
                            <asp:ListItem Text="" Value="0" Selected="False"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvFooterOverhead" runat="server" ControlToValidate="ddFooterOverhead"
                            ErrorMessage="The description is required." Font-Bold="True" ValidationGroup="vgFooterOverhead"
                            Text="<" SetFocusOnError="true" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Fixed Rate" SortExpression="Rate">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditRate" runat="server" Text='<%# Bind("Rate") %>'></asp:TextBox>
                        <asp:CompareValidator runat="server" ID="cvEditRate" Operator="DataTypeCheck" ValidationGroup="vgEditOverhead"
                            Type="double" Text="<" ControlToValidate="txtEditRate" ErrorMessage="Rate must be a number."
                            SetFocusOnError="True" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewRate" runat="server" Text='<%# Bind("Rate") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtFooterRate" runat="server" Text='<%# Bind("Rate") %>'></asp:TextBox>
                        <asp:CompareValidator runat="server" ID="cvFooterRate" Operator="DataTypeCheck" ValidationGroup="vgFooterOverhead"
                            Type="double" Text="<" ControlToValidate="txtFooterRate" ErrorMessage="Rate must be a number."
                            SetFocusOnError="True" />
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Variable Rate" SortExpression="Rate">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditVariableRate" runat="server" Text='<%# Bind("VariableRate") %>'></asp:TextBox>
                        <asp:CompareValidator runat="server" ID="cvEditVariableRate" Operator="DataTypeCheck" ValidationGroup="vgEditOverhead"
                            Type="double" Text="<" ControlToValidate="txtEditVariableRate" ErrorMessage="Rate must be a number."
                            SetFocusOnError="True" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewVariableRate" runat="server" Text='<%# Bind("VariableRate") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtFooterVariableRate" runat="server" Text='<%# Bind("VariableRate") %>'></asp:TextBox>
                        <asp:CompareValidator runat="server" ID="cvFooterVariableRate" Operator="DataTypeCheck" ValidationGroup="vgFooterOverhead"
                            Type="double" Text="<" ControlToValidate="txtFooterVariableRate" ErrorMessage="Rate must be a number."
                            SetFocusOnError="True" />
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Crew Size" SortExpression="CrewSize">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditCrewSize" runat="server" Text='<%# Bind("CrewSize") %>'></asp:TextBox>
                        <asp:CompareValidator runat="server" ID="cvEditCrewSize" Operator="DataTypeCheck"
                            ValidationGroup="vgEditOverhead" Type="double" Text="<" ControlToValidate="txtEditCrewSize"
                            ErrorMessage="Crew Size must be a number." SetFocusOnError="True" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblViewCrewSize" runat="server" Text='<%# Bind("CrewSize") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtFooterCrewSize" runat="server" Text='<%# Bind("CrewSize") %>'></asp:TextBox>
                        <asp:CompareValidator runat="server" ID="cvFooterCrewSize" Operator="DataTypeCheck"
                            ValidationGroup="vgFooterOverhead" Type="double" Text="<" ControlToValidate="txtFooterCrewSize"
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
                        <asp:ImageButton ID="iBtnOverheadUpdate" runat="server" CausesValidation="True" CommandName="Update"
                            ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditOverhead" />
                        <asp:ImageButton ID="iBtnOverheadCancel" runat="server" CausesValidation="False"
                            CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="iBtnOverheadEdit" runat="server" CausesValidation="False" CommandName="Edit"
                            ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgFooterOverhead"
                            runat="server" ID="iBtnFooterOverhead" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                        <asp:ImageButton ID="iBtnOverheadUndo" runat="server" CommandName="Undo" CausesValidation="false"
                            ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsOverhead" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetOverhead" TypeName="OverheadBLL" UpdateMethod="UpdateOverhead"
            InsertMethod="InsertOverhead">
            <SelectParameters>
                <asp:QueryStringParameter Name="LaborID" QueryStringField="LaborID" Type="Int32" />
                <asp:QueryStringParameter Name="LaborDesc" QueryStringField="LaborDesc" Type="String" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="LaborID" Type="Int32" />
                <asp:Parameter Name="original_LaborID" Type="Int32" />
                <asp:Parameter Name="Rate" Type="Double" />
                <asp:Parameter Name="VariableRate" Type="Double" />
                <asp:Parameter Name="CrewSize" Type="Double" />
                <asp:Parameter Name="isOffline" Type="Boolean" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
                <asp:Parameter Name="RowID" Type="Int32" />
                <asp:Parameter Name="ddLaborDesc" Type="String" />
                <asp:Parameter Name="original_RowID" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="LaborID" Type="Int32" />
                <asp:Parameter Name="Rate" Type="Double" />
                <asp:Parameter Name="VariableRate" Type="Double" />
                <asp:Parameter Name="CrewSize" Type="Double" />
                <asp:Parameter Name="isOffline" Type="Boolean" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
