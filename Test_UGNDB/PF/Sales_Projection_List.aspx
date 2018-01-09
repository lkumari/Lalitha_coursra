<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Sales_Projection_List.aspx.vb" Inherits="PMT_Sales_Projection_List"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="1100px" DefaultButton="btnSearch">
        <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
            Visible="False"></asp:Label>
        <table>
            <tr>
                <td class="p_smalltextbold" style="width: 279px; color: #990000">
                    Review existing data or press
                    <asp:Button ID="btnAdd" runat="server" Text="Add" />
                    to enter new data.
                </td>
            </tr>
        </table>
        <hr />
        <table>
            <tr>
                <td class="p_text">
                    Part Number:
                </td>
                <td>
                    <asp:TextBox ID="txtPartNo" runat="server" MaxLength="20"/>
                    <ajax:FilteredTextBoxExtender ID="ftbPartNo" runat="server" TargetControlID="txtPartNo"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,%- " />
                </td>
                <td class="p_text">
                    Product Technology:
                </td>
                <td>
                    <asp:DropDownList ID="ddProductTechnology" runat="server"/>
                </td>
            </tr>
            <tr>
                <td class="p_text" align="right" valign="top">
                    Customer:
                </td>
                <td>
                    <asp:DropDownList ID="ddCustomer" runat="server" AutoPostBack="true" /><br/>{Sold To / CABBV / Customer Name}
                </td>
                <td class="p_text" valign="top">
                    Commodity:
                </td>
                <td>
                    <asp:DropDownList ID="ddCommodity" runat="server" /><br />{Commodity / Classification}
                </td>
            </tr>
            <tr>
                <td class="p_text" align="right" valign="top">
                    Program:
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddProgram" runat="server" /><br/>
                    {Program / Model / Platform / Assembly Plant}
                </td>
            </tr>
            <tr>
                <td class="p_text" nowrap>
                    Program Status:
                </td>
                <td>
                    <asp:DropDownList ID="ddProgramStatus" runat="server">
                        <asp:ListItem Selected="True"></asp:ListItem>
                        <asp:ListItem Value="Awarded - New">Awarded - New</asp:ListItem>
                        <asp:ListItem Value="Awarded - Carry Over">Awarded - Carry Over</asp:ListItem>
                        <asp:ListItem Value="In Process">In Process</asp:ListItem>
                        <asp:ListItem Value="Loss Business">Loss Business</asp:ListItem>
                        <asp:ListItem Value="Potential - New">Potential - New</asp:ListItem>
                        <asp:ListItem Value="Potential - Carry Over">Potential - Carry Over</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="p_text">
                    Account Manager:
                </td>
                <td>
                    <asp:DropDownList ID="ddAccountManager" runat="server"/>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Royalty:
                </td>
                <td>
                    <asp:DropDownList ID="ddRoyalty" runat="server"/>
                </td>
                <td class="p_text" >
                    UGN Facility:
                </td>
                <td>
                    <asp:DropDownList ID="ddUGNFacility" runat="server"/>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="3">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" />
                </td>
            </tr>
        </table>
        <br />
        <em class="p_smalltextbold">Use the parameters above to filter the list below.</em>
        <br />
        <table id="TABLE1" width="100%">
            <tbody>
                <tr>
                    <td class="c_text" style="font-style: italic">
                        <asp:Label ID="lblRecListed" runat="server" Text="Records Listed: " />
                        <asp:Label ID="lblFromRec" runat="server" ForeColor="Red" />
                        <asp:Label ID="lblTo" runat="server" Text=" to " />
                        <asp:Label ID="lblToRec" runat="server" ForeColor="Red" />
                        <asp:Label ID="lblOf" runat="server" Text=" of " />
                        <asp:Label ID="lblTotalRecords" runat="server" ForeColor="Red" />
                    </td>
                    <td align="right" colspan="8">
                        <asp:RegularExpressionValidator ID="revGoToPage" runat="server" ControlToValidate="txtGoToPage"
                            ValidationGroup="vgPFList" ErrorMessage="Numeric Value Required." SetFocusOnError="True"
                            ValidationExpression="\b\d+\b" Height="20px"></asp:RegularExpressionValidator>
                        <asp:Label ID="lblCurrentPage" runat="server"></asp:Label>
                        <asp:Button ID="cmdFirst" runat="server" Text="|<" CssClass="button-search" />
                        <asp:Button ID="cmdPrev" runat="server" Text="<<" CssClass="button-search" />
                        <asp:TextBox ID="txtGoToPage" runat="server" MaxLength="4" Width="25" Height="15px"
                            Font-Size="Small" />
                        <asp:Button ID="cmdGo" runat="server" Text="Go" CssClass="button-search" ValidationGroup="vgPFList" />
                        <asp:Button ID="cmdNext" runat="server" Text=">>" CssClass="button-search" />
                        <asp:Button ID="cmdLast" runat="server" Text=">|" CssClass="button-search" />
                    </td>
                </tr>
                <tr>
                    <td colspan="9">
                        &nbsp;<asp:Repeater ID="rpVehicleVolume" runat="server">
                            <SeparatorTemplate>
                                <tr>
                                    <td colspan="9">
                                        <hr style="height: 0.01em" />
                                    </td>
                                </tr>
                            </SeparatorTemplate>
                            <HeaderTemplate>
                                <tr>
                                    <td class="p_tablebackcolor">
                                        <asp:Label ID="Label3" runat="server">Part Number</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:Label ID="Label5" runat="server">Commodity</asp:Label>
                                        &nbsp;
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:Label ID="lnkCustomer" runat="server">Sold To</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:Label ID="Label6" runat="server">Customer</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:Label ID="lnkProgram" runat="server">Program / Model / Platform / Assembly Plant</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:Label ID="lnkYear" runat="server">Program Status</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:Label ID="lnkVolume" runat="server">UGN Facility</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor" style="text-align: center">
                                        <asp:Label ID="Label2" runat="server">Account<br />Manager</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:Label ID="Label1" runat="server">LastUpdate</asp:Label>
                                    </td>
                                </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td nowrap>
                                        <asp:HyperLink ID="selectVehicleVolume" Font-Underline="true" runat="server" NavigateUrl='<%# "Sales_Projection.aspx?sPartNo=" & DataBinder.Eval (Container.DataItem,"PartNo").tostring %>'>
                                <%#DataBinder.Eval(Container, "DataItem.PartNo")%>
                                        </asp:HyperLink>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.CommodityName")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.SoldTo")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.CABBV")%>
                                    </td>
                                    <td nowrap>
                                        <%#DataBinder.Eval(Container, "DataItem.ProgramName")%>
                                    </td>
                                    <td nowrap>
                                        <%#DataBinder.Eval(Container, "DataItem.ProgramStatus")%>
                                    </td>
                                    <td nowrap>
                                        <%#DataBinder.Eval(Container, "DataItem.UGNFacilityName")%>
                                    </td>
                                    <td nowrap>
                                        <%#DataBinder.Eval(Container, "DataItem.AcctMgrName")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.comboUpdateInfo")%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
            </tbody>
        </table>
    </asp:Panel>
</asp:Content>
