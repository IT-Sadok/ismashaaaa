using AutoMapper;
using MakeupClone.Application.DTOs.Delivery;
using MakeupClone.Domain.Entities;
using MakeupClone.Infrastructure.Data.Entities;

namespace MakeupClone.Infrastructure.Data.MappingProfiles;

public class DeliveryInformationMappingProfile : Profile
{
    public DeliveryInformationMappingProfile()
    {
        CreateMap<DeliveryInformation, DeliveryInformationEntity>().ReverseMap();
        CreateMap<DeliveryRequestDto, DeliveryInformationEntity>().ReverseMap();
    }
}