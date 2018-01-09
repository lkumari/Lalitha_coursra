''************************************************************************************************
''Name:		DBAModule.vb
''Purpose:	This code is referenced from all vb files, mostly for the purpose of calling stored 
''          procedures or functions for the DBA_Workspace Module
''
''Date		    Author	    
''06/03/2009   LRey			Created .Net application
''************************************************************************************************

Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Text
Imports System.Xml
Imports System.IO
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.Page
Imports System.Web.UI.WebControls
Imports Microsoft.VisualBasic
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Public Class DBAModule
    Public Shared Sub DeleteDatabaseGrowthTrackingCookies()

        Try
            HttpContext.Current.Response.Cookies("DBA_DtRecFrom").Value = ""
            HttpContext.Current.Response.Cookies("DBA_DtRecFrom").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("DBA_DtRecTo").Value = ""
            HttpContext.Current.Response.Cookies("DBA_DtRecTo").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("DBA_ServerName").Value = ""
            HttpContext.Current.Response.Cookies("DBA_ServerName").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteDatabaseGrowthTrackingCookies : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> DBAModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DBA_Workspace/DatabaseGrowthTracking.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteDatabaseGrowthTrackingCookies : " & commonFunctions.convertSpecialChar(ex.Message, False), "DBAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub  'EOF DeleteLabRequestMatrixCookies
    Public Shared Sub CleanDBACrystalReports()

        Try
            Dim tempRpt As ReportDocument = New ReportDocument()

            If HttpContext.Current.Session("TempCrystalRptFiles") IsNot Nothing Then

                tempRpt = CType(HttpContext.Current.Session("TempCrystalRptFiles"), ReportDocument)
                If tempRpt IsNot Nothing Then
                    tempRpt.Close()
                    tempRpt.Dispose()
                End If
                HttpContext.Current.Session("TempCrystalRptFiles") = Nothing
                GC.Collect()
            End If

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "CleanDBACrystalReports : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> DBAModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CleanDBACrystalReports : " & commonFunctions.convertSpecialChar(ex.Message, False), "DBAModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub 'EOF CleanDBACrystalReports
End Class
