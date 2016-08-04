
Namespace PDFCropper
    Public Class ComboBox
        Inherits System.Windows.Forms.ComboBox

        Dim ItemFont As Font

        Public Sub New()
            With Me
                .DropDownStyle = ComboBoxStyle.DropDownList
                .DrawMode = DrawMode.OwnerDrawVariable
                .Margin = New Padding(0)
                .Padding = New Padding(0)
                .Font = New Font(Font.FontFamily, 7.5, FontStyle.Regular)
                .SetAutoSizeMode(AutoSizeMode.GrowAndShrink)
                .ItemFont = (New ToolStripMenuItem).Font
                AddHandler .DrawItem, AddressOf Me_DrawItem
            End With
        End Sub

        Private Sub Me_DrawItem(ByVal sender As Object, ByVal e As DrawItemEventArgs)
            If e.Index < 0 Then
                Exit Sub
            End If

            e.DrawBackground()

            Dim Brush As Brush
            If ((e.State And DrawItemState.Selected) _
                <> DrawItemState.None) Then
                Brush = SystemBrushes.HighlightText
            Else
                Brush = SystemBrushes.WindowText
            End If

            e.Graphics.DrawString(Me.Items(e.Index), ItemFont, Brush, e.Bounds.X, e.Bounds.Y - 1)
            ' Draw the focus Rectangle if appropriate
            If ((e.State And DrawItemState.NoFocusRect) _
                    = DrawItemState.None) Then
                e.DrawFocusRectangle()
            End If

        End Sub



    End Class
End Namespace

