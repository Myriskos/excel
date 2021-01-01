Dim objExcel
Dim wbook1
Dim wbook2
dim i

Set objExcel = CreateObject("Excel.Application")
Set wbook2 = objExcel.Workbooks.Add
' Αντιγράφει στοιχεία απ το test.xls sto νεο αρχείο που δημειουργεί ( wbook2  ) 
Set wbook1 = objExcel.Workbooks.Open("C:\EXCEL\test.xls") 

i = 1
do while ( wbook1.ActiveSheet.Cells(i,1) <> "")
  msgbox (wbook1.ActiveSheet.Cells(i, 1))       'cstr(i)
rem	wbook2.ActiveSheet.Cells(1 + cint(i/3), 1) = wbook1.ActiveSheet.Cells(i,   2)
rem 	wbook2.ActiveSheet.Cells(1 + cint(i/3), 2) = wbook1.ActiveSheet.Cells(i+1, 2)
rem 	wbook2.ActiveSheet.Cells(1 + cint(i/3), 3) = wbook1.ActiveSheet.Cells(i+2, 2)
     wbook2.ActiveSheet.Cells(i, 1) = wbook1.ActiveSheet.Cells(i, 1)
     wbook2.ActiveSheet.Cells(i, 1) = wbook1.ActiveSheet.Cells(i, 1)
     wbook2.ActiveSheet.Cells(i, 1) = wbook1.ActiveSheet.Cells(i, 1)

	i = i + 1
	
loop

objExcel.Visible = True
Set objExcel =  Nothing