namespace TraxNy.ManufacturingHub.Dtos;

/// <summary>
/// Elemento del inventario mostrado en la pantalla de Gestión de Inventario.
/// </summary>
public record InventoryItemDto(
    string Code,
    string Name,
    string Status,
    int Quantity,
    double Value
);
