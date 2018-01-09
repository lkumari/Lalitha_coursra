''************************************************************************************************
'Name:		PEModule.vb
'Purpose:	This code is referenced from all vb files, mostly for the purpose of calling stored 
'          procedures or getting user-rights for the Product Engineering Module
'
'Date		    Author	    
'06/03/2008    Roderick Carlson			Created .Net application
'10/20/2008    Roderick Carlson            Added CABBV to getDrawing, insertDrawing, UpdateDrawing
'10/23/2008    Roderick Carlson            Added Packaging Info to Update Drawing
'11/10/2008    Roderick Carlson            Switch Roll Length and Roll Width Labels
'11/19/2008    Roderick Carlson            For some functions, check special characters first, commonFunctions.convertSpecialChar
'12/08/2008    Roderick Carlson            Set timeout of StartTempDrawings to 120 seconds
'01/14/2009    Roderick Carlson            Added Functions To Get Previous and Next Drawing Revisions
'05/28/2009    Roderick Carlson            PDE # 2715 - added vehicle year to get, insert, update Drawing
'06/04/2009    Roderick Carlson            Added SoldTo and DesignationType to a lot of functions
'08/21/2009    Roderick Carlson            Added BPCS Info to SubTable
'08/28/2009    Roderick Carlson            Added Future UGBDB Vendor ID
'08/28/2009    Roderick Carlson            Added Insert and Update Drawing Customer Program Functions
'09/03/2009    Roderick Carlson            Customer and Program are in subtable now
'09/17/2009    Roderick Carlson            Added Insert Drawing BPCS function
'09/22/2009    Roderick Carlson            Added GetDrawingSearch Function
'10/12/2009    Roderick Carlson            PDE-2761 - Added link to upload Customer Drawing Image
'10/14/2009    Roderick Carlson            Added Customer DrawingNo field to Insert Customer Drawing Image
'12/14/2009    Roderick Carlson            PDE-2803 - Added Functions for Product Development Test Pages
'01/05/2010    Roderick Carlson            PDE-2807 - Added Function UpdateSubDrawing
'01/27/2010    Roderick Carlson            PDE-2816 - Added Function GetApprovedVendor, GetUnapprovedVendor,CopyApprovedVendor, CopyUnapprovedVendor
'02/08/2010    Roderick Carlson            Added CopyDrawingImage and GenerateDrawingNo functions (to work with RFD and upgrade of DMS later)
'02/24/2010    Roderick Carlson            Added Product Technology to GetDrawingSearch and PEDeleteCookies
'03/02/2010    Roderick Carlson            Added DrawingNo to Prod Dev Test page / Validation Engineering
'04/19/2010    Roderick Carlson            Adjusted GetDrawing to hide parameters
'06/28/2010    Roderick Carlson            PDE-2909 - Release Type Work
'02/04/2011    Roderick Carlson            Added logic to GetPreviousDrawing and GetNextDrawing if a middle revision was obsolete
'02/07/2011    Roderick Carlson            Added functions to Copy Bill of Materials, and Replace SubDrawing
'02/11/2011    Roderick Carlson            Added functions - get Drawing Max Revision and get Drawing Max Step
'03/02/2011    Roderick Carlson            Added Material Specification Functions
'04/04/2011    Roderick Carlson            Disable Material Specification Functions until further development - see Region DrawingMaterialSpecifications
'07/25/2011    Roderick Carlson            Added function to Append Drawing Revision Notes 
'08/26/2011    Roderick Carlson            Added DrawingNo parameter to GetMaterialSpeciNoSearch 
'10/18/2011    Roderick Carlson            Added Copy_Drawing_Material_Spec_Relate_By_Drawing function
'11/01/2011    Roderick Carlson            Removed External Drawing Functions and ValidationEngineering/ProdDev Test functions
'11/29/2011    Roderick Carlson            Added Function to Delete Subdrawing by Parent Drawing
'11/30/2012    Roderick Carlson            Removed function GetDrawingNameInfo
'12/10/2012    Roderick Carlson            Cleaned up GetSubDrawing some
'12/18/2013    LRey                        Replaced "BPCSPartNo" to "PartNo" wherever used. 
'12/19/2013    LRey                        Replaced "SoldTo|CABBV" to "PartNo" wherever used. 
''************************************************************************************************

Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Text
Imports System.Xml
Imports System.IO
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.Page
Imports System.Web.UI.WebControls
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports Microsoft.VisualBasic

Public Class PEModule
    Public Shared Sub CleanPEDMScrystalReports()

        Try
            Dim tempRpt As ReportDocument = New ReportDocument()
            'in order to clear crystal reports for Drawing Preview
            If HttpContext.Current.Session("DMSDrawingPreview") IsNot Nothing Then

                tempRpt = CType(HttpContext.Current.Session("DMSDrawingPreview"), ReportDocument)
                If tempRpt IsNot Nothing Then
                    tempRpt.Close()
                    tempRpt.Dispose()
                End If
                HttpContext.Current.Session("DMSDrawingPreview") = Nothing
                GC.Collect()
            End If

            'in order to clear crystal reports for Drawing Packaging Info
            If HttpContext.Current.Session("DrawingPackagingPreview") IsNot Nothing Then

                tempRpt = CType(HttpContext.Current.Session("DrawingPackagingPreview"), ReportDocument)
                If tempRpt IsNot Nothing Then
                    tempRpt.Close()
                    tempRpt.Dispose()
                End If
                HttpContext.Current.Session("DrawingPackagingPreview") = Nothing
                GC.Collect()
            End If

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "CleanPEDMScrystalReports : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CleanPEDMScrystalReports : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub
    Public Shared Sub InsertDrawingCustomerProgram(ByVal DrawingNo As String, ByVal Customer As String, _
   ByVal ProgramID As Integer, ByVal ProgramYear As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Drawing_Customer_Program"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@DrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@DrawingNo").Value = DrawingNo

            myCommand.Parameters.Add("@Customer", SqlDbType.VarChar)
            myCommand.Parameters("@Customer").Value = Customer

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@ProgramYear", SqlDbType.Int)
            myCommand.Parameters("@ProgramYear").Value = ProgramYear

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & ", Customer: " & Customer _
            & ", ProgramID: " & ProgramID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertDrawingCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingDetail.aspx?DrawingNo=" & DrawingNo
            UGNErrorTrapping.InsertErrorLog("InsertDrawingCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub InsertDrawingBPCS(ByVal DrawingNo As String, ByVal PartNo As String, ByVal PartRevision As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Drawing_BPCS"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@DrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@DrawingNo").Value = DrawingNo

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            myCommand.Parameters.Add("@PartRevision", SqlDbType.VarChar)
            myCommand.Parameters("@PartRevision").Value = PartRevision

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & ", PartNo: " & PartNo _
            & ", PartRevision: " & PartRevision & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertDrawingBPCS : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingDetail.aspx?DrawingNo=" & DrawingNo
            UGNErrorTrapping.InsertErrorLog("InsertDrawingBPCS : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub UpdateDrawingCustomerProgram(ByVal RowID As Integer, ByVal Customer As String, ByVal ProgramID As Integer, ByVal ProgramYear As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Drawing_Customer_Program"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RowID", SqlDbType.Int)
            myCommand.Parameters("@RowID").Value = RowID

            myCommand.Parameters.Add("@Customer", SqlDbType.VarChar)
            myCommand.Parameters("@Customer").Value = Customer

            myCommand.Parameters.Add("@ProgramYear", SqlDbType.Int)
            myCommand.Parameters("@ProgramYear").Value = ProgramYear

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = ProgramID

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & RowID & ", Customer: " & Customer _
            & ", ProgramID: " & ProgramID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateDrawingCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateDrawingCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub UpdateDrawingCustomerImage(ByVal DrawingNo As String, ByVal CustomerDrawingNo As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Drawing_Customer_Images"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@DrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@DrawingNo").Value = DrawingNo

            myCommand.Parameters.Add("@CustomerDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@CustomerDrawingNo").Value = CustomerDrawingNo

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & ", CustomerDrawingNo: " & CustomerDrawingNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateDrawingCustomerImage : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingDetail.aspx?DrawingNo=" & DrawingNo
            UGNErrorTrapping.InsertErrorLog("UpdateDrawingCustomerImage : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Function GetPreviousDrawingRevision(ByVal CurrentDrawingNo As String) As String

        Try
            Dim ds As DataSet

            Dim bFound As Boolean = False

            Dim iFirstLeftParenthesisLocation As Integer = 0
            Dim strDrawingNoWithoutRevision As String = ""
            Dim strRevision As String = ""
            Dim iCurrentRevision As Integer = 0
            Dim iPreviousRevision As Integer = 0
            Dim strPreviousRevision As String = ""
            Dim strPreviousDrawing As String = ""


            'find the first left parenthesis
            iFirstLeftParenthesisLocation = InStr(CurrentDrawingNo, "(")

            'get the drawing number without the revision
            strDrawingNoWithoutRevision = Mid$(CurrentDrawingNo, 1, iFirstLeftParenthesisLocation)

            'get the numbers between the parenthesis
            strRevision = Left$(Right$(CurrentDrawingNo, 3), 2)

            'convert string to integer
            iCurrentRevision = CInt(strRevision)

            GetPreviousDrawingRevision = ""

            If iCurrentRevision > 0 Then
                iPreviousRevision = iCurrentRevision - 1

                While iPreviousRevision >= 0 And bFound = False
                    strPreviousRevision = CStr(iPreviousRevision).PadLeft(2, "0")
                    strPreviousDrawing = strDrawingNoWithoutRevision + strPreviousRevision + ")"

                    'determine if previous Drawing Number exists
                    ds = PEModule.GetDrawing(strPreviousDrawing)

                    If commonFunctions.CheckDataSet(ds) = True Then
                        If ds.Tables(0).Rows(0).Item("Obsolete") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("Obsolete") = False Then
                                GetPreviousDrawingRevision = strPreviousDrawing
                                bFound = True
                            End If
                        End If
                    End If

                    iPreviousRevision -= 1

                End While

            End If

        Catch ex As Exception
            GetPreviousDrawingRevision = ""

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & CurrentDrawingNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetPreviousDrawingRevision : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingDetail.aspx?DrawingNo=" & CurrentDrawingNo
            UGNErrorTrapping.InsertErrorLog("GetPreviousDrawingRevision : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        End Try

    End Function
    Public Shared Function GetNextDrawingRevision(ByVal CurrentDrawingNo As String) As String

        Try
            Dim ds As DataSet

            Dim iFirstLeftParenthesisLocation As Integer = 0
            Dim strDrawingNoWithoutRevision As String = ""
            Dim strRevision As String = ""
            Dim iCurrentRevision As Integer = 0
            Dim iNextRevision As Integer = 0
            Dim strNextRevision As String = ""
            Dim strNextDrawing As String = ""

            Dim bFound As Boolean = False

            'find the first left parenthesis
            iFirstLeftParenthesisLocation = InStr(CurrentDrawingNo, "(")

            'get the drawing number without the revision
            strDrawingNoWithoutRevision = Mid$(CurrentDrawingNo, 1, iFirstLeftParenthesisLocation)

            'get the numbers between the parenthesis
            strRevision = Left$(Right$(CurrentDrawingNo, 3), 2)

            'convert string to integer
            iCurrentRevision = CInt(strRevision)

            GetNextDrawingRevision = ""

            If iCurrentRevision < 99 Then
                iNextRevision = iCurrentRevision + 1

                While iNextRevision <= 99 And bFound = False
                    strNextRevision = CStr(iNextRevision).PadLeft(2, "0")
                    strNextDrawing = strDrawingNoWithoutRevision + strNextRevision + ")"

                    'determine if next Drawing Number exists
                    ds = PEModule.GetDrawing(strNextDrawing)

                    If commonFunctions.CheckDataSet(ds) = True Then
                        If ds.Tables(0).Rows(0).Item("Obsolete") IsNot System.DBNull.Value Then
                            If ds.Tables(0).Rows(0).Item("Obsolete") = False Then
                                GetNextDrawingRevision = strNextDrawing
                                bFound = True
                            End If
                        End If
                    End If

                    iNextRevision += 1
                End While

            End If

        Catch ex As Exception
            GetNextDrawingRevision = ""

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & CurrentDrawingNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetNextDrawingRevision : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingDetail.aspx?DrawingNo=" & CurrentDrawingNo
            UGNErrorTrapping.InsertErrorLog("GetNextDrawingRevision : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        End Try

    End Function
    Public Shared Sub CopyDrawingCustomerProgram(ByVal NewDrawingNo As String, ByVal OldDrawingNo As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Drawing_Customer_Program"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try

            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@NewDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@NewDrawingNo").Value = NewDrawingNo

            myCommand.Parameters.Add("@OldDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@OldDrawingNo").Value = OldDrawingNo

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewDrawingNo: " & NewDrawingNo & ", OldDrawingNo:" & OldDrawingNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "CopyDrawingCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyDrawingCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub CopyDrawingDrawingMaterialSpecRelateByDrawing(ByVal NewDrawingNo As String, ByVal OldDrawingNo As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Drawing_Material_Spec_Relate_By_Drawing"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try

            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@NewDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@NewDrawingNo").Value = NewDrawingNo

            myCommand.Parameters.Add("@OldDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@OldDrawingNo").Value = OldDrawingNo

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewDrawingNo: " & NewDrawingNo & ", OldDrawingNo:" & OldDrawingNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "CopyDrawingDrawingMaterialSpecRelateByDrawing : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyDrawingDrawingMaterialSpecRelateByDrawing : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub CopyDrawingApprovedVendor(ByVal NewDrawingNo As String, ByVal OldDrawingNo As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Drawing_Approved_Vendor"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try

            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@NewDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@NewDrawingNo").Value = NewDrawingNo

            myCommand.Parameters.Add("@OldDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@OldDrawingNo").Value = OldDrawingNo

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewDrawingNo: " & NewDrawingNo & ", OldDrawingNo:" & OldDrawingNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "CopyDrawingApprovedVendor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyDrawingApprovedVendor : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub CopyDrawingUnapprovedVendor(ByVal NewDrawingNo As String, ByVal OldDrawingNo As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Drawing_Unapproved_Vendor"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try

            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@NewDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@NewDrawingNo").Value = NewDrawingNo

            myCommand.Parameters.Add("@OldDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@OldDrawingNo").Value = OldDrawingNo

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewDrawingNo: " & NewDrawingNo & ", OldDrawingNo:" & OldDrawingNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "CopyDrawingUnapprovedVendor : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyDrawingApprovedVendor : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub CopyDrawingBOM(ByVal NewParentDrawingNo As String, ByVal OldParentDrawingNo As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Drawing_BOM"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try

            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@NewParentDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@NewParentDrawingNo").Value = NewParentDrawingNo

            myCommand.Parameters.Add("@OldParentDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@OldParentDrawingNo").Value = OldParentDrawingNo

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewParentDrawingNo: " & NewParentDrawingNo _
            & ", OldParentDrawingNo:" & OldParentDrawingNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "CopyDrawingBOM : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyDrawingBOM : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub ReplaceSubDrawing(ByVal NewParentDrawingNo As String, _
        ByVal NewChildDrawingNo As String, ByVal OldChildDrawingNo As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Replace_SubDrawing"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try

            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@NewParentDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@NewParentDrawingNo").Value = NewParentDrawingNo

            myCommand.Parameters.Add("@NewChildDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@NewChildDrawingNo").Value = NewChildDrawingNo

            myCommand.Parameters.Add("@OldChildDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@OldChildDrawingNo").Value = OldChildDrawingNo

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "NewParentDrawingNo: " & NewParentDrawingNo _
            & ", NewChildDrawingNo:" & NewChildDrawingNo _
            & ", OldChildDrawingNo:" & OldChildDrawingNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "ReplaceSubDrawing : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"
            UGNErrorTrapping.InsertErrorLog("ReplaceSubDrawing : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub AppendDrawingRevisionNotes(ByVal DrawingNo As String, ByVal AppendedRevisionNotes As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Append_Drawing_Revision_Notes"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Try

            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@DrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@DrawingNo").Value = DrawingNo

            myCommand.Parameters.Add("@AppendedRevisionNotes", SqlDbType.VarChar)
            myCommand.Parameters("@AppendedRevisionNotes").Value = AppendedRevisionNotes

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo _
            & ", AppendedRevisionNotes:" & AppendedRevisionNotes _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "AppendDrawingRevisionNotes : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"
            UGNErrorTrapping.InsertErrorLog("AppendDrawingRevisionNotes : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Function CopyDrawing(ByVal DrawingNo As String, ByVal copyType As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Copy_Drawing"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)

        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try

            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@drawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@drawingNo").Value = DrawingNo

            myCommand.Parameters.Add("@copyType", SqlDbType.VarChar)
            myCommand.Parameters("@copyType").Value = copyType

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CopyDrawing")

            CopyDrawing = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & ", CopyType:" & copyType & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "CopyDrawing : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingDetail.aspx?DrawingNo=" & DrawingNo
            UGNErrorTrapping.InsertErrorLog("CopyDrawing : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            CopyDrawing = Nothing           
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Sub DeletePECookies()

        Try
            HttpContext.Current.Response.Cookies("PEModule_SaveDrawingNoSearch").Value = ""
            HttpContext.Current.Response.Cookies("PEModule_SaveDrawingNoSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PEModule_SaveCustomerPartNoSearch").Value = ""
            HttpContext.Current.Response.Cookies("PEModule_SaveCustomerPartNoSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PEModule_SaveCustomerSearch").Value = ""
            HttpContext.Current.Response.Cookies("PEModule_SaveCustomerSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PEModule_SavePartNoSearch").Value = ""
            HttpContext.Current.Response.Cookies("PEModule_SavePartNoSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PEModule_SavePartNameSearch").Value = ""
            HttpContext.Current.Response.Cookies("PEModule_SavePartNameSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PEModule_SaveCommoditySearch").Value = 0
            HttpContext.Current.Response.Cookies("PEModule_SaveCommoditySearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PEModule_SavePurchasedGoodSearch").Value = 0
            HttpContext.Current.Response.Cookies("PEModule_SavePurchasedGoodSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PEModule_SaveVehicleYearSearch").Value = 0
            HttpContext.Current.Response.Cookies("PEModule_SaveVehicleYearSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PEModule_SaveProgramSearch").Value = 0
            HttpContext.Current.Response.Cookies("PEModule_SaveProgramSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PEModule_SaveSubFamilySearch").Value = 0
            HttpContext.Current.Response.Cookies("PEModule_SaveSubFamilySearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PEModule_SaveDensityValueSearch").Value = 0
            HttpContext.Current.Response.Cookies("PEModule_SaveDensityValueSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PEModule_SaveDrawingByEngineerSearch").Value = 0
            HttpContext.Current.Response.Cookies("PEModule_SaveDrawingByEngineerSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PEModule_SaveConstructionSearch").Value = ""
            HttpContext.Current.Response.Cookies("PEModule_SaveConstructionSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PEModule_SaveNotesSearch").Value = ""
            HttpContext.Current.Response.Cookies("PEModule_SaveNotesSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PEModule_SaveReleaseTypeSearch").Value = 0
            HttpContext.Current.Response.Cookies("PEModule_SaveReleaseTypeSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PEModule_SaveStatusSearch").Value = ""
            HttpContext.Current.Response.Cookies("PEModule_SaveStatusSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PEModule_SaveDrawingDateSearch").Value = ""
            HttpContext.Current.Response.Cookies("PEModule_SaveDrawingDateSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PEModule_SaveDesignationTypeSearch").Value = ""
            HttpContext.Current.Response.Cookies("PEModule_SaveDesignationTypeSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PEModule_SaveMakeSearch").Value = ""
            HttpContext.Current.Response.Cookies("PEModule_SaveMakeSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PEModule_SaveProductTechnologySearch").Value = 0
            HttpContext.Current.Response.Cookies("PEModule_SaveProductTechnologySearch").Expires = DateTime.Now.AddDays(-1)

            'HttpContext.Current.Response.Cookies("PEModule_SaveExternalDrawingDownloadDateSearch").Value = ""
            'HttpContext.Current.Response.Cookies("PEModule_SaveExternalDrawingDownloadDateSearch").Expires = DateTime.Now.AddDays(-1)

            'HttpContext.Current.Response.Cookies("PEModule_SaveExternalDrawingSourceIDSearch").Value = ""
            'HttpContext.Current.Response.Cookies("PEModule_SaveExternalDrawingSourceIDSearch").Expires = DateTime.Now.AddDays(-1)

            'HttpContext.Current.Response.Cookies("PEModule_SaveExternalDrawingCustomerValueSearch").Value = ""
            'HttpContext.Current.Response.Cookies("PEModule_SaveExternalDrawingCustomerValueSearch").Expires = DateTime.Now.AddDays(-1)

            'HttpContext.Current.Response.Cookies("PEModule_SaveExternalDrawingCustomerPartNoSearch").Value = ""
            'HttpContext.Current.Response.Cookies("PEModule_SaveExternalDrawingCustomerPartNoSearch").Expires = DateTime.Now.AddDays(-1)

            'HttpContext.Current.Response.Cookies("PEModule_SaveExternalDrawingCustomerPartNameSearch").Value = ""
            'HttpContext.Current.Response.Cookies("PEModule_SaveExternalDrawingCustomerPartNameSearch").Expires = DateTime.Now.AddDays(-1)

            'HttpContext.Current.Response.Cookies("PEModule_SaveExternalDrawingRevisionSearch").Value = ""
            'HttpContext.Current.Response.Cookies("PEModule_SaveExternalDrawingRevisionSearch").Expires = DateTime.Now.AddDays(-1)

            'HttpContext.Current.Response.Cookies("PEModule_SaveExternalDrawingProgramIDSearch").Value = ""
            'HttpContext.Current.Response.Cookies("PEModule_SaveExternalDrawingProgramIDSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PEModule_SaveDrawingDateStartSearch").Value = ""
            HttpContext.Current.Response.Cookies("PEModule_SaveDrawingDateStartSearch").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PEModule_SaveDrawingDateEndSearch").Value = ""
            HttpContext.Current.Response.Cookies("PEModule_SaveDrawingDateEndSearch").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeletePECookies : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeletePECookies : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub

    Public Shared Function GetDrawing(ByVal DrawingNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Drawing"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandTimeout = 300

            If DrawingNo Is Nothing Then
                DrawingNo = ""
            End If

            myCommand.Parameters.Add("@drawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@drawingNo").Value = commonFunctions.convertSpecialChar(DrawingNo, False)

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Drawing")
            GetDrawing = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo  & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetDrawing : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDrawing : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetDrawing = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetDrawingSearch(ByVal DrawingNo As String, ByVal ReleaseTypeID As Integer, ByVal PartNo As String, _
      ByVal PartName As String, ByVal CustomerPartNo As String, ByVal Customer As String, ByVal DesignationType As String, _
      ByVal VehicleYear As Integer, ByVal Program As Integer, ByVal SubFamily As Integer, ByVal Commodity As Integer, _
      ByVal PurchasedGood As Integer, ByVal DensityValue As Double, ByVal Construction As String, _
      ByVal Status As String, ByVal Notes As String, ByVal DrawingByEngeineer As Integer, _
      ByVal Obsolete As Boolean, ByVal DrawingDateStart As String, _
      ByVal DrawingDateEnd As String, ByVal Make As String, ByVal ProductTechnologyID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Drawing_Search"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandTimeout = 300

            myCommand.Parameters.Add("@DrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@DrawingNo").Value = commonFunctions.convertSpecialChar(If(DrawingNo Is Nothing, "", DrawingNo), False)

            myCommand.Parameters.Add("@ReleaseTypeID", SqlDbType.Int)
            myCommand.Parameters("@ReleaseTypeID").Value = ReleaseTypeID

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = commonFunctions.convertSpecialChar(If(PartNo Is Nothing, "", PartNo), False)

            myCommand.Parameters.Add("@PartName", SqlDbType.VarChar)
            myCommand.Parameters("@PartName").Value = commonFunctions.convertSpecialChar(If(PartName Is Nothing, "", PartName), False)

            myCommand.Parameters.Add("@CustomerPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@CustomerPartNo").Value = commonFunctions.convertSpecialChar(If(CustomerPartNo Is Nothing, "", CustomerPartNo), False)

            myCommand.Parameters.Add("@Customer", SqlDbType.VarChar)
            myCommand.Parameters("@Customer").Value = If(Customer Is Nothing, "", Customer)

            myCommand.Parameters.Add("@DesignationType", SqlDbType.VarChar)
            myCommand.Parameters("@DesignationType").Value = If(DesignationType Is Nothing, "", DesignationType)

            myCommand.Parameters.Add("@VehicleYear", SqlDbType.Int)
            myCommand.Parameters("@VehicleYear").Value = VehicleYear

            myCommand.Parameters.Add("@ProgramID", SqlDbType.Int)
            myCommand.Parameters("@ProgramID").Value = Program

            myCommand.Parameters.Add("@SubFamilyID", SqlDbType.Int)
            myCommand.Parameters("@SubFamilyID").Value = SubFamily

            myCommand.Parameters.Add("@CommodityID", SqlDbType.Int)
            myCommand.Parameters("@CommodityID").Value = Commodity

            myCommand.Parameters.Add("@PurchasedGoodID", SqlDbType.Int)
            myCommand.Parameters("@PurchasedGoodID").Value = PurchasedGood

            myCommand.Parameters.Add("@DensityValue", SqlDbType.Decimal)
            myCommand.Parameters("@DensityValue").Value = DensityValue

            myCommand.Parameters.Add("@Construction", SqlDbType.VarChar)
            myCommand.Parameters("@Construction").Value = commonFunctions.convertSpecialChar(If(Construction Is Nothing, "", Construction), False)

            myCommand.Parameters.Add("@ApprovalStatus", SqlDbType.VarChar)
            myCommand.Parameters("@ApprovalStatus").Value = If(Status Is Nothing, "", Status)

            myCommand.Parameters.Add("@Notes", SqlDbType.VarChar)
            myCommand.Parameters("@Notes").Value = commonFunctions.convertSpecialChar(If(Notes Is Nothing, "", Notes), False)

            myCommand.Parameters.Add("@DrawingByEngineerID", SqlDbType.Int)
            myCommand.Parameters("@DrawingByEngineerID").Value = DrawingByEngeineer

            myCommand.Parameters.Add("@Obsolete", SqlDbType.Bit)
            myCommand.Parameters("@Obsolete").Value = Obsolete

            myCommand.Parameters.Add("@DrawingDateStart", SqlDbType.VarChar)
            myCommand.Parameters("@DrawingDateStart").Value = If(DrawingDateStart Is Nothing, "", DrawingDateStart)

            myCommand.Parameters.Add("@DrawingDateEnd", SqlDbType.VarChar)
            myCommand.Parameters("@DrawingDateEnd").Value = If(DrawingDateEnd Is Nothing, "", DrawingDateEnd)

            myCommand.Parameters.Add("@Make", SqlDbType.VarChar)
            myCommand.Parameters("@Make").Value = Make

            myCommand.Parameters.Add("@ProductTechnologyID", SqlDbType.Int)
            myCommand.Parameters("@ProductTechnologyID").Value = ProductTechnologyID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DrawingSearch")
            GetDrawingSearch = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & ", ReleaseTypeID:" & ReleaseTypeID _
            & ", PartNo:" & PartNo & ", PartName:" & PartName & ", CustomerPartNo:" & CustomerPartNo _
            & ", Customer:" & Customer & ", DesignationType:" & DesignationType _
            & ", VehicleYear:" & VehicleYear & ", Program:" & Program & ", SubFamily:" & SubFamily _
            & ", Commodity:" & Commodity & ", PurchasedGood:" & PurchasedGood & ", DensityValue:" & DensityValue _
            & ", Construction:" & Construction & ", Status:" & Status & ", Notes:" & Notes & ", DrawingByEngeineer:" & DrawingByEngeineer _
            & ", Obsolete:" & Obsolete & ", DrawingDateStart:" & DrawingDateStart _
            & ", DrawingDateEnd:" & DrawingDateEnd & ", Make:" & Make _
            & ", ProductTechnologyID:" & ProductTechnologyID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetDrawingSearch : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDrawingSearch : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetDrawingSearch = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetDrawingCustomerProgram(ByVal DrawingNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Drawing_Customer_Program"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandTimeout = 300

            If DrawingNo Is Nothing Then
                DrawingNo = ""
            End If

            myCommand.Parameters.Add("@drawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@drawingNo").Value = commonFunctions.convertSpecialChar(DrawingNo, False)

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DrawingCustomerProgram")
            GetDrawingCustomerProgram = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetDrawingCustomerProgram : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDrawingCustomerProgram : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetDrawingCustomerProgram = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetDrawingBPCS(ByVal DrawingNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Drawing_BPCS"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandTimeout = 300

            If DrawingNo Is Nothing Then
                DrawingNo = ""
            End If

            myCommand.Parameters.Add("@drawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@drawingNo").Value = commonFunctions.convertSpecialChar(DrawingNo, False)

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetDrawingBPCS")
            GetDrawingBPCS = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetDrawing : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDrawing : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetDrawingBPCS = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetSubDrawing(ByVal DrawingNo As String, ByVal SubDrawingNo As String, ByVal PartNo As String, ByVal PartRevision As String, ByVal SubPartNo As String, ByVal SubPartRevision As String, ByVal DrawingQuantity As Double, ByVal Notes As String, ByVal Obsolete As Boolean) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Sub_Drawing"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If DrawingNo Is Nothing Then
                DrawingNo = ""
            End If

            myCommand.Parameters.Add("@drawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@drawingNo").Value = DrawingNo

            If SubDrawingNo Is Nothing Then
                SubDrawingNo = ""
            End If

            myCommand.Parameters.Add("@subDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@subDrawingNo").Value = SubDrawingNo

            If PartNo Is Nothing Then
                PartNo = ""
            End If

            myCommand.Parameters.Add("@PartNo", SqlDbType.VarChar)
            myCommand.Parameters("@PartNo").Value = PartNo

            If PartRevision Is Nothing Then
                PartRevision = ""
            End If

            myCommand.Parameters.Add("@PartRevision", SqlDbType.VarChar)
            myCommand.Parameters("@PartRevision").Value = PartRevision

            If SubPartNo Is Nothing Then
                SubPartNo = ""
            End If

            myCommand.Parameters.Add("@subPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@subPartNo").Value = SubPartNo

            If SubPartRevision Is Nothing Then
                SubPartRevision = ""
            End If

            myCommand.Parameters.Add("@subPartRevision", SqlDbType.VarChar)
            myCommand.Parameters("@subPartRevision").Value = SubPartRevision

            myCommand.Parameters.Add("@drawingQuantity", SqlDbType.Decimal)
            myCommand.Parameters("@drawingQuantity").Value = DrawingQuantity

            If Notes Is Nothing Then
                Notes = ""
            End If

            myCommand.Parameters.Add("@notes", SqlDbType.VarChar)
            myCommand.Parameters("@notes").Value = Notes

            myCommand.Parameters.Add("@obsolete", SqlDbType.Bit)
            myCommand.Parameters("@obsolete").Value = Obsolete

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "SubDrawing")
            GetSubDrawing = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & ", SubDrawingNo:" & SubDrawingNo & ", PartNo:" & PartNo & ", PartRevision :" & PartRevision & ", SubPartNo:" & SubPartNo & ", SubPartRevision:" & SubPartRevision & ", DrawingQuantity:" & DrawingQuantity & ", Notes :" & Notes & ", Obsolete :" & Obsolete & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetSubDrawing : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetSubDrawing : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetSubDrawing = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetTempDrawings(ByVal KeyDrawingNo As String, ByVal DrawingNo As String) As DataSet

        HttpContext.Current.Session("BLLerror") = Nothing

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString

        Dim strStoredProcName As String = "sp_Get_Temp_Drawings"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try

            If HttpContext.Current.Request.Cookies("UGNDB_User") Is Nothing Then
                commonFunctions.SetUGNDBUser()
            End If

            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@KeyDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@KeyDrawingNo").Value = KeyDrawingNo

            myCommand.Parameters.Add("@DrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@DrawingNo").Value = DrawingNo

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "TempDrawings")
            GetTempDrawings = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "KeyDrawingNo: " & KeyDrawingNo _
            & ", DrawingNo:" & DrawingNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetTempDrawings : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingDetail.aspx?DrawingNo=" & KeyDrawingNo
            UGNErrorTrapping.InsertErrorLog("GetTempDrawings : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetTempDrawings = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function DeleteDrawing(ByVal DrawingNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Drawing"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If DrawingNo Is Nothing Then
                DrawingNo = ""
            End If

            myCommand.Parameters.Add("@drawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@drawingNo").Value = DrawingNo

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DrawingObsolete")
            DeleteDrawing = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteDrawing : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingDetail.aspx?DrawingNo=" & DrawingNo
            UGNErrorTrapping.InsertErrorLog("DeleteDrawing : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            DeleteDrawing = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function InsertDrawing(ByVal DrawingNo As String, ByVal OldPartName As String, ByVal ReleaseTypeID As Integer, _
        ByVal InStepTracking As Integer, ByVal FuturePartNo As String, ByVal RFDNo As Integer, _
        ByVal CustomerPartNo As String, ByVal DesignationType As String, ByVal CADavailable As Boolean, _
        ByVal SubFamilyID As Integer, ByVal ProductTechnologyID As Integer, ByVal CommodityID As Integer, ByVal PurchasedGoodID As Integer, _
        ByVal EngineerID As Integer, ByVal DrawingByEngineerID As Integer, ByVal CheckedByEngineerID As Integer, _
        ByVal ProcessEngineerID As Integer, ByVal QualityEngineerID As Integer, ByVal DensityValue As Double, _
        ByVal DensityUnits As String, ByVal DensityTolerance As String, ByVal ThicknessValue As Double, _
        ByVal ThicknessUnits As String, ByVal ThicknessTolerance As String, ByVal DrawingLayoutType As String, _
        ByVal AMDValue As Double, ByVal AMDUnits As String, ByVal AMDTolerance As String, ByVal WMDValue As Double, _
        ByVal WMDUnits As String, ByVal WMDTolerance As String, ByVal ToleranceID As Integer, _
        ByVal Construction As String, ByVal RevisionNotes As String, _
        ByVal Notes As String, ByVal Comments As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Drawing"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If DrawingNo Is Nothing Then
                DrawingNo = ""
            End If

            myCommand.Parameters.Add("@drawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@drawingNo").Value = DrawingNo

            If OldPartName Is Nothing Then
                OldPartName = ""
            End If

            myCommand.Parameters.Add("@oldPartName", SqlDbType.VarChar)
            myCommand.Parameters("@oldPartName").Value = commonFunctions.convertSpecialChar(OldPartName, False)

            myCommand.Parameters.Add("@releaseTypeID", SqlDbType.Int)
            myCommand.Parameters("@releaseTypeID").Value = ReleaseTypeID

            myCommand.Parameters.Add("@inStepTracking", SqlDbType.Int)
            myCommand.Parameters("@inStepTracking").Value = InStepTracking

            myCommand.Parameters.Add("@futurePartNo", SqlDbType.VarChar)
            myCommand.Parameters("@FuturePartNo").Value = FuturePartNo

            myCommand.Parameters.Add("@rfdNo", SqlDbType.Int)
            myCommand.Parameters("@rfdNo").Value = RFDNo

            myCommand.Parameters.Add("@customerPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@customerPartNo").Value = CustomerPartNo

            myCommand.Parameters.Add("@designationType", SqlDbType.VarChar)
            myCommand.Parameters("@designationType").Value = DesignationType

            myCommand.Parameters.Add("@CADavailable", SqlDbType.Bit)
            myCommand.Parameters("@CADavailable").Value = CADavailable

            myCommand.Parameters.Add("@subFamilyID", SqlDbType.Int)
            myCommand.Parameters("@subFamilyID").Value = SubFamilyID

            myCommand.Parameters.Add("@productTechnologyID", SqlDbType.Int)
            myCommand.Parameters("@productTechnologyID").Value = ProductTechnologyID

            myCommand.Parameters.Add("@commodityID", SqlDbType.Int)
            myCommand.Parameters("@commodityID").Value = CommodityID

            myCommand.Parameters.Add("@purchasedGoodID", SqlDbType.Int)
            myCommand.Parameters("@purchasedGoodID").Value = PurchasedGoodID

            myCommand.Parameters.Add("@engineerID", SqlDbType.Int)
            myCommand.Parameters("@engineerID").Value = EngineerID

            myCommand.Parameters.Add("@drawingByEngineerID", SqlDbType.Int)
            myCommand.Parameters("@drawingByEngineerID").Value = DrawingByEngineerID

            myCommand.Parameters.Add("@checkedByEngineerID", SqlDbType.Int)
            myCommand.Parameters("@checkedByEngineerID").Value = CheckedByEngineerID

            myCommand.Parameters.Add("@processEngineerID", SqlDbType.Int)
            myCommand.Parameters("@processEngineerID").Value = ProcessEngineerID

            myCommand.Parameters.Add("@qualityEngineerID", SqlDbType.Int)
            myCommand.Parameters("@qualityEngineerID").Value = QualityEngineerID

            myCommand.Parameters.Add("@densityValue", SqlDbType.Decimal)
            myCommand.Parameters("@densityValue").Value = DensityValue

            myCommand.Parameters.Add("@densityUnits", SqlDbType.VarChar)
            myCommand.Parameters("@densityUnits").Value = DensityUnits

            myCommand.Parameters.Add("@densityTolerance", SqlDbType.VarChar)
            myCommand.Parameters("@densityTolerance").Value = DensityTolerance

            myCommand.Parameters.Add("@thicknessValue", SqlDbType.Decimal)
            myCommand.Parameters("@thicknessValue").Value = ThicknessValue

            myCommand.Parameters.Add("@thicknessUnits", SqlDbType.VarChar)
            myCommand.Parameters("@thicknessUnits").Value = ThicknessUnits

            myCommand.Parameters.Add("@thicknessTolerance", SqlDbType.VarChar)
            myCommand.Parameters("@thicknessTolerance").Value = ThicknessTolerance

            myCommand.Parameters.Add("@drawingLayoutType", SqlDbType.VarChar)
            myCommand.Parameters("@drawingLayoutType").Value = DrawingLayoutType

            myCommand.Parameters.Add("@amdValue", SqlDbType.Decimal)
            myCommand.Parameters("@amdValue").Value = AMDValue

            myCommand.Parameters.Add("@amdUnits", SqlDbType.VarChar)
            myCommand.Parameters("@amdUnits").Value = AMDUnits

            myCommand.Parameters.Add("@amdTolerance", SqlDbType.VarChar)
            myCommand.Parameters("@amdTolerance").Value = AMDTolerance

            myCommand.Parameters.Add("@wmdValue", SqlDbType.Decimal)
            myCommand.Parameters("@wmdValue").Value = WMDValue

            myCommand.Parameters.Add("@wmdUnits", SqlDbType.VarChar)
            myCommand.Parameters("@wmdUnits").Value = WMDUnits

            myCommand.Parameters.Add("@wmdTolerance", SqlDbType.VarChar)
            myCommand.Parameters("@wmdTolerance").Value = WMDTolerance

            myCommand.Parameters.Add("@toleranceID", SqlDbType.Int)
            myCommand.Parameters("@toleranceID").Value = ToleranceID

            myCommand.Parameters.Add("@construction", SqlDbType.VarChar)
            myCommand.Parameters("@construction").Value = Construction

            myCommand.Parameters.Add("@revisionNotes", SqlDbType.VarChar)
            myCommand.Parameters("@revisionNotes").Value = RevisionNotes

            myCommand.Parameters.Add("@notes", SqlDbType.VarChar)
            myCommand.Parameters("@notes").Value = Notes

            myCommand.Parameters.Add("@comments", SqlDbType.VarChar)
            myCommand.Parameters("@comments").Value = Comments

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "InsertDrawing")
            InsertDrawing = GetData

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & ", ReleaseTypeID : " & ReleaseTypeID _
            & ", InStepTracking: " & InStepTracking & ", FuturePartNo: " & FuturePartNo _
            & ", RFDNo: " & RFDNo & ", CustomerPartNo: " & CustomerPartNo _
            & ", SubFamilyID: " & SubFamilyID & ", ProductTechnologyID: " & ProductTechnologyID & ", CommodityID: " & CommodityID _
            & ", PurchasedGoodID: " & PurchasedGoodID & ", EngineerID: " & EngineerID _
            & ", DrawingByEngineerID: " & DrawingByEngineerID & ", CheckedByEngineerID: " & CheckedByEngineerID _
            & ", ProcessEngineerID: " & ProcessEngineerID & ", QualityEngineerID: " & QualityEngineerID _
            & ", DensityValue: " & DensityValue & ", DensityUnits: " & DensityUnits _
            & ", DensityTolerance: " & DensityTolerance & ", ThicknessValue: " & ThicknessValue _
            & ", ThicknessUnits: " & ThicknessUnits & ", ThicknessTolerance: " & ThicknessTolerance _
            & ", DrawingLayoutType: " & DrawingLayoutType & ", CADavailable: " & CADavailable _
            & ", AMDValue : " & AMDValue & ", AMDUnits : " & AMDUnits _
            & ", AMDTolerance: " & AMDTolerance & ", WMDValue: " & WMDValue & ", WMDTolerance  : " & WMDTolerance _
            & ", ToleranceID : " & ToleranceID & ", Construction : " & Construction _
            & ", RevisionNotes : " & RevisionNotes & ", Notes : " & Notes & ", Comments : " & Comments _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertDrawing : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingDetail.aspx?DrawingNo=" & DrawingNo
            UGNErrorTrapping.InsertErrorLog("InsertDrawing : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertDrawing = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function UpdateDrawing(ByVal DrawingNo As String, ByVal OldPartName As String, ByVal ReleaseTypeID As Integer, _
        ByVal InStepTracking As Integer, ByVal FuturePartNo As String, _
        ByVal RFDNo As Integer, ByVal CustomerPartNo As String, _
        ByVal DesignationType As String, ByVal CADavailable As Boolean, _
        ByVal SubFamilyID As Integer, ByVal ProductTechnologyID As Integer, ByVal CommodityID As Integer, ByVal PurchasedGoodID As Integer, _
        ByVal EngineerID As Integer, ByVal DrawingByEngineerID As Integer, ByVal CheckedByEngineerID As Integer, _
        ByVal ProcessEngineerID As Integer, ByVal QualityEngineerID As Integer, ByVal DensityValue As Double, _
        ByVal DensityUnits As String, ByVal DensityTolerance As String, ByVal ThicknessValue As Double, _
        ByVal ThicknessUnits As String, ByVal ThicknessTolerance As String, ByVal DrawingLayoutType As String, _
        ByVal AMDValue As Double, ByVal AMDUnits As String, ByVal AMDTolerance As String, ByVal WMDValue As Double, _
        ByVal WMDUnits As String, ByVal WMDTolerance As String, ByVal ToleranceID As Integer, ByVal Construction As String, _
        ByVal RevisionNotes As String, ByVal Notes As String, ByVal Comments As String, _
        ByVal UGNDBVendorID As Integer, ByVal PackagingInstructions As String, ByVal PackagingRollLength As Double, _
        ByVal PackagingRollTolerance As String, ByVal PackagingRollUnits As String, ByVal PackagingIncomingInspectionComments As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Drawing"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If DrawingNo Is Nothing Then
                DrawingNo = ""
            End If

            myCommand.Parameters.Add("@drawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@drawingNo").Value = DrawingNo

            If OldPartName Is Nothing Then
                OldPartName = ""
            End If

            myCommand.Parameters.Add("@oldPartName", SqlDbType.VarChar)
            myCommand.Parameters("@oldPartName").Value = commonFunctions.convertSpecialChar(OldPartName, False)

            myCommand.Parameters.Add("@releaseTypeID", SqlDbType.Int)
            myCommand.Parameters("@releaseTypeID").Value = ReleaseTypeID

            myCommand.Parameters.Add("@inStepTracking", SqlDbType.Int)
            myCommand.Parameters("@inStepTracking").Value = InStepTracking

            myCommand.Parameters.Add("@futurePartNo", SqlDbType.VarChar)
            myCommand.Parameters("@FuturePartNo").Value = FuturePartNo

            myCommand.Parameters.Add("@rfdNo", SqlDbType.Int)
            myCommand.Parameters("@rfdNo").Value = RFDNo

            myCommand.Parameters.Add("@customerPartNo", SqlDbType.VarChar)
            myCommand.Parameters("@customerPartNo").Value = CustomerPartNo

            myCommand.Parameters.Add("@designationType", SqlDbType.VarChar)
            myCommand.Parameters("@designationType").Value = DesignationType

            myCommand.Parameters.Add("@CADavailable", SqlDbType.Bit)
            myCommand.Parameters("@CADavailable").Value = CADavailable

            myCommand.Parameters.Add("@subFamilyID", SqlDbType.Int)
            myCommand.Parameters("@subFamilyID").Value = SubFamilyID

            myCommand.Parameters.Add("@productTechnologyID", SqlDbType.Int)
            myCommand.Parameters("@productTechnologyID").Value = ProductTechnologyID

            myCommand.Parameters.Add("@commodityID", SqlDbType.Int)
            myCommand.Parameters("@commodityID").Value = CommodityID

            myCommand.Parameters.Add("@purchasedGoodID", SqlDbType.Int)
            myCommand.Parameters("@purchasedGoodID").Value = PurchasedGoodID

            myCommand.Parameters.Add("@engineerID", SqlDbType.Int)
            myCommand.Parameters("@engineerID").Value = EngineerID

            myCommand.Parameters.Add("@drawingByEngineerID", SqlDbType.Int)
            myCommand.Parameters("@drawingByEngineerID").Value = DrawingByEngineerID

            myCommand.Parameters.Add("@checkedByEngineerID", SqlDbType.Int)
            myCommand.Parameters("@checkedByEngineerID").Value = CheckedByEngineerID

            myCommand.Parameters.Add("@processEngineerID", SqlDbType.Int)
            myCommand.Parameters("@processEngineerID").Value = ProcessEngineerID

            myCommand.Parameters.Add("@qualityEngineerID", SqlDbType.Int)
            myCommand.Parameters("@qualityEngineerID").Value = QualityEngineerID

            myCommand.Parameters.Add("@densityValue", SqlDbType.Decimal)
            myCommand.Parameters("@densityValue").Value = DensityValue

            myCommand.Parameters.Add("@densityUnits", SqlDbType.VarChar)
            myCommand.Parameters("@densityUnits").Value = DensityUnits

            myCommand.Parameters.Add("@densityTolerance", SqlDbType.VarChar)
            myCommand.Parameters("@densityTolerance").Value = DensityTolerance

            myCommand.Parameters.Add("@thicknessValue", SqlDbType.Decimal)
            myCommand.Parameters("@thicknessValue").Value = ThicknessValue

            myCommand.Parameters.Add("@thicknessUnits", SqlDbType.VarChar)
            myCommand.Parameters("@thicknessUnits").Value = ThicknessUnits

            myCommand.Parameters.Add("@thicknessTolerance", SqlDbType.VarChar)
            myCommand.Parameters("@thicknessTolerance").Value = ThicknessTolerance

            myCommand.Parameters.Add("@drawingLayoutType", SqlDbType.VarChar)
            myCommand.Parameters("@drawingLayoutType").Value = DrawingLayoutType

            myCommand.Parameters.Add("@amdValue", SqlDbType.Decimal)
            myCommand.Parameters("@amdValue").Value = AMDValue

            myCommand.Parameters.Add("@amdUnits", SqlDbType.VarChar)
            myCommand.Parameters("@amdUnits").Value = AMDUnits

            myCommand.Parameters.Add("@amdTolerance", SqlDbType.VarChar)
            myCommand.Parameters("@amdTolerance").Value = AMDTolerance

            myCommand.Parameters.Add("@wmdValue", SqlDbType.Decimal)
            myCommand.Parameters("@wmdValue").Value = WMDValue

            myCommand.Parameters.Add("@wmdUnits", SqlDbType.VarChar)
            myCommand.Parameters("@wmdUnits").Value = WMDUnits

            myCommand.Parameters.Add("@wmdTolerance", SqlDbType.VarChar)
            myCommand.Parameters("@wmdTolerance").Value = WMDTolerance

            myCommand.Parameters.Add("@toleranceID", SqlDbType.Int)
            myCommand.Parameters("@toleranceID").Value = ToleranceID

            myCommand.Parameters.Add("@construction", SqlDbType.VarChar)
            myCommand.Parameters("@construction").Value = Construction

            myCommand.Parameters.Add("@revisionNotes", SqlDbType.VarChar)
            myCommand.Parameters("@revisionNotes").Value = RevisionNotes

            myCommand.Parameters.Add("@notes", SqlDbType.VarChar)
            myCommand.Parameters("@notes").Value = Notes

            myCommand.Parameters.Add("@comments", SqlDbType.VarChar)
            myCommand.Parameters("@comments").Value = Comments

            myCommand.Parameters.Add("@UGNDBVendorID", SqlDbType.Int)
            myCommand.Parameters("@UGNDBVendorID").Value = UGNDBVendorID

            myCommand.Parameters.Add("@packagingInstructions", SqlDbType.VarChar)
            myCommand.Parameters("@packagingInstructions").Value = PackagingInstructions

            myCommand.Parameters.Add("@packagingRollLength", SqlDbType.Decimal)
            myCommand.Parameters("@packagingRollLength").Value = PackagingRollLength

            myCommand.Parameters.Add("@packagingRollUnits", SqlDbType.VarChar)
            myCommand.Parameters("@packagingRollUnits").Value = PackagingRollUnits

            myCommand.Parameters.Add("@packagingRollTolerance", SqlDbType.VarChar)
            myCommand.Parameters("@packagingRollTolerance").Value = PackagingRollTolerance

            myCommand.Parameters.Add("@packagingIncomingInspectionComments", SqlDbType.VarChar)
            myCommand.Parameters("@packagingIncomingInspectionComments").Value = PackagingIncomingInspectionComments

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "UpdateDrawing")
            UpdateDrawing = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & ", ReleaseTypeID : " & ReleaseTypeID _
            & ", InStepTracking: " & InStepTracking & ", FuturePartNo: " & FuturePartNo _
            & ", RFDNo: " & RFDNo & ", DesignationType: " & DesignationType _
            & ", CADavailable: " & CADavailable & ", CustomerPartNo: " & CustomerPartNo _
            & ", SubFamilyID: " & SubFamilyID & ", ProductTechnologyID: " & ProductTechnologyID & ", CommodityID: " & CommodityID _
            & ", PurchasedGoodID: " & PurchasedGoodID & ", EngineerID: " & EngineerID _
            & ", DrawingByEngineerID: " & DrawingByEngineerID & ", CheckedByEngineerID: " & CheckedByEngineerID _
            & ", ProcessEngineerID: " & ProcessEngineerID & ", QualityEngineerID: " & QualityEngineerID _
            & ", DensityValue: " & DensityValue & ", DensityUnits: " & DensityUnits _
            & ", DensityTolerance: " & DensityTolerance & ", ThicknessValue: " & ThicknessValue & ", ThicknessUnits: " & ThicknessUnits _
            & ", ThicknessTolerance: " & ThicknessTolerance & ", DrawingLayoutType: " & DrawingLayoutType _
            & ", AMDValue : " & AMDValue & ", AMDUnits : " & AMDUnits & ", AMDTolerance: " & AMDTolerance _
            & ", WMDValue: " & WMDValue & ", WMDTolerance  : " & WMDTolerance & ", ToleranceID : " & ToleranceID _
            & ", Construction : " & Construction _
            & ", RevisionNotes : " & RevisionNotes & ", Notes : " & Notes & "Comments : " & Comments _
            & ", UGNDBVendorID : " & UGNDBVendorID & ", PackagingInstructions : " & PackagingInstructions _
            & ", PackagingRollLength : " & PackagingRollLength & ", PackagingRollUnits : " & PackagingRollUnits _
            & ", PackagingRollTolerance : " & PackagingRollTolerance _
            & ", PackagingIncomingInspectionComments : " & PackagingIncomingInspectionComments _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateDrawing : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingDetail.aspx?DrawingNo=" & DrawingNo
            UGNErrorTrapping.InsertErrorLog("UpdateDrawing : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            UpdateDrawing = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function UpdateDrawingAppendRevisionNotes(ByVal DrawingNo As String, ByVal AppendRevisionNotes As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Drawing_Append_Revision_Notes"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If DrawingNo Is Nothing Then
                DrawingNo = ""
            End If

            myCommand.Parameters.Add("@DrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@DrawingNo").Value = DrawingNo

            If AppendRevisionNotes Is Nothing Then
                AppendRevisionNotes = ""
            End If

            myCommand.Parameters.Add("@AppendRevisionNotes", SqlDbType.VarChar)
            myCommand.Parameters("@AppendRevisionNotes").Value = AppendRevisionNotes

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "UpdateDrawingAppendRevisionNotes")
            UpdateDrawingAppendRevisionNotes = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo _
            & ", AppendRevisionNotes: " & AppendRevisionNotes _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateDrawingAppendRevisionNotes : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingDetail.aspx?DrawingNo=" & DrawingNo

            UGNErrorTrapping.InsertErrorLog("UpdateDrawingAppendRevisionNotes : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            UpdateDrawingAppendRevisionNotes = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function UpdateDrawingReleaseType(ByVal DrawingNo As String, ByVal ReleaseTypeID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Drawing_Release_Type"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If DrawingNo Is Nothing Then
                DrawingNo = ""
            End If

            myCommand.Parameters.Add("@DrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@DrawingNo").Value = DrawingNo

            myCommand.Parameters.Add("@ReleaseTypeID", SqlDbType.Int)
            myCommand.Parameters("@ReleaseTypeID").Value = ReleaseTypeID

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "UpdateDrawingReleaseType")
            UpdateDrawingReleaseType = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo _
            & ", ReleaseTypeID: " & ReleaseTypeID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateDrawingReleaseType : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingDetail.aspx?DrawingNo=" & DrawingNo

            UGNErrorTrapping.InsertErrorLog("UpdateDrawingReleaseType : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            UpdateDrawingReleaseType = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function UpdateDrawingStatus(ByVal DrawingNo As String, ByVal ApprovalStatus As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Drawing_Status"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If DrawingNo Is Nothing Then
                DrawingNo = ""
            End If

            myCommand.Parameters.Add("@drawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@drawingNo").Value = DrawingNo

            If ApprovalStatus Is Nothing Then
                ApprovalStatus = ""
            End If

            myCommand.Parameters.Add("@approvalStatus", SqlDbType.VarChar)
            myCommand.Parameters("@approvalStatus").Value = ApprovalStatus

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "UpdateDrawingStatus")
            UpdateDrawingStatus = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & "ApprovalStatus: " & ApprovalStatus & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateDrawingStatus : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingDetail.aspx?DrawingNo=" & DrawingNo
            UGNErrorTrapping.InsertErrorLog("UpdateDrawingReleaseType : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            UpdateDrawingStatus = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetDrawingByEngineers() As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Drawing_By_Engineers"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DrawingByEngineers")

            GetDrawingByEngineers = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetDrawingByEngineers : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDrawingByEngineers : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetDrawingByEngineers = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetDrawingDensity() As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Drawing_Density"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DrawingDensity")

            GetDrawingDensity = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetDrawingDensity : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDrawingDensity : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetDrawingDensity = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetDrawingImages(ByVal DrawingNo As String, ByVal AlternativeDrawingNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Drawing_Images"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If DrawingNo = Nothing Then
                DrawingNo = ""
            End If

            If AlternativeDrawingNo = Nothing Then
                AlternativeDrawingNo = ""
            End If

            myCommand.Parameters.Add("@drawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@drawingNo").Value = DrawingNo

            myCommand.Parameters.Add("@alternativeDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@alternativeDrawingNo").Value = AlternativeDrawingNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DrawingImages")

            GetDrawingImages = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & "AlternativeDrawingNo: " & AlternativeDrawingNo & "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetDrawingImages : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDrawingImages : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetDrawingImages = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetDrawingCustomerImages(ByVal DrawingNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Drawing_Customer_Images"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If DrawingNo = Nothing Then
                DrawingNo = ""
            End If

            myCommand.Parameters.Add("@drawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@drawingNo").Value = DrawingNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DrawingCustomerImages")

            GetDrawingCustomerImages = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetDrawingCustomerImages : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDrawingCustomerImages : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetDrawingCustomerImages = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetDrawingNotifications(ByVal DrawingNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Drawing_Notifications"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If DrawingNo = Nothing Then
                DrawingNo = ""
            End If

            myCommand.Parameters.Add("@drawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@drawingNo").Value = DrawingNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DrawingImages")

            GetDrawingNotifications = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetDrawingNotifications : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingDrawingDetail.aspx?DrawingNo=" & DrawingNo
            UGNErrorTrapping.InsertErrorLog("GetDrawingNotifications : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetDrawingNotifications = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetDrawingRevisions(ByVal DrawingNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Drawing_Revisions"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If DrawingNo = Nothing Then
                DrawingNo = ""
            End If

            myCommand.Parameters.Add("@drawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@drawingNo").Value = DrawingNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DrawingImages")

            GetDrawingRevisions = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetDrawingRevisions : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingDetail.aspx?DrawingNo=" & DrawingNo
            UGNErrorTrapping.InsertErrorLog("GetDrawingRevisions : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetDrawingRevisions = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetDrawingMaxStep(ByVal DrawingNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Drawing_Max_Step"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If DrawingNo = Nothing Then
                DrawingNo = ""
            End If

            myCommand.Parameters.Add("@DrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@DrawingNo").Value = DrawingNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Drawing")

            GetDrawingMaxStep = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetDrawingMaxStep : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingDetail.aspx?DrawingNo=" & DrawingNo
            UGNErrorTrapping.InsertErrorLog("GetDrawingMaxStep : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetDrawingMaxStep = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetDrawingMaxRevision(ByVal DrawingNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Drawing_Max_Revision"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If DrawingNo = Nothing Then
                DrawingNo = ""
            End If

            myCommand.Parameters.Add("@DrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@DrawingNo").Value = DrawingNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Drawing")

            GetDrawingMaxRevision = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetDrawingMaxRevision : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingDetail.aspx?DrawingNo=" & DrawingNo
            UGNErrorTrapping.InsertErrorLog("GetDrawingMaxRevision : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetDrawingMaxRevision = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function InsertDrawingImage(ByVal DrawingNo As String, ByVal ImageURL As String, ByVal ImageBytes As Byte()) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Drawing_Image"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@drawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@drawingNo").Value = DrawingNo

            myCommand.Parameters.Add("@imageURL", SqlDbType.VarChar)
            myCommand.Parameters("@imageURL").Value = ImageURL

            myCommand.Parameters.Add("@DrawingImage", SqlDbType.Image)
            myCommand.Parameters("@DrawingImage").Value = ImageBytes

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DrawingImages")

            InsertDrawingImage = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertDrawingImage : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingDetail.aspx?DrawingNo=" & DrawingNo
            UGNErrorTrapping.InsertErrorLog("InsertDrawingImage : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertDrawingImage = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function InsertDrawingCustomerImage(ByVal DrawingNo As String, ByVal CustomerDrawingNo As String, ByVal ImageBytes As Byte()) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Drawing_Customer_Image"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@DrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@DrawingNo").Value = DrawingNo

            myCommand.Parameters.Add("@CustomerDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@CustomerDrawingNo").Value = CustomerDrawingNo

            myCommand.Parameters.Add("@CustomerImage", SqlDbType.Image)
            myCommand.Parameters("@CustomerImage").Value = ImageBytes

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DrawingCustomerImages")

            InsertDrawingCustomerImage = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & ", CustomerDrawingNo: " & CustomerDrawingNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertDrawingCustomerImage : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingDetail.aspx?DrawingNo=" & DrawingNo
            UGNErrorTrapping.InsertErrorLog("InsertDrawingCustomerImage : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertDrawingCustomerImage = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Sub DeleteSubDrawingByParentDrawing(ByVal ParentDrawingNo As String, ByVal SubDrawingNo As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Sub_Drawing_By_Parent_Drawing"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@ParentDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@ParentDrawingNo").Value = ParentDrawingNo

            myCommand.Parameters.Add("@SubDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@SubDrawingNo").Value = SubDrawingNo

            'myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            'myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteSubDrawingByParentDrawing : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteSubDrawingByParentDrawing : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub DeleteTempDrawingBOM()

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Temp_Drawing_BOM"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteTempDrawingBOM : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"
            UGNErrorTrapping.InsertErrorLog("DeleteTempDrawingBOM : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Sub InsertTempDrawingBOM(ByVal SeqNo As Integer, ByVal KeyDrawingNo As String, ByVal DrawingNo As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Temp_Drawing_BOM"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@SeqNo", SqlDbType.Int)
            myCommand.Parameters("@SeqNo").Value = SeqNo

            myCommand.Parameters.Add("@KeyDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@KeyDrawingNo").Value = KeyDrawingNo

            myCommand.Parameters.Add("@DrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@DrawingNo").Value = DrawingNo

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SeqNo: " & SeqNo _
            & ", KeyDrawingNo: " & KeyDrawingNo _
            & ", DrawingNo: " & DrawingNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertTempDrawingBOM : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingDetail.aspx?DrawingNo=" & KeyDrawingNo
            UGNErrorTrapping.InsertErrorLog("InsertTempDrawingBOM : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Function InsertDrawingNotification(ByVal DrawingNo As String, ByVal TeamMemberID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Drawing_Notification"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@drawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@drawingNo").Value = DrawingNo

            myCommand.Parameters.Add("@teamMemberID", SqlDbType.Int)
            myCommand.Parameters("@teamMemberID").Value = TeamMemberID

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DrawingImages")

            InsertDrawingNotification = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & "TeamMemberID: " & TeamMemberID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertDrawingNotification : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingDetail.aspx?DrawingNo=" & DrawingNo
            UGNErrorTrapping.InsertErrorLog("InsertDrawingNotification : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertDrawingNotification = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function InsertSubDrawing(ByVal DrawingNo As String, ByVal SubDrawingNo As String, ByVal DrawingQuantity As Double, _
    ByVal Notes As String, ByVal Process As String, ByVal Equipment As String, ByVal ProcessParameters As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Sub_Drawing"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@DrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@DrawingNo").Value = DrawingNo

            myCommand.Parameters.Add("@SubDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@SubDrawingNo").Value = SubDrawingNo

            myCommand.Parameters.Add("@DrawingQuantity", SqlDbType.Decimal)
            myCommand.Parameters("@DrawingQuantity").Value = DrawingQuantity

            myCommand.Parameters.Add("@Notes", SqlDbType.VarChar)
            myCommand.Parameters("@Notes").Value = Notes

            myCommand.Parameters.Add("@Process", SqlDbType.VarChar)
            myCommand.Parameters("@Process").Value = Process

            myCommand.Parameters.Add("@Equipment", SqlDbType.VarChar)
            myCommand.Parameters("@Equipment").Value = Equipment

            myCommand.Parameters.Add("@ProcessParameters", SqlDbType.VarChar)
            myCommand.Parameters("@ProcessParameters").Value = ProcessParameters

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DrawingImages")

            InsertSubDrawing = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & ", SubDrawingNo:" & SubDrawingNo _
            & ", DrawingQuantity:" & DrawingQuantity & ", Notes :" & Notes _
            & ", Process:" & Process & ", Equipment :" & Equipment _
            & ", ProcessParameters:" & ProcessParameters _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "InsertSubDrawing : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingDetail.aspx?DrawingNo=" & DrawingNo
            UGNErrorTrapping.InsertErrorLog("InsertSubDrawing : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertSubDrawing = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function DeleteDrawingImage(ByVal DrawingNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Drawing_Image"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@drawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@drawingNo").Value = DrawingNo

            'myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            'myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DrawingImages")

            DeleteDrawingImage = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteDrawingImage : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingDetail.aspx?DrawingNo=" & DrawingNo
            UGNErrorTrapping.InsertErrorLog("DeleteDrawingImage : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            DeleteDrawingImage = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function DeleteDrawingCustomerImage(ByVal DrawingNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Drawing_Customer_Image"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@drawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@drawingNo").Value = DrawingNo

            'myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            'myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DrawingCustomerImages")

            DeleteDrawingCustomerImage = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeleteDrawingCustomerImage : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingDetail.aspx?DrawingNo=" & DrawingNo
            UGNErrorTrapping.InsertErrorLog("DeleteDrawingCustomerImage : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            DeleteDrawingCustomerImage = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function StartTempDrawings(ByVal DrawingNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Start_Temp_Drawing_And_BOM"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandTimeout = 300

            myCommand.Parameters.Add("@drawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@drawingNo").Value = DrawingNo

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DrawingStartTempBOM")

            StartTempDrawings = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "StartTempDrawings : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingDetail.aspx?DrawingNo=" & DrawingNo
            UGNErrorTrapping.InsertErrorLog("StartTempDrawings : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            StartTempDrawings = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function UpdateTempDrawingBOMImage(ByVal DrawingNo As String, ByVal ImageBytes As Byte()) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Temp_Drawing_BOM_Image"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@drawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@drawingNo").Value = DrawingNo

            myCommand.Parameters.Add("@DrawingImage", SqlDbType.Image)
            myCommand.Parameters("@DrawingImage").Value = ImageBytes

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DrawingUpdateTempBOMImages")

            UpdateTempDrawingBOMImage = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateTempDrawingBOMImage : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingDetail.aspx?DrawingNo=" & DrawingNo
            UGNErrorTrapping.InsertErrorLog("UpdateTempDrawingBOMImage : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            UpdateTempDrawingBOMImage = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function UpdateTempDrawingBOMRevisionNotes(ByVal DrawingNo As String, ByVal AllRevisionNotes As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Temp_Drawing_BOM_Revision_Notes"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@drawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@drawingNo").Value = DrawingNo

            myCommand.Parameters.Add("@allRevisionNotes", SqlDbType.VarChar)
            myCommand.Parameters("@allRevisionNotes").Value = AllRevisionNotes

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DrawingUpdateTempBOMImages")

            UpdateTempDrawingBOMRevisionNotes = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo & "AllRevisionNotes: " & AllRevisionNotes & "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "UpdateTempDrawingBOMRevisionNotes : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingDetail.aspx?DrawingNo=" & DrawingNo
            UGNErrorTrapping.InsertErrorLog("UpdateTempDrawingBOMRevisionNotes : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            UpdateTempDrawingBOMRevisionNotes = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetDrawingTolerance(ByVal ToleranceID As Integer, ByVal ToleranceName As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Drawing_Tolerance"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@toleranceID", SqlDbType.Int)
            myCommand.Parameters("@toleranceID").Value = ToleranceID

            myCommand.Parameters.Add("@toleranceName", SqlDbType.VarChar)
            myCommand.Parameters("@toleranceName").Value = ToleranceName

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DrawingTolerance")

            GetDrawingTolerance = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "ToleranceID: " & ToleranceID & "ToleranceName: " & ToleranceName & "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "GetDrawingTolerance : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingToleranceMaint.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDrawingTolerance : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetDrawingTolerance = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Sub CopyDrawingImage(ByVal NewDrawingNo As String, ByVal OriginalDrawingNo As String, ByVal DrawingLayoutType As String)

        Try

            Dim dsImages As DataSet
            Dim TempImageURL As String = ""
            Dim TempImageBytes As Byte()

            If DrawingLayoutType = "Other" Or DrawingLayoutType = "Other-MD-Critical" Then
                dsImages = PEModule.GetDrawingImages(OriginalDrawingNo, "")

                If commonFunctions.CheckDataset(dsImages) = True Then
                    If dsImages.Tables(0).Rows(0).Item("DrawingImage") IsNot System.DBNull.Value Then
                        TempImageBytes = dsImages.Tables(0).Rows(0).Item("DrawingImage")
                        TempImageURL = dsImages.Tables(0).Rows(0).Item("ImageURL")
                        PEModule.InsertDrawingImage(NewDrawingNo, TempImageURL, TempImageBytes)
                    End If
                End If
            End If

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "CopyDrawingImage : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyDrawingImage : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub

    Public Shared Function GenerateDrawingNo(ByVal DrawingNo As String, ByVal SubFamilyID As Integer, _
       ByVal InitialDimensionAndDensity As Integer, ByVal InStepTracking As Integer) As String

        Try
            '08/05/2008 - New Rule - Facility will not be referenced in DMS Drawing Number
            ''Drawings will have auto-generated numbers in the format of X1234-56789-A(BB)
            ''1234 = represents 4 digit family/subfamily code
            ''56 = represents some type of measurement of the material
            ''789 = represents next numeric sequence, of the presence of materials with same combination of 1234-56 above
            ''A = represents in-step tracking number
            ''BB = represents change level, starts at 0 for each drawing, auto-incremented for revisions       

            Dim ds As DataSet
            Dim iRowCount As Integer = 1
            Dim iAltRowCount As Integer = 1

            Dim iCtr As Integer = 0
            Dim iFirstDashLocation As Integer = 0

            Dim strNewDrawingNo As String = ""
            Dim strChangeLevel As String = ""

            'check digits 5 and 6 before incrementing numSeq, to avoid gaps
            Dim strNumberSequence As String = "1"
            Dim iNumberSequence As Integer = 0

            If DrawingNo <> "" Then
                iFirstDashLocation = InStr(DrawingNo, "-")
                strNumberSequence = Mid$(DrawingNo, iFirstDashLocation + 3, 3)
            End If

            iNumberSequence = CType(strNumberSequence, Integer)

            ''count number of records that have the same 1234-56 value, add 1 for the new record,
            ''initial implementation has all records with value of 56 as '00' ... no
            Dim strSubFamilyID As String = SubFamilyID.ToString

            Dim strInitialDimensionAndDensity As String = InitialDimensionAndDensity.ToString

            strInitialDimensionAndDensity = strInitialDimensionAndDensity.PadLeft(2, "0")

            strChangeLevel = "0"

            strNewDrawingNo = strSubFamilyID.PadLeft(4, "0") & "-" & strInitialDimensionAndDensity                       'PORTION: 1234-56
            strNewDrawingNo = strNewDrawingNo & strNumberSequence.PadLeft(3, "0")                                        'PORTION: 789 
            strNewDrawingNo = strNewDrawingNo & "-" & InStepTracking.ToString & "(" & strChangeLevel.PadLeft(2, "0") & ")"   'PORTION: -A(BB) 

            ''check to make sure new part number is not already used
            'ds = PEModule.GetDrawing(strNewDrawingNo, "", "", "", "", 0, "", "", 0, 0, 0, 0, 0, 0, "", "", "", 0, True, "", "")
            ds = PEModule.GetDrawing(strNewDrawingNo)

            'if the part number exist, increment the numSeq and retry            
            While (iRowCount > 0 Or iAltRowCount > 0)

                iNumberSequence += 1

                strNewDrawingNo = strSubFamilyID.PadLeft(4, "0") + "-" + strInitialDimensionAndDensity                   'PORTION: 1234-56
                strNewDrawingNo = strNewDrawingNo + iNumberSequence.ToString.PadLeft(3, "0")                             'PORTION: 789 

                'first get 1234-56789 section to see if it exists. If it does not exist, use it and append step and revision later.
                'If this section of the drawing number exists, increment the 789 digits
                'ds = PEModule.GetDrawing(strNewDrawingNo & "%", "", "", "", "", 0, "", "", 0, 0, 0, 0, 0, 0, "", "", "", 0, True, "", "")
                ds = PEModule.GetDrawing(strNewDrawingNo & "%")
                iRowCount = ds.Tables.Item(0).Rows.Count

                'check if X or simular number with just UGN Facility is in front of new drawing number
                'ds = PEModule.GetDrawing("%" & strNewDrawingNo & "%", "", "", "", "", 0, "", "", 0, 0, 0, 0, 0, 0, "", "", "", 0, True, "", "")
                ds = PEModule.GetDrawing("%" & strNewDrawingNo & "%")
                iAltRowCount = ds.Tables.Item(0).Rows.Count

                If iRowCount = 0 And iAltRowCount = 0 Then
                    '1234-56789 is available to use, check if the step and revision is too
                    strNewDrawingNo = strNewDrawingNo + "-" + InStepTracking.ToString + "(" + strChangeLevel.PadLeft(2, "0") + ")"    'PORTION: -A(BB) 
                    'ds = PEModule.GetDrawing(strNewDrawingNo, "", "", "", "", 0, "", "", 0, 0, 0, 0, 0, 0, "", "", "", 0, True, "", "")
                    ds = PEModule.GetDrawing(strNewDrawingNo)
                    iRowCount = ds.Tables.Item(0).Rows.Count

                    'ds = PEModule.GetDrawing("%" & strNewDrawingNo, "", "", "", "", 0, "", "", 0, 0, 0, 0, 0, 0, "", "", "", 0, True, "", "")
                    ds = PEModule.GetDrawing("%" & strNewDrawingNo)
                    iAltRowCount = ds.Tables.Item(0).Rows.Count
                End If

            End While

            GenerateDrawingNo = strNewDrawingNo

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "CopyDrawingImage : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("CopyDrawingImage : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GenerateDrawingNo = ""
        End Try

    End Function

    Public Shared Function GetDrawingReleaseTypeList() As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Drawing_Release_Type_List"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetDrawingReleaseTypeList")
            GetDrawingReleaseTypeList = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetDrawingReleaseTypeList : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDrawingReleaseTypeList : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
            Return Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetDrawingApprovedVendor(ByVal DrawingNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Drawing_Approved_Vendor"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandTimeout = 300

            If DrawingNo Is Nothing Then
                DrawingNo = ""
            End If

            myCommand.Parameters.Add("@drawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@drawingNo").Value = commonFunctions.convertSpecialChar(DrawingNo, False)

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetDrawingApprovedVendor")
            GetDrawingApprovedVendor = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetDrawingApprovedVendor : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDrawingApprovedVendor : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetDrawingApprovedVendor = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
    Public Shared Function GetDrawingUnapprovedVendor(ByVal DrawingNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Drawing_Unapproved_Vendor"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandTimeout = 300

            If DrawingNo Is Nothing Then
                DrawingNo = ""
            End If

            myCommand.Parameters.Add("@drawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@drawingNo").Value = commonFunctions.convertSpecialChar(DrawingNo, False)

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "GetDrawingUnapprovedVendor")
            GetDrawingUnapprovedVendor = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetDrawingUnapprovedVendor : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDrawingUnapprovedVendor : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetDrawingUnapprovedVendor = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Sub UpdateSubDrawing(ByVal RowID As Integer, ByVal SubDrawingNo As String, ByVal DrawingQuantity As Double, _
    ByVal Notes As String, ByVal Process As String, ByVal Equipment As String, ByVal ProcessParameters As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Sub_Drawing"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RowID", SqlDbType.Int)
            myCommand.Parameters("@RowID").Value = RowID

            myCommand.Parameters.Add("@SubDrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@SubDrawingNo").Value = SubDrawingNo

            myCommand.Parameters.Add("@DrawingQuantity", SqlDbType.Decimal)
            myCommand.Parameters("@DrawingQuantity").Value = DrawingQuantity

            myCommand.Parameters.Add("@Notes", SqlDbType.VarChar)
            myCommand.Parameters("@Notes").Value = Notes

            myCommand.Parameters.Add("@Process", SqlDbType.VarChar)
            myCommand.Parameters("@Process").Value = Process

            myCommand.Parameters.Add("@Equipment", SqlDbType.VarChar)
            myCommand.Parameters("@Equipment").Value = Equipment

            myCommand.Parameters.Add("@ProcessParameters", SqlDbType.VarChar)
            myCommand.Parameters("@ProcessParameters").Value = ProcessParameters

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & RowID _
            & ", SubDrawingNo: " & SubDrawingNo & ", DrawingQuantity: " & DrawingQuantity _
            & ", Notes: " & Notes _
            & ", Process: " & Process _
            & ", Equipment: " & Equipment _
            & ", ProcessParameters: " & ProcessParameters _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateSubDrawing : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingList.aspx"
            UGNErrorTrapping.InsertErrorLog("UpdateSubDrawing : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
#Region "DrawingMaterialSpecifications"

    Public Shared Function GetDrawingMaterialSpec(ByVal MaterialSpecNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Drawing_Material_Spec"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandTimeout = 300

            If MaterialSpecNo Is Nothing Then
                MaterialSpecNo = ""
            End If

            myCommand.Parameters.Add("@MaterialSpecNo", SqlDbType.VarChar)
            myCommand.Parameters("@MaterialSpecNo").Value = commonFunctions.convertSpecialChar(MaterialSpecNo, False)

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "MaterialSpecNo")
            GetDrawingMaterialSpec = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "MaterialSpecNo: " & MaterialSpecNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetDrawingMaterialSpec : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDrawingMaterialSpec : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetDrawingMaterialSpec = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetDrawingMaterialSpecSearch(ByVal MaterialSpecNo As String, ByVal MaterialSpecDesc As String, _
        ByVal StartRevisionDate As String, ByVal EndRevisionDate As String, _
        ByVal SubfamilyID As String, ByVal AreaWeight As String, ByVal DrawingNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Drawing_Material_Spec_Search"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandTimeout = 300

            If MaterialSpecNo Is Nothing Then
                MaterialSpecNo = ""
            End If

            myCommand.Parameters.Add("@MaterialSpecNo", SqlDbType.VarChar)
            myCommand.Parameters("@MaterialSpecNo").Value = commonFunctions.convertSpecialChar(MaterialSpecNo, False)

            If MaterialSpecDesc Is Nothing Then
                MaterialSpecDesc = ""
            End If

            myCommand.Parameters.Add("@MaterialSpecDesc", SqlDbType.VarChar)
            myCommand.Parameters("@MaterialSpecDesc").Value = commonFunctions.convertSpecialChar(MaterialSpecDesc, False)

            If StartRevisionDate Is Nothing Then
                StartRevisionDate = ""
            End If

            myCommand.Parameters.Add("@StartRevisionDate", SqlDbType.VarChar)
            myCommand.Parameters("@StartRevisionDate").Value = commonFunctions.convertSpecialChar(StartRevisionDate, False)

            If EndRevisionDate Is Nothing Then
                EndRevisionDate = ""
            End If

            myCommand.Parameters.Add("@EndRevisionDate", SqlDbType.VarChar)
            myCommand.Parameters("@EndRevisionDate").Value = commonFunctions.convertSpecialChar(EndRevisionDate, False)

            If SubfamilyID Is Nothing Then
                SubfamilyID = ""
            End If

            myCommand.Parameters.Add("@SubfamilyID", SqlDbType.VarChar)
            myCommand.Parameters("@SubfamilyID").Value = SubfamilyID

            If AreaWeight Is Nothing Then
                AreaWeight = ""
            End If

            myCommand.Parameters.Add("@AreaWeight", SqlDbType.VarChar)
            myCommand.Parameters("@AreaWeight").Value = AreaWeight

            If DrawingNo Is Nothing Then
                DrawingNo = ""
            End If

            myCommand.Parameters.Add("@DrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@DrawingNo").Value = commonFunctions.convertSpecialChar(DrawingNo, False)

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "MaterialSpecList")
            GetDrawingMaterialSpecSearch = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "MaterialSpecNo: " & MaterialSpecNo _
            & ", MaterialSpecDesc" & MaterialSpecDesc _
            & ", StartRevisionDate: " & StartRevisionDate _
            & ", EndRevisionDate" & EndRevisionDate _
            & ", SubfamilyID" & SubfamilyID _
            & ", AreaWeight" & AreaWeight _
            & ", DrawingNo" & DrawingNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetDrawingMaterialSpecSearch : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDrawingMaterialSpecSearch : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetDrawingMaterialSpecSearch = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetDrawingMaterialSpecMatchKind(ByVal SubfamilyID As String, ByVal AreaWeight As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Drawing_Material_Spec_Match_Kind"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandTimeout = 300

            If SubfamilyID Is Nothing Then
                SubfamilyID = ""
            End If

            myCommand.Parameters.Add("@SubfamilyID", SqlDbType.VarChar)
            myCommand.Parameters("@SubfamilyID").Value = SubfamilyID

            If AreaWeight Is Nothing Then
                AreaWeight = ""
            End If

            myCommand.Parameters.Add("@AreaWeight", SqlDbType.Decimal)
            myCommand.Parameters("@AreaWeight").Value = AreaWeight


            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "MaterialSpecNo")
            GetDrawingMaterialSpecMatchKind = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "SubfamilyID: " & SubfamilyID _
            & ", AreaWeight: " & AreaWeight _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetDrawingMaterialSpecMatchKind : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDrawingMaterialSpecMatchKind : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetDrawingMaterialSpecMatchKind = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetDrawingMaterialSpecRelateByDrawingNo(ByVal DrawingNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Drawing_Material_Spec_Relate_By_DrawingNo"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.CommandTimeout = 300

            If DrawingNo Is Nothing Then
                DrawingNo = ""
            End If

            myCommand.Parameters.Add("@DrawingNo", SqlDbType.VarChar)
            myCommand.Parameters("@DrawingNo").Value = commonFunctions.convertSpecialChar(DrawingNo, False)

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "MaterialSpecNo")
            GetDrawingMaterialSpecRelateByDrawingNo = GetData
        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "DrawingNo: " & DrawingNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetDrawingMaterialSpecRelateByDrawingNo : " _
            & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDrawingMaterialSpecRelateByDrawingNo : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetDrawingMaterialSpecRelateByDrawingNo = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function InsertDrawingMaterialSpec(ByVal MaterialSpecNo As String, _
        ByVal MaterialSpecDesc As String, ByVal AreaWeight As Double, _
        ByVal SubfamilyID As String) As DataSet

        'ByVal RevisionDate As String, ByVal RevisionLevel As String

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Drawing_Material_Spec"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If MaterialSpecNo Is Nothing Then
                MaterialSpecNo = ""
            End If

            myCommand.Parameters.Add("@MaterialSpecNo", SqlDbType.VarChar)
            myCommand.Parameters("@MaterialSpecNo").Value = commonFunctions.convertSpecialChar(MaterialSpecNo, False)

            If MaterialSpecDesc Is Nothing Then
                MaterialSpecDesc = ""
            End If

            myCommand.Parameters.Add("@MaterialSpecDesc", SqlDbType.VarChar)
            myCommand.Parameters("@MaterialSpecDesc").Value = commonFunctions.convertSpecialChar(MaterialSpecDesc, False)

            myCommand.Parameters.Add("@AreaWeight", SqlDbType.Decimal)
            myCommand.Parameters("@AreaWeight").Value = AreaWeight

            If SubfamilyID Is Nothing Then
                SubfamilyID = ""
            End If

            myCommand.Parameters.Add("@SubfamilyID", SqlDbType.VarChar)
            myCommand.Parameters("@SubfamilyID").Value = SubfamilyID


            'If RevisionDate Is Nothing Then
            '    RevisionDate = ""
            'End If

            'myCommand.Parameters.Add("@RevisionDate", SqlDbType.VarChar)
            'myCommand.Parameters("@RevisionDate").Value = commonFunctions.convertSpecialChar(RevisionDate, False)

            'If RevisionLevel Is Nothing Then
            '    RevisionLevel = ""
            'End If

            'myCommand.Parameters.Add("@RevisionLevel", SqlDbType.VarChar)
            'myCommand.Parameters("@RevisionLevel").Value = commonFunctions.convertSpecialChar(RevisionLevel, False)

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "InsertDrawingMaterialSpec")
            InsertDrawingMaterialSpec = GetData

        Catch ex As Exception

            '& ", RevisionDate: " & RevisionDate _
            '& ", RevisionLevel: " & RevisionLevel _

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "MaterialSpecNo: " & MaterialSpecNo _
            & ", MaterialSpecDesc" & MaterialSpecDesc _
            & ", SubfamilyID" & SubfamilyID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertDrawingMaterialSpec : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/MaterialSpecDetail.aspx?MaterialSpecNo=" & MaterialSpecNo
            UGNErrorTrapping.InsertErrorLog("InsertDrawingMaterialSpec : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertDrawingMaterialSpec = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function UpdateDrawingMaterialSpec(ByVal MaterialSpecNo As String, ByVal MaterialSpecDesc As String) As DataSet

        'ByVal RevisionDate As String, ByVal RevisionLevel As String

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Drawing_Material_Spec"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If MaterialSpecNo Is Nothing Then
                MaterialSpecNo = ""
            End If

            myCommand.Parameters.Add("@MaterialSpecNo", SqlDbType.VarChar)
            myCommand.Parameters("@MaterialSpecNo").Value = commonFunctions.convertSpecialChar(MaterialSpecNo, False)

            If MaterialSpecDesc Is Nothing Then
                MaterialSpecDesc = ""
            End If

            myCommand.Parameters.Add("@MaterialSpecDesc", SqlDbType.VarChar)
            myCommand.Parameters("@MaterialSpecDesc").Value = commonFunctions.convertSpecialChar(MaterialSpecDesc, False)

            'If RevisionDate Is Nothing Then
            '    RevisionDate = ""
            'End If

            'myCommand.Parameters.Add("@RevisionDate", SqlDbType.VarChar)
            'myCommand.Parameters("@RevisionDate").Value = commonFunctions.convertSpecialChar(RevisionDate, False)

            'If RevisionLevel Is Nothing Then
            '    RevisionLevel = ""
            'End If

            'myCommand.Parameters.Add("@RevisionLevel", SqlDbType.VarChar)
            'myCommand.Parameters("@RevisionLevel").Value = commonFunctions.convertSpecialChar(RevisionLevel, False)

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "UpdateDrawingMaterialSpec")
            UpdateDrawingMaterialSpec = GetData
        Catch ex As Exception

            '& ", RevisionDate: " & RevisionDate _
            '& ", RevisionLevel: " & RevisionLevel _

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "MaterialSpecNo: " & MaterialSpecNo & ", MaterialSpecDesc" & MaterialSpecDesc _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateDrawingMaterialSpec : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/MaterialsSpecDetail.aspx?MaterialSpecNo=" & MaterialSpecNo
            UGNErrorTrapping.InsertErrorLog("UpdateDrawingMaterialSpec : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            UpdateDrawingMaterialSpec = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Sub DeleteDrawingMaterialSpec(ByVal MaterialSpecNo As String)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Drawing_Material_Spec"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
       
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If MaterialSpecNo Is Nothing Then
                MaterialSpecNo = ""
            End If

            myCommand.Parameters.Add("@MaterialSpecNo", SqlDbType.VarChar)
            myCommand.Parameters("@MaterialSpecNo").Value = MaterialSpecNo

            myCommand.Parameters.Add("@updatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@updatedBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "MaterialSpecNo: " & MaterialSpecNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteDrawingMaterialSpec : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/MaterialSpecDetail.aspx?MaterialSpecNo=" & MaterialSpecNo
            UGNErrorTrapping.InsertErrorLog("DeleteDrawingMaterialSpec : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub

    Public Shared Sub DeletePEMaterialSpecCookies()

        Try
            HttpContext.Current.Response.Cookies("PEModule_SaveMaterialSpecNo").Value = ""
            HttpContext.Current.Response.Cookies("PEModule_SaveMaterialSpecNo").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PEModule_SaveMaterialSpecDesc").Value = ""
            HttpContext.Current.Response.Cookies("PEModule_SaveMaterialSpecDesc").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PEModule_SaveMaterialSpecSubfamilyID").Value = 0
            HttpContext.Current.Response.Cookies("PEModule_SaveMaterialSpecSubfamilyID").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PEModule_SaveMaterialAreaWeight").Value = ""
            HttpContext.Current.Response.Cookies("PEModule_SaveMaterialAreaWeight").Expires = DateTime.Now.AddDays(-1)

            HttpContext.Current.Response.Cookies("PEModule_SaveMaterialDrawingNo").Value = ""
            HttpContext.Current.Response.Cookies("PEModule_SaveMaterialDrawingNo").Expires = DateTime.Now.AddDays(-1)

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value
            HttpContext.Current.Session("BLLerror") = "DeletePEMaterialSpecCookies : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("DeletePEMaterialSpecCookies : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        End Try

    End Sub

    Public Shared Function InsertDrawingMaterialSpecSupportingDoc(ByVal MaterialSpecNo As String, _
       ByVal SupportingDocDesc As String, ByVal SupportingDocName As String, _
       ByVal DocBytes As Byte(), ByVal EncodeType As String, ByVal FileSize As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Drawing_Material_Spec_Supporting_Doc"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If MaterialSpecNo Is Nothing Then
                MaterialSpecNo = ""
            End If

            myCommand.Parameters.Add("@MaterialSpecNo", SqlDbType.VarChar)
            myCommand.Parameters("@MaterialSpecNo").Value = MaterialSpecNo

            If SupportingDocDesc Is Nothing Then
                SupportingDocDesc = ""
            End If

            myCommand.Parameters.Add("@SupportingDocDesc", SqlDbType.VarChar)
            myCommand.Parameters("@SupportingDocDesc").Value = commonFunctions.convertSpecialChar(SupportingDocDesc, False)

            If SupportingDocName Is Nothing Then
                SupportingDocName = ""
            End If

            myCommand.Parameters.Add("@SupportingDocName", SqlDbType.VarChar)
            myCommand.Parameters("@SupportingDocName").Value = SupportingDocName

            myCommand.Parameters.Add("@supportingDocBinary", SqlDbType.VarBinary)
            myCommand.Parameters("@supportingDocBinary").Value = DocBytes

            myCommand.Parameters.Add("@EncodeType", SqlDbType.VarChar)
            myCommand.Parameters("@EncodeType").Value = commonFunctions.convertSpecialChar(EncodeType, False)

            myCommand.Parameters.Add("@BinaryFileSizeinBytes", SqlDbType.Int)
            myCommand.Parameters("@BinaryFileSizeinBytes").Value = FileSize

            myCommand.Parameters.Add("@createdBy", SqlDbType.VarChar)
            myCommand.Parameters("@createdBy").Value = HttpContext.Current.Request.Cookies("UGNDB_User").Value

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "NewDrawingMaterialSpecSupportingDoc")
            InsertDrawingMaterialSpecSupportingDoc = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "MaterialSpecNo: " & MaterialSpecNo _
            & ", SupportingDocDesc: " & SupportingDocDesc _
            & ", SupportingDocName: " & SupportingDocName _
            & ", EncodeType: " & EncodeType _
            & ", FileSize: " & FileSize _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertDrawingMaterialSpecSupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("InsertDrawingMaterialSpecSupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            InsertDrawingMaterialSpecSupportingDoc = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetDrawingMaterialSpecSupportingDoc(ByVal RowID As Integer) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Drawing_Material_Spec_Supporting_Doc"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        HttpContext.Current.Session("BLLerror") = Nothing

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@RowID", SqlDbType.Int)
            myCommand.Parameters("@RowID").Value = RowID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "DrawingMaterialSpecSupportingDoc")
            GetDrawingMaterialSpecSupportingDoc = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "RowID: " & RowID _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetDrawingMaterialSpecSupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Home.aspx"
            UGNErrorTrapping.InsertErrorLog("GetDrawingMaterialSpecSupportingDoc : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetDrawingMaterialSpecSupportingDoc = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function

    Public Shared Function GetDrawingMaterialSpecMaxRevision(ByVal MaterialSpecNo As String) As DataSet

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Drawing_Material_Spec_Max_Revision"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            If MaterialSpecNo = Nothing Then
                MaterialSpecNo = ""
            End If

            myCommand.Parameters.Add("@MaterialSpecNo", SqlDbType.VarChar)
            myCommand.Parameters("@MaterialSpecNo").Value = MaterialSpecNo

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "Drawing")

            GetDrawingMaterialSpecMaxRevision = GetData

        Catch ex As Exception

            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "MaterialSpecNo: " & MaterialSpecNo _
            & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetDrawingMaterialSpecMaxRevision : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> PEModule.vb :<br/> " & strUserEditedData
            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/PE/DrawingMaterialSpecDetail.aspx?MaterialSpecNo=" & MaterialSpecNo
            UGNErrorTrapping.InsertErrorLog("GetDrawingMaterialSpecMaxRevision : " & commonFunctions.convertSpecialChar(ex.Message, False), "PEModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetDrawingMaterialSpecMaxRevision = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function
#End Region


End Class
