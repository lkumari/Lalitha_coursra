' ************************************************************************************************
' Name:	crViewSupplierRequest.aspx.vb
' Purpose:	This program is used to call crystal viewer and get data from ExpProj_Tooling table
'
' Date		    Author	    
' 09/24/2010    LRey			Created .Net application
' 06/29/2012    LRey            Removed additional param's that will not be used in this release.
' ************************************************************************************************
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Partial Class SUP_crViewSupplierRequest
    Inherits System.Web.UI.Page
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ViewState("pSUPNo") = HttpContext.Current.Request.QueryString("pSUPNo")

        If ViewState("pSUPNo") <> "" Then
            Try
                Dim oRpt = New ReportDocument()
                If Session("TempCrystalRptFiles") Is Nothing Then
                    ' new report document object 
                    oRpt.Load(Server.MapPath(".\Forms\") & "crSupplierRequest.rpt")
                    Session("TempCrystalRptFiles") = oRpt

                    Dim strDatabaseName As String = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString() '"Test_UGNDB" OR "UGNDB"
                    Dim strServerName As String = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString() '"DB_Server" and soon SQLCLUSTERVS
                    Dim strUserID As String = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
                    Dim strPassword As String = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString
                    Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
                    oRpt.SetDatabaseLogon(strUserID, strPassword, strServerName, strDatabaseName)
                    oRpt.SetParameterValue("@SUPNo", ViewState("pSUPNo"))
                    oRpt.SetParameterValue("@URLLocation", strProdOrTestEnvironment)
                    Session("TempCrystalRptFiles") = oRpt

                    Dim oStream As New System.IO.MemoryStream
                    oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                    Response.Clear()
                    Response.Buffer = True
                    Response.ContentType = "application/pdf"

                    Response.Charset = ""
                    Response.AddHeader("content-disposition", "inline;filename=Supplier Request-" & ViewState("pSUPNo").ToString & "preview.pdf")

                    Response.BinaryWrite(oStream.ToArray())
                Else
                    oRpt = CType(Session("TempCrystalRptFiles"), ReportDocument)
                    CrystalReportViewer1.ReportSource = oRpt
                    Dim oStream As New System.IO.MemoryStream
                    oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                    Response.Clear()
                    Response.Buffer = True
                    Response.ContentType = "application/pdf"
                    Response.Charset = ""
                    Response.AddHeader("content-disposition", "inline;filename=Supplier Request-" & ViewState("pSUPNo").ToString & "preview.pdf")
                    Response.BinaryWrite(oStream.ToArray())
                End If
            Catch ex As Exception
                lblErrors.Text = "Error found in report view. " & ex.Message
                lblErrors.Visible = "True"
            End Try
        End If
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

