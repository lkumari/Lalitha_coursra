' ************************************************************************************************
' Name:	crViewCycleCounterMatrixDetail.aspx.vb
' Purpose:	This program is used to call crystal viewer and get data from UGNDatastore.dbo.Cycle_Count_Matrix table
'           and other sources.  Also used as an approval screen to allow users to view current
'           info related to a capex asset project and approve/reject the project in one screen.
'
' Date		    Author	    
' 06/26/2012    LRey			Created .Net application
' ************************************************************************************************
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Partial Class CCM_crCycleCounterMatrixDetail
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim m As ASP.crviewmasterpage_master = Master

        ''***********************************************
        ''Code Below overrides the breadcrumb navigation 
        ''***********************************************
        Dim mpTextBox As Label
        mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
        If Not mpTextBox Is Nothing Then
            mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Manufacturing</b> > <a href='CycleCounterMatrix.aspx'><b>Cycle Counter Matrix</b></a>"
            mpTextBox.Visible = True
            Master.FindControl("SiteMapPath1").Visible = False
        End If

    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try

            ViewState("pFac") = Nothing
            If HttpContext.Current.Request.QueryString("pFac") IsNot Nothing Then
                ViewState("pFac") = HttpContext.Current.Request.QueryString("pFac")
            End If

            If HttpContext.Current.Request.QueryString("pFD") IsNot Nothing Then
                ViewState("pFD") = HttpContext.Current.Request.QueryString("pFD")
            Else
                If Day(Date.Today) <> 1 Then
                    ViewState("pFD") = Month(Date.Today) & "/1/" & Year(Date.Today)
                Else
                    ViewState("pFD") = Date.Today
                End If
            End If

            If HttpContext.Current.Request.QueryString("pTD") IsNot Nothing Then
                ViewState("pTD") = HttpContext.Current.Request.QueryString("pTD")
            Else
                ViewState("pTD") = Date.Today
            End If

            ViewState("pSB") = "I"
            If HttpContext.Current.Request.QueryString("pSB") IsNot Nothing Then
                ViewState("pSB") = HttpContext.Current.Request.QueryString("pSB")
            End If

            ViewState("pSMEV") = Nothing
            If HttpContext.Current.Request.QueryString("pSMEV") IsNot Nothing Then
                ViewState("pSMEV") = HttpContext.Current.Request.QueryString("pSMEV")
            End If


            Dim oRpt As ReportDocument = New ReportDocument()

            If Session("TempCrystalRptFiles") Is Nothing Then
                Dim crTable As CrystalDecisions.CrystalReports.Engine.Table
                'Dim crTbl As CrystalDecisions.CrystalReports.Engine.Table
                Dim crDatabase As CrystalDecisions.CrystalReports.Engine.Database
                Dim dbConn As TableLogOnInfo = New TableLogOnInfo()

                ' new report document object 
                oRpt.Load(Server.MapPath(".\Forms\") & "crCCMDetail.rpt")

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

                oRpt.SetParameterValue("@UGNFacility", ViewState("pFac"))
                oRpt.SetParameterValue("@FromDate", ViewState("pFD"))
                oRpt.SetParameterValue("@ToDate", ViewState("pTD"))
                oRpt.SetParameterValue("@SortBy", ViewState("pSB"))
                oRpt.SetParameterValue("@SMEV", Nothing)

                ' defining report source 
                CrystalReportViewer1.DisplayGroupTree = False
                CrystalReportViewer1.ReportSource = oRpt

                'Check if there are parameters or not in report.
                Dim intCounter As Integer = oRpt.DataDefinition.ParameterFields.Count

                Session("TempCrystalRptFiles") = oRpt

                Dim Tdate As String = Replace(Date.Now, "/", "-")
                Dim oStream As New System.IO.MemoryStream

                '* Below code opens PDF in IE Browswer
                'oStream = oRpt.ExportToStream(ExportFormatType.Excel)
                ' oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                'With HttpContext.Current.Response
                '    .Clear()
                '    '.AddHeader("content-disposition", "inline;filename=CCM_Detail_" & Tdate & ".xls")
                '    .AddHeader("content-disposition", "inline;filename=CCM_Detail_" & Tdate & ".pdf")
                '    .Charset = ""
                '    '.ContentType = "application/vnd.ms-excel"
                '    .ContentType = "application/pdf"
                '    .BinaryWrite(oStream.ToArray())

                '    Dim stringWrite As StringWriter = New StringWriter(oStream.ToArray())
                '    Dim htmlWrite As HtmlTextWriter = New HtmlTextWriter(stringWrite)
                '    .Write(stringWrite.ToString())
                '    .End()
                'End With

                '* Below code asks to open in PDF 
                Response.Buffer = False
                Response.ClearContent()
                Response.ClearHeaders()
                oRpt.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "CCM_Detail_" & Tdate)

            Else
                oRpt = CType(Session("TempCrystalRptFiles"), ReportDocument)

                CrystalReportViewer1.ReportSource = oRpt
            End If
        Catch ex As Exception
            lblErrors.Text = "Error found in report view" & ex.Message
            lblErrors.Visible = "True"
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
