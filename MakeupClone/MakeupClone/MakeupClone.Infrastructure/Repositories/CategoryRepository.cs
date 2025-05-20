using AutoMapper;
using AutoMapper.QueryableExtensions;
using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Filters;
using MakeupClone.Domain.Interfaces;
using MakeupClone.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MakeupClone.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly MakeupCloneDbContext _dbContext;
    private readonly IMapper _mapper;

    public CategoryRepository(MakeupCloneDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<(IEnumerable<Category> Items, int TotalCount)> GetByFilterAsync(PagingAndSortingFilter filter, CancellationToken cancellationToken)
    {
        var query = _dbContext.Categories
           .AsQueryable();

        var totalCount = await query.CountAsync(cancellationToken);

        query = query.Skip(filter.Skip).Take(filter.Take);

        var categories = await query
            .ProjectTo<Category>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return (categories, totalCount);
    }
}