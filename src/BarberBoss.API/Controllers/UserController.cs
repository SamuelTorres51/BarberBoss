using BarberBoss.Application.UseCases.Users.ChangePassword;
using BarberBoss.Application.UseCases.Users.GetProfile;
using BarberBoss.Application.UseCases.Users.Register;
using BarberBoss.Application.UseCases.Users.Update;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Exception.ExceptionBase;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet]
    [ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status200OK)]
    [Authorize]

    public async Task<IActionResult> GetProfile([FromServices] IGetUserProfileUseCase useCase) {

        var response = await useCase.Execute();

        return Ok(response);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status400BadRequest)]
    [Authorize]

    public async Task<IActionResult> Update([FromBody] RequestUpdateUserJson request, [FromServices] IUpdateUserUseCase useCase) {
        try {
            await useCase.Execute(request);
            return NoContent();
        } catch (ErrorOnValidatorException ex) {
            var response = new ResponseErrorsJson(ex.Errors);
            return BadRequest(response);
        }
    }


    [HttpPut("change-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status400BadRequest)]
    [Authorize]

    public async Task<IActionResult> ChangePassword([FromBody] RequestChangePasswordJson request, [FromServices] IChangeUserPasswordUseCase useCase) {
        try {
            await useCase.Execute(request);
            return NoContent();
        } catch (ErrorOnValidatorException ex) {
            var response = new ResponseErrorsJson(ex.Errors);
            return BadRequest(response);
        }
    }
}
