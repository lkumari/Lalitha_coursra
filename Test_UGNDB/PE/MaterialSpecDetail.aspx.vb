' ***********************************************************************************************
'
' Name:		DrawingDetail.aspx
' Purpose:	This Code Behind is for the Product Engineering Material Specification Detail
'
' Date		    Author	    
' 03/03/2011    Roderick Carlson    Created
' 12/05/2011    Roderick Carlson    Modified - allow DocX and xlsX files to be uploaded

Partial Class MaterialSpecDetail
    Inherits System.Web.UI.Page

    Private Sub ClearMessages()

        Try
            lblMessage.Text = ""
            lblMessageSupportingDocs.Text = ""
            lblMessageDrawingMaterialRelate.Text = ""

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
    Private Sub BindCriteria()

        Try
            Dim ds As DataSet

            ds = commonFunctions.GetSubFamily(0)
            If commonFunctions.CheckDataSet(ds) = True Then
                ddSubFamily.DataSource = ds
                ddSubFamily.DataTextField = ds.Tables(0).Columns("ddSubFamilyName").ColumnName.ToString()
                ddSubFamily.DataValueField = ds.Tables(0).Columns("SubFamilyID").ColumnName
                ddSubFamily.DataBind()
                ddSubFamily.Items.Insert(0, "")
            End If

            ''need a partial partnumber with same base and % here
            'ds = PEModule.GetDrawingMaterialSpecSearch(ViewState("MaterialSpecNo"), "", "", "", "")
            'If commonFunctions.CheckDataSet(ds) = True Then
            '    ddMaterialSpecNo.DataSource = ds
            '    ddMaterialSpecNo.DataTextField = ds.Tables(0).Columns("MaterialSpecNo").ColumnName.ToString()
            '    ddMaterialSpecNo.DataValueField = ds.Tables(0).Columns("MaterialSpecNo").ColumnName.ToString()
            '    ddMaterialSpecNo.DataBind()
            'End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try

            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Material Specification - Detail"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Drawing Management</b> > <a href='MaterialSpecList.aspx'><b>Material Specification Search</b></a> > Material Specification Detail "
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim MasterPanel As CollapsiblePanelExtender
            MasterPanel = CType(Master.FindControl("DMGExtender"), CollapsiblePanelExtender)
            MasterPanel.Collapsed = False

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

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

            If commonFunctions.CheckDataSet(dsTeamMember) = True Then                
                iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")

                If iTeamMemberID = 530 Then
                    'iTeamMemberID = 694 ' Adam.Miller 
                    iTeamMemberID = 433 ' Derek Ames
                End If

                'different form id (36) but same form security as DMS Drawings
                dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, 35)

                If commonFunctions.CheckDataSet(dsRoleForm) = True Then                    
                    iRoleID = dsRoleForm.Tables(0).Rows(0).Item("RoleID")

                    Select Case iRoleID
                        Case 11 '*** UGNAdmin: Full Access
                            ViewState("isAdmin") = True
                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                            ViewState("isAdmin") = True
                        Case 13 '*** UGNAssist: Create/Edit/No Delete
                            ViewState("isAdmin") = True
                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only

                        Case 15 '*** UGNEdit: No Create/Edit/No Delete
                            ViewState("isAdmin") = True
                        Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)

                    End Select
                End If            
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += "<br>" & ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Sub BindData()

        Try

            Dim ds As DataSet
            Dim strNewMaterialSubFamilyAndWeight As String = ""

            ds = PEModule.GetDrawingMaterialSpec(ViewState("MaterialSpecNo"))

            If commonFunctions.CheckDataSet(ds) = True Then
                lblMaterialSpecNo.Text = ds.Tables(0).Rows(0).Item("MaterialSpecNo").ToString
                lblRevisionDate.Text = ds.Tables(0).Rows(0).Item("ddRevisionDate").ToString
                txtMaterialSpecDesc.Text = ds.Tables(0).Rows(0).Item("MaterialSpecDesc").ToString
                If ds.Tables(0).Rows(0).Item("SubfamilyID").ToString <> "" Then
                    If ddSubFamily.Items.FindByValue(ds.Tables(0).Rows(0).Item("SubfamilyID").ToString) IsNot Nothing Then
                        ddSubFamily.SelectedValue = ds.Tables(0).Rows(0).Item("SubfamilyID").ToString
                    End If
                End If

                txtInitialAreaWeight.Text = ds.Tables(0).Rows(0).Item("AreaWeight").ToString

                'get list of revisions of related materials - same subfamily and weight
                strNewMaterialSubFamilyAndWeight = GetMaterialSpecSubFamilyAndWeight()
                If strNewMaterialSubFamilyAndWeight <> "" Then
                    ds = PEModule.GetDrawingMaterialSpecSearch(strNewMaterialSubFamilyAndWeight & "%", "", "", "", "", "", "")

                    If commonFunctions.CheckDataSet(ds) = True Then
                        ddMaterialSpecNo.DataSource = ds
                        ddMaterialSpecNo.DataTextField = ds.Tables(0).Columns("MaterialSpecNo").ColumnName
                        ddMaterialSpecNo.DataValueField = ds.Tables(0).Columns("MaterialSpecNo").ColumnName
                        ddMaterialSpecNo.DataBind()
                        ddMaterialSpecNo.SelectedValue = lblMaterialSpecNo.Text
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

    Private Sub EnableControls()

        Try
            Dim ds As DataSet
            Dim strMaxMaterialSpecRevision As String = ""

            If ViewState("MaterialSpecNo") <> "" Then
                ds = PEModule.GetDrawingMaterialSpecMaxRevision(ViewState("MaterialSpecNo"))
                If commonFunctions.CheckDataSet(ds) = True Then
                    strMaxMaterialSpecRevision = ds.Tables(0).Rows(0).Item("MaxRevisionMaterialSpec").ToString
                End If

                If ViewState("MaterialSpecNo") = strMaxMaterialSpecRevision Or strMaxMaterialSpecRevision = "" Then
                    btnCreateRevision.Visible = ViewState("isAdmin")
                End If

                btnCreateNew.Visible = False
                btnUpdate.Visible = ViewState("isAdmin")

                ddMaterialSpecNo.Visible = True
                ddSubFamily.Enabled = False
                txtInitialAreaWeight.Enabled = False

                btnSaveUploadSupportingDocument.Visible = ViewState("isAdmin")
                lblFileUploadLabel.Visible = ViewState("isAdmin")
                fileUploadSupportingDoc.Visible = ViewState("isAdmin")

                lblDrawingMaterialRelateTitle.Visible = True

                gvSupportingDoc.Visible = True
                gvSupportingDoc.Columns(gvSupportingDoc.Columns.Count - 1).Visible = ViewState("isAdmin")

                gvDrawingMaterialSpecRelate.Visible = True
                gvDrawingMaterialSpecRelate.Columns(gvDrawingMaterialSpecRelate.Columns.Count - 1).Visible = ViewState("isAdmin")
                If gvDrawingMaterialSpecRelate.FooterRow IsNot Nothing Then
                    gvDrawingMaterialSpecRelate.ShowFooter = ViewState("isAdmin")
                End If
            Else
                btnCreateNew.Visible = ViewState("isAdmin")
                btnCreateRevision.Visible = False
                btnUpdate.Visible = False

                ddMaterialSpecNo.Visible = False
                ddSubFamily.Enabled = ViewState("isAdmin")

                btnSaveUploadSupportingDocument.Visible = False
                lblFileUploadLabel.Visible = False
                fileUploadSupportingDoc.Visible = False

                lblDrawingMaterialRelateTitle.Visible = False

                gvSupportingDoc.Visible = False
                gvDrawingMaterialSpecRelate.Visible = False
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            CheckRights()

            If Not Page.IsPostBack Then

                PEModule.CleanPEDMScrystalReports()

                BindCriteria()

                ViewState("MaterialSpecNo") = ""

                If HttpContext.Current.Request.QueryString("MaterialSpecNo") <> "" Then
                    ViewState("MaterialSpecNo") = HttpContext.Current.Request.QueryString("MaterialSpecNo")
                    BindData()
                End If

                btnCreateRevision.Attributes.Add("onclick", "if(confirm('Are you sure that you want to create a revision?.  ')){}else{return false}")

                txtMaterialSpecDesc.Attributes.Add("onkeypress", "return tbLimit();")
                txtMaterialSpecDesc.Attributes.Add("onkeyup", "return tbCount(" + lblMaterialSpecDescCharCount.ClientID + ");")
                txtMaterialSpecDesc.Attributes.Add("maxLength", "400")

                txtSupportingDocDesc.Attributes.Add("onkeypress", "return tbLimit();")
                txtSupportingDocDesc.Attributes.Add("onkeyup", "return tbCount(" + lblSupportingDocDescCharCount.ClientID + ");")
                txtSupportingDocDesc.Attributes.Add("maxLength", "200")
            End If

            EnableControls()

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub

    Private Function GenerateNewMaterialSpecNo() As String

        Try
            '   ''Material Spec Numbers will have auto-generated numbers in the format of 1234-5678-9AB(CC)
            '   ''1234 = represents 4 digit family/subfamily code
            '   ''5678 = represents area weight
            '   ''9AB = represents next numeric sequence, of the presence of materials with same combination of 1234-5678 above            
            '   ''CC = represents change level, starts at 0 for each material specification, auto-incremented for revisions       


            '   Should sequence number, revision, or both be looped and iterated? YES, confirmed March 9, 2011 by Prod Dev Team
            '   Thus one function will be used to increment the sequence for NEW material specs and the other will increment the revisions.
            '   This particular function will incement the sequences.

            Dim ds As DataSet
            Dim iRowCount As Integer = 1
        
            Dim strNewMaterialSubFamilyAndWeight As String = ""
            Dim strNewMaterialWithoutRevision As String = ""
            Dim strNewMaterialSpecNo As String = ""

            'check digits 9AB before incrementing numSeq, to avoid gaps
            Dim strNumberSequence As String = "1"
            Dim iNumberSequence As Integer = 0

            If ViewState("MaterialSpecNo") <> "" Then
                strNumberSequence = Mid$(ViewState("MaterialSpecNo"), 10, 3)
            End If
            iNumberSequence = CType(strNumberSequence, Integer)

            ''count number of records that have the same 1234-5678 value 
            ''initial implementation has all sequences of 001 and revisions of 00
            Dim strSubFamilyID As String = ddSubFamily.SelectedValue().ToString

            Dim strInitialAreaWeight As String = txtInitialAreaWeight.Text.Trim

            strInitialAreaWeight = strInitialAreaWeight.PadLeft(4, "0")

            strNewMaterialSubFamilyAndWeight = strSubFamilyID.PadLeft(4, "0") & "-" & strInitialAreaWeight         'PORTION: 1234-5678
            strNewMaterialWithoutRevision = strNewMaterialSubFamilyAndWeight & "-" & strNumberSequence.PadLeft(3, "0")   'PORTION: 9AB 
            strNewMaterialSpecNo = strNewMaterialWithoutRevision & "(00)"                                                'PORTION: (CC) 

            ''check to see if root level exists
            ''increment the seq until not in use
            ds = PEModule.GetDrawingMaterialSpec(strNewMaterialWithoutRevision & "%")
            If commonFunctions.CheckDataSet(ds) = True Then
                While iRowCount > 0 And iNumberSequence < 999

                    iNumberSequence += 1

                    strNewMaterialWithoutRevision = strNewMaterialSubFamilyAndWeight & "-" & iNumberSequence.ToString.PadLeft(3, "0")   'PORTION: 1234-5678 and Next Seq
                    strNewMaterialSpecNo = strNewMaterialWithoutRevision & "(00)"

                    ds = PEModule.GetDrawingMaterialSpec(strNewMaterialWithoutRevision & "%")
                    If commonFunctions.CheckDataSet(ds) = True Then
                        iRowCount = ds.Tables.Item(0).Rows.Count
                    Else
                        iRowCount = 0
                    End If
                End While
            End If

            'do not let there be more than 999 sequences
            If iNumberSequence > 999 Then
                strNewMaterialSpecNo = ""
            End If

            GenerateNewMaterialSpecNo = strNewMaterialSpecNo

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

            GenerateNewMaterialSpecNo = ""
        End Try

    End Function
    Private Function GetMaterialSpecSubFamilyAndWeight() As String

        Dim strNewMaterialSubFamilyAndWeight As String = ""

        Try
            Dim strNumberSequence As String = "1"
            Dim iNumberSequence As Integer = 0

            If ViewState("MaterialSpecNo") <> "" Then
                strNumberSequence = Mid$(ViewState("MaterialSpecNo"), 11, 3)
            End If
            iNumberSequence = CType(strNumberSequence, Integer)

            Dim strSubFamilyID As String = ddSubFamily.SelectedValue().ToString

            Dim strInitialAreaWeight As String = txtInitialAreaWeight.Text.Trim

            strInitialAreaWeight = strInitialAreaWeight.PadLeft(4, "0")

            strNewMaterialSubFamilyAndWeight = strSubFamilyID.PadLeft(4, "0") & "-" & strInitialAreaWeight         'PORTION: 1234-5678

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        GetMaterialSpecSubFamilyAndWeight = strNewMaterialSubFamilyAndWeight

    End Function

    Private Function GenerateRevisionMaterialSpecNo() As String

        Try
            '   ''Material Spec Numbers will have auto-generated numbers in the format of 1234-5678-9AB(CC)
            '   ''1234 = represents 4 digit family/subfamily code
            '   ''5678 = represents area weight
            '   ''9AB = represents next numeric sequence, of the presence of materials with same combination of 1234-5678 above            
            '   ''CC = represents change level, starts at 0 for each material specification, auto-incremented for revisions       


            '   Should sequence number, revision, or both be looped and iterated? YES, confirmed March 9, 2011 by Prod Dev Team
            '   Thus one function will be used to increment the sequence for NEW material specs and the other will increment the revisions.
            '   This particular function will incement the revision.

            Dim ds As DataSet
            Dim iRowCount As Integer = 1

            Dim strNewMaterialSubFamilyAndWeight As String = ""
            Dim strNewMaterialWithoutRevision As String = ""

            Dim strNewMaterialSpecNo As String = ""
            

            'check digits 9AB before incrementing numSeq, to avoid gaps
            Dim strNumberSequence As String = "1"
            Dim iNumberSequence As Integer = 0

            Dim strRevision As String = "00"
            Dim iRevision As Integer = 0

            If ViewState("MaterialSpecNo") <> "" Then
                strNumberSequence = Mid$(ViewState("MaterialSpecNo"), 11, 3)
            End If
            iNumberSequence = CType(strNumberSequence, Integer)

            If ViewState("MaterialSpecNo") <> "" Then
                strRevision = Mid$(ViewState("MaterialSpecNo"), 15, 2)
            End If
            'start with next revision
            iRevision = CType(strRevision, Integer) + 1

            ''count number of records that have the same 1234-5678 value 
            ''initial implementation has all sequences of 001 and revisions of 00
            Dim strSubFamilyID As String = ddSubFamily.SelectedValue().ToString

            Dim strInitialAreaWeight As String = txtInitialAreaWeight.Text.Trim

            strInitialAreaWeight = strInitialAreaWeight.PadLeft(4, "0")

            strNewMaterialSubFamilyAndWeight = strSubFamilyID.PadLeft(4, "0") & "-" & strInitialAreaWeight         'PORTION: 1234-5678
            strNewMaterialWithoutRevision = strNewMaterialSubFamilyAndWeight & "-" & strNumberSequence.PadLeft(3, "0")   'PORTION: 9AB 
            strNewMaterialSpecNo = strNewMaterialWithoutRevision & "(" & iRevision.ToString.PadLeft(2, "0") & ")"                               'PORTION: (CC) 

            ''increment the revision until not in use
            ds = PEModule.GetDrawingMaterialSpec(strNewMaterialSpecNo)
            If commonFunctions.CheckDataSet(ds) = True Then
                While iRowCount > 0 And iRevision < 99

                    iRevision += 1

                    strNewMaterialSpecNo = strNewMaterialWithoutRevision & "(" & iRevision.ToString.PadLeft(2, "0") & ")"

                    ds = PEModule.GetDrawingMaterialSpec(strNewMaterialSpecNo)
                    If commonFunctions.CheckDataSet(ds) = True Then
                        iRowCount = ds.Tables.Item(0).Rows.Count
                    Else
                        iRowCount = 0
                    End If

                End While
            End If

            'do not let there be more than 999 sequences
            If iRevision > 99 Then
                strNewMaterialSpecNo = ""
            End If

            GenerateRevisionMaterialSpecNo = strNewMaterialSpecNo

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

            GenerateRevisionMaterialSpecNo = ""
        End Try

    End Function

    Private Function CheckForPreExistingMaterialMatch() As String

        Dim strPreExistingMaterialMatch As String = ""

        Try
            Dim ds As DataSet

            Dim dInitialAreaWeight As Double = 0
            Dim strSubFamilyID As String = ""

            If txtInitialAreaWeight.Text.Trim <> "" Then
                dInitialAreaWeight = CType(txtInitialAreaWeight.Text.Trim, Double)
            End If

            strSubFamilyID = ddSubFamily.SelectedValue.PadLeft(4, "0")

            ds = PEModule.GetDrawingMaterialSpecMatchKind(strSubFamilyID, dInitialAreaWeight)

            If commonFunctions.CheckDataSet(ds) = True Then
                strPreExistingMaterialMatch = ds.Tables(0).Rows(0).Item("MaterialSpecNo").ToString
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        CheckForPreExistingMaterialMatch = strPreExistingMaterialMatch

    End Function

    Protected Sub btnCreateNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCreateNew.Click

        Try
            ClearMessages()

            Dim strNewMaterialSpecNo As String = ""
            Dim dAreaWeight As Double = 0
            Dim strPreExistingMaterialMatch As String = ""

            'need to check if there is already a part with existing subfamily and area weight
            'if not then generate new Material Spec Number
            'if so, then increment the seq number

            strPreExistingMaterialMatch = CheckForPreExistingMaterialMatch()

            If strPreExistingMaterialMatch <> "" Then
                lblMessage.Text = "Error: A match was found of the new material specification (" & strPreExistingMaterialMatch & "). So, the sequence was incremented."
            End If

            strNewMaterialSpecNo = GenerateNewMaterialSpecNo()

            If strNewMaterialSpecNo <> "" Then

                If txtInitialAreaWeight.Text.Trim <> "" Then
                    dAreaWeight = CType(txtInitialAreaWeight.Text.Trim, Double)
                End If

                PEModule.InsertDrawingMaterialSpec(strNewMaterialSpecNo, txtMaterialSpecDesc.Text.Trim, dAreaWeight, ddSubFamily.SelectedValue)

                ViewState("MaterialSpecNo") = strNewMaterialSpecNo

                BindData()

                lblMessage.Text = "The new material specification was created successfully."

                EnableControls()
            Else
                lblMessage.Text = "Error: The new material specification could NOT be created."
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

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click

        Try    
            ClearMessages()

            Dim strNewMaterialSpecNo As String = ""
            Dim strPreExistingMaterialMatch As String = ""

            'need to check if there is already a part with existing subfamily and area weight
            'if not then generate new Material Spec Number

            If ViewState("MaterialSpecNo") <> "" Then

                PEModule.UpdateDrawingMaterialSpec(ViewState("MaterialSpecNo"), txtMaterialSpecDesc.Text.Trim)

                lblMessage.Text = "Updated Successfully."
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

    Protected Sub btnCreateRevision_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCreateRevision.Click

        Try
            ClearMessages()

            Dim strNewMaterialSpecNo As String = ""
            Dim dAreaWeight As Double = 0

            If ViewState("MaterialSpecNo") <> "" Then
                strNewMaterialSpecNo = GenerateRevisionMaterialSpecNo()

                If strNewMaterialSpecNo <> "" Then

                    If txtInitialAreaWeight.Text.Trim <> "" Then
                        dAreaWeight = CType(txtInitialAreaWeight.Text.Trim, Double)
                    End If

                    PEModule.InsertDrawingMaterialSpec(strNewMaterialSpecNo, txtMaterialSpecDesc.Text.Trim, dAreaWeight, ddSubFamily.SelectedValue)

                    txtMaterialSpecDesc.Text = ""
                    ViewState("MaterialSpecNo") = strNewMaterialSpecNo

                    BindData()

                    lblMessage.Text = "The revision material specification was created.<br>PLEASE SAVE A NEW DESCRIPTION."
                Else
                    lblMessage.Text = "Error: The revision could NOT be created."
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

    Protected Sub btnSaveUploadSupportingDocument_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveUploadSupportingDocument.Click

        Try
            ClearMessages()

            If fileUploadSupportingDoc.PostedFile.ContentLength <= 3500000 Then
                Dim FileExt As String
                FileExt = System.IO.Path.GetExtension(fileUploadSupportingDoc.FileName).ToLower
                Dim pat As String = "\\(?:.+)\\(.+)\.(.+)"
                Dim r As Regex = New Regex(pat)
                Dim m As Match = r.Match(fileUploadSupportingDoc.PostedFile.FileName)

                'Dim BinaryFile(fileUploadSupportingDoc.PostedFile.InputStream.Length) As Byte
                'Dim EncodeType As String = fileUploadSupportingDoc.PostedFile.ContentType
                'fileUploadSupportingDoc.PostedFile.InputStream.Read(BinaryFile, 0, BinaryFile.Length)
                'Dim FileSize As Integer = fileUploadSupportingDoc.PostedFile.ContentLength

                Dim SupportingDocFileSize As Integer = Convert.ToInt32(fileUploadSupportingDoc.PostedFile.InputStream.Length)
                Dim SupportingDocEncodeType As String = fileUploadSupportingDoc.PostedFile.ContentType
                Dim SupportingDocBinaryFile As [Byte]() = New [Byte](SupportingDocFileSize) {}
                fileUploadSupportingDoc.PostedFile.InputStream.Read(SupportingDocBinaryFile, 0, SupportingDocFileSize)

                If (FileExt = ".pdf") Or (FileExt = ".xls") Or (FileExt = ".xlsx") Or (FileExt = ".doc") Or (FileExt = ".docx") Or (FileExt = ".jpeg") Or (FileExt = ".jpg") Or (FileExt = ".tif") Or (FileExt = ".ppt") Or (FileExt = ".msg") Then

                    'If (FileExt = ".pdf") Or (FileExt = ".xls") Or (FileExt = ".doc") Or (FileExt = ".jpeg") Or (FileExt = ".jpg") Or (FileExt = ".tif") Then

                    'PEModule.InsertDrawingMaterialSpecSupportingDoc(ViewState("MaterialSpecNo"), txtSupportingDocDesc.Text.Trim, fileUploadSupportingDoc.FileName, BinaryFile, EncodeType, FileSize)
                    PEModule.InsertDrawingMaterialSpecSupportingDoc(ViewState("MaterialSpecNo"), txtSupportingDocDesc.Text.Trim, fileUploadSupportingDoc.FileName, SupportingDocBinaryFile, SupportingDocEncodeType, SupportingDocFileSize)

                    revUploadFile.Enabled = False

                    lblMessage.Text += "File Uploaded Successfully<br>"
                    txtSupportingDocDesc.Text = ""

                    gvSupportingDoc.DataBind()
                    gvSupportingDoc.Visible = True

                    revUploadFile.Enabled = True
                End If
            Else
                lblMessage.Text &= "File exceeds size limit.  Please select a file less than 4MB (4000KB)."
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += "<br>" & ex.Message & mb.Name & "<br>"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        lblMessageSupportingDocs.Text = lblMessage.Text

    End Sub

    Protected Sub ddMaterialSpecNo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddMaterialSpecNo.SelectedIndexChanged

        Try
            ClearMessages()

            If ddMaterialSpecNo.SelectedIndex >= 0 Then
                Response.Redirect("MaterialSpecDetail.aspx?MaterialSpecNo=" & ddMaterialSpecNo.SelectedValue, False)
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += "<br>" & ex.Message & mb.Name & "<br>"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

    Protected Sub gvSupportingDoc_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSupportingDoc.DataBound

        Try

            If gvSupportingDoc.HeaderRow IsNot Nothing Then
                gvSupportingDoc.HeaderRow.Cells(0).Visible = False
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub
#Region "Insert Empty GridView Work-Around"

    Private Property LoadDataEmpty_DrawingMaterialSpecRelate() As Boolean

        ' From Andrew Robinson's Insert Empty GridView solution
        ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

        ' some controls that are used within a GridView,
        ' such as the calendar control, can cuase post backs.
        ' we need to preserve LoadDataEmpty across post backs.

        Get
            If ViewState("LoadDataEmpty_DrawingMaterialSpecRelate") IsNot Nothing Then
                Dim tmpBoolean As Boolean = CType(ViewState("LoadDataEmpty_DrawingMaterialSpecRelate"), Boolean)
                Return tmpBoolean
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("LoadDataEmpty_DrawingMaterialSpecRelate") = value
        End Set

    End Property

    Protected Sub odsDrawingMaterialSpecRelate_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles odsDrawingMaterialSpecRelate.Selected

        Try
            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx

            ' bubble exceptions before we touch e.ReturnValue
            If e.Exception IsNot Nothing Then
                Throw e.Exception
            End If

            ' get the DataTable from the ODS select method
            Console.WriteLine(e.ReturnValue)

            Dim dt As Drawings.DrawingMaterialSpecRelateByMaterialSpecNo_MaintDataTable = CType(e.ReturnValue, Drawings.DrawingMaterialSpecRelateByMaterialSpecNo_MaintDataTable)

            ' if rows=0 then add a dummy (null) row and set the LoadDataEmpty flag.
            If dt.Rows.Count = 0 Then
                dt.Rows.Add(dt.NewRow())
                LoadDataEmpty_DrawingMaterialSpecRelate = True
            Else
                LoadDataEmpty_DrawingMaterialSpecRelate = False
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
#End Region

    Protected Sub gvDrawingMaterialSpecRelate_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDrawingMaterialSpecRelate.RowCommand

        Try
            ClearMessages()

            Dim txtDrawingNoTemp As TextBox
            Dim txtDrawingMaterialSpecNotesTemp As TextBox
            Dim ds As DataSet

            ' ''***
            ' ''This section allows the inserting of a new row when called by the OnInserting event call.
            ' ''***
            If (e.CommandName = "Insert") Then

                txtDrawingNoTemp = CType(gvDrawingMaterialSpecRelate.FooterRow.FindControl("txtInsertDrawingNo"), TextBox)
                txtDrawingMaterialSpecNotesTemp = CType(gvDrawingMaterialSpecRelate.FooterRow.FindControl("txtInsertDrawingMaterialSpecNotes"), TextBox)

                If txtDrawingNoTemp.Text.Trim <> "" And lblMaterialSpecNo.Text.Trim <> "" Then
                    ds = PEModule.GetDrawing(txtDrawingNoTemp.Text.Trim)
                    If commonFunctions.CheckDataSet(ds) = True Then
                        odsDrawingMaterialSpecRelate.InsertParameters("MaterialSpecNo").DefaultValue = lblMaterialSpecNo.Text
                        odsDrawingMaterialSpecRelate.InsertParameters("DrawingNo").DefaultValue = txtDrawingNoTemp.Text.Trim
                        odsDrawingMaterialSpecRelate.InsertParameters("DrawingMaterialSpecNotes").DefaultValue = txtDrawingMaterialSpecNotesTemp.Text.Trim

                        odsDrawingMaterialSpecRelate.Insert()
                    Else
                        lblMessage.Text = "Error: The Drawing does not exist. Therefore the row could not be saved."
                    End If
                End If
            End If

            ''***
            ''This section allows show/hides the footer row when the Edit control is clicked
            ''***
            If e.CommandName = "Edit" Then
                gvDrawingMaterialSpecRelate.ShowFooter = False
            Else
                gvDrawingMaterialSpecRelate.ShowFooter = True
            End If

            ''***
            ''This section clears out the values in the footer row
            ''***
            If e.CommandName = "Undo" Then
                txtDrawingNoTemp = CType(gvDrawingMaterialSpecRelate.FooterRow.FindControl("txtInsertDrawingNo"), TextBox)
                txtDrawingNoTemp.Text = ""

                txtDrawingMaterialSpecNotesTemp = CType(gvDrawingMaterialSpecRelate.FooterRow.FindControl("txtInsertDrawingMaterialSpecNotes"), TextBox)
                txtDrawingMaterialSpecNotesTemp.Text = ""
            End If


            lblMessageDrawingMaterialRelate.Text = lblMessage.Text

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvDrawingMaterialSpecRelate_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDrawingMaterialSpecRelate.RowCreated

        Try

            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(0).Attributes.CssStyle.Add("display", "none")
                'e.Row.Cells(1).Attributes.CssStyle.Add("display", "none")
            End If

            ' From Andrew Robinson's Insert Empty GridView solution
            ' http://blog.binaryocean.com/2006/01/05/InsertRowsWithAGridView.aspx
            ' when binding a row, look for a zero row condition based on the flag.
            ' if we have zero data rows (but a dummy row), hide the grid view row
            ' and clear the controls off of that row so they don't cause binding errors

            Dim blnLoadDataEmpty As Boolean = LoadDataEmpty_DrawingMaterialSpecRelate()
            If blnLoadDataEmpty And e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
                e.Row.Controls.Clear()
            End If

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message & "<br>"

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub

    Protected Sub gvDrawingMaterialSpecRelate_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDrawingMaterialSpecRelate.RowDataBound

        Try

            'hide header columns
            If gvDrawingMaterialSpecRelate.Rows.Count > 0 Then
                gvDrawingMaterialSpecRelate.HeaderRow.Cells(0).Visible = False
                'gvDrawingMaterialSpecRelate.HeaderRow.Cells(1).Visible = False
            End If

            ' Build the client script to open a popup window containing
            ' SubDrawings. Pass the ClientID of 4 the 
            ' four TextBoxes (which will receive data from the popup)
            ' in a query string.

            Dim strWindowAttribs As String = _
               "width=950px," & _
               "height=550px," & _
               "left='+((screen.width-950)/2)+'," & _
               "top='+((screen.height-550)/2)+'," & _
               "resizable=yes,scrollbars=yes,status=yes"

            If (e.Row.RowType = DataControlRowType.Footer) Then
                Dim ibtn As ImageButton = CType(e.Row.FindControl("iBtnSearchDrawingNo"), ImageButton)
                Dim txtInsertDrawingNo As TextBox = CType(e.Row.FindControl("txtInsertDrawingNo"), TextBox)
              
                If ibtn IsNot Nothing Then

                    Dim strPagePath As String = _
                "../PE/DrawingLookUp.aspx?DrawingControlID=" & txtInsertDrawingNo.ClientID
                    Dim strClientScript As String = _
                        "window.open('" & strPagePath & "','DrawingNoPopupSearch','" & _
                        strWindowAttribs & "');return false;"
                    ibtn.Attributes.Add("onClick", strClientScript)
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
End Class
