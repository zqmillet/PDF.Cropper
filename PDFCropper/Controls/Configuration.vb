Namespace PDFCropper

    Public Class Configuration
        Dim DataSet As DataSet
        Dim DataTable As DataTable
        Dim Name As String = "Configuration"
        Dim Path As String = ""

        Public Sub New()
            DataSet = New DataSet
            DataTable = New DataTable
        End Sub

        Public Function Load(ByVal Path As String) As Boolean
            Me.Path = Path

            If Not My.Computer.FileSystem.FileExists(Path) Then
                Return False
            End If

            DataSet.ReadXml(Path)

            For Each DataTable As DataTable In DataSet.Tables
                If DataTable.TableName.Trim.ToLower = Me.Name.Trim.ToLower Then
                    Me.DataTable = DataTable
                    Return True
                End If
            Next

            Return False
        End Function

        Public Function GetTagValue(ByVal TagName As String) As String
            For Each Row As DataRow In Me.DataTable.Rows
                If Row(0).ToString.Trim.ToLower = TagName.Trim.ToLower Then
                    Return Row(1)
                End If
            Next

            Return ""
        End Function

        Public Sub SetTagValue(ByVal TagName As String, ByVal TagValue As String)
            If Not Me.DataTable.Columns.Count = 2 Then
                Me.DataTable.Columns.Add("TagName")
                Me.DataTable.Columns.Add("TagValue")
                Me.DataTable.TableName = Me.Name
            End If

            For Each Row As DataRow In Me.DataTable.Rows
                If Row(0).ToString.Trim.ToLower = TagName.Trim.ToLower Then
                    Row(1) = TagValue
                    Exit Sub
                End If
            Next

            Me.DataTable.Rows.Add({TagName, TagValue})
        End Sub

        Public Sub Save()
            If Not DataSet.Tables.Contains(Me.DataTable.TableName) Then
                DataSet.Tables.Add(Me.DataTable)
            End If
            DataSet.WriteXml(Path, XmlWriteMode.WriteSchema)
        End Sub
    End Class
End Namespace

