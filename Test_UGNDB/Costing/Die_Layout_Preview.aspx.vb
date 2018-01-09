' ************************************************************************************************
' Name:	Costing_Die_Layout_Preview.aspx.vb
' Purpose:	This program is used to call crystal viewer and get data from Costing Die Layout
'
' Date		    Author	    
' 04/13/2009    Roderick Carlson			Created .Net application
' 06/16/2010    Roderick Carlson            Modified: send view to PDF immediately
' 08/01/2011    Roderick Carlson            Modified - Refresh User Name cookies if blank, such as clicking on a link in email
' ************************************************************************************************
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class Costing_Die_Layout_Preview
    Inherits System.Web.UI.Page

    Protected Sub CheckRights()

        Try
            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet

            Dim iTeamMemberID As Integer = 0
            Dim iRoleID As Integer = 0

            ViewState("isRestricted") = True
            ViewState("isAdmin") = False
            'ViewState("isEdit") = False

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, True, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count > 0 And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                    'If iTeamMemberID = 530 Then
                    '    'iTeamMemberID = 32 'Dan Cade
                    '    'iTeamMemberID = 762 ' Allen Reynolds 
                    '    'iTeamMemberID = 777 'Chris Hicks 
                    '    'iTeamMemberID = 144 'Jaime.Rangel  
                    '    'iTeamMemberID = 627  'Gina.Lacny 
                    '    'iTeamMemberID = 669 'Jim.Reinking 
                    '    'iTeamMemberID = 4 ' Kenta.Shinohara 
                    '    'iTeamMemberID = 6 'Peter.Anthony 
                    '    'iTeamMemberID = 569 'Randy.Khalaf 
                    '    'iTeamMemberID = 303 'Julie.Sinchak 
                    '    iTeamMemberID = 736 'Eva.Leach 
                    'End If

                    dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 57)

                    If dsRoleForm IsNot Nothing Then
                        If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
                            iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                            Select Case iRoleID
                                Case 11 '*** UGNAdmin: Full Access
                                    ViewState("isAdmin") = True
                                    ' ViewState("isEdit") = True
                                    ViewState("isRestricted") = False
                                Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                    ViewState("isAdmin") = True
                                    'ViewState("isEdit") = True
                                    ViewState("isRestricted") = False
                                Case 13 '*** UGNAssist: Create/Edit/No Delete
                                    ViewState("isAdmin") = True
                                    'ViewState("isEdit") = True
                                    ViewState("isRestricted") = False
                                Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                    ViewState("isRestricted") = False
                                Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                    'ViewState("isEdit") = True
                                    ViewState("isRestricted") = False
                                Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                    ViewState("isRestricted") = False
                            End Select
                        End If
                    End If
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
    '        Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
    '        Dim strEnvironmentVar As String = "Prod"

    '        If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
    '            strEnvironmentVar = "Test"
    '        End If

    '        ' all the parameter fields will be added to this collection 
    '        Dim paramFields As New ParameterFields

    '        ' the parameter fields to be sent to the report 
    '        Dim pfCostSheetID As ParameterField = New ParameterField
    '        Dim pfPreviousCostSheetID As ParameterField = New ParameterField
    '        Dim pfUgndbEnvironment As ParameterField = New ParameterField
    '        Dim pfZero As ParameterField = New ParameterField

    '        ' setting the name of parameter fields with wich they will be received in report 
    '        pfCostSheetID.ParameterFieldName = "@costSheetID"
    '        pfPreviousCostSheetID.ParameterFieldName = "@previousCostSheetID"
    '        pfUgndbEnvironment.ParameterFieldName = "@ugndbEnvironment"
    '        pfZero.ParameterFieldName = "@zero"

    '        ' the above declared parameter fields accept values as discrete objects 
    '        ' so declaring discrete objects 
    '        Dim dcCostSheetID As New ParameterDiscreteValue
    '        Dim dcPreviousCostSheetID As New ParameterDiscreteValue
    '        Dim dcUgndbEnvironment As New ParameterDiscreteValue
    '        Dim dcZero As New ParameterDiscreteValue

    '        ' setting the values of discrete objects 
    '        dcCostSheetID.Value = ViewState("CostSheetID")
    '        dcPreviousCostSheetID.Value = ViewState("PreviousCostSheetID")
    '        dcUgndbEnvironment.Value = strEnvironmentVar
    '        dcZero.Value = 0

    '        ' now adding these discrete values to parameters 
    '        pfCostSheetID.CurrentValues.Add(dcCostSheetID)
    '        pfPreviousCostSheetID.CurrentValues.Add(dcPreviousCostSheetID)
    '        pfUgndbEnvironment.CurrentValues.Add(dcUgndbEnvironment)
    '        pfZero.CurrentValues.Add(dcZero)

    '        ' now adding all these parameter fields to the parameter collection 
    '        paramFields.Add(pfCostSheetID)
    '        paramFields.Add(pfPreviousCostSheetID)
    '        paramFields.Add(pfUgndbEnvironment)
    '        paramFields.Add(pfZero)

    '        ' finally add the parameter collection to the crystal report viewer 
    '        crDieLayoutPreview.ParameterFieldInfo = paramFields

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

            CheckRights()

            If ViewState("isRestricted") = False Then

                If HttpContext.Current.Request.QueryString("CostSheetID") <> "" Then
                    If HttpContext.Current.Request.Cookies("UGNDB_User") Is Nothing Then
                        commonFunctions.SetUGNDBUser()
                    End If

                    Dim oRpt As ReportDocument = New ReportDocument()

                    ViewState("CostSheetID") = HttpContext.Current.Request.QueryString("CostSheetID")

                    If Session("DieLayoutPreviewID") <> ViewState("CostSheetID") Then
                        Session("DieLayoutPreview") = Nothing
                        Session("DieLayoutPreviewID") = Nothing
                    End If

                    If (Session("DieLayoutPreview") Is Nothing) Then

                        Dim ds As DataSet = CostingModule.GetCostSheet(ViewState("CostSheetID"))

                        ViewState("PreviousCostSheetID") = 0

                        If ds IsNot Nothing Then
                            If ds.Tables.Count > 0 And ds.Tables(0).Rows.Count > 0 Then
                                ViewState("PreviousCostSheetID") = ds.Tables(0).Rows(0).Item("PreviousCostSheetID")
                            End If
                        End If

                        'Dim crTable As CrystalDecisions.CrystalReports.Engine.Table
                        'Dim crDatabase As CrystalDecisions.CrystalReports.Engine.Database
                        'Dim dbConn As TableLogOnInfo = New TableLogOnInfo()

                        ' new report document object 
                        oRpt.Load(Server.MapPath(".\Forms\") & "DieLayout.rpt")

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

                        Dim strDatabaseName As String = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString() '"Test_UGNDB" OR "UGNDB"
                        Dim strServerName As String = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString() '"DB_Server" and soon SQLCLUSTERVS
                        Dim strUserID As String = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
                        Dim strPassword As String = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString

                        Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
                        'Dim strEnvironmentVar As String = "Prod"

                        'If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
                        'strEnvironmentVar = "Test"
                        'End If

                        '' applying login info to the table object 
                        'crTable.ApplyLogOnInfo(dbConn)

                        '' defining report source 
                        'crDieLayoutPreview.DisplayGroupTree = False
                        'crDieLayoutPreview.ReportSource = oRpt

                        'so uptill now we have created everything 
                        'what remains is to pass parameters to our report, so it 
                        'shows only selected records. so calling a method to set 
                        'those parameters. 

                        ''Check if there are parameters or not in report.
                        'Dim intCounter As Integer = oRpt.DataDefinition.ParameterFields.Count

                        'setReportParameters()

                        oRpt.SetDatabaseLogon(strUserID, strPassword, strServerName, strDatabaseName)
                        oRpt.SetParameterValue("@costSheetID", ViewState("CostSheetID"))
                        oRpt.SetParameterValue("@previousCostSheetID", ViewState("PreviousCostSheetID"))
                        'oRpt.SetParameterValue("@ugndbEnvironment", strEnvironmentVar) 'strProdOrTestEnvironment
                        oRpt.SetParameterValue("@ugndbEnvironment", strProdOrTestEnvironment)
                        oRpt.SetParameterValue("@zero", 0)

                        Session("DieLayoutPreview") = oRpt
                        Session("DieLayoutPreviewID") = ViewState("CostSheetID")

                        Dim oStream As New System.IO.MemoryStream
                        oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                        Response.Clear()
                        Response.Buffer = True
                        Response.ContentType = "application/pdf"

                        Response.Charset = ""
                        Response.AddHeader("content-disposition", "inline;filename=DieLayout-" & ViewState("CostSheetID").ToString & "preview.pdf")

                        Response.BinaryWrite(oStream.ToArray())
                        'Response.End()

                    Else
                        oRpt = CType(Session("DieLayoutPreview"), ReportDocument)

                        'crDieLayoutPreview.ReportSource = oRpt
                        Dim oStream As New System.IO.MemoryStream
                        oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                        Response.Clear()
                        Response.Buffer = True
                        Response.ContentType = "application/pdf"

                        Response.Charset = ""
                        Response.AddHeader("content-disposition", "inline;filename=DieLayout-" & ViewState("CostSheetID").ToString & "preview.pdf")

                        Response.BinaryWrite(oStream.ToArray())
                        'Response.End()
                    End If
                End If
            Else
                lblMessage.Text = "You do not have access to this information. Please contact the Costing Manager."
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

        If HttpContext.Current.Session("DieLayoutPreview") IsNot Nothing Then
            Dim tempRpt As ReportDocument = New ReportDocument()
            tempRpt = CType(HttpContext.Current.Session("DieLayoutPreview"), ReportDocument)
            If tempRpt IsNot Nothing Then
                tempRpt.Close()
                tempRpt.Dispose()
            End If
            HttpContext.Current.Session("DieLayoutPreview") = Nothing
            HttpContext.Current.Session("DieLayoutPreviewID") = Nothing
            GC.Collect()
        End If

    End Sub
End Class
