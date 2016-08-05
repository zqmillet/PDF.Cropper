﻿Namespace PDFCropper
    ''' <summary>
    ''' This class is the menu which will be shown when the right button of mouse is clicked.
    ''' </summary>
    Public Class ContextMenuStrip
        Inherits System.Windows.Forms.ContextMenuStrip

#Region "Menu Items"
        Public OpenFilesMenuItem As ToolStripMenuItem

        ''' <summary>
        ''' This is a menu item which is used to set whether the FormMain is topmost.
        ''' </summary>
        Public TopMostMenuItem As ToolStripMenuItem
        ''' <summary>
        ''' This is a menu item which is used to exit the program when it is clicked.
        ''' </summary>
        Public ExitMenuItem As ToolStripMenuItem
        ''' <summary>
        ''' This is a memu item which is used to select the opacity of the FormMain.
        ''' </summary>
        Public OpacityMenuItem As ToolStripMenuItem
        ''' <summary>
        ''' This is a menu item which is used to choose the back color of the FormMain.
        ''' </summary>
        Public BackColorMenuItem As ToolStripMenuItem
        ''' <summary>
        ''' This is a menu item which is used to choose the fore color of the FormMain.
        ''' </summary>
        Public ForeColorMenuItem As ToolStripMenuItem
        ''' <summary>
        ''' This is a menu item which is used to choose the font size of the FormMain.
        ''' </summary>
        Public FontSizeMenuItem As ToolStripMenuItem
        ''' <summary>
        ''' This is a menu item which is used to choose the font name of the FormMain.
        ''' </summary>
        Public FontNameMenuItem As ToolStripMenuItem
        ''' <summary>
        ''' This is a menu item which is used to choose the margin width of the output PDF file.
        ''' </summary>
        Public MarginWidthMenuItem As PDFCropper.ToolStripMarginWidthTextBox
        Public NewFileNamePrefixMenuItem As PDFCropper.ToolStripTextBox
        Public NewFileNameSuffixMenuItem As PDFCropper.ToolStripTextBox
        Public GhostScriptPathMenuItem As ToolStripMenuItem
        Public AutoOverwriteMenuItem As ToolStripMenuItem
        Public AddContextMenuForPDFFile As ToolStripMenuItem
#End Region
        ''' <summary>
        ''' This is a string array to store the alternative color names.
        ''' </summary>
        Public ColorList() As String = {"Black", "Blue", "Lime", "Cyan", "Red", "Fuchsia", "Yellow", "White", "Navy", "Green", "Teal", "Maroon", "Purple", "Olive", "Gray"}
        Public FontSizeList() As Integer = {5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15}

#Region "Menu Events"
        Public Event OpenFilesMenuItem_Click(sender As Object)
        ''' <summary>
        ''' This is a event which will be triggered when "TopMostMenuItem" is clicked.
        ''' </summary>
        ''' <param name="sender">It is "TopMostMenuItem" itself.</param>
        Public Event TopMostMenuItem_Click(sender As Object)
        ''' <summary>
        ''' This is a event which will be triggered when "ExitMenuItem" is clicked.
        ''' </summary>
        ''' <param name="sender">It is "ExitMenuItem" itself.</param>
        Public Event ExitMenuItem_Click(sender As Object)
        ''' <summary>
        ''' This is a event which will be triggered when the sub item of "OpacityMenuItem" is clicked.
        ''' </summary>
        ''' <param name="sender">It is the sub item of "OpacityMenuItem".</param>
        Public Event OpacityMenuItem_Click(sender As Object)
        ''' <summary>
        ''' This is a event which will be triggered when the sub item of "BackColorMenuItem" is clicked.
        ''' </summary>
        ''' <param name="sender">It is the sub item of "BackColorMenuItem".</param>
        Public Event BackColorMenuItem_Click(sender As Object)
        ''' <summary>
        ''' This is a event which will be triggered when the sub item of "ForeColorMenuItem" is clicked.
        ''' </summary>
        ''' <param name="sender">It is the sub item of "ForeColorMenuItem".</param>
        Public Event ForeColorMenuItem_Click(sender As Object)
        ''' <summary>
        ''' This is a event which will be triggered when the sub item of "FontSizeMenuItem" is clicked.
        ''' </summary>
        ''' <param name="sender">It is the sub item of "FontSizeMenuItem".</param>
        Public Event FontSizeMenuItem_Click(sender As Object)
        ''' <summary>
        ''' This is a event which will be triggered when the sub item of "FontNameMenuItem" is clicked.
        ''' </summary>
        ''' <param name="sender">It is the sub item of "FontNameMenuItem".</param>
        Public Event FontNameMenuItem_Click(sender As Object)
        ' Public Event MarginWidthMenuItem_Click(sender As Object)
        Public Event GhostScriptPathMenuItem_Click(sender As Object)
        Public Event MarginWidthMenuItem_ValueChanged(sender As Object)
        Public Event NewFileNamePrefixMenuItem_TextChanged(sender As Object)
        Public Event NewFileNameSuffixMenuItem_TextChanged(sender As Object)
        Public Event AutoOverwriteMenuItem_Click(sender As Object)
        Public Event AddContextMenuForPDFFile_Click(sender As Object)
#End Region

#Region "Menu Properties"
        ''' <summary>
        ''' This is a writeonly boolean to represent whether the FormMain is topmost.
        ''' When it is assigned a value, its checked state is changed.
        ''' Then raise the event "TopMostMenuItem_Click".
        ''' </summary>
        Public Property FormMainTopMost As Boolean
            Get
                Return TopMostMenuItem.Checked
            End Get
            Set(Value As Boolean)
                TopMostMenuItem.Checked = Value
                RaiseEvent TopMostMenuItem_Click(TopMostMenuItem)
            End Set
        End Property

        ''' <summary>
        ''' This is a writeonly double to represent the opacity of the FormMain.
        ''' When it is assigned a value, checked states of the corresponding sub items are changed.
        ''' Then raise the event "OpacityMenuItem_Click".
        ''' </summary>
        Public Property FormMainOpacity As Double
            Get
                For Each MenuItem As ToolStripMenuItem In OpacityMenuItem.DropDownItems
                    If MenuItem.Checked Then
                        Return MenuItem.Tag
                    End If
                Next
                Return 1
            End Get
            Set(Value As Double)
                For Each MenuItem As ToolStripMenuItem In OpacityMenuItem.DropDownItems
                    If MenuItem.Tag = Value Then
                        MenuItem.Checked = True
                        RaiseEvent OpacityMenuItem_Click(MenuItem)
                        Exit Property
                    End If
                Next
            End Set
        End Property

        ''' <summary>
        ''' This is a writeonly color to represent the back color of the FormMain.
        ''' When it is assigned a value, the image of this item will be assigned the image of its sub item, whose color is equal to the value.
        ''' Then raise the event "BackColorMenuItem_Click".
        ''' </summary>
        Public Property FormMainBackColor As Color
            Get
                If Not BackColorMenuItem.Tag Is Nothing Then
                    Return BackColorMenuItem.Tag
                Else
                    Return Color.Black
                End If
            End Get
            Set(Value As Color)
                For Each MenuItem As ToolStripMenuItem In BackColorMenuItem.DropDownItems
                    If MenuItem.Tag = Value Then
                        BackColorMenuItem.Image = MenuItem.Image
                        BackColorMenuItem.Tag = Value
                        RaiseEvent BackColorMenuItem_Click(MenuItem)
                        Exit Property
                    End If
                Next
            End Set
        End Property

        ''' <summary>
        ''' This is a writeonly color to represent the fore color of the FormMain.
        ''' When it is assigned a value, the image of this item will be assigned the image of its sub item, whose color is equal to the value.
        ''' Then raise the event "ForeColorMenuItem_Click".
        ''' </summary>
        Public Property FormMainForeColor As Color
            Get
                If Not ForeColorMenuItem.Tag Is Nothing Then
                    Return ForeColorMenuItem.Tag
                Else
                    Return Color.White
                End If
            End Get
            Set(Value As Color)
                For Each MenuItem As ToolStripMenuItem In ForeColorMenuItem.DropDownItems
                    If MenuItem.Tag = Value Then
                        ForeColorMenuItem.Image = MenuItem.Image
                        ForeColorMenuItem.Tag = Value
                        RaiseEvent ForeColorMenuItem_Click(MenuItem)
                    End If
                Next
            End Set
        End Property

        Public Property FormMainFontSize As Integer
            Get
                For Each MenuItem As ToolStripMenuItem In FontSizeMenuItem.DropDownItems
                    If MenuItem.Checked Then
                        Return MenuItem.Tag
                    End If
                Next
                Return Me.Font.Size
            End Get
            Set(Value As Integer)
                For Each MenuItem As ToolStripMenuItem In FontSizeMenuItem.DropDownItems
                    MenuItem.Checked = MenuItem.Tag = Value
                    If MenuItem.Tag = Value Then
                        RaiseEvent FontSizeMenuItem_Click(MenuItem)
                    End If
                Next
            End Set
        End Property

        Public Property FormMainFontName As String
            Get
                For Each MenuItem As ToolStripMenuItem In FontNameMenuItem.DropDownItems
                    If MenuItem.Checked Then
                        Return MenuItem.Tag.Name
                    End If
                Next
                Return Me.Font.SystemFontName
            End Get
            Set(Value As String)
                For Each MenuItem As ToolStripMenuItem In FontNameMenuItem.DropDownItems
                    MenuItem.Checked = MenuItem.Tag.Name = Value
                    If MenuItem.Tag.Name = Value Then
                        RaiseEvent FontNameMenuItem_Click(MenuItem)
                    End If
                Next
            End Set
        End Property

        Public WriteOnly Property GhostScriptBinFolder As Boolean
            Set(Value As Boolean)
                GhostScriptPathMenuItem.Checked = Value
            End Set
        End Property

        Public WriteOnly Property MarginWidthValue As Double
            Set(Value As Double)
                MarginWidthMenuItem.Value = Value
            End Set
        End Property

        Public WriteOnly Property MarginWidthUnitIndex As Integer
            Set(Value As Integer)
                MarginWidthMenuItem.UnitIndex = Value
            End Set
        End Property

        Public Property NewFileNamePrefix As String
            Get
                Return NewFileNamePrefixMenuItem.Text
            End Get
            Set(Value As String)
                NewFileNamePrefixMenuItem.Text = Value
            End Set
        End Property

        Public Property NewFileNameSuffix As String
            Get
                Return NewFileNameSuffixMenuItem.Text
            End Get
            Set(Value As String)
                NewFileNameSuffixMenuItem.Text = Value
            End Set
        End Property

        Public Property AutoOverwrite As Boolean
            Get
                Return AutoOverwriteMenuItem.Checked
            End Get
            Set(Value As Boolean)
                AutoOverwriteMenuItem.Checked = Value
            End Set
        End Property

        Public Property ContextMenuForPDFFile As Boolean
            Get
                Return AddContextMenuForPDFFile.Checked
            End Get
            Set(Value As Boolean)
                AddContextMenuForPDFFile.Checked = Value
            End Set
        End Property
#End Region

        ''' <summary>
        ''' This is the constructor of this class.
        ''' </summary>
        Public Sub New()
            OpenFilesMenuItem = New ToolStripMenuItem
            With OpenFilesMenuItem
                .Text = "Open Files ..."
                .ShortcutKeys = Keys.Control Or Keys.O
                .Name = NameOf(OpenFilesMenuItem)
                AddHandler .Click, AddressOf Me_Click
            End With

            ' Initialize the "TopMostMenuItem"
            TopMostMenuItem = New ToolStripMenuItem
            With TopMostMenuItem
                .Text = "Top Most"
                .ShortcutKeys = Keys.Control Or Keys.T
                .Name = NameOf(TopMostMenuItem)
                .CheckOnClick = True
                AddHandler .Click, AddressOf Me_Click
            End With

            ' Initialize the "ExitMenuItem"
            ExitMenuItem = New ToolStripMenuItem
            With ExitMenuItem
                .Text = "Exit"
                .ShortcutKeys = Keys.Alt Or Keys.X
                .Name = NameOf(ExitMenuItem)
                AddHandler .Click, AddressOf Me_Click
            End With

            ' Initialize the "OpacityMenuItem"
            OpacityMenuItem = New ToolStripMenuItem
            With OpacityMenuItem
                .Text = "Opacity"
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

            ' Initialize the "BackColorMenuItem"
            BackColorMenuItem = New ToolStripMenuItem
            With BackColorMenuItem
                .Text = "Back Color"
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

            ' Initialize the "ForeColorMenuItem"
            ForeColorMenuItem = New ToolStripMenuItem
            With ForeColorMenuItem
                .Text = "Fore Color"
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

            FontSizeMenuItem = New ToolStripMenuItem
            With FontSizeMenuItem
                .Text = "Font Size"
                .Name = NameOf(FontSizeMenuItem)
                For Each FontSize As Integer In FontSizeList
                    Dim MenuItem As New ToolStripMenuItem
                    With MenuItem
                        .Text = FontSize & "pt"
                        .CheckOnClick = True
                        .Name = NameOf(FontSizeMenuItem)
                        .Tag = FontSize
                        AddHandler .Click, AddressOf Me_Click
                    End With
                    .DropDownItems.Add(MenuItem)
                Next
            End With

            FontNameMenuItem = New ToolStripMenuItem
            With FontNameMenuItem
                .Text = "Font Name"
                .Name = NameOf(FontNameMenuItem)
                For Each FontFamily As FontFamily In System.Drawing.FontFamily.Families
                    Dim MenuItem As New ToolStripMenuItem
                    With MenuItem
                        .Text = FontFamily.Name
                        .CheckOnClick = True
                        .Name = NameOf(FontNameMenuItem)
                        .Tag = FontFamily
                        AddHandler .Click, AddressOf Me_Click
                    End With
                    .DropDownItems.Add(MenuItem)
                Next
            End With

            GhostScriptPathMenuItem = New ToolStripMenuItem
            With GhostScriptPathMenuItem
                .Text = "GhostScript Bin Folder"
                .ShortcutKeys = Keys.Control Or Keys.G
                .Name = NameOf(GhostScriptPathMenuItem)
                .CheckOnClick = True
                AddHandler .Click, AddressOf Me_Click
            End With

            MarginWidthMenuItem = New PDFCropper.ToolStripMarginWidthTextBox
            With MarginWidthMenuItem
                .Name = NameOf(MarginWidthMenuItem)
                AddHandler .ValueChanged, AddressOf Me_Click
            End With

            NewFileNamePrefixMenuItem = New PDFCropper.ToolStripTextBox
            With NewFileNamePrefixMenuItem
                .Name = NameOf(NewFileNamePrefixMenuItem)
                .LabelText = "Prefix of New File Name"
                AddHandler .TextChanged, AddressOf Me_Click
            End With

            NewFileNameSuffixMenuItem = New PDFCropper.ToolStripTextBox
            With NewFileNameSuffixMenuItem
                .Name = NameOf(NewFileNameSuffixMenuItem)
                .LabelText = "Suffix of New File Name"
                AddHandler .TextChanged, AddressOf Me_Click
            End With

            AutoOverwriteMenuItem = New ToolStripMenuItem
            With AutoOverwriteMenuItem
                .Name = NameOf(AutoOverwriteMenuItem)
                .Text = "Auto Overwrite Files"
                .CheckOnClick = True
                .ShortcutKeys = Keys.Control Or Keys.W
                AddHandler .Click, AddressOf Me_Click
            End With

            AddContextMenuForPDFFile = New ToolStripMenuItem
            With AddContextMenuForPDFFile
                .Name = NameOf(AddContextMenuForPDFFile)
                .Text = "Add Context Menu for PDF File"
                .CheckOnClick = True
                .ShortcutKeys = Keys.Control Or Keys.Q
                AddHandler .Click, AddressOf Me_Click
            End With

            ' Add all menu items into the menu.
            With Me
                .Items.Add(OpenFilesMenuItem)
                .Items.Add(New ToolStripSeparator)
                .Items.Add(TopMostMenuItem)
                .Items.Add(New ToolStripSeparator)
                .Items.Add(OpacityMenuItem)
                .Items.Add(BackColorMenuItem)
                .Items.Add(ForeColorMenuItem)
                .Items.Add(FontSizeMenuItem)
                .Items.Add(FontNameMenuItem)
                .Items.Add(New ToolStripSeparator)
                .Items.Add(MarginWidthMenuItem)
                .Items.Add(NewFileNamePrefixMenuItem)
                .Items.Add(NewFileNameSuffixMenuItem)
                .Items.Add(AutoOverwriteMenuItem)
                .Items.Add(New ToolStripSeparator)
                .Items.Add(GhostScriptPathMenuItem)
                .Items.Add(New ToolStripSeparator)
                .Items.Add(AddContextMenuForPDFFile)
                .Items.Add(New ToolStripSeparator)
                .Items.Add(ExitMenuItem)
            End With
        End Sub

        ''' <summary>
        ''' This function is used to generate a colored image by the paramter color.
        ''' </summary>
        ''' <param name="Color">This is the color of the image.</param>
        ''' <returns>This is an image whose color is equal to the parameter "Color".</returns>
        Private Function GetImageByColor(ByVal Color As Color)
            Dim Image As Image = New Bitmap(20, 20)
            Dim Graphics As Graphics = Graphics.FromImage(Image)
            Graphics.Clear(Color)
            Return Image
        End Function

        ''' <summary>
        ''' When the items of the menu is clicked, this sub will be triggered.
        ''' </summary>
        ''' <param name="sender">This is the sender of the event.</param>
        ''' <param name="e">This is the arguments of the event.</param>
        Public Sub Me_Click(sender As Object, e As EventArgs)
            If sender Is OpenFilesMenuItem Then
                RaiseEvent OpenFilesMenuItem_Click(sender)
                Exit Sub
            End If

            If sender Is TopMostMenuItem Then
                RaiseEvent TopMostMenuItem_Click(sender)
                Exit Sub
            End If

            If sender Is ExitMenuItem Then
                RaiseEvent ExitMenuItem_Click(sender)
                Exit Sub
            End If

            If sender Is GhostScriptPathMenuItem Then
                RaiseEvent GhostScriptPathMenuItem_Click(sender)
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

            If sender.Name = FontSizeMenuItem.Name Then
                For Each MenuItem As ToolStripMenuItem In FontSizeMenuItem.DropDownItems
                    MenuItem.Checked = MenuItem Is sender
                Next
                RaiseEvent FontSizeMenuItem_Click(sender)
                Exit Sub
            End If

            If sender.Name = FontNameMenuItem.Name Then
                For Each MenuItem As ToolStripMenuItem In FontNameMenuItem.DropDownItems
                    MenuItem.Checked = MenuItem Is sender
                Next
                RaiseEvent FontNameMenuItem_Click(sender)
                Exit Sub
            End If

            If sender Is MarginWidthMenuItem Then
                RaiseEvent MarginWidthMenuItem_ValueChanged(sender)
                Exit Sub
            End If

            If sender Is NewFileNamePrefixMenuItem Then
                RaiseEvent NewFileNamePrefixMenuItem_TextChanged(sender)
                Exit Sub
            End If

            If sender Is NewFileNameSuffixMenuItem Then
                RaiseEvent NewFileNameSuffixMenuItem_TextChanged(sender)
                Exit Sub
            End If

            If sender Is AutoOverwriteMenuItem Then
                RaiseEvent AutoOverwriteMenuItem_Click(sender)
                Exit Sub
            End If

            If sender Is AddContextMenuForPDFFile Then
                RaiseEvent AddContextMenuForPDFFile_Click(sender)
                Exit Sub
            End If
        End Sub
    End Class
End Namespace
