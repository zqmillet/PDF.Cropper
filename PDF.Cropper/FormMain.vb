Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Public Class FormMain

    Dim Console As Console
    Dim CoverPanel As CoverPanel
    Dim MouseDownPoint As Point
    Dim RightClickMenu As RightClickMenu
    Dim Configuration As Configuration
    Dim ConfigurationPath As String = Application.StartupPath & "\Configuration.xml"
    Dim OldLocation As New Point
    Dim GhostScriptBinFolder As String
    Dim CropPDFThread As Threading.Thread
    Dim PDFFileArrayList As ArrayList

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
            .AllowDrop = True
            AddHandler .MouseDown, AddressOf CoverPanel_MouseDown
            AddHandler .MouseUp, AddressOf CoverPanel_MouseUp
            AddHandler .MouseMove, AddressOf CoverPanel_MouseMove
            AddHandler .MouseClick, AddressOf CoverPanel_MouseClick
            AddHandler .DragEnter, AddressOf CoverPanel_DragEnter
            AddHandler .DragDrop, AddressOf CoverPanel_DragDrop
        End With

        With Me
            .FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
            .Controls.Add(CoverPanel)
            .Controls.Add(Console)
            .CheckForIllegalCrossThreadCalls = False
            .Text = "PDF Cropper"
            .Icon = New Icon(Application.StartupPath & "\Icon.ico")
            PDFFileArrayList = New ArrayList
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
        If IsGhostScripBinFolder(GhostScriptBinFolder) Then
            ShowMessage("The Bin folder of the GhostScript has been changed to """ & GhostScriptBinFolder & """ successfully.")
        End If
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
            Console.Font = New Drawing.Font(Console.Font.FontFamily, .Tag)
            Configuration.SetTagValue(.Name, .Tag)
        End With
        ShowMessage("The font size of the message is changed to " & Console.Font.Size & "pt.")
    End Sub

    Private Sub FontNameMenuItem_Click(sender As Object)
        With CType(sender, ToolStripMenuItem)
            Console.Font = New Drawing.Font(.Tag.Name.ToString, Console.Font.Size)
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
        Me.Console.ScrollToCaret()
    End Sub

    Private Function IsGhostScripBinFolder(ByVal Path As String) As Boolean
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

    Private Sub CoverPanel_DragEnter(sender As Object, e As DragEventArgs)
        Dim FileList() As String = CType(e.Data.GetData(DataFormats.FileDrop, False), String())
        If Not FileList Is Nothing Then
            e.Effect = DragDropEffects.All
        End If
    End Sub

    Private Sub CoverPanel_DragDrop(sender As Object, e As DragEventArgs)
        For Each FilePath As String In CType(e.Data.GetData(DataFormats.FileDrop, False), String())
            If Not My.Computer.FileSystem.FileExists(FilePath) Then
                Continue For
            End If

            If Not FilePath.Remove(0, FilePath.LastIndexOf(".") + 1).ToLower.Trim = "pdf" Then
                Continue For
            End If


            PDFFileArrayList.Add(FilePath)
        Next

        If CropPDFThread Is Nothing Then
            CropPDFThread = New Threading.Thread(AddressOf CropPDF)
            CropPDFThread.Start()
            Exit Sub
        End If

        If CropPDFThread.ThreadState = Threading.ThreadState.Stopped Then
            CropPDFThread = New Threading.Thread(AddressOf CropPDF)
            CropPDFThread.Start()
            Exit Sub
        End If
    End Sub

    Private Sub CropPDF()
        If Not IsGhostScripBinFolder(GhostScriptBinFolder) Then
            ShowMessage("The Bin folder of the GhostScript is not correct!")
            Exit Sub
        End If

        Dim GhostScriptExeFilePaths() As String = System.IO.Directory.GetFiles(GhostScriptBinFolder, "gswin??c.exe")
        If GhostScriptExeFilePaths.Length = 0 Then
            Exit Sub
        End If
        Dim GhostScriptExeFilePath As String = GhostScriptExeFilePaths(0)

        While PDFFileArrayList.Count > 0
            Dim PDFFile As String = PDFFileArrayList.Item(0)
            PDFFileArrayList.RemoveAt(0)

            Dim Process As New Process
            With Process
                .StartInfo.FileName = """" & GhostScriptExeFilePath & """"
                .StartInfo.Arguments = " -q -dBATCH -dNOPAUSE -sDEVICE=bbox -f """ & PDFFile & """"
                .StartInfo.UseShellExecute = False
                .StartInfo.RedirectStandardError = True
                .StartInfo.CreateNoWindow = True
                .Start()

                Dim Output As String = .StandardError.ReadToEnd
                Dim FlagString As String = "%%HiResBoundingBox:"
                Output = Output.Remove(0, Output.LastIndexOf(FlagString) + FlagString.Length).Trim
                Dim PDFRectangle As PdfRectangle = GetPDFRectangle(Output, 10)

                Dim NewPDFFile As String = PDFFile & "2"
                Dim PDFReader As PdfReader = New PdfReader(PDFFile)
                Dim PDFPage As PdfDictionary = PDFReader.GetPageN(1)
                PDFPage.Put(PdfName.CROPBOX, PDFRectangle)
                Dim PDFStamper As PdfStamper = New PdfStamper(PDFReader, New FileStream(NewPDFFile, FileMode.Create, FileAccess.Write))
                PDFStamper.Close()
                PDFReader.Close()
                ShowMessage("The file """ & PDFFile & """ has been cropped.")
            End With
        End While
    End Sub

    Private Function GetPDFRectangle(ByVal BoundingBox As String) As PdfRectangle
        Dim Parameters() As String = BoundingBox.Split(" ")
        If Parameters.Length = 4 Then
            Return New PdfRectangle(Parameters(0), Parameters(1), Parameters(2), Parameters(3))
        Else
            Return New PdfRectangle(0, 0, 0, 0)
        End If
    End Function

    Private Function GetPDFRectangle(ByVal BoundingBox As String, ByVal MarginWidth As Integer) As PdfRectangle
        Dim Parameters() As String = BoundingBox.Split(" ")
        If Parameters.Length = 4 Then
            Return New PdfRectangle(Parameters(0) - MarginWidth,
                                    Parameters(1) - MarginWidth,
                                    Parameters(2) + MarginWidth,
                                    Parameters(3) + MarginWidth)
        Else
            Return New PdfRectangle(0, 0, 0, 0)
        End If
    End Function

End Class
