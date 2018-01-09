' ************************************************************************************************
' Name:	crViewDatabaseGrowthTracking.aspx.vb
' Purpose:	This program is used to call crystal viewer and get data from old [Test Issuance Requests] table
'
' Date		    Author	    
' 06/02/2009    LRey			Created .Net application
' ************************************************************************************************
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Partial Class RnD_crViewDatabaseGrowthTracking
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim m As ASP.crviewmasterpage_master = Master

        ''***********************************************
        ''Code Below overrides the breadcrumb navigation 
        ''***********************************************
        Dim mpTextBox As Label
        mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
        If Not mpTextBox Is Nothing Then
            mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Security - DBA Workspace</b> > <a href='DatabaseGrowthTracking.aspx'><b>Database Growth Tracking</b></a> "
            mpTextBox.Visible = True
            Master.FindControl("SiteMapPath1").Visible = False
        End If

    End Sub
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        ViewState("pDtRecFrom") = HttpContext.Current.Request.QueryString("pDtRecFrom")
        ViewState("pDtRecTo") = HttpContext.Current.Request.QueryString("pDtRecTo")
        ViewState("pServerName") = HttpContext.Current.Request.QueryString("pServerName")

        Dim oRpt As ReportDocument = New ReportDocument()

        If Session("TempCrystalRptFiles") Is Nothing Then
            Dim crTable As CrystalDecisions.CrystalReports.Engine.Table
            'Dim crTbl As CrystalDecisions.CrystalReports.Engine.Table
            Dim crDatabase As CrystalDecisions.CrystalReports.Engine.Database
            Dim dbConn As TableLogOnInfo = New TableLogOnInfo()

            ' new report document object 
            oRpt.Load(Server.MapPath(".\Reports\") & "crDatabaseGrowthTracking.rpt")

            'getting the database, the table and the LogOnInfo object which holds login onformation 
            crDatabase = oRpt.Database

            'getting the table in an object array of one item 
            Dim arrTables(2) As CrystalDecisions.CrystalReports.Engine.Table
            crDatabase.Tables.CopyTo(arrTables, 0)
            ' assigning the first item of array to crTable by downcasting the object to Table 
            crTable = arrTables(0)

            ' setting values 
            dbConn = crTable.LogOnInfo
            dbConn.ConnectionInfo.DatabaseName = System.Configuration.ConfigurationManager.AppSettings("DBAInstance").ToString() '"Test_UGN_HR" or "UGN_HR"
            dbConn.ConnectionInfo.ServerName = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString() '"TAPS1"
            dbConn.ConnectionInfo.UserID = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
            dbConn.ConnectionInfo.Password = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString

            ' applying login info to the table object 
            crTable.ApplyLogOnInfo(dbConn)

            ' defining report source 
            CrystalReportViewer1.DisplayGroupTree = True
            CrystalReportViewer1.ReportSource = oRpt

            ' so uptil now we have created everything 
            ' what remains is to pass parameters to our report, so it 
            ' shows only selected records. so calling a method to set 
            ' those parameters. 

            'Check if there are parameters or not in report.
            Dim intCounter As Integer = oRpt.DataDefinition.ParameterFields.Count

            setReportParameters()
            Session("TempCrystalRptFiles") = oRpt
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
            Dim pfDtRecFrom As ParameterField = New ParameterField
            Dim pfDtRecTo As ParameterField = New ParameterField
            Dim pfServerName As ParameterField = New ParameterField

            ' setting the name of parameter fields with wich they will be recieved in report 
            pfServerName.ParameterFieldName = "@ServerName"
            pfDtRecFrom.ParameterFieldName = "@DateRecordedFrom"
            pfDtRecTo.ParameterFieldName = "@DateRecordedTo"

            ' the above declared parameter fields accept values as discrete objects 
            ' so declaring discrete objects 
            Dim dcDtRecFrom As New ParameterDiscreteValue
            Dim dcDtRecTo As New ParameterDiscreteValue
            Dim dcServerName As New ParameterDiscreteValue


            ' setting the values of discrete objects 
            dcDtRecFrom.Value = ViewState("pDtRecFrom")
            dcDtRecTo.Value = ViewState("pDtRecTo")
            dcServerName.Value = ViewState("pServerName")

            ' now adding these discrete values to parameters 
            pfDtRecFrom.CurrentValues.Add(dcDtRecFrom)
            pfDtRecTo.CurrentValues.Add(dcDtRecTo)
            pfServerName.CurrentValues.Add(dcServerName)

            ' now adding all these parameter fields to the parameter collection 
            paramFields.Add(pfDtRecFrom)
            paramFields.Add(pfDtRecTo)
            paramFields.Add(pfServerName)

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
