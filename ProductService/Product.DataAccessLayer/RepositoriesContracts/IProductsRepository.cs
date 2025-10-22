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
        Task<List<Product?>> GetProductsByListOfSkus(List<string> skus);
        Task<List<StockTracking?>> GetStockTrackingListAsync();

        Task WithdrawStockAsync(
             string sku,
             int stockBeforeAction,
             int quantity,
             string remarks,
             Guid? orderId); 
      
    }
}
