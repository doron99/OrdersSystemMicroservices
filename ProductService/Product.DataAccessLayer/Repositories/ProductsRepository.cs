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
    }
}
