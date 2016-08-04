Namespace PDFCropper
    Public Class ComboBox
        Inherits Label

        Public Items As ArrayList
        Public Event SelectedIndexChanged()

        Public Property SelectedIndex As Integer
            Get
                Return Items.IndexOf(Me.Text)
            End Get
            Set(Value As Integer)
                Me.Text = Items(Value)
                RaiseEvent SelectedIndexChanged()
            End Set

        End Property

        Public Sub New()
            With Me
                .BorderStyle = BorderStyle.FixedSingle
                .Items = New ArrayList
                AddHandler .Click, AddressOf Me_Click
            End With
        End Sub

        Private Sub Me_Click()
            Dim NextIndex As Integer = Items.IndexOf(Me.Text)
            If NextIndex = Items.Count - 1 Then
                NextIndex = 0
            Else
                NextIndex += 1
            End If
            Me.Text = Items(NextIndex)
            Me.SelectedIndex = NextIndex
        End Sub
    End Class
End Namespace

