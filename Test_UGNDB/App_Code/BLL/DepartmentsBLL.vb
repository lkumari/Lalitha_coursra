''******************************************************************************************************
''* DepartmentsBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 04/08/2008
''* Modified: {Name} {Date} - {Notes}
''* Modified: Roderick Carlson 03/1//2009 - added filter
''            Roderick Carlson 11/18/2010 - added GLNO column
''******************************************************************************************************

Imports DepartmentsTableAdapters

<System.ComponentModel.DataObject()> _
Public Class DepartmentsBLL
    Private DepartmentsAdapter As DepartmentTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As DepartmentsTableAdapters.DepartmentTableAdapter
        Get
            If DepartmentsAdapter Is Nothing Then
                DepartmentsAdapter = New DepartmentTableAdapter()
            End If
            Return DepartmentsAdapter
        End Get
    End Property
    ''*****
    ''* Select Departments returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetDepartment(ByVal departmentName As String, ByVal UGNFacility As String, ByVal Filter As Boolean) As Departments.Department_MaintDataTable

        Try
            If departmentName Is Nothing Then departmentName = ""
            If UGNFacility Is Nothing Then UGNFacility = ""

            Return Adapter.GetDepartment(departmentName, UGNFacility, Filter)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DepartmentName: " & departmentName & ", UGNFacility:" & UGNFacility _
            & ", Filter:" & Filter & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetDepartments : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> DepartmentsBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/DepartmentMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDepartments : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "DepartmentsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New Department
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertDepartment(ByVal departmentName As String, ByVal UGNFacility As String, _
        ByVal Filter As Boolean, ByVal GLNO As Integer, ByVal createdBy As String) As Boolean

        Try
            createdBy = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Insert the Department record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertDepartment(departmentName, UGNFacility, Filter, GLNO, createdBy)

            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "departmentName: " & departmentName _
            & ", UGNFacility:" & UGNFacility _
            & ", Filter:" & Filter _
            & ", GLNO:" & GLNO _
            & ", createdBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertDepartment : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> DepartmentsBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/DepartmentMaintenance.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertDepartment : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "DepartmentsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try
    End Function
    ''*****
    ''* Update Department
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateDepartment(ByVal DepartmentName As String, ByVal UGNFacility As String, _
            ByVal Filter As Boolean, ByVal GLNO As Integer, ByVal Obsolete As Boolean, _
            ByVal UpdatedBy As String, ByVal original_DepartmentID As Integer) As Boolean

        Try
            UpdatedBy = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Update the Department record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateDepartment(original_DepartmentID, DepartmentName, UGNFacility, Filter, GLNO, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "departmentName: " & DepartmentName _
            & ", UGNFacility:" & UGNFacility _
            & ", Filter:" & Filter _
            & ", GLNO:" & GLNO _
            & ", Obsolete:" & Obsolete _
            & ", createdBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateDepartment : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> DepartmentsBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/DepartmentMaintenance.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateDepartment : " _
            & commonFunctions.convertSpecialChar(ex.Message, False), "DepartmentsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
