' ************************************************************************************************
' Name:	Cost_Sheet_Activity_Preview.aspx.vb
' Purpose:	This program is used to call crystal viewer and get data from either a summary or detailed report of Team Member Activity in the Costing Module
'
' Date		    Author	    
' 05/8/2009    Roderick Carlson			Created .Net application
' ************************************************************************************************
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class Cost_Sheet_Activity_Preview
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
            ViewState("isEdit") = False

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, True, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count > 0 And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                    dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 57)

                    If dsRoleForm IsNot Nothing Then
                        If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
                            iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                            Select Case iRoleID
                                Case 11 '*** UGNAdmin: Full Access
                                    ViewState("isAdmin") = True
                                    ViewState("isEdit") = True
                                    ViewState("isRestricted") = False
                                Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                    ViewState("isAdmin") = True
                                    ViewState("isEdit") = True
                                    ViewState("isRestricted") = False
                                Case 13 '*** UGNAssist: Create/Edit/No Delete
                                    ViewState("isAdmin") = True
                                    ViewState("isEdit") = True
                                    ViewState("isRestricted") = False
                                Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                    ViewState("isRestricted") = False
                                Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                    ViewState("isEdit") = True
                                    ViewState("isRestricted") = False
                                Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                    ViewState("isRestricted") = True
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
    Private Sub setReportParameters()

        Try
            ' all the parameter fields will be added to this collection 
            Dim paramFields As New ParameterFields

            ' the parameter fields to be sent to the report 
            Dim pfStartDate As ParameterField = New ParameterField
            Dim pfEndDate As ParameterField = New ParameterField
            Dim pfUGNFacility As ParameterField = New ParameterField
            Dim pfTeamMemberID As ParameterField = New ParameterField

            ' setting the name of parameter fields with wich they will be received in report 
            pfStartDate.ParameterFieldName = "@startDate"
            pfEndDate.ParameterFieldName = "@endDate"
            pfUGNFacility.ParameterFieldName = "@ugnFacility"
            pfTeamMemberID.ParameterFieldName = "@teamMemberID"

            ' the above declared parameter fields accept values as discrete objects 
            ' so declaring discrete objects 
            Dim dcStartDate As New ParameterDiscreteValue
            Dim dcEndDate As New ParameterDiscreteValue
            Dim dcUGNFacility As New ParameterDiscreteValue
            Dim dcTeamMemberID As New ParameterDiscreteValue

            ' setting the values of discrete objects 
            dcStartDate.Value = ViewState("QuoteDateFrom")
            dcEndDate.Value = ViewState("QuoteDateTo")
            dcUGNFacility.Value = ViewState("UGNFacility")       
            dcTeamMemberID.Value = ViewState("TeamMember")


            ' now adding these discrete values to parameters 
            pfStartDate.CurrentValues.Add(dcStartDate)
            pfEndDate.CurrentValues.Add(dcEndDate)
            pfUGNFacility.CurrentValues.Add(dcUGNFacility)
            pfTeamMemberID.CurrentValues.Add(dcTeamMemberID)

            ' now adding all these parameter fields to the parameter collection 
            paramFields.Add(pfStartDate)
            paramFields.Add(pfEndDate)
            paramFields.Add(pfUGNFacility)
            paramFields.Add(pfTeamMemberID)

            ' finally add the parameter collection to the crystal report viewer 
            crActityReportPreview.ParameterFieldInfo = paramFields

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Protected Sub SetupCrystalReport(ByVal ReportType As String)

        Try
            Dim oRpt As ReportDocument = New ReportDocument()

            If (Session("CostingTeamMemberTurnAroundPreview") Is Nothing) Then

                Dim crTable As CrystalDecisions.CrystalReports.Engine.Table
                Dim crDatabase As CrystalDecisions.CrystalReports.Engine.Database
                Dim dbConn As TableLogOnInfo = New TableLogOnInfo()

                If ReportType = "Detail" Then
                    ' new report document object 
                    oRpt.Load(Server.MapPath(".\Forms\") & "TeamMemberTurnAroundTimeDetail.rpt")
                Else
                    oRpt.Load(Server.MapPath(".\Forms\") & "TeamMemberTurnAroundTimeSummary.rpt")
                End If


                'getting the database, the table and the LogOnInfo object which holds login onformation 
                crDatabase = oRpt.Database

                'getting the table in an object array of one item 
                Dim arrTables(2) As CrystalDecisions.CrystalReports.Engine.Table
                crDatabase.Tables.CopyTo(arrTables, 0)
                ' assigning the first item of array to crTable by downcasting the object to Table 
                crTable = arrTables(0)

                'setting(Values)
                dbConn = crTable.LogOnInfo
                dbConn.ConnectionInfo.DatabaseName = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString() '"Test_UGNDB" OR "UGNDB"
                dbConn.ConnectionInfo.ServerName = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString() '"DB_Server" and soon SQLCLUSTERVS
                dbConn.ConnectionInfo.UserID = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
                dbConn.ConnectionInfo.Password = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString

                ' applying login info to the table object 
                crTable.ApplyLogOnInfo(dbConn)

                ' defining report source 
                crActityReportPreview.DisplayGroupTree = False
                crActityReportPreview.ReportSource = oRpt

                'so uptill now we have created everything 
                'what remains is to pass parameters to our report, so it 
                'shows only selected records. so calling a method to set 
                'those parameters. 

                'Check if there are parameters or not in report.
                Dim intCounter As Integer = oRpt.DataDefinition.ParameterFields.Count

                setReportParameters()

                Session("CostingTeamMemberTurnAroundPreview") = oRpt
            Else
                oRpt = CType(Session("CostingTeamMemberTurnAroundPreview"), ReportDocument)

                crActityReportPreview.ReportSource = oRpt
            End If

            crActityReportPreview.Visible = True
        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" + mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            ViewState("QuoteDateFrom") = ""
            ViewState("QuoteDateTo") = ""
            ViewState("UGNFacility") = ""
            ViewState("TeamMember") = 0

            CheckRights()

            If ViewState("isRestricted") = False Then

                If HttpContext.Current.Request.QueryString("ReportType") <> "" Then

                    'If (Session("CostingTeamMemberTurnAroundPreview") IsNot Nothing) Then
                    '    Dim oRpt As ReportDocument = New ReportDocument()
                    '    oRpt = CType(Session("CostingTeamMemberTurnAroundPreview"), ReportDocument)

                    '    crActityReportPreview.ReportSource = oRpt
                    'End If

                    If HttpContext.Current.Request.QueryString("QuoteDateFrom") <> "" Then
                        ViewState("QuoteDateFrom") = HttpContext.Current.Request.QueryString("QuoteDateFrom")
                    End If

                    If HttpContext.Current.Request.QueryString("QuoteDateTo") <> "" Then
                        ViewState("QuoteDateTo") = HttpContext.Current.Request.QueryString("QuoteDateTo")
                    End If

                    If HttpContext.Current.Request.QueryString("UGNFacility") <> "" Then
                        ViewState("UGNFacility") = HttpContext.Current.Request.QueryString("UGNFacility")
                    End If

                    If HttpContext.Current.Request.QueryString("TeamMember") <> "" Then
                        If HttpContext.Current.Request.QueryString("TeamMember") > 0 Then
                            ViewState("TeamMember") = HttpContext.Current.Request.QueryString("TeamMember")
                        End If
                    End If


                    SetupCrystalReport(HttpContext.Current.Request.QueryString("ReportType"))
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
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

        'in order to clear crystal reports for Costing Preview
        If HttpContext.Current.Session("CostingTeamMemberTurnAroundPreview") IsNot Nothing Then
            Dim tempRpt As ReportDocument = New ReportDocument()
            tempRpt = CType(HttpContext.Current.Session("CostingTeamMemberTurnAroundPreview"), ReportDocument)
            If tempRpt IsNot Nothing Then
                tempRpt.Close()
                tempRpt.Dispose()
            End If
            HttpContext.Current.Session("CostingTeamMemberTurnAroundPreview") = Nothing
            GC.Collect()
        End If

    End Sub
End Class
