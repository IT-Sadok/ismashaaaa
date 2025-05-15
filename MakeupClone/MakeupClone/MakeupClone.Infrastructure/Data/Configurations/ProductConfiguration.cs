using MakeupClone.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MakeupClone.Infrastructure.Data.Configurations;
public class ProductConfiguration : IEntityTypeConfiguration<ProductEntity>
{
    public void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(product => product.Id);

        builder.Property(product => product.Name)
               .IsRequired();

        builder.Property(product => product.Description);

        builder.Property(product => product.Price)
               .IsRequired();

        builder.Property(product => product.StockQuantity);

        builder.Property(product => product.ImageUrl);

        builder.HasOne(product => product.Category)
               .WithMany(category => category.Products)
               .HasForeignKey(product => product.CategoryId);

        builder.HasOne(product => product.Brand)
               .WithMany(brand => brand.Products)
               .HasForeignKey(product => product.BrandId);
    }
}