using Google.Apis.Auth;
using MakeupClone.Application.DTOs.Auth;
using MakeupClone.Application.Interfaces;
using MakeupClone.Application.Services;
using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Enums;
using MakeupClone.Infrastructure.Data;
using MakeupClone.Tests.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace MakeupClone.Tests.Services;

public class AuthServiceTests : IAsyncLifetime
{
    private readonly AuthService _authService;
    private readonly UserManager<User> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly Mock<IGoogleJsonWebSignatureWrapper> _googleJsonWebSignatureWrapper;
    private readonly MakeupCloneDbContext _dbContext;
    private readonly ServiceProvider _serviceProvider;

    public AuthServiceTests()
    {
        _serviceProvider = TestServiceProviderFactory.Create();

        _dbContext = _serviceProvider.GetRequiredService<MakeupCloneDbContext>();
        _userManager = _serviceProvider.GetRequiredService<UserManager<User>>();
        _jwtTokenGenerator = _serviceProvider.GetRequiredService<IJwtTokenGenerator>();

        _googleJsonWebSignatureWrapper = _serviceProvider.GetRequiredService<Mock<IGoogleJsonWebSignatureWrapper>>();
        var googleWrapper = _googleJsonWebSignatureWrapper.Object;

        _authService = new AuthService(
            _userManager,
            _jwtTokenGenerator,
            googleWrapper);
    }

    public async Task InitializeAsync()
    {
        await InitializeDataAsync(_dbContext);
    }

    public Task DisposeAsync()
    {
        TestDbContextFactory.ClearDatabase(_dbContext);

        return Task.CompletedTask;
    }

    private async Task InitializeDataAsync(MakeupCloneDbContext context)
    {
        var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        if (!await roleManager.RoleExistsAsync(Roles.User))
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.User));
        }

        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "existing@example.com",
            Email = "existing@example.com",
            FirstName = "Existing",
            LastName = "User",
            BirthDate = DateTime.UtcNow.AddYears(-25),
            ReceiveNotifications = true
        };

        await _userManager.CreateAsync(user, "Test1234!");
        context.SaveChanges();
    }

    [Fact]
    public async Task RegisterAsync_WithValidData_ShouldReturnToken()
    {
        var registerDto = CreateRegisterDto(email: Guid.NewGuid() + "@example.com");

        var result = await _authService.RegisterAsync(registerDto, CancellationToken.None);

        Assert.True(result.Success);

        Assert.False(string.IsNullOrWhiteSpace(result.Token));
    }

    [Fact]
    public async Task RegisterAsync_WithExistingEmail_ShouldReturnIdentityError()
    {
        var registerDto = CreateRegisterDto(email: "existing@example.com");

        var result = await _authService.RegisterAsync(registerDto, CancellationToken.None);

        Assert.False(result.Success);
        Assert.Contains("is already taken", result.Errors.FirstOrDefault() ?? " ");
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ShouldReturnToken()
    {
        var loginDto = CreateLoginDto(email: "existing@example.com", password: "Test1234!");

        var result = await _authService.LoginAsync(loginDto, CancellationToken.None);

        Assert.True(result.Success);
        Assert.False(string.IsNullOrWhiteSpace(result.Token));
    }

    [Fact]
    public async Task LoginAsync_WithWrongPassword_ShouldReturnError()
    {
        var loginDto = CreateLoginDto(email: "existing@example.com", password: "WrongPass123!");

        var result = await _authService.LoginAsync(loginDto, CancellationToken.None);

        Assert.False(result.Success);
        Assert.Contains("Invalid email or password", result.Errors.FirstOrDefault());
    }

    [Fact]
    public async Task LoginAsync_WithNotExistingUser_ShouldReturnError()
    {
        var loginDto = CreateLoginDto(email: "notfound@example.com");

        var result = await _authService.LoginAsync(loginDto, CancellationToken.None);

        Assert.False(result.Success);
        Assert.Contains("Invalid email or password", result.Errors.FirstOrDefault());
    }

    [Fact]
    public async Task GoogleLoginAsync_WithValidTokenAndExistingUser_ShouldReturnToken()
    {
        var googleLoginDto = CreateGoogleLoginDto("valid_token");
        var payload = new GoogleJsonWebSignature.Payload
        {
            Email = "existing@example.com",
            GivenName = "Existing",
            FamilyName = "User"
        };

        _googleJsonWebSignatureWrapper.Setup(wrapper => wrapper.Validate(googleLoginDto.IdToken, It.IsAny<CancellationToken>())).ReturnsAsync(payload);

        var result = await _authService.GoogleLoginAsync(googleLoginDto, CancellationToken.None);

        Assert.True(result.Success);
        Assert.False(string.IsNullOrWhiteSpace(result.Token));
    }

    [Fact]
    public async Task GoogleLoginAsync_WithValidTokenAndNewUser_ShouldCreateUserAndReturnToken()
    {
        var googleLoginDto = CreateGoogleLoginDto("new_token");
        var payload = new GoogleJsonWebSignature.Payload
        {
            Email = "newgoogle@example.com",
            GivenName = "New",
            FamilyName = "User"
        };

        _googleJsonWebSignatureWrapper.Setup(wrapper => wrapper.Validate(googleLoginDto.IdToken, It.IsAny<CancellationToken>())).ReturnsAsync(payload);

        var result = await _authService.GoogleLoginAsync(googleLoginDto, CancellationToken.None);

        Assert.True(result.Success);
        Assert.False(string.IsNullOrWhiteSpace(result.Token));
    }

    private static RegisterDto CreateRegisterDto(
        string? email = null,
        string? password = null,
        string? confirmPassword = null,
        string? phoneNumber = null)
    {
        return new RegisterDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = email ?? "john.doe@example.com",
            Password = password ?? "StrongPass123!",
            ConfirmPassword = confirmPassword ?? password ?? "StrongPass123!",
            PhoneNumber = phoneNumber ?? "+380931234567",
            BirthDate = DateTime.UtcNow.AddYears(-25),
            ReceiveNotifications = true
        };
    }

    private static LoginDto CreateLoginDto(string? email = null, string? password = null)
    {
        return new LoginDto
        {
            Email = email ?? "existing@example.com",
            Password = password ?? "Test1234!"
        };
    }

    private static GoogleLoginDto CreateGoogleLoginDto(string idToken = "valid_token")
    {
        return new GoogleLoginDto
        {
            IdToken = idToken
        };
    }
}