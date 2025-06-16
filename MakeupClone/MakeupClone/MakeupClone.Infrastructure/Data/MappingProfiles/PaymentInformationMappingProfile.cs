using AutoMapper;
using MakeupClone.Domain.Entities;
using MakeupClone.Infrastructure.Data.Entities;

namespace MakeupClone.Infrastructure.Data.MappingProfiles;

public class PaymentInformationMappingProfile : Profile
{
    public PaymentInformationMappingProfile()
    {
        CreateMap<PaymentInformation, PaymentInformationEntity>().ReverseMap();
    }
}