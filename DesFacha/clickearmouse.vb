Module clickearmouse


    Public Declare Sub mouse_event Lib "user32" (ByVal dwFlags As Integer, ByVal dx As Integer, ByVal dy As Integer, ByVal cButtons As Integer, ByVal dwExtraInfo As Integer)

    Public Declare Function SetCursorPos Lib "user32" (ByVal x As Integer, ByVal y As Integer) As Integer
    Public Declare Function GetCursorPos Lib "user32" (ByVal lpPoint As POINTAPI) As Integer
    Public Const MOUSEEVENTF_LEFTDOWN = &H2
    Public Const MOUSEEVENTF_LEFTUP = &H4
    Public Const MOUSEEVENTF_MIDDLEDOWN = &H20
    Public Const MOUSEEVENTF_MIDDLEUP = &H40
    Public Const MOUSEEVENTF_RIGHTDOWN = &H8
    Public Const MOUSEEVENTF_RIGHTUP = &H10
    Public Const MOUSEEVENTF_MOVE = &H1
    Public Structure POINTAPI
        Dim x As Integer
        Dim y As Integer
    End Structure
    Public Function GetX() As Integer
        Dim n As POINTAPI
        GetCursorPos(n)
        GetX = n.x
    End Function
    Public Function GetY() As Integer
        Dim n As POINTAPI
        GetCursorPos(n)
        GetY = n.y

    End Function
    Public Sub LeftClick()
        LeftDown()
        LeftUp()
    End Sub
    Public Sub LeftDown()
        mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0)
    End Sub
    Public Sub LeftUp()
        mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0)
    End Sub
    Public Sub MiddleClick()
        MiddleDown()
        MiddleUp()
    End Sub
    Public Sub MiddleDown()
        mouse_event(MOUSEEVENTF_MIDDLEDOWN, 0, 0, 0, 0)
    End Sub
    Public Sub MiddleUp()
        mouse_event(MOUSEEVENTF_MIDDLEUP, 0, 0, 0, 0)
    End Sub
    Public Sub RightClick()
        RightDown()
        RightUp()
    End Sub
    Public Sub RightDown()
        mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0)
    End Sub
    Public Sub RightUp()
        mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0)
    End Sub
    Public Sub MoveMouse(ByVal xMove As Integer, ByVal yMove As Integer)
        mouse_event(MOUSEEVENTF_MOVE, xMove, yMove, 0, 0)
    End Sub
    Public Sub SetMousePos(ByVal xPos As Integer, ByVal yPos As Integer)
        SetCursorPos(xPos, yPos)
    End Sub

End Module
