using second_project.Models;

namespace second_project.Services;

public sealed class CursoService : ICursoService
{
    private readonly object _sync = new();
    private readonly List<Curso> _cursos = [];
    private int _siguienteId = 1;

    public List<Curso> ObtenerTodos()
    {
        lock (_sync)
        {
            return _cursos.Select(Copiar).ToList();
        }
    }

    public Curso? ObtenerPorId(int id)
    {
        lock (_sync)
        {
            var curso = _cursos.Find(curso => curso.Id == id);
            return curso is null ? null : Copiar(curso);
        }
    }

    public Curso Crear(Curso curso)
    {
        lock (_sync)
        {
            var nuevoCurso = Copiar(curso);
            nuevoCurso.Id = _siguienteId++;
            _cursos.Add(nuevoCurso);
            return Copiar(nuevoCurso);
        }
    }

    public Curso? Actualizar(int id, Curso curso)
    {
        lock (_sync)
        {
            var indice = _cursos.FindIndex(cursoExistente => cursoExistente.Id == id);
            if (indice < 0)
            {
                return null;
            }

            var cursoActualizado = Copiar(curso);
            cursoActualizado.Id = id;
            _cursos[indice] = cursoActualizado;
            return Copiar(cursoActualizado);
        }
    }

    public bool Eliminar(int id)
    {
        lock (_sync)
        {
            var indice = _cursos.FindIndex(curso => curso.Id == id);
            if (indice < 0)
            {
                return false;
            }

            _cursos.RemoveAt(indice);
            return true;
        }
    }

    private static Curso Copiar(Curso curso) =>
        new()
        {
            Id = curso.Id,
            Codigo = curso.Codigo,
            Nombre = curso.Nombre,
            Area = curso.Area,
            Horas = curso.Horas,
            CupoMaximo = curso.CupoMaximo,
            FechaInicio = curso.FechaInicio
        };
}
