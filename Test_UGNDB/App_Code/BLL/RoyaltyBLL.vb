''******************************************************************************************************
''* RoyaltyBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : LRey 08/06/2010
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports RoyaltyTableAdapters

<System.ComponentModel.DataObject()> _
Public Class RoyaltyBLL
    Private RoyaltyAdapter As RoyaltyTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As RoyaltyTableAdapters.RoyaltyTableAdapter
        Get
            If RoyaltyAdapter Is Nothing Then
                RoyaltyAdapter = New RoyaltyTableAdapter()
            End If
            Return RoyaltyAdapter
        End Get
    End Property
    ''*****
    ''* Select Royalty returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetRoyalty(ByVal RoyaltyID As Integer, ByVal RoyaltyName As String) As Royalty.Royalty_MaintDataTable

        Try

            If RoyaltyName Is Nothing Then
                RoyaltyName = ""
            End If

            Return Adapter.GetRoyalty(RoyaltyName)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RoyaltyID: " & RoyaltyID & ",RoyaltyName: " & RoyaltyName & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetRoyalty : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RoyaltyBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Royalty/RoyaltyMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("GetRoyalty : " & commonFunctions.convertSpecialChar(ex.Message, False), "RoyaltyBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New Royalty
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertRoyalty(ByVal RoyaltyName As String, ByVal Obsolete As Boolean) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If RoyaltyName Is Nothing Then
                RoyaltyName = ""
            End If

            ''*****
            ' Insert the record
            ''*****
            Dim rowsAffected As Integer = Adapter.sp_Insert_Royalty(RoyaltyName, createdBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RoyaltyName:" & RoyaltyName & ", Obsolete: " & Obsolete & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertRoyalty : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RoyaltyBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Royalty/RoyaltyMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertRoyalty : " & commonFunctions.convertSpecialChar(ex.Message, False), "RoyaltyBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ' ''*****
    ''* Update Royalty
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateRoyalty(ByVal RoyaltyName As String, ByVal original_RoyaltyID As Integer, ByVal RoyaltyID As Integer, ByVal Obsolete As Boolean) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            If RoyaltyName Is Nothing Then
                RoyaltyName = ""
            End If

            ''*****
            ' Update the record
            ''*****
            Dim rowsAffected As Integer = Adapter.sp_Update_Royalty(original_RoyaltyID, RoyaltyName, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RoyaltyID:" & original_RoyaltyID & ", RoyaltyName: " & RoyaltyName & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateRoyalty : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> RoyaltyBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Royalty/RoyaltyMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateRoyalty : " & commonFunctions.convertSpecialChar(ex.Message, False), "RoyaltyBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
