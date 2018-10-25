Imports MySql.Data.MySqlClient
Module ModVariables
    Public cmd As MySqlCommand = New MySqlCommand
    Public builder As MySqlCommandBuilder
    Public adapter As MySqlDataAdapter
    Public ds As New DataSet
    Public isCreated As Boolean = True
    Public result As Integer
    Public conn As New MySqlConnection
    Public prereqid As Integer
    Public courseid As Integer
    Public subjid As Integer
    Public dt1 As DataTable
    Public strSearch As String
    Public ServerMySQL As String
    Public PortMySQL As String
    Public UserNameMySQL As String
    Public PwdMySQL As String
    Public DBNameMySQL As String
    Public sqL As String
    Public strUsername As String
    '  Public ds As New DataSet()
    Public dt2 As DataTable
    Public isSave As Boolean = True
    Public dt As New DataTable
    Public isChange As Boolean
    Public StrAdminPass As String = Nothing
    Public tempgrade As String = ""
    Public strTemp As String = Nothing
    Public sqlQuery As String = Nothing
    Public row As Integer
    Public isValid As Boolean
    Public count As Integer = 0
End Module
