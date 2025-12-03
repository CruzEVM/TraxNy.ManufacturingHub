namespace TraxNy.ManufacturingHub.Dtos;

/// <summary>
/// Detalle de producción para un producto (oro, diamante, etc.).
/// </summary>
public record ProductionDetailDto(
    string ProductName,
    string BatchId,
    double Metric1,
    double Metric2,
    string Status,
    IEnumerable<string> Events
);
