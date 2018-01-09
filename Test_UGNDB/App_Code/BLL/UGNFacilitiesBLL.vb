''******************************************************************************************************
''* UGNFacilitiesBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 04/08/2008
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports UGNFacilitiesTableAdapters

<System.ComponentModel.DataObject()> _
Public Class UGNFaciltiesBLL
    Private UGNFaciltiesAdapter As UGNFacilityTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As UGNFacilitiesTableAdapters.UGNFacilityTableAdapter
        Get
            If UGNFaciltiesAdapter Is Nothing Then
                UGNFaciltiesAdapter = New UGNFacilityTableAdapter()
            End If
            Return UGNFaciltiesAdapter
        End Get
    End Property
    ''*****
    ''* Select UGNFacilties returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetUGNFacilties(ByVal UGNFacilityName As String) As UGNFacilities.UGNFacility_MaintDataTable

        Try
            If UGNFacilityName Is Nothing Then
                UGNFacilityName = ""
            End If

            Return Adapter.GetUGNFacilities()
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "UGNFaciltyName: " & UGNFacilityName & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetUGNFacilties : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> UGNFaciltiesBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/UGNFacilityMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("GetUGNFacilties : " & commonFunctions.convertSpecialChar(ex.Message, False), "UGNFaciltiesBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New UGNFacilty
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertUGNFacilty(ByVal UGNFacility As String, ByVal UGNFacilityName As String, ByVal createdBy As String) As Boolean

        Try
            createdBy = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            ''*****
            ' Insert the UGNFacilty record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertUGNFacility(UGNFacility, UGNFacilityName, createdBy)

            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "UGNFacility: " & UGNFacility & "UGNFaciltyName: " & UGNFacilityName & ", createdBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertUGNFacilty : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> UGNFaciltiesBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/UGNFacilityMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertUGNFacilty : " & commonFunctions.convertSpecialChar(ex.Message, False), "UGNFaciltiesBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Update UGNFacilty
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateUGNFacilty(ByVal UGNFacility As String, ByVal UGNFacilityName As String, ByVal Obsolete As Boolean, ByVal UpdatedBy As String, ByVal original_UGNFacility As String) As Boolean

        Try
            UpdatedBy = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Update the UGNFacilty record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateUGNFacility(UGNFacility, UGNFacilityName, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "UGNFacility: " & UGNFacility & "UGNFaciltyName: " & UGNFacilityName & "Obsolete: " & Obsolete & "original_UGNFacility: " & original_UGNFacility & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateUGNFacilty : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> UGNFaciltiesBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/UGNFacilityMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateUGNFacilty : " & commonFunctions.convertSpecialChar(ex.Message, False), "UGNFaciltiesBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
