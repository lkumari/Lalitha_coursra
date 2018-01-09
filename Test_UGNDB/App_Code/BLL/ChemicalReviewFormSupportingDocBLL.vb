''******************************************************************************************************
''* ChemicalReviewFormSupportingDocBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 2/10/2010
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports ChemicalReviewFormTableAdapters

<System.ComponentModel.DataObject()> _
Public Class ChemicalReviewFormSupportingDocBLL
    Private ChemicalReviewFormSupportingDocAdapter As ChemicalReviewFormSupportingDocTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As ChemicalReviewFormTableAdapters.ChemicalReviewFormSupportingDocTableAdapter
        Get
            If ChemicalReviewFormSupportingDocAdapter Is Nothing Then
                ChemicalReviewFormSupportingDocAdapter = New ChemicalReviewFormSupportingDocTableAdapter()
            End If
            Return ChemicalReviewFormSupportingDocAdapter
        End Get
    End Property
    ''*****
    ''* Select ChemicalReviewFormSupportingDoc returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetChemicalReviewFormSupportingDoc(ByVal ChemRevFormID As Integer) As ChemicalReviewForm.ChemicalReviewFormSupportingDoc_MaintDataTable

        Try

            Return Adapter.GetChemicalReviewFormSupportingDocList(ChemRevFormID)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ChemRevFormID: " & ChemRevFormID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetChemicalReviewFormSupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ChemicalReviewFormSupportingDocBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ChemicalReviewForm/ChemicalReviewForm_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetChemicalReviewFormSupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False), "ChemicalReviewFormSupportingDocBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function

    ''*****
    ''* Delete ChemicalReviewFormSupportingDoc
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
        Public Function DeleteChemicalReviewFormSupportingDoc(ByVal RowID As Integer, ByVal original_RowID As Integer) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Obsolete the record
            ''*****
            Dim rowsAffected As Integer = Adapter.DeleteChemicalReviewFormSupportingDoc(original_RowID, UpdatedBy)

            ' Return true if Postcisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID:" & original_RowID & ", UpdatedBy: " & _
            HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteChemicalReviewFormSupportingDoc : " & _
            commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> ChemicalReviewFormSupportingDocBLL.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/ChemicalReviewForm/ChemicalReviewForm_List.aspx"

            UGNErrorTrapping.InsertErrorLog("DeleteChemicalReviewFormSupportingDoc : " & _
            commonFunctions.convertSpecialChar(ex.Message, False), "ChemicalReviewFormSupportingDocBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            Return False
        End Try

    End Function

End Class
