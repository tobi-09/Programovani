using Turnaj.Logic;
using Turnaj.Model;
using Turnaj.Storage;

namespace Turnaj.Ui;

public class ConsoleMenu
{
    private readonly TournamentService _service;
    private readonly string _path;
    private bool _loadFailed;

    public ConsoleMenu(TournamentService service, string path, bool loadFailed)
    {
        _service = service;
        _path = path;
        _loadFailed = loadFailed;
    }

    public void Run()
    {
        while (true)
        {
            ConsoleView.PrintStatus(_service);
            PrintMenu();
            int choice = InputReader.ReadInt("Volba", 0, 16);
            if (choice == 0)
            {
                if (InputReader.Confirm("Uložit před ukončením?")) Save();
                return;
            }
            Execute(choice);
        }
    }

    private static void PrintMenu()
    {
        Console.WriteLine(" 1 Přidat hráče          2 Vypsat hráče        3 Odebrat hráče");
        Console.WriteLine(" 4 Založit turnaj        5 Přihlásit hráče     6 Odhlásit hráče");
        Console.WriteLine(" 7 Nové kolo             8 Přidat dvojici      9 Přidělit volný los");
        Console.WriteLine("10 Zadat/přepsat výsledek 11 Uzavřít kolo     12 Vypsat kolo");
        Console.WriteLine("13 Tabulka              14 Smazat dvojice kola 15 Ukončit turnaj");
        Console.WriteLine("16 Uložit                0 Konec");
    }

    private void Execute(int choice)
    {
        switch (choice)
        {
            case 1:
                ConsoleView.Show(_service.AddPlayer(
                    InputReader.ReadText("Jméno"), InputReader.ReadText("Přezdívka")));
                break;
            case 2: ConsoleView.PrintPlayers(_service); break;
            case 3: ConsoleView.Show(_service.RemovePlayer(InputReader.ReadText("Přezdívka"))); break;
            case 4:
                string name = InputReader.ReadText("Název turnaje");
                string discipline = InputReader.ReadText("Disciplína");
                int rounds = InputReader.ReadInt("Počet kol", 1, 100);
                ConsoleView.Show(_service.CreateTournament(name, discipline, rounds));
                break;
            case 5: ConsoleView.Show(_service.Enroll(InputReader.ReadText("Přezdívka"))); break;
            case 6: ConsoleView.Show(_service.Unenroll(InputReader.ReadText("Přezdívka"))); break;
            case 7: ConsoleView.Show(_service.CreateRound()); break;
            case 8:
                string first = InputReader.ReadText("Přezdívka 1. hráče");
                string second = InputReader.ReadText("Přezdívka 2. hráče");
                ConsoleView.Show(_service.AddPair(first, second));
                break;
            case 9: ConsoleView.Show(_service.SetBye(InputReader.ReadText("Přezdívka"))); break;
            case 10: SetResult(); break;
            case 11:
            {
                int? rn = AskRound();
                if (rn != null) ConsoleView.Show(_service.CloseRound(rn.Value));
                break;
            }
            case 12:
            {
                int? rn = AskRound();
                if (rn != null) ConsoleView.PrintRound(_service, _service.GetRound(rn.Value)!);
                break;
            }
            case 13: ConsoleView.PrintStandings(_service); break;
            case 14:
                if (InputReader.Confirm("Smazat všechny dvojice rozpracovaného kola?"))
                    ConsoleView.Show(_service.ResetRound());
                break;
            case 15:
                if (InputReader.Confirm("Opravdu ukončit turnaj?"))
                    ConsoleView.Show(_service.FinishTournament());
                break;
            case 16: Save(); break;
        }
    }

    private int? AskRound()
    {
        var t = _service.Current;
        if (t == null || t.Rounds.Count == 0)
        {
            Console.WriteLine("Zatím neexistuje žádné kolo.");
            return null;
        }
        return t.Rounds.Count == 1 ? 1 : InputReader.ReadInt("Číslo kola", 1, t.Rounds.Count);
    }

    private void SetResult()
    {
        int? rn = AskRound();
        if (rn == null) return;
        var round = _service.GetRound(rn.Value)!;
        if (round.Matches.Count == 0)
        {
            Console.WriteLine("Kolo nemá žádné zápasy.");
            return;
        }
        ConsoleView.PrintRound(_service, round);
        int mn = InputReader.ReadInt("Číslo zápasu", 1, round.Matches.Count);
        int code = InputReader.ReadInt("Výsledek (1 = výhra prvního, 2 = výhra druhého, 3 = remíza)", 1, 3);
        var result = code switch
        {
            1 => MatchResult.FirstWins,
            2 => MatchResult.SecondWins,
            _ => MatchResult.Draw
        };
        ConsoleView.Show(_service.SetResult(rn.Value, mn, result));
    }

    private void Save()
    {
        if (_loadFailed && !InputReader.Confirm(
                "Při načtení byl soubor poškozený (záloha je v .bad). Opravdu ho přepsat?"))
            return;
        var r = TournamentFileStore.Save(_path, _service.Players, _service.Current);
        ConsoleView.Show(r);
        if (r.Ok) _loadFailed = false;
    }
}
