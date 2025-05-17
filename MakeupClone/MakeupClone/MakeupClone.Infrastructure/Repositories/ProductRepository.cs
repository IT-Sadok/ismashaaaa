using AutoMapper;
using AutoMapper.QueryableExtensions;
using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Filters;
using MakeupClone.Domain.Interfaces;
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

    public async Task<ProductFilterResult> GetByFilterAsync(ProductFilter filter)
    {
        var query = _dbContext.Products
            .Include(product => product.Category)
            .Include(product => product.Brand)
            .AsQueryable();

        query = ApplyFilters(query, filter);

        var totalCount = await query.CountAsync();

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
            .ToListAsync();

        return new ProductFilterResult
        {
            Products = products,
            TotalCount = totalCount,
        };
    }

    private static readonly HashSet<string> AllowedSortFields =
    [
        nameof(Product.Category),
        nameof(Product.Brand),
        nameof(Product.Name),
        nameof(Product.Price)
    ];

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