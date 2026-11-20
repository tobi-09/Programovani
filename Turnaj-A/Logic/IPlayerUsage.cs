namespace Turnaj.Logic;

/// <summary>
/// Spojovací bod na část ostatních členů týmu (turnaj, zápasy).
/// TournamentService ho později implementuje, zatím se používá NoPlayerUsage.
/// </summary>
public interface IPlayerUsage
{
    bool HasPlayedMatch(string nick);
    bool IsEnrolledInOpenTournament(string nick);
}

public class NoPlayerUsage : IPlayerUsage
{
    public bool HasPlayedMatch(string nick) => false;
    public bool IsEnrolledInOpenTournament(string nick) => false;
}
