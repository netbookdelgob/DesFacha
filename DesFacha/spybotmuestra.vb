Imports System.ComponentModel

Public Class spybotmuestra
    Private timer As New System.Windows.Forms.Timer()
    Private Sub spybotmuestra_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        With Me
            FormBorderStyle = FormBorderStyle.None
            TopMost = True
            WindowState = FormWindowState.Maximized
        End With
        PictureBox1.Size = New Point(Me.Width, Me.Height)
        PictureBox1.ImageLocation = "http://www.softzone.es/app/uploads/2008/04/Spybot-search-and-destroy-tutorial-foto-1-655x488.png"
        PictureBox1.Load()


    End Sub
    Dim myTimer As New Timer()
    Private Sub myTimer_Tick(sender As System.Object, e As System.EventArgs)
        myTimer.Stop()
        Me.Close()
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub

    Private Sub PictureBox1_LoadCompleted(sender As Object, e As AsyncCompletedEventArgs) Handles PictureBox1.LoadCompleted
        myTimer.Interval = 5000
        AddHandler myTimer.Tick, AddressOf myTimer_Tick
        myTimer.Start()
    End Sub
End Class