using System;

namespace MakeupClone.Domain.Entities;

public class Review
{
    public Guid Id { get; set; }

    public string Content { get; set; }

    public int Rating { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; }

    public Guid ProductId { get; set; }

    public Product Product { get; set; }

    public DateTime DateCreated { get; set; }
}