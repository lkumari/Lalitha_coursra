' ************************************************************************************************
' Name:	DrawingPackagingPreview.aspx.vb
' Purpose:	This program is used to call crystal viewer and get data from Drawing_Maint, regarding the Drawing Packaging Information
'
' Date		    Author	    
' 10/23/2008    Roderick Carlson		    Created .Net application
' 07/28/2009    Roderick Carlson            Modified: Converted to PopUp
' 06/16/2010    Roderick Carlson            Modified: send view to PDF immediately
' ************************************************************************************************
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Partial Class PE_DrawingPackagingPreview
    Inherits System.Web.UI.Page

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try

            If HttpContext.Current.Request.QueryString("DrawingNo") <> "" Then

                Dim oRpt As ReportDocument = New ReportDocument()

                If (Session("DrawingPackagingPreview") Is Nothing) Then
                    ViewState("DrawingNo") = HttpContext.Current.Request.QueryString("DrawingNo")

                    'Dim crTable As CrystalDecisions.CrystalReports.Engine.Table
                    'Dim crDatabase As CrystalDecisions.CrystalReports.Engine.Database
                    'Dim dbConn As TableLogOnInfo = New TableLogOnInfo()

                    ' new report document object 
                    oRpt.Load(Server.MapPath(".\Forms\") & "DrawingPackagingPreview.rpt")

                    ''getting the database, the table and the LogOnInfo object which holds login onformation 
                    'crDatabase = oRpt.Database

                    ''getting the table in an object array of one item 
                    'Dim arrTables(2) As CrystalDecisions.CrystalReports.Engine.Table
                    'crDatabase.Tables.CopyTo(arrTables, 0)
                    '' assigning the first item of array to crTable by downcasting the object to Table 
                    'crTable = arrTables(0)

                    ''setting(Values)
                    'dbConn = crTable.LogOnInfo
                    'dbConn.ConnectionInfo.DatabaseName = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString() '"Test_UGNDB" OR "UGNDB"
                    'dbConn.ConnectionInfo.ServerName = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString() '"DB_Server" and soon SQLCLUSTERVS
                    'dbConn.ConnectionInfo.UserID = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
                    'dbConn.ConnectionInfo.Password = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString

                    '' applying login info to the table object 
                    'crTable.ApplyLogOnInfo(dbConn)

                    '' defining report source 
                    'crDrawingPackagingPreview.DisplayGroupTree = False
                    'crDrawingPackagingPreview.ReportSource = oRpt

                    ''so uptill now we have created everything 
                    ''what remains is to pass parameters to our report, so it 
                    ''shows only selected records. so calling a method to set 
                    ''those parameters. 

                    ''Check if there are parameters or not in report.
                    'Dim intCounter As Integer = oRpt.DataDefinition.ParameterFields.Count

                    'setReportParameters()


                    Dim strDatabaseName As String = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString() '"Test_UGNDB" OR "UGNDB"
                    Dim strServerName As String = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString() '"DB_Server" and soon SQLCLUSTERVS
                    Dim strUserID As String = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
                    Dim strPassword As String = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString

                    oRpt.SetDatabaseLogon(strUserID, strPassword, strServerName, strDatabaseName)                  
                    oRpt.SetParameterValue("@DrawingNo", ViewState("DrawingNo"))

                    Session("DrawingPackagingPreview") = oRpt

                    Dim oStream As New System.IO.MemoryStream
                    oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                    Response.Clear()
                    Response.Buffer = True
                    Response.ContentType = "application/pdf"

                    Response.Charset = ""
                    Response.AddHeader("content-disposition", "inline;filename=DMS-Packaging-" & ViewState("DrawingNo").ToString & "preview.pdf")

                    Response.BinaryWrite(oStream.ToArray())
                    'Response.End()
                Else
                    oRpt = CType(Session("DrawingPackagingPreview"), ReportDocument)

                    'crDrawingPackagingPreview.ReportSource = oRpt

                    Dim oStream As New System.IO.MemoryStream
                    oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                    Response.Clear()
                    Response.Buffer = True
                    Response.ContentType = "application/pdf"

                    Response.Charset = ""
                    Response.AddHeader("content-disposition", "inline;filename=DMS-Packaging-" & ViewState("DrawingNo").ToString & "preview.pdf")

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

    'Private Sub setReportParameters()

    '    Try
    '        ' all the parameter fields will be added to this collection 
    '        Dim paramFields As New ParameterFields

    '        ' the parameter fields to be sent to the report 
    '        Dim pfDrawingNo As ParameterField = New ParameterField

    '        ' setting the name of parameter fields with wich they will be received in report 
    '        pfDrawingNo.ParameterFieldName = "@drawingNo"

    '        ' the above declared parameter fields accept values as discrete objects 
    '        ' so declaring discrete objects 
    '        Dim dcDrawingNo As New ParameterDiscreteValue

    '        ' setting the values of discrete objects 
    '        dcDrawingNo.Value = ViewState("DrawingNo")

    '        ' now adding these discrete values to parameters 
    '        pfDrawingNo.CurrentValues.Add(dcDrawingNo)

    '        ' now adding all these parameter fields to the parameter collection 
    '        paramFields.Add(pfDrawingNo)

    '        ' finally add the parameter collection to the crystal report viewer 
    '        crDrawingPackagingPreview.ParameterFieldInfo = paramFields

    '    Catch ex As Exception
    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text += ex.Message & "<br>" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

        Try
            'in order to clear crystal reports
            If HttpContext.Current.Session("DrawingPackagingPreview") IsNot Nothing Then
                Dim tempRpt As ReportDocument = New ReportDocument()
                tempRpt = CType(HttpContext.Current.Session("DrawingPackagingPreview"), ReportDocument)
                If tempRpt IsNot Nothing Then
                    tempRpt.Close()
                    tempRpt.Dispose()
                End If
                HttpContext.Current.Session("DrawingPackagingPreview") = Nothing
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
