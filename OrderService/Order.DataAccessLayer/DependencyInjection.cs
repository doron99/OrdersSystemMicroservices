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
                    Order o = await ordersRepo.OrderAddAsync(new Order { CustomerId = Guid.NewGuid(),CustomerName="חברה לדוגמה 1" });
                    await ordersRepo.OrderAddItemAsync(o.OrderId, new OrderItem { OrderId = o.OrderId, OrderItemDesc="test12", ProductId=Guid.NewGuid(),Sku = "Item1", UnitPrice = 19, Quantity = 20 });
                    await ordersRepo.OrderAddItemAsync(o.OrderId, new OrderItem { OrderId = o.OrderId, OrderItemDesc = "test13", ProductId = Guid.NewGuid(), Sku = "Item2", UnitPrice = 1, Quantity = 5 });

                    Order o2 = await ordersRepo.OrderAddAsync(new Order { CustomerId = Guid.NewGuid(),CustomerName="חברה לדוגמה 2" });
                    await ordersRepo.OrderAddItemAsync(o2.OrderId, new OrderItem { OrderId = o2.OrderId, OrderItemDesc = "test12", ProductId = Guid.NewGuid(), Sku = "Item3", UnitPrice = 45, Quantity = 200 });
                    await ordersRepo.OrderAddItemAsync(o2.OrderId, new OrderItem { OrderId = o2.OrderId, OrderItemDesc = "test12", ProductId = Guid.NewGuid(), Sku = "Item4", UnitPrice = 30, Quantity = 55 });

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


