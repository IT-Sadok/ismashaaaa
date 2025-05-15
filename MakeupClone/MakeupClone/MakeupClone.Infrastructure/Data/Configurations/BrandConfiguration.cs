using MakeupClone.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MakeupClone.Infrastructure.Data.Configurations;

public class BrandConfiguration : IEntityTypeConfiguration<BrandEntity>
{
    public void Configure(EntityTypeBuilder<BrandEntity> builder)
    {
        builder.ToTable("Brands");

        builder.HasKey(brand => brand.Id);

        builder.Property(brand => brand.Name)
               .IsRequired();

        builder.HasMany(brand => brand.Products)
               .WithOne(product => product.Brand)
               .HasForeignKey(product => product.BrandId);
    }
}