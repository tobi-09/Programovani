using System.Text;

namespace Turnaj.Storage;

/// <summary>Escapování a dělení polí řádkového formátu. Oddělovač polí je '|'.</summary>
public static class LineCodec
{
    public static string Escape(string s)
    {
        var sb = new StringBuilder();
        foreach (char c in s)
        {
            switch (c)
            {
                case '\\': sb.Append("\\\\"); break;
                case '|': sb.Append("\\p"); break;
                case '\n': sb.Append("\\n"); break;
                case '\r': sb.Append("\\r"); break;
                default: sb.Append(c); break;
            }
        }
        return sb.ToString();
    }

    public static string Unescape(string s)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < s.Length; i++)
        {
            char c = s[i];
            if (c != '\\')
            {
                sb.Append(c);
                continue;
            }
            if (i + 1 >= s.Length) throw new FormatException("neukončená escape sekvence");
            i++;
            switch (s[i])
            {
                case '\\': sb.Append('\\'); break;
                case 'p': sb.Append('|'); break;
                case 'n': sb.Append('\n'); break;
                case 'r': sb.Append('\r'); break;
                default: throw new FormatException($"neznámá escape sekvence '\\{s[i]}'");
            }
        }
        return sb.ToString();
    }

    public static string Join(params string[] fields) =>
        string.Join("|", fields.Select(Escape));

    /// <summary>Escapovaný text neobsahuje syrový '|', takže stačí Split.</summary>
    public static string[] Split(string line) =>
        line.Split('|').Select(Unescape).ToArray();
}
