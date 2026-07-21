using BarberBoss.Communication.Requests;
using BarberBoss.Exception;
using FluentValidation;

namespace BarberBoss.Application.UseCases.Users.Register;

public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>{
    public RegisterUserValidator() {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceErrorMessages.USERNAME_REQUIRED);
        
        RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceErrorMessages.EMAIL_REQUIRED)
            .EmailAddress().WithMessage(ResourceErrorMessages.EMAIL_INVALID);

        RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestRegisterUserJson>());
    }
}