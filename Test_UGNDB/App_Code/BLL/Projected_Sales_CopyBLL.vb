
''******************************************************************************************************
''* Projected_Sales_CopyBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Called From: Sales_Projection.aspx - btnCopy
''* Author  : LRey 05/22/2008
''* Modified: 06/21/2012 LRey   - Modified according to new BLL standards
''******************************************************************************************************

Imports Projected_SalesTableAdapters
<System.ComponentModel.DataObject()> _
Public Class Projected_Sales_CopyBLL
    Private psAdapter As Projected_Sales_Copy_TableAdapter = Nothing

    Protected ReadOnly Property Adapter() As Projected_SalesTableAdapters.Projected_Sales_Copy_TableAdapter
        Get
            If psAdapter Is Nothing Then
                psAdapter = New Projected_Sales_Copy_TableAdapter()
            End If
            Return psAdapter
        End Get
    End Property

    ''*****
    ''* Select Projected_Sales_Copy returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
        Public Function GetProjectedSalesCopy(ByVal SourcePartNo As String) As Projected_Sales.Projected_Sales_CopyDataTable

        Try
            If SourcePartNo Is Nothing Then
                SourcePartNo = ""
            End If

            Return Adapter.Get_Projected_Sales_Copy(SourcePartNo)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SourcePartNo: " & SourcePartNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetProjectedSalesCopy : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> Projected_Sales_CopyBLL :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Copy_Sales_Projection.aspx"
            UGNErrorTrapping.InsertErrorLog("GetProjectedSalesCopy : " & commonFunctions.convertSpecialChar(ex.Message, False), "Projected_Sales_CopyBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
       
    End Function

    ''*****
    ''* Insert a New row to Projected_Sales_Copy table
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertProjectedSalesCopy(ByVal SourcePartNo As String, ByVal DestinationPartNo As String) As Boolean
        Try

            ' Create a new pscpRow instance
            Dim psTable As New Projected_Sales.Projected_Sales_CopyDataTable
            Dim psRow As Projected_Sales.Projected_Sales_CopyRow = psTable.NewProjected_Sales_CopyRow
            Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ' Logical Rule - Cannot insert a record without null columns
            If SourcePartNo = Nothing And HttpContext.Current.Request.QueryString("sPartNo") = Nothing Then
                Throw New ApplicationException("Insert Cancelled: Source PartNo is a required field.")
            End If
            If DestinationPartNo = Nothing Then
                Throw New ApplicationException("Insert Cancelled: Destination PartNo is a required field.")
            End If

            ' Insert the new Projected_Sales_Copy row
            Dim rowsAffected As Integer = Adapter.sp_Insert_Projected_Sales_Copy(SourcePartNo, DestinationPartNo, User)

            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SourcePartNo: " & SourcePartNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertProjectedSalesCopy : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> Projected_Sales_CopyBLL :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Copy_Sales_Projection.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertProjectedSalesCopy : " & commonFunctions.convertSpecialChar(ex.Message, False), "Projected_Sales_CopyBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function

    ''*****
    ''* Delete Projected_Sales_Copy
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteProjectedSalesCopy(ByVal SourcePartNo As String, ByVal DestinationPartNo As String, ByVal original_SourcePartNo As String, ByVal original_DestinationPartNo As String) As Boolean
        Try
            Dim rowsAffected As Integer = Adapter.sp_Delete_Projected_Sales_Copy(SourcePartNo, original_DestinationPartNo)

            ' Return true if precisely one row was deleted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SourcePartNo: " & SourcePartNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteProjectedSalesCopy : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> Projected_Sales_CopyBLL :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Copy_Sales_Projection.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteProjectedSalesCopy : " & commonFunctions.convertSpecialChar(ex.Message, False), "Projected_Sales_CopyBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function

End Class

