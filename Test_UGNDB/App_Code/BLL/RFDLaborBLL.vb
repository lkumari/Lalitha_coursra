''******************************************************************************************************
''* RFDLaborBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 10/13/2010
''* Modified: {Name} {Date} - {Notes}
''* Modifeid: Roderick Carlson - 10/09/2012 - Removed UpdatedBy on Delete
''******************************************************************************************************

Imports RFDTableAdapters

<System.ComponentModel.DataObject()> _
Public Class RFDLaborBLL
    Private RFDLaborAdapter As RFDLaborTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As RFDTableAdapters.RFDLaborTableAdapter
        Get
            If RFDLaborAdapter Is Nothing Then
                RFDLaborAdapter = New RFDLaborTableAdapter()
            End If
            Return RFDLaborAdapter
        End Get
    End Property
    ''*****
    ''* Select RFDLabor returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetRFDLabor(ByVal RFDNo As Integer) As RFD.RFDLabor_MaintDataTable

        Try

            Return Adapter.GetRFDLabor(RFDNo)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetRFDLabor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDLaborBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRFDLabor : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDLaborBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New RFDLabor
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertRFDLabor(ByVal RFDNo As Integer, ByVal LaborID As Integer, _
            ByVal Rate As Double, ByVal CrewSize As Double, _
            ByVal isOffline As Boolean) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter.InsertRFDLabor(RFDNo, LaborID, Rate, CrewSize, isOffline, createdBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo:" & RFDNo _
            & ", LaborID:" & LaborID _
            & ", Rate:" & Rate _
            & ", CrewSize:" & CrewSize _
            & ", isOffline:" & isOffline _
            & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertRFDLabor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDLaborBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertRFDLabor : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDLaborBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    '* Update RFDLabor
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateRFDLabor(ByVal original_RowID As Integer, ByVal RFDNo As Integer, ByVal LaborID As Integer, _
            ByVal Rate As Double, ByVal CrewSize As Double, _
            ByVal isOffline As Boolean) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter.UpdateRFDLabor(original_RowID, RFDNo, LaborID, Rate, CrewSize, isOffline, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo:" & RFDNo _
            & ", LaborID:" & LaborID _
            & ", Rate:" & Rate _
            & ", CrewSize:" & CrewSize _
            & ", isOffline:" & isOffline _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateRFDLabor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDLaborBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateRFDLabor : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDLaborBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

    ''*****
    '* Delete RFDLabor
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteRFDLabor(ByVal RowID As Integer, ByVal RFDNo As Integer, ByVal original_RowID As Integer) As Boolean

        Try

            'Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            'Dim rowsAffected As Integer = Adapter.DeleteRFDLabor(original_RowID, RFDNo, UpdatedBy)
            Dim rowsAffected As Integer = Adapter.DeleteRFDLabor(original_RowID, RFDNo)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteRFDLabor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDLaborBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteRFDLabor : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDLaborBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

End Class
