using System.Text;
using Turnaj.Logic;
using Turnaj.Storage;
using Turnaj.Ui;

Console.OutputEncoding = Encoding.UTF8;

string path = args.Length > 0 ? args[0] : "turnaj.dat";
var service = new TournamentService();
bool loadFailed = false;

var load = TournamentFileStore.Load(path);
switch (load.Status)
{
    case LoadStatus.Ok:
        service.Replace(load.Players!, load.Tournament);
        Console.WriteLine(load.Message);
        break;
    case LoadStatus.Missing:
        Console.WriteLine(load.Message);
        break;
    default:
        loadFailed = true;
        Console.WriteLine(load.Message);
        if (load.Status == LoadStatus.Corrupted)
        {
            try
            {
                File.Copy(path, path + ".bad", true);
                Console.WriteLine($"Kopie souboru je uložena jako '{path}.bad'. Začínáme s prázdným stavem.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Zálohu se nepodařilo vytvořit: " + ex.Message);
            }
        }
        break;
}

try
{
    new ConsoleMenu(service, path, loadFailed).Run();
}
catch (EndOfInputException)
{
    Console.WriteLine();
    Console.WriteLine("Konec vstupu, program končí (neuložené změny se neukládají).");
}
