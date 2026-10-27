using System.ComponentModel.DataAnnotations;

namespace second_project.Models;

public sealed class Estudiante
{
    public int Id { get; set; }

    [Required, StringLength(20)]
    public string Matricula { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(254)]
    public string Correo { get; set; } = string.Empty;
}
