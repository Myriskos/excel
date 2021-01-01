VERSION 5.00
Begin {C62A69F0-16DC-11CE-9E98-00AA00574A4F} frmLogin 
   Caption         =   "Login to Data Entry Application"
   ClientHeight    =   3375
   ClientLeft      =   120
   ClientTop       =   465
   ClientWidth     =   7455
   OleObjectBlob   =   "frmLogin.frx":0000
   StartUpPosition =   1  'CenterOwner
End
Attribute VB_Name = "frmLogin"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit


Private Sub cmdClear_Click()

    Me.txtUserID.Value = ""
    Me.txtPassword.Value = ""
    
    Me.txtUserID.SetFocus
    
    'Code to exit- Please remove the comments if you want to use it exit
'    Unload Me
'    ThisWorkbook.Close Savechanges:=False
'    Application.Visible = True
    

End Sub

Private Sub cmdLogin_Click()

    Dim user As String
    
    Dim password As String
    
    user = Me.txtUserID.Value
    
    password = Me.txtPassword.Value
    
    If (user = "admin" And password = "admin") Or (user = "user" And password = "user") Then
    
        Unload Me
        Application.Visible = True
        
    Else
    
        If LoginInstance < 3 Then
        
            MsgBox "Invalid login credentials. Please try again.", vbOKOnly + vbCritical, "Invalid Login Details"
            LoginInstance = LoginInstance + 1
            
        Else
        
            MsgBox "You have exceeded the maximum number of login attempts.", vbOKOnly + vbCritical, "Invalid Credentials"
            Unload Me
            ThisWorkbook.Close Savechanges:=False
            Application.Visible = True
            LoginInstance = 0
            
        End If
    
    End If
    
End Sub

Private Sub UserForm_Initialize()

    Me.txtUserID.Value = ""
    Me.txtPassword.Value = ""
    
    Me.txtUserID.SetFocus

End Sub




Private Sub UserForm_QueryClose(Cancel As Integer, CloseMode As Integer)

    If CloseMode = 0 Then Cancel = True

End Sub
