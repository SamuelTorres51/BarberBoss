using BarberBoss.Application.UseCases.Billings.Register;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BarberBoss.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BillingController : ControllerBase {

    [HttpPost]
    [ProducesResponseType(ResponseRegistedBillingJson, StatusCodes.Status201Created)]
    [ProducesResponseType(ResponseErrorsJson, StatusCodes.Status400BadRequest)]
    
    public async Task<IActionResult> Register([FromServices] IRegisterBillingUseCase useCase,[FromBody] RequestBillingJson request) {
        try {
            var response = await useCase.Execute(request);
            return Created()
        }
    }
}
