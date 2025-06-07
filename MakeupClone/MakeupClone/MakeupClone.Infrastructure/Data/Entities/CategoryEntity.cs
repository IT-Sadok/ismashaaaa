namespace MakeupClone.Infrastructure.Data.Entities;

public class CategoryEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public ICollection<ProductEntity> Products { get; set; }
}