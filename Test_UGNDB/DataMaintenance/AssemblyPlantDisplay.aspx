<%@ Page Language="VB" MasterPageFile="~/LookUpMasterPage.master" AutoEventWireup="false"
    CodeFile="AssemblyPlantDisplay.aspx.vb" Inherits="DataMaintenance_AssemblyPlantDisplay"
    Title="UGN, Inc.: Assembly Plant Display" MaintainScrollPositionOnPostback="True" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
        Visible="False"></asp:Label>
    <asp:Panel ID="localPanel" runat="server" Width="700px">
        <hr />
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
        <table style="width: 100%; border-bottom-style: groove;" class="sampleStyleC">
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblAssemblyPlantLocation" runat="server" Text="Assembly Plant Location:" />&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblAssembly" runat="server" Text="" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblOEMManufacturer" runat="server" Text="OEM Manufacturer:" />&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblOEMManufacturerVal" runat="server" Text="" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblState" runat="server" Text="State:" />&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblStateVal" runat="server" Text="" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblCountry" runat="server" Text="Label" />&nbsp;
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblCountryVal" runat="server" Text="" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblUGNBusiness" runat="server" Text="UGN Business:" />&nbsp;
                </td>
                <td class="c_textbold" colspan="3">
                    <asp:Label ID="lblUGNBiz" runat="server" Text="" />
                </td>
            </tr>
        </table>
        <br />
        <hr />
        <br />
        <asp:GridView ID="gvAPLOEM" runat="server" AutoGenerateColumns="False" DataKeyNames="APID,OEMModelType,Make,ModelName"
            DataSourceID="odsAPL" AllowPaging="True" AllowSorting="True" PageSize="30" EmptyDataRowStyle-Font-Size="Medium"
            OnRowDataBound="gvAPLOEM_RowDataBound" EmptyDataRowStyle-Font-Bold="true" EmptyDataRowStyle-ForeColor="Red"
            Width="100%" SkinID="StandardGridWOFooter">
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
                <asp:TemplateField HeaderText="Model Name" SortExpression="ModelName" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("ModelName") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Make" SortExpression="Assembly_Plant_Location" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:Label ID="lblMake" runat="server" Text='<%# Bind("Make") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Obsolete" SortExpression="Obsolete" ItemStyle-HorizontalAlign="Center"
                    HeaderStyle-Width="50px" ItemStyle-Width="50px">
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Bind("Obsolete") %>' Enabled="false" />
                    </ItemTemplate>
                    <HeaderStyle Width="50px" />
                    <ItemStyle HorizontalAlign="Center" Width="50px" />
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
                                    BorderColor="White" Width="100%" GridLines="None">
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
                                        <asp:BoundField DataField="BegProduction" HeaderText="Beg Production" SortExpression="BegProduction"
                                            HeaderStyle-HorizontalAlign="left" />
                                        <asp:BoundField DataField="EndProduction" HeaderText="End Production" SortExpression="EndProduction"
                                            HeaderStyle-HorizontalAlign="left" />
                                    </Columns>
                                </asp:GridView>
                                <asp:ObjectDataSource ID="odsAPLPartOEM" runat="server" SelectMethod="GetPartNoByOEM"
                                    TypeName="AssemblyPlantOEMBLL" OldValuesParameterFormatString="original_{0}">
                                    <SelectParameters>
                                        <asp:QueryStringParameter DefaultValue="" Name="APID" QueryStringField="pAPID" Type="Int32" />
                                        <asp:QueryStringParameter Name="ModelName" QueryStringField="pMName" Type="String" />
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
        <asp:ObjectDataSource ID="odsAPL" runat="server" SelectMethod="GetAssemblyPlantOEM"
            TypeName="AssemblyPlantOEMBLL" OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:QueryStringParameter DefaultValue="0" Name="APID" QueryStringField="pAPID" 
                    Type="Int32" />
                <asp:QueryStringParameter Name="ModelName" QueryStringField="pMName" Type="String" />
                <asp:Parameter Name="PlatformID" Type="Int32" DefaultValue="0" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
