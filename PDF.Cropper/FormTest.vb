Public Class FormTest
    Shadows menu As MenuStrip

    Public Sub New()

        InitializeComponent()

        Dim textbox As New PDFCropper.WaterMarkTextBox
        textbox.WaterMarkText = "23333"

        Me.Controls.Add(textbox)
        textbox.SelectAll()
    End Sub
End Class
