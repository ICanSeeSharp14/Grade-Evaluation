Public Class Loading
    Dim val As Integer = 0
    Dim x As Integer = 52
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        x += 6
        Me.progressbar.Width = Me.progressbar.Width + 7
        '  Me.lblPercent.Location = New Point(x, 265)
        If Me.lblLoading.Text = "•" Then
            Me.lblLoading.Text = "• •"
        ElseIf Me.lblLoading.Text = "• •" Then
            Me.lblLoading.Text = "• • •"
        ElseIf Me.lblLoading.Text = "• • •" Then
            Me.lblLoading.Text = "• • • •"
        ElseIf Me.lblLoading.Text = "• • • •" Then
            Me.lblLoading.Text = "•"
        End If
        val += 1
        'If progressbar.Width <= 52 Then
        '    Me.lblPercent.BackColor = System.Drawing.Color.FromArgb(27, 184, 155)
        'End If
        Me.lblPercent.Text = val & "%"
        If progressbar.Width >= 707 Then
            Timer1.Stop()
            Me.Dispose()

            FrmMain.Show()
            Me.progressbar.Visible = False
            Me.RectangleShape4.Visible = False
        End If

    End Sub

    Private Sub Loading_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If ModifierKeys = Keys.Alt And e.KeyCode = Keys.F4 Then
            e.Handled = True
          
        End If
    End Sub

    Private Sub Loading_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.progressbar.Width = 0
        ' Me.lblPercent.BackColor = Color.Transparent
        Me.Timer1.Start()
        Me.progressbar.Visible = True
        Me.RectangleShape4.Visible = True

    End Sub
End Class