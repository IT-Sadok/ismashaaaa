using MakeupClone.Application.Interfaces;
using MakeupClone.Domain.Enums;
using MakeupClone.Tests.Common;
using Microsoft.Extensions.DependencyInjection;

namespace MakeupClone.Tests.Payment;

public class StripePaymentServiceTests : IAsyncLifetime
{
    private readonly ServiceProvider _serviceProvider;
    private readonly IPaymentService _paymentService;

    public StripePaymentServiceTests()
    {
        _serviceProvider = TestServiceProviderFactory.Create();
        _paymentService = _serviceProvider.GetRequiredService<IPaymentService>();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync()
    {
        _serviceProvider.Dispose();
        return Task.CompletedTask;
    }

    [Fact]
    public async Task CreatePaymentIntentAsync_WithValidData_ShouldReturnClientSecret()
    {
        var clientSecret = await _paymentService.CreatePaymentIntentAsync(100, "usd", PaymentType.Visa, CancellationToken.None);
        Assert.False(string.IsNullOrWhiteSpace(clientSecret));
    }

    [Fact]
    public async Task CreatePaymentIntentAsync_WithUnsupportedMethod_ShouldThrowArgumentOutOfRange()
    {
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            _paymentService.CreatePaymentIntentAsync(100, "usd", (PaymentType)999, CancellationToken.None));
    }

    [Fact]
    public async Task ConfirmPaymentAsync_WithInvalidId_ShouldReturnFailed()
    {
        var paymentStatus = await _paymentService.ConfirmPaymentAsync("pi_invalid", CancellationToken.None);
        Assert.Equal(PaymentStatus.Failed, paymentStatus);
    }
}