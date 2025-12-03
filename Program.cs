using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// (Si no usas controllers, no hace falta agregar más servicios)
var app = builder.Build();

// Servir archivos estáticos (wwwroot)
app.UseDefaultFiles();
app.UseStaticFiles();

// ---------- ENDPOINTS LINGOTE (ORO) ----------

app.MapGet("/api/ingot/current", () =>
{
    return Results.Json(new
    {
        id = "B785-A",
        pureza = 999.9,
        peso = 1000.5,
        tempMolde = 1064,
        lote = "B785-A",
        progreso = 66,              // porcentaje
        etapa = "Solidificado"
    });
});

app.MapGet("/api/ingot/events", () =>
{
    var events = new[]
    {
        new { timestamp = "[14:32:10]", mensaje = "Temperatura del molde estabilizada en 1064°C.", tipo = "info" },
        new { timestamp = "[14:33:05]", mensaje = "Vertido de oro completado. Peso: 1000.5g.", tipo = "info" },
        new { timestamp = "[14:33:15]", mensaje = "Protocolo de enfriamiento iniciado.", tipo = "info" },
        new { timestamp = "[14:35:00]", mensaje = "Alerta: Tasa de enfriamiento -5% por debajo del objetivo.", tipo = "alerta" },
        new { timestamp = "[14:35:30]", mensaje = "Sistema de ventilación ajustado +10%.", tipo = "info" },
        new { timestamp = "[14:36:00]", mensaje = "Tasa de enfriamiento restaurada al rango normal.", tipo = "info" },
        new { timestamp = "[14:40:00]", mensaje = "Fase de solidificación alcanzada.", tipo = "info" }
    };

    return Results.Json(events);
});

// ---------- ENDPOINTS DIAMANTE ----------

app.MapGet("/api/diamond/current", () =>
{
    return Results.Json(new
    {
        id = "LDG-23C",
        crecimiento = 25.3,
        tempReactor = 1350,
        presion = 5.8,
        pureza = "Type IIa",
        cristalizacionEstimado = "72h 15m",
        progreso = 45
    });
});

app.MapGet("/api/diamond/events", () =>
{
    var events = new[]
    {
        new { timestamp = "[08:00:15]", mensaje = "Reactor sellado. Lote #LDG-23C iniciado.", tipo = "info" },
        new { timestamp = "[08:15:30]", mensaje = "Presión objetivo 5.8 GPa alcanzada.", tipo = "info" },
        new { timestamp = "[08:25:10]", mensaje = "Temperatura objetivo 1350°C estabilizada.", tipo = "info" },
        new { timestamp = "[08:30:00]", mensaje = "Fase de nucleación de semilla completada.", tipo = "info" },
        new { timestamp = "[09:45:22]", mensaje = "Alerta: Fluctuación de gas metano detectada.", tipo = "alerta" },
        new { timestamp = "[09:45:50]", mensaje = "Regulador de flujo de gas ajustado automáticamente.", tipo = "info" },
        new { timestamp = "[09:47:00]", mensaje = "Parámetros de crecimiento estables. Tasa: 25.3 µm/h.", tipo = "info" }
    };

    return Results.Json(events);
});

app.Run();
