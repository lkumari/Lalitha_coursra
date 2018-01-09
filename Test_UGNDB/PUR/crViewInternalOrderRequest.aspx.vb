' ************************************************************************************************
' Name:	crViewInternalOrderRequest.aspx.vb
' Purpose:	This program is used to call crystal viewer and get data from ExpProj_Assets table
'           and other sources.  Also used as an approval screen to allow users to view current
'           info related to a capex asset project and approve/reject the project in one screen.
'
' Date		    Author	    
' 09/14/2010    LRey			Created .Net application
' ************************************************************************************************
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Partial Class IOR_crViewInternalOrderRequest
    Inherits System.Web.UI.Page

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ViewState("pIORNo") = HttpContext.Current.Request.QueryString("pIORNo")
        ViewState("pProjNo") = HttpContext.Current.Request.QueryString("pProjNo")
        ViewState("pBuyer") = 0
        If HttpContext.Current.Request.QueryString("pBuyer") IsNot Nothing Then
            ViewState("pBuyer") = HttpContext.Current.Request.QueryString("pBuyer")
        Else
            ViewState("pBuyer") = 0
        End If

        If ViewState("pIORNo") <> "" Then
            Try
                Dim oRpt As ReportDocument = New ReportDocument()
                If Session("TempCrystalRptFiles") Is Nothing Then
                    ' new report document object 
                    oRpt.Load(Server.MapPath(".\Forms\") & "crInternalOrderRequest.rpt")
                    Session("TempCrystalRptFiles") = oRpt

                    Dim strDatabaseName As String = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString() '"Test_UGNDB" OR "UGNDB"
                    Dim strServerName As String = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString() '"DB_Server" and soon SQLCLUSTERVS
                    Dim strUserID As String = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
                    Dim strPassword As String = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString
                    Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
                    oRpt.SetDatabaseLogon(strUserID, strPassword, strServerName, strDatabaseName)
                    oRpt.SetParameterValue("@IORNO", ViewState("pIORNo"))
                    oRpt.SetParameterValue("BuyerTMID", ViewState("pBuyer"))
                    oRpt.SetParameterValue("@URLLocation", strProdOrTestEnvironment)
                    Session("TempCrystalRptFiles") = oRpt

                    Dim oStream As New System.IO.MemoryStream
                    oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                    Response.Clear()
                    Response.Buffer = True
                    Response.ContentType = "application/pdf"

                    Response.Charset = ""
                    Response.AddHeader("content-disposition", "inline;filename=IOR-" & ViewState("pIORNo").ToString & "preview.pdf")

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
                    Response.AddHeader("content-disposition", "inline;filename=IOR-" & ViewState("pIORNo").ToString & "preview.pdf")

                    Response.BinaryWrite(oStream.ToArray())


                End If

            Catch ex As Exception
                lblErrors.Text = "Error found in report view" & ex.Message
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
