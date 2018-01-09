Option Explicit On
Option Strict On

''*****************************************************************************
'' Name    : TestIssuanceNew.aspx.vb
'' Purpose : Initiates the creation of a new Test Issuance Request.
''           The user must choose a test type. 
'' 
'' Date        Author     Modifications
'' ----        ------     -------------
'' 12/10/2008  MWeyker    Created ASP.NET page in Visual Studio 2005
''                               
''*****************************************************************************

Partial Class RnD_TestIssuanceNew
    Inherits System.Web.UI.Page

#Region "Page Event Handlers"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If (Not Page.IsPostBack) Then
                ''********
                '' This page initiates entry of a new Test Issuance Request.
                '' If the user does not have an "Insert" type role,
                '' redirect back to the List page.
                ''********
                ' ''Dim blnHasInsertRole As Boolean = RnDModule.HasRole( _
                ' ''    RnDModule.ScreenType.TestIssuance, RnDModule.RoleType.Add)
                ' ''If (blnHasInsertRole = False) Then
                ' ''    'Response.Redirect("TestIssuanceList.aspx", False)
                ' ''End If

                ''********
                '' Initialize the Master Page
                ''********
                InitializeMasterPage()

                ''********
                '' If new Test Request is being copied from another,
                '' display that RequestID and RequestCategory.
                ''********
                'Dim intCopyFromId As Integer = QueryString_CopyId
                Dim intCopyFromId As Integer = RnDModule.QueryStringValue(RnDModule.QueryStringParam.pCopyId)
                If (intCopyFromId > 0) Then
                    pnlCopyFromMessage.Visible = True
                    lblCopyFromNo.Text = intCopyFromId.ToString
                    ''********
                    '' Select the Request Category of "copy from"
                    ''********
                    'Dim intRequestCategory As Integer = QueryString_RequestCategory
                    Dim intRequestCategory As Integer = RnDModule.QueryStringValue(RnDModule.QueryStringParam.pReqCategory)
                    Select Case intRequestCategory
                        Case 1
                            opt1.Checked = True
                        Case 2
                            opt2.Checked = True
                        Case 3
                            opt3.Checked = True
                        Case 4
                            opt4.Checked = True
                        Case Else
                            ' DO NOTHING
                    End Select
                Else
                    pnlCopyFromMessage.Visible = False
                End If
            End If
        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim myMethod As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(myMethod.DeclaringType.Name & _
                "." & myMethod.Name & "(): " & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
#End Region ' Page Event Handlers


#Region "Control Event Handlers"
    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            ''********
            '' The Add button initiates a new test request.
            '' Find out which test type was chosen.
            ''********
            Dim intRequestCategory As Integer = 0
            lblValidation.Text = ""
            If (opt1.Checked = True) Then
                intRequestCategory = 1              'Product Innovation
            End If
            If (opt2.Checked = True) Then
                intRequestCategory = 2              'Current Mass Production Part
            End If
            If (opt3.Checked = True) Then
                intRequestCategory = 3              'Consultation
            End If
            If (opt4.Checked = True) Then
                intRequestCategory = 4              'New Request Category
            End If


            If intRequestCategory = 0 Then
                ''********
                '' No test type was chosen.
                '' Display an error message
                ''********
                lblValidation.Text = "* Please select a Request Category"
            Else
                ''********
                '' Redirect to the Detail page, with these query string settings:
                ''    RequestId=0              (start new request)
                ''    RequestCategory=integer  (for this test type)
                ''    CopyId                   (RequestId to copy)
                ''********
                Response.Redirect("TestIssuanceDetail.aspx?" & _
                    RnDModule.QueryStringParam.pReqCategory.ToString & "=" & intRequestCategory.ToString, False)

            End If
        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim myMethod As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(myMethod.DeclaringType.Name & _
                "." & myMethod.Name & "(): " & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub ' btnAdd_Click

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ''********
        '' New request was cancelled.
        '' Return to the list form
        ''********
        Response.Redirect("TestIssuanceList.aspx", False)

    End Sub ' btnCancel_Click

#End Region ' Control Event Handlers


#Region "Misc. Private Methods"

    Private Sub InitializeMasterPage()
        Try
            ''****************************************************
            '' Update the title and heading on the Master Page
            ''****************************************************
            Dim m As ASP.masterpage_master = DirectCast(Page.Master, ASP.masterpage_master)
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "New Test Issuance Request"

            ''**************************************************
            '' Override the Master Page bread crumb navigation
            ''**************************************************
            Dim ctl As Control = m.FindControl("lblOtherSiteNode")
            If ctl IsNot Nothing Then
                Dim lbl As Label = CType(ctl, Label)
                lbl.Text = _
                    "<a href='../Home.aspx'><b>Home</b></a> > <b>Research and Development</b> > <a href='TestIssuanceList.aspx'><b>Test Issuance Search</b></a> > New Test Issuance Request"
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
            ctl = m.FindControl("RnDExtender")
            If ctl IsNot Nothing Then
                Dim cpe As CollapsiblePanelExtender = CType(ctl, CollapsiblePanelExtender)
                cpe.Collapsed = False
            End If
        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim myMethod As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(myMethod.DeclaringType.Name & _
                "." & myMethod.Name & "(): " & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub ' InitializeMasterPage

    'Private ReadOnly Property QueryString_CopyId() As Integer
    '    Get
    '        ''*******
    '        '' Returns the CopyId query string, or -1 if not found.
    '        ''*******
    '        Dim intReturnValue As Integer = 0
    '        Dim str As String = Request.QueryString("CopyId")
    '        Dim blnResult As Boolean = Integer.TryParse(str, intReturnValue)
    '        Return intReturnValue
    '    End Get
    'End Property ' QueryString_CopyId

    'Private ReadOnly Property QueryString_RequestCategory() As Integer
    '    Get
    '        ''*******
    '        '' Returns the RequestCategory query string, or -1 if not found.
    '        ''*******
    '        Dim intReturnValue As Integer = 0
    '        Dim str As String = Request.QueryString("RequestCategory")
    '        Dim blnResult As Boolean = Integer.TryParse(str, intReturnValue)
    '        Return intReturnValue
    '    End Get
    'End Property ' QueryString_RequestCategory

#End Region ' Misc. Private Methods


End Class ' RnD_TestIssuanceNew
