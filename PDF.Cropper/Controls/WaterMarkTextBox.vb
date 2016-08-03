Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports System.Drawing

Namespace PDFCropper
    Class WaterMarkTextBox
        Inherits TextBox
        Private oldFont As Font = Nothing
        Private waterMarkTextEnabled As [Boolean] = False

#Region "Attributes"
        Private _waterMarkColor As Color = Color.Gray
        Public Property WaterMarkColor() As Color
            Get
                Return _waterMarkColor
            End Get
            Set
                _waterMarkColor = Value
                'thanks to Bernhard Elbl for Invalidate()
                Invalidate()
            End Set
        End Property

        Private _waterMarkText As String = "Water Mark"
        Public Property WaterMarkText() As String
            Get
                Return _waterMarkText
            End Get
            Set
                _waterMarkText = Value
                Invalidate()
            End Set
        End Property
#End Region

        'Default constructor
        Public Sub New()
            JoinEvents(True)
        End Sub

        'Override OnCreateControl ... thanks to  "lpgray .. codeproject guy"
        Protected Overrides Sub OnCreateControl()
            MyBase.OnCreateControl()
            WaterMark_Toggel(Nothing, Nothing)
        End Sub

        'Override OnPaint
        Protected Overrides Sub OnPaint(args As PaintEventArgs)
            ' Use the same font that was defined in base class
            Dim drawFont As New System.Drawing.Font(Font.FontFamily, Font.Size, Font.Style, Font.Unit)
            ' Create new brush with gray color or 
            Dim drawBrush As New SolidBrush(WaterMarkColor)
            ' use Water mark color
            ' Draw Text or WaterMark
            args.Graphics.DrawString((If(waterMarkTextEnabled, WaterMarkText, Text)), drawFont, drawBrush, New PointF(0.0F, 0.0F))
            MyBase.OnPaint(args)
        End Sub

        Private Sub JoinEvents(join As [Boolean])
            If join Then
                AddHandler Me.TextChanged, AddressOf Me.WaterMark_Toggel
                AddHandler Me.LostFocus, AddressOf Me.WaterMark_Toggel
                'No one of the above events will start immeddiatlly 
                'TextBox control still in constructing, so,
                'Font object (for example) couldn't be catched from within WaterMark_Toggle
                'So, call WaterMark_Toggel through OnCreateControl after TextBox is totally created
                'No doupt, it will be only one time call

                'Old solution uses Timer.Tick event to check Create property
                AddHandler Me.FontChanged, AddressOf Me.WaterMark_FontChanged
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
            oldFont = New System.Drawing.Font(Font.FontFamily, Font.Size, Font.Style, Font.Unit)
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
            'Return back oldFont if existed
            If oldFont IsNot Nothing Then
                Me.Font = New System.Drawing.Font(oldFont.FontFamily, oldFont.Size, oldFont.Style, oldFont.Unit)
            End If
        End Sub

        Private Sub WaterMark_FontChanged(sender As Object, args As EventArgs)
            If waterMarkTextEnabled Then
                oldFont = New System.Drawing.Font(Font.FontFamily, Font.Size, Font.Style, Font.Unit)
                Refresh()
            End If
        End Sub
    End Class
End Namespace
