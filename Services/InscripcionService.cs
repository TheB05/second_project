using second_project.Models;

namespace second_project.Services;

public sealed class InscripcionService(
    ICursoService cursoService,
    IEstudianteService estudianteService,
    RegistroAcademicoStore store) : IInscripcionService
{
    public List<Estudiante>? ObtenerEstudiantesPorCurso(int cursoId)
    {
        if (cursoService.ObtenerPorId(cursoId) is null)
        {
            return null;
        }

        lock (store.Sync)
        {
            var estudiantesIds = store.Inscripciones
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
        lock (store.Sync)
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

            if (store.Inscripciones.Any(inscripcion =>
                    inscripcion.CursoId == cursoId &&
                    inscripcion.EstudianteId == estudianteId))
            {
                return (null, "El estudiante ya está inscrito en este curso.", false);
            }

            var cuposOcupados = store.Inscripciones.Count(inscripcion =>
                inscripcion.CursoId == cursoId);
            if (cuposOcupados >= curso.CupoMaximo)
            {
                return (null, "El curso alcanzó su cupo máximo.", false);
            }

            var inscripcion = new Inscripcion
            {
                Id = store.SiguienteInscripcionId++,
                EstudianteId = estudianteId,
                CursoId = cursoId,
                Fecha = DateOnly.FromDateTime(DateTime.Today)
            };
            store.Inscripciones.Add(inscripcion);
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
