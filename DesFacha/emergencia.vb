Public Class emergencia

    'https://msdn.microsoft.com/library/windows/desktop/ms724832.aspx
    'Lista de versiones de Windows

    Private Sub emergencia_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim versionosGRANDE As String = Environment.OSVersion.Version.Major
        Dim versionosCHICA As String = Environment.OSVersion.Version.Minor
        Dim versionfinal As String = versionosGRANDE & "." & versionosCHICA
        Dim sistemaoperativo As String
        Select Case versionfinal
            Case "6.2"
                If My.Computer.Info.OSFullName.Contains("10") Then
                    'MsgBox("Windows 10")
                    sistemaoperativo = "10"
                Else
                    'MsgBox("Windows 8")
                    sistemaoperativo = "8"
                End If
            Case "6.1"
                sistemaoperativo = "7"
            Case "6.0"
                sistemaoperativo = "Vista"
            Case Else
                MsgBox("Sistema operativo NO soportado.", MsgBoxStyle.Critical, "Desfacha")
        End Select



    End Sub
End Class