Public Class SearchStudent
   
    Sub ShowStudentInfo()

        sqlQuery = "select studid as `Student ID`,concat(lname,', ',fname,' ',substring(mname,1,1),'.') as `Complete Name`,currseries as `Curriculum` from tblstudent a " & _
            "left join (select courseid,currseries from tblcourse) b on a.courseid = b.courseid " & _
            "where studid like '%" & Me.txtSearch.Text.Replace("'", "/'") & _
            "%' or concat(lname,', ',fname,' ',substring(mname,1,1),'.') like '%" & Me.txtSearch.Text.Replace("'", "/'") & "%'"

        Me.DGVSearchStudent.DataSource = SQLRetrieve(sqlQuery)

    End Sub

    Private Sub SearchStudent_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'animateWin(Me, True)
        ShowStudentInfo()

        With Me.DGVSearchStudent.Columns
            .Item(0).Width = 140

            .Item(1).Width = 240
            .Item(2).Width = 110
        End With
        Me.txtSearch.Focus()
    End Sub

    Private Sub txtSearch_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            Me.Dispose()
        End If
    End Sub

    Private Sub txtSearch_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearch.TextChanged
        If Me.txtSearch.TextLength > 0 Then
            Me.lblTitle.Visible = False
        Else
            Me.lblTitle.Visible = True
        End If
        ShowStudentInfo()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        If Me.DGVSearchStudent.RowCount > 0 Then
            If strSearch = "Enroll" Then
                FrmMain.txtEnrollStudID.Text = Me.DGVSearchStudent(0, Me.DGVSearchStudent.CurrentRow.Index).Value.ToString
                FrmMain.txtEnrollCurriculum.Text = Me.DGVSearchStudent(2, Me.DGVSearchStudent.CurrentRow.Index).Value.ToString
                FrmMain.txtStudName.Text = Me.DGVSearchStudent(1, Me.DGVSearchStudent.CurrentRow.Index).Value.ToString
            Else
                FrmMain.txtGradeStudentID.Text = Me.DGVSearchStudent(0, Me.DGVSearchStudent.CurrentRow.Index).Value.ToString
                FrmMain.txtGradeCurriculum.Text = Me.DGVSearchStudent(2, Me.DGVSearchStudent.CurrentRow.Index).Value.ToString

            End If
        Else
            With FrmMain
                .txtStudName.Clear()
                .txtEnrollCurriculum.Clear()
                .txtEnrollStudID.Text = ""
                .txtGradeStudentID.Text = ""
                .txtGradeCurriculum.Clear()
            End With
        End If
        Me.Dispose()
    End Sub

    Private Sub DGVSearchStudent_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles DGVSearchStudent.DoubleClick
        If Me.DGVSearchStudent.RowCount > 0 Then
            If strSearch = "Enroll" Then
                FrmMain.txtEnrollStudID.Text = Me.DGVSearchStudent(0, Me.DGVSearchStudent.CurrentRow.Index).Value.ToString
                FrmMain.txtEnrollCurriculum.Text = Me.DGVSearchStudent(2, Me.DGVSearchStudent.CurrentRow.Index).Value.ToString
                FrmMain.txtStudName.Text = Me.DGVSearchStudent(1, Me.DGVSearchStudent.CurrentRow.Index).Value.ToString
            Else
                FrmMain.txtGradeStudentID.Text = Me.DGVSearchStudent(0, Me.DGVSearchStudent.CurrentRow.Index).Value.ToString
                FrmMain.txtGradeCurriculum.Text = Me.DGVSearchStudent(2, Me.DGVSearchStudent.CurrentRow.Index).Value.ToString

            End If
        Else
            With FrmMain
                .txtStudName.Clear()
                .txtEnrollCurriculum.Clear()
                .txtEnrollStudID.Text = ""
                .txtGradeStudentID.Text = ""
                .txtGradeCurriculum.Clear()
            End With
        End If
        Me.Dispose()
    End Sub

   
    Private Sub lblTitle_Click(sender As Object, e As EventArgs) Handles lblTitle.Click
        Me.txtSearch.Focus()
    End Sub

    Private Sub DGVSearchStudent_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVSearchStudent.CellContentClick

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs)

    End Sub
End Class