using MakeupClone.Application.Interfaces;
using MakeupClone.Application.Services;
using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Exceptions;
using MakeupClone.Infrastructure.Data;
using MakeupClone.Infrastructure.Data.Entities;
using MakeupClone.Tests.Common;
using MakeupClone.Tests.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MakeupClone.Tests.Services;

public class AdminProductServiceTests : IAsyncLifetime
{
    private readonly MakeupCloneDbContext _dbContext;
    private readonly IProductRepository _productRepository;
    private readonly AdminProductService _adminProductService;
    private readonly ServiceProvider _serviceProvider;

    public AdminProductServiceTests()
    {
        _serviceProvider = TestServiceProviderFactory.Create();
        _dbContext = _serviceProvider.GetRequiredService<MakeupCloneDbContext>();
        _productRepository = _serviceProvider.GetRequiredService<IProductRepository>();

        _adminProductService = new AdminProductService(_productRepository);
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

    private async Task InitializeDataAsync(MakeupCloneDbContext dbContext)
    {
        var category = new CategoryEntity { Id = Guid.NewGuid(), Name = "TestCategory" };
        dbContext.Categories.Add(category);

        var brand = new BrandEntity { Id = Guid.NewGuid(), Name = "TestBrand" };
        dbContext.Brands.Add(brand);

        var products = new[]
        {
            new ProductEntity
            {
                Id = TestConstants.DefaultFirstTestId,
                Name = "Test Product 1",
                Description = "Description for Product 1",
                Price = 100,
                StockQuantity = 5,
                ImageUrl = "https://example.com/product1.jpg",
                CategoryId = category.Id,
                Category = category,
                BrandId = brand.Id,
                Brand = brand
            },
            new ProductEntity
            {
                Id = TestConstants.DefaultSecondTestId,
                Name = "Test Product 2",
                Description = "Description for Product 2",
                Price = 150,
                StockQuantity = 10,
                ImageUrl = "https://example.com/product2.jpg",
                CategoryId = category.Id,
                Category = category,
                BrandId = brand.Id,
                Brand = brand
            }
        };

        dbContext.Products.AddRange(products);
        await dbContext.SaveChangesAsync();
    }

    [Fact]
    public async Task GetProductByIdAsync_WithExistingId_ShouldReturnProduct()
    {
        var existingProductId = TestConstants.DefaultSecondTestId;

        var result = await _adminProductService.GetProductByIdAsync(existingProductId, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(existingProductId, result.Id);
    }

    [Fact]
    public async Task GetProductByIdAsync_WithNonExistingId_ShouldThrowNotFound()
    {
        var productId = Guid.NewGuid();

        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _adminProductService.GetProductByIdAsync(productId, CancellationToken.None));

        Assert.Equal($"Product with ID {productId} not found.", exception.Message);
    }

    [Fact]
    public async Task GetAllProductsAsync_ShouldReturnAllProducts()
    {
        var result = await _adminProductService.GetAllProductsAsync(CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task AddProductAsync_WithValidProduct_ShouldAddSuccessfully()
    {
        var brand = await _dbContext.Brands.FirstAsync();
        var category = await _dbContext.Categories.FirstAsync();

        _dbContext.Entry(brand).State = EntityState.Detached;
        _dbContext.Entry(category).State = EntityState.Detached;

        var newProduct = new Product
        {
            Id = Guid.NewGuid(),
            Name = "New Product",
            Description = "New Description",
            Price = 150,
            StockQuantity = 10,
            ImageUrl = "https://example.com/addProduct.jpg",
            BrandId = brand.Id,
            CategoryId = category.Id
        };

        await _adminProductService.AddProductAsync(newProduct, CancellationToken.None);

        var result = await _dbContext.Products.FindAsync(newProduct.Id);

        Assert.NotNull(result);
        Assert.Equal("New Product", result.Name);
    }

    [Fact]
    public async Task UpdateProductAsync_WithExistingProduct_ShouldUpdateSuccessfully()
    {
        var existingProduct = await _dbContext.Products.FirstAsync();

        var updatedProduct = new Product
        {
            Id = existingProduct.Id,
            Name = "Updated Name",
            Description = existingProduct.Description,
            Price = existingProduct.Price,
            StockQuantity = existingProduct.StockQuantity,
            ImageUrl = existingProduct.ImageUrl,
            BrandId = existingProduct.BrandId,
            CategoryId = existingProduct.CategoryId
        };

        await _adminProductService.UpdateProductAsync(updatedProduct, CancellationToken.None);

        var result = await _dbContext.Products.FindAsync(updatedProduct.Id);

        Assert.NotNull(result);
        Assert.Equal(updatedProduct.Name, result.Name);
    }

    [Fact]
    public async Task UpdateProductAsync_WithNonExistingProduct_ShouldThrowNotFound()
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "DoesNotExist",
            Description = "Test",
            Price = 10,
            StockQuantity = 1,
            ImageUrl = "https://example.com/updateProduct.jpg",
            BrandId = Guid.NewGuid(),
            CategoryId = Guid.NewGuid()
        };

        var result = await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _adminProductService.UpdateProductAsync(product, CancellationToken.None));

        Assert.Equal($"Product with ID {product.Id} not found.", result.Message);
    }

    [Fact]
    public async Task DeleteProductAsync_WithValidId_ShouldDeleteSuccessfully()
    {
        var productId = TestConstants.DefaultFirstTestId;

        await _adminProductService.DeleteProductAsync(productId, CancellationToken.None);

        var result = await _dbContext.Products.FindAsync(productId);

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteProductAsync_WithNonExistingId_ShouldThrowNotFound()
    {
        var productId = Guid.NewGuid();

        var result = await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _adminProductService.DeleteProductAsync(productId, CancellationToken.None));

        Assert.Equal($"Product with ID {productId} not found.", result.Message);
    }

    [Fact]
    public async Task AddDiscountAsync_WithValidProductId_ShouldApplyDiscount()
    {
        var productId = TestConstants.DefaultFirstTestId;
        decimal discount = 20;

        await _adminProductService.AddDiscountAsync(productId, discount, CancellationToken.None);
        var result = await _dbContext.Products.FindAsync(productId);

        Assert.NotNull(result);
        Assert.Equal(discount, result.DiscountPercentage);
    }

    [Fact]
    public async Task AddDiscountAsync_WithNonExistingProduct_ShouldThrowNotFound()
    {
        var productId = Guid.NewGuid();
        decimal discount = 15;

        var result = await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _adminProductService.AddDiscountAsync(productId, discount, CancellationToken.None));

        Assert.Equal($"Product with ID {productId} not found.", result.Message);
    }

    [Fact]
    public async Task UpdateDiscountAsync_WithValidProductId_ShouldUpdateDiscount()
    {
        var productId = TestConstants.DefaultFirstTestId;
        decimal initialDiscount = 10;
        decimal updatedDiscount = 25;

        await _adminProductService.AddDiscountAsync(productId, initialDiscount, CancellationToken.None);

        await _adminProductService.UpdateDiscountAsync(productId, updatedDiscount, CancellationToken.None);
        var result = await _dbContext.Products.FindAsync(productId);

        Assert.NotNull(result);
        Assert.Equal(updatedDiscount, result.DiscountPercentage);
    }

    [Fact]
    public async Task UpdateDiscountAsync_WithNonExistingProduct_ShouldThrowNotFound()
    {
        var productId = Guid.NewGuid();
        decimal discount = 25;

        var result = await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _adminProductService.UpdateDiscountAsync(productId, discount, CancellationToken.None));

        Assert.Equal($"Product with ID {productId} not found.", result.Message);
    }

    [Fact]
    public async Task RemoveDiscountAsync_WithValidProductId_ShouldRemoveDiscount()
    {
        var productId = TestConstants.DefaultSecondTestId;
        decimal discount = 30;
        await _adminProductService.AddDiscountAsync(productId, discount, CancellationToken.None);

        await _adminProductService.RemoveDiscountAsync(productId, CancellationToken.None);
        var result = await _dbContext.Products.FindAsync(productId);

        Assert.NotNull(result);
        Assert.Null(result.DiscountPercentage);
    }

    [Fact]
    public async Task RemoveDiscountAsync_WithNonExistingProduct_ShouldThrowNotFound()
    {
        var productId = Guid.NewGuid();

        var result = await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _adminProductService.RemoveDiscountAsync(productId, CancellationToken.None));

        Assert.Equal($"Product with ID {productId} not found.", result.Message);
    }
}