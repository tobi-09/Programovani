namespace Turnaj.Storage;

/// <summary>T17: chybějící soubor není chyba, program startuje s prázdným stavem.</summary>
public static class StartupCheck
{
    public static bool FileMissing(string path) => !File.Exists(path);

    public static string Describe(string path) =>
        FileMissing(path)
            ? $"Soubor '{path}' neexistuje, začínáme s prázdným stavem."
            : $"Soubor '{path}' nalezen (jeho načtení řeší T16).";
}
