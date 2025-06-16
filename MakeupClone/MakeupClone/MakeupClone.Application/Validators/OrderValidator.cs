using FluentValidation;
using MakeupClone.Domain.Entities;

namespace MakeupClone.Application.Validators;

public class OrderValidator : AbstractValidator<Order>
{
    public OrderValidator()
    {
        RuleFor(order => order.Id)
            .NotEmpty().WithMessage("Order ID is required.");

        RuleFor(order => order.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(order => order.Status)
            .IsInEnum().WithMessage("Invalid order status.");

        RuleFor(order => order.CreatedAt)
            .NotEmpty().WithMessage("Creation date is required.");

        RuleFor(order => order.Items)
            .NotEmpty().WithMessage("Order must contain at least one item.");

        RuleForEach(order => order.Items)
            .SetValidator(new OrderItemValidator());
    }
}