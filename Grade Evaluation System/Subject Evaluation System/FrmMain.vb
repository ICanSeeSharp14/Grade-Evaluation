Imports MySql.Data.MySqlClient
Imports MySql.Data

Imports System.IO

Public Class FrmMain
    Dim scrollVal As Integer
    Dim adapter As MySqlDataAdapter
    Dim ds As DataSet
    Dim pagesize As Integer = 100
    Dim TotalPage As Integer
    Dim countrow As Integer
    Dim strYear As String = ""

#Region "Methods/Procedures"

    Sub Announcement()
        sqlQuery =
<sql>
Select What From tblannouncement ORDER BY ID DESC;
</sql>

        dt = SQLRetrieve(sqlQuery)
        lbAnnouncement.Items.Clear()
        For x = 0 To dt.Rows.Count - 1
            With lbAnnouncement
                .Items.Add(dt.Rows(x).Item(0).ToString)
            End With
        Next
    End Sub

    Sub SaveSubjectDetails()

        sqlQuery =
<sql>
INSERT INTO tblcoursesub (`courseid`, `subcode`, `subdesc`,
 `units`,  `year`,`sem`)
 VALUES
((SELECT courseid FROM tblcourse where currseries = '<%= Me.cmbSubCurriculum.Text.Replace("'", "/'") %>'),
'<%= Me.txtSubCode.Text.Replace("'", "/'") %>',
'<%= Me.txtSubjectDescription.Text.Replace("'", "/'") %>',
'<%= Me.txtUnits.Text.Replace("'", "/'") %>',
'<%= Me.cmbYearLevel.Text.Replace("'", "/'") %>',
'<%= Me.cmbSubjectSem.Text.Replace("'", "/'") %>')
</sql>

        SQLExecute(sqlQuery)
        If result > 0 Then
            '  btnCancel2.PerformClick()
            LoadStudent(Me.txtSearchSubject.Text)
            Messages.PictureBox6.Image = My.Resources.Info_Squared_50px
            Messages.lblTitle.Text = "Saved"
            Messages.lblMessage.Text = "Subject Successfully Saved"
            Messages.Show()

        End If


    End Sub


    Sub UpdateSubjectDetails()

        sqlQuery =
<sql>
UPDATE tblcoursesub SET courseid 
= (SELECT courseid FROM tblcourse WHERE currseries = '<%= Me.cmbSubCurriculum.Text %>'),
subcode = '<%= Me.txtSubCode.Text.Replace("'", "/'") %>',
subdesc = '<%= Me.txtSubjectDescription.Text.Replace("'", "/'") %>',
units = '<%= Me.txtUnits.Text.Replace("'", "/'") %>',
year = '<%= cmbYearLevel.Text %>',
 sem = '<%= cmbSubjectSem.Text %>'
WHERE
csid = '<%= Key(Me.DGVSubjectDetails) %>'
</sql>

        SQLExecute(sqlQuery)
        '   If result > 0 Then
        btnCancel2.PerformClick()
        LoadStudent(Me.txtSearchSubject.Text)
        Messages.PictureBox6.Image = My.Resources.Info_Squared_50px

        Messages.lblTitle.Text = "Success"
        Messages.lblMessage.Text = "Subject Successfully Updated"
        Messages.ShowDialog()
    End Sub

    Sub LoadUsers()

        sqlQuery = "SELECT userid,Username,Password,Usertype,Name FROM tbladmin"
        Me.DGVUsersList.DataSource = SQLRetrieve(sqlQuery)
        With Me.DGVUsersList.Columns
            .Item(0).Width = 150
            .Item(0).Visible = False
            .Item(1).Width = 150
            .Item(2).Width = 150
            .Item(3).Width = 200
            .Item(4).Width = 250
            .Item(0).Visible = False
            .Item(2).Visible = False

        End With
    End Sub
    Sub LoadCurriculumSeries()
        sqlQuery = "SELECT `courseid`, `coursecode` as `Course Code`, `coursedesc` as `Course Description`, `currseries` as `Curriculum`, `CurriculumDesc` as `Curriculum Description` FROM `tblcourse`"
        Me.DGVCurriculum.DataSource = SQLRetrieve(sqlQuery)
        With Me.DGVCurriculum.Columns
            .Item(0).Visible = False
            .Item(1).Width = 130
            .Item(2).Width = 500
            .Item(3).Width = 150
            .Item(4).Width = 250

        End With

    End Sub

    Sub GradeSchoolYear()
        sqlQuery = "Select distinct year from tblensub where studid = '" & Me.txtGradeStudentID.Text & "'"
        Me.cmbGradeSY.DataSource = SQLRetrieve(sqlQuery)
        Me.cmbGradeSY.DisplayMember = "year"

        sqlQuery = "Select distinct year from tblensub where sem!='Summer' and year!='tor' order by year DESC "
        Me.txtSY.Items.Clear()
        Dim month As Integer = Now.Month
        Dim year As Integer = Now.Year + 1

        For x = Now.Year - 1 To 2008 Step -1
            Me.txtSY.Items.Add(x & "-" & x + 1)
        Next

        If month >= 1 And month <= 5 Or Now.Month = 12 Then
            year = Now.Year - 1
            Me.txtSY.Text = year & "-" & Format(Now, "yyyy")
        End If

    End Sub

    Sub SubjectGrade()

        sqlQuery = "SELECT ensubid,subcode as `Subject Code`,subdesc as `Subject Description`,UCASE(Grade) as `Grades` " & _
            ",`Units`,CASE WHEN grade IN ('5.0') THEN 'Failed' WHEN grade IN ('INC') then 'Incomplete' " & _
            " when grade IN ('') then 'No Grade' WHEN grade IN ('DRP') THEN 'Dropped' WHEN Grade in ('4.0','COND') THEN 'Conditional'" & _
            "when grade in ('1.0','1.25','1.5','1.75','2.0','2.25','2.5','2.75','3.0') then 'Passed' END AS `Remarks` " & _
            " FROM tblensub WHERE studid = '" & Me.txtGradeStudentID.Text & _
            "' and sem like '%" & Me.cmbGradeSem.Text & "%' and year like '" & Me.cmbGradeSY.Text & _
            "%' and (subdesc like '%" & Me.txtSearchGrade.Text & "%' or subcode like '%" & Me.txtSearchGrade.Text & _
            "%') order by year,sem ASC"

        Me.DGVGrades.DataSource = SQLRetrieve(sqlQuery)
        With Me.DGVGrades.Columns
            .Item(0).Visible = False
            .Item(1).Width = 130
            .Item(2).Width = 600
            .Item(1).ReadOnly = True
            .Item(2).ReadOnly = True
            .Item(3).Width = 120
            .Item(5).Width = 120
            .Item(4).ReadOnly = True
            .Item(5).ReadOnly = True

        End With

    End Sub

    Sub isActive5(ByVal Active As Boolean)
        Me.DGVCurriculum.Enabled = Active
        Me.btnNew5.Enabled = Active
        Me.btnDelete5.Enabled = Active
        btnEdit5.Enabled = Active
        btnSave5.Enabled = Not Active
        btnCancel5.Enabled = Not Active
        Me.DGVUsersList.Enabled = Active
        Me.txtUserName.Enabled = Not Active
        Me.txtUserPassword.Enabled = Not Active
        Me.txtUserRole.Enabled = Not Active
        Me.txtNameofUser.Enabled = Not Active
    End Sub

    Sub isActive4(ByVal Active As Boolean)
        Me.DGVCurriculum.Enabled = Active
        Me.btnNew4.Enabled = Active
        btnEdit4.Enabled = Active
        btnSave4.Enabled = Not Active
        btnCancel4.Enabled = Not Active

        Me.DGVCurriculum.Enabled = Active
        Me.txtCourseCode.Enabled = Not Active
        Me.txtCourseDesc.Enabled = Not Active
        Me.txtCurrDesc.Enabled = Not Active
        Me.txtCurrSeries.Enabled = Not Active
    End Sub

    Sub isActive1(ByVal Active As Boolean)
        Me.DGVStudent.Enabled = Active
        Me.btnNew1.Enabled = Active
        btnEdit1.Enabled = Active
        btnDelete1.Enabled = Active
        btnSave1.Enabled = Not Active
        btnCancel1.Enabled = Not Active
        GroupBox1.Enabled = Not Active
        btnPrev1.Enabled = Active
        btnNext1.Enabled = Active
        TextBox2.Enabled = Active

    End Sub

    Sub isActive2(ByVal Active As Boolean)
        Me.DGVSubjectDetails.Enabled = Active
        Me.btnNew2.Enabled = Active
        btnEdit2.Enabled = Active
        btnDelete2.Enabled = Active
        btnSave2.Enabled = Not Active
        btnCancel2.Enabled = Not Active
        ' GBSubjectDetails.Enabled = Not Active
        txtSubCode.Enabled = Not Active
        txtSubjectDescription.Enabled = Not Active
        txtUnits.Enabled = Not Active
        cmbSubjectSem.Enabled = Not Active
        cmbYearLevel.Enabled = Not Active
        cmbSubCurriculum.Enabled = Not Active
        txtSearchSubject.Enabled = Active
        cmbFilterBy.Enabled = Active

    End Sub

    Sub isActive3(ByVal Active As Boolean)
        Me.DGVPrerequiste.Enabled = Active
        Me.btnNew3.Enabled = Active
        btnEdit3.Enabled = Active
        btnDelete3.Enabled = Active
        btnSave3.Enabled = Not Active
        btnCancel3.Enabled = Not Active

        Me.SubjectPanel.Enabled = Not Active
        Me.PrerequisitePanel.Enabled = Not Active
        Me.txtSearchPrereq.Enabled = Active
        ' PrerequisiteTab.Enabled = Not Active
        ' txtSearchSubject.Enabled = Active

    End Sub

    Sub loadPrerequisteSubject()
        sqlQuery =
<sql>
SELECT PID,`PrereqYear`,`PrereqSem`,`Subject Code`,
`Subject Description`,`Prerequisite`,`Prerequisite Description`,
`Year` as `Year Level`,`Semester`,`Curriculum` FROM tblprerequisite a
LEFT JOIN (SELECT csid,subcode as `Subject Code`,
subdesc as `Subject Description`,Year,sem as `Semester` FROM tblcoursesub) b
ON a.subjectid = b.csid
LEFT JOIN (SELECT csid ,subcode as `Prerequisite`,
subdesc as `Prerequisite Description`,`Year` as `PrereqYear`,`Sem` as `PrereqSem`
 FROM tblcoursesub) c
ON a.prerequisitesubjectid = c.csid
LEFT JOIN (SELECT courseid,currseries as `Curriculum` FROM tblcourse) d
ON a.courseid = d.courseid where `Subject Description` LIKE 
'%<%= Me.txtSearchPrereq.Text.Replace("'", "/'") %>%'
OR `Subject Code` LIKE '%<%= Me.txtSearchPrereq.Text.Replace("'", "/'") %>%'
OR prerequisite LIKE '%<%= Me.txtSearchPrereq.Text.Replace("'", "/'") %>%'
OR `Prerequisite Description` LIKE '%<%= Me.txtSearchPrereq.Text.Replace("'", "/'") %>%'
</sql>

        Me.DGVPrerequiste.DataSource = SQLRetrieve(sqlQuery)
        Me.DGVPrerequiste.Columns.Item(0).Visible = False
        Me.DGVPrerequiste.Columns.Item(1).Visible = False
        Me.DGVPrerequiste.Columns.Item(2).Visible = False

    End Sub

    Sub EnrolledSubject()
        sqlQuery = "SELECT ensubid,`Subcode` as `Subject Code`,`Subdesc` as `Subject Description`,`Units`,Grade " & _
            " FROM tblensub WHERE sem= '" & cmbEnrollSem.Text & _
            "' AND year = '" & Me.txtSY.Text & "' AND studid = '" & Me.txtEnrollStudID.Text & "'"

        Me.DGVEnrolledSubject.DataSource = SQLRetrieve(sqlQuery)
        With Me.DGVEnrolledSubject.Columns
            .Item(1).Width = 100
            .Item(2).Width = 400
            .Item(3).Width = 80

        End With

        Me.DGVEnrolledSubject.Columns.Item(0).Visible = False
        Me.DGVEnrolledSubject.Columns.Item("Grade").Visible = False
    End Sub

    Sub SuggestSubjectDescription()
        sqlQuery = "SELECT DISTINCT(subdesc) FROM tblcoursesub;"
        dt = SQLRetrieve(sqlQuery)

        Dim dr As DataRow
        txtSubjectDescription.AutoCompleteCustomSource.Clear()
        For Each dr In dt.Rows
            txtSubjectDescription.AutoCompleteCustomSource.Add(dr.Item(0).ToString)
        Next

    End Sub

    Sub SuggestID()
        sqlQuery = "SELECT studid FROM tblstudent LIMIT 10;"
        dt = SQLRetrieve(sqlQuery)
        Me.txtEnrollStudID.DataSource = dt
        Me.txtEnrollStudID.DisplayMember = "studid"
        ' Me.txtGradeStudentID.DataSource = Me.txtEnrollStudID.DataSource
        ' Me.txtGradeStudentID.DisplayMember = Me.txtEnrollStudID.DisplayMember

    End Sub

    Sub SubjectList()

        Me.DGVSubjectList.Columns.Clear()
        Dim chk As New DataGridViewCheckBoxColumn()
        chk.HeaderText = "Select"
        DGVSubjectList.Columns.Add(chk)

        sqlQuery = <sql>
                       SELECT `Subcode` as `Subject Code`,`Subdesc` as `Subject Description`,`Units`,csid
                        FROM tblcoursesub WHERE sem like '<%= Me.cmbFilterSem.Text %>%' and year like '<%= Me.cmbFilterYear.Text %>%'
                        and courseid IN (SELECT courseid FROM tblcourse WHERE currseries = '<%= Me.txtEnrollCurriculum.Text %>')
                        AND (subdesc not in (select subdesc from tblensub WHERE
                         studid = '<%= Me.txtEnrollStudID.Text %>' 
                        and grade NOT IN ('','4.0','5.0','INC','DRP'))) and (subdesc like '%<%= Me.txtSearchSubjectList.Text %>%'
                        or subcode like '%<%= Me.txtSearchSubjectList.Text %>%')
                    </sql>
        Me.DGVSubjectList.DataSource = SQLRetrieve(sqlQuery)
        With Me.DGVSubjectList.Columns
            .Item(0).Width = 60
            .Item(1).Width = 120
            .Item(2).Width = 300
            .Item(3).Width = 50
            .Item(4).Visible = False
            .Item(0).Frozen = True
            .Item(1).Frozen = True

        End With

    End Sub

    Sub Course()
        sqlQuery = "SELECT  DISTINCT coursecode FROM tblCourse"
        '        Me.cmbCurriculum.Items.Add("")
        Me.cmbCourse.DisplayMember = "coursecode"
        Me.cmbCourse.DataSource = SQLRetrieve(sqlQuery)
        ' Me.cmbPrereqcourse.DisplayMember = "coursecode"
        ' Me.cmbPrereqcourse.DataSource = SQLRetrieve(sqlQuery)
        Me.cmbCourse.Text = ""
    End Sub

    'Sub SubjectCode()
    '    sqlQuery = <sql>
    '                   SELECT subcode FROM tblcoursesub WHERE courseid =
    '         (SELECT courseid FROM tblcourse WHERE currseries = '<%= Me.cmbPrereqCurriculum.Text %>'
    '        ) and sem = '<%= Me.cmbSubjSemester.Text %>' and year = '<%= Me.cmbSubjectYearLevel.Text %>'
    '        and subcode != '<%= Me.cmbPrerequisiteCode.Text %>'
    '               </sql>

    '    Me.cmbPrereqSubCode.DisplayMember = "subcode"
    '    Me.cmbPrereqSubCode.DataSource = SQLRetrieve(sqlQuery)


    ' End Sub

    'Sub Prerequiste()
    '    sqlQuery = "SELECT subcode FROM tblcoursesub WHERE courseid = " & _
    '        "(SELECT courseid FROM tblcourse WHERE currseries = '" & _
    '        Me.cmbPrereqCurriculum.Text & "') and year = '" & Me.cmbPrereqYearLevel.Text & "' and subcode!='" & Me.cmbPrereqSubCode.Text & "'"

    '    'Me.cmbPrerequisiteCode.DisplayMember = "subcode"
    '    'Me.cmbPrerequisiteCode.DataSource = SQLRetrieve(sqlQuery)

    'End Sub

    Sub CurriculumSeries()

        sqlQuery = "SELECT Currseries FROM tblCourse"
        '        Me.cmbCurriculum.Items.Add("")
        Me.cmbCurriculum.DisplayMember = "Currseries"
        Me.cmbCurriculum.DataSource = SQLRetrieve(sqlQuery)

        Me.cmbSubCurriculum.DataSource = SQLRetrieve(sqlQuery)
        Me.cmbSubCurriculum.DisplayMember = "Currseries"
        Me.cmbSubCurriculum.Text = ""
        Me.cmbCurriculum.Text = ""
        Me.cmbPrereqCurriculum.DisplayMember = "Currseries"
        Me.cmbPrereqCurriculum.DataSource = Me.cmbCurriculum.DataSource

        sqlQuery = "SELECT Currseries FROM tblCourse"
        Me.cmbFilterBy.DataSource = SQLRetrieve(sqlQuery)
        Me.cmbFilterBy.DisplayMember = "Currseries"
        Me.cmbFilterBy.SelectedIndex = -1
        Me.txtEnrollStudID.Text = ""

    End Sub

    Sub LoadSubject(ByVal strSearch As String)
        sqlQuery =
<sql>
SELECT `csid`,
 `subcode` as `Subject Code`,
 `subdesc` as `Description`,`Curriculum`, 
 `Units`,
 `Year`, `sem` as `Semester` FROM `tblcoursesub` a
LEFT JOIN (SELECT courseid,currseries as `Curriculum` FROM tblcourse) b
ON a.courseid = b.courseid
 WHERE (Subcode LIKE '%<%= strSearch.Replace("'", "/'") %>%'
OR subdesc LIKE '%<%= strSearch.Replace("'", "/'") %>%') and b.curriculum like '<%= Me.cmbFilterBy.Text %>%';
</sql>


        Me.DGVSubjectDetails.DataSource = SQLRetrieve(sqlQuery)
        With Me.DGVSubjectDetails.Columns
            .Item(0).Visible = False
            .Item(0).Width = 20
            .Item(1).Width = 120
            .Item(2).Width = 480
            .Item(3).Width = 150
            .Item(4).Width = 80
            .Item(5).Width = 120
            .Item(6).Width = 130

        End With

    End Sub

    Sub TotalPages()
        dt = New DataTable
        sqlQuery = "SELECT COUNT(*) FROM tblStudent"
        dt = SQLRetrieve(sqlQuery)
        If dt.Rows.Count > 0 Then
            countrow = Val(dt.Rows(0).Item(0))
        End If
    End Sub

    Sub CountRecord()
        sqlQuery =
<sql>
SELECT COUNT(*) FROM tblStudent;
</sql>
        dt = SQLRetrieve(sqlQuery)
        If dt.Rows.Count > 0 Then
            Me.lblStudentCount.Text = dt.Rows(0).Item(0).ToString
        End If
        sqlQuery =
<sql>
SELECT COUNT(*) FROM tblcoursesub;
</sql>
        dt = SQLRetrieve(sqlQuery)
        If dt.Rows.Count > 0 Then
            Me.lblTotalSubject.Text = dt.Rows(0).Item(0).ToString
        End If

        sqlQuery =
<sql>
SELECT COUNT(*) FROM tblcourse;
</sql>
        dt = SQLRetrieve(sqlQuery)
        If dt.Rows.Count > 0 Then
            Me.lblCurriculumCount.Text = dt.Rows(0).Item(0).ToString
        End If

        sqlQuery =
<sql>
SELECT COUNT(*) FROM tbladmin;
</sql>
        dt = SQLRetrieve(sqlQuery)
        If dt.Rows.Count > 0 Then
            Me.lblUsersCount.Text = dt.Rows(0).Item(0).ToString
        End If

    End Sub

    Sub LoadStudent(ByVal strSearch As String)
        sqlQuery =
<sql>
SELECT studno,`studid` as `Student ID`,
`lname` as `Last Name`,
 `fname` as `First Name`, 
`mname` as `Middle Name`,
 `Gender`,`Course`, `Curriculum`, `Contact_Number` as `Contact Number`,
 `Guardian` as `Guardian Name` FROM `tblstudent` a
LEFT JOIN (SELECT courseid,CourseCode as `Course`, 
CurrSeries as `Curriculum` FROM tblCourse) b
ON a.courseid = b.courseid WHERE studid LIKE '%<%= strSearch.Replace("'", "/'") %>%'
OR fname LIKE '%<%= strSearch.Replace("'", "/'") %>%'
OR mname LIKE '%<%= strSearch.Replace("'", "/'") %>%'
OR lname LIKE '%<%= strSearch.Replace("'", "/'") %>%' ORDER BY lname,fname,mname ASC;
</sql>


        adapter = New MySqlDataAdapter(sqlQuery, conn)
        ds = New DataSet
        adapter.Fill(ds, scrollVal, 50, "tblstudent")
        conn.Close()


        Me.DGVStudent.DataSource = ds
        Me.DGVStudent.DataMember = "tblstudent"
        With Me.DGVStudent.Columns
            .Item(0).Visible = False
            .Item(1).Width = 120
            .Item(2).Width = 150
            .Item(3).Width = 150
            .Item(4).Width = 150
            .Item(5).Width = 100
            .Item("Curriculum").Width = 180
            .Item(8).Width = 180
            .Item(9).Width = 220
        End With

        TotalPages()
    End Sub


#End Region

    Private Sub FrmMain_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If ModifierKeys = Keys.Alt And e.KeyCode = Keys.F4 Then
            e.Handled = True
            Messages.PictureBox6.Image = My.Resources.Close_Window_50px
            Messages.lblTitle.Text = "Exit"
            Messages.btnOkay.Focus()
            Messages.lblMessage.Text = "Nice try but you need to click the Exit button."
            Messages.ShowDialog()
        End If
    End Sub


    Private Sub FrmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ConnOpen()
        Me.btnDashboard.PerformClick()
        '  SubjectCode()
        Announcement()
        ' Prerequiste()
        Me.TabControl1.Appearance = TabAppearance.FlatButtons
        Me.TabControl1.ItemSize = New Size(0, 1)
        Me.TabControl1.SizeMode = TabSizeMode.Fixed
        Me.TabControl1.SelectedTab = DashboardTab

        Timer1.Start()
        CountRecord()
        Me.Focus()
    End Sub

#Region "Timer"
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Me.lblDateTime.Text = Format(Now, "MM/dd/yyyy hh:mm:ss tt")
    End Sub
#End Region



    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        LoadStudent(Me.TextBox2.Text)
    End Sub

#Region "Side Navigation Menu Button Event"

    Private Sub btnSubject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubject.Click

        DefaultBGColor(Me.Panel3)
        CurriculumSeries()
        SuggestSubjectDescription()
        ' Me.btnSubject.BackColor = Color.Black
        Me.btnSubject.BackColor = System.Drawing.Color.FromArgb(79, 89, 97)
        Me.TabControl1.SelectTab(SubjectTab)
        LoadSubject(Me.txtSearchSubject.Text)

        ClearText(GBSubjectDetails)
        isActive2(True)
        Panel36.Enabled = True
    End Sub


    Private Sub btnUserAccount_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUserAccount.Click
        isActive5(True)
        LoadUsers()
        DefaultBGColor(Me.Panel3)
        Me.btnUserAccount.BackColor = System.Drawing.Color.FromArgb(79, 89, 97)
        Me.TabControl1.SelectTab(UsersTab)
    End Sub


    Private Sub btnEnroll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEnroll.Click
        Me.txtShowPrereq.Clear()
        DefaultBGColor(Me.Panel3)
        Me.btnEnroll.BackColor = System.Drawing.Color.FromArgb(79, 89, 97)
        SubjectList()
        Me.cmbEnrollSem.SelectedIndex = -1
        GradeSchoolYear()
        If isCreated = True Then
            SuggestID()
        End If
        isCreated = False
        EnrolledSubject()
        Me.txtEnrollStudID.SelectedIndex = -1

        Me.TabControl1.SelectTab(EnrollSubjectTab)
        txtEnrollStudID.Focus()
        Me.cmbFilterSem.SelectedIndex = -1
        Me.cmbFilterYear.SelectedIndex = -1
    End Sub



    Private Sub btnSMS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSMS.Click
        DefaultBGColor(Me.Panel3)
        Me.btnSMS.BackColor = System.Drawing.Color.FromArgb(79, 89, 97)
        Me.TabControl1.SelectTab(SMSTab)

    End Sub



    Private Sub btnGrade_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGrade.Click
        cmbGradeSem.SelectedIndex = -1
        DefaultBGColor(Me.Panel3)
        count = 0
        SuggestID()
        SubjectGrade()
        GradeSchoolYear()
        Me.btnGrade.BackColor = System.Drawing.Color.FromArgb(79, 89, 97)
        Me.TabControl1.SelectTab(GradeEntryTab)
        Me.txtGradeStudentID.Text = ""
        Me.cmbGradeSY.SelectedIndex = -1
        Me.txtGradeStudentID.Focus()
        isSave = True
    End Sub

    Private Sub btnCurriculum_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurriculum.Click
        DefaultBGColor(Me.Panel3)
        isActive4(True)
        Me.btnCurriculum.BackColor = System.Drawing.Color.FromArgb(79, 89, 97)
        LoadCurriculumSeries()
        Me.TabControl1.SelectTab(CurriculumTab)
    End Sub

    Private Sub btnPrereq_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrereq.Click
        DefaultBGColor(Me.Panel3)

        ModFunction.Subject()
        ModFunction.PrerequisiteInfo()
        ModFunction.CurriculumSeries()
        CurriculumSeries()
        btnCancel3_Click(sender, e)
        loadPrerequisteSubject()
        Course()
        Me.btnPrereq.BackColor = System.Drawing.Color.FromArgb(79, 89, 97)
        Me.TabControl1.SelectTab(PrerequisiteTab)
        isActive3(True)

    End Sub

    Private Sub btnStudentDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStudentDetails.Click
        DefaultBGColor(Me.Panel3)
        Me.btnStudentDetails.BackColor = System.Drawing.Color.FromArgb(79, 89, 97)
        CurriculumSeries()
        Course()
        ClearText(Me.GroupBox1)
        isActive1(True)
        Me.TabControl1.SelectedTab = StudentDetailTab
        LoadStudent(Me.TextBox2.Text)
        Me.cmbCurriculum.SelectedIndex = -1


    End Sub

    Private Sub btnDashboard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDashboard.Click
        DefaultBGColor(Me.Panel3)
        Dim frmLogin As New FrmLogin
        ' 60, 58, 69
        Me.btnDashboard.BackColor = System.Drawing.Color.FromArgb(79, 89, 97)
        'Me.btnDashboard.BackColor = System.Drawing.Color.FromArgb(60, 58, 69)
        Me.TabControl1.SelectTab(DashboardTab)
    End Sub

#End Region

#Region "Save Student"
    Private Sub btnSave1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave1.Click

        If isEmpty(Me.GroupBox1) = True Then
            MsgBox("Dont leave an empty fields", vbExclamation, "Empty")
            Me.txtStudentID.Focus()
            Exit Sub
        Else
            If isSave = True Then
                sqlQuery =
<sql>
    SELECT * FROM tblstudent WHERE studid = '<%= Me.txtStudentID.Text.Replace("'", "/'") %>'
</sql>
                dt = SQLRetrieve(sqlQuery)
                If dt.Rows.Count > 0 Then
                    Messages.PictureBox6.Image = My.Resources.Error_50px
                    Messages.lblTitle.Text = "Exist"
                    Messages.lblMessage.Text = "ID Number " & Me.txtStudentID.Text & " is already exist"
                    Messages.ShowDialog()

                Else
                    sqlQuery =
<sql>
INSERT INTO tblStudent (`studid`, `lname`, `fname`,
 `mname`, `gender`, `courseid`, `Contact_Number`, `Guardian`)
 VALUES
('<%= Me.txtStudentID.Text.Replace("'", "/'") %>',
'<%= Me.txtLastName.Text.Replace("'", "/'") %>',
'<%= Me.txtFirstName.Text.Replace("'", "/'") %>',
'<%= Me.txtMiddleName.Text.Replace("'", "/'") %>',
'<%= Me.cmbGender.Text.Replace("'", "/'") %>',
(SELECT courseid FROM tblcourse where currseries = '<%= Me.cmbCurriculum.Text.Replace("'", "/'") %>'
AND coursecode = '<%= Me.cmbCourse.Text %>'),
'<%= Me.txtContact.Text.Replace("'", "/'") %>',
'<%= Me.txtGuardian.Text.Replace("'", "/'") %>')
</sql>

                    SQLExecute(sqlQuery)
                    If result > 0 Then
                        btnCancel1_Click(sender, e)
                        LoadStudent(Me.TextBox2.Text)
                        Messages.PictureBox6.Image = My.Resources.Info_Squared_50px
                        Messages.lblTitle.Text = "Success"
                        Messages.lblMessage.Text = "Record successfully Saved"
                        Messages.ShowDialog()

                        Exit Sub
                    Else
                        'MsgBox("No Record Added", "0 rows affected")
                        'Exit Sub

                    End If
                End If

            Else
                sqlQuery =
<sql>
SELECT * FROM tblstudent WHERE studid = '<%= Me.txtStudentID.Text.Replace("'", "/'") %>'
AND studno != '<%= Key(Me.DGVStudent) %>'
</sql>
                dt = SQLRetrieve(sqlQuery)
                If dt.Rows.Count > 0 Then
                    Messages.PictureBox6.Image = My.Resources.Error_50px
                    Messages.lblTitle.Text = "Exist"
                    Messages.lblMessage.Text = "ID Number " & Me.txtStudentID.Text & " is already exist"
                    Messages.ShowDialog()


                    Exit Sub
                Else
                    sqlQuery =
<sql>
UPDATE tblStudent SET studid = '<%= Me.txtStudentID.Text %>',
lname = '<%= Me.txtLastName.Text.Replace("'", "/'") %>',
mname = '<%= Me.txtMiddleName.Text.Replace("'", "/'") %>',
fname = '<%= Me.txtFirstName.Text.Replace("'", "/'") %>',
gender = '<%= Me.cmbGender.Text %>',
 contact_number = '<%= Me.txtContact.Text %>',
guardian = '<%= Me.txtGuardian.Text.Replace("'", "/'") %>',
courseid = (SELECT courseid FROM tblcourse WHERE currseries = '<%= Me.cmbCurriculum.Text %>'
AND coursecode = '<%= Me.cmbCourse.Text %>') WHERE
studno = '<%= Key(Me.DGVStudent) %>'
</sql>

                    SQLExecute(sqlQuery)
                    If result > 0 Then
                        btnCancel1_Click(sender, e)
                        Messages.PictureBox6.Image = My.Resources.Info_Squared_50px
                        Messages.lblTitle.Text = "Success"
                        Messages.lblMessage.Text = "Record successfully Updated"
                        Messages.ShowDialog()

                        ' MsgBox("Record sucessfully updated", vbInformation, "Success")

                    Else
                        'MsgBox("No rows affected", vbInformation, "Affected")
                        btnCancel1_Click(sender, e)

                    End If
                End If
            End If
        End If


    End Sub
#End Region

    Private Sub btnNew1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew1.Click
        ' Me.btnNew1.BackColor = Color.DodgerBlue
        isActive1(False)
        ClearText(GroupBox1)
        isSave = True
    End Sub

    Private Sub btnNew1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew1.LostFocus
        Me.btnNew1.BackColor = System.Drawing.Color.FromArgb(41, 119, 204)

    End Sub

    Private Sub btnEdit1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit1.Click
        Me.btnEdit1.BackColor = Color.DodgerBlue
        isSave = False
        If Me.DGVStudent.RowCount > 0 Then
            Dim row As Integer = Me.DGVStudent.CurrentRow.Index
            With Me.DGVStudent
                txtStudentID.Text = Me.DGVStudent(1, row).Value.ToString
                txtLastName.Text = Me.DGVStudent("Last Name", row).Value.ToString
                txtFirstName.Text = Me.DGVStudent("First Name", row).Value.ToString
                txtMiddleName.Text = Me.DGVStudent("Middle Name", row).Value.ToString
                cmbGender.Text = Me.DGVStudent("Gender", row).Value.ToString
                cmbCourse.Text = Me.DGVStudent("Course", row).Value.ToString
                cmbCurriculum.Text = Me.DGVStudent("Curriculum", row).Value.ToString
                txtContact.Text = Me.DGVStudent(8, row).Value.ToString
                txtGuardian.Text = Me.DGVStudent(9, row).Value.ToString
                Me.isActive1(False)
                Exit Sub
            End With
        Else
            MsgBox("Select first a record", vbExclamation, "No Record")
        End If
    End Sub

    Private Sub btnEdit1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit1.LostFocus
        Me.btnEdit1.BackColor = System.Drawing.Color.FromArgb(41, 119, 204)

    End Sub
#Region "Highlight Text"
    Private Sub DGVStudent_CellPainting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles DGVStudent.CellPainting
        'If there is no search string, no rows, or nothing in this cell, then get out. 
        On Error Resume Next
        If TextBox2.Text = String.Empty Then Return
        If (e.Value Is Nothing) Then Return
        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Return

        e.Handled = True
        e.PaintBackground(e.CellBounds, True)

        'Get the value of the text in the cell, and the search term. Work with everything in lowercase for more accurate highlighting
        Dim str_SearchTerm As String = TextBox2.Text.Trim.ToLower
        Dim str_CellText As String = DirectCast(e.FormattedValue, String).ToLower

        'Create a list of the character ranges that need to be highlighted. We need to know the start index and the length
        Dim HLRanges As New List(Of CharacterRange)
        Dim SearchIndex As Integer = str_CellText.IndexOf(str_SearchTerm)
        Do Until SearchIndex = -1
            HLRanges.Add(New CharacterRange(SearchIndex, str_SearchTerm.Length))
            SearchIndex = str_CellText.IndexOf(str_SearchTerm, SearchIndex + str_SearchTerm.Length)
        Loop

        ' We also work with the original cell text which is has not been converted to lowercase, else the sizes are incorrect
        str_CellText = DirectCast(e.FormattedValue, String)

        ' Choose your colours. A different colour is used on the currently selected rows
        Dim HLColour As SolidBrush
        If ((e.State And DataGridViewElementStates.Selected) <> DataGridViewElementStates.None) Then
            HLColour = New SolidBrush(Color.DodgerBlue)
        Else
            HLColour = New SolidBrush(Color.MediumSeaGreen)
        End If

        'Loop through all of the found instances and draw the highlight box
        For Each HLRange In HLRanges

            ' Create the rectangle. It should start just underneath the top of the cell, and go to just above the bottom
            Dim HLRectangle As New Rectangle()
            HLRectangle.Y = e.CellBounds.Y + 2
            HLRectangle.Height = e.CellBounds.Height - 5

            ' Determine the size of the text before the area to highlight, and the size of the text to highlight. 
            ' We need to know the size of the text before so that we know where to start the highlight rectangle
            Dim TextBeforeHL As String = str_CellText.Substring(0, HLRange.First)
            Dim TextToHL As String = str_CellText.Substring(HLRange.First, HLRange.Length)
            Dim SizeOfTextBeforeHL As Size = TextRenderer.MeasureText(e.Graphics, TextBeforeHL, e.CellStyle.Font, e.CellBounds.Size)
            Dim SizeOfTextToHL As Size = TextRenderer.MeasureText(e.Graphics, TextToHL, e.CellStyle.Font, e.CellBounds.Size)

            'Set the width of the rectangle, a little wider to make the highlight clearer
            If SizeOfTextBeforeHL.Width > 5 Then
                HLRectangle.X = e.CellBounds.X + SizeOfTextBeforeHL.Width - 6
                HLRectangle.Width = SizeOfTextToHL.Width - 6
            Else
                HLRectangle.X = e.CellBounds.X + 2
                HLRectangle.Width = SizeOfTextToHL.Width - 6
            End If

            'Paint the highlight area
            e.Graphics.FillRectangle(HLColour, HLRectangle)
        Next

        'Paint the rest of the cell as usual
        e.PaintContent(e.CellBounds)

    End Sub
#End Region

#Region "Next Previous"
    Private Sub btnNext1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext1.Click

        scrollVal = scrollVal + 50
        Dim x As Integer = scrollVal

        If (scrollVal > countrow) Then
            scrollVal = x - 50
            MsgBox("No next page anymore", vbExclamation, "No Page")
            Exit Sub
        End If

        ds.Clear()
        adapter.Fill(ds, scrollVal, 50, "tblstudent")
    End Sub

    Private Sub btnPrev1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrev1.Click
        scrollVal = scrollVal - 50

        If scrollVal <= 0 Then

            scrollVal = 0
        End If
        ds.Clear()

        adapter.Fill(ds, scrollVal, 50, "tblstudent")

    End Sub
#End Region

    Private Sub txtSearchSubject_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearchSubject.TextChanged
        LoadSubject(Me.txtSearchSubject.Text)
        If Me.txtSearchSubject.TextLength > 0 Then
            Me.lblSearchSubject.Visible = False
        Else
            Me.lblSearchSubject.Visible = True

        End If
    End Sub

    Private Sub btnDelete1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete1.Click
        If Me.DGVStudent.RowCount > 0 Then
            If MsgBox("Are you sure you want to delete this record", vbQuestion + vbYesNo, "Delete") = vbYes Then
                sqlQuery = "DELETE FROM tblstudent WHERE studno='" & Key(Me.DGVStudent) & "'"
                SQLExecute(sqlQuery)
                If result > 0 Then
                    Me.btnStudentDetails.PerformClick()
                    Messages.PictureBox6.Image = My.Resources.Error_50px
                    Messages.lblTitle.Text = "No Reocord"
                    Messages.lblMessage.Text = "Record Successfully Deleted."
                    Messages.ShowDialog()
                    'MsgBox("Record Successfully Deleted", vbInformation, "Success")
                Else

                End If
                Exit Sub

            Else
                MsgBox("No Record to be Deleted", vbCritical, "No Record")
            End If
        End If
    End Sub

    Private Sub btnCancel1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel1.Click
        ClearText(Me.GroupBox1)
        Me.TextBox2.Clear()
        isActive1(True)
        LoadStudent(Me.TextBox2.Text)
    End Sub

    Private Sub btnNew2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew2.Click
        Me.cmbFilterBy.SelectedIndex = -1
        isSave = True
        isActive2(False)
        ClearText(Me.GBSubjectDetails)
        Me.cmbSubCurriculum.Focus()
    End Sub

    Private Sub btnEdit2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit2.Click
        isSave = False
        If Me.DGVSubjectDetails.RowCount > 0 Then
            Dim row As Integer = Me.DGVSubjectDetails.CurrentRow.Index
            With Me.DGVSubjectDetails
                txtSubCode.Text = Me.DGVSubjectDetails("Subject Code", row).Value.ToString
                cmbSubCurriculum.Text = Me.DGVSubjectDetails("Curriculum", row).Value.ToString
                txtUnits.Text = Me.DGVSubjectDetails("Units", row).Value.ToString
                txtSubjectDescription.Text = Me.DGVSubjectDetails("Description", row).Value.ToString
                cmbYearLevel.Text = Me.DGVSubjectDetails("Year", row).Value.ToString
                cmbSubjectSem.Text = Me.DGVSubjectDetails("Semester", row).Value.ToString
                Me.isActive2(False)
                Exit Sub
            End With
        Else
            Messages.PictureBox6.Image = My.Resources.Error_50px
            Messages.lblTitle.Text = "No Reocord"
            Messages.lblMessage.Text = "Select a record first."
            Messages.ShowDialog()
            ' MsgBox("Select first a record", vbExclamation, "No Record")
        End If

    End Sub

    Private Sub btnCancel2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel2.Click
        ClearText(Me.GBSubjectDetails)
        isActive2(True)
        Me.txtSearchSubject.Clear()
        LoadSubject(Me.txtSearchSubject.Text)
    End Sub

    Private Sub btnSave2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave2.Click
        If Me.cmbSubCurriculum.SelectedIndex = -1 Or cmbSubjectSem.SelectedIndex = -1 Or _
          cmbYearLevel.SelectedIndex = -1 Then
            Messages.PictureBox6.Image = My.Resources.Error_50px
            Messages.lblTitle.Text = "Required"
            Messages.lblMessage.Text = "All fields are required"
            Messages.ShowDialog()

        Else
            If isEmpty(Me.GBSubjectDetails) = True Then
                Messages.PictureBox6.Image = My.Resources.Error_50px
                Messages.lblTitle.Text = "Required"
                Messages.lblMessage.Text = "All fields are required."
                Messages.ShowDialog()
                Exit Sub
            Else

                If isSave = True Then
                    If Me.txtSubCode.Text.Contains("NSTP") Then
                        sqlQuery =
                         <sql>
                    SELECT * FROM tblcoursesub WHERE subdesc = '<%= Me.txtSubjectDescription.Text.Replace("'", "/'") %>'
                    AND SUBCODE = '<%= Me.txtSubCode.Text %>'
                    AND courseid = (SELECT courseid FROM tblcourse WHERE currseries = '<%= cmbSubCurriculum.Text %>')
                    and sem != 'Summer' 
                    </sql>

                        dt = SQLRetrieve(sqlQuery)

                        If dt.Rows.Count > 0 Then
                            Messages.lblMessage.Text = Me.txtSubjectDescription.Text & " (" & Me.txtSubCode.Text & ") " & vbNewLine & _
                                "already exist " & _
                            "in curriculum " & Me.cmbSubCurriculum.Text & ""
                            Messages.lblTitle.Text = "Exist"
                            Messages.btnOkay.Focus()
                            Messages.PictureBox6.Image = My.Resources.Error_50px
                            Messages.ShowDialog()
                        Else
                            Call SaveSubjectDetails()
                        End If

                    ElseIf Me.txtSubjectDescription.Text.Contains("OJT") Then

                        sqlQuery =
                     <sql>
                    SELECT * FROM tblcoursesub WHERE subcode = '<%= Me.txtSubCode.Text %>'
                    AND courseid = (SELECT courseid FROM tblcourse WHERE currseries = '<%= cmbSubCurriculum.Text %>')
                    and sem = 'Summer' and subdesc = '<%= Me.txtSubjectDescription.Text %>'
                    </sql>

                        dt = SQLRetrieve(sqlQuery)

                        If dt.Rows.Count > 0 Then
                            Messages.lblMessage.Text = Me.txtSubjectDescription.Text & "(" & Me.txtSubCode.Text & ") " & vbNewLine & _
                                "already exist " & _
                            "in curriculum " & Me.cmbSubCurriculum.Text & ""
                            Messages.lblTitle.Text = "Exist"
                            Messages.btnOkay.Focus()
                            Messages.PictureBox6.Image = My.Resources.Error_50px
                            Messages.ShowDialog()
                        Else
                            Call SaveSubjectDetails()

                        End If

                    ElseIf Not Me.txtSubCode.Text.Contains("NSTP") And Not Me.txtSubjectDescription.Text.Contains("OJT") Then

                        sqlQuery =
           <sql>
                    SELECT * FROM tblcoursesub WHERE subdesc = '<%= Me.txtSubjectDescription.Text %>'
                    AND courseid = (SELECT courseid FROM tblcourse WHERE currseries = '<%= cmbSubCurriculum.Text %>')
                    and sem != 'Summer'
                    </sql>

                        dt = SQLRetrieve(sqlQuery)
                        If dt.Rows.Count > 0 Then

                            Messages.lblMessage.Text = Me.txtSubjectDescription.Text & vbNewLine & _
                                "already exist " & _
                            "in curriculum " & Me.cmbSubCurriculum.Text & ""
                            Messages.lblTitle.Text = "Exist"
                            Messages.btnOkay.Focus()
                            Messages.PictureBox6.Image = My.Resources.Error_50px
                            Messages.ShowDialog()
                        Else
                            Call SaveSubjectDetails()
                            Exit Sub
                        End If

                    End If
                ElseIf isSave = False Then
                    If Me.txtSubCode.Text.Contains("NSTP") Then

                        sqlQuery =
          <sql>
SELECT * FROM tblcoursesub WHERE subdesc = '<%= Me.txtSubjectDescription.Text.Replace("'", "/'") %>'
AND courseid = (SELECT courseid FROM tblcourse WHERE currseries = '<%= cmbSubCurriculum.Text %>')
 AND csid != '<%= Key(Me.DGVSubjectDetails) %>'  and subcode = '<%= Me.txtSubCode.Text %>'
</sql>
                        dt = SQLRetrieve(sqlQuery)
                        If dt.Rows.Count > 0 Then
                            Messages.lblMessage.Text = Me.txtSubjectDescription.Text & "(" & Me.txtSubCode.Text & ") " & vbNewLine & _
                                 "already exist " & _
                             "in curriculum " & Me.cmbSubCurriculum.Text & ""
                            Messages.lblTitle.Text = "Exist"
                            Messages.btnOkay.Focus()
                            Messages.PictureBox6.Image = My.Resources.Error_50px
                            Messages.ShowDialog()
                        Else
                            Call UpdateSubjectDetails()
                        End If

                    ElseIf Not Me.txtSubCode.Text.Contains("NSTP") And Me.txtSubjectDescription.Text.Contains("OJT") Then
                        sqlQuery =
                        <sql>
                            SELECT * FROM tblcoursesub WHERE subcode = '<%= Me.txtSubCode.Text %>'
                            AND courseid = (SELECT courseid FROM tblcourse WHERE currseries = '<%= cmbSubCurriculum.Text %>')
                            and sem = 'Summer' and csid != '<%= Key(Me.DGVSubjectDetails) %>' and subdesc = '<%= Me.txtSubjectDescription.Text.Replace("'", "/'") %>'
                        </sql>

                        dt = SQLRetrieve(sqlQuery)

                        If dt.Rows.Count > 0 Then
                            Messages.lblMessage.Text = Me.txtSubjectDescription.Text & "(" & Me.txtSubCode.Text & ") " & vbNewLine & _
                                "already exist " & _
                            "in curriculum " & Me.cmbSubCurriculum.Text & ""
                            Messages.lblTitle.Text = "Exist"
                            Messages.btnOkay.Focus()
                            Messages.PictureBox6.Image = My.Resources.Error_50px
                            Messages.ShowDialog()
                        Else
                            Call UpdateSubjectDetails()

                        End If

                    ElseIf Not Me.txtSubCode.Text.Contains("NSTP") And Not Me.txtSubjectDescription.Text.Contains("OJT") Then

                        sqlQuery =
                     <sql>
                        SELECT * FROM tblcoursesub WHERE subdesc = '<%= Me.txtSubjectDescription.Text %>'
                        AND courseid = (SELECT courseid FROM tblcourse WHERE currseries = '<%= cmbSubCurriculum.Text %>')
                        and sem != 'Summer' and csid != '<%= Key(Me.DGVSubjectDetails) %>'
                    </sql>

                        dt = SQLRetrieve(sqlQuery)
                        If dt.Rows.Count > 0 Then

                            Messages.lblMessage.Text = Me.txtSubjectDescription.Text & vbNewLine & _
                                "already exist " & _
                            "in curriculum " & Me.cmbSubCurriculum.Text & ""
                            Messages.lblTitle.Text = "Exist"
                            Messages.btnOkay.Focus()
                            Messages.PictureBox6.Image = My.Resources.Error_50px
                            Messages.ShowDialog()

                        Else
                            Call UpdateSubjectDetails()

                        End If
                    End If
                End If
            End If
        End If
    End Sub



    Private Sub btnNew3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew3.Click
        isSave = True
        isActive3(False)
        ClearText(Me.PrerequisiteTab)

    End Sub

    Private Sub btnEdit3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit3.Click

        isSave = False
        If Me.DGVPrerequiste.RowCount > 0 Then
            With Me.DGVPrerequiste
                'Dim row As Integer = .CurrentRow.Index
                'Me.cmbPrereqCurriculum.Text = Me.DGVPrerequiste("Curriculum", row).Value.ToString
                'Me.cmbPrereqSubCode.Text = Me.DGVPrerequiste("Subject Code", row).Value.ToString
                'Me.txtPreReqSubDesc.Text = Me.DGVPrerequiste("Subject Description", row).Value.ToString
                'Me.cmbPrerequisiteCode.Text = Me.DGVPrerequiste("Prerequisite", row).Value.ToString
                'Me.txtPrereqDesc.Text = Me.DGVPrerequiste("Prerequisite Description", row).Value.ToString
                'Me.cmbSubjectYearLevel.Text = Me.DGVPrerequiste("Year Level", row).Value.ToString
                'Me.cmbSubjSemester.Text = Me.DGVPrerequiste("Semester", row).Value.ToString
                '' sqlQuery = "SELECT sem,year from tblcoursesub where"
                '' dt = SQLRetrieve(sqlQuery)
                'Me.cmbPrereqYearLevel.Text = Me.DGVPrerequiste(1, row).Value.ToString
                ''  Me.cmbPrereqSemester.Text = Me.DGVPrerequiste(2, row).Value.ToString

                isActive3(False)
                isSave = False

                Exit Sub
            End With
        Else
            '
        End If

    End Sub

    Private Sub btnCancel3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel3.Click
        ClearText(SubjectPanel)
        ClearText(PrerequisitePanel)
        ClearText(PrerequisiteTab)
        loadPrerequisteSubject()
        Me.isActive3(True)
    End Sub

    Private Sub btnSave3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave3.Click
        If isEmpty(SubjectPanel) = True Then
            Messages.PictureBox6.Image = My.Resources.Error_50px
            Messages.lblTitle.Text = "Required"
            Messages.lblMessage.Text = "All fields are required."
            Messages.ShowDialog()
            Exit Sub
        Else

            If isEmpty(PrerequisitePanel) = True Then
                Messages.PictureBox6.Image = My.Resources.Error_50px
                Messages.lblTitle.Text = "Required"
                Messages.lblMessage.Text = "All fields are required."
                Messages.ShowDialog()

                '  MsgBox("Don't Leave an Empty Fields", vbExclamation)
                Exit Sub
                'ElseIf Me.txtPrereqDesc.Text = Me.txtPreReqSubDesc.Text Then
                '    Messages.PictureBox6.Image = My.Resources.Close_Window_50px
                '    Messages.lblTitle.Text = "Error"
                '    Messages.lblMessage.Text = "Must not the same subject."
                '    Messages.ShowDialog()
                Exit Sub
            Else
                'If isSave = True Then
                '    sqlQuery = "SELECT * FROM tblprerequisite WHERE PrerequisiteSubjectID IN " & _
                '        " (SELECT csid FROM tblcoursesub WHERE subcode = '" & _
                '        Me.cmbPrerequisiteCode.Text & "' AND subdesc = '" & _
                '        Me.txtPrereqDesc.Text & "' and courseid IN (SELECT courseid FROM tblcourse " & _
                '        "WHERE currseries = '" & Me.cmbPrereqCurriculum.Text & "')) AND " & _
                '        "subjectid IN (SELECT csid FROM tblcoursesub WHERE subcode = '" & Me.cmbPrereqSubCode.Text & "')"

                '    dt = SQLRetrieve(sqlQuery)
                '    If dt.Rows.Count > 0 Then
                '        MsgBox(Me.cmbPrerequisiteCode.Text & " pre-prequisite of " & Me.cmbPrereqSubCode.Text & _
                '               " curriculum " & cmbPrereqCurriculum.Text & " is already exist in the database", vbExclamation, "Already")
                '        Exit Sub
                '    Else
                '        sqlQuery = "INSERT INTO tblprerequisite (subjectid,prerequisitesubjectid,courseid,subcode,prerequisitecode) VALUES " & _
                '            "((" & subjid & ")," & _
                '            "(" & prereqid & "),(SELECT courseid " & _
                '            " FROM tblcourse WHERE currseries = '" & Me.cmbPrereqCurriculum.Text & "'),'" & Me.cmbPrereqSubCode.Text & "','" & Me.cmbPrerequisiteCode.Text & "')"

                '        SQLExecute(sqlQuery)
                '        ClearText(Me.DGVPrerequiste)

                '        Messages.PictureBox6.Image = My.Resources.Info_Squared_50px
                '        Messages.lblTitle.Text = "Error"
                '        Messages.lblMessage.Text = "Successfully Saved."
                '        Messages.ShowDialog()

                '        Exit Sub

                '    End If
                'Else
                '    sqlQuery = "SELECT * FROM tblprerequisite WHERE PrerequisiteSubjectID IN " & _
                '      " (SELECT csid FROM tblcoursesub WHERE subcode = '" & _
                '      Me.cmbPrerequisiteCode.Text & "' AND subdesc = '" & _
                '      Me.txtPrereqDesc.Text & "' and courseid IN (SELECT courseid FROM tblcourse " & _
                '      "WHERE currseries = '" & Me.cmbPrereqCurriculum.Text & "')) AND " & _
                '      "subjectid IN (SELECT csid FROM tblcoursesub WHERE subcode = '" & Me.cmbPrereqSubCode.Text & _
                '      "') and Pid != '" & Key(Me.DGVPrerequiste) & "'"

                '    dt = SQLRetrieve(sqlQuery)
                '    If dt.Rows.Count > 0 Then
                '        MsgBox(Me.cmbPrerequisiteCode.Text & " pre-prequisite of " & Me.cmbPrereqSubCode.Text & _
                '               " curriculum " & cmbPrereqCurriculum.Text & " is already exist in the database", vbExclamation, "Already")
                '        Exit Sub
                '    Else
                '        sqlQuery = "UPDATE tblprerequisite SET subjectid = (SELECT csid FROM tblcoursesub WHERE subcode = '" & _
                '            Me.cmbPrereqSubCode.Text & "' and courseid IN (SELECT courseid FROM tblcourse WHERE currseries = '" & _
                '            Me.cmbPrereqCurriculum.Text & "')),prerequisitesubjectid = (SELECT csid FROM tblcoursesub WHERE subcode = '" & Me.cmbPrerequisiteCode.Text & _
                '            "' AND subdesc = '" & Me.txtPrereqDesc.Text & "' and courseid IN (SELECT courseid FROM tblcourse WHERE currseries = '" & Me.cmbPrereqCurriculum.Text & _
                '            "')),courseid = (SELECT courseid " & _
                '            " FROM tblcourse WHERE currseries = '" & Me.cmbPrereqCurriculum.Text & _
                '            "'),subcode = '" & Me.cmbPrereqSubCode.Text & "',prerequisitecode = '" & Me.cmbPrerequisiteCode.Text & "' WHERE pid = '" & Key(Me.DGVPrerequiste) & "'"
                '        SQLExecute(sqlQuery)
                '        btnCancel3_Click(sender, e)

                '        Messages.PictureBox6.Image = My.Resources.Info_Squared_50px
                '        Messages.lblTitle.Text = "Error"
                '        Messages.lblMessage.Text = "Successfully updated"
                '        Messages.ShowDialog()

            End If
        End If
        '    End If
        'End If
    End Sub

    Private Sub cmbPrereqCurriculum_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Call Prerequiste()
        'Call SubjectCode()
    End Sub

    Private Sub cmbEnrollSem_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbEnrollSem.SelectedIndexChanged
        Me.CheckBox1.Checked = False
        If Me.txtEnrollStudID.Text <> "" Then
            EnrolledSubject()
            SubjectList()
        End If
    End Sub

#Region "Enroll Subject"
    Private Sub btnAddSubject_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddSubject.Click
        If Me.cmbEnrollSem.Text = "" And Me.cmbEnrollSem.SelectedIndex = -1 Then
            ' MsgBox("Select a semester", vbExclamation)
            Messages.lblMessage.Text = "Select a semester first!"
            Messages.PictureBox6.Image = My.Resources.Close_Window_50px
            Messages.lblTitle.Text = "Select"
            Messages.Show()
            Exit Sub
        End If
        If Me.txtEnrollStudID.Text = "" Then
            Messages.lblMessage.Text = "Select a student first!"
            Messages.PictureBox6.Image = My.Resources.Close_Window_50px
            Messages.lblTitle.Text = "Select"
            Messages.Show()
            Exit Sub
        End If
        For x = 0 To Me.DGVSubjectList.Rows.Count - 1

            'check if selected subject has already enrolled in the same school year and the same semester
            sqlQuery =
<sql>
select subdesc from tblensub where year = '<%= Me.txtSY.Text %>'
and studid = '<%= Me.txtEnrollStudID.Text %>'
and sem = '<%= Me.cmbEnrollSem.Text %>' and year = '<%= Me.txtSY.Text %>'
and subdesc = '<%= Me.DGVSubjectList.Item(2, x).Value.ToString %>'
</sql>
            dt2 = New DataTable
            dt2 = SQLRetrieve(sqlQuery)


            If Me.DGVSubjectList.Item(0, x).Value = True Then 'check if the subject if selected

                Dim PrerequisiteCount As Integer
                sqlQuery =
<Sql>
select * from tblprerequisite where subjectid  = '<%= Me.DGVSubjectList.Item(4, x).Value.ToString %>'
</Sql>

                dt = SQLRetrieve(sqlQuery)
                PrerequisiteCount = Val(dt.Rows.Count) 'determine how many prerequisite subject

                If PrerequisiteCount > 0 Then 'if have prerequsite then

                    'check if all prerequsite subject are enrolled and passed

                    sqlQuery =
    <sql>
select subjectid,Subjcode,SubjDesc,b.subcode as `Prerequisite`,subdesc as `PrerequisiteDesc`
 from  tblprerequisite a
left join (select csid,courseid,subcode,subdesc from tblcoursesub ) b
on a.prerequisitesubjectid  = b.csid 
left join (select csid,courseid,subcode as `SubjCode`,subdesc as `SubjDesc` from tblcoursesub) c
on a.subjectid  = c.csid 
where subdesc in (select subdesc from tblensub
where grade  in ('1.0','1.25','1.5','1.75','2.0','2.25','2.5','2.75','3.0') 
and studid = '<%= Me.txtEnrollStudID.Text %>')
and subjectid = '<%= Me.DGVSubjectList.Item(4, x).Value.ToString %>' 
</sql>

                    '                    sqlQuery =
                    '<sql>
                    'SELECT a.`courseid`, a.`subcode`, a.`subdesc` FROM `tblcoursesub` a
                    'left join (SELECT `SubjectID`, `subcode`, `PrerequisiteSubjectID`, `PrerequisiteCode`, `CourseID`
                    'FROM `tblprerequisite`) b ON a.csid = b.PrerequisiteSubjectID 
                    'left join (SELECT `studid`, `subcode`, `subdesc`, `grade` FROM `tblensub`) c
                    'on a.subdesc = c.subdesc where studid = '<%= Me.txtEnrollStudID.Text %>' and 
                    'c.grade  IN ('1.0'',1.25','1.5','1.75','2.0','2.25','2.5','2.75','3.0')
                    'and b.subcode = '<%= Me.DGVSubjectList.Item(1, x).Value.ToString %>'
                    '</sql>

                    dt1 = SQLRetrieve(sqlQuery)

                    If dt1.Rows.Count > 0 And dt1.Rows.Count <> PrerequisiteCount Then 'kapag hindi nya lahat naipasa ung pre-requisite subject

                        MsgBox("You Need to pass the pre-requisite subject of " & _
                               Me.DGVSubjectList.Item(1, x).Value.ToString, vbExclamation)
                        Me.DGVSubjectList(0, x).Value = False
                        Exit Sub
                    ElseIf dt1.Rows.Count = 0 Then 'pag wala pa syang naipasang prerequisite subject
                        Messages.PictureBox6.Image = My.Resources.Error_50px
                        Messages.lblTitle.Text = "Prerequisite"
                        Messages.btnOkay.Focus()
                        Messages.lblMessage.Text = "You Need to pass the pre-requisite subject of " & _
                            Me.DGVSubjectList.Item(1, x).Value.ToString

                        Me.DGVSubjectList(0, x).Value = False
                        Messages.ShowDialog()

                        Exit Sub 'to ignore other codes or next codes below


                    Else
                        If dt2.Rows.Count > 0 Then
                            Messages.PictureBox6.Image = My.Resources.Error_50px
                            Messages.lblTitle.Text = "Prerequisite"
                            Messages.btnOkay.Focus()
                            Messages.lblMessage.Text = "You Need to pass the pre-requisite subject of " & _
                                Me.DGVSubjectList.Item(1, x).Value.ToString

                            Me.DGVSubjectList(0, x).Value = False
                            Messages.ShowDialog()


                            Me.DGVSubjectList(0, x).Value = False
                        Else
                            sqlQuery = "INSERT INTO tblensub (`year`, `sem`, `studid`, `subcode`, `subdesc`, `units`,`currseries`)" & _
                                           "VALUES ('" & Me.txtSY.Text.Replace(" ", "") & "','" & Me.cmbEnrollSem.Text & "','" & Me.txtEnrollStudID.Text & _
                                           "','" & Me.DGVSubjectList(1, x).Value.ToString & _
                                           "','" & Me.DGVSubjectList(2, x).Value.ToString & "','" & Me.DGVSubjectList(3, x).Value.ToString & _
                                           "','" & Me.txtEnrollCurriculum.Text & "')"

                            SQLExecute(sqlQuery)
                            Me.DGVSubjectList(0, x).Value = False
                            EnrolledSubject()
                        End If

                    End If

                Else
                    If dt2.Rows.Count > 0 Then 'check if selected subject has already 
                        'enrolled in the same school year and the same semester

                        Messages.PictureBox6.Image = My.Resources.Error_50px
                        Messages.lblTitle.Text = "Already"
                        Messages.btnOkay.Focus()
                        Messages.lblMessage.Text = Me.DGVSubjectList.Item(2, x).Value.ToString & " is already added."
                        Messages.ShowDialog()

                        Me.DGVSubjectList(0, x).Value = False

                    Else
                        sqlQuery = "INSERT INTO tblensub (`year`, `sem`, `studid`, `subcode`, `subdesc`, `units`,`currseries`)" & _
                                                "VALUES ('" & Me.txtSY.Text.Replace(" ", "") & "','" & Me.cmbEnrollSem.Text & "','" & Me.txtEnrollStudID.Text & _
                                                "','" & Me.DGVSubjectList(1, x).Value.ToString & _
                                                "','" & Me.DGVSubjectList(2, x).Value.ToString & "','" & Me.DGVSubjectList(3, x).Value.ToString & _
                                                "','" & Me.txtEnrollCurriculum.Text & "')"

                        SQLExecute(sqlQuery)
                        Me.DGVSubjectList(0, x).Value = False
                        EnrolledSubject()

                    End If
                End If
            End If
        Next
        Me.CheckBox1.Checked = False
        SubjectList()
    End Sub
#End Region

    Private Sub DGVSubjectList_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DGVSubjectList.CellClick
        Try
            If e.ColumnIndex = 0 Then
                If Me.DGVSubjectList(0, e.RowIndex).Value = False Then

                    Me.DGVSubjectList(0, e.RowIndex).Value = True
                Else
                    Me.DGVSubjectList(0, e.RowIndex).Value = False

                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub cmbFilterSem_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbFilterSem.SelectedIndexChanged
        SubjectList()

    End Sub

    Private Sub cmbFilterYear_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbFilterYear.SelectedIndexChanged
        SubjectList()

    End Sub

    Private Sub txtSearchSubjectList_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearchSubjectList.TextChanged
        On Error Resume Next
        If Me.txtSearchSubjectList.TextLength > 0 Then
            Me.lblSearchSubjectList.Visible = False
        Else
            Me.lblSearchSubjectList.Visible = True
        End If
        SubjectList()


    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        If Me.DGVEnrolledSubject.RowCount > 0 Then
            If MsgBox("Are you sure you want to drop this subject?", vbQuestion + vbYesNo) = vbYes Then
                Dim row As Integer = Me.DGVEnrolledSubject.CurrentRow.Index

                If Me.DGVEnrolledSubject.Item("Grade", row).Value.ToString <> "" Then
                    MsgBox("Unable to drop. this subject's grade is not null", vbCritical)
                Else

                    sqlQuery =
<sql>
DELETE FROM tblensub WHERE ensubid = '<%= Key(Me.DGVEnrolledSubject) %>'
</sql>
                    result = SQLExecute(sqlQuery)

                    If result > 0 Then
                        EnrolledSubject()
                        MsgBox("Subject Successfully Dropped", vbInformation)
                    Else
                        'do nothing
                    End If

                End If
            End If
        Else
            MsgBox("No subect to be dropped", vbCritical)

        End If
    End Sub


    Private Sub txtFirstName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) _
        Handles txtFirstName.KeyPress, txtMiddleName.KeyPress _
        , txtLastName.KeyPress, txtNameofUser.KeyPress, txtCurrDesc.KeyPress, txtCourseDesc.KeyPress

        LettersOnly(sender, e)

    End Sub

    Private Sub DGVGrades_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs)
        tempgrade = Me.DGVGrades(e.ColumnIndex, e.RowIndex).Value.ToString
        row = e.RowIndex

    End Sub

    Private Sub DGVGrades_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)



    End Sub

    Private Sub DGVGrades_ColumnHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs)
        SubjectGrade()
    End Sub
    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If Me.CheckBox1.Checked = True Then
            For x = 0 To Me.DGVSubjectList.RowCount - 1
                Me.DGVSubjectList(0, x).Value = True
            Next
        Else
            For x = 0 To Me.DGVSubjectList.RowCount - 1
                Me.DGVSubjectList(0, x).Value = False
            Next
        End If
    End Sub

    Private Sub btnInputGrade_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInputGrade.Click
        Me.txtGradeStudentID.Text = Me.txtEnrollStudID.Text
        Me.txtGradeCurriculum.Text = Me.txtEnrollCurriculum.Text
        Me.TabControl1.SelectTab(GradeEntryTab)
    End Sub

    Private Sub cmbPrereqSubCode_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'SubjectCode()
    End Sub

    Private Sub cmbPrereqSubCode_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'If Me.cmbPrereqSubCode.SelectedIndex <> -1 Then
        '    sqlQuery = "SELECT csid,subdesc FROM tblcoursesub WHERE subcode = '" & Me.cmbPrereqSubCode.Text & _
        '        "' And courseid = (select courseid from tblcourse where  currseries = '" & Me.cmbPrereqCurriculum.Text & "')"
        '    dt = SQLRetrieve(sqlQuery)
        '    If dt.Rows.Count > 0 Then
        '        Me.txtPreReqSubDesc.Text = dt.Rows(0).Item(1).ToString
        '        subjid = dt.Rows(0).Item(0)
        '    Else
        '        Me.txtPreReqSubDesc.Clear()
        '    End If
        'Else
        '    Me.txtPreReqSubDesc.Clear()
        'End If
    End Sub


    Private Sub cmbPrerequisiteCode_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ' Prerequiste()
    End Sub

    Private Sub cmbPrerequisiteCode_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'If Me.cmbPrerequisiteCode.SelectedIndex <> -1 Then
        '    sqlQuery = "SELECT csid,subdesc FROM tblcoursesub WHERE subcode = '" & Me.cmbPrerequisiteCode.Text & _
        '        "' And courseid = (select courseid from tblcourse where  currseries = '" & Me.cmbPrereqCurriculum.Text & "')"
        '    dt = SQLRetrieve(sqlQuery)
        '    If dt.Rows.Count > 0 Then
        '        Me.txtPrereqDesc.Text = dt.Rows(0).Item(1).ToString
        '        prereqid = dt.Rows(0).Item(0)
        '    Else
        '        Me.txtPrereqDesc.Clear()
        '    End If
        'Else
        '    Me.txtPrereqDesc.Clear()
        'End If
    End Sub

    Private Sub DGVGrades_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs)



    End Sub

    Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button12.Click
        YesNoMessage.PictureBox6.Image = My.Resources.Help_50px
        YesNoMessage.lblTitle.Text = "Exit"
        YesNoMessage.lblMessage.Text = "Program will close. Continue?"
       
        YesNoMessage.ShowDialog()
    End Sub

    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
        Me.Hide()
        FrmLogin.Show()
    End Sub

    Private Sub btnSaveGrade_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveGrade.Click
        If Me.DGVGrades.RowCount = 0 Then
            Messages.PictureBox6.Image = My.Resources.Close_Window_50px
            Messages.lblTitle.Text = "Select"
            Messages.btnOkay.Focus()
            Messages.lblMessage.Text = "No grade to be save"
            Messages.ShowDialog()
            Exit Sub
        End If

        For i = 0 To Me.DGVGrades.RowCount - 1
            sqlQuery = "SELECT Grade from tblensub where ensubid ='" & Me.DGVGrades(0, i).Value.ToString & _
                "' and grade != '" & Me.DGVGrades(3, i).Value.ToString & "' and grade !=''"
            dt = SQLRetrieve(sqlQuery)
            If dt.Rows.Count > 0 Then
                isChange = True
                Exit For
            Else
                isChange = False
            End If
        Next
        If isChange = False Then
            For x = 0 To Me.DGVGrades.RowCount - 1
                sqlQuery = "UPDATE tblensub SET grade = '" & Me.DGVGrades(3, x).Value.ToString & _
                         "' WHERE ensubid = '" & Me.DGVGrades(0, x).Value.ToString & "'"
                SQLExecute(sqlQuery)
            Next
            SubjectGrade()
            Messages.PictureBox6.Image = My.Resources.Info_Squared_50px
            Messages.lblMessage.Text = "Grade Successfully Saved"
            Messages.lblTitle.Text = "Grade"
            Messages.btnOkay.Focus()
            Messages.ShowDialog()

        Else
            AdminPassword.ShowDialog()
        End If


    End Sub


    Private Sub cmbGradeSem_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbGradeSem.SelectedIndexChanged
        SubjectGrade()
    End Sub

    Private Sub cmbGradeSY_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbGradeSY.SelectedIndexChanged
        SubjectGrade()

    End Sub

    Private Sub txtSearchGrade_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearchGrade.TextChanged
        If Me.txtSearchGrade.TextLength > 0 Then
            Me.Label55.Visible = False
        Else
            Me.Label55.Visible = True

        End If
        SubjectGrade()
    End Sub


    Private Sub cmbFilterBy_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbFilterBy.SelectedIndexChanged
        LoadSubject(Me.txtSearchSubject.Text)
    End Sub

    Private Sub txtSearchPrereq_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearchPrereq.TextChanged
        If Me.txtSearchPrereq.TextLength > 0 Then
            Label32.Visible = False
        Else
            Label32.Visible = True

        End If
        loadPrerequisteSubject()
    End Sub

    Private Sub DGVGrades_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)

    End Sub

    Private Sub txtEnrollStudID_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtEnrollStudID.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtEnrollStudID_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEnrollStudID.SelectedIndexChanged

    End Sub


    Private Sub txtEnrollStudID_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEnrollStudID.TextChanged
        SubjectList()
        If Me.txtEnrollStudID.Text <> "" Then
            SubjectList()

            sqlQuery = "SELECT CONCAT(fname,' ',SUBSTRING(mname,1,1),'. ',lname) as `Name`,currseries FROM tblstudent a " & _
                "LEFT JOIN (SELECT courseid,currseries FROM tblcourse) b ON a.courseid = b.courseid WHERE studid= '" & Me.txtEnrollStudID.Text & "'"
            dt = SQLRetrieve(sqlQuery)
            If dt.Rows.Count > 0 Then
                Me.txtStudName.Text = dt.Rows(0).Item(0).ToString
                Me.txtEnrollCurriculum.Text = dt.Rows(0).Item(1).ToString

            Else
                Me.txtStudName.Clear()
                Me.txtEnrollCurriculum.Clear()

            End If
        Else
            Me.txtStudName.Clear()
            Me.txtEnrollCurriculum.Clear()

        End If
    End Sub

    Private Sub txtSY_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSY.SelectedIndexChanged
        Me.CheckBox1.Checked = False
        If Me.txtEnrollStudID.Text <> "" Then
            EnrolledSubject()
        End If
    End Sub


    Private Sub btnDelete2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete2.Click
        If Me.DGVSubjectDetails.RowCount > 0 Then

            YesNoMessage.PictureBox6.Image = My.Resources.Error_50px
            YesNoMessage.lblTitle.Text = "Delete"
            YesNoMessage.lblMessage.Text = "Are you sure you want to delete this subject"
            YesNoMessage.ShowDialog()

        Else


            Messages.PictureBox6.Image = My.Resources.Close_Window_50px
            Messages.lblTitle.Text = "No Reocord"
            Messages.lblMessage.Text = "No subject to be deleted"
            Messages.ShowDialog()
        End If
    End Sub

    Private Sub btnDelete3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete3.Click
        If Me.DGVSubjectDetails.RowCount > 0 Then

            YesNoMessage.PictureBox6.Image = My.Resources.Error_50px
            YesNoMessage.lblTitle.Text = "Delete"
            YesNoMessage.lblMessage.Text = "Are you sure you want to delete this prerequisite"
            YesNoMessage.ShowDialog()

        Else


            Messages.PictureBox6.Image = My.Resources.Close_Window_50px
            Messages.lblTitle.Text = "No Reocord"
            Messages.lblMessage.Text = "No record to be deleted"
            Messages.ShowDialog()
        End If
    End Sub


    Private Sub btnNew4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew4.Click
        isSave = True
        isActive4(False)
        ClearText(CurriculumEntryPanel)
    End Sub

    Private Sub btnCancel4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel4.Click
        isActive4(True)
        ClearText(CurriculumEntryPanel)
        LoadCurriculumSeries()
    End Sub

    Private Sub btnSave4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave4.Click
        If isEmpty(Me.CurriculumEntryPanel) = True Then
            Messages.PictureBox6.Image = My.Resources.Error_50px
            Messages.lblTitle.Text = "Required"
            Messages.btnOkay.Focus()
            Messages.lblMessage.Text = "All Fields are required"
            Messages.ShowDialog()
        Else
            If isSave = True Then
                sqlQuery = "select currseries from tblcourse where coursedesc = '" & _
                    Me.txtCourseDesc.Text.Replace("'", "/'") & "' and currseries = '" & Me.txtCurrSeries.Text & "'"
                dt = SQLRetrieve(sqlQuery)
                If dt.Rows.Count > 0 Then
                    Messages.PictureBox6.Image = My.Resources.Error_50px
                    Messages.lblTitle.Text = "Exist"
                    Messages.btnOkay.Focus()
                    Messages.lblMessage.Text = "This Curriculum is already exist"
                    Messages.ShowDialog()
                    Exit Sub
                Else
                    sqlQuery = "INSERT INTO tblcourse (coursecode,coursedesc,currseries,curriculumdesc) " & _
                        "VALUES  ('" & Me.txtCourseCode.Text & _
                        "','" & Me.txtCourseDesc.Text & _
                        "','" & Me.txtCurrSeries.Text & _
                        "','" & Me.txtCurrDesc.Text & "')"

                    result = SQLExecute(sqlQuery)
                    If result > 0 Then
                        Me.btnCancel4_Click(sender, e)
                        Messages.PictureBox6.Image = My.Resources.Info_Squared_50px
                        Messages.lblTitle.Text = "Success"
                        Messages.btnOkay.Focus()
                        Messages.lblMessage.Text = "New Curriculum has been added"
                        Messages.ShowDialog()
                        Exit Sub
                    Else
                        Me.btnCancel4_Click(sender, e)
                    End If
                End If
            Else
                sqlQuery = "select currseries from tblcourse where coursedesc = '" & _
                  Me.txtCourseDesc.Text.Replace("'", "/'") & "' and currseries = '" & Me.txtCurrSeries.Text & _
                  "' and courseid != '" & Key(Me.DGVCurriculum) & "'"
                dt = SQLRetrieve(sqlQuery)
                If dt.Rows.Count > 0 Then

                    Messages.PictureBox6.Image = My.Resources.Error_50px
                    Messages.lblTitle.Text = "Exist"
                    Messages.btnOkay.Focus()
                    Messages.lblMessage.Text = "This Curriculum is already exist"
                    Messages.ShowDialog()
                    Exit Sub
                Else

                    sqlQuery = "UPDATE tblcourse SET currseries='" & Me.txtCurrSeries.Text & _
                        "',coursecode = '" & Me.txtCourseCode.Text & _
                        "',coursedesc = '" & Me.txtCourseDesc.Text & _
                        "',curriculumdesc = '" & Me.txtCurrDesc.Text & _
                        "' WHERE courseid = '" & Key(Me.DGVCurriculum) & "'"
                    result = SQLExecute(sqlQuery)
                    If result > 0 Then
                        Me.btnCancel4_Click(sender, e)
                        Messages.PictureBox6.Image = My.Resources.Info_Squared_50px
                        Messages.lblTitle.Text = "Success"
                        Messages.btnOkay.Focus()
                        Messages.lblMessage.Text = "Curriculum has successfully changed"
                        Messages.ShowDialog()
                    Else
                        Me.btnCancel4_Click(sender, e)
                    End If
                End If
            End If
        End If

    End Sub

    Private Sub btnEdit4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit4.Click
        isSave = False
        isActive4(False)
        If Me.DGVCurriculum.RowCount > 0 Then
            Dim row As Integer = Me.DGVCurriculum.CurrentRow.Index
            Me.txtCourseCode.Text = Me.DGVCurriculum(1, row).Value.ToString
            Me.txtCourseDesc.Text = Me.DGVCurriculum(2, row).Value.ToString
            Me.txtCurrSeries.Text = Me.DGVCurriculum(3, row).Value.ToString
            Me.txtCurrDesc.Text = Me.DGVCurriculum(4, row).Value.ToString
        Else
            Messages.PictureBox6.Image = My.Resources.Close_Window_50px
            Messages.lblTitle.Text = "No Reocord"
            Messages.btnOkay.Focus()
            Messages.lblMessage.Text = "No record to be edit."
            Messages.ShowDialog()
        End If

    End Sub

    Private Sub btnDatabase_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDatabase.Click
        DefaultBGColor(Me.Panel3)
        Me.TabControl1.SelectTab(DBConfigTab)
        Me.btnDatabase.BackColor = System.Drawing.Color.FromArgb(79, 89, 97)
    End Sub

    Private Sub btnNew5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew5.Click
        isSave = True
        ClearText(UserEntryPanel)
        isActive5(False)
    End Sub

    Private Sub btnCancel5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel5.Click
        LoadUsers()
        ClearText(UserEntryPanel)
        isActive5(True)
    End Sub

    Private Sub btnEdit5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit5.Click
        isSave = False
        isActive5(False)
        If Me.DGVUsersList.RowCount > 0 Then
            Dim row As Integer = Me.DGVUsersList.CurrentRow.Index
            Me.txtUserName.Text = Me.DGVUsersList(1, row).Value.ToString
            Me.txtUserPassword.Text = Me.DGVUsersList(2, row).Value.ToString
            Me.txtUserRole.Text = Me.DGVUsersList(3, row).Value.ToString
            Me.txtNameofUser.Text = Me.DGVUsersList(4, row).Value.ToString
        Else
            Messages.PictureBox6.Image = My.Resources.Close_Window_50px
            Messages.lblTitle.Text = "No Reocord"
            Messages.btnOkay.Focus()
            Messages.lblMessage.Text = "No record to be edit."
            Messages.ShowDialog()
        End If


    End Sub

    Private Sub btnDelete5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete5.Click
        If Me.DGVUsersList.RowCount > 0 Then
            If Me.DGVUsersList("Usertype", Me.DGVUsersList.CurrentRow.Index).Value.ToString = "Administrator" Then
                Messages.PictureBox6.Image = My.Resources.Close_Window_50px
                Messages.lblTitle.Text = "Account"
                Messages.lblMessage.Text = "Sorry, Administrator account can't be deleted"
                Messages.ShowDialog()
                ' MsgBox("Sorry, Administrator account can't be deleted")
                Exit Sub
            Else
                YesNoMessage.PictureBox6.Image = My.Resources.Error_50px
                YesNoMessage.lblTitle.Text = "Delete"
                YesNoMessage.btnOkay.Focus()
                YesNoMessage.lblMessage.Text = "Are you sure you want to delete this user"
                YesNoMessage.ShowDialog()

            End If


        Else

            Messages.PictureBox6.Image = My.Resources.Close_Window_50px
            Messages.lblTitle.Text = "No Reocord"
            Messages.lblMessage.Text = "No record to be deleted"
            Messages.ShowDialog()
        End If
    End Sub

    Private Sub btnSave5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave5.Click
        If isEmpty(Me.UserEntryPanel) = True Then
            Messages.PictureBox6.Image = My.Resources.Error_50px
            Messages.lblTitle.Text = "Required"
            Messages.btnOkay.Focus()
            Messages.lblMessage.Text = "All Fields are required"
            Messages.ShowDialog()
        Else
            If isSave = True Then
                sqlQuery = "select currseries from tblcourse where coursedesc = '" & _
                    Me.txtCourseDesc.Text.Replace("'", "/'") & "' and currseries = '" & Me.txtCurrSeries.Text & "'"
                dt = SQLRetrieve(sqlQuery)
                If dt.Rows.Count > 0 Then
                    Messages.PictureBox6.Image = My.Resources.Error_50px
                    Messages.lblTitle.Text = "Exist"
                    Messages.btnOkay.Focus()
                    Messages.lblMessage.Text = "This username is already exist"
                    Messages.ShowDialog()
                    Exit Sub
                Else
                    sqlQuery = "INSERT INTO tbladmin (username,password,usertype,name) " & _
                        " VALUES  ('" & Me.txtUserName.Text & _
                        "','" & Me.txtUserPassword.Text & _
                        "','" & Me.txtUserRole.Text & _
                        "','" & Me.txtNameofUser.Text.Replace("'", "/'") & "')"

                    result = SQLExecute(sqlQuery)

                    If result > 0 Then
                        Me.btnCancel5_Click(sender, e)
                        Messages.PictureBox6.Image = My.Resources.Info_Squared_50px
                        Messages.lblTitle.Text = "Success"
                        Messages.btnOkay.Focus()
                        Messages.lblMessage.Text = "New user successfully added"
                        Messages.ShowDialog()
                        Exit Sub
                    Else
                        Me.btnCancel5_Click(sender, e)
                    End If
                End If
            ElseIf isSave = False Then
                sqlQuery = "Select * from tbladmin where username = '" & Me.txtUserName.Text & "' and userid != '" & Key(Me.DGVUsersList) & "'"
                dt = SQLRetrieve(sqlQuery)
                If dt.Rows.Count > 0 Then

                    Messages.PictureBox6.Image = My.Resources.Error_50px
                    Messages.lblTitle.Text = "Exist"
                    Messages.btnOkay.Focus()
                    Messages.lblMessage.Text = "This username is already exist"
                    Messages.ShowDialog()
                    Exit Sub
                Else

                    sqlQuery = "UPDATE tbladmin SET username='" & Me.txtUserName.Text & _
                        "',password = '" & Me.txtUserPassword.Text & _
                        "',usertype = '" & Me.txtUserRole.Text & _
                        "',name = '" & Me.txtNameofUser.Text & _
                        "' WHERE userid = '" & Key(Me.DGVUsersList) & "'"
                    result = SQLExecute(sqlQuery)

                    If result > 0 Then
                        Me.btnCancel5_Click(sender, e)
                        Messages.PictureBox6.Image = My.Resources.Info_Squared_50px
                        Messages.lblTitle.Text = "Success"
                        Messages.btnOkay.Focus()
                        Messages.lblMessage.Text = "User successfully changed"
                        Messages.ShowDialog()
                    Else
                        Me.btnCancel5_Click(sender, e)
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub Button16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.TabControl2.SelectTab(TabPage1)
    End Sub

    Private Sub txtFirstName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFirstName.TextChanged, txtLastName.TextChanged _
        , txtMiddleName.TextChanged, txtGuardian.TextChanged, txtNameofUser.TextChanged
        toProperCase(sender)
    End Sub

    Private Sub DGVSubjectList_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DGVSubjectList.SelectionChanged
        On Error Resume Next
        If Me.DGVSubjectList.RowCount > 0 Then
            Me.txtShowPrereq.Clear()
            Dim row As Integer = Me.DGVSubjectList.CurrentRow.Index
            sqlQuery =
<sql>
        select subjectid,Subjcode,SubjDesc,b.subcode as `Prerequisite`,subdesc as `PrerequisiteDesc`
         from  tblprerequisite a
        left join (select csid,courseid,subcode,subdesc from tblcoursesub ) b
        on a.prerequisitesubjectid  = b.csid 
        left join (select csid,courseid,subcode as `SubjCode`,subdesc as `SubjDesc` from tblcoursesub) c
        on a.subjectid  = c.csid 
        where subjectid = '<%= Me.DGVSubjectList.Item(4, row).Value.ToString %>'
        </sql>

            dt = SQLRetrieve(sqlQuery)
            If dt.Rows.Count > 0 And Me.DGVSubjectList.RowCount <> 0 Then

                For x = 0 To dt.Rows.Count - 1
                    If Me.txtShowPrereq.Text = "" Then
                        Me.txtShowPrereq.Text = dt.Rows(x).Item(4).ToString
                    Else
                        Me.txtShowPrereq.Text = Me.txtShowPrereq.Text & "," & dt.Rows(x).Item(4).ToString
                    End If

                Next
            ElseIf dt.Rows.Count = 0 And Me.DGVSubjectList.RowCount = 0 Then
                Me.txtShowPrereq.Text = ""
            Else
                Me.txtShowPrereq.Text = "None"
            End If
        Else
            Me.txtShowPrereq.Text = ""
        End If
    End Sub

    Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button13.Click

        btnStudentDetails.PerformClick()
    End Sub

    Private Sub Button14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button14.Click
        btnSubject_Click(sender, e)

    End Sub

    Private Sub Button15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button15.Click
        btnCurriculum_Click(sender, e)

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        btnSMS.PerformClick()

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        btnUserAccount_Click(sender, e)
    End Sub



    Private Sub txtStudentID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStudentID.KeyPress
        Dim notallowedkey As String = "qwertyuiopasdfghjklzxcvbnm!@#$%^&*()_+~`=[]{}"":;|\<>?,./'"
        If InStr(notallowedkey, LCase(e.KeyChar)) Then
            e.Handled = True
        End If
    End Sub

    Private Sub Button3_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        btnUserAccount.PerformClick()
    End Sub

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        Me.TabControl1.SelectTab(AccountSettingTab)
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        Dim myStream As Stream
        Dim saveFileDialog1 As New SaveFileDialog()
        saveFileDialog1.FileName = Me.txtDBName.Text & ".sql"
        ' saveFileDialog1.Filter = "Sql Files (*.sql).|*.sql"

        saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*|Sql Files (*.sql).|*.sql"
        saveFileDialog1.FilterIndex = 3
        saveFileDialog1.RestoreDirectory = True

        If saveFileDialog1.ShowDialog() = DialogResult.OK Then
            myStream = saveFileDialog1.OpenFile()
            If (myStream IsNot Nothing) Then
                Me.txtPath.Text = saveFileDialog1.FileName
                ' Code to write the stream goes here.
                myStream.Close()
            End If
        End If
    End Sub

    Private Sub Button21_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button21.Click
        Try
            Dim mysqlconn As New MySqlConnection("server='" & Me.txtImportHost.Text & _
                                                 "';userid='" & Me.txtImportUser.Text & _
                                                 "';pwd='" & Me.txtImportPass.Text & _
                                                 "';port=3306;")



            Dim mysqlcmd As New MySqlCommand
            mysqlcmd.Connection = mysqlconn
            mysqlconn.Open()
            Dim mb As MySqlBackup = New MySqlBackup(mysqlcmd)
            mb.ImportFromFile(txtImportPath.Text)
            mysqlconn.Close()

            Messages.PictureBox6.Image = My.Resources.Info_Squared_50px
            Messages.lblTitle.Text = "Restore"
            Messages.btnOkay.Focus()
            Messages.lblMessage.Text = "Database Successfully Restored"
            Messages.ShowDialog()

        Catch ex As Exception
            MsgBox(ex.Message)

        End Try
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        Try
            Dim mysqlconn As New MySqlConnection("server='" & Me.txtHost.Text & "';userid='" & _
                                                 Me.txtUid.Text & "';pwd='" & Me.txtPassword.Text & _
                                                 "';database='" & Me.txtDBName.Text & "';port = 3306;")
            Dim mysqlcmd As MySqlCommand = New MySqlCommand
            mysqlcmd.Connection = mysqlconn
            mysqlconn.Open()
            Dim mb As MySqlBackup = New MySqlBackup(mysqlcmd)
            mb.ExportToFile(Me.txtPath.Text)
            mysqlconn.Close()


            Messages.PictureBox6.Image = My.Resources.Info_Squared_50px
            Messages.lblTitle.Text = "Backup"
            Messages.btnOkay.Focus()
            Messages.lblMessage.Text = "Database Successfully Backup"
            Messages.ShowDialog()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub ComboBox1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDBName.Click
        On Error Resume Next
        Dim mysqlconn = New MySqlConnection("server='" & Me.txtHost.Text & "';userid='" & Me.txtUid.Text & _
                                            "';pwd='" & Me.txtPassword.Text & "';")
        sqlQuery = "show databases;"
        dt = New DataTable
        adapter = New MySqlDataAdapter(sqlQuery, mysqlconn)
        adapter.Fill(dt)
        Me.txtDBName.DataSource = dt
        Me.txtDBName.DisplayMember = "database"

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDBName.SelectedIndexChanged

    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Dim OpenFile As New OpenFileDialog()

        'OpenFile.FileName = Me.txtDBName.Text & ".sql"
        'OpenFile.Filter = "Sql Files (*.sql).|*.sql"
        OpenFile.ShowDialog()
        OpenFile.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*|Sql Files (*.sql).|*.sql"
        OpenFile.FilterIndex = 3

        Me.txtImportDBName.Text = OpenFile.SafeFileNames.ToString()
        OpenFile.RestoreDirectory = True

        If OpenFile.FileName <> "" Then
            txtImportPath.Text = OpenFile.FileName

        End If
    End Sub

    Private Sub chkShowPass_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowPass.CheckedChanged
        If Me.chkShowPass.Checked Then
            Me.txtSetNewPass.PasswordChar = ""
            Me.txtSetConfirmPass.PasswordChar = ""
            Me.txtSetOldPass.PasswordChar = ""
        Else
            Me.txtSetNewPass.PasswordChar = "●"
            Me.txtSetConfirmPass.PasswordChar = "●"
            Me.txtSetOldPass.PasswordChar = "●"
        End If
    End Sub

    Private Sub PictureBox12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox12.Click
        strSearch = "Enroll"
        SearchStudent.ShowDialog()
    End Sub

    Private Sub PictureBox14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub txtGradeStudentID_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        If Me.txtGradeStudentID.Text <> "" Then
            Call SubjectGrade()
            Call GradeSchoolYear()
            sqlQuery = "select fname,mname,lname from tblstudent where studid = '" & Me.txtGradeStudentID.Text & "'"

            dt = SQLRetrieve(sqlQuery)
            Me.txtGradeFname.Text = dt.Rows(0).Item(0).ToString
            Me.txtGradeMname.Text = dt.Rows(0).Item(1).ToString
            Me.txtGradeLName.Text = dt.Rows(0).Item(2).ToString
        Else
            Me.txtGradeLName.Clear()
            Me.txtGradeFname.Clear()
            Me.txtGradeMname.Clear()
        End If

    End Sub


    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        LoadStudent(Me.TextBox2.Text)
        If Me.TextBox2.TextLength > 0 Then
            Me.lblSearch.Visible = False
        Else
            Me.lblSearch.Visible = True

        End If
    End Sub



    Private Sub lblSearch_Click(sender As Object, e As EventArgs) Handles lblSearch.Click
        Me.TextBox2.Focus()
    End Sub

    Private Sub Label32_Click(sender As Object, e As EventArgs) Handles Label32.Click
        Me.txtSearchPrereq.Focus()
    End Sub

    Private Sub lblSearchSubject_Click(sender As Object, e As EventArgs) Handles lblSearchSubject.Click
        Me.txtSearchSubject.Focus()
    End Sub

    Private Sub lblSearchSubjectList_Click(sender As Object, e As EventArgs) Handles lblSearchSubjectList.Click
        Me.txtSearchSubjectList.Focus()
    End Sub

    Private Sub Label55_Click(sender As Object, e As EventArgs) Handles Label55.Click
        Me.txtSearchGrade.Focus()
    End Sub



    Private Sub DGVSubjectList_RowStateChanged(sender As Object, e As DataGridViewRowStateChangedEventArgs) Handles DGVSubjectList.RowStateChanged
        If Me.DGVSubjectList.RowCount > 0 Then
            Me.DGVSubjectList_SelectionChanged(sender, e)
        End If
    End Sub

    Private Sub Button20_Click(sender As Object, e As EventArgs) Handles Button20.Click
        If isEmpty(Panel33) Then
            MsgBox("Fillup all textbox", vbCritical, "Required")
            Exit Sub
        ElseIf Me.txtSetConfirmPass.Text <> Me.txtSetNewPass.Text Then
            MsgBox("Password do not match", vbExclamation, "Incorrect")
        Else
            sqlQuery =
<sql>
    select password from tbladmin where username = '<%= strUsername %>'
and password = '<%= Me.txtSetOldPass.Text %>'
</sql>

            dt = SQLRetrieve(sqlQuery)

            If dt.Rows.Count = 0 Then
                MsgBox("Incorrect Old password", vbExclamation, "Incorrect")
            Else
                sqlQuery =
<sql>
update tbladmin set username = '<%= Me.txtUsernameSetting.Text %>',
password = '<%= Me.txtSetNewPass.Text %>'
where username = '<%= strUsername.ToString %>'
</sql>

                SQLExecute(sqlQuery)


                MsgBox("Account Info successfully changed", vbInformation, "Success")
            End If
        End If

    End Sub

    Private Sub Button19_Click(sender As Object, e As EventArgs) Handles Button19.Click
        ClearText(Panel33)
    End Sub

    Private Sub DGVSubjectList_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVSubjectList.CellContentClick

    End Sub

    Private Sub cmbSubjectYearLevel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSubjectYearLevel.SelectedIndexChanged
        Subject()
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs)
        PrerequisiteInfo()
    End Sub

    Private Sub cmbPrereqCurriculum_SelectedIndexChanged_1(sender As Object, e As EventArgs) Handles cmbPrereqCurriculum.SelectedIndexChanged
        Subject()
        PrerequisiteInfo()
        If Me.cmbPrereqCurriculum.SelectedIndex >= 0 Then
            sqlQuery = <sql>
                          select courseid from tblcourse where
 currseries = '<%= Me.cmbPrereqCurriculum.Text %>'
                      </sql>

            dt = SQLRetrieve(sqlQuery)

            courseid = dt.Rows(0).Item(0)
        End If
    End Sub

    Private Sub cmbSubjSemester_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSubjSemester.SelectedIndexChanged
        Subject()
    End Sub

    Private Sub cmbPrereqYearLevel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPrereqYearLevel.SelectedIndexChanged
        PrerequisiteInfo()
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click


        For x = 0 To Me.CheckedListBox1.Items.Count - 1

            If Me.CheckedListBox1.GetItemChecked(x) = True Then
                sqlQuery =
<sql>
INSERT INTO tblprerequisite (subjectid,prerequisitesubjectid,courseid)
 VALUES 
((select csid from tblcoursesub where subdesc = '<%= Me.ListBox1.SelectedItem.ToString %>'
and courseid = '<%= courseid %>'),
(select csid from tblcoursesub where subdesc = '<%= Me.CheckedListBox1.Items(x).ToString %>'
and courseid = '<%= courseid %>'),'<%= courseid %>')
</sql>
                SQLExecute(sqlQuery)

            End If
        Next
        Call PrerequisiteInfo()
        ClearText(Me.DGVPrerequiste)
        Messages.PictureBox6.Image = My.Resources.Info_Squared_50px
        Messages.lblTitle.Text = "Add"
        Messages.lblMessage.Text = "Successfully Saved."
        Messages.ShowDialog()

    End Sub

    Private Sub ListBox1_SelectedIndexChanged_1(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        PrerequisiteInfo()
    End Sub

    Private Sub cmbPrereqSem_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPrereqSem.SelectedIndexChanged
        PrerequisiteInfo()
    End Sub

    Private Sub PictureBox14_Click_1(sender As Object, e As EventArgs) Handles PictureBox14.Click
        strSearch = "Grade"
        SearchStudent.ShowDialog()
    End Sub

    Private Sub txtGradeStudentID_TextChanged_1(sender As Object, e As EventArgs) Handles txtGradeStudentID.TextChanged
        SubjectGrade()
        GradeSchoolYear()
        Me.cmbGradeSY.SelectedIndex = -1
        If Me.txtGradeStudentID.Text.Length > 0 Then
            sqlQuery = "SELECT  `lname`, `fname`, `mname`,currseries FROM `tblstudent` a " & _
                "left join (select courseid,currseries from tblcourse) b on a.courseid = b.courseid  WHERE studid = '" & _
                Me.txtGradeStudentID.Text & "'"

            dt = SQLRetrieve(sqlQuery)
            If dt.Rows.Count > 0 Then
                Me.txtGradeLName.Text = dt.Rows(0).Item(0).ToString
                Me.txtGradeMname.Text = dt.Rows(0).Item(2).ToString
                Me.txtGradeFname.Text = dt.Rows(0).Item(1).ToString
                Me.txtGradeCurriculum.Text = dt.Rows(0).Item(3).ToString
            Else
                ' ClearText(GradeEntryTab)
            End If
        Else
            ClearText(GradeEntryTab)

        End If
    End Sub


    Private Sub txtCourseCode_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCourseCode.KeyPress
        Dim notallowedkey As String = "!@#$%^&*()_+~`=[]{}"":;|\<>?,./"
        If InStr(notallowedkey, LCase(e.KeyChar)) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtCourseDesc_TextChanged(sender As Object, e As EventArgs) Handles txtCourseDesc.TextChanged

    End Sub

    Private Sub PictureBox13_Click(sender As Object, e As EventArgs) Handles PictureBox13.Click
        SubjectList()
    End Sub

    Private Sub PictureBox13_MouseHover(sender As Object, e As EventArgs) Handles PictureBox13.MouseHover
        '  Me.PictureBox13.BackColor = System.Drawing.Color.FromArgb(41, 119, 204)
        Me.PictureBox13.BackgroundImage = My.Resources.SearchColorBlue
    End Sub

    Private Sub PictureBox13_MouseLeave(sender As Object, e As EventArgs) Handles PictureBox13.MouseLeave
        Me.PictureBox13.BackColor = Color.White
        Me.PictureBox13.BackgroundImage = My.Resources.Search_24px
    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click
        If txtWhat.Text.Trim.Length = 0 Then
        Else
            sqlQuery =
<sql>
INSERT INTO tblAnnouncement (what,
who,
`where`,
`when`)
 VALUES 
('<%= Me.txtWhat.Text.Replace("'", "\'") %>',
'<%= Me.txtWho.Text.Replace("'", "\'") %>',
'<%= Me.txtWhere.Text.Replace("'", "\'") %>',
'<%= Me.txtWhen.Text.Replace("'", "\'") %>')
</sql>

            SQLExecute(sqlQuery)
            Call Announcement()
            ClearText(Me.DashboardTab)
        End If
    End Sub

    Private Sub Button22_Click(sender As Object, e As EventArgs) Handles Button22.Click
        ClearText(Me.DashboardTab)
    End Sub

    Private Sub lbAnnouncement_DoubleClick(sender As Object, e As EventArgs) Handles lbAnnouncement.DoubleClick
        If Me.lbAnnouncement.Items.Count > 0 Then
            If MsgBox("Delete this Announcement", vbQuestion + vbYesNo) = vbYes Then
                sqlQuery = "Delete from tblannouncement where what = '" & Me.lbAnnouncement.SelectedItem.ToString & "'"

                SQLExecute(sqlQuery)
                Announcement()
            End If
        End If
    End Sub

    Private Sub DGVGrades_CellBeginEdit1(sender As Object, e As DataGridViewCellCancelEventArgs) Handles DGVGrades.CellBeginEdit
        tempgrade = Me.DGVGrades(e.ColumnIndex, e.RowIndex).Value.ToString
        row = e.RowIndex
    End Sub

    Private Sub DGVGrades_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DGVGrades.CellEndEdit
        If Me.DGVGrades(e.ColumnIndex, e.RowIndex).Value.ToString.ToUpper <> "INC" And _
       Me.DGVGrades(e.ColumnIndex, e.RowIndex).Value.ToString <> "1.0" _
     And Me.DGVGrades(e.ColumnIndex, e.RowIndex).Value.ToString <> "1.25" And _
     Me.DGVGrades(e.ColumnIndex, e.RowIndex).Value.ToString <> "1.5" And _
     Me.DGVGrades(e.ColumnIndex, e.RowIndex).Value.ToString <> "1.75" And _
     Me.DGVGrades(e.ColumnIndex, e.RowIndex).Value.ToString <> "2.0" And _
     Me.DGVGrades(e.ColumnIndex, e.RowIndex).Value.ToString <> "2.25" And _
     Me.DGVGrades(e.ColumnIndex, e.RowIndex).Value.ToString <> "2.5" And _
     Me.DGVGrades(e.ColumnIndex, e.RowIndex).Value.ToString <> "2.75" And _
     Me.DGVGrades(e.ColumnIndex, e.RowIndex).Value.ToString <> "3.0" And _
     Me.DGVGrades(e.ColumnIndex, e.RowIndex).Value.ToString <> "4.0" And _
      Me.DGVGrades(e.ColumnIndex, e.RowIndex).Value.ToString <> "5.0" And _
        Me.DGVGrades(e.ColumnIndex, e.RowIndex).Value.ToString <> "" And _
        Me.DGVGrades(e.ColumnIndex, e.RowIndex).Value.ToString <> "DRP" Then

            Messages.PictureBox6.Image = My.Resources.Close_Window_50px
            Messages.lblTitle.Text = "Invalid"
            Messages.btnOkay.Focus()
            Messages.lblMessage.Text = "Invalid inputted grade."
            Messages.ShowDialog()
            Me.DGVGrades("Grades", Me.DGVGrades.CurrentRow.Index).Value = tempgrade
            isValid = False
            Exit Sub
        Else
            isValid = True

        End If


        If isValid = True Then
            If Me.DGVGrades(e.ColumnIndex, e.RowIndex).Value.ToString = "DRP" Then
                Me.DGVGrades("Remarks", e.RowIndex).Value = "Dropped"
            ElseIf Me.DGVGrades(e.ColumnIndex, e.RowIndex).Value.ToString = "5.0" Then
                Me.DGVGrades("Remarks", e.RowIndex).Value = "Failed"
            ElseIf Me.DGVGrades(e.ColumnIndex, e.RowIndex).Value.ToString = "INC" Then
                Me.DGVGrades("Remarks", e.RowIndex).Value = "Incomplete"
            ElseIf Me.DGVGrades(e.ColumnIndex, e.RowIndex).Value.ToString = "4.0" Then
                Me.DGVGrades("Remarks", e.RowIndex).Value = "Conditional"
            ElseIf Me.DGVGrades(e.ColumnIndex, e.RowIndex).Value.ToString = "" Then
                Me.DGVGrades("Remarks", e.RowIndex).Value = "No Grade"
            Else

                Me.DGVGrades("Remarks", e.RowIndex).Value = "Passed"

            End If
        End If

    End Sub

    Private Sub DGVGrades_CellContentClick_1(sender As Object, e As DataGridViewCellEventArgs) Handles DGVGrades.CellContentClick

    End Sub

    Private Sub DGVGrades_SelectionChanged1(sender As Object, e As EventArgs) Handles DGVGrades.SelectionChanged
        If Me.txtGradeFname.Text.Length > 0 Then
            If Me.DGVGrades.RowCount > 0 Then
                Dim row As Integer = Me.DGVGrades.CurrentRow.Index
                With Me.DGVGrades
                    Me.txtGradeSubCode.Text = Me.DGVGrades("Subject Code", row).Value.ToString
                    Me.txtGradeSubDesc.Text = Me.DGVGrades("Subject Description", row).Value.ToString

                End With
            Else
                Me.txtGradeSubCode.Clear()
                Me.txtGradeSubDesc.Clear()

            End If
        Else
            Me.txtGradeSubCode.Clear()
            Me.txtGradeSubDesc.Clear()

        End If

    End Sub

    Private Sub txtGradeSubDesc_TextChanged(sender As Object, e As EventArgs) Handles txtGradeSubDesc.TextChanged

    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim rpt As New sgrpt
        rpt.SetParameterValue("studid", Me.txtGradeStudentID.Text)
        GradeReport.CrystalReportViewer1.ReportSource = rpt
        GradeReport.CrystalReportViewer1.Refresh()
        GradeReport.ShowDialog()
    End Sub

    Private Sub Button16_Click_1(sender As Object, e As EventArgs) Handles Button16.Click
        EventEntry.ShowDialog()
    End Sub
End Class