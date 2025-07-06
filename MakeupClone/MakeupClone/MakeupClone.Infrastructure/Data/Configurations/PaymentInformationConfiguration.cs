using MakeupClone.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MakeupClone.Infrastructure.Data.Configurations;
public class PaymentInformationConfiguration : IEntityTypeConfiguration<PaymentInformationEntity>
{
    public void Configure(EntityTypeBuilder<PaymentInformationEntity> builder)
    {
        builder.HasKey(payment => payment.Id);

        builder.Property(payment => payment.Method)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(payment => payment.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(payment => payment.PaidAt)
            .IsRequired();

        builder.Property(payment => payment.TransactionId)
            .IsRequired();

        builder.Property(payment => payment.Provider)
            .IsRequired();

        builder.Property(payment => payment.Status)
            .HasConversion<int>()
            .IsRequired();
    }
}
