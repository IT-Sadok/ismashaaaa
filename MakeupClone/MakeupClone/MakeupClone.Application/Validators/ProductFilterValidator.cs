using FluentValidation;
using MakeupClone.Domain.Filters;

namespace MakeupClone.Application.Validators;

public class ProductFilterValidator : AbstractValidator<ProductFilter>
{
    public ProductFilterValidator()
    {
        RuleFor(filter => filter.MinimumPrice)
               .GreaterThan(0).WithMessage("Minimum price must be greater than 0.")
               .LessThan(filter => filter.MaximumPrice)
               .When(filter => filter.MaximumPrice.HasValue)
               .WithMessage("Minimum price must be less than maximum price.");

        RuleFor(filter => filter.MaximumPrice)
            .GreaterThan(0).WithMessage("Maximum price must be greater than 0.");
    }
}