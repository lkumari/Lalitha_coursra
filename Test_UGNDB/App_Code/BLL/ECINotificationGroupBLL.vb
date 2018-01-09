''******************************************************************************************************
''* ECINotificationGroupBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 09/21/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports ECITableAdapters

<System.ComponentModel.DataObject()> _
Public Class ECINotificationGroupBLL
    Private ECINotificationGroupAdapter As ECINotificationGroupTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As ECITableAdapters.ECINotificationGroupTableAdapter
        Get
            If ECINotificationGroupAdapter Is Nothing Then
                ECINotificationGroupAdapter = New ECINotificationGroupTableAdapter()
            End If
            Return ECINotificationGroupAdapter
        End Get
    End Property
    ''*****
    ''* Select ECINotificationGroup returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetECINotificationGroup(ByVal ECINo As Integer) As ECI.ECINotificationGroup_MaintDataTable

        Try

            Return Adapter.GetECINotificationGroup(ECINo)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ECINo: " & ECINo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetECINotificationGroup : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECINotificationGroupBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetECINotificationGroup : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECINotificationGroupBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New ECINotificationGroup
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertECINotificationGroup(ByVal ECINo As Integer, ByVal GroupID As Integer) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter.InsertECINotificationGroup(ECINo, GroupID, createdBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ECINo:" & ECINo & ", GroupID:" & GroupID & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertECINotificationGroup : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECINotificationGroupBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertECINotificationGroup : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECINotificationGroupBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

    ''*****
    ''* Delete New ECINotificationGroup
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteECINotificationGroup(ByVal RowID As Integer, ByVal original_RowID As Integer) As Boolean

        Try
    
            Dim rowsAffected As Integer = Adapter.DeleteECINotificationGroup(original_RowID)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "original_RowID:" & original_RowID & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteECINotificationGroup : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ECINotificationGroupBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ECI/ECI_List.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteECINotificationGroup : " & commonFunctions.convertSpecialChar(ex.Message, False), "ECINotificationGroupBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
