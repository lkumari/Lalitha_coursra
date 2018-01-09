''******************************************************************************************************
''* MakesBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 04/12/2008
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports MakesTableAdapters

<System.ComponentModel.DataObject()> _
Public Class MakesBLL
    Private MakesAdapter As MakeTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As MakesTableAdapters.MakeTableAdapter
        Get
            If MakesAdapter Is Nothing Then
                MakesAdapter = New MakeTableAdapter()
            End If
            Return MakesAdapter
        End Get
    End Property
    ''*****
    ''* Select Makes returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetMakes(ByVal MakeName As String) As Makes.Make_MaintDataTable

        Try
            If MakeName Is Nothing Then
                MakeName = ""
            End If

            Return Adapter.GetMakes(MakeName)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "MakeName: " & MakeName & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetMakes : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MakesBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/MakeMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("GetMakes : " & commonFunctions.convertSpecialChar(ex.Message, False), "MakesBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New Make
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertMake(ByVal MakeName As String, ByVal UGNBusiness As Boolean, ByVal createdBy As String) As Boolean

        Try
            createdBy = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Insert the Make record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertMake(MakeName, UGNBusiness, createdBy)

            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "MakeName: " & MakeName & ", createdBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertMake : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MakesBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/MakeMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertMake : " & commonFunctions.convertSpecialChar(ex.Message, False), "MakesBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ''*****
    ''* Update Make
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateMake(ByVal MakeName As String, ByVal UGNBusiness As Boolean, ByVal Obsolete As Boolean, ByVal UpdatedBy As String, ByVal original_MakeID As Integer) As Boolean

        Try
            UpdatedBy = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Update the Make record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateMake(original_MakeID, MakeName, UGNBusiness, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "MakeName: " & MakeName & ", createdBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateMake : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> MakesBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/DataMaintenance/MakeMaintenance.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateMake : " & commonFunctions.convertSpecialChar(ex.Message, False), "MakesBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
End Class
