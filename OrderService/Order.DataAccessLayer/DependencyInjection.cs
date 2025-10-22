using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using DataAccessLayer.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DataAccessLayer
{
    public static class DependencyInjection
    {
        public static async Task<IServiceCollection> AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionStringTemplate = configuration.GetConnectionString("MSSQLDB")!;
            string connectionString = connectionStringTemplate
                .Replace("$MSSQL_HOST", Environment.GetEnvironmentVariable("MSSQL_HOST"))
                .Replace("$MSSQL_PORT", Environment.GetEnvironmentVariable("MSSQL_PORT"))
                .Replace("$MSSQL_DATABASE", Environment.GetEnvironmentVariable("MSSQL_DATABASE"))
                .Replace("$MSSQL_USER", Environment.GetEnvironmentVariable("MSSQL_USER"))
                .Replace("$MSSQL_PASSWORD", Environment.GetEnvironmentVariable("MSSQL_PASSWORD"));


            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));
            
            services.AddScoped<IOrdersRepository,OrdersRepository>();

            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var ordersRepo = scope.ServiceProvider.GetRequiredService<IOrdersRepository>();
                //ensure create db  
                context.Database.EnsureCreated();
                //seed first data
                if (!await context.Orders.AnyAsync())
                {
                    Order o = await ordersRepo.OrderAddAsync(new Order { CustomerId = Guid.Parse("ca474466-8c8d-44e7-941d-9e21d6729899"),CustomerName= "Skyline Builders" });
                    await ordersRepo.OrderAddItemAsync(o.OrderId, new OrderItem { OrderId = o.OrderId, OrderItemDesc="STEEL002", ProductId=Guid.Parse("ba6f446d-3af1-4826-9bc9-80839e136d39"),Sku = "STEEL002", UnitPrice =Convert.ToDecimal(25.5), Quantity = 4 });
                }


                //add example on app load
                //Order o = new Order
                //{
                //    CustomerId = "123"
                //};
                //await ordersRepo.OrderAddAsync(o);

            }

            return services;
        }
    }
}


