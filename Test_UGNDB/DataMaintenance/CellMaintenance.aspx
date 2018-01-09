<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="CellMaintenance.aspx.vb" Inherits="DataMaintenance_CellMaintenance"
    Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <hr />
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <br />
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
        <br />
        <table>
            <tr>
                <td class="p_text" style="width: 168px">
                    <asp:Label ID="lblCell" runat="server" Text="Cell Name:"/> 
                </td>
                <td style="width: 239px">
                    <asp:TextBox ID="txtCellNameSearch" runat="server" Width="200px" MaxLength="30"/>
                </td>
                <td class="p_text" style="width: 176px">
                    <asp:Label ID="lblDepartment" runat="server" Text="Department Name:"/> 
                </td>
                <td style="width: 344px">
                    <asp:DropDownList ID="ddDepartmentSearch" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="p_text" style="width: 168px">
                    <asp:Label ID="lblUGNFacility" runat="server" Text="UGN Facility:"/>   
                </td>
                <td style="width: 239px">
                    <asp:DropDownList ID="ddUGNFacilitySearch" runat="server" />
                </td>
                <td class="p_text" style="width: 176px">
                    <asp:Label ID="lblPlannerCode" runat="server" Text="Planner Code:"/>
                </td>
                <td style="width: 344px">
                    <asp:TextBox ID="txtPlannerCodeSearch" runat="server" Width="30px" MaxLength="1"/>
                </td>
            </tr>
            <tr>
                <td align="center" style="width: 168px">
                    &nbsp;
                </td>
                <td colspan="3">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CausesValidation="False" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="False" />
                </td>
            </tr>
        </table>
        <hr />
        <asp:GridView ID="gvCellList" runat="server" AutoGenerateColumns="False" DataKeyNames="CellID,DepartmentID,UGNFacility,CellName"
            ShowFooter="True" DataSourceID="odsCellList" AllowPaging="True" Width="850px"
            OnRowCommand="gvCellList_RowCommand" AllowSorting="True" SkinID="StandardGrid">
            <Columns>
                <asp:BoundField DataField="CellID" HeaderText="Cell ID" ReadOnly="True" SortExpression="CellID"
                    Visible="False" />
                <asp:TemplateField HeaderText="Cell Name" SortExpression="CellName">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtCellName" runat="server" MaxLength="30" Text='<%# Bind("CellName") %>'
                            Width="200px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvCellName" runat="server" ControlToValidate="txtCellName"
                            Display="Dynamic" ErrorMessage="Cell Name is Required for Update." Font-Bold="True"
                            Font-Size="Medium" ValidationGroup="EditCellInfo"><</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblCellNamePreEdit" runat="server" Text='<%# Bind("ddCellName") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <FooterTemplate>
                        <asp:TextBox ID="txtCellNameInsert" runat="server" MaxLength="30" ValidationGroup="InsertCellInfo"
                            Width="200px"></asp:TextBox>&nbsp;
                        <asp:RequiredFieldValidator ID="rfvCellNameInsert" runat="server" ControlToValidate="txtCellNameInsert"
                            ErrorMessage="Cell Name is Required for Insert" ValidationGroup="InsertCellInfo">
                        <
                        </asp:RequiredFieldValidator>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Department Name" SortExpression="DepartmentName">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddDepartmentEdit" runat="server" ValidationGroup="EditCellInfo"
                            AppendDataBoundItems="true" DataSource='<%# commonFunctions.GetDepartment("","",0)%>'
                            DataValueField="DepartmentID" DataTextField="ddDepartmentName" SelectedValue='<%# Bind("DepartmentID") %>'>
                            <asp:ListItem Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvDepartmetName" runat="server" ControlToValidate="ddDepartmentEdit"
                            Display="Dynamic" ErrorMessage="Department is Required for Update." Font-Bold="True"
                            Font-Size="Medium" ValidationGroup="EditCellInfo"><</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblDepartmentNamePreEdit" runat="server" Text='<%# Bind("DepartmentName") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <FooterTemplate>
                        <asp:DropDownList ID="ddDepartmentInsert" runat="server" ValidationGroup="InsertCellInfo"
                            AppendDataBoundItems="true" DataSource='<%# commonFunctions.GetDepartment("","",0)%>'
                            DataValueField="DepartmentID" DataTextField="ddDepartmentName" SelectedValue='<%# Bind("DepartmentID") %>'>
                            <asp:ListItem Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvDepartmentNameInsert" runat="server" ControlToValidate="ddDepartmentInsert"
                            ErrorMessage="Department is Required for Insert" ValidationGroup="InsertCellInfo">
                        <
                        </asp:RequiredFieldValidator>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="UGN Facility" SortExpression="UGNFacility" HeaderStyle-HorizontalAlign="left">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddUGNFacilityEdit" runat="server" DataSource='<%# commonFunctions.GetUGNFacility("")%>'
                            DataValueField="UGNFacility" DataTextField="ddUGNFacilityName" SelectedValue='<%# Bind("UGNFacility") %>'
                            AppendDataBoundItems="true" ValidationGroup="EditCellInfo">
                            <asp:ListItem Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvUGNFacilityEdit" runat="server" ControlToValidate="ddUGNFacilityEdit"
                            ErrorMessage="UGN Facility is Required for Update." ValidationGroup="EditCellInfo">
                         <
                        </asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblUGNFacilityPreEdit" runat="server" Text='<%# Bind("UGNFacilityName") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddUGNFacilityInsert" runat="server" DataSource='<%# commonFunctions.GetUGNFacility("")%>'
                            DataValueField="UGNFacility" DataTextField="ddUGNFacilityName" SelectedValue='<%# Bind("UGNFacility") %>'
                            AppendDataBoundItems="true" ValidationGroup="InsertCellInfo">
                            <asp:ListItem Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvUGNFacilityInsert" runat="server" ControlToValidate="ddUGNFacilityInsert"
                            ErrorMessage="UGN Facility is Required for Insert" ValidationGroup="InsertCellInfo">                    
                    <
                        </asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Planner</br>Code" SortExpression="PlannerCode">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtPlannerCodeEdit" runat="server" MaxLength="1" Text='<%# Bind("PlannerCode") %>'
                            Width="30px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvPlannerCodeEdit" runat="server" ControlToValidate="txtPlannerCodeEdit"
                            ErrorMessage="Planner Code is Required for Update." ValidationGroup="EditCellInfo">
                         <
                        </asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:Label ID="lblPlannerCodePreEdit" runat="server" Text='<%# Bind("PlannerCode") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtPlannerCodeInsert" runat="server" MaxLength="2" Width="30px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvPlannerCodeInsert" runat="server" ControlToValidate="txtPlannerCodeInsert"
                            ErrorMessage="Planner Code is Required for Insert" ValidationGroup="InsertCellInfo">                    
                    <
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
                <asp:BoundField DataField="comboUpdateInfo" HeaderText="Last Update" ReadOnly="True"
                    SortExpression="comboUpdateInfo" HeaderStyle-HorizontalAlign="left" >
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:TemplateField ShowHeader="False">
                    <EditItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True"
                            CommandName="Update" ImageUrl="~/images/save.jpg" Text="Update" AlternateText="Update"
                            ValidationGroup="EditCellInfo" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ibtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                            ImageUrl="~/images/cancel.jpg" Text="Cancel" AlternateText="Cancel" ValidationGroup="EditCellInfo" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False"
                            CommandName="Edit" ImageUrl="~/images/edit.jpg" Text="Edit" AlternateText="Edit" />&nbsp;&nbsp;&nbsp;
                    </ItemTemplate>
                    <FooterTemplate>
                        &nbsp;&nbsp;<asp:ImageButton Text="Insert" CommandName="Insert" CausesValidation="true"
                            ValidationGroup="InsertCellInfo" runat="server" ID="ibtnInsert" ImageUrl="~/images/save.jpg"
                            AlternateText="Insert" />
                        &nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo"
                            ImageUrl="~/images/undo-gray.jpg" Text="Undo" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        &nbsp;&nbsp;
        <asp:ValidationSummary ID="vsEditCellInfo" runat="server" ShowMessageBox="True" Width="599px"
            ValidationGroup="EditCellInfo" Height="35px" />
        <asp:ValidationSummary ID="vsInsertCellInfo" runat="server" ShowMessageBox="True"
            Width="599px" ValidationGroup="InsertCellInfo" />
        <br />
        <asp:ValidationSummary ID="vsEmptyCellInfo" runat="server" ShowMessageBox="True"
            Width="599px" ValidationGroup="EmptyCellInfo" />
        <asp:ObjectDataSource ID="odsCellList" runat="server" SelectMethod="GetCells" TypeName="CellsBLL"
            UpdateMethod="UpdateCell" InsertMethod="InsertCell" OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:QueryStringParameter DefaultValue="0" Name="CellID" QueryStringField="CellID"
                    Type="Int32" />
                <asp:QueryStringParameter DefaultValue="0" Name="DepartmentID" QueryStringField="DepartmentID"
                    Type="Int32" />
                <asp:QueryStringParameter Name="UGNFacility" QueryStringField="UGNFacility" Type="String"
                    DefaultValue="" />
                <asp:QueryStringParameter Name="CellName" QueryStringField="CellName" Type="String" />
                <asp:QueryStringParameter Name="PlannerCode" QueryStringField="PlannerCode" Type="String" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="DepartmentID" Type="Int32" />
                <asp:Parameter Name="UGNFacility" Type="String" />
                <asp:Parameter Name="CellName" Type="String" />
                <asp:Parameter Name="PlannerCode" Type="String" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
                <asp:Parameter Name="UpdatedBy" Type="String" />
                <asp:Parameter Name="original_CellID" Type="Int32" />
                <asp:Parameter Name="original_DepartmentID" Type="Int32" />
                <asp:Parameter Name="original_UGNFacility" Type="String" />
                <asp:Parameter Name="original_CellName" Type="String" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="DepartmentID" Type="Int32" />
                <asp:Parameter Name="UGNFacility" Type="String" />
                <asp:Parameter Name="CellName" Type="String" />
                <asp:Parameter Name="PlannerCode" Type="String" />
                <asp:Parameter Name="createdBy" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
