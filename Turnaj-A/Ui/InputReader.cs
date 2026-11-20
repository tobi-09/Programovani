namespace Turnaj.Ui;

/// <summary>Vstup skončil (Ctrl+D / Ctrl+Z nebo konec přesměrovaného souboru).</summary>
public class EndOfInputException : Exception { }

/// <summary>T19: jediné místo, které čte z klávesnice. Čtení se opakuje, dokud není vstup platný.</summary>
public static class InputReader
{
    private static string ReadRaw()
    {
        string? s = Console.ReadLine();
        if (s == null) throw new EndOfInputException();
        return s.Trim();
    }

    public static string ReadText(string prompt)
    {
        while (true)
        {
            Console.Write(prompt + ": ");
            string s = ReadRaw();
            if (s.Length > 0) return s;
            Console.WriteLine("Vstup nesmí být prázdný.");
        }
    }

    public static int ReadInt(string prompt, int min, int max)
    {
        while (true)
        {
            Console.Write($"{prompt} [{min}-{max}]: ");
            string s = ReadRaw();
            if (s.Length == 0)
            {
                Console.WriteLine("Vstup nesmí být prázdný.");
                continue;
            }
            if (!int.TryParse(s, out int value))
            {
                Console.WriteLine("Zadejte celé číslo.");
                continue;
            }
            if (value < min || value > max)
            {
                Console.WriteLine($"Číslo musí být v rozsahu {min} až {max}.");
                continue;
            }
            return value;
        }
    }

    public static bool Confirm(string prompt)
    {
        while (true)
        {
            Console.Write(prompt + " (a/n): ");
            string s = ReadRaw().ToLowerInvariant();
            if (s == "a" || s == "ano") return true;
            if (s == "n" || s == "ne") return false;
            Console.WriteLine("Odpovězte 'a' nebo 'n'.");
        }
    }
}
