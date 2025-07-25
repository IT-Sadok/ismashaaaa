﻿using MakeupClone.Domain.Enums;

namespace MakeupClone.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }

    public string UserId { get; set; }

    public User User { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public ICollection<OrderItem> Items { get; set; }

    public Guid? PaymentInformationId { get; set; }

    public PaymentInformation? PaymentInformation { get; set; }

    public Guid? DeliveryInformationId { get; set; }

    public DeliveryInformation? DeliveryInformation { get; set; }
}