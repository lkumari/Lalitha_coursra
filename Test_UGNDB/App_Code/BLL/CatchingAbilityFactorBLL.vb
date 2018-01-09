''******************************************************************************************************
''* CatchingAbilityFactorBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Author  : Roderick Carlson 02/05/2009
''* Modified: {Name} {Date} - {Notes}
''******************************************************************************************************

Imports CostingTableAdapters

<System.ComponentModel.DataObject()> _
Public Class CatchingAbilityFactorBLL
    Private CatchingAbilityFactorAdapter As CatchingAbilityFactorTableAdapter = Nothing
    Protected ReadOnly Property Adapter() As CostingTableAdapters.CatchingAbilityFactorTableAdapter
        Get
            If CatchingAbilityFactorAdapter Is Nothing Then
                CatchingAbilityFactorAdapter = New CatchingAbilityFactorTableAdapter()
            End If
            Return CatchingAbilityFactorAdapter
        End Get
    End Property
    ''*****
    ''* Select CatchingAbilityFactor returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
    Public Function GetCatchingAbilityFactor(ByVal FactorID As Integer) As Costing.CatchingAbilityFactor_MaintDataTable

        Try

            Return Adapter.GetCatchingAbilityFactor(FactorID, 0, 0, True)
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FactorID: " & FactorID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetCatchingAbilityFactor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CatchingAbilityFactorBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetCatchingAbilityFactor : " & commonFunctions.convertSpecialChar(ex.Message, False), "CatchingAbilityFactorBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function
    ''*****
    ''* Insert New CatchingAbilityFactor
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertCatchingAbilityFactor(ByVal MinimumPartLength As Double, _
        ByVal MaximumPartLength As Double, ByVal isSideBySide As Boolean, ByVal CatchingAbilityFactor As Double, _
        ByVal Obsolete As Boolean) As Boolean

        Try
            Dim createdBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ''*****
            ' Insert the record
            ''*****
            Dim rowsAffected As Integer = Adapter.InsertCatchingAbilityFactor(MinimumPartLength, MaximumPartLength, isSideBySide, CatchingAbilityFactor, Obsolete, createdBy)
            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "MinimumPartLength: " & MinimumPartLength & _
            ", MaximumPartLength: " & MaximumPartLength & ", isSideBySide: " & isSideBySide & _
            ", CatchingAbilityFactor: " & CatchingAbilityFactor & _
            ", Obsolete: " & Obsolete & _
            ", CreatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertCatchingAbilityFactor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CatchingAbilityFactorBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertCatchingAbilityFactor : " & commonFunctions.convertSpecialChar(ex.Message, False), "CatchingAbilityFactorBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    ' ''*****
    ''* Update CatchingAbilityFactor
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
        Public Function UpdateCatchingAbilityFactor(ByVal original_FactorID As Integer, ByVal MinimumPartLength As Double, _
        ByVal MaximumPartLength As Double, ByVal isSideBySide As Boolean, ByVal CatchingAbilityFactor As Double, _
        ByVal Obsolete As Boolean) As Boolean

        Try

            Dim UpdatedBy As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            ''*****
            ' Update the record
            ''*****
            Dim rowsAffected As Integer = Adapter.UpdateCatchingAbilityFactor(original_FactorID, MinimumPartLength, MaximumPartLength, isSideBySide, CatchingAbilityFactor, Obsolete, UpdatedBy)

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected = 1
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "FactorID:" & original_FactorID & ", MinimumPartLength: " & MinimumPartLength & _
            ", MaximumPartLength: " & MaximumPartLength & ", isSideBySide: " & isSideBySide & _
            ", CatchingAbilityFactor: " & CatchingAbilityFactor & _
            ", Obsolete: " & Obsolete & _
            ", UpdatedBy: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateCatchingAbilityFactor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CatchingAbilityFactorBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Costing/Cost_Sheet_List.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateCatchingAbilityFactor : " & commonFunctions.convertSpecialChar(ex.Message, False), "CatchingAbilityFactorBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return False
        End Try

    End Function
    
End Class
