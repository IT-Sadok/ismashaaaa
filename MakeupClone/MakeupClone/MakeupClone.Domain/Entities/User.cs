using Microsoft.AspNetCore.Identity;

namespace MakeupClone.Domain.Entities;

public class User : IdentityUser
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime? BirthDate { get; set; }

    public bool ReceiveNotifications { get; set; }
}