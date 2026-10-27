using second_project.Models;

namespace second_project.Services;

public sealed class InscripcionService(
    ICursoService cursoService,
    IEstudianteService estudianteService) : IInscripcionService
{
    private readonly object _sync = new();
    private readonly List<Inscripcion> _inscripciones = [];
    private int _siguienteId = 1;

    public List<Estudiante>? ObtenerEstudiantesPorCurso(int cursoId)
    {
        if (cursoService.ObtenerPorId(cursoId) is null)
        {
            return null;
        }

        lock (_sync)
        {
            var estudiantesIds = _inscripciones
                .Where(inscripcion => inscripcion.CursoId == cursoId)
                .Select(inscripcion => inscripcion.EstudianteId)
                .ToHashSet();

            return estudianteService.ObtenerTodos()
                .Where(estudiante => estudiantesIds.Contains(estudiante.Id))
                .ToList();
        }
    }

    public (Inscripcion? Inscripcion, string? Error, bool NotFound) Inscribir(
        int estudianteId,
        int cursoId)
    {
        lock (_sync)
        {
            var curso = cursoService.ObtenerPorId(cursoId);
            if (curso is null)
            {
                return (null, "El curso seleccionado no existe.", true);
            }

            if (estudianteService.ObtenerPorId(estudianteId) is null)
            {
                return (null, "El estudiante seleccionado no existe.", true);
            }

            if (_inscripciones.Any(inscripcion =>
                    inscripcion.CursoId == cursoId &&
                    inscripcion.EstudianteId == estudianteId))
            {
                return (null, "El estudiante ya está inscrito en este curso.", false);
            }

            var cuposOcupados = _inscripciones.Count(inscripcion =>
                inscripcion.CursoId == cursoId);
            if (cuposOcupados >= curso.CupoMaximo)
            {
                return (null, "El curso alcanzó su cupo máximo.", false);
            }

            var inscripcion = new Inscripcion
            {
                Id = _siguienteId++,
                EstudianteId = estudianteId,
                CursoId = cursoId,
                Fecha = DateOnly.FromDateTime(DateTime.Today)
            };
            _inscripciones.Add(inscripcion);
            return (Copiar(inscripcion), null, false);
        }
    }

    private static Inscripcion Copiar(Inscripcion inscripcion) =>
        new()
        {
            Id = inscripcion.Id,
            EstudianteId = inscripcion.EstudianteId,
            CursoId = inscripcion.CursoId,
            Fecha = inscripcion.Fecha
        };
}
