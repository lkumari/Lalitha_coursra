' ************************************************************************************************
'
' Name:		UGN_IPP_Preview
' Purpose:	This Code Behind is for the UGN IPP Previews in Crystal Reports
'
' Date		    Author	    
' 08/04/2009    Roderick Carlson    Created
' 06/16/2010    Roderick Carlson    Modified: send view to PDF immediately
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class UGN_IPP_Preview
    Inherits System.Web.UI.Page
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            Dim oRpt As ReportDocument = New ReportDocument()

            If HttpContext.Current.Request.QueryString("ECINo") <> "" Then

                ViewState("ECINo") = CType(HttpContext.Current.Request.QueryString("ECINo"), Integer)

                If Session("UgnIppPreviewECINo") <> ViewState("ECINo") Then
                    Session("UgnIppPreview") = Nothing
                    Session("UgnIppPreviewECINo") = Nothing
                End If

                If (Session("UgnIppPreview") Is Nothing) Then
                    '' new report document object 
                    If ViewState("ECINo") >= 200000 Then
                        oRpt.Load(Server.MapPath(".\Forms\") & "UgnIpp.rpt")
                    Else
                        oRpt.Load(Server.MapPath(".\Forms\") & "IppArchive.rpt")
                    End If

                    Dim strDatabaseName As String = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString() '"Test_UGNDB" OR "UGNDB"
                    Dim strServerName As String = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString() '"DB_Server" and soon SQLCLUSTERVS
                    Dim strUserID As String = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
                    Dim strPassword As String = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString

                    oRpt.SetDatabaseLogon(strUserID, strPassword, strServerName, strDatabaseName)
                    oRpt.SetParameterValue("@ECINo", ViewState("ECINo"))
                    oRpt.SetParameterValue("@SOPNo", "IPP-EXBT")

                    Session("UgnIppPreviewECINo") = ViewState("ECINo")
                    Session("UgnIppPreview") = oRpt

                    Dim oStream As New System.IO.MemoryStream
                    oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                    Response.Clear()
                    Response.Buffer = True
                    Response.ContentType = "application/pdf"
                    Response.Charset = ""
                    Response.AddHeader("content-disposition", "inline;filename=IPP-" & ViewState("ECINo").ToString & "preview.pdf")
                    Response.BinaryWrite(oStream.ToArray())
                    'Response.End()
                Else
                    oRpt = CType(Session("UgnIppPreview"), ReportDocument)
                    Dim oStream As New System.IO.MemoryStream
                    oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                    Response.Clear()
                    Response.Buffer = True
                    Response.ContentType = "application/pdf"
                    Response.Charset = ""
                    Response.AddHeader("content-disposition", "inline;filename=IPP-" & ViewState("ECINo").ToString & "preview.pdf")
                    Response.BinaryWrite(oStream.ToArray())
                    'Response.End()
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

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

        Try
            'in order to clear crystal reports
            If HttpContext.Current.Session("UgnIppPreview") IsNot Nothing Then
                Dim tempRpt As ReportDocument = New ReportDocument()
                tempRpt = CType(HttpContext.Current.Session("UgnIppPreview"), ReportDocument)
                If tempRpt IsNot Nothing Then
                    tempRpt.Close()
                    tempRpt.Dispose()
                End If
                HttpContext.Current.Session("UgnIppPreviewECINo") = Nothing
                HttpContext.Current.Session("UgnIppPreview") = Nothing
                GC.Collect()
            End If
        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
End Class
