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
            services.AddHostedService<RabbitMQInventoryCheckAndApproveReceivedHostedService>();
            
            //services.AddHostedService(provider =>
            //{
            //    // Create a scope here to resolve the hosted service
            //    var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
            //    using (var scope = scopeFactory.CreateScope())
            //    {
            //        return scope.ServiceProvider.GetRequiredService<RabbitMQInventoryCheckAndApproveReceivedHostedService>();
            //    }
            //});
            return services;
        }
    }
}
