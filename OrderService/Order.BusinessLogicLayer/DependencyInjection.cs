using BusinessLogicLayer.Mappers;
using BusinessLogicLayer.RabbitMQ;
using BusinessLogicLayer.ServiceContracts;
using BusinessLogicLayer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace BusinessLogicLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddAutoMapper(typeof(OrderAddRequestToOrderMappingProfile).Assembly);
            services.AddAutoMapper(typeof(OrderToOrderResponseMappingProfile).Assembly);

            //services.AddAutoMapper(typeof(ProductToProductResponseMappingProfile).Assembly);
            //services.AddAutoMapper(typeof(ProductUpdateRequestToProductMappingProfile).Assembly);
            
            
            services.AddScoped<IOrdersService, OrdersService>();
            services.AddTransient<IRabbitMQPublisher, RabbitMQPublisher>();
            //services.AddTransient<IRabbitMQProductGetAllReceivedCunsumer, RabbitMQProductGetAllReceivedCunsumer>();
            services.AddHostedService<RabbitMQProductGetAllReceivedHostedService>();
            return services;
        }
    }
}
