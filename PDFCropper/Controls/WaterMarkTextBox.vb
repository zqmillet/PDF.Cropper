Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports System.Drawing

Namespace PDFCropper
    Class WaterMarkTextBox
        Inherits TextBox
        Private waterMarkTextEnabled As Boolean = False

#Region "Attributes"

        Private WaterMarkText As String = """"
        Public Property WaterMark() As String
            Get
                Return WaterMarkText
            End Get
            Set
                WaterMarkText = Value
                Invalidate()
            End Set
        End Property
#End Region

        'Default constructor
        Public Sub New()
            JoinEvents(True)
            Multiline = False
        End Sub

        'Override OnCreateControl ... thanks to  "lpgray .. codeproject guy"
        Protected Overrides Sub OnCreateControl()
            MyBase.OnCreateControl()
            WaterMark_Toggel(Nothing, Nothing)
        End Sub

        'Override OnPaint
        Protected Overrides Sub OnPaint(args As PaintEventArgs)
            ' Draw Text or WaterMark
            Dim Graphics As Graphics = Me.CreateGraphics
            Graphics.DrawString(WaterMarkText, (New ToolStripMenuItem).Font, SystemBrushes.GrayText, New Point(0, 0))
            ' MyBase.OnPaint(args)
        End Sub

        Private Sub JoinEvents(join As [Boolean])
            If join Then
                AddHandler Me.TextChanged, AddressOf Me.WaterMark_Toggel
                AddHandler Me.LostFocus, AddressOf Me.WaterMark_Toggel
            End If
        End Sub

        Private Sub WaterMark_Toggel(sender As Object, args As EventArgs)
            If Me.Text.Length <= 0 Then
                EnableWaterMark()
            Else
                DisbaleWaterMark()
            End If
        End Sub

        Private Sub EnableWaterMark()
            'Save current font until returning the UserPaint style to false (NOTE: It is a try and error advice)
            'oldFont = New System.Drawing.Font(Font.FontFamily, Font.Size, Font.Style, Font.Unit)
            'Enable OnPaint event handler
            Me.SetStyle(ControlStyles.UserPaint, True)
            Me.waterMarkTextEnabled = True
            'Triger OnPaint immediatly
            Refresh()
        End Sub

        Private Sub DisbaleWaterMark()
            'Disbale OnPaint event handler
            Me.waterMarkTextEnabled = False
            Me.SetStyle(ControlStyles.UserPaint, False)
        End Sub
    End Class
End Namespace
