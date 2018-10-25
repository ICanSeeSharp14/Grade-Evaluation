Module Module1
    Public Enum AnimateWindowFlags
        AW_HOR_POSITIVE = &H1
        AW_HOR_NEGATIVE = &H2
        AW_VER_POSITIVE = &H4
        AW_VER_NEGATIVE = &H8
        AW_CENTER = &H10
        AW_HIDE = &H10000
        AW_ACTIVATE = &H20000
        AW_SLIDE = &H40000
        AW_BLEND = &H80000
    End Enum

    Dim f1 As SearchStudent
    ' Dim f2 As frmGradeEntry
    Public Declare Auto Function AnimateWindow Lib "user32" (ByVal hwnd As IntPtr, ByVal time As Integer, ByVal flags As AnimateWindowFlags) As Boolean

    Sub Main()
        f1 = New SearchStudent
        '   f2 = New frmGradeEntry
        Dim xx As Integer = Screen.PrimaryScreen.Bounds.Width - f1.Size.Width
        Dim yy As Integer = Screen.PrimaryScreen.Bounds.Height / 2 - (f1.Size.Height / 2)
        f1.Location = New Point(xx, yy)
        'Dim xxx As Integer = Screen.PrimaryScreen.Bounds.Width - f2.Size.Width
        'Dim yyy As Integer = Screen.PrimaryScreen.Bounds.Height / 2 - (f2.Size.Height / 2)
        'f2.Location = New Point(xxx, yyy)
        'Application.Run(f1)
        Application.Run(f1)
    End Sub

    Sub animateWin(ByVal frmToAnimate As Form, ByVal showForm As Boolean)
        If showForm Then
            AnimateWindow(frmToAnimate.Handle, 1000, AnimateWindowFlags.AW_VER_POSITIVE Or AnimateWindowFlags.AW_SLIDE)
        Else
            AnimateWindow(frmToAnimate.Handle, 1000, AnimateWindowFlags.AW_VER_NEGATIVE Or AnimateWindowFlags.AW_HIDE)
        End If

    End Sub
End Module
