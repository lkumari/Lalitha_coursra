<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" 
    AutoEventWireup="false"
    EnableEventValidation="false"
    MaintainScrollPositionOnPostback="true" 
    CodeFile="CostReductionList.aspx.vb"
    Inherits="CR_Cost_Reduction_List" 
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Visible="False"></asp:Label>
        <table width="98%">
            <tr>
                <td class="p_smalltextbold" style="color: #990000">
                    Review existing data or press
                    <asp:Button ID="btnAdd" runat="server" Text="Add" />
                    to enter new data.</td>
            </tr>
        </table>
        <hr />
        <i>Partial Searches can be completed by placing % before or after text.</i>
        <table width="98%">
            <tr>
                <td class="p_text">
                    Project No:
                </td>
                <td>
                    <asp:TextBox ID="txtProjectNo" runat="server" MaxLength="15" Width="100px" />
                    <ajax:FilteredTextBoxExtender ID="ftbProjectNo" runat="server" TargetControlID="txtProjectNo"
                        FilterType="Custom, Numbers" ValidChars="%" />
                </td>
                <td class="p_text">
                    Project Leader:</td>
                <td>
                    <asp:DropDownList ID="ddLeader" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    UGN Facility:
                </td>
                <td>
                    <asp:DropDownList ID="ddUGNFacility" runat="server" />
                </td>
                <td class="p_text" valign="top">
                    Commodity:
                </td>
                <td  valign="top">
                    <asp:DropDownList ID="ddCommodity" runat="server" /><br />{Commodity / Classification}
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Project Category:
                </td>
                <td>
                    <asp:DropDownList ID="ddProjectCategory" runat="server" />
                </td>
                <td class="p_text">
                    Description:
                </td>
                <td>
                    <asp:TextBox ID="txtDescription" runat="server" Width="300px" MaxLength="200" />
                    <ajax:FilteredTextBoxExtender ID="ftbeDescription" runat="server" TargetControlID="txtDescription"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,% " />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    RFD No:
                </td>
                <td>
                    <asp:TextBox ID="txtRFDNo" runat="server" Width="100px" />
                    <ajax:FilteredTextBoxExtender ID="ftbeRFD" runat="server" TargetControlID="txtRFDNo"
                        FilterType="Custom, Numbers" ValidChars="%" />
                </td>
                <td class="p_text">
                    Reviewed By PlantController:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddPlantControllerReviewed">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Text="Reviewed" Value="1"></asp:ListItem>
                        <asp:ListItem Text="NOT Reviewed" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Include Projects Completed 100%:
                </td>
                <td>
                    <asp:CheckBox runat="server" ID="cbIncludeCompleted" />
                </td>
                <td class="p_text">
                    Offsets Cost Downs:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddOffsetsCostDowns">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Text="Only Offsets Cost Downs" Value="1"></asp:ListItem>
                        <asp:ListItem Text="NO Offsets Cost Downs" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="3">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CommandName="search" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CommandName="reset" CausesValidation="False" />
                    <asp:Button ID="btnExportToExcel" runat="server" Text="Export to Excel" CausesValidation="true" />
                </td>
            </tr>
        </table>
        <hr />
        <i>Use the parameters above to filter the list below.</i><br />
        <font color="red">** Implementation Dates in RED indicates Overdue Projects.</font>
        <table width="98%">
            <tr>
                <td align="right">
                    <asp:RegularExpressionValidator ID="revGoToPage" runat="server" ControlToValidate="txtGoToPage"
                        ValidationGroup="vgTEP" ErrorMessage="Numeric Value Required." SetFocusOnError="True"
                        ValidationExpression="\b\d+\b" Height="20px"></asp:RegularExpressionValidator>
                    <asp:Label ID="lblCurrentPage" runat="server"></asp:Label>
                    <asp:Button ID="cmdFirst" runat="server" Text="|<" CssClass="button-search" />
                    <asp:Button ID="cmdPrev" runat="server" Text="<<" CssClass="button-search" />
                    <asp:TextBox ID="txtGoToPage" runat="server" MaxLength="4" Width="25" Height="15px"
                        Font-Size="Small" />
                    <asp:Button ID="cmdGo" runat="server" Text="Go" CssClass="button-search" ValidationGroup="vgTEP" />
                    <asp:Button ID="cmdNext" runat="server" Text=">>" CssClass="button-search" />
                    <asp:Button ID="cmdLast" runat="server" Text=">|" CssClass="button-search" />
                </td>
            </tr>
        </table>
        <table id="tblRepeater" runat="server" width="98%">
            <tbody>
                <tr>
                    <td>
                        <table width="98%">
                            <asp:Repeater ID="rpCostReduction" runat="server">
                                <SeparatorTemplate>
                                    <tr>
                                        <td colspan="11">
                                            <hr style="height: 0.01em" />
                                        </td>
                                    </tr>
                                </SeparatorTemplate>
                                <HeaderTemplate>
                                    <tr>
                                        <%If ViewState("Admin") = True Then%>
                                        <td class="p_tablebackcolor" align="center">
                                            Email to Admin
                                        </td>
                                        <%End If%>
                                        <td class="p_tablebackcolor">
                                            <asp:LinkButton ID="lnkDescription" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="Description">Description</asp:LinkButton>
                                        </td>
                                        <%If ViewState("ObjectRole") = True Then%>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkRank" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="Rank">Rank</asp:LinkButton>
                                        </td>
                                        <%End If%>
                                        <td class="p_tablebackcolor">
                                            <asp:LinkButton ID="lnkUGNFacility" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="UGNFacility">UGN Location</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor">
                                            <asp:LinkButton ID="lnkProjectCategory" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="ProjectCategoryName">Project Category</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" style="width: 100px">
                                            <asp:LinkButton ID="lnkCommodity" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="CommodityName">Commodity</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkEstImpDate" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="EstImpDate">Impl. Date</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkCompletion" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="Completion">%<br />Cmplt</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkProjectNo" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="ProjectNo">Project<br />No.</asp:LinkButton>
                                        </td>
                                        <td class="p_tablebackcolor">
                                            <asp:Label ID="lblLastUpdate" runat="server">Last Update</asp:Label>
                                        </td>
                                        <%If ViewState("isProposedDetailsViewable") = True Then%>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:Label ID="lblPreview" runat="server">Preview</asp:Label>
                                        </td>
                                        <%End If%>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <%If ViewState("Admin") = True Then%>
                                        <td align="center">
                                            <a runat="server" href="#" id="aHlinkEmail" onserverclick="aHlinkEmail_Click" 
                                             
                                            title='<%#DataBinder.Eval(Container, "DataItem.ProjectNo") & ":" & DataBinder.Eval(Container, "DataItem.Description") %>'>
                                                <asp:Image runat="server" ID="Image1" ImageUrl="~/images/Email1.jpg" />
                                            </a>
                                        </td>
                                        <%End If%>
                                        <td>
                                            <asp:HyperLink ID="HyperLink1" Font-Underline="true" runat="server" NavigateUrl='<%# "CostReduction.aspx?pProjNo=" & DataBinder.Eval (Container.DataItem,"ProjectNo").tostring %>'>
                                 <%#DataBinder.Eval(Container, "DataItem.Description")%>
                                            </asp:HyperLink>
                                        </td>
                                        <%If ViewState("ObjectRole") = True Then%>
                                        <td align="center">
                                            <%#IIF(DataBinder.Eval(Container, "DataItem.Rank")=0,0,String.Format("{0:###,###,###}", DataBinder.Eval(Container, "DataItem.Rank")))%>
                                        </td>
                                        <%End If%>
                                        <td>
                                            <%#DataBinder.Eval(Container, "DataItem.UGNFacilityName")%>
                                        </td>
                                        <td>
                                            <%#DataBinder.Eval(Container, "DataItem.ProjectCategoryName")%>
                                        </td>
                                        <td>
                                            <%#DataBinder.Eval(Container, "DataItem.CommodityName")%>
                                        </td>
                                        <td align="center" style="color: <%# SetTextColor(Container.DataItem("EstImpDate"),Container.DataItem("ProjectCategoryName")).ToString %>">
                                            <%#DataBinder.Eval(Container, "DataItem.EstImpDate")%>
                                        </td>
                                        <td align="center">
                                            <%#DataBinder.Eval(Container, "DataItem.Completion")%>
                                            %
                                        </td>
                                        <td align="center">
                                            <%#DataBinder.Eval(Container, "DataItem.ProjectNo")%>
                                        </td>
                                        <td>
                                            <%#DataBinder.Eval(Container, "DataItem.comboUpdateInfo")%>
                                        </td>
                                        <%If ViewState("isProposedDetailsViewable") = True Then%>
                                        <td align="center">
                                            <a runat="server" id="aPreview" target="blank" href='<%# SetPreviewFormHyperLink(Container.DataItem("ProjectNo")).ToString %>'>
                                                <asp:Image runat="server" ID="imgPreviewForm" ImageUrl="~/images/PreviewUp.jpg" />
                                            </a>
                                        </td>
                                        <%End If%>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <tr>
                                        <td colspan="11">
                                            <hr style="height: 0.01em" />
                                        </td>
                                    </tr>
                                </FooterTemplate>
                            </asp:Repeater>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>       
    </asp:Panel>
</asp:Content>
