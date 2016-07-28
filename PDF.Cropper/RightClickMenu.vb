Public Class RightClickMenu
    Inherits ContextMenuStrip

    Public TopMostMenuItem As ToolStripMenuItem
    Public ExitMenuItem As ToolStripMenuItem
    Public OpacityMenuItem As ToolStripMenuItem
    Public BackColorMenuItem As ToolStripMenuItem
    Public ForeColorMenuItem As ToolStripMenuItem

    Public ColorList() As String = {"Black", "Blue", "Lime", "Cyan", "Red", "Fuchsia", "Yellow", "White", "Navy", "Green", "Teal", "Maroon", "Purple", "Olive", "Gray"}

    Public Event TopMostMenuItem_Click(sender As Object)
    Public Event ExitMenuItem_Click(sender As Object)
    Public Event OpacityMenuItem_Click(sender As Object)
    Public Event BackColorMenuItem_Click(sender As Object)
    Public Event ForeColorMenuItem_Click(sender As Object)

    Public WriteOnly Property FormMainTopMost As Boolean
        Set(Value As Boolean)
            TopMostMenuItem.Checked = Value
            RaiseEvent TopMostMenuItem_Click(TopMostMenuItem)
        End Set
    End Property

    Public WriteOnly Property FormMainOpacity As Double
        Set(Value As Double)
            For Each MenuItem As ToolStripMenuItem In OpacityMenuItem.DropDownItems
                If MenuItem.Tag = Value Then
                    MenuItem.Checked = True
                    RaiseEvent OpacityMenuItem_Click(MenuItem)
                End If
            Next
        End Set
    End Property

    Public WriteOnly Property FormMainBackColor As Color
        Set(Value As Color)
            For Each MenuItem As ToolStripMenuItem In BackColorMenuItem.DropDownItems
                If MenuItem.Tag = Value Then
                    BackColorMenuItem.Image = MenuItem.Image
                    RaiseEvent BackColorMenuItem_Click(MenuItem)
                End If
            Next
        End Set
    End Property

    Public WriteOnly Property FormMainForeColor As Color
        Set(Value As Color)
            For Each MenuItem As ToolStripMenuItem In ForeColorMenuItem.DropDownItems
                If MenuItem.Tag = Value Then
                    ForeColorMenuItem.Image = MenuItem.Image
                    RaiseEvent ForeColorMenuItem_Click(MenuItem)
                End If
            Next
        End Set
    End Property

    Public Sub New()
        TopMostMenuItem = New ToolStripMenuItem
        With TopMostMenuItem
            .Text = "&Top Most"
            .Name = NameOf(TopMostMenuItem)
            .CheckOnClick = True
            AddHandler .Click, AddressOf Me_Click
        End With

        ExitMenuItem = New ToolStripMenuItem
        With ExitMenuItem
            .Text = "E&xit"
            .Name = NameOf(ExitMenuItem)
            AddHandler .Click, AddressOf Me_Click
        End With

        OpacityMenuItem = New ToolStripMenuItem
        With OpacityMenuItem
            .Text = "&Opacity"
            .Name = NameOf(OpacityMenuItem)
            For Index As Integer = 1 To 10
                Dim MenuItem As New ToolStripMenuItem
                With MenuItem
                    .Text = (Index * 10).ToString & "%"
                    .Name = NameOf(OpacityMenuItem)
                    .Tag = Index / 10
                    .CheckOnClick = True
                    AddHandler .Click, AddressOf Me_Click
                End With
                .DropDownItems.Add(MenuItem)
            Next
        End With

        BackColorMenuItem = New ToolStripMenuItem
        With BackColorMenuItem
            .Text = "&Back Color"
            .Name = NameOf(BackColorMenuItem)
            For Each ColorName As String In ColorList
                Dim MenuItem As New ToolStripMenuItem
                With MenuItem
                    .Text = ColorName
                    .Name = NameOf(BackColorMenuItem)
                    .Tag = Color.FromName(ColorName)
                    .Image = GetImageByColor(.Tag)
                    AddHandler .Click, AddressOf Me_Click
                End With
                .DropDownItems.Add(MenuItem)
            Next
        End With

        ForeColorMenuItem = New ToolStripMenuItem
        With ForeColorMenuItem
            .Text = "&Fore Color"
            .Name = NameOf(ForeColorMenuItem)
            For Each ColorName As String In ColorList
                Dim MenuItem As New ToolStripMenuItem
                With MenuItem
                    .Text = ColorName
                    .Name = NameOf(ForeColorMenuItem)
                    .Tag = Color.FromName(ColorName)
                    .Image = GetImageByColor(.Tag)
                    AddHandler .Click, AddressOf Me_Click
                End With
                .DropDownItems.Add(MenuItem)
            Next
        End With

        With Me
            .Items.Add(TopMostMenuItem)
            .Items.Add(New ToolStripSeparator)
            .Items.Add(OpacityMenuItem)
            .Items.Add(BackColorMenuItem)
            .Items.Add(ForeColorMenuItem)
            .Items.Add(New ToolStripSeparator)
            .Items.Add(ExitMenuItem)
        End With
    End Sub

    Private Function GetImageByColor(ByVal Color As Color)
        Dim Image As Image = New Bitmap(20, 20)
        Dim Graphics As Graphics = Graphics.FromImage(Image)
        Graphics.Clear(Color)
        Return Image
    End Function

    Private Sub Me_Click(sender As Object, e As EventArgs)
        If sender Is TopMostMenuItem Then
            RaiseEvent TopMostMenuItem_Click(sender)
            Exit Sub
        End If

        If sender Is ExitMenuItem Then
            RaiseEvent ExitMenuItem_Click(sender)
            Exit Sub
        End If

        If sender.Name = OpacityMenuItem.Name Then
            For Each MenuItem As ToolStripMenuItem In OpacityMenuItem.DropDownItems
                MenuItem.Checked = MenuItem Is sender
            Next
            RaiseEvent OpacityMenuItem_Click(sender)
            Exit Sub
        End If

        If sender.Name = BackColorMenuItem.Name Then
            For Each MenuItem As ToolStripMenuItem In BackColorMenuItem.DropDownItems
                BackColorMenuItem.Image = sender.Image
            Next
            RaiseEvent BackColorMenuItem_Click(sender)
            Exit Sub
        End If

        If sender.Name = ForeColorMenuItem.Name Then
            For Each MenuItem As ToolStripMenuItem In ForeColorMenuItem.DropDownItems
                ForeColorMenuItem.Image = sender.Image
            Next
            RaiseEvent ForeColorMenuItem_Click(sender)
            Exit Sub
        End If
    End Sub


End Class
