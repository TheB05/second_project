using Microsoft.AspNetCore.Mvc;
using second_project.Models;

namespace second_project.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class DatosPersonalesController : ControllerBase
{
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(DatosPersonalesResponse), StatusCodes.Status200OK)]
    public ActionResult<DatosPersonalesResponse> Get()
    {
        var datos = new DatosPersonalesResponse(
            "Ana Pérez",
            new DateOnly(2002, 5, 14),
            "Calle Principal #10");

        return Ok(datos);
    }
}
