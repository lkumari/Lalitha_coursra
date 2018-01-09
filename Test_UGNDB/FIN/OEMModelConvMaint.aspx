<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="OEMModelConvMaint.aspx.vb" Inherits="OEM_Model_Conv_Maint" Title="Untitled Page"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
        Width="568px" Visible="False"></asp:Label><br />
    <asp:Panel ID="localPanel" runat="server">
        <hr />
        <asp:Panel ID="OEMPanel" runat="server" CssClass="collapsePanelHeader" Width="800px">
            <asp:Image ID="imgOEM" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                Height="12px" />&nbsp;
            <asp:Label ID="lblOEM" runat="server" CssClass="c_textbold">Filter list below:</asp:Label>
        </asp:Panel>
        <asp:Panel ID="OEMContentPanel" runat="server" CssClass="collapsePanelHeader">
            <table>
                <tr>
                    <td class="p_textxsmall">
                        OEM:
                    </td>
                    <td>
                        <asp:DropDownList ID="sddOEM" runat="server" CssClass="c_textxsmall" />
                    </td>
                    <td class="p_textxsmall">
                        Customer:
                    </td>
                    <td>
                        <asp:DropDownList ID="sddCABBV" runat="server" CssClass="c_textxsmall" />
                    </td>
                </tr>
                <tr>
                    <td class="p_textxsmall">
                        Sold To:
                    </td>
                    <td>
                        <asp:DropDownList ID="sddSoldTo" runat="server" CssClass="c_textxsmall" />
                    </td>
                    <td class="p_textxsmall">
                        Destination:
                    </td>
                    <td>
                        <asp:DropDownList ID="sddDABBV" runat="server" CssClass="c_textxsmall" />
                    </td>
                </tr>
                <tr>
                    <td class="p_textxsmall">
                        Alt OEM Manufacturer:
                    </td>
                    <td>
                        <asp:DropDownList ID="sddOEMMfg" runat="server" CssClass="c_textxsmall" />
                    </td>
                    <td class="p_textxsmall">
                        Part Field:
                    </td>
                    <td>
                        <asp:DropDownList ID="sddPartField" runat="server" CssClass="c_textxsmall">
                            <asp:ListItem Selected="True"></asp:ListItem>
                            <asp:ListItem>CPART</asp:ListItem>
                            <asp:ListItem>PARTNO</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="height: 27px">
                    </td>
                    <td style="height: 27px" colspan="3">
                        <asp:Button ID="btnSearch" runat="server" Text="Submit" CausesValidation="false" />
                        <asp:Button ID="btnReset1" runat="server" CausesValidation="False" Text="Reset" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <ajax:CollapsiblePanelExtender ID="OEMExtender" runat="server" TargetControlID="OEMContentPanel"
            ExpandControlID="OEMPanel" CollapseControlID="OEMPanel" Collapsed="FALSE" TextLabelID="lblOEM"
            ExpandedText="Filter list below:" CollapsedText="Filter list below:" ImageControlID="imgOEM"
            CollapsedImage="~/images/expand_blue.jpg" ExpandedImage="~/images/collapse_blue.jpg"
            SuppressPostBack="true">
        </ajax:CollapsiblePanelExtender>
        <hr />
        <asp:Panel ID="FEPanel" runat="server" CssClass="collapsePanelHeader" Width="800px">
            <asp:Image ID="imgFE" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                Height="12px" />&nbsp;
            <asp:Label ID="lblFE" runat="server" Text="Label" CssClass="c_textbold">Enter Conversion below:</asp:Label>
        </asp:Panel>
        <asp:Panel ID="FEContentPanel" runat="server" CssClass="collapsePanel">
            <asp:Label ID="Label2" runat="server"><i>An asterick (<asp:Label ID="Label11" runat="server"
                Font-Bold="True" ForeColor="Red" Text="* " />) denotes a required field.</i></asp:Label><br />
            <asp:Label ID="lblRowID" runat="server" Text="" CssClass="c_text" ForeColor="Red"
                Font-Bold="True" Font-Overline="False" Font-Size="Larger" Font-Underline="False"></asp:Label>
            <table>
                <tr>
                    <td class="p_textxsmall">
                        <asp:Label ID="lblOEMRqrd" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                        &nbsp;OEM:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddOEMValidator" runat="server" CssClass="c_textxsmall">
                            <asp:ListItem Selected="True"></asp:ListItem>
                            <asp:ListItem Value="=">Equal To</asp:ListItem>
                            <asp:ListItem Value="&lt;&gt;">Not Equal To</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddOEM" runat="server" CssClass="c_textxsmall" />
                        <asp:RequiredFieldValidator ID="rfvOEM" runat="server" ControlToValidate="ddOEM"
                            ErrorMessage="OEM is a required field." Font-Bold="False" ValidationGroup="vsAddEdit"><</asp:RequiredFieldValidator>
                    </td>
                    <td class="p_textxsmall">
                        <asp:Label ID="lblAltOEMMfgRqrd" runat="server" Font-Bold="True" ForeColor="Red"
                            Text="* " />
                        &nbsp;Select an Alternate OEM Mfg:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddOEMMfg" runat="server" CssClass="c_textxsmall" />
                        <asp:RequiredFieldValidator ID="rfvAltOEMMgg" runat="server" ControlToValidate="ddOEMMfg"
                            ErrorMessage="Alternate OEM Mfg is a required field." Font-Bold="False" ValidationGroup="vsAddEdit"><</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="p_textxsmall">
                        Customer:
                    </td>
                    <td class="c_textxsmall">
                        <asp:DropDownList ID="ddCabbvValidator" runat="server" CssClass="c_textxsmall">
                            <asp:ListItem Selected="True"></asp:ListItem>
                            <asp:ListItem Value="=">Equal To</asp:ListItem>
                            <asp:ListItem Value="&lt;&gt;">Not Equal To</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddCustomer" runat="server" CssClass="c_textxsmall" />
                    </td>
                    <td class="p_textxsmall">
                        Sold To:
                    </td>
                    <td class="c_textxsmall">
                        <asp:DropDownList ID="ddSoldToValidator" runat="server" CssClass="c_textxsmall">
                            <asp:ListItem Selected="True"></asp:ListItem>
                            <asp:ListItem Value="=">Equal To</asp:ListItem>
                            <asp:ListItem Value="&lt;&gt;">Not Equal To</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddSoldTo" runat="server" CssClass="c_textxsmall" />
                    </td>
                </tr>
                <tr>
                    <td class="p_textxsmall">
                        Select Part Field for OEM Model:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddPartField" runat="server" CssClass="c_textxsmall">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem Selected="True">CPART</asp:ListItem>
                            <asp:ListItem>PARTNO</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="p_textxsmall">
                        Substring Part for OEM Model: Start Location:
                    </td>
                    <td class="c_textxsmall">
                        <asp:TextBox ID="txtCPartLoc1" MaxLength="2" Width="40px" runat="server" CssClass="c_textxsmall" />
                        <ajax:FilteredTextBoxExtender ID="fbtCPartLoc1" runat="server" TargetControlID="txtCPartLoc1"
                            FilterType="Numbers" />
                        by # of Chars:
                        <asp:TextBox ID="txtCPartLoc2" MaxLength="2" Width="40px" runat="server" CssClass="c_textxsmall" />
                        <ajax:FilteredTextBoxExtender ID="fbtCPartLoc2" runat="server" TargetControlID="txtCPartLoc2"
                            FilterType="Numbers" />
                    </td>
                </tr>
                <tr>
                    <td class="p_textxsmall">
                        Select Part Field for Part Suffix:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddPartField2" runat="server" CssClass="c_textxsmall">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem Selected="True">CPART</asp:ListItem>
                            <asp:ListItem>PARTNO</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="p_textxsmall">
                        Substring Part for Part Suffix: Start Location:
                    </td>
                    <td class="c_textxsmall">
                        <asp:TextBox ID="txtPartSuffixLoc1" MaxLength="2" Width="40px" runat="server" CssClass="c_textxsmall" />
                        <ajax:FilteredTextBoxExtender ID="ftbPartSuffixLoc1" runat="server" TargetControlID="txtPartSuffixLoc1"
                            FilterType="Numbers" />
                        by # of Chars:
                        <asp:TextBox ID="txtPartSuffixLoc2" MaxLength="2" Width="40px" runat="server" CssClass="c_textxsmall" />
                        <ajax:FilteredTextBoxExtender ID="ftbPartSuffixLoc2" runat="server" TargetControlID="txtPartSuffixLoc2"
                            FilterType="Numbers" />
                    </td>
                </tr>
                <tr>
                    <td class="p_textxsmall">
                        Default Model Type:
                    </td>
                    <td class="c_textxsmall" colspan="3">
                        <asp:TextBox ID="txtMiscValue" MaxLength="15" runat="server" class="c_textxsmall" />
                        <ajax:FilteredTextBoxExtender ID="ftpMiscValue" runat="server" TargetControlID="txtMiscValue"
                            FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890" />
                    </td>

                </tr>
                <tr>
                    <td class="p_textxsmall" valign="top">
                        Additional SQL Query:
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtSQLQuery" runat="server" MaxLength="400" Rows="3" TextMode="MultiLine"
                            Width="600px" CssClass="c_textxsmall" />
                        <asp:Label ID="lblSQLQuery" runat="server" Font-Bold="True" ForeColor="Red" CssClass="c_textxsmall" />&nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="p_textxsmall" valign="top">
                        Notes:
                    </td>
                    <td class="c_textxsmall" colspan="3">
                        <asp:TextBox ID="txtNotes" MaxLength="200" Width="600px" runat="server" CssClass="c_textxsmall" />
                        <asp:Label ID="lblNotes" runat="server" Font-Bold="True" ForeColor="Red" CssClass="c_textxsmall" />&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="height: 27px">
                    </td>
                    <td style="height: 27px" colspan="3">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CausesValidation="true" ValidationGroup="vsAddEdit" />
                        <asp:Button ID="btnReset" runat="server" CausesValidation="False" Text="Reset" />
                        <br />
                        <br />
                        <asp:Label ID="lblMessageView4" runat="server" Font-Bold="True" ForeColor="Red" Height="16px"
                            Text="Label" Visible="False" Width="368px" Font-Size="Small"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:ValidationSummary ID="vsAddEdit" runat="server" ShowMessageBox="True" ValidationGroup="vsAddEdit"
                Width="316px" />
            <ajax:CascadingDropDown ID="cddOEM" runat="server" TargetControlID="ddOEM" Category="OEM"
                PromptText="Please select an OEM Code." LoadingText="[Loading OEM Code...]" ServicePath="~/WS/VehicleCDDService.asmx"
                ServiceMethod="GetOEM" />
            <ajax:CascadingDropDown ID="cddCustomer" runat="server" TargetControlID="ddCustomer"
                ParentControlID="ddOEM" Category="CABBV" PromptText="Select a Customer Abbreviation"
                LoadingText="[Loading Customer Abbreviations...]" ServicePath="~/WS/GeneralCDDService.asmx"
                ServiceMethod="GetCABBVbyOEM" />
            <ajax:CascadingDropDown ID="cddSoldTo" runat="server" TargetControlID="ddSoldTo"
                ParentControlID="ddCustomer" Category="SoldTo" PromptText="Select a Sold To Customer"
                LoadingText="[Loading Sold To Customers...]" ServicePath="~/WS/GeneralCDDService.asmx"
                ServiceMethod="GetSOLDTObyCOMPNYbyCABBVbyOEM" />
            <ajax:CascadingDropDown ID="cddOEMMfg" runat="server" TargetControlID="ddOEMMfg"
                ParentControlID="ddOEM" Category="OEMMfg" PromptText="Please select an OEM Manufacturer."
                LoadingText="[Loading OEM Manufacturer...]" ServicePath="~/WS/GeneralCDDService.asmx"
                ServiceMethod="GetOEMMfgByOEM" />
            <%--GetOEMMfg--%></asp:Panel>
        <ajax:CollapsiblePanelExtender ID="FEExtender" runat="server" TargetControlID="FEContentPanel"
            ExpandControlID="FEPanel" CollapseControlID="FEPanel" Collapsed="FALSE" TextLabelID="lblFE"
            ExpandedText="Enter Conversion below:" CollapsedText="Enter Conversion below:"
            ImageControlID="imgFE" CollapsedImage="~/images/expand_blue.jpg" ExpandedImage="~/images/collapse_blue.jpg"
            SuppressPostBack="true">
        </ajax:CollapsiblePanelExtender>
        <hr />
        <br />
        <asp:Label ID="PagingInformation" runat="server" Text="" /><asp:DropDownList ID="PageList"
            runat="server" CssClass="c_textxsmall" AutoPostBack="true" OnSelectedIndexChanged="PageList_SelectedIndexChanged" />
        <br />
        <asp:GridView ID="gvOEMModelConv" runat="server" AutoGenerateColumns="False" DataKeyNames="RowID"
            OnRowDataBound="gvOEMModelConv_RowDataBound" DataSourceID="odsOEMModelConv" EmptyDataText="No data available for grid view. Use fields above to add new entry."
            Width="100%" CssClass="c_smalltext" PageSize="50" AllowSorting="True" AllowPaging="True">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <EmptyDataRowStyle Wrap="False" />
            <Columns>
                <asp:TemplateField HeaderText="Row ID" SortExpression="RowID" ItemStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <% If ViewState("Admin") = "true" Then%>
                        <asp:HyperLink ID="lblRowID" runat="server" Font-Underline="true" Text='<%# Bind("RowID") %>'
                            NavigateUrl='<%# "OEMModelConvMaint.aspx?sOEM=" & ViewState("sOEM") & "&sCABBV=" & ViewState("sCABBV") & "&sSoldTo=" & ViewState("sSoldTo") & "&sDABBV=" & ViewState("sDABBV") & "&sPartField=" & ViewState("sPartField") & "&sOEMMfg=" & ViewState("sOEMMfg") & "&pRowID=" & DataBinder.Eval (Container.DataItem,"RowID").tostring%>' />
                        <% Else%>
                        <asp:Label ID="lblRow" runat="server" Text='<%# Bind("RowID") %>' />
                        <% End If%>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:BoundField DataField="OEMStmt" HeaderText="OEM" SortExpression="OEMStmt" ItemStyle-Wrap="false">
                    <ItemStyle Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="ALTOEMManufacturer" HeaderText="OEM Manufacturer" SortExpression="ALTOEMManufacturer" />
                <asp:BoundField DataField="CABBVStmt" HeaderText="CABBV" SortExpression="CABBV" ItemStyle-Wrap="false">
                    <ItemStyle Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="SOLDTOStmt" HeaderText="Sold To" SortExpression="SOLDTO" />
                <asp:BoundField DataField="CPARTStmt" HeaderText="OEM Model Substring" SortExpression="CPARTStmt"
                    ItemStyle-Width="200px">
                    <ItemStyle Width="200px" />
                </asp:BoundField>
                 <asp:BoundField DataField="PartSuffixStmt" HeaderText="Part Suffix Substring" SortExpression="PartSuffixStmt"
                    ItemStyle-Width="200px">
                    <ItemStyle Width="200px" />
                </asp:BoundField>
                <asp:BoundField DataField="MiscStmt" HeaderText="Default Model Type" SortExpression="MiscStmt"
                    ItemStyle-Width="100px">
                    <ItemStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="SQLQuery" HeaderText="Additional SQL Query" SortExpression="SQLQuery" />
                <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" />
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                            ImageUrl="~/images/delete.jpg" ToolTip="Delete" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsOEMModelConv" runat="server" SelectMethod="GetOEMModelConv"
            OldValuesParameterFormatString="original_{0}" 
            TypeName="ForecastExceptionBLL">
            <SelectParameters>
                <asp:Parameter DefaultValue="0" Name="RowID" Type="Int32" />
                <asp:QueryStringParameter DefaultValue="" Name="OEM" QueryStringField="sOEM" 
                    Type="String" />
                <asp:QueryStringParameter Name="CABBV" QueryStringField="sCABBV" Type="String" />
                <asp:QueryStringParameter Name="SoldTo" QueryStringField="sSoldTo" Type="Int32" 
                    DefaultValue="0" />
                <asp:QueryStringParameter Name="DABBV" QueryStringField="sDABBV" Type="String" />
                <asp:QueryStringParameter Name="PartField" QueryStringField="sPartField" Type="String" />
                <asp:QueryStringParameter Name="OEMMfg" QueryStringField="sOEMMfg" 
                    Type="String" DefaultValue="" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
