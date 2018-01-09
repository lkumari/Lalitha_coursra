<%@ Page Language="VB" MasterPageFile="~/LookUpMasterPage.master" AutoEventWireup="false"
    CodeFile="IORsByAppropriation.aspx.vb" Inherits="PUR_IORsByAppropriation" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
        Visible="False" />
    <asp:Panel ID="localPanel" runat="server">
  <asp:GridView ID="gvIOR" runat="server" SkinID="StandardGrid" AutoGenerateColumns="False"
            DataKeyNames="IORNO" DataSourceID="odsIOR" Width="950px" OnRowDataBound="gvIOR_RowDataBound" AllowSorting="true">
            <Columns>
                <asp:TemplateField ShowHeader="False" HeaderText="Preview" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" ID="hlnkPreview" NavigateUrl='<%# "~/PUR/crViewInternalOrderRequest.aspx?pIORNo=" & DataBinder.Eval (Container.DataItem,"IORNO").tostring %>'
                            ImageUrl="~/images/PreviewUp.jpg" Target="_blank" ToolTip="Preview E-IOR" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="IORNO" HeaderText="IOR Ref #" ReadOnly="True" SortExpression="IORNO"
                    ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="IORDescription" HeaderText="Description" SortExpression="IORDescription"
                    HeaderStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="IORStatusDesc" HeaderText="Status" ReadOnly="True" SortExpression="IORStatusDesc"
                    HeaderStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="PONo" HeaderText="PO #" SortExpression="PONo" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Expense" SortExpression="TotalExpense">
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("TotalExpense", "{0:c}") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("TotalExpense") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:BoundField DataField="RequestedByName" HeaderText="Requisitioner" ReadOnly="True"
                    SortExpression="RequestedByName" HeaderStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="SubmittedOn" HeaderText="Date Submitted" ReadOnly="True"
                    SortExpression="SubmittedOn" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="UGNFacilityName" HeaderText="UGN Location" ReadOnly="True"
                    SortExpression="UGNFacilityName" HeaderStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Vendor" HeaderText="Vendor" ReadOnly="True" SortExpression="Vendor">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsIOR" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetIORbyExpProj" TypeName="InternalOrderRequestBLL">
            <SelectParameters>
                <asp:QueryStringParameter Name="AppropriationCode" QueryStringField="pProjNo" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
     
    </asp:Panel>
</asp:Content>
