using AutoMapper;
using AutoMapper.QueryableExtensions;
using MakeupClone.Application.Interfaces;
using MakeupClone.Domain.Common;
using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Filters;
using MakeupClone.Infrastructure.Data;
using MakeupClone.Infrastructure.Data.Entities;
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

    public async Task<UnpagedResult<Brand>> GetByFilterAsync(PagingAndSortingFilter filter, CancellationToken cancellationToken)
    {
        IQueryable<BrandEntity> query = _dbContext.Brands;

        var totalCount = await query.CountAsync(cancellationToken);

        query = query.Skip(filter.Skip).Take(filter.Take);

        var brands = await query
            .ProjectTo<Brand>(_mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return new UnpagedResult<Brand>(brands, totalCount);
    }
}