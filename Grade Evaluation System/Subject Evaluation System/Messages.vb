Public Class Messages

    Private Sub Messages_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnOkay_Click(sender, e)
        End If
    End Sub

    Private Sub Messages_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.btnOkay.Focus()
    End Sub

    
    Private Sub Label3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Hide()
    End Sub

    Private Sub btnOkay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOkay.Click
        If Me.lblMessage.Text = "Access Granted" Then
            FrmLogin.Hide()
            Me.Close()
            FrmMain.Show()
        ElseIf lblMessage.Text = "Enter Administrator Password" Then
            AdminPassword.txtPassword.Focus()
            Me.Close()
        ElseIf lblMessage.Text = "Program will close. Continue?." Then
            Application.Exit()
        Else
            Me.Close()
        End If

    End Sub
End Class