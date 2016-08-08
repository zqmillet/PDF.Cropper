Namespace PDFCropper
    ''' <summary>
    ''' This class is used to read and save the configuration of this program.
    ''' </summary>
    Public Class Configuration
        ''' <summary>
        ''' The configuration is saved in a DataTable of a DataSet.
        ''' </summary>
        Dim DataSet As DataSet
        ''' <summary>
        ''' This is the DataTable.
        ''' </summary>
        Dim DataTable As DataTable
        ''' <summary>
        ''' This name is the DataTable's name.
        ''' </summary>
        Dim Name As String = "Configuration"
        ''' <summary>
        ''' This variable is used to save the configuration file path.
        ''' </summary>
        Dim Path As String = ""

        ''' <summary>
        ''' This is the constructor of this class.
        ''' </summary>
        Public Sub New()
            DataSet = New DataSet
            DataTable = New DataTable
        End Sub

        ''' <summary>
        ''' This function is used to load the configuration from a file.
        ''' If the loading process is successful, return true, else, return false.
        ''' </summary>
        ''' <param name="Path"></param>
        ''' <returns></returns>
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

        ''' <summary>
        ''' This function is used to get the value of a tag name.
        ''' If there is no the tag name, return an empty string.
        ''' </summary>
        ''' <param name="TagName"></param>
        ''' <returns></returns>
        Public Function GetTagValue(ByVal TagName As String) As String
            For Each Row As DataRow In Me.DataTable.Rows
                If Row(0).ToString.Trim.ToLower = TagName.Trim.ToLower Then
                    Return Row(1)
                End If
            Next

            Return ""
        End Function

        ''' <summary>
        ''' This sub is used to set the value of a tag name.
        ''' </summary>
        ''' <param name="TagName"></param>
        ''' <param name="TagValue"></param>
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

        ''' <summary>
        ''' This sub is used to save the configuration into a file.
        ''' </summary>
        Public Sub Save()
            If Not DataSet.Tables.Contains(Me.DataTable.TableName) Then
                DataSet.Tables.Add(Me.DataTable)
            End If
            DataSet.WriteXml(Path, XmlWriteMode.WriteSchema)
        End Sub
    End Class
End Namespace

