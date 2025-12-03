namespace TraxNy.ManufacturingHub.Dtos;

/// <summary>
/// Resumen que se muestra en el Dashboard principal.
/// </summary>
public record DashboardSummaryDto(
    double GoldPurity,
    double ReactorTemp,
    double DiamondGrowthRate,
    int ActiveUsers,
    double InventoryValue
);
