using BarberBoss.Application.UseCases.Users.Login;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Exception.ExceptionBase;
using Microsoft.AspNetCore.Mvc;

namespace BarberBoss.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase {

    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status401Unauthorized)]

    public async Task<IActionResult> Login([FromServices] IDoLoginUseCase useCase, [FromBody] RequestLoginJson request) {

        try {
            var response = await useCase.Execute(request);
            return Ok(response);
        } catch (ErrorOnValidatorException ex) {

            var response = new ResponseErrorsJson(ex.Errors);
            return Unauthorized(response);
        }
    }  
}
