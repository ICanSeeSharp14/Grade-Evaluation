Imports MySql.Data.MySqlClient
Public Class DBConfig
    Private TstServerMySQL As String
    Private TstPortMySQL As String
    Private TstUserNameMySQL As String
    Private TstPwdMySQL As String
    Private TstDBNameMySQL As String

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        TstServerMySQL = txtServer.Text
        TstPortMySQL = txtPort.Text
        TstUserNameMySQL = txtUser.Text
        TstPwdMySQL = txtPass.Text
        TstDBNameMySQL = txtDB.Text


        Try

            strConnString = "Server='" & Me.txtServer.Text & _
                "';uid='" & txtUser.Text & _
                "';password='" & Me.txtPass.Text & _
                "';database='" & Me.txtDB.Text & _
                "';port='" & Me.txtPort.Text & "';"

            conn = New MySqlConnection(strConnString)

            DBNameMySQL = txtDB.Text
            ServerMySQL = txtServer.Text
            UserNameMySQL = txtUser.Text
            PortMySQL = txtPort.Text
            PwdMySQL = txtPass.Text

            Call SaveData()
            Me.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Database Settings")
        End Try
        Call DisconnMy()
    End Sub

    Private Sub DBConfig_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Me.Location = New Point(178, 127)
        txtServer.Text = ServerMySQL
        txtPort.Text = PortMySQL
        txtUser.Text = UserNameMySQL
        txtPass.Text = PwdMySQL
        txtDB.Text = DBNameMySQL
    End Sub

    Private Sub txtDB_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDB.Click
        On Error Resume Next
        Dim mysqlconn = New MySqlConnection("server='" & Me.txtServer.Text & "';userid='" & Me.txtUser.Text & _
                                            "';pwd='" & Me.txtPass.Text & "';port = '" & Me.txtPort.Text & "';")
        sqlQuery = "show databases;"
        dt = New DataTable
        adapter = New MySqlDataAdapter(sqlQuery, mysqlconn)
        adapter.Fill(dt)
        Me.txtDB.DataSource = dt
        Me.txtDB.DisplayMember = "database"
    End Sub


    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        TstServerMySQL = txtServer.Text
        TstPortMySQL = txtPort.Text
        TstUserNameMySQL = txtUser.Text
        TstPwdMySQL = txtPass.Text
        TstDBNameMySQL = txtDB.Text


        Try
            conn.ConnectionString = "Server = '" & TstServerMySQL & "';  " _
                                         & "Database = '" & TstDBNameMySQL & "'; " _
                                         & "uid = '" & TstUserNameMySQL & "'; " _
                                         & "password = '" & TstPwdMySQL & "'; " _
                                         & "Port = '" & TstPortMySQL & "'; "



           
            MsgBox("Connection Successful", vbInformation, "Test Result")
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Database Settings")
        End Try

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub
End Class