<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Formula_List.aspx.vb" Inherits="Formula_List" MaintainScrollPositionOnPostback="true"
    Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" DefaultButton="btnSearch">
        <asp:Label runat="server" ID="lblMessage" SkinID="MessageLabelSkin"></asp:Label>
        <asp:Label ID="lblSearchTip" runat="server"><i>Partial Searches can be completed by placing % before or after text.</i></asp:Label>
        <asp:ValidationSummary ID="vsFormulaSearchInfo" runat="server" DisplayMode="List"
            ShowMessageBox="true" ShowSummary="true" EnableClientScript="true" ValidationGroup="vgSearchInfo" />
        <table width="98%">
            <tr>
                <td class="p_smalltextbold" style="color: #990000" colspan="4" align="left">
                    <asp:Label runat="server" ID="lblReview1" Text="Review existing formulas or press"></asp:Label>
                    <asp:Button ID="btnAdd" runat="server" Text="Add" />
                    <asp:Label runat="server" ID="lblReview2" Text="to enter a new formula."></asp:Label>
                    <hr />
                </td>
            </tr>
            <tr>
                <td align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblSearchFormulaName" Text="Formula Name:" CssClass="p_text"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchFormulaName" Width="200px" maxlength="50" Visible="false"></asp:TextBox>
                </td>
               <td align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblSearchDrawingNo" Text="Drawing No:" CssClass="p_text"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchDrawingNo" Width="200px" maxlength="17" Visible="false"></asp:TextBox>
                </td>
               
            </tr>
           <%-- <tr>
                 <td align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblSearchPartName" Text="Internal Part Name:" CssClass="p_text"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchPartName" Width="200px" maxlength="50" Visible="false"></asp:TextBox>
                </td>
                <td align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblSearchPartNo" Text="Internal Part No:" CssClass="p_text"></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearchPartNo" Width="200px" MaxLength="15" Visible="false"></asp:TextBox>
                </td>
            </tr>--%>
            <tr>
                <td align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblSearchDepartment" Text="Department:" Visible="false"
                        CssClass="p_text"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddSearchDepartment" runat="server" Visible="false">
                    </asp:DropDownList>
                </td>
                <td align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblSearchProcess" Text="Process:" Visible="false" CssClass="p_text"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddSearchProcess" runat="server" Visible="false">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right" style="white-space: nowrap;">
                    <asp:Label runat="server" ID="lblSearchTemplate" Text="Template:" Visible="false"
                        CssClass="p_text"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddSearchTemplate" runat="server" Visible="false">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center">
                    <asp:Button runat="server" ID="btnSearch" CausesValidation="true" Text="Search" ValidationGroup="vgSearchInfo"
                        Visible="false" />
                    &nbsp;
                    <asp:Button runat="server" ID="btnReset" CausesValidation="false" Text="Reset" ValidationGroup="vgSearchInfo"
                        Visible="false" />
                </td>
            </tr>
        </table>
        <hr />
        <table width="98%">
            <tbody>
                <tr>
                    <td colspan="7" align="right">
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
                    <td colspan="7">
                        <table width="100%">
                            <asp:Repeater ID="rpFormula" runat="server">
                                <HeaderTemplate>
                                    <tr>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkFormulaName" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="FormulaName">Formula</asp:LinkButton></td>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkDrawingNo" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="DrawingNo">Drawing No</asp:LinkButton></td>
                                        <%--  <td class="p_tablebackcolor" align="left">
                                          <asp:LinkButton ID="lnkPartNo" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="PartNo">Internal PartNo</asp:LinkButton></td>
                                        <td class="p_tablebackcolor" align="center">
                                            <asp:LinkButton ID="lnkPartRevision" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="PartRevision">BPCS Rev.</asp:LinkButton></td>--%>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkDepartment" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="DepartmentName">Department</asp:LinkButton></td>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkProcess" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="ProcessName">Process</asp:LinkButton></td>
                                        <td class="p_tablebackcolor" align="left">
                                            <asp:LinkButton ID="lnkTemplate" ForeColor="white" runat="server" OnClick="SortCommand"
                                                CommandArgument="TemplateName">Template</asp:LinkButton></td>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td align="left" style="white-space: nowrap;">
                                            <asp:HyperLink ID="selectFormulaID" runat="server" Font-Underline="true" NavigateUrl='<%# "Formula_Maint.aspx?FormulaID=" & DataBinder.Eval (Container.DataItem,"FormulaID").tostring %>'><%#DataBinder.Eval(Container, "DataItem.ddFormulaName")%></asp:HyperLink>
                                        </td>
                                        <td align="left" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.DrawingNo")%>
                                        </td>
                                      <%--   <td align="left">
                                            <%#DataBinder.Eval(Container, "DataItem.PartNo")%>
                                        </td>
                                       <td align="center">
                                            <%#DataBinder.Eval(Container, "DataItem.PartRevision")%>
                                        </td>--%>
                                        <td align="left" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.ddDepartmentName")%>
                                        </td>
                                        <td align="left" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.ddProcessName")%>
                                        </td>
                                        <td align="left" style="white-space: nowrap;">
                                            <%#DataBinder.Eval(Container, "DataItem.ddTemplateName")%>
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
