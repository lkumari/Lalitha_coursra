<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="DrawingToleranceMaint.aspx.vb" Inherits="DrawingToleranceMaint" Title="DMS Tolerance Maintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <asp:Label ID="lblMessage" runat="server" SkinID="MessageLabelSkin" />
        <br />
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <br />
        <table>
            <tr>
                <td class="p_text">
                    Tolerance Name:
                </td>
                <td>
                    <asp:TextBox ID="txtSearchToleranceName" runat="server" Width="250px" MaxLength="30">
                    </asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CausesValidation="false"></asp:Button>
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="false"></asp:Button>
                </td>
            </tr>
        </table>
        <hr />
        <table width="100%">
            <tr>
                <td>
                    <asp:GridView ID="gvTolerance" runat="server" AutoGenerateColumns="False" Width="100%"
                        EmptyDataText="No records found." AllowSorting="True" DataKeyNames="toleranceID"
                        DataSourceID="odsTolerance" ShowFooter="True">
                        <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#CCCCCC" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <EmptyDataTemplate>
                            No Records Found in Database.
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="Tolerance Name" SortExpression="ToleranceName" HeaderStyle-HorizontalAlign=Left>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditToleranceName" runat="server" Text='<%# Bind("toleranceName") %>'
                                        MaxLength="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvEditToleranceName" runat="server" ControlToValidate="txtEditToleranceName"
                                        SetFocusOnError="true" ErrorMessage="Tolerance Name is Required for Updating."
                                        Font-Bold="True" ValidationGroup="EditToleranceInfo" Text="<" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblViewToleranceName" runat="server" Text='<%# Bind("ddToleranceName") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtFooterToleranceName" runat="server" Text='<%# Bind("toleranceName") %>'
                                        MaxLength="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvFooterToleranceName" runat="server" ControlToValidate="txtFooterToleranceName"
                                        SetFocusOnError="true" ErrorMessage="Tolerance Name is Required for Inserting."
                                        Font-Bold="True" ValidationGroup="FooterToleranceInfo" Text="<" />
                                </FooterTemplate>
                                <FooterStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Density Val" SortExpression="densityValue">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditDensityValue" runat="server" Text='<%# Bind("densityValue") %>'
                                        MaxLength="10" Width="40px"></asp:TextBox>
                                    <asp:CompareValidator runat="server" ID="cvDensityValueDouble" Operator="DataTypeCheck"
                                        Type="Double" ControlToValidate="txtEditDensityValue" SetFocusOnError="true"
                                        ValidationGroup="EditToleranceInfo" ErrorMessage="Density Value must be a valid number (5, 5.5, etc)."
                                        Text="<" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblViewDensityValue" runat="server" Text='<%# Bind("densityValue") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtFooterDensityValue" runat="server" Text='<%# Bind("densityValue") %>'
                                        MaxLength="10" Width="40px"></asp:TextBox>
                                    <asp:CompareValidator runat="server" ID="cvdensityValueDouble" Operator="DataTypeCheck"
                                        Type="Double" Text="<" ControlToValidate="txtFooterDensityValue" SetFocusOnError="true"
                                        ErrorMessage="Density Value must be a valid number (5, 5.5, etc)." />
                                </FooterTemplate>
                                <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Density Tol" SortExpression="densityTolerance">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditDensityTolerance" runat="server" Text='<%# Bind("densityTolerance") %>'
                                        MaxLength="10" Width="40px"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblViewDensityTolerance" runat="server" Text='<%# Bind("densityTolerance") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtFooterDensityTolerance" runat="server" Text='<%# Bind("densityTolerance") %>'
                                        MaxLength="10" Width="40px"></asp:TextBox>
                                </FooterTemplate>
                                <FooterStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Density Units" SortExpression="DensityUnits">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditDensityUnits" runat="server" Text='<%# Bind("DensityUnits") %>'
                                        MaxLength="15" Width="40px"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblViewDensityUnits" runat="server" Text='<%# Bind("DensityUnits") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtFooterDensityUnits" runat="server" Text='<%# Bind("DensityUnits") %>'
                                        MaxLength="15" Width="40px"></asp:TextBox>
                                </FooterTemplate>
                                <FooterStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Thick Val" SortExpression="thicknessValue">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditThicknessValue" runat="server" Text='<%# Bind("thicknessValue") %>'
                                        MaxLength="10" Width="40px"></asp:TextBox>
                                    <asp:CompareValidator runat="server" ID="cvThicknessValueDouble" Operator="DataTypeCheck"
                                        Type="Double" Text="<" ControlToValidate="txtEditThicknessValue" SetFocusOnError="true"
                                        ValidationGroup="EditToleranceInfo" ErrorMessage="Thickness Value must be a valid number (5, 5.5, etc)." />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblViewThicknessValue" runat="server" Text='<%# Bind("thicknessValue") %>'></asp:Label><br />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <edititemtemplate>
                                        <asp:TextBox ID="txtFooterThicknessValue" runat="server" Text='<%# Bind("thicknessValue") %>'
                                            MaxLength="10" Width="40px"></asp:TextBox>
                                        <asp:CompareValidator runat="server" ID="cvThicknessValueDouble" Operator="DataTypeCheck"
                                            Type="Double" Text="<" ControlToValidate="txtFooterThicknessValue" SetFocusOnError="true"
                                            ValidationGroup="FooterToleranceInfo"
                                            ErrorMessage="Thickness Value must be a valid number (5, 5.5, etc)." />
                                    </edititemtemplate>
                                </FooterTemplate>
                                <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Thick Tol" SortExpression="thicknessTolerance">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditThicknessTolerance" runat="server" Text='<%# Bind("thicknessTolerance") %>'
                                        MaxLength="10" Width="40px"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblViewThicknessTolerance" runat="server" Text='<%# Bind("thicknessTolerance") %>'></asp:Label><br />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtFooterThicknessTolerance" runat="server" Text='<%# Bind("thicknessTolerance") %>'
                                        MaxLength="10" Width="40px"></asp:TextBox>
                                </FooterTemplate>
                                <FooterStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Thick Units" SortExpression="ThicknessUnits">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditThicknessUnits" runat="server" Text='<%# Bind("ThicknessUnits") %>'
                                        MaxLength="15" Width="40px"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblViewThicknessUnits" runat="server" Text='<%# Bind("ThicknessUnits") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtFooterThicknessUnits" runat="server" Text='<%# Bind("ThicknessUnits") %>'
                                        MaxLength="15" Width="40px"></asp:TextBox>
                                </FooterTemplate>
                                <FooterStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="WMD Val" SortExpression="WMDValue">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditWMDValue" runat="server" Text='<%# Bind("WMDValue") %>' MaxLength="5"
                                        Width="40px"></asp:TextBox>
                                    <asp:CompareValidator runat="server" ID="cvWValueDouble" Operator="DataTypeCheck"
                                        Type="Double" Text="<" ControlToValidate="txtEditWMDValue" SetFocusOnError="true"
                                        ValidationGroup="EditToleranceInfo" ErrorMessage="WMD Value must be a valid integer or fraction (5, 5.5, etc) value." />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblViewWMDValue" runat="server" Text='<%# Bind("WMDValue") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtFooterWMDValue" runat="server" Text='<%# Bind("WMDValue") %>'
                                        MaxLength="5" Width="40px"></asp:TextBox>
                                    <asp:CompareValidator runat="server" ID="cvWValueDouble" Operator="DataTypeCheck"
                                        Type="Double" Text="<" ControlToValidate="txtFooterWMDValue" SetFocusOnError="true"
                                        ValidationGroup="FooterToleranceInfo" ErrorMessage="WMD Value must be a valid number (5, 5.5, etc)." />
                                </FooterTemplate>
                                <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="WMD Tol" SortExpression="WMDTolerance">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditWMDTolerance" runat="server" Text='<%# Bind("WMDTolerance") %>'
                                        MaxLength="10" Width="40px"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblViewWMDTolerance" runat="server" Text='<%# Bind("WMDTolerance") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtFooterWMDTolerance" runat="server" Text='<%# Bind("WMDTolerance") %>'
                                        MaxLength="10" Width="40px"></asp:TextBox>
                                </FooterTemplate>
                                <FooterStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="WMD Units" SortExpression="wmdUnits">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddEditWMDUnits" DataTextField="wmdUnits" DataValueField="wmdUnits"
                                        runat="server" Width="52px" SelectedValue='<%# Bind("wmdUnits") %>'>
                                        <asp:ListItem Text="" Value=""></asp:ListItem>
                                        <asp:ListItem Text="m" Value="m"></asp:ListItem>
                                        <asp:ListItem Text="mm" Value="mm"></asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblViewWMDUnits" runat="server" Text='<%# Bind("wmdUnits") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddFooterWMDUnits" DataTextField="wmdUnits" DataValueField="wmdUnits"
                                        runat="server" Width="52px" SelectedValue='<%# Bind("wmdUnits") %>'>
                                        <asp:ListItem Text="" Value=""></asp:ListItem>
                                        <asp:ListItem Text="m" Value="m"></asp:ListItem>
                                        <asp:ListItem Text="mm" Value="mm"></asp:ListItem>
                                    </asp:DropDownList>
                                </FooterTemplate>
                                <FooterStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="AMD Val" SortExpression="AMDValue">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditAMDValue" runat="server" Text='<%# Bind("AMDValue") %>' MaxLength="10"
                                        Width="40px"></asp:TextBox>
                                    <asp:CompareValidator runat="server" ID="cvAValueDouble" Operator="DataTypeCheck"
                                        Type="Double" Text="<" ControlToValidate="txtEditAMDValue" SetFocusOnError="true"
                                        ValidationGroup="EditToleranceInfo" ErrorMessage="AMD Value must be a valid number (5, 5.5, etc)." />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblViewAMDValue" runat="server" Text='<%# Bind("AMDValue") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtFooterAMDValue" runat="server" Text='<%# Bind("AMDValue") %>'
                                        MaxLength="10" Width="40px"></asp:TextBox>
                                    <asp:CompareValidator runat="server" ID="cvAValueDouble" Operator="DataTypeCheck"
                                        Type="Double" Text="*" ControlToValidate="txtFooterAMDValue" ErrorMessage="* AMD Value must be a valid integer or fraction (5, 5.5, etc) value." />
                                </FooterTemplate>
                                <FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="AMD Tol" SortExpression="AMDTolerance">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtEditAMDTolerance" runat="server" Text='<%# Bind("AMDTolerance") %>'
                                        MaxLength="10" Width="40px"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblViewAMDTolerance" runat="server" Text='<%# Bind("AMDTolerance") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtFooterAMDTolerance" runat="server" Text='<%# Bind("AMDTolerance") %>'
                                        MaxLength="10" Width="40px"></asp:TextBox>
                                </FooterTemplate>
                                <FooterStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="AMD Units" SortExpression="amdUnits">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddEditAMDUnits" DataTextField="amdUnits" DataValueField="amdUnits"
                                        runat="server" SelectedValue='<%# Bind("amdUnits") %>' Width="52px">
                                        <asp:ListItem Text="" Value=""></asp:ListItem>
                                        <asp:ListItem Text="m" Value="m"></asp:ListItem>
                                        <asp:ListItem Text="mm" Value="mm"></asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblViewAMDUnits" runat="server" Text='<%# Bind("amdUnits") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddFooterAMDUnits" DataTextField="amdUnits" DataValueField="amdUnits"
                                        runat="server" SelectedValue='<%# Bind("amdUnits") %>' Width="52px">
                                        <asp:ListItem Text="" Value=""></asp:ListItem>
                                        <asp:ListItem Text="m" Value="m"></asp:ListItem>
                                        <asp:ListItem Text="mm" Value="mm"></asp:ListItem>
                                    </asp:DropDownList>
                                </FooterTemplate>
                                <FooterStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Obsolete" SortExpression="Obsolete">
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkObsoleteEdit" runat="server" Checked='<%# Bind("Obsolete") %>' />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkObsoletePreEdit" runat="server" Checked='<%# Bind("Obsolete") %>'
                                        Enabled="false" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <EditItemTemplate>
                                    <asp:ImageButton ID="btnToleranceUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                        ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="EditToleranceInfo" />
                                    <asp:ImageButton ID="btnToleranceCancel" runat="server" CausesValidation="False"
                                        CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnToleranceEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                        ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="FooterToleranceInfo"
                                        runat="server" ID="btnFooterTolerance" ImageUrl="~/images/save.jpg" AlternateText="Insert" />
                                    <asp:ImageButton ID="btnToleranceUndo" runat="server" CommandName="Undo" CausesValidation="false"
                                        ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:ValidationSummary ID="vsEmptyToleranceInfo" runat="server" ShowMessageBox="True"
                        Width="599px" ValidationGroup="EmptyToleranceInfo" />
                    <asp:ValidationSummary ID="vsEditToleranceInfo" runat="server" ShowMessageBox="True"
                        Width="599px" ValidationGroup="EditToleranceInfo" />
                    <asp:ValidationSummary ID="vsFooterToleranceInfo" runat="server" ShowMessageBox="True"
                        Width="599px" ValidationGroup="FooterToleranceInfo" />
                    <asp:ObjectDataSource ID="odsTolerance" runat="server" InsertMethod="InsertDrawingTolerance"
                        OldValuesParameterFormatString="original_{0}" SelectMethod="GetDrawingTolerance"
                        TypeName="DrawingToleranceBLL" UpdateMethod="UpdateDrawingTolerance">
                        <UpdateParameters>
                            <asp:Parameter Name="ToleranceID" Type="Int32" />
                            <asp:Parameter Name="ToleranceName" Type="String" />
                            <asp:Parameter Name="DensityValue" Type="Double" />
                            <asp:Parameter Name="DensityTolerance" Type="String" />
                            <asp:Parameter Name="DensityUnits" Type="String" />
                            <asp:Parameter Name="ThicknessValue" Type="Double" />
                            <asp:Parameter Name="ThicknessTolerance" Type="String" />
                            <asp:Parameter Name="ThicknessUnits" Type="String" />
                            <asp:Parameter Name="WMDValue" Type="Double" />
                            <asp:Parameter Name="WMDTolerance" Type="String" />
                            <asp:Parameter Name="WMDUnits" Type="String" />
                            <asp:Parameter Name="AMDValue" Type="Double" />
                            <asp:Parameter Name="AMDTolerance" Type="String" />
                            <asp:Parameter Name="AMDUnits" Type="String" />
                            <asp:Parameter Name="Obsolete" Type="Boolean" />
                            <asp:Parameter Name="original_toleranceID" Type="Int32" />
                        </UpdateParameters>
                        <SelectParameters>
                            <asp:Parameter Name="ToleranceID" Type="Int32" />
                            <asp:QueryStringParameter Name="ToleranceName" QueryStringField="ToleranceName" Type="String" />
                        </SelectParameters>
                        <InsertParameters>
                            <asp:Parameter Name="ToleranceName" Type="String" />
                            <asp:Parameter Name="DensityValue" Type="Double" />
                            <asp:Parameter Name="DensityTolerance" Type="String" />
                            <asp:Parameter Name="DensityUnits" Type="String" />
                            <asp:Parameter Name="ThicknessValue" Type="Double" />
                            <asp:Parameter Name="ThicknessTolerance" Type="String" />
                            <asp:Parameter Name="ThicknessUnits" Type="String" />
                            <asp:Parameter Name="WMDValue" Type="Double" />
                            <asp:Parameter Name="WMDTolerance" Type="String" />
                            <asp:Parameter Name="WMDUnits" Type="String" />
                            <asp:Parameter Name="AMDValue" Type="Double" />
                            <asp:Parameter Name="AMDTolerance" Type="String" />
                            <asp:Parameter Name="AMDUnits" Type="String" />
                        </InsertParameters>
                    </asp:ObjectDataSource>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
