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


            services.AddScoped<IProductsService, ProductsService>();
            services.AddTransient<IRabbitMQPublisher, RabbitMQPublisher>();
            services.AddHostedService<RabbitMQInventoryCheckAndApproveReceivedHostedService>();
            
            return services;
        }
    }
}
