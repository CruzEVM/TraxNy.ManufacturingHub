namespace TraxNy.ManufacturingHub.Domain;

/// <summary>
/// Proceso del Lingote de Oro
/// </summary>
public class GoldIngotProcess : ManufacturingProcess
{
    public string Size { get; }
    public double Purity { get; }
    public string FurnaceStatus { get; }

    public GoldIngotProcess(IOutput output, string size, double purity, string furnaceStatus)
        : base(output)
    {
        Size = size;
        Purity = purity;
        FurnaceStatus = furnaceStatus;
    }

    protected override string GetProductName() => "Lingote de Oro";

    protected override void Prepare()
    {
        Write("Preparando horno para fundición de oro...");
    }

    protected override void Process()
    {
        Write("Iniciando fundición de oro...");
    }

    protected override void Finish()
    {
        Write("Fundición de oro completada.");
    }

    protected override void ReportStatus()
    {
        Write($"Estado del horno: {FurnaceStatus}, Pureza: {Purity}, Tamaño: {Size}");
    }
}

/// <summary>
/// Proceso del Diamante de Laboratorio
/// </summary>
public class DiamondProcess : ManufacturingProcess
{
    public string Stage { get; }

    public DiamondProcess(IOutput output, string stage)
        : base(output)
    {
        Stage = stage;
    }

    protected override string GetProductName() => "Diamante de Laboratorio";

    protected override void Prepare()
    {
        Write("Preparando sistema para crecimiento de diamante...");
    }

    protected override void Process()
    {
        Write("Crecimiento del diamante iniciado...");
    }

    protected override void Finish()
    {
        Write("Crecimiento del diamante completado.");
    }

    protected override void ReportStatus()
    {
        Write($"Etapa actual del diamante: {Stage}");
    }
}

/// <summary>
/// Proceso de Cadena de Plata/Oro
/// </summary>
public class ChainProcess : ManufacturingProcess
{
    public string Style { get; }
    public double Density { get; }

    public ChainProcess(IOutput output, string style, double density)
        : base(output)
    {
        Style = style;
        Density = density;
    }

    protected override string GetProductName() => "Cadena Forjada";

    protected override void Prepare()
    {
        Write($"Preparando herramientas para forjar cadena {Style}...");
    }

    protected override void Process()
    {
        Write("Forjando cadena...");
    }

    protected override void Finish()
    {
        Write("Cadena forjada correctamente.");
    }

    protected override void ReportStatus()
    {
        Write($"Probando resistencia. Densidad: {Density}");
    }
}
