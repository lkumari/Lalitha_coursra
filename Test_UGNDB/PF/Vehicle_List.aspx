<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Vehicle_List.aspx.vb" Inherits="PMT_Vehicle_List" Title="Untitled Page"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server" Width="1200px" DefaultButton="btnSearch">
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
                <td class="p_text" valign="top">
                    Planning Year:
                </td>
                <td  valign="top"> 
                    <asp:DropDownList ID="ddYear" runat="server"/>
                </td>
                <td class="p_text" valign="top">
                    Customer:
                </td>
                <td>
                    <asp:DropDownList ID="ddCustomer" runat="server" /><br />{Sold To / CABBV / Customer Name}
                </td>
            </tr>
            <tr>
                <td class="p_text"  valign="top">
                    Make:
                </td>
                <td  valign="top">
                    <%--<asp:DropDownList ID="ddMake" runat="server" />--%>
                    <asp:DropDownList ID="ddMakes" runat="server" />
                </td>
                <td class="p_text" valign="top">
                    Program:
                </td>
                <td>
                    <asp:DropDownList ID="ddProgram" runat="server" /><br />{Program / Model / Platform / Assembly Plant}
                </td>
            </tr>
            <tr>
                <td class="p_text"  valign="top">
                    Account Manager:
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddAccountManager" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                </td>
                <td colspan="3">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" />
                </td>
            </tr>
        </table>
       <%-- <ajax:CascadingDropDown ID="cddMakes" runat="server" TargetControlID="ddMakes" Category="Make"
            PromptText="Please select a Make." LoadingText="[Loading Makes...]" ServicePath="~/WS/VehicleCDDService.asmx"
            ServiceMethod="GetMakes" />
        <ajax:CascadingDropDown ID="cddProgram" runat="server" TargetControlID="ddProgram"
            ParentControlID="ddMakes" Category="Program" PromptText="Please select a Program."
            LoadingText="[Loading Programs...]" ServicePath="~/WS/VehicleCDDService.asmx"
            ServiceMethod="GetProgramsModelPlatformAssembly" />--%>
        <br />
        <em class="p_smalltextbold">Use the parameters above to filter the list below.</em><br />
        <em class="p_smalltextbold" style="font-size: small; color: Red;">Disabled links denotes
            an "INACTIVE PROGRAM" that should not be in use for Budget or Forecast.</em>
        <br />
        <table id="TABLE1" width="80%">
            <tbody>
                <tr>
                 <td class="c_text" style="font-style:italic">
                        <asp:Label ID="lblRecListed" runat="server" Text="Records Listed: " />
                        <asp:Label ID="lblFromRec" runat="server" ForeColor="Red" />
                        <asp:Label ID="lblTo" runat="server" Text=" to " />
                        <asp:Label ID="lblToRec" runat="server" ForeColor="Red" />
                        <asp:Label ID="lblOf" runat="server" Text=" of " />
                        <asp:Label ID="lblTotalRecords" runat="server"  ForeColor="Red"/>
                    </td>
                    <td align="right" colspan="8">
                        <asp:RegularExpressionValidator ID="revGoToPage" runat="server" ControlToValidate="txtGoToPage"
                            ValidationGroup="vgTIList" ErrorMessage="Numeric Value Required." SetFocusOnError="True"
                            ValidationExpression="\b\d+\b" Height="20px"></asp:RegularExpressionValidator>
                        <asp:Label ID="lblCurrentPage" runat="server"></asp:Label>
                        <asp:Button ID="cmdFirst" runat="server" Text="|<" CssClass="button-search" />
                        <asp:Button ID="cmdPrev" runat="server" Text="<<" CssClass="button-search" />
                        <asp:TextBox ID="txtGoToPage" runat="server" MaxLength="4" Width="25" Height="15px"
                            Font-Size="Small" />
                        <asp:Button ID="cmdGo" runat="server" Text="Go" CssClass="button-search" ValidationGroup="vgTIList" />
                        <asp:Button ID="cmdNext" runat="server" Text=">>" CssClass="button-search" />
                        <asp:Button ID="cmdLast" runat="server" Text=">|" CssClass="button-search" />
                    </td>
                </tr>
                <tr>
                    <td colspan="8">
                        <asp:Repeater ID="rpVehicleVolume" runat="server">
                            <HeaderTemplate>
                                <tr>
                                    <td class="p_tablebackcolor">
                                        <asp:Label ID="Label4" runat="server">Sold To</asp:Label>
                                        &nbsp;
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:Label ID="lnkCustomer" runat="server">Customer</asp:Label>
                                        &nbsp;
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:Label ID="lnkProgram" runat="server">Program / Model / Platform / Assembly Plant</asp:Label>
                                        &nbsp;
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:Label ID="Label5" runat="server">Make</asp:Label>
                                        &nbsp;
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:Label ID="lnkYear" runat="server">Planning Year</asp:Label>
                                        &nbsp;
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:Label ID="lnkVolume" runat="server">Volume</asp:Label>
                                        &nbsp;
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:Label ID="Label2" runat="server">SOP</asp:Label>
                                        &nbsp;
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:Label ID="Label3" runat="server">EOP</asp:Label>
                                        &nbsp;
                                    </td>
                                    <td class="p_tablebackcolor">
                                        <asp:Label ID="Label1" runat="server">Last Update</asp:Label>
                                        &nbsp;
                                    </td>
                                </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:HyperLink ID="selectVehicleVolume" Font-Underline="true" runat="server" NavigateUrl='<%# "Vehicle_Volume.aspx?sPGMID=" & DataBinder.Eval (Container.DataItem,"ProgramID").tostring & "&sPlatID=" & DataBinder.Eval (Container.DataItem,"PlatformID").tostring  & "&sYear=" & DataBinder.Eval (Container.DataItem,"PlanningYear").tostring & "&sCABBV=" & Server.UrlEncode(DataBinder.Eval (Container.DataItem,"CABBV").tostring) & "&sSoldTo=" & Server.UrlEncode(DataBinder.Eval (Container.DataItem,"SoldTo").tostring)%>'
                                            Enabled='<%# SetClickable(Container.DataItem("vObsolete")).ToString %>'><%#DataBinder.Eval(Container, "DataItem.SoldTo")%></asp:HyperLink>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.Cust")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.ProgramName")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.Make")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.PlanningYear")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.AnnualVolume")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.SOP")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.EOP")%>
                                    </td>
                                    <td>
                                        <%#DataBinder.Eval(Container, "DataItem.UpdateInfo")%>
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
