﻿using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Enums;

namespace MakeupClone.Infrastructure.Data.Entities;

public class OrderEntity
{
    public Guid Id { get; set; }

    public string UserId { get; set; }

    public User User { get; set; }

    public DateTime CreatedAt { get; set; }

    public OrderStatus Status { get; set; }

    public ICollection<OrderItemEntity> Items { get; set; }

    public Guid? PaymentInformationId { get; set; }

    public PaymentInformationEntity? PaymentInformation { get; set; }

    public Guid? DeliveryInformationId { get; set; }

    public DeliveryInformationEntity? DeliveryInformation { get; set; }
}