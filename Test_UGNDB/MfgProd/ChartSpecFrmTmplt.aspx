<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="ChartSpecFrmTmplt.aspx.vb" Inherits="MfgProd_ChartSpecFrmTmplt" Title="UGNDB - Chart Spec Form Template"
    MaintainScrollPositionOnPostback="True" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch" Width="1200px">
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin"></asp:Label>
        <hr />
        <br />
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <br />
        <table>
            <tr>
                <td class="p_text">
                    Formula:
                </td>
                <td>
                    <asp:TextBox ID="txtFormula" runat="server" MaxLength="30" Width="200px" />
                    <ajax:FilteredTextBoxExtender ID="ftbFormula" runat="server" TargetControlID="txtFormula"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,%/- " />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="2">
                    <asp:Button ID="btnSearch" runat="server" Text="Submit" CausesValidation="False" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="False" />
                    <asp:Button ID="btnExport" runat="server" Text="Export to Excel" CausesValidation="False" />
                </td>
            </tr>
        </table>
        <hr />
        <asp:Label ID="lblRaiseError" runat="server" Text="" Visible="false" SkinID="MessageLabelSkin" /><br />
        <br />
        <asp:Panel ID="PSPanel" runat="server" CssClass="collapsePanelHeader" Width="1100px">
            <asp:Image ID="imgPS" runat="server" AlternateText="expand" ImageUrl="~/images/expand_blue.jpg"
                Height="12px" />&nbsp;
            <asp:Label ID="lblPS" runat="server" Text="Label" CssClass="c_textbold">Part Specification Requirements below:</asp:Label>
        </asp:Panel>
        <asp:Panel ID="PSContentPanel" runat="server" CssClass="collapsePanel" Height="1100px">
            <asp:Label ID="PagingInformation" runat="server" Text="" /><asp:DropDownList ID="PageList"
                runat="server" CssClass="c_textxsmall" AutoPostBack="true" OnSelectedIndexChanged="PageList_SelectedIndexChanged" />
            <asp:GridView ID="gvChartSpec" runat="server" AutoGenerateColumns="False" DataSourceID="odsChartSpec"
                DataKeyNames="RowID" AllowPaging="True" AllowSorting="True" CssClass="c_smalltext"
                PageSize="50" Width="1000px" EmptyDataRowStyle-Font-Size="Medium" EmptyDataRowStyle-Font-Bold="true"
                EmptyDataRowStyle-ForeColor="Red" OnRowDataBound="ChartSpec_DataBound">
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
                    <asp:TemplateField ShowHeader="False" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                ImageUrl="~/images/delete.jpg" AlternateText="Delete" />
                        </ItemTemplate>
                        <HeaderStyle Width="30px" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False">
                        <EditItemTemplate>
                            &nbsp;&nbsp;<asp:ImageButton ID="ibtnUpdate" runat="server" AlternateText="Update"
                                CausesValidation="True" CommandName="Update" ImageUrl="~/images/save.jpg" Text="Update"
                                ValidationGroup="EditInfo" />
                            &nbsp;&nbsp;&nbsp;
                            <asp:ImageButton ID="ibtnCancel" runat="server" AlternateText="Cancel" CausesValidation="False"
                                CommandName="Cancel" ImageUrl="~/images/cancel.jpg" Text="Cancel" ValidationGroup="EditInfo" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            &nbsp;&nbsp;<asp:ImageButton ID="ibtnEdit" runat="server" AlternateText="Edit" CausesValidation="False"
                                CommandName="Edit" ImageUrl="~/images/edit.jpg" Text="Edit" />
                            &nbsp;&nbsp;&nbsp;
                        </ItemTemplate>
                        <FooterTemplate>
                            &nbsp;&nbsp;<asp:ImageButton ID="ibtnInsert" runat="server" AlternateText="Insert"
                                CausesValidation="true" CommandName="Insert" ImageUrl="~/images/save.jpg" Text="Insert"
                                ValidationGroup="InsertInfo" />
                            &nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ibtnUndo" runat="server" AlternateText="Undo"
                                CommandName="Undo" ImageUrl="~/images/undo-gray.jpg" Text="Undo" />
                        </FooterTemplate>
                        <HeaderStyle Width="70px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="RowID" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true"
                        HeaderText="RowID" ItemStyle-HorizontalAlign="left" SortExpression="RowID" Visible="False">
                        <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                        <ItemStyle Wrap="False" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Formula ID" SortExpression="FormulaID">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddEFormulaID" runat="server" DataSource='<%# CostingModule.GetFormula(0) %>'
                                DataValueField="FormulaID" DataTextField="ddFormulaName" AppendDataBoundItems="True"
                                CssClass="c_textxsmall" SelectedValue='<%# Bind("FormulaID") %>'>
                                <asp:ListItem Text="N/A" Value="0" Selected="True" />
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("FormulaID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="FormulaName" HeaderText="Formula Name" SortExpression="FormulaName"
                        ReadOnly="true" ItemStyle-CssClass="c_textxsmall">
                        <ItemStyle CssClass="c_textxsmall" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FormulaEffDt" HeaderText="Formula Eff Dt" SortExpression="FormulaEffDt"
                        ItemStyle-CssClass="c_textxsmall" ItemStyle-Wrap="false" Visible="False">
                        <ItemStyle Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FormulaExpDt" HeaderText="Formula Exp Dt" ItemStyle-CssClass="c_textxsmall"
                        ItemStyle-Wrap="false" SortExpression="FormulaExpDt" Visible="False">
                        <ItemStyle Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FormulaRevNo" HeaderText="Formula Rev No" ItemStyle-CssClass="c_textxsmall"
                        ItemStyle-Wrap="false" SortExpression="FormulaRevNo" Visible="False">
                        <ItemStyle Wrap="False" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Label Name" SortExpression="LabelName">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtELabelName" runat="server" CssClass="c_textxsmall" MaxLength="30"
                                Text='<%# Bind("LabelName") %>' />
                            <ajax:FilteredTextBoxExtender ID="ftbELabelName" runat="server" TargetControlID="txtELabelName"
                                FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("LabelName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Column Name" SortExpression="ColumnName">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEColumnName" runat="server" CssClass="c_textxsmall" MaxLength="30"
                                Text='<%# Bind("ColumnName") %>' />
                            <ajax:FilteredTextBoxExtender ID="ftbEColumnName" runat="server" TargetControlID="txtEColumnName"
                                FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("ColumnName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fld Obj Name" SortExpression="FldObjName">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEFldObjName" runat="server" CssClass="c_textxsmall" MaxLength="30"
                                Text='<%# Bind("FldObjName") %>' />
                            <ajax:FilteredTextBoxExtender ID="ftbEFldObjName" runat="server" TargetControlID="txtEFldObjName"
                                FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("FldObjName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fld Type" SortExpression="FldType">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddEFldType" CssClass="c_textxsmall" runat="server" SelectedValue='<%# Bind("FldType") %>'>
                                <asp:ListItem>DropDownList</asp:ListItem>
                                <asp:ListItem>Text</asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label5" runat="server" Text='<%# Bind("FldType") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Dflt Val" SortExpression="DfltVal">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEDfltVal" runat="server" CssClass="c_textxsmall" MaxLength="10" width="80px" Text='<%# Bind("DfltVal") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label6" runat="server" Text='<%# Bind("DfltVal") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Req Fld" SortExpression="ReqFld">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddEReqFld" CssClass="c_textxsmall" runat="server" SelectedValue='<%# Bind("ReqFld") %>'>
                                <asp:ListItem Value="False">No</asp:ListItem>
                                <asp:ListItem Value="True">Yes</asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label7" runat="server" Text='<%# Bind("ReqFldDisplay") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Read Only" SortExpression="ReadOnly">
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddEReadOnly" CssClass="c_textxsmall" runat="server" SelectedValue='<%# Bind("ReadOnly") %>'>
                                <asp:ListItem Value="False">No</asp:ListItem>
                                <asp:ListItem Value="True">Yes</asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label8" runat="server" Text='<%# Bind("ReadOnlyDisplay") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Notes" SortExpression="Notes">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtENotes" runat="server" CssClass="c_textxsmall" Text='<%# Bind("Notes") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label9" runat="server" Text='<%# Bind("Notes") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsChartSpec" runat="server" OldValuesParameterFormatString="original_{0}"
                SelectMethod="GetChartSpecFrmTmplt" TypeName="MfgProdBLL" DeleteMethod="DeleteChartSpecFrmTmplt"
                InsertMethod="InsertChartSpecFrmTmplt" UpdateMethod="UpdateChartSpecFrmTmplt">
                <DeleteParameters>
                    <asp:Parameter Name="original_RowID" Type="Int32" />
                </DeleteParameters>
                <UpdateParameters>
                    <asp:Parameter Name="FormulaID" Type="Int32" />
                    <asp:Parameter Name="FormulaName" Type="String" />
                    <asp:Parameter Name="LabelName" Type="String" />
                    <asp:Parameter Name="ColumnName" Type="Int32" />
                    <asp:Parameter Name="FldObjName" Type="String" />
                    <asp:Parameter Name="FldType" Type="String" />
                    <asp:Parameter Name="DfltVal" Type="String" />
                    <asp:Parameter Name="ReqFld" Type="Boolean" />
                    <asp:Parameter Name="ReadOnly" Type="Boolean" />
                    <asp:Parameter Name="Notes" Type="String" />
                    <asp:Parameter Name="original_RowID" Type="Int32" />
                </UpdateParameters>
                <SelectParameters>
                    <asp:Parameter Name="RowID" Type="Int32" />
                    <asp:Parameter Name="FormulaID" Type="Int32" />
                    <asp:QueryStringParameter Name="FormulaName" QueryStringField="sFormula" Type="String" />
                    <asp:Parameter Name="Obsolete" Type="Boolean" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="FormulaID" Type="Int32" />
                    <asp:Parameter Name="FormulaName" Type="String" />
                    <asp:Parameter Name="LabelName" Type="String" />
                    <asp:Parameter Name="ColumnName" Type="Int32" />
                    <asp:Parameter Name="FldObjName" Type="String" />
                    <asp:Parameter Name="FldType" Type="String" />
                    <asp:Parameter Name="DfltVal" Type="String" />
                    <asp:Parameter Name="ReqFld" Type="Boolean" />
                    <asp:Parameter Name="ReadOnly" Type="Boolean" />
                    <asp:Parameter Name="Notes" Type="String" />
                </InsertParameters>
            </asp:ObjectDataSource>
            <br />
            <br />
            <br />
        </asp:Panel>
        <ajax:CollapsiblePanelExtender ID="PSExtender" runat="server" TargetControlID="PSContentPanel"
            ExpandControlID="PSPanel" CollapseControlID="PSPanel" Collapsed="FALSE" TextLabelID="lblPS"
            ExpandedText="Part Specification Requirements below:" CollapsedText="Part Specification Requirements below:"
            ImageControlID="imgPS" CollapsedImage="~/images/expand_blue.jpg" ExpandedImage="~/images/collapse_blue.jpg"
            SuppressPostBack="true" ScrollContents="true">
        </ajax:CollapsiblePanelExtender>
        <br />
        <asp:ValidationSummary ID="vsEditInfo" runat="server" ShowMessageBox="True" Width="599px"
            ValidationGroup="EditInfo" Height="35px" />
        <asp:ValidationSummary ID="vsInsertInfo" runat="server" ShowMessageBox="True" Width="599px"
            ValidationGroup="InsertInfo" />
        <asp:ValidationSummary ID="vsEmptyInfo" runat="server" ShowMessageBox="True" Width="599px"
            ValidationGroup="EmptyInfo" />
    </asp:Panel>
</asp:Content>
