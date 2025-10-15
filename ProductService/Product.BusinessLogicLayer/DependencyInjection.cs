using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BusinessLogicLayer.RabbitMQ;
using BusinessLogicLayer.ServiceContracts;
using BusinessLogicLayer.Services;

namespace BusinessLogicLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services, IConfiguration configuration)
        {

            //services.AddAutoMapper(typeof(OrderAddRequestToOrderMappingProfile).Assembly);
            //services.AddAutoMapper(typeof(OrderToOrderResponseMappingProfile).Assembly);

            //services.AddAutoMapper(typeof(ProductToProductResponseMappingProfile).Assembly);
            //services.AddAutoMapper(typeof(ProductUpdateRequestToProductMappingProfile).Assembly);


            services.AddScoped<IProductsService, ProductsService>();
            services.AddTransient<IRabbitMQPublisher, RabbitMQPublisher>();
            return services;
        }
    }
}
