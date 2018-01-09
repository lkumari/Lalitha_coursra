''******************************************************************************************************
''* RFDFacilityDeptBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 12/01/2009
''* Modified: {Name} {Date} - {Notes}
''* Modifeid: Roderick Carlson - 10/09/2012 - Removed UpdatedBy on Delete
''******************************************************************************************************

Imports RFDTableAdapters

<System.ComponentModel.DataObject()> _
Public Class RFDFacilityDeptBLL
    Private RFDFacilityDeptAdapter As RFDFacilityDeptTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As RFDTableAdapters.RFDFacilityDeptTableAdapter
        Get
            If RFDFacilityDeptAdapter Is Nothing Then
                RFDFacilityDeptAdapter = New RFDFacilityDeptTableAdapter()
            End If
            Return RFDFacilityDeptAdapter
        End Get
    End Property
    ''*****
    ''* Select RFDFacilityDept returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetRFDFacilityDept(ByVal RFDNo As Integer) As RFD.RFDFacilityDept_MaintDataTable

        Try

            Return Adapter.GetRFDFacilityDept(RFDNo)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo: " & RFDNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetRFDFacilityDept : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDFacilityDeptBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRFDFacilityDept : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDFacilityDeptBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New RFDFacilityDept
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertRFDFacilityDept(ByVal RFDNo As Integer, ByVal UGNFacility As String, ByVal DepartmentID As Integer) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If UGNFacility Is Nothing Then
                UGNFacility = ""
            End If

            Dim rowsAffected As Integer = Adapter.InsertRFDFacilityDept(RFDNo, UGNFacility, DepartmentID, createdBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RFDNo:" & RFDNo _
            & ", UGNFacility:" & UGNFacility & ", DepartmentID:" & DepartmentID _
            & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertRFDFacilityDept : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDFacilityDeptBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertRFDFacilityDept : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "RFDFacilityDeptBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    '* Update RFDFacilityDept
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateRFDFacilityDept(ByVal RowID As Integer, ByVal RFDNo As Integer, ByVal original_RowID As Integer, _
        ByVal UGNFacility As String, ByVal DepartmentID As Integer) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If UGNFacility Is Nothing Then
                UGNFacility = ""
            End If

            Dim rowsAffected As Integer = Adapter.UpdateRFDFacilityDept(original_RowID, RFDNo, UGNFacility, DepartmentID, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & RowID & ", RFDNo:" & RFDNo _
            & ", UGNFacility:" & UGNFacility & ", DepartmentID:" & DepartmentID _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateRFDFacilityDept : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDFacilityDeptBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateRFDFacilityDept : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDFacilityDeptBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

    ''*****
    '* Delete RFDFacilityDept
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteRFDFacilityDept(ByVal RowID As Integer, ByVal original_RowID As Integer, ByVal RFDNo As Integer) As Boolean

        Try

            'Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            'Dim rowsAffected As Integer = Adapter.DeleteRFDFacilityDept(original_RowID, RFDNo, UpdatedBy)
            Dim rowsAffected As Integer = Adapter.DeleteRFDFacilityDept(original_RowID, RFDNo)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID _
            & ", RFDNo:" & RFDNo _
            & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteRFDFacilityDept : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RFDFacilityDeptBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/RFD/RFD_List.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteRFDFacilityDept : " & commonFunctions.convertSpecialChar(ex.Message, False), "RFDFacilityDeptBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
