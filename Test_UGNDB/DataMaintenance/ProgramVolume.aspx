<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="ProgramVolume.aspx.vb" Inherits="DataMaintenance_ProgramMaintenance"
    Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server">
        <hr />
        <asp:Label ID="lblMessage" runat="server" Width="818px" SkinID="MessageLabelSkin"></asp:Label>
        <table style="width: 100%; border-bottom-style: groove;" class="sampleStyleC">
            <tr>
                <td class="p_text">
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
                    <asp:Label ID="Label14" runat="server" Text="UGN Business:" />&nbsp;
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
                <td class="p_text">
                    <asp:Label ID="lblBeginningYear" runat="server" Text="Beginning Year:" />&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblBegYear" runat="server" Text="" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblEndingYear" runat="server" Text="End Year:" />&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblEndYear" runat="server" Text="" />
                </td>
            </tr>
            <tr>
                <td colspan="12">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="p_textbold" style="color: red">
                    <asp:Label ID="lblProgram" runat="server" Text="Program Code:" />&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblPgmCode" runat="server" Text="" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblMake" runat="server" Text="Make:" />&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblMakeVal" runat="server" Text="" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblModel" runat="server" Text="Model:" />&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblModelName" runat="server" Text="" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblVehicleType" runat="server" Text="Vehicle Type:" />&nbsp;
                </td>
                <td class="c_textbold" colspan="5">
                    <asp:Label ID="lblVehicleTypeVal" runat="server" Text="" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblAssemblyPlantLocation" runat="server" Text="Assembly Plant Location:" />&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblAPL" runat="server" Text="" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblState" runat="server" Text="State:" />&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblStateVal" runat="server" Text="" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblCountry" runat="server" Text="Country:" />&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblCountryVal" runat="server" Text="" />
                </td>
                <td class="p_textbold" style="color: red">
                    <asp:Label ID="lblSOP" runat="server" Text="SOP:" />&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblSOPVal" runat="server" Text="" />
                    <asp:Label ID="lblSOPMM" runat="server" Text="" Visible="false" />
                    <asp:Label ID="lblSOPYY" runat="server" Text="" Visible="false" />
                </td>
                <td class="p_textbold" style="color: red">
                    <asp:Label ID="lblEOP" runat="server" Text="EOP:" />&nbsp;
                </td>
                <td class="c_textbold" colspan="3">
                    <asp:Label ID="lblEOPVal" runat="server" Text="" />
                    <asp:Label ID="lblEOPMM" runat="server" Text="" Visible="false" />
                    <asp:Label ID="lblEOPYY" runat="server" Text="" Visible="false" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblUGNBusiness2" runat="server" Text="UGN Business:" />&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblUGNBiz2" runat="server" Text="" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblStatus" runat="server" Text="Status:" />&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblRecStatus" runat="server" Text="" />
                </td>
                <td colspan="8">
                    &nbsp;
                </td>
            </tr>
        </table>
        <br />
        <asp:Label ID="lblRaiseError" runat="server" Text="" Visible="false" SkinID="MessageLabelSkin" /><br />
        <asp:Label ID="Label2" runat="server"><i>Double astericks (**) at the end of each column heading denotes a required field.</i></asp:Label>
        <br />
        <i><font style="background-color: Yellow">Row highlighted in Yellow indicates current
            year.</font></i>
        <asp:GridView ID="gvProgramVolumeList" runat="server" AutoGenerateColumns="False"
            DataKeyNames="ProgramID,YearID" AllowSorting="True" SkinID="StandardGrid" DataSourceID="odsProgramVolume"
            AllowPaging="True" PageSize="30" CssClass="c_smalltext" EmptyDataRowStyle-Font-Size="Medium"
            EmptyDataRowStyle-Font-Bold="true" EmptyDataRowStyle-ForeColor="Red" OnRowCommand="gvProgramVolumeList_RowCommand"
            OnRowDeleted="gvProgramVolumeList_RowDeleted">
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
                <asp:TemplateField ShowHeader="False" ItemStyle-HorizontalAlign="center">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                            ImageUrl="~/images/delete.jpg" AlternateText="Delete" />
                    </ItemTemplate>
                    <HeaderStyle Width="30px" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:BoundField DataField="ProgramID" HeaderText="ProgramID" SortExpression="ProgramID"
                    Visible="false" />
                <asp:TemplateField HeaderText="Year **" SortExpression="YearID" HeaderStyle-HorizontalAlign="Center"
                    HeaderStyle-Width="60px" ItemStyle-Width="60px">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtYY" runat="server" Text='<%# Bind("YearID") %>' MaxLength="4"
                            Width="30px" />
                        <asp:RangeValidator ID="rvYY" runat="server" ErrorMessage="Year values between 1997 to 2030"
                            ControlToValidate="txtYY" MinimumValue="1997" MaximumValue="2030" ValidationGroup="EditInfo"><</asp:RangeValidator>
                        <asp:CompareValidator ID="cvSOPYY" runat="server" ErrorMessage="Year must be greater than or equal to the Program SOP Year."
                            ValueToCompare='<%# lblSOPYY.text %>' ControlToValidate="txtYY" Operator="GreaterThanEqual"
                            Type="Integer" ValidationGroup="EditInfo"><</asp:CompareValidator>
                        <asp:CompareValidator ID="cvEOPYY" runat="server" ErrorMessage="Year must be less than or equal to the Program EOP Year."
                            ValueToCompare='<%# lblEOPYY.text %>' ControlToValidate="txtYY" Operator="LessThanEqual"
                            Type="Integer" ValidationGroup="EditInfo"><</asp:CompareValidator>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("YearID") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                    <FooterTemplate>
                        <asp:TextBox ID="txtYearID" runat="server" MaxLength="4" Width="30px" />
                        <asp:RequiredFieldValidator ID="rfvYYGV" runat="server" ControlToValidate="txtYearID"
                            Display="Dynamic" ErrorMessage="Year is a required field." ValidationGroup="InsertInfo"><</asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cvSOPYYGV" runat="server" ErrorMessage="Year must be greater than or equal to the Program SOP Year."
                            ValueToCompare='<%# lblSOPYY.text %>' ControlToValidate="txtYearID" Operator="GreaterThanEqual"
                            Type="Integer" ValidationGroup="InsertInfo"><</asp:CompareValidator>
                        <asp:CompareValidator ID="cvEOPYYGV" runat="server" ErrorMessage="Year must be less than or equal to the Program EOP Year."
                            ValueToCompare='<%# lblEOPYY.text %>' ControlToValidate="txtYearID" Operator="LessThanEqual"
                            Type="Integer" ValidationGroup="InsertInfo"><</asp:CompareValidator>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Jan Volume" SortExpression="JanVolume">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtJanVolume" runat="server" Text='<%# Bind("JanVolume") %>' Width="60px" />
                        <ajax:FilteredTextBoxExtender ID="ftbJanVolume" runat="server" TargetControlID="txtJanVolume"
                            FilterType="Numbers" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("JanVolume", "{0:n0}") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <asp:TextBox ID="txtJanVol" runat="server" Width="60px" />
                        <ajax:FilteredTextBoxExtender ID="ftbJanVolume" runat="server" TargetControlID="txtJanVol"
                            FilterType="Numbers" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Feb Volume" SortExpression="FebVolume">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtFebVolume" runat="server" Text='<%# Bind("FebVolume") %>' Width="60px" />
                        <ajax:FilteredTextBoxExtender ID="ftbFebVolume" runat="server" TargetControlID="txtFebVolume"
                            FilterType="Numbers" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("FebVolume", "{0:n0}") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <asp:TextBox ID="txtFebVol" runat="server" Width="60px" />
                        <ajax:FilteredTextBoxExtender ID="ftbFebVolume" runat="server" TargetControlID="txtFebVol"
                            FilterType="Numbers" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mar Volume" SortExpression="MarVolume">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtMarVolume" runat="server" Text='<%# Bind("MarVolume") %>' Width="60px" />
                        <ajax:FilteredTextBoxExtender ID="ftbMarVolume" runat="server" TargetControlID="txtMarVolume"
                            FilterType="Numbers" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("MarVolume", "{0:n0}") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <asp:TextBox ID="txtMarVol" runat="server" Width="60px" />
                        <ajax:FilteredTextBoxExtender ID="ftbMarVolume" runat="server" TargetControlID="txtMarVol"
                            FilterType="Numbers" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Apr Volume" SortExpression="AprVolume">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtAprVolume" runat="server" Text='<%# Bind("AprVolume") %>' Width="60px" />
                        <ajax:FilteredTextBoxExtender ID="ftbAprVolume" runat="server" TargetControlID="txtAprVolume"
                            FilterType="Numbers" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label5" runat="server" Text='<%# Bind("AprVolume", "{0:n0}") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <asp:TextBox ID="txtAprVol" runat="server" Width="60px" />
                        <ajax:FilteredTextBoxExtender ID="ftbAprVolume" runat="server" TargetControlID="txtAprVol"
                            FilterType="Numbers" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="May Volume" SortExpression="MayVolume">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtMayVolume" runat="server" Text='<%# Bind("MayVolume") %>' Width="60px" />
                        <ajax:FilteredTextBoxExtender ID="ftbMayVolume" runat="server" TargetControlID="txtMayVolume"
                            FilterType="Numbers" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label6" runat="server" Text='<%# Bind("MayVolume", "{0:n0}") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <asp:TextBox ID="txtMayVol" runat="server" Width="60px" />
                        <ajax:FilteredTextBoxExtender ID="ftbMayVolume" runat="server" TargetControlID="txtMayVol"
                            FilterType="Numbers" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Jun Volume" SortExpression="JunVolume">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtJunVolume" runat="server" Text='<%# Bind("JunVolume") %>' Width="60px" />
                        <ajax:FilteredTextBoxExtender ID="ftbJunVolume" runat="server" TargetControlID="txtJunVolume"
                            FilterType="Numbers" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label7" runat="server" Text='<%# Bind("JunVolume", "{0:n0}") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <asp:TextBox ID="txtJunVol" runat="server" Width="60px" />
                        <ajax:FilteredTextBoxExtender ID="ftbJunVolume" runat="server" TargetControlID="txtJunVol"
                            FilterType="Numbers" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Jul Volume" SortExpression="JulVolume">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtJulVolume" runat="server" Text='<%# Bind("JulVolume") %>' Width="60px" />
                        <ajax:FilteredTextBoxExtender ID="ftbJulVolume" runat="server" TargetControlID="txtJulVolume"
                            FilterType="Numbers" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label8" runat="server" Text='<%# Bind("JulVolume", "{0:n0}") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <asp:TextBox ID="txtJulVol" runat="server" Width="60px" />
                        <ajax:FilteredTextBoxExtender ID="ftbJulVolume" runat="server" TargetControlID="txtJulVol"
                            FilterType="Numbers" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Aug Volume" SortExpression="AugVolume">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtAugVolume" runat="server" Text='<%# Bind("AugVolume") %>' Width="60px" />
                        <ajax:FilteredTextBoxExtender ID="ftbAugVolume" runat="server" TargetControlID="txtAugVolume"
                            FilterType="Numbers" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label9" runat="server" Text='<%# Bind("AugVolume", "{0:n0}") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <asp:TextBox ID="txtAugVol" runat="server" Width="60px" />
                        <ajax:FilteredTextBoxExtender ID="ftbAugVolume" runat="server" TargetControlID="txtAugVol"
                            FilterType="Numbers" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Sep Volume" SortExpression="SepVolume">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtSepVolume" runat="server" Text='<%# Bind("SepVolume") %>' Width="60px" />
                        <ajax:FilteredTextBoxExtender ID="ftbSepVolume" runat="server" TargetControlID="txtSepVolume"
                            FilterType="Numbers" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label10" runat="server" Text='<%# Bind("SepVolume", "{0:n0}") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <asp:TextBox ID="txtSepVol" runat="server" Width="60px" />
                        <ajax:FilteredTextBoxExtender ID="ftbSepVolume" runat="server" TargetControlID="txtSepVol"
                            FilterType="Numbers" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Oct Volume" SortExpression="OctVolume">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtOctVolume" runat="server" Text='<%# Bind("OctVolume") %>' Width="60px" />
                        <ajax:FilteredTextBoxExtender ID="ftbOctVolume" runat="server" TargetControlID="txtOctVolume"
                            FilterType="Numbers" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label11" runat="server" Text='<%# Bind("OctVolume", "{0:n0}") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <asp:TextBox ID="txtOctVol" runat="server" Width="60px" />
                        <ajax:FilteredTextBoxExtender ID="ftbOctVolume" runat="server" TargetControlID="txtOctVol"
                            FilterType="Numbers" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nov Volume" SortExpression="NovVolume">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtNovVolume" runat="server" Text='<%# Bind("NovVolume") %>' Width="60px" />
                        <ajax:FilteredTextBoxExtender ID="ftbNovVolume" runat="server" TargetControlID="txtNovVolume"
                            FilterType="Numbers" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label12" runat="server" Text='<%# Bind("NovVolume", "{0:n0}") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <asp:TextBox ID="txtNovVol" runat="server" Width="60px" />
                        <ajax:FilteredTextBoxExtender ID="ftbNovVolume" runat="server" TargetControlID="txtNovVol"
                            FilterType="Numbers" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Dec Volume" SortExpression="DecVolume">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtDecVolume" runat="server" Text='<%# Bind("DecVolume") %>' Width="60px" />
                        <ajax:FilteredTextBoxExtender ID="ftbDecVolume" runat="server" TargetControlID="txtDecVolume"
                            FilterType="Numbers" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label13" runat="server" Text='<%# Bind("DecVolume", "{0:n0}") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <asp:TextBox ID="txtDecVol" runat="server" Width="60px" />
                        <ajax:FilteredTextBoxExtender ID="ftbDecVolume" runat="server" TargetControlID="txtDecVol"
                            FilterType="Numbers" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Q1Volume" HeaderText="Q1 Volume" SortExpression="Q1Volume"
                    DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center"
                    ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="Q2Volume" HeaderText="Q2 Volume" SortExpression="Q2Volume"
                    DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center"
                    ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="Q3Volume" HeaderText="Q3 Volume" SortExpression="Q3Volume"
                    DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center"
                    ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="Q4Volume" HeaderText="Q4 Volume" SortExpression="Q4Volume"
                    DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center"
                    ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="AnnualVolume" HeaderText="Annual Volume" SortExpression="AnnualVolume"
                    DataFormatString="{0:n0}" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center"
                    ReadOnly="True">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="EFILE_MONTHID" HeaderText="Last E-File Update" SortExpression="EFILE_MONTHID"
                    ItemStyle-HorizontalAlign="CENTER" ReadOnly="True" >
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsProgramVolume" runat="server" InsertMethod="InsertProgramVolume"
            OldValuesParameterFormatString="original_{0}" SelectMethod="GetProgramVolume"
            TypeName="PlatformBLL" UpdateMethod="UpdateProgramVolume" DeleteMethod="DeleteProgramVolume">
            <DeleteParameters>
                <asp:Parameter Name="ProgramID" Type="Int32" />
                <asp:Parameter Name="YearID" Type="Int32" />
                <asp:Parameter Name="original_ProgramID" Type="Int32" />
                <asp:Parameter Name="original_YearID" Type="String" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="JanVolume" Type="Decimal" />
                <asp:Parameter Name="FebVolume" Type="Decimal" />
                <asp:Parameter Name="MarVolume" Type="Decimal" />
                <asp:Parameter Name="AprVolume" Type="Decimal" />
                <asp:Parameter Name="MayVolume" Type="Decimal" />
                <asp:Parameter Name="JunVolume" Type="Decimal" />
                <asp:Parameter Name="JulVolume" Type="Decimal" />
                <asp:Parameter Name="AugVolume" Type="Decimal" />
                <asp:Parameter Name="SepVolume" Type="Decimal" />
                <asp:Parameter Name="OctVolume" Type="Decimal" />
                <asp:Parameter Name="NovVolume" Type="Decimal" />
                <asp:Parameter Name="DecVolume" Type="Decimal" />
                <asp:Parameter Name="original_ProgramID" Type="Int32" />
                <asp:Parameter Name="original_YearID" Type="Int32" />
                <asp:Parameter Name="YearID" Type="Int32" />
            </UpdateParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="ProgramID" QueryStringField="pPgmID" 
                    Type="Int32" DefaultValue="0" />
                <asp:Parameter Name="YearID" Type="Int32" DefaultValue="0" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="ProgramID" Type="Int32" />
                <asp:Parameter Name="YearID" Type="Int32" />
                <asp:Parameter Name="JanVolume" Type="Decimal" />
                <asp:Parameter Name="FebVolume" Type="Decimal" />
                <asp:Parameter Name="MarVolume" Type="Decimal" />
                <asp:Parameter Name="AprVolume" Type="Decimal" />
                <asp:Parameter Name="MayVolume" Type="Decimal" />
                <asp:Parameter Name="JunVolume" Type="Decimal" />
                <asp:Parameter Name="JulVolume" Type="Decimal" />
                <asp:Parameter Name="AugVolume" Type="Decimal" />
                <asp:Parameter Name="SepVolume" Type="Decimal" />
                <asp:Parameter Name="OctVolume" Type="Decimal" />
                <asp:Parameter Name="NovVolume" Type="Decimal" />
                <asp:Parameter Name="DecVolume" Type="Decimal" />
            </InsertParameters>
        </asp:ObjectDataSource>
        <br />
        <asp:ValidationSummary ID="vsEditInfo" runat="server" ShowMessageBox="True" ShowSummary="True"
            Width="498px" ValidationGroup="EditInfo" />
        <asp:ValidationSummary ID="vsInsertInfo" runat="server" ShowMessageBox="True" ShowSummary="True"
            Width="498px" ValidationGroup="InsertInfo" />
        <asp:ObjectDataSource ID="odsVolumeList" runat="server" SelectMethod="GetProgramVolume"
            TypeName="PlatformBLL" InsertMethod="InsertProgramVolume" OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:QueryStringParameter Name="ProgramName" QueryStringField="sPName" Type="String" />
                <asp:QueryStringParameter Name="CSMProgram" QueryStringField="sCSMPN" Type="String" />
                <asp:QueryStringParameter Name="WAFProgram" QueryStringField="sWAFPN" Type="String" />
                <asp:QueryStringParameter Name="Make" QueryStringField="sOEMMF" Type="String" />
                <asp:QueryStringParameter Name="DisplayUGNBusiness" QueryStringField="sDUB" Type="String" />
                <asp:QueryStringParameter Name="DisplayCurrentPlatform" QueryStringField="sDCP" Type="String" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="ProgramName" Type="String" />
                <asp:Parameter Name="CSMProgram" Type="String" />
                <asp:Parameter Name="WAFProgram" Type="String" />
                <asp:Parameter Name="Make" Type="String" />
                <asp:Parameter Name="BegYear" Type="Int32" />
                <asp:Parameter Name="EndYear" Type="Int32" />
                <asp:Parameter Name="UGNBusiness" Type="Boolean" />
                <asp:Parameter Name="CurrentPlatform" Type="Boolean" />
                <asp:Parameter Name="Notes" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
