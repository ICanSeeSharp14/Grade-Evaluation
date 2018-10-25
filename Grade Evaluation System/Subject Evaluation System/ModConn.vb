Imports MySql.Data.MySqlClient
Module ModConn
    '  Public conn As New MySqlConnection("Server=localhost;uid=root;pwd='';Database=dbevaluation;Port=3306;")
    Public strConnString As String

    Sub getData()
        Dim AppName As String = Application.ProductName

        Try
            DBNameMySQL = GetSetting(AppName, "DBEvaluationSection", "DBName", "temp")
            ServerMySQL = GetSetting(AppName, "DBEvaluationSection", "DBServer", "temp")
            PortMySQL = GetSetting(AppName, "DBEvaluationSection", "DBPort", "temp")
            UserNameMySQL = GetSetting(AppName, "DBEvaluationSection", "DBUserID", "temp")
            PwdMySQL = GetSetting(AppName, "DBEvaluationSection", "DBPwd", "temp")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Public Sub ConnDB()
        conn.Close()
        Try

            ' strConnString = "Server=localhost;uid=root;pwd='';Database=dbevaluation;Port=3306;"
            strConnString = "Server='" & ServerMySQL & "';uid='" & UserNameMySQL & "';password='" & PwdMySQL & "';database='" & DBNameMySQL & "';port='" & PortMySQL & "';"
            conn = New MySqlConnection(strConnString)

            If conn.State = ConnectionState.Closed Then conn.Open()

        Catch ex As Exception
            MsgBox("The system failed to establish a connection", MsgBoxStyle.Information, "Database Settings")
        End Try

    End Sub

    Public Sub DisconnMy()

        conn.Close()
        conn.Dispose()

    End Sub

    Sub SaveData()
        Dim AppName As String = Application.ProductName

        SaveSetting(AppName, "DBEvaluationSection", "DBName", DBNameMySQL)
        SaveSetting(AppName, "DBEvaluationSection", "DBServer", ServerMySQL)
        SaveSetting(AppName, "DBEvaluationSection", "DBPort", PortMySQL)
        SaveSetting(AppName, "DBEvaluationSection", "DBUserID", UserNameMySQL)
        SaveSetting(AppName, "DBEvaluationSection", "DBPwd", PwdMySQL)

        MsgBox("Database connection settings are saved.", MsgBoxStyle.Information)
    End Sub


End Module
