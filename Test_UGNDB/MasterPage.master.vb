'************************************************************************************
'* This page is used as the master template for the UGN Database.
'* Created by: Lynette Rey
'* Created on: 12/20/2007
' Modified on 07/24/2008 RCarlson - allow master page to be dynamic between test and production environment: titles and BI Link
' Modified on 09/15/2008 RCarlson - trap event when user closes the browser in the upper left corner, so that crystal reports can be cleaned out.
' Modified on 10/29/2008 RCarlson   - added DMS User Guide.
' Modified on 10/29/2008 RCarlson   - added new AR Module Menu.
' Modified on 11/17/2008 RCarlson   - added new RFD Module menu.
' Modified on 02/06/2009 LRey       - Added New Acoustic Module menu.
' Modified on 02/16/2009 LRey       - Added new R&D Module menu.
' Modified on 05/08/2009 RCarlson   - added cost sheet activity report cleanup
' Modified on 05/18/2009 LRey       - Added new Calendars menu.
' Modified on 05/18/2009 LRey       - Added new DBA Workspace menu.
' Modified on 08/06/2009 RCarlson   - Clean ECI Module Crystal Reports too
' Modified on 10/08/2009 RCarlson   - removed the return value in the javascript function HandleOnClose in the masterpage.master
' Modified on 02/10/2010 RCarlson   - added SafeyModule Clean Crystal Reports function
' Modified on 02/16/2010 LRey       - Added Cost Reduction Menu
' Modified on 06/01/2010 RCarlson   - Added Plant Specific Reports
' Modified on 09/02/2010 LRey       - Added Purchasing Menu
' Modified on 09/07/2010 LRey       - Added Supplier Menu
' Modified on 08/05/2011 LRey       - Added Spending Requests Menu
' Modified on 10/21/2011 LRey       - Consolidating menu/forms per R.Khalif request 10/20/11
' Modified on 12/06/2011 RCarlson   - added Support Detail Link to MasterPage.Master
'************************************************************************************
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.HttpCookie
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Math
Imports System.XML
Partial Class MasterPage
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            ''site level definition for master pages ... put this in web.config:
            '<pages masterPageFile="mySite.master" />
            'Dim ds As DataSet
            If Not IsPostBack Then
                Dim a As String = commonFunctions.UserInfo()
                lblUserFacility.Text = HttpContext.Current.Session("UserFacility")
                Response.Cookies("UGNDB_TMLoc").Value = HttpContext.Current.Session("UserFacility")

                Dim FullName As String = commonFunctions.getUserName()
                Dim UserEmailAddress As String = FullName & "@ugnauto.com"
                Response.Cookies("UGNDB_User_Email").Value = UserEmailAddress
                If FullName = Nothing Then
                    FullName = "Demo.Demo"  '* This account has restricted read only rights.
                End If
                lblUserName.Text = FullName
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

                ''*******
                '' Get current Team Member's TeamMemberID from Team_Member_Maint table
                ''*******
                Dim ds As DataSet = New DataSet
                Dim TeamMemberID As Integer = 0
                Dim TMWorking As Boolean = False

                ds = SecurityModule.GetTeamMember(Nothing, FullName, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
                ''ds = SecurityModule.GetTeamMember(Nothing, "Demo.Demo", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
                If ds IsNot Nothing And ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        TeamMemberID = ds.Tables(0).Rows(0).Item("TeamMemberID").ToString()
                        TMWorking = ds.Tables(0).Rows(0).Item("Working").ToString()
                        Response.Cookies("UGNDB_TMID").Value = TeamMemberID

                        If TMWorking = True Then
                            ''*******
                            '' Enable/Disable Menu items based on current Team Member's form assignements
                            ''*******
                            Dim bList As System.Web.UI.WebControls.BulletedList
                            Dim tmds As DataSet = New DataSet

                            Dim MenuID As Integer = 0
                            Dim FormID As Integer = 0
                            Dim FormHLNK As String = Nothing
                            Dim FormName As String = Nothing
                            Dim Obsolete As Boolean = False
                            Dim i As Integer = 0

                            ds = SecurityModule.GetForm(Nothing, Nothing, Nothing, Nothing, Nothing)
                            For i = 0 To ds.Tables.Item(0).Rows.Count - 1
                                MenuID = ds.Tables(0).Rows(i).Item("MenuID")
                                FormID = ds.Tables(0).Rows(i).Item("FormID")
                                FormName = ds.Tables(0).Rows(i).Item("FormName").ToString()
                                FormHLNK = ds.Tables(0).Rows(i).Item("HyperlinkID").ToString()
                                Obsolete = ds.Tables(0).Rows(i).Item("Obsolete")

                                ''*******
                                '' Confirm TeamMember's FormID assignment
                                ''*******
                                If Obsolete = False Then
                                    tmds = SecurityModule.GetTMRoleForm(TeamMemberID, Nothing, FormID) 'TMID, RoleID, FormID
                                    Select Case MenuID
                                        Case 1
                                            '' *** DATA MAINTENANCE ***
                                            bList = CType(DataMaintContentPanel.FindControl("blDM"), System.Web.UI.WebControls.BulletedList)
                                            Dim DMListItem As New System.Web.UI.WebControls.ListItem
                                            DataMaintContentPanel.Enabled = True
                                            DataMaintPanel.Enabled = True
                                            DMListItem.Enabled = False

                                            DMListItem.Text = FormName
                                            DMListItem.Value = FormHLNK
                                            If (tmds.Tables.Item(0).Rows.Count > 0) Then 'Team Member Assigned: Row Found
                                                DMListItem.Enabled = True
                                            End If
                                            bList.Items.Add(DMListItem)
                                        Case 2
                                            '' *** PLANNING AND FORCASTING ***
                                            bList = CType(PFContentPanel.FindControl("blPF"), System.Web.UI.WebControls.BulletedList)
                                            Dim PFListItem As New System.Web.UI.WebControls.ListItem
                                            PFPanel.Enabled = True
                                            PFContentPanel.Enabled = True
                                            PFListItem.Enabled = False

                                            PFListItem.Text = FormName
                                            PFListItem.Value = FormHLNK
                                            If (tmds.Tables.Item(0).Rows.Count > 0) Then 'Team Member Assigned: Row Found
                                                PFListItem.Enabled = True
                                            End If
                                            bList.Items.Add(PFListItem)
                                        Case 3
                                            '' *** SECURITY ***
                                            bList = CType(SecurityContentPanel.FindControl("blSecurity"), System.Web.UI.WebControls.BulletedList)
                                            Dim SecurityListItem As New System.Web.UI.WebControls.ListItem
                                            SecurityPanel.Enabled = True
                                            SecurityContentPanel.Enabled = True
                                            SecurityListItem.Enabled = False

                                            SecurityListItem.Text = FormName
                                            SecurityListItem.Value = FormHLNK
                                            If (tmds.Tables.Item(0).Rows.Count > 0) Then 'Team Member Assigned: Row Found
                                                SecurityListItem.Enabled = True
                                            End If
                                            bList.Items.Add(SecurityListItem)
                                        Case 4
                                            '' *** WORKFLOW ***
                                            bList = CType(PFContentPanel.FindControl("blWorkFlow"), System.Web.UI.WebControls.BulletedList)
                                            Dim WFListItem As New System.Web.UI.WebControls.ListItem
                                            WorkFlowPanel.Enabled = True
                                            WorkFlowContentPanel.Enabled = True
                                            WFListItem.Enabled = False

                                            WFListItem.Text = FormName
                                            WFListItem.Value = FormHLNK
                                            If (tmds.Tables.Item(0).Rows.Count > 0) Then 'Team Member Assigned: Row Found
                                                WFListItem.Enabled = True
                                            End If
                                            bList.Items.Add(WFListItem)
                                        Case 5
                                            '' *** Drawing Management ***
                                            bList = CType(DrawMgmtContentPanel.FindControl("blDMS"), System.Web.UI.WebControls.BulletedList)
                                            Dim DMSListItem As New System.Web.UI.WebControls.ListItem
                                            DrawMgmtPanel.Enabled = True
                                            DrawMgmtContentPanel.Enabled = True
                                            DMSListItem.Enabled = False

                                            DMSListItem.Text = FormName
                                            DMSListItem.Value = FormHLNK
                                            If (tmds.Tables.Item(0).Rows.Count > 0) Then 'Team Member Assigned: Row Found
                                                DMSListItem.Enabled = True
                                            End If
                                            bList.Items.Add(DMSListItem)
                                        Case 6
                                            '' *** Accounts Receivable ***
                                            bList = CType(ARContentPanel.FindControl("blAR"), System.Web.UI.WebControls.BulletedList)
                                            Dim ARListItem As New System.Web.UI.WebControls.ListItem
                                            ARPanel.Enabled = True
                                            ARContentPanel.Enabled = True
                                            ARListItem.Enabled = False

                                            ARListItem.Text = FormName
                                            ARListItem.Value = FormHLNK
                                            If (tmds.Tables.Item(0).Rows.Count > 0) Then 'Team Member Assigned: Row Found
                                                ARListItem.Enabled = True
                                            End If
                                            bList.Items.Add(ARListItem)
                                        Case 7
                                            '' *** Request For Development ***
                                            bList = CType(RFDContentPanel.FindControl("blRFD"), System.Web.UI.WebControls.BulletedList)
                                            Dim RFDListItem As New System.Web.UI.WebControls.ListItem
                                            RFDPanel.Enabled = True
                                            RFDContentPanel.Enabled = True
                                            RFDListItem.Enabled = False

                                            RFDListItem.Text = FormName
                                            RFDListItem.Value = FormHLNK
                                            If (tmds.Tables.Item(0).Rows.Count > 0) Then 'Team Member Assigned: Row Found
                                                RFDListItem.Enabled = True
                                            End If
                                            bList.Items.Add(RFDListItem)
                                        Case 8
                                            '' *** User Guides ***
                                            bList = CType(UserGuidesContentPanel.FindControl("blUserGuides"), System.Web.UI.WebControls.BulletedList)
                                            Dim UserGuidesListItem As New System.Web.UI.WebControls.ListItem
                                            UserGuidesPanel.Enabled = True
                                            UserGuidesContentPanel.Enabled = True
                                            UserGuidesListItem.Enabled = False

                                            UserGuidesListItem.Text = FormName
                                            UserGuidesListItem.Value = FormHLNK
                                            'If (tmds.Tables.Item(0).Rows.Count > 0) Then 'Team Member Assigned: Row Found
                                            'allow everyone to see the document
                                            UserGuidesListItem.Enabled = True
                                            'End If
                                            bList.Items.Add(UserGuidesListItem)
                                        Case 10
                                            '' *** Research and Development ***
                                            bList = CType(RnDContentPanel.FindControl("blRnD"), System.Web.UI.WebControls.BulletedList)
                                            Dim RnDListItem As New System.Web.UI.WebControls.ListItem
                                            RnDPanel.Enabled = True
                                            RnDContentPanel.Enabled = True
                                            RnDListItem.Enabled = False

                                            RnDListItem.Text = FormName
                                            RnDListItem.Value = FormHLNK
                                            If (tmds.Tables.Item(0).Rows.Count > 0) Then 'Team Member Assigned: Row Found
                                                RnDListItem.Enabled = True
                                            End If
                                            bList.Items.Add(RnDListItem)
                                        Case 11
                                            '' *** Costing ***
                                            bList = CType(CostingContentPanel.FindControl("blCosting"), System.Web.UI.WebControls.BulletedList)
                                            Dim CostingListItem As New System.Web.UI.WebControls.ListItem
                                            CostingPanel.Enabled = True
                                            CostingContentPanel.Enabled = True
                                            CostingListItem.Enabled = False

                                            CostingListItem.Text = FormName
                                            CostingListItem.Value = FormHLNK
                                            If (tmds.Tables.Item(0).Rows.Count > 0) Then 'Team Member Assigned: Row Found
                                                CostingListItem.Enabled = True
                                            End If
                                            bList.Items.Add(CostingListItem)
                                        Case 12
                                            '' *** Packaging ***
                                            bList = CType(PKGContentPanel.FindControl("blPKG"), System.Web.UI.WebControls.BulletedList)
                                            Dim PKGListItem As New System.Web.UI.WebControls.ListItem
                                            PKGPanel.Enabled = True
                                            PKGContentPanel.Enabled = True
                                            PKGListItem.Enabled = False

                                            PKGListItem.Text = FormName
                                            PKGListItem.Value = FormHLNK
                                            If (tmds.Tables.Item(0).Rows.Count > 0) Then 'Team Member Assigned: Row Found
                                                PKGListItem.Enabled = True
                                            End If
                                            bList.Items.Add(PKGListItem)
                                            'Case 13
                                            '    '' *** Acoustic Matrix ***
                                            '    bList = CType(AcousticContentPanel.FindControl("blAcoustic"), System.Web.UI.WebControls.BulletedList)
                                            '    Dim AcousticListItem As New System.Web.UI.WebControls.ListItem
                                            '    AcousticPanel.Enabled = True
                                            '    AcousticContentPanel.Enabled = True
                                            '    AcousticListItem.Enabled = False

                                            '    AcousticListItem.Text = FormName
                                            '    AcousticListItem.Value = FormHLNK
                                            '    If (tmds.Tables.Item(0).Rows.Count > 0) Then 'Team Member Assigned: Row Found
                                            '        AcousticListItem.Enabled = True
                                            '    End If
                                            '    bList.Items.Add(AcousticListItem)
                                            'Case 14
                                            '    '' *** Calendars Matrix ***
                                            '    bList = CType(CalendarsContentPanel.FindControl("blCalendars"), System.Web.UI.WebControls.BulletedList)
                                            '    Dim CalendarsListItem As New System.Web.UI.WebControls.ListItem
                                            '    CalendarsPanel.Enabled = True
                                            '    CalendarsContentPanel.Enabled = True
                                            '    CalendarsListItem.Enabled = False

                                            '    CalendarsListItem.Text = FormName
                                            '    CalendarsListItem.Value = FormHLNK & "?sView=Month"
                                            '    If (tmds.Tables.Item(0).Rows.Count > 0) Then 'Team Member Assigned: Row Found
                                            '        CalendarsListItem.Enabled = True
                                            '    End If
                                            '    bList.Items.Add(CalendarsListItem)
                                            'Case 15
                                            '    ' '' *** DBA Workspace Matrix ***
                                            '    bList = CType(DBAContentPanel.FindControl("blDBA"), System.Web.UI.WebControls.BulletedList)
                                            '    Dim DBAListItem As New System.Web.UI.WebControls.ListItem
                                            '    DBAPanel.Enabled = True
                                            '    DBAContentPanel.Enabled = True
                                            '    DBAListItem.Enabled = False

                                            '    DBAListItem.Text = FormName
                                            '    DBAListItem.Value = FormHLNK
                                            '    If (tmds.Tables.Item(0).Rows.Count > 0) Then 'Team Member Assigned: Row Found
                                            '        DBAListItem.Enabled = True
                                            '    End If
                                            '    bList.Items.Add(DBAListItem)
                                            'Case 16
                                            '    ' '' *** Capital Projects ***
                                            '    bList = CType(EXPContentPanel.FindControl("blEXP"), System.Web.UI.WebControls.BulletedList)
                                            '    Dim EXPListItem As New System.Web.UI.WebControls.ListItem
                                            '    EXPPanel.Enabled = True
                                            '    EXPContentPanel.Enabled = True
                                            '    EXPListItem.Enabled = False

                                            '    EXPListItem.Text = FormName
                                            '    EXPListItem.Value = FormHLNK
                                            '    If (tmds.Tables.Item(0).Rows.Count > 0) Then 'Team Member Assigned: Row Found
                                            '        EXPListItem.Enabled = True
                                            '    End If
                                            '    bList.Items.Add(EXPListItem)
                                        Case 17
                                            ' '' *** Quality  ***
                                            bList = CType(ECIContentPanel.FindControl("blECI"), System.Web.UI.WebControls.BulletedList)
                                            Dim ECIListItem As New System.Web.UI.WebControls.ListItem
                                            ECIPanel.Enabled = True
                                            ECIContentPanel.Enabled = True
                                            ECIListItem.Enabled = False

                                            ECIListItem.Text = FormName
                                            ECIListItem.Value = FormHLNK
                                            If (tmds.Tables.Item(0).Rows.Count > 0) Then 'Team Member Assigned: Row Found
                                                ECIListItem.Enabled = True
                                            End If
                                            bList.Items.Add(ECIListItem)
                                            'Case 18
                                            '    ' '' *** Safety Projects ***
                                            '    bList = CType(SAFContentPanel.FindControl("blSAF"), System.Web.UI.WebControls.BulletedList)
                                            '    Dim SAFListItem As New System.Web.UI.WebControls.ListItem
                                            '    SAFPanel.Enabled = True
                                            '    SAFContentPanel.Enabled = True
                                            '    SAFListItem.Enabled = False

                                            '    SAFListItem.Text = FormName
                                            '    SAFListItem.Value = FormHLNK
                                            '    If (tmds.Tables.Item(0).Rows.Count > 0) Then 'Team Member Assigned: Row Found
                                            '        SAFListItem.Enabled = True
                                            '    End If
                                            '    bList.Items.Add(SAFListItem)
                                        Case 19
                                            ' '' *** Cost Reduction Projects ***
                                            bList = CType(CRContentPanel.FindControl("blCR"), System.Web.UI.WebControls.BulletedList)
                                            Dim CRListItem As New System.Web.UI.WebControls.ListItem
                                            CRPanel.Enabled = True
                                            CRContentPanel.Enabled = True
                                            CRListItem.Enabled = False

                                            CRListItem.Text = FormName
                                            CRListItem.Value = FormHLNK
                                            If (tmds.Tables.Item(0).Rows.Count > 0) Then 'Team Member Assigned: Row Found
                                                CRListItem.Enabled = True
                                            End If
                                            bList.Items.Add(CRListItem)
                                            'Case 20
                                            '    ' '' *** Plant Specific Reports ***
                                            '    bList = CType(CRContentPanel.FindControl("blPSR"), System.Web.UI.WebControls.BulletedList)
                                            '    Dim PSRListItem As New System.Web.UI.WebControls.ListItem
                                            '    PSRPanel.Enabled = True
                                            '    PSRContentPanel.Enabled = True
                                            '    PSRListItem.Enabled = False

                                            '    PSRListItem.Text = FormName
                                            '    PSRListItem.Value = FormHLNK
                                            '    If (tmds.Tables.Item(0).Rows.Count > 0) Then 'Team Member Assigned: Row Found
                                            '        PSRListItem.Enabled = True
                                            '    End If
                                            '    bList.Items.Add(PSRListItem)
                                        Case 21
                                            ' '' *** PURCHASING ***
                                            bList = CType(CRContentPanel.FindControl("blPUR"), System.Web.UI.WebControls.BulletedList)
                                            Dim PURListItem As New System.Web.UI.WebControls.ListItem
                                            PURPanel.Enabled = True
                                            PURContentPanel.Enabled = True
                                            PURListItem.Enabled = False

                                            PURListItem.Text = FormName
                                            PURListItem.Value = FormHLNK
                                            If (tmds.Tables.Item(0).Rows.Count > 0) Then 'Team Member Assigned: Row Found
                                                PURListItem.Enabled = True
                                            End If
                                            bList.Items.Add(PURListItem)
                                            ' ''Case 22
                                            ' ''    ' '' *** SUPPLIER ***
                                            ' ''    bList = CType(CRContentPanel.FindControl("blSUP"), System.Web.UI.WebControls.BulletedList)
                                            ' ''    Dim SUPListItem As New System.Web.UI.WebControls.ListItem
                                            ' ''    SUPPanel.Enabled = True
                                            ' ''    SUPContentPanel.Enabled = True
                                            ' ''    SUPListItem.Enabled = False

                                            ' ''    SUPListItem.Text = FormName
                                            ' ''    SUPListItem.Value = FormHLNK
                                            ' ''    If (tmds.Tables.Item(0).Rows.Count > 0) Then 'Team Member Assigned: Row Found
                                            ' ''        SUPListItem.Enabled = True
                                            ' ''    End If
                                            ' ''    bList.Items.Add(SUPListItem)
                                        Case 23
                                            ' '' *** SPENDING REQUESTS ***
                                            bList = CType(CRContentPanel.FindControl("blSPR"), System.Web.UI.WebControls.BulletedList)
                                            Dim SPRListItem As New System.Web.UI.WebControls.ListItem
                                            SPRPanel.Enabled = True
                                            SPRContentPanel.Enabled = True
                                            SPRListItem.Enabled = False

                                            SPRListItem.Text = FormName
                                            SPRListItem.Value = FormHLNK
                                            If (tmds.Tables.Item(0).Rows.Count > 0) Then 'Team Member Assigned: Row Found
                                                SPRListItem.Enabled = True
                                            End If
                                            bList.Items.Add(SPRListItem)
                                        Case 24
                                            ' '' *** PRODUCTION ***
                                            bList = CType(CRContentPanel.FindControl("blMPR"), System.Web.UI.WebControls.BulletedList)
                                            Dim MPRListItem As New System.Web.UI.WebControls.ListItem
                                            MPRPanel.Enabled = True
                                            MPRContentPanel.Enabled = True
                                            MPRListItem.Enabled = False

                                            MPRListItem.Text = FormName
                                            MPRListItem.Value = FormHLNK
                                            If (tmds.Tables.Item(0).Rows.Count > 0) Then 'Team Member Assigned: Row Found
                                                MPRListItem.Enabled = True
                                            End If
                                            bList.Items.Add(MPRListItem)
                                        Case 25
                                            ' '' *** DOWNLOADS ***
                                            bList = CType(CRContentPanel.FindControl("blDL"), System.Web.UI.WebControls.BulletedList)
                                            Dim DLListItem As New System.Web.UI.WebControls.ListItem
                                            DLPanel.Enabled = True
                                            DLContentPanel.Enabled = True
                                            DLListItem.Enabled = False

                                            DLListItem.Text = FormName
                                            DLListItem.Value = FormHLNK
                                            If (tmds.Tables.Item(0).Rows.Count > 0) Then 'Team Member Assigned: Row Found
                                                DLListItem.Enabled = True
                                            End If
                                            bList.Items.Add(DLListItem)
                                    End Select 'MenuID
                                End If 'EOF Obsolete = False
                            Next 'EOF for For/Next Loop GetForm
                        End If 'EOF of "If TMWorking = True Then"
                    End If ' If ds Tables Rows > 0 Nothing Then
                End If ' If ds IsNot Nothing Then
            End If 'EOF of "If Not IsPostBack Then"

            ' check test or production environments
            Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString()

            'if production environment, then show BI Tool Link, adjust other links
            If strProdOrTestEnvironment = "Test_UGNDB" Then
                lblHeaderTitle.Text = "TEST: UGN Database"
                hlnkMicrostrategyBI.Visible = False               
            Else
                lblHeaderTitle.Text = "UGN Database"
                hlnkMicrostrategyBI.Visible = True              
            End If

        Catch ex As Exception

        End Try

    End Sub
    Public Property ContentLabel() As String
        Get
            Return lblContent.Text
        End Get
        Set(ByVal value As String)
            lblContent.Text = value
        End Set
    End Property
    Public Function IsBucketActive(ByVal nodeBucket As SiteMapNode) As Boolean
        ' pages that don't contain an item in Web.sitemap return null
        If (SiteMap.CurrentNode Is DBNull.Value) Then
            Return False
        Else
            Return SiteMap.CurrentNode.Equals(nodeBucket) Or SiteMap.CurrentNode.IsDescendantOf(nodeBucket)
        End If
    End Function

    Public Property PageTitle() As String
        Get
            Return pageTitle2.Text
        End Get
        Set(ByVal value As String)
            pageTitle2.Text = value
        End Set
    End Property

    Protected Sub btnClosingWork_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClosingWork.Click

        'clear crystal reports
        AcousticModule.CleanAcousticCrystalReports()
        CostingModule.CleanCostingCrystalReports()
        DBAModule.CleanDBACrystalReports()
        ECIModule.CleanECICrystalReports()
        EXPModule.CleanExpCrystalReports()
        'PFModule.CleanPFCrystalReports()
        PEModule.CleanPEDMScrystalReports()
        RnDModule.CleanRnDCrystalReports()
        SafetyModule.CleanChemicalReviewFormCrystalReports()
        PURModule.CleanFormCrystalReports()

    End Sub

End Class

