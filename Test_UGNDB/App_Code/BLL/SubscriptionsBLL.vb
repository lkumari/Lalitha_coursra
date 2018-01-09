''******************************************************************************************************
''* SubscriptionsBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : LRey 02/26/2008
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports SubscriptionsTableAdapters

<System.ComponentModel.DataObject()> _
Public Class SubscriptionsBLL
    Private subscriptionsAdapter As SubscriptionsTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As SubscriptionsTableAdapters.SubscriptionsTableAdapter
        Get
            If subscriptionsAdapter Is Nothing Then
                subscriptionsAdapter = New SubscriptionsTableAdapter()
            End If
            Return subscriptionsAdapter
        End Get
    End Property
    ''*****
    ''* Select Subscriptions returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetSubscriptions(ByVal Subscription As String) As Subscriptions.Subscriptions_MaintDataTable
        Try
            Return Adapter.GetSubscriptions(Subscription)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "Subscription: " & Subscription & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetSubscriptions : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SubscriptionsBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Workflow/Subscriptions.aspx"

            UGNErrorTrapping.InsertErrorLog("GetSubscriptions : " & commonFunctions.convertSpecialChar(ex.Message, False), "SubscriptionsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ' ''*****
    ' ''* Select Subscriptions using the SubscriptionID column
    ' ''*****
    '<System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, False)> _
    'Public Function GetSubscriptionsDataByID(ByVal subscriptionID As Integer) As Subscriptions.Subscriptions_MaintDataTable
    '    Return Adapter.GetSubscriptionsDataByID(subscriptionID)
    'End Function
    '    ''*****
    '    ''* Select Subscriptions using the Subscriptions column
    '    ''*****
    '    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, False)> _
    'Public Function GetSubscriptionsDataByDesc(ByVal subscription As Integer) As Subscriptions.Subscriptions_MaintDataTable
    '        Return Adapter.GetSubscriptionsDataByDesc(subscription)
    '    End Function

    ''*****
    ''* Insert New Subscriptions
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function AddSubscription(ByVal Subscription As String, ByVal createdBy As String) As Boolean
        Try
            ' Create a new pscpRow instance
            Dim pTable As New Subscriptions.Subscriptions_MaintDataTable
            Dim pscpRow As Subscriptions.Subscriptions_MaintRow = pTable.NewSubscriptions_MaintRow
            Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ' Logical Rule - Cannot insert a record without null columns
            If Subscription = Nothing Then
                Throw New ApplicationException("Insert Cancelled: Subscription is a required field.")
            End If

            ' Insert the new Subscriptions_Maint row
            Dim rowsAffected As Integer = Adapter.sp_Insert_Subscriptions_Maint(Subscription, User)

            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "Subscription: " & Subscription & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "AddSubscription : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SubscriptionsBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Workflow/Subscriptions.aspx"

            UGNErrorTrapping.InsertErrorLog("AddSubscription : " & commonFunctions.convertSpecialChar(ex.Message, False), "SubscriptionsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF AddSubscription

    ''*****
    ''* Update Subscriptions
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateSubscription(ByVal subscription As String, ByVal Obsolete As Boolean, ByVal UpdatedBy As String, ByVal original_SubscriptionID As Integer) As Boolean
        Try

            Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter.sp_Update_Subscriptions_Maint(subscription, Obsolete, User, original_SubscriptionID)

            Return rowsAffected = 1

           
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "Subscription: " & subscription & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateSubscription : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SubscriptionsBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Workflow/Subscriptions.aspx"

            UGNErrorTrapping.InsertErrorLog("UpdateSubscription : " & commonFunctions.convertSpecialChar(ex.Message, False), "SubscriptionsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF UpdateSubscription

    ''*****
    ''* Delete Subscriptions
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteSubscription(ByVal SubscriptionID As Integer) As Boolean
        Try
            Dim rowsAffected As Integer = Adapter.sp_Delete_Subscriptions_Maint(subscriptionID)

            ' Return true if precisely one row was deleted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "Subscription: " & SubscriptionID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteSubscription : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> SubscriptionsBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Workflow/Subscriptions.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteSubscription : " & commonFunctions.convertSpecialChar(ex.Message, False), "SubscriptionsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF DeleteSubscription
End Class
