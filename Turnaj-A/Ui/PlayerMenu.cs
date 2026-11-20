using Turnaj.Logic;
using Turnaj.Model;

namespace Turnaj.Ui;

/// <summary>Menu jen pro hráče (položky 1 až 3 hlavního menu).</summary>
public class PlayerMenu
{
    private readonly PlayerService _service;

    public PlayerMenu(PlayerService service)
    {
        _service = service;
    }

    public void Run()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("1 Přidat hráče   2 Vypsat hráče   3 Odebrat hráče   0 Konec");
            int choice = InputReader.ReadInt("Volba", 0, 3);
            switch (choice)
            {
                case 0:
                    return;
                case 1:
                    string name = InputReader.ReadText("Jméno");
                    string nick = InputReader.ReadText("Přezdívka");
                    Show(_service.Add(name, nick));
                    break;
                case 2:
                    PrintPlayers();
                    break;
                case 3:
                    Show(_service.Remove(InputReader.ReadText("Přezdívka")));
                    break;
            }
        }
    }

    private static void Show(OperationResult r)
    {
        if (r.Ok) Console.WriteLine(r.Message.Length > 0 ? "OK: " + r.Message : "OK.");
        else Console.WriteLine("Chyba: " + r.Message);
    }

    private void PrintPlayers()
    {
        var players = _service.List();
        if (players.Count == 0)
        {
            Console.WriteLine("Zatím nejsou žádní hráči.");
            return;
        }
        Console.WriteLine($"{"Přezdívka",-15} Jméno");
        foreach (var p in players)
            Console.WriteLine($"{p.Nickname,-15} {p.Name}");
    }
}
