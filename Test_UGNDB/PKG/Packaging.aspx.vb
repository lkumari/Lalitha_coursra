' ************************************************************************************************
' Name:	Packaging.aspx.vb
' Purpose:	This program is used to bind data and execute insert/update commands.
'           Applied gridview and detailsview events.
'
' Date		    Author	    
' 09/21/2012    SHoward		Created .Net application
' ************************************************************************************************
Imports System.Data.SqlClient
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Image

Partial Class PKG_Packaging
    Inherits System.Web.UI.Page

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try

            Dim m As ASP.masterpage_master = Master
            m.PageTitle = "UGN, Inc."
            m.ContentLabel = " Packaging Layout Entry"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Packaging</b> >  Packaging Layout "
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim MasterPanel As CollapsiblePanelExtender
            MasterPanel = CType(Master.FindControl("PKGExtender"), CollapsiblePanelExtender)
            MasterPanel.Collapsed = False

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'update error on web page
            lblMessage.Text += ex.Message & "<br>" & mb.Name & "<br>"

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub 'EOF Page_Init

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ''*******
            '' Initialize ViewState
            ''*******
            ''Used to define the primary record
            If HttpContext.Current.Request.QueryString("pPKGID") <> "" Then
                ViewState("pPKGID") = HttpContext.Current.Request.QueryString("pPKGID")
            Else
                ViewState("pPKGID") = 0
            End If

            ''*************************************************
            ''Check if IsPostBack
            ''*************************************************
            If Not Page.IsPostBack Then
                ''****************************************
                ''Redirect user to the right tab location
                ''****************************************
                If ViewState("pPKGID") <> 0 Then
                    BindCriteria()
                    BindData(ViewState("pPKGID"))
                Else
                    BindCriteria()
                    txtDescription.Focus()
                End If
            End If

            ''*************************************************
            '' "Form Level Security using Roles &/or Subscriptions"
            ''*************************************************
            CheckRights() '"Form Level Security using Roles &/or Subscriptions"

            ''*************************************************
            '' Initialize maxlength
            ''*************************************************
            txtDescription.Attributes.Add("onkeypress", "return tbLimit();")
            txtDescription.Attributes.Add("onkeyup", "return tbCount(" + lblDescChar.ClientID + ");")
            txtDescription.Attributes.Add("maxLength", "240")

            txtNotes.Attributes.Add("onkeypress", "return tbLimit();")
            txtNotes.Attributes.Add("onkeyup", "return tbCount(" + lblNotesChar.ClientID + ");")
            txtNotes.Attributes.Add("maxLength", "200")

        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF Page_Load

    Protected Sub mnuTabs_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles mnuTabs.MenuItemClick
        mvTabs.ActiveViewIndex = Int32.Parse(e.Item.Value)
    End Sub 'EOF mnuTabs_MenuItemClick

    Protected Sub CheckRights()
        ''********
        '' This function is used to enable/disable controls on the form based on TM's Security/Subscription
        ''********
        Try
            ''*******
            '' Disable controls by default
            ''*******
            btnAdd.Enabled = False
            ViewState("Admin") = False
            ViewState("ObjectRole") = False
            mnuTabs.Items(1).Enabled = False
            mnuTabs.Items(2).Enabled = False
            btnUpload.Visible = False
            btnResetFile.Visible = False
            uploadFile.Visible = False
            lblSelectFile.Visible = False

            ''*******
            '' Get current Team Member's TeamMemberID from Team_Member_Maint table
            ''*******
            Dim strFullName As String = commonFunctions.getUserName()
            Dim dsTeamMember As DataSet
            Dim dsRoleForm As DataSet
            Dim iTeamMemberID As Integer = 0
            Dim iWorking As Boolean = False
            Dim iFormID As Integer = 50 'Packaging Layout Form ID
            Dim iRoleID As Integer = 0

            dsTeamMember = SecurityModule.GetTeamMember(Nothing, strFullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            ''dsTeamMember = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            If dsTeamMember IsNot Nothing Then
                If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then
                    iTeamMemberID = dsTeamMember.Tables(0).Rows(0).Item("TeamMemberID")
                    ViewState("iTeamMemberID") = iTeamMemberID
                    iWorking = dsTeamMember.Tables(0).Rows(0).Item("Working")
                    If iWorking = True Then 'Allow TM access if he/she is an active UGN Team Member
                        'Get Team Member's Role assignment
                        dsRoleForm = SecurityModule.GetTMRoleForm(iTeamMemberID, Nothing, iFormID)

                        If dsRoleForm IsNot Nothing Then
                            If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then
                                For i = 0 To dsRoleForm.Tables(0).Rows.Count - 1
                                    iRoleID = dsRoleForm.Tables(0).Rows(i).Item("RoleID")
                                    Select Case iRoleID
                                        Case 11 '*** UGNAdmin: Full Access
                                            ''Used by full admin such as Developers
                                            ViewState("Admin") = True

                                            btnAdd.Enabled = True
                                            If ViewState("pPKGID") = 0 Then
                                            Else
                                                ViewState("ObjectRole") = True
                                                mnuTabs.Items(1).Enabled = True
                                                mnuTabs.Items(2).Enabled = True
                                                btnUpload.Visible = True
                                                btnResetFile.Visible = True
                                                uploadFile.Visible = True
                                                lblSelectFile.Visible = True

                                            End If
                                        Case 12 '*** UGNChampion: Create/Edit/Delete (Begins Process)
                                            ''Used by the Sales/Program Mgrs, Tooling/Purchasing Leads
                                            ViewState("Admin") = True
                                            btnAdd.Enabled = True
                                            If ViewState("pPKGID") = 0 Then

                                            Else
                                                ViewState("ObjectRole") = True
                                                mnuTabs.Items(1).Enabled = True
                                                mnuTabs.Items(2).Enabled = True
                                                btnUpload.Visible = True
                                                btnResetFile.Visible = True
                                                uploadFile.Visible = True
                                                lblSelectFile.Visible = True

                                            End If
                                        Case 13 '*** UGNAssist: Create/Edit/No Delete
                                            '** Used by Backup persons
                                            'N/A
                                        Case 14 '*** UGNReadOnly: No Create/No Edit/ No Delete/View Only
                                            'N/A
                                        Case 15 '*** UGNEdit: No Create/Edit/No Delete
                                            ''Used by the Accounting Group
                                            'N/A
                                        Case 16 '*** UGNReadOnly_Restriction: No Create/No Edit/ No Delete/View Only (Excludes Cost related information)
                                            'N/A
                                    End Select 'EOF of "Select Case iRoleID"
                                Next
                            End If 'EOF of "If dsRoleForm.Tables.Count And dsRoleForm.Tables(0).Rows.Count > 0 Then"
                        End If 'EOF of "If dsRoleForm IsNot Nothing Then"
                    End If 'EOF of "If iWorking = True Then"
                End If 'EOF of "If dsTeamMember.Tables.Count And dsTeamMember.Tables(0).Rows.Count > 0 Then"
            End If 'EOF of "If dsTeamMember IsNot Nothing Then"
        Catch ex As Exception

            'update error on web page
            lblMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

    End Sub

#Region "Packaging Layout Detail"
    Protected Sub BindCriteria()
        Try
            Dim ds As DataSet = New DataSet

            ''bind existing data to drop down Account Manager control for selection criteria for search
            ds = commonFunctions.GetTeamMemberBySubscription(108) '**SubscriptionID 9 is used for Packaging
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ddPackingLead.DataSource = ds
                ddPackingLead.DataTextField = ds.Tables(0).Columns("TMName").ColumnName.ToString()
                ddPackingLead.DataValueField = ds.Tables(0).Columns("TMID").ColumnName.ToString()
                ddPackingLead.DataBind()
                ddPackingLead.Items.Insert(0, "")
            End If

            ddPackingLead.SelectedValue = IIf(ddPackingLead.SelectedValue = Nothing, ViewState("iTeamMemberID"), ddPackingLead.SelectedValue)
        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message
            lblMessage.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF BindCriteria

    Public Sub BindData(ByVal PKGID As Integer)
        Try
            lblMessage.Text = Nothing
            lblMessage.Visible = False

            Dim ds As DataSet = New DataSet
            If PKGID <> Nothing Then
                ds = PKGModule.GetPKGLayout(PKGID)
                If commonFunctions.CheckDataSet(ds) = True Then
                    txtPKGID.Text = ds.Tables(0).Rows(0).Item("PKGID").ToString()
                    txtDescription.Text = ds.Tables(0).Rows(0).Item("LayoutDesc").ToString()
                    ddPackingLead.SelectedValue = ds.Tables(0).Rows(0).Item("PKGLeadTMID").ToString()
                    ddIsPublish.SelectedValue = ds.Tables(0).Rows(0).Item("IsPublish").ToString()

                    cddUGNLocation.SelectedValue = ds.Tables(0).Rows(0).Item("UGNFacility").ToString()
                    cddDepartment.SelectedValue = ds.Tables(0).Rows(0).Item("DepartmentID").ToString()
                    cddWorkCenter.SelectedValue = ds.Tables(0).Rows(0).Item("WorkCenter").ToString()

                    cddOEMMfg.SelectedValue = ds.Tables(0).Rows(0).Item("OEMManufacturer").ToString()
                    cddContainerNo.SelectedValue = ds.Tables(0).Rows(0).Item("CID").ToString()
                    txtModelYr.Text = ds.Tables(0).Rows(0).Item("ModelYr").ToString()
                    cddMake.SelectedValue = ds.Tables(0).Rows(0).Item("Make").ToString()
                    cddModel.SelectedValue = ds.Tables(0).Rows(0).Item("Model").ToString()
                    cddProgram.SelectedValue = ds.Tables(0).Rows(0).Item("ProgramID").ToString()
                    txtGrossWeight.Text = ds.Tables(0).Rows(0).Item("GrossWeight").ToString()
                    txtNotes.Text = ds.Tables(0).Rows(0).Item("Notes").ToString()
                    ddObsolete.SelectedValue = ds.Tables(0).Rows(0).Item("Obsolete").ToString()
                    If IsDBNull(ds.Tables(0).Rows(0).Item("FileName")) Then
                        imgPicture.Visible = False
                    Else
                        imgPicture.Src = "PackagingImage.aspx?pPKGID=" & ViewState("pPKGID")
                        'sdPicture.SourceUrl = "PackagingImage.aspx?pPKGID=" & ViewState("pPKGID")
                        imgPicture.Visible = True
                    End If
                End If

            End If 'EOF If ContainerNo <> Nothing Then

        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message
            lblMessage.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF BindData

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try

            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            Dim DefaultDate As String = Date.Now
            lblMessage.Text = Nothing
            lblMessage.Visible = False

            Dim UGNLocation As String = commonFunctions.GetCCDValue(cddUGNLocation.SelectedValue)
            Dim Department As String = commonFunctions.GetCCDValue(cddDepartment.SelectedValue)
            Dim WorkCenter As String = commonFunctions.GetCCDValue(cddWorkCenter.SelectedValue)
            Dim OEMMfg As String = commonFunctions.GetCCDValue(cddOEMMfg.SelectedValue)
            Dim ContainerNo As String = commonFunctions.GetCCDValue(cddContainerNo.SelectedValue)
            Dim Make As String = commonFunctions.GetCCDValue(cddMake.SelectedValue)
            Dim Model As String = commonFunctions.GetCCDValue(cddModel.SelectedValue)
            Dim Program As String = commonFunctions.GetCCDValue(cddProgram.SelectedValue)

            If ViewState("pPKGID") <> 0 Then
                '**********************
                '* Update Record
                '**********************
                PKGModule.UpdatePKGLayout(ViewState("pPKGID"), txtDescription.Text, ddPackingLead.SelectedValue, ddIsPublish.SelectedValue, UGNLocation, WorkCenter, ddContainerNo.SelectedValue, txtModelYr.Text, Program, txtGrossWeight.Text, txtNotes.Text, ddObsolete.SelectedValue, DefaultUser)

                BindData(ViewState("pPKGID"))

            Else 'EOF  If ViewState("pPKGID") <> Nothing Then

                '**********************
                '* Save Record
                '**********************
                PKGModule.InsertPKGLayout(txtDescription.Text, ddPackingLead.SelectedValue, ddIsPublish.SelectedValue, UGNLocation, WorkCenter, OEMMfg, ddContainerNo.SelectedValue, txtModelYr.Text, Program, txtGrossWeight.Text, txtNotes.Text, DefaultUser, DefaultDate)

                ''Locate Last PKID for redirection
                Dim ds As DataSet = Nothing
                ds = PKGModule.GetPKGLastLayoutID(txtDescription.Text, DefaultUser, DefaultDate)

                ViewState("pPKGID") = ds.Tables(0).Rows(0).Item("LastPKGID").ToString()

                Response.Redirect("Packaging.aspx?pPKGID=" & ViewState("pPKGID"), False)

            End If
        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message
            lblMessage.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnSave_Click

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Dim TempViewState As Integer
        If ViewState("pPKGID") <> 0 Then
            TempViewState = mvTabs.ActiveViewIndex
            mvTabs.GetActiveView()
            mnuTabs.Items(TempViewState).Selected = True

            BindData(ViewState("pPKGID"))
        Else
            Response.Redirect("Packaging.aspx", False)
        End If
    End Sub 'EOF btnReset_Click

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Try
            '***************
            '* Delete Record
            '***************
            If ViewState("pPKGID") <> 0 Then
                PKGModule.DeletePKGLayout(txtPKGID.Text)

                '***************
                '* Redirect user back to the search page.
                '***************
                Response.Redirect("PackagingList.aspx", False)
            End If

        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message
            lblMessage.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnDelete_Click

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Response.Redirect("Packaging.aspx", False)
    End Sub 'EOF btnAdd_Click

    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        Try
            'System.Threading.Thread.Sleep(3000)
            Dim DefaultUser As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            lblMessage2.Text = Nothing
            lblMessage2.Visible = False

            If ViewState("pPKGID") <> 0 Then
                If uploadFile.HasFile Then
                    If uploadFile.PostedFile.ContentLength <= 3500000 Then
                        Dim FileExt As String
                        FileExt = System.IO.Path.GetExtension(uploadFile.FileName).ToLower
                        Dim pat As String = "\\(?:.+)\\(.+)\.(.+)"
                        Dim r As Regex = New Regex(pat)
                        Dim m As Match = r.Match(uploadFile.PostedFile.FileName)

                        Dim ImageFileSize As Integer = Convert.ToInt32(uploadFile.PostedFile.InputStream.Length)
                        Dim ImageEncodeType As String = uploadFile.PostedFile.ContentType
                        Dim ImageBinaryFile As [Byte]() = New [Byte](ImageFileSize) {}
                        Dim imageLayout(uploadFile.PostedFile.InputStream.Length) As Byte
                        uploadFile.PostedFile.InputStream.Read(ImageBinaryFile, 0, ImageFileSize)

                        If (FileExt = ".jpg") Or (FileExt = ".JPG") Then
                            ''*************
                            '' Display File Info
                            ''*************
                            revUploadFile.Visible = False
                            vsSupDoc.Visible = False
                            lblMessage2.Text = "File name: " & uploadFile.FileName & "<br>" & _
                            "File Size: " & CType((ImageFileSize / 1024), Integer) & " KB<br>"
                            lblMessage2.Visible = True
                            lblMessage2.Width = 500
                            lblMessage2.Height = 30

                            ''***************
                            '' Insert Record
                            ''***************
                            PKGModule.UpdatePKGLayoutImage(ViewState("pPKGID"), ImageBinaryFile, uploadFile.FileName, ImageEncodeType, ImageFileSize, DefaultUser)


                        Else
                            lblMessage2.Text = "File exceeds size limit.  Please select a file less than 4MB (4000KB)."
                            lblMessage2.Visible = True
                            btnUpload.Enabled = False
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            'update error on web page
            lblMessage.Text = ex.Message
            lblMessage.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub 'EOF btnUpload_Click
    Protected Sub btnResetFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetFile.Click

        '***************
        '* Redirect user back to the search page.
        '***************
        Response.Redirect("Packaging.aspx?pPKGID=" + ViewState("pPKGID"), False)
    End Sub
#End Region 'EOF Layout Details



End Class
