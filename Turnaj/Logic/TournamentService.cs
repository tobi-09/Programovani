using Turnaj.Model;

namespace Turnaj.Logic;

/// <summary>Veškerá turnajová logika. Žádný Console, jen OperationResult.</summary>
public class TournamentService
{
    private const string NoTournament = "Není otevřený žádný turnaj.";

    public PlayerRegistry Players { get; private set; } = new();
    public Tournament? Current { get; private set; }

    private Tournament? Open => Current is { IsFinished: false } ? Current : null;

    public void Replace(PlayerRegistry players, Tournament? tournament)
    {
        Players = players;
        Current = tournament;
    }

    public Round? GetRound(int number) => Current?.Rounds.FirstOrDefault(r => r.Number == number);

    // ---------- Hráči ----------

    public OperationResult AddPlayer(string name, string nick) => Players.Add(name, nick);

    public OperationResult RemovePlayer(string nick)
    {
        var p = Players.Find(nick);
        if (p == null) return OperationResult.Fail($"Hráč s přezdívkou '{nick.Trim()}' neexistuje.");
        if (p.PlayedInPastTournament || HasPlayedMatch(p.Nickname))
            return OperationResult.Fail("Hráč má odehraný zápas, nejde odebrat (zkreslila by se tabulka).");
        if (Open != null && Open.IsEnrolled(p.Nickname))
            return OperationResult.Fail("Hráč je přihlášen do otevřeného turnaje. Nejdřív ho odhlaste.");
        if (Current?.IsEnrolled(p.Nickname) == true)
            return OperationResult.Fail("Hráč je na soupisce ukončeného turnaje, nejde ho odebrat.");
        Players.Remove(p.Nickname);
        return OperationResult.Success($"Hráč '{p.Nickname}' byl odebrán.");
    }

    private bool HasPlayedMatch(string nick) =>
        Current != null &&
        Current.Rounds.Any(r => r.Matches.Any(m => m.HasResult && m.Involves(nick)));

    // ---------- Turnaj ----------

    public OperationResult CreateTournament(string? name, string? discipline, int roundCount)
    {
        name = name?.Trim() ?? "";
        discipline = discipline?.Trim() ?? "";
        if (name.Length == 0) return OperationResult.Fail("Název turnaje nesmí být prázdný.");
        if (discipline.Length == 0) return OperationResult.Fail("Disciplína nesmí být prázdná.");
        if (roundCount < 1) return OperationResult.Fail("Počet kol musí být kladné číslo.");
        if (Open != null)
            return OperationResult.Fail("Už existuje otevřený turnaj. Nejdřív ho ukončete.");
        if (Current != null)
            foreach (var match in Current.Rounds.SelectMany(round => round.Matches).Where(match => match.HasResult))
            {
                Players.Find(match.First)!.PlayedInPastTournament = true;
                Players.Find(match.Second)!.PlayedInPastTournament = true;
            }
        Current = new Tournament(name, discipline, roundCount);
        return OperationResult.Success($"Turnaj '{name}' byl založen.");
    }

    public OperationResult Enroll(string? nick)
    {
        var t = Open;
        if (t == null) return OperationResult.Fail(NoTournament);
        if (t.Started) return OperationResult.Fail("Turnaj už začal, přihlášky nejdou měnit.");
        nick = nick?.Trim() ?? "";
        if (nick.Length == 0) return OperationResult.Fail("Přezdívka nesmí být prázdná.");
        var p = Players.Find(nick);
        if (p == null) return OperationResult.Fail($"Hráč '{nick}' neexistuje.");
        if (t.IsEnrolled(p.Nickname)) return OperationResult.Fail("Hráč už je přihlášen.");
        t.Enrolled.Add(p.Nickname);
        return OperationResult.Success($"Hráč '{p.Nickname}' byl přihlášen.");
    }

    public OperationResult Unenroll(string? nick)
    {
        var t = Open;
        if (t == null) return OperationResult.Fail(NoTournament);
        if (t.Started) return OperationResult.Fail("Turnaj už začal, přihlášky nejdou měnit.");
        nick = nick?.Trim() ?? "";
        if (nick.Length == 0) return OperationResult.Fail("Přezdívka nesmí být prázdná.");
        var p = Players.Find(nick);
        if (p == null || !t.IsEnrolled(p.Nickname))
            return OperationResult.Fail("Tento hráč není přihlášen.");
        t.Enrolled.RemoveAll(e => Nick.Eq(e, p.Nickname));
        return OperationResult.Success($"Hráč '{p.Nickname}' byl odhlášen.");
    }

    public OperationResult FinishTournament()
    {
        var t = Open;
        if (t == null) return OperationResult.Fail(NoTournament);
        if (t.Rounds.Count == 0) return OperationResult.Fail("Turnaj nemá žádné kolo.");
        if (!t.Rounds[^1].IsClosed) return OperationResult.Fail("Poslední kolo ještě není uzavřené.");
        t.IsFinished = true;
        return OperationResult.Success("Turnaj byl ukončen.");
    }

    // ---------- Kola ----------

    public OperationResult CreateRound()
    {
        var t = Open;
        if (t == null) return OperationResult.Fail(NoTournament);
        if (t.Enrolled.Count < 2) return OperationResult.Fail("Přihlaste aspoň 2 hráče.");
        if (t.Rounds.Count >= t.RoundCount)
            return OperationResult.Fail($"Turnaj má jen {t.RoundCount} kol, všechna už existují.");
        if (t.Rounds.Count > 0 && !t.Rounds[^1].IsClosed)
            return OperationResult.Fail("Předchozí kolo ještě není uzavřené.");
        var round = new Round(t.Rounds.Count + 1);
        t.Rounds.Add(round);
        return OperationResult.Success($"Kolo {round.Number} bylo vytvořeno.");
    }

    public OperationResult AddPair(string first, string second)
    {
        var t = Open;
        if (t == null) return OperationResult.Fail(NoTournament);
        var round = t.Rounds.LastOrDefault();
        if (round == null) return OperationResult.Fail("Nejdřív vytvořte kolo.");
        if (round.IsClosed) return OperationResult.Fail("Poslední kolo je uzavřené, vytvořte nové.");

        var (a, errA) = ResolveEnrolled(t, first);
        if (a == null) return OperationResult.Fail(errA!);
        var (b, errB) = ResolveEnrolled(t, second);
        if (b == null) return OperationResult.Fail(errB!);

        if (Nick.Eq(a, b)) return OperationResult.Fail("Hráč nemůže hrát sám proti sobě.");
        if (round.Involves(a)) return OperationResult.Fail($"Hráč '{a}' už v tomto kole je.");
        if (round.Involves(b)) return OperationResult.Fail($"Hráč '{b}' už v tomto kole je.");

        round.Matches.Add(new Match(a, b));
        return OperationResult.Success($"Zápas {a} vs {b} byl přidán do kola {round.Number}.");
    }

    public OperationResult SetBye(string nick)
    {
        var t = Open;
        if (t == null) return OperationResult.Fail(NoTournament);
        var round = t.Rounds.LastOrDefault();
        if (round == null) return OperationResult.Fail("Nejdřív vytvořte kolo.");
        if (round.IsClosed) return OperationResult.Fail("Poslední kolo je uzavřené, vytvořte nové.");
        if (t.Enrolled.Count % 2 == 0)
            return OperationResult.Fail("Volný los jde přidělit jen při lichém počtu přihlášených hráčů.");
        if (round.ByeNick != null) return OperationResult.Fail("Volný los už v tomto kole někdo má.");

        var (a, err) = ResolveEnrolled(t, nick);
        if (a == null) return OperationResult.Fail(err!);
        if (round.Involves(a)) return OperationResult.Fail($"Hráč '{a}' už v tomto kole je.");

        round.ByeNick = a;
        return OperationResult.Success($"Hráč '{a}' má v kole {round.Number} volný los.");
    }

    public OperationResult ResetRound()
    {
        var t = Open;
        if (t == null) return OperationResult.Fail(NoTournament);
        var round = t.Rounds.LastOrDefault();
        if (round == null) return OperationResult.Fail("Žádné kolo neexistuje.");
        if (round.IsClosed) return OperationResult.Fail("Uzavřené kolo nejde smazat.");
        round.Matches.Clear();
        round.ByeNick = null;
        return OperationResult.Success($"Dvojice v kole {round.Number} byly smazány.");
    }

    public OperationResult SetResult(int roundNumber, int matchNumber, MatchResult result)
    {
        var t = Open;
        if (t == null) return OperationResult.Fail(NoTournament);
        if (!Match.IsPlayedResult(result)) return OperationResult.Fail("Neplatný výsledek.");
        var round = GetRound(roundNumber);
        if (round == null) return OperationResult.Fail($"Kolo {roundNumber} neexistuje.");
        if (round.IsClosed) return OperationResult.Fail("Kolo je uzavřené, výsledek jde měnit jen před uzavřením.");
        if (matchNumber < 1 || matchNumber > round.Matches.Count)
            return OperationResult.Fail($"Zápas {matchNumber} v kole {roundNumber} neexistuje.");
        round.Matches[matchNumber - 1].Result = result;
        return OperationResult.Success("Výsledek byl zapsán.");
    }

    public OperationResult CloseRound(int roundNumber)
    {
        var t = Open;
        if (t == null) return OperationResult.Fail(NoTournament);
        var round = GetRound(roundNumber);
        if (round == null) return OperationResult.Fail($"Kolo {roundNumber} neexistuje.");
        if (round.IsClosed) return OperationResult.Fail("Kolo už je uzavřené.");
        if (round.Matches.Count == 0) return OperationResult.Fail("Kolo nemá žádný zápas.");

        int missing = round.Matches.Count(m => !m.HasResult);
        if (missing > 0) return OperationResult.Fail($"Chybí výsledek u {missing} zápasů.");

        var unplaced = t.Enrolled.Where(n => !round.Involves(n)).ToList();
        if (unplaced.Count > 0)
            return OperationResult.Fail("V kole chybí hráči: " + string.Join(", ", unplaced) +
                                        " (zařaďte je do dvojice nebo jim přidělte volný los).");

        round.IsClosed = true;
        return OperationResult.Success($"Kolo {round.Number} bylo uzavřeno.");
    }

    // ---------- Tabulka ----------

    public List<StandingRow> GetStandings() =>
        Current == null ? new List<StandingRow>() : StandingsCalculator.Compute(Current);

    private (string? Nick, string? Error) ResolveEnrolled(Tournament t, string input)
    {
        var p = Players.Find(input);
        if (p == null) return (null, $"Hráč '{input.Trim()}' neexistuje.");
        if (!t.IsEnrolled(p.Nickname)) return (null, $"Hráč '{p.Nickname}' není přihlášen do turnaje.");
        return (p.Nickname, null);
    }
}
