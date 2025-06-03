using AutoMapper;
using AutoMapper.QueryableExtensions;
using MakeupClone.Application.Interfaces;
using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Filters;
using MakeupClone.Infrastructure.Data;
using MakeupClone.Infrastructure.Data.Entities;
using MakeupClone.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MakeupClone.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly MakeupCloneDbContext _dbContext;
    private readonly IMapper _mapper;

    public ProductRepository(MakeupCloneDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products
            .Include(product => product.Category)
            .Include(product => product.Brand)
            .AsNoTracking()
            .FirstOrDefaultAsync(product => product.Id == id, cancellationToken);

        return _mapper.Map<Product>(product);
    }

    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Products
            .Include(product => product.Category)
            .Include(product => product.Brand)
            .ProjectTo<Product>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken)
    {
        var productEntity = _mapper.Map<ProductEntity>(product);

        await _dbContext.Products.AddAsync(productEntity, cancellationToken);
    }

    public async Task UpdateAsync(Product product, CancellationToken cancellationToken)
    {
        var existingProduct = await _dbContext.Products
            .Include(productDb => productDb.Category)
            .Include(productDb => productDb.Brand)
            .FirstOrDefaultAsync(products => products.Id == product.Id, cancellationToken);

        if (existingProduct == null)
            return;

        _mapper.Map(product, existingProduct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var existingProduct = await _dbContext.Products.FindAsync(id, cancellationToken);

        if (existingProduct == null)
            return;

        _dbContext.Products.Remove(existingProduct);
    }

    public async Task<(IEnumerable<Product> Items, int TotalCount)> GetByFilterAsync(ProductFilter filter, CancellationToken cancellationToken)
    {
        IQueryable<ProductEntity> query = _dbContext.Products
            .Include(product => product.Category)
            .Include(product => product.Brand);

        query = ApplyFilters(query, filter);

        var totalCount = await query.CountAsync(cancellationToken);

        if (!string.IsNullOrEmpty(filter.OrderBy) && AllowedSortFields.Contains(filter.OrderBy))
        {
            if (filter.OrderBy == nameof(Product.Category))
            {
                query = filter.Descending
                    ? query.OrderByDescending(product => product.Category.Name)
                    : query.OrderBy(product => product.Category.Name);
            }
            else if (filter.OrderBy == nameof(Product.Brand))
            {
                query = filter.Descending
                    ? query.OrderByDescending(product => product.Brand.Name)
                    : query.OrderBy(product => product.Brand.Name);
            }
            else
            {
                query = filter.Descending
                    ? query.OrderByDescending(product => EF.Property<object>(product, filter.OrderBy))
                    : query.OrderBy(product => EF.Property<object>(product, filter.OrderBy));
            }
        }

        query = query.Skip(filter.Skip).Take(filter.Take);

        var products = await query
            .ProjectTo<Product>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return (products, totalCount);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private static readonly HashSet<string> AllowedSortFields = new ()
    {
        nameof(Product.Category),
        nameof(Product.Brand),
        nameof(Product.Name),
        nameof(Product.Price)
    };

    private static IQueryable<ProductEntity> ApplyFilters(IQueryable<ProductEntity> query, ProductFilter filter)
    {
        query = query
            .WhereIf(filter.CategoryId.HasValue, product => product.CategoryId == filter.CategoryId.Value)
            .WhereIf(filter.BrandId.HasValue, product => product.BrandId == filter.BrandId.Value)
            .WhereIf(!string.IsNullOrEmpty(filter.ProductName), product => product.Name.ToLower().Contains(filter.ProductName.ToLower()))
            .WhereIf(filter.MinimumPrice.HasValue, product => product.Price >= filter.MinimumPrice.Value)
            .WhereIf(filter.MaximumPrice.HasValue, product => product.Price <= filter.MaximumPrice.Value);

        return query;
    }
}