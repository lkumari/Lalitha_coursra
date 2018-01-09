' ************************************************************************************************
' Name:	Safety_Chemical_Review_Form_Preview.aspx.vb
' Purpose:	This program is used to call crystal viewer and get data from Chemical Review From
'
' Date		    Author	    
' 02/10/2010    Roderick Carlson			Created .Net application
' 02/28/2011    Roderick Carlson            Show in PDF format 
' ************************************************************************************************
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class Safety_Chemical_Review_Form_Preview
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

            ViewState("isAdmin") = False

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, True, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If commonFunctions.CheckDataset(dsTeamMember) = True Then

                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 96)

                If commonFunctions.CheckDataset(dsRoleForm) = True Then

                    iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                    Select Case iRoleID
                        Case 11 '*** UGNAdmin: Full Access

                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)

                        Case 13 '*** UGNAssist: Create/Edit/No Delete

                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only

                        Case 15 '*** UGNEdit: No Create/Edit/No Delete

                        Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)

                    End Select
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
    '        Dim pfChemRevFormID As ParameterField = New ParameterField

    '        ' setting the name of parameter fields with wich they will be received in report 
    '        pfChemRevFormID.ParameterFieldName = "@ChemRevFormID"

    '        ' the above declared parameter fields accept values as discrete objects 
    '        ' so declaring discrete objects 
    '        Dim dcChemRevFormID As New ParameterDiscreteValue

    '        ' setting the values of discrete objects 
    '        dcChemRevFormID.Value = ViewState("ChemRevFormID")

    '        ' now adding these discrete values to parameters 
    '        pfChemRevFormID.CurrentValues.Add(dcChemRevFormID)

    '        ' now adding all these parameter fields to the parameter collection 
    '        paramFields.Add(pfChemRevFormID)

    '        ' finally add the parameter collection to the crystal report viewer 
    '        crChemicalReviewFormPreview.ParameterFieldInfo = paramFields

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

            If HttpContext.Current.Request.QueryString("ChemRevFormID") <> "" Then

                Dim oRpt As ReportDocument = New ReportDocument()

                ViewState("ChemRevFormID") = HttpContext.Current.Request.QueryString("ChemRevFormID")

                If Session("ChemRevFormPreviewID") <> ViewState("ChemRevFormID") Then
                    Session("ChemRevFormPreview") = Nothing
                    Session("ChemRevFormPreviewID") = Nothing
                End If

                If (Session("ChemRevFormPreview") Is Nothing) Then

                    'Dim crTable As CrystalDecisions.CrystalReports.Engine.Table
                    'Dim crDatabase As CrystalDecisions.CrystalReports.Engine.Database
                    'Dim dbConn As TableLogOnInfo = New TableLogOnInfo()

                    '' new report document object 
                    oRpt.Load(Server.MapPath(".\Forms\") & "ChemicalReviewForm.rpt")

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
                    'crChemicalReviewFormPreview.DisplayGroupTree = False
                    'crChemicalReviewFormPreview.ReportSource = oRpt

                    'so uptill now we have created everything 
                    'what remains is to pass parameters to our report, so it 
                    'shows only selected records. so calling a method to set 
                    'those parameters. 

                    'Check if there are parameters or not in report.
                    'Dim intCounter As Integer = oRpt.DataDefinition.ParameterFields.Count

                    'setReportParameters()

                    Dim strDatabaseName As String = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString() '"Test_UGNDB" OR "UGNDB"
                    Dim strServerName As String = System.Configuration.ConfigurationManager.AppSettings("DBServer").ToString() '"DB_Server" and soon SQLCLUSTERVS
                    Dim strUserID As String = System.Configuration.ConfigurationManager.AppSettings("TUID").ToString
                    Dim strPassword As String = System.Configuration.ConfigurationManager.AppSettings("TPSWD").ToString

                    oRpt.SetDatabaseLogon(strUserID, strPassword, strServerName, strDatabaseName)
                    oRpt.SetParameterValue("@ChemRevFormID", ViewState("ChemRevFormID"))

                    Session("ChemRevFormPreview") = oRpt
                    Session("ChemRevFormPreviewID") = ViewState("ChemRevFormID")

                    Dim oStream As New System.IO.MemoryStream
                    oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                    Response.Clear()
                    Response.Buffer = True
                    Response.ContentType = "application/pdf"

                    'this opens immediately
                    Response.Charset = ""
                    Response.AddHeader("content-disposition", "inline;filename=ChemicalReviewForm-" & ViewState("ChemRevFormID").ToString & "preview.pdf")

                    Response.BinaryWrite(oStream.ToArray())


                Else
                    oRpt = CType(Session("ChemRevFormPreview"), ReportDocument)

                    'crChemicalReviewFormPreview.ReportSource = oRpt
                    Dim oStream As New System.IO.MemoryStream
                    oStream = oRpt.ExportToStream(ExportFormatType.PortableDocFormat)
                    Response.Clear()
                    Response.Buffer = True
                    Response.ContentType = "application/pdf"
                    Response.AddHeader("content-disposition", "attachment;filename=ChemicalReviewForm-" & ViewState("ChemRevFormID").ToString & "preview.pdf")
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

        If HttpContext.Current.Session("ChemRevFormPreview") IsNot Nothing Then
            Dim tempRpt As ReportDocument = New ReportDocument()
            tempRpt = CType(HttpContext.Current.Session("ChemRevFormPreview"), ReportDocument)
            If tempRpt IsNot Nothing Then
                tempRpt.Close()
                tempRpt.Dispose()
            End If
            HttpContext.Current.Session("ChemRevFormPreview") = Nothing
            HttpContext.Current.Session("ChemRevFormPreviewID") = Nothing
            GC.Collect()
        End If

    End Sub
End Class
