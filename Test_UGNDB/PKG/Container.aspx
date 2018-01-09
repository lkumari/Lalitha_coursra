<%@ Page Title="Container" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="Container.aspx.vb" Inherits="Packaging_Container" EnableEventValidation="false" %>

<asp:Content ID="maincontent" runat="Server" ContentPlaceHolderID="maincontent">
    &nbsp;&nbsp;&nbsp;
    <asp:Panel ID="localPanel" runat="server" Height="1168px">
        <asp:Label ID="lblMessage" runat="server" ForeColor="#ff0000" SkinID="MessageLabelSkin"></asp:Label>
        <% If ViewState("pCNO") <> Nothing Then%>
        <table>
            <tr>
                <td class="p_smalltextbold" style="width: 532px; color: #990000">
                    Edit data below or press
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CausesValidation="False" />
                    to enter new data.<%--&nbsp; Press
                    <asp:Button ID="btnCopy" runat="server" Text="Copy" />
                    to carry data into a new part number(s).--%>
                </td>
            </tr>
        </table>
        <%  End If%>
        <hr />
        <br />
        <table>
            <tr>
                <td class="p_textbold">
                    <asp:Label ID="lblCNo" runat="server" Text="Containter No:" />
                </td>
                <td class="c_textbold">
                    <asp:Label ID="lblContainerNo" runat="server" Text="{Automated}" ForeColor="Red" />
                    <asp:Label ID="txtCID" runat="server" Visible="false" />
                    <asp:Label ID="txtOEMMfg" runat="server" Visible="false" />
                    <asp:TextBox ID="txtContainerNo" runat="server" MaxLength="11" Width="100" />
                    <ajax:FilteredTextBoxExtender ID="ftbContainerNo" runat="server" TargetControlID="txtContainerNo"
                        FilterType="Custom" ValidChars="abcdefghijklmnopqrstuvwxyz,ABCDEFGHIJKLMNOPQRSTUVWXYZ,1234567890,-" />
                    <asp:HiddenField ID="hfContainerNo" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="Label3" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                    <asp:Label ID="lblDescription" runat="server" Text="Description:" />
                </td>
                <td>
                    <asp:TextBox ID="txtDescription" runat="server" MaxLength="240" Width="400" />
                    <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription"
                        Enabled="True" ErrorMessage="Description is a required field." Font-Bold="False"
                        ValidationGroup="vsDetail"><</asp:RequiredFieldValidator><br />
                    <asp:Label ID="lblDescChar" runat="server" Font-Bold="True" ForeColor="Red" />
                </td>
            </tr>
        </table>
        <br />
        <%--Begin According View--%>
        <table>
            <tr>
                <td valign="top">
                    <ajax:Accordion ID="accDetail" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
                        HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
                        FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
                        RequireOpenedPane="false" SuppressHeaderPostbacks="true" Width="600px">
                        <Panes>
                            <ajax:AccordionPane ID="apDetail" runat="server">
                                <Header>
                                    <a href="">Container Detail</a></Header>
                                <Content>
                                    <table>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                                <asp:Label ID="lblType" runat="server" Text="Type:" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtType" runat="server" MaxLength="50" Width="250" />
                                                <asp:RequiredFieldValidator ID="rfvType" runat="server" ControlToValidate="txtType"
                                                    Enabled="True" ErrorMessage="Type is a required field." Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator><br />
                                                <asp:Label ID="lblTypeChar" runat="server" Font-Bold="True" ForeColor="Red" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                                <asp:Label ID="lblOEM" runat="server" EnableViewState="False" Text="OEM:" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddOEM" runat="server" />
                                                <asp:RequiredFieldValidator ID="rfvOEM" runat="server" ControlToValidate="ddOEM"
                                                    Enabled="True" ErrorMessage="OEM is a required field." Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                                                <ajax:CascadingDropDown ID="cddOEM" runat="server" TargetControlID="ddOEM" Category="OEM"
                                                    PromptText="Please select an OEM Code." LoadingText="[Loading OEM Code...]" ServicePath="~/WS/VehicleCDDService.asmx"
                                                    ServiceMethod="GetOEM" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label ID="Label8" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                                <asp:Label ID="lblOEMMfg" runat="server" EnableViewState="False" Text="OEM Manufacturer:" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddOEMMfg" runat="server" />
                                                <asp:RequiredFieldValidator ID="rfvOEMMfg" runat="server" ControlToValidate="ddOEMMfg"
                                                    Enabled="True" ErrorMessage="OEM Manufacturer is a required field." Font-Bold="False"
                                                    ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                                                <ajax:CascadingDropDown ID="cddOEMMfg" runat="server" TargetControlID="ddOEMMfg"
                                                    ParentControlID="ddOEM" Category="OEMMfg" PromptText="Please select an OEM Manufacturer."
                                                    LoadingText="[Loading OEM Manufacturer...]" ServicePath="~/WS/GeneralCDDService.asmx"
                                                    ServiceMethod="GetOEMMfgByOEM" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label ID="Label4" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                                <asp:Label ID="lblColor" runat="server" EnableViewState="False" Text="Color:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddColor" runat="server" />
                                                <asp:RequiredFieldValidator ID="rfvColor" runat="server" ControlToValidate="ddColor"
                                                    Enabled="True" ErrorMessage="Color is a required field." Font-Bold="False" ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td style="text-align: center;" class="c_textbold" colspan="3">
                                                                        <asp:Label ID="Label5" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                                                        Inner Dimenions
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="text-align: center; background-color: #CCCCCC" class="c_textbold">
                                                                        L
                                                                    </td>
                                                                    <td style="text-align: center; background-color: #CCCCCC" class="c_textbold">
                                                                        W
                                                                    </td>
                                                                    <td style="text-align: center; background-color: #CCCCCC" class="c_textbold">
                                                                        H
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:TextBox ID="txtInLength" runat="server" MaxLength="10" Width="80px" />
                                                                        <ajax:FilteredTextBoxExtender ID="ftInLength" runat="server" TargetControlID="txtInLength"
                                                                            FilterType="Custom" ValidChars="1234567890,-./ " />
                                                                        <asp:RequiredFieldValidator ID="rfvInLength" runat="server" ControlToValidate="txtInLength"
                                                                            Enabled="True" ErrorMessage="Inner Length is a required field." Font-Bold="False"
                                                                            ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtInWidth" runat="server" MaxLength="10" Width="80px" />
                                                                        <ajax:FilteredTextBoxExtender ID="ftbeInWidth" runat="server" TargetControlID="txtInWidth"
                                                                            FilterType="Custom" ValidChars="1234567890,-./ " />
                                                                        <asp:RequiredFieldValidator ID="rfvInWidth" runat="server" ControlToValidate="txtInWidth"
                                                                            Enabled="True" ErrorMessage="Inner Width is a required field." Font-Bold="False"
                                                                            ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtInHeight" runat="server" MaxLength="10" Width="80px" />
                                                                        <ajax:FilteredTextBoxExtender ID="ftbeInHeight" runat="server" TargetControlID="txtInHeight"
                                                                            FilterType="Custom" ValidChars="1234567890,-./ " />
                                                                        <asp:RequiredFieldValidator ID="rfvInHeight" runat="server" ControlToValidate="txtInHeight"
                                                                            Enabled="True" ErrorMessage="Inner Height is a required field." Font-Bold="False"
                                                                            ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td style="text-align: center;" class="c_textbold" colspan="3">
                                                                        <asp:Label ID="Label6" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                                                        Outer Dimenions
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="text-align: center; background-color: #CCCCCC" class="c_textbold">
                                                                        L
                                                                    </td>
                                                                    <td style="text-align: center; background-color: #CCCCCC" class="c_textbold">
                                                                        W
                                                                    </td>
                                                                    <td style="text-align: center; background-color: #CCCCCC" class="c_textbold">
                                                                        H
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:TextBox ID="txtOutLength" runat="server" MaxLength="10" Width="80px" />
                                                                        <ajax:FilteredTextBoxExtender ID="ftbeOutLength" runat="server" TargetControlID="txtOutLength"
                                                                            FilterType="Custom" ValidChars="1234567890,-./ " />
                                                                        <asp:RequiredFieldValidator ID="rfvOutLength" runat="server" ControlToValidate="txtOutLength"
                                                                            Enabled="True" ErrorMessage="Outer Length is a required field." Font-Bold="False"
                                                                            ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtOutWidth" runat="server" MaxLength="10" Width="80px" />
                                                                        <ajax:FilteredTextBoxExtender ID="ftbeOutWidth" runat="server" TargetControlID="txtOutWidth"
                                                                            FilterType="Custom" ValidChars="1234567890,-./ " />
                                                                        <asp:RequiredFieldValidator ID="rfvOutWidth" runat="server" ControlToValidate="txtOutWidth"
                                                                            Enabled="True" ErrorMessage="Outer Width is a required field." Font-Bold="False"
                                                                            ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtOutHeight" runat="server" MaxLength="10" Width="80px" />
                                                                        <ajax:FilteredTextBoxExtender ID="ftbeOutHeight" runat="server" TargetControlID="txtOutHeight"
                                                                            FilterType="Custom" ValidChars="1234567890,-./ " />
                                                                        <asp:RequiredFieldValidator ID="rfvOutHeight" runat="server" ControlToValidate="txtOutHeight"
                                                                            Enabled="True" ErrorMessage="Outer Height is a required field." Font-Bold="False"
                                                                            ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label ID="Label7" runat="server" Font-Bold="True" ForeColor="Red" Text="* " />
                                                <asp:Label ID="lblTareWeight" runat="server" Text="Tare Weight (lbs):" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTareWeight" runat="server" MaxLength="18" Width="80px" />
                                                <ajax:FilteredTextBoxExtender ID="ftbeTareWeight" runat="server" TargetControlID="txtTareWeight"
                                                    FilterType="Custom, Numbers" ValidChars="-." />
                                                <asp:RequiredFieldValidator ID="rfvTareWeight" runat="server" ControlToValidate="txtTareWeight"
                                                    Enabled="True" ErrorMessage="Tare Weight is a required field." Font-Bold="False"
                                                    ValidationGroup="vsDetail"><</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text" valign="top">
                                                <asp:Label ID="lblNotes" runat="server" Text="Notes:" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNotes" runat="server" MaxLength="200" Rows="3" TextMode="MultiLine"
                                                    Width="350px" /><br />
                                                <asp:Label ID="lblNotesChar" runat="server" Font-Bold="True" ForeColor="Red" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="p_text">
                                                <asp:Label ID="lblObsolete" runat="server" Text="Obsolete:" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddObsolete" runat="server">
                                                    <asp:ListItem Selected="True" Value="False">No</asp:ListItem>
                                                    <asp:ListItem Value="True">Yes</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnSave" runat="server" CausesValidation="True" Text="Save" ValidationGroup="vsDetail"
                                                    Width="80px" />
                                                <asp:Button ID="btnReset" runat="server" Text="Reset" Width="80px" />
                                                <asp:Button ID="btnDelete" runat="server" Text="Delete" Width="80px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:ValidationSummary ID="sDetail" runat="server" ShowMessageBox="True" ValidationGroup="vsDetail" />
                                            </td>
                                        </tr>
                                    </table>
                                </Content>
                            </ajax:AccordionPane>
                        </Panes>
                    </ajax:Accordion>
                </td>
                <td valign="top">
                    <ajax:Accordion ID="accSupplier" runat="server" SelectedIndex="0" HeaderCssClass="accordionHeader"
                        HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
                        FadeTransitions="false" FramesPerSecond="20" TransitionDuration="250" AutoSize="None"
                        RequireOpenedPane="false" SuppressHeaderPostbacks="true" Width="600px">
                        <Panes>
                            <ajax:AccordionPane ID="apSupplier" runat="server">
                                <Header>
                                    <a href="">Customer / Supplier Info</a></Header>
                                <Content>
                                    <!-- Customer Gridview -->
                                    <asp:GridView ID="gvCustomer" runat="server" SkinID="StandardGrid" AllowPaging="True"
                                        AllowSorting="True" AutoGenerateColumns="False" DataSourceID="odsCustomer" PageSize="30"
                                        DataKeyNames="CID,Customer" OnRowCommand="gvCustomer_RowCommand" ShowFooter="True"
                                        Width="500px">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Customer" SortExpression="Customer" HeaderStyle-HorizontalAlign="Left">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtEditCustomer" runat="server" MaxLength="240" Width="350px" Text='<%# Bind("Customer") %>' />
                                                    <asp:RequiredFieldValidator ID="rfvEdit" runat="server" ControlToValidate="txtEditCustomer"
                                                        ErrorMessage="Customer is a required field." Text="<" Font-Bold="True" ValidationGroup="vgEdit"> 
                                                    </asp:RequiredFieldValidator></EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="vCustomer" runat="server" CssClass="c_text" Text='<%# Bind("Customer") %>' />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <%--<asp:DropDownList ID="ddInsertCustomer" runat="server" DataSource='<%# commonFunctions.GetOEMMfgByOEM(ddOEM.SelectedValue) %>'
                                                        DataTextField="ddOEMDesc" DataValueField="OEMManufacturer" AppendDataBoundItems="True">
                                                        <asp:ListItem Text="" Value="0" Selected="False"></asp:ListItem>
                                                    </asp:DropDownList>--%>
                                                    <asp:TextBox ID="txtInsertCustomer" runat="server" MaxLength="240" Width="350px" />
                                                    <asp:RequiredFieldValidator ID="rfvInsert" runat="server" ControlToValidate="txtInsertCustomer"
                                                        ErrorMessage="Customer is a required field." Text="<" Font-Bold="True" ValidationGroup="vgInsert"> 
                                                    </asp:RequiredFieldValidator>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <EditItemTemplate>
                                                    <asp:ImageButton ID="iBtnUpdateCust" runat="server" CausesValidation="True" CommandName="Update"
                                                        ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEdit" /><asp:ImageButton
                                                            ID="iBtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                                            ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" /></EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="iBtnEditCust" runat="server" CausesValidation="False" CommandName="Edit"
                                                        ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                                                    <asp:ImageButton ID="ibtnDeleteCust" runat="server" CausesValidation="False" CommandName="Delete"
                                                        ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" /></ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgInsert"
                                                        runat="server" ID="iBtnSaveCust" ImageUrl="~/images/save.jpg" AlternateText="Insert" /><asp:ImageButton
                                                            ID="iBtnUndoCust" runat="server" CommandName="Undo" CausesValidation="false"
                                                            ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" /></FooterTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:ValidationSummary runat="server" ID="vsEdit" ValidationGroup="vgEdit" ShowMessageBox="true"
                                        ShowSummary="true" />
                                    <asp:ValidationSummary runat="server" ID="vsInsert" ValidationGroup="vgInsert" ShowMessageBox="true"
                                        ShowSummary="true" />
                                    <asp:ObjectDataSource ID="odsCustomer" runat="server" DeleteMethod="DeletePKGContainerCustomer"
                                        InsertMethod="InsertPKGContainerCustomer" OldValuesParameterFormatString="original_{0}"
                                        SelectMethod="GetPKGContainerCustomer" TypeName="PKGBLL" UpdateMethod="UpdatePKGContainerCustomer">
                                        <DeleteParameters>
                                            <asp:ControlParameter ControlID="txtCID" Name="CID" PropertyName="Text" Type="Int32" />
                                            <asp:Parameter Name="Customer" Type="String" />
                                            <asp:Parameter Name="original_CID" Type="Int32" />
                                            <asp:Parameter Name="original_Customer" Type="String" />
                                        </DeleteParameters>
                                        <UpdateParameters>
                                            <asp:Parameter Name="original_Customer" Type="String" />
                                            <asp:Parameter Name="original_CID" Type="Int32" />
                                            <asp:Parameter Name="Customer" Type="String" />
                                        </UpdateParameters>
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="txtCID" Name="CID" PropertyName="Text" Type="Int32" />
                                        </SelectParameters>
                                        <InsertParameters>
                                            <asp:ControlParameter ControlID="txtCID" Name="CID" PropertyName="Text" Type="Int32" />
                                            <asp:Parameter Name="Customer" Type="String" />
                                        </InsertParameters>
                                    </asp:ObjectDataSource>
                                    <br />
                                    <!-- Supplier -->
                                    <asp:GridView ID="gvSupplier" runat="server" SkinID="StandardGrid" AllowPaging="True"
                                        AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="CID,VendorNo" DataSourceID="odsSupplier"
                                        Width="500px">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Supplier" SortExpression="ddVendorName" HeaderStyle-HorizontalAlign="Left">
                                                <EditItemTemplate>
                                                    <asp:DropDownList ID="ddEditSupplier" runat="server" DataSource='<%# SUPModule.GetSupplierLookUp("", "","", "", "", 1) %>'
                                                        DataTextField="ddVendorName" AppendDataBoundItems="True" DataValueField="VendorNo"
                                                        SelectedValue='<%# Bind("VendorNo") %>'>
                                                        <asp:ListItem Text="" Value="" Selected="False" />
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvEditSupplier" runat="server" ControlToValidate="ddEditSupplier"
                                                        ValidationGroup="vgEditSup" ErrorMessage="Supplier is a required field." Text="<"
                                                        Font-Bold="True"> 
                                                    </asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="vddVendorName" runat="server" CssClass="c_text" Text='<%# Bind("ddVendorName") %>' />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:DropDownList ID="ddInsertSupplier" runat="server" DataSource='<%# SUPModule.GetSupplierLookUp("", "", "", "", "", 1) %>'
                                                        DataTextField="ddVendorName" DataValueField="VendorNo" AppendDataBoundItems="True">
                                                        <asp:ListItem Text="" Value="" Selected="False" />
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvInsertSup" runat="server" ControlToValidate="ddInsertSupplier"
                                                        ErrorMessage="Supplier is a required field." Text="<" Font-Bold="True" ValidationGroup="vgInsertSup"> 
                                                    </asp:RequiredFieldValidator>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <EditItemTemplate>
                                                    <asp:ImageButton ID="iBtnUpdateCust" runat="server" CausesValidation="True" CommandName="Update"
                                                        ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditSup" /><asp:ImageButton
                                                            ID="iBtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                                            ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" /></EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="iBtnEditCust" runat="server" CausesValidation="False" CommandName="Edit"
                                                        ImageUrl="~/images/edit.jpg" AlternateText="Edit" />
                                                    <asp:ImageButton ID="ibtnDeleteCust" runat="server" CausesValidation="False" CommandName="Delete"
                                                        ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" /></ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton CommandName="Insert" CausesValidation="true" ValidationGroup="vgInsertSup"
                                                        runat="server" ID="iBtnSaveCust" ImageUrl="~/images/save.jpg" AlternateText="Insert" /><asp:ImageButton
                                                            ID="iBtnUndoCust" runat="server" CommandName="Undo" CausesValidation="false"
                                                            ImageUrl="~/images/undo-gray.jpg" AlternateText="Undo" /></FooterTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:ValidationSummary runat="server" ID="vsEditSup" ValidationGroup="vgEditSup"
                                        ShowMessageBox="true" ShowSummary="true" />
                                    <asp:ValidationSummary runat="server" ID="vsInsertSup" ValidationGroup="vgInsertSup"
                                        ShowMessageBox="true" ShowSummary="true" />
                                    <asp:ObjectDataSource ID="odsSupplier" runat="server" DeleteMethod="DeletePKGContainerSupplier"
                                        InsertMethod="InsertPKGContainerSupplier" OldValuesParameterFormatString="original_{0}"
                                        SelectMethod="GetPKGContainerSupplier" TypeName="PKGBLL" UpdateMethod="UpdatePKGContainerSupplier">
                                        <DeleteParameters>
                                            <asp:ControlParameter ControlID="txtCID" Name="CID" PropertyName="Text" Type="Int32" />
                                            <asp:Parameter Name="VendorNo" Type="Int32" />
                                            <asp:Parameter Name="original_VendorNo" Type="Int32" />
                                            <asp:Parameter Name="original_CID" Type="Int32" />
                                        </DeleteParameters>
                                        <UpdateParameters>
                                            <asp:Parameter Name="original_VendorNo" Type="Int32" />
                                            <asp:Parameter Name="VendorNo" Type="Int32" />
                                            <asp:Parameter Name="original_CID" Type="Int32" />
                                        </UpdateParameters>
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="txtCID" Name="CID" PropertyName="Text" Type="Int32" />
                                            <asp:Parameter Name="VendorNo" Type="Int32" />
                                        </SelectParameters>
                                        <InsertParameters>
                                            <asp:ControlParameter ControlID="txtCID" Name="CID" PropertyName="Text" Type="Int32" />
                                            <asp:Parameter Name="VendorNo" Type="Int32" />
                                        </InsertParameters>
                                    </asp:ObjectDataSource>
                                </Content>
                            </ajax:AccordionPane>
                        </Panes>
                    </ajax:Accordion>
                </td>
            </tr>
        </table>
    </asp:Panel>
    --%>
</asp:Content>
