' ************************************************************************************************
'
' Name:		Customer_IPP_Preview
' Purpose:	This Code Behind is for the Customer IPP Previews in Crystal Reports
'
' Date		    Author	    
' 08/05/2009    Roderick Carlson    Created
' 06/16/2010    Roderick Carlson    Modified: send view to PDF immediately
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class Customer_IPP_Preview
    Inherits System.Web.UI.Page
    'Private Sub setReportParameters()

    '    Try
    '        ' all the parameter fields will be added to this collection 
    '        Dim paramFields As New ParameterFields

    '        ' the parameter fields to be sent to the report 
    '        Dim pfECINo As ParameterField = New ParameterField
    '        Dim pfSOPNo As ParameterField = New ParameterField

    '        ' setting the name of parameter fields with wich they will be received in report 
    '        pfECINo.ParameterFieldName = "@ECINo"
    '        pfSOPNo.ParameterFieldName = "@SOPNo"

    '        ' the above declared parameter fields accept values as discrete objects 
    '        ' so declaring discrete objects 
    '        Dim dcECINo As New ParameterDiscreteValue
    '        Dim dcSOPNo As New ParameterDiscreteValue

    '        ' setting the values of discrete objects 
    '        dcECINo.Value = ViewState("ECINo")
    '        dcSOPNo.Value = "QA-101f"

    '        ' now adding these discrete values to parameters 
    '        pfECINo.CurrentValues.Add(dcECINo)
    '        pfSOPNo.CurrentValues.Add(dcSOPNo)

    '        ' now adding all these parameter fields to the parameter collection 
    '        paramFields.Add(pfECINo)
    '        paramFields.Add(pfSOPNo)

    '        ' finally add the parameter collection to the crystal report viewer 
    '        crCustomerIppPreview.ParameterFieldInfo = paramFields

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text += ex.Message & "<br>" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            Dim oRpt As ReportDocument = New ReportDocument()

            If HttpContext.Current.Request.QueryString("ECINo") <> "" Then

                ViewState("ECINo") = CType(HttpContext.Current.Request.QueryString("ECINo"), Integer)

                If Session("CustomerIppPreviewECINo") <> ViewState("ECINo") Then
                    Session("CustomerIppPreview") = Nothing
                    Session("CustomerIppPreviewECINo") = Nothing
                End If

                If (Session("CustomerIppPreview") Is Nothing) Then

                    'Dim crTable As CrystalDecisions.CrystalReports.Engine.Table
                    'Dim crDatabase As CrystalDecisions.CrystalReports.Engine.Database
                    'Dim dbConn As TableLogOnInfo = New TableLogOnInfo()

                    ' new report document object 

                    If ViewState("ECINo") >= 200000 Then
                        oRpt.Load(Server.MapPath(".\Forms\") & "CustomerIpp.rpt")
                    Else
                        oRpt.Load(Server.MapPath(".\Forms\") & "IppArchive.rpt")
                    End If


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
                    'crCustomerIppPreview.DisplayGroupTree = False
                    'crCustomerIppPreview.ReportSource = oRpt

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
                    oRpt.SetParameterValue("@ECINo", ViewState("ECINo"))
                    oRpt.SetParameterValue("@SOPNo", "QA-101f")

                    Session("CustomerIppPreviewECINo") = ViewState("ECINo")
                    Session("CustomerIppPreview") = oRpt

                    Dim oStream As New System.IO.MemoryStream
                    oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                    Response.Clear()
                    Response.Buffer = True
                    Response.ContentType = "application/pdf"
                    Response.Charset = ""
                    Response.AddHeader("content-disposition", "inline;filename=Customer-IPP-" & ViewState("ECINo").ToString & "preview.pdf")
                    Response.BinaryWrite(oStream.ToArray())
                    'Response.End()
                Else
                    oRpt = CType(Session("CustomerIppPreview"), ReportDocument)

                    'crCustomerIppPreview.ReportSource = oRpt
                    Dim oStream As New System.IO.MemoryStream
                    oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                    Response.Clear()
                    Response.Buffer = True
                    Response.ContentType = "application/pdf"
                    Response.Charset = ""
                    Response.AddHeader("content-disposition", "inline;filename=Customer-IPP-" & ViewState("ECINo").ToString & "preview.pdf")
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
            If HttpContext.Current.Session("CustomerIppPreview") IsNot Nothing Then
                Dim tempRpt As ReportDocument = New ReportDocument()
                tempRpt = CType(HttpContext.Current.Session("CustomerIppPreview"), ReportDocument)
                If tempRpt IsNot Nothing Then
                    tempRpt.Close()
                    tempRpt.Dispose()
                End If
                Session("CustomerIppPreviewECINo") = Nothing
                HttpContext.Current.Session("CustomerIppPreview") = Nothing
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
