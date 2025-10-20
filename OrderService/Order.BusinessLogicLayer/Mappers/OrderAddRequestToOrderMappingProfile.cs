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
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.CustomerName))
                .ForMember(dest => dest.OrderId, opt => opt.Ignore());

        }
    }
}
