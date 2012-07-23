Imports System.IO
Imports System.Diagnostics
Public Class TWM_Form
    Public watchfolder As FileSystemWatcher

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        FolderBrowserDialog1.ShowDialog()
        txt_input.Text = FolderBrowserDialog1.SelectedPath

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_stop.Click
        ' Stop watching the folder
        watchfolder.EnableRaisingEvents = False
        btn_startwatch.Enabled = True
        btn_stop.Enabled = False

    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_startwatch.Click
        If txt_input.Text = "" Then
            MessageBox.Show("You need to pick a folder.", "Hey Dummy!")
        Else
            If Not Directory.Exists(txt_input.Text) Then
                MessageBox.Show("That isn't a folder.", "Hey Dummy!")
            Else
                If txt_output.Text = "" Then txt_output.Text = txt_input.Text

                If Not Directory.Exists(txt_output.Text) Then
                    Directory.CreateDirectory(txt_output.Text)
                End If

                watchfolder = New System.IO.FileSystemWatcher()

                watchfolder.Path = txt_input.Text

                watchfolder.NotifyFilter = IO.NotifyFilters.DirectoryName
                watchfolder.NotifyFilter = watchfolder.NotifyFilter Or _
                                           IO.NotifyFilters.FileName
                watchfolder.NotifyFilter = watchfolder.NotifyFilter Or _
                                           IO.NotifyFilters.Attributes

                AddHandler watchfolder.Created, AddressOf logchange
                watchfolder.EnableRaisingEvents = True
                btn_startwatch.Enabled = False
                btn_stop.Enabled = True
                'initial fire
                Me.WindowState = FormWindowState.Minimized
                'Wait for the minimize animation
                Threading.Thread.Sleep(200)
                Me.Visible = False

                TrayIcon.ShowBalloonTip(10000)

                Watermark()




            End If
        End If
    End Sub


    Private Sub APWatermark_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txt_input.Text = My.Settings.InputPath
        txt_output.Text = My.Settings.OutputPath
        If txt_input.Text = "" Then
            Me.WindowState = FormWindowState.Normal
        Else
            CheckBox1.Checked = True
            btn_startwatch.PerformClick()

        End If

    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub logchange(ByVal source As Object, ByVal e As  _
                        System.IO.FileSystemEventArgs)
        If e.ChangeType = IO.WatcherChangeTypes.Created Then
            Watermark()
        End If

    End Sub

    Private Sub FolderBrowserDialog1_HelpRequest(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FolderBrowserDialog1.HelpRequest

    End Sub

    Sub Watermark()
        Dim DirInfo As New DirectoryInfo(txt_input.Text & "\")
        Dim SubDir() As DirectoryInfo = DirInfo.GetDirectories("*")
        For Each Directory As DirectoryInfo In SubDir
            MsgBox(Directory.FullName)

            Dim filesPNG() As FileInfo = DirInfo.GetFiles("*.png")
            For Each File As FileInfo In filesPNG
                If Not File.Name.Contains("wm_") Then

                    Dim wm_File As New FileInfo(File.DirectoryName & "\wm_" & File.Name)
                    If wm_File.Exists() = False Then
                        Dim p As New ProcessStartInfo
                        p.FileName = "composite.exe"
                        p.Arguments = "-quality 100 -resize ""1680>"" -dissolve 30 -gravity southeast -quality 85 AP_Vid_Watermark.png """ & File.FullName.ToString & """ """ & txt_output.Text & "\wm_" & File.Name.ToString & """"
                        p.WindowStyle = ProcessWindowStyle.Hidden
                        Dim CompositeProcess As Process = Process.Start(p)
                        CompositeProcess.WaitForExit()
                    End If
                End If
            Next

            Dim filesJPG() As FileInfo = DirInfo.GetFiles("*.jpg")
            For Each File As FileInfo In filesJPG
                If Not File.Name.Contains("wm_") Then
                    Dim wm_File As New FileInfo(File.DirectoryName & "\wm_" & File.Name)
                    If wm_File.Exists() = False Then
                        Dim p As New ProcessStartInfo
                        p.FileName = "composite.exe"
                        p.Arguments = "-quality 100 -resize ""1680>"" -dissolve 30 -gravity southeast -quality 85 AP_Vid_Watermark.png """ & File.FullName.ToString & """ """ & txt_output.Text & "\wm_" & File.Name.ToString & """"
                        p.WindowStyle = ProcessWindowStyle.Hidden
                        Dim CompositeProcess As Process = Process.Start(p)
                        CompositeProcess.WaitForExit()
                    End If
                End If
            Next
        Next
    End Sub

    Private Sub txt_output_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_output.TextChanged

    End Sub

    Private Sub Label4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label4.Click

    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        FolderBrowserDialog1.ShowDialog()
        txt_output.Text = FolderBrowserDialog1.SelectedPath
    End Sub

    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub NotifyIcon1_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TrayIcon.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Left Then
            Me.Visible = True
            Me.WindowState = FormWindowState.Normal
        End If
    End Sub


    Private Sub ContextMenuStrip1_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening

    End Sub

    Private Sub ShowToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowToolStripMenuItem.Click

        If Me.Visible = True Then
            Me.Visible = False
            Me.WindowState = FormWindowState.Minimized
        Else
            Me.Visible = True
            Me.WindowState = FormWindowState.Normal
        End If


    End Sub

    Private Sub ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Application.Exit()
    End Sub

    Private Sub CheckBox1_CheckedChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            My.Settings.InputPath = txt_input.Text
            My.Settings.OutputPath = txt_output.Text
        Else
            My.Settings.InputPath = ""
            My.Settings.OutputPath = ""
        End If
        My.Settings.Save()
    End Sub

    Private Sub GroupBox1_Enter(sender As System.Object, e As System.EventArgs)

    End Sub

    Private Sub Button2_Click_2(sender As System.Object, e As System.EventArgs)

    End Sub

    Private Sub Button30_Click(sender As System.Object, e As System.EventArgs)

    End Sub

    Private Sub folderPath_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles folderPath.Click

    End Sub
End Class