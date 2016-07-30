Public Class Console
    Inherits RichTextBox

    Public Sub New()
        With Me
            .Multiline = True
            .Dock = DockStyle.Fill
            .BorderStyle = BorderStyle.None
            .Font = New Font("Consolas", 10, FontStyle.Bold)
            ' AddHandler .TextChanged, AddressOf Me_TextChanged
        End With
    End Sub

    'Private Sub Me_TextChanged()
    '    With Me
    '        .SelectionStart = .TextLength
    '        .SelectionLength = 0
    '    End With
    'End Sub
End Class
