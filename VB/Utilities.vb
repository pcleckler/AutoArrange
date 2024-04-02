Imports System.ComponentModel
Imports System.Text

Module Utilities

    Public Enum DataTableOutputFormats
        Tab
        CSV
        HTML
    End Enum

    <System.Runtime.CompilerServices.Extension()>
    Function AddBackgroundColor(ByVal image As Image, brush As System.Drawing.Brush) As Image

        Dim imageBitmap As Bitmap = Nothing
        Dim canvas As Bitmap = Nothing
        Dim gr As Graphics = Nothing

        imageBitmap = New Bitmap(image)

        imageBitmap.MakeTransparent(Color.Transparent)

        canvas = New Bitmap(imageBitmap.Width, imageBitmap.Height)

        gr = Graphics.FromImage(canvas)

        gr.FillRectangle(brush, New Rectangle(0, 0, imageBitmap.Width, imageBitmap.Height))

        gr.DrawImage(imageBitmap, New Point(0, 0))

        Return canvas

    End Function

    <System.Runtime.CompilerServices.Extension()>
    Function AddPadding(ByVal image As Image, paddingPx As UInt16) As Image

        Dim imageBitmap As Bitmap = Nothing
        Dim canvas As Bitmap = Nothing
        Dim gr As Graphics = Nothing

        imageBitmap = New Bitmap(image)

        canvas = New Bitmap(imageBitmap.Width + (paddingPx * 2), imageBitmap.Height + (paddingPx * 2))

        gr = Graphics.FromImage(canvas)

        gr.DrawImage(imageBitmap, New Point(paddingPx, paddingPx))

        Return canvas

    End Function

    Public Function CamelCase(value As String) As String

        Dim returnString As String = ""

        If value.Length < 1 Then Return ""

        For Each token As String In value.Split(" ")
            If returnString.Length > 0 Then
                returnString += " "
            End If

            If token.Length > 1 Then
                returnString += token.Substring(0, 1).ToUpper() & token.Substring(1).ToLower()
            Else
                returnString += token.ToUpper()
            End If
        Next

        Return returnString

    End Function

    <System.Runtime.CompilerServices.Extension()>
    Public Function CopyToDataTable(rows() As DataRow, template As DataTable) As DataTable

        If rows.Count > 0 Then
            Return rows.CopyToDataTable()
        Else
            If template IsNot Nothing Then
                Return template.Clone()
            Else
                Return New DataTable()
            End If
        End If

    End Function

    Public Sub CreateDirectoryStructure(filePath As String)

        Dim fs As Microsoft.VisualBasic.MyServices.FileSystemProxy = Nothing

        Try
            If filePath Is Nothing Then Exit Sub

            If filePath.Length < 1 Then Exit Sub

            ' Initialize
            fs = My.Computer.FileSystem

            If Not fs.DirectoryExists(filePath) Then
                CreateDirectoryStructure(fs.GetParentPath(filePath))

                fs.CreateDirectory(filePath)
            End If
        Catch ex As Exception
            Throw New Exception("Failed to create directory structure for '" & filePath & "'.")
        End Try

    End Sub

    Public Function DeserialzeFromXML(xml As String, ByVal objectType As Type, encoding As System.Text.Encoding) As Object

        Dim reader As New System.Xml.Serialization.XmlSerializer(objectType)
        Dim ms As New IO.MemoryStream()
        Dim buffer() As Byte = encoding.GetBytes(xml)

        ms.Write(buffer, 0, buffer.Length)

        ms.Position = 0

        Return reader.Deserialize(ms)

    End Function

    Public Function FormatDataTable(data As DataTable, withHeaders As Boolean, headersOnly As Boolean, format As DataTableOutputFormats) As String

        Dim tableText As New StringBuilder
        Dim rowText As New StringBuilder
        Dim separator As String = ""
        Dim fieldStart As String = ""
        Dim fieldEnd As String = ""

        If data IsNot Nothing Then

            Select Case format

                Case DataTableOutputFormats.CSV
                    separator = ","
                    fieldStart = """"
                    fieldEnd = """"

                Case DataTableOutputFormats.Tab
                    separator = vbTab
                    fieldStart = ""
                    fieldEnd = ""

                Case DataTableOutputFormats.HTML
                    separator = ""
                    fieldStart = "<td>"
                    fieldEnd = "</td>"

            End Select

            If format = DataTableOutputFormats.HTML Then
                tableText.AppendLine("<table>")
            End If

            If withHeaders Then
                rowText.Clear()

                If format = DataTableOutputFormats.HTML Then
                    rowText.Append("<tr>")
                End If

                Dim columnIndex As Integer = 0

                For Each column As DataColumn In data.Columns

                    If columnIndex > 0 Then
                        rowText.Append(separator)
                    End If

                    rowText.Append(fieldStart)
                    rowText.Append(column.ColumnName)
                    rowText.Append(fieldEnd)

                    columnIndex += 1
                Next

                If format = DataTableOutputFormats.HTML Then
                    rowText.Append("</tr>")
                End If

                tableText.AppendLine(rowText.ToString())
            End If

            If Not headersOnly Then
                For Each row As DataRow In data.Rows

                    rowText.Clear()

                    If format = DataTableOutputFormats.HTML Then
                        rowText.Append("<tr>")
                    End If

                    Dim columnIndex As Integer = 0

                    For Each column As DataColumn In data.Columns

                        If columnIndex > 0 Then
                            rowText.Append(separator)
                        End If

                        rowText.Append(fieldStart)
                        rowText.Append(ValueWithDefault(row.Item(column.ColumnName), ""))
                        rowText.Append(fieldEnd)

                        columnIndex += 1
                    Next

                    If format = DataTableOutputFormats.HTML Then
                        rowText.Append("</tr>")
                    End If

                    tableText.AppendLine(rowText.ToString())
                Next
            End If

            If format = DataTableOutputFormats.HTML Then
                tableText.AppendLine("</table>")
            End If
        End If

        Return tableText.ToString()

    End Function

    Function GetDefaultValueAttributeValue(value As Object, propertyName As String, defaultValue As Object) As Object

        Dim returnValue As Object = defaultValue

        If value Is Nothing Then Return defaultValue

        If propertyName.Trim().Length < 1 Then Return defaultValue

        For Each pInfo As Reflection.PropertyInfo In value.GetType().GetProperties
            If propertyName.Trim().ToLower() = pInfo.Name.ToLower() Then

                'Dim pValue As Object = pInfo.GetValue(value, Nothing)

                For Each pAttribute As System.Attribute In pInfo.GetCustomAttributes(False)
                    If pAttribute.GetType().Name.ToLower() = "defaultvalueattribute" Then
                        Dim dvAttribute As System.ComponentModel.DefaultValueAttribute = CType(pAttribute, System.ComponentModel.DefaultValueAttribute)

                        returnValue = dvAttribute.Value
                        Exit For
                    End If
                Next

                Exit For
            End If
        Next

        Return returnValue

    End Function

    Public Function GetDelegateParameterTypes(ByVal d As Type) _
        As Type()

        If d.BaseType IsNot GetType(MulticastDelegate) Then
            Throw New ApplicationException("Not a delegate.")
        End If

        Dim invoke As Reflection.MethodInfo = d.GetMethod("Invoke")
        If invoke Is Nothing Then
            Throw New ApplicationException("Not a delegate.")
        End If

        Dim parameters As Reflection.ParameterInfo() = invoke.GetParameters()
        ' Dimension this array Length - 1, because VB adds an extra
        ' element to zero-based arrays.
        Dim typeParameters(parameters.Length - 1) As Type
        For i As Integer = 0 To parameters.Length - 1
            typeParameters(i) = parameters(i).ParameterType
        Next i

        Return typeParameters

    End Function

    Public Function GetDelegateReturnType(ByVal d As Type) As Type

        If d.BaseType IsNot GetType(MulticastDelegate) Then
            Throw New ApplicationException("Not a delegate.")
        End If

        Dim invoke As Reflection.MethodInfo = d.GetMethod("Invoke")
        If invoke Is Nothing Then
            Throw New ApplicationException("Not a delegate.")
        End If

        Return invoke.ReturnType

    End Function

    Public Function GetExceptionMessageTree(primaryEx As Exception) As String

        Dim tempEx As Exception = Nothing
        Dim returnMessage As System.Text.StringBuilder = Nothing

        Try
            returnMessage = New System.Text.StringBuilder
            tempEx = primaryEx

            Do While tempEx IsNot Nothing
                If returnMessage.Length > 0 Then
                    returnMessage.Append(" ")
                End If

                If Right(tempEx.Message, 1) <> "." Then
                    returnMessage.Append(tempEx.Message & ".")
                Else
                    returnMessage.Append(tempEx.Message)
                End If

                tempEx = tempEx.InnerException
            Loop

            Return returnMessage.ToString()
        Catch ex As Exception
            Throw New Exception("Failed to access exception message tree for exception '" & primaryEx.Message & "'. " & ex.Message)
        End Try

    End Function

    Function GetPropertyValue(value As Object, propertyName As String, defaultValue As Object) As Object

        Dim returnValue As Object = defaultValue

        If value Is Nothing Then Return defaultValue

        If propertyName.Trim().Length < 1 Then Return defaultValue

        For Each pInfo As Reflection.PropertyInfo In value.GetType().GetProperties
            If propertyName.Trim().ToLower() = pInfo.Name.ToLower() Then

                returnValue = pInfo.GetValue(value, Nothing)

                If returnValue Is Nothing Then returnValue = defaultValue

                Exit For
            End If
        Next

        Return returnValue

    End Function

    Public Function GetSelectedData(grid As DataGridView) As DataTable

        Dim data As New DataTable
        Dim rows As Dictionary(Of Integer, Dictionary(Of String, Object)) = Nothing
        Dim columns As List(Of DataGridViewColumn) = Nothing

        If grid.SelectedCells.Count > 0 Then
            rows = New Dictionary(Of Integer, Dictionary(Of String, Object))
            columns = New List(Of DataGridViewColumn)

            For Each dataCell As DataGridViewCell In (
                   From c As DataGridViewCell
                     In grid.SelectedCells
                   Order By c.RowIndex, c.ColumnIndex
            )
                Dim row As Dictionary(Of String, Object) = Nothing

                If rows.ContainsKey(dataCell.RowIndex) Then
                    row = rows.Item(dataCell.RowIndex)
                Else
                    row = New Dictionary(Of String, Object)

                    rows.Add(dataCell.RowIndex, row)
                End If

                If Not row.ContainsKey(dataCell.OwningColumn.HeaderText) Then
                    row.Add(dataCell.OwningColumn.HeaderText, grid.Item(dataCell.ColumnIndex, dataCell.RowIndex).Value)
                End If

                If Not columns.Contains(dataCell.OwningColumn) Then
                    columns.Add(dataCell.OwningColumn)
                End If
            Next

            For Each column As DataGridViewColumn In columns
                data.Columns.Add(column.HeaderText, column.ValueType)
            Next

            For Each rowIndex As Integer In rows.Keys
                Dim row As Dictionary(Of String, Object) = rows.Item(rowIndex)
                Dim dataRow As DataRow = data.NewRow()

                For Each column As DataGridViewColumn In columns

                    If row.ContainsKey(column.HeaderText) Then
                        dataRow.Item(column.HeaderText) = row.Item(column.HeaderText)
                    Else
                        dataRow.Item(column.HeaderText) = DBNull.Value
                    End If
                Next

                data.Rows.Add(dataRow)
            Next
        End If

        Return data

    End Function

    <System.Runtime.CompilerServices.Extension()>
    Public Function GetSubclassesOf(t As Type, Optional excludeSystemTypes As Boolean = False, Optional excludeAbstracts As Boolean = False) As List(Of Type)
        Dim list As New List(Of Type)()
        Dim enumerator As IEnumerator = Threading.Thread.GetDomain().GetAssemblies().GetEnumerator()
        While enumerator.MoveNext()
            Try
                Dim types As Type() = DirectCast(enumerator.Current, Reflection.Assembly).GetTypes()
                If Not excludeSystemTypes OrElse (excludeSystemTypes AndAlso Not DirectCast(enumerator.Current, Reflection.Assembly).FullName.StartsWith("System.")) Then
                    Dim enumerator2 As IEnumerator = types.GetEnumerator()
                    While enumerator2.MoveNext()
                        Dim current As Type = DirectCast(enumerator2.Current, Type)

                        If (Not current.IsAbstract) Or ((current.IsAbstract) And (Not excludeAbstracts)) Then

                            If t.IsInterface Then
                                If current.GetInterface(t.FullName) IsNot Nothing Then
                                    list.Add(current)
                                End If
                            ElseIf current.IsSubclassOf(t) Then
                                list.Add(current)
                            End If

                        End If

                    End While
                End If
            Catch
            End Try
        End While
        Return list
    End Function

    Public Function InsertVariables(template As String, ParamArray variables() As Object) As String

        Return InsertVariablesEx(template, "", variables)

    End Function

    Public Function InsertVariablesEx(template As String, macroName As String, ParamArray variables() As Object) As String

        Dim variableList As New Dictionary(Of String, String)(StringComparer.CurrentCultureIgnoreCase)
        Dim returnValue As String = template
        Dim key As String = ""

        For Each value As Object In variables
            If key.Length < 1 Then
                key = ValueWithDefault(value, "")
            Else
                If Not variableList.ContainsKey(key) Then
                    variableList.Add(key, value)
                End If

                key = ""
            End If
        Next

        For Each key In variableList.Keys
            Dim value As String = variableList.Item(key)

            returnValue = Replace(returnValue, "$" & macroName & "(" & key & ")", value)
        Next

        Return returnValue

    End Function

    Public Function Join(values As List(Of String), delimiter As String) As String

        Return Join(values.ToArray(), delimiter)

    End Function

    Public Function Join(values() As String, delimiter As String) As String

        Dim returnValue As New StringBuilder

        For Each value As String In values
            If returnValue.Length > 0 Then
                returnValue.Append(delimiter)
            End If

            returnValue.Append(value)
        Next

        Return returnValue.ToString()

    End Function

    <System.Runtime.CompilerServices.Extension()>
    Function MergeImage(ByVal normalImage As Image, ByVal overlayImage As Image, ByVal overlayCoordinates As Point) As Image

        Dim overlayBitmap As Bitmap = Nothing
        Dim updatedBitmap As Bitmap = Nothing
        Dim gr As Graphics = Nothing

        overlayBitmap = New Bitmap(overlayImage)

        overlayBitmap.MakeTransparent(Color.Transparent)

        updatedBitmap = New Bitmap(normalImage)

        gr = Graphics.FromImage(updatedBitmap)

        gr.DrawImage(overlayBitmap, overlayCoordinates)

        Return updatedBitmap

    End Function

    Public Sub PutHTMLOnClipboard(html As String, fullDocument As Boolean)

        Try

            Dim sb As New System.Text.StringBuilder()
            Dim startHTML As Integer = 0
            Dim endHTML As Integer = 0
            Dim startFragment As Integer = 0
            Dim endFragment As Integer = 0

            ' Generate Header
            sb.AppendLine("Version:0.9")

            sb.AppendLine("StartHTML:$(strHTML)".Replace("$", Chr(1)))
            sb.AppendLine("EndHTML:$(endHTML)".Replace("$", Chr(1)))

            If Not fullDocument Then
                sb.AppendLine("StartFragment:$(strFRAG)".Replace("$", Chr(1)))
                sb.AppendLine("EndFragment:$(endFRAG)".Replace("$", Chr(1)))
            End If

            ' Append HTML
            startHTML = sb.Length

            If fullDocument Then

                sb.Append(html)
            Else

                sb.AppendLine("<HTML>")
                sb.AppendLine("<head>")
                sb.AppendLine("<title>HTML clipboard</title>")
                sb.AppendLine("</head>")
                sb.AppendLine("<body>")
                sb.AppendLine("<!–StartFragment–>")

                startFragment = sb.Length

                sb.Append(html)

                endFragment = sb.Length

                sb.AppendLine("<!–EndFragment–>")
                sb.AppendLine("</body>")
                sb.AppendLine("</html>")

            End If

            endHTML = sb.Length

            ' Supply Start/End Positions
            sb.Replace("$(strHTML)".Replace("$", Chr(1)), String.Format("{0:0000000000}", startHTML))
            sb.Replace("$(endHTML)".Replace("$", Chr(1)), String.Format("{0:0000000000}", endHTML))

            If Not fullDocument Then
                sb.Replace("$(strFRAG)".Replace("$", Chr(1)), String.Format("{0:0000000000}", startFragment))
                sb.Replace("$(endFRAG)".Replace("$", Chr(1)), String.Format("{0:0000000000}", endFragment))
            End If

            ' Place on Clipboard
            Clipboard.Clear()
            Clipboard.SetData(DataFormats.Html, sb.ToString())
        Catch ex As Exception
            Throw New Exception("Failed to copy HTML to clipboard.", ex)
        End Try

    End Sub

    Public Function ReadXML(ByVal filename As String, ByVal objectType As Type) As Object

        Dim file As System.IO.StreamReader = Nothing
        Dim reader As System.Xml.Serialization.XmlSerializer = Nothing

        Try

            ' Initialize Objects
            reader = New System.Xml.Serialization.XmlSerializer(objectType)

            ' Read config from the configuration file
            file = New System.IO.StreamReader(filename)

            Return reader.Deserialize(file)
        Catch ex As Exception

            Throw ex
        Finally

            Try : file.Close() : Catch ex As Exception : End Try

        End Try

    End Function

    Public Function RegExEncode(pattern As String) As String

        Const RegExSpecialCharacters As String = "$^><.\[](){}?:.*+"

        Dim returnPattern As New System.Text.StringBuilder

        If pattern Is Nothing Then Return pattern

        For Each c As String In pattern.ToCharArray()
            If RegExSpecialCharacters.Contains(c) Then
                returnPattern.Append("\" & c)
            Else
                returnPattern.Append(c)
            End If
        Next

        Return returnPattern.ToString()

    End Function

    ''' <summary>
    ''' Case Insensitive replace
    ''' </summary>
    ''' <param name="value">String to be searched</param>
    ''' <param name="searchString">String to find</param>
    ''' <param name="replaceString">String to replace searchString</param>
    ''' <returns>Returns the value with all occurrences of searchString replaced with replaceString, without regard to case.</returns>
    ''' <remarks></remarks>
    Public Function Replace(value As String, searchString As String, replaceString As String) As String

        Return Microsoft.VisualBasic.Replace(value, searchString, replaceString, 1, -1, CompareMethod.Text)

    End Function

    Public Function SerialzeToXML(ByVal xmlData As Object, encoding As System.Text.Encoding) As String

        Dim writer As New System.Xml.Serialization.XmlSerializer(xmlData.GetType)
        Dim ms As New IO.MemoryStream()

        writer.Serialize(ms, xmlData)

        Return encoding.GetString(ms.GetBuffer())

    End Function

    Public Function SingleSpace(value As String, Optional ignoreLineBreaks As Boolean = True) As String

        If value Is Nothing Then Return ""

        Dim s As New StringBuilder
        Dim lastCharacter As Char = ""

        For i As Long = 0 To value.Length - 1
            Dim c As Char = value.Substring(i, 1)

            ' Convert the character
            Select Case c
                Case vbTab
                    c = " "

                Case vbLf, vbCr, vbNewLine
                    If Not ignoreLineBreaks Then
                        c = " "
                    End If

            End Select

            Select Case c
                Case " "
                    If lastCharacter <> " " Then
                        s.Append(c)
                    End If
                Case Else
                    s.Append(c)
            End Select

            lastCharacter = c
        Next

        Return s.ToString()

    End Function

    Public Function Split(value As String, delimiter As String, defaults() As String) As String()

        Dim list As New List(Of String)
        Dim defaultIndex As Integer = 0

        For Each entry As String In value.Split(delimiter)
            If entry.Trim().Length < 1 Then
                If defaults.Length > defaultIndex Then
                    list.Add(defaults(defaultIndex))
                Else
                    list.Add(entry)
                End If
            Else
                list.Add(entry)
            End If

            defaultIndex += 1
        Next

        For i As Integer = defaultIndex To defaults.Length - 1
            list.Add(defaults(i))
        Next

        Return list.ToArray()

    End Function

    Public Function StartBackgroundWorker(supportsCancellation As Boolean, doWorkDelegate As DoWorkEventHandler, Optional asyncState As Object = Nothing, Optional onEndDelegate As RunWorkerCompletedEventHandler = Nothing, Optional progressDelegate As ProgressChangedEventHandler = Nothing) As BackgroundWorker

        Dim worker As New BackgroundWorker()

        worker.WorkerSupportsCancellation = supportsCancellation
        worker.WorkerReportsProgress = (progressDelegate IsNot Nothing)

        AddHandler worker.DoWork, doWorkDelegate

        If onEndDelegate IsNot Nothing Then
            AddHandler worker.RunWorkerCompleted, onEndDelegate
        End If

        If progressDelegate IsNot Nothing Then
            AddHandler worker.ProgressChanged, progressDelegate
        End If

        worker.RunWorkerAsync(asyncState)

        Return worker

    End Function

    ''' <summary>
    ''' Verifies substring startIndex and length parameters to ensure they are in the expected range.
    ''' </summary>
    ''' <param name="value">String to be examined</param>
    ''' <param name="startIndex">Starting zero-based index of string segment.  If negative, specifies the last position of the string minus the startIndex value.  If the startIndex is greater than the last position of the string, the defaultValue is returned.</param>
    ''' <param name="length">Length of string segment to obtain.  If negative, specifies the total length of the value string minus the length value.  If the length is greater than the total length of the string, all available characters are returned.</param>
    ''' <param name="defaultValue"></param>
    ''' <returns>The requested string segment.</returns>
    ''' <remarks></remarks>
    Public Function Substring(value As String, startIndex As Integer, length As Integer, defaultValue As String) As String

        Dim returnValue As String = ""

        If length < 0 Then
            Return Substring(value, startIndex, value.Length - (length * -1), defaultValue)
        End If

        If startIndex < 0 Then
            Return Substring(value, value.Length - (startIndex * -1), length, defaultValue)
        End If

        If startIndex >= value.Length Then
            returnValue = defaultValue
        Else
            If (startIndex + length) <= value.Length Then
                returnValue = value.Substring(startIndex, length)
            Else
                returnValue = value.Substring(startIndex)
            End If
        End If

        Return returnValue

    End Function

    <Runtime.CompilerServices.Extension>
    Function toFilename(ByVal value As String) As String
        Dim temp As New Text.StringBuilder
        Dim invalidCharacters As String = "\/:*?""<>|"
        Dim replacementCharacters As String = "--;+$'()!"
        Dim i As Integer

        For i = 0 To value.Length - 1
            If invalidCharacters.Contains(value.Substring(i, 1)) Then

                Dim pos As Integer = invalidCharacters.IndexOf(value.Substring(i, 1))

                temp.Append(replacementCharacters.Substring(pos, 1))
            Else
                temp.Append(value.Substring(i, 1))
            End If
        Next

        Return temp.ToString()

    End Function

    ''' <summary>
    ''' Checks to see if a value is Nothing or DBNull.  If so, returns the value of defaultVal.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="defaultVal"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function ValueWithDefault(ByVal value As Object, ByVal defaultVal As String) As String

        Dim tempValue As String = ""

        Try

            If IsDBNull(value) Then
                tempValue = ""
            ElseIf (value Is Nothing) Then
                tempValue = ""
            Else
                tempValue = value.ToString().Trim()
            End If

            If tempValue.Length > 0 Then
                Return tempValue
            Else
                Return defaultVal
            End If
        Catch ex As Exception

            Return defaultVal

        End Try

    End Function

    ''' <summary>
    ''' Checks to see if a value is Nothing or DBNull.  If so, returns the value of defaultVal.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="defaultVal"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function ValueWithDefault(ByVal value As Object, ByVal defaultVal As Object) As Object

        Dim tempValue As Object

        If IsDBNull(value) Then
            tempValue = Nothing
        ElseIf (value Is Nothing) Then
            tempValue = Nothing
        Else
            tempValue = value
        End If

        If tempValue IsNot Nothing Then
            Return tempValue
        Else
            Return defaultVal
        End If

    End Function

    Public Sub WriteXML(ByVal filename As String, ByVal xmlData As Object)

        Dim writer As New System.Xml.Serialization.XmlSerializer(xmlData.GetType)
        Dim file As System.IO.StreamWriter

        ' Write data
        file = New System.IO.StreamWriter(filename)
        writer.Serialize(file, xmlData)
        file.Close()
    End Sub

End Module