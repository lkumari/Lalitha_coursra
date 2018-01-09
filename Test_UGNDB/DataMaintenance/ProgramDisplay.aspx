<%@ Page Language="VB" MasterPageFile="~/LookUpMasterPage.master" AutoEventWireup="false"
    CodeFile="ProgramDisplay.aspx.vb" Inherits="DataMaintenance_ProgramDisplay" Title="UGN, Inc.: Detailed Platform/Program Information"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="1050px">
        <hr />
        <table>
            <tr>
                <td>
                    <asp:Button ID="btnClose" runat="server" Text="Close Window" />
                </td>
            </tr>
        </table>
        <asp:Label ID="lblErrors" runat="server" Width="818px" SkinID="MessageLabelSkin"></asp:Label>
        <asp:Label ID="lblMessage" runat="server" Width="818px" SkinID="MessageLabelSkin"></asp:Label>
        <table style="width: 1050px; border-bottom-style: groove;" class="sampleStyleC">
            <tr>
                <td class="p_textbold" style="color: red">
                    <asp:Label ID="lblPlatform" runat="server" Text="Platform:" />&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblPlatformName" runat="server" Text="" />
                </td>
                <td class="p_textbold" style="color: red">
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
            </tr>
            <tr>
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
                <td class="p_text">
                    <asp:Label ID="lblServiceUntil" runat="server" Text="Service Until:" />&nbsp;
                </td>
                <td class="c_textbold" colspan="3">
                    <asp:Label ID="lblSrvYrs" runat="server" Text="" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblPlatformNotes" runat="server" Text="Platform Notes:" />
                </td>
                <td colspan="7" class="c_textbold">
                    <asp:Label ID="lblPlatformNotesVal" runat="server" Text="" />
                </td>
            </tr>
            <tr>
                <td colspan="8">
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
                <td class="p_textbold" style="color: red">
                    <asp:Label ID="lblMake" runat="server" Text="Make:" />&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblMakeVal" runat="server" Text="" />
                </td>
                <td class="p_textbold" style="color: red">
                    <asp:Label ID="lblModel" runat="server" Text="Model:" />&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblModelName" runat="server" Text="" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblVehicleType" runat="server" Text="Vehicle Type:" />&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblVehicleTypeVal" runat="server" Text="" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblProgramSOP" runat="server" Text="Program SOP:" />&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblSOP" runat="server" Text="" />
                    <asp:Label ID="lblSOPMM" runat="server" Text="" Visible="false" />
                    <asp:Label ID="lblSOPYY" runat="server" Text="" Visible="false" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblProgramEOP" runat="server" Text="Program EOP:" />&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblEOP" runat="server" Text="" />
                    <asp:Label ID="lblEOPMM" runat="server" Text="" Visible="false" />
                    <asp:Label ID="lblEOPYY" runat="server" Text="" Visible="false" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblProgramServiceEOP" runat="server" Text="Program Service EOP:" />&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblSrvEOP" runat="server" Text="" />
                    <asp:Label ID="lblSrvEOPMM" runat="server" Text="" Visible="false" />
                    <asp:Label ID="lblSrvEOPYY" runat="server" Text="" Visible="false" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblUGNBusiness2" runat="server" Text="UGN Business:" />&nbsp;
                </td>
                <td class="c_textbold" colspan="3">
                    <asp:Label ID="lblUGNBiz2" runat="server" Text="" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblStatus" runat="server" Text="Status:" />&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblRecStatus" runat="server" Text="" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblProgramNotes" runat="server" Text="Program Notes:" />
                </td>
                <td colspan="5" class="c_textbold">
                    <asp:Label ID="lblProgramNotesVal" runat="server" Text="" />
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="p_textbold" style="color: red">
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
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    &nbsp;
                </td>
            </tr>
        </table>
        <asp:Label ID="lblRaiseError" runat="server" Text="" Visible="false" SkinID="MessageLabelSkin" /><br />
        <br />
        <asp:Panel ID="TCPanel" runat="server" CssClass="collapsePanelHeader" BackColor="#ddffdd">
            <asp:Image ID="imgTC" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                Height="12px" />&nbsp;
            <asp:Label ID="lblTC" runat="server" CssClass="c_textbold" Text="VEHICLE VOLUME(S) FROM IHS:" />
        </asp:Panel>
        <asp:Panel ID="TCContentPanel" runat="server" CssClass="collapsePanel">
            <i><font style="background-color: Yellow">Row highlighted in Yellow indicates current
                year.</font></i>
            <asp:GridView ID="gvProgramVolumeList" runat="server" AutoGenerateColumns="False"
                DataKeyNames="ProgramID,YearID" AllowSorting="True" DataSourceID="odsProgramVolume"
                AllowPaging="True" PageSize="30" EmptyDataRowStyle-Font-Size="Medium" EmptyDataRowStyle-Font-Bold="true"
                EmptyDataRowStyle-ForeColor="Red" OnRowCommand="gvProgramVolumeList_RowCommand"
                OnRowDeleted="gvProgramVolumeList_RowDeleted" CssClass="c_text" SkinID="StandardGridWOFooter">
                <EmptyDataRowStyle Font-Bold="True" Font-Size="Medium" ForeColor="Red" />
                <Columns>
                    <asp:BoundField DataField="ProgramID" HeaderText="ProgramID" SortExpression="ProgramID"
                        Visible="false" />
                    <asp:TemplateField HeaderText="Year" SortExpression="YearID" HeaderStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="60px" ItemStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("YearID") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Jan Volume" SortExpression="JanVolume">
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("JanVolume", "{0:n0}") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Feb Volume" SortExpression="FebVolume">
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("FebVolume", "{0:n0}") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Mar Volume" SortExpression="MarVolume">
                        <ItemTemplate>
                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("MarVolume", "{0:n0}") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Apr Volume" SortExpression="AprVolume">
                        <ItemTemplate>
                            <asp:Label ID="Label5" runat="server" Text='<%# Bind("AprVolume", "{0:n0}") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="May Volume" SortExpression="MayVolume">
                        <ItemTemplate>
                            <asp:Label ID="Label6" runat="server" Text='<%# Bind("MayVolume", "{0:n0}") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Jun Volume" SortExpression="JunVolume">
                        <ItemTemplate>
                            <asp:Label ID="Label7" runat="server" Text='<%# Bind("JunVolume", "{0:n0}") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Jul Volume" SortExpression="JulVolume">
                        <ItemTemplate>
                            <asp:Label ID="Label8" runat="server" Text='<%# Bind("JulVolume", "{0:n0}") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Aug Volume" SortExpression="AugVolume">
                        <ItemTemplate>
                            <asp:Label ID="Label9" runat="server" Text='<%# Bind("AugVolume", "{0:n0}") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sep Volume" SortExpression="SepVolume">
                        <ItemTemplate>
                            <asp:Label ID="Label10" runat="server" Text='<%# Bind("SepVolume", "{0:n0}") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Oct Volume" SortExpression="OctVolume">
                        <ItemTemplate>
                            <asp:Label ID="Label11" runat="server" Text='<%# Bind("OctVolume", "{0:n0}") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nov Volume" SortExpression="NovVolume">
                        <ItemTemplate>
                            <asp:Label ID="Label12" runat="server" Text='<%# Bind("NovVolume", "{0:n0}") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Dec Volume" SortExpression="DecVolume">
                        <ItemTemplate>
                            <asp:Label ID="Label13" runat="server" Text='<%# Bind("DecVolume", "{0:n0}") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" />
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
                    <asp:BoundField DataField="Efile_Monthid" HeaderText="Last E-File Update" SortExpression="Efile_MonthID"
                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="left" ReadOnly="True">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsProgramVolume" runat="server" OldValuesParameterFormatString="original_{0}"
                SelectMethod="GetProgramVolume" TypeName="PlatformBLL">
                <SelectParameters>
                    <asp:QueryStringParameter Name="ProgramID" QueryStringField="pPgmID" 
                        Type="Int32" DefaultValue="0" />
                    <asp:Parameter Name="YearID" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </asp:Panel>
        <ajax:CollapsiblePanelExtender ID="TCExtender" runat="server" TargetControlID="TCContentPanel"
            ExpandControlID="TCPanel" CollapseControlID="TCPanel" Collapsed="FALSE" TextLabelID="lblTC"
            ExpandedText="VEHICLE VOLUMES FROM IHS:" CollapsedText="VEHICLE VOLUMES FROM IHS:"
            ImageControlID="imgTC" CollapsedImage="~/images/expand_blue.jpg" ExpandedImage="~/images/collapse_blue.jpg"
            SuppressPostBack="true" ScrollContents="true">
        </ajax:CollapsiblePanelExtender>
        <br />
        <br />
        <asp:TextBox ID="txtAPID" runat="server" Visible="false" />
        <% If Session("sAPID") <> Nothing Then%>
        <asp:Panel ID="OEMPanel" runat="server" CssClass="collapsePanelHeader" BackColor="#ddffdd">
            <asp:Image ID="imgOEMPanel" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                Height="12px" />&nbsp;
            <asp:Label ID="lblOEMPanel" runat="server" CssClass="c_textbold" Text="PART NUMBER(S) BY OEM MODEL TYPES:" />
        </asp:Panel>
        <asp:Panel ID="OEMContentPanel" runat="server" CssClass="collapsePanel">
            <asp:GridView ID="gvAPLOEM" runat="server" AutoGenerateColumns="False" DataKeyNames="APID,OEMModelType,Make,ModelName"
                DataSourceID="odsAPL" AllowPaging="True" AllowSorting="True" PageSize="100" EmptyDataRowStyle-Font-Size="Medium"
                OnRowDataBound="gvAPLOEM_RowDataBound" EmptyDataRowStyle-Font-Bold="true" EmptyDataRowStyle-ForeColor="Red"
                Width="80%" CssClass="c_textbold">
                <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#CCCCCC" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <EmptyDataTemplate>
                    No records found.
                </EmptyDataTemplate>
                <EmptyDataRowStyle Font-Bold="True" Font-Size="Medium" ForeColor="Red" />
                <Columns>
                    <asp:BoundField DataField="APID" HeaderText="APID" SortExpression="APID" Visible="false" />
                    <asp:TemplateField HeaderText="OEM Model Type" SortExpression="OEMModelType" ItemStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="50px" ItemStyle-Width="50px">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("OEMModelType") %>' />
                        </ItemTemplate>
                        <HeaderStyle Width="50px" />
                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Model Name" SortExpression="ModelName">
                        <ItemTemplate>
                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("ModelName") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Make" SortExpression="Assembly_Plant_Location">
                        <ItemTemplate>
                            <asp:Label ID="lblMake" runat="server" Text='<%# Bind("Make") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Obsolete" SortExpression="Obsolete">
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Bind("Obsolete") %>' Enabled="false" />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="comboUpdateInfo" HeaderText="Last Updated By" ReadOnly="True"
                        Visible="false" SortExpression="comboUpdateInfo" HeaderStyle-HorizontalAlign="left">
                        <HeaderStyle HorizontalAlign="Left" Width="30px" />
                        <ItemStyle Width="30px" />
                    </asp:BoundField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td colspan="3">
                                    <asp:GridView ID="gvAPLPartOEM" runat="server" AutoGenerateColumns="False" DataKeyNames="OEMModelType,PARTNO,CPART,COMPNY,PRCCDE"
                                        DataSourceID="odsAPLPartOEM" AllowPaging="True" AllowSorting="True" PageSize="100"
                                        BorderColor="White" Width="100%" GridLines="None" CssClass="c_text">
                                        <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                        <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                        <EditRowStyle BackColor="#CCCCCC" />
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <EmptyDataTemplate>
                                            No Part(s) found.
                                        </EmptyDataTemplate>
                                        <EmptyDataRowStyle Font-Bold="True" Font-Size="small" ForeColor="Red" />
                                        <Columns>
                                            <asp:BoundField DataField="OEMModelType" HeaderText="OEM" SortExpression="OEMModelType"
                                                Visible="false" />
                                            <asp:BoundField DataField="PARTNO" HeaderText="Internal Part No" SortExpression="PARTNO"
                                                HeaderStyle-HorizontalAlign="left" />
                                            <asp:BoundField DataField="CPART" HeaderText="Customer Part No" SortExpression="CPART"
                                                HeaderStyle-HorizontalAlign="left" />
                                            <asp:BoundField DataField="PRCCDE" HeaderText="Price" SortExpression="PRCCDE" HeaderStyle-HorizontalAlign="center"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="PriceCodeName" HeaderText="Code" SortExpression="PriceCodeName"
                                                HeaderStyle-HorizontalAlign="left" />
                                            <asp:BoundField DataField="UGNFacilityName" HeaderText="UGN Facility" SortExpression="UGNFacilityName"
                                                HeaderStyle-HorizontalAlign="left" />
                                            <%--<asp:BoundField DataField="BegProduction" HeaderText="Beg Production" SortExpression="BegProduction"
                                                HeaderStyle-HorizontalAlign="left" />
                                            <asp:BoundField DataField="EndProduction" HeaderText="End Production" SortExpression="EndProduction"
                                                HeaderStyle-HorizontalAlign="left" />--%>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:ObjectDataSource ID="odsAPLPartOEM" runat="server" SelectMethod="GetPartNoByOEM"
                                        TypeName="AssemblyPlantOEMBLL" OldValuesParameterFormatString="original_{0}">
                                        <SelectParameters>
                                            <asp:SessionParameter SessionField="sAPID" Name="APID" Type="Int32" />
                                            <asp:SessionParameter SessionField="sMname" Name="ModelName" Type="String" />
                                            <asp:Parameter Name="OEMModelType" Type="String" />
                                            <asp:Parameter Name="PARTNO" Type="String" />
                                            <asp:Parameter Name="CPART" Type="String" />
                                            <asp:Parameter Name="COMPNY" Type="String" />
                                            <asp:Parameter Name="PRCCDE" Type="String" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsAPL" runat="server" TypeName="AssemblyPlantOEMBLL" SelectMethod="GetAssemblyPlantOEM"
                OldValuesParameterFormatString="original_{0}">
                <SelectParameters>
                    <asp:SessionParameter SessionField="sAPID" Name="APID" Type="Int32" 
                        DefaultValue="0" />
                    <asp:Parameter Name="ModelName" Type="String" DefaultValue="" />
                    <asp:QueryStringParameter DefaultValue="0" Name="PlatformID" 
                        QueryStringField="pPlatID" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </asp:Panel>
        <ajax:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="OEMContentPanel"
            ExpandControlID="OEMPanel" CollapseControlID="OEMPanel" Collapsed="FALSE" TextLabelID="lblOEMPanel"
            ExpandedText="PART NUMBER(S) BY OEM MODEL TYPES:" CollapsedText="PART NUMBER(S) BY OEM MODEL TYPES:"
            ImageControlID="imgOEMPanel" CollapsedImage="~/images/expand_blue.jpg" ExpandedImage="~/images/collapse_blue.jpg"
            SuppressPostBack="true" ScrollContents="true" ExpandedSize="300">
        </ajax:CollapsiblePanelExtender>
        <% End If%>
        <br />
        <br />
        <table>
            <tr>
                <td class="c_textbold" style="font-size: medium">
                    <asp:Button ID="btnTop" runat="server" Text="Go to Top" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
