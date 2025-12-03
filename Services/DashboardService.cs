using TraxNy.ManufacturingHub.Domain;
using TraxNy.ManufacturingHub.Dtos;

namespace TraxNy.ManufacturingHub.Services;

/// <summary>
/// Servicio que genera datos de ejemplo para el Dashboard
/// y demás pantallas del Manufacturing Control Hub.
/// </summary>
public class DashboardService
{
    private readonly IManufacturingProcessFactory _factory;

    public DashboardService(IManufacturingProcessFactory factory)
    {
        _factory = factory;
    }

    public DashboardSummaryDto GetDashboard()
    {
        // Datos simulados
        return new DashboardSummaryDto(
            GoldPurity: 99.98,
            ReactorTemp: 1064,
            DiamondGrowthRate: 25.3,
            ActiveUsers: 118,
            InventoryValue: 4_150_230
        );
    }

    public ProductionDetailDto GetGoldDetail()
    {
        // Aquí podrías incluso disparar el proceso real:
        var goldProcess = _factory.Create(ProductType.GoldIngot);
        goldProcess.Fabricate(); // ejecuta el Template Method

        var events = new List<string>
        {
            "2025-12-02 10:01 Furnace pre-heated",
            "2025-12-02 10:15 Metal melted",
            "2025-12-02 10:45 Cooling curve stable"
        };

        return new ProductionDetailDto(
            "Lingote de Oro #8795-A",
            "BATCH-8795A",
            Metric1: 999.9,   // pureza
            Metric2: 1000.5,  // peso
            Status: "Cooling",
            Events: events
        );
    }

    public ProductionDetailDto GetDiamondDetail()
    {
        var diamondProcess = _factory.Create(ProductType.Diamond);
        diamondProcess.Fabricate();

        var events = new List<string>
        {
            "2025-12-02 09:10 Growth stage I",
            "2025-12-02 10:20 Growth stage II",
            "2025-12-02 11:05 Crystal inspection"
        };

        return new ProductionDetailDto(
            "Diamante ELD-23C",
            "BATCH-ELD23C",
            Metric1: 5.8,     // presión
            Metric2: 1350,    // temperatura reactor
            Status: "In Reactor",
            Events: events
        );
    }

    public IEnumerable<InventoryItemDto> GetInventory()
        => new List<InventoryItemDto>
        {
            new("G-ING-001", "Gold Ingot 1kg",      "OK",       5, 250_000),
            new("D-LAB-008", "Lab Diamond 2ct",     "On Hold",  3, 150_000),
            new("C-SLV-040", "Silver Chain",        "Low Stock",12, 12_000),
            new("G-ING-002", "Gold Ingot 500g",     "OK",      10, 180_000)
        };

    public IEnumerable<UserDto> GetUsers()
        => new List<UserDto>
        {
            new("Alejandro Vargas", "Engineer",   true,  DateTime.Now.AddMinutes(-15)),
            new("Sofía Reyes",      "Operator",   true,  DateTime.Now.AddHours(-2)),
            new("Marco Díaz",       "Supervisor", false, DateTime.Now.AddDays(-1)),
            new("Elena Montoya",    "Operator",   true,  DateTime.Now.AddMinutes(-5))
        };
}
