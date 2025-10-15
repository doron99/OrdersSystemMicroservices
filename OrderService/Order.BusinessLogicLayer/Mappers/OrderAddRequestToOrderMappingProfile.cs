using AutoMapper;
using BusinessLogicLayer.DTOs;

namespace BusinessLogicLayer.Mappers
{
    public class OrderAddRequestToOrderMappingProfile : Profile
    {
        public OrderAddRequestToOrderMappingProfile()
        {
            CreateMap<OrderAddRequest, DataAccessLayer.Entities.Order>()
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

        }
    }
}
