''*****************************************************************************************************
''* Lock_in_Sales_Projection.aspx.vb
''* The purpose of this page is to allow users to archive data for a planning year and record type 
''* (Budget or Forecast) used for BI reporting.
''*
''* Author  : LRey 05/16/2008
''* Modified: {Name} {Date} - {Notes}
''*****************************************************************************************************
Imports System.Data
Imports System.Data.SqlClient
Partial Class PF_Lock_in_Sales_Projection
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Lock In Sales Projection"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b> Planning and Forecasting </b> > Lock In Sales Projection"
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim testMasterPanel As CollapsiblePanelExtender
            testMasterPanel = CType(Master.FindControl("PFExtender"), CollapsiblePanelExtender)
            testMasterPanel.Collapsed = False


            If Not Page.IsPostBack Then
                'Bind data to drop down lists
                BindCriteria()

                ''Set Focus
                ddYear.Focus()
            End If

            Dim sScript As String
            sScript = "<script language=""JavaScript"">" & vbCrLf
            sScript += "function ConfirmButton(name)" & vbCrLf
            sScript += "{" & vbCrLf
            sScript += vbTab & "var Valid = true;" & vbCrLf
            sScript += vbTab & "if(typeof(Page_ClientValidate) == 'function')" & vbCrLf
            sScript += vbTab & "{" & vbCrLf
            sScript += vbTab & vbTab & "Valid = Page_ClientValidate(); " & vbCrLf
            sScript += vbTab & "}" & vbCrLf
            sScript += vbTab & "if(Valid)" & vbCrLf
            sScript += vbTab & "{" & vbCrLf
            sScript += vbTab & vbTab & "var Status= true;" & vbCrLf
            sScript += vbTab & vbTab & "if (name=='Submit'){" & vbCrLf
            sScript += vbTab & vbTab & vbTab & "Status = confirm('Are you sure you want to Lock In Sales Projection?');}" & vbCrLf
            sScript += vbTab & vbTab & vbTab & "return Status;" & vbCrLf
            sScript += vbTab & vbTab & "}" & vbCrLf
            sScript += vbTab & "else" & vbCrLf
            sScript += vbTab & "{" & vbCrLf
            sScript += vbTab & vbTab & vbTab & "return false;" & vbCrLf
            sScript += vbTab & "}" & vbCrLf
            sScript += "}" & vbCrLf
            sScript += "// -->" & vbCrLf
            sScript += "</script>" & vbCrLf

            If (Not ClientScript.IsClientScriptBlockRegistered("MyScript")) Then
                ClientScript.RegisterClientScriptBlock(Page.GetType, "MyScript", sScript)
            End If

            btnLockIn.Attributes.Add("onClick", "return ConfirmButton('Submit');")
        Catch ex As Exception
            'update error on web page
            lblErrors.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub
    Protected Sub BindCriteria()
        Dim ds As DataSet = New DataSet

        ''bind existing data to drop down Planning Year control for selection criteria for search
        ds = commonFunctions.GetYear("")
        If (ds.Tables.Item(0).Rows.Count > 0) Then
            ddYear.DataSource = ds
            ddYear.DataTextField = ds.Tables(0).Columns("YearID").ColumnName.ToString()
            ddYear.DataValueField = ds.Tables(0).Columns("YearID").ColumnName.ToString()
            ddYear.DataBind()
            ddYear.Items.Insert(0, "")
        End If
    End Sub

    Protected Sub ddRecordType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddRecordType.SelectedIndexChanged
        If ddRecordType.SelectedValue = "Budget" Then
            ddRecordTypeNo.Enabled = False
        Else
            ddRecordTypeNo.Enabled = True
        End If
    End Sub

    Protected Sub btnLockIn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLockIn.Click
        Me.Validate()
        Dim ds As DataSet

        If Page.IsValid Then
            Dim PlanningYear As Integer = ddYear.SelectedValue
            Dim RecordType As String = ddRecordType.SelectedValue
            Dim RecordTypeNo As Integer = CType(IIf(ddRecordTypeNo.SelectedValue = "", 0, ddRecordTypeNo.SelectedValue), Integer)
            Dim MsgDesc As String

            If RecordTypeNo = 0 Then
                MsgDesc = PlanningYear & " " & RecordType
            Else
                MsgDesc = PlanningYear & " " & RecordType & "/" & RecordTypeNo
            End If
            lblErrors.Visible = False

            Try
                ''*****
                ''Verify that data does not already exist in Archive, display message if true.
                ''*****
                ds = PFModule.GetArchiveData(PlanningYear, RecordType, RecordTypeNo)
                If (ds.Tables.Item(0).Rows.Count > 0) Then
                    lblErrors.Text = "Cannot overwrite existing archived data for " & MsgDesc & "."
                    lblErrors.Visible = "True"
                Else
                    ''*****
                    ''Archive data according to the selected Planning Year and Record Type.
                    ''*****
                    PFModule.LockInSalesProjection(PlanningYear, RecordType, RecordTypeNo)
                    lblErrors.Text = "Data Locked for " & MsgDesc & " successfully."
                    lblErrors.Visible = "True"
                End If

            Catch ex As Exception
                lblErrors.Text = "Error occurred during archiving.  Please contact the IS Application Group." & ex.Message
                lblErrors.Visible = "True"
            End Try
        End If
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click

        ''******
        '' Redirect to the Lock in Sales Projection page
        ''******
        Response.Redirect("Lock_in_Sales_Projection.aspx", False)
    End Sub
End Class
