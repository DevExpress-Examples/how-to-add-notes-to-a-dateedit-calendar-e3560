﻿Imports DevExpress.Utils
Imports DevExpress.Utils.Win
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors.Popup
Imports DevExpress.XtraEditors.Repository
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms

Namespace DXApplication1
    Partial Public Class Form1
        Inherits DevExpress.XtraEditors.XtraForm

       Public specialDays As BindingList(Of Date)
        Public Sub New()
            InitializeComponent()
            specialDays = New BindingList(Of Date)()
            specialDays.Add(New Date(Date.Now.Year, Date.Now.Month, 1))
            specialDays.Add(New Date(Date.Now.Year, Date.Now.Month, 2))
            specialDays.Add(New Date(Date.Now.Year, Date.Now.Month, 4))
            specialDays.Add(New Date(Date.Now.Year, Date.Now.Month, 16))
            specialDays.Add(New Date(Date.Now.Year, Date.Now.Month, 13))
            specialDays.Add(New Date(Date.Now.Year, Date.Now.Month, 10))
            specialDays.Add(New Date(Date.Now.Year, Date.Now.Month, 6))
            specialDays.Add(New Date(Date.Now.Year, Date.Now.Month, 21))
            specialDays.Add(New Date(Date.Now.Year, Date.Now.Month, 22))
            specialDays.Add(New Date(Date.Now.Year, Date.Now.Month, 17))
            dateEdit1.Properties.CellStyleProvider = New MyCellStyleProvider()

            gridControl1.DataSource = GetData()
            TryCast(gridView1.Columns(2).ColumnEdit, RepositoryItemDateEdit).CellStyleProvider = New MyCellStyleProvider()
            AddHandler TryCast(gridView1.Columns(2).ColumnEdit, RepositoryItemDateEdit).Popup, AddressOf dateEdit1_Popup
            TryCast(gridView1.Columns(2).ColumnEdit, RepositoryItemDateEdit).CellSize = New Size(50, 50)
            'ContextButton contextButton = new ContextButton();
            'contextButton.Alignment = DevExpress.Utils.ContextItemAlignment.TopFar;
            'contextButton.Id = new System.Guid("add60edd-5f9a-4d98-b09d-f716aaa46999");
            'contextButton.Name = "ContextButton";
            'contextButton.Visibility = DevExpress.Utils.ContextItemVisibility.Visible;

        End Sub
        Private Function GetData() As BindingList(Of Custom)
            Dim list As New BindingList(Of Custom)()
            For i As Integer = 0 To 9
                list.Add(New Custom() With {.ID = i, .Name = "Name" & i, .Time = New Date(Date.Now.Year, Date.Now.Month, 17)})
            Next i
                Return list
        End Function

        Private Sub dateEdit1_EditValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles dateEdit1.EditValueChanged
            For Each specialDate As Date In specialDays
                If specialDate = CDate(dateEdit1.EditValue) Then
                    dateEdit1.Properties.Buttons(1).Visible = True
                Return
                End If
            Next specialDate
            dateEdit1.Properties.Buttons(1).Visible = False

        End Sub

        Private Sub dateEdit1_Popup(ByVal sender As Object, ByVal e As EventArgs) Handles dateEdit1.Popup
            Dim popupForm As PopupDateEditForm = (TryCast((TryCast(sender, IPopupControl)).PopupWindow, PopupDateEditForm))
            If popupForm.Calendar.ContextButtons.Count > 0 Then
                popupForm.Calendar.ContextButtons.Clear()
            End If
            Dim contextButton1 As New ContextButton()
            contextButton1.Alignment = ContextItemAlignment.Center
            contextButton1.Name = "ContextButton"
            contextButton1.Visibility = ContextItemVisibility.Visible
            popupForm.Calendar.ContextButtons.Add(contextButton1)
            AddHandler popupForm.Calendar.ContextButtonClick, AddressOf Calendar_ContextButtonClick
            AddHandler popupForm.Calendar.ContextButtonCustomize, AddressOf Calendar_ContextButtonCustomize
        End Sub

        Private Sub Calendar_ContextButtonCustomize(ByVal sender As Object, ByVal e As CalendarContextButtonCustomizeEventArgs)
            Dim popupCalendarControl As PopupCalendarControl = DirectCast(sender, PopupCalendarControl)

           Dim provider As MyCellStyleProvider = CType(popupCalendarControl.CellStyleProvider, MyCellStyleProvider)
           Dim data As MyCustomCellData = CType(popupCalendarControl.CellStyleProvider, MyCellStyleProvider).GetCell(e.Cell.Date)
            If data Is Nothing OrElse String.IsNullOrEmpty(data.InfoText) Then
                e.Item.Visibility = ContextItemVisibility.Hidden
                Return
            End If
            e.Item.Tag = data
            e.Item.Glyph = data.InfoGlyph
        End Sub

        Private Sub Calendar_ContextButtonClick(ByVal sender As Object, ByVal e As ContextItemClickEventArgs)
            Dim data As MyCustomCellData = CType(e.Item.Tag, MyCustomCellData)
            If data Is Nothing Then
                Return
            End If
            Me.memoEdit1.Text = data.InfoText
            Me.flyoutPanel1.ShowBeakForm(New Point(e.ScreenBounds.X + e.ScreenBounds.Width \ 2, e.ScreenBounds.Top - 5))
        End Sub

        Private Sub gridView1_CustomDrawCell(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs) Handles gridView1.CustomDrawCell
            Dim pictureSize As Size = imageCollection1.Images(0).Size
            If e.Column Is gridColumn3 Then
                For Each specialDate As Date In specialDays
                    If specialDate = CDate(e.CellValue) Then
                        e.Graphics.DrawImage(My.Resources.Column_Priority, New Rectangle(e.Bounds.X + 2, e.Bounds.Y, pictureSize.Width, pictureSize.Height))
                        e.Appearance.DrawString(e.Cache, e.DisplayText, New Rectangle(e.Bounds.X + pictureSize.Width, e.Bounds.Y, e.Bounds.Width - pictureSize.Width, e.Bounds.Height))
                        e.Handled = True
                    End If
                Next specialDate

            End If
        End Sub

    End Class
    Public Class MyCellStyleProvider
        Implements ICalendarCellStyleProvider

        Private image As Image = My.Resources.Column_Priority

        Private cells_Renamed As List(Of MyCustomCellData)
        Protected ReadOnly Property Cells() As List(Of MyCustomCellData)
            Get
                If cells_Renamed Is Nothing Then
                    cells_Renamed = CreateCells()
                End If
                Return cells_Renamed
            End Get
        End Property

        Protected Overridable Function CreateCells() As List(Of MyCustomCellData)
            Dim res As New List(Of MyCustomCellData)()

            res.Add(New MyCustomCellData() With {.Date = New Date(Date.Now.Year, Date.Now.Month, 1), .BackColor = Color.FromArgb(255, 209, 240, 253), .InfoText = "Mexico City. Talks with Pure Products Inc.", .InfoGlyph = image})
            res.Add(New MyCustomCellData() With {.Date = New Date(Date.Now.Year, Date.Now.Month, 2), .BackColor = Color.FromArgb(255, 209, 240, 253)})

            res.Add(New MyCustomCellData() With {.Date = New Date(Date.Now.Year, Date.Now.Month, 4), .SpecialDate = True, .InfoText = "INDEPENDENCE DAY"})
            res.Add(New MyCustomCellData() With {.Date = New Date(Date.Now.Year, Date.Now.Month, 6), .BackColor = Color.FromArgb(255, 229, 253, 177), .InfoText = "New York Knicks vs Orlando Magic", .InfoGlyph = image})
            res.Add(New MyCustomCellData() With {.Date = New Date(Date.Now.Year, Date.Now.Month, 13), .BackColor = Color.FromArgb(255, 229, 253, 177)})

            res.Add(New MyCustomCellData() With {.Date = New Date(Date.Now.Year, Date.Now.Month, 10), .BackColor = Color.FromArgb(255, 255, 228, 239), .InfoText = "Call Susanne Guper, New warehouse issues", .InfoGlyph = image})
            res.Add(New MyCustomCellData() With {.Date = New Date(Date.Now.Year, Date.Now.Month, 16), .InfoText = "JOHN" & ControlChars.Lf & "BIRTHDAY", .SpecialDate = True})

            res.Add(New MyCustomCellData() With {.Date = New Date(Date.Now.Year, Date.Now.Month, 21), .BackColor = Color.FromArgb(255, 255, 228, 239)})
            res.Add(New MyCustomCellData() With {.Date = New Date(Date.Now.Year, Date.Now.Month, 22), .InfoText = "MARY" & ControlChars.Lf & "BIRTHDAY", .SpecialDate = True})


            res.Add(New MyCustomCellData() With {.Date = New Date(Date.Now.Year, Date.Now.Month, 17), .BackColor = Color.FromArgb(255, 229, 253, 177), .InfoText = "Flatiron Club", .InfoGlyph = image})



            Return res
        End Function

        Public Function GetCell(ByVal [date] As Date) As MyCustomCellData
            Return Cells.FirstOrDefault(Function(c) c.Date.Date = [date].Date)
        End Function
        Private Sub ICalendarCellStyleProvider_UpdateAppearance(ByVal cell As CalendarCellStyle) Implements ICalendarCellStyleProvider.UpdateAppearance
            Dim cellInfo As MyCustomCellData = GetCell(cell.Date)
            If cellInfo Is Nothing Then
                Return
            End If

            cell.Description = cellInfo.Description
            If cell.Description IsNot Nothing Then
                cell.DescriptionAppearance = DirectCast(cell.Appearance.Clone(), AppearanceObject)
                cell.DescriptionAppearance.Font = New Font(cell.Appearance.Font.FontFamily, 7.0F, FontStyle.Bold)
                cell.DescriptionAppearance.TextOptions.WordWrap = WordWrap.Wrap
            End If
            If Not cellInfo.ForeColor.IsEmpty Then
                cell.Appearance.ForeColor = cellInfo.ForeColor
            End If
            If Not cellInfo.BackColor.IsEmpty Then
                cell.Appearance.BackColor = cellInfo.BackColor
            End If
            If cellInfo.SpecialDate Then
                cell.Appearance.Font = New Font(cell.Appearance.Font.FontFamily, 7.0F, FontStyle.Bold)
            End If
        End Sub
    End Class
    Public Class MyCustomCellData
        Public Property [Date]() As Date
        Public Property ForeColor() As Color
        Public Property BackColor() As Color
        Public Property InfoGlyph() As Image
        Public Property InfoText() As String

        Public Property Description() As String
        Public Property SpecialDate() As Boolean
    End Class
    Public Class Custom
        Public Property ID() As Integer
        Public Property Name() As String
        Public Property Time() As Date
    End Class
End Namespace
