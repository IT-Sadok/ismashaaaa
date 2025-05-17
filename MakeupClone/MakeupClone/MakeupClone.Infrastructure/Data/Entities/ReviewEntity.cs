using MakeupClone.Domain.Entities;

namespace MakeupClone.Infrastructure.Data.Entities;

public class ReviewEntity
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public ProductEntity Product { get; set; }

    public string UserId { get; set; }

    public User User { get; set; }

    public string Content { get; set; }

    public int Rating { get; set; }

    public DateTime DateCreated { get; set; }
}