Public Class FormMain

    Dim Console As Console
    Dim CoverPanel As CoverPanel
    Dim MouseDownPoint As Point
    Dim RightClickMenu As RightClickMenu
    Dim Configuration As Configuration
    Dim ConfigurationPath As String = Application.StartupPath & "\Configuration.xml"

    Public Sub New()
        InitializeComponent()

        RightClickMenu = New RightClickMenu
        With RightClickMenu
            AddHandler .TopMostMenuItem_Click, AddressOf TopMostMenuItem_Click
            AddHandler .ExitMenuItem_Click, AddressOf ExitMenuItem_Click
            AddHandler .OpacityMenuItem_Click, AddressOf OpacityMenuItem_Click
            AddHandler .BackColorMenuItem_Click, AddressOf BackColorMenuItem_Click
            AddHandler .ForeColorMenuItem_Click, AddressOf ForeColorMenuItem_Click
        End With

        Console = New Console
        With Console
            .AppendText("This is PDF Cropper made by Qiqi." & vbCrLf & "Please drag .pdf files into this form." & vbCrLf & vbCrLf)
        End With

        CoverPanel = New CoverPanel
        With CoverPanel
            AddHandler .MouseDown, AddressOf CoverPanel_MouseDown
            AddHandler .MouseUp, AddressOf CoverPanel_MouseUp
            AddHandler .MouseMove, AddressOf CoverPanel_MouseMove
            AddHandler .MouseClick, AddressOf CoverPanel_MouseClick
        End With

        With Me
            .Opacity = 1
            .FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
            .Controls.Add(CoverPanel)
            .Controls.Add(Console)
        End With

        Configuration = New Configuration
        If Not Configuration.Load(ConfigurationPath) Then
            Exit Sub
        End If

        RightClickMenu.FormMainTopMost = Configuration.GetTagValue(RightClickMenu.TopMostMenuItem.Name)
        RightClickMenu.FormMainOpacity = Val(Configuration.GetTagValue(RightClickMenu.OpacityMenuItem.Name))
        RightClickMenu.FormMainBackColor = Color.FromName(Configuration.GetTagValue(RightClickMenu.BackColorMenuItem.Name))
        RightClickMenu.FormMainForeColor = Color.FromName(Configuration.GetTagValue(RightClickMenu.ForeColorMenuItem.Name))
    End Sub

    Private Sub CoverPanel_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs)
        Me.Cursor = Cursors.SizeAll
        MouseDownPoint = e.Location
    End Sub

    Private Sub CoverPanel_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs)
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub CoverPanel_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs)
        If Not Me.Cursor Is Cursors.SizeAll Then
            Exit Sub
        End If

        Me.Location = New Point(Me.Location.X - MouseDownPoint.X + e.Location.X,
                                Me.Location.Y - MouseDownPoint.Y + e.Location.Y)
    End Sub

    Private Sub CoverPanel_MouseClick(sender As Object, e As MouseEventArgs)
        If e.Button = MouseButtons.Right Then
            RightClickMenu.Show(CoverPanel, e.X, e.Y)
        End If
    End Sub

    Private Sub TopMostMenuItem_Click(sender As Object)
        With CType(sender, ToolStripMenuItem)
            Me.TopMost = .Checked
            Configuration.SetTagValue(.Name, .Checked)
        End With
    End Sub

    Private Sub ExitMenuItem_Click(sender As Object)
        Configuration.Save()
        End
    End Sub

    Private Sub OpacityMenuItem_Click(sender As Object)
        With CType(sender, ToolStripMenuItem)
            Me.Opacity = .Tag
            Configuration.SetTagValue(.Name, .Tag)
        End With
    End Sub

    Private Sub BackColorMenuItem_Click(sender As Object)
        With CType(sender, ToolStripMenuItem)
            Console.BackColor = .Tag
            Configuration.SetTagValue(.Name, .Tag.Name)
        End With
    End Sub

    Private Sub ForeColorMenuItem_Click(sender As Object)
        With CType(sender, ToolStripMenuItem)
            Console.ForeColor = .Tag
            Configuration.SetTagValue(.Name, .Tag.Name)
        End With
    End Sub
End Class
