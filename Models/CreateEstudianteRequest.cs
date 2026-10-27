using System.ComponentModel.DataAnnotations;

namespace second_project.Models;

public sealed record CreateEstudianteRequest
{
    [Required, StringLength(20)]
    public required string Matricula { get; init; }

    [Required, StringLength(100)]
    public required string Nombre { get; init; }

    [Required, EmailAddress, StringLength(254)]
    public required string Correo { get; init; }
}
