using Turnaj.Model;

namespace Turnaj.Logic;

/// <summary>T1, T2, T3: přidání, výpis a odebrání hráče. Žádný Console.</summary>
public class PlayerService
{
    private readonly PlayerRegistry _players;
    private readonly IPlayerUsage _usage;

    public PlayerService(PlayerRegistry players, IPlayerUsage? usage = null)
    {
        _players = players;
        _usage = usage ?? new NoPlayerUsage();
    }

    public OperationResult Add(string name, string nick) => _players.Add(name, nick);

    public List<Player> List() => _players.All.ToList();

    public OperationResult Remove(string nick)
    {
        var p = _players.Find(nick);
        if (p == null)
            return OperationResult.Fail($"Hráč s přezdívkou '{nick.Trim()}' neexistuje.");
        if (_usage.HasPlayedMatch(p.Nickname))
            return OperationResult.Fail("Hráč má odehraný zápas, nejde odebrat (zkreslila by se tabulka).");
        if (_usage.IsEnrolledInOpenTournament(p.Nickname))
            return OperationResult.Fail("Hráč je přihlášen do otevřeného turnaje. Nejdřív ho odhlaste.");
        _players.Remove(p.Nickname);
        return OperationResult.Success($"Hráč '{p.Nickname}' byl odebrán.");
    }
}
