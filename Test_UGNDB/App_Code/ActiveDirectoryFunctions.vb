Option Explicit On
Option Strict On

Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.Data
Imports System.DirectoryServices
Imports System.Diagnostics

''' ==============================================================
'''  The ActiveDirectoryFunctions class can be used to export a 
'''  list of Active Directory Users and work locations.
''' 
'''  Written by: M.Weyker 2/26/2008
''' 
'''  Modification History
'''  --------------------
'''  08/26/2008  MWeyker  Added standard exception reporting,
'''                       using UGNErrorTrapping class.	
''' ==============================================================
Public Class ActiveDirectoryFunctions
    Inherits System.ComponentModel.Component

    ''' ---------------------------------------------------------------
    '''  This enum specifies the list of Active Directory 
    '''  user properties exported by this class. The Active Directory
    '''  users are exported as a DataTable named "ADUsers". 
    '''  The DataTable columns are named after the Active Directory
    '''  properties in this list.
    ''' ---------------------------------------------------------------
    Public Enum UserProperties As Integer
        sn                   ' lname
        givenname            ' fname
        'displayname         ' lname, fname
        samaccountname       ' fname.lname
        mail                 ' fname.lname@ugnusa.com
        l                    ' work at location (city name)
        'distinguishedname
        'department          ' employee department
        'title               ' employee title
    End Enum

    ''' -----------------------------------------------------------------
    ''' The GetAdLocations function returns the "ADLocations" DataTable
    ''' of Active Directory user work locations as a DataView.
    ''' -----------------------------------------------------------------
    Public Shared Function GetADLocations() As DataView
        Try
            Dim dvData As DataView = New DataView(GetADData.Tables("ADLocations"))
            Return dvData
        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)

            Return Nothing
        End Try
    End Function

    ''' -----------------------------------------------------------------
    '''  The GetAdUsers function returns the "ADUsers" DataTable 
    '''  of Active Directory users as a DataView. The input parameters
    '''  are used to build a filter on the DataView.
    ''' 
    '''  Input Parameters:
    '''   lname - Last Name (the AD "sn" property)
    '''   fname - First Name (the AD "givenname" property)
    '''   location - Employee Work Location (the AD "l" property)
    ''' 
    '''  Returns:
    '''   A DataView of the ADUsers DataTable, filtered by
    '''   the input parameters.
    ''' -----------------------------------------------------------------
    Public Shared Function GetAdUsers(ByVal lname As String, ByVal fname As String, _
        ByVal location As String) As DataView


        ' Build the RowFilter string from the input parameters
        Dim s As String = ""
        If Not String.IsNullOrEmpty(lname) Then
            s = String.Concat(s, "sn LIKE '" & lname.Trim() & "'")
        End If
        If Not String.IsNullOrEmpty(fname) Then
            If Not String.IsNullOrEmpty(s) Then
                s = String.Concat(s, " AND ")
            End If
            s = String.Concat(s, "givenname LIKE '" & fname.Trim() & "'")
        End If
        If Not String.IsNullOrEmpty(location) Then
            If Not String.IsNullOrEmpty(s) Then
                s = String.Concat(s, " AND ")
            End If
            s = String.Concat(s, "l = '" & location.Trim() & "'")
        End If

        Try
            ' Create and return a DataView of the "ADUsers" DataTable.
            Dim dvData As DataView = New DataView(GetADData.Tables("ADUsers"))
            dvData.RowFilter = s
            Return dvData
        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)

            Return Nothing
        End Try
    End Function

    ''' -----------------------------------------------------------------
    '''  The GetADData table function returns the "ADUsers" DataTable
    '''  of Active Directory Users and the "ADLocations" DataTable of
    '''  work locations in a DataSet.
    ''' -----------------------------------------------------------------
    Private Shared Function GetADData() As DataSet
        Dim dsData As DataSet
        Dim dtUsers As DataTable
        Dim dtLocations As DataTable

        Try
            ' Create a list for accumulating the list of
            ' user work locations.
            Dim distinctLocationsList As New ArrayList()

            ' Convert the column names to an array
            Dim columnNames() As String = [Enum].GetNames(GetType(UserProperties))

            '--------------------------------------------------
            '  Setup a DirectorySearcher to return all users
            '  in the UGNNET.COM domain, limiting the properties
            '  returned to those in the UserProperties Enum list.
            '--------------------------------------------------
            ' Start at the UGNNET.COM domain. 
            Dim deAd As New DirectoryEntry("LDAP://DC=UGNNET,DC=COM")
            Dim dsAd As New DirectorySearcher(deAd)
            Dim srAd As SearchResult

            ' Get the domain users,
            dsAd.SearchScope = SearchScope.Subtree
            dsAd.Filter() = "(&(objectCategory=person)(objectClass=user))"

            ' Limit the properties returned by DirectorySearcher 
            ' to those in the columnNames list
            For i As Integer = 0 To columnNames.Length() - 1
                dsAd.PropertiesToLoad.Add(columnNames(i))
            Next

            ' Sort the DirectorySearcher results by the "displayname"
            ' (lname, fname) property
            dsAd.Sort = New SortOption("displayname", SortDirection.Ascending)

            '---------------------------------------------------------
            ' Create a DataSet, consisting of two DataTables for
            ' capturing the DirectorySearcher results.
            '---------------------------------------------------------
            dsData = New DataSet()

            ' Create the "ADLocations" DataTable, 
            ' add one column, and setup an array
            ' to accumulate distinct instances of Locations.
            ' Add a null-string entry to the front of
            ' the array.
            dtLocations = New DataTable("ADLocations")
            dtLocations.Columns.Add("l", GetType(String))
            distinctLocationsList.Add("")

            ' Create the "ADUsers" DataTable, and
            ' add columns, one for
            ' each DirectoryEntry Property.
            dtUsers = New DataTable("ADUsers")
            For i As Integer = 0 To columnNames.Length() - 1
                dtUsers.Columns.Add(columnNames(i), GetType(String))
            Next

            ' Add each DataTable to the DataSet
            dsData.Tables.Add(dtUsers)
            dsData.Tables.Add(dtLocations)
            dtUsers.BeginLoadData()

            '-------------------------------------------------------
            ' Step through each user in the AD  search results...
            '-------------------------------------------------------
            For Each srAd In dsAd.FindAll

                Dim blnMissingProperties As Boolean = False

                ' Flag directory entries with missing 
                ' properties.
                For Each dc As DataColumn In dtUsers.Columns
                    If Not srAd.Properties.Contains(dc.ColumnName) Then
                        ' This entry does not contain one of 
                        ' the required properties.
                        blnMissingProperties = True
                        Exit For
                    End If
                Next

                ' If this directory entry got flagged as
                ' having missing properties, skip it.
                If blnMissingProperties Then
                    Continue For
                End If

                '''''''''''' FOR TESTING ''''''''''''''''''''''''''''''''''''''
                '' Display each property returned in the search (for testing)
                ''DisplayPropertyCollection(srAd)
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                ' Create a new row for the "ADUsers" DataTable
                Dim drNewRow As DataRow = dtUsers.NewRow()

                ' Populate each DataColumn of the new row with
                ' the corresponding directory entry property
                ' value.
                Dim loc As String = ""
                For Each dc As DataColumn In dtUsers.Columns
                    If dc.ColumnName.Equals("l") Then
                        loc = srAd.Properties(dc.ColumnName).Item(0).ToString()
                    End If
                    drNewRow(dc.ColumnName) = srAd.Properties(dc.ColumnName).Item(0).ToString
                Next

                ' Add the new row to the "ADUsers" table.
                dtUsers.Rows.Add(drNewRow)

                ' Add the "l" property to the Locations list
                ' if not already present.
                If Not distinctLocationsList.Contains(loc) Then
                    distinctLocationsList.Add(loc)
                End If

            Next srAd

            ' The "ADUsers" table has been populated with data.
            dtUsers.EndLoadData()

            ' Load the user work locations into the "ADLocations" table.
            dtLocations.BeginLoadData()
            distinctLocationsList.Sort()
            For Each obj As Object In distinctLocationsList
                Dim drNewRow As DataRow = dtLocations.NewRow()
                Dim x As String = CType(obj, String)
                drNewRow(0) = CType(obj, String)
                dtLocations.Rows.Add(drNewRow)
            Next
            dtLocations.EndLoadData()

            ' Success: Return the DataSet
            Return dsData

        Catch ex As Exception
            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, _
                System.Web.HttpContext.Current.Request.Url.AbsolutePath)

            ' Failure: Return Nothing
            Return Nothing
        End Try
    End Function


    ''' <summary>
    ''' Print the property collection of a
    ''' directory entry.
    ''' </summary>
    ''' <param name="result">The SearchResult to examine</param>
    ''' <remarks>This subroutine uses Debug.Writeline
    ''' to list the object's property collection</remarks>
    Private Shared Sub DisplayPropertyCollection(ByRef result As SearchResult)
        Dim pc As ResultPropertyCollection = result.Properties
        Debug.WriteLine("******** NEXT DIRECTORY ENTRY ***************")
        For Each myKey As String In pc.PropertyNames
            Dim tab1 As String = "     "
            Debug.Write(myKey & " = ")
            For Each myCollection As Object In pc(myKey)
                Debug.WriteLine(tab1 & myCollection.ToString)
                ' do this once
                Exit For
            Next myCollection
        Next myKey
    End Sub
End Class
