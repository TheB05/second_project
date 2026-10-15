using System.Text.Json.Serialization;

namespace second_project.Models;

public sealed record DatosPersonalesResponse(
    string Nombre,
    [property: JsonPropertyName("fecha_nacimiento")] DateOnly FechaNacimiento,
    string Direccion);
