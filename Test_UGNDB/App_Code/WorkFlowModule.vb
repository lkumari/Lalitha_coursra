''************************************************************************************************
''Name:		WorkFlowModule.vb
''Purpose:	This code is referenced from all vb files, mostly for the purpose of calling stored 
''          procedures or getting user-rights for the Workflow Module
''
''Date		    Author	    
''03/19/2008    LRey			Created .Net application
''11/11/2010    RCarlson        Modified: Added DeleteWFCookies_TeamMemberMakeAssignments function
''************************************************************************************************

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
Imports Microsoft.VisualBasic

Public Class WorkFlowModule
    Public Shared Sub DeleteWFCookies_TeamMemberBackupSearch()
        ''***
        '' Used to clear out cookies in the Team Member Search screen.  Called from Reset button click.
        ''***
        Try
            HttpContext.Current.Response.Cookies("WF_TeamMember").Value = ""
            HttpContext.Current.Response.Cookies("WF_TeamMember").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("WF_Subscription").Value = ""
            HttpContext.Current.Response.Cookies("WF_Subscription").Expires = DateTime.Now.AddDays(-1)
        Catch ex As Exception
            Dim msg As String = ex.Message
        End Try

    End Sub 'EOF DeleteWFCookies_TeamMemberBackupSearch

    Public Shared Sub DeleteWFCookies_TeamMemberAssignments()
        ''***
        '' Used to clear out cookies in the Team Member Search screen.  Called from Reset button click.
        ''***
        Try
            HttpContext.Current.Response.Cookies("WFA_TeamMember").Value = ""
            HttpContext.Current.Response.Cookies("WFA_TeamMember").Expires = DateTime.Now.AddDays(-1)
        Catch ex As Exception
            Dim msg As String = ex.Message
        End Try

    End Sub 'EOF DeleteWFCookies_TeamMemberAssignments

    Public Shared Sub DeleteWFCookies_TeamMemberCommodityAssignments()
        ''***
        '' Used to clear out cookies in the Team Member Search screen.  Called from Reset button click.
        ''***
        Try
            HttpContext.Current.Response.Cookies("WFCA_TeamMember").Value = ""
            HttpContext.Current.Response.Cookies("WFCA_TeamMember").Expires = DateTime.Now.AddDays(-1)
        Catch ex As Exception
            Dim msg As String = ex.Message
        End Try

    End Sub 'EOF DeleteWFCookies_TeamMemberCommodityAssignments

    Public Shared Sub DeleteWFCookies_TeamMemberFamilyAssignments()
        ''***
        '' Used to clear out cookies in the Team Member Search screen.  Called from Reset button click.
        ''***
        Try
            HttpContext.Current.Response.Cookies("WFFA_TeamMember").Value = ""
            HttpContext.Current.Response.Cookies("WFFA_TeamMember").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("WFFA_Family").Value = ""
            HttpContext.Current.Response.Cookies("WFFA_Family").Expires = DateTime.Now.AddDays(-1)
        Catch ex As Exception
            Dim msg As String = ex.Message
        End Try

    End Sub 'EOF DeleteWFCookies_TeamMemberFamilyAssignments

    Public Shared Sub DeleteWFCookies_TeamMemberMakeAssignments()
        ''***
        '' Used to clear out cookies in the Team Member Search screen.  Called from Reset button click.
        ''***
        Try
            HttpContext.Current.Response.Cookies("WFMA_TeamMember").Value = ""
            HttpContext.Current.Response.Cookies("WFMA_TeamMember").Expires = DateTime.Now.AddDays(-1)
        Catch ex As Exception
            Dim msg As String = ex.Message
        End Try

    End Sub 'EOF DeleteWFCookies_TeamMemberCommodityAssignments
    Public Shared Sub InsertTeamMemberCalendar(ByVal TeamMemberID As Integer, ByVal EventDesc As String, ByVal StartDate As String, ByVal EndDate As String, ByVal AlertBackup As Boolean)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Team_Member_Calendar"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@Event", SqlDbType.VarChar)
            myCommand.Parameters("@Event").Value = EventDesc

            myCommand.Parameters.Add("@StartDate", SqlDbType.VarChar)
            myCommand.Parameters("@StartDate").Value = StartDate

            myCommand.Parameters.Add("@EndDate", SqlDbType.VarChar)
            myCommand.Parameters("@EndDate").Value = EndDate

            myCommand.Parameters.Add("@AlertBackup", SqlDbType.Bit)
            myCommand.Parameters("@AlertBackup").Value = AlertBackup


            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = User

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            Dim msg As String = ex.Message
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertTeamMemberCalendar

    Public Shared Sub UpdateTeamMemberCalendar(ByVal CID As Integer, ByVal TeamMemberID As Integer, ByVal EventDesc As String, ByVal StartDate As String, ByVal EndDate As String, ByVal AlertBackup As Boolean)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Team_Member_Calendar"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CID", SqlDbType.Int)
            myCommand.Parameters("@CID").Value = CID

            myCommand.Parameters.Add("@TeamMemberID", SqlDbType.Int)
            myCommand.Parameters("@TeamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@Event", SqlDbType.VarChar)
            myCommand.Parameters("@Event").Value = EventDesc

            myCommand.Parameters.Add("@StartDate", SqlDbType.VarChar)
            myCommand.Parameters("@StartDate").Value = StartDate

            myCommand.Parameters.Add("@AlertBackup", SqlDbType.Bit)
            myCommand.Parameters("@AlertBackup").Value = AlertBackup

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = User

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            Dim msg As String = ex.Message
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF UpdateTeamMemberCalendar

    Public Shared Sub DeleteTeamMemberCalendar(ByVal CID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Team_Member_Calendar"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CID", SqlDbType.Int)
            myCommand.Parameters("@CID").Value = CID

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            Dim msg As String = ex.Message
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF DeleteTeamMemberCalendar

    Public Shared Function GetTeamMemberCalendarByCID(ByVal CID As String) As DataSet
        ''Used in 
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Team_Member_Calendar_By_CID"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CID", SqlDbType.Int)
            myCommand.Parameters("@CID").Value = CID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CID")
            GetTeamMemberCalendarByCID = GetData
        Catch ex As Exception
            Dim rslt As String = ex.Message
            GetTeamMemberCalendarByCID = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetTeamMemberCalendarByCID
End Class
