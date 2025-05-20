using FluentValidation;

using Google.Apis.Auth;

using MakeupClone.Application.DTOs.Auth;
using MakeupClone.Application.Interfaces;
using MakeupClone.Application.Services;
using MakeupClone.Application.Validators;
using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Interfaces;
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
    private readonly IValidator<RegisterDto> _registrationValidator;
    private readonly IValidator<LoginDto> _loginValidator;
    private readonly MakeupCloneDbContext _dbContext;
    private readonly ServiceProvider _serviceProvider;

    public AuthServiceTests()
    {
        _serviceProvider = TestServiceProviderFactory.Create();

        _dbContext = _serviceProvider.GetRequiredService<MakeupCloneDbContext>();
        _userManager = _serviceProvider.GetRequiredService<UserManager<User>>();
        _jwtTokenGenerator = _serviceProvider.GetRequiredService<IJwtTokenGenerator>();
        _registrationValidator = new RegisterValidator();
        _loginValidator = new LoginValidator();

        _googleJsonWebSignatureWrapper = _serviceProvider.GetRequiredService<Mock<IGoogleJsonWebSignatureWrapper>>();
        var googleWrapper = _googleJsonWebSignatureWrapper.Object;

        _authService = new AuthService(
            _userManager,
            _jwtTokenGenerator,
            _registrationValidator,
            _loginValidator,
            googleWrapper);
    }

    public async Task InitializeAsync()
    {
        await InitializeData(_dbContext);
    }

    public Task DisposeAsync()
    {
        TestDbContextFactory.ClearDatabase(_dbContext);

        return Task.CompletedTask;
    }

    private async Task InitializeData(MakeupCloneDbContext context)
    {
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
    public async Task Register_WithValidData_ShouldReturnToken()
    {
        var registerDto = CreateRegisterDto(email: Guid.NewGuid() + "@example.com");

        var result = await _authService.RegisterAsync(registerDto, CancellationToken.None);

        Assert.True(result.Success);

        Assert.False(string.IsNullOrWhiteSpace(result.Token));
    }

    [Fact]
    public async Task Register_WithInvalidEmail_ShouldReturnValidationError()
    {
        var registerDto = CreateRegisterDto(email: "invalid-email");

        var result = await _authService.RegisterAsync(registerDto, CancellationToken.None);

        Assert.False(result.Success);
        Assert.Contains("Invalid email format.", result.Errors);
    }

    [Fact]
    public async Task Register_WithShortPassword_ShouldReturnValidationError()
    {
        var registerDto = CreateRegisterDto(password: "123");

        var result = await _authService.RegisterAsync(registerDto, CancellationToken.None);

        Assert.False(result.Success);
        Assert.Contains("Password must be at least 6 characters long.", result.Errors.FirstOrDefault());
    }

    [Fact]
    public async Task Register_WithExistingEmail_ShouldReturnIdentityError()
    {
        var registerDto = CreateRegisterDto(email: "existing@example.com");

        var result = await _authService.RegisterAsync(registerDto, CancellationToken.None);

        Assert.False(result.Success);
        Assert.Contains("is already taken", result.Errors.FirstOrDefault() ?? " ");
    }

    [Fact]
    public async Task Register_WithInvalidPhone_ShouldReturnValidationError()
    {
        var registerDto = CreateRegisterDto(phoneNumber: "123abc");

        var result = await _authService.RegisterAsync(registerDto, CancellationToken.None);

        Assert.False(result.Success);
        Assert.Contains("Invalid phone number format.", result.Errors);
    }

    [Fact]
    public async Task Register_WithNullFields_ShouldReturnValidationErrors()
    {
        var registerDto = new RegisterDto();

        var result = await _authService.RegisterAsync(registerDto, CancellationToken.None);

        Assert.False(result.Success);
        Assert.Contains("First name is required.", result.Errors);
        Assert.Contains("Last name is required.", result.Errors);
        Assert.Contains("Email is required.", result.Errors);
        Assert.Contains("Password is required.", result.Errors);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ShouldReturnToken()
    {
        var loginDto = CreateLoginDto(email: "existing@example.com", password: "Test1234!");

        var result = await _authService.LoginAsync(loginDto, CancellationToken.None);

        Assert.True(result.Success);
        Assert.False(string.IsNullOrWhiteSpace(result.Token));
    }

    [Fact]
    public async Task Login_WithInvalidEmail_ShouldReturnError()
    {
        var loginDto = CreateLoginDto(email: "invalid-email");

        var result = await _authService.LoginAsync(loginDto, CancellationToken.None);

        Assert.False(result.Success);
        Assert.Contains("Invalid email format.", result.Errors);
    }

    [Fact]
    public async Task Login_WithWrongPassword_ShouldReturnError()
    {
        var loginDto = CreateLoginDto(email: "existing@example.com", password: "WrongPass123!");

        var result = await _authService.LoginAsync(loginDto, CancellationToken.None);

        Assert.False(result.Success);
        Assert.Contains("Invalid email or password", result.Errors.FirstOrDefault());
    }

    [Fact]
    public async Task Login_WithMissingFields_ShouldReturnValidationErrors()
    {
        var loginDto = new LoginDto();

        var result = await _authService.LoginAsync(loginDto, CancellationToken.None);

        Assert.False(result.Success);
        Assert.Contains("Email is required.", result.Errors);
        Assert.Contains("Password is required.", result.Errors);
    }

    [Fact]
    public async Task Login_WithNotExistingUser_ShouldReturnError()
    {
        var loginDto = CreateLoginDto(email: "notfound@example.com");

        var result = await _authService.LoginAsync(loginDto, CancellationToken.None);

        Assert.False(result.Success);
        Assert.Contains("Invalid email or password", result.Errors.FirstOrDefault());
    }

    [Fact]
    public async Task GoogleLogin_WithValidTokenAndExistingUser_ShouldReturnToken()
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
    public async Task GoogleLogin_WithValidTokenAndNewUser_ShouldCreateUserAndReturnToken()
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