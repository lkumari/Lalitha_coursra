<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Future_Part_Maint.aspx.vb" Inherits="PF_Future_PartNo" Title="UGNDB: Future Part"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="100%" DefaultButton="btnSearch">
        <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
            Width="568px" Visible="False"></asp:Label><br />
        <hr />
        <table>
            <tr>
                <td style="height: 15px" class="p_text">
                    Part Number:
                </td>
                <td style="width: 108px; height: 15px;">
                    <asp:TextBox ID="txtPartNo" runat="server" MaxLength="20" Width="150px"></asp:TextBox>
                </td>
                <td style="height: 15px" class="p_text">
                    Part Description:
                </td>
                <td style="width: 108px; height: 15px;">
                    <asp:TextBox ID="txtPartDesc" runat="server" MaxLength="50" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="height: 15px" class="p_text">
                    Team Member:
                </td>
                <td style="width: 108px; height: 15px">
                    <asp:DropDownList ID="ddTeamMember" runat="server">
                    </asp:DropDownList>
                </td>
                <td style="height: 15px">
                </td>
                <td style="width: 108px; height: 15px">
                </td>
            </tr>
            <tr>
                <td colspan="4" style="height: 15px" align="center">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" />
                </td>
            </tr>
        </table>
        <hr />
        <asp:GridView ID="gvFuturePartNo" runat="server" AutoGenerateColumns="False" DataKeyNames="PartNo"
            ShowFooter="True" DataSourceID="odsFuturePartNo" AllowSorting="True" AllowPaging="True"
            Width="650px" OnRowCommand="gvFuturePartNo_RowCommand" OnDataBound="gvFuturePartNo_DataBound"
            PageSize="20">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <EmptyDataTemplate>
                No records found from the database.
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <ItemStyle HorizontalAlign="Left" Wrap="false" />
                    <EditItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True"
                            CommandName="Update" ImageUrl="~/images/save.jpg" Text="Update" AlternateText="Update"
                            ValidationGroup="EditPartInfo" />&nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ibtnCancel"
                                runat="server" CausesValidation="False" CommandName="Cancel" ImageUrl="~/images/cancel.jpg"
                                Text="Cancel" AlternateText="Cancel" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False"
                            CommandName="Edit" ImageUrl="~/images/edit.jpg" Text="Edit" AlternateText="Edit" />
                    </ItemTemplate>
                    <FooterTemplate>
                        &nbsp;&nbsp;<asp:ImageButton Text="Insert" CommandName="Insert" CausesValidation="true"
                            runat="server" ID="ibtnInsert" ImageUrl="~/images/save.jpg" AlternateText="Insert"
                            ValidationGroup="InsertPartInfo" />&nbsp;<asp:ImageButton ID="ibtnUndo" runat="server"
                                CommandName="Undo" ImageUrl="~/images/undo-gray.jpg" Text="Undo" AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Part Number" SortExpression="PartNo">
                    <EditItemTemplate>
                        &nbsp;<asp:TextBox ID="txtPartNo" runat="server" Text='<%# Bind("PartNo") %>' Width="150px"
                            MaxLength="20" />
                        <asp:RequiredFieldValidator ID="rfvPartNo" runat="server" ControlToValidate="txtPartNo"
                            ErrorMessage="Part Number is a required field." Font-Bold="True" Font-Italic="False"
                            ValidationGroup="EditPartInfo"><</asp:RequiredFieldValidator>
                        <ajax:FilteredTextBoxExtender ID="ftbPartNo" runat="server" TargetControlID="txtPartNo"
                            FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,- " />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblPartNo" runat="server" Text='<%# Bind("PartNo") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtPartNo" runat="server" Width="150px" MaxLength="20" />
                        <asp:RequiredFieldValidator ID="rfvPartNo" runat="server" ControlToValidate="txtPartNo"
                            ErrorMessage="Part Number is a required field." Font-Bold="True" Font-Italic="False"
                            ValidationGroup="InsertPartInfo"><</asp:RequiredFieldValidator>
                        <ajax:FilteredTextBoxExtender ID="ftbPartNo" runat="server" TargetControlID="txtPartNo"
                            FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,- " />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Part Description" SortExpression="PartDesc">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtPartDesc" runat="server" Text='<%# Bind("PartDesc") %>' MaxLength="50"
                            Width="200px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvPartDesc" runat="server" ControlToValidate="txtPartDesc"
                            ErrorMessage="Part Description is a required field." Font-Bold="True" Font-Italic="False"
                            ValidationGroup="EditPartInfo"><</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblPartDesc" runat="server" Text='<%# Bind("PartDesc") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtPartDesc" runat="server" Width="200px" MaxLength="50" />
                        <asp:RequiredFieldValidator ID="rfvPartDesc" runat="server" ControlToValidate="txtPartDesc"
                            ErrorMessage="Part Description is a required field." Font-Bold="True" Font-Italic="False"
                            ValidationGroup="InsertPartInfo"><</asp:RequiredFieldValidator>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Designation Type" SortExpression="DesignationType">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddDesignationType1" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvDesignationType1" runat="server" ControlToValidate="ddDesignationType1"
                            ErrorMessage="Designation Type is a required field." Font-Bold="True" Font-Italic="False"
                            ValidationGroup="EditPartInfo"><</asp:RequiredFieldValidator>
                        <ajax:CascadingDropDown ID="cddDesignationType1" runat="server" SelectedValue='<%# Bind("DesignationType") %>'
                            TargetControlID="ddDesignationType1" Category="DesignationType" PromptText="Select a Part Designation Type."
                            LoadingText="[Loading Part Designation Types...]" ServicePath="~/WS/GeneralCDDService.asmx"
                            ServiceMethod="GetDesignationTypes" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblDesignationType" runat="server" Text='<%# Bind("DesignationTypeDesc") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddDesignationType" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvDesignationType" runat="server" ControlToValidate="ddDesignationType"
                            ErrorMessage="Designation Type is a required field." Font-Bold="True" Font-Italic="False"
                            ValidationGroup="InsertPartInfo"><</asp:RequiredFieldValidator>
                        <ajax:CascadingDropDown ID="cddDesignationType" runat="server" TargetControlID="ddDesignationType"
                            SelectedValue="C" Category="DesignationType" PromptText="Select a Part Designation Type."
                            LoadingText="[Loading Part Designation Types...]" ServicePath="~/WS/GeneralCDDService.asmx"
                            ServiceMethod="GetDesignationTypes" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="UGNFacility Name" SortExpression="UGNFacilityName">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddUGNLocation1" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvUGNLocation1" runat="server" ControlToValidate="ddUGNLocation1"
                            ErrorMessage="UGN Facility is a required field." Font-Bold="True" Font-Italic="False"
                            ValidationGroup="EditPartInfo"><</asp:RequiredFieldValidator>
                        <ajax:CascadingDropDown ID="cddUGNLocation1" runat="server" SelectedValue='<%# Bind("UGNFacility") %>'
                            TargetControlID="ddUGNLocation1" ParentControlID="ddDesignationType1" Category="UGNLocation"
                            PromptText="Select a UGN Location" LoadingText="[Loading UGN Location...]" ServicePath="~/WS/GeneralCDDService.asmx"
                            ServiceMethod="GetUGNLocation" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblUGNLocation" runat="server" Text='<%# Bind("UGNFacilityName") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddUGNLocation" runat="server" />
                        <ajax:CascadingDropDown ID="cddUGNLocation" runat="server" TargetControlID="ddUGNLocation"
                            ParentControlID="ddDesignationType" Category="UGNLocation" PromptText="Select a UGN Location"
                            LoadingText="[Loading UGN Location...]" ServicePath="~/WS/GeneralCDDService.asmx"
                            ServiceMethod="GetUGNLocation" />
                        <asp:RequiredFieldValidator ID="rfvUGNLocation" runat="server" ControlToValidate="ddUGNLocation"
                            ErrorMessage="UGN Facility is a required field." Font-Bold="True" Font-Italic="False"
                            ValidationGroup="InsertPartInfo"><</asp:RequiredFieldValidator>
                    </FooterTemplate>
                </asp:TemplateField>
                <%-- <asp:TemplateField HeaderText="OEM" SortExpression="OEM">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddOEM1" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvOEM1" runat="server" ControlToValidate="ddOEM1"
                            ErrorMessage="OEM Code is a required field." Font-Bold="True" Font-Italic="False"
                            ValidationGroup="EditPartInfo"><</asp:RequiredFieldValidator>
                        <ajax:CascadingDropDown ID="cddOEM1" runat="server" SelectedValue='<%# Bind("OEM") %>'
                            TargetControlID="ddOEM1" ParentControlID="ddUGNLocation1" Category="OEM" PromptText="Select an OEM Code."
                            LoadingText="[Loading OEM Codes...]" ServicePath="~/WS/GeneralCDDService.asmx"
                            ServiceMethod="GetOEMbyCOMPNY" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblOEM" runat="server" Text='<%# Bind("OEM") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddOEM" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvOEM" runat="server" ControlToValidate="ddOEM"
                            ErrorMessage="OEM Code is a required field." Font-Bold="True" Font-Italic="False"
                            ValidationGroup="InsertPartInfo"><</asp:RequiredFieldValidator>
                        <ajax:CascadingDropDown ID="cddOEM" runat="server" TargetControlID="ddOEM" ParentControlID="ddUGNLocation"
                            Category="OEM" PromptText="Select an OEM Code." LoadingText="[Loading OEM Codes...]"
                            ServicePath="~/WS/GeneralCDDService.asmx" ServiceMethod="GetOEMbyCOMPNY" />
                    </FooterTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="OEM Manufacturer" SortExpression="OEMManufacturer">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddOEMMfg1" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvOEMMfg1" runat="server" ControlToValidate="ddOEMMfg1"
                            ErrorMessage="OEM Manufacturer is a required field." Font-Bold="True" Font-Italic="False"
                            ValidationGroup="EditPartInfo"><</asp:RequiredFieldValidator>
                        <ajax:CascadingDropDown ID="cddOEMMfg1" runat="server" SelectedValue='<%# Bind("OEMManufacturer") %>'
                            TargetControlID="ddOEMMfg1" Category="OEMMfg"
                            PromptText="Select an OEM Manufacturer" LoadingText="[Loading OEM Manufacturer...]"
                            ServicePath="~/WS/VehicleCDDService.asmx" ServiceMethod="GetOEMMfg" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblOEMMfg" runat="server" Text='<%# Bind("OEMManufacturer") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddOEMMfg" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvOEMMfg" runat="server" ControlToValidate="ddOEMMfg"
                            ErrorMessage="OEM Manufacturer is a required field." Font-Bold="True" Font-Italic="False"
                            ValidationGroup="InsertPartInfo"><</asp:RequiredFieldValidator>
                        <ajax:CascadingDropDown ID="cddOEMMfg" runat="server" TargetControlID="ddOEMMfg"
                            Category="OEMMfg" PromptText="Select an OEM Manufacturer"
                            LoadingText="[Loading OEM Manufacturer...]" ServicePath="~/WS/VehicleCDDService.asmx"
                            ServiceMethod="GetOEMMfg" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:CheckBoxField DataField="Obsolete" HeaderText="Obsolete" SortExpression="Obsolete">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:CheckBoxField>
                <asp:TemplateField HeaderText="Last Update" SortExpression="comboUpdateInfo">
                    <EditItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("comboUpdateInfo") %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("comboUpdateInfo") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsFuturePartNo" runat="server" InsertMethod="InsertFuturePartNo"
            OldValuesParameterFormatString="original_{0}" SelectMethod="GetFuturePartNo"
            TypeName="Future_PartNoBLL" UpdateMethod="UpdateFuturePartNo">
            <UpdateParameters>
                <asp:Parameter Name="PartNo" Type="String" />
                <asp:Parameter Name="PartDesc" Type="String" />
                <asp:Parameter Name="Obsolete" Type="Boolean" />
                <asp:Parameter Name="UGNFacility" Type="String" />
                <asp:Parameter Name="OEM" Type="String" />
                <asp:Parameter Name="OEMManufacturer" Type="String" />
                <asp:Parameter Name="DesignationType" Type="String" />
                <asp:Parameter Name="original_PartNo" Type="String" />
                <asp:Parameter Name="orignal_UGNFacility" Type="String" />
                <asp:Parameter Name="original_OEM" Type="String" />
                <asp:Parameter Name="original_OEMManufacturer" Type="String" />
                <asp:Parameter Name="original_DesignationType" Type="String" />
            </UpdateParameters>
            <SelectParameters>
                <asp:CookieParameter CookieName="PF_txtPartNo" Name="PartNo" Type="String" />
                <asp:CookieParameter CookieName="PF_txtPartDesc" Name="PartDesc" Type="String" />
                <asp:CookieParameter CookieName="PF_ddTeamMember" Name="CreatedBy" Type="String" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="PartNo" Type="String" />
                <asp:Parameter Name="PartDesc" Type="String" />
                <asp:Parameter Name="UGNFacility" Type="String" />
                <asp:Parameter Name="OEM" Type="String" />
                <asp:Parameter Name="OEMManufacturer" Type="String" />
                <asp:Parameter Name="DesignationType" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
        <asp:ValidationSummary ID="EditPartInfo" runat="server" ShowMessageBox="True" ValidationGroup="EditPartInfo" />
        &nbsp;<asp:ValidationSummary ID="InsertPartInfo" runat="server" ShowMessageBox="True"
            ValidationGroup="InsertPartInfo" />
    </asp:Panel>
</asp:Content>
