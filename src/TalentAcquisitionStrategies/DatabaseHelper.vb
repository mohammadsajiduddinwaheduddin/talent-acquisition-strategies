Imports System.Data
Imports System.Data.SqlClient

Public Class DatabaseHelper
    Private ReadOnly _connectionString As String

    Public Sub New(connectionString As String)
        If String.IsNullOrWhiteSpace(connectionString) Then
            Throw New ArgumentException("Connection string cannot be empty.", NameOf(connectionString))
        End If
        _connectionString = connectionString
    End Sub

    ''' <summary>
    ''' Executes a SELECT query and returns the results as a DataTable
    ''' using SqlDataAdapter (ADO.NET disconnected model).
    ''' </summary>
    Public Function ExecuteQuery(sql As String, Optional parameters As SqlParameter() = Nothing) As DataTable
        Dim table As New DataTable()

        Using conn As New SqlConnection(_connectionString)
            Using cmd As New SqlCommand(sql, conn)
                If parameters IsNot Nothing Then
                    cmd.Parameters.AddRange(parameters)
                End If

                Using adapter As New SqlDataAdapter(cmd)
                    adapter.Fill(table)
                End Using
            End Using
        End Using

        Return table
    End Function

    ''' <summary>
    ''' Executes an INSERT, UPDATE, or DELETE command and returns
    ''' the number of affected rows.
    ''' </summary>
    Public Function ExecuteNonQuery(sql As String, Optional parameters As SqlParameter() = Nothing) As Integer
        Using conn As New SqlConnection(_connectionString)
            Using cmd As New SqlCommand(sql, conn)
                If parameters IsNot Nothing Then
                    cmd.Parameters.AddRange(parameters)
                End If

                conn.Open()
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Executes a query and returns the first column of the first row.
    ''' </summary>
    Public Function ExecuteScalar(sql As String, Optional parameters As SqlParameter() = Nothing) As Object
        Using conn As New SqlConnection(_connectionString)
            Using cmd As New SqlCommand(sql, conn)
                If parameters IsNot Nothing Then
                    cmd.Parameters.AddRange(parameters)
                End If

                conn.Open()
                Return cmd.ExecuteScalar()
            End Using
        End Using
    End Function
End Class
