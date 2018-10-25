Imports System.IO
Public Class EventEntry
    Dim strPath As String
    Dim filepath As String
    Dim strFilename As String
    Dim lblPath As New Label

    Private Sub EventEntry_Load(sender As Object, e As EventArgs)
      
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Dim openfile As New OpenFileDialog
        openfile.Filter = "jpg files (*.jpg)|*.jpg|All files (*.*)|*.*"

        openfile.RestoreDirectory = True
        strpath = Path.GetDirectoryName(Application.ExecutablePath) + "\Events\"
        If Directory.Exists(strpath) = False Then
            Directory.CreateDirectory(strpath)
        End If
        If (openfile.ShowDialog = Windows.Forms.DialogResult.OK) Then
            strFilename = openfile.SafeFileName
            filepath = openfile.FileName
            Me.TextBox1.Text = strFilename

         
            PictureBox1.BackgroundImage = New Bitmap(openfile.OpenFile())
        Else
            openfile.Dispose()
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If (System.IO.File.Exists(strPath & Me.TextBox1.Text)) = True Then
            GoTo save

        Else
            File.Copy(filepath, strPath & strFilename)
            GoTo save
        End If
save:
        Dim sqlsave As String
        sqlsave = <sql>
                      insert into tblevent (`PicName`, `EventDesc`)
values ('<%= Me.TextBox1.Text %>','<%= Me.TextBox2.Text %>')
                  </sql>

        SQLExecute(sqlsave)
        Dim events As New EventPanel
        With events
            .lblDesc.Text = Me.TextBox2.Text
            sqlQuery = "select * from tblevent order by eventid desc limit 1"
            dt = SQLRetrieve(sqlQuery)
            .lblEventID.Text = dt.Rows(0).Item(0).ToString
            .lblFileName.Text = TextBox1.Text
            .PictureBox1.BackgroundImage = Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\Events\" & .lblFileName.Text)
            .Dock = DockStyle.Top
        End With
        pnlList.Controls.Add(events)
        PictureBox1.BackgroundImage = Nothing

        
    End Sub

    Private Sub EventEntry_Load_1(sender As Object, e As EventArgs) Handles MyBase.Load
        getData()
        ConnDB()
        pnlList.Controls.Clear()
        sqlQuery = "select * from tblevent order by eventid desc"
        dt = SQLRetrieve(sqlQuery)
        If dt.Rows.Count > 0 Then
            For x = 0 To dt.Rows.Count - 1

                Dim events As New EventPanel
                With events
                    .Name = "event" & x
                    .lblDesc.Text = dt.Rows(x).Item(2).ToString
                    .lblEventID.Text = dt.Rows(x).Item(0).ToString
                    .TabIndex = 0
                    .lblFileName.Text = dt.Rows(x).Item(1).ToString
                    .PictureBox1.BackgroundImage = Image.FromFile(Path.GetDirectoryName(Application.ExecutablePath) + "\Events\" & .lblFileName.Text)
                    .Dock = DockStyle.Top
                    pnlList.Controls.Add(events)

                End With
            Next
        End If
        pnlList.Refresh()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
      

    End Sub

    Private Sub pnlList_Paint(sender As Object, e As PaintEventArgs) Handles pnlList.Paint

    End Sub
End Class