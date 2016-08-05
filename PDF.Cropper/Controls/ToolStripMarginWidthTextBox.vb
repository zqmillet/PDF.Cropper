Namespace PDFCropper
    Public Class ToolStripMarginWidthTextBox
        Inherits ToolStripControlHost

        Public Event ValueChanged(sender As Object, e As EventArgs)

        Public Property Value As Double
            Get
                Return CType(Me.Control, ControlPanel).Value
            End Get
            Set(Value As Double)
                CType(Me.Control, ControlPanel).Value = Value
            End Set
        End Property

        Public ReadOnly Property Unit As String
            Get
                Return CType(Me.Control, ControlPanel).Unit
            End Get
        End Property

        Public Property UnitIndex As Integer
            Get
                Return CType(Me.Control, ControlPanel).UnitIndex
            End Get
            Set(Value As Integer)
                CType(Me.Control, ControlPanel).UnitIndex = Value
            End Set
        End Property

        Public Sub New()
            MyBase.New(New ControlPanel)
            Me.Width = 250
            With CType(Me.Control, ControlPanel)
                AddHandler .ValueChanged, AddressOf Me_ValueChanged
            End With
        End Sub

        Public Sub Me_ValueChanged()
            RaiseEvent ValueChanged(Me, Nothing)
        End Sub

        Public ReadOnly Property ControlPanelControl() As ControlPanel
            Get
                Return CType(Me.Control, ControlPanel)
            End Get
        End Property

        Public Function GetValue() As Double
            Return CType(Me.Control, ControlPanel).GetValue()
        End Function

        Public Class ControlPanel
            Inherits Panel

            Friend WithEvents TextBox As New PDFCropper.WaterMarkTextBox
            Friend WithEvents Label As New Label
            Friend WithEvents ComboBox As New PDFCropper.ComboBox

            Public Event ValueChanged()

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

            Public Property UnitIndex As Integer
                Get
                    Return ComboBox.SelectedIndex
                End Get
                Set(Value As Integer)
                    ComboBox.SelectedIndex = Value
                End Set
            End Property

            Public ReadOnly Property Unit As String
                Get
                    If ComboBox.SelectedIndex < 0 Then
                        Return ComboBox.Items(0)
                    Else
                        Return ComboBox.Items(ComboBox.SelectedIndex)
                    End If
                End Get
            End Property

            Public Sub New()
                Dim ToolTip As System.Windows.Forms.ToolTip = New System.Windows.Forms.ToolTip()

                With Me
                    .BackColor = Color.Transparent
                    .Height = 20
                    .Dock = DockStyle.Fill
                    .Padding = New Padding(0)
                    .Margin = New Padding(0)
                End With

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
                End With

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
                    'AddHandler .MouseLeave, AddressOf Control_MouseLeave
                    'AddHandler .MouseEnter, AddressOf Control_MouseEnter
                End With

                With TextBox
                    .WaterMark = "0"
                    .Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top
                    .AutoSize = False
                    .Width = Me.Width - Label.Width - 12 - ComboBox.Width - 4
                    .Height = Me.Height
                    .Location = New Point(Label.Width + 12, 2)
                    .Font = (New ToolStripMenuItem).Font
                    .Text = ""
                    .Parent = Me
                    .Margin = New Padding(0)
                    AddHandler .MouseLeave, AddressOf Control_MouseLeave
                    AddHandler .MouseEnter, AddressOf Control_MouseEnter
                    AddHandler .TextChanged, AddressOf Control_ValueChanged
                End With

            End Sub

            Private Sub Control_MouseLeave(sender As Object, e As EventArgs)
                With CType(sender, Control)
                    Dim TabStop As Boolean = .TabStop
                    .TabStop = False
                    .Enabled = False
                    .Enabled = True
                    .TabStop = TabStop
                End With
            End Sub

            Private Sub Control_MouseEnter(sender As Object, e As EventArgs)
                If sender Is TextBox Then
                    TextBox.Focus()
                    TextBox.SelectAll()
                End If

                If sender Is ComboBox Then
                    ComboBox.Focus()
                End If
            End Sub

            Private Sub Control_ValueChanged()
                RaiseEvent ValueChanged()
            End Sub

            Public Function GetValue() As Double
                ' UnitList = {"pt", "mm", "cm", "inch"}
                Dim Proportion() As Double = {1, 0.352777778, 0.0352777778, 0.013888888888889}

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

