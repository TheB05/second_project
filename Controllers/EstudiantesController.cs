using Microsoft.AspNetCore.Mvc;
using second_project.Models;
using second_project.Services;

namespace second_project.Controllers;

[ApiController]
[Route("api/estudiantes")]
public sealed class EstudiantesController(IEstudianteService estudianteService) : ControllerBase
{
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IReadOnlyList<Estudiante>), StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyList<Estudiante>> Get()
    {
        return Ok(estudianteService.ObtenerTodos());
    }

    [HttpGet("{id:int}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Estudiante), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Estudiante> GetById(int id)
    {
        var estudiante = estudianteService.ObtenerPorId(id);
        return estudiante is null ? NotFound() : Ok(estudiante);
    }

    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Estudiante), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Estudiante> Post(CreateEstudianteRequest request)
    {
        var estudiante = estudianteService.Crear(request);
        return CreatedAtAction(nameof(GetById), new { id = estudiante.Id }, estudiante);
    }
}
