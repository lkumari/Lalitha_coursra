<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="ContainerList.aspx.vb" Inherits="Packaging_ContainerList" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Height="416px">
        <table style="width: 344px">
            <tr>
                <td align="left" class="p_smalltextbold" style="color: #990000">
                    Review existing data or press
                    <asp:Button ID="btnAdd" runat="server" Text="Add" />
                    to enter new data.
                </td>
            </tr>
        </table>
        <asp:Label ID="lblMessage" runat="server" ForeColor="#ff0000" SkinID="MessageLabelSkin"></asp:Label>
        <br />
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" />
        <br />
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <br />
        <hr />
        <table>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblContainerNo" runat="server" Text="Container No:" />
                </td>
                <td>
                    <asp:TextBox ID="txtContainerNo" runat="server" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblDesc" runat="server" Text="Description:" />
                </td>
                <td>
                    <asp:TextBox ID="txtDescription" runat="server" MaxLength="50" Width="200px" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="Label1" runat="server" Text="OEM Manufacturer:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddOEM" runat="server" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblType" runat="server" Text="Type:" />
                </td>
                <td>
                    <asp:TextBox ID="txtType" runat="server" MaxLength="30" Width="200px" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblCust" runat="server" Text="Customer:" />
                </td>
                <td>
                    <asp:TextBox ID="txtCustomer" runat="server" MaxLength="240" Width="200px" />
                </td>
                <td class="p_text">
                    <asp:Label ID="lblSupplier" runat="server" Text="Supplier:" />
                </td>
                <td>
                    <asp:DropDownList ID="ddVendor" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="3">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" ValidationGroup="" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" />
                </td>
            </tr>
        </table>
        <asp:ValidationSummary ID="summaryList" runat="server" ValidationGroup="vsList" ShowMessageBox="true" />
        <hr />
        <i>Use the parameters above to filter the list below.</i>
        <table>
            <tr>
                <td class="c_smalltext" style="font-style: italic" width="550px">
                    <asp:Label ID="lblRecListed" runat="server" Text="Records Listed: " />
                    <asp:Label ID="lblFromRec" runat="server" ForeColor="Red" />
                    <asp:Label ID="lblTo" runat="server" Text=" to " />
                    <asp:Label ID="lblToRec" runat="server" ForeColor="Red" />
                    <asp:Label ID="lblOf" runat="server" Text=" of " />
                    <asp:Label ID="lblTotalRecords" runat="server" ForeColor="Red" />
                </td>
                <td width="300px" align="right">
                    <asp:Label ID="PagingInformation" runat="server" Text="" /><asp:DropDownList ID="PageList"
                        runat="server" CssClass="c_smalltext" AutoPostBack="true" OnSelectedIndexChanged="PageList_SelectedIndexChanged" />
                </td>
            </tr>
        </table>
        <asp:GridView ID="gvContainer" runat="server" AutoGenerateColumns="False" DataKeyNames="CID"
            DataSourceID="odsContainer" SkinID="StandardGridWOFooter" Width="850px" PageSize="30"
            OnRowDataBound="gvContainer_RowDataBound" OnPageIndexChanged="gvContainer_PageIndexChanged"
            OnSorting="gvContainer_Sorting">
            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" ID="hlknkEdit" ImageUrl="~/images/edit.jpg" ToolTip="Edit Record"
                            NavigateUrl='<%# "Container.aspx?pCNO=" & DataBinder.Eval (Container.DataItem,"ContainerNo").tostring%>' /></ItemTemplate>
                    <HeaderStyle Width="30px" />
                    <ItemStyle HorizontalAlign="Center" Width="30px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Container No" SortExpression="ContainerNo" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("ContainerNo") %>' />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:BoundField DataField="Desc" HeaderText="Description" SortExpression="Desc" HeaderStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" HeaderStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="ddOEMDesc" HeaderText="OEM Mfg" SortExpression="ddOEMDesc"
                    HeaderStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="ColorDesc" HeaderText="Color" SortExpression="ColorDesc"
                    HeaderStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="comboUpdateInfo" HeaderText="Last Update" ReadOnly="True"
                    SortExpression="comboUpdateInfo" HeaderStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsContainer" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetPKGContainer" TypeName="PKGBLL">
            <SelectParameters>
                <asp:Parameter Name="CID" Type="Int32" />
                <asp:ControlParameter ControlID="txtContainerNo" Name="ContainerNo" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="txtDescription" Name="Description" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="txtType" Name="Type" PropertyName="Text" Type="String" />
                <asp:ControlParameter ControlID="ddOEM" Name="OEM" PropertyName="SelectedValue" Type="String" />
                <asp:ControlParameter ControlID="txtCustomer" Name="Customer" PropertyName="Text"
                    Type="String" />
                <asp:ControlParameter ControlID="ddVendor" DefaultValue="" Name="Vendor" PropertyName="SelectedValue"
                    Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:Label class="c_smalltext" ID="SortInformationLabel" runat="server" />
    </asp:Panel>
</asp:Content>
