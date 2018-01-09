<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Copy_Vehicle.aspx.vb" Inherits="PF_Copy_Vehicle" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="100%">
        <asp:Label ID="lblErrors" runat="server" Text="Label" Visible="False" Font-Bold="True"
            ForeColor="Red" />
        <hr />
        <table width="80%">
            <tr>
                <td class="c_text">
                    Data will be copied from this source Program: <font color="red" size="3px"><b>
                        <%=HttpContext.Current.Request.QueryString("DisplayName")%>
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
                   Use the table below to select a new destination of vehicle(s) to copy program info.
                </td>
            </tr>
            <tr>
                <td class="c_text">
                    <asp:GridView ID="gvCopy" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                        GridLines="Horizontal" Width="50%" OnRowDataBound="gvCopy_RowDataBound" OnRowCommand="gvCopy_RowCommand"
                        DataKeyNames="SourceProgramID,SourceCABBV,SourceSoldTo,DestinationProgramID,DestinationCABBV,DestinationSoldTo"
                        EmptyDataText="No Records Found in the database." DataSourceID="odsCopy">
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
                            <asp:TemplateField HeaderText="Destination Program" SortExpression="DestinationProgramID">
                                <EditItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("DestinationProgramName") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("DestinationProgramName") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddDestinationProgramID" runat="server" DataSource='<%# commonFunctions.GetProgram("","","") %>'
                                        DataValueField="ProgramID" DataTextField="ddProgramName" SelectedValue='<%# Bind("DestinationProgramID") %>'
                                        AppendDataBoundItems="True">
                                        <asp:ListItem Selected="True"></asp:ListItem>
                                    </asp:DropDownList>&nbsp;
                                    <asp:RequiredFieldValidator ID="rfvDestPartNo" runat="server" ControlToValidate="ddDestinationProgramID"
                                        ErrorMessage="Destination Program is a required field." ValidationGroup="InsertProgramInfo"><</asp:RequiredFieldValidator>
                                </FooterTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    &nbsp;&nbsp;<asp:ImageButton ID="ibtnDelete" runat="server" CausesValidation="False"
                                        CommandName="Delete" ImageUrl="~/images/delete.jpg"  AlternateText="Delete"
                                        OnClientClick="return confirm('Are you certain you want to delete this Subscription?');"
                                        ValidationGroup="EditPriceInfo" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    &nbsp;&nbsp;<asp:ImageButton ID="ibtnInsert" runat="server" CausesValidation="True"
                                        CommandName="Insert" ImageUrl="~/images/save.jpg"  AlternateText="Insert"
                                        ValidationGroup="InsertProgramInfo" />&nbsp;&nbsp;&nbsp;
                                    <asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo" ImageUrl="~/images/undo-gray.jpg"
                                         Text="Undo" AlternateText="Undo" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="odsCopy" runat="server" DeleteMethod="DeleteVehicleCopy"
                        InsertMethod="InsertVehicleCopy" SelectMethod="GetVehicleCopy" TypeName="Vehicle_CopyBLL"
                        OldValuesParameterFormatString="original_{0}">
                        <DeleteParameters>
                            <asp:QueryStringParameter Name="SourceProgramID" QueryStringField="sPGMID" Type="Int32" />
                            <asp:QueryStringParameter Name="SourceCABBV" QueryStringField="sCABBV" Type="String" />
                            <asp:QueryStringParameter Name="SourceSoldTo" QueryStringField="sSoldTo" Type="Int32" />
                            <asp:Parameter Name="DestinationProgramID" Type="Int32" />
                            <asp:QueryStringParameter Name="DestinationCABBV" QueryStringField="sCABBV" Type="String" />
                            <asp:QueryStringParameter Name="DestinationSoldTo" QueryStringField="sSoldTo" Type="Int32" />
                             <asp:Parameter Name="original_SourceProgramID" Type="Int32" />
                             <asp:Parameter Name="original_SourceCABBV" Type="String" />
                             <asp:Parameter Name="original_SourceSoldTo" Type="Int32" />
                             <asp:Parameter Name="original_DestinationProgramID" Type="Int32" />
                             <asp:Parameter Name="original_DestinationCABBV" Type="String" />
                             <asp:Parameter Name="original_DestinationSoldTo" Type="Int32" />
                        </DeleteParameters>
                        <SelectParameters>
                            <asp:QueryStringParameter Name="SourceProgramID" QueryStringField="sPGMID" Type="Int32" />
                            <asp:QueryStringParameter Name="SourceCABBV" QueryStringField="sCABBV" Type="String" />
                            <asp:QueryStringParameter Name="SourceSoldTo" QueryStringField="sSoldTo" Type="Int32" />
                        </SelectParameters>
                        <InsertParameters>
                            <asp:QueryStringParameter Name="SourceProgramID" QueryStringField="sPGMID" Type="Int32" />
                            <asp:QueryStringParameter Name="SourceCABBV" QueryStringField="sCABBV" Type="String" />
                            <asp:QueryStringParameter Name="SourceSoldTo" QueryStringField="sSoldTo" Type="Int32" />
                            <asp:Parameter Name="DestinationProgramID" Type="Int32" />
                            <asp:QueryStringParameter Name="DestinationCABBV" QueryStringField="sCABBV" Type="String" />
                            <asp:QueryStringParameter Name="DestinationSoldTo" QueryStringField="sSoldTo" Type="Int32" />
                        </InsertParameters>
                    </asp:ObjectDataSource>
                    <asp:ValidationSummary ID="vsProgramInfo" runat="server" ValidationGroup="EmptyProgramInfo"
                        ShowMessageBox="true" />
                    &nbsp;
                    <asp:ValidationSummary ID="vsInsertProgramInfo" runat="server" ShowMessageBox="True"
                        ValidationGroup="InsertProgramInfo" />
                </td>
            </tr>
            <tr>
                <td class="c_text">
                    &nbsp;<asp:Button ID="btnSubmit" runat="server" Text="Submit" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" /></td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
