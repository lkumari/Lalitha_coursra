<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="Material_List.aspx.vb" Inherits="Costing_Material_List" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <asp:Label runat="server" ID="lblMessage" SkinID="MessageLabelSkin"></asp:Label>
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <asp:ValidationSummary ID="vsMaterial" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgSearchMaterial" />
        <table width="98%">
            <tr>
                <td class="p_smalltextbold" style="color: #990000" colspan="4" align="left">
                    <asp:Label runat="server" ID="lblReview1" Text="Review existing materials or press"></asp:Label>
                    <asp:Button ID="btnAdd" runat="server" Text="Add" />
                    <asp:Label runat="server" ID="lblReview2" Text="to enter a new material."></asp:Label>
                    <hr />
                </td>
            </tr>
             <tr>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblSearchMaterialIDLabel" Text="Material ID:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchMaterialIDValue" Width="200px"></asp:TextBox>
                </td>                            
                <td colspan="2">
                   &nbsp;
                </td>
            </tr>
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblSearchPartNameLabel" Text="Material/Internal Part Desc:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchPartNameValue" Width="200px"></asp:TextBox>
                </td>
            
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblSearchDrawingNoLabel" Text="DMS Drawing No:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchDrawingNoValue" Width="200px" MaxLength="17"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblSearchPartNoLabel" Text="Internal Part No (RM):"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchPartNoValue" Width="200px"/>
                </td>
           
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblSearchVendorLabel" Text="Supplier:"/>
                </td>
                <td>
                    <asp:DropDownList ID="ddSearchVendorValue" runat="server" >
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblSearchPurchasedGoodLabel" Text="Purchased Good:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddSearchPurchasedGoodValue" runat="server">
                    </asp:DropDownList>
                </td>
           
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblSearchOldMaterialGroupLabel" Text="Old Material Group:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchOldMaterialGroupValue" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblSearchCoatingLabel" Text="Coating:"></asp:Label>
                </td>
                <td>
                     <asp:DropDownList runat="server" ID="ddSearchCoating">
                     <asp:ListItem Selected="True" Text="All" Value="All"></asp:ListItem>
                     <asp:ListItem Text="NO Coating" Value="None"></asp:ListItem>
                     <asp:ListItem Text="Only Coating" Value="Only"></asp:ListItem>                     
                     </asp:DropDownList>
                </td>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblSearchObsoleteLabel" Text="Obsolete:"></asp:Label>
                </td>
                <td>
                  <asp:DropDownList runat="server" ID="ddSearchObsolete">
                     <asp:ListItem Selected="True" Text="All" Value="All"></asp:ListItem>
                     <asp:ListItem Text="NO Obsolete" Value="None"></asp:ListItem>
                     <asp:ListItem Text="Only Obsolete" Value="Only"></asp:ListItem>
                  </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblSearchPackagingLabel" Text="Packaging:"></asp:Label>
                </td>
                <td>
                   <asp:DropDownList runat="server" ID="ddSearchPackaging">
                     <asp:ListItem Selected="True" Text="All" Value="All"></asp:ListItem>
                     <asp:ListItem Text="NO Packaging" Value="None"></asp:ListItem>
                     <asp:ListItem Text="Only Packaging" Value="Only"></asp:ListItem>
                  </asp:DropDownList>
                </td>
                <td class="p_text" align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblSearchUGNFacilityCodeLabel" Text="UGN Facility:" ></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddSearchUGNFacilityCode" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center">
                    <asp:Button runat="server" ID="btnSearch" CausesValidation="true" Text="Search" ValidationGroup="vgSearchMaterial" />
                    &nbsp;
                    <asp:Button runat="server" ID="btnReset" CausesValidation="false" Text="Reset" ValidationGroup="vgSearchMaterial" />
                </td>
            </tr>
        </table>
        <hr />
        <table width="98%">
            <tbody>
                <tr>
                    <td align="right">
                        <asp:RegularExpressionValidator ID="revGoToPage" runat="server" ControlToValidate="txtGoToPage"
                            ValidationGroup="vgCosting" ErrorMessage="Only numbers can be used for the pages."
                            SetFocusOnError="True" ValidationExpression="\b\d+\b" Height="20px"></asp:RegularExpressionValidator>
                        <asp:Label ID="lblCurrentPage" runat="server"></asp:Label>
                        <asp:Button ID="cmdFirst" runat="server" Text="|<" CssClass="button-search" Visible="false" />
                        <asp:Button ID="cmdPrev" runat="server" Text="<<" CssClass="button-search" Visible="false" />
                        <asp:TextBox ID="txtGoToPage" runat="server" Visible="false" MaxLength="4" Width="25"
                            Height="15px" Font-Size="Small" />
                        <asp:Button ID="cmdGo" runat="server" Text="Go" CssClass="button-search" Visible="false"
                            ValidationGroup="vgCosting" />
                        <asp:Button ID="cmdNext" runat="server" Text=">>" CssClass="button-search" Visible="false" />
                        <asp:Button ID="cmdLast" runat="server" Text=">|" CssClass="button-search" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%">
                            <asp:Repeater ID="rpMaterial" runat="server">
                                <HeaderTemplate>
                                    <tr>
                                    <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkMaterialID" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="MaterialID">Material ID</asp:LinkButton></td>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkMaterialName" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="MaterialName">Material Name</asp:LinkButton></td>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkDrawingNo" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="DrawingNo">Drawing No</asp:LinkButton></td>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkPartNo" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="PartNo">Internal Part No (RM)</asp:LinkButton></td>
                                        <%--<td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkPartRevision" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="PartRevision">BPCS Rev.</asp:LinkButton></td>--%>
                                                <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="UGNFacility" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="UGNFacilityCode">Facility</asp:LinkButton></td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkUGNDBVendorName" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="UGNDBVendorName">Supplier</asp:LinkButton></td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkBPCSStandardCost" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="BPCSStandardCost">Standard Cost</asp:LinkButton></td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkQuoteCost" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="QuoteCost">Quote Cost</asp:LinkButton></td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkFreightCost" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="FreightCost">Freight Cost</asp:LinkButton></td>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td align="center" style="white-space: nowrap;">
                                            <asp:HyperLink ID="selectMaterialID" runat="server" Font-Underline="true" NavigateUrl='<%# "Material_Maint.aspx?MaterialID=" & DataBinder.Eval (Container.DataItem,"MaterialID").tostring %>'><%#DataBinder.Eval(Container, "DataItem.MaterialID")%></asp:HyperLink>
                                        </td>
                                        <td align="left" style="white-space: nowrap;">
                                            <asp:HyperLink ID="selectMaterialName" runat="server" Font-Underline="true" NavigateUrl='<%# "Material_Maint.aspx?MaterialID=" & DataBinder.Eval (Container.DataItem,"MaterialID").tostring %>'><%#DataBinder.Eval(Container, "DataItem.ddMaterialName")%></asp:HyperLink>
                                        </td>
                                        <td align="left" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.DrawingNo")%>
                                        </td>
                                        <td align="left">
                                            <%#DataBinder.Eval(Container, "DataItem.PartNo")%>
                                        </td>
                                        <%--<td align="center">
                                            <%#DataBinder.Eval(Container, "DataItem.UGNFacilityCode")%>
                                        </td>--%>
                                        <td align="left" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.UGNFacilityCode")%>
                                        </td>
                                        <td align="left" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.ddVendorName")%>
                                        </td>
                                        
                                        <td align="right" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.StandardCost")%>
                                        </td>
                                        <td align="right" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.QuoteCost")%>
                                        </td>
                                        <td align="right" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.FreightCost")%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                </FooterTemplate>
                            </asp:Repeater>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
       
    </asp:Panel>
</asp:Content>
