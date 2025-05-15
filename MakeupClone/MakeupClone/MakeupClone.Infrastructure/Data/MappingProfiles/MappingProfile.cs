using AutoMapper;
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
    }
}