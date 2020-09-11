Imports System.ComponentModel
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text

Public Class chkdskform
    <DllImport("kernel32.dll")>
    Private Shared Function GetVolumeInformation(ByVal PathName As String, ByVal VolumeNameBuffer As StringBuilder, ByVal VolumeNameSize As Int32, ByRef VolumeSerialNumber As Int32, ByRef MaximumComponentLength As Int32, ByRef FileSystemFlags As Int32, ByVal FileSystemNameBuffer As StringBuilder, ByVal FileSystemNameSize As Int32) As Long
    End Function
    Dim proc As New Process
    Dim hdserial As String
    Dim rs As New Resizer
    Friend Function GetVolumeSerial(ByVal strDriveLetter As String) As String

        Dim serNum As System.Int32 = 0
        Dim maxCompLen As System.Int32 = 0
        Dim VolLabel As StringBuilder = New StringBuilder(256)
        Dim VolFlags As Int32 = New Int32
        Dim FSName As StringBuilder = New StringBuilder(256)
        strDriveLetter += ":\"
        Dim Ret As Long = GetVolumeInformation(strDriveLetter, VolLabel, CType(VolLabel.Capacity, Int32), serNum, maxCompLen, VolFlags, FSName, CType(FSName.Capacity, Int32))
        Return Convert.ToString(serNum)
    End Function
    'Dim WithEvents proc As New Process
    Dim codigodeexito As Integer
    Private Sub chkdskform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        hdserial = GetVolumeSerial("c")
        rs.FindAllControls(Me)
        Go()
    End Sub
    Dim WithEvents p As New Process

    Sub Go()
        p.EnableRaisingEvents = True
        p.StartInfo.FileName = "chkdsk.exe"
        p.StartInfo.Arguments = "C:"
        p.StartInfo.UseShellExecute = False
        p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        p.StartInfo.CreateNoWindow = True
        p.StartInfo.RedirectStandardOutput = True
        AddHandler p.OutputDataReceived, AddressOf datata
        p.Start()
        p.BeginOutputReadLine()
    End Sub
    Public Sub WriteToLog(ByVal title As String, ByVal msg As String)
        Dim directory As String = My.Application.Info.DirectoryPath
        'Check and make directory
        If Not System.IO.Directory.Exists(directory + "\logs\") Then
            System.IO.Directory.CreateDirectory(directory + "\logs\")
        End If

        'Check and make file
        Dim fs As FileStream = New FileStream(directory + "\logs\" & hdserial & ".log", FileMode.OpenOrCreate, FileAccess.ReadWrite)
        Dim s As StreamWriter = New StreamWriter(fs)
        s.Close()
        fs.Close()

        'Logging
        Dim fs1 As FileStream = New FileStream(directory + "\logs\" & hdserial & ".log", FileMode.Append, FileAccess.Write)
        Dim s1 As StreamWriter = New StreamWriter(fs1)
        s1.Write("Titulo: " & title & vbCrLf)
        s1.Write("Mensaje: " & msg & vbCrLf)
        s1.Write("================================================" & vbCrLf)
        s1.Close()
        fs1.Close()
    End Sub

    Sub datata(ByVal sender As Object, ByVal e As DataReceivedEventArgs)
        UpdateTextBox(e.Data)
    End Sub

    Private Delegate Sub UpdateTextBoxDelegate(ByVal Text As String)
    Private Sub UpdateTextBox(ByVal Tex As String)
        If Me.InvokeRequired Then
            Dim del As New UpdateTextBoxDelegate(AddressOf UpdateTextBox)
            Dim args As Object() = {Tex}
            Me.Invoke(del, args)
        Else
            RichTextBox1.AppendText(Tex + Environment.NewLine)
            '     RichTextBox1.Text &= Tex & Environment.NewLine
            '   Dim end2 As Long

            '   end2 = Len(RichTextBox1.Text)
            '   RichTextBox1.SelectionStart = end2
        End If
    End Sub

    Private Sub chkdskform_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        WriteToLog(Date.Now, "Resultado del checkeo" & vbNewLine & RichTextBox1.Text)
        Dim pProcess() As Process = System.Diagnostics.Process.GetProcessesByName("chkdsk")

        For Each p As Process In pProcess
            p.Kill()
        Next
        RichTextBox1.ResetText()
    End Sub


    Private Sub ProcessExited(ByVal sender As Object, ByVal e As System.EventArgs) Handles p.Exited
        Dim Lookfor As String = "el sistema de archivos sin encontrar problemas"
        Dim eningles As String = "found no problem"
        Dim findstring As String = RichTextBox1.Text
        If findstring.Contains(Lookfor) Or findstring.Contains(eningles) Or p.ExitCode = 0 Then
            MsgBox("No se han encontrado errores. Su disco C: esta sano.")
            Me.Close()
        Else
            MsgBox("Se programara una comprobacion de disco para cuando reinicie el sistema. Se han encontrado errores.", MsgBoxStyle.Exclamation, "Errores en disco")

            Dim textosucio As String = "fsutil dirty set C:"  ' seteamos el dirty bit en el disco C
            Dim directory As String = My.Application.Info.DirectoryPath
            Dim bat1path As String
            bat1path = directory + "\scriptdirty.bat"
            If Not File.Exists(bat1path) Then
                Dim strBatLine1 As String = textosucio
                My.Computer.FileSystem.WriteAllText(bat1path, strBatLine1, False, System.Text.Encoding.Default)
                SetAttr(bat1path, FileAttribute.Normal)
            End If
            Dim ponersucioeldisco As New Process
            ponersucioeldisco.StartInfo.FileName = bat1path
            ponersucioeldisco.Start()
            ponersucioeldisco.WaitForExit()
            On Error Resume Next
            My.Computer.FileSystem.DeleteFile(bat1path)
            bat1path = Nothing
            directory = Nothing
            textosucio = Nothing
            Me.Close()
        End If
    End Sub

    Private Sub chkdskform_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        rs.ResizeAllControls(Me)
    End Sub
End Class