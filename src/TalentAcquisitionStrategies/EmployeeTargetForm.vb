Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Windows.Forms

Public Class EmployeeTargetForm
    Inherits Form

    Private ReadOnly _db As DatabaseHelper

    Private ReadOnly cmbExeCode As New ComboBox()
    Private ReadOnly txtTargetCards As New TextBox()
    Private ReadOnly txtExtraCards As New TextBox()
    Private ReadOnly txtTargetMonthYr As New TextBox()
    Private ReadOnly txtCardsDone As New TextBox()
    Private ReadOnly txtPendingCards As New TextBox()
    Private ReadOnly txtBonusPoints As New TextBox()

    Private ReadOnly btnCalculate As New Button()
    Private ReadOnly btnSave As New Button()
    Private ReadOnly btnClear As New Button()
    Private ReadOnly btnExit As New Button()

    Public Sub New(Optional dbHelper As DatabaseHelper = Nothing)
        _db = If(dbHelper, New DatabaseHelper("Server=localhost;Database=TalentAcquisitionDB;Trusted_Connection=True;"))
        InitializeFormLayout()
        LoadConfirmedExecutives()
    End Sub

    Private Sub InitializeFormLayout()
        Me.Text = "EMPLOYEE'S TARGET"
        Me.Size = New Size(520, 420)
        Me.StartPosition = FormStartPosition.CenterScreen

        Dim lblTitle As New Label() With {
            .Text = "EMPLOYEE'S TARGET",
            .Font = New Font("Arial", 14, FontStyle.Bold Or FontStyle.Underline),
            .AutoSize = True,
            .Location = New Point(150, 20)
        }
        Me.Controls.Add(lblTitle)

        AddFieldRow("EMPLOYEE CODE", cmbExeCode, 70, readOnlyField:=False)
        AddFieldRow("TARGET CARDS", txtTargetCards, 105, readOnlyField:=False)
        AddFieldRow("EXTRA CARDS", txtExtraCards, 140, readOnlyField:=True)
        AddFieldRow("TARGET_MON_YR", txtTargetMonthYr, 175, readOnlyField:=False)
        AddFieldRow("TOTAL CARDS DONE BY HIM/HER", txtCardsDone, 210, readOnlyField:=False)
        AddFieldRow("PENDING CARDS", txtPendingCards, 245, readOnlyField:=True)
        AddFieldRow("BONUS POINTS", txtBonusPoints, 280, readOnlyField:=True)

        btnCalculate.Text = "CALCULATE"
        btnCalculate.Location = New Point(50, 330)
        btnCalculate.Size = New Size(95, 30)
        AddHandler btnCalculate.Click, AddressOf OnCalculateClick

        btnSave.Text = "SAVE"
        btnSave.Location = New Point(155, 330)
        btnSave.Size = New Size(85, 30)
        AddHandler btnSave.Click, AddressOf OnSaveClick

        btnClear.Text = "CLEAR"
        btnClear.Location = New Point(250, 330)
        btnClear.Size = New Size(85, 30)
        AddHandler btnClear.Click, AddressOf OnClearClick

        btnExit.Text = "EXIT"
        btnExit.Location = New Point(345, 330)
        btnExit.Size = New Size(85, 30)
        AddHandler btnExit.Click, Sub() Me.Close()

        Me.Controls.AddRange(New Control() {btnCalculate, btnSave, btnClear, btnExit})
    End Sub

    Private Sub AddFieldRow(labelText As String, inputControl As Control, yPos As Integer, readOnlyField As Boolean)
        Dim lbl As New Label() With {
            .Text = labelText,
            .Location = New Point(35, yPos + 3),
            .Size = New Size(210, 20),
            .Font = New Font("Arial", 9, FontStyle.Bold)
        }
        inputControl.Location = New Point(255, yPos)
        inputControl.Size = New Size(200, 24)

        If TypeOf inputControl Is TextBox Then
            DirectCast(inputControl, TextBox).ReadOnly = readOnlyField
        End If

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
            ' Allow manual entry when running offline without an active SQL Server connection
            cmbExeCode.DropDownStyle = ComboBoxStyle.DropDown
            If cmbExeCode.Items.Count = 0 Then
                cmbExeCode.Items.AddRange(New Object() {"ME001", "MM001"})
            End If
        End Try
    End Sub

    Private Sub OnCalculateClick(sender As Object, e As EventArgs)
        Dim targetCards As Integer
        Dim cardsDone As Integer

        If Not Integer.TryParse(txtTargetCards.Text.Trim(), targetCards) OrElse
           Not Integer.TryParse(txtCardsDone.Text.Trim(), cardsDone) Then
            MessageBox.Show("Please enter valid numeric values for Target Cards and Total Cards Done.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Dim calc = BonusCalculator.CalculateExecutiveTarget(targetCards, cardsDone)
            txtExtraCards.Text = calc.ExtraCards.ToString()
            txtPendingCards.Text = calc.PendingCards.ToString()
            txtBonusPoints.Text = calc.BonusPoints.ToString()
        Catch ex As ArgumentException
            MessageBox.Show(ex.Message, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub OnSaveClick(sender As Object, e As EventArgs)
        OnCalculateClick(sender, e)

        Dim exeCode As String = cmbExeCode.Text.Trim()
        Dim monthYr As String = txtTargetMonthYr.Text.Trim()

        If String.IsNullOrWhiteSpace(exeCode) OrElse String.IsNullOrWhiteSpace(monthYr) Then
            MessageBox.Show("Please select an Employee Code and enter TARGET_MON_YR.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim sql As String =
            "INSERT INTO EXE_TARGET (Exe_Code, Target_Cards, Extra_Cards, Target_Month_Yr, Noc, Noc_Pending, Bonus_pts) " &
            "VALUES (@Exe_Code, @Target_Cards, @Extra_Cards, @Target_Month_Yr, @Noc, @Noc_Pending, @Bonus_pts)"

        Dim parameters As SqlParameter() = {
            New SqlParameter("@Exe_Code", exeCode),
            New SqlParameter("@Target_Cards", Integer.Parse(txtTargetCards.Text.Trim())),
            New SqlParameter("@Extra_Cards", Integer.Parse(txtExtraCards.Text.Trim())),
            New SqlParameter("@Target_Month_Yr", monthYr),
            New SqlParameter("@Noc", Integer.Parse(txtCardsDone.Text.Trim())),
            New SqlParameter("@Noc_Pending", Integer.Parse(txtPendingCards.Text.Trim())),
            New SqlParameter("@Bonus_pts", Integer.Parse(txtBonusPoints.Text.Trim()))
        }

        Try
            _db.ExecuteNonQuery(sql, parameters)
            MessageBox.Show("Record saved successfully.", "Talent Acquisition Strategies", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show($"Could not save record: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub OnClearClick(sender As Object, e As EventArgs)
        txtTargetCards.Clear()
        txtExtraCards.Clear()
        txtTargetMonthYr.Clear()
        txtCardsDone.Clear()
        txtPendingCards.Clear()
        txtBonusPoints.Clear()
    End Sub
End Class
