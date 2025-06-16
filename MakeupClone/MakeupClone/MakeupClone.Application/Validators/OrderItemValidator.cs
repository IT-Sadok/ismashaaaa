using FluentValidation;
using MakeupClone.Domain.Entities;

namespace MakeupClone.Application.Validators;

public class OrderItemValidator : AbstractValidator<OrderItem>
{
    public OrderItemValidator()
    {
        RuleFor(item => item.Id)
            .NotEmpty().WithMessage("Order item ID is required.");

        RuleFor(item => item.ProductId)
            .NotEmpty().WithMessage("Product ID is required.");

        RuleFor(item => item.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

        RuleFor(item => item.PricePerItem)
            .GreaterThan(0).WithMessage("Price per item must be greater than 0.");

        RuleFor(item => item.TotalPrice)
            .Equal(item => item.PricePerItem * item.Quantity)
            .WithMessage("Total price must equal price per item multiplied by quantity.");
    }
}