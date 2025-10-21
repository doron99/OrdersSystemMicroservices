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

        public Task<Product?> UpdateAsync(Product product)
        {
            throw new NotImplementedException();
        }
        public async Task<List<Product?>> GetProductsByListOfSkus(List<string> skus)
        {
            // Check if there are products to look for
            //if (productIds == null || !productIds.Any())
            //{
            //    return new List<Product>();
            //}

            //// Extract ProductIds from the list of ProductToApprove
            //var productIds = productsToApprove.Select(p => p.ProductId).ToList();

            // Query products from the database that are in the provided list

            return await _ctx.Products
                .Where(p => skus.Contains(p.Sku))
                .ToListAsync();
        }

      
    }
}
