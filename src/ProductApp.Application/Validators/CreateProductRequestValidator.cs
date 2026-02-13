using FluentValidation;
using ProductApp.Model.Requests;

namespace ProductApp.Application.Validators;

public sealed class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0);
    }
}
