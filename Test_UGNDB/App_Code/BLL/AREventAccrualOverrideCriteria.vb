''******************************************************************************************************
''* AREventAccrualOverrideCriteriaBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 06/06/2011
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports ARTableAdapters

<System.ComponentModel.DataObject()> _
Public Class AREventAccrualOverrideCriteriaBLL
    Private AREventAccrualOverrideCriteriaTableAdapter As AREventAccrualOverrideCriteriaTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As ARTableAdapters.AREventAccrualOverrideCriteriaTableAdapter
        Get
            If AREventAccrualOverrideCriteriaTableAdapter Is Nothing Then
                AREventAccrualOverrideCriteriaTableAdapter = New AREventAccrualOverrideCriteriaTableAdapter
            End If
            Return AREventAccrualOverrideCriteriaTableAdapter
        End Get
    End Property
    ''*****
    ''* Select AREventAccrualOverrideCriteria returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetAREventAccrualOverrideCriteria(ByVal AREID As Integer) As AR.AREventAccrualOverrideCriteriaDataTable

        Try

            Return Adapter.GetAREventAccrualOverrideCriteria(AREID)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetAREventAccrualOverrideCriteria : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> AREventAccrualOverrideCriteriaBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/AR/AR_Event_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetAREventAccrualOverrideCriteria : " & commonFunctions.convertSpecialChar(ex.Message, False), "AREventAccrualOverrideCriteriaBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function

    ' ''*****
    ''* Insert AREventAccrualOverrideCriteria
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertAREventAccrualOverrideCriteria(ByVal AREID As Integer, ByVal PartNo As String, ByVal PRCCDE As String, _
        ByVal Override_RELPRC As Double, ByVal StartShipDate As String, ByVal EndShipDate As String) As Boolean

        Try

            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = 0

            If PartNo Is Nothing Then
                PartNo = ""
            End If

            If PRCCDE Is Nothing Then
                PRCCDE = ""
            End If

            If StartShipDate Is Nothing Then
                StartShipDate = ""
            End If

            If EndShipDate Is Nothing Then
                EndShipDate = ""
            End If

            rowsAffected = Adapter.InsertAREventAccrualOverrideCriteria(AREID, PartNo, PRCCDE, _
                        Override_RELPRC, StartShipDate, EndShipDate, CreatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID:" & AREID _
             & ", PartNo:" & PartNo _
             & ", PRCCDE:" & PRCCDE _
             & ", Override_RELPRC:" & Override_RELPRC _
             & ", StartShipDate:" & StartShipDate _
             & ", EndShipDate:" & EndShipDate _
             & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertAREventAccrualOverrideCriteria : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> AREventAccrualOverrideCriteriaBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/AR/AR_Event_List.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertAREventAccrualOverrideCriteria : " & commonFunctions.convertSpecialChar(ex.Message, False), "AREventAccrualOverrideCriteriaBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

    ' ''*****
    ''* Update AREventAccrualOverrideCriteria
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateAREventAccrualOverrideCriteria(ByVal AREID As Integer, _
        ByVal Override_RELPRC As Double, ByVal StartShipDate As String, ByVal EndShipDate As String, _
        ByVal original_RowID As Integer, ByVal PartNo As String, ByVal ddPriceCodeName As String) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = 0

            If StartShipDate Is Nothing Then
                StartShipDate = ""
            End If

            If EndShipDate Is Nothing Then
                EndShipDate = ""
            End If

            rowsAffected = Adapter.UpdateAREventAccrualOverrideCriteria(original_RowID, AREID, _
                        Override_RELPRC, StartShipDate, EndShipDate, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID _
             & ", AREID:" & AREID _
             & ", Override_RELPRC:" & Override_RELPRC _
             & ", StartShipDate:" & StartShipDate _
             & ", EndShipDate:" & EndShipDate _
             & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateAREventAccrualOverrideCriteria : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> AREventAccrualOverrideCriteriaBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/AR/AR_Event_List.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateAREventAccrualOverrideCriteria : " & commonFunctions.convertSpecialChar(ex.Message, False), "AREventAccrualOverrideCriteriaBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

    ''*****
    '* Delete AREventAccrualOverrideCriteria
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteAREventAccrualOverrideCriteria(ByVal AREID As Integer, ByVal original_RowID As Integer) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = Adapter.DeleteAREventAccrualOverrideCriteria(original_RowID, AREID, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID _
             & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteAREventAccrualOverrideCriteria : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> AREventAccrualOverrideCriteriaBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/AR/AR_Event_List.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteAREventAccrualOverrideCriteria : " & commonFunctions.convertSpecialChar(ex.Message, False), "AREventAccrualOverrideCriteriaBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
