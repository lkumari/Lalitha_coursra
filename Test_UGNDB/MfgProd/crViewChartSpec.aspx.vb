' ************************************************************************************************
' Name:	crViewChartSpec.aspx.vb
' Purpose:	This program is used to call crystal viewer and get data from ExpProj_Tooling table
'
' Date		    Author	    
' 09/29/2011    LRey			Created .Net application
' ************************************************************************************************
#Region "Directives"

Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Text
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

#End Region

Partial Class MfgProd_crViewChartSpec
    Inherits System.Web.UI.Page

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ViewState("sFac") = HttpContext.Current.Request.QueryString("sFac")
        ViewState("sOMfg") = HttpContext.Current.Request.QueryString("sOMfg")
        ViewState("sCust") = HttpContext.Current.Request.QueryString("sCust")
        ViewState("sPNo") = HttpContext.Current.Request.QueryString("sPNo")
        ViewState("sDept") = HttpContext.Current.Request.QueryString("sDept")
        ViewState("sWrkCntr") = HttpContext.Current.Request.QueryString("sWrkCntr")
        ViewState("sFormula") = HttpContext.Current.Request.QueryString("sFormula")
        ViewState("sRecStatus") = HttpContext.Current.Request.QueryString("sRecStatus")


        Try
            Dim oRpt = New ReportDocument()
            If Session("TempCrystalRptFiles") Is Nothing Then
                ' new report document object 
                oRpt.Load(Server.MapPath(".\Forms\") & "crChartSpec.rpt")
                Session("TempCrystalRptFiles") = oRpt

                Dim strDatabaseName As String = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString() '"Test_UGNDB" OR "UGNDB"
                Dim strServerName As String = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString() '"DB_Server" and soon SQLCLUSTERVS
                Dim strUserID As String = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
                Dim strPassword As String = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString
                Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
                oRpt.SetDatabaseLogon(strUserID, strPassword, strServerName, strDatabaseName)
                oRpt.SetParameterValue("@CSID", 0)
                oRpt.SetParameterValue("@UGNFacility", ViewState("sFac"))
                oRpt.SetParameterValue("@OEMManufacturer", ViewState("sOMfg"))
                oRpt.SetParameterValue("@CustLoc", ViewState("sCust"))
                oRpt.SetParameterValue("@PartNo", ViewState("sPNo"))
                oRpt.SetParameterValue("@DeptNo", IIf(ViewState("sDept") = Nothing, 0, ViewState("sDept")))
                oRpt.SetParameterValue("@WorkCenter", IIf(ViewState("sWrkCntr") = Nothing, 0, ViewState("sWrkCntr")))
                oRpt.SetParameterValue("@Formula", ViewState("sFormula"))
                oRpt.SetParameterValue("@Obsolete", ViewState("sRecStatus"))

                Session("TempCrystalRptFiles") = oRpt

                Dim Tdate As String = Replace(Date.Today.ToShortDateString, "/", "-")
                Dim oStream As New System.IO.MemoryStream
                oStream = oRpt.ExportToStream(ExportFormatType.ExcelRecord)
                With HttpContext.Current.Response
                    .Clear()
                    .AddHeader("content-disposition", "attachment; filename=PartSpec_" & Tdate & ".xls")
                    .Charset = ""
                    .Cache.SetCacheability(HttpCacheability.NoCache)
                    .ContentType = "application/vnd.xls"
                    '.ContentType = "application/vnd.ms-excel"
                    .BinaryWrite(oStream.ToArray())

                    Dim stringWrite As StringWriter = New StringWriter(oStream.ToArray())
                    Dim htmlWrite As HtmlTextWriter = New HtmlTextWriter(stringWrite)
                    .Write(stringWrite.ToString())
                    .End()
                End With
            Else
                oRpt = CType(Session("TempCrystalRptFiles"), ReportDocument)
                CrystalReportViewer1.ReportSource = oRpt
                Dim oStream As New System.IO.MemoryStream
                oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                Response.Clear()
                Response.Buffer = True
                Response.ContentType = "application/pdf"
                Response.Charset = ""
                Response.AddHeader("content-disposition", "inline;filename=Chart_Spec.xls")
                Response.BinaryWrite(oStream.ToArray())
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
