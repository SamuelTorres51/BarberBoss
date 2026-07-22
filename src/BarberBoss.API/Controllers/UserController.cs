using BarberBoss.Application.UseCases.Users.Register;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Exception.ExceptionBase;
using Microsoft.AspNetCore.Mvc;

namespace BarberBoss.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase {

    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson) ,StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status400BadRequest)]

    public async Task<IActionResult> Register(
        [FromBody] RequestRegisterUserJson request, 
        [FromServices] IRegisterUserUseCase useCase) 
        {
        try {
            var response = await useCase.Execute(request);
            return Created(String.Empty, response);
        } catch (ErrorOnValidatorException ex) {

            var response = new ResponseErrorsJson(ex.Errors);
            return BadRequest(response);
        }
    }
}
