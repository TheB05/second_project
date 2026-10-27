using second_project.Models;

namespace second_project.Services;

public sealed class EstudianteService : IEstudianteService
{
    private readonly object _sync = new();
    private readonly List<Estudiante> _estudiantes = [];
    private int _siguienteId = 1;

    public List<Estudiante> ObtenerTodos()
    {
        lock (_sync)
        {
            return _estudiantes.Select(Copiar).ToList();
        }
    }

    public Estudiante? ObtenerPorId(int id)
    {
        lock (_sync)
        {
            var estudiante = _estudiantes.Find(estudiante => estudiante.Id == id);
            return estudiante is null ? null : Copiar(estudiante);
        }
    }

    public Estudiante Crear(CreateEstudianteRequest request)
    {
        lock (_sync)
        {
            var estudiante = new Estudiante
            {
                Id = _siguienteId++,
                Matricula = request.Matricula,
                Nombre = request.Nombre,
                Correo = request.Correo
            };
            _estudiantes.Add(estudiante);
            return Copiar(estudiante);
        }
    }

    private static Estudiante Copiar(Estudiante estudiante) =>
        new()
        {
            Id = estudiante.Id,
            Matricula = estudiante.Matricula,
            Nombre = estudiante.Nombre,
            Correo = estudiante.Correo
        };
}
