using Flurl.Http.Testing;
using MakeupClone.Application.Interfaces;
using MakeupClone.Domain.Enums;
using MakeupClone.Tests.Builders;
using MakeupClone.Tests.Common;
using Microsoft.Extensions.DependencyInjection;

namespace MakeupClone.Tests.Delivery;

public class UkrPoshtaProviderTests : IAsyncLifetime
{
    private readonly ServiceProvider _serviceProvider;
    private readonly IDeliveryProvider _deliveryProvider;
    private HttpTest _http;

    public UkrPoshtaProviderTests()
    {
        _serviceProvider = TestServiceProviderFactory.Create();
        _deliveryProvider = _serviceProvider
            .GetServices<IDeliveryProvider>()
            .First(deliveryProvider => deliveryProvider.DeliveryType == DeliveryType.UkrPoshta);
    }

    public Task InitializeAsync()
    {
        _http = new HttpTest();
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        _http.Dispose();
        _serviceProvider.Dispose();
        return Task.CompletedTask;
    }

    [Fact]
    public async Task CreateDeliveryAsync_WithValid_ShouldReturnBarcode()
    {
        _http.ForCallsTo("*shipments").RespondWithJson(new { Barcode = "UKR-789" });

        var deliveryRequestDto = DeliveryRequestBuilder.BuildDefaultRequest();

        var result = await _deliveryProvider.CreateDeliveryAsync(deliveryRequestDto, CancellationToken.None);
        Assert.Equal("UKR-789", result);
    }

    [Fact]
    public async Task CreateDeliveryAsync_WithNoBarcode_ShouldThrowInvalidOperation()
    {
        _http.ForCallsTo("*shipments").RespondWithJson(new { Barcode = " " });

        var deliveryRequestDto = DeliveryRequestBuilder.BuildDefaultRequest();

        await Assert.ThrowsAsync<InvalidOperationException>(() => _deliveryProvider.CreateDeliveryAsync(deliveryRequestDto, CancellationToken.None));
    }

    [Fact]
    public async Task TrackDeliveryAsync_WithValid_ShouldReturnInformation()
    {
        var trackingNumber = "UKR-456";

        _http.ForCallsTo($"*shipments/{trackingNumber}/tracking").RespondWithJson(new
        {
            Status = "Delivered",
            EstimatedDeliveryDate = "2025-07-10T00:00:00Z"
        });

        var deliveryTrackingInformation = await _deliveryProvider.TrackDeliveryAsync(trackingNumber, CancellationToken.None);
        Assert.Equal(trackingNumber, deliveryTrackingInformation.TrackingNumber);
        Assert.Equal("Delivered", deliveryTrackingInformation.Status);
    }
}