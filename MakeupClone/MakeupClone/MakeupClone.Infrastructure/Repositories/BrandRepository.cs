using AutoMapper;
using AutoMapper.QueryableExtensions;
using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Filters;
using MakeupClone.Domain.Interfaces;
using MakeupClone.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MakeupClone.Infrastructure.Repositories;

public class BrandRepository : IBrandRepository
{
    private readonly MakeupCloneDbContext _dbContext;
    private readonly IMapper _mapper;

    public BrandRepository(MakeupCloneDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Brand>> GetByFilterAsync(PagingAndSortingFilter filter)
    {
        var query = _dbContext.Brands
            .AsQueryable();

        query = query.Skip(filter.Skip).Take(filter.Take);

        var brands = await query
            .ProjectTo<Brand>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync();

        return brands;
    }
}