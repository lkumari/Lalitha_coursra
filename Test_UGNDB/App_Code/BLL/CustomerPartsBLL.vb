''******************************************************************************************************
''* CustomerPartsBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select and Update.
''*
''* Author  : Roderick Carlson 04/16/2008
''* Modified: {Name} {Date} - {Notes}
''* Modified: Roderick Carlson 07/22/2008 - Added Error Trapping, adjusted to point to View instead of Maintenance Table 
''* Modified: Roderick Carlson 04/22/2010 - Added BarCodePartNo
''******************************************************************************************************
Imports CustomerPartsTableAdapters

<System.ComponentModel.DataObject()> _
Public Class CustomerPartsBLL
    Private CustomerPartAdapter As CustomerPartBPCSPartRelateTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CustomerPartsTableAdapters.CustomerPartBPCSPartRelateTableAdapter
        Get
            If CustomerPartAdapter Is Nothing Then
                CustomerPartAdapter = New CustomerPartBPCSPartRelateTableAdapter()
            End If
            Return CustomerPartAdapter
        End Get
    End Property
    '*****
    '* Select Customer Parts returning all rows
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetCustomerParts(ByVal BPCSPartNo As String, ByVal CustomerPartNo As String, ByVal CustomerPartName As String, _
        ByVal CABBV As String, ByVal BarCodePartNo As String) As CustomerParts.CustomerPartBPCSPart_RelateDataTable

        Try
            If BPCSPartNo Is Nothing Then
                BPCSPartNo = ""
            End If

            If CustomerPartNo Is Nothing Then
                CustomerPartNo = ""
            End If

            If CustomerPartName Is Nothing Then
                CustomerPartName = ""
            End If

            If CABBV Is Nothing Then
                CABBV = ""
            End If

            If BarCodePartNo Is Nothing Then
                BarCodePartNo = ""
            End If

            '' Return true if precisely one row was updated, otherwise false
            Return Adapter.GetCustomerParts(BPCSPartNo, CustomerPartNo, CustomerPartName, CABBV, BarCodePartNo)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "BPCSPartNo:" & BPCSPartNo & ", CustomerPartNo: " & CustomerPartNo _
            & ", CABBV: " & CABBV & ", BarCodePartNo: " & BarCodePartNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetCustomerParts : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CustomerPartsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/CustomerMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCustomerParts : " & commonFunctions.convertSpecialChar(ex.Message, False), "CustomerPartsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx")
            Return Nothing
        End Try

    End Function
End Class
