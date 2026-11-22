
Public Class BonusCalculator
    Public Structure TargetResult
        Public ExtraCards As Integer
        Public PendingCards As Integer
        Public BonusPoints As Integer
    End Structure

    ''' <summary>
    ''' Calculates extra cards, pending cards, and monthly bonus points
    ''' for a Business Executive based on monthly target cards and cards sold.
    ''' </summary>
    Public Shared Function CalculateExecutiveTarget(targetCards As Integer, cardsSold As Integer) As TargetResult
        Dim result As New TargetResult()

        If targetCards < 0 OrElse cardsSold < 0 Then
            Throw New ArgumentException("Target cards and cards sold must be non-negative.")
        End If

        If cardsSold >= targetCards Then
            result.ExtraCards = cardsSold - targetCards
            result.PendingCards = 0
            result.BonusPoints = 1 + (result.ExtraCards \ 2) * 2
        Else
            result.ExtraCards = 0
            result.PendingCards = targetCards - cardsSold
            result.BonusPoints = 0
        End If

        Return result
    End Function

    ''' <summary>
    ''' Calculates monthly bonus points for a Business Manager based on whether
    ''' team members achieved their targets, manager extra cards, and pending card status.
    ''' </summary>
    Public Shared Function CalculateManagerBonusPoints(
        allTeamMembersAchievedTarget As Boolean,
        managerExtraCards As Integer,
        hasPendingCards As Boolean
    ) As Integer
        If managerExtraCards < 0 Then
            Throw New ArgumentException("Manager extra cards must be non-negative.")
        End If

        If hasPendingCards Then
            Return 0
        End If

        Dim points As Integer = 0
        If allTeamMembersAchievedTarget Then
            points += 1
        End If

        points += (managerExtraCards \ 2) * 2
        Return points
    End Function
End Class
