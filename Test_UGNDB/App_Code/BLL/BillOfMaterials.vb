''******************************************************************************************************
''* BillOfMaterialsBLL.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select and Update.
''*
''* Author  : RCarlson 05/21/2008
''* Modified: 12/18/2013    LRey    Replaced "BPCSPartNo" to "PartNo" wherever used. 
''******************************************************************************************************
Imports BillOfMaterialsTableAdapters

<System.ComponentModel.DataObject()> _
Public Class BillOfMaterials
    Private BillOfMaterialsAdapter As BillOfMaterialsTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As BillOfMaterialsTableAdapters.BillOfMaterialsTableAdapter
        Get
            If BillOfMaterialsAdapter Is Nothing Then
                BillOfMaterialsAdapter = New BillOfMaterialsTableAdapter()
            End If
            Return BillOfMaterialsAdapter
        End Get
    End Property
    '*****
    '* Select Bill Of Materials Part or Sub Part, returning all rows
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetBillOfMaterials(ByVal PartNo As String, ByVal SubPartNo As String) As BillOfMaterials.BillOfMaterials_ViewDataTable

        Try
            If PartNo Is Nothing Then
                PartNo = ""
            End If

            If SubPartNo Is Nothing Then
                SubPartNo = ""
            End If

            '' Return true if precisely one row was updated, otherwise false
            Return Adapter.GetBillOfMaterials(PartNo, SubPartNo)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PartNo: " & PartNo & ", SubPartNo:" & SubPartNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetBillOfMaterials : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> BillOfMaterialsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/BillOfMaterials.aspx"
            UGNErrorTrapping.InsertErrorLog("GetBillOfMaterials : " & commonFunctions.convertSpecialChar(ex.Message, False), "BillOfMaterialsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function

End Class
