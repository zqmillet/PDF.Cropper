Namespace PDFCropper
    ''' <summary>
    ''' This class inherits ToolStripControlHost, which can be added into the item list of a MenuScript.
    ''' There are a Label, and a TextBox in this class.
    ''' So, this class allow user to change a TextBox in MenuScript.
    ''' </summary>
    Public Class ToolStripTextBox
        Inherits ToolStripControlHost

        ''' <summary>
        ''' If the text of the TextBox is changed, this event will be triggered.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Public Shadows Event TextChanged(sender As Object, e As EventArgs)

        ''' <summary>
        ''' This is a string which represents the text of the Label.
        ''' </summary>
        ''' <returns></returns>
        Public Property LabelText As String
            Get
                Return CType(Me.Control, ControlPanel).LabelText
            End Get
            Set(Value As String)
                CType(Me.Control, ControlPanel).LabelText = Value
            End Set
        End Property

        ''' <summary>
        ''' This is a string which represents the text of the TextBox.
        ''' </summary>
        ''' <returns></returns>
        Public Shadows Property Text As String
            Get
                Return CType(Me.Control, ControlPanel).Text
            End Get
            Set(Value As String)
                CType(Me.Control, ControlPanel).Text = Value
            End Set
        End Property

        ''' <summary>
        ''' This is the constructor.
        ''' </summary>
        Public Sub New()
            MyBase.New(New ControlPanel)
            Me.Width = 270
            With CType(Me.Control, ControlPanel)
                AddHandler .TextChanged, AddressOf Me_TextChanged
            End With
        End Sub

        ''' <summary>
        ''' This sub is triggered when the text of the TextBox is changed.
        ''' </summary>
        Public Sub Me_TextChanged()
            RaiseEvent TextChanged(Me, Nothing)
        End Sub

        ''' <summary>
        ''' This class is the panel which contains the Label, and the TextBox.
        ''' </summary>
        Public Class ControlPanel
            Inherits Panel

            ''' <summary>
            ''' This is the TextBox.
            ''' </summary>
            Friend WithEvents TextBox As New TextBox
            ''' <summary>
            ''' This is the Label.
            ''' </summary>
            Friend WithEvents Label As New Label

            ''' <summary>
            ''' This is a string which represents the text of the Label.
            ''' </summary>
            ''' <returns></returns>
            Public Property LabelText As String
                Get
                    Return Label.Text
                End Get
                Set(Value As String)
                    Label.Text = Value
                End Set
            End Property

            ''' <summary>
            ''' This is a string which represents the text of the TextBox.
            ''' </summary>
            ''' <returns></returns>
            Public Shadows Property Text As String
                Get
                    Return TextBox.Text
                End Get
                Set(Value As String)
                    TextBox.Text = Value
                End Set
            End Property

            ''' <summary>
            ''' This sub is triggered when the text of the TextBox is changed.
            ''' </summary>
            Public Shadows Event TextChanged()

            ''' <summary>
            ''' This is the constructor.
            ''' </summary>
            Public Sub New()
                ' Initialize the panel.
                With Me
                    .BackColor = Color.Transparent
                    .Height = 22
                    .Padding = New Padding(0)
                    .Margin = New Padding(0)
                End With

                ' Initialize the Label.
                With Label
                    .Text = ""
                    .Height = Me.Height
                    .AutoSize = True
                    .Location = New Point(0, 0)
                    .Padding = New Padding(0, 2, 0, 0)
                    .Margin = New Padding(0)
                    .Anchor = AnchorStyles.Top Or AnchorStyles.Left
                    .Parent = Me
                End With

                ' Initialize the TextBox.
                With TextBox
                    .AutoSize = False
                    .Width = 50
                    .Height = (New ComboBox).Height
                    .Location = New Point(Me.Width - .Width, 0)
                    .Anchor = AnchorStyles.Left Or AnchorStyles.Top Or AnchorStyles.Right
                    .Margin = New Padding(0)
                    .Font = (New ToolStripMenuItem).Font
                    .Parent = Me
                    AddHandler .TextChanged, AddressOf TextBox_TextChanged
                    AddHandler .MouseEnter, AddressOf TextBox_MouseEnter
                    AddHandler .MouseLeave, AddressOf TextBox_MouseLeave
                End With
            End Sub

            ''' <summary>
            ''' This sub is triggered when mouse leaves the TextBox.
            ''' This sub is used to let the TextBox loss its focus automatically.
            ''' </summary>
            ''' <param name="sender"></param>
            ''' <param name="e"></param>
            Private Sub TextBox_MouseLeave(sender As Object, e As EventArgs)
                With TextBox
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
            ''' This sub is triggered when the value of the TextBox is changed.
            ''' This sub will raise the event ValueChanged.
            ''' </summary>
            Private Sub TextBox_TextChanged()
                RaiseEvent TextChanged()
            End Sub
        End Class
    End Class
End Namespace

