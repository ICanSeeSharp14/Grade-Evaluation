Public Class YesNoMessage

    
    Private Sub Label3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub YesNoMessage_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.btnOkay.Focus()
    End Sub


    
    Private Sub BunifuThinButton22_Click(sender As Object, e As EventArgs) Handles BunifuThinButton22.Click
        Me.Dispose()
    End Sub

    Private Sub btnOkay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOkay.Click
        If lblMessage.Text = "Program will close. Continue?" Then
            Application.Exit()

        ElseIf lblMessage.Text = "Are you sure you want to delete this subject" Then
            Me.Close()
            sqlQuery = "Delete from tblcoursesub where csid = '" & Key(FrmMain.DGVSubjectDetails) & "'"
            SQLExecute(sqlQuery)
            FrmMain.LoadSubject(FrmMain.txtSearchSubject.Text)

            Messages.PictureBox6.Image = My.Resources.Info_Squared_50px
            Messages.lblTitle.Text = "Deleted"
            Messages.lblMessage.Text = "Subject Successfully Deleted."
            Messages.Show()

        ElseIf lblMessage.Text = "Are you sure you want to delete this prerequisite" Then

            Me.Close()
            sqlQuery = "Delete from tblprerequisite where pid = '" & Key(FrmMain.DGVPrerequiste) & "'"
            SQLExecute(sqlQuery)
            FrmMain.loadPrerequisteSubject()

            Messages.PictureBox6.Image = My.Resources.Info_Squared_50px
            Messages.lblTitle.Text = "Deleted"
            Messages.lblMessage.Text = "Prerequisite Subject Successfully Deleted."
            Messages.Show()

        ElseIf lblMessage.Text = "Are you sure you want to delete this user" Then

            Me.Close()
            sqlQuery = "Delete from tbladmin where username = '" & Key(FrmMain.DGVUsersList) & "'"
            SQLExecute(sqlQuery)
            FrmMain.LoadUsers()

            Messages.PictureBox6.Image = My.Resources.Info_Squared_50px
            Messages.lblTitle.Text = "Deleted"
            Messages.lblMessage.Text = "User Successfully Deleted."
            Messages.Show()

        End If
    End Sub
End Class