using AutoMapper;
using MakeupClone.Application.DTOs.Products;
using MakeupClone.Domain.Entities;
using MakeupClone.Infrastructure.Data.Entities;

namespace MakeupClone.Infrastructure.Data.MappingProfiles;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Product, ProductEntity>()
            .ForMember(productEntity => productEntity.Brand, mappingOptions => mappingOptions.Ignore())
            .ForMember(productEntity => productEntity.Category, mappingOptions => mappingOptions.Ignore())
            .ReverseMap();

        CreateMap<ProductCreateDto, Product>();
        CreateMap<ProductUpdateDto, Product>();
        CreateMap<Product, ProductDto>();
    }
}