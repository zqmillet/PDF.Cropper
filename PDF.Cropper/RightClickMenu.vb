Public Class RightClickMenu
    Inherits ContextMenuStrip

    Dim TopMostMenuItem As ToolStripMenuItem
    Dim ExitMenuItem As ToolStripMenuItem

    Public Sub New()
        TopMostMenuItem = New ToolStripMenuItem
        With TopMostMenuItem
            .Text = "&Top Most"
            .ShortcutKeys = Keys.Control Or Keys.T
            AddHandler .Click, AddressOf Me_Click
        End With

        ExitMenuItem = New ToolStripMenuItem
        With ExitMenuItem
            .Text = "E&xit"
            .ShortcutKeys = Keys.Control Or Keys.X
            AddHandler .Click, AddressOf Me_Click
        End With

        With Me
            .Items.Add(TopMostMenuItem)
            .Items.Add(New ToolStripSeparator)
            .Items.Add(ExitMenuItem)
        End With
    End Sub

    Private Sub Me_Click(sender As Object, e As EventArgs)

    End Sub
End Class
