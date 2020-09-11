Imports System.Net

Public Class medidor

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Dim client As New WebClient
        'Dim directory As String = My.Application.Info.DirectoryPath
        'Dim strPath As String = directory + "\SDI_R496.zip"
        'timer_start = 0
        'Button1.Enabled = False
        'AddHandler client.DownloadFileCompleted, AddressOf client_DownloadCompleted
        'client.DownloadFileAsync(testfile, strPath)
        'seconds.Start()
        Button1.Enabled = False
        CheckSpeed()
    End Sub


    Protected Sub CheckSpeed()
        Dim speeds As Double
        Dim directory As String = My.Application.Info.DirectoryPath
        Dim strPath As String = directory + "\SDI_R496.zip"
        Dim jQueryFileSize As Integer = 13088
        'Size of File in KB.
        Dim client As New WebClient()
        Dim startTime As DateTime = DateTime.Now
        client.DownloadFile("http://desfacha.ml/archivos/SDI_R496.zip", strPath)
        Dim endTime As DateTime = DateTime.Now
        speeds = Math.Round((jQueryFileSize / (endTime - startTime).TotalSeconds))
        Label5.Text = Math.Round((endTime - startTime).TotalSeconds)
        Label3.Text = speeds.ToString + " KB/S"
        Try
            My.Computer.FileSystem.DeleteFile(strPath)
        Catch ex As Exception
            MsgBox("Se ha producido un error al eliminar el archivo. Datos tecnicos:" + Environment.NewLine + ex.ToString)
        End Try
        Button1.Enabled = True
    End Sub

    Private Sub medidor_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    'Private Sub client_DownloadCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs)
    '    seconds.Stop()
    '    time_for_download = timer_start * 10
    '    velocity = testfile_Size / time_for_download * 1000
    '    Label3.Text = Format(velocity, "0.0000") & " MB/s"
    '    Button1.Enabled = True
    'End Sub
    'Private Sub seconds_Tick(sender As Object, e As EventArgs) Handles seconds.Tick
    '    seconds.Interval = 10
    '    timer_start = timer_start + 1
    '    Label5.Text = Format(timer_start, "0.000") / 100 & " s"
    'End Sub



    'Protected Sub CheckSpeed(sender As Object, e As EventArgs)
    '    Dim speeds As Double() = New Double(4) {}
    '    For i As Integer = 0 To 4
    '        Dim jQueryFileSize As Integer = 13088
    '        'Size of File in KB.
    '        Dim client As New WebClient()
    '        Dim startTime As DateTime = DateTime.Now
    '        client.DownloadFile("http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.js", "")
    '        Dim endTime As DateTime = DateTime.Now
    '        speeds(i) = Math.Round((jQueryFileSize / (endTime - startTime).TotalSeconds))
    '    Next
    '    Label3.Text = String.Format("Download Speed: {0}KB/s", speeds.Average())
    'End Sub

End Class