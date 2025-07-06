using MakeupClone.Domain.Enums;

namespace MakeupClone.Infrastructure.Data.Entities;

public class DeliveryInformationEntity
{
    public Guid Id { get; set; }

    public DeliveryType DeliveryMethod { get; set; }

    public string Region { get; set; }

    public string City { get; set; }

    public string Address { get; set; }

    public string PostalCode { get; set; }

    public string PhoneNumber { get; set; }

    public string? TrackingNumber { get; set; }

    public OrderEntity Order { get; set; }
}