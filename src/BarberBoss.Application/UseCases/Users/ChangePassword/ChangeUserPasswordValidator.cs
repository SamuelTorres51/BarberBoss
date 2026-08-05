using BarberBoss.Communication.Requests;
using FluentValidation;

namespace BarberBoss.Application.UseCases.Users.ChangePassword;

public class ChangeUserPasswordValidator : AbstractValidator<RequestChangePasswordJson> {
    public ChangeUserPasswordValidator() {
        RuleFor(user => user.NewPassword).SetValidator(new PasswordValidator<RequestChangePasswordJson>());
    }
}
