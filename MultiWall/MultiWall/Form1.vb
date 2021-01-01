Imports System.IO
Imports System.Environment
Imports Microsoft.Win32
Imports System.Runtime.InteropServices

Public Class Form1

    <StructLayout(LayoutKind.Sequential)>
    Public Structure DEVMODE1
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=32)>
        Public dmDeviceName As String
        Public dmSpecVersion As Short
        Public dmDriverVersion As Short
        Public dmSize As Short
        Public dmDriverExtra As Short
        Public dmFields As Integer
        Public dmOrientation As Short
        Public dmPaperSize As Short
        Public dmPaperLength As Short
        Public dmPaperWidth As Short
        Public dmScale As Short
        Public dmCopies As Short
        Public dmDefaultSource As Short
        Public dmPrintQuality As Short
        Public dmColor As Short
        Public dmDuplex As Short
        Public dmYResolution As Short
        Public dmTTOption As Short
        Public dmCollate As Short
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=32)>
        Public dmFormName As String
        Public dmLogPixels As Short
        Public dmBitsPerPel As Short
        Public dmPelsWidth As Integer
        Public dmPelsHeight As Integer
        Public dmDisplayFlags As Integer
        Public dmDisplayFrequency As Integer
        Public dmICMMethod As Integer
        Public dmICMIntent As Integer
        Public dmMediaType As Integer
        Public dmDitherType As Integer
        Public dmReserved1 As Integer
        Public dmReserved2 As Integer
        Public dmPanningWidth As Integer
        Public dmPanningHeight As Integer
    End Structure

    <DllImport("user32.dll")>
        Public Shared Function EnumDisplaySettings(ByVal deviceName As String, ByVal modeNum As Integer, ByRef devMode As DEVMODE1) As Integer
        End Function

        <DllImport("user32.dll")>
        Public Shared Function ChangeDisplaySettings(ByRef devMode As DEVMODE1, ByVal flags As Integer) As Integer
        End Function


    Public Const CDS_UPDATEREGISTRY As Integer = 1
        Public Const CDS_TEST As Integer = 2
        Public Const DISP_CHANGE_SUCCESSFUL As Integer = 0
        Public Const DISP_CHANGE_RESTART As Integer = 1
    Public Const DISP_CHANGE_FAILED As Integer = -1




    Private Const SPI_SETDESKWALLPAPER As Integer = &H14

    Private Const SPIF_UPDATEINIFILE As Integer = &H1

    Private Const SPIF_SENDWININICHANGE As Integer = &H2



    Private Declare Auto Function SystemParametersInfo Lib "user32.dll" (ByVal uAction As Integer,
                                                                         ByVal uParam As Integer,
                                                                         ByVal lpvParam As String,
                                                                         ByVal fuWinIni As Integer) As Integer

    Private Const ENUM_CURRENT_SETTINGS = -1

    Public Enum REG_TOPLEVEL_KEYS
        HKEY_CLASSES_ROOT = &H80000000
        HKEY_CURRENT_CONFIG = &H80000005
        HKEY_CURRENT_USER = &H80000001
        HKEY_DYN_DATA = &H80000006
        HKEY_LOCAL_MACHINE = &H80000002
        HKEY_PERFORMANCE_DATA = &H80000004
        HKEY_USERS = &H80000003
    End Enum

    Private Declare Function RegCreateKey Lib "advapi32.dll" Alias "RegCreateKeyA" (ByVal Hkey As Long, ByVal lpSubKey As String, phkResult As Long) As Long

    Private Declare Function RegCloseKey Lib "advapi32.dll" (ByVal Hkey As Long) As Long

    Private Declare Function RegSetValueEx Lib "advapi32.dll" Alias "RegSetValueExA" _
   (ByVal Hkey As Long, ByVal lpValueName As String, ByVal Reserved As Long, ByVal dwType As Long, lpData As String, ByVal cbData As Long) As Long

    Private Const REG_SZ = 1

    Dim WallpaperFile As String = ""

    Dim appData As String = GetFolderPath(SpecialFolder.ApplicationData)
    Dim mcount As Integer = Screen.AllScreens.Length
    Dim pb(mcount) As PictureBox

    Const WM_SYSCOMMAND As Int32 = &H112
    Const SC_MINIMIZE As Int32 = &HF020
    Const SC_RESTORE As Int32 = &HF120

    Protected Overrides Sub WndProc(ByRef m As Message)
        If m.Msg = WM_SYSCOMMAND Then
            If m.WParam.ToInt32 = SC_MINIMIZE Then
                'User clicked "minimize"
                Debug.WriteLine("Minimizing...")
                Me.ShowInTaskbar = False
                Me.Hide()

            ElseIf m.WParam.ToInt32 = SC_RESTORE Then
                'Restoring
                Debug.WriteLine("Restoring...")
                Me.ShowInTaskbar = True
                Me.Show()
            End If
        End If
        MyBase.WndProc(m)
    End Sub

    Private Function getScreenResolution(screenNumber As Integer) As Point

        Dim dm As DEVMODE1 = New DEVMODE1
        dm.dmDeviceName = New String(New Char(32) {})
        dm.dmFormName = New String(New Char(32) {})
        dm.dmSize = CType(Marshal.SizeOf(dm), Short)
        EnumDisplaySettings(Screen.AllScreens(screenNumber).DeviceName, ENUM_CURRENT_SETTINGS, dm)

        getScreenResolution.X = dm.dmPelsWidth
        getScreenResolution.Y = dm.dmPelsHeight
    End Function
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        Dim nextMon As Integer = 7
        Dim maxHeight As Integer = 0
        Dim maxRatioHeight As Integer = 200
        Dim ratio As Double = 0
        Dim resPoint As Point
        If My.Settings.MonitorList Is Nothing Then
            ' If it is Nothing then create a new instance
            My.Settings.MonitorList = New ArrayList()
        End If

        'find monitor with higher dimention
        'prepare MonitorList on settings
        For i = 0 To mcount - 1
            If My.Settings.MonitorList.Count < (i + 1) Then My.Settings.MonitorList.Add("")
            resPoint = getScreenResolution(i)
            If maxHeight < resPoint.Y Then
                maxHeight = resPoint.Y
            End If
        Next

        'calculate image ratio for preview generation
        ratio = (maxRatioHeight / maxHeight)

        'create monitor preview
        For i = 0 To mcount - 1
            pb(i) = New PictureBox
            pb(i).Top = 10
            resPoint = getScreenResolution(i)
            pb(i).Width = resPoint.X * ratio
            pb(i).Height = resPoint.Y * ratio
            pb(i).Left = nextMon
            pb(i).Tag = (i + 1).ToString
            pb(i).Cursor = Cursors.Hand
            pb(i).AllowDrop = True

            nextMon = pb(i).Left + pb(i).Width + 5

            LoadWallpaper(i, False)

            AddHandler pb(i).Click, AddressOf _Click
            AddHandler pb(i).DoubleClick, AddressOf _DoubleClick
            AddHandler pb(i).DragDrop, AddressOf _DragDrop
            AddHandler pb(i).DragEnter, AddressOf _DragEnter

            Me.Controls.Add(pb(i))
            pb(i).BringToFront()
        Next

        chkStartup.Checked = My.Settings.RunOnStartup
        If chkStartup.Checked Then My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
                                                                                True).SetValue(Application.ProductName, Application.ExecutablePath)
        'My.Settings.Save()

        Me.Width = nextMon + 8
        btnApply.Left = (Me.Width / 2) - (btnApply.Width / 2)

        picPreviewMonBg.Width = Me.ClientSize.Width 'Me.Width - 20
        picPreviewMonBg.Height = maxRatioHeight + 14
        picPreviewMonBg.Left = 0
        'Me.MinimumSize = New Size(Me.Width, Me.Height)
        'Me.MaximumSize = New Size(Me.Width, Me.Height)
        Dim BMP As New Bitmap(picPreviewMonBg.Width, picPreviewMonBg.Height)
        Dim GFX As Graphics = Graphics.FromImage(BMP)

        GFX.DrawRectangle(New Pen(Color.DarkGray, 4), 0, 0, picPreviewMonBg.Width, picPreviewMonBg.Height)

        picPreviewMonBg.Image = BMP

        Me.WindowState = FormWindowState.Minimized
        Me.ShowInTaskbar = False

    End Sub

    Private Sub _DragEnter(sender As System.Object, e As DragEventArgs)
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub
    Private Sub _DragDrop(ByVal sender As Object, e As DragEventArgs)
        Dim picbox As PictureBox = CType(sender, PictureBox)
        Dim pindex As String = DirectCast(picbox.Tag, String)

        Dim files() As String = CType(e.Data.GetData(DataFormats.FileDrop), String())

        If files.Length <> 0 Then

            'picbox.Image = Image.FromFile(files(0))
            If IsValidImage(files(0)) Then
                My.Settings.MonitorList.Item(CInt(pindex) - 1) = files(0)
                My.Settings.Save()

                For i = 0 To mcount - 1
                    LoadWallpaper(i, (i = CInt(pindex) - 1))
                Next
            End If

        End If
    End Sub

    Function IsValidImage(filename As String) As Boolean
        Try
            Dim img As System.Drawing.Image = System.Drawing.Image.FromFile(filename)
        Catch generatedExceptionName As OutOfMemoryException
            ' Image.FromFile throws an OutOfMemoryException  
            ' if the file does not have a valid image format or 
            ' GDI+ does not support the pixel format of the file. 
            ' 
            Return False
        End Try
        Return True
    End Function

    Private Sub _Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim pbb As PictureBox = DirectCast(sender, PictureBox)

        Dim pindex As String = DirectCast(pbb.Tag, String)

        For i = 0 To mcount - 1
            LoadWallpaper(i, (i = CInt(pindex) - 1))
        Next

    End Sub

    Private Sub LoadWallpaper(index As Integer, Optional selected As Boolean = True)
        Dim BMP As New Bitmap(pb(index).Width, pb(index).Height)
        Dim GFX As Graphics = Graphics.FromImage(BMP)
        Dim tempImg As Image
        Dim imgExist As Boolean = False
        Dim resPoint As Point

        If File.Exists(My.Settings.MonitorList.Item(index).ToString) Then

            Try
                tempImg = Image.FromFile(My.Settings.MonitorList.Item(index).ToString)
            Catch ex As Exception
                MessageBox.Show("Problem opening file.")
                Return
            End Try

            pb(index).BackgroundImageLayout = ImageLayout.Stretch
            pb(index).BackgroundImage = tempImg
            imgExist = True
        Else
            GFX.FillRectangle(Brushes.SteelBlue, 0, 0, pb(index).Width, pb(index).Height)
        End If


        If selected Then
            '# BORDER
            Dim mypen As New Pen(Color.Red, 4)
            GFX.DrawRectangle(mypen, 0, 0, pb(index).Width, pb(index).Height)
        End If

        '# MONITOR NUMBER
        GFX.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        'GFX.DrawString((index + 1).ToString, New Font("Calibri", 20, FontStyle.Bold), Brushes.White, 3, 3)

        Using gp As New Drawing2D.GraphicsPath,
            f As New Font("Calibri", 25, FontStyle.Bold),
            p As New Pen(Brushes.Black, 2)

            gp.AddString((index + 1).ToString, f.FontFamily, f.Style, f.Size + 3, New Point(8, 0),
               StringFormat.GenericTypographic)
            GFX.DrawPath(p, gp)
            GFX.FillPath(Brushes.White, gp)
        End Using

        '# MONITOR RESOLUTION
        resPoint = getScreenResolution(index)

        'GFX.DrawString("(" + Screen.AllScreens(index).Bounds.Width.ToString + "x" + Screen.AllScreens(index).Bounds.Height.ToString + ")",
        '               New Font("Calibri", 11, FontStyle.Regular), Brushes.White, 27, 6)
        Using gp As New Drawing2D.GraphicsPath,
            f As New Font("Calibri", 11, FontStyle.Regular),
            p As New Pen(Brushes.Black, 2)

            'get res from screens doeasnt work on some monitor setups 
            '"(" + Screen.AllScreens(index).Bounds.Width.ToString + "x" + Screen.AllScreens(index).Bounds.Height.ToString + ")"
            gp.AddString("(" + resPoint.X.ToString + "x" + resPoint.Y.ToString + ")",
                         f.FontFamily, f.Style, f.Size + 3,
                         New Point(30, 6),
               StringFormat.GenericTypographic)
            GFX.DrawPath(p, gp)
            GFX.FillPath(Brushes.White, gp)
        End Using

        If imgExist Then
            'Wallpaper details:
            Using gp As New Drawing2D.GraphicsPath,
            f As New Font("Calibri", 10, FontStyle.Regular),
            p As New Pen(Brushes.Black, 2)

                gp.AddString("Image details:",
                             f.FontFamily, f.Style, f.Size + 3,
                             New Point(3, pb(index).Height - 44),
               StringFormat.GenericTypographic)
                GFX.DrawPath(p, gp)
                GFX.FillPath(Brushes.White, gp)
            End Using

            '# IMAGE RESOLUTION
            'GFX.DrawString("(" + tempImg.Width.ToString + "x" + tempImg.Height.ToString + ")",
            '           New Font("Calibri", 8, FontStyle.Regular), Brushes.White, 3, pb(index).Height - 26)
            Using gp As New Drawing2D.GraphicsPath,
            f As New Font("Calibri", 10, FontStyle.Regular),
            p As New Pen(Brushes.Black, 2)

                gp.AddString("Resolution: " + tempImg.Width.ToString + "x" + tempImg.Height.ToString,
                             f.FontFamily, f.Style, f.Size + 3,
                             New Point(3, pb(index).Height - 30),
               StringFormat.GenericTypographic)
                GFX.DrawPath(p, gp)
                GFX.FillPath(Brushes.White, gp)
            End Using

            '# IMAGE FILENAME
            'GFX.DrawString("Filename: " + Path.GetFileName(My.Settings.MonitorList.Item(index).ToString),
            '               New Font("Calibri", 8, FontStyle.Regular), Brushes.White, 3, pb(index).Height - 15)
            Using gp As New Drawing2D.GraphicsPath,
            f As New Font("Calibri", 8, FontStyle.Regular),
            p As New Pen(Brushes.Black, 2)

                gp.AddString("Filename: " + Path.GetFileName(My.Settings.MonitorList.Item(index).ToString),
                             f.FontFamily, f.Style, f.Size + 3,
                             New Point(3, pb(index).Height - 16),
               StringFormat.GenericTypographic)
                GFX.DrawPath(p, gp)
                GFX.FillPath(Brushes.White, gp)
            End Using
        End If


        pb(index).Image = BMP
    End Sub

    Private Sub _DoubleClick(ByVal sender As Object, ByVal e As EventArgs)
        Dim pbb As PictureBox = DirectCast(sender, PictureBox)
        Dim pindex As String = DirectCast(pbb.Tag, String)

        'MsgBox(pindex.ToString,, "Doubleclicked")
        Dim result As DialogResult = OpenFileDialog1.ShowDialog()

        ' Test result.
        If result = Windows.Forms.DialogResult.OK Then

            ' Get the file name.
            Dim path As String = OpenFileDialog1.FileName
            Try
                My.Settings.MonitorList.Item(CInt(pindex) - 1) = path.ToString
                My.Settings.Save()
                LoadWallpaper(CInt(pindex) - 1)
            Catch ex As Exception

            End Try
        End If
    End Sub

    Public Shared Function SafeImageFromFile(path As String) As Image
        Using fs As New FileStream(path, FileMode.Open, FileAccess.Read)
            Dim img = Image.FromStream(fs)
            Return img
        End Using
    End Function

    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        If btnApply.Text <> "Apply wallpaper" Then Exit Sub

        Dim images(mcount - 1) As Image
        Dim NewImageHeight As Integer = 0
        Dim NewImageWidth As Integer = 0
        Dim widthCounter As Integer = 0
        Dim resPoint As Point

        btnApply.Text = "Working..."
        btnApply.Refresh()
        'My.Application.DoEvents()
        ' My.Settings.Save()

        If Directory.Exists(appData + "\EasyMultiWall") = False Then
            My.Computer.FileSystem.CreateDirectory(appData + "\EasyMultiWall")
        End If

        WallpaperFile = appData + "\EasyMultiWall\EasyMultiWall.bmp"

        For i = 0 To mcount - 1
            '1 load images
            'images(i) = Image.FromFile(My.Settings.MonitorList.Item(i))
            ' btnApply.Text = "load img..."
            'btnApply.Refresh()
            If File.Exists(My.Settings.MonitorList.Item(i)) Then _
                images(i) = SafeImageFromFile(My.Settings.MonitorList.Item(i))
            '2 set higher image
            'btnApply.Text = "calc..."
            'btnApply.Refresh()
            resPoint = getScreenResolution(i)
            If NewImageHeight < resPoint.Y Then
                NewImageHeight = resPoint.Y
            End If

            '3 calc total width
            NewImageWidth += Screen.AllScreens(i).Bounds.Width
        Next

        Dim NewImageBmp As New Bitmap(NewImageWidth, NewImageHeight, Imaging.PixelFormat.Format32bppArgb)
        Dim NewImageGrx As Graphics = Graphics.FromImage(NewImageBmp)

        For i = 0 To mcount - 1
            resPoint = getScreenResolution(i)
            If i = 0 Then
                NewImageGrx.DrawImage(images(i), 0, 0, resPoint.X, resPoint.Y)
            Else
                NewImageGrx.DrawImage(images(i), widthCounter, 0, resPoint.X, resPoint.Y)
            End If
            widthCounter += resPoint.X
        Next

        'Dim CombineImage As String = Guid.NewGuid().ToString() + ".bmp"
        NewImageBmp.Save(WallpaperFile, Imaging.ImageFormat.Bmp)

        WriteToRegistry(RegistryHive.CurrentUser, "Control Panel\Desktop", "TileWallpaper", "1")
        Try
            Call SystemParametersInfo(SPI_SETDESKWALLPAPER, 0&, WallpaperFile, SPIF_UPDATEINIFILE Or SPIF_SENDWININICHANGE)
        Catch Ex As Exception
            MsgBox("There was an error setting the wallpaper: " & Ex.Message)
        End Try

        'disposing objects after use
        For i = 0 To mcount - 1
            images(i).Dispose()
        Next

        NewImageBmp.Dispose()
        NewImageGrx.Dispose()

        btnApply.Text = "Done!"
        btnApply.Refresh()
        Threading.Thread.Sleep(500)
        My.Application.DoEvents()
        btnApply.Text = "Apply wallpaper"
        btnApply.Refresh()
    End Sub

    Function WriteToRegistry(ByVal ParentKeyHive As RegistryHive, ByVal SubKeyName As String, ByVal ValueName As String, ByVal Value As Object) As Boolean

        'DEMO USAGE
        'Dim bAns As Boolean
        'bAns = WriteToRegistry(RegistryHive.LocalMachine, "SOFTWARE\MyCompany\MyProgram\", "ProgramHasRunBefore", "Y")
        'Debug.WriteLine("Registry Write Successful: " & bAns)

        Dim objSubKey As RegistryKey
        Dim sException As String
        Dim objParentKey As RegistryKey
        Dim bAns As Boolean


        Try

            Select Case ParentKeyHive
                Case RegistryHive.ClassesRoot
                    objParentKey = Registry.ClassesRoot
                Case RegistryHive.CurrentConfig
                    objParentKey = Registry.CurrentConfig
                Case RegistryHive.CurrentUser
                    objParentKey = Registry.CurrentUser
                Case RegistryHive.DynData
                    objParentKey = Registry.DynData
                Case RegistryHive.LocalMachine
                    objParentKey = Registry.LocalMachine
                Case RegistryHive.PerformanceData
                    objParentKey = Registry.PerformanceData
                Case RegistryHive.Users
                    objParentKey = Registry.Users

            End Select

            'Open 
            objSubKey = objParentKey.OpenSubKey(SubKeyName, True)
            'create if doesn't exist
            If objSubKey Is Nothing Then
                objSubKey = objParentKey.CreateSubKey(SubKeyName)
            End If

            objSubKey.SetValue(ValueName, Value)
            bAns = True
        Catch ex As Exception
            bAns = False

        End Try

        Return True

    End Function


    Private Sub NotifyIcon1_MouseClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseClick
        ' If e.Button = Windows.Forms.MouseButtons.Left Then Me.WindowState = FormWindowState.Normal
        If e.Button = Windows.Forms.MouseButtons.Right Then 'Checks if the pressed button is the Right Mouse

            mnuTray.Show(Cursor.Position)
            'mnuTray.Visible = True
        End If
    End Sub

    Private Sub mnuShow_Click_1(sender As Object, e As EventArgs) Handles mnuShow.Click
        Me.WindowState = FormWindowState.Normal
        Me.ShowInTaskbar = True
        Me.Show()
    End Sub

    Private Sub mnuExit_Click(sender As Object, e As EventArgs) Handles mnuExit.Click
        Me.Close()
    End Sub

    Private Sub NotifyIcon1_DoubleClick(sender As Object, e As EventArgs) Handles NotifyIcon1.DoubleClick
        If Me.ShowInTaskbar = True Then
            Me.WindowState = FormWindowState.Minimized
            Me.ShowInTaskbar = False
            Me.Hide()
        Else
            'Me.WindowState = FormWindowState.Minimized
            Me.WindowState = FormWindowState.Normal
            Me.ShowInTaskbar = True
            Me.Show()
        End If

    End Sub

    Private Sub chkStartup_CheckedChanged(sender As Object, e As EventArgs) Handles chkStartup.CheckedChanged

        If chkStartup.Checked Then
            My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True).SetValue(Application.ProductName, Application.ExecutablePath)
        Else
            My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", True).DeleteValue(Application.ProductName)
        End If

        My.Settings.RunOnStartup = chkStartup.Checked

        My.Settings.Save()
    End Sub
End Class
