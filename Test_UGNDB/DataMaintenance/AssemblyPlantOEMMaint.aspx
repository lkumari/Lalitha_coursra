<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="AssemblyPlantOEMMaint.aspx.vb" Inherits="DataMaintenance_AssemblyPlantOEMMaint"
    MaintainScrollPositionOnPostback="True" Title="UGNDB: Assembly Plant OEM" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server">
        <hr />
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
        <table style="width: 90%; border-bottom-style: groove;" class="sampleStyleC">
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblAssemblyPlantLocation" runat="server" Text="Assembly Plant Location:"/>&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblAssembly" runat="server" Text="" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblState" runat="server" Text="State:"/>&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblStateVal" runat="server" Text="" />
                </td>
                <td class="p_text">
                     <asp:Label ID="lblCountry" runat="server" Text="Country:"/>&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblCountryVal" runat="server" Text="" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblOEMManufacturer" runat="server" Text="OEM Manufacturer:"/>&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblOEMManufacturerVal" runat="server" Text="" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblUGNBusiness" runat="server" Text="UGN Business:"/>&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblUGNBiz" runat="server" Text="" />
                </td>
            </tr>
        </table>
        <br />
        <hr />
        <asp:Label ID="lblRaiseError" runat="server" Text="" Visible="false" SkinID="MessageLabelSkin" />
        <br />
        <asp:Label ID="Label2" runat="server"><i>Double astericks (**) at the end of each column
            heading denotes a required field.</i></asp:Label>
        <br />
        <%-- OnRowUpdating="gvAPLOEM_RowUpdating"--%>
        <asp:GridView ID="gvAPLOEM" runat="server" AutoGenerateColumns="False" ShowFooter="True"
            DataKeyNames="APID,OEMModelType,Make,ModelName" DataSourceID="odsAPL" AllowPaging="True"
            OnRowDataBound="gvAPLOEM_RowDataBound" OnRowCommand="gvAPLOEM_RowCommand" OnRowUpdating="gvAPLOEM_RowUpdating"
            AllowSorting="True" PageSize="30" EmptyDataRowStyle-Font-Size="Medium" EmptyDataRowStyle-Font-Bold="true"
            EmptyDataRowStyle-ForeColor="Red" OnRowDeleted="gvAPLOEM_RowDeleted">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <EmptyDataTemplate>
                No records found for the combination above.
            </EmptyDataTemplate>
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
                <asp:BoundField DataField="APID" HeaderText="APID" SortExpression="APID" ReadOnly="true" />
                <asp:TemplateField HeaderText="OEM Model Type **" SortExpression="OEMModelType" HeaderStyle-HorizontalAlign="Left">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtOEMModelType1" runat="server" MaxLength="10" Width="80px" Text='<%# Bind("OEMModelType") %>' />
                        <asp:RequiredFieldValidator ID="rfvOEMModelType1" runat="server" ControlToValidate="txtOEMModelType1"
                            Display="Dynamic" ErrorMessage="OEM Model Type is a required field." ValidationGroup="EditInfo"><</asp:RequiredFieldValidator>
                        <ajax:FilteredTextBoxExtender ID="ftbOEMModelType1" runat="server" TargetControlID="txtOEMModelType1"
                            FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,- " />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("OEMModelType") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtOEMModelType" runat="server" MaxLength="10" Width="80px" />
                        <asp:RequiredFieldValidator ID="rfvOEMModelType" runat="server" ControlToValidate="txtOEMModelType"
                            Display="Dynamic" ErrorMessage="OEM Model Type is a required field." ValidationGroup="InsertInfo"><</asp:RequiredFieldValidator>
                        <ajax:FilteredTextBoxExtender ID="ftbOEMModelType" runat="server" TargetControlID="txtOEMModelType"
                            FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,- " />
                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Make **" SortExpression="Make">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddMake1" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvMake11" runat="server" ControlToValidate="ddMake1"
                            Display="Dynamic" ErrorMessage="Make is a required field." ValidationGroup="EditInfo"><</asp:RequiredFieldValidator>
                        <ajax:CascadingDropDown ID="cddMakes1" runat="server" TargetControlID="ddMake1" Category="Make"
                            SelectedValue='<%# Bind("Make") %>' PromptText="Please select a Make." LoadingText="[Loading Makes...]"
                            ServicePath="~/WS/VehicleCDDService.asmx" ServiceMethod="GetMakes" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblMake" runat="server" Text='<%# Bind("Make") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <FooterTemplate>
                        <asp:DropDownList ID="ddMake" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvMake11" runat="server" ControlToValidate="ddMake"
                            Display="Dynamic" ErrorMessage="Make is a required field." ValidationGroup="InsertInfo"><</asp:RequiredFieldValidator>
                        <ajax:CascadingDropDown ID="cddMakes" runat="server" TargetControlID="ddMake" Category="Make"
                            PromptText="Please select a Make." LoadingText="[Loading Makes...]" ServicePath="~/WS/VehicleCDDService.asmx"
                            ServiceMethod="GetMakes" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Model **" SortExpression="ModelName">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddModel1" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvModel1" runat="server" ControlToValidate="ddModel1"
                            Display="Dynamic" ErrorMessage="Model is a required field." ValidationGroup="EditInfo"><</asp:RequiredFieldValidator>
                        <ajax:CascadingDropDown ID="cddModel1" runat="server" TargetControlID="ddModel1"
                            ParentControlID="ddMake1" SelectedValue='<%# Bind("ModelName") %>' Category="Model"
                            PromptText="Please select a Model." LoadingText="[Loading Models...]" ServicePath="~/WS/VehicleCDDService.asmx"
                            ServiceMethod="GetModelMaint" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblModel" runat="server" Text='<%# Bind("ddModelName") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <FooterTemplate>
                        <asp:DropDownList ID="ddModel" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvModel" runat="server" ControlToValidate="ddModel"
                            Display="Dynamic" ErrorMessage="Model is a required field." ValidationGroup="InsertInfo"><</asp:RequiredFieldValidator>
                        <ajax:CascadingDropDown ID="cddModel" runat="server" TargetControlID="ddModel" ParentControlID="ddMake"
                            Category="Model" PromptText="Please select a Model." LoadingText="[Loading Models...]"
                            ServicePath="~/WS/VehicleCDDService.asmx" ServiceMethod="GetModelMaint" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Platform **" SortExpression="PlatformID">
                    <EditItemTemplate>
                       <asp:DropDownList ID="ddPlatform1" runat="server" DataSource='<%# commonFunctions.GetPlatform(0,"",lblOEMManufacturer.text,"","","PlatformName") %>'
                            DataValueField="PlatformID" DataTextField="ddPlatformName" AppendDataBoundItems="True"
                            SelectedValue='<%# Bind("PlatformID") %>'>
                            <asp:ListItem Selected="True" Value="" Text="Select a Platform">
                            </asp:ListItem>
                        </asp:DropDownList>
                         <asp:RequiredFieldValidator ID="rfvPlatform1" runat="server" ControlToValidate="ddPlatform1"
                            Display="Dynamic" ErrorMessage="Platform is a required field." ValidationGroup="EditInfo"><</asp:RequiredFieldValidator>
                        <%--  <asp:DropDownList ID="ddPlatform1" runat="server" />
                        <ajax:CascadingDropDown ID="cddPlatform" runat="server" TargetControlID="ddPlatform1"
                            Category="Platform" PromptText="Please select a Platform." LoadingText="[Loading Platform...]"
                            ServicePath="~/WS/VehicleCDDService.asmx" ServiceMethod="GetPlatform" />--%>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblPlatformName" runat="server" Text='<%# Bind("ddPlatformName") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="left" />
                    <ItemStyle HorizontalAlign="left" />
                    <FooterTemplate>
                       <asp:DropDownList ID="ddPlatform" runat="server" DataSource='<%# commonFunctions.GetPlatform(0,"",lblOEMManufacturer.text,"","","PlatformName") %>'
                            DataValueField="PlatformID" DataTextField="ddPlatformName" SelectedValue='<%# Bind("PlatformID") %>'
                            AppendDataBoundItems="True">
                            <asp:ListItem Selected="True" Value="" Text="">
                            </asp:ListItem>
                        </asp:DropDownList>
                        <%-- <asp:DropDownList ID="ddPlatform" runat="server" />
                        <ajax:CascadingDropDown ID="cddPlatform" runat="server" TargetControlID="ddPlatform"
                            Category="Platform" PromptText="Please select a Platform." LoadingText="[Loading Platform...]"
                            ServicePath="~/WS/VehicleCDDService.asmx" ServiceMethod="GetPlatform" />--%>
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
        <asp:ValidationSummary ID="vsEmptyInfo" runat="server" ShowMessageBox="True" Width="599px"
            ValidationGroup="EmptyInfo" />
        <asp:ObjectDataSource ID="odsAPL" runat="server" SelectMethod="GetAssemblyPlantOEM"
            TypeName="AssemblyPlantOEMBLL" UpdateMethod="UpdateAssemblyPlantOEM" InsertMethod="InsertAssemblyPlantOEM"
            DeleteMethod="DeleteAssemblyPlantOEM" OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:QueryStringParameter DefaultValue="0" Name="APID" QueryStringField="pAPID" 
                    Type="Int32" />
                <asp:QueryStringParameter Name="ModelName" QueryStringField="pMName" 
                    Type="String" DefaultValue="" />
                <asp:Parameter Name="PlatformID" Type="Int32" DefaultValue="0" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="APID" Type="Int32" />
                <asp:Parameter Name="OEMModelType" Type="String" />
                <asp:Parameter Name="Make" Type="String" />
                <asp:Parameter Name="ModelName" Type="String" />
                <asp:Parameter Name="original_APID" Type="Int32" />
                <asp:Parameter Name="original_OEMModelType" Type="String" />
                <asp:Parameter Name="original_Make" Type="String" />
                <asp:Parameter Name="original_ModelName" Type="String" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="Obsolete" Type="Boolean" />
                <asp:Parameter Name="original_OEMModelType" Type="String" />
                <asp:Parameter Name="original_Make" Type="String" />
                <asp:Parameter Name="original_ModelName" Type="String" />
                <asp:Parameter Name="original_APID" Type="Int32" />
                <asp:Parameter Name="original_PlatformID" Type="Int32" />
                <asp:Parameter Name="OEMModelType" Type="String" />
                <asp:Parameter Name="Make" Type="String" />
                <asp:Parameter Name="ModelName" Type="String" />
                <asp:Parameter Name="PlatformID" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="APID" Type="Int32" />
                <asp:Parameter Name="PlatformID" Type="Int32" />
                <asp:Parameter Name="OEMModelType" Type="String" />
                <asp:Parameter Name="Make" Type="String" />
                <asp:Parameter Name="ModelName" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
