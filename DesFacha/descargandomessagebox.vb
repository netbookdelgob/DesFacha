Imports System.Net

Public Class descargandomessagebox
    Private Sub descargandomessagebox_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim directory As String = My.Application.Info.DirectoryPath

        Dim strPath As String = directory + "\tweakingrepair.zip"
        Dim client As WebClient = New WebClient
        Dim url As String = "http://www.tweaking.com/files/setups/tweaking.com_windows_repair_aio.zip"
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

        Label2.Text = progreso + "%"
    End Sub
    Private Sub client_DownloadCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs)
        If e.Error IsNot Nothing Then
            MsgBox("Error descargando, por favor verifique su conexion a internet y vuelva a intentarlo.")
        End If
        Dim directory As String = My.Application.Info.DirectoryPath

        Dim strPath As String = directory + "\tweakingrepair.zip"
        Dim shObj As Object = Activator.CreateInstance(Type.GetTypeFromProgID("Shell.Application"))
        'Create directory in which you will unzip your items.
        IO.Directory.CreateDirectory(directory + "\extraido2")

        'Declare the folder where the items will be extracted.
        Dim output As Object = shObj.NameSpace((directory + "\extraido2"))

        'Declare the input zip file.
        Dim input As Object = shObj.NameSpace((strPath))

        'Extract the items from the zip file.
        output.CopyHere((input.Items), 4)
        descargado = True
        Me.Close()
    End Sub

End Class