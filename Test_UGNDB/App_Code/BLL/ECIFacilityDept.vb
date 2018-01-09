''******************************************************************************************************
''* ECIFacilityDeptBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 07/20/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports ECITableAdapters

<System.ComponentModel.DataObject()> _
Public Class ECIFacilityDeptBLL
    Private ECIFacilityDeptAdapter As ECIFacilityDeptTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As ECITableAdapters.ECIFacilityDeptTableAdapter
        Get
            If ECIFacilityDeptAdapter Is Nothing Then
                ECIFacilityDeptAdapter = New ECIFacilityDeptTableAdapter()
            End If
            Return ECIFacilityDeptAdapter
        End Get
    End Property
    ''*****
    ''* Select ECIFacilityDept returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetECIFacilityDept(ByVal ECINo As Integer) As ECI.ECIFacilityDept_MaintDataTable

        Try

            Return Adapter.GetECIFacilityDept(ECINo)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ECINo: " & ECINo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetECIFacilityDept : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIFacilityDeptBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetECIFacilityDept : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIFacilityDeptBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New ECIFacilityDept
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertECIFacilityDept(ByVal ECINo As Integer, ByVal UGNFacility As String, ByVal DepartmentID As Integer) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If UGNFacility Is Nothing Then
                UGNFacility = ""
            End If


            Dim rowsAffected As Integer = Adapter.InsertECIFacilityDept(ECINo, UGNFacility, DepartmentID, createdBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ECINo:" & ECINo & ", UGNFacility:" & UGNFacility _
            & ", DepartmentID:" & DepartmentID _
            & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertECIFacilityDept : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIFacilityDeptBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertECIFacilityDept : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIFacilityDeptBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ' ''*****
    ''* Update ECIFacilityDept
    ''*****
    '<System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    'Public Function UpdateECIFacilityDept(ByVal RowID As Integer, ByVal original_RowID As Integer, ByVal ECINo As Integer, _
    'ByVal original_ECINo As Integer, ByVal UGNFacility As String, ByVal DepartmentID As Integer) As Boolean

    '    Try

    '        Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        If UGNFacility Is Nothing Then
    '            UGNFacility = ""
    '        End If

    '        Dim rowsAffected As Integer = Adapter.UpdateECIFacilityDept(original_RowID, original_ECINo, UGNFacility, DepartmentID, UpdatedBy)

    '        ' Return true if precisely one row was updated, otherwise false
    '        Return rowsAffected = 1
    '    Catch ex As Exception
    '        'on error, collect function data, error, and last page, then redirect to error page
    '        Dim strUserEditedData As String = "RowID:" & original_RowID & ", ECINo:" & original_ECINo & ", UGNFacility:" & UGNFacility _
    '         & ", DepartmentID:" & DepartmentID _
    '         & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

    '        HttpContext.Current.Session("BLLerror") = "UpdateECIFacilityDept : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIFacilityDeptBLL.vb :<br/> " & strUserEditedData
    '        HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
    '        UGNErrorTrapping.InsertErrorLog("UpdateECIFacilityDept : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIFacilityDeptBLL.vb", strUserEditedData)

    '        HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
    '        Return False
    '    End Try

    'End Function

    ''*****
    '* Delete ECIFacilityDept
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteECIFacilityDept(ByVal RowID As Integer, ByVal original_RowID As Integer) As Boolean

        Try

            Dim rowsAffected As Integer = Adapter.DeleteECIFacilityDept(original_RowID)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID _
             & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteECIFacilityDept: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECIFacilityDeptBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteECIFacilityDept : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECIFacilityDeptBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
