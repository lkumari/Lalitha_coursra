''******************************************************************************************************
''* DrawingNotificationsBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, and Update.
''*
''* Author  : RCarlson 07/30/2008
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports DrawingsTableAdapters

<System.ComponentModel.DataObject()> _
Public Class DrawingNotificationsBLL
    Private DrawingNotificationAdapter As DrawingNotificationsTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As DrawingsTableAdapters.DrawingNotificationsTableAdapter
        Get
            If DrawingNotificationAdapter Is Nothing Then
                DrawingNotificationAdapter = New DrawingNotificationsTableAdapter()
            End If
            Return DrawingNotificationAdapter
        End Get
    End Property
    ''*****
    ''* Select DrawingNotifications returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetDrawingNotifications(ByVal DrawingNo As String) As Drawings.DrawingNotifications_MaintDataTable

        Try
            If DrawingNo Is Nothing Then
                DrawingNo = ""
            End If

            Return Adapter.GetDrawingNotifications(DrawingNo)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetDrawingNotifications : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> DrawingNotificationsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingDetail.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDrawingNotifications : " & commonFunctions.convertSpecialChar(ex.Message, False), "DrawingNotificationsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert DrawingNotification
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertDrawingNotification(ByVal DrawingNo As String, ByVal TeamMemberID As Integer) As Boolean

        Try
            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter.InsertDrawingNotification(DrawingNo, TeamMemberID, CreatedBy)
            'MsgBox("Rows Affected: " & rowsAffected)
            '' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & ", TeamMemberID:" & TeamMemberID & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertDrawingNotification : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> DrawingNotificationsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingDetails.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertDrawingNotification : " & commonFunctions.convertSpecialChar(ex.Message, False), "DrawingNotificationsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Delete DrawingNotification
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteDrawingNotification(ByVal DrawingNo As String, ByVal TeamMemberID As Integer, ByVal original_DrawingNo As String, ByVal original_TeamMemberID As Integer) As Boolean

        Try
            'Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter.DeleteDrawingNotification(original_DrawingNo, original_TeamMemberID)
            'MsgBox("Rows Affected: " & rowsAffected)
            '' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & ", TeamMemberID:" & TeamMemberID & "Original DrawingNo: " & original_DrawingNo & ", Original TeamMemberID:" & original_TeamMemberID & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteDrawingNotification : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> DrawingNotificationsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingDetails.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteDrawingNotification : " & commonFunctions.convertSpecialChar(ex.Message, False), "DrawingNotificationsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
