using FluentValidation;
using MakeupClone.Domain.Entities;

namespace MakeupClone.Application.Validators;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(product => product.Name).NotEmpty();
        RuleFor(product => product.Description).NotEmpty();
        RuleFor(product => product.Price).GreaterThan(0).NotEmpty();
        RuleFor(product => product.StockQuantity).GreaterThanOrEqualTo(0).NotEmpty();
        RuleFor(product => product.ImageUrl).NotEmpty();
        RuleFor(product => product.CategoryId).NotEmpty();
        RuleFor(product => product.BrandId).NotEmpty();
    }
}