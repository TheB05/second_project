using System.ComponentModel.DataAnnotations;

namespace second_project.Models;

public sealed class Inscripcion
{
    public int Id { get; set; }

    [Range(1, int.MaxValue)]
    public int EstudianteId { get; set; }

    [Range(1, int.MaxValue)]
    public int CursoId { get; set; }

    public DateOnly Fecha { get; set; }
}
