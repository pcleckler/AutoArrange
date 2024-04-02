Imports System.Text

Module App

    Private mWallpaper As Image = Nothing
    Private fs As Microsoft.VisualBasic.MyServices.FileSystemProxy = My.Computer.FileSystem

    Public Property arrangements As New Arrangements

    Public ReadOnly Property WallPaper As Image

        Get

            Const SPI_GETDESKWALLPAPER As UInteger = &H73

            Dim sb As New StringBuilder(500)

            If (Not SystemParametersInfo(SPI_GETDESKWALLPAPER, CType(sb.Capacity, UInteger), sb, SPIF.None)) Then
                Return Nothing
            End If

            If fs.FileExists(sb.ToString()) Then
                mWallpaper = Image.FromFile(sb.ToString())
            Else
                mWallpaper = New Bitmap(500, 500)
                Dim gr As Graphics = Graphics.FromImage(mWallpaper)

                gr.FillRectangle(SystemBrushes.Desktop, New Rectangle(0, 0, 500, 500))
            End If

            Return mWallpaper

        End Get
    End Property

    Public Sub DisplayException(message As String, ex As Exception)

        MessageBox.Show(message & " " & Utilities.GetExceptionMessageTree(ex), My.Application.Info.Title, MessageBoxButtons.OK, MessageBoxIcon.Stop)

    End Sub

    Public Sub Alert(message As String)

        MessageBox.Show(message, My.Application.Info.Title, MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Public Function Confirm(message As String) As Boolean

        Return (MessageBox.Show(message, My.Application.Info.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = DialogResult.Yes)

    End Function

    Public Function YesNoCancel(message As String, isWarning As Boolean) As Boolean

        Return MessageBox.Show(message, My.Application.Info.Title, MessageBoxButtons.YesNoCancel, IIf(isWarning, MessageBoxIcon.Exclamation, MessageBoxIcon.Question))

    End Function

End Module
