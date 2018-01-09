' ************************************************************************************************
'
' Name:		crSupport_Preview
' Purpose:	This Code Behind is for the RFD Previews in Crystal Reports
'
' Date		    Author	    
' 01/17/2012    Roderick Carlson    Created

Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class crSupport_Preview
    Inherits System.Web.UI.Page

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            Dim ds As DataSet
            Dim oRpt As ReportDocument = New ReportDocument()

            If HttpContext.Current.Request.QueryString("JobNumber") <> "" Then

                If HttpContext.Current.Request.Cookies("UGNDB_User") Is Nothing Then
                    commonFunctions.SetUGNDBUser()
                End If

                ViewState("JobNumber") = HttpContext.Current.Request.QueryString("JobNumber")

                ds = SupportModule.GetSupportRequest(ViewState("JobNumber"))
            
                If commonFunctions.CheckDataSet(ds) = True Then

                    If Session("SupportPreviewJobNumber") <> ViewState("JobNumber") Then
                        Session("SupportPreview") = Nothing
                        Session("SupportPreviewJobNumber") = Nothing
                    End If

                    If (Session("SupportPreview") Is Nothing) Then

                      
                        oRpt.Load(Server.MapPath(".\Forms\") & "Support.rpt")

                 
                        Dim strDatabaseName As String = System.Configuration.ConfigurationManager.AppSettings("DBSupport").ToString() 'Test_DBRequests or DBRequests
                        Dim strServerName As String = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString() 'SQLCLUSTERVS
                        Dim strUserID As String = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
                        Dim strPassword As String = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString

                        oRpt.SetDatabaseLogon(strUserID, strPassword, strServerName, strDatabaseName)
                        oRpt.SetParameterValue("@JobNumber", ViewState("JobNumber"))

                        Session("SupportPreviewJobNumber") = ViewState("JobNumber")
                        Session("SupportPreview") = oRpt

                        Dim oStream As New System.IO.MemoryStream
                        oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                        Response.Clear()
                        Response.Buffer = True
                        Response.ContentType = "application/pdf"

                        'this opens immediately
                        Response.Charset = ""
                        Response.AddHeader("content-disposition", "inline;filename=Support-" & ViewState("JobNumber").ToString & "preview.pdf")

                        Response.BinaryWrite(oStream.ToArray())
                        'Response.End()

                    Else
                        oRpt = CType(Session("SupportPreview"), ReportDocument)

                        'crECIPreview.ReportSource = oRpt
                        Dim oStream As New System.IO.MemoryStream
                        oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                        Response.Clear()
                        Response.Buffer = True
                        Response.ContentType = "application/pdf"

                        'this opens immediately
                        Response.Charset = ""
                        Response.AddHeader("content-disposition", "inline;filename=Support-" & ViewState("JobNumber").ToString & "preview.pdf")

                        Response.BinaryWrite(oStream.ToArray())
                        'Response.End()

                    End If
                End If

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
        If HttpContext.Current.Session("SupportPreview") IsNot Nothing Then
            Dim tempRpt As ReportDocument = New ReportDocument()
            tempRpt = CType(HttpContext.Current.Session("SupportPreview"), ReportDocument)
            If tempRpt IsNot Nothing Then
                tempRpt.Close()
                tempRpt.Dispose()
            End If
            HttpContext.Current.Session("SupportPreview") = Nothing
            HttpContext.Current.Session("SupportPreviewJobNumber") = Nothing
            GC.Collect()
        End If

    End Sub
End Class
