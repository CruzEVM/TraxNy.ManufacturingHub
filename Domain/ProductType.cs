namespace TraxNy.ManufacturingHub.Domain;

public enum ProductType
{
    GoldIngot,
    Diamond,
    Chain
}

/// <summary>
/// Patrón Factory Method
/// Crea procesos de manufactura según el tipo de producto.
/// </summary>
public interface IManufacturingProcessFactory
{
    IManufacturable Create(ProductType type);
}

public class ManufacturingProcessFactory : IManufacturingProcessFactory
{
    private readonly IOutput _output;

    public ManufacturingProcessFactory(IOutput output)
    {
        _output = output;
    }

    public IManufacturable Create(ProductType type)
        => type switch
        {
            ProductType.GoldIngot => new GoldIngotProcess(_output, "1kg", 0.999, "Estable"),
            ProductType.Diamond => new DiamondProcess(_output, "Etapa II"),
            ProductType.Chain => new ChainProcess(_output, "Cuban", 1.45),
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
}
