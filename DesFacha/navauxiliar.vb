Public Class navauxiliar
    Dim directory As String = My.Application.Info.DirectoryPath
    Private Sub navauxiliar_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        With Me

            TopMost = True
            WindowState = FormWindowState.Maximized
        End With
        WebBrowser1.Size = New Point(Me.Width, Me.Height)
        WebBrowser1.Navigate(directory + "\informacion.html")
    End Sub
End Class