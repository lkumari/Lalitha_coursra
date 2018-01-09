<%@ Page Language="VB" MasterPageFile="~/LookUpMasterPage.master" AutoEventWireup="false"
    CodeFile="SupplierLookUp.aspx.vb" Inherits="SUP_SupplierLookUp" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
        Visible="False" />
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <table>
            <tr>
                <td style="color: #990000; font-size: medium">
                    <% If ViewState("pForm") = "SUPPLIER" Then%>
                    Before you create and submit a new Supplier Request, please use the filters below
                    to search for the Supplier to confirm that it does not already exist.
                    <% Else%>
                    Use the filters below to search for a Supplier before making a selection.
                    <% End If%>
                </td>
            </tr>
        </table>
        <hr />
        <table border="0">
            <tr>
                <td class="p_text">
                    Supplier Request Ref No.:
                </td>
                <td>
                    <asp:TextBox ID="txtSUPNo" runat="server" MaxLength="6" Width="80px"></asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftbSUPNO" runat="server" TargetControlID="txtSUPNo"
                        FilterType="Custom" ValidChars="1234567890" />
                </td>
                <td class="p_text">
                    Supplier No.:
                </td>
                <td>
                    <asp:TextBox ID="txtVendor" runat="server" MaxLength="6" Width="80px"></asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftbVendor" runat="server" TargetControlID="txtVendor"
                        FilterType="Custom" ValidChars="1234567890" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Vendor Type:
                </td>
                <td>
                    <asp:DropDownList ID="ddVendorType" runat="server" />
                </td>
                <td class="p_text">
                    Supplier Name:
                </td>
                <td>
                    <asp:TextBox ID="txtVendorName" runat="server" MaxLength="50" Width="250px"></asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftbVendorName" runat="server" TargetControlID="txtVendorName"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,-. " />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Status:
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddRecStatus" runat="server">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Value="Active">Active</asp:ListItem>
                        <asp:ListItem Value="INACTIVE">Inactive</asp:ListItem>
                        <asp:ListItem Value="In Process">Pending Approval</asp:ListItem>
                        <asp:ListItem Value="New Entry">Pending Submission</asp:ListItem>
                        <asp:ListItem>Rejected</asp:ListItem>
                        <asp:ListItem>Void</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="3">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CommandName="search" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CommandName="reset" CausesValidation="False" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
                </td>
            </tr>
            <%  If ViewState("sBtnSrch") = True Then%>
            <tr>
                <td style="color: #990000; font-size: medium" colspan="4">
                    If the Supplier is not in the list below or no records were found, press
                    <asp:Button ID="btnAdd" runat="server" Text="New Supplier" />
                    to submit a New Supplier Request.
                    <br />
                    <br />
                    ** Please note the Supplier <i>Status</i> in the list below. If the Supplier you
                    wish to use is flagged "VOID" or "INACTIVE", <u>DO NOT</u> submit another request.<br />
                    Please contact Accounting or click on the "Email" icon to request a re-activation.
                </td>
            </tr>
            <% End If%>
        </table>
        <hr />
        <br />
        <br />
        <asp:GridView ID="gvSupplierLookUp" runat="server" AutoGenerateColumns="False" AllowSorting="True"
            DataSourceID="odsSupplierLookUp" AllowPaging="True" Width="100%" PageSize="60"
            SkinID="StandardGridWOFooter">
            <EmptyDataTemplate>
                <%  If ViewState("sBtnSrch") = True Then
                %>
                <asp:Label ID="lblTryAgain" runat="server" Text="Please try again." Font-Size="Medium"
                    ForeColor="Red" />
                <% End If%>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemTemplate>
                        <asp:ImageButton ID="ibtnSelect" runat="server" CommandName="Select" ImageUrl="~/images/SelectUser.gif"
                            PostBackUrl='<%# SetHyperlink(Container.DataItem("VendorType"),Container.DataItem("ddVendorNo"),Container.DataItem("RecStatus"),Container.DataItem("SUPNo")).ToString %>'
                            AlternateText="Send back previous page" ToolTip="Send back to parent page" Visible='<%# SetClickable(Container.DataItem("RecStatus")).ToString %>'
                            Target="_blank" Text='<%# Eval("SUPNo") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemTemplate>
                        <asp:ImageButton ID="ibtnActivate" runat="server" CommandName="REQSUPACT" ImageUrl="~/images/Email1.jpg"
                            PostBackUrl='<%# SetHyperlink2(Container.DataItem("VendorType"),Container.DataItem("ddVendorNo"),Container.DataItem("RecStatus"),Container.DataItem("VendorName"),Container.DataItem("SUPNo")).ToString %>'
                            AlternateText="Request Supplier Activation" ToolTip="Request Supplier Activation"
                            Visible='<%# SetClickable2(Container.DataItem("RecStatus")).ToString %>' Target="_blank" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="SUPNo" HeaderText="Supplier Request No." SortExpression="SUPNo"
                    HeaderStyle-Width="60px" HeaderStyle-Wrap="true" ItemStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" Visible="true">
                    <HeaderStyle HorizontalAlign="Center" Width="60px" Wrap="True" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="VendorType" HeaderText="Vendor Type" HeaderStyle-Width="80px"
                    HeaderStyle-Wrap="true" SortExpression="VendorType" ItemStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center" Width="80px" Wrap="True" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="VendorNo" HeaderText="Supplier No." SortExpression="VendorNo"
                    HeaderStyle-Width="70px" HeaderStyle-Wrap="true" ItemStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center" Width="70px" Wrap="True" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="VendorName" HeaderText="Supplier" SortExpression="VendorName"
                    HeaderStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="ReplacesVendorNo" HeaderText="Replaces Supplier No."
                    SortExpression="ReplacesVendorNo" HeaderStyle-Width="70px" HeaderStyle-Wrap="true"
                    ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center" Width="70px" Wrap="True" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="ReplacesVendor" HeaderText="Replaces Supplier" SortExpression="ReplacesVendor" />
                <asp:TemplateField HeaderText="Status" SortExpression="RecStatusDesc">
                    <ItemTemplate>
                        <asp:Label ID="lblRecStatusDesc" runat="server" Text='<%# Bind("RecStatusDesc") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:BoundField DataField="InBPCS" HeaderText="In Oracle" SortExpression="InBPCS" ItemStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="DateSubmitted" HeaderText="Date Submitted/ Last Used"
                    SortExpression="DateSubmitted" HeaderStyle-Width="80px" HeaderStyle-Wrap="true"
                    ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center" Width="80px" Wrap="True" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsSupplierLookUp" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetSupplierLookUp" TypeName="SupplierBLL">
            <SelectParameters>
                <asp:QueryStringParameter Name="SUPNo" QueryStringField="sSUPNo" Type="String" />
                <asp:QueryStringParameter DefaultValue="" Name="VendorName" QueryStringField="sSName"
                    Type="String" />
                <asp:QueryStringParameter DefaultValue="" Name="VendorType" QueryStringField="sVTYPE"
                    Type="String" />
                <asp:QueryStringParameter DefaultValue="" Name="RecStatus" QueryStringField="sRStat"
                    Type="String" />
                <asp:QueryStringParameter DefaultValue="" Name="VendorNo" QueryStringField="sVendor"
                    Type="String" />
                <asp:QueryStringParameter DefaultValue="false" Name="BtnSrch" QueryStringField="sBtnSrch"
                    Type="Boolean" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <br />
    </asp:Panel>
</asp:Content>
