Public Class opciones
    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs)
        If CheckBox1.CheckState = CheckState.Checked Then
            My.Settings.log = True
            My.Settings.Save()
        Else
            My.Settings.log = False
            My.Settings.Save()
        End If
    End Sub
    Dim rs As New Resizer
    Private Sub CheckBox1_CheckStateChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub opciones_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        rs.FindAllControls(Me)
        My.Settings.Reload()

        If My.Settings.log = False Then
            CheckBox1.Checked = False
            CheckBox1.CheckState = CheckState.Unchecked
        Else
            CheckBox1.Checked = True
            CheckBox1.CheckState = CheckState.Checked
        End If
        If My.Settings.stinger = False Then
            CheckBox2.Checked = False
            CheckBox2.CheckState = CheckState.Unchecked
        Else
            CheckBox2.Checked = True
            CheckBox2.CheckState = CheckState.Checked
        End If
        If My.Settings.apagar = False Then
            CheckBox3.Checked = False
            CheckBox3.CheckState = CheckState.Unchecked
        Else
            CheckBox3.Checked = True
            CheckBox3.CheckState = CheckState.Checked
        End If
        If My.Settings.eliminarluego = False Then
            CheckBox4.Checked = False
            CheckBox4.CheckState = CheckState.Unchecked
        Else
            CheckBox4.Checked = True
            CheckBox4.CheckState = CheckState.Checked

        End If
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs)
        If CheckBox2.CheckState = CheckState.Checked Then
            My.Settings.stinger = True
            My.Settings.Save()
        Else
            My.Settings.stinger = False
            My.Settings.Save()
        End If
    End Sub

    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs)
        If CheckBox3.CheckState = CheckState.Checked Then
            My.Settings.apagar = True
            My.Settings.Save()
        Else
            My.Settings.apagar = False
            My.Settings.Save()
        End If
    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs)
        If CheckBox4.CheckState = CheckState.Checked Then
            My.Settings.eliminarluego = True
            My.Settings.Save()
        Else
            My.Settings.eliminarluego = False
            My.Settings.Save()
        End If
    End Sub

    Private Sub opciones_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        rs.ResizeAllControls(Me)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim directory As String = My.Application.Info.DirectoryPath
        On Error Resume Next
        My.Computer.FileSystem.DeleteDirectory(directory + "\logs", FileIO.DeleteDirectoryOption.DeleteAllContents)
    End Sub
End Class