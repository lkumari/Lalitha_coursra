' ************************************************************************************************
' Name:	crPreview_Manufacturing_Metric_Report.aspx.vb
' Purpose:	This program is used to call crystal viewer and get data from Manufacturing Metric Report
'
' Date		    Author	    
' 06/16/2010    Roderick Carlson			Created  
' 01/27/2011    Roderick Carlson            Modified: Adding "ALL UGN FACILITY" reports 
' 03/10/2011    Roderick Carlson            Modified: Add MTD Report Selection
' 03/31/2011    Roderick Carlson            Modified: Add Date Range Reports  
' 04/08/2013    Roderick Carlson            Modified: Add Monthly Actuals Compare 
' ************************************************************************************************
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared


Partial Class PlantSpecificReports_crPreview_Manufacturing_Metric_Report
    Inherits System.Web.UI.Page

    Protected Sub CheckRights()

        Try
            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet

            Dim iTeamMemberID As Integer = 0
            Dim iRoleID As Integer = 0

            ViewState("isAdmin") = False

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, True, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataset(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 107)

                If commonFunctions.CheckDataset(dsRoleForm) = True Then

                    iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                    Select Case iRoleID
                        Case 11 '*** UGNAdmin: Full Access
                            ViewState("isAdmin") = True
                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                            ViewState("isAdmin") = True
                        Case 13 '*** UGNAssist: Create/Edit/No Delete
                            ViewState("isAdmin") = True
                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                            ViewState("isAdmin") = True
                        Case 15 '*** UGNEdit: No Create/Edit/No Delete
                            ViewState("isAdmin") = True
                        Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                            ViewState("isAdmin") = False
                    End Select                
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page

            lblMessage.Text += ex.Message & "<br>" & mb.Name
            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    'Protected Sub HandleDailyReport()

    '    Dim oRpt As ReportDocument = New ReportDocument()

    '    If ViewState("ReportDate") <> "" Then
    '        If (Session("MMReportPreview") Is Nothing) Then

    '            ' new report document object 
    '            If ViewState("UGNFacility") = "" Then
    '                oRpt.Load(Server.MapPath(".\Forms\") & "DailyManufacturingMetricSummaryReport.rpt")
    '            Else
    '                oRpt.Load(Server.MapPath(".\Forms\") & "DailyManufacturingMetricReport.rpt")
    '            End If


    '            Dim strDatabaseName As String = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString() '"Test_UGNDB" OR "UGNDB"
    '            Dim strServerName As String = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString() '"DB_Server" and soon SQLCLUSTERVS
    '            Dim strUserID As String = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
    '            Dim strPassword As String = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString

    '            oRpt.SetDatabaseLogon(strUserID, strPassword, strServerName, strDatabaseName)
    '            oRpt.SetParameterValue("@ReportDate", ViewState("ReportDate"))
    '            oRpt.SetParameterValue("@UGNFacility", ViewState("UGNFacility"))

    '            Session("MMReportPreview") = oRpt

    '            Dim oStream As New System.IO.MemoryStream
    '            oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
    '            Response.Clear()
    '            Response.Buffer = True
    '            Response.ContentType = "application/pdf"

    '            Response.Charset = ""
    '            Response.AddHeader("content-disposition", "inline;filename=ManufacturingMetric-Daily-" & ViewState("ReportDate").ToString & "preview.pdf")

    '            Response.BinaryWrite(oStream.ToArray())
    '            'Response.End()

    '        Else
    '            oRpt = CType(Session("MMReportPreview"), ReportDocument)

    '            Dim oStream As New System.IO.MemoryStream
    '            oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
    '            Response.Clear()
    '            Response.Buffer = True
    '            Response.ContentType = "application/pdf"
    '            Response.Charset = ""
    '            Response.AddHeader("content-disposition", "inline;filename=ManufacturingMetric-Daily-" & ViewState("ReportDate").ToString & "preview.pdf")

    '            Response.BinaryWrite(oStream.ToArray())
    '            'Response.End()
    '        End If
    '    End If

    'End Sub

    Protected Sub HandleMonthlyReport()

        Dim ds As DataSet
        Dim bObsolete As Boolean = True

        'get ReportID based on month and UGNFacility 
        If ViewState("UGNFacility") = "" And ViewState("ReportID") = 0 Then
            ds = PSRModule.GetManufacturingMetricSearch(0, ViewState("MonthID"), ViewState("YearID"), "", 0, 0)
            If commonFunctions.CheckDataSet(ds) = True Then
                'do not save the report ID when getting a summary, just check to make sure at least one month report exists
                If ds.Tables(0).Rows(0).Item("ReportID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("Obsolete") IsNot System.DBNull.Value Then
                        bObsolete = ds.Tables(0).Rows(0).Item("Obsolete")
                    End If
                End If
            End If
        Else
            ds = PSRModule.GetManufacturingMetricSearch(ViewState("ReportID"), ViewState("MonthID"), ViewState("YearID"), ViewState("UGNFacility"), 0, 0)
            If commonFunctions.CheckDataSet(ds) = True Then
                If ds.Tables(0).Rows(0).Item("ReportID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(0).Item("ReportID") > 0 Then
                        ViewState("ReportID") = ds.Tables(0).Rows(0).Item("ReportID")
                    End If

                    If ds.Tables(0).Rows(0).Item("Obsolete") IsNot System.DBNull.Value Then
                        bObsolete = ds.Tables(0).Rows(0).Item("Obsolete")
                    End If
                End If
            End If
        End If

        'show report
        'If ViewState("ReportID") > 0 And bObsolete = False Then
        If bObsolete = False Then

            Dim oRpt As ReportDocument = New ReportDocument()

            If Session("MMReportPreviewID") <> ViewState("ReportID") Or ViewState("ReportID") = 0 Then
                Session("MMReportPreview") = Nothing
                Session("MMReportPreviewID") = Nothing
            End If

            If (Session("MMReportPreview") Is Nothing) Then

                ' new report document object 
                If ViewState("UGNFacility") = "" And ViewState("ReportID") = 0 Then
                    oRpt.Load(Server.MapPath(".\Forms\") & "MonthlyManufacturingMetricSummaryReport.rpt")
                Else
                    oRpt.Load(Server.MapPath(".\Forms\") & "MonthlyManufacturingMetricReport.rpt")
                End If

                Dim strDatabaseName As String = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString() '"Test_UGNDB" OR "UGNDB"
                Dim strServerName As String = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString() '"DB_Server" and soon SQLCLUSTERVS
                Dim strUserID As String = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
                Dim strPassword As String = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString

                oRpt.SetDatabaseLogon(strUserID, strPassword, strServerName, strDatabaseName)

                'if facility is blank then get summary of all
                If ViewState("UGNFacility") = "" And ViewState("ReportID") = 0 Then
                    oRpt.SetParameterValue("@MonthID", ViewState("MonthID"))
                    oRpt.SetParameterValue("@YearID", ViewState("YearID"))
                    Session("MMReportPreviewID") = 0
                Else
                    oRpt.SetParameterValue("@ReportID", ViewState("ReportID"))
                    Session("MMReportPreviewID") = ViewState("ReportID")
                End If

                Session("MMReportPreview") = oRpt

                Dim oStream As New System.IO.MemoryStream
                oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                Response.Clear()
                Response.Buffer = True
                Response.ContentType = "application/pdf"

                Response.Charset = ""
                Response.AddHeader("content-disposition", "inline;filename=ManufacturingMetric-Monthly-" & ViewState("YearID").ToString & ViewState("MonthID").ToString & "preview.pdf")

                Response.BinaryWrite(oStream.ToArray())
                'Response.End()

            Else
                oRpt = CType(Session("MMReportPreview"), ReportDocument)

                Dim oStream As New System.IO.MemoryStream
                oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                Response.Clear()
                Response.Buffer = True
                Response.ContentType = "application/pdf"

                Response.Charset = ""
                Response.AddHeader("content-disposition", "inline;filename=ManufacturingMetric-Monthly-" & ViewState("YearID").ToString & ViewState("MonthID").ToString & "preview.pdf")

                Response.BinaryWrite(oStream.ToArray())
                'Response.End()
            End If
        Else
            lblMessage.Text = "<br><br><br>NO Records exist for the selected month and UGN facility."
        End If

    End Sub

    Protected Sub HandleMonthlyActualsCompareReport()

        'get ReportID based on month and year 
        If ViewState("MonthID") > 0 And ViewState("YearID") > 0 Then

            Session("MMReportPreview") = Nothing
            Session("MMReportPreviewID") = Nothing

            Dim oRpt As ReportDocument = New ReportDocument()

            oRpt.Load(Server.MapPath(".\Forms\") & "MonthlyManufacturingMetricCompareSummary.rpt")

            Dim strDatabaseName As String = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString() '"Test_UGNDB" OR "UGNDB"
            Dim strServerName As String = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString() '"DB_Server" and soon SQLCLUSTERVS
            Dim strUserID As String = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
            Dim strPassword As String = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString

            oRpt.SetDatabaseLogon(strUserID, strPassword, strServerName, strDatabaseName)

            oRpt.SetParameterValue("@YearID", ViewState("YearID"))
            oRpt.SetParameterValue("@MonthID", ViewState("MonthID"))
            Session("MMReportPreview") = oRpt

            Dim oStream As New System.IO.MemoryStream
            oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
            Response.Clear()
            Response.Buffer = True
            Response.ContentType = "application/pdf"

            Response.Charset = ""
            Response.AddHeader("content-disposition", "inline;filename=ManufacturingMetric-Monthly-Compare" & ViewState("YearID").ToString & ViewState("MonthID").ToString & "preview.pdf")

            Response.BinaryWrite(oStream.ToArray())
            'Response.End()

        End If

    End Sub

    Protected Sub HandleDateRangeReport(ByVal ReportStartDate As String, ByVal ReportEndDate As String)

        Dim oRpt As ReportDocument = New ReportDocument()

        If ReportStartDate <> "" And ReportEndDate <> "" Then
            If (Session("MMReportPreview") Is Nothing) Then

                ' new report document object 
                If ViewState("UGNFacility") = "" Then
                    oRpt.Load(Server.MapPath(".\Forms\") & "DateRangeManufacturingMetricSummaryReport.rpt")
                Else
                    oRpt.Load(Server.MapPath(".\Forms\") & "DateRangeManufacturingMetricReport.rpt")
                End If

                Dim strDatabaseName As String = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString() '"Test_UGNDB" OR "UGNDB"
                Dim strServerName As String = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString() '"DB_Server" and soon SQLCLUSTERVS
                Dim strUserID As String = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
                Dim strPassword As String = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString

                oRpt.SetDatabaseLogon(strUserID, strPassword, strServerName, strDatabaseName)
                oRpt.SetParameterValue("@UGNFacility", ViewState("UGNFacility"))
                oRpt.SetParameterValue("@ReportStartDate", ReportStartDate)
                oRpt.SetParameterValue("@ReportEndDate", ReportEndDate)

                Session("MMReportPreview") = oRpt

                Dim oStream As New System.IO.MemoryStream
                oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                Response.Clear()
                Response.Buffer = True
                Response.ContentType = "application/pdf"

                Response.Charset = ""
                Response.AddHeader("content-disposition", "inline;filename=ManufacturingMetric-DateRange-" & ViewState("UGNFacility") & ViewState("ReportStartDate").ToString & "thru" & ViewState("ReportEndDate").ToString & "preview.pdf")

                Response.BinaryWrite(oStream.ToArray())
                'Response.End()

            Else
                oRpt = CType(Session("MMReportPreview"), ReportDocument)

                Dim oStream As New System.IO.MemoryStream
                oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                Response.Clear()
                Response.Buffer = True
                Response.ContentType = "application/pdf"

                Response.Charset = ""
                Response.AddHeader("content-disposition", "inline;filename=ManufacturingMetric-DateRange-" & ViewState("UGNFacility") & ViewState("ReportStartDate").ToString & "thru" & ViewState("ReportEndDate").ToString & "preview.pdf")

                Response.BinaryWrite(oStream.ToArray())
                'Response.End()
            End If
        End If

    End Sub

    Protected Sub HandleMTDReport()

        'Dim oRpt As ReportDocument = New ReportDocument()
        Dim strFirstDayOfMonth As String = ""
        Dim strYesterday As String = ""

        'If Today = DateTime.Today.AddMonths(1).AddDays(-1) Then
        If Today = DateTime.Today.AddDays(-(DateTime.Today.Day - 1)) Then
            strFirstDayOfMonth = Today.AddDays(-1).Month.ToString.PadLeft(2, "0") & "/01/" & Today.Year.ToString
            strYesterday = Today.AddDays(-1).Month.ToString.PadLeft(2, "0") & "/" & Today.AddDays(-1).Day.ToString.PadLeft(2, "0") & "/" & Today.Year.ToString
        Else
            strFirstDayOfMonth = Today.Month.ToString.PadLeft(2, "0") & "/01/" & Today.Year.ToString
            strYesterday = Today.Month.ToString.PadLeft(2, "0") & "/" & Today.AddDays(-1).Day.ToString.PadLeft(2, "0") & "/" & Today.Year.ToString
        End If


        HandleDateRangeReport(strFirstDayOfMonth, strYesterday)

        'If strFirstDayOfMonth <> "" And strToday <> "" Then
        '    If (Session("MMReportPreview") Is Nothing) Then

        '        ' new report document object 
        '        If ViewState("UGNFacility") = "" Then
        '            oRpt.Load(Server.MapPath(".\Forms\") & "MTDManufacturingMetricSummaryReport.rpt")
        '        Else
        '            oRpt.Load(Server.MapPath(".\Forms\") & "MTDManufacturingMetricReport.rpt")
        '        End If

        '        Dim strDatabaseName As String = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString() '"Test_UGNDB" OR "UGNDB"
        '        Dim strServerName As String = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString() '"DB_Server" and soon SQLCLUSTERVS
        '        Dim strUserID As String = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
        '        Dim strPassword As String = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString

        '        oRpt.SetDatabaseLogon(strUserID, strPassword, strServerName, strDatabaseName)
        '        oRpt.SetParameterValue("@UGNFacility", ViewState("UGNFacility"))
        '        oRpt.SetParameterValue("@ReportStartDate", strFirstDayOfMonth)
        '        oRpt.SetParameterValue("@ReportEndDate", strToday)

        '        Session("MMReportPreview") = oRpt

        '        Dim oStream As New System.IO.MemoryStream
        '        oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
        '        Response.Clear()
        '        Response.Buffer = True
        '        Response.ContentType = "application/pdf"

        '        Response.Charset = ""
        '        Response.AddHeader("content-disposition", "inline;filename=ManufacturingMetric-MTD-" & ViewState("UGNFacility") & ViewState("ReportStartDate").ToString & "thru" & ViewState("ReportEndDate").ToString & "preview.pdf")

        '        Response.BinaryWrite(oStream.ToArray())
        '        'Response.End()

        '    Else
        '        oRpt = CType(Session("MMReportPreview"), ReportDocument)

        '        Dim oStream As New System.IO.MemoryStream
        '        oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
        '        Response.Clear()
        '        Response.Buffer = True
        '        Response.ContentType = "application/pdf"

        '        Response.Charset = ""
        '        Response.AddHeader("content-disposition", "inline;filename=ManufacturingMetric-MTD-" & ViewState("UGNFacility") & ViewState("ReportStartDate").ToString & "thru" & ViewState("ReportEndDate").ToString & "preview.pdf")

        '        Response.BinaryWrite(oStream.ToArray())
        '        'Response.End()
        '    End If
        'End If

    End Sub

    'Protected Sub HandleWeeklyReport()

    '    Dim oRpt As ReportDocument = New ReportDocument()

    '    If ViewState("ReportStartDate") <> "" And ViewState("ReportEndDate") <> "" Then

    '        If (Session("MMReportPreview") Is Nothing) Then

    '            ' new report document object 
    '            If ViewState("UGNFacility") = "" Then
    '                'oRpt.Load(Server.MapPath(".\Forms\") & "WeeklyManufacturingMetricSummaryReport.rpt")
    '                oRpt.Load(Server.MapPath(".\Forms\") & "DateRangeManufacturingMetricSummaryReport.rpt")
    '            Else
    '                'oRpt.Load(Server.MapPath(".\Forms\") & "WeeklyManufacturingMetricReport.rpt")
    '                oRpt.Load(Server.MapPath(".\Forms\") & "DateRangeManufacturingMetricReport.rpt")
    '            End If


    '            Dim strDatabaseName As String = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString() '"Test_UGNDB" OR "UGNDB"
    '            Dim strServerName As String = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString() '"DB_Server" and soon SQLCLUSTERVS
    '            Dim strUserID As String = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
    '            Dim strPassword As String = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString

    '            oRpt.SetDatabaseLogon(strUserID, strPassword, strServerName, strDatabaseName)
    '            oRpt.SetParameterValue("@ReportStartDate", ViewState("ReportStartDate"))
    '            oRpt.SetParameterValue("@ReportEndDate", ViewState("ReportEndDate"))
    '            oRpt.SetParameterValue("@UGNFacility", ViewState("UGNFacility"))

    '            Session("MMReportPreview") = oRpt

    '            Dim oStream As New System.IO.MemoryStream
    '            oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
    '            Response.Clear()
    '            Response.Buffer = True
    '            Response.ContentType = "application/pdf"

    '            Response.Charset = ""
    '            Response.AddHeader("content-disposition", "inline;filename=ManufacturingMetric-Weekly-" & ViewState("UGNFacility") & ViewState("ReportStartDate").ToString & "thru" & ViewState("ReportEndDate").ToString & "preview.pdf")

    '            Response.BinaryWrite(oStream.ToArray())
    '            'Response.End()

    '        Else
    '            oRpt = CType(Session("MMReportPreview"), ReportDocument)

    '            Dim oStream As New System.IO.MemoryStream
    '            oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
    '            Response.Clear()
    '            Response.Buffer = True
    '            Response.ContentType = "application/pdf"

    '            Response.Charset = ""
    '            Response.AddHeader("content-disposition", "inline;filename=ManufacturingMetric-Weekly-" & ViewState("UGNFacility") & ViewState("ReportStartDate").ToString & "thru" & ViewState("ReportEndDate").ToString & "preview.pdf")

    '            Response.BinaryWrite(oStream.ToArray())
    '            'Response.End()
    '        End If
    '    End If

    'End Sub

    Protected Sub HandleYearlyReport()

        If ViewState("YearID") > 0 Then

            Dim oRpt As ReportDocument = New ReportDocument()

            If (Session("MMReportPreview") Is Nothing) Then

                ' new report document object 
                If ViewState("UGNFacility") = "" Then
                    oRpt.Load(Server.MapPath(".\Forms\") & "YearlyManufacturingMetricSummaryReport.rpt")
                Else
                    oRpt.Load(Server.MapPath(".\Forms\") & "YearlyManufacturingMetricReport.rpt")
                End If


                Dim strDatabaseName As String = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString() '"Test_UGNDB" OR "UGNDB"
                Dim strServerName As String = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString() '"DB_Server" and soon SQLCLUSTERVS
                Dim strUserID As String = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
                Dim strPassword As String = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString

                oRpt.SetDatabaseLogon(strUserID, strPassword, strServerName, strDatabaseName)
                oRpt.SetParameterValue("@YearID", ViewState("YearID"))

                If ViewState("UGNFacility") <> "" Then
                    oRpt.SetParameterValue("@UGNFacility", ViewState("UGNFacility"))
                End If

                Session("MMReportPreview") = oRpt

                Dim oStream As New System.IO.MemoryStream
                oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                Response.Clear()
                Response.Buffer = True
                Response.ContentType = "application/pdf"

                Response.Charset = ""
                Response.AddHeader("content-disposition", "inline;filename=ManufacturingMetric-YearToMonth-" & ViewState("UGNFacility") & ViewState("YearID").ToString & "preview.pdf")

                Response.BinaryWrite(oStream.ToArray())
                'Response.End()

            Else
                oRpt = CType(Session("MMReportPreview"), ReportDocument)

                Dim oStream As New System.IO.MemoryStream
                oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                Response.Clear()
                Response.Buffer = True
                Response.ContentType = "application/pdf"

                Response.Charset = ""
                Response.AddHeader("content-disposition", "inline;filename=ManufacturingMetric-YearToMonth-" & ViewState("UGNFacility") & ViewState("YearID").ToString & "preview.pdf")

                Response.BinaryWrite(oStream.ToArray())
                'Response.End()
            End If
        End If

    End Sub

    Protected Sub HandleYTDReport()

        Dim strFirstDayOfYear As String = ""
        'Dim strToday As String = Today.Month.ToString.PadLeft(2, "0") & "/" & Today.AddDays(-1).Day.ToString.PadLeft(2, "0") & "/" & Today.Year.ToString 'Now.ToShortDateString
        Dim strYesterday As String = ""

        'if today is first day of the year
        If Today.Day.ToString = "1" And Today.Month.ToString = "1" Then
            'get all of last year
            strFirstDayOfYear = "01/01/" & Today.AddDays(-1).Year.ToString
            strYesterday = "12/31" & Today.AddDays(-1).Year.ToString
        Else
            'if today is first day of the month 
            If Today = DateTime.Today.AddDays(-(DateTime.Today.Day - 1)) Then
                strFirstDayOfYear = "01/01/" & Today.Year.ToString
                strYesterday = Today.AddDays(-1).Month.ToString.PadLeft(2, "0") & "/" & Today.AddDays(-1).Day.ToString.PadLeft(2, "0") & "/" & Today.Year.ToString
            Else
                strFirstDayOfYear = "01/01/" & Today.Year.ToString
                strYesterday = Today.Month.ToString.PadLeft(2, "0") & "/" & Today.AddDays(-1).Day.ToString.PadLeft(2, "0") & "/" & Today.Year.ToString
            End If
        End If

        HandleDateRangeReport(strFirstDayOfYear, strYesterday)

    End Sub
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try

            Dim strReportType As String = "D"

            CheckRights()

            If ViewState("isAdmin") = True Then

                If HttpContext.Current.Request.QueryString("ReportType") <> "" Then
                    strReportType = HttpContext.Current.Request.QueryString("ReportType")
                End If

                ViewState("ReportID") = 0
                If HttpContext.Current.Request.QueryString("ReportID") <> "" Then
                    ViewState("ReportID") = HttpContext.Current.Request.QueryString("ReportID")
                End If

                'ViewState("ReportDate") = ""
                'If HttpContext.Current.Request.QueryString("ReportDate") <> "" Then
                '    ViewState("ReportDate") = HttpContext.Current.Request.QueryString("ReportDate")
                'End If

                ViewState("ReportStartDate") = ""
                If HttpContext.Current.Request.QueryString("ReportStartDate") <> "" Then
                    ViewState("ReportStartDate") = HttpContext.Current.Request.QueryString("ReportStartDate")
                End If

                ViewState("ReportEndDate") = ""
                If HttpContext.Current.Request.QueryString("ReportEndDate") <> "" Then
                    ViewState("ReportEndDate") = HttpContext.Current.Request.QueryString("ReportEndDate")
                End If

                ViewState("MonthID") = 0
                If HttpContext.Current.Request.QueryString("MonthID") <> "" Then
                    ViewState("MonthID") = HttpContext.Current.Request.QueryString("MonthID")
                End If

                ViewState("YearID") = 0
                If HttpContext.Current.Request.QueryString("YearID") <> "" Then
                    ViewState("YearID") = HttpContext.Current.Request.QueryString("YearID")
                End If

                ViewState("UGNFacility") = ""
                If HttpContext.Current.Request.QueryString("UGNFacility") <> "" Then
                    ViewState("UGNFacility") = HttpContext.Current.Request.QueryString("UGNFacility")
                End If

                Select Case strReportType
                    'Case "D"
                    '    HandleDailyReport()
                    Case "M"
                        HandleMonthlyReport()
                    Case "MAC"
                        HandleMonthlyActualsCompareReport()
                    Case "MTD"
                        HandleMTDReport()
                    Case "D", "W", "DR"
                        'HandleWeeklyReport()
                        HandleDateRangeReport(ViewState("ReportStartDate"), ViewState("ReportEndDate"))
                    Case "Y"
                        HandleYearlyReport()
                    Case "YTD"
                        HandleYTDReport()                    
                End Select

            Else
                lblMessage.Text = "You do not have access to this information. Please contact the Corporate Controller."
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            'UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

        If HttpContext.Current.Session("MMReportPreview") IsNot Nothing Then
            Dim tempRpt As ReportDocument = New ReportDocument()
            tempRpt = CType(HttpContext.Current.Session("MMReportPreview"), ReportDocument)
            If tempRpt IsNot Nothing Then
                tempRpt.Close()
                tempRpt.Dispose()
            End If
            HttpContext.Current.Session("MMReportPreview") = Nothing
            HttpContext.Current.Session("MMReportPreview") = Nothing
            GC.Collect()
        End If

    End Sub
End Class
