namespace TraxNy.ManufacturingHub.Domain;

/// <summary>
/// Patrón Observer
/// Notifica eventos del proceso de manufactura.
/// </summary>
public interface IProductionObserver
{
    void OnProductionStarted(string productName);
    void OnProductionFinished(string productName);
}

public class ConsoleProductionObserver : IProductionObserver
{
    public void OnProductionStarted(string productName)
    {
        Console.WriteLine($"[Observer] Producción iniciada: {productName}");
    }

    public void OnProductionFinished(string productName)
    {
        Console.WriteLine($"[Observer] Producción finalizada: {productName}");
    }
}
