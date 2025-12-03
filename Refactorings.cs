// Refactorings.cs
// Proyecto: Manufacturing Control Hub v7.1
// Autor: Cruz Eduardo Valadez Melendez
// Descripción:
//   Archivo que muestra el código original (ANTES) y la versión
//   refactorizada (DESPUÉS) aplicando los patrones solicitados.
//   Los fragmentos "ANTES" están comentados para que el archivo compile.

// =======================================================
//  CÓDIGO ORIGINAL (ANTES)  - SOLO REFERENCIA / NO COMPILA
// =======================================================

/*
public class GoldIngot {
    public string Size;
    public double Purity;
    public string FurnaceStatus;

    public void StartMelting() {
        Console.WriteLine("Gold melting started...");
    }

    public void StopMelting() {
        Console.WriteLine("Melting stopped.");
    }

    public void ReportStatus() {
        Console.WriteLine("Furnace: " + FurnaceStatus + ", Purity: " + Purity);
    }
}

public class DiamondLab {
    public string Stage;
    public void Grow() {
        Console.WriteLine("Diamond growth started...");
    }

    public void Analyze() {
        Console.WriteLine("Analyzing diamond at stage: " + Stage);
    }
}

public class Chain {
    public string Style;
    public double Density;

    public void Forge() {
        Console.WriteLine("Forging " + Style + " chain...");
    }

    public void TestResistance() {
        Console.WriteLine("Testing resistance of chain with density: " + Density);
    }
}

public class ProductionManager {
    public void Produce(string itemType) {
        if (itemType == "gold") {
            var g = new GoldIngot();
            g.StartMelting();
            g.StopMelting();
        } else if (itemType == "diamond") {
            var d = new DiamondLab();
            d.Grow();
            d.Analyze();
        } else if (itemType == "chain") {
            var c = new Chain();
            c.Forge();
            c.TestResistance();
        } else {
            Console.WriteLine("Unknown product type.");
        }
    }
}
*/

using System;
using System.Collections.Generic;

namespace ManufacturingControlHub.Refactorings
{
    // =======================================================
    //  INFRAESTRUCTURA COMÚN USADA EN VARIOS PROBLEMAS
    // =======================================================

    // Tipo de producto soportado en la planta
    public enum ProductType
    {
        GoldIngot,
        Diamond,
        Chain,
        Bracelet // agregado en el problema 10
    }

    // Patrón Adapter / Strategy para salida (problema 9)
    public interface IOutput
    {
        void WriteLine(string message);
    }

    public class ConsoleOutput : IOutput
    {
        public void WriteLine(string message) => Console.WriteLine(message);
    }

    // Implementación para pruebas (por ejemplo en tests unitarios)
    public class InMemoryOutput : IOutput
    {
        private readonly List<string> _messages = new();
        public void WriteLine(string message) => _messages.Add(message);
        public IReadOnlyList<string> Messages => _messages;
    }

    // Interfaz común para cualquier operación de producción (problema 6)
    public interface IProductOperation
    {
        void ExecuteProcess();
    }

    // Template Method (problema 2) para estandarizar flujo de producción
    public abstract class ProductOperationBase : IProductOperation
    {
        public void ExecuteProcess()
        {
            Prepare();
            ProcessCore();
            Validate();
        }

        protected abstract void Prepare();
        protected abstract void ProcessCore();
        protected abstract void Validate();
    }

    // =======================================================
    //  PROBLEMA 1: Atributos públicos en GoldIngot
    //  SOLUCIÓN: Encapsulación con propiedades
    // =======================================================

    public class GoldIngot
    {
        // Antes: campos públicos -> ahora propiedades
        public string Size { get; private set; }
        public double Purity { get; private set; }
        public string FurnaceStatus { get; private set; }

        private readonly IOutput _output;

        public GoldIngot(string size, double purity, string furnaceStatus, IOutput output)
        {
            Size = size;
            Purity = purity;
            FurnaceStatus = furnaceStatus;
            _output = output;
        }

        public void UpdatePurity(double newPurity)
        {
            Purity = newPurity;
        }

        public void UpdateFurnaceStatus(string status)
        {
            FurnaceStatus = status;
        }

        public void ReportStatus()
        {
            _output.WriteLine($"[GoldIngot] Furnace: {FurnaceStatus}, Purity: {Purity}");
        }
    }

    // =======================================================
    //  PROBLEMA 2: Método de proceso largo/confuso
    //  SOLUCIÓN: Template Method en ProductOperationBase
    // =======================================================

    // Implementación concreta para lingote usando Template Method
    public class GoldIngotOperation : ProductOperationBase, IProductOperation
    {
        private readonly IOutput _output;

        public GoldIngotOperation(IOutput output)
        {
            _output = output;
        }

        protected override void Prepare()
        {
            _output.WriteLine("Preparando horno para lingote de oro (limpieza, calibración)...");
        }

        protected override void ProcessCore()
        {
            _output.WriteLine("Gold melting started...");
            _output.WriteLine("Melting in progress...");
        }

        protected override void Validate()
        {
            _output.WriteLine("Verificando pureza final del lingote...");
        }
    }

    // =======================================================
    //  PROBLEMA 3: Clase mezcla responsabilidades (inventario + facturación)
    //  SOLUCIÓN: Separar en InventoryService y BillingService (SRP)
    // =======================================================

    // Versión simplificada sólo para mostrar SRP
    public class InventoryService
    {
        public void ReserveStock(string productCode, int quantity)
        {
            Console.WriteLine($"[Inventory] Reservando {quantity} unidades de {productCode}.");
        }

        public void ReleaseStock(string productCode, int quantity)
        {
            Console.WriteLine($"[Inventory] Liberando {quantity} unidades de {productCode}.");
        }
    }

    public class BillingService
    {
        public void GenerateInvoice(string productCode, int quantity, decimal unitPrice)
        {
            decimal total = quantity * unitPrice;
            Console.WriteLine($"[Billing] Factura para {quantity} x {productCode} = {total:C2}");
        }
    }

    // =======================================================
    //  PROBLEMA 4: Lógica rígida de promociones (if/else por marca)
    //  SOLUCIÓN: Strategy para cálculo de descuento
    // =======================================================

    public interface IDiscountStrategy
    {
        decimal ApplyDiscount(decimal basePrice);
    }

    public class NoDiscountStrategy : IDiscountStrategy
    {
        public decimal ApplyDiscount(decimal basePrice) => basePrice;
    }

    public class GoldDiscountStrategy : IDiscountStrategy
    {
        public decimal ApplyDiscount(decimal basePrice) => basePrice * 0.90m; // 10% menos
    }

    public class DiamondDiscountStrategy : IDiscountStrategy
    {
        public decimal ApplyDiscount(decimal basePrice) => basePrice * 0.85m; // 15% menos
    }

    public class PromotionContext
    {
        private readonly IDiscountStrategy _strategy;

        public PromotionContext(IDiscountStrategy strategy)
        {
            _strategy = strategy;
        }

        public decimal CalculatePrice(decimal basePrice)
        {
            return _strategy.ApplyDiscount(basePrice);
        }
    }

    // =======================================================
    //  PROBLEMA 5: ProductionManager instancia directamente los productos
    //  SOLUCIÓN: Factory Method / Simple Factory
    // =======================================================

    public interface IProductFactory
    {
        IProductOperation CreateProduct(ProductType type);
    }

    public class DefaultProductFactory : IProductFactory
    {
        private readonly IOutput _output;

        public DefaultProductFactory(IOutput output)
        {
            _output = output;
        }

        public IProductOperation CreateProduct(ProductType type)
        {
            return type switch
            {
                ProductType.GoldIngot => new GoldIngotOperation(_output),
                ProductType.Diamond => new DiamondOperation(_output),
                ProductType.Chain => new ChainOperation(_output),
                ProductType.Bracelet => new BraceletOperation(_output),
                _ => throw new ArgumentOutOfRangeException(nameof(type))
            };
        }
    }

    public class ProductionManager
    {
        private readonly IProductFactory _factory;

        public ProductionManager(IProductFactory factory)
        {
            _factory = factory;
        }

        public void Produce(ProductType type)
        {
            IProductOperation productOperation = _factory.CreateProduct(type);
            productOperation.ExecuteProcess();
        }
    }

    // =======================================================
    //  PROBLEMA 6: No existe interfaz común entre productos
    //  SOLUCIÓN: IProductOperation + clases concretas
    //  (GoldIngotOperation ya implementada arriba)
    // =======================================================

    public class DiamondOperation : ProductOperationBase, IProductOperation
    {
        private readonly IOutput _output;

        public DiamondOperation(IOutput output)
        {
            _output = output;
        }

        protected override void Prepare()
        {
            _output.WriteLine("Calibrando reactor de crecimiento de diamante...");
        }

        protected override void ProcessCore()
        {
            _output.WriteLine("Diamond growth started...");
        }

        protected override void Validate()
        {
            _output.WriteLine("Analizando estructura cristalina del diamante...");
        }
    }

    public class ChainOperation : ProductOperationBase, IProductOperation
    {
        private readonly IOutput _output;

        public ChainOperation(IOutput output)
        {
            _output = output;
        }

        protected override void Prepare()
        {
            _output.WriteLine("Preparando forja de cadena...");
        }

        protected override void ProcessCore()
        {
            _output.WriteLine("Forging chain...");
            _output.WriteLine("Testing resistance of chain...");
        }

        protected override void Validate()
        {
            _output.WriteLine("Pruebas de resistencia completadas para la cadena.");
        }
    }

    // =======================================================
    //  PROBLEMA 7: Llamadas directas a métodos de máquina
    //  SOLUCIÓN: Command
    // =======================================================

    // Receptor: horno de oro
    public class GoldFurnace
    {
        private readonly IOutput _output;

        public GoldFurnace(IOutput output)
        {
            _output = output;
        }

        public void StartMelting() => _output.WriteLine("Gold furnace started.");
        public void StopMelting() => _output.WriteLine("Gold furnace stopped.");
    }

    // Interfaz Command
    public interface IProductionCommand
    {
        void Execute();
    }

    public class StartGoldMeltingCommand : IProductionCommand
    {
        private readonly GoldFurnace _furnace;

        public StartGoldMeltingCommand(GoldFurnace furnace)
        {
            _furnace = furnace;
        }

        public void Execute()
        {
            _furnace.StartMelting();
        }
    }

    public class StopGoldMeltingCommand : IProductionCommand
    {
        private readonly GoldFurnace _furnace;

        public StopGoldMeltingCommand(GoldFurnace furnace)
        {
            _furnace = furnace;
        }

        public void Execute()
        {
            _furnace.StopMelting();
        }
    }

    // Invoker
    public class ProductionConsole
    {
        public void RunCommand(IProductionCommand command)
        {
            command.Execute();
        }
    }

    // =======================================================
    //  PROBLEMA 8: No hay mecanismo para registrar estado y eventos
    //  SOLUCIÓN: Observer
    // =======================================================

    public enum ProductionEventType
    {
        BatchStarted,
        BatchCompleted,
        Warning,
        Error
    }

    public class ProductionEvent
    {
        public ProductionEventType Type { get; }
        public string ProductName { get; }
        public string Message { get; }

        public ProductionEvent(ProductionEventType type, string productName, string message)
        {
            Type = type;
            ProductName = productName;
            Message = message;
        }
    }

    public interface IProductionObserver
    {
        void OnEvent(ProductionEvent productionEvent);
    }

    public class ProductionEventBus
    {
        private readonly List<IProductionObserver> _observers = new();

        public void Attach(IProductionObserver observer) => _observers.Add(observer);
        public void Detach(IProductionObserver observer) => _observers.Remove(observer);

        public void Notify(ProductionEvent productionEvent)
        {
            foreach (var observer in _observers)
            {
                observer.OnEvent(productionEvent);
            }
        }
    }

    public class ConsoleProductionObserver : IProductionObserver
    {
        public void OnEvent(ProductionEvent productionEvent)
        {
            Console.WriteLine(
                $"[{productionEvent.Type}] {productionEvent.ProductName}: {productionEvent.Message}");
        }
    }

    // Ejemplo de operación de lingote que notifica eventos
    public class GoldIngotOperationWithEvents : ProductOperationBase
    {
        private readonly ProductionEventBus _eventBus;

        public GoldIngotOperationWithEvents(ProductionEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        protected override void Prepare()
        {
            _eventBus.Notify(new ProductionEvent(
                ProductionEventType.BatchStarted,
                "GoldIngot",
                "Preparando horno para nuevo lote de lingotes."
            ));
        }

        protected override void ProcessCore()
        {
            _eventBus.Notify(new ProductionEvent(
                ProductionEventType.Warning,
                "GoldIngot",
                "Temperatura cercana al límite superior, monitoreando..."
            ));
        }

        protected override void Validate()
        {
            _eventBus.Notify(new ProductionEvent(
                ProductionEventType.BatchCompleted,
                "GoldIngot",
                "Lote de lingotes completado sin errores."
            ));
        }
    }

    // =======================================================
    //  PROBLEMA 9: Uso directo de Console.WriteLine en todas partes
    //  SOLUCIÓN: Adapter IOutput (ya usado en varias clases)
    // =======================================================
    // Ya se aplicó arriba: GoldIngot, GoldIngotOperation, ChainOperation, etc.
    // usan IOutput en vez de Console directamente.

    // =======================================================
    //  PROBLEMA 10: Agregar nuevo producto rompe Produce()
    //  SOLUCIÓN: Extender fábrica, no ProductionManager (OCP)
    // =======================================================

    public class BraceletOperation : ProductOperationBase, IProductOperation
    {
        private readonly IOutput _output;

        public BraceletOperation(IOutput output)
        {
            _output = output;
        }

        protected override void Prepare()
        {
            _output.WriteLine("Preparando estación de pulseras...");
        }

        protected override void ProcessCore()
        {
            _output.WriteLine("Forjando y ensamblando pulsera...");
        }

        protected override void Validate()
        {
            _output.WriteLine("Inspeccionando calidad de la pulsera...");
        }
    }

    // NOTA: Para que todo esto corra en consola, podrías tener un Program.cs como:
    //
    // static void Main()
    // {
    //     IOutput output = new ConsoleOutput();
    //     IProductFactory factory = new DefaultProductFactory(output);
    //     var manager = new ProductionManager(factory);
    //
    //     manager.Produce(ProductType.GoldIngot);
    //     manager.Produce(ProductType.Diamond);
    //     manager.Produce(ProductType.Chain);
    //     manager.Produce(ProductType.Bracelet);
    // }
}
