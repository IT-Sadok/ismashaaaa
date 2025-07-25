﻿using MakeupClone.Domain.Common;
using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Filters;

namespace MakeupClone.Application.Interfaces;

public interface IBrandService
{
    Task<PagedResult<Brand>> GetBrandsByFilterAsync(PagingAndSortingFilter filter, CancellationToken cancellationToken);
}