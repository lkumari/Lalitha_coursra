Option Explicit On
Option Strict On

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.Data
Imports System.Diagnostics
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Net
Imports System.Reflection

''' ==============================================================
'''  File:       SecurityModule.vb
''' 
'''  Purpose:    Supplies database access for the tables and
'''              programs in the Security Module 
'''              This file replaces the SecurityFunctions.vb file.
'''         
'''  Language:   VB.NET 2005
''' 
'''  Written by: M. Weyker 5/7/2008
''' 
'''  --- Modification History ---
'''  6/10/2008  M.Weyker  Added new function: GetAbbrevNameFromCurrentUser()
'''     Use this function to supply the "UpdatedBy" and "CreatedBy" parameters
'''     of stored procedures that INSERT or UPDATE table records. The "UpdatedBy"
'''     and "CreatedBy" parameters were formerly populated by the network account
'''     name of the current user.
''' 
'''  6/10/2008  M.Weyker  Added new function: GetSubscriptions()
''' 
'''  6/10/2008  M.Weyker  Modify methods to match the new design
'''                       of the TeamMember_WorkHistory table.
'''                       The Title column was eliminated, and replaced
'''                       by a SubscriptionID column. The new primary
'''                       key is (TeamMemberID, SubscriptionID).
'''    --- Methods changed ---
'''    GetTMWorkHistory: Replace the StartDate parameter with a
'''        SubscriptionID parameter.
'''    GetTMWorkHistoryCount: Replace the StartDate parameter with a
'''        SubscriptionID parameter.
'''    DeleteTMWorkHistory: Replace the StartDate parameter with a
'''        SubscriptionID parameter.
'''    UpdateTMWorkHistory: Replace the StartDate parameter with a
'''        SubscriptionID parameter.
'''    InsertTMWorkHistory: Remove the StartDate and Title parameters.
'''        Add a SubscriptionID parameter.
''' 
'''  6/13/2008  M.Weyker  Added method "GetPageUrls()" to supply
'''                       page URL's to the Forms Maintenance program.
''' 
'''  6/16/2008  M.Weyker  Modify these methods to change Forms_Maint
'''                       table column "Description" to "HyperlinkID":
'''                       [GetForm(), GetFormCount(), InsertForm(), UpdateForm()]
''' 
'''  6/16/2008  M.Weyker  Add the AddDDColToFormsDataSet() function 
'''                       to supply an extra column of valid 
'''                       "HyperlinkID" data to be presented in the 
'''                       Forms GridView HyperlinkID DropDownList.
''' 
'''  7/15/2008  L.Rey     Added Menu_Maint Functions and added MenuID parameters 
'''                       to the InsertForm and UpdateForm functions.
''' 
'''  08/26/2008 M.Weyker  Added standard exception reporting,
'''                       using UGNErrorTrapping class.	
''' 
'''  10/3/2008  M.Weyker  Modified method AddPageUrlsToList to include
'''                       files with pdf extension. Now it collects a list
'''                       of files with .aspx and .pdf extensions.
''' ==============================================================
Public Class SecurityModule
    Inherits System.ComponentModel.Component


#Region "Module Level Variables"
    Public Enum Menu_Maint_SortBy As Integer
        MenuID
        MenuName
        Description
    End Enum

    Public Enum Forms_Maint_SortBy As Integer
        FormID
        FormName
        Description
    End Enum

    Public Enum Roles_Maint_SortBy As Integer
        RoleID
        RoleName
        Description
    End Enum

    Public Enum TeamMember_Maint_Sortby As Integer
        TeamMemberID
        UserName
        ShortName
        LastName
        FirstName
        Email
        Working
    End Enum

    Public Enum TeamMember_Maint_WorkStatus As Integer
        Working
        NotWorking
        Both
    End Enum

    Private Const PARAMETER_LIST_HEADING As String = _
        "Executing method parameter values ..." & ControlChars.CrLf
#End Region ' Module Level Variables


#Region "Facility_Maint Functions"

    ''' <summary>
    ''' Gets a DataSet of all UGNFacility and UGNFacilityName from Facilities_Maint table
    ''' </summary>
    ''' <returns>Facilities Dataset</returns>
    ''' <remarks>Only gets data not marked as obsolete</remarks>
    Public Shared Function GetFacilities() As DataSet
        Dim cn As SqlConnection = New SqlConnection
        Dim cmd As SqlCommand = New SqlCommand
        Dim ds As New DataSet

        Try
            cn.ConnectionString = _
                System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString()
            cn.Open()

            cmd.Connection = cn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_Get_UGNFacility"

            Dim myAdapter As New SqlDataAdapter(cmd)
            myAdapter.Fill(ds)

        Catch ex As Exception
            Debug.WriteLine("Error encountered in SecurityFunctions.GetFacilities" & _
                ControlChars.Cr & ex.Message)
            ds = Nothing
            Dim strMethodName As String = GetExecutingMethodName()
            SendExceptionEmailNotification("SecurityModule", strMethodName, ex, Nothing)

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        Finally
            cn.Close()
            cmd.Dispose()
        End Try
        Return ds
    End Function

#End Region ' Facility_Maint Functions


#Region "Menu_Maint Functions"
    Public Shared Function GetMenu(ByVal MenuID As Nullable(Of Integer), _
        ByVal MenuName As String, _
        ByVal Obsolete As Nullable(Of Boolean), _
        ByVal SortBy As Nullable(Of Menu_Maint_SortBy)) As DataSet

        Dim cn As SqlConnection = New SqlConnection
        Dim cmd As SqlCommand = New SqlCommand
        Dim ds As New DataSet
        Dim intRowsAffected As Integer
        Dim intReturnValue As Integer
        Dim blnSuccess As Boolean = False
        Dim strMethodParms As String = PARAMETER_LIST_HEADING

        Try
            ' Create the command to execute a stored procedure
            cn.ConnectionString = _
                System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString()
            cmd.Connection = cn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_Get_Menu"

            ' Add the input parameters and values
            If MenuID.HasValue Then
                cmd.Parameters.Add(New SqlParameter("@parm_MenuID", SqlDbType.Int))
                cmd.Parameters("@parm_MenuID").Value = MenuID
                strMethodParms &= "MenuID: " & MenuID.ToString & Environment.NewLine
            Else
                strMethodParms &= "MenuID: Nothing" & Environment.NewLine
            End If

            If Not String.IsNullOrEmpty(MenuName) Then
                cmd.Parameters.Add(New SqlParameter("@parm_MenuName", SqlDbType.VarChar, 30))
                cmd.Parameters("@parm_MenuName").Value = MenuName
                strMethodParms &= "MenuName: " & MenuName & Environment.NewLine
            Else
                strMethodParms &= "MenuName: NullOrEmpty" & Environment.NewLine
            End If

            If Obsolete.HasValue Then
                cmd.Parameters.Add(New SqlParameter("@parm_Obsolete", SqlDbType.Bit))
                cmd.Parameters("@parm_Obsolete").Value = Obsolete
                strMethodParms &= "Obsolete: " & Obsolete.ToString & Environment.NewLine
            Else
                strMethodParms &= "Obsolete: Nothing" & Environment.NewLine
            End If

            If SortBy.HasValue Then
                cmd.Parameters.Add(New SqlParameter("@parm_SortBy", SqlDbType.VarChar, 30))
                cmd.Parameters("@parm_SortBy").Value = SortBy.ToString()
                strMethodParms &= "SortBy: " & SortBy.ToString() & Environment.NewLine
            Else
                strMethodParms &= "SortBy: Nothing" & Environment.NewLine
            End If

            ' Add the output parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_RowsAffected", SqlDbType.Int))
            cmd.Parameters("@parm_RowsAffected").Direction = ParameterDirection.Output

            ' Add the return parameter
            cmd.Parameters.Add(New SqlParameter("ReturnValue", SqlDbType.Int))
            cmd.Parameters("ReturnValue").Direction = ParameterDirection.ReturnValue

            ' Execute the stored procedure,
            ' and return the result.
            cn.Open()
            Dim myAdapter As New SqlDataAdapter(cmd)
            myAdapter.Fill(ds)

            intRowsAffected = Convert.ToInt32(cmd.Parameters("@parm_RowsAffected").Value)
            intReturnValue = Convert.ToInt32(cmd.Parameters("ReturnValue").Value)

            If (intReturnValue <> 0) Then
                Debug.WriteLine("Error encountered SecurityModule.GetForm" & _
                       ControlChars.Cr & _
                       "Error Returned: " & intReturnValue & _
                       ", Rows Affected: " & intRowsAffected)
                ds = Nothing
            Else
                blnSuccess = True
            End If
        Catch ex As Exception
            Debug.WriteLine("Error encountered in SecurityModule.GetForm" & _
                ControlChars.Cr & ex.Message)
            Dim strMethodName As String = GetExecutingMethodName()
            SendExceptionEmailNotification("SecurityModule", strMethodName, ex, strMethodParms)
            ds = Nothing

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        Finally
            cn.Close()
            cmd.Dispose()
        End Try

        Return ds
    End Function ' GetMenu
    ''' <summary>
    ''' Gets a count of the specified Menu_Maint records
    ''' </summary>
    ''' <param name="MenuID">MenuID of records to count</param>
    ''' <param name="MenuName">MenuName of records to count</param>
    ''' <param name="Obsolete">Obsolete value of records to count</param>
    ''' <returns>Record count as Integer</returns>
    ''' <remarks></remarks>
    Public Shared Function GetMenuCount(ByVal MenuID As Nullable(Of Integer), _
        ByVal MenuName As String, _
        ByVal Obsolete As Nullable(Of Boolean)) As Integer

        Dim cn As SqlConnection = New SqlConnection
        Dim cmd As SqlCommand = New SqlCommand
        Dim ds As New DataSet
        Dim intRowsAffected As Integer = -1
        Dim intReturnValue As Integer
        Dim blnSuccess As Boolean = False
        Dim strMethodParms As String = PARAMETER_LIST_HEADING

        Try
            ' Create the command to execute a stored procedure
            cn.ConnectionString = _
                System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString()
            cmd.Connection = cn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_Get_Menu"

            ' Add the input parameters and values
            If MenuID.HasValue Then
                cmd.Parameters.Add(New SqlParameter("@parm_MenuID", SqlDbType.Int))
                cmd.Parameters("@parm_MenuID").Value = MenuID
                strMethodParms &= "MenuID: " & MenuID.ToString & Environment.NewLine
            Else
                strMethodParms &= "MenuID: Nothing" & Environment.NewLine
            End If

            If Not String.IsNullOrEmpty(MenuName) Then
                cmd.Parameters.Add(New SqlParameter("@parm_MenuName", SqlDbType.VarChar, 30))
                cmd.Parameters("@parm_MenuName").Value = MenuName
                strMethodParms &= "MenuName: " & MenuName & Environment.NewLine
            Else
                strMethodParms &= "MenuName: NullOrEmpty" & Environment.NewLine
            End If



            If Obsolete.HasValue Then
                cmd.Parameters.Add(New SqlParameter("@parm_Obsolete", SqlDbType.Bit))
                cmd.Parameters("@parm_Obsolete").Value = Obsolete
                strMethodParms &= "Obsolete: " & Obsolete.ToString & Environment.NewLine
            Else
                strMethodParms &= "Obsolete: Nothing" & Environment.NewLine
            End If

            ' Add the output parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_RowsAffected", SqlDbType.Int))
            cmd.Parameters("@parm_RowsAffected").Direction = ParameterDirection.Output

            ' Add the return parameter
            cmd.Parameters.Add(New SqlParameter("ReturnValue", SqlDbType.Int))
            cmd.Parameters("ReturnValue").Direction = ParameterDirection.ReturnValue

            ' Execute the stored procedure,
            ' and return the result.
            cn.Open()
            Dim myAdapter As New SqlDataAdapter(cmd)
            myAdapter.Fill(ds)

            intRowsAffected = Convert.ToInt32(cmd.Parameters("@parm_RowsAffected").Value)
            intReturnValue = Convert.ToInt32(cmd.Parameters("ReturnValue").Value)

            If (intReturnValue <> 0) Then
                Debug.WriteLine("Error encountered SecurityModule.GetMenuCount" & _
                       ControlChars.Cr & _
                       "Error Returned: " & intReturnValue & _
                       ", Rows Affected: " & intRowsAffected)
                intRowsAffected = -1
            End If
        Catch ex As Exception
            Debug.WriteLine("Error encountered in SecurityModule.GetMenuCount" & _
                ControlChars.Cr & ex.Message)
            Dim strMethodName As String = GetExecutingMethodName()
            SendExceptionEmailNotification("SecurityModule", strMethodName, ex, strMethodParms)
            intRowsAffected = -1

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        Finally
            cn.Close()
            cmd.Dispose()
        End Try

        Return intRowsAffected
    End Function ' GetMenuCount

    ''' <summary>
    ''' Insert a new record in the Menu_Maint table
    ''' </summary>
    ''' <param name="MenuName">MenuName to insert</param>
    ''' <param name="Obsolete">Obsolete value to insert</param>
    ''' <returns>New MenuId as Int32</returns>
    ''' <remarks>If success, return the new MenuId. 
    ''' If failure, return -1.</remarks>
    Public Shared Function InsertMenu(ByVal MenuName As String, _
        ByVal Obsolete As Boolean) As Integer

        Dim cn As SqlConnection = New SqlConnection
        Dim cmd As SqlCommand = New SqlCommand
        Dim intIdent As Integer
        Dim intReturnValue As Integer = -1
        Dim strMethodParms As String = PARAMETER_LIST_HEADING

        Try
            ' Create the SQL command to execute a stored procedure
            cn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString()
            cmd.Connection = cn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_Insert_Menu"

            ' Add the input parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_MenuName", SqlDbType.VarChar, 30))
            cmd.Parameters("@parm_MenuName").Value = MenuName
            strMethodParms &= "MenuName: " & MenuName & Environment.NewLine


            cmd.Parameters.Add(New SqlParameter("@parm_Obsolete", SqlDbType.Bit))
            cmd.Parameters("@parm_Obsolete").Value = Convert.ToByte(Obsolete)
            strMethodParms &= "Obsolete: " & Obsolete.ToString & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_CreatedBy", SqlDbType.VarChar, 50))
            cmd.Parameters("@parm_CreatedBy").Value = GetAbbrevNameFromCurrentUser()

            ' Add the output parameters and values
            'cmd.Parameters.Add(New SqlParameter("@parm_Identity", SqlDbType.Int))
            cmd.Parameters.Add(New SqlParameter("@parm_Ident", SqlDbType.Int))
            cmd.Parameters("@parm_Ident").Direction = ParameterDirection.Output

            ' Add the return parameter
            cmd.Parameters.Add(New SqlParameter("ReturnValue", SqlDbType.Int))
            cmd.Parameters("ReturnValue").Direction = ParameterDirection.ReturnValue

            ' Execute the stored procedure,
            ' and return the result.
            cn.Open()
            'cmd.ExecuteNonQuery()
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            dr.Close()
            intIdent = Convert.ToInt32(cmd.Parameters("@parm_Ident").Value)
            intReturnValue = Convert.ToInt32(cmd.Parameters("ReturnValue").Value)

            If intReturnValue <> 0 Then
                Debug.WriteLine("Error encountered SecurityModule.InsertMenu" & _
                       ControlChars.Cr & _
                       "Error Returned: " & intReturnValue)
                intIdent = -1
            End If
        Catch ex As Exception
            Debug.WriteLine("Error encountered in SecurityModule.InsertMenu" & _
                ControlChars.Cr & ex.Message)
            intIdent = -1
            Dim strMethodName As String = GetExecutingMethodName()
            SendExceptionEmailNotification("SecurityModule", strMethodName, ex, strMethodParms)

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        Finally
            cn.Close()
            cmd.Dispose()
        End Try
        Return intIdent
    End Function ' InsertMenu

    ''' <summary>
    ''' Updates the specified record in the Menu_Maint table
    ''' </summary>
    ''' <param name="MenuId">The record key of the record to update</param>
    ''' <param name="MenuName">New MenuName value</param>
    ''' <param name="Obsolete">New Obsolete value</param>
    ''' <returns>True if success, otherwise False</returns>
    ''' <remarks>
    ''' </remarks>
    Public Shared Function UpdateMenu(ByVal MenuId As Integer, _
            ByVal MenuName As String, _
            ByVal Obsolete As Boolean) As Boolean

        Dim cn As SqlConnection = New SqlConnection
        Dim cmd As SqlCommand = New SqlCommand
        Dim intRowsAffected As Integer
        Dim intReturnValue As Integer
        Dim blnSuccess As Boolean = False
        Dim strMethodParms As String = PARAMETER_LIST_HEADING

        Try
            ' Create the command to execute a stored procedure
            cn.ConnectionString = _
                System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString()
            cmd.Connection = cn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_Update_Menu"

            ' Add the input parameters and values

            cmd.Parameters.Add(New SqlParameter("@parm_MenuID", SqlDbType.Int))
            cmd.Parameters("@parm_MenuID").Value = MenuId
            strMethodParms &= "MenuID: " & MenuId.ToString & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_MenuName", SqlDbType.VarChar, 30))
            cmd.Parameters("@parm_MenuName").Value = MenuName
            strMethodParms &= "MenuName: " & MenuName & Environment.NewLine


            cmd.Parameters.Add(New SqlParameter("@parm_Obsolete", SqlDbType.Bit))
            cmd.Parameters("@parm_Obsolete").Value = Obsolete
            strMethodParms &= "Obsolete: " & Obsolete.ToString & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_UpdatedBy", SqlDbType.VarChar, 50))
            cmd.Parameters("@parm_UpdatedBy").Value = GetAbbrevNameFromCurrentUser()

            ' Add the output parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_RowsAffected", SqlDbType.Int))
            cmd.Parameters("@parm_RowsAffected").Direction = ParameterDirection.Output

            ' Add the return parameter
            cmd.Parameters.Add(New SqlParameter("ReturnValue", SqlDbType.Int))
            cmd.Parameters("ReturnValue").Direction = ParameterDirection.ReturnValue

            ' Execute the stored procedure,
            ' and return the result.
            cn.Open()
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            dr.Close()
            intRowsAffected = Convert.ToInt32(cmd.Parameters("@parm_RowsAffected").Value)
            intReturnValue = Convert.ToInt32(cmd.Parameters("ReturnValue").Value)

            If (intReturnValue <> 0) Or _
               (intRowsAffected <> 1) Then
                Debug.WriteLine("Error encountered SecurityModule.UpdateMenu" & _
                       ControlChars.Cr & _
                       "Error Returned: " & intReturnValue & _
                       ", Rows Affected: " & intRowsAffected)
            Else
                blnSuccess = True
            End If
        Catch ex As Exception
            Debug.WriteLine("Error encountered in SecurityModule.UpdateMenu" & _
                ControlChars.Cr & ex.Message)
            Dim strMethodName As String = GetExecutingMethodName()
            SendExceptionEmailNotification("SecurityModule", strMethodName, ex, strMethodParms)

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        Finally
            cn.Close()
            cmd.Dispose()
        End Try
        Return blnSuccess
    End Function ' UpdateMenu

#End Region 'Menu_Maint Functions


#Region "Forms_Maint Functions"

    ''' <summary>
    ''' Add a column to the Forms DataSet
    ''' </summary>
    ''' <param name="OriginalDataSet"></param>
    ''' <returns>Returns the original DataSet with an additional column.</returns>
    ''' <remarks>Adds the "ddHyperlinkID" column. Contains "HyperlinkID" column value, if
    ''' valid; Otherwise contains "". This will ensure that the  "ddHyperlinkID" column 
    ''' will always contain a valid that is found in the HyperlinkID DropDownList.</remarks>
    Private Shared Function AddDDColToFormsDataSet(ByVal OriginalDataSet As DataSet) As DataSet
        Dim ds As DataSet = OriginalDataSet
        Try
            ' Add a new column "ddHyperlinkID" to the first table of
            ' the original DataSet.

            Dim dt As DataTable = ds.Tables(0)
            dt.Columns.Add("ddHyperlinkID", GetType(String))

            ' Get a sorted copy of the HyperlinkID DropDownList items.
            Dim pageList As ArrayList = GetPageUrls()
            pageList.Sort()

            ' Populate the new "ddHyperlinkID" column in each row
            ' of the DataTable.
            For Each dr As DataRow In dt.Rows
                Dim strHyperlinkID As String = dr("HyperlinkID").ToString
                Dim intIndex As Integer = pageList.BinarySearch(strHyperlinkID)
                If intIndex < 0 Then
                    ' The "HyperlinkID" column contains invalid data.
                    ' Place a null string value in the "ddHyperlinkID" column.
                    dr("ddHyperlinkID") = ""
                Else
                    ' The "HyperlinkID" column contains valid data.
                    ' Copy it's value to the "ddHyperlinkID" column.
                    dr("ddHyperlinkID") = strHyperlinkID
                End If
            Next
        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
        Return ds
    End Function

    ''' <summary>
    ''' Adds the url of each page and pdf to a list.
    ''' </summary>
    ''' <param name="ParentDirectory">The specified directory.</param>
    ''' <param name="ParentDirectoryPath">The virtual path of the specified directory.</param>
    ''' <param name="PageList">The list to populate</param>
    ''' <param name="StartAtLevel">The starting directory level.</param>
    ''' <param name="Level">Current level. Topmost level = 0. Next lower level = 1.</param>
    ''' <remarks>
    ''' This method uses recursion to browse 
    ''' each subdirectory, starting from the parent directory.<br />
    ''' To include page urls at the root level, use StartAtLevel=0.<br/>
    ''' To exclude page urls at the root level, use StartAtLevel=1.<br/>
    ''' </remarks>
    Private Shared Sub AddPageUrlsToList(ByVal ParentDirectory As DirectoryInfo, _
        ByVal ParentDirectoryPath As String, _
        ByRef PageList As ArrayList, _
        ByVal StartAtLevel As Integer, _
        ByVal Level As Integer)
        Try
            Dim files1() As FileInfo = ParentDirectory.GetFiles("*.aspx", SearchOption.TopDirectoryOnly)
            Dim files2() As FileInfo = ParentDirectory.GetFiles("*.pdf", SearchOption.TopDirectoryOnly)
            Dim files3() As FileInfo = ParentDirectory.GetFiles("*.exe", SearchOption.TopDirectoryOnly)
            If Level >= StartAtLevel Then
                For Each fi As FileInfo In files1
                    PageList.Add(ParentDirectoryPath & fi.Name)
                Next
                For Each fi As FileInfo In files2
                    PageList.Add(ParentDirectoryPath & fi.Name)
                Next
                For Each fi As FileInfo In files3
                    PageList.Add(ParentDirectoryPath & fi.Name)
                Next
            End If
            Dim directories() As DirectoryInfo = ParentDirectory.GetDirectories()
            For Each di As DirectoryInfo In directories
                AddPageUrlsToList(di, ParentDirectoryPath & di.Name & "/", PageList, StartAtLevel, Level + 1)
            Next
        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
    End Sub

    ''' <summary>
    ''' Gets a DataSet of Forms_Maint records
    ''' </summary>
    ''' <param name="FormID">FormID of records to return</param>
    ''' <param name="FormName">FormName of records to return</param>
    ''' <param name="HyperlinkID">HyperlinkID of records to return</param>
    ''' <param name="Obsolete">Obsolete value of records to return</param>
    ''' <param name="SortBy">Sort Order of returned DataSet</param>
    ''' <returns>DataSet</returns>
    ''' <remarks></remarks>
    Public Shared Function GetForm(ByVal FormID As Nullable(Of Integer), _
        ByVal FormName As String, _
        ByVal HyperlinkID As String, _
        ByVal Obsolete As Nullable(Of Boolean), _
        ByVal SortBy As Nullable(Of Forms_Maint_SortBy)) As DataSet

        Dim cn As SqlConnection = New SqlConnection
        Dim cmd As SqlCommand = New SqlCommand
        Dim ds As New DataSet
        Dim intRowsAffected As Integer
        Dim intReturnValue As Integer
        Dim blnSuccess As Boolean = False
        Dim strMethodParms As String = PARAMETER_LIST_HEADING

        Try
            ' Create the command to execute a stored procedure
            cn.ConnectionString = _
                System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString()
            cmd.Connection = cn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_Get_Form"

            ' Add the input parameters and values
            If FormID.HasValue Then
                cmd.Parameters.Add(New SqlParameter("@parm_FormID", SqlDbType.Int))
                cmd.Parameters("@parm_FormID").Value = FormID
                strMethodParms &= "FormID: " & FormID.ToString & Environment.NewLine
            Else
                strMethodParms &= "FormID: Nothing" & Environment.NewLine
            End If

            If Not String.IsNullOrEmpty(FormName) Then
                cmd.Parameters.Add(New SqlParameter("@parm_FormName", SqlDbType.VarChar, 30))
                cmd.Parameters("@parm_FormName").Value = FormName
                strMethodParms &= "FormName: " & FormName & Environment.NewLine
            Else
                strMethodParms &= "FormName: NullOrEmpty" & Environment.NewLine
            End If

            If Not String.IsNullOrEmpty(HyperlinkID) Then
                cmd.Parameters.Add(New SqlParameter("@parm_HyperlinkID", SqlDbType.VarChar, 300))
                cmd.Parameters("@parm_HyperlinkID").Value = HyperlinkID
                strMethodParms &= "HyperlinkID: " & HyperlinkID & Environment.NewLine
            Else
                strMethodParms &= "HyperlinkID: NullOrEmpty" & Environment.NewLine
            End If

            If Obsolete.HasValue Then
                cmd.Parameters.Add(New SqlParameter("@parm_Obsolete", SqlDbType.Bit))
                cmd.Parameters("@parm_Obsolete").Value = Obsolete
                strMethodParms &= "Obsolete: " & Obsolete.ToString & Environment.NewLine
            Else
                strMethodParms &= "Obsolete: Nothing" & Environment.NewLine
            End If

            If SortBy.HasValue Then
                cmd.Parameters.Add(New SqlParameter("@parm_SortBy", SqlDbType.VarChar, 30))
                cmd.Parameters("@parm_SortBy").Value = SortBy.ToString()
                strMethodParms &= "SortBy: " & SortBy.ToString() & Environment.NewLine
            Else
                strMethodParms &= "SortBy: Nothing" & Environment.NewLine
            End If

            ' Add the output parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_RowsAffected", SqlDbType.Int))
            cmd.Parameters("@parm_RowsAffected").Direction = ParameterDirection.Output

            ' Add the return parameter
            cmd.Parameters.Add(New SqlParameter("ReturnValue", SqlDbType.Int))
            cmd.Parameters("ReturnValue").Direction = ParameterDirection.ReturnValue

            ' Execute the stored procedure,
            ' and return the result.
            cn.Open()
            Dim myAdapter As New SqlDataAdapter(cmd)
            myAdapter.Fill(ds)

            intRowsAffected = Convert.ToInt32(cmd.Parameters("@parm_RowsAffected").Value)
            intReturnValue = Convert.ToInt32(cmd.Parameters("ReturnValue").Value)

            If (intReturnValue <> 0) Then
                Debug.WriteLine("Error encountered SecurityModule.GetForm" & _
                       ControlChars.Cr & _
                       "Error Returned: " & intReturnValue & _
                       ", Rows Affected: " & intRowsAffected)
                ds = Nothing
            Else
                blnSuccess = True
                ds = AddDDColToFormsDataSet(ds)
            End If
        Catch ex As Exception
            Debug.WriteLine("Error encountered in SecurityModule.GetForm" & _
                ControlChars.Cr & ex.Message)
            Dim strMethodName As String = GetExecutingMethodName()
            SendExceptionEmailNotification("SecurityModule", strMethodName, ex, strMethodParms)
            ds = Nothing

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        Finally
            cn.Close()
            cmd.Dispose()
        End Try

        Return ds
    End Function ' GetForm

    ''' <summary>
    ''' Gets a count of the specified Forms_Maint records
    ''' </summary>
    ''' <param name="FormID">FormID of records to count</param>
    ''' <param name="FormName">FormName of records to count</param>
    ''' <param name="HyperlinkID">HyperlinkID of records to count</param>
    ''' <param name="Obsolete">Obsolete value of records to count</param>
    ''' <returns>Record count as Integer</returns>
    ''' <remarks>On error, returns -1</remarks>
    Public Shared Function GetFormCount(ByVal FormID As Nullable(Of Integer), _
        ByVal FormName As String, _
        ByVal HyperlinkID As String, ByVal Obsolete As Nullable(Of Boolean)) As Integer

        Dim cn As SqlConnection = New SqlConnection
        Dim cmd As SqlCommand = New SqlCommand
        Dim ds As New DataSet
        Dim intRowsAffected As Integer = -1
        Dim intReturnValue As Integer
        Dim blnSuccess As Boolean = False
        Dim strMethodParms As String = PARAMETER_LIST_HEADING

        Try
            ' Create the command to execute a stored procedure
            cn.ConnectionString = _
                System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString()
            cmd.Connection = cn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_Get_Form"

            ' Add the input parameters and values
            If FormID.HasValue Then
                cmd.Parameters.Add(New SqlParameter("@parm_FormID", SqlDbType.Int))
                cmd.Parameters("@parm_FormID").Value = FormID
                strMethodParms &= "FormID: " & FormID.ToString & Environment.NewLine
            Else
                strMethodParms &= "FormID: Nothing" & Environment.NewLine
            End If

            If Not String.IsNullOrEmpty(FormName) Then
                cmd.Parameters.Add(New SqlParameter("@parm_FormName", SqlDbType.VarChar, 30))
                cmd.Parameters("@parm_FormName").Value = FormName
                strMethodParms &= "FormName: " & FormName & Environment.NewLine
            Else
                strMethodParms &= "FormName: NullOrEmpty" & Environment.NewLine
            End If

            If Not String.IsNullOrEmpty(HyperlinkID) Then
                cmd.Parameters.Add(New SqlParameter("@parm_HyperlinkID", SqlDbType.VarChar, 300))
                cmd.Parameters("@parm_HyperlinkID").Value = HyperlinkID
                strMethodParms &= "HyperlinkID: " & HyperlinkID & Environment.NewLine
            Else
                strMethodParms &= "HyperlinkID: NullOrEmpty" & Environment.NewLine
            End If

            If Obsolete.HasValue Then
                cmd.Parameters.Add(New SqlParameter("@parm_Obsolete", SqlDbType.Bit))
                cmd.Parameters("@parm_Obsolete").Value = Obsolete
                strMethodParms &= "Obsolete: " & Obsolete.ToString & Environment.NewLine
            Else
                strMethodParms &= "Obsolete: Nothing" & Environment.NewLine
            End If

            ' Add the output parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_RowsAffected", SqlDbType.Int))
            cmd.Parameters("@parm_RowsAffected").Direction = ParameterDirection.Output

            ' Add the return parameter
            cmd.Parameters.Add(New SqlParameter("ReturnValue", SqlDbType.Int))
            cmd.Parameters("ReturnValue").Direction = ParameterDirection.ReturnValue

            ' Execute the stored procedure,
            ' and return the result.
            cn.Open()
            Dim myAdapter As New SqlDataAdapter(cmd)
            myAdapter.Fill(ds)

            intRowsAffected = Convert.ToInt32(cmd.Parameters("@parm_RowsAffected").Value)
            intReturnValue = Convert.ToInt32(cmd.Parameters("ReturnValue").Value)

            If (intReturnValue <> 0) Then
                Debug.WriteLine("Error encountered SecurityModule.GetFormCount" & _
                       ControlChars.Cr & _
                       "Error Returned: " & intReturnValue & _
                       ", Rows Affected: " & intRowsAffected)
                intRowsAffected = -1
            End If
        Catch ex As Exception
            Debug.WriteLine("Error encountered in SecurityModule.GetFormCount" & _
                ControlChars.Cr & ex.Message)
            Dim strMethodName As String = GetExecutingMethodName()
            SendExceptionEmailNotification("SecurityModule", strMethodName, ex, strMethodParms)
            intRowsAffected = -1

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        Finally
            cn.Close()
            cmd.Dispose()
        End Try

        Return intRowsAffected
    End Function ' GetFormCount

    ''' <summary>
    ''' Gets a list of each page url from this site.
    ''' </summary>
    ''' <returns>Returns a 1-dimensional ArrayList of each file with an .aspx or .pdf extension.</returns>
    ''' <remarks>
    ''' Each list entry is stored as "~/directory/pagename.aspx" or "~/directory/pagename.pdf".
    ''' </remarks>
    Public Shared Function GetPageUrls() As ArrayList
        Dim PageList As ArrayList = New ArrayList()
        Try
            Dim strFolderToBrowse As String = HttpContext.Current.Request.PhysicalApplicationPath
            Dim di As DirectoryInfo = New DirectoryInfo(strFolderToBrowse)
            AddPageUrlsToList(di, "~/", PageList, 1, 0)
        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
        Return PageList
    End Function

    ''' <summary>
    ''' Gets the page url list as a DataSet
    ''' </summary>
    ''' <returns>Returns a DataSet of page urls</returns>
    ''' <remarks>Gets the Page urls as an ArrayList, and converts
    ''' the list to a DataSet</remarks>
    Public Function GetPageUrlsAsDataSet() As DataSet
        ' Get the Urls as an ArrayList
        Dim al As ArrayList = GetPageUrls()

        ' Create a new 1-table, 1-column DataSet
        Dim ds As DataSet = New DataSet()
        Try
            Dim dt As DataTable = New DataTable("HyperLinks")
            dt.Columns.Add("ddHyperlinkID", GetType(String))
            ds.Tables.Add(dt)

            ' Load the data from the ArrayList to the DataTable
            dt.BeginLoadData()
            For Each str As String In al
                Dim dr As DataRow = dt.NewRow()
                dr("ddHyperLinkID") = str
                dt.Rows.Add(dr)
            Next
            dt.EndLoadData()
        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

        ' Return the Url list as a DataSet
        Return ds
    End Function

    ''' <summary>
    ''' Insert a new record in the Forms_Maint table
    ''' </summary>
    ''' <param name="FormName">FormName to insert</param>
    ''' <param name="HyperlinkID">HyperlinkID to insert</param>
    ''' <param name="Obsolete">Obsolete value to insert</param>
    ''' <param name="MenuID">MenuID value to insert</param>
    ''' <returns>FormId as Int32</returns>
    ''' <remarks>If success, return the new FormId. 
    ''' If failure, return -1.</remarks>
    Public Shared Function InsertForm(ByVal FormName As String, _
        ByVal HyperlinkID As String, ByVal Obsolete As Boolean, _
        ByVal MenuID As Integer) As Integer

        Dim cn As SqlConnection = New SqlConnection
        Dim cmd As SqlCommand = New SqlCommand
        Dim intIdent As Integer
        Dim intReturnValue As Integer = -1
        Dim strMethodParms As String = PARAMETER_LIST_HEADING

        Try
            ' Create the SQL command to execute a stored procedure
            cn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString()
            cmd.Connection = cn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_Insert_Form"

            ' Add the input parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_FormName", SqlDbType.VarChar, 30))
            cmd.Parameters("@parm_FormName").Value = FormName
            strMethodParms &= "FormName: " & FormName & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_HyperlinkID", SqlDbType.VarChar, 300))
            cmd.Parameters("@parm_HyperlinkID").Value = HyperlinkID
            strMethodParms &= "HyperlinkID: " & HyperlinkID & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_MenuID", SqlDbType.Int))
            cmd.Parameters("@parm_MenuID").Value = MenuID
            strMethodParms &= "MenuID: " & MenuID & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_Obsolete", SqlDbType.Bit))
            cmd.Parameters("@parm_Obsolete").Value = Convert.ToByte(Obsolete)
            strMethodParms &= "Obsolete: " & Obsolete.ToString & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_CreatedBy", SqlDbType.VarChar, 50))
            cmd.Parameters("@parm_CreatedBy").Value = GetAbbrevNameFromCurrentUser()

            ' Add the output parameters and values
            'cmd.Parameters.Add(New SqlParameter("@parm_Identity", SqlDbType.Int))
            cmd.Parameters.Add(New SqlParameter("@parm_Ident", SqlDbType.Int))
            cmd.Parameters("@parm_Ident").Direction = ParameterDirection.Output

            ' Add the return parameter
            cmd.Parameters.Add(New SqlParameter("ReturnValue", SqlDbType.Int))
            cmd.Parameters("ReturnValue").Direction = ParameterDirection.ReturnValue

            ' Execute the stored procedure,
            ' and return the result.
            cn.Open()
            'cmd.ExecuteNonQuery()
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            dr.Close()
            intIdent = Convert.ToInt32(cmd.Parameters("@parm_Ident").Value)
            intReturnValue = Convert.ToInt32(cmd.Parameters("ReturnValue").Value)

            If intReturnValue <> 0 Then
                Debug.WriteLine("Error encountered SecurityModule.InsertForm" & _
                       ControlChars.Cr & _
                       "Error Returned: " & intReturnValue)
                intIdent = -1
            End If
        Catch ex As Exception
            Debug.WriteLine("Error encountered in SecurityModule.InsertForm" & _
                ControlChars.Cr & ex.Message)
            intIdent = -1
            Dim strMethodName As String = GetExecutingMethodName()
            SendExceptionEmailNotification("SecurityModule", strMethodName, ex, strMethodParms)

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        Finally
            cn.Close()
            cmd.Dispose()
        End Try
        Return intIdent
    End Function ' InsertForm

    ''' <summary>
    ''' Updates the specified record in the Forms_Maint table
    ''' </summary>
    ''' <param name="FormId">The record key of the record to update</param>
    ''' <param name="FormName">New FormName value</param>
    ''' <param name="HyperlinkID">New HyperlinkID value</param>
    ''' <param name="Obsolete">New Obsolete value</param>
    ''' <param name="MenuID">New MenuID value</param>
    ''' <returns>True if success, otherwise False</returns>
    ''' <remarks>
    ''' </remarks>
    Public Shared Function UpdateForm(ByVal FormId As Integer, _
            ByVal FormName As String, _
            ByVal HyperlinkID As String, _
            ByVal Obsolete As Boolean, _
            ByVal MenuID As Integer) As Boolean

        Dim cn As SqlConnection = New SqlConnection
        Dim cmd As SqlCommand = New SqlCommand
        Dim intRowsAffected As Integer
        Dim intReturnValue As Integer
        Dim blnSuccess As Boolean = False
        Dim strMethodParms As String = PARAMETER_LIST_HEADING

        Try
            ' Create the command to execute a stored procedure
            cn.ConnectionString = _
                System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString()
            cmd.Connection = cn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_Update_Form"

            ' Add the input parameters and values

            cmd.Parameters.Add(New SqlParameter("@parm_FormID", SqlDbType.Int))
            cmd.Parameters("@parm_FormID").Value = FormId
            strMethodParms &= "FormID: " & FormId & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_FormName", SqlDbType.VarChar, 30))
            cmd.Parameters("@parm_FormName").Value = FormName
            strMethodParms &= "FormName: " & FormName & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_HyperlinkID", SqlDbType.VarChar, 300))
            cmd.Parameters("@parm_HyperlinkID").Value = HyperlinkID
            strMethodParms &= "HyperlinkID: " & HyperlinkID & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_MenuID", SqlDbType.Int))
            cmd.Parameters("@parm_MenuID").Value = MenuID
            strMethodParms &= "MenuID: " & MenuID & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_Obsolete", SqlDbType.Bit))
            cmd.Parameters("@parm_Obsolete").Value = Obsolete
            strMethodParms &= "Obsolete: " & Obsolete.ToString() & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_UpdatedBy", SqlDbType.VarChar, 50))
            cmd.Parameters("@parm_UpdatedBy").Value = GetAbbrevNameFromCurrentUser()

            ' Add the output parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_RowsAffected", SqlDbType.Int))
            cmd.Parameters("@parm_RowsAffected").Direction = ParameterDirection.Output

            ' Add the return parameter
            cmd.Parameters.Add(New SqlParameter("ReturnValue", SqlDbType.Int))
            cmd.Parameters("ReturnValue").Direction = ParameterDirection.ReturnValue

            ' Execute the stored procedure,
            ' and return the result.
            cn.Open()
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            dr.Close()
            intRowsAffected = Convert.ToInt32(cmd.Parameters("@parm_RowsAffected").Value)
            intReturnValue = Convert.ToInt32(cmd.Parameters("ReturnValue").Value)

            If (intReturnValue <> 0) Or _
               (intRowsAffected <> 1) Then
                Debug.WriteLine("Error encountered SecurityModule.UpdateForm" & _
                       ControlChars.Cr & _
                       "Error Returned: " & intReturnValue & _
                       ", Rows Affected: " & intRowsAffected)
            Else
                blnSuccess = True
            End If
        Catch ex As Exception
            Debug.WriteLine("Error encountered in SecurityModule.UpdateForm" & _
                ControlChars.Cr & ex.Message)
            Dim strMethodName As String = GetExecutingMethodName()
            SendExceptionEmailNotification("SecurityModule", strMethodName, ex, strMethodParms)

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        Finally
            cn.Close()
            cmd.Dispose()
        End Try
        Return blnSuccess
    End Function ' UpdateForm

#End Region ' Forms_Maint Functions


#Region "Roles_Maint Functions"

    ''' <summary>
    ''' Gets a DataSet of Roles_Maint records
    ''' </summary>
    ''' <param name="RoleID">RoleID of records to return</param>
    ''' <param name="RoleName">RoleName of records to return</param>
    ''' <param name="Description">Description of records to return</param>
    ''' <param name="Obsolete">Obsolete value of records to return</param>
    ''' <param name="SortBy">Sort Order of returned DataSet</param>
    ''' <returns>DataSet</returns>
    ''' <remarks></remarks>
    Public Shared Function GetRole(ByVal RoleID As Nullable(Of Integer), _
        ByVal RoleName As String, _
        ByVal Description As String, _
        ByVal Obsolete As Nullable(Of Boolean), _
        ByVal SortBy As Nullable(Of Roles_Maint_SortBy)) As DataSet

        Dim cn As SqlConnection = New SqlConnection
        Dim cmd As SqlCommand = New SqlCommand
        Dim ds As New DataSet
        Dim intRowsAffected As Integer
        Dim intReturnValue As Integer
        Dim blnSuccess As Boolean = False
        Dim strMethodParms As String = PARAMETER_LIST_HEADING

        Try
            ' Create the command to execute a stored procedure
            cn.ConnectionString = _
                System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString()
            cmd.Connection = cn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_Get_Role"

            ' Add the input parameters and values
            If RoleID.HasValue Then
                cmd.Parameters.Add(New SqlParameter("@parm_RoleID", SqlDbType.Int))
                cmd.Parameters("@parm_RoleID").Value = RoleID
                strMethodParms &= "RoleID: " & RoleID.ToString & Environment.NewLine
            Else
                strMethodParms &= "RoleID: Nothing" & Environment.NewLine
            End If

            If Not String.IsNullOrEmpty(RoleName) Then
                cmd.Parameters.Add(New SqlParameter("@parm_RoleName", SqlDbType.VarChar, 30))
                cmd.Parameters("@parm_RoleName").Value = RoleName
                strMethodParms &= "RoleName: " & RoleName & Environment.NewLine
            Else
                strMethodParms &= "RoleName: NullOrEmpty" & Environment.NewLine
            End If

            If Not String.IsNullOrEmpty(Description) Then
                cmd.Parameters.Add(New SqlParameter("@parm_Description", SqlDbType.VarChar, 30))
                cmd.Parameters("@parm_Description").Value = Description
                strMethodParms &= "Desciption: " & Description & Environment.NewLine
            Else
                strMethodParms &= "Description: NullOrEmpty" & Environment.NewLine
            End If

            If Obsolete.HasValue Then
                cmd.Parameters.Add(New SqlParameter("@parm_Obsolete", SqlDbType.Bit))
                cmd.Parameters("@parm_Obsolete").Value = Obsolete
                strMethodParms &= "Obsolete: " & Obsolete.ToString & Environment.NewLine
            Else
                strMethodParms &= "Obsolete: Nothing" & Environment.NewLine
            End If

            If SortBy.HasValue Then
                cmd.Parameters.Add(New SqlParameter("@parm_SortBy", SqlDbType.VarChar, 30))
                cmd.Parameters("@parm_SortBy").Value = SortBy.ToString()
                strMethodParms &= "SortBy: " & SortBy.ToString & Environment.NewLine
            Else
                strMethodParms &= "SortBy: Nothing" & Environment.NewLine
            End If

            ' Add the output parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_RowsAffected", SqlDbType.Int))
            cmd.Parameters("@parm_RowsAffected").Direction = ParameterDirection.Output

            ' Add the return parameter
            cmd.Parameters.Add(New SqlParameter("ReturnValue", SqlDbType.Int))
            cmd.Parameters("ReturnValue").Direction = ParameterDirection.ReturnValue

            ' Execute the stored procedure,
            ' and return the result.
            cn.Open()
            Dim myAdapter As New SqlDataAdapter(cmd)
            myAdapter.Fill(ds)

            intRowsAffected = Convert.ToInt32(cmd.Parameters("@parm_RowsAffected").Value)
            intReturnValue = Convert.ToInt32(cmd.Parameters("ReturnValue").Value)

            If (intReturnValue <> 0) Then
                Debug.WriteLine("Error encountered SecurityModule.GetRole" & _
                       ControlChars.Cr & _
                       "Error Returned: " & intReturnValue & _
                       ", Rows Affected: " & intRowsAffected)
            Else
                blnSuccess = True
            End If

        Catch ex As Exception
            Dim strMethodName As String = GetExecutingMethodName()
            SendExceptionEmailNotification("SecurityModule", strMethodName, ex, strMethodParms)

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        Finally
            cn.Close()
            cmd.Dispose()
        End Try

        Return ds
    End Function ' GetRole

    ''' <summary>
    ''' Gets a count of the specified Roles_Maint records
    ''' </summary>
    ''' <param name="RoleID">RoleID of records to count</param>
    ''' <param name="RoleName">RoleName of records to count</param>
    ''' <param name="Description">Description of records to count</param>
    ''' <param name="Obsolete">Obsolete value of records to count</param>
    ''' <returns>Record count as Integer</returns>
    ''' <remarks></remarks>
    Public Shared Function GetRoleCount(ByVal RoleID As Nullable(Of Integer), _
        ByVal RoleName As String, _
        ByVal Description As String, ByVal Obsolete As Nullable(Of Boolean)) As Integer

        Dim cn As SqlConnection = New SqlConnection
        Dim cmd As SqlCommand = New SqlCommand
        Dim ds As New DataSet
        Dim intRowsAffected As Integer = -1
        Dim intReturnValue As Integer
        Dim blnSuccess As Boolean = False
        Dim strMethodParms As String = PARAMETER_LIST_HEADING

        Try
            ' Create the command to execute a stored procedure
            cn.ConnectionString = _
                System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString()
            cmd.Connection = cn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_Get_Role"

            ' Add the input parameters and values
            If RoleID.HasValue Then
                cmd.Parameters.Add(New SqlParameter("@parm_RoleID", SqlDbType.Int))
                cmd.Parameters("@parm_RoleID").Value = RoleID
                strMethodParms &= "RoleID: " & RoleID.ToString & Environment.NewLine
            Else
                strMethodParms &= "RoleID: Nothing" & Environment.NewLine
            End If

            If Not String.IsNullOrEmpty(RoleName) Then
                cmd.Parameters.Add(New SqlParameter("@parm_RoleName", SqlDbType.VarChar, 30))
                cmd.Parameters("@parm_RoleName").Value = RoleName
                strMethodParms &= "RoleName: " & RoleName & Environment.NewLine
            Else
                strMethodParms &= "RoleName: NullOrEmpty" & Environment.NewLine
            End If

            If Not String.IsNullOrEmpty(Description) Then
                cmd.Parameters.Add(New SqlParameter("@parm_Description", SqlDbType.VarChar, 30))
                cmd.Parameters("@parm_Description").Value = Description
                strMethodParms &= "Description: " & Description & Environment.NewLine
            Else
                strMethodParms &= "Description: NullOrEmpty" & Environment.NewLine
            End If

            If Obsolete.HasValue Then
                cmd.Parameters.Add(New SqlParameter("@parm_Obsolete", SqlDbType.Bit))
                cmd.Parameters("@parm_Obsolete").Value = Obsolete
                strMethodParms &= "Obsolete: " & Obsolete.ToString & Environment.NewLine
            Else
                strMethodParms &= "Obsolete: Nothing" & Environment.NewLine
            End If

            ' Add the output parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_RowsAffected", SqlDbType.Int))
            cmd.Parameters("@parm_RowsAffected").Direction = ParameterDirection.Output

            ' Add the return parameter
            cmd.Parameters.Add(New SqlParameter("ReturnValue", SqlDbType.Int))
            cmd.Parameters("ReturnValue").Direction = ParameterDirection.ReturnValue

            ' Execute the stored procedure,
            ' and return the result.
            cn.Open()
            Dim myAdapter As New SqlDataAdapter(cmd)
            myAdapter.Fill(ds)

            intRowsAffected = Convert.ToInt32(cmd.Parameters("@parm_RowsAffected").Value)
            intReturnValue = Convert.ToInt32(cmd.Parameters("ReturnValue").Value)

            If (intReturnValue <> 0) Then
                Debug.WriteLine("Error encountered SecurityModule.GetRoleCount" & _
                       ControlChars.Cr & _
                       "Error Returned: " & intReturnValue & _
                       ", Rows Affected: " & intRowsAffected)
                intRowsAffected = -1
            End If
        Catch ex As Exception
            Debug.WriteLine("Error encountered in SecurityModule.GetRoleCount" & _
                ControlChars.Cr & ex.Message)
            Dim strMethodName As String = GetExecutingMethodName()
            SendExceptionEmailNotification("SecurityModule", strMethodName, ex, strMethodParms)
            intRowsAffected = -1

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        Finally
            cn.Close()
            cmd.Dispose()
        End Try

        Return intRowsAffected
    End Function ' GetRoleCount

    ''' <summary>
    ''' Insert a new record in the Roles_Maint table
    ''' </summary>
    ''' <param name="RoleName">RoleName to insert</param>
    ''' <param name="Description">Description to insert</param>
    ''' <param name="Obsolete">Obsolete value to insert</param>
    ''' <returns>New RoleId as Int32</returns>
    ''' <remarks>If success, return the new RoleId. 
    ''' If failure, return -1.</remarks>
    Public Shared Function InsertRole(ByVal RoleName As String, _
        ByVal Description As String, ByVal Obsolete As Boolean) As Integer

        Dim cn As SqlConnection = New SqlConnection
        Dim cmd As SqlCommand = New SqlCommand
        Dim intIdent As Integer
        Dim intReturnValue As Integer = -1
        Dim strMethodParms As String = PARAMETER_LIST_HEADING

        Try
            ' Create the SQL command to execute a stored procedure
            cn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString()
            cmd.Connection = cn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_Insert_Role"

            ' Add the input parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_RoleName", SqlDbType.VarChar, 30))
            cmd.Parameters("@parm_RoleName").Value = RoleName
            strMethodParms &= "RoleName: " & RoleName & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_Description", SqlDbType.VarChar, 50))
            cmd.Parameters("@parm_Description").Value = Description
            strMethodParms &= "Description: " & Description & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_Obsolete", SqlDbType.Bit))
            cmd.Parameters("@parm_Obsolete").Value = Convert.ToByte(Obsolete)
            strMethodParms &= "Obsolete: " & Obsolete.ToString & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_CreatedBy", SqlDbType.VarChar, 50))
            cmd.Parameters("@parm_CreatedBy").Value = GetAbbrevNameFromCurrentUser()

            ' Add the output parameters and values
            'cmd.Parameters.Add(New SqlParameter("@parm_Identity", SqlDbType.Int))
            cmd.Parameters.Add(New SqlParameter("@parm_Ident", SqlDbType.Int))
            cmd.Parameters("@parm_Ident").Direction = ParameterDirection.Output

            ' Add the return parameter
            cmd.Parameters.Add(New SqlParameter("ReturnValue", SqlDbType.Int))
            cmd.Parameters("ReturnValue").Direction = ParameterDirection.ReturnValue

            ' Execute the stored procedure,
            ' and return the result.
            cn.Open()
            'cmd.ExecuteNonQuery()
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            dr.Close()
            intIdent = Convert.ToInt32(cmd.Parameters("@parm_Ident").Value)
            intReturnValue = Convert.ToInt32(cmd.Parameters("ReturnValue").Value)

            If intReturnValue <> 0 Then
                Debug.WriteLine("Error encountered SecurityModule.InsertRole" & _
                       ControlChars.Cr & _
                       "Error Returned: " & intReturnValue)
                intIdent = -1
            End If
        Catch ex As Exception
            Debug.WriteLine("Error encountered in SecurityModule.InsertRole" & _
                ControlChars.Cr & ex.Message)
            intIdent = -1
            Dim strMethodName As String = GetExecutingMethodName()
            SendExceptionEmailNotification("SecurityModule", strMethodName, ex, strMethodParms)

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        Finally
            cn.Close()
            cmd.Dispose()
        End Try
        Return intIdent
    End Function ' InsertRole

    ''' <summary>
    ''' Updates the specified record in the Roles_Maint table
    ''' </summary>
    ''' <param name="RoleId">The record key of the record to update</param>
    ''' <param name="RoleName">New RoleName value</param>
    ''' <param name="Description">New Description value</param>
    ''' <param name="Obsolete">New Obsolete value</param>
    ''' <returns>True if success, otherwise False</returns>
    ''' <remarks>
    ''' </remarks>
    Public Shared Function UpdateRole(ByVal RoleId As Integer, _
            ByVal RoleName As String, _
            ByVal Description As String, _
            ByVal Obsolete As Boolean) As Boolean

        Dim cn As SqlConnection = New SqlConnection
        Dim cmd As SqlCommand = New SqlCommand
        Dim intRowsAffected As Integer
        Dim intReturnValue As Integer
        Dim blnSuccess As Boolean = False
        Dim strMethodParms As String = PARAMETER_LIST_HEADING

        Try
            ' Create the command to execute a stored procedure
            cn.ConnectionString = _
                System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString()
            cmd.Connection = cn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_Update_Role"

            ' Add the input parameters and values

            cmd.Parameters.Add(New SqlParameter("@parm_RoleID", SqlDbType.Int))
            cmd.Parameters("@parm_RoleID").Value = RoleId
            strMethodParms &= "RoleID: " & RoleId.ToString & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_RoleName", SqlDbType.VarChar, 30))
            cmd.Parameters("@parm_RoleName").Value = RoleName
            strMethodParms &= "RoleName: " & RoleName & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_Description", SqlDbType.VarChar, 50))
            cmd.Parameters("@parm_Description").Value = Description
            strMethodParms &= "Description: " & Description & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_Obsolete", SqlDbType.Bit))
            cmd.Parameters("@parm_Obsolete").Value = Obsolete
            strMethodParms &= "Obsolete: " & Obsolete.ToString & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_UpdatedBy", SqlDbType.VarChar, 50))
            cmd.Parameters("@parm_UpdatedBy").Value = GetAbbrevNameFromCurrentUser()

            ' Add the output parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_RowsAffected", SqlDbType.Int))
            cmd.Parameters("@parm_RowsAffected").Direction = ParameterDirection.Output

            ' Add the return parameter
            cmd.Parameters.Add(New SqlParameter("ReturnValue", SqlDbType.Int))
            cmd.Parameters("ReturnValue").Direction = ParameterDirection.ReturnValue

            ' Execute the stored procedure,
            ' and return the result.
            cn.Open()
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            dr.Close()
            intRowsAffected = Convert.ToInt32(cmd.Parameters("@parm_RowsAffected").Value)
            intReturnValue = Convert.ToInt32(cmd.Parameters("ReturnValue").Value)

            If (intReturnValue <> 0) Or _
               (intRowsAffected <> 1) Then
                Debug.WriteLine("Error encountered SecurityModule.UpdateRole" & _
                       ControlChars.Cr & _
                       "Error Returned: " & intReturnValue & _
                       ", Rows Affected: " & intRowsAffected)
            Else
                blnSuccess = True
            End If
        Catch ex As Exception
            Debug.WriteLine("Error encountered in SecurityModule.UpdateRole" & _
                ControlChars.Cr & ex.Message)
            Dim strMethodName As String = GetExecutingMethodName()
            SendExceptionEmailNotification("SecurityModule", strMethodName, ex, strMethodParms)

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        Finally
            cn.Close()
            cmd.Dispose()
        End Try
        Return blnSuccess
    End Function ' UpdateRole
#End Region ' Roles_Maint Functions


#Region "Subscriptions_Maint Functions"

    ''' <summary>
    ''' Gets a DataSet of all Subscription_Maint records 
    ''' </summary>
    ''' <returns>Subscriptions Dataset</returns>
    ''' <remarks>Only gets data not marked as obsolete</remarks>
    Public Shared Function GetSubscriptions() As DataSet
        Dim cn As SqlConnection = New SqlConnection
        Dim cmd As SqlCommand = New SqlCommand
        Dim ds As New DataSet

        Try
            cn.ConnectionString = _
                System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString()
            cn.Open()

            cmd.Connection = cn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_Get_Subscriptions_Maint"

            cmd.Parameters.Add(New SqlParameter("@Subscription", SqlDbType.VarChar, 25))
            cmd.Parameters("@Subscription").Value = ""

            Dim myAdapter As New SqlDataAdapter(cmd)
            myAdapter.Fill(ds)

        Catch ex As Exception
            ds = Nothing
            Dim strMethodName As String = GetExecutingMethodName()
            SendExceptionEmailNotification("SecurityModule", strMethodName, ex, Nothing)

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        Finally
            cn.Close()
            cmd.Dispose()
        End Try
        Return ds
    End Function

#End Region ' Subscriptions_Maint Functions


#Region "TeamMember_Maint Functions"

    ''' <summary>
    ''' Gets a DataSet of the specified TeamMember_Maint records
    ''' </summary>
    ''' <param name="TeamMemberID">Filter by TeamMemberID</param>
    ''' <param name="UserName">Filter by UserName</param>
    ''' <param name="ShortName">Filter by ShortName</param>
    ''' <param name="LastName">Filter by LastName</param>
    ''' <param name="FirstName">Filter by FirstName</param>
    ''' <param name="Email">Filter by Email</param>
    ''' <param name="Working">Filter by Working</param>
    ''' <param name="SortBy">Sort Order of returned DataSet</param>
    ''' <returns>DataSet</returns>
    ''' <remarks></remarks>
    Public Shared Function GetTeamMember(ByVal TeamMemberID As Nullable(Of Integer), _
        ByVal UserName As String, _
        ByVal ShortName As String, _
        ByVal LastName As String, _
        ByVal FirstName As String, _
        ByVal Email As String, _
        ByVal Working As Nullable(Of TeamMember_Maint_WorkStatus), _
        ByVal SortBy As Nullable(Of TeamMember_Maint_Sortby)) As DataSet

        Dim cn As SqlConnection = New SqlConnection
        Dim cmd As SqlCommand = New SqlCommand
        Dim ds As New DataSet
        Dim intRowsAffected As Integer
        Dim intReturnValue As Integer
        Dim blnSuccess As Boolean = False
        Dim strMethodParms As String = PARAMETER_LIST_HEADING

        Try
            ' Create the command to execute a stored procedure
            cn.ConnectionString = _
                System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString()
            cmd.Connection = cn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_Get_TeamMember"

            ' Add the input parameters and values
            If TeamMemberID.HasValue Then
                cmd.Parameters.Add(New SqlParameter("@parm_TeamMemberID", SqlDbType.Int))
                cmd.Parameters("@parm_TeamMemberID").Value = TeamMemberID
                strMethodParms &= "TeamMemberID: " & TeamMemberID.ToString & Environment.NewLine
            Else
                strMethodParms &= "TeamMemberID: Nothing" & Environment.NewLine
            End If

            If Not String.IsNullOrEmpty(UserName) Then
                cmd.Parameters.Add(New SqlParameter("@parm_UserName", SqlDbType.VarChar, 50))
                cmd.Parameters("@parm_UserName").Value = UserName
                strMethodParms &= "UserName: " & UserName & Environment.NewLine
            Else
                strMethodParms &= "UserName: NullOrEmpty" & Environment.NewLine
            End If

            If Not String.IsNullOrEmpty(ShortName) Then
                cmd.Parameters.Add(New SqlParameter("@parm_ShortName", SqlDbType.VarChar, 40))
                cmd.Parameters("@parm_ShortName").Value = ShortName
                strMethodParms &= "ShortName: " & ShortName & Environment.NewLine
            Else
                strMethodParms &= "ShortName: NullOrEmpty" & Environment.NewLine
            End If

            If Not String.IsNullOrEmpty(LastName) Then
                cmd.Parameters.Add(New SqlParameter("@parm_LastName", SqlDbType.VarChar, 30))
                cmd.Parameters("@parm_LastName").Value = LastName
                strMethodParms &= "LastName: " & LastName & Environment.NewLine
            Else
                strMethodParms &= "LastName: NullOrEmpty" & Environment.NewLine
            End If

            If Not String.IsNullOrEmpty(FirstName) Then
                cmd.Parameters.Add(New SqlParameter("@parm_FirstName", SqlDbType.VarChar, 30))
                cmd.Parameters("@parm_FirstName").Value = FirstName
                strMethodParms &= "FirstName: " & FirstName & Environment.NewLine
            Else
                strMethodParms &= "FirstName: NullOrEmpty" & Environment.NewLine
            End If

            If Not String.IsNullOrEmpty(Email) Then
                cmd.Parameters.Add(New SqlParameter("@parm_Email", SqlDbType.VarChar, 50))
                cmd.Parameters("@parm_Email").Value = Email
                strMethodParms &= "Email: " & Email & Environment.NewLine
            Else
                strMethodParms &= "Email: NullOrEmpty" & Environment.NewLine
            End If

            If (Working.HasValue) AndAlso (Working.Value <> TeamMember_Maint_WorkStatus.Both) Then
                cmd.Parameters.Add(New SqlParameter("@parm_Working", SqlDbType.Bit))
                If Working.Value = TeamMember_Maint_WorkStatus.NotWorking Then
                    cmd.Parameters("@parm_Working").Value = False
                Else
                    cmd.Parameters("@parm_Working").Value = True
                End If
                strMethodParms &= "Working: " & Working.ToString & Environment.NewLine
            Else
                strMethodParms &= "Working: Nothing" & Environment.NewLine
            End If

            If SortBy.HasValue Then
                cmd.Parameters.Add(New SqlParameter("@parm_SortBy", SqlDbType.VarChar, 30))
                cmd.Parameters("@parm_SortBy").Value = SortBy.ToString()
                strMethodParms &= "SortBy: " & SortBy.ToString & Environment.NewLine
            Else
                strMethodParms &= "SortBy: Nothing" & Environment.NewLine
            End If

            ' Add the output parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_RowsAffected", SqlDbType.Int))
            cmd.Parameters("@parm_RowsAffected").Direction = ParameterDirection.Output

            ' Add the return parameter
            cmd.Parameters.Add(New SqlParameter("ReturnValue", SqlDbType.Int))
            cmd.Parameters("ReturnValue").Direction = ParameterDirection.ReturnValue

            ' Execute the stored procedure,
            ' and return the result.
            cn.Open()
            Dim myAdapter As New SqlDataAdapter(cmd)
            myAdapter.Fill(ds)

            intRowsAffected = Convert.ToInt32(cmd.Parameters("@parm_RowsAffected").Value)
            intReturnValue = Convert.ToInt32(cmd.Parameters("ReturnValue").Value)

            If (intReturnValue <> 0) Then
                Debug.WriteLine("Error encountered SecurityModule.GetRole" & _
                       ControlChars.Cr & _
                       "Error Returned: " & intReturnValue & _
                       ", Rows Affected: " & intRowsAffected)
            Else
                blnSuccess = True
            End If
        Catch ex As Exception
            Debug.WriteLine("Error encountered in SecurityModule.GetTeamMember" & _
                ControlChars.Cr & ex.Message)
            Dim strMethodName As String = GetExecutingMethodName()
            SendExceptionEmailNotification("SecurityModule", strMethodName, ex, strMethodParms)

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        Finally
            cn.Close()
            cmd.Dispose()
        End Try

        Return ds
    End Function ' GetTeamMember

    ''' <summary>
    ''' Gets a Count of the specified TeamMember_Maint records
    ''' </summary>
    ''' <param name="TeamMemberID">Filter by TeamMemberID</param>
    ''' <param name="UserName">Filter by UserName</param>
    ''' <param name="ShortName">Filter by ShortName</param>
    ''' <param name="LastName">Filter by LastName</param>
    ''' <param name="FirstName">Filter by FirstName</param>
    ''' <param name="Email">Filter by Email</param>
    ''' <param name="Working">Filter by Working</param>
    ''' <param name="SortBy">Sort Order of returned DataSet</param>
    ''' <returns>Number of records that match filter parameters</returns>
    ''' <remarks>Returns -1, if exception</remarks>
    Public Shared Function GetTeamMemberCount(ByVal TeamMemberID As Nullable(Of Integer), _
        ByVal UserName As String, _
        ByVal ShortName As String, _
        ByVal LastName As String, _
        ByVal FirstName As String, _
        ByVal Email As String, _
        ByVal Working As Nullable(Of Boolean), _
        ByVal SortBy As Nullable(Of TeamMember_Maint_Sortby)) As Integer

        Dim cn As SqlConnection = New SqlConnection
        Dim cmd As SqlCommand = New SqlCommand
        Dim ds As New DataSet
        Dim intRowsAffected As Integer
        Dim intReturnValue As Integer = -1
        Dim blnSuccess As Boolean = False
        Dim strMethodParms As String = PARAMETER_LIST_HEADING

        Try
            ' Create the command to execute a stored procedure
            cn.ConnectionString = _
                System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString()
            cmd.Connection = cn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_Get_TeamMember"

            ' Add the input parameters and values
            If TeamMemberID.HasValue Then
                cmd.Parameters.Add(New SqlParameter("@parm_TeamMemberID", SqlDbType.Int))
                cmd.Parameters("@parm_TeamMemberID").Value = TeamMemberID
                strMethodParms &= "TeamMemberID: " & TeamMemberID.ToString & Environment.NewLine
            Else
                strMethodParms &= "TeamMemberID: Nothing" & Environment.NewLine
            End If

            If Not String.IsNullOrEmpty(UserName) Then
                cmd.Parameters.Add(New SqlParameter("@parm_UserName", SqlDbType.VarChar, 50))
                cmd.Parameters("@parm_UserName").Value = UserName
                strMethodParms &= "UserName: " & UserName & Environment.NewLine
            Else
                strMethodParms &= "UserName: NullOrEmpty" & Environment.NewLine
            End If

            If Not String.IsNullOrEmpty(ShortName) Then
                cmd.Parameters.Add(New SqlParameter("@parm_ShortName", SqlDbType.VarChar, 40))
                cmd.Parameters("@parm_ShortName").Value = ShortName
                strMethodParms &= "ShortName: " & ShortName & Environment.NewLine
            Else
                strMethodParms &= "ShortName: NullOrEmpty" & Environment.NewLine
            End If

            If Not String.IsNullOrEmpty(LastName) Then
                cmd.Parameters.Add(New SqlParameter("@parm_LastName", SqlDbType.VarChar, 30))
                cmd.Parameters("@parm_LastName").Value = LastName
                strMethodParms &= "LastName: " & LastName & Environment.NewLine
            Else
                strMethodParms &= "LastName: NullOrEmpty" & Environment.NewLine
            End If

            If Not String.IsNullOrEmpty(FirstName) Then
                cmd.Parameters.Add(New SqlParameter("@parm_FirstName", SqlDbType.VarChar, 30))
                cmd.Parameters("@parm_FirstName").Value = FirstName
                strMethodParms &= "FirstName: " & FirstName & Environment.NewLine
            Else
                strMethodParms &= "FirstName: NullOrEmpty" & Environment.NewLine
            End If

            If Not String.IsNullOrEmpty(Email) Then
                cmd.Parameters.Add(New SqlParameter("@parm_Email", SqlDbType.VarChar, 50))
                cmd.Parameters("@parm_Email").Value = Email
                strMethodParms &= "Email: " & Email & Environment.NewLine
            Else
                strMethodParms &= "Email: NullOrEmpty" & Environment.NewLine
            End If

            If Working.HasValue Then
                cmd.Parameters.Add(New SqlParameter("@parm_Working", SqlDbType.Bit))
                cmd.Parameters("@parm_Working").Value = Working
                strMethodParms &= "Working: " & Working.ToString & Environment.NewLine
            Else
                strMethodParms &= "Working: Nothing" & Environment.NewLine
            End If

            If SortBy.HasValue Then
                cmd.Parameters.Add(New SqlParameter("@parm_SortBy", SqlDbType.VarChar, 30))
                cmd.Parameters("@parm_SortBy").Value = SortBy.ToString()
                strMethodParms &= "SortBy: " & SortBy.ToString & Environment.NewLine
            Else
                strMethodParms &= "SortBy: Nothing" & Environment.NewLine
            End If

            ' Add the output parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_RowsAffected", SqlDbType.Int))
            cmd.Parameters("@parm_RowsAffected").Direction = ParameterDirection.Output

            ' Add the return parameter
            cmd.Parameters.Add(New SqlParameter("ReturnValue", SqlDbType.Int))
            cmd.Parameters("ReturnValue").Direction = ParameterDirection.ReturnValue

            ' Execute the stored procedure,
            ' and return the result.
            cn.Open()
            Dim myAdapter As New SqlDataAdapter(cmd)
            myAdapter.Fill(ds)

            intRowsAffected = Convert.ToInt32(cmd.Parameters("@parm_RowsAffected").Value)
            intReturnValue = Convert.ToInt32(cmd.Parameters("ReturnValue").Value)

            If (intReturnValue <> 0) Then
                Debug.WriteLine("Error encountered SecurityModule.GetRole" & _
                       ControlChars.Cr & _
                       "Error Returned: " & intReturnValue & _
                       ", Rows Affected: " & intRowsAffected)
                intRowsAffected = -1
            Else
                blnSuccess = True
            End If
        Catch ex As Exception
            Debug.WriteLine("Error encountered in SecurityModule.GetTeamMemberCount" & _
                ControlChars.Cr & ex.Message)
            Dim strMethodName As String = GetExecutingMethodName()
            SendExceptionEmailNotification("SecurityModule", strMethodName, ex, strMethodParms)
            intRowsAffected = -1

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        Finally
            cn.Close()
            cmd.Dispose()
        End Try

        Return intRowsAffected
    End Function ' GetTeamMemberCount


    ''' <summary>
    ''' Inserts a record into the TeamMember_Maint table with specified values
    ''' </summary>
    ''' <param name="UserName">Value of UserName in nre record</param>
    ''' <param name="ShortName">Value of ShortName in new record</param>
    ''' <param name="LastName">Value of LastName in new record</param>
    ''' <param name="FirstName">Value of FirstName in new record</param>
    ''' <param name="Email">Value of Email in new record</param>
    ''' <param name="Working">Value of Working in new record</param>
    ''' <returns>Autogenerated TeamMemberID of new record</returns>
    ''' <remarks></remarks>
    Public Shared Function InsertTeamMember(ByVal UserName As String, _
    ByVal ShortName As String, ByVal LastName As String, ByVal FirstName As String, _
    ByVal Email As String, ByVal Working As Boolean) As Integer

        Dim cn As SqlConnection = New SqlConnection
        Dim cmd As SqlCommand = New SqlCommand
        Dim intIdent As Integer
        Dim intReturnValue As Integer = -1
        Dim strMethodParms As String = PARAMETER_LIST_HEADING

        Try
            ' Create the SQL command to execute a stored procedure
            cn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString()
            cmd.Connection = cn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_Insert_TeamMember"

            ' Add the input parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_UserName", SqlDbType.VarChar, 50))
            cmd.Parameters("@parm_UserName").Value = UserName
            strMethodParms &= "UserName: " & UserName & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_ShortName", SqlDbType.VarChar, 40))
            cmd.Parameters("@parm_ShortName").Value = ShortName
            strMethodParms &= "ShortName: " & ShortName & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_LastName", SqlDbType.VarChar, 30))
            cmd.Parameters("@parm_LastName").Value = LastName
            strMethodParms &= "LastName: " & LastName & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_FirstName", SqlDbType.VarChar, 30))
            cmd.Parameters("@parm_FirstName").Value = FirstName
            strMethodParms &= "FirstName: " & FirstName & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_Email", SqlDbType.VarChar, 50))
            cmd.Parameters("@parm_Email").Value = Email
            strMethodParms &= "Email: " & Email & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_Working", SqlDbType.Bit))
            cmd.Parameters("@parm_Working").Value = Convert.ToByte(Working)
            strMethodParms &= "Working: " & Working.ToString & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_CreatedBy", SqlDbType.VarChar, 50))
            cmd.Parameters("@parm_CreatedBy").Value = GetAbbrevNameFromCurrentUser()

            ' Add the output parameters and values
            'cmd.Parameters.Add(New SqlParameter("@parm_Identity", SqlDbType.Int))
            cmd.Parameters.Add(New SqlParameter("@parm_Ident", SqlDbType.Int))
            cmd.Parameters("@parm_Ident").Direction = ParameterDirection.Output

            ' Add the return parameter
            cmd.Parameters.Add(New SqlParameter("ReturnValue", SqlDbType.Int))
            cmd.Parameters("ReturnValue").Direction = ParameterDirection.ReturnValue

            ' Execute the stored procedure,
            ' and return the result.
            cn.Open()
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            dr.Close()
            intIdent = Convert.ToInt32(cmd.Parameters("@parm_Ident").Value)
            intReturnValue = Convert.ToInt32(cmd.Parameters("ReturnValue").Value)

            If intReturnValue <> 0 Then
                Debug.WriteLine("Error encountered in SecurityModule.InsertTeamMember" & _
                        ControlChars.Cr & _
                        "Error Returned: " & intReturnValue)
                intIdent = -1
            End If

        Catch ex As Exception
            Debug.WriteLine("Error encountered in SecurityModule.InsertTeamMember" & _
                ControlChars.Cr & ex.Message)
            intIdent = -1
            Dim strMethodName As String = GetExecutingMethodName()
            SendExceptionEmailNotification("SecurityModule", strMethodName, ex, strMethodParms)

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        Finally
            cn.Close()
            cmd.Dispose()
        End Try
        Return intIdent
    End Function ' InsertTeamMember

    ''' <summary>
    ''' Updates the specified record in the TeamMember_Maint table
    ''' </summary>
    ''' <param name="TeamMemberId">Primary key of the record to update</param>
    ''' <param name="UserName">New value of UserName</param>
    ''' <param name="ShortName">New value of ShortName</param>
    ''' <param name="LastName">New value of LastName</param>
    ''' <param name="FirstName">New value of FirstName</param>
    ''' <param name="Email">New value of Email</param>
    ''' <param name="Working">New value of Working</param>
    ''' <returns>True if success, otherwise False</returns>
    ''' <remarks>
    ''' </remarks>
    Public Shared Function UpdateTeamMember(ByVal TeamMemberId As Integer, _
            ByVal UserName As String, _
            ByVal ShortName As String, _
            ByVal LastName As String, _
            ByVal FirstName As String, _
            ByVal Email As String, _
            ByVal Working As Boolean) As Boolean

        Dim cn As SqlConnection = New SqlConnection
        Dim cmd As SqlCommand = New SqlCommand
        Dim intRowsAffected As Integer
        Dim intReturnValue As Integer
        Dim blnSuccess As Boolean = False
        Dim strMethodParms As String = PARAMETER_LIST_HEADING

        Try
            ' Create the command to execute a stored procedure
            cn.ConnectionString = _
                System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString()
            cmd.Connection = cn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_Update_TeamMember"

            ' Add the input parameters and values

            cmd.Parameters.Add(New SqlParameter("@parm_TeamMemberID", SqlDbType.Int))
            cmd.Parameters("@parm_TeamMemberID").Value = TeamMemberId
            strMethodParms &= "TeamMemberID: " & TeamMemberId.ToString & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_UserName", SqlDbType.VarChar, 50))
            cmd.Parameters("@parm_UserName").Value = UserName
            strMethodParms &= "UserName: " & UserName & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_ShortName", SqlDbType.VarChar, 40))
            cmd.Parameters("@parm_ShortName").Value = ShortName
            strMethodParms &= "ShortName: " & ShortName & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_LastName", SqlDbType.VarChar, 30))
            cmd.Parameters("@parm_LastName").Value = LastName
            strMethodParms &= "LastName: " & LastName & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_FirstName", SqlDbType.VarChar, 30))
            cmd.Parameters("@parm_FirstName").Value = FirstName
            strMethodParms &= "FirstName: " & FirstName & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_Email", SqlDbType.VarChar, 50))
            cmd.Parameters("@parm_Email").Value = Email
            strMethodParms &= "Email: " & Email & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_Working", SqlDbType.Bit))
            cmd.Parameters("@parm_Working").Value = Working
            strMethodParms &= "Working: " & Working.ToString & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_UpdatedBy", SqlDbType.VarChar, 50))
            cmd.Parameters("@parm_UpdatedBy").Value = GetAbbrevNameFromCurrentUser()

            ' Add the output parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_RowsAffected", SqlDbType.Int))
            cmd.Parameters("@parm_RowsAffected").Direction = ParameterDirection.Output

            ' Add the return parameter
            cmd.Parameters.Add(New SqlParameter("ReturnValue", SqlDbType.Int))
            cmd.Parameters("ReturnValue").Direction = ParameterDirection.ReturnValue

            ' Execute the stored procedure,
            ' and return the result.
            cn.Open()
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            dr.Close()
            intRowsAffected = Convert.ToInt32(cmd.Parameters("@parm_RowsAffected").Value)
            intReturnValue = Convert.ToInt32(cmd.Parameters("ReturnValue").Value)

            If (intReturnValue <> 0) Or _
               (intRowsAffected <> 1) Then
                Debug.WriteLine("Error encountered SecurityModule.UpdateTeamMember" & _
                       ControlChars.Cr & _
                       "Error Returned: " & intReturnValue & _
                       ", Rows Affected: " & intRowsAffected)
            Else
                blnSuccess = True
            End If
        Catch ex As Exception
            Debug.WriteLine("Error encountered in SecurityModule.UpdateTeamMember" & _
                ControlChars.Cr & ex.Message)
            Dim strMethodName As String = GetExecutingMethodName()
            SendExceptionEmailNotification("SecurityModule", strMethodName, ex, strMethodParms)

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        Finally
            cn.Close()
            cmd.Dispose()
        End Try
        Return blnSuccess
    End Function ' UpdateTeamMember

#End Region ' TeamMember_Maint Functions


#Region "TeamMember_RoleForm Functions"

    ''' <summary>
    ''' Copys a TeamMember_RoleForm records from one Team Member to another.
    ''' </summary>
    ''' <param name="FromTeamMemberID">Specifies the "Copy From" team member</param>
    ''' <param name="ToTeamMemberID">Specifies the "Copy To" team member</param>
    ''' <param name="ResultMessage">Returns result message</param>
    ''' <param name="RecordsCopiedCount">Returns count of records copied</param>
    ''' <returns>True if success, otherwise False</returns>
    ''' <remarks>The "Copy To" Team Member's RoleForm records are replaced.</remarks>
    Public Shared Function CopyTMRoleForm(ByVal FromTeamMemberID As Integer, _
        ByVal ToTeamMemberID As Integer, ByRef ResultMessage As String, _
        ByRef RecordsCopiedCount As Integer) As Boolean

        Dim cn As SqlConnection = New SqlConnection
        Dim cmd As SqlCommand = New SqlCommand
        Dim intRowsAffected As Integer
        Dim intReturnValue As Integer
        Dim blnSuccess As Boolean = False
        Dim strMessage As String = ""
        Dim strMethodParms As String = PARAMETER_LIST_HEADING

        Try
            ' Create the command to execute a stored procedure
            ' Gets ConnectionString from web.config
            cn.ConnectionString = _
                System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString()
            cn.Open()
            cmd.Connection = cn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_Copy_TeamMember_RoleForm"

            ' Add the input parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_ToID", SqlDbType.Int))
            cmd.Parameters("@parm_ToID").Value = ToTeamMemberID
            strMethodParms &= "ToTeamMemberID: " & ToTeamMemberID.ToString & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_FromID", SqlDbType.Int))
            cmd.Parameters("@parm_FromID").Value = FromTeamMemberID
            strMethodParms &= "FromTeamMemberID: " & FromTeamMemberID.ToString & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_CreatedBy", SqlDbType.VarChar, 50))
            cmd.Parameters("@parm_CreatedBy").Value = GetAbbrevNameFromCurrentUser()

            ' Add the output parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_RowsCopied", SqlDbType.Int))
            cmd.Parameters("@parm_RowsCopied").Direction = ParameterDirection.Output

            cmd.Parameters.Add(New SqlParameter("@parm_Message", SqlDbType.VarChar, 255))
            cmd.Parameters("@parm_Message").Direction = ParameterDirection.Output

            ' Add the return parameter
            cmd.Parameters.Add(New SqlParameter("ReturnValue", SqlDbType.Int))
            cmd.Parameters("ReturnValue").Direction = ParameterDirection.ReturnValue

            ' Execute the stored procedure,
            ' and return the result.
            Dim intResult As Integer = cmd.ExecuteNonQuery()

            intRowsAffected = Convert.ToInt32(cmd.Parameters("@parm_RowsCopied").Value)
            intReturnValue = Convert.ToInt32(cmd.Parameters("ReturnValue").Value)
            ResultMessage = cmd.Parameters("@parm_message").Value.ToString()

            If (intReturnValue <> 0) Or _
               (intRowsAffected < 1) Then
                Debug.WriteLine("Error encountered SecurityModule.DeleteTMRoleForm" & _
                       ControlChars.Cr & _
                       "Error Returned: " & intReturnValue & _
                       ", Rows Affected: " & intRowsAffected)
            Else
                blnSuccess = True
            End If

        Catch ex As Exception
            Debug.WriteLine("Error encountered in SecurityModule.CopyTMRoleForm" & _
                ControlChars.Cr & ex.Message)
            Dim strMethodName As String = GetExecutingMethodName()
            SendExceptionEmailNotification("SecurityModule", strMethodName, ex, strMethodParms)
            ResultMessage = "Error has occurred."

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        Finally
            cn.Close()
            cmd.Dispose()
        End Try
        Return blnSuccess
    End Function ' CopyTMRoleForm

    ''' <summary>
    ''' Deletes the specified record from the TeamMember_RoleForm table
    ''' </summary>
    ''' <param name="TeamMemberId">The TeamMemberID of the record to delete</param>
    ''' <param name="RoleID">The RoleID of the record to delete</param>
    ''' <param name="FormID">The FormID of the record to delete</param>
    ''' <returns>True if success, otherwise False</returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteTMRoleForm(ByVal TeamMemberId As Integer, _
        ByVal RoleID As Integer, ByVal FormID As Integer) As Boolean

        Dim cn As SqlConnection = New SqlConnection
        Dim cmd As SqlCommand = New SqlCommand
        Dim intRowsAffected As Integer
        Dim intReturnValue As Integer
        Dim blnSuccess As Boolean = False
        Dim strMethodParms As String = PARAMETER_LIST_HEADING

        Try
            ' Create the command to execute a stored procedure
            ' Gets ConnectionString from web.config
            cn.ConnectionString = _
                System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString()
            cn.Open()
            cmd.Connection = cn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_Delete_TeamMember_RoleForm"

            ' Add the input parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_TeamMemberID", SqlDbType.Int))
            cmd.Parameters("@parm_TeamMemberID").Value = TeamMemberId
            strMethodParms &= "TeamMemberID: " & TeamMemberId.ToString & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_RoleID", SqlDbType.Int))
            cmd.Parameters("@parm_RoleID").Value = RoleID
            strMethodParms &= "RoleID: " & RoleID.ToString & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_FormID", SqlDbType.Int))
            cmd.Parameters("@parm_FormID").Value = FormID
            strMethodParms &= "FormID: " & FormID.ToString & Environment.NewLine

            ' Add the output parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_RowsAffected", SqlDbType.Int))
            cmd.Parameters("@parm_RowsAffected").Direction = ParameterDirection.Output

            ' Add the return parameter
            cmd.Parameters.Add(New SqlParameter("ReturnValue", SqlDbType.Int))
            cmd.Parameters("ReturnValue").Direction = ParameterDirection.ReturnValue

            ' Execute the stored procedure,
            ' and return the result.
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            dr.Close()
            intRowsAffected = Convert.ToInt32(cmd.Parameters("@parm_RowsAffected").Value)
            intReturnValue = Convert.ToInt32(cmd.Parameters("ReturnValue").Value)

            If (intReturnValue <> 0) Or _
               (intRowsAffected <> 1) Then
                Debug.WriteLine("Error encountered SecurityModule.DeleteTMRoleForm" & _
                       ControlChars.Cr & _
                       "Error Returned: " & intReturnValue & _
                       ", Rows Affected: " & intRowsAffected)
            Else
                blnSuccess = True
            End If

        Catch ex As Exception
            Debug.WriteLine("Error encountered in SecurityModule.DeleteTMRoleForm" & _
                ControlChars.Cr & ex.Message)
            Dim strMethodName As String = GetExecutingMethodName()
            SendExceptionEmailNotification("SecurityModule", strMethodName, ex, strMethodParms)

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        Finally
            cn.Close()
            cmd.Dispose()
        End Try
        Return blnSuccess
    End Function ' DeleteTMRoleForm

    ''' <summary>
    ''' Gets a DataSet of TeamMember_RoleForm records
    ''' </summary>
    ''' <param name="TeamMemberID">TeamMemberID of records to get</param>
    ''' <param name="RoleID" >Optional RoleId of records to get</param>
    ''' <param name="FormID">Optional FormId of records to get</param>
    ''' <returns>DataSet</returns>
    ''' <remarks>The RoleName and FormName are included in the DataSet</remarks>
    Public Shared Function GetTMRoleForm(ByVal TeamMemberID As Integer, _
        ByVal RoleID As Nullable(Of Integer), ByVal FormID As Nullable(Of Integer)) As DataSet

        Dim cn As SqlConnection = New SqlConnection
        Dim cmd As SqlCommand = New SqlCommand
        Dim ds As New DataSet
        Dim intRowsAffected As Integer
        Dim intReturnValue As Integer
        Dim blnSuccess As Boolean = False
        Dim strMethodParms As String = PARAMETER_LIST_HEADING

        Try
            ' Create the command to execute a stored procedure
            cn.ConnectionString = _
                System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString()
            cmd.Connection = cn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_Get_TeamMember_RoleForm"

            ' Add the input parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_TeamMemberID", SqlDbType.Int))
            cmd.Parameters("@parm_TeamMemberID").Value = TeamMemberID
            strMethodParms &= "TeamMemberID" & TeamMemberID.ToString & Environment.NewLine

            If RoleID.HasValue Then
                cmd.Parameters.Add(New SqlParameter("@parm_RoleID", SqlDbType.Int))
                cmd.Parameters("@parm_RoleID").Value = RoleID
                strMethodParms &= "RoleID: " & RoleID.ToString & Environment.NewLine
            Else
                strMethodParms &= "RoleID: Nothing" & Environment.NewLine
            End If

            If FormID.HasValue Then
                cmd.Parameters.Add(New SqlParameter("@parm_FormID", SqlDbType.Int))
                cmd.Parameters("@parm_FormID").Value = FormID
                strMethodParms &= "FormID: " & FormID.ToString & Environment.NewLine
            Else
                strMethodParms &= "RoleID: Nothing" & Environment.NewLine
            End If

            ' Add the output parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_RowsAffected", SqlDbType.Int))
            cmd.Parameters("@parm_RowsAffected").Direction = ParameterDirection.Output

            ' Add the return parameter
            cmd.Parameters.Add(New SqlParameter("ReturnValue", SqlDbType.Int))
            cmd.Parameters("ReturnValue").Direction = ParameterDirection.ReturnValue

            ' Execute the stored procedure,
            ' and return the result.
            cn.Open()
            Dim myAdapter As New SqlDataAdapter(cmd)
            myAdapter.Fill(ds)

            intRowsAffected = Convert.ToInt32(cmd.Parameters("@parm_RowsAffected").Value)
            intReturnValue = Convert.ToInt32(cmd.Parameters("ReturnValue").Value)

            If (intReturnValue <> 0) Then
                Debug.WriteLine("Error encountered SecurityModule.GetTMRoleForm" & _
                       ControlChars.Cr & _
                       "Error Returned: " & intReturnValue & _
                       ", Rows Affected: " & intRowsAffected)
            Else
                blnSuccess = True
            End If
        Catch ex As Exception
            Debug.WriteLine("Error encountered in SecurityModule.GetTMRoleForm" & _
                ControlChars.Cr & ex.Message)
            Dim strMethodName As String = GetExecutingMethodName()
            SendExceptionEmailNotification("SecurityModule", strMethodName, ex, strMethodParms)

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        Finally
            cn.Close()
            cmd.Dispose()
        End Try

        Return ds
    End Function ' GetTMRoleForm

    ''' <summary>
    ''' Gets a count of the specified TeamMember_RoleForm records
    ''' </summary>
    ''' <param name="TeamMemberID">TeamMemberID of counted records</param>
    ''' <param name="RoleID" >Optional RoleId of of counted records</param>
    ''' <param name="FormID">Optional FormId of counted records</param>
    ''' <returns>Number of records matching the parameters</returns>
    ''' <remarks>Returns -1 if error was encountered</remarks>
    Public Shared Function GetTMRoleFormCount(ByVal TeamMemberID As Integer, _
        ByVal RoleID As Nullable(Of Integer), ByVal FormID As Nullable(Of Integer)) As Integer

        Dim cn As SqlConnection = New SqlConnection
        Dim cmd As SqlCommand = New SqlCommand
        Dim ds As New DataSet
        Dim intRowsAffected As Integer
        Dim intReturnValue As Integer = -1
        Dim strMethodParms As String = PARAMETER_LIST_HEADING

        Try
            ' Create the command to execute a stored procedure
            cn.ConnectionString = _
                System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString()
            cmd.Connection = cn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_Get_TeamMember_RoleForm"

            ' Add the input parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_TeamMemberID", SqlDbType.Int))
            cmd.Parameters("@parm_TeamMemberID").Value = TeamMemberID
            strMethodParms &= "TeamMemberID: " & TeamMemberID.ToString & Environment.NewLine

            If RoleID.HasValue Then
                cmd.Parameters.Add(New SqlParameter("@parm_RoleID", SqlDbType.Int))
                cmd.Parameters("@parm_RoleID").Value = RoleID
                strMethodParms &= "RoleID: " & RoleID.ToString & Environment.NewLine
            Else
                strMethodParms &= "RoleID: Nothing" & Environment.NewLine
            End If

            If FormID.HasValue Then
                cmd.Parameters.Add(New SqlParameter("@parm_FormID", SqlDbType.Int))
                cmd.Parameters("@parm_FormID").Value = FormID
                strMethodParms &= "FormID: " & FormID.ToString & Environment.NewLine
            Else
                strMethodParms &= "FormID: Nothing" & Environment.NewLine
            End If

            ' Add the output parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_RowsAffected", SqlDbType.Int))
            cmd.Parameters("@parm_RowsAffected").Direction = ParameterDirection.Output

            ' Add the return parameter
            cmd.Parameters.Add(New SqlParameter("ReturnValue", SqlDbType.Int))
            cmd.Parameters("ReturnValue").Direction = ParameterDirection.ReturnValue

            ' Execute the stored procedure,
            ' and return the result.
            cn.Open()
            Dim myAdapter As New SqlDataAdapter(cmd)
            myAdapter.Fill(ds)

            intRowsAffected = Convert.ToInt32(cmd.Parameters("@parm_RowsAffected").Value)
            intReturnValue = Convert.ToInt32(cmd.Parameters("ReturnValue").Value)

            If (intReturnValue <> 0) Then
                Debug.WriteLine("Error encountered SecurityModule.GetTMRoleFormCount" & _
                       ControlChars.Cr & _
                       "Error Returned: " & intReturnValue & _
                       ", Rows Affected: " & intRowsAffected)
                intRowsAffected = -1
            End If
        Catch ex As Exception
            Debug.WriteLine("Error encountered in SecurityModule.GetTMRoleFormCount" & _
                ControlChars.Cr & ex.Message)
            Dim strMethodName As String = GetExecutingMethodName()
            SendExceptionEmailNotification("SecurityModule", strMethodName, ex, strMethodParms)
            intRowsAffected = -1

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        Finally
            cn.Close()
            cmd.Dispose()
        End Try

        Return intRowsAffected
    End Function ' GetTMRoleFormCount

    ''' <summary>
    ''' Gets a DataSet of TeamMember Role/Form security data
    ''' </summary>
    ''' <param name="TeamMemberID">Optional TeamMemberID of records to get</param>
    ''' <param name="UserName">Optional UserName of records to get</param>
    ''' <param name="RoleID" >Optional RoleId of records to get</param>
    ''' <param name="RoleName">Optional RoleName of records to get</param>
    ''' <param name="FormID">Optional FormId of records to get</param>
    ''' <param name="FormName">Optional FormName of records to get</param>
    ''' <param name="HyperlinkID">Optional HyperlinkID of records to get</param>
    ''' <returns>DataSet</returns>
    ''' <remarks>
    ''' These fields are returned in the DataSet: <br />
    ''' TeamMemberID, Username, RoleID, RoleName, FormID, FormName, HyperlinkID.
    ''' </remarks>
    Public Shared Function GetTMSecurity( _
        ByVal TeamMemberID As Nullable(Of Integer), _
        ByVal UserName As String, _
        ByVal RoleID As Nullable(Of Integer), _
        ByVal RoleName As String, _
        ByVal FormID As Nullable(Of Integer), _
        ByVal FormName As String, _
        ByVal HyperlinkID As String) As DataSet

        Dim cn As SqlConnection = New SqlConnection
        Dim cmd As SqlCommand = New SqlCommand
        Dim ds As New DataSet
        Dim intRowsAffected As Integer
        Dim intReturnValue As Integer
        Dim blnSuccess As Boolean = False
        Dim strMethodParms As String = PARAMETER_LIST_HEADING

        Try
            ' Create the command to execute a stored procedure
            cn.ConnectionString = _
                System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString()
            cmd.Connection = cn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_Get_TeamMember_Security"

            ' Add the input parameters and values
            If (TeamMemberID.HasValue = True) Then
                cmd.Parameters.Add(New SqlParameter("@parm_TeamMemberID", SqlDbType.Int))
                cmd.Parameters("@parm_TeamMemberID").Value = TeamMemberID
                strMethodParms &= "TeamMemberID: " & TeamMemberID.ToString & Environment.NewLine
            Else
                strMethodParms &= "TeamMemberID: Nothing" & Environment.NewLine
            End If

            If ((UserName IsNot Nothing) AndAlso _
               (UserName <> "")) Then
                cmd.Parameters.Add(New SqlParameter("@parm_UserName", SqlDbType.VarChar, 50))
                cmd.Parameters("@parm_UserName").Value = UserName
                strMethodParms &= "UserName: " & UserName & Environment.NewLine
            Else
                strMethodParms &= "UserName: Nothing" & Environment.NewLine
            End If

            If (RoleID.HasValue = True) Then
                cmd.Parameters.Add(New SqlParameter("@parm_RoleID", SqlDbType.Int))
                cmd.Parameters("@parm_RoleID").Value = RoleID
                strMethodParms &= "RoleID: " & RoleID.ToString & Environment.NewLine
            Else
                strMethodParms &= "RoleID: Nothing" & Environment.NewLine
            End If

            If ((RoleName IsNot Nothing) AndAlso _
               (RoleName <> "")) Then
                cmd.Parameters.Add(New SqlParameter("@parm_RoleName", SqlDbType.VarChar, 30))
                cmd.Parameters("@parm_RoleName").Value = RoleName
                strMethodParms &= "RoleName: " & RoleName.ToString & Environment.NewLine
            Else
                strMethodParms &= "RoleName: Nothing" & Environment.NewLine
            End If

            If FormID.HasValue Then
                cmd.Parameters.Add(New SqlParameter("@parm_FormID", SqlDbType.Int))
                cmd.Parameters("@parm_FormID").Value = FormID
                strMethodParms &= "FormID: " & FormID.ToString & Environment.NewLine
            Else
                strMethodParms &= "RoleID: Nothing" & Environment.NewLine
            End If

            If ((FormName IsNot Nothing) AndAlso _
               (FormName <> "")) Then
                cmd.Parameters.Add(New SqlParameter("@parm_FormName", SqlDbType.VarChar, 30))
                cmd.Parameters("@parm_FormName").Value = FormName
                strMethodParms &= "FormName: " & FormName.ToString & Environment.NewLine
            Else
                strMethodParms &= "FormName: Nothing" & Environment.NewLine
            End If

            If ((HyperlinkID IsNot Nothing) AndAlso _
               (HyperlinkID <> "")) Then
                cmd.Parameters.Add(New SqlParameter("@parm_HyperlinkID", SqlDbType.VarChar, 70))
                cmd.Parameters("@parm_HyperlinkID").Value = HyperlinkID
                strMethodParms &= "HyperlinkID: " & HyperlinkID.ToString & Environment.NewLine
            Else
                strMethodParms &= "HyperlinkID: Nothing" & Environment.NewLine
            End If

            ' Add the output parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_RowsAffected", SqlDbType.Int))
            cmd.Parameters("@parm_RowsAffected").Direction = ParameterDirection.Output

            ' Add the return parameter
            cmd.Parameters.Add(New SqlParameter("ReturnValue", SqlDbType.Int))
            cmd.Parameters("ReturnValue").Direction = ParameterDirection.ReturnValue

            ' Execute the stored procedure,
            ' and return the result.
            cn.Open()
            Dim myAdapter As New SqlDataAdapter(cmd)
            myAdapter.Fill(ds)

            intRowsAffected = Convert.ToInt32(cmd.Parameters("@parm_RowsAffected").Value)
            intReturnValue = Convert.ToInt32(cmd.Parameters("ReturnValue").Value)

            If (intReturnValue <> 0) Then
                Throw New Exception("Database Error, Error Code = " & intReturnValue)
            Else
                blnSuccess = True
            End If
        Catch ex As Exception
            Dim strMethodName As String = GetExecutingMethodName()
            SendExceptionEmailNotification("SecurityModule", strMethodName, ex, strMethodParms)

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        Finally
            cn.Close()
            cmd.Dispose()
        End Try

        Return ds
    End Function ' GetTMSecurity

    ''' <summary>
    ''' Inserts a new record into the TeamMember_RoleForm table
    ''' </summary>
    ''' <param name="TeamMemberID">The TeamMemberID of the new record</param>
    ''' <param name="RoleID">The RoleID of the new record</param>
    ''' <param name="FormID">The FormID of the new record</param>
    ''' <returns>True if success, otherwise False</returns>
    ''' <remarks></remarks>
    Public Shared Function InsertTMRoleForm(ByVal TeamMemberID As Integer, _
        ByVal RoleID As Integer, ByVal FormID As Integer) As Boolean

        Dim cn As SqlConnection = New SqlConnection
        Dim cmd As SqlCommand = New SqlCommand
        Dim intRowsAffected As Integer
        Dim intReturnValue As Integer
        Dim blnSuccess As Boolean = False
        Dim strMethodParms As String = PARAMETER_LIST_HEADING

        Try
            ' Create the command to execute a stored procedure
            cn.ConnectionString = _
                System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString()
            cmd.Connection = cn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_Insert_TeamMember_RoleForm"

            ' Add the input parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_TeamMemberID", SqlDbType.Int))
            cmd.Parameters("@parm_TeamMemberID").Value = TeamMemberID
            strMethodParms &= "TeamMemberID: " & TeamMemberID.ToString & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_RoleID", SqlDbType.Int))
            cmd.Parameters("@parm_RoleID").Value = RoleID
            strMethodParms &= "RoleID: " & RoleID.ToString & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_FormID", SqlDbType.Int))
            cmd.Parameters("@parm_FormID").Value = FormID
            strMethodParms &= "FormID: " & FormID.ToString & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_CreatedBy", SqlDbType.VarChar, 50))
            cmd.Parameters("@parm_CreatedBy").Value = GetAbbrevNameFromCurrentUser()

            ' Add the output parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_RowsAffected", SqlDbType.Int))
            cmd.Parameters("@parm_RowsAffected").Direction = ParameterDirection.Output

            ' Add the return parameter
            cmd.Parameters.Add(New SqlParameter("ReturnValue", SqlDbType.Int))
            cmd.Parameters("ReturnValue").Direction = ParameterDirection.ReturnValue

            ' Execute the stored procedure,
            ' and return the result.
            cn.Open()
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            dr.Close()
            intRowsAffected = Convert.ToInt32(cmd.Parameters("@parm_RowsAffected").Value)
            intReturnValue = Convert.ToInt32(cmd.Parameters("ReturnValue").Value)

            If (intReturnValue <> 0) Or _
               (intRowsAffected <> 1) Then
                Debug.WriteLine("Error encountered SecurityModule.InsertTMRoleForm" & _
                       ControlChars.Cr & _
                       "Error Returned: " & intReturnValue & _
                       ", Rows Affected: " & intRowsAffected)
            Else
                blnSuccess = True
            End If
        Catch ex As Exception
            Debug.WriteLine("Error encountered in SecurityModule.InsertTMRoleForm" & _
                ControlChars.Cr & ex.Message)
            Dim strMethodName As String = GetExecutingMethodName()
            SendExceptionEmailNotification("SecurityModule", strMethodName, ex, strMethodParms)

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        Finally
            cn.Close()
            cmd.Dispose()
        End Try
        Return blnSuccess
    End Function ' InsertTMRoleForm

    ''' <summary>
    ''' Updates the RoleID of the TeamMember_RoleForm table.
    ''' </summary>
    ''' <param name="TeamMemberId">Original key of record to update.</param>
    ''' <param name="FormID">Original key of record to update.</param>
    ''' <param name="RoleIDOld">Original key of record to update.</param>
    ''' <param name="RoleIDNew">New RoleID</param>
    ''' <returns>True if success; otherwise false.</returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateTMRoleForm(ByVal TeamMemberId As Integer, _
            ByVal FormID As Integer, _
            ByVal RoleIDOld As Integer, _
            ByVal RoleIDNew As Integer) As Boolean

        Dim cn As SqlConnection = New SqlConnection
        Dim cmd As SqlCommand = New SqlCommand
        Dim intRowsAffected As Integer
        Dim intReturnValue As Integer
        Dim blnSuccess As Boolean = False
        Dim strMethodParms As String = PARAMETER_LIST_HEADING

        Try
            ' Create the command to execute a stored procedure
            cn.ConnectionString = _
                System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString()
            cmd.Connection = cn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_Update_TeamMember_RoleForm"

            ' Add the input parameters and values

            cmd.Parameters.Add(New SqlParameter("@parm_TeamMemberID", SqlDbType.Int))
            cmd.Parameters("@parm_TeamMemberID").Value = TeamMemberId
            strMethodParms &= "TeamMemberID: " & TeamMemberId.ToString & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_FormID", SqlDbType.Int))
            cmd.Parameters("@parm_FormID").Value = FormID
            strMethodParms &= "FormID: " & TeamMemberId.ToString & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_RoleIDOld", SqlDbType.Int))
            cmd.Parameters("@parm_RoleIDOld").Value = RoleIDOld
            strMethodParms &= "RoleIDOld: " & TeamMemberId.ToString & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_RoleIDNew", SqlDbType.Int))
            cmd.Parameters("@parm_RoleIDNew").Value = RoleIDNew
            strMethodParms &= "RoleIDNew: " & TeamMemberId.ToString & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_UpdatedBy", SqlDbType.VarChar, 50))
            cmd.Parameters("@parm_UpdatedBy").Value = GetAbbrevNameFromCurrentUser()

            ' Add the output parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_RowsAffected", SqlDbType.Int))
            cmd.Parameters("@parm_RowsAffected").Direction = ParameterDirection.Output

            ' Add the return parameter
            cmd.Parameters.Add(New SqlParameter("ReturnValue", SqlDbType.Int))
            cmd.Parameters("ReturnValue").Direction = ParameterDirection.ReturnValue

            ' Execute the stored procedure,
            ' and return the result.
            cn.Open()
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            dr.Close()
            intRowsAffected = Convert.ToInt32(cmd.Parameters("@parm_RowsAffected").Value)
            intReturnValue = Convert.ToInt32(cmd.Parameters("ReturnValue").Value)

            If (intReturnValue <> 0) Or _
               (intRowsAffected <> 1) Then
                Debug.WriteLine("Error encountered SecurityModule.UpdateTMRoleForm" & _
                       ControlChars.Cr & _
                       "Error Returned: " & intReturnValue & _
                       ", Rows Affected: " & intRowsAffected)
            Else
                blnSuccess = True
            End If
        Catch ex As Exception
            Debug.WriteLine("Error encountered in SecurityModule.UpdateTMRoleForm" & _
                ControlChars.Cr & ex.Message)
            Dim strMethodName As String = GetExecutingMethodName()
            SendExceptionEmailNotification("SecurityModule", strMethodName, ex, strMethodParms)

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        Finally
            cn.Close()
            cmd.Dispose()
        End Try
        Return blnSuccess
    End Function ' UpdateTMRoleForm

#End Region ' TeamMember_RoleForm Functions


#Region "TeamMember_WorkHistory Functions"

    ''' <summary>
    ''' Deletes the specified record from the TeamMember_WorkHistory table
    ''' </summary>
    ''' <param name="TeamMemberId">The TeamMemberID of the record to delete</param>
    ''' <param name="SubscriptionID">The SubscriptionID of the record to delete</param>
    ''' <returns>True if success, otherwise False</returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteTMWorkHistory(ByVal TeamMemberId As Integer, _
        ByVal SubscriptionID As Integer, ByVal UGNFacility As String) As Boolean

        Dim cn As SqlConnection = New SqlConnection
        Dim cmd As SqlCommand = New SqlCommand
        Dim intRowsAffected As Integer
        Dim intReturnValue As Integer
        Dim blnSuccess As Boolean = False
        Dim strMethodParms As String = PARAMETER_LIST_HEADING

        Try
            ' Create the command to execute a stored procedure
            ' Gets ConnectionString from web.config
            cn.ConnectionString = _
                System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString()
            cn.Open()
            cmd.Connection = cn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_Delete_TeamMember_WorkHistory"

            ' Add the input parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_TeamMemberID", SqlDbType.Int))
            cmd.Parameters("@parm_TeamMemberID").Value = TeamMemberId
            strMethodParms &= "TeamMemberID: " & TeamMemberId.ToString & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_SubscriptionID", SqlDbType.Int))
            cmd.Parameters("@parm_SubscriptionID").Value = SubscriptionID
            strMethodParms &= "SubscriptionID: " & SubscriptionID.ToString() & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_UGNFacility", SqlDbType.VarChar))
            cmd.Parameters("@parm_UGNFacility").Value = UGNFacility
            strMethodParms &= "UGNFacility: " & UGNFacility.ToString() & Environment.NewLine


            ' Add the output parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_RowsAffected", SqlDbType.Int))
            cmd.Parameters("@parm_RowsAffected").Direction = ParameterDirection.Output

            ' Add the return parameter
            cmd.Parameters.Add(New SqlParameter("ReturnValue", SqlDbType.Int))
            cmd.Parameters("ReturnValue").Direction = ParameterDirection.ReturnValue

            ' Execute the stored procedure,
            ' and return the result.
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            dr.Close()
            intRowsAffected = Convert.ToInt32(cmd.Parameters("@parm_RowsAffected").Value)
            intReturnValue = Convert.ToInt32(cmd.Parameters("ReturnValue").Value)

            If (intReturnValue <> 0) Or _
               (intRowsAffected <> 1) Then
                Debug.WriteLine("Error encountered SecurityModule.DeleteTMWorkHistory" & _
                       ControlChars.Cr & _
                       "Error Returned: " & intReturnValue & _
                       ", Rows Affected: " & intRowsAffected)
            Else
                blnSuccess = True
            End If

        Catch ex As Exception
            Debug.WriteLine("Error encountered in SecurityModule.DeleteTMWorkHistory" & _
                ControlChars.Cr & ex.Message)
            Dim strMethodName As String = GetExecutingMethodName()
            SendExceptionEmailNotification("SecurityModule", strMethodName, ex, strMethodParms)

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        Finally
            cn.Close()
            cmd.Dispose()
        End Try
        Return blnSuccess
    End Function ' DeleteTMWorkHistory

    ''' <summary>
    ''' Gets a DataSet of TeamMember_WorkHistory records
    ''' </summary>
    ''' <param name="TeamMemberID">TeamMemberID of records to return</param>
    ''' <param name="SubscriptionID">SubscriptionID of records to return (Optional parameter)</param>
    ''' <returns>DataSet</returns>
    ''' <remarks>The UGNFacilityName is included in the DataSet</remarks>
    Public Shared Function GetTMWorkHistory(ByVal TeamMemberID As Integer, _
        ByVal SubscriptionID As Nullable(Of Integer)) As DataSet

        Dim cn As SqlConnection = New SqlConnection
        Dim cmd As SqlCommand = New SqlCommand
        Dim ds As New DataSet
        Dim intRowsAffected As Integer
        Dim intReturnValue As Integer
        Dim blnSuccess As Boolean = False
        Dim strMethodParms As String = PARAMETER_LIST_HEADING

        Try
            ' Create the command to execute a stored procedure
            cn.ConnectionString = _
                System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString()
            cmd.Connection = cn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_Get_TeamMember_WorkHistory"

            ' Add the input parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_TeamMemberID", SqlDbType.Int))
            cmd.Parameters("@parm_TeamMemberID").Value = TeamMemberID
            strMethodParms &= "TeamMemberID: " & TeamMemberID.ToString & Environment.NewLine

            If SubscriptionID.HasValue Then
                cmd.Parameters.Add(New SqlParameter("@parm_SubscriptionID", SqlDbType.Int))
                cmd.Parameters("@parm_SubscriptionID").Value = SubscriptionID
                strMethodParms &= "SubscriptionID: " & SubscriptionID.Value.ToString() & _
                    Environment.NewLine
            Else
                strMethodParms &= "SubscriptionID: Nothing" & Environment.NewLine
            End If

            ' Add the output parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_RowsAffected", SqlDbType.Int))
            cmd.Parameters("@parm_RowsAffected").Direction = ParameterDirection.Output

            ' Add the return parameter
            cmd.Parameters.Add(New SqlParameter("ReturnValue", SqlDbType.Int))
            cmd.Parameters("ReturnValue").Direction = ParameterDirection.ReturnValue

            ' Execute the stored procedure,
            ' and return the result.
            cn.Open()
            Dim myAdapter As New SqlDataAdapter(cmd)
            myAdapter.Fill(ds)

            intRowsAffected = Convert.ToInt32(cmd.Parameters("@parm_RowsAffected").Value)
            intReturnValue = Convert.ToInt32(cmd.Parameters("ReturnValue").Value)

            If (intReturnValue <> 0) Then
                Debug.WriteLine("Error encountered SecurityModule.GetTMWorkHistory" & _
                       ControlChars.Cr & _
                       "Error Returned: " & intReturnValue & _
                       ", Rows Affected: " & intRowsAffected)
            Else
                blnSuccess = True
            End If
        Catch ex As Exception
            Debug.WriteLine("Error encountered in SecurityModule.GetTMWorkHistory" & _
                ControlChars.Cr & ex.Message)
            Dim strMethodName As String = GetExecutingMethodName()
            SendExceptionEmailNotification("SecurityModule", strMethodName, ex, strMethodParms)

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        Finally
            cn.Close()
            cmd.Dispose()
        End Try

        Return ds
    End Function ' GetTMWorkHistory

    ''' <summary>
    ''' Gets a count of the specified TeamMember_WorkHistory records
    ''' </summary>
    ''' <param name="TeamMemberID">TeamMemberID of counted records</param>
    ''' <param name="SubscriptionID">SubscriptionID of counted records (optional parameter)</param>
    ''' <returns>Number of records matching the parameters</returns>
    ''' <remarks>Returns -1 if error was encountered</remarks>
    Public Shared Function GetTMWorkHistoryCount(ByVal TeamMemberID As Integer, _
        ByVal SubscriptionID As Nullable(Of Integer)) As Integer

        Dim cn As SqlConnection = New SqlConnection
        Dim cmd As SqlCommand = New SqlCommand
        Dim ds As New DataSet
        Dim intRowsAffected As Integer
        Dim intReturnValue As Integer = -1
        Dim strMethodParms As String = PARAMETER_LIST_HEADING

        Try
            ' Create the command to execute a stored procedure
            cn.ConnectionString = _
                System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString()
            cmd.Connection = cn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_Get_TeamMember_WorkHistory"

            ' Add the input parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_TeamMemberID", SqlDbType.Int))
            cmd.Parameters("@parm_TeamMemberID").Value = TeamMemberID
            strMethodParms &= "TeamMemberID: " & TeamMemberID & Environment.NewLine

            If SubscriptionID.HasValue Then
                cmd.Parameters.Add(New SqlParameter("@parm_SubscriptionID", SqlDbType.Int))
                cmd.Parameters("@parm_SubscriptionID").Value = SubscriptionID
                strMethodParms &= "SubscriptionID: " & SubscriptionID.Value.ToString() & _
                    Environment.NewLine
            Else
                strMethodParms &= "SubscriptionID: Nothing" & Environment.NewLine
            End If

            ' Add the output parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_RowsAffected", SqlDbType.Int))
            cmd.Parameters("@parm_RowsAffected").Direction = ParameterDirection.Output

            ' Add the return parameter
            cmd.Parameters.Add(New SqlParameter("ReturnValue", SqlDbType.Int))
            cmd.Parameters("ReturnValue").Direction = ParameterDirection.ReturnValue

            ' Execute the stored procedure,
            ' and return the result.
            cn.Open()
            Dim myAdapter As New SqlDataAdapter(cmd)
            myAdapter.Fill(ds)

            intRowsAffected = Convert.ToInt32(cmd.Parameters("@parm_RowsAffected").Value)
            intReturnValue = Convert.ToInt32(cmd.Parameters("ReturnValue").Value)

            If (intReturnValue <> 0) Then
                Debug.WriteLine("Error encountered SecurityModule.GetTMWorkHistoryCount" & _
                       ControlChars.Cr & _
                       "Error Returned: " & intReturnValue & _
                       ", Rows Affected: " & intRowsAffected)
                intRowsAffected = -1
            End If
        Catch ex As Exception
            Debug.WriteLine("Error encountered in SecurityModule.GetTMWorkHistoryCount" & _
                ControlChars.Cr & ex.Message)
            Dim strMethodName As String = GetExecutingMethodName()
            SendExceptionEmailNotification("SecurityModule", strMethodName, ex, strMethodParms)
            intRowsAffected = -1

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        Finally
            cn.Close()
            cmd.Dispose()
        End Try

        Return intRowsAffected
    End Function ' GetTMWorkHistoryCount


    ''' <summary>
    ''' Inserts a new record into the TeamMember_WorkHistory table
    ''' </summary>
    ''' <param name="TeamMemberID">TeamMemberID of the new record</param>
    ''' <param name="SubscriptionID">SubscriptionID of the new record</param>
    ''' <param name="StartDate">StartDate of the new record</param>
    ''' <param name="UGNFacility">UGNFacility key of the new record</param>
    ''' <returns>True if success, otherwise False</returns>
    ''' <remarks></remarks>
    Public Shared Function InsertTMWorkHistory(ByVal TeamMemberID As Integer, _
            ByVal SubscriptionID As Integer, _
            ByVal StartDate As DateTime, _
            ByVal UGNFacility As String) As Boolean

        Dim cn As SqlConnection = New SqlConnection
        Dim cmd As SqlCommand = New SqlCommand
        Dim intRowsAffected As Integer
        Dim intReturnValue As Integer
        Dim blnSuccess As Boolean = False
        Dim strMethodParms As String = PARAMETER_LIST_HEADING

        Try
            ' Create the command to execute a stored procedure
            cn.ConnectionString = _
                System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString()
            cmd.Connection = cn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_Insert_TeamMember_WorkHistory"

            ' Add the input parameters and values

            cmd.Parameters.Add(New SqlParameter("@parm_TeamMemberID", SqlDbType.Int))
            cmd.Parameters("@parm_TeamMemberID").Value = TeamMemberID
            strMethodParms &= "TeamMemberID: " & TeamMemberID.ToString & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_SubscriptionID", SqlDbType.Int))
            cmd.Parameters("@parm_SubscriptionID").Value = SubscriptionID
            strMethodParms &= "SubscriptionID: " & SubscriptionID.ToString & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_StartDate", SqlDbType.DateTime))
            cmd.Parameters("@parm_StartDate").Value = StartDate
            strMethodParms &= "StartDate: " & StartDate.ToString("F") & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_UGNFacility", SqlDbType.VarChar, 2))
            cmd.Parameters("@parm_UGNFacility").Value = UGNFacility
            strMethodParms &= "UGNFacility: " & UGNFacility & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_CreatedBy", SqlDbType.VarChar, 50))
            cmd.Parameters("@parm_CreatedBy").Value = GetAbbrevNameFromCurrentUser()

            ' Add the output parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_RowsAffected", SqlDbType.Int))
            cmd.Parameters("@parm_RowsAffected").Direction = ParameterDirection.Output

            ' Add the return parameter
            cmd.Parameters.Add(New SqlParameter("ReturnValue", SqlDbType.Int))
            cmd.Parameters("ReturnValue").Direction = ParameterDirection.ReturnValue

            ' Execute the stored procedure,
            ' and return the result.
            cn.Open()
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            dr.Close()
            intRowsAffected = Convert.ToInt32(cmd.Parameters("@parm_RowsAffected").Value)
            intReturnValue = Convert.ToInt32(cmd.Parameters("ReturnValue").Value)

            If (intReturnValue <> 0) Or _
               (intRowsAffected <> 1) Then
                Debug.WriteLine("Error encountered SecurityModule.InsertTMRoleForm" & _
                       ControlChars.Cr & _
                       "Error Returned: " & intReturnValue & _
                       ", Rows Affected: " & intRowsAffected)
            Else
                blnSuccess = True
            End If
        Catch ex As Exception
            Debug.WriteLine("Error encountered in SecurityModule.InsertTMRoleForm" & _
                ControlChars.Cr & ex.Message)
            Dim strMethodName As String = GetExecutingMethodName()
            SendExceptionEmailNotification("SecurityModule", strMethodName, ex, strMethodParms)

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        Finally
            cn.Close()
            cmd.Dispose()
        End Try
        Return blnSuccess
    End Function ' InsertTMWorkHistory

    ''' <summary>
    ''' Updates the specified record in the TeamMember_WorkHistory table
    ''' </summary>
    ''' <param name="TeamMemberID">TeamMemberID of the record to update</param>
    ''' <param name="SubscriptionID">SubscriptionID of the record to update</param>
    ''' <param name="StartDate">New value of StartDate</param>
    ''' <param name="EndDate">New value of EndDate(Optional field)</param>
    ''' <param name="UGNFacility">New value of UGNFacility key</param>
    ''' <returns>True if success, otherwise False</returns>
    ''' <remarks>
    ''' Missing EndDate parameter sets EndDate in TeamMember_WorkHIstory to NULL.
    ''' </remarks>
    Public Shared Function UpdateTMWorkHistory(ByVal TeamMemberId As Integer, _
            ByVal SubscriptionID As Integer, _
            ByVal StartDate As DateTime, _
            ByVal EndDate As Nullable(Of DateTime), _
            ByVal UGNFacility As String) As Boolean

        Dim cn As SqlConnection = New SqlConnection
        Dim cmd As SqlCommand = New SqlCommand
        Dim intRowsAffected As Integer
        Dim intReturnValue As Integer
        Dim blnSuccess As Boolean = False
        Dim strMethodParms As String = PARAMETER_LIST_HEADING

        Try
            ' Create the command to execute a stored procedure
            cn.ConnectionString = _
                System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString()
            cmd.Connection = cn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_Update_TeamMember_WorkHistory"

            ' Add the input parameters and values

            cmd.Parameters.Add(New SqlParameter("@parm_TeamMemberID", SqlDbType.Int))
            cmd.Parameters("@parm_TeamMemberID").Value = TeamMemberId
            strMethodParms &= "TeamMemberID: " & TeamMemberId.ToString & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_SubscriptionID", SqlDbType.Int))
            cmd.Parameters("@parm_SubscriptionID").Value = SubscriptionID
            strMethodParms &= "SubscriptionID: " & SubscriptionID.ToString & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_StartDate", SqlDbType.DateTime))
            cmd.Parameters("@parm_StartDate").Value = StartDate
            strMethodParms &= "StartDate: " & StartDate.ToString("F") & Environment.NewLine

            If EndDate.HasValue() Then
                cmd.Parameters.Add(New SqlParameter("@parm_EndDate", SqlDbType.DateTime))
                cmd.Parameters("@parm_EndDate").Value = EndDate
                strMethodParms &= "EndDate: " & EndDate.Value.ToString("F") & Environment.NewLine
            Else
                strMethodParms &= "EndDate: Nothing" & Environment.NewLine
            End If

            cmd.Parameters.Add(New SqlParameter("@parm_UGNFacility", SqlDbType.VarChar, 2))
            cmd.Parameters("@parm_UGNFacility").Value = UGNFacility
            strMethodParms &= "UGNFacility: " & UGNFacility & Environment.NewLine

            cmd.Parameters.Add(New SqlParameter("@parm_UpdatedBy", SqlDbType.VarChar, 50))
            cmd.Parameters("@parm_UpdatedBy").Value = GetAbbrevNameFromCurrentUser()

            ' Add the output parameters and values
            cmd.Parameters.Add(New SqlParameter("@parm_RowsAffected", SqlDbType.Int))
            cmd.Parameters("@parm_RowsAffected").Direction = ParameterDirection.Output

            ' Add the return parameter
            cmd.Parameters.Add(New SqlParameter("ReturnValue", SqlDbType.Int))
            cmd.Parameters("ReturnValue").Direction = ParameterDirection.ReturnValue

            ' Execute the stored procedure,
            ' and return the result.
            cn.Open()
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            dr.Close()
            intRowsAffected = Convert.ToInt32(cmd.Parameters("@parm_RowsAffected").Value)
            intReturnValue = Convert.ToInt32(cmd.Parameters("ReturnValue").Value)

            If (intReturnValue <> 0) Or _
               (intRowsAffected <> 1) Then
                Debug.WriteLine("Error encountered SecurityModule.UpdateTMRoleForm" & _
                       ControlChars.Cr & _
                       "Error Returned: " & intReturnValue & _
                       ", Rows Affected: " & intRowsAffected)
            Else
                blnSuccess = True
            End If
        Catch ex As Exception
            Debug.WriteLine("Error encountered in SecurityModule.UpdateTMRoleForm" & _
                ControlChars.Cr & ex.Message)
            Dim strMethodName As String = GetExecutingMethodName()
            SendExceptionEmailNotification("SecurityModule", strMethodName, ex, strMethodParms)

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        Finally
            cn.Close()
            cmd.Dispose()
        End Try
        Return blnSuccess
    End Function ' UpdateTMWorkHistory

#End Region ' TeamMember_WorkHistory Functions


#Region "Private utility methods"

    ''' <summary>
    ''' Gets the abbreviated user name from the current user's windows account name
    ''' </summary>
    ''' <returns>first initial + last name</returns>
    ''' <remarks>Extracts the first initial and last name from
    ''' the windows account name. The windows account name is expected to
    ''' be formatted like FirstName.LastName</remarks>
    Private Shared Function GetAbbrevNameFromCurrentUser() As String
        Dim strAbbrevName As String = ""
        Try
            Dim strFullName As String = commonFunctions.getUserName().Trim()
            strAbbrevName = strFullName  ' default to Full Name
            Dim intDotOffset As Integer = InStr(strFullName, ".")
            If intDotOffset > 1 Then
                ' Parse the first initial and last name from the account name
                Dim strFirstName As String = Left(strFullName, intDotOffset - 1)
                Dim strFirstInitial As String = Left(strFullName, 1)
                Dim strLastName As String = Right(strFullName, Len(strFullName) - intDotOffset)
                strAbbrevName = LCase(Trim(strFirstInitial & strLastName))
            Else
                ' Unexpected format:
                ' Default to Full Name
            End If
        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try
        Return strAbbrevName
    End Function

#End Region ' Private utility methods


#Region "Exception Handling"

    ''' <summary>
    ''' Returns the Web.config appSetting value for the specified key
    ''' </summary>
    ''' <param name="key"></param>
    ''' <returns>Value for the specified key</returns>
    ''' <remarks></remarks>
    Private Shared Function GetAppSetting(ByVal key As String) As String
        Dim str As String = _
            System.Configuration.ConfigurationManager.AppSettings(key).ToString()
        Return str
    End Function

    Private Shared Function GetExecutingMethodName() As String
        Dim strMethodName As String = ""
        Try
            ' Gets the calling method name
            Dim st As StackTrace = New StackTrace()
            Dim sf As StackFrame = st.GetFrame(1)
            Dim mb As MethodBase = sf.GetMethod()
            strMethodName = mb.Name
        Catch ex As Exception
            ' Omit
        End Try
        Return strMethodName
    End Function

    ''' <summary>
    ''' Sends an Exception Email Notification
    ''' </summary>
    ''' <param name="ModuleName">ModuleName where exception was generated</param>
    ''' <param name="MethodName">MethodName where exception was generated</param>
    ''' <param name="ExceptionObject">the generated exception</param>
    ''' <param name="AdditionalMessage">Additional message</param>
    ''' <remarks>The notification is sent to "Mary.Weyker@ugnusa.com"</remarks>
    Private Shared Sub SendExceptionEmailNotification( _
        ByVal ModuleName As String, _
        ByVal MethodName As String, _
        ByVal ExceptionObject As Exception, _
        ByVal AdditionalMessage As String)

        Try
            Dim strFromAddress As String = "Notifications@ugnauto.com"
            Dim strToAddress As String = "TNPISAppGroup@ugnauto.com"
            Dim mail As New MailMessage()
            Dim sb As New StringBuilder(5000)

            ' Build email body
            sb.Append("EXCEPTION generated from..." & Environment.NewLine & Environment.NewLine)
            sb.Append(String.Format("Time:            {0:F}", Now()))
            sb.Append(Environment.NewLine)
            sb.Append("DBServer:        " & GetAppSetting("DBServer") & Environment.NewLine)
            sb.Append("DBInstance:      " & GetAppSetting("DBInstance") & Environment.NewLine)
            sb.Append("prodOrTestURL:   " & GetAppSetting("ProdOrTestURL") & Environment.NewLine & Environment.NewLine)
            sb.Append("Base Directory:  " & System.AppDomain.CurrentDomain.BaseDirectory() & Environment.NewLine)
            Dim strHostName As String = Dns.GetHostName()
            sb.Append("Host Name:       " & strHostName & Environment.NewLine)
            sb.Append("Host IP Addr:    ")
            Dim ip As IPAddress
            Dim ips As IPAddress() = Dns.GetHostAddresses(strHostName)
            For i As Integer = 0 To ips.Length - 1
                ip = ips(i)
                If i = 0 Then
                    sb.Append(ip.ToString)
                Else
                    sb.Append(", " & ip.ToString)
                End If
            Next i
            sb.Append(Environment.NewLine)
            sb.Append("ASP.NET Account: " & System.Environment.UserName & Environment.NewLine)
            sb.Append("User Account:    " & commonFunctions.getUserName() & Environment.NewLine & Environment.NewLine)
            sb.Append("Module:          " & ModuleName & Environment.NewLine)
            sb.Append("Method:          " & MethodName & Environment.NewLine & Environment.NewLine)

            If ExceptionObject IsNot Nothing Then
                sb.Append("Exception" & Environment.NewLine)
                sb.Append("---------" & Environment.NewLine)
                sb.Append(ExceptionObject.Source & ": " & ExceptionObject.Message)
                sb.Append(Environment.NewLine & Environment.NewLine)
                sb.Append("Stack Trace" & Environment.NewLine)
                sb.Append("-----------" & Environment.NewLine)
                sb.Append(ExceptionObject.StackTrace & Environment.NewLine & Environment.NewLine)
            End If

            If (AdditionalMessage IsNot Nothing) AndAlso (AdditionalMessage.Length > 0) Then
                sb.Append("Additional Information" & Environment.NewLine)
                sb.Append("----------------------" & Environment.NewLine)
                sb.Append(AdditionalMessage.ToString() & Environment.NewLine)
            End If

            sb.Append(Environment.NewLine & "*** END OF MESSAGE ***")

            mail.Subject = "UGNNET Exception Notification"
            mail.Body = sb.ToString()
            mail.From = New MailAddress(strFromAddress)
            mail.To.Add(strToAddress)

            Dim strSMTPClient As String = GetAppSetting("SMTPClient").ToString
            Dim smtp As New SmtpClient(strSMTPClient)
            smtp.Send(mail)
        Catch ex As Exception
            Debug.WriteLine("Exception generated from SecurityModule.SendEmail: " & ex.Message)
        End Try
    End Sub

#End Region ' Exception Handling

End Class ' SecurityModule
