using BarberBoss.Application.UseCases.Billings.Reports.Excel;
using BarberBoss.Communication.Requests;
using BarberBoss.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace BarberBoss.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = Roles.ADMIN)]

public class ReportController : ControllerBase {

    [HttpGet("excel")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status404NotFound)]

    public async Task<IActionResult> GetExcel([FromServices] IGenerateBillingsReportsExcelUseCase useCase, [FromHeader] DateOnly month) {
        byte[] file = await useCase.Execute(month);
        if (file.Length > 0)
            return File(file, MediaTypeNames.Application.Octet, "report.xlsx");

        return NoContent();
    }
}
