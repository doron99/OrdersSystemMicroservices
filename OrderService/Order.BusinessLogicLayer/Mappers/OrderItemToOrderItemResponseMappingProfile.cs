using AutoMapper;
using BusinessLogicLayer.DTOs;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Mappers
{
    public class OrderItemToOrderItemResponseMappingProfile : Profile
    {
        public OrderItemToOrderItemResponseMappingProfile()
        {
            CreateMap<OrderItem, OrderItemReponse>()
                .ForMember(dest => dest.Sku, opt => opt.MapFrom(src => src.Sku))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
                .ForMember(dest => dest.OrderItemDesc, opt => opt.MapFrom(src => src.OrderItemDesc))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))

                .ForMember(dest => dest.OrderItemId, opt => opt.MapFrom(src => src.OrderItemId));


        }
    }
}
