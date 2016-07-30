Public Class FormMain

    Dim Console As Console
    Dim CoverPanel As CoverPanel
    Dim MouseDownPoint As Point
    Dim RightClickMenu As RightClickMenu
    Dim Configuration As Configuration
    Dim ConfigurationPath As String = Application.StartupPath & "\Configuration.xml"
    Dim OldLocation As New Point
    Dim GhostScriptBinFolder As String

    Public Sub New()
        InitializeComponent()

        RightClickMenu = New RightClickMenu
        With RightClickMenu
            AddHandler .TopMostMenuItem_Click, AddressOf TopMostMenuItem_Click
            AddHandler .ExitMenuItem_Click, AddressOf ExitMenuItem_Click
            AddHandler .OpacityMenuItem_Click, AddressOf OpacityMenuItem_Click
            AddHandler .BackColorMenuItem_Click, AddressOf BackColorMenuItem_Click
            AddHandler .ForeColorMenuItem_Click, AddressOf ForeColorMenuItem_Click
            AddHandler .FontSizeMenuItem_Click, AddressOf FontSizeMenuItem_Click
            AddHandler .FontNameMenuItem_Click, AddressOf FontNameMenuItem_Click
            AddHandler .GhostScriptPathMenuItem_Click, AddressOf GhostScriptPathMenuItem_Click
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

        GhostScriptBinFolder = Configuration.GetTagValue(RightClickMenu.GhostScriptPathMenuItem.Name)

        RightClickMenu.FormMainTopMost = Configuration.GetTagValue(RightClickMenu.TopMostMenuItem.Name)
        RightClickMenu.FormMainOpacity = Val(Configuration.GetTagValue(RightClickMenu.OpacityMenuItem.Name))
        RightClickMenu.FormMainBackColor = Color.FromName(Configuration.GetTagValue(RightClickMenu.BackColorMenuItem.Name))
        RightClickMenu.FormMainForeColor = Color.FromName(Configuration.GetTagValue(RightClickMenu.ForeColorMenuItem.Name))
        RightClickMenu.FormMainFontSize = Val(Configuration.GetTagValue(RightClickMenu.FontSizeMenuItem.Name))
        RightClickMenu.FormMainFontName = Configuration.GetTagValue(RightClickMenu.FontNameMenuItem.Name)
        RightClickMenu.GhostScriptBinFolder = IsGhostScripBinFolder(GhostScriptBinFolder)
    End Sub

    Private Sub CoverPanel_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs)
        Me.Cursor = Cursors.SizeAll
        MouseDownPoint = e.Location
        OldLocation = Me.Location
    End Sub

    Private Sub CoverPanel_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs)
        Me.Cursor = Cursors.Default

        If Not Me.Location = OldLocation Then
            ShowMessage("The form has been moved to (" & Me.Location.X & ", " & Me.Location.Y & ").")
        End If
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

        If Me.TopMost Then
            ShowMessage("The form is topmost.")
        Else
            ShowMessage("The form is not topmost.")
        End If
    End Sub

    Private Sub ExitMenuItem_Click(sender As Object)
        Configuration.Save()
        ShowMessage("The configuration has been saved.")
        Threading.Thread.Sleep(1000)
        End
    End Sub

    Private Sub OpacityMenuItem_Click(sender As Object)
        With CType(sender, ToolStripMenuItem)
            Me.Opacity = .Tag
            Configuration.SetTagValue(.Name, .Tag)
        End With
        ShowMessage("The opacity of the form is changed to " & Me.Opacity * 100 & "%.")
    End Sub

    Private Sub BackColorMenuItem_Click(sender As Object)
        With CType(sender, ToolStripMenuItem)
            Console.BackColor = .Tag
            Configuration.SetTagValue(.Name, .Tag.Name)
        End With

        With Console.BackColor
            Dim ColorName As String = ""
            If .IsNamedColor Then
                ColorName = .Name
            End If
            ShowMessage("The back color of the form is change to " & ColorName & "(" & .R & ", " & .G & ", " & .B & ").")
        End With
    End Sub

    Private Sub ForeColorMenuItem_Click(sender As Object)
        With CType(sender, ToolStripMenuItem)
            Console.ForeColor = .Tag
            Configuration.SetTagValue(.Name, .Tag.Name)
        End With

        With Console.ForeColor
            Dim ColorName As String = ""
            If .IsNamedColor Then
                ColorName = .Name
            End If
            ShowMessage("The fore color of the form is change to " & ColorName & "(" & .R & ", " & .G & ", " & .B & ").")
        End With
    End Sub

    Private Sub FontSizeMenuItem_Click(sender As Object)
        With CType(sender, ToolStripMenuItem)
            Console.Font = New Font(Console.Font.FontFamily, .Tag)
            Configuration.SetTagValue(.Name, .Tag)
        End With
        ShowMessage("The font size of the message is changed to " & Console.Font.Size & "pt.")
    End Sub

    Private Sub FontNameMenuItem_Click(sender As Object)
        With CType(sender, ToolStripMenuItem)
            Console.Font = New Font(.Tag.Name.ToString, Console.Font.Size)
            Configuration.SetTagValue(.Name, .Tag.Name.ToString)
        End With
        ShowMessage("The font name of the message is changed to " & Console.Font.Name & ".")
    End Sub

    Private Sub GhostScriptPathMenuItem_Click(sender As Object)
        Dim FolderBrowserDialog As New FolderBrowserDialog
        With FolderBrowserDialog
            .ShowNewFolderButton = False
            .Description = "Please Select the Bin folder Of the GhostScript."
            .SelectedPath = GhostScriptBinFolder
        End With

        With CType(sender, ToolStripMenuItem)
            ShowMessage(FolderBrowserDialog.Description)
            If Not FolderBrowserDialog.ShowDialog = DialogResult.OK Then
                .Checked = IsGhostScripBinFolder(GhostScriptBinFolder)
                ShowMessage("You selected nothing.")
                Exit Sub
            End If

            .Checked = IsGhostScripBinFolder(FolderBrowserDialog.SelectedPath)
            Dim NotString As String = ""
            If Not .Checked Then
                NotString = "not"
            End If
            ShowMessage("The Bin folder of the GhostScript is change to """ & FolderBrowserDialog.SelectedPath & """. And it is " & NotString & " the Bin Folder of the GhostScript.")
            GhostScriptBinFolder = FolderBrowserDialog.SelectedPath
            Configuration.SetTagValue(sender.Name, FolderBrowserDialog.SelectedPath)
        End With
    End Sub



    Private Sub ShowMessage(ByVal Message As String)
        Me.Console.SelectionStart = Me.Console.TextLength
        Me.Console.AppendText(Message)
        Me.Console.SelectionLength = Message.Length
        Me.Console.SelectionBullet = True
        Me.Console.SelectionHangingIndent = 15
        Me.Console.AppendText(vbCrLf)
        Me.Console.SelectionStart = Me.Console.TextLength
        Me.Console.SelectionLength = 0
        Me.Console.SelectionBullet = False
    End Sub

    Public Function IsGhostScripBinFolder(ByVal Path As String) As Boolean
        If Not My.Computer.FileSystem.DirectoryExists(Path) Then
            Return False
        End If

        For Each FileName In {"gswin64.exe", "gsdll64.dll"}
            If Not My.Computer.FileSystem.FileExists(Path.Trim("\") & "\" & FileName) Then
                Return False
            End If
        Next

        Return True
    End Function
End Class
