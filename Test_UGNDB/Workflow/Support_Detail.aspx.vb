' ************************************************************************************************
'
' Name:		Support_Detail.aspx
' Purpose:	This Code Behind is for the Workflow Support Detail page. This uses a master page and the ASCX control that hold most of the page
'
' Date		    Author	    
' 12/06/2011    Roderick Carlson
'
' ************************************************************************************************
Partial Class Support_Detail
    Inherits System.Web.UI.Page

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try
            'If ViewState("ViewType") = "P" Then
            '    '    Dim m As ASP.LookUpMasterPage.master = Master
            'Else
            Dim m As ASP.masterpage_master = Master

            m.PageTitle = "UGN, Inc."
            m.ContentLabel = "Support Detail"
            ''***********************************************
            ''Code Below overrides the breadcrumb navigation 
            ''***********************************************
            Dim mpTextBox As Label
            mpTextBox = CType(Master.FindControl("lblOtherSiteNode"), Label)
            If Not mpTextBox Is Nothing Then
                mpTextBox.Text = "<a href='../Home.aspx'><b>Home</b></a> > <b>Work Flow</b> <a href='Support_List.aspx'><b>Support Search</b></a> > Support Detail"
                mpTextBox.Visible = True
                Master.FindControl("SiteMapPath1").Visible = False
            End If

            ''*****
            ''Expand menu item
            ''*****
            Dim MasterPanel As CollapsiblePanelExtender
            MasterPanel = CType(Master.FindControl("WFExtender"), CollapsiblePanelExtender)
            MasterPanel.Collapsed = False
            ' End If

            'update Support Link
            Dim hlnkSupport As HtmlAnchor = CType(Master.FindControl("hlnkSupportDetail"), HtmlAnchor)
            If hlnkSupport IsNot Nothing Then
                Session("SupportUrl") = Request.ServerVariables("URL")
                Session("SupportQueryString") = Request.ServerVariables("QUERY_STRING")

                'hlnkSupport.Attributes.Remove("href")
                'hlnkSupport.Attributes.Add("onclick", "javascript:void(window.open('../WorkFlow/Support_Detail_Popup.aspx?BMID=SAF','_blank','top=5,left=5,resizable=yes,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no'));")

            End If

        Catch ex As Exception
            'update error on web page
            lblPageMessage.Text = ex.Message

            'get current event name
            Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

            'log and email error
            UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
        End Try

    End Sub


    'Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

    '    Try
    '        '' ******************************************
    '        '' DETERMINE WHICH MASTER PAGE TO LOAD
    '        '' ******************************************

    '        ''Dim strViewType As String = "M" 'Regular Master Page
    '        'ViewState("ViewType") = "M"

    '        'Dim m As ASP.masterpage_master = Master

    '        'If Request.QueryString("ViewType") IsNot Nothing Then
    '        '    ViewState("ViewType") = Request.QueryString("ViewType").ToString  'P would be a popup
    '        'End If

    '        ''ViewState("ViewType") = "M"

    '        'If ViewState("ViewType") = "P" Then
    '        '    MasterPageFile = "~/LookUpMasterPage.master"
    '        'Else 'regular maters
    '        '    MasterPageFile = "~/MasterPage.master"
    '        'End If

    '    Catch ex As Exception
    '        'update error on web page
    '        lblPageMessage.Text = ex.Message

    '        'get current event name
    '        Dim mb As Reflection.MethodBase = Reflection.MethodBase.GetCurrentMethod

    '        'log and email error
    '        UGNErrorTrapping.UpdateUGNErrorLog(mb.Name & ":" & ex.Message, System.Web.HttpContext.Current.Request.Url.AbsolutePath)
    '    End Try

    'End Sub

    
End Class
