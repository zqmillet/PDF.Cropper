Namespace PDFCropper
    ''' <summary>
    ''' This ComboBox inherits from System.Windows.Forms.ComboBox.
    ''' Comparing with the System.Windows.Forms.ComboBox, this ComboBox can set its height and padding independently.
    ''' So, the base line of its text can overlap with the base line of the TextBox's text.
    ''' </summary>
    Public Class ComboBox
        Inherits System.Windows.Forms.ComboBox

        ''' <summary>
        ''' This variable is used to replace the member variable Font.
        ''' The member variable Font is just used to control the height of this ComboBox.
        ''' This variable is used to draw the text of this ComboBox.
        ''' </summary>
        Dim ItemFont As Font

        ''' <summary>
        ''' This is the constructor of the ComboBox.
        ''' </summary>
        Public Sub New()
            With Me
                .DropDownStyle = ComboBoxStyle.DropDownList
                .DrawMode = DrawMode.OwnerDrawVariable
                .Margin = New Padding(0)
                .Padding = New Padding(0)
                ' This code is not used to set the font of this ComboBox, but is used to set the height of this ComboBox.
                .Font = New Font(Font.FontFamily, 7.5, FontStyle.Regular)
                ' This code is used to set the font of this ComboBox.
                .ItemFont = (New ToolStripMenuItem).Font
                .SetAutoSizeMode(AutoSizeMode.GrowAndShrink)
                AddHandler .DrawItem, AddressOf Me_DrawItem
            End With
        End Sub

        ''' <summary>
        ''' This sub is used to draw the items in the ComboBox.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
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

