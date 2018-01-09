
''******************************************************************************************************
''* Team_Member_CalendarBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : LRey 06/16/2008
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports Team_Member_CalendarTableAdapters

<System.ComponentModel.DataObject()> _
Public Class Team_Member_CalendarBLL
    Private Team_Member_Calendar_Adapter As Team_Member_Calendar_TableAdapter = Nothing
    Protected ReadOnly Property Adapter() As Team_Member_CalendarTableAdapters.Team_Member_Calendar_TableAdapter
        Get
            If Team_Member_Calendar_Adapter Is Nothing Then
                Team_Member_Calendar_Adapter = New Team_Member_Calendar_TableAdapter()
            End If
            Return Team_Member_Calendar_Adapter
        End Get
    End Property
    ''*****
    ''* Select Team_Member_Calendar returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetTeamMemberCalendar(ByVal StartDate As String) As Team_Member_Calendar.Team_Member_CalendarDataTable
        Return Adapter.Get_Team_Member_Calendar(StartDate)
    End Function

    ''*****
    ''* Select Team_Member_Calendar returning a single row
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetTeamMemberCalendarbyCID(ByVal CID As Integer) As Team_Member_Calendar.Team_Member_CalendarDataTable
        Return Adapter.Get_Team_Member_Calendar_By_CID(CID)
    End Function
    ''*****
    ''* Insert New Team_Member_Calendar
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function AddTeam_Member_Calendar(ByVal TeamMemberID As Integer, ByVal EventDesc As String, ByVal StartDate As String, ByVal EndDate As String, ByVal AlertBackup As Boolean) As Boolean

        ' Create a new Team_Member_CalendarRow instance
        Dim Team_Member_CalendarTable As New Team_Member_Calendar.Team_Member_CalendarDataTable()
        Dim Team_Member_CalendarRow As Team_Member_Calendar.Team_Member_CalendarRow = Team_Member_CalendarTable.NewTeam_Member_CalendarRow()
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        ' Logical Rule - Cannot insert a record without a null Team_Member_Calendar columns
        If TeamMemberID = Nothing Then
            Throw New ApplicationException("Insert Cancelled: Team Member is a required field.")
        End If
        If EventDesc = Nothing Then
            Throw New ApplicationException("Insert Cancelled: Event is a required field.")
        End If
        If StartDate = Nothing Then
            Throw New ApplicationException("Insert Cancelled: Start Date is a required field.")
        End If

        ' Insert the new Team_Member_Calendar row
        Dim rowsAffected As Integer = Adapter.sp_Insert_Team_Member_Calendar(TeamMemberID, EventDesc, StartDate, EndDate, AlertBackup, User)

        ' Return true if precisely one row was inserted, otherwise false
        Return rowsAffected = 1
    End Function
    ''*****
    ''* Update Team_Member_Calendar
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateTeam_Member_Calendar(ByVal CID As Integer, ByVal TeamMemberID As Integer, ByVal EventDesc As String, ByVal StartDate As String, ByVal AlertBackup As Boolean) As Boolean

        Dim Team_Member_CalendarTable As Team_Member_Calendar.Team_Member_CalendarDataTable = Adapter.Get_Team_Member_Calendar_By_CID(CID)
        Dim Team_Member_CalendarRow As Team_Member_Calendar.Team_Member_CalendarRow = Team_Member_CalendarTable(0)
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        If Team_Member_CalendarTable.Count = 0 Then
            ' no matching record found, return false
            Return False
        End If

        ' Logical Rule - Cannot update a record without a null Team_Member_Calendar column
        If TeamMemberID = Nothing Then
            Throw New ApplicationException("Insert Cancelled: Team Member is a required field.")
        End If
        If EventDesc = Nothing Then
            Throw New ApplicationException("Insert Cancelled: Event is a required field.")
        End If
        If StartDate = Nothing Then
            Throw New ApplicationException("Insert Cancelled: Start Date is a required field.")
        End If

        ' Update the Team_Member_Calendar row
        Dim rowsAffected As Integer = Adapter.sp_Update_Team_Member_Calendar(CID, TeamMemberID, EventDesc, StartDate, AlertBackup, User)

        ' Return true if precisely one row was inserted, otherwise false
        Return rowsAffected = 1
    End Function
    ''*****
    ''* Delete Team_Member_Calendar
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteTeam_Member_Calendar(ByVal CID As Integer) As Boolean

        Dim rowsAffected As Integer = Adapter.sp_Delete_Team_Member_Calendar(CID)

        ' Return true if precisely one row was deleted, otherwise false
        Return rowsAffected = 1

    End Function
End Class




