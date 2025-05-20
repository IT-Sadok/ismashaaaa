using AutoMapper;
using MakeupClone.Application.DTOs.Products;
using MakeupClone.Domain.Entities;
using MakeupClone.Infrastructure.Data.Entities;

namespace MakeupClone.Infrastructure.Data.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductEntity>().ReverseMap();
        CreateMap<Category, CategoryEntity>().ReverseMap();
        CreateMap<Brand, BrandEntity>().ReverseMap();
        CreateMap<Review, ReviewEntity>().ReverseMap();

        CreateMap<ProductCreateDto, Product>();
        CreateMap<ProductUpdateDto, Product>();
        CreateMap<Product, ProductDto>();
    }
}