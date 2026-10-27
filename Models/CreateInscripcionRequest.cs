using System.ComponentModel.DataAnnotations;

namespace second_project.Models;

public sealed record CreateInscripcionRequest
{
    [Range(1, int.MaxValue)]
    public int EstudianteId { get; init; }

    [Range(1, int.MaxValue)]
    public int CursoId { get; init; }
}
