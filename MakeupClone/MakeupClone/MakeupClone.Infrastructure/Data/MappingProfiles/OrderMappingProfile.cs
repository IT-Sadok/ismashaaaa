using AutoMapper;
using MakeupClone.Domain.Entities;
using MakeupClone.Infrastructure.Data.Entities;

namespace MakeupClone.Infrastructure.Data.MappingProfiles;

public class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        CreateMap<Order, OrderEntity>()
            .ForMember(orderEntity => orderEntity.User, mappingOptions => mappingOptions.Ignore())
            .ReverseMap();
    }
}