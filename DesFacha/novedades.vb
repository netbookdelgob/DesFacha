Imports System.Runtime.InteropServices

Public Class novedades
    Private Const WM_VSCROLL As Int32 = &H115
    Private Const SB_BOTTOM As Int32 = 7
    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
    End Function
    Dim rs As New Resizer
    Private Sub novedades_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Esto hace que automaticamente vaya a la ultima linea en el evento load, es decir sin que
        '  el textbox tenga focus
        SendMessage(Me.TextBox3.Handle, WM_VSCROLL, SB_BOTTOM, 0)
        rs.FindAllControls(Me)
    End Sub

    Private Sub novedades_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        rs.ResizeAllControls(Me)
    End Sub
End Class