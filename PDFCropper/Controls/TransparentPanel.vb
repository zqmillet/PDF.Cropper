Namespace PDFCropper
    ''' <summary>
    ''' This class is a panel which is tansparent.
    ''' This class is used to cover the other control, which should be click by mouse.
    ''' </summary>
    Public Class TransparentPanel
        Inherits System.Windows.Forms.Panel

        Public Sub New()
            With Me
                .Dock = DockStyle.Fill
                .BorderStyle = BorderStyle.None
            End With
        End Sub

        ''' <summary>
        ''' CreateParams is a property of Control object.
        ''' CreateParams is used to gets the required creation parameters when the control handle is created.
        ''' In this class, we override the property CreateParams,
        ''' and set the ExStyle = ExStyle Or 0x20 to make the background is trasparent.
        ''' </summary>
        ''' <returns></returns>
        Protected Overrides ReadOnly Property CreateParams() As System.Windows.Forms.CreateParams
            Get
                Dim cp As CreateParams = MyBase.CreateParams
                cp.ExStyle = cp.ExStyle Or &H20
                Return cp
            End Get
        End Property

        ''' <summary>
        ''' We override the sub OnPaintBackground to prevent the background of this panel from being painted.
        ''' </summary>
        ''' <param name="e"></param>
        Protected Overrides Sub OnPaintBackground(ByVal e As System.Windows.Forms.PaintEventArgs)
            ' Do nothing.
        End Sub
        '        Protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
        '  If (keyData == (Keys.Control | Keys.F)) {
        '    MessageBox.Show("What the Ctrl+F?");
        '    Return True;
        '  }
        '  Return base.ProcessCmdKey(ref msg, keyData);
        '}
    End Class
End Namespace
