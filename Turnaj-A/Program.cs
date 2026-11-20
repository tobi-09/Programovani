using System.Text;
using Turnaj.Logic;
using Turnaj.Storage;
using Turnaj.Ui;

Console.OutputEncoding = Encoding.UTF8;

string path = args.Length > 0 ? args[0] : "turnaj.dat";
Console.WriteLine(StartupCheck.Describe(path));

var service = new PlayerService(new PlayerRegistry());
try
{
    new PlayerMenu(service).Run();
}
catch (EndOfInputException)
{
    Console.WriteLine();
    Console.WriteLine("Konec vstupu, program končí.");
}
