'     PARA HACER
'  Leer exit codes de algunos procesos para obtener mas info.
'  Agregar Windows Update minitool (Ya lo subi al server.)
'LA PAGINA FOSSIES TIENE LINKS DIRECTOS DE PROGRAMAS
'https://www.youtube.com/watch?v=TMzJFzRghV4 Alto video xDXD
' IE 5 MINIMO REQUERIDO (PUESTO EL CHECKEO) --13/07/16 SACADO EL CHECKEO, ES AL PEDO.

' Cambiar el metodo de extraccion por el uso de una libreria

Imports System.ComponentModel
Imports System.Globalization
Imports System.IO
Imports System.Management
Imports System.Runtime.InteropServices
Imports System.Security.Cryptography
Imports System.Security.Principal
Imports System.Text
Imports System.Threading

Public Class Form1



    ' Inicio de snippet para el event log
    ' Private Declare Function BackupEventLog Lib "advapi32.dll" Alias "BackupEventLogA" (ByVal hEventLog As IntPtr, ByVal lpBackupFileName As String) As Integer
    '  Private Declare Function CloseEventLog Lib "advapi32.dll" (ByVal hEventLog As IntPtr) As IntPtr
    '  Private Declare Function OpenEventLog Lib "advapi32.dll" Alias "OpenEventLogA" (ByVal lpUNCServerName As String, ByVal lpSourceName As String) As IntPtr
    '  Fin snippet de event log



    Private Declare Function FindWindow Lib "user32" Alias "FindWindowA" _
    (ByVal lpClassName As String, ByVal lpWindowName As String) As Integer

    Private Declare Function FindWindowEx Lib "user32" Alias "FindWindowExA" _
    (ByVal hWnd1 As Integer, ByVal hWnd2 As Integer, ByVal lpsz1 As String,
    ByVal lpsz2 As String) As Integer

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Private Shared Function GetWindowText(ByVal hwnd As IntPtr, ByVal lpString As StringBuilder, ByVal cch As Integer) As Integer
    End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Private Shared Function GetWindowTextLength(ByVal hwnd As IntPtr) As Integer
    End Function

    Private Declare Sub SetWindowPos Lib "user32" (ByVal hwnd As Integer, ByVal _
    hWndInsertAfter As Integer, ByVal X As Integer, ByVal Y As Integer, ByVal cx As _
    Integer, ByVal cy As Integer, ByVal wFlags As Integer)

    Private Declare Function SetCursorPos Lib "user32.dll" (
    ByVal X As Int32, ByVal Y As Int32) As Boolean

    <DllImport("user32.dll")>
    Private Shared Function GetWindowRect(ByVal HWND As Integer, ByRef lpRect As RECT) As Boolean
    End Function

    Private Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Integer)

    Private Declare Sub mouse_event Lib "user32.dll" (ByVal dwFlags As Integer,
    ByVal dx As Integer, ByVal dy As Integer, ByVal cButtons As Integer, ByVal dwExtraInfo As Integer)

    '~~> Constants for pressing left button of the mouse
    Private Const MOUSEEVENTF_LEFTDOWN As Integer = &H2
    '~~> Constants for Releasing left button of the mouse
    Private Const MOUSEEVENTF_LEFTUP As Integer = &H4
    Private Declare Function SetFocus Lib "user32.dll" (ByVal hWnd As IntPtr) As Integer

    <StructLayout(LayoutKind.Sequential)> Public Structure RECT
        Dim Left As Integer
        Dim Top As Integer
        Dim Right As Integer
        Dim Bottom As Integer
    End Structure
    Dim modoseguro As Boolean = False
    Const HWND_TOPMOST = -1
    Const HWND_NOTOPMOST = -2
    Const SWP_NOSIZE = &H1
    Const SWP_NOMOVE = &H2
    Const SWP_NOACTIVATE = &H10
    Const SWP_SHOWWINDOW = &H40
    Public Declare Function SetForegroundWindow Lib "user32.dll" (ByVal hwnd As Integer) As Integer
    Dim Ret As Integer, ChildRet As Integer, OpenRet As Integer
    Dim strBuff As String, ButCap As String
    <DllImport("kernel32.dll")>
    Private Shared Function GetVolumeInformation(ByVal PathName As String, ByVal VolumeNameBuffer As StringBuilder, ByVal VolumeNameSize As Int32, ByRef VolumeSerialNumber As Int32, ByRef MaximumComponentLength As Int32, ByRef FileSystemFlags As Int32, ByVal FileSystemNameBuffer As StringBuilder, ByVal FileSystemNameSize As Int32) As Long
    End Function
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
    Dim hdserial As String

    Public Function Nombredeusuario() As String
        Dim usuario As New WindowsPrincipal(WindowsIdentity.GetCurrent())
        Return usuario.Identity.Name
    End Function
    Public Function Niveldeauntenticacion() As String
        Dim user As New WindowsPrincipal(WindowsIdentity.GetCurrent())
        Return user.Identity.AuthenticationType
    End Function
    Dim usuario As String
    Public Function IsInternetAvailable() As Boolean
        ' Si todas fallan al pingear, retorna falso

        Dim sitesToCheck() As String = {"www.google.com",
    "www.microsoft.com", "www.reddit.com"}

        Dim failedPings As Integer = 0

        ' Se hace un loop
        For Each str As String In sitesToCheck
            Try
                My.Computer.Network.Ping(str)
            Catch ex As Exception
                failedPings += 1
            End Try
        Next

        ' No se pudo pingear..
        If failedPings = sitesToCheck.Length Then
            Return False
        Else
            Return True
        End If
    End Function
    Public Function CheckInstanceOfApp() As Boolean
        Dim appProc() As Process


        Dim strModName, strProcName As String
        strModName = Process.GetCurrentProcess.MainModule.ModuleName
        strProcName = System.IO.Path.GetFileNameWithoutExtension(strModName)

        appProc = Process.GetProcessesByName(strProcName)

        If appProc.Length > 1 Then
            Return False
        End If

        Return True

    End Function

    Private Function GetOSArchitecture() As Integer
        Dim pa As String = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE")
        Return (If(([String].IsNullOrEmpty(pa) OrElse [String].Compare(pa, 0, "x86", 0, 3, True) = 0), 32, 64))
    End Function
    'Dim rs As New Resizer
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' Obtenemos la informacion necesaria...
        Dim hayinternet As Boolean
        hdserial = GetVolumeSerial("c")
        usuario = nombredeusuario()
        hayinternet = isInternetAvailable()
        ' 2 desfachas para doble poder? Nop, asi no funcan las cosas, solo 1 esta permitido.
        If CheckInstanceOfApp() = False Then
            MsgBox("Solo se puede ejecutar una instancia del programa.", MsgBoxStyle.Critical)
            End
        End If
        Dim drive As New DriveInfo("C") ' 
        Dim percentFree As Double = 100 * CDbl(drive.TotalFreeSpace) / drive.TotalSize
        '  '  '  NO DESCOMENTAR! '  '  '  '  ' 

        '  '  '  '  '  '  '  '  '  '  '  '  '  '  '  '  '  '  '  '  '  '  '  '  '  '  '  '  '  ' 
        ' Esto es redundante y no se porque lo tengo como requisito, ya que net 2.0 requiere ie 5 de por si... 
        ' Lo mantengo por si existe alguna build loca por ahi modificada de 98se, me y 2000
        ' Aunque es totalmente improbable que este programa se ejecute en alguna de esas builds...
        '     Dim ie As New WebBrowser
        '   If (ie.Version.Major >= 5) Then
        '    Else
        '        If My.Settings.log = True Then
        '            WriteToLog(Date.Now, "Se requiere minimo internet explorer 5, no se puede continuar. Version de IE: " + ie.Version.Major)
        '         End If
        '         MsgBox("Internet explorer 5 o superior es requerido, no se puede continuar.", MsgBoxStyle.Critical)
        '         End
        '       End If
        '     ie = Nothing
        '  '  '  '  '  '  '  '  '  '  '  '  '  '  '  '  '  '  '  '  '  '  '  '  '  '  '  '  '  ' 
        ToolStripProgressBar2.Value = 0
        PictureBox1.Visible = False
        PictureBox2.Visible = False
        Select Case System.Windows.Forms.SystemInformation.BootMode
            Case BootMode.FailSafe
                MsgBox("Por favor, reinicie en modo seguro con red")

                End
            Case BootMode.FailSafeWithNetwork
                modoseguro = True
                Dim versionos2 = Environment.OSVersion.Version.Major
                If versionos2 = 6 Then
                    Dim officecache As String = "bcdedit /deletevalue {default} safeboot"  ' Script 

                    Dim directory As String = My.Application.Info.DirectoryPath
                    Dim bat1path As String
                    bat1path = directory + "\scriptseguro1.bat"
                    If Not File.Exists(bat1path) Then
                        Dim strBatLine1 As String = officecache
                        My.Computer.FileSystem.WriteAllText(bat1path, strBatLine1, False, System.Text.Encoding.Default)
                        SetAttr(bat1path, FileAttribute.Normal)
                    End If
                    Dim modoseguro1 As New Process
                    modoseguro1.StartInfo.FileName = bat1path
                    modoseguro1.Start()
                    modoseguro1.WaitForExit()
                    If My.Settings.log = True Then
                        WriteToLog(Date.Now, "Se restablecio al modo normal.")
                    End If
                    On Error Resume Next
                    My.Computer.FileSystem.DeleteFile(bat1path)
                End If
                ' Se necesita internet, si no, el programa no tiene sentido, hey, por ahora...
                If hayinternet = False Then
                    If My.Settings.log = True Then
                        WriteToLog(Date.Now, "No hay conexion a internet. No se puede continuar")
                    End If
                    MsgBox("No se ha podido establecer conexion a internet. No se puede continuar", MsgBoxStyle.Critical)
                    End
                End If
                ' Nos libramos de toda culpa
                If My.Settings.eula = False Then
                    Select Case MsgBox("LEER:" + vbNewLine + "Este programa hace muchas cosas, si bien no hacen daño, puede hacer cosas que usted no esperaba, como eliminar cookies, sesiones, etc. Si DesFacha hace algo que no esperaba y no leyo las instrucciones, es SU CULPA. Corriendo este programa usted acepta la completa responsabilidad por cualquier cosa que pase. No hay garantia, usted lo corre bajo su propio riesgo y es su propia responsabilidad." + vbNewLine + "Si acepta esto, CLICKEE ACEPTAR. En caso de no aceptarlo, clickee CANCELAR", MessageBoxButtons.OKCancel, "EULA")
                        Case vbOK
                            My.Settings.eula = True
                            If My.Settings.log = True Then
                                WriteToLog(Date.Now, "Modo seguro con red. Desfacha " & My.Application.Info.Version.ToString & " Inicio del programa. Usuario:" + usuario)
                                Dim versionos = Environment.OSVersion.Version.Major
                                Select Case versionos
                                    Case 6
                                        WriteToLog("Version del SO", "NT 6 " & My.Computer.Info.OSFullName)
                                        If Antivirusinstalado() = True Then
                                            Label1.Text = " Antivirus: Instalado"
                                            PictureBox1.Visible = True
                                        Else
                                            Label1.Text = " Antivirus: NO Instalado"
                                            PictureBox2.Visible = True
                                        End If
                                    Case 5
                                        WriteToLog("Version del SO", "NT 5 " & My.Computer.Info.OSFullName)
                                        If Antivirusinstaladoenxp() = True Then
                                            Label1.Text = " Antivirus: Instalado"
                                            PictureBox1.Visible = True
                                        Else
                                            Label1.Text = " Antivirus: NO Instalado"
                                            PictureBox2.Visible = True
                                        End If
                                    Case 4
                                        If Environment.OSVersion.Version.Minor = 10 Or 90 Then
                                            WriteToLog("Version del SO", "Windows 9X " & My.Computer.Info.OSFullName)
                                            Button2.Enabled = False
                                        End If

                                End Select

                            End If
                        Case vbCancel
                            End
                    End Select
                Else
                    If My.Settings.log = True Then
                        WriteToLog(Date.Now, "Modo seguro con red. Desfacha " & My.Application.Info.Version.ToString & " Inicio del programa. Usuario:" + usuario)
                        Dim versionos = Environment.OSVersion.Version.Major
                        Select Case versionos
                            Case 6
                                WriteToLog("Version del SO", "NT 6 " & My.Computer.Info.OSFullName)
                                If Antivirusinstalado() = True Then
                                    Label1.Text = " Antivirus: Instalado"
                                    PictureBox1.Visible = True
                                Else
                                    Label1.Text = " Antivirus: NO Instalado"
                                    PictureBox2.Visible = True
                                End If
                            Case 5
                                WriteToLog("Version del SO", "NT 5 " & My.Computer.Info.OSFullName)
                                If Antivirusinstaladoenxp() = True Then
                                    Label1.Text = " Antivirus: Instalado"
                                    PictureBox1.Visible = True
                                Else
                                    Label1.Text = " Antivirus: NO Instalado"
                                    PictureBox2.Visible = True
                                End If
                            Case 4
                                If Environment.OSVersion.Version.Minor = 10 Or 90 Then
                                    WriteToLog("Version del SO", "Windows 9X " & My.Computer.Info.OSFullName)
                                    Button2.Enabled = False
                                End If
                        End Select
                    End If
                End If

        End Select
        If modoseguro = True Then
            ToolStripStatusLabel3.Text = "Corriendo en modo seguro"
        Else
            If hayinternet = False Then
                If My.Settings.log = True Then
                    WriteToLog(Date.Now, "No hay internet. No se puede continuar")
                End If
                MsgBox("No se ha podido establecer conexion a internet. No se puede continuar", MsgBoxStyle.Critical)
                End

            End If
            If My.Settings.eula = False Then

                Select Case MsgBox("LEER:" + vbNewLine + "Este programa hace muchas cosas, si bien no hacen daño, puede hacer cosas que usted no esperaba, como eliminar cookies, sesiones, etc. Si DesFacha hace algo que no esperaba y no leyo las instrucciones, es SU CULPA. Corriendo este programa usted acepta la completa responsabilidad por cualquier cosa que pase. No hay garantia, usted lo corre bajo su propio riesgo y es su propia responsabilidad." + vbNewLine + "Si acepta esto, CLICKEE ACEPTAR. En caso de no aceptarlo, clickee CANCELAR", MessageBoxButtons.OKCancel, "EULA")
                    Case vbOK
                        My.Settings.eula = True
                    Case vbCancel
                        End
                End Select
            End If
            ToolStripStatusLabel3.Text = "Corriendo en modo normal"
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "Inicio del programa. Desfacha " & My.Application.Info.Version.ToString & " Modo normal. Usuario:" + usuario)
                Dim versionos = Environment.OSVersion.Version.Major
                Select Case versionos
                    Case 6
                        WriteToLog("Version del SO", "NT 6 " & My.Computer.Info.OSFullName)
                        If Antivirusinstalado() = True Then
                            Label1.Text = " Antivirus: Instalado"
                            PictureBox1.Visible = True
                        Else
                            Label1.Text = " Antivirus: NO Instalado"
                            PictureBox2.Visible = True
                        End If
                    Case 5
                        WriteToLog("Version del SO", "NT 5 " & My.Computer.Info.OSFullName)
                        If Antivirusinstaladoenxp() = True Then
                            Label1.Text = " Antivirus: Instalado"
                            PictureBox1.Visible = True
                        Else
                            Label1.Text = " Antivirus: NO Instalado"
                            PictureBox2.Visible = True
                        End If
                    Case 4
                        If Environment.OSVersion.Version.Minor = 10 Or 90 Then
                            WriteToLog("Version del SO", "Windows 9X " & My.Computer.Info.OSFullName)
                            MessageBox.Show("Desfacha no deberia ser ejecutado en Windows 9x, el programa podria no funcionar correctamente", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        End If
                End Select
            End If
        End If
        ' Esto tampoco tiene sentido, por ahora, ya que permite redimensionar controles
        ' pero no permito que se pueda redimensionar nada, por ahora, si algun dia lo permito, ya esta todo codeado.

        ' rs.FindAllControls(Me) '
        ' Chequeamos que desfacha no este corrupto o que exista y procedemos a bajarlo si no esta, o esta, pero corrupto.
        If File.Exists("desfachaupd.exe") Then
            Dim integridad As String = GenerateFileMD5("desfachaupd.exe")
            If integridad = "7FC977260CBFF75F2ED7AF97326A1F9F" Then
                Dim checkearactualizacion As New Process
                checkearactualizacion.StartInfo.FileName = ("desfachaupd.exe")
                checkearactualizacion.StartInfo.Arguments = "-desfacha"
                checkearactualizacion.Start()
                checkearactualizacion.WaitForExit()
                Me.BringToFront()
            Else
                MsgBox("La version de Desfachaupd.exe no es coincidente. Se bajara nuevamente el archivo...", MsgBoxStyle.Critical)
                Dim Ret As Integer
                Dim strURL As String
                Dim strPath As String
                Dim directory As String = My.Application.Info.DirectoryPath

                strURL = "http://desfacha.ml/desfachaupd.exe"
                strPath = directory + "\desfachaupd.exe"
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(strPath)
                Thread.Sleep(1000)
                Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
                If Ret = 0 Then

                    If My.Settings.log = True Then
                        WriteToLog(Date.Now, "Desfachaupd restaurado. Motivo de descarga: desfachaupd corrupto.")
                    End If
                    Application.Restart()
                Else
                    MsgBox("Error descargando desfachaupd.exe")
                    If My.Settings.log = True Then
                        WriteToLog(Date.Now, "Error descargando desfachaupd. Motivo de descarga: desfachaupd corrupto.")
                    End If
                End If

            End If



        Else

            MsgBox("No se encuentra desfachaupd.exe. Intentando descargarlo...", MsgBoxStyle.Critical)
            Dim Ret As Integer
            Dim strURL As String
            Dim strPath As String
            Dim directory As String = My.Application.Info.DirectoryPath

            strURL = "http://desfacha.ml/desfachaupd.exe"
            strPath = directory + "\desfachaupd.exe"
            Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
            If Ret = 0 Then

                If My.Settings.log = True Then
                    WriteToLog(Date.Now, "Desfachaupd descargado. Motivo: No se encontraba desfachaupd")
                End If
                Application.Restart()
            End If
        End If

        Select Case CultureInfo.CurrentCulture.ThreeLetterISOLanguageName
            Case "eng"
                Button1.Text = "Start"
                Button3.Text = "Clean PC"
                GroupBox3.Text = "General information"
                ToolStripStatusLabel4.Text = "Progress:"
                TabPage1.Text = "Start"
                TabPage2.Text = "Options"
                GroupBox1.Text = "General options"
                CheckBox2.Text = "Shutdown pc when finished"
                CheckBoxlogs.Text = "Delete logs when finished"
                GroupBox2.Text = "Advanced options"
                CheckBox3.Text = "Delete disconnected USB devices"
                GroupBox4.Text = "Start button specific options"
                SToolStripMenuItem.Text = "Tools"
                AplicacionesToolStripMenuItem.Text = "Apps and Fixes"
                InformacionUtilToolStripMenuItem.Text = "Useful information"
                ModoDeEmergenciaToolStripMenuItem.Text = "Emergency mode"
                ReiniciarEnModoSeguroToolStripMenuItem.Text = "Reset PC in safe mode"
                AyudaToolStripMenuItem.Text = "Help"
                NovedadesToolStripMenuItem.Text = "Changelog"
                AcercaDeToolStripMenuItem.Text = "About"
        End Select
    End Sub
    Function GenerateFileMD5(ByVal filePath As String)
        Dim md5 As MD5CryptoServiceProvider = New MD5CryptoServiceProvider
        Dim f As FileStream = New FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 8192)

        f = New FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 8192)
        md5.ComputeHash(f)
        f.Close()

        Dim hash As Byte() = md5.Hash
        Dim buff As StringBuilder = New StringBuilder
        Dim hashByte As Byte

        For Each hashByte In hash
            buff.Append(String.Format("{0:X2}", hashByte))
        Next

        Dim md5string As String
        md5string = buff.ToString()

        Return md5string
    End Function

    '  Dim Findstring = IO.File.ReadAllText("Your File Path")
    '   Dim Lookfor As String = "hello"

    '   If FindString.Contains(Lookfor) Then
    'Do something
    '  Msgbox("Found: " & Lookfor)
    '    End If

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



    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        TextBox1.AppendText("Descargando Malware Bytes 3.0.6.1469" + Environment.NewLine)
        ToolStripProgressBar2.Value = 0
        espereporfavor.Show()

        If My.Computer.FileSystem.DirectoryExists("%ProgramFiles(x86)%\Malwarebytes Anti-Malware") Or My.Computer.FileSystem.DirectoryExists("%ProgramFiles%\Malwarebytes Anti-Malware") = True Then
            Dim directory As String
            Dim buscartemp As String = TextBox3.Text  ' Script de tron script
            directory = My.Application.Info.DirectoryPath
            Dim bat2path As String = directory + "\script3.bat"
            If Not File.Exists(bat2path) Then
                Dim strBatLine1 As String = buscartemp
                My.Computer.FileSystem.WriteAllText(bat2path, strBatLine1, False, System.Text.Encoding.Default)
                SetAttr(bat2path, FileAttribute.Normal)
            End If
            Dim script2 As New Process
            ToolStripProgressBar2.Value = 100
            script2.StartInfo.FileName = bat2path
            script2.Start()
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "Inicio de malwarebytes.")
            End If
            ' MsgBox("Recuerde, para cerrar el tutorial apriete ESC")
            My.Computer.FileSystem.DeleteFile(bat2path)
            '   malwarepasos.Show()
            script2.WaitForExit()

        Else

            Dim Ret As Integer
            Dim strURL As String
            Dim strPath As String
            Dim directory As String = My.Application.Info.DirectoryPath

            '     strURL = "https://data-cdn.mbamupdates.com/web/mbam-setup-2.2.1.1043.exe"
            'strURL = "https://data-cdn.mbamupdates.com/web/mb3-setup-consumer-3.0.6.1469.exe"
            strURL = "https://data-cdn.mbamupdates.com/web/mb3-setup-consumer/mb3-setup-consumer-3.1.2.1733-1.0.122-1.0.1976.exe"
            strPath = directory + "\mbam3.exe"

            Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
            If Ret = 0 Then
                TextBox1.AppendText("Ejecutando malwarebytes" + Environment.NewLine)
                ToolStripProgressBar2.Value = ToolStripProgressBar2.Value + 20
                Dim setup As New Process
                setup.StartInfo.FileName = strPath
                setup.StartInfo.Arguments = "/verysilent"
                setup.Start()
                setup.WaitForExit()
                ToolStripProgressBar2.Value = ToolStripProgressBar2.Value + 20
                Dim buscartemp As String = TextBox3.Text  ' Script de tron script
                Dim bat2path As String = directory + "\script3.bat"
                If Not File.Exists(bat2path) Then
                    Dim strBatLine1 As String = buscartemp
                    My.Computer.FileSystem.WriteAllText(bat2path, strBatLine1, False, System.Text.Encoding.Default)
                    SetAttr(bat2path, FileAttribute.Normal)
                End If
                Dim script2 As New Process
                script2.StartInfo.FileName = bat2path
                script2.Start()
                If My.Settings.log = True Then
                    WriteToLog(Date.Now, "Inicio malwarebytes.")
                End If
                script2.WaitForExit()

                If My.Settings.eliminarluego = True Then
                    On Error Resume Next
                    My.Computer.FileSystem.DeleteFile(strPath)
                End If
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(bat2path)
                ToolStripProgressBar2.Value = 100
                '    MsgBox("Recuerde, para cerrar el tutorial apriete ESC")
                '    malwarepasos.Show()

            End If
            Ret = Nothing
            strURL = Nothing
            strPath = Nothing
            directory = Nothing
        End If
        espereporfavor.Close()
    End Sub

    Public Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        On Error Resume Next
        For Each p As Process In System.Diagnostics.Process.GetProcessesByName("firefox")
            '  Try
            MsgBox("Se ha detectado una instancia de Firefox" + Environment.NewLine + "Necesita cerrarlo para continuar. Al presionar aceptar se cerrara automaticamente.", MsgBoxStyle.OkOnly, "Desfacha")

            p.Kill()
            ' possibly with a timeout
            '        p.WaitForExit()
            ' process was terminating or can't be terminated - deal with it
            '  Catch winException As Win32Exception
            ' process has already exited - might be able to let this one go
            '  Catch invalidException As InvalidOperationException
            '  End Try
        Next
        On Error Resume Next
        For Each p2 As Process In System.Diagnostics.Process.GetProcessesByName("chrome")
            MsgBox("Se ha detectado una instancia de Chrome" + Environment.NewLine + "Necesita cerrarlo para continuar. Al presionar aceptar se cerrara automaticamente.", MsgBoxStyle.OkOnly, "Desfacha")

            p2.Kill()
        Next


        Dim drive As New DriveInfo("C")
        Dim espaciolibreenMB As Integer = Math.Round(drive.TotalFreeSpace / 1024 / 1024, 1)

        ToolStripProgressBar2.Value = 0
        Dim versionos = Environment.OSVersion.Version.Major
        If versionos = 6 Then
            Dim limpiarcacheie As New Process
            limpiarcacheie.StartInfo.FileName = "rundll32.exe"
            limpiarcacheie.StartInfo.Arguments = "inetcpl.cpl,ClearMyTracksByProcess 4351"
            limpiarcacheie.Start()
            ToolStripProgressBar2.Value = ToolStripProgressBar2.Value + 20
            TextBox1.AppendText("Temporales de IE, eliminados." + Environment.NewLine)
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "Temporales de IE, eliminados")
            End If
        End If

        Dim officecache As String = "if exist %SystemDrive%\MSOCache rmdir /S /Q %SystemDrive%\MSOCache"  ' Script de tron script

        Dim directory As String = My.Application.Info.DirectoryPath
        Dim bat1path As String
        bat1path = directory + "\script1.bat"
        If Not File.Exists(bat1path) Then
            Dim strBatLine1 As String = officecache
            My.Computer.FileSystem.WriteAllText(bat1path, strBatLine1, False, System.Text.Encoding.Default)
            SetAttr(bat1path, FileAttribute.Normal)
        End If
        Dim officecacheproceso As New Process
        officecacheproceso.StartInfo.FileName = bat1path
        officecacheproceso.Start()
        officecacheproceso.WaitForExit()
        ToolStripProgressBar2.Value = ToolStripProgressBar2.Value + 20
        TextBox1.AppendText("Temporales de instalacion de office. Eliminados" + Environment.NewLine)
        If My.Settings.log = True Then
            WriteToLog(Date.Now, "Temporales de instalacion de office. Eliminados")
        End If
        On Error Resume Next
        My.Computer.FileSystem.DeleteFile(bat1path)
        bat1path = Nothing
        directory = Nothing
        officecache = Nothing
        officecacheproceso = Nothing

        Dim buscartemp As String = TextBox2.Text  ' Script de tron script
        directory = My.Application.Info.DirectoryPath
        Dim bat2path As String = directory + "\script2.bat"
        If Not File.Exists(bat2path) Then
            Dim strBatLine1 As String = buscartemp
            My.Computer.FileSystem.WriteAllText(bat2path, strBatLine1, False, System.Text.Encoding.Default)
            SetAttr(bat2path, FileAttribute.Normal)
        End If
        Dim script2 As New Process
        script2.StartInfo.FileName = bat1path
        script2.Start()
        script2.WaitForExit()
        ToolStripProgressBar2.Value = ToolStripProgressBar2.Value + 20
        On Error Resume Next
        My.Computer.FileSystem.DeleteFile(bat2path)
        bat2path = Nothing
        directory = Nothing
        script2 = Nothing
        buscartemp = Nothing
        TextBox1.AppendText("Limpieza de cache de Windows integral. Completa" + Environment.NewLine)
        If My.Settings.log = True Then
            WriteToLog(Date.Now, "Limpieza de cache de Windows integral, completa.")
        End If

        Dim Ret As Integer
        Dim strURL As String
        Dim strPath As String
        directory = My.Application.Info.DirectoryPath
        Dim limpiar As New Process
        strURL = "http://desfacha.ml/archivos/ccsetup525.zip"
        strPath = directory + "\ccsetup525.zip"

        Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
        If Ret = 0 Then
            Dim shObj As Object = Activator.CreateInstance(Type.GetTypeFromProgID("Shell.Application"))
            IO.Directory.CreateDirectory(directory + "\extraido")
            Dim output As Object = shObj.NameSpace((directory + "\extraido"))
            Dim input As Object = shObj.NameSpace((strPath))
            output.CopyHere((input.Items), 4)

            If GetOSArchitecture() = 32 Then
                limpiar.StartInfo.FileName = (directory + "\extraido\ccsetup525\CCleaner.exe")

            ElseIf GetOSArchitecture() = 64 Then
                limpiar.StartInfo.FileName = (directory + "\extraido\ccsetup525\CCleaner64.exe")
            End If

            limpiar.StartInfo.Arguments = "/AUTO"
            limpiar.Start()
            limpiar.WaitForExit()
            TextBox1.AppendText("CCleaner ejecutado" + Environment.NewLine)

            limpiar = Nothing
            ToolStripProgressBar2.Value = ToolStripProgressBar2.Value + 20
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "CCLEANER ejecutado (Boton de temporales)")
            End If
            If My.Settings.eliminarluego = True Then
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(strPath)
                My.Computer.FileSystem.DeleteDirectory(directory + "\extraido", FileIO.DeleteDirectoryOption.DeleteAllContents)
            End If
            Ret = Nothing
            strURL = Nothing
            strPath = Nothing
            directory = Nothing
        End If

        ' BLEACHBIT
        strURL = "https://download.bleachbit.org/BleachBit-1.12-portable.zip"
        directory = My.Application.Info.DirectoryPath
        strPath = directory + "\bleachbit112.zip"

        Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)

        If Ret = 0 Then

            Dim shObj As Object = Activator.CreateInstance(Type.GetTypeFromProgID("Shell.Application"))
            'Create directory in which you will unzip your items.
            IO.Directory.CreateDirectory(directory + "\extraido")

            'Declare the folder where the items will be extracted.
            Dim output As Object = shObj.NameSpace((directory + "\extraido"))

            'Declare the input zip file.
            Dim input As Object = shObj.NameSpace((strPath))

            'Extract the items from the zip file.
            output.CopyHere((input.Items), 4)
            Dim ini As String = "http://desfacha.ml/archivos/BleachBit.inix"
            Dim directoriodelini As String = directory + "\extraido\BleachBit-Portable\BleachBit.ini"
            On Error Resume Next
            My.Computer.FileSystem.DeleteFile(directoriodelini)
            Ret = URLDownloadToFile(0, ini, directoriodelini, 0, 0)
            Dim bleachbit As New Process
            bleachbit.StartInfo.FileName = directory + "\extraido\BleachBit-Portable\bleachbit_console.exe"
            bleachbit.StartInfo.Arguments = "--preset --clean"
            bleachbit.Start()
            bleachbit.WaitForExit()
            TextBox1.AppendText("BleachBit ejecutado." + Environment.NewLine)
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "BleachBit ejecutado (Boton de temporales)")
            End If
            If My.Settings.eliminarluego = True Then
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(strPath)
                My.Computer.FileSystem.DeleteDirectory(directory + "\extraido", FileIO.DeleteDirectoryOption.DeleteAllContents)
            End If

            ToolStripProgressBar2.Value = ToolStripProgressBar2.Value + 5
        End If
        'FindDupes


        'SXS windows update
        ' seteamos variable de comprobacion, no es lo mas efectivo, pero funciona.
        Dim comprobadosxs As Boolean = False
        If Environment.OSVersion.Version.Major = 5 Then
            WriteToLog(Date.Now, "Omitiendo los comandos Dism, ya que estamos sobre NT5")
        ElseIf Environment.OSVersion.Version.Major = 6 And Environment.OSVersion.Version.Minor = 0 Then

            Dim versiondelsistema = Environment.OSVersion.Version.Major
            Dim versiondelsistemaMIN = Environment.OSVersion.Version.Minor
            If versiondelsistema = 6 And versiondelsistemaMIN = 0 Then ' WINDOWS VISTA

                Dim dism2 As String = "Dism /online /Cleanup-Image /StartComponentCleanup"  ' Script de tron script

                Dim bat1path7 As String
                bat1path7 = directory + "\script999.bat"
                If Not File.Exists(bat1path7) Then
                    Dim strBatLine1 As String = officecache
                    My.Computer.FileSystem.WriteAllText(bat1path7, strBatLine1, False, System.Text.Encoding.Default)
                    SetAttr(bat1path7, FileAttribute.Normal)
                End If
                Dim dismborrar2 As New Process
                dismborrar2.StartInfo.FileName = bat1path7
                dismborrar2.Start()
                dismborrar2.WaitForExit()
                comprobadosxs = True
                WriteToLog(Date.Now, "Comandos dism ejecutados (Sobre windows vista)")
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(bat1path7)
                ToolStripProgressBar2.Value = ToolStripProgressBar2.Value + 5

            End If
            ' PARA WIN 7 EN ADELANTE
            If comprobadosxs = False Then
                Dim dism As String = "Dism /online /Cleanup-Image /StartComponentCleanup /ResetBase"  ' Script de tron script

                Dim bat1path5 As String
                bat1path5 = directory + "\script999.bat"
                If Not File.Exists(bat1path5) Then
                    Dim strBatLine1 As String = officecache
                    My.Computer.FileSystem.WriteAllText(bat1path5, strBatLine1, False, System.Text.Encoding.Default)
                    SetAttr(bat1path5, FileAttribute.Normal)
                End If
                Dim dismborrar As New Process
                dismborrar.StartInfo.FileName = bat1path5
                dismborrar.Start()
                dismborrar.WaitForExit()
                WriteToLog(Date.Now, "Comandos dism ejecutados (Windows 7 en adelante)")
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(bat1path5)
                ToolStripProgressBar2.Value = ToolStripProgressBar2.Value + 5
            End If
        End If
        ' LIMPIAR CACHE DE WINDOWS UPDATE
        strURL = "http://desfacha.ml/archivos/scriptwu.batx"

        strPath = directory + "\scriptwu.bat"

        Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
        If Ret = 0 Then
            Dim scriptwu As New Process
            scriptwu.StartInfo.FileName = strPath
            scriptwu.Start()
            scriptwu.WaitForExit()
            On Error Resume Next
            My.Computer.FileSystem.DeleteFile(strPath)
            TextBox1.AppendText("Cache de Windows Update borrada." + Environment.NewLine)
            ToolStripProgressBar2.Value = ToolStripProgressBar2.Value + 5
        End If
        ' Duplicados en carpeta descargas
        strURL = "http://desfacha.ml/archivos/finddupe.exe"

        strPath = directory + "\finddupe.exe"

        Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
        If Ret = 0 Then
            Dim duplicados As New Process
            duplicados.StartInfo.FileName = strPath
            duplicados.StartInfo.Arguments = "-z -del " & GetDownloadsPath() & "\**"
            duplicados.Start()
            duplicados.WaitForExit()
            On Error Resume Next
            My.Computer.FileSystem.DeleteFile(strPath)
            TextBox1.AppendText("Duplicados en descargas eliminados." + Environment.NewLine)
            ToolStripProgressBar2.Value = ToolStripProgressBar2.Maximum
        End If
        ' BACKUP DEL EVENT LOG
        '       On Error Resume Next
        '      Dim hEventLog As IntPtr
        '     Dim lretv As Integer
        '    hEventLog = OpenEventLog(vbNullString, "Application")
        '   If hEventLog = IntPtr.Zero Then
        '  WriteToLog(Date.Now, "Error al abrir el visor de eventos.")
        '
        'End If
        'lretv = BackupEventLog(hEventLog, directory + "\logs\backupdelvisor.evt")
        'If lretv = 0 Then
        'WriteToLog(Date.Now, "Error al intentar hacer backup de eventos.")
        'Else
        'WriteToLog(Date.Now, "Backup de eventos, realizado.")

        'End If
        strURL = "http://desfacha.ml/archivos/espacioderestauracion.batx"

        strPath = directory + "\espacioderestauracion.bat"

        Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
        If Ret = 0 Then
            Dim espacio As New Process
            espacio.StartInfo.FileName = strPath
            espacio.Start()
            espacio.WaitForExit()
            On Error Resume Next
            My.Computer.FileSystem.DeleteFile(strPath)
            TextBox1.AppendText("Reducido el espacio asignado a puntos de restauracion." + Environment.NewLine)
        End If
        On Error Resume Next
        My.Computer.FileSystem.DeleteDirectory("C:\AMD", FileIO.DeleteDirectoryOption.DeleteAllContents)
        My.Computer.FileSystem.DeleteDirectory("C:\NVIDIA", FileIO.DeleteDirectoryOption.DeleteAllContents)
        My.Computer.FileSystem.DeleteDirectory("C:\Dell", FileIO.DeleteDirectoryOption.DeleteAllContents)
        My.Computer.FileSystem.DeleteDirectory("C:\Intel", FileIO.DeleteDirectoryOption.DeleteAllContents)
        My.Computer.FileSystem.DeleteDirectory("C:\HP", FileIO.DeleteDirectoryOption.DeleteAllContents)

        strURL = "http://desfacha.ml/archivos/limpieza.batx"

        strPath = directory + "\limpieza.bat"

        Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
        If Ret = 0 Then
            Dim limpiarmui As New Process
            limpiarmui.StartInfo.FileName = strPath
            limpiarmui.StartInfo.UseShellExecute = True
            limpiarmui.Start()
            limpiarmui.WaitForExit()
            On Error Resume Next
            My.Computer.FileSystem.DeleteFile(strPath)
            TextBox1.AppendText("Limpieza de MUI, WER, Search. Realizada." + Environment.NewLine)
        End If

        If CheckBox3.CheckState = CheckState.Checked Then
            strURL = "http://desfacha.ml/archivos/DeviceCleanupCmd.exe"
            If (GetOSArchitecture() = 64) Then
                strURL = "http://desfacha.ml/archivos/DeviceCleanupCmdx64.exe"

            ElseIf GetOSArchitecture() = 32 Then
                strURL = "http://desfacha.ml/archivos/DeviceCleanupCmd.exe"
            End If
            strPath = directory + "\DeviceCleanupCmd.exe"

            Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
            If Ret = 0 Then
                Dim usb As New Process
                usb.StartInfo.FileName = strPath
                usb.StartInfo.Arguments = "-n"
                usb.Start()
                usb.WaitForExit()
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(strPath)
                TextBox1.AppendText("Eliminados dipositivos USB no conectados." + Environment.NewLine)
            End If
            WriteToLog(Date.Now, "Eliminados dipositivos USB no conectados.")
        End If

        Dim textosucio As String = "fsutil dirty set C:"  ' seteamos el dirty bit en el disco C
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

        strURL = "http://desfacha.ml/archivos/msizap.exe"

        strPath = directory + "\msizap.exe"

        Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
        If Ret = 0 Then
            Dim limpiarmui As New Process
            limpiarmui.StartInfo.FileName = strPath
            limpiarmui.StartInfo.Arguments = "G!"
            limpiarmui.Start()
            limpiarmui.WaitForExit()
            On Error Resume Next
            My.Computer.FileSystem.DeleteFile(strPath)
            TextBox1.AppendText("Limpieza de paquetes MSI huerfanos realizada." + Environment.NewLine)
        End If

        If Environment.OSVersion.Version.Major = 6 Then
            Dim batcertpath As String
            Dim certificados As String = "certutil -URLcache * delete"
            batcertpath = directory + "\scriptcert.bat"
            If Not File.Exists(batcertpath) Then

                My.Computer.FileSystem.WriteAllText(batcertpath, certificados, False, System.Text.Encoding.Default)
                SetAttr(batcertpath, FileAttribute.Normal)
            End If
            Dim cert As New Process
            cert.StartInfo.FileName = batcertpath
            cert.Start()
            cert.WaitForExit()
            TextBox1.AppendText("Limpieza de cache de certificados CrypNet SSL" + Environment.NewLine)
            On Error Resume Next
            My.Computer.FileSystem.DeleteFile(batcertpath)
            batcertpath = Nothing
            directory = Nothing
            certificados = Nothing
        End If

        limpiarprogramas.ShowDialog()

        Dim espaciolibrenmbdespues As Integer = Math.Round(drive.TotalFreeSpace / 1024 / 1024, 1)
        Dim espacioliberado As Integer = espaciolibreenMB - espaciolibrenmbdespues
        WriteToLog(Date.Now, "Espacio liberado en disco luego de la limpieza: " & Math.Abs(espacioliberado + " MB"))
        If CheckBoxlogs.Checked = True Then


            On Error Resume Next
            My.Computer.FileSystem.DeleteFile(directory + "\logs\" + Trim(hdserial) + ".log")
        End If
        If CheckBox2.CheckState = CheckState.Checked Then
            '  ExitWindowsEx(ExitWindows.EWX_SHUTDOWN, ShutdownReason.SHTDN_REASON_FLAG_PLANNED)
            Process.Start("shutdown.exe", " -s -t 10 -f -c ""DesFacha ha programado un apagado automatico en 10 segundos""")
        Else
            MsgBox("Ha terminado el proceso satisfactoriamente.", MsgBoxStyle.Information, "Informacion")
        End If
    End Sub
    Public Shared Function GetDownloadsPath() As String
        Dim path__1 As String = Nothing
        If Environment.OSVersion.Version.Major >= 6 Then
            Dim pathPtr As IntPtr
            Dim hr As Integer = SHGetKnownFolderPath(FolderDownloads, 0, IntPtr.Zero, pathPtr)
            If hr = 0 Then
                path__1 = Marshal.PtrToStringUni(pathPtr)
                Marshal.FreeCoTaskMem(pathPtr)
                Return path__1
            End If
        End If
        path__1 = Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.Personal))
        path__1 = Path.Combine(path__1, "Downloads")
        Return path__1
    End Function

    Private Shared FolderDownloads As New Guid("374DE290-123F-4565-9164-39C4925E467B")
    <DllImport("shell32.dll", CharSet:=CharSet.Auto)>
    Private Shared Function SHGetKnownFolderPath(ByRef id As Guid, flags As Integer, token As IntPtr, ByRef path As IntPtr) As Integer
    End Function
    Dim pos As RECT

    Private Sub SToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles SToolStripMenuItem1.Click
        opciones.ShowDialog()
    End Sub

    Private Sub AplicacionesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AplicacionesToolStripMenuItem.Click
        aplicaciones.ShowDialog()
    End Sub

    Private Sub NovedadesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NovedadesToolStripMenuItem.Click
        novedades.Show()
    End Sub

    Private Sub AcercaDeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AcercaDeToolStripMenuItem.Click
        acercade.ShowDialog()
    End Sub

    Private Declare Function URLDownloadToFile Lib "urlmon" _
    Alias "URLDownloadToFileA" (ByVal pCaller As Integer,
    ByVal szURL As String, ByVal szFileName As String,
    ByVal dwReserved As Integer, ByVal lpfnCB As Integer) As Integer

    Private Sub MenuStrip1_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles MenuStrip1.ItemClicked

    End Sub

    Private Sub ReiniciarEnModoSeguroToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReiniciarEnModoSeguroToolStripMenuItem.Click
        Dim versionos = Environment.OSVersion.Version.Major
        Select Case versionos

            Case 6



                Dim comandoseguro As String = "bcdedit /set {current} safeboot network"  ' Script 

                Dim directory As String = My.Application.Info.DirectoryPath
                Dim bat1path As String
                bat1path = directory + "\scriptseguro1.bat"
                If Not File.Exists(bat1path) Then
                    Dim strBatLine1 As String = comandoseguro
                    My.Computer.FileSystem.WriteAllText(bat1path, strBatLine1, False, System.Text.Encoding.Default)
                    SetAttr(bat1path, FileAttribute.Normal)
                End If
                Dim modoseguro1 As New Process
                modoseguro1.StartInfo.FileName = bat1path
                modoseguro1.Start()
                modoseguro1.WaitForExit()
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(bat1path)
                If My.Settings.log = True Then
                    WriteToLog(Date.Now, "Modo seguro activado, reiniciando...")
                End If
                MsgBox("Debera volver a iniciar el programa para reestablecer el modo normal. (Solo hay que iniciar el programa en modo seguro)", MsgBoxStyle.Information)
                Process.Start("shutdown.exe", " -r -t 0 -f")
            Case 5
                MsgBox("Solo se puede reiniciar en modo seguro desde Windows Vista en adelante.", MsgBoxStyle.Information)
            Case 4
                MsgBox("No soportado en Windows 9X.")
        End Select
    End Sub



    Private Sub InformacionUtilToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles InformacionUtilToolStripMenuItem.Click
        informacion.ShowDialog()
    End Sub


    '' 14/03/17 Si el boton no anda, yo cambie private por public sub. Lo mismo con otros botones.

    Public Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Cerramos los navegadores.
        On Error Resume Next
        For Each p As Process In System.Diagnostics.Process.GetProcessesByName("firefox")
            '  Try
            MsgBox("Se ha detectado una instancia de Firefox" + Environment.NewLine + "Necesita cerrarlo para continuar. Al presionar aceptar se cerrara automaticamente.", MsgBoxStyle.OkOnly, "Desfacha")
            p.Kill()
            ' possibly with a timeout
            '        p.WaitForExit()
            ' process was terminating or can't be terminated - deal with it
            '  Catch winException As Win32Exception
            ' process has already exited - might be able to let this one go
            '  Catch invalidException As InvalidOperationException
            '  End Try
        Next
        On Error Resume Next
        For Each p2 As Process In System.Diagnostics.Process.GetProcessesByName("chrome")
            MsgBox("Se ha detectado una instancia de Chrome" + Environment.NewLine + "Necesita cerrarlo para continuar. Al presionar aceptar se cerrara automaticamente.", MsgBoxStyle.OkOnly, "Desfacha")
            p2.Kill()

        Next

        If My.Settings.log = True Then
            WriteToLog(Date.Now, "Click del boton iniciar. Iniciando...")
        End If
        ' BAJAMOS CAFFEINE PARA TENER DESPIERTA LA PC
        Dim Ret As Integer
        Dim strURL As String
        Dim strPath As String
        Dim directory As String = My.Application.Info.DirectoryPath

        strURL = "http://desfacha.ml/archivos/caffeine.exe"

        strPath = directory + "\caffeine.exe"

        Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)

        If Ret = 0 Then
            Dim cafeina As New Process
            cafeina.StartInfo.FileName = strPath
            cafeina.StartInfo.Arguments = "-noicon"
            cafeina.Start()
        End If

        ' Ponemos en hora la PC
        strURL = "http://desfacha.ml/archivos/enhoralapc.batx"
        strPath = directory + "\enhoralapc.bat"
        Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
        If Ret = 0 Then
            Dim enhora As New Process
            enhora.StartInfo.FileName = strPath
            enhora.Start()
            enhora.WaitForExit()
            On Error Resume Next
            My.Computer.FileSystem.DeleteFile(strPath)
        End If
        ' Chekeamos que sea WIN 7 en adelante. Vista tambien es NT 6 pero no lo soporta.
        If Environment.OSVersion.Version.Major = 6 And Not Environment.OSVersion.Version.Minor = 0 Then
            ' Purgamos la Shadow copy
            strURL = "http://desfacha.ml/archivos/lapurga.batx"
            strPath = directory + "\lapurga.bat"
            Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
            If Ret = 0 Then
                Dim lapurga As New Process
                lapurga.StartInfo.FileName = strPath
                lapurga.Start()
                lapurga.WaitForExit()
                TextBox1.AppendText("Purga realizada." + Environment.NewLine)
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(strPath)
            End If
        End If

        ' INICIO CCLEANER
        ToolStripProgressBar2.Value = 0

        Dim limpiar As New Process

        '  Dim thread As Threading.Thread
        ' Thread = New Threading.Thread(AddressOf espereporfavor.Show)
        '  Thread.Start()

        If CheckBoxnolimpieza.Checked = True Then

        Else

            espereporfavor.Show()
            strURL = "http://desfacha.ml/archivos/ccsetup525.zip"

            strPath = directory + "\ccsetup525.zip"

            Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)

            If Ret = 0 Then

                Dim shObj As Object = Activator.CreateInstance(Type.GetTypeFromProgID("Shell.Application"))
                IO.Directory.CreateDirectory(directory + "\extraido")
                Dim output As Object = shObj.NameSpace((directory + "\extraido"))
                Dim input As Object = shObj.NameSpace((strPath))
                output.CopyHere((input.Items), 4)

                If GetOSArchitecture() = 32 Then
                    limpiar.StartInfo.FileName = (directory + "\extraido\ccsetup525\CCleaner.exe")

                ElseIf GetOSArchitecture() = 64 Then
                    limpiar.StartInfo.FileName = (directory + "\extraido\ccsetup525\CCleaner64.exe")
                End If
                limpiar.StartInfo.Arguments = "/AUTO"
                limpiar.Start()
                limpiar.WaitForExit()
                TextBox1.AppendText("CCleaner ejecutado" + Environment.NewLine)

                limpiar = Nothing
                If My.Settings.log = True Then
                    WriteToLog(Date.Now, "Ccleaner ejecutado.")
                End If
                If My.Settings.eliminarluego = True Then
                    On Error Resume Next
                    My.Computer.FileSystem.DeleteFile(strPath)
                    My.Computer.FileSystem.DeleteDirectory(directory + "\extraido", FileIO.DeleteDirectoryOption.DeleteAllContents)
                End If
                Ret = Nothing
                strURL = Nothing
                strPath = Nothing
                ToolStripProgressBar2.Value = ToolStripProgressBar2.Value + 20
            Else
                MessageBox.Show("No se ha podido descargar CCleaner.")
                If My.Settings.log = True Then
                    WriteToLog(Date.Now, "No se ha podido descargar CCleaner.")
                End If
            End If

            ' FIN CCLEANER


            ' Inicio bleachbit
            strURL = "https://download.bleachbit.org/BleachBit-1.12-portable.zip"

            strPath = directory + "\bleachbit112.zip"

            Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)

            If Ret = 0 Then

                Dim shObj As Object = Activator.CreateInstance(Type.GetTypeFromProgID("Shell.Application"))
                'Create directory in which you will unzip your items.
                IO.Directory.CreateDirectory(directory + "\extraido")

                'Declare the folder where the items will be extracted.
                Dim output As Object = shObj.NameSpace((directory + "\extraido"))

                'Declare the input zip file.
                Dim input As Object = shObj.NameSpace((strPath))

                'Extract the items from the zip file.
                output.CopyHere((input.Items), 4)
                Dim ini As String = "http://desfacha.ml/archivos/BleachBit.inix"
                Dim directoriodelini As String = directory + "\extraido\BleachBit-Portable\BleachBit.ini"
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(directoriodelini)
                Ret = URLDownloadToFile(0, ini, directoriodelini, 0, 0)
                Dim bleachbit As New Process
                bleachbit.StartInfo.FileName = directory + "\extraido\BleachBit-Portable\bleachbit_console.exe"
                bleachbit.StartInfo.Arguments = "--preset --clean"
                bleachbit.Start()
                bleachbit.WaitForExit()
                TextBox1.AppendText("BleachBit ejecutado." + Environment.NewLine)
                If My.Settings.log = True Then
                    WriteToLog(Date.Now, "BleachBit ejecutado.")
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
                ToolStripProgressBar2.Value = ToolStripProgressBar2.Value + 20
            End If

            ' FIN BLEACHBIT

            ' INICIO RKILL
            espereporfavor.Close()
        End If
        strURL = "http://desfacha.ml/archivos/rkill.comx"
        strPath = directory + "\rkill.com"

        Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
        If Ret = 0 Then
            TextBox1.AppendText("Ejecutando Rkill" + Environment.NewLine)
            Dim rkill As New Process
            rkill.StartInfo.FileName = strPath
            rkill.StartInfo.Arguments = "-s -l " & directory + "\logs\Rkilldesfacha.txt"
            rkill.Start()
            rkill.WaitForExit()
            '      Thread.Sleep(180000)
            '      Dim theHandle As IntPtr
            '     theHandle = FindWindow(Nothing, "Rkill Finished")
            '     If theHandle <> IntPtr.Zero Then
            '     SetForegroundWindow(theHandle)
            '    SetFocus(theHandle)
            ' End If
            '    SendKeys.Send("{Enter}")
            '    Thread.Sleep(3000)
            '    Dim pProcess() As Process = System.Diagnostics.Process.GetProcessesByName("notepad")

            '    For Each p As Process In pProcess
            '    p.Kill()
            '   Next
            If My.Settings.eliminarluego = True Then
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(strPath)
            End If
            Ret = Nothing
            strURL = Nothing
            strPath = Nothing
            '      pProcess = Nothing
            Me.BringToFront()
            ToolStripProgressBar2.Value = ToolStripProgressBar2.Value + 20
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "Rkill ejecutado.")
            End If
            '   On Error Resume Next
            '   File.Move(directory + "\Rkill.txt", directory + "\logs\Rkill.txt")
        Else
            TextBox1.AppendText("No se ha podido descargar Rkill." + Environment.NewLine)
        End If
        ' FIN RKILL


        ' Iniciamos de nuevo caffeine, ya que Rkill lo cierra.
        If IsProcessRunning("caffeine") Then
        Else
            Dim lugarhacialacafeina As String
            lugarhacialacafeina = directory + "\caffeine.exe"
            Dim cafeina2 As New Process
            cafeina2.StartInfo.FileName = lugarhacialacafeina
            cafeina2.StartInfo.Arguments = "-noicon"
            On Error Resume Next
            cafeina2.Start()
        End If

        ' INICIO  KVRT
        TextBox1.AppendText("Descargando KVRT. Sea paciente. 95,6 MB" + Environment.NewLine)
        '  strURL = "http://devbuilds.kaspersky-labs.com/devbuilds/KVRT/latest/full/KVRT.exe"
        strPath = directory + "\KVRT.exe"
        ' Dim thread2 As Threading.Thread
        '  thread2 = New Threading.Thread(AddressOf espereporfavor.Show)
        ' thread2.Start()
        '    espereporfavor.Show()

        '   Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
        ' If Ret = 0 Then
        kvrtbotoniniciar.ShowDialog()

        If kvrt1 = True Then
            TextBox1.AppendText("Ejecutando KVRT. Sea paciente." + Environment.NewLine)
            Dim EMISOFT As New Process
            EMISOFT.StartInfo.FileName = strPath
            EMISOFT.StartInfo.Arguments = "-accepteula -adinsilent -silent -processlevel 2 -dontcryptsupportinfo -d " + directory
            EMISOFT.Start()
            EMISOFT.WaitForExit()
            TextBox1.AppendText("Analisis terminado. KVRT ejecutado" + Environment.NewLine)
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "KVRT OK.")
            End If
            If My.Settings.eliminarluego = True Then
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(strPath)
            End If
            Ret = Nothing
            strURL = Nothing
            strPath = Nothing
            ToolStripProgressBar2.Value = ToolStripProgressBar2.Value + 20
        Else
            TextBox1.AppendText("Error en la descarga de KVRT" + Environment.NewLine)
        End If
        'End If

        ' FIN KVRT



        ' INICIO TREND MICRO SYSCLEAN
        '   TextBox1.Text = TextBox1.Text + vbNewLine + "Descargando Trend Micro Syslean."
        '   strURL = "http://desfacha.ml/archivos/trend.zip"

        '    strPath = directory + "\trend.zip"

        '    Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)

        '     If Ret = 0 Then

        '     Dim shObj As Object = Activator.CreateInstance(Type.GetTypeFromProgID("Shell.Application"))
        'Create directory in which you will unzip your items.
        '    IO.Directory.CreateDirectory(directory + "\extraido")

        'Declare the folder where the items will be extracted.
        '  Dim output As Object = shObj.NameSpace((directory + "\extraido"))

        'Declare the input zip file.
        '  Dim input As Object = shObj.NameSpace((strPath))

        'Extract the items from the zip file.
        ' output.CopyHere((input.Items), 4)
        'Threading.Thread.Sleep(1000)
        ' Dim trend As New Process
        ' trend.StartInfo.FileName = directory + "\extraido\SysClean.com"
        '   trend.StartInfo.Arguments = "/FULLSILENT"
        '      trend.StartInfo.WorkingDirectory = directory + "\extraido"
        '  trend.StartInfo.UseShellExecute = True
        ' trend.Start()
        'trend.WaitForExit()
        ' TextBox1.Text = TextBox1.Text + vbNewLine + "Trend Micro sysclean ejecutado."
        '     If My.Settings.log = True Then
        '   WriteToLog(Date.Now, "Trend Micro sysclean OK.")
        'End If
        'If My.Settings.eliminarluego = True Then
        'On Error Resume Next
        'My.Computer.FileSystem.DeleteFile(strPath)
        '  My.Computer.FileSystem.DeleteDirectory(directory + "\extraido", FileIO.DeleteDirectoryOption.DeleteAllContents)
        '    End If

        '  Ret = Nothing
        '  strURL = Nothing
        '    strPath = Nothing
        '    output = Nothing
        '    input = Nothing
        '   trend = Nothing
        ' End If





        ' INICIO Stinger


        '  strURL = "http://downloadcenter.mcafee.com/products/mcafee-avert/stinger/stinger32.exe"
        strPath = directory + "\stinger32.exe"
        TextBox1.AppendText("Descargando Stringer. Sea paciente. 15 MB" + Environment.NewLine)

        '  Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
        '  If Ret = 0 Then
        stingerbotoniniciar.ShowDialog()

        If stinger1 = True Then
            TextBox1.AppendText("Ejecutando Stinger" + Environment.NewLine)


            Dim batstingerpath As String

            Dim iniciodelstinger As String = "start /wait stinger32.exe --GO --SILENT --PROGRAM --DELETE --REPORTPATH=" + directory + "\logs"
            batstingerpath = directory + "\stinger.bat"
            If Not File.Exists(batstingerpath) Then

                My.Computer.FileSystem.WriteAllText(batstingerpath, iniciodelstinger, False, System.Text.Encoding.Default)
                SetAttr(batstingerpath, FileAttribute.Normal)
            End If

            Dim sophos As New Process
            sophos.StartInfo.FileName = batstingerpath
            '  sophos.StartInfo.Arguments = "--GO --SILENT --PROGRAM --DELETE --REPORTPATH=" + directory + "\logs"
            sophos.StartInfo.WindowStyle = ProcessWindowStyle.Minimized
            sophos.StartInfo.CreateNoWindow = True

            sophos.Start()
            sophos.WaitForExit()

            TextBox1.AppendText("Stinger ejecutado" + Environment.NewLine)
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "Stinger ejecutado.")
            End If
            If My.Settings.eliminarluego = True Then
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(strPath)
                My.Computer.FileSystem.DeleteFile(batstingerpath)
            End If
            Ret = Nothing
            strURL = Nothing
            strPath = Nothing
            ToolStripProgressBar2.Value = ToolStripProgressBar2.Value + 10
            '  End If
        Else
            TextBox1.AppendText("Error en la descarga de stinger" + Environment.NewLine)
        End If

        ' FIN Stinger

        ' INICIO SPYBOT SEARCH AND DESTROY
        'strURL = "http://ftp.feg.unesp.br/remocao_virus/spybot/spybot-2.4.exe"
        'strPath = directory + "\spybot24.exe"
        'TextBox1.Text = TextBox1.Text + vbNewLine + "Descargando SpyBot S&D. Sea paciente. 44 MB"
        ' Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
        '  If Ret = 0 Then
        ' TextBox1.Text = TextBox1.Text + vbNewLine + "SpyBot search and destroy 2.4 descargado"
        '  Dim spybot As New Process
        '   spybot.StartInfo.FileName = strPath
        '    spybot.StartInfo.Arguments = "/verysilent /suppressmsgboxes /norestart /sp- /dir=" + "'C:\SPYBOT'"
        '     spybot.WaitForExit()
        '      Dim spybotscan As New Process

        ' End If

        ' FIN SPYBOT SEARCH AND DESTROY



        ' INICIO TDDSKILLER

        '  strURL = "http://media.kaspersky.com/utilities/VirusUtilities/en/tdsskiller.exe"
        strPath = directory + "\tdsskiller.exe"
        TextBox1.AppendText("Descargando Tddskiller de Kapersky" + Environment.NewLine)
        ' Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
        ' If Ret = 0 Then
        tddskillerbotoniniciar.ShowDialog()

        If tddskiller = True Then
            TextBox1.AppendText("Ejecutando Tddskiller" + Environment.NewLine)
            Dim tdds As New Process
            tdds.StartInfo.FileName = strPath
            tdds.StartInfo.Arguments = "-l tdds.log -silent -tdlfs -dcexact -accepteula -accepteulaksn"
            tdds.Start()
            tdds.WaitForExit()

            TextBox1.AppendText("Tddskiller ejecutado" + Environment.NewLine)
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "Tddskiller ejecutado.")
            End If
            If My.Settings.eliminarluego = True Then
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(strPath)
            End If
            Ret = Nothing
            strURL = Nothing
            strPath = Nothing
            ToolStripProgressBar2.Value = ToolStripProgressBar2.Value + 10
            ' End If
        Else
            TextBox1.AppendText("Error en la descarga de Tddskiller" + Environment.NewLine)
        End If

        '  INICIO SOPHOS
        '   strURL = "http://desfacha.ml/archivos/sophos.zip"

        strPath = directory + "\sophos.zip"

        '  Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)

        ' If Ret = 0 Then
        sophosbotoniniciar.ShowDialog()

        If sophos = True Then
            Dim shObj As Object = Activator.CreateInstance(Type.GetTypeFromProgID("Shell.Application"))
            IO.Directory.CreateDirectory(directory + "\extraido")
            Dim output As Object = shObj.NameSpace((directory + "\extraido"))
            Dim input As Object = shObj.NameSpace((strPath))
            output.CopyHere((input.Items), 4)

            Dim bat1path As String

            Dim iniciodelsophos As String = "start /wait extraido\svrtcli.exe -yes"
            bat1path = directory + "\sophos.bat"
            If Not File.Exists(bat1path) Then

                My.Computer.FileSystem.WriteAllText(bat1path, iniciodelsophos, False, System.Text.Encoding.Default)
                SetAttr(bat1path, FileAttribute.Normal)
            End If


            Dim sophosvir As New Process
            sophosvir.StartInfo.FileName = bat1path
            sophosvir.StartInfo.WorkingDirectory = directory
            sophosvir.Start()
            sophosvir.WaitForExit()
            TextBox1.AppendText("Sophos Virus Removal Tool ejecutado." + Environment.NewLine)
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "Sophos Virus Removal Tool ejecutado.")
            End If
            If My.Settings.eliminarluego = True Then
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(bat1path)
                My.Computer.FileSystem.DeleteFile(strPath)
                My.Computer.FileSystem.DeleteDirectory(directory + "\extraido", FileIO.DeleteDirectoryOption.DeleteAllContents)
            End If
            bat1path = Nothing
            directory = Nothing
            iniciodelsophos = Nothing
            Ret = Nothing
            strURL = Nothing
            strPath = Nothing
            output = Nothing
            input = Nothing
            sophosvir = Nothing
            '  End If
        Else
            TextBox1.AppendText("Error en la descarga de Sophos" + Environment.NewLine)
        End If


        ' Chequeamos la integridad del sistema operativo
        ' Solo NT6 ya que NT5 pide el cd/dvd de instalacion
        If CheckBoxsfc.Checked = False Then 'chequeamos que no este tildado el NO comprobar archivos...
            If Environment.OSVersion.Version.Major = 6 Then
                TextBox1.AppendText("Se comprobaran los archivos del sistema... Esto puede llevar tiempo." + Environment.NewLine)
                Dim sfc As New Process
                '     Dim NativeDir = IO.Path.Combine(Environment.GetEnvironmentVariable("windir"), "sysnative")
                '     Dim ExePath = IO.Path.Combine(NativeDir, "sfc.exe")
                '   sfc.StartInfo.FileName = "C:\Windows\system32\sfc.exe"
                '  sfc.StartInfo.Arguments = "/scannow"

                Dim batsfcpath As String

                Dim iniciodelsfc As String = "start /wait %windir%\system32\sfc.exe /scannow"
                batsfcpath = directory + "\sfc.bat"
                If Not File.Exists(batsfcpath) Then

                    My.Computer.FileSystem.WriteAllText(batsfcpath, iniciodelsfc, False, System.Text.Encoding.Default)
                    SetAttr(batsfcpath, FileAttribute.Normal)
                End If
                sfc.StartInfo.FileName = batsfcpath
                sfc.Start()
                sfc.WaitForExit()
                TextBox1.AppendText("Finalizada la comprobacion." + Environment.NewLine)
            End If
        End If
        ' Resteamos los permisos de archivos.
        ' DESFASADO.
        'Dim strpath2 As String
        '    strURL = "http://desfacha.ml/archivos/permisosdearchivos.bat"
        '    strpath2 = directory + "\permisosdearchivos.bat"
        '    Ret = URLDownloadToFile(0, strURL, strpath2, 0, 0)
        '    Dim strpath3 As String
        '    strURL = "http://desfacha.ml/archivos/subinacl.exe"
        '    strpath3 = directory + "\subinacl.exe"
        '    Ret = URLDownloadToFile(0, strURL, strpath3, 0, 0)

        '  If Ret = 0 Then
        '  TextBox1.AppendText("Reseteando los permisos de archivos (Nota: esto tarda)" + Environment.NewLine)
        '  Dim resetear As New Process
        ' resetear.StartInfo.FileName = strpath2
        ' resetear.Start()
        'resetear.WaitForExit()
        'On Error Resume Next
        'My.Computer.FileSystem.DeleteFile(strpath2)
        'My.Computer.FileSystem.DeleteFile(strpath3)
        ' TextBox1.AppendText("Permisos de archivos reseteados." + Environment.NewLine)
        '    End If



        espereporfavor.Close()

        If CheckBox3.CheckState = CheckState.Checked Then
            strURL = "..."
            If (GetOSArchitecture() = 64) Then
                strURL = "http://desfacha.ml/archivos/DeviceCleanupCmdx64.exe"

            ElseIf GetOSArchitecture() = 32 Then
                strURL = "http://desfacha.ml/archivos/DeviceCleanupCmd.exe"
            End If
            strPath = directory + "\DeviceCleanupCmd.exe"

            Ret = URLDownloadToFile(0, strURL, strPath, 0, 0)
            If Ret = 0 Then
                Dim usb As New Process
                usb.StartInfo.FileName = strPath
                usb.StartInfo.Arguments = "-n"
                usb.Start()
                usb.WaitForExit()
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(strPath)
                TextBox1.AppendText("Eliminados dipositivos USB no conectados." + Environment.NewLine)
            End If
            WriteToLog(Date.Now, "Eliminados dipositivos USB no conectados.")
        End If
        ' INICIO ADWCLEANER
        If CheckBox2.Checked = False Then
            ' Antiguo link adwcleaner
            ' strURL = "http://195.122.253.115/ftp/MInstAll%20v.24.01.2016%20By%20Andreyonohov%20%26%20Leha342/MInstAll/Portable/AdwCleaner/adwcleaner_5.030.exe"
            ' strURL = "https://fossies.org/windows/misc/adwcleaner_5.119.exe"
            Dim pProcess4() As Process = System.Diagnostics.Process.GetProcessesByName("caffeine")

            For Each p As Process In pProcess4
                p.Kill()
            Next
            Dim eliminarcafeina As String = directory + "\caffeine.exe"
            On Error Resume Next
            My.Computer.FileSystem.DeleteFile(eliminarcafeina)

            strURL = "http://desfacha.ml/archivos/adwcleaner.exe"
            strPath = directory + "\adwcleaner.exe"

            Dim adw As Net.WebClient = New Net.WebClient
            adw.DownloadFile(strURL, strPath)
            TextBox1.AppendText("AdwCleaner descargado." + Environment.NewLine)

            Dim adwcleaner As New Process
            adwcleaner.StartInfo.FileName = strPath
            adwcleaner.Start()
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "ADWCLEANER EJECUTADO.")
            End If
            instruccionesadwcleaner.Show()
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "Proceso finalizado correctamente.")
            End If
            If CheckBoxlogs.Checked = True Then
                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(directory + "\logs\" + Trim(hdserial) + ".log")
            End If
        Else
            If My.Settings.log = True Then
                WriteToLog(Date.Now, "APAGANDO EL EQUIPO. Proceso finalizado correctamente.")
            End If
            If CheckBoxlogs.Checked = True Then

                On Error Resume Next
                My.Computer.FileSystem.DeleteFile(directory + "\logs\" + Trim(hdserial) + ".log")
            End If
            Process.Start("shutdown.exe", " -s -t 10 -f -c ""DesFacha ha programado un apagado automatico en 10 segundos""")


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

    Private Sub ModoDeEmergenciaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ModoDeEmergenciaToolStripMenuItem.Click

        WriteToLog(Date.Now, "Iniciando modo de emergencia...")
        Dim llamaraemergencia As New emergencia
        llamaraemergencia.ShowDialog()

    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        'rs.ResizeAllControls(Me)
    End Sub

    Private Sub AyudaDeDesfachaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AyudaDeDesfachaToolStripMenuItem.Click
        ayuda.Show()
    End Sub

    Private Sub AsistenciaPasoAPasoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AsistenciaPasoAPasoToolStripMenuItem.Click
        asistente.ShowDialog()
    End Sub

    Public Shared Function Antivirusinstalado() As Boolean

        Dim wmipathstr As String = "\\" + Environment.MachineName + "\root\SecurityCenter2"
        Try
            Dim searcher As New ManagementObjectSearcher(wmipathstr, "SELECT * FROM AntivirusProduct")
            Dim instances As ManagementObjectCollection = searcher.[Get]()
            Return instances.Count > 0

        Catch e As Exception
            Console.WriteLine(e.Message)
        End Try

        Return False
    End Function
    Public Shared Function Antivirusinstaladoenxp() As Boolean

        Dim wmipathstr As String = "\\" + Environment.MachineName + "\root\SecurityCenter"
        Try
            Dim searcher As New ManagementObjectSearcher(wmipathstr, "SELECT * FROM AntivirusProduct")
            Dim instances As ManagementObjectCollection = searcher.[Get]()
            Return instances.Count > 0

        Catch e As Exception
            Console.WriteLine(e.Message)
        End Try

        Return False
    End Function

    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Dim directory As String = My.Application.Info.DirectoryPath
        On Error Resume Next
        My.Computer.FileSystem.DeleteDirectory(directory + "\extraido2", FileIO.DeleteDirectoryOption.DeleteAllContents)
        My.Computer.FileSystem.DeleteDirectory(directory + "\crystal", FileIO.DeleteDirectoryOption.DeleteAllContents)
        My.Computer.FileSystem.DeleteFile(directory + "\runtime.dat")
        My.Computer.FileSystem.DeleteFile(directory + "\caffeine.exe")
        My.Computer.FileSystem.DeleteDirectory(directory + "\Legal notices", FileIO.DeleteDirectoryOption.DeleteAllContents)
    End Sub
End Class
