Module WindowsContextMenu
    Dim MainPath As String = "*\shell\PDFCropper"
    Dim CommandPath As String = MainPath & "\command"

    Sub Main()
        For Each Argument In Environment.GetCommandLineArgs()
            Select Case Argument
                Case "AddWindowsContextMenu"
                    AddWindowsContextMenu()
                Case "RemoveWindowsContextMenu"
                    RemoveWindowsContextMenu()
            End Select
        Next
    End Sub

    Private Sub AddWindowsContextMenu()

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
End Module
