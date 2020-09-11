Imports System.Net

Public Class ccleaner
    Private Sub ccleaner_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        paso3 = False
        Dim directory As String = My.Application.Info.DirectoryPath

        Dim strPath As String = directory + "\ccsetup525.exe"
        Dim client As WebClient = New WebClient
        Dim url As String = "http://download.piriform.com/ccsetup525.exe"
        Dim uri1 As New Uri(url)
        AddHandler client.DownloadProgressChanged, AddressOf client_ProgressChanged

        AddHandler client.DownloadFileCompleted, AddressOf client_DownloadCompleted
        client.DownloadFileAsync(uri1, strPath)
    End Sub

    Private Sub client_ProgressChanged(ByVal sender As Object, ByVal e As DownloadProgressChangedEventArgs)
        Dim bytesIn As Double = Double.Parse(e.BytesReceived.ToString())
        Dim totalBytes As Double = Double.Parse(e.TotalBytesToReceive.ToString())
        Dim percentage As Double = bytesIn / totalBytes * 100
        Dim progreso As String = Int32.Parse(Math.Truncate(percentage).ToString())

        ProgressBar1.Value = Int32.Parse(Math.Truncate(percentage).ToString())
        Label1.Text = progreso + " %"
    End Sub
    Private Sub client_DownloadCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs)
        paso3 = True
        Me.Close()
    End Sub

End Class