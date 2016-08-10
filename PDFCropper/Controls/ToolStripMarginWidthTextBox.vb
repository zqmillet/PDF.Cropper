Namespace PDFCropper
    ''' <summary>
    ''' This class inherits ToolStripControlHost, which can be added into the item list of a MenuScript.
    ''' There are a Label, a TextBox, and a ComboBox in this class.
    ''' So, this class allow user to change a TextBox and a ComboBox in MenuScript.
    ''' </summary>
    Public Class ToolStripMarginWidthTextBox
        Inherits ToolStripControlHost

        ''' <summary>
        ''' If the value of the TextBox, or the ComboBox is changed, this event will be triggered.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Public Event ValueChanged(sender As Object, e As EventArgs)

        ''' <summary>
        ''' This is a double which represents the value without unit.
        ''' </summary>
        ''' <returns></returns>
        Public Property Value As Double
            Get
                Return CType(Me.Control, ControlPanel).Value
            End Get
            Set(Value As Double)
                CType(Me.Control, ControlPanel).Value = Value
            End Set
        End Property

        ''' <summary>
        ''' This is a string which represents the unit of margin width.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Unit As String
            Get
                Return CType(Me.Control, ControlPanel).Unit
            End Get
        End Property

        ''' <summary>
        ''' This is a string which represents the selected index of the ComboBox.
        ''' </summary>
        ''' <returns></returns>
        Public Property UnitIndex As Integer
            Get
                Return CType(Me.Control, ControlPanel).UnitIndex
            End Get
            Set(Value As Integer)
                CType(Me.Control, ControlPanel).UnitIndex = Value
            End Set
        End Property

        ''' <summary>
        ''' This is the constructor.
        ''' </summary>
        Public Sub New()
            MyBase.New(New ControlPanel)
            Me.Width = 270
            With CType(Me.Control, ControlPanel)
                AddHandler .ValueChanged, AddressOf Me_ValueChanged
            End With
        End Sub

        ''' <summary>
        ''' This sub is triggered when the value of the TextBox or the ComboBox is changed.
        ''' </summary>
        Public Sub Me_ValueChanged()
            RaiseEvent ValueChanged(Me, Nothing)
        End Sub

        ''' <summary>
        ''' This function is used to obtain the value of margin width, the unit is pt.
        ''' </summary>
        ''' <returns></returns>
        Public Function GetValue() As Double
            Return CType(Me.Control, ControlPanel).GetValue()
        End Function

        ''' <summary>
        ''' This class is the panel which contains the Label, the TextBox, and the ComboBox.
        ''' </summary>
        Public Class ControlPanel
            Inherits Panel

            ''' <summary>
            ''' This is the TextBox.
            ''' </summary>
            Friend WithEvents TextBox As New PDFCropper.WaterMarkTextBox
            ''' <summary>
            ''' This is the Label.
            ''' </summary>
            Friend WithEvents Label As New Label
            ''' <summary>
            ''' This is the ComboBox.
            ''' </summary>
            Friend WithEvents ComboBox As New PDFCropper.ComboBox

            ''' <summary>
            ''' This event is triggered when the value of the TextBox, or the ComboBox is changed.
            ''' </summary>
            Public Event ValueChanged()

            ''' <summary>
            ''' This is a double which is the value of margin width without unit.
            ''' </summary>
            ''' <returns></returns>
            Public Property Value As Double
                Get
                    If Not IsNumeric(TextBox.Text) Then
                        Return 0
                    End If

                    If Val(TextBox.Text) < 0 Then
                        Return 0
                    End If

                    Return Val(TextBox.Text)
                End Get
                Set(Value As Double)
                    TextBox.Text = Value
                End Set
            End Property

            ''' <summary>
            ''' This is a string which represents the selected index of the ComboBox.
            ''' </summary>
            ''' <returns></returns>
            Public Property UnitIndex As Integer
                Get
                    Return ComboBox.SelectedIndex
                End Get
                Set(Value As Integer)
                    ComboBox.SelectedIndex = Value
                End Set
            End Property

            ''' <summary>
            ''' This is a string which represents the unit of margin width.
            ''' </summary>
            ''' <returns></returns>
            Public ReadOnly Property Unit As String
                Get
                    If ComboBox.SelectedIndex < 0 Then
                        Return ComboBox.Items(0)
                    Else
                        Return ComboBox.Items(ComboBox.SelectedIndex)
                    End If
                End Get
            End Property

            ''' <summary>
            ''' This is the constructor.
            ''' </summary>
            Public Sub New()
                ' Initialize the panel.
                With Me
                    .BackColor = Color.Transparent
                    .Height = 20
                    .Dock = DockStyle.Fill
                    .Padding = New Padding(0)
                    .Margin = New Padding(0)
                End With

                ' Initialize the Label.
                With Label
                    .Text = "Margin Width of New File"
                    .AutoSize = True
                    .Height = Me.Height
                    .TextAlign = ContentAlignment.MiddleLeft
                    .Location = New Point(0, 2)
                    .BackColor = Color.Transparent
                    .Anchor = AnchorStyles.Left Or AnchorStyles.Top
                    .Parent = Me
                    .Padding = New Padding(0, 2, 0, 0)
                    .Margin = New Padding(0)

                    AddHandler .SizeChanged, AddressOf Label_SizeChanged
                End With

                ' Initialize the ComboBox.
                With ComboBox
                    .Width = 50
                    .Height = Me.Height
                    .Location = New Point(Me.Width - .Width, 2)
                    .Anchor = AnchorStyles.Top Or AnchorStyles.Right
                    .Parent = Me
                    .Margin = New Padding(0)
                    Dim UnitList() As String = {"pt", "mm", "cm", "inch"}
                    For Each Unit As String In UnitList
                        .Items.Add(Unit)
                    Next

                    AddHandler .SelectedIndexChanged, AddressOf Control_ValueChanged
                    AddHandler .MouseLeave, AddressOf Control_MouseLeave
                End With

                ' Initialize the TextBox.
                With TextBox
                    .WaterMarkText = "0"
                    .Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top
                    .AutoSize = False
                    .Height = Me.Height
                    .Font = (New ToolStripMenuItem).Font
                    .Text = ""
                    .Parent = Me
                    .Margin = New Padding(0)

                    AddHandler .MouseLeave, AddressOf Control_MouseLeave
                    AddHandler .MouseEnter, AddressOf TextBox_MouseEnter
                    AddHandler .TextChanged, AddressOf Control_ValueChanged
                End With
            End Sub

            Private Sub Label_SizeChanged()
                With TextBox
                    .Width = Me.Width - Label.Width - 4 - ComboBox.Width - 4
                    .Location = New Point(Label.Width + 4, 2)
                End With
            End Sub

            ''' <summary>
            ''' This sub is triggered when mouse leaves the TextBox, or the ComboBox.
            ''' This sub is used to let the TextBox and ComboBox loss its focus automatically.
            ''' </summary>
            ''' <param name="sender"></param>
            ''' <param name="e"></param>
            Private Sub Control_MouseLeave(sender As Object, e As EventArgs)
                With CType(sender, Control)
                    Dim TabStop As Boolean = .TabStop
                    .TabStop = False
                    .Enabled = False
                    .Enabled = True
                    .TabStop = TabStop
                End With
            End Sub

            ''' <summary>
            ''' This sub is triggered when mouse enters the TextBox.
            ''' If this sub is triggered, the text of the TextBox will be selected.
            ''' </summary>
            ''' <param name="sender"></param>
            ''' <param name="e"></param>
            Private Sub TextBox_MouseEnter(sender As Object, e As EventArgs)
                TextBox.Focus()
                TextBox.SelectAll()
            End Sub

            ''' <summary>
            ''' This sub is triggered when the value of the TextBox, or the ComboBox is changed.
            ''' This sub will raise the event ValueChanged.
            ''' </summary>
            Private Sub Control_ValueChanged()
                RaiseEvent ValueChanged()
            End Sub

            ''' <summary>
            ''' This function is used to get the value of margin width with the unit pt.
            ''' </summary>
            ''' <returns></returns>
            Public Function GetValue() As Double
                ' UnitList                 = {"pt",         "mm",          "cm",       "inch"}
                Dim Proportion() As Double = {1.0F, 0.352777779F, 0.0352777764F, 0.013888889F}

                If Not IsNumeric(TextBox.Text) Then
                    Return 0
                End If

                If Val(TextBox.Text) < 0 Then
                    Return 0
                End If

                If ComboBox.SelectedIndex < 0 Then
                    Return 0
                End If

                Return 1.0F / Proportion(ComboBox.SelectedIndex) * Val(TextBox.Text)
            End Function
        End Class
    End Class
End Namespace

