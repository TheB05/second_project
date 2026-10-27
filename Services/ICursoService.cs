using second_project.Models;

namespace second_project.Services;

public interface ICursoService
{
    List<Curso> ObtenerTodos();
    Curso? ObtenerPorId(int id);
    Curso Crear(Curso curso);
    Curso? Actualizar(int id, Curso curso);
    bool Eliminar(int id);
}
