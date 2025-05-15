using FluentValidation;
using MakeupClone.Application.DTOs;

namespace MakeupClone.Application.Validators;

public class RegisterValidator : AbstractValidator<RegisterDto>
{
    public RegisterValidator()
    {
        RuleFor(register => register.FirstName)
            .NotEmpty().WithMessage("First name is required.");

        RuleFor(register => register.LastName)
            .NotEmpty().WithMessage("Last name is required.");

        RuleFor(register => register.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(register => register.PhoneNumber)
          .NotEmpty().WithMessage("Phone number is required.")
          .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format.");

        RuleFor(register => register.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]").WithMessage("Password must contain at least one digit.")
            .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.");

        RuleFor(register => register.ConfirmPassword)
            .NotEmpty().WithMessage("Confirm password is required.")
            .Equal(register => register.Password).WithMessage("Passwords do not match.");
    }
}