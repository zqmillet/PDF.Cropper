Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Public Class FormMain

    Private RichTextBox As PDFCropper.RichTextBox
    Private TransparentPanel As PDFCropper.TransparentPanel
    Private MouseDownPoint As Point
    Shadows ContextMenuStrip As PDFCropper.ContextMenuStrip
    Private Configuration As PDFCropper.Configuration
    Private ConfigurationPath As String = Application.StartupPath & "\Configuration.xml"
    Private OldLocation As New Point
    Private GhostScriptBinFolder As String
    Private CropPDFThread As Threading.Thread
    Private PDFFileArrayList As ArrayList

    Private Delegate Sub DelegateShowMessage(ByVal Message As String)
    Private ShowMessageForInvocation As New DelegateShowMessage(AddressOf ShowMessage)

    Public Sub New()
        InitializeComponent()

        ContextMenuStrip = New PDFCropper.ContextMenuStrip
        With ContextMenuStrip
            AddHandler .TopMostMenuItem_Click, AddressOf TopMostMenuItem_Click
            AddHandler .ExitMenuItem_Click, AddressOf ExitMenuItem_Click
            AddHandler .OpacityMenuItem_Click, AddressOf OpacityMenuItem_Click
            AddHandler .BackColorMenuItem_Click, AddressOf BackColorMenuItem_Click
            AddHandler .ForeColorMenuItem_Click, AddressOf ForeColorMenuItem_Click
            AddHandler .FontSizeMenuItem_Click, AddressOf FontSizeMenuItem_Click
            AddHandler .FontNameMenuItem_Click, AddressOf FontNameMenuItem_Click
            AddHandler .GhostScriptPathMenuItem_Click, AddressOf GhostScriptPathMenuItem_Click
        End With

        RichTextBox = New PDFCropper.RichTextBox
        With RichTextBox
            .AppendText("This is PDF Cropper made by Qiqi." & vbCrLf & "Please drag .pdf files into this form." & vbCrLf & vbCrLf)
        End With

        TransparentPanel = New PDFCropper.TransparentPanel
        With TransparentPanel
            .AllowDrop = True
            AddHandler .MouseDown, AddressOf TransparentPanel_MouseDown
            AddHandler .MouseUp, AddressOf TransparentPanel_MouseUp
            AddHandler .MouseMove, AddressOf TransparentPanel_MouseMove
            AddHandler .MouseClick, AddressOf TransparentPanel_MouseClick
            AddHandler .DragEnter, AddressOf TransparentPanel_DragEnter
            AddHandler .DragDrop, AddressOf TransparentPanel_DragDrop
        End With

        With Me
            .FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
            .Controls.Add(TransparentPanel)
            .Controls.Add(RichTextBox)
            .Text = "PDF Cropper"
            .Icon = New Icon(Application.StartupPath & "\Icon.ico")
            PDFFileArrayList = New ArrayList
        End With

        Configuration = New PDFCropper.Configuration
        If Not Configuration.Load(ConfigurationPath) Then
            Exit Sub
        End If

        With ContextMenuStrip
            GhostScriptBinFolder = Configuration.GetTagValue(ContextMenuStrip.GhostScriptPathMenuItem.Name)

            .FormMainTopMost = Configuration.GetTagValue(.TopMostMenuItem.Name)
            .FormMainOpacity = Val(Configuration.GetTagValue(.OpacityMenuItem.Name))
            .FormMainBackColor = Color.FromName(Configuration.GetTagValue(.BackColorMenuItem.Name))
            .FormMainForeColor = Color.FromName(Configuration.GetTagValue(.ForeColorMenuItem.Name))
            .FormMainFontSize = Val(Configuration.GetTagValue(.FontSizeMenuItem.Name))
            .FormMainFontName = Configuration.GetTagValue(.FontNameMenuItem.Name)
            .GhostScriptBinFolder = IsGhostScripBinFolder(GhostScriptBinFolder)
        End With


        If IsGhostScripBinFolder(GhostScriptBinFolder) Then
            ShowMessage("The Bin folder of the GhostScript has been changed to """ & GhostScriptBinFolder & """ successfully.")
        Else
            ShowMessage("The Bin folder of the GhostScript is not correct.")
        End If
    End Sub

    Private Sub TransparentPanel_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs)
        Me.Cursor = Cursors.SizeAll
        MouseDownPoint = e.Location
        OldLocation = Me.Location
    End Sub

    Private Sub TransparentPanel_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs)
        Me.Cursor = Cursors.Default

        If Not Me.Location = OldLocation Then
            ShowMessage("The form has been moved to (" & Me.Location.X & ", " & Me.Location.Y & ").")
        End If
    End Sub

    Private Sub TransparentPanel_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs)
        If Not Me.Cursor Is Cursors.SizeAll Then
            Exit Sub
        End If

        Me.Location = New Point(Me.Location.X - MouseDownPoint.X + e.Location.X,
                                Me.Location.Y - MouseDownPoint.Y + e.Location.Y)
    End Sub

    Private Sub TransparentPanel_MouseClick(sender As Object, e As MouseEventArgs)
        If e.Button = MouseButtons.Right Then
            ContextMenuStrip.Show(TransparentPanel, e.X, e.Y)
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
            RichTextBox.BackColor = .Tag
            Configuration.SetTagValue(.Name, .Tag.Name)
        End With

        With RichTextBox.BackColor
            Dim ColorName As String = ""
            If .IsNamedColor Then
                ColorName = .Name
            End If
            ShowMessage("The back color of the form is change to " & ColorName & "(" & .R & ", " & .G & ", " & .B & ").")
        End With
    End Sub

    Private Sub ForeColorMenuItem_Click(sender As Object)
        With CType(sender, ToolStripMenuItem)
            RichTextBox.ForeColor = .Tag
            Configuration.SetTagValue(.Name, .Tag.Name)
        End With

        With RichTextBox.ForeColor
            Dim ColorName As String = ""
            If .IsNamedColor Then
                ColorName = .Name
            End If
            ShowMessage("The fore color of the form is change to " & ColorName & "(" & .R & ", " & .G & ", " & .B & ").")
        End With
    End Sub

    Private Sub FontSizeMenuItem_Click(sender As Object)
        With CType(sender, ToolStripMenuItem)
            RichTextBox.Font = New Drawing.Font(RichTextBox.Font.FontFamily, .Tag)
            Configuration.SetTagValue(.Name, .Tag)
        End With
        ShowMessage("The font size of the message is changed to " & RichTextBox.Font.Size & "pt.")
    End Sub

    Private Sub FontNameMenuItem_Click(sender As Object)
        With CType(sender, ToolStripMenuItem)
            RichTextBox.Font = New Drawing.Font(.Tag.Name.ToString, RichTextBox.Font.Size)
            Configuration.SetTagValue(.Name, .Tag.Name.ToString)
        End With
        ShowMessage("The font name of the message is changed to " & RichTextBox.Font.Name & ".")
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
                NotString = " not"
            End If
            ShowMessage("The Bin folder of the GhostScript is change to """ & FolderBrowserDialog.SelectedPath & """. And it is" & NotString & " the Bin Folder of the GhostScript.")
            GhostScriptBinFolder = FolderBrowserDialog.SelectedPath
            Configuration.SetTagValue(sender.Name, FolderBrowserDialog.SelectedPath)
        End With
    End Sub

    Private Sub ShowMessage(ByVal Message As String)
        With Me.RichTextBox
            .SelectionStart = .TextLength
            .AppendText(Message)
            .SelectionLength = Message.Length
            .SelectionBullet = True
            .SelectionHangingIndent = 15
            .AppendText(vbCrLf)
            .SelectionStart = .TextLength
            .SelectionLength = 0
            .SelectionBullet = False
            .ScrollToCaret()
        End With
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

    Private Sub TransparentPanel_DragEnter(sender As Object, e As DragEventArgs)
        Dim FileList() As String = CType(e.Data.GetData(DataFormats.FileDrop, False), String())
        If Not FileList Is Nothing Then
            e.Effect = DragDropEffects.All
        End If
    End Sub

    Private Sub TransparentPanel_DragDrop(sender As Object, e As DragEventArgs)
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

                Dim Rectangles As ArrayList = GetPDFRectangles(.StandardError.ReadToEnd)
                Dim NewPDFFile As String = PDFFile.Trim(".pdf") & "2.pdf"
                Dim PDFReader As PdfReader = New PdfReader(PDFFile)
                If Not Rectangles.Count = PDFReader.NumberOfPages Then
                    Me.Invoke(ShowMessageForInvocation, "There are something wrong in the file """ & PDFFile & """.")
                    Continue While
                End If

                For PageNumber As Integer = 1 To PDFReader.NumberOfPages
                    PDFReader.GetPageN(PageNumber).Put(PdfName.CROPBOX, Rectangles.Item(PageNumber - 1))
                Next

                Dim PDFStamper As PdfStamper = New PdfStamper(PDFReader, New FileStream(NewPDFFile, FileMode.Create, FileAccess.Write))
                PDFStamper.Close()
                PDFReader.Close()
                Me.Invoke(ShowMessageForInvocation, "The file """ & PDFFile & """ has been cropped.")
            End With
        End While
    End Sub

    Private Function GetPDFRectangles(ByVal OutputString As String, Optional ByVal MarginWidth As Integer = 0) As ArrayList
        Dim Rectangles As New ArrayList
        Dim FlagString As String = "%%HiResBoundingBox:"
        Dim StringList() As String = OutputString.Split(vbLf)

        For Each Line As String In StringList
            If Not Line.Trim.IndexOf(FlagString) = 0 Then
                Continue For
            End If

            Rectangles.Add(GetPDFRectangle(Line.Remove(0, FlagString.Length), MarginWidth))
        Next

        Return Rectangles
    End Function

    Private Function GetPDFRectangle(ByVal BoundingBox As String) As PdfRectangle
        Dim Parameters() As String = BoundingBox.Split(" ")
        If Parameters.Length = 4 Then
            Return New PdfRectangle(Parameters(0), Parameters(1), Parameters(2), Parameters(3))
        Else
            Return New PdfRectangle(0, 0, 0, 0)
        End If
    End Function

    Private Function GetPDFRectangle(ByVal BoundingBox As String, ByVal MarginWidth As Integer) As PdfRectangle
        Dim Parameters() As String = BoundingBox.Trim.Split(" ")
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
