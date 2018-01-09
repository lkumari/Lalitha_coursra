' ************************************************************************************************
'
' Name:		crPreview_AR_Event_Detail.aspx
' Purpose:	This Code Behind is for to preview and AR Event in crystal reports
'
' Date		Author	    
' 04/06/2010   Roderick Carlson

Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class crPreview_AR_Event_Detail
    Inherits System.Web.UI.Page

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try

            ViewState("AREID") = HttpContext.Current.Request.QueryString("AREID")

            If ViewState("AREID") <> "" Then

                If HttpContext.Current.Request.Cookies("UGNDB_User") Is Nothing Or Response.Cookies("UGNDB_UserFullName") Is Nothing Then
                    ' commonFunctions.SetUGNDBUser()
                    Dim FullName As String = commonFunctions.getUserName()
                    Dim UserEmailAddress As String = FullName & "@ugnauto.com"
                    Response.Cookies("UGNDB_User_Email").Value = UserEmailAddress
                    If FullName = Nothing Then
                        FullName = "Demo.Demo"  '* This account has restricted read only rights.
                    End If
                    Dim LocationOfDot As Integer = InStr(FullName, ".")
                    If LocationOfDot > 0 Then
                        Dim FirstName As String = Left(FullName, LocationOfDot - 1)
                        Dim FirstInitial As String = Left(FullName, 1)
                        Dim LastName As String = Right(FullName, Len(FullName) - LocationOfDot)

                        Response.Cookies("UGNDB_User").Value = FirstInitial & LastName
                        Response.Cookies("UGNDB_UserFullName").Value = Mid(FirstName, 1, 1).ToUpper & Mid(FirstName, 2, Len(FirstName) - 1) & " " & Mid(LastName, 1, 1).ToUpper & Mid(LastName, 2, Len(LastName) - 1)
                    Else
                        Response.Cookies("UGNDB_User").Value = FullName
                        Response.Cookies("UGNDB_UserFullName").Value = FullName

                    End If
                End If


                Dim oRpt As ReportDocument = New ReportDocument()

                If ViewState("AREID") <> Session("ARPreviewAREID") Then
                    Session("ARPreviewAREID") = Nothing
                    Session("ARPreview") = Nothing
                End If

                If Session("ARPreview") Is Nothing Then

                    'Dim crTable As CrystalDecisions.CrystalReports.Engine.Table
                    'Dim crDatabase As CrystalDecisions.CrystalReports.Engine.Database
                    'Dim dbConn As TableLogOnInfo = New TableLogOnInfo()

                    ' new report document object 
                    oRpt.Load(Server.MapPath(".\Forms\") & "crAREvent.rpt")

                    ''getting the database, the table and the LogOnInfo object which holds login onformation 
                    'crDatabase = oRpt.Database

                    ''getting the table in an object array of one item 
                    'Dim arrTables(2) As CrystalDecisions.CrystalReports.Engine.Table
                    'crDatabase.Tables.CopyTo(arrTables, 0)
                    '' assigning the first item of array to crTable by downcasting the object to Table 
                    'crTable = arrTables(0)

                    '' setting values 
                    'dbConn = crTable.LogOnInfo
                    'dbConn.ConnectionInfo.DatabaseName = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString()
                    'dbConn.ConnectionInfo.ServerName = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString()
                    'dbConn.ConnectionInfo.UserID = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
                    'dbConn.ConnectionInfo.Password = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString

                    '' applying login info to the table object 
                    'crTable.ApplyLogOnInfo(dbConn)

                    '' defining report source 
                    'CrystalReportViewer1.DisplayGroupTree = False
                    'CrystalReportViewer1.ReportSource = oRpt
                    'Session("TempCrystalRptFiles") = oRpt

                    ' so uptil now we have created everything 
                    ' what remains is to pass parameters to our report, so it 
                    ' shows only selected records. so calling a method to set 
                    ' those parameters. 

                    ''Check if there are parameters or not in report.
                    'Dim intCounter As Integer = oRpt.DataDefinition.ParameterFields.Count
                    'setReportParameters()

                    Dim strDatabaseName As String = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString() '"Test_UGNDB" OR "UGNDB"
                    Dim strServerName As String = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString() '"DB_Server" and soon SQLCLUSTERVS
                    Dim strUserID As String = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
                    Dim strPassword As String = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString

                    oRpt.SetDatabaseLogon(strUserID, strPassword, strServerName, strDatabaseName)
                    oRpt.SetParameterValue("@AREID", ViewState("AREID"))

                    Session("ARPreviewAREID") = ViewState("AREID")
                    Session("ARPreview") = oRpt

                    Dim oStream As New System.IO.MemoryStream
                    oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                    Response.Clear()
                    Response.Buffer = True
                    Response.ContentType = "application/pdf"
                    Response.Charset = ""
                    Response.AddHeader("content-disposition", "inline;filename=AR-" & ViewState("AREID").ToString & "preview.pdf")

                    Response.BinaryWrite(oStream.ToArray())
                Else
                    oRpt = CType(Session("ARPreview"), ReportDocument)

                    Dim oStream As New System.IO.MemoryStream
                    oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                    Response.Clear()
                    Response.Buffer = True
                    Response.ContentType = "application/pdf"
                    Response.AddHeader("content-disposition", "inline;filename=AR-" & ViewState("AREID").ToString & "preview.pdf")
                    Response.BinaryWrite(oStream.ToArray())
                    'Response.End()

                    'CrystalReportViewer1.ReportSource = oRpt
                End If

            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            'UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try


    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            'CheckRights()
            If Not Page.IsPostBack Then

                If HttpContext.Current.Request.Cookies("UGNDB_User") Is Nothing Or Response.Cookies("UGNDB_UserFullName") Is Nothing Then
                    ' commonFunctions.SetUGNDBUser()
                    Dim FullName As String = commonFunctions.getUserName()
                    Dim UserEmailAddress As String = FullName & "@ugnauto.com"
                    Response.Cookies("UGNDB_User_Email").Value = UserEmailAddress
                    If FullName = Nothing Then
                        FullName = "Demo.Demo"  '* This account has restricted read only rights.
                    End If
                    Dim LocationOfDot As Integer = InStr(FullName, ".")
                    If LocationOfDot > 0 Then
                        Dim FirstName As String = Left(FullName, LocationOfDot - 1)
                        Dim FirstInitial As String = Left(FullName, 1)
                        Dim LastName As String = Right(FullName, Len(FullName) - LocationOfDot)

                        Response.Cookies("UGNDB_User").Value = FirstInitial & LastName
                        Response.Cookies("UGNDB_UserFullName").Value = Mid(FirstName, 1, 1).ToUpper & Mid(FirstName, 2, Len(FirstName) - 1) & " " & Mid(LastName, 1, 1).ToUpper & Mid(LastName, 2, Len(LastName) - 1)
                    Else
                        Response.Cookies("UGNDB_User").Value = FullName
                        Response.Cookies("UGNDB_UserFullName").Value = FullName

                    End If
                End If
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    'Private Sub setReportParameters()
    '    Try
    '        ' all the parameter fields will be added to this collection 
    '        Dim paramFields As New ParameterFields

    '        ' the parameter fields to be sent to the report 
    '        Dim pfAREID As ParameterField = New ParameterField

    '        ' setting the name of parameter fields with which they will be received in report 
    '        pfAREID.ParameterFieldName = "@AREID"

    '        ' the above declared parameter fields accept values as discrete objects 
    '        ' so declaring discrete objects 
    '        Dim dcAREID As New ParameterDiscreteValue

    '        ' setting the values of discrete objects 
    '        dcAREID.Value = ViewState("AREID")

    '        ' now adding these discrete values to parameters 
    '        pfAREID.CurrentValues.Add(dcAREID)

    '        ' now adding all these parameter fields to the parameter collection 
    '        paramFields.Add(pfAREID)

    '        ' finally add the parameter collection to the crystal report viewer 
    '        CrystalReportViewer1.ParameterFieldInfo = paramFields

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text &= "Error found in parameter search: " & ex.Message & "<br />" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

    '    End Try
    'End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

        Try
            'in order to clear crystal reports
            If HttpContext.Current.Session("ARPreview") IsNot Nothing Then
                Dim tempRpt As ReportDocument = New ReportDocument()
                tempRpt = CType(HttpContext.Current.Session("ARPreview"), ReportDocument)
                If tempRpt IsNot Nothing Then
                    tempRpt.Close()
                    tempRpt.Dispose()
                End If
                HttpContext.Current.Session("ARPreviewAREID") = Nothing
                HttpContext.Current.Session("ARPreview") = Nothing
                GC.Collect()
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text &= ex.Message & "<br />" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub
End Class
