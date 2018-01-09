''******************************************************************************************************
''* CellsBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 04/15/2008
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports CellsTableAdapters

<System.ComponentModel.DataObject()> _
Public Class CellsBLL
    Private CellsAdapter As CellTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CellsTableAdapters.CellTableAdapter
        Get
            If CellsAdapter Is Nothing Then
                CellsAdapter = New CellTableAdapter()
            End If
            Return CellsAdapter
        End Get
    End Property
    ''*****
    ''* Select Cells returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetCells(ByVal CellID As Integer, ByVal DepartmentID As Integer, ByVal UGNFacility As String, ByVal CellName As String, ByVal PlannerCode As String) As Cells.Cell_MaintDataTable

        Try
            If UGNFacility Is Nothing Then
                UGNFacility = ""
            End If

            If CellName Is Nothing Then
                CellName = ""
            End If

            If PlannerCode Is Nothing Then
                PlannerCode = ""
            End If

            Return Adapter.GetCells(CellID, DepartmentID, UGNFacility, CellName, PlannerCode)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CellID: " & CellID & ", DepartmentID:" & DepartmentID.ToString & ", UGNFacilityID: " & UGNFacility & ", CellName: " & CellName & ", PlannerCode: " & PlannerCode & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertCell : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CellsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/CellMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCell : " & commonFunctions.convertSpecialChar(ex.Message, False), "CellsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New Cell
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertCell(ByVal DepartmentID As Integer, ByVal UGNFacility As String, ByVal CellName As String, ByVal PlannerCode As String, ByVal createdBy As String) As Boolean

        Try
            createdBy = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Insert the Cell record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertCell(DepartmentID, UGNFacility, CellName, PlannerCode, createdBy)

            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "Insert_DepartmentID:" & DepartmentID.ToString & ", Insert_UGNFacilityID: " & UGNFacility & ", Insert_CellName: " & CellName & ", Insert_PlannerCode: " & PlannerCode & ", CreatedBy: " & createdBy
            HttpContext.Current.Session("BLLerror") = "InsertCell : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CellsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/CellMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCell : " & commonFunctions.convertSpecialChar(ex.Message, False), "CellsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Update Cell
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateCell(ByVal DepartmentID As Integer, ByVal UGNFacility As String, ByVal CellName As String, ByVal PlannerCode As String, ByVal Obsolete As Boolean, ByVal UpdatedBy As String, ByVal original_CellID As Integer, ByVal original_DepartmentID As Integer, ByVal original_UGNFacility As String, ByVal original_CellName As String) As Boolean

        Try

            UpdatedBy = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Update the Cell record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateCell(original_CellID, DepartmentID, UGNFacility, CellName, PlannerCode, original_DepartmentID, original_UGNFacility, original_CellName, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "Update_DepartmentID:" & DepartmentID.ToString & ", Update_UGNFacilityID: " & UGNFacility & ", Update_CellName: " & CellName & ", Update_PlannerCode: " & PlannerCode & ", Update_Obsolete: " & Obsolete.ToString & ", Original_CellID:" & original_CellID & ", UpdatedBy: " & UpdatedBy
            HttpContext.Current.Session("BLLerror") = "UpdateCell : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CellsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/CellMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCell : " & commonFunctions.convertSpecialChar(ex.Message, False), "CellsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
