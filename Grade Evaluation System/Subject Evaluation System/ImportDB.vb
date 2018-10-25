Imports MySql.Data.MySqlClient
Public Class ImportDB

    Private Sub Button21_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button21.Click
        Try
            Dim mysqlconn As New MySqlConnection("server='" & Me.txtImportHost.Text & _
                                                 "';userid='" & Me.txtImportUser.Text & _
                                                 "';pwd='" & Me.txtImportPass.Text & _
                                                 "';port=3306;")



            Dim mysqlcmd As MySqlCommand = New MySqlCommand
            mysqlcmd.Connection = mysqlconn
            mysqlconn.Open()
            Dim mb As MySqlBackup = New MySqlBackup(mysqlcmd)
            mb.ImportFromFile(txtImportPath.Text)
            mysqlconn.Close()
            Me.Close()
            MsgBox("Database successfully import", vbInformation)
        Catch ex As Exception
            MsgBox(ex.Message)

        End Try
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Dim OpenFile As New OpenFileDialog()

        'OpenFile.FileName = Me.txtDBName.Text & ".sql"
        'OpenFile.Filter = "Sql Files (*.sql).|*.sql"
        OpenFile.ShowDialog()
        OpenFile.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*|Sql Files (*.sql).|*.sql"
        OpenFile.FilterIndex = 3

        OpenFile.RestoreDirectory = True

        If OpenFile.FileName <> "" Then
            txtImportPath.Text = OpenFile.FileName

        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ClearText(Me)
        Me.Dispose()
    End Sub

    Private Sub Panel28_Paint(sender As Object, e As PaintEventArgs) Handles Panel28.Paint

    End Sub

    Private Sub ImportDB_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Location = New Point(178, 127)
    End Sub
End Class