
''******************************************************************************************************
''* Customer_Shut_Down_CalendarBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : LRey 05/15/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports Customer_Shut_Down_CalendarTableAdapters

<System.ComponentModel.DataObject()> _
Public Class Customer_Shut_Down_CalendarBLL
    Private Customer_Shut_Down_Calendar_Adapter As Customer_Shut_Down_Calendar_TableAdapter = Nothing
    Protected ReadOnly Property Adapter() As Customer_Shut_Down_CalendarTableAdapters.Customer_Shut_Down_Calendar_TableAdapter
        Get
            If Customer_Shut_Down_Calendar_Adapter Is Nothing Then
                Customer_Shut_Down_Calendar_Adapter = New Customer_Shut_Down_Calendar_TableAdapter()
            End If
            Return Customer_Shut_Down_Calendar_Adapter
        End Get
    End Property

    ''*****
    ''* Select Customer_Shut_Down_Calendar returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetCustomerShutDownCalendar(ByVal StartDate As String) As Customer_Shut_Down_Calendar.Customer_Shut_Down_CalendarDataTable
        Return Adapter.GetCustomerShutDownData(StartDate)
    End Function

    ''*****
    ''* Select Customer_Shut_Down_Calendar returning a single row
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetCustomerShutDownByCID(ByVal CID As Integer) As Customer_Shut_Down_Calendar.Customer_Shut_Down_CalendarDataTable
        If CID = Nothing Or CID = "" Then
            CID = 0
        End If
        Return Adapter.sp_Get_Customer_Shut_Down_Calendar_By_CID(CID)
    End Function
    ''*****
    ''* Insert New Customer_Shut_Down_Calendar
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function AddCustomer_Shut_Down_Calendar(ByVal UGNFacility As String, ByVal OEM As String, ByVal CABBV As String, ByVal SoldTo As Integer, ByVal StartDate As String, ByVal EndDate As String, ByVal WkEndWorkDay As Boolean) As Boolean

        ' Create a new Customer_Shut_Down_CalendarRow instance
        Dim Customer_Shut_Down_CalendarTable As New Customer_Shut_Down_Calendar.Customer_Shut_Down_CalendarDataTable()
        Dim Customer_Shut_Down_CalendarRow As Customer_Shut_Down_Calendar.Customer_Shut_Down_CalendarRow = Customer_Shut_Down_CalendarTable.NewCustomer_Shut_Down_CalendarRow()
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        ' Logical Rule - Cannot insert a record without a null Customer_Shut_Down_Calendar columns
        If UGNFacility = Nothing Then
            Throw New ApplicationException("Insert Cancelled: UGN Facility is a required field.")
        End If
        If OEM = Nothing Then
            Throw New ApplicationException("Insert Cancelled: OEM is a required field.")
        End If
        If CABBV = Nothing And SoldTo = Nothing Then
            Throw New ApplicationException("Insert Cancelled: Customer is a required field.")
        End If
        If StartDate = Nothing Then
            Throw New ApplicationException("Insert Cancelled: Start Date is a required field.")
        End If

        ' Insert the new Customer_Shut_Down_Calendar row
        Dim rowsAffected As Integer = Adapter.sp_Insert_Customer_Shut_Down_Calendar(UGNFacility, OEM, CABBV, SoldTo, StartDate, EndDate, WkEndWorkDay, User)

        ' Return true if precisely one row was inserted, otherwise false
        Return rowsAffected = 1
    End Function
    ''*****
    ''* Update Customer_Shut_Down_Calendar
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateCustomer_Shut_Down_Calendar(ByVal CID As Integer, ByVal UGNFacility As String, ByVal OEM As String, ByVal CABBV As String, ByVal SoldTo As Integer, ByVal StartDate As String, ByVal WkEndWorkDay As Boolean) As Boolean

        Dim Customer_Shut_Down_CalendarTable As Customer_Shut_Down_Calendar.Customer_Shut_Down_CalendarDataTable = Adapter.sp_Get_Customer_Shut_Down_Calendar_By_CID(CID)
        Dim Customer_Shut_Down_CalendarRow As Customer_Shut_Down_Calendar.Customer_Shut_Down_CalendarRow = Customer_Shut_Down_CalendarTable(0)
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        If Customer_Shut_Down_CalendarTable.Count = 0 Then
            ' no matching record found, return false
            Return False
        End If

        ' Logical Rule - Cannot insert a record without a null Customer_Shut_Down_Calendar columns
        If UGNFacility = Nothing Then
            Throw New ApplicationException("Insert Cancelled: UGN Facility is a required field.")
        End If
        If OEM = Nothing Then
            Throw New ApplicationException("Insert Cancelled: OEM is a required field.")
        End If
        If CABBV = Nothing And SoldTo = Nothing Then
            Throw New ApplicationException("Insert Cancelled: Customer is a required field.")
        End If
        If StartDate = Nothing Then
            Throw New ApplicationException("Insert Cancelled: Start Date is a required field.")
        End If
        ' Update the Customer_Shut_Down_Calendar row
        Dim rowsAffected As Integer = Adapter.sp_Update_Customer_Shut_Down_Calendar(CID, UGNFacility, OEM, CABBV, SoldTo, StartDate, WkEndWorkDay, User)

        ' Return true if precisely one row was inserted, otherwise false
        Return rowsAffected = 1
    End Function
    ''*****
    ''* Delete Customer_Shut_Down_Calendar
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteCustomer_Shut_Down_Calendar(ByVal CID As Integer) As Boolean

        Dim rowsAffected As Integer = Adapter.sp_Delete_Customer_Shut_Down_Calendar(CID)

        ' Return true if precisely one row was deleted, otherwise false
        Return rowsAffected = 1

    End Function
End Class




