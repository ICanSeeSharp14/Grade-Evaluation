Public Class AdminPassword

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)




    End Sub

    Private Sub txtPassword_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPassword.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnOkay_Click(sender, e)
        End If
    End Sub

    Private Sub txtPassword_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPassword.TextChanged
        If Me.txtPassword.TextLength > 0 Then
            Me.Label47.Visible = False
        Else
            Me.Label47.Visible = True

        End If
    End Sub

    Private Sub Label3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Button1_Click(sender, e)
    End Sub

    Private Sub Label47_Click(sender As Object, e As EventArgs) Handles Label47.Click
        Me.txtPassword.Focus()
    End Sub

    Private Sub btnOkay_Click(sender As Object, e As EventArgs) Handles btnOkay.Click
        sqlQuery = "SELECT password from tbladmin WHERE password = '" & Me.txtPassword.Text.Replace("'", "/'") & _
          "' and usertype = 'Administrator' and isActive = 1"
        dt = SQLRetrieve(sqlQuery)
        If dt.Rows.Count > 0 Then
            StrAdminPass = dt.Rows(0).Item(0).ToString
        End If
        If Me.txtPassword.Text = "" Then

            Messages.PictureBox6.Image = My.Resources.Info_Squared_50px
            Messages.lblMessage.Text = "Enter Administrator Password"
            Messages.lblTitle.Text = "Password"
            Messages.btnOkay.Focus()
            Messages.Show()

            '    MsgBox("Enter Administrator Password", vbCritical)
            Exit Sub
        Else
            If FrmMain.DGVGrades.RowCount = 0 Then
                Exit Sub
            End If
            If Me.txtPassword.Text = StrAdminPass Then
                For x = 0 To FrmMain.DGVGrades.RowCount - 1

                    sqlQuery = "UPDATE tblensub SET grade = '" & FrmMain.DGVGrades(3, x).Value.ToString & _
                              "' WHERE ensubid = '" & FrmMain.DGVGrades(0, x).Value.ToString & "'"
                    SQLExecute(sqlQuery)

                Next
                Me.txtPassword.Clear()
                If result > 0 Then
                    MsgBox("Grades successfully saved", vbInformation)
                    FrmMain.SubjectGrade()
                    Me.Close()
                    count = 0
                Else

                    MsgBox("no changes in grades of " & FrmMain.txtGradeFname.Text & " " & FrmMain.txtGradeLName.Text, vbInformation)
                    count = 0
                    Me.Close()
                End If
                Exit Sub
            Else
                MsgBox("Invalid Administrator password", vbInformation, "Invalid")
                Me.txtPassword.Clear()
            End If

        End If
    End Sub

    Private Sub BunifuThinButton22_Click(sender As Object, e As EventArgs) Handles BunifuThinButton22.Click
        Me.txtPassword.Clear()
        FrmMain.SubjectGrade()
        count = 0
        Me.Close()
    End Sub

    Private Sub txtPassword_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPassword.Enter
        Me.RectangleShape1.BorderColor = System.Drawing.Color.FromArgb(41, 119, 204)
        Me.RectangleShape1.BorderWidth = 3
    End Sub

    Private Sub txtPassword_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPassword.Leave
        Me.RectangleShape1.BorderColor = System.Drawing.Color.FromArgb(255, 255, 255)
        Me.RectangleShape1.BorderWidth = 1
    End Sub
End Class