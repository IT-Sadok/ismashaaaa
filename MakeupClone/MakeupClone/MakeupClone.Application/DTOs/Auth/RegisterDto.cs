﻿namespace MakeupClone.Application.DTOs.Auth;

public class RegisterDto
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime? BirthDate { get; set; }

    public string PhoneNumber { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string ConfirmPassword { get; set; }

    public bool ReceiveNotifications { get; set; }
}