Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports System.Drawing

Namespace PDFCropper
    ''' <summary>
    ''' This class inherits System.Windows.Forms.TextBox.
    ''' Comparing with System.Windows.Forms.TextBoxthe, this WaterMarkTextBox can show a water mark when it is empty.
    ''' </summary>
    Class WaterMarkTextBox
        Inherits System.Windows.Forms.TextBox

#Region "Member Variables"
        ''' <summary>
        ''' This is the water mark which is shown when the WaterMarkTextBox's text is empty.
        ''' </summary>
        Public WaterMarkText As String = ""
#End Region

#Region "Member Methods"
        ''' <summary>
        ''' This is the constructor.
        ''' </summary>
        Public Sub New()
            AddHandler Me.TextChanged, AddressOf Me.WaterMark_Toggel
            AddHandler Me.LostFocus, AddressOf Me.WaterMark_Toggel
            Multiline = False
            WaterMark_Toggel(Nothing, Nothing)
        End Sub

        ''' <summary>
        ''' Override OnPaint.
        ''' </summary>
        ''' <param name="args"></param>
        Protected Overrides Sub OnPaint(args As PaintEventArgs)
            ' Draw Text or WaterMark
            Dim Graphics As Graphics = Me.CreateGraphics
            Graphics.DrawString(WaterMarkText, (New ToolStripMenuItem).Font, SystemBrushes.GrayText, New Point(0, 0))
        End Sub

        ''' <summary>
        ''' This sub is triggered when text is changed or focus is lost.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="args"></param>
        Private Sub WaterMark_Toggel(sender As Object, args As EventArgs)
            Me.SetStyle(ControlStyles.UserPaint, Me.Text.Length <= 0)
            Refresh()
        End Sub
#End Region
    End Class
End Namespace
