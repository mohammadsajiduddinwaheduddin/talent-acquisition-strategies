Imports System.Windows.Forms

Public Class MainMenuForm
    Inherits Form

    Private ReadOnly _menuStrip As New MenuStrip()

    Public Sub New()
        Me.Text = "Talent Acquisition Strategies"
        Me.IsMdiContainer = True
        Me.Width = 1024
        Me.Height = 700

        ' MASTER menu items
        Dim masterMenu As New ToolStripMenuItem("MASTER")
        masterMenu.DropDownItems.Add("Executive Application", Nothing, AddressOf OpenModuleDialog)
        masterMenu.DropDownItems.Add("Employee Confirmation", Nothing, AddressOf OpenModuleDialog)
        masterMenu.DropDownItems.Add("Branch Information", Nothing, AddressOf OpenModuleDialog)

        ' TRANSACTIONS menu items
        Dim transactionsMenu As New ToolStripMenuItem("TRANSACTIONS")
        transactionsMenu.DropDownItems.Add("Employee's Target", Nothing, AddressOf OpenModuleDialog)
        transactionsMenu.DropDownItems.Add("Debit Card Application", Nothing, AddressOf OpenModuleDialog)
        transactionsMenu.DropDownItems.Add("Debit Card Details", Nothing, AddressOf OpenModuleDialog)
        transactionsMenu.DropDownItems.Add("Debit Card Transactions", Nothing, AddressOf OpenModuleDialog)
        transactionsMenu.DropDownItems.Add("Credit Card Application", Nothing, AddressOf OpenModuleDialog)
        transactionsMenu.DropDownItems.Add("Credit Card Enquiry", Nothing, AddressOf OpenModuleDialog)
        transactionsMenu.DropDownItems.Add("Credit Card Details", Nothing, AddressOf OpenModuleDialog)
        transactionsMenu.DropDownItems.Add("Credit Card Transactions", Nothing, AddressOf OpenModuleDialog)
        transactionsMenu.DropDownItems.Add("Credit Card Bills", Nothing, AddressOf OpenModuleDialog)

        ' REPORTS menu items
        Dim reportsMenu As New ToolStripMenuItem("REPORTS")
        reportsMenu.DropDownItems.Add("Performance of Employee", Nothing, AddressOf OpenModuleDialog)

        _menuStrip.Items.AddRange(New ToolStripItem() {masterMenu, transactionsMenu, reportsMenu})
        Me.MainMenuStrip = _menuStrip
        Me.Controls.Add(_menuStrip)
    End Sub

    Private Sub OpenModuleDialog(sender As Object, e As EventArgs)
        Dim clickedItem = TryCast(sender, ToolStripMenuItem)
        Dim moduleName As String = If(clickedItem?.Text, "Module")
        MessageBox.Show(
            $"Selected module: {moduleName}",
            "Talent Acquisition Strategies",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information
        )
    End Sub
End Class
