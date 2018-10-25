Imports System.IO
Public Class EventPanel

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        sqlQuery = "delete from tblevent where eventid = '" & Me.lblEventID.Text & "'"
        SQLExecute(sqlQuery)
        EventEntry.pnlList.Controls.Remove(Me)


    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        MsgBox(Me.Name)
    End Sub
End Class
