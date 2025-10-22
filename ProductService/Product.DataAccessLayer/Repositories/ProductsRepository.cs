using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DataAccessLayer.Data;
using DataAccessLayer.RepositoriesContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;

namespace DataAccessLayer.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly AppDbContext _ctx;
        private readonly ILogger<IProductsRepository> _logger;

        public ProductsRepository(AppDbContext ctx, ILogger<IProductsRepository> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }
        public Task<Product> AddAsync(Product order)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _ctx.Products.ToListAsync();
        }

        public Task<Product?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
        
        public async Task<Product?> UpdateAsync(Product product)
        {
            throw new NotImplementedException();
        }
        public async Task<List<Product?>> GetProductsByListOfSkus(List<string> skus)
        {
            // Check if there are products to look for
            if (skus == null || !skus.Any())
            {
                return new List<Product>();
            }

            // Query products from the database that are in the provided list
            return await _ctx.Products
                .Where(p => skus.Contains(p.Sku))
                .ToListAsync();
        }
        public async Task WithdrawStockAsync(
            string sku,
            int stockBeforeAction,
            int quantity,
            string remarks,
            Guid? orderId)
        {
            // Retrieve the stock item
            var item = await _ctx.Products
                .FirstOrDefaultAsync(i => i.Sku == sku);

            if (item == null)
            {
                throw new Exception($"Item with SKU {sku} not found.");
            }
            // Update the quantity
            item.Stock -= quantity;
            StockTracking stockTrackingItem = new StockTracking(sku, stockBeforeAction, -quantity, 0, DateTime.Now, "Withdraw", orderId);
            // Create a new stock movement record


            // Add stock movement to the context
            await _ctx.StockTracking.AddAsync(stockTrackingItem);

            // Save changes to the database
            await _ctx.SaveChangesAsync();
        }
        public async Task<List<StockTracking?>> GetStockTrackingListAsync()
        {
            return await _ctx.StockTracking.ToListAsync();
        }


    }
}
