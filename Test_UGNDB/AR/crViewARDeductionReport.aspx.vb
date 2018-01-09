' ************************************************************************************************
' Name:	crViewARDeductionReport.aspx.vb
' Purpose:	This program is used to call crystal viewer and get data from AR_Deduction table
'
' Date		  Author	    
' 07/13/2012  LRey			Created .Net application
' 05/21/2013  LRey          Added a third report option.
' 12/20/2013  LRey          Replace SoldTo|CABBV with Customer.
' ************************************************************************************************
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Partial Class AR_crViewARDeductionReport
    Inherits System.Web.UI.Page

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ViewState("pARDID") = HttpContext.Current.Request.QueryString("pARDID")
        ViewState("pREFNo") = HttpContext.Current.Request.QueryString("pREFNo")
        ViewState("pSBTMID") = HttpContext.Current.Request.QueryString("pSBTMID")
        ViewState("pDCOM") = HttpContext.Current.Request.QueryString("pDCOM")
        ViewState("pDUFAC") = HttpContext.Current.Request.QueryString("pDUFAC")
        ViewState("pDCUST") = HttpContext.Current.Request.QueryString("pDCUST")
        ViewState("pDSF") = HttpContext.Current.Request.QueryString("pDSF")
        ViewState("pDST") = HttpContext.Current.Request.QueryString("pDST")
        ViewState("pDRSTS") = HttpContext.Current.Request.QueryString("pDRSTS")
        ViewState("pDRSN") = HttpContext.Current.Request.QueryString("pDRSN")
        ViewState("pCDF") = HttpContext.Current.Request.QueryString("pCDF")
        ViewState("pCDT") = HttpContext.Current.Request.QueryString("pCDT")
        ViewState("pSB") = HttpContext.Current.Request.QueryString("pSB")
        ViewState("pPNO") = HttpContext.Current.Request.QueryString("pPNO")
        ViewState("pCM") = HttpContext.Current.Request.QueryString("pCM")

        Try
            Dim oRpt As New ReportDocument()
            If Session("TempCrystalRptFiles") Is Nothing Then

                ' new report document object 
                If ViewState("pCM") = 0 Then
                    oRpt.Load(Server.MapPath(".\Forms\") & "crARDeductionReport.rpt")
                ElseIf ViewState("pCM") = 1 Then
                    oRpt.Load(Server.MapPath(".\Forms\") & "crARDeductionReasonReport.rpt")
                ElseIf ViewState("pCM") = 2 Then
                    oRpt.Load(Server.MapPath(".\Forms\") & "crARDeductionCntrMsrReport.rpt")
                End If

                Session("TempCrystalRptFiles") = oRpt

                ' so uptil now we have created everything 
                ' what remains is to pass parameters to our report, so it 
                ' shows only selected records. so calling a method to set 
                ' those parameters. 

                Dim strDatabaseName As String = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString() '"Test_UGNDB" OR "UGNDB"
                Dim strServerName As String = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString() '"DB_Server" and soon SQLCLUSTERVS
                Dim strUserID As String = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
                Dim strPassword As String = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString

                oRpt.SetDatabaseLogon(strUserID, strPassword, strServerName, strDatabaseName)
                oRpt.SetParameterValue("@ARDID", IIf(ViewState("pARDID") = Nothing, "", ViewState("pARDID")))
                oRpt.SetParameterValue("@ReferenceNo", IIf(ViewState("pREFNo") = Nothing, "", ViewState("pREFNo")))
                oRpt.SetParameterValue("@SubmittedByTMID", IIf(ViewState("pSBTMID") = Nothing, 0, ViewState("pSBTMID")))
                oRpt.SetParameterValue("@Comments", IIf(ViewState("pDCOM") = Nothing, "", ViewState("pDCOM")))
                oRpt.SetParameterValue("@UGNFacility", IIf(ViewState("pDUFAC") = Nothing, "", ViewState("pDUFAC")))
                oRpt.SetParameterValue("@Customer", IIf(ViewState("pDCUST") = Nothing, "", ViewState("pDCUST")))
                oRpt.SetParameterValue("@DateSubFrom", IIf(ViewState("pDSF") = Nothing, "", ViewState("pDSF")))
                oRpt.SetParameterValue("@DateSubTo", IIf(ViewState("pDST") = Nothing, "", ViewState("pDST")))
                oRpt.SetParameterValue("@RecStatus", IIf(ViewState("pDRSTS") = Nothing, "", ViewState("pDRSTS")))
                oRpt.SetParameterValue("@Reason", IIf(ViewState("pDRSN") = Nothing, 0, ViewState("pDRSN")))
                oRpt.SetParameterValue("@ClosedDateFrom", IIf(ViewState("pCDF") = Nothing, "", ViewState("pCDF")))
                oRpt.SetParameterValue("@ClosedDateTo", IIf(ViewState("pCDT") = Nothing, "", ViewState("pCDT")))
                oRpt.SetParameterValue("@SortBy", IIf(ViewState("pSB") = Nothing, "", ViewState("pSB")))
                oRpt.SetParameterValue("@PartNo", IIf(ViewState("pPNO") = Nothing, "", ViewState("pPNO")))

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
                If ViewState("pCM") = 0 Then
                    oRpt.ExportToHttpResponse(ExportFormatType.Excel, Response, True, "CustomerDeductionsReport_" & Tdate)
                ElseIf ViewState("pCM") = 1 Then
                    oRpt.ExportToHttpResponse(ExportFormatType.Excel, Response, True, "CustomerDeductionsReceived_" & Tdate)
                ElseIf ViewState("pCM") = 2 Then
                    oRpt.ExportToHttpResponse(ExportFormatType.Excel, Response, True, "CounterMeasureReport_" & Tdate)
                End If

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

