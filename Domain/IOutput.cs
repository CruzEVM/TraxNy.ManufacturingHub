namespace TraxNy.ManufacturingHub.Domain;

/// <summary>
/// Patrón Adapter / Strategy
/// Abstrae la salida para que no dependamos de Console.WriteLine directamente.
/// </summary>
public interface IOutput
{
    void WriteLine(string message);
}

public class ConsoleOutput : IOutput
{
    public void WriteLine(string message)
    {
        Console.WriteLine(message);
    }
}
