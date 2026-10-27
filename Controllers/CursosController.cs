using Microsoft.AspNetCore.Mvc;
using second_project.Models;
using second_project.Services;

namespace second_project.Controllers;

[ApiController]
[Route("api/cursos")]
public sealed class CursosController(ICursoService cursoService) : ControllerBase
{
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IReadOnlyList<Curso>), StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyList<Curso>> Get()
    {
        return Ok(cursoService.ObtenerTodos());
    }

    [HttpGet("{id:int}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Curso), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Curso> GetById(int id)
    {
        var curso = cursoService.ObtenerPorId(id);
        return curso is null ? NotFound() : Ok(curso);
    }

    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Curso), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Curso> Post(Curso curso)
    {
        var nuevoCurso = cursoService.Crear(curso);
        return CreatedAtAction(nameof(GetById), new { id = nuevoCurso.Id }, nuevoCurso);
    }

    [HttpPut("{id:int}")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Curso), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Curso> Put(int id, Curso curso)
    {
        var cursoActualizado = cursoService.Actualizar(id, curso);
        return cursoActualizado is null ? NotFound() : Ok(cursoActualizado);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        return cursoService.Eliminar(id) ? NoContent() : NotFound();
    }
}
