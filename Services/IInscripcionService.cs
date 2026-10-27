using second_project.Models;

namespace second_project.Services;

public interface IInscripcionService
{
    List<Estudiante>? ObtenerEstudiantesPorCurso(int cursoId);
    (Inscripcion? Inscripcion, string? Error, bool NotFound) Inscribir(
        int estudianteId,
        int cursoId);
}
