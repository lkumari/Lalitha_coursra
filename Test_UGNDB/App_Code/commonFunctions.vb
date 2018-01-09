' ************************************************************************************************
' Name:		commonfunctions.vb
' Purpose:	This code is referenced from all vb files, mostly for the purpose of calling stored procedures or getting user-rights
'
' Date		    Author	    
'       	    LR			Created .Net application
' 07/23/2008    LRey        Updated GetTeamMemberBySubscription 
' 07/23/2008    RCarlson    Added connectToOldUGNDatabase, getOldUGNDatabaseUserInfo, getOldUGNDatabaseRoles
' 07/31/2008    RCarlson    Added Get Purchased Good Function, adjusted name parameter in GetCommodity to be CommodityName
' 08/05/2008    RCarlson    Updated stored Procedure name for sp_Get_Drawing in function GetDrawing, added Insert Drawing, Update Drawing, Delete Drawing
' 08/06/2008    RCarlson    Moved all drawing functions to PEModule.vb
' 08/11/2008    LRey        Added SoldTo to sp_Get_Customer
' 10/06/2008    RCarlson    Updated GetVendor function to point to updated stored procedure
' 10/24/2008    RCalrson    Adjusted timeout of some stored procedures
' 12/08/2008    RCarlson    Added Function GetCustomerPartPartRelate
' 01/26/2009    LRey        Added a universal lookup for Month drop down lists.
' 02/20/2009    RCarlon     Added Get Unit Function, commented out unused functions
' 03/12/2009    RCarlson    Added Filter parameter to getDepartment
' 03/31/2009    RCarlson    Adjusted parameters to getVendor, added getUGNDBVendor, added insertUGNDBVendor
' 04/28/2009    RCarlson    Added Function to Get Backup or Dept In Charge Email of Team Member
' 05/12/2009    RCarlson    Added Function Get_UGNDB_Pending_Tasks and Get_UGNDB_Recent_Tasks
' 06/05/2009    RCarlson    Added Functions to get CABBV or SOLDTO from ddCustomerValue
' 06/29/2009    RCarlson    Added Function: GetBusinessProcessType
' 07/01/2009    RCarlson    Added Function: GetPriceCode
' 08/14/2009    RCarlson    Added Function: GetProgramMake
' 08/28/2009    RCarlson    Adjust getUGNDBVendor to allow like searches for BPCSVendorID
' 09/09/2009    RCarlson    adjust convertSpecialChar to show _ instead of ticks and changed paramter in GetCustomer
' 09/18/2009    RCarlson    Added Function SetUGNDBUser
' 01/21/2010    RCarlson    Added Function GetTeamMemberBySubscriptionByUGNFacility
' 01/27/2010    RCarlson    Added Function GetWorkflowFamilyPurchasingAssignments
' 01/28/2010    RCarlson    Added Function GetWorkflowMakePurchasingAssignments, GetCommodityWithWorkFlowAssignments, GetFamilyWithWorkFlowAssignments, GetProgramMakeWithWorkFlowAssignments
' 02/01/2010    RCarlson    Added Function GetBusinessProcessAction
' 04/23/2010    RCarlson    Adjusted Function GetCustomerPartPartRelate to have BarCodePartNo parameters
' 05/25/2010    LRey        Added Function GetDepartmentGLNo based on Facility selection
' 07/20/2010    RCarlson    Added Function CheckDataTable
' 08/06/2010    LREy        Added Function GetRoyalty
' 08/25/2010    LRey        Added Function GetGLAccounts
' 08/27/2010    LRey        Added Function GetVendorType
' 09/01/2010    RCarlson    Added isActiveBPCSOnly Parameter to GetUGNDBVendor
' 11/08/2010    RCarlson    Edited GetWorkflowMakePurchasingAssignments to be GetWorkFlowMakeAssignments so more subscriptions could be leveraged
' 04/19/2011    LRey        Added Function GetPlatform, DeletePlatformCookies
' 05/09/2011    LRey        Added new functions GetDABBV, GetCABBVbyOEM, GetOEMbyCOMPNY
' 05/10/2011    LREY        Added new function GetSoldToByCompnybyCABBVbyOEM
' 05/16/2011    LREY        Added new function DeletePlatformProgramCookies, GetRegion, GetAssemblyPlantLocation
' 05/20/2011    LREY        Added new function DeleteAssemblyPlantLocationCookies
' 05/26/2011    LREY        Added new function GetPlatformProgram
' 06/12/2011    LREY        Added new function GetProgramVolume
' 08/16/2011    LREY        Added new funciton GetPlatformOEMMfgByMake, GetPlatformByMake, GetProgramModelByMake, GetProgramByMake
' 08/24/2011    LREY        Added Make to Get_Model, Added DeleteModelCookies, GetOEMManufacturer functions
' 08/25/2011    LREY        Added GetMakeByOEMMfg function
' 09/28/2011    LREY        Added GetDepartmentLWK function
' 09/28/2011    LREY        Added GetDepartmentWorkCenter function
' 10/12/2011    LREY        Added GetBodyStyle function
' 10/13/2011    LREY        Added GetOEMMfgByOEM function
' 10/14/2011    LREY        Added GetPartNo function
' 10/27/2011    LREY        Added GetCommodityClass function
' 10/28/2011    LREY        Modified GetCommodities function
' 11/28/2011    RCarlson    Modified DisplayData and LocateUser function to handle invalid users
' 01/17/2012    LREY        Added GetOEMbyOEMMfg function
' 01/24/2012    RCarlson    Added GetFutureCustomer function
' 02/08/2012    LREY        Added Send Email functions to centralize the SMTP call for troublshooting.
' 04/12/2012    LREY        Added GetCABBVByOEMMfg function
' 04/17/2012    RCarlson    Modified - added parameter to Get Business Process Action
' 05/16/2012    LREY        Added GetVendorAddress function
' 09/18/2012    LREY        Removed SortbyObsolete from GetCABBVbyOEM function
' 10/16/2012    LREY        Modified EmailSend
' 11/26/2012    LREY        Added GetCountry function
' 12/04/2012    RCarlson    Addded Parameter to TeamMemberWorkflowAssignments Function and Removed GetRawMaterialFunction
' 01/07/2014    LREY        Comment out Various Functions that will not be used according to new ERP method
' ************************************************************************************************
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Text
Imports System.Xml
Imports System.IO
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.Page
Imports System.Web.UI.WebControls
Imports System.Net.Mail
Imports System.Threading
Imports System.Web.Configuration
''Imports ActiveDs

Public Class commonFunctions
    Inherits System.ComponentModel.Component

#Region " Component Designer generated code "

    Public Sub New(ByVal Container As System.ComponentModel.IContainer)
        MyClass.New()

        'Required for Windows.Forms Class Composition Designer support
        Container.Add(Me)
    End Sub

    Public Sub New()
        MyBase.New()

        'This call is required by the Component Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container
    End Sub

#End Region

#Region "TEAM MEMBER INFO"
    Public Shared Function connectUser(ByVal adsPath As String) As ActiveDs.IADsUser

        Dim objNamespace As ActiveDs.IADsOpenDSObject
        Dim uid As String = ConfigurationManager.AppSettings("ADUser")
        Dim pass As String = ConfigurationManager.AppSettings("ADPass")
        connectUser = Nothing

        Try
            objNamespace = GetObject("LDAP:")
            connectUser = objNamespace.OpenDSObject(adsPath, uid, pass, 0)

            'ADDITIONAL AD/LDAP details to query for info:
            'Dim oRoot As DirectoryEntry = New DirectoryEntry(System.Configuration.ConfigurationManager.ConnectionStrings("LDAPConn").ToString)
            'Dim rootDSE As DirectoryEntry = New DirectoryEntry("LDAP://rootDSE")
            'Dim root As DirectoryEntry = New DirectoryEntry("LDAP://" + rootDSE.Properties("defaultNamingContext").Value.ToString)
            'Dim userAdEntry As DirectoryEntry = New DirectoryEntry(root.Path, uid, pass, AuthenticationTypes.Secure)

            'Dim srch As New DirectorySearcher(userAdEntry)
            'srch.Filter = "(&(objectClass=user)(samAccountName=" & uid & "))"
            'Dim result As SearchResult = srch.FindOne
            'If Not result Is Nothing Then
            '    'obj = result.GetDirectoryEntry() 
            '    Return connectUser
            'Else
            '    Return Nothing
            'End If


        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "adsPath: " & adsPath & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "connectUser : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("connectUser : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return Nothing
        End Try

        Return connectUser

    End Function 'EOF connectUser
    Public Shared Function locateUser(ByVal userName As String) As String

        Try
            'Dim dirEntry As New DirectoryServices.DirectoryEntry(System.Configuration.ConfigurationManager.ConnectionStrings("LDAPConn").ToString)
            Dim dirEntry As New DirectoryServices.DirectoryEntry("LDAP://dc=ugnnet,dc=com", "ugnnet\contact.editor", "Ugnus@1")

            Dim searchAgent As New DirectoryServices.DirectorySearcher(dirEntry)
            'dirEntry.Username = ConfigurationManager.AppSettings("LDAPAdmin")
            'dirEntry.Password = ConfigurationManager.AppSettings("LDAPPass")

            Dim result As DirectoryServices.SearchResult
            Dim filter As String
            Dim temp() As String = Split(userName, ".")
            Dim user As String = temp(1) & ", " & temp(0)

            filter = "(&(objectCategory=user)(samaccountname=" & userName & "))"
            searchAgent.Filter = filter
            result = searchAgent.FindOne

            Return result.Path
        Catch ex As Exception
            ''on error, collect function data, error, and last page, then redirect to error page
            'Dim strUserEditedData As String = "userName: " & userName & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            'HttpContext.Current.Session("BLLerror") = "locateUser : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            '" :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            'HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            'UGNErrorTrapping.InsertErrorLog("locateUser : " & _
            'commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            'HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            UGNErrorTrapping.InsertErrorLog("locateUser : " & _
       commonFunctions.convertSpecialChar("User ID not found for " & userName & ". Check TM's Security.", False), "CommonFunctions.vb", "")

            HttpContext.Current.Response.Redirect("InvalidUser.aspx?UserName=" & userName, False)

            Return ""
        End Try

    End Function 'EOF locateUser
    Public Shared Function getUserName() As String

        Try
            Dim computerName As String = My.Computer.Name
            Dim pos As Integer = InStr(My.User.Name, "\")
            Dim userName As String = ""

            If Not (pos = 0) Then
                userName = Microsoft.VisualBasic.Right(My.User.Name, Len(My.User.Name) - pos)
            End If

            Return userName
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "getUserName : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("getUserName : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return ""
        End Try

    End Function 'EOF getUserName
    Public Shared Sub SetUGNDBUser()

        Try
            If HttpContext.Current.Request.Cookies("UGNDB_User") Is Nothing Then
                Dim FullName As String = commonFunctions.getUserName()
                'Dim UserEmailAddress As String = FullName & "@ugnusa.com"
                'HttpContext.Current.Response.Cookies("UGNDB_User_Email").Value = UserEmailAddress
                If FullName = Nothing Then
                    FullName = "Demo.Demo"  '* This account has restricted read only rights.
                End If

                Dim LocationOfDot As Integer = InStr(FullName, ".")
                If LocationOfDot > 0 Then
                    Dim FirstName As String = Left(FullName, LocationOfDot - 1)
                    Dim FirstInitial As String = Left(FullName, 1)
                    Dim LastName As String = Right(FullName, Len(FullName) - LocationOfDot)

                    HttpContext.Current.Response.Cookies("UGNDB_User").Value = FirstInitial & LastName

                Else
                    HttpContext.Current.Response.Cookies("UGNDB_User").Value = FullName
                End If
            End If

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User Not defined yet"
            HttpContext.Current.Session("BLLerror") = "SetUGNDBUser : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("SetUGNDBUser : " & commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        End Try

    End Sub 'EOF SetUGNDBUser
    Public Shared Function GetUserId(ByVal empEmail As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_TeamMember"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim getData As New DataSet

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@parm_TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@parm_TeamMemberID").Value = Nothing

            myCommand.Parameters.Add("@parm_UserName", SqlDbType.Int)
            myCommand.Parameters("@parm_UserName").Value = Nothing

            myCommand.Parameters.Add("@parm_ShortName", SqlDbType.VarChar)
            myCommand.Parameters("@parm_ShortName").Value = Nothing

            myCommand.Parameters.Add("@parm_LastName", SqlDbType.VarChar)
            myCommand.Parameters("@parm_LastName").Value = Nothing

            myCommand.Parameters.Add("@parm_FirstName", SqlDbType.VarChar)
            myCommand.Parameters("@parm_FirstName").Value = Nothing

            myCommand.Parameters.Add("@parm_Email", SqlDbType.VarChar)
            myCommand.Parameters("@parm_Email").Value = empEmail

            myCommand.Parameters.Add("@parm_Working", SqlDbType.Bit)
            myCommand.Parameters("@parm_Working").Value = Nothing

            myCommand.Parameters.Add("@parm_SortBy", SqlDbType.VarChar)
            myCommand.Parameters("@parm_SortBy").Value = Nothing

            Dim myAdapter As SqlDataAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(getData, "UserIdData")
            GetUserId = getData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "empEmail: " & empEmail & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetUserId : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetUserId : " & commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetUserId = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetUserId
    Public Shared Function UserInfo() As String
        Dim cpu As String = My.Computer.Name
        Dim user As String = My.User.Name
        Dim userDomain As String = ""
        Dim userPath As String = ""
        Dim userName As String = ""

        UserInfo = ""
        Try
            Dim objComputer As Object = CreateObject("WScript.Network")

            ''MODIFY THIS, based on the environment(test or prod) , to pull getUserName() when in PROD,
            ''but to get the logged in value from UGN DB for TEST, to test as the user
            userName = getUserName()
            If userName <> "" Then
                userPath = locateUser(userName)
                Dim userObj As ActiveDs.IADsUser

                If Not (userPath Is Nothing) Then
                    userObj = connectUser(userPath)
                    If (userObj Is Nothing) Then
                        GetLogout(True)
                    Else
                        UserInfo = displayData(userObj)
                    End If

                End If
            End If

        Catch ex As Exception
            Return UserInfo + " _ " + userName + " _ " + userPath
        End Try
        Return UserInfo

    End Function 'EOF UserInfo
    Public Shared Function displayData(ByVal user As ActiveDs.IADsUser) As String

        Dim sUserId As String = ""
        Dim sUserName As String = ""
        Dim ds As DataSet = New DataSet

        Try
            HttpContext.Current.Session("userEmail") = user.EmailAddress

            ds = GetUserId(user.EmailAddress)

            If CheckDataSet(ds) = True Then
                'If ds.Tables.Count > 0 Then
                sUserId = ds.Tables(0).Rows(0).Item("TeamMemberID").ToString
                sUserName = ds.Tables(0).Rows(0).Item("UserName").ToString
            Else
                HttpContext.Current.Session("UserId") = "Demo"
                HttpContext.Current.Session("UserName") = "Demo.Demo"
            End If

            If sUserId = Nothing Then
                UGNErrorTrapping.InsertErrorLog("displayData : " & _
           commonFunctions.convertSpecialChar("User ID not found for " & user.EmailAddress & ". Check TM's Security.", False), "CommonFunctions.vb", "")

                HttpContext.Current.Response.Redirect("InvalidUser.aspx?UserName=" & user.FirstName & " " & user.LastName & "&UserEmail=" & user.EmailAddress, False)
            End If

            HttpContext.Current.Session("UserId") = sUserId
            HttpContext.Current.Session("UserName") = sUserName

            Dim sFac As String = Nothing
            If InStr(user.ADsPath, "TinleyPark") > 0 Then
                sFac = "UT"
            ElseIf InStr(user.ADsPath, "ChicagoHeights") > 0 Then
                sFac = "UN"
            ElseIf InStr(user.ADsPath, "Heights") > 0 Then
                sFac = "UN"
            ElseIf InStr(user.ADsPath, "Jackson") > 0 Then
                sFac = "UP"
            ElseIf InStr(user.ADsPath, "Somerset") > 0 Then
                sFac = "UR"
            ElseIf InStr(user.ADsPath, "Valparaiso") > 0 Then
                sFac = "US"
            ElseIf InStr(user.ADsPath, "Mexico") > 0 Then
                sFac = "UW"
            End If
            HttpContext.Current.Session("UserFacility") = sFac

            Return (user.FullName)
            'to get additional information like AD path and email address:
            '+ "<br/>" + user.ADsPath + user.EmailAddress)
            'on error, collect function data, error, and last page, then redirect to error page
        Catch ex As Exception
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value _
            & ", sUserId: " & sUserId & ", sUserName: " & sUserName & ", userEmail: " & user.EmailAddress

            HttpContext.Current.Session("BLLerror") = "displayData : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("displayData : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF displayData
    Public Shared Function GetLogout(ByVal access As Boolean) As String

        HttpContext.Current.Session.Clear()
        If access = False Then
            GetLogout = System.Configuration.ConfigurationManager.AppSettings("logout").ToString & "?na=noaccess"
            HttpContext.Current.Session("ERRORMSG") = "You do not have access."
        Else
            GetLogout = System.Configuration.ConfigurationManager.AppSettings("logout").ToString
        End If

        HttpContext.Current.Response.Redirect(GetLogout)

    End Function 'EOF GetLogout
    Public Shared Function GetTeamMember(ByVal emp As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Team_Member"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "TeamMember")
            GetTeamMember = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "emp: " & emp & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetTeamMember : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetTeamMember : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetTeamMember = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetTeamMember
    Public Shared Function GetUGNDBPendingTasks(ByVal TeamMemberID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_UGNDB_Pending_Tasks"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@teamMemberID", SqlDbType.Int)
            myCommand.Parameters("@teamMemberID").Value = TeamMemberID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "UGNDBPendingTasks")

            GetUGNDBPendingTasks = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TeamMemberID: " & TeamMemberID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetUGNDBPendingTasks : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetUGNDBPendingTasks : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetUGNDBPendingTasks = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetUGNDBPendingTasks
    Public Shared Function GetUGNDBRecentTasks(ByVal TeamMemberID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_UGNDB_Recent_Tasks"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            If HttpContext.Current.Response.Cookies("UGNDB_User").Value Is Nothing Then
                Dim FullName As String = commonFunctions.getUserName()
                Dim LocationOfDot As Integer = InStr(FullName, ".")
                If LocationOfDot > 0 Then
                    Dim FirstName As String = Left(FullName, LocationOfDot - 1)
                    Dim FirstInitial As String = Left(FullName, 1)
                    Dim LastName As String = Right(FullName, Len(FullName) - LocationOfDot)

                    HttpContext.Current.Response.Cookies("UGNDB_User").Value = FirstInitial & LastName

                Else
                    HttpContext.Current.Response.Cookies("UGNDB_User").Value = FullName
                End If
            End If

            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@teamMemberID", SqlDbType.Int)
            myCommand.Parameters("@teamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            'if the developer is mimicking another user, then this value in incorrect but we will work on that later.
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "UGNDBRecentTasks")

            GetUGNDBRecentTasks = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TeamMemberID: " & TeamMemberID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetUGNDBRecentTasks : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetUGNDBRecentTasks : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetUGNDBRecentTasks = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetUGNDBRecentTasks
    Public Shared Function GetTeamMemberAlertedBackupOrDeptInCharge(ByVal TeamMemberID As Integer, ByVal SubscriptionID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Team_Member_Alerted_Backup_Or_Dept_In_Charge"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@teamMemberID", SqlDbType.Int)
            myCommand.Parameters("@teamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@subscriptionID", SqlDbType.Int)
            myCommand.Parameters("@subscriptionID").Value = SubscriptionID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "BackupTeamMemberInfo")
            GetTeamMemberAlertedBackupOrDeptInCharge = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TeamMemberID " & TeamMemberID _
            & ", SubscriptionID " & SubscriptionID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetTeamMemberAlertedBackupOrDeptInCharge : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetTeamMemberAlertedBackupOrDeptInCharge : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetTeamMemberAlertedBackupOrDeptInCharge = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetTeamMemberAlertedBackupOrDeptInCharge
#End Region

#Region "CONVERSION & VALIDATION"
    Public Shared Function isFileInUse(ByVal sFileName As String) As Boolean

        Dim nFileNum As Integer

        ' If the file is already opened by another process and the specified type of access
        ' is not allowed the Open operation fails and an error occurs.
        Try
            nFileNum = FileSystem.FreeFile()
            FileSystem.FileOpen(nFileNum, sFileName, OpenMode.Binary, OpenAccess.Read, OpenShare.LockReadWrite)
            FileSystem.FileClose(nFileNum)
        Catch
            isFileInUse = True
            Exit Function
        End Try

        isFileInUse = False
    End Function 'EOF isFileInUse
    Public Shared Function isFileWatchedAndAvailable(ByVal sFilename As String) As Boolean
        'Handles fileWatcher.Created       
        Dim check_count As Integer

        check_count = 0

check_again:
        If isFileInUse(sFilename) Then
            System.Threading.Thread.Sleep(500)
            check_count = check_count + 1
            If check_count = 10 Then
                Return False
                Exit Function
            End If
            GoTo check_again
        End If

        Return True
    End Function 'EOF isFileWatchedAndAvailable
    Public Shared Function removeSpecialChar(ByVal fieldText As String) As String

        Try
            Dim fieldData As String = fieldText

            'for money fields, remove $ symbol before using convert statements and saving to db
            '$ symbol is for display only, handled by .net
            fieldData = fieldData.Replace("$", "")

            removeSpecialChar = fieldData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "fieldText: " & fieldText & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "removeSpecialChar : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("removeSpecialChar : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            removeSpecialChar = ""
        End Try

    End Function 'EOF removeSpecialChar
    Public Shared Function convertSpecialChar(ByVal fieldText As String, ByVal isFileName As Boolean) As String

        Try
            Dim fieldData As String = fieldText

            If isFileName Then
                'prevent special characters in filename
                fieldData = fieldData.Replace("\", "_")
                fieldData = fieldData.Replace("/", "_")
                fieldData = fieldData.Replace(":", "_")
                fieldData = fieldData.Replace("*", "_")
                fieldData = fieldData.Replace("?", "_")
                fieldData = fieldData.Replace("""", "_")
                fieldData = fieldData.Replace("<", "_")
                fieldData = fieldData.Replace(">", "_")
                fieldData = fieldData.Replace("|", "_")
                fieldData = fieldData.Replace(" ", "_")
                fieldData = fieldData.Replace("'", "_")
                fieldData = fieldData.Replace("%", "_")
                fieldData = fieldData.Replace(";", "_")
                fieldData = fieldData.Replace("!", "_")
                fieldData = fieldData.Replace("@", "_")
                fieldData = fieldData.Replace("#", "_")
                fieldData = fieldData.Replace("$", "_")
                fieldData = fieldData.Replace("&", "_")
                fieldData = fieldData.Replace("=", "_")
                fieldData = fieldData.Replace("~", "_")
                'fieldData = fieldData.Replace(".", "_")
            Else
                'before Saving to Database, ensure quotes are retained
                fieldData = fieldData.Replace("'", "''")
                fieldData = fieldData.Replace("<", "''")
                fieldData = fieldData.Replace(">", "''")
            End If

            convertSpecialChar = fieldData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "fieldText: " & fieldText & "isFileName: " & isFileName & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "convertSpecialChar : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("convertSpecialChar : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            convertSpecialChar = ""

        End Try

    End Function 'EOF convertSpecialChar

    Public Shared Function replaceSpecialChar(ByVal fieldText As String, ByVal isFileName As Boolean) As String

        Try
            Dim fieldData As String = fieldText

            If isFileName Then
                'prevent special characters in filename
                fieldData = fieldData.Replace("\", "_")
                fieldData = fieldData.Replace("/", "_")
                fieldData = fieldData.Replace(":", "_")
                fieldData = fieldData.Replace("*", "_")
                fieldData = fieldData.Replace("?", "_")
                fieldData = fieldData.Replace("""", "_")
                fieldData = fieldData.Replace("<", "_")
                fieldData = fieldData.Replace(">", "_")
                fieldData = fieldData.Replace("|", "_")
                fieldData = fieldData.Replace(" ", "_")
                fieldData = fieldData.Replace("'", "_")
                fieldData = fieldData.Replace("%", "_")
                fieldData = fieldData.Replace(";", "_")
                fieldData = fieldData.Replace("!", "_")
                fieldData = fieldData.Replace("@", "_")
                fieldData = fieldData.Replace("#", "_")
                fieldData = fieldData.Replace("$", "_")
                fieldData = fieldData.Replace("&", "_")
                fieldData = fieldData.Replace("=", "_")
                fieldData = fieldData.Replace("~", "_")
                ''fieldData = fieldData.Replace(".", "_")
            Else
                'before Saving to Database, ensure quotes are retained
                fieldData = fieldData.Replace("<", "''")
                fieldData = fieldData.Replace(">", "''")
            End If

            replaceSpecialChar = fieldData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "fieldText: " & fieldText & "isFileName: " & isFileName & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "replaceSpecialChar : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("replaceSpecialChar : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            replaceSpecialChar = ""

        End Try

    End Function 'EOF replaceSpecialChar

    Public Function readSpecialChar(ByVal fieldText As String) As String

        Try
            Dim fieldData As String = fieldText

            'retrieving from Database, ensure quotes are retained
            fieldText = fieldText.Replace("''", "'")

            readSpecialChar = fieldData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "fieldText: " & fieldText & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "readSpecialChar : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("readSpecialChar : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            readSpecialChar = ""
        End Try

    End Function 'EOF readSpecialChar
    Public Shared Function CheckDataSet(ByVal ds As DataSet) As Boolean

        Dim bReturnValue As Boolean = False

        Try
            'check if dataset has results, if not null table, if table set exists and if rows exist
            If ds IsNot Nothing Then
                If ds.Tables.Count > 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        bReturnValue = True
                    End If
                End If
            End If

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CheckDataset : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CheckDataset : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            CheckDataSet = False
        End Try

        CheckDataSet = bReturnValue
    End Function 'EOF CheckDataSet
    Public Shared Function CheckDataTable(ByVal dt As DataTable) As Boolean

        Dim bReturnValue As Boolean = False

        Try
            'check if DataTable has results, if not null table, if table set exists and if rows exist
            If dt IsNot Nothing Then
                If dt.Rows.Count > 0 Then
                    bReturnValue = True
                End If
            End If

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CheckDataTable : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CheckDataTable : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            CheckDataTable = False
        End Try

        CheckDataTable = bReturnValue
    End Function 'EOF CheckDataTable
    Public Shared Function GetCCDValue(ByVal cddValue As String) As String

        Dim Value As String = cddValue
        Dim sValue = cddValue.Trim
        Dim isValueLen As Integer = Len(sValue)
        Dim isValueStartPos As Integer = InStr(Value, ":")
        sValue = sValue.Substring(0, isValueStartPos - 1)
        Value = commonFunctions.convertSpecialChar(sValue, False)

        Return Value

    End Function 'EOF GetCCDValue


#End Region

#Region "CUSTOMER INFO"
    ''** (LREY) GetCustomerSoldTo will become obsolete as it relates to SoldTo/CABBV values. 
    Public Shared Function GetCustomerSoldTo(ByVal CustomerValue As String) As Integer

        Dim strReturnValue As Integer = 0

        Try

            If CustomerValue <> "" Then
                Dim Pos As Integer = InStr(CustomerValue, "|")

                Dim tempSoldTo As Integer = 0
                If Not (Pos = 0) Then
                    tempSoldTo = Microsoft.VisualBasic.Left(CustomerValue, Pos - 1)
                End If

                strReturnValue = tempSoldTo
            End If

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CustomerValue: " & CustomerValue & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetCustomerSoldTo : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCustomerSoldTo : " & commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        End Try

        GetCustomerSoldTo = strReturnValue

    End Function 'EOF GetCustomerSoldTo

    ''** (LREY) GetCustomerCABBV will become obsolete as it relates to SoldTo/CABBV values. 
    ''**This will be replace by all Customer value fields wherever used.
    Public Shared Function GetCustomerCABBV(ByVal CustomerValue As String) As String

        Dim strReturnValue As String = ""

        Try

            If CustomerValue <> "" Then
                Dim Pos As Integer = InStr(CustomerValue, "|")
                Dim tempCABBV As String = ""

                If Not (Pos = 0) Then
                    tempCABBV = Microsoft.VisualBasic.Right(CustomerValue, Len(CustomerValue) - Pos)
                End If

                strReturnValue = tempCABBV
            End If

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CustomerValue: " & CustomerValue & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetCustomerCABBV : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCustomerCABBV : " & commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        End Try

        GetCustomerCABBV = strReturnValue

    End Function 'EOF GetCustomerCABBV

    ''**(LREY) GetCustomer will become obsolete as it relates to SoldTo/CABBV values. 
    ''**Will not be used. Replaced by sp_Get_OEMManfucturer. SoldTo|CABBV not used in ERP.
    Public Shared Function GetCustomer(ByVal ExcludeFuture As Boolean) As DataSet
        ''Used in PF;
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Customer"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ExcludeFuture", SqlDbType.Bit)
            myCommand.Parameters("@ExcludeFuture").Value = ExcludeFuture

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Customer")
            GetCustomer = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ExcludeFuture: " & ExcludeFuture & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCustomer : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCustomer : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetCustomer = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetCustomer

    ''**(LREY) GetCustomerDestination will become obsolete as it relates to DABBV values. 
    ''**Will not be used. Replaced by sp_Get_OEMManfucturer. DABBV not used in ERP.
    Public Shared Function GetCustomerDestination(ByVal custID As String) As DataSet
        ''Used in PF;
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Customer_Destination"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            If custID Is Nothing Then
                custID = ""
            End If
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Parameters.Add("@CABBV", SqlDbType.VarChar)
            myCommand.Parameters("@CABBV").Value = custID
            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CustomerDestination")
            GetCustomerDestination = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CustID " & custID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCustomerDestination : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCustomerDestination : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCustomerDestination = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetCustomerDestination

    ''**(LREY) GetCABBV will become obsolete as it relates to CABBV values. 
    ''**Will not be used. Replaced by sp_Get_OEMManfucturer. CABBV not used in ERP.
    Public Shared Function GetCABBV() As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_CABBV"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "OEM")

            GetCABBV = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCABBV : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCABBV : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetCABBV = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetCABBV

    ''**(LREY) GetDABBV will become obsolete as it relates to DABBV values. 
    ''**Will not be used. Replaced by sp_Get_OEMManfucturer. DABBV not used in ERP.
    Public Shared Function GetDABBV(ByVal COMPNY As String, ByVal OEM As String, ByVal SortByObsolete As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_DABBV"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@COMPNY", SqlDbType.VarChar)
            myCommand.Parameters("@COMPNY").Value = COMPNY

            myCommand.Parameters.Add("@OEM", SqlDbType.VarChar)
            myCommand.Parameters("@OEM").Value = OEM

            myCommand.Parameters.Add("@SortByObsolete", SqlDbType.Bit)
            myCommand.Parameters("@SortByObsolete").Value = SortByObsolete

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DABBV")

            GetDABBV = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetDABBV : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDABBV : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetDABBV = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetDABBV

    ''**(LREY) GetCABBVbyOEM will become obsolete as it relates to CABBV values. 
    ''**Will not be used. Replaced by sp_Get_OEMManfucturer. CABBV not used in ERP.
    Public Shared Function GetCABBVbyOEM(ByVal COMPNY As String, ByVal OEM As String) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_CABBV_by_OEM"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@COMPNY", SqlDbType.VarChar)
            myCommand.Parameters("@COMPNY").Value = COMPNY

            myCommand.Parameters.Add("@OEM", SqlDbType.VarChar)
            myCommand.Parameters("@OEM").Value = OEM

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CABBVOEM")

            GetCABBVbyOEM = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCABBVbyOEM : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCABBVbyOEM : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetCABBVbyOEM = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetCABBVbyOEM

    ''**(LREY) GetCABBVbyOEMMfg will become obsolete as it relates to CABBV values. 
    ''**Will not be used. Replaced by sp_Get_OEMManfucturer. CABBV not used in ERP.
    Public Shared Function GetCABBVbyOEMMfg(ByVal COMPNY As String, ByVal OEMMfg As String) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_CABBV_by_OEMMfg"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@COMPNY", SqlDbType.VarChar)
            myCommand.Parameters("@COMPNY").Value = COMPNY

            myCommand.Parameters.Add("@OEMMfg", SqlDbType.VarChar)
            myCommand.Parameters("@OEMMfg").Value = OEMMfg

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CABBVOEMMFG")

            GetCABBVbyOEMMfg = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCABBVbyOEMMfg : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCABBVbyOEMMfg : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetCABBVbyOEMMfg = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetCABBVbyOEMMfg

    ''**(LREY) GetOEMMfgCABBV will become obsolete as it relates to CABBV values. 
    Public Shared Function GetOEMMfgCABBV(ByVal COMPNY As String) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_OEMMfg_CABBV"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@COMPNY", SqlDbType.VarChar)
            myCommand.Parameters("@COMPNY").Value = COMPNY

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "OEMMfgCABBV")

            GetOEMMfgCABBV = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetOEMMfgCABBV : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetOEMMfgCABBV : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetOEMMfgCABBV = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetOEMMfgCABBV

    ''**(LREY) GetOEMbyCOMPNY will become obsolete as it relates to CABBV values. 
    ''**Will not be used. Replaced by sp_Get_OEMManfucturer. CABBV not used in ERP.
    Public Shared Function GetOEMbyCOMPNY(ByVal COMPNY As String) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_OEM_by_COMPNY"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@COMPNY", SqlDbType.VarChar)
            myCommand.Parameters("@COMPNY").Value = COMPNY

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "COMPNYOEM")

            GetOEMbyCOMPNY = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetOEMbyCOMPNY : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetOEMbyCOMPNY : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetOEMbyCOMPNY = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetOEMbyCOMPNY

    Public Shared Function GetOEMMfgByOEM(ByVal OEM As String) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_OEM_Mfg_by_OEM"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@OEM", SqlDbType.VarChar)
            myCommand.Parameters("@OEM").Value = OEM

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "OEMMfgByOEM")

            GetOEMMfgByOEM = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetOEMMfgByOEM : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetOEMMfgByOEM : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetOEMMfgByOEM = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetOEMMfgByOEM

    ''**(LREY) GetSOLDTObyCOMPNYbyCABBVbyOEM will become obsolete as it relates to SoldTo|CABBV values. 
    ''**Will not be used. Replaced by sp_Get_OEMManfucturer. SoldTo|CABBV not used in ERP.
    Public Shared Function GetSOLDTObyCOMPNYbyCABBVbyOEM(ByVal COMPNY As String, ByVal OEM As String, ByVal CABBV As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_SoldTo_by_COMPNY_by_CABBV_by_OEM"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@COMPNY", SqlDbType.VarChar)
            myCommand.Parameters("@COMPNY").Value = COMPNY

            myCommand.Parameters.Add("@OEM", SqlDbType.VarChar)
            myCommand.Parameters("@OEM").Value = OEM

            myCommand.Parameters.Add("@CABBV", SqlDbType.VarChar)
            myCommand.Parameters("@CABBV").Value = CABBV

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "COMPNYOEM")

            GetSOLDTObyCOMPNYbyCABBVbyOEM = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetSOLDTObyCOMPNYbyCABBVbyOEM : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetSOLDTObyCOMPNYbyCABBVbyOEM : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetSOLDTObyCOMPNYbyCABBVbyOEM = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetSOLDTObyCOMPNYbyCABBVbyOEM

    ''**(LREY) GetOEMSoldToCABBVbyOEMMfg will become obsolete as it relates to SoldTo|CABBV values. 
    ''**Will not be used. Replaced by sp_Get_OEMManfucturer. SoldTo|CABBV not used in ERP.
    Public Shared Function GetOEMSoldToCABBVbyOEMMfg(ByVal OEM As String, ByVal OEMManufacturer As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_OEM_SoldTo_CABBV_by_OEM_Mfg"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@OEM", SqlDbType.VarChar)
            myCommand.Parameters("@OEM").Value = OEM

            myCommand.Parameters.Add("@OEMManufacturer", SqlDbType.VarChar)
            myCommand.Parameters("@OEMManufacturer").Value = OEMManufacturer

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "OSCOM")

            GetOEMSoldToCABBVbyOEMMfg = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetOEMSoldToCABBVbyOEMMfg : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetOEMSoldToCABBVbyOEMMfg : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetOEMSoldToCABBVbyOEMMfg = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetOEMSoldToCABBVbyOEMMfg

    ''**(LREY) GetCustomerPartBPCSPartRelate will become obsolete as it relates to F3 PXRef values. 
    Public Shared Function GetCustomerPartBPCSPartRelate(ByVal PartNo As String, ByVal customerPartNo As String, ByVal customerPartName As String, ByVal cabbv As String, ByVal barCodePartNo As String) As DataSet
        'Accounts Receivable Tracking
        'Data Maintenance
        'Product Engeineering DMS
        'Request For Development

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Customer_Part_BPCS_Part_Relate"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@bpcsPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@bpcsPartNo").Value = PartNo

            myCommand.Parameters.Add("@customerPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@customerPartNo").Value = customerPartNo

            myCommand.Parameters.Add("@customerPartName", SqlDbType.VarChar)
            myCommand.Parameters("@customerPartName").Value = customerPartName

            myCommand.Parameters.Add("@cabbv", SqlDbType.VarChar)
            myCommand.Parameters("@cabbv").Value = cabbv

            myCommand.Parameters.Add("@barCodePartNo", SqlDbType.VarChar)
            myCommand.Parameters("@barCodePartNo").Value = barCodePartNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetCustomerBPCSInfo")
            GetCustomerPartBPCSPartRelate = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PartNo: " & PartNo & ", customerPartNo: " & customerPartNo & ", customerPartName: " & customerPartName & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetCustomerPartPartRelate : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCustomerPartPartRelate : " & commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCustomerPartBPCSPartRelate = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetCustomerPartBPCSPartRelate

    ''**(LREY) GetShipTo will become obsolete as it relates to SHIPTO values. 
    Public Shared Function GetShipTo() As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_ShipTo"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ShipTo")

            GetShipTo = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetShipTo : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetShipTo : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetShipTo = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetShipTo

    ''**(LREY) GetSoldTo will become obsolete as it relates to SHIPTO values. 
    Public Shared Function GetSoldTo() As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_SoldTo"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "SoldTo")

            GetSoldTo = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetSoldTo : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetSoldTo : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetSoldTo = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetSoldTo

    ''**(LREY) GetFutureCustomer will become obsolete as it relates to SHIPTO values. 
    Public Shared Function GetFutureCustomer() As DataSet
        ''Used in Costing and RFD
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Future_Customer"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "FutureCustomers")
            GetFutureCustomer = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetFutureCustomer : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetFutureCustomer : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetFutureCustomer = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetFutureCustomer
#End Region

#Region "PLATFORM/PROGRAM SOLUTIONS - 201105 TO 201108 -lrey"
    Public Shared Function GetOEM() As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_OEM"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "OEM")

            GetOEM = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetOEM : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetOEM : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetOEM = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetOEM
    Public Shared Function GetOEMManufacturer(ByVal OEMManufacturer As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_OEM_Manufacturer"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@OEMManufacturer", SqlDbType.VarChar)
            myCommand.Parameters("@OEMManufacturer").Value = OEMManufacturer

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "OEM")

            GetOEMManufacturer = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetOEMManufacturer : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetOEMManufacturer : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetOEMManufacturer = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetOEMManufacturer
    Public Shared Function GetOEMbyOEMMfg(ByVal OEMManufacturer As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_OEM_by_OEM_Mfg"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@OEMManufacturer", SqlDbType.VarChar)
            myCommand.Parameters("@OEMManufacturer").Value = OEMManufacturer

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "OEMmfg")

            GetOEMbyOEMMfg = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetOEMbyOEMMfg : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetOEMbyOEMMfg : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetOEMbyOEMMfg = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetOEMbyOEMMfg
    Public Shared Function GetMake(ByVal MakeName As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Make"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@makeName", SqlDbType.VarChar)
            myCommand.Parameters("@makeName").Value = MakeName

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Make")
            GetMake = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "MakeName: " & MakeName & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetMake : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetMake : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetMake = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetMake
    Public Shared Function GetProgramByMake(ByVal Make As String, ByVal OEMManufacturer As String, ByVal PlatformID As Integer, ByVal Model As String) As DataSet
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Program_by_Make"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@Make", SqlDbType.VarChar)
            myCommand.Parameters("@Make").Value = Make

            myCommand.Parameters.Add("@OEMManufacturer", SqlDbType.VarChar)
            myCommand.Parameters("@OEMManufacturer").Value = OEMManufacturer

            myCommand.Parameters.Add("@PlatformID", SqlDbType.Int)
            myCommand.Parameters("@PlatformID").Value = PlatformID

            myCommand.Parameters.Add("@Model", SqlDbType.VarChar)
            myCommand.Parameters("@Model").Value = Model

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "OEMMfg")

            GetProgramByMake = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetProgramByMake : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetProgramByMake : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetProgramByMake = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetProgramByMake
    Public Shared Function GetModel(ByVal ModelName As String, ByVal Make As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Model"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ModelName", SqlDbType.VarChar)
            myCommand.Parameters("@ModelName").Value = ModelName

            myCommand.Parameters.Add("@Make", SqlDbType.VarChar)
            myCommand.Parameters("@Make").Value = Make

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Model")
            GetModel = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ModelName " & ModelName & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetModel : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetModel : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetModel = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetModel
    Public Shared Function GetProgram(ByVal Program As String, ByVal ProgramCode As String, ByVal Make As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Program"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProgramName", SqlDbType.VarChar)
            myCommand.Parameters("@ProgramName").Value = Program

            myCommand.Parameters.Add("@ProgramCode", SqlDbType.VarChar)
            myCommand.Parameters("@ProgramCode").Value = ProgramCode

            myCommand.Parameters.Add("@Make", SqlDbType.VarChar)
            myCommand.Parameters("@Make").Value = Make


            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Program")
            GetProgram = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetProgram : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetProgram = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetProgram
    Public Shared Function GetProgramMake() As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Program_Make"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Program")
            GetProgramMake = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetProgramMake : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetProgramMake : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetProgramMake = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetProgramMake added 08/13/09 rcarlson
    Public Shared Function GetProgramByCABBV(ByVal ProgramID As Integer, ByVal CABBV As String) As DataSet
        ''Used in PF;
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Program_by_CABBV"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID
            myCommand.Parameters.Add("@CABBV", SqlDbType.VarChar)
            myCommand.Parameters("@CABBV").Value = CABBV
            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Program")
            GetProgramByCABBV = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProgramID: " & ProgramID & ", CABBV: " & CABBV & _
            "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetProgramByCABBV : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetProgramByCABBV : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetProgramByCABBV = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetProgramByCABBV added 04/29/08 lrey
    Public Shared Function GetProgramByCABBVDABBV(ByVal ProgramID As Integer, ByVal CABBV As String, ByVal SoldTo As Integer) As DataSet
        ''Used in PF;
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Program_by_CABBV_DABBV"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID
            myCommand.Parameters.Add("@CABBV", SqlDbType.VarChar)
            myCommand.Parameters("@CABBV").Value = CABBV
            myCommand.Parameters.Add("@SoldTo", SqlDbType.Int)
            myCommand.Parameters("@SoldTo").Value = SoldTo
            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Program")
            GetProgramByCABBVDABBV = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProgramID: " & ProgramID & ", CABBV: " & CABBV & _
            ", SoldTo: " & SoldTo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetProgramByCABBVDABBV : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetProgramByCABBVDABBV : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetProgramByCABBVDABBV = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetProgramByCABBVDABBV added 03/19/08 lrey
    Public Shared Function GetPlatform(ByVal PlatformID As Integer, ByVal PlatformName As String, ByVal OEMManufacturer As String, ByVal DisplayUGNBusiness As String, ByVal DisplayCurrentPlatform As String, ByVal SortBy As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Platform"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@PlatformID", SqlDbType.Int)
            myCommand.Parameters("@PlatformID").Value = PlatformID

            myCommand.Parameters.Add("@PlatformName", SqlDbType.VarChar)
            myCommand.Parameters("@PlatformName").Value = PlatformName

            myCommand.Parameters.Add("@OEMManufacturer", SqlDbType.VarChar)
            myCommand.Parameters("@OEMManufacturer").Value = OEMManufacturer

            myCommand.Parameters.Add("@DisplayUGNBusiness", SqlDbType.VarChar)
            myCommand.Parameters("@DisplayUGNBusiness").Value = DisplayUGNBusiness

            myCommand.Parameters.Add("@DisplayCurrentPlatform", SqlDbType.VarChar)
            myCommand.Parameters("@DisplayCurrentPlatform").Value = DisplayCurrentPlatform

            myCommand.Parameters.Add("@SortBy", SqlDbType.VarChar)
            myCommand.Parameters("@SortBy").Value = SortBy

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Platform")
            GetPlatform = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetPlatform : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetPlatform : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetPlatform = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetPlatform
    Public Shared Function GetPlatformProgram(ByVal PlatformID As Integer, ByVal ProgramID As Integer, ByVal ProgramCode As String, ByVal ProgramName As String, ByVal Make As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Platform_Program"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@PlatformID", SqlDbType.Int)
            myCommand.Parameters("@PlatformID").Value = PlatformID

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@ProgramCode", SqlDbType.VarChar)
            myCommand.Parameters("@ProgramCode").Value = ProgramCode

            myCommand.Parameters.Add("@ProgramName", SqlDbType.VarChar)
            myCommand.Parameters("@ProgramName").Value = ProgramName

            myCommand.Parameters.Add("@Make", SqlDbType.VarChar)
            myCommand.Parameters("@Make").Value = Make

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "PlatformProgram")
            GetPlatformProgram = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetPlatformProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetPlatformProgram : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetPlatformProgram = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetPlatformProgram
    Public Shared Function GetProgramVolume(ByVal ProgramID As Integer, ByVal YearID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Program_Volume"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@YearID", SqlDbType.Int)
            myCommand.Parameters("@YearID").Value = YearID


            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ProgramVolume")
            GetProgramVolume = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetProgramVolume : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetProgramVolume : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetProgramVolume = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetProgramVolume 
    Public Shared Function GetRegion(ByVal RegionID As Integer, ByVal Region As String, ByVal Country As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Region"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RegionID", SqlDbType.Int)
            myCommand.Parameters("@RegionID").Value = RegionID

            myCommand.Parameters.Add("@Region", SqlDbType.VarChar)
            myCommand.Parameters("@Region").Value = Region

            myCommand.Parameters.Add("@Country", SqlDbType.VarChar)
            myCommand.Parameters("@Country").Value = Country

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Region")

            GetRegion = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetRegion : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRegion : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetRegion = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetRegion
    Public Shared Function GetAssemblyPlantLocation(ByVal APID As Integer, ByVal Assembly As String, ByVal Country As String, ByVal OEMMfg As String, ByVal AssemblyType As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Assembly_Plant_Location"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@APID", SqlDbType.Int)
            myCommand.Parameters("@APID").Value = APID

            myCommand.Parameters.Add("@Assembly", SqlDbType.VarChar)
            myCommand.Parameters("@Assembly").Value = Assembly

            myCommand.Parameters.Add("@Country", SqlDbType.VarChar)
            myCommand.Parameters("@Country").Value = Country

            myCommand.Parameters.Add("@OEMMfg", SqlDbType.VarChar)
            myCommand.Parameters("@OEMMfg").Value = OEMMfg

            myCommand.Parameters.Add("@AssemblyType", SqlDbType.VarChar)
            myCommand.Parameters("@AssemblyType").Value = AssemblyType

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "AssemblyPlantLocation")

            GetAssemblyPlantLocation = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetAssemblyPlantLocation : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetAssemblyPlantLocation : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetAssemblyPlantLocation = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetAssemblyPlantLocation
    Public Shared Function GetVehicleType(ByVal VTID As Integer, ByVal VehicleType As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Vehicle_Type"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@VTID", SqlDbType.Int)
            myCommand.Parameters("@VTID").Value = VTID

            myCommand.Parameters.Add("@VehicleType", SqlDbType.VarChar)
            myCommand.Parameters("@VehicleType").Value = VehicleType

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "VehicleType")

            GetVehicleType = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetVehicleType : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetVehicleType : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetVehicleType = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetVehicleType
    Public Shared Function GetBodyStyle(ByVal BSID As Integer, ByVal BodyStyle As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Body_Style"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@BSID", SqlDbType.Int)
            myCommand.Parameters("@BSID").Value = BSID

            myCommand.Parameters.Add("@BodyStyle", SqlDbType.VarChar)
            myCommand.Parameters("@BodyStyle").Value = BodyStyle

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "BodyStyle")

            GetBodyStyle = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetBodyStyle : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetBodyStyle : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetBodyStyle = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetBodyStyle
    Public Shared Sub DeletePlatformCookies()

        Try
            HttpContext.Current.Response.Cookies("P_PNAME").Value = ""
            HttpContext.Current.Response.Cookies("P_PNAME").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("P_OEMMF").Value = ""
            HttpContext.Current.Response.Cookies("P_OEMMF").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("P_DUB").Value = ""
            HttpContext.Current.Response.Cookies("P_DUB").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("P_DCP").Value = ""
            HttpContext.Current.Response.Cookies("P_DCP").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeletePlatformCookies : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeletePlatformCookies : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try
    End Sub  'EOF DeletePlatformCookies
    Public Shared Sub DeletePlatformProgramCookies()

        Try
            HttpContext.Current.Response.Cookies("PP_PgmCode").Value = ""
            HttpContext.Current.Response.Cookies("PP_PgmCode").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PP_PName").Value = ""
            HttpContext.Current.Response.Cookies("PP_PName").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PP_Make").Value = ""
            HttpContext.Current.Response.Cookies("PP_Make").Expires = DateTime.Now.AddDays(-1)


        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeletePlatformProgramCookies : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeletePlatformProgramCookies : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try
    End Sub  'EOF DeletePlatformProgramCookies
    Public Shared Sub DeleteAssemblyPlantLocationCookies()

        Try
            HttpContext.Current.Response.Cookies("APLM_APL").Value = ""
            HttpContext.Current.Response.Cookies("APLM_APL").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("APLM_Ctry").Value = ""
            HttpContext.Current.Response.Cookies("APLM_Ctry").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("APLM_OMfg").Value = ""
            HttpContext.Current.Response.Cookies("APLM_OMfg").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteAssemblyPlantLocationCookies : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteAssemblyPlantLocationCookies : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try
    End Sub  'EOF DeleteAssemblyPlantLocationCookies
    Public Shared Sub DeleteModelCookies()

        Try
            HttpContext.Current.Response.Cookies("DM1_MName").Value = ""
            HttpContext.Current.Response.Cookies("DM1_MName").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("DM1_Make").Value = ""
            HttpContext.Current.Response.Cookies("DM1_Make").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteModelCookies : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteModelCookies : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try
    End Sub  'EOF DeleteModelCookies
#End Region

#Region "GENERAL"
    Public Shared Function GetUGNFacility(ByVal facID As String) As DataSet
        ''Used in PF;
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_UGNFacility"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Facility")
            GetUGNFacility = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "facID: " & facID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetUGNFacility : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetUGNFacility : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetUGNFacility = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetUGNFacility
    Public Shared Function GetDepartment(ByVal DepartmentName As String, ByVal UGNFacility As String, ByVal Filter As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Department"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@departmentName ", SqlDbType.VarChar)
            myCommand.Parameters("@departmentName ").Value = DepartmentName

            myCommand.Parameters.Add("@ugnFacility", SqlDbType.VarChar)
            myCommand.Parameters("@ugnFacility").Value = UGNFacility

            myCommand.Parameters.Add("@filter", SqlDbType.Bit)
            myCommand.Parameters("@filter").Value = Filter

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Department")
            GetDepartment = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DepartmentName: " & DepartmentName & ", UGNFacility: " & UGNFacility _
            & ", Filter: " & Filter & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetDepartment : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDepartment : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetDepartment = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetDepartment
    Public Shared Function GetDepartmentGLNo(ByVal UGNFacility As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Department_GLNo"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ugnFacility", SqlDbType.VarChar)
            myCommand.Parameters("@ugnFacility").Value = UGNFacility

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Department")
            GetDepartmentGLNo = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "UGNFacility: " & UGNFacility _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetDepartmentGLNo : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDepartmentGLNo : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetDepartmentGLNo = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetDepartmentGLNo
    Public Shared Function GetDepartmentLWK(ByVal UGNFacility As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Department_LWK"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ugnFacility", SqlDbType.VarChar)
            myCommand.Parameters("@ugnFacility").Value = UGNFacility

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DLWK")
            GetDepartmentLWK = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "UGNFacility: " & UGNFacility _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetDepartmentLWK : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDepartmentLWK : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetDepartmentLWK = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetDepartmentLWK
    Public Shared Function GetDepartmentWorkCenter(ByVal UGNFacility As String, ByVal DeptNo As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Department_WorkCenter"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ugnFacility", SqlDbType.VarChar)
            myCommand.Parameters("@ugnFacility").Value = UGNFacility

            myCommand.Parameters.Add("@DeptNo", SqlDbType.Int)
            myCommand.Parameters("@DeptNo").Value = DeptNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DWC")
            GetDepartmentWorkCenter = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "UGNFacility: " & UGNFacility _
            & ", DeptNo: " & DeptNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetDepartmentWorkCenter : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDepartmentWorkCenter : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetDepartmentWorkCenter = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetDepartmentWorkCenter
    Public Shared Function GetCell(ByVal CellID As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Cell"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Cell")
            GetCell = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CellID " & CellID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCell : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCell : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetCell = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetCell
    Public Shared Function GetUnit(ByVal UnitID As Integer, ByVal UnitName As String, ByVal UnitAbbr As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Unit"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@unitID", SqlDbType.Int)
            myCommand.Parameters("@unitID").Value = UnitID

            myCommand.Parameters.Add("@unitName", SqlDbType.VarChar)
            myCommand.Parameters("@unitName").Value = UnitName

            myCommand.Parameters.Add("@unitAbbr", SqlDbType.VarChar)
            myCommand.Parameters("@unitAbbr").Value = UnitAbbr

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Unit")
            GetUnit = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "UnitID: " & UnitID & ", UnitName: " & UnitName _
            & ", UnitAbbr: " & UnitAbbr & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetUnit : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetUnit : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetUnit = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetUnit
    Public Shared Function GetRoyalty(ByVal RoyaltyName As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Royalty"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RoyaltyName", SqlDbType.VarChar)
            myCommand.Parameters("@RoyaltyName").Value = RoyaltyName

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Royalty")
            GetRoyalty = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetRoyalty : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRoyalty : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetRoyalty = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetRoyalty
    Public Shared Function GetPurchasedGood(ByVal PurchasedGoodName As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Purchased_Good"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "PurchasedGood")
            GetPurchasedGood = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PurchasedGoodName " & PurchasedGoodName & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetPurchasedGood : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetPurchasedGood : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetPurchasedGood = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetPurchasedGood
    Public Shared Function GetProductTechnology(ByVal ProductTechnologyID As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Product_Technology"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ProductTechnology")
            GetProductTechnology = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ProductTechnologyID " & ProductTechnologyID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetProductTechnology : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetProductTechnology : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetProductTechnology = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetProductTechnology
    Public Shared Function GetCommodityClass(ByVal CCID As Integer, ByVal CommodityClass As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Commodity_Class"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CCID", SqlDbType.Int)
            myCommand.Parameters("@CCID").Value = CCID

            myCommand.Parameters.Add("@CommodityClass", SqlDbType.VarChar)
            myCommand.Parameters("@CommodityClass").Value = CommodityClass


            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CommodityClass")
            GetCommodityClass = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CommodityClass " & CommodityClass & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCommodityClass : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCommodityClass : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetCommodityClass = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetCommodityClass
    Public Shared Function GetCommodity(ByVal CommodityID As Integer, ByVal CommodityName As String, ByVal CommodityClass As String, ByVal CCID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Commodity"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CommodityID", SqlDbType.Int)
            myCommand.Parameters("@CommodityID").Value = CommodityID

            myCommand.Parameters.Add("@CommodityName", SqlDbType.VarChar)
            myCommand.Parameters("@CommodityName").Value = CommodityClass

            myCommand.Parameters.Add("@CommodityClass", SqlDbType.VarChar)
            myCommand.Parameters("@CommodityClass").Value = CommodityClass

            myCommand.Parameters.Add("@CCID", SqlDbType.Int)
            myCommand.Parameters("@CCID").Value = CCID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Commodity")
            GetCommodity = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "commodityName " & CommodityName & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCommodity : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCommodity : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetCommodity = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetCommodity
    Public Shared Function GetFamily() As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Family"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Family")
            GetFamily = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetFamily : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetFamily : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetFamily = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetFamily
    Public Shared Function GetSubFamily(ByVal FamilyID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_SubFamily"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@familyID", SqlDbType.Int)
            myCommand.Parameters("@familyID").Value = FamilyID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "SubFamily")
            GetSubFamily = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FamilyID " & FamilyID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetSubFamily : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetSubFamily : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetSubFamily = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetSubFamily
    Public Shared Function GetPriceCode(ByVal PriceCode As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Price_Code"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If PriceCode Is Nothing Then
                PriceCode = ""
            End If

            myCommand.Parameters.Add("@priceCode", SqlDbType.VarChar)
            myCommand.Parameters("@priceCode").Value = PriceCode

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "BusinessProcessType")
            GetPriceCode = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetPriceCode : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> commonFunctions.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetPriceCode : " & commonFunctions.convertSpecialChar(ex.Message, False), "commonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetPriceCode = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetPriceCode
    Public Shared Function GetGLAccounts(ByVal GLNo As String) As DataSet
        ''Used in PF;
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_GLAccounts"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GLAccounts")
            GetGLAccounts = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetGLAccounts : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetGLAccounts : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetGLAccounts = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetGLAccounts
    Public Shared Function GetYear(ByVal YearID As String) As DataSet
        ''Used in PF;
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Year"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Year")
            GetYear = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "YearID " & YearID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetYear : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetYear : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetYear = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetYear
    Public Shared Function GetMonth(ByVal monthID As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Month"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Month")
            GetMonth = GetData
        Catch ex As Exception
            Dim rslt As String = ex.Message
            GetMonth = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetMonth
    Public Shared Function GetBusinessProcessType(ByVal BusinessProcessTypeID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Business_Process_Type"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@businessProcessTypeID", SqlDbType.Int)
            myCommand.Parameters("@businessProcessTypeID").Value = BusinessProcessTypeID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "BusinessProcessType")
            GetBusinessProcessType = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetBusinessProcessType : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> commonFunctions.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetBusinessProcessType : " & commonFunctions.convertSpecialChar(ex.Message, False), "commonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetBusinessProcessType = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetBusinessProcessType
    Public Shared Function GetBusinessProcessAction(ByVal BusinessProcessActionID As Integer, ByVal filterQuoteOnly As Boolean, ByVal isQuoteOnly As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Business_Process_Action"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@businessProcessActionID", SqlDbType.Int)
            myCommand.Parameters("@businessProcessActionID").Value = BusinessProcessActionID

            myCommand.Parameters.Add("@filterQuoteOnly", SqlDbType.Bit)
            myCommand.Parameters("@filterQuoteOnly").Value = filterQuoteOnly

            myCommand.Parameters.Add("@isQuoteOnly", SqlDbType.Bit)
            myCommand.Parameters("@isQuoteOnly").Value = isQuoteOnly

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "BusinessProcessAction")
            GetBusinessProcessAction = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "BusinessProcessActionID: " & BusinessProcessActionID _
            & ", isQuoteOnly: " & isQuoteOnly _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetBusinessProcessAction : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> commonFunctions.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetBusinessProcessAction : " & commonFunctions.convertSpecialChar(ex.Message, False), "commonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetBusinessProcessAction = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetBusinessProcessAction
    Public Shared Function GetCountry(ByVal Country As String) As DataSet
        ''Used in PF;
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Country"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Country")
            GetCountry = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCountry : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCountry : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetCountry = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetCountry

#End Region

#Region "WORKFLOW"
    Public Shared Function GetTeamMemberBySubscription(ByVal SubscriptionID As String) As DataSet
        ''Used in PF, RnD;
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Team_Member_by_Subscription"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SubscriptionID", SqlDbType.Int)
            myCommand.Parameters("@SubscriptionID").Value = SubscriptionID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Emp")
            GetTeamMemberBySubscription = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SubscriptionID " & SubscriptionID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetTeamMemberBySubscription : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetTeamMemberBySubscription : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetTeamMemberBySubscription = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetTeamMemberBySubscription
    Public Shared Function GetTeamMemberBySubscriptionByUGNFacility(ByVal SubscriptionID As Integer, ByVal UGNFacility As String) As DataSet
        ''used in New Chemical Form and RFD, New AR Module
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Team_Member_by_Subscription_By_UGNFacility"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SubscriptionID", SqlDbType.Int)
            myCommand.Parameters("@SubscriptionID").Value = SubscriptionID

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "TeamMemberBySubscriptionByUGNFacility")
            GetTeamMemberBySubscriptionByUGNFacility = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SubscriptionID " & SubscriptionID & ", UGNFacility " & UGNFacility _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetTeamMemberBySubscriptionByUGNFacility : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetTeamMemberBySubscriptionByUGNFacility : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetTeamMemberBySubscriptionByUGNFacility = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetTeamMemberBySubscriptionByUGNFacility
    Public Shared Function GetTeamMemberByWorkFlowAssignments(ByVal TeamMemberID As Integer, ByVal SubscriptionID As Integer, ByVal CommodityID As Integer, _
        ByVal CABBV As String, ByVal SoldTo As Integer) As DataSet

        ''Used in RnD and RFD
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Team_Member_by_WorkFlow_Assignments"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@SubscriptionID", SqlDbType.Int)
            myCommand.Parameters("@SubscriptionID").Value = SubscriptionID

            myCommand.Parameters.Add("@CommodityID", SqlDbType.Int)
            myCommand.Parameters("@CommodityID").Value = CommodityID

            myCommand.Parameters.Add("@CABBV", SqlDbType.VarChar)
            myCommand.Parameters("@CABBV").Value = CABBV

            myCommand.Parameters.Add("@SoldTo", SqlDbType.Int)
            myCommand.Parameters("@SoldTo").Value = SoldTo


            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Emp")
            GetTeamMemberByWorkFlowAssignments = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TeamMemberID: " & TeamMemberID _
            & ", SubscriptionID: " & SubscriptionID _
            & ", CommodityID: " & CommodityID _
            & ", CABBV: " & CABBV _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetTeamMemberByWorkFlowAssignments: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetTeamMemberByWorkFlowAssignments: " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetTeamMemberByWorkFlowAssignments = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetTeamMemberByWorkFlowAssignments
    Public Shared Function GetWorkFlowFamilyPurchasingAssignments(ByVal TeamMemberID As Integer, ByVal FamilyID As Integer) As DataSet
        ''Used in RFD;
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_WorkFlow_Family_Purchasing_Assignments"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@FamilyID", SqlDbType.Int)
            myCommand.Parameters("@FamilyID").Value = FamilyID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "WorkFlowFamilyPurchasingAssignments")
            GetWorkFlowFamilyPurchasingAssignments = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TeamMemberID: " & TeamMemberID & ", FamilyID: " & FamilyID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetWorkFlowFamilyPurchasingAssignments : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetWorkFlowFamilyPurchasingAssignments : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetWorkFlowFamilyPurchasingAssignments = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetWorkFlowFamilyPurchasingAssignments
    Public Shared Function GetWorkFlowMakeAssignments(ByVal Make As String, ByVal TeamMemberID As Integer, ByVal SubscriptionID As Integer) As DataSet
        ''Used in RFD;
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_WorkFlow_Make_Assignments"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If Make Is Nothing Then
                Make = ""
            End If

            myCommand.Parameters.Add("@Make", SqlDbType.VarChar)
            myCommand.Parameters("@Make").Value = Make

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@SubscriptionID", SqlDbType.Int)
            myCommand.Parameters("@SubscriptionID").Value = SubscriptionID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "WorkFlowMakeAssignments")
            GetWorkFlowMakeAssignments = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "Make: " & Make _
            & ", TeamMemberID: " & TeamMemberID _
            & ", SubscriptionID: " & SubscriptionID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetWorkFlowMakeAssignments : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetWorkFlowMakeAssignments : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetWorkFlowMakeAssignments = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetWorkFlowMakeAssignments
    Public Shared Function GetCommodityWithWorkFlowAssignments(ByVal CommodityID As Integer, ByVal CommodityName As String, ByVal TeamMemberID As Integer) As DataSet

        ''Used in RFD
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Commodity_With_Workflow_Assignments"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try

            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CommodityID", SqlDbType.Int)
            myCommand.Parameters("@CommodityID").Value = CommodityID

            myCommand.Parameters.Add("@CommodityName", SqlDbType.VarChar)
            myCommand.Parameters("@CommodityName").Value = CommodityName

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CommodityWithWorkFlowAssignments")
            GetCommodityWithWorkFlowAssignments = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CommodityID: " & CommodityID _
            & ", CommodityName: " & CommodityName _
            & ", TeamMemberID: " & TeamMemberID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCommodityWithWorkFlowAssignments: " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCommodityWithWorkFlowAssignments: " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetCommodityWithWorkFlowAssignments = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetCommodityWithWorkFlowAssignments
    Public Shared Function GetFamilyWithWorkFlowAssignments(ByVal FamilyID As Integer, ByVal FamilyName As String, ByVal TeamMemberID As Integer) As DataSet
        ''Used in RFD;
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Family_With_Workflow_Assignments"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@FamilyID", SqlDbType.Int)
            myCommand.Parameters("@FamilyID").Value = FamilyID

            myCommand.Parameters.Add("@FamilyName", SqlDbType.VarChar)
            myCommand.Parameters("@FamilyName").Value = FamilyName

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "FamilyWithWorkFlowAssignments")
            GetFamilyWithWorkFlowAssignments = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FamilyID: " & FamilyID _
            & ", FamilyName: " & FamilyName _
            & ", TeamMemberID: " & TeamMemberID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetFamilyWithWorkFlowAssignments : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetFamilyWithWorkFlowAssignments : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetFamilyWithWorkFlowAssignments = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetFamilyWithWorkFlowAssignments
    Public Shared Function GetProgramMakeWithWorkFlowAssignments(ByVal Make As String, ByVal TeamMemberID As Integer, ByVal SubscriptionID As Integer) As DataSet
        ''Used in RFD;
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Program_Make_With_Workflow_Assignments"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If Make Is Nothing Then
                Make = ""
            End If

            myCommand.Parameters.Add("@Make", SqlDbType.VarChar)
            myCommand.Parameters("@Make").Value = Make

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@SubscriptionID", SqlDbType.Int)
            myCommand.Parameters("@SubscriptionID").Value = SubscriptionID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "ProgramMakeWithWorkFlowAssignments")
            GetProgramMakeWithWorkFlowAssignments = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "Make: " & Make _
            & ", TeamMemberID: " & TeamMemberID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetProgramMakeWithWorkFlowAssignments : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetProgramMakeWithWorkFlowAssignments : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetProgramMakeWithWorkFlowAssignments = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetProgramMakeWithWorkFlowAssignments
    Public Shared Function GetSubscriptions(ByVal Subscription As String) As DataSet
        ''Used in WorkFlow-Subscriptions; 
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Subscriptions_Maint"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Parameters.Add("@Subscription", SqlDbType.VarChar)
            myCommand.Parameters("@Subscription").Value = Subscription
            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Subscription")
            GetSubscriptions = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "Subscription: " & Subscription & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetSubscriptions : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetSubscriptions : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetSubscriptions = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetSubscriptions
    Public Shared Function GetWorkFlow(ByVal TeamMemberID As Integer, ByVal SubscriptionID As Integer) As DataSet
        ''Used in WorkFlow-Team_Member_Backup_List; 
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_WorkFlow"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID
            myCommand.Parameters.Add("@SubscriptionID", SqlDbType.Int)
            myCommand.Parameters("@SubscriptionID").Value = SubscriptionID
            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "WorkFlow")
            GetWorkFlow = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "TeamMemberID: " & TeamMemberID & "SubscriptionID: " & SubscriptionID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetWorkFlow : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetWorkFlow : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetWorkFlow = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetWorkFlow
#End Region

#Region "VENDOR INFO"
    Public Shared Function GetVendor(ByVal VendorID As Integer, ByVal VendorName As String, ByVal VendorAddress As String, _
    ByVal VendorState As String, ByVal VendorZipCode As String, ByVal VendorCountry As String, _
    ByVal VendorPhone As String, ByVal VendorFAX As String, ByVal VendorType As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Vendor"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandTimeout = 60

            myCommand.Parameters.Add("@vendorID", SqlDbType.Int)
            myCommand.Parameters("@vendorID").Value = VendorID

            myCommand.Parameters.Add("@vendorName", SqlDbType.VarChar)
            myCommand.Parameters("@vendorName").Value = VendorName

            myCommand.Parameters.Add("@vendorAddress", SqlDbType.VarChar)
            myCommand.Parameters("@vendorAddress").Value = VendorAddress

            myCommand.Parameters.Add("@vendorState", SqlDbType.VarChar)
            myCommand.Parameters("@vendorState").Value = VendorState

            myCommand.Parameters.Add("@vendorZipCode", SqlDbType.VarChar)
            myCommand.Parameters("@vendorZipCode").Value = VendorZipCode

            myCommand.Parameters.Add("@vendorCountry", SqlDbType.VarChar)
            myCommand.Parameters("@vendorCountry").Value = VendorCountry

            myCommand.Parameters.Add("@vendorPhone", SqlDbType.VarChar)
            myCommand.Parameters("@vendorPhone").Value = VendorPhone

            myCommand.Parameters.Add("@vendorFAX", SqlDbType.VarChar)
            myCommand.Parameters("@vendorFAX").Value = VendorFAX

            myCommand.Parameters.Add("@vendorType", SqlDbType.VarChar)
            myCommand.Parameters("@vendorType").Value = VendorType

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Vendor")
            GetVendor = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "VendorID:" & VendorID _
            & ", VendorName: " & VendorName & ", VendorAddress:" & VendorAddress _
            & ", VendorState: " & VendorState & ", VendorZipCode:" & VendorZipCode _
            & ", VendorCountry: " & VendorCountry & ", VendorPhone:" & VendorPhone _
            & ", VendorFAX: " & VendorFAX _
            & ", VendorType: " & VendorType _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetVendor : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> commonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetVendor : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "commonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetVendor = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetVendor
    Public Shared Function GetVendorAddress(ByVal VendorID As Integer, ByVal FutureVendor As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Vendor_Address"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandTimeout = 60

            myCommand.Parameters.Add("@vendorID", SqlDbType.Int)
            myCommand.Parameters("@vendorID").Value = VendorID

            myCommand.Parameters.Add("@FutureVendor", SqlDbType.Int)
            myCommand.Parameters("@FutureVendor").Value = FutureVendor

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "VendorAddr")
            GetVendorAddress = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "VendorID:" & VendorID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetVendorAddress : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> commonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetVendorAddress : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "commonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetVendorAddress = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetVendorAddress
    Public Shared Function GetUGNDBVendor(ByVal UGNDBVendorID As Integer, ByVal SupplierNo As String, _
        ByVal SupplierName As String, ByVal isActiveBPCSonly As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_UGNDB_Vendor"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandTimeout = 60

            myCommand.Parameters.Add("@ugndbVendorID", SqlDbType.Int)
            myCommand.Parameters("@ugndbVendorID").Value = UGNDBVendorID

            myCommand.Parameters.Add("@SupplierNo", SqlDbType.VarChar)
            myCommand.Parameters("@SupplierNo").Value = SupplierNo

            If SupplierName Is Nothing Then SupplierName = ""

            myCommand.Parameters.Add("@SupplierName", SqlDbType.VarChar)
            myCommand.Parameters("@SupplierName").Value = SupplierName

            myCommand.Parameters.Add("@isActiveBPCSonly", SqlDbType.Bit)
            myCommand.Parameters("@isActiveBPCSonly").Value = isActiveBPCSonly

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "UGNDBVendor")
            GetUGNDBVendor = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "UGNDBVendorID:" & UGNDBVendorID _
            & ", SupplierNo:" & SupplierNo _
            & ", SupplierName:" & SupplierName _
            & ", isActiveBPCSonly:" & isActiveBPCSonly _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetUGNDBVendor : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> commonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetUGNDBVendor : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "commonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetUGNDBVendor = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF UGNDBVendor
    Public Shared Sub InsertUGNDBVendor(ByVal VendorName As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_UGNDB_Vendor"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ugndbVendorName", SqlDbType.VarChar)
            myCommand.Parameters("@ugndbVendorName").Value = VendorName

            'myCommand.Parameters.Add("@bpcsVendorID", SqlDbType.Int)
            'myCommand.Parameters("@bpcsVendorID").Value = BPCSVendorID

            'myCommand.Parameters.Add("@obsolete", SqlDbType.Bit)
            'myCommand.Parameters("@obsolete").Value = 0

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "VendorName:" & VendorName _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertUGNDBVendor : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> commonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertUGNDBVendor : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "commonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub 'EOF InsertUGNDBVendor
    Public Shared Function GetVendorType(ByVal FilterVType As Boolean) As DataSet
        ''Used in PF;
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_VendorType"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@FilterVType", SqlDbType.Bit)
            myCommand.Parameters("@FilterVType").Value = FilterVType

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "VendorType")
            GetVendorType = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetVendorType : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetVendorType : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetVendorType = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetVendorType
#End Region

#Region "PART INFO"
    Public Shared Function GetDesignationType() As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Designation_Type"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DesignationType")
            GetDesignationType = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetDesignationType : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> commonFunctions.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDesignationType : " & commonFunctions.convertSpecialChar(ex.Message, False), "commonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetDesignationType = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetDesignationType
    Public Shared Function GetBillOfMaterials(ByVal PartNo As String, ByVal SubPartNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Bill_Of_Materials"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandTimeout = 300

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myCommand.Parameters.Add("@subPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@subPartNo").Value = SubPartNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "SubPartData")
            GetBillOfMaterials = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PartNo: " & PartNo & "SubPartNo: " & SubPartNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetBillOfMaterials : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> commonFunctions.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetBillOfMaterials : " & commonFunctions.convertSpecialChar(ex.Message, False), "commonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetBillOfMaterials = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetBillOfMaterials
    Public Shared Function GetAllFinishedGoods() As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_All_Finished_Goods"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandTimeout = 60

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetAllFinishedGoods")
            GetAllFinishedGoods = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetAllFinishedGoods : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetAllFinishedGoods : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            GetAllFinishedGoods = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Function 'EOF GetAllFinishedGoods
    Public Shared Function GetBPCSPartNo(ByVal PartNo As String, ByVal DesignationType As String) As DataSet
        ''Used in PF;Data Maintenance; DMS 
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_BPCS_PartNo"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandTimeout = 120

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myCommand.Parameters.Add("@DesignationType", SqlDbType.VarChar)
            myCommand.Parameters("@DesignationType").Value = DesignationType

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "BPCSPartNo")
            GetBPCSPartNo = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "partNo: " & PartNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetPartNo : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> commonFunctions.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetPartNo : " & commonFunctions.convertSpecialChar(ex.Message, False), "commonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetBPCSPartNo = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetPartNo
    Public Shared Function GetPartNo(ByVal PartNo As String, ByVal DesignationType As String, ByVal UGNFacility As String, ByVal OEM As String, ByVal OEMManufacturer As String) As DataSet
        ''Used in PF;Data Maintenance; DMS 
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_vPartNo"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandTimeout = 120

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myCommand.Parameters.Add("@DesignationType", SqlDbType.VarChar)
            myCommand.Parameters("@DesignationType").Value = DesignationType

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@OEM", SqlDbType.VarChar)
            myCommand.Parameters("@OEM").Value = OEM

            myCommand.Parameters.Add("@OEMManufacturer", SqlDbType.VarChar)
            myCommand.Parameters("@OEMManufacturer").Value = OEMManufacturer

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "PartNo")
            GetPartNo = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "partNo: " & PartNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetPartNo : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> commonFunctions.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetPartNo : " & commonFunctions.convertSpecialChar(ex.Message, False), "commonFunctions.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetPartNo = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetPartNo
#End Region

#Region "Send EMail "
    ''' <summary> 
    ''' Provides a method for sending email. 
    ''' </summary> 
    Public NotInheritable Class Email
        Private Sub New()
        End Sub
        ''' <summary> 
        ''' Constructs and sends an email message. 
        ''' </summary> 
        ''' <param name="fromName">The display name of the person the email is </param> 
        ''' <param name="fromEmail">The email address of the person the email is from.</param> 
        ''' <param name="subject">The subject of the email.</param> 
        ''' <param name="body">The body of the email.</param> 

        Public Shared Sub Send(ByVal fromName As String, ByVal fromEmail As String, ByVal subject As String, ByVal body As String, ByVal sendToEmail As String, ByVal CcEmail As String, ByVal FilePathName As String, ByVal ModuleName As String, ByVal RecID As String)

            Dim MyMessage As New MailMessage() With { _
                 .IsBodyHtml = True, _
              .From = New MailAddress(fromEmail, fromName), _
              .Subject = subject, _
              .Body = body _
              }

            If (FilePathName <> Nothing And FilePathName <> "") Then
                Dim attachFile As New Attachment(FilePathName)
                MyMessage.Attachments.Add(attachFile)
            End If

            Dim emailList1 As String() = CleanEmailList(sendToEmail).Split(";")
            For i = 0 To UBound(emailList1)
                If emailList1(i) <> ";" And emailList1(i).Trim <> "" Then
                    MyMessage.[To].Add(emailList1(i))
                End If
            Next i

            Dim emailList2 As String() = CleanEmailList(CcEmail).Split(";")
            For i = 0 To UBound(emailList2)
                If emailList2(i) <> ";" And emailList2(i).Trim <> "" Then
                    MyMessage.[CC].Add(emailList2(i))
                End If
            Next i

            Select Case ModuleName
                Case "Spending Request (A)"
                    MyMessage.[Bcc].Add("lynette.rey@ugnauto.com")
                Case "Spending Request (D)"
                    MyMessage.[Bcc].Add("lynette.rey@ugnauto.com")
                Case "Spending Request (P)"
                    MyMessage.[Bcc].Add("lynette.rey@ugnauto.com")
                Case "Spending Request (R)"
                    MyMessage.[Bcc].Add("lynette.rey@ugnauto.com")
                Case "Spending Request (T)"
                    MyMessage.[Bcc].Add("lynette.rey@ugnauto.com")
                Case "IOR"
                    MyMessage.[Bcc].Add("lynette.rey@ugnauto.com")
                Case "Supplier Request"
                    MyMessage.[Bcc].Add("lynette.rey@ugnauto.com")
                Case "Acoustic Testing"
                    MyMessage.[Bcc].Add("lynette.rey@ugnauto.com")
                Case "R&D Test Issuance"
                    MyMessage.[Bcc].Add("lynette.rey@ugnauto.com")
                Case "Test Issuance"
                    MyMessage.[Bcc].Add("lynette.rey@ugnauto.com")
                Case "AR_Deduction"
                    MyMessage.[Bcc].Add("lynette.rey@ugnauto.com")
                Case "Sample Material Request"
                    MyMessage.[Bcc].Add("lynette.rey@ugnauto.com")
            End Select

            Dim originalRecipientCount As Integer = MyMessage.[To].Count
            Dim failOnAnyAddress As Boolean = Convert.ToBoolean(WebConfigurationManager.AppSettings("failOnAnyAddress"))

            Try
                Send(MyMessage, ModuleName, RecID)
            Catch generatedExceptionName As SmtpException
                Dim ErrorString As String = Nothing
                ErrorString += "......... Failed to send email."

                Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
                UGNErrorTrapping.InsertErrorLog("commonFunctions - Email[Send] : " & commonFunctions.convertSpecialChar(generatedExceptionName.Message, False) & "; ModuleName: " & ModuleName & "; RecID: " & RecID & "; " & ErrorString, "commonFunctions.vb", strUserEditedData)


                If MyMessage.[To].Count = originalRecipientCount Then
                    ' all recipients failed 
                    Throw
                End If

                If failOnAnyAddress Then
                    ' some (not ALL) recipients failed 
                    Throw
                End If
            End Try
        End Sub

        Private Shared Sub Send(ByVal MyMessage As MailMessage, ByVal ModuleName As String, ByVal RecID As String)
            Dim client As New SmtpClient()

            Try
                client.Send(MyMessage)
            Catch ex As SmtpFailedRecipientsException
                Dim ErrorString As String = Nothing
                ErrorString += "......... Failed to multiple send email."

                Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
                UGNErrorTrapping.InsertErrorLog("commonFunctions - Email[Send] SmtpFailedRecipientsException : " & commonFunctions.convertSpecialChar(ex.Message, False) & "; ModuleName: " & ModuleName & "; RecID: " & RecID & "; " & ErrorString, "commonFunctions.vb", strUserEditedData)

                ' multiple fail 
                MyMessage.[To].Clear()
                MyMessage.[CC].Clear()
                MyMessage.[Bcc].Clear()

                For Each sfrEx As SmtpFailedRecipientException In ex.InnerExceptions
                    CheckStatusAndReaddress(MyMessage, sfrEx)
                Next

                If MyMessage.[To].Count > 0 Then
                    ' wait 5 seconds, try a second time 
                    Thread.Sleep(5000)
                    client.Send(MyMessage)
                Else
                    Throw
                End If
            Catch ex As SmtpFailedRecipientException
                Dim ErrorString As String = Nothing
                ErrorString += "......... Failed to single send email."

                Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
                UGNErrorTrapping.InsertErrorLog("commonFunctions - Email[Send] SmtpFailedRecipientException : " & commonFunctions.convertSpecialChar(ex.Message, False) & "; ModuleName: " & ModuleName & "; RecID: " & RecID & "; " & ErrorString, "commonFunctions.vb", strUserEditedData)

                ' single fail 
                MyMessage.[To].Clear()
                MyMessage.[CC].Clear()
                MyMessage.[Bcc].Clear()

                CheckStatusAndReaddress(MyMessage, ex)

                If MyMessage.[To].Count > 0 Then
                    ' wait 5 seconds, try a second time 
                    Thread.Sleep(5000)
                    client.Send(MyMessage)
                Else
                    Throw
                End If
            Finally
                MyMessage.Dispose()
            End Try
        End Sub

        Private Shared Sub CheckStatusAndReaddress(ByVal MyMessage As MailMessage, ByVal exception As SmtpFailedRecipientException)
            Dim statusCode As SmtpStatusCode = exception.StatusCode

            If statusCode = SmtpStatusCode.MailboxBusy OrElse statusCode = SmtpStatusCode.MailboxUnavailable OrElse statusCode = SmtpStatusCode.TransactionFailed Then
                MyMessage.[To].Add(exception.FailedRecipient)
            End If
        End Sub
    End Class

    Public Shared Function CleanEmailList(ByVal EmailAddressList As String) As String

        'remove duplicates 
        'replace ugnusa with ugnauto

        Dim strReturnList As String = ""

        Try
            Dim strTempList As String = ""

            Dim strEmailToAddress As String = Replace(EmailAddressList, ";;", ";")

            If strEmailToAddress <> "" Then
                'build email To list
                Dim emailList As String() = strEmailToAddress.Split(";")

                For i = 0 To UBound(emailList)
                    If emailList(i) <> ";" And emailList(i).Trim <> "" Then
                        If InStr(strTempList, emailList(i), CompareMethod.Text) <= 0 Then

                            If strTempList <> "" Then
                                strTempList &= ";"
                            End If

                            strTempList &= emailList(i)
                        End If
                    End If
                Next i

                strReturnList = Replace(strTempList, "ugnusa", "ugnauto")
            End If

        Catch ex As Exception

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.DeclaringType.Name & "." & mb.Name & "():" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)

        End Try

        Return strReturnList

    End Function

#End Region 'EOF Send EMail

#Region "UNUSED"
    'Public Shared Function GetAllRawMaterials() As DataSet

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Get_All_Raw_Materials"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '    Dim GetData As New DataSet
    '    Dim myAdapter As New SqlDataAdapter
    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure
    '        myCommand.CommandTimeout = 60

    '        myAdapter = New SqlDataAdapter(myCommand)
    '        myAdapter.Fill(GetData, "GetAllRawMaterials")
    '        GetAllRawMaterials = GetData
    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "GetAllRawMaterials : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
    '        " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("GetAllRawMaterials : " & _
    '        commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
    '        GetAllRawMaterials = Nothing
    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try
    'End Function 'EOF GetAllRawMaterials

    'Public Shared Function getOldUGNDatabaseRoles() As DataSet

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("FormerUGNDB").ToString
    '    Dim strStoredProcName As String = "sp_Get_UserRoles"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '    Dim getData As New DataSet

    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure
    '        myCommand.Parameters.Add("@userEmail", SqlDbType.VarChar)
    '        myCommand.Parameters("@userEmail").Value = HttpContext.Current.Session("userEmail")

    '        Dim myAdapter As SqlDataAdapter = New SqlDataAdapter(myCommand)
    '        myAdapter.Fill(getData, "OldUserIdData")
    '        getOldUGNDatabaseRoles = getData

    '    Catch ex As Exception
    '        getOldUGNDatabaseRoles = Nothing
    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try

    'End Function
    'Public Shared Function getOldUGNDatabaseUserInfo() As DataSet

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("FormerUGNDB").ToString
    '    Dim strStoredProcName As String = "sp_Get_UserId"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '    Dim getData As New DataSet

    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure
    '        myCommand.Parameters.Add("@userEmail", SqlDbType.VarChar)
    '        myCommand.Parameters("@userEmail").Value = HttpContext.Current.Session("userEmail")

    '        Dim myAdapter As SqlDataAdapter = New SqlDataAdapter(myCommand)
    '        myAdapter.Fill(getData, "OldUserIdData")
    '        getOldUGNDatabaseUserInfo = getData

    '    Catch ex As Exception
    '        getOldUGNDatabaseUserInfo = Nothing
    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try
    '
    'End Function 'EOF getOldUGNDatabaseRoles
    'Public Shared Function connectToOldUGNDatabase() As Boolean

    '    Dim bFound As Boolean = False

    '    Try
    '        Dim FullName As String = commonFunctions.getUserName()

    '        If FullName IsNot Nothing Then

    '            Dim LocationOfDot As Integer = InStr(FullName, ".")
    '            Dim FirstName As String = Left(FullName, LocationOfDot - 1)
    '            Dim FirstInitial As String = Left(FullName, 1)
    '            Dim LastName As String = Right(FullName, Len(FullName) - LocationOfDot)

    '            HttpContext.Current.Response.Cookies("UGNDB_User").Value = FirstInitial & LastName

    '            HttpContext.Current.Session("userEmail") = FullName & "@ugnusa.com"

    '            Dim strUserId As String = ""

    '            Dim dsOldUserInfo As DataSet = New DataSet

    '            dsOldUserInfo = getOldUGNDatabaseUserInfo()
    '            If dsOldUserInfo IsNot Nothing Then
    '                If dsOldUserInfo.Tables.Count > 0 And dsOldUserInfo.Tables(0).Rows.Count > 0 Then
    '                    HttpContext.Current.Response.Cookies("UserId").Domain = "tweb2.ugnnet.com"
    '                    HttpContext.Current.Response.Cookies("UserId").Value = dsOldUserInfo.Tables(0).Rows(0).Item("UserId")

    '                    HttpContext.Current.Response.Cookies("MM_UserEmail").Domain = "tweb2.ugnnet.com"
    '                    HttpContext.Current.Response.Cookies("MM_UserEmail").Value = FullName & "@ugnusa.com"

    '                    HttpContext.Current.Response.Cookies("MM_UserID").Domain = "tweb2.ugnnet.com"
    '                    HttpContext.Current.Response.Cookies("MM_UserID").Value = dsOldUserInfo.Tables(0).Rows(0).Item("EmpId")

    '                    HttpContext.Current.Response.Cookies("MM_UGNfacility").Domain = "tweb2.ugnnet.com"
    '                    HttpContext.Current.Response.Cookies("MM_UGNfacility").Value = dsOldUserInfo.Tables(0).Rows(0).Item("UgnFacility")
    '                    HttpContext.Current.Session("MM_UGNfacility") = dsOldUserInfo.Tables(0).Rows(0).Item("UgnFacility")

    '                    HttpContext.Current.Response.Cookies("MM_EmpTitle").Domain = "tweb2.ugnnet.com"
    '                    HttpContext.Current.Response.Cookies("MM_EmpTitle").Value = dsOldUserInfo.Tables(0).Rows(0).Item("EmpTitle")

    '                    HttpContext.Current.Response.Cookies("MM_UGNDeptNo").Domain = "tweb2.ugnnet.com"
    '                    HttpContext.Current.Response.Cookies("MM_UGNDeptNo").Value = dsOldUserInfo.Tables(0).Rows(0).Item("UGNDeptNo")

    '                    HttpContext.Current.Response.Cookies("MM_Username").Domain = "tweb2.ugnnet.com"
    '                    HttpContext.Current.Response.Cookies("MM_Username").Value = FirstInitial & LastName

    '                    HttpContext.Current.Response.Cookies("MM_EmpName").Domain = "tweb2.ugnnet.com"
    '                    HttpContext.Current.Response.Cookies("MM_EmpName").Value = dsOldUserInfo.Tables(0).Rows(0).Item("EmpName")
    '                    bFound = True
    '                End If

    '            End If
    '        End If

    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "User:" & HttpContext.Current.Request.Cookies("UGNDB_User").Value
    '        HttpContext.Current.Session("BLLerror") = "connectToOldUGNDatabase : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> commonfunctions.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("connectToOldUGNDatabase : " & commonFunctions.convertSpecialChar(ex.Message, False), "commonfunctions.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
    '        Return False
    '    End Try

    '    Return bFound

    'End Function 'EOF connectToOldUGNDatabase 'EOF GetOldDatabaseUserInfo
    'Public Shared Function GetPartMakeRelate(ByVal part As String) As DataSet

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Get_Part_Make_Relate"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '    Dim GetData As New DataSet
    '    Dim myAdapter As New SqlDataAdapter
    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure
    '        myAdapter = New SqlDataAdapter(myCommand)
    '        myAdapter.Fill(GetData, "Part")
    '        GetPartMakeRelate = GetData
    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "GetPartMakeRelate : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
    '        " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("GetPartMakeRelate : " & _
    '        commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
    '        GetPartMakeRelate = Nothing
    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try
    'End Function

    'Public Shared Function GetPartModelRelate(ByVal part As String) As DataSet

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Get_Part_Model_Relate"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '    Dim GetData As New DataSet
    '    Dim myAdapter As New SqlDataAdapter
    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure
    '        myAdapter = New SqlDataAdapter(myCommand)
    '        myAdapter.Fill(GetData, "Part")
    '        GetPartModelRelate = GetData
    '    Catch ex As Exception
    '        Dim rslt As String = ex.Message
    '        GetPartModelRelate = Nothing
    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try
    'End Function

    'Public Shared Function GetPartProgramRelate(ByVal part As String) As DataSet

    '    Dim myConnection As SqlConnection = New SqlConnection
    '    Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
    '    Dim strStoredProcName As String = "sp_Get_Part_Program_Relate"
    '    Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
    '    Dim GetData As New DataSet
    '    Dim myAdapter As New SqlDataAdapter
    '    Try
    '        myConnection.ConnectionString = strConnectionString
    '        myCommand.CommandType = CommandType.StoredProcedure
    '        myAdapter = New SqlDataAdapter(myCommand)
    '        myAdapter.Fill(GetData, "Part")
    '        GetPartProgramRelate = GetData
    '    Catch ex As Exception
    '        Dim rslt As String = ex.Message
    '        GetPartProgramRelate = Nothing
    '    Finally
    '        myConnection.Close()
    '        myCommand.Dispose()
    '    End Try
    'End Function 'EOF
    'Public Shared Function SubstNoReg(ByVal initialStr, ByVal oldStr, ByVal newStr) As String

    '    Try
    '        Dim currentPos, oldStrPos, skip
    '        If initialStr.IsNull Or Len(initialStr) = 0 Then
    '            SubstNoReg = ""
    '        ElseIf oldStr.IsNull Or Len(oldStr) = 0 Then
    '            SubstNoReg = initialStr
    '        Else
    '            If newStr.IsNull Then newStr = ""
    '            currentPos = 1
    '            oldStrPos = 0
    '            SubstNoReg = ""
    '            skip = Len(oldStr)
    '            Do While currentPos <= Len(initialStr)
    '                oldStrPos = InStr(currentPos, initialStr, oldStr)
    '                If oldStrPos = 0 Then
    '                    SubstNoReg = SubstNoReg & Mid(initialStr, currentPos, Len(initialStr) - currentPos + 1)
    '                    currentPos = Len(initialStr) + 1
    '                Else
    '                    SubstNoReg = SubstNoReg & Mid(initialStr, currentPos, oldStrPos - currentPos) & newStr
    '                    currentPos = oldStrPos + skip
    '                End If
    '            Loop
    '        End If
    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "initialStr: " & initialStr & ", oldStr: " & oldStr & ", newStr: " & newStr & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "SubstNoReg : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
    '        " :<br/> CommonFunctions.vb :<br/> " & strUserEditedData

    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
    '        UGNErrorTrapping.InsertErrorLog("SubstNoReg : " & _
    '        commonFunctions.convertSpecialChar(ex.Message, False), "CommonFunctions.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
    '        SubstNoReg = ""
    '    End Try

    'End Function 'EOF
#End Region

End Class
