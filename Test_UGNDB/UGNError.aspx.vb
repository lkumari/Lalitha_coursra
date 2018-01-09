''******************************************************************************************************
''* UGNError.vb
''* This error page shows the screen information and page of the error for users to report to IS. It is also logged and emailed.
''*  
''*
''* Author  : Roderick Carlson 2008
''* Modified: {Name} {Date} - {Notes}
''*           Roderick Carlson - check if Session Variable UGNErrorLastWebPage is nothing, set default value
''******************************************************************************************************

Partial Class UGNError
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim strProdOrTestEnvironment As String = System.Configuration.ConfigurationManager.AppSettings("DBInstance").ToString()

        If strProdOrTestEnvironment = "Test_UGNDB" Then
            lblHeaderTitle.Text = "Error Page: Test UGN Database"
        Else
            lblHeaderTitle.Text = "Error Page: UGN Database"
        End If

        If HttpContext.Current.Session("BLLerror") IsNot Nothing Then
            lblMessage.Text = HttpContext.Current.Session("BLLerror").ToString
            HttpContext.Current.Session("BLLerror") = Nothing

            If HttpContext.Current.Session("UGNErrorLastWebPage") IsNot Nothing Then
                lnkGoBack.PostBackUrl = HttpContext.Current.Session("UGNErrorLastWebPage").ToString
            Else
                lnkGoBack.PostBackUrl = "~/Home.aspx"
            End If

            HttpContext.Current.Session("UGNErrorLastWebPage") = Nothing
        End If

    End Sub
End Class
