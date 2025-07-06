using Flurl.Http.Testing;
using MakeupClone.Application.Interfaces;
using MakeupClone.Domain.Enums;
using MakeupClone.Tests.Builders;
using MakeupClone.Tests.Common;
using Microsoft.Extensions.DependencyInjection;

namespace MakeupClone.Tests.Delivery;

public class MeestExpressProviderTests : IAsyncLifetime
{
    private readonly ServiceProvider _serviceProvider;
    private readonly IDeliveryProvider _deliveryProvider;
    private HttpTest _http;

    public MeestExpressProviderTests()
    {
        _serviceProvider = TestServiceProviderFactory.Create();
        _deliveryProvider = _serviceProvider
            .GetServices<IDeliveryProvider>()
            .First(deliveryProvider => deliveryProvider.DeliveryType == DeliveryType.MeestExpress);
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
    public async Task CreateDeliveryAsync_WithValidResponse_ShouldReturnBarcode()
    {
        var trackingNumber = "MEEST-123";

        _http.ForCallsTo("*shipments").RespondWithJson(new { Barcode = trackingNumber });

        var deliveryRequestDto = DeliveryRequestBuilder.BuildDefaultRequest();

        var barcode = await _deliveryProvider.CreateDeliveryAsync(deliveryRequestDto, CancellationToken.None);
        Assert.Equal(trackingNumber, barcode);
    }

    [Fact]
    public async Task CreateDeliveryAsync_WithEmptyBarcode_ShouldThrowInvalidOperation()
    {
        _http.ForCallsTo("*shipments").RespondWithJson(new { Barcode = " " });

        var deliveryRequestDto = DeliveryRequestBuilder.BuildDefaultRequest();

        await Assert.ThrowsAsync<InvalidOperationException>(() => _deliveryProvider.CreateDeliveryAsync(deliveryRequestDto, CancellationToken.None));
    }

    [Fact]
    public async Task TrackDeliveryAsync_WithValidResponse_ShouldReturnInformation()
    {
        var trackingNumber = "MEEST-456";

        _http.ForCallsTo($"*shipments/{trackingNumber}/tracking").RespondWithJson(new
        {
            Status = "InTransit",
            EstimatedDeliveryDate = "2025-07-01T00:00:00Z"
        });

        var deliveryTrackingInformation = await _deliveryProvider.TrackDeliveryAsync(trackingNumber, CancellationToken.None);

        Assert.Equal(trackingNumber, deliveryTrackingInformation.TrackingNumber);
        Assert.Equal("InTransit", deliveryTrackingInformation.Status);
    }
}