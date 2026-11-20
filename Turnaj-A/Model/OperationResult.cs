namespace Turnaj.Model;

/// <summary>Výsledek operace logiky. Logika nic nevypisuje, jen vrací tento objekt.</summary>
public class OperationResult
{
    public bool Ok { get; }
    public string Message { get; }

    private OperationResult(bool ok, string message)
    {
        Ok = ok;
        Message = message;
    }

    public static OperationResult Success(string message = "") => new(true, message);
    public static OperationResult Fail(string message) => new(false, message);
}
