using MakeupClone.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MakeupClone.Infrastructure.Data.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<ReviewEntity>
{
    public void Configure(EntityTypeBuilder<ReviewEntity> builder)
    {
        builder.ToTable("Reviews");

        builder.HasKey(review => review.Id);

        builder.Property(review => review.ProductId)
           .IsRequired();

        builder.Property(review => review.UserId)
            .IsRequired();

        builder.Property(review => review.Content)
            .IsRequired();

        builder.Property(review => review.Rating)
            .IsRequired();

        builder.Property(review => review.DateCreated)
            .IsRequired();

        builder.HasOne(review => review.Product)
            .WithMany(product => product.Reviews)
            .HasForeignKey(review => review.ProductId);

        builder.HasOne(review => review.User)
            .WithMany()
            .HasForeignKey(review => review.UserId);
    }
}