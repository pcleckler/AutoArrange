﻿Imports System.Runtime.InteropServices
Imports System.Drawing
Imports System.Drawing.Imaging

''' <summary>
''' Provides functions to capture the entire screen, or a particular window, and save it to a file.
''' </summary>
Public Class ScreenCapture
    ''' <summary>
    ''' Creates an Image object containing a screen shot of the entire desktop
    ''' </summary>
    ''' <returns></returns>
    Public Function CaptureScreen() As Image
        Return CaptureWindow(User32.GetDesktopWindow())
    End Function
    ''' <summary>
    ''' Creates an Image object containing a screen shot of a specific window
    ''' </summary>
    ''' <param name="handle">The handle to the window. (In windows forms, this is obtained by the Handle property)</param>
    ''' <returns></returns>
    Public Function CaptureWindow(handle As IntPtr) As Image
        ' get te hDC of the target window
        Dim hdcSrc As IntPtr = User32.GetWindowDC(handle)
        ' get the size
        Dim windowRect As New User32.RECT()
        User32.GetWindowRect(handle, windowRect)
        Dim width As Integer = windowRect.right - windowRect.left
        Dim height As Integer = windowRect.bottom - windowRect.top
        ' create a device context we can copy to
        Dim hdcDest As IntPtr = GDI32.CreateCompatibleDC(hdcSrc)
        ' create a bitmap we can copy it to,
        ' using GetDeviceCaps to get the width/height
        Dim hBitmap As IntPtr = GDI32.CreateCompatibleBitmap(hdcSrc, width, height)
        ' select the bitmap object
        Dim hOld As IntPtr = GDI32.SelectObject(hdcDest, hBitmap)
        ' bitblt over
        GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, _
            0, 0, GDI32.SRCCOPY)
        ' restore selection
        GDI32.SelectObject(hdcDest, hOld)
        ' clean up 
        GDI32.DeleteDC(hdcDest)
        User32.ReleaseDC(handle, hdcSrc)
        ' get a .NET image object for it
        Dim img As Image = Image.FromHbitmap(hBitmap)
        ' free up the Bitmap object
        GDI32.DeleteObject(hBitmap)
        Return img
    End Function
    ''' <summary>
    ''' Captures a screen shot of a specific window, and saves it to a file
    ''' </summary>
    ''' <param name="handle"></param>
    ''' <param name="filename"></param>
    ''' <param name="format"></param>
    Public Sub CaptureWindowToFile(handle As IntPtr, filename As String, format As ImageFormat)
        Dim img As Image = CaptureWindow(handle)
        img.Save(filename, format)
    End Sub
    ''' <summary>
    ''' Captures a screen shot of the entire desktop, and saves it to a file
    ''' </summary>
    ''' <param name="filename"></param>
    ''' <param name="format"></param>
    Public Sub CaptureScreenToFile(filename As String, format As ImageFormat)
        Dim img As Image = CaptureScreen()
        img.Save(filename, format)
    End Sub

    ''' <summary>
    ''' Helper class containing Gdi32 API functions
    ''' </summary>
    Private Class GDI32

        Public Const SRCCOPY As Integer = &HCC0020
        ' BitBlt dwRop parameter
        <DllImport("gdi32.dll")> _
        Public Shared Function BitBlt(hObject As IntPtr, nXDest As Integer, nYDest As Integer, nWidth As Integer, nHeight As Integer, hObjectSource As IntPtr, _
                nXSrc As Integer, nYSrc As Integer, dwRop As Integer) As Boolean
        End Function
        <DllImport("gdi32.dll")> _
        Public Shared Function CreateCompatibleBitmap(hDC As IntPtr, nWidth As Integer, nHeight As Integer) As IntPtr
        End Function
        <DllImport("gdi32.dll")> _
        Public Shared Function CreateCompatibleDC(hDC As IntPtr) As IntPtr
        End Function
        <DllImport("gdi32.dll")> _
        Public Shared Function DeleteDC(hDC As IntPtr) As Boolean
        End Function
        <DllImport("gdi32.dll")> _
        Public Shared Function DeleteObject(hObject As IntPtr) As Boolean
        End Function
        <DllImport("gdi32.dll")> _
        Public Shared Function SelectObject(hDC As IntPtr, hObject As IntPtr) As IntPtr
        End Function
    End Class

    ''' <summary>
    ''' Helper class containing User32 API functions
    ''' </summary>
    Private Class User32
        <StructLayout(LayoutKind.Sequential)> _
        Public Structure RECT
            Public left As Integer
            Public top As Integer
            Public right As Integer
            Public bottom As Integer
        End Structure
        <DllImport("user32.dll")> _
        Public Shared Function GetDesktopWindow() As IntPtr
        End Function
        <DllImport("user32.dll")> _
        Public Shared Function GetWindowDC(hWnd As IntPtr) As IntPtr
        End Function
        <DllImport("user32.dll")> _
        Public Shared Function ReleaseDC(hWnd As IntPtr, hDC As IntPtr) As IntPtr
        End Function
        <DllImport("user32.dll")> _
        Public Shared Function GetWindowRect(hWnd As IntPtr, ByRef rect As RECT) As IntPtr
        End Function
    End Class
End Class
