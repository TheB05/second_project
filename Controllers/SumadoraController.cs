using Microsoft.AspNetCore.Mvc;
using second_project.Models;

namespace second_project.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class SumadoraController : ControllerBase
{
    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(SumadoraResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<SumadoraResponse> Post(SumadoraRequest request)
    {
        try
        {
            return Ok(new SumadoraResponse(request.Numero1 + request.Numero2));
        }
        catch (OverflowException)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "La suma excede el rango numérico permitido.",
                Status = StatusCodes.Status400BadRequest
            });
        }
    }
}
