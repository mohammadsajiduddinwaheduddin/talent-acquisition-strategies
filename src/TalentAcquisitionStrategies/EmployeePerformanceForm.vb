Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Windows.Forms

Public Class EmployeePerformanceForm
    Inherits Form

    Private ReadOnly _db As DatabaseHelper

    Private ReadOnly cmbExeCode As New ComboBox()
    Private ReadOnly txtTotalPoints As New TextBox()
    Private ReadOnly txtRankCode As New TextBox()
    Private ReadOnly dtpPchkDate As New DateTimePicker()
    Private ReadOnly txtRating As New TextBox()
    Private ReadOnly dtpNextPchkDate As New DateTimePicker()

    Private ReadOnly btnSave As New Button()
    Private ReadOnly btnClear As New Button()
    Private ReadOnly btnExit As New Button()

    Public Sub New(Optional dbHelper As DatabaseHelper = Nothing)
        _db = If(dbHelper, New DatabaseHelper("Server=localhost;Database=TalentAcquisitionDB;Trusted_Connection=True;"))
        InitializeFormLayout()
        LoadConfirmedExecutives()
    End Sub

    Private Sub InitializeFormLayout()
        Me.Text = "PERFORMANCE OF EMPLOYEE"
        Me.Size = New Size(520, 390)
        Me.StartPosition = FormStartPosition.CenterScreen

        Dim lblTitle As New Label() With {
            .Text = "PERFORMANCE OF EMPLOYEE",
            .Font = New Font("Arial", 14, FontStyle.Bold Or FontStyle.Underline),
            .AutoSize = True,
            .Location = New Point(125, 20)
        }
        Me.Controls.Add(lblTitle)

        dtpPchkDate.Format = DateTimePickerFormat.Short
        dtpNextPchkDate.Format = DateTimePickerFormat.Short

        AddFieldRow("EMPLOYEE CODE", cmbExeCode, 70)
        AddFieldRow("TOTAL POINTS", txtTotalPoints, 105)
        AddFieldRow("RANK", txtRankCode, 140)
        AddFieldRow("PERFORMANCE CHECK DATE", dtpPchkDate, 175)
        AddFieldRow("OVERALL RATING", txtRating, 210)
        AddFieldRow("NEXT CHECKING DATE", dtpNextPchkDate, 245)

        btnSave.Text = "SAVE"
        btnSave.Location = New Point(110, 300)
        btnSave.Size = New Size(90, 30)
        AddHandler btnSave.Click, AddressOf OnSaveClick

        btnClear.Text = "CLEAR"
        btnClear.Location = New Point(220, 300)
        btnClear.Size = New Size(90, 30)
        AddHandler btnClear.Click, AddressOf OnClearClick

        btnExit.Text = "EXIT"
        btnExit.Location = New Point(330, 300)
        btnExit.Size = New Size(90, 30)
        AddHandler btnExit.Click, Sub() Me.Close()

        Me.Controls.AddRange(New Control() {btnSave, btnClear, btnExit})
    End Sub

    Private Sub AddFieldRow(labelText As String, inputControl As Control, yPos As Integer)
        Dim lbl As New Label() With {
            .Text = labelText,
            .Location = New Point(35, yPos + 3),
            .Size = New Size(210, 20),
            .Font = New Font("Arial", 9, FontStyle.Bold)
        }
        inputControl.Location = New Point(255, yPos)
        inputControl.Size = New Size(205, 24)

        Me.Controls.Add(lbl)
        Me.Controls.Add(inputControl)
    End Sub

    Private Sub LoadConfirmedExecutives()
        Try
            Dim dt As DataTable = _db.ExecuteQuery("SELECT Exe_Code FROM EXE_CONFIRM WHERE Flag = 'Y' ORDER BY Exe_Code")
            cmbExeCode.DataSource = dt
            cmbExeCode.DisplayMember = "Exe_Code"
            cmbExeCode.ValueMember = "Exe_Code"
        Catch ex As Exception
            cmbExeCode.DropDownStyle = ComboBoxStyle.DropDown
            If cmbExeCode.Items.Count = 0 Then
                cmbExeCode.Items.AddRange(New Object() {"ME001", "MM001"})
            End If
        End Try
    End Sub

    Private Sub OnSaveClick(sender As Object, e As EventArgs)
        Dim exeCode As String = cmbExeCode.Text.Trim()
        Dim totalPoints As Integer
        Dim rating As Decimal

        If String.IsNullOrWhiteSpace(exeCode) Then
            MessageBox.Show("Please select or enter an Employee Code.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If Not Integer.TryParse(txtTotalPoints.Text.Trim(), totalPoints) OrElse
           Not Decimal.TryParse(txtRating.Text.Trim(), rating) Then
            MessageBox.Show("Please enter valid numeric values for Total Points and Overall Rating.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim sql As String =
            "INSERT INTO EXE_PERFORMANCE (Exe_Code, Total_Points, Pchk_date, Rank_Code, Rating, Next_pchkdate) " &
            "VALUES (@Exe_Code, @Total_Points, @Pchk_date, @Rank_Code, @Rating, @Next_pchkdate)"

        Dim parameters As SqlParameter() = {
            New SqlParameter("@Exe_Code", exeCode),
            New SqlParameter("@Total_Points", totalPoints),
            New SqlParameter("@Pchk_date", dtpPchkDate.Value.Date),
            New SqlParameter("@Rank_Code", If(String.IsNullOrWhiteSpace(txtRankCode.Text), CType(DBNull.Value, Object), txtRankCode.Text.Trim())),
            New SqlParameter("@Rating", rating),
            New SqlParameter("@Next_pchkdate", dtpNextPchkDate.Value.Date)
        }

        Try
            _db.ExecuteNonQuery(sql, parameters)
            MessageBox.Show("Performance record saved successfully.", "Talent Acquisition Strategies", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show($"Could not save performance record: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub OnClearClick(sender As Object, e As EventArgs)
        txtTotalPoints.Clear()
        txtRankCode.Clear()
        txtRating.Clear()
    End Sub
End Class
