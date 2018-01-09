''******************************************************************************************************
''* Projected_Sales_Customer_ProgramBLL.vb
''* This Business Logic Layer was developed to bind data to a gridview and/or drop down list options 
''* based on business rules or user's criteria. Available options are Select, Insert, Update & Delete.
''*
''* Called From: Sales_Projection.aspx - gvCustomerProgram
''* Author  : LRey 03/25/2008
''* Modified: LRey 08/12/2008 Added SoldTo to the insert/update/delete functions
''******************************************************************************************************

Imports Projected_SalesTableAdapters

<System.ComponentModel.DataObject()> _
Public Class Projected_Sales_Customer_ProgramBLL
    Private pscpAdapter As Projected_Sales_Customer_Program_TableAdapter = Nothing

    Protected ReadOnly Property Adapter() As Projected_SalesTableAdapters.Projected_Sales_Customer_Program_TableAdapter
        Get
            If pscpAdapter Is Nothing Then
                pscpAdapter = New Projected_Sales_Customer_Program_TableAdapter()
            End If
            Return pscpAdapter
        End Get
    End Property

    ''*****
    ''* Select Projected_Sales_Customer_Program returning all rows
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, True)> _
        Public Function GetProjectedSalesCustomerProgram(ByVal PartNo As String) As Projected_Sales.Projected_Sales_Customer_ProgramDataTable
        Try
            If PartNo = Nothing Then
                PartNo = ""
                'Throw New ApplicationException("Get Projected Sales Customer Program Cancelled: PartNo is a required field.")
            End If

            Return Adapter.Get_Projected_Sales_Customer_Program(PartNo)

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PartNo: " & PartNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetProjectedSalesCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> Projected_Sales_Customer_ProgramBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Sales_Projection_List.aspx"
            UGNErrorTrapping.InsertErrorLog("GetProjectedSalesCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "Projected_Sales_Customer_ProgramBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF GetProjectedSalesCustomerProgram

    ''*****
    ''* Insert a New row to Projected_Sales_Customer_Program table
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, True)> _
    Public Function InsertProjectedSalesCustomerProgram(ByVal PartNo As String, ByVal CABBV As String, ByVal SoldTo As Integer, ByVal ProgramID As Integer, ByVal UGNFacility As String, ByVal ProgramStatus As String, ByVal PiecesPerVehicle As Decimal, ByVal UsageFactorPerVehicle As Decimal) As Boolean
        Try

            ' Create a new pscpRow instance
            Dim pscpTable As New Projected_Sales.Projected_Sales_Customer_ProgramDataTable
            Dim pscpRow As Projected_Sales.Projected_Sales_Customer_ProgramRow = pscpTable.NewProjected_Sales_Customer_ProgramRow
            Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            ' Logical Rule - Cannot insert a record without null columns
            If PartNo = Nothing And HttpContext.Current.Request.QueryString("sPartNo") = Nothing Then
                Throw New ApplicationException("Insert Cancelled: PartNo is a required field.")
            End If
            If CABBV = Nothing Then
                Throw New ApplicationException("Insert Cancelled: Customer is a required field.")
            End If
            If SoldTo = Nothing Then
                SoldTo = 0
                'Throw New ApplicationException("Insert Cancelled: Sold To is a required field.")
            End If
            If ProgramID = Nothing Then
                Throw New ApplicationException("Insert Cancelled: Program is a required field.")
            End If
            If UGNFacility = Nothing Then
                Throw New ApplicationException("Insert Cancelled: UGN Facility is a required field.")
            End If

            ' Insert the new Projected_Sales_Customer_Program row
            Dim rowsAffected As Integer = Adapter.sp_Insert_Projected_Sales_Customer_Program(HttpContext.Current.Request.QueryString("sPartNo"), CABBV, SoldTo, ProgramID, UGNFacility, ProgramStatus, PiecesPerVehicle, UsageFactorPerVehicle, User)

            ' Return true if precisely one row was inserted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PartNo: " & PartNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertProjectedSalesCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> Projected_Sales_Customer_ProgramBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Sales_Projection_List.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertProjectedSalesCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "Projected_Sales_Customer_ProgramBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF InsertProjectedSalesCustomerProgram

    ''*****
    ''* Update Projected_Sales_Customer_Program
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, True)> _
Public Function UpdateProjectedSalesCustomerProgram(ByVal ProgramStatus As String, ByVal PiecesPerVehicle As Decimal, ByVal UsageFactorPerVehicle As Decimal, ByVal original_PartNo As String, ByVal original_CABBV As String, ByVal original_SoldTo As Integer, ByVal original_ProgramID As Integer, ByVal original_UGNFacility As String, ByVal ddCustomerValue As String, ByVal UGNFacility As String, ByVal CABBV As String, ByVal SoldTo As Integer, ByVal ProgramID As Integer) As Boolean

        Try
            Dim pscpTable As Projected_Sales.Projected_Sales_Customer_ProgramDataTable = Adapter.Get_Projected_Sales_Customer_Program(original_PartNo)
            Dim pscpRow As Projected_Sales.Projected_Sales_Customer_ProgramRow = pscpTable(0)
            Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Response.Cookies("returnToScreen").Value = Nothing
            Dim rowsAffected As Integer = 0
            If pscpTable.Count = 0 Then
                ' no matching record found, return false
                Return False
            End If

            Dim ds As DataSet = New DataSet
            Dim checkProgramID As Integer = 0
            Dim ProgramName As String = Nothing

            If original_ProgramID <> ProgramID Then
                checkProgramID = ProgramID
            Else
                checkProgramID = original_ProgramID
            End If

            ' Logical Rule - Cannot update a record without null columns
            If original_PartNo = Nothing Then
                Throw New ApplicationException("Update Cancelled: PartNo is a required field.")
            End If

            If CABBV = Nothing Then
                Throw New ApplicationException("Update Cancelled: Customer is a required field.")
            End If

            If SoldTo = Nothing Then
                SoldTo = 0
                'Throw New ApplicationException("Update Cancelled: Sold To is a required field.")
            End If

            If ProgramID = Nothing Then
                Throw New ApplicationException("Update Cancelled: Program is a required field.")
            End If

            If UGNFacility = Nothing Then
                Throw New ApplicationException("Update Cancelled: UGN Facility is a required field.")
            End If


            ds = commonFunctions.GetPlatformProgram(0, checkProgramID, "", "", "")
            If (ds.Tables.Item(0).Rows.Count > 0) Then
                ' no matching record found, return false
                HttpContext.Current.Response.Cookies("returnToScreen").Value = "~/PF/Sales_Projection.aspx?sPartNo=" & original_PartNo
                '' If ds.Tables(0).Rows(0).Item("ddProgramName").ToString.Substring(0, 2) = "**" Then
                If ds.Tables(0).Rows(0).Item("ddProgramModelPlatformAssembly").ToString.Substring(0, 2) = "**" Then
                    Throw New ApplicationException("Update Cancelled: Selected Program is Obsolete. Please correct or make another selection.<br/><br/>")
                    rowsAffected = -1
                Else
                    ' Update the Projected_Sales_Customer_Program record
                    rowsAffected = Adapter.sp_Update_Projected_Sales_Customer_Program(original_PartNo, CABBV, SoldTo, ProgramID, UGNFacility, ProgramStatus, PiecesPerVehicle, UsageFactorPerVehicle, original_CABBV, original_SoldTo, original_ProgramID, original_UGNFacility, User)

                End If
            End If

            ' Return true if precisely one row was updated, otherwise false
            Return rowsAffected


        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "original_PartNo: " & original_PartNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateProjectedSalesCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> Projected_Sales_Customer_ProgramBLL.vb :<br/> " & strUserEditedData
            If HttpContext.Current.Request.Cookies("returnToScreen").Value <> Nothing Then
                HttpContext.Current.Session("UGNErrorLastWebPage") = HttpContext.Current.Response.Cookies("returnToScreen").Value
            Else
                HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Sales_Projection_List.aspx"
            End If
            UGNErrorTrapping.InsertErrorLog("UpdateProjectedSalesCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "Projected_Sales_Customer_ProgramBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try

    End Function 'EOF UpdateProjectedSalesCustomerProgram

    ''*****
    ''* Delete Projected_Sales_Customer_Program
    ''*****
    <System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, True)> _
    Public Function DeleteProjectedSalesCustomerProgram(ByVal PartNo As String, ByVal CABBV As String, ByVal SoldTo As Integer, ByVal ProgramID As Integer, ByVal UGNFacility As String, ByVal original_CABBV As String, ByVal original_SoldTo As Integer, ByVal original_ProgramID As Integer, ByVal original_UGNFacility As String, ByVal original_PartNo As String) As Boolean
        Try

            If original_SoldTo = Nothing Then
                original_SoldTo = 0
            End If

            Dim rowsAffected As Integer = Adapter.sp_Delete_Projected_Sales_Customer_Program(original_PartNo, original_CABBV, original_SoldTo, original_ProgramID, original_UGNFacility)

            ' Return true if precisely one row was deleted, otherwise false
            Return rowsAffected = 1

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "PartNo: " & original_PartNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteProjectedSalesCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> Projected_Sales_Customer_ProgramBLL.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PF/Sales_Projection_List.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteProjectedSalesCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "Projected_Sales_Customer_ProgramBLL.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        End Try
    End Function 'EOF DeleteProjectedSalesCustomerProgram

End Class
