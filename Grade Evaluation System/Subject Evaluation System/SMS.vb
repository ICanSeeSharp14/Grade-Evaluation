Imports System.Management
Imports System.Threading
Imports System.Text.RegularExpressions

Public Class frmsms
    Dim rcvdata As String = ""
    Dim index As String = ""

    Private Sub serialport1_datareceived(ByVal sender As Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived
        Dim datain As String = ""
        Dim numbytes As Integer = SerialPort1.BytesToRead
        For i As Integer = 1 To numbytes
            datain &= Chr(SerialPort1.ReadChar)
        Next
        test(datain)
    End Sub
    Private Sub test(ByVal indata As String)
        rcvdata &= indata
    End Sub

    Public Function ModemsConnected() As String
        Dim modems As String = ""
        Try
            Dim searcher As New ManagementObjectSearcher( _
                "root\CIMV2", _
                "SELECT * FROM Win32_POTSModem")

            For Each queryObj As ManagementObject In searcher.Get()
                If queryObj("Status") = "OK" Then
                    modems = modems & (queryObj("AttachedTo") & " - " & queryObj("Description") & "***")
                End If
            Next
        Catch err As ManagementException
            MessageBox.Show("An error occurred while querying for WMI data: " & err.Message)
            Return ""
        End Try
        Return modems
    End Function




    Private Sub cmdconnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdconnect.Click
        cmdconnect.Cursor = Cursors.WaitCursor

        Try
            With SerialPort1
                .PortName = Label1.Text
                .BaudRate = 9600
                .Parity = IO.Ports.Parity.None
                .DataBits = 8
                .StopBits = IO.Ports.StopBits.One
                .Handshake = IO.Ports.Handshake.None
                .RtsEnable = True
                .ReceivedBytesThreshold = 1
                .NewLine = vbCr
                .ReadTimeout = 1000
                .Open()
            End With
            If SerialPort1.IsOpen Then
                Label3.Text = "Connected"
            Else
                Label3.Text = "Got some error"
            End If
            cmdconnect.Cursor = Cursors.Default

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Timer1.Start()
        CircularProgress2.Visible = False
        Dim ports() As String
        ports = Split(ModemsConnected(), "***")
        For i As Integer = 0 To ports.Length - 2
            ComboBox1.Items.Add(ports(i))
        Next

        ComboBox2.Enabled = False
        ComboBox3.Enabled = False
        ComboBox4.Enabled = False
        ComboBox5.Enabled = False
        TextBox1.Enabled = False
        Button1.Enabled = False
        schoolyear()
        DataGridView1.Hide()



    End Sub

    Private Sub ComboBox1_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedValueChanged

        Label1.Text = Trim(Mid(ComboBox1.Text, 1, 5))

    End Sub

    Private Sub Cmdsend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cmdsend.Click

        Dim i As Integer = 0
        Dim b As Integer = 0
        Dim c As Integer = 0
        Dim d As Integer = 0

        If RadioButton1.Checked = True Then

            Do While (i < DataGridView2.RowCount)
                DataGridView2.CurrentCell = DataGridView2.Rows(b).Cells(0)
        


                With SerialPort1
                    .Write("at" & vbCrLf)
                    Threading.Thread.Sleep(1000)
                    .Write("at+cmgf=1" & vbCrLf)
                    Threading.Thread.Sleep(1000)
                    .Write("at+cmgs=" & Chr(34) & Me.DataGridView2.Item("number", Me.DataGridView2.CurrentCellAddress.Y).Value.ToString & Chr(34) & vbCrLf)
                    .Write(txtmessage.Text & Chr(26))
                    Threading.Thread.Sleep(1000)
                    ' MsgBox(rcvdata.ToString)


                    '           Cmdsend.Cursor = Cursors.Default
                End With
                i = i + 1
                b = b + 1

            Loop
                    If i = DataGridView2.RowCount Then
                'MsgBox("Message sent: " & DataGridView2.RowCount & " Message has been succesfully sent", vbInformation)
                CircularProgress2.ProgressText = "Sent: " & DataGridView2.RowCount & " Message has been successfully sent "

                CircularProgress2.IsRunning = False
            End If

        ElseIf RadioButton2.Checked = True Then
            'Cmdsend.Cursor = Cursors.WaitCursor
            Do While (c < DataGridView1.RowCount)
                DataGridView1.CurrentCell = DataGridView1.Rows(d).Cells(0)

                With SerialPort1
                    .Write("at" & vbCrLf)
                    Threading.Thread.Sleep(1000)
                    .Write("at+cmgf=1" & vbCrLf)
                    Threading.Thread.Sleep(1000)
                    .Write("at+cmgs=" & Chr(34) & Me.DataGridView1.Item("cellphone_no", Me.DataGridView1.CurrentCellAddress.Y).Value.ToString & Chr(34) & vbCrLf)
                    .Write(txtmessage.Text & Chr(26))
                    Threading.Thread.Sleep(1000)
                    ' MsgBox(rcvdata.ToString)


                    Cmdsend.Cursor = Cursors.Default
                End With

                
                c = c + 1
                d = d + 1

            Loop
                        If c = DataGridView1.RowCount Then
                MsgBox("message sent: " & DataGridView1.RowCount & " message has been succesfully sent", vbInformation)
            End If
        End If
    End Sub
    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click


        Button1.Cursor = Cursors.WaitCursor
        DataGridView2.Rows.Add(TextBox1.Text)
        Button1.Cursor = Cursors.Default






    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Button2.Cursor = Cursors.WaitCursor
        Retrievemsg()
        Button2.Cursor = Cursors.Default
    End Sub
    Private Sub Retrievemsg()

        Try
            With SerialPort1
                rcvdata = ""
                .Write("AT" & vbCrLf)
                Threading.Thread.Sleep(1000)
                .Write("AT+CMGF=1" & vbCrLf)
                Threading.Thread.Sleep(1000)
                .Write("AT+CPMS=""SM""" & vbCrLf)
                Threading.Thread.Sleep(1000)
                .Write("AT+CMGL=""ALL""" & vbCrLf)
                Threading.Thread.Sleep(1000)
                'MsgBox(rcvdata.ToString)
                ReadMsg()

            End With
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ReadMsg()
        ListView1.Items.Clear()
        Try
            Dim lineoftext As String
            Dim i As Integer
            Dim arytextfile() As String
            lineoftext = rcvdata.ToString
            arytextfile = Split(lineoftext, "+CMGL", , CompareMethod.Text)
            For i = 1 To UBound(arytextfile)
                Dim input As String = arytextfile(i)
                Dim result() As String
                Dim pattern As String = "(:)|(,"")|("","")"
                result = Regex.Split(input, pattern)
                Dim concat() As String
                With ListView1.Items.Add("null")
                    'Index
                    .SubItems.AddRange(New String() {result(2)})
                    'status()
                    .SubItems.AddRange(New String() {result(4)})
                    'number()


                    Dim my_string, position As String
                    my_string = result(6)
                    position = my_string.Length - 2
                    my_string = my_string.Remove(position, 2)
                    .SubItems.Add(my_string)
                    '  for date and time
                    concat = New String() {result(8) & result(9) & result(10) & result(11) & result(12).Substring(0, 2)}
                    .SubItems.AddRange(concat)
                    ' Message(content)
                    Dim lineoftexts As String
                    Dim arytextfiles() As String
                    lineoftexts = arytextfile(i)
                    arytextfiles = Split(lineoftexts, "+32", , CompareMethod.Text)
                    .SubItems.Add(arytextfiles(1))
                End With
            Next
        Catch ex As Exception

        End Try

    End Sub

  
 

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Button4.Cursor = Cursors.WaitCursor

        Try
            With SerialPort1
                'delete SMS
                .Write("AT")
                Threading.Thread.Sleep(1000)
                .Write("AT+CMGF=1")
                Threading.Thread.Sleep(1000)
                .Write("AT+CPMS=""SM""" & vbCrLf)
                Threading.Thread.Sleep(1000)
                'delete
                .Write("AT+CMGD=" & (ListView1.SelectedItems(0).SubItems(1).Text) & ",0" & vbCrLf)
                Threading.Thread.Sleep(1000)


                Retrievemsg()

                MsgBox("Successfully Deleted", vbInformation, "")
                ' Button4.Cursor = Cursors.Default

            End With

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try


    End Sub


    Private Sub ListView1_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListView1.MouseDoubleClick


        TextBox2.Text = ListView1.SelectedItems(0).SubItems(5).Text & vbNewLine & vbNewLine &
            "Number: " & ListView1.SelectedItems(0).SubItems(3).Text & vbNewLine &
         "Date and Time: " & ListView1.SelectedItems(0).SubItems(4).Text


 
    End Sub

    
    

    Private Sub DeleteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteToolStripMenuItem.Click
        
        If ListView1.Items.Count = 0 Then

            MsgBox("Please click to the data or theres no message that can be deleted", vbInformation, "")
        Else

            

            Try
                With SerialPort1
                    'delete SMS
                    .Write("AT")
                    Threading.Thread.Sleep(1000)
                    .Write("AT+CMGF=1")
                    Threading.Thread.Sleep(1000)
                    .Write("AT+CPMS=""SM""" & vbCrLf)
                    Threading.Thread.Sleep(1000)
                    'delete
                    .Write("AT+CMGD=" & (ListView1.SelectedItems(0).SubItems(1).Text) & ",0" & vbCrLf)
                    Threading.Thread.Sleep(1000)


                    Retrievemsg()

                    MsgBox("Successfully Deleted", vbInformation, "")

                End With

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try


        End If

    End Sub

    Private Sub DeleteAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteAllToolStripMenuItem.Click
        If ListView1.Items.Count = 0 Then

            MsgBox("Please click to the data or theres no message that can be deleted", vbInformation, "")
        Else

            Try
                With SerialPort1
                    'delete all
                    .Write("AT")
                    Threading.Thread.Sleep(1000)
                    .Write("AT+CMGF=1")
                    Threading.Thread.Sleep(1000)
                    .Write("AT+CPMS=""SM""" & vbCrLf)
                    Threading.Thread.Sleep(1000)
                    'delete
                    .Write("AT+CMGD=1,4" & vbCrLf)
                    Threading.Thread.Sleep(1000)


                    Retrievemsg()

                    MsgBox("Successfully Deleted", vbInformation, "")

                End With

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    Private Sub ReplyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReplyToolStripMenuItem.Click
        If RadioButton1.Checked = True Then
            DataGridView2.Rows.Add(ListView1.SelectedItems(0).SubItems(3).Text)
            TabControl1.SelectedTab = TabPage1
        ElseIf RadioButton2.Checked = True Then
            MsgBox("please select manual on the send tab first before reply ", vbInformation, "")





             
        End If


    End Sub

     

    Private Sub ButtonX1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonX1.Click
        Me.Close()

    End Sub
 


    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged
        ComboBox2.Enabled = True
        ComboBox3.Enabled = True
        ComboBox4.Enabled = True
        ComboBox5.Enabled = True
        TextBox1.Enabled = False
        Button1.Enabled = False
        DataGridView2.Hide()
        DataGridView1.Show()

    End Sub

    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged
        ComboBox2.Enabled = False
        ComboBox3.Enabled = False
        ComboBox4.Enabled = False
        ComboBox5.Enabled = False
        TextBox1.Enabled = True
        Button1.Enabled = True
        DataGridView2.Show()
        DataGridView1.Hide()










    End Sub
    Private Sub schoolyear()
        Dim dt As DataTable
        Dim sql As String
        sql = "select distinct school_year from scholarship_grant"
        dt = SqlFetch(sql)
        ComboBox4.DataSource = dt
        ComboBox4.DisplayMember = "school_year"

    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox3.SelectedIndexChanged
        Dim dt As DataTable
        Dim sql As String

        If ComboBox2.Text = "ISIP" And ComboBox3.Text = "IN" Then
            sql = "select cellphone_no,concat(scholar.lastname,', ',scholar.firstname) as fullname,scholarship_name,college_school,school_year,semester,type from scholar" _
            & " inner join scholarship on scholar.scholarid=scholarship.scholarid" _
            & " inner join scholarship_grant on scholarship.scholarship_id=scholarship_grant.scholarship_id where scholarship_name ='" & ComboBox2.Text & "' and school_year='" & ComboBox4.Text & "' and semester='" & ComboBox5.Text & "' and type='" & ComboBox3.Text & "' order by fullname"
            dt = SqlFetch(sql)
            DataGridView1.DataSource = dt
        ElseIf ComboBox2.Text = "ISIP" And ComboBox3.Text = "OUT" Then
            sql = "select cellphone_no,concat(scholar.lastname,', ',scholar.firstname) as fullname, scholarship_name,college_school,school_year,semester,type from scholar" _
            & " inner join scholarship on scholar.scholarid=scholarship.scholarid" _
            & " inner join scholarship_grant on scholarship.scholarship_id=scholarship_grant.scholarship_id where scholarship_name ='" & ComboBox2.Text & "' and school_year='" & ComboBox4.Text & "' and semester='" & ComboBox5.Text & "' and type='" & ComboBox3.Text & "' order by fullname"
            dt = SqlFetch(sql)
            DataGridView1.DataSource = dt
        ElseIf ComboBox2.Text = "SAINTS" And ComboBox3.Text = "IN" Then
            sql = "select cellphone_no,concat(scholar.lastname,', ',scholar.firstname) as fullname, scholarship_name,college_school,school_year,semester,type from scholar" _
            & " inner join scholarship on scholar.scholarid=scholarship.scholarid" _
            & " inner join scholarship_grant on scholarship.scholarship_id=scholarship_grant.scholarship_id where scholarship_name ='" & ComboBox2.Text & "' and school_year='" & ComboBox4.Text & "' and semester='" & ComboBox5.Text & "' and type='" & ComboBox3.Text & "' order by fullname"
            dt = SqlFetch(sql)
            DataGridView1.DataSource = dt
        ElseIf ComboBox2.Text = "SAINTS" And ComboBox3.Text = "OUT" Then
            sql = "select cellphone_no,concat(scholar.lastname,', ',scholar.firstname) as fullname,scholarship_name,college_school,school_year,semester,type from scholar" _
            & " inner join scholarship on scholar.scholarid=scholarship.scholarid" _
             & " inner join scholarship_grant on scholarship.scholarship_id=scholarship_grant.scholarship_id where scholarship_name ='" & ComboBox2.Text & "' and school_year='" & ComboBox4.Text & "' and semester='" & ComboBox5.Text & "' and type='" & ComboBox3.Text & "' order by fullname"
            dt = SqlFetch(sql)
            DataGridView1.DataSource = dt
        ElseIf ComboBox2.Text = "ACES" And ComboBox3.Text = "IN" Then
            sql = "select cellphone_no,concat(scholar.lastname,', ',scholar.firstname) as fullname,scholarship_name,college_school,school_year,semester,type from scholar" _
            & " inner join scholarship on scholar.scholarid=scholarship.scholarid" _
            & " inner join scholarship_grant on scholarship.scholarship_id=scholarship_grant.scholarship_id where scholarship_name ='" & ComboBox2.Text & "' and school_year='" & ComboBox4.Text & "' and semester='" & ComboBox5.Text & "' and type='" & ComboBox3.Text & "' order by fullname"
            dt = SqlFetch(sql)
            DataGridView1.DataSource = dt
        ElseIf ComboBox2.Text = "ACES" And ComboBox3.Text = "OUT" Then
            sql = "select cellphone_no,concat(scholar.lastname,', ',scholar.firstname) as fullname, scholarship_name,college_school,school_year,semester,type from scholar" _
           & " inner join scholarship on scholar.scholarid=scholarship.scholarid" _
            & " inner join scholarship_grant on scholarship.scholarship_id=scholarship_grant.scholarship_id where scholarship_name ='" & ComboBox2.Text & "' and school_year='" & ComboBox4.Text & "' and semester='" & ComboBox5.Text & "' and type='" & ComboBox3.Text & "' order by fullname"
            dt = SqlFetch(sql)
            DataGridView1.DataSource = dt
        ElseIf ComboBox2.Text = "SUCCESS" And ComboBox3.Text = "IN" Then
            sql = "select cellphone_no,concat(scholar.lastname,', ',scholar.firstname) as fullname, scholarship_name,college_school,school_year,semester,type from scholar" _
            & " inner join scholarship on scholar.scholarid=scholarship.scholarid" _
             & " inner join scholarship_grant on scholarship.scholarship_id=scholarship_grant.scholarship_id where scholarship_name ='" & ComboBox2.Text & "' and school_year='" & ComboBox4.Text & "' and semester='" & ComboBox5.Text & "' and type='" & ComboBox3.Text & "' order by fullname"
            dt = SqlFetch(sql)
            DataGridView1.DataSource = dt
        ElseIf ComboBox2.Text = "SUCCESS" And ComboBox3.Text = "OUT" Then
            sql = "select cellphone_no,concat(scholar.lastname,', ',scholar.firstname) as fullname, scholarship_name,college_school,school_year,semester,type from scholar" _
            & " inner join scholarship on scholar.scholarid=scholarship.scholarid" _
             & " inner join scholarship_grant on scholarship.scholarship_id=scholarship_grant.scholarship_id where scholarship_name ='" & ComboBox2.Text & "' and school_year='" & ComboBox4.Text & "' and semester='" & ComboBox5.Text & "' and type='" & ComboBox3.Text & "' order by fullname"
            dt = SqlFetch(sql)
            DataGridView1.DataSource = dt
        Else

            MsgBox("Please, Select Properly", vbInformation, "")

        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        CircularProgress2.IsRunning = True
    End Sub

   

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        CircularProgress2.Visible =true
    End Sub
End Class
