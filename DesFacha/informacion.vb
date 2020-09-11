Option Explicit On
Imports System.ComponentModel
Imports System.IO
Imports System.Management
Imports System.Net.NetworkInformation

Public Class informacion
    Dim rs As New Resizer
    Dim arquitectura As String
    Private Sub informacion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim versionos = Environment.OSVersion.Version.Major
            Select Case versionos
                Case 5
                    MsgBox("Atención: La informacion de la placa grafica podria no mostrarse correctamente en Windows 2000/XP/2003")
            End Select
            If (GetOSArchitecture() = 64) Then
                arquitectura = " (64 Bits)"

            ElseIf GetOSArchitecture() = 32 Then
                arquitectura = " (32 Bits)"
            End If
            Me.Text = My.Computer.Info.OSFullName & arquitectura
            Dim drive As New DriveInfo("C") ' Puede traer problemas con esas putas configuraciones mal hechas
            ' En las que no se reconoce como C a la unidad del sistema. (Lo he hecho, pero bueno, no se hace xd)
            Dim percentFree As Double = 100 * CDbl(drive.TotalFreeSpace) / drive.TotalSize

            ProgressBar1.Value = CInt(percentFree)

            Dim nombreduro As String = drive.VolumeLabel
            Dim sistemadearchivo As String = drive.DriveFormat
            Labelnombre.Text = nombreduro
            Labelformato.Text = sistemadearchivo
            '  Labeltotal.Text = "Usado de " & (drive.TotalSize.ToString / 1024 / 1024 / 1024) & "GB"
            Labeltotal.Text = "usado de " & Math.Round(drive.TotalSize.ToString / 1024 / 1024 / 1024) & "GB"
            Label10.Text = Math.Round(drive.TotalFreeSpace / 1024 / 1024 / 1024, 1) & " GB"
            Dim query As New System.Management.SelectQuery("Win32_VideoController")
            Dim search As New System.Management.ManagementObjectSearcher(query)
            Dim info As System.Management.ManagementObject
            For Each info In search.Get()
                Labelplacagrafica.Text = (info("Caption").ToString)
                Labelvram.Text = (info("AdapterRAM").ToString / 1024 / 1024) & " MB"
            Next
            Try
                Dim mos As New ManagementObjectSearcher("root\CIMV2",
                                        "SELECT * FROM Win32_BaseBoard")
                For Each mo As ManagementObject In mos.Get()
                    Try
                        Labelmanu.Text = mo.GetPropertyValue("Manufacturer").ToString
                        Labelmadre.Text = mo.GetPropertyValue("Product").ToString
                    Catch ex As Exception
                        Continue For
                    End Try
                Next
            Catch


            End Try
            Labelram.Text = Math.Round((My.Computer.Info.TotalPhysicalMemory.ToString / 1024 / 1024 / 1024), 3) & "GB usables"


            getinterfaces()
            Dim objSearcher As New System.Management.ManagementObjectSearcher("SELECT * FROM Win32_SoundDevice")

            Dim objCollection As System.Management.ManagementObjectCollection = objSearcher.Get()

            For Each obj As System.Management.ManagementObject In objCollection

                ListBox2.Items.Add(obj.GetPropertyValue("Caption").ToString())

            Next
            Dim moReturn As Management.ManagementObjectCollection

            Dim moSearch As Management.ManagementObjectSearcher

            Dim mo2 As Management.ManagementObject

            moSearch = New Management.ManagementObjectSearcher("Select * from Win32_Processor")

            moReturn = moSearch.Get

            For Each mo2 In moReturn

                Dim strout As String = String.Format("{0} - {1}", mo2("Name"), mo2("CurrentClockSpeed"))

                Labelproce.Text = (strout) & "Mhz"

            Next
            moSearch = Nothing
            moReturn = Nothing
            mo2 = Nothing
            mo2 = Nothing
            objSearcher = Nothing
            objCollection = Nothing
            percentFree = Nothing
            nombreduro = Nothing
            sistemadearchivo = Nothing
            query = Nothing
            search = Nothing
            info = Nothing
        Catch ex As Exception
            MsgBox("Error : " & ex.ToString)
        End Try


        Try
            Dim k As System.Management.ManagementObject
            'Dim search_Memory As New System.Management.ManagementObjectSearcher("Select * from CIM_Memory")
            Dim search_Memory As New System.Management.ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory")


            For Each k In search_Memory.Get()
                Dim totaltotalderam As String = "(" & k("Capacity").ToString / 1024 / 1024 / 1024 & "GB totales)"
                ' No usado.     TextBoxX1.Text = k("BankLabel").ToString     'numero de slot del primer banco de ram
                Label11.Text = totaltotalderam.ToString   'Capacidad del primer modulo de ram
                ' No usado.  TextBoxX3.Text = k("PartNumber").ToString     ' Nombre de parte de un solo modulo de ram

            Next

        Catch ex As Exception
            MessageBox.Show("No se pudo cargar cierta informacion sobre RAM", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        '   Dim mosDisks As New ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive")
        '   Dim modelo As String
        '    For Each moDisk As ManagementObject In mosDisks.[Get]()

        '   modelo = moDisk("Model").ToString()
        '    Label12.Text = modelo
        '    Next
        Label14.Text = DetectSsd.ssdono.HasNominalMediaRotationRate()
        Label12.Text = My.Computer.Name

        '   posicionX = Me.Bounds.X
        '  posicionY = Me.Bounds.Y
        posdelform1 = Me.Width

        '   Dim latencia2 As String = GetPingMs("www.google.com").ToString + " MS"
        '  Label16.Text = Trim(latencia2)

        Dim latencia2 As String = GetPingMs("www.google.com").ToString + " MS"
        Label16.Text = Trim(latencia2)


        rs.FindAllControls(Me)
    End Sub
    Private Sub getinterfaces()
        Dim nics As NetworkInterface() = NetworkInterface.GetAllNetworkInterfaces
        If nics.Length < 0 Or nics Is Nothing Then
            MsgBox("No hay adaptadores de red.")
            Exit Sub
        End If
        ListBox1.Items.Clear()

        For Each netadapter As NetworkInterface In nics
            Dim intproperties As IPInterfaceProperties = netadapter.GetIPProperties()
            ListBox1.Items.Add(netadapter.Description)


        Next
    End Sub
    Public Shared Function GetPingMs(ByRef hostNameOrAddress As String)
        Dim ping As New System.Net.NetworkInformation.Ping
        Return ping.Send(hostNameOrAddress).RoundtripTime
    End Function
    Private Sub GroupBox3_Enter(sender As Object, e As EventArgs) Handles GroupBox3.Enter

    End Sub
    Private Function GetOSArchitecture() As Integer
        Dim pa As String = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE")
        Return (If(([String].IsNullOrEmpty(pa) OrElse [String].Compare(pa, 0, "x86", 0, 3, True) = 0), 32, 64))
    End Function

    Private Sub informacion_Closed(sender As Object, e As EventArgs) Handles Me.Closed

    End Sub

    Private Sub informacion_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        ListBox1.ResetText()
        ListBox1.Items.Clear()
        ListBox2.Items.Clear()
        infodeldisco.Close()
    End Sub

    Private Sub informacion_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        rs.ResizeAllControls(Me)
    End Sub

    Private Sub informacion_LocationChanged(sender As Object, e As EventArgs) Handles Me.LocationChanged
        infodeldisco.Location = New Point(Me.Location.X + Me.Width + 5, Me.Location.Y)

    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        infodeldisco.Show()
        infodeldisco.Location = New Point(Me.Location.X + Me.Width + 5, Me.Location.Y)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim latencia As String = GetPingMs("www.google.com").ToString + " MS"
        Label16.Text = Trim(latencia)
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        medidor.ShowDialog()
    End Sub
End Class