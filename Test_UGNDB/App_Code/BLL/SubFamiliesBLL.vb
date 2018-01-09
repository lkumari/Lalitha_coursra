''******************************************************************************************************
''* SubFamiliesBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 04/11/2008
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports SubFamiliesTableAdapters

<System.ComponentModel.DataObject()> _
Public Class SubFamiliesBLL
    Private SubFamiliesAdapter As SubFamilyTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As SubFamiliesTableAdapters.SubFamilyTableAdapter
        Get
            If SubFamiliesAdapter Is Nothing Then
                SubFamiliesAdapter = New SubFamilyTableAdapter()
            End If
            Return SubFamiliesAdapter
        End Get
    End Property
    ''*****
    ''* Select SubFamilies returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetSubFamilies(ByVal FamilyID As Integer, ByVal SubFamilyID As Integer, ByVal SubFamilyName As String, ByVal getOldSubFamilyName As Boolean) As SubFamilies.SubFamily_MaintDataTable

        Try
            If SubFamilyName = Nothing Then SubFamilyName = ""

            Return Adapter.GetSubFamilies(FamilyID, SubFamilyID, SubFamilyName, getOldSubFamilyName)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FamilyID: " & FamilyID & ", SubFamilyID:" & SubFamilyID & "SubFamilyName: " & SubFamilyName & ", getOldSubFamilyName:" & getOldSubFamilyName & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetBillOfMaterials : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SubFamiliesBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/SubFamilyMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("GetSubFamilies : " & commonFunctions.convertSpecialChar(ex.Message, False), "SubFamiliesBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function

    ''*****
    ''* Update SubFamily
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateSubFamily(ByVal SubFamilyName As String, ByVal Obsolete As Boolean, ByVal UpdatedBy As String, ByVal original_SubFamilyID As Integer) As Boolean

        Try
            UpdatedBy = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Update the SubFamily record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateSubFamily(original_SubFamilyID, SubFamilyName, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SubFamilyName: " & SubFamilyName & ", Obsolete:" & Obsolete & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateSubFamily : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SubFamiliesBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/SubFamilyMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateSubFamily : " & commonFunctions.convertSpecialChar(ex.Message, False), "SubFamiliesBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
