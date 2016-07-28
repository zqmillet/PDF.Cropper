Public Class FormMain

    Dim Console As Console
    Dim CoverPanel As CoverPanel
    Dim MouseDownPoint As Point
    Dim RightClickMenu As RightClickMenu

    Public Sub New()
        InitializeComponent()

        RightClickMenu = New RightClickMenu

        Console = New Console
        With Console
            .AppendText("This is PDF Cropper by Qiqi." & vbCrLf & "Please drag .pdf files into this form." & vbCrLf & vbCrLf)
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
End Class
