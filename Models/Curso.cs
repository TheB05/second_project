using System.ComponentModel.DataAnnotations;

namespace second_project.Models;

public sealed class Curso
{
    public int Id { get; set; }

    [Required, StringLength(20)]
    public string Codigo { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required, StringLength(80)]
    public string Area { get; set; } = string.Empty;

    [Range(1, 1000)]
    public int Horas { get; set; }

    [Range(1, 10000)]
    public int CupoMaximo { get; set; }

    public DateOnly FechaInicio { get; set; }
}
