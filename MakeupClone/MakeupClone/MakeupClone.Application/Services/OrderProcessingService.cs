using MakeupClone.Application.DTOs.Delivery;
using MakeupClone.Application.Interfaces;
using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Enums;

namespace MakeupClone.Application.Services;

public class OrderProcessingService : IOrderProcessingService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentService _paymentService;
    private readonly IDeliveryService _deliveryService;

    public OrderProcessingService(IOrderRepository orderRepository, IPaymentService paymentService, IDeliveryService deliveryService)
    {
        _orderRepository = orderRepository;
        _paymentService = paymentService;
        _deliveryService = deliveryService;
    }

    public async Task<Guid> ProcessOrderAsync(Order order, CancellationToken cancellationToken)
    {
        await SetTrackingNumberAsync(order, cancellationToken);
        var transactionId = await CreatePaymentAsync(order.PaymentInformation, cancellationToken);
        UpdatePaymentDetails(order.PaymentInformation, transactionId, _paymentService.GetProviderName());

        SetupOrder(order);
        LinkOrderRelations(order);
        SetOrderItemsIds(order);

        await SaveOrderAsync(order, cancellationToken);

        return order.Id;
    }

    private async Task SetTrackingNumberAsync(Order order, CancellationToken cancellationToken)
    {
        var deliveryRequestDto = MapToDeliveryRequestDto(order.DeliveryInformation);
        var trackingNumber = await _deliveryService.CreateDeliveryAsync(deliveryRequestDto, cancellationToken);
        order.DeliveryInformation.TrackingNumber = trackingNumber;
    }

    private DeliveryRequestDto MapToDeliveryRequestDto(DeliveryInformation deliveryInformation) => new ()
    {
        DeliveryType = deliveryInformation.DeliveryMethod,
        City = deliveryInformation.City,
        Address = deliveryInformation.Address,
        RecipientsPhoneNumber = deliveryInformation.PhoneNumber,
        WeightKg = (double)deliveryInformation.WeightKg,
        Region = deliveryInformation.Region,
        PostalCode = deliveryInformation.PostalCode,
        RecipientName = deliveryInformation.RecipientName,
        DeclaredPrice = deliveryInformation.DeclaredPrice,
        PaymentMethod = deliveryInformation.PaymentMethod,
        Payer = deliveryInformation.Payer
    };

    private async Task<string> CreatePaymentAsync(PaymentInformation paymentInformation, CancellationToken cancellationToken)
    {
        return await _paymentService.CreatePaymentIntentAsync(paymentInformation.Amount, paymentInformation.Currency, paymentInformation.Method, cancellationToken);
    }

    private void UpdatePaymentDetails(PaymentInformation paymentInformation, string transactionId, string provider)
    {
        paymentInformation.TransactionId = transactionId;
        paymentInformation.Provider = provider;
        paymentInformation.Status = PaymentStatus.Pending;
        paymentInformation.PaidAt = DateTime.UtcNow;
    }

    private void SetupOrder(Order order)
    {
        order.Id = Guid.NewGuid();
        order.CreatedAt = DateTime.UtcNow;
        order.Status = OrderStatus.Pending;
    }

    private void LinkOrderRelations(Order order)
    {
        order.PaymentInformation.Order = order;
        order.PaymentInformationId = order.PaymentInformation.Id;

        order.DeliveryInformation.Order = order;
        order.DeliveryInformationId = order.DeliveryInformation.Id;
    }

    private void SetOrderItemsIds(Order order)
    {
        foreach (var item in order.Items)
        {
            item.Id = Guid.NewGuid();
            item.OrderId = order.Id;
            item.Order = order;
        }
    }

    private async Task SaveOrderAsync(Order order, CancellationToken cancellationToken)
    {
        await _orderRepository.AddAsync(order, cancellationToken);
        await _orderRepository.SaveChangesAsync(cancellationToken);
    }
}