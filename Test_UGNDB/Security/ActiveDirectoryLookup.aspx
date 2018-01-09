<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ActiveDirectoryLookup.aspx.vb" 
    Inherits="Security_ActiveDirectoryLookup" Debug="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>UGN, Inc.</title>
    <script language="javascript" type="text/javascript">
       // Keep the popup in focus until it gets closed.
       // This method works when the document loses focus.
       // It does not work if a form field loses focus.
       function restoreFocus()
       {
          if (!document.hasFocus())
          {
             window.focus();
          }
       }
       onblur=restoreFocus;
    </script>
</head>
<body style="background-color: White;">
    <form id="form1" runat="server" style="background-color: White;">

        <br />
        <h1 style="text-align: center; background-color: White; ">
            Security - Lookup Active Directory Users
        </h1>
        <hr />
        <asp:Label ID="lblErrors" runat="server" SkinID="MessageLabelSkin"></asp:Label>
        <br />
        <table width="100%" style="background-color: White; ">
            <tr>
                <td align="right">
                    Last Name:&nbsp;<br />
                    <em><span style="color: green">(sn)&nbsp;</span></em>
                </td>
                <td>
                    <asp:TextBox ID="txtLname" runat="server" 
                        ToolTip="Search by Last Name (may use wildcard characters: % or *)" 
                        Width="176px">
                    </asp:TextBox>
                    <asp:RegularExpressionValidator ID="revLname" runat="server" 
                        ControlToValidate="txtLname"
                        ErrorMessage="Last Name must contain alpha characters. Wildcard characters may only be used at the beginning or end." 
                        ValidationExpression="^[\*%]?[a-zA-Z-'\s]{1,30}[\*%]?$"
                        ValidationGroup="vgSearch">
                        *
                    </asp:RegularExpressionValidator>
                </td>
                <td align="right">
                    First Name:&nbsp;<br />
                    <em><span style="color: green">(givenname)&nbsp;</span></em>
                </td>
                <td>
                    <asp:TextBox ID="txtFname" runat="server" 
                        ToolTip="Search for First Name (may use wildcard characters: * or %)" 
                        ValidationGroup="vbSearch">
                    </asp:TextBox>
                    <asp:RegularExpressionValidator ID="revFname" runat="server" 
                        ControlToValidate="txtFname"
                        ErrorMessage="First name must contain alpha characters. Wildcard characters may only be used at the beginning or end." 
                        ValidationExpression="^[\*%]?[a-zA-Z-'\s]{1,30}[\*%]?$"
                        ValidationGroup="vgSearch">
                        *
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                    Location:&nbsp;<br />
                    <em><span style="color: green">(l)&nbsp;</span></em>
                </td>
                <td>
                    <asp:DropDownList ID="ddlLocation" runat="server" 
                        DataSourceID="odsLocation" DataTextField="l" 
                        ToolTip="Search by work location" Width="184px">
                    </asp:DropDownList>
                </td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td></td>
                <td colspan="3">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                        ValidationGroup="vgSearch" Width="528px" ShowMessageBox="True" />
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" ToolTip="Search Active Directory" ValidationGroup="vgSearch" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" CausesValidation="False" ToolTip="Clear search fields, and get new list" />
                </td>
            </tr>
        </table>
        <asp:ObjectDataSource ID="odsLocation" runat="server" SelectMethod="GetADLocations"
            TypeName="ActiveDirectoryFunctions">
        </asp:ObjectDataSource>
        <hr />
        <br />
        <asp:GridView ID="gvUsers" runat="server" 
                      DataSourceID="odsUsers" ShowFooter="True" 
                      Width="744px" AllowSorting="True" PageSize="20">
            <FooterStyle BackColor="#CCCCCC"  Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <EmptyDataTemplate>No records found from the database.</EmptyDataTemplate>
            <PagerStyle BackColor="#990000" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#CCCCCC" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
           
            <Columns>
                 <asp:TemplateField ShowHeader="False">
                     <ItemStyle HorizontalAlign="Center" Wrap="False" />
                     <ItemTemplate>
                         <asp:ImageButton ID="ibtnSelectUser" runat="server" 
                             CommandName="Select"
                             ImageUrl="~/images/SelectUser.gif"
                             AlternateText="Send user data back to previous page" 
                             ToolTip="Send user data back to previous page" />
                     </ItemTemplate>
                </asp:TemplateField>
             </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsUsers" runat="server" SelectMethod="GetADUsers" TypeName="ActiveDirectoryFunctions">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtFname"  Name="fname"
                    PropertyName="Text" />
                <asp:ControlParameter ControlID="txtLname"  Name="lname"
                    PropertyName="Text" />
                <asp:ControlParameter ControlID="ddlLocation"  Name="location"
                    PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </form>
</body>
</html>
