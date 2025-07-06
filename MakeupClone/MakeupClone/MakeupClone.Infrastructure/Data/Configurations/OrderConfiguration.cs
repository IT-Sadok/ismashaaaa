using MakeupClone.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MakeupClone.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.HasKey(order => order.Id);

        builder.Property(order => order.CreatedAt)
            .IsRequired();

        builder.Property(order => order.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.HasMany(order => order.Items)
            .WithOne(item => item.Order)
            .HasForeignKey(item => item.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(order => order.PaymentInformation)
            .WithOne()
            .HasForeignKey<OrderEntity>(order => order.PaymentInformationId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(order => order.DeliveryInformation)
             .WithOne()
             .HasForeignKey<OrderEntity>(order => order.DeliveryInformationId)
             .OnDelete(DeleteBehavior.SetNull);
    }
}