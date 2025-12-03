namespace TraxNy.ManufacturingHub.Dtos;

/// <summary>
/// Usuario mostrado en la pantalla de Gestión de Usuarios.
/// </summary>
public record UserDto(
    string Name,
    string Role,
    bool Active,
    DateTime LastLogin
);
