namespace TraxNy.ManufacturingHub.Domain;

/// <summary>
/// Patrón Template Method
/// Define el flujo estándar de producción.
/// </summary>
public abstract class ManufacturingProcess : IManufacturable
{
    private readonly IOutput _output;
    private readonly List<IProductionObserver> _observers = new();

    protected ManufacturingProcess(IOutput output)
    {
        _output = output;
    }

    public void AttachObserver(IProductionObserver observer)
    {
        _observers.Add(observer);
    }

    public void Fabricate()
    {
        NotifyStarted(GetProductName());

        Prepare();
        Process();
        Finish();
        ReportStatus();

        NotifyFinished(GetProductName());
    }

    protected abstract string GetProductName();
    protected abstract void Prepare();
    protected abstract void Process();
    protected abstract void Finish();

    protected virtual void ReportStatus()
    {
        _output.WriteLine($"[Reporte] Producción completada: {GetProductName()}");
    }

    protected void Write(string msg)
    {
        _output.WriteLine(msg);
    }

    private void NotifyStarted(string productName)
    {
        foreach (var obs in _observers)
            obs.OnProductionStarted(productName);
    }

    private void NotifyFinished(string productName)
    {
        foreach (var obs in _observers)
            obs.OnProductionFinished(productName);
    }
}
