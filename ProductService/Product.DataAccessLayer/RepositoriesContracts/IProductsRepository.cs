using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DataAccessLayer.RepositoriesContracts
{
    public interface IProductsRepository
    {
        Task<List<Product>> GetAllAsync();

        Task<Product> AddAsync(Product order);
        Task<Product?> GetByIdAsync(Guid id);
        Task<Product?> UpdateAsync(Product product);
        Task<bool> DeleteAsync(Guid id);
    }
}
