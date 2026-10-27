using second_project.Models;

namespace second_project.Services;

public sealed class RegistroAcademicoStore
{
    internal object Sync { get; } = new();
    internal List<Estudiante> Estudiantes { get; } = [];
    internal List<Inscripcion> Inscripciones { get; } = [];
    internal int SiguienteEstudianteId { get; set; } = 1;
    internal int SiguienteInscripcionId { get; set; } = 1;
}
