Imports System.IO

Public Class asistente
    Dim rs As New Resizer
    Private Declare Function URLDownloadToFile Lib "urlmon" _
    Alias "URLDownloadToFileA" (ByVal pCaller As Integer,
    ByVal szURL As String, ByVal szFileName As String,
    ByVal dwReserved As Integer, ByVal lpfnCB As Integer) As Integer
    Dim hdserial As String
    Private Sub asistente_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TabControl1.Appearance = TabAppearance.FlatButtons
        TabControl1.ItemSize = New Size(0, 1)
        TabControl1.SizeMode = TabSizeMode.Fixed
        rs.FindAllControls(Me)
    End Sub

    Private Sub asistente_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        rs.ResizeAllControls(Me)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If RadioButton1.Checked = True Then
            TabControl1.SelectTab(TabPageopcion1)
        End If
        If RadioButton2.Checked = True Then
            TabControl1.SelectTab(TabPageopcion2)
        End If
        If RadioButton3.Checked = True Then

            MsgBox("Se iniciara el proceso limpieza general. Sea paciente.", MsgBoxStyle.OkOnly, "Informacion")
            Me.Hide()
            Form1.Button3_Click(Me, EventArgs.Empty)
            Me.Close()

        End If
        If RadioButton4.Checked = True Then
            TabControl1.SelectTab(TabPageopcion4)
        End If
        If RadioButton5.Checked = True Then
            MsgBox("Se iniciara el proceso de instalacion de drivers. Sea paciente.", MsgBoxStyle.OkOnly, "Informacion")
            aplicaciones.Button13_Click(Me, EventArgs.Empty)
            Me.Close()
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        TabControl1.SelectTab(TabPage1)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If ropcion1opcion1.Checked = True Then
            MsgBox("Se iniciara el proceso iniciar del desfacha. Sea paciente.", MsgBoxStyle.OkOnly, "Informacion")
            Me.Hide()
            Form1.Button1_Click(Me, EventArgs.Empty)
            TabControl1.SelectTab(TabPage1)
            Me.Close()
        End If
        If ropcion1opcion2.Checked = True Then
            MsgBox("Se iniciara el proceso iniciar del desfacha. Sea paciente.", MsgBoxStyle.OkOnly, "Informacion")
            Me.Hide()
            Form1.Button1_Click(Me, EventArgs.Empty)
            TabControl1.SelectTab(TabPage1)
            Me.Close()
        End If
        If ropcion1opcion3.Checked = True Then
            MsgBox("Se iniciara el proceso iniciar del desfacha. Sea paciente.", MsgBoxStyle.OkOnly, "Informacion")
            Me.Hide()
            Form1.Button1_Click(Me, EventArgs.Empty)
            TabControl1.SelectTab(TabPage1)
            Me.Close()
        End If
        If ropcion1opcion4.Checked = True Then
            MsgBox("Debe instalar un bloqueador de anuncios para el navegador. Como Adblock, Ublock, etc.", MsgBoxStyle.OkOnly, "Informacion")
            TabControl1.SelectTab(TabPage1)
            Me.Close()
        End If

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        TabControl1.SelectTab(TabPage1)
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If ropcion2opcion1.Checked = True Then
            MsgBox("Se iniciara el proceso limpieza general del desfacha. Sea paciente.", MsgBoxStyle.OkOnly, "Informacion")
            Me.Hide()
            Form1.Button3_Click(Me, EventArgs.Empty)
            TabControl1.SelectTab(TabPage1)
            Me.Close()
        End If
        If ropcion2opcion2.Checked = True Then
            MsgBox("Se iniciara el proceso iniciar del desfacha. Sea paciente.", MsgBoxStyle.OkOnly, "Informacion")
            Me.Hide()
            Form1.Button1_Click(Me, EventArgs.Empty)
            TabControl1.SelectTab(TabPage1)
            Me.Close()
        End If
        If ropcion2opcion3.Checked = True Then
            MsgBox("Se iniciara el proceso iniciar del desfacha. Sea paciente.", MsgBoxStyle.OkOnly, "Informacion")
            Me.Hide()
            Form1.Button1_Click(Me, EventArgs.Empty)
            TabControl1.SelectTab(TabPage1)
            Me.Close()
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        TabControl1.SelectTab(TabPage1)
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        If ropcion4opcion1.Checked = True Then
            MsgBox("Se iniciara el proceso de instalacion de archivos necesarios. Sea paciente.", MsgBoxStyle.OkOnly, "Informacion")
            aplicaciones.Button20_Click(Me, EventArgs.Empty)
            TabControl1.SelectTab(TabPage1)
            Me.Close()
        End If
        If ropcion4opcion2.Checked = True Then
            MsgBox("Se iniciara el proceso de instalacion de archivos necesarios. Sea paciente.", MsgBoxStyle.OkOnly, "Informacion")
            aplicaciones.Button20.PerformClick()
            TabControl1.SelectTab(TabPage1)
            Me.Close()
        End If
        If ropcion4opcion3.Checked = True Then
            MsgBox("Se iniciara el programa para actualizar drivers. Sea paciente.", MsgBoxStyle.OkOnly, "Informacion")

            aplicaciones.Button13_Click(Me, EventArgs.Empty)
            TabControl1.SelectTab(TabPage1)
            Me.Close()
        End If
    End Sub
    Private Function GetOSArchitecture() As Integer
        Dim pa As String = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE")
        Return (If(([String].IsNullOrEmpty(pa) OrElse [String].Compare(pa, 0, "x86", 0, 3, True) = 0), 32, 64))
    End Function
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
End Class