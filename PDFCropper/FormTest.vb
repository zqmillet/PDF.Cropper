Public Class FormTest
    Private Sub FormTest_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        With mText
            .Padding = New Padding(0, 3, 0, 0)
            .Margin = New Padding(0)
            .Text = "2333333"

        End With
    End Sub





    Private Sub mText_TextChanged(sender As Object, e As EventArgs) Handles mText.TextChanged

    End Sub


End Class
