''******************************************************************************************************
''* FamiliesBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 04/11/2008
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports FamiliesTableAdapters

<System.ComponentModel.DataObject()> _
Public Class FamiliesBLL
    Private FamiliesAdapter As FamilyTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As FamiliesTableAdapters.FamilyTableAdapter
        Get
            If FamiliesAdapter Is Nothing Then
                FamiliesAdapter = New FamilyTableAdapter()
            End If
            Return FamiliesAdapter
        End Get
    End Property
    ''*****
    ''* Select Families returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetFamilies(ByVal FamilyID As Integer, ByVal oldFamilyID As Integer, ByVal FamilyName As String) As Families.Family_MaintDataTable

        Try
            If FamilyName = Nothing Then FamilyName = ""

            Return Adapter.GetFamilies(FamilyID, oldFamilyID, FamilyName)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FamilyID: " & FamilyID & ", oldFamilyID:" & oldFamilyID & ", FamilyName:" & FamilyName & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetFamilies : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FamiliesBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/FamilyMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("GetFamilies : " & commonFunctions.convertSpecialChar(ex.Message, False), "FamiliesBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function

    ''*****
    ''* Update Family
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateFamily(ByVal FamilyName As String, ByVal FormulaCode As String, ByVal Obsolete As Boolean, ByVal UpdatedBy As String, ByVal original_FamilyID As Integer) As Boolean

        Try
            UpdatedBy = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Update the Family record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateFamily(original_FamilyID, FormulaCode, FamilyName, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FamilyName: " & FamilyName & ", FormulaCode:" & FormulaCode & ", Obsolete:" & Obsolete & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetFamilies : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> FamiliesBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/FamilyMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("GetFamilies : " & commonFunctions.convertSpecialChar(ex.Message, False), "FamiliesBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try
    End Function
End Class
