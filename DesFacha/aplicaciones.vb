Imports System.ComponentModel
Imports System.IO
Imports System.Net
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading
Imports Microsoft.Win32

Public Class aplicaciones
    Private Declare Function FindWindow Lib "user32" Alias "FindWindowA" _
    (ByVal lpClassName As String, ByVal lpWindowName As String) As Integer
    Private Declare Function URLDownloadToFile Lib "urlmon" _
    Alias "URLDownloadToFileA" (ByVal pCaller As Integer,
    ByVal szURL As String, ByVal szFileName As String,
    ByVal dwReserved As Integer, ByVal lpfnCB As Integer) As Integer
    Public Declare Function SetForegroundWindow Lib "user32.dll" (ByVal hwnd As Integer) As Integer
    Private Declare Function SetFocus Lib "user32.dll" (ByVal hWnd As IntPtr) As Integer
    Public Function IsProcessRunning(name As String) As Boolean
        'here we're going to get a list of all running processes on
        'the computer
        For Each clsProcess As Process In Process.GetProcesses()
            If clsProcess.ProcessName.StartsWith(name) Then
                'process found so it's running so return true
                Return True
            End If
        Next
        'process not found, return false
        Return False
    End Function
    <DllImport("kernel32.dll")>
    Private Shared Function GetVolumeInformation(ByVal PathName As String, ByVal VolumeNameBuffer As StringBuilder, ByVal VolumeNameSize As Int32, ByRef VolumeSerialNumber As Int32, ByRef MaximumComponentLength As Int32, ByRef FileSystemFlags As Int32, ByVal FileSystemNameBuffer As StringBuilder, ByVal FileSystemNameSize As Int32) As Long
    End Function
    Dim hdserial As String
    Dim rs As New Resizer
    Private WithEvents descargadwcleaner As New WebClient
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        '   Dim Ret As Integer
        '  Dim strURL As String
        Dim strPath As String
        Dim directory As String = My.Application.Info.DirectoryPath
        '   strURL = "http://desfacha.ml/archivos/adwcleaner.exe"
        strPath = directory + "\adwcleaner.exe"

        descargadwcleaner.DownloadFileAsync(New Uri("http://desfacha.ml/archivos/adwcleaner.exe"), strPath)
        Progresoadw.Show()
        '  Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
        '   If Ret = 0 Then
        '  Dim adwcleaner As New Process
        '  adwcleaner.StartInfo.FileName = strPath
        '  adwcleaner.Start()
        ' If My.Settings.log = True Then
        ' WriteToLog(Date.Now, "Adwcleaner ejecutado. (Menu de aplicaciones no automatizadas)")
        ' End If
        ' adwcleaner.WaitForExit()
        ' If My.Settings.eliminarluego = True Then
        ' On Error Resume Next
        ' My.Computer.FileSystem.DeleteFile(strPath)
        ' End If

        '    Else
        '     MsgBox("No se ha podido descargar adwcleaner", MsgBoxStyle.Critical)
        '   If My.Settings.log = True Then
        '   WriteToLog(Date.Now, "No se ha podido descargar Adwcleaner. (Menu de aplicaciones no automatizadas)")
        '   End If
        '   End If
    End Sub
    Private Sub descargadwcleaner_ProgressChanged(ByVal sender As Object, ByVal e As DownloadProgressChangedEventArgs) Handles descargadwcleaner.DownloadProgressChanged
        Dim bytesIn As Double = Double.Parse(e.BytesReceived.ToString())
        Dim totalBytes As Double = Double.Parse(e.TotalBytesToReceive.ToString())
        Dim percentage As Double = bytesIn / totalBytes * 100
        Dim progreso As String = Int32.Parse(Math.Truncate(percentage).ToString())

        Progresoadw.ProgressBar1.Value = Int32.Parse(Math.Truncate(percentage).ToString())
        Progresoadw.Label2.Text = progreso + " %"
    End Sub
    Private Sub descargadwcleaner_DownloadFileCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs) Handles descargadwcleaner.DownloadFileCompleted
        Progresoadw.Close()

        If e.Error Is Nothing Then
            Dim directory As String = My.Application.Info.DirectoryPath
            Dim strPath As String
            strPath = directory + "\adwcleaner.exe"
            Dim adwcleaner As New Process
            adwcleaner.StartInfo.FileName = strPath
            adwcleaner.Start()
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "Adwcleaner ejecutado. (Menu de aplicaciones no automatizadas)")
            End If
            adwcleaner.WaitForExit()
            If My.Settings.eliminarluego = True Then
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(strPath)
            End If
        Else
            MsgBox("No se ha podido descargar adwcleaner", MsgBoxStyle.Critical)
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "No se ha podido descargar Adwcleaner. (Menu de aplicaciones no automatizadas)")
            End If
        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim Ret As Integer
        Dim strURL As String
        Dim strPath As String
        Dim directory As String = My.Application.Info.DirectoryPath
        ' strURL = "http://ftp.ecps.us/Clean/RKill/rkill.com"
        strURL = "http://desfacha.ml/archivos/rkill.comx"

        strPath = directory + "\rkill.com"

        Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
        If Ret = 0 Then

            Dim rkill As New Process
            rkill.StartInfo.FileName = strPath
            rkill.Start()
            rkill.WaitForExit()
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "Rkill ejecutado. (Menu de aplicaciones no automatizadas)")
            End If
            If My.Settings.eliminarluego = True Then
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(strPath)
            End If
            Ret = Nothing
            strURL = Nothing
            strPath = Nothing
            Me.BringToFront()
        Else
            MsgBox("Rkill no se ha podido descargar.", MsgBoxStyle.Critical)
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "No se ha podido descargar Rkill (Menu de aplicaciones no automatizadas)")
            End If
        End If
    End Sub

    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim Ret As Integer
        Dim strURL As String
        Dim strPath As String
        Dim directory As String = My.Application.Info.DirectoryPath
        strURL = "https://download.bleachbit.org/BleachBit-1.12-portable.zip"

        strPath = directory + "\bleachbit112.zip"

        Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)

        If Ret = 0 Then

            Dim shObj As Object = Activator.CreateInstance(Type.GetTypeFromProgID("Shell.Application"))

            IO.Directory.CreateDirectory(directory + "\extraido")


            Dim output As Object = shObj.NameSpace((directory + "\extraido"))


            Dim input As Object = shObj.NameSpace((strPath))


            output.CopyHere((input.Items), 4)
            Dim ini As String = "https://github.com/bmrf/tron/blob/master/resources/stage_1_tempclean/bleachbit/BleachBit.ini"
            Dim directoriodelini As String = directory + "\extraido\BleachBit-Portable\BleachBit.ini"
            On Error Resume Next
            My.Computer.FileSystem.DeleteFile(directoriodelini)
            Ret = URLDownloadToFile(0, ini, directoriodelini, 0, 0)
            Dim bleachbit As New Process
            bleachbit.StartInfo.FileName = directory + "\extraido\BleachBit-Portable\bleachbit.exe"
            bleachbit.Start()
            bleachbit.WaitForExit()
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "BleachBit ejecutado. (Menu de aplicaciones no automatizadas)")
            End If
            If My.Settings.eliminarluego = True Then
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(strPath)
                My.Computer.FileSystem.DeleteDirectory(directory + "\extraido", FileIO.DeleteDirectoryOption.DeleteAllContents)
            End If

            Ret = Nothing
            strURL = Nothing
            strPath = Nothing
            output = Nothing
            input = Nothing
            bleachbit = Nothing
        Else
            MsgBox("No se ha podido descargar bleachbit", MsgBoxStyle.Critical)
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "No se ha podido descargar bleachbit. (Menu de aplicaciones no automatizadas)")
            End If

        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        Dim strPath As String
        Dim directory As String = My.Application.Info.DirectoryPath
        Dim limpiar As New Process
        ccleaner.ShowDialog()

        strPath = directory + "\ccsetup525.exe"



        If paso3 = True Then

            Dim ccleaner As New Process
            ccleaner.StartInfo.FileName = (strPath)
            ccleaner.StartInfo.Arguments = "/S /D=C:\CCleaner"
            ccleaner.Start()
            ccleaner.WaitForExit()

            limpiar.StartInfo.FileName = ("C:\CCleaner\CCleaner.exe")
            limpiar.Start()
            limpiar.WaitForExit()
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "ccleaner ejecutado (Menu aplicaciones no automatizadas)")
            End If
            Dim eliminarccleaner As New Process
            eliminarccleaner.StartInfo.FileName = "C:\CCleaner\uninst.exe"
            eliminarccleaner.StartInfo.Arguments = "/S"
            eliminarccleaner.Start()
            eliminarccleaner.WaitForExit()
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "ccleaner desinstalado. (Menu aplicaciones no automatizadas)")
            End If
            ccleaner = Nothing
            eliminarccleaner = Nothing
            limpiar = Nothing
            If My.Settings.eliminarluego = True Then
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(strPath)
            End If

            strPath = Nothing

        End If
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


    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        chkdskform.ShowDialog()


    End Sub
    Private Sub aplicaciones_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        rs.FindAllControls(Me)
        hdserial = GetVolumeSerial("c")
        Dim versionos = Environment.OSVersion.Version.Major
        Select Case versionos

            Case 4
                If Environment.OSVersion.Version.Minor = 10 Or 90 Then
                    Button1.Enabled = False
                    Button2.Enabled = False
                    Button3.Enabled = False
                    Button4.Enabled = False
                    Button5.Enabled = False
                    Button6.Enabled = False
                    Button7.Enabled = False
                    Button8.Enabled = False
                    Button20.Enabled = False

                End If
            Case 5
                If Environment.OSVersion.Version.Minor = 0 Then

                    Button9.Enabled = False
                End If
        End Select

    End Sub
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

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        ' DESFASADO

        '  Dim Ret As Integer
        '   Dim strURL As String
        Dim strPath As String
        Dim directory As String = My.Application.Info.DirectoryPath
        '    strURL = "http://devbuilds.kaspersky-labs.com/devbuilds/KVRT/latest/full/KVRT.exe"
        strPath = directory + "\KVRT.exe"
        '   Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
        '   If Ret = 0 Then
        kvrt.ShowDialog()
        If paso2 = True Then
            Dim EMISOFT As New Process
            EMISOFT.StartInfo.FileName = strPath
            EMISOFT.Start()
            EMISOFT.WaitForExit()
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "KVRT OK. (Menu de aplicaciones no automatizadas)")
            End If
            If My.Settings.eliminarluego = True Then
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(strPath)
            End If

            strPath = Nothing
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        windowsrepair.ShowDialog()
        If paso1 = True Then
            Dim directory As String = My.Application.Info.DirectoryPath

            Dim strPath As String = directory + "\tweakingrepair.zip"
            Dim tweaking As New Process
            tweaking.StartInfo.FileName = directory + "\extraido\Tweaking.com - Windows Repair\Repair_Windows.exe"
            tweaking.Start()
            tweaking.WaitForExit()
            Dim pProcess() As Process = System.Diagnostics.Process.GetProcessesByName("WR_Tray_Icon")

            For Each p As Process In pProcess
                p.Kill()
            Next
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "Tweaking windows repair ejecutado. (Menu de aplicaciones no automatizadas)")
            End If
            If My.Settings.eliminarluego = True Then
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(strPath)
                My.Computer.FileSystem.DeleteDirectory(directory + "\extraido", FileIO.DeleteDirectoryOption.DeleteAllContents)
            End If
        End If
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Dim directory As String = My.Application.Info.DirectoryPath
        stinger.ShowDialog()

        If paso4 = True Then
            Dim strPath As String = directory + "\stinger32.exe"

            Dim sophos As New Process
            sophos.StartInfo.FileName = strPath
            sophos.Start()
            sophos.WaitForExit()
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "STINGER ejecutado (Menu de aplicaciones no automatizadas).")
            End If
            If My.Settings.eliminarluego = True Then
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(strPath)
            End If
            sophos = Nothing
        End If
        directory = Nothing
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Dim directory As String = My.Application.Info.DirectoryPath
        hitmanpro.ShowDialog()
        If paso5 = True Then
            Dim strPath As String = directory + "\hitmanpro.exe"

            Dim hitman As New Process
            hitman.StartInfo.FileName = strPath
            hitman.Start()
            hitman.WaitForExit()
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "Hitman Pro ejecutado (Menu de aplicaciones no automatizadas).")
            End If
            If My.Settings.eliminarluego = True Then
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(strPath)
            End If
            hitman = Nothing
        End If
        directory = Nothing
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Dim Ret As Integer
        Dim strURL As String
        Dim strPath As String
        Dim directory As String = My.Application.Info.DirectoryPath
        '  strURL = "https://github.com/Nummer/Destroy-Windows-10-Spying/releases/download/999999/DWS_Lite.exe"
        strURL = "https://github.com/Nummer/Destroy-Windows-10-Spying/releases/download/1.6.722/DWS_Lite.exe"
        strPath = directory + "\DWS.exe"

        Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)

        If Ret = 0 Then
            Dim dws As New Process
            dws.StartInfo.FileName = strPath
            dws.Start()
            dws.WaitForExit()
            On Error Resume Next
            File.Move(directory + "\DWS.log", directory + "\logs\DWS.log")
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "Destroy Windows Spying. OK. (Menu de aplicaciones no automatizadas)")
            End If
            If My.Settings.eliminarluego = True Then
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(strPath)
            End If
        Else
            MsgBox("Error descargando Destroy Windows Spying", MsgBoxStyle.Critical)
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "No se ha podido descargar destroy windows spying (Menu de aplicaciones no automatizadas)")
            End If
        End If

    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Dim directory As String = My.Application.Info.DirectoryPath

        spybot.ShowDialog()
        If paso6 = True Then
            Dim strPath As String = directory + "\spybot.exe"
            MsgBox("Por favor, no toque nada hasta que aparezca la pantalla principal de spybot." & vbNewLine & "La cual se mostrara a modo de ejemplo luego de cerrar este mensaje.")
            '  MOSTRAR UN FORMULARIO CON UNA IMAGEN DEL MENU PRINCIPAL DE SPYBOT POR 3 SEGUNDOS Y LUEGO CERRARLO Y CONTINUAR CON LO NORMAL
            ' spybotmuestra.ShowDialog()

            Dim spybot As New Process
            spybot.StartInfo.FileName = strPath
            spybot.Start()

            Dim theHandle As IntPtr
            theHandle = FindWindow(Nothing, "Select Setup Language")
            If theHandle <> IntPtr.Zero Then
                SetForegroundWindow(theHandle)
                SetFocus(theHandle)
            End If
            Thread.Sleep(2000)
            SendKeys.Send("{Enter}")
            Thread.Sleep(3000)
            SendKeys.Send("{Enter}")
            Thread.Sleep(2000)
            SendKeys.Send("{Enter}")
            Thread.Sleep(2000)
            SendKeys.Send("{Enter}")
            Thread.Sleep(1000)
            SendKeys.Send("{TAB}")
            SendKeys.Send("{UP}")
            SendKeys.Send("{Enter}")
            SendKeys.Send("{Enter}")
            Thread.Sleep(180000)
            SendKeys.Send("{Enter}")

        End If
    End Sub

    Public Sub Button20_Click(sender As Object, e As EventArgs) Handles Button20.Click
        ' AUTOMATIZAR LA INSTALACION DE DIRECTX, OPENAL, Y VC REDISTRIBUTABLES DEL 2008 EN ADELANTE. TANTO X86 COMO X64
        ' Instalar Directx desde web runtimes
        ' Bajar dxsetup desde mi propio host.
        dxespere.Show()
        Dim Ret As Integer
        Dim strURL As String
        Dim strPath As String
        Dim directory As String = My.Application.Info.DirectoryPath
        strURL = "http://desfacha.ml/archivos/dxwebsetup.exe"

        strPath = directory + "\dxwebsetup.exe"

        Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
        If Ret = 0 Then
            Dim directx As New Process
            directx.StartInfo.FileName = strPath
            directx.StartInfo.Arguments = "/Q"
            directx.Start()
            directx.WaitForExit()
            dxespere.Close()
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "DirectX instalado (Menu de arreglos).")
            End If
            If My.Settings.eliminarluego = True Then
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(strPath)
            End If
        Else
            MsgBox("Error descargando DirectX", MsgBoxStyle.Critical)
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "No se ha podido descargar DirectX (Menu de arreglos)")
            End If
        End If
        Ret = Nothing
        strURL = Nothing

        ' OpenAL

        strURL = "http://desfacha.ml/archivos/oalinst.exe"

        strPath = directory + "\openal.exe"
        Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
        If Ret = 0 Then
            Dim openal As New Process
            openal.StartInfo.FileName = strPath
            openal.StartInfo.Arguments = "/S"
            openal.Start()
            openal.WaitForExit()
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "OpenAL instalado (Menu de arreglos).")
            End If
            If My.Settings.eliminarluego = True Then
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(strPath)
            End If
        Else
            MsgBox("Error descargando OpenAL", MsgBoxStyle.Critical)
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "No se ha podido descargar OpenAL (Menu de arreglos)")
            End If
        End If
        Ret = Nothing
        strURL = Nothing
        ' VC REDISTS
        ' 2008
        espereporfavor.Show()
        strURL = "http://desfacha.ml/archivos/vcredist2008_x86.exe"

        strPath = directory + "\vc2008x86.exe"
        Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
        If Ret = 0 Then
            Dim vc2008 As New Process
            vc2008.StartInfo.FileName = strPath
            vc2008.StartInfo.Arguments = "/q /norestart"
            vc2008.Start()
            vc2008.WaitForExit()
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "Visual C++ redistributables 2008 x86 instalado (Menu de arreglos).")
            End If
            If My.Settings.eliminarluego = True Then
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(strPath)
            End If
        Else
            MsgBox("Error descargando Visual C++ redistributables 2008 x86", MsgBoxStyle.Critical)
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "No se ha podido descargar Visual C++ redistributables 2008 x86 (Menu de arreglos)")
            End If
        End If
        Ret = Nothing
        strURL = Nothing
        If (GetOSArchitecture() = 64) Then
            strURL = "http://desfacha.ml/archivos/vcredist2008_x64.exe"
            strPath = directory + "\vc2008x64.exe"
            Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
            If Ret = 0 Then
                Dim vc2008x64 As New Process
                vc2008x64.StartInfo.FileName = strPath
                vc2008x64.StartInfo.Arguments = "/q /norestart"
                vc2008x64.Start()
                vc2008x64.WaitForExit()
                If My.Settings.log = True Then
                    WriteToLog(Date.Now, "Visual C++ redistributables 2008 x64 instalado (Menu de arreglos).")
                End If
                If My.Settings.eliminarluego = True Then
                    On Error Resume Next
                    My.Computer.FileSystem.DeleteFile(strPath)
                End If
            Else
                MsgBox("Error descargando Visual C++ redistributables 2008 x64", MsgBoxStyle.Critical)
                If My.Settings.log = True Then
                    WriteToLog(Date.Now, "No se ha podido descargar Visual C++ redistributables 2008 x64 (Menu de arreglos)")
                End If
            End If
        End If
        ' VC REDISTS
        ' 2010
        strURL = "http://desfacha.ml/archivos/vcredist2010_x86.exe"

        strPath = directory + "\vc2010x86.exe"
        Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
        If Ret = 0 Then
            Dim vc2010 As New Process
            vc2010.StartInfo.FileName = strPath
            vc2010.StartInfo.Arguments = "/q /norestart"
            vc2010.Start()
            vc2010.WaitForExit()
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "Visual C++ redistributables 2010 x86 instalado (Menu de arreglos).")
            End If
            If My.Settings.eliminarluego = True Then
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(strPath)
            End If
        Else
            MsgBox("Error descargando Visual C++ redistributables 2010 x86", MsgBoxStyle.Critical)
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "No se ha podido descargar Visual C++ redistributables 2010 x86 (Menu de arreglos)")
            End If
        End If
        Ret = Nothing
        strURL = Nothing
        If (GetOSArchitecture() = 64) Then
            strURL = "http://desfacha.ml/archivos/vcredist2010_x64.exe"
            strPath = directory + "\vc2010x64.exe"
            Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
            If Ret = 0 Then
                Dim vc2010x64 As New Process
                vc2010x64.StartInfo.FileName = strPath
                vc2010x64.StartInfo.Arguments = "/q /norestart"
                vc2010x64.Start()
                vc2010x64.WaitForExit()
                If My.Settings.log = True Then
                    WriteToLog(Date.Now, "Visual C++ redistributables 2010 x64 instalado (Menu de arreglos).")
                End If
                If My.Settings.eliminarluego = True Then
                    On Error Resume Next
                    My.Computer.FileSystem.DeleteFile(strPath)
                End If
            Else
                MsgBox("Error descargando Visual C++ redistributables 2010 x64", MsgBoxStyle.Critical)
                If My.Settings.log = True Then
                    WriteToLog(Date.Now, "No se ha podido descargar Visual C++ redistributables 2010 x64 (Menu de arreglos)")
                End If
            End If
        End If

        ' VC REDISTS
        ' 2013
        Ret = Nothing
        strURL = Nothing
        strURL = "http://desfacha.ml/archivos/vcredist2013_x86.exe"

        strPath = directory + "\vc2013x86.exe"
        Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
        If Ret = 0 Then
            Dim vc2013 As New Process
            vc2013.StartInfo.FileName = strPath
            vc2013.StartInfo.Arguments = "/q /norestart"
            vc2013.Start()
            vc2013.WaitForExit()
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "Visual C++ redistributables 2013 x86 instalado (Menu de arreglos).")
            End If
            If My.Settings.eliminarluego = True Then
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(strPath)
            End If
        Else
            MsgBox("Error descargando Visual C++ redistributables 2013 x86", MsgBoxStyle.Critical)
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "No se ha podido descargar Visual C++ redistributables 2013 x86 (Menu de arreglos)")
            End If
        End If
        Ret = Nothing
        strURL = Nothing
        If (GetOSArchitecture() = 64) Then
            strURL = "http://desfacha.ml/archivos/vcredist2013_x64.exe"
            strPath = directory + "\vc2013x64.exe"
            Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
            If Ret = 0 Then
                Dim vc2013x64 As New Process
                vc2013x64.StartInfo.FileName = strPath
                vc2013x64.StartInfo.Arguments = "/q /norestart"
                vc2013x64.Start()
                vc2013x64.WaitForExit()
                If My.Settings.log = True Then
                    WriteToLog(Date.Now, "Visual C++ redistributables 2013 x64 instalado (Menu de arreglos).")
                End If
                If My.Settings.eliminarluego = True Then
                    On Error Resume Next
                    My.Computer.FileSystem.DeleteFile(strPath)
                End If
            Else
                MsgBox("Error descargando Visual C++ redistributables 2013 x64", MsgBoxStyle.Critical)
                If My.Settings.log = True Then
                    WriteToLog(Date.Now, "No se ha podido descargar Visual C++ redistributables 2013 x64 (Menu de arreglos)")
                End If
            End If
        End If
        strURL = "http://desfacha.ml/archivos/vcredist2012_x86.exe"

        strPath = directory + "\vc2012x86.exe"
        Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
        If Ret = 0 Then
            Dim vc2013 As New Process
            vc2013.StartInfo.FileName = strPath
            vc2013.StartInfo.Arguments = "/q /norestart"
            vc2013.Start()
            vc2013.WaitForExit()
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "Visual C++ redistributables 2012 x86 instalado (Menu de arreglos).")
            End If
            If My.Settings.eliminarluego = True Then
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(strPath)
            End If
        Else
            MsgBox("Error descargando Visual C++ redistributables 2012 x86", MsgBoxStyle.Critical)
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "No se ha podido descargar Visual C++ redistributables 2012 x86 (Menu de arreglos)")
            End If
        End If
        Ret = Nothing
        strURL = Nothing
        If (GetOSArchitecture() = 64) Then
            strURL = "http://desfacha.ml/archivos/vcredist2012_x64.exe"
            strPath = directory + "\vc2012x64.exe"
            Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
            If Ret = 0 Then
                Dim vc2013x64 As New Process
                vc2013x64.StartInfo.FileName = strPath
                vc2013x64.StartInfo.Arguments = "/q /norestart"
                vc2013x64.Start()
                vc2013x64.WaitForExit()
                If My.Settings.log = True Then
                    WriteToLog(Date.Now, "Visual C++ redistributables 2012 x64 instalado (Menu de arreglos).")
                End If
                If My.Settings.eliminarluego = True Then
                    On Error Resume Next
                    My.Computer.FileSystem.DeleteFile(strPath)
                End If
            Else
                MsgBox("Error descargando Visual C++ redistributables 2012 x64", MsgBoxStyle.Critical)
                If My.Settings.log = True Then
                    WriteToLog(Date.Now, "No se ha podido descargar Visual C++ redistributables 2012 x64 (Menu de arreglos)")
                End If
            End If
        End If
        ' VC REDISTS
        ' 2015
        strURL = "http://desfacha.ml/archivos/vc_redist2015.x86.exe"

        strPath = directory + "\vc2015x86.exe"
        Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
        If Ret = 0 Then
            Dim vc2015 As New Process
            vc2015.StartInfo.FileName = strPath
            vc2015.StartInfo.Arguments = "/q /norestart"
            vc2015.Start()
            vc2015.WaitForExit()
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "Visual C++ redistributables 2015 x86 instalado (Menu de arreglos).")
            End If
            If My.Settings.eliminarluego = True Then
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(strPath)
            End If
        Else
            MsgBox("Error descargando Visual C++ redistributables 2015 x86", MsgBoxStyle.Critical)
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "No se ha podido descargar Visual C++ redistributables 2015 x86 (Menu de arreglos)")
            End If
        End If
        Ret = Nothing
        strURL = Nothing
        If (GetOSArchitecture() = 64) Then
            strURL = "http://desfacha.ml/archivos/vc_redist2015.x64.exe"
            strPath = directory + "\vc2015x64.exe"
            Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
            If Ret = 0 Then
                Dim vc2015x64 As New Process
                vc2015x64.StartInfo.FileName = strPath
                vc2015x64.StartInfo.Arguments = "/q /norestart"
                vc2015x64.Start()
                vc2015x64.WaitForExit()
                If My.Settings.log = True Then
                    WriteToLog(Date.Now, "Visual C++ redistributables 2015 x64 instalado (Menu de arreglos).")
                End If
                If My.Settings.eliminarluego = True Then
                    On Error Resume Next
                    My.Computer.FileSystem.DeleteFile(strPath)
                End If
            Else
                MsgBox("Error descargando Visual C++ redistributables 2015 x64", MsgBoxStyle.Critical)
                If My.Settings.log = True Then
                    WriteToLog(Date.Now, "No se ha podido descargar Visual C++ redistributables 2015 x64 (Menu de arreglos)")
                End If
            End If
        End If
        espereporfavor.Close()
        If My.Settings.eliminarluego = True Then
            On Error Resume Next
            My.Computer.FileSystem.DeleteFile(strPath)
        End If
        MsgBox("Por favor, cierre desfacha y proceda a ejecutar el juego.", MsgBoxStyle.Exclamation)
    End Sub
    Private Function GetOSArchitecture() As Integer
        Dim pa As String = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE")
        Return (If(([String].IsNullOrEmpty(pa) OrElse [String].Compare(pa, 0, "x86", 0, 3, True) = 0), 32, 64))
    End Function



    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        Dim flushdns As String = "ipconfig /flushdns"  ' Script

        Dim directory As String = My.Application.Info.DirectoryPath
        Dim bat1path As String
        bat1path = directory + "\scriptflush.bat"
        If Not File.Exists(bat1path) Then
            Dim strBatLine1 As String = flushdns
            My.Computer.FileSystem.WriteAllText(bat1path, strBatLine1, False, System.Text.Encoding.Default)
            SetAttr(bat1path, FileAttribute.Normal)
        End If
        Dim flushdnsproceso As New Process
        flushdnsproceso.StartInfo.FileName = bat1path
        flushdnsproceso.Start()
        flushdnsproceso.WaitForExit()
        If My.Settings.log = True Then
            WriteToLog(Date.Now, "Realizada la operacion: Flush dns, de ipconfig.")
        End If
        On Error Resume Next
        My.Computer.FileSystem.DeleteFile(bat1path)
        bat1path = Nothing
        directory = Nothing
        flushdnsproceso = Nothing
    End Sub

    Public Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        MessageBox.Show("Cuando aparezca la ventana Devid Driver installer, click en Search for new drivers, luego, clickee en install drivers cuando se le de la opcion, luego le dara a elegir si quiere reiniciar o no, eliga." + Environment.NewLine + "Tenga en cuenta que este proceso deberia ser supervisado.", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Dim Ret As Integer
        Dim strURL As String
        Dim strPath As String
        Dim directory As String = My.Application.Info.DirectoryPath
        strURL = "http://desfacha.ml/archivos/devid.zip"

        strPath = directory + "\devid.zip"

        Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
        If Ret = 0 Then
            Dim shObj As Object = Activator.CreateInstance(Type.GetTypeFromProgID("Shell.Application"))
            IO.Directory.CreateDirectory(directory + "\extraido")

            Dim output As Object = shObj.NameSpace((directory + "\extraido"))

            Dim input As Object = shObj.NameSpace((strPath))

            output.CopyHere((input.Items), 4)
            Dim devid As New Process
            devid.StartInfo.FileName = directory + "\extraido\DevidAgent3.exe"
            devid.StartInfo.WorkingDirectory = directory + "\extraido"
            devid.StartInfo.UseShellExecute = True
            devid.Start()
            devidimagen.ShowDialog()
            AppActivate(devid.Id)
            devid.WaitForExit()
            ' ' ' SE MUESTRA UNA IMAGEN DE LO QUE NO HAY QUE TICKEAR  ' ' '


            '     Select Case MsgBox("¿Instaló nuevos drivers?", MsgBoxStyle.YesNo, "Decida")
            '    Case MsgBoxResult.Yes
            '   Select Case Environment.OSVersion.Version.Major
            ' Case 6
            'If (GetOSArchitecture() = 64) Then
            'strURL = "http://desfacha.ml/archivos/devcon/NT6/64/devcon.exe"
            '
            'ElseIf  GetOSArchitecture() = 32 Then
            '  strURL = "http://desfacha.ml/archivos/devcon/NT6/32/devcon.exe"
            'End If
            '  strPath = directory + "\devcon.exe"
            '
            ' Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
            'If Ret = 0 Then

            ' End If
            'Case 5
            'If (GetOSArchitecture() = 64) Then
            'strURL = "http://desfacha.ml/archivos/devcon/NT5/64/devcon.exe"
            '
            '  ElseIf GetOSArchitecture() = 32 Then
            'strURL = "http://desfacha.ml/archivos/devcon/NT5/32/devcon.exe"
            'End If
            '  strPath = directory + "\devcon.exe"

            ' Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
            'If Ret = 0 Then
            '
            'End If
            '  End Select


            '   End Select
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "Drivers escaneados.")
            End If
            If My.Settings.eliminarluego = True Then
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(strPath)
                My.Computer.FileSystem.DeleteDirectory(directory + "\extraido", FileIO.DeleteDirectoryOption.DeleteAllContents)
            End If
        Else
            MsgBox("Error descargando devid", MsgBoxStyle.Critical)
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "No se ha podido descargar devid (Menu de arreglos)")
            End If
        End If
        Ret = Nothing
        strURL = Nothing
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click

        Dim Ret As Integer
        Dim strURL As String
        Dim strPath As String
        Dim directory As String = My.Application.Info.DirectoryPath
        Dim urldelaconfig As String
        Dim directoriodelaconfig As String
        urldelaconfig = "http://desfacha.ml/archivos/PatchMyPC.settings"
        directoriodelaconfig = directory + "\PatchMyPC.settings"
        Ret = URLDownloadToFile(0, urldelaconfig, directoriodelaconfig, 0, 0)
        strURL = "https://patchmypc.net/freeupdater/PatchMyPC.exe"

        strPath = directory + "\patchmypc.exe"

        Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
        If Ret = 0 Then
            Dim patch As New Process
            patch.StartInfo.FileName = strPath
            patch.StartInfo.Arguments = "/auto"
            patch.Start()
            patch.WaitForExit()
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "Programas actualizados (Menu de arreglos).")
            End If
            If My.Settings.eliminarluego = True Then
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(strPath)
                My.Computer.FileSystem.DeleteFile(directoriodelaconfig)
            End If
        Else
            MsgBox("Error descargando PatchMyPc", MsgBoxStyle.Critical)
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "No se ha podido descargar PatchMyPC (Menu de arreglos)")
            End If
        End If
        Ret = Nothing
        strURL = Nothing

    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        Dim directory As String = My.Application.Info.DirectoryPath

        Dim strPath As String = directory + "\tweakingrepair.zip"

        Dim btnSender As Button = DirectCast(sender, Button)
        Dim ptLowerLeft As New Point(0, btnSender.Height)

        If My.Computer.FileSystem.DirectoryExists(directory + "\extraido2") Then
            If IO.Directory.GetFiles(directory + "\extraido2", "*.*", SearchOption.AllDirectories).Length = 0 Then
                descargandomessagebox.ShowDialog()
                If descargado = True Then
                    On Error Resume Next
                    My.Computer.FileSystem.DeleteFile(strPath)

                    ptLowerLeft = btnSender.PointToScreen(ptLowerLeft)
                    ContextMenuStrip1.Show(ptLowerLeft)
                End If
            Else
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(strPath)
                ptLowerLeft = btnSender.PointToScreen(ptLowerLeft)
                ContextMenuStrip1.Show(ptLowerLeft)
            End If

        Else

            descargandomessagebox.ShowDialog()
            If descargado = True Then
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(strPath)

                ptLowerLeft = btnSender.PointToScreen(ptLowerLeft)
                ContextMenuStrip1.Show(ptLowerLeft)
            End If

        End If



    End Sub
    Private WithEvents wClientWINUPDATE As New WebClient
    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click
        Dim directory As String = My.Application.Info.DirectoryPath

        Dim strPath As String = directory + "\extraido2\Tweaking.com - Windows Repair\settings.ini"
        wClientWINUPDATE.DownloadFileAsync(New Uri("http://desfacha.ml/configuracion/winupdate.ini"), strPath)
        Thread.Sleep(1000) 'Naturalmente, no queremos abrir el tweaking sin el settings.ini y esto le da tiempo a descargarse
        Dim repararwindowsupdate As New Process
        repararwindowsupdate.StartInfo.FileName = directory + "\extraido2\Tweaking.com - Windows Repair\Repair_Windows.exe"
        repararwindowsupdate.StartInfo.Arguments = "/silent"
        repararwindowsupdate.Start()
        repararwindowsupdate.WaitForExit()
        Select Case MsgBox("Reparacion completa, debe reiniciar la pc para que las reparaciones tomen efecto." & vbNewLine & "¿Va a realizar otra reparacion?", vbYesNo)
            Case vbYes

            Case vbNo
                On Error Resume Next
                My.Computer.FileSystem.DeleteDirectory(directory + "\extraido2", FileIO.DeleteDirectoryOption.DeleteAllContents)
        End Select
        WriteToLog(Date.Now, "Arreglado Windows Update")
    End Sub
    Private WithEvents wClientWMI As New WebClient
    Private Sub ArreglarWMIToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ArreglarWMIToolStripMenuItem.Click
        Dim directory As String = My.Application.Info.DirectoryPath

        Dim strPath As String = directory + "\extraido2\Tweaking.com - Windows Repair\settings.ini"
        wClientWMI.DownloadFileAsync(New Uri("http://desfacha.ml/configuracion/wmi.ini"), strPath)
        Thread.Sleep(1000) 'Naturalmente, no queremos abrir el tweaking sin el settings.ini y esto le da tiempo a descargarse
        Dim repararwmi As New Process
        repararwmi.StartInfo.FileName = directory + "\extraido2\Tweaking.com - Windows Repair\Repair_Windows.exe"
        repararwmi.StartInfo.Arguments = "/silent"
        repararwmi.Start()
        repararwmi.WaitForExit()
        Select Case MsgBox("Reparacion completa, debe reiniciar la pc para que las reparaciones tomen efecto." & vbNewLine & "¿Va a realizar otra reparacion?", vbYesNo)
            Case vbYes

            Case vbNo
                On Error Resume Next
                My.Computer.FileSystem.DeleteDirectory(directory + "\extraido2", FileIO.DeleteDirectoryOption.DeleteAllContents)
        End Select
        WriteToLog(Date.Now, "Arreglado WMI")
    End Sub
    Private WithEvents wClienticonos As New WebClient
    Private Sub ArreglarIconosDelMenuInicioToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ArreglarIconosDelMenuInicioToolStripMenuItem.Click
        Dim directory As String = My.Application.Info.DirectoryPath

        Dim strPath As String = directory + "\extraido2\Tweaking.com - Windows Repair\settings.ini"
        wClienticonos.DownloadFileAsync(New Uri("http://desfacha.ml/configuracion/iconos.ini"), strPath)
        Thread.Sleep(1000) 'Naturalmente, no queremos abrir el tweaking sin el settings.ini y esto le da tiempo a descargarse
        Dim reparariconos As New Process
        reparariconos.StartInfo.FileName = directory + "\extraido2\Tweaking.com - Windows Repair\Repair_Windows.exe"
        reparariconos.StartInfo.Arguments = "/silent"
        reparariconos.Start()
        reparariconos.WaitForExit()
        Select Case MsgBox("Reparacion completa, debe reiniciar la pc para que las reparaciones tomen efecto." & vbNewLine & "¿Va a realizar otra reparacion?", vbYesNo)
            Case vbYes

            Case vbNo
                On Error Resume Next
                My.Computer.FileSystem.DeleteDirectory(directory + "\extraido2", FileIO.DeleteDirectoryOption.DeleteAllContents)
        End Select
        WriteToLog(Date.Now, "Arreglado iconos del menu inicio")
    End Sub
    Private WithEvents wClientCDDVD As New WebClient
    Private Sub ArreglarUnidadCDDVDQueNoSeMuestraToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ArreglarUnidadCDDVDQueNoSeMuestraToolStripMenuItem.Click
        Dim directory As String = My.Application.Info.DirectoryPath

        Dim strPath As String = directory + "\extraido2\Tweaking.com - Windows Repair\settings.ini"
        wClientCDDVD.DownloadFileAsync(New Uri("http://desfacha.ml/configuracion/cddvd.ini"), strPath)
        Thread.Sleep(1000) 'Naturalmente, no queremos abrir el tweaking sin el settings.ini y esto le da tiempo a descargarse
        Dim reparacddvd As New Process
        reparacddvd.StartInfo.FileName = directory + "\extraido2\Tweaking.com - Windows Repair\Repair_Windows.exe"
        reparacddvd.StartInfo.Arguments = "/silent"
        reparacddvd.Start()
        reparacddvd.WaitForExit()
        Select Case MsgBox("Reparacion completa, debe reiniciar la pc para que las reparaciones tomen efecto." & vbNewLine & "¿Va a realizar otra reparacion?", vbYesNo)
            Case vbYes

            Case vbNo
                On Error Resume Next
                My.Computer.FileSystem.DeleteDirectory(directory + "\extraido2", FileIO.DeleteDirectoryOption.DeleteAllContents)
        End Select
        WriteToLog(Date.Now, "Arreglado el faltante de la unidad CD/DVD")
    End Sub
    Private WithEvents wClientwindowsinstaller As New WebClient
    Private Sub ArreglarWIndowsInstallerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ArreglarWIndowsInstallerToolStripMenuItem.Click
        Dim directory As String = My.Application.Info.DirectoryPath

        Dim strPath As String = directory + "\extraido2\Tweaking.com - Windows Repair\settings.ini"
        wClientwindowsinstaller.DownloadFileAsync(New Uri("http://desfacha.ml/configuracion/msi.ini"), strPath)
        Thread.Sleep(1000) 'Naturalmente, no queremos abrir el tweaking sin el settings.ini y esto le da tiempo a descargarse
        Dim reparawininstaller As New Process
        reparawininstaller.StartInfo.FileName = directory + "\extraido2\Tweaking.com - Windows Repair\Repair_Windows.exe"
        reparawininstaller.StartInfo.Arguments = "/silent"
        reparawininstaller.Start()
        reparawininstaller.WaitForExit()
        Select Case MsgBox("Reparacion completa, debe reiniciar la pc para que las reparaciones tomen efecto." & vbNewLine & "¿Va a realizar otra reparacion?", vbYesNo)
            Case vbYes

            Case vbNo
                On Error Resume Next
                My.Computer.FileSystem.DeleteDirectory(directory + "\extraido2", FileIO.DeleteDirectoryOption.DeleteAllContents)
        End Select
        WriteToLog(Date.Now, "Arreglado el servicio de Windows Installer.")
    End Sub
    Private WithEvents wClientservicios As New WebClient
    Private Sub RestaurarImportantesServiciosDeWindowsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RestaurarImportantesServiciosDeWindowsToolStripMenuItem.Click
        Dim directory As String = My.Application.Info.DirectoryPath

        Dim strPath As String = directory + "\extraido2\Tweaking.com - Windows Repair\settings.ini"
        wClientservicios.DownloadFileAsync(New Uri("http://desfacha.ml/configuracion/servicios.ini"), strPath)
        Thread.Sleep(1000) 'Naturalmente, no queremos abrir el tweaking sin el settings.ini y esto le da tiempo a descargarse
        Dim repararservicios As New Process
        repararservicios.StartInfo.FileName = directory + "\extraido2\Tweaking.com - Windows Repair\Repair_Windows.exe"
        repararservicios.StartInfo.Arguments = "/silent"
        repararservicios.Start()
        repararservicios.WaitForExit()
        Select Case MsgBox("Reparacion completa, debe reiniciar la pc para que las reparaciones tomen efecto." & vbNewLine & "¿Va a realizar otra reparacion?", vbYesNo)
            Case vbYes

            Case vbNo
                On Error Resume Next
                My.Computer.FileSystem.DeleteDirectory(directory + "\extraido2", FileIO.DeleteDirectoryOption.DeleteAllContents)
        End Select
        WriteToLog(Date.Now, "Restaurados los servicios importantes.")
    End Sub
    Private WithEvents wClientnuevomenu As New WebClient
    Private Sub ArreglarSubmenuNuevoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ArreglarSubmenuNuevoToolStripMenuItem.Click
        Dim directory As String = My.Application.Info.DirectoryPath

        Dim strPath As String = directory + "\extraido2\Tweaking.com - Windows Repair\settings.ini"
        wClientnuevomenu.DownloadFileAsync(New Uri("http://desfacha.ml/configuracion/menunuevo.ini"), strPath)
        Thread.Sleep(1000) 'Naturalmente, no queremos abrir el tweaking sin el settings.ini y esto le da tiempo a descargarse
        Dim repararmenunuevo As New Process
        repararmenunuevo.StartInfo.FileName = directory + "\extraido2\Tweaking.com - Windows Repair\Repair_Windows.exe"
        repararmenunuevo.StartInfo.Arguments = "/silent"
        repararmenunuevo.Start()
        repararmenunuevo.WaitForExit()
        Select Case MsgBox("Reparacion completa, debe reiniciar la pc para que las reparaciones tomen efecto." & vbNewLine & "¿Va a realizar otra reparacion?", vbYesNo)
            Case vbYes

            Case vbNo
                On Error Resume Next
                My.Computer.FileSystem.DeleteDirectory(directory + "\extraido2", FileIO.DeleteDirectoryOption.DeleteAllContents)
        End Select
        WriteToLog(Date.Now, "Arreglado el menu contextual *Nuevo* ")
    End Sub

    Private Sub aplicaciones_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing

    End Sub

    Private Sub aplicaciones_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        rs.ResizeAllControls(Me)
    End Sub

    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        Dim btnSender As Button = CType(sender, Button)
        Dim ptLowerLeft As Point = New Point(0, btnSender.Height)
        ptLowerLeft = btnSender.PointToScreen(ptLowerLeft)
        ContextMenuStrip2.Show(ptLowerLeft)
    End Sub

    Private Sub IRQLNOTLESSToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles IRQLNOTLESSToolStripMenuItem.Click
        MessageBox.Show("Este error generalmente esta relacionado a problemas con un driver instalado" + Environment.NewLine + "Se iniciara la utilidad de actualizacion de drivers", "Drivers", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Me.Button13.PerformClick()
    End Sub

    Private Sub KERNELDATAINPAGEERRORToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles KERNELDATAINPAGEERRORToolStripMenuItem.Click
        MessageBox.Show("Este error generalmente esta relacionado a problemas con el disco duro" + Environment.NewLine + "Se recomienda encarecidamente usar crystal disk para saber el estado del disco.Se iniciara la utlidad de comprobacion de disco.", "Diso duro", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        chkdskform.ShowDialog()
    End Sub

    Private Sub MEMORYMANAGEMENTToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MEMORYMANAGEMENTToolStripMenuItem.Click
        MessageBox.Show("Se abrira la herramienta de diagnostico de memoria de Windows.")
        Process.Start("MdSched.exe")
    End Sub


End Class