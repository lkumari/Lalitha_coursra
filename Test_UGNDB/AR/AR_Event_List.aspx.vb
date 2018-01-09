' ************************************************************************************************
'
' Name:		AR_Event_List.aspx
' Purpose:	This Code Behind is for the main page of the Accounts Receivable Tracking application
'
' Date		    Author	    
' 02/25/2010    Roderick Carlson - Created
' 08/24/2011    Roderick Carlson - Modified - added KeyNewPrice and UGN Facility to search results
' 08/20/2012    Roderick Carlson - Modified - added filter to search by SoldTo and ALL CABBVs associated
' 08/21/2012    Roderick Carlson - Modified - added filter to show or hide voided events
' 12/20/2013    LRey    Increased MaxLength to 40 for Part Number. Modified the bug found in the Approval list update BLL. Replace SoldTo|CABBV with Customer.
' ************************************************************************************************
Partial Class AR_Event_List
    Inherits System.Web.UI.Page

    Protected WithEvents lnkEventStatusName As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkAREID As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkEventType As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkCustApprvEffDate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkKeyField As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkKeyNewPrice As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkKeyUGNFacility As System.Web.UI.WebControls.LinkButton


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Search for AR Event"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Accounts Receivable Tracking </b> > AR Event Search "
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim MasterPanel As CollapsiblePanelExtender
            MasterPanel = CType(Master.FindControl("ARExtender"), CollapsiblePanelExtender)
            MasterPanel.Collapsed = False

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            If HttpContext.Current.Session("sessionARCurrentPage") IsNot Nothing Then
                CurrentPage = HttpContext.Current.Session("sessionARCurrentPage")
            End If

            If Not Page.IsPostBack Then

                CheckRights()

                'clear crystal reports
                ARGroupModule.CleanARCrystalReports()

                ViewState("lnkEventStatusName") = "ASC"
                ViewState("lnkAREID") = "DESC"
                ViewState("lnkEventType") = "ASC"
                ViewState("lnkEventDesc") = "ASC"
                ViewState("lnkCustApprvEffDate") = "ASC"
                ViewState("lnkKeyField") = "ASC"
                ViewState("lnkKeyNewPrice") = "ASC"
                ViewState("lnkKeyUGNFacility") = "ASC"

                ViewState("EventStatusID") = 0
                ViewState("AREID") = ""
                ViewState("EventDesc") = ""
                ViewState("EventTypeID") = 0
                ViewState("AcctMgrTMID") = 0
                ViewState("FilterCustomerApproved") = 0
                ViewState("isCustomerApproved") = 0
                ViewState("CustApprvEffDate") = ""
                ViewState("CustApprvEndDate") = ""
                ViewState("InvoiceNo") = ""
                ViewState("UGNFacility") = ""
                ViewState("CustomerValue") = ""
                ViewState("Customer") = ""
                ViewState("PartNo") = ""
                ViewState("PriceCode") = ""
                ViewState("PartName") = ""
                ViewState("ShowVoid") = 0

                ''******
                '' Bind drop down lists
                ''******
                BindCriteria()

                ''******
                'get saved value of past search criteria or query string, query string takes precedence
                ''******

                If HttpContext.Current.Request.QueryString("AREID") <> "" Then
                    ViewState("AREID") = HttpContext.Current.Request.QueryString("AREID")
                    If ViewState("AREID") <> "" And ViewState("AREID") <> "0" Then
                        txtAREID.Text = HttpContext.Current.Request.QueryString("AREID")
                    End If
                Else
                    If Not Request.Cookies("ARGroupModule_SaveAREIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("ARGroupModule_SaveAREIDSearch").Value) <> "" Then
                            ViewState("AREID") = Request.Cookies("ARGroupModule_SaveAREIDSearch").Value
                            If ViewState("AREID") <> "" And ViewState("AREID") <> "0" Then
                                txtAREID.Text = Request.Cookies("ARGroupModule_SaveAREIDSearch").Value
                            End If
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("EventStatusID") <> "" Then
                    ddEventStatus.SelectedValue = HttpContext.Current.Request.QueryString("EventStatusID")
                    ViewState("EventStatusID") = HttpContext.Current.Request.QueryString("EventStatusID")
                Else
                    If Not Request.Cookies("ARGroupModule_SaveEventStatusIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("ARGroupModule_SaveEventStatusIDSearch").Value) <> "" Then
                            ddEventStatus.SelectedValue = Request.Cookies("ARGroupModule_SaveEventStatusIDSearch").Value
                            ViewState("EventStatusID") = Request.Cookies("ARGroupModule_SaveEventStatusIDSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("ShowVoid") <> "" Then
                    cbShowVoid.Checked = CType(HttpContext.Current.Request.QueryString("ShowVoid"), Integer)
                    ViewState("ShowVoid") = CType(HttpContext.Current.Request.QueryString("ShowVoid"), Integer)
                Else
                    If Not Request.Cookies("ARGroupModule_SaveShowVoidSearch") Is Nothing Then
                        If Trim(Request.Cookies("ARGroupModule_SaveShowVoidSearch").Value) <> "" Then
                            cbShowVoid.Checked = CType(Request.Cookies("ARGroupModule_SaveShowVoidSearch").Value, Integer)
                            ViewState("ShowVoid") = CType(Request.Cookies("ARGroupModule_SaveShowVoidSearch").Value, Integer)
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("EventDesc") <> "" Then
                    ViewState("EventDesc") = HttpContext.Current.Request.QueryString("EventDesc")
                    txtEventDesc.Text = HttpContext.Current.Request.QueryString("EventDesc")
                Else
                    If Not Request.Cookies("ARGroupModule_SaveEventDescSearch") Is Nothing Then
                        If Trim(Request.Cookies("ARGroupModule_SaveEventDescSearch").Value) <> "" Then
                            ViewState("EventDesc") = Request.Cookies("ARGroupModule_SaveEventDescSearch").Value
                            txtEventDesc.Text = Request.Cookies("ARGroupModule_SaveEventDescSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("EventTypeID") <> "" Then
                    ddEventType.SelectedValue = HttpContext.Current.Request.QueryString("EventTypeID")
                    ViewState("EventTypeID") = HttpContext.Current.Request.QueryString("EventTypeID")
                Else
                    If Not Request.Cookies("ARGroupModule_SaveEventTypeIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("ARGroupModule_SaveEventTypeIDSearch").Value) <> "" Then
                            ddEventType.SelectedValue = Request.Cookies("ARGroupModule_SaveEventTypeIDSearch").Value
                            ViewState("EventTypeID") = Request.Cookies("ARGroupModule_SaveEventTypeIDSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("AcctMgrTMID") <> "" Then
                    ddAccountManager.SelectedValue = HttpContext.Current.Request.QueryString("AcctMgrTMID")
                    ViewState("AcctMgrTMID") = HttpContext.Current.Request.QueryString("AcctMgrTMID")
                Else
                    If Not Request.Cookies("ARGroupModule_SaveAcctMgrTMIDSearch") Is Nothing Then
                        If Trim(Request.Cookies("ARGroupModule_SaveAcctMgrTMIDSearch").Value) <> "" Then
                            ddAccountManager.SelectedValue = Request.Cookies("ARGroupModule_SaveAcctMgrTMIDSearch").Value
                            ViewState("AcctMgrTMID") = Request.Cookies("ARGroupModule_SaveAcctMgrTMIDSearch").Value
                        End If
                    End If
                End If

                ViewState("FilterCustomerApproved") = 0
                ViewState("isCustomerApproved") = 0
                ddCustomerApproved.SelectedIndex = -1
                If HttpContext.Current.Request.QueryString("FilterCustomerApproved") <> "" Then
                    If CType(HttpContext.Current.Request.QueryString("FilterCustomerApproved"), Integer) = 1 Then
                        ViewState("FilterCustomerApproved") = 1
                        ViewState("isCustomerApproved") = CType(HttpContext.Current.Request.QueryString("isCustomerApproved"), Integer)
                        ddCustomerApproved.SelectedValue = CType(HttpContext.Current.Request.QueryString("isCustomerApproved"), Integer)
                    End If
                Else
                    If Request.Cookies("ARGroupModule_SaveFilterCustomerApproved") IsNot Nothing Then
                        If Request.Cookies("ARGroupModule_SaveIsCustomerApproved") IsNot Nothing Then
                            If Request.Cookies("ARGroupModule_SaveFilterCustomerApproved").Value <> "" Then
                                If CType(Request.Cookies("ARGroupModule_SaveFilterCustomerApproved").Value, Integer) = 1 Then
                                    ViewState("FilterCustomerApproved") = 1
                                    ViewState("isCustomerApproved") = CType(Request.Cookies("ARGroupModule_SaveIsCustomerApproved").Value, Integer)
                                    ddCustomerApproved.SelectedValue = CType(Request.Cookies("ARGroupModule_SaveIsCustomerApproved").Value, Integer)
                                End If
                            End If
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("CustApprvEffDate") <> "" Then
                    txtCustApprvEffDate.Text = HttpContext.Current.Request.QueryString("CustApprvEffDate")
                    ViewState("CustApprvEffDate") = HttpContext.Current.Request.QueryString("CustApprvEffDate")
                Else
                    If Not Request.Cookies("ARGroupModule_SaveCustApprvEffDateSearch") Is Nothing Then
                        If Trim(Request.Cookies("ARGroupModule_SaveCustApprvEffDateSearch").Value) <> "" Then
                            txtCustApprvEffDate.Text = Request.Cookies("ARGroupModule_SaveCustApprvEffDateSearch").Value
                            ViewState("CustApprvEffDate") = Request.Cookies("ARGroupModule_SaveCustApprvEffDateSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("CustApprvEndDate") <> "" Then
                    txtCustApprvEndDate.Text = HttpContext.Current.Request.QueryString("CustApprvEndDate")
                    ViewState("CustApprvEndDate") = HttpContext.Current.Request.QueryString("CustApprvEndDate")
                Else
                    If Not Request.Cookies("ARGroupModule_SaveCustApprvEndDateSearch") Is Nothing Then
                        If Trim(Request.Cookies("ARGroupModule_SaveCustApprvEndDateSearch").Value) <> "" Then
                            txtCustApprvEndDate.Text = Request.Cookies("ARGroupModule_SaveCustApprvEndDateSearch").Value
                            ViewState("CustApprvEndDate") = Request.Cookies("ARGroupModule_SaveCustApprvEndDateSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("UGNFacility") <> "" Then
                    ddUGNFacility.SelectedValue = HttpContext.Current.Request.QueryString("UGNFacility")
                    ViewState("UGNFacility") = HttpContext.Current.Request.QueryString("UGNFacility")
                Else
                    If Not Request.Cookies("ARGroupModule_SaveUGNFacilitySearch") Is Nothing Then
                        If Trim(Request.Cookies("ARGroupModule_SaveUGNFacilitySearch").Value) <> "" Then
                            ddUGNFacility.SelectedValue = Request.Cookies("ARGroupModule_SaveUGNFacilitySearch").Value
                            ViewState("UGNFacility") = Request.Cookies("ARGroupModule_SaveUGNFacilitySearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("CustomerValue") <> "" Then
                    ddCustomer.SelectedValue = HttpContext.Current.Request.QueryString("CustomerValue")
                    ViewState("CustomerValue") = HttpContext.Current.Request.QueryString("CustomerValue")
                    ViewState("Customer") = HttpContext.Current.Request.QueryString("CustomerValue")
                Else
                    If Not Request.Cookies("ARGroupModule_SaveCustomerSearch") Is Nothing Then
                        If Trim(Request.Cookies("ARGroupModule_SaveCustomerSearch").Value) <> "" Then
                            ddCustomer.SelectedValue = Request.Cookies("ARGroupModule_SaveCustomerSearch").Value
                            ViewState("CustomerValue") = Request.Cookies("ARGroupModule_SaveCustomerSearch").Value
                            ViewState("Customer") = Request.Cookies("ARGroupModule_SaveCustomerSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("PartNo") <> "" Then
                    txtPartNo.Text = HttpContext.Current.Request.QueryString("PartNo")
                    ViewState("PartNo") = HttpContext.Current.Request.QueryString("PartNo")
                Else
                    If Not Request.Cookies("ARGroupModule_SavePartNoSearch") Is Nothing Then
                        If Trim(Request.Cookies("ARGroupModule_SavePartNoSearch").Value) <> "" Then
                            txtPartNo.Text = Request.Cookies("ARGroupModule_SavePartNoSearch").Value
                            ViewState("PartNo") = Request.Cookies("ARGroupModule_SavePartNoSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("PriceCode") <> "" Then
                    ddPriceCode.SelectedValue = HttpContext.Current.Request.QueryString("PriceCode")
                    ViewState("PriceCode") = HttpContext.Current.Request.QueryString("PriceCode")
                Else
                    If Not Request.Cookies("ARGroupModule_SavePriceCodeSearch") Is Nothing Then
                        If Trim(Request.Cookies("ARGroupModule_SavePriceCodeSearch").Value) <> "" Then
                            ddPriceCode.SelectedValue = Request.Cookies("ARGroupModule_SavePriceCodeSearch").Value
                            ViewState("PriceCode") = Request.Cookies("ARGroupModule_SavePriceCodeSearch").Value
                        End If
                    End If
                End If

                If HttpContext.Current.Request.QueryString("PartName") <> "" Then
                    txtPartName.Text = HttpContext.Current.Request.QueryString("PartName")
                    ViewState("PartName") = HttpContext.Current.Request.QueryString("PartName")
                Else
                    If Not Request.Cookies("ARGroupModule_SavePartNameSearch") Is Nothing Then
                        If Trim(Request.Cookies("ARGroupModule_SavePartNameSearch").Value) <> "" Then
                            txtPartName.Text = Request.Cookies("ARGroupModule_SavePartNameSearch").Value
                            ViewState("PartName") = Request.Cookies("ARGroupModule_SavePartNameSearch").Value
                        End If
                    End If
                End If

                'load repeater control
                BindData()

                EnableControls()
            Else

                ViewState("AREID") = txtAREID.Text.Trim

                If ddEventStatus.SelectedIndex > 0 Then
                    ViewState("EventStatusID") = ddEventStatus.SelectedValue
                Else
                    ViewState("EventStatusID") = 0
                End If

                ViewState("EventDesc") = txtEventDesc.Text.Trim

                If ddEventType.SelectedIndex > 0 Then
                    ViewState("EventTypeID") = ddEventType.SelectedValue
                Else
                    ViewState("EventTypeID") = 0
                End If

                If ddAccountManager.SelectedIndex > 0 Then
                    ViewState("AcctMgrTMID") = ddAccountManager.SelectedValue
                Else
                    ViewState("AcctMgrTMID") = 0
                End If

                ViewState("FilterCustomerApproved") = 0
                ViewState("isCustomerApproved") = 0
                If ddCustomerApproved.SelectedIndex > 0 Then
                    ViewState("FilterCustomerApproved") = 1
                    ViewState("isCustomerApproved") = ddCustomerApproved.SelectedValue
                End If

                ViewState("CustApprvEffDate") = txtCustApprvEffDate.Text
                ViewState("CustApprvEndDate") = txtCustApprvEndDate.Text


                If ddUGNFacility.SelectedIndex > 0 Then
                    ViewState("UGNFacility") = ddUGNFacility.SelectedValue
                Else
                    ViewState("UGNFacility") = ""
                End If

                If ddCustomer.SelectedIndex > 0 Then
                    ViewState("CustomerValue") = ddCustomer.SelectedValue
                    ViewState("Customer") = ViewState("CustomerValue")
                Else
                    ViewState("CustomerValue") = ""
                    ViewState("Customer") = ""
                End If

                ViewState("PartNo") = Replace(txtPartNo.Text, "'", "")

                If ddPriceCode.SelectedIndex > 0 Then
                    ViewState("PriceCode") = ddPriceCode.SelectedValue
                Else
                    ViewState("PriceCode") = ""
                End If

                ViewState("PartName") = txtPartName.Text.Trim

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click

        Try

            Response.Redirect("AR_Event_Detail.aspx", False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            lblMessage.Text = ""

            ViewState("PartName") = ""

            HttpContext.Current.Session("sessionARCurrentPage") = Nothing

            'set saved value of what criteria was used to search        
            Response.Cookies("ARGroupModule_SaveAREIDSearch").Value = txtAREID.Text.Trim

            If ddEventStatus.SelectedIndex > 0 Then
                Response.Cookies("ARGroupModule_SaveEventStatusIDSearch").Value = ddEventStatus.SelectedValue
            Else
                Response.Cookies("ARGroupModule_SaveEventStatusIDSearch").Value = 0
                Response.Cookies("ARGroupModule_SaveEventStatusIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If cbShowVoid.Checked = True Then
                Response.Cookies("ARGroupModule_SaveShowVoidSearch").Value = 1
            Else
                Response.Cookies("ARGroupModule_SaveShowVoidSearch").Value = 0
                Response.Cookies("ARGroupModule_SaveShowVoidSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            Response.Cookies("ARGroupModule_SaveEventDescSearch").Value = txtEventDesc.Text.Trim

            If ddEventType.SelectedIndex > 0 Then
                Response.Cookies("ARGroupModule_SaveEventTypeIDSearch").Value = ddEventType.SelectedValue
            Else
                Response.Cookies("ARGroupModule_SaveEventTypeIDSearch").Value = 0
                Response.Cookies("ARGroupModule_SaveEventTypeIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddAccountManager.SelectedIndex > 0 Then
                Response.Cookies("ARGroupModule_SaveAcctMgrTMIDSearch").Value = ddAccountManager.SelectedValue
            Else
                Response.Cookies("ARGroupModule_SaveAcctMgrTMIDSearch").Value = 0
                Response.Cookies("ARGroupModule_SaveAcctMgrTMIDSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            Response.Cookies("ARGroupModule_SaveFilterCustomerApproved").Value = 0
            Response.Cookies("ARGroupModule_SaveIsCustomerApproved").Value = 0
            If ddCustomerApproved.SelectedIndex > 0 Then
                Response.Cookies("ARGroupModule_SaveFilterCustomerApproved").Value = 1
                Response.Cookies("ARGroupModule_SaveIsCustomerApproved").Value = ddCustomerApproved.SelectedValue
            End If

            Response.Cookies("ARGroupModule_SaveCustApprvEffDateSearch").Value = txtCustApprvEffDate.Text.Trim
            Response.Cookies("ARGroupModule_SaveCustApprvEndDateSearch").Value = txtCustApprvEndDate.Text.Trim

            If ddUGNFacility.SelectedIndex > 0 Then
                Response.Cookies("ARGroupModule_SaveUGNFacilitySearch").Value = ddUGNFacility.SelectedValue
            Else
                Response.Cookies("ARGroupModule_SaveUGNFacilitySearch").Value = ""
                Response.Cookies("ARGroupModule_SaveUGNFacilitySearch").Expires = DateTime.Now.AddDays(-1)
            End If

            If ddCustomer.SelectedIndex > 0 Then
                Response.Cookies("ARGroupModule_SaveCustomerSearch").Value = ddCustomer.SelectedValue
            Else
                Response.Cookies("ARGroupModule_SaveCustomerSearch").Value = ""
                Response.Cookies("ARGroupModule_SaveCustomerSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            Response.Cookies("ARGroupModule_SavePartNoSearch").Value = Replace(txtPartNo.Text.Trim, "'", "")

            If ddPriceCode.SelectedIndex > 0 Then
                Response.Cookies("ARGroupModule_SavePriceCodeSearch").Value = ddPriceCode.SelectedValue
            Else
                Response.Cookies("ARGroupModule_SavePriceCodeSearch").Value = ""
                Response.Cookies("ARGroupModule_SavePriceCodeSearch").Expires = DateTime.Now.AddDays(-1)
            End If

            Response.Cookies("ARGroupModule_SavePartNameSearch").Value = Replace(txtPartName.Text.Trim, "'", "")

            Response.Redirect("AR_Event_List.aspx?AREID=" & ViewState("AREID") _
            & "&EventStatusID=" & ViewState("EventStatusID") _
            & "&EventDesc=" & ViewState("EventDesc") _
            & "&EventTypeID=" & ViewState("EventTypeID") _
            & "&AcctMgrTMID=" & ViewState("AcctMgrTMID") _
            & "&FilterCustomerApproved=" & ViewState("FilterCustomerApproved") _
            & "&isCustomerApproved=" & ViewState("isCustomerApproved") _
            & "&CustApprvEffDate=" & ViewState("CustApprvEffDate") _
            & "&CustApprvEndDate=" & ViewState("CustApprvEndDate") _
            & "&InvoiceNo=" & ViewState("InvoiceNo") _
            & "&UGNFacility=" & ViewState("UGNFacility") _
            & "&CustomerValue=" & ViewState("CustomerValue") _
            & "&PartNo=" & ViewState("PartNo") _
            & "&PriceCode=" & ViewState("PriceCode") _
            & "&PartName=" & ViewState("PartName") _
            & "&ShowVoid=" & CType(cbShowVoid.Checked, Integer), False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        Try

            ARGroupModule.DeleteARCookies()
            HttpContext.Current.Session("sessionARCurrentPage") = Nothing

            Response.Redirect("AR_Event_List.aspx", False)

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cmdPrev_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrev.Click, cmdPrevBottom.Click

        Try
            ' Set viewstate variable to the previous page
            CurrentPage -= 1
            HttpContext.Current.Session("sessionARCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click, cmdNextBottom.Click

        Try
            ' Set viewstate variable to the next page
            CurrentPage += 1
            HttpContext.Current.Session("sessionARCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cmdFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFirst.Click, cmdFirstBottom.Click

        Try
            ' Set viewstate variable to the first page
            CurrentPage = 0
            HttpContext.Current.Session("sessionARCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cmdGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdGo.Click

        Try
            If txtGoToPage.Text.Trim <> "" Then
                txtGoToPageBottom.Text = txtGoToPage.Text

                ' Set viewstate variable to the specific page
                If CType(txtGoToPage.Text.Trim, Integer) > ViewState("LastPageCount") Then
                    CurrentPage = ViewState("LastPageCount")
                Else
                    CurrentPage = txtGoToPage.Text - 1
                End If


                HttpContext.Current.Session("sessionARCurrentPage") = CurrentPage

                ' Reload control
                BindData()
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cmdGoBottom_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdGoBottom.Click

        Try
            If txtGoToPageBottom.Text.Trim <> "" Then
                txtGoToPage.Text = txtGoToPageBottom.Text

                ' Set viewstate variable to the specific page
                If CType(txtGoToPageBottom.Text.Trim, Integer) > ViewState("LastPageCount") Then
                    CurrentPage = ViewState("LastPageCount")
                Else
                    CurrentPage = txtGoToPageBottom.Text - 1
                End If

                HttpContext.Current.Session("sessionARCurrentPage") = CurrentPage

                ' Reload control
                BindData()
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub cmdLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLast.Click, cmdLastBottom.Click

        Try
            ' Set viewstate variable to the last page
            CurrentPage = ViewState("LastPageCount")
            HttpContext.Current.Session("sessionARCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Function SetApprovalItemDisplay(ByVal StatusID As String) As String

        Dim strReturnValue As String = "none"

        Try
            If CType(StatusID, Integer) > 0 Then
                strReturnValue = ""
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetApprovalItemDisplay = strReturnValue

    End Function

    Protected Function SetApprovalRowDisplay(ByVal EventStatusID As String) As String

        Dim strReturnValue As String = "none"

        Try
            'In-Process (Pending Accountant Event Approval) OR In-Process (Pending Deduction Form Approval)
            If CType(EventStatusID, Integer) = 2 Or CType(EventStatusID, Integer) = 5 Then
                strReturnValue = ""
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetApprovalRowDisplay = strReturnValue

    End Function

    Protected Function SetApprovalVisible(ByVal StatusID As String) As Boolean

        Dim bReturnValue As Boolean = False

        Try
            If CType(StatusID, Integer) > 0 Then
                bReturnValue = True
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetApprovalVisible = bReturnValue

    End Function

    Protected Function SetEventBackGroundColor(ByVal StatusID As String) As String

        Dim strReturnValue As String = "White" 'N/A or 9-Closed 

        Try
            Select Case StatusID
                Case "1" 'open
                    strReturnValue = "Fuchsia"
                Case "2", "3", "4", "5", "6" 'in-process
                    strReturnValue = "Yellow"
                Case "7", "8" 'rejected
                    strReturnValue = "Red"
                Case "10" 'void
                    strReturnValue = "Gray"
            End Select

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetEventBackGroundColor = strReturnValue

    End Function

    Protected Function SetApprovalBackGroundColor(ByVal StatusID As String) As String

        Dim strReturnValue As String = "White" 'N/A or 4-Approved 

        Try
            Select Case StatusID
                Case "1" 'open
                    strReturnValue = "Fuchsia"
                Case "2" 'in-process
                    strReturnValue = "Yellow"
                Case "3" 'rejected
                    strReturnValue = "Red"
            End Select

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetApprovalBackGroundColor = strReturnValue

    End Function

    Protected Function SetEventForeGroundColor(ByVal StatusID As String) As String

        Dim strReturnValue As String = "Black"

        Try
            Select Case StatusID
                'Case "1", "7", "8", "10"  'rejected and void
                Case "7", "8", "10"  'rejected and void
                    strReturnValue = "White"
            End Select

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetEventForeGroundColor = strReturnValue

    End Function

    Protected Function SetApprovalForeGroundColor(ByVal StatusID As String) As String

        Dim strReturnValue As String = "Black" 'default

        Try
            Select Case StatusID
                'Case "1", "3" 'open, 'rejected
                Case "3" 'rejected
                    strReturnValue = "White"
            End Select

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetApprovalForeGroundColor = strReturnValue

    End Function
    Protected Function SetPreviewHyperLink(ByVal AREID As String) As String

        Dim strReturnValue As String = ""

        Try
            If AREID <> "" Then
                strReturnValue = "javascript:void(window.open('crPreview_AR_Event_Detail.aspx?AREID=" & AREID & "'," & Now.Ticks.ToString & ",'top=5,left=5,height=700,width=1090,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));"
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetPreviewHyperLink = strReturnValue

    End Function

    Protected Function SetPreviewVisible(ByVal EventStatusID As String) As Boolean

        Dim bReturnValue As Boolean = False

        Try
            If EventStatusID <> "10" Then
                bReturnValue = True
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        SetPreviewVisible = bReturnValue

    End Function

    Protected Sub CheckRights()

        Try
            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim dsSubscription As DataSet

            ViewState("SubscriptionID") = 0
            ViewState("isAdmin") = False
            ViewState("TeamMemberID") = 0

            Dim iTeamMemberID As Integer = 0
            Dim iRoleID As Integer = 0


            'dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Jim.Meade", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataset(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                ''test developer as another team member
                If iTeamMemberID = 530 Then
                    'mike echevarria
                    iTeamMemberID = 246

                    'Brett.Barta 
                    'iTeamMemberID = 2

                    ' ''gina lacny
                    'iTeamMemberID = 627

                    ' ''gary hibbler
                    'iTeamMemberID = 671

                    'Ilysa.Albright 
                    'iTeamMemberID = 636

                    'Kara.North 
                    'iTeamMemberID = 667

                    'Kelly.Carolyn 
                    'iTeamMemberID = 638

                    'Jeffrey.Kist 
                    'iTeamMemberID = 718

                    'Paul Papke
                    'iTeamMemberID = 510

                    'Julie.Sinchak()
                    'iTeamMemberID = 303
                End If

                ViewState("TeamMemberID") = iTeamMemberID

                'Sales
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 9)
                If commonFunctions.CheckDataset(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 9
                End If

                'Accounting
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 21)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 21
                End If

                'VP of  Sales
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 23)
                If commonFunctions.CheckDataset(dsSubscription) = True Then                   
                    ViewState("SubscriptionID") = 23                
                End If

                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 49)

                If commonFunctions.CheckDataset(dsRoleForm) = True Then

                    iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                    Select Case iRoleID
                        Case 11 '*** UGNAdmin: Full Access
                            ViewState("isAdmin") = True
                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                            ViewState("isAdmin") = True
                        Case 13 '*** UGNAssist: Create/Edit/No Delete

                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only

                        Case 15 '*** UGNEdit: No Create/Edit/No Delete

                        Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)

                    End Select                
                End If

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Sub SortByColumn(ByVal SortOrder As String)

        Try
            Dim ds As DataSet

            ds = ARGroupModule.GetAREventSearch(ViewState("AREID"), ViewState("EventStatusID"), ViewState("EventDesc"), ViewState("EventTypeID"), ViewState("AcctMgrTMID"), ViewState("FilterCustomerApproved"), ViewState("isCustomerApproved"), ViewState("CustApprvEffDate"), ViewState("CustApprvEndDate"), ViewState("UGNFacility"), ViewState("Customer"), ViewState("PartNo"), ViewState("PriceCode"), ViewState("PartName"), ViewState("ShowVoid"))

            If commonFunctions.CheckDataSet(ds) = True Then

                ' Create a DataView from the DataTable.
                Dim dv As DataView = New DataView(ds.Tables(0))

                'Enforce the sort on the dataview
                dv.Sort = SortOrder

                'Set the DataGrid's Source and bind it.
                rpSearchResult.DataSource = dv
                rpSearchResult.DataBind()

                'Dispose Items
                ds.Dispose()
                dv.Dispose()
            Else
                cmdFirst.Enabled = False
                cmdGo.Enabled = False
                cmdPrev.Enabled = False
                cmdNext.Enabled = False
                cmdLast.Enabled = False

                cmdFirstBottom.Enabled = False
                cmdGoBottom.Enabled = False
                cmdPrevBottom.Enabled = False
                cmdNextBottom.Enabled = False
                cmdLastBottom.Enabled = False

                rpSearchResult.Visible = False

                txtGoToPage.Visible = False
                txtGoToPageBottom.Visible = False
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Public Sub SortCommand(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles lnkEventStatusName.Click, lnkAREID.Click, lnkEventType.Click, lnkCustApprvEffDate.Click, lnkKeyField.Click, lnkKeyNewPrice.Click, lnkKeyUGNFacility.Click

        Try
            Dim lnkButton As New LinkButton
            lnkButton = CType(sender, LinkButton)
            Dim order As String = lnkButton.CommandArgument
            Dim sortType As String = ViewState(lnkButton.ID)

            SortByColumn(order + " " + sortType)
            If sortType = "ASC" Then
                'if column set to ascending sort, change to descending for next click on that column
                lnkButton.CommandName = "DESC"
                ViewState(lnkButton.ID) = "DESC"
            Else
                lnkButton.CommandName = "ASC"
                ViewState(lnkButton.ID) = "ASC"
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Public Property CurrentPage() As Integer

        Get
            ' look for current page in ViewState
            Dim o As Object = ViewState("_CurrentPage")
            If (o Is Nothing) Then
                Return 0 ' default page index of 0
            Else
                Return o
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("_CurrentPage") = value
        End Set

    End Property

    Private Sub BindData()

        Try

            Dim ds As DataSet

            'bind existing AR Event data to repeater control at bottom of screen                       
            ds = ARGroupModule.GetAREventSearch(ViewState("AREID"), ViewState("EventStatusID"), ViewState("EventDesc"), _
            ViewState("EventTypeID"), ViewState("AcctMgrTMID"), _
            ViewState("FilterCustomerApproved"), ViewState("isCustomerApproved"), ViewState("CustApprvEffDate"), ViewState("CustApprvEndDate"), _
            ViewState("UGNFacility"), _
            ViewState("Customer"), ViewState("PartNo"), _
            ViewState("PriceCode"), ViewState("PartName"), _
            ViewState("ShowVoid"))

            If commonFunctions.CheckDataSet(ds) = True Then

                rpSearchResult.DataSource = ds
                rpSearchResult.DataBind()

                ' Populate the repeater control with the Items DataSet
                Dim objPds As PagedDataSource = New PagedDataSource
                objPds.DataSource = ds.Tables(0).DefaultView

                ' Indicate that the data should be paged
                objPds.AllowPaging = True

                ' Set the number of items you wish to display per page
                objPds.PageSize = 30

                ' Set the PagedDataSource's current page
                objPds.CurrentPageIndex = CurrentPage

                rpSearchResult.Visible = True
                rpSearchResult.DataSource = objPds
                rpSearchResult.DataBind()

                '' Disable Prev or Next buttons if necessary            
                lblCurrentPage.Text = "Page: " + (CurrentPage + 1).ToString() + " of " + objPds.PageCount.ToString()
                lblCurrentPageBottom.Text = "Page: " + (CurrentPage + 1).ToString() + " of " + objPds.PageCount.ToString()

                ViewState("LastPageCount") = objPds.PageCount - 1

                txtGoToPage.Visible = True
                txtGoToPageBottom.Visible = True
                txtGoToPage.Text = CurrentPage + 1
                txtGoToPageBottom.Text = CurrentPage + 1

                ' Disable Prev or Next buttons if necessary
                cmdFirst.Enabled = Not objPds.IsFirstPage
                cmdFirstBottom.Enabled = Not objPds.IsFirstPage

                'cmdGo.Enabled = Not objPds.IsFirstPage
                'cmdGoBottom.Enabled = Not objPds.IsFirstPage

                cmdGo.Enabled = True
                cmdGoBottom.Enabled = cmdGo.Enabled

                cmdPrev.Enabled = Not objPds.IsFirstPage
                cmdPrevBottom.Enabled = Not objPds.IsFirstPage

                cmdNext.Enabled = Not objPds.IsLastPage
                cmdNextBottom.Enabled = Not objPds.IsLastPage

                cmdLast.Enabled = Not objPds.IsLastPage
                cmdLastBottom.Enabled = Not objPds.IsLastPage

                ' Display # of records
                If (CurrentPage + 1) > 1 Then
                    lblFromRec.Text = (((CurrentPage + 1) * objPds.PageSize) - objPds.PageSize) + 1
                    lblToRec.Text = (CurrentPage + 1) * objPds.PageSize
                    If lblToRec.Text > objPds.DataSourceCount Then
                        lblToRec.Text = objPds.DataSourceCount
                    End If
                Else
                    lblFromRec.Text = ds.Tables.Count
                    lblToRec.Text = rpSearchResult.Items.Count
                End If
                lblTotalRecords.Text = objPds.DataSourceCount
            Else
                cmdFirst.Enabled = False
                cmdGo.Enabled = False
                cmdPrev.Enabled = False
                cmdNext.Enabled = False
                cmdLast.Enabled = False

                cmdFirstBottom.Enabled = False
                cmdGoBottom.Enabled = False
                cmdPrevBottom.Enabled = False
                cmdNextBottom.Enabled = False
                cmdLastBottom.Enabled = False

                rpSearchResult.Visible = False

                txtGoToPage.Visible = False
                txtGoToPageBottom.Visible = False
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub EnableControls()

        Try
            btnAdd.Enabled = False

            Select Case CType(ViewState("SubscriptionID"), Integer)
                Case 9, 21, 23
                    btnAdd.Enabled = ViewState("isAdmin")
            End Select

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Private Sub BindCriteria()

        Try
            'bind existing data to drop down controls for selection criteria for search       

            Dim ds As DataSet

            ds = ARGroupModule.GetAREventStatusList()
            If commonFunctions.CheckDataSet(ds) = True Then
                ddEventStatus.DataSource = ds
                ddEventStatus.DataTextField = ds.Tables(0).Columns("ddStatusName").ColumnName.ToString()
                ddEventStatus.DataValueField = ds.Tables(0).Columns("StatusID").ColumnName
                ddEventStatus.DataBind()
                ddEventStatus.Items.Insert(0, "")
            End If

            ds = ARGroupModule.GetAREventTypeList(False)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddEventType.DataSource = ds
                ddEventType.DataTextField = ds.Tables(0).Columns("ddEventTypeName").ColumnName.ToString()
                ddEventType.DataValueField = ds.Tables(0).Columns("EventTypeID").ColumnName
                ddEventType.DataBind()
                ddEventType.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetOEMManufacturer("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddCustomer.DataSource = ds
                ddCustomer.DataTextField = ds.Tables(0).Columns("ddOEMManufacturer").ColumnName.ToString()
                ddCustomer.DataValueField = ds.Tables(0).Columns("OEMManufacturer").ColumnName.ToString()
                ddCustomer.DataBind()
                ddCustomer.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetPriceCode("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddPriceCode.DataSource = ds
                ddPriceCode.DataTextField = ds.Tables(0).Columns("ddPriceCodeName").ColumnName.ToString()
                ddPriceCode.DataValueField = ds.Tables(0).Columns("PriceCode").ColumnName
                ddPriceCode.DataBind()
                ddPriceCode.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetUGNFacility("")
            If commonFunctions.CheckDataSet(ds) = True Then
                ddUGNFacility.DataSource = ds
                ddUGNFacility.DataTextField = ds.Tables(0).Columns("ddUGNFacilityName").ColumnName.ToString()
                ddUGNFacility.DataValueField = ds.Tables(0).Columns("UGNFacility").ColumnName
                ddUGNFacility.DataBind()
                ddUGNFacility.Items.Insert(0, "")
            End If

            ds = commonFunctions.GetTeamMemberBySubscription(9)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddAccountManager.DataSource = ds
                ddAccountManager.DataTextField = ds.Tables(0).Columns("TMName").ColumnName
                ddAccountManager.DataValueField = ds.Tables(0).Columns("TMID").ColumnName
                ddAccountManager.DataBind()
                ddAccountManager.Items.Insert(0, "")
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

End Class
