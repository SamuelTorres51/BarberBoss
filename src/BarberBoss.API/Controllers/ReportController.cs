using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace BarberBoss.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportController : ControllerBase {

    [HttpGet("excel")]
    [ProducesResponseType(typeof(FileContentResult) ,StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(FileContentResult) ,StatusCodes.Status404NotFound)]

    public async Task<IActionResult> GetExcel() {
        byte[] file = new byte[1];
        if(file.Length > 0)
            return File(file, MediaTypeNames.Application.Octet, "report.xlsx");

        return NoContent();
    }
}
