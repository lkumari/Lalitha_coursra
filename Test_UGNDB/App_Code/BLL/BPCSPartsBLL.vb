''******************************************************************************************************
''* BPCSPartsBLL.vp
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select and Update.
''*
''* Author  : Roderick Carlson 03/25/2008
''* Modified: {Name} {Date} - {Notes}
''* Modified: Roderick Carlson 02/02/2009 - removed PreviousBPCSPartNo, removed Update Function
''* Modified: 12/18/2013    LRey    Replaced "BPCS Part No" to "Part No" wherever used. 
''******************************************************************************************************
Imports BPCSPartsTableAdapters

<System.ComponentModel.DataObject()> _
Public Class BPCSParts
    Private BPCSPartAdapter As BPCSPartTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As BPCSPartsTableAdapters.BPCSPartTableAdapter
        Get
            If BPCSPartAdapter Is Nothing Then
                BPCSPartAdapter = New BPCSPartTableAdapter()
            End If
            Return BPCSPartAdapter
        End Get
    End Property
    '*****
    '* Select BPCS Parts returning all rows
    '*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetBPCSParts(ByVal PartNo As String, ByVal PartName As String, ByVal DrawingNo As String, ByVal DesignationType As String, ByVal ActiveType As String) As BPCSParts.BPCSPart_MaintDataTable

        Try
            If PartNo Is Nothing Then
                PartNo = ""
            End If

            If PartName Is Nothing Then
                PartName = ""
            End If

            If DrawingNo Is Nothing Then
                DrawingNo = ""
            End If

            If DesignationType Is Nothing Then
                DesignationType = ""
            End If

            If ActiveType Is Nothing Then
                ActiveType = ""
            End If

            '' Return true if precisely one row was updated, otherwise false
            Return Adapter.GetBPCSParts(PartNo, PartName, DrawingNo, DesignationType, ActiveType)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PartNo: " & PartNo & ", PartName: " & PartName & ", DrawingNo: " & DrawingNo & ", DesignationType: " & DesignationType & ", ActiveType: " & ActiveType & ", User:" & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetBPCSParts : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> BPCSPartsBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/PartMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("GetBPCSParts : " & commonFunctions.convertSpecialChar(ex.Message, False), "BPCSPartsBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function

End Class
