using Microsoft.AspNetCore.Mvc;
using second_project.Models;
using second_project.Services;

namespace second_project.Controllers;

[ApiController]
public sealed class InscripcionesController(IInscripcionService inscripcionService) : ControllerBase
{
    [HttpGet("api/cursos/{cursoId:int}/estudiantes")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IReadOnlyList<Estudiante>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IReadOnlyList<Estudiante>> GetEstudiantesPorCurso(int cursoId)
    {
        var estudiantes = inscripcionService.ObtenerEstudiantesPorCurso(cursoId);
        return estudiantes is null ? NotFound() : Ok(estudiantes);
    }

    [HttpPost("api/inscripciones")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Inscripcion), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<Inscripcion> Post(CreateInscripcionRequest request)
    {
        var resultado = inscripcionService.Inscribir(request.EstudianteId, request.CursoId);
        if (resultado.NotFound)
        {
            return NotFound(new ProblemDetails
            {
                Title = "No se pudo completar la inscripción.",
                Detail = resultado.Error,
                Status = StatusCodes.Status404NotFound
            });
        }

        if (resultado.Inscripcion is null)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "No se pudo completar la inscripción.",
                Detail = resultado.Error,
                Status = StatusCodes.Status400BadRequest
            });
        }

        return Created($"/api/cursos/{request.CursoId}/estudiantes", resultado.Inscripcion);
    }
}
