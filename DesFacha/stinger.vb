Imports System.Net
Imports System.Runtime.InteropServices

Public Class stinger
    Private Sub stinger_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim url As String = "d"
        If (GetOSArchitecture() = 64) Then
            url = "http://downloadcenter.mcafee.com/products/mcafee-avert/stinger/stinger64.exe"

        ElseIf GetOSArchitecture() = 32 Then
            url = "http://downloadcenter.mcafee.com/products/mcafee-avert/stinger/stinger32.exe"
        End If
        paso4 = False
        Dim directory As String = My.Application.Info.DirectoryPath

        Dim strPath As String = directory + "\stinger32.exe"
        Dim client As WebClient = New WebClient

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
        paso4 = True
        Me.Close()
    End Sub

    Private Function GetOSArchitecture() As Integer
        Dim pa As String = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE")
        Return (If(([String].IsNullOrEmpty(pa) OrElse [String].Compare(pa, 0, "x86", 0, 3, True) = 0), 32, 64))
    End Function

End Class