using System.Globalization;
using System.Text;
using Turnaj.Logic;
using Turnaj.Model;

namespace Turnaj.Storage;

public enum LoadStatus { Ok, Missing, Corrupted, IoError }

public class LoadResult
{
    public LoadStatus Status { get; }
    public string Message { get; }
    public PlayerRegistry? Players { get; }
    public Tournament? Tournament { get; }

    public LoadResult(LoadStatus status, string message,
        PlayerRegistry? players = null, Tournament? tournament = null)
    {
        Status = status;
        Message = message;
        Players = players;
        Tournament = tournament;
    }
}

/// <summary>Vlastní řádkový formát, popis je v NAVRH.md.</summary>
public static class TournamentFileStore
{
    public const string Header = "TURNAJ-DAT";
    public const string Version = "1";

    // ---------- Uložení ----------

    public static OperationResult Save(string path, PlayerRegistry players, Tournament? t)
    {
        var records = new List<string>();
        foreach (var p in players.All)
            records.Add(LineCodec.Join("P", p.Nickname, p.Name));

        if (t != null)
        {
            records.Add(LineCodec.Join("T", t.Name, t.Discipline,
                t.RoundCount.ToString(CultureInfo.InvariantCulture), t.IsFinished ? "0" : "1"));
            foreach (var n in t.Enrolled) records.Add(LineCodec.Join("E", n));
            foreach (var r in t.Rounds)
            {
                string no = r.Number.ToString(CultureInfo.InvariantCulture);
                records.Add(LineCodec.Join("R", no, r.IsClosed ? "1" : "0"));
                foreach (var m in r.Matches)
                    records.Add(LineCodec.Join("M", no, m.First, m.Second, ResultToCode(m.Result)));
                if (r.ByeNick != null) records.Add(LineCodec.Join("B", no, r.ByeNick));
            }
        }

        var lines = new List<string> { LineCodec.Join(Header, Version) };
        lines.AddRange(records);
        lines.Add(LineCodec.Join("END", records.Count.ToString(CultureInfo.InvariantCulture)));

        try
        {
            // zápis přes dočasný soubor, aby chyba nezničila původní data
            string tmp = path + ".tmp";
            File.WriteAllLines(tmp, lines, new UTF8Encoding(false));
            File.Move(tmp, path, true);
            return OperationResult.Success($"Uloženo do souboru '{path}'.");
        }
        catch (Exception ex)
        {
            return OperationResult.Fail($"Soubor se nepodařilo uložit: {ex.Message}");
        }
    }

    // ---------- Načtení ----------

    public static LoadResult Load(string path)
    {
        if (!File.Exists(path))
            return new LoadResult(LoadStatus.Missing, $"Soubor '{path}' neexistuje, začínáme s prázdným stavem.");

        string[] lines;
        try
        {
            lines = File.ReadAllLines(path, Encoding.UTF8);
        }
        catch (Exception ex)
        {
            return new LoadResult(LoadStatus.IoError, $"Soubor nejde přečíst: {ex.Message}");
        }

        try
        {
            var (players, tournament) = Parse(lines);
            return new LoadResult(LoadStatus.Ok, "Turnaj byl načten.", players, tournament);
        }
        catch (FormatException ex)
        {
            return new LoadResult(LoadStatus.Corrupted, "Soubor je poškozený: " + ex.Message);
        }
    }

    private static (PlayerRegistry, Tournament?) Parse(string[] lines)
    {
        if (lines.Length < 2) throw new FormatException("soubor je prázdný nebo useknutý.");

        var head = LineCodec.Split(lines[0]);
        if (head.Length != 2 || head[0] != Header) throw new FormatException("řádek 1: chybí hlavička.");
        if (head[1] != Version) throw new FormatException($"řádek 1: nepodporovaná verze '{head[1]}'.");

        var end = LineCodec.Split(lines[^1]);
        if (end.Length != 2 || end[0] != "END" ||
            !int.TryParse(end[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out int count) ||
            count != lines.Length - 2)
            throw new FormatException("chybí závěrečný záznam END nebo nesedí počet záznamů (soubor je useknutý).");

        var players = new PlayerRegistry();
        Tournament? t = null;

        for (int i = 1; i < lines.Length - 1; i++)
        {
            try
            {
                ParseRecord(LineCodec.Split(lines[i]), players, ref t);
            }
            catch (FormatException ex)
            {
                throw new FormatException($"řádek {i + 1}: {ex.Message}");
            }
        }

        if (t != null)
            foreach (var r in t.Rounds)
                if (r.IsClosed && r.Matches.Any(m => !m.HasResult))
                    throw new FormatException($"kolo {r.Number} je uzavřené, ale má zápas bez výsledku.");

        return (players, t);
    }

    private static void ParseRecord(string[] f, PlayerRegistry players, ref Tournament? t)
    {
        switch (f[0])
        {
            case "P":
            {
                Need(f, 3);
                var r = players.Add(f[2], f[1]);
                if (!r.Ok) throw new FormatException(r.Message);
                break;
            }
            case "T":
            {
                Need(f, 5);
                if (t != null) throw new FormatException("druhý záznam turnaje.");
                int rounds = ParseInt(f[3]);
                if (rounds < 1) throw new FormatException("počet kol musí být kladný.");
                t = new Tournament(f[1], f[2], rounds) { IsFinished = !ParseBool(f[4]) };
                break;
            }
            case "E":
            {
                Need(f, 2);
                var tt = RequireTournament(t);
                if (tt.Started) throw new FormatException("přihláška po začátku turnaje.");
                var p = RequirePlayer(players, f[1]);
                if (tt.IsEnrolled(p.Nickname)) throw new FormatException($"hráč '{p.Nickname}' je přihlášen dvakrát.");
                tt.Enrolled.Add(p.Nickname);
                break;
            }
            case "R":
            {
                Need(f, 3);
                var tt = RequireTournament(t);
                int no = ParseInt(f[1]);
                if (no != tt.Rounds.Count + 1) throw new FormatException($"kolo {no} je mimo pořadí nebo duplicitní.");
                if (no > tt.RoundCount) throw new FormatException("víc kol, než turnaj povoluje.");
                if (tt.Rounds.Count > 0 && !tt.Rounds[^1].IsClosed)
                    throw new FormatException("nové kolo, ale předchozí není uzavřené.");
                tt.Rounds.Add(new Round(no) { IsClosed = ParseBool(f[2]) });
                break;
            }
            case "M":
            {
                Need(f, 5);
                var tt = RequireTournament(t);
                var round = RequireRound(tt, f[1]);
                string a = RequireEnrolled(tt, players, f[2]);
                string b = RequireEnrolled(tt, players, f[3]);
                if (Nick.Eq(a, b)) throw new FormatException("hráč hraje sám proti sobě.");
                if (round.Involves(a) || round.Involves(b)) throw new FormatException("hráč je v kole víckrát.");
                round.Matches.Add(new Match(a, b) { Result = CodeToResult(f[4]) });
                break;
            }
            case "B":
            {
                Need(f, 3);
                var tt = RequireTournament(t);
                var round = RequireRound(tt, f[1]);
                string a = RequireEnrolled(tt, players, f[2]);
                if (round.ByeNick != null) throw new FormatException("v kole je víc volných losů.");
                if (round.Involves(a)) throw new FormatException("hráč je v kole víckrát.");
                round.ByeNick = a;
                break;
            }
            default:
                throw new FormatException($"neznámý typ záznamu '{f[0]}'.");
        }
    }

    // ---------- pomocné ----------

    private static void Need(string[] f, int n)
    {
        if (f.Length != n) throw new FormatException($"záznam '{f[0]}' má {f.Length} polí, očekáváno {n}.");
    }

    private static int ParseInt(string s)
    {
        if (!int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out int v))
            throw new FormatException($"'{s}' není číslo.");
        return v;
    }

    private static bool ParseBool(string s) => s switch
    {
        "1" => true,
        "0" => false,
        _ => throw new FormatException($"'{s}' není 0 ani 1.")
    };

    private static Tournament RequireTournament(Tournament? t) =>
        t ?? throw new FormatException("záznam je před záznamem turnaje.");

    private static Player RequirePlayer(PlayerRegistry players, string nick) =>
        players.Find(nick) ?? throw new FormatException($"neznámý hráč '{nick}'.");

    private static string RequireEnrolled(Tournament t, PlayerRegistry players, string nick)
    {
        var p = RequirePlayer(players, nick);
        if (!t.IsEnrolled(p.Nickname)) throw new FormatException($"hráč '{p.Nickname}' není přihlášen.");
        return p.Nickname;
    }

    private static Round RequireRound(Tournament t, string numberText)
    {
        int no = ParseInt(numberText);
        return t.Rounds.FirstOrDefault(r => r.Number == no)
               ?? throw new FormatException($"kolo {no} neexistuje.");
    }

    private static string ResultToCode(MatchResult r) => r switch
    {
        MatchResult.FirstWins => "1",
        MatchResult.SecondWins => "2",
        MatchResult.Draw => "D",
        _ => "0"
    };

    private static MatchResult CodeToResult(string c) => c switch
    {
        "0" => MatchResult.None,
        "1" => MatchResult.FirstWins,
        "2" => MatchResult.SecondWins,
        "D" => MatchResult.Draw,
        _ => throw new FormatException($"neznámý výsledek '{c}'.")
    };
}
