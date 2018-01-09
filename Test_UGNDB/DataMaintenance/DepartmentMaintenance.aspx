<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="DepartmentMaintenance.aspx.vb" Inherits="DataMaintenance_DepartmentMaintenance"
    Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <hr />
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <br />
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin" />
        <br />
        <table>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblCOADesc" runat="server" Text="COA Description:" />
                </td>
                <td>
                    <asp:TextBox ID="txtDepartmentNameSearch" runat="server" Width="250px" MaxLength="30" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblUGNFacility" runat="server" Text="UGN Facility:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddUGNFacilitySearch" runat="server" />
                </td>
                <td class="p_text">
                    <asp:CheckBox runat="server" ID="cbFilter" Text="Filter" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="4">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CausesValidation="False" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="False" />
                </td>
            </tr>
        </table>
        <hr />
        <asp:GridView ID="gvDepartmentList" runat="server" AutoGenerateColumns="False" DataKeyNames="DepartmentID"
            DataSourceID="odsDepartmentList" AllowPaging="True" OnRowCommand="gvDepartmentList_RowCommand"
            AllowSorting="True" PageSize="30" Width="900px" SkinID="StandardGrid">
            <Columns>
                <asp:BoundField DataField="DepartmentID" HeaderText="Department ID" ReadOnly="True"
                    SortExpression="DepartmentID" Visible="False" />
                <asp:TemplateField HeaderText="COA Description" SortExpression="DepartmentName">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtDepartmentNameEdit" runat="server" Text='<%# Bind("DepartmentName") %>'
                            MaxLength="50" Width="250px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvDepartmentName" runat="server" ControlToValidate="txtDepartmentNameEdit"
                            Display="Dynamic" ErrorMessage="Department Name is Required for Update." Font-Bold="True"
                            Font-Size="Medium" ValidationGroup="EditDepartmentInfo"><</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblDepartmentNamePreEdit" runat="server" Text='<%# Bind("DepartmentName") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <FooterTemplate>
                        <asp:TextBox ID="txtDepartmentNameInsert" runat="server" Text="" ValidationGroup="InsertDepartmentInfo"
                            MaxLength="50" Width="250px"></asp:TextBox>&nbsp;
                        <asp:RequiredFieldValidator ID="rfvDepartmentNameInsert" runat="server" ControlToValidate="txtDepartmentNameInsert"
                            ErrorMessage="Department Name is Required for Insert" ValidationGroup="InsertDepartmentInfo">
                        <
                        </asp:RequiredFieldValidator>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="General Ledger #" SortExpression="GLNo" HeaderStyle-HorizontalAlign="Left">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtGLNo" runat="server" Text='<%# Bind("GLNo") %>' MaxLength="10"
                            Width="100px" />
                        <ajax:FilteredTextBoxExtender ID="ftbeGLNo" runat="server" TargetControlID="txtGLNo"
                            FilterType="Numbers" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblGLNo" runat="server" Text='<%# Bind("GLNo") %>' />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtGLNoInsert" runat="server" Text="" MaxLength="10" Width="100px" />
                        <ajax:FilteredTextBoxExtender ID="ftbeGLNo" runat="server" TargetControlID="txtGLNoInsert"
                            FilterType="Numbers" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="UGN Facility" SortExpression="UGNFacility">
                    <HeaderStyle HorizontalAlign="Left" />
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddUGNFacilityEdit" runat="server" DataSource='<%# commonFunctions.GetUGNFacility("")%>'
                            DataValueField="UGNFacility" DataTextField="ddUGNFacilityName" SelectedValue='<%# Bind("UGNFacility") %>'
                            AppendDataBoundItems="true" ValidationGroup="EditDepartmentInfo">
                            <asp:ListItem Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvUGNFacilityEdit" runat="server" ControlToValidate="ddUGNFacilityEdit"
                            ErrorMessage="UGN Facility is Required for Update." ValidationGroup="EditDepartmentInfo">
                         <
                        </asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblUGNFacilityPreEdit" runat="server" Text='<%# Bind("UGNFacilityName") %>' />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddUGNFacilityInsert" runat="server" DataSource='<%# commonFunctions.GetUGNFacility("")%>'
                            DataValueField="UGNFacility" DataTextField="ddUGNFacilityName" SelectedValue='<%# Bind("UGNFacility") %>'
                            AppendDataBoundItems="true" Width="156px" ValidationGroup="InsertDepartmentInfo">
                            <asp:ListItem Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvUGNFacilityInsert" runat="server" ControlToValidate="ddUGNFacilityInsert"
                            ErrorMessage="UGN Facility is Required for Insert" ValidationGroup="InsertDepartmentInfo">                    
                    <
                        </asp:RequiredFieldValidator>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Filter" SortExpression="Filter">
                    <EditItemTemplate>
                        <asp:CheckBox ID="chkFilterEdit" runat="server" Checked='<%# Bind("Filter") %>' />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkFilterPreEdit" runat="server" Checked='<%# Bind("Filter") %>'
                            Enabled="false" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:CheckBox ID="chkFilterInsert" runat="server" Checked='<%# Bind("Filter") %>' />
                    </FooterTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                    <FooterStyle HorizontalAlign="Center" />
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
                    SortExpression="comboUpdateInfo">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:TemplateField ShowHeader="False">
                    <EditItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True"
                            CommandName="Update" ImageUrl="~/images/save.jpg" Text="Update" AlternateText="Update"
                            ValidationGroup="EditDepartmentInfo" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ibtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                            ImageUrl="~/images/cancel.jpg" Text="Cancel" AlternateText="Cancel" ValidationGroup="EditDepartmentInfo" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False"
                            CommandName="Edit" ImageUrl="~/images/edit.jpg" Text="Edit" AlternateText="Edit" />&nbsp;&nbsp;&nbsp;
                    </ItemTemplate>
                    <FooterTemplate>
                        &nbsp;&nbsp;<asp:ImageButton Text="Insert" CommandName="Insert" CausesValidation="true"
                            ValidationGroup="InsertDepartmentInfo" runat="server" ID="ibtnInsert" ImageUrl="~/images/save.jpg"
                            AlternateText="Insert" />
                        &nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo"
                            ImageUrl="~/images/undo-gray.jpg" Text="Undo" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        &nbsp;&nbsp;
        <asp:ValidationSummary ID="vsEditDepartmentInfo" runat="server" ShowMessageBox="True"
            Width="599px" ValidationGroup="EditDepartmentInfo" Height="35px" />
        <asp:ValidationSummary ID="vsInsertDepartmentInfo" runat="server" ShowMessageBox="True"
            Width="599px" ValidationGroup="InsertDepartmentInfo" />
        <br />
        <asp:ValidationSummary ID="vsEmptyDepartmentInfo" runat="server" ShowMessageBox="True"
            Width="599px" ValidationGroup="EmptyDepartmentInfo" />
        <asp:ObjectDataSource ID="odsDepartmentList" runat="server" SelectMethod="GetDepartment"
            TypeName="DepartmentsBLL" UpdateMethod="UpdateDepartment" InsertMethod="InsertDepartment"
            OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:QueryStringParameter Name="DepartmentName" QueryStringField="DepartmentName"
                    Type="String" />
                <asp:QueryStringParameter Name="UGNFacility" QueryStringField="UGNFacility" Type="String" />
                <asp:QueryStringParameter Name="Filter" QueryStringField="Filter" Type="Boolean"
                    DefaultValue="false" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="DepartmentName" Type="String" />
                <asp:Parameter Name="UGNFacility" Type="String" />
                <asp:Parameter Name="Filter" Type="Boolean" />
                <asp:Parameter Name="GLNo" Type="Int32" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
                <asp:Parameter Name="UpdatedBy" Type="String" />
                <asp:Parameter Name="original_DepartmentID" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="DepartmentName" Type="String" />
                <asp:Parameter Name="UGNFacility" Type="String" />
                <asp:Parameter Name="Filter" Type="Boolean" />
                <asp:Parameter Name="GLNo" Type="Int32" />
                <asp:Parameter Name="createdBy" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
