' ************************************************************************************************
' Name:	CostingCostSheetPreview.aspx.vb
' Purpose:	This program is used to call crystal viewer and get data from Drawing, Drawing Images, and Display an entire Bill of Materials (BOM)
'
' Date		    Author	    
' 03/06/2009    Roderick Carlson			Created .Net application
' 11/03/2009    Roderick Carlson            Modified: network rights problem - missing parameters added
' 11/19/2009    Roderick Carlson            Modified: added security for Die Layout View only users
' 06/16/2010    Roderick Carlson            Modified: send view to PDF immediately
' 08/01/2011    Roderick Carlson            Modified: Refresh User Name cookies if blank, such as clicking on a link in email
' 06/06/2012    Roderick Carlson            Modified: Allow a non-financial view of Cost Sheet if the team member is NOT an approver
' ************************************************************************************************
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared


Partial Class Costing_Cost_Sheet_Preview
    Inherits System.Web.UI.Page
    'Protected Sub EnableControls()

    '    Try
    '        lblFileName.Visible = ViewState("isAdmin")
    '        txtFileName.Visible = ViewState("isAdmin")
    '        txtFileName.Text = ViewState("CostSheetID") & "-" & ViewState("ShowPartNo") & ".pdf"
    '        btnCreate.Visible = ViewState("isAdmin")

    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text += ex.Message & "<br>" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub
    Protected Sub CheckRights()

        Try
            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim dsSubscription As DataSet

            Dim iTeamMemberID As Integer = 0
            Dim iRoleID As Integer = 0

            ViewState("isRestricted") = True
            ViewState("isAdmin") = False
            ViewState("isDieLayoutOnly") = False
            ViewState("SubscriptionID") = 0

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, True, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataSet(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                'If iTeamMemberID = 530 Then
                '    iTeamMemberID = 32 'Dan Cade
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
                '    'iTeamMemberID = 691 'Dory Moeller
                'End If

                'Die Layout View
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 2)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 2
                    ViewState("isDieLayoutOnly") = True
                End If

                'Program Management
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 31)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 31
                End If

                'Purchasing
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 7)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 7
                End If

                'Accounting 21
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 21)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 21
                End If

                'Plant Controller 20
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 20)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 20
                End If

                'VP Sales 23
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 23)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 23
                End If

                'CEO 24
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 24)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 24
                End If

                'CFO 33
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 33)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 33
                End If

                'CST Costing Coordinator
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 41)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 41
                End If

                'CST Corporate Engineering
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 42)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 42
                End If

                'CST Plant Manager
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 43)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 43
                End If

                'CST(Purchasing)
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 44)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 44
                End If

                'CST Product Development
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 45)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 45
                End If

                'CST Sales
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 46)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 46
                End If

                'CST VP of Operations
                dsSubscription = SecurityModule.GetTMWorkHistory(iTeamMemberID, 47)
                If commonFunctions.CheckDataSet(dsSubscription) = True Then
                    ViewState("SubscriptionID") = 47
                End If

                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 57)

                If commonFunctions.CheckDataSet(dsRoleForm) = True Then

                    iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                    Select Case iRoleID
                        Case 11 '*** UGNAdmin: Full Access
                            ViewState("isAdmin") = True
                            ViewState("isRestricted") = False
                            ViewState("SubscriptionID") = 6
                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                            ViewState("isAdmin") = True
                            ViewState("isRestricted") = False
                            ViewState("SubscriptionID") = 6
                        Case 13 '*** UGNAssist: Create/Edit/No Delete
                            ViewState("isAdmin") = True
                            ViewState("isRestricted") = False
                            ViewState("SubscriptionID") = 6
                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                            ViewState("isRestricted") = False
                        Case 15 '*** UGNEdit: No Create/Edit/No Delete
                            ViewState("isRestricted") = False
                        Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                            'ViewState("isRestricted") = True
                            'show financial view for non-financial team members.
                            ViewState("SubscriptionID") = 153
                            ViewState("isRestricted") = False
                    End Select
                End If

                If ViewState("isDieLayoutOnly") = True Then
                    ViewState("isRestricted") = True
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
    '        Dim pfCostSheetID As ParameterField = New ParameterField
    '        Dim pfPreviousCostSheetID As ParameterField = New ParameterField

    '        ' setting the name of parameter fields with wich they will be received in report 
    '        pfCostSheetID.ParameterFieldName = "@costSheetID"
    '        pfPreviousCostSheetID.ParameterFieldName = "@previousCostSheetID"

    '        ' the above declared parameter fields accept values as discrete objects 
    '        ' so declaring discrete objects 
    '        Dim dcCostSheetID As New ParameterDiscreteValue
    '        Dim dcPreviousCostSheetID As New ParameterDiscreteValue

    '        ' setting the values of discrete objects 
    '        dcCostSheetID.Value = ViewState("CostSheetID")
    '        dcPreviousCostSheetID.Value = ViewState("PreviousCostSheetID")

    '        ' now adding these discrete values to parameters 
    '        pfCostSheetID.CurrentValues.Add(dcCostSheetID)
    '        pfPreviousCostSheetID.CurrentValues.Add(dcPreviousCostSheetID)

    '        ' now adding all these parameter fields to the parameter collection 
    '        paramFields.Add(pfCostSheetID)
    '        paramFields.Add(pfPreviousCostSheetID)

    '        ' finally add the parameter collection to the crystal report viewer 
    '        'crCostFormPreview.ParameterFieldInfo = paramFields

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

            If HttpContext.Current.Request.Cookies("UGNDB_User") Is Nothing Then
                commonFunctions.SetUGNDBUser()
            End If

            CheckRights()

            If ViewState("isRestricted") = False Then

                If HttpContext.Current.Request.QueryString("CostSheetID") <> "" Then

                    Dim oRpt As ReportDocument = New ReportDocument()

                    ViewState("CostSheetID") = HttpContext.Current.Request.QueryString("CostSheetID")

                    If Session("CostSheetPreviewID") <> ViewState("CostSheetID") Then
                        Session("CostSheetPreview") = Nothing
                        Session("CostSheetPreviewID") = Nothing
                    End If

                    If (Session("CostSheetPreview") Is Nothing) Then

                        Dim ds As DataSet = CostingModule.GetCostSheet(ViewState("CostSheetID"))

                        ViewState("PreviousCostSheetID") = 0

                        If commonFunctions.CheckDataSet(ds) = True Then

                            ViewState("PreviousCostSheetID") = ds.Tables(0).Rows(0).Item("PreviousCostSheetID")
                            ViewState("ShowPartNo") = ds.Tables(0).Rows(0).Item("ShowPartNo")

                            'EnableControls()

                        End If

                        'Dim crTable As CrystalDecisions.CrystalReports.Engine.Table
                        'Dim crDatabase As CrystalDecisions.CrystalReports.Engine.Database
                        'Dim dbConn As TableLogOnInfo = New TableLogOnInfo()

                        ' new report document object 
                        oRpt.Load(Server.MapPath(".\Forms\") & "CostForm.rpt")
                     
                        'getting the database, the table and the LogOnInfo object which holds login onformation 
                        'crDatabase = oRpt.Database

                        'getting the table in an object array of one item 
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

                        ' applying login info to the table object 
                        'crTable.ApplyLogOnInfo(dbConn)

                        ' defining report source 
                        'crCostFormPreview.DisplayGroupTree = False
                        'crCostFormPreview.ReportSource = oRpt

                        'so uptill now we have created everything 
                        'what remains is to pass parameters to our report, so it 
                        'shows only selected records. so calling a method to set 
                        'those parameters. 

                        'Check if there are parameters or not in report.
                        'Dim intCounter As Integer = oRpt.DataDefinition.ParameterFields.Count

                        'setReportParameters()

                        'oRpt.SetDatabaseLogon("WebApp", "W!se@cre", "SQLCLUSTERVS", "Test_UGNDB")
                        oRpt.SetDatabaseLogon(strUserID, strPassword, strServerName, strDatabaseName)
                        oRpt.SetParameterValue("@costSheetID", ViewState("CostSheetID"))
                        oRpt.SetParameterValue("@previousCostSheetID", ViewState("PreviousCostSheetID"))
                        oRpt.SetParameterValue("@subscriptionID", ViewState("SubscriptionID"))

                        Session("CostSheetPreview") = oRpt
                        Session("CostSheetPreviewID") = ViewState("CostSheetID")

                        'crCostFormPreview.ReportSource = oRpt

                        Dim oStream As New System.IO.MemoryStream
                        oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                        Response.Clear()
                        Response.Buffer = True
                        Response.ContentType = "application/pdf"

                        Response.Charset = ""
                        Response.AddHeader("content-disposition", "inline;filename=CostSheet-" & ViewState("CostSheetID").ToString & "preview.pdf")

                        Response.BinaryWrite(oStream.ToArray())
                        ''Response.End()

                    Else
                        oRpt = CType(Session("CostSheetPreview"), ReportDocument)

                        'crCostFormPreview.ReportSource = oRpt
                        Dim oStream As New System.IO.MemoryStream
                        oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                        Response.Clear()
                        Response.Buffer = True
                        Response.ContentType = "application/pdf"

                        Response.Charset = ""
                        Response.AddHeader("content-disposition", "inline;filename=CostSheet-" & ViewState("CostSheetID").ToString & "preview.pdf")

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

    'Sub ExportReport()

    '    Try
    '        'If Request("Exp") = "Excel" Then
    '        '    Dim oStream As New System.IO.MemoryStream
    '        '    oStream = rdReport.ExportToStream(ExportFormatType.Excel)
    '        '    Response.Clear()
    '        '    Response.Buffer = True
    '        '    Response.ContentType = "application/vnd.ms-excel"
    '        '    Response.BinaryWrite(oStream.ToArray())
    '        '    Response.End()
    '        'Else
    '        Dim oStream As New System.IO.MemoryStream
    '        oStream = rdReport.ExportToStream(ExportFormatType.PortableDocFormat)
    '        Response.Clear()
    '        Response.Buffer = True
    '        Response.ContentType = "application/pdf"
    '        Response.BinaryWrite(oStream.ToArray())
    '        Response.End()
    '        'End If
    '    Catch ex As Exception

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'update error on web page
    '        lblMessage.Text += ex.Message & "<br>" & mb.Name

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub

    Protected Sub btnCreate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCreate.Click

        'Dim strDebug As String = ""

        'Try
        '    lblMessage.Text = ""

        '    Dim strFileName As String = txtFileName.Text.Trim
        '    strDebug += "Line 256<br>"

        '    If txtFileName.Text.Trim <> "" Then
        '        strFileName = strFileName.Replace(".pdf", "")
        '        strDebug += "Line 261:" & strFileName & "<br>"

        '        If strFileName.Length > 46 Then
        '            strFileName = commonFunctions.convertSpecialChar(Strings.Left(strFileName.Substring(0, strFileName.Length), 46), True) & ".pdf"
        '            strDebug += "Line 265" & strFileName & "<br>"
        '        Else
        '            strFileName = commonFunctions.convertSpecialChar(strFileName.Substring(0, strFileName.Length), True) & ".pdf"
        '            strDebug += "Line 268" & strFileName & "<br>"
        '        End If

        '        Dim oRpt As ReportDocument = CType(Session("CostSheetPreview"), ReportDocument)

        '        ' finally add the parameter collection to the crystal report viewer 
        '        oRpt.SetParameterValue("@costSheetID", ViewState("CostSheetID"))
        '        oRpt.SetParameterValue("@previousCostSheetID", ViewState("PreviousCostSheetID"))

        '        strDebug += "Line 271<br>"

        '        'set the export options to PDF
        '        Dim exportOpts As ExportOptions = oRpt.ExportOptions
        '        strDebug += "Line 276<br>"

        '        exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat
        '        strDebug += "Line 278<br>"

        '        exportOpts.ExportDestinationType = ExportDestinationType.DiskFile
        '        strDebug += "Line 282<br>"

        '        exportOpts.DestinationOptions = New DiskFileDestinationOptions()
        '        strDebug += "Line 285<br>"

        '        Dim CrFormatTypeOptions As New PdfRtfWordFormatOptions
        '        exportOpts.FormatOptions = CrFormatTypeOptions


        '        ' Set the disk file options.
        '        Dim diskOpts As New DiskFileDestinationOptions()
        '        strDebug += "Line 289<br>"

        '        CType(oRpt.ExportOptions.DestinationOptions, DiskFileDestinationOptions).DiskFileName = Server.MapPath("~/Costing/Forms/QuoteSheets/" & strFileName)
        '        strDebug += "Line 292<br>"

        '        'export the report to PDF rather than displaying the report in a viewer
        '        oRpt.Export()
        '        strDebug += "Line 296<br>"

        '        Dim PdfFileCreatedOnWebServer As String
        '        strDebug += "Line 299<br>"

        '        PdfFileCreatedOnWebServer = Server.MapPath("~/Costing/Forms/QuoteSheets/" & strFileName)
        '        strDebug += "Line 302<br>"

        '        'copy file to shared folder
        '        Dim NewCopyToMoveToShareFolder As String
        '        strDebug += "Line 306<br>"

        '        Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
        '        strDebug += "Line 309" & strProdOrTestEnvironment & "<br>"

        '        If InStr(strProdOrTestEnvironment, "Test_", CompareMethod.Text) > 0 Then
        '            strDebug += "Line 312<br>"
        '            'NewCopyToMoveToShareFolder = "\\tweb2\UGNNET_Costing_Work$\Test\" & strFileName
        '            NewCopyToMoveToShareFolder = "\\tweb2\UGNNET_Costing_Work$\Test\"
        '            strDebug += "Line 313<br>"
        '        Else
        '            strDebug += "Line 316<br>"
        '            'NewCopyToMoveToShareFolder = "\\tweb2\UGNNET_Costing_Work$\Prod\" & strFileName
        '            NewCopyToMoveToShareFolder = "\\tweb2\UGNNET_Costing_Work$\Prod\"
        '            strDebug += "Line 318<br>"
        '        End If

        '        Dim strBak As String = ""
        '        Dim NewCopyToMoveToShareFolderPlusFile As String = NewCopyToMoveToShareFolder & strFileName

        '        If System.IO.File.Exists(NewCopyToMoveToShareFolderPlusFile) = True Then
        '            strBak = NewCopyToMoveToShareFolder & "bak_" & Replace(Replace(Replace(Now().ToString, ":", ""), "/", ""), " ", "_") & "_" & strFileName
        '            System.IO.File.Move(NewCopyToMoveToShareFolderPlusFile, strBak)
        '            lblMessage.Text = "<br>The previous copy was backed up.<br>"
        '        End If

        '        'System.IO.File.Copy(PdfFileCreatedOnWebServer, NewCopyToMoveToShareFolder)
        '        System.IO.File.Copy(PdfFileCreatedOnWebServer, NewCopyToMoveToShareFolderPlusFile)
        '        strDebug += "Line 322<br>"

        '        'delete original file           
        '        If System.IO.File.Exists(PdfFileCreatedOnWebServer) = True Then
        '            strDebug += "Line 326<br>"
        '            System.IO.File.Delete(PdfFileCreatedOnWebServer)
        '            strDebug += "Line 328<br>"
        '        End If

        '        lblMessage.Text += "<br>File Saved Successfully."
        '        strDebug += "Line 332<br>"

        '        'oRpt.Close()
        '        'oRpt.Dispose()
        '        'GC.Collect()
        '        strDebug += "Line 400<br>Garbage cleanup"

        '    Else
        '        lblMessage.Text += "Error: a file name is required."
        '    End If

        '    'lblMessage.Text += "<br>" + strDebug
        'Catch ex As Exception

        '    'get current event name
        '    Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

        '    'update error on web page
        '    lblMessage.Text += ex.Message & "<br>" & mb.Name + "<br>" + strDebug

        '    'log and email error
        '    UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        'End Try

    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

        Try
            If HttpContext.Current.Session("CostSheetPreview") IsNot Nothing Then
                Dim tempRpt As ReportDocument = New ReportDocument()
                tempRpt = CType(HttpContext.Current.Session("CostSheetPreview"), ReportDocument)
                If tempRpt IsNot Nothing Then
                    tempRpt.Close()
                    tempRpt.Dispose()
                End If
                HttpContext.Current.Session("CostSheetPreview") = Nothing
                HttpContext.Current.Session("CostSheetPreviewID") = Nothing
                GC.Collect()
            End If
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name + "<br>"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
End Class
