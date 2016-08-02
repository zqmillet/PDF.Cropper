Public Class FormTest
    Shadows menu As MenuStrip

    Public Sub New()

        InitializeComponent()

        menu = New MenuStrip
        Dim item11 As New ToolStripMenuItem
        With item11
            .Text = "12222"
        End With
        Dim item12 As New ToolStripMenuItem
        With item12
            .Text = "23333"
        End With
        Dim item13 As New ToolStripMenuItem
        With item13
            .Text = "34444"
        End With

        Dim item1 As New ToolStripMenuItem
        With item1
            .Text = "01111"
            .DropDownItems.Add(item11)
            .DropDownItems.Add(item12)
            .DropDownItems.Add(New ToolStripSeparator)
            .DropDownItems.Add(New ToolStripMarginWidthTextBox)
            .DropDownItems.Add(New ToolStripSeparator)
            .DropDownItems.Add(item13)
        End With


        With menu
            .Items.Add(item1)
        End With

        Me.Controls.Add(menu)

    End Sub
End Class

Public Class ToolStripMarginWidthTextBox
    Inherits ToolStripControlHost

    Public Sub New()
        MyBase.New(New ControlPanel)
    End Sub

    Public ReadOnly Property ControlPanelControl() As ControlPanel
        Get
            Return CType(Me.Control, ControlPanel)
        End Get
    End Property

    Public Class ControlPanel
        Inherits Panel

        Friend WithEvents TextBox As New TextBox
        Friend WithEvents Label As New Label
        Friend WithEvents ComboBox As New ComboBox

        Public Sub New()
            Dim ToolTip As System.Windows.Forms.ToolTip = New System.Windows.Forms.ToolTip()

            With Label
                .Text = "Margin Width"
                .AutoSize = True
                .TextAlign = ContentAlignment.MiddleLeft
                .Location = New Point(0, 2)
                .BackColor = Color.Transparent
                .Anchor = AnchorStyles.Left Or AnchorStyles.Top
                .Parent = Me
                .Padding = New Padding(0, 1, 0, 0)
            End With

            With ComboBox
                .DropDownStyle = ComboBoxStyle.DropDownList
                .Width = 40
                .Location = New Point(Me.Width - .Width, 2)
                .Anchor = AnchorStyles.Top Or AnchorStyles.Right
                .Parent = Me
                .Font = Label.Font
                Dim UnitList() As String = {"pt", "mm", "cm", "inch"}
                For Each Unit As String In UnitList
                    .Items.Add(Unit)
                Next
                AddHandler .MouseLeave, AddressOf Control_MouseLeave
            End With

            With TextBox
                .Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top
                .AutoSize = False
                .Width = Me.Width - Label.Width - 12 - ComboBox.Width - 4
                .Height = ComboBox.Height
                .Location = New Point(Label.Width + 12, 2)
                .Font = Label.Font
                .Text = ""
                .Parent = Me
                .Padding = New Padding(0, 3, 0, 0)
                AddHandler .MouseLeave, AddressOf Control_MouseLeave
                AddHandler .MouseEnter, AddressOf Control_MouseEnter
            End With

            Me.BackColor = Color.Transparent
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
    End Class
End Class


