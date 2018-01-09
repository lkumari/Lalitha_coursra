<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="MaterialSpecDetail.aspx.vb" Inherits="MaterialSpecDetail" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <asp:Panel ID="localPanel" runat="server">
        <asp:Label ID="lblMessage" SkinID="MessageLabelSkin" runat="server"></asp:Label>
        <asp:ValidationSummary ID="vsSave" runat="server" DisplayMode="List" ShowMessageBox="true"
            ShowSummary="true" EnableClientScript="true" ValidationGroup="vgSave" />
        <hr />
        <table width="98%">
            <tr>
                <td>
                    All Revisions of this type of materials specification:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddMaterialSpecNo" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="p_textbold">
                    Material Specification No.:
                </td>
                <td>
                    <asp:Label runat="server" ID="lblMaterialSpecNo"></asp:Label>
                </td>
                <td class="p_textbold">
                    Revision Date:
                </td>
                <td>
                    <asp:Label runat="server" ID="lblRevisionDate"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblSubFamilyMarker" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="*" />
                    Sub-Family:
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddSubFamily" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="reqSubFamily" CssClass="p_text" runat="server" Display="Dynamic"
                        ControlToValidate="ddSubFamily" ValidationGroup="vgSave" ErrorMessage="SubFamily is required."
                        Text="<" SetFocusOnError="true"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="p_text">
                    <asp:Label ID="lblInitialAreaWeightMarker" runat="server" Font-Bold="True" ForeColor="Red"
                        Text="*" />
                    <asp:Label ID="lblInitialAreaWeight" runat="server" Text="Initial Area Weight:"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtInitialAreaWeight" runat="server" MaxLength="4" Width="50px">0000</asp:TextBox>
                    &nbsp;<i>**Inserted into the Material Spec No.</i>
                    <asp:RequiredFieldValidator ID="reqInitialAreaWeight" runat="server" ControlToValidate="txtInitialAreaWeight"
                        Text="<" ErrorMessage="Initial area weight us required." SetFocusOnError="true"
                        ValidationGroup="vgSave">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="p_textbold">
                    Description:
                </td>
                <td colspan="3">
                    <asp:TextBox runat="server" ID="txtMaterialSpecDesc" TextMode="MultiLine" Width="600px"
                        Height="80px"></asp:TextBox>
                    <br />
                    <asp:Label ID="lblMaterialSpecDescCharCount" SkinID="MessageLabelSkin" runat="server"></asp:Label>
                </td>
            </tr>
            <tr align="center">
                <td colspan="4">
                    <asp:Button runat="server" ID="btnCreateNew" Text="Create New" Visible="false" />
                    <asp:Button runat="server" ID="btnUpdate" Text="Update" Visible="false" />
                    <asp:Button runat="server" ID="btnCreateRevision" Text="Create Revision" Visible="false" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <hr />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Label runat="server" ID="lblMessageSupportingDocs" SkinID="MessageLabelSkin"></asp:Label>
                    <br />
                    <asp:ValidationSummary ID="vsSupportingDocs" runat="server" DisplayMode="List" ShowMessageBox="true"
                        ShowSummary="true" ValidationGroup="vgSupportingDocs" />
                </td>
            </tr>
            <tr>
                <td class="p_textbold" valign="top">
                    File Description:
                </td>
                <td class="c_text" colspan="3">
                    <asp:TextBox ID="txtSupportingDocDesc" runat="server" MaxLength="200" Rows="3" TextMode="MultiLine"
                        Width="600px" />
                    <asp:RequiredFieldValidator ID="rfvSupportingDocDesc" runat="server" ControlToValidate="txtSupportingDocDesc"
                        ErrorMessage="Supporting Document File Description is a required field." Font-Bold="False"
                        ValidationGroup="vgSupportingDocs"><</asp:RequiredFieldValidator><br />
                    <br />
                    <asp:Label ID="lblSupportingDocDescCharCount" SkinID="MessageLabelSkin" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="p_textbold" style="white-space: nowrap;" align="right">
                    <asp:Label runat="server" ID="lblFileUploadLabel" Text="Upload a supporting (PDF,DOC,DOCX,XLS,XLSX,JPEG,TIF) file under 3 MB:"
                        Visible="false"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:FileUpload runat="server" ID="fileUploadSupportingDoc" Width="334px" Visible="False" />
                    <asp:Button ID="btnSaveUploadSupportingDocument" runat="server" Text="Upload" Visible="False"
                        Width="67px" CausesValidation="true" ValidationGroup="vgSupportingDocs"></asp:Button>
                    <br /> 
                    <asp:RegularExpressionValidator ID="revUploadFile" runat="server" ErrorMessage="Please upload only *.PDF, *.DOC,*.DOCX, *.XLS,*.XLSX, *.JPEG, *.JPG, *.TIF files are allowed."
                        ValidationExpression="(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.pdf|.xls|.xlsx|.doc|.docx|.jpeg|.jpg|.tif|.PDF|.XLS|.XLSX|.DOC|.DOCX|.JPEG|.JPG|.TIF)$"
                        ControlToValidate="fileUploadSupportingDoc" ValidationGroup="vgSupportingDocs"
                        Font-Bold="True" Font-Size="Small" /><br />
                </td>
            </tr>
        </table>
        <br />
        <asp:GridView ID="gvSupportingDoc" runat="server" AutoGenerateColumns="False" AllowSorting="True"
            AllowPaging="True" PageSize="15" DataKeyNames="RowID" DataSourceID="odsSupportingDoc"
            EmptyDataText="No supporting documents exist yet." Width="98%">
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:BoundField DataField="RowID">
                    <ItemStyle CssClass="none" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Supporting Document Name" SortExpression="SupportingDocName">
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkViewSupportingDoc" runat="server" NavigateUrl='<%# Eval("RowID", "~/PE/DrawingMaterialSpecSupportingDocView.aspx?RowID={0}") %>'
                            Target="_blank" Text='<%# Eval("SupportingDocName") %>'>
                        </asp:HyperLink>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:BoundField HeaderText="Desciption" DataField="SupportingDocDesc" HeaderStyle-HorizontalAlign="Left">
                </asp:BoundField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="iBtnSupportingDocDelete" runat="server" CausesValidation="False"
                            CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsSupportingDoc" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetDrawingMaterialSpecSupportingDoc" TypeName="DrawingMaterialSpecSupportingDocBLL"
            DeleteMethod="DeleteDrawingMaterialSpecSupportingDoc">
            <SelectParameters>
                <asp:ControlParameter ControlID="lblMaterialSpecNo" Name="MaterialSpecNo" PropertyName="Text"
                    Type="String" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="RowID" Type="Int32" />
                <asp:Parameter Name="original_RowID" Type="Int32" />
            </DeleteParameters>
        </asp:ObjectDataSource>
        <br />
        <asp:label runat="server" ID="lblDrawingMaterialRelateTitle" Text="Drawings associated to this material specification" CssClass="p_bigtextbold" Visible="false"></asp:label>
        <asp:Label ID="lblMessageDrawingMaterialRelate" SkinID="MessageLabelSkin" runat="server"></asp:Label>
        <asp:ValidationSummary ID="vsEditDrawingMaterialSpecRelate" runat="server" ShowMessageBox="True"
            ShowSummary="true" ValidationGroup="vgEditDrawingMaterialSpecRelate" />
        <asp:ValidationSummary ID="vsInsertDrawingMaterialSpecRelate" runat="server" ShowMessageBox="True"
            ShowSummary="true" ValidationGroup="vgInsertDrawingMaterialSpecRelate" />
        <asp:GridView ID="gvDrawingMaterialSpecRelate" runat="server" AutoGenerateColumns="False"
            AllowSorting="True" AllowPaging="True" PageSize="15" DataKeyNames="RowID" DataSourceID="odsDrawingMaterialSpecRelate"
            EmptyDataText="No DMS Drawings relate to this Material Specification yet." ShowFooter="True"
            Width="98%">
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:BoundField DataField="RowID">
                    <ItemStyle CssClass="none" />
                </asp:BoundField>               
                <asp:TemplateField ShowHeader="False">
                    <FooterTemplate>
                        <asp:ImageButton ID="iBtnSearchDrawingNo" runat="server" CausesValidation="False"
                            ImageUrl="~/images/Search.gif" ToolTip="Search for drawing number" AlternateText="Search DrawingNo" />
                    </FooterTemplate>
                    <FooterStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="DrawingNo" SortExpression="DrawingNo">
                    <EditItemTemplate>
                        <asp:Label runat="server" ID="lblEditDrawingNo" Text='<%# Bind("DrawingNo") %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:HyperLink ID="lnkViewDrawngNo" runat="server" NavigateUrl='<%# Eval("DrawingNo", "~/PE/DrawingDetail.aspx?DrawingNo={0}") %>'
                            Target="_blank" Text='<%# Eval("DrawingNo") %>'>
                        </asp:HyperLink>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtInsertDrawingNo" runat="server" MaxLength="18"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvInsertDrawingNo" runat="server" ControlToValidate="txtInsertDrawingNo"
                            ErrorMessage="DrawingNo is required." Font-Bold="True" ValidationGroup="vgInsertDrawingMaterialSpecRelate"
                            Text="<" SetFocusOnError="true">				                                                            
                        </asp:RequiredFieldValidator>
                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Notes" SortExpression="DrawingMaterialSpecNotes">
                    <EditItemTemplate>
                        <asp:TextBox runat="server" ID="txtEditDrawingMaterialSpecNotes" Text='<%# Bind("DrawingMaterialSpecNotes") %>' MaxLength="100"
                            Width="300px"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblViewDrawingMaterialSpecNotes" Text='<%# Bind("DrawingMaterialSpecNotes") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtInsertDrawingMaterialSpecNotes" runat="server" MaxLength="100"
                            Width="300px"></asp:TextBox>
                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <EditItemTemplate>
                        <asp:ImageButton ID="iBtnDrawingMaterialSpecRelateUpdate" runat="server" CausesValidation="False"
                            CommandName="Update" ImageUrl="~/images/save.jpg" AlternateText="Update" ValidationGroup="vgEditDrawingMaterialSpecRelate" />&nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="iBtnDrawingMaterialSpecRelateCancel" runat="server" CausesValidation="False"
                            CommandName="Cancel" ImageUrl="~/images/cancel.jpg" AlternateText="Cancel" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:ImageButton ID="iBtnDrawingMaterialSpecRelateEdit" runat="server" CausesValidation="False"
                            CommandName="Edit" ImageUrl="~/images/edit.jpg" AlternateText="Edit" />&nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="iBtnDrawingMaterialSpecRelateDelete" runat="server" CausesValidation="False"
                            CommandName="Delete" ImageUrl="~/images/delete.jpg" AlternateText="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:ImageButton ID="iBtnDrawingMaterialSpecRelateInsert" runat="server" CausesValidation="True"
                            CommandName="Insert" ImageUrl="~/images/save.jpg" AlternateText="Insert" ValidationGroup="vgInsertDrawingMaterialSpecRelate" />&nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="ibtnUndo" runat="server" CommandName="Undo" ImageUrl="~/images/undo-gray.jpg"
                            AlternateText="Undo" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsDrawingMaterialSpecRelate" runat="server" DeleteMethod="DeleteDrawingMaterialSpecRelate"
            InsertMethod="InsertDrawingMaterialSpecRelate" SelectMethod="GetDrawingMaterialSpecRelateByMaterialSpecNo"
            TypeName="DrawingMaterialSpecRelateBLL" UpdateMethod="UpdateDrawingMaterialSpecRelate"
            OldValuesParameterFormatString="original_{0}">
            <DeleteParameters>
                <asp:Parameter Name="original_RowID" Type="Int32" />                         
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="original_RowID" Type="Int32" />
                <asp:ControlParameter ControlID="lblMaterialSpecNo" Name="MaterialSpecNo" PropertyName="Text"
                    Type="String" />
                <asp:Parameter Name="DrawingNo" Type="String" />
                <asp:Parameter Name="DrawingMaterialSpecNotes" Type="String" />
                <asp:Parameter Name="RowID" Type="Int32" />
            </UpdateParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="lblMaterialSpecNo" Name="MaterialSpecNo" PropertyName="Text"
                    Type="String" />
            </SelectParameters>
            <InsertParameters>
                <asp:ControlParameter ControlID="lblMaterialSpecNo" Name="MaterialSpecNo" PropertyName="Text"
                    Type="String" />
                <asp:Parameter Name="DrawingNo" Type="String" />
                <asp:Parameter Name="DrawingMaterialSpecNotes" Type="String" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</asp:Content>
