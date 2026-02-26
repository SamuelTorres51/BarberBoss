using BarberBoss.Communication.Requests;
using FluentValidation;

namespace BarberBoss.Application.UseCases.Billings;

public class BillingValidator : AbstractValidator<RequestBillingJson>{
    public BillingValidator() {
        RuleFor(billing => billing.BarberName).NotEmpty().WithMessage("O nome do barbeiro é obrigatório.")
            .MaximumLength(80).WithMessage("O nome do barbeiro deve conter no máximo 80 caracteres.");
        RuleFor(billing => billing.ClientName).NotEmpty().WithMessage("O nome do cliente é obrigatório.")
            .MaximumLength(120).WithMessage("O nome do cliente deve conter no máximo 120 caracteres.");
        RuleFor(billing => billing.ServiceName).NotEmpty().WithMessage("O serviço é obrigatório.")
            .MaximumLength(120).WithMessage("O nome do serviço deve conter no máximo 120 caracteres.");
        RuleFor(billing => billing.Amount).GreaterThan(0).WithMessage("O valor deve ser maior que zero.");
        RuleFor(billing => billing.Date).NotEmpty().WithMessage("A data é obrigatória.");
        RuleFor(billing => billing.PaymentMethod).NotEmpty().WithMessage("O método de pagamento é obrigatório.")
            .IsInEnum().WithMessage("O método de pagamento deve ser um valor válido.");
        RuleFor(billing => billing.Status).NotEmpty().WithMessage("O status é obrigatório.")
            .IsInEnum().WithMessage("O status deve ser um valor válido.");
        RuleFor(billing => billing.Notes).MaximumLength(500).WithMessage("As observações devem conter no máximo 500 caracteres.");
    }
}
