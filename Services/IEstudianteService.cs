using second_project.Models;

namespace second_project.Services;

public interface IEstudianteService
{
    List<Estudiante> ObtenerTodos();
    Estudiante? ObtenerPorId(int id);
    Estudiante Crear(CreateEstudianteRequest request);
}
