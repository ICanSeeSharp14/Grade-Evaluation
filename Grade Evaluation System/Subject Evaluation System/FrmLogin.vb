Imports System.Drawing
Public Class FrmLogin
    Dim username As String
    Dim password As String
    Private TstServerMySQL As String
    Private TstPortMySQL As String
    Private TstUserNameMySQL As String
    Private TstPwdMySQL As String
    Private TstDBNameMySQL As String

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
       
        getData()
        ConnDB()

        If Me.txtUsername.Text = "" Then

            Me.lblMessage.Text = "Enter your Username!"
            Me.txtUsername.Focus()

            Exit Sub

        ElseIf Me.txtPassword.Text.Trim.Length = 0 Then
            Me.lblMessage.Text = "Enter your Password!"
            'Messages.lblMessage.Text = "Enter your Password"
            'Messages.btnOkay.Focus()
            Me.txtPassword.Focus()
            'Messages.ShowDialog()
            Exit Sub
        Else
            lblMessage.Text = ""
            dt = SQLRetrieve("SELECT * FROM tbladmin WHERE username = '" & Me.txtUsername.Text & "' and password = '" & Me.txtPassword.Text & "'")
            If dt.Rows.Count > 0 Then
                username = dt.Rows(0).Item(1).ToString
                password = dt.Rows(0).Item(2).ToString
                strUsername = username
                Dim usertype As String = dt.Rows(0).Item("usertype").ToString
                If username = txtUsername.Text And password = txtPassword.Text Then
                    'Trial.Show()
                    'Exit Sub
                    With FrmMain
                        .lblbUserType.Text = "Welcome " & dt.Rows(0).Item("Usertype").ToString
                        .lblNameofUser.Text = "Hi! " & dt.Rows(0).Item("Name").ToString
                    End With
                    If usertype = "Administrator" Then
                        With FrmMain
                            .btnUserAccount.Visible = True
                            .btnCurriculum.Visible = True
                            .btnPrereq.Visible = True
                        End With
                    Else

                        FrmMain.btnUserAccount.Visible = False
                        FrmMain.btnCurriculum.Visible = False
                        FrmMain.btnPrereq.Visible = False
                    End If
                    Me.txtPassword.Clear()
                    Me.txtUsername.Clear()
                    Messages.PictureBox6.Image = My.Resources.Info_Squared_50px
                    Messages.lblTitle.Text = "Login Success"
                    Messages.lblMessage.Text = "Access Granted"
                    Messages.ShowDialog()
                    Me.Hide()
                    Exit Sub
                Else
                    Messages.PictureBox6.Image = My.Resources.Error_50px
                    Messages.lblTitle.Text = "Case Sensitive"
                    lblMessage.Text = "Invalid Username/Password"

                    Me.txtUsername.Focus()
                    Me.txtPassword.Clear()

                    '  Messages.ShowDialog()

                    Exit Sub
                End If
            Else
                Messages.PictureBox6.Image = My.Resources.Close_Window_50px
                Messages.lblTitle.Text = "Invalid"
                lblMessage.Text = "Incorrect Username or Password"
                Me.txtUsername.Focus()
                Me.txtPassword.Clear()
               
              
            End If
        End If

    End Sub

    Private Sub FrmLogin_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        getData()
        lblMessage.Text = ""
        ConnDB()
        Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) / 2
        Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) / 2
    End Sub

    Private Sub Label3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Application.Exit()
    End Sub

    Private Sub Label4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub FrmLogin_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyData = Keys.F12 Then
            DBConfig.Show()
        ElseIf e.KeyData = Keys.F11 Then
            ImportDB.ShowDialog()
        ElseIf e.KeyData = Keys.Enter Then
            Button4.PerformClick()
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Application.Exit()
    End Sub

    Private Sub txtUsername_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles _
        txtUsername.KeyDown, txtPassword.KeyDown
       
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub RectangleShape1_Click(sender As Object, e As EventArgs) Handles RectangleShape1.Click, RectangleShape2.Click, RectangleShape3.Click

    End Sub

    Private Sub txtUsername_LostFocus(sender As Object, e As EventArgs) Handles txtUsername.LostFocus
        'Me.txtUsername.Multiline = True
        'Me.txtPassword.Multiline = True
    End Sub

    Private Sub txtUsername_TextChanged(sender As Object, e As EventArgs) Handles txtUsername.TextChanged
        If Me.txtUsername.Text.Length > 0 Then
            Me.Label21.Visible = False
        Else
            Me.Label21.Visible = True
            Me.txtUsername.TextAlign = HorizontalAlignment.Left

        End If
    End Sub

    Private Sub txtPassword_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPassword.KeyPress, txtUsername.KeyPress
        Select Case Asc(e.KeyChar)
            Case 13
                e.Handled = True
        End Select
    End Sub

    Private Sub txtPassword_LostFocus(sender As Object, e As EventArgs) Handles txtPassword.LostFocus
        'Me.txtUsername.Multiline = True
        'Me.txtPassword.Multiline = True
    End Sub

    Private Sub txtPassword_TextChanged(sender As Object, e As EventArgs) Handles txtPassword.TextChanged
        If Me.txtPassword.Text.Length > 0 Then
          
            Me.Label3.Visible = False
        Else
            Me.txtPassword.TextAlign = HorizontalAlignment.Left
            Me.Label3.Visible = True

        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub Button2_MouseHover(sender As Object, e As EventArgs) Handles Button2.MouseHover
        Me.Button2.ForeColor = Color.White
    End Sub

    Private Sub Button1_MouseHover(sender As Object, e As EventArgs) Handles Button1.MouseHover
        Me.Button1.ForeColor = Color.White
    End Sub

    Private Sub Button1_MouseLeave(sender As Object, e As EventArgs) Handles Button1.MouseLeave
        Me.Button1.ForeColor = Color.LightGray
    End Sub

    Private Sub Button2_MouseLeave(sender As Object, e As EventArgs) Handles Button2.MouseLeave
        Me.Button2.ForeColor = Color.LightGray
    End Sub

    Private Sub Label3_Click_1(sender As Object, e As EventArgs) Handles Label3.Click
        txtPassword.Focus()
    End Sub

    Private Sub Label21_Click(sender As Object, e As EventArgs) Handles Label21.Click
        Me.txtUsername.Focus()
    End Sub

End Class
