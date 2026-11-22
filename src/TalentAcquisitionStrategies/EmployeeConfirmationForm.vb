Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Windows.Forms

Public Class EmployeeConfirmationForm
    Inherits Form

    Private ReadOnly _db As DatabaseHelper

    Private ReadOnly txtAppNo As New TextBox()
    Private ReadOnly txtExeCode As New TextBox()
    Private ReadOnly cmbDesignation As New ComboBox()
    Private ReadOnly dtpConfDate As New DateTimePicker()
    Private ReadOnly txtBasic As New TextBox()
    Private ReadOnly dtpDoj As New DateTimePicker()
    Private ReadOnly cmbBranchCode As New ComboBox()
    Private ReadOnly txtManagerNo As New TextBox()
    Private ReadOnly chkConfirmed As New CheckBox()

    Private ReadOnly btnSave As New Button()
    Private ReadOnly btnClear As New Button()
    Private ReadOnly btnExit As New Button()

    Public Sub New(Optional dbHelper As DatabaseHelper = Nothing)
        _db = If(dbHelper, New DatabaseHelper("Server=localhost;Database=TalentAcquisitionDB;Trusted_Connection=True;"))
        InitializeFormLayout()
        LoadBranches()
    End Sub

    Private Sub InitializeFormLayout()
        Me.Text = "CONFORMATION OF EMPLOYEE"
        Me.Size = New Size(540, 470)
        Me.StartPosition = FormStartPosition.CenterScreen

        Dim lblTitle As New Label() With {
            .Text = "CONFORMATION OF EMPLOYEE",
            .Font = New Font("Arial", 14, FontStyle.Bold Or FontStyle.Underline),
            .AutoSize = True,
            .Location = New Point(120, 20)
        }
        Me.Controls.Add(lblTitle)

        cmbDesignation.DropDownStyle = ComboBoxStyle.DropDownList
        cmbDesignation.Items.AddRange(New Object() {"Marketing Executive", "Marketing Manager"})

        dtpConfDate.Format = DateTimePickerFormat.Short
        dtpDoj.Format = DateTimePickerFormat.Short

        AddFieldRow("APPLICATION NUMBER", txtAppNo, 70)
        AddFieldRow("EXECUTIVE_CODE", txtExeCode, 105)
        AddFieldRow("DESIGNATION", cmbDesignation, 140)
        AddFieldRow("CONFIRMATION DATE", dtpConfDate, 175)
        AddFieldRow("SALARY (BASIC)", txtBasic, 210)
        AddFieldRow("DATE OF JOIN", dtpDoj, 245)
        AddFieldRow("BRANCH CODE", cmbBranchCode, 280)
        AddFieldRow("MANAGER NUMBER", txtManagerNo, 315)

        chkConfirmed.Text = "He/She Confirm (FLAG = Y)"
        chkConfirmed.Checked = True
        chkConfirmed.Location = New Point(255, 350)
        chkConfirmed.Size = New Size(220, 24)
        Me.Controls.Add(chkConfirmed)

        btnSave.Text = "SAVE"
        btnSave.Location = New Point(110, 390)
        btnSave.Size = New Size(90, 30)
        AddHandler btnSave.Click, AddressOf OnSaveClick

        btnClear.Text = "CLEAR"
        btnClear.Location = New Point(220, 390)
        btnClear.Size = New Size(90, 30)
        AddHandler btnClear.Click, AddressOf OnClearClick

        btnExit.Text = "EXIT"
        btnExit.Location = New Point(330, 390)
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
        inputControl.Size = New Size(210, 24)

        Me.Controls.Add(lbl)
        Me.Controls.Add(inputControl)
    End Sub

    Private Sub LoadBranches()
        Try
            Dim dt As DataTable = _db.ExecuteQuery("SELECT Branch_Code FROM BRANCH ORDER BY Branch_Code")
            cmbBranchCode.DataSource = dt
            cmbBranchCode.DisplayMember = "Branch_Code"
            cmbBranchCode.ValueMember = "Branch_Code"
        Catch ex As Exception
            cmbBranchCode.DropDownStyle = ComboBoxStyle.DropDown
            If cmbBranchCode.Items.Count = 0 Then
                cmbBranchCode.Items.AddRange(New Object() {"BR001", "BR002"})
            End If
        End Try
    End Sub

    Private Sub OnSaveClick(sender As Object, e As EventArgs)
        Dim appNo As String = txtAppNo.Text.Trim()
        Dim exeCode As String = txtExeCode.Text.Trim()
        Dim desig As String = If(cmbDesignation.SelectedItem?.ToString(), cmbDesignation.Text.Trim())
        Dim branchCode As String = cmbBranchCode.Text.Trim()
        Dim basicSalary As Decimal

        If String.IsNullOrWhiteSpace(appNo) OrElse String.IsNullOrWhiteSpace(exeCode) OrElse String.IsNullOrWhiteSpace(desig) Then
            MessageBox.Show("Application Number, Executive Code, and Designation are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If Not Decimal.TryParse(txtBasic.Text.Trim(), basicSalary) OrElse basicSalary <= 0D Then
            MessageBox.Show("Please enter a valid positive Basic Salary.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim sql As String =
            "INSERT INTO EXE_CONFIRM (Exe_Code, App_no, Desig, Conf_Date, Basic, DOJ, Branch_Code, BmgrNo, Flag) " &
            "VALUES (@Exe_Code, @App_no, @Desig, @Conf_Date, @Basic, @DOJ, @Branch_Code, @BmgrNo, @Flag)"

        Dim parameters As SqlParameter() = {
            New SqlParameter("@Exe_Code", exeCode),
            New SqlParameter("@App_no", appNo),
            New SqlParameter("@Desig", desig),
            New SqlParameter("@Conf_Date", dtpConfDate.Value.Date),
            New SqlParameter("@Basic", basicSalary),
            New SqlParameter("@DOJ", dtpDoj.Value.Date),
            New SqlParameter("@Branch_Code", branchCode),
            New SqlParameter("@BmgrNo", If(String.IsNullOrWhiteSpace(txtManagerNo.Text), CType(DBNull.Value, Object), txtManagerNo.Text.Trim())),
            New SqlParameter("@Flag", If(chkConfirmed.Checked, "Y", "N"))
        }

        Try
            _db.ExecuteNonQuery(sql, parameters)
            MessageBox.Show("Employee confirmation saved successfully.", "Talent Acquisition Strategies", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show($"Could not save confirmation: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub OnClearClick(sender As Object, e As EventArgs)
        txtAppNo.Clear()
        txtExeCode.Clear()
        txtBasic.Clear()
        txtManagerNo.Clear()
        cmbDesignation.SelectedIndex = -1
        chkConfirmed.Checked = True
    End Sub
End Class
