' ************************************************************************************************
' Name:	crViewTestIssuanceLabMatrix.aspx.vb
' Purpose:	This program is used to call crystal viewer and get data from old [Test Issuance Requests] table
'
' Date		    Author	    
' 05/28/2009    LRey			Created .Net application
' ************************************************************************************************
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Partial Class RnD_crViewTestReportCoverSheet
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim m As ASP.crviewmasterpage_master = Master

        ''*******
        '' Initialize ViewState
        ''*******
        If HttpContext.Current.Request.QueryString("pReqID") <> "" Then
            ViewState("pReqID") = CType(HttpContext.Current.Request.QueryString("pReqID"), Integer)
        Else
            ViewState("pReqID") = 0
        End If

        If HttpContext.Current.Request.QueryString("pReqCategory") <> "" Then
            ViewState("pReqCategory") = CType(HttpContext.Current.Request.QueryString("pReqCategory"), Integer)
        Else
            ViewState("pReqCategory") = 0
        End If

        If HttpContext.Current.Request.QueryString("pRptID") <> "" Then
            ViewState("pRptID") = HttpContext.Current.Request.QueryString("pRptID")
        Else
            ViewState("pRptID") = Nothing
        End If

        ''**************************************************
        '' Override the Master Page bread crumb navigation
        ''**************************************************
        Dim ctl As Control = m.FindControl("lblOtherSiteNode")
        If ctl IsNot Nothing Then
            Dim lbl As Label = CType(ctl, Label)

            Dim pReqCategory As String = ViewState("pReqCategory")
            Select Case pReqCategory
                Case 1
                    lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>R&D Lab</b> > <a href='TestIssuanceList.aspx'><b>Test Issuance Search</b></a> > <a href='TestIssuanceDetail.aspx?pReqID=" & ViewState("pReqID") & "&pReqCategory=" & ViewState("pReqCategory") & "'><b>New Product Development</b></a>"
                    lbl.Visible = True
                Case 2
                    lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>R&D Lab</b> > <a href='TestIssuanceList.aspx'><b>Test Issuance Search</b></a> > <a href='TestIssuanceDetail.aspx?pReqID=" & ViewState("pReqID") & "&pReqCategory=" & ViewState("pReqCategory") & "'><b>Current Mass Production Part</b></a>"
                    lbl.Visible = True
                Case 3
                    lbl.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>R&D Lab</b> > <a href='TestIssuanceList.aspx'><b>Test Issuance Search</b></a> > <a href='TestIssuanceDetail.aspx?pReqID=" & ViewState("pReqID") & "&pReqCategory=" & ViewState("pReqCategory") & "'><b>Consultation</b></a>"
                    lbl.Visible = True
            End Select
        End If

        ctl = m.FindControl("SiteMapPath1")
        If ctl IsNot Nothing Then
            Dim smp As SiteMapPath = CType(ctl, SiteMapPath)
            smp.Visible = False
        End If

    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        ViewState("pReqID") = HttpContext.Current.Request.QueryString("pReqID")
        ViewState("pRptID") = HttpContext.Current.Request.QueryString("pRptID")

        'If Not Page.IsPostBack Then

        Dim oRpt As ReportDocument = New ReportDocument()

        If Session("TempCrystalRptFiles") Is Nothing Then
            Dim crTable As CrystalDecisions.CrystalReports.Engine.Table
            'Dim crTbl As CrystalDecisions.CrystalReports.Engine.Table
            Dim crDatabase As CrystalDecisions.CrystalReports.Engine.Database
            Dim dbConn As TableLogOnInfo = New TableLogOnInfo()

            ' new report document object 
            oRpt.Load(Server.MapPath(".\Forms\") & "crTestReportCoverSheet.rpt")

            'getting the database, the table and the LogOnInfo object which holds login onformation 
            crDatabase = oRpt.Database

            'getting the table in an object array of one item 
            Dim arrTables(2) As CrystalDecisions.CrystalReports.Engine.Table
            crDatabase.Tables.CopyTo(arrTables, 0)
            ' assigning the first item of array to crTable by downcasting the object to Table 
            crTable = arrTables(0)

            ' setting values 
            dbConn = crTable.LogOnInfo
            dbConn.ConnectionInfo.DatabaseName = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString() '"Test_UGN_HR" or "UGN_HR"
            dbConn.ConnectionInfo.ServerName = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString() '"TAPS1"
            dbConn.ConnectionInfo.UserID = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
            dbConn.ConnectionInfo.Password = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString

            ' applying login info to the table object 
            crTable.ApplyLogOnInfo(dbConn)

            '' defining report source 
            'CrystalReportViewer1.DisplayGroupTree = True
            'CrystalReportViewer1.ReportSource = oRpt

            '' so uptil now we have created everything 
            '' what remains is to pass parameters to our report, so it 
            '' shows only selected records. so calling a method to set 
            '' those parameters. 

            ''Check if there are parameters or not in report.
            'Dim intCounter As Integer = oRpt.DataDefinition.ParameterFields.Count

            'setReportParameters()
            'Session("TempCrystalRptFiles") = oRpt


            oRpt.SetParameterValue("@RequestID", ViewState("pReqID"))
            oRpt.SetParameterValue("@TestReportID", ViewState("pRptID"))

            ' defining report source 
            CrystalReportViewer1.DisplayGroupTree = False
            CrystalReportViewer1.ReportSource = oRpt

            'Check if there are parameters or not in report.
            Dim intCounter As Integer = oRpt.DataDefinition.ParameterFields.Count

            Session("TempCrystalRptFiles") = oRpt

            Dim Tdate As String = Replace(Date.Now, "/", "-")
            Dim oStream As New System.IO.MemoryStream

            '* Below code asks to open in PDF 
            Response.Buffer = False
            Response.ClearContent()
            Response.ClearHeaders()
            ' new report document object 
            oRpt.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "R&D_Test_Report_" & ViewState("pRptID") & "_Cover_Sheet_" & Tdate)
      
        Else
        oRpt = CType(Session("TempCrystalRptFiles"), ReportDocument)

        CrystalReportViewer1.ReportSource = oRpt
        End If
    End Sub

    Private Sub setReportParameters()

        Try
            ' all the parameter fields will be added to this collection 
            Dim paramFields As New ParameterFields

            ' the parameter fields to be sent to the report 
            Dim pfRequestID As ParameterField = New ParameterField
            Dim pfRptID As ParameterField = New ParameterField

            ' setting the name of parameter fields with wich they will be recieved in report 
            pfRequestID.ParameterFieldName = "@RequestID"
            pfRptID.ParameterFieldName = "@TestReportID"

            ' the above declared parameter fields accept values as discrete objects 
            ' so declaring discrete objects 
            Dim dcRequestID As New ParameterDiscreteValue
            Dim dcRptID As New ParameterDiscreteValue


            ' setting the values of discrete objects 
            dcRequestID.Value = ViewState("pRequestID")
            dcRptID.Value = ViewState("pRptID")

            ' now adding these discrete values to parameters 
            pfRequestID.CurrentValues.Add(dcRequestID)
            pfRptID.CurrentValues.Add(dcRptID)

            ' now adding all these parameter fields to the parameter collection 
            paramFields.Add(pfRequestID)
            paramFields.Add(pfRptID)

            ' finally add the parameter collection to the crystal report viewer 
            CrystalReportViewer1.ParameterFieldInfo = paramFields

        Catch ex As Exception
            lblErrors.Text = "Error found in parameter search " & ex.Message
            lblErrors.Visible = True
        End Try
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        'in order to clear crystal reports
        If HttpContext.Current.Session("TempCrystalRptFiles") IsNot Nothing Then
            Dim tempRpt As ReportDocument = New ReportDocument()
            tempRpt = CType(HttpContext.Current.Session("TempCrystalRptFiles"), ReportDocument)
            If tempRpt IsNot Nothing Then
                tempRpt.Close()
                tempRpt.Dispose()
            End If
            HttpContext.Current.Session("TempCrystalRptFiles") = Nothing
            GC.Collect()
        End If
    End Sub
End Class
