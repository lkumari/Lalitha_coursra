<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="AssemblyPlantLocationMaint.aspx.vb" Inherits="DataMaintenance_AssemblyPlantLocationMaint"
    MaintainScrollPositionOnPostback="True" Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <hr />
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <br />
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
        <br />
        <table>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblAssemblyPlantLocation" runat="server" Text="Assembly Plant Location:" />
                </td>
                <td>
                    <asp:TextBox ID="txtAssembly" runat="server" MaxLength="50" Width="200px" />
                    <ajax:FilteredTextBoxExtender ID="ftbAPL" runat="server" TargetControlID="txtAssembly"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,%/- " />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblCountry" runat="server" Text="Country:" />
                </td>
                <td>
                    <asp:TextBox ID="txtCountry" runat="server" MaxLength="30" Width="200px" />
                    <ajax:FilteredTextBoxExtender ID="ftbCountry" runat="server" TargetControlID="txtCountry"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,%/- " />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblOEMManufacturer" runat="server" Text="OEM Manufacturer:" />
                </td>
                <td>
                    <asp:TextBox ID="txtOEMMfg" runat="server" MaxLength="50" Width="200px" />
                    <ajax:FilteredTextBoxExtender ID="ftbOEMMfg" runat="server" TargetControlID="txtOEMMfg"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,%/- " />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="5">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CausesValidation="False" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="False" />
                </td>
            </tr>
        </table>
        <hr />
        <asp:Label ID="lblRaiseError" runat="server" Text="" Visible="false" SkinID="MessageLabelSkin" /><br />
        <asp:Label ID="Label2" runat="server"><i>Double astericks (**) at the end of each column heading denotes a required field.</i></asp:Label>
        <br />
        <asp:GridView ID="gvAPL" runat="server" AutoGenerateColumns="False" DataSourceID="odsAPL"
            AllowPaging="True" OnRowCommand="gvAPL_RowCommand" AllowSorting="True" PageSize="30"
            EmptyDataRowStyle-Font-Size="Medium" EmptyDataRowStyle-Font-Bold="true" EmptyDataRowStyle-ForeColor="Red"
            OnRowDeleted="gvAPL_RowDeleted" SkinID="StandardGrid">
            <EmptyDataRowStyle Font-Bold="True" Font-Size="Medium" ForeColor="Red" />
            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <EditItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True"
                            CommandName="Update" ValidationGroup="EditInfo" ImageUrl="~/images/save.jpg"
                            Text="Update" AlternateText="Update" />&nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ibtnCancel"
                                runat="server" CausesValidation="False" CommandName="Cancel" ImageUrl="~/images/cancel.jpg"
                                Text="Cancel" AlternateText="Cancel" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False"
                            CommandName="Edit" ImageUrl="~/images/edit.jpg" Text="Edit" AlternateText="Edit" />&nbsp;&nbsp;&nbsp;
                    </ItemTemplate>
                    <FooterTemplate>
                        &nbsp;&nbsp;<asp:ImageButton Text="Insert" CommandName="Insert" CausesValidation="true"
                            runat="server" ID="ibtnInsert" ImageUrl="~/images/save.jpg" AlternateText="Insert"
                            ValidationGroup="InsertInfo" />
                        &nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo"
                            ImageUrl="~/images/undo-gray.jpg" Text="Undo" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                            ImageUrl="~/images/delete.jpg" AlternateText="Delete" />
                    </ItemTemplate>
                    <HeaderStyle Width="30px" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="OEM Model Type(s)" HeaderStyle-Width="60px">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" ID="hlnkPreview" ImageUrl="~/images/PreviewUp.jpg"
                            ToolTip="Preview OEM Model Types" NavigateUrl='<%# "AssemblyPlantOEMMaint.aspx?pAPID=" & DataBinder.Eval (Container.DataItem,"APID").tostring & "&sAPL=" & txtAssembly.Text & "&sCtry=" & txtCountry.Text & "&sOMfg=" &  txtOEMMfg.Text %>' />
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="APID" SortExpression="APID">
                    <ItemTemplate>
                        <asp:Label ID="Label5" runat="server" Text='<%# Bind("APID") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Label ID="txtAPID" runat="server" Text='<%# Bind("APID") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Orig. IHS Assembly Plant Name" SortExpression="IHS_Assembly_Plant">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtIHSAP1" runat="server" MaxLength="50" Width="200px" Text='<%# Bind("IHS_Assembly_Plant") %>' />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label6" runat="server" Text='<%# Bind("IHS_Assembly_Plant") %>' />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtIHSAP" runat="server" MaxLength="50" Width="200px" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Assembly Plant Location **" SortExpression="Assembly_Plant_Location"
                    HeaderStyle-HorizontalAlign="Left">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtAPL1" runat="server" MaxLength="50" Width="200px" Text='<%# Bind("Assembly_Plant_Location") %>' />
                        <asp:RequiredFieldValidator ID="rfvAPL1" runat="server" ControlToValidate="txtAPL1"
                            Display="Dynamic" ErrorMessage="Assembly Plant Location is a required field."
                            ValidationGroup="EditInfo"><</asp:RequiredFieldValidator>
                        <ajax:FilteredTextBoxExtender ID="ftbAPL1" runat="server" TargetControlID="txtAPL1"
                            FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,()/- " />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("Assembly_Plant_Location") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtAPL" runat="server" MaxLength="50" Width="200px" />
                        <asp:RequiredFieldValidator ID="rfvAPL" runat="server" ControlToValidate="txtAPL"
                            Display="Dynamic" ErrorMessage="Assembly Plant Location is a required field."
                            ValidationGroup="InsertInfo"><</asp:RequiredFieldValidator>
                        <ajax:FilteredTextBoxExtender ID="ftbAPL" runat="server" TargetControlID="txtAPL"
                            FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,()/- " />
                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="State **" SortExpression="State" HeaderStyle-HorizontalAlign="Left">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtState1" runat="server" MaxLength="30" Width="200px" Text='<%# Bind("State") %>' />
                        <asp:RequiredFieldValidator ID="rfvState1" runat="server" ControlToValidate="txtState1"
                            Display="Dynamic" ErrorMessage="State is a required field." ValidationGroup="EditInfo"><</asp:RequiredFieldValidator>
                        <ajax:FilteredTextBoxExtender ID="ftbState1" runat="server" TargetControlID="txtState1"
                            FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,()/- " />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("State") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtState" runat="server" MaxLength="30" Width="200px" />
                        <asp:RequiredFieldValidator ID="rfvState" runat="server" ControlToValidate="txtState"
                            Display="Dynamic" ErrorMessage="State is a required field." ValidationGroup="InsertInfo"><</asp:RequiredFieldValidator>
                        <ajax:FilteredTextBoxExtender ID="ftbState" runat="server" TargetControlID="txtState"
                            FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,()/- " />
                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Country **" SortExpression="Country" HeaderStyle-HorizontalAlign="Left">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddCountry1" runat="server" SelectedValue='<%# Bind("Country") %>'>
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>Brazil</asp:ListItem>
                            <asp:ListItem>Canada</asp:ListItem>
                            <asp:ListItem>Mexico</asp:ListItem>
                            <asp:ListItem>N/A</asp:ListItem>
                            <asp:ListItem>United States</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvCountry1" runat="server" ControlToValidate="ddCountry1"
                            Display="Dynamic" ErrorMessage="Country is a required field." ValidationGroup="EditInfo"><</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("Country") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddCountry" runat="server">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>Brazil</asp:ListItem>
                            <asp:ListItem>Canada</asp:ListItem>
                            <asp:ListItem>Mexico</asp:ListItem>
                            <asp:ListItem>N/A</asp:ListItem>
                            <asp:ListItem>United States</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvCountry" runat="server" ControlToValidate="ddCountry"
                            Display="Dynamic" ErrorMessage="Country is a required field." ValidationGroup="InsertInfo"><</asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="OEM Manufacturer **" SortExpression="OEMManufacturer"
                    HeaderStyle-HorizontalAlign="Left">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddOEMMgf1" runat="server" DataSource='<%# commonFunctions.GetOEMManufacturer("") %>'
                            DataValueField="OEMManufacturer" DataTextField="ddOEMManufacturer" SelectedValue='<%# Bind("OEMManufacturer") %>'
                            AppendDataBoundItems="True">
                            <asp:ListItem Selected="True" Value="" Text="">
                            </asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvOEMMfg1" runat="server" ControlToValidate="ddOEMMgf1"
                            Display="Dynamic" ErrorMessage="OEMManufacturer is a required field." ValidationGroup="EditInfo"><</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("ddOEMManufacturer") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddOEMMfg" runat="server" DataSource='<%# commonFunctions.GetOEMManufacturer("") %>'
                            DataValueField="OEMManufacturer" DataTextField="ddOEMManufacturer" SelectedValue='<%# Bind("OEMManufacturer") %>'
                            AppendDataBoundItems="True">
                            <asp:ListItem Selected="True" Value="" Text="">
                            </asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvOMGMfg" runat="server" ControlToValidate="ddOEMMfg"
                            Display="Dynamic" ErrorMessage="OEM Manufacturer is a required field." ValidationGroup="InsertInfo"><</asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
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
                <asp:TemplateField HeaderText="Assembly Type" SortExpression="AssemblyType">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddAssemblyType" runat="server" SelectedValue='<%# Bind("AssemblyType") %>'>
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem Value="A">Mass</asp:ListItem>
                            <asp:ListItem Value="P">Prototype</asp:ListItem>
                            <asp:ListItem Value="S">Service</asp:ListItem>
                            <asp:ListItem Value="T">Trial</asp:ListItem>
                            <asp:ListItem Value="0">N/A</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvAType" runat="server" ControlToValidate="ddAssemblyType"
                            Display="Dynamic" ErrorMessage="Is this Assembly Plant for Mass Production or Service?"
                            ValidationGroup="EditInfo"><</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblAType" runat="server" Text='<%# Bind("AssemblyTypeDesc") %>' />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddAssemblyTypeGV" runat="server">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem Value="A">Mass</asp:ListItem>
                            <asp:ListItem Value="P">Prototype</asp:ListItem>
                            <asp:ListItem Value="S">Service</asp:ListItem>
                            <asp:ListItem Value="T">Trial</asp:ListItem>
                            <asp:ListItem Value="0">N/A</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvAType" runat="server" ControlToValidate="ddAssemblyTypeGV"
                            Display="Dynamic" ErrorMessage="Is this Assembly Plant for Mass Production or Service?"
                            ValidationGroup="InsertInfo"><</asp:RequiredFieldValidator>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Obsolete" SortExpression="Obsolete" ItemStyle-HorizontalAlign="Center">
                    <EditItemTemplate>
                        <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Bind("Obsolete") %>' />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Bind("Obsolete") %>' Enabled="false" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:BoundField DataField="comboUpdateInfo" HeaderText="Last Updated By" ReadOnly="True"
                    SortExpression="comboUpdateInfo" HeaderStyle-HorizontalAlign="left">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
        &nbsp;&nbsp;
        <asp:ValidationSummary ID="vsEditInfo" runat="server" ShowMessageBox="True" Width="599px"
            ValidationGroup="EditInfo" Height="35px" />
        <asp:ValidationSummary ID="vsInsertInfo" runat="server" ShowMessageBox="True" Width="599px"
            ValidationGroup="InsertInfo" />
        <br />
        <asp:ValidationSummary ID="vsEmptyInfo" runat="server" ShowMessageBox="True" Width="599px"
            ValidationGroup="EmptyInfo" />
        <asp:ObjectDataSource ID="odsAPL" runat="server" SelectMethod="GetAssemblyPlantLocation"
            TypeName="AssemblyPlantLocationBLL" UpdateMethod="UpdateAssemblyPlantLocation"
            InsertMethod="InsertAssemblyPlantLocation" OldValuesParameterFormatString="original_{0}"
            DeleteMethod="DeleteAssemblyPlantLocation">
            <SelectParameters>
                <asp:Parameter Name="APID" Type="Int32" DefaultValue="0" />
                <asp:QueryStringParameter Name="Assembly" QueryStringField="sAPL" Type="String" DefaultValue="" />
                <asp:QueryStringParameter Name="Country" QueryStringField="sCtry" Type="String" />
                <asp:QueryStringParameter Name="OEMMfg" QueryStringField="sOMfg" Type="String" />
                <asp:Parameter DefaultValue="" Name="AssemblyType" Type="String" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="APID" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="APID" Type="Int32" />
                <asp:Parameter Name="Assembly_Plant_Location" Type="String" />
                <asp:Parameter Name="OEMManufacturer" Type="String" />
                <asp:Parameter Name="State" Type="String" />
                <asp:Parameter Name="Country" Type="String" />
                <asp:Parameter Name="UGNBusiness" Type="Boolean" />
                <asp:Parameter Name="AssemblyType" Type="String" />
                <asp:Parameter Name="IHS_Assembly_Plant" Type="String" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="Assembly_Plant_Location" Type="String" />
                <asp:Parameter Name="OEMManufacturer" Type="String" />
                <asp:Parameter Name="State" Type="String" />
                <asp:Parameter Name="Country" Type="String" />
                <asp:Parameter Name="UGNBusiness" Type="Boolean" />
                <asp:Parameter Name="AssemblyType" Type="String" />
                <asp:Parameter Name="IHS_Assembly_Plant" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
