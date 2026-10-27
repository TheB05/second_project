using Microsoft.EntityFrameworkCore;
using second_project.Data;
using second_project.Models;

namespace second_project.Services;

public sealed class CursoService(AppDbContext dbContext) : ICursoService
{
    public List<Curso> ObtenerTodos()
    {
        return dbContext.Cursos
            .AsNoTracking()
            .OrderBy(curso => curso.Id)
            .ToList();
    }

    public Curso? ObtenerPorId(int id)
    {
        return dbContext.Cursos
            .AsNoTracking()
            .FirstOrDefault(curso => curso.Id == id);
    }

    public Curso Crear(Curso curso)
    {
        var nuevoCurso = Copiar(curso);
        dbContext.Cursos.Add(nuevoCurso);
        dbContext.SaveChanges();
        return Copiar(nuevoCurso);
    }

    public Curso? Actualizar(int id, Curso curso)
    {
        var cursoExistente = dbContext.Cursos.Find(id);
        if (cursoExistente is null)
        {
            return null;
        }

        cursoExistente.Codigo = curso.Codigo;
        cursoExistente.Nombre = curso.Nombre;
        cursoExistente.Area = curso.Area;
        cursoExistente.Horas = curso.Horas;
        cursoExistente.CupoMaximo = curso.CupoMaximo;
        cursoExistente.FechaInicio = curso.FechaInicio;
        dbContext.SaveChanges();
        return Copiar(cursoExistente);
    }

    public bool Eliminar(int id)
    {
        var curso = dbContext.Cursos.Find(id);
        if (curso is null)
        {
            return false;
        }

        dbContext.Cursos.Remove(curso);
        dbContext.SaveChanges();
        return true;
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
