using second_project.Models;

namespace second_project.Services;

public sealed class EstudianteService(RegistroAcademicoStore store) : IEstudianteService
{
    public List<Estudiante> ObtenerTodos()
    {
        lock (store.Sync)
        {
            return store.Estudiantes.Select(Copiar).ToList();
        }
    }

    public Estudiante? ObtenerPorId(int id)
    {
        lock (store.Sync)
        {
            var estudiante = store.Estudiantes.Find(estudiante => estudiante.Id == id);
            return estudiante is null ? null : Copiar(estudiante);
        }
    }

    public Estudiante Crear(CreateEstudianteRequest request)
    {
        lock (store.Sync)
        {
            var estudiante = new Estudiante
            {
                Id = store.SiguienteEstudianteId++,
                Matricula = request.Matricula,
                Nombre = request.Nombre,
                Correo = request.Correo
            };
            store.Estudiantes.Add(estudiante);
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
