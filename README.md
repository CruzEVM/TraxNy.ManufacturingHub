# Refactorización – TraxNY INC (Patrones GoF)

Este documento contiene el análisis, problemas detectados y justificación de patrones GoF aplicados para mejorar el diseño del sistema.

---

## 1. Problemas identificados (mínimo 10)

| # | Problema detectado | Categoría / Falta de patrón |
|---|---------------------|-----------------------------|
| 1 | Uso de `if`/`else` o `switch` para seleccionar productos | ❌ Violación OCP — usar Factory Method / Strategy |
| 2 | `ProductionManager` tiene múltiples responsabilidades | ❌ Violación SRP — aplicar Command |
| 3 | Acoplamiento fuerte entre manager y productos concretos | ❌ Falta de abstracción — aplicar Abstract Factory |
| 4 | Lógica duplicada en los flujos de producción | ❌ Oportunidad Template Method |
| 5 | Uso de strings mágicos (`"gold"`, `"diamond"`) | ❌ Código frágil — usar Enum Strategy |
| 6 | No existe interfaz común para productos | ❌ Falta de polimorfismo — crear `IProductOperation` |
| 7 | Métodos como `Grow()`, `Forge()` sin encapsulación | ❌ Aplicar Command |
| 8 | No existe registro de eventos | ❌ Falta Observer / Mediator |
| 9 | Dependencia directa a `Console.WriteLine` | ❌ Aplicar Adapter |
|10 | Agregar un nuevo producto rompe el método `Produce()` | ❌ Violación OCP — Factory Method |
|11 | Violación DIP (manager depende de clases concretas) | ❌ Aplicar interfaces |
|12 | Código difícil de testear (salida fija en consola) | ❌ Adapter + inyección de dependencias |

---

## 2. Justificación de problemas y patrones aplicados

### 1. Condicionales para crear productos → **Factory Method**
Los `if/switch` hacían el código rígido y no extensible.

**Solución:**  
Usar `ProductFactory` para crear productos según `ProductType`.

---

### 2. ProductionManager con demasiadas responsabilidades → **Command**
Controlaba pasos, creación, impresión y registro.

**Solución:**  
Encapsular acciones en comandos independientes.

---

### 3. Acoplamiento fuerte → **Abstract Factory**
`new GoldIngot()` dentro del manager violaba DIP.

**Solución:**  
Crear `IProductFactory` que retorna `IProductOperation`.

---

### 4. Flujo repetido entre productos → **Template Method**
Varios productos repetían los mismos pasos.

**Solución:**  
Crear plantilla general del proceso con pasos modificables.

---

### 5. Strings mágicos → **Enum Strategy**
Los tipos `"gold"`, `"diamond"` eran propensos a errores.

**Solución:**  
Enum seguro: `ProductType`.

---

### 6. Sin interfaz base para productos → **Polimorfismo**
Cada producto tenía métodos diferentes.

**Solución:**  
Crear interfaz común:

```csharp
public interface IProductOperation {
    void ExecuteProcess();
}
```
---
### 7. Métodos sueltos → Command

Operaciones encapsuladas y reutilizables.

---

### 8. Sin sistema de eventos → Observer / Mediator

Nada notificaba cambios.

**Solución:**
Capa IOutput que recibe eventos.

---
### 9. Dependencia a consola → Adapter

El código usaba directamente Console.WriteLine.

**Solución:**
ConsoleOutputAdapter implementa IOutput.

---
### 10. Nuevos productos rompen el flujo → Factory Method

Sin fábrica, había que modificar código.

**Solución:**
OCP asegurado gracias a la fábrica.

---
### 11. Violación DIP → Interfaces

El manager dependía de clases concretas.

**Solución:**
Inyección de IProductFactory y IOutput.

---
### 12. Difícil de testear → Adapter

Salida desacoplada para pruebas unitarias.

---
## 3. Diagramas UML (Antes / Después)
### 📌 Antes – Código rígido
```csharp
@startuml

class ProductionManager {
  +Produce(type: string)
}

class GoldIngot
class Diamond
class Chain

ProductionManager --> GoldIngot
ProductionManager --> Diamond
ProductionManager --> Chain

note right
Código acoplado.
Condicionales con strings.
Violación SRP, OCP y DIP.
end note

@enduml
```
<img width="577" height="192" alt="TP11IaGn38RtEKMMYiuxW0iPw83CGYmAxaDREe5EChHvYuTugdU4Tp4hTY7exFBdv_i8sJUZQlEvG2ZK6lggSGx6Aiyab5F53q2y-rcd6rikwG6RLPPy2vymg2SjyIXPRUG3qrabZkHEn0BmLxtRFVv2_--7waexrAAMi78-6RXjZPmLAUYbKDG9NCSvT91AQXXKnlrjWdVMGe6tLV3jrT_ZY_EuuE7e9qWIyLiB-pxr_tm1" src="https://github.com/user-attachments/assets/86adaa77-a1b4-4b83-8367-8b0293ffd03d" />

### 📌 Después – Aplicación de patrones GoF
```csharp
@startuml

enum ProductType {
  GoldIngot
  Diamond
  Chain
}

interface IProductOperation {
  +ExecuteProcess()
}

interface IProductFactory {
  +CreateProduct(type: ProductType): IProductOperation
}

interface IOutput {
  +Write(msg: string)
}

class ProductionManager {
  -factory : IProductFactory
  -output : IOutput
  +Produce(type: ProductType)
}

class ProductFactory implements IProductFactory
class ConsoleOutputAdapter implements IOutput

class GoldIngot implements IProductOperation
class DiamondLab implements IProductOperation
class Chain implements IProductOperation

ProductionManager --> IProductFactory
ProductionManager --> IOutput
ProductFactory --> IProductOperation
ConsoleOutputAdapter --> IOutput

@enduml
```
<img width="623" height="574" alt="ZP7DIiGm4CVlUOeSTv6-G0-oKZzOKDQ3u7bCPsiWcPHa0YxYkvjj6gpD8ju2y_j_P6ON63n7QuH0YbO-UzT7nI-d4UMdaFB1cNvFW-FqljLW7VNfrRs39l4bX2P6VmI5SZyh3oDwOEreab_TVQ2AZ6ceC8JDzhBe7XGxVygIpYCmpfCDfoRjijQshRDMheV8O-JizkeruyQ6ePM1lQPXgg" src="https://github.com/user-attachments/assets/2e8d8b3c-51a8-47e7-8a18-081cb6eea0a7" />

