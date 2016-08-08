Namespace PDFCropper
    ''' <summary>
    ''' This RichTextBox inherits from ystem.Windows.Forms.RichTextBox.
    ''' This RichTextBox fix the font bug of System.Windows.Forms.RichTextBox.
    ''' </summary>
    Public Class RichTextBox
        Inherits System.Windows.Forms.RichTextBox

        ''' <summary>
        ''' This function is used to lock update of the RichTextBox.
        ''' </summary>
        ''' <param name="hWnd"></param>
        ''' <returns></returns>
        Private Declare Function LockWindowUpdate Lib "user32" (ByVal hWnd As Integer) As Integer

        ''' <summary>
        ''' This is the constructor.
        ''' </summary>
        Public Sub New()
            With Me
                .Multiline = True
                .Dock = DockStyle.Fill
                .BorderStyle = BorderStyle.None
                .ScrollBars = RichTextBoxScrollBars.None
                .Font = New Font("Consolas", 10, FontStyle.Bold)
            End With
        End Sub

        ''' <summary>
        ''' Overrides the event TextChanged.
        ''' </summary>
        ''' <param name="e"></param>
        Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
            TextFontUpdate()
        End Sub

        ''' <summary>
        ''' There is a font bug of System.Windows.Forms.RichTextBox which is dscribed as follows.
        ''' If there exists a character and it is not in the Me.Font, the System.Windows.Forms.RichTextBox will assign this character's font with other font.
        ''' But, the font of the characters behinds this character is not Me.Font. It looks strange.
        ''' This sub is used to fix this font bug.
        ''' </summary>
        Public Sub TextFontUpdate()
            Dim SelectionAt As Integer = Me.SelectionStart

            LockWindowUpdate(Me.Handle.ToInt32)

            Me.SelectionStart = 0
            Me.SelectionLength = Me.TextLength
            Me.SelectionFont = Me.Font

            Me.SelectionStart = SelectionAt
            Me.SelectionLength = 0

            ' Unlock the update
            LockWindowUpdate(0)
        End Sub
    End Class
End Namespace

