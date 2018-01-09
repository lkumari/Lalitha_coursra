'************************************************************************************
'* This page is used as the master template for the UGN Database.
'* Created by: Lynette Rey
'* Created on: 12/20/2007
' Modified on 07/24/2008 RCarlson - allow master page to be dynamic between test and production environment: titles and BI Link
' Modified on 09/15/2008 RCarlson - trap event when user closes the browser in the upper left corner, so that crystal reports can be cleaned out.
' Modified on 10/29/2008 RCarlson - added DMS User Guide.
' Modified on 10/29/2008 RCarlson - added new AR Module Menu.
' Modified on 11/17/2008 RCarlson - added new RFD Module menu.
' Modified on 02/06/2009 LRey - Added New Acoustic Module menu.
' Modified on 02/16/2009 LRey - Added new R&D Module menu.
' Modified on 05/08/2009 RCarlson - added cost sheet activity report cleanup
' Modified on 05/18/2009 LRey - Added new Calendars menu.

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
Partial Class crViewMasterPage
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ''site level definition for master pages ... put this in web.config:
        '<pages masterPageFile="mySite.master" />
        'Dim ds As DataSet
        Try
            If Not IsPostBack Then
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
                Else
                    Response.Cookies("UGNDB_User").Value = FullName
                End If

                'Dim a As String = commonFunctions.UserInfo()
                'lblUserFacility.Text = HttpContext.Current.Session("UserFacility")
                'Response.Cookies("UGNDB_TMLoc").Value = HttpContext.Current.Session("UserFacility")

                'Dim FullName As String = commonFunctions.getUserName()
                'Dim UserEmailAddress As String = FullName & "@ugnauto.com"
                'Response.Cookies("UGNDB_User_Email").Value = UserEmailAddress
                'If FullName = Nothing Then
                '    FullName = "Demo.Demo"  '* This account has restricted read only rights.
                'End If
                'lblUserName.Text = FullName
                'Dim LocationOfDot As Integer = InStr(FullName, ".")
                'If LocationOfDot > 0 Then
                '    Dim FirstName As String = Left(FullName, LocationOfDot - 1)
                '    Dim FirstInitial As String = Left(FullName, 1)
                '    Dim LastName As String = Right(FullName, Len(FullName) - LocationOfDot)

                '    Response.Cookies("UGNDB_User").Value = FirstInitial & LastName
                '    Response.Cookies("UGNDB_UserFullName").Value = Mid(FirstName, 1, 1).ToUpper & Mid(FirstName, 2, Len(FirstName) - 1) & " " & Mid(LastName, 1, 1).ToUpper & Mid(LastName, 2, Len(LastName) - 1)
                'Else
                '    Response.Cookies("UGNDB_User").Value = FullName
                '    Response.Cookies("UGNDB_UserFullName").Value = FullName

                'End If
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

                        Response.Cookies("UGNDB_TMID").Value = TeamMemberID
                    End If
                End If
            End If
        Catch ex As Exception
            'update error on web page
            'lblErrors.Text = ex.Message
            'lblErrors.Visible = True

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub


    Public Function IsBucketActive(ByVal nodeBucket As SiteMapNode) As Boolean
        ' pages that don't contain an item in Web.sitemap return null
        If (SiteMap.CurrentNode Is DBNull.Value) Then
            Return False
        Else
            Return SiteMap.CurrentNode.Equals(nodeBucket) Or SiteMap.CurrentNode.IsDescendantOf(nodeBucket)
        End If
    End Function

    'Protected Sub btnClosingWork_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClosingWork.Click

    '    'clear crystal reports
    '    AcousticModule.CleanAcousticCrystalReports()
    '    CostingModule.CleanCostingCrystalReports()
    '    DBAModule.CleanDBACrystalReports()
    '    ECIModule.CleanECICrystalReports()
    '    EXPModule.CleanExpCrystalReports()
    '    'PFModule.CleanPFCrystalReports()
    '    PEModule.CleanPEDMScrystalReports()
    '    RnDModule.CleanRnDCrystalReports()
    '    SafetyModule.CleanChemicalReviewFormCrystalReports()

    'End Sub

End Class

