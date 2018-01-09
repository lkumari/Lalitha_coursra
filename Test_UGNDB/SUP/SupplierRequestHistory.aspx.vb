' ************************************************************************************************
' Name:	SupplierRequestHistory.aspx.vb
' Purpose:	This program is used to bind data and execute bind data to repeater row commands.
'
' Date		    Author	    
' 10/04/2010    LRey			Created .Net application
' ************************************************************************************************
Partial Class SUP_SupplierRequestHistory
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ''****************************************************
            '' Update the title and heading on the Master Page
            ''****************************************************
            Dim m As ASP.masterpage_master = DirectCast(Page.Master, ASP.masterpage_master)
            m.PageTitle = "UGN, Inc."

            m.ContentLabel = "Supplier Request History"

            ''**************************************************
            '' Override the Master Page bread crumb navigation
            ''**************************************************
            Dim ctl As Control = m.FindControl("lblOtherSiteNode")
            If ctl IsNot Nothing Then
                Dim lbl As Label = CType(ctl, Label)
                lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Purchasing</b> > <a href='SupplierRequestList.aspx'><b>Supplier Request Search</b></a> > History"
                lbl.Visible = True
            End If

            ctl = m.FindControl("SiteMapPath1")
            If ctl IsNot Nothing Then
                Dim smp As SiteMapPath = CType(ctl, SiteMapPath)
                smp.Visible = False
            End If

            ''******************************************
            '' Expand this Master Page menu item
            ''******************************************
            ctl = m.FindControl("PURExtender")
            If ctl IsNot Nothing Then
                Dim cpe As CollapsiblePanelExtender = CType(ctl, CollapsiblePanelExtender)
                cpe.Collapsed = False
            End If


            ''*******
            '' Initialize ViewState
            ''*******
            If HttpContext.Current.Request.QueryString("pSUPNo") <> "" Then
                ViewState("pSUPNo") = HttpContext.Current.Request.QueryString("pSUPNo")
            Else
                ViewState("pSUPNo") = ""
            End If
            If HttpContext.Current.Session("sessionSupCurrentPage") IsNot Nothing Then
                'CurrentPage = HttpContext.Current.Session("sessionSupCurrentPage")
            End If

            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then
                ''******
                '' Bind drop down lists
                ''******
                BindData()
            End If

        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim myMethod As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(myMethod.DeclaringType.Name & _
                "." & myMethod.Name & "(): " & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF Page_Load
    Private Sub BindData()

        Try
            lblErrors.Text = ""

            Dim ds As DataSet = New DataSet

            'bind existing AR Event data to repeater control at bottom of screen                       
            ds = SUPModule.GetSupplierRequestHistory(ViewState("pSUPNo"))

            If ds IsNot Nothing Then
                If ds.Tables.Count > 0 Then
                    lblSUPNo.Text = ds.Tables(0).Rows(0).Item("SUPNo").ToString()
                    lblVendorName.Text = ds.Tables(0).Rows(0).Item("VendorName").ToString()

                    rpSupplierRequest.DataSource = ds
                    rpSupplierRequest.DataBind()

                    ' Populate the repeater control with the Items DataSet
                    Dim objPds As PagedDataSource = New PagedDataSource
                    objPds.DataSource = ds.Tables(0).DefaultView

                    ' Indicate that the data should be paged
                    objPds.AllowPaging = True

                    ' Set the number of items you wish to display per page
                    objPds.PageSize = 50

                    ' Set the PagedDataSource's current page
                    objPds.CurrentPageIndex = CurrentPage

                    rpSupplierRequest.DataSource = objPds
                    rpSupplierRequest.DataBind()

                    lblCurrentPage.Text = "Page: " + (CurrentPage + 1).ToString() + " of " + objPds.PageCount.ToString()
                    ViewState("LastPageCount") = objPds.PageCount - 1
                    txtGoToPage.Text = CurrentPage + 1

                    ' Disable Prev or Next buttons if necessary
                    cmdFirst.Enabled = Not objPds.IsFirstPage
                    cmdPrev.Enabled = Not objPds.IsFirstPage
                    cmdNext.Enabled = Not objPds.IsLastPage
                    cmdLast.Enabled = Not objPds.IsLastPage
                End If
            End If
        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message
            lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF of BindData
#Region "Paging Routine"
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

    End Property 'EOF CurrentPage

    Protected Sub cmdPrev_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPrev.Click

        Try
            ' Set viewstate variable to the previous page
            CurrentPage -= 1
            HttpContext.Current.Session("sessionExpCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF cmdPrev_Click

    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click

        Try
            ' Set viewstate variable to the next page
            CurrentPage += 1
            HttpContext.Current.Session("sessionExpCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF cmdNext_Click

    Protected Sub cmdFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFirst.Click

        Try
            ' Set viewstate variable to the first page
            CurrentPage = 0
            HttpContext.Current.Session("sessionExpCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try


    End Sub 'EOF cmdFirst_Click

    Protected Sub cmdGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdGo.Click

        Try
            If txtGoToPage.Text.Length > 0 Then

                ' Set viewstate variable to the specific page
                If txtGoToPage.Text > ViewState("LastPageCount") Then
                    CurrentPage = ViewState("LastPageCount")
                Else
                    CurrentPage = txtGoToPage.Text - 1
                End If


                HttpContext.Current.Session("sessionExpCurrentPage") = CurrentPage

                ' Reload control
                BindData()
            End If
        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF cmdGo_Click

    Protected Sub cmdLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLast.Click

        Try
            ' Set viewstate variable to the last page
            CurrentPage = ViewState("LastPageCount")
            HttpContext.Current.Session("sessionExpCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF cmdLast_Click

#End Region 'EOF Paging Routine

    Public Function GoToCommBoard(ByVal SUPNo As String, ByVal ActionDesc As String) As String
        If ActionDesc = "Message Sent" Then
            Return "SupplierRequest.aspx?pSUPNo=" & SUPNo & "&pRC=1"
        Else
            Return ""
        End If
    End Function 'EOF GoToCommBoard

    Public Function ShowHideLink(ByVal ActionDesc As String) As Boolean
        If ActionDesc = "Message Sent" Then
            Return True
        Else
            Return False
        End If
    End Function 'EOF ShowHideLink
End Class
