using MakeupClone.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MakeupClone.Infrastructure.Data.Configurations;

public class DeliveryInformationConfiguration : IEntityTypeConfiguration<DeliveryInformationEntity>
{
    public void Configure(EntityTypeBuilder<DeliveryInformationEntity> builder)
    {
        builder.HasKey(delivery => delivery.Id);

        builder.Property(delivery => delivery.DeliveryMethod)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(delivery => delivery.Region)
            .IsRequired();

        builder.Property(delivery => delivery.City)
            .IsRequired();

        builder.Property(delivery => delivery.Address)
            .IsRequired();

        builder.Property(delivery => delivery.PostalCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(delivery => delivery.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(delivery => delivery.TrackingNumber)
            .HasMaxLength(50);
    }
}