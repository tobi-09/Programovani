using Turnaj.Model;

namespace Turnaj.Logic;

public class PlayerRegistry
{
    private readonly Dictionary<string, Player> _players = new(StringComparer.OrdinalIgnoreCase);

    public int Count => _players.Count;

    public IEnumerable<Player> All =>
        _players.Values.OrderBy(p => p.Nickname, StringComparer.OrdinalIgnoreCase);

    public Player? Find(string? nick) =>
        nick != null && _players.TryGetValue(nick.Trim(), out var p) ? p : null;

    public OperationResult Add(string name, string nick)
    {
        name = name.Trim();
        nick = nick.Trim();
        if (name.Length == 0) return OperationResult.Fail("Jméno nesmí být prázdné.");
        if (nick.Length == 0) return OperationResult.Fail("Přezdívka nesmí být prázdná.");
        if (_players.ContainsKey(nick))
            return OperationResult.Fail($"Přezdívka '{nick}' už existuje.");
        _players[nick] = new Player(name, nick);
        return OperationResult.Success($"Hráč '{nick}' byl přidán.");
    }

    public bool Remove(string nick) => _players.Remove(nick.Trim());
}
