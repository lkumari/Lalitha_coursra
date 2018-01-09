''******************************************************************************************************
''* AREventDetailBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 07/08/2010
''* Modified: {Name} {Date} - {Notes}
''*           Roderick Carlson 08/14/2012 - Added Estimated Price
''******************************************************************************************************

Imports ARTableAdapters

<System.ComponentModel.DataObject()> _
Public Class AREventDetailBLL
    Private AREventDetailTableAdapter As AREventDetailTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As ARTableAdapters.AREventDetailTableAdapter
        Get
            If AREventDetailTableAdapter Is Nothing Then
                AREventDetailTableAdapter = New AREventDetailTableAdapter
            End If
            Return AREventDetailTableAdapter
        End Get
    End Property
    ''*****
    ''* Select AREventDetail returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetAREventDetail(ByVal AREID As Integer) As AR.AREventDetailDataTable

        Try

            Return Adapter.GetAREventDetail(AREID)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID: " & AREID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetAREventDetail: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> AREventDetailBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/AR/AR_Event_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetAREventDetail: " & commonFunctions.convertSpecialChar(ex.Message, False), "AREventDetailBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert AREventDetail
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function InsertAREventDetail(ByVal AREID As Integer, ByVal EventTypeID As Integer, ByVal COMPNY As String, _
                                        ByVal Customer As String, ByVal PARTNO As String, ByVal PRCCDE As String, ByVal PRCPRNT As Double, _
                                        ByVal PRCDOLR As Double, ByVal USE_RELPRC As Double, _
                                        ByVal ESTPRC As Double) As Boolean

        Try

            Dim CreatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = 0
            Dim dNewPrice As Double = 0

            If EventTypeID = 2 And PRCPRNT <> 0 Then
                dNewPrice = 0
            Else
                dNewPrice = PRCDOLR
            End If

            rowsAffected = Adapter.InsertAREventDetailPriceByRow(AREID, COMPNY, Customer, PARTNO, "", "", PRCCDE, PRCPRNT, dNewPrice, USE_RELPRC, 0, ESTPRC, CreatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "AREID:" & AREID _
             & ", USE_RELPRC:" & USE_RELPRC _
             & ", EventTypeID:" & EventTypeID _
             & ", PRCPRNT:" & PRCPRNT _
             & ", PRCDOLR:" & PRCDOLR _
             & ", ESTPRC:" & ESTPRC _
             & ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateAREventDetail: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> AREventDetailBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/AR/AR_Event_List.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateAREventDetail : " & commonFunctions.convertSpecialChar(ex.Message, False), "AREventDetailBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

    ''*****
    ''* Update AREventDetail
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
    Public Function UpdateAREventDetail(ByVal AREID As Integer, ByVal EventTypeID As Integer, _
    ByVal USE_RELPRC As Double, ByVal PRCPRNT As Double, ByVal PRCDOLR As Double, ByVal ESTPRC As Double, _
    ByVal original_RowID As Integer, ByVal PartNo As String) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            Dim rowsAffected As Integer = 0
            Dim dNewPrice As Double = 0

            If EventTypeID = 2 And PRCPRNT <> 0 Then
                dNewPrice = 0
            Else
                dNewPrice = PRCDOLR
            End If

            rowsAffected = Adapter.UpdateAREventDetailPriceByRow(original_RowID, AREID, USE_RELPRC, PRCPRNT, dNewPrice, ESTPRC, UpdatedBy)
            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID _
             & ", AREID:" & AREID _
             & ", USE_RELPRC:" & USE_RELPRC _
             & ", EventTypeID:" & EventTypeID _
             & ", PRCPRNT:" & PRCPRNT _
             & ", PRCDOLR:" & PRCDOLR _
             & ", ESTPRC:" & ESTPRC _
             & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateAREventDetail: " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> AREventDetailBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/AR/AR_Event_List.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateAREventDetail : " & commonFunctions.convertSpecialChar(ex.Message, False), "AREventDetailBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function

    ''*****
    '* Delete AREventDetail
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteAREventDetail(ByVal AREID As Integer, ByVal original_RowID As Integer) As Boolean

        Try

            Dim rowsAffected As Integer = Adapter.DeleteAREventDetailByRow(original_RowID, AREID)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID _
             & ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteAREventDetail : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> AREventDetailBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/AR/AR_Event_List.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteAREventDetail : " & commonFunctions.convertSpecialChar(ex.Message, False), "AREventDetailBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
