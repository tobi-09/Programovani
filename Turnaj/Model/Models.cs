namespace Turnaj.Model;

public static class Nick
{
    /// <summary>Přezdívky se porovnávají bez ohledu na velikost písmen.</summary>
    public static bool Eq(string a, string b) =>
        string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
}

public class Player
{
    public string Name { get; }
    public string Nickname { get; }

    public Player(string name, string nickname)
    {
        Name = name;
        Nickname = nickname;
    }
}

public enum MatchResult
{
    None,
    FirstWins,
    SecondWins,
    Draw
}

public class Match
{
    public string First { get; }
    public string Second { get; }
    public MatchResult Result { get; set; } = MatchResult.None;

    public Match(string first, string second)
    {
        First = first;
        Second = second;
    }

    public bool HasResult => Result != MatchResult.None;
    public bool Involves(string nick) => Nick.Eq(First, nick) || Nick.Eq(Second, nick);
}

public class Round
{
    public int Number { get; }
    public List<Match> Matches { get; } = new();
    public string? ByeNick { get; set; }
    public bool IsClosed { get; set; }

    public Round(int number)
    {
        Number = number;
    }

    public bool Involves(string nick) =>
        Matches.Any(m => m.Involves(nick)) || (ByeNick != null && Nick.Eq(ByeNick, nick));
}

public class Tournament
{
    public string Name { get; }
    public string Discipline { get; }
    public int RoundCount { get; }
    public List<string> Enrolled { get; } = new();
    public List<Round> Rounds { get; } = new();
    public bool IsFinished { get; set; }

    public Tournament(string name, string discipline, int roundCount)
    {
        Name = name;
        Discipline = discipline;
        RoundCount = roundCount;
    }

    /// <summary>Turnaj začal vytvořením prvního kola.</summary>
    public bool Started => Rounds.Count > 0;

    public bool IsEnrolled(string nick) => Enrolled.Any(e => Nick.Eq(e, nick));
}

public record StandingRow(int Rank, string Nickname, int Points, int Played);
