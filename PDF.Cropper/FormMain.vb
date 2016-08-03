Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Public Class FormMain

    ''' <summary>
    ''' This RichTextBox is used to show the message of the software.
    ''' </summary>
    Private RichTextBox As PDFCropper.RichTextBox
    ''' <summary>
    ''' This TransparentPanel is used to prevent the RichTextBox from being clicked by mouse.
    ''' </summary>
    Private TransparentPanel As PDFCropper.TransparentPanel
    ''' <summary>
    ''' This ContextMenuStrip is shown when the right mouse button is clicked.
    ''' </summary> 
    Shadows ContextMenuStrip As PDFCropper.ContextMenuStrip
    ''' <summary>
    ''' This Point is used to record the location where mouse button is pressed down.
    ''' </summary>
    Private MouseDownLocation As Point
    ''' <summary>
    ''' This Point is used to record the last location of the FormMain.
    ''' </summary>
    Private FormLastLocation As New Point
    ''' <summary>
    ''' This Configuration is used to read and to save the configuration of the software.
    ''' </summary>
    Private Configuration As PDFCropper.Configuration
    ''' <summary>
    ''' This constant string is used to save the configuration file path.
    ''' </summary>
    Private ConfigurationPath As String = Application.StartupPath & "\Configuration.xml"
    ''' <summary>
    ''' This variable string is used to save the bin folder path of the GhostScript.
    ''' </summary>
    Private GhostScriptBinFolder As String
    ''' <summary>
    ''' This ArrayList is the pool of PDF files which will be cropped by this software.
    ''' If the PDF file in this pool has been cropped by this software, it will be removed from this pool.
    ''' </summary>
    Private PDFFilePool As ArrayList
    ''' <summary>
    ''' This Thread is used to crop the PDF files which are in the "PDFFilePool".
    ''' </summary>
    Private CropPDFThread As Threading.Thread

    ''' <summary>
    ''' This Delegate is the object that refer to method "ShowMessage".
    ''' </summary>
    ''' <param name="Message"></param>
    Private Delegate Sub DelegateShowMessage(ByVal Message As String)
    ''' <summary>
    ''' This DelegateShowMessage is used to be invoked to show the message from another thread.
    ''' </summary>
    Private ShowMessageForInvocation As New DelegateShowMessage(AddressOf ShowMessage)

    Private HasInitialized As Boolean
    Private MarginWidth As Integer

    ''' <summary>
    ''' This is the constructor of FormMain.
    ''' </summary>
    Public Sub New()
        HasInitialized = False

        ' This call is required by the designer.
        InitializeComponent()

        ' Initialize the ContextMenuStrip.
        ContextMenuStrip = New PDFCropper.ContextMenuStrip
        With ContextMenuStrip
            AddHandler .OpenFilesMenuItem_Click, AddressOf OpenFilesMenuItem_Click
            AddHandler .TopMostMenuItem_Click, AddressOf TopMostMenuItem_Click
            AddHandler .ExitMenuItem_Click, AddressOf ExitMenuItem_Click
            AddHandler .OpacityMenuItem_Click, AddressOf OpacityMenuItem_Click
            AddHandler .BackColorMenuItem_Click, AddressOf BackColorMenuItem_Click
            AddHandler .ForeColorMenuItem_Click, AddressOf ForeColorMenuItem_Click
            AddHandler .FontSizeMenuItem_Click, AddressOf FontSizeMenuItem_Click
            AddHandler .FontNameMenuItem_Click, AddressOf FontNameMenuItem_Click
            AddHandler .GhostScriptPathMenuItem_Click, AddressOf GhostScriptPathMenuItem_Click
            AddHandler .MarginWidthMenuItem_ValueChanged, AddressOf MarginWidthMenuItem_ValueChanged
        End With

        ' Initialize the RichTextBox.
        RichTextBox = New PDFCropper.RichTextBox
        With RichTextBox
            .AppendText("This is PDF Cropper made by Qiqi." & vbCrLf & "Please drag .pdf files into this form." & vbCrLf & vbCrLf)
        End With

        ' Initialize the TransparentPanel.
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

        ' Initialize the FormMain.
        With Me
            .FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
            .Controls.Add(TransparentPanel)
            .Controls.Add(RichTextBox)
            .Text = "PDF Cropper"
            .Icon = New Icon(Application.StartupPath & "\Icon\FormMain.ico")
            PDFFilePool = New ArrayList
        End With

        ' Create and load the Configuration'
        Configuration = New PDFCropper.Configuration
        ' If there anything wrong in the Configuration, exit this sub.
        If Not Configuration.Load(ConfigurationPath) Then
            Exit Sub
        End If

        ' Configure the ContextMenuStrip according to the Configuration.
        With ContextMenuStrip
            GhostScriptBinFolder = Configuration.GetTagValue(ContextMenuStrip.GhostScriptPathMenuItem.Name)

            .FormMainTopMost = Configuration.GetTagValue(.TopMostMenuItem.Name)
            .FormMainOpacity = Val(Configuration.GetTagValue(.OpacityMenuItem.Name))
            .FormMainBackColor = Color.FromName(Configuration.GetTagValue(.BackColorMenuItem.Name))
            .FormMainForeColor = Color.FromName(Configuration.GetTagValue(.ForeColorMenuItem.Name))
            .FormMainFontSize = Val(Configuration.GetTagValue(.FontSizeMenuItem.Name))
            .FormMainFontName = Configuration.GetTagValue(.FontNameMenuItem.Name)
            .GhostScriptBinFolder = IsGhostScripBinFolder(GhostScriptBinFolder)
            .MarginWidthValue = Val(Configuration.GetTagValue(.MarginWidthMenuItem.Name & "Value"))
            .MarginWidthUnitIndex = Val(Configuration.GetTagValue(.MarginWidthMenuItem.Name & "UnitIndex"))
            MarginWidth = ContextMenuStrip.MarginWidthMenuItem.GetValue
            ShowMessage("The margin width is " & MarginWidth & "pt.")
        End With

        If IsGhostScripBinFolder(GhostScriptBinFolder) Then
            ShowMessage("The Bin folder of the GhostScript has been changed to """ & GhostScriptBinFolder & """ successfully.")
        Else
            ShowMessage("The Bin folder of the GhostScript is not correct.")
        End If

        HasInitialized = True
        Me.KeyPreview = True
        AddHandler Me.KeyDown, AddressOf FormMain_KeyDown
    End Sub

    Private Sub FormMain_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If (e.Control AndAlso (e.KeyCode = Keys.P)) Then

            MsgBox("2333")
        End If

        Dim KeyCode As New Keys
        If e.Control Then
            KeyCode = KeyCode Or Keys.Control
        End If

        If e.Alt Then
            KeyCode = KeyCode Or Keys.Alt
        End If

        If e.Shift Then
            KeyCode = KeyCode Or Keys.Shift
        End If

        KeyCode = KeyCode Or e.KeyCode

        For Each MenuItem As Object In ContextMenuStrip.Items
            If Not TypeOf MenuItem Is ToolStripMenuItem Then
                Continue For
            End If

            If Not CType(MenuItem, ToolStripMenuItem).ShortcutKeys = KeyCode Then
                Continue For
            End If

            If MenuItem Is ContextMenuStrip.TopMostMenuItem Then
                ContextMenuStrip.TopMostMenuItem.Checked = Not ContextMenuStrip.TopMostMenuItem.Checked
            End If

            ContextMenuStrip.Me_Click(MenuItem, e)
        Next

    End Sub

    ''' <summary>
    ''' This event is triggered when mouse button is pressed.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TransparentPanel_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs)
        ' Change the cursor shape.
        Me.Cursor = Cursors.SizeAll
        ' Save the location of mouse.
        Me.MouseDownLocation = e.Location
        ' Save the location of FormMain.
        Me.FormLastLocation = Me.Location
    End Sub

    ''' <summary>
    ''' This event is triggered when mouse button is popped.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TransparentPanel_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs)
        ' Recover the cursor shape.
        Me.Cursor = Cursors.Default

        ' If the location of FormMain is changed, show the message.
        If Not Me.Location = FormLastLocation Then
            ShowMessage("The form has been moved to (" & Me.Location.X & ", " & Me.Location.Y & ").")
        End If
    End Sub

    ''' <summary>
    ''' This event is triggered when mouse is moving over the TransparentPanel
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TransparentPanel_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs)
        ' If the mouse button is not pressed, exit this sub.
        If Not Me.Cursor Is Cursors.SizeAll Then
            Exit Sub
        End If

        ' Update the location of the FormMain.
        Me.Location = New Point(Me.Location.X - Me.MouseDownLocation.X + e.Location.X,
                                Me.Location.Y - Me.MouseDownLocation.Y + e.Location.Y)
    End Sub

    ''' <summary>
    ''' This event is triggered when mouse is clicked.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TransparentPanel_MouseClick(sender As Object, e As MouseEventArgs)
        ' If the right mouse button is clicked, show the ContextMenuStrip
        If e.Button = MouseButtons.Right Then
            ContextMenuStrip.Show(TransparentPanel, e.X, e.Y)
        End If
    End Sub

    ''' <summary>
    ''' This event is triggered when TopMostMenuItem of the ContextMenuStrip is clicked.
    ''' </summary>
    ''' <param name="sender"></param>
    Private Sub TopMostMenuItem_Click(sender As Object)
        ' Update the topmost attribute of the FormMain, and update the configuration.
        With CType(sender, ToolStripMenuItem)
            Me.TopMost = .Checked
            Configuration.SetTagValue(.Name, .Checked)
        End With

        ' Show the message.
        If Me.TopMost Then
            ShowMessage("The form is topmost.")
        Else
            ShowMessage("The form is not topmost.")
        End If
    End Sub

    ''' <summary>
    ''' This event is triggered when ExitMenuItem of the ContextMenuStrip is clicked.
    ''' </summary>
    ''' <param name="sender"></param>
    Private Sub ExitMenuItem_Click(sender As Object)
        ' Save the configuration to the file.
        Configuration.Save()
        ' Show the message.
        ShowMessage("The configuration has been saved.")
        ' Wait for one second.
        Threading.Thread.Sleep(1000)
        ' Exit the program.
        End
    End Sub

    ''' <summary>
    ''' This event is triggered when OpacityMenuItem of the ContextMenuStrip is clicked.
    ''' </summary>
    ''' <param name="sender"></param>
    Private Sub OpacityMenuItem_Click(sender As Object)
        ' Change the opacity of the FormMain, and update the configuration.
        With CType(sender, ToolStripMenuItem)
            Me.Opacity = .Tag
            Configuration.SetTagValue(.Name, .Tag)
        End With

        ' Show the message.
        ShowMessage("The opacity of the form is changed to " & Me.Opacity * 100 & "%.")
    End Sub

    ''' <summary>
    ''' This event is triggered when BackColorMenuItem of the ContextMenuStrip is clicked.
    ''' </summary>
    ''' <param name="sender"></param>
    Private Sub BackColorMenuItem_Click(sender As Object)
        ' Change the back color of the RichTextBox, and update the configuration.
        With CType(sender, ToolStripMenuItem)
            RichTextBox.BackColor = .Tag
            Configuration.SetTagValue(.Name, .Tag.Name)
        End With

        ' Shown the message.
        With RichTextBox.BackColor
            Dim ColorName As String = ""
            If .IsNamedColor Then
                ColorName = .Name
            End If
            ShowMessage("The back color of the form is change to " & ColorName & "(" & .R & ", " & .G & ", " & .B & ").")
        End With
    End Sub

    ''' <summary>
    ''' This event is triggered when ForeColorMenuItem of the ContextMenuStrip is clicked.
    ''' </summary>
    ''' <param name="sender"></param>
    Private Sub ForeColorMenuItem_Click(sender As Object)
        ' Change the fore color of the RichTextBox, and update the configuration.
        With CType(sender, ToolStripMenuItem)
            RichTextBox.ForeColor = .Tag
            Configuration.SetTagValue(.Name, .Tag.Name)
        End With

        ' Show the message.
        With RichTextBox.ForeColor
            Dim ColorName As String = ""
            If .IsNamedColor Then
                ColorName = .Name
            End If
            ShowMessage("The fore color of the form is change to " & ColorName & "(" & .R & ", " & .G & ", " & .B & ").")
        End With
    End Sub

    ''' <summary>
    ''' This event is triggered when FontSizeMenuItem of the ContextMenuStrip is clicked.
    ''' </summary>
    ''' <param name="sender"></param>
    Private Sub FontSizeMenuItem_Click(sender As Object)
        ' Change the font size of the RichTextBox, and update the configuration.
        With CType(sender, ToolStripMenuItem)
            RichTextBox.Font = New Drawing.Font(RichTextBox.Font.FontFamily, .Tag)
            Configuration.SetTagValue(.Name, .Tag)
        End With

        ' Show the message.
        ShowMessage("The font size of the message is changed to " & RichTextBox.Font.Size & "pt.")
    End Sub

    ''' <summary>
    ''' This event is triggered when FontNameMenuItem of the ContextMenuStrip is clicked.
    ''' </summary>
    ''' <param name="sender"></param>
    Private Sub FontNameMenuItem_Click(sender As Object)
        ' Change the font name of the RichTextBox, and update the configuration.
        With CType(sender, ToolStripMenuItem)
            RichTextBox.Font = New Drawing.Font(.Tag.Name.ToString, RichTextBox.Font.Size)
            Configuration.SetTagValue(.Name, .Tag.Name.ToString)
        End With

        ' Show the message.
        ShowMessage("The font name of the message is changed to " & RichTextBox.Font.Name & ".")
    End Sub

    ''' <summary>
    ''' This event is triggered when GhostScriptPathMenuItem of the ContextMenuStrip is clicked.
    ''' </summary>
    ''' <param name="sender"></param>
    Private Sub GhostScriptPathMenuItem_Click(sender As Object)
        ' Initialize the FolderBrowserDialog.
        Dim FolderBrowserDialog As New FolderBrowserDialog
        With FolderBrowserDialog
            .ShowNewFolderButton = False
            .Description = "Please Select the Bin folder Of the GhostScript."
            .SelectedPath = GhostScriptBinFolder
        End With

        ' Show the FolderBrowserDialog.
        With CType(sender, ToolStripMenuItem)
            ' Show the message.
            ShowMessage(FolderBrowserDialog.Description)
            ' If the dialog result is not OK, update the checked attribute of the GhostScriptPathMenuItem,
            ' show the message, and exit the sub.
            If Not FolderBrowserDialog.ShowDialog = DialogResult.OK Then
                .Checked = IsGhostScripBinFolder(GhostScriptBinFolder)
                ShowMessage("You selected nothing.")
                Exit Sub
            End If

            ' If the dialog result is OK, update the checked attribute of the GhostScriptPathMenuItem,
            ' and show the message.
            .Checked = IsGhostScripBinFolder(FolderBrowserDialog.SelectedPath)
            Dim NotString As String = ""
            If Not .Checked Then
                NotString = " not"
            End If
            ShowMessage("The Bin folder of the GhostScript is change to """ & FolderBrowserDialog.SelectedPath & """. And it is" & NotString & " the Bin Folder of the GhostScript.")

            ' Update the variable GhostScriptBinFolder.
            GhostScriptBinFolder = FolderBrowserDialog.SelectedPath
            ' Update the configuration.
            Configuration.SetTagValue(sender.Name, FolderBrowserDialog.SelectedPath)
        End With
    End Sub

    Private Sub MarginWidthMenuItem_ValueChanged(sender As Object)
        If Not HasInitialized Then
            Exit Sub
        End If

        Configuration.SetTagValue(sender.Name & "Value", CType(sender, PDFCropper.ToolStripMarginWidthTextBox).Value)
        Configuration.SetTagValue(sender.Name & "UnitIndex", CType(sender, PDFCropper.ToolStripMarginWidthTextBox).UnitIndex)
        MarginWidth = ContextMenuStrip.MarginWidthMenuItem.GetValue
        ShowMessage("The margin width has been changed to " & MarginWidth & "pt.")

    End Sub

    ''' <summary>
    ''' This sub is used to show message on the RichTextBox.
    ''' </summary>
    ''' <param name="Message"></param>
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

    ''' <summary>
    ''' This function is used to judge whether the Path is the bin folder of the GhostScript.
    ''' </summary>
    ''' <param name="Path"></param>
    ''' <returns>If the Path is the bin folder of the GhostScript, return true, else return false.</returns>
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

    ''' <summary>
    ''' This sub is triggered when something are dragged into the TransparentPanel.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TransparentPanel_DragEnter(sender As Object, e As DragEventArgs)
        ' If there exists file which is dragged into the TransparentPanel, change the cursor shape.
        Dim FileList() As String = CType(e.Data.GetData(DataFormats.FileDrop, False), String())
        If Not FileList Is Nothing Then
            e.Effect = DragDropEffects.All
        End If
    End Sub

    ''' <summary>
    ''' This sub is triggered when something are dropped into the TransparentPanel.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TransparentPanel_DragDrop(sender As Object, e As DragEventArgs)
        ' For each file which is dropped into the TransparentPanel, ...
        For Each FilePath As String In CType(e.Data.GetData(DataFormats.FileDrop, False), String())
            ' If there does not exist this file, continue loop.
            If Not My.Computer.FileSystem.FileExists(FilePath) Then
                Continue For
            End If

            ' If the file type is not PDF, continue loop.
            If Not FilePath.Remove(0, FilePath.LastIndexOf(".") + 1).ToLower.Trim = "pdf" Then
                Continue For
            End If

            ' Add this existed PDF file into the PDFFilePool.
            PDFFilePool.Add(FilePath)
        Next

        ' If there does not exist the CropPDFThread, create a new one, start it, and exit the sub.
        If CropPDFThread Is Nothing Then
            CropPDFThread = New Threading.Thread(AddressOf CropPDF)
            CropPDFThread.Start()
            Exit Sub
        End If

        ' If the CropPDFThread has stopped, create a new one, start it, and exit the sub.
        If CropPDFThread.ThreadState = Threading.ThreadState.Stopped Then
            CropPDFThread = New Threading.Thread(AddressOf CropPDF)
            CropPDFThread.Start()
            Exit Sub
        End If

        ' If the CropPDFThread is running, do nothing.
    End Sub

    Private Sub OpenFilesMenuItem_Click()
        Dim OpenFileDialog As New OpenFileDialog
        With OpenFileDialog
            .Filter = "PDF File|*.pdf"
            .FileName = ""
            .Multiselect = True
        End With

        ShowMessage("Please select the PDF files that you want to crop.")
        If Not OpenFileDialog.ShowDialog = DialogResult.OK Then
            ShowMessage("You selected nothing.")
            Exit Sub
        End If

        For Each FileName As String In OpenFileDialog.FileNames
            PDFFilePool.Add(FileName)
        Next

        ' If there does not exist the CropPDFThread, create a new one, start it, and exit the sub.
        If CropPDFThread Is Nothing Then
            CropPDFThread = New Threading.Thread(AddressOf CropPDF)
            CropPDFThread.Start()
            Exit Sub
        End If

        ' If the CropPDFThread has stopped, create a new one, start it, and exit the sub.
        If CropPDFThread.ThreadState = Threading.ThreadState.Stopped Then
            CropPDFThread = New Threading.Thread(AddressOf CropPDF)
            CropPDFThread.Start()
            Exit Sub
        End If

        ' If the CropPDFThread is running, do nothing.
    End Sub


    ''' <summary>
    ''' This sub is used to crop the PDF files which are in the "PDFFilePool".
    ''' </summary>
    Private Sub CropPDF()
        ' If the bin folder is not correct, show the message, and exit the sub.
        If Not IsGhostScripBinFolder(GhostScriptBinFolder) Then
            ShowMessage("The Bin folder of the GhostScript is not correct!")
            Exit Sub
        End If

        ' Find the executable file, if there is the executable file, exit the sub.
        Dim GhostScriptExeFilePaths() As String = System.IO.Directory.GetFiles(GhostScriptBinFolder, "gswin??c.exe")
        If GhostScriptExeFilePaths.Length = 0 Then
            Exit Sub
        End If
        Dim GhostScriptExeFilePath As String = GhostScriptExeFilePaths(0)

        ' If there is PDF file in the PDFFilePool, run this loop.
        While PDFFilePool.Count > 0
            ' Get a PDF file in the PDFFilePool.
            Dim PDFFile As String = PDFFilePool.Item(0)
            ' Remove this PDF file from the PDFFilePool.
            PDFFilePool.RemoveAt(0)

            ' Initialize a process to obtain the BoundingBox of the PDF file.
            Dim Process As New Process
            Process.StartInfo.FileName = """" & GhostScriptExeFilePath & """"
            Process.StartInfo.Arguments = " -q -dBATCH -dNOPAUSE -sDEVICE=bbox -f """ & PDFFile & """"
            Process.StartInfo.UseShellExecute = False
            Process.StartInfo.RedirectStandardError = True
            Process.StartInfo.CreateNoWindow = True
            Process.Start()

            ' Get all BoundingBox of all pages of the PDF file.
            Dim Rectangles As ArrayList = GetPDFRectangles(Process.StandardError.ReadToEnd, MarginWidth)
            ' Read the PDF file which need to be cropped.
            Dim PDFReader As PdfReader = New PdfReader(PDFFile)
            ' If the number of pages is not correct, show the message, and continue the loop.
            If Not Rectangles.Count = PDFReader.NumberOfPages Then
                Me.Invoke(ShowMessageForInvocation, "There are something wrong in the file """ & PDFFile & """.")
                Continue While
            End If

            ' For each page of the PDF file, crop it.
            For PageNumber As Integer = 1 To PDFReader.NumberOfPages
                PDFReader.GetPageN(PageNumber).Put(PdfName.CROPBOX, Rectangles.Item(PageNumber - 1))
            Next

            ' Set the name of the new PDF file.
            Dim NewPDFFile As String = Application.StartupPath & "\Temp" & PDFFile.Remove(0, PDFFile.LastIndexOf("\"))
            ' Save the new PDF file.
            Dim PDFStamper As PdfStamper = New PdfStamper(PDFReader, New FileStream(NewPDFFile, FileMode.Create, FileAccess.Write))
            PDFStamper.Close()
            PDFReader.Close()

            ' Replace the old PDF file by new PDF file.
            My.Computer.FileSystem.MoveFile(NewPDFFile, PDFFile, True)

            ' Show the message.
            Me.Invoke(ShowMessageForInvocation, "The file """ & PDFFile & """ has been cropped.")
        End While
    End Sub

    ''' <summary>
    ''' This function is used to get the rectangle list from the output string of the GhostScript.
    ''' </summary>
    ''' <param name="OutputString">The output string of the GhostScript.</param>
    ''' <param name="MarginWidth">The width of margin which you want to keep.</param>
    ''' <returns>The return is an ArrayList. Assuming that there are N pages in the PDF file, the length of the ArrayList is N.</returns>
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

    ''' <summary>
    ''' This function is used to get the rectangle from the BoundingBox string.
    ''' For example, the BoundingBox string is "0 0 100 100", this function will return a new rectangle with these for parameters.
    ''' </summary>
    ''' <param name="BoundingBox">The string of BoundingBox.</param>
    ''' <returns></returns>
    Private Function GetPDFRectangle(ByVal BoundingBox As String) As PdfRectangle
        Dim Parameters() As String = BoundingBox.Split(" ")
        If Parameters.Length = 4 Then
            Return New PdfRectangle(Parameters(0), Parameters(1), Parameters(2), Parameters(3))
        Else
            Return New PdfRectangle(0, 0, 0, 0)
        End If
    End Function

    ''' <summary>
    ''' This function is used to get the rectangle from the BoundingBox string.
    ''' </summary>
    ''' <param name="BoundingBox">The string of BoundingBox.</param>
    ''' <param name="MarginWidth">The width of margin which you want to keep.</param>
    ''' <returns></returns>
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
