''' <summary>
''' This is a console program, but the console program can not hide its own windows, so in this project, I use a transparent form to replace it.
''' </summary>
Public Class FormHide
    ''' <summary>
    ''' This is the root path of the registry.
    ''' </summary>
    Dim RootPath As String = "*\shell\"
    ''' <summary>
    ''' This is the name of new key.
    ''' </summary>
    Dim MainName As String = "PDFCropper"
    Dim MainPath As String = RootPath & MainName

    ''' <summary>
    ''' This is the constructor of this form.
    ''' </summary>
    Private Sub New()
        InitializeComponent()

        ' Hide this form.
        Me.Size = New Drawing.Size(0, 0)
        Me.Opacity = 0

        ' Do something according to the arguments.
        For Each Argument In Environment.GetCommandLineArgs()
            Select Case Argument
                Case "AddWindowsContextMenu"
                    AddWindowsContextMenu()
                Case "RemoveWindowsContextMenu"
                    RemoveWindowsContextMenu()
            End Select
        Next
        End
    End Sub

    ''' <summary>
    ''' This sub is used to add the Windows context menu item for PDF file.
    ''' </summary>
    Private Sub AddWindowsContextMenu()
        ' Clear the registry first.
        RemoveWindowsContextMenu()

        ' Create new key in the registry.
        Try
            Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(RootPath, True).CreateSubKey(MainName)
            Dim Key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(MainPath, True)
            Key.SetValue("", "Crop By PDFCropper")
            Key.SetValue("AppliesTo", ".pdf")
            Key = Key.CreateSubKey("command")
            Key.SetValue("", AppDomain.CurrentDomain.BaseDirectory.Trim("\") & "\PDFCropper.exe %1")
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    ''' <summary>
    ''' This sub is used to remove the Windows context menu item for PDF file.
    ''' </summary>
    Private Sub RemoveWindowsContextMenu()
        If Not Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(MainPath) Is Nothing Then
            Try
                Microsoft.Win32.Registry.ClassesRoot.DeleteSubKeyTree(MainPath)
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
        End If
    End Sub
End Class