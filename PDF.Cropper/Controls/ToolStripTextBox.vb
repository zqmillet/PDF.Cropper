Namespace PDFCropper
    Public Class ToolStripTextBox
        Inherits ToolStripControlHost

        Public Shadows Event TextChanged(sender As Object, e As EventArgs)

        Public Property LabelText As String
            Get
                Return CType(Me.Control, ControlPanel).LabelText
            End Get
            Set(Value As String)
                CType(Me.Control, ControlPanel).LabelText = Value
            End Set
        End Property

        Public Shadows Property Text As String
            Get
                Return CType(Me.Control, ControlPanel).Text
            End Get
            Set(Value As String)
                CType(Me.Control, ControlPanel).Text = Value
            End Set
        End Property

        Public Sub New()
            MyBase.New(New ControlPanel)

            With CType(Me.Control, ControlPanel)
                AddHandler .TextChanged, AddressOf Me_TextChanged
            End With
        End Sub

        Public Sub Me_TextChanged()
            RaiseEvent TextChanged(Me, Nothing)
        End Sub

        Public Class ControlPanel
            Inherits Panel

            Friend WithEvents TextBox As New TextBox
            Friend WithEvents Label As New Label

            Public Property LabelText As String
                Get
                    Return Label.Text
                End Get
                Set(Value As String)
                    Label.Text = Value
                End Set
            End Property

            Public Shadows Property Text As String
                Get
                    Return TextBox.Text
                End Get
                Set(Value As String)
                    TextBox.Text = Value
                End Set
            End Property

            Public Shadows Event TextChanged()

            Public Sub New()
                Dim ToolTip As System.Windows.Forms.ToolTip = New System.Windows.Forms.ToolTip()

                With Me
                    .BackColor = Color.Transparent
                    .Height = 22
                    .Padding = New Padding(0)
                    .Margin = New Padding(0)
                End With

                With TextBox
                    .AutoSize = False
                    .Width = 50
                    .Height = (New ComboBox).Height
                    .Location = New Point(Me.Width - .Width, 0)
                    .Anchor = AnchorStyles.Left Or AnchorStyles.Top Or AnchorStyles.Right
                    .Margin = New Padding(0)
                    .Font = (New ToolStripMenuItem).Font
                    AddHandler .TextChanged, AddressOf TextBox_TextChanged
                    AddHandler .MouseEnter, AddressOf TextBox_MouseEnter
                    AddHandler .MouseLeave, AddressOf TextBox_MouseLeave
                End With

                With Label
                    .Text = ""
                    .Height = Me.Height
                    .AutoSize = True
                    .Location = New Point(0, 0)
                    .Padding = New Padding(0, 2, 0, 0)
                    .Margin = New Padding(0)
                    .Anchor = AnchorStyles.Top Or AnchorStyles.Left
                End With

                Me.Controls.Add(Label)
                Me.Controls.Add(TextBox)
            End Sub

            Private Sub TextBox_MouseLeave(sender As Object, e As EventArgs)
                With TextBox
                    Dim TabStop As Boolean = .TabStop
                    .TabStop = False
                    .Enabled = False
                    .Enabled = True
                    .TabStop = TabStop
                End With
            End Sub

            Private Sub TextBox_MouseEnter(sender As Object, e As EventArgs)
                TextBox.Focus()
                TextBox.SelectAll()
            End Sub

            Private Sub TextBox_TextChanged()
                RaiseEvent TextChanged()
            End Sub
        End Class
    End Class
End Namespace

