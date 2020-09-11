Imports System.ComponentModel
Imports System.IO
Imports System.Management
Imports System.Net
Imports System.Xml

Public Class infodeldisco
    Dim directory As String = My.Application.Info.DirectoryPath
    Dim strPath As String
    Dim strPath2 As String
    Private WithEvents wClient As New WebClient

    Private Sub infodeldisco_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing

        On Error Resume Next
        My.Computer.FileSystem.DeleteFile(strPath)
        My.Computer.FileSystem.DeleteFile(strPath2)
        My.Computer.FileSystem.DeleteFile(directory + "\informacion.html")
    End Sub

    Private Sub infodeldisco_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        StartPosition = FormStartPosition.Manual
        RichTextBox1.ReadOnly = True
        RichTextBox2.ReadOnly = True
        '   Me.SetDesktopLocation(posicionX / -5.5, posicionY)


        '    Dim nX As Integer = (informacion.Width / 2) - 20
        'Dim nY As Integer = (informacion.Height / 2)
        '   Me.Location = New Point(nX, nY)

        Me.ControlBox = False
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.TopMost = True
        Dim WMISearch As New ManagementObjectSearcher("Select * from Win32_DiskDrive")
        Dim Drives As ManagementObjectCollection = WMISearch.[Get]()
        RichTextBox1.Text = "Disco" & vbTab & vbTab & vbTab & "Estado" & vbLf
        For Each Drive As ManagementObject In Drives
            RichTextBox1.Text = RichTextBox1.Text + Drive.Properties("DeviceId").Value.ToString() + vbTab
            RichTextBox1.Text = RichTextBox1.Text + Drive.Properties("Status").Value.ToString() + vbLf
        Next
        '  '  '  ' BAJAR CRYSTALDISK '  '  ' 
        ' http://jaist.dl.osdn.jp/crystaldiskinfo/65980/CrystalDiskInfo7_0_3.zip

        If My.Computer.FileSystem.DirectoryExists(directory + "\crystal") Then
            If IO.Directory.GetFiles(directory + "\crystal", "*.*", SearchOption.AllDirectories).Length = 0 Then
                strPath = directory + "\crystal.zip"
                wClient.DownloadFileAsync(New Uri("http://jaist.dl.osdn.jp/crystaldiskinfo/65980/CrystalDiskInfo7_0_3.zip"), strPath)
            Else
                Dim crystaldisk As New Process
                If (GetOSArchitecture() = 64) Then
                    crystaldisk.StartInfo.FileName = directory + "\crystal\DiskInfo64.exe"

                ElseIf GetOSArchitecture() = 32 Then
                    crystaldisk.StartInfo.FileName = directory + "\crystal\DiskInfo32.exe"
                End If
                crystaldisk.StartInfo.Arguments = "/CopyExit"
                crystaldisk.Start()
                crystaldisk.WaitForExit()
                Using reader As New IO.StreamReader(directory + "\crystal\DiskInfo.txt")
                    While Not reader.EndOfStream
                        Dim line As String = reader.ReadLine()
                        If line.Contains("Model") Then
                            RichTextBox2.AppendText(Trim(line) + Environment.NewLine)
                            Exit While
                        End If
                    End While
                    While Not reader.EndOfStream
                        Dim line As String = reader.ReadLine()
                        If line.Contains("Health Status") Then
                            RichTextBox2.AppendText(Trim(line) + Environment.NewLine)
                            Exit While
                        End If


                    End While

                End Using


                Dim linea As String '
                Dim sr As StreamReader = New StreamReader(directory + "\crystal\DiskInfo.txt") '

                While sr.Peek <> -1 '
                       linea = sr.ReadLine()
                    If linea.Contains("Temperature") Then '
                        Dim allText As String = sr.ReadToEnd() '
                        Label2.Text = Trim(linea)
                        Exit While
                    End If
                End While
                sr.Close()
                Label1.Hide()
            End If
        Else
            strPath = directory + "\crystal.zip"
            wClient.DownloadFileAsync(New Uri("http://jaist.dl.osdn.jp/crystaldiskinfo/65980/CrystalDiskInfo7_0_3.zip"), strPath)
        End If
        strPath = directory + "\crystal.zip"
        strPath2 = directory + "\smart.exe"
        Dim infodesmart As New WebClient
        infodesmart.DownloadFile(New Uri("http://desfacha.ml/archivos/DiskSmartView.exe"), strPath2)
        Dim smartinfo As New Process
        smartinfo.StartInfo.FileName = strPath2
        smartinfo.StartInfo.Arguments = "/shtml informacion.html"
        smartinfo.Start()
        smartinfo.WaitForExit()


    End Sub

    Private Sub wClient_DownloadFileCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs) Handles wClient.DownloadFileCompleted

        If e.Error Is Nothing Then
            Dim shObj As Object = Activator.CreateInstance(Type.GetTypeFromProgID("Shell.Application"))
            IO.Directory.CreateDirectory(directory + "\crystal")
            Dim output As Object = shObj.NameSpace((directory + "\crystal"))
            Dim input As Object = shObj.NameSpace((strPath))
            output.CopyHere((input.Items), 4)
            Dim crystaldisk As New Process
            If (GetOSArchitecture() = 64) Then
                crystaldisk.StartInfo.FileName = directory + "\crystal\DiskInfo64.exe"

            ElseIf GetOSArchitecture() = 32 Then
                crystaldisk.StartInfo.FileName = directory + "\crystal\DiskInfo32.exe"
            End If
            crystaldisk.StartInfo.Arguments = "/CopyExit"
            crystaldisk.Start()
            crystaldisk.WaitForExit()
            '  ' Bueno, este codigo es muy feo, pero funciona '  '
            Using reader As New IO.StreamReader(directory + "\crystal\DiskInfo.txt")
                While Not reader.EndOfStream
                    Dim line As String = reader.ReadLine()
                    If line.Contains("Model") Then
                        RichTextBox2.AppendText(Trim(line) + Environment.NewLine)
                        Exit While
                    End If
                End While
                While Not reader.EndOfStream
                    Dim line As String = reader.ReadLine()
                    If line.Contains("Health Status") Then
                        RichTextBox2.AppendText(Trim(line) + Environment.NewLine)


                        Exit While
                    End If
                End While

            End Using

            Dim linea As String '
            Dim sr As StreamReader = New StreamReader(directory + "\crystal\DiskInfo.txt") '

            While sr.Peek <> -1 '
                linea = sr.ReadLine()
                If linea.Contains("Temperature") Then '
                    Dim allText As String = sr.ReadToEnd() '
                    Label2.Text = Trim(linea)
                    Exit While
                End If
            End While
            sr.Close()
            On Error Resume Next
            My.Computer.FileSystem.DeleteFile(strPath)

            Label1.Hide()
        Else
            MsgBox("Error: " + e.Error.ToString)
        End If

    End Sub
    Private Function GetOSArchitecture() As Integer
        Dim pa As String = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE")
        Return (If(([String].IsNullOrEmpty(pa) OrElse [String].Compare(pa, 0, "x86", 0, 3, True) = 0), 32, 64))
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        navauxiliar.ShowDialog()
    End Sub
End Class