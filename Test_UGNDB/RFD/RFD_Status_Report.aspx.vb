' ************************************************************************************************
'
' Name:		RFD Status Report
' Purpose:	This Code Behind is for the RFD Status Report in Crystal Reports/MS Excel format
'
' Date		    Author	    
' 03/26/2012    Roderick Carlson    Created
' 04/27/2012    Roderick Carlson    Modified - Made RFDNo in Report become a hyperlink

Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Partial Class RFD_Status_Report
    Inherits System.Web.UI.Page

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try

            Dim oRpt As ReportDocument = New ReportDocument()
            Dim Tdate As String = Replace(Date.Today.ToShortDateString, "/", "-")
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()

            If HttpContext.Current.Request.Cookies("UGNDB_User") Is Nothing Then
                commonFunctions.SetUGNDBUser()
            End If

            ViewState("SubscriptionID") = 0
            If HttpContext.Current.Request.QueryString("SubscriptionID") IsNot Nothing Then
                If HttpContext.Current.Request.QueryString("SubscriptionID") <> "" Then
                    ViewState("SubscriptionID") = CType(HttpContext.Current.Request.QueryString("SubscriptionID"), Integer)
                End If
            End If

            ViewState("UGNFacility") = ""
            If HttpContext.Current.Request.QueryString("UGNFacility") IsNot Nothing Then
                ViewState("UGNFacility") = HttpContext.Current.Request.QueryString("UGNFacility")
            End If

            ViewState("FileTypeExt") = "PDF"
            If HttpContext.Current.Request.QueryString("FileTypeExt") IsNot Nothing Then
                ViewState("FileTypeExt") = HttpContext.Current.Request.QueryString("FileTypeExt").ToUpper
            End If

            If (Session("RFDStatusReport") Is Nothing) Then

                oRpt.Load(Server.MapPath(".\Forms\") & "RFD_Status_Report.rpt")

                Dim strDatabaseName As String = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString() '"Test_UGNDB" OR "UGNDB"
                Dim strServerName As String = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString() '"DB_Server" and soon SQLCLUSTERVS
                Dim strUserID As String = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
                Dim strPassword As String = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString

                oRpt.SetDatabaseLogon(strUserID, strPassword, strServerName, strDatabaseName)
                oRpt.SetParameterValue("@SubscriptionID", ViewState("SubscriptionID"))
                oRpt.SetParameterValue("@UGNFacility", ViewState("UGNFacility"))
                oRpt.SetParameterValue("@ugndbEnvironment", strProdOrTestEnvironment)

                Session("RFDStatusReport") = oRpt

                'Dim oStream As New System.IO.MemoryStream
                'oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                'Response.Clear()
                'Response.Buffer = True
                'Response.ContentType = "application/pdf"

                ''this opens immediately
                'Response.Charset = ""
                'Response.AddHeader("content-disposition", "inline;filename=RFD-Status-Report.pdf")

                'Response.BinaryWrite(oStream.ToArray())
                ''Response.End()

                If ViewState("FileTypeExt") = "PDF" Or ViewState("FileTypeExt") = "" Then
                    Dim oStream As New System.IO.MemoryStream
                    oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                    Response.Clear()
                    Response.Buffer = True
                    Response.ContentType = "application/pdf"

                    'this opens immediately
                    Response.Charset = ""
                    Response.AddHeader("content-disposition", "inline;filename=RFD-Status-Report_" & Tdate & ".pdf")

                    Response.BinaryWrite(oStream.ToArray())
                    'Response.End()

                    'Dim oStream As New System.IO.MemoryStream
                    'oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                    'Response.Buffer = False
                    'Response.ClearContent()
                    'Response.ClearHeaders()

                    'Try
                    '    oRpt.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "rfdstatus")
                    'Catch ex As Exception

                    'End Try


                End If

                If ViewState("FileTypeExt") = "XLSX1" Then
                    Dim oStream As New System.IO.MemoryStream
                    oStream = oRpt.ExportToStream(ExportFormatType.Excel) 'with formatting
                    With HttpContext.Current.Response
                        .Clear()
                        .AddHeader("content-disposition", "attachment; filename=RFD-Status-Report_" & Tdate & ".xls")
                        .Charset = ""
                        '.Cache.SetCacheability(HttpCacheability.NoCache)
                        '.ContentType = "application/vnd.xls"
                        .ContentType = "application/vnd.ms-excel"
                        .BinaryWrite(oStream.ToArray())

                        Dim stringWrite As StringWriter = New StringWriter(oStream.ToArray())
                        Dim htmlWrite As HtmlTextWriter = New HtmlTextWriter(stringWrite)
                        .Write(stringWrite.ToString())
                        .End()
                    End With
                End If

                If ViewState("FileTypeExt") = "XLSX2" Then
                    Dim oStream As New System.IO.MemoryStream
                    oStream = oRpt.ExportToStream(ExportFormatType.ExcelRecord) 'remove formatting
                    With HttpContext.Current.Response
                        .Clear()
                        .AddHeader("content-disposition", "attachment; filename=RFD-Status-Report_" & Tdate & ".xls")
                        .Charset = ""
                        '.Cache.SetCacheability(HttpCacheability.NoCache)
                        '.ContentType = "application/vnd.xls"
                        .ContentType = "application/vnd.ms-excel"
                        .BinaryWrite(oStream.ToArray())

                        Dim stringWrite As StringWriter = New StringWriter(oStream.ToArray())
                        Dim htmlWrite As HtmlTextWriter = New HtmlTextWriter(stringWrite)
                        .Write(stringWrite.ToString())
                        .End()
                    End With
                End If

                'Else
                '    oRpt = CType(Session("RFDStatusReport"), ReportDocument)

                '    'Dim oStream As New System.IO.MemoryStream
                '    'oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                '    'Response.Clear()
                '    'Response.Buffer = True
                '    'Response.ContentType = "application/pdf"

                '    ''this opens immediately
                '    'Response.Charset = ""
                '    'Response.AddHeader("content-disposition", "inline;filename=RFD-Status-Report.pdf")

                '    'Response.BinaryWrite(oStream.ToArray())
                '    ''Response.End()
                '    Dim oStream As New System.IO.MemoryStream
                '    oStream = oRpt.ExportToStream(ExportFormatType.Excel)
                '    With HttpContext.Current.Response
                '        .Clear()
                '        .AddHeader("content-disposition", "attachment; filename=RFD-Status-Report_" & Tdate & ".xls")
                '        .Charset = ""
                '        .Cache.SetCacheability(HttpCacheability.NoCache)
                '        .ContentType = "application/vnd.xls"
                '        '.ContentType = "application/vnd.ms-excel"
                '        .BinaryWrite(oStream.ToArray())

                '        Dim stringWrite As StringWriter = New StringWriter(oStream.ToArray())
                '        Dim htmlWrite As HtmlTextWriter = New HtmlTextWriter(stringWrite)
                '        .Write(stringWrite.ToString())
                '        .End()
                '    End With
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

        'in order to clear crystal reports for Costing Preview
        If HttpContext.Current.Session("RFDStatusReport") IsNot Nothing Then
            Dim tempRpt As ReportDocument = New ReportDocument()
            tempRpt = CType(HttpContext.Current.Session("RFDStatusReport"), ReportDocument)
            If tempRpt IsNot Nothing Then
                tempRpt.Close()
                tempRpt.Dispose()
            End If
            HttpContext.Current.Session("RFDStatusReport") = Nothing         
            GC.Collect()
        End If

    End Sub
End Class
