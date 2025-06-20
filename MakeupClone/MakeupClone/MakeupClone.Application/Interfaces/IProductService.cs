﻿using MakeupClone.Domain.Common;
using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Filters;

namespace MakeupClone.Application.Interfaces;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken);

    Task<PagedResult<Product>> GetProductsByFilterAsync(ProductFilter filter, CancellationToken cancellationToken);
}