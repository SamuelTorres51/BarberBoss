using BarberBoss.Communication.Requests;
using FluentValidation;
using BarberBoss.Exception;

namespace BarberBoss.Application.UseCases.Billings;

public class BillingValidator : AbstractValidator<RequestBillingJson>{
    public BillingValidator() {
        RuleFor(billing => billing.BarberName).NotEmpty().WithMessage(ResourceErrorMessages.BARBERNAME_REQUIRED)
            .MaximumLength(80).WithMessage(ResourceErrorMessages.BARBERNAME_SIZE);
        RuleFor(billing => billing.ClientName).NotEmpty().WithMessage(ResourceErrorMessages.CLIENTNAME_REQUIRED)
            .MaximumLength(120).WithMessage(ResourceErrorMessages.CLIENTNAME_SIZE);
        RuleFor(billing => billing.ServiceName).NotEmpty().WithMessage(ResourceErrorMessages.SERVICE_REQUIRED)
            .MaximumLength(120).WithMessage(ResourceErrorMessages.SERVICE_SIZE);
        RuleFor(billing => billing.Amount).GreaterThan(0).WithMessage(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO);
        RuleFor(billing => billing.Date).NotEmpty().WithMessage(ResourceErrorMessages.DATE_REQUIRED);
        RuleFor(billing => billing.PaymentMethod).IsInEnum().WithMessage(ResourceErrorMessages.PAYMENT_IS_VALID);
        RuleFor(billing => billing.Status).IsInEnum().WithMessage(ResourceErrorMessages.STATUS_IS_VALID);
        RuleFor(billing => billing.Notes).MaximumLength(500).WithMessage(ResourceErrorMessages.NOTES_SIZE);
    }
}
