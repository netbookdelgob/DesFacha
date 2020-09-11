Imports System.Net

Public Class kvrtbotoniniciar
    Private Sub kvrtbotoniniciar_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        kvrt1 = False
        Dim directory As String = My.Application.Info.DirectoryPath

        Dim strPath As String = directory + "\KVRT.exe"
        Dim client As WebClient = New WebClient
        Dim url As String = "http://devbuilds.kaspersky-labs.com/devbuilds/KVRT/latest/full/KVRT.exe"
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
        kvrt1 = True
        Me.Close()
    End Sub
End Class