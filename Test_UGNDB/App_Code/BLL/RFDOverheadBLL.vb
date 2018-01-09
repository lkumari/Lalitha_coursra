''******************************************************************************************************
''* RFDOverheadBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 10/13/2010
''* Modified: {Name} {Date} - {Notes}
''* Modifeid: Roderick Carlson - 10/09/2012 - Removed UpdatedBy on Delete
''******************************************************************************************************

Imports RFDTableAdapters

<System.ComponentModel.DataObject()> _
Public Class RFDOverheadBLL
    Private RFDOverheadAdapter As RFDOverheadTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As RFDTableAdapters.RFDOverheadTableAdapter
        Get
            If RFDOverheadAdapter Is Nothing Then
                RFDOverheadAdapter = New RFDOverheadTableAdapter()
            End If
            Return RFDOverheadAdapter
        End Get
    End Property
    ''*****
    ''* Select RFDOverhead returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetRFDOverhead(ByVal RFDNo As Integer) As RFD.RFDOverhead_MaintDataTable

        Try

            Return Adapter.GetRFDOverhead(RFDNo)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetRFDOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDOverheadBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRFDOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDOverheadBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New RFDOverhead
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertRFDOverhead(ByVal RFDNo As Integer, ByVal LaborID As Integer, _
            ByVal FixedRate As Double, ByVal VariableRate As Double, ByVal CrewSize As Double, _
            ByVal NumberOfCarriers As Double, ByVal isOffline As Boolean) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter.InsertRFDOverhead(RFDNo, LaborID, FixedRate, VariableRate, CrewSize, NumberOfCarriers, isOffline, createdBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo:" & RFDNo _
            & ", LaborID:" & LaborID _
            & ", FixedRate:" & FixedRate _
            & ", VariableRate:" & VariableRate _
            & ", CrewSize:" & CrewSize _
            & ", NumberOfCarriers:" & NumberOfCarriers _
            & ", isOffline:" & isOffline _
            & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertRFDOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDOverheadBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertRFDOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDOverheadBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    '* Update RFDOverhead
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateRFDOverhead(ByVal original_RowID As Integer, ByVal RFDNo As Integer, ByVal LaborID As Integer, _
            ByVal FixedRate As Double, ByVal VariableRate As Double, ByVal CrewSize As Double, _
            ByVal NumberOfCarriers As Double, ByVal isOffline As Boolean) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter.UpdateRFDOverhead(original_RowID, RFDNo, LaborID, FixedRate, VariableRate, CrewSize, NumberOfCarriers, isOffline, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo:" & RFDNo _
            & ", LaborID:" & LaborID _
            & ", FixedRate:" & FixedRate _
            & ", VariableRate:" & VariableRate _
            & ", CrewSize:" & CrewSize _
            & ", NumberOfCarriers:" & NumberOfCarriers _
            & ", isOffline:" & isOffline _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateRFDOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDOverheadBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateRFDOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDOverheadBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

    ''*****
    '* Delete RFDOverhead
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteRFDOverhead(ByVal RowID As Integer, ByVal RFDNo As Integer, ByVal original_RowID As Integer) As Boolean

        Try

            'Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            'Dim rowsAffected As Integer = Adapter.DeleteRFDOverhead(original_RowID, RFDNo, UpdatedBy)
            Dim rowsAffected As Integer = Adapter.DeleteRFDOverhead(original_RowID, RFDNo)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteRFDOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDOverheadBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteRFDOverhead : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDOverheadBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

End Class
