namespace Turnaj.Model;

// Při sloučení s celým projektem tento soubor smaž, Player je tam v Models.cs.
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
