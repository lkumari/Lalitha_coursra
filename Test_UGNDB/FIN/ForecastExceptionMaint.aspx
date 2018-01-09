<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="ForecastExceptionMaint.aspx.vb" Inherits="Forecast_Exception_Maint"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
        Width="568px" Visible="False"></asp:Label><br />
    <asp:Panel ID="localPanel" runat="server">
        <hr />
        <asp:Panel ID="FEPanel" runat="server" CssClass="collapsePanelHeader" Width="680px">
            <asp:Image ID="imgFE" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                Height="12px" />&nbsp;
            <asp:Label ID="lblFE" runat="server" Text="Label" CssClass="c_textbold">Enter Exception below:</asp:Label>
        </asp:Panel>
        <asp:Panel ID="FEContentPanel" runat="server" CssClass="collapsePanel">
            <asp:Label ID="lblRowID" runat="server" Text="" CssClass="c_text" ForeColor="Red"
                Font-Bold="True" Font-Overline="False" Font-Size="Larger" Font-Underline="False"></asp:Label>
            <table >
                <tr>
                    <td class="p_smalltext">
                        Company:
                    </td>
                    <td class="c_smalltext">
                        <asp:DropDownList ID="ddCompnyValidator" runat="server">
                            <asp:ListItem Selected="True"></asp:ListItem>
                            <asp:ListItem Value="=">Equal To</asp:ListItem>
                            <asp:ListItem Value="&lt;&gt;">Not Equal To</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddUGNfacility" runat="server" AutoPostBack="True" />
                    </td>
                    <td class="p_smalltext">
                        OEM:
                    </td>
                    <td class="c_smalltext">
                        <asp:DropDownList ID="ddOEMValidator" runat="server">
                            <asp:ListItem Selected="True"></asp:ListItem>
                            <asp:ListItem Value="=">Equal To</asp:ListItem>
                            <asp:ListItem Value="&lt;&gt;">Not Equal To</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddOEM" runat="server" AutoPostBack="True" />
                    </td>
                    <td class="p_smalltext">
                        Customer:
                    </td>
                    <td class="c_smalltext">
                        <asp:DropDownList ID="ddCabbvValidator" runat="server">
                            <asp:ListItem Selected="True"></asp:ListItem>
                            <asp:ListItem Value="=">Equal To</asp:ListItem>
                            <asp:ListItem Value="&lt;&gt;">Not Equal To</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddCustomer" runat="server" AutoPostBack="True" />
                    </td>
                </tr>
                <tr>
                    <td class="p_smalltext">
                        Sold To:
                    </td>
                    <td class="c_smalltext">
                        <asp:DropDownList ID="ddSoldToValidator" runat="server">
                            <asp:ListItem Selected="True"></asp:ListItem>
                            <asp:ListItem Value="=">Equal To</asp:ListItem>
                            <asp:ListItem Value="&lt;&gt;">Not Equal To</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddSoldTo" runat="server" />
                    </td>
                    <td class="p_smalltext">
                        Destination:
                    </td>
                    <td class="c_smalltext">
                        <asp:DropDownList ID="ddDabbvValidator" runat="server">
                            <asp:ListItem Selected="True"></asp:ListItem>
                            <asp:ListItem Value="=">Equal To</asp:ListItem>
                            <asp:ListItem Value="&lt;&gt;">Not Equal To</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddDestination" runat="server" />
                    </td>
                    <td class="p_smalltext">
                        Part Number:
                    </td>
                    <td class="c_smalltext">
                        <asp:DropDownList ID="ddPartNoValidator" runat="server">
                            <asp:ListItem Selected="True"></asp:ListItem>
                            <asp:ListItem Value="=">Equal To</asp:ListItem>
                            <asp:ListItem Value="&lt;&gt;">Not Equal To</asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="txtPartNo" MaxLength="15" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="p_smalltext">
                        Transaction Type:
                    </td>
                    <td class="c_smalltext">
                        <asp:DropDownList ID="ddTrnTyp" runat="server">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>830</asp:ListItem>
                            <asp:ListItem>862</asp:ListItem>
                            <asp:ListItem>850</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="p_smalltext">
                        Required Type:
                    </td>
                    <td class="c_smalltext">
                        <asp:DropDownList ID="ddREQTYP" runat="server">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>B</asp:ListItem>
                            <asp:ListItem>C</asp:ListItem>
                            <asp:ListItem>D</asp:ListItem>
                            <asp:ListItem>Z</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="p_smalltext">
                        Required Frequency:
                    </td>
                    <td class="c_smalltext">
                        <asp:DropDownList ID="ddREQFRQ" runat="server">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>C</asp:ListItem>
                            <asp:ListItem>D</asp:ListItem>
                            <asp:ListItem>F</asp:ListItem>
                            <asp:ListItem>M</asp:ListItem>
                            <asp:ListItem>T</asp:ListItem>
                            <asp:ListItem>W</asp:ListItem>
                            <asp:ListItem>Z</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="p_smalltext">
                        Day of Week:
                    </td>
                    <td class="c_smalltext">
                        <asp:DropDownList ID="ddDayOfWeek" runat="server">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>1</asp:ListItem>
                            <asp:ListItem>2</asp:ListItem>
                            <asp:ListItem>3</asp:ListItem>
                            <asp:ListItem>4</asp:ListItem>
                            <asp:ListItem>5</asp:ListItem>
                            <asp:ListItem>6</asp:ListItem>
                            <asp:ListItem>7</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="p_smalltext">
                    </td>
                    <td class="c_smalltext">
                        &nbsp;
                    </td>
                    <td class="p_smalltext">
                        &nbsp;
                    </td>
                    <td style="font-size=3px;">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="p_smalltext">
                        Week Validator:
                    </td>
                    <td class="c_smalltext">
                        <asp:DropDownList ID="ddWeekValidator" runat="server" AutoPostBack="True">
                            <asp:ListItem Selected="True"></asp:ListItem>
                            <asp:ListItem Value="=">Equal To</asp:ListItem>
                            <asp:ListItem Value="&lt;&gt;">Not Equal To</asp:ListItem>
                            <asp:ListItem Value="&gt;">Greater Than</asp:ListItem>
                            <asp:ListItem Value="&lt;">Less Than</asp:ListItem>
                            <asp:ListItem Value="&gt;=">Greater Than and Equal To</asp:ListItem>
                            <asp:ListItem Value="&lt;=">Less Than and Equal To</asp:ListItem>
                            <asp:ListItem Value="BETWEEN">Between</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="p_smalltext">
                        Start of Week:
                    </td>
                    <td style="font-size=3px;" class="c_smalltext">
                        <asp:TextBox ID="txtStartOfWeek" runat="server" MaxLength="6" Width="80px" />
                        <ajax:FilteredTextBoxExtender ID="ftbeSOW" runat="server" FilterType="Numbers" TargetControlID="txtStartOfWeek" />
                        <i>use 201101 to 201153</i>
                    </td>
                    <td class="p_smalltext">
                        End of Week:
                    </td>
                    <td style="font-size=3px;" class="c_smalltext">
                        <asp:TextBox ID="txtEndOfWeek" runat="server" MaxLength="6" Width="80px" />
                        <ajax:FilteredTextBoxExtender ID="ftbeEOW" runat="server" FilterType="Numbers" TargetControlID="txtEndOfWeek" />
                        <i>use 201101 to 201153</i>
                    </td>
                </tr>
                <tr>
                    <td class="p_smalltext">
                        Month Validator:
                    </td>
                    <td class="c_smalltext">
                        <asp:DropDownList ID="ddMonthValidator" runat="server" AutoPostBack="True">
                            <asp:ListItem Selected="True"></asp:ListItem>
                            <asp:ListItem Value="=">Equal To</asp:ListItem>
                            <asp:ListItem Value="&lt;&gt;">Not Equal To</asp:ListItem>
                            <asp:ListItem Value="&gt;">Greater Than</asp:ListItem>
                            <asp:ListItem Value="&lt;">Less Than</asp:ListItem>
                            <asp:ListItem Value="&gt;=">Greater Than and Equal To</asp:ListItem>
                            <asp:ListItem Value="&lt;=">Less Than and Equal To</asp:ListItem>
                            <asp:ListItem Value="BETWEEN">Between</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="p_smalltext">
                        Start of Month:
                    </td>
                    <td style="font-size=3px;" class="c_smalltext">
                        <asp:TextBox ID="txtStartOfMonth" runat="server" MaxLength="6" Width="80px" />
                        <ajax:FilteredTextBoxExtender ID="ftbeSOM" runat="server" TargetControlID="txtStartOfMonth"
                            FilterType="Numbers" />
                        <i>use 201101 to 201112</i>
                    </td>
                    <td class="p_smalltext">
                        End of Month:
                    </td>
                    <td style="font-size=3px;" class="c_smalltext">
                        <asp:TextBox ID="txtEndOfMonth" runat="server" MaxLength="6" Width="80px" />
                        <ajax:FilteredTextBoxExtender ID="ftbeEOM" runat="server" TargetControlID="txtEndOfMonth"
                            FilterType="Numbers" />
                        <i>use 201101 to 201112</i>
                    </td>
                </tr>
                <tr>
                    <td class="p_smalltext">
                        Year Validator:
                    </td>
                    <td class="c_smalltext">
                        <asp:DropDownList ID="ddYearValidator" runat="server" AutoPostBack="True">
                            <asp:ListItem Selected="True"></asp:ListItem>
                            <asp:ListItem Value="=">Equal To</asp:ListItem>
                            <asp:ListItem Value="&lt;&gt;">Not Equal To</asp:ListItem>
                            <asp:ListItem Value="&gt;">Greater Than</asp:ListItem>
                            <asp:ListItem Value="&lt;">Less Than</asp:ListItem>
                            <asp:ListItem Value="&gt;=">Greater Than and Equal To</asp:ListItem>
                            <asp:ListItem Value="&lt;=">Less Than and Equal To</asp:ListItem>
                            <asp:ListItem Value="BETWEEN">Between</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="p_smalltext">
                        Start of Year:
                    </td>
                    <td class="c_smalltext">
                        <asp:TextBox ID="txtStartOfYear" runat="server" MaxLength="4" Width="50px" />
                        <ajax:FilteredTextBoxExtender ID="ftbeStartOfYear" runat="server" TargetControlID="txtStartOfYear"
                            FilterType="Numbers" />
                    </td>
                    <td class="p_smalltext">
                        End of Year:
                    </td>
                    <td class="c_smalltext">
                        <asp:TextBox ID="txtEndOfYear" runat="server" MaxLength="4" Width="50px" />
                        <ajax:FilteredTextBoxExtender ID="ftbeEndOfYear" runat="server" TargetControlID="txtEndOfYear"
                            FilterType="Numbers" />
                    </td>
                </tr>
                <tr>
                    <td class="p_smalltext">
                        Replace QTYRQ:
                    </td>
                    <td colspan="5">
                        <asp:TextBox ID="txtQTYRQ" runat="server" MaxLength="10" Width="80px" />
                        <ajax:FilteredTextBoxExtender ID="ftbQTYRQ" runat="server" TargetControlID="txtQTYRQ"
                            FilterType="Custom" ValidChars="1234567890,-" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td class="c_smalltext">
                        <asp:CheckBox ID="cbWKNEFWOM" runat="server" Text="ReqDat Week <> First Week of Month" />
                    </td>
                    <td>
                    </td>
                    <td class="c_smalltext" colspan="3">
                        <asp:CheckBox ID="cbWKEQFWOM" runat="server" Text="ReqDat Week = First Week of Month" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td class="c_smalltext">
                        <asp:CheckBox ID="cbRDTGTFDOM" runat="server" Text="ReqDat > First Day of Month" />
                    </td>
                    <td>
                    </td>
                    <td class="c_smalltext">
                        <asp:CheckBox ID="cbRDTLTFDOM" runat="server" Text="ReqDat < First Day of Month" />
                    </td>
                </tr>
                <tr>
                    <td class="p_smalltext" valign="top">
                        Notes:
                    </td>
                    <td class="c_smalltext" colspan="5">
                        <asp:TextBox ID="txtNotes" MaxLength="200" Width="600px" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="height: 27px">
                    </td>
                    <td style="height: 27px" colspan="5">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CausesValidation="true" ValidationGroup="vsForecastException" />
                        <asp:Button ID="btnReset" runat="server" CausesValidation="False" Text="Reset" /><br />
                        <br />
                        <asp:Label ID="lblMessageView4" runat="server" Font-Bold="True" ForeColor="Red" Height="16px"
                            Text="Label" Visible="False" Width="368px" Font-Size="Small"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:ValidationSummary ID="vsFE" runat="server" ValidationGroup="vsForecastException"
                ShowMessageBox="true" ShowSummary="true" />
        </asp:Panel>
        <ajax:CollapsiblePanelExtender ID="FEExtender" runat="server" TargetControlID="FEContentPanel"
            ExpandControlID="FEPanel" CollapseControlID="FEPanel" Collapsed="FALSE" TextLabelID="lblFE"
            ExpandedText="Enter Exception below:" CollapsedText="Enter Exception below:"
            ImageControlID="imgFE" CollapsedImage="~/images/expand_blue.jpg" ExpandedImage="~/images/collapse_blue.jpg"
            SuppressPostBack="true">
        </ajax:CollapsiblePanelExtender>
        <hr />
        <br />
        <asp:GridView ID="gvForecastException" runat="server" AutoGenerateColumns="False"
            DataKeyNames="RowID" OnRowDataBound="gvForecastException_RowDataBound" DataSourceID="odsException"
            EmptyDataText="No data available for grid view. Use fields above to add new entry."
            Width="100%" CssClass="c_smalltext" PageSize="100" AllowSorting="True" AllowPaging="True">
            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <EmptyDataRowStyle Wrap="False" />
            <Columns>
                <asp:TemplateField HeaderText="RowID" SortExpression="RowID" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <% If ViewState("Admin") = "true" Then%>
                        <asp:HyperLink ID="lblRowID" runat="server" Font-Underline="true" Text='<%# Bind("RowID") %>'
                            NavigateUrl='<%# "ForecastExceptionMaint.aspx?pRowID=" & DataBinder.Eval (Container.DataItem,"RowID").tostring%>' />
                        <% Else%>
                        <asp:Label ID="lblRow" runat="server" Text='<%# Bind("RowID") %>' />
                        <% End If%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:BoundField DataField="COMPNYStmt" HeaderText="COMPNY" SortExpression="COMPNYStmt" />
                <asp:BoundField DataField="OEMStmt" HeaderText="OEM" SortExpression="OEMStmt" ItemStyle-Wrap="false">
                    <ItemStyle Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="CABBVStmt" HeaderText="CABBV" SortExpression="CABBVStmt"
                    ItemStyle-Wrap="false">
                    <ItemStyle Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="SOLDTOStmt" HeaderText="SOLDTO" SortExpression="SOLDTOStmt" />
                <asp:BoundField DataField="PARTNOStmt" HeaderText="PARTNO" SortExpression="PARTNOStmt"
                    ItemStyle-Width="100px">
                    <ItemStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="DABBVStmt" HeaderText="DABBV" SortExpression="DABBVStmt"
                    ItemStyle-Wrap="false">
                    <ItemStyle Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="TRNTYP" HeaderText="TRNTYP" SortExpression="TRNTYP" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="REQTYP" HeaderText="REQTYP" SortExpression="REQTYP" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="REQFRQ" HeaderText="REQFRQ" SortExpression="REQFRQ" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="DayOfWeekID" HeaderText="Day Of Wk" SortExpression="DayOfWeekID"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="40px" HeaderStyle-Wrap="true"
                    ItemStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="WeekStmt" HeaderText="Week" SortExpression="WeekStmt"
                    ItemStyle-Wrap="true" ItemStyle-Width="100px">
                    <ItemStyle Width="100px" Wrap="True" />
                </asp:BoundField>
                <asp:BoundField DataField="MonthStmt" HeaderText="Month" SortExpression="MonthStmt"
                    ItemStyle-Wrap="true" ItemStyle-Width="100px">
                    <ItemStyle Width="100px" Wrap="True" />
                </asp:BoundField>
                <asp:BoundField DataField="YearStmt" HeaderText="Year" SortExpression="YearStmt"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="true" ItemStyle-Width="100px">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle Width="100px" Wrap="True" />
                </asp:BoundField>
                <asp:BoundField DataField="ReplaceQTYRQ" HeaderText="Replace QTYRQ" HeaderStyle-Wrap="true"
                    HeaderStyle-HorizontalAlign="Center" SortExpression="ReplaceQTYRQ" ItemStyle-Width="50px"
                    ItemStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                </asp:BoundField>
                <asp:BoundField DataField="WKNEFWOM" HeaderText="WK <> FWOM" SortExpression="WKNEFWOM"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px"
                    HeaderStyle-Wrap="true">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                </asp:BoundField>
                <asp:BoundField DataField="WKEQFWOM" HeaderText="WK = FWOM" SortExpression="WKEQFWOM"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px"
                    HeaderStyle-Wrap="true">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                </asp:BoundField>
                <asp:BoundField DataField="RDTGTFDOM" HeaderText="RDT > FDOM" SortExpression="RDTGTFDOM"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px"
                    HeaderStyle-Wrap="true">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                </asp:BoundField>
                <asp:BoundField DataField="RDTLTFDOM" HeaderText="RDT < FDOM" SortExpression="RDTLTFDOM"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center " ItemStyle-Width="50px"
                    HeaderStyle-Wrap="true">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                </asp:BoundField>
                <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" />
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                            ToolTip="Delete" ImageUrl="~/images/delete.jpg" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsException" runat="server" SelectMethod="GetForecastException"
            OldValuesParameterFormatString="original_{0}" DeleteMethod="DeleteForecastException"
            TypeName="ForecastExceptionBLL">
            <SelectParameters>
                <asp:Parameter Name="RowID" Type="Int32" DefaultValue="0" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="RowID" Type="Int32" />
                <asp:Parameter Name="original_RowID" Type="Int32" />
            </DeleteParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
