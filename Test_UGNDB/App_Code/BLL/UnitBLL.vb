''******************************************************************************************************
''* UnitBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 02/20/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports UnitTableAdapters

<System.ComponentModel.DataObject()> _
Public Class UnitBLL
    Private UnitAdapter As UnitTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As UnitTableAdapters.UnitTableAdapter
        Get
            If UnitAdapter Is Nothing Then
                UnitAdapter = New UnitTableAdapter()
            End If
            Return UnitAdapter
        End Get
    End Property
    ''*****
    ''* Select Unit returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetUnit(ByVal UnitID As Integer, ByVal UnitName As String, ByVal UnitAbbr As String) As Unit.Unit_MaintDataTable

        Try
            If UnitName Is Nothing Then UnitName = ""
            If UnitAbbr Is Nothing Then UnitAbbr = ""

            Return Adapter.GetUnit(UnitID, UnitName, UnitAbbr)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "UnitID: " & UnitID & ", UnitName: " & UnitName & ", UnitAbbr: " & UnitAbbr & _
            ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetUnit : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> UnitBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("GetUnit : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "UnitBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New Unit
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertUnit(ByVal UnitName As String, ByVal UnitAbbr As String, ByVal Obsolete As Boolean) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If UnitName Is Nothing Then
                UnitName = ""
            End If

            If UnitAbbr Is Nothing Then
                UnitAbbr = ""
            End If

            ''*****
            ' Insert the record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertUnit(UnitName, UnitAbbr, Obsolete, createdBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "UnitName: " & UnitName & ", UnitAbbr: " & UnitAbbr & _
            ", Obsolete: " & Obsolete & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertUnit : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> UnitBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("InsertUnit : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "UnitBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ' ''*****
    ''* Update Unit
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateUnit(ByVal original_UnitID As Integer, ByVal UnitName As String, _
        ByVal UnitAbbr As String, ByVal Obsolete As Boolean, ByVal ddUnitName As String, _
        ByVal ddUnitAbbr As String, ByVal UnitID As Integer) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If UnitName Is Nothing Then
                UnitName = ""
            End If

            If UnitAbbr Is Nothing Then
                UnitAbbr = ""
            End If

            ''*****
            ' Update the record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateUnit(original_UnitID, UnitName, UnitAbbr, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "UnitID:" & original_UnitID & ", UnitName: " & UnitName & _
            ", UnitAbbr: " & UnitAbbr & ", Obsolete: " & Obsolete & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateUnit : " & commonFunctions.convertSpecialChar(ex.Message, False) & _
            " :<br/> UnitBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateUnit : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "UnitBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

End Class
