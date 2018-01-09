<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="PlatformMaintenance.aspx.vb" Inherits="DataMaintenance_PlatformMaintenance"
    Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <hr />
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <br />
        <asp:Label ID="lblMessage" runat="server" Width="818px" SkinID="MessageLabelSkin"></asp:Label>
        <table>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblPlatform" runat="server" Text="Platform Name:" />
                </td>
                <td>
                    <asp:TextBox ID="txtPlatformName" runat="server" MaxLength="30" Width="300px" />
                    <ajax:FilteredTextBoxExtender ID="ftbPName" runat="server" TargetControlID="txtPlatformName"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,%/- " />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblOEMManufacturer" runat="server" Text="OEM Manufacturer:" />
                </td>
                <td>
                    <asp:TextBox ID="txtOEMManufacturer" runat="server" MaxLength="30" Width="300px" />
                    <ajax:FilteredTextBoxExtender ID="ftpOEM" runat="server" TargetControlID="txtOEMManufacturer"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,%/- " />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblUGNBusiness" runat="server" Text="UGN Business:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddDispUGNBusiness" runat="server">
                        <asp:ListItem Selected="True" Value=""></asp:ListItem>
                        <asp:ListItem Value="No">No</asp:ListItem>
                        <asp:ListItem Value="Yes">Yes</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="p_text">
                    <asp:Label ID="lblCurrentPlatform" runat="server" Text="Current Platform:"/>
                </td>
                <td>
                    <asp:DropDownList ID="ddDispCurrentPlatform" runat="server">
                        <asp:ListItem Selected="True" Value=""></asp:ListItem>
                        <asp:ListItem Value="No">No</asp:ListItem>
                        <asp:ListItem Value="Yes">Yes</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="5">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" />
                </td>
            </tr>
        </table>
        <hr />
        <%--        <div class="showHideColumnsContainer">
            <asp:DropDownList ID="gvPlatformListShowHideColumns" runat="server" Visible="false"
                AutoPostBack="true" OnSelectedIndexChanged="gvPlatformListShowHideColumns_SelectedIndexChanged" />
        </div>
        <br />
--%>
        <asp:Label ID="Label2" runat="server"><i>Double astericks (**) at the end of each column heading denotes a required field.</i></asp:Label>
        <br />
        <asp:GridView ID="gvPlatformList" runat="server" AutoGenerateColumns="False" DataKeyNames="PlatformID,PlatformName,OEMManufacturer"
            OnRowCommand="gvPlatformList_RowCommand" AllowSorting="True" ShowFooter="True"
            DataSourceID="odsPlatformList" AllowPaging="True" PageSize="30" CssClass="c_smalltext"
            EmptyDataRowStyle-Font-Size="Medium" EmptyDataRowStyle-Font-Bold="true" EmptyDataRowStyle-ForeColor="Red"
            SkinID="StandardGrid">
            <EmptyDataTemplate>
                No records found for the combination above.
            </EmptyDataTemplate>
            <EmptyDataRowStyle Font-Bold="True" Font-Size="Medium" ForeColor="Red" />
            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <EditItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True"
                            CommandName="Update" ValidationGroup="EditPlatformInfo" ImageUrl="~/images/save.jpg"
                            AlternateText="Update" />&nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ibtnCancel" runat="server"
                                CausesValidation="False" CommandName="Cancel" ImageUrl="~/images/cancel.jpg"
                                AlternateText="Cancel" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False"
                            CommandName="Edit" ImageUrl="~/images/edit.jpg" AlternateText="Edit" />&nbsp;&nbsp;&nbsp;
                    </ItemTemplate>
                    <FooterTemplate>
                        &nbsp;&nbsp;<asp:ImageButton CommandName="Insert" CausesValidation="true" runat="server"
                            ID="ibtnInsert" ImageUrl="~/images/save.jpg" AlternateText="Insert" ValidationGroup="InsertPlatformInfo" />
                        &nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo"
                            ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Programs">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" ID="hlnkPreview" ImageUrl='<%# DisplayImage(DataBinder.Eval(Container.DataItem, "OEMManufacturer"))  %>'
                            ToolTip="Preview/Insert/Edit Programs" NavigateUrl='<%# "ProgramMaintenance.aspx?pPlatID=" & DataBinder.Eval (Container.DataItem,"PlatformID").tostring & "&sPName=" & txtPlatformName.Text & "&sOEMMF=" & txtOEMManufacturer.Text & "&sDUB=" & ddDispUGNBusiness.SelectedValue & "&sDCP=" & ddDispCurrentPlatform.SelectedValue%>' />
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="center" />
                </asp:TemplateField>
                <asp:BoundField DataField="PlatformID" HeaderText="Platform ID" ReadOnly="True" SortExpression="PlatformID"
                    HeaderStyle-Width="40px" HeaderStyle-Wrap="true" ItemStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" Visible="true">
                    <HeaderStyle HorizontalAlign="Center" Width="40px" Wrap="True" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
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
                            Display="Dynamic" ErrorMessage="OEMManufacturer is a required field." ValidationGroup="EditPlatformInfo"><</asp:RequiredFieldValidator>
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
                            Display="Dynamic" ErrorMessage="OEM Manufacturer is a required field." ValidationGroup="InsertPlatformInfo"><</asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Platform Name **" SortExpression="PlatformName">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtPlatformName" runat="server" Text='<%# Bind("PlatformName") %>'
                            MaxLength="30" Width="100px" />
                        <asp:RequiredFieldValidator ID="rfvPlatformName" runat="server" ControlToValidate="txtPlatformName"
                            Display="Dynamic" ErrorMessage="Platform Name is a required field." ValidationGroup="EditPlatformInfo"><</asp:RequiredFieldValidator>
                        <ajax:FilteredTextBoxExtender ID="ftbPName" runat="server" TargetControlID="txtPlatformName"
                            FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,()/- " />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblPlatformName" runat="server" Text='<%# Bind("PlatformName") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <FooterTemplate>
                        <asp:TextBox ID="txtPlatformNameGV" runat="server" MaxLength="30" Width="100px" />
                        <asp:RequiredFieldValidator ID="rfvPlatformNameGV" runat="server" ControlToValidate="txtPlatformNameGV"
                            Display="Dynamic" ErrorMessage="Platform Name is a required field." ValidationGroup="InsertPlatformInfo"><</asp:RequiredFieldValidator>
                        <ajax:FilteredTextBoxExtender ID="ftbPName" runat="server" TargetControlID="txtPlatformNameGV"
                            FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,()/- " />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Beg Year **" SortExpression="BegYear">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtBegYear" runat="server" Text='<%# Bind("BegYear") %>' MaxLength="4"
                            Width="60px" />
                        <ajax:FilteredTextBoxExtender ID="fbtBegYear" runat="server" TargetControlID="txtBegYear"
                            FilterType="Numbers" />
                        <asp:RequiredFieldValidator ID="rfvBegYear" runat="server" ControlToValidate="txtBegYear"
                            Display="Dynamic" ErrorMessage="Beg Year is a required field." ValidationGroup="EditPlatformInfo"><</asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rvBegYear" runat="server" ErrorMessage="Beg Year values must be between 1997 to 2030"
                            ControlToValidate="txtBegYear" MinimumValue="1997" MaximumValue="2030" ValidationGroup="EditPlatformInfo"><</asp:RangeValidator>
                        <asp:CompareValidator ID="cvBegYear" runat="server" ErrorMessage="Beg Year must be less than or = End Year."
                            ControlToCompare="txtEndYear" ControlToValidate="txtBegYear" Operator="LessThanEqual"
                            Type="Integer" ValidationGroup="EditPlatformInfo"><</asp:CompareValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblBegYear" runat="server" Text='<%# Bind("BegYear") %>' />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtBegYearGV" runat="server" MaxLength="4" Width="60px" />
                        <ajax:FilteredTextBoxExtender ID="ftbBegYearGV" runat="server" TargetControlID="txtBegYearGV"
                            FilterType="Numbers" />
                        <asp:RequiredFieldValidator ID="rfvBegYearGV" runat="server" ControlToValidate="txtBegYearGV"
                            Display="Dynamic" ErrorMessage="Beg Year is a required field." ValidationGroup="InsertPlatformInfo"><</asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rvBegYear" runat="server" ErrorMessage="Beg Year values must be between 1997 to 2030"
                            ControlToValidate="txtBegYearGV" MinimumValue="1997" MaximumValue="2030" ValidationGroup="InsertPlatformInfo"><</asp:RangeValidator>
                        <asp:CompareValidator ID="cvBegYear" runat="server" ErrorMessage="Beg Year must be less than or = End Year."
                            ControlToCompare="txtEndYearGV" ControlToValidate="txtBegYearGV" Operator="LessThanEqual"
                            Type="Integer" ValidationGroup="InsertPlatformInfo"><</asp:CompareValidator>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="End Year **" SortExpression="EndYear">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEndYear" runat="server" Text='<%# Bind("EndYear") %>' MaxLength="4"
                            Width="60px" />
                        <ajax:FilteredTextBoxExtender ID="ftEndYear" runat="server" TargetControlID="txtEndYear"
                            FilterType="Numbers" />
                        <asp:RequiredFieldValidator ID="rfvEndYear" runat="server" ControlToValidate="txtEndYear"
                            Display="Dynamic" ErrorMessage="Ending Year is a required field." ValidationGroup="EditPlatformInfo"><</asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rvEndYear" runat="server" ErrorMessage="End Year values must be between 1997 to 2030"
                            ControlToValidate="txtEndYear" MinimumValue="1997" MaximumValue="2030" ValidationGroup="EditPlatformInfo"><</asp:RangeValidator>
                        <asp:CompareValidator ID="cvEndYear" runat="server" ControlToCompare="txtBegYear"
                            ControlToValidate="txtEndYear" ErrorMessage="End Year must be greater than or = Beg Year."
                            Operator="GreaterThanEqual" Type="Integer" ValidationGroup="EditPlatformInfo"><</asp:CompareValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblEndYear" runat="server" Text='<%# Bind("EndYear") %>' MaxLength="4"
                            Width="60px" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtEndYearGV" runat="server" MaxLength="4" Width="60px" />
                        <ajax:FilteredTextBoxExtender ID="ftEndYearGV" runat="server" TargetControlID="txtEndYearGV"
                            FilterType="Numbers" />
                        <asp:RequiredFieldValidator ID="rfvEndYearGV" runat="server" ControlToValidate="txtEndYearGV"
                            Display="Dynamic" ErrorMessage="Ending Year is a required field." ValidationGroup="InsertPlatformInfo"><</asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rvEndYear" runat="server" ErrorMessage="End Year values must be between 1997 to 2030"
                            ControlToValidate="txtEndYearGV" MinimumValue="1997" MaximumValue="2030" ValidationGroup="InsertPlatformInfo"><</asp:RangeValidator>
                        <asp:CompareValidator ID="cvEndYear" runat="server" ControlToCompare="txtBegYearGV"
                            ControlToValidate="txtEndYearGV" ErrorMessage="End Year must be greater than or = Beg Year."
                            Operator="GreaterThanEqual" Type="Integer" ValidationGroup="InsertPlatformInfo"><</asp:CompareValidator>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="# of Service Years" SortExpression="ServiceYears"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true"
                    HeaderStyle-Width="50px">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtSrvYrs" runat="server" MaxLength="2" Width="30px" Text='<%# Bind("ServiceYears") %>' />
                        <ajax:FilteredTextBoxExtender ID="ftSrvYrs" runat="server" TargetControlID="txtSrvYrs"
                            FilterType="Numbers" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("ServiceYears") %>' />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtSrvYrsGV" runat="server" MaxLength="2" Width="30px" />
                        <ajax:FilteredTextBoxExtender ID="ftSrvYrsGV" runat="server" TargetControlID="txtSrvYrsGV"
                            FilterType="Numbers" />
                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Center" Width="50px" Wrap="True" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="UGN Business **" SortExpression="UGNBusiness" HeaderStyle-Wrap="true"
                    HeaderStyle-Width="60px">
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
                    <HeaderStyle Width="60px" Wrap="True" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Current Platform **" SortExpression="CurrentPlatform"
                    HeaderStyle-Wrap="true" HeaderStyle-Width="60px">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddCurrentPlatform" runat="server" SelectedValue='<%# Bind("CurrentPlatform") %>'>
                            <asp:ListItem Selected="True" Value="False">No</asp:ListItem>
                            <asp:ListItem Value="True">Yes</asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblCurrPlat" runat="server" Text='<%# Bind("CurrentPlatformDisplay") %>' />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddCurrentPlatformGV" runat="server" SelectedValue='<%# Bind("CurrentPlatform") %>'>
                            <asp:ListItem Selected="True" Value="False">No</asp:ListItem>
                            <asp:ListItem Value="True">Yes</asp:ListItem>
                        </asp:DropDownList>
                    </FooterTemplate>
                    <HeaderStyle Width="60px" Wrap="True" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Notes" SortExpression="Notes">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtNotes" runat="server" Text='<%# Bind("Notes") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblnotes" runat="server" Text='<%# Bind("Notes") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtNotesGV" runat="server" Text='<%# Bind("Notes") %>'></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="comboUpdateInfo" HeaderText="Last Update" ReadOnly="True"
                    HeaderStyle-Width="80px" HeaderStyle-Wrap="true" SortExpression="comboUpdateInfo">
                    <HeaderStyle HorizontalAlign="Left" Width="80px" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
        <br />
        <asp:ValidationSummary ID="vsEditPlatformInfo" runat="server" ShowMessageBox="True"
            ShowSummary="True" Width="498px" ValidationGroup="EditPlatformInfo" />
        <asp:ValidationSummary ID="vsInsertPlatformInfo" runat="server" ShowMessageBox="True"
            ShowSummary="True" Width="498px" ValidationGroup="InsertPlatformInfo" />
        <asp:ObjectDataSource ID="odsPlatformList" runat="server" SelectMethod="GetPlatform"
            TypeName="PlatformBLL" UpdateMethod="UpdatePlatform" InsertMethod="InsertPlatform"
            OldValuesParameterFormatString="original_{0}" DeleteMethod="DeleteProgramVolume">
            <SelectParameters>
                <asp:Parameter DefaultValue="0" Name="PlatformID" Type="Int32" />
                <asp:QueryStringParameter Name="PlatformName" QueryStringField="sPName" Type="String" />
                <asp:QueryStringParameter Name="OEMManufacturer" QueryStringField="sOEMMF" Type="String" />
                <asp:QueryStringParameter Name="DisplayUGNBusiness" QueryStringField="sDUB" Type="String" />
                <asp:QueryStringParameter Name="DisplayCurrentPlatform" QueryStringField="sDCP" Type="String" />
                <asp:Parameter DefaultValue="" Name="SortBy" Type="String" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="ProgramID" Type="Int32" />
                <asp:Parameter Name="YearID" Type="Int32" />
                <asp:Parameter Name="original_ProgramID" Type="Int32" />
                <asp:Parameter Name="original_YearID" Type="String" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="PlatformName" Type="String" />
                <asp:Parameter Name="OEMManufacturer" Type="String" />
                <asp:Parameter Name="BegYear" Type="Int32" />
                <asp:Parameter Name="EndYear" Type="Int32" />
                <asp:Parameter Name="obsolete" Type="Boolean" />
                <asp:Parameter Name="UGNBusiness" Type="Boolean" />
                <asp:Parameter Name="CurrentPlatform" Type="Boolean" />
                <asp:Parameter Name="ServiceYears" Type="Int32" />
                <asp:Parameter Name="Notes" Type="String" />
                <asp:Parameter Name="original_PlatformID" Type="Int32" />
                <asp:Parameter Name="original_PlatformName" Type="String" />
                <asp:Parameter Name="original_OEMManufacturer" Type="String" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="PlatformName" Type="String" />
                <asp:Parameter Name="OEMManufacturer" Type="String" />
                <asp:Parameter Name="BegYear" Type="Int32" />
                <asp:Parameter Name="EndYear" Type="Int32" />
                <asp:Parameter Name="UGNBusiness" Type="Boolean" />
                <asp:Parameter Name="CurrentPlatform" Type="Boolean" />
                <asp:Parameter Name="ServiceYears" Type="Int32" />
                <asp:Parameter Name="Notes" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
