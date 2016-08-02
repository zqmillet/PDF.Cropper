Namespace PDFCropper
    Public Class RichTextBox
        Inherits System.Windows.Forms.RichTextBox

        Public Sub New()
            With Me
                .Multiline = True
                .Dock = DockStyle.Fill
                .BorderStyle = BorderStyle.None
                .Font = New Font("Consolas", 10, FontStyle.Bold)
            End With
        End Sub
    End Class
End Namespace

