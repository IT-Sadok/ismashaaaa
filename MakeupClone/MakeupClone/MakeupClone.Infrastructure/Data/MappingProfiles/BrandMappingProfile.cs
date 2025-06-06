using AutoMapper;
using MakeupClone.Domain.Entities;
using MakeupClone.Infrastructure.Data.Entities;

namespace MakeupClone.Infrastructure.Data.MappingProfiles;

public class BrandMappingProfile : Profile
{
    public BrandMappingProfile()
    {
        CreateMap<Brand, BrandEntity>().ReverseMap();
    }
}