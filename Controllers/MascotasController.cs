using Microsoft.AspNetCore.Mvc;
using second_project.Models;

namespace second_project.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class MascotasController : ControllerBase
{
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IReadOnlyList<MascotaResponse>), StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyList<MascotaResponse>> Get()
    {
        MascotaResponse[] mascotas =
        [
            new(1, "Firulais", "3 años", "Labrador"),
            new(2, "Leon Kennedy", "4 años", "husky"),
            new(3, "Wilson", "2 años", "gato beige"),
            new(4, "Toby", "3 meses", "pomeranian"),
            new(5, "Sonic", "6 años", "erizo")
        ];

        return Ok(mascotas);
    }
}
