using Turnaj.Logic;
using Turnaj.Model;

namespace Turnaj.Ui;

/// <summary>Veškerý výpis. Logika o této třídě nic neví.</summary>
public static class ConsoleView
{
    public static void Show(OperationResult r)
    {
        if (r.Ok) Console.WriteLine(r.Message.Length > 0 ? "OK: " + r.Message : "OK.");
        else Console.WriteLine("Chyba: " + r.Message);
    }

    public static void PrintStatus(TournamentService s)
    {
        Console.WriteLine();
        var t = s.Current;
        if (t == null)
        {
            Console.WriteLine("=== Žádný turnaj ===");
            return;
        }
        string state = t.IsFinished ? "ukončen" : "otevřený";
        Console.WriteLine($"=== {t.Name} ({t.Discipline}), {state}, kola {t.Rounds.Count}/{t.RoundCount}, přihlášeno {t.Enrolled.Count} ===");
    }

    public static void PrintPlayers(TournamentService s)
    {
        if (s.Players.Count == 0)
        {
            Console.WriteLine("Zatím nejsou žádní hráči.");
            return;
        }
        Console.WriteLine($"{"Přezdívka",-15} {"Jméno",-25} Přihlášen");
        foreach (var p in s.Players.All)
        {
            bool enrolled = s.Current != null && s.Current.IsEnrolled(p.Nickname);
            Console.WriteLine($"{p.Nickname,-15} {p.Name,-25} {(enrolled ? "ano" : "ne")}");
        }
    }

    public static void PrintRound(TournamentService s, Round r)
    {
        Console.WriteLine($"Kolo {r.Number} ({(r.IsClosed ? "uzavřené" : "otevřené")})");
        if (r.Matches.Count == 0 && r.ByeNick == null) Console.WriteLine("  (zatím bez zápasů)");
        for (int i = 0; i < r.Matches.Count; i++)
        {
            var m = r.Matches[i];
            Console.WriteLine($"  {i + 1}. {m.First} vs {m.Second}: {ResultText(m)}");
        }
        if (r.ByeNick != null) Console.WriteLine($"  Volný los: {r.ByeNick}");
    }

    public static void PrintStandings(TournamentService s)
    {
        if (s.Current == null)
        {
            Console.WriteLine("Není žádný turnaj.");
            return;
        }
        var rows = s.GetStandings();
        if (rows.Count == 0)
        {
            Console.WriteLine("Do turnaje zatím není přihlášen nikdo.");
            return;
        }
        Console.WriteLine($"{"Poř.",4} {"Přezdívka",-15} {"Jméno",-25} {"Body",5} {"Zápasy",7}");
        foreach (var row in rows)
        {
            string name = s.Players.Find(row.Nickname)?.Name ?? "";
            Console.WriteLine($"{row.Rank,4} {row.Nickname,-15} {name,-25} {row.Points,5} {row.Played,7}");
        }
    }

    private static string ResultText(Match m) => m.Result switch
    {
        MatchResult.FirstWins => "výhra " + m.First,
        MatchResult.SecondWins => "výhra " + m.Second,
        MatchResult.Draw => "remíza",
        _ => "bez výsledku"
    };
}
