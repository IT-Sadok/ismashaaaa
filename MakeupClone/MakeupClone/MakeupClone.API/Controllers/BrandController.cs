using MakeupClone.Domain.Filters;
using MakeupClone.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MakeupClone.API.Controllers;

[ApiController]
[Route("api/brands")]
public class BrandController : ControllerBase
{
    private readonly IBrandService _brandService;

    public BrandController(IBrandService brandService)
    {
        _brandService = brandService;
    }

    [HttpGet("filter")]
    public async Task<IActionResult> GetFilteredBrands([FromQuery] PagingAndSortingFilter baseFilter)
    {
        var brands = await _brandService.GetBrandsByFilterAsync(baseFilter);

        return Ok(brands);
    }
}