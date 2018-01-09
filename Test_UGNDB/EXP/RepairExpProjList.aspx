<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="RepairExpProjList.aspx.vb" Inherits="EXP_RepairExpProjList" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="1000px" DefaultButton="btnSearch">
        <asp:Label ID="lblErrors" runat="server" Font-Bold="True" ForeColor="Red" Text="Label"
            Visible="False" />
        <table>
            <% If ViewState("pAprv") = 1 Then%>
            <tr>
                <td>
                    <asp:HyperLink ID="HyperLink1" runat="server" CssClass="c_text" ForeColor="blue">Go Back to Approval</asp:HyperLink>
                </td>
            </tr>
            <% End If%>
            <tr>
                <td class="p_smalltextbold" style="color: #990000">
                    Review existing data or press
                    <asp:Button ID="btnAdd" runat="server" Text="Add" />
                    to enter new data.
                </td>
            </tr>
        </table>
        <hr />
        <i>Partial Searches can be completed by placing % before or after text.</i>
        <table width="100%" border="0">
            <tr>
                <td class="p_text">
                    Project No:
                </td>
                <td>
                    <asp:TextBox ID="txtProjectNo" runat="server" MaxLength="15" Width="100px" />
                    <ajax:FilteredTextBoxExtender ID="ftbProjectNo" runat="server" TargetControlID="txtProjectNo"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890, %" />
                </td>
                <td class="p_text">
                    Supplement Project No:
                </td>
                <td>
                    <asp:TextBox ID="txtSupProjectNo" runat="server" MaxLength="15" Width="100px" />
                    <ajax:FilteredTextBoxExtender ID="ftbSupProjectNo" runat="server" TargetControlID="txtSupProjectNo"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890, %" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Project Title:
                </td>
                <td>
                    <asp:TextBox ID="txtProjectTitle" runat="server" MaxLength="50" Width="250px" />
                    <ajax:FilteredTextBoxExtender ID="ftbProjectTitle" runat="server" TargetControlID="txtProjectTitle"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890, %." />
                </td>
                <td class="p_text">
                    UGN Location:
                </td>
                <td>
                    <asp:DropDownList ID="ddUGNFacility" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Project Leader:
                </td>
                <td>
                    <asp:DropDownList ID="ddProjectLeader" runat="server">
                    </asp:DropDownList>
                </td>
                <td class="p_text">
                    Department or Cost Center:
                </td>
                <td>
                    <asp:TextBox ID="txtDepartment" runat="server" MaxLength="50" Width="250px" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    Project Status:
                </td>
                <td class="c_textbold" style="color: red;" colspan="3">
                    <asp:DropDownList ID="ddProjectStatus" runat="server">
                        <asp:ListItem Selected="True"></asp:ListItem>
                        <asp:ListItem Value="NOpen">New Project</asp:ListItem>
                        <asp:ListItem Value="AApproved">Approved</asp:ListItem>
                        <asp:ListItem Value="CCapitalized">Capitalized</asp:ListItem>
                        <asp:ListItem Value="ACompleted">Completed</asp:ListItem>
                        <asp:ListItem Value="TIn Process">In Process</asp:ListItem>
                        <asp:ListItem Value="RIn Process">Rejected</asp:ListItem>
                        <asp:ListItem Value="VVoid">Void</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="3">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CommandName="search" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CommandName="reset" CausesValidation="False" />
                </td>
            </tr>
        </table>
        <hr />
        <i>Use the parameters above to filter the list below.</i>
        <table id="TABLE1" width="100%">
            <tbody>
                <tr>
                    <td class="c_text" style="font-style: italic" colspan="2">
                        <asp:Label ID="lblRecListed" runat="server" Text="Records Listed: " />
                        <asp:Label ID="lblFromRec" runat="server" ForeColor="Red" />
                        <asp:Label ID="lblTo" runat="server" Text=" to " />
                        <asp:Label ID="lblToRec" runat="server" ForeColor="Red" />
                        <asp:Label ID="lblOf" runat="server" Text=" of " />
                        <asp:Label ID="lblTotalRecords" runat="server" ForeColor="Red" />
                    </td>
                    <td align="right" colspan="10">
                        <asp:RegularExpressionValidator ID="revGoToPage" runat="server" ControlToValidate="txtGoToPage"
                            ValidationGroup="vgTEP" ErrorMessage="Numeric Value Required." SetFocusOnError="True"
                            ValidationExpression="\b\d+\b" Height="20px"></asp:RegularExpressionValidator>
                        <asp:Label ID="lblCurrentPage" runat="server" />
                        <asp:Button ID="cmdFirst" runat="server" Text="|<" CssClass="button-search" />
                        <asp:Button ID="cmdPrev" runat="server" Text="<<" CssClass="button-search" />
                        <asp:TextBox ID="txtGoToPage" runat="server" MaxLength="4" Width="25" Height="15px"
                            Font-Size="Small" />
                        <asp:Button ID="cmdGo" runat="server" Text="Go" CssClass="button-search" ValidationGroup="vgTEP" />
                        <asp:Button ID="cmdNext" runat="server" Text=">>" CssClass="button-search" />
                        <asp:Button ID="cmdLast" runat="server" Text=">|" CssClass="button-search" />
                    </td>
                </tr>
                <tr>
                    <td colspan="12">
                        &nbsp;<asp:Repeater ID="rpRepairExpProj" runat="server">
                            <SeparatorTemplate>
                                <tr>
                                    <td colspan="12">
                                        <hr style="height: 0.01em" />
                                    </td>
                                </tr>
                            </SeparatorTemplate>
                            <HeaderTemplate>
                                <tr>
                                    <td class="p_tablebackcolor">
                                        <asp:Label ID="Label3" runat="server">Project No.</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:Label ID="Label6" runat="server">Project Title</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:Label ID="Label5" runat="server">Project Leader</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:Label ID="Label1" runat="server">UGN Location</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor" align="center">
                                        <asp:Label ID="lnkProgram" runat="server">Date<br />Submitted</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor" align="center">
                                        <asp:Label ID="Label4" runat="server">Project<br />Status</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor" style="width: 60px" align="center">
                                        <asp:Label ID="Label2" runat="server">Add<br />Supplement</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor" align="center">
                                        <asp:Label ID="lnkVolume" runat="server">Preview</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor" align="center">
                                        <asp:Label ID="Label7" runat="server">History</asp:Label>
                                    </td>
                                    <td class="p_tablebackcolor" align="center">
                                        <asp:Label ID="Label8" runat="server">IOR's</asp:Label>
                                    </td>
                                </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:HyperLink ID="getData" Font-Underline="true" runat="server" NavigateUrl='<%# "RepairExpProj.aspx?pProjNo=" & DataBinder.Eval (Container.DataItem,"ProjectNo").tostring & "&pPrntProjNo=" &  DataBinder.Eval (Container.DataItem,"ParentProjectNo").tostring %>'>
                                 <%#DataBinder.Eval(Container, "DataItem.ProjectNo")%>
                                        </asp:HyperLink>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.ProjectTitle")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.ProjectLeaderName")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.UGNFacilityName")%>
                                    </td>
                                    <td align="center">
                                        <%#DataBinder.Eval(Container, "DataItem.DateSubmitted")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.ProjectStatusDesc")%>
                                    </td>
                                    <td align="center">
                                        <asp:HyperLink ID="lnkAppend" runat="server" ImageUrl="~/images/copy.jpg" Height="24"
                                            Width="24" NavigateUrl='<%# GoToAppend(DataBinder.Eval(Container, "DataItem.ProjectNo"),DataBinder.Eval(Container, "DataItem.ParentProjectNo"))  %>'
                                            Visible='<%# ShowHideImageAppend(DataBinder.Eval(Container, "DataItem.ParentProjectNo"),DataBinder.Eval(Container, "DataItem.RoutingStatus")) %>' />
                                    </td>
                                    <td align="center">
                                        <asp:HyperLink ID="lnkPreview" runat="server" NavigateUrl='<%# "crViewExpProjRepair.aspx?pProjNo=" & DataBinder.Eval (Container.DataItem,"ProjectNo").tostring %>'
                                            Target="_blank" ImageUrl="~/images/PreviewUp.jpg"></asp:HyperLink>
                                    </td>
                                    <td align="center">
                                        <asp:HyperLink ID="lnkHistory" runat="server" NavigateUrl='<%# "RepairExpProjHistory.aspx?pProjNo=" & DataBinder.Eval (Container.DataItem,"ProjectNo").tostring  %>'
                                            ImageUrl="~/images/History.jpg" Visible='<%# ShowHideHistory(DataBinder.Eval(Container, "DataItem.ProjectStatus")) %>' />
                                    </td>
                                    <td align="center">
                                        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl='<%# "IORsByAppropriation.aspx?pProjNo=" & DataBinder.Eval (Container.DataItem,"ProjectNo").tostring  %>'
                                            ImageUrl="~/images/VolSales.jpg" Visible='<%# ShowHideIORimg(DataBinder.Eval(Container, "DataItem.ProjectNo")) %>' />
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                <tr>
                                    <td colspan="12">
                                        <hr style="height: 0.01em" />
                                    </td>
                                </tr>
                            </FooterTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
            </tbody>
        </table>
    </asp:Panel>
</asp:Content>
