<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="ProgramMaintenance.aspx.vb" Inherits="DataMaintenance_ProgramMaintenance"
    Title="UGNDB: Program by Platform" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <hr />
        <asp:Label ID="lblMessage" runat="server" Width="818px" SkinID="MessageLabelSkin"></asp:Label>
        <table style="width: 100%; border-bottom-style: groove;" class="sampleStyleC">
            <tr>
                <td class="p_textbold" style="color: red">
                    <asp:Label ID="lblPlatform" runat="server" Text="Platform:" />&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblPlatformName" runat="server" Text="" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblOEMManufacturer" runat="server" Text="OEM Manufacturer:" />&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblOEM" runat="server" Text="" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblUGNBusiness" runat="server" Text="UGN Business:" />&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblUGNBiz" runat="server" Text="" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblCurrentPlatform" runat="server" Text="Current Platform:" />&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblCurrentPlatformVal" runat="server" Text="" />
                </td>
                <td class="p_textbold" style="color: red">
                    <asp:Label ID="lblBeginningYear" runat="server" Text="Beginning Year:" />&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblBegYear" runat="server" Text="" />
                </td>
                <td class="p_textbold" style="color: red">
                    <asp:Label ID="lblEndingYear" runat="server" Text="End Year:" />&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblEndYear" runat="server" Text="" />
                </td>
                <td class="p_textbold" style="color: red">
                    <asp:Label ID="lblServiceUntil" runat="server" Text="Service Until:" />&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblSrvYrs" runat="server" Text="" />
                </td>
            </tr>
        </table>
        <br />
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <br />
        <table style="width: 70%">
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblProgram" runat="server" Text="Program Code:" />&nbsp;
                </td>
                <td>
                    <asp:TextBox ID="txtProgramCodeSearch" runat="server" MaxLength="5" Width="80px"></asp:TextBox>
                </td>
                <td class="p_text">
                    <asp:Label ID="lblModel" runat="server" Text="Model:" />&nbsp;
                </td>
                <td>
                    <asp:TextBox ID="txtProgramNameSearch" runat="server" MaxLength="25" Width="200px"></asp:TextBox>
                </td>
                <td class="p_text">
                    <asp:Label ID="lblMake" runat="server" Text="Make:" />&nbsp;
                </td>
                <td>
                    <asp:TextBox ID="txtMakeSearch" runat="server" MaxLength="25" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="5">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" />
                    <asp:Button ID="btnExport" runat="server" Text="Export to Excel" CausesValidation="False" />
                </td>
            </tr>
        </table>
        <hr />
        <div class="showHideColumnsContainer">
            <asp:DropDownList ID="gvPlatformProgramListShowHideColumns" runat="server" Visible="false"
                AutoPostBack="true" OnSelectedIndexChanged="gvPlatformProgramListShowHideColumns_SelectedIndexChanged" />
        </div>
        <br />
        <asp:Panel ID="TCPanel" runat="server" CssClass="collapsePanelHeader">
            <asp:Image ID="imgTC" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                Height="12px" />&nbsp;
            <asp:Label ID="lblTC" runat="server" CssClass="c_textbold" Text="Program by Platform data below." />
        </asp:Panel>
        <asp:Panel ID="TCContentPanel" runat="server" CssClass="collapsePanel">
            <asp:Label ID="Label2" runat="server"><i>Double astericks (**) at the end of each column heading denotes a required field.</i></asp:Label>
            <br />
            <asp:GridView ID="gvPlatformProgramList" runat="server" AutoGenerateColumns="False"
                DataKeyNames="PlatformID,ProgramID" OnRowCommand="gvPlatformProgramList_RowCommand"
                OnRowUpdating="gvPlatformProgramList_RowUpdating" AllowSorting="True" SkinID="StandardGrid"
                DataSourceID="odsPlatformProgram" AllowPaging="True" PageSize="30" CssClass="c_smalltext"
                EmptyDataRowStyle-Font-Size="Medium" EmptyDataRowStyle-Font-Bold="true" EmptyDataRowStyle-ForeColor="Red">
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
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                ImageUrl="~/images/delete.jpg" AlternateText="Delete" />
                        </ItemTemplate>
                        <HeaderStyle Width="30px" />
                        <ItemStyle HorizontalAlign="Right" Width="30px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Volumes">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" ID="hlnkVolume" ImageUrl="~/images/volsales.jpg" ToolTip="Preview/Insert/Edit Volume(s)"
                                NavigateUrl='<%# "ProgramVolume.aspx?pPlatID=" & DataBinder.Eval (Container.DataItem,"PlatformID").tostring & "&pPgmID=" & DataBinder.Eval (Container.DataItem,"ProgramID").tostring & "&sPName=" & ViewState("sPName") & "&sOEMMF=" &  ViewState("sOEMMF") & "&sDUB=" & ViewState("sDUB")  & "&sDCP=" &  ViewState("sDCP") & "&sSOPMM=" & DataBinder.Eval (Container.DataItem,"SOPMM").tostring & "&sSOPYY=" & DataBinder.Eval (Container.DataItem,"SOPYY").tostring & "&EOPMM=" & DataBinder.Eval (Container.DataItem,"EOPMM").tostring & "&sEOPYY=" & DataBinder.Eval (Container.DataItem,"EOPYY").tostring%>' />
                        </ItemTemplate>
                        <HeaderStyle Wrap="False" />
                        <ItemStyle HorizontalAlign="center" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="ProgramID" HeaderText="PGM ID" ReadOnly="True" SortExpression="ProgramID"
                        HeaderStyle-Width="40px" HeaderStyle-Wrap="true" ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-HorizontalAlign="Center" Visible="true">
                        <HeaderStyle HorizontalAlign="Center" Width="40px" Wrap="True" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Platform **" SortExpression="PlatformID">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddPlatformID" runat="server" DataSource='<%# commonFunctions.GetPlatform(0,"","","","","PlatformName") %>'
                                DataValueField="PlatformID" DataTextField="ddPlatformName" AppendDataBoundItems="True"
                                CssClass="c_textxsmall" SelectedValue='<%# Bind("PlatformID") %>'>
                                <asp:ListItem Selected="True" Value="null" Text="Select a Platform">
                                </asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvPlatformID" runat="server" ControlToValidate="ddPlatformID"
                                Display="Dynamic" ErrorMessage="Platform Location is a required field." ValidationGroup="EditInfo"><</asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("PlatformName") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="left" />
                        <ItemStyle HorizontalAlign="left" />
                        <FooterTemplate>
                            <asp:DropDownList ID="ddPlatformIDGV" runat="server" DataSource='<%# commonFunctions.GetPlatform(0,"","","","","PlatformName") %>'
                                DataValueField="PlatformID" DataTextField="ddPlatformName" SelectedValue='<%# Bind("PlatformID") %>'
                                CssClass="c_textxsmall" AppendDataBoundItems="True">
                                <asp:ListItem Selected="True" Value="" Text="">
                                </asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvPlatformIDGV" runat="server" ControlToValidate="ddPlatformIDGV"
                                Display="Dynamic" ErrorMessage="Platform Location is a required field." ValidationGroup="InsertInfo"><</asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Mnemonic Platform **" SortExpression="MNEMONIC_PLATFORM">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtMP" runat="server" Width="100px" MaxLength="25" CssClass="c_textxsmall"
                                Text='<%# Bind("MNEMONIC_PLATFORM") %>' />
                            <asp:RequiredFieldValidator ID="rfvMP" runat="server" ControlToValidate="txtMP" Display="Dynamic"
                                ErrorMessage="Mnemonic Platform is a required field." ValidationGroup="EditInfo"><</asp:RequiredFieldValidator>
                            <ajax:FilteredTextBoxExtender ID="ftbMP" runat="server" TargetControlID="txtMP" FilterType="Numbers" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblMP" runat="server" Text='<%# Bind("MNEMONIC_PLATFORM") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtMPGV" runat="server" Width="100px" MaxLength="25" CssClass="c_textxsmall"
                                Text="0" />
                            <asp:RequiredFieldValidator ID="rfvMPGV" runat="server" ControlToValidate="txtMPGV"
                                Display="Dynamic" ErrorMessage="Mnemonic Platform is a required field." ValidationGroup="InsertInfo"><</asp:RequiredFieldValidator>
                            <ajax:FilteredTextBoxExtender ID="ftbMPGV" runat="server" TargetControlID="txtMPGV"
                                FilterType="Numbers" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Mnemonic Vehicle **" SortExpression="MNEMONIC_VEHICLE">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtMV" runat="server" Width="100px" MaxLength="25" CssClass="c_textxsmall"
                                Text='<%# Bind("MNEMONIC_VEHICLE") %>' />
                            <asp:RequiredFieldValidator ID="rfvMV" runat="server" ControlToValidate="txtMV" Display="Dynamic"
                                ErrorMessage="Mnemonic Vehicle is a required field." ValidationGroup="EditInfo"><</asp:RequiredFieldValidator>
                            <ajax:FilteredTextBoxExtender ID="ftbMV" runat="server" TargetControlID="txtMV" FilterType="Numbers" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblMV" runat="server" Text='<%# Bind("MNEMONIC_VEHICLE") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtMVGV" runat="server" Width="100px" MaxLength="25" CssClass="c_textxsmall"
                                Text="0" />
                            <asp:RequiredFieldValidator ID="rfvMVGV" runat="server" ControlToValidate="txtMVGV"
                                Display="Dynamic" ErrorMessage="Mnemonic Vehicle is a required field." ValidationGroup="InsertInfo"><</asp:RequiredFieldValidator>
                            <ajax:FilteredTextBoxExtender ID="ftbMVGV" runat="server" TargetControlID="txtMVGV"
                                FilterType="Numbers" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Mnemonic Vehicle Plant **" SortExpression="MNEMONIC_VEHICLE_PLANT">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtMVP" runat="server" Width="100px" MaxLength="25" CssClass="c_textxsmall"
                                Text='<%# Bind("MNEMONIC_VEHICLE_PLANT") %>' />
                            <asp:RequiredFieldValidator ID="rfvMVP" runat="server" ControlToValidate="txtMVP"
                                Display="Dynamic" ErrorMessage="Mnemonic Vehicle Plant is a required field."
                                ValidationGroup="EditInfo"><</asp:RequiredFieldValidator>
                            <ajax:FilteredTextBoxExtender ID="ftbMVP" runat="server" TargetControlID="txtMVP"
                                FilterType="Numbers" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblMVP" runat="server" Text='<%# Bind("MNEMONIC_VEHICLE_PLANT") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtMVPGV" runat="server" Width="100px" MaxLength="25" CssClass="c_textxsmall"
                                Text="0" />
                            <asp:RequiredFieldValidator ID="rfvMVPGV" runat="server" ControlToValidate="txtMVPGV"
                                Display="Dynamic" ErrorMessage="Mnemonic Vehicle Plant is a required field."
                                ValidationGroup="InsertInfo"><</asp:RequiredFieldValidator>
                            <ajax:FilteredTextBoxExtender ID="ftbMVPGV" runat="server" TargetControlID="txtMVPGV"
                                FilterType="Numbers" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="IHS Program Code" SortExpression="CSM_Program" HeaderStyle-HorizontalAlign="Left">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtCSMProgram" runat="server" CssClass="c_textxsmall" Text='<%# Bind("CSM_Program") %>'
                                MaxLength="15" Width="60px" />
                            <ajax:FilteredTextBoxExtender ID="ftbCSMPgm" runat="server" TargetControlID="txtCSMProgram"
                                FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,()/- " />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblCSMProgram" runat="server" Text='<%# Bind("CSM_Program") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtCSMProgramGV" runat="server" MaxLength="15" Width="60px" CssClass="c_textxsmall" />
                            <ajax:FilteredTextBoxExtender ID="ftbCSMPgm" runat="server" TargetControlID="txtCSMProgramGV"
                                FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,()/- " />
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="IHS Model Name" SortExpression="CSM_Model_Name">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtCSMModelName" runat="server" CssClass="c_textxsmall" Text='<%# Bind("CSM_Model_Name") %>'
                                MaxLength="30" Width="100px" />
                            <ajax:FilteredTextBoxExtender ID="ftbCSMModelName" runat="server" TargetControlID="txtCSMModelName"
                                FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,/- " />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblCSMModelName" runat="server" Text='<%# Bind("CSM_Model_Name") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <FooterTemplate>
                            <asp:TextBox ID="txtCSMModelNameGV" runat="server" MaxLength="30" Width="100px" CssClass="c_textxsmall" />
                            <ajax:FilteredTextBoxExtender ID="ftbCSMModelName" runat="server" TargetControlID="txtCSMModelNameGV"
                                FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,/- " />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Program Code **" SortExpression="BPCSProgramRef">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtBPCSProgramRef" runat="server" CssClass="c_textxsmall" Text='<%# Bind("BPCSProgramRef") %>'
                                MaxLength="5" Width="60px" />
                            <asp:RequiredFieldValidator ID="rfvBPCSProgramRef" runat="server" ControlToValidate="txtBPCSProgramRef"
                                Display="Dynamic" ErrorMessage="Program Code is a required field." ValidationGroup="EditInfo"><</asp:RequiredFieldValidator>
                            <ajax:FilteredTextBoxExtender ID="ftbBPCSProgramRef" runat="server" TargetControlID="txtBPCSProgramRef"
                                FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,/- " />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblBPCSProgramRef" runat="server" Text='<%# Bind("BPCSProgramRef") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <FooterTemplate>
                            <asp:TextBox ID="txtBPCSProgramRefGV" runat="server" MaxLength="5" Width="60px" CssClass="c_textxsmall" />
                            <asp:RequiredFieldValidator ID="rfvBPCSProgramRefGV" runat="server" ControlToValidate="txtBPCSProgramRefGV"
                                Display="Dynamic" ErrorMessage="Program Code is a required field." ValidationGroup="InsertInfo"><</asp:RequiredFieldValidator>
                            <ajax:FilteredTextBoxExtender ID="ftbBPCSProgramRefe" runat="server" TargetControlID="txtBPCSProgramRefGV"
                                FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,/- " />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Pgm Gen" SortExpression="ProgramSuffix">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtProgramSuffix" runat="server" CssClass="c_textxsmall" Text='<%# Bind("ProgramSuffix") %>'
                                MaxLength="1" Width="20px" />
                            <ajax:FilteredTextBoxExtender ID="ftbProgramSuffix" runat="server" TargetControlID="txtProgramSuffix"
                                FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblProgramSuffix" runat="server" Text='<%# Bind("ProgramSuffix") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <FooterTemplate>
                            <asp:TextBox ID="txtProgramSuffixGV" runat="server" MaxLength="1" Width="20px" CssClass="c_textxsmall" />
                            <ajax:FilteredTextBoxExtender ID="ftbProgramSuffix" runat="server" TargetControlID="txtProgramSuffixGV"
                                FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Make **" SortExpression="Make">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddMake1" runat="server" CssClass="c_textxsmall" />
                            <asp:RequiredFieldValidator ID="rfvMake11" runat="server" ControlToValidate="ddMake1"
                                Display="Dynamic" ErrorMessage="Make is a required field." ValidationGroup="EditInfo"><</asp:RequiredFieldValidator>
                            <ajax:CascadingDropDown ID="cddMakes1" runat="server" TargetControlID="ddMake1" Category="Make"
                                SelectedValue='<%# Bind("Make") %>' PromptText="Please select a Make." LoadingText="[Loading Makes...]"
                                ServicePath="~/WS/VehicleCDDService.asmx" ServiceMethod="GetMakes" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblMake" runat="server" Text='<%# Bind("ddMake") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <FooterTemplate>
                            <asp:DropDownList ID="ddMakeGV" runat="server" CssClass="c_textxsmall" />
                            <asp:RequiredFieldValidator ID="rfvMake11" runat="server" ControlToValidate="ddMakeGV"
                                Display="Dynamic" ErrorMessage="Make is a required field." ValidationGroup="InsertInfo"><</asp:RequiredFieldValidator>
                            <ajax:CascadingDropDown ID="cddMakes" runat="server" TargetControlID="ddMakeGV" Category="Make"
                                PromptText="Please select a Make." LoadingText="[Loading Makes...]" ServicePath="~/WS/VehicleCDDService.asmx"
                                ServiceMethod="GetMakes" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Model Name**" SortExpression="ProgramName">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddModel1" runat="server" CssClass="c_textxsmall" />
                            <asp:RequiredFieldValidator ID="rfvModel1" runat="server" ControlToValidate="ddModel1"
                                Display="Dynamic" ErrorMessage="Model is a required field." ValidationGroup="EditInfo"><</asp:RequiredFieldValidator>
                            <ajax:CascadingDropDown ID="cddModel1" runat="server" TargetControlID="ddModel1"
                                ParentControlID="ddMake1" SelectedValue='<%# Bind("ProgramName") %>' Category="Model"
                                PromptText="Please select a Model." LoadingText="[Loading Models...]" ServicePath="~/WS/VehicleCDDService.asmx"
                                ServiceMethod="GetModelMaint" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblModel" runat="server" Text='<%# Bind("ddModel") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <FooterTemplate>
                            <asp:DropDownList ID="ddModelGV" runat="server" CssClass="c_textxsmall" />
                            <asp:RequiredFieldValidator ID="rfvModel" runat="server" ControlToValidate="ddModelGV"
                                Display="Dynamic" ErrorMessage="Model is a required field." ValidationGroup="InsertInfo"><</asp:RequiredFieldValidator>
                            <ajax:CascadingDropDown ID="cddModel" runat="server" TargetControlID="ddModelGV"
                                ParentControlID="ddMakeGV" Category="Model" PromptText="Please select a Model."
                                LoadingText="[Loading Models...]" ServicePath="~/WS/VehicleCDDService.asmx" ServiceMethod="GetModelMaint" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Vehicle Type **" SortExpression="ddVehicleType">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddVehicleType" runat="server" CssClass="c_textxsmall" />
                            <asp:RequiredFieldValidator ID="rfvVehicleType" runat="server" ControlToValidate="ddVehicleType"
                                Display="Dynamic" ErrorMessage="Vehicle Type is a required field." ValidationGroup="EditInfo"><</asp:RequiredFieldValidator>
                            <ajax:CascadingDropDown ID="cddVehicleType" runat="server" TargetControlID="ddVehicleType"
                                SelectedValue='<%# Bind("VTID") %>' Category="VTID" PromptText="Please select a Vehicle Type."
                                LoadingText="[Loading Vehicle Types...]" ServicePath="~/WS/VehicleCDDService.asmx"
                                ServiceMethod="GetVehicleType" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblVehicleType" runat="server" Text='<%# Bind("ddVehicleType") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <FooterTemplate>
                            <asp:DropDownList ID="ddVehicleTypeGV" runat="server" CssClass="c_textxsmall" />
                            <asp:RequiredFieldValidator ID="rfvVehicleTypeGV" runat="server" ControlToValidate="ddVehicleTypeGV"
                                Display="Dynamic" ErrorMessage="Vehicle Type is a required field." ValidationGroup="InsertInfo"><</asp:RequiredFieldValidator>
                            <ajax:CascadingDropDown ID="cddVehicleTypeGV" runat="server" TargetControlID="ddVehicleTypeGV"
                                Category="VTID" PromptText="Please select a Vehicle Type." LoadingText="[Loading Vehicle Types...]"
                                ServicePath="~/WS/VehicleCDDService.asmx" ServiceMethod="GetVehicleType" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Assembly Plant Location **" SortExpression="ddAssemblyPlantLocation">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddAssembly" runat="server" DataSource='<%# commonFunctions.GetAssemblyPlantLocation(0,"","",lblOEM.text,"A") %>'
                                DataValueField="APID" DataTextField="ddAssemblyPlantLocation" SelectedValue='<%# Bind("APID") %>'
                                CssClass="c_textxsmall" AppendDataBoundItems="True">
                                <asp:ListItem Selected="True" Value="1" Text="N/A">
                                </asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvAssembly" runat="server" ControlToValidate="ddAssembly"
                                Display="Dynamic" ErrorMessage="Assembly Plant Location is a required field."
                                ValidationGroup="EditInfo"><</asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:ImageButton ID="btnPrevAPL" runat="server" CausesValidation="False" CommandName="PrevAPL"
                                ImageUrl="~/images/mfg.jpg" AlternateText="Preview list of OEM Model Types by Assembly Plant" />
                            <asp:Label ID="lblAssembly" runat="server" Text='<%# Bind("ddAssemblyPlantLocation") %>' />
                        </ItemTemplate>
                        <ItemStyle VerticalAlign="Middle" />
                        <HeaderStyle HorizontalAlign="Left" />
                        <FooterTemplate>
                            <asp:DropDownList ID="ddAssemblyGV" runat="server" DataSource='<%# commonFunctions.GetAssemblyPlantLocation(0,"","",lblOEM.text,"A") %>'
                                DataValueField="APID" DataTextField="ddAssemblyPlantLocation" SelectedValue='<%# Bind("APID") %>'
                                CssClass="c_textxsmall" AppendDataBoundItems="True">
                                <asp:ListItem Selected="True" Value="1" Text="N/A">
                                </asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvAssemblyGV" runat="server" ControlToValidate="ddAssemblyGV"
                                Display="Dynamic" ErrorMessage="Assembly Plant Location is a required field."
                                ValidationGroup="InsertInfo"><</asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SOP **" SortExpression="SOP">
                        <EditItemTemplate>
                            <asp:Label ID="lblMM1" runat="server" Text="MM:" Font-Bold="true" ForeColor="Red"
                                CssClass="c_textxsmall" />
                            <asp:TextBox ID="txtSOPMM" runat="server" CssClass="c_textxsmall" Text='<%# Bind("SOPMM") %>'
                                MaxLength="2" Width="20px" />
                            <asp:Label ID="lblYY1" runat="server" Text="YY:" Font-Bold="true" ForeColor="Red"
                                CssClass="c_textxsmall" />
                            <asp:TextBox ID="txtSOPYY" runat="server" Text='<%# Bind("SOPYY") %>' MaxLength="4"
                                Width="30px" CssClass="c_textxsmall" />
                            <asp:RequiredFieldValidator ID="rfvSOPMM" runat="server" ControlToValidate="txtSOPMM"
                                Display="Dynamic" ErrorMessage="SOP Month is a required field." ValidationGroup="EditInfo"><</asp:RequiredFieldValidator><asp:RequiredFieldValidator
                                    ID="rfvSOPYY" runat="server" ControlToValidate="txtSOPYY" Display="Dynamic" ErrorMessage="SOP Year is a required field."
                                    ValidationGroup="EditInfo"><</asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvSOPMM" runat="server" ErrorMessage="SOP Month values between 01 to 12"
                                ControlToValidate="txtSOPMM" MinimumValue="0" MaximumValue="12" ValidationGroup="EditInfo"><</asp:RangeValidator>
                            <asp:RangeValidator ID="rvSOPYY" runat="server" ErrorMessage="SOP Year values between 1997 to 2030"
                                ControlToValidate="txtSOPYY" MinimumValue="1997" MaximumValue="2030" ValidationGroup="EditInfo"><</asp:RangeValidator>
                            <asp:CompareValidator ID="cvSOPYY" runat="server" ErrorMessage="SOP Year must be less than or equal to EOP Year."
                                ControlToCompare="txtEOPYY" ControlToValidate="txtSOPYY" Operator="LessThanEqual"
                                Type="Integer" ValidationGroup="EditInfo"><</asp:CompareValidator>
                            <asp:CompareValidator ID="cvPFBegYY" runat="server" ErrorMessage="SOP Year must be greater than or equal to Platform Beginning Year."
                                ValueToCompare='<%# lblBegYear.text %>' ControlToValidate="txtSOPYY" Operator="GreaterThanEqual"
                                Type="Integer" ValidationGroup="EditInfo"><</asp:CompareValidator>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblSOP" runat="server" Text='<%# Bind("SOP") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <FooterTemplate>
                            <asp:Label ID="lblMM2" runat="server" Text="MM:" Font-Bold="true" ForeColor="Red"
                                CssClass="c_textxsmall" />
                            <asp:TextBox ID="txtSOPMMGV" runat="server" MaxLength="2" Width="20px" CssClass="c_textxsmall" />
                            <asp:Label ID="lblYY2" runat="server" Text="YY:" Font-Bold="true" ForeColor="Red"
                                CssClass="c_textxsmall" />
                            <asp:TextBox ID="txtSOPYYGV" runat="server" MaxLength="4" Width="30px" CssClass="c_textxsmall" />
                            <asp:RequiredFieldValidator ID="rfvSOPMMGV" runat="server" ControlToValidate="txtSOPMMGV"
                                Display="Dynamic" ErrorMessage="SOP Month is a required field." ValidationGroup="InsertInfo"><</asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="rfvSOPYYGV" runat="server" ControlToValidate="txtSOPYYGV"
                                Display="Dynamic" ErrorMessage="SOP Year is a required field." ValidationGroup="InsertInfo"><</asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvSOPMM" runat="server" ErrorMessage="SOP Month values between 01 to 12"
                                ControlToValidate="txtSOPMMGV" MinimumValue="0" MaximumValue="12" ValidationGroup="InsertInfo"><</asp:RangeValidator>
                            <asp:CompareValidator ID="cvSOPYY" runat="server" ErrorMessage="SOP Year must be less than or equal to EOP Year."
                                ControlToCompare="txtEOPYYGV" ControlToValidate="txtSOPYYGV" Operator="LessThanEqual"
                                Type="Integer" ValidationGroup="InsertInfo"><</asp:CompareValidator>
                            <asp:CompareValidator ID="cvPFBegYYGV" runat="server" ErrorMessage="SOP Year must be greater than or equal to Platform Beginning Year."
                                ValueToCompare='<%# lblBegYear.text %>' ControlToValidate="txtSOPYYGV" Operator="GreaterThanEqual"
                                Type="Integer" ValidationGroup="InsertInfo"><</asp:CompareValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="EOP **" SortExpression="EOP">
                        <EditItemTemplate>
                            <asp:Label ID="lblMM3" runat="server" Text="MM:" Font-Bold="true" ForeColor="Red"
                                CssClass="c_textxsmall" />
                            <asp:TextBox ID="txtEOPMM" runat="server" CssClass="c_textxsmall" Text='<%# Bind("EOPMM") %>'
                                MaxLength="2" Width="20px" />
                            <asp:Label ID="lblYY3" runat="server" CssClass="c_textxsmall" Text="YY:" Font-Bold="true"
                                ForeColor="Red" />
                            <asp:TextBox ID="txtEOPYY" runat="server" CssClass="c_textxsmall" Text='<%# Bind("EOPYY") %>'
                                MaxLength="4" Width="30px" />
                            <asp:RequiredFieldValidator ID="rfvEOPMM" runat="server" ControlToValidate="txtEOPMM"
                                Display="Dynamic" ErrorMessage="EOP Month is a required field." ValidationGroup="EditInfo"><</asp:RequiredFieldValidator><asp:RequiredFieldValidator
                                    ID="rfvEOPYY" runat="server" ControlToValidate="txtEOPYY" Display="Dynamic" ErrorMessage="EOP Year is a required field."
                                    ValidationGroup="EditInfo"><</asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvEOPMM" runat="server" ErrorMessage="EOP Month values between 01 to 12"
                                ControlToValidate="txtEOPMM" MinimumValue="0" MaximumValue="12" ValidationGroup="EditInfo"><</asp:RangeValidator>
                            <asp:RangeValidator ID="rvEOPYY" runat="server" ErrorMessage="EOP Year values between 1997 to 2030"
                                ControlToValidate="txtEOPYY" MinimumValue="1997" MaximumValue="2030" ValidationGroup="EditInfo"><</asp:RangeValidator>
                            <asp:CompareValidator ID="cvEOPYY" runat="server" ErrorMessage="EOP Year must be greater than or equal to SOP Year."
                                ControlToCompare="txtSOPYY" ControlToValidate="txtEOPYY" Operator="GreaterThanEqual"
                                Type="Integer" ValidationGroup="EditInfo"><</asp:CompareValidator>
                            <asp:CompareValidator ID="cvPFEndYY" runat="server" ErrorMessage="EOP Year must be less than or equal to Platform End Year."
                                ValueToCompare='<%# lblEndYear.text %>' ControlToValidate="txtEOPYY" Operator="LessThanEqual"
                                Type="Integer" ValidationGroup="EditInfo"><</asp:CompareValidator>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblEOP" runat="server" Text='<%# Bind("EOP") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <FooterTemplate>
                            <asp:Label ID="lblMM4" runat="server" CssClass="c_textxsmall" Text="MM:" Font-Bold="true"
                                ForeColor="Red" />
                            <asp:TextBox ID="txtEOPMMGV" runat="server" MaxLength="2" Width="20px" CssClass="c_textxsmall" />
                            <asp:Label ID="lblYY4" runat="server" CssClass="c_textxsmall" Text="YY:" Font-Bold="true"
                                ForeColor="Red" />
                            <asp:TextBox ID="txtEOPYYGV" runat="server" MaxLength="4" Width="30px" CssClass="c_textxsmall" />
                            <asp:RequiredFieldValidator ID="rfvEOPMMGV" runat="server" ControlToValidate="txtEOPMMGV"
                                Display="Dynamic" ErrorMessage="EOP Month is a required field." ValidationGroup="InsertInfo"><</asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="rfvEOPYYGV" runat="server" ControlToValidate="txtEOPYYGV"
                                Display="Dynamic" ErrorMessage="EOP Year is a required field." ValidationGroup="InsertInfo"><</asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvEOPMM" runat="server" ErrorMessage="EOP Month values between 01 to 12"
                                ControlToValidate="txtEOPMMGV" MinimumValue="0" MaximumValue="12" ValidationGroup="InsertInfo"><</asp:RangeValidator>
                            <asp:CompareValidator ID="cvEOPYY" runat="server" ErrorMessage="EOP Year must be greater than or equal to SOP Year."
                                ControlToCompare="txtSOPYYGV" ControlToValidate="txtEOPYYGV" Operator="GreaterThanEqual"
                                Type="Integer" ValidationGroup="InsertInfo"><</asp:CompareValidator>
                            <asp:CompareValidator ID="cvPFEndYYGV" runat="server" ErrorMessage="EOP Year must be less than or equal to Platform End Year."
                                ValueToCompare='<%# lblEndYear.text %>' ControlToValidate="txtEOPYYGV" Operator="LessThanEqual"
                                Type="Integer" ValidationGroup="InsertInfo"><</asp:CompareValidator>
                        </FooterTemplate>
                        <FooterStyle Width="250px" />
                        <ItemStyle Width="250px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Service Assembly Plant Location" SortExpression="ServiceAssemblyPlantLocation">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddServiceAssembly" runat="server" DataSource='<%# commonFunctions.GetAssemblyPlantLocation(0,"","",lblOEM.text,"S") %>'
                                DataValueField="APID" DataTextField="ddAssemblyPlantLocation" SelectedValue='<%# Bind("ServiceAPID") %>'
                                CssClass="c_textxsmall" AppendDataBoundItems="True">
                                <asp:ListItem Selected="True" Value="1" Text="N/A">
                                </asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("ServiceAssemblyPlantLocation") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddServiceAssemblyGV" runat="server" DataSource='<%# commonFunctions.GetAssemblyPlantLocation(0,"","",lblOEM.text,"S") %>'
                                DataValueField="APID" DataTextField="ddAssemblyPlantLocation" SelectedValue='<%# Bind("ServiceAPID") %>'
                                CssClass="c_textxsmall" AppendDataBoundItems="True">
                                <asp:ListItem Selected="True" Value="1" Text="N/A">
                                </asp:ListItem>
                            </asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Service EOP" SortExpression="ServiceEOP" HeaderStyle-Width="250px">
                        <EditItemTemplate>
                            <asp:Label ID="lblSrvMM1" runat="server" CssClass="c_textxsmall" Text="MM:" Font-Bold="true"
                                ForeColor="Red" />
                            <asp:TextBox ID="txtServiceEOPMM" runat="server" CssClass="c_textxsmall" Text='<%# Bind("ServiceEOPMM") %>'
                                MaxLength="2" Width="20px" />
                            <asp:Label ID="lblSrvYY1" runat="server" CssClass="c_textxsmall" Text="YY:" Font-Bold="true"
                                ForeColor="Red" />
                            <asp:TextBox ID="txtServiceEOPYY" runat="server" CssClass="c_textxsmall" Text='<%# Bind("ServiceEOPYY") %>'
                                MaxLength="4" Width="30px" />
                            <asp:RangeValidator ID="rvServiceEOPMM" runat="server" ErrorMessage="Service EOP Month values between 01 to 12"
                                ControlToValidate="txtServiceEOPMM" MinimumValue="0" MaximumValue="12" ValidationGroup="EditInfo"><</asp:RangeValidator>
                            <asp:RangeValidator ID="rvServiceEOPYY" runat="server" ErrorMessage="Service EOP Year values between 1997 to 2030"
                                ControlToValidate="txtServiceEOPYY" MinimumValue="1997" MaximumValue="2030" ValidationGroup="EditInfo"><</asp:RangeValidator>
                            <asp:CompareValidator ID="cvServiceEOPYY" runat="server" ErrorMessage="Service EOP Year must be greater than or equal to EOP Year."
                                ControlToCompare="txtEOPYY" ControlToValidate="txtServiceEOPYY" Operator="GreaterThanEqual"
                                Type="Integer" ValidationGroup="EditInfo"><</asp:CompareValidator>
                            <asp:CompareValidator ID="cvSrvEOPYY2" runat="server" ErrorMessage="Service EOP Year must be less than or equal to Service Until."
                                ValueToCompare='<%# lblSrvYrs.text %>' ControlToValidate="txtServiceEOPYY" Operator="LessThanEqual"
                                Type="Integer" ValidationGroup="EditInfo"><</asp:CompareValidator>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblServiceEOP" runat="server" Text='<%# Bind("ServiceEOP") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <%-- <FooterTemplate>
                        <asp:Label ID="lblSrvMM2" runat="server" Text="MM:" Font-Bold="true" ForeColor="Red" /><asp:TextBox
                            ID="txtServiceEOPMMGV" runat="server" MaxLength="2" Width="20px" /><br />
                        <asp:Label ID="lblSrvYY2" runat="server" Text="YY:" Font-Bold="true" ForeColor="Red" /><asp:TextBox
                            ID="txtServiceEOPYYGV" runat="server" MaxLength="4" Width="30px" />
                        <asp:RangeValidator ID="rvSERVICEEOPMM" runat="server" ErrorMessage="Service EOP Month values between 01 to 12"
                            ControlToValidate="txtServiceEOPMMGV" MinimumValue="0" MaximumValue="12" ValidationGroup="InsertInfo"><</asp:RangeValidator>
                        <asp:CompareValidator ID="cvSERVICEEOPYY" runat="server" ErrorMessage="Service EOP Year must be greater than EOP Year."
                            ControlToCompare="txtEOPYYGV" ControlToValidate="txtServiceEOPYYGV" Operator="GreaterThan"
                            Type="Integer" ValidationGroup="InsertInfo"><</asp:CompareValidator>
                             <asp:RangeValidator ID="rvSrvYrsGV" runat="server" ErrorMessage="Service EOP Year values must be less or equal to "
                            ControlToValidate="txtServiceEOPYYGV" MinimumValue='<%# Bind("EOPYY") %>' MaximumValue='<%# Bind("EOPYY") %>' ValidationGroup="InsertInfo"><</asp:RangeValidator>
                    </FooterTemplate>
                    <FooterStyle Width="250px" />--%>
                        <ItemStyle Width="250px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="UGN Business **" SortExpression="UGNBusiness" HeaderStyle-Wrap="true"
                        HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddUGNBusiness" runat="server" CssClass="c_textxsmall" SelectedValue='<%# Bind("UGNBusiness") %>'>
                                <asp:ListItem Selected="True" Value="False">No</asp:ListItem>
                                <asp:ListItem Value="True">Yes</asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblUGNBiz" runat="server" Text='<%# Bind("UGNBusinessDisplay") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddUGNBusinessGV" runat="server" CssClass="c_textxsmall" SelectedValue='<%# Bind("UGNBusiness") %>'>
                                <asp:ListItem Selected="True" Value="False">No</asp:ListItem>
                                <asp:ListItem Value="True">Yes</asp:ListItem>
                            </asp:DropDownList>
                        </FooterTemplate>
                        <HeaderStyle Width="60px" Wrap="True" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Obsolete" SortExpression="Obsolete">
                        <EditItemTemplate>
                            <asp:CheckBox ID="chkObsoleteEdit" runat="server" CssClass="c_textxsmall" Checked='<%# Bind("Obsolete") %>' />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkObsoletePreEdit" runat="server" CssClass="c_textxsmall" Checked='<%# Bind("Obsolete") %>'
                                Enabled="false" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Notes" SortExpression="Notes" HeaderStyle-HorizontalAlign="Left">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtNotes" runat="server" CssClass="c_textxsmall" Text='<%# Bind("Notes") %>'
                                Width="300px" MaxLength="200" Rows="3" TextMode="MultiLine" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblnotes" runat="server" Text='<%# Bind("Notes") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtNotesGV" runat="server" CssClass="c_textxsmall" Text='<%# Bind("Notes") %>'
                                Width="300px" MaxLength="200" Rows="3" TextMode="MultiLine" />
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="comboUpdateInfo" HeaderText="Last Update" ReadOnly="True"
                        HeaderStyle-Width="80px" HeaderStyle-Wrap="true" SortExpression="comboUpdateInfo">
                        <HeaderStyle HorizontalAlign="Left" Width="80px" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsPlatformProgram" runat="server" InsertMethod="InsertPlatformProgram"
                OldValuesParameterFormatString="original_{0}" SelectMethod="GetPlatformProgram"
                TypeName="PlatformBLL" UpdateMethod="UpdatePlatformProgram" DeleteMethod="DeletePlatformProgram">
                <DeleteParameters>
                    <asp:Parameter Name="PlatformID" Type="Int32" />
                    <asp:Parameter Name="ProgramID" Type="Int32" />
                    <asp:Parameter Name="original_PlatformID" Type="Int32" />
                    <asp:Parameter Name="original_ProgramID" Type="String" />
                </DeleteParameters>
                <UpdateParameters>
                    <asp:Parameter Name="Make" Type="String" />
                    <asp:Parameter Name="CSM_Program" Type="String" />
                    <asp:Parameter Name="CSM_Model_Name" Type="String" />
                    <asp:Parameter Name="WAF_Model_Name" Type="String" />
                    <asp:Parameter Name="VTID" Type="Int32" />
                    <asp:Parameter Name="APID" Type="Int32" />
                    <asp:Parameter Name="SOPMM" Type="Int32" />
                    <asp:Parameter Name="SOPYY" Type="Int32" />
                    <asp:Parameter Name="EOPMM" Type="Int32" />
                    <asp:Parameter Name="EOPYY" Type="Int32" />
                    <asp:Parameter Name="BPCSProgramRef" Type="String" />
                    <asp:Parameter Name="ProgramSuffix" Type="String" />
                    <asp:Parameter Name="UGNBusiness" Type="Boolean" />
                    <asp:Parameter Name="Notes" Type="String" />
                    <asp:Parameter Name="ServiceAPID" Type="Int32" />
                    <asp:Parameter Name="ServiceEOPMM" Type="Int32" />
                    <asp:Parameter Name="ServiceEOPYY" Type="Int32" />
                    <asp:Parameter Name="Obsolete" Type="Boolean" />
                    <asp:Parameter Name="original_ProgramID" Type="Int32" />
                    <asp:Parameter Name="original_PlatformID" Type="Int32" />
                    <asp:Parameter Name="ProgramName" Type="String" />
                    <asp:Parameter Name="PlatformID" Type="Int32" />
                    <asp:Parameter Name="Mnemonic_Platform" Type="Int32" />
                    <asp:Parameter Name="Mnemonic_Vehicle" Type="Int32" />
                    <asp:Parameter Name="Mnemonic_Vehicle_Plant" Type="Int32" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:QueryStringParameter DefaultValue="" Name="PlatformID" QueryStringField="pPlatID"
                        Type="Int32" />
                    <asp:Parameter Name="ProgramID" Type="Int32" />
                    <asp:QueryStringParameter DefaultValue="" Name="ProgramCode" QueryStringField="sPgmCode"
                        Type="String" />
                    <asp:QueryStringParameter Name="ModelName" QueryStringField="sPgmName" Type="String" />
                    <asp:QueryStringParameter DefaultValue="" Name="Make" QueryStringField="sMake" Type="String" />
                </SelectParameters>
                <InsertParameters>
                    <asp:QueryStringParameter DefaultValue="" Name="PlatformID" QueryStringField="pPlatID"
                        Type="Int32" />
                    <asp:Parameter Name="Mnemonic_Platform" Type="Int32" />
                    <asp:Parameter Name="Mnemonic_Vehicle" Type="Int32" />
                    <asp:Parameter Name="Mnemonic_Vehicle_Plant" Type="Int32" />
                    <asp:Parameter Name="Make" Type="String" />
                    <asp:Parameter Name="CSM_Program" Type="String" />
                    <asp:Parameter Name="CSM_Model_Name" Type="String" />
                    <asp:Parameter Name="VTID" Type="Int32" />
                    <asp:Parameter Name="APID" Type="Int32" />
                    <asp:Parameter Name="SOPMM" Type="Int32" />
                    <asp:Parameter Name="SOPYY" Type="Int32" />
                    <asp:Parameter Name="EOPMM" Type="Int32" />
                    <asp:Parameter Name="EOPYY" Type="Int32" />
                    <asp:Parameter Name="BPCSProgramRef" Type="String" />
                    <asp:Parameter Name="ProgramName" Type="String" />
                    <asp:Parameter Name="ProgramSuffix" Type="String" />
                    <asp:Parameter Name="UGNBusiness" Type="Boolean" />
                    <asp:Parameter Name="Notes" Type="String" />
                    <asp:Parameter Name="ServiceAPID" Type="Int32" />
                    <asp:Parameter Name="ServiceEOPMM" Type="Int32" />
                    <asp:Parameter Name="ServiceEOPYY" Type="Int32" />
                </InsertParameters>
            </asp:ObjectDataSource>
            <br />
            <br />
        </asp:Panel>
        <ajax:CollapsiblePanelExtender ID="TCExtender" runat="server" TargetControlID="TCContentPanel"
            ExpandControlID="TCPanel" CollapseControlID="TCPanel" Collapsed="FALSE" TextLabelID="lblTC"
            ExpandedText="Program by Platform data below." CollapsedText="Program by Platform data below."
            ImageControlID="imgTC" CollapsedImage="~/images/expand_blue.jpg" ExpandedImage="~/images/collapse_blue.jpg"
            SuppressPostBack="true" ScrollContents="true">
        </ajax:CollapsiblePanelExtender>
        <br />
        <asp:ValidationSummary ID="vsEditInfo" runat="server" ShowMessageBox="True" ShowSummary="True"
            Width="498px" ValidationGroup="EditInfo" />
        <asp:ValidationSummary ID="vsInsertInfo" runat="server" ShowMessageBox="True" ShowSummary="True"
            Width="498px" ValidationGroup="InsertInfo" />
    </asp:Panel>
</asp:Content>
