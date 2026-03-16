using BarberBoss.Application.UseCases.Billings.Delete;
using BarberBoss.Application.UseCases.Billings.GetAll;
using BarberBoss.Application.UseCases.Billings.GetById;
using BarberBoss.Application.UseCases.Billings.Register;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Exception.ExceptionBase;
using Microsoft.AspNetCore.Mvc;

namespace BarberBoss.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BillingController : ControllerBase {

    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegistedBillingJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorsJson), StatusCodes.Status400BadRequest)]

    public async Task<IActionResult> Register([FromServices] IRegisterBillingUseCase useCase, [FromBody] RequestBillingJson request) {
        try {
            var response = await useCase.Execute(request);
            return Created(String.Empty, response);
        } catch (ErrorOnValidatorException ex) {

            var response = new ResponseErrorsJson(ex.Errors);
            return BadRequest(response);
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseBillingsJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    public async Task<IActionResult> GetAll([FromServices] IGetAllBillingsUseCase useCase) {
        var response = await useCase.Execute();
        if (response.Billings.Count > 0) {
            return Ok(response);
        }
        return NotFound();
    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseBillingJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    public async Task<IActionResult> GetById([FromServices] IGetByIdBillingsUseCase useCase, long id) {
        var response = await useCase.Execute(id);
        if (response is not null) {
            return Ok(response);
        }
        return NotFound();
    }


    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    public async Task<IActionResult> Delete([FromServices] IDeleteBillingUseCase useCase, long id) {
        try {
            await useCase.Execute(id);
            return NoContent();

        } catch (NotFoundException ex) {

            var response = new ResponseErrorsJson(ex.Message);
            return NotFound(response);
        }
    }


}