' ************************************************************************************************
' Name:	crViewCostReductionDetail.aspx.vb
' Purpose:	This program is used to call preview cost reduction details
'
' Date		    Author	    
' 02/24/2010    Roderick Carlson			Created .Net application
' 09/01/2011    Roderick Carlson            Adjust Crystal/PDF settings to avoid user prompt when opening
' 12/10/2012    Roderick Carlson            Fix spelling error recieve to receive
' 03/03/2014    LRey                        Updated stored procedure.
' ************************************************************************************************
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Partial Class CR_crViewCostReductionDetail
    Inherits System.Web.UI.Page

    Protected Sub CheckRights()
        ''********
        '' This function is used to enable/disable controls on the form based on TM's Security/Subscription
        ''********
        Try

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim iTeamMemberID As Integer = 0

            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 97 'Cost Reduction Project Form ID
            Dim iRoleID As Integer = 0

            ViewState("isViewable") = False

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                    ''developer testing as another team member
                    'If iTeamMemberID = 530 Then
                    '    'iTeamMemberID = 612 'dan marcon                        
                    '    iTeamMemberID = 571 'adrian way                        
                    'End If

                    iWorking = dsTeamMember.Tables(0).Rows(0).Item("Working")
                    If iWorking = True Then 'Allow TM access if he/she is an active UGN Team Member
                        'Get Team Member's Role assignment
                        dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, iFormID)
                        If dsRoleForm IsNot Nothing Then
                            If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
                                iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")


                                Select Case iRoleID
                                    Case 11 '*** UGNAdmin: Full Access
                                        ViewState("ObjectRole") = True
                                        ViewState("Admin") = True
                                        ViewState("isViewable") = True
                                    Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                        ViewState("ObjectRole") = True
                                        ViewState("isViewable") = True
                                    Case 13 '*** UGNAssist: Create/Edit/No Delete
                                        ViewState("ObjectRole") = True
                                        ViewState("isViewable") = True
                                    Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                        ViewState("ObjectRole") = False
                                        ViewState("isViewable") = True
                                    Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                        ViewState("ObjectRole") = True
                                        ViewState("isViewable") = True
                                    Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                        ''** No Entry allowed **''
                                        ViewState("ObjectRole") = False
                                        ViewState("isViewable") = False
                                End Select 'EOF of "Select Case iRoleID"
                            End If 'EOF of "If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then"
                        End If 'EOF of "If dsRoleForm IsNot Nothing Then"
                    End If 'EOF of "If iWorking = True Then"
                End If 'EOF of "If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then"
            End If 'EOF of "If dsTeamMember IsNot Nothing Then"

            If HttpContext.Current.Request.Cookies("UGNDB_User") Is Nothing Then
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
                Else
                    Response.Cookies("UGNDB_User").Value = FullName
                End If
            End If

            ViewState("DefaultUser") = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += "<br>" & ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message & ", TESTING", System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try
    End Sub

    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    Dim m As ASP.crviewmasterpage_master = Master

    '    ''***********************************************
    '    ''Code Below overrides the breadcrumb navigation 
    '    ''***********************************************
    '    Dim mpTextBox As Label
    '    mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
    '    If Not mpTextBox Is Nothing Then

    '        mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Cost Reduction</b> > <a href='CostReductionList.aspx'><b>Cost Reduction Project Search</b></a> > <a href='CostReduction.aspx?pProjNo=" & ViewState("pProjNo") & "'><b>Cost Reduction Project</b></a> > <a href='CostReductionProposedDetail.aspx?pProjNo=" & ViewState("pProjNo") & "'><b>Cost Reduction Proposed Details </b></a> "
    '        mpTextBox.Visible = True
    '        Master.FindControl("SiteMapPath1").Visible = False
    '    End If

    'End Sub

    'Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

    '    ViewState("pProjNo") = 0
    '    If HttpContext.Current.Request.QueryString("pProjNo") > 0 Then
    '        ViewState("pProjNo") = HttpContext.Current.Request.QueryString("pProjNo")
    '    End If

    '    CheckRights()

    '    If ViewState("isViewable") = True Then

    '        Dim oRpt As ReportDocument = New ReportDocument()

    '        If Session("TempCrystalRptFiles") Is Nothing Then
    '            Dim crTable As CrystalDecisions.CrystalReports.Engine.Table
    '            'Dim crTbl As CrystalDecisions.CrystalReports.Engine.Table
    '            Dim crDatabase As CrystalDecisions.CrystalReports.Engine.Database
    '            Dim dbConn As TableLogOnInfo = New TableLogOnInfo()

    '            ' new report document object 
    '            oRpt.Load(Server.MapPath(".\Forms\") & "crCostReductionDetail.rpt")

    '            'getting the database, the table and the LogOnInfo object which holds login onformation 
    '            crDatabase = oRpt.Database

    '            'getting the table in an object array of one item 
    '            Dim arrTables(2) As CrystalDecisions.CrystalReports.Engine.Table
    '            crDatabase.Tables.CopyTo(arrTables, 0)
    '            ' assigning the first item of array to crTable by downcasting the object to Table 
    '            crTable = arrTables(0)

    '            ' setting values 
    '            dbConn = crTable.LogOnInfo
    '            dbConn.ConnectionInfo.DatabaseName = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString() '"Test_UGN_HR" or "UGN_HR"
    '            dbConn.ConnectionInfo.ServerName = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString() '"TAPS1"
    '            dbConn.ConnectionInfo.UserID = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
    '            dbConn.ConnectionInfo.Password = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString

    '            ' applying login info to the table object 
    '            crTable.ApplyLogOnInfo(dbConn)

    '            ' defining report source 
    '            CrystalReportViewer1.DisplayGroupTree = True
    '            CrystalReportViewer1.ReportSource = oRpt

    '            'Check if there are parameters or not in report.
    '            Dim intCounter As Integer = oRpt.DataDefinition.ParameterFields.Count

    '            setReportParameters()
    '            Session("TempCrystalRptFiles") = oRpt
    '        Else
    '            oRpt = CType(Session("TempCrystalRptFiles"), ReportDocument)

    '            CrystalReportViewer1.ReportSource = oRpt
    '        End If
    '    Else
    '        lblMessage.Text = "You do not have access to this information. Please submit a support request and get approval from the VP of Product Development to get access."
    '        CrystalReportViewer1.Visible = False
    '    End If
    'End Sub

    'Private Sub setReportParameters()

    '    Try
    '        ' all the parameter fields will be added to this collection 
    '        Dim paramFields As New ParameterFields

    '        ' the parameter fields to be sent to the report 
    '        Dim pfProjectNo As ParameterField = New ParameterField

    '        ' setting the name of parameter fields with wich they will be received in report 
    '        pfProjectNo.ParameterFieldName = "@ProjectNo"

    '        ' the above declared parameter fields accept values as discrete objects 
    '        ' so declaring discrete objects 
    '        Dim dcProjectNo As New ParameterDiscreteValue
    '        ' setting the values of discrete objects 
    '        dcProjectNo.Value = ViewState("pProjNo")

    '        ' now adding these discrete values to parameters 
    '        pfProjectNo.CurrentValues.Add(dcProjectNo)

    '        ' now adding all these parameter fields to the parameter collection 
    '        paramFields.Add(pfProjectNo)

    '        ' finally add the parameter collection to the crystal report viewer 
    '        CrystalReportViewer1.ParameterFieldInfo = paramFields

    '    Catch ex As Exception
    '        lblMessage.Text += "Error found in parameter search " & ex.Message
    '    End Try

    'End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        ViewState("pProjNo") = 0
        If HttpContext.Current.Request.QueryString("pProjNo") > 0 Then
            ViewState("pProjNo") = HttpContext.Current.Request.QueryString("pProjNo")
        End If

        CheckRights()

        If ViewState("isViewable") = True Then

            Dim oRpt As ReportDocument = New ReportDocument()

            If Session("TempCrystalRptFiles") Is Nothing Then

                ' new report document object 
                oRpt.Load(Server.MapPath(".\Forms\") & "crCostReductionDetail.rpt")
                'Session("TempCrystalRptFiles") = oRpt

                Dim strDatabaseName As String = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString() '"Test_UGNDB" OR "UGNDB"
                Dim strServerName As String = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString() '"DB_Server" and soon SQLCLUSTERVS
                Dim strUserID As String = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
                Dim strPassword As String = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString
                Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("prodOrTestURL").ToString()
                oRpt.SetDatabaseLogon(strUserID, strPassword, strServerName, strDatabaseName)
                oRpt.SetParameterValue("@ProjectNo", ViewState("pProjNo"))
                'oRpt.SetParameterValue("@URLLocation", strProdOrTestEnvironment)

                Session("TempCrystalRptFiles") = oRpt

                Dim oStream As New System.IO.MemoryStream
                oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                Response.Clear()
                Response.Buffer = True
                Response.ContentType = "application/pdf"
                Response.Charset = ""
                Response.AddHeader("content-disposition", "inline;filename=CostReduction-" & ViewState("pProjNo").ToString & "preview.pdf")
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
                Response.AddHeader("content-disposition", "inline;filename=CostSheet-" & ViewState("pProjNo").ToString & "preview.pdf")

                Response.BinaryWrite(oStream.ToArray())
            End If
        Else
            lblMessage.Text = "You do not have access to this information. Please submit a support request and get approval from the VP of Product Development to get access."
            CrystalReportViewer1.Visible = False
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
