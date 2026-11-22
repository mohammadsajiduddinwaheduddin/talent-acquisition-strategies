Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Windows.Forms

Public Class CreditCardBillsForm
    Inherits Form

    Private ReadOnly _db As DatabaseHelper

    Private ReadOnly txtBillNo As New TextBox()
    Private ReadOnly txtCcNo As New TextBox()
    Private ReadOnly txtCcTransNo As New TextBox()
    Private ReadOnly dtpBillDate As New DateTimePicker()
    Private ReadOnly dtpDueDate As New DateTimePicker()
    Private ReadOnly txtBillAmt As New TextBox()
    Private ReadOnly txtAmtPaid As New TextBox()
    Private ReadOnly rbCleared As New RadioButton()
    Private ReadOnly rbNotCleared As New RadioButton()

    Private ReadOnly btnSave As New Button()
    Private ReadOnly btnClear As New Button()
    Private ReadOnly btnExit As New Button()

    Public Sub New(Optional dbHelper As DatabaseHelper = Nothing)
        _db = If(dbHelper, New DatabaseHelper("Server=localhost;Database=TalentAcquisitionDB;Trusted_Connection=True;"))
        InitializeFormLayout()
    End Sub

    Private Sub InitializeFormLayout()
        Me.Text = "BILLS"
        Me.Size = New Size(520, 440)
        Me.StartPosition = FormStartPosition.CenterScreen

        Dim lblTitle As New Label() With {
            .Text = "BILLS",
            .Font = New Font("Arial", 14, FontStyle.Bold Or FontStyle.Underline),
            .AutoSize = True,
            .Location = New Point(220, 20)
        }
        Me.Controls.Add(lblTitle)

        dtpBillDate.Format = DateTimePickerFormat.Short
        dtpDueDate.Format = DateTimePickerFormat.Short

        AddFieldRow("BILL NUMBER", txtBillNo, 70)
        AddFieldRow("CREDIT CARD NUMBER", txtCcNo, 105)
        AddFieldRow("CREDIT CARD TRANSACTION NO", txtCcTransNo, 140)
        AddFieldRow("BILL DATE", dtpBillDate, 175)
        AddFieldRow("DUE DATE", dtpDueDate, 210)
        AddFieldRow("BILL AMOUNT", txtBillAmt, 245)
        AddFieldRow("AMOUNT PAID", txtAmtPaid, 280)

        Dim grpStatus As New GroupBox() With {
            .Text = "BILL STATUS",
            .Location = New Point(120, 315),
            .Size = New Size(260, 45)
        }
        rbCleared.Text = "CLEARED"
        rbCleared.Location = New Point(25, 18)
        rbCleared.Size = New Size(100, 20)
        rbCleared.Checked = True

        rbNotCleared.Text = "NOT CLEARED"
        rbNotCleared.Location = New Point(135, 18)
        rbNotCleared.Size = New Size(115, 20)

        grpStatus.Controls.AddRange(New Control() {rbCleared, rbNotCleared})
        Me.Controls.Add(grpStatus)

        btnSave.Text = "SAVE"
        btnSave.Location = New Point(110, 370)
        btnSave.Size = New Size(90, 30)
        AddHandler btnSave.Click, AddressOf OnSaveClick

        btnClear.Text = "CLEAR"
        btnClear.Location = New Point(220, 370)
        btnClear.Size = New Size(90, 30)
        AddHandler btnClear.Click, AddressOf OnClearClick

        btnExit.Text = "EXIT"
        btnExit.Location = New Point(330, 370)
        btnExit.Size = New Size(90, 30)
        AddHandler btnExit.Click, Sub() Me.Close()

        Me.Controls.AddRange(New Control() {btnSave, btnClear, btnExit})
    End Sub

    Private Sub AddFieldRow(labelText As String, inputControl As Control, yPos As Integer)
        Dim lbl As New Label() With {
            .Text = labelText,
            .Location = New Point(35, yPos + 3),
            .Size = New Size(215, 20),
            .Font = New Font("Arial", 9, FontStyle.Bold)
        }
        inputControl.Location = New Point(255, yPos)
        inputControl.Size = New Size(205, 24)

        Me.Controls.Add(lbl)
        Me.Controls.Add(inputControl)
    End Sub

    Private Sub OnSaveClick(sender As Object, e As EventArgs)
        Dim billNo As String = txtBillNo.Text.Trim()
        Dim ccNo As String = txtCcNo.Text.Trim()
        Dim transNo As Integer
        Dim billAmt As Decimal
        Dim amtPaid As Decimal

        If String.IsNullOrWhiteSpace(billNo) OrElse String.IsNullOrWhiteSpace(ccNo) Then
            MessageBox.Show("Bill Number and Credit Card Number are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If Not Integer.TryParse(txtCcTransNo.Text.Trim(), transNo) OrElse
           Not Decimal.TryParse(txtBillAmt.Text.Trim(), billAmt) OrElse
           Not Decimal.TryParse(txtAmtPaid.Text.Trim(), amtPaid) Then
            MessageBox.Show("Please enter valid numeric values for Transaction No, Bill Amount, and Amount Paid.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim billStatus As String = If(rbCleared.Checked, "CLEARED", "NOT CLEARED")

        Dim sql As String =
            "INSERT INTO BILLS (Bill_No, CC_Trans_No, CCNO, Bill_date, Due_date, Bill_Amt, Amt_Paid, Bill_Status) " &
            "VALUES (@Bill_No, @CC_Trans_No, @CCNO, @Bill_date, @Due_date, @Bill_Amt, @Amt_Paid, @Bill_Status)"

        Dim parameters As SqlParameter() = {
            New SqlParameter("@Bill_No", billNo),
            New SqlParameter("@CC_Trans_No", transNo),
            New SqlParameter("@CCNO", ccNo),
            New SqlParameter("@Bill_date", dtpBillDate.Value.Date),
            New SqlParameter("@Due_date", dtpDueDate.Value.Date),
            New SqlParameter("@Bill_Amt", billAmt),
            New SqlParameter("@Amt_Paid", amtPaid),
            New SqlParameter("@Bill_Status", billStatus)
        }

        Try
            _db.ExecuteNonQuery(sql, parameters)
            MessageBox.Show("Bill record saved successfully.", "Talent Acquisition Strategies", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show($"Could not save bill: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub OnClearClick(sender As Object, e As EventArgs)
        txtBillNo.Clear()
        txtCcNo.Clear()
        txtCcTransNo.Clear()
        txtBillAmt.Clear()
        txtAmtPaid.Clear()
        rbCleared.Checked = True
    End Sub
End Class
