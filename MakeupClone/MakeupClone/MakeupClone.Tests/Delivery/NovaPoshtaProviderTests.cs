using MakeupClone.Application.DTOs.Delivery;
using MakeupClone.Application.Interfaces;
using MakeupClone.Domain.Enums;
using MakeupClone.Tests.Builders;
using MakeupClone.Tests.Common;
using Microsoft.Extensions.DependencyInjection;

namespace MakeupClone.Tests.Delivery;

public class NovaPoshtaProviderTests : IAsyncLifetime
{
    private readonly ServiceProvider _serviceProvider;
    private readonly IDeliveryProvider _deliveryProvider;

    public NovaPoshtaProviderTests()
    {
        _serviceProvider = TestServiceProviderFactory.Create();
        _deliveryProvider = _serviceProvider
            .GetServices<IDeliveryProvider>()
            .First(deliveryProvider => deliveryProvider.DeliveryType == DeliveryType.NovaPoshta);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync()
    {
        _serviceProvider.Dispose();

        return Task.CompletedTask;
    }

    [Fact]
    public async Task CreateDeliveryAsync_WithValidRequest_ShouldReturnTrackingNumber()
    {
        var deliveryRequestDto = new DeliveryRequestDto
        {
            City = "Київ",
            Address = "Відділення №1",
            RecipientsPhoneNumber = "+380954282549",
            SendersPhoneNumber = "+380507654321",
            WeightKg = 2,
            Region = "Київська",
            PostalCode = "01001",
            DeclaredPrice = 300,
            RecipientName = "Іван Петров",
            DeliveryPaymentMethod = "Cash",
            Payer = PayerType.Recipient,
            SeatsAmount = "1",
            Description = "Тестове замовлення"
        };
        var result = await _deliveryProvider.CreateDeliveryAsync(deliveryRequestDto, CancellationToken.None);
        Assert.False(string.IsNullOrWhiteSpace(result));
    }

    [Fact]
    public async Task CreateDeliveryAsync_WithInvalidRequest_ShouldThrowInvalidOperation()
    {
        var deliveryRequestDto = DeliveryRequestBuilder.BuildDefaultRequest();

        await Assert.ThrowsAsync<InvalidOperationException>(() => _deliveryProvider.CreateDeliveryAsync(deliveryRequestDto, CancellationToken.None));
    }

    [Fact]
    public async Task TrackDeliveryAsync_WithInvalidNumber_ShouldThrowInvalidOperation()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(() => _deliveryProvider.TrackDeliveryAsync("XYZ123", CancellationToken.None));
    }
}