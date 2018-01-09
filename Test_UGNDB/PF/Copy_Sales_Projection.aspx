<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Copy_Sales_Projection.aspx.vb" Inherits="PF_Copy_Sales_Projection"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="100%">
        <asp:Label ID="lblErrors" runat="server" Text="Label" Visible="False" Font-Bold="True"
            ForeColor="Red" />
        <hr />
        <table width="80%">
            <tr>
                <td class="c_text">
                    Data will be copied from this source Part Number: <font color="red" size="3px"><b>
                        <%=HttpContext.Current.Request.QueryString("sPartNo")  %>
                    </b></font>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="c_text">
                    Select the new destination part number(s) to copy data into in the table below.
                </td>
            </tr>
            <tr>
                <td class="c_text">
                    <asp:GridView ID="gvCopy" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                        DataKeyNames="SourcePartNo,DestinationPartNo" DataSourceID="odsCopy" GridLines="Horizontal"
                        Width="44%" OnRowDataBound="gvCopy_RowDataBound" OnRowCommand="gvCopy_RowCommand">
                        <FooterStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="White" Wrap="False" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Wrap="False" />
                        <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" Wrap="False" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" Wrap="False" />
                        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#E2DED6" Wrap="False" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" Wrap="False" />
                        <EmptyDataRowStyle BackColor="White" Wrap="False" />
                        <EmptyDataTemplate>
                            No Records Found in the database.
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="Destination Part Number" SortExpression="DestinationPartNo">
                                <EditItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("DestinationPartNo") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("DestinationPartNo") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddDestinationPartNo" runat="server" DataSource='<%# commonFunctions.GetBPCSPartNo("","C") %>'
                                        DataValueField="BPCSPartNo" DataTextField="PartNo" SelectedValue='<%# Bind("DestinationPartNo") %>'
                                        AppendDataBoundItems="True">
                                        <asp:ListItem Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp;
                                    <asp:RequiredFieldValidator ID="rfvDestPartNo" runat="server" ControlToValidate="ddDestinationPartNo"
                                        ErrorMessage="Destination Part Number is a required field." ValidationGroup="InsertPartInfo"><</asp:RequiredFieldValidator>
                                </FooterTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    &nbsp;&nbsp;<asp:ImageButton ID="ibtnDelete" runat="server" CausesValidation="False"
                                        CommandName="Delete" ImageUrl="~/images/delete.jpg" Text="Delete" AlternateText="Delete"
                                        OnClientClick="return confirm('Are you certain you want to delete this &#13;&#10;Subscription?');"
                                        ValidationGroup="EditPriceInfo" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    &nbsp;&nbsp;<asp:ImageButton ID="ibtnInsert" runat="server" CausesValidation="True"
                                        CommandName="Insert" ImageUrl="~/images/save.jpg" Text="Insert" AlternateText="Insert"
                                        ValidationGroup="InsertPartInfo" />&nbsp;&nbsp;&nbsp;
                                    <asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo" ImageUrl="~/images/undo-gray.jpg"
                                        Text="Undo" AlternateText="Undo" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="odsCopy" runat="server" DeleteMethod="DeleteProjectedSalesCopy"
                        InsertMethod="InsertProjectedSalesCopy" SelectMethod="GetProjectedSalesCopy"
                        TypeName="Projected_Sales_CopyBLL" OldValuesParameterFormatString="original_{0}">
                        <DeleteParameters>
                            <asp:QueryStringParameter Name="SourcePartNo" QueryStringField="sPartNo" Type="String" />
                            <asp:Parameter Name="DestinationPartNo" Type="String" />
                            <asp:Parameter Name="original_SourcePartNo" Type="String" />
                            <asp:Parameter Name="original_DestinationPartNo" Type="String" />
                        </DeleteParameters>
                        <SelectParameters>
                            <asp:QueryStringParameter Name="SourcePartNo" QueryStringField="sPartNo" Type="String" />
                        </SelectParameters>
                        <InsertParameters>
                            <asp:QueryStringParameter Name="SourcePartNo" QueryStringField="sPartNo" Type="String" />
                            <asp:Parameter Name="DestinationPartNo" Type="String" />
                        </InsertParameters>
                    </asp:ObjectDataSource>
                    <asp:ValidationSummary ID="vsPartInfo" runat="server" ValidationGroup="EmptyPartInfo"
                        ShowMessageBox="true" />
                    &nbsp;
                    <asp:ValidationSummary ID="vsInsertPartInfo" runat="server" ShowMessageBox="True"
                        ValidationGroup="InsertPartInfo" />
                </td>
            </tr>
            <tr>
                <td class="c_text">
                    &nbsp;<asp:Button ID="btnSubmit" runat="server" Text="Submit" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
