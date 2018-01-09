' ************************************************************************************************
'
' Name:		AutoComplete.vb
' Purpose:	This Code Behind for a web service to be used by the UGN DB Vendor list to automatically search for vendors based on what the user types
'
' Date		Author	    
' 2009       Roderick Carlson
' 08/25/2010 Roderick Carlson - adjusted extra isActiveBPCSOnly parameter

' ************************************************************************************************
Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Collections.Generic

<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
<System.Web.Script.Services.ScriptService()> _
Public Class AutoComplete
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function GetVendorList(ByVal prefixText As String, ByVal count As Integer) As String()

        prefixText = prefixText & "%"

        If (count = 0) Then
            count = 10
        End If

        Dim items As New List(Of String)

        Dim ds As DataSet

        Dim strResultText As String = ""

        Dim strUGNDBVendorName As String = ""
        Dim strBPCSVendorName As String = ""

        Dim iRowCounter As Integer = 0

        ds = commonFunctions.GetUGNDBVendor(0, "", prefixText, False)

        If commonFunctions.CheckDataSet(ds) = True Then

            If ds.Tables(0).Rows.Count < count Then
                count = ds.Tables(0).Rows.Count - 1
            End If

            For iRowCounter = 0 To count
                strResultText = ""

                If ds.Tables(0).Rows(iRowCounter).Item("BPCSVendorID") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(iRowCounter).Item("BPCSVendorID") > 0 Then
                        strResultText = ds.Tables(0).Rows(iRowCounter).Item("BPCSVendorID").ToString & " | " & ds.Tables(0).Rows(iRowCounter).Item("BPCSVendorName").ToString()
                    End If
                End If

                If strResultText.Trim <> "" Then
                    strResultText += " | "
                End If

                If ds.Tables(0).Rows(iRowCounter).Item("ddUGNDBVendorName").ToString.Trim <> "" Then
                    strResultText += ds.Tables(0).Rows(iRowCounter).Item("ddUGNDBVendorName").ToString()
                End If

                If strResultText.Trim <> "" Then
                    items.Add(strResultText)
                End If

            Next iRowCounter

        End If

        ds = commonFunctions.GetVendor(0, prefixText, "", "", "", "", "", "", "")

       If commonFunctions.CheckDataSet(ds) = True Then

            If ds.Tables(0).Rows.Count < count Then
                count = ds.Tables(0).Rows.Count - 1
            End If

            For iRowCounter = 0 To count
                strResultText = ""

                If ds.Tables(0).Rows(iRowCounter).Item("VENDOR") IsNot System.DBNull.Value Then
                    If ds.Tables(0).Rows(iRowCounter).Item("VENDOR") > 0 Then
                        strResultText = ds.Tables(0).Rows(iRowCounter).Item("VENDOR").ToString & " | " & ds.Tables(0).Rows(iRowCounter).Item("VNDNAM").ToString()
                    End If
                End If

                If strResultText.Trim <> "" Then
                    items.Add(strResultText)
                End If

            Next iRowCounter      
        End If

        If items.Count = 0 Then
            items.Add("no matches found yet")
        End If

        Return items.ToArray()

    End Function
    <WebMethod()> _
    Public Function GetCompletionList(ByVal prefixText As String, ByVal count As Integer) As String()
        Dim c1 As Char
        Dim c2 As Char
        Dim c3 As Char

        If (count = 0) Then
            count = 10
        End If

        Dim rnd As New Random()

        Dim items As New List(Of String)

        For i As Integer = 1 To count

            c1 = CStr(rnd.Next(65, 90))
            c2 = CStr(rnd.Next(97, 122))
            c3 = CStr(rnd.Next(97, 122))

            items.Add(prefixText + c1 + c2 + c3)
        Next i

        Return items.ToArray()
    End Function
End Class
