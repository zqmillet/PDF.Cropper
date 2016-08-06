Public Class FormHide

    Dim RootPath As String = "*\shell\"
    Dim MainName As String = "PDFCropper"
    Dim MainPath As String = "*\shell\" & MainName

    Private Sub New()
        InitializeComponent()
        Me.Size = New Drawing.Size(0, 0)
        Me.Opacity = 0

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

    Private Sub AddWindowsContextMenu()
        RemoveWindowsContextMenu()

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