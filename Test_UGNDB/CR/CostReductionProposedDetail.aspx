<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="CostReductionProposedDetail.aspx.vb" Inherits="CR_CostReductionProposedDetail"
    MaintainScrollPositionOnPostback="true" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Visible="false">
        <asp:Label ID="lblMessage" SkinID="MessageLabelSkin" runat="server" />
        <asp:ValidationSummary ID="vsSave" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgSave" />
        <br />
        <table>
            <tr>
                <td class="p_text">
                    Project No:
                </td>
                <td style="color: #990000;">
                    <asp:Label ID="lblProjectNo" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Project Desc:
                </td>
                <td>
                    <asp:Label ID="lblDescription" runat="server" />
                </td>
            </tr>
        </table>
        <br />
        <table width="98%">
            <tr>
                <td colspan="3">
                    <hr />
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <h2>
                        Actual Savings Analysis</h2>
                    <table>
                        <tr>
                            <td class="p_text">
                                Actual Material Price and Usage:
                            </td>
                            <td class="p_text">
                                $<asp:Label runat="server" ID="lblTotalSavingsMaterialPriceAndUsage" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                Actual Cycle Time (Direct Labor) Reduction:
                            </td>
                            <td class="p_text">
                                $<asp:Label runat="server" ID="lblTotalSavingsCycleTime" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                Actual D/L or I/D/L Elimination:
                            </td>
                            <td class="p_text">
                                $<asp:Label runat="server" ID="lblTotalSavingsHeadCount" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                Actual Overhead:
                            </td>
                            <td class="p_text">
                                $<asp:Label runat="server" ID="lblTotalSavingsOverhead" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_textbold">
                                Actual Total Gross Savings:
                            </td>
                            <td class="p_text">
                                $<asp:Label runat="server" ID="lblTotalSavings" CssClass="p_textbold" ForeColor="red" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_textbold">
                                Customer Give Back:
                            </td>
                            <td class="p_text">
                                $<asp:Label runat="server" ID="lblCustomerGiveBack" CssClass="p_textbold" ForeColor="red" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_textbold">
                                Actual Total Net Savings:
                            </td>
                            <td class="p_text">
                                $<asp:Label runat="server" ID="lblTotalNetSavings" CssClass="p_textbold" ForeColor="red" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <h2>
                        Budget Savings Analysis</h2>
                    <table>
                        <tr>
                            <td class="p_text">
                                Budget Material Price and Usage:
                            </td>
                            <td class="p_text">
                                $<asp:Label runat="server" ID="lblTotalSavingsMaterialPriceAndUsageBudget" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                Budget Cycle Time (Direct Labor) Reduction:
                            </td>
                            <td class="p_text">
                                $<asp:Label runat="server" ID="lblTotalSavingsCycleTimeBudget" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                Budget D/L or I/D/L Elimination:
                            </td>
                            <td class="p_text">
                                $<asp:Label runat="server" ID="lblTotalSavingsHeadCountBudget" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                Budget Overhead:
                            </td>
                            <td class="p_text">
                                $<asp:Label runat="server" ID="lblTotalSavingsOverheadBudget" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_textbold">
                                Budget Total Gross Savings:
                            </td>
                            <td class="p_text">
                                $<asp:Label runat="server" ID="lblTotalSavingsBudget" CssClass="p_textbold" ForeColor="red" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_textbold">
                                &nbsp;
                            </td>
                            <td class="p_text">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="p_textbold">
                                Budget Total Net Savings:
                            </td>
                            <td class="p_text">
                                $<asp:Label runat="server" ID="lblTotalNetSavingsBudget" CssClass="p_textbold" ForeColor="red" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <h2>
                        Capital And Expenses</h2>
                    <table>
                        <tr>
                            <td class="p_text">
                                New Capital:
                            </td>
                            <td class="p_text">
                                $<asp:Label runat="server" ID="lblTotalCECapital" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                Materials:
                            </td>
                            <td class="p_text">
                                $<asp:Label runat="server" ID="lblTotalCEMaterial" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                Outside Support:
                            </td>
                            <td class="p_text">
                                $<asp:Label runat="server" ID="lblTotalCEOutsideSupport" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                Misc:
                            </td>
                            <td class="p_text">
                                $<asp:Label runat="server" ID="lblTotalCEMisc" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                In-House Support:
                            </td>
                            <td class="p_text">
                                $<asp:Label runat="server" ID="lblTotalCEInHouseSupport" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text">
                                Existing Fixed Asset (Net Book) Write Off:
                            </td>
                            <td class="p_text">
                                $<asp:Label runat="server" ID="lblTotalCEWriteOff" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_textbold">
                                Total Capital and Expenses:
                            </td>
                            <td class="p_text">
                                $<asp:Label runat="server" ID="lblTotalCE" CssClass="p_textbold" ForeColor="red" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="3" align="center">
                    <hr />
                    <h2>
                        Payback Analysis</h2>
                    <table width="100%">
                        <tr>
                            <td>
                                Capital Expenses / Total Savings = &nbsp
                                <asp:Label runat="server" ID="lblTotalAnnualSavingsANDCE" />
                                &nbsp
                                <asp:Label runat="server" ID="lblTotalPayback" Font-Bold="true" ForeColor="red" />
                                &nbsp years
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <hr />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblReqAnnCostChngRsnMarker" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="*" />
                    <asp:Label ID="lblAnnCostChngRsn" runat="server" Text="Savings Change Reason:" ForeColor="Red" />
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtAnnCostChngRsn" runat="server" Width="400px" />
                    <asp:RequiredFieldValidator ID="rfvAnnCostChngRsn" runat="server" ErrorMessage="Annual Cost Change Reason is a required field."
                        ControlToValidate="txtAnnCostChngRsn" ValidationGroup="vgSave" Text="<"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblReqCapExChngRsnMarker" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="*" />
                    <asp:Label ID="lblCapExChngRsn" runat="server" Text="CapEx Change Reason:" ForeColor="Red" />
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtCapExChngRsn" runat="server" Width="400px" />
                    <asp:RequiredFieldValidator ID="rfvCapExChngRsn" runat="server" ErrorMessage="CAPEX Change Reason is a required field."
                        ControlToValidate="txtCapExChngRsn" ValidationGroup="vgSave" Text="<"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button runat="server" ID="btnSave" Text="Save" CausesValidation="true" ValidationGroup="vgSave" />
                    <asp:Button runat="server" ID="btnCalculate" Text="Calculate" CausesValidation="true"
                        ValidationGroup="vgCalculate" />
                    <asp:Button runat="server" ID="btnReset" Text="Reset" CausesValidation="false" />
                    <asp:Button runat="server" ID="btnPreview" Text="Preview" CausesValidation="false" />
                    <asp:Button runat="server" ID="btnReturnToProject" Text="Return to Project Info"
                        CausesValidation="false" />
                </td>
            </tr>
        </table>
        <asp:ValidationSummary ID="vsCalculate" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgCalculate" />
        <asp:Menu ID="menuTabs" Height="30px" runat="server" Orientation="Horizontal" StaticEnableDefaultPopOutImage="False"
            StaticMenuItemStyle-CssClass="tab" StaticSelectedStyle-CssClass="selectedTab"
            CssClass="tabs">
            <Items>
                <asp:MenuItem Text="General" Value="0" ImageUrl="" />
                <asp:MenuItem Text="Customer" Value="1" ImageUrl="" />
                <asp:MenuItem Text="Material Price" Value="2" ImageUrl="" />
                <asp:MenuItem Text="Material Usage" Value="3" ImageUrl="" />
                <asp:MenuItem Text="Cycle Time" Value="4" ImageUrl="" />
                <asp:MenuItem Text="D/L or I/D/L Elimination" Value="5" ImageUrl="" />
                <asp:MenuItem Text="Overhead" Value="6" ImageUrl="" />
                <asp:MenuItem Text="Supporting Documents" Value="7" ImageUrl="" />
            </Items>
        </asp:Menu>
        <asp:MultiView ID="mvTabs" runat="server" Visible="true" ActiveViewIndex="0" EnableViewState="true">
            <asp:View ID="vGeneral" runat="server">
                <asp:Label ID="lblMessageGeneral" SkinID="MessageLabelSkin" runat="server" />
                <table width="98%" border="0">
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblCurrentMethodMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="*" />Current Method:
                        </td>
                        <td>
                            <asp:TextBox ID="txtCurrentMethod" runat="server" MaxLength="200" Width="600px" Rows="2"
                                TextMode="MultiLine" />
                            <asp:RequiredFieldValidator ID="rfvCurrentMethod" runat="server" ErrorMessage="Current Method is a required field."
                                ControlToValidate="txtCurrentMethod" ValidationGroup="vgSave" SetFocusOnError="true"
                                Text="<"></asp:RequiredFieldValidator>
                            <br />
                            <asp:Label ID="lblCurrentMethodCharCount" SkinID="MessageLabelSkin" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblProposedMethodMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="*" />Proposed Method:
                        </td>
                        <td>
                            <asp:TextBox ID="txtProposedMethod" runat="server" MaxLength="200" Width="600px"
                                Rows="2" TextMode="MultiLine" />
                            <asp:RequiredFieldValidator ID="rfvProposed" runat="server" ErrorMessage="Proposed Method is a required field."
                                ControlToValidate="txtProposedMethod" ValidationGroup="vgSave" SetFocusOnError="true"
                                Text="<"></asp:RequiredFieldValidator>
                            <br />
                            <asp:Label ID="lblProposedMethodCharCount" SkinID="MessageLabelSkin" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label ID="lblBenefitsMarker" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="*" />Benefits:
                        </td>
                        <td>
                            <asp:TextBox ID="txtBenefits" runat="server" MaxLength="200" Width="600px" Rows="2"
                                TextMode="MultiLine" />
                            <asp:RequiredFieldValidator ID="rfvBenefits" runat="server" ErrorMessage="Benefits are required."
                                ControlToValidate="txtBenefits" ValidationGroup="vgSave" SetFocusOnError="true"
                                Text="<"></asp:RequiredFieldValidator>
                            <br />
                            <asp:Label ID="lblBenefitsCharCount" SkinID="MessageLabelSkin" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            Customer Part No:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCustomerPartNo" MaxLength="30" />
                            <asp:ImageButton ID="iBtnCustomerPartNoSearch" runat="server" ImageUrl="~/images/Search.gif"
                                ToolTip="Click here to search for the customer part number." Visible="false" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Label runat="server" ID="lblMessageFinishedGood" SkinID="MessageLabelSkin" /><br />
                <asp:ValidationSummary ID="vsInsertFinishedGood" runat="server" DisplayMode="List"
                    ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgInsertFinishedGood" />
                <asp:ValidationSummary ID="vsEditFinishedGood" runat="server" DisplayMode="List"
                    ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgEditFinishedGood" />
                <h2>
                    Finished Good / Internal Part No(s)</h2>
                <asp:GridView ID="gvFinishedGood" runat="server" AutoGenerateColumns="False" DataKeyNames="RowID"
                    DataSourceID="odsFinishedGood" EmptyDataText="No Finished Goods found" AllowSorting="True"
                    AllowPaging="True" PageSize="15" ShowFooter="True" Width="600px">
                    <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#CCCCCC" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="RowID" HeaderText="RowID" ReadOnly="True" SortExpression="RowID" />
                        <asp:BoundField DataField="ProjectNo" HeaderText="ProjectNo" SortExpression="ProjectNo"
                            ReadOnly="True" />
                        <asp:TemplateField HeaderText="Internal Part No" SortExpression="PartNo">
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditPartNo" runat="server" Text='<%# Bind("PartNo") %>' MaxLength="40"
                                    Width="200px" />
                                <asp:ImageButton ID="ibtnEditSearchPartNo" runat="server" CausesValidation="False"
                                    ImageUrl="~/images/Search.gif" ToolTip="Fill in Internal Part No if known" AlternateText="Search for Internal Part No"
                                    ValidationGroup="vgEditFinishedGood" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblViewPartNo" runat="server" Text='<%# Bind("PartNo") %>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtInsertPartNo" runat="server" MaxLength="40" Width="200px" />
                                <asp:ImageButton ID="ibtnInsertSearchPartNo" runat="server" CausesValidation="False"
                                    ImageUrl="~/images/Search.gif" ToolTip="Fill in Internal Part No if known" AlternateText="Search for Internal Part No"
                                    ValidationGroup="vgInsertFinishedGood" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="PartName" HeaderText="Part Description" SortExpression="PartName"
                            ReadOnly="True" />
                        <asp:TemplateField ShowHeader="False">
                            <EditItemTemplate>
                                <asp:ImageButton ID="iBtnFinishedGoodUpdate" runat="server" CommandName="Update"
                                    ImageUrl="~/images/save.jpg" AlternateText="Update" CausesValidation="true" ValidationGroup="vgEditFinishedGood" />
                                <asp:ImageButton ID="iBtnFinishedCancelEdit" runat="server" CausesValidation="False"
                                    CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="ibtnFinishedGoodEdit" runat="server" CommandName="Edit" ImageUrl="~/images/edit.jpg"
                                    AlternateText="Edit" />&nbsp;&nbsp;&nbsp;
                                <asp:ImageButton ID="ibtnFinishedGoodDelete" runat="server" CommandName="Delete"
                                    ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgInsertFinishedGood"
                                    runat="server" ID="iBtnFinishedGoodSave" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                                <asp:ImageButton ID="iBtnFinishedGoodUndo" runat="server" CommandName="Undo" CausesValidation="false"
                                    ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsFinishedGood" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetCostReductionFinishedGood" TypeName="CRFinishedGoodBLL" DeleteMethod="DeleteCostReductionFinishedGood"
                    InsertMethod="InsertCostReductionFinishedGood" UpdateMethod="UpdateCostReductionFinishedGood">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="Int32" />
                    </SelectParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="RowID" Type="Int32" />
                        <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="Int32" />
                        <asp:Parameter Name="original_RowID" Type="Int32" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="Int32" />
                        <asp:Parameter Name="PartNo" Type="String" />
                        <asp:Parameter Name="CustomerPartNo" Type="String" />
                        <asp:Parameter Name="original_RowID" Type="Int32" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="Int32" />
                        <asp:Parameter Name="PartNo" Type="String" />
                    </InsertParameters>
                </asp:ObjectDataSource>
            </asp:View>
            <asp:View ID="vCustomer" runat="server">
                <asp:ValidationSummary runat="server" ID="vsCustomerProgram" ValidationGroup="vgCustomerProgram"
                    ShowMessageBox="true" ShowSummary="true" />
                <asp:Label ID="lblMessageCustomerProgram" runat="server" SkinID="MessageLabelSkin" />
                <table>
                    <tr>
                        <td class="p_text">
                            <asp:Label runat="server" ID="lblMake" Text="Make:" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddMake" runat="server" AutoPostBack="true" Visible="true">
                                <asp:ListItem Text="" Value="0" Selected="False" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label runat="server" ID="lblProgram" Text="Program:" />
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddProgram" runat="server" Visible="true" ValidationGroup="vgCustomerProgram">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvProgram" runat="server" ControlToValidate="ddProgram"
                                ErrorMessage="Program is required." Font-Bold="True" ValidationGroup="vgCustomerProgram"
                                Text="<" SetFocusOnError="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text">
                            <asp:Label runat="server" ID="lblYear" Text="Year:" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddYear" runat="server" Visible="true" ValidationGroup="vgCustomerProgram">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvYear" runat="server" ControlToValidate="ddYear"
                                ErrorMessage="Year is required." Font-Bold="True" ValidationGroup="vgCustomerProgram"
                                Text="<" SetFocusOnError="true" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Button runat="server" ID="btnSaveCustomerProgram" Text="Add Customer/Program"
                                CausesValidation="true" ValidationGroup="vgCustomerProgram" />
                            <asp:Button runat="server" ID="btnCancelEditCustomerProgram" Text="Reset Customer/Program"
                                Visible="false" />
                        </td>
                    </tr>
                </table>
                <asp:Label ID="lblMessageCustomerProgramBottom" runat="server" SkinID="MessageLabelSkin" />
                <br />
                <asp:GridView ID="gvCustomerProgram" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                    AllowPaging="True" PageSize="15" DataKeyNames="RowID" DataSourceID="odsCustomerProgram"
                    EmptyDataText="No Programs or Customers found" Width="600px">
                    <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#CCCCCC" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="RowID" HeaderText="RowID" SortExpression="RowID" Visible="false" />
                        <asp:BoundField DataField="ProgramID" HeaderText="ProgramID" SortExpression="ProgramID"
                            Visible="false" />
                        <asp:BoundField DataField="ddCustomerDesc" HeaderText="Customer" SortExpression="ddCustomerDesc"
                            ReadOnly="True"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField DataField="ddProgramName" HeaderText="Program" SortExpression="ddProgramName"
                            ReadOnly="True"  HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
                        <asp:BoundField DataField="ProgramYear" HeaderText="Year" SortExpression="ProgramYear"
                            ReadOnly="True" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                              <%--  <asp:ImageButton ID="iBtnCustomerProgramEdit" runat="server" CausesValidation="False"
                                    CommandName="Select" ImageUrl="~/images/edit.jpg" AlternateText="Edit" />--%>
                                <asp:ImageButton ID="iBtnCustomerProgramDelete" runat="server" CausesValidation="False"
                                    CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsCustomerProgram" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetCostReductionCustomerProgram" TypeName="CRCustomerProgramBLL"
                    DeleteMethod="DeleteCostReductionCustomerProgram">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="Int32" />
                    </SelectParameters>
                    <DeleteParameters>
                        <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="Int32" />
                        <asp:Parameter Name="original_RowID" Type="Int32" />
                    </DeleteParameters>
                </asp:ObjectDataSource>
                <br />
                <br />
                <hr />
                <table runat="server" id="tblCustomerGiveBackByDollar">
                    <tr>
                        <td class="p_text">
                            Customer Give Back $:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCustomerGiveBackDollar" MaxLength="10" Width="75px" />
                            <asp:RangeValidator ID="rvCustomerGiveBackDollar" runat="server" ControlToValidate="txtCustomerGiveBackDollar"
                                SetFocusOnError="true" Text="<" Display="Dynamic" ErrorMessage="Customer Give Back requires a numeric value -99,999,999.99 to 99,999,999.99"
                                Height="16px" MaximumValue="99999999.99" MinimumValue="-99999999.99" Type="Currency"
                                ValidationGroup="vgCalculate"><</asp:RangeValidator>
                            <ajax:FilteredTextBoxExtender ID="ftbeCustomerGiveBackDollar" runat="server" TargetControlID="txtCustomerGiveBackDollar"
                                FilterType="Custom, Numbers" ValidChars="-.," />
                        </td>
                    </tr>
                </table>
                <table runat="server" id="tblCustomerGiveBackByPercent" visible="false">
                    <tr>
                        <td class="p_text">
                            Customer Give Back %:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCustomerGiveBackPercent" MaxLength="10" Width="75px" />
                            <asp:RangeValidator ID="rvCustomerGiveBackPercent" runat="server" ControlToValidate="txtCustomerGiveBackPercent"
                                SetFocusOnError="true" Text="<" Display="Dynamic" ErrorMessage="Customer Give Back requires a numeric value -99.99 to 99.99"
                                Height="16px" MaximumValue="99.99" MinimumValue="-99.99" Type="Double" ValidationGroup="vgCalculate"><</asp:RangeValidator>
                            <ajax:FilteredTextBoxExtender ID="ftbeCustomerGiveBackPercent" runat="server" TargetControlID="txtCustomerGiveBackPercent"
                                FilterType="Custom, Numbers" ValidChars="-.," />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:RadioButtonList runat="server" ID="rbCustomerGiveBack" AutoPostBack="true" RepeatDirection="Horizontal">
                                <asp:ListItem Text="By Fixed Dollar" Value="D" Selected="True" />
                                <asp:ListItem Text="By Percent" Value="P" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vMaterialPrice" runat="server">
                <asp:Label ID="lblMessageMaterialPrice" SkinID="MessageLabelSkin" runat="server" />
                <asp:ValidationSummary ID="vsMaterialPrice" runat="server" DisplayMode="List" ShowMessageBox="true"
                    ShowSummary="true" EnableClientScript="true" ValidationGroup="vgMaterialPrice" />
                <br />
                <span style="text-decoration: underline; font-weight: bold">Cost Analysis:</span>
                <br />
                <br />
                <table width="98%">
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="c_textbold" style="color: Blue">
                            Actual Cost<br />
                            Per Unit
                        </td>
                        <td class="c_textbold" style="color: Blue">
                            Actual Annual<br />
                            Volume
                        </td>
                        <td class="c_textbold">
                            &nbsp;
                        </td>
                        <td class="c_textbold">
                            &nbsp;
                        </td>
                        <td class="c_textbold">
                            &nbsp;
                        </td>
                        <td class="c_textbold" style="color: Blue">
                            Budget Cost<br />
                            Per Unit
                        </td>
                        <td class="c_textbold" style="color: Blue">
                            Budget Annual<br />
                            Volume
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="5">
                            <span style="text-decoration: underline; font-style: italic">Current Method:</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            Actual Price:&nbsp;$
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMaterialPriceCurrentPrice" MaxLength="10" Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvMaterialPriceCurrentPrice" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="Current actual material price must be a number."
                                ValidationGroup="vgCalculate" ControlToValidate="txtMaterialPriceCurrentPrice"
                                SetFocusOnError="true" />
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMaterialPriceCurrentVolume" MaxLength="10" Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvMaterialPriceCurrentVolume" Operator="DataTypeCheck"
                                Type="integer" Text="<" ErrorMessage="Material actual volume must be an integer."
                                ValidationGroup="vgCalculate" ControlToValidate="txtMaterialPriceCurrentVolume"
                                SetFocusOnError="true" />
                        </td>
                        <td class="p_text">
                            $&nbsp;<asp:Label runat="server" ID="lblMaterialPriceCurrentPriceByVolume" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            Budget Price:&nbsp;$
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMaterialPriceCurrentPriceBudget" MaxLength="10"
                                Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvMaterialPriceCurrentPriceBudget" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="Current budget material price must be a number."
                                ValidationGroup="vgCalculate" ControlToValidate="txtMaterialPriceCurrentPriceBudget"
                                SetFocusOnError="true" />
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMaterialPriceCurrentVolumeBudget" MaxLength="10"
                                Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvMaterialPriceCurrentVolumeBudget" Operator="DataTypeCheck"
                                Type="integer" Text="<" ErrorMessage="Material budget volume must be an integer."
                                ValidationGroup="vgCalculate" ControlToValidate="txtMaterialPriceCurrentVolumeBudget"
                                SetFocusOnError="true" />
                        </td>
                        <td class="p_text">
                            $&nbsp;<asp:Label runat="server" ID="lblMaterialPriceCurrentPriceByVolumeBudget" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            Actual Freight:&nbsp;$
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMaterialPriceCurrentFreight" Width="75px" MaxLength="10" />
                            <asp:CompareValidator runat="server" ID="cvMaterialPriceCurrentFreight" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="Current actual material freight must be a number."
                                ValidationGroup="vgCalculate" ControlToValidate="txtMaterialPriceCurrentFreight"
                                SetFocusOnError="true" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text">
                            $&nbsp;<asp:Label runat="server" ID="lblMaterialPriceCurrentFreightByVolume" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            Budget Freight:&nbsp;$
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMaterialPriceCurrentFreightBudget" Width="75px"
                                MaxLength="10" />
                            <asp:CompareValidator runat="server" ID="cvMaterialPriceCurrentFreightBudget" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="Current budget material freight must be a number."
                                ValidationGroup="vgCalculate" ControlToValidate="txtMaterialPriceCurrentFreightBudget"
                                SetFocusOnError="true" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text">
                            $&nbsp;<asp:Label runat="server" ID="lblMaterialPriceCurrentFreightByVolumeBudget" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_textbold">
                            Actual Material Landed:&nbsp;$
                        </td>
                        <td class="c_textbold">
                            <asp:Label runat="server" ID="lblMaterialPriceCurrentMaterialLanded" />
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_textbold">
                            $&nbsp;
                            <asp:Label runat="server" ID="lblMaterialPriceCurrentMaterialLandedTotal" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_textbold">
                            Budget Material Landed:&nbsp;$
                        </td>
                        <td class="c_textbold">
                            <asp:Label runat="server" ID="lblMaterialPriceCurrentMaterialLandedBudget" />
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_textbold">
                            $&nbsp;
                            <asp:Label runat="server" ID="lblMaterialPriceCurrentMaterialLandedTotalBudget" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="10">
                            <span style="text-decoration: underline; font-style: italic">Proposed Method:</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            Price:&nbsp;$
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMaterialPriceProposedPrice" MaxLength="10" Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvMaterialPriceProposedPrice" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="Proposed price must be a number." ValidationGroup="vgCalculate"
                                ControlToValidate="txtMaterialPriceProposedPrice" SetFocusOnError="true" />
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMaterialPriceProposedVolume" MaxLength="10" Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvMaterialPriceProposedVolume" Operator="DataTypeCheck"
                                Type="integer" Text="<" ErrorMessage="Proposed volume must be an integer." ValidationGroup="vgCalculate"
                                ControlToValidate="txtMaterialPriceProposedVolume" SetFocusOnError="true" />
                        </td>
                        <td class="p_text">
                            $&nbsp;<asp:Label runat="server" ID="lblMaterialPriceProposedPriceByVolume" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            Freight:&nbsp;$
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMaterialPriceProposedFreight" Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvMaterialPriceProposedFreight" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="Proposed freight must be a number." ValidationGroup="vgCalculate"
                                ControlToValidate="txtMaterialPriceProposedFreight" SetFocusOnError="true" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text">
                            $&nbsp;<asp:Label runat="server" ID="lblMaterialPriceProposedFreightByVolume" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_textbold">
                            Material Landed:&nbsp;$
                        </td>
                        <td class="c_textbold">
                            <asp:Label runat="server" ID="lblMaterialPriceProposedMaterialLanded" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_textbold">
                            $&nbsp;
                            <asp:Label runat="server" ID="lblMaterialPriceProposedMaterialLandedTotal" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="11">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="11">
                            <span style="text-decoration: underline; font-weight: bold">Savings Analysis:</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text">
                            Actual Current Method:
                        </td>
                        <td class="p_text">
                            $&nbsp;<asp:Label runat="server" ID="lblMaterialPriceCurrentMethod" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text">
                            Budget Current Method:
                        </td>
                        <td class="p_text">
                            $&nbsp;<asp:Label runat="server" ID="lblMaterialPriceCurrentMethodBudget" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text">
                            Less Proposed Method:
                        </td>
                        <td class="p_text">
                            $&nbsp;<asp:Label runat="server" ID="lblMaterialPriceProposedMethod" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_textbold">
                            Actual Annual<br />
                            Material Savings:
                        </td>
                        <td class="p_textbold" valign="top">
                            $&nbsp;<asp:Label runat="server" ID="lblMaterialPriceSavings" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_textbold">
                            Budget Annual<br />
                            Material Savings:
                        </td>
                        <td class="p_textbold" valign="top">
                            $&nbsp;<asp:Label runat="server" ID="lblMaterialPriceSavingsBudget" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="11">
                            <hr />
                        </td>
                    </tr>
                </table>
                <table width="98%">
                    <tr>
                        <td colspan="9">
                            <span style="text-decoration: underline; font-weight: bold">Capital and Expenses:</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            New Capital:&nbsp;$
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMaterialPriceCECapital" MaxLength="10" Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvMaterialPriceCECapital" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="Capital Expense Capital must be a number."
                                ValidationGroup="vgCalculate" ControlToValidate="txtMaterialPriceCECapital" SetFocusOnError="true" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            Materials:&nbsp;$
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMaterialPriceCEMaterial" MaxLength="10" Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvMaterialPriceCEMaterial" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="Capital Expense Material must be a number."
                                ValidationGroup="vgCalculate" ControlToValidate="txtMaterialPriceCEMaterial"
                                SetFocusOnError="true" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            Outside Support:&nbsp;$
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMaterialPriceCEOutsideSupport" MaxLength="10"
                                Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvMaterialPriceCEOutsideSupport" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="Capital Expense Outside Support must be a number."
                                ValidationGroup="vgCalculate" ControlToValidate="txtMaterialPriceCEOutsideSupport"
                                SetFocusOnError="true" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            Misc.:&nbsp;$
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMaterialPriceCEMisc" MaxLength="10" Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvMaterialPriceCEMisc" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="Capital Expense miscellaneous must be a number."
                                ValidationGroup="vgCalculate" ControlToValidate="txtMaterialPriceCEMisc" SetFocusOnError="true" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            In-House Support:&nbsp;$
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMaterialPriceCEInHouseSupport" MaxLength="10"
                                Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvMaterialPriceCEInHouseSupport" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="Capital Expense In-House must be a number."
                                ValidationGroup="vgCalculate" ControlToValidate="txtMaterialPriceCEInHouseSupport"
                                SetFocusOnError="true" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_textbold">
                            Total Capital and Expense:&nbsp;$
                        </td>
                        <td class="c_textbold">
                            <asp:Label runat="server" ID="lblMaterialPriceCETotal" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <span style="text-decoration: underline; font-weight: bold">Payback Analysis:</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="5" align="center">
                            Capital Expenses / Annual Savings = &nbsp
                            <asp:Label runat="server" ID="lblMaterialPriceSavingsANDCE" />
                            &nbsp
                            <asp:Label runat="server" ID="lblMaterialPricePayback" CssClass="c_textbold" />
                            &nbsp years
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vMaterialUsage" runat="server">
                <asp:Label ID="lblMessageMaterialUsage" SkinID="MessageLabelSkin" runat="server" />
                <br />
                <span style="text-decoration: underline; font-weight: bold">Cost Analysis:</span>
                <br />
                <br />
                <table width="98%">
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="4">
                            <span style="text-decoration: underline; font-style: italic">Current Method:</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            Actual Cost Per Unit with Freight:&nbsp;$
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMaterialUsageCurrentCostPerUnit" MaxLength="10"
                                Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvMaterialUsageCurrentCostPerUnit" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="Actual Cost per unit must be a number."
                                ValidationGroup="vgCalculate" ControlToValidate="txtMaterialUsageCurrentCostPerUnit"
                                SetFocusOnError="true" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            Budget Cost Per Unit with Freight:&nbsp;$
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMaterialUsageCurrentCostPerUnitBudget" MaxLength="10"
                                Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvMaterialUsageCurrentCostPerUnitBudget"
                                Operator="DataTypeCheck" Type="double" Text="<" ErrorMessage="Budget Cost per unit must be a number."
                                ValidationGroup="vgCalculate" ControlToValidate="txtMaterialUsageCurrentCostPerUnitBudget"
                                SetFocusOnError="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            Actual Units Per Each Parent:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMaterialUsageCurrentUnitPerParent" MaxLength="10"
                                Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvMaterialUsageCurrentUnitPerParent" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="Actual Current Unit Per Parent must be a number."
                                ValidationGroup="vgCalculate" ControlToValidate="txtMaterialUsageCurrentUnitPerParent"
                                SetFocusOnError="true" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            Budget Units Per Each Parent:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMaterialUsageCurrentUnitPerParentBudget" MaxLength="10"
                                Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvMaterialUsageCurrentUnitPerParentBudget"
                                Operator="DataTypeCheck" Type="double" Text="<" ErrorMessage="Budget Current Unit Per Parent must be a number."
                                ValidationGroup="vgCalculate" ControlToValidate="txtMaterialUsageCurrentUnitPerParentBudget"
                                SetFocusOnError="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_textbold">
                            Actual Total Cost in Material:&nbsp;$
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblMaterialUsageCurrentCostTotal" CssClass="c_textbold" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_textbold">
                            Budget Total Cost in Material:&nbsp;$
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblMaterialUsageCurrentCostTotalBudget" CssClass="c_textbold" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="4">
                            <span style="text-decoration: underline; font-style: italic">Proposed Method:</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            Cost Per Unit with Freight:&nbsp;$
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMaterialUsageProposedCostPerUnit" MaxLength="10"
                                Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvMaterialUsageProposedCostPerUnit" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="Proposed Cost Per Unit must be a number."
                                ValidationGroup="vgCalculate" ControlToValidate="txtMaterialUsageProposedCostPerUnit"
                                SetFocusOnError="true" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            Units Per Each Parent:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMaterialUsageProposedUnitPerParent" MaxLength="10"
                                Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvMaterialUsageProposedUnitPerParent" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="Proposed Unit Per Parent must be a number."
                                ValidationGroup="vgCalculate" ControlToValidate="txtMaterialUsageProposedUnitPerParent"
                                SetFocusOnError="true" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_textbold">
                            Total Cost in Material:&nbsp;$
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblMaterialUsageProposedCostTotal" CssClass="c_textbold" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            Actual Volume of Program:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMaterialUsageProgramVolume" MaxLength="10" Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvMaterialUsageProgramVolume" Operator="DataTypeCheck"
                                Type="integer" Text="<" ErrorMessage="Actual Volume must be an integer." ValidationGroup="vgCalculate"
                                ControlToValidate="txtMaterialUsageProgramVolume" SetFocusOnError="true" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            Budget Volume of Program:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMaterialUsageProgramVolumeBudget" MaxLength="10"
                                Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvMaterialUsageProgramVolumeBudget" Operator="DataTypeCheck"
                                Type="integer" Text="<" ErrorMessage="Budget Volume must be an integer." ValidationGroup="vgCalculate"
                                ControlToValidate="txtMaterialUsageProgramVolumeBudget" SetFocusOnError="true" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="9">
                            <span style="text-decoration: underline; font-weight: bold">Savings Analysis:</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text">
                            Actual Current Method:
                        </td>
                        <td class="c_text">
                            $&nbsp;<asp:Label runat="server" ID="lblMaterialUsageCurrentMethod" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text">
                            Budget Current Method:
                        </td>
                        <td class="c_text">
                            $&nbsp;<asp:Label runat="server" ID="lblMaterialUsageCurrentMethodBudget" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text">
                            Less Proposed Method:
                        </td>
                        <td class="c_text">
                            $&nbsp;<asp:Label runat="server" ID="lblMaterialUsageProposedMethod" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_textbold">
                            Actual Annual Material Savings:
                        </td>
                        <td class="c_textbold">
                            $&nbsp;<asp:Label runat="server" ID="lblMaterialUsageSavings" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_textbold">
                            Budget Annual Material Savings:
                        </td>
                        <td class="c_textbold">
                            $&nbsp;<asp:Label runat="server" ID="lblMaterialUsageSavingsBudget" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="9">
                            <hr />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td colspan="5">
                            <span style="text-decoration: underline; font-weight: bold">Capital and Expenses:</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            New Capital:&nbsp;$
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMaterialUsageCECapital" MaxLength="10" Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvMaterialUsageCECapital" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="Capital Expense Capital must be a number."
                                ValidationGroup="vgCalculate" ControlToValidate="txtMaterialUsageCECapital" SetFocusOnError="true" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            Materials:&nbsp;$
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMaterialUsageCEMaterial" MaxLength="10" Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvMaterialUsageCEMaterial" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="Capital Expense Material must be a number."
                                ValidationGroup="vgCalculate" ControlToValidate="txtMaterialUsageCEMaterial"
                                SetFocusOnError="true" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            Outside Support:&nbsp;$
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMaterialUsageCEOutsideSupport" MaxLength="10"
                                Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvMaterialUsageCEOutsideSupport" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="Capital Expense Ouside Support must be a number."
                                ValidationGroup="vgCalculate" ControlToValidate="txtMaterialUsageCEOutsideSupport"
                                SetFocusOnError="true" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            Misc.:&nbsp;$
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMaterialUsageCEMisc" MaxLength="10" Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvMaterialUsageCEMisc" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="Capital Expense miscellaneous must be a number."
                                ValidationGroup="vgCalculate" ControlToValidate="txtMaterialUsageCEMisc" SetFocusOnError="true" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            In-House Support:&nbsp;$
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMaterialUsageCEInHouseSupport" MaxLength="10"
                                Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvMaterialUsageCEInHouseSupport" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="Capital Expense In-House must be a number."
                                ValidationGroup="vgCalculate" ControlToValidate="txtMaterialUsageCEInHouseSupport"
                                SetFocusOnError="true" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_textbold">
                            Total Capital and Expense:&nbsp;$
                        </td>
                        <td class="c_textbold">
                            <asp:Label runat="server" ID="lblMaterialUsageCETotal" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <table width="98%">
                    <tr>
                        <td colspan="9">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <span style="text-decoration: underline; font-weight: bold">Payback Analysis:</span>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="4" align="center">
                            Capital Expenses / Annual Savings = &nbsp
                            <asp:Label runat="server" ID="lblMaterialUsageSavingsANDCE" />
                            &nbsp
                            <asp:Label runat="server" ID="lblMaterialUsagePayback" CssClass="c_textbold" />
                            &nbsp years
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vCycleTime" runat="server">
                <asp:Label ID="lblMessageCycleTime" SkinID="MessageLabelSkin" runat="server" />
                <br />
                <span style="text-decoration: underline; font-weight: bold">Cost Analysis:</span>
                <br />
                <br />
                <table width="98%">
                    <tr>
                        <td colspan="9">
                            <span style="text-decoration: underline; font-style: italic">Current Method:</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td class="c_text" style="color: Blue">
                            Actual
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td style="width: 10%">
                            &nbsp;
                        </td>
                        <td style="width: 10%">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="c_text" style="color: Blue">
                            Budget
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td style="width: 10%">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" style="color: Blue">
                            Pieces Per Hour:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCycleTimeCurrentPiecesPerHour" MaxLength="10"
                                Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvCycleTimeCurrentPiecesPerHour" Operator="DataTypeCheck"
                                Type="integer" Text="<" ErrorMessage="Actual Current pieces per hour must be an integer."
                                ValidationGroup="vgCalculate" ControlToValidate="txtCycleTimeCurrentPiecesPerHour"
                                SetFocusOnError="true" />
                        </td>
                        <td class="p_text">
                            Machine Hour / Pieces:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblCycleTimeCurrentMachineHourPerPieces" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            Pieces / Hour:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCycleTimeCurrentPiecesPerHourBudget" MaxLength="10"
                                Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvCycleTimeCurrentPiecesPerHourBudget" Operator="DataTypeCheck"
                                Type="integer" Text="<" ErrorMessage="Budget Current pieces per hour must be an integer."
                                ValidationGroup="vgCalculate" ControlToValidate="txtCycleTimeCurrentPiecesPerHourBudget"
                                SetFocusOnError="true" />
                        </td>
                        <td class="p_text">
                            Machine Hour / Pieces:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblCycleTimeCurrentMachineHourPerPiecesBudget" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" style="color: Blue">
                            Crew Size:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCycleTimeCurrentCrewSize" MaxLength="10" Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvCycleTimeCurrentCrewSize" Operator="DataTypeCheck"
                                Type="integer" Text="<" ErrorMessage="Actual Current crew size must be an integer."
                                ValidationGroup="vgCalculate" ControlToValidate="txtCycleTimeCurrentCrewSize"
                                SetFocusOnError="true" />
                        </td>
                        <td class="p_text">
                            Man Hour / Pieces:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblCycleTimeCurrentManHourPerPieces" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            Crew Size:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCycleTimeCurrentCrewSizeBudget" MaxLength="10"
                                Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvCycleTimeCurrentCrewSizeBudget" Operator="DataTypeCheck"
                                Type="integer" Text="<" ErrorMessage="Budget Current crew size must be an integer."
                                ValidationGroup="vgCalculate" ControlToValidate="txtCycleTimeCurrentCrewSizeBudget"
                                SetFocusOnError="true" />
                        </td>
                        <td class="p_text">
                            Man Hour / Pieces:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblCycleTimeCurrentManHourPerPiecesBudget" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" style="color: Blue">
                            Actual Volume:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCycleTimeCurrentVolume" MaxLength="10" Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvCycleTimeCurrentVolume" Operator="DataTypeCheck"
                                Type="integer" Text="<" ErrorMessage="Actual Current volume be an integer." ValidationGroup="vgCalculate"
                                ControlToValidate="txtCycleTimeCurrentVolume" SetFocusOnError="true" />
                        </td>
                        <td class="p_textbold">
                            Total Man Hours<br />
                            to Produce Volume:
                        </td>
                        <td valign="top">
                            <asp:Label runat="server" ID="lblCycleTimeCurrentTotalManHours" CssClass="c_textbold" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            Budget Volume:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCycleTimeCurrentVolumeBudget" MaxLength="10" Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvCycleTimeCurrentVolumeBudget" Operator="DataTypeCheck"
                                Type="integer" Text="<" ErrorMessage="Budget Current volume be an integer." ValidationGroup="vgCalculate"
                                ControlToValidate="txtCycleTimeCurrentVolumeBudget" SetFocusOnError="true" />
                        </td>
                        <td class="p_textbold">
                            Total Man Hours<br />
                            to Produce Volume:
                        </td>
                        <td valign="top">
                            <asp:Label runat="server" ID="lblCycleTimeCurrentTotalManHoursBudget" CssClass="c_textbold" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="9">
                            <span style="text-decoration: underline; font-style: italic">Proposed Method:</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" style="color: Blue">
                            Pieces / Hour:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCycleTimeProposedPiecesPerHour" MaxLength="10"
                                Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvCycleTimeProposedPiecesPerHour" Operator="DataTypeCheck"
                                Type="integer" Text="<" ErrorMessage="Proposed pieces per hour must be an integer."
                                ValidationGroup="vgCalculate" ControlToValidate="txtCycleTimeProposedPiecesPerHour"
                                SetFocusOnError="true" />
                        </td>
                        <td class="p_text">
                            Machine Hour / Pieces:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblCycleTimeProposedMachineHourPerPieces" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" style="color: Blue">
                            Crew Size:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCycleTimeProposedCrewSize" MaxLength="10" Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvCycleTimeProposedCrewSize" Operator="DataTypeCheck"
                                Type="integer" Text="<" ErrorMessage="Proposed crew size must be an integer."
                                ValidationGroup="vgCalculate" ControlToValidate="txtCycleTimeProposedCrewSize"
                                SetFocusOnError="true" />
                        </td>
                        <td class="p_text">
                            Man Hour / Pieces:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblCycleTimeProposedManHourPerPieces" />
                        </td>
                    </tr>
                    <tr>
                        <td class="p_text" style="color: Blue">
                            Volume:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCycleTimeProposedVolume" MaxLength="10" Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvCycleTimeProposedVolume" Operator="DataTypeCheck"
                                Type="integer" Text="<" ErrorMessage="Proposed volume must be an integer." ValidationGroup="vgCalculate"
                                ControlToValidate="txtCycleTimeProposedVolume" SetFocusOnError="true" />
                        </td>
                        <td class="p_textbold">
                            Total Man Hours<br />
                            to Produce Volume:
                        </td>
                        <td valign="top">
                            <asp:Label runat="server" ID="lblCycleTimeProposedTotalManHours" CssClass="c_textbold" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="9">
                            <hr />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td colspan="7">
                            <span style="text-decoration: underline; font-weight: bold">Fringes and Rates:</span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7" style="color: Red; font-style: italic">
                            An example for all percentages below would be to type 12.5 for 12.5%.
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            FUTA Rate:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCycleTimeFUTARate" MaxLength="10" Width="75px" />
                            % &nbsp;
                            <asp:CompareValidator runat="server" ID="cvCycleTimeFUTARate" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="FUTA Rate must be a number." ValidationGroup="vgCalculate"
                                ControlToValidate="txtCycleTimeFUTARate" SetFocusOnError="true" />
                            &nbsp;
                            <asp:Label runat="server" ID="lblCycleTimeFUTARateDecimal" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            SUTA Rate:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCycleTimeSUTARate" MaxLength="10" Width="75px" />
                            % &nbsp;
                            <asp:CompareValidator runat="server" ID="cvCycleTimeSUTARate" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="SUTA Rate must be a number." ValidationGroup="vgCalculate"
                                ControlToValidate="txtCycleTimeSUTARate" SetFocusOnError="true" />
                            <asp:Label runat="server" ID="lblCycleTimeSUTARateDecimal" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            FICA Rate:
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCycleTimeFICARate" MaxLength="10" Width="75px" />
                            % &nbsp;
                            <asp:CompareValidator runat="server" ID="cvtxtCycleTimeFICARate" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="FICA Rate must be a number." ValidationGroup="vgCalculate"
                                ControlToValidate="txtCycleTimeFICARate" SetFocusOnError="true" />
                            <asp:Label runat="server" ID="lblCycleTimeFICARateDecimal" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text">
                            Total Variable Fringes:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblCycleTimeVariableFringes" />
                            % &nbsp
                            <asp:Label runat="server" ID="lblCycleTimeVariableFringesDecimal" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            Wages:&nbsp;$
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCycleTimeWages" MaxLength="10" Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvCycleTimeWages" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="Wages must be a number." ValidationGroup="vgCalculate"
                                ControlToValidate="txtCycleTimeWages" SetFocusOnError="true" />
                            &nbsp (Wage / Hour)
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text">
                            Wages Plus Fringes:&nbsp;$
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblCycleTimeWagesPlusFringes" CssClass="c_textbold" />
                        </td>
                    </tr>
                </table>
                <table width="98%">
                    <tr>
                        <td colspan="7">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <span style="text-decoration: underline; font-weight: bold">Savings Analysis:</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text">
                            Actual Current Method:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblCycleTimeCurrentMethod" CssClass="c_textbold" />
                            &nbsp; (Total Man Hours Required)
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text">
                            Budget Current Method:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblCycleTimeCurrentMethodBudget" CssClass="c_textbold" />
                            &nbsp; (Total Man Hours Required)
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text">
                            Less Proposed Method:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblCycleTimeProposedMethod" CssClass="c_textbold" />
                            &nbsp; (Total Man Hours Required)
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text">
                            Actual Difference:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblCycleTimeMethodDifference" CssClass="c_textbold" />
                            &nbsp; (Total Man Hours Saved)
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text">
                            Budget Difference:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblCycleTimeMethodDifferenceBudget" CssClass="c_textbold" />
                            &nbsp; (Total Man Hours Saved)
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_textbold">
                            Actual Annual Savings:&nbsp$
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblCycleTimeSavings" CssClass="c_textbold" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_textbold">
                            Budget Annual Savings:&nbsp$
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblCycleTimeSavingsBudget" CssClass="c_textbold" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="9">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <span style="text-decoration: underline; font-weight: bold">Capital and Expenses:</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            New Capital:&nbsp;$
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCycleTimeCECapital" MaxLength="10" Width="75px">
                            </asp:TextBox>
                            <asp:CompareValidator runat="server" ID="cvCycleTimeCECapital" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="Capital Expense Capital must be a number."
                                ValidationGroup="vgCalculate" ControlToValidate="txtCycleTimeCECapital" SetFocusOnError="true" />
                        </td>
                        <td colspan="5">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            Materials:&nbsp;$
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCycleTimeCEMaterial" MaxLength="10" Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvCycleTimeCEMaterial" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="Capital Expense Material must be a number."
                                ValidationGroup="vgCalculate" ControlToValidate="txtCycleTimeCEMaterial" SetFocusOnError="true" />
                        </td>
                        <td colspan="5">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            Outside Support:&nbsp;$
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCycleTimeCEOutsideSupport" MaxLength="10" Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvCycleTimeCEOutsideSupport" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="Capital Expense Outside Support must be a number."
                                ValidationGroup="vgCalculate" ControlToValidate="txtCycleTimeCEOutsideSupport"
                                SetFocusOnError="true" />
                        </td>
                        <td colspan="5">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            Misc.:&nbsp;$
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCycleTimeCEMisc" MaxLength="10" Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvCycleTimeCEMisc" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="Capital Expense miscellaneous must be a number."
                                ValidationGroup="vgCalculate" ControlToValidate="txtCycleTimeCEMisc" SetFocusOnError="true" />
                        </td>
                        <td colspan="5">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            In-House Support:&nbsp;$
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCycleTimeCEInHouseSupport" MaxLength="10" Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvCycleTimeCEInHouseSupport" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="Capital Expense In-House Support must be a number."
                                ValidationGroup="vgCalculate" ControlToValidate="txtCycleTimeCEInHouseSupport"
                                SetFocusOnError="true" />
                        </td>
                        <td colspan="5">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_textbold">
                            Total Capital and Expense:&nbsp;$
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblCycleTimeCETotal" CssClass="c_textbold" />
                        </td>
                        <td colspan="5">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <span style="text-decoration: underline; font-weight: bold">Payback Analysis:</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="6" align="center">
                            Capital Expenses / Annual Savings = &nbsp
                            <asp:Label runat="server" ID="lblCycleTimeSavingsANDCE" />
                            &nbsp
                            <asp:Label runat="server" ID="lblCycleTimePayback" CssClass="c_textbold" />
                            &nbsp years
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vHeadCount" runat="server">
                <asp:Label ID="lblMessageHeadCount" SkinID="MessageLabelSkin" runat="server" />
                <br />
                <span style="text-decoration: underline; font-weight: bold">Cost Analysis:</span>
                <br />
                <br />
                <table width="98%">
                    <tr>
                        <td valign="top">
                            <table>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td class="c_textbold" style="color: Blue">
                                        Actual
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td class="c_textbold" style="color: Blue">
                                        Budget
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td class="p_text" style="color: Blue">
                                        Wages:&nbsp;$
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtHeadCountWages" MaxLength="10" Width="75px" />
                                        <asp:CompareValidator runat="server" ID="cvHeadCountWages" Operator="DataTypeCheck"
                                            Type="double" Text="<" ErrorMessage="Actual Wages must be a number." ValidationGroup="vgCalculate"
                                            ControlToValidate="txtHeadCountWages" SetFocusOnError="true" />
                                        (Wage / Hour)
                                    </td>
                                    <td class="p_text" style="white-space: nowrap">
                                        Annual Labor Cost:
                                    </td>
                                    <td class="p_text">
                                        $&nbsp;<asp:Label runat="server" ID="lblHeadCountAnnualLaborCost" />
                                    </td>
                                    <td class="p_text" style="color: Blue">
                                        Wages:&nbsp;$
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtHeadCountWagesBudget" MaxLength="10" Width="75px" />
                                        <asp:CompareValidator runat="server" ID="cvHeadCountWagesBudget" Operator="DataTypeCheck"
                                            Type="double" Text="<" ErrorMessage="Budget Wages must be a number." ValidationGroup="vgCalculate"
                                            ControlToValidate="txtHeadCountWagesBudget" SetFocusOnError="true" />
                                        (Wage / Hour)
                                    </td>
                                    <td class="p_text" style="white-space: nowrap">
                                        Annual Labor Cost:
                                    </td>
                                    <td class="p_text">
                                        $&nbsp;<asp:Label runat="server" ID="lblHeadCountAnnualLaborCostBudget" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td colspan="8">
                                        <span style="text-decoration: underline; font-style: italic">Current Method:</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td class="p_text" style="color: Blue">
                                        Head Count (D\L or I\D\L):
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtHeadCountCurrentLaborCount" MaxLength="10" Width="75px" />
                                        <asp:CompareValidator runat="server" ID="cvHeadCountCurrentLaborCount" Operator="DataTypeCheck"
                                            Type="integer" Text="<" ErrorMessage="Current Head Count must be an integer."
                                            ValidationGroup="vgCalculate" ControlToValidate="txtHeadCountCurrentLaborCount"
                                            SetFocusOnError="true" />
                                    </td>
                                    <td class="p_text">
                                        Labor Cost:
                                    </td>
                                    <td class="p_text" style="width: 10%">
                                        $&nbsp;<asp:Label runat="server" ID="lblHeadCountCurrentLaborCost" />
                                    </td>
                                    <td class="p_text" style="color: Blue">
                                        Head Count (D\L or I\D\L):
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtHeadCountCurrentLaborCountBudget" MaxLength="10"
                                            Width="75px" />
                                        <asp:CompareValidator runat="server" ID="cvHeadCountCurrentLaborCountBudget" Operator="DataTypeCheck"
                                            Type="integer" Text="<" ErrorMessage="Budget Current Head Count must be an integer."
                                            ValidationGroup="vgCalculate" ControlToValidate="txtHeadCountCurrentLaborCountBudget"
                                            SetFocusOnError="true" />
                                    </td>
                                    <td class="p_text">
                                        Labor Cost:
                                    </td>
                                    <td class="p_text" style="width: 10%">
                                        $&nbsp;<asp:Label runat="server" ID="lblHeadCountCurrentLaborCostBudget" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td class="p_text">
                                        Fringes:
                                    </td>
                                    <td class="p_text">
                                        $&nbsp;<asp:Label runat="server" ID="lblHeadCountCurrentLaborFringes" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td class="p_textbold" colspan="2">
                                        Actual Total Labor Cost:
                                    </td>
                                    <td class="p_textbold">
                                        $&nbsp;<asp:Label runat="server" ID="lblHeadCountCurrentLaborTotal" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td class="p_textbold" colspan="2">
                                        Budget Total Labor Cost:
                                    </td>
                                    <td class="p_textbold">
                                        $&nbsp;<asp:Label runat="server" ID="lblHeadCountCurrentLaborTotalBudget" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td colspan="8">
                                        <span style="text-decoration: underline; font-style: italic">Proposed Method:</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td class="p_text" style="color: Blue">
                                        Head Count (D\L or I\D\L):
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtHeadCountProposedLaborCount" MaxLength="10" Width="75px" />
                                        <asp:CompareValidator runat="server" ID="cvHeadCountProposedLaborCount" Operator="DataTypeCheck"
                                            Type="integer" Text="<" ErrorMessage="Proposed Head Count must be an integer."
                                            ValidationGroup="vgCalculate" ControlToValidate="txtHeadCountProposedLaborCount"
                                            SetFocusOnError="true" />
                                    </td>
                                    <td class="p_text">
                                        Labor Cost:
                                    </td>
                                    <td class="p_text">
                                        $&nbsp;<asp:Label runat="server" ID="lblHeadCountProposedLaborCost" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td class="p_text">
                                        Fringes:
                                    </td>
                                    <td class="p_text">
                                        $&nbsp;<asp:Label runat="server" ID="lblHeadCountProposedLaborFringes" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td class="p_textbold">
                                        Total Labor Cost:
                                    </td>
                                    <td class="p_textbold">
                                        $&nbsp;<asp:Label runat="server" ID="lblHeadCountProposedLaborTotal" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="9">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="9">
                                        <span style="text-decoration: underline; font-weight: bold">Savings Analysis:</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        &nbsp;
                                    </td>
                                    <td colspan="2" class="p_text">
                                        Actual Current Method:
                                    </td>
                                    <td class="p_text">
                                        $&nbsp;<asp:Label runat="server" ID="lblHeadCountCurrentMethod" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td colspan="2" class="p_text">
                                        Budget Current Method:
                                    </td>
                                    <td class="p_text">
                                        $&nbsp;<asp:Label runat="server" ID="lblHeadCountCurrentMethodBudget" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        &nbsp;
                                    </td>
                                    <td colspan="2" class="p_text">
                                        Actual Less Proposed Method:
                                    </td>
                                    <td class="p_text">
                                        $&nbsp;<asp:Label runat="server" ID="lblHeadCountProposedMethod" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        &nbsp;
                                    </td>
                                    <td colspan="2" class="p_textbold">
                                        Actual Annual Labor Savings:
                                    </td>
                                    <td class="p_textbold">
                                        $&nbsp;<asp:Label runat="server" ID="lblHeadCountSavings" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td colspan="2" class="p_textbold">
                                        Budget Annual Labor Savings:
                                    </td>
                                    <td class="p_textbold">
                                        $&nbsp;<asp:Label runat="server" ID="lblHeadCountSavingsBudget" />
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td colspan="9">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="9">
                                        <span style="text-decoration: underline; font-weight: bold">Capital and Expenses:</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td class="p_text" style="color: Blue">
                                        New Capital:&nbsp;$
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtHeadCountCECapital" MaxLength="10" Width="75px" />
                                        <asp:CompareValidator runat="server" ID="cvHeadCountCECapital" Operator="DataTypeCheck"
                                            Type="double" Text="<" ErrorMessage="Capital Expense Capital must be a number."
                                            ValidationGroup="vgCalculate" ControlToValidate="txtHeadCountCECapital" SetFocusOnError="true" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td class="p_text" style="color: Blue">
                                        Materials:&nbsp;$
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtHeadCountCEMaterial" MaxLength="10" Width="75px" />
                                        <asp:CompareValidator runat="server" ID="vcHeadCountCEMaterial" Operator="DataTypeCheck"
                                            Type="double" Text="<" ErrorMessage="Capital Expense Material must be a number."
                                            ValidationGroup="vgCalculate" ControlToValidate="txtHeadCountCECapital" SetFocusOnError="true" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td class="p_text" style="color: Blue">
                                        Outside Support:&nbsp;$
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtHeadCountCEOutsideSupport" MaxLength="10" Width="75px" />
                                        <asp:CompareValidator runat="server" ID="cvHeadCountCEOutsideSupport" Operator="DataTypeCheck"
                                            Type="double" Text="<" ErrorMessage="Capital Expense Outside Support must be a number."
                                            ValidationGroup="vgCalculate" ControlToValidate="txtHeadCountCEOutsideSupport"
                                            SetFocusOnError="true" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td class="p_text" style="color: Blue">
                                        Misc.:&nbsp;$
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtHeadCountCEMisc" MaxLength="10" Width="75px" />
                                        <asp:CompareValidator runat="server" ID="cvHeadCountCEMisc" Operator="DataTypeCheck"
                                            Type="double" Text="<" ErrorMessage="Capital Expense miscellaneous must be a number."
                                            ValidationGroup="vgCalculate" ControlToValidate="txtHeadCountCEMisc" SetFocusOnError="true" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td class="p_text" style="color: Blue">
                                        In-House Support:&nbsp;$
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtHeadCountCEInHouseSupport" MaxLength="10" Width="75px" />
                                        <asp:CompareValidator runat="server" ID="cvHeadCountCEInHouseSupport" Operator="DataTypeCheck"
                                            Type="double" Text="<" ErrorMessage="Capital Expense In-House must be a number."
                                            ValidationGroup="vgCalculate" ControlToValidate="txtHeadCountCEInHouseSupport"
                                            SetFocusOnError="true" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td class="p_textbold">
                                        Total Capital and Expense:&nbsp;$
                                    </td>
                                    <td class="c_textbold">
                                        <asp:Label runat="server" ID="lblHeadCountCETotal" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8">
                                        <span style="text-decoration: underline; font-weight: bold">Payback Analysis:</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td colspan="7" align="center">
                                        Capital Expenses / Annual Savings = &nbsp
                                        <asp:Label runat="server" ID="lblHeadCountSavingsANDCE" />
                                        &nbsp
                                        <asp:Label runat="server" ID="lblHeadCountPayback" CssClass="c_textbold" />
                                        &nbsp years
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table border="1" style="border-color: Black">
                                <tr>
                                    <td>
                                        <table>
                                            <tr>
                                                <td class="p_textbold">
                                                    Fringe Desc.
                                                </td>
                                                <td class="c_textbold" style="color: Blue">
                                                    Amount
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="p_text" style="color: Blue">
                                                    FUTA Capped:&nbsp;$
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtHeadCountFUTA" MaxLength="10" Width="75px" />
                                                    <asp:CompareValidator runat="server" ID="cvHeadCountFUTA" Operator="DataTypeCheck"
                                                        Type="double" Text="<" ErrorMessage="Fringe FUTA must be a number." ValidationGroup="vgCalculate"
                                                        ControlToValidate="txtHeadCountFUTA" SetFocusOnError="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="p_text" style="color: Blue">
                                                    SUTA Capped:&nbsp;$
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtHeadCountSUTA" MaxLength="10" Width="75px" />
                                                    <asp:CompareValidator runat="server" ID="cvHeadCountSUTA" Operator="DataTypeCheck"
                                                        Type="double" Text="<" ErrorMessage="Fringe SUTA must be a number." ValidationGroup="vgCalculate"
                                                        ControlToValidate="txtHeadCountSUTA" SetFocusOnError="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="p_text" style="color: Blue">
                                                    FICA Capped:&nbsp;$
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtHeadCountFICA" MaxLength="10" Width="75px" />
                                                    <asp:CompareValidator runat="server" ID="cvHeadCountFICA" Operator="DataTypeCheck"
                                                        Type="double" Text="<" ErrorMessage="Fringe FICA must be a number." ValidationGroup="vgCalculate"
                                                        ControlToValidate="txtHeadCountFICA" SetFocusOnError="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="p_text" style="color: Blue">
                                                    Pension (401K):&nbsp;$
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtHeadCountPension" MaxLength="10" Width="75px" />
                                                    <asp:CompareValidator runat="server" ID="cvHeadCountPension" Operator="DataTypeCheck"
                                                        Type="double" Text="<" ErrorMessage="Fringe Pension must be a number." ValidationGroup="vgCalculate"
                                                        ControlToValidate="txtHeadCountPension" SetFocusOnError="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="p_text" style="color: Blue">
                                                    Bonus:&nbsp;$
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtHeadCountBonus" MaxLength="10" Width="75px" />
                                                    <asp:CompareValidator runat="server" ID="cvtxtHeadCountBonus" Operator="DataTypeCheck"
                                                        Type="double" Text="<" ErrorMessage="Fringe Bonus must be a number." ValidationGroup="vgCalculate"
                                                        ControlToValidate="txtHeadCountPension" SetFocusOnError="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="p_text" style="color: Blue">
                                                    Life/LTD/AD &amp D:&nbsp;$
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtHeadCountLife" MaxLength="10" Width="75px" />
                                                    <asp:CompareValidator runat="server" ID="cvHeadCountLife" Operator="DataTypeCheck"
                                                        Type="double" Text="<" ErrorMessage="Fringe life/LTD/AD&D must be a number."
                                                        ValidationGroup="vgCalculate" ControlToValidate="txtHeadCountLife" SetFocusOnError="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="p_text" style="color: Blue">
                                                    Group Insurance:&nbsp;$
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtHeadCountGroupInsurance" MaxLength="10" Width="75px" />
                                                    <asp:CompareValidator runat="server" ID="cvHeadCountGroupInsurance" Operator="DataTypeCheck"
                                                        Type="double" Text="<" ErrorMessage="Fringe Group Insurance must be a number."
                                                        ValidationGroup="vgCalculate" ControlToValidate="txtHeadCountGroupInsurance"
                                                        SetFocusOnError="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="p_text">
                                                    Workers Comp:&nbsp;$
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblHeadCountWorkersComp" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="p_text" style="color: Blue">
                                                    401k Quarterly:&nbsp;$
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtHeadCountPensionQuarterly" MaxLength="10" Width="75px" />
                                                    <asp:CompareValidator runat="server" ID="cvHeadCountPensionQuarterly" Operator="DataTypeCheck"
                                                        Type="double" Text="<" ErrorMessage="Fringe Pension Quarterly must be a number."
                                                        ValidationGroup="vgCalculate" ControlToValidate="txtHeadCountPensionQuarterly"
                                                        SetFocusOnError="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="p_text">
                                                    Total Fringes:&nbsp;$
                                                </td>
                                                <td class="c_textbold">
                                                    <asp:Label runat="server" ID="lblHeadCountTotalFringes" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vOverhead" runat="server">
                <asp:Label ID="lblMessageOverhead" SkinID="MessageLabelSkin" runat="server" />
                <asp:ValidationSummary ID="vsInsertOverheadCurrent" runat="server" DisplayMode="List"
                    ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgInsertOverheadCurrent" />
                <asp:ValidationSummary ID="vsEditOverheadCurrent" runat="server" DisplayMode="List"
                    ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgEditOverheadCurrent" />
                <asp:ValidationSummary ID="vsInsertOverheadProposed" runat="server" DisplayMode="List"
                    ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgInsertOverheadProposed" />
                <asp:ValidationSummary ID="vsEditOverheadProposed" runat="server" DisplayMode="List"
                    ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgEditOverheadProposed" />
                <br />
                <span style="text-decoration: underline; font-weight: bold">Cost Analysis:</span>
                <br />
                <br />
                <table width="98%">
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="5">
                            <span style="font-size: large; underline; font-style: italic">Current Method:</span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:GridView ID="gvOverheadCurrent" runat="server" AutoGenerateColumns="False" DataKeyNames="RowID"
                                DataSourceID="odsOverheadCurrent" EmptyDataText="No overhead found" AllowSorting="True"
                                AllowPaging="True" PageSize="15" ShowFooter="True">
                                <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#CCCCCC" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField DataField="RowID" HeaderText="RowID" ReadOnly="True" SortExpression="RowID" />
                                    <asp:BoundField DataField="ProjectNo" HeaderText="ProjectNo" SortExpression="ProjectNo" />
                                    <asp:TemplateField HeaderText="Expensed Item Name" SortExpression="ExpensedName"
                                        ControlStyle-Width="100px">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditCurrentExpensedName" runat="server" Text='<%# Bind("ExpensedName") %>'
                                                MaxLength="50" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewCurrentExpensedName" runat="server" Text='<%# Bind("ExpensedName") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtInsertCurrentExpensedName" runat="server" MaxLength="50" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Actual<br> Cost Per Unit" SortExpression="CurrentCostPerUnit"
                                        ControlStyle-Width="75px">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditCurrentCostPerUnit" runat="server" Text='<%# Bind("CurrentCostPerUnit") %>'
                                                MaxLength="10" Width="75px" />
                                            <asp:CompareValidator runat="server" ID="cvEditCurrentCostPerUnit" Operator="DataTypeCheck"
                                                Type="double" Text="<" ErrorMessage="Current Actual Overhead Cost per Unit must be a number."
                                                ValidationGroup="vgEditOverheadCurrent" ControlToValidate="txtEditCurrentCostPerUnit"
                                                SetFocusOnError="true" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewCurrentCostPerUnit" runat="server" Text='<%# Bind("CurrentCostPerUnit") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtInsertCurrentCostPerUnit" runat="server" MaxLength="10" Width="75px" />
                                            <asp:CompareValidator runat="server" ID="cvInsertCurrentCostPerUnit" Operator="DataTypeCheck"
                                                Type="double" Text="<" ErrorMessage="Current Actual Overhead Cost per Unit must be a number."
                                                ValidationGroup="vgInsertOverheadCurrent" ControlToValidate="txtInsertCurrentCostPerUnit"
                                                SetFocusOnError="true" />
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Actual<br> Volume" SortExpression="CurrentVolume"
                                        ControlStyle-Width="75px">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditCurrentVolume" runat="server" Text='<%# Bind("CurrentVolume") %>'
                                                MaxLength="10" Width="75px" />
                                            <asp:CompareValidator runat="server" ID="cvEditCurrentVolume" Operator="DataTypeCheck"
                                                Type="integer" Text="<" ErrorMessage="Current Actual Overhead Volume must be an integer."
                                                ValidationGroup="vgEditOverheadCurrent" ControlToValidate="txtEditCurrentVolume"
                                                SetFocusOnError="true" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewCurrentVolume" runat="server" Text='<%# Bind("CurrentVolume") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtInsertCurrentVolume" runat="server" MaxLength="10" Width="75px" />
                                            <asp:CompareValidator runat="server" ID="cvInsertCurrentVolume" Operator="DataTypeCheck"
                                                Type="integer" Text="<" ErrorMessage="Current Actual Overhead Volume must be an integer."
                                                ValidationGroup="vgInsertOverheadCurrent" ControlToValidate="txtInsertCurrentVolume"
                                                SetFocusOnError="true" />
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Actual Cost by Volume" DataField="CurrentCostSubTotal"
                                        SortExpression="CurrentCostSubTotal" ReadOnly="True">
                                        <ItemStyle HorizontalAlign="Right" Width="75px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Budget Cost Per Unit" SortExpression="CurrentCostPerUnitBudget">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditCurrentCostPerUnitBudget" runat="server" Text='<%# Bind("CurrentCostPerUnitBudget") %>'
                                                MaxLength="10" Width="75px" />
                                            <asp:CompareValidator runat="server" ID="cvEditCurrentCostPerUnitBudget" Operator="DataTypeCheck"
                                                Type="double" Text="<" ErrorMessage="Current Budget Overhead Cost per Unit must be a number."
                                                ValidationGroup="vgEditOverheadCurrent" ControlToValidate="txtEditCurrentCostPerUnitBudget"
                                                SetFocusOnError="true" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewCurrentCostPerUnitBudget" runat="server" Text='<%# Bind("CurrentCostPerUnitBudget") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtInsertCurrentCostPerUnitBudget" runat="server" MaxLength="10"
                                                Width="75px" />
                                            <asp:CompareValidator runat="server" ID="cvInsertCurrentCostPerUnitBudget" Operator="DataTypeCheck"
                                                Type="double" Text="<" ErrorMessage="Current Budget Overhead Cost per Unit must be a number."
                                                ValidationGroup="vgInsertOverheadCurrent" ControlToValidate="txtInsertCurrentCostPerUnitBudget"
                                                SetFocusOnError="true" />
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Budget Volume" SortExpression="CurrentVolumeBudget">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditCurrentVolumeBudget" runat="server" Text='<%# Bind("CurrentVolumeBudget") %>'
                                                MaxLength="10" Width="75px" />
                                            <asp:CompareValidator runat="server" ID="cvEditCurrentVolumeBudget" Operator="DataTypeCheck"
                                                Type="integer" Text="<" ErrorMessage="Current Budget Overhead Volume must be an integer."
                                                ValidationGroup="vgEditOverheadCurrent" ControlToValidate="txtEditCurrentVolumeBudget"
                                                SetFocusOnError="true" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewCurrentVolumeBudget" runat="server" Text='<%# Bind("CurrentVolumeBudget") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtInsertCurrentVolumeBudget" runat="server" MaxLength="10" Width="75px" />
                                            <asp:CompareValidator runat="server" ID="cvInsertCurrentVolumeBudget" Operator="DataTypeCheck"
                                                Type="integer" Text="<" ErrorMessage="Current Budget Overhead Volume must be an integer."
                                                ValidationGroup="vgInsertOverheadCurrent" ControlToValidate="txtInsertCurrentVolumeBudget"
                                                SetFocusOnError="true" />
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Budget Cost by Volume" DataField="CurrentCostSubTotalBudget"
                                        SortExpression="CurrentCostSubTotalBudget" ReadOnly="True">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:TemplateField ShowHeader="False">
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="ibtnOverheadCurrentUpdate" runat="server" CommandName="Update"
                                                ImageUrl="~/images/save.jpg" AlternateText="Update" CausesValidation="true" ValidationGroup="vgEditOverheadCurrent" />
                                            <asp:ImageButton ID="iBtnOverheadCurrentCancelEdit" runat="server" CausesValidation="False"
                                                CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibtnOverheadCurrentEdit" runat="server" CommandName="Edit" ImageUrl="~/images/edit.jpg"
                                                AlternateText="Edit" />&nbsp;&nbsp;&nbsp;
                                            <asp:ImageButton ID="ibtnOverheadCurrentDelete" runat="server" CommandName="Delete"
                                                ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgInsertOverheadCurrent"
                                                runat="server" ID="iBtnOverheadCurrentSave" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                                            <asp:ImageButton ID="iBtnOverheadCurrentUndo" runat="server" CommandName="Undo" CausesValidation="false"
                                                ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:ObjectDataSource ID="odsOverheadCurrent" runat="server" OldValuesParameterFormatString="original_{0}"
                                SelectMethod="GetCostReductionOverhead" TypeName="CROverheadBLL" DeleteMethod="DeleteCostReductionOverhead"
                                InsertMethod="InsertCostReductionOverhead" UpdateMethod="UpdateCostReductionOverheadCurrent">
                                <SelectParameters>
                                    <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="Int32" />
                                </SelectParameters>
                                <DeleteParameters>
                                    <asp:Parameter Name="RowID" Type="Int32" />
                                    <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="Int32" />
                                    <asp:Parameter Name="original_RowID" Type="Int32" />
                                </DeleteParameters>
                                <UpdateParameters>
                                    <asp:Parameter Name="RowID" Type="Int32" />
                                    <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="Int32" />
                                    <asp:Parameter Name="ExpensedName" Type="String" />
                                    <asp:Parameter Name="CurrentCostPerUnit" Type="Double" />
                                    <asp:Parameter Name="CurrentCostPerUnitBudget" Type="Double" />
                                    <asp:Parameter Name="CurrentVolume" Type="Int32" />
                                    <asp:Parameter Name="CurrentVolumeBudget" Type="Int32" />
                                    <asp:Parameter Name="original_RowID" Type="Int32" />
                                </UpdateParameters>
                                <InsertParameters>
                                    <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="Int32" />
                                    <asp:Parameter Name="ExpensedName" Type="String" />
                                    <asp:Parameter Name="CurrentCostPerUnit" Type="Double" />
                                    <asp:Parameter Name="CurrentCostPerUnitBudget" Type="Double" />
                                    <asp:Parameter Name="CurrentVolume" Type="Int32" />
                                    <asp:Parameter Name="CurrentVolumeBudget" Type="Int32" />
                                    <asp:Parameter Name="ProposedCostPerUnit" Type="Double" />
                                    <asp:Parameter Name="ProposedVolume" Type="Int32" />
                                </InsertParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_textbold">
                            Total Actual Current Cost:
                        </td>
                        <td class="p_textbold">
                            $&nbsp;<asp:Label runat="server" ID="lblOverheadCurrentTotalCost" />
                        </td>
                        <td class="p_textbold">
                            Total Budget Current Cost:
                        </td>
                        <td class="p_textbold">
                            $&nbsp;<asp:Label runat="server" ID="lblOverheadCurrentTotalCostBudget" Text="0.00" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="5">
                            <span style="font-size: large; text-decoration: underline; font-style: italic">Proposed
                                Method:</span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:GridView ID="gvOverheadProposed" runat="server" AutoGenerateColumns="False"
                                DataKeyNames="RowID" DataSourceID="odsOverheadProposed" EmptyDataText="No overhead found"
                                AllowSorting="True" AllowPaging="True" PageSize="15" ShowFooter="True">
                                <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#CCCCCC" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField DataField="RowID" HeaderText="RowID" ReadOnly="True" SortExpression="RowID" />
                                    <asp:BoundField DataField="ProjectNo" HeaderText="ProjectNo" SortExpression="ProjectNo" />
                                    <asp:TemplateField HeaderText="Expensed Item Name" SortExpression="ExpensedName"
                                        ControlStyle-Width="100px">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditProposedExpensedName" runat="server" Text='<%# Bind("ExpensedName") %>'
                                                MaxLength="50" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewProposedExpensedName" runat="server" Text='<%# Bind("ExpensedName") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtInsertProposedExpensedName" runat="server" MaxLength="50" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cost Per Unit" SortExpression="ProposedCostPerUnit"
                                        ControlStyle-Width="75px">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditProposedCostPerUnit" runat="server" Text='<%# Bind("ProposedCostPerUnit") %>'
                                                MaxLength="10" Width="75px" />
                                            <asp:CompareValidator runat="server" ID="cvEditProposedCostPerUnit" Operator="DataTypeCheck"
                                                Type="double" Text="<" ErrorMessage="Proposed Overhead Cost per Unit must be a number."
                                                ValidationGroup="vgEditOverheadProposed" ControlToValidate="txtEditProposedCostPerUnit"
                                                SetFocusOnError="true" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewProposedCostPerUnit" runat="server" Text='<%# Bind("ProposedCostPerUnit") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtInsertProposedCostPerUnit" runat="server" MaxLength="10" Width="75px" />
                                            <asp:CompareValidator runat="server" ID="cvInsertProposedCostPerUnit" Operator="DataTypeCheck"
                                                Type="double" Text="<" ErrorMessage="Proposed Overhead Cost per Unit must be a number."
                                                ValidationGroup="vgInsertOverheadProposed" ControlToValidate="txtInsertProposedCostPerUnit"
                                                SetFocusOnError="true" />
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Volume" SortExpression="ProposedVolume" ControlStyle-Width="75px">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditProposedVolume" runat="server" Text='<%# Bind("ProposedVolume") %>'
                                                MaxLength="10" Width="75px" />
                                            <asp:CompareValidator runat="server" ID="cvEditProposedVolume" Operator="DataTypeCheck"
                                                Type="integer" Text="<" ErrorMessage="Proposed Overhead Volume must be an integer."
                                                ValidationGroup="vgEditOverheadProposed" ControlToValidate="txtEditProposedVolume"
                                                SetFocusOnError="true" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblViewProposedVolume" runat="server" Text='<%# Bind("ProposedVolume") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtInsertProposedVolume" runat="server" MaxLength="10" Width="75px" />
                                            <asp:CompareValidator runat="server" ID="cvInsertProposedVolume" Operator="DataTypeCheck"
                                                Type="integer" Text="<" ErrorMessage="Proposed Overhead Volume must be an integer."
                                                ValidationGroup="vgInsertOverheadProposed" ControlToValidate="txtInsertProposedVolume"
                                                SetFocusOnError="true" />
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Cost by Volume" DataField="ProposedCostSubTotal" SortExpression="ProposedCostSubTotal"
                                        ReadOnly="True">
                                        <ItemStyle HorizontalAlign="Right" Width="75px" />
                                    </asp:BoundField>
                                    <asp:TemplateField ShowHeader="False">
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="ibtnOverheadProposedUpdate" runat="server" CommandName="Update"
                                                ImageUrl="~/images/save.jpg" AlternateText="Update" CausesValidation="true" ValidationGroup="vgEditOverheadProposed" />
                                            <asp:ImageButton ID="iBtnOverheadProposedCancelEdit" runat="server" CausesValidation="False"
                                                CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibtnOverheadProposedEdit" runat="server" CommandName="Edit"
                                                ImageUrl="~/images/edit.jpg" AlternateText="Edit" />&nbsp;&nbsp;&nbsp;
                                            <asp:ImageButton ID="ibtnOverheadProposedDelete" runat="server" CommandName="Delete"
                                                ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgInsertOverheadProposed"
                                                runat="server" ID="iBtnOverheadProposedSave" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                                            <asp:ImageButton ID="iBtnOverheadProposedUndo" runat="server" CommandName="Undo"
                                                CausesValidation="false" ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:ObjectDataSource ID="odsOverheadProposed" runat="server" OldValuesParameterFormatString="original_{0}"
                                SelectMethod="GetCostReductionOverhead" TypeName="CROverheadBLL" DeleteMethod="DeleteCostReductionOverhead"
                                InsertMethod="InsertCostReductionOverhead" UpdateMethod="UpdateCostReductionOverheadProposed">
                                <SelectParameters>
                                    <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="Int32" />
                                </SelectParameters>
                                <DeleteParameters>
                                    <asp:Parameter Name="RowID" Type="Int32" />
                                    <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="Int32" />
                                    <asp:Parameter Name="original_RowID" Type="Int32" />
                                </DeleteParameters>
                                <UpdateParameters>
                                    <asp:Parameter Name="RowID" Type="Int32" />
                                    <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="Int32" />
                                    <asp:Parameter Name="ExpensedName" Type="String" />
                                    <asp:Parameter Name="ProposedCostPerUnit" Type="Double" />
                                    <asp:Parameter Name="ProposedVolume" Type="Int32" />
                                    <asp:Parameter Name="original_RowID" Type="Int32" />
                                </UpdateParameters>
                                <InsertParameters>
                                    <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="Int32" />
                                    <asp:Parameter Name="ExpensedName" Type="String" />
                                    <asp:Parameter Name="CurrentCostPerUnit" Type="Double" />
                                    <asp:Parameter Name="CurrentVolume" Type="Int32" />
                                    <asp:Parameter Name="ProposedCostPerUnit" Type="Double" />
                                    <asp:Parameter Name="ProposedVolume" Type="Int32" />
                                </InsertParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_textbold">
                            Total Actual Proposed Cost:
                        </td>
                        <td class="p_textbold">
                            $&nbsp;<asp:Label runat="server" ID="lblOverheadProposedTotalCost" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <span style="text-decoration: underline; font-weight: bold">Savings Analysis:</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text">
                            Actual Current Method:
                        </td>
                        <td class="p_text">
                            $&nbsp;<asp:Label runat="server" ID="lblOverheadCurrentMethod" />
                        </td>
                        <td class="p_text">
                            Budget Current Method:
                        </td>
                        <td class="p_text">
                            $&nbsp;<asp:Label runat="server" ID="lblOverheadCurrentMethodBudget" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text">
                            Less Proposed Method:
                        </td>
                        <td class="p_text">
                            $&nbsp;<asp:Label runat="server" ID="lblOverheadProposedMethod" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_textbold">
                            Actual Annual Overhead Savings:
                        </td>
                        <td class="p_textbold">
                            $&nbsp;<asp:Label runat="server" ID="lblOverheadSavings" />
                        </td>
                        <td class="p_textbold">
                            Budget Annual Overhead Savings:
                        </td>
                        <td class="p_textbold">
                            $&nbsp;<asp:Label runat="server" ID="lblOverheadSavingsBudget" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <span style="text-decoration: underline; font-weight: bold">Capital and Expenses:</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            New Capital:&nbsp;$
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtOverheadCECapital" MaxLength="10" Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvOverheadCECapital" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="Capital Expense Capital must be a number."
                                ValidationGroup="vgCalculate" ControlToValidate="txtOverheadCECapital" SetFocusOnError="true" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            Materials:&nbsp;$
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtOverheadCEMaterial" MaxLength="10" Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvOverheadCEMaterial" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="Capital Expense Material must be a number."
                                ValidationGroup="vgCalculate" ControlToValidate="txtOverheadCEMaterial" SetFocusOnError="true" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            Outside Support:&nbsp;$
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtOverheadCEOutsideSupport" MaxLength="10" Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvOverheadCEOutsideSupport" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="Capital Expense Outside Support must be a number."
                                ValidationGroup="vgCalculate" ControlToValidate="txtOverheadCEOutsideSupport"
                                SetFocusOnError="true" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            Misc.:&nbsp;$
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtOverheadCEMisc" MaxLength="10" Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvOverheadCEMisc" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="Capital Expense miscellaneous must be a number."
                                ValidationGroup="vgCalculate" ControlToValidate="txtOverheadCEMisc" SetFocusOnError="true" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            In-House Support:&nbsp;$
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtOverheadCEInHouseSupport" MaxLength="10" Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvOverheadCEInHouseSupport" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="Capital Expense miscellaneous must be a number."
                                ValidationGroup="vgCalculate" ControlToValidate="txtOverheadCEInHouseSupport"
                                SetFocusOnError="true" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_text" style="color: Blue">
                            Existing Fixed Asset (Net Book) Write Off:&nbsp;$
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtOverheadCEWriteOff" MaxLength="10" Width="75px" />
                            <asp:CompareValidator runat="server" ID="cvOverheadCEWriteOff" Operator="DataTypeCheck"
                                Type="double" Text="<" ErrorMessage="Capital Expense Write Off must be a number."
                                ValidationGroup="vgCalculate" ControlToValidate="txtOverheadCEWriteOff" SetFocusOnError="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="p_textbold" style="color: Blue">
                            Total Capital and Expense:&nbsp;$
                        </td>
                        <td class="c_textbold">
                            <asp:Label runat="server" ID="lblOverheadCETotal" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <span style="text-decoration: underline; font-weight: bold">Payback Analysis:</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="5" align="center">
                            Capital Expenses / Annual Savings = &nbsp
                            <asp:Label runat="server" ID="lblOverheadSavingsANDCE" />
                            &nbsp
                            <asp:Label runat="server" ID="lblOverheadPayback" CssClass="c_textbold" />
                            &nbsp years
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vsSupportingDocuments" runat="server">
                <asp:Panel ID="SDPanel" runat="server" CssClass="collapsePanelHeader" Width="680px">
                    <asp:Image ID="imgSD" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                        Height="12px" />&nbsp;
                    <asp:Label ID="lblSD" runat="server" Text="Label" CssClass="c_textbold">SUPPORTING DOCUMENT(S):</asp:Label>
                </asp:Panel>
                <asp:Panel ID="SDContentPanel" runat="server" CssClass="collapsePanel" Width="800px">
                    <br />
                    <asp:Label ID="lblSupDoc" runat="server" CssClass="p_smalltextbold" Style="width: 800px;
                        color: #990000" Text="This section is available as an option to include additional information. *.PDF, *.DOC, *.DOCX, *XLS, and *.XLSX files are allowed for upload up to 4MB each." /><br />
                    <br />
                    <asp:Label ID="lblSupDoc2" runat="server" CssClass="p_smalltextbold" Style="width: 800px;
                        color: #990000" Text="NOTE: Please be sure to upload the latest copy of any document. Any changes you make will not be saved to the upload files. Please be sure to make a copy of the file locally and upload a new version. You have the option to delete or keep previous version of the file for reference. Please use the 'File Description' area to comment on the changes you make." /><br />
                    <br />
                    <table>
                        <tr>
                            <td class="p_text">
                                Upload By:
                            </td>
                            <td class="c_text">
                                <asp:DropDownList ID="ddTeamMember" runat="server" />
                                <asp:RequiredFieldValidator ID="rfvTeamMember" runat="server" ControlToValidate="ddTeamMember"
                                    ErrorMessage="Team Member is a required field." Font-Bold="False" ValidationGroup="vsSupportingDocuments"><</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text" valign="top">
                                File Description:
                            </td>
                            <td class="c_text">
                                <asp:TextBox ID="txtFileDesc" runat="server" MaxLength="200" Rows="3" TextMode="MultiLine"
                                    Width="600px" />
                                <asp:RequiredFieldValidator ID="rfvFileDesc" runat="server" ControlToValidate="txtFileDesc"
                                    ErrorMessage="File Description is a required field." Font-Bold="False" ValidationGroup="vsSupportingDocuments"><</asp:RequiredFieldValidator><br />
                                <asp:Label ID="lblFileDesc" runat="server" Font-Bold="True" ForeColor="Red" />
                            </td>
                        </tr>
                        <tr>
                            <td class="p_text" valign="top">
                                Supporting Document:
                            </td>
                            <td class="c_text">
                                <asp:FileUpload ID="uploadFile" runat="server" Height="22px" Width="600px" />
                                <asp:RequiredFieldValidator ID="rfvUpload" runat="server" ControlToValidate="uploadFile"
                                    ErrorMessage="Supporting Document is required." Font-Bold="False" ValidationGroup="vsSupportingDocuments"><</asp:RequiredFieldValidator><br />
                                <asp:RegularExpressionValidator ID="revUploadFile" runat="server" ErrorMessage="Only *.PDF, *.DOC, *DOCX, *.XLS, *.XLSX files are allowed!"
                                    ValidationExpression="(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.pdf|.xls|.xlsx|.doc|.docx|.PDF|.XLS|.XLSX|.DOC|.DOCX)$"
                                    ControlToValidate="uploadFile" ValidationGroup="vsSupportingDocuments" Font-Bold="True"
                                    Font-Size="Small" /><br />
                                <asp:Label ID="lblMessageView4" runat="server" Font-Bold="True" ForeColor="Red" Height="16px"
                                    Text="Label" Visible="False" Width="368px" Font-Size="Small" />
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 27px">
                            </td>
                            <td style="height: 27px">
                                <asp:Button ID="btnUpload" runat="server" Text="Upload" CausesValidation="true" ValidationGroup="vsSupportingDocuments" />
                                <asp:Button ID="btnReset3" runat="server" CausesValidation="False" Text="Reset" />
                            </td>
                        </tr>
                    </table>
                    <asp:ValidationSummary ID="vsSupDoc" runat="server" ValidationGroup="vsSupportingDocuments"
                        ShowMessageBox="true" ShowSummary="true" />
                </asp:Panel>
                <ajax:CollapsiblePanelExtender ID="SDExtender" runat="server" TargetControlID="SDContentPanel"
                    ExpandControlID="SDPanel" CollapseControlID="SDPanel" Collapsed="FALSE" TextLabelID="lblSD"
                    ExpandedText="SUPPORTING DOCUMENT(S):" CollapsedText="SUPPORTING DOCUMENT(S):"
                    ImageControlID="imgSD" CollapsedImage="~/images/expand_blue.jpg" ExpandedImage="~/images/collapse_blue.jpg"
                    SuppressPostBack="true">
                </ajax:CollapsiblePanelExtender>
                <br />
                <asp:GridView ID="gvSupportingDocument" runat="server" AutoGenerateColumns="False"
                    DataKeyNames="ProjectNo,DocID" DataSourceID="odsSupportingDocument" Width="900px"
                    AllowSorting="True">
                    <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" Wrap="False" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#E2DED6" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <EmptyDataRowStyle Wrap="False" />
                    <Columns>
                        <asp:BoundField DataField="Description" HeaderText="File Description" SortExpression="Description">
                            <HeaderStyle HorizontalAlign="Left" Width="400px" />
                            <ItemStyle Width="400px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="comboUploadBy" HeaderText="Uploaded By" SortExpression="comboUploadBy">
                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                            <ItemStyle Width="150px" />
                        </asp:BoundField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="hlnkPreview" ImageUrl='<%# DisplayImage(DataBinder.Eval(Container, "DataItem.EncodeType").tostring)  %>'
                                    NavigateUrl='<%# "CostReductionDocument.aspx?pProjNo=" & DataBinder.Eval (Container.DataItem,"ProjectNo").tostring & "&pDocID=" & DataBinder.Eval (Container.DataItem,"DocID").tostring %>'
                                    Target="_blank" Visible='<%# Bind("BinaryFound") %>' ToolTip="Preview Test Report" />
                            </ItemTemplate>
                            <HeaderStyle Width="30px" />
                            <ItemStyle HorizontalAlign="Center" Width="30px" />
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                    ImageUrl="~/images/delete.jpg" AlternateText="Delete" />
                            </ItemTemplate>
                            <HeaderStyle Width="30px" />
                            <ItemStyle HorizontalAlign="Right" Width="30px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="odsSupportingDocument" runat="server" DeleteMethod="DeleteCostReductionDocuments"
                    OldValuesParameterFormatString="original_{0}" SelectMethod="GetCostReductionDocuments"
                    TypeName="CRDocumentsBLL">
                    <DeleteParameters>
                        <asp:Parameter Name="ProjectNo" Type="String" />
                        <asp:Parameter Name="Original_DocID" Type="Int32" />
                        <asp:Parameter Name="Original_ProjectNo" Type="String" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:QueryStringParameter Name="ProjectNo" QueryStringField="pProjNo" Type="Int32" />
                        <asp:Parameter Name="DocID" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </asp:View>
        </asp:MultiView>
        <br />
        <table width="68%">
            <tr>
                <td colspan="2" align="center">
                    <asp:Button runat="server" ID="btnSaveBottom" Text="Save" CausesValidation="true"
                        ValidationGroup="vgSave" />
                    <asp:Button runat="server" ID="btnCalculateBottom" Text="Calculate" CausesValidation="true"
                        ValidationGroup="vgCalculate" />
                    <asp:Button runat="server" ID="btnResetBottom" Text="Reset" CausesValidation="false" />
                    <asp:Button runat="server" ID="btnPreviewBottom" Text="Preview" CausesValidation="false" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Label runat="server" ID="lblMessageBottom" SkinID="MessageLabelSkin" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
