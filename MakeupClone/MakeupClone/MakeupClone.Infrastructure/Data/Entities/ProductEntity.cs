namespace MakeupClone.Infrastructure.Data.Entities;

public class ProductEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public string ImageUrl { get; set; }

    public Guid CategoryId { get; set; }

    public CategoryEntity Category { get; set; }

    public Guid BrandId { get; set; }

    public BrandEntity Brand { get; set; }

    public ICollection<ReviewEntity> Reviews { get; set; }
}