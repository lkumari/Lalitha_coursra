<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Sales_Projection.aspx.vb" Inherits="PMT_Sales_Projection" Title="Untitled Page"
    MaintainScrollPositionOnPostback="True" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="100%">
        <asp:Label ID="lblErrors" runat="server" Text="Label" Visible="False" SkinID="MessageLabelSkin" />
        <%  If HttpContext.Current.Request.QueryString("sPartNo") <> "" And HttpContext.Current.Request.QueryString("sPartNo") <> Nothing Then%>
        <table>
            <tr>
                <td class="p_smalltextbold" style="width: 532px; color: #990000">
                    Edit data below or press
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CausesValidation="False" />
                    to enter new data.&nbsp; Press
                    <asp:Button ID="btnCopy" runat="server" Text="Copy" />
                    to carry data into a new part number(s).
                </td>
            </tr>
        </table>
        <% End If%>
        <hr />
        <br />
        <table style="width: 85%">
            <tr>
                <td class="p_text" style="width: 136px">
                    <asp:Label ID="lblReq1" runat="server" Font-Bold="True" ForeColor="Red" Text="*" />&nbsp;
                    Part Number:
                </td>
                <td>
                   <%-- <asp:DropDownList ID="ddPartNo" runat="server" />
                    &nbsp;--%>
                     <asp:TextBox ID="txtPartNo" runat="server" Width="200px" MaxLength="40" />
                    <asp:RequiredFieldValidator ID="rfvPartNo" runat="server" ControlToValidate="txtPartNo"
                        ErrorMessage="PartNo is a required field." Font-Bold="False" ValidationGroup="PartInfo"><</asp:RequiredFieldValidator>
                </td>
                <td class="p_text" style="width: 136px">
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Red" Text="*" />&nbsp;
                    Key Part Indicator:
                </td>
                <td>
                    <%--<asp:DropDownList ID="ddKeyPartIndicator" runat="server" />
                    &nbsp;--%>
                    <asp:TextBox ID="txtKeyPartIndicator" runat="server" Width="200px" MaxLength="40" />
                    <asp:RequiredFieldValidator ID="rfvKeyPartIndicator" runat="server" ControlToValidate="txtKeyPartIndicator"
                        ErrorMessage="Key Part Indicator is a required field." Font-Bold="False" ValidationGroup="PartInfo"><</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="p_text" valign="top">
                    <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Red" Text="*" />&nbsp;
                    Commodity:
                </td>
                <td>
                    <asp:DropDownList ID="ddCommodity" runat="server" />
                    &nbsp;
                    <asp:RequiredFieldValidator ID="rfvCommodity" runat="server" ControlToValidate="ddCommodity"
                        ErrorMessage="Commodity is a required field." Font-Bold="False" ValidationGroup="PartInfo"><</asp:RequiredFieldValidator><br />
                    {Commodity / Classification}
                </td>
                <td class="p_text" style="width: 136px">
                    <asp:Label ID="Label3" runat="server" Font-Bold="True" ForeColor="Red" Text="*" />&nbsp;
                    Product Technology:
                </td>
                <td>
                    <asp:DropDownList ID="ddProductTechnology" runat="server" />
                    &nbsp;
                    <asp:RequiredFieldValidator ID="rfvProdTech" runat="server" ControlToValidate="ddProductTechnology"
                        ErrorMessage="Product Technology is a required field." Font-Bold="False" ValidationGroup="PartInfo"
                        SetFocusOnError="True"><</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="Label8" runat="server" Font-Bold="True" ForeColor="Red" Text="*" />&nbsp;
                    Royalty:
                </td>
                <td>
                    <asp:DropDownList ID="ddRoyalty" runat="server" />
                    &nbsp;
                    <asp:RequiredFieldValidator ID="rfvRoyalty" runat="server" ControlToValidate="ddRoyalty"
                        ErrorMessage="Royalty is a required field." Font-Bold="False" ValidationGroup="PartInfo"><</asp:RequiredFieldValidator>
                </td>
                <td class="p_text">
                    Cost Sheet ID:
                </td>
                <td>
                    <asp:TextBox ID="txtCostSheetID" runat="server" Width="80px" />
                    <ajax:FilteredTextBoxExtender ID="ftbeCostSheetID" runat="server" TargetControlID="txtCostSheetID"
                        FilterType="Numbers" />
                    <asp:HyperLink ID="hlnkNewCostSheetID" runat="server" Font-Underline="true" ImageUrl="~/images/PreviewUp.jpg"
                        Target="_blank" ToolTip="Preview Cost Sheet." Visible="false" />
                </td>
            </tr>
            <tr>
                <td class="p_text" valign="top">
                    Comments
                </td>
                <td colspan="4">
                    <asp:TextBox ID="txtComments" runat="server" MaxLength="300" Rows="4" TextMode="MultiLine"
                        Width="400px" /><br />
                    <asp:Label ID="lblComments" runat="server" Font-Bold="True" ForeColor="Red" />
                </td>
            </tr>
            <tr>
                <td class="p_text" style="height: 26px; width: 136px;">
                </td>
                <td colspan="5" style="height: 26px">
                    <asp:Button ID="btnSave" runat="server" Text="Save" CausesValidation="True" ValidationGroup="PartInfo" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="False" />
                    <asp:Button ID="btnDelete" runat="server" Text="Delete" CausesValidation="False"
                        OnClientClick="return confirm('Are you sure you want to delete this record?');" />
                </td>
            </tr>
        </table>
        <asp:ValidationSummary ID="vsProjectedSales" runat="server" Font-Size="X-Small" ShowMessageBox="True"
            ShowSummary="true" ValidationGroup="PartInfo" />
        <br />
        <%'If HttpContext.Current.Request.QueryString("sPartNo") <> "" And HttpContext.Current.Request.QueryString("sPartNo") <> Nothing Then%>
        <ajax:Accordion ID="accPrice" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
            FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
            RequireOpenedPane="false" SuppressHeaderPostbacks="true">
            <Panes>
                <ajax:AccordionPane ID="AccordionPane1" runat="server">
                    <Header>
                        1. <a href="">Price Detail</a></Header>
                    <Content>
                        <asp:GridView ID="gvPrice" runat="server" DataSourceID="odsPrice" AutoGenerateColumns="False"
                            DataKeyNames="EffDate" ShowFooter="True" GridLines="Horizontal" EmptyDataText="No records found in the data source for child table."
                            Width="45%" OnRowDataBound="gvPrice_RowDataBound" OnRowCommand="gvPrice_RowCommand">
                            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" Wrap="False" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Wrap="False" />
                            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" Wrap="False" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" Wrap="False" />
                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#E2DED6" Wrap="False" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" Wrap="False" />
                            <EmptyDataRowStyle BackColor="White" Wrap="False" />
                            <Columns>
                                <asp:TemplateField HeaderText="Cost Down %" SortExpression="CostDown">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtCostDown" runat="server" Text='<%# Bind("CostDown") %>' Width="80px"
                                            MaxLength="10"></asp:TextBox>
                                        <asp:RangeValidator ID="rvCostDown" runat="server" ControlToValidate="txtCostDown"
                                            Display="Dynamic" ErrorMessage="Cost Down requires a numeric value -9999.9999 to 9999.9999"
                                            Height="16px" MaximumValue="9999.99" MinimumValue="-9999.99" Type="Double" ValidationGroup="vsAdmin"><</asp:RangeValidator>
                                        <ajax:FilteredTextBoxExtender ID="ftbeCostDown" runat="server" TargetControlID="txtCostDown"
                                            FilterType="Custom, Numbers" ValidChars="-." />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        &nbsp<asp:Label ID="lblCostDown" runat="server" Text='<%# Bind("CostDown") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtCostDown" runat="server" Width="80px" MaxLength="10">0.0000</asp:TextBox>
                                        <asp:RangeValidator ID="rvCostDown" runat="server" ControlToValidate="txtCostDown"
                                            Display="Dynamic" ErrorMessage="Cost Down requires a numeric value -9999.9999 to 9999.9999"
                                            Height="16px" MaximumValue="9999.99" MinimumValue="-9999.99" Type="Double" ValidationGroup="vsAdmin"><</asp:RangeValidator>
                                        <ajax:FilteredTextBoxExtender ID="ftbeCostDown" runat="server" TargetControlID="txtCostDown"
                                            FilterType="Custom, Numbers" ValidChars="-." />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Price" SortExpression="Price">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtPrice" runat="server" Text='<%# Bind("Price") %>' Width="80px"
                                            MaxLength="10"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvPrice" runat="server" ErrorMessage="Price is a required field."
                                            ControlToValidate="txtPrice" Font-Bold="True" Font-Italic="False" ValidationGroup="EditPriceInfo"><</asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="rvPrice" runat="server" ControlToValidate="txtPrice" ErrorMessage="Price requires a numeric value 0-9999.9999"
                                            Font-Bold="True" MaximumValue="9999.9999" MinimumValue="0" ValidationGroup="EditPriceInfo"
                                            Type="Double"><</asp:RangeValidator>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        &nbsp<asp:Label ID="lblPrice" runat="server" Text='<%# Bind("Price") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtPrice" runat="server" Width="80px" MaxLength="10">0.0000</asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvPrice" runat="server" ControlToValidate="txtPrice"
                                            ErrorMessage="Price is a required field." Font-Bold="True" Font-Italic="False"
                                            ValidationGroup="InsertPriceInfo"><</asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="rvPrice" runat="server" ControlToValidate="txtPrice" ErrorMessage="Price requires a numeric value 0-9999.999"
                                            Font-Bold="True" MaximumValue="9999.9999" MinimumValue="0" ValidationGroup="InsertPriceInfo"
                                            Type="Double"><</asp:RangeValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Effective Date" SortExpression="EffDate">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" ID="txtEffDate" Text='<%# Bind("EffDate") %>' Width="85px" />&nbsp;
                                        <asp:ImageButton runat="server" ID="imgEffDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                            AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                                        <ajax:CalendarExtender ID="calendarButtonExtender" runat="server" TargetControlID="txtEffDate"
                                            PopupButtonID="imgEffDate" Format="MM/dd/yyyy" />
                                        <asp:RegularExpressionValidator ID="revEffDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                            ControlToValidate="txtEffDate" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                            Width="8px" ValidationGroup="EditPriceInfo"><</asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="rfvEffDate" runat="server" ErrorMessage="Effective Date is a required field."
                                            ControlToValidate="txtEffDate" Font-Bold="True" ValidationGroup="EditPriceInfo"><</asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        &nbsp<asp:Label ID="lblEffDate" runat="server" Text='<%# Bind("EffDate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox runat="server" ID="txtEffDate" Width="85px" />&nbsp;
                                        <asp:ImageButton runat="server" ID="imgEffDate" ImageUrl="~/images/ajax/Calendar_scheduleHS.png"
                                            AlternateText="Click to show calendar" Height="19px" ImageAlign="Middle" Width="19px" />
                                        <ajax:CalendarExtender ID="calendarButtonExtender" runat="server" TargetControlID="txtEffDate"
                                            PopupButtonID="imgEffDate" Format="MM/dd/yyyy" />
                                        <asp:RegularExpressionValidator ID="revEffDate" runat="server" ErrorMessage='Invalid Date Entry:  use "mm/dd/yyyy" or "m/d/yyyy" format '
                                            ControlToValidate="txtEffDate" Font-Bold="True" ToolTip="MM/DD/YYYY" ValidationExpression="^(((0?[1-9]|1[012])/(0?[1-9]|1\d|2[0-8])|(0?[13456789]|1[012])/(29|30)|(0?[13578]|1[02])/31)/(19|[2-9]\d)\d{2}|0?2/29/((19|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(([2468][048]|[3579][26])00)))$"
                                            Width="8px" ValidationGroup="InsertPriceInfo"><</asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="rfvEffDate" runat="server" ControlToValidate="txtEffDate"
                                            ErrorMessage="Effective Date is a required field." Font-Bold="True" ValidationGroup="InsertPriceInfo"><</asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <EditItemTemplate>
                                        <asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                            ImageUrl="~/images/save.jpg" Text="Update" AlternateText="Update" ValidationGroup="EditPriceInfo" />&nbsp;&nbsp;&nbsp;
                                        <asp:ImageButton ID="ibtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                            ImageUrl="~/images/cancel.jpg" Text="Cancel" AlternateText="Cancel" ValidationGroup="EditPriceInfo" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                            ImageUrl="~/images/edit.jpg" Text="Edit" AlternateText="Edit" ValidationGroup="EditPriceInfo" />&nbsp;&nbsp;&nbsp;
                                        <asp:ImageButton ID="ibtnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                            Visible='<%# ViewState("ObjectRole")%>' ImageUrl="~/images/delete.jpg" Text="Delete"
                                            AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton ID="ibtnInsert" runat="server" CausesValidation="True" CommandName="Insert"
                                            ImageUrl="~/images/save.jpg" Text="Insert" AlternateText="Insert" ValidationGroup="InsertPriceInfo" />&nbsp;&nbsp;&nbsp;
                                        <asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo" ImageUrl="~/images/undo-gray.jpg"
                                            Text="Undo" AlternateText="Undo" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                No Records Found in the database.
                            </EmptyDataTemplate>
                        </asp:GridView>
                        <asp:ValidationSummary ID="vsEditPriceInfo" runat="server" ShowMessageBox="True"
                            ValidationGroup="EditPriceInfo" />
                        <asp:ObjectDataSource ID="odsPrice" runat="server" DeleteMethod="DeleteProjectedSalesPrice"
                            InsertMethod="InsertProjectedSalesPrice" SelectMethod="GetProjectedSalesPrice"
                            TypeName="Projected_Sales_PriceBLL" UpdateMethod="UpdateProjectedSalesPrice"
                            OldValuesParameterFormatString="original_{0}">
                            <DeleteParameters>
                                <asp:QueryStringParameter Name="PartNo" QueryStringField="sPartNo" Type="String" />
                                <asp:Parameter Name="EffDate" Type="String" />
                                <asp:Parameter Name="original_EffDate" Type="String" />
                            </DeleteParameters>
                            <SelectParameters>
                                <asp:QueryStringParameter Name="PartNo" QueryStringField="sPartNo" Type="String" />
                            </SelectParameters>
                            <InsertParameters>
                                <asp:QueryStringParameter Name="PartNo" QueryStringField="sPartNo" Type="String" />
                                <asp:Parameter Name="Price" Type="Decimal" />
                                <asp:Parameter Name="EffDate" Type="String" />
                                <asp:Parameter Name="CostDown" Type="Decimal" />
                            </InsertParameters>
                            <UpdateParameters>
                                <asp:QueryStringParameter Name="PartNo" QueryStringField="sPartNo" Type="String" />
                                <asp:Parameter Name="Price" Type="Decimal" />
                                <asp:Parameter Name="EffDate" Type="String" />
                                <asp:Parameter Name="CostDown" Type="Decimal" />
                                <asp:Parameter Name="original_EffDate" Type="String" />
                            </UpdateParameters>
                        </asp:ObjectDataSource>
                        <asp:ValidationSummary ID="vsInsertPriceInfo" runat="server" ShowMessageBox="True"
                            ValidationGroup="InsertPriceInfo" />
                        <asp:ValidationSummary ID="vsEmptyPriceInfo" runat="server" ShowMessageBox="True"
                            ValidationGroup="EmptyPriceInfo" />
                    </Content>
                </ajax:AccordionPane>
            </Panes>
        </ajax:Accordion>
        <!-- Customer Program -->
        <ajax:Accordion ID="accCustomerProgram" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
            FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
            RequireOpenedPane="false" SuppressHeaderPostbacks="true">
            <Panes>
                <ajax:AccordionPane ID="AccordionPane2" runat="server">
                    <Header>
                        2. <a href="">Customer and Program Detail</a></Header>
                    <Content>
                        <asp:GridView ID="gvCustomerProgram" runat="server" AutoGenerateColumns="False" CellPadding="4"
                            EmptyDataText="No data in the data source for child table." DataKeyNames="PartNo,CABBV,SoldTo,ProgramID,UGNFacility"
                            DataSourceID="odsProjectedSalesCustomerProgram" OnRowCommand="gvCustomerProgram_RowCommand"
                            OnRowDataBound="gvCustomerProgram_RowDataBound" ShowFooter="True" GridLines="Horizontal"
                            Width="100%">
                            <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" Wrap="False" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#E2DED6" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <EmptyDataRowStyle Wrap="False" />
                            <Columns>
                                <asp:TemplateField HeaderText="Customer" SortExpression="CABBV">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddCustomer" runat="server" DataSource='<%# commonFunctions.GetCustomer("false") %>'
                                            DataValueField="ddCustomerValue" DataTextField="ddCustomerDesc" SelectedValue='<%# Bind("ddCustomerValue") %>'
                                            AutoPostBack="True" OnSelectedIndexChanged="ddCustomer_SelectedIndexChanged1"
                                            Width="270px">
                                            <asp:ListItem Selected="True">
                                            </asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvCustomer" runat="server" ControlToValidate="ddCustomer"
                                            ErrorMessage="Customer is a required field." Font-Bold="True" ValidationGroup="EditCustomerInfo"><</asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("ddCustomerDesc") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddCustomer" runat="server" DataSource='<%# commonFunctions.GetCustomer("false") %>'
                                            DataValueField="ddCustomerValue" DataTextField="ddCustomerDesc" SelectedValue='<%# Bind("ddCustomerValue") %>'
                                            AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddCustomer_Footer_SelectedIndexChanged"
                                            Width="270px">
                                            <asp:ListItem Selected="True">
                                            </asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvCustomer" runat="server" ControlToValidate="ddCustomer"
                                            ErrorMessage="Customer is a required field." Font-Bold="True" ValidationGroup="InsertCustomerInfo"><</asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Program" SortExpression="ProgramID">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddProgram" runat="server" DataSourceID="sdsProgram_by_CABBV"
                                            DataTextField="ProgramName" DataValueField="ProgramID">
                                            <asp:ListItem Selected="True">
                                            </asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="sdsProgram_by_CABBV" runat="server" ConnectionString="<%$ ConnectionStrings:SQLConnection %>"
                                            SelectCommand="sp_Get_Program_by_CABBV" SelectCommandType="StoredProcedure">
                                            <SelectParameters>
                                                <asp:Parameter DefaultValue="0" Name="ProgramID" Type="Int32" />
                                                <asp:Parameter Name="CABBV" Type="String" />
                                                <asp:Parameter Name="SoldTo" Type="Int32" />
                                                <asp:Parameter Name="NewEntry" Type="Boolean" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:RequiredFieldValidator ID="rfvProgram" runat="server" ControlToValidate="ddProgram"
                                            ErrorMessage="Program is a required field." Font-Bold="True" ValidationGroup="EditCustomerInfo"><</asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("ProgramName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddProgram" runat="server" DataSourceID="sdsProgram_by_CABBV"
                                            DataTextField="ProgramName" DataValueField="ProgramID">
                                            <asp:ListItem Selected="True">
                                            </asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="sdsProgram_by_CABBV" runat="server" ConnectionString="<%$ ConnectionStrings:SQLConnection %>"
                                            SelectCommand="sp_Get_Program_by_CABBV" SelectCommandType="StoredProcedure">
                                            <SelectParameters>
                                                <asp:Parameter DefaultValue="0" Name="ProgramID" Type="Int32" />
                                                <asp:Parameter Name="CABBV" Type="String" />
                                                <asp:Parameter Name="SoldTo" Type="Int32" />
                                                <asp:Parameter Name="NewEntry" Type="Boolean" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:RequiredFieldValidator ID="rfvProgram" runat="server" ControlToValidate="ddProgram"
                                            ErrorMessage="Program is a required field." Font-Bold="True" ValidationGroup="InsertCustomerInfo"><</asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Program Status" SortExpression="ProgramStatus">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddProgramStatus" runat="server" SelectedValue='<%# Bind("ProgramStatus") %>'>
                                            <asp:ListItem Selected="True">
                                            </asp:ListItem>
                                            <asp:ListItem Value="Awarded - New">Awarded - New</asp:ListItem>
                                            <asp:ListItem Value="Awarded - Carry Over">Awarded - Carry Over</asp:ListItem>
                                            <asp:ListItem Value="In Process">In Process</asp:ListItem>
                                            <asp:ListItem Value="Loss Business">Loss Business</asp:ListItem>
                                            <asp:ListItem Value="Potential - New">Potential - New</asp:ListItem>
                                            <asp:ListItem Value="Potential - Carry Over">Potential - Carry Over</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvProgramStatus" runat="server" ControlToValidate="ddProgramStatus"
                                            ErrorMessage="Program Status is a required field." Font-Bold="True" ValidationGroup="EditCustomerInfo"><</asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("ProgramStatus") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddProgramStatus" runat="server" SelectedValue='<%# Bind("ProgramStatus") %>'>
                                            <asp:ListItem Selected="True">
                                            </asp:ListItem>
                                            <asp:ListItem Value="Awarded - New">Awarded - New</asp:ListItem>
                                            <asp:ListItem Value="Awarded - Carry Over">Awarded - Carry Over</asp:ListItem>
                                            <asp:ListItem Value="In Process">In Process</asp:ListItem>
                                            <asp:ListItem Value="Loss Business">Loss Business</asp:ListItem>
                                            <asp:ListItem Value="Potential - New">Potential - New</asp:ListItem>
                                            <asp:ListItem Value="Potential - Carry Over">Potential - Carry Over</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvProgramStatus" runat="server" ControlToValidate="ddProgramStatus"
                                            ErrorMessage="Program Status is a required field." Font-Bold="True" ValidationGroup="InsertCustomerInfo"><</asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="UGN Facility" SortExpression="UGNFacility">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddUGNFacility" runat="server" SelectedValue='<%# Bind("UGNFacility") %>'
                                            AppendDataBoundItems="True" DataSourceID="dsUGNFacility" DataTextField="UGNFacilityName"
                                            DataValueField="UGNFacility">
                                            <asp:ListItem Selected="True">
                                            </asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="dsUGNFacility" runat="server" ConnectionString="<%$ ConnectionStrings:SQLConnection %>"
                                            SelectCommand="sp_Get_UGNFacility" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                                        <asp:RequiredFieldValidator ID="rfvUGNFacility" runat="server" ControlToValidate="ddUGNFacility"
                                            ErrorMessage="UGN Facility is a required field." Font-Bold="True" ValidationGroup="EditCustomerInfo"><</asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label5" runat="server" Text='<%# Bind("UGNFacilityName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddUGNFacility" runat="server" SelectedValue='<%# Bind("UGNFacility") %>'
                                            AppendDataBoundItems="True" DataSourceID="dsUGNFacility" DataTextField="UGNFacilityName"
                                            DataValueField="UGNFacility">
                                            <asp:ListItem Selected="True">
                                            </asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="dsUGNFacility" runat="server" ConnectionString="<%$ ConnectionStrings:SQLConnection %>"
                                            SelectCommand="sp_Get_UGNFacility" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                                        <asp:RequiredFieldValidator ID="rfvUGNFacility" runat="server" ControlToValidate="ddUGNFacility"
                                            ErrorMessage="UGN Facility is a required field." Font-Bold="True" ValidationGroup="InsertCustomerInfo"><</asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Pieces/Vehicle" SortExpression="PiecesPerVehicle">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtPiecesPerVehicle" runat="server" Text='<%# Bind("PiecesPerVehicle") %>'
                                            Width="70px"></asp:TextBox>
                                        <asp:RangeValidator ID="rvPiecesPerVehicle" runat="server" ControlToValidate="txtPiecesPerVehicle"
                                            ErrorMessage="Pieces/Vehicle requires a numeric value 0-9999.9999" Font-Bold="True"
                                            MaximumValue="9999.9999" MinimumValue="0.0" Type="Double" ValidationGroup="EditCustomerInfo"><</asp:RangeValidator>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label6" runat="server" Text='<%# Bind("PiecesPerVehicle") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtPiecesPerVehicle" runat="server" Text='<%# Bind("PiecesPerVehicle") %>'
                                            Width="70px"></asp:TextBox>
                                        <asp:RangeValidator ID="rvPiecesPerVehicle" runat="server" ControlToValidate="txtPiecesPerVehicle"
                                            ErrorMessage="Pieces/Vehicle requires a numeric value 0-9999.9999" Font-Bold="True"
                                            MaximumValue="9999.9999" MinimumValue="0" Type="Double" ValidationGroup="InsertCustomerInfo"><</asp:RangeValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Usage Factor" SortExpression="UsageFactorPerVehicle">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtUsageFactorPerVehicle" runat="server" Text='<%# Bind("UsageFactorPerVehicle") %>'
                                            Width="70px"></asp:TextBox>
                                        <asp:RangeValidator ID="rvUsageFactor" runat="server" ControlToValidate="txtUsageFactorPerVehicle"
                                            ErrorMessage="Usage Factor requires a numeric value 0-9999.9999" Font-Bold="True"
                                            MaximumValue="9999.9999" MinimumValue="0" Type="Double" ValidationGroup="EditCustomerInfo"><</asp:RangeValidator>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label7" runat="server" Text='<%# Bind("UsageFactorPerVehicle") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtUsageFactorPerVehicle" runat="server" Text='<%# Bind("UsageFactorPerVehicle") %>'
                                            Width="70px"></asp:TextBox>
                                        <asp:RangeValidator ID="rvUsageFactor" runat="server" ControlToValidate="txtUsageFactorPerVehicle"
                                            ErrorMessage="Usage Factor requires a numeric value 0-9999.9999" Font-Bold="True"
                                            MaximumValue="9999.9999" MinimumValue="0" Type="Double" ValidationGroup="InsertCustomerInfo"><</asp:RangeValidator>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <EditItemTemplate>
                                        <asp:ImageButton ID="ibtnUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                            ImageUrl="~/images/save.jpg" Text="Update" AlternateText="Update" ValidationGroup="EditCustomerInfo" />&nbsp;&nbsp;&nbsp;<asp:ImageButton
                                                ID="ibtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                                ImageUrl="~/images/cancel.jpg" Text="Cancel" AlternateText="Cancel" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibtnEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                            ImageUrl="~/images/edit.jpg" Text="Edit" AlternateText="Edit" />&nbsp;&nbsp;&nbsp;
                                        <asp:ImageButton ID="ibtnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                            Visible='<%# ViewState("ObjectRole")%>' ImageUrl="~/images/delete.jpg" Text="Delete"
                                            AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton ID="ibtnInsert" runat="server" CausesValidation="True" CommandName="Insert"
                                            ImageUrl="~/images/save.jpg" Text="Update" AlternateText="Insert" ValidationGroup="InsertCustomerInfo" />&nbsp;&nbsp;&nbsp;
                                        <asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo" ImageUrl="~/images/undo-gray.jpg"
                                            Text="Undo" AlternateText="Undo" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="PartNo" HeaderText="PartNo" ReadOnly="True" Visible="False" />
                            </Columns>
                            <EmptyDataTemplate>
                                No Records Found in the database.
                            </EmptyDataTemplate>
                        </asp:GridView>
                        <asp:ValidationSummary ID="vsEditCustomerInfo" runat="server" ShowMessageBox="True"
                            ValidationGroup="EditCustomerInfo" />
                        <asp:ValidationSummary ID="vsEmptyCustomerInfo" runat="server" ShowMessageBox="True"
                            ValidationGroup="EmptyCustomerInfo" />
                        <asp:ValidationSummary ID="vsInsertCustomerInfo" runat="server" ShowMessageBox="True"
                            ValidationGroup="InsertCustomerInfo" />
                        <asp:ObjectDataSource ID="odsProjectedSalesCustomerProgram" runat="server" SelectMethod="GetProjectedSalesCustomerProgram"
                            TypeName="Projected_Sales_Customer_ProgramBLL" OldValuesParameterFormatString="original_{0}"
                            UpdateMethod="UpdateProjectedSalesCustomerProgram" DeleteMethod="DeleteProjectedSalesCustomerProgram"
                            InsertMethod="InsertProjectedSalesCustomerProgram">
                            <DeleteParameters>
                                <asp:Parameter Name="PartNo" Type="String" />
                                <asp:Parameter Name="CABBV" Type="String" />
                                <asp:Parameter Name="SoldTo" Type="Int32" />
                                <asp:Parameter Name="ProgramID" Type="Int32" />
                                <asp:Parameter Name="UGNFacility" Type="String" />
                                <asp:Parameter Name="original_CABBV" Type="String" />
                                <asp:Parameter Name="original_SoldTo" Type="Int32" />
                                <asp:Parameter Name="original_ProgramID" Type="Int32" />
                                <asp:Parameter Name="original_UGNFacility" Type="String" />
                            </DeleteParameters>
                            <UpdateParameters>
                                <asp:Parameter Name="ProgramStatus" Type="String" />
                                <asp:Parameter Name="PiecesPerVehicle" Type="Decimal" />
                                <asp:Parameter Name="UsageFactorPerVehicle" Type="Decimal" />
                                <asp:Parameter Name="original_PartNo" Type="String" />
                                <asp:Parameter Name="original_CABBV" Type="String" />
                                <asp:Parameter Name="original_SoldTo" Type="Int32" />
                                <asp:Parameter Name="original_ProgramID" Type="Int32" />
                                <asp:Parameter Name="original_UGNFacility" Type="String" />
                                <asp:Parameter Name="ddCustomerValue" Type="String" />
                                <asp:Parameter Name="UGNFacility" Type="String" />
                                <asp:Parameter Name="CABBV" Type="String" />
                                <asp:Parameter Name="SoldTo" Type="Int32" />
                                <asp:Parameter Name="ProgramID" Type="Int32" />
                            </UpdateParameters>
                            <SelectParameters>
                                <asp:QueryStringParameter Name="PartNo" QueryStringField="sPartNo" Type="String" />
                            </SelectParameters>
                            <InsertParameters>
                                <asp:Parameter Name="PartNo" Type="String" />
                                <asp:Parameter Name="CABBV" Type="String" />
                                <asp:Parameter Name="SoldTo" Type="Int32" />
                                <asp:Parameter Name="ProgramID" Type="Int32" />
                                <asp:Parameter Name="UGNFacility" Type="String" />
                                <asp:Parameter Name="ProgramStatus" Type="String" />
                                <asp:Parameter Name="PiecesPerVehicle" Type="Decimal" />
                                <asp:Parameter Name="UsageFactorPerVehicle" Type="Decimal" />
                            </InsertParameters>
                        </asp:ObjectDataSource>
                    </Content>
                </ajax:AccordionPane>
            </Panes>
        </ajax:Accordion>
        <% 'End If%>
    </asp:Panel>
</asp:Content>
