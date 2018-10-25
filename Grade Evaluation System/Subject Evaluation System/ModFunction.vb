Imports MySql.Data.MySqlClient

Module ModFunction
    Dim a As ComboBox
    Dim btn As Button
    Public strText As String

    Sub loadPrerequisteSubject()
        sqlQuery =
<sql>
SELECT PID,`PrereqYear`,`PrereqSem`,`Subject Code`,
`Subject Description`,`Prerequisite`,`Prerequisite Description`,
`Year` as `Year Level`,`SeFrmMainster`,`Curriculum` FROM tblprerequisite a
LEFT JOIN (SELECT csid,subcode as `Subject Code`,
subdesc as `Subject Description`,Year,sem as `SeFrmMainster` FROM tblcoursesub) b
ON a.subjectid = b.csid
LEFT JOIN (SELECT csid ,subcode as `Prerequisite`,
subdesc as `Prerequisite Description`,`Year` as `PrereqYear`,`Sem` as `PrereqSem`
 FROM tblcoursesub) c
ON a.prerequisitesubjectid = c.csid
LEFT JOIN (SELECT courseid,currseries as `Curriculum` FROM tblcourse) d
ON a.courseid = d.courseid where `Subject Description` LIKE 
'%<%= FrmMain.txtSearchPrereq.Text.Replace("'", "/'") %>%'
OR `Subject Code` LIKE '%<%= FrmMain.txtSearchPrereq.Text.Replace("'", "/'") %>%'
OR prerequisite LIKE '%<%= FrmMain.txtSearchPrereq.Text.Replace("'", "/'") %>%'
OR `Prerequisite Description` LIKE '%<%= FrmMain.txtSearchPrereq.Text.Replace("'", "/'") %>%'
</sql>

        FrmMain.DGVPrerequiste.DataSource = SQLRetrieve(sqlQuery)
        FrmMain.DGVPrerequiste.Columns.Item(0).Visible = False
        FrmMain.DGVPrerequiste.Columns.Item(1).Visible = False
        FrmMain.DGVPrerequiste.Columns.Item(2).Visible = False
    End Sub
    Sub CurriculumSeries()

        sqlQuery = "SELECT Currseries FROM tblCourse"
        dt = SQLRetrieve(sqlQuery)
        '        FrmMain.cmbCurriculum.Items.Add("")
        FrmMain.cmbPrereqCurriculum.DisplayMember = "Currseries"
        FrmMain.cmbPrereqCurriculum.DataSource = dt



    End Sub


    Sub Subject()
        sqlQuery =
<sql>
Select subdesc,subcode from tblcoursesub where courseid IN 
(select courseid from tblcourse where currseries = '<%= FrmMain.cmbPrereqCurriculum.Text %>')
and sem like '<%= FrmMain.cmbSubjSemester.Text %>%'
and year like '<%= FrmMain.cmbSubjectYearLevel.Text %>%'
and sem != 'summer' 
order by subdesc asc; 
</sql>
        FrmMain.ListBox1.Items.Clear()
        dt = SQLRetrieve(sqlQuery)
        For x = 0 To dt.Rows.Count - 1
            FrmMain.ListBox1.Items.Add(dt.Rows(x).Item(0).ToString)
        Next x

    End Sub

    Sub PrerequisiteInfo()
        On Error Resume Next
        sqlQuery =
<sql>
Select subdesc from tblcoursesub where courseid IN 
(select courseid from tblcourse where currseries = '<%= FrmMain.cmbPrereqCurriculum.Text %>')
and year like '<%= FrmMain.cmbPrereqYearLevel.Text %>%'
and sem like '<%= FrmMain.cmbPrereqSem.Text %>%'
and csid not in (select prerequisitesubjectid from tblprerequisite
where subjectid in (select csid from tblcoursesub where
subdesc = '<%= FrmMain.ListBox1.SelectedItem.ToString %>'))
and subdesc != '<%= FrmMain.ListBox1.SelectedItem.ToString %>'
and sem != 'summer'
order by subdesc asc; 
</sql>

        FrmMain.CheckedListBox1.Items.Clear()
        dt = SQLRetrieve(sqlQuery)
        For x = 0 To dt.Rows.Count - 1
            FrmMain.CheckedListBox1.Items.Add(dt.Rows(x).Item(0).ToString)

        Next x

    End Sub
  
    Sub LettersOnly(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        Dim notallowedkey As String = "0123456789!@#$%^&*()_+~`-=[]{}"":;|\<>?,./'"
        If InStr(notallowedkey, LCase(e.KeyChar)) Then
            e.Handled = True
        End If
    End Sub

    Sub NumberOnly(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        Dim notallowedkey As String = "qwertyuiopasdfghjklzxcvbnm!@#$%^&*()_+~`=[]{}"":;|\<>?,./-"
        If InStr(notallowedkey, LCase(e.KeyChar)) Then
            e.Handled = True
        End If
    End Sub


    Sub toProperCase(ByVal txt As TextBox)

        On Error Resume Next
        If txt.Text.Trim.Length > 0 Then
            strText = txt.Text
            txt.Text = (StrConv(strText, VbStrConv.ProperCase))
            txt.Select(txt.Text.Length, 0)

        End If
    End Sub

    Sub DefaultBGColor(ByVal obj As Control)
        Dim ctrl As Control
        For Each ctrl In obj.Controls
            If TypeOf ctrl Is Button Then
                btn = ctrl
                btn.BackColor = System.Drawing.Color.FromArgb(34, 45, 50)
            End If
        Next
    End Sub

    Sub ConnOpen()
        If conn.State = ConnectionState.Closed Then conn.Open()
    End Sub
    Sub ConnClose()
        conn.Close()
    End Sub

    Sub ShowMessage()

        Messages.PictureBox6.Image = My.Resources.Info_Squared_50px
        Messages.lblTitle.Text = "Saved"
        Messages.lblMessage.Text = "Subject Successfully Saved"
        Messages.Show()
    End Sub

    Function SQLExecute(ByVal sql As String) As Integer
        Try
            ConnOpen()
            cmd = New MySqlCommand(sql, conn)
            result = cmd.ExecuteNonQuery


        Catch ex As Exception
            MsgBox(ex.Message, vbCritical, "Execution Failed")
        Finally
            '    cmd.Dispose()
            conn.Dispose()
        End Try
        Return result
    End Function

    Function isEmpty(ByVal obj As Control) As Boolean
        Dim ctrl As Control
        For Each ctrl In obj.Controls
            If TypeOf ctrl Is TextBox Then
                If ctrl.Text = "" Then
                    Return True
                End If
            ElseIf TypeOf ctrl Is ComboBox Then
                a = ctrl
                If a.Text = "" Then
                    Return True
                End If
            End If
        Next
        Return False
    End Function

    Sub ClearText(ByVal obj As Control)
        Dim ctrl As Control
        For Each ctrl In obj.Controls
            If TypeOf ctrl Is TextBox Then

                ctrl.Text = ""
            ElseIf TypeOf ctrl Is ComboBox Then
                a = ctrl
                If a.DropDownStyle = ComboBoxStyle.DropDownList Or a.DropDownStyle = ComboBoxStyle.DropDown Then
                    a.ResetText()
                    a.SelectedIndex = -1
                End If
            End If
        Next
    End Sub

    Function Key(ByVal dgView As DataGridView)
        Return dgView(0, dgView.CurrentRow.Index).Value.ToString
    End Function

    Function SQLRetrieve(ByVal sql As String) As DataTable

        Try
            ConnOpen()
            dt = New DataTable
            cmd = New MySqlCommand(sql, conn)
            dt.Load(cmd.ExecuteReader, LoadOption.OverwriteChanges)

        Catch ex As Exception
            MsgBox(ex.Message, vbCritical, "Execution Failed")

        Finally
            cmd.Dispose()
            conn.Close()
            conn.Dispose()

        End Try

        Return dt
    End Function

End Module
