' ************************************************************************************************
' Name:	Manufacturing_Metric_History.aspx.vb
' Purpose:	This program is used to bind data and execute bind data to repeater row commands.
'
' Date		    Author	    
' 07/01/2010    Roderick Carlson			Created .Net application
' ************************************************************************************************

Partial Class PlantSpecificReports_Manufacturing_Metric_History
    Inherits System.Web.UI.Page

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            ''****************************************************
            '' Update the title and heading on the Master Page
            ''****************************************************
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Manufacturing Metric Monthly Data History"

            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("MPRExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub EnableControls()

        cmdFirst.Visible = False
        cmdPrev.Visible = False
        cmdNext.Visible = False
        cmdLast.Visible = False
        txtGoToPage.Visible = False
        cmdGo.Visible = False

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            ''*******
            '' Initialize ViewState
            ''*******
            ViewState("ReportID") = 0
            If HttpContext.Current.Request.QueryString("ReportID") <> "" Then
                ViewState("ReportID") = HttpContext.Current.Request.QueryString("ReportID")
            End If

            'ViewState("YearID") = 0
            'If HttpContext.Current.Request.QueryString("YearID") <> "" Then
            '    ViewState("YearID") = HttpContext.Current.Request.QueryString("YearID")
            'End If

            'ViewState("MonthName") = ""
            'If HttpContext.Current.Request.QueryString("MonthName") <> "" Then
            '    ViewState("MonthName") = HttpContext.Current.Request.QueryString("MonthName")
            'End If

            'ViewState("UGNFacilityName") = ""
            'If HttpContext.Current.Request.QueryString("UGNFacilityName") <> "" Then
            '    ViewState("UGNFacilityName") = HttpContext.Current.Request.QueryString("UGNFacilityName")
            'End If

            If HttpContext.Current.Session("sessionMMHisotryCurrentPage") IsNot Nothing Then
                CurrentPage = HttpContext.Current.Session("sessionMMHisotryCurrentPage")
            End If

            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then

                EnableControls()

                If ViewState("ReportID") > 0 Then
                    ''******
                    '' Bind drop down lists
                    ''******
                    BindData()

                    'lblMonth.Text = ViewState("MonthName")
                    'lblYear.Text = ViewState("YearID")
                    'lblUGNFacility.Text = ViewState("UGNFacilityName")

                End If

            End If

            'normally, this would all be handled in the Init function but since there is a need to check the AR EventID against the deduction table, it needs to be handled here.
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b> Plant Specific Reports </b> > <a href='Manufacturing_Metric_List.aspx'><b> Manufacturing Metric Monthly Report List </b></a> > <a href='Manufacturing_Metric_Detail.aspx?ReportID=" & ViewState("ReportID") & " '><b> Manufacturing Metric Monthly Report Detail </b></a> > Manufacturing Metric Monthly Report History "

                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF Page_Load

    Private Sub BindData()

        Try
            Dim ds As DataSet
            Dim dsHeader As DataSet

            'bind existing AR Event data to repeater control at bottom of screen                       
            ds = PSRModule.GetManufacturingMetricHistory(ViewState("ReportID"))

            If commonFunctions.CheckDataset(ds) = True Then

                rpHistory.DataSource = ds
                rpHistory.DataBind()

                ' Populate the repeater control with the Items DataSet
                Dim objPds As PagedDataSource = New PagedDataSource
                objPds.DataSource = ds.Tables(0).DefaultView

                ' Indicate that the data should be paged
                objPds.AllowPaging = True

                ' Set the number of items you wish to display per page
                objPds.PageSize = 50

                ' Set the PagedDataSource's current page
                objPds.CurrentPageIndex = CurrentPage

                rpHistory.DataSource = objPds
                rpHistory.DataBind()

                lblCurrentPage.Text = "Page: " + (CurrentPage + 1).ToString() + " of " + objPds.PageCount.ToString()
                ViewState("LastPageCount") = objPds.PageCount - 1
                txtGoToPage.Text = CurrentPage + 1

                ' Disable Prev or Next buttons if necessary
                cmdFirst.Enabled = Not objPds.IsFirstPage
                cmdPrev.Enabled = Not objPds.IsFirstPage
                cmdNext.Enabled = Not objPds.IsLastPage
                cmdLast.Enabled = Not objPds.IsLastPage

                cmdFirst.Visible = True
                cmdPrev.Visible = True
                cmdNext.Visible = True
                cmdLast.Visible = True
                txtGoToPage.Visible = True
                cmdGo.Visible = True

                dsHeader = PSRModule.GetManufacturingMetricHeader(ViewState("ReportID"))
                If commonFunctions.CheckDataSet(dsHeader) = True Then
                    lblMonth.Text = dsHeader.Tables(0).Rows(0).Item("ddMonthName").ToString
                    lblYear.Text = dsHeader.Tables(0).Rows(0).Item("YearID").ToString
                    lblUGNFacility.Text = dsHeader.Tables(0).Rows(0).Item("ddUGNFacilityName").ToString
                End If

            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
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
            HttpContext.Current.Session("sessionMMHistoryCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF cmdPrev_Click

    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click

        Try
            ' Set viewstate variable to the next page
            CurrentPage += 1
            HttpContext.Current.Session("sessionMMHistoryCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF cmdNext_Click

    Protected Sub cmdFirst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFirst.Click

        Try
            ' Set viewstate variable to the first page
            CurrentPage = 0
            HttpContext.Current.Session("sessionMMHistoryCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
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


                HttpContext.Current.Session("sessionMMHistoryCurrentPage") = CurrentPage

                ' Reload control
                BindData()
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF cmdGo_Click

    Protected Sub cmdLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdLast.Click

        Try
            ' Set viewstate variable to the last page
            CurrentPage = ViewState("LastPageCount")
            HttpContext.Current.Session("sessionMMHistoryCurrentPage") = CurrentPage

            ' Reload control
            BindData()
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF cmdLast_Click

#End Region 'EOF Paging Routine
End Class
