namespace MakeupClone.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public string ImageUrl { get; set; }

    public Guid CategoryId { get; set; }

    public Category Category { get; set; }

    public Guid BrandId { get; set; }

    public Brand Brand { get; set; }

    public ICollection<Review> Reviews { get; set; }

    public decimal? DiscountPercentage { get; private set; }

    public decimal DiscountedPrice => DiscountPercentage.HasValue ? Price * (1 - (DiscountPercentage.Value / 100)) : Price;

    public void ApplyDiscount(decimal percentage)
    {
        if (percentage <= 0 || percentage >= 100)
            throw new ArgumentOutOfRangeException(nameof(percentage), "Discount must be between 0 and 100.");

        DiscountPercentage = percentage;
    }

    public void RemoveDiscount() => DiscountPercentage = null;

    public void UpdateDiscount(decimal percentage) => ApplyDiscount(percentage);
}