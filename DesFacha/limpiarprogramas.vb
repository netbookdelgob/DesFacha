Imports System.ComponentModel


Public Class limpiarprogramas
    Private Declare Function URLDownloadToFile Lib "urlmon" _
    Alias "URLDownloadToFileA" (ByVal pCaller As Integer,
    ByVal szURL As String, ByVal szFileName As String,
    ByVal dwReserved As Integer, ByVal lpfnCB As Integer) As Integer
    Private Sub limpiarprogramas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label3.Text = "Por GUID"
        BackgroundWorker1.RunWorkerAsync()
    End Sub
    Dim directory As String = My.Application.Info.DirectoryPath
    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Dim Ret As Integer
        Dim porguid As New Process
        Dim strURLguid As String
        strURLguid = "http://desfacha.ml/archivos/guid.txtx"
        Dim strPathguid As String = directory + "\guid.txt"
        Ret = URLDownloadToFile(0, strURLguid, strPathguid, 0, 0)

        Dim strURL As String
        strURL = "http://desfacha.ml/archivos/porguid.batx"
        Dim strPath As String = directory + "\porguid.bat"
        Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
        If Ret = 0 Then
            porguid.StartInfo.FileName = strPath
            porguid.StartInfo.CreateNoWindow = True
            porguid.Start()
            Threading.Thread.Sleep(500)
            Me.BringToFront()
            porguid.WaitForExit()
            ProgressBar1.Value = 35
        End If
        Dim toolbars As New Process
        strURL = "http://desfacha.ml/archivos/toolbars.batx"
        strPath = directory + "\toolbars.bat"
        Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
        If Ret = 0 Then
            toolbars.StartInfo.FileName = strPath
            Label3.Text = "Por GUID de toolbars"
            toolbars.StartInfo.CreateNoWindow = True
            toolbars.Start()
            Threading.Thread.Sleep(500)
            Me.BringToFront()
            toolbars.WaitForExit()
            ProgressBar1.Value += 35
        End If
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Dim borrar1 As String = directory + "\porguid.bat"
        Dim borrar2 As String = directory + "\toolbars.bat"
        On Error Resume Next
        My.Computer.FileSystem.DeleteFile(borrar1)
        My.Computer.FileSystem.DeleteFile(borrar2)
        Me.Close()

    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged

    End Sub
End Class