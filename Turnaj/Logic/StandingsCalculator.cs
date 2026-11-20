using Turnaj.Model;

namespace Turnaj.Logic;

/// <summary>Počítá tabulku podle SPECIFIKACE.md. Tabulka se neukládá, vždy se spočítá znovu.</summary>
public static class StandingsCalculator
{
    public const int WinPoints = 2;
    public const int DrawPoints = 1;
    public const int ByePoints = 2;

    public static List<StandingRow> Compute(Tournament t)
    {
        var points = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        var played = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        foreach (var n in t.Enrolled)
        {
            points[n] = 0;
            played[n] = 0;
        }

        foreach (var round in t.Rounds)
        {
            foreach (var m in round.Matches)
            {
                if (!m.HasResult) continue;
                played[m.First]++;
                played[m.Second]++;
                switch (m.Result)
                {
                    case MatchResult.FirstWins: points[m.First] += WinPoints; break;
                    case MatchResult.SecondWins: points[m.Second] += WinPoints; break;
                    case MatchResult.Draw:
                        points[m.First] += DrawPoints;
                        points[m.Second] += DrawPoints;
                        break;
                }
            }
            // volný los: body ano, odehraný zápas ne
            if (round.ByeNick != null) points[round.ByeNick] += ByePoints;
        }

        var sorted = points
            .Select(kv => (Nick: kv.Key, Points: kv.Value, Played: played[kv.Key]))
            .OrderByDescending(x => x.Points)
            .ThenBy(x => x.Nick, StringComparer.OrdinalIgnoreCase)
            .ToList();

        var rows = new List<StandingRow>();
        int rank = 0;
        for (int i = 0; i < sorted.Count; i++)
        {
            if (i == 0 || sorted[i].Points != sorted[i - 1].Points) rank = i + 1;
            rows.Add(new StandingRow(rank, sorted[i].Nick, sorted[i].Points, sorted[i].Played));
        }
        return rows;
    }
}
