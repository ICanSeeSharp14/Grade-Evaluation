Public Class StudentGradeReport

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim rpt As New StudentGrade
        rpt.SetParameterValue("studid", Me.TextBox1.Text)
        Me.CrystalReportViewer1.ReportSource = rpt
        Me.CrystalReportViewer1.Refresh()

    End Sub
End Class