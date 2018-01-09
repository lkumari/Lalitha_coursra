
''******************************************************************************************************
''* Vehicle_CopyBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Called From: Vehicle_Volume.aspx - btnCopy, Copy_Vehicle.aspx
''* Author  : LRey 05/14/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports Projected_SalesTableAdapters
<System.ComponentModel.DataObject()> _
Public Class Vehicle_CopyBLL
    Private psAdapter As Vehicle_Copy_TableAdapter = Nothing

    Protected ReadOnly Property Adapter() As Projected_SalesTableAdapters.Vehicle_Copy_TableAdapter
        Get
            If psAdapter Is Nothing Then
                psAdapter = New Vehicle_Copy_TableAdapter()
            End If
            Return psAdapter
        End Get
    End Property

    ''*****
    ''* Select Vehicle_Copy returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
        Public Function GetVehicleCopy(ByVal SourceProgramID As Integer, ByVal SourceCABBV As String, ByVal SourceSoldTo As Integer) As Projected_Sales.Vehicle_CopyDataTable

        ''Return Adapter.Get_Vehicle_Copy(HttpContext.Current.Request.QueryString("sPGMID"), HttpContext.Current.Request.QueryString("sCABBV"), HttpContext.Current.Request.QueryString("sSoldTo"))
        Return Adapter.Get_Vehicle_Copy(SourceProgramID, SourceCABBV, SourceSoldTo)

    End Function

    ''*****
    ''* Insert a New row to Vehicle_Copy table
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertVehicleCopy(ByVal SourceProgramID As Integer, ByVal SourceCABBV As String, ByVal SourceSoldTo As Integer, ByVal DestinationProgramID As Integer, ByVal DestinationCABBV As String, ByVal DestinationSoldTo As Integer) As Boolean

        ' Create a new pscpRow instance
        Dim psTable As New Projected_Sales.Vehicle_CopyDataTable
        Dim psRow As Projected_Sales.Vehicle_CopyRow = psTable.NewVehicle_CopyRow
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        ' Logical Rule - Cannot insert a record without null columns
        If SourceProgramID = Nothing And HttpContext.Current.Request.QueryString("sPGMID") = Nothing Then
            Throw New ApplicationException("Insert Cancelled: Source Program is a required field.")
        End If
        If DestinationProgramID = Nothing Then
            Throw New ApplicationException("Insert Cancelled: Destination Program is a required field.")
        End If

        ' Insert the new Vehicle_Copy row
        ''Dim rowsAffected As Integer = Adapter.sp_Insert_Vehicle_Copy(HttpContext.Current.Request.QueryString("sPGMID"), HttpContext.Current.Request.QueryString("sCABBV"), HttpContext.Current.Request.QueryString("sSoldTo"), DestinationProgramID, DestinationCABBV, DestinationSoldTo, User)
        Dim rowsAffected As Integer = Adapter.sp_Insert_Vehicle_Copy(SourceProgramID, SourceCABBV, SourceSoldTo, DestinationProgramID, DestinationCABBV, DestinationSoldTo, User)

        ' Return true if precisely one row was inserted, otherwise false
        Return rowsAffected = 1
    End Function

    ''*****
    ''* Delete Vehicle_Copy
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteVehicleCopy(ByVal SourceProgramID As Integer, ByVal SourceCABBV As String, ByVal SourceSoldTo As Integer, ByVal DestinationProgramID As Integer, ByVal DestinationCABBV As String, ByVal DestinationSoldTo As Integer, ByVal original_SourceProgramID As Integer, ByVal original_SourceCABBV As String, ByVal original_SourceSoldTo As Integer, ByVal original_DestinationProgramID As Integer, ByVal original_DestinationCABBV As String, ByVal original_DestinationSoldTo As Integer) As Boolean

        ''Dim rowsAffected As Integer = Adapter.sp_Delete_Vehicle_Copy(HttpContext.Current.Request.QueryString("sPGMID"), HttpContext.Current.Request.QueryString("sCABBV"), HttpContext.Current.Request.QueryString("sSoldTo"), DestinationProgramID, DestinationCABBV, DestinationSoldTo)

        Dim rowsAffected As Integer = Adapter.sp_Delete_Vehicle_Copy(SourceProgramID, SourceCABBV, SourceSoldTo, DestinationProgramID, DestinationCABBV, DestinationSoldTo)

        ' Return true if precisely one row was deleted, otherwise false
        Return rowsAffected = 1

    End Function

End Class

