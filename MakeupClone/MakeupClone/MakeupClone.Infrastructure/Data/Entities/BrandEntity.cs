namespace MakeupClone.Infrastructure.Data.Entities;

public class BrandEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public ICollection<ProductEntity> Products { get; set; }
}