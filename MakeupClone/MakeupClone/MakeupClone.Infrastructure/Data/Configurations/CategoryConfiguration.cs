using MakeupClone.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MakeupClone.Infrastructure.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<CategoryEntity>
{
    public void Configure(EntityTypeBuilder<CategoryEntity> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(category => category.Id);

        builder.Property(category => category.Name)
               .IsRequired();

        builder.HasMany(category => category.Products)
               .WithOne(product => product.Category)
               .HasForeignKey(product => product.CategoryId);
    }
}