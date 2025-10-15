using AutoMapper;
using BusinessLogicLayer.DTOs;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Mappers
{
    public class OrderItemAddRequestToOrderItemMappingProfile : Profile
    {
        public OrderItemAddRequestToOrderItemMappingProfile()
        {
            CreateMap<OrderItemAddRequest, OrderItem>()
                .ForMember(dest => dest.Sku, opt => opt.MapFrom(src => src.Sku))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.OrderItemDesc, opt => opt.MapFrom(src => src.OrderItemDesc))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.OrderItemId, opt => opt.Ignore());

        }
    }
}
