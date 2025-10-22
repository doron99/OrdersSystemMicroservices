
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.ServiceContracts
{
    public interface IProductsService
    {
        Task<List<DataAccessLayer.Entities.Product?>> GetAll();
        Task<List<Product>> GetProductsByListOfSkus(List<string> skus);
        Task<List<StockTracking?>> GetStockTrackingList();
    }
}
