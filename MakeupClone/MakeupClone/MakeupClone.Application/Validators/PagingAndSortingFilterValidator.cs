using FluentValidation;
using MakeupClone.Domain.Filters;

namespace MakeupClone.Application.Validators;

public class PagingAndSortingFilterValidator : AbstractValidator<PagingAndSortingFilter>
{
    public PagingAndSortingFilterValidator()
    {
        RuleFor(filter => filter.Take).GreaterThan(0).WithMessage("Take must be greater than zero.");
        RuleFor(filter => filter.Skip).GreaterThanOrEqualTo(0).WithMessage("Skip cannot be negative.");
    }
}