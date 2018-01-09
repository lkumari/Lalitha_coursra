''************************************************************************************************
''Name:		CalendarModule.vb
''Purpose:	This code is referenced from all vb files, mostly for the purpose of calling stored 
''          procedures or getting user-rights for the Workflow Module
''
''Date		    Author	    
''05/15/2009    LRey			Created .Net application
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
Imports Microsoft.VisualBasic

Public Class CalendarModule
    Public Shared Sub InsertCustomerShutDownCalendar(ByVal UGNFacility As String, ByVal OEM As String, ByVal CABBV As String, ByVal SoldTo As Integer, ByVal StartDate As String, ByVal EndDate As String, ByVal WkEndWorkDay As Boolean)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Insert_Customer_Shut_Down_Calendar"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@OEM", SqlDbType.VarChar)
            myCommand.Parameters("@OEM").Value = OEM

            myCommand.Parameters.Add("@CABBV", SqlDbType.VarChar)
            myCommand.Parameters("@CABBV").Value = CABBV

            myCommand.Parameters.Add("@SoldTo", SqlDbType.Int)
            myCommand.Parameters("@SoldTo").Value = SoldTo

            myCommand.Parameters.Add("@StartDate", SqlDbType.VarChar)
            myCommand.Parameters("@StartDate").Value = StartDate

            myCommand.Parameters.Add("@EndDate", SqlDbType.VarChar)
            myCommand.Parameters("@EndDate").Value = EndDate

            myCommand.Parameters.Add("@WkEndWorkDay", SqlDbType.Bit)
            myCommand.Parameters("@WkEndWorkDay").Value = WkEndWorkDay

            myCommand.Parameters.Add("@CreatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@CreatedBy").Value = User

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "InsertCustomerShutDownCalendar : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CalendarModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Calendars/CustomerShutDownCalendar.aspx?sView=week"

            UGNErrorTrapping.InsertErrorLog("InsertCustomerShutDownCalendar : " & commonFunctions.convertSpecialChar(ex.Message, False), "CalendarModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF InsertCustomerShutDownCalendar

    Public Shared Sub UpdateCustomerShutDownCalendar(ByVal CID As Integer, ByVal UGNFacility As String, ByVal OEM As String, ByVal CABBV As String, ByVal SoldTo As Integer, ByVal StartDate As String, ByVal EndDate As String, ByVal WkEndWorkDay As Boolean)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Update_Customer_Shut_Down_Calendar"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CID", SqlDbType.Int)
            myCommand.Parameters("@CID").Value = CID

            myCommand.Parameters.Add("@UGNFacility", SqlDbType.VarChar)
            myCommand.Parameters("@UGNFacility").Value = UGNFacility

            myCommand.Parameters.Add("@OEM", SqlDbType.VarChar)
            myCommand.Parameters("@OEM").Value = OEM

            myCommand.Parameters.Add("@CABBV", SqlDbType.VarChar)
            myCommand.Parameters("@CABBV").Value = CABBV

            myCommand.Parameters.Add("@SoldTo", SqlDbType.Int)
            myCommand.Parameters("@SoldTo").Value = SoldTo

            myCommand.Parameters.Add("@StartDate", SqlDbType.VarChar)
            myCommand.Parameters("@StartDate").Value = StartDate

            myCommand.Parameters.Add("@WkEndWorkDay", SqlDbType.Bit)
            myCommand.Parameters("@WkEndWorkDay").Value = WkEndWorkDay

            myCommand.Parameters.Add("@UpdatedBy", SqlDbType.VarChar)
            myCommand.Parameters("@UpdatedBy").Value = User

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CID: " & CID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "UpdateCustomerShutDownCalendar : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CalendarModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Calendars/CustomerShutDownCalendar.aspx?sView=week"

            UGNErrorTrapping.InsertErrorLog("UpdateCustomerShutDownCalendar : " & commonFunctions.convertSpecialChar(ex.Message, False), "CalendarModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF UpdateCustomerShutDownCalendar

    Public Shared Sub DeleteCustomerShutDownCalendar(ByVal CID As Integer)

        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Delete_Customer_Shut_Down_Calendar"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim User As String = HttpContext.Current.Request.Cookies("UGNDB_User").Value

        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CID", SqlDbType.Int)
            myCommand.Parameters("@CID").Value = CID

            myConnection.Open()
            myCommand.ExecuteNonQuery()

        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CID: " & CID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "DeleteCustomerShutDownCalendar : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CalendarModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Calendars/CustomerShutDownCalendar.aspx?sView=week"

            UGNErrorTrapping.InsertErrorLog("DeleteCustomerShutDownCalendar : " & commonFunctions.convertSpecialChar(ex.Message, False), "CalendarModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try
    End Sub 'EOF DeleteCustomerShutDownCalendar

    Public Shared Function GetCustomerShutDownCalendarByCID(ByVal CID As String) As DataSet
        ''Used in 
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Customer_Shut_Down_Calendar_By_CID"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@CID", SqlDbType.Int)
            myCommand.Parameters("@CID").Value = CID

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CID")
            GetCustomerShutDownCalendarByCID = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "CID: " & CID & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCustomerShutDownCalendarByCID : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CalendarModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Calendars/CustomerShutDownCalendar.aspx?sView=week"

            UGNErrorTrapping.InsertErrorLog("GetCustomerShutDownCalendarByCID : " & commonFunctions.convertSpecialChar(ex.Message, False), "CalendarModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCustomerShutDownCalendarByCID = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetCustomerShutDownCalendarByCID


    Public Shared Function GetCustomerShutDownCalendar(ByVal StartDate As String) As DataSet
        ''Used in 
        Dim myConnection As SqlConnection = New SqlConnection
        Dim strConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("SQLConnection").ToString
        Dim strStoredProcName As String = "sp_Get_Customer_Shut_Down_Calendar"
        Dim myCommand As SqlCommand = New SqlCommand(strStoredProcName, myConnection)
        Dim GetData As New DataSet
        Dim myAdapter As New SqlDataAdapter
        Try
            myConnection.ConnectionString = strConnectionString
            myCommand.CommandType = CommandType.StoredProcedure

            myCommand.Parameters.Add("@StartDate", SqlDbType.VarChar)
            myCommand.Parameters("@StartDate").Value = StartDate

            myAdapter = New SqlDataAdapter(myCommand)
            myAdapter.Fill(GetData, "CID")
            GetCustomerShutDownCalendar = GetData
        Catch ex As Exception
            'on error, collect function data, error, and last page, then redirect to error page
            Dim strUserEditedData As String = "StartDate: " & StartDate & ", User: " & HttpContext.Current.Request.Cookies("UGNDB_User").Value

            HttpContext.Current.Session("BLLerror") = "GetCustomerShutDownCalendar : " & commonFunctions.convertSpecialChar(ex.Message, False) & " :<br/> CalendarModule.vb :<br/> " & strUserEditedData

            HttpContext.Current.Session("UGNErrorLastWebPage") = "~/Calendars/CustomerShutDownCalendar.aspx?sView=week"

            UGNErrorTrapping.InsertErrorLog("GetCustomerShutDownCalendar : " & commonFunctions.convertSpecialChar(ex.Message, False), "CalendarModule.vb", strUserEditedData)

            HttpContext.Current.Response.Redirect("~/UGNError.aspx", False)

            GetCustomerShutDownCalendar = Nothing
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Function 'EOF GetCustomerShutDownCalendar
End Class
