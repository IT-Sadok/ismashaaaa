using MakeupClone.Domain.Filters;
using MakeupClone.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MakeupClone.API.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("filter")]
    public async Task<IActionResult> GetFilteredProducts([FromQuery] ProductFilter productFilter)
    {
        var filteredResult = await _productService.GetProductsByFilterAsync(productFilter);

        return Ok(filteredResult);
    }
}